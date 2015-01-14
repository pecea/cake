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
        /// Builds one or more projects or solutions.
        /// </summary>
        /// <param name="projectFile">Project's file name or path. Might contain wildcards.</param>
        /// <param name="configuration">Build configuration.</param>
        /// <param name="platform">Build platform.</param>
        /// <param name="outputPath">Build output path.</param>
        /// <returns>true in case of success, false otherwise.</returns>
        public static bool BuildProject(string projectFile, string outputPath = null, string configuration = "Debug", string platform = "Any CPU")
        {
            var paths = projectFile.GetFilePaths();

            if (paths == null || !paths.Any()) return false;

            return paths.Aggregate(true,
                (current, path) => current & BuildSingleProject(path, outputPath, configuration, platform));
        }

        private static bool BuildSingleProject(string projectFile, string outputPath, string configuration, string platform)
        {
            if (String.IsNullOrEmpty(outputPath)) outputPath = @".\bin\" + configuration;
            if (!CheckBuildProjectArguments(projectFile, outputPath, configuration, platform)) return false;

            var projectName = projectFile.Split(new[] { "\\", "/" }, StringSplitOptions.RemoveEmptyEntries).Last();

            var globalProperties = new Dictionary<string, string> { { "Configuration", configuration }, { "Platform", platform }, { "OutputPath", outputPath } };
            var buildRequestData = new BuildRequestData(projectFile, globalProperties, null, new[] { "Build" }, null);
            var buildParameters = new BuildParameters(new ProjectCollection()) { Loggers = new List<ILogger>(new ILogger[] { }) };
            var buildResult = BuildManager.DefaultBuildManager.Build(buildParameters, buildRequestData);

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

        private static bool CheckBuildProjectArguments(string projectFile, string outputPath, string configuration, string platform)
        {
            if (!File.Exists(projectFile))
            {
                Logger.Log(LogLevel.Error, "The project file specified is nonexistent.");
                return false;
            }

            if (platform != "Any CPU" && platform != "x86" && platform != "x64")
            {
                Logger.Log(LogLevel.Error, "The platform parameter must be one of: \"Any CPU\", \"x86\", \"x64\".");
                return false;
            }

            if (configuration != "Debug" && configuration != "Release")
            {
                Logger.Log(LogLevel.Error, "The configuration parameter must be set to \"Debug\" or \"Release\".");
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