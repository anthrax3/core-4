using P.Core.Common.Contracts;
using P.Core.Common.Faults;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace N.Core.Common.Data
{
   public abstract class DbContextBase : DbContext
   {
      public Type _seedEntityTypeConfigurationType;

      public DbContextBase(string nameOrConnectionString, Type seedEntityTypeConfigurationType)
         : base(nameOrConnectionString)
      {
         _seedEntityTypeConfigurationType = seedEntityTypeConfigurationType;
      }

      protected override void OnModelCreating(DbModelBuilder modelBuilder)
      {
         modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); // singular table names

         // ignore properties associated with the following inheriteds and interfaces
         modelBuilder.Ignore<PropertyChangedEventHandler>();
         modelBuilder.Ignore<ExtensionDataObject>();
         modelBuilder.Ignore<IIdentifiableEntity>();

         // add entity configurations
         var typesToRegister = Assembly.GetAssembly(_seedEntityTypeConfigurationType).GetTypes()
             .Where(type => !String.IsNullOrEmpty(type.Namespace))
             .Where(type => type.BaseType != null && type.BaseType.IsGenericType
               && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

         foreach (var type in typesToRegister)
         {
            dynamic configurationInstance = Activator.CreateInstance(type);
            modelBuilder.Configurations.Add(configurationInstance);
         }

         base.OnModelCreating(modelBuilder);
      }

      public override int SaveChanges()
      {
         try  // add the entity DateCreated if a new addition or update the DateModified if an update
         {
            var modifiedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);

            foreach (DbEntityEntry item in modifiedEntities)
            {
               var changedOrAddedItem = item.Entity as IDateTracking;
               if (changedOrAddedItem != null)
               {
                  if (item.State == EntityState.Added)
                  {
                     changedOrAddedItem.DateCreated = DateTime.UtcNow;
                  }
                  else
                  {
                     changedOrAddedItem.DateCreated = (DateTime)item.OriginalValues["DateCreated"];
                  }
                  changedOrAddedItem.DateModified = DateTime.UtcNow;
               }
            }

            return base.SaveChanges();  // third, save to the database
         }
         catch (DbEntityValidationException entityException)
         {
            var errors = entityException.EntityValidationErrors;
            var result = new StringBuilder();
            var allErrors = new List<ValidationResult>();

            foreach (var error in errors)
            {
               foreach (var validationError in error.ValidationErrors)
               {
                  result.AppendFormat("\r\n  Entity of type {0} has validation error \"{1}\" for property {2}.\r\n", error.Entry.Entity.GetType().ToString(), validationError.ErrorMessage, validationError.PropertyName);
                  var domainEntity = error.Entry.Entity as IIdentifiableEntity;
                  if (domainEntity != null)
                  {
                     result.Append((domainEntity.EntityID == 0)
                        ? "  This entity was created in this session.\r\n"
                        : string.Format("  The id of the entity is {0}.\r\n", domainEntity.EntityID));
                  }
                  allErrors.Add(new ValidationResult(validationError.ErrorMessage, new[] { validationError.PropertyName }));
               }
            }
            EntityValidationFault fault = new EntityValidationFault(result.ToString(), allErrors);
            throw new FaultException<EntityValidationFault>(fault, fault.Message);
         }
         catch (DbUpdateException ex)
         {
            UpdateException updateException = (UpdateException)ex.InnerException;
            SqlException sqlException = (SqlException)updateException.InnerException;
            var result = new StringBuilder
               ("The following errors during the context.SaveChanges() operation: \r\n");

            foreach (SqlError error in sqlException.Errors)
            {
               string errString = error.Message;
               result.Append("\r\n" + error.Message);
            }
            throw new FaultException(result.ToString());
         }
      }
   }
}
