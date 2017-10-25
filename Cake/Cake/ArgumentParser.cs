using System;
using System.Collections.Generic;
using System.Linq;
using Common;

namespace Cake
{
    internal class ArgumentParser
    {
        internal ArgumentParser()
        {
            _arguments = new List<Argument>
                           {
                               new Argument(new[] { "/runjob", "/r" }, true),
                               new Argument(new[] { "/scriptverbosity", "/sv" }, true),
                               new Argument(new[] {"/appverbosity", "/av" }, true),
                               new Argument(new[] { "/help", "/h" }, false),
                               new Argument(new[] { "/script", "/s" }, true)
                           }.ToArray();
        }

        private readonly Argument[] _arguments;
        /// <summary>
        /// Parses arguments passed when running Cake.
        /// </summary>
        /// <param name="args">Arguments passed to the application</param>
        /// <returns>Array of <see cref="Argument"/></returns>
        public Argument[] Parse(string[] args)
        {
            var result = new List<Argument>();

            for (var i = 0; i < args.Length; i++)
            {
                var argName = args[i];
                var argument = _arguments.FirstOrDefault(arg => arg.Names.Contains(argName));
                if (argument == null)
                    throw new ArgumentException(string.Empty, paramName: argName);

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