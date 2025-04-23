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
	/// Summary description for WorkingGroup.
	/// </summary>
	public class WorkingGroup
	{
		DAOFactory daoFactory = null;
		WorkingGroupDAO wrkGroupDAO = null;

		DebugLog log;

        WorkingGroupTO wgTO = new WorkingGroupTO();

		public WorkingGroupTO WGroupTO
		{
			get{ return wgTO; }
			set{ wgTO = value; }
		}
        		
		public WorkingGroup()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			wrkGroupDAO = daoFactory.getWorkingGroupDAO(null);
		}

        public WorkingGroup(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            wrkGroupDAO = daoFactory.getWorkingGroupDAO(dbConnection);
        }

		public WorkingGroup(int employeeGroupID, string groupName, string description)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			wrkGroupDAO = daoFactory.getWorkingGroupDAO(null);

			this.WGroupTO.EmployeeGroupID = employeeGroupID;
			this.WGroupTO.GroupName = groupName;
			this.WGroupTO.Description = description;
		}

        public int Save(string groupName, string description, DateTime date, int timeSchemaID, int startCycleDay)
		{
			int inserted;
			try
			{
				inserted = wrkGroupDAO.insert(groupName, description, date, timeSchemaID, startCycleDay);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WorkingGroup.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}

		/*public int Save(WorkingGroupTO wrkGroupTO)
		{
			int inserted;
			try
			{
				inserted = wrkGroupDAO.insert(wrkGroupTO.GroupName, wrkGroupTO.Description);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WorkingGroup.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}*/

		public List<WorkingGroupTO> Search()
		{
			try
			{				
				return wrkGroupDAO.getWorkingGroups(this.WGroupTO);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WorkingGroup.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public List<WorkingGroupTO> SearchIDSort()
        {
            try
            {
                return wrkGroupDAO.getWorkingGroupsIDSort(this.WGroupTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingGroup.SearchIDSort(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

		public bool Update(int employeeGroupID, string groupName, string description)
		{
			bool isUpdated;

			try
			{
				isUpdated = wrkGroupDAO.update(employeeGroupID, groupName, description);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WorkingGroup.Update(): " + ex.Message + "\n");
                throw ex;
			}

			return isUpdated;
		}

        public bool Update(int employeeGroupID, string groupName, string description, bool doCommit)
        {
            bool isUpdated;

            try
            {
                isUpdated = wrkGroupDAO.update(employeeGroupID, groupName, description, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingGroup.Update(): " + ex.Message + "\n");
                throw ex;
            }

            return isUpdated;
        }

		public bool Delete(int employeeGroupID)
		{
			bool isDeleted = false;

			try
			{
				isDeleted = wrkGroupDAO.delete(employeeGroupID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WorkingGroup.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

        public bool Delete(int employeeGroupID, bool doCommit)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = wrkGroupDAO.delete(employeeGroupID, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingGroup.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        // Same as method Find(int employeeGroupID, IDbTransaction trans)
		public WorkingGroupTO Find(int employeeGroupID)
		{
			WorkingGroupTO wrkGroupTO = new WorkingGroupTO();

			try
			{
				wrkGroupTO = wrkGroupDAO.find(employeeGroupID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WorkingGroup.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return wrkGroupTO;
		}

        // Same as method Find(int employeeGroupID)
        public WorkingGroupTO Find(int employeeGroupID, IDbTransaction trans)
        {
            WorkingGroupTO wrkGroupTO = new WorkingGroupTO();

            try
            {
                wrkGroupTO = wrkGroupDAO.find(employeeGroupID, trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingGroup.Find(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return wrkGroupTO;
        }
        		
		public void Clear()
		{
			this.WGroupTO = new WorkingGroupTO();
		}

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = wrkGroupDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingGroup.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                wrkGroupDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingGroup.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                wrkGroupDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingGroup.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return wrkGroupDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingGroup.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                wrkGroupDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingGroup.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
	}
}
