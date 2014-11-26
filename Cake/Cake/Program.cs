namespace Cake
{
    using System;

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
            foreach (var script in args)
            {
                RoslynEngine.ExecuteFile(script);
            }
            Console.ReadKey();
        }
    }
}
