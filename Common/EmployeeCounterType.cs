using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
    public class EmployeeCounterType
    {
        DAOFactory daoFactory = null;
		EmployeeCounterTypeDAO typeDAO = null;

		DebugLog log;

        EmployeeCounterTypeTO typeTO = new EmployeeCounterTypeTO();

        EmployeeCounterTypeDAO employeeCounterTypeDAO = null;


        public EmployeeCounterTypeTO TypeTO
		{
            get { return typeTO; }
			set{ typeTO = value; }
		}

        public EmployeeCounterType()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            typeDAO = daoFactory.getEmployeeCounterTypeDAO(null);
		}
        public EmployeeCounterType(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            typeDAO = daoFactory.getEmployeeCounterTypeDAO(dbConnection);
        }

        public int Save(EmployeeCounterTypeTO type)
        {            
            try
            {
                return typeDAO.insert(type);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterType.Save(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public int Save()
        {
            try
            {
                return typeDAO.insert(this.TypeTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterType.Save(): " + ex.Message + "\n");
                throw ex;
            }
        }


        public List<EmployeeCounterTypeTO> Search()
        {
            try
            {
                return typeDAO.getEmplCounterTypes(this.TypeTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterType.Search(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool Update()
        {
            try
            {
                return typeDAO.update(this.TypeTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterType.Update(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool Delete(int typeID)
        {
            try
            {
                return typeDAO.delete(typeID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterType.Delete(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool BeginTransaction()
        {
            try
            {
                return typeDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterType.BeginTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void CommitTransaction()
        {
            try
            {
                typeDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterType.CommitTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                typeDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterType.RollbackTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return typeDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterType.GetTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                typeDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterType.SetTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public EmployeeCounterTypeTO Find(int counterTypeID)
        {
            EmployeeCounterTypeTO employeeCounterTypeTO = new EmployeeCounterTypeTO();

            try
            {
                employeeCounterTypeTO = employeeCounterTypeDAO.find(counterTypeID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterType.Find(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return employeeCounterTypeTO;
        }

        public void ReceiveTransferObject(EmployeeCounterTypeTO employeeCounterTypeTO)
        {
            this.TypeTO.EmplCounterTypeID = employeeCounterTypeTO.EmplCounterTypeID;
            this.TypeTO.Name = employeeCounterTypeTO.Name;
            this.TypeTO.NameAlt = employeeCounterTypeTO.NameAlt;
            this.TypeTO.Desc = employeeCounterTypeTO.Desc;
        }

        public void Clear()
        {		
			this.TypeTO.EmplCounterTypeID = -1;
			this.TypeTO.Name          = "";
            this.TypeTO.NameAlt       = "";
			this.TypeTO.Desc          = "";

        }
    }
}
