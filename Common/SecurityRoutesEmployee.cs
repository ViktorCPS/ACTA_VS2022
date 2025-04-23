using System;
using System.Collections;
using System.Text;
using System.Data;

using TransferObjects;
using Util;
using DataAccess;

namespace Common
{
    public class SecurityRoutesEmployee
    {
        private int _employeeID;
        private string _employeeName;
        private string _wuName;

        private DAOFactory daoFactory;
        private SecurityRoutesEmployeeDAO secRouteEmplDAO;
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

        public string WUName
        {
            get { return _wuName; }
            set { _wuName = value; }
        }

        public SecurityRoutesEmployee()
        {
            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            secRouteEmplDAO = daoFactory.getSecurityRoutesEmployeeDAO(null);
			
            // Init properties
            EmployeeID = -1;
            EmployeeName = "";
            WUName = "";
        }
        public SecurityRoutesEmployee(object dbConnection)
        {
            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            secRouteEmplDAO = daoFactory.getSecurityRoutesEmployeeDAO(dbConnection);

            // Init properties
            EmployeeID = -1;
            EmployeeName = "";
            WUName = "";
        }

        public void ReceiveTransferObject(SecurityRoutesEmployeeTO secRouteEmployeeTO)
        {
            try
            {
                this.EmployeeID = secRouteEmployeeTO.EmployeeID;
                this.EmployeeName = secRouteEmployeeTO.EmployeeName;
                this.WUName = secRouteEmployeeTO.WUName;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesEmployee.ReceiveTransferObject(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public SecurityRoutesEmployeeTO SendTransferObject()
        {
            SecurityRoutesEmployeeTO secRouteEmployeeTO = new SecurityRoutesEmployeeTO();

            try
            {
                secRouteEmployeeTO.EmployeeID = this.EmployeeID;
                secRouteEmployeeTO.EmployeeName = this.EmployeeName;
                secRouteEmployeeTO.WUName = this.WUName;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesEmployee.SendTransferObject(): " + ex.Message + "\n");
                throw ex;
            }

            return secRouteEmployeeTO;
        }

        public ArrayList SearchByWU(string wUnits)
        {
            ArrayList routeEmployeeTOList = new ArrayList();
            ArrayList routeEmployeeList = new ArrayList();

            try
            {
                SecurityRoutesEmployee routeEmplMember = new SecurityRoutesEmployee();

                routeEmployeeTOList = secRouteEmplDAO.getEmployeesByWU(wUnits);

                foreach (SecurityRoutesEmployeeTO routeEmplTO in routeEmployeeTOList)
                {
                    routeEmplMember = new SecurityRoutesEmployee();
                    routeEmplMember.ReceiveTransferObject(routeEmplTO);

                    routeEmployeeList.Add(routeEmplMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesEmployee.SearchByWU(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return routeEmployeeList;
        }

        public ArrayList Search()
        {
            ArrayList routeEmployeeTOList = new ArrayList();
            ArrayList routeEmployeeList = new ArrayList();

            try
            {
                SecurityRoutesEmployee routeEmplMember = new SecurityRoutesEmployee();

                routeEmployeeTOList = secRouteEmplDAO.getEmployees();

                foreach (SecurityRoutesEmployeeTO routeEmplTO in routeEmployeeTOList)
                {
                    routeEmplMember = new SecurityRoutesEmployee();
                    routeEmplMember.ReceiveTransferObject(routeEmplTO);

                    routeEmployeeList.Add(routeEmplMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesEmployee.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return routeEmployeeList;
        }

        public int Save(string employeeID)
        {
            int rowsAffected = 0;

            try
            {
                rowsAffected = this.secRouteEmplDAO.insert(employeeID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesEmployee.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return rowsAffected;
        }

        public bool Delete(string employeeID)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = this.secRouteEmplDAO.delete(employeeID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesEmployee.Delete(): " + ex.Message + "\n");
                throw ex;
            }

            return isDeleted;
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = secRouteEmplDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesEmployee.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                secRouteEmplDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesEmployee.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                secRouteEmplDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesEmployee.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return secRouteEmplDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesEmployee.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                secRouteEmplDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesEmployee.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
