using System;
using System.Globalization;
using System.Text;

using System.Xml;
using System.Xml.Serialization;

namespace TransferObjects
{
    /// <summary>
    /// Summary description for LogTO.
    /// </summary>
    [XmlRootAttribute()]
    public class LogTO
    {
        string dateTimeformat = "";

        private uint _log_id = 0;
        private int _reader_id = -1;
        private uint _tag_id = 0;
        private int _antenna = -1;
        private int _event_happened = -1;
        private int _action_commited = -1;
        private DateTime _event_time = new DateTime();
        private string _eventTimeString = "";
        private int _pass_gen_used = -1;
        private string _location = "";
        private string _gate = "";
        private string _reader_description = "";
        private string _direction = "";
        private string _employee_name = "";
        private int _button = -1;

        private LogAdditionalInfoTO _addTO = new LogAdditionalInfoTO();

        public LogTO()
        {
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;

        }
        // Boris, dodat atribut button za TFT, 20160930
        public LogTO(uint log_id, int reader_id, uint tag_id,
            int antenna, int event_happened, int action_commited, DateTime event_time, int pass_gen_used, string location, string direction, string gate, string readerDescription, string employeeName,
            int button)
        {
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;

            LogID = log_id;
            ReaderID = reader_id;
            TagID = tag_id;
            Antenna = antenna;
            EventHappened = event_happened;
            ActionCommited = action_commited;
            EventTime = event_time;
            PassGenUsed = pass_gen_used;
            Location = location;
            Gate = gate;
            ReaderDescription = readerDescription;
            Direction = direction;
            EmployeeName = employeeName;
            Button = button;

        }

        [XmlIgnore]
        public LogAdditionalInfoTO AddTO
        {
            get { return _addTO; }
            set { _addTO = value; }
        }

        [XmlIgnore]
        public string EmployeeName
        {
            get { return _employee_name; }
            set { _employee_name = value; }
        }
        [XmlIgnore]
        public string Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
        [XmlIgnore]
        public string ReaderDescription
        {
            get { return _reader_description; }
            set { _reader_description = value; }
        }
        [XmlIgnore]
        public string Gate
        {
            get { return _gate; }
            set { _gate = value; }
        }
        [XmlIgnore]
        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }
        public uint LogID
        {
            get { return _log_id; }
            set { _log_id = value; }
        }
        public int ReaderID
        {
            get { return _reader_id; }
            set { _reader_id = value; }
        }
        public uint TagID
        {
            get { return _tag_id; }
            set { _tag_id = value; }
        }
        public int Antenna
        {
            get { return _antenna; }
            set { _antenna = value; }
        }
        public int EventHappened
        {
            get { return _event_happened; }
            set { _event_happened = value; }
        }
        public int ActionCommited
        {
            get { return _action_commited; }
            set { _action_commited = value; }
        }

        [XmlIgnore]
        public DateTime EventTime
        {
            get { return _event_time; }
            set
            {
                _event_time = value;
                EventTimeString = _event_time.ToString(dateTimeformat);
            }
        }
        public int Button
        {
            get { return _button; }
            set { _button = value; }
        }
        public int PassGenUsed
        {
            get { return _pass_gen_used; }
            set { _pass_gen_used = value; }
        }

        [XmlElement("EventTime")]
        public string EventTimeString
        {
            get { return _eventTimeString; }
            set
            {
                _eventTimeString = value;
                _event_time = Convert.ToDateTime(value);
            }
        }

        /// <summary>
        /// Override Object.ToString(). 
        /// Return current log object data.
        /// </summary>
        /// <returns></returns>
        /// 
        // Boris, dodat atribut button za TFT, 20160930
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\n ---- Log Object Data: ----");
            sb.Append("\n LogID: " + this.LogID.ToString());
            sb.Append("\n ReaderID: " + this.ReaderID.ToString());
            sb.Append("\n TagID: " + this.TagID.ToString());
            sb.Append("\n Antenna: " + this.Antenna.ToString());
            sb.Append("\n EventHappened: " + this.EventHappened.ToString());
            sb.Append("\n EventTime: " + this.EventTime.ToString());
            sb.Append("\n ActionCommited: " + this.ActionCommited.ToString());
            sb.Append("\n PassGenUsed: " + this.PassGenUsed.ToString());
            sb.Append("\n Button: " + this.Button.ToString());
            sb.Append("\n --------------------------");

            return sb.ToString();
        }
    }
}
