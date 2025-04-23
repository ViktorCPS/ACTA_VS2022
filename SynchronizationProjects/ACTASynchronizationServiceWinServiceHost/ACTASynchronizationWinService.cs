using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using ACTASynchronizationService;

namespace ACTASynchronizationServiceWinServiceHost
{
    partial class ACTASynchronizationWinService : ServiceBase
    {
        private static ServiceHost host;

        public ACTASynchronizationWinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            host = new ServiceHost(ACTASynchronizationService.ACTASynchronizationService.Instance);
            host.Open();
            ACTASynchronizationService.ACTASynchronizationService.Instance.Start();
        }

        protected override void OnStop()
        {
            if (host != null)
            {
                ACTASynchronizationService.ACTASynchronizationService.Instance.Stop();
                host.Close();
            }
        }
    }
}
