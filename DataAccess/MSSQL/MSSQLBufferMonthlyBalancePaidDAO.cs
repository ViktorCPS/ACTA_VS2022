using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;

using TransferObjects;
using Util;

namespace DataAccess
{
    public class MSSQLBufferMonthlyBalancePaidDAO : BufferMonthlyBalancePaidDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLBufferMonthlyBalancePaidDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MSSQLBufferMonthlyBalancePaidDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(BufferMonthlyBalancePaidTO balanceTO, bool doCommit)
        {            
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            
            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employee_py_data_buffers_monthly_balances_paid ");
                sbInsert.Append("(py_calc_id, employee_counter_type_id, employee_id, month, hours_payed, recalculation_flag, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("'" + balanceTO.PYCalcID.ToString().Trim() + "', ");
                sbInsert.Append("'" + balanceTO.EmplCounterTypeID.ToString().Trim() + "', ");
                sbInsert.Append("'" + balanceTO.EmployeeID.ToString().Trim() + "', ");
                sbInsert.Append("'" + balanceTO.Month.ToString(dateTimeformat) + "', ");
                sbInsert.Append("'" + balanceTO.ValuePaid.ToString().Trim() + "', ");
                sbInsert.Append("'" + balanceTO.RecalcFlag.ToString().Trim() + "', ");
                if (!balanceTO.CreatedBy.Trim().Equals(""))
                    sbInsert.Append("N'" + balanceTO.CreatedBy.Trim() + "', ");
                else
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!balanceTO.CreatedTime.Equals(new DateTime()))
                    sbInsert.Append("'" + balanceTO.CreatedTime.ToString(dateTimeformat) + "') ");
                else
                    sbInsert.Append("GETDATE()) ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, SqlTrans);

                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit)
                    commitTransaction();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    rollbackTransaction();

                throw ex;
            }

