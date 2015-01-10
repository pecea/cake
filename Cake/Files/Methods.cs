using System.Security.Cryptography;
using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
namespace Files
{
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
        public static bool CopyFolder(string sourceDir, string destinationDir, bool copySubDirs = true, bool overwrite = false, bool cleanDestinationDirectory = false)
        {

            DirectoryInfo dir;
            try
            {
                dir = new DirectoryInfo(sourceDir);
            }
            catch (Exception)
            {
                Logger.Log(LogLevel.Error, String.Format("Incorrect directory name: {0}", sourceDir));
                return false;
            }
            if (!dir.Exists)
            {
                Logger.Log(LogLevel.Error, String.Format("Directory {0} not found", sourceDir));
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
                    Logger.Log(LogLevel.Error, String.Format("Could not create {0}", destinationDir));

                    return false;
                }
                Logger.Log(LogLevel.Info, String.Format("Directory {0} created", destinationDir));

            }
            else
            {
                if (cleanDestinationDirectory)
                    try
                    {
                        CleanDirectory(destinationDir);
                        Logger.Log(LogLevel.Info, String.Format("Directory {0} cleaned", destinationDir));
                    }
                    catch (Exception)
                    {
                        Logger.Log(LogLevel.Error, String.Format("Could not clean {0}", destinationDir));

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
                    Logger.Log(LogLevel.Debug, String.Format("File {0} copied", file.Name));
                    

                }
                catch (Exception)
                {
                    Logger.Log(LogLevel.Warn, String.Format("Could not copy {0}", file.Name));
                }
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (!copySubDirs) return true;
            foreach (var subdir in dirs)
            {
                try
                {
                    var tempPath = Path.Combine(destinationDir, subdir.Name);
                    Logger.Log(LogLevel.Info, String.Format("Copying {0} from {1}", subdir.Name, tempPath));
                    CopyFolder(subdir.FullName, tempPath, true, overwrite);
                }
                catch (Exception)
                {
                    Logger.Log(LogLevel.Warn, String.Format("Could not copy {0}", subdir.FullName));
                    
                    //return false;
                }

            }
            return true;
        }

        /// <summary>
        /// Copy a file
        /// </summary>
        /// <param name="sourceName">Path to source file</param>
        /// <param name="destName">Path to destination</param>
        /// <param name="overwrite">If true, file in destination directory is overwritten. Default value is true.</param>
        /// <returns>True, if file was correctly copied</returns>
        public static bool CopyFile(string sourceName, string destName, bool overwrite = true) 
        {
            if (!File.Exists(sourceName))
            {
                Logger.Log(LogLevel.Error, String.Format("Could not find {0}", sourceName));
                
                return false;
            }
            //TODO: jakiś error
            try
            {
                File.Copy(sourceName, destName, overwrite);
                Logger.Log(LogLevel.Info, String.Format("File {0} copied", sourceName));
                    
                return true;
            }
            catch (Exception)
            {
                Logger.Log(LogLevel.Error, String.Format("Could not copy {0}", sourceName));
                 
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
                Logger.Log(LogLevel.Error, String.Format("Could not find {0}", filePath));
                
                return false;
            }
            //TODO: jakiś error
            try
            {
                File.Delete(filePath);
                Logger.Log(LogLevel.Info, String.Format("File {0} deleted", filePath));
                
                return true;
            }
            catch (Exception)
            {
                Logger.Log(LogLevel.Error, String.Format("Could not delete {0}", filePath));
                
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
            if (!Directory.Exists(parentDirectoryPath))
            {
                Logger.Log(LogLevel.Error, String.Format("Could not find {0}", parentDirectoryPath));
                
                return false;
            }
            //TODO: jakiś error
            foreach (var directory in GetFilesWithPattern(parentDirectoryPath, filePattern))
            {
                try
                {
                    DeleteFile(directory);
                    Logger.Log(LogLevel.Debug, String.Format("File {0} deleted", directory));
                
                }
                catch (Exception)
                {
                    Logger.Log(LogLevel.Warn, String.Format("Could not delete {0}", directory));
                
                }
            }

            Logger.Log(LogLevel.Info, String.Format("Files from {0} deleted", parentDirectoryPath));
                
            return true;
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
            var option = subdirectories == true ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            if (!Directory.Exists(parentDirectoryPath))
            {
                Logger.Log(LogLevel.Error, String.Format("Could not find {0}", parentDirectoryPath));
                
                return false;
            }
            //TODO: jakiś error
            foreach (var directory in Directory.GetDirectories(parentDirectoryPath, directoryPattern, option))
            {
                try
                {
                    DeleteDirectory(directory);
                    Logger.Log(LogLevel.Debug, String.Format("Directory {0} deleted", directory));
                

                }
                catch (Exception)
                {
                    Logger.Log(LogLevel.Warn, String.Format("Could not delete {0}", directory));
                
                
                }

            }
            Logger.Log(LogLevel.Info, String.Format("Directories from {0} deleted", parentDirectoryPath));
                
            return true;
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
                Logger.Log(LogLevel.Error, String.Format("Could not find {0}", directoryPath));
                
                return false;
            }
            //TODO: jakiś error
            try
            {
                Directory.Delete(directoryPath, true);
                Logger.Log(LogLevel.Info, String.Format("Directory {0} deleted", directoryPath));
                
                return true;
            }
            catch (Exception)
            {
                Logger.Log(LogLevel.Error, String.Format("Could not delete {0}", directoryPath));
                
                return false;
            }
        }

