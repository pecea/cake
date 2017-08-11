namespace Cake
{
    using Common;
    using System;
    using System.Collections.Generic;

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
        public static void RegisterJob(CakeJob job)
        {
            Logger.Log(LogLevel.Trace, "Method started");
            _jobs.Add(job.Name, job);
            Logger.Log(LogLevel.Debug, $"Job \"{job.Name}\" registered.");
            Logger.Log(LogLevel.Trace, "Method finished");
        }

        /// <summary>
        /// Runs <see cref="PerformJobWithDependencies"/> recursive function 
        /// and resets <see cref="CakeJob.Status"/> property of each <see cref="CakeJob"/> from <see cref="_jobs"/> after it is finished.
        /// </summary>
        /// <param name="name">Name of a <see cref="CakeJob"/> to be executed.</param>
        public static void SetDefault(string name)
        {
            Logger.Log(LogLevel.Trace, "Method started");
            if (!string.IsNullOrEmpty(JobToRun)) name = JobToRun;
            //PerformJobWithDependencies(name);
            var result = PerformJobWithDependencies(name);
            foreach (var job in _jobs)
            {
                job.Value.Status = JobStatus.NotVisited;
            }
            if (!result)
                throw new JobException($"Job {name} did not end succesfully!");

            Logger.Log(LogLevel.Trace, "Method finished");

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
            Logger.Log(LogLevel.Trace, "Method started");
            _jobs = new Dictionary<string, CakeJob>();
            Logger.Log(LogLevel.Trace, "Method finished");
        }
        
        private static bool PerformJobWithDependencies(string name)
        {
            Logger.Log(LogLevel.Trace, "Method started");
            CakeJob job;
            try
            {
                job = _jobs[name];
            }
            catch (KeyNotFoundException e)
            {
                throw new JobException($"Could not find the definition of Job \"{name}\".", e.Source);
            }
            switch(job.Status)
            {
                case JobStatus.Pending:
                    throw new JobException(
                       $"There is a circular dependency defined in the script. Job visited twice for dependency examination: {job.Name}.");
                case JobStatus.Failed:
                    return false;
                case JobStatus.Done:
                    return true;
            }
            job.Status = JobStatus.Pending;
            foreach (var dependency in job.Dependencies ?? new List<string>())
            {
                if (PerformJobWithDependencies(dependency)) continue;
                job.Status = JobStatus.Failed;
                throw new JobDependencyException($"Dependency {dependency} did not run succesfully!\n", "JobManager.RunJobWithDependencies");
            }
            try
            {
                if (job.Execute())
                {
                    job.Status = JobStatus.Done;
                    Logger.Log(LogLevel.Trace, "Method finished successfully");
                    return true;
                }
                job.Status = JobStatus.Failed;
                Logger.Log(LogLevel.Trace, "Method finished unsuccessfully");
                return false;
            }
            catch (Exception e)
            {
                Logger.LogException(LogLevel.Error, e, "An exception occured while performing a job\n");
                job.Status = JobStatus.Failed;
                throw new JobException($"Job {name} did not end succesfully!\n");
            }
        }
    }
}