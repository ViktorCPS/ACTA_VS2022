using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using System.Globalization;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using TransferObjects;
using Util;

namespace DataAccess
{
    public class MSSQLCameraSnapshotFileDAO : CameraSnapshotFileDAO
    {
        SqlConnection conn = null;
        protected string dateTimeformat = "";
        string database = "";
        SqlTransaction _sqlTrans = null;

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLCameraSnapshotFileDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
            database = Constants.GetDatabaseString + "_files.actamgr.";
		}
        public MSSQLCameraSnapshotFileDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
            database = Constants.GetDatabaseString + "_files.actamgr.";
        }
        public int insert(string fileName, int cameraID,
            DateTime cameraCreatedTime, DateTime fileCreatedTime, byte[] content, bool doCommit)
		{
            SqlTransaction sqlTrans = null;

            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }

			int rowsAffected = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO " + database + "camera_snapshot_files ");
                sbInsert.Append("(file_name, camera_id, camera_created_time, file_created_time, content, created_by, created_time) ");
				sbInsert.Append("VALUES (");

                sbInsert.Append("N'" + fileName.Trim() + "', ");
                sbInsert.Append("" + cameraID + ", ");

                if (!cameraCreatedTime.Equals(new DateTime(0)))
                {
                    sbInsert.Append("'" + cameraCreatedTime.ToString(dateTimeformat) + "', ");
                    //sbInsert.Append("'" + cameraCreatedTime.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (!fileCreatedTime.Equals(new DateTime(0)))
                {
                    sbInsert.Append("'" + fileCreatedTime.ToString(dateTimeformat) + "', ");
                    //sbInsert.Append("'" + fileCreatedTime.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append("@Content, ");

				sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

				SqlCommand cmd = new SqlCommand( sbInsert.ToString(), conn, sqlTrans );
                cmd.Parameters.Add("@Content", SqlDbType.Image, content.Length).Value = content;

				rowsAffected = cmd.ExecuteNonQuery();
                if (doCommit)
                {
                    sqlTrans.Commit();
                }
			}
			catch(Exception ex)
			{
                if (doCommit)
                    sqlTrans.Rollback("INSERT");
                else
                    sqlTrans.Rollback();
				throw ex;
			}

			return rowsAffected;
		}

        public int insert(CameraSnapshotFileTO cameraFileTO, bool doCommit)
        {
            SqlTransaction sqlTrans = null;

            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO " + database + "camera_snapshot_files ");
                sbInsert.Append("(file_name, camera_id, camera_created_time, file_created_time, content,tag_id,event_time, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                sbInsert.Append("N'" + cameraFileTO.FileName + "', ");
                sbInsert.Append("" + cameraFileTO.CameraID + ", ");
                sbInsert.Append("'" + cameraFileTO.CameraCreatedTime.ToString(dateTimeformat) + "', ");
                sbInsert.Append("'" + cameraFileTO.FileCreatedTime.ToString(dateTimeformat) + "', ");
                sbInsert.Append("@Content, ");
                sbInsert.Append("" + cameraFileTO.TagID + ", ");
                sbInsert.Append("'" + cameraFileTO.EventTime.ToString(dateTimeformat) + "', ");

                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                cmd.Parameters.Add("@Content", SqlDbType.Image, cameraFileTO.Content.Length).Value = cameraFileTO.Content;

                rowsAffected = cmd.ExecuteNonQuery();
                if (doCommit)
                {
                    sqlTrans.Commit();
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                    sqlTrans.Rollback("INSERT");
                else
                    sqlTrans.Rollback();
                throw ex;
            }

            return rowsAffected;
        }

        /*public bool update(int readerID, string type, string oldStatus, string newStatus,
            DateTime uploadStartTime, DateTime uploadEndTime, int delay, int recordID, 
            string modifiedBy, bool doCommit)
		{
			bool isUpdated = false;
            SqlTransaction sqlTrans = null;
            string select = "";

            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");
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
                    sbUpdate.Append("upload_start_time = GETDATE(), ");
                    //sbUpdate.Append("upload_start_time = '" + uploadStartTime.ToString(dateTimeformat) + "', ");
                    //sbUpdate.Append("upload_start_time = '" + uploadStartTime.ToString("yyy-MM-dd HH:mm") + "', ");
                }
                if (!uploadEndTime.Equals(new DateTime(0)))
                {
                    sbUpdate.Append("upload_end_time = GETDATE(), ");
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
				sbUpdate.Append("modified_time = GETDATE() ");
                
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

                SqlCommand cmd = new SqlCommand(select, conn, sqlTrans);
				int res = cmd.ExecuteNonQuery();
				if(res > 0)
				{
					isUpdated = true;	
				}
                if (doCommit)
                {
                    sqlTrans.Commit();
                }
			}
			catch(Exception ex)
			{
                if (doCommit)
                    sqlTrans.Rollback("UPDATE");
                else
                    sqlTrans.Rollback();
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}*/

		/*public ExtraHourTO find(int employeeID, DateTime dateEarned)
		{
			DataSet dataSet = new DataSet();
			ExtraHourTO extraHourTO = new ExtraHourTO();
			try
			{
				SqlCommand cmd = new SqlCommand("SELECT employee_id, date_earned, extra_time_amt FROM extra_hours WHERE employee_id = " 
					+ employeeID + " AND date_earned = '" + dateEarned.ToString("yyy-MM-dd").Trim() + "'", conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

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
						extraHourTO.DateEarned = (DateTime)table.Rows[0]["date_earned"];
					}

					if (!table.Rows[0]["extra_time_amt"].Equals(DBNull.Value))
					{
						extraHourTO.ExtraTimeAmt = Int32.Parse(table.Rows[0]["extra_time_amt"].ToString().Trim());						
						extraHourTO.CalculatedTimeAmt = Util.Misc.transformMinToStringTime(extraHourTO.ExtraTimeAmt);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return extraHourTO;
		}*/

        public ArrayList getCameraSnapshotFiles(int cameraID,
            DateTime cameraCreatedTime, DateTime fileCreatedTime)
		{
			DataSet dataSet = new DataSet();
            CameraSnapshotFileTO cameraSnapshotFileTO = new CameraSnapshotFileTO();
            ArrayList cameraSnapshotFileList = new ArrayList();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
                sb.Append("SELECT record_id, camera_id, camera_created_time, file_created_time, content FROM " + database + "camera_snapshot_files ");

                if ((cameraID != -1) ||
                    (!cameraCreatedTime.Equals(new DateTime(0))) || (!fileCreatedTime.Equals(new DateTime(0))))
				{
					sb.Append(" WHERE ");

                    if (cameraID != -1)
                    {
                        sb.Append(" camera_id = " + cameraID + " AND");
                    }
                    /*if (!cameraCreatedTime.Equals(new DateTime(0)))
					{
                        //sekunde
                        sb.Append(" camera_created_time = convert(datetime,'" + cameraCreatedTime.ToString("yyy-MM-dd HH:mm:ss") + "', 120) AND"); //da li 4y
					}
                    if (!fileCreatedTime.Equals(new DateTime(0)))
					{
                        //sekunde
                        sb.Append(" file_created_time = convert(datetime,'" + fileCreatedTime.ToString("yyy-MM-dd HH:mm:ss") + "', 120) AND"); //da li 4y
					}*/

                    select = sb.ToString(0, sb.ToString().Length - 3);
				}
				else
				{
					select = sb.ToString();
				}

                select = select + " ORDER BY camera_id, file_created_time";

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "CameraSnapshotFile");
                DataTable table = dataSet.Tables["CameraSnapshotFile"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
                        cameraSnapshotFileTO = new CameraSnapshotFileTO();

                        if (!row["record_id"].Equals(DBNull.Value))
                        {
                            cameraSnapshotFileTO.RecordID = Int32.Parse(row["record_id"].ToString().Trim());							
                        }
                        if (!row["camera_id"].Equals(DBNull.Value))
                        {
                            cameraSnapshotFileTO.CameraID = Int32.Parse(row["camera_id"].ToString().Trim());
                        }
                        if (!row["camera_created_time"].Equals(DBNull.Value))
						{
                            cameraSnapshotFileTO.CameraCreatedTime = (DateTime)row["camera_created_time"];
						}
                        if (!row["file_created_time"].Equals(DBNull.Value))
                        {
                            cameraSnapshotFileTO.FileCreatedTime = (DateTime)row["file_created_time"];
                        }
                        if (!row["content"].Equals(DBNull.Value))
                        {
                            cameraSnapshotFileTO.Content = (byte[])row["content"];
                        }

                        cameraSnapshotFileList.Add(cameraSnapshotFileTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

            return cameraSnapshotFileList;
		}

        public ArrayList getCamSnapshotFilesForDates(DateTime DateFrom, DateTime DateTo)
        {
            DataSet dataSet = new DataSet();
            CameraSnapshotFileTO cameraSnapshotFileTO = new CameraSnapshotFileTO();
            ArrayList cameraSnapshotFileList = new ArrayList();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT record_id, file_name, camera_id, camera_created_time, file_created_time, content FROM " + database + "camera_snapshot_files ");
                sb.Append(" WHERE");
                if (!DateFrom.Equals(new DateTime(0)))
                {

                    sb.Append(" file_created_time >= convert(datetime,'" + DateFrom.ToString("yyy-MM-dd") + "', 120) AND"); //da li 4y
                }

                if (!DateTo.Equals(new DateTime(0)))
                {

                    sb.Append(" file_created_time <= convert(datetime,'" + DateTo.AddDays(1).ToString("yyy-MM-dd") + "', 120) "); //da li 4y
                    select = sb.ToString();
                }
                else
                {
                    select = sb.ToString().Substring(0,sb.Length-3);
                }                  
                               
                select = select + " ORDER BY camera_id, file_created_time";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "CameraSnapshotFile");
                DataTable table = dataSet.Tables["CameraSnapshotFile"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        cameraSnapshotFileTO = new CameraSnapshotFileTO();

                        if (!row["record_id"].Equals(DBNull.Value))
                        {
                            cameraSnapshotFileTO.RecordID = Int32.Parse(row["record_id"].ToString().Trim());
                        }
                        if (!row["file_name"].Equals(DBNull.Value))
                        {
                            cameraSnapshotFileTO.FileName = row["file_name"].ToString().Trim();
                        }
                        if (!row["camera_id"].Equals(DBNull.Value))
                        {
                            cameraSnapshotFileTO.CameraID = Int32.Parse(row["camera_id"].ToString().Trim());
                        }
                        if (!row["camera_created_time"].Equals(DBNull.Value))
                        {
                            cameraSnapshotFileTO.CameraCreatedTime = (DateTime)row["camera_created_time"];
                        }
                        if (!row["file_created_time"].Equals(DBNull.Value))
                        {
                            cameraSnapshotFileTO.FileCreatedTime = (DateTime)row["file_created_time"];
                        }
                        if (!row["content"].Equals(DBNull.Value))
                        {
                            cameraSnapshotFileTO.Content = (byte[])row["content"];
                        }

                        cameraSnapshotFileList.Add(cameraSnapshotFileTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return cameraSnapshotFileList;
        }

        public int getCameraSnapshotFilesCount(int cameraID)
        {
            DataSet dataSet = new DataSet();
            int count = -1;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT COUNT(*) AS count FROM " + database + "camera_snapshot_files ");

                if (cameraID != -1)
                {
                    sb.Append(" WHERE ");
                    sb.Append(" camera_id = " + cameraID);
                }

                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "CameraSnapshotFile");
                DataTable table = dataSet.Tables["CameraSnapshotFile"];

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

        public ArrayList getCSFilesForPass(int passID, DateTime fromDate, DateTime toDate, string direction)
        {
            DataSet dataSet = new DataSet();
            CameraSnapshotFileTO cameraSnapshotFileTO = new CameraSnapshotFileTO();
            ArrayList cameraSnapshotFileList = new ArrayList();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT csf.record_id, csf.camera_created_time, csf.file_created_time");
                sb.Append(" FROM passes, tags, log, cameras_x_readers cxr, " + database + "camera_snapshot_files csf");
                sb.Append(" WHERE");
                sb.Append(" passes.pass_id = " + passID);
                sb.Append(" AND passes.employee_id = tags.owner_id");
                sb.Append(" AND log.tag_id = tags.tag_id");
                sb.Append(" AND log.event_time = passes.event_time");
                sb.Append(" AND cxr.reader_id = log.reader_id");
                if (!direction.Equals(""))
                {
                    sb.Append(" AND cxr.direction_covered IN (" + direction.ToString().Trim() + ")");
                }
                sb.Append(" AND csf.camera_id = cxr.camera_id");
                sb.Append(" AND csf.file_created_time >= convert(datetime,'" + fromDate.ToString("yyyy-MM-dd HH:mm:ss") + "', 120)");
                sb.Append(" AND csf.file_created_time <= convert(datetime,'" + toDate.ToString("yyyy-MM-dd HH:mm:ss") + "', 120)");
                sb.Append(" ORDER BY csf.file_created_time");

                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "CameraSnapshotFile");
                DataTable table = dataSet.Tables["CameraSnapshotFile"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        cameraSnapshotFileTO = new CameraSnapshotFileTO();

                        if (!row["record_id"].Equals(DBNull.Value))
                        {
                            cameraSnapshotFileTO.RecordID = Int32.Parse(row["record_id"].ToString().Trim());
                        }
                        if (!row["camera_created_time"].Equals(DBNull.Value))
                        {
                            cameraSnapshotFileTO.CameraCreatedTime = (DateTime)row["camera_created_time"];
                        }
                        if (!row["file_created_time"].Equals(DBNull.Value))
                        {
                            cameraSnapshotFileTO.FileCreatedTime = (DateTime)row["file_created_time"];
                        }

                        cameraSnapshotFileList.Add(cameraSnapshotFileTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return cameraSnapshotFileList;
        }

        public ArrayList getCameraSnapshotFiles(string cameraID,  DateTime dateFrom, DateTime dateTo, DateTime timeFrom, DateTime timeTo)
        {
            DataSet dataSet = new DataSet();
            CameraSnapshotFileTO cameraSnapshotFileTO = new CameraSnapshotFileTO();
            ArrayList cameraSnapshotFileList = new ArrayList();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT record_id, camera_id, camera_created_time, file_created_time, content FROM " + database + "camera_snapshot_files ");

                if ((!cameraID.Equals("")) || 
                     (!dateFrom.Equals(new DateTime(0))) || (!dateTo.Equals(new DateTime(0))) || (!timeFrom.Equals(new DateTime(0))) || (!timeTo.Equals(new DateTime(0))))
                {
                    sb.Append(" WHERE ");

                    if (!cameraID.Equals(""))
                    {
                        sb.Append(" camera_id IN ( " + cameraID + ") AND");
                    }
                    if (!dateFrom.Equals(new DateTime(0)) && !dateTo.Equals(new DateTime(0)))
                    {
                        //sekunde
                        sb.Append(" file_created_time >= convert(datetime,'" + dateFrom.ToString("yyy-MM-dd") + "', 111) AND");
                        sb.Append(" file_created_time <= convert(datetime,'" + dateTo.AddDays(1).ToString("yyy-MM-dd") + "', 111) AND");
                    }
                    if (!timeFrom.Equals(new DateTime(0)) && !timeTo.Equals(new DateTime(0)))
                    {
                        //sekunde
                        sb.Append(" CONVERT(datetime, CONVERT(varchar(10), file_created_time, 108), 108) >= CONVERT(datetime,'" + timeFrom.ToString("HH:mm:ss") + "', 108) AND ");
                        sb.Append(" CONVERT(datetime, CONVERT(varchar(10), file_created_time, 108), 108) <= CONVERT(datetime,'" + timeTo.ToString("HH:mm:ss") + "', 108) AND ");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 4);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + " ORDER BY camera_id, file_created_time";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "CameraSnapshotFile");
                DataTable table = dataSet.Tables["CameraSnapshotFile"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        cameraSnapshotFileTO = new CameraSnapshotFileTO();

                        if (!row["record_id"].Equals(DBNull.Value))
                        {
                            cameraSnapshotFileTO.RecordID = Int32.Parse(row["record_id"].ToString().Trim());
                        }
                        if (!row["camera_id"].Equals(DBNull.Value))
                        {
                            cameraSnapshotFileTO.CameraID = Int32.Parse(row["camera_id"].ToString().Trim());
                        }
                        if (!row["camera_created_time"].Equals(DBNull.Value))
                        {
                            cameraSnapshotFileTO.CameraCreatedTime = (DateTime)row["camera_created_time"];
                        }
                        if (!row["file_created_time"].Equals(DBNull.Value))
                        {
                            cameraSnapshotFileTO.FileCreatedTime = (DateTime)row["file_created_time"];
                        }
                        if (!row["content"].Equals(DBNull.Value))
                        {
                            cameraSnapshotFileTO.Content = (byte[])row["content"];
                        }

                        cameraSnapshotFileList.Add(cameraSnapshotFileTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return cameraSnapshotFileList;
        }

        public ArrayList getCSFilesForPassDisplay(string recordID)
        {
            DataSet dataSet = new DataSet();
            CameraSnapshotFileTO cameraSnapshotFileTO = new CameraSnapshotFileTO();
            ArrayList cameraSnapshotFileList = new ArrayList();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT record_id, file_created_time, content, camera_id FROM " + database + "camera_snapshot_files ");

                if (!recordID.Equals(""))
                {
                    sb.Append(" WHERE ");
                    sb.Append(" record_id IN (" + recordID.ToString().Trim() + ") ");
                }
                sb.Append(" ORDER BY file_created_time");

                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "CameraSnapshotFile");
                DataTable table = dataSet.Tables["CameraSnapshotFile"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        cameraSnapshotFileTO = new CameraSnapshotFileTO();

                        if (!row["record_id"].Equals(DBNull.Value))
                        {
                            cameraSnapshotFileTO.RecordID = Int32.Parse(row["record_id"].ToString().Trim());
                        }
                        if (!row["file_created_time"].Equals(DBNull.Value))
                        {
                            cameraSnapshotFileTO.FileCreatedTime = (DateTime)row["file_created_time"];
                        }
                        if (!row["content"].Equals(DBNull.Value))
                        {
                            cameraSnapshotFileTO.Content = (byte[])row["content"];
                        }
                        if (!row["camera_id"].Equals(DBNull.Value))
                        {
                            cameraSnapshotFileTO.CameraID = Int32.Parse(row["camera_id"].ToString().Trim());
                        }
                        cameraSnapshotFileList.Add(cameraSnapshotFileTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return cameraSnapshotFileList;
        }

        public bool DeleteUntilDate(DateTime fileCreatedTime, bool doCommit)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = null;

            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }
            DataSet dataSet = new DataSet();
            CameraSnapshotFileTO cameraSnapshotFileTO = new CameraSnapshotFileTO();
            ArrayList cameraSnapshotFileList = new ArrayList();
            string delete = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("DELETE FROM " + database + "camera_snapshot_files ");

                if (!fileCreatedTime.Equals(new DateTime(0)))
                {
                    //sekunde
                    sb.Append(" WHERE");
                    sb.Append(" file_created_time <= '" + fileCreatedTime.AddDays(1).ToString("yyy-MM-dd") + "' "); //da li 4y
                }

                delete = sb.ToString();


                SqlCommand cmd = new SqlCommand(delete, conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    isDeleted = true;
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

            return isDeleted;
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

		// TODO!!!
        public void serialize(ArrayList cameraSnapshotFileTOList)
		{
			try
			{
                string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLCameraSnapshotFileFile"];
				Stream stream = File.Open(filename, FileMode.Create);

                CameraSnapshotFileTO[] cameraSnapshotFileTOArray = (CameraSnapshotFileTO[])cameraSnapshotFileTOList.ToArray(typeof(CameraSnapshotFileTO));

                XmlSerializer bformatter = new XmlSerializer(typeof(CameraSnapshotFileTO[]));
                bformatter.Serialize(stream, cameraSnapshotFileTOArray);
				stream.Close();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
    }
}
