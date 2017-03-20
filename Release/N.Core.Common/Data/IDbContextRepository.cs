using P.Core.Common.Contracts;
using System;
using System.Data.Entity;

namespace N.Core.Common.Data
{
   /// using this as the interface for DataRepositoryBase<T, U> will require
   /// referencing EntityFramework directly in Business projects that require
   /// data access. That's the only way to use the navigation properties--the entity
   /// will be returned with the DbContext still alive (ie. not using using(..)).
   /// It's something to think about doing (the entities all have the nav. properties
   /// and EF is using lazy loading), but only after further thought.

   public interface IDbContextRepository<T> : IDataRepository<T>, IDisposable
      where T : class, IIdentifiableEntity, new()
   {
      DbContext Context { get; }
      DbSet<T> EntitySet { get; }
      T Entity(T proxy);
   }
}
