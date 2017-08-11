using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake
{
    public abstract class CakeJob
    {

        internal string Name { get; }

        internal List<string> Dependencies { get; }

        internal JobStatus Status { get; set; }

        /// <summary>
        /// CakeJob constructor that is also registering newly created job to the <see cref="JobManager"/>.
        /// </summary>
        /// <param name="name"></param>
        public CakeJob(string name)
        {
            Name = name;
            Status = JobStatus.NotVisited;
            Dependencies = new List<string>();
            //_actionResult = () => true;
            JobManager.RegisterJob(this);
        }



        ///// <summary>
        ///// Adds one or more Jobs that this job is dependent on.
        ///// </summary>
        ///// <param name="dependenciesToAdd">Names of depenedencies to be added to <see cref="Dependencies"/>.</param>
        ///// <returns>The Job object is returned so that method chaining can be used in the script.</returns>
        //public abstract CakeJob DependsOn(params string[] dependenciesToAdd);

        ///// <summary>
        ///// Adds one or more <see cref="CakeJob"/> that this job is dependent on.
        ///// </summary>
        ///// <param name="dependenciesToAdd">Jobs that this job will be reliant on.</param>
        ///// <returns>The Job object is returned so that method chaining can be used in the script.</returns>
        //public abstract CakeJob DependsOn(params CakeJob[] dependenciesToAdd);

        internal abstract bool Execute();
    }
}
