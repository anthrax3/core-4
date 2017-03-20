using P.Core.Common.Contracts;
using P.Core.Common.Core;
using System.Runtime.Serialization;

namespace P.Core.Common.Meta
{
   public class MetaSetting : EntityBase, IIdentifiableEntity
   {
      public int MetaSettingID { get; set; }
      public string Environment { get; set; }
      public string Type { get; set; }
      public string Code { get; set; }
      public string Value { get; set; }
      public bool SortOrder { get; set; }
      public bool Enabled { get; set; }

      #region Members.IIdentifiableEntity
      [IgnoreDataMember]
      int IIdentifiableEntity.EntityID
      {
         get { return MetaSettingID; }
         set { MetaSettingID = value; }
      }
      #endregion
   }
}
