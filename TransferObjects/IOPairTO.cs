using System;
using System.Text;


namespace TransferObjects
{
	/// <summary>
	/// IOPairsTO Transfer Object class.
	/// These object are used in DAO and Common object
	/// messagaing.
	/// </summary>
	public class IOPairTO
	{
		private int _ioPairId = -1;
		private DateTime _ioPairDate = new DateTime();
		private int _employeeId = -1;
		private int _locationId = -1;
		private int _passTypeId = -1;
		private string _passType = "";
		private int _isWrkHrsCount = -1;
		private DateTime _startTime = new DateTime();
		private DateTime _endTime = new DateTime();
		private int _manualCreated = -1;
		private string _employeeName = "";
		private string _employeeLastName = "";
		private string _locationName = "";
		private string _wuName = "";
        private int _processed = -1;
        private int gateID = -1;
        private string _createdBy = ""; //25.10.2019.   BOJAN
        private DateTime _createdTime = new DateTime(); //25.10.2019.   BOJAN
        private string _time = "";    //  23.01.2020. BOJAN

        public string Time
        {
            get { return _time; }
            set { _time = value; }
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

        public int GateID
        {
            get { return gateID; }
            set { gateID = value; }
        }

        public int ProcessedGenUsed
        {
            get { return _processed; }
            set { _processed = value; }
        }

		public int IOPairID
		{
			get { return _ioPairId; }
			set { _ioPairId = value; }
		}

		public DateTime IOPairDate
		{
			get { return _ioPairDate; }
			set { _ioPairDate = value; }
		}

		public int EmployeeID
		{ 
			get { return _employeeId; }
			set { _employeeId = value; }
		}

		public int LocationID
		{
			get { return _locationId; }
			set { _locationId = value; }
		}

		public int PassTypeID
		{
			get { return _passTypeId; }
			set { _passTypeId = value; }
		}

		public string PassType
		{
			get { return _passType; }
			set { _passType = value; }
		}

		public int IsWrkHrsCount
		{
			get { return _isWrkHrsCount; }
			set { _isWrkHrsCount = value; }
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

		public int ManualCreated
		{ 
			get { return _manualCreated; }
			set { _manualCreated = value; }
		}

		public string EmployeeName
		{ 
			get { return _employeeName; }
			set { _employeeName = value; }
		}

		public string EmployeeLastName
		{ 
			get { return _employeeLastName; }
			set { _employeeLastName = value; }
		}

		public string LocationName
		{ 
			get { return _locationName; }
			set { _locationName = value; }
		}

		public string WUName
		{ 
			get { return _wuName; }
			set { _wuName = value; }
		}

		public IOPairTO()
		{			
		}
       

		public IOPairTO(IOPairTO ioPairTO)
		{
			IOPairID = ioPairTO.IOPairID;
			IOPairDate = new DateTime(ioPairTO.IOPairDate.Ticks);
			EmployeeID = ioPairTO.EmployeeID;
			LocationID = ioPairTO.LocationID;
			PassTypeID = ioPairTO.PassTypeID;
			PassType = ioPairTO.PassType;
			IsWrkHrsCount = ioPairTO.IsWrkHrsCount;
			StartTime = new DateTime(ioPairTO.StartTime.Ticks);
			EndTime = new DateTime(ioPairTO.EndTime.Ticks);
			ManualCreated = ioPairTO.ManualCreated;
			EmployeeName = ioPairTO.EmployeeName;
			EmployeeLastName = ioPairTO.EmployeeLastName;
			LocationName = ioPairTO.LocationName;
			WUName = ioPairTO.WUName;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("\n ---- IOPair Object Data: ----");
			sb.Append("\nIOPairID : " + this.IOPairID.ToString());
			sb.Append("\nIOPairDate : " + this.IOPairDate.ToString());
			sb.Append("\nEmployeeID : " + this.EmployeeID.ToString());
			sb.Append("\nLocationID: " + this.LocationID.ToString());
			sb.Append("\nPassTypeID : " + this.PassTypeID.ToString());
			sb.Append("\nStartTime : " + this.StartTime.ToString());
			sb.Append("\nEndTime : " + this.EndTime.ToString());
			sb.Append("\nManualCreated : " + this.ManualCreated.ToString());
			sb.Append("\n -------------------------- \n");

			return sb.ToString();
		}
        public string CustomToStringForNoStartTime()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("IO Pair ID: " + this.IOPairID.ToString());
            sb.AppendLine("\n");
            sb.AppendLine(" Pass type: " + this.PassTypeID.ToString());
            sb.AppendLine("\n");
            sb.AppendLine(" End time: " + this.EndTime.ToString());
            sb.AppendLine(" Pass type: " + this.PassType.ToString());
            return sb.ToString();
        }
        public string CustomToStringForNoEndTime()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("IO Pair ID: " + this.IOPairID.ToString());
            sb.AppendLine("\n");
            sb.AppendLine(" Pass type: " + this.PassTypeID.ToString());
            sb.AppendLine("\n"); ;
            sb.AppendLine(" Start time: " + this.StartTime.ToString());
            return sb.ToString();
        }
	}
}
