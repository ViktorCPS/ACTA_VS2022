using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;


using System.Xml;
using System.Xml.Serialization;
using System.IO;

using TransferObjects;
using Util;

namespace DataAccess
{
	/// <summary>
	/// Provide MSSQL Readers's data access.
	/// </summary>
	public class MSSQLReaderDAO : ReaderDAO
	{
		SqlConnection conn = null;
        private static List<ReaderTO> cachedReadersTO = new List<ReaderTO>();
		protected string dateTimeformat = "";
        SqlTransaction _sqlTrans = null;

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }
		
		public MSSQLReaderDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MSSQLReaderDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public List<ReaderTO> getCachedReaders()
		{
			// TODO:  Add MSSQLReaderDAO.getCachedReaders implementation
			return cachedReadersTO;
		}

        public List<ReaderTO> searchForIDs(string readerID)
        {
            ReaderTO readerTO = new ReaderTO();
            DataSet dataSet = new DataSet();
            ReaderTO readTO = new ReaderTO();
            List<ReaderTO> readerList = new List<ReaderTO>();

            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM readers WHERE reader_id IN (" + readerID.Trim() + ")", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Reader");
                DataTable table = dataSet.Tables["Reader"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        readTO = new ReaderTO();

                        if (row["reader_id"] != DBNull.Value)
                        {
                            readTO.ReaderID = (int)row["reader_id"];
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            readTO.Description = row["description"].ToString().Trim();
                        }
                        if (row["conn_type"] != DBNull.Value)
                        {
                            readTO.ConnectionType = row["conn_type"].ToString().Trim();
                        }
                        if (row["conn_address"] != DBNull.Value)
                        {
                            readTO.ConnectionAddress = row["conn_address"].ToString().Trim();
                        }
                        if (row["ant_0_gate_id"] != DBNull.Value)
                        {
                            readTO.A0GateID = (int)row["ant_0_gate_id"];
                        }
                        if (row["ant_0_location_id"] != DBNull.Value)
                        {
                            readTO.A0LocID = (int)row["ant_0_location_id"];
                        }
                        if (row["ant_0_location_dir"] != DBNull.Value)
                        {
                            readTO.A0Direction = row["ant_0_location_dir"].ToString().Trim();
                        }
                        if (row["ant_0_sec_location_id"] != DBNull.Value)
                        {
                            readTO.A0SecLocID = (int)row["ant_0_sec_location_id"];
                        }
                        if (row["ant_0_sec_location_dir"] != DBNull.Value)
                        {
                            readTO.A0SecDirection = row["ant_0_sec_location_dir"].ToString().Trim();
                        }
                        if (row["ant_0_is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            readTO.A0IsCounter = (int)row["ant_0_is_wrk_hrs_counter"];
                        }
                        if (row["ant_1_gate_id"] != DBNull.Value)
                        {
                            readTO.A1GateID = (int)row["ant_1_gate_id"];
                        }
                        if (row["ant_1_location_id"] != DBNull.Value)
                        {
                            readTO.A1LocID = (int)row["ant_1_location_id"];
                        }
                        if (row["ant_1_location_dir"] != DBNull.Value)
                        {
                            readTO.A1Direction = row["ant_1_location_dir"].ToString().Trim();
                        }
                        if (row["ant_1_sec_location_id"] != DBNull.Value)
                        {
                            readTO.A1SecLocID = (int)row["ant_1_sec_location_id"];
                        }
                        if (row["ant_1_sec_location_dir"] != DBNull.Value)
                        {
                            readTO.A1SecDirection = row["ant_1_sec_location_dir"].ToString().Trim();
                        }
                        if (row["ant_1_is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            readTO.A1IsCounter = (int)row["ant_1_is_wrk_hrs_counter"];
                        }
                        if (row["technology_type"] != DBNull.Value)
                        {
                            readTO.TechType = row["technology_type"].ToString().Trim();
                        }
                        /*
                        if (row["settings"] != DBNull.Value)
                        {
                            readTO.Settings = (byte) row["settings"];
                        }
                        */

                        if (row["status"] != DBNull.Value)
                        {
                            readTO.Status = row["status"].ToString().Trim();
                        }

                        readerList.Add(readTO);
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Exception: " + ex.Message);
            }

            return readerList;
        }

		public int insert(ReaderTO readerData)
		{
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
			int rowsAffected = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO readers  ");
				sbInsert.Append("(reader_id, description, conn_type, conn_address, ");
				sbInsert.Append("ant_0_gate_id, ant_0_location_id, ant_0_location_dir, ant_0_sec_location_id, ant_0_sec_location_dir, ant_0_is_wrk_hrs_counter, ");
				sbInsert.Append("ant_1_gate_id, ant_1_location_id, ant_1_location_dir, ant_1_sec_location_id, ant_1_sec_location_dir, ant_1_is_wrk_hrs_counter, ");
				sbInsert.Append("technology_type, settings, status, ");
				sbInsert.Append("created_by, created_time) ");
				sbInsert.Append("VALUES ( ");
				sbInsert.Append("'" + readerData.ReaderID.ToString() + "', N'" + readerData.Description + "', N'" + readerData.ConnectionType + "', N'" + readerData.ConnectionAddress + "', ");

				// Antenna 0
				if (readerData.A0GateID != -1)
				{
					sbInsert.Append("'" + readerData.A0GateID.ToString() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (readerData.A0LocID != -1)
				{
					sbInsert.Append("'" + readerData.A0LocID.ToString() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}

				sbInsert.Append("N'" + readerData.A0Direction + "', ");

				if (readerData.A0SecLocID != -1)
				{
					sbInsert.Append("'" + readerData.A0SecLocID.ToString() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}

				if (!readerData.A0SecDirection.Trim().Equals(""))
				{
					sbInsert.Append("N'" + readerData.A0SecDirection + "', ");
				}
				else
				{
					sbInsert.Append(" NULL, ");
				}

				sbInsert.Append("'" + readerData.A0IsCounter.ToString() + "', ");
				//sbInsert.Append(readerData.A1LocID + ", ");
				//sbInsert.Append("N'" + readerData.A1Direction + "', ");

				// Antenna 1
				if (readerData.A1GateID != -1)
				{
					sbInsert.Append(readerData.A1GateID.ToString() + ", ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (readerData.A1LocID != -1)
				{
					sbInsert.Append(readerData.A1LocID.ToString() + ", ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}

				sbInsert.Append("N'" + readerData.A1Direction + "', ");

				if (readerData.A1SecLocID != -1)
				{
					sbInsert.Append("'" + readerData.A1SecLocID.ToString() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}

				if (!readerData.A1SecDirection.Trim().Equals(""))
				{
					sbInsert.Append("N'" + readerData.A1SecDirection + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}

				sbInsert.Append("'" + readerData.A1IsCounter.ToString() + "', ");
				sbInsert.Append("N'" + readerData.TechType + "', NULL, ");

				sbInsert.Append("N'" + readerData.Status + "', ");

				sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE())");

				SqlCommand cmd = new SqlCommand( sbInsert.ToString(), conn, sqlTrans );
				rowsAffected = cmd.ExecuteNonQuery();
				sqlTrans.Commit();
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback("INSERT");
				throw ex;
			}

			return rowsAffected;
		}


        public int insert(ReaderTO readerData,bool doCommit)
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
            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO readers  ");
                sbInsert.Append("(reader_id, description, conn_type, conn_address, ");
                sbInsert.Append("ant_0_gate_id, ant_0_location_id, ant_0_location_dir, ant_0_sec_location_id, ant_0_sec_location_dir, ant_0_is_wrk_hrs_counter, ");
                sbInsert.Append("ant_1_gate_id, ant_1_location_id, ant_1_location_dir, ant_1_sec_location_id, ant_1_sec_location_dir, ant_1_is_wrk_hrs_counter, ");
                sbInsert.Append("technology_type, settings, status, ");
                sbInsert.Append("created_by, created_time) ");
                sbInsert.Append("VALUES ( ");
                sbInsert.Append("'" + readerData.ReaderID.ToString() + "', N'" + readerData.Description + "', N'" + readerData.ConnectionType + "', N'" + readerData.ConnectionAddress + "', ");

                // Antenna 0
                if (readerData.A0GateID != -1)
                {
                    sbInsert.Append("'" + readerData.A0GateID.ToString() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (readerData.A0LocID != -1)
                {
                    sbInsert.Append("'" + readerData.A0LocID.ToString() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append("N'" + readerData.A0Direction + "', ");

                if (readerData.A0SecLocID != -1)
                {
                    sbInsert.Append("'" + readerData.A0SecLocID.ToString() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (!readerData.A0SecDirection.Trim().Equals(""))
                {
                    sbInsert.Append("N'" + readerData.A0SecDirection + "', ");
                }
                else
                {
                    sbInsert.Append(" NULL, ");
                }

                sbInsert.Append("'" + readerData.A0IsCounter.ToString() + "', ");
                //sbInsert.Append(readerData.A1LocID + ", ");
                //sbInsert.Append("N'" + readerData.A1Direction + "', ");

                // Antenna 1
                if (readerData.A1GateID != -1)
                {
                    sbInsert.Append(readerData.A1GateID.ToString() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (readerData.A1LocID != -1)
                {
                    sbInsert.Append(readerData.A1LocID.ToString() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append("N'" + readerData.A1Direction + "', ");

                if (readerData.A1SecLocID != -1)
                {
                    sbInsert.Append("'" + readerData.A1SecLocID.ToString() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (!readerData.A1SecDirection.Trim().Equals(""))
                {
                    sbInsert.Append("N'" + readerData.A1SecDirection + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append("'" + readerData.A1IsCounter.ToString() + "', ");
                sbInsert.Append("N'" + readerData.TechType + "', NULL, ");

                sbInsert.Append("N'" + readerData.Status + "', ");

                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE())");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();
                if(doCommit)
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if(doCommit)
                sqlTrans.Rollback("INSERT");
                throw ex;
            }

            return rowsAffected;
        }

		public bool delete(int readerID)
		{
			bool isDeleted = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

			try
			{
				StringBuilder sbDelete = new StringBuilder();
				sbDelete.Append("DELETE FROM readers WHERE reader_id = " + readerID + "; ");
				
				SqlCommand cmd = new SqlCommand( sbDelete.ToString(), conn, sqlTrans);
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

        public List<ReaderTO> getReaders(int locationID, int gateID)
        {
            DataSet dataSet = new DataSet();
            List<ReaderTO> readerList = new List<ReaderTO>();
            string select = "";
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" SELECT * FROM readers ");
                if ((locationID > 0) || (gateID >= 0))
                {
                    sb.Append("WHERE ");
                    if (locationID > 0)
                    {
                        sb.Append("( ant_0_location_id = " + locationID + " ) OR ");
                        sb.Append("( ant_1_location_id = " + locationID + " ) AND ");
                    }
                    if (gateID >= 0)
                    {
                        sb.Append("( ant_0_gate_id = " + gateID + " ) OR ");
                        sb.Append("( ant_1_gate_id = " + gateID + " ) AND ");
                    }
                    select = sb.ToString(0, sb.Length - 4);
                }
                else
                {
                    select = sb.ToString();
                }
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Reader");
                DataTable table = dataSet.Tables["Reader"];

                ReaderTO readTO = new ReaderTO();

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        readTO = new ReaderTO();

                        if (row["reader_id"] != DBNull.Value)
                        {
                            readTO.ReaderID = (int)row["reader_id"];
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            readTO.Description = row["description"].ToString().Trim();
                        }
                        if (row["conn_type"] != DBNull.Value)
                        {
                            readTO.ConnectionType = row["conn_type"].ToString().Trim();
                        }
                        if (row["conn_address"] != DBNull.Value)
                        {
                            readTO.ConnectionAddress = row["conn_address"].ToString().Trim();
                        }
                        if (row["ant_0_gate_id"] != DBNull.Value)
                        {
                            readTO.A0GateID = (int)row["ant_0_gate_id"];
                        }
                        if (row["ant_0_location_id"] != DBNull.Value)
                        {
                            readTO.A0LocID = (int)row["ant_0_location_id"];
                        }
                        if (row["ant_0_location_dir"] != DBNull.Value)
                        {
                            readTO.A0Direction = row["ant_0_location_dir"].ToString().Trim();
                        }
                        if (row["ant_0_sec_location_id"] != DBNull.Value)
                        {
                            readTO.A0SecLocID = (int)row["ant_0_sec_location_id"];
                        }
                        if (row["ant_0_sec_location_dir"] != DBNull.Value)
                        {
                            readTO.A0SecDirection = row["ant_0_sec_location_dir"].ToString().Trim();
                        }
                        if (row["ant_0_is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            readTO.A0IsCounter = (int)row["ant_0_is_wrk_hrs_counter"];
                        }
                        if (row["ant_1_gate_id"] != DBNull.Value)
                        {
                            readTO.A1GateID = (int)row["ant_1_gate_id"];
                        }
                        if (row["ant_1_location_id"] != DBNull.Value)
                        {
                            readTO.A1LocID = (int)row["ant_1_location_id"];
                        }
                        if (row["ant_1_location_dir"] != DBNull.Value)
                        {
                            readTO.A1Direction = row["ant_1_location_dir"].ToString().Trim();
                        }
                        if (row["ant_1_sec_location_id"] != DBNull.Value)
                        {
                            readTO.A1SecLocID = (int)row["ant_1_sec_location_id"];
                        }
                        if (row["ant_1_sec_location_dir"] != DBNull.Value)
                        {
                            readTO.A1SecDirection = row["ant_1_sec_location_dir"].ToString().Trim();
                        }
                        if (row["ant_1_is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            readTO.A1IsCounter = (int)row["ant_1_is_wrk_hrs_counter"];
                        }
                        if (row["technology_type"] != DBNull.Value)
                        {
                            readTO.TechType = row["technology_type"].ToString().Trim();
                        }
                      
                        readerList.Add(readTO);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return readerList;
        }

        public List<ReaderTO> getReadersForMap(int mapID)
        {
            DataSet dataSet = new DataSet();
            List<ReaderTO> readerList = new List<ReaderTO>();
            string select = "";
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" SELECT * FROM readers ");
                sb.Append("WHERE reader_id NOT IN (");
                sb.Append("SELECT object_id FROM maps_object_hdr WHERE type = 'READER'");
                if (mapID != -1)
                {
                    sb.Append("AND map_id = " + mapID + " )");
                }
                else
                {
                    sb.Append(")");
                }
                sb.Append(" ORDER BY description");
                select = sb.ToString();
              
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Reader");
                DataTable table = dataSet.Tables["Reader"];

                ReaderTO readTO = new ReaderTO();

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        readTO = new ReaderTO();

                        if (row["reader_id"] != DBNull.Value)
                        {
                            readTO.ReaderID = (int)row["reader_id"];
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            readTO.Description = row["description"].ToString().Trim();
                        }
                        if (row["conn_type"] != DBNull.Value)
                        {
                            readTO.ConnectionType = row["conn_type"].ToString().Trim();
                        }
                        if (row["conn_address"] != DBNull.Value)
                        {
                            readTO.ConnectionAddress = row["conn_address"].ToString().Trim();
                        }
                        if (row["ant_0_gate_id"] != DBNull.Value)
                        {
                            readTO.A0GateID = (int)row["ant_0_gate_id"];
                        }
                        if (row["ant_0_location_id"] != DBNull.Value)
                        {
                            readTO.A0LocID = (int)row["ant_0_location_id"];
                        }
                        if (row["ant_0_location_dir"] != DBNull.Value)
                        {
                            readTO.A0Direction = row["ant_0_location_dir"].ToString().Trim();
                        }
                        if (row["ant_0_sec_location_id"] != DBNull.Value)
                        {
                            readTO.A0SecLocID = (int)row["ant_0_sec_location_id"];
                        }
                        if (row["ant_0_sec_location_dir"] != DBNull.Value)
                        {
                            readTO.A0SecDirection = row["ant_0_sec_location_dir"].ToString().Trim();
                        }
                        if (row["ant_0_is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            readTO.A0IsCounter = (int)row["ant_0_is_wrk_hrs_counter"];
                        }
                        if (row["ant_1_gate_id"] != DBNull.Value)
                        {
                            readTO.A1GateID = (int)row["ant_1_gate_id"];
                        }
                        if (row["ant_1_location_id"] != DBNull.Value)
                        {
                            readTO.A1LocID = (int)row["ant_1_location_id"];
                        }
                        if (row["ant_1_location_dir"] != DBNull.Value)
                        {
                            readTO.A1Direction = row["ant_1_location_dir"].ToString().Trim();
                        }
                        if (row["ant_1_sec_location_id"] != DBNull.Value)
                        {
                            readTO.A1SecLocID = (int)row["ant_1_sec_location_id"];
                        }
                        if (row["ant_1_sec_location_dir"] != DBNull.Value)
                        {
                            readTO.A1SecDirection = row["ant_1_sec_location_dir"].ToString().Trim();
                        }
                        if (row["ant_1_is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            readTO.A1IsCounter = (int)row["ant_1_is_wrk_hrs_counter"];
                        }
                        if (row["technology_type"] != DBNull.Value)
                        {
                            readTO.TechType = row["technology_type"].ToString().Trim();
                        }

                        readerList.Add(readTO);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return readerList;
        }

		/// <summary>
		/// Update Reader record getting values from Transfer Object
		/// </summary>
		/// <param name="readerData"></param>
		/// <returns></returns>
		public bool update(ReaderTO readerData)
		{
			bool isUpdated = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

			try
			{
				StringBuilder sbUpdate = new StringBuilder();

				sbUpdate.Append("UPDATE readers SET ");
				sbUpdate.Append("description = N'" + readerData.Description + "', ");
				sbUpdate.Append("conn_type = N'" + readerData.ConnectionType + "', ");
				sbUpdate.Append("conn_address = N'" + readerData.ConnectionAddress  + "', ");
				sbUpdate.Append("ant_0_gate_id = " + readerData.A0GateID + ", ");
				sbUpdate.Append("ant_0_location_id = " + readerData.A0LocID + ", ");
				sbUpdate.Append("ant_0_location_dir = N'" + readerData.A0Direction + "', ");

				if (readerData.A0SecLocID != -1)
				{
					sbUpdate.Append("ant_0_sec_location_id = " + readerData.A0SecLocID + ", ");
				}
				else
				{
					sbUpdate.Append("ant_0_sec_location_id = NULL, ");
				}

				if (!readerData.A0SecDirection.Trim().Equals(""))
				{
					sbUpdate.Append("ant_0_sec_location_dir = N'" + readerData.A0SecDirection + "', ");
				}
				else
				{
					sbUpdate.Append("ant_0_sec_location_dir = NULL, ");
				}

				sbUpdate.Append("ant_0_is_wrk_hrs_counter = " + readerData.A0IsCounter + ", ");
				sbUpdate.Append("ant_1_gate_id = " + readerData.A1GateID + ", ");
				sbUpdate.Append("ant_1_location_id = " + readerData.A1LocID + ", ");
				sbUpdate.Append("ant_1_location_dir = N'" + readerData.A1Direction + "', ");

				if (readerData.A1SecLocID != -1)
				{
					sbUpdate.Append("ant_1_sec_location_id = " + readerData.A1SecLocID + ", ");
				}
				else
				{
					sbUpdate.Append("ant_1_sec_location_id = NULL, ");
				}

				if (!readerData.A1SecDirection.Trim().Equals(""))
				{
					sbUpdate.Append("ant_1_sec_location_dir = N'" + readerData.A1SecDirection + "', ");
				}
				else
				{
					sbUpdate.Append("ant_1_sec_location_dir = NULL, ");
				}

				sbUpdate.Append("ant_1_is_wrk_hrs_counter = " + readerData.A1IsCounter + ", ");
				sbUpdate.Append("technology_type = N'" + readerData.TechType + "', ");
				sbUpdate.Append("settings = NULL, ");

				sbUpdate.Append("status = N'" + readerData.Status + "', ");
				
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = GETDATE() ");
				sbUpdate.Append("WHERE reader_id = '" + readerData.ReaderID + "'");

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

        /// <summary>
        /// Update Reader record getting values from Transfer Object
        /// </summary>
        /// <param name="readerData"></param>
        /// <returns></returns>
        public bool update(ReaderTO readerData,bool doCommit)
        {
            bool isUpdated = false;
            SqlTransaction sqlTrans = null;

            if (doCommit)
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

                sbUpdate.Append("UPDATE readers SET ");
                sbUpdate.Append("description = N'" + readerData.Description + "', ");
                sbUpdate.Append("conn_type = N'" + readerData.ConnectionType + "', ");
                sbUpdate.Append("conn_address = N'" + readerData.ConnectionAddress + "', ");
                sbUpdate.Append("ant_0_gate_id = " + readerData.A0GateID + ", ");
                sbUpdate.Append("ant_0_location_id = " + readerData.A0LocID + ", ");
                sbUpdate.Append("ant_0_location_dir = N'" + readerData.A0Direction + "', ");

                if (readerData.A0SecLocID != -1)
                {
                    sbUpdate.Append("ant_0_sec_location_id = " + readerData.A0SecLocID + ", ");
                }
                else
                {
                    sbUpdate.Append("ant_0_sec_location_id = NULL, ");
                }

                if (!readerData.A0SecDirection.Trim().Equals(""))
                {
                    sbUpdate.Append("ant_0_sec_location_dir = N'" + readerData.A0SecDirection + "', ");
                }
                else
                {
                    sbUpdate.Append("ant_0_sec_location_dir = NULL, ");
                }

                sbUpdate.Append("ant_0_is_wrk_hrs_counter = " + readerData.A0IsCounter + ", ");
                sbUpdate.Append("ant_1_gate_id = " + readerData.A1GateID + ", ");
                sbUpdate.Append("ant_1_location_id = " + readerData.A1LocID + ", ");
                sbUpdate.Append("ant_1_location_dir = N'" + readerData.A1Direction + "', ");

                if (readerData.A1SecLocID != -1)
                {
                    sbUpdate.Append("ant_1_sec_location_id = " + readerData.A1SecLocID + ", ");
                }
                else
                {
                    sbUpdate.Append("ant_1_sec_location_id = NULL, ");
                }

                if (!readerData.A1SecDirection.Trim().Equals(""))
                {
                    sbUpdate.Append("ant_1_sec_location_dir = N'" + readerData.A1SecDirection + "', ");
                }
                else
                {
                    sbUpdate.Append("ant_1_sec_location_dir = NULL, ");
                }

                sbUpdate.Append("ant_1_is_wrk_hrs_counter = " + readerData.A1IsCounter + ", ");
                sbUpdate.Append("technology_type = N'" + readerData.TechType + "', ");
                sbUpdate.Append("settings = NULL, ");

                sbUpdate.Append("status = N'" + readerData.Status + "', ");

                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE reader_id = '" + readerData.ReaderID + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                if (doCommit)
                {
                    sqlTrans.Commit();
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback("UPDATE");
                }
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

		public ReaderTO find(string readerID)
		{
			ReaderTO readerTO = new ReaderTO();
			DataSet dataSet = new DataSet();

			try
			{
				SqlCommand cmd = new SqlCommand( "SELECT * FROM readers WHERE reader_id = '" + readerID.Trim() + "'", conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Reader");
				DataTable table = dataSet.Tables["Reader"];

				if (table.Rows.Count == 1)
				{
					readerTO.ReaderID = Int32.Parse(table.Rows[0]["reader_id"].ToString().Trim());

					if (table.Rows[0]["description"] != DBNull.Value)
					{
						readerTO.Description = table.Rows[0]["description"].ToString().Trim();
					}
					if (table.Rows[0]["conn_type"] != DBNull.Value)
					{
						readerTO.ConnectionType = table.Rows[0]["conn_type"].ToString().Trim();
					}

					if (table.Rows[0]["conn_address"] != DBNull.Value)
					{
						readerTO.ConnectionAddress = table.Rows[0]["conn_address"].ToString().Trim();
					}

					if (table.Rows[0]["ant_0_gate_id"] != DBNull.Value)
					{
						readerTO.A0GateID = (int) table.Rows[0]["ant_0_gate_id"];
					}

					if (table.Rows[0]["ant_0_location_id"] != DBNull.Value)
					{
						readerTO.A0LocID = (int) table.Rows[0]["ant_0_location_id"];
					}

					if (table.Rows[0]["ant_0_location_dir"] != DBNull.Value)
					{
						readerTO.A0Direction = table.Rows[0]["ant_0_location_dir"].ToString().Trim();
					}

					if (table.Rows[0]["ant_0_sec_location_id"] != DBNull.Value)
					{
						readerTO.A0SecLocID = (int) table.Rows[0]["ant_0_sec_location_id"];
					}

					if (table.Rows[0]["ant_0_sec_location_dir"] != DBNull.Value)
					{
						readerTO.A0SecDirection = table.Rows[0]["ant_0_sec_location_dir"].ToString().Trim();
					}

					if (table.Rows[0]["ant_0_is_wrk_hrs_counter"] != DBNull.Value)
					{
						readerTO.A0IsCounter = (int) table.Rows[0]["ant_0_is_wrk_hrs_counter"];
					}

					if (table.Rows[0]["ant_1_gate_id"] != DBNull.Value)
					{
						readerTO.A1GateID = (int) table.Rows[0]["ant_1_gate_id"];
					}

					if (table.Rows[0]["ant_1_location_id"] != DBNull.Value)
					{
						readerTO.A1LocID = (int) table.Rows[0]["ant_1_location_id"];
					}

					if (table.Rows[0]["ant_1_location_dir"] != DBNull.Value)
					{
						readerTO.A1Direction = table.Rows[0]["ant_1_location_dir"].ToString().Trim();
					}

					if (table.Rows[0]["ant_1_sec_location_id"] != DBNull.Value)
					{
						readerTO.A1SecLocID = (int) table.Rows[0]["ant_1_sec_location_id"];
					}

					if (table.Rows[0]["ant_1_sec_location_dir"] != DBNull.Value)
					{
						readerTO.A1SecDirection = table.Rows[0]["ant_1_sec_location_dir"].ToString().Trim();
					}

					if (table.Rows[0]["ant_1_is_wrk_hrs_counter"] != DBNull.Value)
					{
						readerTO.A1IsCounter = (int) table.Rows[0]["ant_1_is_wrk_hrs_counter"];
					}

					if (table.Rows[0]["technology_type"] != DBNull.Value)
					{
						readerTO.TechType = table.Rows[0]["technology_type"].ToString().Trim();
					}
					/*
					if (table.Rows[0]["settings"] != DBNull.Value)
					{
						readerTO.Settings = (byte) table.Rows[0]["settings"];
					}
					*/

					if (table.Rows[0]["status"] != DBNull.Value)
					{
						readerTO.Status = table.Rows[0]["status"].ToString().Trim();
					}
				}

			}
			catch(Exception ex)
			{
				
				throw new Exception("Exception: " + ex.Message);
			}

			return readerTO;
		}

		public int findMAXReaderID()
		{
			DataSet dataSet = new DataSet();
			int readerID = 0;
			try
			{
				SqlCommand cmd = new SqlCommand( "SELECT MAX(reader_id) AS reader_id FROM readers", conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Readers");
				DataTable table = dataSet.Tables["Readers"];

				if (table.Rows.Count == 1 && !table.Rows[0]["reader_id"].Equals(DBNull.Value))
				{					
					readerID = Int32.Parse(table.Rows[0]["reader_id"].ToString().Trim());
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return readerID;
		}
        public Dictionary<int, ReaderTO> getReadersDictionary(ReaderTO readerData)
        {
            DataSet dataSet = new DataSet();
            Dictionary<int, ReaderTO> readerList = new Dictionary<int, ReaderTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM readers ");

                if ((!readerData.ReaderID.Equals(-1)) ||
                    (!readerData.Description.Equals("")) ||
                    (!readerData.ConnectionType.Equals("")) ||
                    (!readerData.ConnectionAddress.Equals("")) ||
                    (!readerData.A0GateID.Equals(-1)) ||
                    (!readerData.A0LocID.Equals(-1)) ||
                    (!readerData.A0Direction.Equals("")) ||
                    (!readerData.A0SecLocID.Equals(-1)) ||
                    (!readerData.A0SecDirection.Equals("")) ||
                    (!readerData.A0IsCounter.Equals(-1)) ||
                    (!readerData.A1IsCounter.Equals(-1)) ||
                    (!readerData.A1GateID.Equals(-1)) ||
                    (!readerData.A1LocID.Equals(-1)) ||
                    (!readerData.A1Direction.Equals("")) ||
                    (!readerData.A1SecLocID.Equals(-1)) ||
                    (!readerData.A1SecDirection.Equals("")) ||
                    (!readerData.Status.Equals("")) ||
                    //(!readerData.Settings.Equals(0)) ||
                    (!readerData.TechType.Equals("")))
                {
                    sb.Append(" WHERE ");

                    if (!readerData.ReaderID.Equals(-1))
                    {
                        sb.Append(" reader_id = '" + readerData.ReaderID.ToString() + "' AND");
                    }
                    if (!readerData.Description.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(description) LIKE N'%" + readerData.Description.Trim().ToUpper() + "%' AND");
                    }
                    if (!readerData.ConnectionType.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(conn_type) LIKE N'%" + readerData.ConnectionType.Trim().ToUpper() + "%' AND");
                    }
                    if (!readerData.ConnectionAddress.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(conn_address) LIKE N'%" + readerData.ConnectionAddress.Trim().ToUpper() + "%' AND");
                    }
                    if (!readerData.A0GateID.Equals(-1))
                    {
                        sb.Append(" ant_0_gate_id = '" + readerData.A0GateID.ToString() + "' AND");
                    }
                    if (!readerData.A0LocID.Equals(-1))
                    {
                        sb.Append(" ant_0_location_id = '" + readerData.A0LocID.ToString() + "' AND");
                    }
                    if (!readerData.A0Direction.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(ant_0_location_dir) LIKE N'%" + readerData.A0Direction.ToUpper() + "%' AND");
                    }
                    if (!readerData.A0SecLocID.Equals(-1))
                    {
                        sb.Append(" ant_0_sec_location_id = '" + readerData.A0SecLocID.ToString() + "' AND");
                    }
                    if (!readerData.A0SecDirection.Equals(""))
                    {
                        sb.Append(" UPPER(ant_0_sec_location_dir) LIKE N'%" + readerData.A0SecDirection.ToUpper() + "%' AND");
                    }
                    if (!readerData.A0IsCounter.Equals(-1))
                    {
                        sb.Append(" ant_0_is_wrk_hrs_counter = '" + readerData.A0IsCounter.ToString() + "' AND");
                    }
                    if (!readerData.A1IsCounter.Equals(-1))
                    {
                        sb.Append(" ant_1_is_wrk_hrs_counter = '" + readerData.A1IsCounter.ToString() + "' AND");
                    }
                    if (!readerData.A1GateID.Equals(-1))
                    {
                        sb.Append(" ant_1_gate_id = '" + readerData.A1GateID.ToString() + "' AND");
                    }
                    if (!readerData.A1LocID.Equals(-1))
                    {
                        sb.Append(" ant_1_location_id = '" + readerData.A1LocID.ToString() + "' AND");
                    }
                    if (!readerData.A1Direction.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(ant_1_location_dir) LIKE N'%" + readerData.A1Direction.Trim().ToUpper() + "%' AND");
                    }
                    if (!readerData.A1SecLocID.Equals(-1))
                    {
                        sb.Append(" ant_1_sec_location_id = '" + readerData.A1SecLocID.ToString() + "' AND");
                    }
                    if (!readerData.A1SecDirection.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(ant_1_sec_location_dir) LIKE N'%" + readerData.A1SecDirection.Trim().ToUpper() + "%' AND");
                    }
                    if (!readerData.TechType.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(technology_type) LIKE N'%" + readerData.TechType.Trim().ToUpper() + "%' AND");
                    }
                    if (!readerData.Status.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(status) LIKE N'%" + readerData.Status.Trim().ToUpper() + "%' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);

                }
                else
                {
                    select = sb.ToString();
                }

                //MySqlCommand cmd = new MySqlCommand(select, conn );
                SqlCommand cmd;
                if (this.SqlTrans == null)
                    cmd = new SqlCommand(select, conn);
                else
                    cmd = new SqlCommand(select, conn, this.SqlTrans);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);


                sqlDataAdapter.Fill(dataSet, "Reader");
                DataTable table = dataSet.Tables["Reader"];

                ReaderTO readTO = new ReaderTO();

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        readTO = new ReaderTO();

                        if (row["reader_id"] != DBNull.Value)
                        {
                            readTO.ReaderID = (int)row["reader_id"];
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            readTO.Description = row["description"].ToString().Trim();
                        }
                        if (row["conn_type"] != DBNull.Value)
                        {
                            readTO.ConnectionType = row["conn_type"].ToString().Trim();
                        }
                        if (row["conn_address"] != DBNull.Value)
                        {
                            readTO.ConnectionAddress = row["conn_address"].ToString().Trim();
                        }
                        if (row["ant_0_gate_id"] != DBNull.Value)
                        {
                            readTO.A0GateID = (int)row["ant_0_gate_id"];
                        }
                        if (row["ant_0_location_id"] != DBNull.Value)
                        {
                            readTO.A0LocID = (int)row["ant_0_location_id"];
                        }
                        if (row["ant_0_location_dir"] != DBNull.Value)
                        {
                            readTO.A0Direction = row["ant_0_location_dir"].ToString().Trim();
                        }
                        if (row["ant_0_sec_location_id"] != DBNull.Value)
                        {
                            readTO.A0SecLocID = (int)row["ant_0_sec_location_id"];
                        }
                        if (row["ant_0_sec_location_dir"] != DBNull.Value)
                        {
                            readTO.A0SecDirection = row["ant_0_sec_location_dir"].ToString().Trim();
                        }
                        if (row["ant_0_is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            readTO.A0IsCounter = (int)row["ant_0_is_wrk_hrs_counter"];
                        }
                        if (row["ant_1_gate_id"] != DBNull.Value)
                        {
                            readTO.A1GateID = (int)row["ant_1_gate_id"];
                        }
                        if (row["ant_1_location_id"] != DBNull.Value)
                        {
                            readTO.A1LocID = (int)row["ant_1_location_id"];
                        }
                        if (row["ant_1_location_dir"] != DBNull.Value)
                        {
                            readTO.A1Direction = row["ant_1_location_dir"].ToString().Trim();
                        }
                        if (row["ant_1_sec_location_id"] != DBNull.Value)
                        {
                            readTO.A1SecLocID = (int)row["ant_1_sec_location_id"];
                        }
                        if (row["ant_1_sec_location_dir"] != DBNull.Value)
                        {
                            readTO.A1SecDirection = row["ant_1_sec_location_dir"].ToString().Trim();
                        }
                        if (row["ant_1_is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            readTO.A1IsCounter = (int)row["ant_1_is_wrk_hrs_counter"];
                        }
                        if (row["technology_type"] != DBNull.Value)
                        {
                            readTO.TechType = row["technology_type"].ToString().Trim();
                        }
                        /*
                        if (row["settings"] != DBNull.Value)
                        {
                            readTO.Settings = (byte) row["settings"];
                        }
                        */

                        if (row["status"] != DBNull.Value)
                        {
                            readTO.Status = row["status"].ToString().Trim();
                        }
                        if (!readerList.ContainsKey(readTO.ReaderID))
                            readerList.Add(readTO.ReaderID, readTO);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return readerList;
        }

        public List<ReaderTO> getReaders(ReaderTO readerData)
		{
			DataSet dataSet = new DataSet();
            List<ReaderTO> readerList = new List<ReaderTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT * FROM readers ");

				if ((!readerData.ReaderID.Equals(-1)) || 
					(!readerData.Description.Equals("")) || 
					(!readerData.ConnectionType.Equals("")) ||
					(!readerData.ConnectionAddress.Equals("")) ||
					(!readerData.A0GateID.Equals(-1)) ||
					(!readerData.A0LocID.Equals(-1)) ||
					(!readerData.A0Direction.Equals("")) ||
					(!readerData.A0SecLocID.Equals(-1)) ||
					(!readerData.A0SecDirection.Equals("")) ||
					(!readerData.A0IsCounter.Equals(-1)) ||
					(!readerData.A1IsCounter.Equals(-1)) ||
					(!readerData.A1GateID.Equals(-1)) ||
					(!readerData.A1LocID.Equals(-1)) ||
					(!readerData.A1Direction.Equals("")) ||
					(!readerData.A1SecLocID.Equals(-1)) ||
					(!readerData.A1SecDirection.Equals("")) ||
                    (!readerData.Status.Equals("")) ||
					//(!readerData.Settings.Equals(0)) ||
					(!readerData.TechType.Equals("")))

				{
					sb.Append(" WHERE ");
					
					if (!readerData.ReaderID.Equals(-1))
					{
						sb.Append(" reader_id = '" + readerData.ReaderID.ToString() + "' AND");
					}
					if (!readerData.Description.Trim().Equals(""))
					{
						sb.Append(" UPPER(description) LIKE N'%" + readerData.Description.Trim().ToUpper() + "%' AND");
					}
					if (!readerData.ConnectionType.Trim().Equals(""))
					{
						sb.Append(" UPPER(conn_type) LIKE N'%" + readerData.ConnectionType.Trim().ToUpper() + "%' AND");
					}
					if (!readerData.ConnectionAddress.Trim().Equals(""))
					{
						sb.Append(" UPPER(conn_address) LIKE N'%" + readerData.ConnectionAddress.Trim().ToUpper() + "%' AND");
					}
					if (!readerData.A0GateID.Equals(-1))
					{
						sb.Append(" ant_0_gate_id = '" + readerData.A0GateID.ToString() + "' AND");
					}
					if (!readerData.A0LocID.Equals(-1))
					{
						sb.Append(" ant_0_location_id = '" + readerData.A0LocID.ToString() + "' AND");
					}
					if (!readerData.A0Direction.Trim().Equals(""))
					{
						sb.Append(" UPPER(ant_0_location_dir) LIKE N'%" + readerData.A0Direction.ToUpper() + "%' AND");
					}
					if (!readerData.A0SecLocID.Equals(-1))
					{
						sb.Append(" ant_0_sec_location_id = '" + readerData.A0SecLocID.ToString() + "' AND");
					}
					if (!readerData.A0SecDirection.Equals(""))
					{
						sb.Append(" UPPER(ant_0_sec_location_dir) LIKE N'%" + readerData.A0SecDirection.ToUpper() + "%' AND");
					}
					if (!readerData.A0IsCounter.Equals(-1))
					{
						sb.Append(" ant_0_is_wrk_hrs_counter = '" + readerData.A0IsCounter.ToString() + "' AND");
					}
					if (!readerData.A1IsCounter.Equals(-1))
					{
						sb.Append(" ant_1_is_wrk_hrs_counter = '" + readerData.A1IsCounter.ToString() + "' AND");
					}
					if (!readerData.A1GateID.Equals(-1))
					{
						sb.Append(" ant_1_gate_id = '" + readerData.A1GateID.ToString() + "' AND");
					}
					if (!readerData.A1LocID.Equals(-1))
					{
						sb.Append(" ant_1_location_id = '" + readerData.A1LocID.ToString() + "' AND");
					}
					if (!readerData.A1Direction.Trim().Equals(""))
					{
						sb.Append(" UPPER(ant_1_location_dir) LIKE N'%" + readerData.A1Direction.Trim().ToUpper() + "%' AND");
					}
					if (!readerData.A1SecLocID.Equals(-1))
					{
						sb.Append(" ant_1_sec_location_id = '" + readerData.A1SecLocID.ToString() + "' AND");
					}
					if (!readerData.A1SecDirection.Trim().Equals(""))
					{
						sb.Append(" UPPER(ant_1_sec_location_dir) LIKE N'%" + readerData.A1SecDirection.Trim().ToUpper() + "%' AND");
					}
					if (!readerData.TechType.Trim().Equals(""))
					{
						sb.Append(" UPPER(technology_type) LIKE N'%" + readerData.TechType.Trim().ToUpper() + "%' AND");
					}
                    if (!readerData.Status.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(status) LIKE N'%" + readerData.Status.Trim().ToUpper() + "%' AND");
                    }

					select = sb.ToString(0, sb.ToString().Length - 3);
				}
				else
				{
					select = sb.ToString();
				}

				//SqlCommand cmd = new SqlCommand(select, conn );
                SqlCommand cmd;
                if (this.SqlTrans == null)
                    cmd = new SqlCommand(select, conn);
                else
                    cmd = new SqlCommand(select, conn, this.SqlTrans);
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Reader");
				DataTable table = dataSet.Tables["Reader"];

				ReaderTO readTO = new ReaderTO();

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						readTO = new ReaderTO();

						if (row["reader_id"] != DBNull.Value)
						{
							readTO.ReaderID = (int) row["reader_id"];
						}
						if (row["description"] != DBNull.Value)
						{
							readTO.Description = row["description"].ToString().Trim();
						}
						if (row["conn_type"] != DBNull.Value)
						{
							readTO.ConnectionType = row["conn_type"].ToString().Trim();
						}
						if (row["conn_address"] != DBNull.Value)
						{
							readTO.ConnectionAddress = row["conn_address"].ToString().Trim();
						}
						if (row["ant_0_gate_id"] != DBNull.Value)
						{
							readTO.A0GateID = (int) row["ant_0_gate_id"];
						}
						if (row["ant_0_location_id"] != DBNull.Value)
						{
							readTO.A0LocID = (int) row["ant_0_location_id"];
						}
						if (row["ant_0_location_dir"] != DBNull.Value)
						{
							readTO.A0Direction = row["ant_0_location_dir"].ToString().Trim();
						}
						if (row["ant_0_sec_location_id"] != DBNull.Value)
						{
							readTO.A0SecLocID = (int) row["ant_0_sec_location_id"];
						}
						if (row["ant_0_sec_location_dir"] != DBNull.Value)
						{
							readTO.A0SecDirection = row["ant_0_sec_location_dir"].ToString().Trim();
						}
						if (row["ant_0_is_wrk_hrs_counter"] != DBNull.Value)
						{
							readTO.A0IsCounter = (int) row["ant_0_is_wrk_hrs_counter"];
						}
						if (row["ant_1_gate_id"] != DBNull.Value)
						{
							readTO.A1GateID = (int) row["ant_1_gate_id"];
						}
						if (row["ant_1_location_id"] != DBNull.Value)
						{
							readTO.A1LocID = (int) row["ant_1_location_id"];
						}
						if (row["ant_1_location_dir"] != DBNull.Value)
						{
							readTO.A1Direction = row["ant_1_location_dir"].ToString().Trim();
						}
						if (row["ant_1_sec_location_id"] != DBNull.Value)
						{
							readTO.A1SecLocID = (int) row["ant_1_sec_location_id"];
						}
						if (row["ant_1_sec_location_dir"] != DBNull.Value)
						{
							readTO.A1SecDirection = row["ant_1_sec_location_dir"].ToString().Trim();
						}
						if (row["ant_1_is_wrk_hrs_counter"] != DBNull.Value)
						{
							readTO.A1IsCounter = (int) row["ant_1_is_wrk_hrs_counter"];
						}
						if (row["technology_type"] != DBNull.Value)
						{
							readTO.TechType = row["technology_type"].ToString().Trim();
						}
						/*
						if (row["settings"] != DBNull.Value)
						{
							readTO.Settings = (byte) row["settings"];
						}
						*/

						if (row["status"] != DBNull.Value)
						{
							readTO.Status = row["status"].ToString().Trim();
						}

						readerList.Add(readTO);
					}
				}
			}
			catch(Exception ex)
			{
				
				throw new Exception("Exception: " + ex.Message);
			}

			return readerList;
		}

        public List<ReaderTO> getReaders(string[] gatesArray)
		{
			DataSet dataSet = new DataSet();
            List<ReaderTO> readerList = new List<ReaderTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT");
				sb.Append(" readers.reader_id AS reader_id,");
				sb.Append(" readers.description AS description,");
				sb.Append(" readers.conn_type AS conn_type,");
				sb.Append(" readers.conn_address AS conn_address,");
				sb.Append(" readers.ant_0_gate_id AS ant_0_gate_id,");
				sb.Append(" readers.ant_0_location_id AS ant_0_location_id,");
				sb.Append(" readers.ant_0_location_dir AS ant_0_location_dir,");
				sb.Append(" readers.ant_0_sec_location_id AS ant_0_sec_location_id,");
				sb.Append(" readers.ant_0_sec_location_dir AS ant_0_sec_location_dir,");
				sb.Append(" readers.ant_0_is_wrk_hrs_counter AS ant_0_is_wrk_hrs_counter,");

				sb.Append(" readers.ant_1_gate_id AS ant_1_gate_id,");
				sb.Append(" readers.ant_1_location_id AS ant_1_location_id,");
				sb.Append(" readers.ant_1_location_dir AS ant_1_location_dir,");
				sb.Append(" readers.ant_1_sec_location_id AS ant_1_sec_location_id,");
				sb.Append(" readers.ant_1_sec_location_dir AS ant_1_sec_location_dir,");
				sb.Append(" readers.ant_1_is_wrk_hrs_counter AS ant_1_is_wrk_hrs_counter,");
				sb.Append(" readers.technology_type AS technology_type,");

				sb.Append(" gates.download_start_time AS download_start_time,");
				sb.Append(" gates.download_interval AS download_interval,");
				sb.Append(" gates.download_erase_counter AS download_erase_counter");
				sb.Append(" FROM readers, gates WHERE");
				foreach( string gate in gatesArray )
				{
					sb.Append(" ((UPPER(ant_0_gate_id) LIKE '" + gate.Trim().ToUpper() + "' OR");
					sb.Append(" UPPER(ant_1_gate_id) LIKE '" + gate.Trim().ToUpper() + "' ) AND");
					sb.Append(" gates.gate_id LIKE '" + gate.Trim().ToUpper() + "' ) OR");
				}
				if ( gatesArray.Length > 0 )
				{
					select = sb.ToString(0, sb.ToString().Length - 2);					
				}
				else
				{
					select = sb.ToString(0, sb.ToString().Length - 5);
				}

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Reader");
				DataTable table = dataSet.Tables["Reader"];

				ReaderTO readTO = new ReaderTO();

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						readTO = new ReaderTO();

						if (row["reader_id"] != DBNull.Value)
						{
							readTO.ReaderID = (int) row["reader_id"];
						}
						if (row["description"] != DBNull.Value)
						{
							readTO.Description = row["description"].ToString().Trim();
						}
						if (row["conn_type"] != DBNull.Value)
						{
							readTO.ConnectionType = row["conn_type"].ToString().Trim();
						}
						if (row["conn_address"] != DBNull.Value)
						{
							readTO.ConnectionAddress = row["conn_address"].ToString().Trim();
						}
						if (row["ant_0_gate_id"] != DBNull.Value)
						{
							readTO.A0GateID = (int) row["ant_0_gate_id"];
						}
						if (row["ant_0_location_id"] != DBNull.Value)
						{
							readTO.A0LocID = (int) row["ant_0_location_id"];
						}
						if (row["ant_0_location_dir"] != DBNull.Value)
						{
							readTO.A0Direction = row["ant_0_location_dir"].ToString().Trim();
						}
						if (row["ant_0_sec_location_id"] != DBNull.Value)
						{
							readTO.A0SecLocID = (int) row["ant_0_sec_location_id"];
						}
						if (row["ant_0_sec_location_dir"] != DBNull.Value)
						{
							readTO.A0SecDirection = row["ant_0_sec_location_dir"].ToString().Trim();
						}
						if (row["ant_0_is_wrk_hrs_counter"] != DBNull.Value)
						{
							readTO.A0IsCounter = (int) row["ant_0_is_wrk_hrs_counter"];
						}
						if (row["ant_1_gate_id"] != DBNull.Value)
						{
							readTO.A1GateID = (int) row["ant_1_gate_id"];
						}
						if (row["ant_1_location_id"] != DBNull.Value)
						{
							readTO.A1LocID = (int) row["ant_1_location_id"];
						}
						if (row["ant_1_location_dir"] != DBNull.Value)
						{
							readTO.A1Direction = row["ant_1_location_dir"].ToString().Trim();
						}
						if (row["ant_1_sec_location_id"] != DBNull.Value)
						{
							readTO.A1SecLocID = (int) row["ant_1_sec_location_id"];
						}
						if (row["ant_1_sec_location_dir"] != DBNull.Value)
						{
							readTO.A1SecDirection = row["ant_1_sec_location_dir"].ToString().Trim();
						}
						if (row["ant_1_is_wrk_hrs_counter"] != DBNull.Value)
						{
							readTO.A1IsCounter = (int) row["ant_1_is_wrk_hrs_counter"];
						}
						if (row["technology_type"] != DBNull.Value)
						{
							readTO.TechType = row["technology_type"].ToString().Trim();
						}
						/*
						if (row["settings"] != DBNull.Value)
						{
							readTO.Settings = (byte) row["settings"];
						}
						*/
						readTO.DownloadStartTime = Convert.ToDateTime(row["download_start_time"].ToString().Trim());
						readTO.DownloadInterval = Int32.Parse(row["download_interval"].ToString().Trim());
						readTO.DownloadEraseCounter = Int32.Parse(row["download_erase_counter"].ToString().Trim());

						readerList.Add(readTO);
					}
				}
			}
			catch(Exception ex)
			{				
				throw new Exception("Exception: " + ex.Message);
			}

			return readerList;
		}

        public List<ReaderTO> getAllReaders()
        {
            DataSet dataSet = new DataSet();
            List<ReaderTO> readerList = new List<ReaderTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT");
                sb.Append(" readers.reader_id AS reader_id,");
                sb.Append(" readers.description AS description,");
                sb.Append(" readers.conn_type AS conn_type,");
                sb.Append(" readers.conn_address AS conn_address,");
                sb.Append(" readers.ant_0_gate_id AS ant_0_gate_id,");
                sb.Append(" readers.ant_0_location_id AS ant_0_location_id,");
                sb.Append(" readers.ant_0_location_dir AS ant_0_location_dir,");
                sb.Append(" readers.ant_0_sec_location_id AS ant_0_sec_location_id,");
                sb.Append(" readers.ant_0_sec_location_dir AS ant_0_sec_location_dir,");
                sb.Append(" readers.ant_0_is_wrk_hrs_counter AS ant_0_is_wrk_hrs_counter,");

                sb.Append(" readers.ant_1_gate_id AS ant_1_gate_id,");
                sb.Append(" readers.ant_1_location_id AS ant_1_location_id,");
                sb.Append(" readers.ant_1_location_dir AS ant_1_location_dir,");
                sb.Append(" readers.ant_1_sec_location_id AS ant_1_sec_location_id,");
                sb.Append(" readers.ant_1_sec_location_dir AS ant_1_sec_location_dir,");
                sb.Append(" readers.ant_1_is_wrk_hrs_counter AS ant_1_is_wrk_hrs_counter,");
                sb.Append(" readers.technology_type AS technology_type");

                sb.Append(" FROM readers ");
               
                select = sb.ToString();               

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Reader");
                DataTable table = dataSet.Tables["Reader"];

                ReaderTO readTO = new ReaderTO();

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        readTO = new ReaderTO();

                        if (row["reader_id"] != DBNull.Value)
                        {
                            readTO.ReaderID = (int)row["reader_id"];
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            readTO.Description = row["description"].ToString().Trim();
                        }
                        if (row["conn_type"] != DBNull.Value)
                        {
                            readTO.ConnectionType = row["conn_type"].ToString().Trim();
                        }
                        if (row["conn_address"] != DBNull.Value)
                        {
                            readTO.ConnectionAddress = row["conn_address"].ToString().Trim();
                        }
                        if (row["ant_0_gate_id"] != DBNull.Value)
                        {
                            readTO.A0GateID = (int)row["ant_0_gate_id"];
                        }
                        if (row["ant_0_location_id"] != DBNull.Value)
                        {
                            readTO.A0LocID = (int)row["ant_0_location_id"];
                        }
                        if (row["ant_0_location_dir"] != DBNull.Value)
                        {
                            readTO.A0Direction = row["ant_0_location_dir"].ToString().Trim();
                        }
                        if (row["ant_0_sec_location_id"] != DBNull.Value)
                        {
                            readTO.A0SecLocID = (int)row["ant_0_sec_location_id"];
                        }
                        if (row["ant_0_sec_location_dir"] != DBNull.Value)
                        {
                            readTO.A0SecDirection = row["ant_0_sec_location_dir"].ToString().Trim();
                        }
                        if (row["ant_0_is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            readTO.A0IsCounter = (int)row["ant_0_is_wrk_hrs_counter"];
                        }
                        if (row["ant_1_gate_id"] != DBNull.Value)
                        {
                            readTO.A1GateID = (int)row["ant_1_gate_id"];
                        }
                        if (row["ant_1_location_id"] != DBNull.Value)
                        {
                            readTO.A1LocID = (int)row["ant_1_location_id"];
                        }
                        if (row["ant_1_location_dir"] != DBNull.Value)
                        {
                            readTO.A1Direction = row["ant_1_location_dir"].ToString().Trim();
                        }
                        if (row["ant_1_sec_location_id"] != DBNull.Value)
                        {
                            readTO.A1SecLocID = (int)row["ant_1_sec_location_id"];
                        }
                        if (row["ant_1_sec_location_dir"] != DBNull.Value)
                        {
                            readTO.A1SecDirection = row["ant_1_sec_location_dir"].ToString().Trim();
                        }
                        if (row["ant_1_is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            readTO.A1IsCounter = (int)row["ant_1_is_wrk_hrs_counter"];
                        }
                        if (row["technology_type"] != DBNull.Value)
                        {
                            readTO.TechType = row["technology_type"].ToString().Trim();
                        }
                      
                        readerList.Add(readTO);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return readerList;
        }

        public List<ReaderTO> getReadersOnAntenna0()
        {
            DataSet dataSet = new DataSet();
            List<ReaderTO> readerList = new List<ReaderTO>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT readers.reader_id, readers.description, gates.download_start_time");
                sb.Append(" FROM readers, gates WHERE");
                sb.Append(" readers.ant_0_gate_id = gates.gate_id");

                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Reader");
                DataTable table = dataSet.Tables["Reader"];

                ReaderTO readTO = new ReaderTO();

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        readTO = new ReaderTO();

                        if (row["reader_id"] != DBNull.Value)
                        {
                            readTO.ReaderID = (int)row["reader_id"];
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            readTO.Description = row["description"].ToString().Trim();
                        }
                        if (!row["download_start_time"].Equals(DBNull.Value))
                        {
                            readTO.DownloadStartTime = (DateTime)row["download_start_time"];
                        }

                        readerList.Add(readTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return readerList;
        }

        public List<ReaderTO> getReadersLastReadTime()
		{
			DataSet dataSet = new DataSet();
            List<ReaderTO> readerList = new List<ReaderTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append(" SELECT log.reader_id, readers.description, MAX(log.modified_time) AS readerTime");
				sb.Append(" FROM log, readers");
				sb.Append(" WHERE log.reader_id = readers.reader_id");
				sb.Append(" AND");
				sb.Append(" (");
				sb.Append(" (log.antenna = 0 AND readers.ant_0_is_wrk_hrs_counter = 1)");
				sb.Append(" OR");
				sb.Append(" (log.antenna = 1 AND readers.ant_1_is_wrk_hrs_counter = 1)");
				sb.Append(" )");
				sb.Append(" AND readers.status = '" + Constants.readerStatusEnabled + "'");
				sb.Append(" GROUP BY log.reader_id, readers.description");
				select = sb.ToString();

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Reader");
				DataTable table = dataSet.Tables["Reader"];

				ReaderTO readTO = new ReaderTO();

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						readTO = new ReaderTO();

						if (row["reader_id"] != DBNull.Value)
						{
							readTO.ReaderID = (int) row["reader_id"];
						}
						if (row["description"] != DBNull.Value)
						{
							readTO.Description = row["description"].ToString().Trim();
						}
						if (row["readerTime"] != DBNull.Value)
						{
							readTO.LastReadTime = (DateTime) row["readerTime"];
						}
						
						readerList.Add(readTO);
					}
				}
			}
			catch(Exception ex)
			{
				
				throw new Exception("Exception: " + ex.Message);
			}

			return readerList;
		}

		public DateTime getAllReadersLastReadTime()
		{
			DataSet dataSet = new DataSet();
            List<ReaderTO> readerList = new List<ReaderTO>();
			string select = "";
			DateTime allReadersTime = new DateTime(0);

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append(" SELECT MIN(ReadersTime.readerTime) AS allReadersTime FROM");
				sb.Append(" (");
				sb.Append(" SELECT MAX(log.modified_time) AS readerTime");
				sb.Append(" FROM log, readers");
				sb.Append(" WHERE log.reader_id = readers.reader_id");
				sb.Append(" AND");
				sb.Append(" (");
				sb.Append(" (log.antenna = 0 AND readers.ant_0_is_wrk_hrs_counter = 1)");
				sb.Append(" OR");
				sb.Append(" (log.antenna = 1 AND readers.ant_1_is_wrk_hrs_counter = 1)");
				sb.Append(" )");
				sb.Append(" AND readers.status = '" + Constants.readerStatusEnabled + "'");
				sb.Append(" GROUP BY log.reader_id");
				sb.Append(" ) AS ReadersTime");
				select = sb.ToString();

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Reader");
				DataTable table = dataSet.Tables["Reader"];

				ReaderTO readTO = new ReaderTO();

				if (table.Rows.Count == 1)
				{
					if (table.Rows[0]["allReadersTime"] != DBNull.Value)
					{
						allReadersTime = (DateTime) table.Rows[0]["allReadersTime"];
					}
				}
			}
			catch(Exception ex)
			{
				
				throw new Exception("Exception: " + ex.Message);
			}

			return allReadersTime;
		}

        public DateTime getLastLogUsed(int readerID, string direction)
        {
            DataSet dataSet = new DataSet();            
            string select = "";
            DateTime lastPass = new DateTime();

            try
            {
                select = "SELECT MAX(l.event_time) AS last_pass FROM log l, readers r " 
                    + "WHERE l.reader_id = " + readerID.ToString().Trim() + " AND l.reader_id = r.reader_id " 
                    + "AND ((l.antenna = 0 AND UPPER(r.ant_0_location_dir) = '" + direction.Trim().ToUpper() + "') OR (l.antenna = 1 AND UPPER(r.ant_1_location_dir) = '" + direction.Trim().ToUpper() + "')) " 
                    + "AND l.pass_gen_used = '" + ((int)Constants.PassGenUsed.Used).ToString().Trim() + "'";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "LastPass");
                DataTable table = dataSet.Tables["LastPass"];
                
                if (table.Rows.Count == 1)
                {
                    if (table.Rows[0]["last_pass"] != DBNull.Value)
                    {
                        lastPass = DateTime.Parse(table.Rows[0]["last_pass"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lastPass;
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

        public void serialize(List<ReaderTO> ReadersTOList)
		{
			try
			{
				//string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLReadersFile"];
                string filename = Constants.XMLDataSourceDir + Constants.XMLReadersFile;
				Stream stream = File.Open(filename, FileMode.Create);

				ReaderTO[] readerTOArray = (ReaderTO[]) ReadersTOList.ToArray();

				XmlSerializer bformatter = new XmlSerializer(typeof(ReaderTO[]));
				bformatter.Serialize(stream, readerTOArray);				
				stream.Close();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
	}
}

