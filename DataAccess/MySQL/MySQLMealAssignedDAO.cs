using System;
using System.Collections;
using System.Data;
using MySql.Data.MySqlClient;
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
    public class MySQLMealAssignedDAO:MealAssignedDAO
    {
        MySqlConnection conn = null;
		//protected string dateTimeformat = "";
		MySqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

		public MySqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

        public MySQLMealAssignedDAO()
		{
			conn = MySQLDAOFactory.getConnection();
			DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
            
		}
        public MySQLMealAssignedDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
			DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public bool insert(int employeeID, int mealTypeID, DateTime validFrom, DateTime validTo, int quantity, int quantityDaily, int moneyAmount)
        {
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            int rowsAffected = 0;
            bool inserted = false;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO meals_assigned ");
                sbInsert.Append("(employee_id, meal_type_id, valid_from, valid_to, qty, qty_daily, money_amt, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                if (employeeID != -1)
                {
                    sbInsert.Append("'" + employeeID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (mealTypeID != -1)
                {
                    sbInsert.Append("'" + mealTypeID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (validFrom.Equals(new DateTime(0)))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + validFrom.ToString(dateTimeformat).Trim() + "', ");
                }
                if (validTo.Equals(new DateTime(0)))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + validTo.ToString(dateTimeformat).Trim() + "', ");
                }
                
                    sbInsert.Append("'" + quantity.ToString().Trim() + "', ");
               
                
                    sbInsert.Append("'" + quantityDaily.ToString().Trim() + "', ");              
                
              
                    sbInsert.Append("'" + moneyAmount.ToString().Trim() + "', ");
             
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");

                DataSet dataSet = new DataSet();
                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);

                rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    inserted = true;
                }
                sqlTrans.Commit();

            }
           
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw ex;
            }

            return inserted;
        }

        public bool delete(string recID)
        {
            bool isDeleted = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM meals_assigned WHERE rec_id = '" + recID + "'");

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

        public ArrayList getMealsAssigned(string employeeID, string mealTypeID, DateTime validFrom, DateTime validTo)
        {
            DataSet dataSet = new DataSet();
            MealAssignedTO mealAssigned = new MealAssignedTO();
            ArrayList mealsAssignedList = new ArrayList();
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM meals_assigned ");
                if ((!employeeID.Equals("")) || (!mealTypeID.Equals("")) || (!validFrom.Equals(new DateTime())) ||
                    (!validTo.Equals(new DateTime())))
                {
                    sb.Append(" WHERE");
                    if (!employeeID.Equals(""))
                    {
                        sb.Append(" employee_id IN (" + employeeID + ") AND");
                    }
                    if (!mealTypeID.Equals(""))
                    {
                        sb.Append(" meal_type_id IN (" + mealTypeID + ") AND");
                    }
                    if (!validFrom.Equals(new DateTime()))
                    {
                        sb.Append(" ((valid_from <= convert('" + validFrom.ToString("yyyy-MM-dd") + "', datetime) AND");
                        sb.Append("  valid_to >= convert('" + validFrom.ToString("yyyy-MM-dd") + "', datetime)) OR");
                    }
                    if (!validTo.Equals(new DateTime()))
                    {
                        sb.Append(" (valid_from <= convert('" + validTo.ToString("yyyy-MM-dd") + "', datetime) AND");
                        sb.Append("  valid_to >= convert('" + validTo.ToString("yyyy-MM-dd") + "', datetime))) AND");
                    }
                    sb.Append("  valid_from <> '1900-01-01 00:00:00.000' AND");
                    sb.Append(" valid_to <> '2099-01-01 00:00:00.000' ");
                }
                select = sb.ToString();

                select = select + " ORDER BY employee_id";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealAssigned");
                DataTable table = dataSet.Tables["MealAssigned"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        mealAssigned = new MealAssignedTO();
                        mealAssigned.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        mealAssigned.MealTypeID = Int32.Parse(row["meal_type_id"].ToString().Trim());
                        if (row["qty"] != DBNull.Value)
                        {
                            mealAssigned.Quantity = int.Parse(row["qty"].ToString().Trim());
                        }
                        if (row["qty_daily"] != DBNull.Value)
                        {
                            mealAssigned.QuantityDaily = int.Parse(row["qty_daily"].ToString().Trim());
                        }
                        if (row["money_amt"] != DBNull.Value)
                        {
                            mealAssigned.MoneyAmount = int.Parse(row["money_amt"].ToString().Trim());
                        }
                        if (row["valid_from"] != DBNull.Value)
                        {
                            mealAssigned.ValidFrom = DateTime.Parse(row["valid_from"].ToString().Trim());
                        }
                        if (row["valid_to"] != DBNull.Value)
                        {
                            mealAssigned.ValidTo = DateTime.Parse(row["valid_to"].ToString().Trim());
                        }
                        mealsAssignedList.Add(mealAssigned);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return mealsAssignedList;
        }

        public ArrayList getMealsAssigned(string employeeID)
        {
            DataSet dataSet = new DataSet();
            MealAssignedTO mealAssigned = new MealAssignedTO();
            ArrayList mealsAssignedList = new ArrayList();
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ma.*, mt.name FROM meals_assigned ma, meals_type mt WHERE ma.meal_type_id = mt.meal_type_id AND ");
                if (!employeeID.Equals(""))
                {
                    sb.Append("employee_id = '" + employeeID + "' AND ");
                }
                    
                sb.Append("valid_from <= convert('" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "', datetime) AND ");
                sb.Append("valid_to >= convert('" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "', datetime)");
                    
                select = sb.ToString();

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealAssigned");
                DataTable table = dataSet.Tables["MealAssigned"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        mealAssigned = new MealAssignedTO();
                        mealAssigned.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["meal_type_id"] != DBNull.Value)
                        {
                            mealAssigned.MealTypeID = Int32.Parse(row["meal_type_id"].ToString().Trim());
                        }
                        if (row["name"] != DBNull.Value)
                        {
                            mealAssigned.MealTypeName = row["name"].ToString().Trim();
                        }
                        if (row["qty"] != DBNull.Value)
                        {
                            mealAssigned.Quantity = Int32.Parse(row["qty"].ToString().Trim());
                        }
                        if (row["qty_daily"] != DBNull.Value)
                        {
                            mealAssigned.QuantityDaily = Int32.Parse(row["qty_daily"].ToString().Trim());
                        }
                        if (row["money_amt"] != DBNull.Value)
                        {
                            mealAssigned.MoneyAmount = Int32.Parse(row["money_amt"].ToString().Trim());
                        }
                        if (row["valid_from"] != DBNull.Value)
                        {
                            mealAssigned.ValidFrom = DateTime.Parse(row["valid_from"].ToString().Trim());
                        }
                        if (row["valid_to"] != DBNull.Value)
                        {
                            mealAssigned.ValidTo = DateTime.Parse(row["valid_to"].ToString().Trim());
                        }
                        mealsAssignedList.Add(mealAssigned);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return mealsAssignedList;
        }

        public int getMealsAssignedCount(string employeeID, string mealTypeID, DateTime validFrom, DateTime validTo)
        {
            DataSet dataSet = new DataSet();
            int count = 0;
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT count(ma.rec_id) count_mealsAssigned FROM meals_assigned ma");
                if ((!employeeID.Equals("")) || (!mealTypeID.Equals("")) || (!validFrom.Equals(new DateTime())) ||
                    (!validTo.Equals(new DateTime())))
                {
                    sb.Append(" WHERE");
                    if (!employeeID.Equals(""))
                    {
                        sb.Append(" employee_id IN (" + employeeID + ") AND");
                    }
                    if (!mealTypeID.Equals(""))
                    {
                        sb.Append(" meal_type_id IN (" + mealTypeID + ") AND");
                    }
                    if (!validFrom.Equals(new DateTime()))
                    {
                        sb.Append(" ((valid_from <= convert('" + validFrom.ToString("yyyy-MM-dd") + "', datetime) AND");
                        sb.Append("  valid_to >= convert('" + validFrom.ToString("yyyy-MM-dd") + "', datetime)) OR");
                    }
                    if (!validTo.Equals(new DateTime()))
                    {
                        sb.Append(" (valid_from <= convert('" + validTo.ToString("yyyy-MM-dd") + "', datetime) AND");
                        sb.Append("  valid_to >= convert('" + validTo.ToString("yyyy-MM-dd") + "', datetime))) AND");
                    }
                    sb.Append("  valid_from <> '1900-01-01 00:00:00.000' AND");
                    sb.Append(" valid_to <> '2099-01-01 00:00:00.000' ");
                }
                select = sb.ToString();

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealAssigned");
                DataTable table = dataSet.Tables["MealAssigned"];

                if (table.Rows.Count > 0)
                {
                    count = Int32.Parse(table.Rows[0]["count_mealsAssigned"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return count;
        }

        public ArrayList getMealsAssigned(int employeeID, int mealTypeID, DateTime validFrom, DateTime validTo,
            int qtyFrom, int qtyTo, int qtyDailyFrom, int qtyDailyTo, int mAmtFrom, int mAmtTo, bool unlimited)
        {
            DataSet dataSet = new DataSet();
            MealAssignedTO mealAssigned = new MealAssignedTO();
            ArrayList mealsAssignedList = new ArrayList();
            string selectQ = "";
            string selectM = "";
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ma.*, mt.name mt_name, e.first_name, e.last_name ");
                sb.Append("FROM meals_assigned ma, meals_type mt, employees e ");
                sb.Append("WHERE ma.qty <> '0' AND mt.meal_type_id = ma.meal_type_id AND ma.employee_id = e.employee_id AND ");
                
                if ((employeeID != -1) || (mealTypeID != -1) || (!validFrom.Equals(new DateTime())) ||
                    (!validTo.Equals(new DateTime())) || (qtyFrom != -1) || (qtyTo != -1) || 
                    (qtyDailyFrom != -1) || (qtyDailyTo != -1))
                {
                    if (employeeID != -1)
                    {
                        sb.Append("ma.employee_id = '" + employeeID.ToString() + "' AND ");
                    }
                    if (mealTypeID != -1)
                    {
                        sb.Append("ma.meal_type_id = '" + mealTypeID.ToString() + "' AND ");
                    }
                    if (!validFrom.Equals(new DateTime()) && !validTo.Equals(new DateTime()))
                    {
                        sb.Append("((valid_from < CONVERT('" + validFrom.AddDays(1).ToString("yyyy-MM-dd") + "', datetime) AND ");
                        sb.Append("valid_to >= CONVERT('" + validFrom.ToString("yyyy-MM-dd") + "', datetime)) OR ");
                        sb.Append("(valid_from < CONVERT('" + validTo.AddDays(1).ToString("yyyy-MM-dd") + "', datetime) AND ");
                        sb.Append("valid_to >= CONVERT('" + validTo.ToString("yyyy-MM-dd") + "', datetime)) OR ");
                        sb.Append("(valid_from >= CONVERT('" + validFrom.ToString("yyyy-MM-dd") + "', datetime) AND ");
                        sb.Append("valid_to < CONVERT('" + validTo.AddDays(1).ToString("yyyy-MM-dd") + "', datetime))) AND ");

                        if (!unlimited)
                        {
                            sb.Append("valid_from <> '1900-01-01 00:00:00.000' AND ");
                            sb.Append("valid_to <> '2099-01-01 00:00:00.000' AND ");
                        }
                    }
                    if (qtyFrom != -1)
                    {
                        sb.Append("qty >= '" + qtyFrom.ToString() + "' AND ");
                    }
                    if (qtyTo != -1)
                    {
                        sb.Append("qty <= '" + qtyTo.ToString() + "' AND ");
                    }
                    if (qtyDailyFrom != -1)
                    {
                        sb.Append("qty_daily >= '" + qtyDailyFrom.ToString() + "' AND ");
                    }
                    if (qtyDailyTo != -1)
                    {
                        sb.Append("qty_daily <= '" + qtyDailyTo.ToString() + "' AND ");
                    }
                }

                selectQ = sb.ToString().Substring(0, sb.ToString().Length - 4);

                sb = new StringBuilder();
                sb.Append("SELECT ma.*, mt.name mt_name, e.first_name, e.last_name ");
                sb.Append("FROM meals_assigned ma, meals_type mt, employees e ");
                sb.Append("WHERE ma.money_amt <> '0' AND mt.meal_type_id = ma.meal_type_id AND ma.employee_id = e.employee_id AND ");

                if ((employeeID != -1) || (mealTypeID != -1) || (!validFrom.Equals(new DateTime())) ||
                    (!validTo.Equals(new DateTime())) || (mAmtFrom != -1) || (mAmtTo != -1))
                {
                    if (employeeID != -1)
                    {
                        sb.Append("ma.employee_id = '" + employeeID.ToString() + "' AND ");
                    }
                    if (mealTypeID != -1)
                    {
                        sb.Append("ma.meal_type_id = '" + mealTypeID.ToString() + "' AND ");
                    }
                    if (!validFrom.Equals(new DateTime()) && !validTo.Equals(new DateTime()))
                    {
                        sb.Append("((valid_from < CONVERT('" + validFrom.AddDays(1).ToString("yyyy-MM-dd") + "', datetime) AND ");
                        sb.Append("valid_to >= CONVERT('" + validFrom.ToString("yyyy-MM-dd") + "', datetime)) OR ");
                        sb.Append("(valid_from < CONVERT('" + validTo.AddDays(1).ToString("yyyy-MM-dd") + "', datetime) AND ");
                        sb.Append("valid_to >= CONVERT('" + validTo.ToString("yyyy-MM-dd") + "', datetime)) OR ");
                        sb.Append("(valid_from >= CONVERT('" + validFrom.ToString("yyyy-MM-dd") + "', datetime) AND ");
                        sb.Append("valid_to < CONVERT('" + validTo.AddDays(1).ToString("yyyy-MM-dd") + "', datetime))) AND ");

                        if (!unlimited)
                        {
                            sb.Append("valid_from <> '1900-01-01 00:00:00.000' AND ");
                            sb.Append("valid_to <> '2099-01-01 00:00:00.000' AND ");
                        }
                    }
                    if (mAmtFrom != -1)
                    {
                        sb.Append("money_amt >= '" + mAmtFrom.ToString() + "' AND ");
                    }
                    if (mAmtTo != -1)
                    {
                        sb.Append("money_amt <= '" + mAmtTo.ToString() + "' AND ");
                    }
                }

                selectM = sb.ToString().Substring(0, sb.ToString().Length - 4);

                select = "(" + selectQ + ") UNION (" + selectM + ") ORDER BY last_name";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealAssigned");
                DataTable table = dataSet.Tables["MealAssigned"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        mealAssigned = new MealAssignedTO();
                        mealAssigned.RecID = Int32.Parse(row["rec_id"].ToString().Trim());
                        if (row["employee_id"] != DBNull.Value)
                        {
                            mealAssigned.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["meal_type_id"] != DBNull.Value)
                        {
                            mealAssigned.MealTypeID = Int32.Parse(row["meal_type_id"].ToString().Trim());
                        }
                        if (row["qty"] != DBNull.Value)
                        {
                            mealAssigned.Quantity = int.Parse(row["qty"].ToString().Trim());
                        }
                        if (row["qty_daily"] != DBNull.Value)
                        {
                            mealAssigned.QuantityDaily = int.Parse(row["qty_daily"].ToString().Trim());
                        }
                        if (row["money_amt"] != DBNull.Value)
                        {
                            mealAssigned.MoneyAmount = int.Parse(row["money_amt"].ToString().Trim());
                        }
                        if (row["valid_from"] != DBNull.Value)
                        {
                            mealAssigned.ValidFrom = DateTime.Parse(row["valid_from"].ToString().Trim());
                        }
                        if (row["valid_to"] != DBNull.Value)
                        {
                            mealAssigned.ValidTo = DateTime.Parse(row["valid_to"].ToString().Trim());
                        }
                        if (row["mt_name"] != DBNull.Value)
                        {
                            mealAssigned.MealTypeName = row["mt_name"].ToString().Trim();
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            mealAssigned.EmployeeFirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            mealAssigned.EmployeeLastName = row["last_name"].ToString().Trim();
                        }
                        mealsAssignedList.Add(mealAssigned);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return mealsAssignedList;
        }

        public int getMealsAssignedCount(int employeeID, int mealTypeID, DateTime validFrom, DateTime validTo,
            int qtyFrom, int qtyTo, int qtyDailyFrom, int qtyDailyTo, int mAmtFrom, int mAmtTo, bool unlimited)
        {
            DataSet dataSet = new DataSet();
            int count = 0;
            string selectQ = "";
            string selectM = "";
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ma.*, mt.name mt_name, e.first_name, e.last_name ");
                sb.Append("FROM meals_assigned ma, meals_type mt, employees e ");
                sb.Append("WHERE ma.qty <> '0' AND mt.meal_type_id = ma.meal_type_id AND ma.employee_id = e.employee_id AND ");

                if ((employeeID != -1) || (mealTypeID != -1) || (!validFrom.Equals(new DateTime())) ||
                    (!validTo.Equals(new DateTime())) || (qtyFrom != -1) || (qtyTo != -1) ||
                    (qtyDailyFrom != -1) || (qtyDailyTo != -1))
                {
                    if (employeeID != -1)
                    {
                        sb.Append("ma.employee_id = '" + employeeID.ToString() + "' AND ");
                    }
                    if (mealTypeID != -1)
                    {
                        sb.Append("ma.meal_type_id = '" + mealTypeID.ToString() + "' AND ");
                    }
                    if (!validFrom.Equals(new DateTime()) && !validTo.Equals(new DateTime()))
                    {
                        sb.Append("((valid_from < CONVERT('" + validFrom.AddDays(1).ToString("yyyy-MM-dd") + "', datetime) AND ");
                        sb.Append("valid_to >= CONVERT('" + validFrom.ToString("yyyy-MM-dd") + "', datetime)) OR ");
                        sb.Append("(valid_from < CONVERT('" + validTo.AddDays(1).ToString("yyyy-MM-dd") + "', datetime) AND ");
                        sb.Append("valid_to >= CONVERT('" + validTo.ToString("yyyy-MM-dd") + "', datetime)) OR ");
                        sb.Append("(valid_from >= CONVERT('" + validFrom.ToString("yyyy-MM-dd") + "', datetime) AND ");
                        sb.Append("valid_to < CONVERT('" + validTo.AddDays(1).ToString("yyyy-MM-dd") + "', datetime))) AND ");

                        if (!unlimited)
                        {
                            sb.Append("valid_from <> '1900-01-01 00:00:00.000' AND ");
                            sb.Append("valid_to <> '2099-01-01 00:00:00.000' AND ");
                        }
                    }
                    if (qtyFrom != -1)
                    {
                        sb.Append("qty >= '" + qtyFrom.ToString() + "' AND ");
                    }
                    if (qtyTo != -1)
                    {
                        sb.Append("qty <= '" + qtyTo.ToString() + "' AND ");
                    }
                    if (qtyDailyFrom != -1)
                    {
                        sb.Append("qty_daily >= '" + qtyDailyFrom.ToString() + "' AND ");
                    }
                    if (qtyDailyTo != -1)
                    {
                        sb.Append("qty_daily <= '" + qtyDailyTo.ToString() + "' AND ");
                    }
                }

                selectQ = sb.ToString().Substring(0, sb.ToString().Length - 4);

                sb = new StringBuilder();
                sb.Append("SELECT ma.*, mt.name mt_name, e.first_name, e.last_name ");
                sb.Append("FROM meals_assigned ma, meals_type mt, employees e ");
                sb.Append("WHERE ma.money_amt <> '0' AND mt.meal_type_id = ma.meal_type_id AND ma.employee_id = e.employee_id AND ");

                if ((employeeID != -1) || (mealTypeID != -1) || (!validFrom.Equals(new DateTime())) ||
                    (!validTo.Equals(new DateTime())) || (mAmtFrom != -1) || (mAmtTo != -1))
                {
                    if (employeeID != -1)
                    {
                        sb.Append("ma.employee_id = '" + employeeID.ToString() + "' AND ");
                    }
                    if (mealTypeID != -1)
                    {
                        sb.Append("ma.meal_type_id = '" + mealTypeID.ToString() + "' AND ");
                    }
                    if (!validFrom.Equals(new DateTime()) && !validTo.Equals(new DateTime()))
                    {
                        sb.Append("((valid_from < CONVERT('" + validFrom.AddDays(1).ToString("yyyy-MM-dd") + "', datetime) AND ");
                        sb.Append("valid_to >= CONVERT('" + validFrom.ToString("yyyy-MM-dd") + "', datetime)) OR ");
                        sb.Append("(valid_from < CONVERT('" + validTo.AddDays(1).ToString("yyyy-MM-dd") + "', datetime) AND ");
                        sb.Append("valid_to >= CONVERT('" + validTo.ToString("yyyy-MM-dd") + "', datetime)) OR ");
                        sb.Append("(valid_from >= CONVERT('" + validFrom.ToString("yyyy-MM-dd") + "', datetime) AND ");
                        sb.Append("valid_to < CONVERT('" + validTo.AddDays(1).ToString("yyyy-MM-dd") + "', datetime))) AND ");

                        if (!unlimited)
                        {
                            sb.Append("valid_from <> '1900-01-01 00:00:00.000' AND ");
                            sb.Append("valid_to <> '2099-01-01 00:00:00.000' AND ");
                        }
                    }
                    if (mAmtFrom != -1)
                    {
                        sb.Append("money_amt >= '" + mAmtFrom.ToString() + "' AND ");
                    }
                    if (mAmtTo != -1)
                    {
                        sb.Append("money_amt <= '" + mAmtTo.ToString() + "' AND ");
                    }
                }

                selectM = sb.ToString().Substring(0, sb.ToString().Length - 4);

                select = "SELECT COUNT(*) count_mealsAssigned FROM ((" + selectQ + ") UNION (" + selectM + ")) AS qry";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealAssigned");
                DataTable table = dataSet.Tables["MealAssigned"];

                if (table.Rows.Count > 0)
                {
                    count = Int32.Parse(table.Rows[0]["count_mealsAssigned"].ToString().Trim());
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
    }
}
