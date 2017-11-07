using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Common;

namespace Files
{
    /// <summary>
    /// Encloses methods used with operations on files.
    /// </summary>
    public static class Methods
    {
        /// <summary>
        /// Copy directory content to another directory
        /// </summary>
        /// <param name="sourceDir">Path to source directory</param>
        /// <param name="destinationDir">Path to destination directory</param>
        /// <param name="copySubDirs">If true, all subdirectories are also copied. Default value is true.</param>
        /// <param name="overwrite">If true, files in destination directory are overwriten. Default value is false.</param>
        /// <param name="cleanDestinationDirectory">If true, all files in destination directory are deleted before the operation. Default value is false.</param>
        /// <returns>True, if copying succedeed</returns>
        public static bool CopyDirectory(string sourceDir, string destinationDir, bool copySubDirs = true, bool overwrite = false, bool cleanDestinationDirectory = false)
        {
            Logger.LogMethodStart();
            var res = true;

            var dir = new DirectoryInfo(sourceDir);
            if (!dir.Exists)
            {
                Logger.Log(LogLevel.Warn, $"Directory {sourceDir} not found.");
                return false;
            }

            // Get the subdirectories for the specified directory.
            var dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
                Logger.Log(LogLevel.Info, $"Directory {destinationDir} created.");
            }
            else if (cleanDestinationDirectory)
            {
                res &= CleanDirectory(destinationDir);
                Logger.Log(LogLevel.Info, $"Directory {destinationDir} cleaned.");
            }

            // Get the files in the directory and copy them to the new location.
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                try
                {
                    var tempPath = Path.Combine(destinationDir, file.Name);
                    file.CopyTo(tempPath, overwrite);
                    Logger.Log(LogLevel.Debug, $"File {file.Name} copied.");
                }
                catch (Exception ex)
                {
                    Logger.LogException(LogLevel.Error, ex, $"Could not copy {file.Name}.");
                }
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (var subdir in dirs)
                {
                    try
                    {
                        var tempPath = Path.Combine(destinationDir, subdir.Name);
                        Logger.Log(LogLevel.Info, $"Copying {subdir.Name} from {tempPath}.");
                        res &= CopyDirectory(subdir.FullName, tempPath, true, overwrite);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(LogLevel.Error, ex, $"Could not copy {subdir.FullName}.");
                    }
                }
            }

