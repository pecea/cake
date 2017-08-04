namespace Common
{
    /// <summary>
    /// Container for <see cref="System.Diagnostics.Process"/> result
    /// </summary>
    public static class ProcessResult
    {
        /// <summary>
        /// flag indicating whether process run successfully
        /// </summary>
        public static bool Success { get; set; }
        /// <summary>
        /// output string
        /// </summary>
        public static string Output { get; set; }
        /// <summary>
        /// error string
        /// </summary>
        public static string Error { get; set; }

    }
}
