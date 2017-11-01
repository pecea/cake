using System;

namespace Build
{
    internal class CompilationException : ApplicationException
    {
        internal CompilationException(string projectName)
            : base($"{projectName} did not compile successfully.")
        {
        }
    }
}
