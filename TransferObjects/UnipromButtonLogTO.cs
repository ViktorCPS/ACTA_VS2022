using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
   public class UnipromButtonLogTO
    {
       
        private int _logID = -1;
        private int _readerID = -1;
        private int _antennaOutput = -1;
        private int _status = -1;
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

        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public int AntennaOutput
        {
            get { return _antennaOutput; }
            set { _antennaOutput = value; }
        }

        public int ReaderID
        {
            get { return _readerID; }
            set { _readerID = value; }
        }

        public int LogID
        {
            get { return _logID; }
            set { _logID = value; }
        }

        public UnipromButtonLogTO()
        { }

    }
}
