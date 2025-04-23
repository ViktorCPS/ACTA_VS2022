using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using TransferObjects;
using System.Data;
using Util;
using System.Configuration;

namespace SyncDataAccess
{
    public class MSSQLSynchronizationDAO:SynchronizationDAO
    {
        public static string ConnectionString = "";
        // thread-safe locker
        private static object locker = new object();
        public static SqlConnection connection;
        private SqlConnection tsConnection = null;

        public MSSQLSynchronizationDAO()
        {
            lock (locker)
            {
                // Decrypt user name and password
                //Crypt cr = new Crypt();

                // Decrypt connection string
                ConnectionString = ConfigurationManager.AppSettings["syncConnectionString"];

                byte[] buffer = Convert.FromBase64String(ConnectionString);
                ConnectionString = Util.Misc.decrypt(buffer);

                // ConnectionString contains data provader and it should be ejected from connection string
                int startIndex = -1;

                startIndex = ConnectionString.ToLower().IndexOf("server=");

                if (startIndex >= 0)
                {
                    ConnectionString = ConnectionString.Substring(startIndex);
                }

                //ConnectionString = cr.DecryptConnectionString(ConnectionString);
            }
        }

        public override bool TestDataSourceConnection()
        {
            bool isConnected = false;

            try
            {
                SqlConnection testConn = getConnection();
                
                tsConnection = MakeNewDBConnection() as SqlConnection;
                
            }
            catch
            {
                return isConnected;
            }

            return isConnected;
        }

        public static SqlConnection getConnection()
        {
            lock (locker)
            {
                try
                {
                    if (connection == null)
                    {
                        // Set connection for the first time
                        connection = new SqlConnection(ConnectionString);
                        connection.Open();
                        return connection;
                    }
                    else
                    {
                        // TODO: Check if connection is closed or opened
                        // Connection already established
                        return connection;
                    }
                }
                catch (InvalidOperationException ioex)
                {
                    connection = null;
                    throw ioex;
                }
                catch (SqlException sqlex)
                {
                    connection = null;
                    throw sqlex;
                }
                catch (Exception ex)
                {
                    connection = null;
                    throw ex;
                }
            }
        }

        public override void CloseConnection()
        {
            lock (locker)
            {
                try
                {
                    if (connection != null) connection.Close();
                }
                catch { }
                finally
                {
                    connection = null;
                }
            }
        }

