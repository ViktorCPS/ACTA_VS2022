using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

using TransferObjects;
using Util;
using System.Collections.Generic;

namespace DataAccess
{
	/// <summary>
	/// DAO implementation for managing Passes data form XML files, 
	/// using serialization/deserialization.
	/// </summary>
	public class XMLPassDAO : PassDAO
	{
		private static DataSet cachedPassesTO = new DataSet();
		protected string dateTimeformat = "";
		private const string tableName = "AllPasses";
		private const string resultTableName = "resultRows";
		
		private DataSet getCachedVisits()
		{			
			if (!cachedPassesTO.Tables.Contains(tableName) 
				|| (((DataTable) cachedPassesTO.Tables[tableName]).Rows.Count == 0))
			{
				deserializeToCache();
			}
			
			return cachedPassesTO;
		}

		public XMLPassDAO()
		{
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;	
		}

		#region PassDAO Members

		public int insert(int employeeID, string direction, DateTime eventTime, int passTypeID,
            int pairGenUsed, int locationID, int manualCreated, string createdBy, DateTime createdTime,int isWrkHrsCount)
		{
			List<PassTO> passes = new List<PassTO>();
			int inserted = 0;

			try
			{
				//string filePath = ConfigurationManager.AppSettings["XMLUpdateFilesDir"] + 
				string filePath = Constants.XMLUpdateFilesDir +
                    Constants.XMLPassesUpdateFile;
					//ConfigurationManager.AppSettings["XMLPassesUpdateFile"];

				PassTO newPass = new PassTO(-1,employeeID, direction, eventTime, passTypeID, 
								pairGenUsed, locationID, manualCreated, createdBy, createdTime, isWrkHrsCount); 
				
				// Deserialize already existing passes
				passes = deserialize(filePath);

				// Add new Pass
				passes.Add(newPass);
		
				// Serialize again
				serialize(passes, filePath);
				inserted = 1;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			
			return inserted;
		}

        public int insert(int employeeID, string direction, DateTime eventTime, int passTypeID, int pairGenUsed,
           int locationID, int manualCreated, int isWrkHrsCount)
        { return 1; }
		public int insert(TransferObjects.PassTO passTO, bool doCommit)
		{
			// TODO:  Add XMLPassDAO.DataAccess.PassDAO.insert implementation
			return 0;
		}
        public int insertGetID(TransferObjects.PassTO passTO, bool doCommit)
        {
            // TODO:  Add XMLPassDAO.DataAccess.PassDAO.insert implementation
            return 0;
        }
		public int insert(List<PassTO> passTOList, ExitPermissionTO perm, string createdBy, DAOFactory factory)
		{
			// TODO:  Add XMLPassDAO.DataAccess.PassDAO.insert implementation
			return 0;
		}

		public int insert(PassTO passTo, string createdBy, bool doCommit)
		{
			// TODO:  Add XMLPassDAO.DataAccess.PassDAO.insert implementation
			return 0;
		}

		// TODO!!!!!
		public int insertPassesPermission(List<PassTO> passTOList, ExitPermissionTO perm, string createdBy, DAOFactory factory)
		{
			return 0;
		}

		public bool delete(string passID)
		{
			// TODO:  Add XMLPassDAO.delete implementation
			return false;
		}

		public bool delete(string passID, bool doCommit)
		{
			// TODO:  Add XMLPassDAO.delete implementation
			return false;
		}

        public bool log2pass(List<PassTO> passTOList, List<LogTO> logTO, ExitPermissionTO exitPerm, DAOFactory factory)
		{
			// TODO:  Add XMLPassDAO.log2pass implementation
			return false;
		}

        public void logToPass(List<PassTO> passTOList, List<LogTO> logTOList, DAOFactory factory)
		{
			// TODO:  Add XMLPassDAO.logToPass implementation
		}

		public bool update(int passID, int employeeID, string direction, DateTime eventTime, int passTypeID, int pairGenUsed, int locationID, int manualCreated, int isWrkHrsCount)
		{
			// TODO:  Add XMLPassDAO.update implementation
			return false;
		}

		public bool update(TransferObjects.PassTO passTo, bool doCommit)
		{
			// TODO:  Add XMLPassDAO.DataAccess.PassDAO.update implementation
			return false;
		}

		public bool update(int employeeID, string date, bool doCommit)
		{
			// TODO:  Add XMLPassDAO.DataAccess.PassDAO.update implementation
			return false;
		}

		public PassTO find(string passID)
		{
			// TODO:  Add XMLPassDAO.find implementation
			return null;
		}

		public List<PassTO> getPassesList(PassTO passTO)
		{
			// TODO:  Add XMLPassDAO.getPasses implementation
			return null;
		}

        public Dictionary<int, Dictionary<DateTime, List<PassTO>>> getPassesForEmployeesPeriod(string employeesID, DateTime from, DateTime to)
        {
            return null;
        }

        public List<PassTO> getCurrentPasses(PassTO passTO, DateTime fromTime, DateTime toTime, string wUnits, string modifiedBy)
        {
            return null;
        }

		public List<PassTO> getPassesInterval(PassTO passTO, DateTime fromTime, DateTime toTime, string wUnits, DateTime advFromTime, DateTime advToTime)
		{
			// TODO:  Add XMLPassDAO.getPassesInterval implementation
			return null;
		}

        public Dictionary<int, Dictionary<DateTime, List<PassTO>>> getPassesInterval(DateTime month, string emplIDs)
        {         
            return null;
        }

		public int getPassesIntervalCount(PassTO passTO, DateTime fromTime, DateTime toTime, string wUnits, DateTime advFromTime, DateTime advToTime)
		{
			// TODO:  Add XMLPassDAO.getPassesInterval implementation
			int count = 0;
			return count;
		}

        public List<PassTO> getPassesForSnapshots(string passID)
        {
            // TODO:  Add XMLPassDAO.getPassesInterval implementation
            return null;
        }

		public List<PassTO> getPasses(PassTO passTO)
		{
			// TODO:  Add XMLPassDAO.DataAccess.PassDAO.getPasses implementation
			return null;
		}

		public ArrayList getEmpoloyeesByDate()
		{
			// TODO:  Add XMLPassDAO.getEmpoloyeesByDate implementation
			return null;
		}

		public List<PassTO> getPassesForEmployee(int empoyeeID, string date)
		{
			// TODO:  Add XMLPassDAO.getPassesForEmployee implementation
			return null;
		}
        public List<PassTO> getPassesForEmployeeAll(int empoyeeID, DateTime date)
        {
            // TODO:  Add XMLPassDAO.getPassesForEmployee implementation
            return null;
        }
        public List<PassTO> getPassesForEmployeeSched(int empoyeeID)
        {
            // TODO:  Add XMLPassDAO.getPassesForEmployee implementation
            return null;
        }
		public List<PassTO> getPassesForExitPerm(int employeeID, DateTime eventTime)
		{
			// TODO:  Add XMLPassDAO.getPassesForEmployee implementation
			return null;
		}

		public List<PassTO> getPassesForExitPerm(int employeeID, DateTime startTime, int offset)
		{
			// TODO:  Add XMLPassDAO.getPassesForEmployee implementation
			return null;
		}

		public PassTO getPassBeforePermissionPass(int empoyeeID, DateTime eventTime)
		{
			// TODO:  Add XMLPassDAO.getPassesForEmployee implementation
			return null;
		}
        public List<PassTO> getDiferencePasses(DateTime eventTime)
        {
            // TODO:  Add XMLPassDAO.getPassesForEmployee implementation
            return null;
        }
		public List<PassTO> getPermPassesForPair(int employeeID, DateTime startTime, DateTime endTime)
		{
			// TODO:  Add XMLPassDAO.getPassesForEmployee implementation
			return null;
		}

        public List<PassTO> getPassForPerm(int employeeID, string direction, DateTime eventTime, int isWrkHrsCount)
        {
            // TODO:  Add XMLPassDAO.getPassesForEmployee implementation
            return null;
        }
        public void UnlockPasses(DateTime startDate, DateTime endTime, bool doCommit)
        {
            // TODO:  Add XMLPassDAO.getPassesForEmployee implementation
            return ;
        }
        public List<PassTO> getPassesIntervalForZINReport(DateTime fromTime, DateTime toTime, string employees)
        {
            // TODO:  Add XMLPassDAO.getPassesForEmployee implementation
            return null;
        }
        public int getPassesIntervalCountForZINReport(DateTime fromTime, DateTime toTime, string employees)
        {
            // TODO:  Add XMLPassDAO.getPassesForEmployee implementation
            return 0;
        }

        public bool beginTransaction()
        {
            return true;
        }

        public void commitTransaction()
        { 
        }

        public void rollbackTransaction()
        { 
        }

        public IDbTransaction getTransaction()
        {
            return null; 
        }

        public void setTransaction(IDbTransaction trans)
        { 
        }

		public bool serialize(List<PassTO> passesTOList)
		{
			bool isSerialized = false;
			try
			{
				string filePath = Constants.XMLDataSourceDir
                    + Constants.XMLPassesFile;
					//+ ConfigurationManager.AppSettings["XMLPassesFile"];

				serialize(passesTOList, filePath);

				isSerialized = true;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			return isSerialized;
		}

		#endregion

		private void deserializeToCache()
		{   
			List<PassTO> passesTOList = new List<PassTO>();
			try
			{
				if (File.Exists(Constants.XMLDataSourceDir
                    + Constants.XMLPassesFile))
					//+ ConfigurationManager.AppSettings["XMLPassesFile"]))
				{
					cachedPassesTO.Clear();
					passesTOList = deserialize(Constants.XMLDataSourceDir
                        + Constants.XMLPassesFile);
						//+ ConfigurationManager.AppSettings["XMLPassesFile"]);
					cachedPassesTO = toDataSet(passesTOList);
				}
				else
				{
					cachedPassesTO = new DataSet();
					DataTable dataTable = new DataTable();
					cachedPassesTO.Tables.Add(tableName);
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public List<PassTO> deserialize(string filePath)
		{
			List<PassTO> passesTO = new List<PassTO>();
			try
			{
				if (File.Exists(filePath))
				{
					Stream stream = File.Open(filePath, FileMode.Open);

					XmlSerializer bformatter = new XmlSerializer(typeof(PassTO[]));
					PassTO[] deserialized = (PassTO[]) bformatter.Deserialize(stream);
					ArrayList passesListTO = ArrayList.Adapter(deserialized);

                    foreach (PassTO passTO in passesListTO)
                    {
                        passesTO.Add(passTO);
                    }
                    
                    stream.Close();
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}

			return passesTO;
		}

		private DataSet toDataSet(List<PassTO> list)
		{
			DataSet dataset = new DataSet();

			try
			{
				DataTable dataTable = new DataTable();

				dataset.Tables.Add(tableName);
				dataTable = dataset.Tables[tableName];

				dataTable.Columns.Add("PassID", typeof(string));
				dataTable.Columns.Add("EmployeeID", typeof(string));
				dataTable.Columns.Add("Direction", typeof(string));
				dataTable.Columns.Add("EventTime", typeof(string));
				dataTable.Columns.Add("PassTypeID", typeof(string));
				dataTable.Columns.Add("PairGenUsed", typeof(string));
				dataTable.Columns.Add("LocationID", typeof(string));
				dataTable.Columns.Add("ManualyCreated", typeof(string));
				dataTable.Columns.Add("IsWrkHrsCount", typeof(string));

				foreach(PassTO pass in list)
				{
					DataRow row = dataTable.NewRow();

					row["PassID"] = pass.PassID;
					row["EmployeeID"] = pass.EmployeeID;
					row["Direction"] = pass.Direction;
					row["EventTime"] = pass.EventTime.ToString(dateTimeformat);
					row["PassTypeID"] = pass.PassTypeID;
					row["PairGenUsed"] = pass.PairGenUsed;
					row["LocationID"] = pass.LocationID;
					row["ManualyCreated"] = pass.ManualyCreated;
					row["IsWrkHrsCount"] = pass.IsWrkHrsCount;
					
					dataTable.Rows.Add(row);
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
				
			return dataset;
		}		

		private List<PassTO> dataTable2ArrayList(DataTable dataTable)
		{
			List<PassTO> passesTOList = new List<PassTO>();

			try
			{
				PassTO passTO = new PassTO();

				foreach(DataRow row in dataTable.Rows)
				{
					passTO = new PassTO();

					passTO.PassID = Int32.Parse(row["PassID"].ToString());
					passTO.EmployeeID = Int32.Parse(row["EmployeeID"].ToString());
					passTO.Direction = row["Direction"].ToString();
					passTO.EventTime = Convert.ToDateTime(row["EventTime"].ToString());
					passTO.PassTypeID = Int32.Parse(row["PassTypeID"].ToString());
					passTO.PairGenUsed = Int32.Parse(row["PairGenUsed"].ToString());
					passTO.LocationID = Int32.Parse(row["LocationID"].ToString());
					passTO.ManualyCreated = Int32.Parse(row["ManualyCreated"].ToString());
					passTO.IsWrkHrsCount = Int32.Parse(row["IsWrkHrsCount"].ToString());

					passesTOList.Add(passTO);
				}

			}
			catch(Exception ex)
			{
				throw ex;
			}

			return passesTOList;
		}

		private bool serialize(List<PassTO> passesTO, string filePath)
		{
			bool isSerialized = false;
			try
			{
				Stream stream = File.Open(filePath, FileMode.Create);

				PassTO[] passesArray = (PassTO[]) passesTO.ToArray();

				XmlSerializer bformatter = new XmlSerializer(typeof(PassTO[]));
				bformatter.Serialize(stream, passesArray);
				stream.Close();
				isSerialized = true;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			return isSerialized;
		}

        //TAMARA 30.1.2020.
        public bool compareGates(int gateID, int organUnitID)
        {
            throw new Exception();
        }

        public int getOrganUnitID(ulong tagID)
        {
            throw new Exception();
        }

        public int getGate(int readerID)
        {
            throw new Exception();
        }
        public List<PassTO> getListOfIN_OUTforDay(DateTime date, int emplID, string direction)
        {
            throw new Exception();
        }
        public void updateListPassesToWrkHrs0(string passIDs)
        {
        }
        public List<PassTO> getPassesForDayForEmployee(int emplID, DateTime day)
        {
            throw new Exception();
        }
        public void updatePassesOnUnprocessed(int emplID, DateTime date, bool doCommit)
        {
        }
	}
}
