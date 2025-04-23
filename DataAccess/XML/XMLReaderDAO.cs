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
using Util;

namespace DataAccess
{
	/// <summary>
	/// DAO implementation for managing Readers data form XML files, 
	/// using serialization/deserialization.
	/// </summary>
	public class XMLReaderDAO : ReaderDAO
	{
		private static DataSet cachedReadersTO = new DataSet();
		private const string tableName = "AllReaders";
		private const string resultTableName = "ResultTable";
		protected string dateTimeformat = "";

		public XMLReaderDAO()
		{
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

		#region ReaderDAO Members


        public List<ReaderTO> searchForIDs(string readerID)
        {
            return null;
        }
		public int insert(ReaderTO readerData)
		{
			// TODO:  Add XMLReaderDAO.insert implementation
			return 0;
		}

        public int insert(ReaderTO readerData,bool doCommit)
        {
            // TODO:  Add XMLReaderDAO.insert implementation
            return 0;
        }

		public bool delete(int readerID)
		{
			// TODO:  Add XMLReaderDAO.delete implementation
			return false;
		}

		public bool update(ReaderTO readerData)
		{
			// TODO:  Add XMLReaderDAO.update implementation
			return false;
		}

        public bool update(ReaderTO readerData,bool doCommit)
        {
            // TODO:  Add XMLReaderDAO.update implementation
            return false;
        }

		public ReaderTO find(string readerID)
		{
			// TODO:  Add XMLReaderDAO.find implementation
			return null;
		}

        public DateTime getLastLogUsed(int readerID, string direction)
        {
            return new DateTime();
        }

		public int findMAXReaderID()
		{
			// TODO:  Add XMLReaderDAO.findMAXReaderID implementation
			return 0;
		}

        public List<ReaderTO> getReaders(ReaderTO readerData)
		{
			DataSet dataSet = getCachedReaders();
            List<ReaderTO> resultList = new List<ReaderTO>();

			if (dataSet.Tables.Contains(tableName) && ((DataTable) dataSet.Tables[tableName]).Rows.Count > 0)
			{
				DataTable table = dataSet.Tables[tableName];
				DataTable resultTable = new DataTable(resultTableName);
				resultTable = table.Clone();
				StringBuilder sb = new StringBuilder();
				string select = "";
				
				try
				{
					if ((!readerData.ReaderID.Equals(-1)) || 
						(!readerData.Description.Trim().Equals("")) || 
						(!readerData.ConnectionType.Trim().Equals("")) ||
						(!readerData.ConnectionAddress.Trim().Equals("")) ||

						(!readerData.A0GateID.Equals(-1)) ||
						(!readerData.A0LocID.Equals(-1)) ||
						(!readerData.A0Direction.Trim().Equals("")) ||
						(!readerData.A0SecLocID.Equals(-1)) ||
						(!readerData.A0SecDirection.Trim().Equals("")) ||
						(!readerData.A0IsCounter.Equals(-1)) ||

						(!readerData.A1GateID.Equals(-1)) ||
						(!readerData.A1LocID.Equals(-1)) ||
						(!readerData.A1Direction.Trim().Equals("")) ||
						(!readerData.A1SecLocID.Equals(-1)) ||
						(!readerData.A1SecDirection.Trim().Equals("")) ||
						(!readerData.A1IsCounter.Equals(-1)) ||

						//(!readerData.Settings.Equals(0)) ||
						(!readerData.TechType.Trim().Equals("")))

					{

						if (!readerData.ReaderID.Equals(-1))
						{
							sb.Append("  ReaderID = '" + readerData.ReaderID.ToString() + "' and");
						}
						if (!readerData.Description.Trim().Equals(""))
						{
							sb.Append(" Description like '" + readerData.Description.Trim().ToUpper() + "' and");
						}
						if (!readerData.ConnectionType.Trim().Equals(""))
						{
							sb.Append(" ConnectionType like '" + readerData.ConnectionType.Trim().ToUpper() + "' and");
						}
						if (!readerData.ConnectionAddress.Trim().Equals(""))
						{
							sb.Append(" ConnectionAddress like '" + readerData.ConnectionAddress.Trim().ToUpper() + "' and");
						}
						if (!readerData.A0GateID.Equals("-1"))
						{
							sb.Append(" A0GateID = '" + readerData.A0GateID.ToString() + "' and");
						}
						if (!readerData.A0LocID.Equals("-1"))
						{
							sb.Append(" A0LocID = '" + readerData.A0LocID.ToString() + "' and");
						}
						if (!readerData.A0Direction.Trim().Equals(""))
						{
							sb.Append(" A0Direction like '" + readerData.A0Direction.Trim().ToUpper() + "' and");
						}
						if (!readerData.A0SecLocID.Equals(-1))
						{
							sb.Append(" A0SecLocID like '" + readerData.A0SecLocID.ToString() + "' and");
						}
						if (!readerData.A0SecDirection.Trim().Equals(""))
						{
							sb.Append(" A0SecDirection like '" + readerData.A0SecDirection.Trim().ToUpper() + "' and");
						}
						if (!readerData.A0IsCounter.Equals(-1))
						{
							sb.Append(" A0IsCounter like '" + readerData.A0IsCounter.ToString() + "' and");
						}
						if (!readerData.A1IsCounter.Equals(-1))
						{
							sb.Append(" A1IsCounter like '" + readerData.A1IsCounter.ToString() + "' and");
						}
						if (!readerData.A1GateID.Equals("-1"))
						{
							sb.Append(" A1GateID = '" + readerData.A1GateID.ToString() + "' and");
						}
						if (!readerData.A1LocID.Equals(-1))
						{
							sb.Append(" A1LocID = '" + readerData.A1LocID.ToString() + "' and");
						}
						if (!readerData.A1Direction.Trim().Equals(""))
						{
							sb.Append(" A1Direction like '" + readerData.A1Direction.Trim().ToUpper() + "' and");
						}
						if (!readerData.A1SecLocID.Equals(-1))
						{
							sb.Append(" A1SecLocID like '" + readerData.A1SecLocID.ToString() + "' and");
						}
						if (!readerData.A1SecDirection.Trim().Equals(""))
						{
							sb.Append(" A1SecDirection like '" + readerData.A1SecDirection.Trim().ToUpper() + "' and");
						}
					
						if (!readerData.TechType.Trim().Equals(""))
						{
							sb.Append(" TechType like '" + readerData.TechType.Trim().ToUpper() + "' and");
						}

						select = sb.ToString(0, sb.ToString().Length - 3);

						table.CaseSensitive = false;
						DataRow[] rows = table.Select(select);

						foreach(DataRow row in rows)
						{
							resultTable.ImportRow(row);
						}
					}
					else
					{
						resultTable = table;	
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
        public Dictionary<int, ReaderTO> getReadersDictionary(ReaderTO readerData)
        {
            Dictionary<int, ReaderTO> resultList = new Dictionary<int, ReaderTO>();          

            return resultList;
        }


        List<ReaderTO> ReaderDAO.getReaders(string[] gatesArray)
		{
			DataSet dataSet = getCachedReaders();
            List<ReaderTO> resultList = new List<ReaderTO>();

			if (dataSet.Tables.Contains(tableName) && ((DataTable) dataSet.Tables[tableName]).Rows.Count > 0)
			{
				DataTable table = dataSet.Tables[tableName];
				DataTable resultTable = new DataTable(resultTableName);
				resultTable = table.Clone();
				StringBuilder sb = new StringBuilder();
				string select = "";
				
				try
				{					
					foreach( string gate in gatesArray )
					{
						sb.Append(" A0GateID = " + gate.Trim() + " OR");
						sb.Append(" A1GateID = " + gate.Trim()+ " OR");
					}
					if ( gatesArray.Length > 0 )
					{
						select = sb.ToString(0, sb.ToString().Length - 2);
					}
					else
					{
						select = sb.ToString(0, sb.ToString().Length - 5);
					}
					table.CaseSensitive = false;
					DataRow[] rows = table.Select(select);

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

		private DataSet getCachedReaders()
		{
			if ((!cachedReadersTO.Tables.Contains(tableName)) || 
				((DataTable) cachedReadersTO.Tables[tableName]).Rows.Count == 0)
			{
				deserializeToCache();
			}
			return cachedReadersTO;
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

        public IDbTransaction getTransaction()
        {
            return null;
        }

        public void setTransaction(IDbTransaction trans)
        {
        }


        public void serialize(List<ReaderTO> ReadersTOList)
		{
			try
			{
				//string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLReadersFile"];
                string filename = Constants.XMLDataSourceDir + Constants.XMLReadersFile;
				Stream stream = File.Open(filename, FileMode.Create);

				ReaderTO[] readersTOArray = (ReaderTO[]) ReadersTOList.ToArray();

				XmlSerializer bformatter = new XmlSerializer(typeof(ReaderTO[]));
				bformatter.Serialize(stream, readersTOArray);
				stream.Close();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		#endregion

		private void deserializeToCache()
		{
            List<ReaderTO> readerList = new List<ReaderTO>();

			try
			{
				//if (File.Exists(Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLReadersFile"]))
                if (File.Exists(Constants.XMLDataSourceDir + Constants.XMLReadersFile))
				{
					//string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLReadersFile"];
                    string filename = Constants.XMLDataSourceDir + Constants.XMLReadersFile;
					Stream stream = File.Open(filename, FileMode.Open);

					XmlSerializer bformatter = new XmlSerializer(typeof(ReaderTO[]));
					ReaderTO[] deserialized = (ReaderTO[]) bformatter.Deserialize(stream);
					ArrayList readerArray = ArrayList.Adapter(deserialized);

                    foreach (ReaderTO reader in readerArray)
                    {
                        readerList.Add(reader);
                    }

					stream.Close();

					cachedReadersTO = toDataSet(readerList);
				}
				else
				{
					cachedReadersTO = new DataSet();
					DataTable dataTable = new DataTable();
					cachedReadersTO.Tables.Add(tableName);
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

        private DataSet toDataSet(List<ReaderTO> list)
		{
			DataSet dataset = new DataSet();

			try
			{
				DataTable dataTable = new DataTable();

				dataset.Tables.Add(tableName);
				dataTable = dataset.Tables[tableName];

				dataTable.Columns.Add("ReaderID", typeof(string));
				dataTable.Columns.Add("Description", typeof(string));
				dataTable.Columns.Add("ConnectionAddress", typeof(string));
				dataTable.Columns.Add("ConnectionType", typeof(string));

				dataTable.Columns.Add("A0GateID", typeof(string));
				dataTable.Columns.Add("A0LocID", typeof(string));
				dataTable.Columns.Add("A0Direction", typeof(string));
				dataTable.Columns.Add("A0IsCounter", typeof(string));
				dataTable.Columns.Add("A0SecLocID", typeof(string));
				dataTable.Columns.Add("A0SecDirection", typeof(string));

				dataTable.Columns.Add("A1GateID", typeof(string));
				dataTable.Columns.Add("A1LocID", typeof(string));
				dataTable.Columns.Add("A1Direction", typeof(string));
				dataTable.Columns.Add("A1IsCounter", typeof(string));
				dataTable.Columns.Add("A1SecLocID", typeof(string));
				dataTable.Columns.Add("A1SecDirection", typeof(string));

				dataTable.Columns.Add("TechType", typeof(string));
				dataTable.Columns.Add("Settings", typeof(string));

				dataTable.Columns.Add("DownloadStartTime", typeof(string));
				dataTable.Columns.Add("DownloadInterval", typeof(string));
				dataTable.Columns.Add("DownloadEraseCounter", typeof(string));

				foreach(ReaderTO rTO in list)
				{
					DataRow row = dataTable.NewRow();
			
					row["ReaderID"] = rTO.ReaderID.ToString();
					row["Description"] = rTO.Description;
					row["ConnectionAddress"] = rTO.ConnectionAddress;
					row["ConnectionType"] = rTO.ConnectionType;

					row["A0GateID"] = rTO.A0GateID.ToString();
					row["A0LocID"] = rTO.A0LocID.ToString();
					row["A0Direction"] = rTO.A0Direction;
					row["A0IsCounter"] = rTO.A0IsCounter.ToString();
					row["A0SecLocID"] = rTO.A0SecLocID.ToString();
					row["A0SecDirection"] = rTO.A0SecDirection;

					row["A1GateID"] = rTO.A1GateID.ToString();
					row["A1LocID"] = rTO.A1LocID.ToString();
					row["A1Direction"] = rTO.A1Direction;
					row["A1IsCounter"] = rTO.A1IsCounter.ToString();
					row["A1SecLocID"] = rTO.A1SecLocID.ToString();
					row["A1SecDirection"] = rTO.A1SecDirection;

					row["TechType"] = rTO.TechType;
					// TODO: Not implemented yet
					//row["Settings"] = "";
					row["DownloadStartTime"] = rTO.DownloadStartTime.ToString(dateTimeformat);;
					row["DownloadInterval"] = rTO.DownloadInterval;
					row["DownloadEraseCounter"] = rTO.DownloadEraseCounter;
					
					dataTable.Rows.Add(row);
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}

			return dataset;
		}


        private List<ReaderTO> dataTable2ArrayList(DataTable dataTable)
		{
            List<ReaderTO> readerList = new List<ReaderTO>();

			try
			{
				ReaderTO rTO = new ReaderTO();

				foreach(DataRow row in dataTable.Rows)
				{
					rTO = new ReaderTO();
					
					rTO.ReaderID = Int32.Parse(row["ReaderID"].ToString());
					rTO.Description = row["Description"].ToString();
					rTO.ConnectionAddress = row["ConnectionAddress"].ToString();
					rTO.ConnectionType = row["ConnectionType"].ToString();

					rTO.A0GateID = Int32.Parse(row["A0GateID"].ToString());
					rTO.A0LocID = Int32.Parse(row["A0LocID"].ToString());
					rTO.A0Direction = row["A0Direction"].ToString();
					rTO.A0IsCounter = Int32.Parse(row["A0IsCounter"].ToString());
					rTO.A0SecLocID = Int32.Parse(row["A0SecLocID"].ToString());
					rTO.A0SecDirection = row["A0SecDirection"].ToString();

					rTO.A1GateID = Int32.Parse(row["A1GateID"].ToString());
					rTO.A1LocID = Int32.Parse(row["A1LocID"].ToString());
					rTO.A1Direction = row["A1Direction"].ToString();
					rTO.A1IsCounter = Int32.Parse(row["A1IsCounter"].ToString());
					rTO.A1SecLocID = Int32.Parse(row["A1SecLocID"].ToString());
					rTO.A1SecDirection = row["A1SecDirection"].ToString();

					rTO.TechType = row["TechType"].ToString();

					rTO.DownloadStartTime = Convert.ToDateTime(row["DownloadStartTime"]);
					rTO.DownloadInterval = Int32.Parse(row["DownloadInterval"].ToString());
					rTO.DownloadEraseCounter = Int32.Parse(row["DownloadEraseCounter"].ToString());

					// TODO: Not implemented yet
					//rTO.Settings = null;

					readerList.Add(rTO);
				}

			}
			catch(Exception ex)
			{
				throw ex;
			}

			return readerList;
		}

		//not implemented yet
        public List<ReaderTO> getReadersLastReadTime()
		{
			return null;
		}

		//not implemented yet
		public DateTime getAllReadersLastReadTime()
		{
			return new DateTime(0);
		}

        //not implemented yet
        public List<ReaderTO> getReadersOnAntenna0()
        {
            return null;
        }
        //not implemented yet
        public List<ReaderTO> getAllReaders()
        {
            return null;
        }
        //not implemented yet
        public List<ReaderTO> getReaders(int locationID, int gateID)
        {
            return null;
        }
        //not implemented yet
        public List<ReaderTO> getReadersForMap(int mapID)
        {
            return null;
        }
	}
}
