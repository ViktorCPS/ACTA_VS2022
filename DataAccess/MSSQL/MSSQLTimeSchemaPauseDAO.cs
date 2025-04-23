using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using TransferObjects;
using Util;

namespace DataAccess
{
	/// <summary>
	/// Summary description for MSSQLTimeSchemaPauseDAO.
	/// </summary>
	public class MSSQLTimeSchemaPauseDAO : TimeSchemaPauseDAO
	{
		SqlConnection conn = null;

		public MSSQLTimeSchemaPauseDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
		}
        public MSSQLTimeSchemaPauseDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
        }
		public int insert(int pauseID, string description, int pauseDuration,
			int earliestUseTime, int latestUseTime, int pauseOffset, int shortBreakDuration)
		{	
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
			int rowsAffected = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO time_schema_pauses ");
				sbInsert.Append("(pause_id, description, pause_duration, earliest_use_time, latest_use_time, pause_offset, short_break_duration, created_by, created_time) ");
				sbInsert.Append("VALUES (");

				if (pauseID != -1)
				{
					sbInsert.Append(pauseID + ", ");
				}
				else
				{
					//sbInsert.Append("NULL, ");
					//can't be null, exception
				}

				sbInsert.Append("N'" + description.Trim() + "', ");

				if (pauseDuration != -1)
				{
					sbInsert.Append(pauseDuration + ", ");
				}
				else
				{
					//sbInsert.Append("NULL, ");
					//can't be null, exception
				}

				if (earliestUseTime != -1)
				{
					sbInsert.Append(earliestUseTime + ", ");
				}
				else
				{
					//sbInsert.Append("NULL, ");
					//can't be null, exception
				}

				if (latestUseTime != -1)
				{
					sbInsert.Append(latestUseTime + ", ");
				}
				else
				{
					//sbInsert.Append("NULL, ");
					//can't be null, exception
				}

				if (pauseOffset != -1)
				{
					sbInsert.Append(pauseOffset + ", ");
				}
				else
				{
					//sbInsert.Append("NULL, ");
					//can't be null, exception
				}

				if (shortBreakDuration != -1)
				{
					sbInsert.Append(shortBreakDuration + ", ");
				}
				else
				{
					//sbInsert.Append("NULL, ");
					//can't be null, exception
				}

				sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

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

		public bool update(int pauseID, string description, int pauseDuration,
			int earliestUseTime, int latestUseTime, int pauseOffset, int shortBreakDuration)
		{
			bool isUpdated = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

			try
			{
				StringBuilder sbUpdate = new StringBuilder();
				sbUpdate.Append("UPDATE time_schema_pauses SET ");
				
				if (!description.Trim().Equals(""))
				{
					sbUpdate.Append("description = N'" + description.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("description = null, ");
				}

				if (pauseDuration != -1)
				{
					sbUpdate.Append("pause_duration = " + pauseDuration + ", ");
				}
				else
				{
					//sbUpdate.Append("pause_duration = null, ");
					//can't be null, exception
				}

				if (earliestUseTime != -1)
				{
					sbUpdate.Append("earliest_use_time = " + earliestUseTime + ", ");
				}
				else
				{
					//sbUpdate.Append("earliest_use_time = null, ");
					//can't be null, exception
				}

				if (latestUseTime != -1)
				{
					sbUpdate.Append("latest_use_time = " + latestUseTime + ", ");
				}
				else
				{
					//sbUpdate.Append("latest_use_time = null, ");
					//can't be null, exception
				}

				if (pauseOffset != -1)
				{
					sbUpdate.Append("pause_offset = " + pauseOffset + ", ");
				}
				else
				{
					//sbUpdate.Append("pause_offset = null, ");
					//can't be null, exception
				}

				if (shortBreakDuration != -1)
				{
					sbUpdate.Append("short_break_duration = " + shortBreakDuration + ", ");
				}
				else
				{
					//sbUpdate.Append("short_break_duration = null, ");
					//can't be null, exception
				}
				
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = GETDATE() ");
				sbUpdate.Append("WHERE pause_id = " + pauseID);
				
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

		public bool delete(int pauseID)
		{
			bool isDeleted = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

			try
			{
				StringBuilder sbDelete = new StringBuilder();
				sbDelete.Append("DELETE FROM time_schema_pauses WHERE pause_id = " + pauseID);
				
				SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
				int res = cmd.ExecuteNonQuery();
				if (res != 0)
				{
					isDeleted = true;
				}
				sqlTrans.Commit();
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback("DELETE");
				
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public TimeSchemaPauseTO find(int pauseID)
		{
			DataSet dataSet = new DataSet();
			TimeSchemaPauseTO timeSchemaPauseTO = new TimeSchemaPauseTO();
			try
			{
				SqlCommand cmd = new SqlCommand("SELECT pause_id, description, pause_duration, earliest_use_time, latest_use_time, pause_offset, short_break_duration FROM time_schema_pauses WHERE pause_id = " + pauseID, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "TimeSchemaPause");
				DataTable table = dataSet.Tables["TimeSchemaPause"];

				if (table.Rows.Count == 1)
				{
					timeSchemaPauseTO = new TimeSchemaPauseTO();

					if (table.Rows[0]["pause_id"] != DBNull.Value)
					{
						timeSchemaPauseTO.PauseID = Int32.Parse(table.Rows[0]["pause_id"].ToString().Trim());
					}

					if (!table.Rows[0]["description"].Equals(DBNull.Value))
					{
						timeSchemaPauseTO.Description = table.Rows[0]["description"].ToString().Trim();
					}

					if (table.Rows[0]["pause_duration"] != DBNull.Value)
					{
						timeSchemaPauseTO.PauseDuration = Int32.Parse(table.Rows[0]["pause_duration"].ToString().Trim());
					}

					if (table.Rows[0]["earliest_use_time"] != DBNull.Value)
					{
						timeSchemaPauseTO.EarliestUseTime = Int32.Parse(table.Rows[0]["earliest_use_time"].ToString().Trim());
					}

					if (table.Rows[0]["latest_use_time"] != DBNull.Value)
					{
						timeSchemaPauseTO.LatestUseTime = Int32.Parse(table.Rows[0]["latest_use_time"].ToString().Trim());
					}

					if (table.Rows[0]["pause_offset"] != DBNull.Value)
					{
						timeSchemaPauseTO.PauseOffset = Int32.Parse(table.Rows[0]["pause_offset"].ToString().Trim());
					}

					if (table.Rows[0]["short_break_duration"] != DBNull.Value)
					{
						timeSchemaPauseTO.ShortBreakDuration = Int32.Parse(table.Rows[0]["short_break_duration"].ToString().Trim());
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return timeSchemaPauseTO;
		}

		public int findMAXPauseID()
		{
			DataSet dataSet = new DataSet();
			int pID = 0;
			try
			{
				SqlCommand cmd = new SqlCommand( "SELECT MAX(pause_id) AS pause_id FROM time_schema_pauses", conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Pauses");
				DataTable table = dataSet.Tables["Pauses"];

				if (table.Rows.Count == 1 && !table.Rows[0]["pause_id"].Equals(DBNull.Value))
				{					
					pID = Int32.Parse(table.Rows[0]["pause_id"].ToString().Trim());
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return pID;
		}

        // Same as method getTimeSchemaPause(string description, IDBTransaction trans) - command is made without transaction
        public List<TimeSchemaPauseTO> getTimeSchemaPause(string description)
		{
			DataSet dataSet = new DataSet();
			TimeSchemaPauseTO timeSchemaPauseTO = new TimeSchemaPauseTO();
            List<TimeSchemaPauseTO> timeSchemaPauseList = new List<TimeSchemaPauseTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT pause_id, description, pause_duration, earliest_use_time, latest_use_time, pause_offset, short_break_duration FROM time_schema_pauses ");

				if (!description.Trim().Equals(""))
				{
					sb.Append(" WHERE ");					
					sb.Append(" UPPER(description) = N'" + description.ToUpper().Trim() + "'");

					select = sb.ToString();
				}
				else
				{
					select = sb.ToString();
				}

				select = select + " ORDER BY description ";

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "TimeSchemaPause");
				DataTable table = dataSet.Tables["TimeSchemaPause"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						timeSchemaPauseTO = new TimeSchemaPauseTO();

						if (row["pause_id"] != DBNull.Value)
						{
							timeSchemaPauseTO.PauseID = Int32.Parse(row["pause_id"].ToString().Trim());
						}
						if (!row["description"].Equals(DBNull.Value))
						{
							timeSchemaPauseTO.Description = row["description"].ToString().Trim();
						}
						if (row["pause_duration"] != DBNull.Value)
						{
							timeSchemaPauseTO.PauseDuration = Int32.Parse(row["pause_duration"].ToString().Trim());
						}
						if (row["earliest_use_time"] != DBNull.Value)
						{
							timeSchemaPauseTO.EarliestUseTime = Int32.Parse(row["earliest_use_time"].ToString().Trim());
						}
						if (row["latest_use_time"] != DBNull.Value)
						{
							timeSchemaPauseTO.LatestUseTime = Int32.Parse(row["latest_use_time"].ToString().Trim());
						}
						if (row["pause_offset"] != DBNull.Value)
						{
							timeSchemaPauseTO.PauseOffset = Int32.Parse(row["pause_offset"].ToString().Trim());
						}
						if (row["short_break_duration"] != DBNull.Value)
						{
							timeSchemaPauseTO.ShortBreakDuration = Int32.Parse(row["short_break_duration"].ToString().Trim());
						}

						timeSchemaPauseList.Add(timeSchemaPauseTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return timeSchemaPauseList;
		}

        // Same as method getTimeSchemaPause(string description) - command is made with transaction
        public List<TimeSchemaPauseTO> getTimeSchemaPause(string description, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            TimeSchemaPauseTO timeSchemaPauseTO = new TimeSchemaPauseTO();
            List<TimeSchemaPauseTO> timeSchemaPauseList = new List<TimeSchemaPauseTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT pause_id, description, pause_duration, earliest_use_time, latest_use_time, pause_offset, short_break_duration FROM time_schema_pauses ");

                if (!description.Trim().Equals(""))
                {
                    sb.Append(" WHERE ");
                    sb.Append(" UPPER(description) = N'" + description.ToUpper().Trim() + "'");

                    select = sb.ToString();
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + " ORDER BY description ";

                SqlCommand cmd = new SqlCommand(select, conn, (SqlTransaction) trans);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "TimeSchemaPause");
                DataTable table = dataSet.Tables["TimeSchemaPause"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        timeSchemaPauseTO = new TimeSchemaPauseTO();

                        if (row["pause_id"] != DBNull.Value)
                        {
                            timeSchemaPauseTO.PauseID = Int32.Parse(row["pause_id"].ToString().Trim());
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            timeSchemaPauseTO.Description = row["description"].ToString().Trim();
                        }
                        if (row["pause_duration"] != DBNull.Value)
                        {
                            timeSchemaPauseTO.PauseDuration = Int32.Parse(row["pause_duration"].ToString().Trim());
                        }
                        if (row["earliest_use_time"] != DBNull.Value)
                        {
                            timeSchemaPauseTO.EarliestUseTime = Int32.Parse(row["earliest_use_time"].ToString().Trim());
                        }
                        if (row["latest_use_time"] != DBNull.Value)
                        {
                            timeSchemaPauseTO.LatestUseTime = Int32.Parse(row["latest_use_time"].ToString().Trim());
                        }
                        if (row["pause_offset"] != DBNull.Value)
                        {
                            timeSchemaPauseTO.PauseOffset = Int32.Parse(row["pause_offset"].ToString().Trim());
                        }
                        if (row["short_break_duration"] != DBNull.Value)
                        {
                            timeSchemaPauseTO.ShortBreakDuration = Int32.Parse(row["short_break_duration"].ToString().Trim());
                        }

                        timeSchemaPauseList.Add(timeSchemaPauseTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return timeSchemaPauseList;
        }

		// TODO!!!
        public void serialize(List<TimeSchemaPauseTO> timeSchemaPauseTOList)
		{
			try
			{
				string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLTimeSchemaPauseFile"];
				Stream stream = File.Open(filename, FileMode.Create);

				TimeSchemaPauseTO[] timeSchemaPauseTOArray = (TimeSchemaPauseTO[]) timeSchemaPauseTOList.ToArray();

				XmlSerializer bformatter = new XmlSerializer(typeof(TimeSchemaPauseTO[]));
				bformatter.Serialize(stream, timeSchemaPauseTOArray);
				stream.Close();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
	}
}
