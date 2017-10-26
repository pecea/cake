using System;

namespace Git
{
    public static partial class Methods
    {
        private static string _repositoryPath;
        /// <summary>
        /// Path to the git repository
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
        /// User <see cref="Identity"/>
        /// </summary>
        public static Identity UserIdentity { get; set; } = new Identity();
    }
}
