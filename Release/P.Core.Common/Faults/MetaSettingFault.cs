using System.Runtime.Serialization;

namespace P.Core.Common.Faults
{
   [DataContract]
   public class MetaSettingFault
   {
      public MetaSettingFault(string message)
      {
         Message = message;
      }

      [DataMember]
      public string Message { get; set; }
   }
}
