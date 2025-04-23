using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    class MSSQLSecurityRouteScheduleDAO : SecurityRouteScheduleDAO
    {
        SqlConnection conn = null;
        protected string dateTimeformat = "";
        SqlTransaction _sqlTrans = null;

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLSecurityRouteScheduleDAO()
		{
			conn = MSSQLDAOFactory.getConnection();	
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MSSQLSecurityRouteScheduleDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public int insert(int employeeID, int routeId, DateTime date, bool doCommit)
        {
            SqlTransaction sqlTrans = null;

            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO security_routes_schedule ");
                sbInsert.Append("(employee_id, security_route_id, date, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                if (employeeID == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + employeeID.ToString().Trim() + "', ");
                }
                if (routeId == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + routeId.ToString().Trim() + "', ");
                }
                if (!date.Equals(new DateTime()))
                {
                    sbInsert.Append("'" + date.ToString(dateTimeformat).Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("null, ");
                }

                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit)
                {
                    sqlTrans.Commit();
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

        public ArrayList getRoutesSch(int emplID, int routeID, DateTime from, DateTime to)
        {
            DataSet dataSet = new DataSet();
            SecurityRouteScheduleTO schTO = new SecurityRouteScheduleTO();
            ArrayList schedulesList = new ArrayList();
            string select = "";
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT rs.*, empl.last_name, empl.first_name, r.name ");
                sb.Append("FROM security_routes_schedule rs, employees empl, security_routes_hdr r ");
                sb.Append("WHERE ");

                if ((emplID != -1) || (routeID != -1) || (!from.Equals(new DateTime(0))) || (!to.Equals(new DateTime(0))))
                {
                    if (emplID != -1)
                    {
                        sb.Append("rs.employee_id = '" + emplID.ToString().Trim() + "' AND ");
                    }
                    if (routeID != -1)
                    {
                        sb.Append("rs.security_route_id = '" + routeID.ToString().Trim() + "' AND ");
                    }
                    if (!from.Equals(new DateTime()) && !to.Equals(new DateTime()))
                    {
                        sb.Append(" rs.date >= CONVERT(datetime, '" + from.ToString("yyyy-MM-dd") + "') AND ");
                        sb.Append(" rs.date < CONVERT(datetime, '" + to.AddDays(1).ToString("yyyy-MM-dd") + "') AND ");
                    }
                }

                select = sb.ToString();

                select += "empl.employee_id = rs.employee_id AND r.security_route_id = rs.security_route_id";

                select += " ORDER BY rs.employee_id, rs.date";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Schedules");

                DataTable table = dataSet.Tables["Schedules"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        schTO = new SecurityRouteScheduleTO();

                        if (row["employee_id"] != DBNull.Value)
                        {
                            schTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["date"] != DBNull.Value)
                        {
                            schTO.Date = DateTime.Parse(row["date"].ToString());
                        }
                        if (row["security_route_id"] != DBNull.Value)
                        {
                            schTO.SecurityRouteID = Int32.Parse(row["security_route_id"].ToString().Trim());
                        }

                        if (row["last_name"] != DBNull.Value)
                        {
                            schTO.EmployeeName = row["last_name"].ToString().Trim();
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            schTO.EmployeeName += " " + row["first_name"].ToString().Trim();
                        }
                        if (row["name"] != DBNull.Value)
                        {
                            schTO.RouteName = row["name"].ToString().Trim();
                        }

                        schedulesList.Add(schTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return schedulesList;
        }

        public bool delete(int emplID, int routeID, DateTime date)
        {
            bool isDeleted = false;
            SqlTransaction trans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                string select = "";

                select = "DELETE FROM security_routes_schedule WHERE employee_id = '" + emplID.ToString().Trim() + "' "
                    + "AND security_route_id = '" + routeID.ToString().Trim() + "' "
                    + "AND CONVERT(datetime, CONVERT(VARCHAR(20), date), 111) = CONVERT(datetime, '" + date.ToString("yyyy-MM-dd") + "', 111) ";
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

        public bool delete(int emplID, DateTime date, bool doCommit)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = null;

            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }

            try
            {
                string select = "";

                select = "DELETE FROM security_routes_schedule WHERE employee_id = '" + emplID.ToString().Trim() + "' "
                    + "AND CONVERT(datetime, CONVERT(VARCHAR(20), date), 111) = CONVERT(datetime, '" + date.ToString("yyyy-MM-dd") + "', 111) ";

                SqlCommand cmd = new SqlCommand(select, conn, sqlTrans);
                int affectedRows = cmd.ExecuteNonQuery();

                if (affectedRows >= 0)
                {
                    isDeleted = true;
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

            return isDeleted;
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
