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
	/// <summary>
	/// Summary description for EmployeeLocation.
	/// </summary>
	public class EmployeeLocation
	{
		DAOFactory daoFactory = null;
		EmployeeLocationDAO empllocationDAO = null;

		DebugLog log;

        EmployeeLocationTO emplLocTO = new EmployeeLocationTO();

        // recovery parameters for updating employee location
        private static readonly int maxNumberOfRetries = 1;
        private int numberOfRetries = 0;

        // thread-safe locker for updating employee location
        private static object locker = new object();

        public EmployeeLocationTO EmplLocTO
		{
			get{ return emplLocTO; }
			set{ emplLocTO = value; }
		}
        
		public EmployeeLocation(int employeeID, int readerID, int antenna, int passTypeID, DateTime eventTime, int locationID)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			this.EmplLocTO.EmployeeID = employeeID;
            this.EmplLocTO.ReaderID = readerID;
            this.EmplLocTO.Antenna = antenna;
            this.EmplLocTO.PassTypeID = passTypeID;
            this.EmplLocTO.EventTime = eventTime;
            this.EmplLocTO.LocationID = locationID;
            this.EmplLocTO.WUID = -1;

			daoFactory = DAOFactory.getDAOFactory();
			empllocationDAO = daoFactory.getEmployeeLocationDAO(null);
		}

		public EmployeeLocation()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			empllocationDAO = daoFactory.getEmployeeLocationDAO(null);
		}

        public EmployeeLocation(Object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
            
            daoFactory = DAOFactory.getDAOFactory();
            empllocationDAO = daoFactory.getEmployeeLocationDAO(dbConnection);
            empllocationDAO.SetDBConnection(dbConnection);
        }

        public int Save(int employeeID, int readerID, int antenna, int passTypeID, DateTime eventTime, int locationID, bool doCommit)
		{
			int saved = 0;
			try
			{
				saved = empllocationDAO.insert(employeeID, readerID, antenna, passTypeID, eventTime, locationID, doCommit);

			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeLocation.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return saved;
		}

		public bool Update(int employeeID, int readerID, int antenna, int passTypeID, DateTime eventTime, int locationID)
		{
			bool isUpdated = false;

            lock (locker)
            {
                try
                {
                    isUpdated = empllocationDAO.update(employeeID, readerID, antenna, passTypeID, eventTime, locationID);
                    numberOfRetries = 0;
                }
                catch (FailToStartDBTransactionException ftex)
                {
                    log.writeLog(DateTime.Now + " EmployeeLocation.Update(): " + ftex.Message + "\n");
                    if (numberOfRetries <= maxNumberOfRetries)
                    {
                        daoFactory.CloseConnection();
                        log.writeLog(DateTime.Now + " EmployeeLocation.Update(): " + "close DB connection." + "\n");
                        numberOfRetries++;
                    }
                    throw new Exception(ftex.Message);
                }
                catch (Exception ex)
                {
                    log.writeLog(DateTime.Now + " EmployeeLocation.Update(): " + ex.Message + "\n");
                    throw new Exception(ex.Message);
                }
            }

			return isUpdated;
		}

		public bool Delete(int employeeId)
		{
			bool isDeleted = false;

			try
			{
				isDeleted = empllocationDAO.delete(employeeId);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeLocation.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public EmployeeLocationTO Find(int employeeID)
		{
			EmployeeLocationTO emplLocationTO = new EmployeeLocationTO();

			try
			{
				emplLocationTO = empllocationDAO.find(employeeID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeLocation.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return emplLocationTO;
		}

		public List<EmployeeLocationTO> Search(string locationID, string wUnits)
		{
			try
			{
				return empllocationDAO.getEmployeeLocations(this.EmplLocTO, locationID, wUnits);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeLocation.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
        public List<EmployeeLocationTO> Search(string locationID, string wUnits, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return empllocationDAO.getEmployeeLocations(this.EmplLocTO, locationID, wUnits,fromDate,toDate);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocation.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public List<EmployeeLocationTO> SearchAll()
		{
			try
			{
				return empllocationDAO.getEmployeeLocationsAll(this.EmplLocTO);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeLocation.SearchAll(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public List<EmployeeLocationTO> SearchMittal(string workingUnitsOther)
		{
			try
			{
				return empllocationDAO.getEmployeeLocationsMittal(workingUnitsOther);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeLocation.SearchMittal(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public List<EmployeeLocationTO> SearchOther(string workingUnitsOther)
		{
			try
			{
				return empllocationDAO.getEmployeeLocationsOther(workingUnitsOther);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeLocation.SearchOther(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public int SearchTotalMittalOut (string workingUnitsOther)
		{
			int totalMittal = 0;

			try
			{
				totalMittal = empllocationDAO.getTotalMittalOut(workingUnitsOther);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeLocation.SearchTotalMittalOut(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
			return totalMittal;
		}

		public int SearchTotalOtherOut (string workingUnitsOther)
		{
			int totalMittal = 0;

			try
			{
				totalMittal = empllocationDAO.getTotalOtherOut(workingUnitsOther);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeLocation.SearchTotalOtherOut(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
			return totalMittal;
		}

        public List<EmployeeLocationTO> SearchMittalDet(string locationID, string workingUnitsOther)
		{
			try
			{
				return empllocationDAO.getEmployeeLocationsMittalDet(locationID, workingUnitsOther);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeLocation.SearchMittalDet(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public List<EmployeeLocationTO> SearchOtherDet(string locationID, string workingUnitsOther)
		{
			try
			{				
				return empllocationDAO.getEmployeeLocationsOtherDet(locationID, workingUnitsOther);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeLocation.SearchOther(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public List<EmployeeLocationTO> SearchEmployeeLocationsIn(string locationID)
        {           
            try
            {
                return empllocationDAO.getEmployeeLocationsIn(locationID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocation.SearchEmployeeLocationsIn(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeLocationTO> SearchEmployeeLocationsInByWU(string wuID)
        {
            try
            {
                return empllocationDAO.getEmployeeLocationsInByWU(wuID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocation.SearchEmployeeLocationsInByWU(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        
        public void Clear()
		{
			this.EmplLocTO = new EmployeeLocationTO();
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = empllocationDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocation.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                empllocationDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocation.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                empllocationDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocation.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return empllocationDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocation.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                empllocationDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocation.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
	}
}
