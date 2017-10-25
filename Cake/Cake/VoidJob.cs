using System;

namespace Cake
{
    /// <summary>
    /// Represents a void job to be executed at some point.
    /// </summary>
    public class VoidJob : CakeJob
    {
        private Action _action;
        /// <summary>
        /// Job constructor that is also registering newly created job to the<see cref="JobManager"/>.
        /// </summary>
        /// <param name="name"></param>
        public VoidJob(string name) : base(name)
        {
            _action = () => { };
        }

        /// <summary>
        /// Adds one or more Jobs that this job is dependent on.
        /// </summary>
        /// <param name="dependenciesToAdd">Names of depenedencies to be added to <see cref="CakeJob.Dependencies"/>.</param>
        /// <returns><see cref="VoidJob"/> object is returned so that method chaining can be used in the script.</returns>
        public VoidJob DependsOn(params string[] dependenciesToAdd)
        {
            base.DependsOn(dependenciesToAdd);
            return this;
        }

        /// <summary>
        /// Adds one or more <see cref="CakeJob"/> that this job is dependent on.
        /// </summary>
        /// <param name="dependenciesToAdd">Jobs that this job will be reliant on.</param>
        /// <returns><see cref="VoidJob"/> object is returned so that method chaining can be used in the script.</returns>
        public VoidJob DependsOn(params VoidJob[] dependenciesToAdd)
        {
            base.DependsOn(dependenciesToAdd);
            return this;
        }

        /// <summary>
        /// Defines an <see cref="Action"/> that can be performed by this job.
        /// </summary>
        /// <param name="actionToDo">Action delegate to be passed to this job.</param>
        /// <returns><see cref="VoidJob"/> object is returned so that method chaining can be used in the script.</returns>
        public VoidJob Does(Action actionToDo)
        {
            _action = actionToDo;
            return this;
        }

        /// <summary>
        /// Method for defining an exception path in the script
        /// </summary>
        /// <param name="exceptionJobName">Job that should run on exception</param>
        /// <returns></returns>
        public new VoidJob OnException(string exceptionJobName)
        {
            base.OnException(exceptionJobName);
            return this;    
        }

        /// <summary>
        /// Method for defining an exception path in the script
        /// </summary>
        /// <param name="exceptionJob">Job that should run on exception</param>
        /// <returns></returns>
        public new VoidJob OnException(CakeJob exceptionJob)
        {
            base.OnException(exceptionJob);
            return this;
        }

        protected override JobResult ExecuteJob()
        {
            _action();
            return new JobResult { Success = true };
        }
    }
}
