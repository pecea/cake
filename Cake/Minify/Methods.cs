using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;
using Microsoft.Ajax.Utilities;

namespace Minify
{
    /// <summary>
    /// Encloses methods used with minifying files.
    /// </summary>
    public static class Methods
    {
        /// <summary>
        /// Method for minifying .js files.
        /// </summary>
        /// <param name="pattern">Pattern for matching files to minify</param>
        /// <param name="excludePattern">Pattern for not matching files to minify</param>
        /// <param name="destination">Path to the output directory</param>
        /// <param name="ignoreCase">Flag indicating whether to ignore case in files</param>
        /// <returns>True in case of success, false otherwise</returns>
        public static bool MinifyJs(string pattern, string excludePattern = null, string destination = null, bool ignoreCase = true)
        {
            Logger.LogMethodStart();
            var result = MinifyFiles(pattern, excludePattern, destination, ignoreCase, (minifier, file, dest) => minifier.MinifyJavaScriptFile(file, dest));
            Logger.LogMethodEnd();
            return result;
        }
        /// <summary>
        /// Method for minifying .css files.
        /// </summary>
        /// <param name="pattern">Pattern for matching files to minify</param>
        /// <param name="excludePattern">Pattern for not matching files to minify</param>
        /// <param name="destination">Path to the output directory</param>
        /// <param name="ignoreCase">Flag indicating whether to ignore case in files</param>
        /// <returns>True in case of success, false otherwise</returns>
        public static bool MinifyCss(string pattern, string excludePattern = null, string destination = null, bool ignoreCase = true)
        {
            Logger.LogMethodStart();
            var result = MinifyFiles(pattern, excludePattern, destination, ignoreCase, (minifier, file, dest) => minifier.MinifyCssFile(file, dest));
            Logger.LogMethodEnd();
            return result;
        }

        private static bool MinifyFiles(string pattern, string excludePattern, string destination, bool ignoreCase, Action<Minifier, FileSystemInfo, string> minifyAction)
        {
            Logger.LogMethodStart();

            var files = Glob.Glob.Expand(pattern, ignoreCase).ToArray();

            if (!ValidateGlob(files, pattern))
                return false;

            if (!string.IsNullOrWhiteSpace(excludePattern))
            {
                var excludedFiles = Glob.Glob.Expand(excludePattern, ignoreCase);
                files = files.Where(f => excludedFiles.All(ef => ef.FullName != f.FullName)).ToArray();
            }

            if (!string.IsNullOrEmpty(destination) && !Directory.Exists(destination))
                Directory.CreateDirectory(destination);

            var minifier = new Minifier();
            foreach (var fileInfo in files)
                minifyAction(minifier, fileInfo, destination);

            Logger.Log(LogLevel.Info, $"{files.Count()} files minified.");

            Logger.LogMethodEnd();
            return true;
        }

        private static bool ValidateGlob(IEnumerable<FileSystemInfo> files, string pattern)
        {
            Logger.LogMethodStart();

            files = files.ToArray();
            if (!files.Any())
                throw new ArgumentException($"Pattern {pattern} did not match any files.", nameof(pattern));

            if (files.Any(f => f.Attributes == FileAttributes.Directory))
            {
                var directories = string.Join(
                    ",\n",
                    files.Where(f => f.Attributes == FileAttributes.Directory).Select(f => f.FullName)
                );

                throw new ArgumentException($"Pattern {pattern} matched directories: {directories}.", nameof(pattern));
            }

            Logger.LogMethodEnd();
            return true;
        }

        /// <summary>
        /// Methodd for bundling files into one.
        /// </summary>
        /// <param name="pattern">Pattern for matching files to minify</param>
        /// <param name="destination">Path and name of the output file</param>
        /// <param name="separator">Character to separate bundled files</param>
        /// <param name="excludePattern">Pattern for not matching files to minify</param>
        /// <param name="ignoreCase">Flag indicating whether to ignore case in files</param>
        /// <returns>True in case of success, false otherwise</returns>
        public static bool BundleFiles(string pattern, string destination, char? separator = null, string excludePattern = null, bool ignoreCase = true)
        {
            Logger.LogMethodStart();
            var files = Glob.Glob.Expand(pattern, ignoreCase).ToArray();

            if (!ValidateGlob(files, pattern))
                return false;

            if (!string.IsNullOrWhiteSpace(excludePattern))
            {
                var excludedFiles = Glob.Glob.Expand(excludePattern, ignoreCase);
                files = files.Where(f => excludedFiles.All(ef => ef.FullName != f.FullName)).ToArray();
            }

            if (string.IsNullOrEmpty(destination))            
                throw new ArgumentNullException(nameof(destination));            

            var dest = Path.GetDirectoryName(destination);
            if (!string.IsNullOrEmpty(dest))
                Directory.CreateDirectory(dest);

            using (var outputStream = File.AppendText(destination))
            {
                foreach (var fileName in files.Select(s => $"{s.FullName}"))
                {
                    outputStream.Write(File.ReadAllText(fileName));
                    if (separator != null)
                        outputStream.Write(separator);

                    Logger.Log(LogLevel.Info, $"The file {fileName} has been processed.");
                }
            }

            Logger.LogMethodEnd();
            return true;
        }
    }
}
