using System.Diagnostics;
using System.IO;

namespace Common
{
    /// <summary>
    /// Encloses methods used with running processes.
    /// </summary>
    public static class Processor
    {
        /// <summary>
        /// Runs command wth arguments
        /// </summary>
        /// <param name="command">Command to run</param>
        /// <param name="arguments">Arguments to go with command</param>
        /// <param name="workingDirectory">Directory on which command should run</param>
        /// <returns></returns> 
        public static ProcessResult RunProcess(string command, string arguments = "", string workingDirectory = ".")
        {
            Logger.LogMethodStart();
            var result = new ProcessResult();
            Logger.Log(LogLevel.Debug, $"Running command: {command} {arguments}.");

            if (workingDirectory != ".")
            {
                if (!Directory.Exists(workingDirectory))
                {
                    workingDirectory = ".";
                    Logger.Log(LogLevel.Warn, $"Working directory path does not exist! Changed path to {Directory.GetCurrentDirectory()}.");
                }
            }

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
                process.OutputDataReceived +=
                    (sender, e) => { if (!string.IsNullOrEmpty(e.Data)) Logger.Log(LogLevel.Info, e.Data); };
                process.ErrorDataReceived +=
                    (sender, e) => { if (!string.IsNullOrEmpty(e.Data)) Logger.Log(LogLevel.Error, e.Data); };

                process.Start();

                result.Output = process.StandardOutput.ReadToEnd();
                result.Error = process.StandardError.ReadToEnd();
                if(!string.IsNullOrEmpty(result.Output))
                    Logger.Log(LogLevel.Info, $"Process result: {result.Output}.");
                if(!string.IsNullOrEmpty(result.Error))
                    Logger.Log(LogLevel.Warn, $"Process error: {result.Error}.");

                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    Logger.Log(LogLevel.Info, "Process run successfully!");
                    result.Success = true;
                }
                else
                {
                    Logger.Log(LogLevel.Warn, "Process exited with an error!");
                    result.Success = false;
                }
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
    }
}
