using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace TransferObjects
{
    public class MachineTO
    {
        int _machineID = -1;
        string _name = "";
        string _description = "";

        public int MachineID
        {
            get { return _machineID; }
            set { _machineID = value; }
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

        public MachineTO()
        {

        }

        public MachineTO(string name)
        {
            this.Name = name;
        }

        public MachineTO(int machineID, string name, string description)
        {
            this.MachineID = machineID;
            this.Name = name;
            this.Description = description;
        }
    }
}
