using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using MySql.Data.MySqlClient;
using System.Globalization;
using TransferObjects;
using Util;
using System.Collections;

namespace DataAccess
{
   public class MySQLMealsEmployeeScheduleDAO:MealsEmployeeScheduleDAO
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

       public MySQLMealsEmployeeScheduleDAO()
		{
			conn = MySQLDAOFactory.getConnection();
			DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
       public MySQLMealsEmployeeScheduleDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
			DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public int insert(int mealTypeID,DateTime date, int employeeID, string shift)
        {
            int rowsAffected = 0;
            MySqlTransaction sqltrans = null;
            sqltrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            
            try
            {

                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO meals_employee_schedule ");
                sbInsert.Append("(meal_type_id, date, employee_id, shift, created_by, created_time) ");
                sbInsert.Append("VALUES (");
               if (mealTypeID != -1)
                {
                    sbInsert.Append("'" + mealTypeID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (date.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + date.ToString(dateTimeformat) + "', ");
                }                
                if (employeeID == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append( employeeID + ", ");
                }
                if (shift.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + shift + ", ");
                }
              
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");
                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqltrans);
                rowsAffected = cmd.ExecuteNonQuery();
                sqltrans.Commit();
               
                rowsAffected = 1;
            }
            catch (Exception ex)
            {             
                sqltrans.Rollback();               
                throw ex;
            }

            return rowsAffected;
        }

       public int insert(int mealTypeID, DateTime date, int employeeID, string shift, bool doCommit)
       {
           int rowsAffected = 0;
           MySqlTransaction sqlTrans = null;
           if (doCommit)
           {
               sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
           }
           else
           {
               sqlTrans = this.SqlTrans;
           }

           try
           {

               StringBuilder sbInsert = new StringBuilder();
               sbInsert.Append("INSERT INTO meals_employee_schedule ");
               sbInsert.Append("(meal_type_id, date, employee_id, created_by, created_time) ");
               sbInsert.Append("VALUES (");
               if (mealTypeID != -1)
               {
                   sbInsert.Append("'" + mealTypeID.ToString().Trim() + "', ");
               }
               else
               {
                   sbInsert.Append("NULL, ");
               }
               if (date.Equals(new DateTime()))
               {
                   sbInsert.Append("null, ");
               }
               else
               {
                   sbInsert.Append("N'" + date.ToString(dateTimeformat) + "', ");
               }
               if (employeeID == -1)
               {
                   sbInsert.Append("null, ");
               }
               else
               {
                   sbInsert.Append(employeeID + ", ");
               }
               if (shift.Equals(""))
               {
                   sbInsert.Append("null, ");
               }
               else
               {
                   sbInsert.Append("N'"+shift + "', ");
               }

               sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");
               MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);
               rowsAffected = cmd.ExecuteNonQuery();
               if (doCommit)
               {
                   sqlTrans.Commit();
               }

               rowsAffected = 1;
           }
           catch (Exception ex)
           {
               if (doCommit)
                   sqlTrans.Rollback();
               throw ex;
           }

