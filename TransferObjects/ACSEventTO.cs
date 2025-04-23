using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class ACSEventTO
    {

        uint _id;

        public uint Id
        {
            get { return _id; }
            set { _id = value; }
        }

        int _eventType;

        public int EventType
        {
            get { return _eventType; }
            set { _eventType = value; }
        }

        string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        string _location;

        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }

        string _acsName;

        public string AcsName
        {
            get { return _acsName; }
            set { _acsName = value; }
        }

        DateTime eventDateTime;

        public DateTime EventDateTime
        {
            get { return eventDateTime; }
            set { eventDateTime = value; }
        }

        int _acsID;

        public int AcsID
        {
            get { return _acsID; }
            set { _acsID = value; }
        }

        uint _cardID;

        public uint CardID
        {
            get { return _cardID; }
            set { _cardID = value; }
        }

        int _direction;

        public int Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        int _employeeID;

        public int EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }
    }
}
