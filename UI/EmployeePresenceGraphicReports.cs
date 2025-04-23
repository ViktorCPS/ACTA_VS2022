using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Resources;
using System.Globalization;

using Common;
using TransferObjects;
using Util;

namespace UI
{
    public partial class EmployeePresenceGraphicReports : Form
    {
        ApplUserTO logInUser;
        ResourceManager rm;
        private CultureInfo culture;
        DebugLog log;
        // Controller instance
        public NotificationController Controller;

        // Observer client instance
        public NotificationObserverClient observerClient;

        public EmployeePresenceGraphicReports()
        {
            InitializeComponent();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            logInUser = NotificationController.GetLogInUser();

            this.CenterToScreen();
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(EmployeeAccessGroups).Assembly);
            setLanguage();

            IntitObserverClient();
                            
        }
        private void setLanguage()
        {
            this.Text = rm.GetString("EmployeePresenceGraphRepots", culture);
            this.tpDay.Text = rm.GetString("gbDay", culture);
            this.tpEmployee.Text = rm.GetString("tpEmployee", culture);
        }
        private void IntitObserverClient()
        {
            Controller = NotificationController.GetInstance();
            observerClient = new NotificationObserverClient(this.ToString());
            Controller.AttachToNotifier(observerClient);
            this.observerClient.Notification += new NotificationEventHandler(this.CloseGraphicReportsClick);
        }
        public void CloseGraphicReportsClick(object sender, NotificationEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (e.closeGrphicReports)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePresenceGraphRepots.CloseGraphicReportsClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally { 
                this.Cursor = Cursors.AppStarting; 
            }
        }

        private void EmployeePresenceGraphicReports_KeyUp(object sender, KeyEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (e.KeyCode.Equals(Keys.F1))
                {
                    Util.Misc.helpManualHtml(this.Name);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePresenceGraphicReports.EmployeePresenceGraphicReports_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
     
    }
}