            Logger.LogMethodEnd();
            return res;
        }

        /// <summary>
        /// Copy a file
        /// </summary>
        /// <param name="sourceName">Path to source file</param>
        /// <param name="destName">Path to destination</param>
        /// <param name="overwrite">If true, file in destination directory is overwritten. Default value is true.</param>
        /// <returns>True, if file was correctly copied</returns>
        public static bool CopyFile(string sourceName, string destName, bool overwrite = true) //TODO: check whether overwrite works - probably when it's set to false, but there is already a file with the name of source file, we get an exception
        {
            Logger.LogMethodStart();
            if (!File.Exists(sourceName))
            {
                Logger.Log(LogLevel.Warn, $"Could not find {sourceName}.");
                return false;
            }

            File.Copy(sourceName, destName, overwrite);
            Logger.Log(LogLevel.Info, $"File {sourceName} copied.");

            Logger.LogMethodEnd();
            return true;
        }

        /// <summary>
        /// Delete a file
        /// </summary>
        /// <param name="filePath">Path to file</param>
        /// <returns>True, if file was correctly deleted</returns>
        public static bool DeleteFile(string filePath)
        {
            Logger.LogMethodStart();
            if (!File.Exists(filePath))
            {
                Logger.Log(LogLevel.Warn, $"Could not find {filePath}");
                return false;
            }

            File.Delete(filePath);
            Logger.Log(LogLevel.Info, $"File {filePath} deleted");

            Logger.LogMethodEnd();
            return true;
        }

        /// <summary>
        /// Get all the files of a directory following a regex patern
        /// </summary>
        /// <param name="parentDirectoryPath">Path to directory</param>
        /// <param name="filePattern">Search pattern</param>
        /// <param name="subdirectories">If true, search all subdirectories</param>
        /// <returns>Names of files that match the pattern</returns>
        public static string[] GetFilesWithPattern(string parentDirectoryPath, string filePattern, bool subdirectories = false)
        {
            Logger.LogMethodStart();
            var searchOption = subdirectories
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;

            var result = Directory.Exists(parentDirectoryPath)
                ? Directory.GetFiles(parentDirectoryPath, filePattern, searchOption).Select(path => path.Replace('\\', '/')).ToArray()
                : new string[0];

            Logger.Log(LogLevel.Info, "Result:");
            foreach (var path in result)
                Logger.Log(LogLevel.Info, $"{path}");

            Logger.LogMethodEnd();
            return result;
        }

        /// <summary>
        /// Delete all the files of a directory following a regex patern
        /// </summary>
        /// <param name="parentDirectoryPath">Path to directory</param>
        /// <param name="filePattern">Delete pattern</param>
        /// <returns>True, if files were correctly deleted</returns>
        public static bool DeleteFilesWithPattern(string parentDirectoryPath, string filePattern)
        {
            Logger.LogMethodStart();
            var res = true;

            if (!Directory.Exists(parentDirectoryPath))
            {
                Logger.Log(LogLevel.Warn, $"Could not find {parentDirectoryPath}");
                return false;
            }

            foreach (var file in GetFilesWithPattern(parentDirectoryPath, filePattern))
            {
                try
                {
                    res &= DeleteFile(file);
                    Logger.Log(LogLevel.Debug, $"File {file} deleted");
                }
                catch (Exception ex)
                {
                    Logger.LogException(LogLevel.Error, ex, $"Could not delete {file}");
                }
            }

            Logger.Log(LogLevel.Info, $"Files from {parentDirectoryPath} deleted");

            Logger.LogMethodEnd();
            return res;
        }

        /// <summary>
        /// Delete all the subdirectories of a directory following a regex patern
        /// </summary>
        /// <param name="parentDirectoryPath">Path to directory</param>
        /// <param name="directoryPattern">Delete pattern</param>
        /// <param name="subdirectories">If true, recursive</param>
        /// <returns>True, if directories where correctly deleted</returns>
        public static bool DeleteDirectoriesWithPattern(string parentDirectoryPath, string directoryPattern, bool subdirectories = false)
        {
            Logger.LogMethodStart();
            var res = true;

            if (!Directory.Exists(parentDirectoryPath))
            {
                Logger.Log(LogLevel.Warn, $"Could not find {parentDirectoryPath}");
                return false;
            }

            var option = subdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var directory in Directory.GetDirectories(parentDirectoryPath, directoryPattern, option))
            {
                try
                {
                    res &= DeleteDirectory(directory);
                    Logger.Log(LogLevel.Debug, $"Directory {directory} deleted");
                }
                catch (Exception ex)
                {
                    Logger.LogException(LogLevel.Error, ex, $"Could not delete {directory}");
                }
            }

            Logger.Log(LogLevel.Info, $"Directories from {parentDirectoryPath} deleted");

            Logger.LogMethodEnd();
            return res;
        }

        /// <summary>
        /// Delete a directory
        /// </summary>
        /// <param name="directoryPath">Path to directory</param>
        /// <returns>True, if directory was correctly deleted</returns>
        public static bool DeleteDirectory(string directoryPath)
        {
            Logger.LogMethodStart();
            if (!Directory.Exists(directoryPath))
            {
                Logger.Log(LogLevel.Warn, $"Could not find {directoryPath}");
                return false;
            }

            Directory.Delete(directoryPath, true);
            Logger.Log(LogLevel.Info, $"Directory {directoryPath} deleted");

            Logger.LogMethodEnd();
            return true;
        }

        /// <summary>
        /// Delete the content of a directory
        /// </summary>
        /// <param name="directoryPath">Path to directory</param>
        /// <returns>True, if all files and directories were correctly deleted</returns>
        public static bool CleanDirectory(string directoryPath) //TODO: check whether files outside directory are also cleaned :)
        {
            Logger.LogMethodStart();

            if (!Directory.Exists(directoryPath))
            {
                Logger.Log(LogLevel.Warn, $"Could not find {directoryPath}");
                return false;
            }

            var directoriesToClean = Directory.GetDirectories(directoryPath);
            foreach (var dir in directoriesToClean)
            {
                try
                {
                    Directory.Delete(dir, true);
                    Logger.Log(LogLevel.Debug, $"Directory {dir} deleted");
                }
                catch (Exception ex)
                {
                    Logger.LogException(LogLevel.Error, ex, $"Could not delete {dir}");
                }
            }

            var filesToClean = Directory.GetFiles(directoryPath);
            foreach (var file in filesToClean)
            {
                try
                {
                    File.Delete(file);
                    Logger.Log(LogLevel.Debug, $"File {file} deleted");
                }
                catch (Exception ex)
                {
                    Logger.LogException(LogLevel.Error, ex, $"Could not delete {file}");
                }
            }

            Logger.Log(LogLevel.Info, $"{directoryPath} finished cleaning");
            Logger.LogMethodEnd();

            return true;
        }

        /// <summary>
        /// Replace a text following a regular expression in a file by another text
        /// </summary>
        /// <param name="filePath">Path to file</param>
        /// <param name="regex">Regular expression to replace</param>
        /// <param name="newText">New expression</param>
        /// <returns>True, if file was correctly overwritten</returns>
        public static bool ReplaceText(string filePath, string regex, string newText)
        {
            Logger.LogMethodStart();

            if (!File.Exists(filePath))
            {
                Logger.Log(LogLevel.Warn, $"Could not find {filePath}");
                return false;
            }

            File.WriteAllText(filePath, Regex.Replace(File.ReadAllText(filePath), regex, newText));
            Logger.Log(LogLevel.Info, $"File {filePath} overwritten");

            Logger.LogMethodEnd();
            return true;
        }

        /// <summary>
        /// Look for a file in different directories and return the full path where it is found
        /// </summary>
        /// <param name="filename">Filename</param>
        /// <param name="directories">Path to directories to search files in</param>
        /// <returns>Paths to file, if any found, filename otherwise</returns>
        public static string[] LookForFileInDirectories(string filename, params string[] directories)
        {
            Logger.LogMethodStart();
            string[] res;

            try
            {
                var paths = directories.Select(directory => Path.Combine(directory, filename)).Where(File.Exists).ToList();
                res = paths.Count > 0 ? paths.ToArray() : new[] { filename };
            }
            catch (Exception ex)
            {
                Logger.LogException(LogLevel.Error, ex, "Incorrect filename or directories paths.");
                res = new[] { filename };
            }

            Logger.Log(LogLevel.Info, "Result:");
            foreach (var path in res)
                Logger.Log(LogLevel.Info, $"{path}");

            Logger.LogMethodEnd();
            return res;
        }

        /// <summary>
        /// Writes content of a file to the standard output.
        /// </summary>
        /// <param name="fileName">Path and name of the file</param>
        /// <returns>True in case of success, otherwise false.</returns>
        public static bool WriteFile(string fileName)
        {
            Logger.LogMethodStart();

            if (!File.Exists(fileName))
            {
                Logger.Log(LogLevel.Warn, $"Could not find {fileName}.");
                return false;
            }

            Logger.Log(LogLevel.Info, File.ReadAllText(fileName));

            Logger.LogMethodEnd();
            return true;
        }
    }
}
