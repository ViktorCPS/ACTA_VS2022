using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class EmployeePYDataSumTO
    {
        private uint _recID = 0;
        private uint _pYCalcID = 0;
        private string _companyCode = "";
        private string _type = "";
        private int _employeeID = -1;
        private string _employeeType = "";
        private string _ccName = "";
        private string _ccDesc = "";
        private DateTime _dateStart = new DateTime();
        private DateTime _dateEnd = new DateTime();
        private string _paymentCode = "";
        private decimal _hrsAmount = -1;
        private DateTime _dateStartSickness = new DateTime();
        private DateTime _createdTime = new DateTime();

        public uint RecID
        {
            get { return _recID; }
            set { _recID = value; }
        }

        public DateTime DateStart
        {
            get { return _dateStart; }
            set { _dateStart = value; }
        }
        
        public DateTime DateEnd
        {
            get { return _dateEnd; }
            set { _dateEnd = value; }
        }
        
        public decimal HrsAmount
        {
            get { return _hrsAmount; }
            set { _hrsAmount = value; }
        }

        public string PaymentCode
        {
            get { return _paymentCode; }
            set { _paymentCode = value; }
        }

        public int EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }

        public string EmployeeType
        {
            get { return _employeeType; }
            set { _employeeType = value; }
        }

        public string CCName
        {
            get { return _ccName; }
            set { _ccName = value; }
        }

        public string CCDesc
        {
            get { return _ccDesc; }
            set { _ccDesc = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string CompanyCode
        {
            get { return _companyCode; }
            set { _companyCode = value; }
        }

        public uint PYCalcID
        {
            get { return _pYCalcID; }
            set { _pYCalcID = value; }
        }

        public DateTime DateStartSickness
        {
            get { return _dateStartSickness; }
            set { _dateStartSickness = value; }
        }
        
        public DateTime CreatedTime
        {
            get { return _createdTime; }
            set { _createdTime = value; }
        }
    }
}
