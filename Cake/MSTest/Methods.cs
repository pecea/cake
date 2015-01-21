﻿using System.Diagnostics;

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
        private static readonly string[] Paths =
            {
                Path.Combine(
                    @"C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE",
                    "MSTest.exe"),
                Path.Combine(
                    @"C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\IDE",
                    "MSTest.exe"),
                Path.Combine(
                    @"C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE",
                    "MSTest.exe"),
                Path.Combine(
                    @"C:\Program Files (x86)\Microsoft Visual Studio 9.0\Common7\IDE",
                    "MSTest.exe"),
                Path.Combine(
                    @"D:\Programs\Microsoft Visual Studio 12.0\Common7\IDE",
                    "MSTest.exe")
            };

        /// <summary>
        /// Local path to MSTest.exe
        /// </summary>
        public static string PathToExe { get; set; }

        private static string FullPathExe
        {
            get
            {
                return File.Exists(PathToExe) ? PathToExe : Paths.FirstOrDefault(File.Exists) ?? "MSTest.exe";
            }
        }

        /// <summary>
        /// Runs mstest.exe with test assemblies.
        /// </summary>
        /// <param name="testAssembliesPaths">Assemblies of the tests to be run. Paths may contain wildcards.</param>
        /// <returns>True if the tests were executed successfully, false otherwise.</returns>
        public static bool Test(params string[] testAssembliesPaths)
        {
            if (!File.Exists(FullPathExe))
            {
                Logger.Log(LogLevel.Error, "mstest.exe file not found.");
                return false;
            }
            var result = true;

            foreach (var path in testAssembliesPaths)
            {
                try
                {
                    var paths = path.GetFilePaths();
                    if (paths == null)
                    {
                        result = false;
                        continue;
                    }

                    var enumeratedPaths = paths.ToArray();

                    if (enumeratedPaths.Length == 0)
                    {
                        result = false;
                        continue;
                    }

                    result = enumeratedPaths.Aggregate(result,
                        (current, testPath) =>
                            Processor.RunProcess(FullPathExe, "/testcontainer:" + Processor.QuoteArgument(testPath)) &&
                            current);
                }
                catch (Exception e)
                {
                    Logger.LogException(LogLevel.Error, e, "An exception occured while running mstest.exe");
                    result = false;
                }
            }

            return result;
        }
    }
}
