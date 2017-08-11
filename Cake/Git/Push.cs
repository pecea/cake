using Common;
using LibGit2Sharp;
using LogLevel = Common.LogLevel;

namespace Git
{
    public static partial class Methods
    {
        public static bool Push()
        {
            Logger.Log(LogLevel.Trace, "Method started");

            var options = new PushOptions
            {
                CredentialsProvider = UserIdentity.CredentialsProvider
            };

            using (var repo = new Repository(RepositoryPath))
            {
                Branch branch = repo.Head;
                repo.Network.Push(branch, options);
                Logger.Log(LogLevel.Info, $"Successfully pushed to {branch.RemoteName}.");
            }

            Logger.Log(LogLevel.Trace, "Method finished");
            return true;
        }
    }
}
