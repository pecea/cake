using System;
using System.Diagnostics;
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
            var types = assembly.GetCustomAttributes(typeof(DebuggableAttribute), false);
            return assembly.GetCustomAttributes(typeof(DebuggableAttribute), false).Length > 0;
            //return assembly.DefinedTypes.Any(t => t.Name == "Debug");
        }

        //public bool IsRelease(string assemblyPath)
        //{
        //    var assembly = Assembly.LoadFile(Path.GetFullPath(assemblyPath));
        //    var types = assembly.GetCustomAttributes(typeof(DebuggableAttribute), false);
        //    return assembly.DefinedTypes.Any(t => t.Name == "Release");
        //}
    }
}