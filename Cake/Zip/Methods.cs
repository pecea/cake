namespace Zip
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Common;

    using Ionic.Zip;
    /// <summary>
    /// Encloses methods used with zipping files.
    /// </summary>
    public static class Methods
    {
        #region methods

        /// <summary>
        /// Adds files to .zip archive
        /// </summary>
        /// <param name="zipPathAndName">Path and name of the zip file</param>
        /// <param name="filePaths">Paths to files for zipping</param>
        /// <returns></returns>
        public static bool ZipFiles(string zipPathAndName, params string[] filePaths)
        {
            var paths = new List<string>();
            foreach (var filePath in filePaths)
            {
                paths.AddRange(filePath.GetFilePaths());
                paths.AddRange(filePath.GetDirectoriesPaths());
            }
            filePaths = paths.ToArray();

            if (!CheckZipFilesArguments(filePaths, zipPathAndName)) return false;
            try
            {
                using (var zip = new ZipFile())
                {
                    foreach (var path in filePaths)
                    {
                        var attributes = File.GetAttributes(path);
                        if ((attributes & FileAttributes.Directory) == FileAttributes.Directory) zip.AddDirectory(path);
                        else zip.AddFile(path,"");
                    }
                    zip.Save($"{zipPathAndName}.zip");
                    Logger.Log(LogLevel.Info, $"{zipPathAndName} zipped succesfully.");
                }
            }
            catch (DirectoryNotFoundException e)
            {
                Logger.LogException(LogLevel.Error, e, $"Zipping {zipPathAndName}.zip failed.");
                return false;
            }
            catch (FileNotFoundException e)
            {
                Logger.LogException(LogLevel.Error, e, $"Zipping {zipPathAndName}.zip failed.");
                return false;
            }
            return true;
        }


        /// <summary>
        /// Adds files to .zip archive
        /// </summary>
        /// <param name="zipPathAndName">Path and name of the zip file</param>
        /// <param name="filePaths">Paths to files for zipping</param>
        /// <returns></returns>
        public static bool ZipFiles(string zipPathAndName, string filePaths)
        {
            var paths = new List<string>();
            var splittedFilePaths = filePaths.Split(',').Select(f => f.Trim()).ToArray();
            foreach (var filePath in splittedFilePaths)
            {
                paths.AddRange(filePath.GetFilePaths());
                paths.AddRange(filePath.GetDirectoriesPaths());
            }
            splittedFilePaths = paths.ToArray();

            if (!CheckZipFilesArguments(splittedFilePaths, zipPathAndName)) return false;
            try
            {
                using (var zip = new ZipFile())
                {
                    foreach (var path in splittedFilePaths)
                    {
                        var attributes = File.GetAttributes(path);
                        if ((attributes & FileAttributes.Directory) == FileAttributes.Directory) zip.AddDirectory(path);
                        else zip.AddFile(path, "");
                    }
                    zip.Save(zipPathAndName.Contains(".zip") ? zipPathAndName : $"{zipPathAndName}.zip");
                    Logger.Log(LogLevel.Info, $"{zipPathAndName} zipped succesfully.");
                }
            }
            catch (DirectoryNotFoundException e)
            {
                Logger.LogException(LogLevel.Error, e, $"Zipping {zipPathAndName}.zip failed.");
                return false;
            }
            catch (FileNotFoundException e)
            {
                Logger.LogException(LogLevel.Error, e, $"Zipping {zipPathAndName}.zip failed.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if arguments passed to ZipFiles methods are valid.
        /// </summary>
        /// <param name="filePaths">Paths to files</param>
        /// <param name="zipPath">Path of the zip to save</param>
        /// <returns></returns>
        private static bool CheckZipFilesArguments(IEnumerable<string> filePaths, string zipPath)
        {
            var enumerable = filePaths as IList<string> ?? filePaths.ToList();
            if (!enumerable.All(filePath => File.Exists(filePath) || Directory.Exists(filePath)) || !enumerable.Any())
                return false;

            if (string.IsNullOrEmpty(zipPath)) return true;
            string fullPath;
            try
            {
                fullPath = Path.GetFullPath(zipPath);
            }
            catch (Exception)
            {
                Logger.Log(LogLevel.Warn, $"The zipPath parameter {zipPath} is not a valid path.");
                return false;
            }
            return !string.IsNullOrEmpty(fullPath);
        }

        #endregion
    }
}
