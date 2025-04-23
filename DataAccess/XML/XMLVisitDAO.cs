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
	/// DAO implementation for managing Visit's data form XML files, 
	/// using serialization/deserialization.
	/// </summary>
	public class XMLVisitDAO : VisitDAO
	{
		private static DataSet cachedVisitsTO = new DataSet();
		protected string dateTimeformat = "";
		private const string tableName = "AllVisits";
		private const string resultTableName = "resultRows";
		
		private DataSet getCachedVisits()
		{
			
			if (!cachedVisitsTO.Tables.Contains(tableName) 
				|| (((DataTable) cachedVisitsTO.Tables[tableName]).Rows.Count == 0))
			{
				deserializeToCache();
			}
			
			return cachedVisitsTO;
		}

		public XMLVisitDAO()
		{
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;	
		}
		#region VisitDAO Members

		public int insert(int visitID, int employeeID, string firstName, 
			string lastName, string visitorJMBG, string visitorID, 
			DateTime dateStart, DateTime dateEnd, int visitedPerson, 
			int visitedWorkingUnit, string visitDescr, int locationID, 
			string remarks)
		{
			ArrayList visitsTOList = new ArrayList();
			
			int inserted = 0;
			// XML resource file Name
			//string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLVisitsFile"];
            string filename = Constants.XMLDataSourceDir + Constants.XMLVisitsFile;
			//string insertFilename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLVisitsUpdateFile"];
            string insertFilename = Constants.XMLDataSourceDir + Constants.XMLVisitsUpdateFile;

			try
			{	
				VisitTO newVisit = new VisitTO(visitID,  employeeID,  firstName, 
						lastName,  visitorJMBG,  visitorID, 
						dateStart,  dateEnd,  visitedPerson, 
						visitedWorkingUnit,  visitDescr,  locationID, 
						remarks, "", "", "", "", "");


				// Deserialize DataSource XML file
				cachedVisitsTO.Clear();
				deserializeToCache();

				// Check constrains
				string select = "EmployeeID = " + employeeID + " and DateEnd = '" 
						+ new DateTime().ToString(dateTimeformat) + "' "; 
				DataRow[] resultRows = cachedVisitsTO.Tables[tableName].Select(select);

				if (resultRows.Length.Equals(0))
				{
					visitsTOList = dataTable2ArrayList(cachedVisitsTO.Tables[tableName]);

					// Add new VisitTO to cachedVisits
					visitsTOList.Add(newVisit);

					// Serialize to datasource
					if (serialize(visitsTOList, filename))
					{
						// change cached data
						cachedVisitsTO = toDataSet(visitsTOList);	
					}

					ArrayList newVisitsList = new ArrayList(deserialize(insertFilename));
					newVisitsList.Add(newVisit);
					// Serialize to Update XML file
					serialize(newVisitsList, insertFilename);
					inserted = 1;
				}
				else
				{
					throw new Exception("Can't insert, constraint violated");
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			return inserted;
		}

		public int insert(TransferObjects.VisitTO visitTO)
		{
			int inserted = 0;

			try
			{
				inserted = this.insert(visitTO.VisitID,  visitTO.EmployeeID,  visitTO.FirstName, 
						visitTO.LastName,  visitTO.VisitorJMBG,  visitTO.VisitorID, 
						visitTO.DateStart,  visitTO.DateEnd,  visitTO.VisitedPerson, 
						visitTO.VisitedWorkingUnit,  visitTO.VisitDescr,  visitTO.LocationID, 
						visitTO.Remarks);
			}
			catch(Exception ex)
			{
				throw ex;
			}
			return inserted;
		}
        public int insert(TransferObjects.VisitTO visitTO, bool doCommit)
        {
            return -1;
        }


		public bool delete(string visitID)
		{
			// TODO:  Add XMLVisitDAO.delete implementation
			return false;
		}

		public bool update(int visitID, int employeeID, string firstName, string lastName, string visitorJMBG, string visitorID, DateTime dateStart, DateTime dateEnd, int visitedPerson, int visitedWorkingUnit, string visitDescr, int locationID, string remarks)
		{
			// TODO:  Add XMLVisitDAO.update implementation
			return false;
		}
        public bool update( string firstName, string lastName, string jmbg)
            {
			// TODO:  Add XMLVisitDAO.update implementation
			return false;
		}
		public bool update(TransferObjects.VisitTO visitTO)
		{
			int updated = 0;
			bool isUpdated = false;

			ArrayList visitsTOList = new ArrayList();
			
			// XML resource file Name
			//string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLVisitsFile"];
            string filename = Constants.XMLDataSourceDir + Constants.XMLVisitsFile;
			//string insertFilename = ConfigurationManager.AppSettings["XMLUpdateFilesDir"] + ConfigurationManager.AppSettings["XMLVisitsUpdateFile"];
            string insertFilename = ConfigurationManager.AppSettings["XMLUpdateFilesDir"] + Constants.XMLVisitsUpdateFile;

			try
			{	
				// Deserialize DataSource XML file
				cachedVisitsTO.Clear();
				deserializeToCache();
				visitsTOList = dataTable2ArrayList(cachedVisitsTO.Tables[tableName]);

				// Add End date
				foreach(VisitTO visitMember in visitsTOList)
				{
					//if (visitMember.VisitID.Equals(visitTO))
					if ((visitMember.EmployeeID.Equals(visitTO.EmployeeID)) 
						&& (visitMember.DateEnd.Equals(new DateTime()))
						&& (!visitTO.DateEnd.Equals(new DateTime()))
						&& (visitMember.DateStart.Equals(visitTO.DateStart)))
					{
						visitMember.DateEnd = visitTO.DateEnd;
						visitMember.Remarks = visitTO.Remarks;

						updated ++;

						break;
					}
				}

				// Serialize to datasource
				if (serialize(visitsTOList, filename))
				{
					// change cached data
					cachedVisitsTO = toDataSet(visitsTOList);	
				}

				ArrayList newVisitsList = new ArrayList(deserialize(insertFilename));

				if (newVisitsList.Count > 0)
				{
					// Add End date if file exists
					foreach(VisitTO visitMember in newVisitsList)
					{
						if ((visitMember.EmployeeID.Equals(visitTO.EmployeeID)) 
							&& (visitMember.DateEnd.Equals(new DateTime()))
							&& (!visitTO.DateEnd.Equals(new DateTime()))
							&& (visitMember.DateStart.Equals(visitTO.DateStart)))
						{
							visitMember.DateEnd = visitTO.DateEnd;
							visitMember.Remarks = visitTO.Remarks;
							updated ++;
							break;
						}
					}
				}
				else
				{
					// List is empty, add to newVisitsList
					newVisitsList.Add(visitTO);
					updated ++;
				}

				// Serialize to Update XML file
				serialize(newVisitsList, insertFilename);

				if (updated == 2)
				{
					isUpdated = true;
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			return isUpdated;
		}
        public bool updateVisit(TransferObjects.VisitTO visitTO)
        {
            int updated = 0;
            bool isUpdated = false;

            ArrayList visitsTOList = new ArrayList();

            // XML resource file Name
            //string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLVisitsFile"];
            string filename = Constants.XMLDataSourceDir + Constants.XMLVisitsFile;
            //string insertFilename = ConfigurationManager.AppSettings["XMLUpdateFilesDir"] + ConfigurationManager.AppSettings["XMLVisitsUpdateFile"];
            string insertFilename = ConfigurationManager.AppSettings["XMLUpdateFilesDir"] + Constants.XMLVisitsUpdateFile;

            try
            {
                // Deserialize DataSource XML file
                cachedVisitsTO.Clear();
                deserializeToCache();
                visitsTOList = dataTable2ArrayList(cachedVisitsTO.Tables[tableName]);

                // Add End date
                foreach (VisitTO visitMember in visitsTOList)
                {
                    //if (visitMember.VisitID.Equals(visitTO))
                    if ((visitMember.EmployeeID.Equals(visitTO.EmployeeID))
                        && (visitMember.DateEnd.Equals(new DateTime()))
                        && (!visitTO.DateEnd.Equals(new DateTime()))
                        && (visitMember.DateStart.Equals(visitTO.DateStart)))
                    {
                        visitMember.DateEnd = visitTO.DateEnd;
                        visitMember.Remarks = visitTO.Remarks;

                        updated++;

                        break;
                    }
                }

                // Serialize to datasource
                if (serialize(visitsTOList, filename))
                {
                    // change cached data
                    cachedVisitsTO = toDataSet(visitsTOList);
                }

                ArrayList newVisitsList = new ArrayList(deserialize(insertFilename));

                if (newVisitsList.Count > 0)
                {
                    // Add End date if file exists
                    foreach (VisitTO visitMember in newVisitsList)
                    {
                        if ((visitMember.EmployeeID.Equals(visitTO.EmployeeID))
                            && (visitMember.DateEnd.Equals(new DateTime()))
                            && (!visitTO.DateEnd.Equals(new DateTime()))
                            && (visitMember.DateStart.Equals(visitTO.DateStart)))
                        {
                            visitMember.DateEnd = visitTO.DateEnd;
                            visitMember.Remarks = visitTO.Remarks;
                            updated++;
                            break;
                        }
                    }
                }
                else
                {
                    // List is empty, add to newVisitsList
                    newVisitsList.Add(visitTO);
                    updated++;
                }

                // Serialize to Update XML file
                serialize(newVisitsList, insertFilename);

                if (updated == 2)
                {
                    isUpdated = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isUpdated;
        }
		public VisitTO find(string visitID)
		{
			DataSet sourceData = new DataSet();
			sourceData = getCachedVisits();
			DataTable sourceTable = new DataTable();
			ArrayList resultList = new ArrayList();
			VisitTO result = new VisitTO();


			if ((sourceData.Tables.Contains(tableName)) && ((sourceTable = sourceData.Tables[tableName]).Rows.Count > 0))
			{
				DataTable resultTable = new DataTable();
				resultTable = sourceTable.Clone();
			
				try
				{
					string select = "EmployeeID = '" + visitID.ToString() + "' and DateEnd = '" 
						+ new DateTime().ToString(dateTimeformat) + "' "; 
					
					DataRow[] rows = sourceTable.Select(select);

					foreach(DataRow row in rows)
					{
						resultTable.ImportRow(row);
					}

					resultList = dataTable2ArrayList(resultTable);
					
					if (resultList.Count == 1)
					{ 
						result = (VisitTO) resultList[0];
					}
				}
				catch(Exception ex)
				{
					throw ex;
				}
			}

			return result;
			
		}

		public ArrayList getVisits(int visitID, int employeeID, string firstName, string lastName, string visitorJMBG, string visitorID, DateTime dateStart, DateTime dateEnd, int visitedPerson, int visitedWorkingUnit, string visitDescr, int locationID, string remarks)
		{
			// TODO:  Add XMLVisitDAO.getVisits implementation
			return null;
		}
        public ArrayList getVisitsUNIPROM(int visitID, int employeeID, string firstName, string lastName, string visitorJMBG, string visitorID, DateTime dateStart, DateTime dateEnd, int visitedPerson, int visitedWorkingUnit, string visitDescr, int locationID, string remarks)
        {
            // TODO:  Add XMLVisitDAO.getVisits implementation
            return null;
        }

        public ArrayList getVisitsDateInterval(string employeeID, string visitedWorkingUnit,
            string visitedPerson, string visitorIdent, DateTime dateFrom, DateTime dateTo,
            string visitDescr, string wUnits)
        {
            // TODO:  Add XMLVisitDAO.getVisits implementation
            return null;
        }
       public ArrayList getVisitsDateInterval(string employeeID, string visitedWorkingUnit, string visitedPerson, string visitorIdent,
            DateTime dateFrom, DateTime dateTo, string visitDescr, string wUnits, string state, string company, string visitor, string privacy, string visitAsco4, Dictionary<int, List<VisitAsco4TO>> ascoTO)
        {
            // TODO:  Add XMLVisitDAO.getVisits implementation
            return null;
        }

        public ArrayList getVisitsDateIntervalDistinct(string employeeID, string visitedWorkingUnit, string visitedPerson, string visitorIdent,
          DateTime dateFrom, DateTime dateTo, string visitDescr, string wUnits, string state, string company, string visitor, string privacy, string visitAsco4, Dictionary<int, List<VisitAsco4TO>> ascoTO)
        {
            // TODO:  Add XMLVisitDAO.getVisits implementation
            return null;
        }

        public int getVisitsDateIntervalCount(string employeeID, string visitedWorkingUnit,
            string visitedPerson, string visitorIdent, DateTime dateFrom, DateTime dateTo,
            string visitDescr, string wUnits)
        {
            // TODO:  Add XMLVisitDAO.getVisits implementation
            return 0;
        }

        public ArrayList getCurrentVisits(string wUnits)
		{
			DataSet sourceData = new DataSet();
			sourceData = getCachedVisits();
			DataTable sourceTable = new DataTable();
			ArrayList resultList = new ArrayList();

			if ((sourceData.Tables.Contains(tableName)) && ((sourceTable = sourceData.Tables[tableName]).Rows.Count > 0))
			{
				DataTable resultTable = new DataTable();
				resultTable = sourceTable.Clone();
				StringBuilder sb = new StringBuilder();
			
				try
				{
					string select = "DateEnd = '" + new DateTime().ToString(dateTimeformat) + "' ";
					DataRow[] rows = sourceTable.Select(select);

					foreach(DataRow row in rows)
					{
						resultTable.ImportRow(row);
					}

					resultList = dataTable2ArrayList(resultTable);
				}
				catch(Exception ex)
				{
					throw ex;
				}
			}

			return resultList;
		}

        public ArrayList getCurrentVisitsDetail(string wUnits)
        {
            // TODO:  Add XMLVisitDAO.getVisits implementation
            return null;
        }

        public ArrayList getAllVisits(string wUnits)
        {
            // TODO:  Add XMLVisitDAO.getVisits implementation
            return null;
        }

        public VisitTO findMAXVisitPIN(string PIN)
        {
            return null;
        }

        public VisitTO findMAXVisitIdCard(string idCard)
        {
            return null;
        }

		/// <summary>
		/// Serialize data to DataSource file
		/// </summary>
		/// <param name="visitTO"></param>
		/// <returns></returns>
		public bool serialize(ArrayList visitTO)
		{
			bool isSerialized = false;
			try
			{
				string filePath = Constants.XMLDataSourceDir
                    + Constants.XMLVisitsFile;
                //+ ConfigurationManager.AppSettings["XMLVisitsFile"];

				serialize(visitTO, filePath);

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
			ArrayList visitTOList = new ArrayList();

			try
			{
				//if (File.Exists(Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLVisitsFile"]))
                if (File.Exists(Constants.XMLDataSourceDir + Constants.XMLVisitsFile))
				{
					//string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLVisitsFile"];
                    string filename = Constants.XMLDataSourceDir + Constants.XMLVisitsFile;
					Stream stream = File.Open(filename, FileMode.Open);

					XmlSerializer bformatter = new XmlSerializer(typeof(VisitTO[]));
					VisitTO[] deserialized = (VisitTO[]) bformatter.Deserialize(stream);
					visitTOList = ArrayList.Adapter(deserialized);
					stream.Close();

					cachedVisitsTO = toDataSet(visitTOList);
				}
				else
				{
					cachedVisitsTO = new DataSet();
					DataTable dataTable = new DataTable();
					cachedVisitsTO.Tables.Add(tableName);
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		private DataSet toDataSet(ArrayList list)
		{
			DataSet dataset = new DataSet();

			try
			{
				DataTable dataTable = new DataTable();

				dataset.Tables.Add(tableName);
				dataTable = dataset.Tables[tableName];

				dataTable.Columns.Add("VisitID", typeof(string));
				dataTable.Columns.Add("EmployeeID", typeof(string));
				dataTable.Columns.Add("FirstName", typeof(string));
				dataTable.Columns.Add("LastName", typeof(string));
				dataTable.Columns.Add("VisitorJMBG", typeof(string));
				dataTable.Columns.Add("VisitorID", typeof(string));
				dataTable.Columns.Add("DateStart", typeof(string));
				dataTable.Columns.Add("DateEnd", typeof(string));
				dataTable.Columns.Add("VisitedPerson", typeof(string));
				dataTable.Columns.Add("VisitedWorkingUnit", typeof(string));
				dataTable.Columns.Add("VisitDescr", typeof(string));
				dataTable.Columns.Add("LocationID", typeof(string));
				dataTable.Columns.Add("Remarks", typeof(string));

				foreach(VisitTO visit in list)
				{
					DataRow row = dataTable.NewRow();

					row["VisitID"] = visit.VisitID;
					row["EmployeeID"] = visit.EmployeeID.ToString();
					row["FirstName"] = visit.FirstName;
					row["LastName"] = visit.LastName;
					row["VisitorJMBG"] = visit.VisitorJMBG;
					row["VisitorID"] = visit.VisitorID;
					row["DateStart"] = visit.DateStart.ToString(dateTimeformat.Replace("'", "")).Trim();
					row["DateEnd"] = visit.DateEnd.ToString(dateTimeformat.Replace("'", "")).Trim();
					row["VisitedPerson"] = visit.VisitedPerson;
					row["VisitedWorkingUnit"] = visit.VisitedWorkingUnit.ToString();
					row["VisitDescr"] = visit.VisitDescr;
					row["LocationID"] = visit.LocationID.ToString();
					row["Remarks"] = visit.Remarks;
				
					dataTable.Rows.Add(row);
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
				
			return dataset;
		}

		private ArrayList dataTable2ArrayList(DataTable dataTable)
		{
			ArrayList visitsTOList = new ArrayList();

			try
			{
				VisitTO visitTO = new VisitTO();

				foreach(DataRow row in dataTable.Rows)
				{
					visitTO = new VisitTO();

					visitTO.VisitID = Int32.Parse(row["VisitID"].ToString());
					visitTO.EmployeeID = Int32.Parse(row["EmployeeID"].ToString());
					visitTO.FirstName = row["FirstName"].ToString();
					visitTO.LastName = row["LastName"].ToString();
					visitTO.VisitorJMBG = row["VisitorJMBG"].ToString();
					visitTO.VisitorID = row["VisitorID"].ToString();
					visitTO.DateStart = Convert.ToDateTime(row["DateStart"].ToString());
					visitTO.DateEnd = Convert.ToDateTime(row["DateEnd"].ToString());
					visitTO.VisitedPerson = Int32.Parse(row["VisitedPerson"].ToString());
					visitTO.VisitedWorkingUnit = Int32.Parse(row["VisitedWorkingUnit"].ToString());
					visitTO.VisitDescr = row["VisitDescr"].ToString();
					visitTO.LocationID = Int32.Parse(row["LocationID"].ToString());
					visitTO.Remarks = row["Remarks"].ToString();

					visitsTOList.Add(visitTO);
				}

			}
			catch(Exception ex)
			{
				throw ex;
			}

			return visitsTOList;
		}

		public ArrayList deserialize(string filePath)
		{
			ArrayList visitListTO = new ArrayList();
			try
			{
				if (File.Exists(filePath))
				{
					Stream stream = File.Open(filePath, FileMode.Open);

					XmlSerializer bformatter = new XmlSerializer(typeof(VisitTO[]));
					VisitTO[] deserialized = (VisitTO[]) bformatter.Deserialize(stream);
					visitListTO = ArrayList.Adapter(deserialized);
					stream.Close();
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}

			return visitListTO;
		}

		public bool serialize(ArrayList visitTO, string filePath)
		{
			bool isSerialized = false;
			try
			{
				
				Stream stream = File.Open(filePath, FileMode.Create);

				VisitTO[] visitsArray = (VisitTO[]) visitTO.ToArray(typeof(VisitTO));

				XmlSerializer bformatter = new XmlSerializer(typeof(VisitTO[]));
				bformatter.Serialize(stream, visitsArray);
				stream.Close();
				isSerialized = true;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			return isSerialized;
		}
        public List<string> getDistinctVisitType()
        {
            return null;
        }
        public List<string> getDistinctName()
        {
            return null;
        }
        public List<string> getDistinctCompany()
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
        public ArrayList getVisitsCompleted()
        {
            return null;
        }
        public ArrayList getVisitsNotCompleted(bool notCompleted)
        {
            return null;
        }
        public DataSet getVisitsCompletedDS()
        {
            return null;
        }
	}

}
