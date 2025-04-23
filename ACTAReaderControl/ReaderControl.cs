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

using ReaderManagement;
using Common;
using Util;
using UI;
using TransferObjects;

namespace ACTAReaderControl
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class ReaderControl : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		// Check Box list, each check box contains in the tag property 
		// Reader's object.
		ArrayList cbReaderList = new ArrayList();

		// Debug
		DebugLog debug;

		// Observer initialization
		// Receive message that log downloading has started, and change 
		// Cursor or display a message.
		// Controller instance
		public NotificationController Controller;
		// Observer client instance
		public NotificationObserverClient observerClient;

		// Langage
		ResourceManager rm;
		private System.Windows.Forms.Button btnStartStop;
		private System.Windows.Forms.StatusBar statusBar;
		private CultureInfo culture;


		public ReaderControl()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			// Debug
			NotificationController.SetApplicationName(this.Text.Replace(" ", ""));
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);

			// Language
			//culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            culture = CultureInfo.CreateSpecificCulture(Constants.Lang_en);
			rm = new ResourceManager("UI.Resource",typeof(Employees).Assembly);
			setLanguage();

            cbReaderList = CraeteReaderList();
			InitializeObserverClient();
            
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReaderControl));
            this.btnStartStop = new System.Windows.Forms.Button();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.SuspendLayout();
            // 
            // btnStartStop
            // 
            this.btnStartStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStartStop.Location = new System.Drawing.Point(262, 344);
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
            this.statusBar.Location = new System.Drawing.Point(0, 391);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(694, 24);
            this.statusBar.TabIndex = 3;
            // 
            // ReaderControl
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(694, 415);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.statusBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ReaderControl";
            this.Text = "ACTA Reader Control";
            this.Load += new System.EventHandler(this.ReaderControl_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.ReaderControl_Closing);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new ReaderControl());
		}

		private ArrayList CraeteReaderList()
		{
			string gateString = ConfigurationManager.AppSettings["Gates"];
			Reader reader = new Reader();
			ArrayList cbReadersList = new ArrayList();
			List<ReaderTO> raedersOnGate = new List<ReaderTO>();

			try
			{
				int i = 0;
				int y = 0;

				raedersOnGate = reader.Search(gateString);

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

				this.Invalidate();
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".CraeteReaderList() : " + ex.Message + "\n");
			}

			return cbReadersList;
		}

		/*
		private void ACTAReaderControl_Closed(object sender, System.EventArgs e)
		{
			try
			{
				
				DialogResult procreq = MessageBox.Show(rm.GetString("exitApplication", culture), "", MessageBoxButtons.YesNo);

				if (procreq == DialogResult.Yes)
				{
				
					DownloadManager manager = DownloadManager.GetInstance();
					manager.StopReadingLogs();
					Controller.DettachFromNotifier(this.observerClient);

					ReaderEventController control = ReaderEventController.GetInstance();
					control.StopListening();
					return;
				}
				else
				{
					((CancelEventArgs) e).Cancel = true;
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".ACTAReaderControl_Closed() : " + ex.Message + "\n");
			}
		}
		*/

		private List<ReaderTO> getSelectedReaders()
		{
			List<ReaderTO> selectedReaders = new List<ReaderTO>();

			debug.writeLog(DateTime.Now + " Start : getSelectedReaders() \n"); 
			foreach(ReaderActivity cbReader in cbReaderList)
			{
				if (cbReader.IsSelected())
				{
					selectedReaders.Add(cbReader.Reader);

					debug.writeLog("\n***** Reader *****\n");
					debug.writeLog("ReaderID: " + cbReader.Reader.ReaderID.ToString() + " \n");
					debug.writeLog("A0GateID: " + cbReader.Reader.A0GateID.ToString() + " \n");
					debug.writeLog("A1GateID: " + cbReader.Reader.A1GateID.ToString() + " \n");
					debug.writeLog("DownloadInterval: " + cbReader.Reader.DownloadInterval.ToString() + " \n");
					debug.writeLog("DownloadStartTime: " + cbReader.Reader.DownloadStartTime.ToString() + " \n");
					debug.writeLog("\n***** Reader *****\n");
				}
			}
			debug.writeLog(DateTime.Now + " End : getSelectedReaders() \n"); 

			return selectedReaders;
		}

		private void InitializeObserverClient()
		{
			observerClient = new NotificationObserverClient(this.ToString());
			Controller = NotificationController.GetInstance();	
			Controller.AttachToNotifier(observerClient);
			this.observerClient.Notification += new NotificationEventHandler(this.ReaderAction);
			this.observerClient.ReaderNotification += new NotificationEventHandler(this.ReaderAlert);
            this.observerClient.PingNotification += new NotificationEventHandler(this.PingAlert);
		}

		private void ReaderAction(Object sender, NotificationEventArgs arg)
		{
			try
			{
				foreach(ReaderActivity ra in cbReaderList)
				{
					if(ra.Reader.ReaderID == arg.reader.ReaderID)
					{
						if (arg.isDownloading)
						{
							ra.Init();
							if (!arg.readerAction.Equals(""))
							{
								ra.SetInProgress(arg.readerAction);
							}
						}
						else
						{
							if (!arg.nextTimeDownload.Equals(new DateTime()))
							{
								ra.SetInProgress(arg.nextTimeDownload.ToString("HH:mm:ss  dd.MM.yy")); 
								ra.SetOccupation(arg.reader.MemoryOccupation.ToString() + "%");
							}
						}

						break;
					}
				}

				this.Invalidate();
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".DownloadStartedEvent() : " + ex.Message + "\n");
			}
		}

		private void ReaderAlert(Object sender, NotificationEventArgs arg)
		{
			foreach(ReaderActivity ra in cbReaderList)
			{
				if(ra.Reader.ReaderID == arg.reader.ReaderID)
				{
					if (!arg.isNetExist)
					{
						ra.SetLanExists(false);
						break;

					}

					if (!arg.isDataExist)
					{
						ra.SetDataExists(false);
					}

					break;
				}
			}
		}

        private void PingAlert(Object sender, NotificationEventArgs arg)
        {
            foreach (ReaderActivity ra in cbReaderList)
            {
                if (ra.Reader.ReaderID == arg.reader.ReaderID)
                {
                   
                        ra.SetPingAlert(arg.isNetExist);
                        break;

                }
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

			ReaderEventController eventController = ReaderEventController.GetInstance();
			eventController.StartListening();
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
				DownloadManager manager = DownloadManager.GetInstance();
				debug.writeLog(DateTime.Now + "btnStartStop_Click() \n");  

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
					if (!manager.chekPrerequests())
					{
						DialogResult procreq = MessageBox.Show(rm.GetString("SettingsFailed", culture), "", MessageBoxButtons.OK);
						if(procreq == DialogResult.OK)
						{

							return;
						}
					}
					else
					{
						ReaderEventController controller = ReaderEventController.GetInstance();
						manager.CreateReaderList(this.getSelectedReaders());
						manager.PushOldReaderLogs();
						manager.StartReading();
						this.btnStartStop.Text = rm.GetString("btnStop", culture);
						this.statusBar.Text = "";

						debug.writeLog("\n" + DateTime.Now + "btnStartStop_Click() : Log Reading Started!  \n"); 
					}

					// Disable CheckBoxes
					foreach(ReaderActivity cb in cbReaderList)
					{
						cb.Desable();
                        cb.SetDataExists(true);
						cb.SetLanExists(true);
					}

					this.Invalidate();
				}
				else if (btnStartStop.Text.Trim().Equals(rm.GetString("btnStop", culture)))
				{
					// Shutdown log reading
					manager.AbortReading();
					this.btnStartStop.Text = rm.GetString("btnStart", culture);
					

					debug.writeLog("\n " + DateTime.Now + " btnStartStop_Click() : Log Reading Stopped!  \n");  

					// Disable CheckBoxes
					foreach(ReaderActivity ra in cbReaderList)
					{
						ra.Enable();
						ra.Init();
					}

					this.Cursor = Cursors.Default;

					this.Invalidate();
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".btnStart_Click() : " + ex.StackTrace + "\n");
			}
		}

		private void ReaderControl_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			DialogResult procreq = MessageBox.Show(rm.GetString("exitApplication", culture), "", MessageBoxButtons.YesNo);

			if (procreq == DialogResult.Yes)
			{
				
				DownloadManager manager = DownloadManager.GetInstance();
				manager.StopReadingLogs();
				Controller.DettachFromNotifier(this.observerClient);

				ReaderEventController control = ReaderEventController.GetInstance();
				control.StopListening();
				debug.writeLog(DateTime.Now + " " + this.ToString() + " has been closed! \n");
				return;
			}
			else
			{
				e.Cancel = true;
			}
		}
	}
}

