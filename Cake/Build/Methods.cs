using Build.Extensions;
using Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Build
{
    /// <summary>
    /// Encloses methods used with building projects.
    /// </summary>
    public static class Methods
    {
        private static Type _ = typeof(CSharpFormattingOptions); // hack to recognize C# as a valid language in compilation

        private static readonly Dictionary<OutputKind, string> ProjectOutputs = new Dictionary<OutputKind, string>
        {
            {OutputKind.DynamicallyLinkedLibrary, ".dll"},
            {OutputKind.ConsoleApplication,".exe" },
            {OutputKind.WindowsApplication, ".exe"},
            {OutputKind.WindowsRuntimeApplication, ".exe"},
            {OutputKind.NetModule, ".netmodule"},
            {OutputKind.WindowsRuntimeMetadata, ".winmdobj"}
        };

        private static readonly Dictionary<string, Platform> PlatformOptions = new Dictionary<string, Platform>
        {
            { "x86", Platform.X86 },
            { "x64", Platform.X64 },
            { "Any CPU", Platform.AnyCpu }
        };

        private static readonly Dictionary<string, OptimizationLevel> OptimizationOptions = new Dictionary<string, OptimizationLevel>
        {
            { "Debug", OptimizationLevel.Debug },
            { "Release", OptimizationLevel.Release }
        };

        private static async Task<bool> CompileProjectAsync(string projectUrl, string outputDir, string configuration, string platform)
        {
            Logger.LogMethodStart();
            var options = new Dictionary<string, string> { { "Configuration", configuration } };
            var workspace = MSBuildWorkspace.Create(options);

            var project = await workspace.OpenProjectAsync(projectUrl).ConfigureAwait(false);
            var success = await CompileProjectAsync(project, outputDir, configuration, platform).ConfigureAwait(false);
            
            workspace.CloseSolution();

            Logger.LogMethodEnd();
            return success;
        }

        private static void WriteStreamToFile(Stream stream, string fileName)
        {
            using (var file = File.Create(fileName))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(file);
            }
            stream.Close();
        }

        private static async Task<bool> CompileProjectAsync(Project project, string outputPath, string configuration, string platform, ProjectDependencyGraph graph = null, Dictionary<ProjectId, BuildResult> library = null)
        {
            var success = false;
            Logger.LogMethodStart();

            var opts = project.CompilationOptions
                .WithOptimizationLevel(OptimizationOptions[configuration])
                .WithPlatform(PlatformOptions[platform]);

            if (!project.MetadataReferences.Any(mref => mref.Display.Contains("mscorlib")))
            {
                var mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
                project = project.AddMetadataReference(mscorlib);
            }

            var projectCompilation = await project
                .WithMetadataReferences(project.MetadataReferences)
                .WithCompilationOptions(opts)
                .GetCompilationAsync()
                .ConfigureAwait(false);

            if (string.IsNullOrEmpty(outputPath))
                outputPath = $"{Path.GetDirectoryName(string.IsNullOrEmpty(project.OutputFilePath) ? Path.Combine(Path.GetDirectoryName(project.FilePath) ?? "", "bin\\" + configuration) : project.OutputFilePath)}\\";
            outputPath = outputPath.Replace('/', '\\');
            if (!outputPath.EndsWith("\\"))
                outputPath += '\\';
            if (graph != null)
            {
                outputPath += $"{project.Name}\\";
                if (!Directory.Exists(outputPath))
                    Directory.CreateDirectory(outputPath);
            }
            if (!string.IsNullOrEmpty(projectCompilation?.AssemblyName))
            {
                var stream = new MemoryStream();
                var stream2 = new MemoryStream();
                var stream3 = new MemoryStream();
                //var newResult = projectCompilation.Emit("test.exe", "test.pdb", "test");
                var result = projectCompilation.Emit(stream, stream2, stream3);

                if (result.Diagnostics != null)
                {
                    foreach (var diagnostic in result.Diagnostics)
                    {
                        Logger.Log(diagnostic.Severity.ToLogLevel(), diagnostic.ToString());
                    }
                }

                if (result.Success)
                {
                    Logger.Log(LogLevel.Info, $"Project {project.Name} compiled successfully to '{Path.GetFullPath(outputPath)}'.");
                    var extension = ProjectOutputs[project.CompilationOptions.OutputKind];
                    var pathOne = $"{outputPath}{projectCompilation.AssemblyName}{extension}";
                    var pathTwo = $"{outputPath}{projectCompilation.AssemblyName}.pdb";
                    var pathThree = $"{outputPath}{projectCompilation.AssemblyName}.xml";
                    WriteStreamToFile(stream, pathOne);
                    WriteStreamToFile(stream2, pathTwo);
                    WriteStreamToFile(stream3, pathThree);
                    if (library != null)
                    {
                        library[project.Id] = new BuildResult
                        {
                            OutputPath = pathOne,
                            PdbPath = pathTwo,
                            XmlPath = pathThree
                        };
                        foreach (var dependency in graph?.GetProjectsThatThisProjectTransitivelyDependsOn(project.Id)?.ToArray() ?? new ProjectId[0])
                        {
                            File.Copy(library[dependency].OutputPath, $"{outputPath}{library[dependency]?.OutputPath?.Split('\\').LastOrDefault()}", true);
                            File.Copy(library[dependency].PdbPath, $"{outputPath}{library[dependency]?.PdbPath?.Split('\\').LastOrDefault()}", true);
                            File.Copy(library[dependency].XmlPath, $"{outputPath}{library[dependency]?.XmlPath?.Split('\\').LastOrDefault()}", true);
                        }
                    }

                    foreach (var met in project.MetadataReferences)
                    {
                        File.Copy(met.Display, $"{outputPath}{met.Display?.Split('\\').LastOrDefault()}", true);
                    }
                }
                else
                {
                    throw new CompilationException(project.Name);
                }

                success = result.Success;
            }

            Logger.LogMethodEnd();
            return success;
        }

        private static async Task<bool> CompileSolutionAsync(string solutionUrl, string outputDir, string configuration, string platform)
        {
            Logger.LogMethodStart();
            var success = true;
            var options = new Dictionary<string, string> { { "Configuration", configuration } };
            var workspace = MSBuildWorkspace.Create(options);

            var solution = await workspace.OpenSolutionAsync(solutionUrl).ConfigureAwait(false);
            var projectGraph = solution.GetProjectDependencyGraph();
            foreach (var project in projectGraph.GetTopologicallySortedProjects())
            {
                success &= await CompileProjectAsync(solution.GetProject(project), outputDir, configuration, platform).ConfigureAwait(false);
            }
            //success = projectGraph.GetTopologicallySortedProjects().Aggregate(true, (current, projectId) => current & CompileProjectAsync(solution.GetProject(projectId), outputDir, configuration, platform, projectGraph, library).Result);
            workspace.CloseSolution();

            Logger.LogMethodEnd();
            return success;
        }

        private static bool CheckBuildProjectArguments(string projectFile, string outputPath, string configuration, string platform)
        {
            Logger.LogMethodStart();

            if (!File.Exists(projectFile))
                throw new FileNotFoundException($"Could not find the project file ({Path.GetFullPath(projectFile)}).");

            if (!PlatformOptions.ContainsKey(platform))
                throw new ArgumentException($"Invalid platform specified (\"{platform}\"). The platform parameter must be one of: {string.Join(", ", PlatformOptions.Select(k => k.Key))}.", nameof(platform));
            
            if (!OptimizationOptions.ContainsKey(configuration))
                throw new ArgumentException($"Invalid configuration specified (\"{configuration}\"). The configuration parameter must be one of: {string.Join(", ", OptimizationOptions.Select(k => k.Key))}.", nameof(configuration));
            
            if (!string.IsNullOrEmpty(outputPath) && !Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);            

            Logger.LogMethodEnd();
            return true;
        }

        /// <summary>
        /// Builds one or more solutions.
        /// </summary>
        /// <param name="solutionFile">Solution's file name or path. Might contain wildcards.</param>
        /// <param name="configuration">Build configuration.</param>
        /// <param name="platform">Build platform.</param>
        /// <param name="outputPath">Build output path.</param>
        /// <returns>true in case of success, false otherwise.</returns>
        public static async Task<bool> BuildSolutionAsync(string solutionFile, string outputPath = null, string configuration = "Debug", string platform = "Any CPU")
        {
            Logger.LogMethodStart();

            if (!CheckBuildProjectArguments(solutionFile, outputPath, configuration, platform)) return false;
            var res = await CompileSolutionAsync(solutionFile, outputPath, configuration, platform).ConfigureAwait(false);

            Logger.LogMethodEnd();
            return res;
        }

        /// <summary>
        /// Builds one or more projects.
        /// </summary>
        /// <param name="projectFile">Project's file name or path. Might contain wildcards.</param>
        /// <param name="configuration">Build configuration.</param>
        /// <param name="platform">Build platform.</param>
        /// <param name="outputPath">Build output path.</param>
        /// <returns>true in case of success, false otherwise.</returns>
        public static async Task<bool> BuildProjectAsync(string projectFile, string outputPath = null, string configuration = "Debug", string platform = "Any CPU")
        {
            Logger.LogMethodStart();

            if (!CheckBuildProjectArguments(projectFile, outputPath, configuration, platform)) return false;
            var res = await CompileProjectAsync(projectFile, outputPath, configuration, platform).ConfigureAwait(false);

            Logger.LogMethodEnd();
            return res;
        }
    }
}