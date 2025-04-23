using System;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;
using System.Data;
using System.Configuration;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using TransferObjects;
using Util;

namespace DataAccess
{
	/// <summary>
	/// Summary description for MySQLEmployeDAO.
	/// </summary>
	public class MySQLLocationDAO : LocationDAO
	{
		MySqlConnection conn = null;

		public MySQLLocationDAO()
		{
			conn = MySQLDAOFactory.getConnection();
		}

        public MySQLLocationDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
            DAOController.GetInstance();
        }

		public int insert(LocationTO locTO)
		{
			int rowsAffected = 0;
			MySqlTransaction trans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO locations ");
				sbInsert.Append("(location_id, name, description, parent_location_id, address_id, status, created_by, created_time) ");
				sbInsert.Append("VALUES (");
                sbInsert.Append("'" + locTO.LocationID.ToString().Trim() + "', ");
                if (locTO.Name.Trim().Equals(""))
                    sbInsert.Append("NULL, ");
                else
                    sbInsert.Append("N'" + locTO.Name.Trim() + "', ");
                if (locTO.Description.Trim().Equals(""))
                    sbInsert.Append("NULL, ");
                else
                    sbInsert.Append("N'" + locTO.Description.Trim() + "', ");
                sbInsert.Append("'" + locTO.ParentLocationID.ToString().Trim() + "', ");
                if (locTO.AddressID == -1)
                    sbInsert.Append("NULL, ");
                else
                    sbInsert.Append("'" + locTO.AddressID.ToString().Trim() + "', ");
                if (locTO.Status.Trim().Equals(""))
                    sbInsert.Append("NULL, ");
                else
                    sbInsert.Append("N'" + locTO.Status.Trim() + "', ");
                if (locTO.SegmentColor.Trim().Equals(""))
                    sbInsert.Append("NULL, ");
                else
                    sbInsert.Append("N'" + locTO.SegmentColor.Trim() + "', ");				
				sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', current_timestamp) ");
				
				MySqlCommand cmd = new MySqlCommand( sbInsert.ToString(), conn, trans);
				rowsAffected = cmd.ExecuteNonQuery();
				trans.Commit();				
			}
			catch(MySqlException ex)
			{
				trans.Rollback();
				if (ex.Number.Equals(1062))
				{
					throw new Exception("Location with a same ID already exist.");
				}
				else
					throw ex;
			}
			catch(Exception ex)
			{
				trans.Rollback();
				
				throw new Exception("Exception: " + ex.Message);
			}

