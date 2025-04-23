using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class TimeSchemaIntervalLibraryDtlTO
    {
        int time_schema_interval_id = -1;
        DateTime modified_time = new DateTime();
        DateTime start_time = new DateTime();
        DateTime end_time = new DateTime();
        string created_by = "";
        DateTime created_time = new DateTime();
        string modified_by = "";

        public int TimeSchemaIntervalId
        {
            get { return time_schema_interval_id; }
            set { time_schema_interval_id = value; }
        }

        public DateTime StartTime
        {
            get { return start_time; }
            set { start_time = value; }
        }
        public DateTime EndTime
        {
            get { return end_time; }
            set { end_time = value; }
        }
        public string CreatedBy
        {
            get { return created_by; }
            set { created_by = value; }
        }
        public DateTime CreatedTime
        {
            get { return created_time; }
            set { created_time = value; }
        }
        public string ModifiedBy
        {
            get { return modified_by; }
            set { modified_by = value; }
        }

        public DateTime ModifiedTime
        {
            get { return modified_time; }
            set { modified_time = value; }
        }
    }
}
