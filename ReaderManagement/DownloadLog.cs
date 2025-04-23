using System;
using System.Collections;
using System.Configuration;
using System.Threading;
using System.Collections.Generic;

using Common;
using ReaderInterface;
using TransferObjects;
using Util;

namespace ReaderManagement
{
	/// <summary>
	/// DownloadLog
	/// </summary>
	public class DownloadLog : IComparable
	{
		IReaderInterface readerInterface;
		private ReaderTO _reader;
		// File sufix
		private static string SUFFIXFORMAT = "yyyyMMddHHmmss";
		private static string FILEEXT = ".xml";
		private static string COMPLETELOG = "C";
		private string logDirectory = "";
		private DateTime nextTimeReading = new DateTime();

		private int _downloadInterval = -1;
		private int _numOfAttempts = 0;
		private int _totalReadingAttempts = 0;
		public int comPortNum = -1;
		public int CardAttempt = 0;
		public int TimeAccessProfileAttempt = 0;
		public DateTime ClockSettedAt = new DateTime();
        private DateTime lastPingTime = new DateTime();

        public DateTime LastPingTime
        {
            get { return lastPingTime; }
            set { lastPingTime = value; }
        }

		// Debug
		DebugLog debug;

		public ReaderTO Reader
		{
			get { return _reader; }
			set { _reader = value;}
		}

		public DateTime NextTimeReading
		{
			get { return nextTimeReading; }
			set { nextTimeReading = value; }
		}

		public int NumOfAttempts
		{
			get { return _numOfAttempts; }
			set { _numOfAttempts = value; }
		}

		public int TotalReadingAttempts
		{
			get { return _totalReadingAttempts; }
			set { _totalReadingAttempts = value; }
		}

		public int DownloadInterval 
		{
			get { return _downloadInterval; }
		}		

		// Observer initialization
		// Send message to other object that log downloading has started
		// Controller instance
		public NotificationController Controller;
		// Observer client instance
		public NotificationObserverClient observerClient;

		public DownloadLog()
		{
			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);

			InitializeObserverClient();
		}

		public DownloadLog(ReaderTO reader, string logPath)
		{
			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);

			ReaderFactory.TechnologyType = reader.TechType;
			readerInterface = ReaderFactory.GetReader;

			this.Reader = reader;
			this.NextTimeReading = DateTime.Now.AddSeconds(10);

			// Download interval in seconds
			this._downloadInterval = this.Reader.DownloadInterval * 60;

