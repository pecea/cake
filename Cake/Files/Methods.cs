namespace Files
{
    using Common;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    /// <summary>
    /// Encloses methods used with operations on files.
    /// </summary>
    public static class Methods
    {

        
        /// <summary>
        /// Copy folder content to another folder
        /// </summary>
        /// <param name="sourceDir">Path to source folder</param>
        /// <param name="destinationDir">Path to destination folder</param>
        /// <param name="copySubDirs">If true, all subdirectories are also copied. Default value is true.</param>
        /// <param name="overwrite">If true, files in destination directory are overwriten. Default value is false.</param>
        /// <param name="cleanDestinationDirectory">If true, all files in destination directory are deleted before the operation. Default value is false.</param>
        /// <returns>True, if copying succedeed</returns>
        public static bool CopyDirectory(string sourceDir, string destinationDir, bool copySubDirs = true, bool overwrite = false, bool cleanDestinationDirectory = false)
        {
            var res = true;
            DirectoryInfo dir;
            try
            {
                dir = new DirectoryInfo(sourceDir);
            }
            catch (Exception)
            {
                Logger.Log(LogLevel.Error, $"Incorrect directory name: {sourceDir}");
                return false;
            }
            if (!dir.Exists)
            {
                Logger.Log(LogLevel.Error, $"Directory {sourceDir} not found");
                return false;
            }
            // Get the subdirectories for the specified directory.
            var dirs = dir.GetDirectories();

            //

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destinationDir))
            {
                try
                {
                    Directory.CreateDirectory(destinationDir);
                }
                catch (Exception)
                {
                    Logger.Log(LogLevel.Error, $"Could not create {destinationDir}");

                    return false;
                }
                Logger.Log(LogLevel.Info, $"Directory {destinationDir} created");

            }
            else
            {
                if (cleanDestinationDirectory)
                    try
                    {
                        res &= CleanDirectory(destinationDir);
                        Logger.Log(LogLevel.Info, $"Directory {destinationDir} cleaned");
                    }
                    catch (Exception)
                    {
                        Logger.Log(LogLevel.Error, $"Could not clean {destinationDir}");

                        return false;
                    }

            }

            // Get the files in the directory and copy them to the new location.
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                try
                {
                    var tempPath = Path.Combine(destinationDir, file.Name);
                    file.CopyTo(tempPath, overwrite);
                    Logger.Log(LogLevel.Debug, $"File {file.Name} copied");
                    

                }
                catch (Exception)
                {
                    Logger.Log(LogLevel.Warn, $"Could not copy {file.Name}");
                }
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (!copySubDirs) return res;
            foreach (var subdir in dirs)
            {
                try
                {
                    var tempPath = Path.Combine(destinationDir, subdir.Name);
                    Logger.Log(LogLevel.Info, $"Copying {subdir.Name} from {tempPath}");
                    res &= CopyDirectory(subdir.FullName, tempPath, true, overwrite);
                }
                catch (Exception)
                {
                    Logger.Log(LogLevel.Warn, $"Could not copy {subdir.FullName}");
                    
                    //return false;
                }

            }
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
            if (!File.Exists(sourceName))
            {
                Logger.Log(LogLevel.Error, $"Could not find {sourceName}");
                
                return false;
            }
            try
            {
                File.Copy(sourceName, destName, overwrite);
                Logger.Log(LogLevel.Info, $"File {sourceName} copied");
                    
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(LogLevel.Error, ex, $"Could not copy {sourceName}");
                 
                return false;
            }

        }

        /// <summary>
        /// Delete a file
        /// </summary>
        /// <param name="filePath">Path to file</param>
        /// <returns>True, if file was correctly deleted</returns>
        public static bool DeleteFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Logger.Log(LogLevel.Error, $"Could not find {filePath}");
                
                return false;
            }
            try
            {
                File.Delete(filePath);
                Logger.Log(LogLevel.Info, $"File {filePath} deleted");
                
                return true;
            }
            catch (Exception)
            {
                Logger.Log(LogLevel.Error, $"Could not delete {filePath}");
                
                return false;
            }
            
        }

        /// <summary>
        /// Get all the files of a directory following a regex patern
        /// </summary>
        /// <param name="parentDirectoryPath">Path to folder</param>
        /// <param name="filePattern">Search pattern</param>
        /// <param name="subdirectories">If true, search all subdirectories</param>
        /// <returns>Names of files that match the pattern</returns>
        public static string[] GetFilesWithPattern(string parentDirectoryPath, string filePattern, bool subdirectories = false)
        {
            return !Directory.Exists(parentDirectoryPath) ? new string[0] : Directory.GetFiles(parentDirectoryPath, filePattern, subdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// Delete all the files of a directory following a regex patern
        /// </summary>
        /// <param name="parentDirectoryPath">Path to folder</param>
        /// <param name="filePattern">Delete pattern</param>
        /// <returns>True, if files were correctly deleted</returns>
        public static bool DeleteFilesWithPattern(string parentDirectoryPath, string filePattern)
        {
            var res = true;
            if (!Directory.Exists(parentDirectoryPath))
            {
                Logger.Log(LogLevel.Error, $"Could not find {parentDirectoryPath}");
                
                return false;
            }
            foreach (var directory in GetFilesWithPattern(parentDirectoryPath, filePattern))
            {
                try
                {
                    res &= DeleteFile(directory);
                    Logger.Log(LogLevel.Debug, $"File {directory} deleted");
                
                }
                catch (Exception)
                {
                    Logger.Log(LogLevel.Warn, $"Could not delete {directory}");
                
                }
            }

            Logger.Log(LogLevel.Info, $"Files from {parentDirectoryPath} deleted");
                
            return res;
        }

        /// <summary>
        /// Delete all the subdirectories of a directory following a regex patern
        /// </summary>
        /// <param name="parentDirectoryPath">Path to folder</param>
        /// <param name="directoryPattern">Delete pattern</param>
        /// <param name="subdirectories">If true, recursive</param>
        /// <returns>True, if directories where correctly deleted</returns>
        public static bool DeleteDirectoriesWithPattern(string parentDirectoryPath, string directoryPattern, bool subdirectories = false)
        {
            var res = true;
            var option = subdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            if (!Directory.Exists(parentDirectoryPath))
            {
                Logger.Log(LogLevel.Error, $"Could not find {parentDirectoryPath}");
                
                return false;
            }
            foreach (var directory in Directory.GetDirectories(parentDirectoryPath, directoryPattern, option))
            {
                try
                {
                    res &= DeleteDirectory(directory);
                    Logger.Log(LogLevel.Debug, $"Directory {directory} deleted");
                

                }
                catch (Exception)
                {
                    Logger.Log(LogLevel.Warn, $"Could not delete {directory}");
                
                
                }

            }
            Logger.Log(LogLevel.Info, $"Directories from {parentDirectoryPath} deleted");
                
            return res;
        }

        /// <summary>
        /// Delete a directory
        /// </summary>
        /// <param name="directoryPath">Path to folder</param>
        /// <returns>True, if folder was correctly deleted</returns>
        public static bool DeleteDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Logger.Log(LogLevel.Error, $"Could not find {directoryPath}");
                
                return false;
            }
            try
            {
                Directory.Delete(directoryPath, true);
                Logger.Log(LogLevel.Info, $"Directory {directoryPath} deleted");
                
                return true;
            }
            catch (Exception)
            {
                Logger.Log(LogLevel.Error, $"Could not delete {directoryPath}");
                
                return false;
            }
        }

        /// <summary>
        /// Delete the content of a directory
        /// </summary>
        /// <param name="directoryPath">Path to folder</param>
        /// <returns>True, if all files and folders were correctly deleted</returns>
        public static bool CleanDirectory(string directoryPath) //TODO: check whether files outside directory are also cleaned :)
        {
            if (!Directory.Exists(directoryPath))
            {
                Logger.Log(LogLevel.Error, $"Could not find {directoryPath}");
                
                return false;
            }
            try
            {
                var directoriesToClean = Directory.GetDirectories(directoryPath);
                foreach (var dir in directoriesToClean)
                {
                    try
                    {
                        Directory.Delete(dir, true);
                        Logger.Log(LogLevel.Debug, $"Directory {dir} deleted");
                
                    }
                    catch (Exception)
                    {
                        Logger.Log(LogLevel.Warn, $"Could not delete {dir}");
                    }
                }
            }
            catch (Exception)
            {
                Logger.Log(LogLevel.Error, $"Could not clean {directoryPath}");
                
                return false;
            }

            try
            {
                var filesToClean = Directory.GetFiles(directoryPath);
                foreach (var file in filesToClean)
                {
                    try
                    {
                        File.Delete(file);
                        Logger.Log(LogLevel.Debug, $"File {file} deleted");
                
            
                    }
                    catch (Exception)
                    {
                        Logger.Log(LogLevel.Warn, $"Could not delete {file}");
                
                        //return false;
                    }
                    
                }        
            }
            catch (Exception)
            {
                Logger.Log(LogLevel.Error, $"Could not clean {directoryPath}");
                
                return false;
            }
            Logger.Log(LogLevel.Info, $"{directoryPath} finished cleaning");
                
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
            if (!File.Exists(filePath))
            {
                Logger.Log(LogLevel.Error, $"Could not find {filePath}");
                
                return false;
            }
            try
            {
                File.WriteAllText(filePath, Regex.Replace(File.ReadAllText(filePath), regex, newText));
                Logger.Log(LogLevel.Info, $"File {filePath} overwritten");
                
                return true;
            }
            catch (Exception)
            {
                Logger.Log(LogLevel.Error, $"Could not overwrite {filePath}");
                
                return false;
            }
        }

        /// <summary>
        /// Look for a file in different folders and return the full path where it is found
        /// </summary>
        /// <param name="filename">Filename</param>
        /// <param name="directories">Path to folders to search files in</param>
        /// <returns>Paths to file, if any found, filename otherwise</returns>
        public static string[] LookForFileInDirectories(string filename, params string[] directories)
        {
            try
            {
                var paths = directories.Select(directory => Path.Combine(directory, filename)).Where(File.Exists).ToList();
                return paths.Count > 0 ? paths.ToArray() : new[] { filename };
            }
            catch (Exception)
            {
                Logger.Log(LogLevel.Warn, "Incorrect filename or directories paths");
                
                return new[] {filename};
            }
        }
    }
}
