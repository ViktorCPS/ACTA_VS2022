using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using Util;

namespace TransferObjects
{
    /// <summary>
    /// Summary description for LogTO.
    /// </summary>
    [XmlRootAttribute()]
    public class LogTmpAdditionalInfoTO
    {
        string dateTimeformat = "";
        char delimiter = '|';

        private int _log_id = -1;
        private int _reader_id = -1;
        private uint _tag_id = 0;
        private int _antenna = -1;
        private int _event_happened = -1;
        private int _action_commited = -1;
        private DateTime _event_time = new DateTime();
        private string _eventTimeString = "";
        private int _pass_gen_used = -1;
        private string _gpsData = "";
        private string _cardholderName = "";
        private int _cardholderID = -1;

        public LogTmpAdditionalInfoTO()
        {
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public LogTmpAdditionalInfoTO(LogTmpAdditionalInfoTO logTO)
        {
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;

            this.LogID = logTO.LogID;
            this.ReaderID = logTO.ReaderID;
            this.TagID = logTO.TagID;
            this.Antenna = logTO.Antenna;
            this.EventHappened = logTO.EventHappened;
            this.ActionCommited = logTO.ActionCommited;
            this.EventTime = logTO.EventTime;
            this.PassGenUsed = logTO.PassGenUsed;
            this.GpsData = logTO.GpsData;
            this.CardholderName = logTO.CardholderName;
            this.CardholderID = logTO.CardholderID;
        }

        public LogTmpAdditionalInfoTO CreateLogFromFileLine(string line, Dictionary<string, int> readers)
        {
            LogTmpAdditionalInfoTO logTO = new LogTmpAdditionalInfoTO();
            try
            {
                string[] logData = line.Split(delimiter);

                // create log record from file line
                if (logData.Length > 8)
                {
                    uint tagID;
                    int cardHolderID = -1;
                    DateTime eventTime = new DateTime();
                    if (readers.ContainsKey(logData[1].Trim()))
                        logTO.ReaderID = readers[logData[1].Trim()];
                    else
                        return null;
                    logTO.GpsData = logData[2].Trim() + delimiter + logData[3].Trim();
                    if (DateTime.TryParse(logData[4].Trim(), out eventTime))
                        logTO.EventTime = eventTime;
                    else
                        return null;
                    if (uint.TryParse(logData[5].Trim(), out tagID))
                        logTO.TagID = tagID;
                    else
                        return null;
                    if (logData[6].Trim().ToUpper() == Constants.DirectionIn.Trim().ToUpper())
                        logTO.Antenna = Constants.passAntenna0;
                    else if (logData[6].Trim().ToUpper() == Constants.DirectionOut.Trim().ToUpper())
                        logTO.Antenna = Constants.passAntenna1;
                    else
                        return null;
                    if (int.TryParse(logData[7].Trim(), out cardHolderID))
                        logTO.CardholderID = cardHolderID;
                    else
                        return null;
                    logTO.CardholderName = logData[8].Trim();
                    logTO.EventHappened = (int)Constants.EventTag.eventTagAllowed;
                    logTO.ActionCommited = Constants.actionCommitedAllowed;
                    logTO.PassGenUsed = (int)Constants.PassGenUsed.Unused;
                }
                else
                    return null;
            }
            catch
            {
                logTO = null;
            }

            return logTO;
        }

        public int LogID
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

        public string GpsData
        {
            get { return _gpsData; }
            set { _gpsData = value; }
        }

        public string CardholderName
        {
            get { return _cardholderName; }
            set { _cardholderName = value; }
        }

        public int CardholderID
        {
            get { return _cardholderID; }
            set { _cardholderID = value; }
        }
    }
}
