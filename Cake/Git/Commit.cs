using Common;
using LibGit2Sharp;
using LogLevel = Common.LogLevel;

namespace Git
{
    public static partial class Methods
    {
        public static bool CommitAllChanges(string message) => Commit(message, onlyStaged: false);

        public static bool CommitStagedChanges(string message) => Commit(message, onlyStaged: true);

        /// <summary>
        /// Performs a commit.
        /// </summary>
        /// <param name="message">The commit message.</param>
        /// <param name="onlyStaged">Whether to commit staged or all changes.</param>
        private static bool Commit(string message, bool onlyStaged)
        {
            Logger.Log(LogLevel.Trace, "Method started");

            using (var repo = new Repository(RepositoryPath))
            {
                if (!onlyStaged)
                    Stage(repo);

                var author = UserIdentity.GetSignature();
                var committer = author;

                var commit = repo.Commit(message, author, committer);
                Logger.Log(LogLevel.Info, $"Commit {commit.Id} created.");
            }

            return true;
        }
    }
}
