using Common;
using System;
using System.Collections.Generic;

namespace Cake
{
    /// <summary>
    /// Registers jobs read from the script, figures out job execution order and executes them.
    /// </summary>
    public static class JobManager
    {
        private static Dictionary<string, CakeJob> _jobs;

        internal static string JobToRun { get; set; }

        static JobManager()
        {
            _jobs = new Dictionary<string, CakeJob>();
        }

        /// <summary>
        /// Registers <see cref="Job"/> by adding it to the <see cref="_jobs"/> dictionary.
        /// </summary>
        /// <param name="job">A <see cref="Job"/> to be added.</param>
        internal static void RegisterJob(CakeJob job)
        {
            Logger.LogMethodStart();

            _jobs.Add(job.Name, job);

            Logger.Log(LogLevel.Debug, $"Job {job.Name} registered.");
            Logger.LogMethodEnd();
        }

        /// <summary>
        /// Runs <see cref="PerformJobWithDependencies"/> recursive function 
        /// and resets <see cref="CakeJob.Status"/> property of each <see cref="CakeJob"/> from <see cref="_jobs"/> after it is finished.
        /// </summary>
        /// <param name="name">Name of a <see cref="CakeJob"/> to be executed.</param>
        public static void SetDefault(string name)
        {
            Logger.LogMethodStart();
            if (!string.IsNullOrEmpty(JobToRun)) name = JobToRun;

            try
            {
                var validator = new DependenciesValidator(_jobs);
                var validationResult = validator.Validate();

                if (!validationResult.IsValid)
                    throw new JobException($"There is a circular dependency defined in the script. Job visited twice for dependency examination: {validationResult.CycleSourceJobName}.");
            }
            catch (Exception ex)
            {
                var stringException = "Exception occured while resolving dependencies in the script!";
                Logger.LogException(LogLevel.Error, ex, stringException);
                throw new JobException(stringException, ex);
            }

            var result = PerformJobWithDependencies(name);

            if (!result.Success)
                Logger.Log(LogLevel.Warn, $"Job {name} did not end succesfully!");

            Logger.LogMethodEnd();
        }

        /// <summary>
        /// Runs <see cref="PerformJobWithDependencies"/> recursive function 
        /// and resets <see cref="CakeJob.Status"/> property of each <see cref="CakeJob"/> from <see cref="_jobs"/> after it is finished.
        /// </summary>
        /// <param name="job">A <see cref="CakeJob"/> to be executed.</param>
        public static void SetDefault(CakeJob job)
        {
            SetDefault(job.Name);
        }

        /// <summary>
        /// Clears JobManager's job list.
        /// Used in unit testing.
        /// </summary>
        public static void ClearJobs()
        {
            Logger.LogMethodStart();
            _jobs = new Dictionary<string, CakeJob>();
            Logger.LogMethodEnd();
        }

        private static JobResult PerformJobWithDependencies(string name)
        {
            Logger.LogMethodStart();
            Logger.Debug($"Resolving {name} execution request.");

            JobResult result;
            var job = GetJob(name);

            if (job.Status == JobStatus.Failed)
            {
                Logger.Warn($"{name} has already failed.");
                return job.Result;
            }
            if (job.Status == JobStatus.Done)
            {
                Logger.Debug($"{name} has already finished.");
                return job.Result;
            }

            job.Status = JobStatus.Pending;

            foreach (var dependency in job.Dependencies)
            {
                Logger.Log(LogLevel.Debug, $"Resolving {name}'s dependencies ({job.Dependencies.Count}).");

                if (!PerformJobWithDependencies(dependency).Success)                
                    return new JobResult { Success = false };                
            }

            result = job.Execute();

            job.Status = result.Success 
                ? JobStatus.Done 
                : JobStatus.Failed;

            if (!result.Success)
                result = PerformExceptionJobWithDependencies(job);

            Logger.LogMethodEnd();
            return result;
        }

        private static CakeJob GetJob(string jobName)
        {
            try
            {
                return _jobs[jobName];
            }
            catch (KeyNotFoundException e)
            {
                throw new JobException($"Could not find the definition of Job \"{jobName}\".", e);
            }
        }

        private static JobResult PerformExceptionJobWithDependencies(CakeJob job)
        {
            if (job.HasExceptionJob)
            {
                return PerformJobWithDependencies(job.ExceptionJob);
            }
            // Break execution
            throw new JobException($"Job {job.Name} did not end succesfully!", job);
        }
    }
}