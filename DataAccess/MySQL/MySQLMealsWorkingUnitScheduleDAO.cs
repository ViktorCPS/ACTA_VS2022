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
  public  class MySQLMealsWorkingUnitScheduleDAO:MealsWorkingUnitScheduleDAO
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

      public MySQLMealsWorkingUnitScheduleDAO()
		{
			conn = MySQLDAOFactory.getConnection();
			DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
      public MySQLMealsWorkingUnitScheduleDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
			DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public int insert(int mealTypeID,DateTime date, int WUID)
        {
            int rowsAffected = 0;
            MySqlTransaction sqltrans = null;
            sqltrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            
            try
            {

                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO meals_working_unit_shedule ");
                sbInsert.Append("(meal_type_id, date, working_unit_id, created_by, created_time) ");
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
                if (WUID == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append( WUID + ", ");
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

      public int insert(int mealTypeID, DateTime date, int WUID, bool doCommit)
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
              sbInsert.Append("INSERT INTO meals_working_unit_schedule ");
              sbInsert.Append("(meal_type_id, date, working_unit_id, created_by, created_time) ");
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
              if (WUID == -1)
              {
                  sbInsert.Append("null, ");
              }
              else
              {
                  sbInsert.Append(WUID + ", ");
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


      public  bool delete(int mealTypeID, DateTime date, int WUID)
        { 
            bool isDeleted = false;
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
			
			try
			{
				StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM meals_working_unit_schedule WHERE ");
                if (mealTypeID != -1)
                {
                    sbDelete.Append("meal_type_id = '" + mealTypeID + "' AND");
                }
                if (!date.Equals(new DateTime()))
                {
                    sbDelete.Append("  date = N'" + date.ToString(dateTimeformat) + "' AND");
                }
                if (WUID != -1)
                {
                    sbDelete.Append(" working_unit_id = " + WUID + " AND");
                }
              

                MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(0, sbDelete.ToString().Length - 3), conn, sqlTrans);
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

      public bool delete(int mealTypeID, DateTime date, int WUID,bool doCommit)
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
              sbDelete.Append("DELETE FROM meals_working_unit_schedule WHERE ");
              if (mealTypeID != -1)
              {
                  sbDelete.Append("meal_type_id = '" + mealTypeID + "' AND");
              }
              if (!date.Equals(new DateTime()))
              {
                  sbDelete.Append("  date = N'" + date.ToString(dateTimeformat) + "' AND");
              }
              if (WUID != -1)
              {
                  sbDelete.Append(" working_unit_id = " + WUID + " AND");
              }

              MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(0, sbDelete.ToString().Length - 4), conn, sqlTrans);
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

      public ArrayList getMealsWUSchedule(int mealTypeID, DateTime dateFrom, DateTime dateTo, int WUID)
       {
           DataSet dataSet = new DataSet();
           MealsWorkingUnitScheduleTO mealType = new MealsWorkingUnitScheduleTO();
           ArrayList mealsTypeList = new ArrayList();
           string select;

           try
           {
               StringBuilder sb = new StringBuilder();
               sb.Append("SELECT mes.*, mt.name as type_name, wu.name as wu_name FROM meals_working_unit_schedule mes, meals_type mt, working_units wu ");
               sb.Append("WHERE mes.meal_type_id = mt.meal_type_id AND mes.working_unit_id = wu.working_unit_id");
               if ((mealTypeID != -1) || ((!dateFrom.Equals(new DateTime())) &&
                    (!dateTo.Equals(new DateTime()))) ||
                   (WUID != -1))
               {
                   sb.Append(" AND ");
                   if (mealTypeID != -1)
                   {
                       sb.Append("mes.meal_type_id = '" + mealTypeID.ToString().Trim() + "' AND ");
                   }
                   if ((!dateFrom.Equals(new DateTime())) &&
                    (!dateTo.Equals(new DateTime())))
                   {
                       //sb.Append("DATE_FORMAT(ps.event_time,GET_FORMAT(DATETIME,'ISO')) >= '" + fromTime.ToString(dateTimeformat).Trim() + "' AND DATE_FORMAT(ps.event_time,GET_FORMAT(DATETIME,'USA')) <= '" + toTime.AddDays(1).ToString(dateTimeformat).Trim() + "' AND ");
                       sb.Append("mes.date >= convert('" + dateFrom.ToString("yyyy-MM-dd") + "', datetime) AND ");
                       sb.Append("mes.date < convert('" + dateTo.AddDays(1).ToString("yyyy-MM-dd") + "', datetime) AND ");
                   }
                   if (WUID != -1)
                   {
                       sb.Append("mes.working_unit_id = " + WUID + " AND ");

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
                       mealType = new MealsWorkingUnitScheduleTO();
                       mealType.MealTypeID = Int32.Parse(row["meal_type_id"].ToString().Trim());
                       if (row["date"] != DBNull.Value)
                       {
                           mealType.Date = DateTime.Parse(row["date"].ToString().Trim());
                       }
                       if (row["type_name"] != DBNull.Value)
                       {
                           mealType.MealType = row["type_name"].ToString().Trim();
                       }
                       if (row["working_unit_id"] != DBNull.Value)
                       {
                           mealType.WorkingUnitID = int.Parse(row["working_unit_id"].ToString());
                       }
                       if (row["wu_name"] != DBNull.Value)
                       {
                           mealType.WorkingUnit = row["wu_name"].ToString();
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
    

       public ArrayList getMealsWUSchedule(int mealTypeID, DateTime date,  int WUID)
        {
            DataSet dataSet = new DataSet();
            MealsWorkingUnitScheduleTO mealType = new MealsWorkingUnitScheduleTO();
            ArrayList mealsTypeList = new ArrayList();
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT mes.*, mt.name as type_name, wu.name as wu_name FROM meals_working_unit_schedule mes, meals_type mt, working_units wu ");
                sb.Append("WHERE mes.meal_type_id = mt.meal_type_id AND mes.working_unit_id = wu.working_unit_id");
                if ((mealTypeID != -1) || (!date.Equals(new DateTime())) || 
                    (WUID != -1))
                {
                    sb.Append(" AND ");
                    if (mealTypeID != -1)
                    {
                        sb.Append("mes.meal_type_id = '" + mealTypeID.ToString().Trim() + "' AND ");
                    }
                    if (!date.Equals(new DateTime()))
                    {
                        sb.Append("mes.date = N'" + date.ToString(dateTimeformat).Trim() + "' AND ");
                    }                    
                    if (WUID != -1)
                    {
                        sb.Append("mes.working_unit_id = " + WUID + " AND ");
                        
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
                        mealType = new MealsWorkingUnitScheduleTO ();
                        mealType.MealTypeID = Int32.Parse(row["meal_type_id"].ToString().Trim());
                        if (row["date"] != DBNull.Value)
                        {
                            mealType.Date = DateTime.Parse(row["date"].ToString().Trim());
                        }
                        if (row["type_name"] != DBNull.Value)
                        {
                            mealType.MealType = row["type_name"].ToString().Trim();
                        }
                        if (row["working_unit_id"] != DBNull.Value)
                        {
                            mealType.WorkingUnitID = int.Parse(row["working_unit_id"].ToString());
                        }
                        if (row["wu_name"] != DBNull.Value)
                        {
                            mealType.WorkingUnit= row["wu_name"].ToString();
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
    }
}
