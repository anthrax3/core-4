using System;

namespace P.Core.Common.Contracts
{
   /// <summary>
   /// Represents a unit of work
   /// </summary>
   public interface IUnitOfWork : IDisposable
   {
      /// <summary>
      /// Commits the changes to the underlying data store
      /// </summary>
      /// <param name="resetAfterCommit">When true, all the previously retrieved objects should be cleared from the underlying model / cache.</param>
      void Commit(bool resetAfterCommit);

      /// <summary>
      /// Reverses all changes to entities in the model
      /// </summary>
      void Undo();
   }
}