            return rowsAffected;
        }

        public bool update(BufferMonthlyBalancePaidTO balanceTO, bool doCommit)
        {
            bool isUpdated = false;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE employee_py_data_buffers_monthly_balances_paid SET ");
                if (balanceTO.RecalcFlag != -1)
                {
                    sbUpdate.Append("recalculation_flag = '" + balanceTO.RecalcFlag.ToString().Trim() + "', ");
                }
                if (balanceTO.ValuePaid != -1)
                {
                    sbUpdate.Append("hours_payed = '" + balanceTO.ValuePaid.ToString().Trim() + "', ");
                }

                if (!balanceTO.ModifiedBy.Trim().Equals(""))
                    sbUpdate.Append("modified_by = N'" + balanceTO.ModifiedBy.Trim() + "', ");
                else
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE employee_counter_type_id = '" + balanceTO.EmplCounterTypeID.ToString().Trim() + "' AND employee_id = '" + balanceTO.EmployeeID.ToString().Trim() 
                    + "' AND py_calc_id = '" + balanceTO.PYCalcID.ToString().Trim() + "' AND month = '" + balanceTO.Month.ToString(dateTimeformat) + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, SqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                if (doCommit)
                    commitTransaction();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    rollbackTransaction();
                throw ex;
            }

            return isUpdated;
        }
        
        public Dictionary<int, Dictionary<DateTime, Dictionary<int, BufferMonthlyBalancePaidTO>>> getEmployeeBalancesPaid(uint pyCalcID, int counterType)
        {
            DataSet dataSet = new DataSet();
            BufferMonthlyBalancePaidTO balTO = new BufferMonthlyBalancePaidTO();
            Dictionary<int, Dictionary<DateTime, Dictionary<int, BufferMonthlyBalancePaidTO>>> balanceDict = new Dictionary<int, Dictionary<DateTime, Dictionary<int, BufferMonthlyBalancePaidTO>>>();
            string select = "";

            try
            {                
                select = "SELECT * FROM employee_py_data_buffers_monthly_balances_paid WHERE py_calc_id = '" + pyCalcID.ToString().Trim() + "'"
                    + " AND employee_counter_type_id = '" + counterType.ToString().Trim() + "' ORDER BY employee_id, month, employee_counter_type_id ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Balances");
                DataTable table = dataSet.Tables["Balances"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        balTO = new BufferMonthlyBalancePaidTO();

                        balTO.PYCalcID = UInt32.Parse(row["py_calc_id"].ToString().Trim());
                        if (!row["employee_counter_type_id"].Equals(DBNull.Value))
                        {
                            balTO.EmplCounterTypeID = Int32.Parse(row["employee_counter_type_id"].ToString().Trim());
                        }
                        if (!row["employee_id"].Equals(DBNull.Value))
                        {
                            balTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (!row["month"].Equals(DBNull.Value))
                        {
                            balTO.Month = DateTime.Parse(row["month"].ToString().Trim());
                        }
                        if (!row["hours_payed"].Equals(DBNull.Value))
                        {
                            balTO.ValuePaid = Int32.Parse(row["hours_payed"].ToString().Trim());
                        }
                        if (!row["recalculation_flag"].Equals(DBNull.Value))
                        {
                            balTO.RecalcFlag = Int32.Parse(row["recalculation_flag"].ToString().Trim());
                        }
                        if (!row["created_by"].Equals(DBNull.Value))
                        {
                            balTO.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (!row["created_time"].Equals(DBNull.Value))
                        {
                            balTO.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (!row["modified_by"].Equals(DBNull.Value))
                        {
                            balTO.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (!row["modified_time"].Equals(DBNull.Value))
                        {
                            balTO.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }

                        if (!balanceDict.ContainsKey(balTO.EmployeeID))
                            balanceDict.Add(balTO.EmployeeID, new Dictionary<DateTime, Dictionary<int, BufferMonthlyBalancePaidTO>>());

                        if (!balanceDict[balTO.EmployeeID].ContainsKey(balTO.Month.Date))
                            balanceDict[balTO.EmployeeID].Add(balTO.Month, new Dictionary<int, BufferMonthlyBalancePaidTO>());

                        if (!balanceDict[balTO.EmployeeID][balTO.Month].ContainsKey(balTO.EmplCounterTypeID))
                            balanceDict[balTO.EmployeeID][balTO.Month].Add(balTO.EmplCounterTypeID, balTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return balanceDict;
        }

        public Dictionary<int, Dictionary<DateTime, Dictionary<int, BufferMonthlyBalancePaidTO>>> getEmployeeBalancesPaid(string emplIDs, DateTime from, DateTime to)
        {
            DataSet dataSet = new DataSet();
            BufferMonthlyBalancePaidTO balTO = new BufferMonthlyBalancePaidTO();
            Dictionary<int, Dictionary<DateTime, Dictionary<int, BufferMonthlyBalancePaidTO>>> balanceDict = new Dictionary<int, Dictionary<DateTime, Dictionary<int, BufferMonthlyBalancePaidTO>>>();
            DateTime fromMonth = new DateTime();
            DateTime toMonth = new DateTime();

            if (from != new DateTime())
                fromMonth = new DateTime(from.Year, from.Month, 1);

            if (to != new DateTime())
                toMonth = new DateTime(to.Year, to.Month, 1);

            string select = "";

            try
            {
                select = "SELECT * FROM employee_py_data_buffers_monthly_balances_paid WHERE recalculation_flag = '" + Constants.yesInt.ToString().Trim() + "'";

                if (emplIDs.Trim() != "")
                    select += " AND employee_id IN (" + emplIDs + ")";

                if (fromMonth != new DateTime())
                    select += " AND month >= '" + fromMonth.Date.ToString(dateTimeformat) + "'";

                if (toMonth != new DateTime())
                    select += " AND month <= '" + toMonth.Date.ToString(dateTimeformat) + "'";

                select += " ORDER BY employee_id, month, employee_counter_type_id ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Balances");
                DataTable table = dataSet.Tables["Balances"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        balTO = new BufferMonthlyBalancePaidTO();

                        balTO.PYCalcID = UInt32.Parse(row["py_calc_id"].ToString().Trim());
                        if (!row["employee_counter_type_id"].Equals(DBNull.Value))
                        {
                            balTO.EmplCounterTypeID = Int32.Parse(row["employee_counter_type_id"].ToString().Trim());
                        }
                        if (!row["employee_id"].Equals(DBNull.Value))
                        {
                            balTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (!row["month"].Equals(DBNull.Value))
                        {
                            balTO.Month = DateTime.Parse(row["month"].ToString().Trim());
                        }
                        if (!row["hours_payed"].Equals(DBNull.Value))
                        {
                            balTO.ValuePaid = Int32.Parse(row["hours_payed"].ToString().Trim());
                        }
                        if (!row["recalculation_flag"].Equals(DBNull.Value))
                        {
                            balTO.RecalcFlag = Int32.Parse(row["recalculation_flag"].ToString().Trim());
                        }
                        if (!row["created_by"].Equals(DBNull.Value))
                        {
                            balTO.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (!row["created_time"].Equals(DBNull.Value))
                        {
                            balTO.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (!row["modified_by"].Equals(DBNull.Value))
                        {
                            balTO.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (!row["modified_time"].Equals(DBNull.Value))
                        {
                            balTO.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }

                        if (!balanceDict.ContainsKey(balTO.EmployeeID))
                            balanceDict.Add(balTO.EmployeeID, new Dictionary<DateTime, Dictionary<int, BufferMonthlyBalancePaidTO>>());

                        if (!balanceDict[balTO.EmployeeID].ContainsKey(balTO.Month.Date))
                            balanceDict[balTO.EmployeeID].Add(balTO.Month, new Dictionary<int, BufferMonthlyBalancePaidTO>());

                        if (!balanceDict[balTO.EmployeeID][balTO.Month].ContainsKey(balTO.EmplCounterTypeID))
                            balanceDict[balTO.EmployeeID][balTO.Month].Add(balTO.EmplCounterTypeID, balTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return balanceDict;
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
                throw ex;
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
