using System;

using System.Xml;
using System.Xml.Serialization;

namespace TransferObjects
{
	/// <summary>
	/// This class is used by the DAO layer to send and receive 
	/// data from the Reader class
	/// </summary>
	[XmlRootAttribute()]
	public class ReaderTO
	{
		private int _readerId = -1;
		private string _description = "";
		private string _connType = "";
		private string _connAdderss = "";
		private byte _settings = 0;
		private string _technologyType = "";

		// Antenna 0
		private int _a0IsCounter = -1;
		private int _a0GateID = -1;

		private int _a0LocationID = -1;
		private string _a0Direction = "";
		private int _a0SecLocationID = -1;
		private string _a0SecDirection = "";

		// Antenna 1
		private int _a1IsCounter = -1;
		private int _a1GateID = -1;

		private int _a1LocationID = -1;
		private string _a1Direction = "";
		private int _a1SecLocationID = -1;
		private string _a1SecDirection = "";

        private DateTime _downloadStartTime = new DateTime(DateTime.Now.Year, 1, 1);
		private int _downloadInterval = -1;
		private int _downloadEraseCounter = -1;

		private string _status = "";
		private DateTime _LastReadTime = new DateTime();

        private int _currerntMemoryOccupation = -1;

		[XmlAttributeAttribute(AttributeName="ReaderID")]
		public int ReaderID
		{
			get { return _readerId; }
			set { _readerId = value; }
		}

		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		public string ConnectionType
		{
			get { return _connType; }
			set { _connType = value; }
		}

		public string ConnectionAddress
		{
			get { return _connAdderss; }
			set { _connAdderss = value; }
		}

		public byte Settings
		{
			get { return _settings; }
			set { _settings = value; }
		}

		public string TechType
		{
			get { return _technologyType; }
			set { _technologyType = value; }
		}

		// Antenna 0
		public int A0IsCounter
		{
			get { return _a0IsCounter; }
			set { _a0IsCounter = value; }
		}

		public int A0GateID
		{
			get { return _a0GateID; }
			set { _a0GateID = value; }
		}

		public int A0LocID
		{
			get { return _a0LocationID; }
			set { _a0LocationID = value; }
		}

		public string A0Direction
		{
			get { return _a0Direction; }
			set {_a0Direction = value; }
		}

		public int A0SecLocID
		{
			get { return _a0SecLocationID; }
			set { _a0SecLocationID = value; }
		}

		public string A0SecDirection
		{
			get { return _a0SecDirection; }
			set {_a0SecDirection = value; }
		}

		// Antenna 1
		public int A1IsCounter
		{
			get { return _a1IsCounter; }
			set { _a1IsCounter = value; }
		}

		public int A1GateID
		{
			get { return _a1GateID; }
			set { _a1GateID = value; }
		}

		public int A1LocID
		{
			get { return _a1LocationID; }
			set { _a1LocationID = value; }
		}

		public string A1Direction
		{
			get { return _a1Direction; }
			set {_a1Direction = value; }
		}

		public int A1SecLocID
		{
			get { return _a1SecLocationID; }
			set { _a1SecLocationID = value; }
		}

		public string A1SecDirection
		{
			get { return _a1SecDirection; }
			set {_a1SecDirection = value; }
		}

		public DateTime DownloadStartTime
		{
			get { return _downloadStartTime; }
			set { _downloadStartTime = value; }
		}

		public int DownloadInterval
		{
			get { return _downloadInterval; }
			set { _downloadInterval = value; }
		}

		public int DownloadEraseCounter
		{
			get { return _downloadEraseCounter; }
			set { _downloadEraseCounter = value; }
		}

		public string Status
		{
			get { return _status; }
			set { _status = value; }
		}

		public DateTime LastReadTime
		{
			get { return _LastReadTime; }
			set {_LastReadTime = value; }
		}

        public int MemoryOccupation
        {
            get { return _currerntMemoryOccupation; }
            set { _currerntMemoryOccupation = value; }
        }

        public ReaderTO()
        { }
	}
}
