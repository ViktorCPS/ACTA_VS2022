using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class SystemMessageTO
    {
        private int _messageID = -1;
        private int _workingUnitID = -1;
        private int _applUserCategoryID = -1;
        private DateTime _startTime = new DateTime();
        private DateTime _endTime = new DateTime();
        private string _messageSR = "";
        private string _messageEN = "";
        private string _company = "";
        private string _role = "";
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        private string _modifiedBy = "";
        private DateTime _modifiedTime = new DateTime();

        public int MessageID
        {
            get { return _messageID; }
            set { _messageID = value; }
        }

        public int WorkingUnitID
        {
            get { return _workingUnitID; }
            set { _workingUnitID = value; }
        }

        public int ApplUserCategoryID
        {
            get { return _applUserCategoryID; }
            set { _applUserCategoryID = value; }
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

        public string Company
        {
            get { return _company; }
            set { _company = value; }
        }

        public string Role
        {
            get { return _role; }
            set { _role = value; }
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
