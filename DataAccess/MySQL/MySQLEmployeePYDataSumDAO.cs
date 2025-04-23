using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Globalization;
using TransferObjects;

namespace DataAccess
{
    public class MySQLEmployeePYDataSumDAO:EmployeePYDataSumDAO
    {
         MySqlConnection conn = null;
		MySqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

		public MySqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}
		
		public MySQLEmployeePYDataSumDAO()
		{
			conn = MySQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
			DAOController.GetInstance();
		}

        public MySQLEmployeePYDataSumDAO(MySqlConnection sqlConnection)
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

        public uint getMaxCalcID()
        {
            DataSet dataSet = new DataSet();
            uint calcID = 0;
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT MAX(py_calc_id) AS calc_id FROM employee_py_data_sum", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeePY");
                DataTable table = dataSet.Tables["EmployeePY"];

                if (table.Rows.Count == 1 && !table.Rows[0]["calc_id"].Equals(DBNull.Value))
                {
                    calcID = UInt32.Parse(table.Rows[0]["calc_id"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return calcID;
        }

        public DateTime getSumMonth(uint calcID)
        {
            DataSet dataSet = new DataSet();
            DateTime month = new DateTime();
            try
            {
                if (calcID == 0)
                    return month;

                MySqlCommand cmd = new MySqlCommand("SELECT MAX(date_start) AS month FROM employee_py_data_sum WHERE py_calc_id = '" + calcID.ToString().Trim() + "'", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeePY");
                DataTable table = dataSet.Tables["EmployeePY"];

                if (table.Rows.Count == 1 && !table.Rows[0]["month"].Equals(DBNull.Value))
                {
                    month = DateTime.Parse(table.Rows[0]["month"].ToString().Trim());
                    month = new DateTime(month.Year, month.Month, 1);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return month;
        }

        public List<EmployeePYDataSumTO> getSumDates(DateTime from, DateTime to)
        {
            DataSet dataSet = new DataSet();
            EmployeePYDataSumTO sum = new EmployeePYDataSumTO();
            List<EmployeePYDataSumTO> sumList = new List<EmployeePYDataSumTO>();
            List<uint> calcIDList = new List<uint>();

            try
            {
                if (from == new DateTime() || to == new DateTime())
                    return sumList;

                string select = "SELECT * FROM employee_py_data_sum s WHERE date_start = '" + from.Date.ToString(dateTimeformat) + "' AND date_end = '" + to.Date.ToString(dateTimeformat)
                    + "' AND created_time = (SELECT MIN(created_time) FROM employee_py_data_sum WHERE py_calc_id = s.py_calc_id)";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Sum");
                DataTable table = dataSet.Tables["Sum"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        sum = new EmployeePYDataSumTO();

                        if (row["py_calc_id"] != DBNull.Value)
                        {
                            sum.PYCalcID = UInt32.Parse(row["py_calc_id"].ToString().Trim());
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            sum.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }

                        if (!calcIDList.Contains(sum.PYCalcID))
                        {
                            sumList.Add(sum);
                            calcIDList.Add(sum.PYCalcID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return sumList;
        }

        public List<int> getEmployees(uint calcID)
        {
            DataSet dataSet = new DataSet();
            List<int> emplList = new List<int>();
            try
            {
                if (calcID == 0)
                    return emplList;

                MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT employee_id FROM employee_py_data_sum WHERE py_calc_id = '" + calcID.ToString().Trim() + "'", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeePY");
                DataTable table = dataSet.Tables["EmployeePY"];

                foreach (DataRow row in table.Rows)
                {
                    if (!row["employee_id"].Equals(DBNull.Value))
                    {
                        emplList.Add(int.Parse(row["employee_id"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return emplList;
        }

        public List<EmployeePYDataSumTO> getEmployeesSum(EmployeePYDataSumTO emplTO)
        {
            DataSet dataSet = new DataSet();
            EmployeePYDataSumTO employee = new EmployeePYDataSumTO();
            List<EmployeePYDataSumTO> employeesList = new List<EmployeePYDataSumTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employee_py_data_sum");
                if ((emplTO.EmployeeID != -1) || (!emplTO.CompanyCode.Trim().Equals("")) ||
                    (!emplTO.PaymentCode.Trim().Equals("")) || (emplTO.PYCalcID != 0) ||
                    (emplTO.HrsAmount != -1) || (!emplTO.DateEnd.Equals(new DateTime())) ||
                    (!emplTO.EmployeeType.Trim().Equals("")) || (!emplTO.CCName.Trim().Equals("")) ||
                    (!emplTO.CCDesc.Trim().Equals("")) || (!emplTO.DateStartSickness.Equals(new DateTime())) ||
                    (!emplTO.DateEnd.Equals(new DateTime())) || (!emplTO.Type.Trim().Equals("")))                    
                {
                    sb.Append(" WHERE");

                    if (emplTO.EmployeeID != -1)
                    {
                        //sb.Append(" UPPER(employee_id) LIKE '" + EmployeeID.Trim().ToUpper() + "' AND");
                        sb.Append(" employee_id = '" + emplTO.EmployeeID.ToString().Trim() + "' AND");
                    }
                    if (!emplTO.EmployeeType.Trim().Equals(""))
                    {
                        sb.Append(" employee_type_name = '" + emplTO.EmployeeType + "' AND");
                    }
                    if (!emplTO.CCName.Trim().Equals(""))
                    {
                        sb.Append(" cost_center_name = '" + emplTO.CCName + "' AND");
                    }
                    if (!emplTO.CCDesc.Trim().Equals(""))
                    {
                        sb.Append(" cost_center_desc = '" + emplTO.CCDesc + "' AND");
                    }
                    if (!emplTO.DateEnd.Equals(new DateTime()))
                    {
                        sb.Append(" date_end = '" + emplTO.DateEnd.ToString(dateTimeformat) + "' AND");
                    }
                    if (!emplTO.DateStart.Equals(new DateTime()))
                    {
                        sb.Append(" date_start = '" + emplTO.DateStart.ToString(dateTimeformat) + "' AND");
                    }
                    if (!emplTO.DateStartSickness.Equals(new DateTime()))
                    {
                        sb.Append(" date_start_sickness = '" + emplTO.DateStartSickness.ToString(dateTimeformat) + "' AND");
                    }
                    if (emplTO.HrsAmount != -1)
                    {
                        //sb.Append(" UPPER(working_unit_id) LIKE '" + WorkingUnitID.Trim().ToUpper() + "' AND");
                        sb.Append(" hrs_amount = " + emplTO.HrsAmount.ToString() + " AND");
                    }
                    if (!emplTO.PaymentCode.Trim().Equals(""))
                    {
                        sb.Append(" payment_code = '" + emplTO.PaymentCode + "' AND");
                    }
                    if (!emplTO.CompanyCode.Trim().Equals(""))
                    {
                        sb.Append(" company_code = '" + emplTO.CompanyCode + "' AND");
                    }      
                    if (emplTO.PYCalcID != 0)
                    {
                        //sb.Append(" UPPER(address_id) LIKE '" + AddressID.Trim().ToUpper() + "' AND");
                        sb.Append(" py_calc_id = " + emplTO.PYCalcID.ToString().Trim() + " AND");
                    }                   
                    if (!emplTO.Type.Trim().Equals(""))
                    {
                        sb.Append(" type = '" + emplTO.Type.Trim().ToUpper() + "' AND");
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
                        employee = new EmployeePYDataSumTO();
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
                        if (row["date_end"] != DBNull.Value)
                        {
                            employee.DateEnd = DateTime.Parse(row["date_end"].ToString().Trim());
                        }
                        if (row["date_start"] != DBNull.Value)
                        {
                            employee.DateStart = DateTime.Parse(row["date_start"].ToString().Trim());
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
                        if (row["rec_id"] != DBNull.Value)
                        {
                            employee.RecID = UInt32.Parse(row["rec_id"].ToString().Trim());
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

        public Dictionary<int, Dictionary<string, decimal>> getEmployeesSumValues(uint calcID, string codes)
        {
            DataSet dataSet = new DataSet();
            EmployeePYDataSumTO sum = new EmployeePYDataSumTO();
            Dictionary<int, Dictionary<string, decimal>> sumDict = new Dictionary<int, Dictionary<string, decimal>>();
            string select = "";

            try
            {
                if (calcID == 0 && codes.Trim().Equals(""))
                    return sumDict;

                select = "SELECT * FROM employee_py_data_sum WHERE";

                if (!codes.Trim().Equals(""))                
                    select += " payment_code IN (" + codes + ") AND";
                                
                if (calcID != 0)                
                    select += " py_calc_id = '" + calcID.ToString().Trim() + "' AND";                    

                select = select.Substring(0, select.Length - 3);

                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeSum");
                DataTable table = dataSet.Tables["EmployeeSum"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        sum = new EmployeePYDataSumTO();
                        sum.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["employee_type_name"] != DBNull.Value)
                        {
                            sum.EmployeeType = row["employee_type_name"].ToString().Trim();
                        }
                        if (row["cost_center_name"] != DBNull.Value)
                        {
                            sum.CCName = row["cost_center_name"].ToString().Trim();
                        }
                        if (row["cost_center_desc"] != DBNull.Value)
                        {
                            sum.CCDesc = row["cost_center_desc"].ToString().Trim();
                        }
                        if (row["date_end"] != DBNull.Value)
                        {
                            sum.DateEnd = DateTime.Parse(row["date_end"].ToString().Trim());
                        }
                        if (row["date_start"] != DBNull.Value)
                        {
                            sum.DateStart = DateTime.Parse(row["date_start"].ToString().Trim());
                        }
                        if (row["date_start_sickness"] != DBNull.Value)
                        {
                            sum.DateStartSickness = DateTime.Parse(row["date_start_sickness"].ToString().Trim());
                        }
                        if (row["hrs_amount"] != DBNull.Value)
                        {
                            sum.HrsAmount = Decimal.Parse(row["hrs_amount"].ToString().Trim());
                        }
                        if (row["company_code"] != DBNull.Value)
                        {
                            sum.CompanyCode = row["company_code"].ToString().Trim();
                        }
                        if (row["payment_code"] != DBNull.Value)
                        {
                            sum.PaymentCode = row["payment_code"].ToString().Trim();
                        }
                        if (row["type"] != DBNull.Value)
                        {
                            sum.Type = row["type"].ToString().Trim();
                        }
                        if (row["py_calc_id"] != DBNull.Value)
                        {
                            sum.PYCalcID = UInt32.Parse(row["py_calc_id"].ToString().Trim());
                        }
                        if (row["rec_id"] != DBNull.Value)
                        {
                            sum.RecID = UInt32.Parse(row["rec_id"].ToString().Trim());
                        }

                        if (!sumDict.ContainsKey(sum.EmployeeID))
                            sumDict.Add(sum.EmployeeID, new Dictionary<string, decimal>());

                        if (!sumDict[sum.EmployeeID].ContainsKey(sum.PaymentCode))
                            sumDict[sum.EmployeeID].Add(sum.PaymentCode, 0);
                        
                        sumDict[sum.EmployeeID][sum.PaymentCode] += sum.HrsAmount;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sumDict;
        }

        public int insert(EmployeePYDataSumTO emplPYDataSum, bool doCommit)
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
                sbInsert.Append("INSERT INTO employee_py_data_sum ");
                sbInsert.Append("(py_calc_id, company_code, type, employee_id, employee_type_name, cost_center_name, cost_center_desc, date_start, date_end, date_start_sickness, payment_code, hrs_amount, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append(emplPYDataSum.PYCalcID + ", ");
                sbInsert.Append("'" + emplPYDataSum.CompanyCode + "', ");
                sbInsert.Append("'" + emplPYDataSum.Type + "', ");
                sbInsert.Append(emplPYDataSum.EmployeeID + ", ");
                sbInsert.Append("'" + emplPYDataSum.EmployeeType.Trim() + "', ");
                sbInsert.Append("'" + emplPYDataSum.CCName.Trim() + "', ");
                sbInsert.Append("'" + emplPYDataSum.CCDesc.Trim() + "', ");
                sbInsert.Append("'" + emplPYDataSum.DateStart.ToString(dateTimeformat) + "', ");
                sbInsert.Append("'" + emplPYDataSum.DateEnd.ToString(dateTimeformat) + "', ");
                sbInsert.Append("'" + emplPYDataSum.DateStartSickness.ToString(dateTimeformat) + "', ");
                sbInsert.Append("'" + emplPYDataSum.PaymentCode + "', ");
                sbInsert.Append("'" + emplPYDataSum.HrsAmount.ToString().Trim().Replace(',', '.') + "', ");
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
