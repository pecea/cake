namespace Minify
{
    using Common;
    using Glob;
    using Microsoft.Ajax.Utilities;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static partial class Methods
    {
        public static bool MinifyJs(string pattern, string excludePattern = null, string destination = null, bool ignoreCase = true)
        {
            Logger.Log(LogLevel.Trace, "Method started");
            bool result = MinifyFiles(pattern, excludePattern, destination, ignoreCase, (minifier, file, dest) => minifier.MinifyJavaScriptFile(file, dest));
            Logger.Log(LogLevel.Trace, "Method finished");
            return result;
        }

        public static bool MinifyCss(string pattern, string excludePattern = null, string destination = null, bool ignoreCase = true)
        {
            Logger.Log(LogLevel.Trace, "Method started");
            bool result = MinifyFiles(pattern, excludePattern, destination, ignoreCase, (minifier, file, dest) => minifier.MinifyCssFile(file, dest));
            Logger.Log(LogLevel.Trace, "Method finished");
            return result;
        }

        private static bool MinifyFiles(string pattern, string excludePattern, string destination, bool ignoreCase, Action<Minifier, FileSystemInfo, string> minifyAction)
        {
            Logger.Log(LogLevel.Trace, "Method started");
            IEnumerable<FileSystemInfo> files = Glob.Expand(pattern, ignoreCase);

            if (!ValidateGlob(files, pattern))
                return false;

            if (!string.IsNullOrWhiteSpace(excludePattern))
            {
                IEnumerable<FileSystemInfo> excludedFiles = Glob.Expand(excludePattern, ignoreCase);
                files = files.Where(f => excludedFiles.All(ef => ef.FullName != f.FullName));
            }

            var minifier = new Minifier();
            foreach (FileSystemInfo fileInfo in files)
                minifyAction(minifier, fileInfo, destination);

            Logger.Log(LogLevel.Info, $"{files.Count()} files minified.");
            Logger.Log(LogLevel.Trace, "Method finished");
            return true;
        }

        private static bool ValidateGlob(IEnumerable<FileSystemInfo> files, string pattern)
        {
            Logger.Log(LogLevel.Trace, "Method started");
            if (!files.Any())
            {
                Logger.Log(LogLevel.Warn, $"Pattern {pattern} did not match any files.");
                return false;
            }

            if (files.Any(f => f.Attributes == FileAttributes.Directory))
            {
                string directories = string.Join(
                    separator: ",\n",
                    values: files.Where(f => f.Attributes == FileAttributes.Directory).Select(f => f.FullName)
                );

                Logger.Log(LogLevel.Warn, $"Pattern {pattern} matched directories: {directories}.");
                return false;
            }
            Logger.Log(LogLevel.Trace, "Method finished");

            return true;
        }
    }
}
