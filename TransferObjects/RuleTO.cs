using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class RuleTO
    {
        private int _ruleID = -1;
        private int _workingUnitID = -1;
        private string _ruleType = "";
        private int _employeeTypeID = -1;
        private int _ruleValue = -1;
        private string _ruleDescription = "";
        private DateTime _ruleDateTime1 = new DateTime();
        private DateTime _ruleDateTime2 = new DateTime();

        public DateTime RuleDateTime2
        {
            get { return _ruleDateTime2; }
            set { _ruleDateTime2 = value; }
        }

        public DateTime RuleDateTime1
        {
            get { return _ruleDateTime1; }
            set { _ruleDateTime1 = value; }
        }

        public string RuleDescription
        {
            get { return _ruleDescription; }
            set { _ruleDescription = value; }
        }

        public int RuleValue
        {
            get { return _ruleValue; }
            set { _ruleValue = value; }
        }

        public int EmployeeTypeID
        {
            get { return _employeeTypeID; }
            set { _employeeTypeID = value; }
        }

        public string RuleType
        {
            get { return _ruleType; }
            set { _ruleType = value; }
        }

        public int WorkingUnitID
        {
            get { return _workingUnitID; }
            set { _workingUnitID = value; }
        }

        public int RuleID
        {
            get { return _ruleID; }
            set { _ruleID = value; }
        }
    }
}
