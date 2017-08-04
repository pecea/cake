namespace Git
{
    using Common;
    using LibGit2Sharp;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LogLevel = Common.LogLevel;

    public static class Methods
    {
        public static string RepositoryPath { get; set; }
        public static string UserName { get; set; }
        public static string UserEmail { get; set; }

        public static bool CommitAll(string message) => Commit(message, DiffTargets.WorkingDirectory);

        public static bool CommitAdded(string message) => Commit(message, DiffTargets.Index);

        public static bool CommitStaged(string message) => Commit(message, mode: null);

        /// <summary>
        /// Performs a commit.
        /// </summary>
        /// <param name="message">The commit message.</param>
        /// <param name="mode">
        /// Commit mode.
        /// <para>Set to <see cref="DiffTargets.Index"/> to commit all added (to index) and modified files.</para>
        /// <para>Set to <see cref="DiffTargets.WorkingDirectory"/> to commit all new and modified files.</para>
        /// <para>Set to null to commit only staged files.</para>
        /// </param>
        /// <returns></returns>
        public static bool Commit(string message, DiffTargets? mode)
        {
            if (string.IsNullOrEmpty(RepositoryPath))
                throw new InvalidOperationException("Set RepositoryPath before interacting with the repository.");

            using (var repo = new Repository(RepositoryPath))
            {
                if (mode.HasValue)
                    foreach (TreeEntryChanges c in repo.Diff.Compare<TreeChanges>(repo.Head.Tip.Tree, mode.Value))
                    {
                        Logger.Log(LogLevel.Info, $"Staging {c.Status} {c.Path}");
                        Commands.Stage(repo, c.Path);
                    }

                var author = new Signature(UserName, UserEmail, DateTime.Now);
                var committer = author;

                var commit = repo.Commit(message, author, committer);
                Logger.Log(LogLevel.Info, $"Commit {commit.Id} created.");
            }

            return true;
        }

        public static bool Fetch(string username = "", string password = "")
        {
            string logMessage = "";
            var options = new FetchOptions
            {
                CredentialsProvider = new LibGit2Sharp.Handlers.CredentialsHandler((url, usernameFromUrl, types) =>
                    new UsernamePasswordCredentials
                    {
                        Username = username,
                        Password = password
                    })
            };

            using (var repo = new Repository(RepositoryPath))
            {
                foreach (Remote remote in repo.Network.Remotes)
                {
                    IEnumerable<string> refSpecs = remote.FetchRefSpecs.Select(rs => rs.Specification);
                    Commands.Fetch(repo, remote.Name, refSpecs, options, logMessage);
                }
            }

            Console.WriteLine(logMessage);

            return true;
        }

        public static bool Reset()
        {
            using (var repo = new Repository(RepositoryPath))
            {
                repo.Reset(ResetMode.Hard);
            }

            return true;
        }

        public static bool Diff()
        {
            // https://stackoverflow.com/questions/3689838/difference-between-head-working-tree-index-in-git
            using (var repo = new Repository(RepositoryPath))
            {
                Console.WriteLine("Index vs HEAD:");
                foreach (TreeEntryChanges c in repo.Diff.Compare<TreeChanges>(repo.Head.Tip.Tree, DiffTargets.Index))
                {
                    Console.WriteLine($"{c.Path} was {c.Status}");
                }

                Console.WriteLine("Workspace vs HEAD:");
                foreach (TreeEntryChanges c in repo.Diff.Compare<TreeChanges>(repo.Head.Tip.Tree, DiffTargets.WorkingDirectory))
                {
                    Console.WriteLine($"{c.Path} was {c.Status}");
                }
            }

            return true;
        }

        public static bool Tag(string v)
        {
            throw new NotImplementedException();
        }

        public static bool Clean()
        {
            throw new NotImplementedException();
        }

        public static bool Run(string v)
        {
            throw new NotImplementedException();
        }

        public static bool CurrentBranch()
        {
            throw new NotImplementedException();
        }

        public static bool CurrentSha()
        {
            throw new NotImplementedException();
        }

        public static void Clean(bool v)
        {
            throw new NotImplementedException();
        }
    }

    ///// <summary>
    ///// Encloses methods used with running git commands.
    ///// </summary>
    //public static class Methods
    //{
    //    private const string App = "git.exe";

    //    private static readonly string[] Paths = {
    //        "../../../External/git.exe"
    //    };

    //    private static bool _initialized;

    //    public static string PathToExe { get; set; }
    //    public static string PathToRepository { get; set; }

    //    public static string FullPathExe
    //    {
    //        get
    //        {
    //            if (File.Exists(PathToExe)) return PathToExe;
    //            foreach (var path in Paths.Where(File.Exists))
    //            {
    //                return path;
    //            }
    //            return App;
    //        }
    //    }

    //    private static void Initialize()
    //    {
    //        var path = Path.Combine(Directory.GetParent(Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName).FullName, ".gitconfig");
    //        _initialized = true;

    //        using (var reader = new StreamReader(path))
    //        {
    //            var fileData = reader.ReadToEnd();
    //            if (!fileData.Contains("notepad"))
    //            {
    //                Logger.Log(LogLevel.Debug, "Setting notepad.exe as git's standard input editor.");
    //                Run("config --global core.editor \"%windir%/system32/notepad.exe\"");
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Gets the current sha
    //    /// </summary>
    //    /// <returns></returns>
    //    public static bool CurrentSha()
    //    {
    //        if (!_initialized) Initialize();

    //        return Processor.RunProcess(FullPathExe, "rev-parse HEAD", PathToRepository);
    //    }

    //    /// <summary>
    //    /// Gets the current branch
    //    /// </summary>
    //    /// <returns></returns>
    //    public static bool CurrentBranch()
    //    {
    //        if (!_initialized) Initialize();

    //        return Processor.RunProcess(FullPathExe, "rev-parse --abbrev-ref HEAD", PathToRepository);
    //    }

    //    /// <summary>
    //    /// Executes git tag command
    //    /// </summary>
    //    /// <param name="tag">string containing tag</param>
    //    /// <returns>true in case of success, false otherwise.</returns>
    //    public static bool Tag(string tag)
    //    {
    //        if (!_initialized) Initialize();

    //        var res = Processor.RunProcess(FullPathExe, "tag " + tag, PathToRepository);
    //        if (res)
    //            Logger.Log(LogLevel.Info, "Git tag" + tag + " added correctly");
    //        else
    //            Logger.Log(LogLevel.Error, "Git tag not added!");
    //        return res;
    //    }

    //    /// <summary>
    //    /// Executes git push command
    //    /// </summary>
    //    /// <param name="repository"></param>
    //    /// <param name="branches"></param>
    //    /// <returns>true in case of success, false otherwise.</returns>
    //    public static bool Push(string repository, params string[] branches)
    //    {
    //        if (!_initialized) Initialize();

    //        var refToPush = branches == null ? string.Empty : string.Join(" ", branches);
    //        var res = Processor.RunProcess(FullPathExe, "push " + repository + " " + refToPush, PathToRepository);
    //        if (res)
    //            Logger.Log(LogLevel.Info, "Git push command executed correctly");
    //        else
    //            Logger.Log(LogLevel.Error, "Git push command executed with error!");
    //        return res;
    //    }

    //    /// <summary>
    //    /// Executes git reset command
    //    /// </summary>
    //    /// <returns>true in case of success, false otherwise.</returns>
    //    public static bool ResetAllModifications()
    //    {
    //        if (!_initialized) Initialize();

    //        var res = Processor.RunProcess(FullPathExe, "reset --hard", PathToRepository);
    //        if (res)
    //            Logger.Log(LogLevel.Info, "Git reset command executed correctly");
    //        else
    //            Logger.Log(LogLevel.Error, "Git reset command executed with error!");
    //        return res;
    //    }
    //    /// <summary>
    //    /// Executes git clean command
    //    /// </summary>
    //    /// <param name="allFiles"></param>
    //    /// <returns>true in case of success, false otherwise.</returns>
    //    public static bool Clean(bool allFiles = false)
    //    {
    //        if (!_initialized) Initialize();

    //        var res = Processor.RunProcess(FullPathExe, "clean -f" + (allFiles ? " -dx" : string.Empty), PathToRepository);
    //        if (res)
    //            Logger.Log(LogLevel.Info, "Git clean command executed correctly");
    //        else
    //            Logger.Log(LogLevel.Error, "Git clean command executed with error!");
    //        return res;
    //    }

    //    /// <summary>
    //    /// Executes git user-specified command
    //    /// </summary>
    //    /// <param name="parameters"></param>
    //    /// <returns>true in case of success, false otherwise.</returns>
    //    public static bool Run(string parameters)
    //    {
    //        if (!_initialized) Initialize();

    //        var res =  Processor.RunProcess(FullPathExe, parameters, PathToRepository);
    //        if (res)
    //            Logger.Log(LogLevel.Info, "Git run command executed correctly");
    //        else
    //            Logger.Log(LogLevel.Error, "Git run command executed with error!");
    //        return res;
    //    }
    //}
}
