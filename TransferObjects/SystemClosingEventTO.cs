using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class SystemClosingEventTO
    {
        private int _eventID = -1;        
        private string _type = "";       
        private DateTime _startTime = new DateTime();
        private DateTime _endTime = new DateTime();
        private string _messageSR = "";
        private string _messageEN = "";
        private string _dpEngineState = "";
        private DateTime _dpEngineStateModifiedTime = new DateTime();
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        private string _modifiedBy = "";
        private DateTime _modifiedTime = new DateTime();

        public int EventID
        {
            get { return _eventID; }
            set { _eventID = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
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

        public string MessageSR
        {
            get { return _messageSR; }
            set { _messageSR = value; }
        }

        public string MessageEN
        {
            get { return _messageEN; }
            set { _messageEN = value; }
        }

        public string DPEngineState
        {
            get { return _dpEngineState; }
            set { _dpEngineState = value; }
        }

        public DateTime DPEngineStateModifiedTime
        {
            get { return _dpEngineStateModifiedTime; }
            set { _dpEngineStateModifiedTime = value; }
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

        public string ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
        }

        public DateTime ModifiedTime
        {
            get { return _modifiedTime; }
            set { _modifiedTime = value; }
        }
    }
}
