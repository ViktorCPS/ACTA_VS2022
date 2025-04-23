using System;
using System.Globalization;
using System.Text;

using System.Xml;
using System.Xml.Serialization;

namespace TransferObjects
{
	/// <summary>
	/// Summary description for PassTO.
	/// </summary>
	[XmlRootAttribute()]
	public class PassTO
	{
		string dateTimeformat = "";	

		private int _passID = -1;
		private int _employeeID = -1;
		private string _direction = "";
		private DateTime _eventTime = new DateTime();
		private string _eventTimeString = "";
		private int _passTypeID = -1;
		private int _pairGenUsed = -1;
		private int _locationID = -1;
		private int _isWrkHrsCount = -1;
		private int _manualyCreated = -1;
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
		private string _employeeName = "";
		private string _passType = "";
		private string _locationName = "";
        private string _WUName = "";
        private int _gateID = -1;
        private string _gateName = "";
        //28.04.2010. Natasa ****for email notification only
        private string email = "";
        private string status = "";
        private string description = "";

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        //****for email notification only
		[XmlAttributeAttribute(AttributeName="PassID")]
		public int PassID
		{
			get{ return _passID; }
			set{ _passID = value; }
		}

		public int EmployeeID
		{
			get{ return _employeeID; }
			set{ _employeeID = value; }
		}

        public int GateID
        {
            get { return _gateID; }
            set { _gateID = value; }
        }

        public string GateName
        {
            get { return _gateName; }
            set { _gateName = value; }
        }

		public string Direction
		{
			get{ return _direction; }
			set{ _direction = value; }
		}

		[XmlIgnore]
		public DateTime EventTime
		{
			get{ return _eventTime; }
			set
			{ 
				_eventTime = value; 
				EventTimeString = _eventTime.ToString(dateTimeformat);
			}
		}

		[XmlElement("EventTime")]
		public string EventTimeString
		{
			get { return _eventTimeString; }
			set
			{
				_eventTimeString = value;
				_eventTime = Convert.ToDateTime(value);
			}
		}

		public int PassTypeID
		{
			get{ return _passTypeID; }
			set{ _passTypeID = value; }
		}

		public int PairGenUsed
		{
			get{ return _pairGenUsed; }
			set{ _pairGenUsed = value; }
		}

		public int LocationID
		{
			get{ return _locationID; }
			set{ _locationID = value; }
		}

		public int ManualyCreated
		{
			get{ return _manualyCreated; }
			set{ _manualyCreated = value; }
		}

        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        public DateTime CreatedTime
        {
            get { return _createdTime; }
            set { _createdTime = value; }
        }

		public int IsWrkHrsCount
		{ 
			get { return _isWrkHrsCount; }
			set { _isWrkHrsCount = value; }
		}

		public string EmployeeName
		{
			get{ return _employeeName; }
			set{ _employeeName = value; }
		}

		public string PassType
		{
			get{ return _passType; }
			set{ _passType = value; }
		}

		public string LocationName
		{
			get{ return _locationName; }
			set{ _locationName = value; }
		}

        public string WUName
		{
            get { return _WUName; }
            set { _WUName = value; }
		}

		public PassTO(int passID, int employeeID, 
			string direction, DateTime eventTime, int passTypeID, int pairGenUsed, 
			int locationID, int manualCreated, string createdBy, DateTime createdTime, int isWrkHrsCount)
		{
			this.PassID = passID;
			this.EmployeeID = employeeID;
			this.Direction = direction;
			this.EventTime = eventTime;
			this.PassTypeID = passTypeID;
			this.PairGenUsed = pairGenUsed;
			this.LocationID = locationID;
			this.ManualyCreated = manualCreated;
            this.CreatedBy = createdBy;
            this.CreatedTime = createdTime;  
			this.IsWrkHrsCount = isWrkHrsCount;
			
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;	
		}

        public PassTO(int passID, int employeeID,
        string direction, DateTime eventTime, int passTypeID, int pairGenUsed,
        int locationID, int manualCreated, int isWrkHrsCount)
        {
            this.PassID = passID;
            this.EmployeeID = employeeID;
            this.Direction = direction;
            this.EventTime = eventTime;
            this.PassTypeID = passTypeID;
            this.PairGenUsed = pairGenUsed;
            this.LocationID = locationID;
            this.ManualyCreated = manualCreated;
            this.CreatedBy = "";
            this.CreatedTime = new DateTime();
            this.IsWrkHrsCount = isWrkHrsCount;

            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }	

		public PassTO()
		{			
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;	
		}
	
		/// <summary>
		/// Return current object properties.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("\n ---- PassTO Object Data: ----");
			sb.Append("\nPassID : " + this.PassID.ToString());
			sb.Append("\nEmployeeID : " + this.EmployeeID.ToString());
			sb.Append("\nDirection : " + this.Direction.ToString());
			sb.Append("\nEventTime : " + this.EventTime.ToString());
			sb.Append("\nPassTypeID : " + this.PassTypeID.ToString());
			sb.Append("\nPairGenUsed : " + this.PairGenUsed.ToString());
			sb.Append("\nLocationID : " + this.LocationID.ToString());
			sb.Append("\nManualyCreated : " + this.ManualyCreated.ToString());
            sb.Append("\nCreatedBy : " + this.CreatedBy.ToString());
            sb.Append("\nCreatedTime : " + this.CreatedTime.ToString());
			sb.Append("\nIsWrkHrsCount : " + this.IsWrkHrsCount.ToString());
			sb.Append("\nEmployeeName : " + this.EmployeeName.ToString());
			sb.Append("\nPassType : " + this.PassType.ToString());
			sb.Append("\nLocationName : " + this.LocationName.ToString());
			sb.Append("\n --------------------------");

			return sb.ToString ();
		}
	}
}
