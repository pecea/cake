namespace Cake
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Reflection;

    using Cake.Configuration;

    using Roslyn.Scripting;
    using Roslyn.Scripting.CSharp;

    /// <summary>
    /// Handles Roslyn API and providing access to some of its methods.
    /// </summary>
    public static class RoslynEngine
    {
        /// <summary>
        /// Scripting engine's session. 
        /// For internal usage only.
        /// </summary>
        private static readonly Session Session;

        /// <summary>
        /// Initialises the engine and adds necessary assemblies and namespaces.
        /// External assemblies are loaded from App.config file.
        /// From those assemblies all static classes are imported as namespaces containing methods to be used in the script.
        /// </summary>
        static RoslynEngine()
        {
            var engine = new ScriptEngine();
            Session = engine.CreateSession();

            Session.AddReference(Assembly.GetEntryAssembly());

            Session.ImportNamespace(typeof(Task).Namespace);
            Session.ImportNamespace(typeof(TaskFactory).FullName);

            foreach (var assembly in GetExternalAssemblies())
            {
                engine.AddReference(assembly);
                Session.AddReference(assembly);
                foreach (var type in assembly.GetTypes().Where(type => type.IsStatic()))
                {
                    Session.ImportNamespace(type.FullName);
                }
            }
        }

        /// <summary>
        /// Executes .csx script with C# code in it.
        /// </summary>
        /// <param name="file">Script's path.</param>
        public static void ExecuteFile(string file)
        {
            Session.ExecuteFile(file);
        }

        /// <summary>
        /// Goes through App.config file and loads assemblies from specified paths.
        /// </summary>
        /// <returns>A collection of needed assemblies.</returns>
        private static IEnumerable<Assembly> GetExternalAssemblies()
        {
            var configurationSection = ConfigurationManager.GetSection("assembliesSection") as AssembliesSection;
            if (configurationSection == null) yield break;

            foreach (var configurationElement in configurationSection.Assemblies)
            {
                var assemblyElement = configurationElement as AssemblyElement;
                
                if (assemblyElement == null) yield break;
                yield return Assembly.LoadFrom(assemblyElement.Path);
            }
        }

        /// <summary>
        /// Determines whether specified type is static.
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <returns>True if the type provided is static, false otherwise.</returns>
        private static bool IsStatic(this Type type)
        {
            return type.IsSealed && type.IsAbstract;
        }
    }
}