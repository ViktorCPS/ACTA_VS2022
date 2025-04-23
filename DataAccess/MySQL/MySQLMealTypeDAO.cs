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
  public  class MySQLMealTypeDAO:MealTypeDAO
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

        public MySQLMealTypeDAO()
		{
			conn = MySQLDAOFactory.getConnection();
			DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MySQLMealTypeDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
			DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public int insert(int mealTypeID, string name, string description, DateTime hoursFrom, DateTime hoursTo)
        {
            int rowsAffected = 0;
            MySqlTransaction sqltrans = null;
            sqltrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            
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
                    sbInsert.Append("'" + hoursFrom.ToString(dateTimeformat).Trim() + "', ");
                }
                if (hoursTo.Equals(new DateTime(0)))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + hoursTo.ToString(dateTimeformat).Trim() + "', ");
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

        public int insert(int mealTypeID, string name, string description, DateTime hoursFrom, DateTime hoursTo, bool doCommit)
        {
            int rowsAffected = 0;
            MySqlTransaction sqlTrans = null;
            if (doCommit)
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            else
                sqlTrans = SqlTrans;

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
                    sbInsert.Append("'" + hoursFrom.ToString(dateTimeformat).Trim() + "', ");
                }
                if (hoursTo.Equals(new DateTime(0)))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + hoursTo.ToString(dateTimeformat).Trim() + "', ");
                }
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");
                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();
             //   rowsAffected = 1;

                if (doCommit)
                    sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    sqlTrans.Rollback();
                throw ex;
            }

            return rowsAffected;
        }

      public  bool delete(int mealTypeID)
        { 
            bool isDeleted = false;
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
			
			try
			{
				StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM meals_type WHERE meal_type_id = '" + mealTypeID + "'");
				
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


      public bool delete(int mealTypeID, bool doCommit)
      {
          bool isDeleted = false;
          MySqlTransaction sqlTrans = null;
          if (doCommit)
              sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
          else
              sqlTrans = SqlTrans;

          try
          {
              StringBuilder sbDelete = new StringBuilder();
              sbDelete.Append("DELETE FROM meals_type WHERE meal_type_id = '" + mealTypeID + "'");

              MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, sqlTrans);
              int res = cmd.ExecuteNonQuery();
              if (res != 0)
              {
                  isDeleted = true;
              }
              
              if (doCommit)
                  sqlTrans.Commit();
          }
          catch (Exception ex)
          {
              if (doCommit)
                  sqlTrans.Rollback();
              throw ex;
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

              MySqlCommand cmd = new MySqlCommand(select, conn);
              MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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
                if ((mealTypeID != -1) || (!name.Equals("")) || (!description.Equals(""))||
                    (!hoursFrom.Equals(new DateTime()))||(!hoursTo.Equals(new DateTime())))
                {
                    sb.Append("WHERE ");
                    if (mealTypeID != -1)
                    {
                        sb.Append("meal_type_id = '" + mealTypeID.ToString().Trim() + "' AND ");
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
                        sb.Append("DATE_FORMAT(hrs_from, '%T') >= DATE_FORMAT(CONVERT('" + hoursFrom.ToString(dateTimeformat) + "', datetime), '%T') AND ");
                        sb.Append("DATE_FORMAT(hrs_to, '%T') <= DATE_FORMAT(CONVERT('" + hoursTo.ToString(dateTimeformat) + "', datetime), '%T') AND ");
                    }
                    select = sb.ToString(0, sb.ToString().Length - 4);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY meal_type_id";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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
                            mealType.HoursFrom =DateTime.Parse(row["hrs_from"].ToString());
                        }
                        if (row["hrs_to"] != DBNull.Value)
                        {
                            mealType.HoursTo =DateTime.Parse(row["hrs_to"].ToString());
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
                        sb.Append(" meal_type_id = " + mealTypeID.ToString().Trim() + " AND ");
                    }
                    if (!name.Equals(""))
                    {
                        sb.Append(" name = N'" + name.ToString().Trim() + "' AND ");
                    }
                    if (!description.Equals(""))
                    {
                        sb.Append(" description = N'" + description.ToString().Trim() + "' AND ");
                    }
                    if (!hoursFrom.Equals(new DateTime()) && !hoursTo.Equals(new DateTime()))
                    {
                        sb.Append(" hrs_from >= CONVERT('" + hoursFrom.ToString("HH:mm") + "', datetime)' AND ");
                        sb.Append(" hrs_to < CONVERT('" + hoursTo.AddMinutes(1).ToString("HH:mm") + "', datetime) AND ");
                    }
                    
                    select = sb.ToString(0, sb.ToString().Length - 4);
                }
                else
                {
                    select = sb.ToString();
                }
                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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
          MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
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
              sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().ToString().Trim() + "', modified_time = " + " NOW() ");
              sbUpdate.Append("WHERE meal_type_id = '" + mealTypeID.ToString().Trim() + "'");
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



        #region MealTypeDAO Members


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
                    sb.Append("meal_type_id = '" + mealTypeID.ToString().Trim() + "' AND ");
                }
                select = sb.ToString();
                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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
                            mealType.HoursFrom = DateTime.Parse(row["hrs_from"].ToString());
                        }
                        if (row["hrs_to"] != DBNull.Value)
                        {
                            mealType.HoursTo = DateTime.Parse(row["hrs_to"].ToString());
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

        #endregion

        #region MealTypeDAO Members


        public bool update(int mealTypeID, string name, string description, DateTime hoursFrom, DateTime hoursTo, bool doCommit)
        {
            MySqlTransaction sqlTrans = null;
            if (doCommit)
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            else
                sqlTrans = SqlTrans;

            bool isUpdated = false;
          //  MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
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
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().ToString().Trim() + "', modified_time = " + " NOW() ");
                sbUpdate.Append("WHERE meal_type_id = '" + mealTypeID.ToString().Trim() + "'");
                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

              //  sqlTrans.Commit();
                if (doCommit)
                    sqlTrans.Commit();

            }
            catch (MySqlException sqlex)
            {
                if (doCommit)
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
                if (doCommit)
                    sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        #endregion
    }
}
