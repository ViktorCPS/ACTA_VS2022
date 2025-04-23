using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Globalization;
using DataAccess;
using Util;
using TransferObjects;

namespace ReaderRemoteDataAccess
{
    public class MSSQLReaderRemoteDAO : ReaderRemoteDAO
    {
        public static string ConnectionString = "";
        // thread-safe locker
        private static object locker = new object();
        public static SqlConnection connection;
        private SqlConnection tsConnection = null;
        protected string dateTimeformat = "";
        SqlTransaction _sqlTrans = null;
        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLReaderRemoteDAO()
        {
            lock (locker)
            {
                // Decrypt user name and password
                //Crypt cr = new Crypt();

                // Decrypt connection string
                ConnectionString = ConfigurationManager.AppSettings["restaurantConnectionString"];

                byte[] buffer = Convert.FromBase64String(ConnectionString);
                ConnectionString = Util.Misc.decrypt(buffer);

                // ConnectionString contains data provader and it should be ejected from connection string
                int startIndex = -1;

                startIndex = ConnectionString.ToLower().IndexOf("server=");

                if (startIndex >= 0)
                {
                    ConnectionString = ConnectionString.Substring(startIndex);
                }
                DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
                dateTimeformat = dateTimeFormat.SortableDateTimePattern;
                //ConnectionString = cr.DecryptConnectionString(ConnectionString);
            }
        }

        public override bool TestDataSourceConnection()
        {
            bool isConnected = false;

            try
            {
                SqlConnection testConn = getConnection();

                tsConnection = MakeNewDBConnection() as SqlConnection;

            }
            catch
            {
                return isConnected;
            }

            return isConnected;
        }

        public static SqlConnection getConnection()
        {
            lock (locker)
            {
                try
                {
                    if (connection == null)
                    {
                        // Set connection for the first time
                        connection = new SqlConnection(ConnectionString);
                        connection.Open();
                        return connection;
                    }
                    else
                    {
                        // TODO: Check if connection is closed or opened
                        // Connection already established
                        return connection;
                    }
                }
                catch (InvalidOperationException ioex)
                {
                    connection = null;
                    throw ioex;
                }
                catch (SqlException sqlex)
                {
                    connection = null;
                    throw sqlex;
                }
                catch (Exception ex)
                {
                    connection = null;
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
                    if (connection != null) connection.Close();
                }
                catch { }
                finally
                {
                    connection = null;
                }
            }
        }

        public override void CloseConnection(object dbConnection)
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

        public override void setDBConnection(object dbConnection)
        {
            try
            {
                if (dbConnection != null)
                {
                    connection = dbConnection as SqlConnection;
                }
            }
            catch { }
        }

        public override object MakeNewDBConnection()
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

        public override int Count(int employeeID, DateTime from, DateTime to)
        {
            DataSet dataSet = new DataSet();
            string select = "";
            int count = -1;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT COUNT(*) AS count FROM online_meals_used ");
                sb.Append("WHERE online_validation = 1 AND ");

                if (employeeID != -1)
                {
                    sb.Append("employee_id = '" + employeeID.ToString().Trim().ToUpper() + "' AND ");
                }

                if (!from.Equals(new DateTime(0)) && !to.Equals(new DateTime()))
                {
                    sb.Append("event_time >= CONVERT(datetime, '" + from.ToString(dateTimeformat) + "') AND ");
                    sb.Append("event_time < CONVERT(datetime, '" + to.AddDays(1).ToString(dateTimeformat) + "') AND");
                }
                else
                {
                    DateTime now = DateTime.Now.Date;
                    DateTime firstDay = new DateTime(now.Year, now.Month, 1);
                    DateTime firstDayNextMonth = new DateTime(now.AddMonths(1).Year, now.AddMonths(1).Month, 1);
                    sb.Append("event_time >= CONVERT(datetime, '" + firstDay.ToString(dateTimeformat) + "') AND ");
                    sb.Append("event_time < CONVERT(datetime, '" + firstDayNextMonth.ToString(dateTimeformat) + "') AND");
                }

                select = sb.ToString(0, sb.ToString().Length - 3);

                //SqlCommand cmd = new SqlCommand(select, conn);
                SqlCommand cmd = new SqlCommand(select, connection);


                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "OnlineMealsUsed");
                DataTable table = dataSet.Tables["OnlineMealsUsed"];

