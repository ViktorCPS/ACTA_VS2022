using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using TransferObjects;
using Util;

namespace DataAccess
{
   public class MySQLOnlineMealsRestaurantDAO:OnlineMealsRestaurantDAO
    {
  
      MySqlConnection conn = null;
		//protected string dateTimeformat = "";
		MySqlTransaction _sqlTrans = null;

		public MySqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

        public MySQLOnlineMealsRestaurantDAO()
		{
			conn = MySQLDAOFactory.getConnection();
			DAOController.GetInstance();
		}
        public MySQLOnlineMealsRestaurantDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
			DAOController.GetInstance();
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


        public List<OnlineMealsRestaurantTO> getRestaurants(OnlineMealsRestaurantTO onlineRestaurant)
        {
            DataSet dataSet = new DataSet();
            OnlineMealsRestaurantTO meal = new OnlineMealsRestaurantTO();
            List<OnlineMealsRestaurantTO> mealsList = new List<OnlineMealsRestaurantTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM online_meals_restaurants");

                if ((onlineRestaurant.RestaurantID != -1) || (!onlineRestaurant.Name.Trim().Equals("")) ||
                    (!onlineRestaurant.Description.Trim().Equals("")))
                {
                    sb.Append(" WHERE");

                    if (onlineRestaurant.RestaurantID != -1)
                    {
                        //sb.Append(" UPPER(employee_id) LIKE '" + EmployeeID.Trim().ToUpper() + "' AND");
                        sb.Append(" restaurant_id = '" + onlineRestaurant.RestaurantID.ToString().Trim() + "' AND");
                    }
                    if (!onlineRestaurant.Name.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(name) LIKE N'%" + onlineRestaurant.Name.Trim().ToUpper() + "%' AND");
                    }
                    if (!onlineRestaurant.Description.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(description) LIKE N'%" + onlineRestaurant.Description.Trim().ToUpper() + "%' AND");
                    }
                   
                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select += " ORDER BY restaurant_id ";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "OnlineRestaurant");
                DataTable table = dataSet.Tables["OnlineRestaurant"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        meal = new OnlineMealsRestaurantTO();
                        meal.RestaurantID = int.Parse(row["restaurant_id"].ToString().Trim());

                       
                        if (row["name"] != DBNull.Value)
                        {
                            meal.Name = row["name"].ToString().Trim();
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            meal.Description = row["description"].ToString().Trim();
                        }
                       
                        mealsList.Add(meal);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return mealsList;
        }
        public int insert(string name, string description)
        {
            int rowsAffected = 0;
            MySqlTransaction sqltrans = null;
            sqltrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {

                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO online_meals_restaurants ");
                sbInsert.Append("(name, description,created_by, created_time) ");
                sbInsert.Append("VALUES (");

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

        public bool update(int restaurantID, string name, string description)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE online_meals_restaurants SET ");
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

                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().ToString().Trim() + "', modified_time = " + " NOW() ");
                sbUpdate.Append("WHERE restaurant_id = '" + restaurantID.ToString().Trim() + "'");
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


        public bool delete(int restaurantID)
        {
            bool isDeleted = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM online_meals_restaurants WHERE restaurant_id = '" + restaurantID + "'");

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
    }
}
