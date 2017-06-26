namespace Cake
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;

    /// <summary>
    /// Represents a job to be executed at some point.
    /// </summary>
    public class Job
    {
        private Action _action;

        //private Func<bool> _actionResult;

        internal string Name { get; }


        internal List<string> Dependencies { get; }


        internal JobStatus Status { get; set; }

        /// <summary>
        /// Job constructor that is also registering newly created job to the <see cref="JobManager"/>.
        /// </summary>
        /// <param name="name"></param>
        public Job(string name)
        {
            Name = name;
            Status = JobStatus.NotVisited;
            Dependencies = new List<string>();
            _action = () => { };
            //_actionResult = () => true;
            JobManager.RegisterJob(this);
        }


        public Job DependsOn(string otherJobs)
        {
            foreach (var dependency in otherJobs.Split(',').Select(j => j.Trim()).Where(d => Dependencies.All(added => added != d)).ToArray())
            {
                Dependencies.Add(dependency);   
            }
            //Dependencies.Add(otherJob);
            return this;
        }

        /// <summary>
        /// Adds one or more Jobs that this job is dependent on.
        /// </summary>
        /// <param name="dependenciesToAdd">Names of depenedencies to be added to <see cref="Dependencies"/>.</param>
        /// <returns>The Job object is returned so that method chaining can be used in the script.</returns>
        public Job DependsOn(params string[] dependenciesToAdd)
        {
            foreach (var dependency in dependenciesToAdd.Where(dependency => Dependencies.All(added => added != dependency)))
            {
                Dependencies.Add(dependency);
            }
            return this;
        }

        /// <summary>
        /// Adds one or more Jobs that this job is dependent on.
        /// </summary>
        /// <param name="dependenciesToAdd">Jobs that this job will be reliant on.</param>
        /// <returns>The Job object is returned so that method chaining can be used in the script.</returns>
        public Job DependsOn(params Job[] dependenciesToAdd)
        {
            return DependsOn(dependenciesToAdd.Select(dependency => dependency.Name).ToArray());
        }

        /// <summary>
        /// Defines an <see cref="Action"/> that can be performed by this job.
        /// </summary>
        /// <param name="actionToDo">Action delegate to be passed to this job.</param>
        /// <returns>The Job object is returned so that method chaining can be used in the script.</returns>
        public Job Does(Action actionToDo)
        {
            _action = actionToDo;
            return this;
        }

        //internal void Execute()
        //{
        //    _action();
        //    Logger.Log(LogLevel.Debug, $"Job \"{Name}\" executed.");
        //}

        //internal bool ExecuteNew()
        //{
        //    Logger.Log(LogLevel.Debug, $"Executing job \"{Name}\".");
        //    try
        //    {
        //        return _actionNew();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogException(LogLevel.Error, ex, "Exception occured during a job!");
        //        return false;
        //    }
        //    return true;
        //}
        internal bool Execute()
        {
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

        //internal bool ExecuteWithResult()
        //{
        //    try
        //    {
        //        return _actionResult();
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.LogException(LogLevel.Error, e, "Exception occured during a job!");
        //        return false;
        //    }
        //}

        //public Job Performs(Func<bool> actionToDo)
        //{
        //    _actionResult = actionToDo;
        //    return this;
        //}
    }
}