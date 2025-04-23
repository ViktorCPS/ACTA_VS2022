using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class SecurityRoutesReaderTO
    {
        private int _readerID;
        private string _name;
        private string _description;

        public int ReaderID
        {
            get { return _readerID; }
            set { _readerID = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public SecurityRoutesReaderTO()
        {
            // Init properties
            ReaderID = -1;
            Name = "";
            Description = "";
        }
    }
}
