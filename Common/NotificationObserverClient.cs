using System;
using System.Configuration;
using System.Collections;

using TransferObjects;
using Util;

namespace Common
{
	/// <summary>
	/// Delegate declaration.
	/// This event is delegated to the class that receive notification.
	/// Each form will handle this event in a different way.
	/// </summary>
	public delegate void NotificationEventHandler(object sender, NotificationEventArgs e);

	/// <summary>
	/// NotificationObserverClient recive changes, fire event and delegate event
	/// </summary>
	public class NotificationObserverClient
	{
		public event NotificationEventHandler Notification;
        public event NotificationEventHandler ReaderNotification; 
        public event NotificationEventHandler PingNotification;
		public event NotificationEventHandler OnDataProcessingStateChanged;
        public event NotificationEventHandler OnNotificationStateChanged;
		public event NotificationEventHandler OnCardOwnerObserved;

		private string _name;
		private DebugLog log;

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}


		public NotificationObserverClient(string name)
		{
			Name = name;
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);
		}

		
		public void Update(string text)
		{
			try
			{
				NotificationEventArgs e = new NotificationEventArgs(text);
				// Invokes the delegates. 
				OnNotification(this, e);
			}
			catch(Exception ex)
			{
				log.writeLog("NotificationObserverClient.Update(text):" + ex.Message + "\n");
			}
		}

		public void Update(ReaderTO currReader)
		{
			try
			{
				NotificationEventArgs e = new NotificationEventArgs(currReader);
				// Invokes the delegates. 
				OnNotification(this, e);
			}
			catch(Exception ex)
			{
				log.writeLog("NotificationObserverClient.Update(Reader):" + ex.Message + "\n");
			}

		}

		public void Update(bool isEmpoloyeeChanged)
		{
			try
			{
				NotificationEventArgs e = new NotificationEventArgs(isEmpoloyeeChanged);
				// Invokes the delegate
				OnNotification(this, e);
			}
			catch(Exception ex)
			{
				log.writeLog("NotificationObserverClient.Update(bool isEmpoloyeeChanged):" + ex.Message + "\n");
			}
		}

		public void PassTypeChanged(bool isChanged)
		{
			try
			{
				NotificationEventArgs e = new NotificationEventArgs();

				e.isPassTypeChanged = isChanged;
				OnNotification(this, e);
			}
			catch(Exception ex)
			{
				log.writeLog("NotificationObserverClient.PassTypeChanged(bool isChanged):" + ex.Message + "\n");
			}
		}

        public void DBChanged(bool isChanged)
        {
            try
            {
                NotificationEventArgs e = new NotificationEventArgs();

                e.dbChanged = isChanged;
                OnNotification(this, e);
            }
            catch (Exception ex)
            {
                log.writeLog("NotificationObserverClient.DBChanged(bool isChanged):" + ex.Message + "\n");
            }

        }

		public void TimeSchemaChanged(WorkTimeSchemaTO schema)
		{
			try
			{
				NotificationEventArgs e = new NotificationEventArgs();

				e.schema = schema;
				OnNotification(this, e);
			}
			catch(Exception ex)
			{
				log.writeLog("NotificationObserverClient.TimeSchemaDayChanged(TimeSchemaDay day):" + ex.Message + "\n");
			}

		}

		public void DaySelected(DateTime daySelected)
		{
			try
			{
				NotificationEventArgs e = new NotificationEventArgs();

				e.daySelected = daySelected;
				OnNotification(this, e);
			}
			catch(Exception ex)
			{
				log.writeLog("NotificationObserverClient.DaySelected(TimeSchemaDay day):" + ex.Message + "\n");
			}

		}

        public void AuxPortChanged(ArrayList readersAuxPorts)
        {
            try
            {
                NotificationEventArgs e = new NotificationEventArgs();

                e.readersAuxPorts = readersAuxPorts;
                OnNotification(this, e);
            }
            catch (Exception ex)
            {
                log.writeLog("NotificationObserverClient.AuxPortChanged(ArraList readersAuxPorts):" + ex.Message + "\n");
            }
        }

        public void CellBoxCheckedChanged(int roleID, string position, string cellBoxChecked)
        {
            try
            {
                NotificationEventArgs e = new NotificationEventArgs();

                e.roleID = roleID;
                e.position = position;
                e.cellBoxChecked = cellBoxChecked;
                OnNotification(this, e);
            }
            catch (Exception ex)
            {
                log.writeLog("NotificationObserverClient.AuxPortChanged(ArraList readersAuxPorts):" + ex.Message + "\n");
            }
        }

        public void CloseGraphicReportsClick(bool closeGrphicReports)
        {
            try
            {
                NotificationEventArgs e = new NotificationEventArgs();

                e.closeGrphicReports = closeGrphicReports;
                OnNotification(this, e);
            }
            catch (Exception ex)
            {
                log.writeLog("NotificationObserverClient.CloseGraphicReportsClick(bool closeGrphicReports):" + ex.Message + "\n");
            }
        }
        public void UNIPROMTagIDChanged(string unipromTagID)
        {
            try
            {
                NotificationEventArgs e = new NotificationEventArgs();

                e.unipromTagID = unipromTagID;
                OnNotification(this, e);
            }
            catch (Exception ex)
            {
                log.writeLog("NotificationObserverClient.CloseGraphicReportsClick(bool closeGrphicReports):" + ex.Message + "\n");
            }
        }

        internal void UNIPROMTransactionStatusChanged(string unipromTransactionStatus)
        {
            try
            {
                NotificationEventArgs e = new NotificationEventArgs();

                e.unipromTransactionStatus = unipromTransactionStatus;
                OnNotification(this, e);
            }
            catch (Exception ex)
            {
                log.writeLog("NotificationObserverClient.CloseGraphicReportsClick(bool closeGrphicReports):" + ex.Message + "\n");
            }
        }

        internal void UNIPROMStatusChanged(string unipromStatus)
        {
            try
            {
                NotificationEventArgs e = new NotificationEventArgs();

                e.unipromStatus = unipromStatus;
                OnNotification(this, e);
            }
            catch (Exception ex)
            {
                log.writeLog("NotificationObserverClient.CloseGraphicReportsClick(bool closeGrphicReports):" + ex.Message + "\n");
            }
        }
        public void IOPairDateChanged(bool showIOPairsDateChanged)
        {
            try
            {
                NotificationEventArgs e = new NotificationEventArgs();

                e.showIOPairsDateChanged = showIOPairsDateChanged;
                OnNotification(this, e);
            }
            catch (Exception ex)
            {
                log.writeLog("NotificationObserverClient.IOPairDateChanged(bool showIOPairsDateChanged):" + ex.Message + "\n");
            }
        }

        public void AuxPortChanged(int gateID)
        {
            try
            {
                NotificationEventArgs e = new NotificationEventArgs();

                e.gateID = gateID;
                OnNotification(this, e);
            }
            catch (Exception ex)
            {
                log.writeLog("NotificationObserverClient.AuxPortChanged(int gateID):" + ex.Message + "\n");
            }
        }

		public void LogInUser(ApplUserTO logInUser)
		{
			try
			{
				NotificationEventArgs e = new NotificationEventArgs();

				e.logInUser = logInUser;
				OnNotification(this, e);
			}
			catch(Exception ex)
			{
				log.writeLog("NotificationObserverClient.LogInUser(ApplUser logInUser):" + ex.Message + "\n");
			}

		}

		public void MonitorEventHappened(ReaderTO reader, int antNum, UInt64 tagID)
		{
			try
			{
				NotificationEventArgs e = new NotificationEventArgs(reader, antNum, tagID);
				OnNotification(this, e);
				
			}
			catch(Exception ex)
			{
				log.writeLog("NotificationObserverClient.MonitorEventHappened():" + ex.Message + "\n");
				throw ex;
			}
		}

		public void Downloading(ReaderTO reader, bool isDownloadState)
		{
			try
			{
				NotificationEventArgs e = new NotificationEventArgs(reader, isDownloadState);
				OnNotification(this, e);
			}
			catch(Exception ex)
			{
				log.writeLog("NotificationObserverClient.Downloading():" + ex.Message + "\n");
				throw ex;
			}
		}

		public void FileProcessing(string XMLFileName, bool isProcessingNow)
		{
			try
			{
				NotificationEventArgs e = new NotificationEventArgs(XMLFileName, isProcessingNow);
				OnNotification(this, e);
			}
			catch(Exception ex)
			{
				log.writeLog("NotificationObserverClient.FileProcessing():" + ex.Message + "\n");
				throw ex;
			}
		}

		public void DataProcessingStateChanged(string state)
		{
			try
			{
				NotificationEventArgs e = new NotificationEventArgs(state);
				if (OnDataProcessingStateChanged != null) 
				{
					// Fires the event
					OnDataProcessingStateChanged(this, e);
				}

			}
			catch(Exception ex)
			{
				log.writeLog("NotificationObserverClient.DataProcessingStateChanged():" + ex.Message + "\n");
				throw ex;
			}
		}

        public void NotificationStateChanged(string state)
        {
            try
            {
                NotificationEventArgs e = new NotificationEventArgs(state);
                if (OnNotificationStateChanged != null)
                {
                    // Fires the event
                    OnNotificationStateChanged(this, e);
                }

            }
            catch (Exception ex)
            {
                log.writeLog("NotificationObserverClient.DataProcessingStateChanged():" + ex.Message + "\n");
                throw ex;
            }
        }


		/// <summary>
		/// Download process occure
		/// </summary>
		/// <param name="isDownloadingNow"></param>
		/// <param name="nextDownloadingAt"></param>
		public void DownloadStarted(bool isDownloadingNow, DateTime nextDownloadingAt)
		{
			try
			{
				NotificationEventArgs e = new NotificationEventArgs(isDownloadingNow, nextDownloadingAt);
				OnNotification(this, e);
			}
			catch(Exception ex)
			{
				log.writeLog("NotificationObserverClient.DownloadStarted():" + ex.Message + "\n");
				throw ex;
			}
		}

		public void ReaderAction(ReaderTO currentReader, bool isStarted, bool isSucceeded, 
			bool isPingSucceeded, DateTime nextDownloadingAt)
		{
			try
			{
				NotificationEventArgs e = new NotificationEventArgs();

				e.reader = currentReader;
				e.isDownloading = isStarted;
				e.isDownloadSucceeded = isSucceeded;
				e.isPingSucceeded = isPingSucceeded;
				e.nextTimeDownload = nextDownloadingAt;

				// Invokes the delegates. 
				OnNotification(this, e);
			}
			catch(Exception ex)
			{
				log.writeLog("NotificationObserverClient.ReaderAction():" + ex.Message + "\n");
				throw ex;
			}
		}

		public void ReaderDataExchange(ReaderTO currentReader, bool isStarted, DateTime nextReading, string actionName)
		{
			NotificationEventArgs e = new NotificationEventArgs();
			
			e.reader = currentReader;
			e.isDownloading = isStarted;
			e.nextTimeDownload = nextReading;
			e.readerAction = actionName;

			OnNotification(this, e);
		}

		public void ReaderFailed(ReaderTO currentReader, bool isNetExist, bool isDataExist)
		{
			NotificationEventArgs e = new NotificationEventArgs();

			e.reader = currentReader;
			e.isNetExist = isNetExist;
			e.isDataExist = isDataExist;

			OnReaderNotification(this, e);
		}

        public void PingFailed(ReaderTO currentReader, bool isNetExist)
        {
            NotificationEventArgs e = new NotificationEventArgs();

            e.reader = currentReader;
            e.isNetExist = isNetExist;

            OnPingNotification(this, e);
        }

		/// <summary>
		/// The protected OnNotification method raises the event by invoking 
		/// the delegates. The sender is always current instance 
		/// of the class.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void OnNotification(object sender, NotificationEventArgs e)
		{
				
			if (Notification != null) 
			{
				// Invokes the delegates. 
				Notification(this, e);
			}
		}
		
		/// <summary>
		/// The protected OnReaderNotification method raises the event by invoking 
		/// the delegates. The sender is always current instance 
		/// of the class.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void OnReaderNotification(object sender, NotificationEventArgs e)
		{
				
			if (ReaderNotification != null) 
			{
				// Invokes the delegates. 
				ReaderNotification(this, e);
			}
		}

        public virtual void OnPingNotification(object sender, NotificationEventArgs e)
        {

            if (ReaderNotification != null)
            {
                // Invokes the delegates. 
                PingNotification(this, e);
            }
        }
		
		public void CardOwnerObserved(CardOwner cardOwner)
		{
			try
			{
				NotificationEventArgs e = new NotificationEventArgs(cardOwner);
				if (OnCardOwnerObserved != null) 
				{
					// Fires the event
					OnCardOwnerObserved(this, e);
				}

			}
			catch(Exception ex)
			{
				log.writeLog("NotificationObserverClient.CardOwnerObserved():" + ex.Message + "\n");
				throw ex;
			}
		}

       
    }
}
