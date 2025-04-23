using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Common;
using TransferObjects;
using Util;

namespace ReaderMobileManagement
{
	/// <summary>
	/// Manage logs from reader.
	/// </summary>
	public class DownloadMobileManager : IDisposable
	{
		private static DownloadMobileManager managerInstance;

        object ACTAConnection = null;

		private Thread downloadManagerThread;
		
		private static DownloadLogMobile _logMobileDownload;
		private string _logFileDirectory = "";
		private volatile bool isReading = false;
		private int MAXATTEMPTSNUM = -1;
		private int ATTEMPTINTERVAL = -1;
		private int DOWNLOADSLEEPINTERVAL = -1;
		private ReaderEventMobileController eventController;
		public bool IsRestarted = false;
        
		// Debug
		DebugLog debug;
        
        public DownloadLogMobile LogMobileDownload
        {
            get { return _logMobileDownload; }
        }

		public string LogFileDirectory
		{
			get { return _logFileDirectory; }
			set { _logFileDirectory = value; }
		}

		protected DownloadMobileManager()
		{
			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);

			// Set default vaules			
            MAXATTEMPTSNUM = Convert.ToInt32(Constants.MAXATTEMPTSNUM.ToString());			
            ATTEMPTINTERVAL = Convert.ToInt32(Constants.ATTEMPTINTERVAL.ToString());
            DOWNLOADSLEEPINTERVAL = Convert.ToInt32(Constants.DOWNLOADSLEEPINTERVAL.ToString());

			downloadManagerThread = new Thread(new ThreadStart(Reading));
			isReading = false;
			eventController = ReaderEventMobileController.GetInstance();
        }
		
