﻿using Common;
using System;
using System.IO;
using System.Linq;

namespace XUnit
{
    public class Methods
    {
        private static string FullPathExe => "../../../External/xunit.console.exe";
        /// <summary>
        /// Runs XUnit unit tests from the speciffied assemblyPath
        /// </summary>
        /// <param name="assemblyPath">Path to an assembly with XUnit unit tests</param>
        /// <param name="traits">Attirubtes on test methods in a dictionary form of names and values</param>
        /// <returns>True if xunit tests process run successfully, otherwise false</returns>
        public static bool RunTests(string assemblyPath, string traits = null)
        {
            if (!File.Exists(FullPathExe))
            {
                Logger.Log(LogLevel.Error, "xunit.console.exe file not found.");
                return false;
            }
            try
            {
                if (string.IsNullOrEmpty(assemblyPath) || !File.Exists(assemblyPath))
                {
                    Logger.Log(LogLevel.Error, $"Incorrect test assemby paths!\n");
                    return false;
                }
                var parameters = $"{assemblyPath} -nologo";
                //parameters += " -nologo";
                if (!string.IsNullOrEmpty(traits))
                    parameters = traits.Split(',').Select(t => t.Trim()).Aggregate(parameters, (current, trait) => current + $"-trait {Processor.QuoteArgument(trait)} ");
                var result = Processor.RunProcess(FullPathExe, parameters);
                //(bool success, string output, string error) = Processor.RunProcess(FullPathExe, parameters);
                if (!string.IsNullOrEmpty(result.Item2))
                    Logger.Log(LogLevel.Debug, "Tests output: \n");
                if (!string.IsNullOrEmpty(result.Item3))
                    Logger.Log(LogLevel.Debug, "XUnit process error: \n");

                return result.Item1;
            }
            catch (Exception e)
            {
                Logger.LogException(LogLevel.Error, e, "An exception occured while running xunit.console.exe");
                return false;
            }
        }
        /// <summary>
        /// Runs XUnit tests from the specified assemblyPath with different options
        /// </summary>
        /// <param name="assemblyPath">Path to an assembly with XUnit unit tests</param>
        /// <param name="traits">Attirubtes on test methods must be a dictionary in a form of names and values</param> 
        /// <param name="notraits">Attributes on test methods must not be a dictionary in a form of names and values</param>
        /// <param name="methodname">Fully specified methodname of a test method - namespace, classname and methodname</param>
        /// <param name="classname">Fully specified classname of a test class - namespace and classname</param>
        /// <param name="parallel">Sets parallelization. Possible values: none - turn off all parallelization, collections - only parallelize collections, assemblies - only parallelize assemblies, all - parallelize assemblies & collections</param>
        /// <param name="maxthreads">Maximum thread count for collection parallelization: 0 - run with unbounded thread count, >0 - limit task thread pool size to 'count'</param>
        /// <param name="noshadow">Flag indicating whether to shadow copy assemblies</param>
        /// <param name="quiet">Flag indicating whether to show progress messages</param>
        /// <param name="serialize">Flag indicating whether to serialize all test cases - for diagnostic purposes only</param>
        /// <param name="outputTypeAndName">Option to specify output path and filename. Possible values: xml filename - xUnit.net v2 style XML file, xmlv1 filename - xUnit.net v1 style XML file, nunit filename - NUnit-style XML file, html filename - HTML file</param>
        /// <returns>True if xunit tests process run successfully, otherwise false</returns>

        public static bool RunTestsWithOptions(string assemblyPath, string traits = null, string notraits = null, string methodname = null, string classname = null, 
            string parallel = null, int? maxthreads = null, bool? noshadow = null, bool? quiet = null, bool? serialize = null, string outputTypeAndName = null)
        {
            if (!File.Exists(FullPathExe))
            {
                Logger.Log(LogLevel.Error, "xunit.console.exe file not found.");
                return false;
            }
            try
            {
                if (string.IsNullOrEmpty(assemblyPath) || !File.Exists(assemblyPath))
                {
                    Logger.Log(LogLevel.Error, $"Incorrect test assemby paths!\n");
                    return false;
                }
                var parameters = $"{assemblyPath} -nologo";
                //parameters += " -nologo";
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

                var result = Processor.RunProcess(FullPathExe, parameters);

                //(bool success, string output, string error) = Processor.RunProcess(FullPathExe, parameters);
                if (!string.IsNullOrEmpty(result.Item2))
                    Logger.Log(LogLevel.Debug, "Tests output: \n");
                if (!string.IsNullOrEmpty(result.Item3))
                    Logger.Log(LogLevel.Debug, "XUnit process error: \n");

                return result.Item1;
            }
            catch (Exception e)
            {
                Logger.LogException(LogLevel.Error, e, "An exception occured while running xunit.console.exe");
                return false;
            }
        }
    }
}
