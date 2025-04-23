using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class SiemensLogTO
    {
        // object for IT_Reg record + SiPass AuditTrail + SiPass employee whose registration is
        private string _id = "";        // ID - employee number for E registrations, JMBG for V registrations
        private DateTime _regTime = new DateTime(); // RegTime - registration time
        private int _regLoc = -1;       // RegLoc - registration location - Gate of registration point
        private string _direction = ""; // Direction (I-IN, O-OUT)
        private string _typeID = "";    // TypeID (E-employee, V-visitor)
        private string _name = "";      // Name (employee forst name)
        private string _lastName = "";  // LastName (employee last name)
        private string _company = "";   // Company
        private int _readStatus = -1;   // ReadStatus flag
        private string _col1 = "";      // Col1 - manually created passes remark
        private int _logID = -1;        // at_id from AuditTrail
        private string _msg = "";       // message from AuditTrail
        private int _pointID = -1;      // SiPass point ID
        private int _emplID = -1;       // SiPass employee ID
        private bool _fromFile = false; // is log created from AuditTrail file
        private bool _truckCandidate = false; // is log candidate for truck pass
        private uint _pk = 0;        // PK - autoincrement primary key
        public int LogID
        {
            get { return _logID; }
            set { _logID = value; }
        }

        public int EmplID
        {
            get { return _emplID; }
            set { _emplID = value; }
        }

        public uint PK
        {
            get { return _pk; }
            set { _pk = value; }
        }

        public int PointID
        {
            get { return _pointID; }
            set { _pointID = value; }
        }

        public string Col1
        {
            get { return _col1; }
            set { _col1 = value; }
        }

        public int ReadStatus
        {
            get { return _readStatus; }
            set { _readStatus = value; }
        }

        public string Company
        {
            get { return _company; }
            set { _company = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string TypeID
        {
            get { return _typeID; }
            set { _typeID = value; }
        }

        public string Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public int RegLoc
        {
            get { return _regLoc; }
            set { _regLoc = value; }
        }

        public DateTime RegTime
        {
            get { return _regTime; }
            set { _regTime = value; }
        }

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Message
        {
            get { return _msg; }
            set { _msg = value; }
        }

        public bool FromFile
        {
            get { return _fromFile; }
            set { _fromFile = value; }
        }

        public bool TruckCandidate
        {
            get { return _truckCandidate; }
            set { _truckCandidate = value; }
        }
    }
}
