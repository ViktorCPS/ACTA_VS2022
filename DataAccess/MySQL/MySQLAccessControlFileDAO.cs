using System;
using System.Collections;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Globalization;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using TransferObjects;
using Util;

namespace DataAccess
{
    /// <summary>
    /// Summary description for MySQLAccessControlFileDAO.
    /// </summary>
    public class MySQLAccessControlFileDAO : AccessControlFileDAO
    {
        MySqlConnection conn = null;
        protected string dateTimeformat = "";
        string database = "";
        MySqlTransaction _sqlTrans = null;

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MySQLAccessControlFileDAO()
		{
			conn = MySQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
            database = Constants.GetDatabaseString + "_files.";
		}

        public MySQLAccessControlFileDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
            database = Constants.GetDatabaseString + "_files.";
        }

        public void CloseDBConnection()
        {
            try
            {
                (new MySQLDAOFactory()).CloseConnection();
            }
            catch { }
        }

        public int insert(string type, int readerID, int delay, string status, DateTime uploadStartTime,
            DateTime uploadEndTime, byte[] content, bool doCommit)
        {
            MySqlTransaction sqlTrans = null;

            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO " + database + "access_control_files ");
                sbInsert.Append("(type, reader_id, delay, status, upload_start_time, upload_end_time, content, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                sbInsert.Append("N'" + type + "', ");
                sbInsert.Append("" + readerID + ", ");
                sbInsert.Append("" + delay + ", ");
                sbInsert.Append("N'" + status.Trim() + "', ");

                if (!uploadStartTime.Equals(new DateTime(0)))
                {
                    sbInsert.Append("'" + uploadStartTime.ToString(dateTimeformat) + "', ");
                    //sbInsert.Append("'" + uploadStartTime.ToString("yyy-MM-dd HH:mm") + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (!uploadEndTime.Equals(new DateTime(0)))
                {
                    sbInsert.Append("'" + uploadEndTime.ToString(dateTimeformat) + "', ");
                    //sbInsert.Append("'" + uploadEndTime.ToString("yyy-MM-dd HH:mm") + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append("?Content, ");

                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");

                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);
                cmd.Parameters.Add("?Content", MySqlDbType.MediumBlob, content.Length).Value = content;

                rowsAffected = cmd.ExecuteNonQuery();
                if (doCommit)
                {
                    sqlTrans.Commit();
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw ex;
            }

            return rowsAffected;
        }

        public bool deleteOld(int readerID, string type, bool doCommit)
        {
            bool isDeleted = false;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                string delete = "DELETE FROM " + database + "access_control_files WHERE reader_id = '" + readerID.ToString().Trim() + "' AND type = '" + type.Trim() + "' AND created_time < "
                    + "(SELECT created_time FROM "
                    + "(SELECT * FROM " + database + "access_control_files WHERE reader_id = '" + readerID.ToString().Trim() + "' AND type = '" + type.Trim() 
                    + "' ORDER BY created_time DESC LIMIT " + Constants.acfNumOfSetsLeave.ToString().Trim() + ") as tmp ORDER BY created_time LIMIT 1)";
                                
                MySqlCommand cmd = new MySqlCommand(delete, conn, SqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res >= 0)
                {
                    isDeleted = true;
                }

                if (doCommit)
                    SqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    SqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public bool update(int readerID, string type, string oldStatus, string newStatus,
            DateTime uploadStartTime, DateTime uploadEndTime, int delay, int recordID,
            string modifiedBy, bool doCommit)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans = null;
            string select = "";

            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE " + database + "access_control_files SET ");

                if (!newStatus.Trim().Equals(""))
                {
                    sbUpdate.Append("status = N'" + newStatus.Trim() + "', ");
                }
                if (!uploadStartTime.Equals(new DateTime(0)))
                {
                    sbUpdate.Append("upload_start_time = NOW(), ");
                    //sbUpdate.Append("upload_start_time = '" + uploadStartTime.ToString(dateTimeformat) + "', ");
                    //sbUpdate.Append("upload_start_time = '" + uploadStartTime.ToString("yyy-MM-dd HH:mm") + "', ");
                }
                if (!uploadEndTime.Equals(new DateTime(0)))
                {
                    sbUpdate.Append("upload_end_time = NOW(), ");
                    //sbUpdate.Append("upload_end_time = '" + uploadEndTime.ToString(dateTimeformat) + "', ");
                    //sbUpdate.Append("upload_end_time = '" + uploadEndTime.ToString("yyy-MM-dd HH:mm") + "', ");
                }
                if (delay != -1)
                {
                    sbUpdate.Append("delay = " + delay + ", ");
                }

                if (modifiedBy.Equals(""))
                {
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("modified_by = N'" + modifiedBy.Trim() + "', ");
                }
                sbUpdate.Append("modified_time = NOW() ");

                if ((readerID != -1) || (!type.Equals("")) || (!oldStatus.Equals("")) || (recordID != -1))
                {
                    sbUpdate.Append(" WHERE ");

                    if (readerID != -1)
                    {
                        sbUpdate.Append(" reader_id = " + readerID + " AND");
                    }
                    if (!type.Equals(""))
                    {
                        sbUpdate.Append(" type = N'" + type + "' AND");
                    }
                    if (!oldStatus.Equals(""))
                    {
                        sbUpdate.Append(" status = N'" + oldStatus + "' AND");
                    }
                    if (recordID != -1)
                    {
                        sbUpdate.Append(" record_id = " + recordID + " AND");
                    }

                    select = sbUpdate.ToString(0, sbUpdate.ToString().Length - 3);
                }
                else
                {
                    select = sbUpdate.ToString();
                }

                MySqlCommand cmd = new MySqlCommand(select, conn, sqlTrans);
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
                sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool updateOthers(int readerID, string type, string oldStatus, string newStatus,
            DateTime uploadStartTime, DateTime uploadEndTime, int delay, int recordID,
            string modifiedBy, bool doCommit)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans = null;
            string select = "";

            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE " + database + "access_control_files SET ");

                if (!newStatus.Trim().Equals(""))
                {
                    sbUpdate.Append("status = N'" + newStatus.Trim() + "', ");
                }
                if (!uploadStartTime.Equals(new DateTime(0)))
                {
                    sbUpdate.Append("upload_start_time = NOW(), ");
                    //sbUpdate.Append("upload_start_time = '" + uploadStartTime.ToString(dateTimeformat) + "', ");
                    //sbUpdate.Append("upload_start_time = '" + uploadStartTime.ToString("yyy-MM-dd HH:mm") + "', ");
                }
                if (!uploadEndTime.Equals(new DateTime(0)))
                {
                    sbUpdate.Append("upload_end_time = NOW(), ");
                    //sbUpdate.Append("upload_end_time = '" + uploadEndTime.ToString(dateTimeformat) + "', ");
                    //sbUpdate.Append("upload_end_time = '" + uploadEndTime.ToString("yyy-MM-dd HH:mm") + "', ");
                }
                if (delay != -1)
                {
                    sbUpdate.Append("delay = " + delay + ", ");
                }

                if (modifiedBy.Equals(""))
                {
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("modified_by = N'" + modifiedBy.Trim() + "', ");
                }
                sbUpdate.Append("modified_time = NOW() ");

                if ((readerID != -1) || (!type.Equals("")) || (!oldStatus.Equals("")) || (recordID != -1))
                {
                    sbUpdate.Append(" WHERE ");

                    if (readerID != -1)
                    {
                        sbUpdate.Append(" reader_id = " + readerID + " AND");
                    }
                    if (!type.Equals(""))
                    {
                        sbUpdate.Append(" type = N'" + type + "' AND");
                    }
                    if (!oldStatus.Equals(""))
                    {
                        sbUpdate.Append(" status = N'" + oldStatus + "' AND");
                    }
                    if (recordID != -1)
                    {
                        sbUpdate.Append(" record_id <> " + recordID + " AND");
                    }

                    select = sbUpdate.ToString(0, sbUpdate.ToString().Length - 3);
                }
                else
                {
                    select = sbUpdate.ToString();
                }

                MySqlCommand cmd = new MySqlCommand(select, conn, sqlTrans);
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
                sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        /*public ExtraHourTO find(int employeeID, DateTime dateEarned)
        {
            DataSet dataSet = new DataSet();
            ExtraHourTO extraHourTO = new ExtraHourTO();
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT employee_id, date_earned, extra_time_amt FROM extra_hours WHERE employee_id = "
                    + employeeID + " AND date_earned = '" + dateEarned.ToString("yyy-MM-dd").Trim() + "'", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ExtraHour");
                DataTable table = dataSet.Tables["ExtraHour"];

                if (table.Rows.Count == 1)
                {
                    extraHourTO = new ExtraHourTO();

                    if (table.Rows[0]["employee_id"] != DBNull.Value)
                    {
                        extraHourTO.EmployeeID = Int32.Parse(table.Rows[0]["employee_id"].ToString().Trim());
                    }

                    if (!table.Rows[0]["date_earned"].Equals(DBNull.Value))
                    {
                        extraHourTO.DateEarned = DateTime.Parse(table.Rows[0]["date_earned"].ToString());
                    }

                    if (!table.Rows[0]["extra_time_amt"].Equals(DBNull.Value))
                    {
                        extraHourTO.ExtraTimeAmt = Int32.Parse(table.Rows[0]["extra_time_amt"].ToString().Trim());
                        extraHourTO.CalculatedTimeAmt = Util.Misc.transformMinToStringTime(extraHourTO.ExtraTimeAmt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return extraHourTO;
        }*/

        public ArrayList getAccessControlFiles(string type, int reader_id, int delay, string status,
            DateTime uploadStartTime, DateTime uploadEndTime)
        {
            DataSet dataSet = new DataSet();
            AccessControlFileTO accessControlFileTO = new AccessControlFileTO();
            ArrayList accessControlFileList = new ArrayList();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT record_id, reader_id, delay, status, upload_start_time, upload_end_time, content FROM " + database + "access_control_files ");

                if ((!type.Equals("")) || (reader_id != -1) || (delay != -1) || (!status.Equals("")) ||
                    (!uploadStartTime.Equals(new DateTime(0))) || (!uploadEndTime.Equals(new DateTime(0))))
                {
                    sb.Append(" WHERE ");

                    if (!type.Equals(""))
                    {
                        sb.Append(" type = N'" + type + "' AND");
                    }
                    if (reader_id != -1)
                    {
                        sb.Append(" reader_id = " + reader_id + " AND");
                    }
                    if (delay != -1)
                    {
                        sb.Append(" delay = " + delay + " AND");
                    }
                    if (!status.Equals(""))
                    {
                        sb.Append(" status = N'" + status + "' AND");
                    }
                    /*if (!uploadStartTime.Equals(new DateTime()))
					{
                        //sekunde
                        sb.Append(" upload_start_time = convert('" + uploadStartTime.ToString("yyy-MM-dd HH:mm:ss") + "', datetime) AND"); //da li 4y
					}
                    if (!uploadEndTime.Equals(new DateTime()))
					{
                        //sekunde
                        sb.Append(" upload_end_time = convert('" + uploadEndTime.ToString("yyy-MM-dd HH:mm:ss") + "', datetime) AND"); //da li 4y
					}*/

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + " ORDER BY reader_id, created_time DESC";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "AccessControlFile");
                DataTable table = dataSet.Tables["AccessControlFile"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        accessControlFileTO = new AccessControlFileTO();

                        if (!row["record_id"].Equals(DBNull.Value))
                        {
                            accessControlFileTO.RecordID = Int32.Parse(row["record_id"].ToString().Trim());
                        }
                        if (!row["reader_id"].Equals(DBNull.Value))
                        {
                            accessControlFileTO.ReaderID = Int32.Parse(row["reader_id"].ToString().Trim());
                        }
                        if (!row["delay"].Equals(DBNull.Value))
                        {
                            accessControlFileTO.Delayed = Int32.Parse(row["delay"].ToString().Trim());
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            accessControlFileTO.Status = row["status"].ToString().Trim();
                        }
                        if (!row["upload_start_time"].Equals(DBNull.Value))
                        {
                            accessControlFileTO.UploadStartTime = DateTime.Parse(row["upload_start_time"].ToString());
                        }
                        if (!row["upload_end_time"].Equals(DBNull.Value))
                        {
                            accessControlFileTO.UploadEndTime = DateTime.Parse(row["upload_end_time"].ToString());
                        }
                        if (!row["content"].Equals(DBNull.Value))
                        {
                            accessControlFileTO.Content = (byte[])row["content"];
                        }

                        accessControlFileList.Add(accessControlFileTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return accessControlFileList;
        }

        public AccessControlFileTO getAccessControlFilesMax(string type, int readerID)
        {
            DataSet dataSet = new DataSet();
            AccessControlFileTO accessControlFileTO = new AccessControlFileTO();
            string select = "";
            MySQLDAOFactory daoFactory = new MySQLDAOFactory();
            MySqlConnection connection = null;

            try
            {
                select = "SELECT * FROM " + database + "access_control_files WHERE type = '" + type.Trim() + "' AND reader_id = '" + readerID.ToString() + "' "
                        + "AND record_id = (SELECT MAX(record_id) FROM " + database + "access_control_files "
                        + "WHERE type = '" + type.Trim() + "' AND reader_id = '" + readerID.ToString() + "')";

                connection = (MySqlConnection)daoFactory.MakeNewDBConnection();
                
                MySqlCommand cmd = new MySqlCommand(select, connection);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "AccessControlFile");
                DataTable table = dataSet.Tables["AccessControlFile"];

                if (table.Rows.Count == 1)
                {
                    if (!table.Rows[0]["record_id"].Equals(DBNull.Value))
                    {
                        accessControlFileTO.RecordID = Int32.Parse(table.Rows[0]["record_id"].ToString().Trim());
                    }
                    if (!table.Rows[0]["reader_id"].Equals(DBNull.Value))
                    {
                        accessControlFileTO.ReaderID = Int32.Parse(table.Rows[0]["reader_id"].ToString().Trim());
                    }
                    if (!table.Rows[0]["delay"].Equals(DBNull.Value))
                    {
                        accessControlFileTO.Delayed = Int32.Parse(table.Rows[0]["delay"].ToString().Trim());
                    }
                    if (!table.Rows[0]["status"].Equals(DBNull.Value))
                    {
                        accessControlFileTO.Status = table.Rows[0]["status"].ToString().Trim();
                    }
                    if (!table.Rows[0]["upload_start_time"].Equals(DBNull.Value))
                    {
                        accessControlFileTO.UploadStartTime = DateTime.Parse(table.Rows[0]["upload_start_time"].ToString());
                    }
                    if (!table.Rows[0]["upload_end_time"].Equals(DBNull.Value))
                    {
                        accessControlFileTO.UploadEndTime = DateTime.Parse(table.Rows[0]["upload_end_time"].ToString());
                    }
                    if (!table.Rows[0]["content"].Equals(DBNull.Value))
                    {
                        accessControlFileTO.Content = (byte[])table.Rows[0]["content"];
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection != null)
                {
                    daoFactory.CloseConnection(connection);
                }
            }

            return accessControlFileTO;
        }

        public int getAccessControlFilesCount(string type, string status)
        {
            DataSet dataSet = new DataSet();
            string select = "";
            int count = -1;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT COUNT(*) AS count FROM " + database + "access_control_files ");

                if ((!type.Equals("")) || (!status.Equals("")))
                {
                    sb.Append(" WHERE ");
                    if (!type.Equals(""))
                    {
                        sb.Append(" type = N'" + type + "' AND");
                    }
                    if (!status.Equals(""))
                    {
                        sb.Append(" status = N'" + status + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                //MySqlCommand cmd = new SqlCommand(select, conn);
                MySqlCommand cmd;
                if (this.SqlTrans == null)
                    cmd = new MySqlCommand(select, conn);
                else
                    cmd = new MySqlCommand(select, conn, this.SqlTrans);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "AccessControlFile");
                DataTable table = dataSet.Tables["AccessControlFile"];

                if (table.Rows.Count > 0)
                {
                    count = Int32.Parse(table.Rows[0]["count"].ToString().Trim());
                }
            }
            catch
            {
                count = -1;
            }

            return count;
        }

        public ArrayList getLastIssuedACFiles()
        {
            DataSet dataSet = new DataSet();
            AccessControlFileTO accessControlFileTO = new AccessControlFileTO();
            ArrayList accessControlFileList = new ArrayList();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT acf.record_id, acf.reader_id, acf.type, acf.delay, acf.status, acf.upload_start_time, appl_users.name, acf.created_time");
                sb.Append(" FROM " + database + "access_control_files AS acf, appl_users");
                sb.Append(" WHERE ");
                sb.Append(" acf.created_time IN");
                sb.Append(" (");
                sb.Append(" SELECT MAX(created_time)");
                sb.Append(" FROM " + database + "access_control_files AS acf1");
                sb.Append(" WHERE ");
                sb.Append(" acf1.reader_id = acf.reader_id ");
                sb.Append(" AND acf1.type = acf.type");
                sb.Append(" )");
                sb.Append(" AND acf.created_by = appl_users.user_id");
                sb.Append(" ORDER BY acf.reader_id, acf.type, acf.record_id DESC");

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "AccessControlFile");
                DataTable table = dataSet.Tables["AccessControlFile"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        accessControlFileTO = new AccessControlFileTO();

                        if (!row["record_id"].Equals(DBNull.Value))
                        {
                            accessControlFileTO.RecordID = Int32.Parse(row["record_id"].ToString().Trim());
                        }
                        if (!row["reader_id"].Equals(DBNull.Value))
                        {
                            accessControlFileTO.ReaderID = Int32.Parse(row["reader_id"].ToString().Trim());
                        }
                        if (!row["type"].Equals(DBNull.Value))
                        {
                            accessControlFileTO.Type = row["type"].ToString().Trim();
                        }
                        if (!row["delay"].Equals(DBNull.Value))
                        {
                            accessControlFileTO.Delayed = Int32.Parse(row["delay"].ToString().Trim());
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            accessControlFileTO.Status = row["status"].ToString().Trim();
                        }
                        if (!row["upload_start_time"].Equals(DBNull.Value))
                        {
                            accessControlFileTO.UploadStartTime = DateTime.Parse(row["upload_start_time"].ToString());
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            accessControlFileTO.CreatedBy = row["name"].ToString().Trim();
                        }
                        if (!row["created_time"].Equals(DBNull.Value))
                        {
                            accessControlFileTO.CreatedTime = DateTime.Parse(row["created_time"].ToString());
                        }

                        accessControlFileList.Add(accessControlFileTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return accessControlFileList;
        }

        public ArrayList getLastUploadTime()
        {
            DataSet dataSet = new DataSet();
            AccessControlFileTO accessControlFileTO = new AccessControlFileTO();
            ArrayList accessControlFileList = new ArrayList();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT reader_id, type, MAX(upload_end_time) AS UploadEndTime");
                sb.Append(" FROM " + database + "access_control_files");
                sb.Append(" WHERE upload_end_time IS NOT NULL");
                sb.Append(" GROUP BY reader_id, type");

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "AccessControlFile");
                DataTable table = dataSet.Tables["AccessControlFile"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        accessControlFileTO = new AccessControlFileTO();

                        if (!row["reader_id"].Equals(DBNull.Value))
                        {
                            accessControlFileTO.ReaderID = Int32.Parse(row["reader_id"].ToString().Trim());
                        }
                        if (!row["type"].Equals(DBNull.Value))
                        {
                            accessControlFileTO.Type = row["type"].ToString().Trim();
                        }
                        if (!row["UploadEndTime"].Equals(DBNull.Value))
                        {
                            accessControlFileTO.UploadEndTime = DateTime.Parse(row["UploadEndTime"].ToString());
                        }

                        accessControlFileList.Add(accessControlFileTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return accessControlFileList;
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

        // TODO!!!
        public void serialize(ArrayList accessControlFileTOList)
        {
            try
            {
                string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLAccessControlFileFile"];
                Stream stream = File.Open(filename, FileMode.Create);

                AccessControlFileTO[] accessControlFileTOArray = (AccessControlFileTO[])accessControlFileTOList.ToArray(typeof(AccessControlFileTO));

                XmlSerializer bformatter = new XmlSerializer(typeof(AccessControlFileTO[]));
                bformatter.Serialize(stream, accessControlFileTOArray);
                stream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
