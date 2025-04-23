using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Data;
using MySql.Data.MySqlClient;

using TransferObjects;
using Util;

namespace DataAccess
{
    public class MySQLMedicalCheckVisitDtlDAO : MedicalCheckVisitDtlDAO
    {
        MySqlConnection conn = null;
        MySqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MySQLMedicalCheckVisitDtlDAO()
        {
            conn = MySQLDAOFactory.getConnection();
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MySQLMedicalCheckVisitDtlDAO(MySqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(MedicalCheckVisitDtlTO dtlTO, bool doCommit)
        {
            int rowsAffected = 0;

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

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
                    sbInsert.Append("NOW()) ");

                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, SqlTrans);

                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit)
                    SqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    SqlTrans.Rollback();
                throw ex;
            }

            return rowsAffected;
        }

        public bool update(MedicalCheckVisitDtlTO dtlTO, bool doCommit)
        {
            bool isUpdated = false;

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE medical_chk_visits_dtl SET ");
                if (dtlTO.VisitID != 0)
                    sbUpdate.Append("visit_id = '" + dtlTO.VisitID.ToString().Trim() + "', ");
                if (dtlTO.CheckID != -1)
                    sbUpdate.Append("check_id = '" + dtlTO.CheckID.ToString().Trim() + "', ");
                if (!dtlTO.Type.Trim().Equals(""))
                    sbUpdate.Append("type = N'" + dtlTO.Type.Trim() + "', ");
                if (!dtlTO.Result.Trim().Equals(""))
                    sbUpdate.Append("result = N'" + dtlTO.Result.Trim() + "', ");
                else
                    sbUpdate.Append("result = NULL, ");
                if (!dtlTO.DatePerformed.Equals(new DateTime()))
                    sbUpdate.Append("date_performed = '" + dtlTO.DatePerformed.ToString(dateTimeformat) + "', ");
                else
                    sbUpdate.Append("date_performed = NULL, ");
                if (!dtlTO.ModifiedBy.Trim().Equals(""))
                    sbUpdate.Append("modified_by = N'" + dtlTO.ModifiedBy.Trim() + "', ");
                else
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!dtlTO.ModifiedTime.Equals(new DateTime()))
                    sbUpdate.Append("modified_time = '" + dtlTO.ModifiedTime.ToString(dateTimeformat) + "' ");
                else
                    sbUpdate.Append("modified_time = NOW() ");
                sbUpdate.Append("WHERE rec_id = '" + dtlTO.RecID.ToString().Trim() + "'");

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

