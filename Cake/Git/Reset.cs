using Common;
using LibGit2Sharp;
using LogLevel = Common.LogLevel;

namespace Git
{
    public static partial class Methods
    {
        public static bool ResetSoft(string commitShaHash = null) => Reset(ResetMode.Soft, commitShaHash);

        public static bool ResetMixed(string commitShaHash = null) => Reset(ResetMode.Mixed, commitShaHash);

        public static bool ResetHard(string commitShaHash = null) => Reset(ResetMode.Hard, commitShaHash);

        private static bool Reset(ResetMode mode, string commitShaHash)
        {
            Logger.Log(LogLevel.Trace, "Method started");

            using (var repo = new Repository(RepositoryPath))
            {
                Commit commit = string.IsNullOrWhiteSpace(commitShaHash)
                    ? repo.Head.Tip
                    : repo.Lookup<Commit>(commitShaHash);

                if (commit == null)
                {
                    Logger.Log(LogLevel.Error, $"Couldn't retrieve commit {commitShaHash}. Reset will not be performed.");
                    return false;
                }

                repo.Reset(mode, commit);
                Logger.Log(LogLevel.Info, $"Repository reset {mode} to commit {commit.Id}.");
            }

            Logger.Log(LogLevel.Trace, "Method finished");
            return true;
        }
    }
}
