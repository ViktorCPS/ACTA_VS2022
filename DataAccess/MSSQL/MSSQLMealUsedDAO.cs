using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using TransferObjects;
using Util;
namespace DataAccess
{
    public class MSSQLMealUsedDAO:MealUsedDAO
    {
        SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

		public MSSQLMealUsedDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MSSQLMealUsedDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(string transId, int employeeId, DateTime eventTime, int pointId, int mealTypeId, int quantity, int moneyAmount)
        {
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO meals_used ");
                sbInsert.Append("(trans_id, employee_id, event_time, point_id, meal_type_id, qty, money_amt, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                if (!transId.Equals(""))
                {
                    sbInsert.Append("'" + transId.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (employeeId != -1)
                {
                    sbInsert.Append("'" + employeeId.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (eventTime.Equals(new DateTime(0)))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + eventTime.ToString("yyyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (pointId != -1)
                {
                    sbInsert.Append("'" + pointId.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (mealTypeId != -1)
                {
                    sbInsert.Append("'" + mealTypeId.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (quantity != -1)
                {
                    sbInsert.Append("'" + quantity.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (moneyAmount != -1)
                {
                    sbInsert.Append("'" + moneyAmount.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

                DataSet dataSet = new DataSet();
                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);

                rowsAffected = cmd.ExecuteNonQuery();
                sqlTrans.Commit();

            }
            catch (SqlException sqlEx)
            {
                sqlTrans.Rollback("INSERT");
                if (sqlEx.Number == 2627)
                {
                    throw new Exception(sqlEx.Number.ToString());
                }
                else
                {
                    throw new Exception(sqlEx.Message);
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("INSERT");
                throw ex;
            }

            return rowsAffected;
        }

        public bool delete(string employeeID)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM meals_used WHERE employee_id IN (" + employeeID + " )");

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    isDeleted = true;
                }
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("DELETE");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }
        public ArrayList getMealsUsed(int transId, int employeeID, DateTime eventTime, int pointId, int mealTypeID, int quantity, int moneyAmount)
        {
            DataSet dataSet = new DataSet();
            MealUsedTO mealUsed = new MealUsedTO();
            ArrayList mealsUsedList = new ArrayList();
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM meals_used ");
                if ((employeeID != -1) || (transId != -1) || (!eventTime.Equals(new DateTime())) ||
                                    (pointId != -1) || (mealTypeID != -1) || (quantity != -1) || (moneyAmount != -1))
                {
                    sb.Append("WHERE ");
                    if (employeeID != -1)
                    {
                        sb.Append("employee_id = " + employeeID.ToString().Trim());
                    }
                    if (mealTypeID != -1)
                    {
                        sb.Append("meal_type_id = " + mealTypeID.ToString().Trim());
                    }
                    if (!eventTime.Equals(new DateTime()))
                    {
                        sb.Append(" CONVERT(VARCHAR(24), io_pair_date, 120) = '" + eventTime.ToString("yyy-MM-dd HH:mm:ss").Trim().ToUpper() + "' AND");
                    }
                    if (pointId != -1)
                    {
                        sb.Append("point_id = " + pointId.ToString().Trim());
                    }
                    if (transId != -1)
                    {
                        sb.Append("trans_id = " + transId.ToString().Trim());
                    }
                    if (quantity != -1)
                    {
                        sb.Append("qty = " + quantity.ToString().Trim());
                    }
                    if (moneyAmount != -1)
                    {
                        sb.Append("money_amt = " + moneyAmount.ToString().Trim());
                    }
                }
               
                select = sb.ToString();

                select = select + " ORDER BY employee_id";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealsUsed");
                DataTable table = dataSet.Tables["MealsUsed"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        mealUsed = new MealUsedTO();
                        mealUsed.TransID = row["trans_id"].ToString().Trim();
                        if (row["employee_id"] != DBNull.Value)
                        {
                            mealUsed.EmployeeID = int.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["point_id"] != DBNull.Value)
                        {
                            mealUsed.PointID = int.Parse(row["point_id"].ToString().Trim());
                        }
                        if (row["event_time"] != DBNull.Value)
                        {
                            mealUsed.EventTime = DateTime.Parse(row["event_time"].ToString().Trim());
                        }
                        if (row["meal_type_id"] != DBNull.Value)
                        {
                            mealUsed.MealTypeID = int.Parse(row["meal_type_id"].ToString().Trim());
                        }
                        if (row["qty"] != DBNull.Value)
                        {
                            mealUsed.Quantity= int.Parse(row["qty"].ToString().Trim());
                        }
                        if (row["money_amt"] != DBNull.Value)
                        {
                            mealUsed.MoneyAmount = int.Parse(row["money_amt"].ToString().Trim());
                        }
                        mealsUsedList.Add(mealUsed);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return mealsUsedList;
        }

        public ArrayList getMealsUsed(int employeeID, DateTime from, DateTime to, int pointID, int mealTypeID, int qtyFrom, int qtyTo, int moneyAmtFrom, int moneyAmtTo, string wUnits)
        {
            try
            {
                DataSet dataSet = new DataSet();
                ArrayList mealsList = new ArrayList();
                string selectQ = "";
                string selectM = "";
                string select = "";

                try
                {
                    MealUsedTO meal = new MealUsedTO();

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
                            sb.Append("mu.event_time >= CONVERT(datetime, '" + from.ToString(dateTimeformat) + "') AND ");
                            sb.Append("mu.event_time < CONVERT(datetime, '" + to.AddDays(1).ToString(dateTimeformat) + "') AND ");
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
                        (mealTypeID != -1) || (moneyAmtFrom != -1) || (moneyAmtTo != -1))
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

                        if (moneyAmtFrom != -1)
                        {
                            sb.Append("mu.money_amt >= '" + moneyAmtFrom.ToString().Trim().ToUpper() + "' AND ");
                        }
                        if (moneyAmtTo != -1)
                        {
                            sb.Append("mu.money_amt <= '" + moneyAmtTo.ToString().Trim().ToUpper() + "' AND ");
                        }

                        if (!from.Equals(new DateTime(0)) && !to.Equals(new DateTime(0)))
                        {
                            sb.Append("mu.event_time >= CONVERT(datetime, '" + from.ToString(dateTimeformat) + "') AND ");
                            sb.Append("mu.event_time < CONVERT(datetime, '" + to.AddDays(1).ToString(dateTimeformat) + "') AND ");
                        }
                    }

                    selectM = sb.ToString();

                    selectM += "mu.money_amt <> '0' AND empl.employee_id = mu.employee_id AND mp.point_id = mu.point_id AND "
                        + "mt.meal_type_id = mu.meal_type_id ";

                    if (!wUnits.Equals(""))
                    {
                        selectM += "AND empl.working_unit_id IN (" + wUnits + ") ";
                    }

                    select = "(" + selectQ + ") UNION (" + selectM + ") ORDER BY last_name, first_name, mu.event_time";

                    SqlCommand cmd = new SqlCommand(select, conn);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "MealsUsed");
                    DataTable table = dataSet.Tables["MealsUsed"];

                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            meal = new MealUsedTO();
                            meal.TransID = row["trans_id"].ToString().Trim();

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
                                meal.MealTypeName = row["type_name"].ToString().Trim();
                            }
                            if (row["qty"] != DBNull.Value)
                            {
                                meal.Quantity = Int32.Parse(row["qty"].ToString().Trim());
                            }
                            if (row["money_amt"] != DBNull.Value)
                            {
                                meal.MoneyAmount = Int32.Parse(row["money_amt"].ToString().Trim());
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ArrayList getMealsUsed(int employeeID, DateTime from, DateTime to)
        {
            DataSet dataSet = new DataSet();
            ArrayList mealsList = new ArrayList();
            string select = "";

            try
            {
                MealUsedTO meal = new MealUsedTO();

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM meals_used ");
                sb.Append("WHERE ");

                if (employeeID != -1)
                {
                    sb.Append("employee_id = '" + employeeID.ToString().Trim().ToUpper() + "' AND ");
                }

                if (!from.Equals(new DateTime(0)) && !to.Equals(new DateTime()))
                {
                    sb.Append("event_time >= CONVERT(datetime, '" + from.ToString(dateTimeformat) + "') AND ");
                    sb.Append("event_time < CONVERT(datetime, '" + to.AddDays(1).ToString(dateTimeformat) + "') ");
                }
                else
                {
                    DateTime now = DateTime.Now.Date;
                    DateTime firstDay = new DateTime(now.Year, now.Month, 1);
                    DateTime firstDayNextMonth = new DateTime(now.AddMonths(1).Year, now.AddMonths(1).Month, 1);
                    sb.Append("event_time >= CONVERT(datetime, '" + firstDay.ToString(dateTimeformat) + "') AND ");
                    sb.Append("event_time < CONVERT(datetime, '" + firstDayNextMonth.ToString(dateTimeformat) + "') ");
                }

                select = sb.ToString();

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealsUsed");
                DataTable table = dataSet.Tables["MealsUsed"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        meal = new MealUsedTO();
                        meal.TransID = row["trans_id"].ToString().Trim();

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
                            meal.Quantity = Int32.Parse(row["qty"].ToString().Trim());
                        }
                        if (row["money_amt"] != DBNull.Value)
                        {
                            meal.MoneyAmount = Int32.Parse(row["money_amt"].ToString().Trim());
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

        public int getMealsUsedCount(int transId, int employeeID, DateTime eventTime, int pointId, int mealTypeID, int quantity, int moneyAmount)
        {
            DataSet dataSet = new DataSet();
            int count = 0;
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT count(mu.trans_id) count_mealUsed FROM meals_used mu");
                if ((employeeID != -1) || (transId != -1) || (!eventTime.Equals(new DateTime())) ||
                                    (pointId != -1) || (mealTypeID != -1) || (quantity != -1) || (moneyAmount != -1))
                {
                    sb.Append("WHERE ");
                    if (employeeID != -1)
                    {
                        sb.Append("employee_id = " + employeeID.ToString().Trim());
                    }
                    if (mealTypeID != -1)
                    {
                        sb.Append("meal_type_id = " + mealTypeID.ToString().Trim());
                    }
                    if (!eventTime.Equals(new DateTime()))
                    {
                        sb.Append(" CONVERT(VARCHAR(24), io_pair_date, 120) = '" + eventTime.ToString("yyy-MM-dd HH:mm:ss").Trim().ToUpper() + "' AND");
                    }
                    if (pointId != -1)
                    {
                        sb.Append("employee_id = " + pointId.ToString().Trim());
                    }
                    if (transId != -1)
                    {
                        sb.Append("employee_id = " + transId.ToString().Trim());
                    }
                    if (quantity != -1)
                    {
                        sb.Append("qty = " + quantity.ToString().Trim());
                    }
                    if (moneyAmount != -1)
                    {
                        sb.Append("money_amt = " + moneyAmount.ToString().Trim());
                    }
                }
                select = sb.ToString();

                select = select + " ORDER BY employee_id";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealUsed");
                DataTable table = dataSet.Tables["MealUsed"];

                if (table.Rows.Count > 0)
                {
                    count = Int32.Parse(table.Rows[0]["count_mealUsed"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return count;
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

        public int getTransID()
        {
            DataSet dataSet = new DataSet();
            int transID = 0;
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT MAX(CAST(mu.trans_id AS integer))as max_trans_id FROM meals_used mu");

                select = sb.ToString();


                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealUsed");
                DataTable table = dataSet.Tables["MealUsed"];

                if (table.Rows.Count > 0)
                {
                    transID = Int32.Parse(table.Rows[0]["max_trans_id"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return transID;
        }

        #region MealUsedDAO Members


        public ArrayList getNumerOfUsedMeals(DateTime from, DateTime to, string wuString)
        {
            DataSet dataSet = new DataSet();
            ArrayList mealsList = new ArrayList();
            string select = "";

            try
            {
                MealUsedTO meal = new MealUsedTO();

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT mu.*, empl.first_name, empl.last_name, wu.working_unit_id, wu.name as name FROM meals_used mu, employees empl, working_units wu ");
                sb.Append("WHERE ");

                sb.Append(" mu.employee_id = empl.employee_id AND empl.working_unit_id = wu.working_unit_id ");

                if (wuString != null && !wuString.Equals(""))
                {
                    sb.Append("AND empl.working_unit_id IN (" + wuString + ") ");
                }

                if (!from.Equals(new DateTime(0)) && !to.Equals(new DateTime()))
                {
                    sb.Append("AND ");
                    sb.Append("event_time >= CONVERT(datetime, '" + from.ToString(dateTimeformat) + "') AND ");
                    sb.Append("event_time < CONVERT(datetime, '" + to.AddDays(1).ToString(dateTimeformat) + "') ");
                }
                else
                {
                    sb.Append("AND ");
                    DateTime now = DateTime.Now.Date;
                    DateTime firstDay = new DateTime(now.Year, now.Month, 1);
                    DateTime firstDayNextMonth = new DateTime(now.AddMonths(1).Year, now.AddMonths(1).Month, 1);
                    sb.Append("event_time >= CONVERT(datetime, '" + firstDay.ToString(dateTimeformat) + "') AND ");
                    sb.Append("event_time < CONVERT(datetime, '" + firstDayNextMonth.ToString(dateTimeformat) + "') ");
                }
                select = select + " ORDER BY empl.working_unit_id, empl.employee_id";
                select = sb.ToString();

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealsUsed");
                DataTable table = dataSet.Tables["MealsUsed"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        meal = new MealUsedTO();

                        if (row["employee_id"] != DBNull.Value)
                        {
                            meal.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["qty"] != DBNull.Value)
                        {
                            meal.Quantity = Int32.Parse(row["qty"].ToString().Trim());
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            meal.EmployeeName = row["last_name"].ToString().Trim();
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            meal.EmployeeName += " " + row["first_name"].ToString().Trim();
                        }
                        if (row["working_unit_id"] != DBNull.Value)
                        {
                            meal.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["name"] != DBNull.Value)
                        {
                            meal.WorkingUnit = row["name"].ToString().Trim();
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

        #endregion
    }
}
