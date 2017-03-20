using P.Core.Common.Contracts;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace P.Core.Client
{
   public abstract class ServiceAwareObject
   {
      public ServiceAwareObject(IServiceFactory serviceFactory)
      {
         _serviceFactory = serviceFactory;
      }

      protected IServiceFactory _serviceFactory;

      protected virtual void WithClient<T>(Action<T> codeToExecute) where T : IServiceContract
      {
         T proxy = _serviceFactory.CreateClient<T>();

         WithClient<T>(proxy, codeToExecute);
      }

      protected virtual void WithClient<T>(T proxy, Action<T> codeToExecute)
      {
         // execute service call using the proxy
         codeToExecute.Invoke(proxy);
         DisposeProxy((IDisposable)proxy);
      }

      protected virtual async Task WithClientAsync<T>(T proxy, Func<T, Task> codeToExecute)
      {
         await codeToExecute.Invoke(proxy);
         DisposeProxy((IDisposable)proxy);
      }

      void DisposeProxy(IDisposable proxy)
      {
         // dispose the proxy
         IDisposable disposableClient = proxy as IDisposable;
         if (disposableClient != null)
            disposableClient.Dispose();

         // dispose the proxy
         //try
         //{
         //   ICommunicationObject communicationObject = proxy as ICommunicationObject;
         //   if (communicationObject.State != CommunicationState.Faulted)
         //      communicationObject.Close();
         //}
         //finally
         //{
         //   ICommunicationObject communicationObject = proxy as ICommunicationObject;
         //   if (communicationObject.State != CommunicationState.Closed)
         //      communicationObject.Abort();
         //}
      }
   }
}
