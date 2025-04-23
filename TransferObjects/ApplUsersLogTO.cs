using System;
using System.Xml;
using System.Xml.Serialization;
using TransferObjects;

namespace TransferObjects
{
	/// <summary>
	/// Summary description for ApplUsersLog.
	/// </summary>
	public class ApplUserLogTO
	{
		private int _sessionID = 0;
		private DateTime _logInTime = new DateTime();
		private DateTime _logOutTime = new DateTime();
		private string _host = "";
		private string _userID = "";
        private string _userName = "";
        private TimeSpan _duration = new TimeSpan();
        private string _loginChanel = "";
        private string _loginStatus = "";
        private string _loginType = "";
        private int _loginChange = -1;
        
		public int SessionID
		{
			get{ return _sessionID; }
			set{ _sessionID = value; }
		}

		public DateTime LogInTime
		{
			get { return _logInTime; }
			set {_logInTime = value; }
		}

		public DateTime LogOutTime
		{
			get { return _logOutTime; }
			set {_logOutTime = value; }
		}

		public string Host
		{
			get { return _host; }
			set {_host = value; }
		}

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public TimeSpan Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

		public string UserID
		{
			get { return _userID; }
			set {_userID = value; }
		}

        public string LoginChanel
        {
            get { return _loginChanel; }
            set { _loginChanel = value; }
        }

        public string LoginStatus
        {
            get { return _loginStatus; }
            set { _loginStatus = value; }
        }

        public string LoginType
        {
            get { return _loginType; }
            set { _loginType = value; }
        }

        public int LoginChange
        {
            get { return _loginChange; }
            set { _loginChange = value; }
        }
        
		public ApplUserLogTO()
		{
			
		}		
		
		public ApplUserLogTO(int sessionID, DateTime loginTime, DateTime logoutTime, string hostIP)
		{
			this.SessionID = sessionID;
			this.LogInTime = loginTime;
			this.LogOutTime = logoutTime;
			this.Host = hostIP;
		}
	}
}
