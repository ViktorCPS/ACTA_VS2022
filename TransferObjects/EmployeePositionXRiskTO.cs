using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class EmployeePositionXRiskTO
    {        
        private int _positionID = -1;
        private int _riskID = -1;
        private int _rotation = -1;
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        private string _modifiedBy = "";
        private DateTime _modifiedTime = new DateTime();
        
        public int PositionID
        {
            get { return _positionID; }
            set { _positionID = value; }
        }

        public int RiskID
        {
            get { return _riskID; }
            set { _riskID = value; }
        }
        
        public int Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
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

        public EmployeePositionXRiskTO()
        { }
    }
}
