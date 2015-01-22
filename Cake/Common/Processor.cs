using System;
using System.IO;

namespace Common
{
    using System.Diagnostics;
    using System.Text;

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
        public static bool RunProcess(string command, string arguments = "", string workingDirectory = ".")
        {
            var outputBuilder = new StringBuilder();
            Logger.Log(LogLevel.Debug, "Running command:" + command + " " + arguments);

            if (workingDirectory != ".")
            {
                if (!Directory.Exists(workingDirectory))
                {
                    workingDirectory = ".";
                    Logger.Log(LogLevel.Warn, "Working directory path does not exist! Changed path to "+Directory.GetCurrentDirectory());
                }
            }

            using (var process = new Process
            {
                StartInfo =
                {
                    FileName = command,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WorkingDirectory = Path.GetFullPath(workingDirectory)
                }
            })
            {

                process.OutputDataReceived += (sender, e) => outputBuilder.AppendLine(e.Data);

                process.Start();
                process.BeginOutputReadLine();
                process.CancelOutputRead();
                process.WaitForExit();
                //process.CancelOutputRead();

                var output = outputBuilder.ToString().TrimEnd('\n', '\r');
                if (process.ExitCode == 0)
                {
                    Logger.Log(LogLevel.Debug, "Process run successfully!");
                    Logger.Log(LogLevel.Info, output);
                    return true;
                }
                Logger.Log(LogLevel.Debug, "Process exited with an error!");
                Logger.Log(LogLevel.Warn, output);
                return false;
            }
        }

        /// <summary>
        /// Encloses a string with quotes if it contains any whitespace characters.
        /// </summary>
        /// <param name="arg">String to be modified.</param>
        /// <returns>String enclosed with quotes</returns>
        public static string QuoteArgument(string arg)
        {
            return arg.Contains(" ") ? String.Format(@"""{0}""", arg) : arg;
        }
    }
}