        public override void CloseConnection(object dbConnection)
        {
            try
            {
                if (dbConnection != null)
                {
                    SqlConnection conn = dbConnection as SqlConnection;
                    if (conn != null)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            catch { }
        }

        public override void setDBConnection(object dbConnection)
        {
            try
            {
                if (dbConnection != null)
                {
                    connection = dbConnection as SqlConnection;                   
                }
            }
            catch { }
        }

        public override object MakeNewDBConnection()
        {
            lock (locker)
            {
                SqlConnection sqlConnection = null;
                try
                {
                    sqlConnection = new SqlConnection(ConnectionString);
                    sqlConnection.Open();
                    return sqlConnection;
                }
                catch (InvalidOperationException ioex)
                {
                    throw ioex;
                }
                catch (SqlException sqlex)
                {
                    throw sqlex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public override List<SyncEmployeeTO> getDifEmployees()
        {
            DataSet dataSet = new DataSet();
            SyncEmployeeTO employee = new SyncEmployeeTO();
            List<SyncEmployeeTO> employeesList = new List<SyncEmployeeTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from " + DataBase + "." + User + ".sync_employees");

                select = sb.ToString();
                select += " ORDER BY rec_id ";

                SqlCommand cmd = new SqlCommand(select, connection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        employee = new SyncEmployeeTO(); 
                        employee.RecID = Int32.Parse(row["rec_id"].ToString().Trim());
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["employee_id_old"] != DBNull.Value)
                        {
                            int res = 0;
                            bool isNum = Int32.TryParse(row["employee_id_old"].ToString().Trim(), out res);
                            employee.EmployeeIDOld = res;
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            employee.FirstName = getValidString(row["first_name"].ToString().Trim());
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            employee.LastName = getValidString(row["last_name"].ToString().Trim());
                        }
                        if (row["tag_id"] != DBNull.Value)
                        {
                            uint tagID = 0;
                            if (row["tag_id"].ToString().ToUpper() == Constants.syncStringNullValue)
                                employee.DeleteTag = Constants.yesInt;
                            else
                            {
                                if (!uint.TryParse(row["tag_id"].ToString().Trim(), out tagID))
                                    tagID = 0;
                            }

                            employee.TagID = tagID;
                        }
                        if (row["valid_from"] != DBNull.Value)
                        {
                            employee.ValidFrom = DateTime.Parse(row["valid_from"].ToString().Trim());                            
                        }
                        if (row["fs_unit_id"] != DBNull.Value)
                        {
                            employee.FsUnitID = int.Parse(row["fs_unit_id"].ToString().Trim());
                        }                        
                        if (row["picture"] != DBNull.Value)
                        {
                            employee.Picture = (byte[])row["picture"];
                        }
                        if (row["employee_type_id"] != DBNull.Value)
                        {
                            employee.EmployeeTypeID = int.Parse(row["employee_type_id"].ToString().Trim());
                        }
                        if (row["personal_holiday_category"] != DBNull.Value)
                        {
                            employee.PersonalHolidayCategory = int.Parse(row["personal_holiday_category"].ToString().Trim());
                            if (employee.PersonalHolidayCategory == -1)
                                employee.PersonalHolidayCategory = Constants.syncIntNullValue;
                        }
                        if (row["personal_holiday_date"] != DBNull.Value)
                        {
                            employee.PersonalHolidayDate = DateTime.Parse(row["personal_holiday_date"].ToString().Trim());
                        }
                        if (row["organizational_unit_id"] != DBNull.Value)
                        {
                            employee.OrganizationalUnitID = int.Parse(row["organizational_unit_id"].ToString().Trim());
                        }
                        
                        if (row["email_address"] != DBNull.Value)
                        {
                            employee.EmailAddress = row["email_address"].ToString().Trim();
                        }                        
                        if (row["JMBG"] != DBNull.Value)
                        {
                            employee.JMBG = row["JMBG"].ToString().Trim();
                        }
                        if (row["username"] != DBNull.Value)
                        {
                            employee.Username = row["username"].ToString().Trim();
                        }
                        if (row["status"] != DBNull.Value)
                        {
                            employee.Status = row["status"].ToString().Trim();
                        }
                        if (row["employee_branch"] != DBNull.Value)
                        {
                            employee.EmployeeBranch = row["employee_branch"].ToString().Trim();
                        }
                        if (row["annual_leave_current_year"] != DBNull.Value)
                        {
                            employee.AnnualLeaveCurrentYear = int.Parse(row["annual_leave_current_year"].ToString().Trim());
                            if (employee.AnnualLeaveCurrentYear == -1)
                                employee.AnnualLeaveCurrentYear = Constants.syncIntNullValue;
                        }
                        if (row["annual_leave_current_year_left"] != DBNull.Value)
                        {
                            employee.AnnualLeaveCurrentYearLeft = int.Parse(row["annual_leave_current_year_left"].ToString().Trim());
                            if (employee.AnnualLeaveCurrentYearLeft == -1)
                                employee.AnnualLeaveCurrentYearLeft = Constants.syncIntNullValue;
                        }
                        if (row["annual_leave_previous_year"] != DBNull.Value)
                        {
                            employee.AnnualLeavePreviousYear = int.Parse(row["annual_leave_previous_year"].ToString().Trim());
                            if (employee.AnnualLeavePreviousYear == -1)
                                employee.AnnualLeavePreviousYear = Constants.syncIntNullValue;
                        }
                        if (row["annual_leave_previous_year_left"] != DBNull.Value)
                        {
                            employee.AnnualLeavePreviousYearLeft = int.Parse(row["annual_leave_previous_year_left"].ToString().Trim());
                            if (employee.AnnualLeavePreviousYearLeft == -1)
                                employee.AnnualLeavePreviousYearLeft = Constants.syncIntNullValue;
                        }
                        if (row["annual_leave_start_date"] != DBNull.Value)
                        {
                            employee.AnnualLeaveStartDate = DateTime.Parse(row["annual_leave_start_date"].ToString().Trim());
                        }
                        if (row["work_location_id"] != DBNull.Value)
                        {
                            employee.WorkLocationID = Int32.Parse(row["work_location_id"].ToString().Trim());

                            if (employee.WorkLocationID == Constants.syncIntNullValue)
                                employee.WorkLocationID = -1;
                        }
                        if (row["work_location_code"] != DBNull.Value)
                        {
                            employee.WorkLocationCode = row["work_location_code"].ToString().Trim();

                            if (employee.WorkLocationCode == Constants.syncStringNullValue)
                                employee.WorkLocationCode = "";
                        }
                        if (row["position_id"] != DBNull.Value)
                        {
                            employee.PositionID = Int32.Parse(row["position_id"].ToString().Trim());

                            if (employee.PositionID == Constants.syncIntNullValue)
                                employee.PositionID = -1;
                        }
                        if (row["address"] != DBNull.Value)
                        {
                            employee.Address = row["address"].ToString().Trim();                            
                        }
                        if (row["phone_number_1"] != DBNull.Value)
                        {
                            employee.PhoneNumber1 = row["phone_number_1"].ToString().Trim();                            
                        }
                        if (row["phone_number_2"] != DBNull.Value)
                        {
                            employee.PhoneNumber2 = row["phone_number_2"].ToString().Trim();                            
                        }
                        if (row["date_of_birth"] != DBNull.Value)
                        {
                            employee.DateOfBirth = DateTime.Parse(row["date_of_birth"].ToString().Trim());

                            if (employee.DateOfBirth.Equals(Constants.syncDateTimeNullValue()))
                                employee.DateOfBirth = new DateTime();
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            employee.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["language"] != DBNull.Value)
                        {
                            employee.Language = row["language"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            employee.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }

                        employeesList.Add(employee);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return employeesList;
        }

        private string getValidString(string syncString)
        {
            string str = syncString;
            try
            {
                if (str.Length > 50)
                    str = str.Substring(0, 50);
                str = str.Replace("'", "");

            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }
            return str;
        }

        public override List<SyncResponsibilityTO> getDifResponsibility()
        {
            DataSet dataSet = new DataSet();
            SyncResponsibilityTO fs = new SyncResponsibilityTO();
            List<SyncResponsibilityTO> fsList = new List<SyncResponsibilityTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from " + DataBase + "." + User + ".sync_responsibility");

                select = sb.ToString();
                select += " ORDER BY rec_id ";

                SqlCommand cmd = new SqlCommand(select, connection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "SyncResponse");
                DataTable table = dataSet.Tables["SyncResponse"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {

                        fs = new SyncResponsibilityTO();
                        fs.RecID = Int32.Parse(row["rec_id"].ToString().Trim());
                        if (row["unit_id"] != DBNull.Value)
                        {
                            fs.UnitID = Int32.Parse(row["unit_id"].ToString().Trim());
                        }
                        if (row["responsible_person_id"] != DBNull.Value)
                        {
                            fs.ResponsiblePersonID = int.Parse(row["responsible_person_id"].ToString().Trim());
                        }
                       
                        if (row["valid_from"] != DBNull.Value)
                        {
                            fs.ValidFrom = DateTime.Parse(row["valid_from"].ToString().Trim());
                            if (fs.ValidFrom.Equals(Constants.syncDateTimeNullValue()))
                                fs.ValidFrom = new DateTime();
                        }
                        if (row["valid_to"] != DBNull.Value)
                        {
                            fs.ValidTo = DateTime.Parse(row["valid_to"].ToString().Trim());
                            if (fs.ValidTo.Equals(Constants.syncDateTimeNullValue()))
                                fs.ValidTo = new DateTime();
                        }
                        if (row["structure_type"] != DBNull.Value)
                        {
                            fs.StructureType = row["structure_type"].ToString().Trim();
                        }                       
                        if (row["created_by"] != DBNull.Value)
                        {
                            fs.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            fs.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }

                        fsList.Add(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return fsList;
        }

        public override List<SyncFinancialStructureTO> getDifFinancialStructures()
        {
            DataSet dataSet = new DataSet();
            SyncFinancialStructureTO fs = new SyncFinancialStructureTO();
            List<SyncFinancialStructureTO> fsList = new List<SyncFinancialStructureTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from " + DataBase + "." + User + ".sync_financial_structure");

                select = sb.ToString();
                select += " ORDER BY rec_id ";

                SqlCommand cmd = new SqlCommand(select, connection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {

                        fs = new SyncFinancialStructureTO();
                        fs.RecID = Int32.Parse(row["rec_id"].ToString().Trim());
                        fs.UnitID = Int32.Parse(row["unit_id"].ToString().Trim());
                        if (row["unit_stringone"] != DBNull.Value)
                        {
                            fs.UnitStringone = row["unit_stringone"].ToString().Trim();
                        }
                        if (row["valid_from"] != DBNull.Value)
                        {
                            fs.ValidFrom = DateTime.Parse(row["valid_from"].ToString().Trim());
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            fs.Description = row["description"].ToString().Trim();
                        }
                        if (row["company_code"] != DBNull.Value)
                        {
                            fs.CompanyCode = row["company_code"].ToString().Trim();
                        }
                        if (row["status"] != DBNull.Value)
                        {
                            fs.Status = row["status"].ToString().Trim();
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            fs.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            fs.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }

                        fsList.Add(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return fsList;
        }

        public override List<SyncOrganizationalStructureTO> getDifOrganizationalStructuresParental()
        {
            DataSet dataSet = new DataSet();
            SyncOrganizationalStructureTO fs = new SyncOrganizationalStructureTO();
            List<SyncOrganizationalStructureTO> fsList = new List<SyncOrganizationalStructureTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from " + DataBase + "." + User + ".sync_organizational_structure ");
                sb.Append("WHERE unit_id IN (");
                sb.Append("select DISTINCT parent_unit_id from " + DataBase + "." + User + ".sync_organizational_structure ) ");
                select = sb.ToString();
                select += " ORDER BY rec_id ";

                SqlCommand cmd = new SqlCommand(select, connection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {

                        fs = new SyncOrganizationalStructureTO();
                        fs.RecID = Int32.Parse(row["rec_id"].ToString().Trim());
                        fs.UnitID = Int32.Parse(row["unit_id"].ToString().Trim());
                        if (row["parent_unit_id"] != DBNull.Value)
                        {
                            fs.ParentUnitID = int.Parse(row["parent_unit_id"].ToString().Trim());
                        }
                        //if (row["parent_unit_id_valid_from"] != DBNull.Value)
                        //{
                        //    fs.ParentUnitIDValidFrom = DateTime.Parse(row["parent_unit_id_valid_from"].ToString().Trim());
                        //}
                        if (row["valid_from"] != DBNull.Value)
                        {
                            fs.ValidFrom = DateTime.Parse(row["valid_from"].ToString().Trim());
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            fs.Description = row["description"].ToString().Trim();
                        }
                        //if (row["description_valid_from"] != DBNull.Value)
                        //{
                        //    fs.DescriptionValidFrom = DateTime.Parse(row["description_valid_from"].ToString().Trim());
                        //}
                        if (row["code"] != DBNull.Value)
                        {
                            fs.Code = row["code"].ToString().Trim();
                        }
                        if (row["cost_center_company_code"] != DBNull.Value)
                        {
                            fs.CompanyCode = row["cost_center_company_code"].ToString().Trim();
                        }
                        //if (row["code_valid_from"] != DBNull.Value)
                        //{
                        //    fs.CodeValidFrom = DateTime.Parse(row["code_valid_from"].ToString().Trim());
                        //}
                        if (row["cost_center_code"] != DBNull.Value)
                        {
                            fs.CostCenterStringone = row["cost_center_code"].ToString().Trim();
                        }
                        //if (row["cost_center_stringone_valid_from"] != DBNull.Value)
                        //{
                        //    fs.CostCenterStringoneValidFrom = DateTime.Parse(row["cost_center_stringone_valid_from"].ToString().Trim());
                        //}
                        if (row["created_by"] != DBNull.Value)
                        {
                            fs.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            fs.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }

                        fsList.Add(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return fsList;
        }

        public override List<SyncOrganizationalStructureTO> getDifOrganizationalStructuresNOTParental()
        {
            DataSet dataSet = new DataSet();
            SyncOrganizationalStructureTO fs = new SyncOrganizationalStructureTO();
            List<SyncOrganizationalStructureTO> fsList = new List<SyncOrganizationalStructureTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from " + DataBase + "." + User + ".sync_organizational_structure ");
                sb.Append("WHERE unit_id NOT IN (");
                sb.Append("select DISTINCT parent_unit_id from " + DataBase + "." + User + ".sync_organizational_structure ) ");
                select = sb.ToString();
                select += " ORDER BY rec_id ";

                SqlCommand cmd = new SqlCommand(select, connection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {

                        fs = new SyncOrganizationalStructureTO();
                        fs.RecID = Int32.Parse(row["rec_id"].ToString().Trim());
                        fs.UnitID = Int32.Parse(row["unit_id"].ToString().Trim());
                        if (row["parent_unit_id"] != DBNull.Value)
                        {
                            fs.ParentUnitID = int.Parse(row["parent_unit_id"].ToString().Trim());
                        }
                        //if (row["parent_unit_id_valid_from"] != DBNull.Value)
                        //{
                        //    fs.ParentUnitIDValidFrom = DateTime.Parse(row["parent_unit_id_valid_from"].ToString().Trim());
                        //}
                        if (row["valid_from"] != DBNull.Value)
                        {
                            fs.ValidFrom = DateTime.Parse(row["valid_from"].ToString().Trim());
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            fs.Description = row["description"].ToString().Trim();
                        }
                        //if (row["description_valid_from"] != DBNull.Value)
                        //{
                        //    fs.DescriptionValidFrom = DateTime.Parse(row["description_valid_from"].ToString().Trim());
                        //}
                        if (row["code"] != DBNull.Value)
                        {
                            fs.Code = row["code"].ToString().Trim();
                        }
                        if (row["cost_center_company_code"] != DBNull.Value)
                        {
                            fs.CompanyCode = row["cost_center_company_code"].ToString().Trim();
                        }
                        //if (row["code_valid_from"] != DBNull.Value)
                        //{
                        //    fs.CodeValidFrom = DateTime.Parse(row["code_valid_from"].ToString().Trim());
                        //}
                        if (row["cost_center_code"] != DBNull.Value)
                        {
                            fs.CostCenterStringone = row["cost_center_code"].ToString().Trim();
                        }
                        //if (row["cost_center_stringone_valid_from"] != DBNull.Value)
                        //{
                        //    fs.CostCenterStringoneValidFrom = DateTime.Parse(row["cost_center_stringone_valid_from"].ToString().Trim());
                        //}
                        if (row["created_by"] != DBNull.Value)
                        {
                            fs.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            fs.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }

                        fsList.Add(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return fsList;
        }

        public override List<SyncOrganizationalStructureTO> getDifOrganizationalStructures()
        {
            DataSet dataSet = new DataSet();
            SyncOrganizationalStructureTO fs = new SyncOrganizationalStructureTO();
            List<SyncOrganizationalStructureTO> fsList = new List<SyncOrganizationalStructureTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from " + DataBase + "." + User + ".sync_organizational_structure");

                select = sb.ToString();
                select += " ORDER BY rec_id ";

                SqlCommand cmd = new SqlCommand(select, connection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {

                        fs = new SyncOrganizationalStructureTO();
                        fs.RecID = Int32.Parse(row["rec_id"].ToString().Trim());
                        fs.UnitID = Int32.Parse(row["unit_id"].ToString().Trim());
                        if (row["parent_unit_id"] != DBNull.Value)
                        {
                            fs.ParentUnitID = int.Parse(row["parent_unit_id"].ToString().Trim());
                        }
                        //if (row["parent_unit_id_valid_from"] != DBNull.Value)
                        //{
                        //    fs.ParentUnitIDValidFrom = DateTime.Parse(row["parent_unit_id_valid_from"].ToString().Trim());
                        //}
                        if (row["valid_from"] != DBNull.Value)
                        {
                            fs.ValidFrom = DateTime.Parse(row["valid_from"].ToString().Trim());
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            fs.Description = row["description"].ToString().Trim();
                        }
                        //if (row["description_valid_from"] != DBNull.Value)
                        //{
                        //    fs.DescriptionValidFrom = DateTime.Parse(row["description_valid_from"].ToString().Trim());
                        //}
                        if (row["code"] != DBNull.Value)
                        {
                            fs.Code = row["code"].ToString().Trim();
                        }
                        if (row["cost_center_company_code"] != DBNull.Value)
                        {
                            fs.CompanyCode = row["cost_center_company_code"].ToString().Trim();
                        }
                        //if (row["code_valid_from"] != DBNull.Value)
                        //{
                        //    fs.CodeValidFrom = DateTime.Parse(row["code_valid_from"].ToString().Trim());
                        //}
                        if (row["cost_center_code"] != DBNull.Value)
                        {
                            fs.CostCenterStringone = row["cost_center_code"].ToString().Trim();
                        }
                        //if (row["cost_center_stringone_valid_from"] != DBNull.Value)
                        //{
                        //    fs.CostCenterStringoneValidFrom = DateTime.Parse(row["cost_center_stringone_valid_from"].ToString().Trim());
                        //}

                        if (row["status"] != DBNull.Value)
                        {
                            fs.Status = row["status"].ToString().Trim();
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            fs.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            fs.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }

                        fsList.Add(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return fsList;
        }

        public override List<SyncCostCenterTO> getDifCostCenters()
        {
            DataSet dataSet = new DataSet();
            SyncCostCenterTO fs = new SyncCostCenterTO();
            List<SyncCostCenterTO> fsList = new List<SyncCostCenterTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from " + DataBase + "." + User + ".sync_cost_centers");

                select = sb.ToString();
                select += " ORDER BY rec_id ";

                SqlCommand cmd = new SqlCommand(select, connection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "CostCenters");
                DataTable table = dataSet.Tables["CostCenters"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        fs = new SyncCostCenterTO();
                        fs.RecID = Int32.Parse(row["rec_id"].ToString().Trim());                        
                        if (row["cost_center_code"] != DBNull.Value)
                        {
                            fs.Code = row["cost_center_code"].ToString().Trim();
                        }
                        if (row["cost_center_company_code"] != DBNull.Value)
                        {
                            fs.CompanyCode = row["cost_center_company_code"].ToString().Trim();
                        }
                        if (row["cost_center_description"] != DBNull.Value)
                        {
                            fs.Desc = row["cost_center_description"].ToString().Trim();
                        }
                        if (row["valid_from"] != DBNull.Value)
                        {
                            fs.ValidFrom = DateTime.Parse(row["valid_from"].ToString().Trim());
                        }                        
                        
                        if (row["created_by"] != DBNull.Value)
                        {
                            fs.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            fs.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }

                        fsList.Add(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return fsList;
        }

        public override List<SyncEmployeePositionTO> getDifEmployeePositions()
        {
            DataSet dataSet = new DataSet();
            SyncEmployeePositionTO pos = new SyncEmployeePositionTO();
            List<SyncEmployeePositionTO> posList = new List<SyncEmployeePositionTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from " + DataBase + "." + User + ".sync_employee_positions");

                select = sb.ToString();
                select += " ORDER BY rec_id ";

                SqlCommand cmd = new SqlCommand(select, connection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Positions");
                DataTable table = dataSet.Tables["Positions"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pos = new SyncEmployeePositionTO();
                        pos.RecID = Int32.Parse(row["rec_id"].ToString().Trim());
                        if (row["company_code"] != DBNull.Value)
                        {
                            pos.CompanyCode = row["company_code"].ToString().Trim();
                        }
                        if (row["position_id"] != DBNull.Value)
                        {
                            pos.PositionID = int.Parse(row["position_id"].ToString().Trim());

                            if (pos.PositionID == Constants.syncIntNullValue)
                                pos.PositionID = -1;
                        }
                        if (row["position_code"] != DBNull.Value)
                        {
                            pos.PositionCode = row["position_code"].ToString().Trim();
                        }
                        if (row["status"] != DBNull.Value)
                        {
                            pos.Status = row["status"].ToString().Trim();
                        }
                        if (row["position_title_sr"] != DBNull.Value)
                        {
                            pos.PositionTitleSR = row["position_title_sr"].ToString().Trim().Replace("'", "");
                        }
                        if (row["position_title_en"] != DBNull.Value)
                        {
                            pos.PositionTitleEN = row["position_title_en"].ToString().Trim().Replace("'", "");
                        }
                        if (row["desc_sr"] != DBNull.Value)
                        {
                            pos.DescSR = row["desc_sr"].ToString().Trim().Replace("'", "");
                        }
                        if (row["desc_en"] != DBNull.Value)
                        {
                            pos.DescEN = row["desc_en"].ToString().Trim().Replace("'", "");
                        }
                        if (row["status"] != DBNull.Value)
                        {
                            pos.Status = row["status"].ToString().Trim();
                        }
                        if (row["valid_from"] != DBNull.Value)
                        {
                            pos.ValidFrom = DateTime.Parse(row["valid_from"].ToString().Trim());
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            pos.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pos.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }

                        posList.Add(pos);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return posList;
        }

        public override List<SyncAnnualLeaveRecalcTO> getDifAnnualLeaveRecalc(int emplID)
        {
            DataSet dataSet = new DataSet();
            SyncAnnualLeaveRecalcTO recalc = new SyncAnnualLeaveRecalcTO();
            List<SyncAnnualLeaveRecalcTO> recalcList = new List<SyncAnnualLeaveRecalcTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from " + DataBase + "." + User + ".sync_annual_leave_recalc");

                if (emplID != -1)
                    sb.Append(" where employee_id = '" + emplID.ToString().Trim() + "'");

                select = sb.ToString();
                select += " ORDER BY rec_id ";

                SqlCommand cmd = new SqlCommand(select, connection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Recalcs");
                DataTable table = dataSet.Tables["Recalcs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        recalc = new SyncAnnualLeaveRecalcTO();
                        recalc.RecID = Int32.Parse(row["rec_id"].ToString().Trim());
                        if (row["employee_id"] != DBNull.Value)
                        {
                            recalc.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["year"] != DBNull.Value)
                        {
                            recalc.Year = DateTime.Parse(row["year"].ToString().Trim());
                        }
                        if (row["num_of_days"] != DBNull.Value)
                        {
                            recalc.NumOfDays = Int32.Parse(row["num_of_days"].ToString().Trim());
                        }                        
                        if (row["created_by"] != DBNull.Value)
                        {
                            recalc.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            recalc.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }

                        recalcList.Add(recalc);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return recalcList;
        }

        public override bool delSyncEmployee(SyncEmployeeTO syncEmployee)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = connection.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM sync_employees WHERE rec_id = " + syncEmployee.RecID);

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), connection, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    isDeleted = true;
                }
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("DELETE");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public override bool delSyncCostCenters(SyncCostCenterTO syncCC)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = connection.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM sync_cost_centers WHERE rec_id = " + syncCC.RecID);

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), connection, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    isDeleted = true;
                }
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("DELETE");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public override bool delSyncAnnualLeaveRecalc(SyncAnnualLeaveRecalcTO syncRecalc)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = connection.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM sync_annual_leave_recalc WHERE rec_id = " + syncRecalc.RecID);

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), connection, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    isDeleted = true;
                }
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("DELETE");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public override bool delSyncEmployeePosition(SyncEmployeePositionTO syncEmplPosition)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = connection.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM sync_employee_positions WHERE rec_id = " + syncEmplPosition.RecID);

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), connection, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    isDeleted = true;
                }
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("DELETE");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public override bool delSyncResponsibility(SyncResponsibilityTO syncResponse)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = connection.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM sync_responsibility WHERE rec_id = " + syncResponse.RecID);

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), connection, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    isDeleted = true;
                }
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("DELETE");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public override bool delSyncFinancialStructure(SyncFinancialStructureTO syncFinancialStructure)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = connection.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM sync_financial_structure WHERE rec_id = " + syncFinancialStructure.RecID);

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), connection, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    isDeleted = true;
                }
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("DELETE");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public override bool delSyncOrganizationalStructure(SyncOrganizationalStructureTO syncOrganizationalStructure)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = connection.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM sync_organizational_structure WHERE rec_id = " + syncOrganizationalStructure.RecID);

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), connection, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    isDeleted = true;
                }
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("DELETE");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public override int insertSyncBufferData(SyncBufferDataTO syncBuffer)
        {
            SqlTransaction sqlTrans = connection.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            int recID = -1;
            
            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("SET NOCOUNT ON ");
                sbInsert.Append("INSERT INTO sync_buffer_data ");
                sbInsert.Append("(employee_id, days_of_holiday_for_current_years_left, days_of_holiday_for_previous_years_left, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                if (syncBuffer.EmployeeID != -1)
                {
                    sbInsert.Append(syncBuffer.EmployeeID.ToString() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (syncBuffer.DaysOfHolidayForCurrentYearLeft != -1)
                {
                    sbInsert.Append(syncBuffer.DaysOfHolidayForCurrentYearLeft.ToString() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (syncBuffer.DaysOfHolidayForPreviousYearLeft != -1)
                {
                    sbInsert.Append(syncBuffer.DaysOfHolidayForPreviousYearLeft.ToString() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                              

                sbInsert.Append("N'" + Constants.syncUser + "', GETDATE()) ");
                sbInsert.Append("SELECT @@Identity AS rec_id ");
                sbInsert.Append("SET NOCOUNT OFF ");

                DataSet dataSet = new DataSet();
                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), connection, sqlTrans);
                cmd.CommandTimeout = Constants.commandTimeout;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "BufferData");

                DataTable table = dataSet.Tables["BufferData"];

                recID = Int32.Parse(table.Rows[0]["rec_id"].ToString());

                sqlTrans.Commit();

            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("INSERT");
                throw ex;
            }

            return recID;
        }
    }
}
