using Common;
using LibGit2Sharp;
using LogLevel = Common.LogLevel;

namespace Git
{
    /// <summary>
    /// Encloses methods used with pulling files in git.
    /// </summary>
    public static partial class Methods
    {
        /// <summary>
        /// Performs a pull of remote changes to your local repository with automatic merge.
        /// </summary>
        /// <returns>True in case of success, false otherwise</returns>
        public static bool Pull()
        {
            MergeResult result = null;
            Logger.LogMethodStart();

            var options = new PullOptions
            {
                FetchOptions = new FetchOptions
                {
                    CredentialsProvider = UserIdentity.CredentialsProvider
                }
            };

            using (var repo = new Repository(RepositoryPath))
            {
                var signature = UserIdentity.GetSignature();
                result = Commands.Pull(repo, signature, options);

                if (result.Status == MergeStatus.Conflicts)
                    Logger.Log(LogLevel.Warn, $"Pull completed. Merge unsuccessful due to conflicts.");
                else if (result.Status == MergeStatus.UpToDate)
                    Logger.Log(LogLevel.Info, $"Pull completed, your repository was up to date.");
                else
                    Logger.Log(LogLevel.Info, $"Pull completed. Merge commit: {result.Commit.Id}.");
            }

            Logger.LogMethodEnd();
            return result.Status != MergeStatus.Conflicts;
        }
    }
}
