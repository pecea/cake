namespace Cake
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class ArgumentParser
    {
        static ArgumentParser()
        {
            var args = new List<Argument>
                           {
                               new Argument(new[] { "/runtask", "/r" }, true),
                               new Argument(new[] { "/verbosity", "/v" }, true),
                               new Argument(new[] { "/help", "/h" }, false),
                               new Argument(new[] { "/script", "/s" }, true)
                           };

            Arguments = args.ToArray();
        }

        private static readonly Argument[] Arguments;

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