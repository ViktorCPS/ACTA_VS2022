using System;
using System.Collections;
using System.Configuration;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using TransferObjects;
using Util;

namespace DataAccess
{
    public class MySQLMealsPointDAO:MealsPointDAO
    {
         MySqlConnection conn = null;
		//protected string dateTimeformat = "";
		MySqlTransaction _sqlTrans = null;

		public MySqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

        public MySQLMealsPointDAO()
		{
			conn = MySQLDAOFactory.getConnection();
			DAOController.GetInstance();
		}
        public MySQLMealsPointDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
			DAOController.GetInstance();
        }
        public int insert(string terminalSerial, string name, string description)
        {
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO meals_points ");
                sbInsert.Append("(terminal_serial, name, description , created_by, created_time) ");
                sbInsert.Append("VALUES (");
                if (!terminalSerial.Equals(""))
                {
                    sbInsert.Append("N'" + terminalSerial + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (!name.Equals(""))
                {
                    sbInsert.Append("N'" + name + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (!description.Equals(""))
                {
                    sbInsert.Append("N'" + description + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
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

        public bool delete(string pointsID)
        {
            bool isDeleted = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM meals_points WHERE point_id  = '" + pointsID + "'");

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

        public ArrayList getMealsPoints(int pointId, string teminalSerial, string name, string description)
        {
            DataSet dataSet = new DataSet();
            MealsPointTO mealsPoint = new MealsPointTO();
            ArrayList mealsAssignedList = new ArrayList();
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM meals_points ");
                if ((pointId != -1) || (!teminalSerial.Equals("")) || (!name.Equals("")) ||
                    (!description.Equals("")))
                {
                    sb.Append("WHERE ");
                    if (pointId != -1)
                    {
                        sb.Append("point_id = '" + pointId.ToString().Trim() + "' AND ");
                    }
                    if (!teminalSerial.Equals(""))
                    {
                        sb.Append("terminal_serial = N'" + teminalSerial.ToString().Trim() + "' AND ");
                    }
                    if (!name.Equals(""))
                    {
                        sb.Append("name = '" + name.ToString().Trim() + "' AND ");
                    }
                    if (!description.Equals(""))
                    {
                        sb.Append("description = '" + description.ToString().Trim() + "' AND ");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 4);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY terminal_serial";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealsPoint");
                DataTable table = dataSet.Tables["MealsPoint"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        mealsPoint = new MealsPointTO();
                        mealsPoint.PointID = Int32.Parse(row["point_id"].ToString().Trim());
                        if (row["terminal_serial"] != DBNull.Value)
                        {
                            mealsPoint.TerminalSerial = row["terminal_serial"].ToString();
                        }
                        if (row["name"] != DBNull.Value)
                        {
                            mealsPoint.Name = row["name"].ToString();
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            mealsPoint.Description = row["description"].ToString();
                        }

                        mealsAssignedList.Add(mealsPoint);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return mealsAssignedList;
        }

        public int getMealsPointsCount(int pointId, string teminalSerial, string name, string description)
        {
            DataSet dataSet = new DataSet();
            int count = 0;
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT count(mp.point_id) count_mealsPoint FROM meals_points mp");
                if ((pointId != -1) || (!teminalSerial.Equals("")) || (!name.Equals("")) ||
                    (!description.Equals("")))
                {
                    sb.Append("WHERE");
                    if (pointId != -1)
                    {
                        sb.Append(" point_id = " + pointId.ToString().Trim());
                    }
                    if (!teminalSerial.Equals(""))
                    {
                        sb.Append(" terminal_serial = " + teminalSerial.ToString().Trim());
                    }
                    if (!name.Equals(""))
                    {
                        sb.Append(" name = " + name.ToString().Trim());
                    }
                    if (!description.Equals(""))
                    {
                        sb.Append(" name = " + description.ToString().Trim());
                    }
                }
                select = sb.ToString();

                select = select + " ORDER BY terminal_serial";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealPoint");
                DataTable table = dataSet.Tables["MealPoint"];

                if (table.Rows.Count > 0)
                {
                    count = Int32.Parse(table.Rows[0]["count_count_mealsPoint"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return count;
        }


        public bool update(int pointId, string terminalSerial, string name, string description)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE meals_points SET ");
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
                if (!terminalSerial.Equals(""))
                {
                    sbUpdate.Append("terminal_serial = N'" + terminalSerial + "', ");
                }
                else
                {
                    sbUpdate.Append("terminal_serial = NULL, ");
                }
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().ToString().Trim() + "', modified_time = " + " NOW() ");
                sbUpdate.Append("WHERE point_id = '" + pointId.ToString().Trim() + "'");
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
    }
}
