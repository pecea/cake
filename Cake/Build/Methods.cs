namespace Build
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
        /// <param name="projectFile">Project's file name or path.</param>
        /// <param name="configuration">Build configuration.</param>
        /// <param name="platform">Build platform.</param>
        /// <param name="outputPath">Build output path.</param>
        public static void BuildProject(string projectFile, string outputPath = null, string configuration = "Debug", string platform = "Any CPU")
        {
            var projectName = projectFile.Split(new[] { "\\" }, StringSplitOptions.RemoveEmptyEntries).Last();
            if (String.IsNullOrEmpty(outputPath))
            {
                outputPath = @".\bin\" + configuration;
            }

            var globalProperties = new Dictionary<string, string> { { "Configuration", configuration }, { "Platform", platform }, { "OutputPath", outputPath } };
            var buildRequest = new BuildRequestData(projectFile, globalProperties, null, new[] { "Build" }, null);
            var buildParameters = new BuildParameters(new ProjectCollection()) { Loggers = new List<ILogger>(new ILogger[] { }) };
            var buildResult = BuildManager.DefaultBuildManager.Build(buildParameters, buildRequest);

            switch (buildResult.OverallResult)
            {
                case BuildResultCode.Success:
                    Logger.Log(LogLevel.Info, String.Format("{0} built successfully.", projectName));
                    break;
                case BuildResultCode.Failure:
                    if (buildResult.Exception != null) Logger.LogException(LogLevel.Error, buildResult.Exception, String.Format("Building {0} failed.", projectName));
                    else Logger.Log(LogLevel.Error, String.Format("Building {0} failed.", projectName));
                    break;
                default:
                    throw new ArgumentOutOfRangeException("buildResult.OverallResult");
            }
        }
    }
}