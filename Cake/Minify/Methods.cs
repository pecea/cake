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

        public static bool BundleFiles(string pattern, string excludePattern = null, string destination = null, bool ignoreCase = true)
        {
            Logger.Log(LogLevel.Trace, "Method started");
            bool result = true;
            IEnumerable<FileSystemInfo> files = Glob.Expand(pattern, ignoreCase);

            if (!ValidateGlob(files, pattern))
                return false;

            if (!string.IsNullOrWhiteSpace(excludePattern))
            {
                IEnumerable<FileSystemInfo> excludedFiles = Glob.Expand(excludePattern, ignoreCase);
                files = files.Where(f => excludedFiles.All(ef => ef.FullName != f.FullName));
            }
            if (string.IsNullOrEmpty(destination))
            {
                Logger.Log(LogLevel.Warn, "You have to specify the output file!");
                return false;
            }

            using (var outputStream = File.Create(destination))
            {
                foreach (var fileName in files.Select(s => s.FullName))
                {
                    using (var inputStream = File.OpenRead(fileName))
                    {
                        // Buffer size can be passed as the second argument.
                        inputStream.CopyTo(outputStream);
                    }
                    Logger.Log(LogLevel.Info, $"The file {fileName} has been processed.");
                }
            }


            Logger.Log(LogLevel.Trace, "Method finished");
            return result;
        }

        //private static void CombineMultipleFilesIntoSingleFile(string inputDirectoryPath, string inputFileNamePattern, string outputFilePath)
        //{
        //    string[] inputFilePaths = Directory.GetFiles(inputDirectoryPath, inputFileNamePattern);
        //    Console.WriteLine("Number of files: {0}.", inputFilePaths.Length);
        //    using (var outputStream = File.Create(outputFilePath))
        //    {
        //        foreach (var inputFilePath in inputFilePaths)
        //        {
        //            using (var inputStream = File.OpenRead(inputFilePath))
        //            {
        //                // Buffer size can be passed as the second argument.
        //                inputStream.CopyTo(outputStream);
        //            }
        //            Console.WriteLine("The file {0} has been processed.", inputFilePath);
        //        }
        //    }
        //}
    }
}
