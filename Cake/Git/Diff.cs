using System.Linq;
using Common;
using LibGit2Sharp;
using LogLevel = Common.LogLevel;

namespace Git
{
    public static partial class Methods
    {
        /// <summary>
        /// Lists changes between local index and HEAD.
        /// </summary>
        /// <returns>True in case of success, otherwise false</returns>
        public static bool DiffStaged() => Diff(DiffTargets.Index);

        /// <summary>
        /// Lists changes between working directory and HEAD.
        /// </summary>
        /// /// <returns>True in case of success, otherwise false</returns>
        public static bool DiffWorkingDir() => Diff(DiffTargets.WorkingDirectory);

        /// <summary>
        /// Lists changes between both local index and working directory and HEAD.
        /// </summary>
        /// <returns>True in case of success, otherwise false</returns>
        public static bool DiffAll() => Diff(DiffTargets.Index, DiffTargets.WorkingDirectory);

        private static bool Diff(params DiffTargets[] modes)
        {
            Logger.LogMethodStart();

            using (var repo = new Repository(RepositoryPath))
            {
                foreach (var mode in modes)
                {
                    Logger.Log(LogLevel.Info, $"{mode} vs HEAD:");

                    var changes = repo.Diff.Compare<TreeChanges>(repo.Head.Tip.Tree, mode);
                    foreach (var c in changes)
                        Logger.Log(LogLevel.Info, $"{c.Path} was {c.Status.ToString().ToLower()}.");

                    if (!changes.Any())
                        Logger.Log(LogLevel.Info, "No changes.");
                }
            }

            Logger.LogMethodEnd();
            return true;
        }
    }
}
