using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class ValidationTerminalTO
    {
        private int _validationTerminalID = -1;
        private string _description = "";
        private string _name = "";
        private double _blockPassInterval = -1;
        private long _ticketTransactionTimeOut = 0;
        private int _passTimeOut = -1;
        private double _pingPeriod = -1;
        private int _logDebugMessages = -1;
        private string _ipAddress = "";
        private string _status = "";
        private string _cameraIP1 = "";
        private string _cameraIP2 = "";
        private string _cameraIP3 = "";
        private string _cameraIP4 = "";
        private int _screenTimeOut = 0;
        private string numberOfMealsTime = "";

        public string NumberOfMealsTime
        {
            get { return numberOfMealsTime; }
            set { numberOfMealsTime = value; }
        }

        private string downloadStartTime = "";

        public string DownloadStartTime
        {
            get { return downloadStartTime; }
            set { downloadStartTime = value; }
        }
        private int downloadInterval = 0;

        public int DownloadInterval
        {
            get { return downloadInterval; }
            set { downloadInterval = value; }
        }

        public int ScreenTimeOut
        {
            get { return _screenTimeOut; }
            set { _screenTimeOut = value; }
        }

        private Dictionary<int, string> _locations = new Dictionary<int, string>();

        public Dictionary<int, string> Locations
        {
            get { return _locations; }
            set { _locations = value; }
        }

        public string CameraIP4
        {
            get { return _cameraIP4; }
            set { _cameraIP4 = value; }
        }

        public string CameraIP3
        {
            get { return _cameraIP3; }
            set { _cameraIP3 = value; }
        }

        public string CameraIP2
        {
            get { return _cameraIP2; }
            set { _cameraIP2 = value; }
        }

        public string CameraIP1
        {
            get { return _cameraIP1; }
            set { _cameraIP1 = value; }
        }
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public string IpAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        public int LogDebugMessages
        {
            get { return _logDebugMessages; }
            set { _logDebugMessages = value; }
        }

        public double PingPeriod
        {
            get { return _pingPeriod; }
            set { _pingPeriod = value; }
        }

        public int PassTimeOut
        {
            get { return _passTimeOut; }
            set { _passTimeOut = value; }
        }

        public long TicketTransactionTimeOut
        {
            get { return _ticketTransactionTimeOut; }
            set { _ticketTransactionTimeOut = value; }
        }

        public double BlockPassInterval
        {
            get { return _blockPassInterval; }
            set { _blockPassInterval = value; }
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

        public int ValidationTerminalID
        {
            get { return _validationTerminalID; }
            set { _validationTerminalID = value; }
        }
    }
}
