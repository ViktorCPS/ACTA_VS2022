using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class SiemensVechicleLogTO
    {
        // object for vechicle_registrations record        
        private DateTime _eventTime = new DateTime(); // event_time - registration time
        private int _pointID = -1;      // SiPass point id
        private string _direction = ""; // Direction (I-IN, O-OUT)                
        private int _driverID = -1;     // SiPass employee ID of driver
        private int _vechicleID = -1;   // SiPass employee ID of vechicle

        public int DriverID
        {
            get { return _driverID; }
            set { _driverID = value; }
        }

        public int VechicleID
        {
            get { return _vechicleID; }
            set { _vechicleID = value; }
        }

        public int PointID
        {
            get { return _pointID; }
            set { _pointID = value; }
        }

        public string Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public DateTime EventTime
        {
            get { return _eventTime; }
            set { _eventTime = value; }
        }
    }
}
