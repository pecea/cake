using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake
{

    //public static class JobManager<T> where T: new()
    //{
    //    private static Dictionary<string, GenericJob<T>> _jobs;

    //    internal static string JobToRun { get; set; }

    //    static JobManager()
    //    {
    //        _jobs = new Dictionary<string, GenericJob<T>>();
    //    }

    //    /// <summary>
    //    /// Registers <see cref="Job"/> by adding it to the <see cref="_jobs"/> dictionary.
    //    /// </summary>
    //    /// <param name="job">A <see cref="Job"/> to be added.</param>
    //    public static void RegisterJob(GenericJob<T> job)
    //    {
    //        Logger.LogMethodStart();
    //        _jobs.Add(job.Name, job);
    //        Logger.Log(LogLevel.Debug, $"Job \"{job.Name}\" registered.");
    //        Logger.LogMethodEnd();
    //    }

    //    /// <summary>
    //    /// Runs <see cref="PerformJobWithDependencies"/> recursive function 
    //    /// and resets <see cref="CakeJob.Status"/> property of each <see cref="CakeJob"/> from <see cref="_jobs"/> after it is finished.
    //    /// </summary>
    //    /// <param name="name">Name of a <see cref="CakeJob"/> to be executed.</param>
    //    public static void SetDefault(string name)
    //    {
    //        Logger.LogMethodStart();
    //        if (!string.IsNullOrEmpty(JobToRun)) name = JobToRun;
    //        //PerformJobWithDependencies(name);
    //        var result = PerformJobWithDependencies(name);
    //        foreach (var job in _jobs)
    //        {
    //            job.Value.Status = JobStatus.NotVisited;
    //        }
    //        if(!result.Item2)
    //        ///if (!result)
    //            Logger.Log(LogLevel.Warn, $"Job {name} did not end succesfully!");

    //        Logger.LogMethodEnd();

    //    }

    //    /// <summary>
    //    /// Runs <see cref="PerformJobWithDependencies"/> recursive function 
    //    /// and resets <see cref="CakeJob.Status"/> property of each <see cref="CakeJob"/> from <see cref="_jobs"/> after it is finished.
    //    /// </summary>
    //    /// <param name="job">A <see cref="CakeJob"/> to be executed.</param>
    //    public static void SetDefault(GenericJob<T> job)
    //    {
    //        SetDefault(job.Name);
    //    }

    //    /// <summary>
    //    /// Clears JobManager's job list.
    //    /// Used in unit testing.
    //    /// </summary>
    //    public static void ClearJobs()
    //    {
    //        Logger.LogMethodStart();
    //        _jobs = new Dictionary<string, GenericJob<T>>();
    //        Logger.LogMethodEnd();
    //    }

    //    private static (T, bool) PerformJobWithDependencies(string name)
    //    {
    //        Logger.LogMethodStart();
    //        GenericJob<T> job;
    //        try
    //        {
    //            job = _jobs[name];
    //        }
    //        catch (KeyNotFoundException e)
    //        {
    //            throw new JobException($"Could not find the definition of Job \"{name}\".", e.Source);
    //        }
    //        switch (job.Status)
    //        {
    //            case JobStatus.Pending:
    //                throw new JobException(
    //                   $"There is a circular dependency defined in the script. Job visited twice for dependency examination: {job.Name}.");
    //            case JobStatus.Failed:
    //                return (default(T), false);
    //            case JobStatus.Done:
    //                return (new T(), true);
    //        }
    //        job.Status = JobStatus.Pending;
    //        foreach (var dependency in job.Dependencies ?? new List<string>())
    //        {
    //            if (PerformJobWithDependencies(dependency).Item2) continue;
    //            job.Status = JobStatus.Failed;
    //            throw new JobDependencyException($"Dependency {dependency} did not run succesfully!\n", "JobManager.RunJobWithDependencies");
    //        }
    //        try
    //        {
    //            //var result = job.Execute();
    //            //if (result.Item2)
    //            //{
    //            job.Status = JobStatus.Done;
    //            Logger.Log(LogLevel.Trace, "Method finished successfully.");
    //            return (job.Execute(), true);
    //                //return (result.Item1, true);
    //            //}
    //            //job.Status = JobStatus.Failed;
    //            //Logger.Log(LogLevel.Trace, "Method finished unsuccessfully.");
    //            //return (result.Item1, false);
    //        }
    //        catch (Exception e)
    //        {
    //            Logger.LogException(LogLevel.Error, e, "An exception occured while performing a job.\n");
    //            job.Status = JobStatus.Failed;
    //            throw new JobException($"Job {name} did not end succesfully!\n");
    //        }
    //    }
    //}

    /// <summary>
    /// Registers jobs read from the script, figures out job execution order and executes them.
    /// </summary>
    public static class JobManager
    {
        private static Dictionary<string, CakeJob> _jobs;
        private static List<string> _dependencies;

        internal static string JobToRun { get; set; }

        static JobManager()
        {
            _jobs = new Dictionary<string, CakeJob>();
            _dependencies = new List<string>();
        }

        private static bool CycleDetection()
        {

            Dictionary<string, bool> visited = new Dictionary<string, bool>();
            Dictionary<string, bool> visitedTemporarily = new Dictionary<string, bool>();
            foreach (var job in _jobs)
            {
                visited.Add(job.Key, false);
                visitedTemporarily.Add(job.Key, false);
            }
            //var node = visited.FirstOrDefault(v => !v.Value);

            while (visited.Any(v => !v.Value))
            {
                var node = visited.First(v => !v.Value);
                if (Visit(node, ref visited, ref visitedTemporarily))
                    return true;
            }
            return false;
        }

        private static bool Visit(KeyValuePair<string, bool> node, ref Dictionary<string,bool> visited, ref Dictionary<string, bool> visitedTemporarily)
        {
            CakeJob job;
            bool visitedTemporarilyNode;
            try
            {
                job = _jobs[node.Key];
                visitedTemporarilyNode = visitedTemporarily[node.Key];
            }
            catch(KeyNotFoundException ex)
            {
                Logger.LogException(LogLevel.Error, ex, $"Job {node.Key} does not exist!");
                throw;
            }
            if (visitedTemporarilyNode) return true;
            visitedTemporarily[node.Key] = true;
            foreach (var dependency in job.Dependencies)
            {
                KeyValuePair<string, bool> visitedNode;
                try
                {
                    visitedNode = visited.First(v => v.Key == dependency);
                }
                catch(InvalidOperationException ex)
                {
                    Logger.LogException(LogLevel.Error, ex, $"Dependency {dependency} does not exist!");
                    throw;
                }
                if (Visit(visitedNode, ref visited, ref visitedTemporarily))
                    return true;
            }
            visited[node.Key] = true;
            visitedTemporarily[node.Key] = false;
            return false;
        }

        //private static void CheckDependencies(string jobName)
        //{
        //    foreach(var)
        //    //CakeJob job;
        //    //try
        //    //{
        //    //    job = _jobs[jobName];
        //    //}
        //    //catch (KeyNotFoundException e)
        //    //{
        //    //    throw new JobException($"Could not find the definition of Job \"{jobName}\".", e.Source);
        //    //}
        //    //if (_dependencies.Contains(jobName))
        //    //    throw new JobException(
        //    //           $"There is a circular dependency defined in the script. Job visited twice for dependency examination: {jobName}.");
        //    //_dependencies.Add(jobName);
        //    //foreach (var dependency in job.Dependencies)
        //    //    CheckDependencies(dependency);
        //}

        /// <summary>
        /// Registers <see cref="Job"/> by adding it to the <see cref="_jobs"/> dictionary.
        /// </summary>
        /// <param name="job">A <see cref="Job"/> to be added.</param>
        public static void RegisterJob(CakeJob job)
        {
            Logger.LogMethodStart();
            _jobs.Add(job.Name, job);
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
            //CheckDependencies(name);
            try
            {
                if (CycleDetection())
                    throw new JobException(
                           $"There is a circular dependency defined in the script. Job visited twice for dependency examination: {name}.");
            }
            catch(Exception ex)
            {
                var stringException = "Exception occured while resolving dependencies in the script!";
                Logger.LogException(LogLevel.Error, ex, stringException);
                throw new JobException(stringException);
            }

            JobResult result;
            try
            {
                result = PerformJobWithDependencies(name);
            }
            catch (Exception ex)
            {
                Logger.LogException(LogLevel.Error, ex, $"Exception occurred in job {name}");
                throw new JobException($"Job {name} did not run succesfully!\n");
            }
            //foreach (var job in _jobs)
            //{
            //    job.Value.Status = JobStatus.NotVisited;
            //}
            if(!result.Success)
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

        //private static string Serialize<T>(T value)
        //{
        //    if (value == null)
        //    {
        //        return string.Empty;
        //    }
        //    try
        //    {
        //        var xmlserializer = new XmlSerializer(typeof(T));
        //        var stringWriter = new StringWriter();
        //        using (var writer = XmlWriter.Create(stringWriter))
        //        {
        //            xmlserializer.Serialize(writer, value);
        //            return stringWriter.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Cannot serialize the object!", ex);
        //    }
        //}

        private static JobResult PerformJobWithDependencies(string name)
        {
            Logger.LogMethodStart();
            CakeJob job;
            try
            {
                job = _jobs[name];
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
                    return new JobResult
                    {
                        ResultObject = name,
                        Success = false
                    };
                case JobStatus.Done:
                    return new JobResult
                    {
                        ResultObject = name,
                        Success = true
                    };
            }
            job.Status = JobStatus.Pending;
            foreach (var dependency in job.Dependencies ?? new List<string>())
            {
                JobResult dependencyResult;
                try
                {
                    dependencyResult = PerformJobWithDependencies(dependency);
                }
                catch (Exception ex)
                {
                    Logger.LogException(LogLevel.Error, ex, $"Exception occurred in dependency {dependency}");
                    job.Status = JobStatus.Failed;
                    throw new JobDependencyException($"Dependency {dependency} did not run succesfully!\n");
                }
                //try
                //{
                //    var serialized = Serialize(dependencyResult);
                //    Logger.Log(LogLevel.Info, $"Dependency {dependency} result: {serialized}");
                //}
                //catch (Exception ex)
                //{
                //    Logger.LogException(LogLevel.Warn, ex, $"Could not serialize dependency {dependency} result!");
                //}
                
                //if (PerformJobWithDependencies(dependency)) continue;
                //job.Status = JobStatus.Failed;
                //throw new JobDependencyException($"Dependency {dependency} did not run succesfully!\n", "JobManager.RunJobWithDependencies");
            }
            try
            {
                var result = job.Execute();
                job.Status = JobStatus.Done;
                Logger.Log(LogLevel.Trace, "Method finished successfully.");
                return new JobResult
                {
                    ResultObject = result,
                    Success = true
                };
                //job.Status = JobStatus.Failed;
                //Logger.Log(LogLevel.Trace, "Method finished unsuccessfully.");
                //return false;
            }
            catch (Exception e)
            {
                Logger.LogException(LogLevel.Error, e, "An exception occured while performing a job.\n");
                job.Status = JobStatus.Failed;
                throw new JobException($"Job {name} did not end succesfully!\n");
            }
        }
    }
}