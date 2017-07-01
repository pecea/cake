using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.IO;

namespace NUnit
{
    public class Methods
    {
        //noh | noheader - no copyright information at the top of the output

        private static string FullPathExe => "../../../External/nunit3-console.exe";
        #region oldMethods
        ///// <summary>
        ///// Run unit tests written with NUnit compiled to .dll's
        ///// </summary>
        ///// <param name="assemblyPaths">Paths should be separated by space></param>
        ///// <returns></returns>
        //public static bool RunTests(string assemblyPaths)
        //{
        //    if (!File.Exists(FullPathExe))
        //    {
        //        Logger.Log(LogLevel.Error, "Nunit3-console.exe file not found.");
        //        return false;
        //    }
        //    try
        //    {
        //        if (string.IsNullOrEmpty(assemblyPaths) || !File.Exists(assemblyPaths))
        //        {
        //            Logger.Log(LogLevel.Error, $"Incorrect test assemby paths!\n");
        //            return false;
        //        }
        //        return Processor.RunProcess(FullPathExe, "--noh " + assemblyPaths);
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.LogException(LogLevel.Error, e, "An exception occured while running nunit3-console.exe");
        //        return false;
        //    }
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="assemblyPaths">Paths should be separated by space></param>
        ///// <param name="conditions">Conditions may specify test names, classes, methods, categories or properties comparing them to actual values with the operators ==, !=, =~ and !~</param>
        ///// <returns></returns>
        //public static bool RunTests(string assemblyPaths, string conditions)
        //{
        //    if (!File.Exists(FullPathExe))
        //    {
        //        Logger.Log(LogLevel.Error, "Nunit3-console.exe file not found.");
        //        return false;
        //    }
        //    try
        //    {
        //        if (string.IsNullOrEmpty(assemblyPaths) || !File.Exists(assemblyPaths))
        //        {
        //            Logger.Log(LogLevel.Error, $"Incorrect test assemby paths!\n");
        //            return false;
        //        }
        //        return Processor.RunProcess(FullPathExe, $"--noh {assemblyPaths} --where={conditions}");
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.LogException(LogLevel.Error, e, "An exception occured while running nunit3-console.exe");
        //        return false;
        //    }
        //}
#endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyPaths">Paths should be separated by space</param>
        /// <param name="conditions">Conditions may specify test names, classes, methods, categories or properties comparing them to actual values with the operators ==, !=, =~ and !~</param>
        /// <param name="config">Name of a project configuration to load (e.g. Debug) </param>
        /// <returns></returns>
        public static bool RunTests(string assemblyPaths, string conditions = null, string config = null)
        {
            if (!File.Exists(FullPathExe))
            {
                Logger.Log(LogLevel.Error, "Nunit3-console.exe file not found.");
                return false;
            }
            try
            {
                if (string.IsNullOrEmpty(assemblyPaths) || !File.Exists(assemblyPaths))
                {
                    Logger.Log(LogLevel.Error, $"Incorrect test assemby paths!\n");
                    return false;
                }
                var parameters = $"--noh {assemblyPaths}";
                if (!string.IsNullOrEmpty(conditions))
                    parameters += $" --where={conditions}";
                if(!string.IsNullOrEmpty(config))
                    parameters += $" --config={config}";
                return Processor.RunProcess(FullPathExe, parameters);
            }
            catch (Exception e)
            {
                Logger.LogException(LogLevel.Error, e, "An exception occured while running nunit3-console.exe");
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyPaths">Paths should be separated by space</param>
        /// <param name="conditions">Conditions may specify test names, classes, methods, categories or properties comparing them to actual values with the operators ==, !=, =~ and !~</param>
        /// <param name="config">Name of a project configuration to load (e.g. Debug) </param>
        /// <param name="workingDirectoryPath">Path of the directory to use for output files.</param>
        /// <param name="outputPath">File path to contain text output from the tests.</param>
        /// <param name="errorPath">File path to contain error output from the tests.</param>
        /// <param name="stopOnError">Stop run immediately upon any test failure or error.</param>
        /// <param name="skipNonAssemblies">Skip any non-test assemblies specified, without error.</param>
        /// <param name="noResult">Don't save any test results.</param>
        /// <param name="verbosity">Set internal trace level. Possible values: Off, Error, Warning, Info, Verbose (Debug)</param>
        /// <param name="timeout">Set timeout for each test case in milliseconds</param>
        /// <param name="shadowcopy">Tells .NET to copy loaded assemblies to the shadowcopy directory.</param>
        /// <param name="processIsolation">Process isolation for test assemblies. Possible values: Single, Separate, Multiple. If not specified, defaults to Separate for a single assembly or Multiple for more than one. By default, processes are run in parallel.</param>
        /// <param name="numberOfAgents">Number of agents that may be allowed to run simultaneously assuming you are not running in a single process. If not specified, all agent processes run tests at the same time, whatever the number of assemblies. This setting is used to control running your assemblies in parallel.</param>
        /// <param name="domainIsolation">Domain isolation for test assemblies. Possible values: None, Single, Multiple. If not specified, defaults to Single for a single assembly or Multiple for more than one.</param>
        /// <param name="frameworkVersion">Framework type/version to use for tests. (e.g.: mono, net-4.5, v4.0, 2.0, mono-4.0)</param>
        /// <param name="runIn32Bit">Run tests in a 32-bit process on 64-bit systems.</param>
        /// <param name="disposeRunners">Dispose each test runner after it has finished running its tests</param>
        /// <returns></returns>
        public static bool RunTestsWithOptions(string assemblyPaths, string conditions = null, string config = null, string workingDirectoryPath = null, string outputPath = null, string errorPath = null, bool? stopOnError = null, bool? skipNonAssemblies = null, bool? noResult = null, string verbosity = null, string timeout = null, bool? shadowcopy = null, string processIsolation = null, string numberOfAgents = null, string domainIsolation = null, string frameworkVersion = null, bool? runIn32Bit = null, bool? disposeRunners = null)
        {
            if (!File.Exists(FullPathExe))
            {
                Logger.Log(LogLevel.Error, "Nunit3-console.exe file not found.");
                return false;
            }
            try
            {
                if (string.IsNullOrEmpty(assemblyPaths) || !File.Exists(assemblyPaths))
                {
                    Logger.Log(LogLevel.Error, $"Incorrect test assemby paths!\n");
                    return false;
                }
                var parameters = $"--noh {assemblyPaths}";
                if (!string.IsNullOrEmpty(conditions))
                    parameters += $" --where={conditions}";
                if (!string.IsNullOrEmpty(config))
                    parameters += $" --config={config}";
                if (!string.IsNullOrEmpty(workingDirectoryPath))
                    parameters += $" --work={workingDirectoryPath}";
                if (!string.IsNullOrEmpty(outputPath))
                    parameters += $" --out={outputPath}";
                if (!string.IsNullOrEmpty(errorPath))
                    parameters += $" --err={errorPath}";
                if(stopOnError.HasValue)
                    parameters += $" --stoponerror";
                if (skipNonAssemblies.HasValue)
                    parameters += $" --skipnontestassemblies";
                if (noResult.HasValue)
                    parameters += $" --noresult";
                if (!string.IsNullOrEmpty(verbosity))
                    parameters += $" --trace={verbosity}";
                if (!string.IsNullOrEmpty(timeout))
                    parameters += $" --timeout={timeout}";
                if (shadowcopy.HasValue)
                    parameters += $" --shadowcopy";
                if (!string.IsNullOrEmpty(processIsolation))
                    parameters += $" --process={processIsolation}";
                if (!string.IsNullOrEmpty(numberOfAgents))
                    parameters += $" --agents={numberOfAgents}";
                if (!string.IsNullOrEmpty(domainIsolation))
                    parameters += $" --domain={domainIsolation}";
                if (!string.IsNullOrEmpty(frameworkVersion))
                    parameters += $" --framework={frameworkVersion}";
                if (runIn32Bit.HasValue)
                    parameters += $" --x86";
                if (disposeRunners.HasValue)
                    parameters += $" --dispose-runners";
                return Processor.RunProcess(FullPathExe, parameters);
            }
            catch (Exception e)
            {
                Logger.LogException(LogLevel.Error, e, "An exception occured while running nunit3-console.exe");
                return false;
            }
        }

    }
}
