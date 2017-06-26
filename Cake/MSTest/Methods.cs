using System.Diagnostics;
using System.Xml;
using Microsoft.Win32;

namespace MSTest
{
    using System;
    using System.IO;
    using System.Linq;

    using Common;
    
    /// <summary>
    /// Encloses methods used for testing with MSTest
    /// </summary>
    public static class Methods
    {
        private const string DefaultTestResultsPath = @"TestResults";
        private static string FullPathExe => "../../../External/MsTest.exe";
        ///// <summary>
        ///// Runs mstest.exe with test assemblies.
        ///// </summary>
        ///// <param name="testAssembliesPaths">Assemblies of the tests to be run. Paths may contain wildcards.</param>
        ///// <returns>True if the tests were executed successfully, false otherwise.</returns>
        //public static bool RunTests(params string[] testAssembliesPaths)
        //{
        //    if (!File.Exists(FullPathExe))
        //    {
        //        Logger.Log(LogLevel.Error, "MSTest.exe file not found.");
        //        return false;
        //    }
        //    var result = true;

        //    foreach (var path in testAssembliesPaths)
        //    {
        //        try
        //        {
        //            var paths = path.GetFilePaths()?.ToArray();
        //            if (paths == null || paths.Length == 0)
        //            {
        //                result = false;
        //                continue;
        //            }

        //            foreach (var p in paths)
        //            {
        //                var tmp = Processor.RunProcess(FullPathExe,
        //                            $"/testcontainer:{Processor.QuoteArgument(p)}");
        //                var res = tmp ? "success" : "failure";
        //                Logger.Log(LogLevel.Info, $"Running unit tests from {p} resulted in {res}\n");
        //                result = result && tmp;
        //            }
        //            //result = enumeratedPaths.Aggregate(result,
        //            //    (current, testPath) =>
        //            //        Processor.RunProcess(FullPathExe,
        //            //                "/testcontainer:" + Processor.QuoteArgument(testPath))
        //            //            &&
        //            //            current);
        //        }
        //        catch (Exception e)
        //        {
        //            Logger.LogException(LogLevel.Error, e, "An exception occured while running mstest.exe");
        //            result = false;
        //        }
        //    }

        //    return result;
        //}

        //public static bool RunTests(string testAssemblyPath, string categories)
        //{
        //    if (!File.Exists(FullPathExe))
        //    {
        //        Logger.Log(LogLevel.Error, "MSTest.exe file not found.");
        //        return false;
        //    }
        //    var result = true;
        //    try
        //    {
        //        var paths = testAssemblyPath.GetFilePaths()?.ToArray();
        //        if (paths == null || paths.Length == 0)
        //        {
        //            Logger.Log(LogLevel.Error, $"Incorrect test assemby path!\n");
        //            return false;
        //        }

        //        foreach (var p in paths)
        //        {
        //            var tmp = Processor.RunProcess(FullPathExe,
        //                        $"/testcontainer:{Processor.QuoteArgument(p)} /category:{Processor.QuoteArgument(categories)}");
        //            var res = tmp ? "success" : "failure";
        //            Logger.Log(LogLevel.Info, $"Running unit tests from {p} resulted in {res}\n");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.LogException(LogLevel.Error, e, "An exception occured while running mstest.exe");
        //        result = false;
        //    }

        //    return result;
        //}


