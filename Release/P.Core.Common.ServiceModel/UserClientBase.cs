using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace P.Core.Common.ServiceModel
{
   public class UserClientBase<T> : ClientBase<T> where T : class
   {
      public UserClientBase(string userName)
      {
         // add current user to a message header
         MessageHeader<string> header = new MessageHeader<string>(userName);

         // wrap the operation channel with a scope
         OperationContextScope contextScope = new OperationContextScope(InnerChannel);
 
         // add the header to the scope's headers collection 
         OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("String", "System"));
      }
   }
}
