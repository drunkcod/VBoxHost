using System.Diagnostics;

namespace VBoxHost
{
    class VirtualMachine
    {
        readonly string nameOrId;
        readonly Process headlessProcess;

        public VirtualMachine(string nameOrId) {
            this.nameOrId = nameOrId;
            this.headlessProcess = VBoxHeadless(nameOrId);
        }

        public void Start() {
            headlessProcess.Start();
        }

        public void PowerOff() {
            VBoxManage("-q controlvm \"{0}\" poweroff", nameOrId);
            headlessProcess.WaitForExit();
        }

        static void VBoxManage(string format, params object[] args) {
            var host = Process.Start(new ProcessStartInfo {
                FileName = VirtualBox.Command("VBoxManage.exe"),
                Arguments = string.Format(format, args),
                UseShellExecute = false,
                RedirectStandardOutput = true
            });
            host.WaitForExit();
        }

        static Process VBoxHeadless(string machineName) {
            return new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = VirtualBox.Command("VBoxHeadless.exe"),
                    Arguments = string.Format("-s \"{0}\" -v config", machineName),
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            };
        }
    }
}
