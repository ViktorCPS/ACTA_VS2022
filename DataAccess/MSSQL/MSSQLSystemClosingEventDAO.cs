using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

using TransferObjects;
using Util;

namespace DataAccess
{
    public class MSSQLSystemClosingEventDAO : SystemClosingEventDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLSystemClosingEventDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
			DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

        public MSSQLSystemClosingEventDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        
        public int insert(SystemClosingEventTO eventTO, bool doCommit)
        {
            if (SqlTrans == null)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO system_closing_events ");
                sbInsert.Append("(type, start_time, end_time, message_sr, message_en, dp_engine_state, dp_engine_state_modified_time, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("N'" + eventTO.Type.Trim() + "', ");
                sbInsert.Append("'" + eventTO.StartTime.ToString(dateTimeformat) + "', ");
                if (eventTO.EndTime != new DateTime())
                    sbInsert.Append("'" + eventTO.EndTime.ToString(dateTimeformat) + "', ");
                else
                    sbInsert.Append("NULL, ");                
                sbInsert.Append("N'" + eventTO.MessageSR.Trim() + "', ");
                sbInsert.Append("N'" + eventTO.MessageEN.Trim() + "', ");                
                if (eventTO.DPEngineState.Trim() != "")
                    sbInsert.Append("N'" + eventTO.DPEngineState.Trim() + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (eventTO.DPEngineStateModifiedTime != new DateTime())
                    sbInsert.Append("'" + eventTO.DPEngineStateModifiedTime.ToString(dateTimeformat) + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (!eventTO.CreatedBy.Trim().Equals(""))
                    sbInsert.Append("N'" + eventTO.CreatedBy.Trim() + "', ");                
                else                
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!eventTO.CreatedTime.Equals(new DateTime()))
                    sbInsert.Append("'" + eventTO.CreatedTime.ToString(dateTimeformat) + "') ");                
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

        public bool update(SystemClosingEventTO eventTO, bool doCommit)
        {
            bool isUpdated = false;

            if (SqlTrans == null)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE system_closing_events SET ");
                if (eventTO.Type.Trim() != "")                
                    sbUpdate.Append("type = N'" + eventTO.Type.Trim() + "', ");
                if (eventTO.StartTime != new DateTime())
                    sbUpdate.Append("start_time = '" + eventTO.StartTime.ToString(dateTimeformat) + "', ");
                if (eventTO.EndTime != new DateTime())
                    sbUpdate.Append("end_time = '" + eventTO.EndTime.ToString(dateTimeformat) + "', ");
                else
                    sbUpdate.Append("end_time = NULL, ");
                if (eventTO.MessageSR.Trim() != "")
                    sbUpdate.Append("message_sr = N'" + eventTO.MessageSR.Trim() + "', ");               
                if (eventTO.MessageEN.Trim() != "")
                    sbUpdate.Append("message_en = N'" + eventTO.MessageEN.Trim() + "', ");
                if (eventTO.DPEngineState.Trim() != "")
                    sbUpdate.Append("dp_engine_state = N'" + eventTO.DPEngineState.Trim() + "', ");
                else
                    sbUpdate.Append("dp_engine_state = NULL, ");
                if (eventTO.DPEngineStateModifiedTime != new DateTime())
                    sbUpdate.Append("dp_engine_state_modified_time = '" + eventTO.DPEngineStateModifiedTime.ToString(dateTimeformat) + "', ");
                else
                    sbUpdate.Append("dp_engine_state_modified_time = NULL, ");
                if (!eventTO.ModifiedBy.Trim().Equals(""))
                    sbUpdate.Append("modified_by = N'" + eventTO.ModifiedBy.Trim() + "', ");
                else
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!eventTO.ModifiedTime.Equals(new DateTime()))
                    sbUpdate.Append("modified_time = '" + eventTO.ModifiedTime.ToString(dateTimeformat) + "' ");
                else
                    sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE event_id = '" + eventTO.EventID.ToString().Trim() + "'");

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

        public bool delete(int eventID, bool doCommit)
        {
            bool isDeleted = false;

            if (SqlTrans == null)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM system_closing_events WHERE event_id = '" + eventID.ToString().Trim() + "'");

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

        public List<SystemClosingEventTO> getClosingEvents(DateTime from, DateTime to)
        {
            DataSet dataSet = new DataSet();
            SystemClosingEventTO eventTO = new SystemClosingEventTO();
            List<SystemClosingEventTO> eventsList = new List<SystemClosingEventTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM system_closing_events ");

                if (from != new DateTime() || to != new DateTime())
                {
                    sb.Append("WHERE type = '" + Constants.closingEventTypeRegularPeriodical + "' OR ( ");

                    if (from != new DateTime())
                        sb.Append("(end_time >= '" + from.ToString(dateTimeformat) + "' OR end_time IS NULL)");

                    if (to != new DateTime())
                        sb.Append("AND (start_time <= '" + to.ToString(dateTimeformat) + "')");

                    sb.Append(")");
                }
                
                select = sb.ToString() + " ORDER BY start_time, end_time ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Events");
                DataTable table = dataSet.Tables["Events"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        eventTO = new SystemClosingEventTO();

                        eventTO.EventID = Int32.Parse(row["event_id"].ToString().Trim());

                        if (!row["type"].Equals(DBNull.Value))
                        {
                            eventTO.Type = row["type"].ToString().Trim();
                        }
                        if (!row["start_time"].Equals(DBNull.Value))
                        {
                            eventTO.StartTime = DateTime.Parse(row["start_time"].ToString().Trim());
                        }
                        if (!row["end_time"].Equals(DBNull.Value))
                        {
                            eventTO.EndTime = DateTime.Parse(row["end_time"].ToString().Trim());
                        }
                        if (!row["message_sr"].Equals(DBNull.Value))
                        {
                            eventTO.MessageSR = row["message_sr"].ToString().Trim();
                        }
                        if (!row["message_en"].Equals(DBNull.Value))
                        {
                            eventTO.MessageEN = row["message_en"].ToString().Trim();
                        }
                        if (!row["dp_engine_state"].Equals(DBNull.Value))
                        {
                            eventTO.DPEngineState = row["dp_engine_state"].ToString().Trim();
                        }
                        if (!row["dp_engine_state_modified_time"].Equals(DBNull.Value))
                        {
                            eventTO.DPEngineStateModifiedTime = DateTime.Parse(row["dp_engine_state_modified_time"].ToString().Trim());
                        }
                        if (!row["created_by"].Equals(DBNull.Value))
                        {
                            eventTO.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (!row["created_time"].Equals(DBNull.Value))
                        {
                            eventTO.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (!row["modified_by"].Equals(DBNull.Value))
                        {
                            eventTO.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (!row["modified_time"].Equals(DBNull.Value))
                        {
                            eventTO.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }

                        eventsList.Add(eventTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return eventsList;
        }

        public List<string> getClosingEventsMessages(string lang)
        {
            DataSet dataSet = new DataSet();            
            List<string> msgList = new List<string>();
            string select = "";

            try
            {
                string field = "message_sr";
                if (lang.Trim() != Constants.Lang_sr)
                    field = "message_en";

                select = "SELECT DISTINCT " + field + " FROM system_closing_events ORDER BY " + field;
                
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Events");
                DataTable table = dataSet.Tables["Events"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (!row[field].Equals(DBNull.Value) && row[field].ToString().Trim() != "")
                            msgList.Add(row[field].ToString().Trim());                        
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
