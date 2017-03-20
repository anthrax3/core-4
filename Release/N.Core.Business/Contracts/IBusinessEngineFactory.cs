namespace N.Core.Business.Contracts
{
   public interface IBusinessEngineFactory
   {
      T GetBusinessEngine<T>() where T : IBusinessEngine;
   }
}
