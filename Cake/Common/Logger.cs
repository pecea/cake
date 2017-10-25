using System;
using System.Linq;
using System.Runtime.CompilerServices;
using NLog;

namespace Common
{
    /// <summary>
    /// Provides tools for logging while executing methods from the modules or from the scrip itself.
    /// </summary>
    public static class Logger
    {
        private const string RoslynCallerMemberName = "<Initialize>";
        private const string ScriptLoggerName = "Script";
        /// <summary>
        /// Logs message of <see cref="LogLevel.Trace"/> level
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="loggerName">Logger name</param>
        public static void Trace(string message, [CallerMemberName] string loggerName = ScriptLoggerName) =>
            Log(LogLevel.Trace, message, loggerName);
        /// <summary>
        /// Logs message of <see cref="LogLevel.Debug"/> level
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="loggerName">Logger name</param>
        public static void Debug(string message, [CallerMemberName] string loggerName = ScriptLoggerName) =>
            Log(LogLevel.Debug, message, loggerName);
        /// <summary>
        /// Logs message of <see cref="LogLevel.Info"/> level
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="loggerName">Logger name</param>
        public static void Info(string message, [CallerMemberName] string loggerName = ScriptLoggerName) =>
            Log(LogLevel.Info, message, loggerName);
        /// <summary>
        /// Logs message of <see cref="LogLevel.Warn"/> level
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="loggerName">Logger name</param>
        public static void Warn(string message, [CallerMemberName] string loggerName = ScriptLoggerName) =>
            Log(LogLevel.Warn, message, loggerName);
        /// <summary>
        /// Logs message of <see cref="LogLevel.Error"/> level
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="loggerName">Logger name</param>
        public static void Error(string message, [CallerMemberName] string loggerName = ScriptLoggerName) =>
            Log(LogLevel.Error, message, loggerName);
        /// <summary>
        /// Logs message of <see cref="LogLevel.Fatal"/> level
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="loggerName">Logger name</param>
        public static void Fatal(string message, [CallerMemberName] string loggerName = ScriptLoggerName) =>
            Log(LogLevel.Fatal, message, loggerName);

        /// <summary>
        /// Logs a message using the loggers specified in the App.config.
        /// </summary>
        /// <param name="logLevel"><see cref="LogLevel"/> of the log.</param>
        /// <param name="message">Message to be logged.</param>
        /// <param name="loggerName">Name of the logger to be used.</param>
        public static void Log(LogLevel logLevel, string message, [CallerMemberName] string loggerName = ScriptLoggerName)
        {
            if (loggerName == RoslynCallerMemberName)
                loggerName = ScriptLoggerName;

            switch (logLevel)
            {
                case LogLevel.Trace:
                    LogManager.GetLogger(loggerName).Trace(message);
                    break;
                case LogLevel.Debug:
                    LogManager.GetLogger(loggerName).Debug(message);
                    break;
                case LogLevel.Info:
                    LogManager.GetLogger(loggerName).Info(message);
                    break;
                case LogLevel.Warn:
                    LogManager.GetLogger(loggerName).Warn(message);
                    break;
                case LogLevel.Error:
                    LogManager.GetLogger(loggerName).Error(message);
                    break;
                case LogLevel.Fatal:
                    LogManager.GetLogger(loggerName).Fatal(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }

        /// <summary>
        /// Logs an exception and its inner exceptions using the loggers specified in the App.config.
        /// </summary>
        /// <param name="e">Exception to be logged.</param>
        /// <param name="logLevel"><see cref="LogLevel"/> of the log.</param>
        /// <param name="message">Message to be logged. There will be info appended to it about exception's type, source and message.</param>
        /// <param name="loggerName">Name of the logger to be used.</param>
        public static void LogException(LogLevel logLevel, Exception e, string message, bool includeStackTrace = true, [CallerMemberName] string loggerName = "Script")
        {
            string msg = $"{message}{GetExceptionLogMessage(e, includeStackTrace)}";

            if (e.InnerException != null)            
                msg += $"\n\nBase exception:{GetExceptionLogMessage(e.GetBaseException(), includeStackTrace)}";
            
            Log(logLevel, msg, loggerName);
        }

        private static string GetExceptionLogMessage(Exception e, bool includeStackTrace)
        {
            var result = $"\n\nMessage: {e.Message}\nType: {e.GetType()}.\n";

            if (includeStackTrace)
                result += $"Stack trace:\n{e.StackTrace}\n";

            return result;
        }

        /// <summary>
        /// Method for run-time reconfiguration of logging level
        /// </summary>
        /// <param name="logLevelName">Type of logging - application or script</param>
        /// <param name="targetName">NLog configuration target name</param>
        public static void Reconfigure(string logLevelName, string targetName)
        {
            NLog.LogLevel logLevel;
            try
            {
                logLevel = NLog.LogLevel.FromString(logLevelName);
            }
            catch (Exception e)
            {
                Log(LogLevel.Warn, $"Invalid log level argument was specified. Valid log levels are: Trace, Debug, Info, Warn, Error and Fatal. Exception: {e}.");
                return;
            }

            var rules = LogManager.Configuration.LoggingRules
                .Where(r => r.Targets.Any(t => string.Equals(t.Name, targetName, StringComparison.CurrentCultureIgnoreCase))).ToArray();

            if (!rules.Any())
            {
                Log(LogLevel.Warn, $"Couldn't find any rules with the {targetName} target.");
                return;
            }
            foreach (var rule in rules)
            {
                foreach (var level in NLog.LogLevel.AllLoggingLevels)
                {
                    if (level.Ordinal >= logLevel.Ordinal) rule.EnableLoggingForLevel(level);
                    else rule.DisableLoggingForLevel(level);
                }
            }

            LogManager.ReconfigExistingLoggers();
        }
        /// <summary>
        /// Method for logging start of any method in the application
        /// </summary>
        /// <param name="methodName">Logging place</param>
        public static void LogMethodStart([CallerMemberName] string methodName = ScriptLoggerName) => Log(LogLevel.Trace, "Method started.", methodName);
        /// <summary>
        /// Method for logging end of any method in the application
        /// </summary>
        /// <param name="methodName">Logging place</param>
        public static void LogMethodEnd([CallerMemberName] string methodName = ScriptLoggerName) => Log(LogLevel.Trace, "Method finished.", methodName);
    }
}
