using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// MSSQLWorkTimeSchemaDAO implements WorkTimeSchemaDAO interface of MSSQL database;
	/// </summary>
	public class MSSQLWorkTimeSchemaDAO : WorkTimeSchemaDAO
	{
		SqlConnection conn = null;
		protected string dateTimeformat = "";


		/// <summary>
		/// Constructor, get database 
		/// connection from it's factory class
		/// </summary>
		public MSSQLWorkTimeSchemaDAO()
		{
			conn = MSSQLDAOFactory.getConnection();	
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

        public MSSQLWorkTimeSchemaDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

		#region WorkTimeSchemaDAO Members

		public int insert(WorkTimeSchemaTO wtSchemaTO)
		{
			int rowsAffected = 0;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
			DataSet dataSet = new DataSet();
			string id = "";

			try
			{
				StringBuilder sbInsert = new StringBuilder();

				// Insert into header table
				sbInsert.Append("SET NOCOUNT ON ");
				sbInsert.Append("INSERT INTO time_schema_hdr (name, description, time_schema_type,working_unit_id,flag_turnus, created_by, created_time) ");
				sbInsert.Append("VALUES (N'" + wtSchemaTO.Name.Trim() + "', N'" + wtSchemaTO.Description.Trim() + "', N'" + wtSchemaTO.Type.Trim() + "', ");
                if (wtSchemaTO.WorkingUnitID != -1)
                    sbInsert.Append(wtSchemaTO.WorkingUnitID + ", ");
                else
                    sbInsert.Append("NULL, "); 
                if (wtSchemaTO.Turnus != -1)
                    sbInsert.Append(wtSchemaTO.Turnus + ", ");
                else
                    sbInsert.Append("NULL, ");
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");  
				sbInsert.Append("SELECT @@Identity AS schema_id ");
				sbInsert.Append("SET NOCOUNT OFF ");
				
				SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans );
				//rowsAffected = cmd.ExecuteNonQuery();
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
				sqlDataAdapter.Fill(dataSet, "WorkingTimeSchemaID");
				DataTable table = dataSet.Tables["WorkingTimeSchemaID"];

				id = ((DataRow) table.Rows[0])["schema_id"].ToString();

				if ((!id.Equals("")) && (wtSchemaTO.Days.Count > 0))
				{
					sbInsert.Length = 0;
					Dictionary<int, WorkTimeIntervalTO> intervalsTO = new Dictionary<int,WorkTimeIntervalTO>();
					WorkTimeIntervalTO intervalTO = new WorkTimeIntervalTO();

					foreach(int dayKey in wtSchemaTO.Days.Keys)
					{
						intervalsTO = wtSchemaTO.Days[dayKey];

						foreach(int intervalKey in intervalsTO.Keys)
						{
							intervalTO = intervalsTO[intervalKey];
							cmd.CommandText = prepareIntervalInsert(intervalTO, id);
							rowsAffected += cmd.ExecuteNonQuery();
						}
					}

					sqlTrans.Commit();
				}
				else
				{
					sqlTrans.Rollback("INSERT");
				}

			}
			catch(Exception ex)
			{
				sqlTrans.Rollback("INSERT");
				throw ex;
			}
			return rowsAffected;
		}

		public bool delete(int workTimeSchemaID)
		{
			bool isDeleted = false;
			SqlTransaction trans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");
		
			try
			{
				StringBuilder sb = new StringBuilder();

				// Delete form time_schema_dtl
				sb.Append("DELETE FROM time_schema_dtl WHERE time_schema_id = " + workTimeSchemaID + " ");
				SqlCommand cmd = new SqlCommand( sb.ToString(), conn, trans );
				int affectedRows = cmd.ExecuteNonQuery();

				//Delete form time_schema_hdr
				sb.Append("DELETE FROM time_schema_hdr WHERE time_schema_id = " + workTimeSchemaID + " ");
				cmd.CommandText = sb.ToString();

				affectedRows += cmd.ExecuteNonQuery();

				if (affectedRows > 0)
				{
					isDeleted = true;
				}

				trans.Commit();
			}
			catch(Exception ex)
			{
				trans.Rollback();
				throw ex;
			}

			return isDeleted;
		}

		public bool delete(WorkTimeIntervalTO workTimeDayTO)
		{
			return false;
		}

		public bool update(WorkTimeSchemaTO wtSchemaTO)
		{
			bool isUpdated = false;
			SqlTransaction trans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

			try
			{
				StringBuilder sb = new StringBuilder();

				sb.Append("UPDATE time_schema_hdr SET ");
				sb.Append("name = N'" + wtSchemaTO.Name + "', ");
				sb.Append("description = N'" + wtSchemaTO.Description + "', ");
				sb.Append("time_schema_type = N'" + wtSchemaTO.Type + "', ");
                if(wtSchemaTO.Status != "")
                    sb.Append("status = N'" + wtSchemaTO.Status + "', ");
                if (wtSchemaTO.WorkingUnitID != -1)
                    sb.Append("working_unit_id = " + wtSchemaTO.WorkingUnitID.ToString() + ", ");
                if (wtSchemaTO.Turnus != -1)
                    sb.Append("flag_turnus = " + wtSchemaTO.Turnus + ", ");
				sb.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sb.Append("modified_time = GETDATE() ");
				sb.Append("WHERE time_schema_id = " + wtSchemaTO.TimeSchemaID + " ");

				SqlCommand cmd = new SqlCommand(sb.ToString(), conn, trans);
				int res = cmd.ExecuteNonQuery();
			
				if (res > 0)
				{
					// Update details
					string parentKey = wtSchemaTO.TimeSchemaID.ToString();
					int intervalRes = 0;
					Dictionary<int, WorkTimeIntervalTO> intervalsTO = new Dictionary<int,WorkTimeIntervalTO>();
					WorkTimeIntervalTO intervalTO = new WorkTimeIntervalTO();  

					foreach(int dayKey in wtSchemaTO.Days.Keys)
					{
						intervalsTO = wtSchemaTO.Days[dayKey];

						foreach(int intervalKey in intervalsTO.Keys)
						{
							// Try to update
							intervalTO = intervalsTO[intervalKey];
							cmd.CommandText = prepareIntrvalUpdate(intervalTO);

							if ((intervalRes = cmd.ExecuteNonQuery()) == 0)
							{
								// Record don't exists, do insert
								cmd.CommandText = prepareIntervalInsert(intervalTO, intervalTO.TimeSchemaID.ToString());
								intervalRes = cmd.ExecuteNonQuery();
							}
						}
					}

					if (intervalRes != 0)
					{
						isUpdated = true;
						trans.Commit();
					}
					else
					{
						trans.Rollback();
					}
				}
				else
				{
					trans.Rollback();
				}
			}
			catch(Exception ex)
			{
				trans.Rollback();
				throw ex;
			}

			return isUpdated;
		}

		public WorkTimeSchemaTO find(int schemaID)
		{
			// TODO:  Add MSSLWorkTimeSchemaDAO.find implementation
			return null;
		}

		#endregion

        // Same as method getWorkTimeSchemas(string timeSchemaID, string timeSchemaName, string decsription, string timeSchemaType, string duration, IDBTransaction trans)
        // command is made without transaction
		public List<WorkTimeSchemaTO> getWorkTimeSchemas(WorkTimeSchemaTO tsTO)
		{
            List<WorkTimeSchemaTO> wtSchemas = new List<WorkTimeSchemaTO>();
			DataSet dataSet = new DataSet();
			WorkTimeSchemaTO wtSchemaTO = new WorkTimeSchemaTO();

			string having = "";
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();

                sb.Append("SELECT COUNT (DISTINCT dtl.cycle_day) duration, hdr.time_schema_id, hdr.name, hdr.status,hdr.working_unit_id, "
                                + "hdr.description, hdr.time_schema_type, hdr.flag_turnus from time_schema_hdr hdr, time_schema_dtl dtl ");
				sb.Append("WHERE hdr.time_schema_id = dtl.time_schema_id AND ");

				if (tsTO.TimeSchemaID != -1)
				{
                    sb.Append("hdr.time_schema_id = '" + tsTO.TimeSchemaID.ToString().Trim() + "' AND ");
				}
                if (tsTO.WorkingUnitID != -1)
                {
                    sb.Append("hdr.working_unit_id = '" + tsTO.WorkingUnitID.ToString().Trim() + "' AND ");
                } 
                if (!tsTO.Status.Trim().Equals(""))
                {
                    sb.Append("hdr.status = '" + tsTO.Status.Trim().ToUpper() + "' AND ");
                }
                if (!tsTO.Name.Trim().Equals(""))
				{
                    sb.Append("UPPER(hdr.name) LIKE N'%" + tsTO.Name.Trim().ToUpper() + "%' AND ");
				}
                if (!tsTO.Description.Trim().Equals(""))
				{
                    sb.Append("UPPER(hdr.description) LIKE N'%" + tsTO.Description.Trim().ToUpper() + "%' AND ");
				}
                if (!tsTO.Type.Trim().Equals(""))
				{
                    sb.Append("UPPER(hdr.time_schema_type) LIKE N'%" + tsTO.Type.Trim().ToUpper() + "%' AND ");
				}
                if (tsTO.CycleDuration != -1)
				{
                    having = "having count (distinct dtl.cycle_day) = '" + tsTO.CycleDuration.ToString().Trim() + "' ";
				}

				select = sb.ToString();
				select = select.Substring(0, select.Length - 4);
                select += "GROUP BY hdr.time_schema_id, hdr.time_schema_id, hdr.name, hdr.description, hdr.time_schema_type, hdr.status,hdr.working_unit_id , hdr.flag_turnus";
				select += having;

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "WorkingTimeSchema");
				DataTable table = dataSet.Tables["WorkingTimeSchema"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						wtSchemaTO = new WorkTimeSchemaTO();

						wtSchemaTO.TimeSchemaID = Int32.Parse(row["time_schema_id"].ToString());
						wtSchemaTO.Name = row["name"].ToString();
						wtSchemaTO.Description = row["description"].ToString();
						wtSchemaTO.CycleDuration = Int32.Parse(row["duration"].ToString());
						wtSchemaTO.Type = row["time_schema_type"].ToString();
                        if (row["status"] != DBNull.Value)
                        wtSchemaTO.Status = row["status"].ToString();
                        if(row["working_unit_id"] != DBNull.Value)
                        wtSchemaTO.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString());
                        if (row["flag_turnus"] != DBNull.Value)
                            wtSchemaTO.Turnus = Int32.Parse(row["flag_turnus"].ToString());

						wtSchemaTO.Days = this.getWorkTimeSchemaDetails(wtSchemaTO.TimeSchemaID);
						wtSchemas.Add(wtSchemaTO);
					}
				}

			}
			catch(Exception ex)
			{
				throw ex;
			}

			return wtSchemas;
		}
        public List<WorkTimeSchemaTO> getWorkTimeSchemas(string timeSchemaID)
        {
            List<WorkTimeSchemaTO> wtSchemas = new List<WorkTimeSchemaTO>();
            DataSet dataSet = new DataSet();
            WorkTimeSchemaTO wtSchemaTO = new WorkTimeSchemaTO();

            string having = "";
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT COUNT (DISTINCT dtl.cycle_day) duration, hdr.time_schema_id, hdr.name, "
                                + "hdr.description, hdr.time_schema_type, hdr.flag_turnus from time_schema_hdr hdr, time_schema_dtl dtl ");
                sb.Append("WHERE hdr.time_schema_id = dtl.time_schema_id AND ");

                if (!timeSchemaID.Trim().Equals(""))
                {
                    sb.Append("UPPER(hdr.time_schema_id) IN ("+ timeSchemaID.ToUpper() +") AND ");
                }
                select = sb.ToString();
                select = select.Substring(0, select.Length - 4);
                select += "GROUP BY hdr.time_schema_id, hdr.time_schema_id, hdr.name, hdr.description, hdr.time_schema_type, hdr.flag_turnus ";
                select += having;

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "WorkingTimeSchema");
                DataTable table = dataSet.Tables["WorkingTimeSchema"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        wtSchemaTO = new WorkTimeSchemaTO();

                        wtSchemaTO.TimeSchemaID = Int32.Parse(row["time_schema_id"].ToString());
                        wtSchemaTO.Name = row["name"].ToString();
                        wtSchemaTO.Description = row["description"].ToString();
                        wtSchemaTO.CycleDuration = Int32.Parse(row["duration"].ToString());
                        wtSchemaTO.Type = row["time_schema_type"].ToString();
                        if (row["flag_turnus"] != DBNull.Value)
                            wtSchemaTO.Turnus = Int32.Parse(row["flag_turnus"].ToString());

                        wtSchemaTO.Days = this.getWorkTimeSchemaDetails(wtSchemaTO.TimeSchemaID);
                        wtSchemas.Add(wtSchemaTO);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return wtSchemas;
        }
        public Dictionary<int, WorkTimeSchemaTO> getDictionary(WorkTimeSchemaTO tsTO, IDbTransaction trans)
        {
            Dictionary<int, WorkTimeSchemaTO> wtSchemas = new Dictionary<int, WorkTimeSchemaTO>();
            DataSet dataSet = new DataSet();
            WorkTimeSchemaTO wtSchemaTO = new WorkTimeSchemaTO();

            string having = "";
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT COUNT (DISTINCT dtl.cycle_day) duration, hdr.time_schema_id, hdr.name, "
                                + "hdr.description, hdr.time_schema_type, hdr.flag_turnus from time_schema_hdr hdr, time_schema_dtl dtl ");
                sb.Append("WHERE hdr.time_schema_id = dtl.time_schema_id AND ");

                if (tsTO.TimeSchemaID != -1)
                {
                    sb.Append("hdr.time_schema_id = '" + tsTO.TimeSchemaID.ToString().Trim() + "' AND ");
                }
                if (!tsTO.Name.Trim().Equals(""))
                {
                    sb.Append("UPPER(hdr.name) LIKE N'%" + tsTO.Name.Trim().ToUpper() + "%' AND ");
                }
                if (!tsTO.Description.Trim().Equals(""))
                {
                    sb.Append("UPPER(hdr.description) LIKE N'%" + tsTO.Description.Trim().ToUpper() + "%' AND ");
                }
                if (!tsTO.Type.Trim().Equals(""))
                {
                    sb.Append("UPPER(hdr.time_schema_type) LIKE N'%" + tsTO.Type.Trim().ToUpper() + "%' AND ");
                }
                if (tsTO.CycleDuration != -1)
                {
                    having = "having count (distinct dtl.cycle_day) = '" + tsTO.CycleDuration.ToString().Trim() + "' ";
                }

                select = sb.ToString();
                select = select.Substring(0, select.Length - 4);
                select += "GROUP BY hdr.time_schema_id, hdr.time_schema_id, hdr.name, hdr.description, hdr.time_schema_type, hdr.flag_turnus ";
                select += having;

                SqlCommand cmd = new SqlCommand(select, conn,(SqlTransaction)trans);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "WorkingTimeSchema");
                DataTable table = dataSet.Tables["WorkingTimeSchema"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        wtSchemaTO = new WorkTimeSchemaTO();

                        wtSchemaTO.TimeSchemaID = Int32.Parse(row["time_schema_id"].ToString());
                        wtSchemaTO.Name = row["name"].ToString();
                        wtSchemaTO.Description = row["description"].ToString();
                        wtSchemaTO.CycleDuration = Int32.Parse(row["duration"].ToString());
                        wtSchemaTO.Type = row["time_schema_type"].ToString();
                        if (row["flag_turnus"] != DBNull.Value)
                            wtSchemaTO.Turnus = Int32.Parse(row["flag_turnus"].ToString());
                        wtSchemaTO.Days = this.getWorkTimeSchemaDetails(wtSchemaTO.TimeSchemaID,trans);
                        if (!wtSchemas.ContainsKey(wtSchemaTO.TimeSchemaID))
                        {
                            wtSchemas.Add(wtSchemaTO.TimeSchemaID, wtSchemaTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return wtSchemas;
        }
        public Dictionary<int, WorkTimeSchemaTO> getDictionary(WorkTimeSchemaTO tsTO)
        {
            Dictionary<int, WorkTimeSchemaTO> wtSchemas = new Dictionary<int, WorkTimeSchemaTO>();
            DataSet dataSet = new DataSet();
            WorkTimeSchemaTO wtSchemaTO = new WorkTimeSchemaTO();

            string having = "";
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT COUNT (DISTINCT dtl.cycle_day) duration,hdr.status as status, hdr.time_schema_id, hdr.name, "
                                + "hdr.description, hdr.time_schema_type, hdr.flag_turnus from time_schema_hdr hdr, time_schema_dtl dtl ");
                sb.Append("WHERE hdr.time_schema_id = dtl.time_schema_id AND ");

                if (tsTO.TimeSchemaID != -1)
                {
                    sb.Append("hdr.time_schema_id = '" + tsTO.TimeSchemaID.ToString().Trim() + "' AND ");
                }
                if (!tsTO.Name.Trim().Equals(""))
                {
                    sb.Append("UPPER(hdr.name) LIKE N'%" + tsTO.Name.Trim().ToUpper() + "%' AND ");
                }
                if (!tsTO.Description.Trim().Equals(""))
                {
                    sb.Append("UPPER(hdr.description) LIKE N'%" + tsTO.Description.Trim().ToUpper() + "%' AND ");
                }
                if (!tsTO.Type.Trim().Equals(""))
                {
                    sb.Append("UPPER(hdr.time_schema_type) LIKE N'%" + tsTO.Type.Trim().ToUpper() + "%' AND ");
                }
                if (tsTO.CycleDuration != -1)
                {
                    having = "having count (distinct dtl.cycle_day) = '" + tsTO.CycleDuration.ToString().Trim() + "' ";
                }

                select = sb.ToString();
                select = select.Substring(0, select.Length - 4);
                select += "GROUP BY hdr.time_schema_id, hdr.time_schema_id, hdr.name, hdr.description, hdr.time_schema_type, hdr.flag_turnus,hdr.status ";
                select += having;

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "WorkingTimeSchema");
                DataTable table = dataSet.Tables["WorkingTimeSchema"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        wtSchemaTO = new WorkTimeSchemaTO();

                        wtSchemaTO.TimeSchemaID = Int32.Parse(row["time_schema_id"].ToString());
                        wtSchemaTO.Name = row["name"].ToString();
                        wtSchemaTO.Description = row["description"].ToString();
                        wtSchemaTO.CycleDuration = Int32.Parse(row["duration"].ToString());
                        wtSchemaTO.Type = row["time_schema_type"].ToString();
                        if (row["flag_turnus"] != DBNull.Value)
                            wtSchemaTO.Turnus = Int32.Parse(row["flag_turnus"].ToString());
                         if (row["status"] != DBNull.Value)
                            wtSchemaTO.Status = row["status"].ToString();
                        wtSchemaTO.Days = this.getWorkTimeSchemaDetails(wtSchemaTO.TimeSchemaID);
                        if (!wtSchemas.ContainsKey(wtSchemaTO.TimeSchemaID))
                        {
                            wtSchemas.Add(wtSchemaTO.TimeSchemaID, wtSchemaTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return wtSchemas;
        }

        public Dictionary<int, WorkTimeSchemaTO> getWorkTimeSchemasDictionary(WorkTimeSchemaTO tsTO, IDbTransaction trans)
        {
            Dictionary<int, WorkTimeSchemaTO> wtSchemas = new Dictionary<int, WorkTimeSchemaTO>();
            DataSet dataSet = new DataSet();
            WorkTimeSchemaTO wtSchemaTO = new WorkTimeSchemaTO();

            string having = "";
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT COUNT (DISTINCT dtl.cycle_day) duration, hdr.time_schema_id, hdr.name, "
                                + "hdr.description, hdr.time_schema_type, hdr.flag_turnus from time_schema_hdr hdr, time_schema_dtl dtl ");
                sb.Append("WHERE hdr.time_schema_id = dtl.time_schema_id AND ");

                if (tsTO.TimeSchemaID != -1)
                {
                    sb.Append("hdr.time_schema_id = '" + tsTO.TimeSchemaID.ToString().Trim() + "' AND ");
                }
                if (!tsTO.Name.Trim().Equals(""))
                {
                    sb.Append("UPPER(hdr.name) LIKE N'%" + tsTO.Name.Trim().ToUpper() + "%' AND ");
                }
                if (!tsTO.Description.Trim().Equals(""))
                {
                    sb.Append("UPPER(hdr.description) LIKE N'%" + tsTO.Description.Trim().ToUpper() + "%' AND ");
                }
                if (!tsTO.Type.Trim().Equals(""))
                {
                    sb.Append("UPPER(hdr.time_schema_type) LIKE N'%" + tsTO.Type.Trim().ToUpper() + "%' AND ");
                }
                if (tsTO.CycleDuration != -1)
                {
                    having = "having count (distinct dtl.cycle_day) = '" + tsTO.CycleDuration.ToString().Trim() + "' ";
                }

                select = sb.ToString();
                select = select.Substring(0, select.Length - 4);
                select += "GROUP BY hdr.time_schema_id, hdr.time_schema_id, hdr.name, hdr.description, hdr.time_schema_type, hdr.flag_turnus ";
                select += having;

                SqlCommand cmd;
                if (trans != null)
                    cmd = new SqlCommand(select, conn, (SqlTransaction)trans);
                else
                    cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "WorkingTimeSchema");
                DataTable table = dataSet.Tables["WorkingTimeSchema"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        wtSchemaTO = new WorkTimeSchemaTO();

                        wtSchemaTO.TimeSchemaID = Int32.Parse(row["time_schema_id"].ToString());
                        wtSchemaTO.Name = row["name"].ToString();
                        wtSchemaTO.Description = row["description"].ToString();
                        wtSchemaTO.CycleDuration = Int32.Parse(row["duration"].ToString());
                        wtSchemaTO.Type = row["time_schema_type"].ToString();
                        if (row["flag_turnus"] != DBNull.Value)
                            wtSchemaTO.Turnus = Int32.Parse(row["flag_turnus"].ToString());

                        wtSchemaTO.Days = this.getWorkTimeSchemaDetailsByStartTime(wtSchemaTO.TimeSchemaID, trans);

                        if (!wtSchemas.ContainsKey(wtSchemaTO.TimeSchemaID))
                            wtSchemas.Add(wtSchemaTO.TimeSchemaID, wtSchemaTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return wtSchemas;
        }

        // Same as method getWorkTimeSchemas(string timeSchemaID, string timeSchemaName, string decsription, string timeSchemaType, string duration)
        // command is made with transaction
        public List<WorkTimeSchemaTO> getWorkTimeSchemas(WorkTimeSchemaTO tsTO, IDbTransaction trans)
        {
            List<WorkTimeSchemaTO> wtSchemas = new List<WorkTimeSchemaTO>();
            DataSet dataSet = new DataSet();
            WorkTimeSchemaTO wtSchemaTO = new WorkTimeSchemaTO();

            string having = "";
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT COUNT (DISTINCT dtl.cycle_day) duration, hdr.time_schema_id, hdr.name, "
                                + "hdr.description, hdr.time_schema_type, hdr.flag_turnus from time_schema_hdr hdr, time_schema_dtl dtl ");
                sb.Append("WHERE hdr.time_schema_id = dtl.time_schema_id AND ");

                if (tsTO.TimeSchemaID != -1)
                {
                    sb.Append("hdr.time_schema_id = '" + tsTO.TimeSchemaID.ToString().Trim() + "' AND ");
                }
                if (!tsTO.Name.Trim().Equals(""))
                {
                    sb.Append("UPPER(hdr.name) LIKE N'%" + tsTO.Name.Trim().ToUpper() + "%' AND ");
                }
                if (!tsTO.Description.Trim().Equals(""))
                {
                    sb.Append("UPPER(hdr.description) LIKE N'%" + tsTO.Description.Trim().ToUpper() + "%' AND ");
                }
                if (!tsTO.Type.Trim().Equals(""))
                {
                    sb.Append("UPPER(hdr.time_schema_type) LIKE N'%" + tsTO.Type.Trim().ToUpper() + "%' AND ");
                }
                if (tsTO.CycleDuration != -1)
                {
                    having = "having count (distinct dtl.cycle_day) = '" + tsTO.CycleDuration.ToString().Trim() + "' ";
                }

                select = sb.ToString();
                select = select.Substring(0, select.Length - 4);
                select += "GROUP BY hdr.time_schema_id, hdr.time_schema_id, hdr.name, hdr.description, hdr.time_schema_type, hdr.flag_turnus ";
                select += having;

                SqlCommand cmd = new SqlCommand(select, conn, (SqlTransaction) trans);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "WorkingTimeSchema");
                DataTable table = dataSet.Tables["WorkingTimeSchema"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        wtSchemaTO = new WorkTimeSchemaTO();

                        wtSchemaTO.TimeSchemaID = Int32.Parse(row["time_schema_id"].ToString());
                        wtSchemaTO.Name = row["name"].ToString();
                        wtSchemaTO.Description = row["description"].ToString();
                        wtSchemaTO.CycleDuration = Int32.Parse(row["duration"].ToString());
                        wtSchemaTO.Type = row["time_schema_type"].ToString();
                        if (row["flag_turnus"] != DBNull.Value)
                            wtSchemaTO.Turnus = Int32.Parse(row["flag_turnus"].ToString());

                        wtSchemaTO.Days = this.getWorkTimeSchemaDetails(wtSchemaTO.TimeSchemaID, trans);
                        wtSchemas.Add(wtSchemaTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return wtSchemas;
        }

        // Same as method getWorkTimeSchemaDetails(int timeSchemaID, IDBTransaction trans) - command is made without transaction
		private Dictionary<int, Dictionary<int, WorkTimeIntervalTO>> getWorkTimeSchemaDetails(int timeSchemaID)
		{
			
			DataSet dataSet = new DataSet();

            Dictionary<int, Dictionary<int, WorkTimeIntervalTO>> Days = new Dictionary<int, Dictionary<int, WorkTimeIntervalTO>>();
            Dictionary<int, WorkTimeIntervalTO> Intervals = new Dictionary<int, WorkTimeIntervalTO>();

			WorkTimeIntervalTO intervalTO = new WorkTimeIntervalTO();

			try
			{
				
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT * FROM time_schema_dtl ");
				sb.Append("WHERE time_schema_id = " + timeSchemaID.ToString() + " ORDER BY cycle_day, interval_ord_num");

				SqlCommand cmd = new SqlCommand(sb.ToString() , conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "WTSchemaDtl");
				DataTable table = dataSet.Tables["WTSchemaDtl"];				
				
				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						intervalTO = new WorkTimeIntervalTO();

						// Data
						if (row["time_schema_id"] != DBNull.Value)
						{
							intervalTO.TimeSchemaID = Int32.Parse(row["time_schema_id"].ToString());
						}
						if (row["cycle_day"] != DBNull.Value)
						{
							intervalTO.DayNum = Int32.Parse(row["cycle_day"].ToString());
						}
						if (row["interval_ord_num"] != DBNull.Value)
						{
							intervalTO.IntervalNum = Int32.Parse(row["interval_ord_num"].ToString());
						}
						if (row["ea_time"] != DBNull.Value)
						{
							intervalTO.EarliestArrived = Convert.ToDateTime(row["ea_time"].ToString());
						}
						if (row["start_time"] != DBNull.Value)
						{
							intervalTO.StartTime = Convert.ToDateTime(row["start_time"].ToString());
						}
						if (row["la_time"] != DBNull.Value)
						{
							intervalTO.LatestArrivaed = Convert.ToDateTime(row["la_time"].ToString());
						}
						if (row["el_time"] != DBNull.Value)
						{
							intervalTO.EarliestLeft = Convert.ToDateTime(row["el_time"].ToString());
						}
						if (row["end_time"] != DBNull.Value)
						{
							intervalTO.EndTime = Convert.ToDateTime(row["end_time"].ToString());
						}
						if (row["ll_time"] != DBNull.Value)
						{
							intervalTO.LatestLeft = Convert.ToDateTime(row["ll_time"].ToString());
						}
						if (row["auto_close"] != DBNull.Value)
						{
							intervalTO.AutoClose = Int32.Parse(row["auto_close"].ToString());
						}
						if (row["pause_id"] != DBNull.Value)
						{
							intervalTO.PauseID = Int32.Parse(row["pause_id"].ToString());
						}
                        if (row["description"] != DBNull.Value)
                        {
                            intervalTO.Description = row["description"].ToString();
                        } 

						if (!Days.ContainsKey(intervalTO.DayNum))
						{
							Intervals = new Dictionary<int,WorkTimeIntervalTO>();
							Intervals.Add(intervalTO.IntervalNum, intervalTO);

							Days.Add(intervalTO.DayNum, Intervals);
						}
						else
						{
							Intervals = Days[intervalTO.DayNum];
							Intervals.Add(intervalTO.IntervalNum, intervalTO);
						}
					}
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}

			return Days;
		}

        // Same as method getWorkTimeSchemaDetails(int timeSchemaID) - command is made with transaction
        private Dictionary<int, Dictionary<int, WorkTimeIntervalTO>> getWorkTimeSchemaDetails(int timeSchemaID, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();

            Dictionary<int, Dictionary<int, WorkTimeIntervalTO>> Days = new Dictionary<int, Dictionary<int, WorkTimeIntervalTO>>();
            Dictionary<int, WorkTimeIntervalTO> Intervals = new Dictionary<int, WorkTimeIntervalTO>();

            WorkTimeIntervalTO intervalTO = new WorkTimeIntervalTO();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM time_schema_dtl ");
                sb.Append("WHERE time_schema_id = " + timeSchemaID.ToString() + " ORDER BY cycle_day, interval_ord_num");

                SqlCommand cmd = new SqlCommand(sb.ToString(), conn, (SqlTransaction) trans);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "WTSchemaDtl");
                DataTable table = dataSet.Tables["WTSchemaDtl"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        intervalTO = new WorkTimeIntervalTO();

                        // Data
                        if (row["time_schema_id"] != DBNull.Value)
                        {
                            intervalTO.TimeSchemaID = Int32.Parse(row["time_schema_id"].ToString());
                        }
                        if (row["cycle_day"] != DBNull.Value)
                        {
                            intervalTO.DayNum = Int32.Parse(row["cycle_day"].ToString());
                        }
                        if (row["interval_ord_num"] != DBNull.Value)
                        {
                            intervalTO.IntervalNum = Int32.Parse(row["interval_ord_num"].ToString());
                        }
                        if (row["ea_time"] != DBNull.Value)
                        {
                            intervalTO.EarliestArrived = Convert.ToDateTime(row["ea_time"].ToString());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            intervalTO.StartTime = Convert.ToDateTime(row["start_time"].ToString());
                        }
                        if (row["la_time"] != DBNull.Value)
                        {
                            intervalTO.LatestArrivaed = Convert.ToDateTime(row["la_time"].ToString());
                        }
                        if (row["el_time"] != DBNull.Value)
                        {
                            intervalTO.EarliestLeft = Convert.ToDateTime(row["el_time"].ToString());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            intervalTO.EndTime = Convert.ToDateTime(row["end_time"].ToString());
                        }
                        if (row["ll_time"] != DBNull.Value)
                        {
                            intervalTO.LatestLeft = Convert.ToDateTime(row["ll_time"].ToString());
                        }
                        if (row["auto_close"] != DBNull.Value)
                        {
                            intervalTO.AutoClose = Int32.Parse(row["auto_close"].ToString());
                        }
                        if (row["pause_id"] != DBNull.Value)
                        {
                            intervalTO.PauseID = Int32.Parse(row["pause_id"].ToString());
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            intervalTO.Description = row["description"].ToString();
                        }

                        if (!Days.ContainsKey(intervalTO.DayNum))
                        {
                            Intervals = new Dictionary<int,WorkTimeIntervalTO>();
                            Intervals.Add(intervalTO.IntervalNum, intervalTO);

                            Days.Add(intervalTO.DayNum, Intervals);
                        }
                        else
                        {
                            Intervals = Days[intervalTO.DayNum];
                            Intervals.Add(intervalTO.IntervalNum, intervalTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Days;
        }

        private Dictionary<int, Dictionary<int, WorkTimeIntervalTO>> getWorkTimeSchemaDetailsByStartTime(int timeSchemaID, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();

            Dictionary<int, Dictionary<int, WorkTimeIntervalTO>> Days = new Dictionary<int, Dictionary<int, WorkTimeIntervalTO>>();
            Dictionary<int, WorkTimeIntervalTO> Intervals = new Dictionary<int, WorkTimeIntervalTO>();

            WorkTimeIntervalTO intervalTO = new WorkTimeIntervalTO();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM time_schema_dtl ");
                sb.Append("WHERE time_schema_id = " + timeSchemaID.ToString() + " ORDER BY cycle_day, start_time");

                SqlCommand cmd;
                if (trans != null)
                    cmd = new SqlCommand(sb.ToString(), conn, (SqlTransaction)trans);
                else
                    cmd = new SqlCommand(sb.ToString(), conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "WTSchemaDtl");
                DataTable table = dataSet.Tables["WTSchemaDtl"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        intervalTO = new WorkTimeIntervalTO();

                        // Data
                        if (row["time_schema_id"] != DBNull.Value)
                        {
                            intervalTO.TimeSchemaID = Int32.Parse(row["time_schema_id"].ToString());
                        }
                        if (row["cycle_day"] != DBNull.Value)
                        {
                            intervalTO.DayNum = Int32.Parse(row["cycle_day"].ToString());
                        }
                        if (row["interval_ord_num"] != DBNull.Value)
                        {
                            intervalTO.IntervalNum = Int32.Parse(row["interval_ord_num"].ToString());
                        }
                        if (row["ea_time"] != DBNull.Value)
                        {
                            intervalTO.EarliestArrived = Convert.ToDateTime(row["ea_time"].ToString());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            intervalTO.StartTime = Convert.ToDateTime(row["start_time"].ToString());
                        }
                        if (row["la_time"] != DBNull.Value)
                        {
                            intervalTO.LatestArrivaed = Convert.ToDateTime(row["la_time"].ToString());
                        }
                        if (row["el_time"] != DBNull.Value)
                        {
                            intervalTO.EarliestLeft = Convert.ToDateTime(row["el_time"].ToString());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            intervalTO.EndTime = Convert.ToDateTime(row["end_time"].ToString());
                        }
                        if (row["ll_time"] != DBNull.Value)
                        {
                            intervalTO.LatestLeft = Convert.ToDateTime(row["ll_time"].ToString());
                        }
                        if (row["auto_close"] != DBNull.Value)
                        {
                            intervalTO.AutoClose = Int32.Parse(row["auto_close"].ToString());
                        }
                        if (row["pause_id"] != DBNull.Value)
                        {
                            intervalTO.PauseID = Int32.Parse(row["pause_id"].ToString());
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            intervalTO.Description = row["description"].ToString();
                        }

                        if (!Days.ContainsKey(intervalTO.DayNum))
                        {
                            Intervals = new Dictionary<int, WorkTimeIntervalTO>();
                            Intervals.Add(intervalTO.IntervalNum, intervalTO);

                            Days.Add(intervalTO.DayNum, Intervals);
                        }
                        else
                        {
                            Intervals = Days[intervalTO.DayNum];
                            Intervals.Add(intervalTO.IntervalNum, intervalTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Days;
        }

		private string prepareIntervalInsert(WorkTimeIntervalTO interval, string schema_id)
		{
			StringBuilder sbInsert = new StringBuilder();

			// Insert statement
			sbInsert.Append("INSERT INTO time_schema_dtl ( ");
			sbInsert.Append("time_schema_id, cycle_day, interval_ord_num, ");
			sbInsert.Append("start_time, end_time, ea_time, ");
			sbInsert.Append("la_time, el_time, ll_time, auto_close, pause_id, description, ");
			sbInsert.Append("created_by, created_time) ");
			sbInsert.Append("VALUES (" + schema_id + ", " + interval.DayNum + ", " + interval.IntervalNum + ", ");
			sbInsert.Append("'" + interval.StartTime.ToLongTimeString() + "', '" + interval.EndTime.ToLongTimeString() + "', '" + interval.EarliestArrived.ToLongTimeString() + "', ");
			sbInsert.Append("'" + interval.LatestArrivaed.ToLongTimeString() + "', '" + interval.EarliestLeft.ToLongTimeString() + "', '" + interval.LatestLeft.ToLongTimeString() +"', " + interval.AutoClose.ToString().Trim() + ", ");
			if (interval.PauseID != -1)
				sbInsert.Append(interval.PauseID.ToString().Trim() + ", ");
			else
				sbInsert.Append("null, ");
            sbInsert.Append("N'" + interval.Description+ "', ");
			sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE())");

			return sbInsert.ToString();
		}

		private string prepareIntrvalUpdate(WorkTimeIntervalTO interval)
		{
			
			StringBuilder sb = new StringBuilder();

			// Update statement
			sb.Append("UPDATE time_schema_dtl SET ");
			sb.Append("start_time = '" + interval.StartTime.ToLongTimeString() + "', ");
			sb.Append("end_time = '" + interval.EndTime.ToLongTimeString() + "', ");
			sb.Append("ea_time = '" + interval.EarliestArrived.ToLongTimeString() + "', ");
			sb.Append("la_time = '" + interval.LatestArrivaed.ToLongTimeString() + "', ");
			sb.Append("el_time = '" + interval.EarliestLeft.ToLongTimeString() + "', ");
			sb.Append("ll_time = '" + interval.LatestLeft.ToLongTimeString() + "', ");
			sb.Append("auto_close = '" + interval.AutoClose.ToString().Trim() + "', ");
			if (interval.PauseID != -1)
				sb.Append("pause_id = '" + interval.PauseID.ToString().Trim() + "', ");
			else
                sb.Append("pause_id = null, ");
            sb.Append("description = N'" + interval.Description.Trim() + "', ");
			sb.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
			sb.Append("modified_time = GETDATE() ");
			sb.Append("WHERE time_schema_id = " + interval.TimeSchemaID + " AND ");
			sb.Append("cycle_day = " + interval.DayNum + " AND interval_ord_num = " + interval.IntervalNum + " ");

			return sb.ToString();
		}

		
		public bool timeSchemaIsUsed(int timeSchemaID)
		{
			bool isUsed = false;
			DataSet dataset = new DataSet();

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT COUNT(*) num FROM employees_time_schedule WHERE time_schema_id = " + timeSchemaID + " ");
				SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
				SqlDataAdapter addapter = new SqlDataAdapter(cmd);
				addapter.Fill(dataset, "count");

				DataTable datatable = dataset.Tables["count"];
				int num = Int32.Parse(datatable.Rows[0]["num"].ToString());

				if (num > 0)
				{
					isUsed = true;
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}

			return isUsed;
		}
		
		public List<WorkTimeIntervalTO> GetCriticalMoments()
		{
            List<WorkTimeIntervalTO> moments = new List<WorkTimeIntervalTO>();
			DataSet dataset = new DataSet();

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT distinct start_time as start_time, end_time as end_time FROM time_schema_dtl");
				SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
				SqlDataAdapter addapter = new SqlDataAdapter(cmd);
				addapter.Fill(dataset, "time");

				DataTable table = dataset.Tables["time"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						WorkTimeIntervalTO wiTO = new WorkTimeIntervalTO();
						
						wiTO.StartTime = Convert.ToDateTime(row["start_time"].ToString());
						wiTO.EndTime = Convert.ToDateTime(row["end_time"].ToString());

						moments.Add(wiTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}

			return moments;
		}

		public int getPauses()
		{
			int pauseCount = 0;
			DataSet dataSet = new DataSet();

			try
			{
				StringBuilder sb = new StringBuilder();
				
				sb.Append("SELECT COUNT(*) count FROM time_schema_dtl");
				sb.Append(" WHERE pause_id IS NOT null");

				SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "WorkingTimeSchema");
				DataTable table = dataSet.Tables["WorkingTimeSchema"];

				if (table.Rows.Count > 0)
				{
					pauseCount = Int32.Parse(table.Rows[0]["count"].ToString().Trim());
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return pauseCount;
		}
	}
}
