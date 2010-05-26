using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ServiceProcess;
using System.IO;
using System.ComponentModel;
using System.Configuration.Install;

namespace VBoxHost
{
    [RunInstaller(true)]
    public class VBoxHostInstaller : Installer
    {
        private ServiceInstaller serviceInstaller1;
        private ServiceProcessInstaller processInstaller;

        public VBoxHostInstaller()
        {
            // Instantiate installers for process and services.
            processInstaller = new ServiceProcessInstaller();
            serviceInstaller1 = new ServiceInstaller();

            // The services run under the system account.
            processInstaller.Account = ServiceAccount.LocalSystem;

            // The services are started manually.
            serviceInstaller1.StartType = ServiceStartMode.Manual;

            // ServiceName must equal those on ServiceBase derived classes.
            serviceInstaller1.ServiceName = "VBoxHost";
            serviceInstaller1.DisplayName = "VBoxHost";                      

            // Add installers to collection. Order is not important.
            Installers.Add(serviceInstaller1);
            Installers.Add(processInstaller);
        }

    }

    public class VBoxHost : ServiceBase
    {
        protected override void OnStart(string[] args) {
            StartSvc();
            File.AppendAllText("VBoxHost.log", "OnStart" + "\r\n");
            var startInfo = new ProcessStartInfo();
            startInfo.Arguments = "-q list vms";
            startInfo.FileName = "C:\\Program Files\\Oracle\\VirtualBox\\VBoxManage.exe";
            startInfo.WorkingDirectory = "C:\\";
            startInfo.LoadUserProfile = true;
            //startInfo.FileName = @"D:\junk\VBoxHost\EchoEnv\bin\Debug\EchoEnv.exe";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;            
            var host = Process.Start(startInfo);
            host.WaitForExit();
            File.AppendAllText("VBoxHost.log", host.StandardOutput.ReadToEnd() + "\r\n");
        }

        void StartSvc() {
            File.AppendAllText("VBoxHost.log", "StartSvc" + "\r\n");
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = "C:\\Program Files\\Oracle\\VirtualBox\\VBoxSVC.exe";
            //startInfo.FileName = @"D:\junk\VBoxHost\EchoEnv\bin\Debug\EchoEnv.exe";
            startInfo.UseShellExecute = true;
            var host = Process.Start(startInfo);
            host.WaitForInputIdle();
            //File.AppendAllText("VBoxHost.log", host.StandardOutput.ReadToEnd() + "\r\n");

        }
    }

    class Program
    {
        static void Main(string[] args) {
            if (Environment.UserInteractive) {
                var startInfo = new ProcessStartInfo();
                startInfo.Arguments = "-q list vms";
                startInfo.FileName = "C:\\Program Files\\Oracle\\VirtualBox\\VBoxManage.exe";
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                var host = Process.Start(startInfo);
                host.WaitForExit();
                Console.WriteLine(host.StandardOutput.ReadToEnd());
            }
            else {
                var service = new VBoxHost();        
                ServiceBase.Run(service);
            }
        }
    }
}
