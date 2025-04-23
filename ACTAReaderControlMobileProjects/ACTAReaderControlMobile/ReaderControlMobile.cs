using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Resources;
using System.Globalization;
using System.Threading;

using ReaderMobileManagement;
using Common;
using Util;
using TransferObjects;

namespace ACTAReaderControlMobile
{
    public partial class ReaderControlMobile : Form
    {
        // Debug
		DebugLog debug;

		// Observer initialization
		// Receive message that log downloading has started, and change 
		// Cursor or display a message.
		// Controller instance
		//public NotificationController Controller;
		// Observer client instance
		//public NotificationObserverClient observerClient;
        
		public ReaderControlMobile()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			// Debug
			NotificationController.SetApplicationName(this.Text.Replace(" ", ""));
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);
                        
			//InitializeObserverClient();
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new ReaderControlMobile());
		}

        //private void InitializeObserverClient()
        //{
        //    observerClient = new NotificationObserverClient(this.ToString());
        //    Controller = NotificationController.GetInstance();	
        //    Controller.AttachToNotifier(observerClient);
        //    this.observerClient.Notification += new NotificationEventHandler(this.ReaderAction);
        //    this.observerClient.ReaderNotification += new NotificationEventHandler(this.ReaderAlert);
        //    this.observerClient.PingNotification += new NotificationEventHandler(this.PingAlert);
        //}

        //private void ReaderAction(Object sender, NotificationEventArgs arg)
        //{
        //    try
        //    {
        //        foreach(ReaderActivity ra in cbReaderList)
        //        {
        //            if(ra.Reader.ReaderID == arg.reader.ReaderID)
        //            {
        //                if (arg.isDownloading)
        //                {
        //                    ra.Init();
        //                    if (!arg.readerAction.Equals(""))
        //                    {
        //                        ra.SetInProgress(arg.readerAction);
        //                    }
        //                }
        //                else
        //                {
        //                    if (!arg.nextTimeDownload.Equals(new DateTime()))
        //                    {
        //                        ra.SetInProgress(arg.nextTimeDownload.ToString("HH:mm:ss  dd.MM.yy")); 
        //                        ra.SetOccupation(arg.reader.MemoryOccupation.ToString() + "%");
        //                    }
        //                }

        //                break;
        //            }
        //        }

        //        this.Invalidate();
        //    }
        //    catch(Exception ex)
        //    {
        //        debug.writeLog(DateTime.Now + " Exception in: " + 
        //            this.ToString() + ".DownloadStartedEvent() : " + ex.Message + "\n");
        //    }
        //}

        //private void ReaderAlert(Object sender, NotificationEventArgs arg)
        //{
        //    foreach(ReaderActivity ra in cbReaderList)
        //    {
        //        if(ra.Reader.ReaderID == arg.reader.ReaderID)
        //        {
        //            if (!arg.isNetExist)
        //            {
        //                ra.SetLanExists(false);
        //                break;

        //            }

        //            if (!arg.isDataExist)
        //            {
        //                ra.SetDataExists(false);
        //            }

        //            break;
        //        }
        //    }
        //}

        //private void PingAlert(Object sender, NotificationEventArgs arg)
        //{
        //    foreach (ReaderActivity ra in cbReaderList)
        //    {
        //        if (ra.Reader.ReaderID == arg.reader.ReaderID)
        //        {
                   
        //                ra.SetPingAlert(arg.isNetExist);
        //                break;

        //        }
        //    }
        //}
		

		private void ReaderControlMobile_Load(object sender, System.EventArgs e)
		{
            try
            {
                ReaderEventMobileController eventController = ReaderEventMobileController.GetInstance();
                eventController.StartListening();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
		}

		private void btnStartStop_Click(object sender, System.EventArgs e)
		{
			try
			{
                DownloadMobileManager manager = DownloadMobileManager.GetInstance();
				debug.writeLog(DateTime.Now + "btnStartStop_Click() \n");  

				if (btnStartStop.Text.Trim().Equals("Start"))
				{
					// Run Log Reading
					if (!manager.chekPrerequests())
					{
						DialogResult procreq = MessageBox.Show("Settings failed", "", MessageBoxButtons.OK);
						if(procreq == DialogResult.OK)
						{
							return;
						}
					}
					else
					{
						ReaderEventMobileController controller = ReaderEventMobileController.GetInstance();
						manager.CreateDownloadObject();
						manager.PushOldReaderLogs();
						manager.StartReading();
						this.btnStartStop.Text = "Stop";
						
						debug.writeLog("\n" + DateTime.Now + "btnStartStop_Click() : Log Reading Started!  \n"); 
					}

					this.Invalidate();
				}
				else if (btnStartStop.Text.Trim().Equals("Stop"))
				{
					// Shutdown log reading
					manager.AbortReading();
					this.btnStartStop.Text = "Start";

					debug.writeLog("\n " + DateTime.Now + " btnStartStop_Click() : Log Reading Stopped!  \n");  
                    
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

		private void ReaderControlMobile_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			DialogResult procreq = MessageBox.Show("Are you sure you want to exit application?", "", MessageBoxButtons.YesNo);

			if (procreq == DialogResult.Yes)
			{
                DownloadMobileManager manager = DownloadMobileManager.GetInstance();
				manager.StopReadingLogs();
				//Controller.DettachFromNotifier(this.observerClient);

				ReaderEventMobileController control = ReaderEventMobileController.GetInstance();
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