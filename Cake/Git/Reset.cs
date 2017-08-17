using Common;
using LibGit2Sharp;
using LogLevel = Common.LogLevel;

namespace Git
{
    public static partial class Methods
    {
        /// <summary>
        /// Performs a soft reset of the changs in the remote repository. (The changes will stay in local repository as "staged").
        /// </summary>
        /// <param name="commitShaHash">Sha of a commit to reset changes to.</param>
        /// <returns>True in case of success, false otherwise.</returns>
        public static bool ResetSoft(string commitShaHash = null) => Reset(ResetMode.Soft, commitShaHash);
        /// <summary>
        /// Performs a mixed reset of the changs in the remote repository. (The changes will stay in local repository as "unstaged").
        /// </summary>
        /// <param name="commitShaHash">Sha of a commit to reset changes to.</param>
        /// <returns>True in case of success, false otherwise.</returns>
        public static bool ResetMixed(string commitShaHash = null) => Reset(ResetMode.Mixed, commitShaHash);
        /// <summary>
        /// Performs a hard reset of the changs in the remote repository. (The changes will not stay in local repository).
        /// </summary>
        /// <param name="commitShaHash">Sha of a commit to reset changes to.</param>
        /// <returns>True in case of success, false otherwise.</returns>
        public static bool ResetHard(string commitShaHash = null) => Reset(ResetMode.Hard, commitShaHash);

        private static bool Reset(ResetMode mode, string commitShaHash)
        {
            Logger.LogMethodStart();

            using (var repo = new Repository(RepositoryPath))
            {
                var commit = string.IsNullOrWhiteSpace(commitShaHash)
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

            Logger.LogMethodEnd();
            return true;
        }
    }
}