        public static bool RunTests(string testAssemblyPath, string categories = null, string resultFile = null, string singleTest = null, bool unique = false)
        {
            if (!File.Exists(FullPathExe))
            {
                Logger.Log(LogLevel.Error, "MSTest.exe file not found.");
                return false;
            }
            var result = true;
            try
            {
                var paths = testAssemblyPath.GetFilePaths()?.ToArray();
                if (paths == null || paths.Length == 0)
                {
                    Logger.Log(LogLevel.Error, $"Incorrect test assemby path!\n");
                    return false;
                }
                foreach (var p in paths)
                {
                    var arguments = $"/testcontainer:{Processor.QuoteArgument(p)}";
                    if (!string.IsNullOrEmpty(categories))
                        arguments += $" /category:{Processor.QuoteArgument(categories)}";
                    if (!string.IsNullOrEmpty(resultFile))
                        arguments += $" /resultsfile:{resultFile}";
                    if (!string.IsNullOrEmpty(singleTest))
                        arguments += $" /test:{singleTest}";
                    if (unique)
                        arguments += " /unique";
                    var defaultTestDir = new DirectoryInfo(DefaultTestResultsPath);
                    foreach(var dir in defaultTestDir.GetDirectories())
                        dir.Delete(true);
                    foreach (var f in defaultTestDir.GetFiles())
                        f.Delete();

                    var tmp = Processor.RunProcess(FullPathExe, arguments);
                    var res = tmp ? "success" : "failure";
                    Logger.Log(LogLevel.Info, $"Running unit tests from {p} resulted in {res}\n");
                    result &= tmp;
                }
            }
            catch (Exception e)
            {
                Logger.LogException(LogLevel.Error, e, "An exception occured while running mstest.exe");
                result = false;
            }

            return result;
        }



        public static bool RunTests(string testAssemblyPath, string resultFile = null, params string[] singleTests)
        {
            if (!File.Exists(FullPathExe))
            {
                Logger.Log(LogLevel.Error, "MSTest.exe file not found.");
                return false;
            }
            var result = true;
            try
            {
                var paths = testAssemblyPath.GetFilePaths()?.ToArray();
                if (paths == null || paths.Length == 0)
                {
                    Logger.Log(LogLevel.Error, $"Incorrect test assemby path!\n");
                    return false;
                }

                foreach (var p in paths)
                {
                    var arguments = $"/testcontainer:{Processor.QuoteArgument(p)}";
                    if (!string.IsNullOrEmpty(resultFile))
                        arguments += $" /resultsfile:{resultFile}";
                    arguments = (singleTests ?? new string[0]).Aggregate(arguments, (current, test) => current + $" /test:{test}");
                    var tmp = Processor.RunProcess(FullPathExe, arguments);
                    var res = tmp ? "success" : "failure";
                    Logger.Log(LogLevel.Info, $"Running unit tests from {p} resulted in {res}\n");
                    result &= tmp;
                }
            }
            catch (Exception e)
            {
                Logger.LogException(LogLevel.Error, e, "An exception occured while running mstest.exe");
                result = false;
            }

            return result;
        }

        //public static bool RunTests(string testAssemblyPath, string resultFile = null, bool unique = false, string singleTest = null)
        //{
        //    if (!File.Exists(FullPathExe))
        //    {
        //        Logger.Log(LogLevel.Error, "MSTest.exe file not found.");
        //        return false;
        //    }
        //    var result = true;
        //    try
        //    {
        //        var paths = testAssemblyPath.GetFilePaths()?.ToArray();
        //        if (paths == null || paths.Length == 0)
        //        {
        //            Logger.Log(LogLevel.Error, $"Incorrect test assemby path!\n");
        //            return false;
        //        }

        //        foreach (var p in paths)
        //        {
        //            var arguments = $"/testcontainer:{Processor.QuoteArgument(p)}";
        //            if (!string.IsNullOrEmpty(resultFile))
        //                arguments += $" /resultsfile:{resultFile}";
        //            if (!string.IsNullOrEmpty(singleTest))
        //                arguments += $" /test:{singleTest}";
        //            if (unique)
        //                arguments += $" /unique";
        //            var tmp = Processor.RunProcess(FullPathExe, arguments);
        //            var res = tmp ? "success" : "failure";
        //            Logger.Log(LogLevel.Info, $"Running unit tests from {p} resulted in {res}\n");
        //            result &= tmp;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.LogException(LogLevel.Error, e, "An exception occured while running mstest.exe");
        //        result = false;
        //    }

        //    return result;
        //}

        //publish tests?
    }
}
