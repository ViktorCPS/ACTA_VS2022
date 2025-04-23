using System;
using System.Collections;
using System.Text;

namespace TransferObjects
{
   public class MapsObjectHdrTO
    {
        private int _objectID;
        private string _type;
        private int _mapID;

        public Hashtable Points;

        public int MapID
        {
            get { return _mapID; }
            set { _mapID = value; }
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

        public MapsObjectHdrTO()
        {
            MapID = -1;
            Type = "";
            ObjectID = -1;
            Points = new Hashtable();
        }
        public MapsObjectHdrTO(int objectID, string type, int mapID, Hashtable points)
        {
            MapID = mapID;
            Type = type;
            ObjectID = ObjectID;
            Points = points;
        }
    }
}
