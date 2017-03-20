using FluentValidation;
using FluentValidation.Results;
using N.Core.Common.Exceptions;
using N.Core.Common.Extensions;
using N.Core.Common.Utils;
using P.Core.Common.Contracts;
using P.Core.Common.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace N.Core.Common.Core
{
   /// <summary>
   /// Provides change notification, validation, etc. for client-side entities.
   /// IDataErrorInfo is used for validation error notification to XAML clients
   /// </summary>
   public abstract class ObjectBase : NotificationObject, IDirtyCapable, IExtensibleDataObject, IDataErrorInfo
   {
      #region Properties

      protected bool _IsDirty = false;
      protected IValidator _Validator = null;
      protected IEnumerable<ValidationFailure> _ValidationErrors = null;
      #endregion
      //-----------------------------------------------------------------------------------------------------

      #region Constructors

      public ObjectBase()
      {
         _Validator = GetValidator();
         Validate();
      }
      #endregion
      //-----------------------------------------------------------------------------------------------------

      #region Accessors

      public static CompositionContainer Container { get; set; }

      [NotNavigable]
      public IEnumerable<ValidationFailure> ValidationErrors
      {
         get { return _ValidationErrors; }
         set { }
      }

      [NotNavigable]
      public virtual bool IsValid
      {
         get
         {
            if (_ValidationErrors != null && _ValidationErrors.Count() > 0)
               return false;
            else
               return true;
         }
      }
      #endregion
      //-----------------------------------------------------------------------------------------------------

      #region Methods
      /// <summary>
      /// Set the value of a target property from the passed-in propertyName and value object
      /// </summary>
      /// <param name="propertyName">Name of the target property</param>
      /// <param name="value">New value of the target property</param>
      public virtual void SetValue(string propertyName, object value)
      {
         PropertyInfo property = this.GetType().GetProperty(propertyName);

         if (property == null)
            throw new NotFoundException(string.Format("{0} does not contain a property definition for {0}.", this.GetType().ToString(), propertyName));

         if (!property.CanWrite)
            throw new InvalidOperationException(string.Format("Property {0} is a read-only property.", propertyName));

         Type type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

         object safeValue = (value == null) ? null : Convert.ChangeType(value, type);

         property.SetValue(this, safeValue, null);
      }

      //--// Validation methods
      protected virtual IValidator GetValidator()
      {
         return null;
      }

      public void Validate()
      {
         if (_Validator != null)
         {
            ValidationResult results = _Validator.Validate(this);
            _ValidationErrors = results.Errors;
         }
      }

      //--// Object graph
      protected void WalkObjectGraph(Func<ObjectBase, bool> snippetForObject,
                                    Action<IList> snippetForCollection,
                                    params string[] exemptProperties)
      {
         List<ObjectBase> visited = new List<ObjectBase>();
         Action<ObjectBase> walk = null;

         List<string> exemptions = new List<string>();
         if (exemptProperties != null)
            exemptions = exemptProperties.ToList();

         walk = (o) =>
         {
            if (o != null && !visited.Contains(o))
            {
               visited.Add(o);

               bool exitWalk = snippetForObject.Invoke(o);

               if (!exitWalk)
               {
                  PropertyInfo[] properties = o.GetBrowsableProperties();
                  foreach (PropertyInfo property in properties)
                  {
                     if (!exemptions.Contains(property.Name))
                     {
                        if (property.PropertyType.IsSubclassOf(typeof(ObjectBase)))
                        {
                           ObjectBase obj = (ObjectBase)(property.GetValue(o, null));
                           walk(obj);
                        }
                        else
                        {
                           IList coll = property.GetValue(o, null) as IList;
                           if (coll != null)
                           {
                              snippetForCollection.Invoke(coll);

                              foreach (object item in coll)
                              {
                                 if (item is ObjectBase)
                                    walk((ObjectBase)item);
                              }
                           }
                        }
                     }
                  }
               }
            }
         };

         walk(this);
      }
      #endregion
      //-----------------------------------------------------------------------------------------------------

      #region Members.Override

      protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
      {
         OnPropertyChanged(propertyName, true);
      }

      protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression, bool makeDirty)
      {
         string propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
         OnPropertyChanged(propertyName, makeDirty);
      }

      protected void OnPropertyChanged(string propertyName, bool makeDirty)
      {
         base.OnPropertyChanged(propertyName);

         if (makeDirty)
            IsDirty = true;

         Validate();
      }
      #endregion
      //-----------------------------------------------------------------------------------------------------

      #region Members.IDirtyCapable

      [NotNavigable]
      public virtual bool IsDirty
      {
         get { return _IsDirty; }
         protected set
         {
            _IsDirty = value;
            OnPropertyChanged("IsDirty", false);
         }
      }

      public virtual bool IsAnythingDirty()
      {
         bool isDirty = false;

         WalkObjectGraph(
         o =>
         {
            if (o.IsDirty)
            {
               isDirty = true;
               return true; // short circuit
            }
            else
               return false;
         }, coll => { });

         return isDirty;
      }

      public List<IDirtyCapable> GetDirtyObjects()
      {
         List<IDirtyCapable> dirtyObjects = new List<IDirtyCapable>();

         WalkObjectGraph(
         o =>
         {
            if (o.IsDirty)
               dirtyObjects.Add(o);
            return false;
         }, coll => { });

         return dirtyObjects;
      }

      public void CleanAll()
      {
         WalkObjectGraph(
         o =>
         {
            if (o.IsDirty)
               o.IsDirty = false;
            return false;
         }, coll => { });
      }

      #endregion
      //-----------------------------------------------------------------------------------------------------

      #region Members.IExtensibleDataObject

      public ExtensionDataObject ExtensionData { get; set; }

      #endregion
      //-----------------------------------------------------------------------------------------------------

      #region Members.IDataErrorInfo

      string IDataErrorInfo.Error
      {
         get { return string.Empty; }
      }

      string IDataErrorInfo.this[string columnName]
      {
         get
         {
            StringBuilder errors = new StringBuilder();

            if (_ValidationErrors != null && _ValidationErrors.Count() > 0)
            {
               foreach (ValidationFailure validationError in _ValidationErrors)
               {
                  if (validationError.PropertyName == columnName)
                     errors.AppendLine(validationError.ErrorMessage);
               }
            }

            return errors.ToString();
         }
      }
      #endregion
   }
}
