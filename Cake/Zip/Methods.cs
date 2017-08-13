using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;
using Ionic.Zip;
using Ionic.Zlib;

namespace Zip
{
    //using Ionic.Zip;
    /// <summary>
    /// Encloses methods used with zipping files.
    /// </summary>
    public static class Methods
    {
        #region methods

        /// <summary>
        /// Adds files and directories to .zip archive
        /// </summary>
        /// <param name="zipPathAndName">Path and name of the zip file</param>
        /// <param name="entriesPaths">Paths to files and directories for zipping</param>
        /// <returns>True, if zipping was successful, false otherwise</returns>
        public static bool ZipFiles(string zipPathAndName, params string[] entriesPaths)
        {
            Logger.Log(LogLevel.Trace, "Method started");
            var paths = new List<string>();
            foreach (var filePath in entriesPaths)
            {
                paths.AddRange(filePath.GetFilePaths());
                paths.AddRange(filePath.GetDirectoriesPaths());
            }
            //entriesPaths = paths.ToArray();

            if (!CheckZipFilesArguments(paths, zipPathAndName)) return false;
            try
            {
                using (var zip = new ZipFile())
                {
                    foreach (var path in paths)
                    {
                        var attributes = File.GetAttributes(path);
                        if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
                            zip.AddDirectory(path, path.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault());
                        else zip.AddFile(path, "");
                    }
                    if (!string.IsNullOrEmpty(zipPathAndName) && zipPathAndName.Contains(".zip"))
                        zip.Save(zipPathAndName);
                    else
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
            Logger.Log(LogLevel.Trace, "Method finished");
            return true;
        }

        /// <summary>
        /// Adds files and directories to .zip archive
        /// </summary>
        /// <param name="zipPathAndName">Path and name of the zip file</param>
        /// <param name="password">Archive password</param>
        /// <param name="compression">Compression level. Possible values: none, best, fastest</param>
        /// <param name="aes256Encryption">Flag indicating whether to use Aes256 encryption for the archive content</param>
        /// <param name="useZip64">Flag indicating whether to use Zip64 when saving the archive (for large files)</param>
        /// <param name="filePaths">Paths to files and directories for zipping</param>
        /// <returns>True, if zipping was successful, false otherwise</returns>
        public static bool ZipFilesWithOptions(string zipPathAndName, string password = null, string compression = null, bool aes256Encryption = false, bool useZip64 = false, params string[] filePaths)
        {
            Logger.Log(LogLevel.Trace, "Method started");
            var paths = new List<string>();
            foreach (var filePath in filePaths)
            {
                paths.AddRange(filePath.GetFilePaths());
                paths.AddRange(filePath.GetDirectoriesPaths());
            }

            if (!CheckZipFilesArguments(paths, zipPathAndName)) return false;
            try
            {
                using (var zip = new ZipFile())
                {
                    if(useZip64)
                        zip.UseZip64WhenSaving = Zip64Option.AsNecessary;
                    if (!string.IsNullOrEmpty(password))
                        zip.Password = password;
                    if(aes256Encryption)
                        zip.Encryption = EncryptionAlgorithm.WinZipAes256;
                    switch (compression)
                    {
                        case "none":
                            zip.CompressionLevel = CompressionLevel.None;
                            break;
                        case "best":
                            zip.CompressionLevel = CompressionLevel.BestCompression;
                            break;
                        case "fastest":
                            zip.CompressionLevel = CompressionLevel.BestSpeed;
                            break;
                        default:
                            zip.CompressionLevel = CompressionLevel.Default;
                            break;
                    }
                    foreach (var path in paths)
                    {
                        var attributes = File.GetAttributes(path);
                        if ((attributes & FileAttributes.Directory) == FileAttributes.Directory) zip.AddDirectory(path, path);
                        else zip.AddFile(path, "");
                    }
                    if (!string.IsNullOrEmpty(zipPathAndName) && zipPathAndName.Contains(".zip"))
                        zip.Save(zipPathAndName);
                    else
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
            Logger.Log(LogLevel.Trace, "Method finished");
            return true;
        }
        /// <summary>
        /// Extracts files and directories from a .zip file
        /// </summary>
        /// <param name="zipPathAndName">Path and name of the archive</param>
        /// <param name="destination">Destination where unzipped content should be saved</param>
        /// <param name="password">Password for the archive</param>
        /// <param name="overwrite">Overwrite files in the destination directory</param>
        /// <returns>True, if unzipping was successful, false otherwise</returns>
        public static bool ExtractFiles(string zipPathAndName, string destination, string password = null, bool overwrite = false)
        {
            Logger.Log(LogLevel.Trace, "Method started");
            if (!CheckIfArchiveExists(zipPathAndName))
            {
                Logger.Log(LogLevel.Warn, $"Could not find {zipPathAndName}");
                return false;
            }
            try
            {
                using (var zip = ZipFile.Read(zipPathAndName))
                {
                    foreach (var entry in zip)
                    {
                        if (!string.IsNullOrEmpty(password))
                        {
                            if(overwrite)
                                entry.ExtractWithPassword(destination, ExtractExistingFileAction.OverwriteSilently, password);
                            else
                                entry.ExtractWithPassword(destination, password);
                        }
                        else
                        {
                            if (overwrite)
                                entry.Extract(destination, ExtractExistingFileAction.OverwriteSilently);
                            else
                                entry.Extract(destination);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(LogLevel.Error, ex, $"Unzipping {zipPathAndName}.zip failed.");
                return false;
            }
            Logger.Log(LogLevel.Trace, "Method finished");
            return true;
        }

        /// <summary>
        /// Removes specified entries from the archive.
        /// </summary>
        /// <param name="zipPathAndName">Path and name of the modified archive</param>
        /// <param name="entriesToDelete">Names of entries to be removed</param>
        /// <returns>True, if deleting was successful, false otherwise</returns>
        public static bool DeleteEntries(string zipPathAndName, params string[] entriesToDelete)
        {
            Logger.Log(LogLevel.Trace, "Method started");
            if (!CheckIfArchiveExists(zipPathAndName))
            {
                Logger.Log(LogLevel.Warn, $"Could not find {zipPathAndName}");
                return false;
            }
            try
            {
                using (var zip = ZipFile.Read(zipPathAndName))
                {
                    zip.RemoveEntries(entriesToDelete);
                    zip.Save();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(LogLevel.Error, ex, $"Deleting files from {zipPathAndName}.zip failed.");
                return false;
            }
            Logger.Log(LogLevel.Trace, "Method finished");
            return true;
        }

        /// <summary>
        /// Removes specified entries from the archive.
        /// </summary>
        /// <param name="zipPathAndName">Path and name of the modified archive</param>
        /// <param name="entriesToUpdate">Names of entries to be updated</param>
        /// <returns></returns>
        public static bool UpdateEntries(string zipPathAndName, params string[] entriesToUpdate)
        {
            Logger.Log(LogLevel.Trace, "Method started");
            if (!CheckIfArchiveExists(zipPathAndName))
            {
                Logger.Log(LogLevel.Warn, $"Could not find {zipPathAndName}");
                return false;
            }
            try
            {
                using (var zip = ZipFile.Read(zipPathAndName))
                {
                    foreach (var entry in entriesToUpdate)
                    {
                        var mod = entry;
                        var attributes = File.GetAttributes(mod);
                        if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
                        {
                            mod = mod.Replace('\\', '/').Split(new[] { '/'}, StringSplitOptions.RemoveEmptyEntries).LastOrDefault() +'/';
                            zip.UpdateItem(entry, mod);
                        }
                        else
                            zip.UpdateItem(entry);
                    }
                    zip.Save();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(LogLevel.Error, ex, $"Updating files in {zipPathAndName}.zip failed.");
                return false;
            }
            Logger.Log(LogLevel.Trace, "Method finished");
            return true;
        }
        /// <summary>
        /// Renames a file in the arhcive
        /// </summary>
        /// <param name="zipPathAndName">Path and name of the archive</param>
        /// <param name="oldName">Old name of the entry to be renamed</param>
        /// <param name="newName">New name of the entry to be renamed</param>
        /// <returns></returns>
        public static bool RenameEntry(string zipPathAndName, string oldName, string newName)
        {
            Logger.Log(LogLevel.Trace, "Method started");
            if (!CheckIfArchiveExists(zipPathAndName))
            {
                Logger.Log(LogLevel.Warn, $"Could not find {zipPathAndName}");
                return false;
            }
            try
            {
                using (var zip = ZipFile.Read(zipPathAndName))
                {

                    if(zip[oldName] != null)
                        zip[oldName].FileName = newName;
                    else
                    {
                        
                        Logger.Log(LogLevel.Warn, $"File {oldName} not found in the archive!");
                    }
                    zip.Save();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(LogLevel.Error, ex, $"Updating files in {zipPathAndName}.zip failed.");
                return false;
            }
            Logger.Log(LogLevel.Trace, "Method finished");
            return true;
        }

        private static bool CheckZipFilesArguments(IEnumerable<string> filePaths, string zipPath)
        {
            Logger.Log(LogLevel.Trace, "Method started");
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
            Logger.Log(LogLevel.Trace, "Method finished");
            return !string.IsNullOrEmpty(fullPath);
        }

        private static bool CheckIfArchiveExists(string archivePath) => File.Exists(archivePath);

        #endregion
    }
}
