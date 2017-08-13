using Common;
using LibGit2Sharp;
using LogLevel = Common.LogLevel;

namespace Git
{
    public static partial class Methods
    {
        /// <summary>
        /// Performs a pull of remote changes to your local repository with automatic merge.
        /// </summary>
        /// <returns>True in case of success, false otherwise</returns>
        public static bool Pull()
        {
            Logger.Log(LogLevel.Trace, "Method started");

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
                var result = Commands.Pull(repo, signature, options);

                if (result.Status == MergeStatus.Conflicts)
                    Logger.Log(LogLevel.Warn, $"Pull completed. Merge unsuccessful due to conflicts.");
                else
                    Logger.Log(LogLevel.Info, $"Pull completed. Merge commit: {result.Commit.Id}.");
            }

            Logger.Log(LogLevel.Trace, "Method finished");
            return true;
        }
    }
}
