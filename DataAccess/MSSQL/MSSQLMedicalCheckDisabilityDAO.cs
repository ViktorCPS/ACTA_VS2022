using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

using TransferObjects;

namespace DataAccess
{
    public class MSSQLMedicalCheckDisabilityDAO : MedicalCheckDisabilityDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }
        public MSSQLMedicalCheckDisabilityDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MSSQLMedicalCheckDisabilityDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(MedicalCheckDisabilityTO disabilityTO, bool doCommit)
        {
            int rowsAffected = 0;

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
                
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO medical_chk_disabilities ");
                sbInsert.Append("(working_unit_id, disability_id, disability_code, desc_sr, desc_en, status, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("'" + disabilityTO.WorkingUnitID.ToString().Trim() + "', ");
                sbInsert.Append("(select max(disability_id)+1 from medical_chk_disabilities), ");
                sbInsert.Append("N'" + disabilityTO.DisabilityCode.ToString().Trim() + "', ");
                sbInsert.Append("N'" + disabilityTO.DescSR.ToString().Trim() + "', ");
                sbInsert.Append("N'" + disabilityTO.DescEN.ToString().Trim() + "', ");
                sbInsert.Append("N'" + disabilityTO.Status.Trim() + "', ");
                if (!disabilityTO.CreatedBy.Trim().Equals(""))
                    sbInsert.Append("N'" + disabilityTO.CreatedBy.Trim() + "', ");
                else
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!disabilityTO.CreatedTime.Equals(new DateTime()))
                    sbInsert.Append("'" + disabilityTO.CreatedTime.ToString(dateTimeformat) + "') ");
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

        public bool update(MedicalCheckDisabilityTO disabilityTO, bool doCommit)
        {
            bool isUpdated = false;

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
                
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE medical_chk_disabilities SET ");
                if (disabilityTO.WorkingUnitID != -1)
                    sbUpdate.Append("working_unit_id = '" + disabilityTO.WorkingUnitID.ToString().Trim() + "', ");
                if (!disabilityTO.DisabilityCode.Trim().Equals(""))
                    sbUpdate.Append("disability_code = N'" + disabilityTO.DisabilityCode.Trim() + "', ");
                if (!disabilityTO.DescSR.Trim().Equals(""))
                    sbUpdate.Append("desc_sr = N'" + disabilityTO.DescSR.Trim() + "', ");
                if (!disabilityTO.DescEN.Trim().Equals(""))
                    sbUpdate.Append("desc_en = N'" + disabilityTO.DescEN.Trim() + "', ");
                if (!disabilityTO.Status.Trim().Equals(""))
                    sbUpdate.Append("status = N'" + disabilityTO.Status.Trim() + "', ");
                if (!disabilityTO.ModifiedBy.Trim().Equals(""))
                    sbUpdate.Append("modified_by = N'" + disabilityTO.ModifiedBy.Trim() + "', ");
                else
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!disabilityTO.ModifiedTime.Equals(new DateTime()))
                    sbUpdate.Append("modified_time = '" + disabilityTO.ModifiedTime.ToString(dateTimeformat) + "' ");
                else
                    sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE disability_id = '" + disabilityTO.DisabilityID.ToString().Trim() + "'");

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

        public bool delete(int disID, bool doCommit)
        {
            bool isDeleted = false;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM medical_chk_disabilities WHERE disability_id = '" + disID.ToString().Trim() + "'");

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

        public List<MedicalCheckDisabilityTO> getMedicalCheckDisabilities(MedicalCheckDisabilityTO disabilityTO)
        {
            DataSet dataSet = new DataSet();
            MedicalCheckDisabilityTO disTO = new MedicalCheckDisabilityTO();
            List<MedicalCheckDisabilityTO> disList = new List<MedicalCheckDisabilityTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM medical_chk_disabilities ");                
                if ((disabilityTO.WorkingUnitID != -1) || (disabilityTO.DisabilityID != -1) || (!disabilityTO.DisabilityCode.Trim().Equals("")) 
                    || (!disabilityTO.DescSR.Trim().Equals("")) || (!disabilityTO.DescEN.Trim().Equals("")) || (!disabilityTO.Status.Trim().Equals("")))
                {
                    sb.Append(" WHERE");

                    if (disabilityTO.WorkingUnitID != -1)
                    {
                        sb.Append(" working_unit_id = '" + disabilityTO.WorkingUnitID.ToString().Trim() + "' AND");
                    }
                    if (disabilityTO.DisabilityID != -1)
                    {
                        sb.Append(" disability_id = '" + disabilityTO.DisabilityID.ToString().Trim() + "' AND");
                    }
                    if (!disabilityTO.DisabilityCode.Trim().Equals(""))
                    {
                        sb.Append(" disability_code = N'" + disabilityTO.DisabilityCode.Trim() + "' AND");
                    }
                    if (!disabilityTO.DescSR.Trim().Equals(""))
                    {
                        sb.Append(" desc_sr = N'" + disabilityTO.DescSR.Trim() + "' AND");
                    }
                    if (!disabilityTO.DescEN.Trim().Equals(""))
                    {
                        sb.Append(" desc_en = N'" + disabilityTO.DescEN.Trim() + "' AND");
                    }
                    if (!disabilityTO.Status.Trim().Equals(""))
                    {
                        sb.Append(" status = N'" + disabilityTO.Status.Trim() + "' AND");
                    }                    

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                SqlCommand cmd = new SqlCommand(select + " ORDER BY disability_code", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Disabilities");
                DataTable table = dataSet.Tables["Disabilities"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        disTO = new MedicalCheckDisabilityTO();
                        if (row["working_unit_id"] != DBNull.Value)
                        {
                            disTO.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["disability_id"] != DBNull.Value)
                        {
                            disTO.DisabilityID = Int32.Parse(row["disability_id"].ToString().Trim());
                        }
                        if (row["disability_code"] != DBNull.Value)
                        {
                            disTO.DisabilityCode = row["disability_code"].ToString().Trim();
                        }
                        if (row["desc_sr"] != DBNull.Value)
                        {
                            disTO.DescSR = row["desc_sr"].ToString().Trim();
                        }
                        if (row["desc_en"] != DBNull.Value)
                        {
                            disTO.DescEN = row["desc_en"].ToString().Trim();
                        }
                        if (row["status"] != DBNull.Value)
                        {
                            disTO.Status = row["status"].ToString().Trim();
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            disTO.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            disTO.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            disTO.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            disTO.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }

                        disList.Add(disTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return disList;
        }

        public Dictionary<int, MedicalCheckDisabilityTO> getMedicalCheckDisabilitiesDictionary(MedicalCheckDisabilityTO disabilityTO)
        {
            DataSet dataSet = new DataSet();
            MedicalCheckDisabilityTO disTO = new MedicalCheckDisabilityTO();
            Dictionary<int, MedicalCheckDisabilityTO> disList = new Dictionary<int, MedicalCheckDisabilityTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM medical_chk_disabilities ");
                if ((disabilityTO.WorkingUnitID != -1) || (disabilityTO.DisabilityID != -1) || (!disabilityTO.DisabilityCode.Trim().Equals(""))
                    || (!disabilityTO.DescSR.Trim().Equals("")) || (!disabilityTO.DescEN.Trim().Equals("")) || (!disabilityTO.Status.Trim().Equals("")))
                {
                    sb.Append(" WHERE");

                    if (disabilityTO.WorkingUnitID != -1)
                    {
                        sb.Append(" working_unit_id = '" + disabilityTO.WorkingUnitID.ToString().Trim() + "' AND");
                    }
                    if (disabilityTO.DisabilityID != -1)
                    {
                        sb.Append(" disability_id = '" + disabilityTO.DisabilityID.ToString().Trim() + "' AND");
                    }
                    if (!disabilityTO.DisabilityCode.Trim().Equals(""))
                    {
                        sb.Append(" disability_code = N'" + disabilityTO.DisabilityCode.Trim() + "' AND");
                    }
                    if (!disabilityTO.DescSR.Trim().Equals(""))
                    {
                        sb.Append(" desc_sr = N'" + disabilityTO.DescSR.Trim() + "' AND");
                    }
                    if (!disabilityTO.DescEN.Trim().Equals(""))
                    {
                        sb.Append(" desc_en = N'" + disabilityTO.DescEN.Trim() + "' AND");
                    }
                    if (!disabilityTO.Status.Trim().Equals(""))
                    {
                        sb.Append(" status = N'" + disabilityTO.Status.Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                SqlCommand cmd = new SqlCommand(select + " ORDER BY disability_code", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Disabilities");
                DataTable table = dataSet.Tables["Disabilities"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        disTO = new MedicalCheckDisabilityTO();
                        if (row["working_unit_id"] != DBNull.Value)
                        {
                            disTO.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["disability_id"] != DBNull.Value)
                        {
                            disTO.DisabilityID = Int32.Parse(row["disability_id"].ToString().Trim());
                        }
                        if (row["disability_code"] != DBNull.Value)
                        {
                            disTO.DisabilityCode = row["disability_code"].ToString().Trim();
                        }
                        if (row["desc_sr"] != DBNull.Value)
                        {
                            disTO.DescSR = row["desc_sr"].ToString().Trim();
                        }
                        if (row["desc_en"] != DBNull.Value)
                        {
                            disTO.DescEN = row["desc_en"].ToString().Trim();
                        }
                        if (row["status"] != DBNull.Value)
                        {
                            disTO.Status = row["status"].ToString().Trim();
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            disTO.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            disTO.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            disTO.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            disTO.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }

                        if (!disList.ContainsKey(disTO.DisabilityID))
                            disList.Add(disTO.DisabilityID, disTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return disList;
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
