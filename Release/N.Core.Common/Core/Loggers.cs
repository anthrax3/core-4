using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using N.Core.Common.Contracts;
using N.Core.Common.Extensions;
using P.Core.Common.Contracts;
using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace N.Core.Common.Core
{
   /// <summary>
   /// MicrosoftPracticesLogger is a logging service that uses the Microsoft Enterprise Patterns and Practices library to provide logging support.
   /// </summary>
   [Export(typeof(IEventLogger))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class MicrosoftPracticesLogger : ConsoleLogger, IEventLogger
   {
      private static readonly LogWriterFactory _logWriterFactory;
      private readonly LogWriter _logWriter;

      /// <summary>
      /// Initializes the MicrosoftPracticesLogger class.
      /// </summary>
      static MicrosoftPracticesLogger()
      {
         string configFilePath = typeof(MicrosoftPracticesLogger).Assembly.GetConfigFilePath();

         if (File.Exists(configFilePath) == false)
         {
            throw new InvalidOperationException(String.Format("Configuration file could not be found at '{0}'", configFilePath));
         }

         FileConfigurationSource configSource = new FileConfigurationSource(configFilePath);

         _logWriterFactory = new LogWriterFactory(configSource);
      }

      /// <summary>
      /// Initializes a new instance of the MicrosoftPracticesLogger class.
      /// </summary>
      public MicrosoftPracticesLogger() 
         : base(false)
      {
         this._logWriter = _logWriterFactory.Create();
      }

      #region Members.ILogger
      public override string LogException(Exception ex, string message)
      {
         return LogException(ex, message, null);
      }

      [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1604:ElementDocumentationMustHaveSummary", Justification = "InheritDoc")]
      [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1611:ElementParametersMustBeDocumented", Justification = "InheritDoc")]
      public override string LogException(Exception ex, string messageFormat, params object[] parameters)
      {
         string message = base.LogException(ex, messageFormat, parameters);

         return WriteLogEntry(TraceEventType.Error, message);
      }

      public override string LogMessage(string message)
      {
         return LogMessage(message, null);
      }

      public override string LogMessage(string messageFormat, params object[] parameters)
      {
         string message = base.LogMessage(BuildMessage(messageFormat, parameters).ToString());

         return WriteLogEntry(TraceEventType.Information, message);
      }
      #endregion

      #region Members.IEventLogger
      public string LogMessage(TraceEventType severity, string message)
      {
         return LogMessage(severity, message, null);
      }

      public string LogMessage(TraceEventType severity, string messageFormat, params object[] parameters)
      {
         string message = base.LogMessage(BuildMessage(messageFormat, parameters).ToString());

         return WriteLogEntry(severity, message);
      }

      public string LogException(TraceEventType severity, Exception ex, string message)
      {
         return LogException(severity, ex, message, null);
      }

      public string LogException(TraceEventType severity, Exception ex, string messageFormat, params object[] parameters)
      {
         string message = base.LogException(ex, messageFormat, parameters);

         return WriteLogEntry(severity, message);
      }
      #endregion

      #region Utilities
      protected virtual LogEntry CreateLogEntry(TraceEventType severity, string message)
      {
         LogEntry logEntry = new LogEntry
         {
            Message = message,
            Severity = severity
         };

         return logEntry;
      }

      protected virtual string WriteLogEntry(TraceEventType severity, string message)
      {
         LogEntry logEntry = CreateLogEntry(severity, message);

         if (severity != 0)
            this._logWriter.Write(logEntry);

         return logEntry.Message;
      }
      #endregion
   }

   /// <summary>
   /// ConsoleLogger is a logging service that provides logging support via writes to the console.
   /// </summary>
   public class ConsoleLogger : ILogger
   {
      public ConsoleLogger()
      {
         LogToConsole = true;
      }

      /// <summary>
      /// Initializes the MicrosoftPracticesLogger class.
      /// </summary>
      /// <param name="logToConsole"></param>
      public ConsoleLogger(bool logToConsole)
      {
         LogToConsole = LogToConsole;
      }

      public bool LogToConsole { get; set; }

      public virtual string LogMessage(string message)
      {
         if (LogToConsole) 
            Console.WriteLine("\n" + message.Replace(Environment.NewLine, ""));

         return message;
      }

      public virtual string LogMessage(string messageFormat, params object[] parameters)
      {
         return LogMessage(BuildMessage(messageFormat, parameters).ToString());
      }

      public virtual string LogException(Exception ex, string message)
      {
         return LogException(ex, message, null);
      }

      [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1604:ElementDocumentationMustHaveSummary", Justification = "InheritDoc")]
      [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1611:ElementParametersMustBeDocumented", Justification = "InheritDoc")]
      public virtual string LogException(Exception ex, string messageFormat, params object[] parameters)
      {
         if (ex == null)
         {
            throw new ArgumentNullException("ex");
         }

         if (messageFormat == null)
         {
            throw new ArgumentNullException("messageFormat");
         }

         string message = null;

         StringBuilder messageBuilder = BuildMessage(messageFormat, parameters);

         messageBuilder.AppendLine(ex.Message);
         messageBuilder.AppendLine(ex.StackTrace);

         if (ex.InnerException != null)
         {
            Exception root = ex;

            while (root.InnerException != null)
            {
               root = root.InnerException;
            }

            messageBuilder.AppendLine("Caused by:");
            messageBuilder.AppendLine(root.StackTrace);
         }

         message = LogMessage(messageBuilder.ToString());

         return message;
      }

      protected virtual StringBuilder BuildMessage(string messageFormat, object[] parameters)
      {
         StringBuilder messageBuilder = new StringBuilder();

         if (parameters == null)
            messageBuilder.AppendLine(messageFormat);
         else
            messageBuilder.AppendFormat(messageFormat, parameters).AppendLine();

         return messageBuilder;
      }
   }
}
