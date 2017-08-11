namespace Cake
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;

    /// <summary>
    /// Represents a job to be executed at some point.
    /// </summary>
    public class Job : CakeJob
    {
        private Func<bool> _actionWithResult;
        
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
        /// <returns>The Job object is returned so that method chaining can be used in the script.</returns>
        public Job DependsOn(params string[] dependenciesToAdd)
        {
            Logger.Log(LogLevel.Trace, "Method started");
            foreach (var dependency in dependenciesToAdd.Where(dependency => Dependencies.All(added => added != dependency)))
            {
                Dependencies.Add(dependency);
            }
            return this;
        }

        /// <summary>
        /// Adds one or more <see cref="Job"/> that this job is dependent on.
        /// </summary>
        /// <param name="dependenciesToAdd">Jobs that this job will be reliant on.</param>
        /// <returns>The Job object is returned so that method chaining can be used in the script.</returns>
        public Job DependsOn(params Job[] dependenciesToAdd)
        {
            Logger.Log(LogLevel.Trace, "Method started");
            return DependsOn(dependenciesToAdd.Select(dependency => dependency.Name).ToArray());
        }

        /// <summary>
        /// Defines a <see cref="Func{T, TResult}"/> that can be perfromed by this job.
        /// </summary>
        /// <param name="actionWithResultToDo">Function delegate to be passed to this job.</param>
        /// <returns>The Job object is returned so that method chaining can be used in the script.</returns>
        public Job Does(Func<bool> actionWithResultToDo)
        {
            Logger.Log(LogLevel.Trace, "Method started");
            _actionWithResult = actionWithResultToDo;
            return this;
        }

        
        internal override bool Execute()
        {
            Logger.Log(LogLevel.Trace, "Method started");
            try
            {
                bool res = _actionWithResult();
                Logger.Log(LogLevel.Debug, $"Job \"{Name}\" executed.");
                return res;
            }
            catch (Exception ex)
            {
                Logger.LogException(LogLevel.Error, ex, "Exception occured during a job!");
                return false;
            }
        }
    }
}