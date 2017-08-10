namespace Cake
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// These exceptions are thrown when there are errors in user written script concerning jobs.
    /// </summary>
    [Serializable]
    public class JobException : Exception, ISerializable
    {
        /// <summary>
        /// The source of the error
        /// </summary>
        public sealed override string Source { get { return _source; } set { _source = value; } }

        private string _source;

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
        /// <summary>
        /// The method is called on serialization.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Use the AddValue method to specify serialized values.
            info.AddValue("properties", _source, typeof(string));

        }
        
        /// <summary>
        /// The special constructor is used to deserialize values.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public JobException(SerializationInfo info, StreamingContext context)
        {
            // Reset the property value using the GetValue method.
            _source = (string)info.GetValue("properties", typeof(string));
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