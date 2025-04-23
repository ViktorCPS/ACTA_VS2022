using System;
using System.Xml;
using System.Xml.Serialization;
using System.Globalization;

namespace TransferObjects
{
	/// <summary>
	/// Summary description for VisitTO.
	/// </summary>
	[XmlRootAttribute()]
	public class VisitTO
	{
		string dateTimeFormat = "";	

		private int _visitID;
		private int _employeeID;
		private string _firstName;
		private string _lastName;
		private string _visitorJMBG;
		private string _visitorID;
		private DateTime _dateStart;
		private string _dateStartString;
		private DateTime _dateEnd;
		private string _dateEndString;
		private int _visitedPerson;
		private int _visitedWorkingUnit;
		private string _visitDescr;
		private int _locationID;
		private string _remarks;
        private string _employeeFirstName;
        private string _employeeLastName;
        private string _visitedFirstName;
        private string _visitedLastName;
        private string _wuName;
        private string _company;

        public string Company
        {
            get { return _company; }
            set { _company = value; }
        }

		public int VisitID
		{
			get{ return _visitID; }
			set{ _visitID = value; }
		}

		public int EmployeeID
		{
			get{ return _employeeID; }
			set{ _employeeID = value; }
		}

		public string FirstName
		{
			get{ return _firstName; }
			set{ _firstName = value; }
		}

		public string LastName
		{
			get{ return _lastName; }
			set{ _lastName = value; }
		}

		public string VisitorJMBG
		{
			get{ return _visitorJMBG; }
			set{ _visitorJMBG = value; }
		}

		public string VisitorID
		{
			get{ return _visitorID; }
			set{ _visitorID = value; }
		}
		
		public DateTime DateStart
		{
			get{ return _dateStart; }
			set
			{
				_dateStart = value; 
				DateStartStringValue = _dateStart.ToString(dateTimeFormat);
			}
		}

		public string DateStartStringValue
		{
			get
			{ 
				return _dateStartString;
			}
			set
			{
				_dateStartString = value;
				_dateStart = Convert.ToDateTime(value);
			}
		}
		
		public DateTime DateEnd
		{
			get{ return _dateEnd; }
			set	
			{
				_dateEnd = value; 
				DateEndStringValue = _dateEnd.ToString(dateTimeFormat);
			}
		}

		public string DateEndStringValue
		{
			get 
			{ 
				return _dateEndString; 
			}
			set 
			{ 
				_dateEndString = value;
				_dateEnd = Convert.ToDateTime(value);
			}
		}

		public int VisitedPerson
		{ 
			get { return _visitedPerson; }
			set { _visitedPerson = value; }
		}

		public int VisitedWorkingUnit
		{
			get{ return _visitedWorkingUnit; }
			set{ _visitedWorkingUnit = value; }
		}

		public string VisitDescr
		{
			get{ return _visitDescr; }
			set{ _visitDescr = value; }
		}

		public int LocationID
		{
			get{ return _locationID; }
			set{ _locationID = value; }
		}

		public string Remarks
		{
			get{ return _remarks; }
			set{ _remarks = value; }
		}

        public string EmployeeFirstName
		{
			get{ return _employeeFirstName; }
			set{ _employeeFirstName = value; }
		}

        public string EmployeeLastName
		{
			get{ return _employeeLastName; }
			set{ _employeeLastName = value; }
		}

        public string VisitedFirstName
		{
			get{ return _visitedFirstName; }
			set{ _visitedFirstName = value; }
		}

        public string VisitedLastName
		{
			get{ return _visitedLastName; }
			set{ _visitedLastName = value; }
		}

        public string WUName
		{
			get{ return _wuName; }
			set{ _wuName = value; }
		}

		public VisitTO()
		{
			this.VisitID = -1;
			this.EmployeeID = -1;
			this.FirstName = "";
			this.LastName = "";
			this.VisitorJMBG = "";
			this.VisitorID = "";
			this.DateStart = new DateTime();
			this.DateEnd = new DateTime();
			this.VisitedPerson = -1;
			this.VisitedWorkingUnit = -1;
			this.VisitDescr = "";
			this.LocationID = -1;
			this.Remarks = "";
            this.EmployeeFirstName = "";
            this.EmployeeLastName = "";
            this.VisitedFirstName = "";
            this.VisitedLastName = "";
            this.WUName = "";
            this.Company = "";

			DateTimeFormatInfo dateTimeFormatInfo = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeFormat = dateTimeFormatInfo.SortableDateTimePattern;	
		}
		public VisitTO(int visitID, int employeeID, string firstName, string lastName, string visitorJMBG, string visitorID, 
			DateTime dateStart, DateTime dateEnd, int visitedPerson, int visitedWorkingUnit, string visitDescr, int locationID,
            string remarks, string employeeFirstName, string employeeLastName, string visitedFirstName, string visitedLastName,
            string wuName)
		{
			this.VisitID = visitID;
			this.EmployeeID = employeeID;
			this.FirstName = firstName;
			this.LastName = lastName;
			this.VisitorJMBG = visitorJMBG;
			this.VisitorID = visitorID;
			this.DateStart = dateStart;
			this.DateEnd = dateEnd;
			this.VisitedPerson = visitedPerson;
			this.VisitedWorkingUnit = visitedWorkingUnit;
			this.VisitDescr = visitDescr;
			this.LocationID = locationID;
			this.Remarks = remarks;
            this.EmployeeFirstName = employeeFirstName;
            this.EmployeeLastName = employeeLastName;
            this.VisitedFirstName = visitedFirstName;
            this.VisitedLastName = visitedLastName;
            this.WUName = wuName;
            this.Company = "";//15.12.2009 Natasa Company has default value

			DateTimeFormatInfo dateTimeFormatInfo = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeFormat = dateTimeFormatInfo.SortableDateTimePattern;	
		}
	}
}
