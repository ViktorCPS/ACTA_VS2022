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
    public class MSSQLEmployeeXVaccineDAO : EmployeeXVaccineDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }
        public MSSQLEmployeeXVaccineDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MSSQLEmployeeXVaccineDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(EmployeeXVaccineTO vacTO, bool doCommit)
        {
            int rowsAffected = 0;

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
                
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employee_x_vaccines ");
                sbInsert.Append("(vaccine_id, employee_id, date_performed, rotation, rotation_flag_used, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("'" + vacTO.VaccineID.ToString().Trim() + "', ");
                sbInsert.Append("'" + vacTO.EmployeeID.ToString().Trim() + "', ");                
                sbInsert.Append("'" + vacTO.DatePerformed.ToString(dateTimeformat) + "', ");
                if (vacTO.Rotation != -1)
                    sbInsert.Append("'" + vacTO.Rotation.ToString().Trim() + "', ");
                else
                    sbInsert.Append("NULL, ");
                sbInsert.Append("'" + vacTO.RotationFlagUsed.ToString().Trim() + "', ");
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

        public bool update(EmployeeXVaccineTO vacTO, bool doCommit)
        {
            bool isUpdated = false;

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
                
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE employee_x_vaccines SET ");
                if (vacTO.VaccineID != -1)
                    sbUpdate.Append("vaccine_id = '" + vacTO.VaccineID.ToString().Trim() + "', ");
                if (vacTO.EmployeeID != -1)
                    sbUpdate.Append("employee_id = '" + vacTO.EmployeeID.ToString().Trim() + "', ");                
                if (!vacTO.DatePerformed.Equals(new DateTime()))
                    sbUpdate.Append("date_performed = '" + vacTO.DatePerformed.ToString(dateTimeformat) + "', ");
                if (vacTO.Rotation != -1)
                    sbUpdate.Append("rotation = '" + vacTO.Rotation.ToString().Trim() + "', ");
                else
                    sbUpdate.Append("rotation = NULL, ");
                if (vacTO.RotationFlagUsed != -1)
                    sbUpdate.Append("rotation_flag_used = '" + vacTO.RotationFlagUsed.ToString().Trim() + "', ");
                if (!vacTO.ModifiedBy.Trim().Equals(""))
                    sbUpdate.Append("modified_by = N'" + vacTO.ModifiedBy.Trim() + "', ");
                else
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!vacTO.ModifiedTime.Equals(new DateTime()))
                    sbUpdate.Append("modified_time = '" + vacTO.ModifiedTime.ToString(dateTimeformat) + "' ");
                else
                    sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE rec_id = '" + vacTO.RecID.ToString().Trim() + "'");

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

        public bool delete(string recID, bool doCommit)
        {
            bool isDeleted = false;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM employee_x_vaccines WHERE rec_id IN (" + recID.ToString().Trim() + ")");

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

        public List<EmployeeXVaccineTO> getEmployeeXVaccines(EmployeeXVaccineTO vacTO)
        {
            DataSet dataSet = new DataSet();
            EmployeeXVaccineTO vac = new EmployeeXVaccineTO();
            List<EmployeeXVaccineTO> vacList = new List<EmployeeXVaccineTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employee_x_vaccines ");                
                if ((vacTO.RecID != 0) || (vacTO.VaccineID != -1) || (vacTO.EmployeeID != -1) || (!vacTO.DatePerformed.Equals(new DateTime()))
                    || (vacTO.Rotation != -1) || (vacTO.RotationFlagUsed != -1))
                {
                    sb.Append(" WHERE");

                    if (vacTO.RecID != 0)
                    {
                        sb.Append(" rec_id = '" + vacTO.RecID.ToString().Trim() + "' AND");
                    }
                    if (vacTO.VaccineID != -1)
                    {
                        sb.Append(" vaccine_id = '" + vacTO.VaccineID.ToString().Trim() + "' AND");
                    }
                    if (vacTO.EmployeeID != -1)
                    {
                        sb.Append(" employee_id = '" + vacTO.EmployeeID.ToString().Trim() + "' AND");
                    }
                    if (!vacTO.DatePerformed.Equals(new DateTime()))
                    {
                        sb.Append(" date_performed = '" + vacTO.DatePerformed.ToString(dateTimeformat) + "' AND");
                    }                    
                    if (vacTO.Rotation != -1)
                    {
                        sb.Append(" rotation = '" + vacTO.Rotation.ToString().Trim() + "' AND");
                    }
                    if (vacTO.RotationFlagUsed != -1)
                    {
                        sb.Append(" rotation_flag_used = '" + vacTO.RotationFlagUsed.ToString().Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                SqlCommand cmd = new SqlCommand(select + " ORDER BY employee_id, date_performed", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Vaccines");
                DataTable table = dataSet.Tables["Vaccines"];
               
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        vac = new EmployeeXVaccineTO();
                        if (row["rec_id"] != DBNull.Value)
                        {
                            vac.RecID = UInt32.Parse(row["rec_id"].ToString().Trim());
                        }
                        if (row["vaccine_id"] != DBNull.Value)
                        {
                            vac.VaccineID = Int32.Parse(row["vaccine_id"].ToString().Trim());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            vac.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["date_performed"] != DBNull.Value)
                        {
                            vac.DatePerformed = DateTime.Parse(row["date_performed"].ToString().Trim());
                        }                        
                        if (row["rotation"] != DBNull.Value)
                        {
                            vac.Rotation = Int32.Parse(row["rotation"].ToString().Trim());
                        }
                        if (row["rotation_flag_used"] != DBNull.Value)
                        {
                            vac.RotationFlagUsed = Int32.Parse(row["rotation_flag_used"].ToString().Trim());
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

        public List<EmployeeXVaccineTO> getEmployeeXVaccinesNotProcessed(string emplIDs)
        {
            DataSet dataSet = new DataSet();
            EmployeeXVaccineTO vac = new EmployeeXVaccineTO();
            List<EmployeeXVaccineTO> vacList = new List<EmployeeXVaccineTO>();
            string select = "";

            try
            {
                select = "SELECT * FROM employee_x_vaccines WHERE rotation IS NOT NULL AND rotation_flag_used = '" + Constants.noInt.ToString().Trim() + "'";

                if (emplIDs.Length > 0)
                    select += " AND employee_id IN (" + emplIDs.Trim() + ")";
                
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Vaccines");
                DataTable table = dataSet.Tables["Vaccines"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        vac = new EmployeeXVaccineTO();
                        if (row["rec_id"] != DBNull.Value)
                        {
                            vac.RecID = UInt32.Parse(row["rec_id"].ToString().Trim());
                        }
                        if (row["vaccine_id"] != DBNull.Value)
                        {
                            vac.VaccineID = Int32.Parse(row["vaccine_id"].ToString().Trim());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            vac.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["date_performed"] != DBNull.Value)
                        {
                            vac.DatePerformed = DateTime.Parse(row["date_performed"].ToString().Trim());
                        }
                        if (row["rotation"] != DBNull.Value)
                        {
                            vac.Rotation = Int32.Parse(row["rotation"].ToString().Trim());
                        }
                        if (row["rotation_flag_used"] != DBNull.Value)
                        {
                            vac.RotationFlagUsed = Int32.Parse(row["rotation_flag_used"].ToString().Trim());
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

        public Dictionary<uint, EmployeeXVaccineTO> getEmployeeXVaccines(string emplIDs, string vaccines, DateTime from, DateTime to)
        {
            DataSet dataSet = new DataSet();
            EmployeeXVaccineTO vac = new EmployeeXVaccineTO();
            Dictionary<uint, EmployeeXVaccineTO> vacList = new Dictionary<uint, EmployeeXVaccineTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employee_x_vaccines ");
                if ((!emplIDs.Trim().Equals("")) || (!vaccines.Trim().Equals("")) || (!from.Equals(new DateTime())) || (!to.Equals(new DateTime())))
                {
                    sb.Append(" WHERE");

                    if (!emplIDs.Trim().Equals(""))
                    {
                        sb.Append(" employee_id IN (" + emplIDs.Trim() + ") AND");
                    }
                    if (!vaccines.Trim().Equals(""))
                    {
                        sb.Append(" vaccine_id IN (" + vaccines.Trim() + ") AND");
                    }
                    if (!from.Equals(new DateTime()))
                    {
                        sb.Append(" date_performed >= '" + from.Date.ToString(dateTimeformat) + "' AND");
                    }
                    if (!to.Equals(new DateTime()))
                    {
                        sb.Append(" date_performed < '" + to.Date.AddDays(1).ToString(dateTimeformat) + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                SqlCommand cmd = new SqlCommand(select + " ORDER BY employee_id, date_performed", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Vaccines");
                DataTable table = dataSet.Tables["Vaccines"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        vac = new EmployeeXVaccineTO();
                        if (row["rec_id"] != DBNull.Value)
                        {
                            vac.RecID = UInt32.Parse(row["rec_id"].ToString().Trim());
                        }
                        if (row["vaccine_id"] != DBNull.Value)
                        {
                            vac.VaccineID = Int32.Parse(row["vaccine_id"].ToString().Trim());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            vac.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["date_performed"] != DBNull.Value)
                        {
                            vac.DatePerformed = DateTime.Parse(row["date_performed"].ToString().Trim());
                        }
                        if (row["rotation"] != DBNull.Value)
                        {
                            vac.Rotation = Int32.Parse(row["rotation"].ToString().Trim());
                        }
                        if (row["rotation_flag_used"] != DBNull.Value)
                        {
                            vac.RotationFlagUsed = Int32.Parse(row["rotation_flag_used"].ToString().Trim());
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

                        if (!vacList.ContainsKey(vac.RecID))
                            vacList.Add(vac.RecID, vac);
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
