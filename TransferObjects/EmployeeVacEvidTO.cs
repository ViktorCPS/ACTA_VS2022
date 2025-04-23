using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class EmployeeVacEvidTO
    {
        private int _employeeID = -1;
        private DateTime _vacYear = new DateTime();
        private int _numOfDays = -1;
        private string _note = "";
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        private string _firstName = "";
        private string _lastName = "";
        private string _workingUnit = "";
        private int _workingUnitID = -1;
        private Dictionary<int, EmployeeVacEvidScheduleTO> _vacationSchedules = new Dictionary<int,EmployeeVacEvidScheduleTO>();
        private string _modifiedBy = "";
        private DateTime _modifiedTime = new DateTime();
        private int _fromLastYear = -1;
        private int _totalForUse = -1;
        private int _usedDays = -1;
        private int _daysLeft = -1;
        private DateTime _validTo = new DateTime();
             
        public EmployeeVacEvidTO()
        { }
        
        public int DaysLeft
        {
            get { return _daysLeft; }
            set { _daysLeft = value; }
        }

        public int UsedDays
        {
            get { return _usedDays; }
            set { _usedDays = value; }
        }
        public DateTime ValidTo
        {
            get { return _validTo; }
            set { _validTo = value; }
        }
       
        public int TotalForUse
        {
            get { return _totalForUse; }
            set { _totalForUse = value; }
        }

        public int FromLastYear
        {
            get { return _fromLastYear; }
            set { _fromLastYear = value; }
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
        public Dictionary<int, EmployeeVacEvidScheduleTO> VacationSchedules
        {
            get { return _vacationSchedules; }
            set { _vacationSchedules = value; }
        }     

        public int WorkingUnitID
        {
            get { return _workingUnitID; }
            set { _workingUnitID = value; }
        }

        public string WorkingUnit
        {
            get { return _workingUnit; }
            set { _workingUnit = value; }
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

        public string Note
        {
            get { return _note; }
            set { _note = value; }
        }

        public int NumOfDays
        {
            get { return _numOfDays; }
            set { _numOfDays = value; }
        }

        public DateTime VacYear
        {
            get { return _vacYear; }
            set { _vacYear = value; }
        }

        public int EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }
    }
}
