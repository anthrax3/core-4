using P.Core.Common.Contracts;

namespace N.Core.Business.Contracts
{
   public interface IBusinessEngine
   {
      IDataRepositoryFactory _DataRepositoryFactory { get; set; }
   }
}