			logDirectory = logPath;
			InitializeObserverClient();
		}
		
		public DateTime GetNextReading()
		{
			DateTime readingTime = new DateTime();
			DateTime currentTime = DateTime.Now;

			DateTime downloadStartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
				this.Reader.DownloadStartTime.Hour, this.Reader.DownloadStartTime.Minute, 
				this.Reader.DownloadStartTime.Second);

			int i = 0;
			
			
			try
			{
				readingTime = downloadStartTime;

				if (readingTime.CompareTo(currentTime) >= 0)
				{
					while (currentTime.CompareTo(readingTime) < 0) 
					{
						readingTime = readingTime.AddMinutes(-this.Reader.DownloadInterval);
						i++;								
					}
					readingTime = readingTime.AddMinutes(this.Reader.DownloadInterval);
				}
				else
				{
					while (readingTime.CompareTo(currentTime) < 0) 
					{
						readingTime = readingTime.AddMinutes(this.Reader.DownloadInterval);
						i++;								
					}
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + ex.StackTrace);
			}

			return readingTime;
		}
		

		/// <summary>
		/// Read log from current reader, pushing to the log file
		/// </summary>
		/// <param name="needOnlyDiff">
		/// If you want to take just last a new data it need to be true,
		/// if you want all of the data from reader, need to be false. 
		/// </param>
		/// <returns>true if succeeded, false otherways</returns>
		public bool getLog(bool needOnlyDiff)
		{
			string path = "";

			try
			{
                Reader reader = new Reader();
                reader.RdrTO = this.Reader;
				this.comPortNum = reader.GetReaderAddress();

                reader = new Reader();
				LogTO logTOData = new LogTO();
                List<LogTO> logTOList = new List<LogTO>();
                ArrayList logLineList = new ArrayList();
				
				try
				{
					if (!this.comPortNum.Equals(-1))
					{
						//Send notification to the other objects that log downloading has started
						//Controller.Downloading(this.Reader, true);

						if (needOnlyDiff)
						{
							logLineList = readerInterface.GetDiffLog(this.comPortNum);
						}
						else
						{
							logLineList = readerInterface.GetLog(this.comPortNum);
						}

						//Send notification to the other objects that log downloading has finished
						//Controller.Downloading(this.Reader, false);
					}
					if (!readerInterface.GetError().Equals(""))
					{
						string dif = "";
						if (needOnlyDiff)
						{
							dif = "DIFF LOG";
						}
						else
						{
							dif = "COMPLETE LOG";
						}

                        System.Console.WriteLine(DateTime.Now + "Address: " + this.Reader.ConnectionAddress + " ReaderID: " + this.Reader.ReaderID + readerInterface.GetError() + " Records: " + logLineList.Count);
						System.Console.WriteLine(" " + dif);
                        debug.writeLog(DateTime.Now + "Address: " + this.Reader.ConnectionAddress + " ReaderID: " + this.Reader.ReaderID + readerInterface.GetError() + " Records: " + logLineList.Count);
						debug.writeLog(" " + dif + "\n\n");

						if (readerInterface.GetError().IndexOf("as a valid DateTime") != -1)
						{
							debug.writeLog("\n**** " );
						}
					}

					// Kontrola da li su meseci u log zapisima u rastucem redosledu - anomalija primecena u Eunet-u, Hitag
					int prevMonth = -1;
					foreach (LogLine logLine in logLineList)
					{
						int month = Int32.Parse((logLine.DateTime).Substring(5,2));
						if ((prevMonth > month) && (month != 1))
						{
							month = prevMonth;
							logLine.DateTime = logLine.DateTime.Substring(0,5) + month.ToString("D2") + logLine.DateTime.Substring(7);
						}
						prevMonth = month;
					}

				}
				catch(Exception ex)
				{
					System.Console.WriteLine("\n**** " + DateTime.Now + " Exception in: " + this.ToString() + 
						".GetLog() : ComPortNumber " + comPortNum + " ReaderID: " + this.Reader.ReaderID + " Error: " + readerInterface.GetError() + " " + ex.StackTrace + " ****\n\n");

					debug.writeLog("\n**** " + DateTime.Now + " Exception in: " + this.ToString() + 
						".GetLog() : ComPortNumber " + this.Reader.ConnectionAddress + " ReaderID: " + this.Reader.ReaderID + " Error: " + readerInterface.GetError() + " " + ex.StackTrace + " ****\n\n");

					return false;
				}
				
				foreach(LogLine logLine in logLineList)
				{
					if (logLine.IsValid)
					{
						// Create LogTO List
						logTOData = new LogTO();

						logTOData.ReaderID = this.Reader.ReaderID;
						logTOData.TagID = Convert.ToUInt32(logLine.TagID.ToString());
						logTOData.Antenna = Convert.ToInt32(logLine.Antenna.ToString());
						logTOData.EventHappened = Convert.ToInt32(logLine.Event.ToString());
						logTOData.ActionCommited = Convert.ToInt32(logLine.Action.ToString());
						logTOData.EventTime = Convert.ToDateTime(logLine.DateTime.ToString());
                        //Boris, posto su svi byte-ovi u RFID interface-u zauzeti, neka pri punjenju XML-a 
                        //u polje button upisuje antenu
                        logTOData.Button = Convert.ToInt32(logLine.Antenna.ToString());
						logTOData.PassGenUsed = 0;
					
						logTOList.Add(logTOData);
					}
				}

				// Serialize LogTOList to XML file
				Log currentLog = new Log();

				if (needOnlyDiff)
				{
					// Send diff log XML file to unprocessed
					path = this.logDirectory
                        + Constants.ReaderXMLLogFile 
						//+ ConfigurationManager.AppSettings["ReaderXMLLogFile"] 
						+ "_" + this.Reader.ReaderID.ToString().Trim() 
						+ "_" + DateTime.Now.ToString(SUFFIXFORMAT) 
						+ FILEEXT;
				}
				else
				{
					// Send complete log XML file to archived to preserve processing
					path = Constants.archived
                        + Constants.ReaderXMLLogFile 
						//+ ConfigurationManager.AppSettings["ReaderXMLLogFile"] 
						+ "_" + this.Reader.ReaderID.ToString().Trim() 
						+ "_" + DateTime.Now.ToString(SUFFIXFORMAT) 
						+ "_" + COMPLETELOG
						+ FILEEXT;
				}

				bool succeed = false;

				if (logTOList.Count > 0)
				{
					succeed = currentLog.SaveToFile(logTOList, path);
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + 
					".getLog() : Message: "+ex.Message+"/n StackTrace:" + ex.StackTrace);  
				throw ex;
			}
		
			return true;
		}


		public bool EraseLog()
		{
			bool succeed = false;

			try
			{
				if (this.comPortNum != -1)
				{
					readerInterface.EraseLog(this.comPortNum);
					System.Console.WriteLine("---- EraseLog ComPort: " + this.Reader.ConnectionAddress + " ReaderNum.: "+ this.Reader.ReaderID + " ----- GetError(): " + readerInterface.GetError() );
					debug.writeLog("---- EraseLog ComPort: " + this.Reader.ConnectionAddress + " ReaderNum.: "+ this.Reader.ReaderID + " ----- GetError(): " + readerInterface.GetError() + "\n\n");

					succeed = true;
				}

			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + 
					".EraseLog() : Message: "+ex.Message+"/n StackTrace:" + ex.StackTrace);  
			}
			
			return succeed;
		}

		private void InitializeObserverClient()
		{
			observerClient = new NotificationObserverClient(this.ToString());
			Controller = NotificationController.GetInstance();	
			//Controller.AttachToNotifier(observerClient);
			//this.observerClient.Notification += new NotificationEventHandler(this.MonitorEvent);
		}

		/// <summary>
		/// Get percentage of memory occupation for a given reader
		/// </summary>
		/// <returns>memory occupation</returns>
		public int readerMemoryOccupation()
		{
			int percentage = 0;

			try
			{
				percentage = readerInterface.GetLogPercentage();
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + 
					".readerMemoryOccupation() : Message: "+ex.Message+"/n StackTrace:" + ex.StackTrace);  
			}
			return percentage;
		}
		#region IComparable Members

		public int CompareTo(object obj)
		{
			DownloadLog log = (DownloadLog)obj;
			return this.Reader.A0GateID.CompareTo(log.Reader.A0GateID);
		}

		#endregion 
	}
}
