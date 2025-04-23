using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using System.Globalization;
using System.IO;
using System.Threading;

using Common;
using Util;
using ReaderManagement;
using UI;

using ACTADataProcessingServiceWinClient.DataProcessingService;

namespace ACTADataProcessingServiceWinClient
{
	/// <summary>
	/// Data processing main form.
	/// </summary>
	public class DataProcessingMainForm : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Button btnStartStop;
        private IContainer components;

		// Debug
		DebugLog log;

		// Language settings
		private ResourceManager rm;
		private System.Windows.Forms.Label lblStateVal;
		private System.Windows.Forms.Label lblState;
		private System.Windows.Forms.StatusBar stsBar;
		private CultureInfo culture;

		// Controller instance
		public NotificationController Controller;

        private System.Windows.Forms.Label lblMessage;
		private System.Windows.Forms.Label lblThreadState;
		public const string NOMESSAGE = "---";
        private System.Windows.Forms.Timer timer1;

        private NotifyIcon notifyIcon1;
        private ContextMenuStrip ctxMenu;
        private ToolStripMenuItem openMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem dataProcessingStartToolStripMenuItem;
        private ToolStripMenuItem dataProcessingStopToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;

        // service proxy
        DataProcessingServiceClient dataProcessingServiceClient;

        // tray handling
        private Boolean OkToClose = false;

        // Constants
        const String APP_NAME = "ACTA Data Processing Manager";

        // Service status worker
        private BackgroundWorker ServiceStatusWorker;
        bool serviceStatusAvailable = false;
        string currentFileInProcessing = "";
        string dataProcessingThreadState = "";

