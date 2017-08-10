namespace Minify
{
    using Common;
    using Glob;
    using Microsoft.Ajax.Utilities;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static partial class Methods
    {
        public static bool MinifyJs(string pattern, string excludePattern = null, string destination = null, bool ignoreCase = true)
        {
            Logger.Log(LogLevel.Trace, "Method started");
            IEnumerable<FileSystemInfo> files = Glob.Expand(pattern, ignoreCase);

            if (!ValidateGlob(files, pattern))
            {
                Logger.Log(LogLevel.Trace, "Method finished");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(excludePattern))
            {
                IEnumerable<FileSystemInfo> excludedFiles = Glob.Expand(excludePattern, ignoreCase);
                files = files.Where(f => excludedFiles.All(ef => ef.FullName != f.FullName));
            }

            var minifier = new Minifier();
            foreach (FileSystemInfo fileInfo in files)
                minifier.MinifyJavaScriptFile(fileInfo, destination);

            Logger.Log(LogLevel.Info, $"{files.Count()} files minified.");
            Logger.Log(LogLevel.Trace, "Method finished");
            return true;
        }
    }
}
