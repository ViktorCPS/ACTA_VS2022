using System;
using System.Runtime.Serialization;

using System.Xml;
using System.Xml.Serialization;

namespace TransferObjects
{
	/// <summary>
	/// This is Transfer Object used by the EmployeeDAO to send and receive 
	/// data from the Employee class.
	/// </summary>

    // [DataContract] & [DataMemeber] attributes from System.Runtime.Serialization put here for WCF service in ACTAWeb solution purposes only
    [DataContract]
	[XmlRootAttribute()]
	public class EmployeeTO
	{
		private int _employeeID = -1;
		private string _firstName = "";
		private string _lastName = "";
		private int _workingUnitID = -1;
		private string _status = "";
		private string _password = "";
		private int _addressID = -1;
		private string _picture = "";
		private int _workingGroupID = -1;
		private string _type = "";
		private string _workingUnitName = "";
		private bool _hasTag = false;
		private string _schemaName = "";
		private DateTime _scheduleDate = new DateTime();
        private string _mealTypeName = "";
		private int _accessGroupID = -1; // 17/05/2007, Bilja, this property is not in the constructors, use GET/SET methods
        private uint _tagID = 0;
        private object _tag = new object(); //18.11.2009 Natasa, this property use for any object needed
        private int _employeeTypeID = -1;
        private int _orgUnitID = -1;
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        private string _modifiedBy = "";
        private string _orgUnitName, _division = ""; //    10.06.2019. BOJAN
        private int _vacationThisYear, _vacationLastYear, _vacationUsed = -1;    //10.06.2019. BOJAN

        public DateTime CreatedTime
        {
            get { return _createdTime; }
            set { _createdTime = value; }
        }

        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }


        public int EmployeeTypeID
        {
            get { return _employeeTypeID; }
            set { _employeeTypeID = value; }
        }

        public int OrgUnitID
        {
            get { return _orgUnitID; }
            set { _orgUnitID = value; }
        }

        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }        

        [DataMember]
		[XmlAttributeAttribute(AttributeName="EmployeeID")]
		public int EmployeeID
		{
			get{ return _employeeID; }
			set{ _employeeID = value; }
		}

        [DataMember]
        [XmlAttributeAttribute(AttributeName = "TagID")]
        public uint TagID
        {
            get { return _tagID; }
            set { _tagID = value; }
        }

        [DataMember]
		public string FirstName
		{
			get { return _firstName; }
			set {_firstName = value; }
		}

        [DataMember]
		public string LastName
		{
			get { return _lastName; }
			set { _lastName = value; }
		}

        [DataMember]
        public string FirstAndLastName
        {
            get { return _lastName + " " +_firstName; }
        }

        public string IDLastNameFirstName
        {
            get { return ((_employeeID != -1 ? _employeeID.ToString().Trim() + " - " : "") + _lastName  + " " + _firstName).Trim(); }
        }

        public string FirstNameLastNameID
        {
            get { return (_firstName + " " + _lastName + (_employeeID != -1 ? " - " + _employeeID.ToString().Trim() : "")).Trim(); }
        }

        public string LastNameFirstNameID
        {
            get { return (_lastName + " " + _firstName + (_employeeID != -1 ? " - " + _employeeID.ToString().Trim() : "")).Trim(); }
        }

        [DataMember]
		public int WorkingUnitID
		{
			get { return _workingUnitID; }
			set {_workingUnitID = value; }
		}

        [DataMember]
		public string Status
		{
			get { return _status; }
			set {_status = value; }
		}

        [DataMember]
		public string Password
		{
			get { return _password; }
			set {_password = value; }
		}

        [DataMember]
		public int AddressID
		{
			get{ return _addressID; }
			set{ _addressID = value; }
		}

        [DataMember]
		public string Picture
		{
			get { return _picture; }
			set {_picture = value; }
		}

        [DataMember]
		public int WorkingGroupID
		{
			get{ return _workingGroupID; }
			set{ _workingGroupID = value; }
		}

        [DataMember]
		public string Type
		{
			get { return _type; }
			set {_type = value; }
		}

        [DataMember]
		public string WorkingUnitName
		{
			get{ return _workingUnitName; }
			set{ _workingUnitName = value; }
		}

        [DataMember]
        public string OrgUnitName {
            get { return _orgUnitName; }
            set { _orgUnitName = value; }
        }

        [DataMember]
        public string Division {
            get { return _division; }
            set { _division = value; }
        }

        [DataMember]
        public int VacationThisYear
		{
            get { return _vacationThisYear; }
            set { _vacationThisYear = value; }
		}

        [DataMember]
        public int VacationLastYear
		{
            get { return _vacationLastYear; }
            set { _vacationLastYear = value; }
		}

        [DataMember]
        public int VacationUsed
		{
            get { return _vacationUsed; }
            set { _vacationUsed = value; }
		}

        [DataMember]
		public bool HasTag
		{
			get{ return _hasTag; }
			set{ _hasTag = value; }
		}

        [DataMember]
		public string SchemaName
		{
			get{ return _schemaName; }
			set{ _schemaName = value; }
		}

        [DataMember]
		public DateTime ScheduleDate
		{
			get{ return _scheduleDate; }
			set{ _scheduleDate = value; }
		}

        [DataMember]
		public int AccessGroupID
		{
			get{ return _accessGroupID; }
			set{ _accessGroupID = value; }
		}

        [DataMember]
        public string MealTypeName
        {
            get { return _mealTypeName; }
            set { _mealTypeName = value; }
        }

        public string ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
        }

        public EmployeeTO()
        { }

		public EmployeeTO(int employeeID, string firstName, string lastName, int workingUnitId, 
			string status, string password, int addressID, string picture, int workingGroupID, string type)
		{
			this.EmployeeID = employeeID;
			this.FirstName = firstName;
			this.LastName = lastName;
			this.WorkingUnitID = workingUnitId;
			this.Status = status;
			this.Password = password;
			this.AddressID = addressID;
			this.Picture = picture;
			this.WorkingGroupID = workingGroupID;
			this.Type = type;
		}

        public EmployeeTO(EmployeeTO emplTO)
        {
            this.AccessGroupID = emplTO.AccessGroupID;
            this.AddressID = emplTO.AddressID;
            this.CreatedBy = emplTO.CreatedBy;
            this.CreatedTime = emplTO.CreatedTime;
            this.EmployeeID = emplTO.EmployeeID;
            this.EmployeeTypeID = emplTO.EmployeeTypeID;
            this.FirstName = emplTO.FirstName;
            this.LastName = emplTO.LastName;
            this.OrgUnitID = emplTO.OrgUnitID;
            this.Password = emplTO.Password;
            this.Picture = emplTO.Picture;
            this.Status = emplTO.Status;
            this.Type = emplTO.Type;
            this.WorkingGroupID = emplTO.WorkingGroupID;
            this.WorkingUnitID = emplTO.WorkingUnitID;
        }
	}
}
