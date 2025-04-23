using System;

using TransferObjects;

using System.Xml;
using System.Xml.Serialization;

namespace Common
{
	/// <summary>
	/// CardOwner holds necessary information about a card owner for ACTAMonitor application.
	/// </summary>
    ///
    [XmlRootAttribute()]
	public class CardOwner
	{
        [XmlAttributeAttribute(AttributeName = "cardSerialNumber")]
		public uint cardSerialNumber;
        [XmlAttributeAttribute(AttributeName = "antennaNum")]
        public int antennaNum;
        [XmlAttributeAttribute(AttributeName = "entrancePermitted")]
        public bool entrancePermitted;
        [XmlAttributeAttribute(AttributeName = "eventTime")]
        public DateTime eventTime;      //DC 1.9.2008.


        [XmlAttributeAttribute(AttributeName = "employee")]
        public EmployeeTO employee;
        [XmlAttributeAttribute(AttributeName = "reader")]
		public ReaderTO reader;

		public CardOwner(ReaderTO reader, uint serNo, int anum, bool entrancePermitted, DateTime eventtime)
		{
			this.reader = reader;
			this.cardSerialNumber = serNo;
			this.antennaNum = anum;
			this.entrancePermitted = entrancePermitted;
            this.eventTime = eventtime;
		}
	}
}
