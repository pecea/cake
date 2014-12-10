using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Common;
using GitHubWrapper = GitHubWrapper.GitHubWrapper;

namespace Git
{
    /// <summary>
    /// Encloses methods used with running git commands.
    /// </summary>
    public static class Methods
    {
        private static string[] paths = new[]
        {
            Path.Combine(@"C:\Program Files (x86)\Git\bin", "git.exe"),
            Path.Combine(@"C:\Program Files\Git\bin", "git.exe"),
            Path.Combine(PathToExe ?? String.Empty, "git.exe")
        };

        public static string PathToExe { get; set; }

        public static string FullPathExe
        {
            get
            {
                foreach (var path in paths)
                {
                    if (File.Exists(path))
                        return path;
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

        /// <summary>
        /// Executes git push command
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="branches"></param>
        /// <returns>true in case of success, false otherwise.</returns>
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

        /// <summary>
        /// Executes git reset command
        /// </summary>
        /// <returns>true in case of success, false otherwise.</returns>
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
        /// <summary>
        /// Executes git clean command
        /// </summary>
        /// <param name="allFiles"></param>
        /// <returns>true in case of success, false otherwise.</returns>
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
        /// <summary>
        /// Executes git user-specified command
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>true in case of success, false otherwise.</returns>
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
