using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

using DataAccess;
using TransferObjects;
using Util;


namespace Common
{
	/// <summary>
	/// Summary description for ExtraHour.
	/// </summary>
	public class ExtraHour
	{		
		DAOFactory daoFactory = null;
		ExtraHourDAO extraHourDAO = null;

		DebugLog log;

        ExtraHourTO hourTO = new ExtraHourTO();

		public ExtraHourTO HourTO
        {
			get { return hourTO; }
			set { hourTO = value; }
		}
        
		public ExtraHour()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			extraHourDAO = daoFactory.getExtraHourDAO(null);
		}

        public ExtraHour(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            extraHourDAO = daoFactory.getExtraHourDAO(dbConnection);
        }

		public ExtraHour(int employeeID, DateTime dateEarned, int extraTimeAmt)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			this.HourTO.EmployeeID   = employeeID;
			this.HourTO.DateEarned   = dateEarned;
			this.HourTO.ExtraTimeAmt = extraTimeAmt;			
			this.HourTO.CalculatedTimeAmt = Util.Misc.transformMinToStringTime(extraTimeAmt);

			daoFactory = DAOFactory.getDAOFactory();
			extraHourDAO = daoFactory.getExtraHourDAO(null);
		}

		public int Save(int employeeID, DateTime dateEarned, int extraTimeAmt)
		{
			int inserted;
			try
			{
				inserted = extraHourDAO.insert(employeeID, dateEarned, extraTimeAmt);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHour.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}

        public int Save(int employeeID, DateTime dateEarned, int extraTimeAmt, bool doCommit)
        {
            int inserted;
            try
            {
                inserted = extraHourDAO.insert(employeeID, dateEarned, extraTimeAmt,doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHour.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return inserted;
        }

		public bool Update(int employeeID, DateTime dateEarned, int extraTimeAmt)
		{
			bool isUpdated;

			try
			{
				isUpdated = extraHourDAO.update(employeeID, dateEarned, extraTimeAmt);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHour.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public bool Delete(int employeeID, DateTime dateEarned)
		{
			bool isDeleted = false;

			try
			{
				isDeleted = extraHourDAO.delete(employeeID, dateEarned);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHour.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}		

		public ExtraHourTO Find(int employeeID, DateTime dateEarned)
		{
			try
			{
				return extraHourDAO.find(employeeID, dateEarned);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHour.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
        
		public void Clear()
		{
			this.HourTO = new ExtraHourTO();
		}

        public List<ExtraHourTO> Search(int employeeID, DateTime earnedFrom, DateTime earnedTo)
		{
			try
			{				
				return extraHourDAO.getExtraHour(employeeID, earnedFrom, earnedTo);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHour.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		//not in use any more. This was used for calculating total available hours for employee, 
		//but now, that is calculated by adding rows from listview that contains 
		//available hours for each earned date
        public List<ExtraHourTO> SearchEmployeeSum(int employeeID)
		{
			try
			{				
				return extraHourDAO.getEmployeeSum(employeeID);				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHour.SearchEmployeeSum(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public List<ExtraHourTO> SearchEmployeeAvailableDates(int employeeID)
		{
			try
			{
				return extraHourDAO.getEmployeeAvailableDates(employeeID);				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHour.SearchEmployeeAvailableDates(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		//not in use any more.
		public int SearchExtraHoursCount()
		{
			int count = 0;

			try
			{
				count = extraHourDAO.extraHoursCount();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHour.SearchExtraHoursCount(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return count;
		}

        private void CacheData(List<ExtraHourTO> extraHourTOList)
		{
			try
			{
				extraHourDAO.serialize(extraHourTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHour.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}

		/*
		 * public void CacheData(string description, string fromDate, string toDate)
		{
			ArrayList extraHourTOList = new ArrayList();

			try
			{
				extraHourTOList = extraHourDAO.getExtraHour(description, fromDate, toDate);
				this.CacheData(extraHourTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHour.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}
		
		
		/// <summary>
		/// Cache all of the Holidays from database
		/// </summary>
		public void CacheAllData()
		{
			try
			{
				this.CacheData(extraHourDAO.getExtraHour("", "", ""));
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHour.CacheAllData(): " + ex.Message + "\n");
				throw ex;
			}
		}
		 */

		/// <summary>
		/// Change DAO, start to use XML data source
		/// </summary>
		public void SwitchToXMLDAO()
		{
			try
			{
                daoFactory = DAOFactory.getDAOFactory(Constants.XMLDataProvider);
				extraHourDAO = daoFactory.getExtraHourDAO(null);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHour.SwitchToXMLDAO(): " + ex.Message + "\n");
				throw ex;
			}
		}

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = extraHourDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHourUsed.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                extraHourDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHourUsed.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                extraHourDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHourUsed.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return extraHourDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHourUsed.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                extraHourDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHourUsed.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
	}
}
