using System;
using System.Collections;
using System.Configuration;

using TransferObjects;
using Util;

namespace Common
{
	/// <summary>
	/// Implements Observer pattern.
	/// This class sends changed object to all Observer Clients that 
	/// are currently in the notification queue. 
	/// when changes occurred. 
	/// </summary>
	/// <seealso>NotificationObserverClient, NotificationEventArgs</seealso>
	public class Notifier
	{
		private static Notifier instance;
		private ArrayList observersList = new ArrayList();
		private string _message;
		private ReaderTO _currentReader;
        private WorkTimeSchemaTO _schema;

		private bool _employeeChanged;
        private bool _dbChanged;
        private ArrayList _readersAuxPorts;

        private int _roleID;
        private string _position;
        private string _cellBoxChecked;

        private bool _closeGraphicReports;

        private bool _showIOPairsDateChanged;

        private string _unipromTagID;
        private string _unipromStatus;
        private string _unipromTransactionStatus;

        private int _gateID;

		private bool _passTypeHaveChanged;

		private DateTime _daySelected;

		private ApplUserTO _logInUser;

		private int _antenna;
		private UInt64 _tagID;

		// Reader Log Download State 
		//(true if download is active)
		private bool _downloadState = false;

		DebugLog log;

		public string Message
		{
			get { return _message;}
			set 
			{
				_message = value; 
				Notify(_message);
			}
		}

		public ReaderTO CurrentReader
		{
			get {return _currentReader; }
			set
			{
				_currentReader = value;
				CurrentReaderChanged(_currentReader);
			}
		}

		public bool EmployeeChanged
		{
			get { return _employeeChanged; }
			set 
			{ 
				_employeeChanged = value;
				EmpolyeeChanged(_employeeChanged);
			}
		}

        public bool dbChanged
        {
            get { return _dbChanged; }
            set
            {
                _dbChanged = value;
                DBChanged(_dbChanged);
            }
        }

		public bool PassTypeHaveChanged
		{
			get { return _passTypeHaveChanged; }
			set 
			{ 
				_passTypeHaveChanged = value; 
				PassTypeChanged(_passTypeHaveChanged);
			}
		}

        public WorkTimeSchemaTO Schema
		{
			get { return _schema; }
			set 
			{
				_schema = value;
				TimeSchemaChanged(_schema);
			}
		}

        public int RoleID
        {
            get { return _roleID; }
            set
            {
                _roleID = value;
            }
        }

        public string Position
        {
            get { return _position; }
            set
            {
                _position = value;
            }
        }

        public string CellBoxChecked
        {
            get { return _cellBoxChecked; }
            set
            {
                _cellBoxChecked = value;
                CellBoxCheckedChanged(_roleID, _position, _cellBoxChecked);
            }
        }

        public ArrayList ReadersAuxPorts
        {
            get { return _readersAuxPorts; }
            set
            {
                _readersAuxPorts = value;
                AuxPortChanged(_readersAuxPorts);
            }
        }

        public bool CloseGraphicReports
        {
            get { return _closeGraphicReports; }
            set
            {
                _closeGraphicReports = value;
                CloseGraphicReportsClick(_closeGraphicReports);
            }
        }
         public string ShowUNIPROMTagIDChanged
         {
             get { return _unipromTagID; }
            set
            {
                _unipromTagID = value;
                UNIPROMTagIDChanged(_unipromTagID);
            }
        }
       
        public string ShowUNIPROMStatusChanged
        {
            get { return _unipromStatus; }
            set
            {
                _unipromStatus = value;
                UNIPROMStatusChanged(_unipromStatus);
            }
        }

        
        public string ShowUNIPROMTransactionStatusChanged
        {
            get { return _unipromTransactionStatus; }
            set
            {
                _unipromTransactionStatus = value;
                UNIPROMTransactionStatusChanged(_unipromTransactionStatus);
            }
        }

       
        public bool ShowIOPairsDateChanged
        {
            get { return _showIOPairsDateChanged; }
            set
            {
                _showIOPairsDateChanged = value;
                IOPairDateChanged(_showIOPairsDateChanged);
            }
        }
        public int GateID
        {
            get { return _gateID; }
            set
            {
                _gateID = value;
                AuxPortChanged(_gateID);
            }
        }

