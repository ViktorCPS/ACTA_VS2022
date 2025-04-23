using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

using TransferObjects;


namespace DataAccess
{
	/// <summary>
	/// Summary description for XMLLogDAO.
	/// </summary>
	public class XMLLogDAO : LogDAO
	{
		public XMLLogDAO()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#region LogDAO Members

		public int insert(LogTO logTo)
		{
			// TODO:  Add XMLLogDAO.insert implementation
			return 0;
		}

        public int insertMobile(LogTmpAdditionalInfoTO logTo, bool doCommit)
        {
            // TODO:  Add XMLLogDAO.insertMobile implementation
            return 0;
        }

		public bool delete(int LogID)
		{
			// TODO:  Add XMLLogDAO.delete implementation
			return false;
		}

		public bool update(LogTO logTo, bool doCommit)
		{
			// TODO:  Add XMLLogDAO.update implementation
			return false;
		}

		public LogTO find(int LogID)
		{
			// TODO:  Add XMLLogDAO.find implementation
			return null;
		}

        public List<LogTO> getLogs(LogTO logTo)
		{
			// TODO:  Add XMLLogDAO.getLogs implementation
			return null;
		}
        public List<LogTO> getLogsForReader(int readerID)
        {
            return null;
        }
        public List<LogTO> getLogIn(int employeeID, DateTime date)
        {
            // TODO:  Add XMLLogDAO.getLogs implementation
            return null;
        }
        public List<LogTO> getLogs(string logID, string readerID, string tagID, string antenna, string eventHappened, string actionCommited, string eventTime, string passGenUsed)
		{
			// TODO:  Add XMLLogDAO.DataAccess.LogDAO.getLogs implementation
			return null;
		}

        public Dictionary<long, LogTO> getLogs(int event1, int event2, int passGenUsed)
		{
			// TODO:  Add XMLLogDAO.DataAccess.LogDAO.getLogs implementation
			return null;
		}

        public List<LogTO> getLogsForPeriod(LogTO logTO, DateTime from, DateTime to)
        {
            // TODO:  Add XMLLogDAO.DataAccess.LogDAO.getLogs implementation
            return null;
        }

        public bool serialize(List<LogTO> LogTOList, string filePath)
		{
			bool isSerialized = false;

			try
			{
				Stream stream = File.Open(filePath, FileMode.Create);
				LogTO[] logTOArray = (LogTO[]) LogTOList.ToArray();

				XmlSerializer bformatter = new XmlSerializer(typeof(LogTO[]));
				bformatter.Serialize(stream, logTOArray);
				stream.Close();
				isSerialized = true;
			}
			catch(Exception ex)
			{
				throw ex;
			}

			return isSerialized;
		}

		public List<LogTO> deserialize(string filePath)
		{
            List<LogTO> logList = new List<LogTO>();

			try
			{
				if (File.Exists(filePath))
				{
					Stream stream = File.Open(filePath, FileMode.Open);

					XmlSerializer bformatter = new XmlSerializer(typeof(LogTO[]));
					LogTO[] deserialized = (LogTO[]) bformatter.Deserialize(stream);
					ArrayList logs = ArrayList.Adapter(deserialized);

                    foreach (LogTO log in logs)
                    {
                        logList.Add(log);
                    }

					stream.Close();
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}

			return logList;
		}

        public bool serializeMobile(List<LogTmpAdditionalInfoTO> LogTOList, string filePath)
        {
            bool isSerialized = false;

            try
            {
                Stream stream = File.Open(filePath, FileMode.Create);
                LogTmpAdditionalInfoTO[] logTOArray = (LogTmpAdditionalInfoTO[])LogTOList.ToArray();

                XmlSerializer bformatter = new XmlSerializer(typeof(LogTmpAdditionalInfoTO[]));
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

        public List<LogTmpAdditionalInfoTO> deserializeMobile(string filePath)
        {
            List<LogTmpAdditionalInfoTO> logList = new List<LogTmpAdditionalInfoTO>();

            try
            {
                if (File.Exists(filePath))
                {
                    Stream stream = File.OpenRead(filePath);
                    XmlSerializer bformatter = new XmlSerializer(typeof(LogTmpAdditionalInfoTO[]));
                    LogTmpAdditionalInfoTO[] deserialized;

                    try
                    {
                        deserialized = (LogTmpAdditionalInfoTO[])bformatter.Deserialize(stream);
                        ArrayList logs = ArrayList.Adapter(deserialized);

                        foreach (LogTmpAdditionalInfoTO log in logs)
                        {
                            logList.Add(log);
                        }
                    }
                    catch (Exception ex)
                    {
                        stream.Close();
                        throw ex;
                    }

                    stream.Close();
                }
            }
            catch (IOException ioEx)
            {
                throw ioEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return logList;
        }

		public int insertToTmp(LogTO logTo)
		{
			// TODO:  Add XMLLogDAO.insertToTmp implementation
			return 0;
		}

        public int insertToMobileTmp(LogTmpAdditionalInfoTO logTo)
        {
            // TODO:  Add XMLLogDAO.insertToMobileTmp implementation
            return 0;
        }

		public bool clearLogTmp()
		{
			// TODO:  Add XMLLogDAO.deleteTableTmpLog implementation
			return false;
		}

        public bool clearLogMobileTmp()
        {
            // TODO:  Add XMLLogDAO.deleteTableTmpLog implementation
            return false;
        }

		public int importLog()
		{
			// TODO:  Add XMLLogDAO.importLog implementation
			return 0;
		}

        public List<LogTmpAdditionalInfoTO> getLogMobileTmp()
        {
            // TODO:  Add XMLLogDAO.getLogMobileTmp implementation
            return null;
        }

        public List<LogTO> getTrespassLogs(int locationID, int gateID, int readerID, string direction, int employeeID, int eventHappened, DateTime dateFrom, DateTime dateTo)
        {
            return null;
        }

        public bool deleteLogs(DateTime dateFrom, DateTime dateTo, DateTime timeFrom, DateTime timeTo, string readerID)
        {
            return false;
        }

        public List<LogTO> getLogsForGraph(DateTime dateFrom, DateTime dateTo, DateTime timeFrom, DateTime timeTo, string readerID)
        {
            return null;
        }
        public bool beginTransaction()
        {
            return false;
        }

        public void commitTransaction()
        {
        }

        public void rollbackTransaction()
        {
        }

        public void setTransaction(IDbTransaction trans)
        {
        }

        public IDbTransaction getTransaction()
        {
            return null;
        }
		#endregion
	}
}
