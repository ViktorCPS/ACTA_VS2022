using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Data.SqlClient;
using System.Data;
using TransferObjects;
using Util;

namespace DataAccess
{
    public class MSSQLOnlineMealsUsedHistDAO : OnlineMealsUsedHistDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLOnlineMealsUsedHistDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MSSQLOnlineMealsUsedHistDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(OnlineMealsUsedHistTO onlineMealUsedTO, bool doCommit)
        {
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO online_meals_used_hist ");
                sbInsert.Append("(transaction_id,employee_id, event_time, point_id, meal_type_id, qty, online_validation, manual_created, approved, approved_by,approved_time,approved_desc, reason_refused, auto_check, auto_check_time, auto_check_failure_reason, created_by, created_time, modified_by, modified_time) ");
                sbInsert.Append("VALUES (");

                if (onlineMealUsedTO.TransactionID != -1)
                {
                    sbInsert.Append("'" + onlineMealUsedTO.TransactionID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
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
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbInsert.Append("GETDATE() ) ");

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

        public List<OnlineMealsUsedHistTO> getMealsUsed(string employeeIDs, DateTime from, DateTime to, string restaurantIDs, string pointsIDs, string mealTypeIDs, int qtyFrom, int qtyTO, int manual_created, int online_validation, int auto_check, string reasonsReader, string reasonsAutoCheck, string status, string operater, DateTime approvedFrom, DateTime approvedTo, DateTime modifiedFrom, DateTime modifiedTo)
        {
            DataSet dataSet = new DataSet();
            List<OnlineMealsUsedHistTO> mealsList = new List<OnlineMealsUsedHistTO>();
            string select = "";

            try
            {
                OnlineMealsUsedHistTO meal = new OnlineMealsUsedHistTO();

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT used.*,r.name as restaurant, t.name as type, p.name as point FROM online_meals_used_hist used, online_meals_restaurants r,online_meals_points p,online_meals_types t ");
                sb.Append("WHERE used.point_id=p.point_id and p.restaurant_id=r.restaurant_id  and used.meal_type_id=t.meal_type_id AND ");

                if (!employeeIDs.Equals(""))
                {
                    sb.Append("used.employee_id in (" + employeeIDs.Trim().ToUpper() + ") AND ");
                }

                if (!from.Equals(new DateTime(0)) && !to.Equals(new DateTime()))
                {
                    sb.Append("used.event_time >= CONVERT(datetime, '" + from.ToString(dateTimeformat) + "') AND ");
                    sb.Append("used.event_time < CONVERT(datetime, '" + to.ToString(dateTimeformat) + "') AND ");
                }
                else
                {
                    DateTime now = DateTime.Now.Date;
                    DateTime firstDay = new DateTime(now.Year, now.Month, 1);
                    DateTime firstDayNextMonth = new DateTime(now.AddMonths(1).Year, now.AddMonths(1).Month, 1);
                    sb.Append("used.event_time >= CONVERT(datetime, '" + firstDay.ToString(dateTimeformat) + "') AND ");
                    sb.Append("used.event_time < CONVERT(datetime, '" + firstDayNextMonth.ToString(dateTimeformat) + "') AND ");
                }
                if (!modifiedFrom.Equals(new DateTime(0)) && !modifiedTo.Equals(new DateTime()))
                {
                    sb.Append("used.modified_time >= CONVERT(datetime, '" + modifiedFrom.ToString(dateTimeformat) + "') AND ");
                    sb.Append("used.modified_time < CONVERT(datetime, '" + modifiedTo.AddDays(1).ToString(dateTimeformat) + "') AND ");
                }
                if (!restaurantIDs.Equals(""))
                {

                    sb.Append("r.restaurant_id in (" + restaurantIDs.Trim() + ") AND ");
                }
                if (!mealTypeIDs.Equals(""))
                {

                    sb.Append("t.meal_type_id in (" + mealTypeIDs.Trim() + ") AND ");
                }
                if (!pointsIDs.Equals(""))
                {

                    sb.Append("p.point_id in (" + pointsIDs.Trim() + ") AND ");
                }
                if (qtyFrom != -1)
                {
                    sb.Append("used.qty >= '" + qtyFrom + "' AND ");

                }
                if (qtyTO != -1)
                {
                    sb.Append("used.qty <= '" + qtyTO + "' AND ");

                }
                if (manual_created != -1)
                {
                    sb.Append("used.manual_created = '" + manual_created + "' AND ");

                }
                if (online_validation != -1)
                {
                    sb.Append("used.online_validation = '" + online_validation + "' AND ");

                }
                if (auto_check != -1)
                {
                    sb.Append("used.auto_check = '" + auto_check + "' AND ");

                }
                if (!reasonsReader.Equals(""))
                {

                    sb.Append("used.reason_refused in (");
                    string reson = "";
                    foreach (string reason in reasonsReader.Split(','))
                    {
                        reson += "'" + reason + "',";
                    }
                    if (reson.Length > 0)
                        reson = reson.Remove(reson.LastIndexOf(','));

                    sb.Append(reson + ") AND ");
                }
                if (!reasonsAutoCheck.Equals(""))
                {
                    sb.Append("used.auto_check_failure_reason in (");
                    string reson = "";
                    foreach (string reason in reasonsAutoCheck.Split(','))
                    {
                        reson += "'" + reason + "',";
                    }
                    if (reson.Length > 0)
                        reson = reson.Remove(reson.LastIndexOf(','));

                    sb.Append(reson + ") AND ");
                }
                if (!status.Equals(""))
                {
                    sb.Append("used.approved in (");
                    string reson = "";
                    foreach (string reason in status.Split(','))
                    {
                        reson += "'" + reason + "',";
                    }
                    if (reson.Length > 0)
                        reson = reson.Remove(reson.LastIndexOf(','));

                    sb.Append(reson + ") AND ");

                }
                if (!operater.Equals(""))
                {
                    sb.Append("used.approved_by in (");
                    string reson = "";
                    foreach (string reason in operater.Split(','))
                    {
                        reson += "'" + reason + "',";
                    }
                    if (reson.Length > 0)
                        reson = reson.Remove(reson.LastIndexOf(','));

                    sb.Append(reson + ") AND ");

                }

                if (!approvedFrom.Equals(new DateTime(0)) && !approvedTo.Equals(new DateTime()))
                {
                    sb.Append("used.approved_time >= CONVERT(datetime, '" + approvedFrom.ToString(dateTimeformat) + "') AND ");
                    sb.Append("used.approved_time < CONVERT(datetime, '" + approvedTo.AddDays(1).ToString(dateTimeformat) + "') AND ");
                }

                select = sb.ToString(0, sb.ToString().Length - 4);
                select += " ORDER BY used.employee_id ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealsUsed");
                DataTable table = dataSet.Tables["MealsUsed"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        meal = new OnlineMealsUsedHistTO();
                        meal.RecID = Int32.Parse(row["rec_id"].ToString().Trim());
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
                        if (row["point"] != DBNull.Value)
                        {
                            meal.PointName = row["point"].ToString().Trim();
                        }
                        if (row["meal_type_id"] != DBNull.Value)
                        {
                            meal.MealTypeID = Int32.Parse(row["meal_type_id"].ToString().Trim());
                        }
                        if (row["type"] != DBNull.Value)
                        {
                            meal.MealType = row["type"].ToString().Trim();
                        }
                        if (row["qty"] != DBNull.Value)
                        {
                            meal.Qty = Int32.Parse(row["qty"].ToString().Trim());
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
                        if (row["auto_check_failure_reason"] != DBNull.Value)
                        {
                            meal.AutoCheckFailureReason = row["auto_check_failure_reason"].ToString().Trim();
                        }

                        if (row["auto_check"] != DBNull.Value)
                        {
                            meal.AutoCheck = Int32.Parse(row["auto_check"].ToString().Trim());
                        }

                        if (row["online_validation"] != DBNull.Value)
                        {
                            meal.OnlineValidation = Int32.Parse(row["online_validation"].ToString().Trim());
                        }

                        if (row["manual_created"] != DBNull.Value)
                        {
                            meal.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }
                        if (row["approved_time"] != DBNull.Value)
                        {
                            meal.ApprovedTime = DateTime.Parse(row["approved_time"].ToString().Trim());
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            meal.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            meal.ModifiedBy += " " + row["modified_by"].ToString().Trim();
                        }
                        if (row["restaurant"] != DBNull.Value)
                        {
                            meal.RestaurantName += " " + row["restaurant"].ToString().Trim();
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


        public List<OnlineMealsUsedHistTO> getMealsUsed(string transactionID)
        {
            DataSet dataSet = new DataSet();
            List<OnlineMealsUsedHistTO> mealsList = new List<OnlineMealsUsedHistTO>();
            string select = "";

            try
            {
                OnlineMealsUsedHistTO meal = new OnlineMealsUsedHistTO();

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT used.*,r.name as restaurant, t.name as type, p.name as point FROM online_meals_used_hist used, online_meals_restaurants r,online_meals_points p,online_meals_types t ");
                sb.Append("WHERE used.point_id=p.point_id and p.restaurant_id=r.restaurant_id  and used.meal_type_id=t.meal_type_id AND ");

                if (!transactionID.Equals("")) {
                    sb.Append("transaction_id = '"+transactionID+"' AND ");
                }
                select = sb.ToString(0, sb.ToString().Length - 4);
                select += " ORDER BY used.employee_id ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealsUsedHist");
                DataTable table = dataSet.Tables["MealsUsedHist"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        meal = new OnlineMealsUsedHistTO();
                        meal.RecID = Int32.Parse(row["rec_id"].ToString().Trim());
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
                        if (row["point"] != DBNull.Value)
                        {
                            meal.PointName = row["point"].ToString().Trim();
                        }
                        if (row["meal_type_id"] != DBNull.Value)
                        {
                            meal.MealTypeID = Int32.Parse(row["meal_type_id"].ToString().Trim());
                        }
                        if (row["type"] != DBNull.Value)
                        {
                            meal.MealType = row["type"].ToString().Trim();
                        }
                        if (row["qty"] != DBNull.Value)
                        {
                            meal.Qty = Int32.Parse(row["qty"].ToString().Trim());
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
                        if (row["auto_check_failure_reason"] != DBNull.Value)
                        {
                            meal.AutoCheckFailureReason = row["auto_check_failure_reason"].ToString().Trim();
                        }

                        if (row["auto_check"] != DBNull.Value)
                        {
                            meal.AutoCheck = Int32.Parse(row["auto_check"].ToString().Trim());
                        }

                        if (row["online_validation"] != DBNull.Value)
                        {
                            meal.OnlineValidation = Int32.Parse(row["online_validation"].ToString().Trim());
                        }

                        if (row["manual_created"] != DBNull.Value)
                        {
                            meal.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }
                        if (row["approved_time"] != DBNull.Value)
                        {
                            meal.ApprovedTime = DateTime.Parse(row["approved_time"].ToString().Trim());
                        }

                        if (row["restaurant"] != DBNull.Value)
                        {
                            meal.RestaurantName += " " + row["restaurant"].ToString().Trim();
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            meal.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            meal.ModifiedBy += " " + row["modified_by"].ToString().Trim();
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
  
