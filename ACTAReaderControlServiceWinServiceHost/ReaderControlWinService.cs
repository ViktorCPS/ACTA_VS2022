using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;

using ReaderControlService;

namespace ACTAReaderControlServiceWinServiceHost
{
    public partial class ReaderControlWinService : ServiceBase
    {
        private static ServiceHost host;

        public ReaderControlWinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // uncomment to enable pause for attaching process to debugger
            //System.Threading.Thread.Sleep(30000);

            host = new ServiceHost(ReaderControlService.ReaderControlService.Instance);
            host.Open();
            ReaderControlService.ReaderControlService.Instance.StartLogDownload();

            if (!ReaderControlService.ReaderControlService.Instance.IsLogDownloadStarted())
            {
                this.Stop();
            }
        }

        protected override void OnStop()
        {
            if (host != null)
            {
                ReaderControlService.ReaderControlService.Instance.StopLogDownload();
                host.Close();
            }
        }
    }
}
