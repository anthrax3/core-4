using P.Core.Common.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace P.Core.Common.Extensions
{
   public static class CoreExtensions
   {
      #region ObservableCollection
      public static void Merge<T>(this ObservableCollection<T> source, IEnumerable<T> collection)
      {
         Merge<T>(source, collection, false);
      }

      public static void Merge<T>(this ObservableCollection<T> source, IEnumerable<T> collection, bool ignoreDuplicates)
      {
         if (collection != null)
         {
            foreach (T item in collection)
            {
               bool addItem = true;

               if (ignoreDuplicates)
                  addItem = !source.Contains(item);

               if (addItem)
                  source.Add(item);
            }
         }
      }
      #endregion

      #region PropertyInfo
      public static bool IsNavigable(this PropertyInfo property)
      {
         bool navigable = true;

         object[] attributes = property.GetCustomAttributes(typeof(NotNavigableAttribute), true).ToArray();
         if (attributes.Length > 0)
            navigable = false;

         return navigable;
      }

      public static ReadOnlyDictionary<string, bool> BrowsableProperties
      {
         get { return new ReadOnlyDictionary<string, bool>(_BrowsableProperties); }
      }
      public static void AddBrowsableProperty(string key, bool value)
      {
         _BrowsableProperties.Add(key, value);
      }

      public static ReadOnlyDictionary<string, PropertyInfo[]> BrowsablePropertyInfos
      {
         get { return new ReadOnlyDictionary<string, PropertyInfo[]>(_BrowsablePropertyInfos); }
      }
      public static void AddBrowsablePropertyInfo(string key, PropertyInfo[] propertyInfo)
      {
         _BrowsablePropertyInfos.Add(key, propertyInfo);
      }

      static Dictionary<string, bool> _BrowsableProperties = new Dictionary<string, bool>();
      static Dictionary<string, PropertyInfo[]> _BrowsablePropertyInfos = new Dictionary<string, PropertyInfo[]>();

      public static bool IsBrowsable(this object obj, PropertyInfo property)
      {
         string key = string.Format("{0}.{1}", obj.GetType(), property.Name);

         if (!_BrowsableProperties.ContainsKey(key))
         {
            bool browsable = property.IsNavigable();
            _BrowsableProperties.Add(key, browsable);
         }

         return _BrowsableProperties[key];
      }
      #endregion
   }
}
