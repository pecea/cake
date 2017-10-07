

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Common
{

    public static class JobManager<T> where T : new()
    {
        private static Dictionary<string, GenericJob<T>> _jobs;

        private static Dictionary<string, GenericJob<T>> _failJobs;

        //private static GenericJob<T> _failJob;

        internal static string JobToRun { get; set; }

        static JobManager()
        {
            _jobs = new Dictionary<string, GenericJob<T>>();
            _failJobs = new Dictionary<string, GenericJob<T>>();
        }

        /// <summary>
        /// Registers <see cref="Job"/> by adding it to the <see cref="_jobs"/> dictionary.
        /// </summary>
        /// <param name="job">A <see cref="Job"/> to be added.</param>
        public static void RegisterJob(GenericJob<T> job, bool failJob = false)
        {
            Logger.LogMethodStart();
            if (!failJob)
                _jobs.Add(job.Name, job);
            //else if (_failJobs.Count > 0)
            //    throw new JobDependencyException("There can only be one job done on fail!");
            else
                _failJobs.Add(job.Name, job);
            Logger.Log(LogLevel.Debug, $"Job \"{job.Name}\" registered.");
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
            //PerformJobWithDependencies(name);
            var result = PerformJobWithDependencies(name, _jobs);
            foreach (var job in _jobs)
            {
                job.Value.Status = JobStatus.NotVisited;
            }
            foreach (var job in _failJobs)
            {
                job.Value.Status = JobStatus.NotVisited;
            }
            if (!result.Item2)
                ///if (!result)
                Logger.Log(LogLevel.Warn, $"Job {name} did not end succesfully!");

            Logger.LogMethodEnd();

        }

        /// <summary>
        /// Runs <see cref="PerformJobWithDependencies"/> recursive function 
        /// and resets <see cref="CakeJob.Status"/> property of each <see cref="CakeJob"/> from <see cref="_jobs"/> after it is finished.
        /// </summary>
        /// <param name="job">A <see cref="CakeJob"/> to be executed.</param>
        public static void SetDefault(GenericJob<T> job)
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
            _jobs = new Dictionary<string, GenericJob<T>>();
            _failJobs = new Dictionary<string, GenericJob<T>>();
            Logger.LogMethodEnd();
        }

        private static string Serialize(T value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            try
            {
                var xmlserializer = new XmlSerializer(typeof(T));
                var stringWriter = new StringWriter();
                using (var writer = XmlWriter.Create(stringWriter))
                {
                    xmlserializer.Serialize(writer, value);
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot serialize the object!", ex);
            }
        }

        private static (T, bool) PerformJobWithDependencies(string name, Dictionary<string, GenericJob<T>> jobs)
        {
            Logger.LogMethodStart();
            GenericJob<T> job;
            try
            {
                job = jobs[name];
            }
            catch (KeyNotFoundException e)
            {
                throw new JobException($"Could not find the definition of Job \"{name}\".", e.Source);
            }
            switch (job.Status)
            {
                case JobStatus.Pending:
                    throw new JobException(
                       $"There is a circular dependency defined in the script. Job visited twice for dependency examination: {job.Name}.");
                case JobStatus.Failed:
                    return (default(T), false);
                case JobStatus.Done:
                    return (default(T), true);
            }
            job.Status = JobStatus.Pending;
            foreach (var dependency in job.Dependencies ?? new List<string>())
            {
                var dependencyResult = PerformJobWithDependencies(dependency, jobs);
                try
                {
                    var serialized = Serialize(dependencyResult.Item1);
                    Logger.Log(LogLevel.Info, serialized);
                }
                catch(Exception ex)
                {
                    Logger.LogException(LogLevel.Warn, ex, "Could not serialize dependency result!");
                }
                if (dependencyResult.Item2) continue;
                job.Status = JobStatus.Failed;
                throw new JobDependencyException($"Dependency {dependency} did not run succesfully!\n", "JobManager.RunJobWithDependencies");
            }
            try
            {
                var result = job.Execute();
                //if (result.Item2)
                //{
                job.Status = JobStatus.Done;
                Logger.Log(LogLevel.Trace, "Method finished successfully.");
                return (result, true);
                //return (result.Item1, true);
                //}
                //job.Status = JobStatus.Failed;
                //Logger.Log(LogLevel.Trace, "Method finished unsuccessfully.");
                //return (result.Item1, false);
            }
            catch (Exception e)
            {
                Logger.LogException(LogLevel.Error, e, "An exception occured while performing a job.\n");
                job.Status = JobStatus.Failed;
                PerformJobWithDependencies(job.FailJob, _failJobs);
                return (default(T), false);
                //throw new JobException($"Job {name} did not end succesfully!\n");
            }
        }
    }
}