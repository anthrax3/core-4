using System.Security.Principal;
using System.ServiceModel;
using System.Threading;
using PCCS = P.Core.Common.ServiceModel;

namespace N.Core.Common.ServiceModel
{
   public abstract class UserClientBase<T> : PCCS.UserClientBase<T> where T : class 
   {
      public UserClientBase()
         : base(System.Security.Principal.WindowsIdentity.GetCurrent().Name)
      {
      }
   }
}
