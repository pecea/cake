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
                    zip.Save(String.Format("{0}.zip", zipPathAndName));
                    Logger.Log(LogLevel.Info, String.Format("{0} zipped succesfully.", zipPathAndName));
                }
            }
            catch (DirectoryNotFoundException e)
            {
                Logger.LogException(LogLevel.Error, e, String.Format("Zipping {0}.zip failed.", zipPathAndName));
                return false;
            }
            catch (FileNotFoundException e)
            {
                Logger.LogException(LogLevel.Error, e, String.Format("Zipping {0}.zip failed.", zipPathAndName));
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
            if (!filePaths.All(filePath => File.Exists(filePath) || Directory.Exists(filePath)) || !filePaths.Any())
                return false;

            if (String.IsNullOrEmpty(zipPath)) return true;
            try
            {
                Path.GetFullPath(zipPath);
            }
            catch (Exception)
            {
                Logger.Log(LogLevel.Warn, "The zipPath parameter is not a valid path.");
                return false;
            }
            return true;
        }

        #endregion
    }
}
