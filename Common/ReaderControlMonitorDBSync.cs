using System;
using System.Collections;
using System.Configuration;
using System.Data;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
	/// <summary>
	/// ReaderControlMonitorFileSync is responsible for synchronization of a gate access among ReaderControl 
	/// and Monitor applications. The synchronization is based on GatesSync database table.
	/// </summary>
    public class ReaderControlMonitorDBSync : ReaderControlMonitorSync
	{
        private DAOFactory daoFactory = null;
        private GateSyncDAO gateSyncDAO = null;

        private bool readSync = true;

		public ReaderControlMonitorDBSync(string gate) : base(gate)
		{
            daoFactory = DAOFactory.getDAOFactory();
            gateSyncDAO = daoFactory.getGateSyncDAO(null);
            try
            {
                _DBConnection = DBConnectionManager.Instance.MakeNewDBConnection();
                gateSyncDAO.SetDBConnection(_DBConnection);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ReaderControlMonitorDBSync.Constructor(): " + ex.Message + "\n");
            }
		}

        public ReaderControlMonitorDBSync(string gate, Object dbConnection)
            : base(gate)
        {
            daoFactory = DAOFactory.getDAOFactory();
            gateSyncDAO = daoFactory.getGateSyncDAO(dbConnection);
            try
            {
                _DBConnection = dbConnection;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ReaderControlMonitorDBSync.Constructor(): " + ex.Message + "\n");
            }
        }

        public override bool CreateGateSync()
        {
            bool success = false;
            try
            {
                success = gateSyncDAO.Insert(gate, "NO", "YES");
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ReaderControlMonitorDBSync.CreateGateSync(): " + ex.Message + "\n");
            }
            return success;
        }

        public override bool WriteGateSync(ReaderControlMonitorSync rcmSync) 
		{
            bool success = false;
            try
            {
                success = gateSyncDAO.Update(gate, rcmSync.readerControlRequest, rcmSync.monitoring);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ReaderControlMonitorDBSync.WriteGateSync(): " + ex.Message + "\n");
            }
			return success;
		}

		public override ReaderControlMonitorSync ReadGateSync() 
		{
			ReaderControlMonitorSync rcmSync = null;
            try
            {
                ArrayList gateSyncTOList = gateSyncDAO.Search(gate);
                if (gateSyncTOList.Count == 1)
                {
                    rcmSync = new ReaderControlMonitorSync(((GateSyncTO)gateSyncTOList[0]).ReaderControlRequest,
                        ((GateSyncTO)gateSyncTOList[0]).Monitoring);
                    if (readSync == false)
                    {
                        log.writeLog(DateTime.Now + " ReaderControlMonitorDBSync.ReadGateSync(): read gate sync succeeded.\n");
                        readSync = true;
                    }
                }
            }
            catch (Exception ex)
            {
                if (readSync == true)
                {
                    log.writeLog(DateTime.Now + " ReaderControlMonitorDBSync.ReadGateSync(): " + ex.Message + "\n");
                    readSync = false;
                }
            }
			return rcmSync;
		}

        public override bool DeleteGateSync()
        {
            bool deleted = false;
            try
            {
                deleted = gateSyncDAO.Delete(gate);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ReaderControlMonitorDBSync.DeleteGateSync(): " + ex.Message + "\n");
            }
            return deleted;
        }

        public override bool CanSynchronize()
        {
            int count = -1;
            try
            {
                count = gateSyncDAO.Count(gate);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ReaderControlMonitorDBSync.CanSynchronize(): " + ex.Message + "\n");
            }
            return (count == 1);
        }

        public bool IsDBSyncPossible()
        {
            int count = -1;
            try
            {
                count = gateSyncDAO.Count(gate);
                if (count == -1)
                {
                    log.writeLog(DateTime.Now + " ReaderControlMonitorDBSync.IsDBSyncPossible(): Can't synchronize by DB.\n");
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ReaderControlMonitorDBSync.IsDBSyncPossible(): " + ex.Message + "\n");
            }
            return (count != -1);
        }
	}
}
