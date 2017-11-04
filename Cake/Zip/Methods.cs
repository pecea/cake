using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;
using Ionic.Zip;
using Ionic.Zlib;

namespace Zip
{
    /// <summary>
    /// Encloses methods used with zipping files.
    /// </summary>
    public static class Methods
    {
        /// <summary>
        /// Adds files and directories to .zip archive
        /// </summary>
        /// <param name="zipPathAndName">Path and name of the zip file</param>
        /// <param name="entriesPaths">Paths to files and directories for zipping</param>
        /// <returns>True, if zipping was successful, false otherwise</returns>
        public static bool ZipFiles(string zipPathAndName, params string[] entriesPaths)
        {
            Logger.LogMethodStart();
            var paths = new List<string>();
            foreach (var filePath in entriesPaths)
            {
                paths.AddRange(filePath.GetFilePaths());
                paths.AddRange(filePath.GetDirectoriesPaths());
            }

            if (!CheckZipFilesArguments(paths, zipPathAndName)) return false;

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

            Logger.LogMethodEnd();
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
            Logger.LogMethodStart();
            var paths = new List<string>();
            foreach (var filePath in filePaths)
            {
                paths.AddRange(filePath.GetFilePaths());
                paths.AddRange(filePath.GetDirectoriesPaths());
            }

            if (!CheckZipFilesArguments(paths, zipPathAndName)) return false;

            using (var zip = new ZipFile())
            {
                if (useZip64)
                    zip.UseZip64WhenSaving = Zip64Option.AsNecessary;
                if (!string.IsNullOrEmpty(password))
                    zip.Password = password;
                if (aes256Encryption)
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


            Logger.LogMethodEnd();
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
            Logger.LogMethodStart();
            if (!CheckIfArchiveExists(zipPathAndName))
            {
                Logger.Log(LogLevel.Warn, $"Could not find {zipPathAndName}.");
                return false;
            }

            using (var zip = ZipFile.Read(zipPathAndName))
            {
                foreach (var entry in zip)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        if (overwrite)
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
                Logger.Log(LogLevel.Info, $"{zipPathAndName} unzipped succesfully.");
            }

            Logger.LogMethodEnd();
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
            Logger.LogMethodStart();
            if (!CheckIfArchiveExists(zipPathAndName))
            {
                Logger.Log(LogLevel.Warn, $"Could not find {zipPathAndName}.");
                return false;
            }

            using (var zip = ZipFile.Read(zipPathAndName))
            {
                zip.RemoveEntries(entriesToDelete);
                zip.Save();
            }

            Logger.LogMethodEnd();
            return true;
        }

        /// <summary>
        /// Removes specified entries from the archive.
        /// </summary>
        /// <param name="zipPathAndName">Path and name of the modified archive</param>
        /// <param name="entriesToUpdate">Names of entries to be updated</param>
        /// <returns>True, if updating was successful, false otherwise</returns>
        public static bool UpdateEntries(string zipPathAndName, params string[] entriesToUpdate)
        {
            Logger.LogMethodStart();
            if (!CheckIfArchiveExists(zipPathAndName))
            {
                Logger.Log(LogLevel.Warn, $"Could not find {zipPathAndName}.");
                return false;
            }

            using (var zip = ZipFile.Read(zipPathAndName))
            {
                foreach (var entry in entriesToUpdate)
                {
                    var mod = entry;
                    var attributes = File.GetAttributes(mod);
                    if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        mod = mod.Replace('\\', '/').Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault() + '/';
                        zip.UpdateItem(entry, mod);
                    }
                    else
                        zip.UpdateItem(entry);
                }
                zip.Save();
            }


            Logger.LogMethodEnd();
            return true;
        }
        /// <summary>
        /// Renames a file in the arhcive
        /// </summary>
        /// <param name="zipPathAndName">Path and name of the archive</param>
        /// <param name="oldName">Old name of the entry to be renamed</param>
        /// <param name="newName">New name of the entry to be renamed</param>
        /// <returns>True, if renaming was successful, false otherwise</returns>
        public static bool RenameEntry(string zipPathAndName, string oldName, string newName)
        {
            Logger.LogMethodStart();
            if (!CheckIfArchiveExists(zipPathAndName))
            {
                Logger.Log(LogLevel.Warn, $"Could not find {zipPathAndName}.");
                return false;
            }

            using (var zip = ZipFile.Read(zipPathAndName))
            {
                if (zip[oldName] != null)
                    zip[oldName].FileName = newName;
                else
                {
                    Logger.Log(LogLevel.Warn, $"File {oldName} not found in the archive!");
                }
                zip.Save();
            }

            Logger.LogMethodEnd();
            return true;
        }

        private static bool CheckZipFilesArguments(IEnumerable<string> filePaths, string zipPath)
        {
            Logger.LogMethodStart();

            if (filePaths == null || !filePaths.Any())
                throw new ArgumentException("No file paths provided.", nameof(filePaths));

            var invalidPaths = filePaths.Where(p => !File.Exists(p) && !Directory.Exists(p));
            if (invalidPaths.Any())
                throw new ArgumentException($"Could not find parts of paths:\n{string.Join(Environment.NewLine, invalidPaths)}).", nameof(filePaths));

            if (!string.IsNullOrWhiteSpace(zipPath) && !Uri.IsWellFormedUriString(zipPath, UriKind.RelativeOrAbsolute))
                throw new ArgumentException($"'{zipPath}' is not a valid path.", nameof(zipPath));

            return true;
        }

        private static bool CheckIfArchiveExists(string archivePath)
        {
            if (!File.Exists(archivePath))
                throw new FileNotFoundException($"Could not find a part of the path: '{Path.GetFullPath(archivePath)}'.");

            return true;
        }
    }
}
