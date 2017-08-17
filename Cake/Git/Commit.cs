using Common;
using LibGit2Sharp;
using System;
using LogLevel = Common.LogLevel;

namespace Git
{
    public static partial class Methods
    {
        /// <summary>
        /// Performs a commit with all changes.
        /// </summary>
        /// <param name="message">The commit message</param>
        /// <returns>True in case of success, otherwise false</returns>
        public static bool CommitAllChanges(string message = null) => Commit(message, false);
        /// <summary>
        /// Performs a commit with staged changes.
        /// </summary>
        /// <param name="message">The commit message</param>
        /// <returns>True in case of success, otherwise false</returns>
        public static bool CommitStagedChanges(string message = null) => Commit(message, true);

        /// <summary>
        /// Performs a commit.
        /// </summary>
        /// <param name="message">The commit message.</param>
        /// <param name="onlyStaged">Whether to commit staged or all changes.</param>
        private static bool Commit(string message, bool onlyStaged)
        {
            Logger.Log(LogLevel.Trace, "Method started.");

            if (string.IsNullOrWhiteSpace(message))
            {
                Logger.Log(LogLevel.Warn, "Please enter commit message:");
                message = Console.ReadLine();
            }

            using (var repo = new Repository(RepositoryPath))
            {
                if (!onlyStaged)
                    Stage(repo);

                var author = UserIdentity.GetSignature();
                var committer = author;

                var commit = repo.Commit(message, author, committer);
                Logger.Log(LogLevel.Info, $"Commit {commit.Id} created.");
            }
            Logger.Log(LogLevel.Trace, "Method finished.");

            return true;
        }
    }
}
