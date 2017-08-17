using System;
using System.Linq;
using Common;

namespace Cake
{
    /// <summary>
    /// Class for
    /// </summary>
    public class VoidJob : CakeJob
    {
        private Action _action;
        /// <summary>
        /// Job constructor that is also registering newly created job to the <see cref="JobManager"/>.
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
        /// <returns>The Job object is returned so that method chaining can be used in the script.</returns>
        public VoidJob DependsOn(params string[] dependenciesToAdd)
        {
            Logger.LogMethodStart();
            foreach (var dependency in dependenciesToAdd.Where(dependency => Dependencies.All(added => added != dependency)))
            {
                Dependencies.Add(dependency);
            }

            Logger.LogMethodEnd();
            return this;
        }

        /// <summary>
        /// Adds one or more <see cref="VoidJob"/> that this job is dependent on.
        /// </summary>
        /// <param name="dependenciesToAdd">Jobs that this job will be reliant on.</param>
        /// <returns>The Job object is returned so that method chaining can be used in the script.</returns>
        public VoidJob DependsOn(params VoidJob[] dependenciesToAdd)
        {
            return DependsOn(dependenciesToAdd.Select(dependency => dependency.Name).ToArray());
        }

        /// <summary>
        /// Defines an <see cref="Action"/> that can be performed by this job.
        /// </summary>
        /// <param name="actionToDo">Action delegate to be passed to this job.</param>
        /// <returns>The Job object is returned so that method chaining can be used in the script.</returns>
        public VoidJob Does(Action actionToDo)
        {
            _action = actionToDo;
            return this;
        }

        internal override bool Execute()
        {
            Logger.LogMethodStart();
            try
            {
                _action();
                Logger.Log(LogLevel.Debug, $"Job \"{Name}\" executed.");
                Logger.LogMethodEnd();
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(LogLevel.Error, ex, "Exception occured during a job!");
                Logger.LogMethodEnd();
                return false;
            }
        }
    }
}
