using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class SyncEmployeeTO
    {
        private int _recID = -1;
        private int _employeeID = -1;
        private int _employeeIDOld = -1;
        private string _firstName = "";
        private string _lastName = "";
        private uint _tagID = 0;
        private DateTime _tagIDValidFrom = new DateTime();
        private int _fsUnitID = -1;
        private DateTime _fsUnitIDValidFrom = new DateTime();
        private byte[] _picture = null;
        private int _employeeTypeID = -1;
        private DateTime _employeeTypeIDValidFrom = new DateTime();
        private int _personalHolidayCategory = -1;
        private DateTime _personalHolidayCategoryValidFrom = new DateTime();
        private DateTime _personalHolidayDate = new DateTime();
        private DateTime _personalHolidayDateValidFrom = new DateTime();
        private int _organizationalUnitID = -1;
        private DateTime _organizationalUnitIDValidFrom = new DateTime();
        private int _responsibilityFsUnitID = -1;
        private DateTime _responsibilityFsUnitIDValidFrom = new DateTime();
        private int _responsibilityOuUnitID = -1;
        private DateTime _responsibilityOuUnitIDValidFrom = new DateTime();
        private string _emailAddress = "";
        private DateTime _emailAddressValidFrom = new DateTime();
        private string _JMBG = "";
        private string _username = "";
        private DateTime _usernameValidFrom = new DateTime();
        private string _status = "";
        private DateTime _statusValidFrom = new DateTime();
        private string _employeeBranch = "";
        private DateTime _employeeBranchValidFrom = new DateTime();
        private int _annualLeaveCurrentYear = -1;
        private DateTime _annualLeaveCurrentYearValidFrom = new DateTime();        
        private int _annualLeavePreviousYear = -1;
        private DateTime _annualLeavePreviousYearValidFrom = new DateTime();
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        private int result = -1;
        private string _remark = "";
        private string _language = "";
        private DateTime _validFrom = new DateTime();
        private int _deleteTag = -1;
        private int _annualLeaveCurrentYearLeft = -1;
        private int _annualLeavePreviousYearLeft = -1;
        private DateTime _annualLeaveStartDate = new DateTime();
        private int _workLocationID = -1;
        private string _workLocationCode = "";
        private int _positionID = -1;
        private string _address = "";
        private DateTime _dateOfBirth = new DateTime();
        private string _phoneNumber1 = "";
        private string _phoneNumber2 = "";
        private DateTime _createdTimeHist = new DateTime();

        public int AnnualLeavePreviousYearLeft
        {
            get { return _annualLeavePreviousYearLeft; }
            set { _annualLeavePreviousYearLeft = value; }
        }

        public int AnnualLeaveCurrentYearLeft
        {
            get { return _annualLeaveCurrentYearLeft; }
            set { _annualLeaveCurrentYearLeft = value; }
        }

        public int DeleteTag
        {
            get { return _deleteTag; }
            set { _deleteTag = value; }
        }

        public DateTime AnnualLeaveStartDate
        {
            get { return _annualLeaveStartDate; }
            set { _annualLeaveStartDate = value; }
        }

        public DateTime ValidFrom
        {
            get { return _validFrom; }
            set { _validFrom = value; }
        }

        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }

        public DateTime CreatedTimeHist
        {
            get { return _createdTimeHist; }
            set { _createdTimeHist = value; }
        }

        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

        public int Result
        {
            get { return result; }
            set { result = value; }
        }
        //public DateTime AnnualLeaveCurrentYearValidFrom
        //{
        //    get { return _annualLeaveCurrentYearValidFrom; }
        //    set { _annualLeaveCurrentYearValidFrom = value; }
        //}
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

        //public DateTime AnnualLeavePreviousYearValidFrom
        //{
        //    get { return _annualLeavePreviousYearValidFrom; }
        //    set { _annualLeavePreviousYearValidFrom = value; }
        //}

        public int AnnualLeavePreviousYear
        {
            get { return _annualLeavePreviousYear; }
            set { _annualLeavePreviousYear = value; }
        }

        public int AnnualLeaveCurrentYear
        {
            get { return _annualLeaveCurrentYear; }
            set { _annualLeaveCurrentYear = value; }
        }

        //public DateTime EmployeeBranchValidFrom
        //{
        //    get { return _employeeBranchValidFrom; }
        //    set { _employeeBranchValidFrom = value; }
        //}

        public string EmployeeBranch
        {
            get { return _employeeBranch; }
            set { _employeeBranch = value; }
        }


        //public DateTime StatusValidFrom
        //{
        //    get { return _statusValidFrom; }
        //    set { _statusValidFrom = value; }
        //}

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        //public DateTime UsernameValidFrom
        //{
        //    get { return _usernameValidFrom; }
        //    set { _usernameValidFrom = value; }
        //}

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public string JMBG
        {
            get { return _JMBG; }
            set { _JMBG = value; }
        }

        //public DateTime EmailAddressValidFrom
        //{
        //    get { return _emailAddressValidFrom; }
        //    set { _emailAddressValidFrom = value; }
        //}

        public string EmailAddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }

        //public DateTime ResponsibilityOuUnitIDValidFrom
        //{
        //    get { return _responsibilityOuUnitIDValidFrom; }
        //    set { _responsibilityOuUnitIDValidFrom = value; }
        //}

        public int ResponsibilityOuUnitID
        {
            get { return _responsibilityOuUnitID; }
            set { _responsibilityOuUnitID = value; }
        }

        //public DateTime ResponsibilityFsUnitIDValidFrom
        //{
        //    get { return _responsibilityFsUnitIDValidFrom; }
        //    set { _responsibilityFsUnitIDValidFrom = value; }
        //}

        public int ResponsibilityFsUnitID
        {
            get { return _responsibilityFsUnitID; }
            set { _responsibilityFsUnitID = value; }
        }

        //public DateTime OrganizationalUnitIDValidFrom
        //{
        //    get { return _organizationalUnitIDValidFrom; }
        //    set { _organizationalUnitIDValidFrom = value; }
        //}

        public int OrganizationalUnitID
        {
            get { return _organizationalUnitID; }
            set { _organizationalUnitID = value; }
        }

        //public DateTime PersonalHolidayDateValidFrom
        //{
        //    get { return _personalHolidayDateValidFrom; }
        //    set { _personalHolidayDateValidFrom = value; }
        //}

        public DateTime PersonalHolidayDate
        {
            get { return _personalHolidayDate; }
            set { _personalHolidayDate = value; }
        }

        //public DateTime PersonalHolidayCategoryValidFrom
        //{
        //    get { return _personalHolidayCategoryValidFrom; }
        //    set { _personalHolidayCategoryValidFrom = value; }
        //}
        

        public int PersonalHolidayCategory
        {
            get { return _personalHolidayCategory; }
            set { _personalHolidayCategory = value; }
        }

        //public DateTime EmployeeTypeIDValidFrom
        //{
        //    get { return _employeeTypeIDValidFrom; }
        //    set { _employeeTypeIDValidFrom = value; }
        //}

        public int EmployeeTypeID
        {
            get { return _employeeTypeID; }
            set { _employeeTypeID = value; }
        }

        public byte[] Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }

        //public DateTime FsUnitIDValidFrom
        //{
        //    get { return _fsUnitIDValidFrom; }
        //    set { _fsUnitIDValidFrom = value; }
        //}

        public int FsUnitID
        {
            get { return _fsUnitID; }
            set { _fsUnitID = value; }
        }

        //public DateTime TagIDValidFrom
        //{
        //    get { return _tagIDValidFrom; }
        //    set { _tagIDValidFrom = value; }
        //}

        public uint TagID
        {
            get { return _tagID; }
            set { _tagID = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }
       
        public int EmployeeIDOld
        {
            get { return _employeeIDOld; }
            set { _employeeIDOld = value; }
        }

        public int EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }

        public int RecID
        {
            get { return _recID; }
            set { _recID = value; }
        }

        public int WorkLocationID
        {
            get { return _workLocationID; }
            set { _workLocationID = value; }
        }

        public string WorkLocationCode
        {
            get { return _workLocationCode; }
            set { _workLocationCode = value; }
        }

        public int PositionID
        {
            get { return _positionID; }
            set { _positionID = value; }
        }

        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = value; }
        }

        public string PhoneNumber1
        {
            get { return _phoneNumber1; }
            set { _phoneNumber1 = value; }
        }

        public string PhoneNumber2
        {
            get { return _phoneNumber2; }
            set { _phoneNumber2 = value; }
        }
    }
}
