namespace Build
{
    using System;
    using System.Collections.Generic;
    using System.IO;
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
        /// <returns>true in case of success, false otherwise.</returns>
        public static bool BuildProject(string projectFile, string outputPath = null, string configuration = "Debug", string platform = "Any CPU")
        {
            if (String.IsNullOrEmpty(outputPath)) outputPath = @".\bin\" + configuration;
            if (!CheckBuildProjectArguments(projectFile, outputPath, configuration, platform)) return false;

            var projectName = projectFile.Split(new[] { "\\", "/" }, StringSplitOptions.RemoveEmptyEntries).Last();

            var globalProperties = new Dictionary<string, string> { { "Configuration", configuration }, { "Platform", platform }, { "OutputPath", outputPath } };
            var buildRequest = new BuildRequestData(projectFile, globalProperties, null, new[] { "Build" }, null);
            var buildParameters = new BuildParameters(new ProjectCollection()) { Loggers = new List<ILogger>(new ILogger[] { }) };
            var buildResult = BuildManager.DefaultBuildManager.Build(buildParameters, buildRequest);

            if (buildResult.OverallResult == BuildResultCode.Success)
            {
                Logger.Log(LogLevel.Info, String.Format("{0} built successfully. Output files are located in {1}", projectName, Path.GetFullPath(outputPath)));
                return true;
            }
            else
            {
                if (buildResult.Exception != null) 
                    Logger.LogException(LogLevel.Error, buildResult.Exception, String.Format("Building {0} failed.", projectName));
                else 
                    Logger.Log(LogLevel.Error, String.Format("Building {0} failed.", projectName));
                return false;
            }
        }

        /// <summary>
        /// Checks if arguments passed to BuildProject methods are valid.
        /// </summary>
        /// <param name="projectFile">Project's file name or path.</param>
        /// <param name="configuration">Build configuration.</param>
        /// <param name="platform">Build platform.</param>
        /// <param name="outputPath">Build output path.</param>
        /// <returns>true in case of success, false otherwise.</returns>
        private static bool CheckBuildProjectArguments(string projectFile, string outputPath, string configuration, string platform)
        {
            if (!File.Exists(projectFile))
            {
                Logger.Log(LogLevel.Error, "The project file specified is nonexistent.");
                return false;
            }

            if (platform != "Any CPU" && platform != "x86" && platform != "x64") // TODO: te wartości z configa pls
            {
                Logger.Log(LogLevel.Error, "The platform parameter must be one of: \"Any CPU\", \"x86\", \"x64\"."); // TODO: te wartości z configa pls
                return false;
            }

            if (configuration != "Debug" && configuration != "Release") // TODO: te wartości z configa pls
            {
                Logger.Log(LogLevel.Error, "The configuration parameter must be set to \"Debug\" or \"Release\"."); // TODO: te wartości z configa pls
                return false;
            }

            try
            {
                Path.GetFullPath(outputPath);
            }
            catch (Exception)
            {
                Logger.Log(LogLevel.Error, "The outputPath parameter is not a valid path.");
                return false;
            }
            return true;
        }
    }
}