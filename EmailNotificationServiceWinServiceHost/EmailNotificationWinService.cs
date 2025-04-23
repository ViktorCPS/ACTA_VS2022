using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;

namespace EmailNotificationServiceWinServiceHost
{
    public partial class EmailNotificationWinService : ServiceBase
    {
        private static ServiceHost host;

        public EmailNotificationWinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            host = new ServiceHost(EmailNotificationService.EmailNotificationService.Instance);
            host.Open();
            EmailNotificationService.EmailNotificationService.Instance.StartNotification();
        }

        protected override void OnStop()
        {
            if (host != null)
            {

                EmailNotificationService.EmailNotificationService.Instance.StopNotification();
                host.Close();
            }
        }
    }
}
