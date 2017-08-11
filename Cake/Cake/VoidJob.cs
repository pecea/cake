using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake
{
    public class VoidJob : CakeJob
    {
        private Action _action;
        /// <summary>
        /// Job constructor that is also registering newly created job to the <see cref="JobManager"/>.
        /// </summary>
        /// <param name="name"></param>
        public VoidJob(string name) : base(name)
        {
            //Name = name;
            //Status = JobStatus.NotVisited;
            //Dependencies = new List<string>();
            _action = () => { };
            //JobManager.RegisterJob(this);
        }

        /// <summary>
        /// Adds one or more Jobs that this job is dependent on.
        /// </summary>
        /// <param name="dependenciesToAdd">Names of depenedencies to be added to <see cref="CakeJob.Dependencies"/>.</param>
        /// <returns>The Job object is returned so that method chaining can be used in the script.</returns>
        public new VoidJob DependsOn(params string[] dependenciesToAdd)
        {
            Logger.Log(LogLevel.Trace, "Method started");
            foreach (var dependency in dependenciesToAdd.Where(dependency => Dependencies.All(added => added != dependency)))
            {
                Dependencies.Add(dependency);
            }
            return this;
        }

        /// <summary>
        /// Adds one or more <see cref="VoidJob"/> that this job is dependent on.
        /// </summary>
        /// <param name="dependenciesToAdd">Jobs that this job will be reliant on.</param>
        /// <returns>The Job object is returned so that method chaining can be used in the script.</returns>
        public VoidJob DependsOn(params VoidJob[] dependenciesToAdd)
        {
            Logger.Log(LogLevel.Trace, "Method started");
            return DependsOn(dependenciesToAdd.Select(dependency => dependency.Name).ToArray());
        }

        /// <summary>
        /// Defines an <see cref="Action"/> that can be performed by this job.
        /// </summary>
        /// <param name="actionToDo">Action delegate to be passed to this job.</param>
        /// <returns>The Job object is returned so that method chaining can be used in the script.</returns>
        public VoidJob Does(Action actionToDo)
        {
            Logger.Log(LogLevel.Trace, "Method started");
            _action = actionToDo;
            return this;
        }

        internal override bool Execute()
        {
            Logger.Log(LogLevel.Trace, "Method started");
            try
            {
                _action();
                Logger.Log(LogLevel.Debug, $"Job \"{Name}\" executed.");
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(LogLevel.Error, ex, "Exception occured during a job!");
                return false;
            }
        }
    }
}
