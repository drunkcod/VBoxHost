using System.Diagnostics;

namespace VBoxHost
{
    class VirtualMachine
    {
        readonly string nameOrId;

        public VirtualMachine(string nameOrId) {
            this.nameOrId = nameOrId;
        }

        public void Start(VirtualMachineMode mode) {
            VBoxManage(string.Format("-q startvm \"{0}\" --type {1}", nameOrId, TranslateMachineMode(mode)));
        }

        public void PowerOff() {
            VBoxManage(string.Format("-q controlvm \"{0}\" poweroff", nameOrId));
        }

        static string TranslateMachineMode(VirtualMachineMode mode) {
            switch (mode)
            {
                default: return "gui";
                case VirtualMachineMode.Sdl: return "sdl";
                case VirtualMachineMode.Vrdp: return "vrdp";
                case VirtualMachineMode.Headless: return "headless";
            }
        }

        static void VBoxManage(string arguments) {
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = VirtualBox.Command("VBoxManage.exe");
            startInfo.Arguments = arguments;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            var host = Process.Start(startInfo);
            host.WaitForExit();
        }
    }
}
