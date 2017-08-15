using Common;
using LibGit2Sharp;
using LogLevel = Common.LogLevel;

namespace Git
{
    public static partial class Methods
    {
        /// <summary>
        /// Performs a push operation of local repository changes to the current branch.
        /// </summary>
        /// <returns>True in case of success, false otherwise</returns>
        public static bool Push()
        {
            Logger.Log(LogLevel.Trace, "Method started.");

            var options = new PushOptions
            {
                CredentialsProvider = UserIdentity.CredentialsProvider
            };

            using (var repo = new Repository(RepositoryPath))
            {
                var branch = repo.Head;
                repo.Network.Push(branch, options);
                Logger.Log(LogLevel.Info, $"Successfully pushed to {branch.RemoteName}.");
            }

            Logger.Log(LogLevel.Trace, "Method finished.");
            return true;
        }
    }
}
