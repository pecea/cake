using System;

namespace Build
{
    internal class CompilationException : ApplicationException
    {
        public CompilationException(string projectName)
            : base($"{projectName} did not compile successfully.")
        {
        }
    }
}
