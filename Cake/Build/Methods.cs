namespace Build
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Common;
    using Microsoft.CodeAnalysis.MSBuild;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Emit;

    /// <summary>
    /// Encloses methods used with building projects.
    /// </summary>
    public static class Methods
    {
        private static Type _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions); // hack to recognize C# as a valid language in compilation
        private static Dictionary<string, Platform> platformOptions = new Dictionary<string, Platform>()
        {
            { "x86", Platform.X86 },
            { "x64", Platform.X64 },
            { "Any CPU", Platform.AnyCpu }
            //,
            //{ "Any CPU 32", Platform.AnyCpu32BitPreferred },
            //{ "Arm", Platform.Arm },
            //{ "Itanium", Platform.Itanium }
        };

        private static Dictionary<string, OptimizationLevel> optimizationOptions = new Dictionary<string, OptimizationLevel>()
        {
            { "Debug", OptimizationLevel.Debug },
            { "Release", OptimizationLevel.Release }
        };

        private static bool CompileProject(string projectUrl, string outputDir, string configuration, string platform)
        {
            bool success = true;
            var options = new Dictionary<string, string> { { "Configuration", configuration } };
                //{"Platform", platform } };
            //MSBuildWorkspace workspace = MSBuildWorkspace.Create(new Dictionary<string, string> {
            //    { "Configuration", configuration },
            //    { "Platform", platform}
            //});
            MSBuildWorkspace workspace = MSBuildWorkspace.Create(options);
            workspace.LoadMetadataForReferencedProjects = true;
            try
            {
                Project project = workspace.OpenProjectAsync(projectUrl).Result;
                success = CompileProject(project, outputDir, configuration, platform);
                workspace.CloseSolution();
            }
            catch(Exception ex)
            {
                Logger.LogException(LogLevel.Error, ex, $"Could not build project {projectUrl}");
                success = false;
            }

            return success;
        }

        private static bool CompileProject(Project project, string outputPath, string configuration, string platform)
        {
            bool success = true;
            Compilation projectCompilation = project?.WithCompilationOptions(project?.CompilationOptions?.WithOptimizationLevel(optimizationOptions[configuration])?.WithPlatform(platformOptions[platform]))?.GetCompilationAsync()?.Result;
            //Compilation projectCompilation = project.GetCompilationAsync().Result;
            if (!string.IsNullOrEmpty(projectCompilation?.AssemblyName))
            {
                using (var stream = new MemoryStream())
                {
                    EmitResult result = projectCompilation.Emit(stream);
                    if (result.Success)
                    {
                        string fileName = string.Format("{0}.dll", projectCompilation.AssemblyName);

                        using (FileStream file = File.Create(outputPath + '\\' + fileName))
                        {
                            stream.Seek(0, SeekOrigin.Begin);
                            stream.CopyTo(file);
                        }
                    }
                    else
                    {
                        success = false;
                    }
                }
            }
            else
            {
                success = false;
            }
            return success;
        }

        private static bool CompileSolution(string solutionUrl, string outputDir, string configuration, string platform)
        {
            bool success = true;
            MSBuildWorkspace workspace = MSBuildWorkspace.Create(new Dictionary<string, string> {
                { "Configuration", configuration },
                { "Platform", platform}
            });
            workspace.LoadMetadataForReferencedProjects = true;
            try
            {
                Solution solution = workspace.OpenSolutionAsync(solutionUrl).Result;
                ProjectDependencyGraph projectGraph = solution.GetProjectDependencyGraph();
                //Dictionary<string, Stream> assemblies = new Dictionary<string, Stream>();

                foreach (ProjectId projectId in projectGraph.GetTopologicallySortedProjects())
                    success &= CompileProject(solution.GetProject(projectId), outputDir, configuration, platform);
                workspace.CloseSolution();
            }
            catch (Exception ex)
            {
                Logger.LogException(LogLevel.Error, ex, $"Could not build solution {solutionUrl}");
                success = false;
            }
            return success;
        }

        /// <summary>
        /// Builds one or more solutions.
        /// </summary>
        /// <param name="solutionFile">Solution's file name or path. Might contain wildcards.</param>
        /// <param name="configuration">Build configuration.</param>
        /// <param name="platform">Build platform.</param>
        /// <param name="outputPath">Build output path.</param>
        /// <returns>true in case of success, false otherwise.</returns>
        public static bool BuildSolution(string solutionFile, string outputPath = null, string configuration = "Debug", string platform = "Any CPU")
        {
            Logger.Log(LogLevel.Trace, "Method started");

            var paths = solutionFile.GetFilePaths();
            var enumerable = paths as IList<string> ?? paths.ToList();
            if (!enumerable.Any()) return false;

            if (string.IsNullOrEmpty(outputPath)) outputPath = @".\bin\" + configuration;
            if (!CheckBuildProjectArguments(solutionFile, outputPath, configuration, platform)) return false;

            return enumerable.Aggregate(true,
                (current, path) => current & CompileSolution(path, outputPath, configuration, platform));
        }

        /// <summary>
        /// Builds one or more projects.
        /// </summary>
        /// <param name="projectFile">Project's file name or path. Might contain wildcards.</param>
        /// <param name="configuration">Build configuration.</param>
        /// <param name="platform">Build platform.</param>
        /// <param name="outputPath">Build output path.</param>
        /// <returns>true in case of success, false otherwise.</returns>
        public static bool BuildProject(string projectFile, string outputPath = null, string configuration = "Debug", string platform = "Any CPU")
        {
            //var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);
            Logger.Log(LogLevel.Trace, "Method started");
            var paths = projectFile.GetFilePaths();

            var enumerable = paths as IList<string> ?? paths.ToList();
            if (!enumerable.Any()) return false;

            if (string.IsNullOrEmpty(outputPath)) outputPath = @".\bin\" + configuration;
            if (!CheckBuildProjectArguments(projectFile, outputPath, configuration, platform)) return false;

            return enumerable.Aggregate(true,
                (current, path) => current & CompileProject(path, outputPath, configuration, platform));
        }

        //private static bool BuildSingleProject(string projectFile, string outputPath, string configuration, string platform)
        //{
        //    Logger.Log(LogLevel.Trace, "Method started");
        //    if (string.IsNullOrEmpty(outputPath)) outputPath = @".\bin\" + configuration;
        //    if (!CheckBuildProjectArguments(projectFile, outputPath, configuration, platform)) return false;

        //    bool success = BuildSolution(projectFile, outputPath, configuration, platform);
        //    return success;

        //    //var projectName = projectFile.Split(new[] { "\\", "/" }, StringSplitOptions.RemoveEmptyEntries).Last();

        //    //var globalProperties = new Dictionary<string, string> { { "Configuration", configuration }, { "Platform", platform }, { "OutputPath", outputPath } };
        //    //var buildRequestData = new BuildRequestData(projectFile, globalProperties, "15.0", new[] { "Build" }, null);
        //    //var logger = new ConsoleLogger
        //    //{
        //    //    //Parameters = new[]{ "D:/Dane/Ernest/Praca/buildLog.txt" },
        //    //    Verbosity = LoggerVerbosity.Minimal,
        //    //    ShowSummary = true
        //    //};
        //    //var buildParameters = new BuildParameters(new ProjectCollection() { SkipEvaluation = true })
        //    //{
        //    //    Loggers = new[] { logger },

        //    //};
        //    //var buildResult = BuildManager.DefaultBuildManager.Build(buildParameters, buildRequestData);

        //    //if (buildResult.OverallResult == BuildResultCode.Success)
        //    //{
        //    //    Logger.Log(LogLevel.Info,
        //    //        $"{projectName} built successfully. Output files are located in {Path.GetFullPath(outputPath)}");
        //    //    return true;
        //    //}
        //    //if (buildResult.Exception != null)
        //    //    Logger.LogException(LogLevel.Error, buildResult.Exception, $"Building {projectName} failed.");
        //    //else
        //    //    Logger.Log(LogLevel.Warn, $"Building {projectName} failed.");
        //    //return false;
        //}

        private static bool CheckBuildProjectArguments(string projectFile, string outputPath, string configuration, string platform)
        {
            Logger.Log(LogLevel.Trace, "Method started");
            if (!File.Exists(projectFile))
            {
                Logger.Log(LogLevel.Warn, "The project file specified is nonexistent.");
                return false;
            }
            if(!platformOptions.ContainsKey(platform))
            {
                Logger.Log(LogLevel.Warn, $"The platform parameter must be one of: {string.Join(", ", platformOptions.Select(k => k.Key))}");
                return false;
            }

            if (!optimizationOptions.ContainsKey(configuration))
            {
                Logger.Log(LogLevel.Warn, "The configuration parameter must be set to \"Debug\" or \"Release\".");
                return false;
            }
            if (!Directory.Exists(outputPath))
            {
                try
                {
                    Directory.CreateDirectory(outputPath);
                }
                catch(Exception ex)
                {
                    Logger.LogException(LogLevel.Error, ex, $"Could not create output folder: {outputPath}");
                    return false;
                }
            }
            return true;
        }
    }
}