           return rowsAffected;
       }

      public  bool delete(int mealTypeID, DateTime date, int employeeID)
        { 
            bool isDeleted = false;
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
			
			try
			{
				StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM meals_employee_schedule WHERE meal_type_id = '" + mealTypeID + "'");
                if (!date.Equals(new DateTime()))
                {
                    sbDelete.Append(" AND date = N'" + date.ToString(dateTimeformat) + "'");
                }
                if (employeeID != -1)
                {
                    sbDelete.Append(" AND employee_id = " + employeeID );
                }
				
				MySqlCommand cmd = new MySqlCommand( sbDelete.ToString(), conn, sqlTrans);
				int res = cmd.ExecuteNonQuery();
				if (res != 0)
				{
					isDeleted = true;
				}
				sqlTrans.Commit();				
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback();
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

       public bool delete(int mealTypeID, DateTime date, int employeeID, bool doCommit)
       {
           bool isDeleted = false;
           MySqlTransaction sqlTrans = null;
           if (doCommit)
           {
               sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
           }
           else
           {
               sqlTrans = this.SqlTrans;
           }

           try
           {
               StringBuilder sbDelete = new StringBuilder();
               sbDelete.Append("DELETE FROM meals_employee_schedule WHERE ");
               if (mealTypeID != -1)
               {
                   sbDelete.Append("meal_type_id = '" + mealTypeID + "' AND");
               }
               if (!date.Equals(new DateTime()))
               {
                   sbDelete.Append(" date = N'" + date.ToString(dateTimeformat) + "' AND");
               }
               if (employeeID != -1)
               {
                   sbDelete.Append(" employee_id = " + employeeID+" AND");
               }

               MySqlCommand cmd = new MySqlCommand(sbDelete.ToString().Substring(0, sbDelete.ToString().Length - 4), conn, sqlTrans);
               int res = cmd.ExecuteNonQuery();
               if (res != 0)
               {
                   isDeleted = true;
               }
               if (doCommit)
               {
                   if (isDeleted)
                   {
                       sqlTrans.Commit();
                   }
                   else
                   {
                       sqlTrans.Rollback();
                   }
               }
           }
           catch (Exception ex)
           {
               if (doCommit)
               {
                   sqlTrans.Rollback();
               }
               throw new Exception(ex.Message);
           }

           return isDeleted;
       }

       public ArrayList getMealsEmployeeSchedule(DateTime from, DateTime to, int WUID)
       {
           DataSet dataSet = new DataSet();
           MealsEmployeeSchaduleTO mealType = new MealsEmployeeSchaduleTO();
           ArrayList mealsTypeList = new ArrayList();
           string select;

           try
           {
               StringBuilder sb = new StringBuilder();
               sb.Append("SELECT mes.*, mt.name as type_name, e.first_name, e.last_name FROM meals_employee_schedule mes, meals_type mt, employees e ");
               sb.Append("WHERE mes.meal_type_id = mt.meal_type_id AND mes.employee_id = e.employee_id");
               if ((!from.Equals(new DateTime()) && !to.Equals(new DateTime())) ||
                   (WUID != -1))
               {
                   sb.Append(" AND ");
                   if (!from.Equals(new DateTime()) && !to.Equals(new DateTime()))
                   {
                        //sb.Append("DATE_FORMAT(ps.event_time,GET_FORMAT(DATETIME,'ISO')) >= '" + fromTime.ToString(dateTimeformat).Trim() + "' AND DATE_FORMAT(ps.event_time,GET_FORMAT(DATETIME,'USA')) <= '" + toTime.AddDays(1).ToString(dateTimeformat).Trim() + "' AND ");
                       sb.Append("mes.date >= convert('" + from.ToString("yyyy-MM-dd") + "', datetime) AND ");
                       sb.Append("mes.date < convert('" + to.ToString("yyyy-MM-dd") + "', datetime) AND ");
                    
                   }
                   if (WUID != -1)
                   {
                       sb.Append("e.working_unit_id = " + WUID+" AND ");

                   }
                   select = sb.ToString(0, sb.ToString().Length - 4);
               }
               else
               {
                   select = sb.ToString();
               }

               select = select + " ORDER BY mes.meal_type_id";

               MySqlCommand cmd = new MySqlCommand(select, conn);
               MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

               sqlDataAdapter.Fill(dataSet, "MealType");
               DataTable table = dataSet.Tables["MealType"];

               if (table.Rows.Count > 0)
               {
                   foreach (DataRow row in table.Rows)
                   {
                       mealType = new MealsEmployeeSchaduleTO();
                       mealType.MealTypeID = Int32.Parse(row["meal_type_id"].ToString().Trim());
                       if (row["date"] != DBNull.Value)
                       {
                           mealType.Date = DateTime.Parse(row["date"].ToString().Trim());
                       }
                       if (row["type_name"] != DBNull.Value)
                       {
                           mealType.MealType = row["type_name"].ToString().Trim();
                       }
                       if (row["employee_id"] != DBNull.Value)
                       {
                           mealType.EmployeeID = int.Parse(row["employee_id"].ToString());
                       }
                       if (row["first_name"] != DBNull.Value)
                       {
                           mealType.EmployeeFirstName = row["first_name"].ToString();
                       }
                       if (row["last_name"] != DBNull.Value)
                       {
                           mealType.EmployeeLastName = row["last_name"].ToString();
                       }
                       if (row["shift"] != DBNull.Value)
                       {
                           mealType.Shift = row["shift"].ToString();
                       }
                       mealsTypeList.Add(mealType);
                   }
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }

           return mealsTypeList;
       }

       public ArrayList getMealsEmployeeScheduleForEmpl(DateTime from, DateTime to, int employeeID)
       {
           DataSet dataSet = new DataSet();
           MealsEmployeeSchaduleTO mealType = new MealsEmployeeSchaduleTO();
           ArrayList mealsTypeList = new ArrayList();
           string select;

           try
           {
               StringBuilder sb = new StringBuilder();
               sb.Append("SELECT mes.*, mt.name as type_name, e.first_name, e.last_name FROM meals_employee_schedule mes, meals_type mt, employees e ");
               sb.Append("WHERE mes.meal_type_id = mt.meal_type_id AND mes.employee_id = e.employee_id");
               if ((!from.Equals(new DateTime()) && !to.Equals(new DateTime())) ||
                   (employeeID != -1))
               {
                   sb.Append(" AND ");
                   if (!from.Equals(new DateTime()) && !to.Equals(new DateTime()))
                   {
                       //sb.Append("DATE_FORMAT(ps.event_time,GET_FORMAT(DATETIME,'ISO')) >= '" + fromTime.ToString(dateTimeformat).Trim() + "' AND DATE_FORMAT(ps.event_time,GET_FORMAT(DATETIME,'USA')) <= '" + toTime.AddDays(1).ToString(dateTimeformat).Trim() + "' AND ");
                       sb.Append("mes.date >= convert('" + from.ToString("yyyy-MM-dd") + "', datetime) AND ");
                       sb.Append("mes.date < convert('" + to.AddDays(1).ToString("yyyy-MM-dd") + "', datetime) AND ");

                   }
                   if (employeeID != -1)
                   {
                       sb.Append("e.employee_id = " + employeeID+" AND ");

                   }
                   select = sb.ToString(0, sb.ToString().Length - 4);
               }
               else
               {
                   select = sb.ToString();
               }

               select = select + " ORDER BY mes.meal_type_id";

               MySqlCommand cmd = new MySqlCommand(select, conn);
               MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

               sqlDataAdapter.Fill(dataSet, "MealType");
               DataTable table = dataSet.Tables["MealType"];

               if (table.Rows.Count > 0)
               {
                   foreach (DataRow row in table.Rows)
                   {
                       mealType = new MealsEmployeeSchaduleTO();
                       mealType.MealTypeID = Int32.Parse(row["meal_type_id"].ToString().Trim());
                       if (row["date"] != DBNull.Value)
                       {
                           mealType.Date = DateTime.Parse(row["date"].ToString().Trim());
                       }
                       if (row["type_name"] != DBNull.Value)
                       {
                           mealType.MealType = row["type_name"].ToString().Trim();
                       }
                       if (row["employee_id"] != DBNull.Value)
                       {
                           mealType.EmployeeID = int.Parse(row["employee_id"].ToString());
                       }
                       if (row["first_name"] != DBNull.Value)
                       {
                           mealType.EmployeeFirstName = row["first_name"].ToString();
                       }
                       if (row["last_name"] != DBNull.Value)
                       {
                           mealType.EmployeeLastName = row["last_name"].ToString();
                       }
                       if (row["shift"] != DBNull.Value)
                       {
                           mealType.Shift = row["shift"].ToString();
                       }
                       mealsTypeList.Add(mealType);
                   }
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }

           return mealsTypeList;
       }
       
       public ArrayList getMealsEmployeeSchedule(int mealTypeID, DateTime date,  int employeeID)
        {
            DataSet dataSet = new DataSet();
            MealsEmployeeSchaduleTO mealType = new MealsEmployeeSchaduleTO();
            ArrayList mealsTypeList = new ArrayList();
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT mes.*, mt.name as type_name, e.first_name, e.last_name FROM meals_employee_schedule mes, meals_type mt, employees e ");
                sb.Append("WHERE mes.meal_type_id = mt.meal_type_id AND mes.employee_id = e.employee_id");
                if ((mealTypeID != -1) || (!date.Equals(new DateTime())) || 
                    (employeeID != -1))
                {
                    sb.Append("AND ");
                    if (mealTypeID != -1)
                    {
                        sb.Append("mes.meal_type_id = '" + mealTypeID.ToString().Trim() + "' AND ");
                    }
                    if (!date.Equals(new DateTime()))
                    {
                        sb.Append("mes.date = N'" + date.ToString(dateTimeformat).Trim() + "' AND ");
                    }                    
                    if (employeeID != -1)
                    {
                        sb.Append("mes.employee_id = "+employeeID);
                        
                    }
                    select = sb.ToString(0, sb.ToString().Length - 4);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY mes.meal_type_id";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealType");
                DataTable table = dataSet.Tables["MealType"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        mealType = new MealsEmployeeSchaduleTO();
                        mealType.MealTypeID = Int32.Parse(row["meal_type_id"].ToString().Trim());
                        if (row["date"] != DBNull.Value)
                        {
                            mealType.Date = DateTime.Parse(row["date"].ToString().Trim());
                        }
                        if (row["type_name"] != DBNull.Value)
                        {
                            mealType.MealType = row["type_name"].ToString().Trim();
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            mealType.EmployeeID =int.Parse(row["employee_id"].ToString());
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            mealType.EmployeeFirstName = row["first_name"].ToString();
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            mealType.EmployeeLastName = row["last_name"].ToString();
                        }
                        if (row["shift"] != DBNull.Value)
                        {
                            mealType.Shift = row["shift"].ToString();
                        }
                        mealsTypeList.Add(mealType);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return mealsTypeList;
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


        public bool update(int mealTypeID, DateTime date, int employeeID, string shift)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE meals_employee_schedule SET ");
                if (!mealTypeID.Equals(""))
                {
                    sbUpdate.Append("meal_type_id = N'" + mealTypeID + "', ");
                }
                else
                {
                    sbUpdate.Append("meal_type_id = NULL, ");
                }
                if (!shift.Equals(""))
                {
                    sbUpdate.Append("shift = N'" + shift + "', ");
                }

                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().ToString().Trim() + "', modified_time = " + " NOW() ");
                sbUpdate.Append("WHERE employee_id = '" + employeeID.ToString().Trim() + "' AND date = N'" + date.ToString(dateTimeformat) + "'");
                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                sqlTrans.Commit();

            }
            catch (MySqlException sqlex)
            {
                sqlTrans.Rollback();

                if (sqlex.Number == 1062)
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message, 12);
                    throw procEx;
                }
                else
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message, 11);
                    throw procEx;
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }



        public ArrayList getScheduleForDateEmplMeal(DateTime from, DateTime to, string wuID, int emplID, int mealTypeID)
        {
            DataSet dataSet = new DataSet();
            MealsEmployeeSchaduleTO mealType = new MealsEmployeeSchaduleTO();
            ArrayList mealsTypeList = new ArrayList();
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT mes.*, mt.name as type_name, e.first_name, e.last_name, wu.name as working_unit_name FROM meals_employee_schedule mes, meals_type mt, employees e, working_units wu ");
                sb.Append("WHERE mes.meal_type_id = mt.meal_type_id AND mes.employee_id = e.employee_id AND e.working_unit_id = wu.working_unit_id ");
                if ((!from.Equals(new DateTime()) && !to.Equals(new DateTime())) || (emplID != -1) ||
                    (wuID != null && !wuID.Equals("") || (mealTypeID != -1)))
                {
                    sb.Append("AND ");

                    if (!from.Equals(new DateTime()) && !to.Equals(new DateTime()))
                    {
                        sb.Append(" mes.date >= CONVERT(datetime,'" + from.ToString("yyyy-MM-dd") + "', 111) AND ");
                        sb.Append(" mes.date < CONVERT(datetime,'" + to.ToString("yyyy-MM-dd") + "', 111) AND ");
                    }
                    if (emplID != -1)
                    {
                        sb.Append(" e.employee_id IN (" + emplID + ") AND");
                    }
                    if (wuID != null && !wuID.Equals(""))
                    {
                        sb.Append(" e.working_unit_id IN (" + wuID + ") AND");
                    }
                    if (mealTypeID != -1)
                    {
                        sb.Append(" mes.meal_type_id IN (" + mealTypeID + ") AND");
                    }
                    select = sb.ToString(0, sb.ToString().Length - 4);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + " ORDER BY mes.date";


                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealType");
                DataTable table = dataSet.Tables["MealType"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        mealType = new MealsEmployeeSchaduleTO();
                        mealType.MealTypeID = Int32.Parse(row["meal_type_id"].ToString().Trim());
                        if (row["date"] != DBNull.Value)
                        {
                            mealType.Date = DateTime.Parse(row["date"].ToString().Trim());
                        }
                        if (row["type_name"] != DBNull.Value)
                        {
                            mealType.MealType = row["type_name"].ToString().Trim();
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            mealType.EmployeeID = int.Parse(row["employee_id"].ToString());
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            mealType.EmployeeFirstName = row["first_name"].ToString();
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            mealType.EmployeeLastName = row["last_name"].ToString();
                        }
                        if (row["working_unit_name"] != DBNull.Value)
                        {
                            mealType.WorkingUnit = row["working_unit_name"].ToString();
                        }
                        if (row["shift"] != DBNull.Value)
                        {
                            mealType.WorkingUnit = row["shift"].ToString();
                        }
                        mealsTypeList.Add(mealType);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return mealsTypeList;
        }


        public Dictionary<DateTime, List<EmployeeTO>> getDateEndEmployeesWhoOrdered(DateTime from, DateTime to, string wuID)
        {
            DataSet dataSet = new DataSet();
            MealsEmployeeSchaduleTO mealType = new MealsEmployeeSchaduleTO();
            EmployeeTO empl = new EmployeeTO();
            Dictionary<DateTime, List<EmployeeTO>> ordersDic = new Dictionary<DateTime, List<EmployeeTO>>();
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT mes.*, e.employee_id, e.first_name, e.last_name, wu.name as working_unit_name FROM meals_employee_schedule mes, employees e, working_units wu ");
                sb.Append("WHERE mes.employee_id = e.employee_id AND e.working_unit_id = wu.working_unit_id ");
                if ((!from.Equals(new DateTime()) && !to.Equals(new DateTime())) || (wuID != null && !wuID.Equals("")))
                {
                    sb.Append("AND ");

                    if (!from.Equals(new DateTime()) && !to.Equals(new DateTime()))
                    {
                        sb.Append(" mes.date >= CONVERT(datetime,'" + from.ToString("yyyy-MM-dd") + "', 111) AND ");
                        sb.Append(" mes.date < CONVERT(datetime,'" + to.ToString("yyyy-MM-dd") + "', 111) AND ");
                    }
                    if (wuID != null && !wuID.Equals(""))
                    {
                        sb.Append(" e.working_unit_id IN (" + wuID + ") AND");
                    }
                    select = sb.ToString(0, sb.ToString().Length - 4);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + " ORDER BY mes.date";


                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealType");
                DataTable table = dataSet.Tables["MealType"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        empl = new EmployeeTO();
                        // mealType.MealTypeID = Int32.Parse(row["meal_type_id"].ToString().Trim());
                        if (row["date"] != DBNull.Value)
                        {
                            if (!ordersDic.ContainsKey(mealType.Date))
                            {
                                ordersDic.Add(DateTime.Parse(row["date"].ToString().Trim()), new List<EmployeeTO>());
                            }
                            //mealType.Date = DateTime.Parse(row["date"].ToString().Trim()) ;
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            empl.EmployeeID = int.Parse(row["employee_id"].ToString());
                            //mealType.EmployeeID = int.Parse(row["employee_id"].ToString());
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            empl.FirstName = row["first_name"].ToString();
                            //mealType.EmployeeFirstName = row["first_name"].ToString();
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            empl.LastName = row["last_name"].ToString();
                            //mealType.EmployeeLastName = row["last_name"].ToString();
                        }
                        if (row["working_unit_name"] != DBNull.Value)
                        {
                            empl.WorkingUnitName = row["working_unit_name"].ToString();
                            //mealType.WorkingUnit = row["working_unit_name"].ToString();
                        }

                        ordersDic[DateTime.Parse(row["date"].ToString().Trim())].Add(empl);


                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ordersDic;
        }


        #region MealsEmployeeScheduleDAO Members


        public ArrayList getNumberOfScheduledMeals(DateTime from, DateTime to, string wuString)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
