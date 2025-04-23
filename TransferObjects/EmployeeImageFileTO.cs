using System;

using System.Xml;
using System.Xml.Serialization;

using Util;

namespace TransferObjects
{
    /// <summary>
    /// Summary description for EmployeeImageFileTO.
    /// </summary>
    [XmlRootAttribute()]
    public class EmployeeImageFileTO
    {
        private int _employeeID = -1;
        private byte[] _picture = null;
        private DateTime _modifiedTime = new DateTime(0);
        private string _pictureName = "";

        public int EmployeeID
		{
            get { return _employeeID; }
            set { _employeeID = value; }
		}

        public byte[] Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }

        public DateTime ModifiedTime
        {
            get { return _modifiedTime; }
            set { _modifiedTime = value; }
        }

        public string PictureName
        {
            get { return _pictureName; }
            set { _pictureName = value; }
        }

		public EmployeeImageFileTO()
		{
		}

        public EmployeeImageFileTO(int employeeID, byte[] picture, DateTime modifiedTime,
            string pictureName)
		{
            this.EmployeeID = employeeID;
            this.Picture = picture;
            this.ModifiedTime = modifiedTime;
            this.PictureName = pictureName;
		}
    }
}
