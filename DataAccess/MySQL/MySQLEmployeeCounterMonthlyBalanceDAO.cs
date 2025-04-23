using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Data;

using TransferObjects;
using Util;

namespace DataAccess
{
    public class MySQLEmployeeCounterMonthlyBalanceDAO : EmployeeCounterMonthlyBalanceDAO
    {
        MySqlConnection conn = null;
        MySqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MySQLEmployeeCounterMonthlyBalanceDAO()
		{
            conn = MySQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

        public MySQLEmployeeCounterMonthlyBalanceDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(EmployeeCounterMonthlyBalanceTO balanceTO, bool doCommit)
        {
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employee_counter_monthly_balances ");
                sbInsert.Append("(employee_counter_type_id, employee_id, month, value_earned, value_used, balance, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("'" + balanceTO.EmplCounterTypeID.ToString().Trim() + "', ");
                sbInsert.Append("'" + balanceTO.EmployeeID.ToString().Trim() + "', ");
                sbInsert.Append("'" + balanceTO.Month.ToString(dateTimeformat) + "', ");
                sbInsert.Append("'" + balanceTO.ValueEarned.ToString().Trim() + "', ");
                sbInsert.Append("'" + balanceTO.ValueUsed.ToString().Trim() + "', ");
                sbInsert.Append("'" + balanceTO.Balance.ToString().Trim() + "', ");
                if (!balanceTO.CreatedBy.Trim().Equals(""))
                    sbInsert.Append("N'" + balanceTO.CreatedBy.Trim() + "', ");
                else
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!balanceTO.CreatedTime.Equals(new DateTime()))
                    sbInsert.Append("'" + balanceTO.CreatedTime.ToString(dateTimeformat) + "') ");
                else
                    sbInsert.Append("NOW()) ");

                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, SqlTrans);

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

        public bool update(EmployeeCounterMonthlyBalanceTO balanceTO, bool doCommit)
        {
            bool isUpdated = false;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE employee_counter_monthly_balances SET ");
                if (balanceTO.ValueEarned != -1)
                    sbUpdate.Append("value_earned = '" + balanceTO.ValueEarned.ToString().Trim() + "', ");
                if (balanceTO.ValueUsed != -1)
                    sbUpdate.Append("value_used = '" + balanceTO.ValueUsed.ToString().Trim() + "', ");
                if (balanceTO.Balance != -1)
                    sbUpdate.Append("balance = '" + balanceTO.Balance.ToString().Trim() + "', ");
                if (!balanceTO.ModifiedBy.Trim().Equals(""))
                    sbUpdate.Append("modified_by = N'" + balanceTO.ModifiedBy.Trim() + "', ");
                else
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!balanceTO.ModifiedTime.Equals(new DateTime()))
                    sbUpdate.Append("modified_time = '" + balanceTO.ModifiedTime.ToString(dateTimeformat) + "' ");
                else
                    sbUpdate.Append("modified_time = NOW() ");
                sbUpdate.Append("WHERE employee_counter_type_id = '" + balanceTO.EmplCounterTypeID.ToString().Trim() + "' AND employee_id = '" + balanceTO.EmployeeID.ToString().Trim() + "' AND month = '" + balanceTO.Month.ToString(dateTimeformat) + "'");

                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, SqlTrans);
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

