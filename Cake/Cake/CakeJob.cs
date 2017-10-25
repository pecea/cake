using Common;
using System;
using System.Collections.Generic;
using System.Linq;

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

        internal string ExceptionJob { get; private set; }

        internal bool HasExceptionJob => !string.IsNullOrWhiteSpace(ExceptionJob);

        /// <summary>
        /// Property that keeps the job's action result
        /// </summary>
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

        /// <summary>
        /// Method for defining an exception path in the script
        /// </summary>
        /// <param name="exceptionJobName">Job that should run on exception</param>
        /// <returns></returns>
        protected CakeJob OnException(string exceptionJobName)
        {
            ExceptionJob = exceptionJobName;
            return this;
        }

        /// <summary>
        /// Method for defining an exception path in the script
        /// </summary>
        /// <param name="exceptionJob">Job that should run on exception</param>
        /// <returns></returns>
        protected CakeJob OnException(CakeJob exceptionJob)
        {
            ExceptionJob = exceptionJob.Name;
            return this;
        }

        /// <summary>
        /// Adds one or more Jobs that this job is dependent on.
        /// </summary>
        /// <param name="dependenciesToAdd">Names of depenedencies to be added to <see cref="CakeJob.Dependencies"/>.</param>
        /// <returns>The Job object is returned so that method chaining can be used in the script.</returns>
        protected CakeJob DependsOn(params string[] dependenciesToAdd)
        {
            Logger.LogMethodStart();

            foreach (var dependency in dependenciesToAdd.Where(dependency => Dependencies.All(added => added != dependency)))
            {
                Dependencies.Add(dependency);
            }

            Logger.LogMethodEnd();
        /// <returns><see cref="CakeJob"/> object is returned so that method chaining can be used in the script.</returns>
        public CakeJob OnException(string exceptionJob)
        {
            ExceptionJob = exceptionJob;
            return this;
        }

        /// <summary>
        /// Adds one or more <see cref="CakeJob"/> that this job is dependent on.
        /// </summary>
        /// <param name="dependenciesToAdd">Jobs that this job will be reliant on.</param>
        /// <returns>The Job object is returned so that method chaining can be used in the script.</returns>
        protected CakeJob DependsOn(params CakeJob[] dependenciesToAdd)
        {
            return DependsOn(dependenciesToAdd.Select(dependency => dependency.Name).ToArray());
        }

        internal JobResult Execute()
        {
            Logger.LogMethodStart();

            try
            {
                Logger.Log(LogLevel.Debug, $"Executing {Name}.");
                Result = ExecuteJob();
                Logger.Log(LogLevel.Debug, $"{Name} executed.");

                return Result;
            }
            catch (Exception e)
            {
                return HandleExecuteException(e);
            }
            finally
            {
                Logger.LogMethodEnd();
            }
        }

        protected abstract JobResult ExecuteJob();

        private JobResult HandleExecuteException(Exception e)
        {
            LogLevel level;
            string message = $"An exception occured while executing {Name}";

            if (HasExceptionJob)
            {
                level = LogLevel.Warn;
                message += $", {ExceptionJob} will be executed.";
            }
            else
            {
                level = LogLevel.Error;
                message += $" and there is no \"OnException\" job specified.";
            }
            
            Logger.LogException(level, e, message, includeStackTrace: true);

            return Result = new JobResult
            {
                Exception = e,
                Success = false
            };
        }
    }
}
