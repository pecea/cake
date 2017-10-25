using System;

namespace Git.Helpers
{
    /// <summary>
    /// Class for helping reading console input
    /// </summary>
    public static class ConsoleHelper
    {
        /// <summary>
        /// Method for reading console input
        /// </summary>
        /// <returns>Input, masked for passwords</returns>
        public static string ReadLineMasked()
        {
            var pass = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Backspace should not work
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key != ConsoleKey.Backspace || pass.Length <= 0) continue;
                    pass = pass.Substring(0, (pass.Length - 1));
                    Console.Write("\b \b");
                }
            }
            // Stops receving keys once enter is pressed
            while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();

            return pass;
        }
    }
}
