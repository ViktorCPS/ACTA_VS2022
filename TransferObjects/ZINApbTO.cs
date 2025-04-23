using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
     public class ZINApbTO
    {

        private int _recID = -1;
        private int _employeeID = -1;
        private string _cardNum = "";
        private string _nVarcharValue1 = "";
        private string _nVarcharValue2 = "";
        private string _nVarcharValue3 = "";
        private string _nVarcharValue4 = "";
        private string _nVarcharValue5 = "";
        private string _createdBy = "";
         private DateTime _createdTime = new DateTime();

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


        public string NVarcharValue5
        {
            get { return _nVarcharValue5; }
            set { _nVarcharValue5 = value; }
        }

        public string NVarcharValue4
        {
            get { return _nVarcharValue4; }
            set { _nVarcharValue4 = value; }
        }

        public string NVarcharValue3
        {
            get { return _nVarcharValue3; }
            set { _nVarcharValue3 = value; }
        }

        public string NVarcharValue2
        {
            get { return _nVarcharValue2; }
            set { _nVarcharValue2 = value; }
        }

        public string NVarcharValue1
        {
            get { return _nVarcharValue1; }
            set { _nVarcharValue1 = value; }
        }

        public string CardNum
        {
            get { return _cardNum; }
            set { _cardNum = value; }
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

         public ZINApbTO()
         { }

    }
}