        public bool delete(string recIDs, bool doCommit)
        {
            bool isDeleted = false;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM medical_chk_visits_dtl WHERE rec_id IN (" + recIDs.ToString().Trim() + ")");

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

        public List<MedicalCheckVisitDtlTO> getMedicalCheckVisitDetails(MedicalCheckVisitDtlTO dtlTO)
        {
            DataSet dataSet = new DataSet();
            MedicalCheckVisitDtlTO dtl = new MedicalCheckVisitDtlTO();
            List<MedicalCheckVisitDtlTO> dtlList = new List<MedicalCheckVisitDtlTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM medical_chk_visits_dtl ");
                if ((dtlTO.VisitID != 0) || (dtlTO.CheckID != -1) || (!dtlTO.Type.Trim().Equals("")) || (!dtlTO.Result.Trim().Equals("")) || (dtlTO.RecID != 0))
                {
                    sb.Append(" WHERE");

                    if (dtlTO.RecID != 0)
                    {
                        sb.Append(" rec_id = '" + dtlTO.RecID.ToString().Trim() + "' AND");
                    }
                    if (dtlTO.VisitID != 0)
                    {
                        sb.Append(" visit_id = '" + dtlTO.VisitID.ToString().Trim() + "' AND");
                    }
                    if (dtlTO.CheckID != -1)
                    {
                        sb.Append(" check_id = '" + dtlTO.CheckID.ToString().Trim() + "' AND");
                    }
                    if (!dtlTO.Type.Trim().Equals(""))
                    {
                        sb.Append(" type = N'" + dtlTO.Type.Trim().ToString() + "' AND");
                    }
                    if (!dtlTO.Result.Trim().Equals(""))
                    {
                        sb.Append(" result = N'" + dtlTO.Result.Trim().ToString() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visits");
                DataTable table = dataSet.Tables["Visits"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
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

                        dtlList.Add(dtl);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return dtlList;
        }

        public List<MedicalCheckVisitDtlTO> getMedicalCheckVisitDetails(string recIDs)
        {
            DataSet dataSet = new DataSet();
            MedicalCheckVisitDtlTO dtl = new MedicalCheckVisitDtlTO();
            List<MedicalCheckVisitDtlTO> dtlList = new List<MedicalCheckVisitDtlTO>();
            string select = "";

            try
            {
                if (recIDs.Trim().Length <= 0)
                    return dtlList;

                select = "SELECT * FROM medical_chk_visits_dtl WHERE rec_id IN (" + recIDs.Trim() + ")";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visits");
                DataTable table = dataSet.Tables["Visits"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
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

                        dtlList.Add(dtl);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return dtlList;
        }

        public List<MedicalCheckVisitDtlTO> getPerformedVisits(string emplIDs, string type, DateTime fromDate, DateTime toDate)
        {
            DataSet dataSet = new DataSet();
            MedicalCheckVisitDtlTO dtl = new MedicalCheckVisitDtlTO();
            List<MedicalCheckVisitDtlTO> dtlList = new List<MedicalCheckVisitDtlTO>();
            string select = "";

            try
            {
                select = "SELECT h.employee_id, d.* FROM medical_chk_visits_dtl d, medical_chk_visits_hdr h WHERE d.visit_id = h.visit_id";

                if (emplIDs.Length > 0)
                    select += " AND h.employee_id IN (" + emplIDs.Trim() + ")";

                if (type.Length > 0)
                    select += " AND UPPER(d.type) = '" + type.Trim().ToUpper() + "'";

                if (!fromDate.Equals(new DateTime()))
                    select += " AND date_performed IS NOT NULL AND date_performed >= '" + fromDate.Date.ToString(dateTimeformat) + "'";

                if (!toDate.Equals(new DateTime()))
                    select += " AND date_performed IS NOT NULL AND date_performed < '" + toDate.Date.AddDays(1).ToString(dateTimeformat) + "'";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visits");
                DataTable table = dataSet.Tables["Visits"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        dtl = new MedicalCheckVisitDtlTO();
                        if (row["employee_id"] != DBNull.Value)
                        {
                            dtl.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
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

                        dtlList.Add(dtl);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return dtlList;
        }

        public Dictionary<int, Dictionary<string, List<int>>> getScheduledVisits(string emplIDs, string type)
        {
            DataSet dataSet = new DataSet();
            MedicalCheckVisitDtlTO dtl = new MedicalCheckVisitDtlTO();
            Dictionary<int, Dictionary<string, List<int>>> emplDetails = new Dictionary<int, Dictionary<string, List<int>>>();
            string select = "";

            try
            {
                select = "SELECT h.employee_id, d.* FROM medical_chk_visits_dtl d, medical_chk_visits_hdr h WHERE h.status <> '"
                    + Constants.MedicalCheckVisitStatus.DELETED.ToString().Trim() + "' AND d.visit_id = h.visit_id AND d.date_performed IS NULL";

                if (emplIDs.Length > 0)
                    select += " AND h.employee_id IN (" + emplIDs.Trim() + ")";

                if (type.Length > 0)
                    select += " AND UPPER(d.type) = '" + type.Trim().ToUpper() + "'";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visits");
                DataTable table = dataSet.Tables["Visits"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        dtl = new MedicalCheckVisitDtlTO();
                        if (row["employee_id"] != DBNull.Value)
                        {
                            dtl.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["check_id"] != DBNull.Value)
                        {
                            dtl.CheckID = Int32.Parse(row["check_id"].ToString().Trim());
                        }
                        if (row["type"] != DBNull.Value)
                        {
                            dtl.Type = row["type"].ToString().Trim();
                        }

                        if (!emplDetails.ContainsKey(dtl.EmployeeID))
                            emplDetails.Add(dtl.EmployeeID, new Dictionary<string, List<int>>());

                        if (!emplDetails[dtl.EmployeeID].ContainsKey(dtl.Type))
                            emplDetails[dtl.EmployeeID].Add(dtl.Type, new List<int>());

                        if (!emplDetails[dtl.EmployeeID][dtl.Type].Contains(dtl.CheckID))
                            emplDetails[dtl.EmployeeID][dtl.Type].Add(dtl.CheckID);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return emplDetails;
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