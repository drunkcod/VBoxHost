﻿using System;
using System.ServiceProcess;

namespace VBoxHost
{
    class Program
    {
        static void Main(string[] args) {
            var host = new VBoxHost();
            foreach (var item in args)
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
