using P.Core.Common.Extensions;
using System;
using System.Linq;
using System.Reflection;
using Xciles.PclValueInjecter;

namespace P.Core.Common.Utils
{
   public static class ValueInjector
   {
      public static object InjectWith(this object target, object source, bool caseSensitive = true)
      {
         if (caseSensitive)
            target.InjectFrom<NavigablesOnlyInjection>(source);
         else
            target.InjectFrom<IgnoreCaseInjection>(source);

         return target;
      }
   }

   #region Injection conventions
   public class NavigablesOnlyInjection : ConventionInjection
   {
      protected StringComparison _comparison = StringComparison.Ordinal;

      protected override bool Match(ConventionInfo c)
      {
         string sourcePropName = c.SourceProp.Name;
         string targetPropName = c.TargetProp.Name;


         PropertyInfo sourceProp = c.Source.Type.GetInfos().FirstOrDefault(p => p.Name == c.SourceProp.Name && p.IsNavigable());
         PropertyInfo targetProp = c.Target.Type.GetInfos().FirstOrDefault(p => p.Name == c.TargetProp.Name && p.IsNavigable());

         if (sourceProp == null || targetProp == null)
            return false;
         else
            return String.Compare(c.SourceProp.Name, c.TargetProp.Name, _comparison) == 0;
      }
   }

   public class IgnoreCaseInjection : NavigablesOnlyInjection
   {
      protected override bool Match(ConventionInfo c)
      {
         _comparison = StringComparison.OrdinalIgnoreCase;

         return base.Match(c);
      }
   }
   #endregion
}
