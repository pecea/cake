using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Build.Tests
{
    internal class ConfigurationChecker : MarshalByRefObject
    {
        public bool IsDebug(string assemblyPath)
        {
            var assembly = Assembly.LoadFile(Path.GetFullPath(assemblyPath));
            return assembly.DefinedTypes.Any(t => t.Name == "Debug");
        }

        public bool IsRelease(string assemblyPath)
        {
            var assembly = Assembly.LoadFile(Path.GetFullPath(assemblyPath));
            return assembly.DefinedTypes.Any(t => t.Name == "Release");
        }
    }
}