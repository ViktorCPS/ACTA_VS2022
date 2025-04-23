using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.ServiceProcess;
using Util;

namespace ACTASurveillanceServiceWinServiceHost
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ServiceBase[] ServicesToRun;           

            ServicesToRun = new ServiceBase[] { new ACTASurveillanceWinService() };

            ServiceBase.Run(ServicesToRun);
        }
    }
}
