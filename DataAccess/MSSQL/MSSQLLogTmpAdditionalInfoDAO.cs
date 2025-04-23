using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using TransferObjects;
using Util;
using System.Globalization;


namespace DataAccess.MSSQL
{
    public class MSSQLLogTmpAdditionalInfoDAO : LogTmpAdditionalInfoDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLLogTmpAdditionalInfoDAO()
		{

			conn = MSSQLDAOFactory.getConnection();

            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        
        public MSSQLLogTmpAdditionalInfoDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;

            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
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

        public void setTransaction(IDbTransaction trans)
        {
            _sqlTrans = (SqlTransaction)trans;
        }

        public IDbTransaction getTransaction()
        {
            return _sqlTrans;
        }

        public int insert(LogTmpAdditionalInfoTO logTmpTO)
        {
            int rowsAffected = 0;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");

            try
            {
                StringBuilder sbInsert = new StringBuilder();

                sbInsert.Append("INSERT INTO log_tmp_additional_info ");
                sbInsert.Append("(reader_id, tag_id, antenna, event_happened, action_commited, "
                    + "event_time, gps_data, cardholder_name, cardholder_id) ");
                sbInsert.Append("VALUES ("
                    + logTmpTO.ReaderID + ", " + logTmpTO.TagID + ", " + logTmpTO.Antenna + ", "
                    + logTmpTO.EventHappened + ", " + logTmpTO.ActionCommited + ", '" + logTmpTO.EventTime.ToString(dateTimeformat.Replace("'", "")).Trim() + "', "
                    + logTmpTO.GpsData + ", " + logTmpTO.CardholderName + ", " + logTmpTO.CardholderID + ") ");


                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();
                sqlTrans.Commit();
            }
            catch (SqlException sqlex)
            {
                sqlTrans.Rollback("INSERT");
                if (sqlex.Number == 2627)
                {
                    throw new Exception("Log already exists");
                }
                else
                {
                    throw new Exception(sqlex.Message);
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("INSERT");

                throw new Exception("Exception: " + ex.Message);

            }

            return rowsAffected;
        }

        public List<LogTmpAdditionalInfoTO> getLogs(LogTmpAdditionalInfoTO logTmpAdditionalInfoTO)
        {
            DataSet dataSet = new DataSet();
            LogTmpAdditionalInfoTO log = new LogTmpAdditionalInfoTO();
            List<LogTmpAdditionalInfoTO> logTmpTOList = new List<LogTmpAdditionalInfoTO>();
            StringBuilder sb = new StringBuilder();
            string select;

            try
            {
                sb.Append("SELECT * FROM log_tmp_additional_info ");

                if ((logTmpAdditionalInfoTO.ReaderID != -1) || (logTmpAdditionalInfoTO.TagID > 0) ||
                    (logTmpAdditionalInfoTO.Antenna != -1) || (logTmpAdditionalInfoTO.EventHappened != -1) || (logTmpAdditionalInfoTO.ActionCommited != -1) ||
                    (!logTmpAdditionalInfoTO.EventTime.Equals(null)) || (!logTmpAdditionalInfoTO.GpsData.Equals(null))
                    || (!logTmpAdditionalInfoTO.CardholderName.Equals(null)) || (logTmpAdditionalInfoTO.CardholderID != -1))
                {
                    sb.Append("WHERE ");

                    if (logTmpAdditionalInfoTO.ReaderID != -1)
                    {
                        sb.Append("reader_id  = " + logTmpAdditionalInfoTO.ReaderID + " AND ");
                    }
                    if (logTmpAdditionalInfoTO.TagID > 0)
                    {
                        sb.Append("tag_id  = " + logTmpAdditionalInfoTO.TagID + " AND ");
                    }
                    if (logTmpAdditionalInfoTO.Antenna != -1)
                    {
                        sb.Append("antenna  = " + logTmpAdditionalInfoTO.Antenna + " AND ");
                    }
                    if (logTmpAdditionalInfoTO.EventHappened != -1)
                    {
                        sb.Append("event_happened  = " + logTmpAdditionalInfoTO.EventHappened + " AND ");
                    }
                    if (logTmpAdditionalInfoTO.ActionCommited != -1)
                    {
                        sb.Append("action_commited  = " + logTmpAdditionalInfoTO.ActionCommited + " AND ");
                    }
                    if (!logTmpAdditionalInfoTO.EventTime.Equals(new DateTime()))
                    {
                        sb.Append("event_time  = '" + logTmpAdditionalInfoTO.EventTime + "' AND ");
                    }
                    if (!logTmpAdditionalInfoTO.GpsData.Equals(null))
                    {
                        sb.Append("gps_data  = " + logTmpAdditionalInfoTO.GpsData + " AND ");
                    }
                    if (!logTmpAdditionalInfoTO.CardholderName.Equals(null))
                    {
                        sb.Append("cardholder_name  = " + logTmpAdditionalInfoTO.CardholderName + " AND ");
                    }
                    if (logTmpAdditionalInfoTO.CardholderID != -1)
                    {
                        sb.Append("cardholder_id  = " + logTmpAdditionalInfoTO.CardholderID + " AND ");
                    }
                    select = sb.ToString(0, sb.ToString().Length - 4);

                }
                else
                {
                    select = sb.ToString();
                }

                select += "ORDER BY tag_id ASC";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dataSet, "Log");
                DataTable dataTable = dataSet.Tables["Log"];

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        log = new LogTmpAdditionalInfoTO();

                        // Map value to Log Transfer Object
                        if (row["reader_id"] != DBNull.Value)
                        {
                            log.ReaderID = Int32.Parse(row["reader_id"].ToString().Trim());
                        }
                        if (row["tag_id"] != DBNull.Value)
                        {
                            log.TagID = UInt32.Parse(row["tag_id"].ToString().Trim());
                        }
                        if (row["antenna"] != DBNull.Value)
                        {
                            log.Antenna = Int32.Parse(row["antenna"].ToString().Trim());
                        }
                        if (row["event_happened"] != DBNull.Value)
                        {
                            log.EventHappened = Int32.Parse(row["event_happened"].ToString().Trim());
                        }
                        if (row["action_commited"] != DBNull.Value)
                        {
                            log.ActionCommited = Int32.Parse(row["action_commited"].ToString().Trim());
                        }
                        if (row["event_time"] != DBNull.Value)
                        {
                            log.EventTime = (DateTime)row["event_time"];
                        }
                        if (row["gps_data"] != DBNull.Value)
                        {
                            log.GpsData = row["gps_data"].ToString().Trim();
                        }
                        if (row["cardholder_name"] != DBNull.Value)
                        {
                            log.CardholderName = row["cardholder_name"].ToString().Trim();
                        }
                        if (row["cardholder_id"] != DBNull.Value)
                        {
                            log.CardholderID = Int32.Parse(row["cardholder_id"].ToString().Trim());
                        }
                        // Add Log Transfer Object to List
                        logTmpTOList.Add(log);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return logTmpTOList;
        }

        public bool update(LogTmpAdditionalInfoTO logTmpTo, bool doCommit)
        {
            bool isUpdated = false;
            SqlTransaction sqlTrans = null;

            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }
            SqlCommand cmd = null;

            try
            {
                StringBuilder sbUpdate = new StringBuilder();

                sbUpdate.Append("UPDATE log_tmp_additional_info SET ");
                //sbUpdate.Append("reader_id = " + logTmpTo.ReaderID + ", ");
                //sbUpdate.Append("tag_id = " + logTmpTo.TagID + ", ");
                //sbUpdate.Append("antenna = " + logTmpTo.Antenna + ", ");
                sbUpdate.Append("event_happened = " + logTmpTo.EventHappened + ", ");
                sbUpdate.Append("action_commited = " + logTmpTo.ActionCommited + ", ");
                //sbUpdate.Append("event_time = '" + logTmpTo.EventTime.ToString(dateTimeformat.Replace("'", "")).Trim() + "', ");
                sbUpdate.Append("gps_data = " + logTmpTo.GpsData + ", ");
                sbUpdate.Append("cardholder_name = " + logTmpTo.CardholderName + ", ");
                sbUpdate.Append("cardholder_id = " + logTmpTo.CardholderID + ", ");
                sbUpdate.Append("WHERE reader_id = " + logTmpTo.ReaderID + " AND tag_id = " + logTmpTo.TagID + " AND antenna = " + logTmpTo.Antenna + " AND event_time = " + logTmpTo.EventTime.ToString(dateTimeformat.Replace("'", "")).Trim());

                cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);

                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                if (doCommit)
                {
                    sqlTrans.Commit();
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback("UPDATE");
                }

                throw new Exception("Exception: " + ex.Message);
            }

