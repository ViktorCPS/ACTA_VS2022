using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

using TransferObjects;

namespace DataAccess
{
    public class MSSQLRiskDAO : RiskDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }
        public MSSQLRiskDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MSSQLRiskDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(RiskTO riskTO, bool doCommit)
        {
            int rowsAffected = 0;

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
                                
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO risks ");
                sbInsert.Append("(working_unit_id, risk_id, risk_code, desc_sr, desc_en, default_rotation, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("'" + riskTO.WorkingUnitID.ToString().Trim() + "', ");
                sbInsert.Append("(select max(risk_id)+1 from risks), ");
                sbInsert.Append("N'" + riskTO.RiskCode.ToString().Trim() + "', ");
                sbInsert.Append("N'" + riskTO.DescSR.ToString().Trim() + "', ");
                sbInsert.Append("N'" + riskTO.DescEN.ToString().Trim() + "', ");
                sbInsert.Append("'" + riskTO.DefaultRotation.ToString().Trim() + "', ");
                if (!riskTO.CreatedBy.Trim().Equals(""))
                    sbInsert.Append("N'" + riskTO.CreatedBy.Trim() + "', ");
                else
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!riskTO.CreatedTime.Equals(new DateTime()))
                    sbInsert.Append("'" + riskTO.CreatedTime.ToString(dateTimeformat) + "') ");
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

        public bool update(RiskTO riskTO, bool doCommit)
        {
            bool isUpdated = false;

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE risks SET ");
                if (riskTO.WorkingUnitID != -1)
                    sbUpdate.Append("working_unit_id = '" + riskTO.WorkingUnitID.ToString().Trim() + "', ");
                if (!riskTO.RiskCode.Trim().Equals(""))
                    sbUpdate.Append("risk_code = N'" + riskTO.RiskCode.Trim() + "', ");
                if (!riskTO.DescSR.Trim().Equals(""))
                    sbUpdate.Append("desc_sr = N'" + riskTO.DescSR.Trim() + "', ");
                if (!riskTO.DescEN.Trim().Equals(""))
                    sbUpdate.Append("desc_en = N'" + riskTO.DescEN.Trim() + "', ");
                if (riskTO.DefaultRotation != -1)
                    sbUpdate.Append("default_rotation = '" + riskTO.DefaultRotation.ToString().Trim() + "', ");
                if (!riskTO.ModifiedBy.Trim().Equals(""))
                    sbUpdate.Append("modified_by = N'" + riskTO.ModifiedBy.Trim() + "', ");
                else
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!riskTO.ModifiedTime.Equals(new DateTime()))
                    sbUpdate.Append("modified_time = '" + riskTO.ModifiedTime.ToString(dateTimeformat) + "' ");
                else
                    sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE risk_id = '" + riskTO.RiskID.ToString().Trim() + "'");

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

        public bool delete(int riskID, bool doCommit)
        {
            bool isDeleted = false;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM risks WHERE risk_id = '" + riskID.ToString().Trim() + "'");

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

        public List<RiskTO> getRisks(RiskTO riskTO)
        {
            DataSet dataSet = new DataSet();
            RiskTO risk = new RiskTO();
            List<RiskTO> riskList = new List<RiskTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM risks ");

                if ((riskTO.WorkingUnitID != -1) || (riskTO.RiskID != -1) || (!riskTO.RiskCode.Trim().Equals("")) || (!riskTO.DescSR.Trim().Equals(""))
                    || (!riskTO.DescEN.Trim().Equals("")) || (riskTO.DefaultRotation != -1))
                {
                    sb.Append(" WHERE");

                    if (riskTO.WorkingUnitID != -1)
                    {
                        sb.Append(" working_unit_id = '" + riskTO.WorkingUnitID.ToString().Trim() + "' AND");
                    }
                    if (riskTO.RiskID != -1)
                    {
                        sb.Append(" risk_id = '" + riskTO.RiskID.ToString().Trim() + "' AND");
                    }
                    if (!riskTO.RiskCode.Trim().Equals(""))
                    {
                        sb.Append(" risk_code = N'" + riskTO.RiskCode.Trim() + "' AND");
                    }
                    if (!riskTO.DescSR.Trim().Equals(""))
                    {
                        sb.Append(" desc_sr = N'" + riskTO.DescSR.Trim() + "' AND");
                    }
                    if (!riskTO.DescEN.Trim().Equals(""))
                    {
                        sb.Append(" desc_en = N'" + riskTO.DescEN.Trim() + "' AND");
                    }
                    if (riskTO.DefaultRotation != -1)
                    {
                        sb.Append(" default_rotation = '" + riskTO.DefaultRotation.ToString().Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                SqlCommand cmd = new SqlCommand(select + " ORDER BY risk_code", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Risks");
                DataTable table = dataSet.Tables["Risks"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        risk = new RiskTO();
                        if (row["working_unit_id"] != DBNull.Value)
                        {
                            risk.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["risk_id"] != DBNull.Value)
                        {
                            risk.RiskID = Int32.Parse(row["risk_id"].ToString().Trim());
                        }
                        if (row["risk_code"] != DBNull.Value)
                        {
                            risk.RiskCode = row["risk_code"].ToString().Trim();
                        }                        
                        if (row["desc_sr"] != DBNull.Value)
                        {
                            risk.DescSR = row["desc_sr"].ToString().Trim();
                        }
                        if (row["desc_en"] != DBNull.Value)
                        {
                            risk.DescEN = row["desc_en"].ToString().Trim();
                        }
                        if (row["default_rotation"] != DBNull.Value)
                        {
                            risk.DefaultRotation = Int32.Parse(row["default_rotation"].ToString().Trim());
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            risk.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            risk.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            risk.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            risk.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }

                        riskList.Add(risk);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return riskList;
        }

        public Dictionary<int, RiskTO> getRisksDictionary(RiskTO riskTO)
        {
            DataSet dataSet = new DataSet();
            RiskTO risk = new RiskTO();
            Dictionary<int, RiskTO> riskList = new Dictionary<int, RiskTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM risks ");

                if ((riskTO.WorkingUnitID != -1) || (riskTO.RiskID != -1) || (!riskTO.RiskCode.Trim().Equals("")) || (!riskTO.DescSR.Trim().Equals(""))
                    || (!riskTO.DescEN.Trim().Equals("")) || (riskTO.DefaultRotation != -1))
                {
                    sb.Append(" WHERE");

                    if (riskTO.WorkingUnitID != -1)
                    {
                        sb.Append(" working_unit_id = '" + riskTO.WorkingUnitID.ToString().Trim() + "' AND");
                    }
                    if (riskTO.RiskID != -1)
                    {
                        sb.Append(" risk_id = '" + riskTO.RiskID.ToString().Trim() + "' AND");
                    }
                    if (!riskTO.RiskCode.Trim().Equals(""))
                    {
                        sb.Append(" risk_code = N'" + riskTO.RiskCode.Trim() + "' AND");
                    }
                    if (!riskTO.DescSR.Trim().Equals(""))
                    {
                        sb.Append(" desc_sr = N'" + riskTO.DescSR.Trim() + "' AND");
                    }
                    if (!riskTO.DescEN.Trim().Equals(""))
                    {
                        sb.Append(" desc_en = N'" + riskTO.DescEN.Trim() + "' AND");
                    }
                    if (riskTO.DefaultRotation != -1)
                    {
                        sb.Append(" default_rotation = '" + riskTO.DefaultRotation.ToString().Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                SqlCommand cmd = new SqlCommand(select + " ORDER BY risk_code", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Risks");
                DataTable table = dataSet.Tables["Risks"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        risk = new RiskTO();
                        if (row["working_unit_id"] != DBNull.Value)
                        {
                            risk.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["risk_id"] != DBNull.Value)
                        {
                            risk.RiskID = Int32.Parse(row["risk_id"].ToString().Trim());
                        }
                        if (row["risk_code"] != DBNull.Value)
                        {
                            risk.RiskCode = row["risk_code"].ToString().Trim();
                        }
                        if (row["desc_sr"] != DBNull.Value)
                        {
                            risk.DescSR = row["desc_sr"].ToString().Trim();
                        }
                        if (row["desc_en"] != DBNull.Value)
                        {
                            risk.DescEN = row["desc_en"].ToString().Trim();
                        }
                        if (row["default_rotation"] != DBNull.Value)
                        {
                            risk.DefaultRotation = Int32.Parse(row["default_rotation"].ToString().Trim());
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            risk.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            risk.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            risk.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            risk.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }

                        if (!riskList.ContainsKey(risk.RiskID))
                            riskList.Add(risk.RiskID, risk);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return riskList;
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
