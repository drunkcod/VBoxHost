using System.Collections.Generic;
using System.ServiceProcess;

namespace VBoxHost
{
    public interface ILogger 
    {
        void Info(string format, params object[] args);
    }

    public class NullLogger : ILogger
    {
        public void Info(string format, params object[] args) {}
    }

    public class VirtualBoxHostService : ServiceBase
    {
        readonly VirtualBoxBridge bridge;
        readonly List<VirtualMachine> machines = new List<VirtualMachine>();

        public VirtualBoxHostService(VirtualBoxBridge bridge) {
            this.bridge = bridge;
            Logger = new NullLogger();
        }

        public void AddMachine(string name) { 
            machines.Add(new VirtualMachine(bridge, name) {
                Logger = Logger
            }); 
        }

        public void Start() { OnStart(new string[0]); }

        protected override void OnStart(string[] args) {
            bridge.EnsureVBoxSvc();
            Logger.Info("OnStart");
            machines.ForEach(x => x.Start());
        }

        public ILogger Logger { get; set; }

        protected override void OnStop() {
            for (int i = machines.Count; --i >= 0; )
                machines[i].PowerOff();
        }
    }
}
