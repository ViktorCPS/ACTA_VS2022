using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public class MSSQLSecurityRoutesReaderDAO : SecurityRoutesReaderDAO
    {
        SqlConnection conn = null;
        protected string dateTimeformat = "";

        public MSSQLSecurityRoutesReaderDAO()
		{
			conn = MSSQLDAOFactory.getConnection();	
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MSSQLSecurityRoutesReaderDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public int insert(string name, string description)
        {
            int rowsAffected = 0;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbInsert = new StringBuilder();

                sbInsert.Append("INSERT INTO security_routes_readers (name, description, created_by, created_time) ");
                sbInsert.Append("VALUES (N'" + name.Trim() + "', N'" + description.Trim() +
                    "', N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
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
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

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
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE reader_id = '" + readerID.ToString().Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
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

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

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
            SqlTransaction trans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                string select = "";

                // Delete form security_routes_points
                select = "DELETE FROM security_routes_readers WHERE reader_id = '" + readerID.ToString().Trim() + "' ";
                SqlCommand cmd = new SqlCommand(select, conn, trans);
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
