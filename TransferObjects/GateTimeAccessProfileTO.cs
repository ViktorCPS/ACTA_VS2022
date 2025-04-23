using System;

using System.Xml;
using System.Xml.Serialization;

namespace TransferObjects
{
	/// <summary>
	/// Summary description for GateTimeAccessProfileTO.
	/// </summary>
	[XmlRootAttribute()]
	public class GateTimeAccessProfileTO
	{
		private int     _gateTAProfileId = -1;
		private string  _name = "";
		private string  _description = "";
		private int     _gateTAProfile0 = -1;
		private int     _gateTAProfile1 = -1;
		private int     _gateTAProfile2 = -1;
		private int     _gateTAProfile3 = -1;
		private int     _gateTAProfile4 = -1;
		private int     _gateTAProfile5 = -1;
		private int     _gateTAProfile6 = -1;
		private int     _gateTAProfile7 = -1;
		private int     _gateTAProfile8 = -1;
		private int     _gateTAProfile9 = -1;
		private int     _gateTAProfile10 = -1;
		private int     _gateTAProfile11 = -1;
		private int     _gateTAProfile12 = -1;
		private int     _gateTAProfile13 = -1;
		private int     _gateTAProfile14 = -1;
		private int     _gateTAProfile15 = -1;

		public int GateTAProfileId
		{
			get { return _gateTAProfileId; }
			set { _gateTAProfileId = value; }
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

		public int GateTAProfile0
		{
			get { return _gateTAProfile0; }
			set { _gateTAProfile0 = value; }
		}

		public int GateTAProfile1
		{
			get { return _gateTAProfile1; }
			set { _gateTAProfile1 = value; }
		}

		public int GateTAProfile2
		{
			get { return _gateTAProfile2; }
			set { _gateTAProfile2 = value; }
		}

		public int GateTAProfile3
		{
			get { return _gateTAProfile3; }
			set { _gateTAProfile3 = value; }
		}

		public int GateTAProfile4
		{
			get { return _gateTAProfile4; }
			set { _gateTAProfile4 = value; }
		}

		public int GateTAProfile5
		{
			get { return _gateTAProfile5; }
			set { _gateTAProfile5 = value; }
		}

		public int GateTAProfile6
		{
			get { return _gateTAProfile6; }
			set { _gateTAProfile6 = value; }
		}

		public int GateTAProfile7
		{
			get { return _gateTAProfile7; }
			set { _gateTAProfile7 = value; }
		}

		public int GateTAProfile8
		{
			get { return _gateTAProfile8; }
			set { _gateTAProfile8 = value; }
		}

		public int GateTAProfile9
		{
			get { return _gateTAProfile9; }
			set { _gateTAProfile9 = value; }
		}

		public int GateTAProfile10
		{
			get { return _gateTAProfile10; }
			set { _gateTAProfile10 = value; }
		}

		public int GateTAProfile11
		{
			get { return _gateTAProfile11; }
			set { _gateTAProfile11 = value; }
		}

		public int GateTAProfile12
		{
			get { return _gateTAProfile12; }
			set { _gateTAProfile12 = value; }
		}

		public int GateTAProfile13
		{
			get { return _gateTAProfile13; }
			set { _gateTAProfile13 = value; }
		}

		public int GateTAProfile14
		{
			get { return _gateTAProfile14; }
			set { _gateTAProfile14 = value; }
		}

		public int GateTAProfile15
		{
			get { return _gateTAProfile15; }
			set { _gateTAProfile15 = value; }
		}

		public GateTimeAccessProfileTO()
		{
		}

		public GateTimeAccessProfileTO(int gateTAProfileId, string name, string description,
			int gateTAProfile0, int gateTAProfile1, int gateTAProfile2, int gateTAProfile3,
			int gateTAProfile4, int gateTAProfile5, int gateTAProfile6, int gateTAProfile7,
			int gateTAProfile8, int gateTAProfile9, int gateTAProfile10, int gateTAProfile11,
			int gateTAProfile12, int gateTAProfile13, int gateTAProfile14, int gateTAProfile15)
		{
			this.GateTAProfileId = gateTAProfileId;
			this.Name            = name;
			this.Description     = description;
			this.GateTAProfile0  = gateTAProfile0;
			this.GateTAProfile1  = gateTAProfile1;
			this.GateTAProfile2  = gateTAProfile2;
			this.GateTAProfile3  = gateTAProfile3;
			this.GateTAProfile4  = gateTAProfile4;
			this.GateTAProfile5  = gateTAProfile5;
			this.GateTAProfile6  = gateTAProfile6;
			this.GateTAProfile7  = gateTAProfile7;
			this.GateTAProfile8  = gateTAProfile8;
			this.GateTAProfile9  = gateTAProfile9;
			this.GateTAProfile10  = gateTAProfile10;
			this.GateTAProfile11  = gateTAProfile11;
			this.GateTAProfile12  = gateTAProfile12;
			this.GateTAProfile13  = gateTAProfile13;
			this.GateTAProfile14  = gateTAProfile14;
			this.GateTAProfile15  = gateTAProfile15;
		}
	}
}
