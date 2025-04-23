using System;
using System.Collections;
using System.Text;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public class MySQLSecurityRoutesPointDAO : SecurityRoutesPointDAO
    {
        MySqlConnection conn = null;
		protected string dateTimeformat = "";

        public MySQLSecurityRoutesPointDAO()
		{
			conn = MySQLDAOFactory.getConnection();	
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MySQLSecurityRoutesPointDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public int getMaxID()
        {
            DataSet dataSet = new DataSet();
            int pointID = 0;
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT MAX(control_point_id) AS cp_id FROM security_routes_points", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Points");
                DataTable table = dataSet.Tables["Points"];

                if (table.Rows.Count == 1 && !table.Rows[0]["cp_id"].Equals(DBNull.Value))
                {
                    pointID = Int32.Parse(table.Rows[0]["cp_id"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return pointID;
        }
        public int insert(int controlPointID, string name, string description, string tagID)
        {
            int rowsAffected = 0;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbInsert = new StringBuilder();

                sbInsert.Append("INSERT INTO security_routes_points (control_point_id, name, description, tag_id, created_by, created_time) ");
                sbInsert.Append("VALUES ('" + controlPointID.ToString().Trim() + "', N'" + name.Trim() + "', N'" + description.Trim() +
                    "', '" + tagID.Trim() + "', N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");

                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    sqlTrans.Commit();
                }
                else
                {
                    sqlTrans.Rollback();
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw ex;
            }
            return rowsAffected;
        }

        public bool update(int controlPointID, string name, string description, string tagID)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE security_routes_points SET ");
                if (!name.Trim().Equals(""))
                {
                    sbUpdate.Append("name = N'" + name.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("name = null, ");
                }
                if (!description.Trim().Equals(""))
                {
                    sbUpdate.Append("description = N'" + description.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("description = null, ");
                }
                if (!tagID.Equals(""))
                {
                    sbUpdate.Append("tag_id = '" + tagID.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("tag_id = null, ");
                }
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = NOW() ");
                sbUpdate.Append("WHERE control_point_id = '" + controlPointID.ToString().Trim() + "'");

                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                    sqlTrans.Commit();
                }
                else
                {
                    sqlTrans.Rollback();
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw ex;
            }

            return isUpdated;
        }

        public ArrayList getPoints(int pointID, string name, string desc, string tagID)
        {
            DataSet dataSet = new DataSet();
            SecurityRoutesPointTO pointTO = new SecurityRoutesPointTO();
            ArrayList pointsList = new ArrayList();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM security_routes_points ");

                if ((pointID != -1) || (!name.Trim().Equals("")) || (!desc.Trim().Equals("")) || (!tagID.Equals("")))
                {
                    sb.Append("WHERE ");
                    if (pointID != -1)
                    {
                        sb.Append("control_point_id = '" + pointID.ToString().Trim() + "' AND ");
                    }
                    if (!name.Trim().Equals(""))
                    {
                        sb.Append("UPPER(name) LIKE N'" + name.ToUpper().Trim() + "' AND ");
                    }
                    if (!desc.Trim().Equals(""))
                    {
                        sb.Append("UPPER(description) LIKE N'" + desc.ToUpper().Trim() + "' AND ");
                    }
                    if (!tagID.Equals(""))
                    {
                        sb.Append("tag_id = '" + tagID.ToString().Trim() + "' AND ");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 4);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY name";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Points");
                DataTable table = dataSet.Tables["Points"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pointTO = new SecurityRoutesPointTO();

                        pointTO.ControlPointID = Int32.Parse(row["control_point_id"].ToString().Trim());
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            pointTO.Name = row["name"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            pointTO.Description = row["description"].ToString().Trim();
                        }
                        if (!row["tag_id"].Equals(DBNull.Value))
                        {
                            pointTO.TagID = row["tag_id"].ToString().Trim();
                        }

                        pointsList.Add(pointTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return pointsList;
        }

        public bool delete(int controlPointID)
        {
            bool isDeleted = false;
            MySqlTransaction trans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                string select = "";

                // Delete form security_routes_points
                select = "DELETE FROM security_routes_points WHERE control_point_id = '" + controlPointID.ToString().Trim() + "' ";
                MySqlCommand cmd = new MySqlCommand(select, conn, trans);
                int affectedRows = cmd.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    isDeleted = true;
                    trans.Commit();
                }
                else
                {
                    trans.Rollback();
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }

            return isDeleted;
        }
    }
}
