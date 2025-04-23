using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;

namespace ACTAWorkAnalysisWinServiceHost
{
    partial class ACTAWorkAnalysisWinService : ServiceBase
    {
        private static ServiceHost host;

        public ACTAWorkAnalysisWinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            host = new ServiceHost(ACTAWorkAnalysisService.ACTAWorkAnalysisService.Instance);
            host.Open();
            ACTAWorkAnalysisService.ACTAWorkAnalysisService.Instance.Start();
        }

        protected override void OnStop()
        {
            if (host != null)
            {
                ACTAWorkAnalysisService.ACTAWorkAnalysisService.Instance.Stop();
                host.Close();
            }
        }
    }
}
