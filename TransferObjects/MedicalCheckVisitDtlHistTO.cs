using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class MedicalCheckVisitDtlHistTO
    {
        private uint _recHistID = 0;
        private uint _recID = 0;
        private uint _visitID = 0;
        private int _checkID = -1;
        private string _type = "";
        private string _result = ""; 
        private string _createdBy = "";
        private DateTime _datePerformed = new DateTime();
        private DateTime _createdTime = new DateTime();
        private string _modifiedBy = "";
        private DateTime _modifiedTime = new DateTime();

        public uint RecHistID
        {
            get { return _recHistID; }
            set { _recHistID = value; }
        }

        public uint RecID
        {
            get { return _recID; }
            set { _recID = value; }
        }

        public uint VisitID
        {
            get { return _visitID; }
            set { _visitID = value; }
        }

        public int CheckID
        {
            get { return _checkID; }
            set { _checkID = value; }
        }
        
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Result
        {
            get { return _result; }
            set { _result = value; }
        }

        public DateTime DatePerformed
        {
            get { return _datePerformed; }
            set { _datePerformed = value; }
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

        public MedicalCheckVisitDtlHistTO()
        { }

        public MedicalCheckVisitDtlHistTO(MedicalCheckVisitDtlTO dtlTO)
        {
            this.CheckID = dtlTO.CheckID;
            this.CreatedBy = dtlTO.CreatedBy;
            this.CreatedTime = dtlTO.CreatedTime;
            this.DatePerformed = dtlTO.DatePerformed;
            this.ModifiedBy = dtlTO.ModifiedBy;
            this.ModifiedTime = dtlTO.ModifiedTime;
            this.Result = dtlTO.Result;
            this.Type = dtlTO.Type;
            this.VisitID = dtlTO.VisitID;
        }
    }
}
