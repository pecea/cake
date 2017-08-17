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
            Logger.LogMethodStart();

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

            Logger.LogMethodEnd();
            return true;
        }
    }
}
