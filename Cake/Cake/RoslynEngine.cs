using Common;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using Microsoft.CSharp;

namespace Cake
{
    /// <summary>
    /// Handles Roslyn API and providing access to some of its methods.
    /// </summary>
    public sealed class RoslynEngine
    {
        private static volatile RoslynEngine _instance;
        private static readonly List<Assembly> Assemblies = new List<Assembly>();
        private static readonly List<string> Namespaces = new List<string>();
        private static readonly object LockObject = new object();

        private RoslynEngine()
        {
            Assemblies.Add(typeof(Job).Assembly);
            Assemblies.Add(typeof(Logger).Assembly);
            Assemblies.Add(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly); //dynamic 
            Namespaces.Add(typeof(Job).Namespace);
            Namespaces.Add(typeof(JobManager).Namespace);
            Namespaces.Add(typeof(Logger).Namespace);
            Namespaces.Add(typeof(Logger).FullName);
        }
        /// <summary>
        /// Method for getting instance of RoslynEngine
        /// </summary>
        public static RoslynEngine Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (LockObject)
                    {
                        if (_instance == null)
                            _instance = new RoslynEngine();
                    }
                }

                return _instance;
            }
        }
        /// <summary>
        /// Executes .csx script with C# code in it.
        /// </summary>
        /// <param name="filePath">Script's path.</param>
        /// <returns><see cref="Task"/></returns>
        public async Task ExecuteFile(string filePath)
        {
            var assembliesPaths = GetAssembliesSearchPaths();
            var scriptDirectory = Path.GetDirectoryName(filePath);
            var metadataResolver = ScriptMetadataResolver.Default
                .WithSearchPaths(assembliesPaths)
                .WithBaseDirectory(scriptDirectory);

            Logger.Debug($"Setting script's working directory to '{scriptDirectory}'.");

            var options = ScriptOptions.Default
                .WithReferences(Assemblies)
                .WithImports(Namespaces)
                .WithMetadataResolver(metadataResolver);

            await CSharpScript.RunAsync(File.ReadAllText(filePath), options).ConfigureAwait(false);
        }

        private IEnumerable<string> GetAssembliesSearchPaths()
        {
            IList<string> gacDirectories = new List<string>();
            string winDir = Environment.GetFolderPath(Environment.SpecialFolder.Windows);

            try
            {
                gacDirectories = Directory
                    .EnumerateDirectories($@"{winDir}\Microsoft.NET\", "*", SearchOption.AllDirectories)
                    .ToList();
            }
            catch (Exception e)
            {
                Logger.LogException(LogLevel.Warn, e, "Could not enumerate GAC directories.");
            }

            string runtimeDir = RuntimeEnvironment.GetRuntimeDirectory();
            gacDirectories.Add($@"{winDir}\Microsoft.NET\");
            return gacDirectories;

        }
    }
}
