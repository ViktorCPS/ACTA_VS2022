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
    public class MSSQLApplUserLoginRFIDDAO : ApplUserLoginRFIDDAO
    {
        SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

        public MSSQLApplUserLoginRFIDDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

        public MSSQLApplUserLoginRFIDDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(ApplUserLoginRFIDTO rfidTO, bool doCommit)
        {
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO appl_users_login_rfid (host, tag_id, created_time) VALUES (");
                sbInsert.Append("N'" + rfidTO.Host.Trim() + "', ");
                sbInsert.Append("'" + rfidTO.TagID.Trim() + "', GETDATE())");

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

        public bool update(ApplUserLoginRFIDTO rfidTO, bool doCommit)
        {
            bool isUpdated = false;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE appl_users_login_rfid SET ");
                sbUpdate.Append("tag_id = '" + rfidTO.TagID.Trim() + "',");
                sbUpdate.Append("created_time = GETDATE() ");
                sbUpdate.Append("WHERE host = N'" + rfidTO.Host.Trim() + "'");

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

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool delete(string host, DateTime created, bool doCommit)
        {
            bool isDeleted = false;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM appl_users_login_rfid WHERE host = N'" + host.Trim() + "'");
                if (created != new DateTime())
                    sbDelete.Append(" AND created_time < '" + created.ToString(dateTimeformat) + "'");

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, SqlTrans);
                int res = cmd.ExecuteNonQuery();
                isDeleted = true;

                if (doCommit)
                    commitTransaction();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    rollbackTransaction();

                throw ex;
            }

            return isDeleted;
        }

        public ApplUserLoginRFIDTO getLoginRFID(string host)
        {
            DataSet dataSet = new DataSet();
            ApplUserLoginRFIDTO rfidTO = new ApplUserLoginRFIDTO();

            try
            {
                if (host.Trim() == "")
                    return rfidTO;

                string select = "SELECT * FROM appl_users_login_rfid WHERE host = N'" + host.Trim() + "'";
                                
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Categories");
                DataTable table = dataSet.Tables["Categories"];

                if (table.Rows.Count == 1)
                {
                    rfidTO = new ApplUserLoginRFIDTO();

                    if (!table.Rows[0]["host"].Equals(DBNull.Value))
                    {
                        rfidTO.Host = table.Rows[0]["host"].ToString().Trim();
                    }
                    if (!table.Rows[0]["tag_id"].Equals(DBNull.Value))
                    {
                        rfidTO.TagID = table.Rows[0]["tag_id"].ToString().Trim();
                    }
                    if (!table.Rows[0]["created_time"].Equals(DBNull.Value))
                    {
                        rfidTO.CreatedTime = DateTime.Parse(table.Rows[0]["created_time"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rfidTO;
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

    

