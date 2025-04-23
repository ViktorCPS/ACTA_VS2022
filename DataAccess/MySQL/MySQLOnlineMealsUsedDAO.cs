using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Globalization;

using TransferObjects;
using Util;

namespace DataAccess
{
    public class MySQLOnlineMealsUsedDAO : OnlineMealsUsedDAO
    {
        MySqlConnection conn = null;
        protected string dateTimeformat = "";
        MySqlTransaction _sqlTrans = null;

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MySQLOnlineMealsUsedDAO()
        {
            conn = MySQLDAOFactory.getConnection();
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public MySQLOnlineMealsUsedDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public OnlineMealsUsedTO find(string transactionID)
        {
            DataSet dataSet = new DataSet();
            OnlineMealsUsedTO meal = new OnlineMealsUsedTO();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT used.*,r.name as restaurant, t.name as type, p.name as point FROM online_meals_used used, online_meals_restaurants r,online_meals_points p,online_meals_types t ");
                sb.Append("WHERE used.point_id=p.point_id and p.restaurant_id=r.restaurant_id  and used.meal_type_id=t.meal_type_id AND transaction_id = '" + transactionID.Trim() + "' ");
                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "OnlineMealsUsed");
                DataTable table = dataSet.Tables["OnlineMealsUsed"];

                if (table.Rows.Count == 1)
                {
                    meal = new OnlineMealsUsedTO();
                    DataRow row = table.Rows[0];

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
                    if (row["restaurant"] != DBNull.Value)
                    {
                        meal.RestaurantName += " " + row["restaurant"].ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Exception: " + ex.Message);
            }

            return meal;
        }

        public int findByStatus(int online_validation, DateTime date, int point_id)
        {
            DataSet dataSet = new DataSet();
            int numMeals = 0;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM online_meals_used used ");
                sb.Append("WHERE online_validation = '" + online_validation + "'  and created_by='RC SERVICE' ");
                if (date != new DateTime())
                {

                    sb.Append("AND event_time >= '" + date.ToString(dateTimeformat) + "'");
                }
                if (point_id != -1)
                {
                    sb.Append("AND point_id = '" + point_id + "'");
                }
                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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

        public int insert(OnlineMealsUsedTO onlineMealUsedTO, bool doCommit)
        {
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO online_meals_used ");
                sbInsert.Append("(employee_id, event_time, point_id, meal_type_id, qty, online_validation, manual_created, approved, approved_by,approved_time,approved_desc, reason_refused, auto_check, auto_check_time, auto_check_failure_reason, created_by, created_time, modified_by, modified_time) ");
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
                    sbInsert.Append("'" + DAOController.GetLogInUser().Trim() + "',");
                }
                if (onlineMealUsedTO.CreatedTime != new DateTime())
                {
                    sbInsert.Append("'" + onlineMealUsedTO.CreatedTime.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbInsert.Append("NOW(),");
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
                    sbInsert.Append("'" + onlineMealUsedTO.ModifiedTime.ToString(dateTimeformat) + "')");
                }
                else
                {
                    sbInsert.Append("NULL)");
                }

                DataSet dataSet = new DataSet();
                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, SqlTrans);

                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit)
                    SqlTrans.Commit();
            }

            catch (Exception ex)
            {
                if (doCommit)
                    SqlTrans.Rollback();
                throw ex;
            }

            return rowsAffected;
        }

