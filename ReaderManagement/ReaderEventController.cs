using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

using Common;
using Util;
using TransferObjects;

namespace ReaderManagement
{
	/// <summary>
	/// Sinchronize dowload log data from reader device and 
	/// real time monitorning process.
	/// </summary>
	// TODO: Move class to ReaderManagement namespace after testing and change Win Form populating ...
	public class ReaderEventController
	{
		protected static ReaderEventController instance;
		private ArrayList readerThreads = new ArrayList();
		private List<EmployeeTO> employees = new List<EmployeeTO>();
		private List<TagTO> tags = new List<TagTO>();		
		private List<ReaderTO> readers = new List<ReaderTO>();
		private List<LocationTO> locations = new List<LocationTO>();
		private Thread downloadThreadKiller; 
		public bool doListening = false;
		public bool NeedRestart = false;

		// Controller instance
		public NotificationController Controller;
		
		// Observer client instance
		public NotificationObserverClient observerClient;

		// Debug
		DebugLog debug;

		public List<EmployeeTO> Employees
		{
			get { return employees; }
		}

		public List<TagTO> Tags
		{
			get { return tags; }
		}

		private ReaderEventController()
		{
			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);

			InitializeObserverClient();
		}

		public static ReaderEventController GetInstance()
		{
			try
			{
				if (instance == null)
				{
					instance = new ReaderEventController();
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}

			return instance;
		}


		public void SetData(List<EmployeeTO> employeeList, List<TagTO> tagList, 
		    List<ReaderTO> readersList, List<LocationTO> locationsList)
		{
			try
			{
				employees = employeeList;
				tags = tagList;
				readers = readersList;
				locations = locationsList;
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".StartMonitor() : " + ex.StackTrace + "\n\n");  
				throw ex;
			}
		}								  

		public int StartMonitor()
		{
			int threadStarted = 0;

			try
			{
				for(int j=0; j<readerThreads.Count; j++)
				{
					((MonitorThread) readerThreads[j]).StartComPortReading();	
					threadStarted ++;
				}
				
				System.Console.WriteLine("Monitor started! Num. Of readers: " + threadStarted.ToString());
				debug.writeLog("Monitor started! Num. Of readers: " + threadStarted.ToString()+ "\n\n");  
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".StartMonitor() : " + ex.StackTrace + "\n\n");  
				throw ex;
			}

