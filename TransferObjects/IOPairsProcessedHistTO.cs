using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public  class IOPairsProcessedHistTO
    {
        private uint _recID = 0;
        private int _ioPairID = -1;
        private DateTime _IOPairDate = new DateTime();
        private int _employeeID = -1;
        private int _locationID = -1;
        private int _isWrkHrsCounter = -1;
        private int _passTypeID = -1;
        private DateTime _startTime = new DateTime();
        private DateTime _endTime = new DateTime();
        private int _manualCreated = -1;
        private int _confirmationFlag = -1;
        private string _confirmedBy = "";
        private DateTime _confirmationTime = new DateTime();
        private int _verificationFlag = -1;
        private string _verifiedBy = "";
        private DateTime _verifiedTime = new DateTime();
        private string _alert = "";
        private string _desc = "";
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        private string _modifiedBy = "";
        private DateTime _modifiedTime = new DateTime();
        private string _modifiedName = "";

        public DateTime ModifiedTime
        {
            get { return _modifiedTime; }
            set { _modifiedTime = value; }
        }

        public string ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
        }

        public string ModifiedName
        {
            get { return _modifiedName; }
            set { _modifiedName = value; }
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

        public string Alert
        {
            get { return _alert; }
            set { _alert = value; }
        }

        public DateTime VerifiedTime
        {
            get { return _verifiedTime; }
            set { _verifiedTime = value; }
        }

        public string VerifiedBy
        {
            get { return _verifiedBy; }
            set { _verifiedBy = value; }
        }

        public int VerificationFlag
        {
            get { return _verificationFlag; }
            set { _verificationFlag = value; }
        }

        public DateTime ConfirmationTime
        {
            get { return _confirmationTime; }
            set { _confirmationTime = value; }
        }

        public string ConfirmedBy
        {
            get { return _confirmedBy; }
            set { _confirmedBy = value; }
        }

        public int ConfirmationFlag
        {
            get { return _confirmationFlag; }
            set { _confirmationFlag = value; }
        }

        public int ManualCreated
        {
            get { return _manualCreated; }
            set { _manualCreated = value; }
        }

        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        public int PassTypeID
        {
            get { return _passTypeID; }
            set { _passTypeID = value; }
        }

        public int IsWrkHrsCounter
        {
            get { return _isWrkHrsCounter; }
            set { _isWrkHrsCounter = value; }
        }

        public int LocationID
        {
            get { return _locationID; }
            set { _locationID = value; }
        }

        public int EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }

        public DateTime IOPairDate
        {
            get { return _IOPairDate; }
            set { _IOPairDate = value; }
        }

        public int IOPairID
        {
            get { return _ioPairID; }
            set { _ioPairID = value; }
        }

        public uint RecID
        {
            get { return _recID; }
            set { _recID = value; }
        }

        public string Desc
        {
            get { return _desc; }
            set { _desc = value; }
        }

        public IOPairsProcessedHistTO()
        { 
        }

        public IOPairsProcessedHistTO(IOPairProcessedTO pairTO)
        {
            this.Alert = pairTO.Alert;
            this.ConfirmationFlag = pairTO.ConfirmationFlag;
            this.ConfirmationTime = pairTO.ConfirmationTime;
            this.ConfirmedBy = pairTO.ConfirmedBy;
            this.Desc = pairTO.Desc;
            this.EmployeeID = pairTO.EmployeeID;
            this.EndTime = pairTO.EndTime;
            this.IOPairDate = pairTO.IOPairDate;
            this.IOPairID = pairTO.IOPairID;
            this.IsWrkHrsCounter = pairTO.IsWrkHrsCounter;
            this.LocationID = pairTO.LocationID;
            this.ManualCreated = pairTO.ManualCreated;
            this.PassTypeID = pairTO.PassTypeID;
            this.RecID = pairTO.RecID;
            this.StartTime = pairTO.StartTime;
            this.VerificationFlag = pairTO.VerificationFlag;
            this.VerifiedBy = pairTO.VerifiedBy;
            this.VerifiedTime = pairTO.VerifiedTime;
            this.CreatedBy = pairTO.CreatedBy;
            this.CreatedTime = pairTO.CreatedTime;            
        }
    }
}
