namespace Common
{
    /// <summary>
    /// Represents level of output information
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// A level for most verbose information
        /// </summary>
        Trace,
        /// <summary>
        /// A level for debugging information
        /// </summary>
        Debug,
        /// <summary>
        /// A level for normal behaviour information
        /// </summary>
        Info,
        /// <summary>
        /// A level for incorrect behaviour
        /// </summary>
        Warn,
        /// <summary>
        /// A level for errors, crashes, exceptions in applicatiton
        /// </summary>
        Error,
        /// <summary>
        /// A level for logging fatal information
        /// </summary>
        Fatal
    }
}