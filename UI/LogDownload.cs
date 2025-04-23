using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using ReaderManagement;
using Common;
using TransferObjects;
using Util;

namespace UI
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class LogDownload : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnStop;
		DebugLog debug;

		// Check Box list, each check box contains in the tag property 
		// Reader object thet it reapresent.
		ArrayList cbReaderList = new ArrayList();

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public LogDownload()
		{
			InitializeComponent();
			
			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(LogDownload));
			this.btnStart = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(32, 192);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(176, 23);
			this.btnStart.TabIndex = 4;
			this.btnStart.Text = "Start Reading Log from Readers";
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnStop
			// 
			this.btnStop.Location = new System.Drawing.Point(32, 232);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(176, 23);
			this.btnStop.TabIndex = 5;
			this.btnStop.Text = "Stop Reading Log from Readers";
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// LogDownload
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(248, 277);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.btnStart);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximumSize = new System.Drawing.Size(256, 304);
			this.MinimumSize = new System.Drawing.Size(256, 304);
			this.Name = "LogDownload";
			this.ShowInTaskbar = false;
			this.Text = "ACTA Log Reader";
			this.Closed += new System.EventHandler(this.LogReaderManager_Closed);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new LogDownload());
		}

		private void btnStart_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                DownloadManager manager = DownloadManager.GetInstance();

                manager.CreateReaderList(this.getSelectedReaders());
                manager.PushOldReaderLogs();
                manager.StartReading();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".btnStart_Click() : " + ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnStop_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                DownloadManager manager = DownloadManager.GetInstance();
                manager.StopReadingLogs();
            }
            catch (Exception ex) {
                debug.writeLog(DateTime.Now + " Exception in: " +
                        this.ToString() + ".btnStop_Click() : " + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private ArrayList CraeteReaderList()
		{
			string gateString = ConfigurationManager.AppSettings["Gates"];			
			ArrayList cbReadersList = new ArrayList();
			List<ReaderTO> raedersOnGate = new List<ReaderTO>();

			try
			{
				int i = 0;
				raedersOnGate = new Reader().Search(gateString);

				foreach(ReaderTO readerMember in raedersOnGate)
				{
					CheckBox cbReader = new CheckBox();

					cbReader.Text = readerMember.Description;
					cbReader.Location = new System.Drawing.Point(32, 32 + (i++ * 32));
					cbReader.Size = new System.Drawing.Size(176, 23);
					cbReader.Name = Convert.ToString(readerMember.ReaderID);
						
					cbReader.Visible = true;
					cbReader.Enabled = true;
					cbReader.Checked = false;
					cbReader.Tag = readerMember;

					cbReader.Invalidate();
					this.Controls.Add(cbReader);

					cbReadersList.Add(cbReader);
				}

				this.Invalidate();
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".getSelectedReaders() : " + ex.Message);
			}

			return cbReadersList;
		}


		private List<ReaderTO> getSelectedReaders()
		{
			List<ReaderTO> selectedReaders = new List<ReaderTO>();
			
			foreach(CheckBox cbReader in cbReaderList)
			{
				if (cbReader.Checked)
				{
					selectedReaders.Add((ReaderTO) cbReader.Tag);
				}
			}

			return selectedReaders;
		}

		private void LogReaderManager_Closed(object sender, System.EventArgs e)
        {
            try
            {
                DownloadManager manager = DownloadManager.GetInstance();
                manager.StopReadingLogs();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                        this.ToString() + ".LogReaderManager_Closed() : " + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}
	}
}
