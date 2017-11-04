using System;
using System.Diagnostics;
using System.IO;

namespace Common
{
    /// <summary>
    /// Encloses methods used with running processes.
    /// </summary>
    public static class Processor
    {
        private static Uri ProcessorWorkingDirectory;

        /// <summary>
        /// Sets <see cref="Processor"/>'s working directory.
        /// </summary>
        /// <param name="path">Path to working directory</param>
        public static void SetWorkingDirectory(string path)
        {
            string fullPath = Path.GetFullPath(path);

            if (!Directory.Exists(fullPath))
            {
                throw new DirectoryNotFoundException($"Could not find directory '{fullPath}'.");
            }

            Logger.Debug($"Setting Processor's working directory to '{fullPath}'.");
            ProcessorWorkingDirectory = new Uri(fullPath, UriKind.Absolute);
        }

        /// <summary>
        /// Runs command wth arguments
        /// </summary>
        /// <param name="command">Command to run</param>
        /// <param name="arguments">Arguments to go with command</param>
        /// <param name="workingDirectory">Directory on which command should run</param>
        /// <returns><see cref="ProcessResult"/></returns> 
        public static ProcessResult RunProcess(string command, string arguments = "", string workingDirectory = null)
        {
            Logger.LogMethodStart();
            var result = new ProcessResult();

            if (string.IsNullOrWhiteSpace(workingDirectory))
            {
                workingDirectory = Uri.UnescapeDataString(ProcessorWorkingDirectory.AbsolutePath);
            }
            else if (!Directory.Exists(workingDirectory))
            {
                Logger.Warn($"Working directory path does not exist! ({Path.GetFullPath(workingDirectory)}) Using Processor's working directory ({ProcessorWorkingDirectory}).");
                workingDirectory = Uri.UnescapeDataString(ProcessorWorkingDirectory.AbsolutePath);
            }

            Logger.Log(LogLevel.Debug, $"Running command: {command} {arguments} in '{workingDirectory}'.");

            using (var process = new Process
            {
                StartInfo =
                {
                    FileName = command,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WorkingDirectory = workingDirectory,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            })
            {
                process.OutputDataReceived += (sender, e) => { if (!string.IsNullOrEmpty(e.Data)) Logger.Log(LogLevel.Info, e.Data); };
                process.ErrorDataReceived += (sender, e) => { if (!string.IsNullOrEmpty(e.Data)) Logger.Log(LogLevel.Error, e.Data); };

                process.Start();

                result.Output = process.StandardOutput.ReadToEnd();
                result.Error = process.StandardError.ReadToEnd();

                process.WaitForExit();
                result.ExitCode = process.ExitCode;

                LogProcessOutput(result);
                Logger.LogMethodEnd();
                return result;
            }
        }

        /// <summary>
        /// Encloses a string with quotes.
        /// </summary>
        /// <param name="arg">String to be modified.</param>
        /// <returns>String enclosed with quotes</returns>
        public static string QuoteArgument(string arg)
        {
            return $@"""{arg}""";
        }

        private static void LogProcessOutput(ProcessResult result)
        {
            LogLevel outputLevel = result.Success ? LogLevel.Info : LogLevel.Warn;

            if (!string.IsNullOrWhiteSpace(result.Output))
                Logger.Log(outputLevel, result.Output);

            if (!string.IsNullOrWhiteSpace(result.Error))
                Logger.Warn(result.Error);

            if (result.Success)
                Logger.Log(LogLevel.Info, "Process finished successfully.");
            else
                Logger.Log(LogLevel.Warn, $"Process finished with exit code {result.ExitCode}.");
        }
    }
}
