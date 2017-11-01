using System;

namespace Git.Helpers
{
    internal static class ConsoleHelper
    {
        internal static string ReadLineMasked()
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
