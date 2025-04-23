using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using Util;
using TransferObjects;
using System.Collections;

namespace DataAccess
{
   public class MSSQLMealsTypeScheduleDAO:MealsTypeScheduleDAO
    {
          SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;
       protected string dateTimeformat = "";

		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

       public MSSQLMealsTypeScheduleDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
       public MSSQLMealsTypeScheduleDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public int insert(int mealTypeID, DateTime date, DateTime hoursFrom, DateTime hoursTo)
        {
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            int rowsAffected = 0;

            DateTime HoursFrom = new DateTime(1900, 1, 1, hoursFrom.Hour, hoursFrom.Minute, 0, 0);
            DateTime HoursTo = new DateTime(1900, 1, 1, hoursTo.Hour, hoursTo.Minute, 0, 0);

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO meals_type_shedule ");
                sbInsert.Append("(meal_type_id, date, hrs_from, hrs_to, created_by, created_time) ");
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
                if (hoursFrom.Equals(new DateTime(0)))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + HoursFrom.ToString(dateTimeformat).Trim() + "', ");
                }
                if (hoursTo.Equals(new DateTime(0)))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + HoursTo.ToString(dateTimeformat).Trim() + "', ");
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

       public int insert(int mealTypeID, DateTime date, DateTime hoursFrom, DateTime hoursTo, bool doCommit)
       {
           SqlTransaction sqlTrans = null;
           if (doCommit)
           {
               sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
           }
           else
           {
               sqlTrans = this.SqlTrans;
           }
           int rowsAffected = 0;

           DateTime HoursFrom = new DateTime(1900, 1, 1, hoursFrom.Hour, hoursFrom.Minute, 0, 0);
           DateTime HoursTo = new DateTime(1900, 1, 1, hoursTo.Hour, hoursTo.Minute, 0, 0);

           try
           {
               StringBuilder sbInsert = new StringBuilder();
               sbInsert.Append("INSERT INTO meals_type_schedule ");
               sbInsert.Append("(meal_type_id, date, hrs_from, hrs_to, created_by, created_time) ");
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
               if (hoursFrom.Equals(new DateTime(0)))
               {
                   sbInsert.Append("null, ");
               }
               else
               {
                   sbInsert.Append("'" + HoursFrom.ToString(dateTimeformat).Trim() + "', ");
               }
               if (hoursTo.Equals(new DateTime(0)))
               {
                   sbInsert.Append("null, ");
               }
               else
               {
                   sbInsert.Append("'" + HoursTo.ToString(dateTimeformat).Trim() + "', ");
               }
               sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

               DataSet dataSet = new DataSet();
               SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);

               rowsAffected = cmd.ExecuteNonQuery();
               if (doCommit)
               {
                   sqlTrans.Commit();
               }

           }
           catch (SqlException sqlEx)
           {
               if (doCommit)

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
               if (doCommit)
                   sqlTrans.Rollback("INSERT");
               else
                   sqlTrans.Rollback();
               throw ex;
           }

           return rowsAffected;
       }

        public bool delete(int mealTypeID, DateTime date)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM meals_type_schedule WHERE meal_type_id = '" + mealTypeID + "'");
                if (!date.Equals(new DateTime()))
                {
                    sbDelete.Append(" AND date = N'" + date.ToString(dateTimeformat) + "'");
                }

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

       public bool delete(int mealTypeID, DateTime date, bool doCommit)
       {
           bool isDeleted = false;
           SqlTransaction sqlTrans = null;
           if (doCommit)
           {
               sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
           }
           else
           {
               sqlTrans = this.SqlTrans;
           }
           try
           {
               StringBuilder sbDelete = new StringBuilder();
               sbDelete.Append("DELETE FROM meals_type_schedule WHERE meal_type_id = '" + mealTypeID + "'");
               if (!date.Equals(new DateTime()))
               {
                   sbDelete.Append(" AND date = N'" + date.ToString(dateTimeformat) + "'");
               }

               SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
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
                       sqlTrans.Rollback("DELETE");
                   }
               }
           }
           catch (Exception ex)
           {
               if (doCommit)
               {
                   sqlTrans.Rollback("DELETE");
               }
               throw new Exception(ex.Message);
           }

           return isDeleted;
       }

       public ArrayList getMealsTypeSchedule(int mealTypeID, DateTime dateFrom, DateTime dateTo)
        {
            DataSet dataSet = new DataSet();
            MealsTypeScheduleTO mealType = new MealsTypeScheduleTO();
            ArrayList mealsTypeList = new ArrayList();
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT mts.*, mt.name as type_name FROM meals_type_schedule mts, meals_type mt ");
                sb.Append("WHERE mts.meal_type_id = mt.meal_type_id ");
                if ((mealTypeID != -1) || ((!dateFrom.Equals(new DateTime())) &&
                    (!dateTo.Equals(new DateTime()))))
                {
                    sb.Append("AND ");
                    if (mealTypeID != -1)
                    {
                        sb.Append("mts.meal_type_id = '" + mealTypeID.ToString().Trim() + "' AND ");
                    }
                    if ((!dateFrom.Equals(new DateTime())) && (!dateTo.Equals(new DateTime())))
                    {
                        //sb.Append(" CONVERT(datetime,ps.event_time,101) >= '" + fromTime.ToString(dateTimeformat).Trim() + "' AND CONVERT(datetime,ps.event_time,101) <= '" + toTime.AddDays(1).ToString(dateTimeformat).Trim() + "' AND");
                        sb.Append(" mts.date >= CONVERT(datetime,'" + dateFrom.ToString("yyyy-MM-dd") + "', 111) AND ");
                        sb.Append(" mts.date < CONVERT(datetime,'" + dateTo.AddDays(1).ToString("yyyy-MM-dd") + "', 111) AND ");
                    }
                    
                    select = sb.ToString(0, sb.ToString().Length - 4);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY mts.meal_type_id";


               SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealType");
                DataTable table = dataSet.Tables["MealType"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        mealType = new MealsTypeScheduleTO();
                        mealType.MealTypeID = Int32.Parse(row["meal_type_id"].ToString().Trim());
                        if (row["date"] != DBNull.Value)
                        {
                            mealType.Date = DateTime.Parse(row["date"].ToString().Trim());
                        }
                        if (row["type_name"] != DBNull.Value)
                        {
                            mealType.MealType = row["type_name"].ToString().Trim();
                        }
                        if (row["hrs_from"] != DBNull.Value)
                        {
                            mealType.HrsFrom = DateTime.Parse(row["hrs_from"].ToString());
                        }
                        if (row["hrs_to"] != DBNull.Value)
                        {
                            mealType.HrsTo = DateTime.Parse(row["hrs_to"].ToString());
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
    
       public bool update(int mealTypeID,DateTime date, DateTime hoursFrom, DateTime hoursTo)
       {
           bool isUpdated = false;
           SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
           try
           {
               StringBuilder sbUpdate = new StringBuilder();
               sbUpdate.Append("UPDATE meals_type SET ");
               if (!date.Equals(new DateTime()))
               {
                   sbUpdate.Append("date = N'" + date.ToString(dateTimeformat) + "', ");
               }
               else
               {
                   sbUpdate.Append("date = NULL, ");
               }
               if (!hoursFrom.Equals(new DateTime()))
               {
                   sbUpdate.Append("hrs_from = '" + hoursFrom.ToString(dateTimeformat).Trim() + "', ");
               }
               else
               {
                   sbUpdate.Append("hrs_from = NULL, ");
               }
               if (!hoursTo.Equals(new DateTime()))
               {
                   sbUpdate.Append("hrs_to = '" + hoursTo.ToString(dateTimeformat).Trim() + "', ");
               }
               else
               {
                   sbUpdate.Append("hrs_to = NULL, ");
               }
               sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().ToString().Trim() + "', modified_time = " + " GETDATE() ");
               sbUpdate.Append("WHERE meal_type_id = '" + mealTypeID.ToString().Trim() + "'");
               SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
               int res = cmd.ExecuteNonQuery();
               if (res > 0)
               {
                   isUpdated = true;
               }

               sqlTrans.Commit();

           }
           catch (SqlException sqlex)
           {
               sqlTrans.Rollback();

               if (sqlex.Number == 2627)
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
