using System;
using System.Collections;

using TransferObjects;

namespace Common
{
	/// <summary>
	/// NotificationEventArgs class is a container for data 
	/// that need to be passed to Observer Client who will fire 
	/// event of the class instance that it belongs to.
	/// </summary>
	/// <seealso>NotificationObserverClient</seealso>
	public class NotificationEventArgs : EventArgs
	{
		public string message = "";
		public ReaderTO reader; 
		public bool isEmployeeChanged;
        public bool dbChanged;
		public bool isPassTypeChanged;
		public WorkTimeSchemaTO schema;
		public DateTime daySelected;
		public ApplUserTO logInUser;
        public ArrayList readersAuxPorts;
        public int gateID;
        public int roleID;
        public string position;
        public string cellBoxChecked;

        //UNIPROM properties
        public string unipromTagID;
        public string unipromStatus;
        public string unipromTransactionStatus;

        //Close graphic reports form
        public bool closeGrphicReports;
        //Show label IOPairsChanged on graphic report
        public bool showIOPairsDateChanged;

		// Monitor Event Happened
		public int antennaNum = -1;
		public UInt64 tagID = 0;
		
		//
		// Notification about Download data from Reader
		//
		// Downlaod Started/Stopped
		public bool isDownloading = false;
		public string readerAction = "";
		public bool isNetExist = false;
		public bool isDataExist = false;


		// Download has succeeded
		public bool isDownloadSucceeded = false;
		// Ping has succeeded
		public bool isPingSucceeded = false;
		// Next Download process will begin at
		public DateTime nextTimeDownload = new DateTime();

		// XML File that started to be processed
		public string XMLFileName = "";
		// True if XML file start to be processed,
		// false if processing was finished
		public bool isProcessingNow = false;
		
		public CardOwner cardOwner = null;

		public NotificationEventArgs()
		{
			
		}
		public NotificationEventArgs(string text)
		{
			message = text;		
		}

		public NotificationEventArgs(ReaderTO currReader)
		{
			reader = currReader;
		}

		public NotificationEventArgs(bool isEmpChanged)
		{
			isEmployeeChanged = isEmpChanged;
		}

		public NotificationEventArgs(ReaderTO currentReader, int currentAntennaNum, UInt64 currentTagID)
		{
			reader = currentReader;
			antennaNum = currentAntennaNum;
			tagID = currentTagID;
		}

		public NotificationEventArgs(ReaderTO currentReader, bool isActive)
		{
			reader = currentReader;
			isDownloading = isActive;
		}

		public NotificationEventArgs(string XMLfile, bool isProcessing)
		{
			XMLFileName = XMLfile;
			isProcessingNow = isProcessing;
		}

		/// <summary>
		/// Download process occure
		/// </summary>
		/// <param name="isStartedNow">true if download started</param>
		/// <param name="nextTime">Next Download process will begin at</param>
		public NotificationEventArgs(bool isStartedNow, DateTime nextTime)
		{
			isDownloading = isStartedNow;
			nextTimeDownload = nextTime;
		}

		public NotificationEventArgs(CardOwner owner)
		{
			this.cardOwner = owner;
		}
	}
}