		public static DownloadMobileManager GetInstance()
		{
			try
			{
				if (managerInstance == null)
				{
					managerInstance = new DownloadMobileManager();
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}

			return managerInstance;
		}

		private void Reading()
		{
            bool downloadSucceeded = false;
            DateTime downloadTime = DateTime.Now;
            DateTime uploadTime = DateTime.Now;
            
            bool isFirstReading = true;

            try
            {
                while (isReading)
                {
                    if (ConnectToDataBase())
                    {
                        bool readersSet = false;
                        Dictionary<string, int> readersIMEIDict = new Dictionary<string, int>();
                        Dictionary<int, List<CameraTO>> readersCameraDict = new Dictionary<int, List<CameraTO>>();

                        try
                        {
                            // get readers dictionary
                            List<ReaderTO> readersList = new Reader(ACTAConnection).Search();

                            foreach (ReaderTO rTO in readersList)
                            {
                                // skip not mobile readers
                                if (rTO.ConnectionType != Constants.ConnTypeGSM)
                                    continue;

                                // create IMEI dictionary
                                if (!readersIMEIDict.ContainsKey(rTO.ConnectionAddress.Trim()))
                                    readersIMEIDict.Add(rTO.ConnectionAddress.Trim(), rTO.ReaderID);

                                // search cameras
                                ArrayList cameraList = new Camera(ACTAConnection).SearchForReaders(rTO.ReaderID.ToString().Trim(), "");

                                if (!readersCameraDict.ContainsKey(rTO.ReaderID))
                                    readersCameraDict.Add(rTO.ReaderID, new List<CameraTO>());

                                foreach (Camera cam in cameraList)
                                {
                                    readersCameraDict[rTO.ReaderID].Add(new CameraTO(cam.CameraID, cam.ConnAddress, cam.Description, cam.Type));
                                }
                            }

                            CloseConnection();
                            readersSet = true;
                        }
                        catch (Exception ex)
                        {
                            debug.writeLog(DateTime.Now + this.ToString() + "Reading() - getting readers and cameras exception: " + ex.Message + "\n");
                            CloseConnection();
                            readersSet = false;
                        }

                        if (LogMobileDownload != null && readersSet)
                        {
                            // Download logs from readers
                            if (IsDownloadTimeNow(LogMobileDownload, isFirstReading))
                            {
                                isFirstReading = false;

                                downloadSucceeded = this.DownloadLog(LogMobileDownload, readersIMEIDict, readersCameraDict);

                                if (downloadSucceeded)
                                {
                                    debug.writeBenchmarkLog(DateTime.Now + " Reading(): DownloadLog succeeded\n");
                                    // Next Download interval
                                    LogMobileDownload.GetNextReading();
                                    Thread.Sleep(1000);
                                }
                                // Log Reading failed
                                else
                                {
                                    debug.writeBenchmarkLog(DateTime.Now + " Reading(): DownloadLog failed\n");
                                    LogMobileDownload.NextTimeReading = LogMobileDownload.NextTimeReading.AddSeconds(ATTEMPTINTERVAL);
                                    Thread.Sleep(1000);

                                    // Max num of attempts reached
                                    if (LogMobileDownload.NumOfAttempts >= MAXATTEMPTSNUM)
                                    {
                                        LogMobileDownload.GetNextReading();
                                        debug.writeBenchmarkLog(DateTime.Now + "Reading ftp logs failed! Attempt number: " + LogMobileDownload.NumOfAttempts + " Reached the maximum number of Attempts! \n");
                                        LogMobileDownload.NumOfAttempts = 0;
                                    }
                                    else
                                    {
                                        LogMobileDownload.NextTimeReading = LogMobileDownload.NextTimeReading.AddSeconds(ATTEMPTINTERVAL);
                                        debug.writeBenchmarkLog(DateTime.Now + "Reading ftp logs failed! Attempt number: " + LogMobileDownload.NumOfAttempts + " Next attempt for: " + ATTEMPTINTERVAL + " \n");
                                        LogMobileDownload.NumOfAttempts++;
                                    }
                                }
                            }
                        }
                    }

                    Thread.Sleep(DOWNLOADSLEEPINTERVAL * 1000);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(ex);
                AbortReading();
            }
		}

		public bool StopReadingLogs()
		{
			try
			{
				if (downloadManagerThread.IsAlive)
				{
					this.isReading = false;
				}

				System.Console.WriteLine("Thread Stopped at:  " + DateTime.Now.ToString("hh:mm:ss"));
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".StopReadingLogs() : Message: "+ex.Message+"/n StackTrace:" + ex.StackTrace); 
			}

			return false;
		}

		/// <summary>
		/// Data dowloaded from readers must be saved as an XML file to remote 
		/// (mapped) directory or a local directory if connection to remote 
		/// server don't exists.
		/// </summary>
		/// <returns>path to reader's logs in XML file format.</returns>
		public string getLogFilePath()
		{			
			string filePath = "";
			string remotePath = Constants.unprocessedMobile; 
			string localPath = Constants.LocalReaderLogDir; 

			try
			{
				Stream writer = File.Open(remotePath + "Test", FileMode.Create);
				filePath = remotePath;
				writer.Close();
				File.Delete(remotePath + "Test");
			}
			catch(IOException)
			{
				filePath = localPath;
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + 
					".getLogFilePath() : Message: "+ex.Message+"/n StackTrace:" + ex.StackTrace);  
				throw ex;
			}

			return filePath;
		}

