using Microsoft.Ajax.Utilities;
using System.IO;

namespace Minify
{
    public static class Extensions
    {
        public static string MinFullName(this FileSystemInfo fileInfo, string newPath = null)
        {
            string fullName = fileInfo.FullName;
            string extension = fileInfo.Extension;

            if (newPath != null)
                fullName = Path.Combine(newPath, fileInfo.Name);

            return fullName.Insert(fullName.LastIndexOf(extension), ".min");
        }

        public static void MinifyJavaScriptFile(this Minifier minifier, FileSystemInfo fileInfo, string destination)
        {
            string contents = File.ReadAllText(fileInfo.FullName);
            string minifiedContents = minifier.MinifyJavaScript(contents);
            string newFullName = fileInfo.MinFullName(destination);

            using (StreamWriter sw = File.CreateText(newFullName))
                sw.Write(minifiedContents);
        }
    }
}
