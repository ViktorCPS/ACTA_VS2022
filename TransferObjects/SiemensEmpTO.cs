using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class SiemensEmpTO
    {
        // object for IT_Emp
        private string _id = "";    // emp_id - employee number
        private string _idc = "";   // emp_idc - employee card number
        private int _rs = -1;       // emp_rs - read status

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }
        
        public int RS
        {
            get { return _rs; }
            set { _rs = value; }
        }

        public string IDC
        {
            get { return _idc; }
            set { _idc = value; }
        }

        public SiemensEmpTO()
        { }

        public SiemensEmpTO(SiemensEmpTO empTO)
        {
            this.ID = empTO.ID;
            this.IDC = empTO.IDC;
            this.RS = empTO.RS;
        }
    }
}
