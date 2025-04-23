using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class EmployeeHistTO
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
		private int _accessGroupID = -1; // 17/05/2007, Bilja, this property is not in the constructors, use GET/SET methods
        private int _employeeTypeID = -1;
        private int _orgUnitID = -1;
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        private string _modifiedBy = "";
        private DateTime _modifiedTime = new DateTime();
        private DateTime _validTo = new DateTime();

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

        public DateTime ModifiedTime
        {
            get { return _modifiedTime; }
            set { _modifiedTime = value; }
        }

        public string ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
        }

        public DateTime ValidTo
        {
            get { return _validTo; }
            set { _validTo = value; }
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
                
		public int EmployeeID
		{
			get{ return _employeeID; }
			set{ _employeeID = value; }
		}

        public string FirstName
		{
			get { return _firstName; }
			set {_firstName = value; }
		}
                
		public string LastName
		{
			get { return _lastName; }
			set { _lastName = value; }
		}
                
        public string FirstAndLastName
        {
            get { return _lastName + " " +_firstName; }
        }
        
		public int WorkingUnitID
		{
			get { return _workingUnitID; }
			set {_workingUnitID = value; }
		}
                
		public string Status
		{
			get { return _status; }
			set {_status = value; }
		}

		public string Password
		{
			get { return _password; }
			set {_password = value; }
		}
                
		public int AddressID
		{
			get{ return _addressID; }
			set{ _addressID = value; }
		}
                
		public string Picture
		{
			get { return _picture; }
			set {_picture = value; }
		}
                
		public int WorkingGroupID
		{
			get{ return _workingGroupID; }
			set{ _workingGroupID = value; }
		}
                
		public string Type
		{
			get { return _type; }
			set {_type = value; }
		}
                
		public int AccessGroupID
		{
			get{ return _accessGroupID; }
			set{ _accessGroupID = value; }
		}
                

        public EmployeeHistTO()
        { }
        
        public EmployeeHistTO(EmployeeTO emplTO)
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
