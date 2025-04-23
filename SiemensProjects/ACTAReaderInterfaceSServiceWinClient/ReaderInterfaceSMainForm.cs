using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using System.Globalization;
using System.Threading;

using ReaderInterfaceSManagement;
using Common;
using Util;
using UI;
using TransferObjects;

using ACTAReaderInterfaceSServiceWinClient.ReaderInterfaceSService;

namespace ACTAReaderInterfaceSServiceWinClient
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class ReaderInterfaceSMainForm : System.Windows.Forms.Form
    {
        private IContainer components;
		// Check Box list, each check box contains in the tag property 
		// Reader's object.
		ArrayList cbReaderList = new ArrayList();

		// Debug
		DebugLog log;

		// Langage
		ResourceManager rm;
		private System.Windows.Forms.Button btnStartStop;
		private System.Windows.Forms.StatusBar statusBar;
        private ContextMenuStrip ctxMenu;
        private ToolStripMenuItem openMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem readerControlStartToolStripMenuItem;
        private ToolStripMenuItem readerControlStopToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private NotifyIcon notifyIcon1;
		private CultureInfo culture;

        // service proxy
        ReaderInterfaceSServiceClient readerInterfaceSServiceClient;

        // tray handling
        private Boolean OkToClose = false;

        // Constants
        const String APP_NAME = "ACTA Reader InterfaceS Manager";

        // Service status worker
        private BackgroundWorker ServiceStatusWorker;
        bool serviceStatusAvailable = false;

        // data from the service
        private ReadersStatuses readersStatuses;
        private bool isLogDownloadStarted;

        private static bool isReadersListCreated = false;
        private static bool canRefreshReaderActivities = true;

		public ReaderInterfaceSMainForm()
		{
			InitializeComponent();
            InitializeBackgoundWorkers();
            this.notifyIcon1.Icon = global::ACTAReaderInterfaceSServiceWinClient.Properties.Resources.ReaderControlStartingIcon;

			// Debug
            NotificationController.SetApplicationName("ACTAReaderInterfaceSClient");
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			// Language
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource",typeof(Employees).Assembly);
			setLanguage();

            // initialize service proxy
            readerInterfaceSServiceClient = new ReaderInterfaceSServiceClient();

			cbReaderList = CraeteReaderList();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReaderInterfaceSMainForm));
            this.btnStartStop = new System.Windows.Forms.Button();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.ctxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.readerControlStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readerControlStopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ServiceStatusWorker = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.ctxMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStartStop
            // 
            this.btnStartStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStartStop.Location = new System.Drawing.Point(262, 336);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(70, 23);
            this.btnStartStop.TabIndex = 2;
            this.btnStartStop.Text = "Start";
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // statusBar
            // 
            this.statusBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.statusBar.Dock = System.Windows.Forms.DockStyle.None;
            this.statusBar.Location = new System.Drawing.Point(0, 383);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(694, 24);
            this.statusBar.TabIndex = 3;
            // 
            // ctxMenu
            // 
            this.ctxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMenuItem,
            this.toolStripSeparator1,
            this.readerControlStartToolStripMenuItem,
            this.readerControlStopToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.ctxMenu.Name = "ctxMenu";
            this.ctxMenu.Size = new System.Drawing.Size(289, 126);
            // 
            // openMenuItem
            // 
            this.openMenuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.Size = new System.Drawing.Size(288, 22);
            this.openMenuItem.Text = "Open ACTA Reader Control Manager";
            this.openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(285, 6);
            // 
            // readerControlStartToolStripMenuItem
            // 
            this.readerControlStartToolStripMenuItem.Enabled = false;
            this.readerControlStartToolStripMenuItem.Name = "readerControlStartToolStripMenuItem";
            this.readerControlStartToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.readerControlStartToolStripMenuItem.Text = "Reader control - Start";
            this.readerControlStartToolStripMenuItem.Click += new System.EventHandler(this.readerControlStartToolStripMenuItem_Click);
            // 
            // readerControlStopToolStripMenuItem
            // 
            this.readerControlStopToolStripMenuItem.Enabled = false;
            this.readerControlStopToolStripMenuItem.Name = "readerControlStopToolStripMenuItem";
            this.readerControlStopToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.readerControlStopToolStripMenuItem.Text = "Reader control - Stop";
            this.readerControlStopToolStripMenuItem.Click += new System.EventHandler(this.readerControlStopToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(285, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // ServiceStatusWorker
            // 
            this.ServiceStatusWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ServiceStatusWorker_DoWork);
            this.ServiceStatusWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ServiceStatusWorker_RunWorkerCompleted);
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
            this.notifyIcon1.Text = "ACTA Reader InterfaceS Manager";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // ReaderInterfaceSMainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(694, 407);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.statusBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1000, 600);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(230, 340);
            this.Name = "ReaderInterfaceSMainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ACTA InterfaceS Control";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.ReaderControl_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReaderControlMainForm_FormClosing);
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

        private ArrayList CraeteReaderList()
		{
			ArrayList cbReadersList = new ArrayList();
			List<ReaderTO> raedersOnGate = new List<ReaderTO>();

			try
			{
                // get readers from the service
                ReadersInfos readersInfos = readerInterfaceSServiceClient.GetReadersInfos();
                if (readersInfos.InfosData.Count == 0)
                {
                    throw new System.Exception("Number of readers is 0.");
                }

                foreach (ReaderInfo readerInfo in readersInfos.InfosData)
                {
                    ReaderTO rTO = new ReaderTO();
                    rTO.ReaderID = readerInfo.ReaderID;
                    rTO.Description = readerInfo.Description;
                    rTO.A0GateID = readerInfo.A0GateID;
                    rTO.A1GateID = readerInfo.A1GateID;
                    rTO.DownloadInterval = readerInfo.DownloadInterval;
                    rTO.DownloadStartTime = readerInfo.DownloadStartTime;

                    raedersOnGate.Add(rTO);
                }

				int i = 0;
				int y = 0;

				if (raedersOnGate.Count > 10)
				{
					this.MinimumSize = new System.Drawing.Size((420 * ((raedersOnGate.Count / 10) + 1) + 10), 400);
					this.Size = new System.Drawing.Size((420 * ((raedersOnGate.Count / 10) + 1) + 10), 400);
					this.btnStartStop.Location = 
						new System.Drawing.Point(((420 * (raedersOnGate.Count / 10)) - 35), 300);
				}
				else
				{
					this.MinimumSize = new System.Drawing.Size(430, 60 * raedersOnGate.Count + 10);
					this.Size = new System.Drawing.Size(430, 60 * raedersOnGate.Count + 10);
					this.btnStartStop.Location = new System.Drawing.Point(185, 60 * raedersOnGate.Count - 55);
				}


                foreach (ReaderActivity ra in cbReaderList)
                {
                    if (this.Controls.Contains(ra)) 
                    {
                        this.Controls.Remove(ra);
                    }
                }

				foreach(ReaderTO readerMember in raedersOnGate)
				{
					ReaderActivity acControll = new ReaderActivity(readerMember, 
								rm.GetString("conn", culture), rm.GetString("data", culture));
					acControll.Location = new System.Drawing.Point(15 + (y * 420), 15 + (i++ * 40));
					acControll.Init();
					
					if (i > 10)
					{
						i = 0;
						y ++;
					}
					
					this.Controls.Add(acControll);
					cbReadersList.Add(acControll);
				}

                isReadersListCreated = true;
                if (cbReadersList.Count > 0)
                {
                    this.ClientSize = new System.Drawing.Size(226, 30 * cbReadersList.Count + 100);
                }

				this.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".CraeteReaderList() : " + ex.Message + "\n");
			}

			return cbReadersList;
		}

		private List<ReaderTO> getSelectedReaders()
		{
			List<ReaderTO> selectedReaders = new List<ReaderTO>();

			log.writeLog(DateTime.Now + " Start : getSelectedReaders() \n"); 
			foreach(ReaderActivity cbReader in cbReaderList)
			{
				if (cbReader.IsSelected())
				{
					selectedReaders.Add(cbReader.Reader);

					log.writeLog("\n***** Reader *****\n");
					log.writeLog("ReaderID: " + cbReader.Reader.ReaderID.ToString() + " \n");
					log.writeLog("A0GateID: " + cbReader.Reader.A0GateID.ToString() + " \n");
					log.writeLog("A1GateID: " + cbReader.Reader.A1GateID.ToString() + " \n");
					log.writeLog("DownloadInterval: " + cbReader.Reader.DownloadInterval.ToString() + " \n");
					log.writeLog("DownloadStartTime: " + cbReader.Reader.DownloadStartTime.ToString() + " \n");
					log.writeLog("\n***** Reader *****\n");
				}
			}
			log.writeLog(DateTime.Now + " End : getSelectedReaders() \n"); 

			return selectedReaders;
		}

        /// <summary>
        /// Refreshes UI main form with all readers downloading statuses. It is called once, upon application start, to
        /// to get the statuses from the service that is supposed to be running.
        /// </summary>
		private void RefreshMainFormForAllReaders()
		{
			try
			{
                RefreshNotifyIcon();

                if (serviceStatusAvailable)
                {
                    RefreshStartStopButton();

                    foreach (ReaderActivity ra in cbReaderList)
                    {
                        int readerID = ra.Reader.ReaderID;
                        ReaderStatus readerStatus = readersStatuses.StatusesData[readerID];

                        if (canRefreshReaderActivities)
                        {
                            // from reader action handler
                            RefreshReaderActivityOnReaderAction(ra, readerStatus);

                            // from reader alert handler
                            RefreshReaderActivityOnReaderAlert(ra, readerStatus);
                        }
                    }

                    this.Invalidate();
                }
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".RefreshMainFormForAllReaders() : " + ex.Message + "\n");
			}
		}

        private void RefreshReaderActivityOnReaderAction(ReaderActivity ra, ReaderStatus readerStatus)
        {
            if (readerStatus.IsReaderActionDone)
            {
                if (readerStatus.IsDownloading)
                {
                    ra.Init();
                    if (!readerStatus.Action.Equals(""))
                    {
                        ra.SetInProgress(readerStatus.Action);
                    }
                }
                else
                {
                    if (!readerStatus.NextTimeDownload.Equals(new DateTime()))
                    {
                        ra.SetInProgress(readerStatus.NextTimeDownload.ToString("HH:mm:ss  dd.MM.yy"));
                       // ra.SetOccupation(readerStatus.MemoryOccupation.ToString() + "%");
                    }
                }
            }
        }

        private void RefreshReaderActivityOnReaderAlert(ReaderActivity ra, ReaderStatus readerStatus)
        {
            if (readerStatus.IsReaderAlertDone)
            {
                if (!readerStatus.IsNetExist)
                {
                    ra.SetLanExists(false);
                    return;
                }

                if (!readerStatus.IsDataExist)
                {
                    ra.SetDataExists(false);
                }
            }
        }

        private void RefreshStartStopButton()
        {
            if (isLogDownloadStarted && (this.btnStartStop.Text == rm.GetString("btnStart", culture)))
            {
                this.btnStartStop.Text = rm.GetString("btnStop", culture);
                this.readerControlStartToolStripMenuItem.Enabled = false;
                this.readerControlStopToolStripMenuItem.Enabled = true;
                this.statusBar.Text = "";
                canRefreshReaderActivities = true;

                // Disable CheckBoxes
                foreach (ReaderActivity ra in cbReaderList)
                {
                    ra.Desable();
                    ra.SetDataExists(true);
                    ra.SetLanExists(true);
                }
            }
            else if (!isLogDownloadStarted && (this.btnStartStop.Text == rm.GetString("btnStop", culture)))
            {
                this.btnStartStop.Text = rm.GetString("btnStart", culture);
                this.readerControlStartToolStripMenuItem.Enabled = true;
                this.readerControlStopToolStripMenuItem.Enabled = false;
                canRefreshReaderActivities = false;

                // Enable CheckBoxes
                foreach (ReaderActivity ra in cbReaderList)
                {
                    ra.Enable();
                    ra.Init();
                }
            }
        }

        private void RefreshNotifyIcon()
        {
            if (serviceStatusAvailable)
            {
                if (isReadersListCreated)
                {
                    this.notifyIcon1.Icon = global::ACTAReaderInterfaceSServiceWinClient.Properties.Resources.ReaderControlRunningIcon;
                    this.btnStartStop.Enabled = true;
                    this.statusBar.Text = "";
                }
                else
                {
                    this.notifyIcon1.Icon = global::ACTAReaderInterfaceSServiceWinClient.Properties.Resources.ReaderControlStoppedIcon;
                    this.btnStartStop.Enabled = false;
                    this.statusBar.Text = "Getting readers list from the service failed!";

                    cbReaderList = CraeteReaderList();
                }
            }
            else
            {
                this.notifyIcon1.Icon = global::ACTAReaderInterfaceSServiceWinClient.Properties.Resources.ReaderControlStoppedIcon;
                this.btnStartStop.Enabled = false;
                this.readerControlStartToolStripMenuItem.Enabled = false;
                this.readerControlStopToolStripMenuItem.Enabled = false;
                this.statusBar.Text = "Getting status from the service failed!";

                isReadersListCreated = false;
                this.btnStartStop.Text = rm.GetString("btnStart", culture);
            }
        }

		private void ReaderControl_Load(object sender, System.EventArgs e)
		{
			if (cbReaderList.Count > 0)
			{
				this.ClientSize = new System.Drawing.Size(226, 30 * cbReaderList.Count + 100);
				//this.btnStartStop.Location = new System.Drawing.Point(72, 30 * cbReaderList.Count + 50);
			}
			this.Invalidate();

            this.Hide();
            this.timer1.Enabled = true;
		}

		private void setLanguage()
		{
			try
			{
				this.Text = rm.GetString("ReaderControl", culture);
				this.btnStartStop.Text = rm.GetString("btnStart", culture);
			}
			catch
			{
				MessageBox.Show(rm.GetString("LogProcCantStop", culture));
			}
		}

		private void btnStartStop_Click(object sender, System.EventArgs e)
		{
			try
			{
				log.writeLog(DateTime.Now + "btnStartStop_Click() \n");  

				if (btnStartStop.Text.Trim().Equals(rm.GetString("btnStart", culture)))
				{
					if (this.getSelectedReaders().Count == 0)
					{
						DialogResult procreq = MessageBox.Show(rm.GetString("noSelectedReader", culture), "", MessageBoxButtons.OK);
						if(procreq == DialogResult.OK)
						{
							return;
						}
					}
					// Run Log Reading
					if (!readerInterfaceSServiceClient.ChekPrerequests())
					{
						DialogResult procreq = MessageBox.Show(rm.GetString("SettingsFailed", culture), "", MessageBoxButtons.OK);
						if(procreq == DialogResult.OK)
						{

							return;
						}
					}
					else
					{
                        // set selected readers for the servis
                        ReadersInfos selectedReaders = new ReadersInfos();
                        selectedReaders.InfosData = new BindingList<ReaderInfo>();
                        List<ReaderTO> selected = this.getSelectedReaders();
                        foreach (ReaderTO selectedReader in selected)
                        {
                            ReaderInfo readerInfo = new ReaderInfo();
                            readerInfo.ReaderID = selectedReader.ReaderID;
                            selectedReaders.InfosData.Add(readerInfo);
                        }
                        readerInterfaceSServiceClient.SetSelectedReaders(selectedReaders);

                        readerInterfaceSServiceClient.StartLogDownload();

						log.writeLog("\n" + DateTime.Now + "btnStartStop_Click() : Log Reading Started!  \n"); 
					}
				}
				else if (btnStartStop.Text.Trim().Equals(rm.GetString("btnStop", culture)))
				{
					// Shutdown log reading
                    readerInterfaceSServiceClient.AbortLogDownload();

					log.writeLog("\n " + DateTime.Now + " btnStartStop_Click() : Log Reading Stopped!  \n");  
				}
			}
			catch(Exception ex)
			{
                this.timer1.Enabled = false;
                MessageBox.Show("Start/Stop service command failed. Please restart the service.\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				log.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".btnStart_Click() : " + ex.StackTrace + "\n");
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

        private void ReaderControlMainForm_FormClosing(object sender, FormClosingEventArgs e)
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
                    readerInterfaceSServiceClient.Close();

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
                log.writeLog(DateTime.Now + " " + this.ToString() + ".ReaderControl_Closing(): " + ex.Message + "\n");
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

        private void readerControlStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnStartStop_Click(sender, e);
        }

        private void readerControlStopToolStripMenuItem_Click(object sender, EventArgs e)
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
        /// Gets readers statuses and download status from the service.
        /// </summary>
        private void BackgroundRefreshService()
        {
            serviceStatusAvailable = false;
            try
            {
                isLogDownloadStarted = readerInterfaceSServiceClient.IsLogDownloadStarted();
                readersStatuses = readerInterfaceSServiceClient.GetReadersStatuses();
                serviceStatusAvailable = true;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " " + this.ToString() + ".BackgroundRefreshService(): " + ex.Message + "\n");
            }
        }

        private void ServiceStatusWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundRefreshService();
        }

        private void ServiceStatusWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RefreshMainFormForAllReaders();
        }
    }
}

