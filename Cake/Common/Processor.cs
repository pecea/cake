using System;
using System.Diagnostics;
using System.Text;

namespace Common
{
    /// <summary>
    /// Encloses methods used with running processes.
    /// </summary>
    public class Processor
    {
        /// <summary>
        /// Runs command wth arguments
        /// </summary>
        /// <param name="command">string containing command</param>
        /// <param name="arguments">arguments for command</param>
        /// <returns></returns>
        public static bool RunProcess(string command, string arguments = "")
        {
            var outputBuilder = new StringBuilder();
            Logger.Log(LogLevel.Debug, "Running command:" + command + " " + arguments);

            var process = new Process
            {
                StartInfo =
                {
                    FileName = command,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WorkingDirectory = "."
                }
            };
            process.OutputDataReceived += (sender, e) =>
            {
                outputBuilder.AppendLine(e.Data);
                Logger.Log(LogLevel.Debug, e.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
            process.CancelOutputRead();

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
}
