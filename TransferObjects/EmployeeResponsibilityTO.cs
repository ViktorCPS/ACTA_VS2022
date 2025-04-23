using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class EmployeeResponsibilityTO
    {
        private int _recID = -1;
        private int _employeeID = -1;
        private int _unitID = -1;
        private string _type = "";

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public int UnitID
        {
            get { return _unitID; }
            set { _unitID = value; }
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
    }
}
