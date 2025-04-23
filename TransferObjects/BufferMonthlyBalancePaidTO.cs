using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class BufferMonthlyBalancePaidTO
    {
        private uint _pyCalcID = 0;
        private int _emplCounterTypeID = -1;
        private int _emplID = -1;        
        private DateTime _month = new DateTime();        
        private int _valuePaid = -1;
        private int _recalcFlag = -1;
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        private string _modifiedBy = "";
        private DateTime _modifiedTime = new DateTime();

        public uint PYCalcID
        {
            get { return _pyCalcID; }
            set { _pyCalcID = value; }
        }

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

        public int ValuePaid
        {
            get { return _valuePaid; }
            set { _valuePaid = value; }
        }

        public int RecalcFlag
        {
            get { return _recalcFlag; }
            set { _recalcFlag = value; }
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

        public BufferMonthlyBalancePaidTO()
        { }

        public BufferMonthlyBalancePaidTO(BufferMonthlyBalancePaidTO balanceTO)
        {
            this.PYCalcID = balanceTO.PYCalcID;
            this.EmplCounterTypeID = balanceTO.EmplCounterTypeID;
            this.EmployeeID = balanceTO.EmployeeID;            
            this.Month = balanceTO.Month;
            this.ValuePaid = balanceTO.ValuePaid;            
            this.CreatedBy = balanceTO.CreatedBy;
            this.CreatedTime = balanceTO.CreatedTime;
            this.ModifiedBy = balanceTO.ModifiedBy;
            this.ModifiedTime = balanceTO.ModifiedTime;
        }
    }
}
