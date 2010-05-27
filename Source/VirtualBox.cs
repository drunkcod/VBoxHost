using System;
using System.IO;

namespace VBoxHost
{
    internal static class VirtualBox
    {
        public static string VirtualBoxInstallPath { get { return Environment.GetEnvironmentVariable("VBOX_INSTALL_PATH"); } }

        public static string Command(string application) { return Path.Combine(VirtualBoxInstallPath, application); }
    }
}
