using System;
using System.Collections;
using System.Text;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public class MySQLSecurityRoutesEmployeeDAO : SecurityRoutesEmployeeDAO
    {
        MySqlConnection conn = null;
		protected string dateTimeformat = "";
        MySqlTransaction _sqlTrans = null;

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MySQLSecurityRoutesEmployeeDAO()
		{
			conn = MySQLDAOFactory.getConnection();	
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MySQLSecurityRoutesEmployeeDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public int insert(string employeeID)
        {
            int rowsAffected = 0;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbInsert = new StringBuilder();

                sbInsert.Append("INSERT INTO security_routes_employees (employee_id, created_by, created_time) ");
                sbInsert.Append("VALUES ('" + employeeID.ToString().Trim() + "', N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");

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

        public bool delete(string employeeID)
        {
            bool isDeleted = false;
            MySqlTransaction trans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                string select = "";

                // Delete form security_routes_points
                select = "DELETE FROM security_routes_employees WHERE employee_id = '" + employeeID.ToString().Trim() + "' ";
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

                MySqlCommand cmd = new MySqlCommand(select + " ORDER BY e.last_name, e.first_name", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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

                MySqlCommand cmd = new MySqlCommand(select + "ORDER BY e.last_name, e.first_name", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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
            _sqlTrans = (MySqlTransaction)trans;
        }
    }
}
