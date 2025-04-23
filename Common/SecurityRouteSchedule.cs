using System;
using System.Collections;
using System.Text;
using System.Data;

using Util;
using DataAccess;
using TransferObjects;

namespace Common
{
    public class SecurityRouteSchedule
    {
        private int _employeeID;
        private string _employeeName;
        private DateTime _date;
        private int _securityRouteID;
        private string _routeName;
        private string _type;
        private string _status;

        private DAOFactory daoFactory;
        private SecurityRouteScheduleDAO secRouteSchDAO;
        DebugLog log;

        public int EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }

        public string EmployeeName
        {
            get { return _employeeName; }
            set { _employeeName = value; }
        }

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public int SecurityRouteID
        {
            get { return _securityRouteID; }
            set { _securityRouteID = value; }
        }

        public string RouteName
        {
            get { return _routeName; }
            set { _routeName = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public SecurityRouteSchedule()
		{
            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            secRouteSchDAO = daoFactory.getSecurityRouteScheduleDAO(null);

			// Init properties
            EmployeeID = -1;
            EmployeeName = "";
            Date = new DateTime();
            SecurityRouteID = -1;
			RouteName = "";
            Type = "";
            Status = "";

		}
        public SecurityRouteSchedule(object dbConnection)
        {
            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            secRouteSchDAO = daoFactory.getSecurityRouteScheduleDAO(dbConnection);

            // Init properties
            EmployeeID = -1;
            EmployeeName = "";
            Date = new DateTime();
            SecurityRouteID = -1;
            RouteName = "";
            Type = "";
            Status = "";

        }

        public void ReceiveTransferObject(SecurityRouteScheduleTO secRouteSchTO)
        {
            try
            {
                this.EmployeeID = secRouteSchTO.EmployeeID;
                this.EmployeeName = secRouteSchTO.EmployeeName;
                this.Date = secRouteSchTO.Date;
                this.SecurityRouteID = secRouteSchTO.SecurityRouteID;
                this.RouteName = secRouteSchTO.RouteName;
                this.Type = secRouteSchTO.Type;
                this.Status = secRouteSchTO.Status;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRouteSchedule.ReceiveTransferObject(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public SecurityRouteScheduleTO SendTransferObject()
        {
            SecurityRouteScheduleTO secRouteSchTO = new SecurityRouteScheduleTO();

            try
            {
                secRouteSchTO.EmployeeID = this.EmployeeID;
                secRouteSchTO.EmployeeName = this.EmployeeName;
                secRouteSchTO.Date = this.Date;
                secRouteSchTO.SecurityRouteID = this.SecurityRouteID;
                secRouteSchTO.RouteName = this.RouteName;
                secRouteSchTO.Type = this.Type;
                secRouteSchTO.Status = this.Status;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRouteSchedule.SendTransferObject(): " + ex.Message + "\n");
                throw ex;
            }

            return secRouteSchTO;
        }

        public ArrayList Search(int emplID, int routeID, DateTime from, DateTime to)
        {
            // List that contins TO object
            ArrayList routesSchTO = new ArrayList();
            ArrayList routesSch = new ArrayList();

            try
            {
                routesSchTO = secRouteSchDAO.getRoutesSch(emplID, routeID, from, to);
                SecurityRouteSchedule member;

                foreach (SecurityRouteScheduleTO routeSchTO in routesSchTO)
                {
                    member = new SecurityRouteSchedule();
                    member.ReceiveTransferObject(routeSchTO);

                    routesSch.Add(member);
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesSchedule.Search(): " + ex.Message + "\n");
                throw ex;
            }

            return routesSch;
        }

        public bool Delete(SecurityRouteSchedule schedule)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = this.secRouteSchDAO.delete(schedule.EmployeeID, schedule.SecurityRouteID, schedule.Date);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRouteSchedule.Delete(): " + ex.Message + "\n");
                throw ex;
            }

            return isDeleted;
        }

        public bool Delete(int emplID, DateTime date, bool doCommit)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = this.secRouteSchDAO.delete(emplID, date, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRouteSchedule.Delete(): " + ex.Message + "\n");
                throw ex;
            }

            return isDeleted;
        }

        public int Save(int emplID, int routeID, DateTime date, bool doCommit)
        {
            int rowsAffected = 0;

            try
            {
                rowsAffected = this.secRouteSchDAO.insert(emplID, routeID, date, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRouteSchedule.Insert(): " + ex.Message + "\n");
                throw ex;
            }

            return rowsAffected;
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = secRouteSchDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRouteSchedule.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                secRouteSchDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRouteSchedule.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                secRouteSchDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRouteSchedule.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return secRouteSchDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRouteSchedule.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                secRouteSchDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRouteSchedule.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
