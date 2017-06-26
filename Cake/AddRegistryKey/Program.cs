using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace AddRegistryKey
{
    class Program
    {
        static void Main(string[] args)
        {
            string directory = @"C:\Users\ErnestPrzestrzelski\Desktop\Ernest\priv\workaround.reg";
            Process regeditProcess = Process.Start("regedit.exe", "/s \"" + directory + "\"");
            regeditProcess?.WaitForExit();
            //            RegistryKey softwareKey = Registry.LocalMachine.OpenSubKey("SOFTWARE", true);
            //var newKey = softwareKey?.CreateSubKey(@"\Microsoft\VisualStudio\15.0\EnterpriseTools\QualityTools\TestTypes\{13cdc9d9-ddb5-4fa4-a97d-d965ccfc6d4b}");
            //newKey?.SetValue("test", "test");
        }
    }
}
