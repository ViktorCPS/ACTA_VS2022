using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Common;
using ReaderInterface;
using TransferObjects;
using Util;

namespace ReaderManagement
{
	/// <summary>
	/// Manage logs from reader.
	/// </summary>
	public class DownloadManager : IDisposable
	{
		private static DownloadManager managerInstance;

        Object ACTAConnection;

		private Thread downloadManagerThread;
		// List of Readers
		private static ArrayList _readerLogList = new ArrayList();
		private const string timeformat = "hh:mm:ss";
		private string _logFileDirectory = "";
		private volatile bool isReading = false;
		private int MAXATTEMPTSNUM = -1;
		private int ATTEMPTINTERVAL = -1;
		private int DOWNLOADSLEEPINTERVAL = -1;
		private ReaderEventController eventController;
		public bool IsRestarted = false;
		private static string FILESUFIX = "mmss";
		private static string UPLOADTIMEFLAG = "_L";
		private static string TEMPTIMEFLAG = "_T";
		private DateTime _cardUploadTime = new DateTime();

        private bool dbConnectionSucc = true;

		private System.Collections.Hashtable _readerControlMonitorSyncs = new Hashtable();

        private bool useDatabaseFiles = false;
        byte[] uploadTAP = null;
        int tapRecordID = -1;
        byte[] uploadCard = null;
        int cardRecordID = -1;
		
		// Debug
		DebugLog debug;

        // system ping instead of the one made by us
        private acPing ping;

		public ArrayList ReaderLogList
		{
			get { return _readerLogList; }
		}

		public string LogFileDirectory
		{
			get { return _logFileDirectory; }
			set { _logFileDirectory = value; }
		}
		
		public DateTime CardUploadTime
		{
			get { return _cardUploadTime; }
		}

		protected DownloadManager()
		{
			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);

			// Set default vaules
			//MAXATTEMPTSNUM = Convert.ToInt32(ConfigurationManager.AppSettings["MAXATTEMPTSNUM"]);
            MAXATTEMPTSNUM = Convert.ToInt32(Constants.MAXATTEMPTSNUM.ToString());
			//ATTEMPTINTERVAL = Convert.ToInt32(ConfigurationManager.AppSettings["ATTEMPTINTERVAL"]);
            ATTEMPTINTERVAL = Convert.ToInt32(Constants.ATTEMPTINTERVAL.ToString());
			//DOWNLOADSLEEPINTERVAL = Convert.ToInt32(ConfigurationManager.AppSettings["DOWNLOADSLEEPINTERVAL"]);
            DOWNLOADSLEEPINTERVAL = Convert.ToInt32(Constants.DOWNLOADSLEEPINTERVAL.ToString());

			downloadManagerThread = new Thread(new ThreadStart(Reading));
			isReading = false;
			eventController = ReaderEventController.GetInstance();

            ping = new acPing();
        }
		
