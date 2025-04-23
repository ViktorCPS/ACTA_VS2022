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
    public class MSSQLEmployeeCounterMonthlyBalanceHistDAO : EmployeeCounterMonthlyBalanceHistDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLEmployeeCounterMonthlyBalanceHistDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public MSSQLEmployeeCounterMonthlyBalanceHistDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
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
                sbInsert.Append("INSERT INTO employee_counter_monthly_balances_hist ");
                sbInsert.Append("(employee_counter_type_id, employee_id, month, value_earned, value_used, balance, created_by, created_time, modified_by, modified_time) ");
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
                    sbInsert.Append("NULL, ");
                if (!balanceTO.CreatedTime.Equals(new DateTime()))
                    sbInsert.Append("'" + balanceTO.CreatedTime.ToString(dateTimeformat) + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (!balanceTO.ModifiedBy.Trim().Equals(""))
                    sbInsert.Append("N'" + balanceTO.ModifiedBy.Trim() + "', ");
                else
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!balanceTO.ModifiedTime.Equals(new DateTime()))
                    sbInsert.Append("'" + balanceTO.ModifiedTime.ToString(dateTimeformat) + "') ");
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
