using System;
using System.Configuration;
using System.IO;
using System.Linq;
using Common;

namespace XUnit
{
    /// <summary>
    /// Encloses methods used with running unit tests written in xUnit.
    /// </summary>
    public class Methods
    {
        private static string FullPathExe => ConfigurationManager.AppSettings["XUnitPath"];
        private const string TestsPassed = "Errors: 0, Failed: 0";
        /// <summary>
        /// Runs XUnit unit tests from the speciffied assemblyPath
        /// </summary>
        /// <param name="assemblyPaths">Paths to .dlls with XUnit unit tests</param>
        /// <param name="traits">Attirubtes on test methods in a dictionary form of names and values</param>
        /// <param name="notraits">Attributes on test methods must not be a dictionary in a form of names and values</param>
        /// <returns>True if xunit tests run successfully, otherwise false</returns>
        public static bool RunTests(string traits = null, string notraits = null, params string[] assemblyPaths)
        {
            Logger.LogMethodStart();
            var res = true;
            if (!File.Exists(FullPathExe))
            {
                Logger.Log(LogLevel.Warn, "xunit.console.exe file not found.");
                return false;
            }

            if (assemblyPaths.Any(ass => string.IsNullOrEmpty(ass) || !File.Exists(ass)))
            {
                Logger.Log(LogLevel.Warn, "Incorrect test assemby paths!\n");
                return false;
            }

            var parameters = assemblyPaths.Aggregate((current, path) => current + $" {Processor.QuoteArgument(path)}");
            if (!string.IsNullOrEmpty(traits))
                parameters = traits.Split(',').Select(t => t.Trim()).Aggregate(parameters, (current, trait) => current + $" -trait {Processor.QuoteArgument(trait)} ");
            if (!string.IsNullOrEmpty(notraits))
                parameters = notraits.Split(',').Select(nt => nt.Trim()).Aggregate(parameters, (current, notrait) => current + $" -notrait {Processor.QuoteArgument(notrait)}");
            parameters += " -nologo";

            var result = Processor.RunProcess(FullPathExe, parameters);

            if (!string.IsNullOrEmpty(result.Output))
                res = result.Output.Contains(TestsPassed);

            Logger.LogMethodEnd();
            return res;
        }
        /// <summary>
        /// Runs XUnit tests from the specified assemblyPath with different options
        /// </summary>
        /// <param name="assemblyPaths">Paths to .dlls with XUnit unit tests separated by commas</param>
        /// <param name="traits">Attirubtes on test methods must be a dictionary in a form of names and values</param> 
        /// <param name="notraits">Attributes on test methods must not be a dictionary in a form of names and values</param>
        /// <param name="methodname">Fully specified methodname of a test method - namespace, classname and methodname</param>
        /// <param name="classname">Fully specified classname of a test class - namespace and classname</param>
        /// <param name="parallel">Sets parallelization. Possible values: none - turn off all parallelization, collections - only parallelize collections, assemblies - only parallelize assemblies, all - parallelize assemblies & collections</param>
        /// <param name="maxthreads">Maximum thread count for collection parallelization. Possible values: 0 - run with unbounded thread count, >0 - limit task thread pool size to 'count'</param>
        /// <param name="noshadow">Flag indicating whether to shadow copy assemblies</param>
        /// <param name="quiet">Flag indicating whether to show progress messages</param>
        /// <param name="serialize">Flag indicating whether to serialize all test cases - for diagnostic purposes only</param>
        /// <param name="outputTypeAndName">Option to specify output type and filename. Possible values: xml filename - xUnit.net v2 style XML file, xmlv1 filename - xUnit.net v1 style XML file, nunit filename - NUnit-style XML file, html filename - HTML file</param>
        /// <returns>True if xunit tests run successfully, otherwise false</returns>

        public static bool RunTestsWithOptions(string assemblyPaths, string traits = null, string notraits = null, string methodname = null, string classname = null,
            string parallel = null, int? maxthreads = null, bool? noshadow = null, bool? quiet = null, bool? serialize = null, string outputTypeAndName = null)
        {
            Logger.LogMethodStart();
            var res = true;
            if (!File.Exists(FullPathExe))
            {
                Logger.Log(LogLevel.Warn, "xunit.console.exe file not found.");
                return false;
            }

            var paths = assemblyPaths.Split(',').Select(ass => ass.Trim()).ToArray();
            if (paths.Any(ass => string.IsNullOrEmpty(ass) || !File.Exists(ass)))
            {
                Logger.Log(LogLevel.Warn, "Incorrect test assemby paths!\n");
                return false;
            }
            var parameters = paths.Aggregate((current, path) => current + $" {Processor.QuoteArgument(path)}");

            if (!string.IsNullOrEmpty(traits))
                parameters = traits.Split(',').Select(t => t.Trim()).Aggregate(parameters, (current, trait) => current + $" -trait {Processor.QuoteArgument(trait)}");
            if (!string.IsNullOrEmpty(notraits))
                parameters = notraits.Split(',').Select(nt => nt.Trim()).Aggregate(parameters, (current, notrait) => current + $" -notrait {Processor.QuoteArgument(notrait)}");
            if (!string.IsNullOrEmpty(methodname))
                parameters += $" -method {methodname}";
            if (!string.IsNullOrEmpty(classname))
                parameters += $" -class {classname}";
            if (!string.IsNullOrEmpty(parallel))
                parameters += $" -parallel {parallel}";
            if (maxthreads != null)
                parameters += $" -maxthreads {maxthreads}";
            if (noshadow != null && noshadow.Value)
                parameters += " -noshadow";
            if (quiet != null && quiet.Value)
                parameters += " -quiet";
            if (serialize != null && serialize.Value)
                parameters += " -serialize";
            if (!string.IsNullOrEmpty(outputTypeAndName))
                parameters += $" -{outputTypeAndName}";
            parameters += " -nologo";

            var result = Processor.RunProcess(FullPathExe, parameters);

            if (!string.IsNullOrEmpty(result.Output))
                res = result.Output.Contains(TestsPassed);

            Logger.LogMethodEnd();
            return res;
        }
    }
}
