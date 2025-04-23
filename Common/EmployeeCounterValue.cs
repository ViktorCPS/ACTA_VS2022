using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
    public class EmployeeCounterValue
    {
        DAOFactory daoFactory = null;
		EmployeeCounterValueDAO valueDAO = null;

		DebugLog log;

        EmployeeCounterValueTO valueTO = new EmployeeCounterValueTO();

		public EmployeeCounterValueTO ValueTO
		{
            get { return valueTO; }
			set{ valueTO = value; }
		}

        public EmployeeCounterValue()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            valueDAO = daoFactory.getEmployeeCounterValueDAO(null);
		}

        public EmployeeCounterValue(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            valueDAO = daoFactory.getEmployeeCounterValueDAO(dbConnection);
        }

        public int Save()
        {            
            try
            {
                return valueDAO.insert(this.ValueTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValue.Save(): " + ex.Message + "\n");
                throw ex;
            }
        }
        public int Save(bool doCommit)
        {
            try
            {
                return valueDAO.insert(this.ValueTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValue.Save(): " + ex.Message + "\n");
                throw ex;
            }
        }
        public List<EmployeeCounterValueTO> Search()
        {
            try
            {
                return valueDAO.getEmplCounterValues(this.ValueTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValue.Search(): " + ex.Message + "\n");
                throw ex;
            }
        }
        public List<EmployeeCounterValueTO> SearchNegative(string emplIDs)
        {
            try
            {
                return valueDAO.getEmplCounterValuesNegative(emplIDs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValue.SearchNegative(): " + ex.Message + "\n");
                throw ex;
            }
        }
        public List<EmployeeCounterValueTO> SearchModifiedValues(DateTime fromDate)
        {
            try
            {
                return valueDAO.getModifiedValues(fromDate);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValue.Search(): " + ex.Message + "\n");
                throw ex;
            }
        }
        public Dictionary<int, Dictionary<int, int>> Search(string emplIDs)
        {
            try
            {
                return valueDAO.getEmplCounterValues(emplIDs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValue.Search(): " + ex.Message + "\n");
                throw ex;
            }
        }
        public Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> SearchValues(string emplIDs, IDbTransaction trans)
        {
            try
            {
                return valueDAO.getEmplCounterValuesTO(emplIDs,trans );
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValue.SearchValues(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> SearchValues(string emplIDs)
        {
            try
            {
                return valueDAO.getEmplCounterValuesTO(emplIDs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValue.SearchValues(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> SearchValuesAll()
        {
            try
            {
                return valueDAO.getEmplCounterValuesAll();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValue.SearchValuesAll(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> SearchValuesOrderedByName(string emplIDs)
        {
            try
            {
                return valueDAO.getEmplCounterValuesOrderedByName(emplIDs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValue.SearchValuesOrderedByName(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool Update(bool doCommit)
        {
            try
            {
                return valueDAO.update(this.ValueTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValue.Update(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool Delete(int typeID, int emplID)
        {
            try
            {
                return valueDAO.delete(typeID, emplID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValue.Delete(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public EmployeeCounterValueTO Find(int typeID, int emplID)
        {
            try
            {
                return valueDAO.find(typeID, emplID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValue.Find(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool BeginTransaction()
        {
            try
            {
                return valueDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValue.BeginTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void CommitTransaction()
        {
            try
            {
                valueDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValue.CommitTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                valueDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValue.RollbackTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return valueDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValue.GetTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                valueDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValue.SetTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

    }
}
