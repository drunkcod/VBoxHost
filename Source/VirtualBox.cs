using System;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace VBoxHost
{
    internal static class VirtualBox
    {
        public static string VirtualBoxInstallPath { get { return Environment.GetEnvironmentVariable("VBOX_INSTALL_PATH"); } }

        public static string Command(string application) { return Path.Combine(VirtualBoxInstallPath, application); }

        public static void EnsureVBoxSvc() {
            var host = Process.Start(VirtualBox.Command("VBoxSVC.exe"));
            Thread.Sleep(100);//give it some pre-flight time.
        }
    }
}
