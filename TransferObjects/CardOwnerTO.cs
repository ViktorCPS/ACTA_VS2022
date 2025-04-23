using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.Serialization;

using System.Xml;
using System.Xml.Serialization;

namespace TransferObjects
{
    [DataContract]
     [XmlRootAttribute()]
   public class CardOwnerTO
    {
        [DataMember]
        [XmlAttributeAttribute(AttributeName = "cardSerialNumber")]
		public uint cardSerialNumber;
        [DataMember]
        [XmlAttributeAttribute(AttributeName = "antennaNum")]
        public int antennaNum;
        [DataMember]
        [XmlAttributeAttribute(AttributeName = "entrancePermitted")]
        public bool entrancePermitted;
        [DataMember]
        [XmlAttributeAttribute(AttributeName = "eventTime")]
        public DateTime eventTime;      //DC 1.9.2008.
        [DataMember]
        [XmlAttributeAttribute(AttributeName = "employee")]
        public EmployeeTO employee;
        [DataMember]
        [XmlAttributeAttribute(AttributeName = "reader")]
		public ReaderTO reader;

        public CardOwnerTO(ReaderTO reader, uint serNo, int anum, bool entrancePermitted, DateTime eventtime)
		{
			this.reader = reader;
			this.cardSerialNumber = serNo;
			this.antennaNum = anum;
			this.entrancePermitted = entrancePermitted;
            this.eventTime = eventtime;
		}
    }
}