			return rowsAffected;
		}

		public bool delete(int locationID)
		{
			bool isDeleted = false;
			MySqlTransaction trans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

			try
			{
				StringBuilder sbDelete = new StringBuilder();
				sbDelete.Append("DELETE FROM locations WHERE location_id = '" + locationID.ToString().Trim() + "'");
				
				MySqlCommand cmd = new MySqlCommand( sbDelete.ToString(), conn, trans);
				int res = cmd.ExecuteNonQuery();
				isDeleted = true;
				trans.Commit();
			}
			catch(Exception ex)
			{
				trans.Rollback();
				
				throw new Exception("Exception: " + ex.Message);
			}

			return isDeleted;
		}

		public LocationTO find(int locationID)
		{
			DataSet dataSet = new DataSet();
			LocationTO location = new LocationTO();
			try
			{
				MySqlCommand cmd = new MySqlCommand( "SELECT * FROM locations WHERE location_id = '" + locationID.ToString().Trim() + "'", conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Location");
				DataTable table = dataSet.Tables["Location"];

				if (table.Rows.Count == 1)
				{
					location = new LocationTO();
					location.LocationID = Int32.Parse(table.Rows[0]["location_id"].ToString().Trim());

					if (!table.Rows[0]["parent_location_id"].Equals(DBNull.Value))
					{
						location.ParentLocationID = Int32.Parse(table.Rows[0]["parent_location_id"].ToString().Trim());
					}
					if (!table.Rows[0]["name"].Equals(DBNull.Value))
					{
						location.Name = table.Rows[0]["name"].ToString().Trim();
					}
					if (!table.Rows[0]["description"].Equals(DBNull.Value))
					{
						location.Description = table.Rows[0]["description"].ToString().Trim();
					}
					if (!table.Rows[0]["address_id"].Equals(DBNull.Value))
					{
						location.AddressID = Int32.Parse(table.Rows[0]["address_id"].ToString().Trim());
					}	
					if (!table.Rows[0]["status"].Equals(DBNull.Value))
					{
						location.Status = table.Rows[0]["status"].ToString().Trim();
					}
                    if (!table.Rows[0]["segment_color"].Equals(DBNull.Value))
                    {
                        location.SegmentColor = table.Rows[0]["segment_color"].ToString().Trim();
                    }
				}
			}
			catch(Exception ex)
			{
				
				throw new Exception("Exception: " + ex.Message);
			}

			return location;
		}

		public int findMAXLocID()
		{
			DataSet dataSet = new DataSet();
			int locID = 0;
			try
			{
				MySqlCommand cmd = new MySqlCommand( "SELECT MAX(location_id) AS location_id FROM locations", conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Locations");
				DataTable table = dataSet.Tables["Locations"];

				if (table.Rows.Count == 1 && !table.Rows[0]["location_id"].Equals(DBNull.Value))
				{					
					locID = Int32.Parse(table.Rows[0]["location_id"].ToString().Trim());
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return locID;
		}

		public bool update(LocationTO locTO)
		{
			bool isUpdated = false;
			MySqlTransaction trans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

			try
			{
				StringBuilder sbUpdate = new StringBuilder();
				sbUpdate.Append("UPDATE locations SET ");
				if (!locTO.Name.Trim().Equals(""))				
					sbUpdate.Append("name = N'" + locTO.Name.Trim() + "', ");				
				else				
					sbUpdate.Append("name = NULL, ");				
				if (!locTO.Description.Trim().Equals(""))				
					sbUpdate.Append("description = N'" + locTO.Description.Trim() + "', ");				
				else				
					sbUpdate.Append("description = NULL, ");				
				if (locTO.ParentLocationID >= 0)				
					sbUpdate.Append("parent_location_id = '" + locTO.ParentLocationID.ToString().Trim() + "', ");
				if (!locTO.Status.Trim().Equals(""))				
					sbUpdate.Append("status = N'" + locTO.Status.Trim() + "', ");				
				else				
					sbUpdate.Append("status = NULL, ");
                if (!locTO.SegmentColor.Trim().Equals(""))
                    sbUpdate.Append("segment_color = N'" + locTO.SegmentColor.Trim() + "', ");
                else
                    sbUpdate.Append("segment_color = NULL, ");
				if (locTO.AddressID != -1)				
					sbUpdate.Append("address_id = '" + locTO.AddressID.ToString().Trim() + "', ");				
				else				
					sbUpdate.Append("address_id = NULL, ");				
				sbUpdate.Append("modified_by = '" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = current_timestamp ");
				sbUpdate.Append("WHERE location_id = " + locTO.LocationID.ToString().Trim());
				
				MySqlCommand cmd = new MySqlCommand( sbUpdate.ToString(), conn, trans);
				int res = cmd.ExecuteNonQuery();
				if(res > 0)
				{
					isUpdated = true;
				}
				trans.Commit();
			}
			catch(Exception ex)
			{
				trans.Rollback();
				
				throw new Exception("Exception: " + ex.Message);
			}

			return isUpdated;
		}

        public List<LocationTO> getLocations(LocationTO locTO)
		{
			DataSet dataSet = new DataSet();
			LocationTO location = new LocationTO();
            List<LocationTO> locationsList = new List<LocationTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT * FROM locations ");

                if ((locTO.LocationID != -1) || (!locTO.Name.Trim().Equals("")) ||
                    (!locTO.Description.Trim().Equals("")) || (locTO.ParentLocationID != -1)
                    || (locTO.AddressID != -1) || (!locTO.Status.ToString().Trim().Equals("")) || (locTO.SegmentColor.Trim() != ""))
				{
					sb.Append(" WHERE ");

                    if (locTO.LocationID != -1)
					{
                        sb.Append(" location_id = '" + locTO.LocationID.ToString().Trim() + "' AND");
					}
                    if (!locTO.Name.Trim().Equals(""))
					{
                        sb.Append(" UPPER(name) LIKE N'%" + locTO.Name.ToUpper().Trim() + "%' AND");
					}
                    if (!locTO.Description.Trim().Equals(""))
					{
                        sb.Append(" UPPER(description) LIKE N'%" + locTO.Description.ToUpper().Trim() + "%' AND");
					}
                    if (locTO.ParentLocationID != -1)
					{
                        sb.Append(" parent_location_id = '" + locTO.ParentLocationID.ToString().Trim() + "' AND");
					}
                    if (locTO.AddressID != -1)
					{
                        sb.Append(" address_id = '" + locTO.AddressID.ToString().Trim() + "' AND");
					}
                    if (!locTO.Status.Trim().Equals(""))
					{
                        sb.Append(" UPPER(status) LIKE N'%" + locTO.Status.ToUpper().Trim() + "%' AND");
					}
                    if (!locTO.SegmentColor.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(segment_color) LIKE N'%" + locTO.SegmentColor.ToUpper().Trim() + "%' AND");
                    }

					select = sb.ToString(0, sb.ToString().Length - 3);				
				}
				else
				{
					select = sb.ToString();
				}
				
				select = select + "ORDER BY name ";

				MySqlCommand cmd = new MySqlCommand(select, conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Location");
				DataTable table = dataSet.Tables["Location"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						location = new LocationTO();
						location.LocationID = Int32.Parse(row["location_id"].ToString().Trim());

						if (!row["name"].Equals(DBNull.Value))
						{
							location.Name =row["name"].ToString().Trim();
						}
						if (!row["description"].Equals(DBNull.Value))
						{
							location.Description = row["description"].ToString().Trim();
						}						
						if (!row["parent_location_id"].Equals(DBNull.Value))
						{
							location.ParentLocationID = Int32.Parse(row["parent_location_id"].ToString().Trim());
						}
						if (!row["address_id"].Equals(DBNull.Value))
						{
							location.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
						}
						if (!row["status"].Equals(DBNull.Value))
						{
							location.Status = row["status"].ToString().Trim();
						}
                        if (!row["segment_color"].Equals(DBNull.Value))
                        {
                            location.SegmentColor = row["segment_color"].ToString().Trim();
                        }

						locationsList.Add(location);
					}
				}
			}
			catch(Exception ex)
			{				
				throw new Exception("Exception: " + ex.Message);
			}
			return locationsList;
		}

        public Dictionary<int, LocationTO> getLocationsDict(LocationTO locTO)
        {
            DataSet dataSet = new DataSet();
            LocationTO location = new LocationTO();
            Dictionary<int, LocationTO> locationsDict = new Dictionary<int, LocationTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM locations ");

                if ((locTO.LocationID != -1) || (!locTO.Name.Trim().Equals("")) ||
                    (!locTO.Description.Trim().Equals("")) || (locTO.ParentLocationID != -1)
                    || (locTO.AddressID != -1) || (!locTO.Status.ToString().Trim().Equals("")) || (locTO.SegmentColor.Trim() != ""))
                {
                    sb.Append(" WHERE ");

                    if (locTO.LocationID != -1)
                    {
                        sb.Append(" location_id = '" + locTO.LocationID.ToString().Trim() + "' AND");
                    }
                    if (!locTO.Name.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(name) LIKE N'%" + locTO.Name.ToUpper().Trim() + "%' AND");
                    }
                    if (!locTO.Description.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(description) LIKE N'%" + locTO.Description.ToUpper().Trim() + "%' AND");
                    }
                    if (locTO.ParentLocationID != -1)
                    {
                        sb.Append(" parent_location_id = '" + locTO.ParentLocationID.ToString().Trim() + "' AND");
                    }
                    if (locTO.AddressID != -1)
                    {
                        sb.Append(" address_id = '" + locTO.AddressID.ToString().Trim() + "' AND");
                    }
                    if (!locTO.Status.ToString().Trim().Equals(""))
                    {
                        sb.Append(" UPPER(status) LIKE N'%" + locTO.Status.ToUpper().Trim() + "%' AND");
                    }
                    if (!locTO.SegmentColor.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(segment_color) LIKE N'%" + locTO.SegmentColor.ToUpper().Trim() + "%' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY name ";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Location");
                DataTable table = dataSet.Tables["Location"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        location = new LocationTO();
                        location.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            location.Name = row["name"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            location.Description = row["description"].ToString().Trim();
                        }
                        if (!row["parent_location_id"].Equals(DBNull.Value))
                        {
                            location.ParentLocationID = Int32.Parse(row["parent_location_id"].ToString().Trim());
                        }
                        if (!row["address_id"].Equals(DBNull.Value))
                        {
                            location.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            location.Status = row["status"].ToString().Trim();
                        }
                        if (!row["segment_color"].Equals(DBNull.Value))
                        {
                            location.SegmentColor = row["segment_color"].ToString().Trim();
                        }

                        if (!locationsDict.ContainsKey(location.LocationID))
                            locationsDict.Add(location.LocationID, location);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return locationsDict;
        }

        public List<LocationTO> getLocationsForMap(int mapID)
        {
            DataSet dataSet = new DataSet();
            LocationTO location = new LocationTO();
            List<LocationTO> locationsList = new List<LocationTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM locations ");
                sb.Append(" WHERE location_id NOT IN (");
                sb.Append(" SELECT object_id from maps_object_hdr WHERE type = 'LOCATION'");
                if (mapID != -1)
                    sb.Append(" AND map_id = " + mapID);
                sb.Append(" ) ");
                 
                select = sb.ToString();              

                select = select + "ORDER BY name ";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Location");
                DataTable table = dataSet.Tables["Location"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        location = new LocationTO();
                        location.LocationID = Int32.Parse(row["location_id"].ToString().Trim());

                        if (!row["name"].Equals(DBNull.Value))
                        {
                            location.Name = row["name"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            location.Description = row["description"].ToString().Trim();
                        }
                        if (!row["parent_location_id"].Equals(DBNull.Value))
                        {
                            location.ParentLocationID = Int32.Parse(row["parent_location_id"].ToString().Trim());
                        }
                        if (!row["address_id"].Equals(DBNull.Value))
                        {
                            location.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            location.Status = row["status"].ToString().Trim();
                        }
                        if (!row["segment_color"].Equals(DBNull.Value))
                        {
                            location.SegmentColor = row["segment_color"].ToString().Trim();
                        }

                        locationsList.Add(location);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Exception: " + ex.Message);
            }
            return locationsList;
        }

        public DataSet getLocations(string locationID)
        {
            DataSet dataSet = new DataSet();
            string select = "";
            try
            {
                StringBuilder sb = new StringBuilder();
                
                sb.Append("SELECT * FROM locations WHERE name IS NOT null AND status = 'ACTIVE' ");
                if (!locationID.Equals(""))
                {
                    sb.Append("AND location_id IN (" + locationID + ")");
                }
                select = sb.ToString();

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Locations");
                DataTable table = dataSet.Tables["Locations"];
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return dataSet;
        }

        public List<LocationTO> getRootLocations()
        {
            DataSet dataSet = new DataSet();
            LocationTO location = new LocationTO();
            List<LocationTO> locationsList = new List<LocationTO>();
            
            try
            {
                string select = "SELECT * FROM locations WHERE location_id = parent_location_id AND status = 'ACTIVE' ORDER BY name";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Location");
                DataTable table = dataSet.Tables["Location"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        location = new LocationTO();
                        location.LocationID = Int32.Parse(row["location_id"].ToString().Trim());

                        if (!row["name"].Equals(DBNull.Value))
                        {
                            location.Name = row["name"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            location.Description = row["description"].ToString().Trim();
                        }
                        if (!row["parent_location_id"].Equals(DBNull.Value))
                        {
                            location.ParentLocationID = Int32.Parse(row["parent_location_id"].ToString().Trim());
                        }
                        if (!row["address_id"].Equals(DBNull.Value))
                        {
                            location.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            location.Status = row["status"].ToString().Trim();
                        }
                        if (!row["segment_color"].Equals(DBNull.Value))
                        {
                            location.SegmentColor = row["segment_color"].ToString().Trim();
                        }

                        locationsList.Add(location);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return locationsList;
        }

        public List<LocationTO> getChildLocations(string parentID)
        {
            DataSet dataSet = new DataSet();
            LocationTO location = new LocationTO();
            List<LocationTO> locationsList = new List<LocationTO>();

            try
            {
                string select = "SELECT * FROM locations WHERE parent_location_id = '" + parentID.Trim() + 
                    "' AND location_id <> parent_location_id AND status = 'ACTIVE' ORDER BY name";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Location");
                DataTable table = dataSet.Tables["Location"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        location = new LocationTO();
                        location.LocationID = Int32.Parse(row["location_id"].ToString().Trim());

                        if (!row["name"].Equals(DBNull.Value))
                        {
                            location.Name = row["name"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            location.Description = row["description"].ToString().Trim();
                        }
                        if (!row["parent_location_id"].Equals(DBNull.Value))
                        {
                            location.ParentLocationID = Int32.Parse(row["parent_location_id"].ToString().Trim());
                        }
                        if (!row["address_id"].Equals(DBNull.Value))
                        {
                            location.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            location.Status = row["status"].ToString().Trim();
                        }
                        if (!row["segment_color"].Equals(DBNull.Value))
                        {
                            location.SegmentColor = row["segment_color"].ToString().Trim();
                        }

                        locationsList.Add(location);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return locationsList;
        }

        public void serialize(List<LocationTO> LocationTOList)
		{
			try
			{
				//string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLLocationFile"];
                string filename = Constants.XMLDataSourceDir + Constants.XMLLocationFile;
				Stream stream = File.Open(filename, FileMode.Create);

				LocationTO[] locationTOArray = (LocationTO[]) LocationTOList.ToArray();

				XmlSerializer bformatter = new XmlSerializer(typeof(LocationTO[]));
				bformatter.Serialize(stream, locationTOArray);
				stream.Close();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
	}
}

