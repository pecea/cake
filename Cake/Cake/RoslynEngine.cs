namespace Cake
{
    using System.Collections.Generic;
    using System.Configuration;

    using Cake.Configuration;

    using Roslyn.Scripting;
    using Roslyn.Scripting.CSharp;

    /// <summary>
    /// Static class handling Roslyn API and providing access to some of its methods.
    /// </summary>
    public static class RoslynEngine
    {
        /// <summary>
        /// Scripting engine's session. For internal usage only.
        /// </summary>
        private static readonly Session Session;

        /// <summary>
        /// Static constructor initialising the engine and adding necessary assemblies.
        /// </summary>
        static RoslynEngine()
        {
            var engine = new ScriptEngine();
            foreach (var path in GetExternalAssembliesPaths())
            {
                engine.AddReference(path);
            }
            Session = engine.CreateSession();
        }

        /// <summary>
        /// Executes .csx script with C# code in it.
        /// </summary>
        /// <param name="file">Script's path.</param>
        public static void ExecuteFile(string file)
        {
            Session.ExecuteFile(file);
        }

        private static IEnumerable<string> GetExternalAssembliesPaths()
        {
            var configurationSection = ConfigurationManager.GetSection("assembliesSection") as AssembliesSection;
            if (configurationSection == null)
            {
                yield break; // TODO: log
            }
            foreach (var configurationElement in configurationSection.Assemblies)
            {
                var assemblyElement = configurationElement as AssemblyElement;
                if (assemblyElement == null)
                {
                    yield break; // TODO: log czy continue?
                }
                yield return assemblyElement.Path;
            }
        }
    }
}