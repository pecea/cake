using Common;
using LibGit2Sharp;
using LogLevel = Common.LogLevel;

namespace Git
{
    public static partial class Methods
    {
        public static bool Push()
        {
            Branch branch;
            var options = new PushOptions
            {
                CredentialsProvider = UserIdentity.CredentialsProvider
            };

            using (var repo = new Repository(RepositoryPath))
            {
                branch = repo.Head;
                repo.Network.Push(branch, options);
            }

            Logger.Log(LogLevel.Info, $"Successfully pushed to {branch.RemoteName}.");
            return true;
        }
    }
}
