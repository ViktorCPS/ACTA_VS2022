using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

using TransferObjects;
using Util;

namespace DataAccess
{
	/// <summary>
	/// Summary description for XMLLocationDAO.
	/// </summary>
	public class XMLLocationDAO : LocationDAO
	{
		private static DataSet cachedLocationTO = new DataSet();
		private const string tableName = "AllLocations";
		private const string resultTableName = "resultLocations";

		private DataSet getCachedLocations()
		{
			if ((!cachedLocationTO.Tables.Contains(tableName)) || 
				((DataTable) cachedLocationTO.Tables[tableName]).Rows.Count == 0)
			{
				deserializeToCache();
			}
			return cachedLocationTO;
		}

		public XMLLocationDAO()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		#region LocationDAO Members

		public int insert(LocationTO locTO)
		{
			// TODO:  Add XMLLocationDAO.insert implementation
			return 0;
		}

		public bool delete(int locationID)
		{
			// TODO:  Add XMLLocationDAO.delete implementation
			return false;
		}

		public bool update(LocationTO locTO)
		{
			// TODO:  Add XMLLocationDAO.update implementation
			return false;
		}

		public LocationTO find(int locationID)
		{
			// TODO:  Add XMLLocationDAO.find implementation
			return null;
		}

		// TODO!!!!!
		public int findMAXLocID()
		{
			return 0;
		}

        public Dictionary<int, LocationTO> getLocationsDict(LocationTO locTO)
        {
            return null;
        }

        public List<LocationTO> getLocations(LocationTO locTO)
		{
			DataSet dataSet = getCachedLocations();
			DataTable table = new DataTable();
			DataTable result = new DataTable(resultTableName);
			StringBuilder sb = new StringBuilder();
            List<LocationTO> locationList = new List<LocationTO>();
			string select = "";

			try
			{
				if (dataSet.Tables.Contains(tableName) && 
					((table = dataSet.Tables[tableName]).Rows.Count > 0))
				{
					result = table.Clone();

                    if ((locTO.LocationID != -1) || (!locTO.Name.Trim().Equals("")) ||
                    (!locTO.Description.Trim().Equals("")) || (locTO.ParentLocationID != -1)
                    || (locTO.AddressID != -1) || (!locTO.Status.ToString().Trim().Equals("")))
					{
						if (locTO.LocationID != -1)
						{
                            sb.Append(" LocationID like '" + locTO.LocationID.ToString().Trim() + "' AND");
						}
                        if (!locTO.Name.Trim().Equals(""))
						{
                            sb.Append(" Name like '" + locTO.Name.ToUpper().Trim() + "' AND");
						}
                        if (!locTO.Description.Trim().Equals(""))
						{
                            sb.Append(" Description like '" + locTO.Description.ToUpper().Trim() + "' AND");
						}
                        if (locTO.ParentLocationID != -1)
						{
                            sb.Append(" ParentLocationID like '" + locTO.ParentLocationID.ToString().Trim() + "' AND");
						}
                        if (locTO.AddressID != -1)
						{
                            sb.Append(" AddressID like '" + locTO.AddressID.ToString().Trim() + "' AND");
						}
                        if (!locTO.Status.ToString().Trim().Equals(""))
						{
                            sb.Append(" Status like '" + locTO.Status.ToUpper().Trim() + "' AND");
						}

						select = sb.ToString(0, sb.ToString().Length - 3);
						DataRow[] resultRows = table.Select(select);

						foreach(DataRow row in resultRows)
						{
							result.ImportRow(row);
						}
					}
					else
					{
						result = table;
					}

					locationList = dataTable2ArrayList(result);

				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			
			return locationList;
		}

        public List<LocationTO> getRootLocations()
        {
            return null;
        }

        public List<LocationTO> getChildLocations(string parentID)
        {
            return null;
        }

        public void serialize(List<LocationTO> LoactionTOList)
		{
			// TODO:  Add XMLLocationDAO.serialize implementation
		}

		#endregion


		private void deserializeToCache()
		{
            List<LocationTO> locationList = new List<LocationTO>();

			try
			{
				//if (File.Exists(Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLLocationFile"]))
                if (File.Exists(Constants.XMLDataSourceDir + Constants.XMLLocationFile))
				{
					//string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLLocationFile"];
                    string filename = Constants.XMLDataSourceDir + Constants.XMLLocationFile;
					Stream stream = File.Open(filename, FileMode.Open);

					XmlSerializer bformatter = new XmlSerializer(typeof(LocationTO[]));
					LocationTO[] deserialized = (LocationTO[]) bformatter.Deserialize(stream);
					ArrayList locationTOList = ArrayList.Adapter(deserialized);

                    foreach (LocationTO locTO in locationTOList)
                    {
                        locationList.Add(locTO);
                    }

					stream.Close();

					cachedLocationTO = toDataSet(locationList, tableName);
				}
				else
				{
					cachedLocationTO = new DataSet();
					DataTable dataTable = new DataTable();
					cachedLocationTO.Tables.Add(tableName);
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

        private DataSet toDataSet(List<LocationTO> list, string tblName)
		{
			DataSet dataSet = new DataSet();

			try
			{
				DataTable table = dataSet.Tables.Add(tblName);

				table.Columns.Add("LocationID", typeof(string));
				table.Columns.Add("Name", typeof(string));
				table.Columns.Add("Description", typeof(string));
				table.Columns.Add("ParentLocationID", typeof(string));
				table.Columns.Add("AddressID", typeof(string));
				table.Columns.Add("Status", typeof(string));

				foreach(LocationTO locTO in list)
				{
					DataRow row = table.NewRow();

					row["LocationID"] = locTO.LocationID.ToString();
					row["Name"] = locTO.Name;
					row["Description"] = locTO.Description;
					row["ParentLocationID"] = locTO.ParentLocationID.ToString();
					row["AddressID"] = locTO.AddressID.ToString();
					row["Status"] = locTO.Status;

					table.Rows.Add(row);
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			return dataSet;
		}

        private List<LocationTO> dataTable2ArrayList(DataTable dataTable)
		{
            List<LocationTO> locationTOList = new List<LocationTO>();

			try
			{
				LocationTO locTO = new LocationTO();

				foreach(DataRow row in dataTable.Rows)
				{
					locTO = new LocationTO();

					locTO.LocationID = Int32.Parse(row["LocationID"].ToString());
					locTO.Name = row["Name"].ToString();
						locTO.Description = row["Description"].ToString();
					locTO.ParentLocationID = Int32.Parse(row["ParentLocationID"].ToString());
					locTO.AddressID = Int32.Parse(row["AddressID"].ToString());
					locTO.Status = row["Status"].ToString();

					locationTOList.Add(locTO);
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}

			return locationTOList;
		}
        public DataSet getLocations(string locationID)
        {
            return null;
        }
        public List<LocationTO> getLocationsForMap(int mapID)
        {
            return null;
        }
	}
}
