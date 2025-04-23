using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using TransferObjects;

namespace DataAccess
{
    public class MSSQLSyncBufferDataHistDAO:SyncBufferDataHistDAO
    {
  SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

		public MSSQLSyncBufferDataHistDAO()
		{
            conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

        public MSSQLSyncBufferDataHistDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public DateTime getMaxDate()
        {
            DataSet dataSet = new DataSet();
            DateTime maxDate = new DateTime();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT MAX(created_time) AS max_date FROM sync_buffer_data_hist", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "WUnits");
                DataTable table = dataSet.Tables["WUnits"];

                if (table.Rows.Count == 1 && !table.Rows[0]["max_date"].Equals(DBNull.Value))
                {
                    maxDate = DateTime.Parse(table.Rows[0]["max_date"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return maxDate;
        }
        public bool insert(SyncBufferDataTO syncFS, bool doCommit)
        {
            if (doCommit)
            {
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            }
            else
            {
                SqlTrans = this.SqlTrans;
            }
            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO sync_buffer_data_hist ");
                sbInsert.Append("(rec_hist_id, employee_id, days_of_holiday_for_current_years_left, Days_of_holiday_for_previous_years_left, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append(" " + syncFS.RecID + ",");
                sbInsert.Append(" " + syncFS.EmployeeID + ",");
                if (syncFS.DaysOfHolidayForCurrentYearLeft != -1)
                {
                    sbInsert.Append(" '" + syncFS.DaysOfHolidayForCurrentYearLeft + "',");
                } 
                if (syncFS.DaysOfHolidayForPreviousYearLeft != -1)
                {
                    sbInsert.Append(" '" + syncFS.DaysOfHolidayForPreviousYearLeft + "',");
                }
                if (syncFS.CreatedBy != "")
                {
                    sbInsert.Append(" N'" + syncFS.CreatedBy + "',");
                }
                if (syncFS.CreatedTime != new DateTime())
                {
                    sbInsert.Append(" '" + syncFS.CreatedTime.ToString(dateTimeformat) + "')");
                }

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

            return rowsAffected>0;
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
