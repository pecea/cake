using System;

namespace Git
{
    /// <summary>
    /// Encloses methods used with repository in git.
    /// </summary>
    public static partial class Methods
    {
        private static string _repositoryPath;

        /// <summary>
        /// Git repository path
        /// </summary>
        public static string RepositoryPath
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_repositoryPath)) return _repositoryPath;
                Console.WriteLine("Please provide the path to your repository:");
                _repositoryPath = Console.ReadLine();

                return _repositoryPath;
            }
            set => _repositoryPath = value;
        }
        /// <summary>
        /// User identity in git
        /// </summary>
        public static Identity UserIdentity { get; set; } = new Identity();
    }
}
