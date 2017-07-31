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
        public static (bool, string, string) RunProcess(string command, string arguments = "", string workingDirectory = ".")
        {
            Logger.Log(LogLevel.Debug, "Running command:" + command + " " + arguments);

            if (workingDirectory != ".")
            {
                if (!Directory.Exists(workingDirectory))
                {
                    workingDirectory = ".";
                    Logger.Log(LogLevel.Warn, "Working directory path does not exist! Changed path to " + Directory.GetCurrentDirectory());
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

                var outEnd = process.StandardOutput.ReadToEnd();
                var errEnd = process.StandardError.ReadToEnd();
                if(!string.IsNullOrEmpty(outEnd))
                    Logger.Log(LogLevel.Info, $"Process result: {outEnd}");
                if(!string.IsNullOrEmpty(errEnd))
                    Logger.Log(LogLevel.Error, $"Process error: {errEnd}");
               // process.BeginOutputReadLine();
               // process.BeginErrorReadLine();

                process.WaitForExit();

                //process.CancelOutputRead();
                //process.CancelErrorRead();
                
                if (process.ExitCode == 0)
                {
                    Logger.Log(LogLevel.Debug, "Process run successfully!");
                    return (true, outEnd, errEnd);
                }
                Logger.Log(LogLevel.Debug, "Process exited with an error!");
                return (false, outEnd, errEnd);
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
