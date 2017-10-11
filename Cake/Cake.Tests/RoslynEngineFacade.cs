using System;

namespace Cake.Tests
{
    internal class RoslynEngineFacade : MarshalByRefObject
    {
        public void ExecuteFile(string filePath)
        {
            RoslynEngine.Instance.ExecuteFile(filePath).Wait();
        }
    }
}