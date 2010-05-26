using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading;

namespace VBoxHost
{
    public class VBoxHost : ServiceBase
    {
        protected override void OnStart(string[] args) {
            EnsureVBoxSvc();
            File.AppendAllText("VBoxHost.log", "OnStart" + "\r\n");
            var startInfo = new ProcessStartInfo();
            startInfo.Arguments = "-q list vms";
            startInfo.FileName = "C:\\Program Files\\Oracle\\VirtualBox\\VBoxManage.exe";
            startInfo.WorkingDirectory = "C:\\";
            startInfo.LoadUserProfile = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;            
            var host = Process.Start(startInfo);
            host.WaitForExit();
            File.AppendAllText("VBoxHost.log", host.StandardOutput.ReadToEnd() + "\r\n");
        }

        void EnsureVBoxSvc() {
            File.AppendAllText("VBoxHost.log", "StartSvc" + "\r\n");
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = "C:\\Program Files\\Oracle\\VirtualBox\\VBoxSVC.exe";
            startInfo.UseShellExecute = true;
            var host = Process.Start(startInfo);
            Thread.Sleep(100);//give it some pre-flight time.
        }
    }
}
