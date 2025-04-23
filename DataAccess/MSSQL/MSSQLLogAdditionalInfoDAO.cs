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

namespace DataAccess.MSSQL
{
    public class MSSQLLogAdditionalInfoDAO : LogAdditionalInfoDAO
    {

        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLLogAdditionalInfoDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
		}

        public MSSQLLogAdditionalInfoDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
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

        public int insert(LogAdditionalInfoTO logAdditionalInfoTO)
        {
            int rowsAffected = 0;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");

            try
            {
                StringBuilder sbInsert = new StringBuilder();

                sbInsert.Append("INSERT INTO log_additional_info ");
                sbInsert.Append("(log_id,  gps_data, cardholder_name, cardholder_id, created_by, created_time) ");
                sbInsert.Append("VALUES ("
                     + logAdditionalInfoTO.LogID + ", " + logAdditionalInfoTO.GpsData + ", " + logAdditionalInfoTO.CardholderName + ", "
                     + logAdditionalInfoTO.CardholderID + ", N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");


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

        public List<LogAdditionalInfoTO> getLogs(LogAdditionalInfoTO logAdditionalInfoTO)
        {
            DataSet dataSet = new DataSet();
            LogAdditionalInfoTO log = new LogAdditionalInfoTO();
            List<LogAdditionalInfoTO> logAddinfoList = new List<LogAdditionalInfoTO>();
            StringBuilder sb = new StringBuilder();
            string select;

            try
            {
                sb.Append("SELECT * FROM log_additional_info ");

                if ((logAdditionalInfoTO.LogID != 0) || (!logAdditionalInfoTO.GpsData.Equals(null))
                    || (!logAdditionalInfoTO.CardholderName.Equals(null)) || (logAdditionalInfoTO.CardholderID != -1))
                {
                    sb.Append("WHERE ");


                    if (logAdditionalInfoTO.LogID > 0)
                    {
                        sb.Append("log_id  = " + logAdditionalInfoTO.LogID + " AND ");
                    }
                    if (!logAdditionalInfoTO.GpsData.Equals(null))
                    {
                        sb.Append("gps_data  = " + logAdditionalInfoTO.GpsData + " AND ");
                    }
                    if (!logAdditionalInfoTO.CardholderName.Equals(null))
                    {
                        sb.Append("cardholder_name  = " + logAdditionalInfoTO.CardholderName + " AND ");
                    }
                    if (logAdditionalInfoTO.CardholderID != -1)
                    {
                        sb.Append("cardholder_id  = " + logAdditionalInfoTO.CardholderID + " AND ");
                    }
                    select = sb.ToString(0, sb.ToString().Length - 4);

                }
                else
                {
                    select = sb.ToString();
                }

                select += "ORDER BY log_id ASC";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dataSet, "Log");
                DataTable dataTable = dataSet.Tables["Log"];

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        log = new LogAdditionalInfoTO();

                        // Map value to Log Transfer Object
                        if (row["log_id"] != DBNull.Value)
                        {
                            log.LogID = UInt32.Parse(row["log_id"].ToString().Trim());
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
                        logAddinfoList.Add(log);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return logAddinfoList;
        }

        public bool update(LogAdditionalInfoTO logAdditionalInfoTO, bool doCommit)
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

                sbUpdate.Append("UPDATE log_additional_info SET ");
                sbUpdate.Append("gps_data = " + logAdditionalInfoTO.GpsData + ", ");
                sbUpdate.Append("cardholder_name = " + logAdditionalInfoTO.CardholderName + ", ");
                sbUpdate.Append("cardholder_id = " + logAdditionalInfoTO.CardholderID + ", ");
                sbUpdate.Append("modified_by =N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE log_id = " + logAdditionalInfoTO.LogID);

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

        public bool delete(int logID)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM log_additional_info WHERE log_id = " + logID);

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

        public LogAdditionalInfoTO find(int logID)
        {
            DataSet dataSet = new DataSet();
            LogAdditionalInfoTO logTO = new LogAdditionalInfoTO();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM log_additional_info WHERE log_id = " + logID, conn);
                SqlDataAdapter sqlAddapter = new SqlDataAdapter(cmd);
                sqlAddapter.Fill(dataSet, "Log");
                DataTable table = dataSet.Tables["Log"];

                if (table.Rows.Count == 1)
                {
                    LogAdditionalInfoTO logTo = new LogAdditionalInfoTO();

                    if (table.Rows[0]["gps_data"] != DBNull.Value)
                    {
                        logTO.GpsData = table.Rows[0]["gps_data"].ToString().Trim();
                    }
                    if (table.Rows[0]["cardholder_name"] != DBNull.Value)
                    {
                        logTO.CardholderName = table.Rows[0]["cardholder_name"].ToString().Trim();
                    }
                    if (table.Rows[0]["cardholder_id"] != DBNull.Value)
                    {
                        logTO.CardholderID = Int32.Parse(table.Rows[0]["cardholder_id"].ToString().Trim());
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
