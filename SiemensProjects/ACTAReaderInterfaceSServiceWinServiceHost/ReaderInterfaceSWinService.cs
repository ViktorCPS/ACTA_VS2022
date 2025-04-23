using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;

using ReaderInterfaceSService;

namespace ACTAReaderInterfaceSServiceWinServiceHost
{
    public partial class ReaderInterfaceSWinService : ServiceBase
    {
        private static ServiceHost host;

        public ReaderInterfaceSWinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // uncomment to enable pause for attaching process to debugger
            //System.Threading.Thread.Sleep(30000);

            host = new ServiceHost(ReaderInterfaceSService.ReaderInterfaceSService.Instance);
            host.Open();
            ReaderInterfaceSService.ReaderInterfaceSService.Instance.StartLogDownload();

            if (!ReaderInterfaceSService.ReaderInterfaceSService.Instance.IsLogDownloadStarted())
            {
                this.Stop();
            }
        }

        protected override void OnStop()
        {
            if (host != null)
            {
                ReaderInterfaceSService.ReaderInterfaceSService.Instance.StopLogDownload();
                host.Close();
            }
        }
    }
}
