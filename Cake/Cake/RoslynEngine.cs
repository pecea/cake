using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Cake
{
    using Common;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Handles Roslyn API and providing access to some of its methods.
    /// </summary>
    public static class RoslynEngine
    {
        /// <summary>
        /// Executes .csx script with C# code in it.
        /// </summary>
        /// <param name="filePath">Script's path.</param>
        public static void ExecuteFile(string filePath)
        {
            try
            {
                //var script=CSharpScript.Create(File.ReadAllText(@"../../../scripts/script3.csx")).ContinueWith(File.ReadAllText(@"../../../scripts/script4.csx"));
                //script.RunAsync().Wait();
                CSharpScript.RunAsync(LoadReferencedScripts(filePath), LoadAssemblies(filePath)).Wait();
            }
            catch (JobException)
            {
                throw;
            }
            catch (Exception e)
            {
                Logger.LogException(LogLevel.Error, e, "An exception occured while executing the script with Roslyn\n");
                throw;
            }
        }


        private static string LoadReferencedScripts(string filePath)
        {
            var scriptRegex = new Regex(@"^//\s*cake load ""([a-zA-Z0-9\-\./\\-_:zżźćńółęąśŻŹĆĄŚĘŁÓŃ\s])+"";*");
            var otherScripts = string.Empty;
            using (var streamReader = new StreamReader(filePath, Encoding.GetEncoding("ISO-8859-2")))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    var match2 = scriptRegex.Match(line);
                    if (!match2.Success)
                        continue;
                    var scriptPath = ExtractScriptPath(match2.Groups[0].Value);
                    otherScripts = string.Concat(otherScripts, $"{File.ReadAllText(scriptPath)}\n");

                    Logger.Log(LogLevel.Debug,
                        $"Script \"{scriptPath}\" referenced. Adding the code from the referenced script to the main script.");
                }
            }
            return string.Concat(otherScripts, File.ReadAllText(filePath));
        }
        private static ScriptOptions LoadAssemblies(string filePath)
        {
            var assemblyRegex = new Regex(@"^//\s*cake using ""([a-zA-Z0-9\-\./\\-_:zżźćńółęąśŻŹĆĄŚĘŁÓŃ\s])+"";*");
            var assemblies = new List<Assembly>();
            var namespaceStrings = new List<string>();
            //var options = ScriptOptions.Default;
            using (var streamReader = new StreamReader(filePath, Encoding.GetEncoding("ISO-8859-2")))
            {
                string line;
               
                while ((line = streamReader.ReadLine()) != null)
                {
                    var match = assemblyRegex.Match(line);
                    if (!match.Success) continue;

                    var assemblyPath = ExtractAssemblyPath(match.Groups[0].Value);
                    var assembly = Assembly.LoadFrom(assemblyPath);
                    assemblies.Add(assembly);
                    //Session.AddReference(assembly);
                    //options.AddReferences(assembly);
                    Logger.Log(LogLevel.Debug,
                        $"Assembly \"{assembly.FullName}\" referenced. Importing namespaces from this assembly.");

                    var namespaces = assembly.GetTypes().Select(type => type.Namespace);
                    var staticTypes = assembly.GetTypes().Where(type => type.IsStatic()).Select(type => type.FullName);

                    namespaces = namespaces.Union(staticTypes);

                    foreach (var ns in namespaces)
                    {
                        //Session.ImportNamespace(ns);
                        namespaceStrings.Add(ns);
                        Logger.Log(LogLevel.Debug, $"Namespace \"{ns}\" imported.");
                    }
                }
            }
            assemblies.Add(typeof(Job).Assembly);
            assemblies.Add(typeof(Logger).Assembly);
            namespaceStrings.Add(typeof(Job).Namespace);
            namespaceStrings.Add(typeof(JobManager).Namespace);
            namespaceStrings.Add(typeof(Logger).Namespace);
            namespaceStrings.Add(typeof(Logger).FullName);
            return ScriptOptions.Default.WithReferences(assemblies).WithImports(namespaceStrings);
        }

        private static string ExtractAssemblyPath(string usingDirective)
        {
            return usingDirective
                    .TrimStart('/')
                    .TrimEnd(';')
                    .Trim()
                    .Replace("cake using ", string.Empty)
                    .TrimStart('"')
                    .TrimEnd('"');
        }

        private static string ExtractScriptPath(string loadDirective)
        {
            return loadDirective
                .TrimStart('/')
                .TrimEnd(';')
                .Trim()
                .Replace("cake load ", string.Empty)
                .TrimStart('"')
                .TrimEnd('"');
        }



        private static bool IsStatic(this Type type)
        {
            return type.IsSealed && type.IsAbstract;
        }
    }
}