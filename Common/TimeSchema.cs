using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
	/// <summary>
	/// TimeSchema contains working time intervals.
	/// </summary>
	public class TimeSchema
	{
		private DAOFactory daoFactory;
		private WorkTimeSchemaDAO wtSchemaDAO;

		// Debug log
        DebugLog log;

        WorkTimeSchemaTO timeSchemaTO = new WorkTimeSchemaTO();

        public WorkTimeSchemaTO TimeSchemaTO
		{
			get { return timeSchemaTO; }
			set { timeSchemaTO = value; }
		}
        
		public TimeSchema()
		{
			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);
			
			daoFactory = DAOFactory.getDAOFactory();
			wtSchemaDAO = daoFactory.getWorkTimeSchemaDAO(null);
		}


        public TimeSchema(object dbConnection)
        {
            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            wtSchemaDAO = daoFactory.getWorkTimeSchemaDAO(dbConnection);
        }

        public TimeSchema(bool createDAO)
        {
            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                wtSchemaDAO = daoFactory.getWorkTimeSchemaDAO(null);
            }
        }
        public Dictionary<int, WorkTimeSchemaTO> getDictionary(IDbTransaction trans)
        {
            try
            {
                return wtSchemaDAO.getDictionary(this.TimeSchemaTO, trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TimeSchema.getDictionary(): " + ex.Message + "\n");
                throw ex;
            }
        }
        public Dictionary<int, WorkTimeSchemaTO> getDictionary()
        {
            try
            {
                return wtSchemaDAO.getDictionary(this.TimeSchemaTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TimeSchema.getDictionary(): " + ex.Message + "\n");
                throw ex;
            }
        }
        // Same as method Search(string tschemaID, string name, string decsription, string timeSchemaType, string duration, IDBTransaction trans) 
		public List<WorkTimeSchemaTO> Search()
		{
			try
			{				
				return wtSchemaDAO.getWorkTimeSchemas(this.TimeSchemaTO);	
            }
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeSchema.Search(): " + ex.Message + "\n");
				throw ex;
			}
		}

        public List<WorkTimeSchemaTO> Search(bool createDAO)
        {
            try
            {
                return wtSchemaDAO.getWorkTimeSchemas(this.TimeSchemaTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TimeSchema.Search(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public List<WorkTimeSchemaTO> Search(string tschemaID)
        {
            try
            {
                return wtSchemaDAO.getWorkTimeSchemas(tschemaID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TimeSchema.Search(): " + ex.Message + "\n");
                throw ex;
            }
        }

        // Same as method Search(string tschemaID, string name, string decsription, string timeSchemaType, string duration) 
        public List<WorkTimeSchemaTO> Search(IDbTransaction trans)
        {
            try
            {
                return wtSchemaDAO.getWorkTimeSchemas(this.TimeSchemaTO, trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TimeSchema.Search(): " + ex.Message + "\n");
                throw ex;
            }
        }
        //NATALIJA08112017
        public List<WorkTimeSchemaTO> SearchTimeSchedule(WorkTimeSchemaTO timeSchemaTO, IDbTransaction trans)
        {
            try
            {
                return wtSchemaDAO.getWorkTimeSchemas(timeSchemaTO, trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TimeSchema.Search(): " + ex.Message + "\n");
                throw ex;
            }
        }


        public Dictionary<int, WorkTimeSchemaTO> SearchDictionary(IDbTransaction trans)
        {
            try
            {
                return wtSchemaDAO.getWorkTimeSchemasDictionary(this.TimeSchemaTO, trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TimeSchema.SearchDictionary(): " + ex.Message + "\n");
                throw ex;
            }
        }
        		
		/// <summary>
		/// Save current schema
		/// </summary>
		/// <returns>true if saved successfully</returns>
		public int Save()
		{
			int affectedRows;
			try
			{
				affectedRows = this.wtSchemaDAO.insert(this.TimeSchemaTO);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeSchema.Save(): " + ex.Message + "\n");
				throw ex;
			}
			
			return affectedRows;
		}

		public bool Update()
		{
			bool isUpdated = false;

			try
			{
				isUpdated = this.wtSchemaDAO.update(this.TimeSchemaTO);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeSchema.Update(): " + ex.Message + "\n");
				throw ex;
			}

			return isUpdated;
		}


		/// <summary>
		/// Delete current schema
		/// </summary>
		/// <returns>true if it is successfully deleted</returns>
		public bool Delete()
		{
			bool isDeleted = false;

			try
			{
				isDeleted = this.wtSchemaDAO.delete(this.TimeSchemaTO.TimeSchemaID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeSchema.Delete(): " + ex.Message + "\n");
				throw ex;
			}

			return isDeleted;
		}

		public bool SchemaIsUsed(int schemaID)
		{
			bool isUsed = false;

			try
			{
				isUsed = this.wtSchemaDAO.timeSchemaIsUsed(this.TimeSchemaTO.TimeSchemaID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeSchema.SchemaIsUsed(): " + ex.Message + "\n");
				throw ex;
			}

			return isUsed;
		}

        public List<WorkTimeIntervalTO> GetCriticalMoments()
		{
			try
			{
				return wtSchemaDAO.GetCriticalMoments();				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeSchema.GetCriticalMoments(): " + ex.Message + "\n");
				throw ex;
			}
		}

		public int SearchPauses()
		{
			int pauseCount = 0;
			try
			{
				pauseCount = wtSchemaDAO.getPauses();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeSchema.SearchPauses(): " + ex.Message + "\n");
				throw ex;
			}
			
			return pauseCount;
		}



       
    }
}
