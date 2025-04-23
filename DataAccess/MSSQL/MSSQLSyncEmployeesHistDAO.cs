using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TransferObjects;
using System.Globalization;
using Util;

namespace DataAccess
{
    public class MSSQLSyncEmployeesHistDAO:SyncEmployeesHistDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

		public MSSQLSyncEmployeesHistDAO()
		{
            conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

        public MSSQLSyncEmployeesHistDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public bool insert(SyncEmployeeTO syncEmpl, bool doCommit)
        {
            if (doCommit)
            {
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            }
            else
            {
                SqlTrans = this.SqlTrans;
            }
            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO sync_employees_hist ");
                sbInsert.Append("(rec_id, employee_id, employee_id_old, first_name, last_name, tag_id, fs_unit_id, picture,employee_type_id,  ");
                sbInsert.Append("personal_holiday_category, personal_holiday_date, organizational_unit_id,  ");
                sbInsert.Append("responsibility_fs_unit_id, responsibility_ou_unit_id, email_address, JMBG, work_location_id, work_location_code, ");
                sbInsert.Append("username, status, employee_branch, position_id, address, date_of_birth, phone_number_1, phone_number_2, ");
                sbInsert.Append("annual_leave_date_start, annual_leave_current_year, annual_leave_previous_year, language, valid_from, result, remark,annual_leave_current_year_left, annual_leave_previous_year_left, created_by, created_time, created_time_hist) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append(" " + syncEmpl.RecID + ",");
                sbInsert.Append(" " + syncEmpl.EmployeeID + ",");
                if (syncEmpl.EmployeeIDOld != -1)
                {
                    sbInsert.Append(" '" + syncEmpl.EmployeeIDOld + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncEmpl.FirstName != "")
                {
                    sbInsert.Append(" N'" + syncEmpl.FirstName + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncEmpl.LastName != "")
                {
                    sbInsert.Append(" N'" + syncEmpl.LastName + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncEmpl.DeleteTag == Constants.yesInt)
                {
                    sbInsert.Append(" '" + Constants.syncStringNullValue + "',");
                }
                else
                {
                    if (syncEmpl.TagID != 0)
                    {
                        sbInsert.Append(" '" + syncEmpl.TagID + "',");
                    }
                    else
                    {
                        sbInsert.Append("NULL, ");
                    }
                }                
                if (syncEmpl.FsUnitID != -1)
                {
                    sbInsert.Append(" N'" + syncEmpl.FsUnitID + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }                
                if (syncEmpl.Picture != null)
                {
                    sbInsert.Append("@Content, ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncEmpl.EmployeeTypeID!= -1)
                {
                    sbInsert.Append(" '" + syncEmpl.EmployeeTypeID + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncEmpl.PersonalHolidayCategory != -1)
                {
                    sbInsert.Append(" '" + syncEmpl.PersonalHolidayCategory + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncEmpl.PersonalHolidayDate != new DateTime())
                {
                    sbInsert.Append(" '" + syncEmpl.PersonalHolidayDate.ToString(dateTimeformat) + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncEmpl.OrganizationalUnitID != -1)
                {
                    sbInsert.Append(" '" + syncEmpl.OrganizationalUnitID + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncEmpl.ResponsibilityFsUnitID != -1)
                {
                    sbInsert.Append(" '" + syncEmpl.ResponsibilityFsUnitID + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }                
                if (syncEmpl.ResponsibilityOuUnitID != -1)
                {
                    sbInsert.Append(" '" + syncEmpl.ResponsibilityOuUnitID + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }                
                if (syncEmpl.EmailAddress != "")
                {
                    sbInsert.Append(" N'" + syncEmpl.EmailAddress + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }                
                if (syncEmpl.JMBG != "")
                {
                    sbInsert.Append(" N'" + syncEmpl.JMBG + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncEmpl.WorkLocationID != -1)
                {
                    sbInsert.Append(" '" + syncEmpl.WorkLocationID.ToString().Trim() + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncEmpl.WorkLocationCode.Trim() != "")
                {
                    sbInsert.Append(" N'" + syncEmpl.WorkLocationCode.Trim() + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncEmpl.Username != "")
                {
                    sbInsert.Append(" N'" + syncEmpl.Username + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }                
                if (syncEmpl.Status != "")
                {
                    sbInsert.Append(" N'" + syncEmpl.Status + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }                
                if (syncEmpl.EmployeeBranch != "")
                {
                    sbInsert.Append(" N'" + syncEmpl.EmployeeBranch + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncEmpl.PositionID != -1)
                {
                    sbInsert.Append(" '" + syncEmpl.PositionID.ToString() + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncEmpl.Address != "")
                {
                    sbInsert.Append(" N'" + syncEmpl.Address + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncEmpl.DateOfBirth != new DateTime())
                {
                    sbInsert.Append(" '" + syncEmpl.DateOfBirth.ToString(dateTimeformat) + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncEmpl.PhoneNumber1 != "")
                {
                    sbInsert.Append(" N'" + syncEmpl.PhoneNumber1 + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncEmpl.PhoneNumber2 != "")
                {
                    sbInsert.Append(" N'" + syncEmpl.PhoneNumber2 + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncEmpl.AnnualLeaveStartDate != new DateTime())
                {
                    sbInsert.Append(" '" + syncEmpl.AnnualLeaveStartDate.ToString(dateTimeformat) + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncEmpl.AnnualLeaveCurrentYear != -1)
                {
                    sbInsert.Append(" '" + syncEmpl.AnnualLeaveCurrentYear + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }                
                if (syncEmpl.AnnualLeavePreviousYear != -1)
                {
                    sbInsert.Append(" '" + syncEmpl.AnnualLeavePreviousYear + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }                
                if (syncEmpl.Language != "")
                {
                    sbInsert.Append(" '" + syncEmpl.Language + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                } 
                if (syncEmpl.ValidFrom != new DateTime())
                {
                    sbInsert.Append(" '" + syncEmpl.ValidFrom.ToString(dateTimeformat) + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncEmpl.Result != -1)
                {
                    sbInsert.Append(" " + syncEmpl.Result + ",");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncEmpl.Remark != "")
                {
                    sbInsert.Append(" N'" + syncEmpl.Remark + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncEmpl.AnnualLeaveCurrentYearLeft != -1)
                {
                    sbInsert.Append(" '" + syncEmpl.AnnualLeaveCurrentYearLeft + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncEmpl.AnnualLeavePreviousYearLeft != -1)
                {
                    sbInsert.Append(" '" + syncEmpl.AnnualLeavePreviousYearLeft + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncEmpl.CreatedBy != "")
                {
                    sbInsert.Append(" N'" + syncEmpl.CreatedBy + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncEmpl.CreatedTime != new DateTime())
                {
                    sbInsert.Append(" '" + syncEmpl.CreatedTime.ToString(dateTimeformat) + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                sbInsert.Append(" GETDATE()) ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, SqlTrans);
                if(syncEmpl.Picture != null)
                cmd.Parameters.Add("@Content", SqlDbType.Image, syncEmpl.Picture.Length).Value = syncEmpl.Picture;

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

            return rowsAffected > 0;
        }

        public List<SyncEmployeeTO> getEmployees(DateTime from, DateTime to, int emplID, int result)
        {
            DataSet dataSet = new DataSet();
            SyncEmployeeTO emplTO = new SyncEmployeeTO();
            List<SyncEmployeeTO> emplList = new List<SyncEmployeeTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM sync_employees_hist ");

                if (!from.Equals(new DateTime()) || !to.Equals(new DateTime()) || emplID != -1 || result != -1)
                {
                    sb.Append("WHERE ");

                    if (!from.Equals(new DateTime()))
                        sb.Append("created_time_hist >= '" + from.Date.ToString(dateTimeformat) + "' AND ");

                    if (!to.Equals(new DateTime()))
                        sb.Append("created_time_hist < '" + to.Date.AddDays(1).ToString(dateTimeformat) + "' AND ");

                    if (emplID != -1)
                        sb.Append("employee_id = '" + emplID.ToString().Trim() + "' AND ");

                    if (result != -1)
                        sb.Append("result = '" + result.ToString().Trim() + "' AND ");

                    select = sb.ToString(0, sb.ToString().Length - 4);
                }
                else
                    select = sb.ToString();

                select = select + "ORDER BY created_time_hist ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EMPL");
                DataTable table = dataSet.Tables["EMPL"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        emplTO = new SyncEmployeeTO();

                        if (!row["employee_id"].Equals(DBNull.Value))
                        {
                            emplTO.EmployeeID = int.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (!row["employee_id_old"].Equals(DBNull.Value))
                        {
                            int id = -1;
                            if (!int.TryParse(row["employee_id_old"].ToString().Trim(), out id))
                                id = -1;

                            emplTO.EmployeeIDOld = id;
                        }
                        if (!row["first_name"].Equals(DBNull.Value))
                        {
                            emplTO.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (!row["last_name"].Equals(DBNull.Value))
                        {
                            emplTO.LastName = row["last_name"].ToString().Trim();
                        }
                        if (!row["tag_id"].Equals(DBNull.Value))
                        {
                            uint id = 0;
                            if (!uint.TryParse(row["tag_id"].ToString().Trim(), out id))
                                id = 0;

                            emplTO.TagID = id;
                        }
                        if (!row["fs_unit_id"].Equals(DBNull.Value))
                        {
                            emplTO.FsUnitID = int.Parse(row["fs_unit_id"].ToString().Trim());
                        }
                        if (!row["picture"].Equals(DBNull.Value))
                        {
                            emplTO.Picture = (byte[])row["picture"];
                        }
                        if (!row["employee_type_id"].Equals(DBNull.Value))
                        {
                            emplTO.EmployeeTypeID = int.Parse(row["employee_type_id"].ToString().Trim());
                        }
                        if (!row["personal_holiday_category"].Equals(DBNull.Value))
                        {
                            emplTO.PersonalHolidayCategory = int.Parse(row["personal_holiday_category"].ToString().Trim());
                        }
                        if (!row["personal_holiday_date"].Equals(DBNull.Value))
                        {
                            emplTO.PersonalHolidayDate = DateTime.Parse(row["personal_holiday_date"].ToString().Trim());
                        }
                        if (!row["organizational_unit_id"].Equals(DBNull.Value))
                        {
                            emplTO.OrganizationalUnitID = int.Parse(row["organizational_unit_id"].ToString().Trim());
                        }
                        if (!row["email_address"].Equals(DBNull.Value))
                        {
                            emplTO.EmailAddress = row["email_address"].ToString().Trim();
                        }
                        if (!row["JMBG"].Equals(DBNull.Value))
                        {
                            emplTO.JMBG = row["JMBG"].ToString().Trim();
                        }
                        if (!row["username"].Equals(DBNull.Value))
                        {
                            emplTO.Username = row["username"].ToString().Trim();
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            emplTO.Status = row["status"].ToString().Trim();
                        }
                        if (!row["employee_branch"].Equals(DBNull.Value))
                        {
                            emplTO.EmployeeBranch = row["employee_branch"].ToString().Trim();
                        }
                        if (!row["annual_leave_date_start"].Equals(DBNull.Value))
                        {
                            emplTO.AnnualLeaveStartDate = DateTime.Parse(row["annual_leave_date_start"].ToString().Trim());
                        }
                        if (!row["annual_leave_current_year"].Equals(DBNull.Value))
                        {
                            emplTO.AnnualLeaveCurrentYear = int.Parse(row["annual_leave_current_year"].ToString().Trim());
                        }
                        if (!row["annual_leave_previous_year"].Equals(DBNull.Value))
                        {
                            emplTO.AnnualLeavePreviousYear = int.Parse(row["annual_leave_previous_year"].ToString().Trim());
                        }
                        if (!row["annual_leave_current_year_left"].Equals(DBNull.Value))
                        {
                            emplTO.AnnualLeaveCurrentYearLeft = int.Parse(row["annual_leave_current_year_left"].ToString().Trim());
                        }
                        if (!row["annual_leave_previous_year_left"].Equals(DBNull.Value))
                        {
                            emplTO.AnnualLeavePreviousYearLeft = int.Parse(row["annual_leave_previous_year_left"].ToString().Trim());
                        }
                        if (!row["language"].Equals(DBNull.Value))
                        {
                            emplTO.Language = row["language"].ToString().Trim();
                        }
                        if (!row["work_location_id"].Equals(DBNull.Value))
                        {
                            emplTO.WorkLocationID = int.Parse(row["work_location_id"].ToString().Trim());
                        }
                        if (!row["work_location_code"].Equals(DBNull.Value))
                        {
                            emplTO.WorkLocationCode = row["work_location_code"].ToString().Trim();
                        }
                        if (!row["position_id"].Equals(DBNull.Value))
                        {
                            emplTO.PositionID = int.Parse(row["position_id"].ToString().Trim());
                        }
                        if (!row["address"].Equals(DBNull.Value))
                        {
                            emplTO.Address = row["address"].ToString().Trim();
                        }
                        if (!row["date_of_birth"].Equals(DBNull.Value))
                        {
                            emplTO.DateOfBirth = DateTime.Parse(row["date_of_birth"].ToString().Trim());
                        }
                        if (!row["phone_number_1"].Equals(DBNull.Value))
                        {
                            emplTO.PhoneNumber1 = row["phone_number_1"].ToString().Trim();
                        }
                        if (!row["phone_number_2"].Equals(DBNull.Value))
                        {
                            emplTO.PhoneNumber2 = row["phone_number_2"].ToString().Trim();
                        }
                        if (!row["valid_from"].Equals(DBNull.Value))
                        {
                            emplTO.ValidFrom = DateTime.Parse(row["valid_from"].ToString().Trim());
                        }
                        if (!row["created_by"].Equals(DBNull.Value))
                        {
                            emplTO.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (!row["created_time"].Equals(DBNull.Value))
                        {
                            emplTO.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (!row["created_time_hist"].Equals(DBNull.Value))
                        {
                            emplTO.CreatedTimeHist = DateTime.Parse(row["created_time_hist"].ToString().Trim());
                        }
                        if (!row["result"].Equals(DBNull.Value))
                        {
                            emplTO.Result = int.Parse(row["result"].ToString().Trim());
                        }
                        if (!row["remark"].Equals(DBNull.Value))
                        {
                            emplTO.Remark = row["remark"].ToString().Trim();
                        }

                        emplList.Add(emplTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return emplList;
        }

        public int getLastOU(int employeeID)
        {
            DataSet dataSet = new DataSet();
            int ouID = -1;
            string select = "";

            try
            {
                if (employeeID == -1)
                    return ouID;
                
                select = "SELECT TOP 1 organizational_unit_id FROM sync_employees_hist WHERE employee_id = '" + employeeID.ToString().Trim() + "' AND organizational_unit_id IS NOT NULL ORDER BY created_time_hist DESC";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EMPL");
                DataTable table = dataSet.Tables["EMPL"];

                if (table.Rows.Count > 0)
                {
                    if (!table.Rows[0]["organizational_unit_id"].Equals(DBNull.Value))
                    {
                        ouID = int.Parse(table.Rows[0]["organizational_unit_id"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return ouID;
        }

        public int getLastWU(int employeeID)
        {
            DataSet dataSet = new DataSet();
            int wuID = -1;
            string select = "";

            try
            {
                if (employeeID == -1)
                    return wuID;

                select = "SELECT TOP 1 fs_unit_id FROM sync_employees_hist WHERE employee_id = '" + employeeID.ToString().Trim() + "' AND fs_unit_id IS NOT NULL ORDER BY created_time_hist DESC";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EMPL");
                DataTable table = dataSet.Tables["EMPL"];

                if (table.Rows.Count > 0)
                {
                    if (!table.Rows[0]["fs_unit_id"].Equals(DBNull.Value))
                    {
                        wuID = int.Parse(table.Rows[0]["fs_unit_id"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return wuID;
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
