namespace Cake
{
    using System;
    using System.Linq;

    using Common;

    /// <summary>
    /// Provides an entry point for the application.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// Entry point of the application.
        /// </summary>
        /// <param name="args">Paths to *.csx scripts to be executed.</param>
        static void Main(string[] args)
        {
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
            if(helpArgument != null)
                Console.Write("\n\nTo use the program run cake.exe with arguments:\n\n1./script (/s) - path to your c# script - necessary\n2./verbosity (/v) - level of output\n    Possible values: debug, info, warn, error, fatal\n3./runtask (/r) - name of the task to run from your c# script\n   Necessary if your c# script does not have SetDefault(taskName) method\n4./help (/h) - show help\n\n");

            // Retrieving user specified logging level
            var logLevelArgument = arguments.SingleOrDefault(arg => arg.Names.Contains("/verbosity"));
            if (logLevelArgument != null) Logger.Reconfigure(logLevelArgument.Value);
            // Retrieving script path
            var scriptArgument = arguments.SingleOrDefault(arg => arg.Names.Contains("/script"));
            if (scriptArgument == null)
            {
                Logger.Log(LogLevel.Fatal, "You must specify a script to be run with the /script or /s argument.");
                return;
            }

            // Retrieving user specified task to run
            var taskToRunArgument = arguments.SingleOrDefault(arg => arg.Names.Contains("/runtask"));
            if (taskToRunArgument != null) TaskManager.TaskToRun = taskToRunArgument.Value;


            Logger.Log(LogLevel.Info, String.Format("Cake started for script: {0}", scriptArgument.Value));

            // Running the script
            try
            {
                RoslynEngine.ExecuteFile(scriptArgument.Value);
            }
            catch (Exception e)
            {
                Logger.LogException(LogLevel.Fatal, e, "A fatal error has occured.");
                return;
            }

            Logger.Log(LogLevel.Info, "Cake finished successfully.");
        }
    }
}