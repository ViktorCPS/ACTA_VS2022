using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Data;
using MySql.Data.MySqlClient;

using TransferObjects;

namespace DataAccess
{
    public class MySQLEmployeeXMedicalCheckDisabilityDAO : EmployeesXMedicalCheckDisabilityDAO
    {
        MySqlConnection conn = null;
        MySqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MySQLEmployeeXMedicalCheckDisabilityDAO()
        {
            conn = MySQLDAOFactory.getConnection();
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MySQLEmployeeXMedicalCheckDisabilityDAO(MySqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(EmployeeXMedicalCheckDisabilityTO disTO, bool doCommit)
        {
            int rowsAffected = 0;

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employees_x_medical_chk_disabilities ");
                sbInsert.Append("(employee_id, disability_id, type, date_start, date_end, note, flag_email, flag_email_created_time, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("'" + disTO.EmployeeID.ToString().Trim() + "', ");
                sbInsert.Append("'" + disTO.DisabilityID.ToString().Trim() + "', ");
                sbInsert.Append("N'" + disTO.Type.ToString().Trim() + "', ");
                sbInsert.Append("'" + disTO.DateStart.ToString(dateTimeformat) + "', ");
                if (!disTO.DateEnd.Equals(new DateTime()))
                    sbInsert.Append("'" + disTO.DateEnd.ToString(dateTimeformat) + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (!disTO.Note.Trim().Equals(""))
                    sbInsert.Append("N'" + disTO.Note.Trim() + "', ");
                else
                    sbInsert.Append("NULL, ");
                sbInsert.Append("'" + disTO.FlagEmail.ToString().Trim() + "', ");
                if (!disTO.FlagEmailCratedTime.Equals(new DateTime()))
                    sbInsert.Append("'" + disTO.FlagEmailCratedTime.ToString(dateTimeformat) + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (!disTO.CreatedBy.Trim().Equals(""))
                    sbInsert.Append("N'" + disTO.CreatedBy.Trim() + "', ");
                else
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!disTO.CreatedTime.Equals(new DateTime()))
                    sbInsert.Append("'" + disTO.CreatedTime.ToString(dateTimeformat) + "') ");
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

        public bool update(EmployeeXMedicalCheckDisabilityTO disTO, bool doCommit)
        {
            bool isUpdated = false;

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE employees_x_medical_chk_disabilities SET ");
                if (disTO.EmployeeID != -1)
                    sbUpdate.Append("employee_id = '" + disTO.EmployeeID.ToString().Trim() + "', ");
                if (disTO.DisabilityID != -1)
                    sbUpdate.Append("disability_id = '" + disTO.DisabilityID.ToString().Trim() + "', ");
                if (!disTO.DateStart.Equals(new DateTime()))
                    sbUpdate.Append("date_start = '" + disTO.DateStart.ToString(dateTimeformat) + "', ");
                if (!disTO.DateEnd.Equals(new DateTime()))
                    sbUpdate.Append("date_end = '" + disTO.DateEnd.ToString(dateTimeformat) + "', ");
                else
                    sbUpdate.Append("date_end = NULL, ");
                if (!disTO.Note.Trim().Equals(""))
                    sbUpdate.Append("note = N'" + disTO.Note.Trim() + "', ");
                else
                    sbUpdate.Append("note = NULL, ");
                if (disTO.FlagEmail != -1)
                    sbUpdate.Append("flag_email = '" + disTO.FlagEmail.ToString().Trim() + "', ");
                if (!disTO.FlagEmailCratedTime.Equals(new DateTime()))
                    sbUpdate.Append("flag_email_created_time = '" + disTO.FlagEmailCratedTime.ToString(dateTimeformat) + "', ");
                else
                    sbUpdate.Append("flag_email_created_time = NULL, ");
                if (!disTO.Type.Trim().Equals(""))
                    sbUpdate.Append("type = '" + disTO.Type.Trim() + "', ");
                if (!disTO.ModifiedBy.Trim().Equals(""))
                    sbUpdate.Append("modified_by = N'" + disTO.ModifiedBy.Trim() + "', ");
                else
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!disTO.ModifiedTime.Equals(new DateTime()))
                    sbUpdate.Append("modified_time = '" + disTO.ModifiedTime.ToString(dateTimeformat) + "' ");
                else
                    sbUpdate.Append("modified_time = NOW() ");
                sbUpdate.Append("WHERE rec_id = '" + disTO.RecID.ToString().Trim() + "'");

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
                sbDelete.Append("DELETE FROM employees_x_medical_chk_disabilities WHERE rec_id IN (" + recID.ToString().Trim() + ")");

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

        public List<EmployeeXMedicalCheckDisabilityTO> getEmployeeXMedicalCheckDisabilities(EmployeeXMedicalCheckDisabilityTO disTO)
        {
            DataSet dataSet = new DataSet();
            EmployeeXMedicalCheckDisabilityTO dis = new EmployeeXMedicalCheckDisabilityTO();
            List<EmployeeXMedicalCheckDisabilityTO> disList = new List<EmployeeXMedicalCheckDisabilityTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employees_x_medical_chk_disabilities ");

                if ((disTO.RecID != 0) || (disTO.EmployeeID != -1) || (disTO.DisabilityID != -1) || (!disTO.Type.Trim().Equals(""))
                    || (!disTO.DateStart.Equals(new DateTime())) || (!disTO.DateEnd.Equals(new DateTime())) || (!disTO.Note.Trim().Equals(""))
                    || (disTO.FlagEmail != -1) || (!disTO.FlagEmailCratedTime.Equals(new DateTime())))
                {
                    sb.Append(" WHERE");

                    if (disTO.RecID != 0)
                    {
                        sb.Append(" rec_id = '" + disTO.RecID.ToString().Trim() + "' AND");
                    }
                    if (disTO.EmployeeID != -1)
                    {
                        sb.Append(" employee_id = '" + disTO.EmployeeID.ToString().Trim() + "' AND");
                    }
                    if (disTO.DisabilityID != -1)
                    {
                        sb.Append(" disability_id = '" + disTO.DisabilityID.ToString().Trim() + "' AND");
                    }
                    if (!disTO.Type.Trim().Equals(""))
                    {
                        sb.Append(" type = N'" + disTO.Type.ToString().Trim() + "' AND");
                    }
                    if (!disTO.Note.Trim().Equals(""))
                    {
                        sb.Append(" note = N'" + disTO.Note.ToString().Trim() + "' AND");
                    }
                    if (!disTO.DateStart.Equals(new DateTime()))
                    {
                        sb.Append(" date_start = '" + disTO.DateStart.ToString(dateTimeformat) + "' AND");
                    }
                    if (!disTO.DateEnd.Equals(new DateTime()))
                    {
                        sb.Append(" date_end = '" + disTO.DateEnd.ToString(dateTimeformat) + "' AND");
                    }
                    if (disTO.FlagEmail != -1)
                    {
                        sb.Append(" flag_email = '" + disTO.FlagEmail.ToString().Trim() + "' AND");
                    }
                    if (!disTO.FlagEmailCratedTime.Equals(new DateTime()))
                    {
                        sb.Append(" flag_email_created_time = '" + disTO.FlagEmailCratedTime.ToString(dateTimeformat) + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                MySqlCommand cmd = new MySqlCommand(select + " ORDER BY employee_id, date_start", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Disabilities");
                DataTable table = dataSet.Tables["Disabilities"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        dis = new EmployeeXMedicalCheckDisabilityTO();
                        if (row["rec_id"] != DBNull.Value)
                        {
                            dis.RecID = UInt32.Parse(row["rec_id"].ToString().Trim());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            dis.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["disability_id"] != DBNull.Value)
                        {
                            dis.DisabilityID = Int32.Parse(row["disability_id"].ToString().Trim());
                        }
                        if (row["type"] != DBNull.Value)
                        {
                            dis.Type = row["type"].ToString().Trim();
                        }
                        if (row["note"] != DBNull.Value)
                        {
                            dis.Note = row["note"].ToString().Trim();
                        }
                        if (row["date_start"] != DBNull.Value)
                        {
                            dis.DateStart = DateTime.Parse(row["date_start"].ToString().Trim());
                        }
                        if (row["date_end"] != DBNull.Value)
                        {
                            dis.DateEnd = DateTime.Parse(row["date_end"].ToString().Trim());
                        }
                        if (row["flag_email"] != DBNull.Value)
                        {
                            dis.FlagEmail = Int32.Parse(row["flag_email"].ToString().Trim());
                        }
                        if (row["flag_email_created_time"] != DBNull.Value)
                        {
                            dis.FlagEmailCratedTime = DateTime.Parse(row["flag_email_created_time"].ToString().Trim());
                        }  
                        if (row["created_by"] != DBNull.Value)
                        {
                            dis.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            dis.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            dis.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            dis.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }

                        disList.Add(dis);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return disList;
        }

        public Dictionary<uint, EmployeeXMedicalCheckDisabilityTO> getEmployeeXMedicalCheckDisabilities(string emplIDs, string data, DateTime from, DateTime to)
        {
            DataSet dataSet = new DataSet();
            EmployeeXMedicalCheckDisabilityTO dis = new EmployeeXMedicalCheckDisabilityTO();
            Dictionary<uint, EmployeeXMedicalCheckDisabilityTO> disList = new Dictionary<uint, EmployeeXMedicalCheckDisabilityTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employees_x_medical_chk_disabilities ");

                if ((!emplIDs.Trim().Equals("")) || (!data.Trim().Equals("")) || (!from.Equals(new DateTime())) || (!to.Equals(new DateTime())))
                {
                    sb.Append(" WHERE");

                    if (!emplIDs.Trim().Equals(""))
                    {
                        sb.Append(" employee_id IN (" + emplIDs.Trim() + ") AND");
                    }
                    if (!data.Trim().Equals(""))
                    {
                        sb.Append(" disability_id IN (" + data.Trim() + ") AND");
                    }
                    if (!from.Equals(new DateTime()))
                    {
                        sb.Append(" (date_end IS NULL OR date_end >= '" + from.Date.ToString(dateTimeformat) + "') AND");
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

                sqlDataAdapter.Fill(dataSet, "Disabilities");
                DataTable table = dataSet.Tables["Disabilities"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        dis = new EmployeeXMedicalCheckDisabilityTO();
                        if (row["rec_id"] != DBNull.Value)
                        {
                            dis.RecID = UInt32.Parse(row["rec_id"].ToString().Trim());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            dis.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["disability_id"] != DBNull.Value)
                        {
                            dis.DisabilityID = Int32.Parse(row["disability_id"].ToString().Trim());
                        }
                        if (row["type"] != DBNull.Value)
                        {
                            dis.Type = row["type"].ToString().Trim();
                        }
                        if (row["note"] != DBNull.Value)
                        {
                            dis.Note = row["note"].ToString().Trim();
                        }
                        if (row["date_start"] != DBNull.Value)
                        {
                            dis.DateStart = DateTime.Parse(row["date_start"].ToString().Trim());
                        }
                        if (row["date_end"] != DBNull.Value)
                        {
                            dis.DateEnd = DateTime.Parse(row["date_end"].ToString().Trim());
                        }
                        if (row["flag_email"] != DBNull.Value)
                        {
                            dis.FlagEmail = Int32.Parse(row["flag_email"].ToString().Trim());
                        }
                        if (row["flag_email_created_time"] != DBNull.Value)
                        {
                            dis.FlagEmailCratedTime = DateTime.Parse(row["flag_email_created_time"].ToString().Trim());
                        }  
                        if (row["created_by"] != DBNull.Value)
                        {
                            dis.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            dis.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            dis.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            dis.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }

                        if (!disList.ContainsKey(dis.RecID))
                            disList.Add(dis.RecID, dis);
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
            _sqlTrans = (MySqlTransaction)trans;
        }
    }
}