		public static DownloadManager GetInstance()
		{
			try
			{
				if (managerInstance == null)
				{
					managerInstance = new DownloadManager();
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}

			return managerInstance;
		}

		private bool StopMonitorApplication(int gateID) 
		{
			ReaderControlMonitorSync rcmSync = (ReaderControlMonitorSync)_readerControlMonitorSyncs[gateID];

			if (!rcmSync.CanSynchronize()) 
			{
				return true;
			}

			// command Monitor application to stop
			bool stopped = false;
			bool succeeded = false;
			DateTime t0 = DateTime.Now;
			while(!succeeded)
			{
				try
				{
					succeeded = rcmSync.WriteGateSync(new ReaderControlMonitorSync("YES", "YES"));
					stopped = succeeded;
				}
				catch
				{
				}
				finally 
				{
					Thread.Sleep(100);
					if((DateTime.Now.Ticks - t0.Ticks) / TimeSpan.TicksPerMillisecond > 10000) 
					{
						succeeded = true; // abort stopping Monitor application
						if (!stopped) 
						{
							debug.writeLog(DateTime.Now + " Unable to stop Monitor application for the gate " + gateID.ToString() + "!");
						}
					}
				}
			}

			if (!stopped) return false;

			// wait for Monitor reply
			stopped = false;
			succeeded = false;
			t0 = DateTime.Now;
			while(!succeeded)
			{
				try
				{
					ReaderControlMonitorSync monReply = rcmSync.ReadGateSync();
					if ((monReply != null) && (monReply.monitoring.ToUpper() == "NO")) 
					{
						succeeded = true;
					}
					stopped = succeeded;
				}
				catch {}
				finally 
				{
					Thread.Sleep(100);
					if((DateTime.Now.Ticks - t0.Ticks) / TimeSpan.TicksPerMillisecond > 60000) 
					{
						succeeded = true; // abort reading Monitor reply
						if (!stopped) 
						{
							debug.writeLog(DateTime.Now + " StopMonitorApplication: unable to read Monitor application reply for the gate " + gateID.ToString() + "!");
						}
					}
				}
			}

			return stopped;
		}

		private bool StartMonitorApplication(int gateID) 
		{
			ReaderControlMonitorSync rcmSync = (ReaderControlMonitorSync)_readerControlMonitorSyncs[gateID];

            if (!rcmSync.CanSynchronize()) 
			{
				return true;
			}

			// command Monitor application to start
			bool started = false;
			bool succeeded = false;
			DateTime t0 = DateTime.Now;
			while(!succeeded)
			{
				try
				{
					succeeded = rcmSync.WriteGateSync(new ReaderControlMonitorSync("NO", "NO"));
					started = succeeded;
				}
				catch {}
				finally 
				{
					Thread.Sleep(100);
					if((DateTime.Now.Ticks - t0.Ticks) / TimeSpan.TicksPerMillisecond > 10000) 
					{
						succeeded = true; // abort starting Monitor application
						if (!started) 
						{
							debug.writeLog(DateTime.Now + " Unable to start Monitor application for the gate " + gateID.ToString() + "!");
						}
					}
				}
			}

			return started;
		}

		private bool StartMonitorApplicationWithWait(int gateID) 
		{
			ReaderControlMonitorSync rcmSync = (ReaderControlMonitorSync)_readerControlMonitorSyncs[gateID];

            if (!rcmSync.CanSynchronize())  
			{
				return true;
			}

			// command Monitor application to start
			bool started = false;
			bool succeeded = false;
			DateTime t0 = DateTime.Now;
			while(!succeeded)
			{
				try
				{
					succeeded = rcmSync.WriteGateSync(new ReaderControlMonitorSync("NO", "NO"));
					started = succeeded;
				}
				catch
				{
				}
				finally 
				{
					Thread.Sleep(100);
					if((DateTime.Now.Ticks - t0.Ticks) / TimeSpan.TicksPerMillisecond > 10000) 
					{
						succeeded = true; // abort starting Monitor application
						if (!started) 
						{
							debug.writeLog(DateTime.Now + " Unable to start Monitor application with wait for the gate " + gateID.ToString() + "!");
						}
					}
				}
			}

			if (!started) return false;

			// wait for Monitor reply
			started = false;
			succeeded = false;
			t0 = DateTime.Now;
			while(!succeeded)
			{
				try
				{
					ReaderControlMonitorSync monReply = rcmSync.ReadGateSync();
					if ((monReply != null) && (monReply.monitoring.ToUpper() == "YES")) 
					{
						succeeded = true;
					}
					started = succeeded;
				}
				catch {}
				finally 
				{
					Thread.Sleep(100);
					if((DateTime.Now.Ticks - t0.Ticks) / TimeSpan.TicksPerMillisecond > 120000) 
					{
						succeeded = true; // abort reading Monitor reply
						if (!started) 
						{
							debug.writeLog(DateTime.Now + " StartMonitorApplicationWithWait: unable to read Monitor application reply for the gate " + gateID.ToString() + "!");
						}
					}
				}
			}

			return started;
		}

		private void Reading()
		{
			bool downloadSucceeded = false;
			DateTime downloadTime = DateTime.Now;
			DateTime uploadTime = DateTime.Now;

            //v TimeCheck instead of Ping
            IReaderInterface rfid = ReaderFactory.GetReader;
            //^ TimeCheck instead of Ping

			int cardAtt = 0;
			// Critical moments for download
            List<WorkTimeIntervalTO> critMoments = new List<WorkTimeIntervalTO>();
			bool isFirstReading = true;

			try
			{
                //v TimeCheck instead of Ping
                ReaderFactory.TechnologyType = "MIFARE";
                rfid = ReaderFactory.GetReader;
                //^ TimeCheck instead of Ping
                
                critMoments = new TimeSchema().GetCriticalMoments();
                
                int databaseCount = new AccessControlFile().SearchCount("", "");
                if (databaseCount >= 0)
                    useDatabaseFiles = true;

				while(isReading)
				{
                    if (!useDatabaseFiles)
                    {
                        debug.writeBenchmarkLog(DateTime.Now + " Reading(): usedatabase files = false\n");
                        // If New Cards Files exists
                        if (Directory.GetFiles(Constants.cards).Length > 0)
                        {
                            // Upload cards to readers
                            this.UpdateCards(ReaderLogList, DateTime.Now, cardAtt);
                        }

                        // If New Time Access Profiles Files exists
                        if (Directory.GetFiles(Constants.timeaccessprofiles).Length > 0)
                        {
                            // Upload Time Access Profiles to readers
                            this.UpdateTimeAccessProfiles(ReaderLogList, DateTime.Now);
                        }
                    }
                    else
                    {
                        if (!ConnectToDataBase())
                            ACTAConnection = null;

                        AccessControlFile acf;

                        if (ACTAConnection != null)
                            acf = new AccessControlFile(ACTAConnection);
                        else
                            acf = new AccessControlFile();

                        debug.writeBenchmarkLog(DateTime.Now + " Reading(): usedatabase files = true\n");
                        // If New Cards Files exists
                        databaseCount = acf.SearchCount(Constants.ACFilesTypeCards, Constants.ACFilesStatusUnused);
                        debug.writeBenchmarkLog(DateTime.Now + " Reading(): acf.SearchCount(Constants.ACFilesTypeCards, Constants.ACFilesStatusUnused): " + databaseCount.ToString() + " \n");
                        if (databaseCount > 0)
                        {
                            // Upload cards to readers
                            this.UpdateCards(ReaderLogList, DateTime.Now, cardAtt);
                        }

                        // If New Time Access Profiles Files exists
                        databaseCount = acf.SearchCount(Constants.ACFilesTypeTAProfile, Constants.ACFilesStatusUnused);
                        if (databaseCount > 0)
                        {
                            // Upload Time Access Profiles to readers
                            this.UpdateTimeAccessProfiles(ReaderLogList, DateTime.Now);
                        }

                        if (databaseCount < 0)      // select exception, database connection probably lost
                        {
                            acf.CloseDBConnection();    // close DB connection so code will try to make a new one next time it accesses DB
                        }

                        CloseConnection();
                    }

                    //v TimeCheck instead of Ping
					// Setup Date Time
					//bool timeSet = this.SetTime(ReaderLogList);
                    //debug.writeBenchmarkLog(DateTime.Now + " Reading(): SetTime(ReaderLogList): " + timeSet.ToString() + " \n");
                    //^ TimeCheck instead of Ping

					int gateID = -1;
					bool lastStopped = false;
					foreach(DownloadLog readLog in ReaderLogList)
					{
                       // debug.writeBenchmarkLog(DateTime.Now + " Reading(): readLog.Reader.ReaderID: " + readLog.Reader.ReaderID.ToString() + " \n");
                       
                        //07.07.2009 Natasa from now try to ping each reader every minute and if it is pinging set c to grean else set it to red
                        if (readLog.LastPingTime.Equals(new DateTime()) || readLog.LastPingTime.AddSeconds(Constants.pingInterval) < DateTime.Now)
                        {
                            readLog.LastPingTime = DateTime.Now;

                            // Try Ping if IP
                           if (((readLog.Reader.ConnectionType.Equals(Constants.ConnTypeIP))
                               //v TimeCheck instead of Ping
                                //&& (ping.Completed(readLog.Reader.ConnectionAddress, 2)))
                                && (rfid.CheckTime(readLog.Reader.ConnectionAddress)))
                               //^ TimeCheck instead of Ping
                                || (readLog.Reader.ConnectionType.Equals(Constants.ConnTypeSerial)))
                            {
                                //v TimeCheck instead of Ping
                                //debug.writeLog(DateTime.Now + " Reading(): CheckTime reader: " + readLog.Reader.ConnectionAddress + " SUCCEEDED\n");
                                //^ TimeCheck instead of Ping

                                eventController.PingFailed(readLog.Reader, true);
                            }
                            else
                            {
                                //v TimeCheck instead of Ping
                                //debug.writeLog(DateTime.Now + " Reading(): Ping reader: " + readLog.Reader.ConnectionAddress + " failed\n");
                                debug.writeLog(DateTime.Now + " Reading(): CheckTime reader: " + readLog.Reader.ConnectionAddress + " FAILED\n");
                                //^ TimeCheck instead of Ping

                                eventController.PingFailed(readLog.Reader, false);
                            }
                        }
                        
						// Download logs from readers
						if (IsDownloadTimeNow(readLog, critMoments, isFirstReading))
						{
                            debug.writeBenchmarkLog(DateTime.Now + " Reading(): IsDownloadTimeNow(readLog, critMoments, isFirstReading)\n");
							// synchronize with the Monitor application
							if (gateID != readLog.Reader.A0GateID) 
							{
								if (gateID != -1) 
								{
									StartMonitorApplicationWithWait(gateID);
								}
								gateID = readLog.Reader.A0GateID;
								StopMonitorApplication(gateID);
								lastStopped = true;
							}

							isFirstReading = false;

							// Try Ping if IP
							eventController.ReaderDataExchange(readLog.Reader, true, new DateTime(), Constants.ActionLog);

							if (((readLog.Reader.ConnectionType.Equals(Constants.ConnTypeIP)) 
                                 //v TimeCheck instead of Ping
								 //&& (ping.Completed(readLog.Reader.ConnectionAddress, 2)))
                                 && (rfid.CheckTime(readLog.Reader.ConnectionAddress)))
                                 //^ TimeCheck instead of Ping
								|| (readLog.Reader.ConnectionType.Equals(Constants.ConnTypeSerial)))
							{
								downloadSucceeded = this.DownloadLog(readLog, true);

								// Erase log from raeder
								if (readLog.readerMemoryOccupation() >= readLog.Reader.DownloadEraseCounter)
								{
									downloadSucceeded = this.DownloadLog(readLog, false);
									readLog.EraseLog();
								}
							
								if (downloadSucceeded)
								{
                                    debug.writeBenchmarkLog(DateTime.Now + " Reading(): DownloadLog succeeded\n");
									// Next Download interval
									readLog.NextTimeReading = readLog.GetNextReading();
									Thread.Sleep(1000);

									readLog.Reader.MemoryOccupation = readLog.readerMemoryOccupation();
									eventController.ReaderDataExchange(readLog.Reader, false, readLog.NextTimeReading, Constants.ActionLog);
								}
									// Log Reading failed
								else
								{
                                    debug.writeBenchmarkLog(DateTime.Now + " Reading(): DownloadLog failed\n");
									readLog.NextTimeReading = readLog.NextTimeReading.AddSeconds(ATTEMPTINTERVAL);
									Thread.Sleep(1000);
								
									// Max num of attempts reached
									if (readLog.NumOfAttempts >= MAXATTEMPTSNUM) 
									{
										readLog.NextTimeReading = readLog.GetNextReading();
										System.Console.WriteLine(DateTime.Now + "Reading from " + readLog.Reader.ConnectionAddress + " failed! Attempt number: " + readLog.NumOfAttempts + " Reached the maximum number of Attempts! \n" );
                                        debug.writeBenchmarkLog(DateTime.Now + "Reading from " + readLog.Reader.ConnectionAddress + " failed! Attempt number: " + readLog.NumOfAttempts + " Max Attempts reached! \n");
										readLog.NumOfAttempts = 0;
									}
									else
									{
										readLog.NextTimeReading = readLog.NextTimeReading.AddSeconds(ATTEMPTINTERVAL);
										System.Console.WriteLine("Reading from " + readLog.Reader.ConnectionAddress + " failed! Attempt number: " + readLog.NumOfAttempts + " Next attempt for: " + ATTEMPTINTERVAL);
                                        debug.writeBenchmarkLog(DateTime.Now + "Reading from " + readLog.Reader.ConnectionAddress + " failed! Attempt number: " + readLog.NumOfAttempts + " Next attempt for: " + ATTEMPTINTERVAL + " \n");
										readLog.NumOfAttempts ++;
									}
									
									eventController.ReaderDataExchange(readLog.Reader, false, readLog.NextTimeReading, Constants.ActionLog);
									eventController.ReaderFailed(readLog.Reader, true, false);
                                    dbConnectionSucc = false;
                                    
								}
							}
							else
							{
								Thread.Sleep(1000);
								// Max num of attempts reached
								if (readLog.NumOfAttempts >= MAXATTEMPTSNUM) 
								{
									readLog.NextTimeReading = readLog.GetNextReading();
									
									System.Console.WriteLine(DateTime.Now + "Reading from " + readLog.Reader.ConnectionAddress + " failed! Attempt number: " + readLog.NumOfAttempts + " Reached the maximum number of Attempts! \n" );
                                    debug.writeBenchmarkLog(DateTime.Now + "Reading from " + readLog.Reader.ConnectionAddress + " failed! Attempt number: " + readLog.NumOfAttempts + " Max Attempts reached! \n");
									readLog.NumOfAttempts = 0;
								}
								else
								{
									readLog.NextTimeReading = readLog.NextTimeReading.AddSeconds(ATTEMPTINTERVAL);
									System.Console.WriteLine("Reading from " + readLog.Reader.ConnectionAddress + " failed! Attempt number: " + readLog.NumOfAttempts + " Next attempt for: " + ATTEMPTINTERVAL);
                                    debug.writeBenchmarkLog(DateTime.Now + "Reading from " + readLog.Reader.ConnectionAddress + " failed! Attempt number: " + readLog.NumOfAttempts + " Next attempt for: " + ATTEMPTINTERVAL + " \n");
									readLog.NumOfAttempts ++;
								}

								eventController.ReaderDataExchange(readLog.Reader, false, readLog.NextTimeReading, Constants.ActionLog);
								eventController.ReaderFailed(readLog.Reader, false, false);
                                dbConnectionSucc = false;
							}
						}
					}

					// start Monitor application for the last gate
					if (lastStopped)
					{
						StartMonitorApplicationWithWait(gateID);
						lastStopped = false;
					}

					Thread.Sleep(DOWNLOADSLEEPINTERVAL * 1000);
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(ex);
                AbortReading();
                //downloadManagerThread.Abort();
                //downloadManagerThread = null;
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
			string remotePath = Constants.unprocessed; 
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

				if (LogFileDirectory.Equals(Constants.unprocessed))
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
		/// For a given raeders, create DownloadLog objects and crate ArrayList
		/// </summary>
		/// <param name="radersList">List of Reader objects</param>
		/// <param name="path">path to the saved Reader Log Files</param>
		/// <returns>number of created ReaderLog objects</returns>
        //public int CreateReaderList(List<ReaderTO> radersList)
        //{
        //    ReaderManagement.DownloadLog downloadLog1 = new ReaderManagement.DownloadLog();
        //    this.LogFileDirectory = this.getLogFilePath();
        //    try
        //    {
        //        if (!this.LogFileDirectory.Equals(""))
        //        {
        //            DownloadManager._readerLogList.Clear();
        //            this._readerControlMonitorSyncs.Clear();
        //            int key = -1;
        //            object dbConnection = (object)null;
        //            foreach (ReaderTO raders in radersList)
        //            {
        //                ReaderManagement.DownloadLog downloadLog2 = new ReaderManagement.DownloadLog(raders, this.LogFileDirectory);
        //                DownloadManager._readerLogList.Add((object)downloadLog2);
        //                if (key != downloadLog2.Reader.A0GateID)
        //                {
        //                    key = downloadLog2.Reader.A0GateID;
        //                    ReaderControlMonitorSync controlMonitorSync;
        //                    if (dbConnection == null)
        //                    {
        //                        controlMonitorSync = ReaderControlMonitorSyncFactory.GetReaderControlMonitorSync(key.ToString());
        //                        dbConnection = controlMonitorSync.DBConnection;
        //                    }
        //                    else
        //                        controlMonitorSync = ReaderControlMonitorSyncFactory.GetReaderControlMonitorSync(key.ToString(), dbConnection);
        //                    this._readerControlMonitorSyncs.Add((object)key, (object)controlMonitorSync);
        //                }
        //            }
        //            DownloadManager._readerLogList.Sort();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.debug.writeLog(DateTime.Now.ToString() + " Exception in: " + this.ToString() + ".CreateReaderList() : Message: " + ex.Message + "/n StackTrace:" + ex.StackTrace);
        //    }
        //    return DownloadManager._readerLogList.Count;
        //}
        public int CreateReaderList(List<ReaderTO> radersList)
        {
            DownloadLog readerLog = new DownloadLog();
            this.LogFileDirectory = getLogFilePath();

            try
            {
                if (!this.LogFileDirectory.Equals(""))
                {
                    _readerLogList.Clear();

                    _readerControlMonitorSyncs.Clear();
                    int gateID = -1;

                    Object sharedDBConnection = null;
                    foreach (ReaderTO currentReader in radersList)
                    {
                        readerLog = new DownloadLog(currentReader, this.LogFileDirectory);
                        _readerLogList.Add(readerLog);

                        if (gateID != readerLog.Reader.A0GateID)
                        {
                            gateID = readerLog.Reader.A0GateID;
                            ReaderControlMonitorSync rcmSync;
                            if (sharedDBConnection == null)
                            {
                                rcmSync = ReaderControlMonitorSyncFactory.GetReaderControlMonitorSync(gateID.ToString());
                                sharedDBConnection = rcmSync.DBConnection;
                            }
                            else
                            {
                                rcmSync = ReaderControlMonitorSyncFactory.GetReaderControlMonitorSync(gateID.ToString(), sharedDBConnection);
                            }
                            //  BOJAN 12. 11. 2018. Provera duplikata gatea
                            if (!_readerControlMonitorSyncs.ContainsKey(gateID))
                            {
                                _readerControlMonitorSyncs.Add(gateID, rcmSync);
                            }

                        }
                    }

                    // sort _readerLogList by gateID
                    _readerLogList.Sort();
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".CreateReaderList() : Message: " + ex.Message + "/n StackTrace:" + ex.StackTrace);
            }

            return _readerLogList.Count;
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

		/// <summary>
		/// Upload new cards form file 
		/// </summary>
		/// <param name="reader">Reader that need to be updated with the new tags</param>
		/// <param name="actionTime"></param>
		/// <returns>true if succeeded, false if it failed</returns>
		private bool CardsUpload(ReaderTO reader, string fileName)
		{
			bool succeeded = false;
			List<TagTO> tagList = new List<TagTO>();
			
			try
			{
				try
				{
                    if (!useDatabaseFiles)
                    {
                        tagList = new Tag().GetFromXMLSource(fileName);
                    }
                    else
                    {
                        MemoryStream memStream = new MemoryStream(uploadCard);

                        // Set the position to the beginning of the stream.
                        memStream.Seek(0, SeekOrigin.Begin);

                        Tag tag;
                        if (ACTAConnection != null)
                            tag = new Tag(ACTAConnection);
                        else
                            tag = new Tag();

                        tagList = tag.GetFromXMLSource(memStream);

                        memStream.Close();
                    }
				}
				catch(Exception desEx)
				{
					throw desEx;
				}

				// Update reader data
				if (this.UpdateReaderTags(reader, tagList))
				{
					//succeeded = this.MoveCardsToArchived(fileName);
					succeeded = true;
				}
			}
			catch(Exception exDes)
			{
				if (exDes is DataProcessingException)
				{
					DataProcessingException dex = (DataProcessingException) exDes; 
					// Can't open XML file - continue
					if (dex.Number == 2)
					{
						debug.writeLog("File: " + fileName + " " + dex.message + "\n");
					}
						// Can't deserialize XML file - move to Trash and continue
					else if (dex.Number == 3)
					{
						debug.writeLog("File: " + fileName + " " + dex.message + "\n");
					}
				}
				else
				{
					debug.writeLog(DateTime.Now + " Exception in: " + 
						this.ToString() + ".CardsUpload() : " + exDes.Message);
				}

				succeeded = false;
			}

			return succeeded;
		}

		private bool UpdateReaderTags(ReaderTO reader, List<TagTO> tags)
		{
			bool succeeded = false;
			IReaderInterface rfid = ReaderFactory.GetReader;

			try
			{
				debug.writeLog(DateTime.Now + " +++ CardsUpload : STARTED! \n +++ ");

				ReaderFactory.TechnologyType = reader.TechType;
				rfid = ReaderFactory.GetReader;
				ArrayList cardList = new ArrayList();

				foreach(TagTO tag in tags)
				{
					Card card = new Card(tag.TagID, (byte) tag.AccessGroupID);
					cardList.Add(card);
				}
                debug.writeBenchmarkLog(DateTime.Now + " + Reader ID: " + reader.ReaderID + " Num. cards: " + cardList.Count + "\n");

                //If working with database, update status to InProgress, and set uploaded start time
                if (useDatabaseFiles)
                {
                    AccessControlFile acf;
                    
                    if (ACTAConnection != null)
                        acf = new AccessControlFile(ACTAConnection);
                    else
                        acf = new AccessControlFile();
                    
                    acf.Update(-1, "", "", Constants.ACFilesStatusInProgress, DateTime.Now,
                        new DateTime(0), -1, cardRecordID, Constants.ReaderControlUser, true);
                }

                Reader r;

                if (ACTAConnection != null)
                    r = new Reader(ACTAConnection);
                else
                    r = new Reader();

                r.RdrTO = reader;
				rfid.SetCards(r.GetReaderAddress(), cardList);
				if (rfid.GetError().Equals(""))
				{
					succeeded = true;
					debug.writeLog(DateTime.Now + " +++ CardsUpload : FINISHED! \n +++ ");
				}
				else
				{
					succeeded = false;
					debug.writeLog(DateTime.Now + " +++ CardsUpload : FAILED! " + rfid.GetError() + 
							"\n +++ ");
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".CardsUpload() : " + ex.StackTrace + "RFID Error: " + rfid.GetError() +"\n");
			}

			return succeeded;
		}

		private bool MoveCardsToArchived(string file)
		{
			bool succeeded = false;
			string newFilePath = ""; 

			try
			{
				newFilePath = Constants.archived + 
					Path.GetFileNameWithoutExtension(file) + 
					"_Copy" + 
					DateTime.Now.ToString(FILESUFIX) +
					Path.GetExtension(file);

				debug.writeLog(DateTime.Now + " Moving file : " + file + " to " + newFilePath + "\n");
				try
				{
					File.Move(file, newFilePath);
				}
				catch(Exception exIO)
				{
					throw exIO;
				}
				debug.writeLog(DateTime.Now + " File moved: " + file + " to " + newFilePath + "\n");

				succeeded = true;
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".MoveCardsToArchived() : " + ex.StackTrace + "\n");
			}

			return succeeded;
		}

		public string NewCardsExists(ReaderTO reader)
		{
			string newFile = "";
			string[] paths;

			try
			{
				// If file exists in Cards directory
				if (Directory.Exists(Constants.cards))
				{
					paths = Directory.GetFiles(Constants.cards);

					if (paths.Length > 0)
					{
						foreach(string currentFile in paths)
						{
							// Checking if nontemporary files for current reader exists
							if ((currentFile.IndexOf ("_" + reader.ReaderID + "_") > 0)
								&& (currentFile.LastIndexOf ("_" + reader.ReaderID + "_") > 0)
								&& (currentFile.LastIndexOf(TEMPTIMEFLAG) < 0)
								&& currentFile.IndexOf ("_" + reader.ReaderID + "_").Equals(
								currentFile.LastIndexOf("_" + reader.ReaderID + "_")))
							{
								newFile = currentFile;
								break;
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".NewCardsExists() : " + ex.StackTrace + "\n");
			}

			return newFile;
		}

		private bool ChangeFileName(ReaderTO reader)
		{
			string newFileName = "";
			bool succeeded = false;
			string fileName = "";

			try
			{
				fileName = this.NewCardsExists(reader);

				if (!fileName.Equals(""))
				{
					newFileName = Path.GetFileNameWithoutExtension(fileName) + "_L" +
						Path.GetExtension(fileName);
					File.Move(fileName, Path.GetDirectoryName(fileName) + "\\" + newFileName);

					succeeded = true;
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".ChangeFileName() : " + ex.StackTrace + "\n");
			}

			return succeeded;
		}
		
		private bool DownloadLog(DownloadLog readLog, bool needOnlyDiff)
		{
			bool succeeded = false;

			try
			{
				if (readLog.getLog(needOnlyDiff))
				{
					succeeded = true;
				}

			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".DownloadLog() : " + ex.StackTrace + "\n");
			}

			return succeeded;
		}

        private bool IsDownloadTimeNow(DownloadLog readerLog, List<WorkTimeIntervalTO> criticalMoments, bool isFirstReading)
		{
			bool isTime = false;
			DateTime downloadTime = readerLog.NextTimeReading;

			try
			{
				// Skip download in the critical moments
				DateTime startOffset = new DateTime();
				DateTime endOffset = new DateTime();

				int critStartBefore = Convert.ToInt32(ConfigurationManager.AppSettings["critStartBefore"]);
				int critStartAfter = Convert.ToInt32(ConfigurationManager.AppSettings["critStartAfter"]);
				int critEndBefore = Convert.ToInt32(ConfigurationManager.AppSettings["critEndBefore"]);
				int critEndAfter = Convert.ToInt32(ConfigurationManager.AppSettings["critEndAfter"]);

				if (DateTime.Compare(downloadTime, DateTime.Now) < 0)
				{
					if (criticalMoments.Count != 0)
					{
                        foreach (WorkTimeIntervalTO interval in criticalMoments)
						{
							// Check critical start time
							startOffset = new DateTime(downloadTime.Year, downloadTime.Month, downloadTime.Day, 
								interval.StartTime.Hour, interval.StartTime.Minute, 0);

							// Check critical end time
							endOffset = new DateTime(downloadTime.Year, downloadTime.Month, downloadTime.Day,
								interval.EndTime.Hour, interval.EndTime.Minute, 0);
						
							if (((DateTime.Compare(DateTime.Now, startOffset.AddMinutes(-critStartBefore)) >= 0) && 
								(DateTime.Compare(DateTime.Now, startOffset.AddMinutes(critStartAfter)) <= 0)) ||
								((DateTime.Compare(DateTime.Now, endOffset.AddMinutes(-critEndBefore)) >= 0) && 
								(DateTime.Compare(DateTime.Now, endOffset.AddMinutes(critEndAfter)) <= 0)))
							{
								isTime = false;
								debug.writeLog(DateTime.Now + "\n Critical moment : SKIP Download \n");
								readerLog.NextTimeReading = readerLog.GetNextReading();

								break;
							}
							else
							{
								isTime = true;
							}
						}
					}
					else
					{
						isTime = true;
					}
				}

				if (isFirstReading)
				{
					isTime = true;
				}

			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".IsUpdateTimeNow() : " + ex.StackTrace + "\n");
			}

			return isTime;
		}

		private bool UpdateCards(ArrayList readersLog, DateTime currentTime, int attempt)
		{
			bool succeeded = false;
			string currentCardFile = "";
			ReaderTO currentReader = new ReaderTO();

			try
			{	
				int gateID = -1;
				bool lastStopped = false;

				foreach(DownloadLog currentReaderLog in readersLog)
				{
					currentReader = currentReaderLog.Reader;
                    if (!useDatabaseFiles)
                    {
                        // Check if file for that reader exists
                        currentCardFile = NewCardsExists(currentReader);
                        if (!currentCardFile.Trim().Equals(""))
                        {
                            // Upload now
                            if ((currentCardFile.IndexOf(UPLOADTIMEFLAG) == currentCardFile.LastIndexOf(UPLOADTIMEFLAG)) &&
                                (currentCardFile.LastIndexOf(UPLOADTIMEFLAG) == -1))
                            {
                                eventController.ReaderDataExchange(currentReader, true, currentReaderLog.NextTimeReading, Constants.ActionTags);

                                if (((currentReader.ConnectionType.Equals(Constants.ConnTypeIP))
                                    && (ping.Completed(currentReader.ConnectionAddress, 2)))
                                    || (currentReader.ConnectionType.Equals(Constants.ConnTypeSerial)))
                                {

                                    // synchronize with the Monitor application
                                    if (gateID != currentReaderLog.Reader.A0GateID)
                                    {
                                        if (gateID != -1) StartMonitorApplicationWithWait(gateID);
                                        gateID = currentReaderLog.Reader.A0GateID;
                                        StopMonitorApplication(gateID);
                                        lastStopped = true;
                                    }

                                    if (this.CardsUpload(currentReader, currentCardFile))
                                    {
                                        succeeded = true;
                                        this.MoveCardsToArchived(currentCardFile);
                                        eventController.ReaderDataExchange(currentReader, false,
                                            currentReaderLog.NextTimeReading, Constants.ActionTags);
                                        eventController.ReaderFailed(currentReader, true, true&&dbConnectionSucc);
                                    }
                                    else
                                    {
                                        eventController.ReaderFailed(currentReader, true, false);

                                        if (currentReaderLog.CardAttempt > 3)
                                        {
                                            this.ChangeFileName(currentReaderLog.Reader);
                                            currentReaderLog.CardAttempt = 0;
                                        }
                                        else
                                        {
                                            currentReaderLog.CardAttempt++;
                                        }

                                        eventController.ReaderDataExchange(currentReader, false,
                                            currentReaderLog.NextTimeReading, Constants.ActionTags);
                                    }
                                }
                                else
                                {
                                    eventController.ReaderFailed(currentReader, false, false);
                                    if (currentReaderLog.CardAttempt > 3)
                                    {
                                        this.ChangeFileName(currentReaderLog.Reader);
                                        currentReaderLog.CardAttempt = 0;
                                    }
                                    else
                                    {
                                        currentReaderLog.CardAttempt++;
                                    }

                                    eventController.ReaderDataExchange(currentReader, false,
                                        currentReaderLog.NextTimeReading, Constants.ActionTags);
                                }
                            }
                            // Upload later (_L file found)
                            else
                            {
                                if ((currentTime.Hour == currentReader.DownloadStartTime.Hour) &&
                                    (currentTime.Minute == currentReader.DownloadStartTime.Minute))
                                {
                                    eventController.ReaderDataExchange(currentReader, true, currentReaderLog.NextTimeReading, Constants.ActionTags);

                                    if (((currentReader.ConnectionType.Equals(Constants.ConnTypeIP))
                                        && (ping.Completed(currentReader.ConnectionAddress, 2)))
                                        || (currentReader.ConnectionType.Equals(Constants.ConnTypeSerial)))
                                    {
                                        // synchronize with the Monitor application
                                        if (gateID != currentReaderLog.Reader.A0GateID)
                                        {
                                            if (gateID != -1) StartMonitorApplicationWithWait(gateID);
                                            gateID = currentReaderLog.Reader.A0GateID;
                                            StopMonitorApplication(gateID);
                                            lastStopped = true;
                                        }

                                        if (this.CardsUpload(currentReader, currentCardFile))
                                        {
                                            succeeded = true;
                                            this.MoveCardsToArchived(currentCardFile);

                                            eventController.ReaderDataExchange(currentReader, false,
                                                currentReaderLog.NextTimeReading, Constants.ActionTags);
                                            eventController.ReaderFailed(currentReader, true, true&&dbConnectionSucc);
                                        }
                                        else
                                        {
                                            eventController.ReaderFailed(currentReader, true, false);
                                        }
                                    }
                                    else
                                    {
                                        eventController.ReaderFailed(currentReader, false, false);
                                    }

                                    eventController.ReaderDataExchange(currentReader, true, currentReaderLog.NextTimeReading, Constants.ActionTags);
                                }
                            }
                        } //if (!currentCardFile.Trim().Equals(""))
                    } //if (!useDatabaseFiles)
                    else
                    {
                        AccessControlFile acf;

                        if (ACTAConnection != null)
                            acf = new AccessControlFile(ACTAConnection);
                        else
                            acf = new AccessControlFile();

                        ArrayList al = acf.Search(Constants.ACFilesTypeCards, currentReader.ReaderID, -1, Constants.ACFilesStatusUnused, new DateTime(0), new DateTime(0));
                        debug.writeBenchmarkLog(DateTime.Now + " UpdateCards(): AccessControlFile.Search count: " + al.Count.ToString() + "\n");
                        // Check if record for that reader exists
                        if (al.Count > 0)
                        {
                            uploadCard = ((AccessControlFile)al[0]).Content;
                            cardRecordID = ((AccessControlFile)al[0]).RecordID;

                            //If there is more than one record with status Unused, update all except last one to status Overwritten
                            if (al.Count > 1)
                            {
                                bool updated = acf.UpdateOthers(currentReader.ReaderID, Constants.ACFilesTypeCards, Constants.ACFilesStatusUnused,
                                    Constants.ACFilesStatusOverwritten, new DateTime(0), new DateTime(0), -1, cardRecordID, Constants.ReaderControlUser, true);
                                debug.writeBenchmarkLog(DateTime.Now + " UpdateCards(): AccessControlFile.UpdateOthers: " + updated.ToString() + "\n");
                            }

                            // Upload now
                            if (((AccessControlFile)al[0]).Delayed == (int)Constants.ACFilesDelay.DontDelay)
                            {
                                eventController.ReaderDataExchange(currentReader, true, currentReaderLog.NextTimeReading, Constants.ActionTags);

                                if (((currentReader.ConnectionType.Equals(Constants.ConnTypeIP))
                                    && (ping.Completed(currentReader.ConnectionAddress, 2)))
                                    || (currentReader.ConnectionType.Equals(Constants.ConnTypeSerial)))
                                {

                                    // synchronize with the Monitor application
                                    if (gateID != currentReaderLog.Reader.A0GateID)
                                    {
                                        if (gateID != -1) StartMonitorApplicationWithWait(gateID);
                                        gateID = currentReaderLog.Reader.A0GateID;
                                        StopMonitorApplication(gateID);
                                        lastStopped = true;
                                    }

                                    if (this.CardsUpload(currentReader, ""))
                                    {
                                        debug.writeBenchmarkLog(DateTime.Now + " UpdateCards(): AccessControlFile.CardsUpload succeeded\n");
                                        succeeded = true;
                                        bool dbupdated = acf.Update(-1, "", "", Constants.ACFilesStatusUsed, new DateTime(0),
                                            DateTime.Now, -1, cardRecordID, Constants.ReaderControlUser, true);
                                        debug.writeBenchmarkLog(DateTime.Now + " UpdateCards(): AccessControlFile.Update to status used: " + dbupdated.ToString() + "\n");
                                        eventController.ReaderDataExchange(currentReader, false,
                                            currentReaderLog.NextTimeReading, Constants.ActionTags);
                                        eventController.ReaderFailed(currentReader, true, true&&dbConnectionSucc);
                                    }
                                    else
                                    {
                                        debug.writeBenchmarkLog(DateTime.Now + " UpdateCards(): AccessControlFile.CardsUpload failed\n");
                                        eventController.ReaderFailed(currentReader, true, false);
                                        if (currentReaderLog.CardAttempt > 3)
                                        {
                                            bool updateunused1 = acf.Update(-1, "", "", Constants.ACFilesStatusUnused, new DateTime(0),
                                                new DateTime(0), (int)Constants.ACFilesDelay.Delay, cardRecordID, Constants.ReaderControlUser, true);
                                            debug.writeBenchmarkLog(DateTime.Now + " UpdateCards(): AccessControlFile.Update to status unused delay: " + updateunused1.ToString() + "\n");
                                            currentReaderLog.CardAttempt = 0;
                                        }
                                        else
                                        {
                                            bool updateunused2 = acf.Update(-1, "", "", Constants.ACFilesStatusUnused, new DateTime(0),
                                                new DateTime(0), -1, cardRecordID, Constants.ReaderControlUser, true);
                                            debug.writeBenchmarkLog(DateTime.Now + " UpdateCards(): AccessControlFile.Update to status unused: " + updateunused2.ToString() + "\n");
                                            currentReaderLog.CardAttempt++;
                                        }

                                        eventController.ReaderDataExchange(currentReader, false,
                                            currentReaderLog.NextTimeReading, Constants.ActionTags);
                                    }
                                }
                                else
                                {
                                    eventController.ReaderFailed(currentReader, false, false);
                                    if (currentReaderLog.CardAttempt > 3)
                                    {
                                        bool updateunused3 = acf.Update(-1, "", "", Constants.ACFilesStatusUnused, new DateTime(0),
                                            new DateTime(0), (int)Constants.ACFilesDelay.Delay, cardRecordID, Constants.ReaderControlUser, true);
                                        debug.writeBenchmarkLog(DateTime.Now + " UpdateCards(): AccessControlFile.Update to status unused delay: " + updateunused3.ToString() + "\n");
                                        currentReaderLog.CardAttempt = 0;
                                    }
                                    else
                                    {
                                        currentReaderLog.CardAttempt++;
                                    }

                                    eventController.ReaderDataExchange(currentReader, false,
                                        currentReaderLog.NextTimeReading, Constants.ActionTags);
                                }
                            }
                            // Upload later (_L file found)
                            else
                            {
                                if ((currentTime.Hour == currentReader.DownloadStartTime.Hour) &&
                                    //v 26.11.2012. uslov promenjen za slucaj da gornji select traje duze od minut, da ne bude preskocen upload
                                    //(currentTime.Minute == currentReader.DownloadStartTime.Minute))
                                    (currentTime.Minute >= currentReader.DownloadStartTime.Minute))
                                    //^ 26.11.2012. uslov promenjen za slucaj da gornji select traje duze od minut, da ne bude preskocen upload
                                {
                                    eventController.ReaderDataExchange(currentReader, true, currentReaderLog.NextTimeReading, Constants.ActionTags);

                                    if (((currentReader.ConnectionType.Equals(Constants.ConnTypeIP))
                                        && (ping.Completed(currentReader.ConnectionAddress, 2)))
                                        || (currentReader.ConnectionType.Equals(Constants.ConnTypeSerial)))
                                    {
                                        // synchronize with the Monitor application
                                        if (gateID != currentReaderLog.Reader.A0GateID)
                                        {
                                            if (gateID != -1) StartMonitorApplicationWithWait(gateID);
                                            gateID = currentReaderLog.Reader.A0GateID;
                                            StopMonitorApplication(gateID);
                                            lastStopped = true;
                                        }

                                        if (this.CardsUpload(currentReader, ""))
                                        {
                                            succeeded = true;
                                            bool updateunused4 = acf.Update(-1, "", "", Constants.ACFilesStatusUsed, new DateTime(0),
                                                DateTime.Now, -1, cardRecordID, Constants.ReaderControlUser, true);
                                            debug.writeBenchmarkLog(DateTime.Now + " UpdateCards(): AccessControlFile.Update to status used: " + updateunused4.ToString() + "\n");
                                            eventController.ReaderDataExchange(currentReader, false,
                                                currentReaderLog.NextTimeReading, Constants.ActionTags);
                                            eventController.ReaderFailed(currentReader, true, true&&dbConnectionSucc);
                                        }
                                        else
                                        {
                                            bool updateunused5 = acf.Update(-1, "", "", Constants.ACFilesStatusUnused, new DateTime(0),
                                                new DateTime(0), -1, cardRecordID, Constants.ReaderControlUser, true);
                                            debug.writeBenchmarkLog(DateTime.Now + " UpdateCards(): AccessControlFile.Update to status unused: " + updateunused5.ToString() + "\n");
                                            eventController.ReaderFailed(currentReader, true, false);
                                        }
                                    }
                                    else
                                    {
                                        eventController.ReaderFailed(currentReader, false, false);
                                    }

                                    eventController.ReaderDataExchange(currentReader, true, currentReaderLog.NextTimeReading, Constants.ActionTags);
                                }
                            }
                        } //if (al.Count > 0)
                    }//if (useDatabaseFiles)
				} //foreach

				// start Monitor application for the last gate
				if (lastStopped)
				{
					StartMonitorApplicationWithWait(gateID);
					lastStopped = false;
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".IsUpdateTimeNow() : " + ex.StackTrace + "\n");
			}

			return succeeded;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>        
		private bool SetTime(ArrayList readersList)
		{
			bool succeeded = false;
			DateTime now = DateTime.Now;
			string timeServer = "";
			try
			{
				try
				{
					timeServer = ConfigurationManager.AppSettings["TimeServer"].Trim();
				}
				catch
				{
					return true;
				}

				if (!timeServer.Equals(""))
				{
					foreach(DownloadLog readerLog in readersList)
					{
						// Setting for the first time
						if (readerLog.ClockSettedAt.Equals(new DateTime()))
						{
							readerLog.ClockSettedAt = new DateTime(now.Year,now.Month,
								now.Day, readerLog.Reader.DownloadStartTime.Hour, readerLog.Reader.DownloadStartTime.Minute, 0);

							if (DateTime.Compare(now, readerLog.ClockSettedAt) < 0) 
							{
								readerLog.ClockSettedAt = readerLog.ClockSettedAt.AddDays(-1);
							}
						}

						if ((now.Month.Equals(readerLog.ClockSettedAt.AddDays(1).Month)) &&
							(now.Day.Equals(readerLog.ClockSettedAt.AddDays(1).Day)) &&
							(now.Hour.Equals(readerLog.Reader.DownloadStartTime.Hour)) &&
							(now.Minute >= readerLog.Reader.DownloadStartTime.Minute))
						{
							// Set local time to local mashine
							debug.writeLog(DateTime.Now + " Setting up System.Time to machine: STARTED! \n");
							try
							{
								NTPClient client = new NTPClient(ConfigurationManager.AppSettings["TimeServer"]);
								client.Connect(true);

								readerLog.ClockSettedAt = new DateTime(now.Year,now.Month,
									now.Day, readerLog.Reader.DownloadStartTime.Hour, readerLog.Reader.DownloadStartTime.Minute, 0);
							}
							catch(Exception ex)
							{
								debug.writeLog(DateTime.Now + " Setting up System.Time to machine: FAILED! " + ex.StackTrace + "\n");
							}

							succeeded = true;
							debug.writeLog(DateTime.Now + " Setting up System.Time to machine: FINISHED! \n");
						}
					}
				}
				else return true;
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".SetTime() : " + ex.StackTrace + "\n");
			}
			return succeeded;
		}
        
		private bool UpdateTimeAccessProfiles(ArrayList readersLog, DateTime currentTime)
		{
			bool succeeded = false;
			string currentTAProfileFile = "";
			ReaderTO currentReader = new ReaderTO();

            //v TimeCheck instead of Ping
            IReaderInterface rfid = ReaderFactory.GetReader;
            //^ TimeCheck instead of Ping

			try
			{
                //v TimeCheck instead of Ping
                ReaderFactory.TechnologyType = "MIFARE";
                rfid = ReaderFactory.GetReader;
                //^ TimeCheck instead of Ping

				int gateID = -1;
				bool lastStopped = false;

				foreach(DownloadLog currentReaderLog in readersLog)
				{
					currentReader = currentReaderLog.Reader;
                    if (!useDatabaseFiles)
                    {
                        // Check if file for that reader exists
                        currentTAProfileFile = NewTimeAccessProfilesExists(currentReader);
                        if (!currentTAProfileFile.Trim().Equals(""))
                        {
                            // Upload now
                            if ((currentTAProfileFile.IndexOf(UPLOADTIMEFLAG) == currentTAProfileFile.LastIndexOf(UPLOADTIMEFLAG)) &&
                                (currentTAProfileFile.LastIndexOf(UPLOADTIMEFLAG) == -1))
                            {
                                eventController.ReaderDataExchange(currentReader, true, currentReaderLog.NextTimeReading, Constants.ActionTimeAccess);

                                if (((currentReader.ConnectionType.Equals(Constants.ConnTypeIP))
                                    && (ping.Completed(currentReader.ConnectionAddress, 2)))
                                    || (currentReader.ConnectionType.Equals(Constants.ConnTypeSerial)))
                                {

                                    // synchronize with the Monitor application
                                    if (gateID != currentReaderLog.Reader.A0GateID)
                                    {
                                        if (gateID != -1) StartMonitorApplicationWithWait(gateID);
                                        gateID = currentReaderLog.Reader.A0GateID;
                                        StopMonitorApplication(gateID);
                                        lastStopped = true;
                                    }

                                    if (this.TimeAccessProfilesUpload(currentReader, currentTAProfileFile))
                                    {
                                        succeeded = true;
                                        this.MoveCardsToArchived(currentTAProfileFile);
                                        eventController.ReaderDataExchange(currentReader, false,
                                            currentReaderLog.NextTimeReading, Constants.ActionTimeAccess);
                                        eventController.ReaderFailed(currentReader, true, true&&dbConnectionSucc);
                                    }
                                    else
                                    {
                                        eventController.ReaderFailed(currentReader, true, false);

                                        if (currentReaderLog.TimeAccessProfileAttempt > 3)
                                        {
                                            this.ChangeTimeAccessProfileFileName(currentReaderLog.Reader);
                                            currentReaderLog.TimeAccessProfileAttempt = 0;
                                        }
                                        else
                                        {
                                            currentReaderLog.TimeAccessProfileAttempt++;
                                        }

                                        eventController.ReaderDataExchange(currentReader, false,
                                            currentReaderLog.NextTimeReading, Constants.ActionTimeAccess);
                                    }
                                }
                                else
                                {
                                    eventController.ReaderFailed(currentReader, false, false);
                                    if (currentReaderLog.TimeAccessProfileAttempt > 3)
                                    {
                                        this.ChangeTimeAccessProfileFileName(currentReaderLog.Reader);
                                        currentReaderLog.TimeAccessProfileAttempt = 0;
                                    }
                                    else
                                    {
                                        currentReaderLog.TimeAccessProfileAttempt++;
                                    }

                                    eventController.ReaderDataExchange(currentReader, false,
                                        currentReaderLog.NextTimeReading, Constants.ActionTimeAccess);
                                }
                            }
                            // Upload later (_L file found)
                            else
                            {
                                if ((currentTime.Hour == currentReader.DownloadStartTime.Hour) &&
                                    //v 26.11.2012. uslov promenjen za slucaj da gornji select traje duze od minut, da ne bude preskocen upload
                                    //(currentTime.Minute == currentReader.DownloadStartTime.Minute))
                                    (currentTime.Minute >= currentReader.DownloadStartTime.Minute))
                                    //^ 26.11.2012. uslov promenjen za slucaj da gornji select traje duze od minut, da ne bude preskocen upload
                                {
                                    eventController.ReaderDataExchange(currentReader, true, currentReaderLog.NextTimeReading, Constants.ActionTimeAccess);

                                    if (((currentReader.ConnectionType.Equals(Constants.ConnTypeIP))
                                        && (ping.Completed(currentReader.ConnectionAddress, 2)))
                                        || (currentReader.ConnectionType.Equals(Constants.ConnTypeSerial)))
                                    {
                                        // synchronize with the Monitor application
                                        if (gateID != currentReaderLog.Reader.A0GateID)
                                        {
                                            if (gateID != -1) StartMonitorApplicationWithWait(gateID);
                                            gateID = currentReaderLog.Reader.A0GateID;
                                            StopMonitorApplication(gateID);
                                            lastStopped = true;
                                        }

                                        if (this.TimeAccessProfilesUpload(currentReader, currentTAProfileFile))
                                        {
                                            succeeded = true;
                                            this.MoveCardsToArchived(currentTAProfileFile);

                                            eventController.ReaderDataExchange(currentReader, false,
                                                currentReaderLog.NextTimeReading, Constants.ActionTimeAccess);
                                            eventController.ReaderFailed(currentReader, true, true&&dbConnectionSucc);
                                        }
                                        else
                                        {
                                            eventController.ReaderFailed(currentReader, true, false);
                                        }
                                    }
                                    else
                                    {
                                        eventController.ReaderFailed(currentReader, false, false);
                                    }

                                    eventController.ReaderDataExchange(currentReader, true, currentReaderLog.NextTimeReading, Constants.ActionTimeAccess);
                                }
                            }
                        }//if (!currentTAProfileFile.Trim().Equals(""))
                    }//if (!useDatabaseFiles)
                    else
                    {
                        AccessControlFile acf;

                        if (ACTAConnection != null)
                            acf = new AccessControlFile(ACTAConnection);
                        else
                            acf = new AccessControlFile();

                        ArrayList al = acf.Search(Constants.ACFilesTypeTAProfile, currentReader.ReaderID, -1, Constants.ACFilesStatusUnused, new DateTime(0), new DateTime(0));
                        debug.writeBenchmarkLog(DateTime.Now + " UpdateTimeAccessProfiles(): AccessControlFile.Search count: " + al.Count.ToString() + "\n");
                        // Check if record for that reader exists
                        if (al.Count > 0)
                        {
                            uploadTAP = ((AccessControlFile)al[0]).Content;
                            tapRecordID = ((AccessControlFile)al[0]).RecordID;

                            //If there is more than one record with status Unused, update all except last one to status Overwritten
                            if (al.Count > 1)
                            {
                                bool updated = acf.UpdateOthers(currentReader.ReaderID, Constants.ACFilesTypeTAProfile, Constants.ACFilesStatusUnused,
                                    Constants.ACFilesStatusOverwritten, new DateTime(0), new DateTime(0), -1, tapRecordID, Constants.ReaderControlUser, true);
                                debug.writeBenchmarkLog(DateTime.Now + " UpdateTimeAccessProfiles(): AccessControlFile.UpdateOthers: " + updated.ToString() + "\n");
                            }

                            // Upload now
                            if (((AccessControlFile)al[0]).Delayed == (int)Constants.ACFilesDelay.DontDelay)
                            {
                                eventController.ReaderDataExchange(currentReader, true, currentReaderLog.NextTimeReading, Constants.ActionTimeAccess);

                                if (((currentReader.ConnectionType.Equals(Constants.ConnTypeIP))
                                    && (ping.Completed(currentReader.ConnectionAddress, 2)))
                                    || (currentReader.ConnectionType.Equals(Constants.ConnTypeSerial)))
                                {

                                    // synchronize with the Monitor application
                                    if (gateID != currentReaderLog.Reader.A0GateID)
                                    {
                                        if (gateID != -1) StartMonitorApplicationWithWait(gateID);
                                        gateID = currentReaderLog.Reader.A0GateID;
                                        StopMonitorApplication(gateID);
                                        lastStopped = true;
                                    }

                                    if (this.TimeAccessProfilesUpload(currentReader, ""))
                                    {
                                        debug.writeBenchmarkLog(DateTime.Now + " UpdateTimeAccessProfiles(): TimeAccessProfilesUpload succeeded\n");
                                        succeeded = true;
                                        bool update1 = acf.Update(-1, "", "", Constants.ACFilesStatusUsed, new DateTime(0),
                                            DateTime.Now, -1, tapRecordID, Constants.ReaderControlUser, true);
                                        debug.writeBenchmarkLog(DateTime.Now + " UpdateTimeAccessProfiles(): Update used: " + update1.ToString() + "\n");
                                        eventController.ReaderDataExchange(currentReader, false,
                                            currentReaderLog.NextTimeReading, Constants.ActionTimeAccess);
                                        eventController.ReaderFailed(currentReader, true, true&&dbConnectionSucc);
                                    }
                                    else
                                    {
                                        debug.writeBenchmarkLog(DateTime.Now + " UpdateTimeAccessProfiles(): TimeAccessProfilesUpload failed\n");
                                        eventController.ReaderFailed(currentReader, true, false);
                                        if (currentReaderLog.TimeAccessProfileAttempt > 3)
                                        {
                                            bool update2 = acf.Update(-1, "", "", Constants.ACFilesStatusUnused, new DateTime(0),
                                                new DateTime(0), (int)Constants.ACFilesDelay.Delay, tapRecordID, Constants.ReaderControlUser, true);
                                            debug.writeBenchmarkLog(DateTime.Now + " UpdateTimeAccessProfiles(): Update unused delay: " + update2.ToString() + "\n");
                                            currentReaderLog.TimeAccessProfileAttempt = 0;
                                        }
                                        else
                                        {
                                            bool update3 = acf.Update(-1, "", "", Constants.ACFilesStatusUnused, new DateTime(0),
                                                new DateTime(0), -1, tapRecordID, Constants.ReaderControlUser, true);
                                            debug.writeBenchmarkLog(DateTime.Now + " UpdateTimeAccessProfiles(): Update unused: " + update3.ToString() + "\n");
                                            currentReaderLog.TimeAccessProfileAttempt++;
                                        }

                                        eventController.ReaderDataExchange(currentReader, false,
                                            currentReaderLog.NextTimeReading, Constants.ActionTimeAccess);
                                    }
                                }
                                else
                                {
                                    eventController.ReaderFailed(currentReader, false, false);
                                    if (currentReaderLog.TimeAccessProfileAttempt > 3)
                                    {
                                        bool update4 = acf.Update(-1, "", "", Constants.ACFilesStatusUnused, new DateTime(0),
                                            new DateTime(0), (int)Constants.ACFilesDelay.Delay, tapRecordID, Constants.ReaderControlUser, true);
                                        debug.writeBenchmarkLog(DateTime.Now + " UpdateTimeAccessProfiles(): Update unused delay: " + update4.ToString() + "\n");
                                        currentReaderLog.TimeAccessProfileAttempt = 0;
                                    }
                                    else
                                    {
                                        currentReaderLog.TimeAccessProfileAttempt++;
                                    }

                                    eventController.ReaderDataExchange(currentReader, false,
                                        currentReaderLog.NextTimeReading, Constants.ActionTimeAccess);
                                }
                            }
                            // Upload later (_L file found)
                            else
                            {
                                if ((currentTime.Hour == currentReader.DownloadStartTime.Hour) &&
                                    (currentTime.Minute == currentReader.DownloadStartTime.Minute))
                                {
                                    eventController.ReaderDataExchange(currentReader, true, currentReaderLog.NextTimeReading, Constants.ActionTimeAccess);

                                    if (((currentReader.ConnectionType.Equals(Constants.ConnTypeIP))
                                        && (ping.Completed(currentReader.ConnectionAddress, 2)))
                                        || (currentReader.ConnectionType.Equals(Constants.ConnTypeSerial)))
                                    {
                                        // synchronize with the Monitor application
                                        if (gateID != currentReaderLog.Reader.A0GateID)
                                        {
                                            if (gateID != -1) StartMonitorApplicationWithWait(gateID);
                                            gateID = currentReaderLog.Reader.A0GateID;
                                            StopMonitorApplication(gateID);
                                            lastStopped = true;
                                        }

                                        if (this.TimeAccessProfilesUpload(currentReader, ""))
                                        {
                                            succeeded = true;
                                            bool update5 = acf.Update(-1, "", "", Constants.ACFilesStatusUsed, new DateTime(0),
                                                DateTime.Now, -1, tapRecordID, Constants.ReaderControlUser, true);
                                            debug.writeBenchmarkLog(DateTime.Now + " UpdateTimeAccessProfiles(): Update used later: " + update5.ToString() + "\n");
                                            eventController.ReaderDataExchange(currentReader, false,
                                                currentReaderLog.NextTimeReading, Constants.ActionTimeAccess);
                                            eventController.ReaderFailed(currentReader, true, true);
                                            dbConnectionSucc = true;
                                        }
                                        else
                                        {
                                            bool update6 = acf.Update(-1, "", "", Constants.ACFilesStatusUnused, new DateTime(0),
                                                new DateTime(0), -1, tapRecordID, Constants.ReaderControlUser, true);
                                            debug.writeBenchmarkLog(DateTime.Now + " UpdateTimeAccessProfiles(): Update unused later: " + update6.ToString() + "\n");
                                            eventController.ReaderFailed(currentReader, true, false);
                                        }
                                    }
                                    else
                                    {
                                        eventController.ReaderFailed(currentReader, false, false);
                                    }

                                    eventController.ReaderDataExchange(currentReader, true, currentReaderLog.NextTimeReading, Constants.ActionTimeAccess);
                                }
                            }
                        } //if (al.Count > 0)
                    } //if (useDatabaseFiles)
				} //foreach

				// start Monitor application for the last gate
				if (lastStopped)
				{
					StartMonitorApplicationWithWait(gateID);
					lastStopped = false;
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".UpdateTimeAccessProfiles() : " + ex.StackTrace + "\n");
			}

			return succeeded;
		}

		/// <summary>
		/// Upload new time access profile from file 
		/// </summary>
		/// <param name="reader">Reader that need to be updated with the new time access profile</param>
		/// <param name="actionTime"></param>
		/// <returns>true if succeeded, false if it failed</returns>
		private bool TimeAccessProfilesUpload(ReaderTO reader, string fileName)
		{
			bool succeeded = false;
			ArrayList timeAccessProfilesList = new ArrayList();
			TimeAccessProfileDtl timeAccessProfileDtl;

            if (ACTAConnection != null)
                timeAccessProfileDtl = new TimeAccessProfileDtl(ACTAConnection);
            else
                timeAccessProfileDtl = new TimeAccessProfileDtl();
                    
			try
			{
				try
				{
                    if (!useDatabaseFiles)
                    {
                        timeAccessProfilesList = timeAccessProfileDtl.GetFromXMLSource(fileName);
                    }
                    else
                    {
                        MemoryStream memStream = new MemoryStream(uploadTAP);
                        
                        // Set the position to the beginning of the stream.
                        memStream.Seek(0, SeekOrigin.Begin);
                        
                        timeAccessProfilesList = timeAccessProfileDtl.GetFromXMLSource(memStream);

                        memStream.Close();
                    }
				}
				catch(Exception desEx)
				{
					throw desEx;
				}

				// Update reader data
				if (this.UpdateReaderTimeAccessProfiles(reader, timeAccessProfilesList))
				{
					succeeded = true;
				}
			}
			catch(Exception exDes)
			{
				if (exDes is DataProcessingException)
				{
					DataProcessingException dex = (DataProcessingException) exDes; 
					// Can't open XML file - continue
					if (dex.Number == 2)
					{
						debug.writeLog("File: " + fileName + " " + dex.message + "\n");
					}
						// Can't deserialize XML file - move to Trash and continue
					else if (dex.Number == 3)
					{
						debug.writeLog("File: " + fileName + " " + dex.message + "\n");
					}
				}
				else
				{
					debug.writeLog(DateTime.Now + " Exception in: " + 
						this.ToString() + ".TimeAccessProfilesUpload() : " + exDes.Message);
				}

				succeeded = false;
			}

			return succeeded;
		}

		private bool UpdateReaderTimeAccessProfiles(ReaderTO reader, ArrayList timeAccessProfiles)
		{
			bool succeeded = false;
			IReaderInterface rfid = ReaderFactory.GetReader;

			byte[] TAPDbytes = new byte[2*16*7*24];
			for (int i = 0; i < TAPDbytes.Length; i++) TAPDbytes[i] = 0;

			try
			{
				debug.writeLog(DateTime.Now + " +++ TimeAccessProfilesUpload : STARTED! \n +++ ");

				ReaderFactory.TechnologyType = reader.TechType;
				rfid = ReaderFactory.GetReader;
				ArrayList timeAccessProfilesList = new ArrayList();

				foreach(TimeAccessProfileDtl timeAccessProfileDtl in timeAccessProfiles)
				{
					AddToTAPDbytes(timeAccessProfileDtl,TAPDbytes,reader.A0Direction,reader.A1Direction);
				}
				debug.writeBenchmarkLog(DateTime.Now + " + Reader ID: " + reader.ReaderID + " Num. time access profiles: " + timeAccessProfilesList.Count + "\n");

                //If working with database, update status to InProgress, and set uploaded start time
                if (useDatabaseFiles)
                {
                    AccessControlFile acf;

                    if (ACTAConnection != null)
                        acf = new AccessControlFile(ACTAConnection);
                    else
                        acf = new AccessControlFile();

                    acf.Update(-1, "", "", Constants.ACFilesStatusInProgress, DateTime.Now,
                        new DateTime(0), -1, tapRecordID, Constants.ReaderControlUser, true);
                }

                Reader r;
                if (ACTAConnection != null)
                    r = new Reader(ACTAConnection);
                else
                    r = new Reader();
                r.RdrTO = reader;
				rfid.SetTimeAccessProfiles(r.GetReaderAddress(),TAPDbytes);
				if (rfid.GetError().Equals(""))
				{
					succeeded = true;
					debug.writeLog(DateTime.Now + " +++ TimeAccessProfilesUpload : FINISHED! \n +++ ");
				}
				else
				{
					succeeded = false;
					debug.writeLog(DateTime.Now + " +++ TimeAccessProfilesUpload : FAILED! " + rfid.GetError() + 
						"\n +++ ");
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".UpdateReaderTimeAccessProfiles() : " + ex.StackTrace + "RFID Error: " + rfid.GetError() +"\n");
			}

			return succeeded;
		}
/*
        private void AddToTAPDbytes(TimeAccessProfileDtl tapd, byte[] tapdB, string A0Direction, string A1Direction)
        {
            int dir = (tapd.Direction == "IN") ? 0 : 1;

            tapdB[dir * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 0] = (byte)tapd.Hrs0;
            tapdB[dir * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 1] = (byte)tapd.Hrs1;
            tapdB[dir * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 2] = (byte)tapd.Hrs2;
            tapdB[dir * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 3] = (byte)tapd.Hrs3;
            tapdB[dir * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 4] = (byte)tapd.Hrs4;
            tapdB[dir * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 5] = (byte)tapd.Hrs5;
            tapdB[dir * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 6] = (byte)tapd.Hrs6;
            tapdB[dir * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 7] = (byte)tapd.Hrs7;
            tapdB[dir * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 8] = (byte)tapd.Hrs8;
            tapdB[dir * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 9] = (byte)tapd.Hrs9;
            tapdB[dir * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 10] = (byte)tapd.Hrs10;
            tapdB[dir * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 11] = (byte)tapd.Hrs11;
            tapdB[dir * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 12] = (byte)tapd.Hrs12;
            tapdB[dir * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 13] = (byte)tapd.Hrs13;
            tapdB[dir * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 14] = (byte)tapd.Hrs14;
            tapdB[dir * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 15] = (byte)tapd.Hrs15;
            tapdB[dir * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 16] = (byte)tapd.Hrs16;
            tapdB[dir * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 17] = (byte)tapd.Hrs17;
            tapdB[dir * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 18] = (byte)tapd.Hrs18;
            tapdB[dir * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 19] = (byte)tapd.Hrs19;
            tapdB[dir * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 20] = (byte)tapd.Hrs20;
            tapdB[dir * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 21] = (byte)tapd.Hrs21;
            tapdB[dir * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 22] = (byte)tapd.Hrs22;
            tapdB[dir * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 23] = (byte)tapd.Hrs23;
        }
*/
        private void AddToTAPDbytes(TimeAccessProfileDtl tapd, byte[] tapdB, string A0Direction, string A1Direction)
        {
            if (tapd.Direction == A0Direction) AddToTAPDbytesForAntenna(tapd, tapdB, 0);
            if (tapd.Direction == A1Direction) AddToTAPDbytesForAntenna(tapd, tapdB, 1);
        }

        private void AddToTAPDbytesForAntenna(TimeAccessProfileDtl tapd, byte[] tapdB, int ant)
        {
            tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 0] = (byte)tapd.Hrs0;
            tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 1] = (byte)tapd.Hrs1;
            tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 2] = (byte)tapd.Hrs2;
            tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 3] = (byte)tapd.Hrs3;
            tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 4] = (byte)tapd.Hrs4;
            tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 5] = (byte)tapd.Hrs5;
            tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 6] = (byte)tapd.Hrs6;
            tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 7] = (byte)tapd.Hrs7;
            tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 8] = (byte)tapd.Hrs8;
            tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 9] = (byte)tapd.Hrs9;
            tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 10] = (byte)tapd.Hrs10;
            tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 11] = (byte)tapd.Hrs11;
            tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 12] = (byte)tapd.Hrs12;
            tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 13] = (byte)tapd.Hrs13;
            tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 14] = (byte)tapd.Hrs14;
            tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 15] = (byte)tapd.Hrs15;
            tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 16] = (byte)tapd.Hrs16;
            tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 17] = (byte)tapd.Hrs17;
            tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 18] = (byte)tapd.Hrs18;
            tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 19] = (byte)tapd.Hrs19;
            tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 20] = (byte)tapd.Hrs20;
            tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 21] = (byte)tapd.Hrs21;
            tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 22] = (byte)tapd.Hrs22;
            tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 23] = (byte)tapd.Hrs23;
        }

		public string NewTimeAccessProfilesExists(ReaderTO reader)
		{
			string newFile = "";
			string[] paths;

			try
			{
				// If file exists in timeaccessprofiles directory
				if (Directory.Exists(Constants.timeaccessprofiles))
				{
					paths = Directory.GetFiles(Constants.timeaccessprofiles);

					if (paths.Length > 0)
					{
						foreach(string currentFile in paths)
						{
							// Checking if nontemporary files for current reader exists
							if ((currentFile.IndexOf ("_" + reader.ReaderID + "_") > 0)
								&& (currentFile.LastIndexOf ("_" + reader.ReaderID + "_") > 0)
								&& (currentFile.LastIndexOf(TEMPTIMEFLAG) < 0)
								&& currentFile.IndexOf ("_" + reader.ReaderID + "_").Equals(
								currentFile.LastIndexOf("_" + reader.ReaderID + "_")))
							{
								newFile = currentFile;
								break;
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".NewTimeAccessProfilesExists() : " + ex.StackTrace + "\n");
			}

			return newFile;
		}

		private bool ChangeTimeAccessProfileFileName(ReaderTO reader)
		{
			string newFileName = "";
			bool succeeded = false;
			string fileName = "";

			try
			{
				fileName = this.NewTimeAccessProfilesExists(reader);

				if (!fileName.Equals(""))
				{
					newFileName = Path.GetFileNameWithoutExtension(fileName) + "_L" +
						Path.GetExtension(fileName);
					File.Move(fileName, Path.GetDirectoryName(fileName) + "\\" + newFileName);

					succeeded = true;
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".ChangeTimeAccessProfileFileName() : " + ex.StackTrace + "\n");
			}

			return succeeded;
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
