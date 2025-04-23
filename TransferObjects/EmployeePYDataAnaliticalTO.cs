using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class EmployeePYDataAnaliticalTO
    {
        private uint _PYCalcID = 0;
        private string _companyCode = "";
        private string _type = "";
        private int _employeeID = -1;
        private string _employeeType = "";
        private string _ccName = "";
        private string _ccDesc = "";
        private DateTime _date = new DateTime();
        private string _paymentCode = "";
        private DateTime _dateStartSickness = new DateTime();
        private decimal _hrsAmount = -1;

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

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
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
            get { return _PYCalcID; }
            set { _PYCalcID = value; }
        }

        public DateTime DateStartSickness
        {
            get { return _dateStartSickness; }
            set { _dateStartSickness = value; }
        }

        public EmployeePYDataAnaliticalTO()
        { }

        public EmployeePYDataAnaliticalTO(EmployeePYDataAnaliticalTO analiticalTO)
        {
            this.CCDesc = analiticalTO.CCDesc;
            this.CCName = analiticalTO.CCName;
            this.CompanyCode = analiticalTO.CompanyCode;
            this.Date = analiticalTO.Date;
            this.DateStartSickness = analiticalTO.DateStartSickness;
            this.EmployeeID = analiticalTO.EmployeeID;
            this.EmployeeType = analiticalTO.EmployeeType;
            this.HrsAmount = analiticalTO.HrsAmount;
            this.PaymentCode = analiticalTO.PaymentCode;
            this.PYCalcID = analiticalTO.PYCalcID;
            this.Type = analiticalTO.Type;
        }
    }
}
