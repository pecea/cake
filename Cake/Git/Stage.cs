using System.Collections.Generic;
using System.Linq;
using Common;
using LibGit2Sharp;
using LogLevel = Common.LogLevel;

namespace Git
{
    public static partial class Methods
    {
        /// <summary>
        /// Informs local repository about changes in a file.
        /// </summary>
        /// <param name="path">Path to the file for staging</param>
        /// <returns>True in case of success, false otherwise.</returns>
        public static bool Stage(string path) => Stage(new[] { path });
        /// <summary>
        /// Informs local repository about changes in files.
        /// </summary>
        /// <param name="paths">Paths to files for staging</param>
        /// <returns>True in case of success, false otherwise.</returns>
        public static bool Stage(IEnumerable<string> paths = null)
        {
            using (var repo = new Repository(RepositoryPath))
                return Stage(repo, paths);
        }

        private static bool Stage(IRepository repo, IEnumerable<string> paths = null)
        {
            Logger.LogMethodStart();

            if (paths == null)
            {
                paths = repo.Diff
                    .Compare<Patch>(repo.Head.Tip.Tree, DiffTargets.WorkingDirectory)
                    .Select(c => c.Path);
            }

            var enumerable = paths as string[] ?? paths.ToArray();
            if (!enumerable.Any())
            {
                Logger.Log(LogLevel.Warn, "No files to stage!");
                return false;
            }

            Logger.Log(LogLevel.Info, $"Staging files:\n{string.Join("\n", enumerable)}.");
            Commands.Stage(repo, enumerable);

            Logger.LogMethodEnd();
            return true;
        }
    }
}
