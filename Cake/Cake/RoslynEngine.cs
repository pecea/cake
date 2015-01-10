namespace Cake
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    using Common;

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
            Session = new ScriptEngine().CreateSession();

            Session.AddReference(Assembly.GetAssembly(typeof(Task)));
            Session.AddReference(Assembly.GetAssembly(typeof(Logger)));
            
            Session.ImportNamespace(typeof(Task).Namespace);
            Session.ImportNamespace(typeof(TaskManager).FullName);
            Session.ImportNamespace(typeof(Logger).Namespace);
            Session.ImportNamespace(typeof(Logger).FullName);
        }

        /// <summary>
        /// Executes .csx script with C# code in it.
        /// </summary>
        /// <param name="filePath">Script's path.</param>
        public static void ExecuteFile(string filePath)
        {
            LoadAssemblies(filePath);
            Session.ExecuteFile(filePath);
        }

        private static void LoadAssemblies(string filePath)
        {
            var assemblyRegex = new Regex(@"^//\s*cake using ""([a-zA-Z0-9\./\\-_:zżźćńółęąśŻŹĆĄŚĘŁÓŃ\s])+"";");

            using (var streamReader = new StreamReader(filePath, Encoding.GetEncoding("ISO-8859-2")))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    var match = assemblyRegex.Match(line);
                    if (!match.Success) continue;

                    var assemblyPath = ExtractAssemblyPath(match.Groups[0].Value);
                    var assembly = Assembly.LoadFrom(assemblyPath);

                    Session.AddReference(assembly);
                    Logger.Log(LogLevel.Debug, String.Format("Assembly \"{0}\" referenced. Importing namespaces from this assembly.", assembly.FullName));

                    var namespaces = assembly.GetTypes().Select(type => type.Namespace);
                    var staticTypes = assembly.GetTypes().Where(type => type.IsStatic()).Select(type => type.FullName);

                    namespaces = namespaces.Union(staticTypes);

                    foreach (var ns in namespaces)
                    {
                        Session.ImportNamespace(ns);
                        Logger.Log(LogLevel.Debug, String.Format("Namespace \"{0}\" imported.", ns));
                    }
                }
            }
        }

        private static string ExtractAssemblyPath(string usingDirective)
        {
            return usingDirective
                    .TrimStart('/')
                    .TrimEnd(';')
                    .Trim()
                    .Replace("cake using ", String.Empty)
                    .TrimStart('"')
                    .TrimEnd('"');
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