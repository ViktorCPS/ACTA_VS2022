using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class UnipromButtonTO
    {
        private int _readerID = -1;
        private int _antennaOutput = -1;
        private int _status = -1;
        private DateTime _modifiedTime = new DateTime();
        private string _direction = "";

        public string Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public DateTime ModifiedTime
        {
            get { return _modifiedTime; }
            set { _modifiedTime = value; }
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

        public UnipromButtonTO()
        { }

    }
}
