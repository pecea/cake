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