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
using System.Globalization;

using TransferObjects;
using Util;

namespace DataAccess
{
    /// <summary>
    /// Summary description for MSSQLApplUserDAO.
    /// </summary>
    public class MSSQLApplUserDAO : ApplUserDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLApplUserDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MSSQLApplUserDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(ApplUserTO userTO)
        {
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO appl_users ");
                sbInsert.Append("(user_id, password, name, description, privilege_lvl, status, num_of_tries, lang_code, exit_permission_verification_lvl, created_by, created_time, extra_hrs_advanced_amt) ");
                sbInsert.Append("VALUES (");

                if (userTO.UserID.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + userTO.UserID.Trim() + "', ");
                }
                if (userTO.Password.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + userTO.Password.Trim() + "', ");
                }
                if (userTO.Name.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + userTO.Name.Trim() + "', ");
                }
                if (userTO.Description.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + userTO.Description.Trim() + "', ");
                }
                if (userTO.PrivilegeLvl == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + userTO.PrivilegeLvl.ToString().Trim() + "', ");
                }
                if (userTO.Status.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + userTO.Status.Trim() + "', ");
                }
                if (userTO.NumOfTries == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + userTO.NumOfTries.ToString().Trim() + "', ");
                }
                if (userTO.LangCode.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + userTO.LangCode.Trim() + "', ");
                }
                if (userTO.ExitPermVerification == -1)
                {
                    sbInsert.Append("'1', "); //not null
                }
                else
                {
                    sbInsert.Append("'" + userTO.ExitPermVerification.ToString() + "', ");
                }
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE(), ");
                if (userTO.ExtraHoursAdvancedAmt == -1)
                {
                    sbInsert.Append("'0' )");
                }
                else
                {
                    sbInsert.Append("'" + userTO.ExtraHoursAdvancedAmt.ToString().Trim() + "' )");
                }
                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("INSERT");

                throw ex;
            }

            return rowsAffected;
        }

        public int insert(ApplUserTO userTO, bool doCommit)
        {
            SqlTransaction sqlTrans = null;
            if (doCommit)
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            else
                sqlTrans = SqlTrans;

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO appl_users ");
                sbInsert.Append("(user_id, password, name, description, privilege_lvl, status, num_of_tries, lang_code, exit_permission_verification_lvl, created_by, created_time, extra_hrs_advanced_amt) ");
                sbInsert.Append("VALUES (");

                if (userTO.UserID.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + userTO.UserID.Trim() + "', ");
                }
                if (userTO.Password.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + userTO.Password.Trim() + "', ");
                }
                if (userTO.Name.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + userTO.Name.Trim() + "', ");
                }
                if (userTO.Description.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + userTO.Description.Trim() + "', ");
                }
                if (userTO.PrivilegeLvl == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + userTO.PrivilegeLvl.ToString().Trim() + "', ");
                }
                if (userTO.Status.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + userTO.Status.Trim() + "', ");
                }
                if (userTO.NumOfTries == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + userTO.NumOfTries.ToString().Trim() + "', ");
                }
                if (userTO.LangCode.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + userTO.LangCode.Trim() + "', ");
                }
                if (userTO.ExitPermVerification == -1)
                {
                    sbInsert.Append("'1', "); //not null
                }
                else
                {
                    sbInsert.Append("'" + userTO.ExitPermVerification.ToString() + "', ");
                }
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE(), ");
                if (userTO.ExtraHoursAdvancedAmt == -1)
                {
                    sbInsert.Append("'0' )");
                }
                else
                {
                    sbInsert.Append("'" + userTO.ExtraHoursAdvancedAmt.ToString().Trim() + "' )");
                }
                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();
                if (doCommit)
                    sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    sqlTrans.Rollback("INSERT");

                throw ex;
            }

            return rowsAffected;
        }

        public bool delete(string userID)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM appl_users WHERE user_id = N'" + userID.ToString().Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                isDeleted = true;
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("DELETE");

                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public bool update(ApplUserTO userTO)
        {
            bool isUpdated = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE appl_users SET ");

                sbUpdate.Append("password = N'" + userTO.Password.Trim() + "', ");
                if (!userTO.Name.Trim().Equals(""))
                {
                    sbUpdate.Append("name = N'" + userTO.Name.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("name = null, ");
                }
                if (!userTO.Description.Trim().Equals(""))
                {
                    sbUpdate.Append("description = N'" + userTO.Description.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("description = null, ");
                }
                if (userTO.PrivilegeLvl != -1)
                {
                    sbUpdate.Append("privilege_lvl = '" + userTO.PrivilegeLvl.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("privilege_lvl = null, ");
                }
                if (!userTO.Status.Trim().Equals(""))
                {
                    sbUpdate.Append("status = N'" + userTO.Status.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("status = null, ");
                }
                if (userTO.NumOfTries != -1)
                {
                    sbUpdate.Append("num_of_tries = '" + userTO.NumOfTries.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("num_of_tries = null, ");
                }
                if (!userTO.LangCode.Trim().Equals(""))
                {
                    sbUpdate.Append("lang_code = N'" + userTO.LangCode.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("lang_code = null, ");
                }
                if (userTO.ModifiedBy.Trim() != "")
                    sbUpdate.Append("modified_by = N'" + userTO.ModifiedBy.Trim() + "', ");
                else
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE user_id = N'" + userTO.UserID.ToString().Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("UPDATE");

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool update(ApplUserTO userTO, bool doCommit)
        {
            bool isUpdated = false;
            SqlTransaction sqlTrans = null;
            if (doCommit)
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");
            else
                sqlTrans = SqlTrans;

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE appl_users SET ");

                sbUpdate.Append("password = N'" + userTO.Password.Trim() + "', ");
                if (!userTO.Name.Trim().Equals(""))
                {
                    sbUpdate.Append("name = N'" + userTO.Name.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("name = null, ");
                }
                if (!userTO.Description.Trim().Equals(""))
                {
                    sbUpdate.Append("description = N'" + userTO.Description.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("description = null, ");
                }
                if (userTO.PrivilegeLvl != -1)
                {
                    sbUpdate.Append("privilege_lvl = '" + userTO.PrivilegeLvl.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("privilege_lvl = null, ");
                }
                if (!userTO.Status.Trim().Equals(""))
                {
                    sbUpdate.Append("status = N'" + userTO.Status.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("status = null, ");
                }
                if (userTO.NumOfTries != -1)
                {
                    sbUpdate.Append("num_of_tries = '" + userTO.NumOfTries.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("num_of_tries = null, ");
                }
                if (!userTO.LangCode.Trim().Equals(""))
                {
                    sbUpdate.Append("lang_code = N'" + userTO.LangCode.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("lang_code = null, ");
                }
                if (userTO.ModifiedBy.Trim() != "")
                    sbUpdate.Append("modified_by = N'" + userTO.ModifiedBy.Trim() + "', ");
                else
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE user_id = N'" + userTO.UserID.ToString().Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }
                if (doCommit)
                    sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    sqlTrans.Rollback("UPDATE");

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool updatePassword(string userID, string password)
        {
            bool isUpdated = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE appl_users SET ");
                sbUpdate.Append("password = N'" + password.Trim() + "', ");
                sbUpdate.Append("modified_by = N'" + userID.Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE user_id = N'" + userID.Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("UPDATE");

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool updateExitPermVerification(ApplUserTO userTO)
        {
            bool isUpdated = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE appl_users SET ");

                sbUpdate.Append("password = N'" + userTO.Password.Trim() + "', ");
                if (!userTO.Name.Trim().Equals(""))
                {
                    sbUpdate.Append("name = N'" + userTO.Name.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("name = null, ");
                }
                if (!userTO.Description.Trim().Equals(""))
                {
                    sbUpdate.Append("description = N'" + userTO.Description.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("description = null, ");
                }
                if (userTO.PrivilegeLvl != -1)
                {
                    sbUpdate.Append("privilege_lvl = '" + userTO.PrivilegeLvl.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("privilege_lvl = null, ");
                }
                if (!userTO.Status.Trim().Equals(""))
                {
                    sbUpdate.Append("status = N'" + userTO.Status.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("status = null, ");
                }
                if (userTO.NumOfTries != -1)
                {
                    sbUpdate.Append("num_of_tries = '" + userTO.NumOfTries.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("num_of_tries = null, ");
                }
                if (!userTO.LangCode.Trim().Equals(""))
                {
                    sbUpdate.Append("lang_code = N'" + userTO.LangCode.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("lang_code = null, ");
                }
                if (userTO.ExitPermVerification != -1)
                {
                    sbUpdate.Append("exit_permission_verification_lvl = '" + userTO.ExitPermVerification.ToString() + "', ");
                }
                else
                {
                    // not null 
                    //sbUpdate.Append("exit_permission_verification_lvl = null, ");
                }

                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE(), ");
                if (userTO.ExitPermVerification != -1)
                {
                    sbUpdate.Append("extra_hrs_advanced_amt = '" + userTO.ExtraHoursAdvancedAmt + "'");
                }
                else
                {
                    sbUpdate.Append("extra_hrs_advanced_amt = '0'");
                }
                sbUpdate.Append(" WHERE user_id = N'" + userTO.UserID.ToString().Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("UPDATE");

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool updateExitPermVerification(ApplUserTO userTO, bool doCommit)
        {
            bool isUpdated = false;
            SqlTransaction sqlTrans = null;
            if (doCommit)
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");
            else
                sqlTrans = SqlTrans;

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE appl_users SET ");

                sbUpdate.Append("password = N'" + userTO.Password.Trim() + "', ");
                if (!userTO.Name.Trim().Equals(""))
                {
                    sbUpdate.Append("name = N'" + userTO.Name.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("name = null, ");
                }
                if (!userTO.Description.Trim().Equals(""))
                {
                    sbUpdate.Append("description = N'" + userTO.Description.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("description = null, ");
                }
                if (userTO.PrivilegeLvl != -1)
                {
                    sbUpdate.Append("privilege_lvl = '" + userTO.PrivilegeLvl.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("privilege_lvl = null, ");
                }
                if (!userTO.Status.Trim().Equals(""))
                {
                    sbUpdate.Append("status = N'" + userTO.Status.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("status = null, ");
                }
                if (userTO.NumOfTries != -1)
                {
                    sbUpdate.Append("num_of_tries = '" + userTO.NumOfTries.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("num_of_tries = null, ");
                }
                if (!userTO.LangCode.Trim().Equals(""))
                {
                    sbUpdate.Append("lang_code = N'" + userTO.LangCode.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("lang_code = null, ");
                }
                if (userTO.ExitPermVerification != -1)
                {
                    sbUpdate.Append("exit_permission_verification_lvl = '" + userTO.ExitPermVerification.ToString() + "', ");
                }
                else
                {
                    // not null 
                    //sbUpdate.Append("exit_permission_verification_lvl = null, ");
                }

                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE(), ");
                if (userTO.ExitPermVerification != -1)
                {
                    sbUpdate.Append("extra_hrs_advanced_amt = '" + userTO.ExtraHoursAdvancedAmt + "'");
                }
                else
                {
                    sbUpdate.Append("extra_hrs_advanced_amt = '0'");
                }
                sbUpdate.Append(" WHERE user_id = N'" + userTO.UserID.ToString().Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }
                if (doCommit)
                    sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    sqlTrans.Rollback("UPDATE");

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public ApplUserTO find(string userID)
        {
            DataSet dataSet = new DataSet();
            ApplUserTO applUserTO = new ApplUserTO();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM appl_users WHERE user_id = N'" + userID.Trim() + "'", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ApplUsers");
                DataTable table = dataSet.Tables["ApplUsers"];

                if (table.Rows.Count == 1)
                {
                    applUserTO = new ApplUserTO();
                    if (!table.Rows[0]["user_id"].Equals(DBNull.Value))
                    {
                        applUserTO.UserID = table.Rows[0]["user_id"].ToString().Trim();
                    }
                    if (!table.Rows[0]["password"].Equals(DBNull.Value))
                    {
                        applUserTO.Password = table.Rows[0]["password"].ToString().Trim();
                    }
                    if (!table.Rows[0]["name"].Equals(DBNull.Value))
                    {
                        applUserTO.Name = table.Rows[0]["name"].ToString().Trim();
                    }
                    if (!table.Rows[0]["description"].Equals(DBNull.Value))
                    {
                        applUserTO.Description = table.Rows[0]["description"].ToString().Trim();
                    }
                    if (!table.Rows[0]["privilege_lvl"].Equals(DBNull.Value))
                    {
                        applUserTO.PrivilegeLvl = Int32.Parse(table.Rows[0]["privilege_lvl"].ToString().Trim());
                    }
                    if (!table.Rows[0]["status"].Equals(DBNull.Value))
                    {
                        applUserTO.Status = table.Rows[0]["status"].ToString().Trim();
                    }
                    if (!table.Rows[0]["num_of_tries"].Equals(DBNull.Value))
                    {
                        applUserTO.NumOfTries = Int32.Parse(table.Rows[0]["num_of_tries"].ToString().Trim());
                    }
                    if (!table.Rows[0]["lang_code"].Equals(DBNull.Value))
                    {
                        applUserTO.LangCode = table.Rows[0]["lang_code"].ToString().Trim();
                    }
                    if (!table.Rows[0]["exit_permission_verification_lvl"].Equals(DBNull.Value))
                    {
                        applUserTO.ExitPermVerification = Int32.Parse(table.Rows[0]["exit_permission_verification_lvl"].ToString().Trim());
                    }
                    if (!table.Rows[0]["extra_hrs_advanced_amt"].Equals(DBNull.Value))
                    {
                        applUserTO.ExtraHoursAdvancedAmt = Int32.Parse(table.Rows[0]["extra_hrs_advanced_amt"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return applUserTO;
        }

        public ApplUserTO findUserPassword(string userID, string password)
        {
            DataSet dataSet = new DataSet();
            ApplUserTO applUserTO = new ApplUserTO();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM appl_users WHERE user_id = N'" + userID + "'");

                if (!password.Trim().Equals(""))
                {
                    sb.Append(" AND password = N'" + password + "'");
                }

                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ApplUsers");
                DataTable table = dataSet.Tables["ApplUsers"];

                if (table.Rows.Count == 1)
                {
                    applUserTO = new ApplUserTO();
                    if (!table.Rows[0]["user_id"].Equals(DBNull.Value))
                    {
                        applUserTO.UserID = table.Rows[0]["user_id"].ToString().Trim();
                    }
                    if (!table.Rows[0]["password"].Equals(DBNull.Value))
                    {
                        applUserTO.Password = table.Rows[0]["password"].ToString().Trim();
                    }
                    if (!table.Rows[0]["name"].Equals(DBNull.Value))
                    {
                        applUserTO.Name = table.Rows[0]["name"].ToString().Trim();
                    }
                    if (!table.Rows[0]["description"].Equals(DBNull.Value))
                    {
                        applUserTO.Description = table.Rows[0]["description"].ToString().Trim();
                    }
                    if (!table.Rows[0]["privilege_lvl"].Equals(DBNull.Value))
                    {
                        applUserTO.PrivilegeLvl = Int32.Parse(table.Rows[0]["privilege_lvl"].ToString().Trim());
                    }
                    if (!table.Rows[0]["status"].Equals(DBNull.Value))
                    {
                        applUserTO.Status = table.Rows[0]["status"].ToString().Trim();
                    }
                    if (!table.Rows[0]["num_of_tries"].Equals(DBNull.Value))
                    {
                        applUserTO.NumOfTries = Int32.Parse(table.Rows[0]["num_of_tries"].ToString().Trim());
                    }
                    if (!table.Rows[0]["lang_code"].Equals(DBNull.Value))
                    {
                        applUserTO.LangCode = table.Rows[0]["lang_code"].ToString().Trim();
                    }
                    if (!table.Rows[0]["exit_permission_verification_lvl"].Equals(DBNull.Value))
                    {
                        applUserTO.ExitPermVerification = Int32.Parse(table.Rows[0]["exit_permission_verification_lvl"].ToString().Trim());
                    }
                    if (!table.Rows[0]["extra_hrs_advanced_amt"].Equals(DBNull.Value))
                    {
                        applUserTO.ExtraHoursAdvancedAmt = Int32.Parse(table.Rows[0]["extra_hrs_advanced_amt"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return applUserTO;
        }

        public ApplUserTO findExitPermVerification(string userID)
        {
            DataSet dataSet = new DataSet();
            ApplUserTO applUserTO = new ApplUserTO();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM appl_users WHERE user_id = N'" + userID.Trim() + "'", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ApplUsers");
                DataTable table = dataSet.Tables["ApplUsers"];

                if (table.Rows.Count == 1)
                {
                    applUserTO = new ApplUserTO();
                    if (!table.Rows[0]["user_id"].Equals(DBNull.Value))
                    {
                        applUserTO.UserID = table.Rows[0]["user_id"].ToString().Trim();
                    }
                    if (!table.Rows[0]["password"].Equals(DBNull.Value))
                    {
                        applUserTO.Password = table.Rows[0]["password"].ToString().Trim();
                    }
                    if (!table.Rows[0]["name"].Equals(DBNull.Value))
                    {
                        applUserTO.Name = table.Rows[0]["name"].ToString().Trim();
                    }
                    if (!table.Rows[0]["description"].Equals(DBNull.Value))
                    {
                        applUserTO.Description = table.Rows[0]["description"].ToString().Trim();
                    }
                    if (!table.Rows[0]["privilege_lvl"].Equals(DBNull.Value))
                    {
                        applUserTO.PrivilegeLvl = Int32.Parse(table.Rows[0]["privilege_lvl"].ToString().Trim());
                    }
                    if (!table.Rows[0]["status"].Equals(DBNull.Value))
                    {
                        applUserTO.Status = table.Rows[0]["status"].ToString().Trim();
                    }
                    if (!table.Rows[0]["num_of_tries"].Equals(DBNull.Value))
                    {
                        applUserTO.NumOfTries = Int32.Parse(table.Rows[0]["num_of_tries"].ToString().Trim());
                    }
                    if (!table.Rows[0]["lang_code"].Equals(DBNull.Value))
                    {
                        applUserTO.LangCode = table.Rows[0]["lang_code"].ToString().Trim();
                    }
                    if (!table.Rows[0]["exit_permission_verification_lvl"].Equals(DBNull.Value))
                    {
                        applUserTO.ExitPermVerification = Int32.Parse(table.Rows[0]["exit_permission_verification_lvl"].ToString().Trim());
                    }
                    if (!table.Rows[0]["extra_hrs_advanced_amt"].Equals(DBNull.Value))
                    {
                        applUserTO.ExtraHoursAdvancedAmt = Int32.Parse(table.Rows[0]["extra_hrs_advanced_amt"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return applUserTO;
        }
        public List<ApplUserTO> getApplUsersForCategory(int appl_users_category_id)
        {
            DataSet dataSet = new DataSet();
            ApplUserTO applUserTO = new ApplUserTO();
            List<ApplUserTO> applUserList = new List<ApplUserTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT users.* from actamgr.appl_users users, actamgr.appl_users_categories cat,actamgr.appl_users_x_appl_users_categories x ");
                sb.Append("WHERE users.user_id= x.user_id AND x.appl_users_category_id = cat.appl_users_category_id AND ");
                sb.Append("users.status='ACTIVE'");
                if (appl_users_category_id != -1)
                    sb.Append(" AND cat.appl_users_category_id = '" + appl_users_category_id + "'");

                select = sb.ToString();
                select = select + "ORDER BY user_id ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ApplUser");
                DataTable table = dataSet.Tables["ApplUser"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        applUserTO = new ApplUserTO();

                        if (!row["user_id"].Equals(DBNull.Value))
                        {
                            applUserTO.UserID = row["user_id"].ToString().Trim();
                        }
                        if (!row["password"].Equals(DBNull.Value))
                        {
                            applUserTO.Password = row["password"].ToString().Trim();
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            applUserTO.Name = row["name"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            applUserTO.Description = row["description"].ToString().Trim();
                        }
                        if (!row["privilege_lvl"].Equals(DBNull.Value))
                        {
                            applUserTO.PrivilegeLvl = Int32.Parse(row["privilege_lvl"].ToString().Trim());
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            applUserTO.Status = row["status"].ToString().Trim();
                        }
                        if (!row["num_of_tries"].Equals(DBNull.Value))
                        {
                            applUserTO.NumOfTries = Int32.Parse(row["num_of_tries"].ToString().Trim());
                        }
                        if (!row["lang_code"].Equals(DBNull.Value))
                        {
                            applUserTO.LangCode = row["lang_code"].ToString().Trim();
                        }
                        if (!row["exit_permission_verification_lvl"].Equals(DBNull.Value))
                        {
                            applUserTO.ExitPermVerification = Int32.Parse(row["exit_permission_verification_lvl"].ToString().Trim());
                        }
                        if (!row["extra_hrs_advanced_amt"].Equals(DBNull.Value))
                        {
                            applUserTO.ExtraHoursAdvancedAmt = Int32.Parse(row["extra_hrs_advanced_amt"].ToString().Trim());
                        }

                        applUserList.Add(applUserTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return applUserList;
        }

        public List<ApplUserTO> getApplUsers(ApplUserTO userTO)
        {
            DataSet dataSet = new DataSet();
            ApplUserTO applUserTO = new ApplUserTO();
            List<ApplUserTO> applUserList = new List<ApplUserTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM appl_users ");

                if ((!userTO.UserID.Trim().Equals("")) || (!userTO.Password.Trim().Equals("")) ||
                    (!userTO.Name.Trim().Equals("")) || (!userTO.Description.Trim().Equals("")) ||
                    (userTO.PrivilegeLvl != -1) || (!userTO.Status.Trim().Equals("")) ||
                    (userTO.NumOfTries != -1) || (!userTO.LangCode.Trim().Equals("")))
                {
                    sb.Append(" WHERE ");

                    if (!userTO.UserID.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(user_id) LIKE N'%" + userTO.UserID.ToUpper().Trim() + "%' AND");
                    }
                    if (!userTO.Password.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(password) LIKE N'%" + userTO.Password.ToUpper().Trim() + "%' AND");
                    }
                    if (!userTO.Name.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(name) LIKE N'%" + userTO.Name.ToUpper().Trim() + "%' AND");
                    }
                    if (!userTO.Description.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(description) LIKE N'%" + userTO.Description.ToUpper().Trim() + "%' AND");
                    }
                    if (userTO.PrivilegeLvl != -1)
                    {
                        sb.Append(" privilege_lvl = '" + userTO.PrivilegeLvl.ToString().Trim() + "' AND");
                    }
                    if (!userTO.Status.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(status) LIKE N'%" + userTO.Status.ToUpper().Trim() + "%' AND");
                    }
                    if (userTO.NumOfTries != -1)
                    {
                        sb.Append(" num_of_tries = '" + userTO.NumOfTries.ToString().Trim() + "' AND");
                    }
                    if (!userTO.LangCode.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(lang_code) LIKE N'%" + userTO.LangCode.ToUpper().Trim() + "%' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY user_id ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ApplUser");
                DataTable table = dataSet.Tables["ApplUser"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        applUserTO = new ApplUserTO();

                        if (!row["user_id"].Equals(DBNull.Value))
                        {
                            applUserTO.UserID = row["user_id"].ToString().Trim();
                        }
                        if (!row["password"].Equals(DBNull.Value))
                        {
                            applUserTO.Password = row["password"].ToString().Trim();
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            applUserTO.Name = row["name"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            applUserTO.Description = row["description"].ToString().Trim();
                        }
                        if (!row["privilege_lvl"].Equals(DBNull.Value))
                        {
                            applUserTO.PrivilegeLvl = Int32.Parse(row["privilege_lvl"].ToString().Trim());
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            applUserTO.Status = row["status"].ToString().Trim();
                        }
                        if (!row["num_of_tries"].Equals(DBNull.Value))
                        {
                            applUserTO.NumOfTries = Int32.Parse(row["num_of_tries"].ToString().Trim());
                        }
                        if (!row["lang_code"].Equals(DBNull.Value))
                        {
                            applUserTO.LangCode = row["lang_code"].ToString().Trim();
                        }
                        if (!row["exit_permission_verification_lvl"].Equals(DBNull.Value))
                        {
                            applUserTO.ExitPermVerification = Int32.Parse(row["exit_permission_verification_lvl"].ToString().Trim());
                        }
                        if (!row["extra_hrs_advanced_amt"].Equals(DBNull.Value))
                        {
                            applUserTO.ExtraHoursAdvancedAmt = Int32.Parse(row["extra_hrs_advanced_amt"].ToString().Trim());
                        }

                        applUserList.Add(applUserTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return applUserList;
        }

        public List<ApplUserTO> getInactiveUsers(DateTime monthCreated)
        {
            DataSet dataSet = new DataSet();
            ApplUserTO applUserTO = new ApplUserTO();
            List<ApplUserTO> applUserList = new List<ApplUserTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM appl_users WHERE created_time < '" + monthCreated.ToString(dateTimeformat) + "' AND status <> '" + Constants.statusRetired + "' AND user_id NOT IN ");
                sb.Append("(SELECT DISTINCT user_id FROM appl_users_log WHERE login_status = '" + Constants.UserLoginStatus.SUCCESSFUL.ToString().Trim() + "' AND login_time >= '" + monthCreated.ToString(dateTimeformat) + "') ");
                
                select = sb.ToString();                

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ApplUser");
                DataTable table = dataSet.Tables["ApplUser"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        applUserTO = new ApplUserTO();

                        if (!row["user_id"].Equals(DBNull.Value))
                        {
                            applUserTO.UserID = row["user_id"].ToString().Trim();
                        }
                        if (!row["password"].Equals(DBNull.Value))
                        {
                            applUserTO.Password = row["password"].ToString().Trim();
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            applUserTO.Name = row["name"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            applUserTO.Description = row["description"].ToString().Trim();
                        }
                        if (!row["privilege_lvl"].Equals(DBNull.Value))
                        {
                            applUserTO.PrivilegeLvl = Int32.Parse(row["privilege_lvl"].ToString().Trim());
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            applUserTO.Status = row["status"].ToString().Trim();
                        }
                        if (!row["num_of_tries"].Equals(DBNull.Value))
                        {
                            applUserTO.NumOfTries = Int32.Parse(row["num_of_tries"].ToString().Trim());
                        }
                        if (!row["lang_code"].Equals(DBNull.Value))
                        {
                            applUserTO.LangCode = row["lang_code"].ToString().Trim();
                        }
                        if (!row["exit_permission_verification_lvl"].Equals(DBNull.Value))
                        {
                            applUserTO.ExitPermVerification = Int32.Parse(row["exit_permission_verification_lvl"].ToString().Trim());
                        }
                        if (!row["extra_hrs_advanced_amt"].Equals(DBNull.Value))
                        {
                            applUserTO.ExtraHoursAdvancedAmt = Int32.Parse(row["extra_hrs_advanced_amt"].ToString().Trim());
                        }

                        applUserList.Add(applUserTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return applUserList;
        }

        public Dictionary<string, ApplUserTO> getApplUsersDictionary(ApplUserTO userTO)
        {
            DataSet dataSet = new DataSet();
            ApplUserTO applUserTO = new ApplUserTO();
            Dictionary<string, ApplUserTO> applUserList = new Dictionary<string, ApplUserTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM appl_users ");

                if ((!userTO.UserID.Trim().Equals("")) || (!userTO.Password.Trim().Equals("")) ||
                    (!userTO.Name.Trim().Equals("")) || (!userTO.Description.Trim().Equals("")) ||
                    (userTO.PrivilegeLvl != -1) || (!userTO.Status.Trim().Equals("")) ||
                    (userTO.NumOfTries != -1) || (!userTO.LangCode.Trim().Equals("")))
                {
                    sb.Append(" WHERE ");

                    if (!userTO.UserID.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(user_id) LIKE N'%" + userTO.UserID.ToUpper().Trim() + "%' AND");
                    }
                    if (!userTO.Password.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(password) LIKE N'%" + userTO.Password.ToUpper().Trim() + "%' AND");
                    }
                    if (!userTO.Name.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(name) LIKE N'%" + userTO.Name.ToUpper().Trim() + "%' AND");
                    }
                    if (!userTO.Description.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(description) LIKE N'%" + userTO.Description.ToUpper().Trim() + "%' AND");
                    }
                    if (userTO.PrivilegeLvl != -1)
                    {
                        sb.Append(" privilege_lvl = '" + userTO.PrivilegeLvl.ToString().Trim() + "' AND");
                    }
                    if (!userTO.Status.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(status) LIKE N'%" + userTO.Status.ToUpper().Trim() + "%' AND");
                    }
                    if (userTO.NumOfTries != -1)
                    {
                        sb.Append(" num_of_tries = '" + userTO.NumOfTries.ToString().Trim() + "' AND");
                    }
                    if (!userTO.LangCode.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(lang_code) LIKE N'%" + userTO.LangCode.ToUpper().Trim() + "%' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY name ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ApplUser");
                DataTable table = dataSet.Tables["ApplUser"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        applUserTO = new ApplUserTO();

                        if (!row["user_id"].Equals(DBNull.Value))
                        {
                            applUserTO.UserID = row["user_id"].ToString().Trim();
                        }
                        if (!row["password"].Equals(DBNull.Value))
                        {
                            applUserTO.Password = row["password"].ToString().Trim();
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            applUserTO.Name = row["name"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            applUserTO.Description = row["description"].ToString().Trim();
                        }
                        if (!row["privilege_lvl"].Equals(DBNull.Value))
                        {
                            applUserTO.PrivilegeLvl = Int32.Parse(row["privilege_lvl"].ToString().Trim());
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            applUserTO.Status = row["status"].ToString().Trim();
                        }
                        if (!row["num_of_tries"].Equals(DBNull.Value))
                        {
                            applUserTO.NumOfTries = Int32.Parse(row["num_of_tries"].ToString().Trim());
                        }
                        if (!row["lang_code"].Equals(DBNull.Value))
                        {
                            applUserTO.LangCode = row["lang_code"].ToString().Trim();
                        }
                        if (!row["exit_permission_verification_lvl"].Equals(DBNull.Value))
                        {
                            applUserTO.ExitPermVerification = Int32.Parse(row["exit_permission_verification_lvl"].ToString().Trim());
                        }
                        if (!row["extra_hrs_advanced_amt"].Equals(DBNull.Value))
                        {
                            applUserTO.ExtraHoursAdvancedAmt = Int32.Parse(row["extra_hrs_advanced_amt"].ToString().Trim());
                        }

                        if (!applUserList.ContainsKey(applUserTO.UserID))
                            applUserList.Add(applUserTO.UserID, applUserTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return applUserList;
        }

        public List<ApplUserTO> getApplUsersWithStatus(ApplUserTO userTO, List<string> statuses)
        {
            DataSet dataSet = new DataSet();
            ApplUserTO applUserTO = new ApplUserTO();
            List<ApplUserTO> applUserList = new List<ApplUserTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM appl_users ");

                if ((!userTO.UserID.Trim().Equals("")) || (!userTO.Password.Trim().Equals("")) ||
                    (!userTO.Name.Trim().Equals("")) || (!userTO.Description.Trim().Equals("")) ||
                    (userTO.PrivilegeLvl != -1) || (!userTO.Status.Trim().Equals("")) ||
                    (userTO.NumOfTries != -1) || (!userTO.LangCode.Trim().Equals("")) || statuses.Count > 0)
                {
                    sb.Append(" WHERE ");

                    if (!userTO.UserID.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(user_id) LIKE N'%" + userTO.UserID.ToUpper().Trim() + "%' AND");
                    }
                    if (!userTO.Password.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(password) LIKE N'%" + userTO.Password.ToUpper().Trim() + "%' AND");
                    }
                    if (!userTO.Name.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(name) LIKE N'%" + userTO.Name.ToUpper().Trim() + "%' AND");
                    }
                    if (!userTO.Description.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(description) LIKE N'%" + userTO.Description.ToUpper().Trim() + "%' AND");
                    }
                    if (userTO.PrivilegeLvl != -1)
                    {
                        sb.Append(" privilege_lvl = '" + userTO.PrivilegeLvl.ToString().Trim() + "' AND");
                    }
                    if (statuses.Count > 0)
                    {
                        string statusString = " (";
                        foreach (string status in statuses)
                        {
                            statusString += (" UPPER(status) = N'" + status.ToUpper() + "' OR ");
                        }
                        statusString = statusString.Substring(0, statusString.Length - 3);
                        sb.Append(statusString + ") AND");
                    }
                    if (userTO.NumOfTries != -1)
                    {
                        sb.Append(" num_of_tries = '" + userTO.NumOfTries.ToString().Trim() + "' AND");
                    }
                    if (!userTO.LangCode.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(lang_code) LIKE N'%" + userTO.LangCode.ToUpper().Trim() + "%' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY user_id ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ApplUser");
                DataTable table = dataSet.Tables["ApplUser"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        applUserTO = new ApplUserTO();

                        if (!row["user_id"].Equals(DBNull.Value))
                        {
                            applUserTO.UserID = row["user_id"].ToString().Trim();
                        }
                        if (!row["password"].Equals(DBNull.Value))
                        {
                            applUserTO.Password = row["password"].ToString().Trim();
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            applUserTO.Name = row["name"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            applUserTO.Description = row["description"].ToString().Trim();
                        }
                        if (!row["privilege_lvl"].Equals(DBNull.Value))
                        {
                            applUserTO.PrivilegeLvl = Int32.Parse(row["privilege_lvl"].ToString().Trim());
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            applUserTO.Status = row["status"].ToString().Trim();
                        }
                        if (!row["num_of_tries"].Equals(DBNull.Value))
                        {
                            applUserTO.NumOfTries = Int32.Parse(row["num_of_tries"].ToString().Trim());
                        }
                        if (!row["lang_code"].Equals(DBNull.Value))
                        {
                            applUserTO.LangCode = row["lang_code"].ToString().Trim();
                        }
                        if (!row["exit_permission_verification_lvl"].Equals(DBNull.Value))
                        {
                            applUserTO.ExitPermVerification = Int32.Parse(row["exit_permission_verification_lvl"].ToString().Trim());
                        }
                        if (!row["extra_hrs_advanced_amt"].Equals(DBNull.Value))
                        {
                            applUserTO.ExtraHoursAdvancedAmt = Int32.Parse(row["extra_hrs_advanced_amt"].ToString().Trim());
                        }

                        applUserList.Add(applUserTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return applUserList;
        }

        public List<ApplUserTO> getApplUsersVerifiedByWUnits(string wUnits)
        {
            DataSet dataSet = new DataSet();
            ApplUserTO applUserTO = new ApplUserTO();
            List<ApplUserTO> applUserList = new List<ApplUserTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT auser.user_id, auser.name ");
                sb.Append("FROM exit_permissions ep, employees empl, appl_users auser");
                sb.Append(" WHERE ");

                sb.Append(" ep.employee_id = empl.employee_id");
                sb.Append(" AND ep.verified_by = auser.user_id");
                if (!wUnits.Equals(""))
                {
                    sb.Append(" AND empl.working_unit_id IN (" + wUnits + ")");
                }

                select = sb.ToString();

                select = select + " ORDER BY user_id ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ApplUser");
                DataTable table = dataSet.Tables["ApplUser"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        applUserTO = new ApplUserTO();

                        if (!row["user_id"].Equals(DBNull.Value))
                        {
                            applUserTO.UserID = row["user_id"].ToString().Trim();
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            applUserTO.Name = row["name"].ToString().Trim();
                        }

                        applUserList.Add(applUserTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return applUserList;
        }

        // TODO!!!!!
        public void serialize(List<ApplUserTO> applUserTOList)
        {
            try
            {
                string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLApplUserFile"];
                Stream stream = File.Open(filename, FileMode.Create);

                ApplUserTO[] applUserTOArray = (ApplUserTO[])applUserTOList.ToArray();

                XmlSerializer bformatter = new XmlSerializer(typeof(ApplUserTO[]));
                bformatter.Serialize(stream, applUserTOArray);
                stream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

    }
}

