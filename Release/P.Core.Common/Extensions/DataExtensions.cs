using System.Collections.Generic;
using System.Linq;

namespace P.Core.Common.Extensions
{
   public static class DataExtensions
   {
      /// <summary>
      /// Converts an IQueryable to an IEnumerable via ToList(). 
      /// If the IQueryable is the result-set from an LINQ-to-EF query, the object graph of each object will be fully loaded.
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="query"></param>
      /// <returns>A List object. </returns>
      public static IEnumerable<T> ToFullyLoaded<T>(this IQueryable<T> query)
      {
         return query.ToArray().ToList();
      }
   }
}
