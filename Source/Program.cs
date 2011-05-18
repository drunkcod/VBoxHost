using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text.RegularExpressions;

namespace VBoxHost
{
    class Program
    {
        static void Main(string[] args) {
            var vboxPath = Environment.GetEnvironmentVariable("VBOX_INSTALL_PATH");
            var machines = new List<string>();

            foreach(var item in args) {
                var m = Regex.Match(item, @"--vboxpath=(?<value>.+$)");
                if(m.Success)
                    vboxPath = m.Groups["value"].Value;
                else
                    machines.Add(item);
            }

            var bridge = new VirtualBoxBridge(vboxPath);
            var host = new VirtualBoxHostService(bridge);
            foreach (var item in machines)
                host.AddMachine(item);
            if (Environment.UserInteractive) {
                host.Start();
                Console.WriteLine("<press any key to stop host>");
                Console.ReadKey();
                host.Stop();
            } else {
                ServiceBase.Run(host);
            }
        }
    }
}
