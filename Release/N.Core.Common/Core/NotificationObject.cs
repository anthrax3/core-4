using N.Core.Common.Utils;
using System;
using System.Linq.Expressions;
using PCCC = P.Core.Common.Core;

namespace N.Core.Common.Core
{
   /// <summary>
   /// This is the abstract base class for any object that provides property change notifications.  
   /// </summary>
   public abstract class NotificationObject : PCCC.NotificationObject
   {
      protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
      {
         string propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
         OnPropertyChanged(propertyName);
      }
   }
}
