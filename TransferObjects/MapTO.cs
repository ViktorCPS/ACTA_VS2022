using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class MapTO
    {
        private int _mapID;
        private int _parentMapID;
        private string _name;
        private string _description;
        private byte[] _content;

        public byte[] Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int ParentMapID
        {
            get { return _parentMapID; }
            set { _parentMapID = value; }
        }

        public int MapID
        {
            get { return _mapID; }
            set { _mapID = value; }
        }
        public MapTO()
        {
            this.MapID = -1;
            this.Name = "";
            this.Description = "";
            this.ParentMapID = -1;
            this.Content = null;
        }
        public MapTO(int mapID, int parentMapID, string name, string description, byte[] content)
        {
            this.MapID = mapID;
            this.Name = name;
            this.Description = description;
            this.ParentMapID = parentMapID;
            this.Content = content;
        }
    }
}