		public DateTime SelectedDay
		{
			get { return _daySelected;}
			set 
			{
				_daySelected = value; 
				DaySelected(_daySelected);
			}
		}

		public ApplUserTO LogInUser
		{
			get { return _logInUser; }
			set 
			{
				_logInUser = value;
				LogInUserChanged(_logInUser);
			}
		}

		public int Antenna
		{
			get { return _antenna; }
			set {_antenna = value; }
		}

		public UInt64 TagID
		{
			get { return _tagID; }
			set {_tagID = value; }
		}

		public bool DownloadState
		{
			get {return _downloadState; }
		}

		protected Notifier()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);
		}

		public static Notifier GetInstance()
		{
			if (instance == null)
			{
				instance = new Notifier();
			}

			return instance;
		}

		public void Attach(NotificationObserverClient obs)
		{
			observersList.Add(obs);
		}

		public void Detach(NotificationObserverClient obs)
		{
			observersList.Remove(obs);
		}

		public void Notify(string message)
		{
				
			foreach (NotificationObserverClient obs in observersList)
			{
				obs.Update(message);
			}

			log.writeLog("FormNotifier.Notify(message): All observers are notified about changes: Message is :" + message + "\n");
		}

		public void CurrentReaderChanged(ReaderTO crrReader)
		{
			foreach(NotificationObserverClient obs in observersList)
			{
				obs.Update(CurrentReader);
			}
			log.writeLog("FormNotifier.Notify(Reader crrReader): All observers are notified about changes:" + CurrentReader.ReaderID.ToString() + "\n");
		}

		public void EmpolyeeChanged(bool isChanged)
		{
			foreach(NotificationObserverClient obs in observersList)
			{
				obs.Update(isChanged);
			}
			log.writeLog("FormNotifier.Notify(Reader crrReader): All observers are notified about Employee's changes \n");

		}

        public void DBChanged(bool isChanged)
        {
            foreach (NotificationObserverClient obs in observersList)
            {
                obs.DBChanged(isChanged);
            }
        }

		public void PassTypeChanged(bool isChanged)
		{
			foreach(NotificationObserverClient obs in observersList)
			{
				obs.PassTypeChanged(isChanged);
			}
		}

		public void TimeSchemaChanged(WorkTimeSchemaTO schema)
		{
			foreach(NotificationObserverClient obs in observersList)
			{
				obs.TimeSchemaChanged(schema);
			}
		}

		public void DaySelected(DateTime daySelected)
		{
			foreach(NotificationObserverClient obs in observersList)
			{
				obs.DaySelected(daySelected);
			}
		}

        public void AuxPortChanged(ArrayList readersAuxPorts)
        {
            foreach (NotificationObserverClient obs in observersList)
            {
                obs.AuxPortChanged(readersAuxPorts);
            }
        }

        public void CellBoxCheckedChanged(int roleID, string position, string cellBoxChecked)
        {
            foreach (NotificationObserverClient obs in observersList)
            {
                obs.CellBoxCheckedChanged(roleID, position, cellBoxChecked);
            }
        }

        public void CloseGraphicReportsClick(bool closeGraphicReports)
        {
            foreach (NotificationObserverClient obs in observersList)
            {
                obs.CloseGraphicReportsClick(closeGraphicReports);
            }
        }
        private void UNIPROMTagIDChanged(string unipromTagID)
        {
            foreach (NotificationObserverClient obs in observersList)
            {
                obs.UNIPROMTagIDChanged(unipromTagID);
            }
        }
        private void UNIPROMTransactionStatusChanged(string unipromTransactionStatus)
        {
            foreach (NotificationObserverClient obs in observersList)
            {
                obs.UNIPROMTransactionStatusChanged(unipromTransactionStatus);
            }
        }
        private void UNIPROMStatusChanged(string unipromStatus)
        {
            foreach (NotificationObserverClient obs in observersList)
            {
                obs.UNIPROMStatusChanged(unipromStatus);
            }
        }
        public void IOPairDateChanged(bool showIOPairsDateChanged)
        {
            foreach (NotificationObserverClient obs in observersList)
            {
                obs.IOPairDateChanged(showIOPairsDateChanged);
            }
        }

        public void AuxPortChanged(int gateID)
        {
            foreach (NotificationObserverClient obs in observersList)
            {
                obs.AuxPortChanged(gateID);
            }
        }

		public void LogInUserChanged(ApplUserTO logInUser)
		{
			foreach(NotificationObserverClient obs in observersList)
			{
				obs.LogInUser(logInUser);
			}
		}

		public void MonitorEvent(ReaderTO currentReader, int currentAntennaNum, UInt64 currentTagID)
		{
			this.CurrentReader = currentReader;
			this.Antenna = currentAntennaNum;
			this.TagID = currentTagID;

			foreach(NotificationObserverClient obs in observersList)
			{
				obs.MonitorEventHappened(this.CurrentReader, this.Antenna,this.TagID);
			}
		}

		public void Downloading(ReaderTO reader, bool isDownloadState)
		{
			//this.CurrentReader = reader;
			_downloadState = isDownloadState;

			foreach(NotificationObserverClient obs in observersList)
			{
				obs.Downloading(reader, this.DownloadState);
			}
		}

		/// <summary>
		/// In the moment and XML file start to be processed, this method will be called 
		/// to notify other classes about event and file name.  
		/// </summary>
		/// <param name="filename">current file path</param>
		/// <param name="isProcessingNow">true if file start with processing, false if it ends processing</param>
		public void FileProcessing(string filename, bool isProcessingNow)
		{
			foreach(NotificationObserverClient obs in observersList)
			{
				obs.FileProcessing(filename, isProcessingNow);
			}
		}

		public void DataProcessingStateChanged(string state)
		{
			foreach(NotificationObserverClient obs in observersList)
			{
				obs.DataProcessingStateChanged(state);
			}
		}
        public void NotificationStateChanged(string state)
        {
            foreach (NotificationObserverClient obs in observersList)
            {
                obs.NotificationStateChanged(state);
            }
        }

		public void DownloadStarted(bool isDownloadingNow, DateTime nextDownloadingAt)
		{
			foreach(NotificationObserverClient obs in observersList)
			{
				obs.DownloadStarted(isDownloadingNow, nextDownloadingAt);
			}
		}

		public void ReaderAction(ReaderTO currentReader, bool isStarted, bool isSucceeded, 
			bool isPingSucceeded, DateTime nextDownloadingAt)
		{
			foreach(NotificationObserverClient obs in observersList)
			{
				obs.ReaderAction(currentReader, isStarted, isSucceeded, 
					isPingSucceeded, nextDownloadingAt);
			}
		}

		public void ReaderDataExchange(ReaderTO reader, bool isStarted, DateTime nextReading, string actionName)
		{
			foreach(NotificationObserverClient obs in observersList)
			{
				obs.ReaderDataExchange(reader, isStarted, nextReading, actionName);
			}
		}

		public void ReaderFailed(ReaderTO reader, bool isNetFailed, bool isDataFailed)
		{
			foreach(NotificationObserverClient obs in observersList)
			{
				obs.ReaderFailed(reader, isNetFailed, isDataFailed);
			}
		}

        public void PingFailed(ReaderTO reader, bool isNetFailed)
        {
            foreach (NotificationObserverClient obs in observersList)
            {
                obs.PingFailed(reader, isNetFailed);
            }
        }

		public void CardOwnerObserved(CardOwner cardOwner)
		{
			foreach(NotificationObserverClient obs in observersList)
			{
				obs.CardOwnerObserved(cardOwner);
			}
		}
	}
}
