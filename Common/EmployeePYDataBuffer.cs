using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using Util;
using TransferObjects;
using System.Data;

namespace Common
{
    public class EmployeePYDataBuffer
    {
        DAOFactory daoFactory = null;
        EmployeePYDataBufferDAO edao = null;
		
		DebugLog log;

        EmployeePYDataBufferTO emplBuffTO = new EmployeePYDataBufferTO();

        public EmployeePYDataBufferTO EmplBuffTO
        {
            get { return emplBuffTO; }
            set { emplBuffTO = value; }
        }

        public EmployeePYDataBuffer()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getEmployeePYDataBufferDAO(null);
		}

        public EmployeePYDataBuffer(Object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getEmployeePYDataBufferDAO(dbConnection);
        }

        /// <summary>
        /// Save Employee 
        /// </summary>
        /// <returns></returns>
        public int Save( bool doCommit)
        {
            int saved = 0;
            try
            {
                saved = edao.insert(EmplBuffTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYDataBuffer.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return saved;
        }

        public int SaveExpat(bool doCommit)
        {
            int saved = 0;
            try
            {
                saved = edao.insertExpat(EmplBuffTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYDataBuffer.SaveExpat(): " + ex.Message + "\n");
                throw ex;
            }

            return saved;
        }

        public List<EmployeePYDataBufferTO> getEmployeeBuffers(uint calcID)
        {
            try
            {
                return edao.getEmployeeBuffers(calcID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYDataBuffer.getEmployeesBuffers(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public uint getMaxCalcID()
        {
            try
            {
                return edao.getMaxCalcID();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYDataBuffer.getMaxCalcID(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeePYDataBuffer.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeePYDataBuffer.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeePYDataBuffer.RollbackTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeePYDataBuffer.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeePYDataBuffer.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
