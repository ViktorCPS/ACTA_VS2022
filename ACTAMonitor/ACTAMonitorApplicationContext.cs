using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using UI;

namespace ACTAMonitor
{
    public class ACTAMonitorApplicationContext : ApplicationContext
    {
        public bool isMainFormClosed = false;

        public ACTAMonitorApplicationContext()
        {
            try
            {
                using (ACTASplashScreen splash = new ACTASplashScreen(ResImages.ACTASplash))
                {
                    ACTAMonitorLib.Monitor.Instance.Load += new EventHandler(delegate(object sender, EventArgs e)
                      {
                          splash.Close();
                      });

                    ACTAMonitorLib.Monitor.Instance.FormClosed += new FormClosedEventHandler(delegate(object sender, FormClosedEventArgs e)
                      {
                          isMainFormClosed = true;
                          ExitThread();
                      });

                    //splash.Show();

                    //ACTAMonitorLib.Monitor.Instance.splash = splash;
                    ACTAMonitorLib.Monitor.Instance.Show();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
