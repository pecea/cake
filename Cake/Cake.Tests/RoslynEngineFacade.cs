namespace Cake.Tests
{
    using System;

    internal class RoslynEngineFacade : MarshalByRefObject
    {
        public void ExecuteFile(string filePath)
        {
            RoslynEngine.ExecuteFile(filePath);
        }
    }
}