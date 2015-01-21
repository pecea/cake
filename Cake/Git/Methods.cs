using System.Linq;

namespace Git
{
    using System.IO;

    using Common;

    /// <summary>
    /// Encloses methods used with running git commands.
    /// </summary>
    public static class Methods
    {
        private const string app = "git.exe";

        private static readonly string[] Paths = {
            Path.Combine(@"C:\Program Files (x86)\Git\bin", app),
            Path.Combine(@"C:\Program Files\Git\bin", app)
        };

        public static string PathToExe { get; set; }
        public static string PathToRepository { get; set; }

        public static string FullPathExe
        {
            get
            {
                if (File.Exists(PathToExe)) return PathToExe;
                foreach (var path in Paths.Where(File.Exists))
                {
                    return path;
                }
                return app;
            }
        }

        //public static string CurrentSha()
        //{
        //    var result = String.Empty;
        //    try
        //    {
        //        result = Processor.RunProcess(FullPathExe, "rev-parse HEAD");
        //        return String.IsNullOrEmpty(result) ? String.Empty : result;

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}

        //public static string CurrentBranch()
        //{
        //    var result =Processor.RunProcess(FullPathExe, "rev-parse --abbrev-ref HEAD");
        //    return String.IsNullOrEmpty(result) ? String.Empty : result;
        //}

        /// <summary>
        /// Executes git tag command
        /// </summary>
        /// <param name="tag">string containing tag</param>
        /// <returns>true in case of success, false otherwise.</returns>
        public static bool Tag(string tag)
        {
            var res = Processor.RunProcess(FullPathExe, "tag " + tag, PathToRepository);
            if (res)
                Logger.Log(LogLevel.Info, "Git tag" + tag + " added correctly");
            else
                Logger.Log(LogLevel.Error, "Git tag not added!");
            return res;
        }

        /// <summary>
        /// Executes git push command
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="branches"></param>
        /// <returns>true in case of success, false otherwise.</returns>
        public static bool Push(string repository, params string[] branches)
        {
            var refToPush = branches == null ? string.Empty : string.Join(" ", branches);
            var res = Processor.RunProcess(FullPathExe, "push " + repository + " " + refToPush, PathToRepository);
            if (res)
                Logger.Log(LogLevel.Info, "Git push command executed correctly");
            else
                Logger.Log(LogLevel.Error, "Git push command executed with error!");
            return res;


        }

        /// <summary>
        /// Executes git reset command
        /// </summary>
        /// <returns>true in case of success, false otherwise.</returns>
        public static bool ResetAllModifications()
        {
            var res = Processor.RunProcess(FullPathExe, "reset --hard", PathToRepository);
            if (res)
                Logger.Log(LogLevel.Info, "Git reset command executed correctly");
            else
                Logger.Log(LogLevel.Error, "Git reset command executed with error!");
            return res;
        }
        /// <summary>
        /// Executes git clean command
        /// </summary>
        /// <param name="allFiles"></param>
        /// <returns>true in case of success, false otherwise.</returns>
        public static bool Clean(bool allFiles = false)
        {
            var res = Processor.RunProcess(FullPathExe, "clean -f" + (allFiles ? " -dx" : string.Empty), PathToRepository);
            if (res)
                Logger.Log(LogLevel.Info, "Git clean command executed correctly");
            else
                Logger.Log(LogLevel.Error, "Git clean command executed with error!");
            return res;
        }

        /// <summary>
        /// Executes git user-specified command
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>true in case of success, false otherwise.</returns>
        public static bool Run(string parameters)
        {
            var res =  Processor.RunProcess(FullPathExe, parameters, PathToRepository);
            if (res)
                Logger.Log(LogLevel.Info, "Git run command executed correctly");
            else
                Logger.Log(LogLevel.Error, "Git run command executed with error!");
            return res;
        }
    }
}
