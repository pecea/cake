using System;
using System.Linq;
using Common;
using System.Threading.Tasks;

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
        /// <param name="args">Paths to *.csx scripts to be executed.</param>
        private static async Task Main(string[] args)
        {
            Logger.Log(LogLevel.Trace, "Cake program starting ...");
            //var msTestPath = 
            // Parsing arguments
            Argument[] arguments;
            try
            {
                arguments = ArgumentParser.Parse(args);
            }
            catch (Exception e)
            {
                Logger.LogException(LogLevel.Fatal, e, "A fatal error has occured.");
                return;
            }


            // Retrieving whether to show help or not
            var helpArgument = arguments.SingleOrDefault(arg => arg.Names.Contains("/help"));
            if (helpArgument != null)
            {
                Console.Write(
                    "\n\nTo use the program run cake.exe with arguments:\n\n1./script (/s) - path to your c# script - necessary\n2./verbosity (/v) - level of output\n    Possible values: debug, info, warn, error, fatal\n3./runjob (/r) - name of the job to run from your c# script\n   Necessary if your c# script does not have SetDefault(jobName) method\n4./help (/h) - show help\n\n");
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
                await RoslynEngine.Instance.ExecuteFile(scriptArgument.Value).ConfigureAwait(false);
            }
            catch (JobException j)
            {
                Logger.LogException(LogLevel.Error, j, "An exception occured while performing a job.\n");

                Console.ReadKey();
                return;
            }
            catch (Exception e)
            {
                Logger.LogException(LogLevel.Fatal, e, "A fatal error has occured.\n");
                Console.ReadKey();
                return;
            }

            Logger.Log(LogLevel.Info, "Cake finished successfully.");
            Console.ReadKey();
        }
    }
}