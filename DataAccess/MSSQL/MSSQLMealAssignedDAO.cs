using System;
using System.Collections;
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
    public class MSSQLMealAssignedDAO:MealAssignedDAO
    {

        SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

        public MSSQLMealAssignedDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MSSQLMealAssignedDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public bool insert(int employeeID, int mealTypeID, DateTime validFrom, DateTime validTo, int quantity, int quantityDaily, int moneyAmount)
        {
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
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

                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

                DataSet dataSet = new DataSet();
                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);

                rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    inserted = true;
                }
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

            return inserted;
        }
        public bool delete(string recID)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM meals_assigned WHERE rec_id = '" + recID + "'");

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
                        sb.Append(" ((valid_from <= convert(datetime, '" + validFrom.ToString(dateTimeformat) + "') AND");
                        sb.Append("  valid_to >= convert(datetime, '" + validFrom.ToString(dateTimeformat) + "')) OR");
                    }
                    if (!validTo.Equals(new DateTime()))
                    {
                        sb.Append(" (valid_from <= convert(datetime, '" + validTo.ToString(dateTimeformat) + "') AND");
                        sb.Append("  valid_to >= convert(datetime, '" + validTo.ToString(dateTimeformat) + "'))) AND");
                    }
                    sb.Append("  valid_from <> '1900-01-01 00:00:00.000' AND");
                    sb.Append(" valid_to <> '2099-01-01 00:00:00.000' ");
                }
                select = sb.ToString();

                select = select + " ORDER BY employee_id";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

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

        public int getMealsAssignedCount(string employeeID, string mealTypeID, DateTime validFrom, DateTime validTo)
        {
            DataSet dataSet = new DataSet();
            int count = 0;
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT count(ma.rec_id) count_mealsAssigned FROM meals_assigned ma ");
                if ((!employeeID.Equals("")) || (!mealTypeID.Equals("")) || (!validFrom.Equals(new DateTime())) ||
                    (!validTo.Equals(new DateTime())))
                {
                    sb.Append("WHERE ");
                    if (!employeeID.Equals(""))
                    {
                        sb.Append("employee_id IN (" + employeeID + ") AND ");
                    }
                    if (!mealTypeID.Equals(""))
                    {
                        sb.Append("meal_type_id IN (" + mealTypeID + ") AND ");
                    }
                    if (!validFrom.Equals(new DateTime()))
                    {
                        sb.Append("((valid_from <= convert(datetime, '" + validFrom.ToString(dateTimeformat) + "') AND ");
                        sb.Append("valid_to >= convert(datetime, '" + validFrom.ToString(dateTimeformat) + "')) OR ");
                    }
                    if (!validTo.Equals(new DateTime()))
                    {
                        sb.Append("(valid_from <= convert(datetime, '" + validTo.ToString(dateTimeformat) + "') AND ");
                        sb.Append("valid_to >= convert(datetime, '" + validTo.ToString(dateTimeformat) + "'))) AND ");
                    }
                    sb.Append("valid_from <> '1900-01-01 00:00:00.000' AND ");
                    sb.Append("valid_to <> '2099-01-01 00:00:00.000' ");
                }
                select = sb.ToString();

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

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
                        sb.Append("((valid_from < CONVERT(datetime, '" + validFrom.AddDays(1).ToString(dateTimeformat) + "') AND ");
                        sb.Append("valid_to >= CONVERT(datetime, '" + validFrom.ToString(dateTimeformat) + "')) OR ");
                        sb.Append("(valid_from < CONVERT(datetime, '" + validTo.AddDays(1).ToString(dateTimeformat) + "') AND ");
                        sb.Append("valid_to >= CONVERT(datetime, '" + validTo.ToString(dateTimeformat) + "')) OR ");
                        sb.Append("(valid_from >= CONVERT(datetime, '" + validFrom.ToString(dateTimeformat) + "') AND ");
                        sb.Append("valid_to < CONVERT(datetime, '" + validTo.AddDays(1).ToString(dateTimeformat) + "'))) AND ");

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
                        sb.Append("((valid_from < CONVERT(datetime, '" + validFrom.AddDays(1).ToString(dateTimeformat) + "') AND ");
                        sb.Append("valid_to >= CONVERT(datetime, '" + validFrom.ToString(dateTimeformat) + "')) OR ");
                        sb.Append("(valid_from < CONVERT(datetime, '" + validTo.AddDays(1).ToString(dateTimeformat) + "') AND ");
                        sb.Append("valid_to >= CONVERT(datetime, '" + validTo.ToString(dateTimeformat) + "')) OR ");
                        sb.Append("(valid_from >= CONVERT(datetime, '" + validFrom.ToString(dateTimeformat) + "') AND ");
                        sb.Append("valid_to < CONVERT(datetime, '" + validTo.AddDays(1).ToString(dateTimeformat) + "'))) AND ");

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

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

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

                sb.Append("valid_from <= convert(datetime, '" + DateTime.Now.Date.ToString(dateTimeformat) + "') AND ");
                sb.Append("valid_to >= convert(datetime, '" + DateTime.Now.Date.ToString(dateTimeformat) + "')");

                select = sb.ToString();

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

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
                        sb.Append("((valid_from < CONVERT(datetime, '" + validFrom.AddDays(1).ToString(dateTimeformat) + "') AND ");
                        sb.Append("valid_to >= CONVERT(datetime, '" + validFrom.ToString(dateTimeformat) + "')) OR ");
                        sb.Append("(valid_from < CONVERT(datetime, '" + validTo.AddDays(1).ToString(dateTimeformat) + "') AND ");
                        sb.Append("valid_to >= CONVERT(datetime, '" + validTo.ToString(dateTimeformat) + "')) OR ");
                        sb.Append("(valid_from >= CONVERT(datetime, '" + validFrom.ToString(dateTimeformat) + "') AND ");
                        sb.Append("valid_to < CONVERT(datetime, '" + validTo.AddDays(1).ToString(dateTimeformat) + "'))) AND ");

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
                        sb.Append("((valid_from < CONVERT(datetime, '" + validFrom.AddDays(1).ToString(dateTimeformat) + "') AND ");
                        sb.Append("valid_to >= CONVERT(datetime, '" + validFrom.ToString(dateTimeformat) + "')) OR ");
                        sb.Append("(valid_from < CONVERT(datetime, '" + validTo.AddDays(1).ToString(dateTimeformat) + "') AND ");
                        sb.Append("valid_to >= CONVERT(datetime, '" + validTo.ToString(dateTimeformat) + "')) OR ");
                        sb.Append("(valid_from >= CONVERT(datetime, '" + validFrom.ToString(dateTimeformat) + "') AND ");
                        sb.Append("valid_to < CONVERT(datetime, '" + validTo.AddDays(1).ToString(dateTimeformat) + "'))) AND ");

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

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

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
