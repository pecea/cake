using Common;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
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
            try
            {
                await CSharpScript.RunAsync(File.ReadAllText(filePath), ScriptOptions.Default
                    .WithReferences(Assemblies).WithImports(Namespaces)).ConfigureAwait(false);
            }
            catch (CompilationErrorException ce)
            {
                throw new JobException($"Error inside the script: {ce.Message}.", ce.Source);
            }
            catch (AggregateException ae)
            {
                if (ae.InnerException != null)
                    throw ae.InnerException;
            }
        }
    }
}
//    /// <summary>
//    /// Handles Roslyn API and providing access to some of its methods.
//    /// </summary>
//    public static class RoslynEngine
//    {
//        private static readonly List<Assembly> assemblies;
//        private static readonly List<string> namespaces;

//        static RoslynEngine()
//        {
//            assemblies = new List<Assembly>();
//            namespaces = new List<string>();
//            assemblies.Add(typeof(Job).Assembly);
//            assemblies.Add(typeof(Logger).Assembly);
//            namespaces.Add(typeof(Job).Namespace);
//            namespaces.Add(typeof(JobManager).Namespace);
//            namespaces.Add(typeof(Logger).Namespace);
//            namespaces.Add(typeof(Logger).FullName);
//        }
//        /// <summary>
//        /// Executes .csx script with C# code in it.
//        /// </summary>
//        /// <param name="filePath">Script's path.</param>
//        public static async Task ExecuteFile(string filePath)
//        {
//            try
//            {
//                await CSharpScript.RunAsync(File.ReadAllText(filePath), ScriptOptions.Default
//                    .WithReferences(assemblies).WithImports(namespaces)).ConfigureAwait(false);
//            }
//            catch (CompilationErrorException ce)
//            {
//                throw new JobException($"Error inside the script: {ce.Message}.", ce.Source);
//            }
//            catch (AggregateException ae)
//            {
//                if (ae.InnerException != null)
//                    throw ae.InnerException;
//            }
//        }


//        //private static string LoadReferencedScripts(string filePath)
//        //{
//        //    Logger.Log(LogLevel.Trace, "Loading referenced scripts ...");
//        //    var scriptRegex = new Regex(@"^//\s*cake load ""([a-zA-Z0-9\-\./\\-_:zżźćńółęąśŻŹĆĄŚĘŁÓŃ\s])+"";*");
//        //    var otherScripts = string.Empty;
//        //    using (var streamReader = new StreamReader(filePath, Encoding.GetEncoding("ISO-8859-2")))
//        //    {
//        //        string line;
//        //        while ((line = streamReader.ReadLine()) != null)
//        //        {
//        //            var match2 = scriptRegex.Match(line);
//        //            if (!match2.Success)
//        //                continue;
//        //            var scriptPath = ExtractScriptPath(match2.Groups[0].Value);
//        //            otherScripts = string.Concat(otherScripts, $"{File.ReadAllText(scriptPath)}\n");

//        //            Logger.Log(LogLevel.Debug,
//        //                $"Script \"{scriptPath}\" referenced. Adding the code from the referenced script to the main script.");
//        //        }
//        //    }
//        //    return string.Concat(otherScripts, File.ReadAllText(filePath));
//        //}
//        //private static ScriptOptions LoadAssemblies(string filePath)
//        //{
//        //    Logger.Log(LogLevel.Trace, "Loading default assemblies ...");
//        //    //var assemblyRegex = new Regex(@"^//\s*cake using ""([a-zA-Z0-9\-\./\\-_:zżźćńółęąśŻŹĆĄŚĘŁÓŃ\s])+"";*");
//        //    var assemblies = new List<Assembly>();
//        //    var namespaceStrings = new List<string>();
//        //    //var options = ScriptOptions.Default;
//        //    //using (var streamReader = new StreamReader(filePath, Encoding.GetEncoding("ISO-8859-2")))
//        //    //{
//        //    //    string line;
               
//        //    //    while ((line = streamReader.ReadLine()) != null)
//        //    //    {
//        //    //        var match = assemblyRegex.Match(line);
//        //    //        if (!match.Success) continue;

//        //    //        var assemblyPath = ExtractAssemblyPath(match.Groups[0].Value);
//        //    //        var assembly = Assembly.LoadFrom(assemblyPath);
//        //    //        assemblies.Add(assembly);
//        //    //        Logger.Log(LogLevel.Debug,
//        //    //            $"Assembly \"{assembly.FullName}\" referenced. Importing namespaces from this assembly.");

//        //    //        var namespaces = assembly.GetTypes().Where(t => t.IsPublic).Select(type => type.Namespace);
//        //    //        var staticTypes = assembly.GetTypes().Where(type => type.IsStatic() && type.IsPublic).Select(type => type.FullName);

//        //    //        namespaces = namespaces.Union(staticTypes).Where(ns => !string.IsNullOrWhiteSpace(ns));

//        //    //        foreach (var ns in namespaces)
//        //    //        {
//        //    //            namespaceStrings.Add(ns);
//        //    //            Logger.Log(LogLevel.Debug, $"Namespace \"{ns}\" imported.");
//        //    //        }
//        //    //    }
//        //    //}
//        //    assemblies.Add(typeof(Job).Assembly);
//        //    assemblies.Add(typeof(Logger).Assembly);
//        //    namespaceStrings.Add(typeof(Job).Namespace);
//        //    namespaceStrings.Add(typeof(JobManager).Namespace);
//        //    namespaceStrings.Add(typeof(Logger).Namespace);
//        //    namespaceStrings.Add(typeof(Logger).FullName);
//        //    return ScriptOptions.Default.WithReferences(assemblies).WithImports(namespaceStrings);
//        //}

//        //private static string ExtractAssemblyPath(string usingDirective)
//        //{
//        //    Logger.LogMethodStart();
//        //    return usingDirective
//        //            .TrimStart('/')
//        //            .TrimEnd(';')
//        //            .Trim()
//        //            .Replace("cake using ", string.Empty)
//        //            .TrimStart('"')
//        //            .TrimEnd('"');
//        //}

//        //private static string ExtractScriptPath(string loadDirective)
//        //{
//        //    Logger.LogMethodStart();
//        //    return loadDirective
//        //        .TrimStart('/')
//        //        .TrimEnd(';')
//        //        .Trim()
//        //        .Replace("cake load ", string.Empty)
//        //        .TrimStart('"')
//        //        .TrimEnd('"');
//        //}



//        //private static bool IsStatic(this Type type)
//        //{
//        //    Logger.Log(LogLevel.Trace, "IsStatic method started.");
//        //    return type.IsSealed && type.IsAbstract;
//        //}
//    //}
//}