using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

using TransferObjects;

namespace DataAccess
{
    public class MSSQLSystemMessageDAO : SystemMessageDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLSystemMessageDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
			DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

        public MSSQLSystemMessageDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(SystemMessageTO msgTO, bool doCommit)
        {
            if (SqlTrans == null)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO system_messages ");
                sbInsert.Append("(working_unit_id, appl_users_category_id, start_time, end_time, message_sr, message_en, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("'" + msgTO.WorkingUnitID.ToString().Trim() + "', ");
                sbInsert.Append("'" + msgTO.ApplUserCategoryID.ToString().Trim() + "', ");
                sbInsert.Append("'" + msgTO.StartTime.ToString(dateTimeformat) + "', ");
                sbInsert.Append("'" + msgTO.EndTime.ToString(dateTimeformat) + "', ");                
                sbInsert.Append("N'" + msgTO.MessageSR.Trim() + "', ");
                sbInsert.Append("N'" + msgTO.MessageEN.Trim() + "', ");                
                if (!msgTO.CreatedBy.Trim().Equals(""))
                    sbInsert.Append("N'" + msgTO.CreatedBy.Trim() + "', ");
                else
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!msgTO.CreatedTime.Equals(new DateTime()))
                    sbInsert.Append("'" + msgTO.CreatedTime.ToString(dateTimeformat) + "') ");
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

        public bool update(SystemMessageTO msgTO, bool doCommit)
        {
            bool isUpdated = false;

            if (SqlTrans == null)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE system_messages SET ");
                if (msgTO.WorkingUnitID != -1)
                    sbUpdate.Append("working_unit_id = '" + msgTO.WorkingUnitID.ToString().Trim() + "', ");
                if (msgTO.ApplUserCategoryID != -1)
                    sbUpdate.Append("appl_users_category_id = '" + msgTO.ApplUserCategoryID.ToString().Trim() + "', ");
                if (msgTO.StartTime != new DateTime())
                    sbUpdate.Append("start_time = '" + msgTO.StartTime.ToString(dateTimeformat) + "', ");
                if (msgTO.EndTime != new DateTime())
                    sbUpdate.Append("end_time = '" + msgTO.EndTime.ToString(dateTimeformat) + "', ");
                if (msgTO.MessageSR.Trim() != "")
                    sbUpdate.Append("message_sr = N'" + msgTO.MessageSR.Trim() + "', ");                
                if (msgTO.MessageEN.Trim() != "")
                    sbUpdate.Append("message_en = N'" + msgTO.MessageEN.Trim() + "', ");                
                if (!msgTO.ModifiedBy.Trim().Equals(""))
                    sbUpdate.Append("modified_by = N'" + msgTO.ModifiedBy.Trim() + "', ");
                else
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!msgTO.ModifiedTime.Equals(new DateTime()))
                    sbUpdate.Append("modified_time = '" + msgTO.ModifiedTime.ToString(dateTimeformat) + "' ");
                else
                    sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE message_id = '" + msgTO.MessageID.ToString().Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, SqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                if (doCommit)
                {
                    if (isUpdated)
                        commitTransaction();
                    else
                        rollbackTransaction();
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                    rollbackTransaction();
                throw ex;
            }

            return isUpdated;
        }

        public bool delete(int msgID, bool doCommit)
        {
            bool isDeleted = false;

            if (SqlTrans == null)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM system_messages WHERE message_id = '" + msgID.ToString().Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, SqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    isDeleted = true;
                }

                if (doCommit)
                {
                    if (isDeleted)
                        commitTransaction();
                    else
                        rollbackTransaction();
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                    rollbackTransaction();

                throw ex;
            }

            return isDeleted;
        }

        public List<SystemMessageTO> getSystemMessages(DateTime from, DateTime to, string company, int role)
        {
            DataSet dataSet = new DataSet();
            SystemMessageTO msgTO = new SystemMessageTO();
            List<SystemMessageTO> msgList = new List<SystemMessageTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM system_messages ");

                if (from != new DateTime() || to != new DateTime() || company.Trim() != "" || role != -1)
                {
                    sb.Append("WHERE ");

                    if (from != new DateTime())
                        sb.Append("end_time >= '" + from.ToString(dateTimeformat) + "' AND ");

                    if (to != new DateTime())
                        sb.Append("start_time <= '" + to.ToString(dateTimeformat) + "' AND ");

                    if (company.Trim() != "")
                        sb.Append("working_unit_id IN (" + company.Trim() + ") AND ");

                    if (role != -1)
                        sb.Append("appl_users_category_id = '" + role.ToString().Trim() + "' AND ");

                    select = sb.ToString().Substring(0, sb.Length - 4);
                }
                else
                    select = sb.ToString();

                select += " ORDER BY start_time, end_time ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Events");
                DataTable table = dataSet.Tables["Events"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        msgTO = new SystemMessageTO();

                        msgTO.MessageID = Int32.Parse(row["message_id"].ToString().Trim());

                        if (!row["working_unit_id"].Equals(DBNull.Value))
                        {
                            msgTO.WorkingUnitID = int.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (!row["appl_users_category_id"].Equals(DBNull.Value))
                        {
                            msgTO.ApplUserCategoryID = int.Parse(row["appl_users_category_id"].ToString().Trim());
                        }
                        if (!row["start_time"].Equals(DBNull.Value))
                        {
                            msgTO.StartTime = DateTime.Parse(row["start_time"].ToString().Trim());
                        }
                        if (!row["end_time"].Equals(DBNull.Value))
                        {
                            msgTO.EndTime = DateTime.Parse(row["end_time"].ToString().Trim());
                        }
                        if (!row["message_sr"].Equals(DBNull.Value))
                        {
                            msgTO.MessageSR = row["message_sr"].ToString().Trim();
                        }
                        if (!row["message_en"].Equals(DBNull.Value))
                        {
                            msgTO.MessageEN = row["message_en"].ToString().Trim();
                        }
                        if (!row["created_by"].Equals(DBNull.Value))
                        {
                            msgTO.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (!row["created_time"].Equals(DBNull.Value))
                        {
                            msgTO.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (!row["modified_by"].Equals(DBNull.Value))
                        {
                            msgTO.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (!row["modified_time"].Equals(DBNull.Value))
                        {
                            msgTO.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }

                        msgList.Add(msgTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return msgList;
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
