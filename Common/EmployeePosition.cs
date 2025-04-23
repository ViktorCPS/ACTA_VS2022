using System;
using System.Collections.Generic;
using System.Text;
using Util;
using System.Data;

using TransferObjects;
using DataAccess;

namespace Common
{
    public class EmployeePosition
    {
        DAOFactory daoFactory = null;
        EmployeePositionDAO edao = null;
		
		DebugLog log;

        EmployeePositionTO emplPositionTO = new EmployeePositionTO();

        public EmployeePositionTO EmplPositionTO
        {
            get { return emplPositionTO; }
            set { emplPositionTO = value; }
        }
        		
		public EmployeePosition()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			edao = daoFactory.getEmployeePositionDAO(null);
		}

        public EmployeePosition(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getEmployeePositionDAO(dbConnection);
        }

        public EmployeePosition(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                edao = daoFactory.getEmployeePositionDAO(null);
            }
        }

        public int Save(bool doCommit)
        {
            try
            {
                return edao.insert(this.EmplPositionTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePosition.Save(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Update(bool doCommit)
        {
            try
            {
                return edao.update(this.EmplPositionTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePosition.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Delete(int posID, bool doCommit)
        {
            try
            {
                return edao.delete(posID, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePosition.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeePositionTO> SearchEmployeePositions()
        {           
            try
            {
                return edao.getEmployeePositions(this.EmplPositionTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePosition.SearchEmployeePositions(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public Dictionary<int,EmployeePositionTO> SearchEmployeePositionsDictionary()
        {
            try
            {
                return edao.getEmployeePositionsDictionary(this.EmplPositionTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePosition.SearchEmployeePositions(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = edao.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePosition.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                edao.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePosition.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                edao.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePosition.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return edao.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePosition.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                edao.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePosition.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
