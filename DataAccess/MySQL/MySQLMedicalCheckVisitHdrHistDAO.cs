using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Data;
using MySql.Data.MySqlClient;

using TransferObjects;

namespace DataAccess
{
    public class MySQLMedicalCheckVisitHdrHistDAO : MedicalCheckVisitHdrHistDAO
    {
        MySqlConnection conn = null;
        MySqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MySQLMedicalCheckVisitHdrHistDAO()
        {
            conn = MySQLDAOFactory.getConnection();
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MySQLMedicalCheckVisitHdrHistDAO(MySqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public bool insert(MedicalCheckVisitHdrHistTO hdrTO, bool doCommit)
        {
            bool saved = true;

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

                DataSet dataSet = new DataSet();

                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO medical_chk_visits_hdr_hist ");
                sbInsert.Append("(visit_id, employee_id, schedule_date, point_id, status, flag_email, flag_email_created_time, created_by, created_time, modified_by, modified_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("'" + hdrTO.VisitID.ToString().Trim() + "', ");
                sbInsert.Append("'" + hdrTO.EmployeeID.ToString().Trim() + "', ");
                sbInsert.Append("'" + hdrTO.ScheduleDate.ToString(dateTimeformat) + "', ");
                sbInsert.Append("'" + hdrTO.PointID.ToString().Trim() + "', ");
                sbInsert.Append("N'" + hdrTO.Status.Trim() + "', ");
                sbInsert.Append("'" + hdrTO.FlagEmail.ToString().Trim() + "', ");
                if (!hdrTO.FlagEmailCratedTime.Equals(new DateTime()))
                    sbInsert.Append("'" + hdrTO.FlagEmailCratedTime.ToString(dateTimeformat) + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (!hdrTO.CreatedBy.Trim().Equals(""))
                    sbInsert.Append("N'" + hdrTO.CreatedBy.Trim() + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (!hdrTO.CreatedTime.Equals(new DateTime()))
                    sbInsert.Append("'" + hdrTO.CreatedTime.ToString(dateTimeformat) + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (!hdrTO.ModifiedBy.Trim().Equals(""))
                    sbInsert.Append("N'" + hdrTO.ModifiedBy.Trim() + "', ");
                else
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!hdrTO.ModifiedTime.Equals(new DateTime()))
                    sbInsert.Append("'" + hdrTO.ModifiedTime.ToString(dateTimeformat) + "') ");
                else
                    sbInsert.Append("NOW()) ");
                sbInsert.Append("SELECT LAST_INSERT_ID() AS rec_id ");

                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, SqlTrans);

                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);
                sqlDataAdapter.Fill(dataSet, "RecID");
                DataTable table = dataSet.Tables["RecID"];

                uint id = 0;
                if (table.Rows.Count > 0)
                {
                    if (!uint.TryParse(((DataRow)table.Rows[0])["rec_id"].ToString(), out id))
                        id = 0;
                }

                int rowsAffected = 0;
                if (id > 0)
                {
                    foreach (MedicalCheckVisitDtlHistTO dtlTO in hdrTO.VisitDetails)
                    {
                        dtlTO.RecID = id;
                        cmd.CommandText = prepareDetailInsert(dtlTO);
                        rowsAffected += cmd.ExecuteNonQuery();
                    }

                    if (rowsAffected != hdrTO.VisitDetails.Count)
                        saved = false;
                }
                else
                    saved = false;

                if (doCommit)
                {
                    if (saved)
                        SqlTrans.Commit();
                    else
                        SqlTrans.Rollback();
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                    SqlTrans.Rollback();
                throw ex;
            }

            return saved;
        }

        private string prepareDetailInsert(MedicalCheckVisitDtlHistTO dtlTO)
        {
            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO medical_chk_visits_dtl_hist ");
                sbInsert.Append("(rec_id, visit_id, check_id, type, result, date_performed, created_by, created_time, modified_by, modified_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("'" + dtlTO.RecID.ToString().Trim() + "', ");
                sbInsert.Append("'" + dtlTO.VisitID.ToString().Trim() + "', ");
                sbInsert.Append("'" + dtlTO.CheckID.ToString().Trim() + "', ");
                sbInsert.Append("N'" + dtlTO.Type.Trim() + "', ");
                if (!dtlTO.Result.Trim().Equals(""))
                    sbInsert.Append("N'" + dtlTO.Result.Trim() + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (!dtlTO.DatePerformed.Equals(new DateTime()))
                    sbInsert.Append("'" + dtlTO.DatePerformed.ToString(dateTimeformat) + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (!dtlTO.CreatedBy.Trim().Equals(""))
                    sbInsert.Append("N'" + dtlTO.CreatedBy.Trim() + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (!dtlTO.CreatedTime.Equals(new DateTime()))
                    sbInsert.Append("'" + dtlTO.CreatedTime.ToString(dateTimeformat) + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (!dtlTO.ModifiedBy.Trim().Equals(""))
                    sbInsert.Append("N'" + dtlTO.ModifiedBy.Trim() + "', ");
                else
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!dtlTO.ModifiedTime.Equals(new DateTime()))
                    sbInsert.Append("'" + dtlTO.ModifiedTime.ToString(dateTimeformat) + "') ");
                else
                    sbInsert.Append("NOW()) ");

                return sbInsert.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool update(MedicalCheckVisitHdrHistTO hdrTO, bool doCommit)
        {
            bool isUpdated = false;

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE medical_chk_visits_hdr_hist SET ");
                if (hdrTO.VisitID != 0)
                    sbUpdate.Append("visit_id = '" + hdrTO.VisitID.ToString().Trim() + "', ");
                if (hdrTO.EmployeeID != -1)
                    sbUpdate.Append("employee_id = '" + hdrTO.EmployeeID.ToString().Trim() + "', ");
                if (!hdrTO.ScheduleDate.Equals(new DateTime()))
                    sbUpdate.Append("schedule_date = '" + hdrTO.ScheduleDate.ToString(dateTimeformat) + "', ");
                if (hdrTO.PointID != -1)
                    sbUpdate.Append("point_id = '" + hdrTO.PointID.ToString().Trim() + "', ");
                if (!hdrTO.Status.Trim().Equals(""))
                    sbUpdate.Append("status = N'" + hdrTO.Status.Trim() + "', ");
                if (hdrTO.FlagEmail != -1)
                    sbUpdate.Append("flag_email = '" + hdrTO.FlagEmail.ToString().Trim() + "', ");
                if (!hdrTO.FlagEmailCratedTime.Equals(new DateTime()))
                    sbUpdate.Append("flag_email_created_time = '" + hdrTO.FlagEmailCratedTime.ToString(dateTimeformat) + "', ");
                else
                    sbUpdate.Append("flag_email_created_time = NULL, ");
                if (!hdrTO.ModifiedBy.Trim().Equals(""))
                    sbUpdate.Append("modified_by = N'" + hdrTO.ModifiedBy.Trim() + "', ");
                else
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!hdrTO.ModifiedTime.Equals(new DateTime()))
                    sbUpdate.Append("modified_time = '" + hdrTO.ModifiedTime.ToString(dateTimeformat) + "' ");
                else
                    sbUpdate.Append("modified_time = NOW() ");
                sbUpdate.Append("WHERE rec_id = '" + hdrTO.RecID.ToString().Trim() + "'");

                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, SqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res >= 0)
                {
                    isUpdated = true;
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

            return isUpdated;
        }

        public bool delete(uint recID, bool doCommit)
        {
            bool isDeleted = false;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM medical_chk_visits_hdr_hist WHERE rec_id = '" + recID.ToString().Trim() + "'");

                MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, SqlTrans);
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

        public List<MedicalCheckVisitHdrHistTO> getMedicalCheckVisitHeadersHistory(MedicalCheckVisitHdrHistTO hdrTO)
        {
            DataSet dataSet = new DataSet();
            MedicalCheckVisitHdrHistTO hdr = new MedicalCheckVisitHdrHistTO();
            List<MedicalCheckVisitHdrHistTO> hdrList = new List<MedicalCheckVisitHdrHistTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM medical_chk_visits_hdr_hist ");
                if ((hdrTO.RecID != 0) || (hdrTO.VisitID != 0) || (hdrTO.EmployeeID != -1) || (!hdrTO.ScheduleDate.Equals(new DateTime())) || (hdrTO.PointID != -1)
                    || (!hdrTO.Status.Trim().Equals("")) || (hdrTO.FlagEmail != -1) || (!hdrTO.FlagEmailCratedTime.Equals(new DateTime())))
                {
                    sb.Append(" WHERE");

                    if (hdrTO.RecID != 0)
                    {
                        sb.Append(" rec_id = '" + hdrTO.RecID.ToString().Trim() + "' AND");
                    }
                    if (hdrTO.VisitID != 0)
                    {
                        sb.Append(" visit_id = '" + hdrTO.VisitID.ToString().Trim() + "' AND");
                    }
                    if (hdrTO.EmployeeID != -1)
                    {
                        sb.Append(" employee_id = '" + hdrTO.EmployeeID.ToString().Trim() + "' AND");
                    }
                    if (!hdrTO.ScheduleDate.Equals(new DateTime()))
                    {
                        sb.Append(" schedule_date = '" + hdrTO.ScheduleDate.ToString(dateTimeformat) + "' AND");
                    }
                    if (hdrTO.PointID != -1)
                    {
                        sb.Append(" point_id = '" + hdrTO.PointID.ToString().Trim() + "' AND");
                    }
                    if (!hdrTO.Status.Trim().Equals(""))
                    {
                        sb.Append(" status = N'" + hdrTO.Status.Trim().ToString() + "' AND");
                    }
                    if (hdrTO.FlagEmail != -1)
                    {
                        sb.Append(" flag_email = '" + hdrTO.FlagEmail.ToString().Trim() + "' AND");
                    }
                    if (!hdrTO.FlagEmailCratedTime.Equals(new DateTime()))
                    {
                        sb.Append(" flag_email_created_time = '" + hdrTO.FlagEmailCratedTime.ToString(dateTimeformat) + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                MySqlCommand cmd = new MySqlCommand(select + " ORDER BY visit_id, modified_time", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visits");
                DataTable table = dataSet.Tables["Visits"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        hdr = new MedicalCheckVisitHdrHistTO();
                        if (row["rec_id"] != DBNull.Value)
                        {
                            hdr.RecID = UInt32.Parse(row["rec_id"].ToString().Trim());
                        }
                        if (row["visit_id"] != DBNull.Value)
                        {
                            hdr.VisitID = UInt32.Parse(row["visit_id"].ToString().Trim());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            hdr.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["schedule_date"] != DBNull.Value)
                        {
                            hdr.ScheduleDate = DateTime.Parse(row["schedule_date"].ToString().Trim());
                        }
                        if (row["point_id"] != DBNull.Value)
                        {
                            hdr.PointID = Int32.Parse(row["point_id"].ToString().Trim());
                        }
                        if (row["status"] != DBNull.Value)
                        {
                            hdr.Status = row["status"].ToString().Trim();
                        }
                        if (row["flag_email"] != DBNull.Value)
                        {
                            hdr.FlagEmail = Int32.Parse(row["flag_email"].ToString().Trim());
                        }
                        if (row["flag_email_created_time"] != DBNull.Value)
                        {
                            hdr.FlagEmailCratedTime = DateTime.Parse(row["flag_email_created_time"].ToString().Trim());
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            hdr.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            hdr.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            hdr.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            hdr.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }

                        hdrList.Add(hdr);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return hdrList;
        }

        public DataTable getMedicalCheckVisitHeadersHistory(uint visitID)
        {
            DataSet dataSet = new DataSet();
            DataTable hdrTable = new DataTable();

            string select = "";

            try
            {
                if (visitID.ToString().Trim().Equals(""))
                    return hdrTable;

                select = "SELECT v.rec_id AS rec_id, v.schedule_date AS schedule_date, p.description AS point, v.status AS status, u.name AS user_name, v.modified_time AS mod_time "
                    + "FROM medical_chk_visits_hdr_hist v, medical_chk_points p, appl_users u "
                    + "WHERE v.point_id = p.point_id AND v.modified_by = u.user_id AND v.visit_id = '" + visitID.ToString().Trim() + "' ORDER BY v.modified_time DESC";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visits");
                hdrTable = dataSet.Tables["Visits"];
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return hdrTable;
        }

        public List<uint> getMedicalCheckVisitHeadersHistory(string visitIDs)
        {
            DataSet dataSet = new DataSet();
            DataTable hdrTable = new DataTable();
            List<uint> idList = new List<uint>();

            string select = "";

            try
            {
                if (visitIDs.Trim().Equals(""))
                    return idList;

                select = "SELECT DISTINCT visit_id FROM medical_chk_visits_hdr_hist WHERE visit_id IN (" + visitIDs.Trim() + ")";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visits");
                hdrTable = dataSet.Tables["Visits"];

                if (hdrTable.Rows.Count > 0)
                {
                    foreach (DataRow row in hdrTable.Rows)
                    {
                        if (row["visit_id"] != DBNull.Value)
                        {
                            idList.Add(uint.Parse(row["visit_id"].ToString().Trim()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return idList;
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