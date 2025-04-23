using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using Util;
using TransferObjects;

namespace DataAccess
{
   public class MSSQLMealTypeDAO:MealTypeDAO
    {
        SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;
       protected string dateTimeformat = "";

		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

		public MSSQLMealTypeDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MSSQLMealTypeDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(int mealTypeID, string name, string description, DateTime hoursFrom, DateTime hoursTo)
        {
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO meals_type ");
                sbInsert.Append("(meal_type_id, name, description, hrs_from, hrs_to, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                if (mealTypeID!= -1)
                {
                    sbInsert.Append("'" + mealTypeID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (name.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + name.Trim() + "', ");
                }
                if (description.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + description.Trim() + "', ");
                }
                if (hoursFrom.Equals(new DateTime(0)))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + hoursFrom.ToString("HH:mm:ss").Trim() + "', ");
                }
                if (hoursTo.Equals(new DateTime(0)))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + hoursTo.ToString("HH:mm:ss").Trim() + "', ");
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

        public int insert(int mealTypeID, string name, string description, DateTime hoursFrom, DateTime hoursTo, bool doCommit)
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

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO meals_type ");
                sbInsert.Append("(meal_type_id, name, description, hrs_from, hrs_to, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                if (mealTypeID != -1)
                {
                    sbInsert.Append("'" + mealTypeID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (name.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + name.Trim() + "', ");
                }
                if (description.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + description.Trim() + "', ");
                }
                if (hoursFrom.Equals(new DateTime(0)))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + hoursFrom.ToString("HH:mm:ss").Trim() + "', ");
                }
                if (hoursTo.Equals(new DateTime(0)))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + hoursTo.ToString("HH:mm:ss").Trim() + "', ");
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
                else
                    sqlTrans.Rollback();

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

        public bool delete(int mealTypeID)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM meals_type WHERE meal_type_id = '" + mealTypeID + "'");

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

        public bool delete(int mealTypeID, bool doCommit)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = null;

            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM meals_type WHERE meal_type_id = '" + mealTypeID + "'");

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    isDeleted = true;
                }
                if (doCommit)
                {
                    sqlTrans.Commit();
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                    sqlTrans.Rollback("DELETE");
                else
                    sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

       public int getMaxID()
       {
           DataSet dataSet = new DataSet();
           int mealTypeID = 0;
           string select;

           try
           {
               StringBuilder sb = new StringBuilder();
               sb.Append("select Max(meal_type_id) as max_id from meals_type ");
               
               select = sb.ToString();
              
               SqlCommand cmd = new SqlCommand(select, conn);
               SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

               sqlDataAdapter.Fill(dataSet, "MealType");
               DataTable table = dataSet.Tables["MealType"];

               if (table.Rows.Count > 0)
               {
                   DataRow row = table.Rows[0];
                  
                       if (row["max_id"] != DBNull.Value)
                       {
                           mealTypeID = int.Parse(row["max_id"].ToString().Trim());
                       }                   
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }

           return mealTypeID;
       }

       public ArrayList getMealsType(int mealTypeID, string name, string description, DateTime hoursFrom, DateTime hoursTo)
        {
            DataSet dataSet = new DataSet();
            MealTypeTO mealType = new MealTypeTO();
            ArrayList mealsTypeList = new ArrayList();
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM meals_type ");
                if ((mealTypeID != -1) || (!name.Equals("")) || (!description.Equals("")) ||
                    (!hoursFrom.Equals(new DateTime())) || (!hoursTo.Equals(new DateTime())))
                {
                    sb.Append("WHERE ");
                    if (mealTypeID != -1)
                    {
                        sb.Append("meal_type_id = " + mealTypeID.ToString().Trim() + " AND ");
                    }
                    if (!name.Equals(""))
                    {
                        sb.Append("name = N'" + name.ToString().Trim() + "' AND ");
                    }
                    if (!description.Equals(""))
                    {
                        sb.Append("description = N'" + description.ToString().Trim() + "' AND ");
                    }
                    if (!hoursFrom.Equals(new DateTime()) && !hoursTo.Equals(new DateTime()))
                    {
                        sb.Append("CONVERT(datetime, CONVERT(VARCHAR(25), hrs_from, 108), 108) >= CONVERT(datetime, '" + hoursFrom.ToString("HH:mm:ss") + "', 108) AND ");
                        sb.Append("CONVERT(datetime, CONVERT(VARCHAR(25), hrs_to, 108), 108) <= CONVERT(datetime, '" + hoursTo.ToString("HH:mm:ss") + "', 108) AND ");
                    }
                    
                    select = sb.ToString(0, sb.Length - 4);
                }
                else
                {
                    select = sb.ToString();
                }
               
                select = select + "ORDER BY meal_type_id";

               SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealType");
                DataTable table = dataSet.Tables["MealType"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        mealType = new MealTypeTO();
                        mealType.MealTypeID = Int32.Parse(row["meal_type_id"].ToString().Trim());
                        if (row["name"] != DBNull.Value)
                        {
                            mealType.Name = row["name"].ToString().Trim();
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            mealType.Description = row["description"].ToString().Trim();
                        }
                        if (row["hrs_from"] != DBNull.Value)
                        {
                            mealType.HoursFrom =DateTime.Parse(row["hrs_from"].ToString().Trim());
                        }
                        if (row["hrs_to"] != DBNull.Value)
                        {
                            mealType.HoursTo =DateTime.Parse(row["hrs_to"].ToString().Trim());
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

       public int getMealsTypeCount(int mealTypeID, string name, string description, DateTime hoursFrom, DateTime hoursTo)
        {
            DataSet dataSet = new DataSet();
            int count = 0;
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT count(mt.meal_type_id) count_mealTypes FROM meals_type mt ");
                if ((mealTypeID != -1) || (!name.Equals("")) || (!description.Equals("")) ||
                    (!hoursFrom.Equals(new DateTime())) || (!hoursTo.Equals(new DateTime())))
                {
                    sb.Append("WHERE");
                    if (mealTypeID != -1)
                    {
                        sb.Append(" meal_type_id = " + mealTypeID.ToString().Trim() + " AND");
                    }
                    if (!name.Equals(""))
                    {
                        sb.Append(" name = N'" + name.ToString().Trim() + "' AND");
                    }
                    if (!description.Equals(""))
                    {
                        sb.Append(" description = N'" + description.ToString().Trim() + "' AND");
                    }
                    if (!hoursFrom.Equals(new DateTime()))
                    {
                        sb.Append(" hrs_from >= '" + hoursFrom.ToString().Trim() + "' AND");
                    }
                    if (!hoursTo.Equals(new DateTime()))
                    {
                        sb.Append(" hrs_to <= '" + hoursTo.ToString().Trim() + "' AND");
                    }
                    select = sb.ToString(0, sb.Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }
               
               SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealType");
                DataTable table = dataSet.Tables["MealType"];

                if (table.Rows.Count > 0)
                {
                    count = Int32.Parse(table.Rows[0]["count_mealTypes"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return count;
        }

       public bool update(int mealTypeID, string name, string description, DateTime hoursFrom, DateTime hoursTo)
       {
           bool isUpdated = false;
           SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
           try
           {
               StringBuilder sbUpdate = new StringBuilder();
               sbUpdate.Append("UPDATE meals_type SET ");
               if (!name.Equals(""))
               {
                   sbUpdate.Append("name = N'" + name + "', ");
               }
               else
               {
                   sbUpdate.Append("name = NULL, ");
               }
               if (!description.Equals(""))
               {
                   sbUpdate.Append("description = N'" + description + "', ");
               }
               else
               {
                   sbUpdate.Append("description = NULL, ");
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
               sbUpdate.Append("modified_by = '" + DAOController.GetLogInUser().ToString().Trim() + "', modified_time = " + " GETDATE() ");
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
        public MealTypeTO getMealType(int mealTypeID)
        {
            DataSet dataSet = new DataSet();
            MealTypeTO mealType = new MealTypeTO();
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM meals_type ");

                sb.Append("WHERE ");
                if (mealTypeID != -1)
                {
                    sb.Append("meal_type_id = " + mealTypeID.ToString().Trim());
                }
                select = sb.ToString();
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealType");
                DataTable table = dataSet.Tables["MealType"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        mealType = new MealTypeTO();
                        mealType.MealTypeID = Int32.Parse(row["meal_type_id"].ToString().Trim());
                        if (row["name"] != DBNull.Value)
                        {
                            mealType.Name = row["name"].ToString().Trim();
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            mealType.Description = row["description"].ToString().Trim();
                        }
                        if (row["hrs_from"] != DBNull.Value)
                        {
                            mealType.HoursFrom = DateTime.Parse(row["hrs_from"].ToString().Trim());
                        }
                        if (row["hrs_to"] != DBNull.Value)
                        {
                            mealType.HoursTo = DateTime.Parse(row["hrs_to"].ToString().Trim());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return mealType;
        }


        #region MealTypeDAO Members


        public bool update(int mealTypeID, string name, string description, DateTime hoursFrom, DateTime hoursTo, bool doCommit)
        {
            SqlTransaction sqlTrans = null;

            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }

            bool isUpdated = false;
          //  SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE meals_type SET ");
                if (!name.Equals(""))
                {
                    sbUpdate.Append("name = N'" + name + "', ");
                }
                else
                {
                    sbUpdate.Append("name = NULL, ");
                }
                if (!description.Equals(""))
                {
                    sbUpdate.Append("description = N'" + description + "', ");
                }
                else
                {
                    sbUpdate.Append("description = NULL, ");
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
                sbUpdate.Append("modified_by = '" + DAOController.GetLogInUser().ToString().Trim() + "', modified_time = " + " GETDATE() ");
                sbUpdate.Append("WHERE meal_type_id = '" + mealTypeID.ToString().Trim() + "'");
                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

               // sqlTrans.Commit();
                if (doCommit)
                {
                    sqlTrans.Commit();
                }

            }
            catch (SqlException sqlex)
            {
               // sqlTrans.Rollback();

                if (doCommit)
                    sqlTrans.Rollback("UPDATE");
                else
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
               // sqlTrans.Rollback();
                if (doCommit)
                    sqlTrans.Rollback("UPDATE");
                else
                    sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        #endregion
    }
}
