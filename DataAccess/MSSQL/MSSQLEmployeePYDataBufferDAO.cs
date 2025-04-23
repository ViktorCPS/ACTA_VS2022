using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Data.SqlClient;
using TransferObjects;
using System.Data;

namespace DataAccess
{
    public class MSSQLEmployeePYDataBufferDAO:EmployeePYDataBufferDAO
    {
        SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}
		
		public MSSQLEmployeePYDataBufferDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
			DAOController.GetInstance();
		}

        public MSSQLEmployeePYDataBufferDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
            DAOController.GetInstance();
        }

        public void SetDBConnection(Object dbConnection)
        {
            conn = dbConnection as SqlConnection;
        }

        public int insert(EmployeePYDataBufferTO emplPYDataBuffer, bool doCommit)
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
                sbInsert.Append("INSERT INTO employee_py_data_buffers ");
                sbInsert.Append("(py_calc_id, company_code, type, employee_id, employee_type_name, cost_center_name, cost_center_desc, first_name, last_name, date_start, date_end, fund_hrs, ");
                sbInsert.Append("fund_day, fund_hrs_est, fund_day_est, meal_counter, transport_counter, vacation_left_curr_year, vacation_left_prev_year, ");
                sbInsert.Append("vacation_used_curr_year, vacation_used_prev_year, bank_hrs_balans, paid_leave_balans, paid_leave_used, stop_working_hrs_balans, created_time, ");
                sbInsert.Append("not_justified_overtime_balans, not_justified_overtime_movement, ");
                sbInsert.Append("KG_regular_approved, BG_regular_approved, KG_PARK_regular_approved, KG_regular_not_approved, BG_regular_not_approved, KG_PARK_regular_not_approved)");
                sbInsert.Append("VALUES (");                
                sbInsert.Append(emplPYDataBuffer.PYCalcID + ", ");
                sbInsert.Append("'" + emplPYDataBuffer.CompanyCode + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.Type + "', ");
                sbInsert.Append(emplPYDataBuffer.EmployeeID + ", ");
                sbInsert.Append("'" + emplPYDataBuffer.EmployeeType.Trim() + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.CCName.Trim() + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.CCDesc.Trim() + "', ");
                sbInsert.Append("N'" + emplPYDataBuffer.FirstName + "', ");
                sbInsert.Append("N'" + emplPYDataBuffer.LastName + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.DateStart.ToString(dateTimeformat) + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.DateEnd.ToString(dateTimeformat) + "', ");                 
                sbInsert.Append("'" + emplPYDataBuffer.FundHrs.ToString() + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.FundDay.ToString() + "', ");                
                if (emplPYDataBuffer.FundHrsEst != -1)                
                    sbInsert.Append("'" + emplPYDataBuffer.FundHrsEst.ToString() + "', ");                
                else                
                    sbInsert.Append("NULL, ");                
                if (emplPYDataBuffer.FundDayEst != -1)                
                    sbInsert.Append("'" + emplPYDataBuffer.FundDayEst.ToString() + "', ");                
                else                
                    sbInsert.Append("NULL, ");                
                sbInsert.Append("'" + emplPYDataBuffer.MealCounter.ToString() + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.TransportCounter.ToString() + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.VacationLeftCurrYear.ToString() + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.VacationLeftPrevYear.ToString() + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.VacationUsedCurrYear.ToString() + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.ValactionUsedPrevYear.ToString() + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.BankHrsBalans.ToString().Trim().Replace(',', '.') + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.PaidLeaveBalans.ToString() + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.PaidLeaveUsed.ToString() + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.StopWorkingHrsBalans.ToString().Trim().Replace(',', '.') + "', ");                
                sbInsert.Append(" GETDATE(), ");
                sbInsert.Append("'" + emplPYDataBuffer.NotJustifiedOvertimeBalans.ToString().Trim() + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.NotJustifiedOvertime.ToString().Trim() + "', ");

                foreach (int type in emplPYDataBuffer.ApprovedMeals.Keys)
                {
                    sbInsert.Append("'" + emplPYDataBuffer.ApprovedMeals[type].ToString().Trim() + "', ");
                }

                foreach (int type in emplPYDataBuffer.NotApprovedMeals.Keys)
                {
                    sbInsert.Append("'" + emplPYDataBuffer.NotApprovedMeals[type].ToString().Trim() + "', ");
                }

                sbInsert = sbInsert.Remove(sbInsert.Length - 2, 1);
                sbInsert.Append(")");
                
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
                    sqlTrans.Rollback("INSERT");
                }
                throw new Exception(emplPYDataBuffer.EmployeeID.ToString() + " " + ex.Message);
            }

            return rowsAffected;
        }

        public int insertExpat(EmployeePYDataBufferTO emplPYDataBuffer, bool doCommit)
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
                sbInsert.Append("INSERT INTO employee_py_data_buffers_expat ");
                sbInsert.Append("(py_calc_id, company_code, type, employee_id, employee_type_name, cost_center_name, cost_center_desc, first_name, last_name, date_start, date_end, fund_hrs, ");
                sbInsert.Append("fund_day, fund_hrs_est, fund_day_est, meal_counter, transport_counter, vacation_left_curr_year, vacation_left_prev_year, ");
                sbInsert.Append("vacation_used_curr_year, vacation_used_prev_year, bank_hrs_balans, paid_leave_balans, paid_leave_used, stop_working_hrs_balans, created_time, ");
                sbInsert.Append("not_justified_overtime_balans, not_justified_overtime_movement, ");
                sbInsert.Append("KG_regular_approved, BG_regular_approved, KG_PARK_regular_approved, KG_regular_not_approved, BG_regular_not_approved, KG_PARK_regular_not_approved)");
                sbInsert.Append("VALUES (");
                sbInsert.Append(emplPYDataBuffer.PYCalcID + ", ");
                sbInsert.Append("'" + emplPYDataBuffer.CompanyCode + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.Type + "', ");
                sbInsert.Append(emplPYDataBuffer.EmployeeID + ", ");
                sbInsert.Append("'" + emplPYDataBuffer.EmployeeType.Trim() + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.CCName.Trim() + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.CCDesc.Trim() + "', ");
                sbInsert.Append("N'" + emplPYDataBuffer.FirstName + "', ");
                sbInsert.Append("N'" + emplPYDataBuffer.LastName + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.DateStart.ToString(dateTimeformat) + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.DateEnd.ToString(dateTimeformat) + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.FundHrs.ToString() + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.FundDay.ToString() + "', ");
                if (emplPYDataBuffer.FundHrsEst != -1)
                    sbInsert.Append("'" + emplPYDataBuffer.FundHrsEst.ToString() + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (emplPYDataBuffer.FundDayEst != -1)
                    sbInsert.Append("'" + emplPYDataBuffer.FundDayEst.ToString() + "', ");
                else
                    sbInsert.Append("NULL, ");
                sbInsert.Append("'" + emplPYDataBuffer.MealCounter.ToString() + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.TransportCounter.ToString() + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.VacationLeftCurrYear.ToString() + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.VacationLeftPrevYear.ToString() + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.VacationUsedCurrYear.ToString() + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.ValactionUsedPrevYear.ToString() + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.BankHrsBalans.ToString().Trim().Replace(',', '.') + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.PaidLeaveBalans.ToString() + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.PaidLeaveUsed.ToString() + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.StopWorkingHrsBalans.ToString().Trim().Replace(',', '.') + "', ");
                sbInsert.Append(" GETDATE(), ");
                sbInsert.Append("'" + emplPYDataBuffer.NotJustifiedOvertimeBalans.ToString().Trim() + "', ");
                sbInsert.Append("'" + emplPYDataBuffer.NotJustifiedOvertime.ToString().Trim() + "', ");

                foreach (int type in emplPYDataBuffer.ApprovedMeals.Keys)
                {
                    sbInsert.Append("'" + emplPYDataBuffer.ApprovedMeals[type].ToString().Trim() + "', ");
                }

                foreach (int type in emplPYDataBuffer.NotApprovedMeals.Keys)
                {
                    sbInsert.Append("'" + emplPYDataBuffer.NotApprovedMeals[type].ToString().Trim() + "', ");
                }

                sbInsert = sbInsert.Remove(sbInsert.Length - 2, 1);
                sbInsert.Append(")");

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
                    sqlTrans.Rollback("INSERT");
                }
                throw new Exception(emplPYDataBuffer.EmployeeID.ToString() + " " + ex.Message);
            }

            return rowsAffected;
        }

        public uint getMaxCalcID()
        {
            DataSet dataSet = new DataSet();
            uint calcID = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT MAX(py_calc_id) AS calc_id FROM employee_py_data_buffers", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

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

        public List<EmployeePYDataBufferTO> getEmployeeBuffers(uint calcID)
        {
            DataSet dataSet = new DataSet();
            EmployeePYDataBufferTO buff = new EmployeePYDataBufferTO();
            List<EmployeePYDataBufferTO> buffList = new List<EmployeePYDataBufferTO>();
            string select = "";

            try
            {
                select = "SELECT * FROM employee_py_data_buffers WHERE py_calc_id = '" + calcID.ToString().Trim() + "' ORDER BY employee_id";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Buffers");
                DataTable table = dataSet.Tables["Buffers"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        buff = new EmployeePYDataBufferTO();
                        buff.PYCalcID = UInt32.Parse(row["py_calc_id"].ToString().Trim());
                        if (row["company_code"] != DBNull.Value)
                        {
                            buff.CompanyCode = row["company_code"].ToString().Trim();
                        }
                        if (row["type"] != DBNull.Value)
                        {
                            buff.Type = row["type"].ToString().Trim();
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            buff.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["employee_type_name"] != DBNull.Value)
                        {
                            buff.EmployeeType = row["employee_type_name"].ToString().Trim();
                        }
                        if (row["cost_center_name"] != DBNull.Value)
                        {
                            buff.CCName = row["cost_center_name"].ToString().Trim();
                        }
                        if (row["cost_center_desc"] != DBNull.Value)
                        {
                            buff.CCDesc = row["cost_center_desc"].ToString().Trim();
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            buff.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            buff.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["date_start"] != DBNull.Value)
                        {
                            buff.DateStart = DateTime.Parse(row["date_start"].ToString().Trim());
                        }
                        if (row["date_end"] != DBNull.Value)
                        {
                            buff.DateEnd = DateTime.Parse(row["date_end"].ToString().Trim());
                        }
                        if (row["fund_hrs"] != DBNull.Value)
                        {
                            buff.FundHrs = Int32.Parse(row["fund_hrs"].ToString().Trim());
                        }
                        if (row["fund_day"] != DBNull.Value)
                        {
                            buff.FundDay = Int32.Parse(row["fund_day"].ToString().Trim());
                        }
                        if (row["fund_hrs_est"] != DBNull.Value)
                        {
                            buff.FundHrsEst = Int32.Parse(row["fund_hrs_est"].ToString().Trim());
                        }
                        if (row["fund_day_est"] != DBNull.Value)
                        {
                            buff.FundDayEst = Int32.Parse(row["fund_day_est"].ToString().Trim());
                        }
                        if (row["meal_counter"] != DBNull.Value)
                        {
                            buff.MealCounter = Int32.Parse(row["meal_counter"].ToString().Trim());
                        }
                        if (row["transport_counter"] != DBNull.Value)
                        {
                            buff.TransportCounter = Int32.Parse(row["transport_counter"].ToString().Trim());
                        }
                        if (row["vacation_left_curr_year"] != DBNull.Value)
                        {
                            buff.VacationLeftCurrYear = Int32.Parse(row["vacation_left_curr_year"].ToString().Trim());
                        }
                        if (row["vacation_left_prev_year"] != DBNull.Value)
                        {
                            buff.VacationLeftPrevYear = Int32.Parse(row["vacation_left_prev_year"].ToString().Trim());
                        }
                        if (row["vacation_used_curr_year"] != DBNull.Value)
                        {
                            buff.VacationUsedCurrYear = Int32.Parse(row["vacation_used_curr_year"].ToString().Trim());
                        }
                        if (row["vacation_used_prev_year"] != DBNull.Value)
                        {
                            buff.ValactionUsedPrevYear = Int32.Parse(row["vacation_used_prev_year"].ToString().Trim());
                        }
                        if (row["bank_hrs_balans"] != DBNull.Value)
                        {
                            buff.BankHrsBalans = Decimal.Parse(row["bank_hrs_balans"].ToString().Trim());
                        }
                        if (row["paid_leave_balans"] != DBNull.Value)
                        {
                            buff.PaidLeaveBalans = Int32.Parse(row["paid_leave_balans"].ToString().Trim());
                        }
                        if (row["paid_leave_used"] != DBNull.Value)
                        {
                            buff.PaidLeaveUsed = Int32.Parse(row["paid_leave_used"].ToString().Trim());
                        }
                        if (row["stop_working_hrs_balans"] != DBNull.Value)
                        {
                            buff.StopWorkingHrsBalans = Decimal.Parse(row["stop_working_hrs_balans"].ToString().Trim());
                        }
                        if (row["not_justified_overtime_balans"] != DBNull.Value)
                        {
                            buff.NotJustifiedOvertimeBalans = Int32.Parse(row["not_justified_overtime_balans"].ToString().Trim());
                        }
                        if (row["not_justified_overtime_movement"] != DBNull.Value)
                        {
                            buff.NotJustifiedOvertime = Int32.Parse(row["not_justified_overtime_movement"].ToString().Trim());
                        }

                        buffList.Add(buff);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return buffList;
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
