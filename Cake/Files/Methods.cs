using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
namespace Files
{
    public class Methods
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
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir;
            try
            {
                dir = new DirectoryInfo(sourceDir);
            }
            catch (Exception)
            {
                return false;
            }
            //var dir = new DirectoryInfo(sourceDir);
            if (!dir.Exists)
            {
                //TODO: jakiś error
                return false;
            }
            var dirs = dir.GetDirectories();

            

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destinationDir))
            {
                try
                {
                    Directory.CreateDirectory(destinationDir);
                }
                catch (Exception)
                {
                    return false;
                }

            }
            else
            {
                if (cleanDestinationDirectory)
                    try
                    {
                        CleanDirectory(destinationDir);
                    }
                    catch (Exception)
                    {
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

                }
                catch (Exception)
                {
                    return false;
                }
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (!copySubDirs) return true;
            foreach (var subdir in dirs)
            {
                try
                {
                    var tempPath = Path.Combine(destinationDir, subdir.Name);
                    CopyFolder(subdir.FullName, tempPath, true, overwrite);
                }
                catch (Exception)
                {
                    return false;
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
                return false;
            //TODO: jakiś error
            try
            {
                File.Copy(sourceName, destName, overwrite);
                return true;
            }
            catch (Exception)
            {
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
                return false;
            //TODO: jakiś error
            try
            {
                File.Delete(filePath);
                return true;
            }
            catch (Exception)
            {
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
                return false;
            //TODO: jakiś error
            foreach (var directory in GetFilesWithPattern(parentDirectoryPath, filePattern))
            {
                try
                {
                    DeleteFile(directory);
                }
                catch (Exception)
                {
                    return false;
                }
            }
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
                return false;
            //TODO: jakiś error
            foreach (var directory in Directory.GetDirectories(parentDirectoryPath, directoryPattern, option))
            {
                try
                {
                    DeleteDirectory(directory);

                }
                catch (Exception)
                {
                    return false;
                }

            }
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
                return false;
            //TODO: jakiś error
            try
            {
                Directory.Delete(directoryPath, true);
                return true;
            }
            catch (Exception)
            {
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
                return false;
            //TODO: jakiś error
            try
            {
                var directoriesToClean = Directory.GetDirectories(directoryPath);
                foreach (var dir in directoriesToClean)
                {
                    try
                    {
                        Directory.Delete(dir, true);
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
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
            
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                    
                }        
            }
            catch (Exception)
            {
                return false;
            }
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
                return false;
            //TODO: jakiś error
            try
            {
                File.WriteAllText(filePath, Regex.Replace(File.ReadAllText(filePath), regex, newText));
                return true;
            }
            catch (Exception)
            {
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
                return new[] {filename};
            }
        }
    }
}
