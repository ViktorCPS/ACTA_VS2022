using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class SyncOrganizationalStructureTO
    {
        private int _recID = -1;
        private int _unitID = -1;
        private int _parentUnitID = -1;
        private DateTime _parentUnitIDValidFrom = new DateTime();
        private string _description = "";
        private DateTime _descriptionValidFrom = new DateTime();
        private string _code = "";
        private DateTime _codeValidFrom = new DateTime();
        private string _costCenterStringone = "";
        private DateTime _costCenterStringoneValidFrom = new DateTime();
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        private DateTime _createdTimeHist = new DateTime();
        private int result = -1;
        private string _remark = "";
        private string company_code = "";
        private DateTime _validFrom = new DateTime();
        private string _status = "";

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public DateTime ValidFrom
        {
            get { return _validFrom; }
            set { _validFrom = value; }
        }
        public string CompanyCode
        {
            get { return company_code; }
            set { company_code = value; }
        }

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

        //public DateTime DescriptionValidFrom
        //{
        //    get { return _descriptionValidFrom; }
        //    set { _descriptionValidFrom = value; }
        //}
        //public DateTime ParentUnitIDValidFrom
        //{
        //    get { return _parentUnitIDValidFrom; }
        //    set { _parentUnitIDValidFrom = value; }
        //}

        public DateTime CreatedTimeHist
        {
            get { return _createdTimeHist; }
            set { _createdTimeHist = value; }
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

        //public DateTime CostCenterStringoneValidFrom
        //{
        //    get { return _costCenterStringoneValidFrom; }
        //    set { _costCenterStringoneValidFrom = value; }
        //}

        public string CostCenterStringone
        {
            get { return _costCenterStringone; }
            set { _costCenterStringone = value; }
        }

        //public DateTime CodeValidFrom
        //{
        //    get { return _codeValidFrom; }
        //    set { _codeValidFrom = value; }
        //}
        
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public int ParentUnitID
        {
            get { return _parentUnitID; }
            set { _parentUnitID = value; }
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
