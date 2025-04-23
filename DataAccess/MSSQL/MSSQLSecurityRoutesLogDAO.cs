using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public class MSSQLSecurityRoutesLogDAO : SecurityRoutesLogDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLSecurityRoutesLogDAO()
		{
			conn = MSSQLDAOFactory.getConnection();	
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MSSQLSecurityRoutesLogDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public int insert(int readerID, string tagID, int employeeID, DateTime eventTime)
        {
            int rowsAffected = 0;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbInsert = new StringBuilder();

                sbInsert.Append("INSERT INTO security_routes_log (reader_id, employee_id, tag_id, event_time, created_by, created_time) ");
                sbInsert.Append("VALUES ('" + readerID.ToString().Trim() + "', '" + "', '" + employeeID.ToString().Trim() + tagID.Trim() +
                    "', '" + eventTime.ToString(dateTimeformat).Trim() + "', N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

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
        public int insert(int readerID, string tagID, int employeeID, DateTime eventTime,bool doCommit)
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

                sbInsert.Append("INSERT INTO security_routes_log (reader_id, employee_id, tag_id, event_time, created_by, created_time) ");
                sbInsert.Append("VALUES ('" + readerID.ToString().Trim() + "', '" + employeeID.ToString().Trim() + "', '" + tagID.Trim() + 
                    "', '" + eventTime.ToString(dateTimeformat).Trim() + "', N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    if (doCommit)
                    {
                        sqlTrans.Commit();
                    }
                }
                else
                {
                    if (doCommit)
                    {
                        sqlTrans.Rollback();
                    }
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback();
                }
                throw ex;
            }
            return rowsAffected;
        }

        public ArrayList getLogsInterval(int employeeID, int readerID, string tagID, DateTime fromTime, DateTime toTime, string wUnits)
        {
            DataSet dataSet = new DataSet();

            SecurityRoutesLogTO log = new SecurityRoutesLogTO();
            ArrayList logsList = new ArrayList();
            string select = "";
            CultureInfo ci = CultureInfo.InvariantCulture;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT l.*, p.control_point_id point_id ");
                sb.Append("FROM security_routes_log l, security_routes_points p, employees e ");
                sb.Append("WHERE ");

                if ((employeeID != -1) || (readerID != -1) || (!tagID.Equals("")) ||
                    (!fromTime.Equals(new DateTime(0))) || (!toTime.Equals(new DateTime(0))))
                {
                    if (employeeID != -1)
                    {
                        sb.Append("l.employee_id = '" + employeeID.ToString().Trim().ToUpper() + "' AND ");
                    }
                    if (readerID != -1)
                    {
                        sb.Append("l.reader_id = '" + readerID.ToString().Trim().ToUpper() + "' AND ");
                    }
                    if (!tagID.Equals(""))
                    {
                        sb.Append("l.tag_id = '" + tagID.ToString().Trim().ToUpper() + "' AND ");
                    }
                    if (!fromTime.Equals(new DateTime(0)) && !toTime.Equals(new DateTime(0)))
                    {
                        sb.Append("l.event_time >= CONVERT(datetime, '" + fromTime.ToString("yyyy-MM-dd") + "') AND ");
                        sb.Append("l.event_time < CONVERT(dateTime, '" + toTime.AddDays(1).ToString("yyyy-MM-dd") + "') AND ");
                    }
                }

                select = sb.ToString();

                select += "l.tag_id = p.tag_id AND l.employee_id = e.employee_id ";

                if (!wUnits.Equals(""))
                {
                    select += "AND e.working_unit_id IN (" + wUnits + ") ";
                }

                select += "ORDER BY l.employee_id, l.event_time";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Log");

                DataTable table = dataSet.Tables["Log"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        log = new SecurityRoutesLogTO();
                        log.LogID = Int32.Parse(row["log_id"].ToString().Trim());

                        if (row["employee_id"] != DBNull.Value)
                        {
                            log.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["reader_id"] != DBNull.Value)
                        {
                            log.ReaderID = Int32.Parse(row["reader_id"].ToString().Trim());
                        }
                        if (row["tag_id"] != DBNull.Value)
                        {
                            log.TagID = row["tag_id"].ToString().Trim();
                        }
                        if (row["event_time"] != DBNull.Value)
                        {
                            log.EventTime = DateTime.Parse(row["event_time"].ToString());
                        }
                        if (row["point_id"] != DBNull.Value)
                        {
                            log.PointID = Int32.Parse(row["point_id"].ToString().Trim());
                        }

                        logsList.Add(log);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return logsList;
        }

        public ArrayList search(int employeeID, string tagID, string wUnits, DateTime dateFrom, DateTime dateTo, DateTime fromTime, DateTime toTime)
        {
            DataSet dataSet = new DataSet();

            SecurityRoutesLogTO log = new SecurityRoutesLogTO();
            ArrayList logsList = new ArrayList();
            string select = "";
            CultureInfo ci = CultureInfo.InvariantCulture;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT l.*, p.name point_name, e.first_name, e.last_name ");
                sb.Append("FROM security_routes_log l, security_routes_points p, employees e ");
                sb.Append("WHERE ");

               
                    if (employeeID != -1)
                    {
                        sb.Append("l.employee_id = '" + employeeID.ToString().Trim().ToUpper() + "' AND ");
                    }                   
                    if (!tagID.Equals(""))
                    {
                        sb.Append("l.tag_id = '" + tagID.ToString().Trim().ToUpper() + "' AND ");
                    }
                    if (!dateFrom.Equals(new DateTime(0)) && !dateTo.Equals(new DateTime(0)))
                    {
                        sb.Append("l.event_time >= CONVERT(datetime, '" + dateFrom.ToString("yyyy-MM-dd") + "') AND ");
                        sb.Append("l.event_time < CONVERT(dateTime, '" + dateTo.AddDays(1).ToString("yyyy-MM-dd") + "') AND ");
                    }
                    if (!fromTime.Equals(new DateTime()) || !toTime.Equals(new DateTime()))
                    {
                        if (!fromTime.Equals(new DateTime()))
                            sb.Append(" CONVERT(datetime, CONVERT(varchar(10), l.event_time, 108), 108) >= CONVERT(datetime,'" + fromTime.ToString("HH:mm:ss") + "', 108) AND ");
                        if (!toTime.Equals(new DateTime()))
                            sb.Append(" CONVERT(datetime, CONVERT(varchar(10), l.event_time, 108), 108) < CONVERT(datetime,'" + toTime.AddMinutes(1).ToString("HH:mm:ss") + "', 108) AND ");
                    }
                

                select = sb.ToString();

                select += "l.tag_id = p.tag_id AND l.employee_id = e.employee_id ";

                if (!wUnits.Equals(""))
                {
                    select += "AND e.working_unit_id IN (" + wUnits + ") ";
                }

                select += "ORDER BY l.employee_id, l.event_time";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Log");

                DataTable table = dataSet.Tables["Log"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        log = new SecurityRoutesLogTO();
                        log.LogID = Int32.Parse(row["log_id"].ToString().Trim());

                        if (row["employee_id"] != DBNull.Value)
                        {
                            log.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            log.EmployeeName = row["last_name"].ToString().Trim();
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            if (log.EmployeeName.Length > 0)
                                log.EmployeeName += " ";
                            log.EmployeeName += row["first_name"].ToString().Trim();
                        }
                        if (row["reader_id"] != DBNull.Value)
                        {
                            log.ReaderID = Int32.Parse(row["reader_id"].ToString().Trim());
                        }
                        if (row["tag_id"] != DBNull.Value)
                        {
                            log.TagID = row["tag_id"].ToString().Trim();
                        }
                        if (row["event_time"] != DBNull.Value)
                        {
                            log.EventTime = DateTime.Parse(row["event_time"].ToString());
                        }
                        if (row["point_name"] != DBNull.Value)
                        {
                            log.PointName = row["point_name"].ToString().Trim();
                        }

                        logsList.Add(log);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return logsList;
        }

        public int getEmplCount(string employeeID)
        {
            DataSet dataSet = new DataSet();
            int count = 0;
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                select = "SELECT COUNT(*) count FROM security_routes_log WHERE employee_id = '" + employeeID.ToString().Trim() + "'";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Log");

                DataTable table = dataSet.Tables["Log"];

                if (table.Rows.Count > 0)
                {
                    count = Int32.Parse(table.Rows[0]["count"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return count;
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
