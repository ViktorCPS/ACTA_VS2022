using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

using TransferObjects;
using Util;

namespace DataAccess
{
    public class MSSQLSyncAnnualLeaveRecalcHistDAO : SyncAnnualLeaveRecalcHistDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

		public MSSQLSyncAnnualLeaveRecalcHistDAO()
		{
            conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

        public MSSQLSyncAnnualLeaveRecalcHistDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public bool insert(SyncAnnualLeaveRecalcTO syncRecalc, bool doCommit)
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
                sbInsert.Append("INSERT INTO sync_annual_leave_recalc_hist ");
                sbInsert.Append("(rec_id, employee_id, year, num_of_days, created_by, created_time, result, remark, created_time_hist) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append(" " + syncRecalc.RecID + ",");
                sbInsert.Append(" " + syncRecalc.EmployeeID + ",");
                sbInsert.Append(" '" + syncRecalc.Year.ToString(dateTimeformat) + "',");
                sbInsert.Append(" " + syncRecalc.NumOfDays + ",");
                sbInsert.Append(" N'" + syncRecalc.CreatedBy + "',");
                sbInsert.Append(" '" + syncRecalc.CreatedTime.ToString(dateTimeformat) + "',");
                sbInsert.Append(" '" + syncRecalc.Result.ToString().Trim() + "',");
                if (syncRecalc.Remark != "")
                {
                    sbInsert.Append(" N'" + syncRecalc.Remark + "',");
                }
                else
                {
                    sbInsert.Append(" NULL,");
                }
                sbInsert.Append(" GETDATE()) ");

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

            return rowsAffected > 0;
        }

        public List<SyncAnnualLeaveRecalcTO> getALRecalculations(DateTime from, DateTime to, int emplID, int result)
        {
            DataSet dataSet = new DataSet();
            SyncAnnualLeaveRecalcTO alTO = new SyncAnnualLeaveRecalcTO();
            List<SyncAnnualLeaveRecalcTO> alList = new List<SyncAnnualLeaveRecalcTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM sync_annual_leave_recalc_hist ");

                if (!from.Equals(new DateTime()) || !to.Equals(new DateTime()) || emplID != -1 || result != -1)
                {
                    sb.Append("WHERE ");

                    if (!from.Equals(new DateTime()))
                        sb.Append("created_time_hist >= '" + from.Date.ToString(dateTimeformat) + "' AND ");

                    if (!to.Equals(new DateTime()))
                        sb.Append("created_time_hist < '" + to.Date.AddDays(1).ToString(dateTimeformat) + "' AND ");

                    if (emplID != -1)
                        sb.Append("employee_id = '" + emplID.ToString().Trim() + "' AND ");

                    if (result != -1)
                        sb.Append("result = '" + result.ToString().Trim() + "' AND ");

                    select = sb.ToString(0, sb.ToString().Length - 4);
                }
                else
                    select = sb.ToString();

                select = select + "ORDER BY created_time_hist ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "AL");
                DataTable table = dataSet.Tables["AL"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        alTO = new SyncAnnualLeaveRecalcTO();

                        if (!row["employee_id"].Equals(DBNull.Value))
                        {
                            alTO.EmployeeID = int.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (!row["year"].Equals(DBNull.Value))
                        {
                            alTO.Year = DateTime.Parse(row["year"].ToString().Trim());
                        }
                        if (!row["num_of_days"].Equals(DBNull.Value))
                        {
                            alTO.NumOfDays = int.Parse(row["num_of_days"].ToString().Trim());
                        }
                        if (!row["created_by"].Equals(DBNull.Value))
                        {
                            alTO.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (!row["created_time"].Equals(DBNull.Value))
                        {
                            alTO.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (!row["created_time_hist"].Equals(DBNull.Value))
                        {
                            alTO.CreatedTimeHist = DateTime.Parse(row["created_time_hist"].ToString().Trim());
                        }
                        if (!row["result"].Equals(DBNull.Value))
                        {
                            alTO.Result = int.Parse(row["result"].ToString().Trim());
                        }
                        if (!row["remark"].Equals(DBNull.Value))
                        {
                            alTO.Remark = row["remark"].ToString().Trim();
                        }

                        alList.Add(alTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return alList;
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
