using Cake;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            JobManager.SetDefault(new Job("BuildSolution").Does(() =>
            {
                var res = Build.Methods.BuildSolutionAsync(@"D:\Dane\Ernest\Praca\cake\Cake\Cake.sln", @"D:\Dane\Ernest\Praca\Output", "Release");
                return res.Result;
            }));
            //RoslynEngine.ExecuteFile(@"D:\Dane\Ernest\Praca\cake\Cake\scripts\realScript.csx");
        }
    }
}
