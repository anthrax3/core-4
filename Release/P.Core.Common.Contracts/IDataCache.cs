using System;
using System.Collections.Generic;

namespace P.Core.Common.Contracts
{
   public interface IDataCache
   {
      int Count { get; }

      void AddItem(string key, object value, IEnumerable<string> dependentEntitySets, TimeSpan slidingExpiration, DateTimeOffset absoluteExpiration);
      bool GetItem(string key, out object value);
      void RemoveItem(string key);
      void RemoveSets(IEnumerable<string> entitySets);
      void Purge();
   }
}
