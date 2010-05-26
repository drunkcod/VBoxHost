using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System;
using System.Collections.Generic;

namespace VBoxHost
{
    enum VirtualMachineMode
    {
        Gui, Sdl, Vrdp, Headless
    }

    public class VBoxHost : ServiceBase
    {
        readonly List<string> machineNames = new List<string>();

        public void AddMachine(string name) { machineNames.Add(name); }

        public void Start() { OnStart(new string[0]); }

        protected override void OnStart(string[] args) {
            EnsureVBoxSvc();
            Info("OnStart");
            machineNames.ForEach(machine => StartVM(machine, VirtualMachineMode.Vrdp));
        }

        protected override void OnStop()
        {
            for (int i = machineNames.Count; --i >= 0; )
                PowerOff(machineNames[i]);
        }

        void EnsureVBoxSvc() {
            var host = Process.Start(VirtualBoxCommand("VBoxSVC.exe"));
            Thread.Sleep(100);//give it some pre-flight time.
        }

        string VirtualBoxInstallPath { get { return Environment.GetEnvironmentVariable("VBOX_INSTALL_PATH"); } }

        string VirtualBoxCommand(string application) { return Path.Combine(VirtualBoxInstallPath, application); }

        void StartVM(string name, VirtualMachineMode mode) {

            VBoxManage(string.Format("-q startvm \"{0}\" --type {1}", name, TranslateMachineMode(mode)));
        }

        void PowerOff(string name){
            VBoxManage(string.Format("-q controlvm \"{0}\" poweroff", name));
        }

        static string TranslateMachineMode(VirtualMachineMode mode){
            switch (mode){
                default: return "gui";
                case VirtualMachineMode.Sdl: return "sdl";
                case VirtualMachineMode.Vrdp: return "vrdp";
                case VirtualMachineMode.Headless: return "headless";                  
            }
        }

        void VBoxManage(string arguments) {
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = VirtualBoxCommand("VBoxManage.exe");
            startInfo.Arguments = arguments;
            startInfo.WorkingDirectory = "C:\\";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            var host = Process.Start(startInfo);
            host.WaitForExit();
            Info(host.StandardOutput.ReadToEnd());
        }

        void Info(string message) {
            File.AppendAllText("VBoxHost.log", string.Format("[{0} - Info]{1}{2}", DateTime.Now, message, Environment.NewLine));
        }
    }
}
