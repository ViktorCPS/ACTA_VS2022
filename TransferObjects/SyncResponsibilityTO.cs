using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
  public class SyncResponsibilityTO
    {
        private int _recID = -1;
        private int _unitID = -1;
        private int _responsiblePersonID = -1;
        private string _structureType = "";
        private DateTime _validFrom = new DateTime();
        private DateTime _validTo = new DateTime();
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        private DateTime _createdTimeHist = new DateTime();
        private int result = -1;
        private string _remark = "";

        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

        public int Result
        {
            get { return result; }
            set { result = value; }
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

        public DateTime CreatedTimeHist
        {
            get { return _createdTimeHist; }
            set { _createdTimeHist = value; }
        }

        public DateTime ValidTo
        {
            get { return _validTo; }
            set { _validTo = value; }
        }

        public DateTime ValidFrom
        {
            get { return _validFrom; }
            set { _validFrom = value; }
        }
     

        public string StructureType
        {
            get { return _structureType; }
            set { _structureType = value; }
        }

        public int ResponsiblePersonID
        {
            get { return _responsiblePersonID; }
            set { _responsiblePersonID = value; }
        }

        public int UnitID
        {
            get { return _unitID; }
            set { _unitID = value; }
        }

        public int RecID
        {
            get { return _recID; }
            set { _recID = value; }
        }
    }
}
