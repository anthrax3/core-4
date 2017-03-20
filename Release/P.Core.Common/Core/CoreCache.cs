using P.Core.Common.Contracts;
using P.Core.Common.Meta;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace P.Core.Common.Core
{
   public class CoreCache : IDataCache, IDisposable
   {
      #region declarations
      static IEnumerable<MetaLookup> _lookups;
      static IEnumerable<MetaResource> _resources;
      static IEnumerable<MetaFieldDefinition> _fieldDefinitions;
      static IEnumerable<MetaSetting> _settings;
      #endregion

      #region properties
      public static IEnumerable<MetaLookup> Lookups { get { return _lookups; } }
      public static IEnumerable<MetaResource> Resources { get { return _resources; } }
      public static IEnumerable<MetaFieldDefinition> FieldDefinitions { get { return _fieldDefinitions; } }
      public static IEnumerable<MetaSetting> Settings { get { return _settings; } }
      #endregion

      public CoreCache()
      {
         Initialize();
      }

      public virtual void Initialize() 
      {
         if (_lookups == null) _lookups = new Collection<MetaLookup>();
         if (_resources == null) _resources = new Collection<MetaResource>();
         if (_fieldDefinitions == null) _fieldDefinitions = new Collection<MetaFieldDefinition>();
         if (_settings == null) _settings = new Collection<MetaSetting>();
      }

      #region Members.IDataCache
      public virtual int Count
      {
         get
         {
            int count = 0;
            count += _lookups.Count() + _resources.Count() + _fieldDefinitions.Count() + _settings.Count();
            return count;
         }
      }

      public virtual void AddItem(string key, object value, IEnumerable<string> dependentEntitySets, TimeSpan slidingExpiration, DateTimeOffset absoluteExpiration)
      {
         throw new NotImplementedException();
      }

      public virtual bool GetItem(string key, out object value)
      {
         throw new NotImplementedException();
      }

      public virtual void RemoveItem(string key)
      {
         throw new NotImplementedException();
      }

      public virtual void RemoveSets(IEnumerable<string> entitySets)
      {
         throw new NotImplementedException();
      }

      public virtual void Purge()
      {
         _lookups = new Collection<MetaLookup>();
         _resources = new Collection<MetaResource>();
         _fieldDefinitions = new Collection<MetaFieldDefinition>();
         _settings = new Collection<MetaSetting>();
      }
      #endregion

      public virtual void Dispose()
      {
         _lookups = null;
         _resources = null;
         _fieldDefinitions = null;
         _settings = null;
      }
   }
}
