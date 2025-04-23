using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class TimeSchemaIntervalLibraryTO
    {
        private int time_schema_interval_id = -1;
        private int time_schema_id = -1;
        string created_by = "";
        DateTime created_time = new DateTime();
        string modified_by = "";
        DateTime modified_time = new DateTime();

        public int TimeSchemaIntervalId
        {
            get { return time_schema_interval_id; }
            set { time_schema_interval_id = value; }
        }

        public int TimeSchemaId
        {
            get { return time_schema_id; }
            set { time_schema_id = value; }
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
