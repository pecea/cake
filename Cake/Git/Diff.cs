using Common;
using LibGit2Sharp;
using System.Linq;
using LogLevel = Common.LogLevel;

namespace Git
{
    public static partial class Methods
    {
        /// <summary>
        /// Lists changes between local index and HEAD.
        /// </summary>
        /// <returns></returns>
        public static bool DiffStaged() => Diff(DiffTargets.Index);

        /// <summary>
        /// Lists changes between working directory and HEAD.
        /// </summary>
        public static bool DiffWorkingDir() => Diff(DiffTargets.WorkingDirectory);

        /// <summary>
        /// Lists changes between both local index and working directory and HEAD.
        /// </summary>
        public static bool DiffAll() => Diff(DiffTargets.Index, DiffTargets.WorkingDirectory);

        private static bool Diff(params DiffTargets[] modes)
        {
            Logger.Log(LogLevel.Trace, "Method started");

            using (var repo = new Repository(RepositoryPath))
            {
                foreach (DiffTargets mode in modes)
                {
                    Logger.Log(LogLevel.Info, $"{mode} vs HEAD:");

                    var changes = repo.Diff.Compare<TreeChanges>(repo.Head.Tip.Tree, mode);
                    foreach (TreeEntryChanges c in changes)
                        Logger.Log(LogLevel.Info, $"{c.Path} was {c.Status.ToString().ToLower()}.");

                    if (!changes.Any())
                        Logger.Log(LogLevel.Info, "No changes.");
                }
            }

            Logger.Log(LogLevel.Trace, "Method finished");
            return true;
        }
    }
}
