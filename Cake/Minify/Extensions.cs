using System;
using System.IO;
using Microsoft.Ajax.Utilities;

namespace Minify
{
    public static class Extensions
    {
        public static string MinFullName(this FileSystemInfo fileInfo, string newPath = null)
        {
            var fullName = fileInfo.FullName;
            var extension = fileInfo.Extension;

            if (newPath != null)
                fullName = Path.Combine(newPath, fileInfo.Name);

            return fullName.Insert(fullName.LastIndexOf(extension, StringComparison.Ordinal), ".min");
        }

        public static void MinifyCssFile(this Minifier minifier, FileSystemInfo fileInfo, string destination)
        {
            MinifyFile(fileInfo, destination, minifier.MinifyStyleSheet);
        }

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
