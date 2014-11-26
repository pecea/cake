namespace Cake
{
    using System;

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
            try
            {
                foreach (var script in args)
                {
                    RoslynEngine.ExecuteFile(script);
                }
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
            }
            finally
            {
                Logger.Info("Program finished.");
                Console.ReadKey();
            }
        }
    }
}
