using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using MySql.Data.MySqlClient;
using TransferObjects;
using System.Data;

namespace DataAccess
{
   public class MySQLOnlineMealsIntervalsDAO:OnlineMealsIntervalsDAO
    {
         MySqlConnection conn = null;
        protected string dateTimeformat = "";
        MySqlTransaction _sqlTrans = null;

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MySQLOnlineMealsIntervalsDAO()
        {
            conn = MySQLDAOFactory.getConnection();
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public MySQLOnlineMealsIntervalsDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public List<OnlineMealsIntervalsTO> getAll(string type)
        {
            DataSet dataSet = new DataSet();
            List<OnlineMealsIntervalsTO> mealsList = new List<OnlineMealsIntervalsTO>();
            string select = "";

            try
            {
                OnlineMealsIntervalsTO meal = new OnlineMealsIntervalsTO();

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM online_meals_intervals");
                if (!type.Equals(""))
                {

                    sb.Append(" WHERE type ='" + type + "'");
                }
                select = sb.ToString();


                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealsIntervals");
                DataTable table = dataSet.Tables["MealsIntervals"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        meal = new OnlineMealsIntervalsTO();
                        meal.Rec_id = Int32.Parse(row["rec_id"].ToString().Trim());


                        if (row["interval_start_time"] != DBNull.Value)
                        {
                            meal.Interval_start_time = DateTime.Parse(row["interval_start_time"].ToString());
                        }
                        if (row["interval_end_time"] != DBNull.Value)
                        {
                            meal.Interval_end_time = DateTime.Parse(row["interval_end_time"].ToString());
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            meal.Created_by = row["created_by"].ToString().Trim();
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            meal.Modified_by = row["modified_by"].ToString().Trim();
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            meal.Modified_time = DateTime.Parse(row["modified_time"].ToString());
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            meal.Created_time = DateTime.Parse(row["created_time"].ToString());
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
    }
}
