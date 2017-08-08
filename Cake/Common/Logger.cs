namespace Common
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    using NLog;

    /// <summary>
    /// Provides tools for logging while executing methods from the modules or from the scrip itself.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Logs a message using the loggers specified in the App.config.
        /// </summary>
        /// <param name="logLevel"><see cref="LogLevel"/> of the log.</param>
        /// <param name="message">Message to be logged.</param>
        /// <param name="loggerName">Name of the logger to be used.</param>
        public static void Log(LogLevel logLevel, string message, [CallerMemberName] string loggerName = "Script")
        {
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
        /// <seealso cref="LogInnerException"/>
        /// </summary>
        /// <param name="e">Exception to be logged.</param>
        /// <param name="logLevel"><see cref="LogLevel"/> of the log.</param>
        /// <param name="message">Message to be logged. There will be info appended to it about exception's type, source and message.</param>
        /// <param name="loggerName">Name of the logger to be used.</param>
        public static void LogException(LogLevel logLevel, Exception e, string message, [CallerMemberName] string loggerName = "Script")
        {
            Log(logLevel,
                $"{message}\n ExceptionType: {e.GetType()}. Exception source: {e.Source}.\n Exception message: {e.Message}\n", loggerName);
            LogInnerException(logLevel, e, loggerName);
        }

        /// <summary>
        /// Logs an inner exception with a predefined message using the loggers specified in the App.config.
        /// </summary>
        /// <param name="logLevel"><see cref="LogLevel"/> of the log.</param>
        /// <param name="e">Exception to be logged.</param>
        /// <param name="loggerName">Name of the logger to be used.</param>
        private static void LogInnerException(LogLevel logLevel, Exception e, string loggerName)
        {
            for (e = e.InnerException; e != null; e = e.InnerException)
                Log(logLevel, $"Inner exception source: {e.Source}. Inner exception message: {e.Message}", loggerName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logLevelName"></param>
        public static void Reconfigure(string logLevelName)
        {
            Log(LogLevel.Trace, "Reconfigure NLog method started");
            NLog.LogLevel logLevel;
            try
            {
                logLevel = NLog.LogLevel.FromString(logLevelName);
            }
            catch (Exception e)
            {
                Log(LogLevel.Warn, $"Invalid log level argument was specified. Valid log levels are: Trace, Debug, Info, Warn, Error and Fatal. Exception: {e}");
                return;
            }

            var logLevels = new List<NLog.LogLevel>
                                {
                                    NLog.LogLevel.Trace,
                                    NLog.LogLevel.Debug,
                                    NLog.LogLevel.Info,
                                    NLog.LogLevel.Warn,
                                    NLog.LogLevel.Error,
                                    NLog.LogLevel.Fatal
                                };

            foreach (var loggingRule in LogManager.Configuration.LoggingRules)
            {
                foreach (var level in logLevels)
                {
                    if (level.Ordinal >= logLevel.Ordinal) loggingRule.EnableLoggingForLevel(level);
                    else loggingRule.DisableLoggingForLevel(level);
                }
            }

            Log(LogLevel.Trace, "Reconfigure NLog method finished");
        }
    }
}
