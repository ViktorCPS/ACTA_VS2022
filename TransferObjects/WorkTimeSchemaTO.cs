using System;
using System.Collections;
using System.Collections.Generic;

namespace TransferObjects
{
	/// <summary>
	/// WorkTimeSchema transfere object.
	/// </summary>
	public class WorkTimeSchemaTO
	{
		private int _timeSchemaID = -1;
		private string _name = "";
		private string _description = "";
		private string _type = "";
		private int _cycleDuration = -1;
        private string _status = "";
        private int _workingUnitID = -1;
        private int _turnus = -1;

        public int Turnus
        {
            get { return _turnus; }
            set { _turnus = value; }
        }

        public int WorkingUnitID
        {
            get { return _workingUnitID; }
            set { _workingUnitID = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

		public Dictionary<int, Dictionary<int, WorkTimeIntervalTO>> Days = new Dictionary<int,Dictionary<int,WorkTimeIntervalTO>>();

		public int TimeSchemaID
		{
			get { return _timeSchemaID; }
			set { _timeSchemaID = value; }
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

		public string Type
		{
			get { return _type; }
			set { _type = value; }
		}

		public int CycleDuration
		{
			get { return _cycleDuration; }
			set { _cycleDuration = value; }
		}
		
		public WorkTimeSchemaTO()
		{
			// Init properties
			TimeSchemaID = -1;
			Name = "";
			Description = "";
			Type = "";
			CycleDuration = -1;
			Days = new Dictionary<int,Dictionary<int,WorkTimeIntervalTO>>();
		}

        //natalija08112017
        public WorkTimeSchemaTO(int timeSchemaID)
        {
            TimeSchemaID = timeSchemaID;
        }
	}
}