                if (table.Rows.Count > 0)
                {
                    count = Int32.Parse(table.Rows[0]["count"].ToString().Trim());
                }
            }
            catch
            {
                count = -1;
            }

            return count;
        }

        public override int Save(OnlineMealsUsedTO onlineMealUsedTO, bool doCommit, int transferFlag)
        {
            if (doCommit)
                SqlTrans = connection.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO online_meals_used ");
                sbInsert.Append("(employee_id, event_time, point_id, meal_type_id, qty, online_validation, manual_created, approved, approved_by,approved_time,approved_desc, reason_refused, auto_check, auto_check_time, auto_check_failure_reason, created_by, created_time, modified_by, modified_time,transfer_flag) ");
                sbInsert.Append("VALUES (");

                if (onlineMealUsedTO.EmployeeID != -1)
                {
                    sbInsert.Append("'" + onlineMealUsedTO.EmployeeID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (onlineMealUsedTO.EventTime.Equals(new DateTime(0)))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + onlineMealUsedTO.EventTime.ToString(dateTimeformat).Trim() + "', ");
                }
                if (onlineMealUsedTO.PointID != -1)
                {
                    sbInsert.Append("'" + onlineMealUsedTO.PointID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (onlineMealUsedTO.MealTypeID != -1)
                {
                    sbInsert.Append("'" + onlineMealUsedTO.MealTypeID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (onlineMealUsedTO.Qty != -1)
                {
                    sbInsert.Append("'" + onlineMealUsedTO.Qty.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (onlineMealUsedTO.OnlineValidation != -1)
                {
                    sbInsert.Append("'" + onlineMealUsedTO.OnlineValidation + "', ");
                }
                else
                {
                    sbInsert.Append("NULL,");
                }
                if (onlineMealUsedTO.ManualCreated != -1)
                {
                    sbInsert.Append("'" + onlineMealUsedTO.ManualCreated + "', ");
                }
                else
                {
                    sbInsert.Append("NULL,");
                }
                if (!onlineMealUsedTO.Approved.Equals(""))
                {
                    sbInsert.Append("N'" + onlineMealUsedTO.Approved + "', ");
                }
                else
                {
                    sbInsert.Append("NULL,");
                }
                if (!onlineMealUsedTO.ApprovedBy.Equals(""))
                {
                    sbInsert.Append("N'" + onlineMealUsedTO.ApprovedBy + "', ");
                }
                else
                {
                    sbInsert.Append("NULL,");
                }
                if (onlineMealUsedTO.ApprovedTime != new DateTime())
                {
                    sbInsert.Append("'" + onlineMealUsedTO.ApprovedTime.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbInsert.Append("NULL,");
                }
                if (!onlineMealUsedTO.ApprovedDesc.Equals(""))
                {
                    sbInsert.Append("N'" + onlineMealUsedTO.ApprovedDesc + "', ");
                }
                else
                {
                    sbInsert.Append("NULL,");
                }
                if (!onlineMealUsedTO.ReasonRefused.Equals(""))
                {
                    sbInsert.Append("N'" + onlineMealUsedTO.ReasonRefused + "', ");
                }
                else
                {
                    sbInsert.Append("NULL,");
                }
                if (onlineMealUsedTO.AutoCheck != -1)
                {
                    sbInsert.Append("'" + onlineMealUsedTO.AutoCheck.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (onlineMealUsedTO.AutoCheckTime != new DateTime())
                {
                    sbInsert.Append("'" + onlineMealUsedTO.AutoCheckTime.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbInsert.Append("NULL,");
                }
                if (!onlineMealUsedTO.AutoCheckFailureReason.Equals(""))
                {
                    sbInsert.Append("N'" + onlineMealUsedTO.AutoCheckFailureReason + "', ");
                }
                else
                {
                    sbInsert.Append("NULL,");
                }
                if (!onlineMealUsedTO.CreatedBy.Equals(""))
                {
                    sbInsert.Append("N'" + onlineMealUsedTO.CreatedBy + "', ");
                }
                else
                {
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                }
                if (onlineMealUsedTO.CreatedTime != new DateTime())
                {
                    sbInsert.Append("'" + onlineMealUsedTO.CreatedTime.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbInsert.Append("GETDATE(), ");
                }
                if (!onlineMealUsedTO.ModifiedBy.Equals(""))
                {
                    sbInsert.Append("N'" + onlineMealUsedTO.ModifiedBy + "', ");
                }
                else
                {
                    sbInsert.Append("NULL,");
                }

                if (onlineMealUsedTO.ModifiedTime != new DateTime())
                {
                    sbInsert.Append("'" + onlineMealUsedTO.ModifiedTime.ToString(dateTimeformat) + "',");
                }
                else
                {
                    sbInsert.Append("NULL,");
                }
                if (transferFlag != -1)
                {
                    sbInsert.Append("'" + transferFlag + "')");
                }
                else
                {
                    sbInsert.Append("NULL)");
                }
                DataSet dataSet = new DataSet();
                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), connection, SqlTrans);

                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit)
                    SqlTrans.Commit();
            }
            catch (SqlException sqlex)
            {
                if (doCommit)
                    SqlTrans.Rollback("INSERT");
                if (sqlex.Number == Constants.TCPErrorNumber || sqlex.Number == Constants.ConnectionErrorNumber || sqlex.Message.Contains(Constants.ConnectionErrorString))
                {
                    throw new Exception(Constants.noConnectionEng);
                }
                else
                    throw sqlex;
            }
            catch (Exception ex)
            {
                if (doCommit)
                    SqlTrans.Rollback("INSERT");
                throw ex;
            }

            return rowsAffected;
        }

        public override List<OnlineMealsUsedTO> GetMealsToTransfer()
        {
            DataSet dataSet = new DataSet();
            List<OnlineMealsUsedTO> mealsList = new List<OnlineMealsUsedTO>();
            string select = "";

            try
            {
                OnlineMealsUsedTO meal = new OnlineMealsUsedTO();

                StringBuilder sb = new StringBuilder();
                select = "SELECT * FROM online_meals_used WHERE transfer_flag = '" + Constants.noInt.ToString().Trim() + "' ORDER BY transaction_id ";

                SqlCommand cmd = new SqlCommand(select, connection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealsUsed");
                DataTable table = dataSet.Tables["MealsUsed"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        meal = new OnlineMealsUsedTO();
                        meal.TransactionID = Int32.Parse(row["transaction_id"].ToString().Trim());
                        if (row["employee_id"] != DBNull.Value)
                        {
                            meal.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["event_time"] != DBNull.Value)
                        {
                            meal.EventTime = DateTime.Parse(row["event_time"].ToString());
                        }
                        if (row["point_id"] != DBNull.Value)
                        {
                            meal.PointID = Int32.Parse(row["point_id"].ToString().Trim());
                        }                        
                        if (row["meal_type_id"] != DBNull.Value)
                        {
                            meal.MealTypeID = Int32.Parse(row["meal_type_id"].ToString().Trim());
                        }                        
                        if (row["qty"] != DBNull.Value)
                        {
                            meal.Qty = Int32.Parse(row["qty"].ToString().Trim());
                        }
                        if (row["online_validation"] != DBNull.Value)
                        {
                            meal.OnlineValidation = Int32.Parse(row["online_validation"].ToString().Trim());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            meal.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }
                        if (row["reason_refused"] != DBNull.Value)
                        {
                            meal.ReasonRefused = row["reason_refused"].ToString().Trim();
                            //meal.ReasonRefused = meal.ReasonRefused.Substring(8);
                        }
                        if (row["approved"] != DBNull.Value)
                        {
                            meal.Approved = row["approved"].ToString().Trim();
                        }
                        if (row["approved_desc"] != DBNull.Value)
                        {
                            meal.ApprovedDesc = row["approved_desc"].ToString().Trim();
                        }
                        if (row["approved_by"] != DBNull.Value)
                        {
                            meal.ApprovedBy = row["approved_by"].ToString().Trim();
                        }
                        if (row["approved_time"] != DBNull.Value)
                        {
                            meal.ApprovedTime = DateTime.Parse(row["approved_time"].ToString().Trim());
                        }
                        if (row["auto_check"] != DBNull.Value)
                        {
                            meal.AutoCheck = Int32.Parse(row["auto_check"].ToString().Trim());
                        }
                        if (row["auto_check_time"] != DBNull.Value)
                        {
                            meal.AutoCheckTime = DateTime.Parse(row["auto_check_time"].ToString().Trim());
                        }
                        if (row["auto_check_failure_reason"] != DBNull.Value)
                        {
                            meal.AutoCheckFailureReason = row["auto_check_failure_reason"].ToString().Trim();
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            meal.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            meal.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            meal.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            meal.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }

                        mealsList.Add(meal);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return mealsList;
        }

        public override bool UpdateToTransfered(string transIDs, bool doCommit)
        {
            bool updated = true;

            if (doCommit)
                SqlTrans = connection.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

            try
            {
                if (transIDs.Trim().Length <= 0)
                    return updated;

                string updateString = "UPDATE online_meals_used SET transfer_flag = '" + Constants.yesInt.ToString().Trim() + "' WHERE transaction_id IN (" + transIDs.Trim() + ")";

                SqlCommand cmd = new SqlCommand(updateString.Trim(), connection, SqlTrans);

                updated = cmd.ExecuteNonQuery() >= 0;

                if (doCommit)
                    SqlTrans.Commit();
            }
            catch (SqlException sqlex)
            {
                if (doCommit)
                    SqlTrans.Rollback("UPDATE");
                if (sqlex.Number == Constants.TCPErrorNumber || sqlex.Number == Constants.ConnectionErrorNumber || sqlex.Message.Contains(Constants.ConnectionErrorString))
                {
                    throw new Exception(Constants.noConnectionEng);
                }
                else
                    throw sqlex;
            }
            catch (Exception ex)
            {
                if (doCommit)
                    SqlTrans.Rollback("UPDATE");
                throw ex;
            }

            return updated;
        }

        public override bool DeletePreviousDay(bool doCommit)
        {
            int rowsAffected = 0;

            if (doCommit)
                SqlTrans = connection.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");
            
            try
            {
                string select = "DELETE FROM online_meals_used WHERE event_time < '" + DateTime.Now.Date.ToString(dateTimeformat) + "'";
                
                SqlCommand cmd = new SqlCommand(select.Trim(), connection, SqlTrans);

                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit)
                    SqlTrans.Commit();
            }
            catch (SqlException sqlex)
            {
                if (doCommit)
                    SqlTrans.Rollback("DELETE");
                if (sqlex.Number == Constants.TCPErrorNumber || sqlex.Number == Constants.ConnectionErrorNumber || sqlex.Message.Contains(Constants.ConnectionErrorString))
                {
                    throw new Exception(Constants.noConnectionEng);
                }
                else
                    throw sqlex;
            }
            catch (Exception ex)
            {
                if (doCommit)
                    SqlTrans.Rollback("DELETE");
                throw ex;
            }

            return rowsAffected >= 0;
        }
    }
}
