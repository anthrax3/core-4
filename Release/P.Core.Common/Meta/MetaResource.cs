using P.Core.Common.Contracts;
using P.Core.Common.Core;
using System.Runtime.Serialization;

namespace P.Core.Common.Meta
{
   public class MetaResource : EntityBase, IIdentifiableEntity
   {
      public int MetaResourceID { get; set; }
      public string Set { get; set; }
      public string Type { get; set; }
      public string Key { get; set; }    
      public string Value { get; set; }
      public string Category { get; set; }
      public string CultureCode { get; set; }
      public bool Translated { get; set; }
      public bool Enabled { get; set; }

      #region Members.IIdentifiableEntity
      [IgnoreDataMember]
      int IIdentifiableEntity.EntityID
      {
         get { return MetaResourceID; }
         set { MetaResourceID = value; }
      }
      #endregion
   }
}
