namespace Git
{
    using System;
    using System.IO;

    using Common;

    /// <summary>
    /// Encloses methods used with running git commands.
    /// </summary>
    public static class Methods
    {
        private static readonly string[] Paths = {
            Path.Combine(@"C:\Program Files (x86)\Git\bin", "git.exe"),
            Path.Combine(@"C:\Program Files\Git\bin", "git.exe")
        };

        public static string PathToExe { get; set; }

        public static string FullPathExe
        {
            get
            {
                if (File.Exists(PathToExe)) return PathToExe;
                foreach (var path in Paths)
                {
                    if (File.Exists(path)) return path;
                }
                return "git.exe";
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
            return Processor.RunProcess(FullPathExe, "tag ");
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
            return Processor.RunProcess(FullPathExe, "push " + repository + " " + refToPush);


        }

        /// <summary>
        /// Executes git reset command
        /// </summary>
        /// <returns>true in case of success, false otherwise.</returns>
        public static bool ResetAllModifications()
        {
            return Processor.RunProcess(FullPathExe, "reset --hard");
        }
        /// <summary>
        /// Executes git clean command
        /// </summary>
        /// <param name="allFiles"></param>
        /// <returns>true in case of success, false otherwise.</returns>
        public static bool Clean(bool allFiles = false)
        {
            return Processor.RunProcess(FullPathExe, "clean -f" + (allFiles ? " -dx" : string.Empty));
        }

        /// <summary>
        /// Executes git user-specified command
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>true in case of success, false otherwise.</returns>
        public static bool Run(string parameters)
        {
            return Processor.RunProcess(FullPathExe, parameters);
        }
    }
}
