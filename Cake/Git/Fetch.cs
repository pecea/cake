using System.Linq;
using Common;
using LibGit2Sharp;
using LogLevel = Common.LogLevel;

namespace Git
{
    /// <summary>
    /// Encloses methods used with fetching files in git.
    /// </summary>
    public static partial class Methods
    {
        /// <summary>
        /// Performs a fetch of the remote repositories into your local repository.
        /// </summary>
        /// <returns>True in case of success, false otherwise</returns>
        public static bool Fetch()
        {
            Logger.LogMethodStart();

            string logMessage = "";
            var options = new FetchOptions
            {
                CredentialsProvider = UserIdentity.CredentialsProvider
            };

            using (var repo = new Repository(RepositoryPath))
            {
                foreach (var remote in repo.Network.Remotes)
                {
                    var refSpecs = remote.FetchRefSpecs.Select(rs => rs.Specification);
                    Commands.Fetch(repo, remote.Name, refSpecs, options, logMessage);
                }
            }

            Logger.Log(LogLevel.Info, $"Fetch completed. {logMessage}");
            Logger.LogMethodEnd();

            return true;
        }
    }
}
