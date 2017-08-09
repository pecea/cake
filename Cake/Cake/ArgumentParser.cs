namespace Cake
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class ArgumentParser
    {
        static ArgumentParser()
        {
            Arguments = new List<Argument>
                           {
                               new Argument(new[] { "/runjob", "/r" }, true),
                               new Argument(new[] { "/scriptverbosity", "/sv" }, true),
                               new Argument(new[] {"/appverbosity", "/av" }, true),
                               new Argument(new[] { "/help", "/h" }, false),
                               new Argument(new[] { "/script", "/s" }, true)
                           }.ToArray();
        }

        private static readonly Argument[] Arguments;
        /// <summary>
        /// Parses arguments passed when running Cake.
        /// </summary>
        /// <param name="args">Arguments passed to the application</param>
        /// <returns></returns>
        public static Argument[] Parse(string[] args)
        {
            var result = new List<Argument>();

            for (var i = 0; i < args.Length; i++)
            {
                var argName = args[i];
                var argument = Arguments.FirstOrDefault(arg => arg.Names.Contains(argName));
                if (argument == null)
                    throw new ArgumentException(
                        $"The argument {argName} is invalid. Run the program with /help or /h for help.");

                if (argument.HasValue)
                {
                    var argValue = args[++i];
                    result.Add(new Argument(argument.Names, argValue, argument.HasValue));
                }
                else
                {
                    result.Add(new Argument(argument.Names, argument.HasValue));
                }
            }

            return result.ToArray();
        }
    }
}