using System.Runtime.Serialization;

namespace P.Core.Common.Faults
{
   [DataContract]
   public class NotFoundFault
   {
      public NotFoundFault(string message)
      {
         Message = message;
      }

      [DataMember]
      public string Message { get; set; }
   }
}
