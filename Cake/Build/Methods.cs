using Build.Extensions;
using Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;

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
            //,
            //{ "Any CPU 32", Platform.AnyCpu32BitPreferred },
            //{ "Arm", Platform.Arm },
            //{ "Itanium", Platform.Itanium }
        };

        private static readonly Dictionary<string, OptimizationLevel> OptimizationOptions = new Dictionary<string, OptimizationLevel>
        {
            { "Debug", OptimizationLevel.Debug },
            { "Release", OptimizationLevel.Release }
        };


        internal static bool TryGetOutputKind(string outputKind, out OutputKind kind)
        {
            if (string.Equals(outputKind, "Library", StringComparison.OrdinalIgnoreCase))
            {
                kind = OutputKind.DynamicallyLinkedLibrary;
                return true;
            }
            else if (string.Equals(outputKind, "Exe", StringComparison.OrdinalIgnoreCase))
            {
                kind = OutputKind.ConsoleApplication;
                return true;
            }
            else if (string.Equals(outputKind, "WinExe", StringComparison.OrdinalIgnoreCase))
            {
                kind = OutputKind.WindowsApplication;
                return true;
            }
            else if (string.Equals(outputKind, "Module", StringComparison.OrdinalIgnoreCase))
            {
                kind = OutputKind.NetModule;
                return true;
            }
            else if (string.Equals(outputKind, "WinMDObj", StringComparison.OrdinalIgnoreCase))
            {
                kind = OutputKind.WindowsRuntimeMetadata;
                return true;
            }
            else
            {
                kind = OutputKind.DynamicallyLinkedLibrary;
                return false;
            }
        }

        private static async Task<bool> CompileProjectAsync(string projectUrl, string outputDir, string configuration, string platform)
        {
            Logger.LogMethodStart();
            bool success;
            var options = new Dictionary<string, string> { { "Configuration", configuration } };
            var workspace = MSBuildWorkspace.Create(options);

            var project = await workspace.OpenProjectAsync(projectUrl).ConfigureAwait(false);
            success = await CompileProjectAsync(project, outputDir, configuration, platform).ConfigureAwait(false);

            Logger.Log(LogLevel.Info, $"Project {projectUrl} compilation finished.");
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
            bool success = false;
            Logger.LogMethodStart();
            var doc = new XmlDocument();
            doc.Load(project.FilePath);
            var outputType = doc.GetElementById("OutputType");
            var output = doc.GetElementsByTagName("OutputType").OfType<XmlNode>();
            var type = output.FirstOrDefault()?.InnerText;
            var regognizedKind = TryGetOutputKind(type, out OutputKind kind);

            var coreDir = Path.GetDirectoryName(typeof(object).GetTypeInfo().Assembly.Location);
            var mscorlib = MetadataReference.CreateFromFile(Path.Combine(coreDir, "mscorlib.dll"));

            var opts = project.CompilationOptions
                .WithOutputKind(kind)
                .WithOptimizationLevel(OptimizationOptions[configuration])
                .WithPlatform(PlatformOptions[platform]);

            var projectCompilation = await project
                .WithMetadataReferences(new [] { mscorlib })
                .WithCompilationOptions(opts)
                .GetCompilationAsync()
                .ConfigureAwait(false);

            if (string.IsNullOrEmpty(outputPath))
                outputPath = $"{Path.GetDirectoryName(string.IsNullOrEmpty(project?.OutputFilePath) ? Path.Combine(Path.GetDirectoryName(project?.FilePath), "bin\\" + configuration) : project?.OutputFilePath)}\\";
            outputPath = outputPath.Replace('/', '\\');
            if (!outputPath.EndsWith("\\"))
                outputPath += '\\';
            if (graph != null)
            {
                outputPath += $"{project?.Name}\\";
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

                if (result != null && result.Success)
                {
                    Logger.Log(LogLevel.Info, $"Project {project.Name} compiled successfully.");
                    var pathOne = $"{outputPath}{projectCompilation.AssemblyName}{ProjectOutputs[kind]}";
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
                    throw new CompilationException(project?.Name);
                }

                success = result?.Success ?? false;
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
            //var dllLibrary = new Dictionary<ProjectId, FileStream>();
            var library = new Dictionary<ProjectId, BuildResult>();
            foreach (var project in projectGraph.GetTopologicallySortedProjects())
            {
                success &= await CompileProjectAsync(solution.GetProject(project), outputDir, configuration, platform).ConfigureAwait(false);
            }
            //success = projectGraph.GetTopologicallySortedProjects().Aggregate(true, (current, projectId) => current & CompileProjectAsync(solution.GetProject(projectId), outputDir, configuration, platform, projectGraph, library).Result);
            workspace.CloseSolution();

            Logger.LogMethodEnd();
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

        private static bool CheckBuildProjectArguments(string projectFile, string outputPath, string configuration, string platform)
        {
            Logger.LogMethodStart();
            if (!File.Exists(projectFile))
            {
                Logger.Log(LogLevel.Warn, "The project file specified is nonexistent.");
                return false;
            }
            if (!PlatformOptions.ContainsKey(platform))
            {
                Logger.Log(LogLevel.Warn, $"The platform parameter must be one of: {string.Join(", ", PlatformOptions.Select(k => k.Key))}.");
                return false;
            }

            if (!OptimizationOptions.ContainsKey(configuration))
            {
                Logger.Log(LogLevel.Warn, "The configuration parameter must be set to \"Debug\" or \"Release\".");
                return false;
            }
            if (!string.IsNullOrEmpty(outputPath) && !Directory.Exists(outputPath))
            {
                try
                {
                    Directory.CreateDirectory(outputPath);
                }
                catch (Exception ex)
                {
                    Logger.LogException(LogLevel.Error, ex, $"Could not create output folder: {outputPath}.");
                    return false;
                }
            }
            Logger.LogMethodEnd();
            return true;
        }
    }
}