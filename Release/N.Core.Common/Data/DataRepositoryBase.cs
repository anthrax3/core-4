﻿using N.Core.Common.Utils;
using P.Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace N.Core.Common.Data
{
   /// <summary>
   /// Base class for repositories that read/write to DbContext abstractions
   /// </summary>
   /// <typeparam name="T">Entity that implements IIdentifiableEntity and types a DbSet of U</typeparam>
   /// <typeparam name="U">A DbContext object</typeparam>
   public abstract class DataRepositoryBase<T, U> : IDbContextRepository<T>
      where T : class, IIdentifiableEntity, new()
      where U : DbContext, new()
   {
      protected abstract T AddEntity(U entityContext, T entity);
      protected abstract T UpdateEntity(U entityContext, T entity);
      protected abstract IEnumerable<T> GetEntities(U entityContext);
      protected abstract T GetEntity(U entityContext, int id);

      //---------------------------------------------------------------------
      #region Members.IDataRepository

      public T Add(T entity)
      {
         using (U entityContext = new U())
         {
            T addedEntity = AddEntity(entityContext, entity);
            entityContext.SaveChanges();
            return addedEntity;
         }
      }

      public void Remove(T entity)
      {
         using (U entityContext = new U())
         {
            entityContext.Entry<T>(entity).State = EntityState.Deleted;
            entityContext.SaveChanges();
         }
      }

      public void Remove(int id)
      {
         using (U entityContext = new U())
         {
            T entity = GetEntity(entityContext, id);
            entityContext.Entry<T>(entity).State = EntityState.Deleted;
            entityContext.SaveChanges();
         }
      }

      public T Update(T entity)
      {
         using (U entityContext = new U())
         {
            T existingEntity = UpdateEntity(entityContext, entity);

            SimpleMapper.PropertyMap(entity, existingEntity);

            entityContext.SaveChanges();
            return existingEntity;
         }
      }

      public IEnumerable<T> Get()
      {
         using (U entityContext = new U())
            return (GetEntities(entityContext)).ToArray().ToList();
      }

      public T Get(int id)
      {
         using (U entityContext = new U())
            return GetEntity(entityContext, id);
      }
      #endregion

      //---------------------------------------------------------------------
      #region Members.IDbContextRepository

      U _entityContext = null;

      DbSet<T> IDbContextRepository<T>.EntitySet
      {
         get
         {
            if (_entityContext == null)
               _entityContext = new U();

            return _entityContext.Set<T>();
         }
      }

      DbContext IDbContextRepository<T>.Context
      {
         get
         {
            if (_entityContext == null)
               _entityContext = new U();
            return _entityContext;
         }
      }

      T IDbContextRepository<T>.Entity(T proxy)
      {
         bool proxyCreationEnabled = _entityContext.Configuration.ProxyCreationEnabled;

         try
         {
            _entityContext.Configuration.ProxyCreationEnabled = false;
            T entity = _entityContext.Entry(proxy).CurrentValues.ToObject() as T;

            return entity;
         }
         finally
         {
            _entityContext.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
         }
      }

      public void Dispose()
      {
         _entityContext.Dispose();
      }
      #endregion
   }
}

