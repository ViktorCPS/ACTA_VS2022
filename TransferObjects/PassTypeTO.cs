using System;
using System.Runtime.Serialization;

using System.Xml;
using System.Xml.Serialization;

namespace TransferObjects
{
    /// <summary>
    /// Summary description for PassTypeTO.
    /// </summary>
    [XmlRootAttribute()]
    [DataContract]
    public class PassTypeTO
    {
        private int _passTypeId = -1;
        private string _description = "";
        private int _button = -1;
        private int _isPass = -1;
        private string _paymentCode = "";
        private string descAlt = "";
        private string segmentColor = "";
        private int limitCompositeID = -1;
        private int limitElementaryID = -1;
        private int limitOccasionID = -1;
        private int wuID = -1;
        private int confirmFlag = -1;
        private int mssiveInput = -1;
        private int manualInputFlag = -1;
        private int verivicationFlag = -1;

        [XmlAttributeAttribute(AttributeName = "PassTypeID")]
        [DataMember]
        public int PassTypeID
        {
            get { return _passTypeId; }
            set { _passTypeId = value; }
        }

        [DataMember]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        [DataMember]
        public string DescriptionAndID
        {
            get 
            {
                string descAndCode = "";

                if (!_paymentCode.Trim().Equals(""))
                    descAndCode = "(" + _paymentCode + ") " + _description;
                else
                    descAndCode = _description;

                return descAndCode; 
            }
        }
        [DataMember]
        public string DescriptionAltAndID
        {
            get
            {
                string descAltAndCode = "";

                if (!_paymentCode.Trim().Equals(""))
                    descAltAndCode = "(" + _paymentCode + ") " + descAlt;
                else
                    descAltAndCode = descAlt;

                return descAltAndCode;
            }
        }
        [DataMember]
        public int Button
        {
            get { return _button; }
            set { _button = value; }
        }

        [DataMember]
        public int IsPass
        {
            get { return _isPass; }
            set { _isPass = value; }
        }

        [DataMember]
        public string PaymentCode
        {
            get { return _paymentCode; }
            set { _paymentCode = value; }
        }

        public string DescAlt
        {
            get { return descAlt; }
            set { descAlt = value; }
        }

        public string SegmentColor
        {
            get { return segmentColor; }
            set { segmentColor = value; }
        }

        public int LimitCompositeID
        {
            get { return limitCompositeID; }
            set { limitCompositeID = value; }
        }

        public int LimitElementaryID
        {
            get { return limitElementaryID; }
            set { limitElementaryID = value; }
        }

        public int LimitOccasionID
        {
            get { return limitOccasionID; }
            set { limitOccasionID = value; }
        }

        public int WUID
        {
            get { return wuID; }
            set { wuID = value; }
        }

        public int ConfirmFlag
        {
            get { return confirmFlag; }
            set { confirmFlag = value; }
        }

        public int MassiveInput
        {
            get { return mssiveInput; }
            set { mssiveInput = value; }
        }

        public int ManualInputFlag
        {
            get { return manualInputFlag; }
            set { manualInputFlag = value; }
        }

        public int VerificationFlag
        {
            get { return verivicationFlag; }
            set { verivicationFlag = value; }
        }

        public PassTypeTO()
        {
        }

        public PassTypeTO(int passTypeId, string description,
            int button, int isPass, string paymentCode)
        {
            this.PassTypeID = passTypeId;
            this.Description = description;
            this.Button = button;
            this.IsPass = isPass;
            this.PaymentCode = paymentCode;
        }
    }
}
