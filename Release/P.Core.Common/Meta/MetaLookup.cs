using P.Core.Common.Contracts;
using P.Core.Common.Core;
using System.Runtime.Serialization;

namespace P.Core.Common.Meta
{
   public class MetaLookup : EntityBase, IIdentifiableEntity
   {
      public int MetaLookupID { get; set; }
      public string Type { get; set; }
      public string Code { get; set; }
      public string ShortDesc { get; set; }
      public string LongDesc { get; set; }
      public int SortOrder { get; set; }
      public bool Enabled { get; set; }

      #region Members.IIdentifiableEntity
      [IgnoreDataMember]
      int IIdentifiableEntity.EntityID
      {
         get { return MetaLookupID; }
         set { MetaLookupID = value; }
      }
      #endregion
   }
}
