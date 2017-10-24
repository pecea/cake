using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake
{
    internal class DependenciesValidator
    {
        private readonly IDictionary<string, CakeJob> _jobs;

        public DependenciesValidator(IDictionary<string, CakeJob> jobs)
        {
            _jobs = jobs;
        }

        public DependenciesValidationResult Validate()
        {
            var visited = new Dictionary<string, bool>();
            var visitedTemporarily = new Dictionary<string, bool>();

            foreach (var job in _jobs)
            {
                visited.Add(job.Key, false);
                visitedTemporarily.Add(job.Key, false);
            }

            while (visited.Any(v => !v.Value))
            {
                var node = visited.First(v => !v.Value);
                if (Visit(node, ref visited, ref visitedTemporarily))
                {
                    return new DependenciesValidationResult
                    {
                        IsValid = false,
                        CycleSourceJobName = node.Key
                    };
                }
            }

            return new DependenciesValidationResult { IsValid = true };
        }

        private bool Visit(KeyValuePair<string, bool> node, ref Dictionary<string, bool> visited, ref Dictionary<string, bool> visitedTemporarily)
        {
            CakeJob job;
            bool visitedTemporarilyNode;
            try
            {
                job = _jobs[node.Key];
                visitedTemporarilyNode = visitedTemporarily[node.Key];
            }
            catch (KeyNotFoundException ex)
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
                    var failJob = _jobs[dependency].ExceptionJob;
                    visitedNode = !string.IsNullOrEmpty(failJob)
                        ? visited.First(v => v.Key == failJob)
                        : visited.First(v => v.Key == dependency);
                }
                catch (Exception ex) when (ex is KeyNotFoundException || ex is InvalidOperationException)
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
    }
}
