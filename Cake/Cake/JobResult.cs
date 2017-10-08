namespace Cake
{
    /// <summary>
    /// Class representing job execution result
    /// </summary>
    public class JobResult
    {
        /// <summary>
        /// Container for result of the job action
        /// </summary>
        public dynamic ResultObject { get; set; }
        /// <summary>
        /// Flag indicating whether job was successful
        /// </summary>
        public bool Success { get; set; }
    }
}
