using P.Core.Common.Contracts;
using System;
using System.Runtime.Serialization;

namespace P.Core.Common.Core
{
   [DataContract]
    public abstract class EntityBase : IDateTracking
   {
      #region Members.IDateTracking

      /// <summary>
      /// Gets or sets the date and time the object was created.
      /// </summary>
      public DateTime DateCreated { get; set; }

      /// <summary>
      /// Gets or sets the date and time the object was last modified.
      /// </summary>
      public DateTime DateModified { get; set; }
      #endregion
   }
}
