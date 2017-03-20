using System;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace N.Core.Common.ServiceModel
{
   public class ApplyProxyDataContractResolverAttribute : Attribute, IServiceBehavior, IOperationBehavior
   {
      public ApplyProxyDataContractResolverAttribute()
      {
      }

      #region IServiceBehavior members
      /// <summary>
      /// Invoked during all service operations to modify or extend operation behavior
      /// </summary>
      /// <param name="description"></param>
      /// <param name="proxy"></param>
      public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
      {
      }

      /// <summary>
      /// Invoked during all service operations to modify or extend operation behavior
      /// </summary>
      /// <param name="description"></param>
      /// <param name="proxy"></param>
      public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
      {
         foreach (var endPoint in serviceDescription.Endpoints)
         {
            foreach (var operation in endPoint.Contract.Operations)
            {
               var behavior = operation.OperationBehaviors.OfType<DataContractSerializerOperationBehavior>().SingleOrDefault();
               behavior.DataContractResolver = new ProxyDataContractResolver();
            }
         }
      }

      /// <summary>
      /// Invoked during all service operations to modify or extend operation behavior
      /// </summary>
      /// <param name="description"></param>
      /// <param name="proxy"></param>
      public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
      {
         // Do validation?
      }
      #endregion


      #region IOperationBehavior members
      /// <summary>
      /// Invoked during a client-side or service-side operation to modify or extend operation behavior
      /// </summary>
      /// <param name="description"></param>
      /// <param name="proxy"></param>
      public void AddBindingParameters(OperationDescription description, BindingParameterCollection parameters)
      {
      }

      /// <summary>
      /// Invoked during a client-side operation to modify or extend operation behavior
      /// </summary>
      /// <param name="description"></param>
      /// <param name="proxy"></param>
      public void ApplyClientBehavior(OperationDescription description, ClientOperation proxy)
      {
         DataContractSerializerOperationBehavior behavior = description.Behaviors.Find<DataContractSerializerOperationBehavior>();
         behavior.DataContractResolver = new ProxyDataContractResolver();
      }

      /// <summary>
      /// Invoked during a service-side operation to modify or extend operation behavior
      /// </summary>
      /// <param name="description"></param>
      /// <param name="proxy"></param>
      public void ApplyDispatchBehavior(OperationDescription description, DispatchOperation dispatch)
      {
         DataContractSerializerOperationBehavior behavior = description.Behaviors.Find<DataContractSerializerOperationBehavior>();
         behavior.DataContractResolver = new ProxyDataContractResolver();
      }

      /// <summary>
      /// Invoked during a client-side or service-side operation to modify or extend operation behavior
      /// </summary>
      /// <param name="description"></param>
      /// <param name="proxy"></param>
      public void Validate(OperationDescription description)
      {
         // Do validation?
      }
      #endregion
   }
}
