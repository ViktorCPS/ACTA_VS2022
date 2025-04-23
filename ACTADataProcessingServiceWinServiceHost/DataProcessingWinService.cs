using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;

using DataProcessingService;

namespace ACTADataProcessingServiceWinServiceHost
{
    public partial class DataProcessingWinService : ServiceBase
    {
        private static ServiceHost host;

        public DataProcessingWinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            host = new ServiceHost(DataProcessingService.DataProcessingService.Instance);
            host.Open();
            DataProcessingService.DataProcessingService.Instance.StartLogProcessing();
        }

        protected override void OnStop()
        {
            if (host != null)
            {
                DataProcessingService.DataProcessingService.Instance.StopLogProcessing();
                host.Close();
            }
        }
    }
}
