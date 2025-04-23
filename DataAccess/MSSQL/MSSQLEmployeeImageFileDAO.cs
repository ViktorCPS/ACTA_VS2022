using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using System.Globalization;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using TransferObjects;
using Util;

namespace DataAccess
{
    public class MSSQLEmployeeImageFileDAO : EmployeeImageFileDAO
    {
        SqlConnection conn = null;
        string database = "";
        SqlTransaction _sqlTrans = null;

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLEmployeeImageFileDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
            database = Constants.GetDatabaseString + "_files.actamgr.";
		}
        public MSSQLEmployeeImageFileDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            database = Constants.GetDatabaseString + "_files.actamgr.";
        }
        public void SetDBConnection(Object dbConnection)
        {
            conn = dbConnection as SqlConnection;
        }

        public int insert(int employeeID, byte[] picture, bool doCommit)
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
                sbInsert.Append("INSERT INTO " + database + "employee_image_files ");
                sbInsert.Append("(employee_id, picture, created_by, created_time, modified_time) ");
                sbInsert.Append("VALUES (");

                sbInsert.Append("" + employeeID + ", ");

                sbInsert.Append("@Picture, ");

                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE(), GETDATE()) ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                cmd.Parameters.Add("@Picture", SqlDbType.Image, picture.Length).Value = picture;

                rowsAffected = cmd.ExecuteNonQuery();
                if (doCommit)
                {
                    sqlTrans.Commit();
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

        public bool update(int employeeID, byte[] picture, bool doCommit)
        {
            bool isUpdated = false;
            SqlTransaction sqlTrans = null;

            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE " + database + "employee_image_files SET ");

                if (picture != null)
                {
                    sbUpdate.Append("picture = @Picture, ");
                }

                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");

                if (employeeID != -1)
                {
                    sbUpdate.Append(" WHERE ");
                    sbUpdate.Append(" employee_id = " + employeeID);
                }

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                cmd.Parameters.Add("@Picture", SqlDbType.Image, picture.Length).Value = picture;

                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }
                if (doCommit)
                {
                    sqlTrans.Commit();
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                    sqlTrans.Rollback("UPDATE");
                else
                    sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool delete(int employeeID, bool doCommit)
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
                sbDelete.Append("DELETE FROM " + database + "employee_image_files ");

                if (employeeID != -1)
                {
                    sbDelete.Append(" WHERE ");
                    sbDelete.Append(" employee_id = " + employeeID);
                }

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

        public bool deleteAll(string employeeID, bool doCommit)
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
                sbDelete.Append("DELETE FROM " + database + "employee_image_files ");

                if (employeeID != "")
                {
                    sbDelete.Append(" WHERE ");
                    sbDelete.Append(" employee_id IN (" + employeeID.ToString().Trim() + ") ");
                }

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
        /*public ExtraHourTO find(int employeeID, DateTime dateEarned)
		{
			DataSet dataSet = new DataSet();
			ExtraHourTO extraHourTO = new ExtraHourTO();
			try
			{
				SqlCommand cmd = new SqlCommand("SELECT employee_id, date_earned, extra_time_amt FROM extra_hours WHERE employee_id = " 
					+ employeeID + " AND date_earned = '" + dateEarned.ToString("yyy-MM-dd").Trim() + "'", conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ExtraHour");
				DataTable table = dataSet.Tables["ExtraHour"];

				if (table.Rows.Count == 1)
				{
					extraHourTO = new ExtraHourTO();

					if (table.Rows[0]["employee_id"] != DBNull.Value)
					{
						extraHourTO.EmployeeID = Int32.Parse(table.Rows[0]["employee_id"].ToString().Trim());
					}					

					if (!table.Rows[0]["date_earned"].Equals(DBNull.Value))
					{
						extraHourTO.DateEarned = (DateTime)table.Rows[0]["date_earned"];
					}

					if (!table.Rows[0]["extra_time_amt"].Equals(DBNull.Value))
					{
						extraHourTO.ExtraTimeAmt = Int32.Parse(table.Rows[0]["extra_time_amt"].ToString().Trim());						
						extraHourTO.CalculatedTimeAmt = Util.Misc.transformMinToStringTime(extraHourTO.ExtraTimeAmt);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return extraHourTO;
		}*/

        public ArrayList getEmployeeImageFiles(int employeeID)
        {
            DataSet dataSet = new DataSet();
            EmployeeImageFileTO employeeImageFileTO = new EmployeeImageFileTO();
            ArrayList employeeImageFileList = new ArrayList();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT employee_id, picture, modified_time FROM " + database + "employee_image_files ");

                if (employeeID != -1)
                {
                    sb.Append(" WHERE ");
                    sb.Append(" employee_id = " + employeeID);
                }

                select = sb.ToString() + " ORDER BY employee_id";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeImageFile");
                DataTable table = dataSet.Tables["EmployeeImageFile"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        employeeImageFileTO = new EmployeeImageFileTO();

                        if (!row["employee_id"].Equals(DBNull.Value))
                        {
                            employeeImageFileTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (!row["modified_time"].Equals(DBNull.Value))
                        {
                            employeeImageFileTO.ModifiedTime = (DateTime)row["modified_time"];
                        }
                        if (!row["picture"].Equals(DBNull.Value))
                        {
                            employeeImageFileTO.Picture = (byte[])row["picture"];
                        }

                        employeeImageFileList.Add(employeeImageFileTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return employeeImageFileList;
        }

        public int getEmployeeImageFilesCount(int employeeID)
        {
            DataSet dataSet = new DataSet();
            int count = -1;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT COUNT(*) AS count FROM " + database + "employee_image_files ");

                if (employeeID != -1)
                {
                    sb.Append(" WHERE ");
                    sb.Append(" employee_id = " + employeeID);
                }

                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeImageFile");
                DataTable table = dataSet.Tables["EmployeeImageFile"];

                if (table.Rows.Count > 0)
                {
                    count = Int32.Parse(table.Rows[0]["count"].ToString().Trim());
                }
            }
            catch
            {
                count = -1;
            }

            return count;
        }

        public ArrayList getEmployeeImageInfo(string[] statuses)
        {
            DataSet dataSet = new DataSet();
            EmployeeImageFileTO employeeImageFileTO = new EmployeeImageFileTO();
            ArrayList employeeImageFileList = new ArrayList();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT employees.employee_id, employees.picture, " + database + "employee_image_files.modified_time ");
                sb.Append(" FROM employees, " + database + "employee_image_files ");
                sb.Append(" WHERE ");
                sb.Append(" employees.employee_id = " + database + "employee_image_files.employee_id ");

                if (statuses.Length > 0)
                {
                    string statusString = " AND (";
                    foreach (string status in statuses)
                    {
                        statusString += (" UPPER(employees.status) = N'" + status.ToUpper() + "' OR ");
                    }
                    statusString = statusString.Substring(0, statusString.Length - 3);
                    sb.Append(statusString + ") ");
                }

                sb.Append(" ORDER BY employees.employee_id");

                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeImageFile");
                DataTable table = dataSet.Tables["EmployeeImageFile"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        employeeImageFileTO = new EmployeeImageFileTO();

                        if (!row["employee_id"].Equals(DBNull.Value))
                        {
                            employeeImageFileTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (!row["modified_time"].Equals(DBNull.Value))
                        {
                            employeeImageFileTO.ModifiedTime = (DateTime)row["modified_time"];
                        }
                        if (!row["picture"].Equals(DBNull.Value))
                        {
                            employeeImageFileTO.PictureName = row["picture"].ToString();
                        }

                        employeeImageFileList.Add(employeeImageFileTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return employeeImageFileList;
        }

        public ArrayList getEmployeeImageForSnapshots(string employeeID)
        {
            DataSet dataSet = new DataSet();
            EmployeeImageFileTO employeeImageFileTO = new EmployeeImageFileTO();
            ArrayList employeeImageFileList = new ArrayList();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT employee_id, picture FROM " + database + "employee_image_files ");

                if (!employeeID.Equals(""))
                {
                    sb.Append(" WHERE ");
                    sb.Append("employee_id IN (" + employeeID.ToString().Trim() + ") ");
                }
                sb.Append(" ORDER BY employee_id");

                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeImageFile");
                DataTable table = dataSet.Tables["EmployeeImageFile"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        employeeImageFileTO = new EmployeeImageFileTO();

                        if (!row["employee_id"].Equals(DBNull.Value))
                        {
                            employeeImageFileTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (!row["picture"].Equals(DBNull.Value))
                        {
                            employeeImageFileTO.Picture = (byte[])row["picture"];
                        }

                        employeeImageFileList.Add(employeeImageFileTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return employeeImageFileList;
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

        // TODO!!!
        public void serialize(ArrayList employeeImageFileTOList)
        {
            try
            {
                string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLEmployeeImageFileFile"];
                Stream stream = File.Open(filename, FileMode.Create);

                EmployeeImageFileTO[] employeeImageFileTOArray = (EmployeeImageFileTO[])employeeImageFileTOList.ToArray(typeof(EmployeeImageFileTO));

                XmlSerializer bformatter = new XmlSerializer(typeof(EmployeeImageFileTO[]));
                bformatter.Serialize(stream, employeeImageFileTOArray);
                stream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
