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
    /// Summary description for EmployeeGroupsTimeSchedule.
    /// </summary>
    public class EmployeeGroupsTimeSchedule
    {       
		DAOFactory daoFactory = null;
        EmployeeGroupsTimeScheduleDAO employeeGroupsTimeScheduleDAO = null;

		DebugLog log;

        EmployeeGroupsTimeScheduleTO emplGroupTSTO = new EmployeeGroupsTimeScheduleTO();

        public EmployeeGroupsTimeScheduleTO EmplGroupTSTO
        {
            get { return emplGroupTSTO; }
            set { emplGroupTSTO = value; }
        }
        		
		public EmployeeGroupsTimeSchedule()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            employeeGroupsTimeScheduleDAO = daoFactory.getEmployeeGroupsTimeScheduleDAO(null);
		}
        public EmployeeGroupsTimeSchedule(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            employeeGroupsTimeScheduleDAO = daoFactory.getEmployeeGroupsTimeScheduleDAO(dbConnection);
        }
        public EmployeeGroupsTimeSchedule(int employeeGroupID, DateTime date, int timeSchemaID, int startCycleDay)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            employeeGroupsTimeScheduleDAO = daoFactory.getEmployeeGroupsTimeScheduleDAO(null);

            this.EmplGroupTSTO.EmployeeGroupID = employeeGroupID;
			this.EmplGroupTSTO.Date = date;
			this.EmplGroupTSTO.TimeSchemaID = timeSchemaID;
			this.EmplGroupTSTO.StartCycleDay = startCycleDay;
		}

        public int Save(int employeeGroupID, DateTime date, int timeSchemaID, int startCycleDay, bool doCommit)
		{
			int inserted;
			try
			{
                inserted = employeeGroupsTimeScheduleDAO.insert(employeeGroupID, date, timeSchemaID, startCycleDay, doCommit);
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " EmployeeGroupsTimeSchedule.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}

        public bool DeleteSchedule(int employeeGroupID, bool doCommit)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = employeeGroupsTimeScheduleDAO.deleteSchedule(employeeGroupID, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeGroupsTimeSchedule.DeleteMonthSchedule(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public bool DeleteSchedule(int employeeGroupID, DateTime from, DateTime to, bool doCommit)
        {
            try
            {
                return employeeGroupsTimeScheduleDAO.deleteSchedule(employeeGroupID, from, to, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeGroupsTimeSchedule.DeleteSchedule(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteMonthSchedule(int employeeGroupID, DateTime date, bool doCommit)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = employeeGroupsTimeScheduleDAO.deleteMonthSchedule(employeeGroupID, date, doCommit);
            }
            catch(Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeGroupsTimeSchedule.DeleteMonthSchedule(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        // Same as method SearchMonthSchedule(int employeeGroupID, DateTime date, IDBTransaction trans)
        public List<EmployeeGroupsTimeScheduleTO> SearchMonthSchedule(int employeeGroupID, DateTime date)
        {
            try
            {
                return employeeGroupsTimeScheduleDAO.getEmployeeMonthSchedules(employeeGroupID, date);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeGroupsTimeSchedule.SearchMonthSchedule(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        // Same as method SearchMonthSchedule(int employeeGroupID, DateTime date)
        public List<EmployeeGroupsTimeScheduleTO> SearchMonthSchedule(int employeeGroupID, DateTime date, IDbTransaction trans)
        {
            try
            {
                return employeeGroupsTimeScheduleDAO.getEmployeeMonthSchedules(employeeGroupID, date, trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeGroupsTimeSchedule.SearchMonthSchedule(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeGroupsTimeScheduleTO> SearchGroupsSchedules(string groups, DateTime fromDate, DateTime toDate, IDbTransaction trans)
        {
            try
            {
                return employeeGroupsTimeScheduleDAO.getGroupsSchedules(groups, fromDate, toDate, trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeGroupsTimeSchedule.SearchGroupsSchedules(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeGroupsTimeScheduleTO> SearchFromSchedules(int employeeGroupID, DateTime fromDate, IDbTransaction trans)
        {
            try
            {
                return employeeGroupsTimeScheduleDAO.getGroupFromSchedules(employeeGroupID, fromDate, trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeGroupsTimeSchedule.SearchFromSchedules(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeGroupsTimeScheduleTO> SearchGroupsNextSchedule(string employeeGroups, DateTime date)
        {
            try
            {
                return employeeGroupsTimeScheduleDAO.getGroupsNextSchedule(employeeGroups, date);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeGroupsTimeSchedule.SearchEmployeesNextSchedule(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        
		public void Clear()
		{
            this.EmplGroupTSTO = new EmployeeGroupsTimeScheduleTO();
		}

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = employeeGroupsTimeScheduleDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeGroupsTimeSchedule.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                employeeGroupsTimeScheduleDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeGroupsTimeSchedule.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                employeeGroupsTimeScheduleDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeGroupsTimeSchedule.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return employeeGroupsTimeScheduleDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeGroupsTimeSchedule.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                employeeGroupsTimeScheduleDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeGroupsTimeSchedule.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        //natalija07112017
        public EmployeeGroupsTimeScheduleTO Find(int timeSchemaID, int employeeGroupID, IDbTransaction trans)
        {
            EmployeeGroupsTimeScheduleTO emplGroupTSTO = new EmployeeGroupsTimeScheduleTO();

            try
            {
                emplGroupTSTO = employeeGroupsTimeScheduleDAO.find(timeSchemaID, employeeGroupID, trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeGroupsTimeSchedule.Find(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return emplGroupTSTO;
        }
      
    }
}
