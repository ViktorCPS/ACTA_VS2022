using System;
using System.Text;
using System.Net;
using System.Data;
using MySql.Data.MySqlClient;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;

using TransferObjects;
using Util;

namespace DataAccess
{
	/// <summary>
	/// Summary description for MySQLApplUsersLogDAO.
	/// </summary>
	public class MySQLApplUsersLogDAO : ApplUsersLogDAO
	{
		MySqlConnection conn = null;
		protected string dateTimeformat = "";

		public MySQLApplUsersLogDAO()
		{
			conn = MySQLDAOFactory.getConnection();
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MySQLApplUsersLogDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public ApplUserLogTO insert(ApplUserLogTO userTO)
        {
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            int logID = 0;

            try
            {
                userTO.LogInTime = DateTime.Now;
                if (userTO.Host.Trim().Equals(""))
                    userTO.Host = Dns.GetHostName();

                StringBuilder sbInsert = new StringBuilder();                
                sbInsert.Append("INSERT INTO appl_users_log ");
                sbInsert.Append("(user_id, login_time, host, logout_time, login_chanel, login_status, login_type, created_by, created_time) ");
                sbInsert.Append("VALUES (" + "N'" + userTO.UserID + "', '" + userTO.LogInTime.ToString(dateTimeformat) + "', ");
                sbInsert.Append("N'" + userTO.Host.Trim() + "', NULL, ");
                sbInsert.Append("N'" + userTO.LoginChanel.Trim() + "', N'" + userTO.LoginStatus.Trim() + "', N'" + userTO.LoginType.Trim() + "', ");
                sbInsert.Append("N'" + userTO.UserID.Trim() + "', NOW());");
                sbInsert.Append("SELECT LAST_INSERT_ID() AS login_id ");

                DataSet dataSet = new DataSet();
                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);
                sqlDataAdapter.Fill(dataSet, "userlog");
                DataTable table = dataSet.Tables["userlog"];
                logID = Int32.Parse(table.Rows[0]["login_id"].ToString());

                userTO.SessionID = logID;
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();

                throw ex;
            }

            return userTO;
        }		

