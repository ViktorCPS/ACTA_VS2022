using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Collections;
using System.Globalization;
using System.Configuration;
using System.Net;

using Util;
using Common;
using TransferObjects;

namespace SiemensDataAccess
{
    public class MSSQLSiemensReportDAO : SiemensReportDAO
    {
        private static string ConnectionString = "";
        // thread-safe locker
        private static object locker = new object();
        public static SqlConnection conn;
        private SqlConnection tsConnection = null;
        SqlTransaction _sqlTrans = null;

        protected string dateTimeformat = "";
        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public override void SetDBConnection(Object dbConnection)
        {
            conn = dbConnection as SqlConnection;
        }

        public MSSQLSiemensReportDAO()
        {
            lock (locker)
            {
                DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;                
                dateTimeformat = dateTimeFormat.SortableDateTimePattern;

                ConnectionString = ConfigurationManager.AppSettings["connectionStringSiPassReport"];

                byte[] buffer = Convert.FromBase64String(ConnectionString);
                ConnectionString = Util.Misc.decrypt(buffer);

                // ConnectionString contains data provader and it should be ejected from connection string
                int startIndex = -1;

                startIndex = ConnectionString.ToLower().IndexOf("server=");

                if (startIndex >= 0)
                {
                    int connEnd = ConnectionString.ToLower().IndexOf("table");
                    if (connEnd > 0)
                        ConnectionString = ConnectionString.Substring(startIndex, connEnd - startIndex);
                    else
                        ConnectionString = ConnectionString.Substring(startIndex);
                }
            }
        }

        public static SqlConnection getConnection()
        {
            lock (locker)
            {
                try
                {
                    if (conn == null)
                    {
                        // Set connection for the first time
                        conn = new SqlConnection(ConnectionString);
                        conn.Open();
                        return conn;
                    }
                    else
                    {
                        // TODO: Check if connection is closed or opened
                        // Connection already established
                        return conn;
                    }
                }
                catch (InvalidOperationException ioex)
                {
                    conn = null;
                    throw ioex;
                }
                catch (SqlException sqlex)
                {
                    conn = null;
                    throw sqlex;
                }
                catch (Exception ex)
                {
                    conn = null;
                    throw ex;
                }
            }
        }

        public override void CloseConnection()
        {
            lock (locker)
            {
                try
                {
                    if (conn != null) conn.Close();
                }
                catch { }
                finally
                {
                    conn = null;
                }
            }
        }

