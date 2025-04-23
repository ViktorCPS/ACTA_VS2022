using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using Util;
using System.Resources;
using System.Globalization;
using UI;
using System.ServiceProcess;

namespace ACTASynchronizationServiceWinClient
{
    public partial class ACTASynchronizationServiceClient : Form
    {
        // Debug
        DebugLog log;
        ServiceReference1.ACTASynchronizationServiceClient client;
        // tray handling
        private Boolean OkToClose = false;
        bool serviceStatusAvailable = false;
        string syncThreadState = "";
        // Constants
        const String APP_NAME = "ACTA Synchronization Manager";
        // Language settings
        private ResourceManager rm;
        private CultureInfo culture;

        public ACTASynchronizationServiceClient()
        {
            InitializeComponent();
            InitializeBackgoundWorkers();
            this.notifyIcon1.Icon = global::ACTASynchronizationServiceWinClient.Properties.Resources.sync_not_active;
            if (NotificationController.GetApplicationName().Equals(""))
            {
                NotificationController.SetApplicationName(this.Text.Replace(" ", ""));
            } 
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
            client = new ServiceReference1.ACTASynchronizationServiceClient();
            // Set Language 
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(ACTASynchronizationServiceClient).Assembly); ;
            this.CenterToScreen();
        }
        private void InitializeBackgoundWorkers()
        {
            //used to poll service status
            ServiceStatusWorker.WorkerReportsProgress = false;
            ServiceStatusWorker.WorkerSupportsCancellation = false;
        }
        private void openMenuItem_Click(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState && this.Visible == true)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
            Application.DoEvents();
        }

        /// <summary>
        /// Gets current file in processing and data processing thread state from the service.
        /// </summary>
        private void BackgroundRefreshService()
        {
            try
            {
                syncThreadState = client.GetServiceStatus();
                serviceStatusAvailable = true;
            }
            catch (Exception ex)
            {
                serviceStatusAvailable = false;
                log.writeLog(DateTime.Now + " " + this.ToString() + ".BackgroundRefreshService(): " + ex.Message + "\n");
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OkToClose = true;
                this.timer1.Enabled = false;
                ServiceStatusWorker.Dispose();
                this.Close();
            }
            catch
            { }
            finally
            {
                Application.Exit();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ServiceStatusWorker.IsBusy == false)
            {
                ServiceStatusWorker.RunWorkerAsync();
            }
        }

        private void ServiceStatusWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundRefreshService();
        }

        private void ServiceStatusWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RefreshMainForm();
        }
        /// <summary>
        /// Refreshes UI main form with current data processing service status.
        /// </summary>
        private void RefreshMainForm()
        {
            try
            {
                if (serviceStatusAvailable)
                {                  
                        this.lblMessage.Text = syncThreadState;                   
                }

                // update notify icon
                if (serviceStatusAvailable)
                {
                    this.notifyIcon1.Icon = global::ACTASynchronizationServiceWinClient.Properties.Resources.sync_active;
                    this.stsBar.Text = "";
                    btnStartStop.Text = "Stop";  
                }
                else
                {
                    this.notifyIcon1.Icon = global::ACTASynchronizationServiceWinClient.Properties.Resources.sync_not_active;
                    this.stsBar.Text = "Getting status from the service failed!";
                    btnStartStop.Text = "Start";
                    this.lblMessage.Text = "";
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " " + this.ToString() + ".RefreshMainForm(): " + ex.Message + "\n");
            }
        }

        private void ACTASynchronizationServiceClient_Load(object sender, EventArgs e)
        {
            this.Hide();
            this.timer1.Enabled = true;
        }

        private void ACTASynchronizationServiceClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (e.CloseReason == CloseReason.WindowsShutDown)
                {
                    OkToClose = true;
                }

                if (OkToClose == false)
                {
                    e.Cancel = true; //don't close 
                    this.Hide();
                    return;
                }

                DialogResult procreq = DialogResult.No;
                if (e.CloseReason != CloseReason.WindowsShutDown)
                {
                    procreq = MessageBox.Show(rm.GetString("exitApplication", culture), APP_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }

                if (procreq == DialogResult.Yes || e.CloseReason == CloseReason.WindowsShutDown)
                {
                    this.timer1.Enabled = false;
                     client.Close();

                    log.writeLog(DateTime.Now + " " + this.ToString() + " has been closed! \n");
                }
                else
                {
                    e.Cancel = true;
                    OkToClose = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                log.writeLog(DateTime.Now + " " + this.ToString() + ".DataProcessing_Closing(): " + ex.Message + "\n");
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState && this.Visible == true)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
            Application.DoEvents();
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serviceStatusAvailable)
                {

                    ServiceController service = new ServiceController("ACTA Synchronization Service");
                    try
                    {
                        TimeSpan timeout = TimeSpan.FromMilliseconds(10000);

                        service.Start();
                        service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                        btnStartStop.Text = "Stop";
                    }
                    catch
                    {
                        DialogResult procreq = MessageBox.Show("Starting faild", NotificationController.GetApplicationName(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    ServiceController service = new ServiceController("ACTA Synchronization Service");
                    try
                    {
                        TimeSpan timeout = TimeSpan.FromMilliseconds(10000);

                        service.Stop();
                        service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                        MessageBox.Show("ACTA Synchronization Service stopped successfully", NotificationController.GetApplicationName(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //this..Text = NOMESSAGE;
                        btnStartStop.Text = "Start";
                    }
                    catch
                    {
                        MessageBox.Show("ACTA Synchronization Service stop faild", NotificationController.GetApplicationName(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                }
            }
            catch (Exception ex)
            {
                this.timer1.Enabled = false;
                MessageBox.Show("Start/Stop service command failed. Please restart the service.\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                log.writeLog(DateTime.Now + " " + this.ToString() + ".btnStartStop_Click(): " + ex.Message + "\n");
            }
        }

        private void btnStartStop_TextChanged(object sender, EventArgs e)
        {
            syncStartToolStripMenuItem.Enabled = (btnStartStop.Text == "Start");
            SyncStopToolStripMenuItem.Enabled = (btnStartStop.Text != "Start");
        }
    }
}
