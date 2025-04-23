using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Data;
using MySql.Data.MySqlClient;

using TransferObjects;

namespace DataAccess
{
    public class MySQLEmployeeXRiskDAO : EmployeeXRiskDAO
    {
        MySqlConnection conn = null;
        MySqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MySQLEmployeeXRiskDAO()
        {
            conn = MySQLDAOFactory.getConnection();
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MySQLEmployeeXRiskDAO(MySqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(EmployeeXRiskTO riskTO, bool doCommit)
        {
            int rowsAffected = 0;

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employees_x_risks ");
                sbInsert.Append("(employee_id, risk_id, date_start, date_end, rotation, last_date_performed, last_visit_rec_id, last_schedule_date, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("'" + riskTO.EmployeeID.ToString().Trim() + "', ");
                sbInsert.Append("'" + riskTO.RiskID.ToString().Trim() + "', ");
                sbInsert.Append("'" + riskTO.DateStart.ToString(dateTimeformat) + "', ");
                if (!riskTO.DateEnd.Equals(new DateTime()))
                    sbInsert.Append("'" + riskTO.DateEnd.ToString(dateTimeformat) + "', ");
                else
                    sbInsert.Append("NULL, ");
                sbInsert.Append("'" + riskTO.Rotation.ToString().Trim() + "', ");
                if (!riskTO.LastDatePerformed.Equals(new DateTime()))
                    sbInsert.Append("'" + riskTO.LastDatePerformed.ToString(dateTimeformat) + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (riskTO.LastVisitRecID != 0)
                    sbInsert.Append("'" + riskTO.LastVisitRecID.ToString().Trim() + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (!riskTO.LastScheduleDate.Equals(new DateTime()))
                    sbInsert.Append("'" + riskTO.LastScheduleDate.ToString(dateTimeformat) + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (!riskTO.CreatedBy.Trim().Equals(""))
                    sbInsert.Append("N'" + riskTO.CreatedBy.Trim() + "', ");
                else
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!riskTO.CreatedTime.Equals(new DateTime()))
                    sbInsert.Append("'" + riskTO.CreatedTime.ToString(dateTimeformat) + "') ");
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

        public bool update(EmployeeXRiskTO riskTO, bool doCommit)
        {
            bool isUpdated = false;

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE employees_x_risks SET ");
                if (riskTO.EmployeeID != -1)
                    sbUpdate.Append("employee_id = '" + riskTO.EmployeeID.ToString().Trim() + "', ");
                if (riskTO.RiskID != -1)
                    sbUpdate.Append("risk_id = '" + riskTO.RiskID.ToString().Trim() + "', ");
                if (!riskTO.DateStart.Equals(new DateTime()))
                    sbUpdate.Append("date_start = '" + riskTO.DateStart.ToString(dateTimeformat) + "', ");
                if (!riskTO.DateEnd.Equals(new DateTime()))
                    sbUpdate.Append("date_end = '" + riskTO.DateEnd.ToString(dateTimeformat) + "', ");
                else
                    sbUpdate.Append("date_end = NULL, ");
                if (riskTO.Rotation != -1)
                    sbUpdate.Append("rotation = '" + riskTO.Rotation.ToString().Trim() + "', ");
                if (!riskTO.LastDatePerformed.Equals(new DateTime()))
                    sbUpdate.Append("last_date_performed = '" + riskTO.LastDatePerformed.ToString(dateTimeformat) + "', ");
                else
                    sbUpdate.Append("last_date_performed = NULL, ");
                if (riskTO.LastVisitRecID != 0)
                    sbUpdate.Append("last_visit_rec_id = '" + riskTO.LastVisitRecID.ToString().Trim() + "', ");
                else
                    sbUpdate.Append("last_visit_rec_id = NULL, ");
                if (!riskTO.LastScheduleDate.Equals(new DateTime()))
                    sbUpdate.Append("last_schedule_date = '" + riskTO.LastScheduleDate.ToString(dateTimeformat) + "', ");
                else
                    sbUpdate.Append("last_schedule_date = NULL, ");
                if (!riskTO.ModifiedBy.Trim().Equals(""))
                    sbUpdate.Append("modified_by = N'" + riskTO.ModifiedBy.Trim() + "', ");
                else
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!riskTO.ModifiedTime.Equals(new DateTime()))
                    sbUpdate.Append("modified_time = '" + riskTO.ModifiedTime.ToString(dateTimeformat) + "' ");
                else
                    sbUpdate.Append("modified_time = NOW() ");
                sbUpdate.Append("WHERE rec_id = '" + riskTO.RecID.ToString().Trim() + "'");

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

        public bool delete(string recID, bool doCommit)
        {
            bool isDeleted = false;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM employees_x_risks WHERE rec_id IN (" + recID.ToString().Trim() + ")");

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

        public List<EmployeeXRiskTO> getEmployeeXRisks(EmployeeXRiskTO riskTO)
        {
            DataSet dataSet = new DataSet();
            EmployeeXRiskTO risk = new EmployeeXRiskTO();
            List<EmployeeXRiskTO> riskList = new List<EmployeeXRiskTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employees_x_risks ");

                if ((riskTO.RecID != 0) || (riskTO.EmployeeID != -1) || (riskTO.RiskID != -1) || (!riskTO.DateStart.Equals(new DateTime())) || (!riskTO.DateEnd.Equals(new DateTime()))
                    || (riskTO.Rotation != -1) || (!riskTO.LastDatePerformed.Equals(new DateTime())) || (!riskTO.LastScheduleDate.Equals(new DateTime())) || (riskTO.LastVisitRecID != 0))
                {
                    sb.Append(" WHERE");

                    if (riskTO.RecID != 0)
                    {
                        sb.Append(" rec_id = '" + riskTO.RecID.ToString().Trim() + "' AND");
                    }
                    if (riskTO.EmployeeID != -1)
                    {
                        sb.Append(" employee_id = '" + riskTO.EmployeeID.ToString().Trim() + "' AND");
                    }
                    if (riskTO.RiskID != -1)
                    {
                        sb.Append(" risk_id = '" + riskTO.RiskID.ToString().Trim() + "' AND");
                    }
                    if (!riskTO.DateStart.Equals(new DateTime()))
                    {
                        sb.Append(" date_start = '" + riskTO.DateStart.ToString(dateTimeformat) + "' AND");
                    }
                    if (!riskTO.DateEnd.Equals(new DateTime()))
                    {
                        sb.Append(" date_end = '" + riskTO.DateEnd.ToString(dateTimeformat) + "' AND");
                    }
                    if (!riskTO.LastDatePerformed.Equals(new DateTime()))
                    {
                        sb.Append(" last_date_performed = '" + riskTO.LastDatePerformed.ToString(dateTimeformat) + "' AND");
                    }
                    if (riskTO.LastVisitRecID != 0)
                    {
                        sb.Append(" last_visit_rec_id = '" + riskTO.LastVisitRecID.ToString().Trim() + "' AND");
                    }
                    if (!riskTO.LastScheduleDate.Equals(new DateTime()))
                    {
                        sb.Append(" last_schedule_date = '" + riskTO.LastScheduleDate.ToString(dateTimeformat) + "' AND");
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

                MySqlCommand cmd = new MySqlCommand(select + " ORDER BY employee_id, date_start", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Risks");
                DataTable table = dataSet.Tables["Risks"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        risk = new EmployeeXRiskTO();
                        if (row["rec_id"] != DBNull.Value)
                        {
                            risk.RecID = UInt32.Parse(row["rec_id"].ToString().Trim());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            risk.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["risk_id"] != DBNull.Value)
                        {
                            risk.RiskID = Int32.Parse(row["risk_id"].ToString().Trim());
                        }
                        if (row["date_start"] != DBNull.Value)
                        {
                            risk.DateStart = DateTime.Parse(row["date_start"].ToString().Trim());
                        }
                        if (row["date_end"] != DBNull.Value)
                        {
                            risk.DateEnd = DateTime.Parse(row["date_end"].ToString().Trim());
                        }
                        if (row["rotation"] != DBNull.Value)
                        {
                            risk.Rotation = Int32.Parse(row["rotation"].ToString().Trim());
                        }
                        if (row["last_date_performed"] != DBNull.Value)
                        {
                            risk.LastDatePerformed = DateTime.Parse(row["last_date_performed"].ToString().Trim());
                        }
                        if (row["last_visit_rec_id"] != DBNull.Value)
                        {
                            risk.LastVisitRecID = UInt32.Parse(row["last_visit_rec_id"].ToString().Trim());
                        }
                        if (row["last_schedule_date"] != DBNull.Value)
                        {
                            risk.LastScheduleDate = DateTime.Parse(row["last_schedule_date"].ToString().Trim());
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

        public int getEmployeeXRisksCount(EmployeeXRiskTO riskTO)
        {
            DataSet dataSet = new DataSet();
            int count = 0;
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT COUNT(*) AS count FROM employees_x_risks ");

                if ((riskTO.RecID != 0) || (riskTO.EmployeeID != -1) || (riskTO.RiskID != -1) || (!riskTO.DateStart.Equals(new DateTime())) || (!riskTO.DateEnd.Equals(new DateTime()))
                    || (riskTO.Rotation != -1) || (!riskTO.LastDatePerformed.Equals(new DateTime())) || (!riskTO.LastScheduleDate.Equals(new DateTime())) || (riskTO.LastVisitRecID != 0))
                {
                    sb.Append(" WHERE");

                    if (riskTO.RecID != 0)
                    {
                        sb.Append(" rec_id = '" + riskTO.RecID.ToString().Trim() + "' AND");
                    }
                    if (riskTO.EmployeeID != -1)
                    {
                        sb.Append(" employee_id = '" + riskTO.EmployeeID.ToString().Trim() + "' AND");
                    }
                    if (riskTO.RiskID != -1)
                    {
                        sb.Append(" risk_id = '" + riskTO.RiskID.ToString().Trim() + "' AND");
                    }
                    if (!riskTO.DateStart.Equals(new DateTime()))
                    {
                        sb.Append(" date_start = '" + riskTO.DateStart.ToString(dateTimeformat) + "' AND");
                    }
                    if (!riskTO.DateEnd.Equals(new DateTime()))
                    {
                        sb.Append(" date_end = '" + riskTO.DateEnd.ToString(dateTimeformat) + "' AND");
                    }
                    if (riskTO.Rotation != -1)
                    {
                        sb.Append(" rotation = '" + riskTO.Rotation.ToString().Trim() + "' AND");
                    }
                    if (!riskTO.LastDatePerformed.Equals(new DateTime()))
                    {
                        sb.Append(" last_date_performed = '" + riskTO.LastDatePerformed.ToString(dateTimeformat) + "' AND");
                    }
                    if (riskTO.LastVisitRecID != 0)
                    {
                        sb.Append(" last_visit_rec_id = '" + riskTO.LastVisitRecID.ToString().Trim() + "' AND");
                    }
                    if (!riskTO.LastScheduleDate.Equals(new DateTime()))
                    {
                        sb.Append(" last_schedule_date = '" + riskTO.LastScheduleDate.ToString(dateTimeformat) + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Risks");
                DataTable table = dataSet.Tables["Risks"];

                if (table.Rows.Count > 0)
                {
                    if (table.Rows[0]["count"] != DBNull.Value)
                    {
                        count = Int32.Parse(table.Rows[0]["count"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return count;
        }

        public List<EmployeeXRiskTO> getEmployeeXRisksNotScheduled(string emplIDs, DateTime from, DateTime to)
        {
            DataSet dataSet = new DataSet();
            EmployeeXRiskTO risk = new EmployeeXRiskTO();
            List<EmployeeXRiskTO> riskList = new List<EmployeeXRiskTO>();
            string select = "";

            try
            {
                select = "SELECT * FROM employees_x_risks WHERE last_schedule_date IS NULL";

                if (emplIDs.Length > 0)
                    select += " AND employee_id IN (" + emplIDs.Trim() + ")";

                if (!from.Equals(new DateTime()))
                    select += " AND (date_end IS NULL OR date_end >= '" + from.Date.ToString(dateTimeformat) + "')";

                if (!to.Equals(new DateTime()))
                    select += " AND date_start < '" + to.Date.AddDays(1).ToString(dateTimeformat) + "'";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Risks");
                DataTable table = dataSet.Tables["Risks"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        risk = new EmployeeXRiskTO();
                        if (row["rec_id"] != DBNull.Value)
                        {
                            risk.RecID = UInt32.Parse(row["rec_id"].ToString().Trim());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            risk.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["risk_id"] != DBNull.Value)
                        {
                            risk.RiskID = Int32.Parse(row["risk_id"].ToString().Trim());
                        }
                        if (row["date_start"] != DBNull.Value)
                        {
                            risk.DateStart = DateTime.Parse(row["date_start"].ToString().Trim());
                        }
                        if (row["date_end"] != DBNull.Value)
                        {
                            risk.DateEnd = DateTime.Parse(row["date_end"].ToString().Trim());
                        }
                        if (row["rotation"] != DBNull.Value)
                        {
                            risk.Rotation = Int32.Parse(row["rotation"].ToString().Trim());
                        }
                        if (row["last_date_performed"] != DBNull.Value)
                        {
                            risk.LastDatePerformed = DateTime.Parse(row["last_date_performed"].ToString().Trim());
                        }
                        if (row["last_visit_rec_id"] != DBNull.Value)
                        {
                            risk.LastVisitRecID = UInt32.Parse(row["last_visit_rec_id"].ToString().Trim());
                        }
                        if (row["last_schedule_date"] != DBNull.Value)
                        {
                            risk.LastScheduleDate = DateTime.Parse(row["last_schedule_date"].ToString().Trim());
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

        public int getMaxRotation()
        {
            DataSet dataSet = new DataSet();
            int max = 0;
            string select = "";

            try
            {
                select = "SELECT MAX(rotation) AS max_rotation FROM employees_x_risks";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Risks");
                DataTable table = dataSet.Tables["Risks"];

                if (table.Rows.Count > 0)
                {
                    if (table.Rows[0]["max_rotation"] != DBNull.Value)
                    {
                        max = Int32.Parse(table.Rows[0]["max_rotation"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return max;
        }

        public Dictionary<uint, EmployeeXRiskTO> getEmployeeXRisks(string emplIDs, string risks, DateTime from, DateTime to)
        {
            DataSet dataSet = new DataSet();
            EmployeeXRiskTO risk = new EmployeeXRiskTO();
            Dictionary<uint, EmployeeXRiskTO> riskList = new Dictionary<uint, EmployeeXRiskTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employees_x_risks ");

                if ((!emplIDs.Trim().Equals("")) || (!risks.Trim().Equals("")) || (!from.Equals(new DateTime())) || (!to.Equals(new DateTime())))
                {
                    sb.Append(" WHERE");

                    if (!emplIDs.Trim().Equals(""))
                    {
                        sb.Append(" employee_id IN (" + emplIDs.Trim() + ") AND");
                    }
                    if (!risks.Trim().Equals(""))
                    {
                        sb.Append(" risk_id IN (" + risks.Trim() + ") AND");
                    }
                    if (!from.Equals(new DateTime()))
                    {
                        sb.Append(" (date_end IS NULL OR date_end > '" + from.Date.ToString(dateTimeformat) + "') AND");
                    }
                    if (!to.Equals(new DateTime()))
                    {
                        sb.Append(" date_start < '" + to.Date.AddDays(1).ToString(dateTimeformat) + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                MySqlCommand cmd = new MySqlCommand(select + " ORDER BY employee_id, date_start", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Risks");
                DataTable table = dataSet.Tables["Risks"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        risk = new EmployeeXRiskTO();
                        if (row["rec_id"] != DBNull.Value)
                        {
                            risk.RecID = UInt32.Parse(row["rec_id"].ToString().Trim());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            risk.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["risk_id"] != DBNull.Value)
                        {
                            risk.RiskID = Int32.Parse(row["risk_id"].ToString().Trim());
                        }
                        if (row["date_start"] != DBNull.Value)
                        {
                            risk.DateStart = DateTime.Parse(row["date_start"].ToString().Trim());
                        }
                        if (row["date_end"] != DBNull.Value)
                        {
                            risk.DateEnd = DateTime.Parse(row["date_end"].ToString().Trim());
                        }
                        if (row["rotation"] != DBNull.Value)
                        {
                            risk.Rotation = Int32.Parse(row["rotation"].ToString().Trim());
                        }
                        if (row["last_date_performed"] != DBNull.Value)
                        {
                            risk.LastDatePerformed = DateTime.Parse(row["last_date_performed"].ToString().Trim());
                        }
                        if (row["last_visit_rec_id"] != DBNull.Value)
                        {
                            risk.LastVisitRecID = UInt32.Parse(row["last_visit_rec_id"].ToString().Trim());
                        }
                        if (row["last_schedule_date"] != DBNull.Value)
                        {
                            risk.LastScheduleDate = DateTime.Parse(row["last_schedule_date"].ToString().Trim());
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

                        if (!riskList.ContainsKey(risk.RecID))
                            riskList.Add(risk.RecID, risk);
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
            _sqlTrans = (MySqlTransaction)trans;
        }
    }
}