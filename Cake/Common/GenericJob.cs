using System;
using System.Linq;
using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// Represents a job to be executed at some point.
    /// </summary>
    public class GenericJob<T> where T: new()
    {
        internal string Name { get; }

        internal List<string> Dependencies { get; }

        internal string FailJob { get; set; }

        //internal List<string> FailDependencies { get; }

        internal JobStatus Status { get; set; }

        private Func<T> _action;

        //private Action _failAction;

        /// <summary>
        /// Job constructor that is also registering newly created job to the <see cref="JobManager"/>.
        /// </summary>
        /// <param name="name"></param>
        public GenericJob(string name, bool failJob = false)
        {
            Name = name;
            Status = JobStatus.NotVisited;
            Dependencies = new List<string>();
            //FailDependencies = new List<string>();
            JobManager<T>.RegisterJob(this, failJob);
        }

        /// <summary>
        /// Adds one or more Jobs that this job is dependent on.
        /// </summary>
        /// <param name="dependenciesToAdd">Names of depenedencies to be added to <see cref="CakeJob.Dependencies"/>.</param>
        /// <returns>The Job object is returned so that method chaining can be used in the script.</returns>
        public GenericJob<T> DependsOn(params string[] dependenciesToAdd)
        {
            Logger.LogMethodStart();
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
        public GenericJob<T> DependsOn(params GenericJob<T>[] dependenciesToAdd)
        {
            Logger.LogMethodStart();
            return DependsOn(dependenciesToAdd.Select(dependency => dependency.Name).ToArray());
        }


        /// <summary>
        /// Defines a <see cref="Func{T, TResult}"/> that can be perfromed by this job.
        /// </summary>
        /// <param name="actionWithResultToDo">Function delegate to be passed to this job.</param>
        /// <returns>The Job object is returned so that method chaining can be used in the script.</returns>
        public GenericJob<T> Does(Func<T> actionToDo)
        {
            _action = actionToDo;
            return this;
        }

        public GenericJob<T> OnFail(string failJob)
        {
            FailJob = failJob;
            return this;
        }

        internal T Execute()
        {
            Logger.LogMethodStart();
            //try
            //{
                var res = _action();
                Logger.Log(LogLevel.Debug, $"Job \"{Name}\" executed.");
                return res;
            //}
            //catch (Exception ex)
            //{
            //    Logger.LogException(LogLevel.Error, ex, "Exception occured during a job!");
                ///return default(T);
            //}
        }
    }
}