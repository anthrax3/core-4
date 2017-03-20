using System;
using System.ComponentModel;

namespace P.Core.Common.Contracts
{
   public interface ILogger
   {
      bool LogToConsole { get; set; }

      string LogMessage(string message);
      string LogMessage(string messageFormat, params object[] parameters);
      string LogException(Exception ex, string message);
      string LogException(Exception ex, string messageFormat, params object[] parameters);
   }
}
