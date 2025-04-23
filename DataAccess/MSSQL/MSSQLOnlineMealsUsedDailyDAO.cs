using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;
using TransferObjects;
using Util;

namespace DataAccess
{
    public class MSSQLOnlineMealsUsedDailyDAO : OnlineMealsUsedDailyDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLOnlineMealsUsedDailyDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MSSQLOnlineMealsUsedDailyDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(OnlineMealsUsedTO onlineMealUsedTO, bool doCommit,int transfer_flag)
        {
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO online_meals_used_daily ");
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
               if (transfer_flag != -1)
               {
                   sbInsert.Append("'" + transfer_flag + "')");
               }
               else
               {
                   sbInsert.Append("NULL)");
               }
                DataSet dataSet = new DataSet();
                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, SqlTrans);

                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit)
                    commitTransaction();
            }
            catch (SqlException sqlex)
            {
                if (doCommit)
                    rollbackTransaction();
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

        public int getOnlineMealUsedCount(int employeeID, DateTime from, DateTime to)
        {
            DataSet dataSet = new DataSet();
            string select = "";
            int count = -1;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT COUNT(*) AS count FROM online_meals_used_daily ");
                sb.Append("WHERE online_validation = 1 AND");

                if (employeeID != -1)
                {
                    sb.Append(" employee_id = '" + employeeID.ToString().Trim().ToUpper() + "' AND");
                }

                if (!from.Equals(new DateTime(0)) && !to.Equals(new DateTime()))
                {
                    sb.Append(" event_time >= CONVERT(datetime, '" + from.ToString(dateTimeformat) + "') AND");
                    sb.Append(" event_time < CONVERT(datetime, '" + to.AddDays(1).ToString(dateTimeformat) + "') AND");
                }
                else
                {
                    DateTime now = DateTime.Now.Date;
                    DateTime firstDay = new DateTime(now.Year, now.Month, 1);
                    DateTime firstDayNextMonth = new DateTime(now.AddMonths(1).Year, now.AddMonths(1).Month, 1);
                    sb.Append(" event_time >= CONVERT(datetime, '" + firstDay.ToString(dateTimeformat) + "') AND");
                    sb.Append(" event_time < CONVERT(datetime, '" + firstDayNextMonth.ToString(dateTimeformat) + "') AND");
                }

                select = sb.ToString(0, sb.ToString().Length - 3);

                SqlCommand cmd = new SqlCommand(select, conn);
               
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "OnlineMealsUsed");
                DataTable table = dataSet.Tables["OnlineMealsUsed"];

                if (table.Rows.Count > 0)
                {
                    count = Int32.Parse(table.Rows[0]["count"].ToString().Trim());
                }
            }
            catch(Exception ex)
            {
                count = -1;
                throw ex;
            }

            return count;
        }

        public List<OnlineMealsUsedTO> getMealsToTransfer()
        {
            DataSet dataSet = new DataSet();
            List<OnlineMealsUsedTO> mealsList = new List<OnlineMealsUsedTO>();
            string select = "";

            try
            {
                OnlineMealsUsedTO meal = new OnlineMealsUsedTO();

                StringBuilder sb = new StringBuilder();
                select = "SELECT * FROM online_meals_used_daily WHERE transfer_flag = '" + Constants.noInt.ToString().Trim() + "' ORDER BY transaction_id ";

                SqlCommand cmd = new SqlCommand(select, conn);
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

        public bool updateToTransfered(string transIDs, bool doCommit)
        {
            bool updated = true;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                if (transIDs.Trim().Length <= 0)
                    return updated;

                string updateString = "UPDATE online_meals_used_daily SET transfer_flag = '" + Constants.yesInt.ToString().Trim() + "' WHERE transaction_id IN (" + transIDs.Trim() + ")";

                SqlCommand cmd = new SqlCommand(updateString.Trim(), conn, SqlTrans);

                updated = cmd.ExecuteNonQuery() >= 0;

                if (doCommit)
                    commitTransaction();
            }
            catch (SqlException sqlex)
            {
                if (doCommit)
                    rollbackTransaction();
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

        public bool deletePreviousDay(bool doCommit)
        {
            int rowsAffected = 0;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                string select = "DELETE FROM online_meals_used_daily WHERE event_time < '" + DateTime.Now.Date.ToString(dateTimeformat) + "' AND transfer_flag = '" + Constants.yesInt.ToString().Trim() + "'";

                SqlCommand cmd = new SqlCommand(select.Trim(), conn, SqlTrans);

                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit)
                    commitTransaction();
            }
            catch (SqlException sqlex)
            {
                if (doCommit)
                    rollbackTransaction();
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
                    rollbackTransaction();
                throw ex;
            }

            return rowsAffected >= 0;
        }

        public int findByStatus(int online_validation, DateTime date, int point_id)
        {
            DataSet dataSet = new DataSet();
            int numMeals = 0;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM online_meals_used_daily used ");
                sb.Append("WHERE online_validation = '" + online_validation + "' AND created_by = 'RC SERVICE' ");
                if (date != new DateTime())
                    sb.Append("AND event_time >= '" + date.Date.ToString(dateTimeformat) + "' AND event_time < '" + date.Date.AddDays(1).ToString(dateTimeformat) + "' ");
                if (point_id != -1)
                    sb.Append("AND point_id = '" + point_id + "'");

                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "OnlineMealsUsed");
                DataTable table = dataSet.Tables["OnlineMealsUsed"];
                numMeals = table.Rows.Count;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return numMeals;
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
