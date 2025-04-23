using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using ReaderRemoteControlService;

namespace ReaderRemoteControlWinServiceHost
{
    partial class ReaderRemoteWinService : ServiceBase
    {
        private static ServiceHost host;

        public ReaderRemoteWinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            host = new ServiceHost(ReaderRemoteControlService.ReaderRemoteControlService.Instance);
            host.Open();
            ReaderRemoteControlService.ReaderRemoteControlService.Instance.StartTicketProcessing();
        }

        protected override void OnStop()
        {
            if (host != null)
            {
                ReaderRemoteControlService.ReaderRemoteControlService.Instance.StopTicketProcessing();
                host.Close();
            }
        }
    }
}
