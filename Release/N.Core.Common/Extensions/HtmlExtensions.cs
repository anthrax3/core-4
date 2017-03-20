using System;
using System.Web.Caching;
using System.Web.Mvc;

namespace N.Core.Common.Extensions
{
   public static class HtmlExtensions
   {
      public static MvcHtmlString IncludeVersionedJS(this HtmlHelper helper, string fileName)
      {
         string version = GetVersion(helper, fileName);
         return MvcHtmlString.Create("<script src='" + fileName + version + "' type='text/javascript'></script>");
      }

      public static string GetVersion(this HtmlHelper helper, string fileName)
      {
         var context = helper.ViewContext.RequestContext.HttpContext;

         if (context.Cache[fileName] == null)
         {
            var physicalPath = context.Server.MapPath(fileName);
            var version = "?v=" + new System.IO.FileInfo(physicalPath).LastWriteTime.ToString("yyyyMMddhhmmss");
            context.Cache.Add(physicalPath, version, null, DateTime.Now.AddMinutes(1), TimeSpan.Zero,
                  CacheItemPriority.Normal, null);
            context.Cache[fileName] = version;
            return version;
         }
         else
         {
            return context.Cache[fileName] as string;
         }
      }
   }
}