		/// <summary>
		/// Move reader's log files to remote directory.
		/// </summary>
		/// <returns></returns>
		public bool PushOldReaderLogs()
		{
			bool areSaved = false;

			try
			{
				this.LogFileDirectory = getLogFilePath();

				string localLogDir = Constants.LocalReaderLogDir;

				if (LogFileDirectory.Equals(Constants.unprocessedMobile))
				{
					if(Directory.GetFiles(localLogDir).Length > 0)
					{
						foreach(String  path in Directory.GetFiles(localLogDir))
						{
							File.Copy(path, Constants.unprocessed + Path.GetFileName(path));
							File.Delete(path);
						}
					}
				}
			}
			catch(IOException ex)
			{
				areSaved = false;
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".PushAllReaderLogs() : Message: "+ex.Message+"/n StackTrace:" + ex.StackTrace);  
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + ".PushAllReaderLogs() : Message: "+ex.Message+"/n StackTrace:" + ex.StackTrace);  
				throw ex;
			}

			return areSaved;
		}

		public bool StartReading()
		{
			try
			{
				this.isReading = true;

				if (downloadManagerThread == null)
				{
					downloadManagerThread = new Thread(new ThreadStart(Reading));
					downloadManagerThread.Start();
				}
				else if (!downloadManagerThread.IsAlive)
				{
					downloadManagerThread = new Thread(new ThreadStart(Reading));
					downloadManagerThread.Start();
				}
				else if (downloadManagerThread.ThreadState == ThreadState.Suspended)
				{
                    debug.writeLog("Thread was suspended, it should be resumed!");
					//downloadManagerThread.Resume();
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + 
					".StartReading() : Message: "+ex.Message+"/n StackTrace:" + ex.StackTrace);  
				throw ex;
			}

			return this.isReading;
		}

		/// <summary>
		/// Create DownloadLog object
		/// </summary>
		public void CreateDownloadObject()
        {
            this.LogFileDirectory = getLogFilePath();

            try
            {
                if (!this.LogFileDirectory.Equals(""))
                {
                    _logMobileDownload = new DownloadLogMobile(this.LogFileDirectory);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".CreateDownloadObject() : Message: " + ex.Message + "/n StackTrace:" + ex.StackTrace);
            }
        }

		public bool AbortReading()
		{
			bool isAborted = false;
			try
			{
				try
				{
					downloadManagerThread.Abort();
				}
				catch
				{}
				downloadManagerThread = null;
				isAborted = true;
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".Reset() : Message: "+ex.Message+"/n StackTrace:" + ex.StackTrace);
			}

			return isAborted;
		}

		// Implement IDisposable.
		// Do not make this method virtual.
		// A derived class should not be able to override this method.
		public void Dispose()
		{
			// This object will be cleaned up by the Dispose method.
			// Therefore, you should call GC.SupressFinalize to
			// take this object off the finalization queue 
			// and prevent finalization code for this object
			// from executing a second time.
			this.downloadManagerThread = null;
			GC.Collect();
		}

		public bool chekPrerequests()
		{
			bool isOk = false;
			this.LogFileDirectory = getLogFilePath();

			if (!this.LogFileDirectory.Equals(""))
			{
				isOk = true;
			}

			return isOk;
		}

        private bool DownloadLog(DownloadLogMobile readLog, Dictionary<string, int> readersIMEIDict, Dictionary<int, List<CameraTO>> readersCamerasDict)
		{
			bool succeeded = false;

			try
			{
				if (readLog.GetLogFiles(readersIMEIDict, readersCamerasDict))
				{
					succeeded = true;
				}

			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".DownloadLog() : " + ex.StackTrace + "\n");
                succeeded = false;
			}

			return succeeded;
		}

        private bool IsDownloadTimeNow(DownloadLogMobile log, bool isFirstReading)
		{
			bool isTime = false;
            DateTime downloadTime = log.NextTimeReading;

			try
			{
				if (downloadTime < DateTime.Now)				
                    isTime = true;

                if (isFirstReading)                
                    isTime = true;                				
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".IsDownloadTimeNow() : " + ex.StackTrace + "\n");
			}

			return isTime;
		}

        private bool ConnectToDataBase()
        {
            bool succ = false;
            try
            {
                ACTAConnection = DBConnectionManager.Instance.MakeNewDBConnection();
                if (ACTAConnection == null)
                {
                    debug.writeLog(DateTime.Now + " " + this.ToString() + ".ConnectToDataBase() Make new ACTA DB connection faild!");
                    return false;
                }

                succ = true;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " " + this.ToString() + ".ConnectToDataBase() exception: " + ex.Message);
            }

            return succ;
        }

        private void CloseConnection()
        {
            try
            {
                DBConnectionManager.Instance.CloseDBConnection(ACTAConnection);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " " + this.ToString() + ".CloseConnection() exception: " + ex.Message);
            }
        }
	}
}
