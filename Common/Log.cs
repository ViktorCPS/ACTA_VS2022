using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

using DataAccess;
using TransferObjects;
using Util;
using System.Data;

namespace Common
{
	/// <summary>
	/// Implements basic operations within Log data
	/// </summary>
	public class Log
	{
		DAOFactory daoFactory = null;
		LogDAO logDAO = null;

		// Internal Application Debug
		DebugLog log;

        LogTO lgTO = new LogTO();

        public LogTO LgTO
        {
            get { return lgTO; }
            set { lgTO = value; }
        }

		public Log()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			logDAO = daoFactory.getLogDAO(null);
		}
        public Log(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            logDAO = daoFactory.getLogDAO(dbConnection);
        }

        public Log(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                logDAO = daoFactory.getLogDAO(null);
            }
        }
		
		public Log(uint log_id, int reader_id, uint tag_id,
            int antenna, int event_happened, int action_commited, DateTime event_time, int pass_gen_used, string location, string direction, string gate, string readerDescription, string employeeName)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			this.LgTO.LogID = log_id;
            this.LgTO.ReaderID = reader_id;
            this.LgTO.TagID = tag_id;
            this.LgTO.Antenna = antenna;
            this.LgTO.EventHappened = event_happened;
            this.LgTO.ActionCommited = action_commited;
            this.LgTO.EventTime = event_time;
            this.LgTO.PassGenUsed = pass_gen_used;
            this.LgTO.Location = location;
            this.LgTO.Gate = gate;
            this.LgTO.ReaderDescription = readerDescription;
            this.LgTO.Direction = direction;
            this.LgTO.EmployeeName = employeeName;
		}
        		
		public int Save(LogTO logTo)
		{
			int inserted;
			try
			{
				inserted = logDAO.insert(logTo);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Log.Save(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return inserted;
		}

        public int SaveMobile(LogTmpAdditionalInfoTO logTo, bool doCommit)
        {
            try
            {
                return logDAO.insertMobile(logTo, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Log.SaveMobile(): " + ex.Message + "\n");
                throw ex;
            }
        }

		public List<LogTO> Search()
		{
			try
			{
				return logDAO.getLogs(this.LgTO);				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Log.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public Dictionary<long, LogTO> Search(int event1, int event2, int passGenUsed)
		{
			try
			{
				return logDAO.getLogs(event1, event2, passGenUsed);				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Log.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public List<LogTO> Search(string logID, string readerID, string tagID, string antenna, string eventHappened,
							  string actionCommited, string  eventTime, string passGenUsed)
		{
			try
			{
				return logDAO.getLogs(logID, readerID, tagID, antenna, eventHappened,
							   actionCommited, eventTime, passGenUsed);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Log.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public bool Update(LogTO logTo, bool doCommit)
		{
			bool isUpdated;

			try
			{
				isUpdated = logDAO.update(logTo, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Log.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public LogTO Find(string logId)
		{
			LogTO logTo = new LogTO();

			try
			{
				logTo = logDAO.find(Int32.Parse(logId));
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Log.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return logTo;
		}
        

		public void Clear()
		{
			this.LgTO = new LogTO();
		}

        public bool SaveToFile(List<LogTO> logTOList, string filePath)
		{
			bool succeeded = false;

			try
			{
				succeeded = logDAO.serialize(logTOList, filePath);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Log.CacheData(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
			return succeeded;
		}

        public bool SaveToFileMobile(List<LogTmpAdditionalInfoTO> logTOList, string filePath)
        {
            bool succeeded = false;

            try
            {
                succeeded = logDAO.serializeMobile(logTOList, filePath);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Log.SaveToFileMobile(): " + ex.Message + "\n");
                throw ex;
            }
            return succeeded;
        }

        public bool SaveLogToFile(LogTO log, string filePath)
        {
            bool isSerialized = false;

            try
            {
                Stream stream = File.Open(filePath, FileMode.Create);
                List<LogTO> LogTOList = new List<LogTO>();
                LogTOList.Add(log);
                LogTO[] logTOArray = (LogTO[])LogTOList.ToArray();
                XmlSerializer bformatter = new XmlSerializer(typeof(LogTO[]));
                bformatter.Serialize(stream, logTOArray);
                stream.Close();
                
                isSerialized = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isSerialized;
        }

        public List<LogTO> GetFromXMLSource(string filePath)
		{
			try
			{
				return logDAO.deserialize(filePath);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Log.GetFromFile(): " + ex.Message + "\n");
				throw ex;
			}
		}

        public List<LogTmpAdditionalInfoTO> GetFromXMLSourceMobile(string filePath)
        {
            try
            {
                return logDAO.deserializeMobile(filePath);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Log.GetFromFileMobile(): " + ex.Message + "\n");
                throw ex;
            }
        }

		public int SaveToTmp(LogTO logTo)
		{
			int inserted;
			try
			{
				inserted = logDAO.insertToTmp(logTo);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Log.SaveToTmp(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}

        public int SaveToMobileTmp(LogTmpAdditionalInfoTO logTo)
        {
            int inserted;
            try
            {
                inserted = logDAO.insertToMobileTmp(logTo);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Log.SaveToMobileTmp(): " + ex.Message + "\n");
                throw ex;
            }

            return inserted;
        }

		public bool ClearLogTmp()
		{
			bool isDeleted = false;

			try
			{
				isDeleted = logDAO.clearLogTmp();
				isDeleted = true;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Log.ClearLogTmp(): " + ex.Message + "\n");
				throw ex;
			}

			return isDeleted;
		}

        public bool ClearLogMobileTmp()
        {
            bool isDeleted = false;

            try
            {
                isDeleted = logDAO.clearLogMobileTmp();
                isDeleted = true;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Log.ClearLogMobileTmp(): " + ex.Message + "\n");
                throw ex;
            }

            return isDeleted;
        }

		public int importLog()
		{
			int rowsAffected = 0;

			try
			{
				rowsAffected = logDAO.importLog();	
			}
			catch(Exception ex)
			{	
				throw ex;
			}
			return rowsAffected;
		}

        public bool importLogMobile()
        {
            try
            {
                List<LogTmpAdditionalInfoTO> logTmpList = GetLogMobileTmp();
                int rowsInserted = 0;

                if (logTmpList.Count > 0)
                {
                    Log currLog = new Log();
                    if (currLog.BeginTransaction())
                    {
                        try
                        {
                            foreach (LogTmpAdditionalInfoTO tmpTO in logTmpList)
                            {
                                rowsInserted += currLog.SaveMobile(tmpTO, false);
                            }

                            if (rowsInserted == logTmpList.Count)
                                currLog.CommitTransaction();
                            else
                                currLog.RollbackTransaction();
                        }
                        catch (Exception ex)
                        {
                            if (currLog.GetTransaction() != null)
                                currLog.RollbackTransaction();

                            throw ex;
                        }
                    }
                }

                return rowsInserted == logTmpList.Count;
            }
            catch (Exception ex)
            {
                throw ex;                
            }
        }

        public List<LogTmpAdditionalInfoTO> GetLogMobileTmp()
        {
            try
            {
                return logDAO.getLogMobileTmp();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		/// <summary>
		/// Override Object.ToString(). 
		/// Return current log object data.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("\n ---- Log Object Data: ----");
			sb.Append("\n LogID: " + this.LgTO.LogID.ToString());
			sb.Append("\n ReaderID: "  + this.LgTO.ReaderID.ToString() );
			sb.Append("\n TagID: " + this.LgTO.TagID.ToString());
			sb.Append("\n Antenna: " + this.LgTO.Antenna.ToString());
			sb.Append("\n EventHappened: " + this.LgTO.EventHappened.ToString());
			sb.Append("\n EventTime: " + this.LgTO.EventTime.ToString());
			sb.Append("\n ActionCommited: " + this.LgTO.ActionCommited.ToString());
			sb.Append("\n PassGenUsed: " + this.LgTO.PassGenUsed.ToString());
			sb.Append("\n --------------------------");
			
			return sb.ToString();
		}

        public List<LogTO> getTrespassLogs(int locationID, int gateID, int readerID, string direction, int employeeID, int eventHappened, DateTime dateFrom, DateTime dateTo)
        {          
            try
            {         
                return logDAO.getTrespassLogs(locationID,gateID,readerID,direction,employeeID,eventHappened,dateFrom,dateTo);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Log.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<LogTO> searchLogsForGraph(DateTime dateFrom, DateTime dateTo, DateTime timeFrom, DateTime timeTo, string readerId)
        {            
            try
            {                
                return logDAO.getLogsForGraph(dateFrom, dateTo,timeFrom,timeTo,readerId);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Log.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        
        public bool deleteLogs(DateTime dateFrom, DateTime dateTo, DateTime timeFrom, DateTime timeTo, string readerId)
        {
            bool deleted = false;

            try
            {
                 deleted = logDAO.deleteLogs(dateFrom, dateTo, timeFrom, timeTo, readerId);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Log.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return deleted;
        }

        public List<LogTO> SearchForLogPeriod(DateTime from, DateTime to)
        {          
            try
            {
                return logDAO.getLogsForPeriod(this.LgTO, from,to);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Log.SearchForLogPeriod(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<LogTO> getLogIn(int employeeID, DateTime dateTime)
        {
            try
            {
                return logDAO.getLogIn(employeeID,dateTime);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Log.SearchForLogPeriod(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<LogTO> SearchLogsForReader(int readerID)
        {
            try
            {
                return logDAO.getLogsForReader(readerID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Log.SearchForLogPeriod(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = logDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employee.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                logDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employee.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                logDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employee.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return logDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employee.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                logDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employee.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
