using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using Util;
using TransferObjects;
using System.Data;

namespace Common
{
    public class EmployeeAsco4Hist
    {
          DAOFactory daoFactory = null;
        EmployeeAsco4HistDAO edao = null;

        DebugLog log;

        EmployeeAsco4TO emplAsco4TO = new EmployeeAsco4TO();

        public EmployeeAsco4TO EmplAsco4TO
        {
            get { return emplAsco4TO; }
            set { emplAsco4TO = value; }
        }
        
        public EmployeeAsco4Hist()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			edao = daoFactory.getEmployeeAsco4HistDAO(null);
		}

        public EmployeeAsco4Hist(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getEmployeeAsco4HistDAO(dbConnection);
        }

        public bool save(bool doCommit)
        {
            bool saved = false;
            try
            {
                saved = edao.insert(this.EmplAsco4TO.EmployeeID, this.EmplAsco4TO.IntegerValue1, this.EmplAsco4TO.IntegerValue2, this.EmplAsco4TO.IntegerValue3, this.EmplAsco4TO.IntegerValue4, this.EmplAsco4TO.IntegerValue5,
                    this.EmplAsco4TO.IntegerValue6, this.EmplAsco4TO.IntegerValue7, this.EmplAsco4TO.IntegerValue8, this.EmplAsco4TO.IntegerValue9, this.EmplAsco4TO.IntegerValue10, this.EmplAsco4TO.DatetimeValue1,
                     this.EmplAsco4TO.DatetimeValue2, this.EmplAsco4TO.DatetimeValue3, this.EmplAsco4TO.DatetimeValue4, this.EmplAsco4TO.DatetimeValue5, this.EmplAsco4TO.DatetimeValue6, this.EmplAsco4TO.DatetimeValue7,
                     this.EmplAsco4TO.DatetimeValue8, this.EmplAsco4TO.DatetimeValue9, this.EmplAsco4TO.DatetimeValue10, this.EmplAsco4TO.NVarcharValue1, this.EmplAsco4TO.NVarcharValue2, this.EmplAsco4TO.NVarcharValue3,
                     this.EmplAsco4TO.NVarcharValue4, this.EmplAsco4TO.NVarcharValue5, this.EmplAsco4TO.NVarcharValue6, this.EmplAsco4TO.NVarcharValue7, this.EmplAsco4TO.NVarcharValue8, this.EmplAsco4TO.NVarcharValue9,
                      this.EmplAsco4TO.NVarcharValue10, this.emplAsco4TO.CreatedBy,this.EmplAsco4TO.CreatedTime, doCommit);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAsco4Hist.Save(): " + ex.Message + this.EmplAsco4TO.EmployeeID.ToString() + "\n");
                throw ex;
            }

            return saved;
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
                log.writeLog(DateTime.Now + " EmployeeAsco4Hist.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeAsco4Hist.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeAsco4Hist.RollbackTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeAsco4Hist.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeAsco4Hist.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

    }
}
