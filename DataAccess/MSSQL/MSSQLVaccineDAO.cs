using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

using TransferObjects;

namespace DataAccess
{
    public class MSSQLVaccineDAO : VaccineDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }
        public MSSQLVaccineDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MSSQLVaccineDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(VaccineTO vacTO, bool doCommit)
        {
            int rowsAffected = 0;
            
            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO vaccines ");
                sbInsert.Append("(vaccine_id, vaccine_type, desc_sr, desc_en, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("(select max(vaccine_id)+1 from vaccines), ");
                sbInsert.Append("N'" + vacTO.VaccineType.Trim() + "', ");                
                sbInsert.Append("N'" + vacTO.DescSR.ToString().Trim() + "', ");
                sbInsert.Append("N'" + vacTO.DescEN.ToString().Trim() + "', ");
                if (!vacTO.CreatedBy.Trim().Equals(""))
                    sbInsert.Append("N'" + vacTO.CreatedBy.Trim() + "', ");
                else
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!vacTO.CreatedTime.Equals(new DateTime()))
                    sbInsert.Append("'" + vacTO.CreatedTime.ToString(dateTimeformat) + "') ");
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

        public bool update(VaccineTO vacTO, bool doCommit)
        {
            bool isUpdated = false;

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
                
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE vaccines SET ");                
                if (!vacTO.VaccineType.Trim().Equals(""))
                    sbUpdate.Append("vaccine_type = N'" + vacTO.VaccineType.Trim() + "', ");
                if (!vacTO.DescSR.Trim().Equals(""))
                    sbUpdate.Append("desc_sr = N'" + vacTO.DescSR.Trim() + "', ");
                if (!vacTO.DescEN.Trim().Equals(""))
                    sbUpdate.Append("desc_en = N'" + vacTO.DescEN.Trim() + "', ");
                if (!vacTO.ModifiedBy.Trim().Equals(""))
                    sbUpdate.Append("modified_by = N'" + vacTO.ModifiedBy.Trim() + "', ");
                else
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!vacTO.ModifiedTime.Equals(new DateTime()))
                    sbUpdate.Append("modified_time = '" + vacTO.ModifiedTime.ToString(dateTimeformat) + "' ");
                else
                    sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE vaccine_id = '" + vacTO.VaccineID.ToString().Trim() + "'");

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

        public bool delete(int vacID, bool doCommit)
        {
            bool isDeleted = false;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM vaccines WHERE vaccine_id = '" + vacID.ToString().Trim() + "'");

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

        public List<VaccineTO> getVaccines(VaccineTO vacTO)
        {
            DataSet dataSet = new DataSet();
            VaccineTO vac = new VaccineTO();
            List<VaccineTO> vacList = new List<VaccineTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM vaccines ");                
                if ((vacTO.VaccineID != -1) || (!vacTO.VaccineType.Trim().Equals("")) || (!vacTO.DescSR.Trim().Equals("")) || (!vacTO.DescEN.Trim().Equals("")))
                {
                    sb.Append(" WHERE");

                    if (vacTO.VaccineID != -1)
                    {
                        sb.Append(" vaccine_id = '" + vacTO.VaccineID.ToString().Trim() + "' AND");
                    }
                    if (!vacTO.VaccineType.Trim().Equals(""))
                    {
                        sb.Append(" vaccine_type = N'" + vacTO.VaccineType.Trim() + "' AND");
                    }
                    if (!vacTO.DescSR.Trim().Equals(""))
                    {
                        sb.Append(" desc_sr = N'" + vacTO.DescSR.Trim() + "' AND");
                    }
                    if (!vacTO.DescEN.Trim().Equals(""))
                    {
                        sb.Append(" desc_en = N'" + vacTO.DescEN.Trim() + "' AND");
                    }                    

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                SqlCommand cmd = new SqlCommand(select + " ORDER BY vaccine_type", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Risks");
                DataTable table = dataSet.Tables["Risks"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        vac = new VaccineTO();
                        if (row["vaccine_id"] != DBNull.Value)
                        {
                            vac.VaccineID = Int32.Parse(row["vaccine_id"].ToString().Trim());
                        }
                        if (row["vaccine_type"] != DBNull.Value)
                        {
                            vac.VaccineType = row["vaccine_type"].ToString().Trim();
                        }
                        if (row["desc_sr"] != DBNull.Value)
                        {
                            vac.DescSR = row["desc_sr"].ToString().Trim();
                        }
                        if (row["desc_en"] != DBNull.Value)
                        {
                            vac.DescEN = row["desc_en"].ToString().Trim();
                        }                        
                        if (row["created_by"] != DBNull.Value)
                        {
                            vac.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            vac.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            vac.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            vac.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }

                        vacList.Add(vac);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return vacList;
        }

        public Dictionary<int, VaccineTO> getVaccinesDictionary(VaccineTO vacTO)
        {
            DataSet dataSet = new DataSet();
            VaccineTO vac = new VaccineTO();
            Dictionary<int, VaccineTO> vacList = new Dictionary<int, VaccineTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM vaccines ");
                if ((vacTO.VaccineID != -1) || (!vacTO.VaccineType.Trim().Equals("")) || (!vacTO.DescSR.Trim().Equals("")) || (!vacTO.DescEN.Trim().Equals("")))
                {
                    sb.Append(" WHERE");

                    if (vacTO.VaccineID != -1)
                    {
                        sb.Append(" vaccine_id = '" + vacTO.VaccineID.ToString().Trim() + "' AND");
                    }
                    if (!vacTO.VaccineType.Trim().Equals(""))
                    {
                        sb.Append(" vaccine_type = N'" + vacTO.VaccineType.Trim() + "' AND");
                    }
                    if (!vacTO.DescSR.Trim().Equals(""))
                    {
                        sb.Append(" desc_sr = N'" + vacTO.DescSR.Trim() + "' AND");
                    }
                    if (!vacTO.DescEN.Trim().Equals(""))
                    {
                        sb.Append(" desc_en = N'" + vacTO.DescEN.Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                SqlCommand cmd = new SqlCommand(select + " ORDER BY vaccine_type", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Risks");
                DataTable table = dataSet.Tables["Risks"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        vac = new VaccineTO();
                        if (row["vaccine_id"] != DBNull.Value)
                        {
                            vac.VaccineID = Int32.Parse(row["vaccine_id"].ToString().Trim());
                        }
                        if (row["vaccine_type"] != DBNull.Value)
                        {
                            vac.VaccineType = row["vaccine_type"].ToString().Trim();
                        }
                        if (row["desc_sr"] != DBNull.Value)
                        {
                            vac.DescSR = row["desc_sr"].ToString().Trim();
                        }
                        if (row["desc_en"] != DBNull.Value)
                        {
                            vac.DescEN = row["desc_en"].ToString().Trim();
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            vac.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            vac.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            vac.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            vac.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }

                        if (!vacList.ContainsKey(vac.VaccineID))
                            vacList.Add(vac.VaccineID, vac);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return vacList;
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
