namespace MSTest
{
    using System;
    using System.IO;

    using Common;

    /// <summary>
    /// Encloses methods used for testing with MSTest
    /// </summary>
    public static class Methods
    {
        private static readonly string[] Paths = {
            Path.Combine(@"C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE", "MSTest.exe"),
            Path.Combine(@"C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\IDE", "MSTest.exe"),
            Path.Combine(@"C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE", "MSTest.exe"),
            Path.Combine(@"C:\Program Files (x86)\Microsoft Visual Studio 9.0\Common7\IDE", "MSTest.exe"),
        };

        public static string PathToExe { get; set; }

        private static string FullPathExe
        {
            get
            {
                if (File.Exists(PathToExe)) return PathToExe;
                foreach (var path in Paths)
                {
                    if (File.Exists(path)) return path;
                }
                return "MSTest.exe";
            }
        }

        /// <summary>
        /// Runs mstest.exe with test assemblies.
        /// </summary>
        /// <param name="testAssembliesPaths">Assemblies of the tests to be run.</param>
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
                    Processor.RunProcess(FullPathExe, "/testcontainer:" + path);
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
