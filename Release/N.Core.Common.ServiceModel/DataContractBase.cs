using System.Runtime.Serialization;
using PCCS = P.Core.Common.ServiceModel;

namespace N.Core.Common.ServiceModel
{
   [DataContract]
   public class DataContractBase : PCCS.DataContractBase, IExtensibleDataObject
   {
      #region Members.Interface.IExtensibleDataObject

      /// <summary>
      /// Enables version tolerance during serialization by caching unmatched members.
      /// The cached member values are appended to the return prior to transmission.
      /// </summary>
      public ExtensionDataObject ExtensionData { get; set; }
      #endregion
   }
}
