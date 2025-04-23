using System;
using System.Collections;
using System.Configuration;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
	/// <summary>
	/// TimeSchemaInterval defines one day in the TimeSchema, geting data from 
	/// timeSchemaDetails database table;
	/// </summary>
	public class TimeSchemaInterval
	{
		// Debug log
		DebugLog log;

        WorkTimeIntervalTO intervalTO = new WorkTimeIntervalTO();

		public WorkTimeIntervalTO IntervalTO
		{
			get { return intervalTO; }
			set { intervalTO = value; }
		}
        
		//TODO : add four more fileds
		public TimeSchemaInterval()
		{
			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);
		}

		public TimeSchemaInterval(WorkTimeIntervalTO timeSchemaInterval)
		{
			this.IntervalTO.TimeSchemaID = timeSchemaInterval.TimeSchemaID;
            this.IntervalTO.DayNum = timeSchemaInterval.DayNum;
            this.IntervalTO.IntervalNum = timeSchemaInterval.IntervalNum;

            this.IntervalTO.EarliestArrived = new DateTime(timeSchemaInterval.EarliestArrived.Ticks);
            this.IntervalTO.StartTime = new DateTime(timeSchemaInterval.StartTime.Ticks);
            this.IntervalTO.LatestArrivaed = new DateTime(timeSchemaInterval.LatestArrivaed.Ticks);

            this.IntervalTO.EarliestLeft = new DateTime(timeSchemaInterval.EarliestLeft.Ticks);
            this.IntervalTO.EndTime = new DateTime(timeSchemaInterval.EndTime.Ticks);
            this.IntervalTO.LatestLeft = new DateTime(timeSchemaInterval.LatestLeft.Ticks);

            this.IntervalTO.AutoClose = timeSchemaInterval.AutoClose;
            this.IntervalTO.PauseID = timeSchemaInterval.PauseID;
		}
	}
}