        public bool update(OnlineMealsUsedTO onlineMealsUsed, bool doCommit)
        {
            bool isUpdated = false;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE online_meals_used SET ");
                if (onlineMealsUsed.EmployeeID != -1)
                    sbUpdate.Append("employee_id = '" + onlineMealsUsed.EmployeeID.ToString().Trim() + "', ");
                if (onlineMealsUsed.PointID != -1)
                    sbUpdate.Append("point_id = '" + onlineMealsUsed.PointID.ToString().Trim() + "', ");
                if (onlineMealsUsed.MealTypeID != -1)
                    sbUpdate.Append("meal_type_id = '" + onlineMealsUsed.MealTypeID.ToString().Trim() + "', ");
                if (!onlineMealsUsed.EventTime.Equals(new DateTime()))
                    sbUpdate.Append("event_time = '" + onlineMealsUsed.EventTime.ToString(dateTimeformat) + "', ");
                if (onlineMealsUsed.Qty != -1)
                    sbUpdate.Append("qty = '" + onlineMealsUsed.Qty.ToString().Trim() + "', ");
                if (onlineMealsUsed.OnlineValidation != -1)
                    sbUpdate.Append("online_validation = N'" + onlineMealsUsed.OnlineValidation.ToString().Trim() + "', ");
                else
                    sbUpdate.Append("online_validation = NULL, ");
                if (onlineMealsUsed.ManualCreated != -1)
                    sbUpdate.Append("manual_created = '" + onlineMealsUsed.ManualCreated.ToString().Trim() + "', ");
                if (!onlineMealsUsed.Approved.Equals(""))
                    sbUpdate.Append("approved = N'" + onlineMealsUsed.Approved.Trim() + "', ");
                else
                    sbUpdate.Append("approved = NULL, ");
                if (!onlineMealsUsed.ApprovedBy.Equals(""))
                    sbUpdate.Append("approved_by = N'" + onlineMealsUsed.ApprovedBy.Trim() + "', ");
                else
                    sbUpdate.Append("approved_by = NULL, ");
                if (!onlineMealsUsed.ApprovedDesc.Equals(""))
                    sbUpdate.Append("approved_desc = N'" + onlineMealsUsed.ApprovedDesc.Trim() + "', ");
                else
                    sbUpdate.Append("approved_desc = NULL, ");
                if (onlineMealsUsed.ApprovedTime != new DateTime())
                    sbUpdate.Append("approved_time = '" + onlineMealsUsed.ApprovedTime.ToString(dateTimeformat) + "', ");
                else
                    sbUpdate.Append("approved_time = NULL, ");
                if (onlineMealsUsed.AutoCheck != -1)
                    sbUpdate.Append("auto_check = N'" + onlineMealsUsed.AutoCheck.ToString().Trim() + "', ");
                else
                    sbUpdate.Append("auto_check = NULL, ");
                if (!onlineMealsUsed.AutoCheckFailureReason.Equals(""))
                    sbUpdate.Append("auto_check_failure_reason = N'" + onlineMealsUsed.AutoCheckFailureReason.Trim() + "', ");
                else
                    sbUpdate.Append("auto_check_failure_reason = NULL, ");
                if (onlineMealsUsed.AutoCheckTime != new DateTime())
                    sbUpdate.Append("auto_check_time = '" + onlineMealsUsed.AutoCheckTime.ToString(dateTimeformat) + "', ");
                else
                    sbUpdate.Append("auto_check_time = NULL, ");
                if (!onlineMealsUsed.ModifiedBy.Equals(""))
                {
                    sbUpdate.Append("modified_by = N'" + onlineMealsUsed.ModifiedBy.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().ToString().Trim() + "', ");
                }

                sbUpdate.Append("modified_time = NOW() ");
                sbUpdate.Append("WHERE transaction_id = '" + onlineMealsUsed.TransactionID.ToString().Trim() + "'");
                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, SqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res >= 0)
                {
                    isUpdated = true;
                }

