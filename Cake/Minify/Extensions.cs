using System;
using System.IO;
using Microsoft.Ajax.Utilities;

namespace Minify
{
    /// <summary>
    /// Encloses extension methods used with minifying files.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets minified file name
        /// </summary>
        /// <param name="fileInfo"><see cref="FileSystemInfo"/> of the file for minification</param>
        /// <param name="newPath">Path for the minified file</param>
        /// <returns>Minified file name</returns>
        public static string MinFullName(this FileSystemInfo fileInfo, string newPath = null)
        {
            var fullName = fileInfo.FullName;
            var extension = fileInfo.Extension;

            if (newPath != null)
                fullName = Path.Combine(newPath, fileInfo.Name);

            return fullName.Insert(fullName.LastIndexOf(extension, StringComparison.Ordinal), ".min");
        }
        /// <summary>
        /// Minifies .css file
        /// </summary>
        /// <param name="minifier"><see cref="Minifier"/> which performs minification</param>
        /// <param name="fileInfo"><see cref="FileSystemInfo"/> of the file for minification</param>
        /// <param name="destination">Path for the minified file</param>
        public static void MinifyCssFile(this Minifier minifier, FileSystemInfo fileInfo, string destination)
        {
            MinifyFile(fileInfo, destination, minifier.MinifyStyleSheet);
        }
        /// <summary>
        /// Minifies .js file
        /// </summary>
        /// <param name="minifier"><see cref="Minifier"/> which performs minification</param>
        /// <param name="fileInfo"><see cref="FileSystemInfo"/> of the file for minification</param>
        /// <param name="destination">Path for the minified file</param>
        public static void MinifyJavaScriptFile(this Minifier minifier, FileSystemInfo fileInfo, string destination)
        {
            MinifyFile(fileInfo, destination, minifier.MinifyJavaScript);
        }

        private static void MinifyFile(FileSystemInfo fileInfo, string destination, Func<string, string> minifyFunc)
        {
            var contents = File.ReadAllText(fileInfo.FullName);
            var minifiedContents = minifyFunc(contents);
            var newFullName = fileInfo.MinFullName(destination);

            using (var sw = File.CreateText(newFullName))
                sw.Write(minifiedContents);
        }
    }
}
