using System;

using System.Xml;
using System.Xml.Serialization;

using Util;

namespace TransferObjects
{
	/// <summary>
	/// Summary description for ExtraHourUsedTO.
	/// </summary>
	[XmlRootAttribute()]
	public class ExtraHourUsedTO
	{
		private int      _recordID         = -1;
		private int      _employeeID       = -1;
		private DateTime _dateEarned       = new DateTime();
		private DateTime _dateUsed         = new DateTime();
		private int      _extraTimeAmtUsed = -1;
		private DateTime _startTime        = new DateTime();
		private DateTime _endTime          = new DateTime();
		private Int32    _ioPairID         = -1;
		private string   _calculatedTimeAmtUsed = "";
		private int      _employeeUsedSum  = -1;
        private string   _type             = "";
        private string   _createdByName    = "";

		public int RecordID
		{
			get { return _recordID; }
			set { _recordID = value; }
		}

		public int EmployeeID
		{
			get { return _employeeID; }
			set { _employeeID = value; }
		}

		public DateTime DateEarned
		{
			get { return _dateEarned; }
			set { _dateEarned = value; }
		}

		public DateTime DateUsed
		{
			get { return _dateUsed; }
			set { _dateUsed = value; }
		}

		public int ExtraTimeAmtUsed
		{
			get { return _extraTimeAmtUsed; }
			set { _extraTimeAmtUsed = value; }
		}

		public DateTime StartTime
		{
			get { return _startTime; }
			set { _startTime = value; }
		}

		public DateTime EndTime
		{
			get { return _endTime; }
			set { _endTime = value; }
		}

		public Int32 IOPairID
		{
			get { return _ioPairID; }
			set { _ioPairID = value; }
		}

		public string CalculatedTimeAmtUsed
		{
			get { return _calculatedTimeAmtUsed; }
			set { _calculatedTimeAmtUsed = value; }
		}

		public int EmployeeUsedSum
		{
			get { return _employeeUsedSum; }
			set { _employeeUsedSum = value; }
		}

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string CreatedByName
        {
            get { return _createdByName; }
            set { _createdByName = value; }
        }

		public ExtraHourUsedTO()
		{
		}

		public ExtraHourUsedTO(int recordID, int employeeID, DateTime dateEarned, DateTime dateUsed,
            int extraTimeAmtUsed, DateTime startTime, DateTime endTime, Int32 ioPairID, string type, string createdByName)
		{
			this.RecordID         = recordID;
			this.EmployeeID       = employeeID;
			this.DateEarned       = dateEarned;
			this.DateUsed         = dateUsed;
			this.ExtraTimeAmtUsed = extraTimeAmtUsed;
			this.StartTime        = startTime;
			this.EndTime          = endTime;
			this.IOPairID         = ioPairID;			
			this.CalculatedTimeAmtUsed = Util.Misc.transformMinToStringTime(extraTimeAmtUsed);
            this.Type             = type;
            this.CreatedByName    = createdByName;
		}
	}
}
