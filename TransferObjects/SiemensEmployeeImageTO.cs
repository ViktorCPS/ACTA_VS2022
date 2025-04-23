using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class SiemensEmployeeImageTO
    {
        // object for IT_HRImage record
        private string _id = "";        // ID - employee number
        private byte[] _image = null;   // Image - employee image
        private int _readStatus = -1;   // ReadStatus
        private string _typeOfCh = "";  // TypeOfCh

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public byte[] Image
        {
            get { return _image; }
            set { _image = value; }
        }

        public int ReadStatus
        {
            get { return _readStatus; }
            set { _readStatus = value; }
        }

        public string TypeOfCh
        {
            get { return _typeOfCh; }
            set { _typeOfCh = value; }
        }

        public SiemensEmployeeImageTO()
        { }

        public SiemensEmployeeImageTO(SiemensEmployeeImageTO imgTO)
        {
            this.ID = imgTO.ID;
            this.Image = imgTO.Image;
            this.ReadStatus = imgTO.ReadStatus;
            this.TypeOfCh = imgTO.TypeOfCh;
        }
    }
}
