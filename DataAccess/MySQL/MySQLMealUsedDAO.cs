using System;
using System.Collections;
using System.Configuration;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using TransferObjects;
using Util;
namespace DataAccess
{
    public class MySQLMealUsedDAO:MealUsedDAO
    {
         MySqlConnection conn = null;
		//protected string dateTimeformat = "";
		MySqlTransaction _sqlTrans = null;

		public MySqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

        public MySQLMealUsedDAO()
		{
			conn = MySQLDAOFactory.getConnection();
			DAOController.GetInstance();
		}
        public MySQLMealUsedDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
			DAOController.GetInstance();
        }
        public int insert(string transId, int employeeId, DateTime eventTime, int pointId, int mealTypeId, int quantity, int moneyAmount)
        {
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
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
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");

                DataSet dataSet = new DataSet();
                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);

                rowsAffected = cmd.ExecuteNonQuery();
                sqlTrans.Commit();
            }
            
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw ex;
            }

            return rowsAffected;
        }

        public bool delete(string transID)
        {
            bool isDeleted = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM meals_used WHERE employee_id IN (" + transID + " )");

                MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    isDeleted = true;
                }
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }
        public ArrayList getMealsUsed(int transId, int employeeID, DateTime eventTime, int pointId, int mealTypeID, int quantity, int moneyAmount)
        {
            DataSet dataSet = new DataSet();
            MealUsedTO mealUsed = new MealUsedTO();
            ArrayList mealsTypeList = new ArrayList();
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

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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
                            mealUsed.Quantity = int.Parse(row["qty"].ToString().Trim());
                        }
                        if (row["money_amt"] != DBNull.Value)
                        {
                            mealUsed.MoneyAmount = int.Parse(row["money_amt"].ToString().Trim());
                        }
                        mealsTypeList.Add(mealUsed);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return mealsTypeList;
        }

        public ArrayList getMealsUsed(int employeeID, DateTime from, DateTime to, int pointID, int mealTypeID, int qtyFrom, int qtyTo, int moneyAmtFrom, int moneyAmtTo, string wUnits)
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
                        sb.Append("mu.event_time >= CONVERT('" + from.ToString("yyyy-MM-dd") + "', datetime) AND ");
                        sb.Append("mu.event_time < CONVERT('" + to.AddDays(1).ToString("yyyy-MM-dd") + "', datetime) AND ");
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
                        sb.Append("mu.event_time >= CONVERT('" + from.ToString("yyyy-MM-dd") + "', datetime) AND ");
                        sb.Append("mu.event_time < CONVERT('" + to.AddDays(1).ToString("yyyy-MM-dd") + "', datetime) AND ");
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
					foreach(DataRow row in table.Rows)
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
                    sb.Append("event_time >= CONVERT('" + from.ToString("yyyy-MM-dd") + "', datetime) AND ");
                    sb.Append("event_time < CONVERT('" + to.AddDays(1).ToString("yyyy-MM-dd") + "', datetime) ");
                }
                else
                {
                    DateTime now = DateTime.Now.Date;
                    DateTime firstDay = new DateTime(now.Year, now.Month, 1);
                    DateTime firstDayNextMonth = new DateTime(now.AddMonths(1).Year, now.AddMonths(1).Month, 1);
                    sb.Append("event_time >= CONVERT('" + firstDay.ToString("yyyy-MM-dd") + "', datetime) AND ");
                    sb.Append("event_time < CONVERT('" + firstDayNextMonth.ToString("yyyy-MM-dd") + "', datetime) ");
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

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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


                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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
            throw new NotImplementedException();
        }

        #endregion
    }
}
