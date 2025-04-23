using System;
using System.Collections;
using System.Text;

using TransferObjects;
using Util;
using DataAccess;
using System.Data;
using System.Collections.Generic;

namespace Common
{
    public class SecurityRoutesLog
    {
        private int _logID;
        private int _readerID;
        private string _tagID;
        private int _employeeID;
        private int _pointID;
        private DateTime _eventTime;

        private DAOFactory daoFactory;
        private SecurityRoutesLogDAO secRouteLogDAO;
        DebugLog log;

        private string _employeeName;
        private string _pointName;

        public string PointName
        {
            get { return _pointName; }
            set { _pointName = value; }
        }

        public string EmployeeName
        {
            get { return _employeeName; }
            set { _employeeName = value; }
        }

        public int LogID
        {
            get { return _logID; }
            set { _logID = value; }
        }

        public int ReaderID
        {
            get { return _readerID; }
            set { _readerID = value; }
        }

        public string TagID
        {
            get { return _tagID; }
            set { _tagID = value; }
        }

        public int EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }

        public int PointID
        {
            get { return _pointID; }
            set { _pointID = value; }
        }

        public DateTime EventTime
        {
            get { return _eventTime; }
            set { _eventTime = value; }
        }

        public SecurityRoutesLog()
        {
            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            secRouteLogDAO = daoFactory.getSecurityRoutesLogDAO(null);
			
            // Init properties
            LogID = -1;
            ReaderID = -1;
            TagID = "";
            EmployeeID = -1;
            PointID = -1;
            EventTime = new DateTime();
        }

        public SecurityRoutesLog(object dbConnection)
        {
            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            secRouteLogDAO = daoFactory.getSecurityRoutesLogDAO(dbConnection);

            // Init properties
            LogID = -1;
            ReaderID = -1;
            TagID = "";
            EmployeeID = -1;
            PointID = -1;
            EventTime = new DateTime();
        }

        public void ReceiveTransferObject(SecurityRoutesLogTO secRouteLogTO)
        {
            try
            {
                this.LogID = secRouteLogTO.LogID;
                this.TagID = secRouteLogTO.TagID;
                this.ReaderID = secRouteLogTO.ReaderID;
                this.EmployeeID = secRouteLogTO.EmployeeID;
                this.EventTime = secRouteLogTO.EventTime;
                this.PointID = secRouteLogTO.PointID;
                this.EmployeeName = secRouteLogTO.EmployeeName;
                this.PointName = secRouteLogTO.PointName;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesLog.ReceiveTransferObject(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public SecurityRoutesLogTO SendTransferObject()
        {
            SecurityRoutesLogTO secRouteLogTO = new SecurityRoutesLogTO();

            try
            {
                secRouteLogTO.ReaderID = this.ReaderID;
                secRouteLogTO.TagID = this.TagID;
                secRouteLogTO.EmployeeID = this.EmployeeID;
                secRouteLogTO.LogID = this.LogID;
                secRouteLogTO.EventTime = this.EventTime;
                secRouteLogTO.PointID = this.PointID;
                secRouteLogTO.EmployeeName = this.EmployeeName;
                secRouteLogTO.PointName = this.PointName;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesLog.SendTransferObject(): " + ex.Message + "\n");
                throw ex;
            }

            return secRouteLogTO;
        }

        public ArrayList SearchInterval(int employeeID, int readerID,string tagID, DateTime fromTime, DateTime toTime, string wUnits)
        {
            ArrayList logTOList = new ArrayList();
            ArrayList logList = new ArrayList();

            try
            {
                SecurityRoutesLog logMember = new SecurityRoutesLog();
                logTOList = secRouteLogDAO.getLogsInterval(employeeID, readerID, tagID, fromTime, toTime, wUnits);

                foreach (SecurityRoutesLogTO logTO in logTOList)
                {
                    logMember = new SecurityRoutesLog();
                    logMember.ReceiveTransferObject(logTO);

                    logList.Add(logMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesLog.SearchInterval(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return logList;
        }

        public int SearchEmplCount(string employeeID)
        {
            int count = 0;

            try
            {
                count = secRouteLogDAO.getEmplCount(employeeID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesLog.SearchEmplCount(): " + ex.Message + "\n");
                throw ex;
            }

            return count;
        }

        public int Save(int readerID, string tagID, int employeeID, DateTime eventTime)
        {
            int rowsAffected = 0;

            try
            {
                rowsAffected = secRouteLogDAO.insert(readerID, tagID, employeeID, eventTime);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesLog.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return rowsAffected;
        }
        public int Save(int readerID, string tagID, int employeeID, DateTime eventTime,bool doCommit)
        {
            int rowsAffected = 0;

            try
            {
                rowsAffected = secRouteLogDAO.insert(readerID, tagID, employeeID, eventTime,doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesLog.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return rowsAffected;
        }
        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = secRouteLogDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesLog.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                secRouteLogDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesLog.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                secRouteLogDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesLog.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return secRouteLogDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesLog.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                secRouteLogDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesLog.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }


        public ArrayList Search(int EmployeeID, string tagID, string workingUnitID, DateTime dateFrom, DateTime dateTo, DateTime fromTime, DateTime toTime)
        {
            ArrayList list = new ArrayList();
            try
            {
                list = secRouteLogDAO.search(EmployeeID, tagID, workingUnitID, dateFrom, dateTo, fromTime, toTime);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesLog.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return list;
        }
    }
}
