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

namespace ACTADataProcessing
{
	/// <summary>
	/// Data processing main form.
	/// </summary>
	public class DataProcessing : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnStartStop;
		private System.ComponentModel.Container components = null;

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
		// Observer client instance
		public NotificationObserverClient observerClient;
		private System.Windows.Forms.Label lblMessage;
		private System.Windows.Forms.Label lblThreadState;
		public const string NOMESSAGE = "---";

        private delegate void InvokeDelegate(string s);

		public DataProcessing()
		{
			InitializeComponent();
			
			// Init Debug
			NotificationController.SetApplicationName(this.Text.Replace(" ", ""));
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			// Set Language 
			//culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            culture = CultureInfo.CreateSpecificCulture(Constants.Lang_en);
			rm = new ResourceManager("UI.Resource",typeof(Employees).Assembly);
			setLanguage();
			this.CenterToScreen();

			// This form need to receive notification 
			// about currently processed files
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DataProcessing));
			this.btnStartStop = new System.Windows.Forms.Button();
			this.lblState = new System.Windows.Forms.Label();
			this.lblStateVal = new System.Windows.Forms.Label();
			this.lblMessage = new System.Windows.Forms.Label();
			this.stsBar = new System.Windows.Forms.StatusBar();
			this.lblThreadState = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnStartStop
			// 
			this.btnStartStop.Location = new System.Drawing.Point(168, 128);
			this.btnStartStop.Name = "btnStartStop";
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
			// DataProcessing
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
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
			this.Name = "DataProcessing";
			this.Text = "ACTA Data Processing";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.DataProcessing_Closing);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			bool newMutexCreated = false;

			try
			{
				string mutexName = "Local\\" + 
					System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
				Mutex mutex = null;
				try
				{
					// Create a new mutex object with a unique name
					mutex = new Mutex(false, mutexName, out newMutexCreated);
				}
				catch(Exception ex)
				{
					MessageBox.Show (ex.Message+"\n\n"+ex.StackTrace+ 
						"\n\n"+"Application Exiting...","Exception thrown");
					Application.Exit ();
				}

				if(newMutexCreated)
				{
					DataProcessing acta = new DataProcessing();
					Application.Run(acta);
				}
				else
				{
					ResourceManager rm;
					CultureInfo culture;
					culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
					rm = new ResourceManager("UI.Resource",typeof(Employees).Assembly);
					MessageBox.Show(rm.GetString("applRunning", culture));
				}

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btnStartStop_Click(object sender, System.EventArgs e)
		{
			// Start automatic Data processing
			DataProcessingManager procManager = DataProcessingManager.GetInstance();

			if (!procManager.IsProcessing)
			{
				if (procManager.chekPrerequests())
				{
					procManager.StartLogProcessing();
					this.lblMessage.Text = NOMESSAGE;
					this.btnStartStop.Text = rm.GetString("btnStop", culture);
					this.lblStateVal.Text = rm.GetString("activated", culture);
				}
				else
				{
					DialogResult procreq = MessageBox.Show(rm.GetString("SettingsFailed", culture), "", MessageBoxButtons.OK);
					if(procreq == DialogResult.OK)
					{
						return;
					}
				}

			}
			else
			{
				lblThreadState.Text = "Thread: processing is being stopped";
				if (procManager.StopLogProcessing())
				{
					MessageBox.Show(rm.GetString("LogProcStopped", culture));
					this.btnStartStop.Text = rm.GetString("btnStart", culture);
					this.lblStateVal.Text = rm.GetString("stopped", culture);
					this.lblMessage.Text = NOMESSAGE;
				}
				else
				{
					MessageBox.Show(rm.GetString("LogProcCantStop", culture));
				}
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

		private void InitializeObserverClient()
		{
			observerClient = new NotificationObserverClient(this.ToString());
			Controller = NotificationController.GetInstance();	
			Controller.AttachToNotifier(observerClient);
			this.observerClient.Notification += new NotificationEventHandler(this.FileProcessingEvent);
			this.observerClient.OnDataProcessingStateChanged += new NotificationEventHandler(this.DataProcessingStateChangedHandler);
		}

		private void FileProcessingEvent(object sender, NotificationEventArgs args)
		{
			try
			{
				if (!args.XMLFileName.Equals(""))
				{
					if (args.isProcessingNow)
					{
                        string lblMessageText = rm.GetString("lblCurrFile", culture)+ " " + Path.GetFileName(args.XMLFileName);
                        this.lblMessage.BeginInvoke(new InvokeDelegate(SetLblMessageText), lblMessageText);
					}
					else
					{
                        this.lblMessage.BeginInvoke(new InvokeDelegate(SetLblMessageText), NOMESSAGE);
					}
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " " + this.ToString() + ".MonitorEvent(): " + ex.Message + "\n");
			}
		}

		private void DataProcessingStateChangedHandler(object sender, NotificationEventArgs args)
		{
			try
			{
				if (!args.message.Equals(""))
				{
					if (!args.message.StartsWith("Thread"))
					{
                        this.lblMessage.BeginInvoke(new InvokeDelegate(SetLblMessageText), args.message);
					}
					else
					{
                        this.lblThreadState.BeginInvoke(new InvokeDelegate(SetLblThreadStateText), args.message);

                        // if thread stopped after max number of retries
						if (args.message.StartsWith("Thread: after"))
						{
                            this.lblStateVal.BeginInvoke(new InvokeDelegate(SetLblStateValText), rm.GetString("stopped", culture));
                            this.btnStartStop.BeginInvoke(new InvokeDelegate(SetBtnStartStopText), rm.GetString("btnStart", culture));
						}
					}
				}
				else
				{
                    this.lblMessage.BeginInvoke(new InvokeDelegate(SetLblMessageText), NOMESSAGE);
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " " + this.ToString() + ".MonitorEvent(): " + ex.Message + "\n");
			}
		}

        private void SetLblMessageText(string text)
        {
            this.lblMessage.Text = text;
        }

        private void SetLblThreadStateText(string text)
        {
            this.lblThreadState.Text = text;
        }

        private void SetLblStateValText(string text)
        {
            this.lblStateVal.Text = text;
        }

        private void SetBtnStartStopText(string text)
        {
            this.btnStartStop.Text = text;
        }

        private void DataProcessing_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			DialogResult procreq = MessageBox.Show(rm.GetString("exitApplication", culture), "", MessageBoxButtons.YesNo);

			if (procreq == DialogResult.Yes)
			{
				Controller.DettachFromNotifier(this.observerClient);
				DataProcessingManager procManager = DataProcessingManager.GetInstance();
				if (procManager.IsProcessing)
				{
					if (procManager.StopLogProcessing())
					{
						MessageBox.Show(rm.GetString("LogProcStopped", culture));
						this.btnStartStop.Text = rm.GetString("btnStart", culture);
					}
					else
					{
						MessageBox.Show(rm.GetString("LogProcCantStop", culture));
					}
				}
				
				log.writeLog(DateTime.Now + " " + this.ToString() + " has been closed! \n");
				return;
			}
			else
			{
				e.Cancel = true;
			}
		}

	}
}
