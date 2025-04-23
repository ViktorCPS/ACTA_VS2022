using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Configuration;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
    public class PassHist
    {
        DAOFactory daoFactory = null;
        PassHistDAO phdao = null;

        DebugLog debug;

        PassHistTO phTO = new PassHistTO();

        public PassHistTO PHistTO
        {
            get { return phTO; }
            set { phTO = value; }
        }
        
        public PassHist()
		{
			daoFactory = DAOFactory.getDAOFactory();
			phdao = daoFactory.getPassHistDAO(null);

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);
		}

        public PassHist(object dbConnection)
        {
            daoFactory = DAOFactory.getDAOFactory();
            phdao = daoFactory.getPassHistDAO(dbConnection);

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            debug = new DebugLog(logFilePath);
        }

        public PassHist(int passID, int employeeID, string direction, DateTime eventTime, int passTypeID, int pairGenUsed, 
				int locationID, int manualCreated, string createdBy, DateTime createdTime, int isWrkHrsCount, string remarks)
		{
			daoFactory = DAOFactory.getDAOFactory();
			phdao = daoFactory.getPassHistDAO(null);

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);

			this.PHistTO.PassID = passID;
            this.PHistTO.EmployeeID = employeeID;
            this.PHistTO.Direction = direction;
            this.PHistTO.EventTime = eventTime;
            this.PHistTO.PassTypeID = passTypeID;
            this.PHistTO.PairGenUsed = pairGenUsed;
            this.PHistTO.LocationID = locationID;
            this.PHistTO.ManualyCreated = manualCreated;
            this.PHistTO.CreatedBy = createdBy;
            this.PHistTO.CreatedTime = createdTime;
            this.PHistTO.IsWrkHrsCount = isWrkHrsCount;
            this.PHistTO.Remarks = remarks;
            this.PHistTO.EmployeeName = "";
            this.PHistTO.PassType = "";
            this.PHistTO.LocationName = "";
            this.PHistTO.WUName = "";
		}

        public int Save(int passID, int employeeID, string direction, DateTime eventTime, int passTypeID, int isWrkHrsCount, int locationID,
            int pairGenUsed, int manualCreated, string remarks, string createdBy, DateTime createdTime, bool doCommit)
        {
            int saved = 0;

            try
            { 
                // TODO: create TO and send to DAO

                saved = phdao.insert(passID, employeeID, direction, eventTime, passTypeID, isWrkHrsCount, locationID, pairGenUsed, manualCreated, remarks, createdBy, createdTime, doCommit);
            }
            catch (Exception ex)
            {
                //if (ex.Message.Trim().Equals(ConfigurationManager.AppSettings["SQLExceptionPrimaryKeyViolation"]))
                if (ex.Message.Trim().Equals(Constants.SQLExceptionPrimaryKeyViolation.ToString()))
                {
                    debug.writeLog(DateTime.Now + " PassHist.Save(): Record already exist! Primary Key Violation SqlException.Number" + ex.Message + "\n");
                    throw new Exception(ex.Message);
                }
                else
                {
                    debug.writeLog(DateTime.Now + " PassHist.Save(): " + ex.Message + "\n");
                    throw new Exception(ex.Message);
                }
            }

            return saved;
        }

        public List<PassHistTO> SearchInterval(DateTime fromTime, DateTime toTime, bool eventTimeChecked, string wUnits, DateTime fromModifiedTime, DateTime toModifiedTime, bool modifiedTimeChecked)
        {
            try
            {
                return phdao.getPassHistInterval(this.PHistTO, fromTime, toTime, eventTimeChecked, wUnits, fromModifiedTime, toModifiedTime, modifiedTimeChecked);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassHist.SearchInterval(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int SearchIntervalCount(DateTime fromTime, DateTime toTime, bool eventTimeChecked, string wUnits, DateTime fromModifeidTime, DateTime toModifiedTime, bool modifiedTimeChecked)
        {
            try
            {
                return phdao.getPassesIntervalCount(this.PHistTO, fromTime, toTime, eventTimeChecked, wUnits, fromModifeidTime, toModifiedTime, modifiedTimeChecked);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassHist.SearchIntervalCount(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<PassHistTO> Find(int passID)
        {
            try
            {                
                return phdao.find(passID);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassHist.Find(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = phdao.beginTransaction();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassHist.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                phdao.commitTransaction();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassHist.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                phdao.rollbackTransaction();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassHist.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return phdao.getTransaction();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassHist.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                phdao.setTransaction(trans);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassHist.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }        
    }
}
