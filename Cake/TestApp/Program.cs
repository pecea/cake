using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Cake;

namespace AddRegistryKey
{
    class Program
    {
        static void Main(string[] args)
        {
            JobManager.SetDefault(new Job("BuildSolution").Does(() =>
            {
                var res = Build.Methods.BuildProject(@"D:\Dane\Ernest\Praca\cake\Cake\Cake.sln", @"D:\Dane\Ernest\Praca\TestOutput\", "Release");
                return res;
            }));
            //RoslynEngine.ExecuteFile(@"D:\Dane\Ernest\Praca\cake\Cake\scripts\realScript.csx");
        }
    }
}
