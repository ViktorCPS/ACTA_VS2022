using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DataAccess;
using Util;
using TransferObjects;

namespace Common
{
    public class EmployeeTypeVisibility
    {
        DAOFactory daoFactory = null;
        EmployeeTypeVisibilityDAO edao = null;

        DebugLog log;

        EmployeeTypeVisibilityTO emplTypeVisibilityTO = new EmployeeTypeVisibilityTO();

        public EmployeeTypeVisibilityTO EmplTypeVisibilityTO
        {
            get { return emplTypeVisibilityTO; }
            set { emplTypeVisibilityTO = value; }
        }

        public EmployeeTypeVisibility()
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getEmployeeTypeVisibilityDAO(null);
        }

        public EmployeeTypeVisibility(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getEmployeeTypeVisibilityDAO(dbConnection);
        }

        public EmployeeTypeVisibility(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                edao = daoFactory.getEmployeeTypeVisibilityDAO(null);
            }
        }

        public Dictionary<int, List<int>> SearchCompanyVisibleTypes(int catID)
        {
            try
            {
                return edao.getCompanyVisibleTypes(catID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeTypeVisibility.SearchCompanyVisibleTypes(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeTypeVisibility.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeTypeVisibility.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeTypeVisibility.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeTypeVisibility.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeTypeVisibility.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
