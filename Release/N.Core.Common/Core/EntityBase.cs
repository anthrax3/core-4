using P.Core.Common.Contracts;
using System;
using System.Runtime.Serialization;
using PCCC = P.Core.Common.Core;

namespace N.Core.Common.Core
{
   [DataContract]
    public abstract class EntityBase : PCCC.EntityBase, IExtensibleDataObject
   {
      #region Members.IExtensibleDataObject

      /// <summary>
      /// oco: Used by serializer to store extra properties/data found during  
      ///      Account entity <-> business entity serialization or de-serialization.
      ///      Inherited by business entities.
      ///      Inherited by Account entities via ObjectBase.
      /// </summary>
      public ExtensionDataObject ExtensionData { get; set; }
      #endregion
   }
}