        public override void CloseConnection(Object dbConnection)
        {
            try
            {
                if (dbConnection != null)
                {
                    SqlConnection conn = dbConnection as SqlConnection;
                    if (conn != null)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            catch { }
        }

        public override Object MakeNewDBConnection()
        {
            lock (locker)
            {
                SqlConnection sqlConnection = null;
                try
                {
                    sqlConnection = new SqlConnection(ConnectionString);
                    sqlConnection.Open();
                    return sqlConnection;
                }
                catch (InvalidOperationException ioex)
                {
                    throw ioex;
                }
                catch (SqlException sqlex)
                {
                    throw sqlex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
     
        public override bool TestDataSourceConnection()
        {
            bool isConnected = false;

            try
            {
                SqlConnection testConn = getConnection();
                if (threadSafe)
                {
                    tsConnection = MakeNewDBConnection() as SqlConnection;
                }
                if (!threadSafe)
                {
                    isConnected = (testConn.State == ConnectionState.Open);
                }
                else
                {
                    isConnected = (((tsConnection != null) && (tsConnection.State == ConnectionState.Open)) &&
                                   (testConn.State == ConnectionState.Open));
                }
            }
            catch
            {
                return isConnected;
            }

            return isConnected;
        }

        public override int insertApplUser(ApplUserTO userTO, bool doCommit)
        {
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            
            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO appl_users ");
                sbInsert.Append("(user_id, password, name, description, privilege_lvl, status, num_of_tries, lang_code, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("N'" + userTO.UserID.Trim() + "', ");
                if (userTO.Password.Trim().Equals(""))
                    sbInsert.Append("null, ");
                else
                    sbInsert.Append("N'" + userTO.Password.Trim() + "', ");
                if (userTO.Name.Trim().Equals(""))
                    sbInsert.Append("null, ");
                else
                    sbInsert.Append("N'" + userTO.Name.Trim() + "', ");
                if (userTO.Description.Trim().Equals(""))
                    sbInsert.Append("null, ");
                else
                    sbInsert.Append("N'" + userTO.Description.Trim() + "', ");
                if (userTO.PrivilegeLvl == -1)
                    sbInsert.Append("null, ");
                else
                    sbInsert.Append("'" + userTO.PrivilegeLvl.ToString().Trim() + "', ");
                sbInsert.Append("N'" + userTO.Status.Trim() + "', ");
                if (userTO.NumOfTries == -1)
                    sbInsert.Append("null, ");
                else
                    sbInsert.Append("'" + userTO.NumOfTries.ToString().Trim() + "', ");                
                if (userTO.LangCode.Trim().Equals(""))
                    sbInsert.Append("null, ");                
                else
                    sbInsert.Append("N'" + userTO.LangCode.Trim() + "', ");                
                sbInsert.Append("'" + userTO.ExitPermVerification.ToString() + "', ");                
                sbInsert.Append("N'" + userTO.CreatedBy.Trim() + "', GETDATE()) ");
                
                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, SqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit)
                    commitTransaction();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    rollbackTransaction();

                throw ex;
            }

            return rowsAffected;
        }

        public override int insertApplUserCategory(ApplUserCategoryTO userCatTO, bool doCommit)
        {
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO appl_users_categories (appl_users_category_id, description, name, created_by, created_time) VALUES (");
                sbInsert.Append("'" + userCatTO.CategoryID.ToString().Trim() + "', ");
                if (!userCatTO.Desc.Trim().Equals(""))
                    sbInsert.Append("N'" + userCatTO.Desc.Trim() + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (!userCatTO.Name.Trim().Equals(""))
                    sbInsert.Append("N'" + userCatTO.Name.Trim() + "', ");
                else
                    sbInsert.Append("NULL, ");

                sbInsert.Append("N'" + userCatTO.CreatedBy.Trim() + "', GETDATE()) ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, SqlTrans);

                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit)
                    commitTransaction();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    rollbackTransaction();

                throw ex;
            }

            return rowsAffected;
        }

        public override int insertApplUserXApplUserCategory(ApplUserXApplUserCategoryTO userXCatTO, bool doCommit)
        {
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO appl_users_x_appl_users_categories (appl_users_category_id, user_id, default_category, created_by, created_time) VALUES (");
                sbInsert.Append("'" + userXCatTO.CategoryID.ToString().Trim() + "', ");
                sbInsert.Append("'" + userXCatTO.UserID.Trim() + "', ");
                sbInsert.Append("'" + userXCatTO.DefaultCategory.ToString().Trim() + "', ");
                sbInsert.Append("N'" + userXCatTO.CreatedBy.Trim() + "', GETDATE()) ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, SqlTrans);

                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit)
                    commitTransaction();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    rollbackTransaction();

                throw ex;
            }

            return rowsAffected;
        }

        public override int insertApplUserXOrganizationalUnit(ApplUserXOrgUnitTO userXOrgUnitTO, bool doCommit)
        {
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            
            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO appl_users_x_organizational_units ");
                sbInsert.Append("(user_id, organizational_unit_id, purpose) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("N'" + userXOrgUnitTO.UserID.Trim() + "', ");
                sbInsert.Append("'" + userXOrgUnitTO.OrgUnitID.ToString().Trim() + "', ");
                sbInsert.Append("N'" + userXOrgUnitTO.Purpose.Trim() + "')");
                
                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, SqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit)                
                    commitTransaction();                
            }
            catch (Exception ex)
            {
                if (doCommit)                
                    rollbackTransaction();                

                throw ex;
            }

            return rowsAffected;
        }

        public override int insertRegAddInfo(uint PK, int siPassID, bool doCommit)
        {
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO IT_Reg_add_info ");
                sbInsert.Append("(PK, sipass_id, created_time) ");
                sbInsert.Append("VALUES ('" + PK.ToString().Trim() + "', '" + siPassID.ToString().Trim() + "', GETDATE())");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, SqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit)
                    commitTransaction();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    rollbackTransaction();

                throw ex;
            }

            return rowsAffected;
        }

        public override int insertKamioniAddInfo(int id, int siPassVechicleID, int siPassDriverID, bool doCommit)
        {
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO IT_kamioni_add_info ");
                sbInsert.Append("(id, vehicle_sipass_id, driver_sipass_id, created_time) ");
                sbInsert.Append("VALUES ('" + id.ToString().Trim() + "', '" + siPassVechicleID.ToString().Trim() + "', '" + siPassDriverID.ToString().Trim() + "', GETDATE())");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, SqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit)
                    commitTransaction();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    rollbackTransaction();

                throw ex;
            }

            return rowsAffected;
        }

