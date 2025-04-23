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
    public class MSSQLMedicalCheckVisitHdrDAO : MedicalCheckVisitHdrDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLMedicalCheckVisitHdrDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MSSQLMedicalCheckVisitHdrDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public bool insert(MedicalCheckVisitHdrTO hdrTO, bool doCommit)
        {
            bool saved = true;

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

                DataSet dataSet = new DataSet();

                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("SET NOCOUNT ON ");
                sbInsert.Append("INSERT INTO medical_chk_visits_hdr ");
                sbInsert.Append("(employee_id, schedule_date, point_id, status, flag_email, flag_email_created_time, flag_change, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("'" + hdrTO.EmployeeID.ToString().Trim() + "', ");
                sbInsert.Append("'" + hdrTO.ScheduleDate.ToString(dateTimeformat) + "', ");
                sbInsert.Append("'" + hdrTO.PointID.ToString().Trim() + "', ");
                sbInsert.Append("N'" + hdrTO.Status.Trim() + "', ");
                sbInsert.Append("'" + hdrTO.FlagEmail.ToString().Trim() + "', ");
                if (!hdrTO.FlagEmailCratedTime.Equals(new DateTime()))
                    sbInsert.Append("'" + hdrTO.FlagEmailCratedTime.ToString(dateTimeformat) + "', ");
                else
                    sbInsert.Append("NULL, ");
                sbInsert.Append("'" + hdrTO.FlagChange.ToString().Trim() + "', ");
                if (!hdrTO.CreatedBy.Trim().Equals(""))
                    sbInsert.Append("N'" + hdrTO.CreatedBy.Trim() + "', ");
                else
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!hdrTO.CreatedTime.Equals(new DateTime()))
                    sbInsert.Append("'" + hdrTO.CreatedTime.ToString(dateTimeformat) + "') ");
                else
                    sbInsert.Append("GETDATE()) ");
                sbInsert.Append("SELECT @@Identity AS visit_id ");
                sbInsert.Append("SET NOCOUNT OFF ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, SqlTrans);
                
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                sqlDataAdapter.Fill(dataSet, "VisitID");
                DataTable table = dataSet.Tables["VisitID"];

                uint id = 0;
                if (table.Rows.Count > 0)
                {
                    if (!uint.TryParse(((DataRow)table.Rows[0])["visit_id"].ToString(), out id))
                        id = 0;
                }
                
                int rowsAffected = 0;
                if (id > 0)
                {
                    foreach (MedicalCheckVisitDtlTO dtlTO in hdrTO.VisitDetails)
                    {
                        dtlTO.VisitID = id;                        
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

            return saved;
        }

        private string prepareDetailInsert(MedicalCheckVisitDtlTO dtlTO)
        {
            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO medical_chk_visits_dtl ");
                sbInsert.Append("(visit_id, check_id, type, result, date_performed, created_by, created_time) ");
                sbInsert.Append("VALUES (");
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
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!dtlTO.CreatedTime.Equals(new DateTime()))
                    sbInsert.Append("'" + dtlTO.CreatedTime.ToString(dateTimeformat) + "') ");
                else
                    sbInsert.Append("GETDATE()) ");

                return sbInsert.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool update(MedicalCheckVisitHdrTO hdrTO, bool doCommit)
        {
            bool isUpdated = false;

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE medical_chk_visits_hdr SET ");
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
                if (hdrTO.FlagChange != -1)
                    sbUpdate.Append("flag_change = '" + hdrTO.FlagChange.ToString().Trim() + "', ");
                if (!hdrTO.ModifiedBy.Trim().Equals(""))
                    sbUpdate.Append("modified_by = N'" + hdrTO.ModifiedBy.Trim() + "', ");
                else
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!hdrTO.ModifiedTime.Equals(new DateTime()))
                    sbUpdate.Append("modified_time = '" + hdrTO.ModifiedTime.ToString(dateTimeformat) + "' ");
                else
                    sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE visit_id = '" + hdrTO.VisitID.ToString().Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, SqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res >= 0)
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

        public bool delete(string visitID, bool doCommit)
        {
            bool isDeleted = false;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM medical_chk_visits_dtl WHERE visit_id IN (" + visitID.ToString().Trim() + ");");
                sbDelete.Append("DELETE FROM medical_chk_visits_hdr WHERE visit_id IN (" + visitID.ToString().Trim() + ");");

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, SqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res >= 0)
                {
                    isDeleted = true;
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

            return isDeleted;
        }

        public bool deleteEmptyVisits(string visitIDs, bool doCommit)
        {
            bool isDeleted = false;

            if (visitIDs.Trim().Length <= 0)
                return true;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM medical_chk_visits_hdr WHERE visit_id IN (" + visitIDs.Trim() + ") AND visit_id NOT IN ");
                sbDelete.Append("(SELECT DISTINCT visit_id FROM medical_chk_visits_dtl WHERE visit_id IN (" + visitIDs.Trim() + "))");

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, SqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res >= 0)
                {
                    isDeleted = true;
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

            return isDeleted;
        }

        public List<MedicalCheckVisitHdrTO> getEmptyVisits(string visitIDs)
        {
            DataSet dataSet = new DataSet();
            MedicalCheckVisitHdrTO hdr = new MedicalCheckVisitHdrTO();
            List<MedicalCheckVisitHdrTO> hdrList = new List<MedicalCheckVisitHdrTO>();
            string select = "";

            try
            {                
                select = "SELECT * FROM medical_chk_visits_hdr WHERE visit_id IN (" + visitIDs.Trim() + ") AND visit_id NOT IN " 
                    + "(SELECT DISTINCT visit_id FROM medical_chk_visits_dtl WHERE visit_id IN (" + visitIDs.Trim() + "))";
                
                SqlCommand cmd;
                if (SqlTrans != null)
                    cmd = new SqlCommand(select + " ORDER BY employee_id, schedule_date", conn, SqlTrans);
                else
                    cmd = new SqlCommand(select + " ORDER BY employee_id, schedule_date", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visits");
                DataTable table = dataSet.Tables["Visits"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        hdr = new MedicalCheckVisitHdrTO();
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
                        if (row["flag_change"] != DBNull.Value)
                        {
                            hdr.FlagChange = Int32.Parse(row["flag_change"].ToString().Trim());
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

        public List<MedicalCheckVisitHdrTO> getMedicalCheckVisitHeaders(MedicalCheckVisitHdrTO hdrTO)
        {
            DataSet dataSet = new DataSet();
            MedicalCheckVisitHdrTO hdr = new MedicalCheckVisitHdrTO();
            List<MedicalCheckVisitHdrTO> hdrList = new List<MedicalCheckVisitHdrTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM medical_chk_visits_hdr ");
                if ((hdrTO.VisitID != 0) || (hdrTO.EmployeeID != -1) || (!hdrTO.ScheduleDate.Equals(new DateTime())) || (hdrTO.PointID != -1)
                    || (!hdrTO.Status.Trim().Equals("")) || (hdrTO.FlagEmail != -1) || (!hdrTO.FlagEmailCratedTime.Equals(new DateTime())) || (hdrTO.FlagChange != -1))
                {
                    sb.Append(" WHERE");

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
                    if (hdrTO.FlagChange != -1)
                    {
                        sb.Append(" flag_change = '" + hdrTO.FlagChange.ToString().Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                SqlCommand cmd = new SqlCommand(select + " ORDER BY employee_id, schedule_date", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visits");
                DataTable table = dataSet.Tables["Visits"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        hdr = new MedicalCheckVisitHdrTO();
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
                        if (row["flag_change"] != DBNull.Value)
                        {
                            hdr.FlagChange = Int32.Parse(row["flag_change"].ToString().Trim());
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

        public Dictionary<uint, MedicalCheckVisitHdrTO> getMedicalCheckVisits(string emplIDs, string status, string point, string check, string type, DateTime from, DateTime to)
        {
            DataSet dataSet = new DataSet();
            DataSet dataSetDtl = new DataSet();
            MedicalCheckVisitHdrTO hdr = new MedicalCheckVisitHdrTO();
            MedicalCheckVisitDtlTO dtl = new MedicalCheckVisitDtlTO();
            Dictionary<uint, MedicalCheckVisitHdrTO> hdrList = new Dictionary<uint, MedicalCheckVisitHdrTO>();
            Dictionary<uint, List<MedicalCheckVisitDtlTO>> dtlList = new Dictionary<uint, List<MedicalCheckVisitDtlTO>>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("FROM medical_chk_visits_hdr ");
                if ((!emplIDs.Trim().Equals("")) || (!status.Trim().Equals("")) || (!point.Trim().Equals("")) || ((!check.Trim().Equals("")) && (!type.Trim().Equals(""))) 
                    || (!from.Equals(new DateTime())) || (!to.Equals(new DateTime())))
                {
                    sb.Append(" WHERE");

                    if (!emplIDs.Trim().Equals(""))
                    {
                        sb.Append(" employee_id IN (" + emplIDs.Trim() + ") AND");
                    }
                    if (!status.Trim().Equals(""))
                    {
                        sb.Append(" status IN (" + status.Trim() + ") AND");
                    }
                    if (!point.Trim().Equals(""))
                    {
                        sb.Append(" point_id IN (" + point.Trim() + ") AND");
                    }
                    if (!from.Equals(new DateTime()))
                    {
                        sb.Append(" (schedule_date >= '" + from.Date.ToString(dateTimeformat) + "' OR schedule_date = '" + Constants.dateTimeNullValue().ToString(dateTimeformat) + "') AND");
                    }
                    if (!to.Equals(new DateTime()))
                    {
                        sb.Append(" (schedule_date < '" + to.Date.AddDays(1).ToString(dateTimeformat) + "' OR schedule_date = '" + Constants.dateTimeNullValue().ToString(dateTimeformat) + "') AND");
                    }
                    if (!check.Trim().Equals("") && (!type.Trim().Equals("")))
                    {
                        sb.Append(" visit_id IN (SELECT visit_id FROM medical_chk_visits_dtl WHERE");                        
                        sb.Append(" check_id IN (" + check.Trim() + ") AND");
                        sb.Append(" type = '" + type.Trim() + "'");
                        sb.Append(") AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                // get visit details
                string selectDtl = "SELECT * FROM medical_chk_visits_dtl WHERE visit_id IN (SELECT visit_id " + select + ")";
                SqlCommand cmdDtl = new SqlCommand(selectDtl, conn);
                SqlDataAdapter sqlDataAdapterDtl = new SqlDataAdapter(cmdDtl);

                sqlDataAdapterDtl.Fill(dataSetDtl, "VisitsDtl");
                DataTable tableDtl = dataSetDtl.Tables["VisitsDtl"];

                if (tableDtl.Rows.Count > 0)
                {
                    foreach (DataRow row in tableDtl.Rows)
                    {
                        dtl = new MedicalCheckVisitDtlTO();
                        if (row["rec_id"] != DBNull.Value)
                        {
                            dtl.RecID = UInt32.Parse(row["rec_id"].ToString().Trim());
                        }
                        if (row["visit_id"] != DBNull.Value)
                        {
                            dtl.VisitID = UInt32.Parse(row["visit_id"].ToString().Trim());
                        }
                        if (row["check_id"] != DBNull.Value)
                        {
                            dtl.CheckID = Int32.Parse(row["check_id"].ToString().Trim());
                        }
                        if (row["type"] != DBNull.Value)
                        {
                            dtl.Type = row["type"].ToString().Trim();
                        }
                        if (row["result"] != DBNull.Value)
                        {
                            dtl.Result = row["result"].ToString().Trim();
                        }
                        if (row["date_performed"] != DBNull.Value)
                        {
                            dtl.DatePerformed = DateTime.Parse(row["date_performed"].ToString().Trim());
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            dtl.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            dtl.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            dtl.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            dtl.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }

                        if (!dtlList.ContainsKey(dtl.VisitID))
                            dtlList.Add(dtl.VisitID, new List<MedicalCheckVisitDtlTO>());

                        dtlList[dtl.VisitID].Add(dtl);
                    }
                }

                SqlCommand cmd = new SqlCommand("SELECT * " + select + " ORDER BY employee_id, schedule_date", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visits");
                DataTable table = dataSet.Tables["Visits"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        hdr = new MedicalCheckVisitHdrTO();
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
                        if (row["flag_change"] != DBNull.Value)
                        {
                            hdr.FlagChange = Int32.Parse(row["flag_change"].ToString().Trim());
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

                        if (dtlList.ContainsKey(hdr.VisitID))
                            hdr.VisitDetails = dtlList[hdr.VisitID];

                        if (!hdrList.ContainsKey(hdr.VisitID))
                            hdrList.Add(hdr.VisitID, hdr);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return hdrList;
        }

        public List<MedicalCheckVisitHdrTO> getMedicalCheckVisits(string visitIDs)
        {
            DataSet dataSet = new DataSet();
            DataSet dataSetDtl = new DataSet();
            MedicalCheckVisitHdrTO hdr = new MedicalCheckVisitHdrTO();
            MedicalCheckVisitDtlTO dtl = new MedicalCheckVisitDtlTO();
            List<MedicalCheckVisitHdrTO> hdrList = new List<MedicalCheckVisitHdrTO>();
            Dictionary<uint, List<MedicalCheckVisitDtlTO>> dtlList = new Dictionary<uint, List<MedicalCheckVisitDtlTO>>();
            string select = "";

            try
            {
                if (visitIDs.Trim().Length <= 0)
                    return hdrList;

                select = "SELECT * FROM medical_chk_visits_hdr WHERE visit_id IN (" + visitIDs.Trim() + ") ORDER BY employee_id, schedule_date";

                // get visit details
                string selectDtl = "SELECT * FROM medical_chk_visits_dtl WHERE visit_id IN (" + visitIDs.Trim() + ")";
                SqlCommand cmdDtl = new SqlCommand(selectDtl, conn);
                SqlDataAdapter sqlDataAdapterDtl = new SqlDataAdapter(cmdDtl);

                sqlDataAdapterDtl.Fill(dataSetDtl, "VisitsDtl");
                DataTable tableDtl = dataSetDtl.Tables["VisitsDtl"];

                if (tableDtl.Rows.Count > 0)
                {
                    foreach (DataRow row in tableDtl.Rows)
                    {
                        dtl = new MedicalCheckVisitDtlTO();
                        if (row["rec_id"] != DBNull.Value)
                        {
                            dtl.RecID = UInt32.Parse(row["rec_id"].ToString().Trim());
                        }
                        if (row["visit_id"] != DBNull.Value)
                        {
                            dtl.VisitID = UInt32.Parse(row["visit_id"].ToString().Trim());
                        }
                        if (row["check_id"] != DBNull.Value)
                        {
                            dtl.CheckID = Int32.Parse(row["check_id"].ToString().Trim());
                        }
                        if (row["type"] != DBNull.Value)
                        {
                            dtl.Type = row["type"].ToString().Trim();
                        }
                        if (row["result"] != DBNull.Value)
                        {
                            dtl.Result = row["result"].ToString().Trim();
                        }
                        if (row["date_performed"] != DBNull.Value)
                        {
                            dtl.DatePerformed = DateTime.Parse(row["date_performed"].ToString().Trim());
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            dtl.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            dtl.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            dtl.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            dtl.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }

                        if (!dtlList.ContainsKey(dtl.VisitID))
                            dtlList.Add(dtl.VisitID, new List<MedicalCheckVisitDtlTO>());

                        dtlList[dtl.VisitID].Add(dtl);
                    }
                }

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visits");
                DataTable table = dataSet.Tables["Visits"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        hdr = new MedicalCheckVisitHdrTO();
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
                        if (row["flag_change"] != DBNull.Value)
                        {
                            hdr.FlagChange = Int32.Parse(row["flag_change"].ToString().Trim());
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

                        if (dtlList.ContainsKey(hdr.VisitID))
                            hdr.VisitDetails = dtlList[hdr.VisitID];
                                                
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
