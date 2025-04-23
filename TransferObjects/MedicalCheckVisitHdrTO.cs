using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class MedicalCheckVisitHdrTO
    {
        private uint _visitID = 0;
        private int _employeeID = -1;
        private DateTime _scheduleDate = new DateTime();
        private int _pointID = -1;
        private string _status = "";
        private int _flagEmail = -1;        
        private DateTime _flagEmailCratedTime = new DateTime();
        private int _flagChange = -1;
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        private string _modifiedBy = "";
        private DateTime _modifiedTime = new DateTime();

        private List<MedicalCheckVisitDtlTO> details = new List<MedicalCheckVisitDtlTO>();

        public uint VisitID
        {
            get { return _visitID; }
            set { _visitID = value; }
        }

        public int EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }

        public DateTime ScheduleDate
        {
            get { return _scheduleDate; }
            set { _scheduleDate = value; }
        }

        public int PointID
        {
            get { return _pointID; }
            set { _pointID = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public int FlagEmail
        {
            get { return _flagEmail; }
            set { _flagEmail = value; }
        }

        public DateTime FlagEmailCratedTime
        {
            get { return _flagEmailCratedTime; }
            set { _flagEmailCratedTime = value; }
        }

        public int FlagChange
        {
            get { return _flagChange; }
            set { _flagChange = value; }
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

        public List<MedicalCheckVisitDtlTO> VisitDetails
        {
            get { return details; }
            set { details = value; }
        }

        public MedicalCheckVisitHdrTO()
        { }

        public MedicalCheckVisitHdrTO(MedicalCheckVisitHdrTO hdrTO)
        {
            this.CreatedBy = hdrTO.CreatedBy;
            this.CreatedTime = hdrTO.CreatedTime;
            this.EmployeeID = hdrTO.EmployeeID;
            this.FlagEmail = hdrTO.FlagEmail;
            this.FlagEmailCratedTime = hdrTO.FlagEmailCratedTime;
            this.FlagChange = hdrTO.FlagChange;
            this.ModifiedBy = hdrTO.ModifiedBy;
            this.ModifiedTime = hdrTO.ModifiedTime;
            this.PointID = hdrTO.PointID;
            this.ScheduleDate = hdrTO.ScheduleDate;            
            this.Status = hdrTO.Status;
            this.VisitID = hdrTO.VisitID;

            foreach (MedicalCheckVisitDtlTO dtlTO in hdrTO.VisitDetails)
            {
                this.VisitDetails.Add(new MedicalCheckVisitDtlTO(dtlTO));
            }
        }
    }
}
