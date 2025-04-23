using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Globalization;

using Util;

namespace DataAccess
{
    public class MySQLSyncAnnualLeaveRecalcFlagDAO : SyncAnnualLeaveRecalcFlagDAO
    {
        MySqlConnection conn = null;
        MySqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

		public MySQLSyncAnnualLeaveRecalcFlagDAO()
		{
            conn = MySQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

        public MySQLSyncAnnualLeaveRecalcFlagDAO(MySqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public bool update(int flag, bool isServiceUpdate, bool doCommit)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans = null;

            if (doCommit)
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            else
                sqlTrans = SqlTrans;

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE sync_annual_leave_recalc_hist_flag SET ");
                sbUpdate.Append("flag = " + flag.ToString().Trim() + ", ");

                if (isServiceUpdate)
                    sbUpdate.Append("updated_time_service = GETDATE() ");
                else
                    sbUpdate.Append("updated_operater = N'" + DAOController.GetLogInUser().Trim() + "', updated_time_operater = GETDATE() ");

                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                if (doCommit)
                    sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    sqlTrans.Rollback();
                throw ex;
            }

            return isUpdated;
        }

        public int getFlag()
        {
            DataSet dataSet = new DataSet();
            int flag = Constants.noInt;
            string select = "";

            try
            {
                select = "SELECT * FROM sync_annual_leave_recalc_hist_flag";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "AL");
                DataTable table = dataSet.Tables["AL"];

                if (table.Rows.Count > 0 && !table.Rows[0]["flag"].Equals(DBNull.Value))
                    flag = int.Parse(table.Rows[0]["flag"].ToString().Trim());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return flag;
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