        public DataProcessingMainForm()
		{
			InitializeComponent();
            InitializeBackgoundWorkers();
            this.notifyIcon1.Icon = global::DataProcessingServiceWinClient.Properties.Resources.DataProcessingStartingIcon;

			// Init Debug
            NotificationController.SetApplicationName("ACTADataProcessingClient");
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			// Set Language 
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource",typeof(Employees).Assembly);
			setLanguage();
			this.CenterToScreen();

            // initialize service proxy
            dataProcessingServiceClient = new DataProcessingServiceClient();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataProcessingMainForm));
            this.btnStartStop = new System.Windows.Forms.Button();
            this.lblState = new System.Windows.Forms.Label();
            this.lblStateVal = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.stsBar = new System.Windows.Forms.StatusBar();
            this.lblThreadState = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.ctxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.dataProcessingStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataProcessingStopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ServiceStatusWorker = new System.ComponentModel.BackgroundWorker();
            this.ctxMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(168, 128);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(75, 23);
            this.btnStartStop.TabIndex = 0;
            this.btnStartStop.Text = "Start";
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // lblState
            // 
            this.lblState.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblState.Location = new System.Drawing.Point(16, 32);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(144, 23);
            this.lblState.TabIndex = 1;
            this.lblState.Text = "State:";
            this.lblState.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStateVal
            // 
            this.lblStateVal.Location = new System.Drawing.Point(168, 32);
            this.lblStateVal.Name = "lblStateVal";
            this.lblStateVal.Size = new System.Drawing.Size(112, 23);
            this.lblStateVal.TabIndex = 2;
            this.lblStateVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(16, 64);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(376, 24);
            this.lblMessage.TabIndex = 6;
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stsBar
            // 
            this.stsBar.Location = new System.Drawing.Point(0, 157);
            this.stsBar.Name = "stsBar";
            this.stsBar.Size = new System.Drawing.Size(408, 16);
            this.stsBar.TabIndex = 7;
            // 
            // lblThreadState
            // 
            this.lblThreadState.Location = new System.Drawing.Point(16, 96);
            this.lblThreadState.Name = "lblThreadState";
            this.lblThreadState.Size = new System.Drawing.Size(376, 24);
            this.lblThreadState.TabIndex = 8;
            this.lblThreadState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.ctxMenu;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "ACTA Data Processing Manager";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // ctxMenu
            // 
            this.ctxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMenuItem,
            this.toolStripSeparator1,
            this.dataProcessingStartToolStripMenuItem,
            this.dataProcessingStopToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.ctxMenu.Name = "ctxMenu";
            this.ctxMenu.Size = new System.Drawing.Size(298, 126);
            // 
            // openMenuItem
            // 
            this.openMenuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.Size = new System.Drawing.Size(297, 22);
            this.openMenuItem.Text = "Open ACTA Data Processing Manager";
            this.openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(294, 6);
            // 
            // dataProcessingStartToolStripMenuItem
            // 
            this.dataProcessingStartToolStripMenuItem.Enabled = false;
            this.dataProcessingStartToolStripMenuItem.Name = "dataProcessingStartToolStripMenuItem";
            this.dataProcessingStartToolStripMenuItem.Size = new System.Drawing.Size(297, 22);
            this.dataProcessingStartToolStripMenuItem.Text = "Data processing - Start";
            this.dataProcessingStartToolStripMenuItem.Click += new System.EventHandler(this.dataProcessingStartToolStripMenuItem_Click);
            // 
            // dataProcessingStopToolStripMenuItem
            // 
            this.dataProcessingStopToolStripMenuItem.Enabled = false;
            this.dataProcessingStopToolStripMenuItem.Name = "dataProcessingStopToolStripMenuItem";
            this.dataProcessingStopToolStripMenuItem.Size = new System.Drawing.Size(297, 22);
            this.dataProcessingStopToolStripMenuItem.Text = "Data processing - Stop";
            this.dataProcessingStopToolStripMenuItem.Click += new System.EventHandler(this.dataProcessingStopToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(294, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(297, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(297, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // ServiceStatusWorker
            // 
            this.ServiceStatusWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ServiceStatusWorker_DoWork);
            this.ServiceStatusWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ServiceStatusWorker_RunWorkerCompleted);
            // 
            // DataProcessingMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 173);
            this.Controls.Add(this.lblThreadState);
            this.Controls.Add(this.stsBar);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblStateVal);
            this.Controls.Add(this.lblState);
            this.Controls.Add(this.btnStartStop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(416, 200);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(416, 200);
            this.Name = "DataProcessingMainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ACTA Data Processing";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DataProcessingMainForm_FormClosing);
            this.Load += new System.EventHandler(this.DataProcessingMainForm_Load);
            this.ctxMenu.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

        private void InitializeBackgoundWorkers()
        {
            //used to poll service status
            ServiceStatusWorker.WorkerReportsProgress = false;
            ServiceStatusWorker.WorkerSupportsCancellation = false;
        }

        private void DataProcessingMainForm_Load(object sender, EventArgs e)
        {
            this.Hide();
            this.timer1.Enabled = true;
        }
        
        private void btnStartStop_Click(object sender, System.EventArgs e)
		{
            try
            {
                if (!dataProcessingServiceClient.IsProcessing())
                {
                    if (dataProcessingServiceClient.ChekPrerequests())
                    {
                        dataProcessingServiceClient.StartLogProcessing();
                        this.lblMessage.Text = NOMESSAGE;
                    }
                    else
                    {
                        DialogResult procreq = MessageBox.Show(rm.GetString("SettingsFailed", culture), APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (procreq == DialogResult.OK)
                        {
                            return;
                        }
                    }
                }
                else
                {
                    Cursor cursor = this.Cursor;
                    this.Cursor = Cursors.WaitCursor;
                    bool stopped = dataProcessingServiceClient.StopLogProcessing();
                    this.Cursor = cursor;
                    if (stopped)
                    {
                        MessageBox.Show(rm.GetString("LogProcStopped", culture), APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.lblMessage.Text = NOMESSAGE;
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("LogProcCantStop", culture), APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

		private void setLanguage()
		{
			try
			{
				this.Text = rm.GetString("DataProcessing", culture);

				this.lblState.Text = rm.GetString("lblState", culture);
				//this.lblCurrFile.Text = rm.GetString("lblCurrFile", culture);
				this.lblMessage.Text = NOMESSAGE;

			}
			catch
			{
				MessageBox.Show(rm.GetString("LogProcCantStop", culture));
			}
		}

        /// <summary>
        /// Calls service status background worker to refresh service status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ServiceStatusWorker.IsBusy == false)
            {
                ServiceStatusWorker.RunWorkerAsync();
            }
        }

        private void DataProcessingMainForm_FormClosing(object sender, FormClosingEventArgs e)
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
                    dataProcessingServiceClient.Close();

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

        private void dataProcessingStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnStartStop_Click(sender, e);
        }

        private void dataProcessingStopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnStartStop_Click(sender, e);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aform = new AboutForm();
            aform.ShowDialog(this);
            aform.Dispose();
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

        /// <summary>
        /// Gets current file in processing and data processing thread state from the service.
        /// </summary>
        private void BackgroundRefreshService()
        {
            serviceStatusAvailable = false;
            try
            {
                currentFileInProcessing = dataProcessingServiceClient.GetCurrentFileInProcessing();
                dataProcessingThreadState = dataProcessingServiceClient.GetDataProcessingState();
                serviceStatusAvailable = true;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " " + this.ToString() + ".BackgroundRefreshService(): " + ex.Message + "\n");
            }
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
                    if (currentFileInProcessing.StartsWith(Constants.fileInProcessing))
                    {
                        currentFileInProcessing = currentFileInProcessing.Replace(Constants.fileInProcessing, "");
                        this.lblMessage.Text = rm.GetString("lblCurrFile", culture) + " " + currentFileInProcessing;
                    }
                    else
                    {
                        this.lblMessage.Text = currentFileInProcessing;
                    }

                    this.lblThreadState.Text = dataProcessingThreadState;

                    if (dataProcessingThreadState.StartsWith("Thread: processing started") || dataProcessingThreadState.StartsWith("Thread: processing restarted"))
                    {
                        this.btnStartStop.Text = rm.GetString("btnStop", culture);
                        this.lblStateVal.Text = rm.GetString("activated", culture);
                        this.dataProcessingStartToolStripMenuItem.Enabled = false;
                        this.dataProcessingStopToolStripMenuItem.Enabled = true;
                    }
                    else if (dataProcessingThreadState.StartsWith("Thread: processing finished") || dataProcessingThreadState.StartsWith("Thread: processing aborted"))
                    {
                        this.btnStartStop.Text = rm.GetString("btnStart", culture);
                        this.lblStateVal.Text = rm.GetString("stopped", culture);
                        this.dataProcessingStartToolStripMenuItem.Enabled = true;
                        this.dataProcessingStopToolStripMenuItem.Enabled = false;
                    }

                    // if thread stopped after max number of retries
                    if (dataProcessingThreadState.StartsWith("Thread: after"))
                    {
                        this.lblStateVal.Text = rm.GetString("stopped", culture);
                        this.btnStartStop.Text = rm.GetString("btnStart", culture);
                    }
                }

                // update notify icon
                if (serviceStatusAvailable)
                {
                    this.notifyIcon1.Icon = global::DataProcessingServiceWinClient.Properties.Resources.DataProcessingRunningIcon;
                    this.btnStartStop.Enabled = true;
                    this.stsBar.Text = "";
                }
                else
                {
                    this.notifyIcon1.Icon = global::DataProcessingServiceWinClient.Properties.Resources.DataProcessingStoppedIcon;
                    this.btnStartStop.Enabled = false;
                    this.stsBar.Text = "Getting status from the service failed!";
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " " + this.ToString() + ".RefreshMainForm(): " + ex.Message + "\n");
            }
        }

        private void ServiceStatusWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundRefreshService();
        }

        private void ServiceStatusWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RefreshMainForm();
        }
	}
}
