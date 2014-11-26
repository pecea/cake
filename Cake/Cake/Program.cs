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
            if (args.Length == 0)
            {
                Logger.Log(LogLevel.Fatal ,"There was no scripts to execute specified. Run the program with command line arguments equal to the paths of the scripts you want to be executed.");
                Console.ReadKey();
                return;
            }
            Logger.Log(LogLevel.Info, String.Format("Cake started for {0}: {1}", args.Length > 1 ? "scripts" : "script", args.Aggregate((a, b) => String.Format("{0}, {1}", a, b))));

            try
            {
                foreach (var script in args)
                {
                    RoslynEngine.ExecuteFile(script);
                }
            }
            catch (Exception e)
            {
                Logger.LogException(LogLevel.Fatal, e, "A fatal error has occured.");
            }

            Logger.Log(LogLevel.Info, "Cake finished. Press any key to continue...");
            Console.ReadKey();
        }
    }
}
