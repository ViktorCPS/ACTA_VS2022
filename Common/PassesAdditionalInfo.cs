using System;
using System.Collections.Generic;
using System.Text;

using DataAccess;
using TransferObjects;
using Util;
using System.Data;

namespace Common
{
    public class PassesAdditionalInfo
    {
        DAOFactory daoFactory = null;
        PassesAdditionalInfoDAO passesAdditionalInfoDAO = null;

        DebugLog log;

        PassesAdditionalInfoTO passesAdditionalInfoTO = new PassesAdditionalInfoTO();

        public PassesAdditionalInfoTO PassesAdditionalInfoTO
        {
            get { return passesAdditionalInfoTO; }
            set { passesAdditionalInfoTO = value; }
        }

        public PassesAdditionalInfo()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            passesAdditionalInfoDAO = daoFactory.getPassesAdditionalInfoDAO(null);
		}

        public PassesAdditionalInfo(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            passesAdditionalInfoDAO = daoFactory.getPassesAdditionalInfoDAO(dbConnection);
        }

        public PassesAdditionalInfo(uint pass_id, string gps_data, string cardholder_name, int cardholder_id, string created_by, DateTime created_time, string modified_by, DateTime modified_time)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            passesAdditionalInfoDAO = daoFactory.getPassesAdditionalInfoDAO(null);

            this.PassesAdditionalInfoTO.PassID = pass_id;
            this.PassesAdditionalInfoTO.GpsData = gps_data;
            this.PassesAdditionalInfoTO.CardholderID = cardholder_id;
            this.PassesAdditionalInfoTO.CardholderName = cardholder_name;
            this.PassesAdditionalInfoTO.CreatedBy = created_by;
            this.PassesAdditionalInfoTO.CreatedTime = created_time;
            this.PassesAdditionalInfoTO.ModifiedBy = modified_by;
            this.PassesAdditionalInfoTO.ModifiedTime = modified_time;
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = passesAdditionalInfoDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesAdditionalInfo.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                passesAdditionalInfoDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesAdditionalInfo.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                passesAdditionalInfoDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesAdditionalInfo.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return passesAdditionalInfoDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesAdditionalInfo.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                passesAdditionalInfoDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesAdditionalInfo.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
     
        
        public List<PassesAdditionalInfoTO> Search(PassesAdditionalInfoTO  pass)
        {
            try
            {
                return passesAdditionalInfoDAO.getPasses(pass);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesAdditionalInfo.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public PassesAdditionalInfoTO Find(uint passID)
        {
            try
            {
                return passesAdditionalInfoDAO.find(passID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesAdditionalInfo.Find(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void Clear()
        {
            this.PassesAdditionalInfoTO = new PassesAdditionalInfoTO();
        }

        public int Save(bool doCommit)
        {
            int saved = 0;
            try
            {
                saved = passesAdditionalInfoDAO.insert(this.PassesAdditionalInfoTO, doCommit);
            }
            catch (Exception ex)
            {
                //if (ex.Message.Trim().Equals(ConfigurationManager.AppSettings["SQLExceptionPrimaryKeyViolation"]))
                if (ex.Message.Trim().Equals(Constants.SQLExceptionPrimaryKeyViolation.ToString()))
                {
                    log.writeLog(DateTime.Now + " PassesAdditionalInfo.Save(): Record already exist!   Primary Key Violation SqlException.Number: " + ex.Message);
                }
                else
                {
                    log.writeLog(DateTime.Now + " PassesAdditionalInfo.Save(): " + ex.Message + "\n");
                    throw new Exception(ex.Message);
                }
            }

            return saved;
        }

        public bool Update(bool doCommit)
        {
            bool isUpdated = false;

            try
            {
                isUpdated = passesAdditionalInfoDAO.update(this.PassesAdditionalInfoTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesAdditionalInfo.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return isUpdated;
        }

        public bool Delete(uint passId, bool doCommit)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = passesAdditionalInfoDAO.delete(passId, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesAdditionalInfo.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }


    }
}
