namespace Cake
{
    using System;

    /// <summary>
    /// These exceptions are thrown when there are errors in user written script concerning jobs.
    /// </summary>
    public class JobException : Exception
    {
        /// <summary>
        /// The source of the error
        /// </summary>
        public sealed override string Source { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public JobException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="source">The source of the error.</param>
        public JobException(string message, string source) : base(message)
        {
            Source = source;
        }
    }

    /// <summary>
    /// These exceptions are thrown when there are errors in dependencies of jobs.
    /// </summary>
    public sealed class JobDependencyException : JobException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobDependencyException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public JobDependencyException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobDependencyException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="source">The source of the error.</param>
        public JobDependencyException(string message, string source) : base(message)
        {
            Source = source;
        }
    }
}