        public Dictionary<int, Dictionary<DateTime, Dictionary<int, EmployeeCounterMonthlyBalanceTO>>> getEmployeeBalances(string emplIDs, DateTime month, bool exactMonth)
        {
            DataSet dataSet = new DataSet();
            EmployeeCounterMonthlyBalanceTO balTO = new EmployeeCounterMonthlyBalanceTO();
            Dictionary<int, Dictionary<DateTime, Dictionary<int, EmployeeCounterMonthlyBalanceTO>>> balanceDict = new Dictionary<int, Dictionary<DateTime, Dictionary<int, EmployeeCounterMonthlyBalanceTO>>>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employee_counter_monthly_balances ");
                if ((emplIDs.Trim() != "") || (!month.Equals(new DateTime())))
                {
                    sb.Append(" WHERE");
                    if ((emplIDs.Trim() != ""))
                    {
                        sb.Append(" employee_id IN (" + emplIDs.Trim() + ") AND");
                    }
                    if (month != new DateTime())
                    {
                        if (exactMonth)
                            sb.Append(" month = '" + month.ToString(dateTimeformat) + "' AND");
                        else
                            sb.Append(" month > '" + month.ToString(dateTimeformat) + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY employee_id, month, employee_counter_type_id ";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Balances");
                DataTable table = dataSet.Tables["Balances"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        balTO = new EmployeeCounterMonthlyBalanceTO();

                        balTO.EmplCounterTypeID = Int32.Parse(row["employee_counter_type_id"].ToString().Trim());

                        if (!row["employee_id"].Equals(DBNull.Value))
                        {
                            balTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (!row["month"].Equals(DBNull.Value))
                        {
                            balTO.Month = DateTime.Parse(row["month"].ToString().Trim());
                        }
                        if (!row["value_earned"].Equals(DBNull.Value))
                        {
                            balTO.ValueEarned = Int32.Parse(row["value_earned"].ToString().Trim());
                        }
                        if (!row["value_used"].Equals(DBNull.Value))
                        {
                            balTO.ValueUsed = Int32.Parse(row["value_used"].ToString().Trim());
                        }
                        if (!row["balance"].Equals(DBNull.Value))
                        {
                            balTO.Balance = Int32.Parse(row["balance"].ToString().Trim());
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
                            balanceDict.Add(balTO.EmployeeID, new Dictionary<DateTime, Dictionary<int, EmployeeCounterMonthlyBalanceTO>>());

                        if (!balanceDict[balTO.EmployeeID].ContainsKey(balTO.Month.Date))
                            balanceDict[balTO.EmployeeID].Add(balTO.Month, new Dictionary<int, EmployeeCounterMonthlyBalanceTO>());

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
        public Dictionary<int, Dictionary<DateTime, Dictionary<int, EmployeeCounterMonthlyBalanceTO>>> getEmployeeBalances(string emplIDs, DateTime month, int counterTypeID)
        {
            DataSet dataSet = new DataSet();
            EmployeeCounterMonthlyBalanceTO balTO = new EmployeeCounterMonthlyBalanceTO();
            Dictionary<int, Dictionary<DateTime, Dictionary<int, EmployeeCounterMonthlyBalanceTO>>> balanceDict = new Dictionary<int, Dictionary<DateTime, Dictionary<int, EmployeeCounterMonthlyBalanceTO>>>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employee_counter_monthly_balances WHERE balance <> 0");
                if ((emplIDs.Trim() != "") || (!month.Equals(new DateTime())) || (counterTypeID != -1))
                {
                    if ((emplIDs.Trim() != ""))
                    {
                        sb.Append(" AND employee_id IN (" + emplIDs.Trim() + ")");
                    }
                    if (month != new DateTime())
                    {
                        sb.Append(" AND month <= '" + month.ToString(dateTimeformat) + "'");
                    }
                    if (counterTypeID != -1)
                    {
                        sb.Append(" AND employee_counter_type_id = '" + counterTypeID.ToString().Trim() + "'");
                    }

                    select = sb.ToString();
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + " ORDER BY employee_id, month, employee_counter_type_id ";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Balances");
                DataTable table = dataSet.Tables["Balances"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        balTO = new EmployeeCounterMonthlyBalanceTO();

                        balTO.EmplCounterTypeID = Int32.Parse(row["employee_counter_type_id"].ToString().Trim());

                        if (!row["employee_id"].Equals(DBNull.Value))
                        {
                            balTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (!row["month"].Equals(DBNull.Value))
                        {
                            balTO.Month = DateTime.Parse(row["month"].ToString().Trim());
                        }
                        if (!row["value_earned"].Equals(DBNull.Value))
                        {
                            balTO.ValueEarned = Int32.Parse(row["value_earned"].ToString().Trim());
                        }
                        if (!row["value_used"].Equals(DBNull.Value))
                        {
                            balTO.ValueUsed = Int32.Parse(row["value_used"].ToString().Trim());
                        }
                        if (!row["balance"].Equals(DBNull.Value))
                        {
                            balTO.Balance = Int32.Parse(row["balance"].ToString().Trim());
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
                            balanceDict.Add(balTO.EmployeeID, new Dictionary<DateTime, Dictionary<int, EmployeeCounterMonthlyBalanceTO>>());

                        if (!balanceDict[balTO.EmployeeID].ContainsKey(balTO.Month.Date))
                            balanceDict[balTO.EmployeeID].Add(balTO.Month, new Dictionary<int, EmployeeCounterMonthlyBalanceTO>());

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
        
        public List<EmployeeCounterMonthlyBalanceTO> getEmployeeBalances(string emplIDs, string types, DateTime from, DateTime to)
        {
            DataSet dataSet = new DataSet();
            EmployeeCounterMonthlyBalanceTO balTO = new EmployeeCounterMonthlyBalanceTO();
            List<EmployeeCounterMonthlyBalanceTO> balanceList = new List<EmployeeCounterMonthlyBalanceTO>();
            string select = "";

            DateTime fromDate = from;
            DateTime toDate = to;

            if (fromDate != new DateTime())
                fromDate = new DateTime(fromDate.Year, fromDate.Month, 1);

            if (toDate != new DateTime())
                toDate = new DateTime(toDate.Year, toDate.Month, 1);

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employee_counter_monthly_balances ");
                if ((emplIDs.Trim() != "") || (types.Trim() != "") || (!fromDate.Equals(new DateTime())) || (!toDate.Equals(new DateTime())))
                {
                    sb.Append(" WHERE");
                    if ((emplIDs.Trim() != ""))
                    {
                        sb.Append(" employee_id IN (" + emplIDs.Trim() + ") AND");
                    }
                    if ((types.Trim() != ""))
                    {
                        sb.Append(" employee_counter_type_id IN (" + types.Trim() + ") AND");
                    }
                    if (fromDate != new DateTime())
                    {
                        sb.Append(" month >= '" + fromDate.ToString(dateTimeformat) + "' AND");
                    }
                    if (toDate != new DateTime())
                    {
                        sb.Append(" month <= '" + toDate.ToString(dateTimeformat) + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY employee_id, month, employee_counter_type_id ";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Balances");
                DataTable table = dataSet.Tables["Balances"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        balTO = new EmployeeCounterMonthlyBalanceTO();

                        balTO.EmplCounterTypeID = Int32.Parse(row["employee_counter_type_id"].ToString().Trim());

                        if (!row["employee_id"].Equals(DBNull.Value))
                        {
                            balTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (!row["month"].Equals(DBNull.Value))
                        {
                            balTO.Month = DateTime.Parse(row["month"].ToString().Trim());
                        }
                        if (!row["value_earned"].Equals(DBNull.Value))
                        {
                            balTO.ValueEarned = Int32.Parse(row["value_earned"].ToString().Trim());
                        }
                        if (!row["value_used"].Equals(DBNull.Value))
                        {
                            balTO.ValueUsed = Int32.Parse(row["value_used"].ToString().Trim());
                        }
                        if (!row["balance"].Equals(DBNull.Value))
                        {
                            balTO.Balance = Int32.Parse(row["balance"].ToString().Trim());
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

                        balanceList.Add(balTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return balanceList;
        }

        public DateTime getMinPositiveMonth(string emplIDs)
        {
            DataSet dataSet = new DataSet();
            DateTime month = new DateTime();

            try
            {
                string select = "SELECT MIN(month) AS month FROM employee_counter_monthly_balances WHERE balance <> 0";

                if ((emplIDs.Trim() != ""))
                {
                    select += " AND employee_id IN (" + emplIDs.Trim() + ")";
                }

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Balances");
                DataTable table = dataSet.Tables["Balances"];

                if (table.Rows.Count > 0)
                {
                    if (!table.Rows[0]["month"].Equals(DBNull.Value))
                    {
                        month = DateTime.Parse(table.Rows[0]["month"].ToString().Trim());
                        month = new DateTime(month.Year, month.Month, 1).Date;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return month;
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
            _sqlTrans = (MySqlTransaction)trans;
        }
    }
}
