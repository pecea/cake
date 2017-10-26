using Common;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

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
            //_assemblies = new List<Assembly>();
            //_namespaces = new List<string>();
            Assemblies.Add(typeof(Job).Assembly);
            Assemblies.Add(typeof(Logger).Assembly);
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
        public async Task ExecuteFile(string filePath)
        {
            await CSharpScript.RunAsync(File.ReadAllText(filePath), ScriptOptions.Default
                .WithReferences(Assemblies).WithImports(Namespaces)).ConfigureAwait(false);
        }
    }
}