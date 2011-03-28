using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;

namespace VBoxHost
{
    public class VirtualBoxHostService : ServiceBase
    {
        readonly List<VirtualMachine> machines = new List<VirtualMachine>();

        public void AddMachine(string name) { machines.Add(new VirtualMachine(name)); }

        public void Start() { OnStart(new string[0]); }

        protected override void OnStart(string[] args) {
            VirtualBox.EnsureVBoxSvc();
            Info("OnStart");
            machines.ForEach(x => x.Start());
        }

        protected override void OnStop() {
            for (int i = machines.Count; --i >= 0; )
                machines[i].PowerOff();
        }

        void Info(string message) {
            File.AppendAllText("VBoxHost.log", string.Format("[{0} - Info]{1}{2}", DateTime.Now, message, Environment.NewLine));
        }
    }
}
