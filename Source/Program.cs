using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VBoxHost
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Environment.UserInteractive) {
                var startInfo = new ProcessStartInfo();
                startInfo.Arguments = "-q list vms";
                startInfo.FileName = "C:\\Program Files\\Oracle\\VirtualBox\\VBoxManage.exe";
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                var host = Process.Start(startInfo);
                host.WaitForExit();
                Console.WriteLine(host.StandardOutput.ReadToEnd());
            } else {
                var service = new VBoxHost();
                ServiceBase.Run(service);
            }
        }
    }
}
