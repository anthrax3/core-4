using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N.Core.Common.Misc
{
   [TestClass]
   public abstract class UnitTestBase
   {
      protected bool classInitialized;
      protected short testCount;

      //UnitTestBase()
      //{
      //   ClassInitialize();
      //}
      //~UnitTestBase()
      //{
      //}

      ///// </summary>
      //[ClassInitialize()]
      //public virtual void ClassInitialize()
      //{
      //   ClassCleanup();
      //}

      ///// <summary>
      ///// Runs code to clean up state after all tests are completed.
      ///// </summary>
      //[ClassCleanup()]
      //public static void ClassCleanup()
      //{
      //}

      /// <summary>
      /// Runs code to initialize state before invoking a test.
      /// </summary>
      [TestInitialize()]
      public virtual void TestInitialize()
      {
      }

      /// <summary>
      /// Runs code to clean up state after a test.
      /// </summary>
      [TestCleanup()]
      public virtual void TestCleanup()
      {
      }
   }
}
