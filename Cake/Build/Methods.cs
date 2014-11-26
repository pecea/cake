namespace Build
{
    using System;
    using System.Collections.Generic;

    using Common;

    using Microsoft.Build.Evaluation;
    using Microsoft.Build.Execution;
    using Microsoft.Build.Framework;

    /// <summary>
    /// Encloses methods used with building projects.
    /// </summary>
    public static class Methods
    {
        /// <summary>
        /// Builds a single project or a solution.
        /// </summary>
        /// <param name="projectFileName">Project's file name or path.</param>
        /// <param name="configuration">Build configuration.</param>
        /// <param name="platform">Build platform.</param>
        /// <param name="outputPath">Build output path.</param>
        public static void BuildProject(string projectFileName, string outputPath = null, string configuration = "Debug", string platform = "Any CPU")
        {
            if (String.IsNullOrEmpty(outputPath))
            {
                outputPath = @".\bin\" + configuration;
            }
            var globalProperties = new Dictionary<string, string> { { "Configuration", configuration }, { "Platform", platform }, { "OutputPath", outputPath } };
            var buildRequest = new BuildRequestData(projectFileName, globalProperties, null, new[] { "Build" }, null);
            var buildParameters = new BuildParameters(new ProjectCollection()) { Loggers = new List<ILogger>(new ILogger[] { }) };
            var buildResult = BuildManager.DefaultBuildManager.Build(buildParameters, buildRequest);
            Logger.Info(buildResult.OverallResult.ToString());
        }
    }
}