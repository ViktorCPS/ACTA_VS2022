using System;
using System.Globalization;
using System.Text;

using System.Xml;
using System.Xml.Serialization;

namespace TransferObjects
{
    /// <summary>
    /// Summary description for PassTO.
    /// </summary>
    [XmlRootAttribute()]
    public class PassHistTO
    {
        string dateTimeformat = "";

        private int _recID = -1;
        private int _passID = -1;
        private int _employeeID = -1;
        private string _direction = "";
        private DateTime _eventTime = new DateTime();
        private string _eventTimeString = "";
        private int _passTypeID = -1;
        private int _isWrkHrsCount = -1;
        private int _locationID = -1;
        private int _pairGenUsed = -1;
        private int _manualyCreated = -1;
        private string _remarks= "";
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        private string _createdTimeString = ""; 
        private string _modifiedBy = "";
        private DateTime _modifiedTime = new DateTime();
        //private string _modifiedTimeString = "";
        private string _employeeName = "";
        private string _passType = "";
        private string _locationName = "";
        private string _WUName = "";

        [XmlAttributeAttribute(AttributeName = "PassID")]
        public int RecID
        {
            get { return _recID; }
            set { _recID = value; }
        }
        public int PassID
        {
            get { return _passID; }
            set { _passID = value; }
        }

        public int EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }

        public string Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        [XmlIgnore]
        public DateTime EventTime
        {
            get { return _eventTime; }
            set
            {
                _eventTime = value;
                EventTimeString = _eventTime.ToString(dateTimeformat);
            }
        }

        [XmlElement("EventTime")]
        public string EventTimeString
        {
            get { return _eventTimeString; }
            set
            {
                _eventTimeString = value;
                _eventTime = Convert.ToDateTime(value);
            }
        }

        public int PassTypeID
        {
            get { return _passTypeID; }
            set { _passTypeID = value; }
        }

        public int PairGenUsed
        {
            get { return _pairGenUsed; }
            set { _pairGenUsed = value; }
        }

        public int LocationID
        {
            get { return _locationID; }
            set { _locationID = value; }
        }

        public int ManualyCreated
        {
            get { return _manualyCreated; }
            set { _manualyCreated = value; }
        }

        public int IsWrkHrsCount
        {
            get { return _isWrkHrsCount; }
            set { _isWrkHrsCount = value; }
        }

        public string Remarks
        {
            get { return _remarks; }
            set { _remarks = value; }
        }

        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        public DateTime CreatedTime
        {
            get { return _createdTime; }
            set
            {
                _createdTime= value;
                CreatedTimeString = _createdTime.ToString(dateTimeformat);
            }
        }

       public string CreatedTimeString
        {
            get { return _createdTimeString; }
            set
            {
                _createdTimeString = value;
                _createdTime = Convert.ToDateTime(value);
            }
        }

        public string ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
        }

        public DateTime ModifiedTime
        { 
            get { return _modifiedTime; }
            set 
            {
                _modifiedTime = value;
                //ModifiedTimeString = Convert.ToString(dateTimeformat); 
            }
        }

      /*  public string ModifiedTimeString
        {
            get { return _modifiedTimeString; }
            set
            {
                _modifiedTimeString = value;
                _modifiedTime = Convert.ToDateTime(value);
            }
        }*/

        public string EmployeeName
        {
            get { return _employeeName; }
            set { _employeeName = value; }
        }

        public string PassType
        {
            get { return _passType; }
            set { _passType = value; }
        }

        public string LocationName
        {
            get { return _locationName; }
            set { _locationName = value; }
        }

        public string WUName
        {
            get { return _WUName; }
            set { _WUName = value; }
        }

        public PassHistTO(int recID,int passID, int employeeID,
            string direction, DateTime eventTime, int passTypeID, int isWrkHrsCount,
            int locationID, int pairGenUsed,  int manualCreated,  string remarks,
            string createdBy, DateTime createdTime, string modifiedBy, DateTime modifiedTime)
		{
            this.RecID = recID;
			this.PassID = passID;
			this.EmployeeID = employeeID;
			this.Direction = direction;
			this.EventTime = eventTime;
			this.PassTypeID = passTypeID;
			this.PairGenUsed = pairGenUsed;
			this.LocationID = locationID;
			this.ManualyCreated = manualCreated;
			this.IsWrkHrsCount = isWrkHrsCount;
            this.Remarks = remarks;
            this.CreatedBy = createdBy;
            this.CreatedTime = createdTime;
            this.ModifiedBy = modifiedBy;
            this.ModifiedTime = modifiedTime;
			
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;	
		}	

        public PassHistTO()
		{
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;	
		}
    }
}
