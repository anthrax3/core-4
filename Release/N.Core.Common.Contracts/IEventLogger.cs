using P.Core.Common.Contracts;
using System;
using System.Diagnostics;

namespace N.Core.Common.Contracts
{
   public interface IEventLogger : ILogger
   {
      string LogMessage(TraceEventType severity, string message);
      string LogMessage(TraceEventType severity, string messageFormat, params object[] parameters);
      string LogException(TraceEventType severity, Exception ex, string message);
      string LogException(TraceEventType severity, Exception ex, string messageFormat, params object[] parameters);
   }
}
