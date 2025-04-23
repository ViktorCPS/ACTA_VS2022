using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class SiemensDepartmentTO
    {
        // object for IT_DPData record
        private int _siPassID = -1;         // ID of this department after transfering in SiPass
        private uint _pk = 0;               // PK - autoincrement id, prymary key
        private string _id = "";            // IDDept - this ID is not in use in synchronization
        private string _name = "";          // NameDept
        private int _level = -1;            // LevelDept
        private string _nameE = "";         // NameDeptE
        private int _parentID = -1;         // IDDeptParent - ID of the parent department
        private string _parentIDOld = "";   // IDDeptParentOld
        private string _typeOfCh = "";      // TypeCh - type of change: N-new record, C-change record, D-delete record
        private int _readStatus = -1;       // ReadStatus - status flag
        private int _odid = -1;             // Odid - ID of department -  this ID is send for employee department
        private DateTime _timeSc = new DateTime(); // TimeSc

        public int SiPassID
        {
            get { return _siPassID; }
            set { _siPassID = value; }
        }

        public uint PK
        {
            get { return _pk; }
            set { _pk = value; }
        }

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }

        public string NameE
        {
            get { return _nameE; }
            set { _nameE = value; }
        }

        public int ParentID
        {
            get { return _parentID; }
            set { _parentID = value; }
        }

        public string ParentIDOld
        {
            get { return _parentIDOld; }
            set { _parentIDOld = value; }
        }

        public string TypeOfCh
        {
            get { return _typeOfCh; }
            set { _typeOfCh = value; }
        }

        public int ReadStatus
        {
            get { return _readStatus; }
            set { _readStatus = value; }
        }

        public int Odid
        {
            get { return _odid; }
            set { _odid = value; }
        }

        public DateTime TimeSc
        {
            get { return _timeSc; }
            set { _timeSc = value; }
        }

        public SiemensDepartmentTO()
        { }

        public SiemensDepartmentTO(SiemensDepartmentTO deptTO)
        {
            this.PK = deptTO.PK;
            this.ID = deptTO.ID;
            this.Level = deptTO.Level;
            this.Name = deptTO.Name;
            this.NameE = deptTO.NameE;
            this.Odid = deptTO.Odid;
            this.ParentID = deptTO.ParentID;
            this.ParentIDOld = deptTO.ParentIDOld;
            this.ReadStatus = deptTO.ReadStatus;
            this.TimeSc = deptTO.TimeSc;
            this.TypeOfCh = deptTO.TypeOfCh;
        }
    }
}
