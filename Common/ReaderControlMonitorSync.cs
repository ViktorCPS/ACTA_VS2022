using System;
using System.Xml.Serialization;
using System.IO;

using Util;

namespace Common
{
	/// <summary>
	/// ReaderControlMonitorFileSync is responsible for synchronization of a gate access among ReaderControl 
	/// and Monitor applications.
	/// </summary>
	[Serializable]
	[XmlRoot("ReaderControlMonitorSync")]
	public class ReaderControlMonitorSync
	{
		[XmlElement("ReaderControlRequest")]
		public string readerControlRequest;

		[XmlElement("Monitoring")]
		public string monitoring;

        public string gate;

    	protected DateTime lastWriteTime;
		protected string syncFile;
        protected Object _DBConnection;

		protected DebugLog log = null;

        public ReaderControlMonitorSync()
        {
        }

        public ReaderControlMonitorSync(string RCRequest, string monitoring)
		{
			this.readerControlRequest = RCRequest;
			this.monitoring = monitoring;
		}

        public ReaderControlMonitorSync(string gate)
        {
            log = new DebugLog(Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt");

            this.gate = gate;
            this.syncFile = Constants.MonitoringPath + "GateSync" + gate + ".xml";
            lastWriteTime = DateTime.MinValue;
        }

        public string SyncFile
		{
			get { return syncFile; }
		}

        public Object DBConnection
        {
            get { return _DBConnection; }
        }

        public virtual bool CreateGateSync()
        {
            return false;
        }

        public virtual bool WriteGateSync(ReaderControlMonitorSync rcmSync)
        {
            return false;
        }

        public virtual ReaderControlMonitorSync ReadGateSync()
        {
            return new ReaderControlMonitorSync("YES", "NO");
        }

        public virtual bool DeleteGateSync()
        {
            return false;
        }

        public virtual bool CanSynchronize()
        {
            return false;
        }
	}
}
