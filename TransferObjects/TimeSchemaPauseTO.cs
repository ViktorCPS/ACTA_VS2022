using System;

using System.Xml;
using System.Xml.Serialization;

namespace TransferObjects
{
	/// <summary>
	/// Summary description for TimeSchemaPauseTO.
	/// </summary>
	[XmlRootAttribute()]
	public class TimeSchemaPauseTO
	{
		private int     _pauseID = -1;
		private string  _description = "";
		private int     _pauseDuration = -1;
		private int     _earliestUseTime = -1;
		private int     _latestUseTime = -1;
		private int     _pauseOffset = -1;
		private int     _shortBreakDuration = -1;

		public int PauseID
		{
			get { return _pauseID; }
			set { _pauseID = value; }
		}

		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		public int PauseDuration
		{
			get { return _pauseDuration; }
			set { _pauseDuration = value; }
		}

		public int EarliestUseTime
		{
			get { return _earliestUseTime; }
			set { _earliestUseTime = value; }
		}

		public int LatestUseTime
		{
			get { return _latestUseTime; }
			set { _latestUseTime = value; }
		}

		public int PauseOffset
		{
			get { return _pauseOffset; }
			set { _pauseOffset = value; }
		}

		public int ShortBreakDuration
		{
			get { return _shortBreakDuration; }
			set { _shortBreakDuration = value; }
		}

		public TimeSchemaPauseTO()
		{
		}

		public TimeSchemaPauseTO(int pauseID, string description, int pauseDuration,
			int earliestUseTime, int latestUseTime, int pauseOffset, int shortBreakDuration)
		{
			this.PauseID            = pauseID;
			this.Description        = description;
			this.PauseDuration      = pauseDuration;
			this.EarliestUseTime    = earliestUseTime;
			this.LatestUseTime      = latestUseTime;
			this.PauseOffset        = pauseOffset;
			this.ShortBreakDuration = shortBreakDuration;
		}
	}
}
