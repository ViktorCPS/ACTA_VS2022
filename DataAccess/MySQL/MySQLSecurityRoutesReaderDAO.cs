using System;
using System.Collections;
using System.Text;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public class MySQLSecurityRoutesReaderDAO : SecurityRoutesReaderDAO
    {
        MySqlConnection conn = null;
		protected string dateTimeformat = "";

        public MySQLSecurityRoutesReaderDAO()
		{
			conn = MySQLDAOFactory.getConnection();	
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MySQLSecurityRoutesReaderDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public int insert(string name, string description)
        {
            int rowsAffected = 0;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbInsert = new StringBuilder();

                sbInsert.Append("INSERT INTO security_routes_readers (name, description, created_by, created_time) ");
                sbInsert.Append("VALUES (N'" + name.Trim() + "', N'" + description.Trim() +
                    "', N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");

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

        public bool update(int readerID, string name, string description)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE security_routes_readers SET ");
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
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = NOW() ");
                sbUpdate.Append("WHERE reader_id = '" + readerID.ToString().Trim() + "'");

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

        public ArrayList getReaders(int readerID, string name, string desc)
        {
            DataSet dataSet = new DataSet();
            SecurityRoutesReaderTO readerTO = new SecurityRoutesReaderTO();
            ArrayList readersList = new ArrayList();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM security_routes_readers ");

                if ((readerID != -1) || (!name.Trim().Equals("")) || (!desc.Trim().Equals("")))
                {
                    sb.Append("WHERE ");
                    if (readerID != -1)
                    {
                        sb.Append("reader_id = '" + readerID.ToString().Trim() + "' AND ");
                    }
                    if (!name.Trim().Equals(""))
                    {
                        sb.Append("UPPER(name) LIKE N'" + name.ToUpper().Trim() + "' AND ");
                    }
                    if (!desc.Trim().Equals(""))
                    {
                        sb.Append("UPPER(description) LIKE N'" + desc.ToUpper().Trim() + "' AND ");
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

                sqlDataAdapter.Fill(dataSet, "Readers");
                DataTable table = dataSet.Tables["Readers"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        readerTO = new SecurityRoutesReaderTO();

                        readerTO.ReaderID = Int32.Parse(row["reader_id"].ToString().Trim());
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            readerTO.Name = row["name"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            readerTO.Description = row["description"].ToString().Trim();
                        }

                        readersList.Add(readerTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return readersList;
        }

        public bool delete(int readerID)
        {
            bool isDeleted = false;
            MySqlTransaction trans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                string select = "";

                // Delete form security_routes_points
                select = "DELETE FROM security_routes_readers WHERE reader_id = '" + readerID.ToString().Trim() + "' ";
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
