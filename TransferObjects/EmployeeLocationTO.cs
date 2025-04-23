using System;

namespace TransferObjects
{
	/// <summary>
	/// Summary description for EmployeeLocationTO.
	/// </summary>
	public class EmployeeLocationTO
	{
		private int _employeeID = -1;
		private int _readerID = -1;
		private int _antenna = -1;
		private int _passTypeID = -1;
		private DateTime _eventTime = new DateTime();
		private int _locationID = -1;
        private int _wuID = -1;
		private string _wuName = "";
		private string _locName = "";
		private string _passType = "";
		private string _employeeName = "";
        private int _type = -1;

        public int Type
        {
            get { return _type; }
            set { _type = value; }
        }

		public int EmployeeID
		{
			get{ return _employeeID; }
			set{ _employeeID = value; }
		}

		public int ReaderID
		{
			get{ return _readerID; }
			set{ _readerID = value; }
		}

		public int Antenna
		{
			get{ return _antenna; }
			set{ _antenna = value; }
		}

		public int PassTypeID
		{
			get{ return _passTypeID; }
			set{ _passTypeID = value; }
		}

		public DateTime EventTime
		{
			get{ return _eventTime; }
			set{ _eventTime = value; }
		}

		public int LocationID
		{
			get{ return _locationID; }
			set{ _locationID = value; }
		}

        public int WUID
        {
            get { return _wuID; }
            set { _wuID = value; }
        }
		
		public string WUName
		{
			get{ return _wuName; }
			set{ _wuName = value; }
		}

		public string LocName
		{
			get{ return _locName; }
			set{ _locName = value; }
		}

		public string PassType
		{
			get{ return _passType; }
			set{ _passType = value; }
		}

		public string EmployeeName
		{
			get{ return _employeeName; }
			set{ _employeeName = value; }
		}

		public EmployeeLocationTO(int employeeID, int readerID, int antenna, int passTypeID, DateTime eventTime, int locationID)
		{			
			this.EmployeeID = employeeID;
			this.ReaderID = readerID;
			this.Antenna = antenna;
			this.PassTypeID = passTypeID;
			this.EventTime = eventTime;
			this.LocationID = locationID;			
		}

        public EmployeeLocationTO()
        { }
	}
}