            return isUpdated;
        }

        public bool delete(int readerID, uint tagID, int antenna, DateTime eventTime)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM log_tmp_additional_info WHERE reader_id = " + readerID + " AND tag_id = " + tagID + " AND antenna = " + antenna + " AND event_time = " + eventTime.ToString());


                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                isDeleted = true;
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("DELETE");

                throw new Exception("Exception: " + ex.Message);
            }

            return isDeleted;
        }

        public LogTmpAdditionalInfoTO find(int readerID, uint tagID, int antenna, DateTime eventTime)
        {
            DataSet dataSet = new DataSet();
            LogTmpAdditionalInfoTO logTO = new LogTmpAdditionalInfoTO();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM log_tmp_additional_info WHERE reader_id = " + readerID + " AND tag_id = " + tagID + " AND antenna = " + antenna + " AND event_time = " + eventTime.ToString(dateTimeformat.Replace("'", "")).Trim(), conn);
                SqlDataAdapter sqlAddapter = new SqlDataAdapter(cmd);
                sqlAddapter.Fill(dataSet, "Log");
                DataTable table = dataSet.Tables["Log"];

                if (table.Rows.Count == 1)
                {
                    LogTmpAdditionalInfoTO logTo = new LogTmpAdditionalInfoTO();

                    if (table.Rows[0]["reader_id"] != DBNull.Value)
                    {
                        logTo.ReaderID = Int32.Parse(table.Rows[0]["reader_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["tag_id"] != DBNull.Value)
                    {
                        logTo.TagID = UInt32.Parse(table.Rows[0]["tag_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["antenna"] != DBNull.Value)
                    {
                        logTo.Antenna = Int32.Parse(table.Rows[0]["antenna"].ToString().Trim());
                    }
                    if (table.Rows[0]["event_happened"] != DBNull.Value)
                    {
                        logTo.EventHappened = Int32.Parse(table.Rows[0]["event_happened"].ToString().Trim());
                    }
                    if (table.Rows[0]["action_commited"] != DBNull.Value)
                    {
                        logTo.ActionCommited = Int32.Parse(table.Rows[0]["action_commited"].ToString().Trim());
                    }
                    if (table.Rows[0]["event_time"] != DBNull.Value)
                    {
                        logTo.EventTime = (DateTime)table.Rows[0]["event_time"];
                    }
                    if (table.Rows[0]["gps_data"] != DBNull.Value)
                    {
                        logTo.GpsData = table.Rows[0]["gps_data"].ToString().Trim();
                    }
                    if (table.Rows[0]["cardholder_name"] != DBNull.Value)
                    {
                        logTo.CardholderName = table.Rows[0]["cardholder_name"].ToString().Trim();
                    }
                    if (table.Rows[0]["cardholder_id"] != DBNull.Value)
                    {
                        logTo.CardholderID = Int32.Parse(table.Rows[0]["cardholder_id"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return logTO;
        }

    }
}
