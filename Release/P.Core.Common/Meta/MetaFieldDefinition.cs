using P.Core.Common.Contracts;
using P.Core.Common.Core;
using System.Runtime.Serialization;

namespace P.Core.Common.Meta
{
   public class MetaFieldDefinition : EntityBase, IIdentifiableEntity
   {
      public int MetaFieldDefinitionID { get; set; }
      public string TableName { get; set; }
      public string FieldName { get; set; }
      public string Caption { get; set; }
      public string LookupTable { get; set; }
      public string LookupDescriptionType { get; set; }
      public string LookupDescriptionFormat { get; set; }
      public string FormatType { get; set; }
      public string MaximumValue { get; set; }
      public string MaximumLength { get; set; }
      public string RegularExpression { get; set; }
      public bool Required { get; set; }
      public string ReadPermissionId { get; set; }
      public string EditPermissionId { get; set; }
      public bool Enabled { get; set; }

      #region Members.IIdentifiableEntity
      [IgnoreDataMember]
      int IIdentifiableEntity.EntityID
      {
         get { return MetaFieldDefinitionID; }
         set { MetaFieldDefinitionID = value; }
      }
      #endregion
   }
}
