namespace Cake
{
    using System;

    public sealed class TaskException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TaskException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="source">The source of the error.</param>
        public TaskException(string message, string source) : base(message)
        {
            Source = source;
        }
    }
}