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
	/// Summary description for EmployeesTimeSchedule.
	/// </summary>
	public class EmployeesTimeSchedule
	{
		DAOFactory daoFactory = null;
		EmployeeTimeScheduleDAO emplTimeScheduleDAO = null;

		DebugLog log;

        EmployeeTimeScheduleTO emplTSTO = new EmployeeTimeScheduleTO();
		
		public EmployeeTimeScheduleTO EmplTSTO
		{
			get{ return emplTSTO; }
			set{ emplTSTO = value; }
		}
        
		public EmployeesTimeSchedule()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			emplTimeScheduleDAO = daoFactory.getEmployeeTimeScheduleDAO(null);
		}

        public EmployeesTimeSchedule(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                emplTimeScheduleDAO = daoFactory.getEmployeeTimeScheduleDAO(null);
            }
        }

		public EmployeesTimeSchedule(int employeeID, DateTime date, int timeSchemaID, int startCycleDay)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			emplTimeScheduleDAO = daoFactory.getEmployeeTimeScheduleDAO(null);

			this.EmplTSTO.EmployeeID = employeeID;
			this.EmplTSTO.Date = date;
			this.EmplTSTO.TimeSchemaID = timeSchemaID;
			this.EmplTSTO.StartCycleDay = startCycleDay;
		}

        public EmployeesTimeSchedule(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            emplTimeScheduleDAO = daoFactory.getEmployeeTimeScheduleDAO(dbConnection);
        }


		public int Save(int employeeID, DateTime date, int timeSchemaID, int startCycleDay)
		{
			int inserted;
			try
			{
				inserted = emplTimeScheduleDAO.insert(employeeID, date, timeSchemaID, startCycleDay);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesTimeSchedule.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}

        public int Save(int employeeID, DateTime date, int timeSchemaID, int startCycleDay, string user, bool doCommit)
        {
            int inserted;
            try
            {
                inserted = emplTimeScheduleDAO.insert(employeeID, date, timeSchemaID, startCycleDay, user, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesTimeSchedule.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return inserted;
        }

		public int Save(EmployeeTimeScheduleTO emplTimeScheduleTO)
		{
			int inserted;
			try
			{
				inserted = emplTimeScheduleDAO.insert(emplTimeScheduleTO.EmployeeID, emplTimeScheduleTO.Date,
					emplTimeScheduleTO.TimeSchemaID, emplTimeScheduleTO.StartCycleDay);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesTimeSchedule.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}

		public List<EmployeeTimeScheduleTO> Search()
		{
			try
			{
				return emplTimeScheduleDAO.getEmployeeTimeSchedules(this.EmplTSTO);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesTimeSchedule.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public List<EmployeeTimeScheduleTO> SearchEmployeeMonthTimeSchedules(DateTime month)
		{
			try
			{
				return emplTimeScheduleDAO.getEmployeeMonthTimeSchedules(month);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesTimeSchedule.SearchEmployeeMonthTimeSchedules(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public List<EmployeeTimeScheduleTO> SearchMonthSchedule(int employeeID, DateTime date)
		{
			try
			{
				return emplTimeScheduleDAO.getEmployeeMonthSchedules(employeeID, date);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesTimeSchedule.SearchMonthSchedule(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
        public List<EmployeeTimeScheduleTO> SearchMonthScheduleFromDataSet(int employeeID, DateTime date, DataSet dsTimeSchedules)
        {
            try
            {
                return emplTimeScheduleDAO.getEmployeeMonthSchedulesFromDataSet(employeeID, date, dsTimeSchedules);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesTimeSchedule.SearchMonthScheduleFromDataSet(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int, List<EmployeeTimeScheduleTO>> SearchEmployeesSchedulesDS(string employees, DateTime fromDate, DateTime toDate, IDbTransaction trans)
        {
            try
            {
                return emplTimeScheduleDAO.getEmployeesSchedulesDS(employees, fromDate, toDate,trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesTimeSchedule.SearchEmployeesSchedulesDS(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int, List<EmployeeTimeScheduleTO>> SearchEmployeesSchedulesDS(string employees, DateTime fromDate, DateTime toDate)
		{
			try
			{
                return emplTimeScheduleDAO.getEmployeesSchedulesDS(employees, fromDate, toDate);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesTimeSchedule.SearchMonthScheduleFromDataSet(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public Dictionary<int, List<EmployeeTimeScheduleTO>> SearchEmployeesSchedulesExactDate(string employees, DateTime fromDate, DateTime toDate, IDbTransaction trans)
        {
            try
            {
                return emplTimeScheduleDAO.getEmployeesSchedulesExactDate(employees, fromDate, toDate, trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesTimeSchedule.SearchEmployeesSchedulesExactDate(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        // Same as method SearchEmployeesSchedules(string employees, DateTime fromDate, DateTime toDate, IDBTransacction)
        public List<EmployeeTimeScheduleTO> SearchEmployeesSchedules(string employees, DateTime fromDate, DateTime toDate)
		{
			try
			{
				return emplTimeScheduleDAO.getEmployeesSchedules(employees, fromDate, toDate);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesTimeSchedule.SearchMonthSchedule(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public List<EmployeeTimeScheduleTO> SearchEmployeesSchedules(string employees, DateTime fromDate, DateTime toDate, bool createDAO)
        {
            try
            {
                return emplTimeScheduleDAO.getEmployeesSchedules(employees, fromDate, toDate);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesTimeSchedule.SearchMonthSchedule(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        // Same as method SearchEmployeesSchedules(string employees, DateTime fromDate, DateTime toDate)
        public List<EmployeeTimeScheduleTO> SearchEmployeesSchedules(string employees, DateTime fromDate, DateTime toDate, IDbTransaction trans)
        {
            try
            {
                return emplTimeScheduleDAO.getEmployeesSchedules(employees, fromDate, toDate, trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesTimeSchedule.SearchMonthSchedule(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

		public DataSet GetTimeSchedules()
		{			
			try
			{
				return emplTimeScheduleDAO.getTimeSchedules();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesTimeSchedule.GetTimeSchedules(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public bool Update(int employeeID, DateTime date, int timeSchemaID, int startCycleDay)
		{
			bool isUpdated;

			try
			{
				isUpdated = emplTimeScheduleDAO.update(employeeID, date, timeSchemaID, startCycleDay);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesTimeSchedule.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public EmployeeTimeScheduleTO Find(int employeeID, DateTime date)
		{
			EmployeeTimeScheduleTO emplTimeScheduleTO = new EmployeeTimeScheduleTO();

			try
			{
				emplTimeScheduleTO = emplTimeScheduleDAO.find(employeeID, date);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesTimeSchedule.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return emplTimeScheduleTO;
		}
        //natalija07112017 dodat statucb
        public bool DeleteFromToSchedule(int employeeID, DateTime fromDate, DateTime toDate, string modifiedBy, bool doCommit, bool statuscb)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = emplTimeScheduleDAO.deleteFromToSchedule(employeeID, fromDate, toDate, modifiedBy, doCommit, statuscb);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesTimeSchedule.DeleteFromToSchedule(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public bool DeleteFromToSchedule(int employeeID, DateTime fromDate, DateTime toDate, string modifiedBy, bool doCommit)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = emplTimeScheduleDAO.deleteFromToSchedule(employeeID, fromDate, toDate, modifiedBy, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesTimeSchedule.DeleteFromToSchedule(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public List<EmployeeTimeScheduleTO> SearchEmployeesNextSchedule(string employees, DateTime date, IDbTransaction trans)
        {
            try
            {
                return emplTimeScheduleDAO.getEmployeesNextSchedule(employees, date, trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesTimeSchedule.SearchEmployeesNextSchedule(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        
		public void Clear()
		{
			this.EmplTSTO = new EmployeeTimeScheduleTO();
		}

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = emplTimeScheduleDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesTimeSchedule.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                emplTimeScheduleDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesTimeSchedule.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                emplTimeScheduleDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesTimeSchedule.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return emplTimeScheduleDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesTimeSchedule.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                emplTimeScheduleDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesTimeSchedule.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public List<EmployeeTimeScheduleTO> findEmplSch(int emplID)
        {
            try
            {
                return emplTimeScheduleDAO.findEmplSch(emplID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesTimeSchedule.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public bool Update(int employeeID, DateTime date, int timeSchemaID, int startCycleDay, bool naDan)
        {
            bool isUpdated;

            try
            {
                isUpdated = emplTimeScheduleDAO.update(employeeID, date, timeSchemaID, startCycleDay, naDan);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesTimeSchedule.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }
       
    }
}
