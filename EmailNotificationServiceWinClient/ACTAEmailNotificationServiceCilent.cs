using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceProcess;
using Common;
using Util;
using System.Resources;
using System.Globalization;

namespace EmailNotificationServiceWinClient
{
    public partial class ACTAEmailNotificationServiceClient : Form
    {
        private Boolean OkToClose = false;
        bool serviceStatusAvailable = false;
        DebugLog log;
        string syncThreadState = "";
        string notificationStatus = "";
        const String APP_NAME = "ACTA EmailNotification Manager";
        ServiceReference1.EmailNotificationServiceClient client;
        private ResourceManager rm;
        private CultureInfo culture;

        public ACTAEmailNotificationServiceClient()
        {
            InitializeComponent();
            InitializeBackgoundWorkers();
            this.notifyIcon1.Icon = global::EmailNotificationServiceWinClient.Properties.Resources.email;

            NotificationController.SetApplicationName("ACTAEmailNotificationManager");

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
            client = new ServiceReference1.EmailNotificationServiceClient();
            // Set Language 
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(ACTAEmailNotificationServiceClient).Assembly); ;
            this.CenterToScreen();
        }

        private void openACTAEmailNotificationManagerToolStripMenuItem_Click(object sender, EventArgs e)
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
        private void InitializeBackgoundWorkers()
        {
            //used to poll service status
            ServiceStatusWorker.WorkerReportsProgress = false;
            ServiceStatusWorker.WorkerSupportsCancellation = false;
        }

        private void BackgroundRefreshService()
        {
            try
            {
                syncThreadState = client.GetServiceStatus();
                notificationStatus = client.GetServiceStatus();
                serviceStatusAvailable = true;
            }
            catch (Exception ex)
            {
                serviceStatusAvailable = false;
                log.writeLog(DateTime.Now + " " + this.ToString() + ".BackgroundRefreshService(): " + ex.Message + "\n");
            }
        }
        private void btStartStop_Click(object sender, EventArgs e)
        {
            try
            {
                   if (!serviceStatusAvailable)
                    {
                        ServiceController service = new ServiceController("ACTA Email Notification Service");
                        try
                        {
                            TimeSpan timeout = TimeSpan.FromMilliseconds(10000);

                            service.Start();
                            service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                            btStartStop.Text = "Stop";
                        }
                        catch
                        {
                            DialogResult procreq = MessageBox.Show("Starting failed", NotificationController.GetApplicationName(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                    }
                    else
                    {
                        ServiceController service = new ServiceController("ACTA Email Notification Service");
                        try
                        {
                            TimeSpan timeout = TimeSpan.FromMilliseconds(10000);

                            service.Stop();
                            service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                            MessageBox.Show("Email notification Service stopped successfully", NotificationController.GetApplicationName(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //this..Text = NOMESSAGE;
                            btStartStop.Text = "Start";
                        }
                        catch
                        {
                            MessageBox.Show("Email notification Service stop failed", NotificationController.GetApplicationName(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

        private void btnStartStop_TextChanged(object sender, EventArgs e)
        {
            notificationStartToolStripMenuItem.Enabled = (btStartStop.Text == "Start");
            notificationEndToolStripMenuItem.Enabled = (btStartStop.Text == "Stop");
        }

        private void ACTAEmailNotificationServiceClient_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
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

        private void ServiceStatusWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (serviceStatusAvailable)
                {
                    this.lblMessage.Text = syncThreadState;
                    this.lblNotificationStatus.Text = notificationStatus;
                }

                // update notify icon
                if (serviceStatusAvailable)
                {
                    this.notifyIcon1.Icon = global::EmailNotificationServiceWinClient.Properties.Resources.mail;
                    // this.stsBar.Text = "";
                    btStartStop.Text = "Stop";
                }
                else
                {
                    this.notifyIcon1.Icon = global::EmailNotificationServiceWinClient.Properties.Resources.email;
                    //   this.stsBar.Text = "Getting status from the service failed!";
                    btStartStop.Text = "Start";
                    this.lblMessage.Text = "";
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " " + this.ToString() + ".RefreshMainForm(): " + ex.Message + "\n");
            }
        }

        private void ACTAEmailNotificationServiceClient_Load(object sender, EventArgs e)
        {
            this.Hide();
            this.timer1.Enabled = true;
        }

        private void ServiceStatusWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundRefreshService();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ServiceStatusWorker.IsBusy == false)
            {
                ServiceStatusWorker.RunWorkerAsync();
            }
        }
    }
}
