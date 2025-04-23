using System;
using System.Collections;

namespace TransferObjects
{
	/// <summary>
	/// WorkTimeIntervalTO contains data from time_schema_dtl table.
	/// </summary>
	public class WorkTimeIntervalTO
	{
		// Schema ID that this day belongs
		private int _timeSchemaID = -1;
		// Order number for this day in TimeSchema
		private int _dayNum = -1;
		// Order number of time interval in day 
		private int _intervalNum = -1;

		private DateTime _startTime = new DateTime();
		private DateTime _endTime = new DateTime();
		private DateTime _earliestArrivedTime = new DateTime();
		private DateTime _latestArrivedTime = new DateTime();
		private DateTime _earliestLeftTime = new DateTime();
		private DateTime _latestLeftTime = new DateTime();
		private int _autoClose = -1;
		private int _pauseID = -1;
        private string _description = "";

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

		public int TimeSchemaID
		{
			get { return _timeSchemaID; }
			set { _timeSchemaID = value; }
		}

		public int DayNum
		{
			get { return _dayNum; }
			set { _dayNum = value; }
		}

		public int IntervalNum
		{
			get { return _intervalNum; }
			set { _intervalNum = value; }
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
		
		public DateTime EarliestArrived
		{
			get { return _earliestArrivedTime; }
			set { _earliestArrivedTime = value; }
		}

		public DateTime LatestArrivaed
		{
			get { return _latestArrivedTime; }
			set { _latestArrivedTime = value; }
		}

		public DateTime EarliestLeft
		{
			get { return _earliestLeftTime; }
			set { _earliestLeftTime = value; }
		}

		public DateTime LatestLeft
		{
			get { return _latestLeftTime; }
			set { _latestLeftTime = value; }
		}

		public int AutoClose
		{
			get { return _autoClose; }
			set { _autoClose = value; }
		}

		public int PauseID
		{
			get { return _pauseID; }
			set { _pauseID = value; }
		}

		public WorkTimeIntervalTO()
		{
		}
        
		public WorkTimeIntervalTO Clone()
		{
			WorkTimeIntervalTO newDay = new WorkTimeIntervalTO();

			newDay.TimeSchemaID = this.TimeSchemaID;
			newDay.DayNum = this.DayNum;
			newDay.IntervalNum = this.IntervalNum;

			newDay.EarliestArrived = this.EarliestArrived;
			newDay.StartTime = this.StartTime;
			newDay.LatestArrivaed = this.LatestArrivaed;
			newDay.EarliestLeft = this.EarliestLeft;
			newDay.EndTime = this.EndTime;
			newDay.LatestLeft = this.LatestLeft;
			newDay.AutoClose = this.AutoClose;
			newDay.PauseID = this.PauseID;

			return newDay;
		}
	}
}
