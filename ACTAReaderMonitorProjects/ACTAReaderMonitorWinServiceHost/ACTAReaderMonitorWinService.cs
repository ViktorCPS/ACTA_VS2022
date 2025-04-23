using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;

namespace ACTAReaderMonitorWinServiceHost
{
    partial class ACTAReaderMonitorWinService : ServiceBase
    {
        private static ServiceHost host;

        public ACTAReaderMonitorWinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            host = new ServiceHost(ACTAReaderMonitorService.ACTAReaderMonitorService.Instance);
            host.Open();
            ACTAReaderMonitorService.ACTAReaderMonitorService.Instance.StartTicketProcessing();
        }

        protected override void OnStop()
        {
            if (host != null)
            {
                ACTAReaderMonitorService.ACTAReaderMonitorService.Instance.StopTicketProcessing();
                host.Close();
            }
        }
    }
}
