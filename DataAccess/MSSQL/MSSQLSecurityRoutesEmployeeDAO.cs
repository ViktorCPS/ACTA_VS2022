using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public class MSSQLSecurityRoutesEmployeeDAO : SecurityRoutesEmployeeDAO
    {
        SqlConnection conn = null;
        protected string dateTimeformat = "";
        SqlTransaction _sqlTrans = null;

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLSecurityRoutesEmployeeDAO()
		{
			conn = MSSQLDAOFactory.getConnection();	
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MSSQLSecurityRoutesEmployeeDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public int insert(string employeeID)
        {
            int rowsAffected = 0;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbInsert = new StringBuilder();

                sbInsert.Append("INSERT INTO security_routes_employees (employee_id, created_by, created_time) ");
                sbInsert.Append("VALUES ('" + employeeID.ToString().Trim() + "', N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

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

        public bool delete(string employeeID)
        {
            bool isDeleted = false;
            SqlTransaction trans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                string select = "";

                // Delete form security_routes_points
                select = "DELETE FROM security_routes_employees WHERE employee_id = '" + employeeID.ToString().Trim() + "' ";
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

        public ArrayList getEmployeesByWU(string wUnits)
        {
            DataSet dataSet = new DataSet();
            SecurityRoutesEmployeeTO routeEmployee = new SecurityRoutesEmployeeTO();
            ArrayList routeEmployeesList = new ArrayList();
            string select;

            try
            {
                select = "SELECT re.*, e.first_name, e.last_name FROM employees e, security_routes_employees re "
                    + "WHERE e.employee_id = re.employee_id ";

                if (!wUnits.Equals(""))
                {
                    select += "AND e.working_unit_id IN (" + wUnits.Trim() + ")";
                }

                SqlCommand cmd = new SqlCommand(select + " ORDER BY e.last_name, e.first_name", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "RouteEmployee");
                DataTable table = dataSet.Tables["RouteEmployee"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        routeEmployee = new SecurityRoutesEmployeeTO();
                        routeEmployee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["last_name"] != DBNull.Value)
                        {
                            routeEmployee.EmployeeName = row["last_name"].ToString().Trim();
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            routeEmployee.EmployeeName += " " + row["first_name"].ToString().Trim();
                        }

                        routeEmployeesList.Add(routeEmployee);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return routeEmployeesList;
        }

        public ArrayList getEmployees()
        {
            DataSet dataSet = new DataSet();
            SecurityRoutesEmployeeTO routeEmployee = new SecurityRoutesEmployeeTO();
            ArrayList routeEmployeesList = new ArrayList();
            string select;

            try
            {
                select = "SELECT re.*, e.first_name, e.last_name, wu.name wu_name FROM employees e, security_routes_employees re, working_units wu "
                    + "WHERE e.employee_id = re.employee_id AND e.working_unit_id = wu.working_unit_id ";

                SqlCommand cmd = new SqlCommand(select + "ORDER BY e.last_name, e.first_name", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "RouteEmployee");
                DataTable table = dataSet.Tables["RouteEmployee"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        routeEmployee = new SecurityRoutesEmployeeTO();
                        routeEmployee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["last_name"] != DBNull.Value)
                        {
                            routeEmployee.EmployeeName = row["last_name"].ToString().Trim();
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            routeEmployee.EmployeeName += " " + row["first_name"].ToString().Trim();
                        }
                        if (row["wu_name"] != DBNull.Value)
                        {
                            routeEmployee.WUName += " " + row["wu_name"].ToString().Trim();
                        }
                        routeEmployeesList.Add(routeEmployee);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return routeEmployeesList;
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
