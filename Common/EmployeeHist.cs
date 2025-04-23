using System;
using System.Collections.Generic;
using System.Text;
using Util;
using TransferObjects;
using DataAccess;
using System.Data;

namespace Common
{
    public class EmployeeHist
    {

DAOFactory daoFactory = null;
		EmployeeHistDAO edao = null;
		
		DebugLog log;

        EmployeeHistTO emplHistTO = new EmployeeHistTO();
        
        public EmployeeHistTO EmplHistTO
        {
            get { return emplHistTO; }
            set { emplHistTO = value; }
        }

        public EmployeeHist()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			edao = daoFactory.getEmployeeHistDAO(null);
		}
        public EmployeeHist(Object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getEmployeeHistDAO(dbConnection);
        }
        /// <summary>
        /// Save Employee 
        /// </summary>
        /// <returns></returns>
        public int Save(string EmployeeID, string FirstName, string LastName, string WorkingUnitID,
            string Status, string Password, string AddressID, string Picture, string WorkingGroupID,
            string type, string accessGroupID, string EmployeeTypeID, string orgUnitID, string createdBy, DateTime createdTime, DateTime validTo, bool doCommit)
        {
            int saved = 0;
            try
            {
                saved = edao.insert(EmployeeID, FirstName, LastName,
                    WorkingUnitID, Status, Password, AddressID, Picture, WorkingGroupID, type, accessGroupID, EmployeeTypeID, orgUnitID, createdBy, createdTime, validTo, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeHist.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return saved;
        }        

        public int Save(bool doCommit)
        {            
            try
            {
                return edao.insert(this.EmplHistTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeHist.Save(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public EmployeeHistTO SearchDateEmployee(DateTime date, EmployeeTO emplTO)
        {
            try
            {
                return edao.getDateEmployee(date, emplTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeHist.SearchDateEmployee(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public Dictionary<int, List<EmployeeHistTO>> SearchEmployeeChanges(DateTime from, DateTime to, string emplIDs)
        {
            try
            {
                return edao.getEmployeeChanges(from, to, emplIDs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeHist.SearchEmployeeChanges(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public DateTime SearchRetiredDate(int emplID)
        {
            try
            {
                return edao.getRetiredDate(emplID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeHist.SearchRetiredDate(): " + ex.Message + "\n");
                throw ex;
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
                log.writeLog(DateTime.Now + " EmployeeHist.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeHist.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeHist.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeHist.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeHist.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
