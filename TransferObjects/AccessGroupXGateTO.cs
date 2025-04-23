using System;

using System.Xml;
using System.Xml.Serialization;

namespace TransferObjects
{
	/// <summary>
	/// Summary description for AccessGroupXGateTO.
	/// </summary>
	[XmlRootAttribute()]
	public class AccessGroupXGateTO
	{
		private int   _accessGroupID = -1;
		private int   _gateID = -1;
		private int   _gateTimeAccessProfile = -1;
		private int   _reader_access_group_ord_num = -1;

		public int AccessGroupID
		{
			get { return _accessGroupID; }
			set { _accessGroupID = value; }
		}

		public int GateID
		{
			get { return _gateID; }
			set { _gateID = value; }
		}

		public int GateTimeAccessProfile
		{
			get { return _gateTimeAccessProfile; }
			set { _gateTimeAccessProfile = value; }
		}

		public int ReaderAccessGroupOrdNum
		{
			get { return _reader_access_group_ord_num; }
			set { _reader_access_group_ord_num = value; }
		}

		public AccessGroupXGateTO()
		{
		}

		public AccessGroupXGateTO(int accessGroupID, int gateID, int gateTimeAccessProfile,
			int readerAccessGroupOrdNum)
		{
			this.AccessGroupID           = accessGroupID;
			this.GateID                  = gateID;
			this.GateTimeAccessProfile   = gateTimeAccessProfile;
			this.ReaderAccessGroupOrdNum = readerAccessGroupOrdNum;
		}
	}
}
