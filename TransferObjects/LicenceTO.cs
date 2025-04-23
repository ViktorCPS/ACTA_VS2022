using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    /// <summary>
    /// LicenceTO (Transfer Object) used to send 
    /// and receive data from and to DAO
    /// </summary>
    public class LicenceTO
    {
        private int _recId = -1;
		private string _licenceKey = "";
				
		public int RecID
		{
			get{ return _recId; }
			set{ _recId = value; }
		}

		public string LicenceKey
		{
			get { return _licenceKey; }
			set {_licenceKey = value; }
		}

		public LicenceTO()
		{
		}

        public LicenceTO(int recID, string licenceKey)
		{
			this.RecID = recID;
			this.LicenceKey = licenceKey;
		}		
    }
}
