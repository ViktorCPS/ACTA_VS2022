using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using TransferObjects;
using Util;

namespace DataAccess
{
	/// <summary>
	/// Summary description for MSSQLGateDAO.
	/// </summary>
	public class MSSQLGateDAO : GateDAO
	{
		SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

		public MSSQLGateDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MSSQLGateDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
		public int insert(string name, string description, DateTime downloadStartTime, int downloadInterval, int downloadEraseCounter)
		{
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
			int gateID = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("SET NOCOUNT ON ");
				sbInsert.Append("INSERT INTO gates ");
				sbInsert.Append("(name, description, download_start_time, download_interval, download_erase_counter, created_by, created_time) ");
				sbInsert.Append("VALUES (");
				
				if (name.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + name.Trim() + "', ");
				}
				if (description.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + description.Trim() + "', ");
				}
				if (downloadStartTime.Equals(new DateTime(0)))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("'" + downloadStartTime.ToString("yyy-MM-dd HH:mm").Trim() + "', ");
				}
				if (downloadInterval == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("'" + downloadInterval.ToString().Trim() + "', ");
				}
				if (downloadEraseCounter == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("'" + downloadEraseCounter.ToString().Trim() + "', ");
				}
				sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");
				sbInsert.Append("SELECT @@Identity AS gate_id ");
				sbInsert.Append("SET NOCOUNT OFF ");

				DataSet dataSet = new DataSet();
				SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans );

				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Gates");

				DataTable table = dataSet.Tables["Gates"];

				gateID = Int32.Parse(table.Rows[0]["gate_id"].ToString());

				sqlTrans.Commit();
				
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback("INSERT");
				
				throw ex;
			}

			return gateID;
		}

        public int insert(string name, string description, DateTime downloadStartTime, int downloadInterval, int downloadEraseCounter, bool doCommit)
        {
            SqlTransaction sqlTrans = null;

            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }
            int gateID = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("SET NOCOUNT ON ");
                sbInsert.Append("INSERT INTO gates ");
                sbInsert.Append("(name, description, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                if (name.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + name.Trim() + "', ");
                }
                if (description.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + description.Trim() + "', ");
                }               
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");
                sbInsert.Append("SELECT @@Identity AS gate_id ");
                sbInsert.Append("SET NOCOUNT OFF ");

                DataSet dataSet = new DataSet();
                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Gates");

                DataTable table = dataSet.Tables["Gates"];

                gateID = Int32.Parse(table.Rows[0]["gate_id"].ToString());
                if (doCommit)
                {
                    sqlTrans.Commit();
                }

            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback("INSERT");
                }

                throw ex;
            }

            return gateID;
        }


		public bool delete(int gateID)
		{
			bool isDeleted = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

			try
			{
				StringBuilder sbDelete = new StringBuilder();
				sbDelete.Append("DELETE FROM gates WHERE gate_id = '" + gateID.ToString().Trim() + "'");
				
				SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
				int res = cmd.ExecuteNonQuery();
				isDeleted = true;
				sqlTrans.Commit();
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback("DELETE");
				
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public bool beginTransaction()
		{
			bool isStarted = false;

			try
			{
				SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
				isStarted = true;
			}
			catch (Exception ex)
			{
				isStarted = false;
				throw new Exception(ex.Message);
			}

			return isStarted;
		}

		public void commitTransaction()
		{
			this.SqlTrans.Commit();
            this.SqlTrans = null;
		}

		public void rollbackTransaction()
		{
			this.SqlTrans.Rollback();
            this.SqlTrans = null;
		}

		public IDbTransaction getTransaction()
		{
			return _sqlTrans;
		}

		public void setTransaction(IDbTransaction trans)
		{
			_sqlTrans = (SqlTransaction)trans;
		}

		public GateTO find(int gateID)
		{
			DataSet dataSet = new DataSet();
			GateTO gateTO = new GateTO();
			try
			{
				SqlCommand cmd = new SqlCommand( "SELECT * FROM gates WHERE gate_id = '" + gateID.ToString().Trim() + "'", conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Gates");
				DataTable table = dataSet.Tables["Gates"];

				if (table.Rows.Count == 1)
				{
					gateTO = new GateTO();
					gateTO.GateID = Int32.Parse(table.Rows[0]["gate_id"].ToString().Trim());

					if (!table.Rows[0]["name"].Equals(DBNull.Value))
					{
						gateTO.Name = table.Rows[0]["name"].ToString().Trim();
					}
					if (!table.Rows[0]["description"].Equals(DBNull.Value))
					{
						gateTO.Description = table.Rows[0]["description"].ToString().Trim();
					}
					if (!table.Rows[0]["download_start_time"].Equals(DBNull.Value))
					{
						gateTO.DownloadStartTime = (DateTime) table.Rows[0]["download_start_time"];
					}
					if (!table.Rows[0]["download_interval"].Equals(DBNull.Value))
					{
						gateTO.DownloadInterval = Int32.Parse(table.Rows[0]["download_interval"].ToString().Trim());
					}
					if (!table.Rows[0]["download_erase_counter"].Equals(DBNull.Value))
					{
						gateTO.DownloadEraseCounter = Int32.Parse(table.Rows[0]["download_erase_counter"].ToString().Trim());
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return gateTO;
		}

        public List<GateTO> getGatesForMap(int mapID)
        {
            DataSet dataSet = new DataSet();
            GateTO gateTO = new GateTO();
            List<GateTO> gatesList = new List<GateTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM gates");

                sb.Append(" WHERE gate_id NOT IN (");
                sb.Append("SELECT object_id FROM maps_object_hdr WHERE type = 'GATE'");
                if (mapID != -1)
                {
                    sb.Append("AND map_id = " + mapID + " )");
                }
                else
                {
                    sb.Append(")");
                } 
                select = sb.ToString();

                select = select + "ORDER BY name ";
                SqlCommand cmd = new SqlCommand(select, conn);
               SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Gates");
                DataTable table = dataSet.Tables["Gates"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        gateTO = new GateTO();

                        gateTO.GateID = Int32.Parse(row["gate_id"].ToString().Trim());
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            gateTO.Name = row["name"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            gateTO.Description = row["description"].ToString().Trim();
                        }

                        gatesList.Add(gateTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return gatesList;
        }
        
		public bool update(int gateID, string name, string description, DateTime downloadStartTime, int downloadInterval, int downloadEraseCounter)
		{
			bool isUpdated = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

			try
			{
				StringBuilder sbUpdate = new StringBuilder();
				sbUpdate.Append("UPDATE gates SET ");
				if (!name.Trim().Equals(""))
				{
					sbUpdate.Append("name = N'" + name.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("name = null, ");
				}
				if (!description.Trim().Equals(""))
				{
					sbUpdate.Append("description = N'" + description.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("description = null, ");
				}
				if (!downloadStartTime.Equals(new DateTime(0)))
				{
					sbUpdate.Append("download_start_time = '" + downloadStartTime.ToString("yyy-MM-dd HH:mm").Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("download_start_time = null, ");
				}
				if (downloadInterval != -1)
				{
					sbUpdate.Append("download_interval = '" + downloadInterval.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("download_interval = null, ");
				}
				if (downloadEraseCounter != -1)
				{
					sbUpdate.Append("download_erase_counter = '" + downloadEraseCounter.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("download_erase_counter = null, ");
				}
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = GETDATE() ");
				sbUpdate.Append("WHERE gate_id = '" + gateID.ToString().Trim() + "'");
				
				SqlCommand cmd = new SqlCommand( sbUpdate.ToString(), conn, sqlTrans );
				int res = cmd.ExecuteNonQuery();
				if(res > 0)
				{
					isUpdated = true;	
				}
				sqlTrans.Commit();
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback("UPDATE");
				
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		// This method should be called with downloadStartTime converted to string in format "HH:mm:ss"!!!!!
        public List<GateTO> getGates(GateTO gTO)
		{
			DataSet dataSet = new DataSet();
			GateTO gateTO = new GateTO();
            List<GateTO> gatesList = new List<GateTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT * FROM gates ");
                sb.Append(" WHERE gate_id >= 0");
                if ((gTO.GateID != -1) || (!gTO.Name.Trim().Equals("")) ||
                    (!gTO.Description.Trim().Equals("")) || (!gTO.DownloadStartTime.Equals(new DateTime())) ||
                    (gTO.DownloadInterval != -1) || (gTO.DownloadEraseCounter != -1) || (!gTO.GateType.Trim().Equals("")))// 30.01.2020.    BOJAN dodato za GateType
				{
                    sb.Append(" AND");
                    if (gTO.GateID != -1)
					{
						sb.Append(" gate_id = '" + gTO.GateID.ToString().Trim() + "' AND");
					}
                    if (!gTO.Name.Trim().Equals(""))
					{
                        sb.Append(" UPPER(name) LIKE N'%" + gTO.Name.ToUpper().Trim() + "%' AND");
					}
                    if (!gTO.Description.Trim().Equals(""))
					{
                        sb.Append(" UPPER(description) LIKE N'%" + gTO.Description.ToUpper().Trim() + "%' AND");
					}
                    if (!gTO.DownloadStartTime.Equals(new DateTime()))
					{                        
                        sb.Append(" CONVERT(CHAR(24), download_start_time, 108) = '" + gTO.DownloadStartTime.ToString(dateTimeformat).Trim() + "' AND");
					}
                    if (gTO.DownloadInterval != -1)
					{
                        sb.Append(" download_interval = '" + gTO.DownloadInterval.ToString().Trim() + "' AND");
					}
                    if (gTO.DownloadEraseCounter != -1)
					{
                        sb.Append(" download_erase_counter = '" + gTO.DownloadEraseCounter.ToString().Trim() + "' AND");
					}
                    // 30.01.2020.    BOJAN dodato za GateType
                    if (!gTO.GateType.Trim().Equals(""))
                    {
                        sb.Append(" gate_type <> 'DEFAULT' AND");
                    }

					select = sb.ToString(0, sb.ToString().Length - 3);
				}
				else
				{
					select = sb.ToString();
				}
				
				select = select + "ORDER BY name ";

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Gates");
				DataTable table = dataSet.Tables["Gates"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						gateTO = new GateTO();
							
						gateTO.GateID = Int32.Parse(row["gate_id"].ToString().Trim());
						if (!row["name"].Equals(DBNull.Value))
						{
							gateTO.Name = row["name"].ToString().Trim();
						}
						if (!row["description"].Equals(DBNull.Value))
						{
							gateTO.Description = row["description"].ToString().Trim();
						}
						if (!row["download_start_time"].Equals(DBNull.Value))
						{
							gateTO.DownloadStartTime = (DateTime) row["download_start_time"];
						}
						if (!row["download_interval"].Equals(DBNull.Value))
						{
							gateTO.DownloadInterval = Int32.Parse(row["download_interval"].ToString().Trim());
						}
						if (!row["download_erase_counter"].Equals(DBNull.Value))
						{
							gateTO.DownloadEraseCounter = Int32.Parse(row["download_erase_counter"].ToString().Trim());
						}

						gatesList.Add(gateTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return gatesList;
		}

        public List<GateTO> getGatesForLocation(int locationID)
        {
            DataSet dataSet = new DataSet();
            GateTO gateTO = new GateTO();
            List<GateTO> gatesList = new List<GateTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT g.* FROM gates g, readers r");
               
                sb.Append(" WHERE (r.ant_0_location_id = "+ locationID+" AND");
                sb.Append(" g.gate_id = r.ant_0_gate_id) OR ");
                sb.Append("( r.ant_1_location_id = " + locationID + " AND ");
                sb.Append("g.gate_id = r.ant_1_gate_id )");
                   
                select = sb.ToString();               

                select = select + "ORDER BY name ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Gates");
                DataTable table = dataSet.Tables["Gates"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        gateTO = new GateTO();

                        gateTO.GateID = Int32.Parse(row["gate_id"].ToString().Trim());
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            gateTO.Name = row["name"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            gateTO.Description = row["description"].ToString().Trim();
                        }
                        if (!row["download_start_time"].Equals(DBNull.Value))
                        {
                            gateTO.DownloadStartTime = (DateTime)row["download_start_time"];
                        }
                        if (!row["download_interval"].Equals(DBNull.Value))
                        {
                            gateTO.DownloadInterval = Int32.Parse(row["download_interval"].ToString().Trim());
                        }
                        if (!row["download_erase_counter"].Equals(DBNull.Value))
                        {
                            gateTO.DownloadEraseCounter = Int32.Parse(row["download_erase_counter"].ToString().Trim());
                        }

                        gatesList.Add(gateTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return gatesList;
        }

        public List<GateTO> getGatesForLocationEnabled(int locationID)
        {
            DataSet dataSet = new DataSet();
            GateTO gateTO = new GateTO();
            List<GateTO> gatesList = new List<GateTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT g.* FROM gates g, readers r");

                sb.Append(" WHERE ");
                if (locationID != -1)
                {
                    sb.Append("(r.ant_0_location_id = " + locationID + " AND");
                    sb.Append(" g.gate_id = r.ant_0_gate_id) OR ");
                    sb.Append("( r.ant_1_location_id = " + locationID + " AND ");
                    sb.Append("g.gate_id = r.ant_1_gate_id ) AND ");
                }
                else
                {
                    sb.Append(" (g.gate_id = r.ant_0_gate_id OR ");
                    sb.Append(" g.gate_id = r.ant_1_gate_id) AND ");
                }
                sb.Append("r.status = 'ENABLED'");
                select = sb.ToString();

                select = select + "ORDER BY name ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Gates");
                DataTable table = dataSet.Tables["Gates"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        gateTO = new GateTO();

                        gateTO.GateID = Int32.Parse(row["gate_id"].ToString().Trim());
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            gateTO.Name = row["name"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            gateTO.Description = row["description"].ToString().Trim();
                        }
                        if (!row["download_start_time"].Equals(DBNull.Value))
                        {
                            gateTO.DownloadStartTime = (DateTime)row["download_start_time"];
                        }
                        if (!row["download_interval"].Equals(DBNull.Value))
                        {
                            gateTO.DownloadInterval = Int32.Parse(row["download_interval"].ToString().Trim());
                        }
                        if (!row["download_erase_counter"].Equals(DBNull.Value))
                        {
                            gateTO.DownloadEraseCounter = Int32.Parse(row["download_erase_counter"].ToString().Trim());
                        }

                        gatesList.Add(gateTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return gatesList;
        }

        public List<GateTO> getGatesGetGateTAProfile()
		{
			DataSet dataSet = new DataSet();
			GateTO gateTO = new GateTO();
            List<GateTO> gatesList = new List<GateTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT gates.gate_id, gates.name, gates.gate_timeaccess_profile_id, ");
				sb.Append("gate_timeaccess_profiles_hdr.name as ProfileName, gate_timeaccess_profiles_hdr.description ");
				sb.Append("FROM gates, gate_timeaccess_profiles_hdr ");
				sb.Append("WHERE gates.gate_timeaccess_profile_id = gate_timeaccess_profiles_hdr.gate_timeaccess_profile_id AND gate_id >= 0");

				select = sb.ToString() + "ORDER BY gates.name ";

				SqlCommand cmd = new SqlCommand(select, conn);
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Gates");
				DataTable table = dataSet.Tables["Gates"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						gateTO = new GateTO();
							
						gateTO.GateID = Int32.Parse(row["gate_id"].ToString().Trim());
						if (!row["name"].Equals(DBNull.Value))
						{
							gateTO.Name = row["name"].ToString().Trim();
						}
						if (!row["gate_timeaccess_profile_id"].Equals(DBNull.Value))
						{
							gateTO.GateTimeaccessProfileID = Int32.Parse(row["gate_timeaccess_profile_id"].ToString().Trim());
						}
						//put Profile name and Profile Description into Description property
						//delimiter is "~"
						if (!row["ProfileName"].Equals(DBNull.Value))
						{
							gateTO.Description = row["ProfileName"].ToString().Trim() + "~";
						}
						else gateTO.Description = "~";
						if (!row["description"].Equals(DBNull.Value))
						{
							gateTO.Description += row["description"].ToString().Trim();
						}

						gatesList.Add(gateTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}

			return gatesList;
		}

		public bool updateGateTAProfile(string  gateID, string gateTAProfileID, bool doCommit)
		{
			bool isUpdated = false;
			SqlTransaction sqlTrans = null;

			if(doCommit)
			{
				sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");
			}
			else
			{
				sqlTrans = this.SqlTrans;
			}

			try
			{
				StringBuilder sbUpdate = new StringBuilder();
				sbUpdate.Append("UPDATE gates SET ");
				sbUpdate.Append("gate_timeaccess_profile_id = " + gateTAProfileID.Trim() + ", ");
				
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = GETDATE() ");
				sbUpdate.Append("WHERE gate_id = '" + gateID.Trim() + "'");
				
				SqlCommand cmd = new SqlCommand( sbUpdate.ToString(), conn, sqlTrans );
				int res = cmd.ExecuteNonQuery();
				if(res > 0)
				{
					isUpdated = true;	
				}
				if (doCommit)
				{
					sqlTrans.Commit();
				}
			}
			catch(Exception ex)
			{
				if(doCommit)
					sqlTrans.Rollback("UPDATE");
				else
					sqlTrans.Rollback();
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public GateTO findGetAccessProfile(string gateID)
		{
			DataSet dataSet = new DataSet();
			GateTO gateTO = new GateTO();
			try
			{
				//SqlCommand cmd = new SqlCommand( "SELECT gate_id, name, description, gate_timeaccess_profile_id FROM gates WHERE gate_id = '" + gateID.Trim() + "'" , conn );
                SqlCommand cmd;
                if (this.SqlTrans == null)
                    cmd = new SqlCommand( "SELECT gate_id, name, description, gate_timeaccess_profile_id FROM gates WHERE gate_id = '" + gateID.Trim() + "'" , conn );
                else
                    cmd = new SqlCommand("SELECT gate_id, name, description, gate_timeaccess_profile_id FROM gates WHERE gate_id = '" + gateID.Trim() + "'", conn, this.SqlTrans);
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Gates");
				DataTable table = dataSet.Tables["Gates"];

				if (table.Rows.Count == 1)
				{
					gateTO = new GateTO();
					gateTO.GateID = Int32.Parse(table.Rows[0]["gate_id"].ToString().Trim());

					if (!table.Rows[0]["name"].Equals(DBNull.Value))
					{
						gateTO.Name = table.Rows[0]["name"].ToString().Trim();
					}
					if (!table.Rows[0]["description"].Equals(DBNull.Value))
					{
						gateTO.Description = table.Rows[0]["description"].ToString().Trim();
					}										
					if (!table.Rows[0]["gate_timeaccess_profile_id"].Equals(DBNull.Value))
					{
						gateTO.GateTimeaccessProfileID = Int32.Parse(table.Rows[0]["gate_timeaccess_profile_id"].ToString().Trim());
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return gateTO;
		}

        public List<GateTO> getGatesWithGateTAProfile(string gateTimeaccessProfileId)
		{
			DataSet dataSet = new DataSet();
			GateTO gateTO = new GateTO();
            List<GateTO> gatesList = new List<GateTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT gate_id, name, description, gate_timeaccess_profile_id FROM gates");

                sb.Append(" WHERE gate_id >= 0");
				if (!gateTimeaccessProfileId.Trim().Equals(""))
                {
                    sb.Append(" AND");
					sb.Append(" gate_timeaccess_profile_id = " + Int32.Parse(gateTimeaccessProfileId.Trim()));

					select = sb.ToString();										
				}
				else
				{
					select = sb.ToString();
				}
				select = select + " ORDER BY gates.name ";

				SqlCommand cmd = new SqlCommand(select, conn);
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Gates");
				DataTable table = dataSet.Tables["Gates"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						gateTO = new GateTO();
							
						gateTO.GateID = Int32.Parse(row["gate_id"].ToString().Trim());
						if (!row["name"].Equals(DBNull.Value))
						{
							gateTO.Name = row["name"].ToString().Trim();
						}
						if (!row["description"].Equals(DBNull.Value))
						{
							gateTO.Description = row["description"].ToString().Trim();
						}
						if (!row["gate_timeaccess_profile_id"].Equals(DBNull.Value))
						{
							gateTO.GateTimeaccessProfileID = Int32.Parse(row["gate_timeaccess_profile_id"].ToString().Trim());
						}

						gatesList.Add(gateTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}

			return gatesList;
		}

		// TODO!!!
        public void serialize(List<GateTO> GateTOList)
		{
			try
			{
				string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLGateFile"];
				Stream stream = File.Open(filename, FileMode.Create);

				GateTO[] gateTOArray = (GateTO[]) GateTOList.ToArray();

				XmlSerializer bformatter = new XmlSerializer(typeof(GateTO[]));
				bformatter.Serialize(stream, gateTOArray);
				stream.Close();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
	}
}
