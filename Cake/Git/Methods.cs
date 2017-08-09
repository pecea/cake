namespace Git
{
    using System;

    public static partial class Methods
    {
        private static string _repositoryPath;

        public static string RepositoryPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_repositoryPath))
                {
                    Console.WriteLine("Please provide the path to your repository:");
                    _repositoryPath = Console.ReadLine();
                }

                return _repositoryPath;
            }
            set { _repositoryPath = value; }
        }

        public static Identity UserIdentity { get; set; } = new Identity();
    }
}
