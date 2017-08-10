namespace Common
{
    /// <summary>
    /// Container for <see cref="System.Diagnostics.Process"/> result
    /// </summary>
    public class ProcessResult
    {
        /// <summary>
        /// flag indicating whether process run successfully
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// output string
        /// </summary>
        public string Output { get; set; }
        /// <summary>
        /// error string
        /// </summary>
        public string Error { get; set; }

    }
}
