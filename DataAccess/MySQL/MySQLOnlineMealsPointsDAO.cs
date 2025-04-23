using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using TransferObjects;

namespace DataAccess
{
    public class MySQLOnlineMealsPointsDAO : OnlineMealsPointsDAO
    {
        MySqlConnection conn = null;
        //protected string dateTimeformat = "";
        MySqlTransaction _sqlTrans = null;

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MySQLOnlineMealsPointsDAO()
        {
            conn = MySQLDAOFactory.getConnection();
            DAOController.GetInstance();
        }
        public MySQLOnlineMealsPointsDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
            DAOController.GetInstance();
        }


        public List<OnlineMealsPointsTO> findByRestaurant(string restaurantID)
        {
            DataSet dataSet = new DataSet();
            List<OnlineMealsPointsTO> mealsList = new List<OnlineMealsPointsTO>();
            string select = "";

            try
            {
                OnlineMealsPointsTO meal = new OnlineMealsPointsTO();

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM online_meals_points ");


                if (!restaurantID.Equals(""))
                {
                    sb.Append("WHERE ");
                    sb.Append("restaurant_id = '" + restaurantID + "' AND ");
                }

                select = sb.ToString();

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealsPoints");
                DataTable table = dataSet.Tables["MealsPoints"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        meal = new OnlineMealsPointsTO();
                        meal.PointID = int.Parse(row["point_id"].ToString().Trim());

                        if (row["reader_ant"] != DBNull.Value)
                        {
                            meal.Reader_ant = Int32.Parse(row["reader_ant"].ToString().Trim());
                        }
                        if (row["meal_type_id"] != DBNull.Value)
                        {
                            meal.MealTypeID = Int32.Parse(row["meal_type_id"].ToString().Trim());
                        }
                        if (row["reader_ip_address"] != DBNull.Value)
                        {
                            meal.ReaderIPAddress = row["reader_ip_address"].ToString();
                        }
                        if (row["restaurant_id"] != DBNull.Value)
                        {
                            meal.RestaurantID = int.Parse(row["restaurant_id"].ToString().Trim());
                        }
                        if (row["name"] != DBNull.Value)
                        {
                            meal.Name = row["name"].ToString().Trim();
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            meal.Description = row["description"].ToString().Trim();
                        }
                        if (row["reader_peripherial"] != DBNull.Value)
                        {
                            meal.Reader_peripherial = int.Parse(row["reader_peripherial"].ToString().Trim());
                        }
                        if (row["reader_peripherial_desc"] != DBNull.Value)
                        {
                            meal.ReaderPeripherialDesc = row["reader_peripherial_desc"].ToString().Trim();
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            meal.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            meal.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            meal.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            meal.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
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

        public List<OnlineMealsPointsTO> getMealsPoints(string ipAddress, int ant_num)
        {
            DataSet dataSet = new DataSet();
            List<OnlineMealsPointsTO> mealsList = new List<OnlineMealsPointsTO>();
            string select = "";

            try
            {
                OnlineMealsPointsTO meal = new OnlineMealsPointsTO();

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM online_meals_points ");
                sb.Append("WHERE ");

                if (!ipAddress.Equals(""))
                {
                    sb.Append("reader_ip_address = '" + ipAddress + "' AND");
                }

                if (ant_num != -1)
                {

                    sb.Append(" reader_ant = '" + ant_num + "' AND");
                }
                select = sb.ToString(0, sb.ToString().Length - 3);



                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealsPoints");
                DataTable table = dataSet.Tables["MealsPoints"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        meal = new OnlineMealsPointsTO();
                        meal.PointID = int.Parse(row["point_id"].ToString().Trim());

                        if (row["reader_ant"] != DBNull.Value)
                        {
                            meal.Reader_ant = Int32.Parse(row["reader_ant"].ToString().Trim());
                        }
                        if (row["meal_type_id"] != DBNull.Value)
                        {
                            meal.MealTypeID = Int32.Parse(row["meal_type_id"].ToString().Trim());
                        }
                        if (row["reader_ip_address"] != DBNull.Value)
                        {
                            meal.ReaderIPAddress = row["reader_ip_address"].ToString();
                        }
                        if (row["restaurant_id"] != DBNull.Value)
                        {
                            meal.RestaurantID = int.Parse(row["restaurant_id"].ToString().Trim());
                        }
                        if (row["name"] != DBNull.Value)
                        {
                            meal.Name = row["name"].ToString().Trim();
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            meal.Description = row["description"].ToString().Trim();
                        }
                        if (row["reader_peripherial"] != DBNull.Value)
                        {
                            meal.Reader_peripherial = int.Parse(row["reader_peripherial"].ToString().Trim());
                        }
                        if (row["reader_peripherial_desc"] != DBNull.Value)
                        {
                            meal.ReaderPeripherialDesc = row["reader_peripherial_desc"].ToString().Trim();
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            meal.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            meal.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            meal.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            meal.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
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

        public OnlineMealsPointsTO find(string pointID)
        {
            OnlineMealsPointsTO meal = new OnlineMealsPointsTO();
            DataSet dataSet = new DataSet();

            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM online_meal_points WHERE point_id = '" + pointID.Trim() + "'", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "OnlineMealPoint");
                DataTable table = dataSet.Tables["OnlineMealPoint"];

                if (table.Rows.Count == 1)
                {

                    meal.PointID = int.Parse(table.Rows[0]["point_id"].ToString().Trim());

                    if (table.Rows[0]["reader_ant"] != DBNull.Value)
                    {
                        meal.Reader_ant = Int32.Parse(table.Rows[0]["reader_ant"].ToString().Trim());
                    }
                    if (table.Rows[0]["meal_type_id"] != DBNull.Value)
                    {
                        meal.MealTypeID = Int32.Parse(table.Rows[0]["meal_type_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["reader_ip_address"] != DBNull.Value)
                    {
                        meal.ReaderIPAddress = table.Rows[0]["reader_ip_address"].ToString();
                    }
                    if (table.Rows[0]["restaurant_id"] != DBNull.Value)
                    {
                        meal.RestaurantID = int.Parse(table.Rows[0]["restaurant_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["name"] != DBNull.Value)
                    {
                        meal.Name = table.Rows[0]["name"].ToString().Trim();
                    }
                    if (table.Rows[0]["description"] != DBNull.Value)
                    {
                        meal.Description = table.Rows[0]["description"].ToString().Trim();
                    }
                    if (table.Rows[0]["reader_peripherial"] != DBNull.Value)
                    {
                        meal.Reader_peripherial = int.Parse(table.Rows[0]["reader_peripherial"].ToString().Trim());
                    }
                    if (table.Rows[0]["reader_peripherial_desc"] != DBNull.Value)
                    {
                        meal.ReaderPeripherialDesc = table.Rows[0]["reader_peripherial_desc"].ToString().Trim();
                    }
                    if (table.Rows[0]["created_by"] != DBNull.Value)
                    {
                        meal.CreatedBy = table.Rows[0]["created_by"].ToString().Trim();
                    }
                    if (table.Rows[0]["modified_by"] != DBNull.Value)
                    {
                        meal.ModifiedBy = table.Rows[0]["modified_by"].ToString().Trim();
                    }
                    if (table.Rows[0]["created_time"] != DBNull.Value)
                    {
                        meal.CreatedTime = DateTime.Parse(table.Rows[0]["created_time"].ToString().Trim());
                    }
                    if (table.Rows[0]["modified_time"] != DBNull.Value)
                    {
                        meal.ModifiedTime = DateTime.Parse(table.Rows[0]["modified_time"].ToString().Trim());
                    }


                }

            }
            catch (Exception ex)
            {

                throw new Exception("Exception: " + ex.Message);
            }

            return meal;
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

        public List<OnlineMealsPointsTO> getMealsPoints(OnlineMealsPointsTO onlineMealsTO)
        {
            DataSet dataSet = new DataSet();
            OnlineMealsPointsTO meal = new OnlineMealsPointsTO();
            List<OnlineMealsPointsTO> mealsList = new List<OnlineMealsPointsTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" select p.*, r.name as restaurant, t.name as type from online_meals_points p, online_meals_restaurants r, online_meals_types t where r.restaurant_id = p.restaurant_id and p.meal_type_id=t.meal_type_id AND ");

                if ((onlineMealsTO.MealTypeID != -1) || (!onlineMealsTO.Name.Trim().Equals("")) ||
                    (!onlineMealsTO.Description.Trim().Equals("")) || (!onlineMealsTO.ReaderPeripherialDesc.Equals("")) || (!onlineMealsTO.ReaderIPAddress.Equals(""))
                    || (onlineMealsTO.Reader_peripherial != -1) || (onlineMealsTO.Reader_ant != -1) || (onlineMealsTO.RestaurantID != -1) || (onlineMealsTO.PointID != -1))
                {

                    if (onlineMealsTO.MealTypeID != -1)
                    {
                        //sb.Append(" UPPER(employee_id) LIKE '" + EmployeeID.Trim().ToUpper() + "' AND");
                        sb.Append(" meal_type_id = '" + onlineMealsTO.MealTypeID.ToString().Trim() + "' AND");
                    }
                    if (!onlineMealsTO.Name.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(name) LIKE N'%" + onlineMealsTO.Name.Trim().ToUpper() + "%' AND");
                    }
                    if (!onlineMealsTO.Description.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(description) LIKE N'%" + onlineMealsTO.Description.Trim().ToUpper() + "%' AND");
                    }
                    if (onlineMealsTO.Reader_peripherial != -1)
                    {
                        //sb.Append(" UPPER(employee_id) LIKE '" + EmployeeID.Trim().ToUpper() + "' AND");
                        sb.Append(" reader_peripherial = '" + onlineMealsTO.Reader_peripherial.ToString().Trim() + "' AND");
                    }
                    if (!onlineMealsTO.ReaderIPAddress.Trim().Equals(""))
                    {
                        sb.Append(" reader_ip_address LIKE '%" + onlineMealsTO.ReaderIPAddress.Trim().ToUpper() + "%' AND");
                    }
                    if (!onlineMealsTO.ReaderPeripherialDesc.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(reader_peripherial_desc) LIKE N'%" + onlineMealsTO.ReaderPeripherialDesc.Trim().ToUpper() + "%' AND");
                    }
                    if (onlineMealsTO.Reader_ant != -1)
                    {
                        //sb.Append(" UPPER(employee_id) LIKE '" + EmployeeID.Trim().ToUpper() + "' AND");
                        sb.Append(" reader_ant = '" + onlineMealsTO.Reader_ant.ToString().Trim() + "' AND");
                    }
                    if (onlineMealsTO.RestaurantID != -1)
                    {
                        //sb.Append(" UPPER(employee_id) LIKE '" + EmployeeID.Trim().ToUpper() + "' AND");
                        sb.Append(" restaurant_id = '" + onlineMealsTO.RestaurantID.ToString().Trim() + "' AND");
                    }
                    if (onlineMealsTO.PointID != -1)
                    {
                        //sb.Append(" UPPER(employee_id) LIKE '" + EmployeeID.Trim().ToUpper() + "' AND");
                        sb.Append(" point_id = '" + onlineMealsTO.PointID.ToString().Trim() + "' AND");
                    }
                }
                select = sb.ToString(0, sb.ToString().Length - 3);



                select += " ORDER BY point_id ";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "OnlineMealPoint");
                DataTable table = dataSet.Tables["OnlineMealPoint"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        meal = new OnlineMealsPointsTO();
                        meal.PointID = int.Parse(row["point_id"].ToString().Trim());

                        if (row["reader_ant"] != DBNull.Value)
                        {
                            meal.Reader_ant = Int32.Parse(row["reader_ant"].ToString().Trim());
                        }
                        if (row["meal_type_id"] != DBNull.Value)
                        {
                            meal.MealTypeID = Int32.Parse(row["meal_type_id"].ToString().Trim());
                        }
                        if (row["reader_ip_address"] != DBNull.Value)
                        {
                            meal.ReaderIPAddress = row["reader_ip_address"].ToString();
                        }
                        if (row["restaurant_id"] != DBNull.Value)
                        {
                            meal.RestaurantID = int.Parse(row["restaurant_id"].ToString().Trim());
                        }
                        if (row["name"] != DBNull.Value)
                        {
                            meal.Name = row["name"].ToString().Trim();
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            meal.Description = row["description"].ToString().Trim();
                        }
                        if (row["reader_peripherial"] != DBNull.Value)
                        {
                            meal.Reader_peripherial = int.Parse(row["reader_peripherial"].ToString().Trim());
                        }
                        if (row["reader_peripherial_desc"] != DBNull.Value)
                        {
                            meal.ReaderPeripherialDesc = row["reader_peripherial_desc"].ToString().Trim();
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            meal.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            meal.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            meal.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            meal.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
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

        public int insert(OnlineMealsPointsTO onlineMealsPoint)
        {
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO online_meals_points ");
                sbInsert.Append("(name, description,restaurant_id,reader_ip_address,reader_ant,reader_peripherial,reader_peripherial_desc,meal_type_id, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                if (onlineMealsPoint.Name.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + onlineMealsPoint.Name.Trim() + "', ");
                }
                if (onlineMealsPoint.Description.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + onlineMealsPoint.Description.Trim() + "', ");
                }
                if (onlineMealsPoint.RestaurantID == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + onlineMealsPoint.RestaurantID.ToString() + "', ");
                }
                if (onlineMealsPoint.ReaderIPAddress.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + onlineMealsPoint.ReaderIPAddress.Trim() + "', ");
                }
                if (onlineMealsPoint.Reader_ant == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + onlineMealsPoint.Reader_ant.ToString().Trim() + "', ");
                }
                if (onlineMealsPoint.Reader_peripherial == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + onlineMealsPoint.Reader_peripherial.ToString().Trim() + "', ");
                }
                if (onlineMealsPoint.ReaderPeripherialDesc.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + onlineMealsPoint.ReaderPeripherialDesc.ToString().Trim() + "', ");
                }
                if (onlineMealsPoint.MealTypeID == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + onlineMealsPoint.MealTypeID.ToString().Trim() + "', ");
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

        public bool update(OnlineMealsPointsTO onlineMealsPoint)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE online_meals_points SET ");
                if (!onlineMealsPoint.Name.Equals(""))
                {
                    sbUpdate.Append("name = N'" + onlineMealsPoint.Name + "', ");
                }
                else
                {
                    sbUpdate.Append("name = NULL, ");
                }
                if (!onlineMealsPoint.Description.Equals(""))
                {
                    sbUpdate.Append("description = N'" + onlineMealsPoint.Description + "', ");
                }
                else
                {
                    sbUpdate.Append("description = NULL, ");
                }
                if (onlineMealsPoint.RestaurantID != -1)
                {
                    sbUpdate.Append("restaurant_id = N'" + onlineMealsPoint.RestaurantID.ToString() + "', ");
                }
                else
                {
                    sbUpdate.Append("restaurant_id = NULL, ");
                }
                if (onlineMealsPoint.Reader_ant != -1)
                {
                    sbUpdate.Append("reader_ant = N'" + onlineMealsPoint.Reader_ant.ToString() + "', ");
                }
                else
                {
                    sbUpdate.Append("reader_ant = NULL, ");
                }
                if (!onlineMealsPoint.ReaderIPAddress.Equals(""))
                {
                    sbUpdate.Append("reader_ip_address = N'" + onlineMealsPoint.ReaderIPAddress.ToString() + "', ");
                }
                else
                {
                    sbUpdate.Append("reader_ip_address = NULL, ");
                }
                if (!onlineMealsPoint.ReaderPeripherialDesc.Equals(""))
                {
                    sbUpdate.Append("reader_peripherial_desc = N'" + onlineMealsPoint.ReaderPeripherialDesc.ToString() + "', ");
                }
                else
                {
                    sbUpdate.Append("reader_peripherial_desc = NULL, ");
                }
                if (onlineMealsPoint.Reader_peripherial != -1)
                {
                    sbUpdate.Append("reader_peripherial = N'" + onlineMealsPoint.Reader_peripherial.ToString() + "', ");
                }
                else
                {
                    sbUpdate.Append("reader_peripherial = NULL, ");
                }
                if (onlineMealsPoint.MealTypeID != -1)
                {
                    sbUpdate.Append("meal_type_id = N'" + onlineMealsPoint.MealTypeID.ToString() + "', ");
                }
                else
                {
                    sbUpdate.Append("meal_type_id = NULL, ");
                }
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().ToString().Trim() + "', modified_time = " + " NOW() ");
                sbUpdate.Append("WHERE point_id = '" + onlineMealsPoint.PointID.ToString().Trim() + "'");
                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                sqlTrans.Commit();

            }


            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }


        public bool delete(int pointID)
        {
            bool isDeleted = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM online_meals_points WHERE point_id = '" + pointID + "'");

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

        public List<OnlineMealsPointsTO> searchForIDs(string pointID)
        {
            OnlineMealsPointsTO meal = new OnlineMealsPointsTO();
            DataSet dataSet = new DataSet();
            ReaderTO readTO = new ReaderTO();
            List<OnlineMealsPointsTO> mealsList = new List<OnlineMealsPointsTO>();

            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM online_meals_points WHERE point_id IN (" + pointID.Trim() + ")", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Reader");
                DataTable table = dataSet.Tables["Reader"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        meal = new OnlineMealsPointsTO();
                        meal.PointID = int.Parse(row["point_id"].ToString().Trim());

                        if (row["reader_ant"] != DBNull.Value)
                        {
                            meal.Reader_ant = Int32.Parse(row["reader_ant"].ToString().Trim());
                        }
                        if (row["meal_type_id"] != DBNull.Value)
                        {
                            meal.MealTypeID = Int32.Parse(row["meal_type_id"].ToString().Trim());
                        }
                        if (row["reader_ip_address"] != DBNull.Value)
                        {
                            meal.ReaderIPAddress = row["reader_ip_address"].ToString();
                        }
                        if (row["restaurant_id"] != DBNull.Value)
                        {
                            meal.RestaurantID = int.Parse(row["restaurant_id"].ToString().Trim());
                        }
                        if (row["name"] != DBNull.Value)
                        {
                            meal.Name = row["name"].ToString().Trim();
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            meal.Description = row["description"].ToString().Trim();
                        }
                        if (row["reader_peripherial"] != DBNull.Value)
                        {
                            meal.Reader_peripherial = int.Parse(row["reader_peripherial"].ToString().Trim());
                        }
                        if (row["reader_peripherial_desc"] != DBNull.Value)
                        {
                            meal.ReaderPeripherialDesc = row["reader_peripherial_desc"].ToString().Trim();
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            meal.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            meal.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            meal.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            meal.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
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
    }
}
