using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class EmployeeCounterMonthlyBalanceTO
    {
        private int _emplCounterTypeID = -1;
        private int _emplID = -1;        
        private DateTime _month = new DateTime();
        private int _valueEarned = -1;
        private int _valueUsed = -1;
        private int _balance = -1;
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        private string _modifiedBy = "";
        private DateTime _modifiedTime = new DateTime();

        public int EmplCounterTypeID
        {
            get { return _emplCounterTypeID; }
            set { _emplCounterTypeID = value; }
        }

        public int EmployeeID
        {
            get { return _emplID; }
            set { _emplID = value; }
        }

        public DateTime Month
        {
            get { return _month; }
            set { _month = value; }
        }

        public int ValueEarned
        {
            get { return _valueEarned; }
            set { _valueEarned = value; }
        }

        public int ValueUsed
        {
            get { return _valueUsed; }
            set { _valueUsed = value; }
        }

        public int Balance
        {
            get { return _balance; }
            set { _balance = value; }
        }

        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        public DateTime CreatedTime
        {
            get { return _createdTime; }
            set { _createdTime = value; }
        }

        public string ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
        }

        public DateTime ModifiedTime
        {
            get { return _modifiedTime; }
            set { _modifiedTime = value; }
        }

        public EmployeeCounterMonthlyBalanceTO()
        { }

        public EmployeeCounterMonthlyBalanceTO(EmployeeCounterMonthlyBalanceTO balanceTO)
        {
            this.EmplCounterTypeID = balanceTO.EmplCounterTypeID;
            this.EmployeeID = balanceTO.EmployeeID;
            this.Balance = balanceTO.Balance;            
            this.Month = balanceTO.Month;
            this.ValueEarned = balanceTO.ValueEarned;
            this.ValueUsed = balanceTO.ValueUsed;
            this.CreatedBy = balanceTO.CreatedBy;
            this.CreatedTime = balanceTO.CreatedTime;
            this.ModifiedBy = balanceTO.ModifiedBy;
            this.ModifiedTime = balanceTO.ModifiedTime;
        }
    }
}

