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
        Debug,
        /// <summary>
        /// A level for detailed information
        /// </summary>
        Info,
        /// <summary>
        /// A level for warnings
        /// </summary>
        Warn,
        /// <summary>
        /// A level for errors in applicatiton
        /// </summary>
        Error,
        /// <summary>
        /// A level for logging fatal information
        /// </summary>
        Fatal
    }
}