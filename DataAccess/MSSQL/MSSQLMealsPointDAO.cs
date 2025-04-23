using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using TransferObjects;
using Util;

namespace DataAccess
{
    public class MSSQLMealsPointDAO:MealsPointDAO
    {
        SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;

		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

        public MSSQLMealsPointDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
		}
        public MSSQLMealsPointDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
        }
        public int insert(string terminalSerial, string name, string description)
        {
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
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
                if (! name.Equals(""))
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

        public bool delete(string pointsID)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM meals_points WHERE point_id  = '" + pointsID + "'");

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
                        sb.Append("name = N'" + name.ToString().Trim() + "' AND ");
                    }
                    if (!description.Equals(""))
                    {
                        sb.Append("description = N'" + description.ToString().Trim() + "' AND ");
                    }

                    select = sb.ToString(0, sb.Length - 4);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY terminal_serial";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealsPoint");
                DataTable table = dataSet.Tables["MealsPoint"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        mealsPoint = new MealsPointTO();
                        mealsPoint.PointID = Int32.Parse(row["point_id"].ToString().Trim());
                        if(row["terminal_serial"] != DBNull.Value)
                        {
                            mealsPoint.TerminalSerial = row["terminal_serial"].ToString();
                        }
                        if (row["name"] != DBNull.Value)
                        {
                            mealsPoint.Name =row["name"].ToString();
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
                    sb.Append(" WHERE");
                    if (pointId != -1)
                    {
                        sb.Append(" point_id = " + pointId.ToString().Trim() + " AND");
                    }
                    if (!teminalSerial.Equals(""))
                    {
                        sb.Append(" terminal_serial = '" + teminalSerial.ToString().Trim() + "' AND");
                    }
                    if (!name.Equals(""))
                    {
                        sb.Append(" name = '" + name.ToString().Trim() + "' AND");
                    }
                    if (!description.Equals(""))
                    {
                        sb.Append(" description = '" + description.ToString().Trim() + "' AND");
                    }
                    select = sb.ToString(0, sb.Length - 3);
                }
                else
                 {
                    select = sb.ToString();
                }
                

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealPoint");
                DataTable table = dataSet.Tables["MealPoint"];

                if (table.Rows.Count > 0)
                {
                    count = Int32.Parse(table.Rows[0]["count_mealsPoint"].ToString().Trim());
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
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
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
               
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().ToString().Trim() + "', modified_time = " + " GETDATE() ");
                sbUpdate.Append("WHERE point_id = '" + pointId.ToString().Trim() + "'");
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
