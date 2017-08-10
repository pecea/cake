using Common;
using LibGit2Sharp;
using System.Collections.Generic;
using System.Linq;
using LogLevel = Common.LogLevel;

namespace Git
{
    public static partial class Methods
    {
        public static bool Stage(string path) => Stage(new string[] { path });

        public static bool Stage(IEnumerable<string> paths = null)
        {
            using (var repo = new Repository(RepositoryPath))
                return Stage(repo, paths);
        }

        private static bool Stage(IRepository repo, IEnumerable<string> paths = null)
        {
            Logger.Log(LogLevel.Trace, "Method started");

            if (paths == null)
            {
                paths = repo.Diff
                    .Compare<Patch>(repo.Head.Tip.Tree, DiffTargets.WorkingDirectory)
                    .Select(c => c.Path);
            }

            if (!paths.Any())
            {
                Logger.Log(LogLevel.Warn, "No files to stage!");
                return false;
            }

            Logger.Log(LogLevel.Info, $"Staging files:\n{string.Join("\n", paths)}.");
            Commands.Stage(repo, paths);

            return true;
        }
    }
}
