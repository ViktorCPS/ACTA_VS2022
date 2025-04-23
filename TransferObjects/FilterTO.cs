using System;
using System.Collections;
using System.Text;

using System.Xml;
using System.Xml.Serialization;
namespace TransferObjects
{
    [XmlRootAttribute()]
    public class FilterTO
    {
        private string _applMenuItemID= "";
        private string _tabID = "";
        private ArrayList _controlValues = new ArrayList();
        private int _filterID = -1;
        private string _filterName = "";
        private string _description = "";
        private int _default = -1;
        private string _xmlDocument = "";
        private string _userID;
        private DateTime _createdTime = new DateTime();

        public DateTime CreatedTime
        {
            get { return _createdTime; }
            set { _createdTime = value; }
        }

        public string UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        public string XmlDocument
        {
            get { return _xmlDocument; }
            set { _xmlDocument = value; }
        }

        public int Default
        {
            get { return _default; }
            set { _default = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string FilterName
        {
            get { return _filterName; }
            set { _filterName = value; }
        }

        public int FilterID
        {
            get { return _filterID; }
            set { _filterID = value; }
        }

        public ArrayList ControlValues
        {
            get { return _controlValues; }
            set { _controlValues = value; }
        }

        public string TabID
        {
            get { return _tabID; }
            set { _tabID = value; }
        }

        public string ApplMenuItemID
        {
            get { return _applMenuItemID; }
            set { _applMenuItemID = value; }
        }

        public FilterTO()
        { }
    }
}
