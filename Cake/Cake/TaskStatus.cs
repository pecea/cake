namespace Cake
{
    /// <summary>
    /// Represents current status of a task.
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// Represents a task that has not been visited yet.
        /// </summary>
        NotVisited,
        /// <summary>
        /// Represents a task whose dependencies are being examined and executed.
        /// </summary>
        Pending,
        /// <summary>
        /// Represents a task whose action has been executed.
        /// </summary>
        Done
    }
}