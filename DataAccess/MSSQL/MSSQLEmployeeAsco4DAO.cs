using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TransferObjects;
using Util;

namespace DataAccess
{
    public class MSSQLEmployeeAsco4DAO : EmployeeAsco4DAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLEmployeeAsco4DAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            DAOController.GetInstance();
        }

        public MSSQLEmployeeAsco4DAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
        }

        public List<EmployeeAsco4TO> getEmployeesAsco(EmployeeAsco4TO emplTO)
        {
            DataSet dataSet = new DataSet();
            EmployeeAsco4TO employee = new EmployeeAsco4TO();
            List<EmployeeAsco4TO> employeesList = new List<EmployeeAsco4TO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employees_asco4");
                if ((emplTO.EmployeeID != -1)
                     || (emplTO.IntegerValue1 != -1) || (emplTO.IntegerValue2 != -1) || (emplTO.IntegerValue3 != -1) || (emplTO.IntegerValue4 != -1) || (emplTO.IntegerValue5 != -1)
                     || (emplTO.IntegerValue6 != -1) || (emplTO.IntegerValue7 != -1) || (emplTO.IntegerValue8 != -1) || (emplTO.IntegerValue9 != -1) || (emplTO.IntegerValue10 != -1)
                     || (!emplTO.DatetimeValue1.Equals(new DateTime())) || (!emplTO.DatetimeValue2.Equals(new DateTime())) || (!emplTO.DatetimeValue3.Equals(new DateTime()))
                     || (!emplTO.DatetimeValue4.Equals(new DateTime())) || (!emplTO.DatetimeValue5.Equals(new DateTime())) || (!emplTO.DatetimeValue6.Equals(new DateTime()))
                     || (!emplTO.DatetimeValue7.Equals(new DateTime())) || (!emplTO.DatetimeValue8.Equals(new DateTime())) || (!emplTO.DatetimeValue9.Equals(new DateTime())) || (!emplTO.DatetimeValue10.Equals(new DateTime()))
                     || (!emplTO.NVarcharValue1.Trim().Equals("")) || (!emplTO.NVarcharValue2.Trim().Equals("")) || (!emplTO.NVarcharValue3.Trim().Equals("")) || (!emplTO.NVarcharValue4.Trim().Equals(""))
                     || (!emplTO.NVarcharValue5.Trim().Equals("")) || (!emplTO.NVarcharValue6.Trim().Equals("")) || (!emplTO.NVarcharValue7.Trim().Equals("")) || (!emplTO.NVarcharValue8.Trim().Equals(""))
                     || (!emplTO.NVarcharValue9.Trim().Equals("")) || (!emplTO.NVarcharValue10.Trim().Equals("")))
                {
                    sb.Append(" WHERE");

                    if (emplTO.EmployeeID != -1)
                    {
                        sb.Append(" employee_id = " + emplTO.EmployeeID.ToString().Trim() + " AND");
                    }
                    if (emplTO.IntegerValue1 != -1)
                    {
                        sb.Append(" integer_value_1 = " + emplTO.IntegerValue1.ToString().Trim() + " AND");
                    }
                    if (emplTO.IntegerValue2 != -1)
                    {
                        sb.Append(" integer_value_2 = " + emplTO.IntegerValue2.ToString().Trim() + " AND");
                    }
                    if (emplTO.IntegerValue3 != -1)
                    {
                        sb.Append(" integer_value_3 = " + emplTO.IntegerValue3.ToString().Trim() + " AND");
                    }
                    if (emplTO.IntegerValue4 != -1)
                    {
                        sb.Append(" integer_value_4 = " + emplTO.IntegerValue4.ToString().Trim() + " AND");
                    }
                    if (emplTO.IntegerValue5 != -1)
                    {
                        sb.Append(" integer_value_5 = " + emplTO.IntegerValue5.ToString().Trim() + " AND");
                    }
                    if (emplTO.IntegerValue6 != -1)
                    {
                        sb.Append(" integer_value_6 = " + emplTO.IntegerValue6.ToString().Trim() + " AND");
                    }
                    if (emplTO.IntegerValue7 != -1)
                    {
                        sb.Append(" integer_value_7 = " + emplTO.IntegerValue7.ToString().Trim() + " AND");
                    }
                    if (emplTO.IntegerValue8 != -1)
                    {
                        sb.Append(" integer_value_8 = " + emplTO.IntegerValue8.ToString().Trim() + " AND");
                    }
                    if (emplTO.IntegerValue9 != -1)
                    {
                        sb.Append(" integer_value_9 = " + emplTO.IntegerValue9.ToString().Trim() + " AND");
                    }
                    if (emplTO.IntegerValue10 != -1)
                    {
                        sb.Append(" integer_value_10 = " + emplTO.IntegerValue10.ToString().Trim() + " AND");
                    }
                    if (!emplTO.DatetimeValue1.Equals(new DateTime()))
                    {
                        sb.Append(" datetime_value_1 >= CONVERT(datetime,'" + emplTO.DatetimeValue1.ToString("yyyy-MM-dd") + "', 111) AND ");
                    }
                    if (!emplTO.DatetimeValue2.Equals(new DateTime()))
                    {
                        sb.Append(" datetime_value_2 >= CONVERT(datetime,'" + emplTO.DatetimeValue2.ToString("yyyy-MM-dd") + "', 111) AND ");
                    }
                    if (!emplTO.DatetimeValue3.Equals(new DateTime()))
                    {
                        sb.Append(" datetime_value_3 >= CONVERT(datetime,'" + emplTO.DatetimeValue3.ToString("yyyy-MM-dd") + "', 111) AND ");
                    }
                    if (!emplTO.DatetimeValue4.Equals(new DateTime()))
                    {
                        sb.Append(" datetime_value_4 >= CONVERT(datetime,'" + emplTO.DatetimeValue4.ToString("yyyy-MM-dd") + "', 111) AND ");
                    }
                    if (!emplTO.DatetimeValue5.Equals(new DateTime()))
                    {
                        sb.Append(" datetime_value_5 >= CONVERT(datetime,'" + emplTO.DatetimeValue5.ToString("yyyy-MM-dd") + "', 111) AND ");
                    }
                    if (!emplTO.DatetimeValue6.Equals(new DateTime()))
                    {
                        sb.Append(" datetime_value_6 >= CONVERT(datetime,'" + emplTO.DatetimeValue6.ToString("yyyy-MM-dd") + "', 111) AND ");
                    }
                    if (!emplTO.DatetimeValue7.Equals(new DateTime()))
                    {
                        sb.Append(" datetime_value_7 >= CONVERT(datetime,'" + emplTO.DatetimeValue7.ToString("yyyy-MM-dd") + "', 111) AND ");
                    }
                    if (!emplTO.DatetimeValue8.Equals(new DateTime()))
                    {
                        sb.Append(" datetime_value_8 >= CONVERT(datetime,'" + emplTO.DatetimeValue8.ToString("yyyy-MM-dd") + "', 111) AND ");
                    }
                    if (!emplTO.DatetimeValue9.Equals(new DateTime()))
                    {
                        sb.Append(" datetime_value_9 >= CONVERT(datetime,'" + emplTO.DatetimeValue9.ToString("yyyy-MM-dd") + "', 111) AND ");
                    }
                    if (!emplTO.DatetimeValue10.Equals(new DateTime()))
                    {
                        sb.Append(" datetime_value_10 >= CONVERT(datetime,'" + emplTO.DatetimeValue10.ToString("yyyy-MM-dd") + "', 111) AND ");
                    }
                    if (!emplTO.NVarcharValue1.Trim().Equals(""))
                    {
                        sb.Append(" nvarchar_value_1 = N'" + emplTO.NVarcharValue1.Trim() + "' AND");
                    }
                    if (!emplTO.NVarcharValue2.Trim().Equals(""))
                    {
                        sb.Append(" nvarchar_value_2 = N'" + emplTO.NVarcharValue2.Trim() + "' AND");
                    }
                    if (!emplTO.NVarcharValue3.Trim().Equals(""))
                    {
                        sb.Append(" nvarchar_value_3 = N'" + emplTO.NVarcharValue3.Trim() + "' AND");
                    }
                    if (!emplTO.NVarcharValue4.Trim().Equals(""))
                    {
                        sb.Append(" nvarchar_value_4 = N'" + emplTO.NVarcharValue4.Trim() + "' AND");
                    }
                    if (!emplTO.NVarcharValue5.Trim().Equals(""))
                    {
                        sb.Append(" nvarchar_value_5 = N'" + emplTO.NVarcharValue5.Trim() + "' AND");
                    }
                    if (!emplTO.NVarcharValue6.Trim().Equals(""))
                    {
                        sb.Append(" nvarchar_value_6 = N'" + emplTO.NVarcharValue6.Trim() + "' AND");
                    }
                    if (!emplTO.NVarcharValue7.Trim().Equals(""))
                    {
                        sb.Append(" nvarchar_value_7 = N'" + emplTO.NVarcharValue7.Trim() + "' AND");
                    }
                    if (!emplTO.NVarcharValue8.Trim().Equals(""))
                    {
                        sb.Append(" nvarchar_value_8 = N'" + emplTO.NVarcharValue8.Trim() + "' AND");
                    }
                    if (!emplTO.NVarcharValue9.Trim().Equals(""))
                    {
                        sb.Append(" nvarchar_value_9 = N'" + emplTO.NVarcharValue9.Trim() + "' AND");
                    }
                    if (!emplTO.NVarcharValue10.Trim().Equals(""))
                    {
                        sb.Append(" nvarchar_value_10 = N'" + emplTO.NVarcharValue10.Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select += " ORDER BY employee_id ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        employee = new EmployeeAsco4TO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["integer_value_1"] != DBNull.Value)
                        {
                            employee.IntegerValue1 = Int32.Parse(row["integer_value_1"].ToString().Trim());
                        }
                        if (row["integer_value_2"] != DBNull.Value)
                        {
                            employee.IntegerValue2 = Int32.Parse(row["integer_value_2"].ToString().Trim());
                        }
                        if (row["integer_value_3"] != DBNull.Value)
                        {
                            employee.IntegerValue3 = Int32.Parse(row["integer_value_3"].ToString().Trim());
                        }
                        if (row["integer_value_4"] != DBNull.Value)
                        {
                            employee.IntegerValue4 = Int32.Parse(row["integer_value_4"].ToString().Trim());
                        }
                        if (row["integer_value_5"] != DBNull.Value)
                        {
                            employee.IntegerValue5 = Int32.Parse(row["integer_value_5"].ToString().Trim());
                        }
                        if (row["integer_value_6"] != DBNull.Value)
                        {
                            employee.IntegerValue6 = Int32.Parse(row["integer_value_6"].ToString().Trim());
                        }
                        if (row["integer_value_7"] != DBNull.Value)
                        {
                            employee.IntegerValue7 = Int32.Parse(row["integer_value_7"].ToString().Trim());
                        }
                        if (row["integer_value_8"] != DBNull.Value)
                        {
                            employee.IntegerValue8 = Int32.Parse(row["integer_value_8"].ToString().Trim());
                        }
                        if (row["integer_value_9"] != DBNull.Value)
                        {
                            employee.IntegerValue9 = Int32.Parse(row["integer_value_9"].ToString().Trim());
                        }
                        if (row["integer_value_10"] != DBNull.Value)
                        {
                            employee.IntegerValue10 = Int32.Parse(row["integer_value_10"].ToString().Trim());
                        }
                        if (row["datetime_value_1"] != DBNull.Value)
                        {
                            employee.DatetimeValue1 = DateTime.Parse(row["datetime_value_1"].ToString().Trim());
                        }
                        if (row["datetime_value_2"] != DBNull.Value)
                        {
                            employee.DatetimeValue2 = DateTime.Parse(row["datetime_value_2"].ToString().Trim());
                        }
                        if (row["datetime_value_3"] != DBNull.Value)
                        {
                            employee.DatetimeValue3 = DateTime.Parse(row["datetime_value_3"].ToString().Trim());
                        }
                        if (row["datetime_value_4"] != DBNull.Value)
                        {
                            employee.DatetimeValue4 = DateTime.Parse(row["datetime_value_4"].ToString().Trim());
                        }
                        if (row["datetime_value_5"] != DBNull.Value)
                        {
                            employee.DatetimeValue5 = DateTime.Parse(row["datetime_value_5"].ToString().Trim());
                        }
                        if (row["datetime_value_6"] != DBNull.Value)
                        {
                            employee.DatetimeValue6 = DateTime.Parse(row["datetime_value_6"].ToString().Trim());
                        }
                        if (row["datetime_value_7"] != DBNull.Value)
                        {
                            employee.DatetimeValue7 = DateTime.Parse(row["datetime_value_7"].ToString().Trim());
                        }
                        if (row["datetime_value_8"] != DBNull.Value)
                        {
                            employee.DatetimeValue8 = DateTime.Parse(row["datetime_value_8"].ToString().Trim());
                        }
                        if (row["datetime_value_9"] != DBNull.Value)
                        {
                            employee.DatetimeValue9 = DateTime.Parse(row["datetime_value_9"].ToString().Trim());
                        }
                        if (row["datetime_value_10"] != DBNull.Value)
                        {
                            employee.DatetimeValue10 = DateTime.Parse(row["datetime_value_10"].ToString().Trim());
                        }
                        if (row["nvarchar_value_1"] != DBNull.Value)
                        {
                            employee.NVarcharValue1 = row["nvarchar_value_1"].ToString().Trim();
                        }
                        if (row["nvarchar_value_2"] != DBNull.Value)
                        {
                            employee.NVarcharValue2 = row["nvarchar_value_2"].ToString().Trim();
                        }
                        if (row["nvarchar_value_3"] != DBNull.Value)
                        {
                            employee.NVarcharValue3 = row["nvarchar_value_3"].ToString().Trim();
                        }
                        if (row["nvarchar_value_4"] != DBNull.Value)
                        {
                            employee.NVarcharValue4 = row["nvarchar_value_4"].ToString().Trim();
                        }
                        if (row["nvarchar_value_5"] != DBNull.Value)
                        {
                            employee.NVarcharValue5 = row["nvarchar_value_5"].ToString().Trim();
                        }
                        if (row["nvarchar_value_6"] != DBNull.Value)
                        {
                            employee.NVarcharValue6 = row["nvarchar_value_6"].ToString().Trim();
                        }
                        if (row["nvarchar_value_7"] != DBNull.Value)
                        {
                            employee.NVarcharValue7 = row["nvarchar_value_7"].ToString().Trim();
                        }
                        if (row["nvarchar_value_8"] != DBNull.Value)
                        {
                            employee.NVarcharValue8 = row["nvarchar_value_8"].ToString().Trim();
                        }
                        if (row["nvarchar_value_9"] != DBNull.Value)
                        {
                            employee.NVarcharValue9 = row["nvarchar_value_9"].ToString().Trim();
                        }
                        if (row["nvarchar_value_10"] != DBNull.Value)
                        {
                            employee.NVarcharValue10 = row["nvarchar_value_10"].ToString().Trim();
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            employee.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            employee.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
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

        public List<EmployeeAsco4TO> getEmployeesAsco(string emplIDs)
        {
            DataSet dataSet = new DataSet();
            EmployeeAsco4TO employee = new EmployeeAsco4TO();
            List<EmployeeAsco4TO> employeesList = new List<EmployeeAsco4TO>();
            string select = "";

            try
            {
                if (emplIDs.Trim().Length <= 0)
                    return employeesList;

                select = "SELECT * FROM employees_asco4 WHERE employee_id IN (" + emplIDs.Trim() + ") ORDER BY employee_id";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        employee = new EmployeeAsco4TO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["integer_value_1"] != DBNull.Value)
                        {
                            employee.IntegerValue1 = Int32.Parse(row["integer_value_1"].ToString().Trim());
                        }
                        if (row["integer_value_2"] != DBNull.Value)
                        {
                            employee.IntegerValue2 = Int32.Parse(row["integer_value_2"].ToString().Trim());
                        }
                        if (row["integer_value_3"] != DBNull.Value)
                        {
                            employee.IntegerValue3 = Int32.Parse(row["integer_value_3"].ToString().Trim());
                        }
                        if (row["integer_value_4"] != DBNull.Value)
                        {
                            employee.IntegerValue4 = Int32.Parse(row["integer_value_4"].ToString().Trim());
                        }
                        if (row["integer_value_5"] != DBNull.Value)
                        {
                            employee.IntegerValue5 = Int32.Parse(row["integer_value_5"].ToString().Trim());
                        }
                        if (row["integer_value_6"] != DBNull.Value)
                        {
                            employee.IntegerValue6 = Int32.Parse(row["integer_value_6"].ToString().Trim());
                        }
                        if (row["integer_value_7"] != DBNull.Value)
                        {
                            employee.IntegerValue7 = Int32.Parse(row["integer_value_7"].ToString().Trim());
                        }
                        if (row["integer_value_8"] != DBNull.Value)
                        {
                            employee.IntegerValue8 = Int32.Parse(row["integer_value_8"].ToString().Trim());
                        }
                        if (row["integer_value_9"] != DBNull.Value)
                        {
                            employee.IntegerValue9 = Int32.Parse(row["integer_value_9"].ToString().Trim());
                        }
                        if (row["integer_value_10"] != DBNull.Value)
                        {
                            employee.IntegerValue10 = Int32.Parse(row["integer_value_10"].ToString().Trim());
                        }
                        if (row["datetime_value_1"] != DBNull.Value)
                        {
                            employee.DatetimeValue1 = DateTime.Parse(row["datetime_value_1"].ToString().Trim());
                        }
                        if (row["datetime_value_2"] != DBNull.Value)
                        {
                            employee.DatetimeValue2 = DateTime.Parse(row["datetime_value_2"].ToString().Trim());
                        }
                        if (row["datetime_value_3"] != DBNull.Value)
                        {
                            employee.DatetimeValue3 = DateTime.Parse(row["datetime_value_3"].ToString().Trim());
                        }
                        if (row["datetime_value_4"] != DBNull.Value)
                        {
                            employee.DatetimeValue4 = DateTime.Parse(row["datetime_value_4"].ToString().Trim());
                        }
                        if (row["datetime_value_5"] != DBNull.Value)
                        {
                            employee.DatetimeValue5 = DateTime.Parse(row["datetime_value_5"].ToString().Trim());
                        }
                        if (row["datetime_value_6"] != DBNull.Value)
                        {
                            employee.DatetimeValue6 = DateTime.Parse(row["datetime_value_6"].ToString().Trim());
                        }
                        if (row["datetime_value_7"] != DBNull.Value)
                        {
                            employee.DatetimeValue7 = DateTime.Parse(row["datetime_value_7"].ToString().Trim());
                        }
                        if (row["datetime_value_8"] != DBNull.Value)
                        {
                            employee.DatetimeValue8 = DateTime.Parse(row["datetime_value_8"].ToString().Trim());
                        }
                        if (row["datetime_value_9"] != DBNull.Value)
                        {
                            employee.DatetimeValue9 = DateTime.Parse(row["datetime_value_9"].ToString().Trim());
                        }
                        if (row["datetime_value_10"] != DBNull.Value)
                        {
                            employee.DatetimeValue10 = DateTime.Parse(row["datetime_value_10"].ToString().Trim());
                        }
                        if (row["nvarchar_value_1"] != DBNull.Value)
                        {
                            employee.NVarcharValue1 = row["nvarchar_value_1"].ToString().Trim();
                        }
                        if (row["nvarchar_value_2"] != DBNull.Value)
                        {
                            employee.NVarcharValue2 = row["nvarchar_value_2"].ToString().Trim();
                        }
                        if (row["nvarchar_value_3"] != DBNull.Value)
                        {
                            employee.NVarcharValue3 = row["nvarchar_value_3"].ToString().Trim();
                        }
                        if (row["nvarchar_value_4"] != DBNull.Value)
                        {
                            employee.NVarcharValue4 = row["nvarchar_value_4"].ToString().Trim();
                        }
                        if (row["nvarchar_value_5"] != DBNull.Value)
                        {
                            employee.NVarcharValue5 = row["nvarchar_value_5"].ToString().Trim();
                        }
                        if (row["nvarchar_value_6"] != DBNull.Value)
                        {
                            employee.NVarcharValue6 = row["nvarchar_value_6"].ToString().Trim();
                        }
                        if (row["nvarchar_value_7"] != DBNull.Value)
                        {
                            employee.NVarcharValue7 = row["nvarchar_value_7"].ToString().Trim();
                        }
                        if (row["nvarchar_value_8"] != DBNull.Value)
                        {
                            employee.NVarcharValue8 = row["nvarchar_value_8"].ToString().Trim();
                        }
                        if (row["nvarchar_value_9"] != DBNull.Value)
                        {
                            employee.NVarcharValue9 = row["nvarchar_value_9"].ToString().Trim();
                        }
                        if (row["nvarchar_value_10"] != DBNull.Value)
                        {
                            employee.NVarcharValue10 = row["nvarchar_value_10"].ToString().Trim();
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

        public Dictionary<string, EmployeeTO> getICodeData()
        {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            string iCodeTag = "";
            Dictionary<string, EmployeeTO> dict = new Dictionary<string, EmployeeTO>();
            string select = "";

            try
            {
                select = "SELECT a.nvarchar_value_10, e.employee_id, e.first_name, e.last_name FROM employees_asco4 a, employees e WHERE a.employee_id = e.employee_id AND e.status = '" 
                    + Constants.statusActive.Trim().ToUpper() + "'";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        employee = new EmployeeTO();
                        if (row["employee_id"] != DBNull.Value)
                        {
                            employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            employee.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            employee.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["nvarchar_value_10"] != DBNull.Value)
                        {
                            iCodeTag = row["nvarchar_value_10"].ToString().Trim();
                        }
                        else
                            iCodeTag = "";

                        if (iCodeTag.Trim() != "" && !dict.ContainsKey(iCodeTag))
                            dict.Add(iCodeTag, employee);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return dict;
        }

        public Dictionary<int, EmployeeAsco4TO> getEmployeesAscoDictionary(string emplIDs)
        {
            DataSet dataSet = new DataSet();
            EmployeeAsco4TO employee = new EmployeeAsco4TO();
            Dictionary<int, EmployeeAsco4TO> employeesList = new Dictionary<int,EmployeeAsco4TO>();
            string select = "";

            try
            {
                select = "SELECT * FROM employees_asco4 ";

                if (emplIDs.Length > 0)
                    select += "WHERE employee_id IN (" + emplIDs.Trim() + ") ";

                select += "ORDER BY employee_id";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        employee = new EmployeeAsco4TO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["integer_value_1"] != DBNull.Value)
                        {
                            employee.IntegerValue1 = Int32.Parse(row["integer_value_1"].ToString().Trim());
                        }
                        if (row["integer_value_2"] != DBNull.Value)
                        {
                            employee.IntegerValue2 = Int32.Parse(row["integer_value_2"].ToString().Trim());
                        }
                        if (row["integer_value_3"] != DBNull.Value)
                        {
                            employee.IntegerValue3 = Int32.Parse(row["integer_value_3"].ToString().Trim());
                        }
                        if (row["integer_value_4"] != DBNull.Value)
                        {
                            employee.IntegerValue4 = Int32.Parse(row["integer_value_4"].ToString().Trim());
                        }
                        if (row["integer_value_5"] != DBNull.Value)
                        {
                            employee.IntegerValue5 = Int32.Parse(row["integer_value_5"].ToString().Trim());
                        }
                        if (row["integer_value_6"] != DBNull.Value)
                        {
                            employee.IntegerValue6 = Int32.Parse(row["integer_value_6"].ToString().Trim());
                        }
                        if (row["integer_value_7"] != DBNull.Value)
                        {
                            employee.IntegerValue7 = Int32.Parse(row["integer_value_7"].ToString().Trim());
                        }
                        if (row["integer_value_8"] != DBNull.Value)
                        {
                            employee.IntegerValue8 = Int32.Parse(row["integer_value_8"].ToString().Trim());
                        }
                        if (row["integer_value_9"] != DBNull.Value)
                        {
                            employee.IntegerValue9 = Int32.Parse(row["integer_value_9"].ToString().Trim());
                        }
                        if (row["integer_value_10"] != DBNull.Value)
                        {
                            employee.IntegerValue10 = Int32.Parse(row["integer_value_10"].ToString().Trim());
                        }
                        if (row["datetime_value_1"] != DBNull.Value)
                        {
                            employee.DatetimeValue1 = DateTime.Parse(row["datetime_value_1"].ToString().Trim());
                        }
                        if (row["datetime_value_2"] != DBNull.Value)
                        {
                            employee.DatetimeValue2 = DateTime.Parse(row["datetime_value_2"].ToString().Trim());
                        }
                        if (row["datetime_value_3"] != DBNull.Value)
                        {
                            employee.DatetimeValue3 = DateTime.Parse(row["datetime_value_3"].ToString().Trim());
                        }
                        if (row["datetime_value_4"] != DBNull.Value)
                        {
                            employee.DatetimeValue4 = DateTime.Parse(row["datetime_value_4"].ToString().Trim());
                        }
                        if (row["datetime_value_5"] != DBNull.Value)
                        {
                            employee.DatetimeValue5 = DateTime.Parse(row["datetime_value_5"].ToString().Trim());
                        }
                        if (row["datetime_value_6"] != DBNull.Value)
                        {
                            employee.DatetimeValue6 = DateTime.Parse(row["datetime_value_6"].ToString().Trim());
                        }
                        if (row["datetime_value_7"] != DBNull.Value)
                        {
                            employee.DatetimeValue7 = DateTime.Parse(row["datetime_value_7"].ToString().Trim());
                        }
                        if (row["datetime_value_8"] != DBNull.Value)
                        {
                            employee.DatetimeValue8 = DateTime.Parse(row["datetime_value_8"].ToString().Trim());
                        }
                        if (row["datetime_value_9"] != DBNull.Value)
                        {
                            employee.DatetimeValue9 = DateTime.Parse(row["datetime_value_9"].ToString().Trim());
                        }
                        if (row["datetime_value_10"] != DBNull.Value)
                        {
                            employee.DatetimeValue10 = DateTime.Parse(row["datetime_value_10"].ToString().Trim());
                        }
                        if (row["nvarchar_value_1"] != DBNull.Value)
                        {
                            employee.NVarcharValue1 = row["nvarchar_value_1"].ToString().Trim();
                        }
                        if (row["nvarchar_value_2"] != DBNull.Value)
                        {
                            employee.NVarcharValue2 = row["nvarchar_value_2"].ToString().Trim();
                        }
                        if (row["nvarchar_value_3"] != DBNull.Value)
                        {
                            employee.NVarcharValue3 = row["nvarchar_value_3"].ToString().Trim();
                        }
                        if (row["nvarchar_value_4"] != DBNull.Value)
                        {
                            employee.NVarcharValue4 = row["nvarchar_value_4"].ToString().Trim();
                        }
                        if (row["nvarchar_value_5"] != DBNull.Value)
                        {
                            employee.NVarcharValue5 = row["nvarchar_value_5"].ToString().Trim();
                        }
                        if (row["nvarchar_value_6"] != DBNull.Value)
                        {
                            employee.NVarcharValue6 = row["nvarchar_value_6"].ToString().Trim();
                        }
                        if (row["nvarchar_value_7"] != DBNull.Value)
                        {
                            employee.NVarcharValue7 = row["nvarchar_value_7"].ToString().Trim();
                        }
                        if (row["nvarchar_value_8"] != DBNull.Value)
                        {
                            employee.NVarcharValue8 = row["nvarchar_value_8"].ToString().Trim();
                        }
                        if (row["nvarchar_value_9"] != DBNull.Value)
                        {
                            employee.NVarcharValue9 = row["nvarchar_value_9"].ToString().Trim();
                        }
                        if (row["nvarchar_value_10"] != DBNull.Value)
                        {
                            employee.NVarcharValue10 = row["nvarchar_value_10"].ToString().Trim();
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            employee.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            employee.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }

                        if (employeesList.ContainsKey(employee.EmployeeID))
                            employeesList[employee.EmployeeID] = employee;
                        else
                            employeesList.Add(employee.EmployeeID, employee);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return employeesList;
        }

        public bool insert(int emplID, int int1, int int2, int int3, int int4, int int5, int int6, int int7, int int8, int int9, int int10,
            DateTime dateTime1, DateTime dateTime2, DateTime dateTime3, DateTime dateTime4, DateTime dateTime5,
            DateTime dateTime6, DateTime dateTime7, DateTime dateTime8, DateTime dateTime9, DateTime dateTime10,
            string string1, string string2, string string3, string string4, string string5
            , string string6, string string7, string string8, string string9, string string10)
        {
            bool isInserted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employees_asco4 ");
                sbInsert.Append("(employee_id,integer_value_1, integer_value_2, integer_value_3, integer_value_4, integer_value_5,integer_value_6, integer_value_7, integer_value_8, integer_value_9, integer_value_10,");
                sbInsert.Append("datetime_value_1,datetime_value_2,datetime_value_3,datetime_value_4,datetime_value_5,datetime_value_6,datetime_value_7,datetime_value_8,datetime_value_9,datetime_value_10,");
                sbInsert.Append("nvarchar_value_1,nvarchar_value_2,nvarchar_value_3,nvarchar_value_4,nvarchar_value_5,nvarchar_value_6,nvarchar_value_7,nvarchar_value_8,nvarchar_value_9,nvarchar_value_10, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append(emplID + ", ");
                if (int1 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int1 + ", ");
                }
                if (int2 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int2 + ", ");
                }
                if (int3 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int3 + ", ");
                }
                if (int4 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int4 + ", ");
                }
                if (int5 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int5 + ", ");
                }
                if (int6 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int6 + ", ");
                }
                if (int7 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int7 + ", ");
                }
                if (int8 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int8 + ", ");
                }
                if (int9 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int9 + ", ");
                }
                if (int10 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int10 + ", ");
                }
                if (dateTime1.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime1.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime2.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime2.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime3.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime3.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime4.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime4.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime5.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime5.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime6.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime6.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime7.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime7.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime8.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime8.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime9.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime9.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime10.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime10.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (string1.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string1 + "', ");
                }
                if (string2.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string2 + "', ");
                }
                if (string3.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string3 + "', ");
                }
                if (string4.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string4 + "', ");
                }
                if (string5.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string5 + "', ");
                }
                if (string6.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string6 + "', ");
                }
                if (string7.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string7 + "', ");
                }
                if (string8.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string8 + "', ");
                }
                if (string9.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string9 + "', ");
                }
                if (string10.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string10 + "', ");
                }
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()); ");


                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                int rowsAffected = cmd.ExecuteNonQuery();

                sqlTrans.Commit();

                if (rowsAffected > 0)
                    isInserted = true;
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("INSERT");

                throw ex;
            }

            return isInserted;
        }

        public bool insert(int emplID, int int1, int int2, int int3, int int4, int int5, int int6, int int7, int int8, int int9, int int10, DateTime dateTime1, DateTime dateTime2, DateTime dateTime3, DateTime dateTime4, DateTime dateTime5,
            DateTime dateTime6, DateTime dateTime7, DateTime dateTime8, DateTime dateTime9, DateTime dateTime10, string string1, string string2, string string3, string string4, string string5
            , string string6, string string7, string string8, string string9, string string10, bool doCommit)
        {
            bool isInserted = false;
            SqlTransaction sqlTrans = null;
            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }


            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employees_asco4 ");
                sbInsert.Append("(employee_id,integer_value_1, integer_value_2, integer_value_3, integer_value_4, integer_value_5,integer_value_6, integer_value_7, integer_value_8, integer_value_9, integer_value_10,");
                sbInsert.Append("datetime_value_1,datetime_value_2,datetime_value_3,datetime_value_4,datetime_value_5,datetime_value_6,datetime_value_7,datetime_value_8,datetime_value_9,datetime_value_10,");
                sbInsert.Append("nvarchar_value_1,nvarchar_value_2,nvarchar_value_3,nvarchar_value_4,nvarchar_value_5,nvarchar_value_6,nvarchar_value_7,nvarchar_value_8,nvarchar_value_9,nvarchar_value_10, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append(emplID + ", ");
                if (int1 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int1 + ", ");
                }
                if (int2 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int2 + ", ");
                }
                if (int3 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int3 + ", ");
                }
                if (int4 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int4 + ", ");
                }
                if (int5 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int5 + ", ");
                }
                if (int6 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int6 + ", ");
                }
                if (int7 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int7 + ", ");
                }
                if (int8 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int8 + ", ");
                }
                if (int9 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int9 + ", ");
                }
                if (int10 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int10 + ", ");
                }
                if (dateTime1.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime1.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime2.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime2.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime3.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime3.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime4.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime4.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime5.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime5.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime6.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime6.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime7.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime7.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime8.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime8.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime9.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime9.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime10.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime10.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (string1.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string1 + "', ");
                }
                if (string2.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string2 + "', ");
                }
                if (string3.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string3 + "', ");
                }
                if (string4.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string4 + "', ");
                }
                if (string5.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string5 + "', ");
                }
                if (string6.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string6 + "', ");
                }
                if (string7.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string7 + "', ");
                }
                if (string8.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string8 + "', ");
                }
                if (string9.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string9 + "', ");
                }
                if (string10.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string10 + "', ");
                }
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()); ");


                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (doCommit)
                {
                    sqlTrans.Commit();
                }
                if (rowsAffected > 0)
                    isInserted = true;
            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback("INSERT");
                }

                throw ex;
            }

            return isInserted;
        }

        public bool update(int employeeID, int int1, int int2, int int3, int int4, int int5, int int6, int int7, int int8, int int9, int int10, DateTime dateTime1, DateTime dateTime2, DateTime dateTime3, DateTime dateTime4, DateTime dateTime5,
           DateTime dateTime6, DateTime dateTime7, DateTime dateTime8, DateTime dateTime9, DateTime dateTime10, string string1, string string2, string string3, string string4, string string5
           , string string6, string string7, string string8, string string9, string string10, bool doCommit)
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

                sbUpdate.Append("UPDATE employees_asco4 SET ");
                if (int1 != -1)
                {
                    sbUpdate.Append("integer_value_1 = " + int1 + ", ");
                }
                else
                {
                    sbUpdate.Append("integer_value_1 = null, ");
                }
                if (int2 != -1)
                {
                    sbUpdate.Append("integer_value_2 = " + int2 + ", ");
                }
                else
                {
                    sbUpdate.Append("integer_value_2 = null, ");
                }
                if (int3 != -1)
                {
                    sbUpdate.Append("integer_value_3 = " + int3 + ", ");
                }
                else
                {
                    sbUpdate.Append("integer_value_3 = null, ");
                }
                if (int4 != -1)
                {
                    sbUpdate.Append("integer_value_4 = " + int4 + ", ");
                }
                else
                {
                    sbUpdate.Append("integer_value_4 = null, ");
                }
                if (int5 != -1)
                {
                    sbUpdate.Append("integer_value_5 = " + int5 + ", ");
                }
                else
                {
                    sbUpdate.Append("integer_value_5 = null, ");
                }
                if (int6 != -1)
                {
                    sbUpdate.Append("integer_value_6 = " + int6 + ", ");
                }
                else
                {
                    sbUpdate.Append("integer_value_6 = null, ");
                }
                if (int7 != -1)
                {
                    sbUpdate.Append("integer_value_7 = " + int7 + ", ");
                }
                else
                {
                    sbUpdate.Append("integer_value_7 = null, ");
                }
                if (int8 != -1)
                {
                    sbUpdate.Append("integer_value_8 = " + int8 + ", ");
                }
                else
                {
                    sbUpdate.Append("integer_value_8 = null, ");
                }
                if (int9 != -1)
                {
                    sbUpdate.Append("integer_value_9 = " + int9 + ", ");
                }
                else
                {
                    sbUpdate.Append("integer_value_9 = null, ");
                }
                if (int10 != -1)
                {
                    sbUpdate.Append("integer_value_10 = " + int10 + ", ");
                }
                else
                {
                    sbUpdate.Append("integer_value_10 = null, ");
                }
                if (dateTime1 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_1 = '" + dateTime1.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("datetime_value_1 = null, ");
                }
                if (dateTime2 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_2 = '" + dateTime2.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("datetime_value_2 = null, ");
                }
                if (dateTime3 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_3 = '" + dateTime3.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("datetime_value_3 = null, ");
                }
                if (dateTime4 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_4 = '" + dateTime4.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("datetime_value_4 = null, ");
                }
                if (dateTime5 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_5 = '" + dateTime5.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("datetime_value_5 = null, ");
                }
                if (dateTime6 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_6 = '" + dateTime6.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("datetime_value_6 = null, ");
                }
                if (dateTime7 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_7 = '" + dateTime7.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("datetime_value_7 = null, ");
                }
                if (dateTime8 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_8 = '" + dateTime8.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("datetime_value_8 = null, ");
                }
                if (dateTime9 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_9 = '" + dateTime9.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("datetime_value_9 = null, ");
                }
                if (dateTime10 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_10 = '" + dateTime10.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("datetime_value_10 = null, ");
                }
                if (!string1.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_1 = N'" + string1 + "', ");
                }
                else
                {
                    sbUpdate.Append("nvarchar_value_1 = null, ");
                }
                if (!string2.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_2 = N'" + string2 + "', ");
                }
                else
                {
                    sbUpdate.Append("nvarchar_value_2 = null, ");
                }
                if (!string3.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_3 = N'" + string3 + "', ");
                }
                else
                {
                    sbUpdate.Append("nvarchar_value_3 = null, ");
                }
                if (!string4.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_4 = N'" + string4 + "', ");
                }
                else
                {
                    sbUpdate.Append("nvarchar_value_4 = null, ");
                }
                if (!string5.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_5 = N'" + string5 + "', ");
                }
                else
                {
                    sbUpdate.Append("nvarchar_value_5 = null, ");
                }
                if (!string6.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_6 = N'" + string6 + "', ");
                }
                else
                {
                    sbUpdate.Append("nvarchar_value_6 = null, ");
                }
                if (!string7.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_7 = N'" + string7 + "', ");
                }
                else
                {
                    sbUpdate.Append("nvarchar_value_7 = null, ");
                }
                if (!string8.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_8 = N'" + string8 + "', ");
                }
                else
                {
                    sbUpdate.Append("nvarchar_value_8 = null, ");
                }
                if (!string9.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_9 = N'" + string9 + "', ");
                }
                else
                {
                    sbUpdate.Append("nvarchar_value_9 = null, ");
                }
                if (!string10.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_10 = N'" + string10 + "', ");
                }
                else
                {
                    sbUpdate.Append("nvarchar_value_10 = null, ");
                }


                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE employee_id = " + employeeID);

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
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
                {
                    sqlTrans.Rollback("UPDATE");
                }
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }
        public bool update(int employeeID, int int1, int int2, int int3, int int4, int int5, int int6, int int7, int int8, int int9, int int10, DateTime dateTime1, DateTime dateTime2, DateTime dateTime3, DateTime dateTime4, DateTime dateTime5,
           DateTime dateTime6, DateTime dateTime7, DateTime dateTime8, DateTime dateTime9, DateTime dateTime10, string string1, string string2, string string3, string string4, string string5
           , string string6, string string7, string string8, string string9, string string10)
        {
            bool isUpdated = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");


            try
            {
                StringBuilder sbUpdate = new StringBuilder();

                sbUpdate.Append("UPDATE employees_asco4 SET ");

                if (int1 != -1)
                {
                    sbUpdate.Append("integer_value_1 = " + int1 + ", ");
                }
                else
                {
                    sbUpdate.Append("integer_value_1 = null, ");
                }
                if (int2 != -1)
                {
                    sbUpdate.Append("integer_value_2 = " + int2 + ", ");
                }
                else
                {
                    sbUpdate.Append("integer_value_2 = null, ");
                }
                if (int3 != -1)
                {
                    sbUpdate.Append("integer_value_3 = " + int3 + ", ");
                }
                else
                {
                    sbUpdate.Append("integer_value_3 = null, ");
                }
                if (int4 != -1)
                {
                    sbUpdate.Append("integer_value_4 = " + int4 + ", ");
                }
                else
                {
                    sbUpdate.Append("integer_value_4 = null, ");
                }
                if (int5 != -1)
                {
                    sbUpdate.Append("integer_value_5 = " + int5 + ", ");
                }
                else
                {
                    sbUpdate.Append("integer_value_5 = null, ");
                }
                if (int6 != -1)
                {
                    sbUpdate.Append("integer_value_6 = " + int6 + ", ");
                }
                else
                {
                    sbUpdate.Append("integer_value_6 = null, ");
                }
                if (int7 != -1)
                {
                    sbUpdate.Append("integer_value_7 = " + int7 + ", ");
                }
                else
                {
                    sbUpdate.Append("integer_value_7 = null, ");
                }
                if (int8 != -1)
                {
                    sbUpdate.Append("integer_value_8 = " + int8 + ", ");
                }
                else
                {
                    sbUpdate.Append("integer_value_8 = null, ");
                }
                if (int9 != -1)
                {
                    sbUpdate.Append("integer_value_9 = " + int9 + ", ");
                }
                else
                {
                    sbUpdate.Append("integer_value_9 = null, ");
                }
                if (int10 != -1)
                {
                    sbUpdate.Append("integer_value_10 = " + int10 + ", ");
                }
                else
                {
                    sbUpdate.Append("integer_value_10 = null, ");
                }
                if (dateTime1 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_1 = '" + dateTime1.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("datetime_value_1 = null, ");
                }
                if (dateTime2 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_2 = '" + dateTime2.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("datetime_value_2 = null, ");
                }
                if (dateTime3 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_3 = '" + dateTime3.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("datetime_value_3 = null, ");
                }
                if (dateTime4 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_4 = '" + dateTime4.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("datetime_value_4 = null, ");
                }
                if (dateTime5 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_5 = '" + dateTime5.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("datetime_value_5 = null, ");
                }
                if (dateTime6 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_6 = '" + dateTime6.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("datetime_value_6 = null, ");
                }
                if (dateTime7 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_7 = '" + dateTime7.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("datetime_value_7 = null, ");
                }
                if (dateTime8 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_8 = '" + dateTime8.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("datetime_value_8 = null, ");
                }
                if (dateTime9 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_9 = '" + dateTime9.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("datetime_value_9 = null, ");
                }
                if (dateTime10 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_10 = '" + dateTime10.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("datetime_value_10 = null, ");
                }
                if (!string1.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_1 = N'" + string1 + "', ");
                }
                else
                {
                    sbUpdate.Append("nvarchar_value_1 = null, ");
                }
                if (!string2.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_2 = N'" + string2 + "', ");
                }
                else
                {
                    sbUpdate.Append("nvarchar_value_2 = null, ");
                }
                if (!string3.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_3 = N'" + string3 + "', ");
                }
                else
                {
                    sbUpdate.Append("nvarchar_value_3 = null, ");
                }
                if (!string4.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_4 = N'" + string4 + "', ");
                }
                else
                {
                    sbUpdate.Append("nvarchar_value_4 = null, ");
                }
                if (!string5.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_5 = N'" + string5 + "', ");
                }
                else
                {
                    sbUpdate.Append("nvarchar_value_5 = null, ");
                }
                if (!string6.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_6 = N'" + string6 + "', ");
                }
                else
                {
                    sbUpdate.Append("nvarchar_value_6 = null, ");
                }
                if (!string7.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_7 = N'" + string7 + "', ");
                }
                else
                {
                    sbUpdate.Append("nvarchar_value_7 = null, ");
                }
                if (!string8.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_8 = N'" + string8 + "', ");
                }
                else
                {
                    sbUpdate.Append("nvarchar_value_8 = null, ");
                }
                if (!string9.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_9 = N'" + string9 + "', ");
                }
                else
                {
                    sbUpdate.Append("nvarchar_value_9 = null, ");
                }
                if (!string10.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_10 = N'" + string10 + "', ");
                }
                else
                {
                    sbUpdate.Append("nvarchar_value_10 = null, ");
                }


                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE employee_id = " + employeeID);

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                sqlTrans.Commit();


            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("UPDATE");

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool delete(int empolyeeID)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM employees_asco4 WHERE employee_id = " + empolyeeID);

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

        public bool delete(int empolyeeID, bool doCommit)
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
                sbDelete.Append("DELETE FROM employees_asco4 WHERE employee_id = " + empolyeeID);

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    isDeleted = true;
                }
                if (doCommit)
                    sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    sqlTrans.Rollback("DELETE");
                throw new Exception(ex.Message);
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