        /// <summary>
        /// Delete the content of a directory
        /// </summary>
        /// <param name="directoryPath">Path to folder</param>
        /// <returns>True, if all files and folders were correctly deleted</returns>
        public static bool CleanDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Logger.Log(LogLevel.Error, String.Format("Could not find {0}", directoryPath));
                
                return false;
            }
            //TODO: jakiś error
            try
            {
                var directoriesToClean = Directory.GetDirectories(directoryPath);
                foreach (var dir in directoriesToClean)
                {
                    try
                    {
                        Directory.Delete(dir, true);
                        Logger.Log(LogLevel.Debug, String.Format("Directory {0} deleted", dir));
                
                    }
                    catch (Exception)
                    {
                        Logger.Log(LogLevel.Warn, String.Format("Could not delete {0}", dir));
                    }
                }
            }
            catch (Exception)
            {
                Logger.Log(LogLevel.Error, String.Format("Could not clean {0}", directoryPath));
                
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
                        Logger.Log(LogLevel.Debug, String.Format("File {0} deleted", file));
                
            
                    }
                    catch (Exception)
                    {
                        Logger.Log(LogLevel.Warn, String.Format("Could not delete {0}", file));
                
                        //return false;
                    }
                    
                }        
            }
            catch (Exception)
            {
                Logger.Log(LogLevel.Error, String.Format("Could not clean {0}", directoryPath));
                
                return false;
            }
            Logger.Log(LogLevel.Info, String.Format("{0} finished cleaning", directoryPath));
                
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
                Logger.Log(LogLevel.Error, String.Format("Could not find {0}", filePath));
                
                return false;
            }
            //TODO: jakiś error
            try
            {
                File.WriteAllText(filePath, Regex.Replace(File.ReadAllText(filePath), regex, newText));
                Logger.Log(LogLevel.Info, String.Format("File {0} overwritten", filePath));
                
                return true;
            }
            catch (Exception)
            {
                Logger.Log(LogLevel.Error, String.Format("Could not overwrite {0}", filePath));
                
                return false;
            }
        }

        /// <summary>
        /// Look for a file in different folders and return the full path where it is found
        /// </summary>
        /// <param name="filename">Filename</param>
        /// <param name="folders">Path to folders for search</param>
        /// <returns>Paths to file, if any found, filename otherwise</returns>
        public static string[] LookForFileInFolders(string filename, params string[] folders)
        {
            try
            {
                var paths = folders.Select(folder => Path.Combine(folder, filename)).Where(File.Exists).ToList();
                return paths.Count > 0 ? paths.ToArray() : new[] { filename };
            }
            catch (Exception)
            {
                Logger.Log(LogLevel.Warn, String.Format("Incorrect filename or folderPaths"));
                
                return new[] {filename};
            }
        }
    }
}
