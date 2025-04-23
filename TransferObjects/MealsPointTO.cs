using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
   public class MealsPointTO
    {
        int _pointID;
        string _terminalSerial;
        string _name;
        string _description;
       
       public int PointID
       {
           get { return _pointID; }
           set { _pointID = value; }
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

        public string TerminalSerial
        {
            get { return _terminalSerial; }
            set { _terminalSerial = value; }
        }
        public MealsPointTO()
        {
            this.PointID = -1;
            this.TerminalSerial = "";
            this.Name = "";
            this.Description = "";
        }
        public MealsPointTO(int pointID,string terminalSerial, string name, string description)
        {
            this.PointID = pointID;
            this.TerminalSerial = terminalSerial;
            this.Name = name;
            this.Description = description;
        }
    }
}
