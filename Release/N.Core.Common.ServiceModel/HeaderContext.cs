using System.Runtime.Serialization;
using System.ServiceModel;
using PCCS = P.Core.Common.ServiceModel;

namespace N.Core.Common.ServiceModel
{
   [DataContract]
   public class HeaderContext<T> : PCCS.HeaderContext<T>
   {
      public HeaderContext(T value) : base(value)
      {

      }
   }
}
