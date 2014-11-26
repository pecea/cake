namespace Common
{
    using System;
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
                    throw new ArgumentOutOfRangeException("logLevel");
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
            Log(logLevel, String.Format("{0} ExceptionType: {1}. Exception source: {2}. Exception message: {3}", message.Trim(), e.GetType(), e.Source, e.Message), loggerName);
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
                Log(logLevel, String.Format("Inner exception source: {0}. Inner exception message: {1}", e.Source, e.Message), loggerName);
        }
    }
}
