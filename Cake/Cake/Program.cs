using System;
using System.Linq;
using Common;
using System.Threading.Tasks;
using System.Text;
using System.IO;

namespace Cake
{
    /// <summary>
    /// Provides an entry point for the application.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Entry point of the application.
        /// </summary>
        /// <param name="args">Path to *.csx script to be executed, logging verbosity and optionally job to be run</param>
        private static async Task Main(string[] args)
        {
            // Parsing arguments
            Argument[] arguments;
            try
            {
                arguments = new ArgumentParser().Parse(args);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"{e.ParamName} is not a valid argument.");
                ShowHelp();
                Console.ReadKey();
                return;
            }

            // Retrieving whether to show help or not
            var helpArgument = arguments.SingleOrDefault(arg => arg.Names.Contains("/help"));
            if (helpArgument != null)
            {
                ShowHelp();
                Console.ReadKey();
                return;
            }

            // Retrieving user specified logging levels
            var scriptLogLevelArgument = arguments.SingleOrDefault(arg => arg.Names.Contains("/scriptverbosity"));
            if (scriptLogLevelArgument != null)
                Logger.Reconfigure(scriptLogLevelArgument.Value, "script");

            var appLogLevelArgument = arguments.SingleOrDefault(arg => arg.Names.Contains("/appverbosity"));
            if (appLogLevelArgument != null)
                Logger.Reconfigure(appLogLevelArgument.Value, "coloredConsole");

            // Retrieving script path
            var scriptArgument = arguments.SingleOrDefault(arg => arg.Names.Contains("/script"));
            if (scriptArgument == null)
            {
                Logger.Log(LogLevel.Fatal, "You must specify a script to be run with the /script or /s argument.");
                return;
            }

            // Retrieving user specified job to run
            var jobToRunArgument = arguments.SingleOrDefault(arg => arg.Names.Contains("/runjob"));
            if (jobToRunArgument != null) JobManager.JobToRun = jobToRunArgument.Value;

            Logger.Log(LogLevel.Info, $"Cake started for script: {scriptArgument.Value}.");

            // Running the script
            try
            {
                string scriptPath = Path.GetFullPath(scriptArgument.Value);
                // Set Processor's working directory to locate .exe files
                Processor.SetWorkingDirectory(Environment.CurrentDirectory);
                // Set current directory to the scripts directory
                Environment.CurrentDirectory = Path.GetDirectoryName(scriptPath);

                await RoslynEngine.Instance.ExecuteFile(scriptPath).ConfigureAwait(false);
            }
            catch (JobException j)
            {
                HandleJobException(j);
                Console.ReadKey();
                return;
            }
            catch (Exception e)
            {
                Logger.LogException(LogLevel.Fatal, e, "A fatal error has occured.");
                Console.ReadKey();
                return;
            }

            Logger.Log(LogLevel.Info, "Cake finished successfully.");
            Console.ReadKey();
        }

        private static void HandleJobException(JobException j)
        {
            var message = "Script execution finished with errors.";

            if (!string.IsNullOrWhiteSpace(j.SourceJobName))
            {
                message += $" Exception source job: {j.SourceJobName}.";
            }

            Logger.Error(message);
        }

        private static void ShowHelp()
        {
            var sb = new StringBuilder();

            sb.AppendLine("To use the program run cake.exe with arguments:");
            sb.AppendLine();
            sb.AppendLine("1. /script (/s)      Path to your c# script - necessary.");
            sb.AppendLine("2. /verbosity (/v)   Level of output.");
            sb.AppendLine("                     Possible values: debug, info, warn, error, fatal.");
            sb.AppendLine("3. /runjob (/r)      Name of the job to run from your c# script.");
            sb.AppendLine("                     Necessary if your c# script does not have SetDefault(jobName) method.");
            sb.AppendLine("4. /help (/h)        Show help.");

            Console.Write(sb.ToString());
        }
    }
};