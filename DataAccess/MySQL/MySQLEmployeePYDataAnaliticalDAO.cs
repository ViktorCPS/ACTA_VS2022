using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Data;
using TransferObjects;

namespace DataAccess
{
    public class MySQLEmployeePYDataAnaliticalDAO : EmployeePYDataAnaliticalDAO
    {
        MySqlConnection conn = null;
        MySqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MySQLEmployeePYDataAnaliticalDAO()
        {
            conn = MySQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
            DAOController.GetInstance();
        }

        public MySQLEmployeePYDataAnaliticalDAO(MySqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
            DAOController.GetInstance();
        }

        public void SetDBConnection(Object dbConnection)
        {
            conn = dbConnection as MySqlConnection;
        }

        public int insert(EmployeePYDataAnaliticalTO emplPYDataAnalitical, bool doCommit)
        {
            MySqlTransaction sqlTrans = null;

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
                sbInsert.Append("INSERT INTO employee_py_data_analitical ");
                sbInsert.Append("(py_calc_id, company_code, type, employee_id, employee_type_name, cost_center_name, cost_center_desc, date, payment_code, date_start_sickness, hrs_amount,  created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append(emplPYDataAnalitical.PYCalcID + ", ");
                sbInsert.Append("'" + emplPYDataAnalitical.CompanyCode + "', ");
                sbInsert.Append("'" + emplPYDataAnalitical.Type + "', ");
                sbInsert.Append(emplPYDataAnalitical.EmployeeID + ", ");
                sbInsert.Append("'" + emplPYDataAnalitical.EmployeeType.Trim() + "', ");
                sbInsert.Append("'" + emplPYDataAnalitical.CCName.Trim() + "', ");
                sbInsert.Append("'" + emplPYDataAnalitical.CCDesc.Trim() + "', ");
                sbInsert.Append("'" + emplPYDataAnalitical.Date.ToString(dateTimeformat) + "', ");
                sbInsert.Append("'" + emplPYDataAnalitical.PaymentCode + "', ");
                if (emplPYDataAnalitical.DateStartSickness != new DateTime())
                    sbInsert.Append("'" + emplPYDataAnalitical.DateStartSickness.ToString(dateTimeformat) + "', ");
                else
                    sbInsert.Append("NULL, ");
                sbInsert.Append("'" + emplPYDataAnalitical.HrsAmount.ToString().Trim().Replace(',', '.') + "', ");
                sbInsert.Append(" NOW()) ");

                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit)
                {
                    sqlTrans.Commit();
                }
                rowsAffected = 1;
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
        public List<EmployeePYDataAnaliticalTO> getEmployeesAnalitical(string employees, string payment_code, uint py_calc_id, string type)
        {
            DataSet dataSet = new DataSet();
            EmployeePYDataAnaliticalTO employee = new EmployeePYDataAnaliticalTO();
            List<EmployeePYDataAnaliticalTO> employeesList = new List<EmployeePYDataAnaliticalTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employee_py_data_analitical");
                if (!employees.Equals("") || !payment_code.Equals("") || py_calc_id > 0 || !type.Equals(""))
                {
                    sb.Append(" WHERE");

                    if (py_calc_id > 0)
                    {
                        //sb.Append(" UPPER(employee_id) LIKE '" + EmployeeID.Trim().ToUpper() + "' AND");
                        sb.Append(" py_calc_id = '" + py_calc_id + "' AND");
                    }
                    if (!employees.Equals(""))
                    {
                        sb.Append(" employee_id in ('" + employees + "') AND");
                    }
                    if (!payment_code.Equals(""))
                    {
                        sb.Append(" payment_code in ( '" + payment_code + "') AND");
                    }

                    if (!type.Equals(""))
                    {
                        sb.Append(" type ='" + type + "' AND");
                    }
                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }


                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        employee = new EmployeePYDataAnaliticalTO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["employee_type_name"] != DBNull.Value)
                        {
                            employee.EmployeeType = row["employee_type_name"].ToString().Trim();
                        }
                        if (row["cost_center_name"] != DBNull.Value)
                        {
                            employee.CCName = row["cost_center_name"].ToString().Trim();
                        }
                        if (row["cost_center_desc"] != DBNull.Value)
                        {
                            employee.CCDesc = row["cost_center_desc"].ToString().Trim();
                        }
                        if (row["date"] != DBNull.Value)
                        {
                            employee.Date = DateTime.Parse(row["date"].ToString().Trim());
                        }

                        if (row["date_start_sickness"] != DBNull.Value)
                        {
                            employee.DateStartSickness = DateTime.Parse(row["date_start_sickness"].ToString().Trim());
                        }
                        if (row["hrs_amount"] != DBNull.Value)
                        {
                            employee.HrsAmount = Decimal.Parse(row["hrs_amount"].ToString().Trim());
                        }
                        if (row["company_code"] != DBNull.Value)
                        {
                            employee.CompanyCode = row["company_code"].ToString().Trim();
                        }
                        if (row["payment_code"] != DBNull.Value)
                        {
                            employee.PaymentCode = row["payment_code"].ToString().Trim();
                        }
                        if (row["type"] != DBNull.Value)
                        {
                            employee.Type = row["type"].ToString().Trim();
                        }
                        if (row["py_calc_id"] != DBNull.Value)
                        {
                            employee.PYCalcID = UInt32.Parse(row["py_calc_id"].ToString().Trim());
                        }

                        employeesList.Add(employee);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return employeesList;
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
