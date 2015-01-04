namespace Cake.Tests
{
    using System;

    public class RoslynEngineFacade : MarshalByRefObject
    {
        public void ExecuteFile(string filePath)
        {
            RoslynEngine.ExecuteFile(filePath);
        }
    }
}