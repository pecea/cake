using System.Collections.Generic;

namespace Cake
{
    /// <summary>
    /// Parent class for <see cref="Job"/> and <see cref="VoidJob"/>
    /// </summary>
    public abstract class CakeJob
    {

        internal string Name { get; }

        internal List<string> Dependencies { get; }

        internal JobStatus Status { get; set; }

        public JobResult Result { get; set; }

        /// <summary>
        /// CakeJob constructor that is also registering newly created job to the <see cref="JobManager"/>
        /// </summary>
        /// <param name="name"></param>
        protected CakeJob(string name)
        {
            Name = name;
            Status = JobStatus.NotVisited;
            Dependencies = new List<string>();
            JobManager.RegisterJob(this);
        }

        internal abstract JobResult Execute();
    }
}
