using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
   public class MapsObjectDtlTO
    {
        private int _objectID;
        private string _type;
        private int _pointOrder;
        private double _X;
        private double _Y;

        public double Y
        {
            get { return _Y; }
            set { _Y = value; }
        }

        public double X
        {
            get { return _X; }
            set { _X = value; }
        }

        public int PointOrder
        {
            get { return _pointOrder; }
            set { _pointOrder = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public int ObjectID
        {
            get { return _objectID; }
            set { _objectID = value; }
        }

        public MapsObjectDtlTO()
        {
            ObjectID = -1;
            Type = "";
            PointOrder = -1;
            X = -1;
            Y = -1;
        }
    }
}
