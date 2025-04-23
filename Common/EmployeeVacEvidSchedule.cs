using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Runtime.Serialization;
using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
    public class EmployeeVacEvidSchedule
    {
        DebugLog log;

        EmployeeVacEvidScheduleTO scheduleTO = new EmployeeVacEvidScheduleTO();

        public EmployeeVacEvidScheduleTO ScheduleTO
        {
            get { return scheduleTO; }
            set { scheduleTO = value; }
        }
       
        public EmployeeVacEvidSchedule()
        {
            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
        }
    }
}