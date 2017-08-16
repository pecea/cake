using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;
using Microsoft.Ajax.Utilities;

namespace Minify
{
    public static class Methods
    {
        /// <summary>
        /// Method for minifying .js files.
        /// </summary>
        /// <param name="pattern">Pattern for matching files to minify</param>
        /// <param name="excludePattern">Pattern for not matching files to minify</param>
        /// <param name="destination">Path and name of the output file</param>
        /// <param name="ignoreCase">Flag indicating whether to ignore case in files</param>
        /// <returns>True in case of success, false otherwise</returns>
        public static bool MinifyJs(string pattern, string excludePattern = null, string destination = null, bool ignoreCase = true)
        {
            Logger.Log(LogLevel.Trace, "Method started.");
            var result = MinifyFiles(pattern, excludePattern, destination, ignoreCase, (minifier, file, dest) => minifier.MinifyJavaScriptFile(file, dest));
            Logger.Log(LogLevel.Trace, "Method finished.");
            return result;
        }
        /// <summary>
        /// Method for minifying .css files.
        /// </summary>
        /// <param name="pattern">Pattern for matching files to minify</param>
        /// <param name="excludePattern">Pattern for not matching files to minify</param>
        /// <param name="destination">Path and name of the output file</param>
        /// <param name="ignoreCase">Flag indicating whether to ignore case in files</param>
        /// <returns>True in case of success, false otherwise</returns>
        public static bool MinifyCss(string pattern, string excludePattern = null, string destination = null, bool ignoreCase = true)
        {
            Logger.Log(LogLevel.Trace, "Method started.");
            var result = MinifyFiles(pattern, excludePattern, destination, ignoreCase, (minifier, file, dest) => minifier.MinifyCssFile(file, dest));
            Logger.Log(LogLevel.Trace, "Method finished.");
            return result;
        }

        private static bool MinifyFiles(string pattern, string excludePattern, string destination, bool ignoreCase, Action<Minifier, FileSystemInfo, string> minifyAction)
        {
            Logger.Log(LogLevel.Trace, "Method started.");
            try
            {
                var files = Glob.Glob.Expand(pattern, ignoreCase).ToArray();

                if (!ValidateGlob(files, pattern))
                    return false;

                if (!string.IsNullOrWhiteSpace(excludePattern))
                {
                    var excludedFiles = Glob.Glob.Expand(excludePattern, ignoreCase);
                    files = files.Where(f => excludedFiles.All(ef => ef.FullName != f.FullName)).ToArray();
                }

                var minifier = new Minifier();
                foreach (var fileInfo in files)
                    minifyAction(minifier, fileInfo, destination);

                Logger.Log(LogLevel.Info, $"{files.Count()} files minified.");
            }
            catch(Exception ex)
            {
                Logger.LogException(LogLevel.Error, ex, "Could not minify files!");
                return false;
            }
            Logger.Log(LogLevel.Trace, "Method finished.");
            return true;
        }

        private static bool ValidateGlob(IEnumerable<FileSystemInfo> files, string pattern)
        {
            Logger.Log(LogLevel.Trace, "Method started.");
            files = files.ToArray();
            if (!files.Any())
            {
                Logger.Log(LogLevel.Warn, $"Pattern {pattern} did not match any files.");
                return false;
            }

            if (files.Any(f => f.Attributes == FileAttributes.Directory))
            {
                var directories = string.Join(
                    ",\n",
                    files.Where(f => f.Attributes == FileAttributes.Directory).Select(f => f.FullName)
                );

                Logger.Log(LogLevel.Warn, $"Pattern {pattern} matched directories: {directories}.");
                return false;
            }
            Logger.Log(LogLevel.Trace, "Method finished.");

            return true;
        }

        /// <summary>
        /// Methodd for bundling files into one.
        /// </summary>
        /// <param name="pattern">Pattern for matching files to minify</param>
        /// <param name="excludePattern">Pattern for not matching files to minify</param>
        /// <param name="destination">Path and name of the output file</param>
        /// <param name="ignoreCase">Flag indicating whether to ignore case in files</param>
        /// <returns>True in case of success, false otherwise</returns>
        public static bool BundleFiles(string pattern, string destination, char? separator = null, string excludePattern = null, bool ignoreCase = true)
        {
            Logger.Log(LogLevel.Trace, "Method started.");
            try
            {
                var files = Glob.Glob.Expand(pattern, ignoreCase).ToArray();

                if (!ValidateGlob(files, pattern))
                    return false;

                if (!string.IsNullOrWhiteSpace(excludePattern))
                {
                    var excludedFiles = Glob.Glob.Expand(excludePattern, ignoreCase);
                    files = files.Where(f => excludedFiles.All(ef => ef.FullName != f.FullName)).ToArray();
                }
                if (string.IsNullOrEmpty(destination))
                {
                    Logger.Log(LogLevel.Warn, "You have to specify the output file!");
                    return false;
                }

                using (var outputStream = File.AppendText(destination)) //File.OpenRead(destination)
                {
                    foreach (var fileName in files.Select(s => $"{s.FullName}"))
                    {
                        outputStream.Write(File.ReadAllText(fileName));
                        if(separator != null)
                            outputStream.Write(separator);

                        Logger.Log(LogLevel.Info, $"The file {fileName} has been processed.");
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.LogException(LogLevel.Error, ex, "Could not bundle files!");
                return false;
            }

            Logger.Log(LogLevel.Trace, "Method finished.");
            return true;
        }
    }
}
