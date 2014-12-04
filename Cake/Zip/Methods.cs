namespace Zip
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Common;

    using Ionic.Zip;

    public static class Methods
    {
        #region methods

        /// <summary>
        /// Encloses methods used with zipping projects.
        /// </summary>
        /// <param name="zipName">Name of the zip file</param>
        /// <param name="filePaths">Paths to files</param>
        /// <param name="zipPath">Path of the zip to save</param>
        /// <returns>true in case of success, false otherwise.</returns>
        /// TODO: zipName i zipPath do jednego argumentu, filePaths dodać params
        public static bool ZipFiles(string zipName, string[] filePaths, string zipPath = null)
        {
            if (!CheckZipFilesArguments(filePaths, zipPath)) return false;
            try
            {
                using (var zip = new ZipFile())
                {
                    foreach (var path in filePaths)
                    {
                        var attributes = File.GetAttributes(path);
                        if ((attributes & FileAttributes.Directory) == FileAttributes.Directory) zip.AddDirectory(path);
                        else zip.AddFile(path);
                    }
                    zip.Save(
                        !String.IsNullOrEmpty(zipPath)
                            ? String.Format("{0}/{1}.zip", zipPath, zipName)
                            : string.Format("{0}.zip", zipName));
                    Logger.Log(LogLevel.Info, String.Format("{0} zipped succesfully.", zipName));
                }
            }
            catch (DirectoryNotFoundException e)
            {
                Logger.LogException(LogLevel.Error, e, String.Format("Zipping {0}.zip failed.", zipName));
                return false;
            }
            catch (FileNotFoundException e)
            {
                Logger.LogException(LogLevel.Error, e, String.Format("Zipping {0}.zip failed.", zipName));
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
            if (!filePaths.All(filePath => File.Exists(filePath) || Directory.Exists(filePath)))
                return false;

            if (zipPath != null)
            {
                try
                {
                    var path = Path.GetFullPath(zipPath);
                }
                catch (Exception)
                {
                    Logger.Log(LogLevel.Warn, "The zipPath parameter is not a valid path.");
                    return false;
                }
            }
            return true;
        }

        #endregion
    }
}
