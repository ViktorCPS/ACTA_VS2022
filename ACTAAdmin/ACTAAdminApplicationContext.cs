using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using UI;

namespace ACTAAdmin
{
    public class ACTAAdminApplicationContext : ApplicationContext
    {
        public bool isMainFormClosed = false;

        public ACTAAdminApplicationContext(string userID, string userPass)
        {
            try
            {
                using (ACTASplashScreen splash = new ACTASplashScreen(ResImages.ACTASplash))
                {
                    ACTA acta = new ACTA();

                    acta.userID = userID;
                    acta.password = userPass;

                    acta.Load += new EventHandler(delegate(object sender, EventArgs e)
                      {
                          splash.Close();
                      });

                    acta.FormClosed += new FormClosedEventHandler(delegate(object sender, FormClosedEventArgs e)
                      {
                          isMainFormClosed = true;
                          ExitThread();
                      });

                    splash.Show();

                    acta.splash = splash;
                    acta.Show();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
