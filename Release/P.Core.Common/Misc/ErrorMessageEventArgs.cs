using System;

namespace P.Core.Common.Misc
{
   public class ErrorMessageEventArgs : EventArgs
   {
      public ErrorMessageEventArgs(string errorMessage)
      {
         ErrorMessage = errorMessage;
      }

      public string ErrorMessage { get; set; }
   }
}

