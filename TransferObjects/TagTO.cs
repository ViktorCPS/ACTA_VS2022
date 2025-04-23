using System;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;


namespace TransferObjects
{
	/// <summary>
	/// Used in communication between Common and DataAccess classes
	/// </summary>
	[XmlRootAttribute()]
	public class TagTO
	{
		string dateTimeFormat = "";	

		private int _recordID = -1;
		private uint _tagID = 0;
		private int _ownerID = -1;
		private DateTime _issued = new DateTime();
		private string _issuedString = "";
		private DateTime _validTo = new DateTime();
		private string _validToString = "";
		private string _status = "";
		private string _description = "";
		private string _employeeName = "";
		private string _modifiedBy = "";
		private DateTime _modifiedTime = new DateTime();
		private int _accessGroupID = 0; // 17/05/2007, Bilja, this property is not in the constructors, use GET/SET methods
        private string _createdBy = "";
        private string _workingUnit = "";

        public string WorkingUnit
        {
            get { return _workingUnit; }
            set { _workingUnit = value; }
        }

        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

		public int RecordID
		{
			get{ return _recordID; }
			set{ _recordID = value; }
		}

		[XmlAttributeAttribute(AttributeName="TagID")]
		public uint TagID
		{
			get{ return _tagID; }
			set{ _tagID = value; }
		}

		public int OwnerID
		{
			get{ return _ownerID; }
			set{ _ownerID = value; }
		}
		
		[XmlIgnore]
		public DateTime Issued
		{
			get{ return _issued; }
			set
			{ 
				_issued = value; 
				IssuedString = _issued.ToString(dateTimeFormat);
			}
		}

		[XmlElement("Issued")]
		public string IssuedString
		{
			get{ return Issued.ToString(dateTimeFormat); }
			set{ _issuedString = value; }
		}

		[XmlIgnore]
		public DateTime ValidTO
		{
			get{ return _validTo; }
			set
			{ 
				_validTo = value; 
				ValidTOString = _validTo.ToString(dateTimeFormat);
			}
		}
		[XmlElement("ValidTO")]
		public string ValidTOString
		{
			get { return ValidTO.ToString(dateTimeFormat); }
			set { _validToString = value; }
		}

		public string Status
		{
			get{ return _status; }
			set{ _status = value; }
		}

		public string Description
		{
			get{ return _description; }
			set{ _description = value; }
		}

		public string EmployeeName
		{
			get{ return _employeeName; }
			set{ _employeeName = value; }
		}

		public string ModifiedBy
		{
			get{ return _modifiedBy; }
			set{ _modifiedBy = value; }
		}

		public DateTime ModifiedTime
		{
			get{ return _modifiedTime; }
			set{ _modifiedTime = value; }
		}

		public int AccessGroupID
		{
			get{ return _accessGroupID; }
			set{ _accessGroupID = value; }
		}

		public TagTO()
		{
			DateTimeFormatInfo dateTimeFormatInfo = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeFormat = dateTimeFormatInfo.SortableDateTimePattern;	
		}

		public TagTO(int recordID, uint tagID, int ownerID, string status, string description)
		{
			this.RecordID = recordID;
			this.TagID = tagID;
			this.OwnerID = ownerID;
			this.Status = status;
			this.Issued = new DateTime();
			this.ValidTO = new DateTime();
			this.Description = description;

			DateTimeFormatInfo dateTimeFormatInfo = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeFormat = dateTimeFormatInfo.SortableDateTimePattern;	
		}	

		public TagTO(int recordID, uint tagID, int ownerID, string status, string description, int accessGroupID)
		{
			this.RecordID = recordID;
			this.TagID = tagID;
			this.OwnerID = ownerID;
			this.Status = status;
			this.Issued = new DateTime();
			this.ValidTO = new DateTime();
			this.Description = description;
			this.AccessGroupID = accessGroupID;

			DateTimeFormatInfo dateTimeFormatInfo = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeFormat = dateTimeFormatInfo.SortableDateTimePattern;	
		}
	}
}
