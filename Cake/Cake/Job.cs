using System;

namespace Cake
{
    /// <summary>
    /// Represents a job to be executed at some point.
    /// </summary>
    public class Job : CakeJob
    {
        private Func<dynamic> _actionWithResult;

        /// <summary>
        /// Job constructor that is also registering newly created job to the <see cref="JobManager"/>.
        /// </summary>
        /// <param name="name"></param>
        public Job(string name) : base(name)
        {
            _actionWithResult = () => true;
        }

        /// <summary>
        /// Adds one or more Jobs that this job is dependent on.
        /// </summary>
        /// <param name="dependenciesToAdd">Names of depenedencies to be added to <see cref="CakeJob.Dependencies"/>.</param>
        /// <returns><see cref="Job"/> object is returned so that method chaining can be used in the script.</returns>
        public Job DependsOn(params string[] dependenciesToAdd)
        {
            base.DependsOn(dependenciesToAdd);
            return this;
        }

        /// <summary>
        /// Adds one or more <see cref="CakeJob"/> that this job is dependent on.
        /// </summary>
        /// <param name="dependenciesToAdd">Jobs that this job will be reliant on.</param>
        /// <returns><see cref="Job"/> object is returned so that method chaining can be used in the script.</returns>
        public Job DependsOn(params Job[] dependenciesToAdd)
        {
            base.DependsOn(dependenciesToAdd);
            return this;
        }

        /// <summary>
        /// Defines a <see cref="Func{T, TResult}"/> that can be perfromed by this job.
        /// </summary>
        /// <param name="actionWithResultToDo">Function delegate to be passed to this job.</param>
        /// <returns><see cref="Job"/> object is returned so that method chaining can be used in the script.</returns>
        public Job Does(Func<dynamic> actionWithResultToDo)
        {
            _actionWithResult = actionWithResultToDo;
            return this;
        }

        /// <summary>
        /// Method for defining an exception path in the script
        /// </summary>
        /// <param name="exceptionJobName">Job that should run on exception</param>
        /// <returns></returns>
        public new Job OnException(string exceptionJobName)
        {
            base.OnException(exceptionJobName);
            return this;
        }

        /// <summary>
        /// Method for defining an exception path in the script
        /// </summary>
        /// <param name="exceptionJob">Job that should run on exception</param>
        /// <returns></returns>
        public new Job OnException(CakeJob exceptionJob)
        {
            base.OnException(exceptionJob);
            return this;
        }

        protected override JobResult ExecuteJob()
        {
            dynamic actionResult = _actionWithResult();
            return new JobResult
            {
                ResultObject = actionResult,
                Success = true
            };
        }
    }
}