        public override int insertVechicleLog(SiemensVechicleLogTO logTO, bool doCommit)
        {
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO vehicle_registrations ");
                sbInsert.Append("(vehicle_sipass_id, driver_sipass_id, event_time, point_id, direction, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("'" + logTO.VechicleID.ToString().Trim() + "', ");
                sbInsert.Append("'" + logTO.DriverID.ToString().Trim() + "', ");
                sbInsert.Append("'" + logTO.EventTime.ToString(dateTimeformat) + "', ");
                sbInsert.Append("'" + logTO.PointID.ToString().Trim() + "', ");
                sbInsert.Append("N'" + logTO.Direction.ToString().Trim() + "', ");                
                sbInsert.Append("GETDATE())");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, SqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit)
                    commitTransaction();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    rollbackTransaction();

                throw ex;
            }

            return rowsAffected;
        }

        public override ApplUserLogTO insertApplUserLog(ApplUserLogTO userTO)
        {
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            int logID = 0;

            try
            {
                userTO.LogInTime = DateTime.Now;

                if (userTO.Host.Trim().Equals(""))
                    userTO.Host = Dns.GetHostName();

                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("SET NOCOUNT ON ");
                sbInsert.Append("INSERT INTO appl_users_log ");
                sbInsert.Append("(user_id, login_time, host, logout_time, created_by, created_time) ");
                sbInsert.Append("VALUES (" + "N'" + userTO.UserID + "', '" + userTO.LogInTime.ToString(dateTimeformat) + "',");
                sbInsert.Append("N'" + userTO.Host.Trim() + "', NULL,");
                sbInsert.Append("N'" + userTO.UserID.Trim() + "', GETDATE())");
                sbInsert.Append("SELECT @@Identity AS login_id ");
                sbInsert.Append("SET NOCOUNT OFF ");

                DataSet dataSet = new DataSet();
                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
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

        public override int insertEmployeeData(SiemensEmployeeTO emplTO, bool doCommit)
        {
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            int rowsAffected = 0;
            string insert = "";

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employee_data ");
                sbInsert.Append("(employee_id, employee_number, first_name, last_name, workgroup_id, card_number, card_status, visitor, jmbg, card_type, po, company, vechile_category, vechile_brand, vechile_color, vechile_type, card_number_visitor, created_by, created_time) ");                
                sbInsert.Append("VALUES (");
                sbInsert.Append("'" + emplTO.EmplID.ToString().Trim() + "', ");
                if (emplTO.ID.Trim().Equals(""))
                    sbInsert.Append("null, ");
                else
                    sbInsert.Append("N'" + emplTO.ID.Trim().Replace("'", "") + "', ");
                if (emplTO.FirstName.Trim().Equals(""))
                    sbInsert.Append("null, ");
                else
                    sbInsert.Append("N'" + emplTO.FirstName.Trim().Replace("'", "") + "', ");
                if (emplTO.LastName.Trim().Equals(""))
                    sbInsert.Append("null, ");
                else
                    sbInsert.Append("N'" + emplTO.LastName.Trim().Replace("'", "") + "', ");
                if (emplTO.DepartmentID == -1)
                    sbInsert.Append("null, ");
                else
                    sbInsert.Append("N'" + emplTO.DepartmentID.ToString().Trim() + "', ");
                if (emplTO.CardNumber.Trim().Equals(""))
                    sbInsert.Append("null, ");
                else
                    sbInsert.Append("N'" + emplTO.CardNumber.Trim().Replace("'", "") + "', ");
                if (emplTO.CardStatus.Trim().Equals(""))
                    sbInsert.Append("null, ");
                else
                    sbInsert.Append("N'" + emplTO.CardStatus.Trim().Replace("'", "") + "', ");
                if (emplTO.Visitor)
                    sbInsert.Append("'" + Constants.yesInt.ToString().Trim() + "', ");
                else
                    sbInsert.Append("'" + Constants.noInt.ToString().Trim() + "', ");
                if (emplTO.JMBG.Trim().Equals(""))
                    sbInsert.Append("null, ");
                else
                    sbInsert.Append("N'" + emplTO.JMBG.Trim().Replace("'", "") + "', ");
                if (emplTO.CardType.Trim().Equals(""))
                    sbInsert.Append("null, ");
                else
                    sbInsert.Append("N'" + emplTO.CardType.Trim().Replace("'", "") + "', ");
                if (emplTO.Po.Trim().Equals(""))
                    sbInsert.Append("null, ");
                else
                    sbInsert.Append("N'" + emplTO.Po.Trim().Replace("'", "") + "', ");
                if (emplTO.Company.Trim().Equals(""))
                    sbInsert.Append("null, ");
                else
                    sbInsert.Append("N'" + emplTO.Company.Trim().Replace("'", "") + "', ");
                if (emplTO.VechicleCategory.Trim().Equals(""))
                    sbInsert.Append("null, ");
                else
                    sbInsert.Append("N'" + emplTO.VechicleCategory.Trim().Replace("'", "") + "', ");
                if (emplTO.VechicleBrand.Trim().Equals(""))
                    sbInsert.Append("null, ");
                else
                    sbInsert.Append("N'" + emplTO.VechicleBrand.Trim().Replace("'", "") + "', ");
                if (emplTO.VechicleColor.Trim().Equals(""))
                    sbInsert.Append("null, ");
                else
                    sbInsert.Append("N'" + emplTO.VechicleColor.Trim().Replace("'", "") + "', ");
                if (emplTO.VechicleType.Trim().Equals(""))
                    sbInsert.Append("null, ");
                else
                    sbInsert.Append("N'" + emplTO.VechicleType.Trim().Replace("'", "") + "', ");
                if (emplTO.CardNumberVisitor.Trim().Equals(""))
                    sbInsert.Append("null, ");
                else
                    sbInsert.Append("N'" + emplTO.CardNumberVisitor.Trim().Replace("'", "") + "', ");
                if (emplTO.CreatedBy.Trim().Equals(""))
                    sbInsert.Append("'', ");
                else
                    sbInsert.Append("N'" + emplTO.CreatedBy.Trim() + "', ");
                if (emplTO.CreatedTime.Equals(new DateTime()))
                    sbInsert.Append("GETDATE()) ");
                else
                    sbInsert.Append("'" + emplTO.CreatedTime.ToString(dateTimeformat) + "') ");

                insert = sbInsert.ToString();
                SqlCommand cmd = new SqlCommand(insert, conn, SqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit)
                    commitTransaction();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    rollbackTransaction();

                throw new Exception (insert + " - " + ex.Message);
            }

            return rowsAffected;
        }

        public override bool updateApplUser(ApplUserTO userTO, bool doCommit)
        {
            bool isUpdated = false;
            
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            
            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE appl_users SET ");
                if (!userTO.Password.Trim().Equals(""))
                    sbUpdate.Append("password = N'" + userTO.Password.Trim() + "', ");
                else
                    sbUpdate.Append("password = NULL, ");
                if (!userTO.Name.Trim().Equals(""))
                    sbUpdate.Append("name = N'" + userTO.Name.Trim() + "', ");
                else
                    sbUpdate.Append("name = null, ");                
                if (!userTO.Description.Trim().Equals(""))
                    sbUpdate.Append("description = N'" + userTO.Description.Trim() + "', ");
                else
                    sbUpdate.Append("description = null, ");
                if (userTO.PrivilegeLvl != -1)
                    sbUpdate.Append("privilege_lvl = '" + userTO.PrivilegeLvl.ToString().Trim() + "', ");
                else
                    sbUpdate.Append("privilege_lvl = null, ");
                sbUpdate.Append("status = N'" + userTO.Status.Trim() + "', ");
                if (userTO.NumOfTries != -1)
                    sbUpdate.Append("num_of_tries = '" + userTO.NumOfTries.ToString().Trim() + "', ");
                else
                    sbUpdate.Append("num_of_tries = null, ");
                if (!userTO.LangCode.Trim().Equals(""))
                    sbUpdate.Append("lang_code = N'" + userTO.LangCode.Trim() + "', ");
                else
                    sbUpdate.Append("lang_code = null, ");
                sbUpdate.Append("modified_by = N'" + userTO.ModifiedBy.Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE user_id = N'" + userTO.UserID.ToString().Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, SqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                    isUpdated = true;
                
                if (doCommit)
                    commitTransaction();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    rollbackTransaction();

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public override int updateApplUserLog(int loginID, string modifiedBy)
        {
            int isUpdated = 0;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE appl_users_log SET ");
                sbUpdate.Append("logout_time = GETDATE(), ");
                sbUpdate.Append("modified_by = N'" + modifiedBy.Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE login_id = '" + loginID.ToString().Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
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

        public override bool setApplUserDefaultCategory(ApplUserXApplUserCategoryTO userXCatTO)
        {
            bool isUpdated = false;

            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbUpdate = new StringBuilder();

                // set all categories to not default
                sbUpdate.Append("UPDATE appl_users_x_appl_users_categories SET ");
                sbUpdate.Append("default_category = '" + Constants.categoryNotDefault.ToString().Trim() + "', ");
                sbUpdate.Append("modified_by = N'" + userXCatTO.ModifiedBy.Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE user_id = '" + userXCatTO.UserID.Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                cmd.ExecuteNonQuery();

                sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE appl_users_x_appl_users_categories SET ");
                sbUpdate.Append("default_category = '" + Constants.categoryDefault.ToString().Trim() + "', ");
                sbUpdate.Append("modified_by = N'" + userXCatTO.ModifiedBy.Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE appl_users_category_id = '" + userXCatTO.CategoryID.ToString().Trim() + "' AND user_id = '" + userXCatTO.UserID.Trim() + "'");

                cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                cmd.ExecuteNonQuery();

                isUpdated = true;

                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw ex;
            }

            return isUpdated;
        }

        public override bool updateEmployeeData(SiemensEmployeeTO emplTO, bool doCommit)
        {
            bool isUpdated = false;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            string update = "";

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE employee_data SET ");
                if (!emplTO.ID.Trim().Equals(""))
                    sbUpdate.Append("employee_number = N'" + emplTO.ID.Trim().Replace("'", "") + "', ");
                else
                    sbUpdate.Append("employee_number = NULL, ");
                if (!emplTO.FirstName.Trim().Equals(""))
                    sbUpdate.Append("first_name = N'" + emplTO.FirstName.Trim().Replace("'", "") + "', ");
                else
                    sbUpdate.Append("first_name = null, ");
                if (!emplTO.LastName.Trim().Equals(""))
                    sbUpdate.Append("last_name = N'" + emplTO.LastName.Trim().Replace("'", "") + "', ");
                else
                    sbUpdate.Append("last_name = null, ");
                if (emplTO.DepartmentID != -1)
                    sbUpdate.Append("workgroup_id = '" + emplTO.DepartmentID.ToString().Trim() + "', ");
                else
                    sbUpdate.Append("workgroup_id = null, ");                
                if (!emplTO.CardNumber.Trim().Equals(""))
                    sbUpdate.Append("card_number = '" + emplTO.CardNumber.Trim().Replace("'", "") + "', ");
                else
                    sbUpdate.Append("card_number = null, ");
                if (!emplTO.CardStatus.Trim().Equals(""))
                    sbUpdate.Append("card_status = N'" + emplTO.CardStatus.Trim().Replace("'", "") + "', ");
                else
                    sbUpdate.Append("card_status = null, ");
                if (!emplTO.JMBG.Trim().Equals(""))
                    sbUpdate.Append("jmbg = N'" + emplTO.JMBG.Trim().Replace("'", "") + "', ");
                else
                    sbUpdate.Append("jmbg = null, ");
                if (!emplTO.CardType.Trim().Equals(""))
                    sbUpdate.Append("card_type = N'" + emplTO.CardType.Trim().Replace("'", "") + "', ");
                else
                    sbUpdate.Append("card_type = null, ");
                if (!emplTO.Po.Trim().Equals(""))
                    sbUpdate.Append("po = N'" + emplTO.Po.Trim().Replace("'", "") + "', ");
                else
                    sbUpdate.Append("po = null, ");
                if (!emplTO.Company.Trim().Equals(""))
                    sbUpdate.Append("company = N'" + emplTO.Company.Trim().Replace("'", "") + "', ");
                else
                    sbUpdate.Append("company = null, ");
                if (!emplTO.VechicleCategory.Trim().Equals(""))
                    sbUpdate.Append("vechile_category = N'" + emplTO.VechicleCategory.Trim().Replace("'", "") + "', ");
                else
                    sbUpdate.Append("vechile_category = null, ");
                if (!emplTO.VechicleBrand.Trim().Equals(""))
                    sbUpdate.Append("vechile_brand = N'" + emplTO.VechicleBrand.Trim().Replace("'", "") + "', ");
                else
                    sbUpdate.Append("vechile_brand = null, ");
                if (!emplTO.VechicleColor.Trim().Equals(""))
                    sbUpdate.Append("vechile_color = N'" + emplTO.VechicleColor.Trim().Replace("'", "") + "', ");
                else
                    sbUpdate.Append("vechile_color = null, ");
                if (!emplTO.VechicleType.Trim().Equals(""))
                    sbUpdate.Append("vechile_type = N'" + emplTO.VechicleType.Trim().Replace("'", "") + "', ");
                else
                    sbUpdate.Append("vechile_type = null, ");
                if (!emplTO.CardNumberVisitor.Trim().Equals(""))
                    sbUpdate.Append("card_number_visitor = N'" + emplTO.CardNumberVisitor.Trim().Replace("'", "") + "', ");
                else
                    sbUpdate.Append("card_number_visitor = null, ");
                if (!emplTO.ModifiedBy.Trim().Equals(""))
                    sbUpdate.Append("modified_by = N'" + emplTO.ModifiedBy.Trim() + "', ");
                else
                    sbUpdate.Append("modified_by = '', ");
                if (!emplTO.ModifiedTime.Equals(new DateTime()))
                    sbUpdate.Append("modified_time = '" + emplTO.ModifiedTime.ToString(dateTimeformat) + "' ");
                else
                    sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE employee_id = '" + emplTO.EmplID.ToString().Trim() + "'");

                update = sbUpdate.ToString();

                SqlCommand cmd = new SqlCommand(update, conn, SqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                    isUpdated = true;

                if (doCommit)
                    commitTransaction();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    rollbackTransaction();

                throw new Exception(update + " - " + ex.Message);
            }

            return isUpdated;
        }
        
        public override bool deleteApplUserXApplUserCategory(string userID, int categoryID, bool doCommit)
        {
            bool isDeleted = false;
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            
            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM appl_users_x_appl_users_categories WHERE ");
                sbDelete.Append(" user_id = '" + userID.Trim() + "'");
                if (categoryID != -1)
                    sbDelete.Append(" AND appl_users_category_id = '" + categoryID + "'");

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, SqlTrans);
                int res = cmd.ExecuteNonQuery();
                isDeleted = true;
                if (doCommit)
                    commitTransaction();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    rollbackTransaction();
                throw ex;
            }

            return isDeleted;
        }

        public override bool deleteApplUserXOrganizationalUnit(string userID, int ouID, string purpose, bool doCommit)
        {
            bool isDeleted = false;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            
            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM appl_users_x_organizational_units WHERE user_id = N'" + userID.ToString().Trim() + "'");
                if (ouID != -1)                
                    sbDelete.Append(" AND organizational_unit_id = '" + ouID.ToString().Trim() + "'");
                if (purpose.Trim() != "")
                    sbDelete.Append(" AND purpose = N'" + purpose.Trim() + "'");
                
                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, SqlTrans);
                int res = cmd.ExecuteNonQuery();
                isDeleted = true;

                if (doCommit)
                    commitTransaction();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    rollbackTransaction();

                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public override bool deleteEmployeeData(string emplIDs, bool doCommit)
        {
            bool isDeleted = false;
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                if (emplIDs.Trim() != "")
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM employee_data WHERE employee_id IN (" + emplIDs.Trim() + ")", conn, SqlTrans);
                    int res = cmd.ExecuteNonQuery();
                }

                if (doCommit)
                    commitTransaction();

                isDeleted = true;
            }
            catch (Exception ex)
            {
                if (doCommit)
                    rollbackTransaction();
                throw ex;
            }

            return isDeleted;
        }
        
        public override Dictionary<int, SiemensEmployeeTO> getEmployeeData(string emplNums, string groupIDs, bool filterVisitors, bool visitor, string cardTypes)
        {
            DataSet dataSet = new DataSet();
            SiemensEmployeeTO emplTO = new SiemensEmployeeTO();
            Dictionary<int, SiemensEmployeeTO> emplDict = new Dictionary<int, SiemensEmployeeTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employee_data ");

                if (emplNums.Trim() != "" || groupIDs.Trim() != "" || filterVisitors || cardTypes.Trim() != "")
                {
                    sb.Append("WHERE ");

                    if (emplNums.Trim() != "")
                        sb.Append(" employee_number IN (" + emplNums.Trim() + ") AND ");

                    if (groupIDs.Trim() != "")
                        sb.Append(" workgroup_id IN (" + groupIDs.Trim() + ") AND ");

                    if (filterVisitors)
                        sb.Append(" visitor = '" + (visitor ? Constants.yesInt.ToString().Trim() : Constants.noInt.ToString().Trim()) + "' AND ");

                    if (cardTypes.Trim() != "")
                        sb.Append(" card_type IN (" + cardTypes.Trim() + ") AND ");

                    select = sb.ToString().Substring(0, sb.ToString().Length - 4);
                }
                else
                    select = sb.ToString();
                
                select += "ORDER BY last_name, first_name";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        emplTO = new SiemensEmployeeTO();
                        if (row["employee_id"] != DBNull.Value)
                            emplTO.EmplID = int.Parse(row["employee_id"].ToString().Trim());
                        if (row["employee_number"] != DBNull.Value)
                            emplTO.ID = row["employee_number"].ToString().Trim();
                        if (row["first_name"] != DBNull.Value)
                            emplTO.FirstName = row["first_name"].ToString().Trim();
                        if (row["last_name"] != DBNull.Value)
                            emplTO.LastName = row["last_name"].ToString().Trim();
                        if (row["workgroup_id"] != DBNull.Value)
                            emplTO.DepartmentID = int.Parse(row["workgroup_id"].ToString().Trim());
                        if (row["card_number"] != DBNull.Value)
                            emplTO.CardNumber = row["card_number"].ToString().Trim();
                        if (row["card_status"] != DBNull.Value)
                            emplTO.CardStatus = row["card_status"].ToString().Trim();
                        if (row["jmbg"] != DBNull.Value)
                            emplTO.JMBG = row["jmbg"].ToString().Trim();
                        if (row["card_type"] != DBNull.Value)
                            emplTO.CardType = row["card_type"].ToString().Trim();
                        if (row["po"] != DBNull.Value)
                            emplTO.Po = row["po"].ToString().Trim();
                        if (row["company"] != DBNull.Value)
                            emplTO.Company = row["company"].ToString().Trim();
                        if (row["visitor"] != DBNull.Value)
                            emplTO.Visitor = int.Parse(row["visitor"].ToString().Trim()) == Constants.SiemensVisitorInt;
                        if (row["vechile_category"] != DBNull.Value)
                            emplTO.VechicleCategory = row["vechile_category"].ToString().Trim();
                        if (row["vechile_brand"] != DBNull.Value)
                            emplTO.VechicleBrand = row["vechile_brand"].ToString().Trim();
                        if (row["vechile_color"] != DBNull.Value)
                            emplTO.VechicleColor = row["vechile_color"].ToString().Trim();
                        if (row["vechile_type"] != DBNull.Value)
                            emplTO.VechicleType = row["vechile_type"].ToString().Trim();
                        if (row["card_number_visitor"] != DBNull.Value)
                            emplTO.CardNumberVisitor = row["card_number_visitor"].ToString().Trim();

                        if (!emplDict.ContainsKey(emplTO.EmplID))
                            emplDict.Add(emplTO.EmplID, emplTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception (select + " - " + ex.Message);
            }

            return emplDict;
        }

        public override List<int> getEmployeeDataIDList()
        {
            DataSet dataSet = new DataSet();
            List<int> idList = new List<int>();

            try
            {
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT employee_id FROM employee_data", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (row["employee_id"] != DBNull.Value)
                            idList.Add(int.Parse(row["employee_id"].ToString().Trim()));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return idList;
        }

        public override Dictionary<int, List<uint>> getRegAddInfo(string PKs, int id)
        {
            DataSet dataSet = new DataSet();
            Dictionary<int, List<uint>> idList = new Dictionary<int, List<uint>>();

            try
            {
                if (PKs.Length > 0 || id != -1)
                {
                    string select = "SELECT * FROM IT_Reg_add_info WHERE";
                    if (PKs.Length > 0)
                        select += " PK IN (" + PKs.Trim() + ") AND";
                    if (id != -1)
                        select += " sipass_id = '" + id.ToString().Trim() + "' AND";

                    select = select.Substring(0, select.Length - 3);

                    SqlCommand cmd = new SqlCommand(select, conn);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "RegAddInfo");
                    DataTable table = dataSet.Tables["RegAddInfo"];

                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            if (row["PK"] != DBNull.Value && row["sipass_id"] != DBNull.Value)
                            {
                                if (!idList.ContainsKey(int.Parse(row["sipass_id"].ToString().Trim())))
                                    idList.Add(int.Parse(row["sipass_id"].ToString().Trim()), new List<uint>());

                                idList[int.Parse(row["sipass_id"].ToString().Trim())].Add(uint.Parse(row["PK"].ToString().Trim()));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return idList;
        }

        public override Dictionary<int, Dictionary<int, List<int>>> getKamioniAddInfo(string ids, int driverID, int truckID)
        {
            DataSet dataSet = new DataSet();
            Dictionary<int, Dictionary<int, List<int>>> idList = new Dictionary<int, Dictionary<int, List<int>>>();

            try
            {
                if (ids.Length > 0 || driverID != -1 || truckID != -1)
                {
                    string select = "SELECT * FROM IT_kamioni_add_info WHERE";
                    if (ids.Length > 0)
                        select += " id IN (" + ids.Trim() + ") AND";
                    if (driverID != -1)
                        select += " driver_sipass_id = '" + driverID.ToString().Trim() + "' AND";
                    if (truckID != -1)
                        select += " vehicle_sipass_id = '" + truckID.ToString().Trim() + "' AND";

                    select = select.Substring(0, select.Length - 3);

                    SqlCommand cmd = new SqlCommand(select, conn);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "RegAddInfo");
                    DataTable table = dataSet.Tables["RegAddInfo"];

                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            if (row["id"] != DBNull.Value && row["driver_sipass_id"] != DBNull.Value && row["vehicle_sipass_id"] != DBNull.Value)
                            {
                                if (!idList.ContainsKey(int.Parse(row["driver_sipass_id"].ToString().Trim())))
                                    idList.Add(int.Parse(row["driver_sipass_id"].ToString().Trim()), new Dictionary<int, List<int>>());

                                if (!idList[int.Parse(row["driver_sipass_id"].ToString().Trim())].ContainsKey(int.Parse(row["vehicle_sipass_id"].ToString().Trim())))
                                    idList[int.Parse(row["driver_sipass_id"].ToString().Trim())].Add(int.Parse(row["vehicle_sipass_id"].ToString().Trim()), new List<int>());

                                idList[int.Parse(row["driver_sipass_id"].ToString().Trim())][int.Parse(row["vehicle_sipass_id"].ToString().Trim())].Add(int.Parse(row["id"].ToString().Trim()));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return idList;
        }

        public override List<SiemensVechicleLogTO> getVechiclePasses(DateTime from, DateTime to)
        {
            DataSet dataSet = new DataSet();
            SiemensVechicleLogTO pass = new SiemensVechicleLogTO();
            List<SiemensVechicleLogTO> passList = new List<SiemensVechicleLogTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM vehicle_registrations");

                if (from != new DateTime() || to != new DateTime())
                {
                    sb.Append(" WHERE ");
                    
                    if (from != new DateTime())
                        sb.Append("event_time >= '" + from.Date.ToString(dateTimeformat) + "' AND ");

                    if (to != new DateTime())
                        sb.Append("event_time < '" + to.Date.AddDays(1).ToString(dateTimeformat) + "' AND ");

                    select = sb.ToString();

                    select = select.Substring(0, select.Length - 4);
                }
                else
                    select = sb.ToString();

                select += " ORDER BY event_time";

                SqlCommand cmd = new SqlCommand(select, conn);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Passes");
                DataTable table = dataSet.Tables["Passes"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pass = new SiemensVechicleLogTO();
                        if (row["vehicle_sipass_id"] != DBNull.Value)
                        {
                            pass.VechicleID = int.Parse(row["vehicle_sipass_id"].ToString().Trim());
                        }
                        if (row["driver_sipass_id"] != DBNull.Value)
                        {
                            pass.DriverID = int.Parse(row["driver_sipass_id"].ToString().Trim());
                        }
                        if (row["event_time"] != DBNull.Value)
                        {
                            pass.EventTime = DateTime.Parse(row["event_time"].ToString());
                        }
                        if (row["point_id"] != DBNull.Value)
                        {
                            pass.PointID = int.Parse(row["point_id"].ToString().Trim());
                        }
                        if (row["direction"] != DBNull.Value)
                        {
                            pass.Direction = row["direction"].ToString();
                        }

                        passList.Add(pass);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return passList;
        }
        
        public override bool beginTransaction()
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

        public override void commitTransaction()
        {
            this.SqlTrans.Commit();
            this.SqlTrans = null;
        }

        public override void rollbackTransaction()
        {
            if (this.SqlTrans != null)
                this.SqlTrans.Rollback();
            this.SqlTrans = null;
        }

        public override IDbTransaction getTransaction()
        {
            return _sqlTrans;
        }

        public override void setTransaction(IDbTransaction trans)
        {
            _sqlTrans = (SqlTransaction)trans;
        }
    }
}