                if (doCommit)
                    SqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    SqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public List<OnlineMealsUsedTO> getMealsUsed(string employeeIDs, DateTime from, DateTime to, string restaurantIDs, string pointsIDs, string mealTypeIDs, int qtyFrom, int qtyTO, int manual_created, int online_validation, int auto_check, string reasonsReader, string reasonsAutoCheck, string status, string operater, DateTime approvedFrom, DateTime approvedTo)
        {
            DataSet dataSet = new DataSet();
            List<OnlineMealsUsedTO> mealsList = new List<OnlineMealsUsedTO>();
            string select = "";

            try
            {
                OnlineMealsUsedTO meal = new OnlineMealsUsedTO();

                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT used.*,r.name as restaurant, t.name as type, p.name as point FROM online_meals_used used, online_meals_restaurants r,online_meals_points p,online_meals_types t ");
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

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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
        public int getOnlineMealUsedCount(int employeeID, DateTime from, DateTime to)
        {
            DataSet dataSet = new DataSet();
            string select = "";
            int count = -1;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT COUNT(*) AS count FROM online_meals_used ");
                sb.Append("WHERE ");

                if (employeeID != -1)
                {
                    sb.Append("employee_id = '" + employeeID.ToString().Trim().ToUpper() + "' AND ");
                }

                if (!from.Equals(new DateTime(0)) && !to.Equals(new DateTime()))
                {
                    sb.Append("event_time >= CONVERT('" + from.ToString(dateTimeformat) + "', datetime) AND ");
                    sb.Append("event_time < CONVERT('" + to.AddDays(1).ToString(dateTimeformat) + "', datetime) ");
                }
                else
                {
                    DateTime now = DateTime.Now.Date;
                    DateTime firstDay = new DateTime(now.Year, now.Month, 1);
                    DateTime firstDayNextMonth = new DateTime(now.AddMonths(1).Year, now.AddMonths(1).Month, 1);
                    sb.Append("event_time >= CONVERT('" + firstDay.ToString(dateTimeformat) + "', datetime) AND ");
                    sb.Append("event_time < CONVERT('" + firstDayNextMonth.ToString(dateTimeformat) + "', datetime) ");
                }

                select = sb.ToString();
                //MySqlCommand cmd = new SqlCommand(select, conn);
                MySqlCommand cmd;
                if (this.SqlTrans == null)
                    cmd = new MySqlCommand(select, conn);
                else
                    cmd = new MySqlCommand(select, conn, this.SqlTrans);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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
        public List<OnlineMealsUsedTO> getMealsUsed(int employeeID, DateTime from, DateTime to)
        {
            DataSet dataSet = new DataSet();
            List<OnlineMealsUsedTO> mealsList = new List<OnlineMealsUsedTO>();
            string select = "";

            try
            {
                OnlineMealsUsedTO meal = new OnlineMealsUsedTO();

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM online_meals_used ");
                sb.Append("WHERE ");

                if (employeeID != -1)
                {
                    sb.Append("employee_id = '" + employeeID.ToString().Trim().ToUpper() + "' AND ");
                }

                if (!from.Equals(new DateTime(0)) && !to.Equals(new DateTime()))
                {
                    sb.Append("event_time >= CONVERT('" + from.ToString(dateTimeformat) + "', datetime) AND ");
                    sb.Append("event_time < CONVERT('" + to.AddDays(1).ToString(dateTimeformat) + "', datetime) ");
                }
                else
                {
                    DateTime now = DateTime.Now.Date;
                    DateTime firstDay = new DateTime(now.Year, now.Month, 1);
                    DateTime firstDayNextMonth = new DateTime(now.AddMonths(1).Year, now.AddMonths(1).Month, 1);
                    sb.Append("event_time >= CONVERT('" + firstDay.ToString(dateTimeformat) + "', datetime) AND ");
                    sb.Append("event_time < CONVERT('" + firstDayNextMonth.ToString(dateTimeformat) + "', datetime) ");
                }

                select = sb.ToString();

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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

        public List<OnlineMealsUsedTO> getMealsUsed(string employeeIDs, DateTime from, DateTime to)
        {
            DataSet dataSet = new DataSet();
            List<OnlineMealsUsedTO> mealsList = new List<OnlineMealsUsedTO>();
            string select = "";

            try
            {
                OnlineMealsUsedTO meal = new OnlineMealsUsedTO();

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM online_meals_used ");
                if (employeeIDs.Trim() != "" || !from.Equals(new DateTime()) && !to.Equals(new DateTime()))
                {
                    sb.Append("WHERE ");

                    if (employeeIDs.Trim() != "")
                        sb.Append("employee_id IN (" + employeeIDs.Trim() + ") AND ");

                    if (!from.Equals(new DateTime()))
                        sb.Append("event_time >= '" + from.ToString(dateTimeformat) + "' AND ");

                    if (!to.Equals(new DateTime()))
                        sb.Append("event_time <= '" + to.ToString(dateTimeformat) + "' AND ");

                    select = sb.ToString(0, sb.ToString().Length - 4);
                }

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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

        public bool beginTransaction()
        {
            bool isStarted = false;

            try
            {
                this.SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
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

        public List<OnlineMealsUsedTO> getMealsUsed(int employeeID, DateTime from, DateTime to, int pointID, int mealTypeID, int qtyFrom, int qtyTo, string wUnits)
        {
            DataSet dataSet = new DataSet();
            List<OnlineMealsUsedTO> mealsList = new List<OnlineMealsUsedTO>();
            string selectQ = "";
            string selectM = "";
            string select = "";

            try
            {
                OnlineMealsUsedTO meal = new OnlineMealsUsedTO();

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT mu.*, mt.name type_name, mp.name point_name, empl.first_name first_name, empl.last_name last_name ");
                sb.Append("FROM meals_used mu, meals_type mt, meals_points mp, employees empl ");
                sb.Append("WHERE ");

                if ((employeeID != -1) || (pointID != -1) ||
                    (!from.Equals(new DateTime(0))) || (!to.Equals(new DateTime(0))) ||
                    (mealTypeID != -1) || (qtyFrom != -1) || (qtyTo != -1))
                {
                    if (employeeID != -1)
                    {
                        sb.Append("mu.employee_id = '" + employeeID.ToString().Trim().ToUpper() + "' AND ");
                    }
                    if (pointID != -1)
                    {
                        sb.Append("mu.point_id = '" + pointID.ToString().Trim().ToUpper() + "' AND ");
                    }
                    if (mealTypeID != -1)
                    {
                        sb.Append("mu.meal_type_id = '" + mealTypeID.ToString().Trim().ToUpper() + "' AND ");
                    }

                    if (qtyFrom != -1)
                    {
                        sb.Append("mu.qty >= '" + qtyFrom.ToString().Trim().ToUpper() + "' AND ");
                    }
                    if (qtyTo != -1)
                    {
                        sb.Append("mu.qty <= '" + qtyTo.ToString().Trim().ToUpper() + "' AND ");
                    }

                    if (!from.Equals(new DateTime(0)) && !to.Equals(new DateTime(0)))
                    {
                        sb.Append("mu.event_time >= CONVERT('" + from.ToString(dateTimeformat) + "', datetime) AND ");
                        sb.Append("mu.event_time < CONVERT('" + to.AddDays(1).ToString(dateTimeformat) + "', datetime) AND ");
                    }
                }

                selectQ = sb.ToString();

                selectQ += "mu.qty <> '0' AND empl.employee_id = mu.employee_id AND mp.point_id = mu.point_id AND "
                    + "mt.meal_type_id = mu.meal_type_id ";

                if (!wUnits.Equals(""))
                {
                    selectQ += "AND empl.working_unit_id IN (" + wUnits + ") ";
                }

                sb = new StringBuilder();
                sb.Append("SELECT mu.*, mt.name type_name, mp.name point_name, empl.first_name first_name, empl.last_name last_name ");
                sb.Append("FROM meals_used mu, meals_type mt, meals_points mp, employees empl ");
                sb.Append("WHERE ");

                if ((employeeID != -1) || (pointID != -1) ||
                    (!from.Equals(new DateTime(0))) || (!to.Equals(new DateTime(0))) ||
                    (mealTypeID != -1))
                {
                    if (employeeID != -1)
                    {
                        sb.Append("mu.employee_id = '" + employeeID.ToString().Trim().ToUpper() + "' AND ");
                    }
                    if (pointID != -1)
                    {
                        sb.Append("mu.point_id = '" + pointID.ToString().Trim().ToUpper() + "' AND ");
                    }
                    if (mealTypeID != -1)
                    {
                        sb.Append("mu.meal_type_id = '" + mealTypeID.ToString().Trim().ToUpper() + "' AND ");
                    }
                    if (!from.Equals(new DateTime(0)) && !to.Equals(new DateTime(0)))
                    {
                        sb.Append("mu.event_time >= CONVERT('" + from.ToString(dateTimeformat) + "', datetime) AND ");
                        sb.Append("mu.event_time < CONVERT('" + to.AddDays(1).ToString(dateTimeformat) + "', datetime) AND ");
                    }
                }

                selectM = sb.ToString();

                selectM += "mu.money_amt <> '0' AND empl.employee_id = mu.employee_id AND mp.point_id = mu.point_id AND "
                    + "mt.meal_type_id = mu.meal_type_id ";

                if (!wUnits.Equals(""))
                {
                    selectM += "AND empl.working_unit_id IN (" + wUnits + ") ";
                }

                select = "(" + selectQ + ") UNION (" + selectM + ") ORDER BY last_name, first_name, event_time";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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
                        if (row["point_name"] != DBNull.Value)
                        {
                            meal.PointName = row["point_name"].ToString().Trim();
                        }
                        if (row["meal_type_id"] != DBNull.Value)
                        {
                            meal.MealTypeID = Int32.Parse(row["meal_type_id"].ToString().Trim());
                        }
                        if (row["type_name"] != DBNull.Value)
                        {
                            meal.MealType = row["type_name"].ToString().Trim();
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
                        if (row["last_name"] != DBNull.Value)
                        {
                            meal.EmployeeName = row["last_name"].ToString().Trim();
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            meal.EmployeeName += " " + row["first_name"].ToString().Trim();
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

        public Dictionary<int, Dictionary<DateTime, List<OnlineMealsUsedTO>>> getMealsUsedDict(string emplIDs, DateTime from, DateTime to)
        {
            try
            {
                DataSet dataSet = new DataSet();
                Dictionary<int, Dictionary<DateTime, List<OnlineMealsUsedTO>>> mealsDict = new Dictionary<int, Dictionary<DateTime, List<OnlineMealsUsedTO>>>();
                string select = "";

                try
                {
                    if (emplIDs.Trim().Equals(""))
                        return mealsDict;

                    OnlineMealsUsedTO meal = new OnlineMealsUsedTO();

                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT * FROM online_meals_used WHERE employee_id IN (" + emplIDs.Trim() + ") ");

                    if (!from.Equals(new DateTime()))
                        sb.Append("AND event_time >= '" + from.ToString(dateTimeformat) + "' ");
                    if (!to.Equals(new DateTime()))
                        sb.Append("AND event_time < '" + to.ToString(dateTimeformat) + "' ");

                    select = sb.ToString();

                    MySqlCommand cmd = new MySqlCommand(select + " ORDER BY employee_id, event_time", conn);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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

                            if (mealsDict.ContainsKey(meal.EmployeeID))
                                mealsDict.Add(meal.EmployeeID, new Dictionary<DateTime, List<OnlineMealsUsedTO>>());

                            if (!mealsDict[meal.EmployeeID].ContainsKey(meal.EventTime.Date))
                                mealsDict[meal.EmployeeID].Add(meal.EventTime.Date, new List<OnlineMealsUsedTO>());

                            mealsDict[meal.EmployeeID][meal.EventTime.Date].Add(meal);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return mealsDict;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dictionary<int, Dictionary<DateTime, List<OnlineMealsUsedTO>>> getMealsForValidation(DateTime from, DateTime to, string emplIDs)
        {
            try
            {
                DataSet dataSet = new DataSet();
                Dictionary<int, Dictionary<DateTime, List<OnlineMealsUsedTO>>> mealsDict = new Dictionary<int, Dictionary<DateTime, List<OnlineMealsUsedTO>>>();
                string select = "";

                try
                {
                    if (emplIDs.Trim().Equals(""))
                        return mealsDict;

                    OnlineMealsUsedTO meal = new OnlineMealsUsedTO();

                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT * FROM online_meals_used WHERE employee_id IN (" + emplIDs.Trim() + ") ");
                    sb.Append("AND ((UPPER(approved) = '" + Constants.MealNotApproved.Trim().ToUpper() + "' AND UPPER(approved_by) = '" + Constants.AutoCheckUser.ToUpper().Trim()
                        + "') OR approved IS NULL) ");

                    if (!from.Equals(new DateTime()))
                        sb.Append("AND event_time >= '" + from.ToString(dateTimeformat) + "' ");
                    if (!to.Equals(new DateTime()))
                        sb.Append("AND event_time <= '" + to.ToString(dateTimeformat) + "' ");

                    select = sb.ToString();

                    MySqlCommand cmd = new MySqlCommand(select + " ORDER BY employee_id, event_time", conn);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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
                            if (row["point_id"] != DBNull.Value)
                            {
                                meal.PointID = Int32.Parse(row["point_id"].ToString().Trim());
                            }
                            if (row["meal_type_id"] != DBNull.Value)
                            {
                                meal.MealTypeID = Int32.Parse(row["meal_type_id"].ToString().Trim());
                            }
                            if (row["event_time"] != DBNull.Value)
                            {
                                meal.EventTime = DateTime.Parse(row["event_time"].ToString().Trim());
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

                            if (!mealsDict.ContainsKey(meal.EmployeeID))
                                mealsDict.Add(meal.EmployeeID, new Dictionary<DateTime, List<OnlineMealsUsedTO>>());
                            if (!mealsDict[meal.EmployeeID].ContainsKey(meal.EventTime.Date))
                                mealsDict[meal.EmployeeID].Add(meal.EventTime.Date, new List<OnlineMealsUsedTO>());

                            mealsDict[meal.EmployeeID][meal.EventTime.Date].Add(meal);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return mealsDict;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dictionary<int, Dictionary<DateTime, List<OnlineMealsUsedTO>>> getMealsValidated(DateTime from, DateTime to, DateTime fromValidation, DateTime toValidation, string emplIDs)
        {
            try
            {
                DataSet dataSet = new DataSet();
                Dictionary<int, Dictionary<DateTime, List<OnlineMealsUsedTO>>> mealsDict = new Dictionary<int, Dictionary<DateTime, List<OnlineMealsUsedTO>>>();
                string select = "";

                try
                {
                    if (emplIDs.Trim().Equals("") || from.Equals(new DateTime()) || to.Equals(new DateTime()) || fromValidation.Equals(new DateTime()) || toValidation.Equals(new DateTime()))
                        return mealsDict;

                    OnlineMealsUsedTO meal = new OnlineMealsUsedTO();

                    select += "SELECT * FROM online_meals_used WHERE employee_id IN (" + emplIDs.Trim() + ") ";
                    select += "AND ((approved IS NOT NULL AND UPPER(approved_by) <> '" + Constants.AutoCheckUser.ToUpper().Trim() + "' ";
                    select += "AND event_time >= '" + fromValidation.ToString(dateTimeformat) + "' ";
                    select += "AND event_time <= '" + toValidation.ToString(dateTimeformat) + "') ";
                    select += "OR (event_time >= '" + from.ToString(dateTimeformat) + "' AND event_time < '" + fromValidation.ToString(dateTimeformat) + "') ";
                    select += "OR (event_time > '" + toValidation.ToString(dateTimeformat) + "' AND event_time < '" + to.ToString(dateTimeformat) + "'))";

                    MySqlCommand cmd = new MySqlCommand(select + " ORDER BY employee_id, event_time", conn);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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
                            if (row["point_id"] != DBNull.Value)
                            {
                                meal.PointID = Int32.Parse(row["point_id"].ToString().Trim());
                            }
                            if (row["meal_type_id"] != DBNull.Value)
                            {
                                meal.MealTypeID = Int32.Parse(row["meal_type_id"].ToString().Trim());
                            }
                            if (row["event_time"] != DBNull.Value)
                            {
                                meal.EventTime = DateTime.Parse(row["event_time"].ToString().Trim());
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

                            if (!mealsDict.ContainsKey(meal.EmployeeID))
                                mealsDict.Add(meal.EmployeeID, new Dictionary<DateTime, List<OnlineMealsUsedTO>>());
                            if (!mealsDict[meal.EmployeeID].ContainsKey(meal.EventTime.Date))
                                mealsDict[meal.EmployeeID].Add(meal.EventTime.Date, new List<OnlineMealsUsedTO>());

                            mealsDict[meal.EmployeeID][meal.EventTime.Date].Add(meal);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return mealsDict;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<OnlineMealsUsedTO> getMealsUsed(string IDs)
        {
            try
            {
                DataSet dataSet = new DataSet();
                List<OnlineMealsUsedTO> mealsList = new List<OnlineMealsUsedTO>();
                string select = "";

                try
                {
                    if (IDs.Trim().Equals(""))
                        return mealsList;

                    OnlineMealsUsedTO meal = new OnlineMealsUsedTO();

                    StringBuilder sb = new StringBuilder();
                    select = "SELECT * FROM online_meals_used WHERE transaction_id IN (" + IDs.Trim() + ") ";

                    MySqlCommand cmd = new MySqlCommand(select + " ORDER BY employee_id, event_time", conn);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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
                            if (row["point_id"] != DBNull.Value)
                            {
                                meal.PointID = Int32.Parse(row["point_id"].ToString().Trim());
                            }
                            if (row["meal_type_id"] != DBNull.Value)
                            {
                                meal.MealTypeID = Int32.Parse(row["meal_type_id"].ToString().Trim());
                            }
                            if (row["event_time"] != DBNull.Value)
                            {
                                meal.EventTime = DateTime.Parse(row["event_time"].ToString().Trim());
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
