namespace Common
{
    /// <summary>
    /// Represents current status of a job.
    /// </summary>
    public enum JobStatus
    {
        /// <summary>
        /// Represents a job that has not been visited yet.
        /// </summary>
        NotVisited,
        /// <summary>
        /// Represents a job which dependencies are being examined and executed.
        /// </summary>
        Pending,
        /// <summary>
        /// Represents a job which action has been executed succesfully.
        /// </summary>
        Done,
        /// <summary>
        /// Represents a job which action has been executed unsuccesfully.
        /// </summary>
        Failed
    }
}