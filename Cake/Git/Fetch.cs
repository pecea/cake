using Common;
using LibGit2Sharp;
using System.Collections.Generic;
using System.Linq;
using LogLevel = Common.LogLevel;

namespace Git
{
    public static partial class Methods
    {
        public static bool Fetch()
        {
            Logger.Log(LogLevel.Trace, "Method started");

            string logMessage = "";
            var options = new FetchOptions
            {
                CredentialsProvider = UserIdentity.CredentialsProvider
            };

            using (var repo = new Repository(RepositoryPath))
            {
                foreach (Remote remote in repo.Network.Remotes)
                {
                    IEnumerable<string> refSpecs = remote.FetchRefSpecs.Select(rs => rs.Specification);
                    Commands.Fetch(repo, remote.Name, refSpecs, options, logMessage);
                }
            }

            Logger.Log(LogLevel.Info, $"Fetch completed. {logMessage}");

            return true;
        }
    }
}
