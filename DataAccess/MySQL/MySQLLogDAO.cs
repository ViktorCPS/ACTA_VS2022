using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Globalization;
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
	/// MSSQL Database specific LogDAO, implements LogDAO interface
	/// </summary>
	public class MySQLLogDAO : LogDAO
	{
		MySqlConnection conn = null;
		MySqlTransaction _sqlTrans = null;
		protected string dateTimeformat = "";

		public MySQLLogDAO()
		{
			conn = MySQLDAOFactory.getConnection();

			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MySQLLogDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
		public MySqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

		/// <summary>
		/// Insert data from logTo object into Log table
		/// </summary>
		/// <param name="logTo">Log Transfer Object</param>
		/// <returns>Number of affected rows</returns>
		public int insert(LogTO logTo)
		{
			int rowsAffected = 0;
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

			try
			{
				StringBuilder sbInsert = new StringBuilder();

				sbInsert.Append("INSERT INTO log ");
				sbInsert.Append("(reader_id, tag_id, antenna, event_happened, action_commited, " 
					+ "event_time, pass_gen_used, created_by, created_time) ");			
				sbInsert.Append("VALUES (" 
					+ logTo.ReaderID + ", " + logTo.TagID + ", " + logTo.Antenna + ", " 
					+ logTo.EventHappened + ", " + logTo.ActionCommited + ", '" + logTo.EventTime.ToString(dateTimeformat.Replace("'", "")).Trim() + "', "
					+ logTo.PassGenUsed + ", N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");  

				
				MySqlCommand cmd = new MySqlCommand( sbInsert.ToString(), conn, sqlTrans );
				rowsAffected = cmd.ExecuteNonQuery();
				sqlTrans.Commit();
			}
			catch(MySqlException sqlex)
			{
				sqlTrans.Rollback();
				if(sqlex.Number == 1062)
				{
					throw new Exception("Pass Type already exists");
				}
				else
				{
					throw new Exception(sqlex.Message);
				}
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback();
				
				throw new Exception("Exception: " + ex.Message);
			}

			return rowsAffected;
		}

        /// <summary>
        /// Insert data from logTo object into Log table
        /// </summary>
        /// <param name="logTo">Log Transfer Object</param>
        /// <returns>Number of affected rows</returns>
        public int insertMobile(LogTmpAdditionalInfoTO logTmpTO, bool doCommit)
        {
            int rowsAffected = 0;
            DataSet dataSet = new DataSet();

            MySqlTransaction sqlTrans = null;
            if (doCommit)
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            else
                sqlTrans = this.SqlTrans;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO log ");
                sbInsert.Append("(reader_id, tag_id, antenna, event_happened, action_commited, event_time, pass_gen_used, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("'" + logTmpTO.ReaderID.ToString().Trim() + "', ");
                sbInsert.Append("'" + logTmpTO.TagID.ToString().Trim() + "', ");
                sbInsert.Append("'" + logTmpTO.Antenna.ToString().Trim() + "', ");
                if (logTmpTO.EventHappened != -1)
                    sbInsert.Append("'" + logTmpTO.EventHappened.ToString().Trim() + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (logTmpTO.ActionCommited != -1)
                    sbInsert.Append("'" + logTmpTO.ActionCommited.ToString().Trim() + "', ");
                else
                    sbInsert.Append("NULL, ");
                sbInsert.Append("'" + logTmpTO.EventTime.ToString(dateTimeformat) + "', ");
                sbInsert.Append("'" + logTmpTO.PassGenUsed.ToString().Trim() + "', ");
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbInsert.Append("GETDATE()) ");
                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MySqlCommand identityCmd = new MySqlCommand("SELECT IDENT_CURRENT('log') AS log_id", conn, sqlTrans);
                    MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(identityCmd);
                    sqlAdapter.Fill(dataSet, "Log");
                    DataTable dataTable = dataSet.Tables["Log"];

                    uint logID = 0;
                    if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["log_id"] != DBNull.Value && uint.TryParse(dataTable.Rows[0]["log_id"].ToString().Trim(), out logID))
                    {
                        sbInsert = new StringBuilder();
                        sbInsert.Append("INSERT INTO log_additional_info ");
                        sbInsert.Append("(log_id, gps_data, cardholder_name, cardholder_id, created_by, created_time) ");
                        sbInsert.Append("VALUES (");
                        sbInsert.Append("'" + logID.ToString().Trim() + "', ");
                        sbInsert.Append("'" + logTmpTO.GpsData.ToString().Trim() + "', ");
                        sbInsert.Append("'" + logTmpTO.CardholderName.Trim() + "', ");
                        sbInsert.Append("'" + logTmpTO.CardholderID.ToString().Trim() + "', ");
                        sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                        sbInsert.Append("GETDATE()) ");
                        MySqlCommand addCmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);

                        rowsAffected = addCmd.ExecuteNonQuery();
                    }
                }

                if (doCommit)
                {
                    if (rowsAffected == 1)
                        sqlTrans.Commit();
                    else
                        sqlTrans.Rollback();
                }
            }
            catch (MySqlException sqlex)
            {
                if (doCommit)
                    sqlTrans.Rollback();
                if (sqlex.Number == 2627)
                {
                    throw new Exception("Log already exists");
                }
                else
                {
                    throw new Exception(sqlex.Message);
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                    sqlTrans.Rollback();

                throw new Exception("Exception: " + ex.Message);
            }

            return rowsAffected;
        }

		/// <summary>
		/// Delete log with specified LogID
		/// </summary>
		/// <param name="LogID">Log Id </param>
		/// <returns>true if succedeed, false otherwise</returns>
		public bool delete(int LogID)
		{
			bool isDeleted = false;
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

			try
			{
				StringBuilder sbDelete = new StringBuilder();
				sbDelete.Append("DELETE FROM log WHERE log_id = " + LogID);
				
				MySqlCommand cmd = new MySqlCommand( sbDelete.ToString(), conn, sqlTrans );
				int res = cmd.ExecuteNonQuery();
				isDeleted = true;
				sqlTrans.Commit();
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback();
				
				throw new Exception("Exception: " + ex.Message);
			}
            
            return isDeleted;
		}

		/// TODO: Check if it is used anywhere
		/// <summary>
		/// Update Log record
		/// </summary>
		/// <param name="logTo"></param>
		/// <param name="doCommit">true - do commit, false  transaction is started elsewhere, 
		/// don't do commit here</param>
		/// <returns>true if succedeed, false otherwise</returns>
		public bool update(LogTO logTo, bool doCommit)
		{
			bool isUpdated = false;
			MySqlTransaction sqlTrans = null;

			if (doCommit)
			{
				sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
			}
			else
			{
				sqlTrans = this.SqlTrans;
			}
			MySqlCommand cmd = null;
			
			try
			{
				StringBuilder sbUpdate = new StringBuilder();

				sbUpdate.Append("UPDATE log SET ");
				sbUpdate.Append("reader_id = " + logTo.ReaderID + ", ");
				sbUpdate.Append("tag_id = " + logTo.TagID + ", ");
				sbUpdate.Append("antenna = " + logTo.Antenna + ", ");
				sbUpdate.Append("event_happened = " + logTo.EventHappened + ", ");
				sbUpdate.Append("action_commited = " + logTo.ActionCommited + ", " );
				sbUpdate.Append("event_time = '" + logTo.EventTime.ToString(dateTimeformat.Replace("'", "")).Trim() + "', ");
				sbUpdate.Append("pass_gen_used = " + logTo.PassGenUsed + ", ");
				sbUpdate.Append("modified_by =N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = NOW() ");
				sbUpdate.Append("WHERE log_id = " + logTo.LogID);
 			
				cmd = new MySqlCommand( sbUpdate.ToString(), conn, sqlTrans);

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
				{
					sqlTrans.Rollback();
				}

				throw new Exception("Exception: " + ex.Message);
			}

			return isUpdated;
		}

		/// <summary>
		/// Find log record with a given ID
		/// </summary>
		/// <param name="LogID">Log ID, primary key</param>
		/// <returns>Log transfer object fulfilled with data form database</returns>
		public LogTO find(int LogID)
		{
			DataSet dataSet = new DataSet();
			LogTO logTO = new LogTO();
			try
			{
				MySqlCommand cmd = new MySqlCommand("SELECT * FROM LOG WHERE LOG_ID = " + LogID, conn);
				MySqlDataAdapter sqlAddapter = new MySqlDataAdapter(cmd);
				sqlAddapter.Fill(dataSet, "Log");
				DataTable table  = dataSet.Tables["Log"];

				if (table.Rows.Count == 1)
				{
					LogTO logTo = new LogTO();
					if (table.Rows[0]["log_id"] != DBNull.Value)
					{
						logTO.LogID = UInt32.Parse(table.Rows[0]["log_id"].ToString().Trim());
					}
					
					if (table.Rows[0]["reader_id"] != DBNull.Value)
					{
						logTO.ReaderID = Int32.Parse(table.Rows[0]["reader_id"].ToString().Trim());
					}
					if (table.Rows[0]["tag_id"] != DBNull.Value)
					{
						logTO.TagID = UInt32.Parse(table.Rows[0]["tag_id"].ToString().Trim());
					}
					if (table.Rows[0]["antenna"] != DBNull.Value)
					{
						logTO.Antenna = Int32.Parse(table.Rows[0]["antenna"].ToString().Trim());
					}
					if (table.Rows[0]["event_happened"] != DBNull.Value)
					{
						logTO.EventHappened = Int32.Parse(table.Rows[0]["event_happened"].ToString().Trim());
					}
					if (table.Rows[0]["action_commited"] != DBNull.Value)
					{
						logTO.ActionCommited = Int32.Parse(table.Rows[0]["action_commited"].ToString().Trim());
					}
					if (table.Rows[0]["event_time"] != DBNull.Value)
					{
						logTO.EventTime = DateTime.Parse( table.Rows[0]["event_time"].ToString());
					}
					if (table.Rows[0]["pass_gen_used"] != DBNull.Value)
					{
						logTO.PassGenUsed = Int32.Parse(table.Rows[0]["pass_gen_used"].ToString().Trim());
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return logTO;
		}

        public List<LogTO> getTrespassLogs(int locationID, int gateID, int readerID, string direction, int employeeID, int eventHappened, DateTime dateFrom, DateTime dateTo)
        {
            DataSet dataSet = new DataSet();
            LogTO memberLogTo = new LogTO();
            List<LogTO> logTOList = new List<LogTO>();
            StringBuilder sb = new StringBuilder();
            string select;

            try
            {
                sb.Append("SELECT l.*, g.name gate, loc.name locName, e.last_name lastName, e.first_name firstName, r.description readerDescription, r.ant_0_location_dir , r.ant_1_location_dir ");
                sb.Append("FROM log l, readers r, tags t, employees e, locations loc, gates g ");

                if ((locationID != -1) ||
                    (gateID!= -1) ||
                    (readerID!= -1) ||
                    (!direction.Trim().Equals("")) ||
                    (employeeID != -1) ||
                    (eventHappened != -1) ||
                    (!dateFrom.Equals(new DateTime())) ||
                    (!dateTo.Equals(new DateTime())))
                {
                    sb.Append("WHERE ");

                    if ((locationID != -1) ||
                    (gateID != -1) ||
                    (readerID != -1) ||
                    (!direction.Trim().Equals("")))
                    {
                        sb.Append("(( ");
                        if (locationID != -1)
                        {
                            sb.Append("r.ant_0_location_id = " + locationID + " AND ");                            
                        }
                        if (gateID != -1)
                        {
                            sb.Append("r.ant_0_gate_id = " + gateID + " AND ");                           
                        }
                        if (readerID != -1)
                        {
                            sb.Append("r.reader_id = " + readerID + " AND ");
                        }
                        if (!direction.Trim().Equals(""))
                        {
                            sb.Append("r.ant_0_location_dir = '" + direction + "' AND ");
                        }
                        sb.Remove(sb.Length-4,4);
                        sb.Append(" AND l.antenna = 0 ) OR (");
                        if (locationID != -1)
                        {
                            sb.Append("r.ant_1_location_id = " + locationID + " AND ");                           
                        }
                        if (gateID != -1)
                        {
                            sb.Append("r.ant_1_gate_id = " + gateID + " AND ");                           
                        }
                        if (readerID != -1)
                        {
                            sb.Append("r.reader_id = " + readerID + " AND ");
                        }
                        if (!direction.Trim().Equals(""))
                        {
                            sb.Append("r.ant_1_location_dir = '" + direction + "' AND ");
                        }
                        sb.Remove(sb.Length - 4, 4);
                        sb.Append(" AND l.antenna = 1 )) AND ");
                    }
                    if (employeeID != -1)
                    {
                        sb.Append("t.owner_id = " + employeeID + " AND ");
                    }
                    if (eventHappened != -1)
                    {
                        sb.Append("l.event_happened = " + eventHappened + " AND ");
                    }
                    else
                    {
                        sb.Append("l.event_happened IN ( 1,3 ) AND ");
                    }
                    if (!dateFrom.Equals(new DateTime()))
                    {
                        sb.Append("l.event_time >= convert('" + dateFrom.ToString("yyyy-MM-dd") + "', datetime) AND ");
                    }
                    if (!dateTo.Equals(new DateTime()))
                    {
                        sb.Append("l.event_time <= convert('" + dateTo.ToString("yyyy-MM-dd") + "', datetime) AND ");
                    }
                }
               
                select = sb.ToString();
                sb.Append("l.reader_id = r.reader_id AND l.tag_id = t.tag_id AND record_id = (SELECT MAX(record_id) FROM tags ta WHERE ta.tag_id = l.tag_id) ");
                sb.Append(" AND t.owner_id = e.employee_id");
                sb.Append("  AND ((r.ant_0_location_id = loc.location_id) OR (r.ant_1_location_id = loc.location_id)) AND ((r.ant_0_gate_id = g.gate_id) OR (r.ant_1_gate_id = g.gate_id))");

                sb.Append("UNION SELECT l.*, g.name gate, loc.name locName, '' lastName, '' firstName, r.description readerDescription, r.ant_0_location_dir , r.ant_1_location_dir ");
                sb.Append("FROM log l, readers r, locations loc, gates g ");

                if ((locationID != -1) ||
                    (gateID != -1) ||
                    (readerID != -1) ||
                    (!direction.Trim().Equals("")) ||
                    (employeeID != -1) ||
                    (eventHappened != -1) ||
                    (!dateFrom.Equals(new DateTime())) ||
                    (!dateTo.Equals(new DateTime())))
                {
                    sb.Append("WHERE ");

                    if ((locationID != -1) ||
                    (gateID != -1) ||
                    (readerID != -1) ||
                    (!direction.Trim().Equals("")))
                    {
                        sb.Append("(( ");
                        if (locationID != -1)
                        {
                            sb.Append("r.ant_0_location_id = " + locationID + " AND ");
                        }
                        if (gateID != -1)
                        {
                            sb.Append("r.ant_0_gate_id = " + gateID + " AND ");
                        }
                        if (readerID != -1)
                        {
                            sb.Append("r.reader_id = " + readerID + " AND ");
                        }
                        if (!direction.Trim().Equals(""))
                        {
                            sb.Append("r.ant_0_location_dir = '" + direction + "' AND ");
                        }
                        sb.Remove(sb.Length - 4, 4);
                        sb.Append(" AND l.antenna = 0 ) OR (");
                        if (locationID != -1)
                        {
                            sb.Append("r.ant_1_location_id = " + locationID + " AND ");
                        }
                        if (gateID != -1)
                        {
                            sb.Append("r.ant_1_gate_id = " + gateID + " AND ");
                        }
                        if (readerID != -1)
                        {
                            sb.Append("r.reader_id = " + readerID + " AND ");
                        }
                        if (!direction.Trim().Equals(""))
                        {
                            sb.Append("r.ant_1_location_dir = '" + direction + "' AND ");
                        }
                        sb.Remove(sb.Length - 4, 4);
                        sb.Append(" AND l.antenna = 1 )) AND ");
                    }
                    if (employeeID != -1)
                    {
                        sb.Append("t.owner_id = " + employeeID + " AND ");
                    }
                    if (eventHappened != -1)
                    {
                        sb.Append("l.event_happened = " + eventHappened + " AND ");
                    }
                    else
                    {
                        sb.Append("l.event_happened IN ( 1 ) AND ");
                    }
                    if (!dateFrom.Equals(new DateTime()))
                    {
                        sb.Append("l.event_time >= convert('" + dateFrom.ToString("yyyy-MM-dd") + "', datetime) AND ");
                    }
                    if (!dateTo.Equals(new DateTime()))
                    {
                        sb.Append("l.event_time <= convert('" + dateTo.ToString("yyyy-MM-dd") + "', datetime) AND ");
                    }
                }

                select = sb.ToString();
                select += "l.reader_id = r.reader_id" ;
                select += "  AND ((r.ant_0_location_id = loc.location_id) OR (r.ant_1_location_id = loc.location_id)) AND ((r.ant_0_gate_id = g.gate_id) OR (r.ant_1_gate_id = g.gate_id))";
                               
                MySqlCommand cmd = new MySqlCommand(select, conn);
				MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(cmd);
				sqlAdapter.Fill(dataSet, "Log");
				DataTable dataTable = dataSet.Tables["Log"];
                if (dataTable.Rows.Count > 0)
				{
					foreach (DataRow row in dataTable.Rows)
					{
						memberLogTo = new LogTO();
						
						// Map value to Log Transfer Object
						if(row["log_id"] != DBNull.Value)
						{
							memberLogTo.LogID = UInt32.Parse(row["log_id"].ToString().Trim());
						}
						if(row["reader_id"] != DBNull.Value)
						{
							memberLogTo.ReaderID = Int32.Parse(row["reader_id"].ToString().Trim());
						}
						if(row["tag_id"] != DBNull.Value)
						{
							memberLogTo.TagID = UInt32.Parse(row["tag_id"].ToString().Trim());
						}
						if(row["antenna"] != DBNull.Value)
						{
							memberLogTo.Antenna= Int32.Parse(row["antenna"].ToString().Trim());
						}
						if(row["event_happened"] != DBNull.Value)
						{
							memberLogTo.EventHappened = Int32.Parse(row["event_happened"].ToString().Trim());
						}
						if(row["action_commited"] != DBNull.Value)
						{
							memberLogTo.ActionCommited = Int32.Parse(row["action_commited"].ToString().Trim());
						}
						if(row["event_time"] != DBNull.Value)
						{
							memberLogTo.EventTime = DateTime.Parse(row["event_time"].ToString());
						}
						if(row["pass_gen_used"] != DBNull.Value)
						{
							memberLogTo.PassGenUsed = Int32.Parse(row["pass_gen_used"].ToString().Trim());
						}
                        if(row["event_happened"] != DBNull.Value)
						{
							memberLogTo.EventHappened = Int32.Parse(row["event_happened"].ToString().Trim());
						}
						if(row["action_commited"] != DBNull.Value)
						{
							memberLogTo.ActionCommited = Int32.Parse(row["action_commited"].ToString().Trim());
						}
						if(row["readerDescription"] != DBNull.Value)
						{
                            memberLogTo.ReaderDescription = row["readerDescription"].ToString().Trim();
						}
						if((row["firstName"] != DBNull.Value)||(row["lastName"] != DBNull.Value))
						{
                            memberLogTo.EmployeeName = row["lastName"].ToString().Trim()+ " " + row["firstName"].ToString().Trim();
						}
                        if(row["locName"] != DBNull.Value)
						{
							memberLogTo.Location =  row["locName"].ToString().Trim();
						}
                        if(row["antenna"] != DBNull.Value)
						{
                            if((int.Parse(row["antenna"].ToString().Trim()) == 0)&&(row["ant_0_location_dir"] != DBNull.Value))
                            {                               
							  memberLogTo.Direction=  row["ant_0_location_dir"].ToString().Trim();                              
                            }
                            if((int.Parse(row["antenna"].ToString().Trim()) == 1)&&(row["ant_1_location_dir"] != DBNull.Value))
                            {                               
							  memberLogTo.Direction=  row["ant_1_location_dir"].ToString().Trim();                              
                            }
						}
                        if(row["gate"] != DBNull.Value)
						{
							memberLogTo.Gate =  row["gate"].ToString().Trim();
						}
						
						// Add Log Transfer Object to List
						logTOList.Add(memberLogTo);
					}
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

			return logTOList;
        }

		/// <summary>
		/// Get all records from Log table that fulfill the criteria 
		/// </summary>
		/// <param name="logTo">Contain filter data</param>
		/// <returns>return ArrayList of Log transmission objects</returns>
		public List<LogTO> getLogs(LogTO logTo)
		{
			DataSet dataSet = new DataSet();
			LogTO memberLogTo = new LogTO();
            List<LogTO> logTOList = new List<LogTO>();
			StringBuilder sb = new StringBuilder();
			string select;

			try
			{
				sb.Append("SELECT * FROM log ");

				if (( logTo.LogID != -1) || (logTo.ReaderID != -1) || (logTo.TagID > 0) || 
					(logTo.Antenna != -1) || (logTo.EventHappened != -1) || (logTo.ActionCommited != -1) ||
					(!logTo.EventTime.Equals(new DateTime())) || (logTo.PassGenUsed != -1))
				{
					sb.Append("WHERE ");

					if (logTo.LogID != -1)
					{
						sb.Append("log_id  = " + logTo.LogID + " AND ");
					}
					if (logTo.ReaderID != -1)
					{
						sb.Append("reader_id  = " + logTo.ReaderID  + " AND ");
					}
					if (logTo.TagID > 0)
					{
						sb.Append("tag_id  = " + logTo.TagID  + " AND ");
					}
					if (logTo.Antenna != -1)
					{
						sb.Append("antenna  = " + logTo.Antenna  + " AND ");
					}
					if (logTo.EventHappened != -1)
					{
						sb.Append("event_happened  = " + logTo.EventHappened  + " AND ");
					}
					if (logTo.ActionCommited != -1)
					{
						sb.Append("action_commited  = " + logTo.ActionCommited  + " AND ");
					}
					if (!logTo.EventTime.Equals(new DateTime()))
					{
						sb.Append("event_time  = '" + logTo.EventTime.ToString(dateTimeformat) + "' AND ");
					}
					if (logTo.PassGenUsed != -1)
					{
						sb.Append("pass_gen_used  = " + logTo.PassGenUsed + " AND ");
					}

					select = sb.ToString(0, sb.ToString().Length - 4);
					
				}
				else
				{
					select = sb.ToString();
				}
				
				select += "ORDER BY log_id ASC";

				MySqlCommand cmd = new MySqlCommand(select, conn);
				MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(cmd);
				sqlAdapter.Fill(dataSet, "Log");
				DataTable dataTable = dataSet.Tables["Log"];


				if (dataTable.Rows.Count > 0)
				{
					foreach (DataRow row in dataTable.Rows)
					{
						memberLogTo = new LogTO();
						
						// Map value to Log Transfer Object
						if(row["log_id"] != DBNull.Value)
						{
							memberLogTo.LogID = UInt32.Parse(row["log_id"].ToString().Trim());
						}
						if(row["reader_id"] != DBNull.Value)
						{
							memberLogTo.ReaderID = Int32.Parse(row["reader_id"].ToString().Trim());
						}
						if(row["tag_id"] != DBNull.Value)
						{
							memberLogTo.TagID = UInt32.Parse(row["tag_id"].ToString().Trim());
						}
						if(row["antenna"] != DBNull.Value)
						{
							memberLogTo.Antenna= Int32.Parse(row["antenna"].ToString().Trim());
						}
						if(row["event_happened"] != DBNull.Value)
						{
							memberLogTo.EventHappened = Int32.Parse(row["event_happened"].ToString().Trim());
						}
						if(row["action_commited"] != DBNull.Value)
						{
							memberLogTo.ActionCommited = Int32.Parse(row["action_commited"].ToString().Trim());
						}
						if(row["event_time"] != DBNull.Value)
						{
							memberLogTo.EventTime = DateTime.Parse( row["event_time"].ToString());
						}
						if(row["pass_gen_used"] != DBNull.Value)
						{
							memberLogTo.PassGenUsed = Int32.Parse(row["pass_gen_used"].ToString().Trim());
						}
						
						// Add Log Transfer Object to List
						logTOList.Add(memberLogTo);
					}

				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return logTOList;
		}

        public List<LogTO> getLogsForPeriod(LogTO logTO, DateTime from, DateTime to)
        {
            DataSet dataSet = new DataSet();
            LogTO memberLogTo = new LogTO();
            List<LogTO> logTOList = new List<LogTO>();
            StringBuilder sb = new StringBuilder();
            CultureInfo ci = CultureInfo.InvariantCulture;
            string select;

            try
            {             
                sb.Append("SELECT * FROM log ");

                if ((logTO.ReaderID != -1) ||
                    (logTO.TagID != 0) ||
                    (logTO.Antenna != -1) ||
                    (logTO.PassGenUsed != -1))
                {
                    sb.Append("WHERE ");

                    if (logTO.ReaderID != -1)
                    {
                        sb.Append(" reader_id = '" + logTO.ReaderID.ToString().Trim() + "' AND");
                    }
                    if (logTO.TagID != 0)
                    {
                        sb.Append(" tag_id = '" + logTO.TagID.ToString().Trim() + "' AND");
                    }
                    if (logTO.Antenna != -1)
                    {
                        sb.Append(" antenna = '" + logTO.Antenna.ToString().Trim() + "' AND");
                    }
                    if (logTO.PassGenUsed != -1)
                    {
                        sb.Append(" pass_gen_used = '" + logTO.PassGenUsed.ToString().Trim() + "' AND");
                    }
                    sb.Append(" event_time >= CONVERT('" + from.ToString("yyyy-MM-dd HH:mm:ss") + "', datetime) AND");
                    sb.Append(" event_time <= CONVERT('" + to.ToString("yyyy-MM-dd HH:mm:ss") + "', datetime) AND");

                    select = sb.ToString(0, sb.ToString().Length - 3);

                }
                else
                {
                    select = sb.ToString();
                }

                select += "ORDER BY log_id ASC";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(cmd);
                sqlAdapter.Fill(dataSet, "Log");
                DataTable dataTable = dataSet.Tables["Log"];


                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        memberLogTo = new LogTO();

                        // Map value to Log Transfer Object
                        if (row["log_id"] != DBNull.Value)
                        {
                            memberLogTo.LogID = UInt32.Parse(row["log_id"].ToString().Trim());
                        }
                        if (row["reader_id"] != DBNull.Value)
                        {
                            memberLogTo.ReaderID = Int32.Parse(row["reader_id"].ToString().Trim());
                        }
                        if (row["tag_id"] != DBNull.Value)
                        {
                            memberLogTo.TagID = UInt32.Parse(row["tag_id"].ToString().Trim());
                        }
                        if (row["antenna"] != DBNull.Value)
                        {
                            memberLogTo.Antenna = Int32.Parse(row["antenna"].ToString().Trim());
                        }
                        if (row["event_happened"] != DBNull.Value)
                        {
                            memberLogTo.EventHappened = Int32.Parse(row["event_happened"].ToString().Trim());
                        }
                        if (row["action_commited"] != DBNull.Value)
                        {
                            memberLogTo.ActionCommited = Int32.Parse(row["action_commited"].ToString().Trim());
                        }
                        if (row["event_time"] != DBNull.Value)
                        {
                            memberLogTo.EventTime = DateTime.Parse(row["event_time"].ToString());
                        }
                        if (row["pass_gen_used"] != DBNull.Value)
                        {
                            memberLogTo.PassGenUsed = Int32.Parse(row["pass_gen_used"].ToString().Trim());
                        }

                        // Add Log Transfer Object to List
                        logTOList.Add(memberLogTo);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return logTOList;
        }

		/// <summary>
		/// Get all records from Log table that fulfill the criteria 
		/// </summary>
		/// <param name="logID"></param>
		/// <param name="readerID"></param>
		/// <param name="tagID"></param>
		/// <param name="antenna"></param>
		/// <param name="eventHappened"></param>
		/// <param name="actionCommited"></param>
		/// <param name="eventTime"></param>
		/// <param name="passGenUsed"></param>
		/// <returns></returns>
		public List<LogTO> getLogs(string logID, string readerID, string tagID, string antenna, string eventHappened,
			string actionCommited, string  eventTime, string passGenUsed)
		{
			DataSet dataSet = new DataSet();
			LogTO memberLogTo = new LogTO();
            List<LogTO> logTOList = new List<LogTO>();
			StringBuilder sb = new StringBuilder();
            CultureInfo ci = CultureInfo.InvariantCulture;
			string select;

			try
			{
				sb.Append("SELECT * FROM log ");

				if ((!logID.Trim().Equals("")) || 
					(!readerID.Trim().Equals("")) ||
					(!tagID.Trim().Equals("")) || 
					(!antenna.Trim().Equals("")) || 
					(!eventHappened.Trim().Equals("")) || 
					(!actionCommited.Trim().Equals("")) ||
					(!eventTime.Trim().Equals("")) || 
					(!passGenUsed.Trim().Equals("")))
				{
					sb.Append("WHERE ");

					if (!logID.Trim().Equals(""))
					{
						sb.Append(" UPPER(log_id) LIKE '" + logID.Trim().ToUpper() + "' AND");
					}
					if (!readerID.Trim().Equals(""))
					{
						sb.Append(" UPPER(reader_id) LIKE '" + readerID.Trim().ToUpper() + "' AND");
					}
					if (!tagID.Trim().Equals(""))
					{
						sb.Append(" UPPER(tag_id) LIKE '" + tagID.Trim().ToUpper() + "' AND");
					}
					if (!antenna.Trim().Equals(""))
					{
						sb.Append(" UPPER(antenna) LIKE '" + antenna.Trim().ToUpper() + "' AND");
					}
					if (!eventHappened.Trim().Equals(""))
					{
						sb.Append(" UPPER(event_happened) LIKE '" + eventHappened.Trim().ToUpper() + "' AND");
					}
					if (!actionCommited.Trim().Equals(""))
					{
						sb.Append(" UPPER(action_commited) LIKE '" + actionCommited.Trim().ToUpper() + "' AND");
					}
					if (!eventTime.Trim().Equals(""))
					{
                        sb.Append(" DATE_FORMAT(event_time,'%Y-%m-%dT%h:%m:%s') >='" + (DateTime.Parse(eventTime)).ToString(dateTimeformat, ci).Trim() + "' AND");
					}
					if (!passGenUsed.Trim().Equals(""))
					{
						sb.Append(" UPPER(pass_gen_used) LIKE '" + passGenUsed.Trim().ToUpper() + "' AND");
					}

					select = sb.ToString(0, sb.ToString().Length - 3);
					
				}
				else
				{
					select = sb.ToString();
				}
				
				select += "ORDER BY log_id ASC";

				MySqlCommand cmd = new MySqlCommand(select, conn);
				MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(cmd);
				sqlAdapter.Fill(dataSet, "Log");
				DataTable dataTable = dataSet.Tables["Log"];


				if (dataTable.Rows.Count > 0)
				{
					foreach (DataRow row in dataTable.Rows)
					{
						memberLogTo = new LogTO();
						
						// Map value to Log Transfer Object
						if(row["log_id"] != DBNull.Value)
						{
							memberLogTo.LogID = UInt32.Parse(row["log_id"].ToString().Trim());
						}
						if(row["reader_id"] != DBNull.Value)
						{
							memberLogTo.ReaderID = Int32.Parse(row["reader_id"].ToString().Trim());
						}
						if(row["tag_id"] != DBNull.Value)
						{
							memberLogTo.TagID = UInt32.Parse(row["tag_id"].ToString().Trim());
						}
						if(row["antenna"] != DBNull.Value)
						{
							memberLogTo.Antenna= Int32.Parse(row["antenna"].ToString().Trim());
						}
						if(row["event_happened"] != DBNull.Value)
						{
							memberLogTo.EventHappened = Int32.Parse(row["event_happened"].ToString().Trim());
						}
						if(row["action_commited"] != DBNull.Value)
						{
							memberLogTo.ActionCommited = Int32.Parse(row["action_commited"].ToString().Trim());
						}
						if(row["event_time"] != DBNull.Value)
						{
							memberLogTo.EventTime = DateTime.Parse( row["event_time"].ToString());
						}
						if(row["pass_gen_used"] != DBNull.Value)
						{
							memberLogTo.PassGenUsed = Int32.Parse(row["pass_gen_used"].ToString().Trim());
						}
						
						// Add Log Transfer Object to List
						logTOList.Add(memberLogTo);
					}

				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return logTOList;
		}

        public List<LogTO> getLogIn(int employeeID, DateTime date)
        {
            DataSet dataSet = new DataSet();
            LogTO memberLogTo = new LogTO();
            List<LogTO> logTOList = new List<LogTO>();
            StringBuilder sb = new StringBuilder();
            string select;

            try
            {
                sb.Append("SELECT l.* FROM log l, tags t, readers r, employees e ");


                sb.Append("WHERE ");

                sb.Append("l.tag_id = t.tag_id AND ");
                sb.Append("t.owner_id = e.employee_id AND ");
                sb.Append("l.reader_id = r.reader_id AND ");
                sb.Append("e.employee_id =" + employeeID + " AND ");
                sb.Append("((l.antenna = 0 AND r.ant_0_location_dir = 'IN') OR ");
                sb.Append("(l.antenna = 1 AND r.ant_1_location_dir = 'IN')) AND ");
                sb.Append(" l.event_time >= convert('" + date.ToString("yyyy-MM-dd") + "', datetime) AND ");
                sb.Append(" l.event_time <= convert('" + date.ToString("yyyy-MM-dd") + "', datetime) ");
                select = sb.ToString();

                select += " ORDER BY log_id ASC";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(cmd);
                sqlAdapter.Fill(dataSet, "Log");
                DataTable dataTable = dataSet.Tables["Log"];
                
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        memberLogTo = new LogTO();

                        // Map value to Log Transfer Object
                        if (row["log_id"] != DBNull.Value)
                        {
                            memberLogTo.LogID = UInt32.Parse(row["log_id"].ToString().Trim());
                        }
                        if (row["reader_id"] != DBNull.Value)
                        {
                            memberLogTo.ReaderID = Int32.Parse(row["reader_id"].ToString().Trim());
                        }
                        if (row["tag_id"] != DBNull.Value)
                        {
                            memberLogTo.TagID = UInt32.Parse(row["tag_id"].ToString().Trim());
                        }
                        if (row["antenna"] != DBNull.Value)
                        {
                            memberLogTo.Antenna = Int32.Parse(row["antenna"].ToString().Trim());
                        }
                        if (row["event_happened"] != DBNull.Value)
                        {
                            memberLogTo.EventHappened = Int32.Parse(row["event_happened"].ToString().Trim());
                        }
                        if (row["action_commited"] != DBNull.Value)
                        {
                            memberLogTo.ActionCommited = Int32.Parse(row["action_commited"].ToString().Trim());
                        }
                        if (row["event_time"] != DBNull.Value)
                        {
                            memberLogTo.EventTime = DateTime.Parse(row["event_time"].ToString());
                        }
                        if (row["pass_gen_used"] != DBNull.Value)
                        {
                            memberLogTo.PassGenUsed = Int32.Parse(row["pass_gen_used"].ToString().Trim());
                        }

                        // Add Log Transfer Object to List
                        logTOList.Add(memberLogTo);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return logTOList;
        }

		/// <summary>
		/// Gets record from Log table that represent 
		/// two events and have pass_gen_used with a given value.
		/// For example, this methods is used in logs processing and populating 
		/// passes table.
		/// </summary>
		/// <param name="event1">event_happened value</param>
		/// <param name="event2">event_happened value</param>
		/// <param name="passGenUsed">pass_gen_used_value</param>
		/// <returns></returns>
        public Dictionary<long, LogTO> getLogs(int event1, int event2, int passGenUsed)
		{
            DataSet dataSet = new DataSet();
            DataSet addDataSet = new DataSet();
            LogTO memberLogTo = new LogTO();
            LogAdditionalInfoTO memberLogAddTo = new LogAdditionalInfoTO();
            Dictionary<long, LogTO> logTOList = new Dictionary<long, LogTO>();
            StringBuilder sb = new StringBuilder();

			try
			{
				sb.Append("SELECT * FROM log WHERE ");
				sb.Append("( event_happened = " + event1 + " OR ");
				sb.Append("event_happened = " + event2 + ") AND ");
				sb.Append("pass_gen_used = " + passGenUsed + " ");
				sb.Append("ORDER BY log_id ASC");

				MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
				MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(cmd);
				sqlAdapter.Fill(dataSet, "Log");
				DataTable dataTable = dataSet.Tables["Log"];

                string ids = "";
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        memberLogTo = new LogTO();

                        // Map value to Log Transfer Object
                        if (row["log_id"] != DBNull.Value)
                        {
                            memberLogTo.LogID = UInt32.Parse(row["log_id"].ToString().Trim());
                        }
                        if (row["reader_id"] != DBNull.Value)
                        {
                            memberLogTo.ReaderID = Int32.Parse(row["reader_id"].ToString().Trim());
                        }
                        if (row["tag_id"] != DBNull.Value)
                        {
                            memberLogTo.TagID = UInt32.Parse(row["tag_id"].ToString().Trim());
                        }
                        if (row["antenna"] != DBNull.Value)
                        {
                            memberLogTo.Antenna = Int32.Parse(row["antenna"].ToString().Trim());
                        }
                        if (row["event_happened"] != DBNull.Value)
                        {
                            memberLogTo.EventHappened = Int32.Parse(row["event_happened"].ToString().Trim());
                        }
                        if (row["action_commited"] != DBNull.Value)
                        {
                            memberLogTo.ActionCommited = Int32.Parse(row["action_commited"].ToString().Trim());
                        }
                        if (row["event_time"] != DBNull.Value)
                        {
                            memberLogTo.EventTime = (DateTime)row["event_time"];
                        }
                        if (row["pass_gen_used"] != DBNull.Value)
                        {
                            memberLogTo.PassGenUsed = Int32.Parse(row["pass_gen_used"].ToString().Trim());
                        }

                        // Add Log Transfer Object to List
                        if (!logTOList.ContainsKey(memberLogTo.LogID))
                        {
                            logTOList.Add(memberLogTo.LogID, memberLogTo);
                            ids += memberLogTo.LogID.ToString().Trim() + ",";
                        }
                    }
                }

                if (ids.Length > 0)
                {
                    // get additional info records
                    MySqlCommand addCmd = new MySqlCommand("SELECT * FROM log_additional_info WHERE log_id IN (" + ids.Substring(0, ids.Length - 1) + ")", conn);
                    MySqlDataAdapter addSqlAdapter = new MySqlDataAdapter(addCmd);
                    addSqlAdapter.Fill(addDataSet, "LogAdd");
                    DataTable addDataTable = addDataSet.Tables["LogAdd"];

                    foreach (DataRow row in addDataTable.Rows)
                    {
                        memberLogAddTo = new LogAdditionalInfoTO();

                        // Map value to Log Transfer Object
                        if (row["log_id"] != DBNull.Value)
                        {
                            memberLogAddTo.LogID = UInt32.Parse(row["log_id"].ToString().Trim());
                        }
                        if (row["gps_data"] != DBNull.Value)
                        {
                            memberLogAddTo.GpsData = row["gps_data"].ToString().Trim();
                        }
                        if (row["cardholder_name"] != DBNull.Value)
                        {
                            memberLogAddTo.CardholderName = row["cardholder_name"].ToString().Trim();
                        }
                        if (row["cardholder_id"] != DBNull.Value)
                        {
                            memberLogAddTo.CardholderID = Int32.Parse(row["cardholder_id"].ToString().Trim());
                        }

                        // Set Log Additional Info Transfer Object to Logs found
                        if (logTOList.ContainsKey(memberLogAddTo.LogID))
                            logTOList[memberLogAddTo.LogID].AddTO = memberLogAddTo;
                    }
                }
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return logTOList;
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
					Stream stream = File.OpenRead(filePath);
					XmlSerializer bformatter = new XmlSerializer(typeof(LogTO[]));
					LogTO[] deserialized;

					try
					{
						deserialized = (LogTO[]) bformatter.Deserialize(stream);
						ArrayList logs = ArrayList.Adapter(deserialized);

                        foreach (LogTO l in logs)
                        {
                            logList.Add(l);
                        }
					}
					catch(Exception ex)
					{
						stream.Close();
						throw new DataProcessingException("File: " + filePath + " " + ex.Message + "\n", 3);
					}
					
					stream.Close();
				}
			}
			catch(IOException ioEx)
			{
				throw new DataProcessingException(ioEx + " File: " + filePath, 2);
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
                        throw new DataProcessingException("File: " + filePath + " " + ex.Message + "\n", 3);
                    }

                    stream.Close();
                }
            }
            catch (IOException ioEx)
            {
                throw new DataProcessingException(ioEx + " File: " + filePath, 2);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return logList;
        }
        
		/// <summary>
		/// Insert data from logTo object into LogTmp table
		/// </summary>
		/// <param name="logTo">Log Transfer Object</param>
		/// <returns>Number of affected rows</returns>
		public int insertToTmp(LogTO logTo)
		{
			int rowsAffected = 0;
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO log_tmp ");
				sbInsert.Append("(reader_id, tag_id, antenna, event_happened, action_commited, " 
					+ "event_time) ");			
				sbInsert.Append("VALUES (" 
					+ logTo.ReaderID + ", " + logTo.TagID + ", " + logTo.Antenna + ", " 
					+ logTo.EventHappened + ", " + logTo.ActionCommited + ", '" 
					+ logTo.EventTime.ToString(dateTimeformat.Replace("'", "")).Trim() + "') ");
					
				MySqlCommand cmd = new MySqlCommand( sbInsert.ToString(), conn, sqlTrans );
				rowsAffected = cmd.ExecuteNonQuery();
				sqlTrans.Commit();
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback();
				throw new Exception("Exception: " + ex.Message);
			}

			return rowsAffected;
		}

        /// <summary>
        /// Insert data from logTo object into LogTmpAdditionalInfo table
        /// </summary>
        /// <param name="logTo">LogTmpAdditionalInfo Transfer Object</param>
        /// <returns>Number of affected rows</returns>
        public int insertToMobileTmp(LogTmpAdditionalInfoTO logTmpTO)
        {
            int rowsAffected = 0;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO log_tmp_additional_info ");
                sbInsert.Append("(reader_id, tag_id, antenna, event_happened, action_commited, event_time, gps_data, cardholder_name, cardholder_id) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("'" + logTmpTO.ReaderID.ToString().Trim() + "', ");
                sbInsert.Append("'" + logTmpTO.TagID.ToString().Trim() + "', ");
                sbInsert.Append("'" + logTmpTO.Antenna.ToString().Trim() + "', ");
                if (logTmpTO.EventHappened != -1)
                    sbInsert.Append("'" + logTmpTO.EventHappened.ToString().Trim() + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (logTmpTO.ActionCommited != -1)
                    sbInsert.Append("'" + logTmpTO.ActionCommited.ToString().Trim() + "', ");
                else
                    sbInsert.Append("NULL, ");
                sbInsert.Append("'" + logTmpTO.EventTime.ToString(dateTimeformat) + "', ");
                sbInsert.Append("'" + logTmpTO.GpsData.Trim() + "', ");
                sbInsert.Append("'" + logTmpTO.CardholderName.Trim() + "', ");
                sbInsert.Append("'" + logTmpTO.CardholderID.ToString().Trim() + "') ");

                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw new Exception("Exception: " + ex.Message);
            }

            return rowsAffected;
        }

		/// <summary>
		/// Delete all records from log_tmp table
		/// </summary>
		/// <returns></returns>
		public bool clearLogTmp()
		{
			bool isDeleted = false;
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("DELETE FROM log_tmp");

				MySqlCommand cmd = new MySqlCommand( sb.ToString(), conn, sqlTrans );
				cmd.ExecuteNonQuery();
				sqlTrans.Commit();
				isDeleted = true;
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback();
				throw ex;
			}

			return isDeleted;
		}

        /// <summary>
        /// Delete all records from log_tmp_additional_info table
        /// </summary>
        /// <returns></returns>
        public bool clearLogMobileTmp()
        {
            bool isDeleted = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("DELETE FROM log_tmp_additional_info");

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn, sqlTrans);
                cmd.ExecuteNonQuery();
                sqlTrans.Commit();
                isDeleted = true;
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw ex;
            }

            return isDeleted;
        }

		public int importLog()
		{
			int rowsAffected = 0;
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

			try
			{
			
				StringBuilder sb = new StringBuilder();
				sb.Append("INSERT INTO log ");
				sb.Append("(reader_id,tag_id,antenna,event_happened,action_commited,event_time, created_by, created_time  ) ");
				sb.Append("select distinct ");
				sb.Append("log_tmp.reader_id, ");
				sb.Append("log_tmp.tag_id, ");
				sb.Append("log_tmp.antenna, ");
				sb.Append("log_tmp.event_happened, ");
				sb.Append("log_tmp.action_commited, ");
				sb.Append("log_tmp.event_time,  ");
				sb.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW() ");
				sb.Append(@"from log_tmp,tags  ");
				sb.Append("where  ");
				sb.Append("not exists  ");
				sb.Append("( ");
				sb.Append("select reader_id  ");
				sb.Append("from log  ");
				sb.Append("where ");
				//sb.Append("log_tmp.reader_id = log.reader_id ");
				//sb.Append("AND ");
				sb.Append("log_tmp.tag_id = log.tag_id ");
				sb.Append("AND ");
				//sb.Append("log_tmp.antenna = log.antenna ");
				//sb.Append("AND ");
                sb.Append("log_tmp.event_time = log.event_time)"); 

				MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn, sqlTrans);
				rowsAffected = cmd.ExecuteNonQuery();
				sqlTrans.Commit();
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback();
				throw ex;
			}

			return rowsAffected;
		}

        public List<LogTmpAdditionalInfoTO> getLogMobileTmp()
        {
            List<LogTmpAdditionalInfoTO> tmpList = new List<LogTmpAdditionalInfoTO>();
            LogTmpAdditionalInfoTO log = new LogTmpAdditionalInfoTO();
            DataSet dataSet = new DataSet();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT ");
                sb.Append("log_tmp_additional_info.reader_id, ");
                sb.Append("log_tmp_additional_info.tag_id, ");
                sb.Append("log_tmp_additional_info.antenna, ");
                sb.Append("log_tmp_additional_info.event_happened, ");
                sb.Append("log_tmp_additional_info.action_commited, ");
                sb.Append("log_tmp_additional_info.event_time, ");
                sb.Append("log_tmp_additional_info.gps_data, ");
                sb.Append("log_tmp_additional_info.cardholder_name, ");
                sb.Append("log_tmp_additional_info.cardholder_id ");
                sb.Append("FROM log_tmp_additional_info ");
                sb.Append("WHERE  ");
                sb.Append("NOT EXISTS  ");
                sb.Append("( ");
                sb.Append("SELECT reader_id  ");
                sb.Append("FROM log  ");
                sb.Append("WHERE ");
                //sb.Append("log_tmp.reader_id = log.reader_id ");
                //sb.Append("AND ");
                sb.Append("log_tmp_additional_info.tag_id = log.tag_id ");
                sb.Append("AND ");
                //sb.Append("log_tmp.antenna = log.antenna ");
                //sb.Append("AND ");
                sb.Append("log_tmp_additional_info.event_time = log.event_time)");

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(cmd);
                sqlAdapter.Fill(dataSet, "LogTmp");
                DataTable dataTable = dataSet.Tables["LogTmp"];

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        log = new LogTmpAdditionalInfoTO();

                        // Map value to Log Transfer Object
                        if (row["reader_id"] != DBNull.Value)
                        {
                            log.ReaderID = Int32.Parse(row["reader_id"].ToString().Trim());
                        }
                        if (row["tag_id"] != DBNull.Value)
                        {
                            log.TagID = UInt32.Parse(row["tag_id"].ToString().Trim());
                        }
                        if (row["antenna"] != DBNull.Value)
                        {
                            log.Antenna = Int32.Parse(row["antenna"].ToString().Trim());
                        }
                        if (row["event_happened"] != DBNull.Value)
                        {
                            log.EventHappened = Int32.Parse(row["event_happened"].ToString().Trim());
                        }
                        if (row["action_commited"] != DBNull.Value)
                        {
                            log.ActionCommited = Int32.Parse(row["action_commited"].ToString().Trim());
                        }
                        if (row["event_time"] != DBNull.Value)
                        {
                            log.EventTime = (DateTime)row["event_time"];
                        }
                        if (row["gps_data"] != DBNull.Value)
                        {
                            log.GpsData = row["gps_data"].ToString().Trim();
                        }
                        if (row["cardholder_name"] != DBNull.Value)
                        {
                            log.CardholderName = row["cardholder_name"].ToString().Trim();
                        }
                        if (row["cardholder_id"] != DBNull.Value)
                        {
                            log.CardholderID = Int32.Parse(row["cardholder_id"].ToString().Trim());
                        }
                        log.PassGenUsed = (int)Constants.PassGenUsed.Unused;
                        // Add Log Transfer Object to List
                        tmpList.Add(log);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tmpList;
        }

		/// <summary>
		/// Update Log record
		/// </summary>
		/// <param name="logTo"></param>
		/// <param name="doCommit">true - do commit, false  transaction is started elsewhere, 
		/// don't do commit here</param>
		/// <returns>true if succedeed, false otherwise</returns>
		public bool updateAsUsed(LogTO logTo, bool doCommit)
		{
			bool isUpdated = false;
			MySqlTransaction sqlTrans = null;			
			sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
						
			MySqlCommand cmd = null;
			
			try
			{
				StringBuilder sbUpdate = new StringBuilder();

				sbUpdate.Append("UPDATE log SET ");
				sbUpdate.Append("pass_gen_used = " + logTo.PassGenUsed + ", ");
				sbUpdate.Append("modified_by =N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = NOW() ");
				sbUpdate.Append("WHERE log_id = " + logTo.LogID);
 			
				cmd = new MySqlCommand( sbUpdate.ToString(), conn, sqlTrans);

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
				{
					sqlTrans.Rollback();
				}

				DataProcessingException dpException = new DataProcessingException(ex.Message + logTo.ToString(), 10);				
				throw dpException;
			}
			return isUpdated;
		}

        public List<LogTO> getLogsForGraph(DateTime dateFrom, DateTime dateTo, DateTime timeFrom, DateTime timeTo, string readerID)
        {
            DataSet dataSet = new DataSet();
            LogTO memberLogTo = new LogTO();
            List<LogTO> logTOList = new List<LogTO>();
            StringBuilder sb = new StringBuilder();
            string select;

            try
            {
                sb.Append("SELECT * FROM log ");
                sb.Append("WHERE tag_id = 0 AND antenna < 256  AND action_commited <= 4");

                if (!dateFrom.Equals(new DateTime(0)) && !dateTo.Equals(new DateTime(0)))
                {                   
                    sb.Append(" AND event_time >= CONVERT('" + dateFrom.ToString("yyyy-MM-dd") + "', datetime) AND ");
                    sb.Append(" event_time <= CONVERT('" + dateTo.AddDays(1).ToString("yyyy-MM-dd") + "', datetime) ");
				}
                if (!timeFrom.Equals(new DateTime()) && !timeTo.Equals(new DateTime()))
                {

                    sb.Append(" AND event_time >= CONVERT('" + timeFrom.ToString("yyyy-MM-dd HH:mm:ss") + "', datetime) AND ");
                    sb.Append(" event_time <= CONVERT('" + timeTo.ToString("yyyy-MM-dd HH:mm:ss") + "', datetime) ");
                }
                if (!readerID.Equals(""))
                {
                    sb.Append(" AND reader_id IN (" + readerID + ") ");
                }
                    select = sb.ToString();
               

                select += "ORDER BY reader_id,event_time ASC,action_commited DESC";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(cmd);
                sqlAdapter.Fill(dataSet, "Log");
                DataTable dataTable = dataSet.Tables["Log"];


                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        memberLogTo = new LogTO();

                        // Map value to Log Transfer Object
                        if (row["log_id"] != DBNull.Value)
                        {
                            memberLogTo.LogID = UInt32.Parse(row["log_id"].ToString().Trim());
                        }
                        if (row["reader_id"] != DBNull.Value)
                        {
                            memberLogTo.ReaderID = Int32.Parse(row["reader_id"].ToString().Trim());
                        }                       
                        if (row["action_commited"] != DBNull.Value)
                        {
                            memberLogTo.ActionCommited = Int32.Parse(row["action_commited"].ToString().Trim());
                        }
                        if (row["event_time"] != DBNull.Value)
                        {
                            memberLogTo.EventTime = DateTime.Parse(row["event_time"].ToString());
                        }
                        if (row["antenna"] != DBNull.Value)
                        {
                            memberLogTo.Antenna = Int32.Parse(row["antenna"].ToString().Trim());
                        }
                        if (row["event_happened"] != DBNull.Value)
                        {
                            memberLogTo.EventHappened = Int32.Parse(row["event_happened"].ToString().Trim());
                        }
                        // Add Log Transfer Object to List
                        logTOList.Add(memberLogTo);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return logTOList;
        }

        public List<LogTO> getLogsForReader(int readerID)
        {
            DataSet dataSet = new DataSet();
            LogTO memberLogTo = new LogTO();
            List<LogTO> logTOList = new List<LogTO>();
            
            string select;

            try
            {
                select = "SELECT * FROM log WHERE reader_id = '" + readerID.ToString().Trim() + "' ORDER BY event_time DESC LIMIT " + Constants.maxLogsForReaderNum.ToString().Trim();

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(cmd);
                sqlAdapter.Fill(dataSet, "Log");
                DataTable dataTable = dataSet.Tables["Log"];

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        memberLogTo = new LogTO();

                        // Map value to Log Transfer Object
                        if (row["log_id"] != DBNull.Value)
                        {
                            memberLogTo.LogID = UInt32.Parse(row["log_id"].ToString().Trim());
                        }
                        if (row["tag_id"] != DBNull.Value)
                        {
                            memberLogTo.TagID = uint.Parse(row["tag_id"].ToString().Trim());
                        }
                        if (row["reader_id"] != DBNull.Value)
                        {
                            memberLogTo.ReaderID = Int32.Parse(row["reader_id"].ToString().Trim());
                        }
                        if (row["action_commited"] != DBNull.Value)
                        {
                            memberLogTo.ActionCommited = Int32.Parse(row["action_commited"].ToString().Trim());
                        }
                        if (row["event_time"] != DBNull.Value)
                        {
                            memberLogTo.EventTime = DateTime.Parse(row["event_time"].ToString());
                        }
                        if (row["antenna"] != DBNull.Value)
                        {
                            memberLogTo.Antenna = Int32.Parse(row["antenna"].ToString().Trim());
                        }
                        if (row["event_happened"] != DBNull.Value)
                        {
                            memberLogTo.EventHappened = Int32.Parse(row["event_happened"].ToString().Trim());
                        }
                        if (row["pass_gen_used"] != DBNull.Value)
                        {
                            memberLogTo.PassGenUsed = int.Parse(row["pass_gen_used"].ToString().Trim());
                        }
                        // Add Log Transfer Object to List
                        logTOList.Add(memberLogTo);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return logTOList;
        }

        public bool deleteLogs(DateTime dateFrom, DateTime dateTo, DateTime timeFrom, DateTime timeTo, string readerID)
        {
            bool isDeleted = false;
            MySqlTransaction trans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("DELETE FROM log");

                if (!dateFrom.Equals(new DateTime(0)) && !dateTo.Equals(new DateTime(0)))
                {
                    sb.Append(" WHERE event_time >= CONVERT('" + dateFrom.ToString("yyyy-MM-dd") + "', datetime) AND ");
                    sb.Append(" event_time < CONVERT('" + dateTo.AddDays(1).ToString("yyyy-MM-dd") + "', datetime) ");
                }
                if (!timeFrom.Equals(new DateTime()) && !timeTo.Equals(new DateTime()))
                {

                    sb.Append(" AND event_time >= CONVERT('" + timeFrom.ToString("yyyy-MM-dd HH:mm:ss") + "', datetime) AND ");
                    sb.Append(" event_time < CONVERT('" + timeTo.ToString("yyyy-MM-dd HH:mm:ss") + "', datetime) ");
                }
                if (!readerID.Equals(""))
                {
                    sb.Append(" AND reader_id IN (" + readerID + ") ");
                }

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn, trans);
                int res = cmd.ExecuteNonQuery();
                if (res <= 0)
                {
                    isDeleted = false;
                }
                else
                {
                    isDeleted = true;
                }
               
                trans.Commit();
               
            }
            catch (Exception ex)
            {
                trans.Rollback();

                throw new Exception("Exception: " + ex.Message);
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
            _sqlTrans = (MySqlTransaction)trans;
        }
	}
}

