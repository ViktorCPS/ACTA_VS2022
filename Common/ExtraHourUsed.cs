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
	/// Summary description for ExtraHourUsed.
	/// </summary>
	public class ExtraHourUsed
	{		
		DAOFactory daoFactory = null;
		ExtraHourUsedDAO extraHourUsedDAO = null;

		DebugLog log;

        ExtraHourUsedTO hourUsedTO = new ExtraHourUsedTO();

		public ExtraHourUsedTO HourUsedTO
		{
			get { return hourUsedTO; }
			set { hourUsedTO = value; }
		}
        		
		public ExtraHourUsed()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			extraHourUsedDAO = daoFactory.getExtraHourUsedDAO(null);
		}

        public ExtraHourUsed(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            extraHourUsedDAO = daoFactory.getExtraHourUsedDAO(dbConnection);
        }

		public ExtraHourUsed(int recordID, int employeeID, DateTime dateEarned, DateTime dateUsed,
            int extraTimeAmtUsed, DateTime startTime, DateTime endTime, Int32 ioPairID, string type, string createdByName)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			this.HourUsedTO.RecordID         = recordID;
			this.HourUsedTO.EmployeeID       = employeeID;
			this.HourUsedTO.DateEarned       = dateEarned;
			this.HourUsedTO.DateUsed         = dateUsed;
			this.HourUsedTO.ExtraTimeAmtUsed = extraTimeAmtUsed;	
			this.HourUsedTO.StartTime        = startTime;
			this.HourUsedTO.EndTime          = endTime;
			this.HourUsedTO.IOPairID         = ioPairID;
			this.HourUsedTO.CalculatedTimeAmtUsed = Util.Misc.transformMinToStringTime(extraTimeAmtUsed);
            this.HourUsedTO.Type             = type;
            this.HourUsedTO.CreatedByName    = createdByName;

			daoFactory = DAOFactory.getDAOFactory();
			extraHourUsedDAO = daoFactory.getExtraHourUsedDAO(null);
		}

		public int Save(int employeeID, DateTime dateEarned, DateTime dateUsed, int extraTimeAmtUsed, DateTime startTime, DateTime endTime, Int32 ioPairID, string type, bool doCommit)
		{
			int inserted;
			try
			{
                if (Misc.isLockedDate(dateUsed.Date))
                {
                    string exceptionString = Misc.dataLockedMessage(dateUsed.Date);
                    throw new Exception(exceptionString);
                }
				inserted = extraHourUsedDAO.insert(employeeID, dateEarned, dateUsed, extraTimeAmtUsed, startTime, endTime, ioPairID, type, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHourUsed.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}

		//not in use yet
		public bool Update(int recordID, int employeeID, DateTime dateEarned, DateTime dateUsed, 
			int extraTimeAmtUsed, bool doCommit)
		{
			bool isUpdated;

			try
			{
                if (Misc.isLockedDate(dateUsed.Date))
                {
                    string exceptionString = Misc.dataLockedMessage(dateUsed.Date);
                    throw new Exception(exceptionString);
                }
				isUpdated = extraHourUsedDAO.update(recordID, employeeID, dateEarned, dateUsed, 
					extraTimeAmtUsed, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHourUsed.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

        public bool Delete(int recordID, bool isRegularWork, DateTime dateUsed)
		{
			bool isDeleted = false;

			try
			{                
                if (Misc.isLockedDate(dateUsed.Date))
                {
                    string exceptionString = Misc.dataLockedMessage(dateUsed.Date);
                    throw new Exception(exceptionString);
                }
                isDeleted = extraHourUsedDAO.delete(recordID, isRegularWork);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHourUsed.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}	
	
		public bool BeginTransaction()
		{
			bool isStarted = false;

			try
			{
				isStarted = extraHourUsedDAO.beginTransaction();
			}
			catch(Exception ex)
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
				extraHourUsedDAO.commitTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHourUsed.CommitTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void RollbackTransaction()
		{
			try
			{
				extraHourUsedDAO.rollbackTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHourUsed.RollbackTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public IDbTransaction GetTransaction()
		{
			try
			{
				return extraHourUsedDAO.getTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHourUsed.GetTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void SetTransaction(IDbTransaction trans)
		{
			try
			{
				extraHourUsedDAO.setTransaction(trans);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHourUsed.SetTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		//not in use yet
		/*public ExtraHourUsedTO Find(int employeeID, DateTime dateEarned)
		{
			ExtraHourUsedTO extraHourUsedTO = new ExtraHourUsedTO();

			try
			{
				extraHourUsedTO = extraHourUsedDAO.find(employeeID, dateEarned);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHourUsed.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return extraHourUsedTO;
		}*/
        
		public void Clear()
		{
            this.HourUsedTO = new ExtraHourUsedTO();
		}

        public List<ExtraHourUsedTO> Search(int employeeID, DateTime usedFrom, DateTime usedTo, string type)
		{
			try
			{
                return extraHourUsedDAO.getExtraHourUsed(employeeID, usedFrom, usedTo, type);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHourUsed.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public List<ExtraHourUsedTO> SearchByType(int emplID, string type)
        {
            try
            {
                return extraHourUsedDAO.getExtraHourUsedByType(emplID,type);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHourUsed.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

		//not in use any more. This was used for calculating total available hours for employee, 
		//but now, that is calculated by adding rows from listview that contains 
		//available hours for each earned date
        public List<ExtraHourUsedTO> SearchEmployeeUsedSum(int employeeID)
		{
			try
			{				
				return extraHourUsedDAO.getEmployeeUsedSum(employeeID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHourUsed.SearchEmployeeUsedSum(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public int SearchEmployeeUsedSumByType(int employeeID, DateTime fromDate, DateTime toDate, string type)
        {
            int sum = 0;

            try
            {
                sum = extraHourUsedDAO.getEmployeeUsedSumByType(employeeID, fromDate, toDate, type);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHourUsed.SearchEmployeeUsedSumByType(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return sum;
        }

		//not in use any more. This was used for calculating available hours left for each
		//earned date, but now, there is getEmployeeAvailableDates() in ExtraHourDAO, that
		//calculate everything on database side
        public List<ExtraHourUsedTO> SearchEmployeeUsedSumDateEarned(int employeeID)
		{
			try
			{				
				return extraHourUsedDAO.getEmployeeUsedSumDateEarned(employeeID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHourUsed.SearchEmployeeUsedSumDateEarned(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public bool ExistOverlap(int employeeID, DateTime dateUsed, DateTime startTime, DateTime endTime)
		{
			bool existOverlap = false;

			try
			{
				existOverlap = extraHourUsedDAO.existOverlap(employeeID, dateUsed, startTime, endTime);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHourUsed.ExistOverlap(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return existOverlap;
		}

        private void CacheData(List<ExtraHourUsedTO> extraHourUsedTOList)
		{
			try
			{
				extraHourUsedDAO.serialize(extraHourUsedTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHourUsed.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}

		/*
		 * public void CacheData(string description, string fromDate, string toDate)
		{
			ArrayList extraHourUsedTOList = new ArrayList();

			try
			{
				extraHourUsedTOList = extraHourUsedDAO.getExtraHourUsed(description, fromDate, toDate);
				this.CacheData(extraHourUsedTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHourUsed.CacheData(): " + ex.Message + "\n");
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
				this.CacheData(extraHourUsedDAO.getExtraHourUsed("", "", ""));
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHourUsed.CacheAllData(): " + ex.Message + "\n");
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
				extraHourUsedDAO = daoFactory.getExtraHourUsedDAO(null);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHourUsed.SwitchToXMLDAO(): " + ex.Message + "\n");
				throw ex;
			}
		}
	}
}
