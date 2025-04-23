using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{


  public  class OnlineMealsIntervalsTO
    {
        int rec_id = 0;
        DateTime interval_start_time;
        DateTime interval_end_time;
        string created_by;
        DateTime created_time;
        string modified_by;
        DateTime modified_time;

        public DateTime Modified_time
        {
            get { return modified_time; }
            set { modified_time = value; }
        }

        public string Modified_by
        {
            get { return modified_by; }
            set { modified_by = value; }
        }

        public DateTime Created_time
        {
            get { return created_time; }
            set { created_time = value; }
        }

        public string Created_by
        {
            get { return created_by; }
            set { created_by = value; }
        }

        public DateTime Interval_end_time
        {
            get { return interval_end_time; }
            set { interval_end_time = value; }
        }

        public DateTime Interval_start_time
        {
            get { return interval_start_time; }
            set { interval_start_time = value; }
        }
        public int Rec_id
        {
            get { return rec_id; }
            set { rec_id = value; }
        }

        public OnlineMealsIntervalsTO() {
            created_by = "";
            created_time = new DateTime();
            modified_by = "";
            modified_time = new DateTime();
            interval_start_time = new DateTime();
            interval_end_time = new DateTime();
        }
    }
}
