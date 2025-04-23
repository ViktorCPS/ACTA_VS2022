using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

using TransferObjects;

namespace DataAccess
{
    public class MSSQLEmployeePositionXRiskDAO : EmployeePositionXRiskDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }
        public MSSQLEmployeePositionXRiskDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MSSQLEmployeePositionXRiskDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(EmployeePositionXRiskTO riskTO, bool doCommit)
        {
            int rowsAffected = 0;

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employee_positions_x_risks ");
                sbInsert.Append("(position_id, risk_id, rotation, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("'" + riskTO.PositionID.ToString().Trim() + "', ");
                sbInsert.Append("'" + riskTO.RiskID.ToString().Trim() + "', ");
                sbInsert.Append("'" + riskTO.Rotation.ToString().Trim() + "', ");
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

        public bool update(EmployeePositionXRiskTO riskTO, bool doCommit)
        {
            bool isUpdated = false;

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE employee_positions_x_risks SET ");
                if (riskTO.Rotation != -1)
                    sbUpdate.Append("rotation = '" + riskTO.Rotation.ToString().Trim() + "', ");
                if (!riskTO.ModifiedBy.Trim().Equals(""))
                    sbUpdate.Append("modified_by = N'" + riskTO.ModifiedBy.Trim() + "', ");
                else
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!riskTO.ModifiedTime.Equals(new DateTime()))
                    sbUpdate.Append("modified_time = '" + riskTO.ModifiedTime.ToString(dateTimeformat) + "' ");
                else
                    sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE position_id = '" + riskTO.PositionID.ToString().Trim() + "' AND risk_id = '" + riskTO.RiskID.ToString().Trim() + "'");

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

        public bool delete(int posID, int riskID, bool doCommit)
        {
            bool isDeleted = false;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM employee_positions_x_risks WHERE position_id = '" + posID.ToString().Trim() + "' AND risk_id = '" + riskID.ToString().Trim() + "'");

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
        public List<EmployeePositionXRiskTO> getEmployeePositionXRisksByWU(EmployeePositionXRiskTO riskTO, int working_unit_id)
        {
            DataSet dataSet = new DataSet();
            EmployeePositionXRiskTO risk = new EmployeePositionXRiskTO();
            List<EmployeePositionXRiskTO> riskList = new List<EmployeePositionXRiskTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT pxr.* FROM employee_positions_x_risks pxr, employee_positions p, risks r where p.position_id = pxr.position_id and pxr.risk_id=r.risk_id and r.working_unit_id = " + working_unit_id + " AND p.working_unit_id= " + working_unit_id);

                if ((riskTO.PositionID != -1) || (riskTO.RiskID != -1) || (riskTO.Rotation != -1))
                {
                    sb.Append(" AND");

                    if (riskTO.PositionID != -1)
                    {
                        sb.Append(" position_id = '" + riskTO.PositionID.ToString().Trim() + "' AND");
                    }
                    if (riskTO.RiskID != -1)
                    {
                        sb.Append(" risk_id = '" + riskTO.RiskID.ToString().Trim() + "' AND");
                    }
                    if (riskTO.Rotation != -1)
                    {
                        sb.Append(" rotation = '" + riskTO.Rotation.ToString().Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Risks");
                DataTable table = dataSet.Tables["Risks"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        risk = new EmployeePositionXRiskTO();
                        if (row["position_id"] != DBNull.Value)
                        {
                            risk.PositionID = Int32.Parse(row["position_id"].ToString().Trim());
                        }
                        if (row["risk_id"] != DBNull.Value)
                        {
                            risk.RiskID = Int32.Parse(row["risk_id"].ToString().Trim());
                        }
                        if (row["rotation"] != DBNull.Value)
                        {
                            risk.Rotation = Int32.Parse(row["rotation"].ToString().Trim());
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

        public List<EmployeePositionXRiskTO> getEmployeePositionXRisks(EmployeePositionXRiskTO riskTO)
        {
            DataSet dataSet = new DataSet();
            EmployeePositionXRiskTO risk = new EmployeePositionXRiskTO();
            List<EmployeePositionXRiskTO> riskList = new List<EmployeePositionXRiskTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employee_positions_x_risks ");

                if ((riskTO.PositionID != -1) || (riskTO.RiskID != -1) || (riskTO.Rotation != -1))
                {
                    sb.Append(" WHERE");

                    if (riskTO.PositionID != -1)
                    {
                        sb.Append(" position_id = '" + riskTO.PositionID.ToString().Trim() + "' AND");
                    }
                    if (riskTO.RiskID != -1)
                    {
                        sb.Append(" risk_id = '" + riskTO.RiskID.ToString().Trim() + "' AND");
                    }
                    if (riskTO.Rotation != -1)
                    {
                        sb.Append(" rotation = '" + riskTO.Rotation.ToString().Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Risks");
                DataTable table = dataSet.Tables["Risks"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        risk = new EmployeePositionXRiskTO();
                        if (row["position_id"] != DBNull.Value)
                        {
                            risk.PositionID = Int32.Parse(row["position_id"].ToString().Trim());
                        }
                        if (row["risk_id"] != DBNull.Value)
                        {
                            risk.RiskID = Int32.Parse(row["risk_id"].ToString().Trim());
                        }
                        if (row["rotation"] != DBNull.Value)
                        {
                            risk.Rotation = Int32.Parse(row["rotation"].ToString().Trim());
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

        public List<RiskTO> getRisks(string posIDs)
        {
            DataSet dataSet = new DataSet();
            RiskTO risk = new RiskTO();
            List<RiskTO> riskList = new List<RiskTO>();
            string select = "";

            try
            {
                if (posIDs.Length <= 0)
                    return riskList;

                select = "SELECT * FROM risks WHERE risk_id IN (SELECT risk_id FROM employee_positions_x_risks WHERE position_id IN (" + posIDs.Trim() + ")) ORDER BY risk_code";

                SqlCommand cmd = new SqlCommand(select, conn);
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
