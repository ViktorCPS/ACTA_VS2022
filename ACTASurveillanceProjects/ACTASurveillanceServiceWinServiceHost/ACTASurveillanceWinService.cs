using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using ACTASurveillanceService;

namespace ACTASurveillanceServiceWinServiceHost
{
    partial class ACTASurveillanceWinService : ServiceBase
    {
        private static ServiceHost host;

        public ACTASurveillanceWinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            host = new ServiceHost(ACTASurveillanceService.ACTASurveillanceService.Instance);
            host.Open();
            ACTASurveillanceService.ACTASurveillanceService.Instance.Start();
        }

        protected override void OnStop()
        {
            if (host != null)
            {
                ACTASurveillanceService.ACTASurveillanceService.Instance.Stop();
                host.Close();
            }
        }
    }
}
