using N.Core.Common.Core;
using N.Core.Common.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using PCCEX = P.Core.Common.Extensions.CoreExtensions;
using System.IO;
using P.Core.Common.Utils;
using System.Linq;

namespace N.Core.Common.Extensions
{
   public static class CoreExtensions
   {
      public static void Merge<T>(this ObservableCollection<T> source, IEnumerable<T> collection)
      {
         PCCEX.Merge<T>(source, collection);
      }

      public static void Merge<T>(this ObservableCollection<T> source, IEnumerable<T> collection, bool ignoreDuplicates)
      {
         PCCEX.Merge<T>(source, collection, ignoreDuplicates);
      }

      public static bool IsNavigable(this PropertyInfo property)
      {
         return PCCEX.IsNavigable(property);
      }

      public static bool IsNavigable(this ObjectBase obj, string propertyName)
      {
         PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
         return propertyInfo.IsNavigable();
      }

      public static bool IsNavigable<T>(this ObjectBase obj, Expression<Func<T>> propertyExpression)
      {
         string propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
         PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
         return propertyInfo.IsNavigable();
      }

      public static bool IsBrowsable(this object obj, PropertyInfo property)
      {
         return PCCEX.IsBrowsable(obj, property);
      }

      public static PropertyInfo[] GetBrowsableProperties(this object obj)
      {
         string key = obj.GetType().ToString();

         if (!PCCEX.BrowsablePropertyInfos.ContainsKey(key))
         {
            List<PropertyInfo> propertyInfoList = new List<PropertyInfo>();
            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
               if ((property.PropertyType.IsSubclassOf(typeof(ObjectBase)) || property.PropertyType.GetInterface("IList") != null))
               {
                  // only add to list of the property is NOT marked with [NotNavigable]
                  if (IsBrowsable(obj, property))
                     propertyInfoList.Add(property);
               }
            }

            PCCEX.AddBrowsablePropertyInfo(key, propertyInfoList.ToArray());
         }

         return PCCEX.BrowsablePropertyInfos[key];
      }


      #region Assembly
      public static string GetConfigFilePath(this Assembly assembly)
      {
         string assemblyName = (assembly.GetName().Name + ".dll").ToLower();
         string assemblyPath = new Uri(assembly.CodeBase).LocalPath.ToLower().Replace(assemblyName, "");

         DirectoryInfo assemblyDirectory = new DirectoryInfo(assemblyPath);
         FileInfo[] configFiles = assemblyDirectory.GetFiles("*.config");

         // is there a custom assembly-specific config file?
         FileInfo configFile = configFiles.FirstOrDefault(f => f.Name == assembly.GetName().Name + ".config");

         // if not, get the .exe config file
         if (configFile == null)
         {
            foreach (FileInfo file in configFiles)
            {
               if (CoreUtilities.InList<string>(file.Name.ToLower(), "app.config", "web.config"))
               {
                  configFile = file;
                  break;
               }
            }
         }

         return configFile.FullName;
      }
      #endregion
   }
}

