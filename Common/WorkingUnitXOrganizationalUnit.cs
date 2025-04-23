using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using Util;
using TransferObjects;
using System.Data;

namespace Common
{
    public class WorkingUnitXOrganizationalUnit
    {
        DAOFactory daoFactory = null;
		WorkingUnitXOrganizationalUnitDAO dao = null;

		DebugLog log;

        WorkingUnitXOrganizationalUnitTO wuXouTO = new WorkingUnitXOrganizationalUnitTO();

        public WorkingUnitXOrganizationalUnitTO WUXouTO
		{
			get { return wuXouTO; }
			set {wuXouTO = value; }
		}

		public WorkingUnitXOrganizationalUnit()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			dao = daoFactory.getWorkingUnitXOrganizationalUnitDAO(null);
		}

        public WorkingUnitXOrganizationalUnit(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            dao = daoFactory.getWorkingUnitXOrganizationalUnitDAO(dbConnection);
        }

        public WorkingUnitXOrganizationalUnit(int orgUnitID, int wuID, string purpose)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            dao = daoFactory.getWorkingUnitXOrganizationalUnitDAO(null);

            this.wuXouTO.OrgUnitID = orgUnitID;
			this.wuXouTO.WorkingUnitID = wuID;
			this.wuXouTO.Purpose = purpose;
		}

		public int Save(WorkingUnitXOrganizationalUnitTO wuXouTO)
		{
			int inserted;
			try
			{
                inserted = dao.insert(wuXouTO);
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " WorkingUnitXOrganizationalUnit.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}

        public int Save(WorkingUnitXOrganizationalUnitTO wuXouTO, bool doCommit)
		{
			int inserted;
			try
			{
				inserted = dao.insert(wuXouTO,doCommit);
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " WorkingUnitXOrganizationalUnit.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}

        public List<WorkingUnitXOrganizationalUnitTO> Search()
		{
			try
			{				
				return dao.getWUXOU(this.wuXouTO);				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

	
		public bool Delete(int orgUnitID, int wuID)
		{
			bool isDeleted = false;

			try
			{
                isDeleted = dao.delete(orgUnitID, wuID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public bool Delete(int orgUnitID, int wuID, bool doCommit)
		{
			bool isDeleted = false;

			try
			{
				isDeleted = dao.delete(orgUnitID,wuID, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}
       

		public bool BeginTransaction()
		{
			bool isStarted = false;

			try
			{
				isStarted = dao.beginTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.BeginTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isStarted;
		}

		public void CommitTransaction()
		{
			try
			{
				dao.commitTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.CommitTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void RollbackTransaction()
		{
			try
			{
				dao.rollbackTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.RollbackTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public IDbTransaction GetTransaction()
		{
			try
			{
				return dao.getTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.GetTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void SetTransaction(IDbTransaction trans)
		{
			try
			{
				dao.setTransaction(trans);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.SetTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
        
		public void Clear()
		{
			this.wuXouTO = new WorkingUnitXOrganizationalUnitTO();
		}
	
		
    }
}