			return threadStarted;
		}

		public bool StartMonitor(int readerID)
		{
			bool isStarted = false;

			try
			{
				for(int j=0; j<readers.Count; j++)
				{
					MonitorThread instance = (MonitorThread) readerThreads[j];

					if (Convert.ToInt32(instance.ReaderID).Equals(readerID))
					{
						instance.StartComPortReading();
						isStarted = true;

						System.Console.WriteLine("Monitor started! ReaderID: " + instance.ReaderID );
						debug.writeLog("Monitor started! ReaderID: " + instance.ReaderID + "\n\n");

						return true;
					}
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".StartMonitor() : " + ex.StackTrace + "\n\n");  
				throw ex;
			}

			return isStarted;
		}

		public int StopMonitor()
		{
			int threadStopped = 0;

			try
			{
				for(int j=0; j<readers.Count; j++)
				{
					((MonitorThread) readerThreads[j]).StopComPortReading();
					threadStopped ++;
				}

				System.Console.WriteLine("Monitor stopped! Num. Of readers: " + threadStopped.ToString());
				debug.writeLog("Monitor stopped! Num. Of readers: " + threadStopped.ToString() + "\n\n");
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".StopMonitor() : Message: "+ex.Message+"/n StackTrace:" + ex.StackTrace);  
				throw ex;
			}

			return threadStopped;
		}


		public bool StopMonitor(int readerID)
		{
			bool isStopped = false;

			try
			{
				
				for(int j=0; j<readers.Count; j++)
				{
					MonitorThread instance = (MonitorThread) readerThreads[j];

					if (Convert.ToInt32(instance.ReaderID).Equals(readerID))
					{
						instance.StopComPortReading();
						isStopped = true;

						System.Console.WriteLine("Monitor stopped! ReaderID: " + instance.ReaderID );
						debug.writeLog("Monitor stopped! ReaderID: " + instance.ReaderID + "\n\n");

						return true;
					}
				}

				
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".StopMonitor() : " + ex.StackTrace + "\n\n");  
			}

			return isStopped;
		}

		public int SetReaders()
		{
			try
			{
				for(int j=0; j<readers.Count; j++)
				{
					readerThreads.Add(new MonitorThread(readers[j], j+1));	
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".SetReaders() : Message: "+ex.Message+"/n StackTrace:" + ex.StackTrace);  
				throw ex;
			}

			return readers.Count;
		}


		/// <summary>
		/// Check wather rader monitor is active for that reader
		/// </summary>
		/// <param name="raderID"></param>
		/// <returns></returns>
		public bool isActive(int readerID)
		{
			bool isActive = false;

			try
			{
				foreach(MonitorThread rt in readerThreads)
				{
					if ((rt.ReaderID.Equals(readerID.ToString())) && (rt.IsActive))
					{
						return true;
					}
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".isActive() : Message: "+ex.Message+"/n StackTrace:" + ex.StackTrace);  
				throw ex;
			}

			return isActive;
		}

		/// <summary>
		/// Check if Monitor was started by checkig if 
		/// any thread exists for any reader.
		/// </summary>
		/// <returns></returns>
		public bool isStarted()
		{
			bool isStarted = false;
			
			if(readerThreads.Count > 0)
			{
				isStarted = true;
			}
			
			return isStarted;
		}


		private void InitializeObserverClient()
		{
			observerClient = new NotificationObserverClient(this.ToString());
			Controller = NotificationController.GetInstance();	
			//Controller.AttachToNotifier(observerClient);
			//this.observerClient.Notification += new NotificationEventHandler(this.DownloadStarted);
		}

		public void SendMonitorNotification(ReaderTO currentReader, int currentAntennaNum, UInt64 currentTagID)
		{
			try
			{
				NotificationController.MonitorEventHappened(currentReader, currentAntennaNum, currentTagID);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".NotifyAll() : Message: "+ex.Message+"/n StackTrace:" + ex.StackTrace);  
				throw ex;
			}
		}

		public void RestartReading(DownloadManager mgrInstance)
		{
			try
			{
				mgrInstance.StopReadingLogs();
				mgrInstance = null;
				GC.Collect();
				DownloadManager downloadMgr = DownloadManager.GetInstance();
				//DownloadManager downloadMgr = new DownloadManager();
				downloadMgr.StartReading();
				downloadMgr.IsRestarted = true;
				System.Console.WriteLine("Thread Restarted at:  " + DateTime.Now.ToString("hh:mm:ss"));
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".ReastartReading() : Message: "+ex.Message+"/n StackTrace:" + ex.StackTrace);  
				throw ex;
			}

		}

		public void StartListening()
		{
			try
			{
				downloadThreadKiller = new Thread(new ThreadStart(Kill));
				this.doListening = true;
				downloadThreadKiller.Start();
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + 
					".StartListening() : Message: "+ex.Message+"/n StackTrace:" + ex.StackTrace);  
				throw ex;
			}
		}

		public void StopListening()
		{
			this.doListening = false;
		}

		public void Kill()
		{
			try
			{
				while(doListening)
				{
					if (NeedRestart)
					{
						debug.writeLog(DateTime.Now + " Try Restarting! \n");
						DownloadManager mgr = DownloadManager.GetInstance();
						mgr.AbortReading();

						Thread.Sleep(2000);

						NeedRestart = false;
						mgr.StartReading();
						debug.writeLog("\n" + DateTime.Now + " Restarted! \n");
					}
					else
					{
						Thread.Sleep(2000);
					}
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + 
					".Kill() : Message: "+ex.Message+"/n StackTrace:" + ex.StackTrace);  
				throw ex;
			}
		}

		/// <summary>
		/// Notify all objects that log's download has started (or finished)
		/// </summary>
		/// <param name="isDownloadingNow">true if download started, false if it finished</param>
		/// <param name="nextDownloadingAt">next time download will occur</param>
		public void ReaderdAction(ReaderTO currentReader, bool isStarted, bool isSucceeded, 
			bool isPingSucceeded, DateTime nextDownloadingAt)
		{
			try
			{
				// Stop / Start monitor if it is Active 
				if (this.isStarted())
				{
					if (this.isActive(currentReader.ReaderID))
					{
						this.StopMonitor(currentReader.ReaderID);
					}
					else
					{
						this.StartMonitor(currentReader.ReaderID);
					}
				}

				// Send notification about downloading
				NotificationController notifController = NotificationController.GetInstance();
				notifController.ReaderAction(currentReader,isStarted,isSucceeded, 
					isPingSucceeded, nextDownloadingAt);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + 
					".DownloadStarted() : Message: "+ex.Message+"/n StackTrace:" + ex.StackTrace);  
				throw ex;
			}
		}

		/// <summary>
		/// Send event notification when download or upload data 
		/// has started or stoped.
		/// </summary>
		/// <param name="readerDownload">DownloadLog instance that contain current reader</param>
		/// <param name="isStarted">true if download has started, false if it is stopped</param>
		public void ReaderDataExchange(ReaderTO reader, bool isStarted, DateTime nextReading, string actionName)
		{
			try
			{
				NotificationController notifController = NotificationController.GetInstance();
				notifController.ReaderDataExchange(reader, isStarted, nextReading, actionName);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".ReaderDataExchange() : " + ex.StackTrace + "\n");
			}

		}

		/// <summary>
		/// Send event notification when connection to the reader device has failed 
		/// </summary>
		/// <param name="readerDownload">DownloadLog instance that contain current reader</param>
		/// <param name="isNetFailed">true if ping to the device failed</param>
		/// <param name="isDataTranFailed">true if data transfer for some reason failed</param>
		public void ReaderFailed(ReaderTO reader, bool isNetExist, bool isDataTranExist)
		{
			try
			{
				NotificationController notifController = NotificationController.GetInstance();
				notifController.ReaderFailed(reader, isNetExist, isDataTranExist);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".ReaderFailed() : " + ex.StackTrace + "\n");
			}

		}

        public void PingFailed(ReaderTO reader, bool isNetExist)
        {
            try
            {
                NotificationController notifController = NotificationController.GetInstance();
                notifController.PingFailed(reader, isNetExist);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".ReaderFailed() : " + ex.StackTrace + "\n");
            }

        }

	}
}
