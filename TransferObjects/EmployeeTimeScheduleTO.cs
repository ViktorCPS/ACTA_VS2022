using System;

namespace TransferObjects
{
	/// <summary>
	/// Summary description for EmployeeTimeScheduleTO.
	/// </summary>
	public class EmployeeTimeScheduleTO
	{
		private int _employeeID = -1;
		private DateTime _date = new DateTime();
		private int _timeSchemaID = -1;
		private int _startCycleDay = -1;
		
		public int EmployeeID
		{
			get{ return _employeeID; }
			set{ _employeeID = value; }
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

		public EmployeeTimeScheduleTO()
		{ }

		public EmployeeTimeScheduleTO(int employeeID, DateTime date, int timeSchemaID, int startCycleDay)
		{
			this.EmployeeID = employeeID;
			this.Date = date;
			this.TimeSchemaID = timeSchemaID;
			this.StartCycleDay = startCycleDay;
		}
	}
}
