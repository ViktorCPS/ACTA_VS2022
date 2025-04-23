using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

using TransferObjects;

namespace DataAccess
{
    public class MSSQLMedicalCheckVisitDtlHistDAO : MedicalCheckVisitDtlHistDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }
        public MSSQLMedicalCheckVisitDtlHistDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MSSQLMedicalCheckVisitDtlHistDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(MedicalCheckVisitDtlHistTO dtlTO, bool doCommit)
        {
            int rowsAffected = 0;

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

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

        public bool update(MedicalCheckVisitDtlHistTO dtlTO, bool doCommit)
        {
            bool isUpdated = false;

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE medical_chk_visits_dtl_hist SET ");
                if (dtlTO.RecID != 0)
                    sbUpdate.Append("rec_id = '" + dtlTO.RecID.ToString().Trim() + "', ");
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
                    sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE rec_id_hist = '" + dtlTO.RecHistID.ToString().Trim() + "'");

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

        public bool delete(uint reciIDHist, bool doCommit)
        {
            bool isDeleted = false;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM medical_chk_visits_dtl_hist WHERE rec_id_hist = '" + reciIDHist.ToString().Trim() + "'");

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

        public List<MedicalCheckVisitDtlHistTO> getMedicalCheckVisitDetailsHistory(MedicalCheckVisitDtlHistTO dtlTO)
        {
            DataSet dataSet = new DataSet();
            MedicalCheckVisitDtlHistTO dtl = new MedicalCheckVisitDtlHistTO();
            List<MedicalCheckVisitDtlHistTO> dtlList = new List<MedicalCheckVisitDtlHistTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM medical_chk_visits_dtl_hist ");
                if ((dtlTO.RecHistID != 0) || (dtlTO.RecID != 0) || (dtlTO.VisitID != 0) || (dtlTO.CheckID != -1) || (!dtlTO.Type.Trim().Equals("")) 
                    || (!dtlTO.Result.Trim().Equals("")))
                {
                    sb.Append(" WHERE");

                    if (dtlTO.RecHistID != 0)
                    {
                        sb.Append(" rec_id_hist = '" + dtlTO.RecHistID.ToString().Trim() + "' AND");
                    }
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

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visits");
                DataTable table = dataSet.Tables["Visits"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        dtl = new MedicalCheckVisitDtlHistTO();
                        if (row["rec_id_hist"] != DBNull.Value)
                        {
                            dtl.RecHistID = UInt32.Parse(row["rec_id_hist"].ToString().Trim());
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

        public DataTable getMedicalCheckVisitDetailsHistory(string recIDs)
        {
            DataSet dataSet = new DataSet();
            DataTable dtlTable = new DataTable();

            string select = "";

            try
            {
                if (recIDs.Trim().Equals(""))
                    return dtlTable;

                select = "SELECT type, check_id, result, date_performed FROM medical_chk_visits_dtl_hist WHERE rec_id IN (" + recIDs.Trim() + ") ORDER BY type";
                
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visits");
                dtlTable = dataSet.Tables["Visits"];                
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return dtlTable;
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

