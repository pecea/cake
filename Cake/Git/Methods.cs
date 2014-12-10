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

        public static bool Tag(string tag)
        {
            try
            {
                Logger.Log(LogLevel.Info, Processor.RunProcess(FullPathExe, "tag "));
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public static bool Push(string repository, params string[] branches)
        {
            var refToPush = branches == null ? string.Empty : string.Join(" ", branches);
            try
            {
                Logger.Log(LogLevel.Info, Processor.RunProcess(FullPathExe, "push " + repository + " " + refToPush));
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public static bool ResetAllModifications()
        {
            try
            {
                Logger.Log(LogLevel.Info, Processor.RunProcess(FullPathExe, "reset --hard"));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool Clean(bool allFiles = false)
        {
            try
            {
                Logger.Log(LogLevel.Info, Processor.RunProcess(FullPathExe, "clean -f" + (allFiles ? " -dx" : string.Empty)));
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public static bool Run(string parameters)
        {
            try
            {
                Logger.Log(LogLevel.Info, Processor.RunProcess(FullPathExe, parameters));
                return true;
            }
            catch (Exception)
            {
                return false;
            } 
        }
    }
}
