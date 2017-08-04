using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Build.Tests
{
    internal class ConfigurationChecker : MarshalByRefObject
    {
        public bool IsDebug(string assemblyPath)
        {
            var assembly = Assembly.LoadFile(Path.GetFullPath(assemblyPath));
            return assembly?.GetCustomAttributes(typeof(DebuggableAttribute), false).Length > 0;
        }
    }
}