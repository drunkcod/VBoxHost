using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace VBoxHost
{
    public class VirtualBoxBridge
    {
        readonly string installPath; 

        public VirtualBoxBridge(string installPath) {
            this.installPath = installPath;
        }

        public string VirtualBoxInstallPath { get { return installPath; } }

        public string Command(string application) { return Path.Combine(VirtualBoxInstallPath, application); }

        public void EnsureVBoxSvc() {
            var host = Process.Start(Command("VBoxSVC.exe"));
            Thread.Sleep(100);//give it some pre-flight time.
        }
    }
}
