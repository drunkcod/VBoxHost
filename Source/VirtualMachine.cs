using System.Diagnostics;

namespace VBoxHost
{
    class VirtualMachine
    {
        readonly VirtualBoxBridge bridge;
        readonly string nameOrId;
        readonly Process headlessProcess;

        public VirtualMachine(VirtualBoxBridge bridge, string nameOrId) {
            this.bridge = bridge;
            this.nameOrId = nameOrId;
            this.headlessProcess = VBoxHeadless(nameOrId);
        }

        public ILogger Logger { get; set; }

        public void Start() {
            Logger.Info("Starting {0}", nameOrId);
            headlessProcess.Start();
        }

        public void PowerOff() {
            VBoxManage("-q controlvm \"{0}\" poweroff", nameOrId);
            headlessProcess.WaitForExit();
        }

        void VBoxManage(string format, params object[] args) {
            var host = Process.Start(new ProcessStartInfo {
                FileName = bridge.Command("VBoxManage.exe"),
                Arguments = string.Format(format, args),
                UseShellExecute = false,
                RedirectStandardOutput = true
            });
            host.WaitForExit();
        }

        Process VBoxHeadless(string machineName) {
            return new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = bridge.Command("VBoxHeadless.exe"),
                    Arguments = string.Format("-s \"{0}\" -v config", machineName),
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            };
        }
    }
}