        public int update(int loginID, string createdBy, string modifiedBy)
        {
            int isUpdated = 0;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE appl_users_log SET ");
                sbUpdate.Append("logout_time = NOW(), ");
                if (!createdBy.Trim().Equals(""))
                {
                    sbUpdate.Append("created_by = N'" + createdBy.Trim() + "', ");
                }
                if (!modifiedBy.Trim().Equals(""))
                {
                    sbUpdate.Append("modified_by = N'" + modifiedBy.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                }
                sbUpdate.Append("modified_time = NOW() ");
                sbUpdate.Append("WHERE login_id = '" + loginID.ToString().Trim() + "'");

                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = loginID;
                }
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

		public int update(int loginID)
		{
			int isUpdated=0;
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

			try
			{
				StringBuilder sbUpdate = new StringBuilder();
				sbUpdate.Append("UPDATE appl_users_log SET ");	
				sbUpdate.Append("logout_time = NOW(),");		
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = NOW() ");
				sbUpdate.Append("WHERE login_id = '" + loginID.ToString().Trim() + "'");
				
				MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, sqlTrans);
				int res = cmd.ExecuteNonQuery();
				if(res > 0)
				{
					isUpdated = loginID;	
				}
				sqlTrans.Commit();
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback();
				
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

        public ApplUserLogTO findMaxSession(string userID, string host, string chanel)
        {
            DataSet dataSet = new DataSet();
            ApplUserLogTO applUserLogTO = new ApplUserLogTO();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM appl_users_log WHERE user_id = '" + userID.Trim() + "'");
                sb.Append(" AND host = '" + host.Trim() + "' AND login_chanel = '" + chanel.Trim() + "' AND login_id = (SELECT MAX(login_id) FROM appl_users_log");
                sb.Append(" WHERE user_id = '" + userID.Trim() + "' AND host = '" + host.Trim() + "' AND login_chanel = '" + chanel.Trim() + "')");

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ApplUsersLog");
                DataTable table = dataSet.Tables["ApplUsersLog"];

                if (table.Rows.Count == 1)
                {
                    applUserLogTO = new ApplUserLogTO();
                    if (!table.Rows[0]["login_id"].Equals(DBNull.Value))
                    {
                        applUserLogTO.SessionID = Convert.ToInt32(table.Rows[0]["login_id"].ToString().Trim());
                    }
                    if (!table.Rows[0]["user_id"].Equals(DBNull.Value))
                    {
                        applUserLogTO.UserID = table.Rows[0]["user_id"].ToString().Trim();
                    }
                    if (!table.Rows[0]["login_time"].Equals(DBNull.Value))
                    {
                        applUserLogTO.LogInTime = DateTime.Parse(table.Rows[0]["login_time"].ToString());
                    }
                    if (!table.Rows[0]["logout_time"].Equals(DBNull.Value))
                    {
                        applUserLogTO.LogOutTime = DateTime.Parse(table.Rows[0]["logout_time"].ToString());
                    }
                    if (!table.Rows[0]["host"].Equals(DBNull.Value))
                    {
                        applUserLogTO.Host = table.Rows[0]["host"].ToString().Trim();
                    }
                    if (!table.Rows[0]["login_chanel"].Equals(DBNull.Value))
                    {
                        applUserLogTO.LoginChanel = table.Rows[0]["login_chanel"].ToString().Trim();
                    }
                    if (!table.Rows[0]["login_status"].Equals(DBNull.Value))
                    {
                        applUserLogTO.LoginStatus = table.Rows[0]["login_status"].ToString().Trim();
                    }
                    if (!table.Rows[0]["login_type"].Equals(DBNull.Value))
                    {
                        applUserLogTO.LoginType = table.Rows[0]["login_type"].ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return applUserLogTO;
        }

        public List<ApplUserLogTO> getOpenSessions(string userID, string host, string chanel)
        {
            DataSet dataSet = new DataSet();
            ApplUserLogTO applUserLogTO = new ApplUserLogTO();
            List<ApplUserLogTO> applUsersLogList = new List<ApplUserLogTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM appl_users_log WHERE");
                if (!userID.Trim().Equals(""))
                {
                    sb.Append(" UPPER(user_id) = N'" + userID.ToUpper().Trim() + "' AND");
                }
                if (!host.Trim().Equals(""))
                {
                    sb.Append(" UPPER(host) = N'" + host.ToUpper().Trim() + "' AND");
                }
                if (!chanel.Trim().Equals(""))
                {
                    sb.Append(" UPPER(login_chanel) = N'" + chanel.ToUpper().Trim() + "' AND");
                }

                sb.Append(" logout_time is null");

                select = sb.ToString();

                select = select + " ORDER BY user_id, login_time";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ApplUsersLog");
                DataTable table = dataSet.Tables["ApplUsersLog"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        applUserLogTO = new ApplUserLogTO();

                        if (!row["user_id"].Equals(DBNull.Value))
                        {
                            applUserLogTO.UserID = row["user_id"].ToString().Trim();
                        }
                        if (!row["login_id"].Equals(DBNull.Value))
                        {
                            applUserLogTO.SessionID = Convert.ToInt32(row["login_id"]);
                        }
                        if (!row["login_time"].Equals(DBNull.Value))
                        {
                            applUserLogTO.LogInTime = DateTime.Parse(row["login_time"].ToString());
                        }
                        if (!row["logout_time"].Equals(DBNull.Value))
                        {
                            applUserLogTO.LogOutTime = DateTime.Parse(row["logout_time"].ToString());
                        }
                        if (!row["host"].Equals(DBNull.Value))
                        {
                            applUserLogTO.Host = row["host"].ToString().Trim();
                        }
                        if (!row["login_chanel"].Equals(DBNull.Value))
                        {
                            applUserLogTO.LoginChanel = row["login_chanel"].ToString().Trim();
                        }
                        if (!row["login_status"].Equals(DBNull.Value))
                        {
                            applUserLogTO.LoginStatus = row["login_status"].ToString().Trim();
                        }
                        if (!row["login_type"].Equals(DBNull.Value))
                        {
                            applUserLogTO.LoginType = row["login_type"].ToString().Trim();
                        }

                        applUsersLogList.Add(applUserLogTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return applUsersLogList;
        }

        public List<ApplUserLogTO> getApplUsersLog(ApplUserLogTO logTO, string userIDs, DateTime dateFrom, DateTime dateTo, List<string> changeTables, Dictionary<string, ApplUserTO> users)
        {
            DataSet dataSet = new DataSet();
            ApplUserLogTO applUserLogTO = new ApplUserLogTO();
            List<ApplUserLogTO> applUserLogList = new List<ApplUserLogTO>();
            string select = "";

            try
            {
                StringBuilder sbCondition = new StringBuilder();
                if (!logTO.LoginChanel.Trim().Equals(""))
                {
                    sbCondition.Append(" UPPER(login_chanel) = N'" + logTO.LoginChanel.ToUpper().Trim() + "' AND");
                }
                if (!logTO.LoginStatus.Trim().Equals(""))
                {
                    sbCondition.Append(" UPPER(login_status) = N'" + logTO.LoginStatus.ToUpper().Trim() + "' AND");
                }
                if (!logTO.LoginType.Trim().Equals(""))
                {
                    sbCondition.Append(" UPPER(login_type) = N'" + logTO.LoginType.ToUpper().Trim() + "' AND");
                }

                if (!dateFrom.Equals(new DateTime()))
                {
                    //sb.Append(" CONVERT(datetime,CONVERT(varchar(30),login_time,101),101) BETWEEN CONVERT(datetime,'" + dateFrom + "',101) AND");
                    sbCondition.Append(" login_time >= '" + dateFrom.ToString(dateTimeformat) + "' AND");
                }

                if (!dateTo.Equals(new DateTime()))
                {
                    //sb.Append(" CONVERT(datetime,'" + dateTo + "',101)");
                    sbCondition.Append(" login_time < '" + dateTo.Date.AddDays(1).ToString(dateTimeformat) + "' AND");
                }

                string select1 = "SELECT * FROM appl_users_log ";
                string cond1 = "";
                string cond2 = " WHERE user_id NOT IN (SELECT user_id FROM appl_users) AND";

                if (userIDs.Trim().Equals("") || sbCondition.ToString().Length > 0)
                {
                    cond1 += " WHERE ";

                    if (sbCondition.ToString().Length > 0)
                    {
                        cond1 += sbCondition.ToString();
                        cond2 += sbCondition.ToString();
                    }

                    if (!userIDs.Trim().Equals(""))
                    {
                        cond1 += " UPPER(user_id) IN (" + userIDs.ToUpper().Trim() + ") AND";
                    }

                    cond1 = cond1.Substring(0, cond1.Length - 3);
                    cond2 = cond2.Substring(0, cond2.Length - 3);
                }

                select = select1 + cond1;

                if (userIDs.Split(',').Length > 1)
                    select += " UNION " + select1 + cond2;

                select += " ORDER BY user_id, login_time";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ApplUsersLog");
                DataTable table = dataSet.Tables["ApplUsersLog"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        applUserLogTO = new ApplUserLogTO();

                        if (!row["user_id"].Equals(DBNull.Value))
                        {
                            applUserLogTO.UserID = row["user_id"].ToString().Trim();
                        }
                        if (users.ContainsKey(applUserLogTO.UserID))
                            applUserLogTO.UserName = users[applUserLogTO.UserID].Name;
                        if (!row["login_id"].Equals(DBNull.Value))
                        {
                            applUserLogTO.SessionID = Convert.ToInt32(row["login_id"]);
                        }
                        if (!row["login_time"].Equals(DBNull.Value))
                        {
                            applUserLogTO.LogInTime = Convert.ToDateTime(row["login_time"]);
                        }
                        if (!row["logout_time"].Equals(DBNull.Value))
                        {
                            applUserLogTO.LogOutTime = Convert.ToDateTime(row["logout_time"]);
                        }
                        if (!row["host"].Equals(DBNull.Value))
                        {
                            applUserLogTO.Host = row["host"].ToString().Trim();
                        }
                        if (!row["login_chanel"].Equals(DBNull.Value))
                        {
                            applUserLogTO.LoginChanel = row["login_chanel"].ToString().Trim();
                        }
                        if (!row["login_status"].Equals(DBNull.Value))
                        {
                            applUserLogTO.LoginStatus = row["login_status"].ToString().Trim();
                        }
                        if (!row["login_type"].Equals(DBNull.Value))
                        {
                            applUserLogTO.LoginType = row["login_type"].ToString().Trim();
                        }

                        if (!applUserLogTO.LogOutTime.Equals(new DateTime()))
                        {
                            applUserLogTO.Duration = applUserLogTO.LogOutTime.Subtract(applUserLogTO.LogInTime);
                            applUserLogTO.LoginChange = Constants.noInt;
                            foreach (string tbl in changeTables)
                            {
                                try
                                {
                                    int rows = -1;
                                    string chSelect = "SELECT COUNT(*) AS count FROM " + tbl.Trim() + " WHERE (created_by = '" + applUserLogTO.UserID.Trim() + "' AND created_time >= '"
                                        + applUserLogTO.LogInTime.ToString(dateTimeformat) + "' AND created_time <= '" + applUserLogTO.LogOutTime.ToString(dateTimeformat) + "') OR "
                                        + "(modified_by = '" + applUserLogTO.UserID.Trim() + "' AND modified_time >= '" + applUserLogTO.LogInTime.ToString(dateTimeformat)
                                        + "' AND modified_time <= '" + applUserLogTO.LogOutTime.ToString(dateTimeformat) + "') ";
                                    MySqlCommand chCmd = new MySqlCommand(chSelect, conn);
                                    MySqlDataAdapter chSqlDataAdapter = new MySqlDataAdapter(chCmd);
                                    DataSet chSet = new DataSet();

                                    chSqlDataAdapter.Fill(chSet, "Changes");
                                    DataTable chTable = chSet.Tables["Changes"];

                                    if (chTable.Rows.Count > 0)
                                        rows = int.Parse(chTable.Rows[0]["count"].ToString());

                                    if (rows > 0)
                                    {
                                        applUserLogTO.LoginChange = Constants.yesInt;
                                        break;
                                    }
                                }
                                catch { }
                            }
                        }

                        if (logTO.LoginChange != -1 && logTO.LoginChange != applUserLogTO.LoginChange)
                            continue;

                        applUserLogList.Add(applUserLogTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return applUserLogList;
        }
	}
}

