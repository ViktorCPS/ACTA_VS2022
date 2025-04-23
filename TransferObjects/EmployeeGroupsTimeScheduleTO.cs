using System;

namespace TransferObjects
{
    /// <summary>
    /// Summary description for EmployeeGroupsTimeScheduleTO.
    /// </summary>
    public class EmployeeGroupsTimeScheduleTO
    {
        private int _employeeGroupID = -1;
		private DateTime _date = new DateTime();
		private int _timeSchemaID = -1;
		private int _startCycleDay = -1;

        public int EmployeeGroupID
		{
            get { return _employeeGroupID; }
            set { _employeeGroupID = value; }
		}

		public DateTime Date
		{
			get { return _date; }
			set {_date = value; }
		}

		public int TimeSchemaID
		{
			get{ return _timeSchemaID; }
			set{ _timeSchemaID = value; }
		}

		public int StartCycleDay
		{
			get{ return _startCycleDay; }
			set{ _startCycleDay = value; }
		}

		public EmployeeGroupsTimeScheduleTO()
		{ }

        public EmployeeGroupsTimeScheduleTO(int employeeGroupID, DateTime date, int timeSchemaID, int startCycleDay)
		{
            this.EmployeeGroupID = employeeGroupID;
			this.Date = date;
			this.TimeSchemaID = timeSchemaID;
			this.StartCycleDay = startCycleDay;
		}
    }
}
