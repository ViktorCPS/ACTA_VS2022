using System;

using System.Xml;
using System.Xml.Serialization;

namespace TransferObjects
{
	/// <summary>
	/// Summary description for TimeAccessProfileDtlTO.
	/// </summary>
	[XmlRootAttribute()]
	public class TimeAccessProfileDtlTO
	{
		private int     _timeAccessProfileId = -1;
		private int     _dayOfWeek = -1;
		private string  _direction = "";
		private int     _hrs0 = -1;
		private int     _hrs1 = -1;
		private int     _hrs2 = -1;
		private int     _hrs3 = -1;
		private int     _hrs4 = -1;
		private int     _hrs5 = -1;
		private int     _hrs6 = -1;
		private int     _hrs7 = -1;
		private int     _hrs8 = -1;
		private int     _hrs9 = -1;
		private int     _hrs10 = -1;
		private int     _hrs11 = -1;
		private int     _hrs12 = -1;
		private int     _hrs13 = -1;
		private int     _hrs14 = -1;
		private int     _hrs15 = -1;
		private int     _hrs16 = -1;
		private int     _hrs17 = -1;
		private int     _hrs18 = -1;
		private int     _hrs19 = -1;
		private int     _hrs20 = -1;
		private int     _hrs21 = -1;
		private int     _hrs22 = -1;
		private int     _hrs23 = -1;

		public int TimeAccessProfileId
		{
			get { return _timeAccessProfileId; }
			set { _timeAccessProfileId = value; }
		}

		public int DayOfWeek
		{
			get { return _dayOfWeek; }
			set { _dayOfWeek = value; }
		}

		public string Direction
		{
			get { return _direction; }
			set { _direction = value; }
		}

		public int Hrs0
		{
			get { return _hrs0; }
			set { _hrs0 = value; }
		}

		public int Hrs1
		{
			get { return _hrs1; }
			set { _hrs1 = value; }
		}

		public int Hrs2
		{
			get { return _hrs2; }
			set { _hrs2 = value; }
		}

		public int Hrs3
		{
			get { return _hrs3; }
			set { _hrs3 = value; }
		}

		public int Hrs4
		{
			get { return _hrs4; }
			set { _hrs4 = value; }
		}

		public int Hrs5
		{
			get { return _hrs5; }
			set { _hrs5 = value; }
		}

		public int Hrs6
		{
			get { return _hrs6; }
			set { _hrs6 = value; }
		}

		public int Hrs7
		{
			get { return _hrs7; }
			set { _hrs7 = value; }
		}

		public int Hrs8
		{
			get { return _hrs8; }
			set { _hrs8 = value; }
		}

		public int Hrs9
		{
			get { return _hrs9; }
			set { _hrs9 = value; }
		}

		public int Hrs10
		{
			get { return _hrs10; }
			set { _hrs10 = value; }
		}

		public int Hrs11
		{
			get { return _hrs11; }
			set { _hrs11 = value; }
		}

		public int Hrs12
		{
			get { return _hrs12; }
			set { _hrs12 = value; }
		}

		public int Hrs13
		{
			get { return _hrs13; }
			set { _hrs13 = value; }
		}

		public int Hrs14
		{
			get { return _hrs14; }
			set { _hrs14 = value; }
		}

		public int Hrs15
		{
			get { return _hrs15; }
			set { _hrs15 = value; }
		}

		public int Hrs16
		{
			get { return _hrs16; }
			set { _hrs16 = value; }
		}

		public int Hrs17
		{
			get { return _hrs17; }
			set { _hrs17 = value; }
		}

		public int Hrs18
		{
			get { return _hrs18; }
			set { _hrs18 = value; }
		}

		public int Hrs19
		{
			get { return _hrs19; }
			set { _hrs19 = value; }
		}

		public int Hrs20
		{
			get { return _hrs20; }
			set { _hrs20 = value; }
		}

		public int Hrs21
		{
			get { return _hrs21; }
			set { _hrs21 = value; }
		}

		public int Hrs22
		{
			get { return _hrs22; }
			set { _hrs22 = value; }
		}

		public int Hrs23
		{
			get { return _hrs23; }
			set { _hrs23 = value; }
		}

		public TimeAccessProfileDtlTO()
		{
		}

		public TimeAccessProfileDtlTO(int timeAccessProfileId, int dayOfWeek, string direction, int hrs0,
			int hrs1, int hrs2, int hrs3, int hrs4, int hrs5, int hrs6, int hrs7, int hrs8, int hrs9, 
			int hrs10, int hrs11, int hrs12, int hrs13, int hrs14, int hrs15, int hrs16, int hrs17, int hrs18, 
			int hrs19, int hrs20, int hrs21, int hrs22, int hrs23)
		{
			this.TimeAccessProfileId = timeAccessProfileId;
			this.DayOfWeek           = dayOfWeek;
			this.Direction           = direction;
			this.Hrs0                = hrs0;
			this.Hrs1                = hrs1;
			this.Hrs2                = hrs2;
			this.Hrs3                = hrs3;
			this.Hrs4                = hrs4;
			this.Hrs5                = hrs5;
			this.Hrs6                = hrs6;
			this.Hrs7                = hrs7;
			this.Hrs8                = hrs8;
			this.Hrs9                = hrs9;
			this.Hrs10               = hrs10;
			this.Hrs11               = hrs11;
			this.Hrs12               = hrs12;
			this.Hrs13               = hrs13;
			this.Hrs14               = hrs14;
			this.Hrs15               = hrs15;
			this.Hrs16               = hrs16;
			this.Hrs17               = hrs17;
			this.Hrs18               = hrs18;
			this.Hrs19               = hrs19;
			this.Hrs20               = hrs20;
			this.Hrs21               = hrs21;
			this.Hrs22               = hrs22;
			this.Hrs23               = hrs23;
		}
	}
}
