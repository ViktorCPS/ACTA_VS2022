using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
   public class PointTO
    {
        private string _pointName;
        private int _direction;
        private int _gate;
        private int _timeAttCounter;
        private int _pointID;
        private int _readPasses;

        public int ReadPasses
        {
            get { return _readPasses; }
            set { _readPasses = value; }
        }

        public int PointID
        {
            get { return _pointID; }
            set { _pointID = value; }
        }
       
        public int TimeAttCounter
        {
            get { return _timeAttCounter; }
            set { _timeAttCounter = value; }
        }

        public int Gate
        {
            get { return _gate; }
            set { _gate = value; }
        } 

        public int Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public string PointName
        {
            get { return _pointName; }
            set { _pointName = value; }
        }
       public PointTO()
       {
           TimeAttCounter = -1;
           Gate = -1;
           Direction = -1;
           PointName = "";
           PointID = -1;
           ReadPasses = -1;
       }

    }
}
