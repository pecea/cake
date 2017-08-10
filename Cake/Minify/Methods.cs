namespace Minify
{
    using Common;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static partial class Methods
    {
        private static bool ValidateGlob(IEnumerable<FileSystemInfo> files, string pattern)
        {
            if (!files.Any())
            {
                Logger.Log(LogLevel.Warn, $"Pattern {pattern} did not match any files.");
                return false;
            }

            if (files.Any(f => f.Attributes == FileAttributes.Directory))
            {
                string directories = string.Join(
                    separator: ",\n",
                    values: files.Where(f => f.Attributes == FileAttributes.Directory).Select(f => f.FullName)
                );

                Logger.Log(LogLevel.Warn, $"Pattern {pattern} matched directories: {directories}.");
                return false;
            }

            return true;
        }
    }
}
