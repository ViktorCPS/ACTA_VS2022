using System;
using System.Collections;
using System.Text;

using TransferObjects;
using Util;

namespace Common
{
    public class SecurityRouteDtl
    {
        private int _securityRouteID;
        private int _segmentID;
        private int _gateID;
        private string _gateName;
        private DateTime _timeFrom;
        private DateTime _timeTo;

        DebugLog log;

        public int SecurityRouteID
        {
            get { return _securityRouteID; }
            set { _securityRouteID = value; }
        }

        public int SegmentID
        {
            get { return _segmentID; }
            set { _segmentID = value; }
        }

        public int GateID
        {
            get { return _gateID; }
            set { _gateID = value; }
        }

        public string GateName
        {
            get { return _gateName; }
            set { _gateName = value; }
        }

        public DateTime TimeFrom
        {
            get { return _timeFrom; }
            set { _timeFrom = value; }
        }

        public DateTime TimeTo
        {
            get { return _timeTo; }
            set { _timeTo = value; }
        }

        public SecurityRouteDtl()
		{
            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

			// Init properties
            SecurityRouteID = -1;
            SegmentID = -1;
            GateID = -1;
            GateName = "";
            TimeFrom = new DateTime();
            TimeTo = new DateTime();
		}

        public void ReceiveTransferObject(SecurityRouteDtlTO secRouteTO)
        {
            try
            {
                this.SecurityRouteID = secRouteTO.SecurityRouteID;
                this.SegmentID = secRouteTO.SegmentID;
                this.GateID = secRouteTO.GateID;
                this.GateName = secRouteTO.GateName;
                this.TimeFrom = secRouteTO.TimeFrom;
                this.TimeTo = secRouteTO.TimeTo;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRouteDtl.ReceiveTransferObject(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public SecurityRouteDtlTO SendTransferObject()
        {
            SecurityRouteDtlTO secRouteTO = new SecurityRouteDtlTO();

            try
            {
                secRouteTO.SecurityRouteID = this.SecurityRouteID;
                secRouteTO.SegmentID = this.SegmentID;
                secRouteTO.GateID = this.GateID;
                secRouteTO.GateName = this.GateName;
                secRouteTO.TimeFrom = this.TimeFrom;
                secRouteTO.TimeTo = this.TimeTo;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRouteDtl.SendTransfereObject(): " + ex.Message + "\n");
                throw ex;
            }

            return secRouteTO;
        }
    }
}
