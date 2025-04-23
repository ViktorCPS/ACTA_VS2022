using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace TransferObjects
{
    [Serializable()]    
    public class ControlFilterTO
    {
        private string _type = "";
        private string _controlID = "";
        private string _value = "";

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public string ControlID
        {
            get { return _controlID; }
            set { _controlID = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public ControlFilterTO()
        { }
    }
}
