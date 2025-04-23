using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Globalization;
using TransferObjects;

namespace DataAccess
{
    public class MySQLSyncBufferDataHistDAO:SyncBufferDataHistDAO
    {
          MySqlConnection conn = null;
        MySqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

		public MySQLSyncBufferDataHistDAO()
		{
            conn = MySQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

        public MySQLSyncBufferDataHistDAO(MySqlConnection sqlConnection)
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
                MySqlCommand cmd = new MySqlCommand("SELECT MAX(created_time) AS max_date FROM sync_buffer_data_hist", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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
                sbInsert.Append("(rec_id, employee_id, days_of_holiday_for_current_years_left, Days_of_holiday_for_previous_years_left, created_by, created_time) ");
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

                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, SqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();
                if (doCommit)
                    SqlTrans.Commit();

            }
            catch (Exception ex)
            {
                if (doCommit)
                    SqlTrans.Rollback();

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
            _sqlTrans = (MySqlTransaction)trans;
        }
    }
}
