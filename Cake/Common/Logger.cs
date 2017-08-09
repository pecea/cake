namespace Common
{
    using NLog;
    using NLog.Config;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Provides tools for logging while executing methods from the modules or from the scrip itself.
    /// </summary>
    public static class Logger
    {
        private const string RoslynCallerMemberName = "<Initialize>";
        private const string ScriptLoggerName = "Script";

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
        public static void LogException(LogLevel logLevel, Exception e, string message, [CallerMemberName] string loggerName = "Script")
        {
            Log(logLevel, $"{message}\nType: {e.GetType()}.\nSource: {e.Source}.\nMessage: {e.Message}\nStack trace: {e.StackTrace}", loggerName);

            if (e.InnerException != null)
            {
                var baseException = e.GetBaseException();
                LogException(logLevel, baseException, "Base exception:");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logLevelName"></param>
        /// <param name="targetName"></param>
        public static void Reconfigure(string logLevelName, string targetName)
        {
            Log(LogLevel.Trace, "Method started");
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

            IEnumerable<LoggingRule> rules = LogManager.Configuration.LoggingRules
                .Where(r => r.Targets.Any(t => t.Name.ToLower() == targetName.ToLower()));

            if (!rules.Any())
            {
                Log(LogLevel.Warn, $"Couldn't find any rules with the {targetName} target.");
                return;
            }

            foreach (LoggingRule rule in rules)
            {
                foreach (var level in NLog.LogLevel.AllLoggingLevels)
                {
                    if (level.Ordinal >= logLevel.Ordinal) rule.EnableLoggingForLevel(level);
                    else rule.DisableLoggingForLevel(level);
                }
            }

            LogManager.ReconfigExistingLoggers();
            Log(LogLevel.Trace, "Method finished");
        }
    }
}
