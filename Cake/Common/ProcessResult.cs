namespace Common
{
    /// <summary>
    /// Container for <see cref="System.Diagnostics.Process"/> result
    /// </summary>
    public class ProcessResult
    {
        /// <summary>
        /// Flag indicating whether process run successfully
        /// </summary>
        public bool Success => ExitCode == 0;
        /// <summary>
        /// Output string
        /// </summary>
        public string Output { get; set; }
        /// <summary>
        /// Error string
        /// </summary>
        public string Error { get; set; }
        /// <summary>
        /// Process exit code
        /// </summary>
        public int ExitCode { get; set; }
    }
}
