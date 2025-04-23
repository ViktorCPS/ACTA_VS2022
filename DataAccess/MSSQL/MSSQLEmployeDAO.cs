

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Globalization;

using TransferObjects;
using Util;

namespace DataAccess {
    /// <summary>
    /// EmployeeDAO implementation for managing Employee's data form MSSQL database. 
    /// </summary>
    public class MSSQLEmployeeDAO : EmployeeDAO {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLEmployeeDAO() {
            conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
            DAOController.GetInstance();
        }

        public MSSQLEmployeeDAO(SqlConnection sqlConnection) {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
            DAOController.GetInstance();
        }

        public void SetDBConnection(Object dbConnection) {
            conn = dbConnection as SqlConnection;
        }

        public int insert(string EmployeeID, string FirstName, string LastName, string WorkingUnitID,
            string Status, string Password, string AddressID, string Picture, string WorkingGroupID,
            string type, string accessGroupID, bool doCommit) {
            SqlTransaction sqlTrans = null;

            if (doCommit) {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            }
            else {
                sqlTrans = this.SqlTrans;
            }
            int rowsAffected = 0;

            try {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employees ");
                sbInsert.Append("(employee_id, first_name, last_name, working_unit_id, status, password, address_id, picture, employee_group_id, type, access_group_id, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                if (Int32.Parse(EmployeeID.Trim()) != -1) {
                    sbInsert.Append(EmployeeID + ", ");
                }
                else {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append("N'" + FirstName.Trim() + "', N'" + LastName.Trim() + "', ");

                if (Int32.Parse(WorkingUnitID.Trim()) != -1) {
                    sbInsert.Append(WorkingUnitID + ", ");
                }
                else {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append("N'" + Status.Trim() + "', N'" + Password.Trim() + "', ");

                if (Int32.Parse(AddressID.Trim()) != -1) {
                    sbInsert.Append(AddressID + ", ");
                }
                else {
                    sbInsert.Append("NULL, ");
                }

                if (!Picture.Trim().Equals("")) {
                    sbInsert.Append("N'" + Picture.Trim() + "', ");
                }
                else {
                    sbInsert.Append("NULL, ");
                }
                if (Int32.Parse(WorkingGroupID.Trim()) != -1) {
                    sbInsert.Append(WorkingGroupID + ", ");
                }
                else {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append("N'" + type.Trim() + "', ");

                if (Int32.Parse(accessGroupID.Trim()) != -1) {
                    sbInsert.Append(accessGroupID + ", ");
                }
                else {
                    //can not be null
                    sbInsert.Append("0" + ", ");
                }

                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit) {
                    sqlTrans.Commit();
                }
                rowsAffected = 1;
            }
            catch (Exception ex) {
                if (doCommit) {
                    sqlTrans.Rollback("INSERT");
                }
                throw ex;
            }

            return rowsAffected;
        }
        public int insert(string EmployeeID, string FirstName, string LastName, string WorkingUnitID, string Status, string Password, string AddressID, string Picture,
            string WorkingGroupID, string type, string accessGroupID, int orgUnitID, bool doCommit) {
            SqlTransaction sqlTrans = null;

            if (doCommit) {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            }
            else {
                sqlTrans = this.SqlTrans;
            }
            int rowsAffected = 0;

            try {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employees ");
                sbInsert.Append("(employee_id, first_name, last_name, working_unit_id, status, password, address_id, picture, employee_group_id, employee_type_id, access_group_id,organizational_unit_id, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                if (Int32.Parse(EmployeeID.Trim()) != -1) {
                    sbInsert.Append(EmployeeID + ", ");
                }
                else {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append("N'" + FirstName.Trim() + "', N'" + LastName.Trim() + "', ");

                if (Int32.Parse(WorkingUnitID.Trim()) != -1) {
                    sbInsert.Append(WorkingUnitID + ", ");
                }
                else {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append("N'" + Status.Trim() + "', N'" + Password.Trim() + "', ");

                if (Int32.Parse(AddressID.Trim()) != -1) {
                    sbInsert.Append(AddressID + ", ");
                }
                else {
                    sbInsert.Append("NULL, ");
                }

                if (!Picture.Trim().Equals("")) {
                    sbInsert.Append("N'" + Picture.Trim() + "', ");
                }
                else {
                    sbInsert.Append("NULL, ");
                }
                if (Int32.Parse(WorkingGroupID.Trim()) != -1) {
                    sbInsert.Append(WorkingGroupID + ", ");
                }
                else {
                    sbInsert.Append("NULL, ");
                }


                sbInsert.Append("'" + type.Trim() + "', ");

                if (Int32.Parse(accessGroupID.Trim()) != -1) {
                    sbInsert.Append(accessGroupID + ", ");
                }
                else {
                    //can not be null
                    sbInsert.Append("0" + ", ");
                }
                if (orgUnitID != -1) {
                    sbInsert.Append(orgUnitID + ", ");
                }
                else {
                    sbInsert.Append("NULL, ");
                }
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit) {
                    sqlTrans.Commit();
                }
                rowsAffected = 1;
            }
            catch (Exception ex) {
                if (doCommit) {
                    sqlTrans.Rollback("INSERT");
                }
                throw ex;
            }

            return rowsAffected;
        }
        public int insert(string EmployeeID, string FirstName, string LastName, string WorkingUnitID,
         string Status, string Password, string AddressID, string Picture, string WorkingGroupID,
         string type, string accessGroupID, string user, bool doCommit) {
            SqlTransaction sqlTrans = null;

            if (doCommit) {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            }
            else {
                sqlTrans = this.SqlTrans;
            }
            int rowsAffected = 0;

            try {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employees ");
                sbInsert.Append("(employee_id, first_name, last_name, working_unit_id, status, password, address_id, picture, employee_group_id, type, access_group_id, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                if (Int32.Parse(EmployeeID.Trim()) != -1) {
                    sbInsert.Append(EmployeeID + ", ");
                }
                else {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append("N'" + FirstName.Trim() + "', N'" + LastName.Trim() + "', ");

                if (Int32.Parse(WorkingUnitID.Trim()) != -1) {
                    sbInsert.Append(WorkingUnitID + ", ");
                }
                else {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append("N'" + Status.Trim() + "', N'" + Password.Trim() + "', ");

                if (Int32.Parse(AddressID.Trim()) != -1) {
                    sbInsert.Append(AddressID + ", ");
                }
                else {
                    sbInsert.Append("NULL, ");
                }

                if (!Picture.Trim().Equals("")) {
                    sbInsert.Append("N'" + Picture.Trim() + "', ");
                }
                else {
                    sbInsert.Append("NULL, ");
                }
                if (Int32.Parse(WorkingGroupID.Trim()) != -1) {
                    sbInsert.Append(WorkingGroupID + ", ");
                }
                else {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append("N'" + type.Trim() + "', ");

                if (Int32.Parse(accessGroupID.Trim()) != -1) {
                    sbInsert.Append(accessGroupID + ", ");
                }
                else {
                    //can not be null
                    sbInsert.Append("0" + ", ");
                }

                sbInsert.Append("N'" + user + "', GETDATE()) ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit) {
                    sqlTrans.Commit();
                }
                rowsAffected = 1;
            }
            catch (Exception ex) {
                if (doCommit) {
                    sqlTrans.Rollback("INSERT");
                }
                throw ex;
            }

            return rowsAffected;
        }

        public bool updatePicture(int employeeID, string picture, bool doCommit) {
            SqlTransaction sqlTrans = null;
            bool isUpdated = false;

            if (doCommit) {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            }
            else {
                sqlTrans = this.SqlTrans;
            }

            try {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE employees SET ");
                sbUpdate.Append("picture = N'" + picture.Trim() + "' ");
                sbUpdate.Append("WHERE employee_id = " + employeeID);

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0) {
                    isUpdated = true;
                }

                if (doCommit) {
                    sqlTrans.Commit();
                }
            }
            catch (Exception ex) {
                if (doCommit)
                    sqlTrans.Rollback("UPDATE");
                else
                    sqlTrans.Rollback();

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool updateSiemens(int employeeID, string firstName, string lastName, int wuID, int addressID) {
            SqlTransaction sqlTrans = null;
            bool isUpdated = false;

            sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

            try {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE employees SET ");
                sbUpdate.Append("first_name = N'" + firstName + "', ");
                sbUpdate.Append("last_name = N'" + lastName + "', ");
                sbUpdate.Append("working_unit_id = " + wuID + " ");
                if (addressID != -1)
                    sbUpdate.Append(", address_id = " + addressID + " ");
                sbUpdate.Append("WHERE employee_id = " + employeeID);

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0) {
                    isUpdated = true;
                }

                sqlTrans.Commit();

            }
            catch (Exception ex) {

                sqlTrans.Rollback("UPDATE");

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }


        public int insert(EmployeeTO employeeTO) {
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            int rowsAffected = 0;

            try {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employees ");
                sbInsert.Append("(employee_id, first_name, last_name, working_unit_id, status, password, address_id, picture, employee_group_id, type, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                if (employeeTO.EmployeeID != -1) {
                    sbInsert.Append(employeeTO.EmployeeID.ToString() + ", ");
                }
                else {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append("N'" + employeeTO.FirstName.Trim() + "', N'" + employeeTO.LastName.Trim() + "', ");

                if (employeeTO.WorkingUnitID != -1) {
                    sbInsert.Append(employeeTO.WorkingUnitID.ToString() + ", ");
                }
                else {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append("N'" + employeeTO.Status.Trim() + "', N'" + employeeTO.Password.Trim() + "', ");

                if (employeeTO.AddressID != -1) {
                    sbInsert.Append(employeeTO.AddressID.ToString() + ", ");
                }
                else {
                    sbInsert.Append("NULL, ");
                }

                if (!employeeTO.Picture.Equals("")) {
                    sbInsert.Append("N'" + employeeTO.Picture.Trim() + "' , ");
                }
                else {
                    sbInsert.Append("NULL, ");
                }

                if (employeeTO.WorkingGroupID != -1) {
                    sbInsert.Append(employeeTO.WorkingGroupID.ToString() + ", ");
                }
                else {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append("N'" + employeeTO.Type.Trim() + "' , ");

                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();
                sqlTrans.Commit();
            }
            catch (Exception ex) {
                sqlTrans.Rollback("INSERT");
                throw ex;
            }

            return rowsAffected;
        }

        public int insert(EmployeeTO empl, bool doCommit) {
            int rowsAffected = 0;
            SqlTransaction sqltrans = null;
            if (doCommit)
                sqltrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            else
                sqltrans = this.SqlTrans;

            try {

                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employees ");
                sbInsert.Append("(employee_id, first_name, last_name, working_unit_id, status, password, address_id, picture, employee_group_id, type, access_group_id, organizational_unit_id, employee_type_id, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                sbInsert.Append(empl.EmployeeID.ToString().Trim() + ", ");
                sbInsert.Append("N'" + empl.FirstName.Trim() + "', N'" + empl.LastName.Trim() + "', ");
                sbInsert.Append(empl.WorkingUnitID.ToString().Trim() + ", ");
                sbInsert.Append("N'" + empl.Status.Trim() + "', ");
                if (!empl.Password.Trim().Equals(""))
                    sbInsert.Append("N'" + empl.Password.Trim() + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (empl.AddressID != -1)
                    sbInsert.Append(empl.AddressID.ToString().Trim() + ", ");
                else
                    sbInsert.Append("NULL, ");
                if (!empl.Picture.Trim().Equals(""))
                    sbInsert.Append("N'" + empl.Picture.Trim() + "', ");
                else
                    sbInsert.Append("NULL, ");

                if (empl.WorkingGroupID != -1)
                    sbInsert.Append(empl.WorkingGroupID.ToString().Trim() + ", ");
                else
                    sbInsert.Append("NULL, ");
                sbInsert.Append("N'" + empl.Type.Trim() + "', ");
                sbInsert.Append(empl.AccessGroupID.ToString().Trim() + ", ");
                sbInsert.Append(empl.OrgUnitID.ToString().Trim() + ", ");
                sbInsert.Append(empl.EmployeeTypeID.ToString().Trim() + ", ");

                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqltrans);
                rowsAffected = cmd.ExecuteNonQuery();
                if (doCommit) {
                    sqltrans.Commit();
                }
                rowsAffected = 1;
            }
            catch (Exception ex) {
                if (doCommit) {
                    sqltrans.Rollback();
                }
                throw ex;
            }

            return rowsAffected;
        }

        public bool delete(string empolyeeID) {
            bool isDeleted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM employees WHERE employee_id = " + Int32.Parse(empolyeeID.Trim()));

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0) {
                    isDeleted = true;
                }
                sqlTrans.Commit();
            }
            catch (Exception ex) {
                sqlTrans.Rollback("DELETE");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        // Same as method find(string EmployeID, IDBTransaction trans) - command is made without transaction
        public EmployeeTO find(string EmployeID) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            try {
                SqlCommand cmd = new SqlCommand("SELECT * FROM employees WHERE employee_id = '" + EmployeID.Trim() + "'", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count == 1) {
                    employee = new EmployeeTO();
                    employee.EmployeeID = Int32.Parse(table.Rows[0]["employee_id"].ToString().Trim());
                    if (table.Rows[0]["first_name"] != DBNull.Value) {
                        employee.FirstName = table.Rows[0]["first_name"].ToString().Trim();
                    }
                    if (table.Rows[0]["last_name"] != DBNull.Value) {
                        employee.LastName = table.Rows[0]["last_name"].ToString().Trim();
                    }
                    if (table.Rows[0]["working_unit_id"] != DBNull.Value) {
                        employee.WorkingUnitID = Int32.Parse(table.Rows[0]["working_unit_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["status"] != DBNull.Value) {
                        employee.Status = table.Rows[0]["status"].ToString().Trim();
                    }
                    if (table.Rows[0]["password"] != DBNull.Value) {
                        employee.Password = table.Rows[0]["password"].ToString().Trim();
                    }
                    if (table.Rows[0]["address_id"] != DBNull.Value) {
                        employee.AddressID = Int32.Parse(table.Rows[0]["address_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["picture"] != DBNull.Value) {
                        employee.Picture = table.Rows[0]["picture"].ToString().Trim();
                    }
                    if (table.Rows[0]["employee_group_id"] != DBNull.Value) {
                        employee.WorkingGroupID = Int32.Parse(table.Rows[0]["employee_group_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["type"] != DBNull.Value) {
                        employee.Type = table.Rows[0]["type"].ToString().Trim();
                    }
                    if (table.Rows[0]["access_group_id"] != DBNull.Value) {
                        employee.AccessGroupID = Int32.Parse(table.Rows[0]["access_group_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["organizational_unit_id"] != DBNull.Value) {
                        employee.OrgUnitID = Int32.Parse(table.Rows[0]["organizational_unit_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["employee_type_id"] != DBNull.Value) {
                        employee.EmployeeTypeID = Int32.Parse(table.Rows[0]["employee_type_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["created_by"] != DBNull.Value) {
                        employee.CreatedBy = table.Rows[0]["created_by"].ToString().Trim();
                    }
                    if (table.Rows[0]["created_time"] != DBNull.Value) {
                        employee.CreatedTime = DateTime.Parse(table.Rows[0]["created_time"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Exception: " + ex.Message);
            }

            return employee;
        }

        //// Same as method find(string EmployeID, IDBTransaction trans) - command is made without transaction
        //public EmployeeTO findWithASCO(string EmployeID)
        //{
        //    DataSet dataSet = new DataSet();
        //    EmployeeTO employee = new EmployeeTO();
        //    EmployeeAsco4TO asco4TO = new EmployeeAsco4TO();
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("SELECT e.*,asco.* FROM employees e,employees_asco4 asco  WHERE e.employee_id = asco.employee_id AND employee_id = '" + EmployeID.Trim() + "'", conn);
        //        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

        //        sqlDataAdapter.Fill(dataSet, "Employee");
        //        DataTable table = dataSet.Tables["Employee"];

        //        if (table.Rows.Count == 1)
        //        {
        //            employee = new EmployeeTO();
        //            employee.EmployeeID = Int32.Parse(table.Rows[0]["employee_id"].ToString().Trim());
        //            if (table.Rows[0]["first_name"] != DBNull.Value)
        //            {
        //                employee.FirstName = table.Rows[0]["first_name"].ToString().Trim();
        //            }
        //            if (table.Rows[0]["last_name"] != DBNull.Value)
        //            {
        //                employee.LastName = table.Rows[0]["last_name"].ToString().Trim();
        //            }
        //            if (table.Rows[0]["working_unit_id"] != DBNull.Value)
        //            {
        //                employee.WorkingUnitID = Int32.Parse(table.Rows[0]["working_unit_id"].ToString().Trim());
        //            }
        //            if (table.Rows[0]["status"] != DBNull.Value)
        //            {
        //                employee.Status = table.Rows[0]["status"].ToString().Trim();
        //            }
        //            if (table.Rows[0]["password"] != DBNull.Value)
        //            {
        //                employee.Password = table.Rows[0]["password"].ToString().Trim();
        //            }
        //            if (table.Rows[0]["address_id"] != DBNull.Value)
        //            {
        //                employee.AddressID = Int32.Parse(table.Rows[0]["address_id"].ToString().Trim());
        //            }
        //            if (table.Rows[0]["picture"] != DBNull.Value)
        //            {
        //                employee.Picture = table.Rows[0]["picture"].ToString().Trim();
        //            }
        //            if (table.Rows[0]["employee_group_id"] != DBNull.Value)
        //            {
        //                employee.WorkingGroupID = Int32.Parse(table.Rows[0]["employee_group_id"].ToString().Trim());
        //            }
        //            if (table.Rows[0]["type"] != DBNull.Value)
        //            {
        //                employee.Type = table.Rows[0]["type"].ToString().Trim();
        //            }
        //            if (table.Rows[0]["access_group_id"] != DBNull.Value)
        //            {
        //                employee.AccessGroupID = Int32.Parse(table.Rows[0]["access_group_id"].ToString().Trim());
        //            }
        //            if (table.Rows[0]["organizational_unit_id"] != DBNull.Value)
        //            {
        //                employee.OrgUnitID = Int32.Parse(table.Rows[0]["organizational_unit_id"].ToString().Trim());
        //            }
        //            if (table.Rows[0]["employee_type_id"] != DBNull.Value)
        //            {
        //                employee.EmployeeTypeID = Int32.Parse(table.Rows[0]["employee_type_id"].ToString().Trim());
        //            }
        //            //set asco4 values for employee
        //            if (table.Rows[0]["integer_value_1"] != DBNull.Value)
        //            {
        //                asco4TO.IntegerValue1 = Int32.Parse(table.Rows[0]["integer_value_1"].ToString().Trim());
        //            }
        //            if (table.Rows[0]["integer_value_2"] != DBNull.Value)
        //            {
        //                asco4TO.IntegerValue2 = Int32.Parse(table.Rows[0]["integer_value_2"].ToString().Trim());
        //            }
        //            if (table.Rows[0]["integer_value_3"] != DBNull.Value)
        //            {
        //                asco4TO.IntegerValue3 = Int32.Parse(table.Rows[0]["integer_value_3"].ToString().Trim());
        //            }
        //            if (table.Rows[0]["integer_value_4"] != DBNull.Value)
        //            {
        //                asco4TO.IntegerValue4 = Int32.Parse(table.Rows[0]["integer_value_4"].ToString().Trim());
        //            }
        //            if (table.Rows[0]["integer_value_5"] != DBNull.Value)
        //            {
        //                asco4TO.IntegerValue5 = Int32.Parse(table.Rows[0]["integer_value_5"].ToString().Trim());
        //            }
        //            if (table.Rows[0]["datetime_value_1"] != DBNull.Value)
        //            {
        //                asco4TO.DatetimeValue1 = DateTime.Parse(table.Rows[0]["datetime_value_1"].ToString().Trim());
        //            }
        //            if (table.Rows[0]["datetime_value_2"] != DBNull.Value)
        //            {
        //                asco4TO.DatetimeValue2 = DateTime.Parse(table.Rows[0]["datetime_value_2"].ToString().Trim());
        //            }
        //            if (table.Rows[0]["datetime_value_3"] != DBNull.Value)
        //            {
        //                asco4TO.DatetimeValue3 = DateTime.Parse(table.Rows[0]["datetime_value_3"].ToString().Trim());
        //            }
        //            if (table.Rows[0]["datetime_value_4"] != DBNull.Value)
        //            {
        //                asco4TO.DatetimeValue4 = DateTime.Parse(table.Rows[0]["datetime_value_4"].ToString().Trim());
        //            }
        //            if (table.Rows[0]["datetime_value_5"] != DBNull.Value)
        //            {
        //                asco4TO.DatetimeValue5 = DateTime.Parse(table.Rows[0]["datetime_value_5"].ToString().Trim());
        //            }
        //            if (table.Rows[0]["nvarchar_value_1"] != DBNull.Value)
        //            {
        //                asco4TO.NVarcharValue1 = table.Rows[0]["nvarchar_value_1"].ToString().Trim();
        //            }
        //            if (table.Rows[0]["nvarchar_value_2"] != DBNull.Value)
        //            {
        //                asco4TO.NVarcharValue2 = table.Rows[0]["nvarchar_value_2"].ToString().Trim();
        //            }
        //            if (table.Rows[0]["nvarchar_value_3"] != DBNull.Value)
        //            {
        //                asco4TO.NVarcharValue3 = table.Rows[0]["nvarchar_value_3"].ToString().Trim();
        //            }
        //            if (table.Rows[0]["nvarchar_value_4"] != DBNull.Value)
        //            {
        //                asco4TO.NVarcharValue4 = table.Rows[0]["nvarchar_value_4"].ToString().Trim();
        //            }
        //            if (table.Rows[0]["nvarchar_value_5"] != DBNull.Value)
        //            {
        //                asco4TO.NVarcharValue5 = table.Rows[0]["nvarchar_value_5"].ToString().Trim();
        //            }

        //            employee.Tag = asco4TO;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Exception: " + ex.Message);
        //    }

        //    return employee;
        //}

        // Same as method find(string EmployeID) - command is made with transaction
        public EmployeeTO find(string EmployeID, IDbTransaction trans) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            try {
                SqlCommand cmd = new SqlCommand("SELECT * FROM employees WHERE employee_id = '" + EmployeID.Trim() + "'", conn, (SqlTransaction)trans);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count == 1) {
                    employee = new EmployeeTO();
                    employee.EmployeeID = Int32.Parse(table.Rows[0]["employee_id"].ToString().Trim());
                    if (table.Rows[0]["first_name"] != DBNull.Value) {
                        employee.FirstName = table.Rows[0]["first_name"].ToString().Trim();
                    }
                    if (table.Rows[0]["last_name"] != DBNull.Value) {
                        employee.LastName = table.Rows[0]["last_name"].ToString().Trim();
                    }
                    if (table.Rows[0]["working_unit_id"] != DBNull.Value) {
                        employee.WorkingUnitID = Int32.Parse(table.Rows[0]["working_unit_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["status"] != DBNull.Value) {
                        employee.Status = table.Rows[0]["status"].ToString().Trim();
                    }
                    if (table.Rows[0]["password"] != DBNull.Value) {
                        employee.Password = table.Rows[0]["password"].ToString().Trim();
                    }
                    if (table.Rows[0]["address_id"] != DBNull.Value) {
                        employee.AddressID = Int32.Parse(table.Rows[0]["address_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["picture"] != DBNull.Value) {
                        employee.Picture = table.Rows[0]["picture"].ToString().Trim();
                    }
                    if (table.Rows[0]["employee_group_id"] != DBNull.Value) {
                        employee.WorkingGroupID = Int32.Parse(table.Rows[0]["employee_group_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["type"] != DBNull.Value) {
                        employee.Type = table.Rows[0]["type"].ToString().Trim();
                    }

                    if (table.Rows[0]["access_group_id"] != DBNull.Value) {
                        employee.AccessGroupID = Int32.Parse(table.Rows[0]["access_group_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["organizational_unit_id"] != DBNull.Value) {
                        employee.OrgUnitID = Int32.Parse(table.Rows[0]["organizational_unit_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["employee_type_id"] != DBNull.Value) {
                        employee.EmployeeTypeID = Int32.Parse(table.Rows[0]["employee_type_id"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Exception: " + ex.Message);
            }

            return employee;
        }

        public EmployeeTO findUserEmployee(string userID) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            try {
                SqlCommand cmd = new SqlCommand("SELECT e.* FROM employees e, employees_asco4 a WHERE e.employee_id = a.employee_id AND a.nvarchar_value_5 = '" + userID.Trim() + "'", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    employee = new EmployeeTO();
                    employee.EmployeeID = Int32.Parse(table.Rows[0]["employee_id"].ToString().Trim());
                    if (table.Rows[0]["first_name"] != DBNull.Value) {
                        employee.FirstName = table.Rows[0]["first_name"].ToString().Trim();
                    }
                    if (table.Rows[0]["last_name"] != DBNull.Value) {
                        employee.LastName = table.Rows[0]["last_name"].ToString().Trim();
                    }
                    if (table.Rows[0]["working_unit_id"] != DBNull.Value) {
                        employee.WorkingUnitID = Int32.Parse(table.Rows[0]["working_unit_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["status"] != DBNull.Value) {
                        employee.Status = table.Rows[0]["status"].ToString().Trim();
                    }
                    if (table.Rows[0]["password"] != DBNull.Value) {
                        employee.Password = table.Rows[0]["password"].ToString().Trim();
                    }
                    if (table.Rows[0]["address_id"] != DBNull.Value) {
                        employee.AddressID = Int32.Parse(table.Rows[0]["address_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["picture"] != DBNull.Value) {
                        employee.Picture = table.Rows[0]["picture"].ToString().Trim();
                    }
                    if (table.Rows[0]["employee_group_id"] != DBNull.Value) {
                        employee.WorkingGroupID = Int32.Parse(table.Rows[0]["employee_group_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["type"] != DBNull.Value) {
                        employee.Type = table.Rows[0]["type"].ToString().Trim();
                    }
                    if (table.Rows[0]["access_group_id"] != DBNull.Value) {
                        employee.AccessGroupID = Int32.Parse(table.Rows[0]["access_group_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["organizational_unit_id"] != DBNull.Value) {
                        employee.OrgUnitID = Int32.Parse(table.Rows[0]["organizational_unit_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["employee_type_id"] != DBNull.Value) {
                        employee.EmployeeTypeID = Int32.Parse(table.Rows[0]["employee_type_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["created_by"] != DBNull.Value) {
                        employee.CreatedBy = table.Rows[0]["created_by"].ToString().Trim();
                    }
                    if (table.Rows[0]["created_time"] != DBNull.Value) {
                        employee.CreatedTime = DateTime.Parse(table.Rows[0]["created_time"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Exception: " + ex.Message);
            }

            return employee;
        }

        public bool update(string EmployeeID, string FirstName, string LastName, string WorkingUnitID,
            string Status, string Password, string AddressID, string Picture, string WorkingGroupID,
            string type, string accessGroupID) {
            bool isUpdated = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

            try {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE employees SET ");
                sbUpdate.Append("first_name = N'" + FirstName.Trim() + "', ");
                sbUpdate.Append("last_name = N'" + LastName.Trim() + "', ");

                if (Int32.Parse(WorkingUnitID.Trim()) != -1) {
                    sbUpdate.Append("working_unit_id = " + WorkingUnitID.Trim() + ", ");
                }
                else {
                    sbUpdate.Append("working_unit_id = NULL, ");
                }

                sbUpdate.Append("status = N'" + Status.Trim() + "', ");
                sbUpdate.Append("password = N'" + Password.Trim() + "', ");

                if (Int32.Parse(AddressID.Trim()) != -1) {
                    sbUpdate.Append("address_id = " + AddressID.Trim() + ", ");
                }
                else {
                    sbUpdate.Append("address_id = NULL, ");
                }

                if (!Picture.Trim().Equals("")) {
                    sbUpdate.Append("picture = N'" + Picture.Trim() + "', ");
                }
                // don't change picture name if it already exists
                //				else
                //				{
                //					sbUpdate.Append("picture = NULL, ");
                //				}
                if (Int32.Parse(WorkingGroupID.Trim()) != -1) {
                    sbUpdate.Append("employee_group_id = " + WorkingGroupID.Trim() + ", ");
                }
                else {
                    sbUpdate.Append("employee_group_id = NULL, ");
                }
                sbUpdate.Append("type = N'" + type.Trim() + "', ");

                if (Int32.Parse(accessGroupID.Trim()) != -1) {
                    sbUpdate.Append("access_group_id = " + accessGroupID.Trim() + ", ");
                }
                else {
                    //can not be null
                    sbUpdate.Append("access_group_id = 0" + ", ");
                }

                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE employee_id = '" + EmployeeID.Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0) {
                    isUpdated = true;
                }

                sqlTrans.Commit();

            }
            catch (Exception ex) {
                sqlTrans.Rollback("UPDATE");

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }
        public bool update(string EmployeeID, string FirstName, string LastName, string WorkingUnitID, string Status, string Password, string AddressID, string Picture, string WorkingGroupID, string type, string accessGroupID, int orgUnitID, bool doCommit) {
            bool isUpdated = false;
            SqlTransaction sqlTrans = null;
            if (doCommit) {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");
            }
            else {
                sqlTrans = this.SqlTrans;
            }

            try {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE employees SET ");
                sbUpdate.Append("first_name = N'" + FirstName.Trim() + "', ");
                sbUpdate.Append("last_name = N'" + LastName.Trim() + "', ");

                if (Int32.Parse(WorkingUnitID.Trim()) != -1) {
                    sbUpdate.Append("working_unit_id = " + WorkingUnitID.Trim() + ", ");
                }
                else {
                    sbUpdate.Append("working_unit_id = NULL, ");
                }
                if (orgUnitID != -1) {
                    sbUpdate.Append("organizational_unit_id = " + orgUnitID.ToString().Trim() + ", ");
                }
                else {
                    sbUpdate.Append("organizational_unit_id = NULL, ");
                }

                sbUpdate.Append("status = N'" + Status.Trim() + "', ");
                sbUpdate.Append("password = N'" + Password.Trim() + "', ");

                if (Int32.Parse(AddressID.Trim()) != -1) {
                    sbUpdate.Append("address_id = " + AddressID.Trim() + ", ");
                }
                else {
                    sbUpdate.Append("address_id = NULL, ");
                }

                if (!Picture.Trim().Equals("")) {
                    sbUpdate.Append("picture = N'" + Picture.Trim() + "', ");
                }
                // don't change picture name if it already exists
                //				else
                //				{
                //					sbUpdate.Append("picture = NULL, ");
                //				}
                if (Int32.Parse(WorkingGroupID.Trim()) != -1) {
                    sbUpdate.Append("employee_group_id = " + WorkingGroupID.Trim() + ", ");
                }
                else {
                    sbUpdate.Append("employee_group_id = NULL, ");
                }
                sbUpdate.Append("type = N'" + type.Trim() + "', ");

                if (Int32.Parse(accessGroupID.Trim()) != -1) {
                    sbUpdate.Append("access_group_id = " + accessGroupID.Trim() + ", ");
                }
                else {
                    //can not be null
                    sbUpdate.Append("access_group_id = 0" + ", ");
                }

                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE employee_id = '" + EmployeeID.Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0) {
                    isUpdated = true;
                }

                if (doCommit) {
                    sqlTrans.Commit();
                }
            }
            catch (Exception ex) {
                if (doCommit) {
                    sqlTrans.Rollback("UPDATE");
                }

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        //NATALIJA08112017
        public bool update(EmployeeTO empl, int emplGroupID, IDbTransaction trans) {
            bool isUpdated = false;


            try {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE employees SET ");

                sbUpdate.Append("employee_group_id = " + emplGroupID.ToString().Trim() + ", ");

                if (!empl.ModifiedBy.Trim().Equals("")) {
                    sbUpdate.Append("modified_by = N'" + empl.ModifiedBy.Trim() + "', ");
                }
                else {
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                }
                //sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE employee_id = '" + empl.EmployeeID + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, (SqlTransaction)trans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0) {
                    isUpdated = true;
                }

            }
            catch (Exception sqlex) {
                trans.Rollback();
                throw new Exception(sqlex.Message);
            }

            return isUpdated;
        }

        public bool update(EmployeeTO empl, bool doCommit) {
            bool isUpdated = false;
            SqlTransaction sqlTrans = null;
            if (doCommit) {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");
            }
            else {
                sqlTrans = this.SqlTrans;
            }

            try {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE employees SET ");
                sbUpdate.Append("first_name = N'" + empl.FirstName.Trim() + "', ");
                sbUpdate.Append("last_name = N'" + empl.LastName.Trim() + "', ");

                if (empl.WorkingUnitID != -1) {
                    sbUpdate.Append("working_unit_id = " + empl.WorkingUnitID + ", ");
                }
                else {
                    sbUpdate.Append("working_unit_id = NULL, ");
                }
                if (empl.OrgUnitID != -1) {
                    sbUpdate.Append("organizational_unit_id = " + empl.OrgUnitID.ToString().Trim() + ", ");
                }
                else {
                    sbUpdate.Append("organizational_unit_id = NULL, ");
                }

                sbUpdate.Append("status = N'" + empl.Status.Trim() + "', ");
                sbUpdate.Append("password = N'" + empl.Password.Trim() + "', ");

                if (empl.AddressID != -1) {
                    sbUpdate.Append("address_id = " + empl.AddressID + ", ");
                }
                else {
                    sbUpdate.Append("address_id = NULL, ");
                }

                if (!empl.Picture.Trim().Equals("")) {
                    sbUpdate.Append("picture = N'" + empl.Picture.Trim() + "', ");
                }
                // don't change picture name if it already exists
                //				else
                //				{
                //					sbUpdate.Append("picture = NULL, ");
                //				}
                if (empl.WorkingGroupID != -1) {
                    sbUpdate.Append("employee_group_id = " + empl.WorkingGroupID + ", ");
                }
                else {
                    sbUpdate.Append("employee_group_id = NULL, ");
                }
                sbUpdate.Append("type = N'" + empl.Type.Trim() + "', ");

                if (empl.AccessGroupID != -1) {
                    sbUpdate.Append("access_group_id = " + empl.AccessGroupID + ", ");
                }
                else {
                    //can not be null
                    sbUpdate.Append("access_group_id = 0" + ", ");
                }
                if (empl.EmployeeTypeID != -1) {
                    sbUpdate.Append("employee_type_id = " + empl.EmployeeTypeID + ", ");
                }
                else {
                    //can not be null
                    sbUpdate.Append("employee_type_id = NULL, ");
                }
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE employee_id = '" + empl.EmployeeID + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0) {
                    isUpdated = true;
                }

                if (doCommit) {
                    sqlTrans.Commit();
                }
            }
            catch (Exception ex) {
                if (doCommit) {
                    sqlTrans.Rollback("UPDATE");
                }

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool update(string EmployeeID, string FirstName, string LastName, string WorkingUnitID,
            string Status, string Password, string AddressID, string Picture, string WorkingGroupID,
            string type, string accessGroupID, bool doCommit) {
            bool isUpdated = false;
            SqlTransaction sqlTrans = null;
            if (doCommit) {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");
            }
            else {
                sqlTrans = this.SqlTrans;
            }

            try {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE employees SET ");
                sbUpdate.Append("first_name = N'" + FirstName.Trim() + "', ");
                sbUpdate.Append("last_name = N'" + LastName.Trim() + "', ");

                if (Int32.Parse(WorkingUnitID.Trim()) != -1) {
                    sbUpdate.Append("working_unit_id = " + WorkingUnitID.Trim() + ", ");
                }
                else {
                    sbUpdate.Append("working_unit_id = NULL, ");
                }

                sbUpdate.Append("status = N'" + Status.Trim() + "', ");
                sbUpdate.Append("password = N'" + Password.Trim() + "', ");

                if (Int32.Parse(AddressID.Trim()) != -1) {
                    sbUpdate.Append("address_id = " + AddressID.Trim() + ", ");
                }
                else {
                    sbUpdate.Append("address_id = NULL, ");
                }

                if (!Picture.Trim().Equals("")) {
                    sbUpdate.Append("picture = N'" + Picture.Trim() + "', ");
                }
                // don't change picture name if it already exists
                //				else
                //				{
                //					sbUpdate.Append("picture = NULL, ");
                //				}
                if (Int32.Parse(WorkingGroupID.Trim()) != -1) {
                    sbUpdate.Append("employee_group_id = " + WorkingGroupID.Trim() + ", ");
                }
                else {
                    sbUpdate.Append("employee_group_id = NULL, ");
                }
                sbUpdate.Append("type = N'" + type.Trim() + "', ");

                if (Int32.Parse(accessGroupID.Trim()) != -1) {
                    sbUpdate.Append("access_group_id = " + accessGroupID.Trim() + ", ");
                }
                else {
                    //can not be null
                    sbUpdate.Append("access_group_id = 0" + ", ");
                }

                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE employee_id = '" + EmployeeID.Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0) {
                    isUpdated = true;
                }

                if (doCommit) {
                    sqlTrans.Commit();
                }
            }
            catch (Exception ex) {
                if (doCommit) {
                    sqlTrans.Rollback("UPDATE");
                }

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public List<EmployeeTO> getEmployeesWUResponsible(string wuID, List<int> typesVisible, DateTime from, DateTime to) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> employeesList = new List<EmployeeTO>();
            string select = "";

            try {
                if (wuID.Trim().Length <= 0 || (typesVisible != null && typesVisible.Count <= 0))
                    return employeesList;

                select = "(SELECT e.employee_id, e.last_name, e.first_name FROM employees e, employees_asco4 ea WHERE e.employee_id = ea.employee_id AND ea.integer_value_2 IN (" + wuID.Trim() + ") ";
                select += "AND (ea.datetime_value_2 < '" + to.AddDays(1).Date.ToString(dateTimeformat) + "') "
                            + "AND (ea.datetime_value_3 IS NULL OR ea.datetime_value_3 > '" + from.Date.ToString(dateTimeformat) + "') ";

                if (typesVisible != null) {
                    select += "AND e.employee_type_id IN (";
                    foreach (int id in typesVisible) {
                        select += id.ToString().Trim() + ",";
                    }

                    select = select.Substring(0, select.Length - 1);
                    select += ") ";
                }
                //if (onlyEmplTypeID != -1)
                //    select += "AND e.employee_type_id = '" + onlyEmplTypeID.ToString().Trim() + "' ";
                //if (exceptEmplTypeID != -1)
                //    select += "AND e.employee_type_id <> '" + exceptEmplTypeID.ToString().Trim() + "' ";
                select += ") UNION (SELECT e.employee_id, e.last_name, e.first_name FROM employees e, employees_asco4 ea, employee_responsibility er WHERE e.employee_id = ea.employee_id AND e.employee_id = er.employee_id AND er.unit_id IN ("
                    + wuID.Trim() + ") AND UPPER(er.type) = '" + Constants.emplResTypeWU.Trim().ToUpper() + "' "
                    + "AND (ea.datetime_value_2 < '" + to.AddDays(1).Date.ToString(dateTimeformat) + "') "
                    + "AND (ea.datetime_value_3 IS NULL OR ea.datetime_value_3 > '" + from.Date.ToString(dateTimeformat) + "') ";

                if (typesVisible != null) {
                    select += "AND e.employee_type_id IN (";
                    foreach (int id in typesVisible) {
                        select += id.ToString().Trim() + ",";
                    }

                    select = select.Substring(0, select.Length - 1);
                    select += ") ";
                }
                //if (onlyEmplTypeID != -1)
                //    select += "AND e.employee_type_id = '" + onlyEmplTypeID.ToString().Trim() + "' ";
                //if (exceptEmplTypeID != -1)
                //    select += "AND e.employee_type_id <> '" + exceptEmplTypeID.ToString().Trim() + "' ";
                select += ")";

                select += " ORDER BY e.last_name, e.first_name ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    foreach (DataRow row in table.Rows) {
                        employee = new EmployeeTO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["first_name"] != DBNull.Value) {
                            employee.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value) {
                            employee.LastName = row["last_name"].ToString().Trim();
                        }

                        employeesList.Add(employee);
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Exception: " + ex.Message);
            }

            return employeesList;
        }

        public List<EmployeeTO> getEmployeesOUResponsible(string ouID, List<int> typesVisible, DateTime from, DateTime to) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> employeesList = new List<EmployeeTO>();
            string select = "";

            try {
                if (ouID.Trim().Length <= 0 || (typesVisible != null && typesVisible.Count <= 0))
                    return employeesList;

                select = "(SELECT e.employee_id, e.last_name, e.first_name FROM employees e, employees_asco4 ea WHERE e.employee_id = ea.employee_id AND ea.integer_value_3 IN (" + ouID.Trim() + ") ";
                select += "AND (ea.datetime_value_2 < '" + to.AddDays(1).Date.ToString(dateTimeformat) + "') "
                           + "AND (ea.datetime_value_3 IS NULL OR ea.datetime_value_3 > '" + from.Date.ToString(dateTimeformat) + "') ";

                if (typesVisible != null) {
                    select += "AND e.employee_type_id IN (";
                    foreach (int id in typesVisible) {
                        select += id.ToString().Trim() + ",";
                    }

                    select = select.Substring(0, select.Length - 1);
                    select += ") ";
                }
                //if (onlyEmplTypeID != -1)
                //    select += "AND e.employee_type_id = '" + onlyEmplTypeID.ToString().Trim() + "' ";
                //if (exceptEmplTypeID != -1)
                //    select += "AND e.employee_type_id <> '" + exceptEmplTypeID.ToString().Trim() + "' ";
                select += ") UNION (SELECT e.employee_id, e.last_name, e.first_name FROM employees e, employees_asco4 ea, employee_responsibility er WHERE e.employee_id = ea.employee_id AND e.employee_id = er.employee_id AND er.unit_id IN ("
                    + ouID.Trim() + ") AND UPPER(er.type) = '" + Constants.emplResTypeOU.Trim().ToUpper() + "' "
                    + "AND (ea.datetime_value_2 < '" + to.AddDays(1).Date.ToString(dateTimeformat) + "') "
                    + "AND (ea.datetime_value_3 IS NULL OR ea.datetime_value_3 > '" + from.Date.ToString(dateTimeformat) + "') ";

                if (typesVisible != null) {
                    select += "AND e.employee_type_id IN (";
                    foreach (int id in typesVisible) {
                        select += id.ToString().Trim() + ",";
                    }

                    select = select.Substring(0, select.Length - 1);
                    select += ") ";
                }
                //if (onlyEmplTypeID != -1)
                //    select += "AND e.employee_type_id = '" + onlyEmplTypeID.ToString().Trim() + "' ";
                //if (exceptEmplTypeID != -1)
                //    select += "AND e.employee_type_id <> '" + exceptEmplTypeID.ToString().Trim() + "' ";
                select += ")";

                select += " ORDER BY e.last_name, e.first_name ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    foreach (DataRow row in table.Rows) {
                        employee = new EmployeeTO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["first_name"] != DBNull.Value) {
                            employee.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value) {
                            employee.LastName = row["last_name"].ToString().Trim();
                        }

                        employeesList.Add(employee);
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Exception: " + ex.Message);
            }

            return employeesList;
        }

        public Dictionary<int, EmployeeTO> getEmployeesDictionaryWithASCO(EmployeeTO emplTO) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            EmployeeAsco4TO asco4TO = new EmployeeAsco4TO();
            Dictionary<int, EmployeeTO> employeesList = new Dictionary<int, EmployeeTO>();
            string select = "";

            try {
                //employee must have asco4 data
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT e.*,wu.name wu_name,asco.* FROM employees e, working_units wu, employees_asco4 asco");
                sb.Append(" WHERE e.working_unit_id = wu.working_unit_id AND e.employee_id = asco.employee_id");
                if ((emplTO.EmployeeID != -1) || (!emplTO.FirstName.Trim().Equals("")) ||
                    (!emplTO.LastName.Trim().Equals("")) || (emplTO.WorkingUnitID != -1) ||
                    (!emplTO.Status.Trim().Equals("")) || (!emplTO.Password.Trim().Equals("")) ||
                    (emplTO.AddressID != -1) || (!emplTO.Picture.Trim().Equals("")) ||
                    (emplTO.WorkingGroupID != -1) || (!emplTO.Type.Trim().Equals("")) || emplTO.EmployeeTypeID != -1) {
                    sb.Append(" AND");

                    if (emplTO.EmployeeID != -1) {
                        sb.Append(" e.employee_id = '" + emplTO.EmployeeID.ToString().Trim() + "' AND");
                    }
                    if (!emplTO.FirstName.Trim().Equals("")) {
                        sb.Append(" UPPER(e.first_name) LIKE N'%" + emplTO.FirstName.Trim().ToUpper() + "%' AND");
                    }
                    if (!emplTO.LastName.Trim().Equals("")) {
                        sb.Append(" UPPER(e.last_name) LIKE N'%" + emplTO.LastName.Trim().ToUpper() + "%' AND");
                    }
                    if (emplTO.WorkingUnitID != -1) {
                        sb.Append(" e.working_unit_id IN (" + emplTO.WorkingUnitID.ToString().Trim() + ") AND");
                    }
                    if (!emplTO.Status.Trim().Equals("")) {
                        sb.Append(" UPPER(e.status) LIKE N'%" + emplTO.Status.Trim().ToUpper() + "%' AND");
                    }
                    if (!emplTO.Password.Trim().Equals("")) {
                        sb.Append(" UPPER(e.password) LIKE N'%" + emplTO.Password.Trim().ToUpper() + "%' AND");
                    }
                    if (emplTO.AddressID != -1) {
                        sb.Append(" e.address_id = '" + emplTO.AddressID.ToString().Trim() + "' AND");
                    }
                    if (!emplTO.Picture.Trim().Equals("")) {
                        sb.Append(" UPPER(e.picture) LIKE N'%" + emplTO.Picture.Trim().ToUpper() + "%' AND");
                    }
                    if (emplTO.WorkingGroupID != -1) {
                        sb.Append(" e.employee_group_id = '" + emplTO.WorkingGroupID.ToString().Trim() + "' AND");
                    }
                    if (emplTO.EmployeeTypeID != -1) {
                        sb.Append(" e.employee_type_id = " + emplTO.WorkingGroupID.ToString().Trim() + " AND");
                    }
                    if (!emplTO.Type.Trim().Equals("")) {
                        sb.Append(" UPPER(e.type) LIKE N'%" + emplTO.Type.Trim().ToUpper() + "%' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else {
                    select = sb.ToString();
                }

                select += " ORDER BY e.last_name, e.first_name ";

                SqlCommand cmd = new SqlCommand(select, conn);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    foreach (DataRow row in table.Rows) {
                        employee = new EmployeeTO();
                        asco4TO = new EmployeeAsco4TO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["first_name"] != DBNull.Value) {
                            employee.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value) {
                            employee.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["working_unit_id"] != DBNull.Value) {
                            employee.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["wu_name"] != DBNull.Value) {
                            employee.WorkingUnitName = row["wu_name"].ToString().Trim();
                        }
                        if (row["status"] != DBNull.Value) {
                            employee.Status = row["status"].ToString().Trim();
                        }
                        if (row["password"] != DBNull.Value) {
                            employee.Password = row["password"].ToString().Trim();
                        }
                        if (row["address_id"] != DBNull.Value) {
                            employee.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        if (row["picture"] != DBNull.Value) {
                            employee.Picture = row["picture"].ToString().Trim();
                        }
                        if (row["employee_group_id"] != DBNull.Value) {
                            employee.WorkingGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());
                        }
                        if (row["employee_type_id"] != DBNull.Value) {
                            employee.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                        }
                        if (row["type"] != DBNull.Value) {
                            employee.Type = row["type"].ToString().Trim();
                        }
                        if (row["organizational_unit_id"] != DBNull.Value) {
                            employee.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
                        }

                        //set asco4 values for employee
                        if (row["integer_value_1"] != DBNull.Value) {
                            asco4TO.IntegerValue1 = Int32.Parse(row["integer_value_1"].ToString().Trim());
                        }
                        if (row["integer_value_2"] != DBNull.Value) {
                            asco4TO.IntegerValue2 = Int32.Parse(row["integer_value_2"].ToString().Trim());
                        }
                        if (row["integer_value_3"] != DBNull.Value) {
                            asco4TO.IntegerValue3 = Int32.Parse(row["integer_value_3"].ToString().Trim());
                        }
                        if (row["integer_value_4"] != DBNull.Value) {
                            asco4TO.IntegerValue4 = Int32.Parse(row["integer_value_4"].ToString().Trim());
                        }
                        if (row["integer_value_5"] != DBNull.Value) {
                            asco4TO.IntegerValue5 = Int32.Parse(row["integer_value_5"].ToString().Trim());
                        }
                        if (row["datetime_value_1"] != DBNull.Value) {
                            asco4TO.DatetimeValue1 = DateTime.Parse(row["datetime_value_1"].ToString().Trim());
                        }
                        if (row["datetime_value_2"] != DBNull.Value) {
                            asco4TO.DatetimeValue2 = DateTime.Parse(row["datetime_value_2"].ToString().Trim());
                        }
                        if (row["datetime_value_3"] != DBNull.Value) {
                            asco4TO.DatetimeValue3 = DateTime.Parse(row["datetime_value_3"].ToString().Trim());
                        }
                        if (row["datetime_value_4"] != DBNull.Value) {
                            asco4TO.DatetimeValue4 = DateTime.Parse(row["datetime_value_4"].ToString().Trim());
                        }
                        if (row["datetime_value_5"] != DBNull.Value) {
                            asco4TO.DatetimeValue5 = DateTime.Parse(row["datetime_value_5"].ToString().Trim());
                        }
                        if (row["nvarchar_value_1"] != DBNull.Value) {
                            asco4TO.NVarcharValue1 = row["nvarchar_value_1"].ToString().Trim();
                        }
                        if (row["nvarchar_value_2"] != DBNull.Value) {
                            asco4TO.NVarcharValue2 = row["nvarchar_value_2"].ToString().Trim();
                        }
                        if (row["nvarchar_value_3"] != DBNull.Value) {
                            asco4TO.NVarcharValue3 = row["nvarchar_value_3"].ToString().Trim();
                        }
                        if (row["nvarchar_value_4"] != DBNull.Value) {
                            asco4TO.NVarcharValue4 = row["nvarchar_value_4"].ToString().Trim();
                        }
                        if (row["nvarchar_value_5"] != DBNull.Value) {
                            asco4TO.NVarcharValue5 = row["nvarchar_value_5"].ToString().Trim();
                        }

                        employee.Tag = asco4TO;

                        if (!employeesList.ContainsKey(employee.EmployeeID))
                            employeesList.Add(employee.EmployeeID, employee);
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Exception: " + ex.Message);
            }

            return employeesList;
        }
        public Dictionary<int, EmployeeTO> getEmployeesDictionary(EmployeeTO emplTO) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            Dictionary<int, EmployeeTO> employeesList = new Dictionary<int, EmployeeTO>();
            string select = "";

            try {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT e.*,wu.name wu_name FROM employees e, working_units wu");
                sb.Append(" WHERE e.working_unit_id = wu.working_unit_id");
                if ((emplTO.EmployeeID != -1) || (!emplTO.FirstName.Trim().Equals("")) ||
                    (!emplTO.LastName.Trim().Equals("")) || (emplTO.WorkingUnitID != -1) ||
                    (!emplTO.Status.Trim().Equals("")) || (!emplTO.Password.Trim().Equals("")) ||
                    (emplTO.AddressID != -1) || (!emplTO.Picture.Trim().Equals("")) ||
                    (emplTO.WorkingGroupID != -1) || (!emplTO.Type.Trim().Equals("")) || emplTO.EmployeeTypeID != -1) {
                    sb.Append(" AND");

                    if (emplTO.EmployeeID != -1) {
                        //sb.Append(" UPPER(employee_id) LIKE '" + EmployeeID.Trim().ToUpper() + "' AND");
                        sb.Append(" e.employee_id = '" + emplTO.EmployeeID.ToString().Trim() + "' AND");
                    }
                    if (!emplTO.FirstName.Trim().Equals("")) {
                        sb.Append(" UPPER(e.first_name) LIKE N'%" + emplTO.FirstName.Trim().ToUpper() + "%' AND");
                    }
                    if (!emplTO.LastName.Trim().Equals("")) {
                        sb.Append(" UPPER(e.last_name) LIKE N'%" + emplTO.LastName.Trim().ToUpper() + "%' AND");
                    }
                    if (emplTO.WorkingUnitID != -1) {
                        //sb.Append(" UPPER(working_unit_id) LIKE '" + WorkingUnitID.Trim().ToUpper() + "' AND");
                        sb.Append(" e.working_unit_id IN (" + emplTO.WorkingUnitID.ToString().Trim() + ") AND");
                    }
                    if (!emplTO.Status.Trim().Equals("")) {
                        sb.Append(" UPPER(e.status) LIKE N'%" + emplTO.Status.Trim().ToUpper() + "%' AND");
                    }
                    if (!emplTO.Password.Trim().Equals("")) {
                        sb.Append(" UPPER(e.password) LIKE N'%" + emplTO.Password.Trim().ToUpper() + "%' AND");
                    }
                    if (emplTO.AddressID != -1) {
                        //sb.Append(" UPPER(address_id) LIKE '" + AddressID.Trim().ToUpper() + "' AND");
                        sb.Append(" e.address_id = '" + emplTO.AddressID.ToString().Trim() + "' AND");
                    }
                    if (!emplTO.Picture.Trim().Equals("")) {
                        sb.Append(" UPPER(e.picture) LIKE N'%" + emplTO.Picture.Trim().ToUpper() + "%' AND");
                    }
                    if (emplTO.WorkingGroupID != -1) {
                        //sb.Append(" UPPER(employee_group_id) LIKE '" + WorkingGroupID.Trim().ToUpper() + "' AND");
                        sb.Append(" e.employee_group_id = '" + emplTO.WorkingGroupID.ToString().Trim() + "' AND");
                    }
                    if (emplTO.EmployeeTypeID != -1) {
                        //sb.Append(" UPPER(employee_group_id) LIKE '" + WorkingGroupID.Trim().ToUpper() + "' AND");
                        sb.Append(" e.employee_type_id = " + emplTO.WorkingGroupID.ToString().Trim() + " AND");
                    }
                    if (!emplTO.Type.Trim().Equals("")) {
                        sb.Append(" UPPER(e.type) LIKE N'%" + emplTO.Type.Trim().ToUpper() + "%' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else {
                    select = sb.ToString();
                }

                select += " ORDER BY e.last_name, e.first_name ";

                SqlCommand cmd = new SqlCommand(select, conn);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    foreach (DataRow row in table.Rows) {
                        employee = new EmployeeTO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["first_name"] != DBNull.Value) {
                            employee.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value) {
                            employee.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["working_unit_id"] != DBNull.Value) {
                            employee.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["wu_name"] != DBNull.Value) {
                            employee.WorkingUnitName = row["wu_name"].ToString().Trim();
                        }
                        if (row["status"] != DBNull.Value) {
                            employee.Status = row["status"].ToString().Trim();
                        }
                        if (row["password"] != DBNull.Value) {
                            employee.Password = row["password"].ToString().Trim();
                        }
                        if (row["address_id"] != DBNull.Value) {
                            employee.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        if (row["picture"] != DBNull.Value) {
                            employee.Picture = row["picture"].ToString().Trim();
                        }
                        if (row["employee_group_id"] != DBNull.Value) {
                            employee.WorkingGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());
                        }
                        if (row["employee_type_id"] != DBNull.Value) {
                            employee.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                        }
                        if (row["type"] != DBNull.Value) {
                            employee.Type = row["type"].ToString().Trim();
                        }
                        if (row["organizational_unit_id"] != DBNull.Value) {
                            employee.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
                        }
                        if (row["employee_type_id"] != DBNull.Value) {
                            employee.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                        }
                        if (!employeesList.ContainsKey(employee.EmployeeID))
                            employeesList.Add(employee.EmployeeID, employee);
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Exception: " + ex.Message);
            }

            return employeesList;
        }

        public Dictionary<int, EmployeeTO> getEmployeesDictionary(string emplIDs) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            Dictionary<int, EmployeeTO> employeesList = new Dictionary<int, EmployeeTO>();
            string select = "";

            try {
                select = "SELECT * FROM employees";
                if (emplIDs.Length > 0)
                    select += " WHERE employee_id IN (" + emplIDs.Trim() + ")";

                select += " ORDER BY last_name, first_name ";

                SqlCommand cmd = new SqlCommand(select, conn);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    foreach (DataRow row in table.Rows) {
                        employee = new EmployeeTO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["first_name"] != DBNull.Value) {
                            employee.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value) {
                            employee.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["working_unit_id"] != DBNull.Value) {
                            employee.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["employee_type_id"] != DBNull.Value) {
                            employee.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                        }
                        if (row["status"] != DBNull.Value) {
                            employee.Status = row["status"].ToString().Trim();
                        }
                        if (row["password"] != DBNull.Value) {
                            employee.Password = row["password"].ToString().Trim();
                        }
                        if (row["address_id"] != DBNull.Value) {
                            employee.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        if (row["picture"] != DBNull.Value) {
                            employee.Picture = row["picture"].ToString().Trim();
                        }
                        if (row["employee_group_id"] != DBNull.Value) {
                            employee.WorkingGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());
                        }
                        if (row["type"] != DBNull.Value) {
                            employee.Type = row["type"].ToString().Trim();
                        }
                        if (row["organizational_unit_id"] != DBNull.Value) {
                            employee.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
                        }
                        if (row["employee_type_id"] != DBNull.Value) {
                            employee.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                        }
                        if (row["created_by"] != DBNull.Value) {
                            employee.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value) {
                            employee.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (!employeesList.ContainsKey(employee.EmployeeID))
                            employeesList.Add(employee.EmployeeID, employee);
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Exception: " + ex.Message);
            }

            return employeesList;
        }


        public Dictionary<int, EmployeeTO> getEmployeesDictionaryWCSelfService() {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            Dictionary<int, EmployeeTO> employeesList = new Dictionary<int, EmployeeTO>();
            string select = "";

            try {
                select = "SELECT e.* FROM appl_users_x_appl_users_categories axc, employees_asco4 asco, appl_users appl_user, employees e ";

                select += "where appl_user.user_id=axc.user_id and axc.user_id = asco.nvarchar_value_5 and e.employee_id = asco.employee_id and appl_users_category_id = 2 and appl_user.status='ACTIVE'";

                SqlCommand cmd = new SqlCommand(select, conn);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    foreach (DataRow row in table.Rows) {
                        employee = new EmployeeTO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["first_name"] != DBNull.Value) {
                            employee.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value) {
                            employee.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["working_unit_id"] != DBNull.Value) {
                            employee.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["employee_type_id"] != DBNull.Value) {
                            employee.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                        }
                        if (row["status"] != DBNull.Value) {
                            employee.Status = row["status"].ToString().Trim();
                        }
                        if (row["password"] != DBNull.Value) {
                            employee.Password = row["password"].ToString().Trim();
                        }
                        if (row["address_id"] != DBNull.Value) {
                            employee.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        if (row["picture"] != DBNull.Value) {
                            employee.Picture = row["picture"].ToString().Trim();
                        }
                        if (row["employee_group_id"] != DBNull.Value) {
                            employee.WorkingGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());
                        }
                        if (row["type"] != DBNull.Value) {
                            employee.Type = row["type"].ToString().Trim();
                        }
                        if (row["organizational_unit_id"] != DBNull.Value) {
                            employee.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
                        }
                        if (row["employee_type_id"] != DBNull.Value) {
                            employee.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                        }
                        if (row["created_by"] != DBNull.Value) {
                            employee.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value) {
                            employee.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (!employeesList.ContainsKey(employee.EmployeeID))
                            employeesList.Add(employee.EmployeeID, employee);
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Exception: " + ex.Message);
            }

            return employeesList;
        }
        // Same as method getEmployees(string EmployeeID, string FirstName, string LastName, string WorkingUnitID, string Status, string Password, string AddressID, string Picture, string WorkingGroupID, string type, IdbTransaction trans)
        // command is made without transaction
        public List<EmployeeTO> getEmployees(EmployeeTO emplTO) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> employeesList = new List<EmployeeTO>();
            string select = "";

            try {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT e.*,wu.name wu_name FROM employees e, working_units wu");
                sb.Append(" WHERE e.working_unit_id = wu.working_unit_id");
                if ((emplTO.EmployeeID != -1) || (!emplTO.FirstName.Trim().Equals("")) ||
                    (!emplTO.LastName.Trim().Equals("")) || (emplTO.WorkingUnitID != -1) ||
                    (!emplTO.Status.Trim().Equals("")) || (!emplTO.Password.Trim().Equals("")) ||
                    (emplTO.AddressID != -1) || (!emplTO.Picture.Trim().Equals("")) ||
                    (emplTO.WorkingGroupID != -1) || (!emplTO.Type.Trim().Equals(""))
                    || (emplTO.OrgUnitID != -1)) {
                    sb.Append(" AND");

                    if (emplTO.EmployeeID != -1) {
                        //sb.Append(" UPPER(employee_id) LIKE '" + EmployeeID.Trim().ToUpper() + "' AND");
                        sb.Append(" e.employee_id = '" + emplTO.EmployeeID.ToString().Trim() + "' AND");
                    }
                    if (!emplTO.FirstName.Trim().Equals("")) {
                        sb.Append(" UPPER(e.first_name) LIKE N'%" + emplTO.FirstName.Trim().ToUpper() + "%' AND");
                    }
                    if (!emplTO.LastName.Trim().Equals("")) {
                        sb.Append(" UPPER(e.last_name) LIKE N'%" + emplTO.LastName.Trim().ToUpper() + "%' AND");
                    }
                    if (emplTO.WorkingUnitID != -1) {
                        //sb.Append(" UPPER(working_unit_id) LIKE '" + WorkingUnitID.Trim().ToUpper() + "' AND");
                        sb.Append(" e.working_unit_id IN (" + emplTO.WorkingUnitID.ToString().Trim() + ") AND");
                    }
                    if (!emplTO.Status.Trim().Equals("")) {
                        sb.Append(" UPPER(e.status) LIKE N'%" + emplTO.Status.Trim().ToUpper() + "%' AND");
                    }
                    if (!emplTO.Password.Trim().Equals("")) {
                        sb.Append(" UPPER(e.password) LIKE N'%" + emplTO.Password.Trim().ToUpper() + "%' AND");
                    }
                    if (emplTO.AddressID != -1) {
                        //sb.Append(" UPPER(address_id) LIKE '" + AddressID.Trim().ToUpper() + "' AND");
                        sb.Append(" e.address_id = '" + emplTO.AddressID.ToString().Trim() + "' AND");
                    }
                    if (!emplTO.Picture.Trim().Equals("")) {
                        sb.Append(" UPPER(e.picture) LIKE N'%" + emplTO.Picture.Trim().ToUpper() + "%' AND");
                    }
                    if (emplTO.WorkingGroupID != -1) {
                        //sb.Append(" UPPER(employee_group_id) LIKE '" + WorkingGroupID.Trim().ToUpper() + "' AND");
                        sb.Append(" e.employee_group_id = '" + emplTO.WorkingGroupID.ToString().Trim() + "' AND");
                    }
                    if (!emplTO.Type.Trim().Equals("")) {
                        sb.Append(" UPPER(e.type) LIKE N'%" + emplTO.Type.Trim().ToUpper() + "%' AND");
                    }
                    if (emplTO.OrgUnitID != -1) {
                        sb.Append(" e.organizational_unit_id = '" + emplTO.OrgUnitID.ToString().Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else {
                    select = sb.ToString();
                }

                select += " ORDER BY e.last_name, e.first_name ";

                SqlCommand cmd = new SqlCommand(select, conn);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    foreach (DataRow row in table.Rows) {
                        employee = new EmployeeTO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["first_name"] != DBNull.Value) {
                            employee.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value) {
                            employee.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["working_unit_id"] != DBNull.Value) {
                            employee.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["wu_name"] != DBNull.Value) {
                            employee.WorkingUnitName = row["wu_name"].ToString().Trim();
                        }
                        if (row["status"] != DBNull.Value) {
                            employee.Status = row["status"].ToString().Trim();
                        }
                        if (row["password"] != DBNull.Value) {
                            employee.Password = row["password"].ToString().Trim();
                        }
                        if (row["address_id"] != DBNull.Value) {
                            employee.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        if (row["picture"] != DBNull.Value) {
                            employee.Picture = row["picture"].ToString().Trim();
                        }
                        if (row["employee_group_id"] != DBNull.Value) {
                            employee.WorkingGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());
                        }
                        if (row["type"] != DBNull.Value) {
                            employee.Type = row["type"].ToString().Trim();
                        }
                        if (row["organizational_unit_id"] != DBNull.Value) {
                            employee.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
                        }
                        if (row["employee_type_id"] != DBNull.Value) {
                            employee.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                        }
                        employeesList.Add(employee);
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Exception: " + ex.Message);
            }

            return employeesList;
        }

        public List<EmployeeTO> getEmployees(string emplIDs) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> employeesList = new List<EmployeeTO>();
            string select = "";

            try {
                if (emplIDs.Length > 0) {
                    select = "SELECT * FROM employees WHERE employee_id IN (" + emplIDs + ") ORDER BY last_name, first_name";

                    SqlCommand cmd = new SqlCommand(select, conn);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "Employee");
                    DataTable table = dataSet.Tables["Employee"];

                    if (table.Rows.Count > 0) {
                        foreach (DataRow row in table.Rows) {
                            employee = new EmployeeTO();
                            employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                            if (row["first_name"] != DBNull.Value) {
                                employee.FirstName = row["first_name"].ToString().Trim();
                            }
                            if (row["last_name"] != DBNull.Value) {
                                employee.LastName = row["last_name"].ToString().Trim();
                            }
                            if (row["working_unit_id"] != DBNull.Value) {
                                employee.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                            }
                            if (row["status"] != DBNull.Value) {
                                employee.Status = row["status"].ToString().Trim();
                            }
                            if (row["password"] != DBNull.Value) {
                                employee.Password = row["password"].ToString().Trim();
                            }
                            if (row["address_id"] != DBNull.Value) {
                                employee.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                            }
                            if (row["picture"] != DBNull.Value) {
                                employee.Picture = row["picture"].ToString().Trim();
                            }
                            if (row["employee_group_id"] != DBNull.Value) {
                                employee.WorkingGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());
                            }
                            if (row["type"] != DBNull.Value) {
                                employee.Type = row["type"].ToString().Trim();
                            }
                            if (row["organizational_unit_id"] != DBNull.Value) {
                                employee.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
                            }
                            if (row["employee_type_id"] != DBNull.Value) {
                                employee.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                            }
                            employeesList.Add(employee);
                        }
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Exception: " + ex.Message);
            }

            return employeesList;
        }

        // Same as method getEmployees(string  EmployeeID, string FirstName, string LastName, string WorkingUnitID, string Status, string Password, string AddressID, string Picture, string WorkingGroupID, string type)
        // command is made with transaction
        public List<EmployeeTO> getEmployees(EmployeeTO emplTO, IDbTransaction trans) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> employeesList = new List<EmployeeTO>();
            string select = "";

            try {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employees");
                if ((emplTO.EmployeeID != -1) || (!emplTO.FirstName.Trim().Equals("")) ||
                    (!emplTO.LastName.Trim().Equals("")) || (emplTO.WorkingUnitID != -1) ||
                    (!emplTO.Status.Trim().Equals("")) || (!emplTO.Password.Trim().Equals("")) ||
                    (emplTO.AddressID != -1) || (!emplTO.Picture.Trim().Equals("")) ||
                    (emplTO.WorkingGroupID != -1) || (!emplTO.Type.Trim().Equals(""))
                    || (emplTO.OrgUnitID != -1)) {

                    sb.Append(" WHERE");

                    if (emplTO.EmployeeID != -1) {
                        //sb.Append(" UPPER(employee_id) LIKE '" + EmployeeID.Trim().ToUpper() + "' AND");
                        sb.Append(" employee_id = '" + emplTO.EmployeeID.ToString().Trim() + "' AND");
                    }
                    if (!emplTO.FirstName.Trim().Equals("")) {
                        sb.Append(" UPPER(first_name) LIKE N'%" + emplTO.FirstName.Trim().ToUpper() + "%' AND");
                    }
                    if (!emplTO.LastName.Trim().Equals("")) {
                        sb.Append(" UPPER(last_name) LIKE N'%" + emplTO.LastName.Trim().ToUpper() + "%' AND");
                    }
                    if (emplTO.WorkingUnitID != -1) {
                        //sb.Append(" UPPER(working_unit_id) LIKE '" + WorkingUnitID.Trim().ToUpper() + "' AND");
                        sb.Append(" working_unit_id = '" + emplTO.WorkingUnitID.ToString().Trim() + "' AND");
                    }
                    if (!emplTO.Status.Trim().Equals("")) {
                        sb.Append(" UPPER(status) LIKE N'%" + emplTO.Status.Trim().ToUpper() + "%' AND");
                    }
                    if (!emplTO.Password.Trim().Equals("")) {
                        sb.Append(" UPPER(password) LIKE N'%" + emplTO.Password.Trim().ToUpper() + "%' AND");
                    }
                    if (emplTO.AddressID != -1) {
                        //sb.Append(" UPPER(address_id) LIKE '" + AddressID.Trim().ToUpper() + "' AND");
                        sb.Append(" address_id = '" + emplTO.AddressID.ToString().Trim() + "' AND");
                    }
                    if (!emplTO.Picture.Trim().Equals("")) {
                        sb.Append(" UPPER(picture) LIKE N'%" + emplTO.Picture.Trim().ToUpper() + "%' AND");
                    }
                    if (emplTO.WorkingGroupID != -1) {
                        //sb.Append(" UPPER(employee_group_id) LIKE '" + WorkingGroupID.Trim().ToUpper() + "' AND");
                        sb.Append(" employee_group_id = '" + emplTO.WorkingGroupID.ToString().Trim() + "' AND");
                    }
                    if (!emplTO.Type.Trim().Equals("")) {
                        sb.Append(" UPPER(type) LIKE N'%" + emplTO.Type.Trim().ToUpper() + "%' AND");
                    }
                    if (emplTO.OrgUnitID != -1) {
                        sb.Append(" e.organizational_unit_id = '" + emplTO.OrgUnitID.ToString().Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else {
                    select = sb.ToString();
                }

                select += " ORDER BY last_name, first_name ";

                SqlCommand cmd = null;

                if (trans != null) {
                    cmd = new SqlCommand(select, conn, (SqlTransaction)trans);
                }
                else {
                    cmd = new SqlCommand(select, conn);
                }

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    foreach (DataRow row in table.Rows) {
                        employee = new EmployeeTO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["first_name"] != DBNull.Value) {
                            employee.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value) {
                            employee.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["working_unit_id"] != DBNull.Value) {
                            employee.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["status"] != DBNull.Value) {
                            employee.Status = row["status"].ToString().Trim();
                        }
                        if (row["password"] != DBNull.Value) {
                            employee.Password = row["password"].ToString().Trim();
                        }
                        if (row["address_id"] != DBNull.Value) {
                            employee.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        if (row["picture"] != DBNull.Value) {
                            employee.Picture = row["picture"].ToString().Trim();
                        }
                        if (row["employee_group_id"] != DBNull.Value) {
                            employee.WorkingGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());
                        }
                        if (row["type"] != DBNull.Value) {
                            employee.Type = row["type"].ToString().Trim();
                        }
                        if (row["organizational_unit_id"] != DBNull.Value) {
                            employee.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
                        }
                        if (row["employee_type_id"] != DBNull.Value) {
                            employee.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                        }
                        employeesList.Add(employee);
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Exception: " + ex.Message);
            }

            return employeesList;
        }

        public Dictionary<int, int> getEmployeesGroups(string emplIDs, IDbTransaction trans) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            Dictionary<int, int> employeesGroups = new Dictionary<int, int>();
            string select = "";

            try {
                if (emplIDs.Length > 0) {
                    select = "SELECT * FROM employees WHERE employee_id IN (" + emplIDs.Trim() + ")";

                    SqlCommand cmd;
                    if (trans != null)
                        cmd = new SqlCommand(select, conn, (SqlTransaction)trans);
                    else
                        cmd = new SqlCommand(select, conn);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "Employee");
                    DataTable table = dataSet.Tables["Employee"];

                    if (table.Rows.Count > 0) {
                        foreach (DataRow row in table.Rows) {
                            employee = new EmployeeTO();
                            employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                            if (row["employee_group_id"] != DBNull.Value) {
                                employee.WorkingGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());
                            }

                            if (!employeesGroups.ContainsKey(employee.EmployeeID))
                                employeesGroups.Add(employee.EmployeeID, employee.WorkingGroupID);
                        }
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Exception: " + ex.Message);
            }

            return employeesGroups;
        }

        public List<EmployeeTO> getEmployesWithStatus(EmployeeTO emplTO, List<string> statuses, string wuString) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> employeesList = new List<EmployeeTO>();
            string select;

            try {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT empl.*, wu.name wu_name FROM employees empl, working_units wu");

                sb.Append(" WHERE");
                sb.Append(" empl.working_unit_id = wu.working_unit_id AND");

                if ((emplTO.EmployeeID != -1) || (!emplTO.FirstName.Trim().Equals("")) ||
                    (!emplTO.LastName.Trim().Equals("")) || (!wuString.Trim().Equals("")) ||
                    (!emplTO.Status.Trim().Equals("")) || (!emplTO.Password.Trim().Equals("")) ||
                    (emplTO.AddressID != -1) || (!emplTO.Picture.Trim().Equals("")) ||
                    (emplTO.WorkingGroupID != -1) || (!emplTO.Type.Trim().Equals(""))) {
                    if (emplTO.EmployeeID != -1) {
                        //sb.Append(" UPPER(empl.employee_id) LIKE '" + EmployeeID.Trim().ToUpper() + "' AND");
                        sb.Append(" empl.employee_id = '" + emplTO.EmployeeID.ToString().Trim() + "' AND");
                    }
                    if (!emplTO.FirstName.Trim().Equals("")) {
                        sb.Append(" UPPER(empl.first_name) LIKE N'%" + emplTO.FirstName.Trim().ToUpper() + "%' AND");
                    }
                    if (!emplTO.LastName.Trim().Equals("")) {
                        sb.Append(" UPPER(empl.last_name) LIKE N'%" + emplTO.LastName.Trim().ToUpper() + "%' AND");
                    }
                    if (!wuString.Trim().Equals("")) {
                        //sb.Append(" UPPER(empl.working_unit_id) LIKE '" + WorkingUnitID.Trim().ToUpper() + "' AND");
                        sb.Append(" empl.working_unit_id IN (" + wuString.Trim() + ") AND");
                    }

                    if (statuses.Count > 0) {
                        string statusString = " (";
                        foreach (string status in statuses) {
                            statusString += (" UPPER(empl.status) = N'" + status.ToUpper() + "' OR ");
                        }
                        statusString = statusString.Substring(0, statusString.Length - 3);
                        sb.Append(statusString + ") AND");
                    }
                    if (!emplTO.Password.Trim().Equals("")) {
                        sb.Append(" UPPER(empl.password) LIKE N'%" + emplTO.Password.Trim().ToUpper() + "%' AND");
                    }
                    if (emplTO.AddressID != -1) {
                        //sb.Append(" UPPER(empl.address_id) LIKE '" + AddressID.Trim().ToUpper() + "' AND");
                        sb.Append(" empl.address_id = '" + emplTO.AddressID.ToString().Trim() + "' AND");
                    }
                    if (!emplTO.Picture.Trim().Equals("")) {
                        sb.Append(" UPPER(empl.picture) LIKE N'%" + emplTO.Picture.Trim().ToUpper() + "%' AND");
                    }
                    if (emplTO.WorkingGroupID != -1) {
                        //sb.Append(" UPPER(empl.employee_group_id) LIKE '" + WorkingGroupID.Trim().ToUpper() + "' AND");
                        sb.Append(" empl.employee_group_id = '" + emplTO.WorkingGroupID.ToString().Trim() + "' AND");
                    }
                    if (!emplTO.Type.Trim().Equals("")) {
                        sb.Append(" UPPER(empl.type) LIKE N'%" + emplTO.Type.Trim().ToUpper() + "%' AND");
                    }
                }

                select = sb.ToString(0, sb.ToString().Length - 3);

                SqlCommand cmd = new SqlCommand(select + " ORDER BY empl.last_name, empl.first_name", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    foreach (DataRow row in table.Rows) {
                        employee = new EmployeeTO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["first_name"] != DBNull.Value) {
                            employee.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value) {
                            employee.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["working_unit_id"] != DBNull.Value) {
                            employee.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["status"] != DBNull.Value) {
                            employee.Status = row["status"].ToString().Trim();
                        }
                        if (row["password"] != DBNull.Value) {
                            employee.Password = row["password"].ToString().Trim();
                        }
                        if (row["address_id"] != DBNull.Value) {
                            employee.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        if (row["picture"] != DBNull.Value) {
                            employee.Picture = row["picture"].ToString().Trim();
                        }
                        if (row["employee_group_id"] != DBNull.Value) {
                            employee.WorkingGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());
                        }
                        if (row["type"] != DBNull.Value) {
                            employee.Type = row["type"].ToString().Trim();
                        }
                        if (row["wu_name"] != DBNull.Value) {
                            employee.WorkingUnitName = row["wu_name"].ToString().Trim();
                        }
                        if (row["organizational_unit_id"] != DBNull.Value) {
                            employee.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
                        }
                        if (row["employee_type_id"] != DBNull.Value) {
                            employee.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                        }
                        employeesList.Add(employee);
                    }
                }

            }
            catch (Exception ex) {
                throw ex;
            }
            return employeesList;
        }

        public List<EmployeeTO> getEmployesWithStatusNotInGroup(List<string> statuses, string wuString, int groupID) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> employeesList = new List<EmployeeTO>();
            string select;

            try {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT empl.*, wu.name wu_name FROM employees empl, working_units wu");

                sb.Append(" WHERE");
                sb.Append(" empl.working_unit_id = wu.working_unit_id AND empl.employee_group_id <> '" + groupID.ToString().Trim() + "' ");
                if (!wuString.Trim().Equals("")) {
                    sb.Append("AND empl.working_unit_id IN (" + wuString.Trim() + ") ");
                }

                if (statuses.Count > 0) {
                    string statusString = "AND (";
                    foreach (string status in statuses) {
                        statusString += (" UPPER(empl.status) = N'" + status.ToUpper() + "' OR ");
                    }
                    statusString = statusString.Substring(0, statusString.Length - 3);
                    sb.Append(statusString + ")");
                }

                select = sb.ToString();

                SqlCommand cmd = new SqlCommand(select + " ORDER BY empl.last_name, empl.first_name", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    foreach (DataRow row in table.Rows) {
                        employee = new EmployeeTO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["first_name"] != DBNull.Value) {
                            employee.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value) {
                            employee.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["working_unit_id"] != DBNull.Value) {
                            employee.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["status"] != DBNull.Value) {
                            employee.Status = row["status"].ToString().Trim();
                        }
                        if (row["password"] != DBNull.Value) {
                            employee.Password = row["password"].ToString().Trim();
                        }
                        if (row["address_id"] != DBNull.Value) {
                            employee.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        if (row["picture"] != DBNull.Value) {
                            employee.Picture = row["picture"].ToString().Trim();
                        }
                        if (row["employee_group_id"] != DBNull.Value) {
                            employee.WorkingGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());
                        }
                        if (row["type"] != DBNull.Value) {
                            employee.Type = row["type"].ToString().Trim();
                        }
                        if (row["wu_name"] != DBNull.Value) {
                            employee.WorkingUnitName = row["wu_name"].ToString().Trim();
                        }
                        if (row["organizational_unit_id"] != DBNull.Value) {
                            employee.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
                        }
                        if (row["employee_type_id"] != DBNull.Value) {
                            employee.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                        }
                        employeesList.Add(employee);
                    }
                }
            }
            catch (Exception ex) {
                throw ex;
            }
            return employeesList;
        }

        public List<EmployeeTO> getEmployesTagsWithStatus(EmployeeTO emplTO, List<string> Statuses, string wUnits, int hasTag) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> employeesList = new List<EmployeeTO>();
            string select1 = "";
            string select2 = "";
            string condition = "";
            string select = "";

            try {
                StringBuilder sb = new StringBuilder();
                select1 = "(SELECT empl.*, wu.name wu_name, '1' has_tag, ets.date ets_date, ts.name ts_name"
                    + " FROM employees empl, working_units wu, employees_time_schedule ets, time_schema_hdr ts"
                    + " WHERE";

                select2 = "(SELECT empl.*, wu.name wu_name, '0' has_tag, ets.date ets_date, ts.name ts_name"
                    + " FROM employees empl, working_units wu, employees_time_schedule ets, time_schema_hdr ts"
                    + " WHERE";

                if ((emplTO.EmployeeID != -1) || (emplTO.OrgUnitID != -1) || (!emplTO.FirstName.Trim().Equals("")) ||
                    (!emplTO.LastName.Trim().Equals("")) || (emplTO.WorkingUnitID != -1) ||
                    (Statuses.Count > 0) || (!emplTO.Password.Trim().Equals("")) ||
                    (emplTO.AddressID != -1) || (!emplTO.Picture.Trim().Equals("")) ||
                    (emplTO.WorkingGroupID != -1) || (!emplTO.Type.Trim().Equals(""))) {
                    if (emplTO.EmployeeID != -1) {
                        //condition += " UPPER(empl.employee_id) LIKE '" + EmployeeID.Trim().ToUpper() + "' AND";
                        condition += " empl.employee_id = '" + emplTO.EmployeeID.ToString().Trim() + "' AND";
                    }
                    if (emplTO.OrgUnitID != -1) {
                        condition += " empl.organizational_unit_id = '" + emplTO.OrgUnitID.ToString().Trim() + "' AND";
                    }
                    if (!emplTO.FirstName.Trim().Equals("")) {
                        condition += " UPPER(empl.first_name) LIKE N'%" + emplTO.FirstName.Trim().ToUpper() + "%' AND";
                    }
                    if (!emplTO.LastName.Trim().Equals("")) {
                        condition += " UPPER(empl.last_name) LIKE N'%" + emplTO.LastName.Trim().ToUpper() + "%' AND";
                    }
                    if (emplTO.WorkingUnitID != -1) {
                        //condition += " UPPER(empl.working_unit_id) LIKE '" + WorkingUnitID.Trim().ToUpper() + "' AND";
                        condition += " empl.working_unit_id = '" + emplTO.WorkingUnitID.ToString().Trim() + "' AND";
                    }

                    if (Statuses.Count > 0) {
                        string statusString = " (";
                        foreach (string status in Statuses) {
                            statusString += (" UPPER(empl.status) = N'" + status.ToUpper() + "' OR ");
                        }
                        statusString = statusString.Substring(0, statusString.Length - 3);
                        condition += statusString + ") AND";
                    }
                    if (!emplTO.Password.Trim().Equals("")) {
                        condition += " UPPER(empl.password) LIKE N'" + emplTO.Password.Trim().ToUpper() + "' AND";
                    }
                    if (emplTO.AddressID != -1) {
                        //condition += " UPPER(empl.address_id) LIKE '" + AddressID.Trim().ToUpper() + "' AND";
                        condition += " empl.address_id = '" + emplTO.AddressID.ToString().Trim() + "' AND";
                    }
                    if (!emplTO.Picture.Trim().Equals("")) {
                        condition += " UPPER(empl.picture) LIKE N'" + emplTO.Picture.Trim().ToUpper() + "' AND";
                    }
                    if (emplTO.WorkingGroupID != -1) {
                        //condition += " UPPER(empl.employee_group_id) LIKE '" + WorkingGroupID.Trim().ToUpper() + "' AND";
                        condition += " empl.employee_group_id = '" + emplTO.WorkingGroupID.ToString().Trim() + "' AND";
                    }
                    if (!emplTO.Type.Trim().Equals("")) {
                        condition += " UPPER(empl.type) LIKE N'" + emplTO.Type.Trim().ToUpper() + "' AND";
                    }
                }

                condition += " empl.working_unit_id = wu.working_unit_id AND empl.working_unit_id IN (" + wUnits.Trim() + ")";
                condition += " AND empl.employee_id = ets.employee_id AND ets.time_schema_id = ts.time_schema_id";
                condition += " AND NOT EXISTS (SELECT etsch.* FROM employees_time_schedule etsch";
                condition += " WHERE empl.employee_id = etsch.employee_id and etsch.date > ets.date)";

                select1 += condition;
                select2 += condition;

                select1 += " AND EXISTS (SELECT * FROM tags t WHERE empl.employee_id = t.owner_id AND (UPPER(t.status) = N'ACTIVE' OR UPPER(t.status) = N'BLOCKED')))";
                select2 += " AND NOT EXISTS (SELECT * FROM tags t WHERE empl.employee_id = t.owner_id AND (UPPER(t.status) = N'ACTIVE' OR UPPER(t.status) = N'BLOCKED')))";

                if (hasTag == (int)Constants.HasTag.no) {
                    select = select2;
                }
                else if (hasTag == (int)Constants.HasTag.yes) {
                    select = select1;
                }
                else {
                    select = select1 + " UNION " + select2;
                }

                SqlCommand cmd = new SqlCommand(select + " ORDER BY empl.last_name, empl.first_name", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    foreach (DataRow row in table.Rows) {
                        employee = new EmployeeTO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["first_name"] != DBNull.Value) {
                            employee.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value) {
                            employee.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["working_unit_id"] != DBNull.Value) {
                            employee.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["status"] != DBNull.Value) {
                            employee.Status = row["status"].ToString().Trim();
                        }
                        if (row["password"] != DBNull.Value) {
                            employee.Password = row["password"].ToString().Trim();
                        }
                        if (row["address_id"] != DBNull.Value) {
                            employee.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        if (row["picture"] != DBNull.Value) {
                            employee.Picture = row["picture"].ToString().Trim();
                        }
                        if (row["employee_group_id"] != DBNull.Value) {
                            employee.WorkingGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());
                        }
                        if (row["type"] != DBNull.Value) {
                            employee.Type = row["type"].ToString().Trim();
                        }
                        if (row["wu_name"] != DBNull.Value) {
                            employee.WorkingUnitName = row["wu_name"].ToString().Trim();
                        }

                        if (row["has_tag"] != DBNull.Value) {
                            if (Int32.Parse(row["has_tag"].ToString().Trim()) > 0) {
                                employee.HasTag = true;
                            }
                            else {
                                employee.HasTag = false;
                            }
                        }

                        if (row["ts_name"] != DBNull.Value) {
                            employee.SchemaName = row["ts_name"].ToString().Trim();
                        }
                        if (row["ets_date"] != DBNull.Value) {
                            employee.ScheduleDate = (DateTime)row["ets_date"];
                        }
                        if (row["organizational_unit_id"] != DBNull.Value) {
                            employee.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
                        }
                        if (row["employee_type_id"] != DBNull.Value) {
                            employee.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                        }
                        employeesList.Add(employee);
                    }
                }

            }
            catch (Exception ex) {
                throw ex;
            }
            return employeesList;
        }

        public List<EmployeeTO> getEmployeesByWU(string wUnits) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> employeesList = new List<EmployeeTO>();
            string select;

            try {
                if (wUnits.Length > 0) {
                    select = "SELECT empl.*, wu.name wu_name FROM employees empl, working_units wu "
                        + "WHERE empl.working_unit_id = wu.working_unit_id AND empl.working_unit_id IN (" + wUnits + ")";

                    SqlCommand cmd = new SqlCommand(select + " ORDER BY empl.last_name, empl.first_name", conn);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "Employee");
                    DataTable table = dataSet.Tables["Employee"];

                    if (table.Rows.Count > 0) {
                        foreach (DataRow row in table.Rows) {
                            employee = new EmployeeTO();
                            employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                            if (row["first_name"] != DBNull.Value) {
                                employee.FirstName = row["first_name"].ToString().Trim();
                            }
                            if (row["last_name"] != DBNull.Value) {
                                employee.LastName = row["last_name"].ToString().Trim();
                            }
                            if (row["working_unit_id"] != DBNull.Value) {
                                employee.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                            }
                            if (row["status"] != DBNull.Value) {
                                employee.Status = row["status"].ToString().Trim();
                            }
                            if (row["password"] != DBNull.Value) {
                                employee.Password = row["password"].ToString().Trim();
                            }
                            if (row["address_id"] != DBNull.Value) {
                                employee.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                            }
                            if (row["picture"] != DBNull.Value) {
                                employee.Picture = row["picture"].ToString().Trim();
                            }
                            if (row["employee_group_id"] != DBNull.Value) {
                                employee.WorkingGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());
                            }
                            if (row["type"] != DBNull.Value) {
                                employee.Type = row["type"].ToString().Trim();
                            }
                            if (row["wu_name"] != DBNull.Value) {
                                employee.WorkingUnitName = row["wu_name"].ToString().Trim();
                            }
                            if (row["organizational_unit_id"] != DBNull.Value) {
                                employee.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
                            }
                            if (row["employee_type_id"] != DBNull.Value) {
                                employee.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                            }
                            employeesList.Add(employee);
                        }
                    }
                }
            }
            catch (Exception ex) {
                throw ex;
            }

            return employeesList;
        }

        public List<EmployeeTO> getEmployeesByOU(string oUnits, int emplID, List<int> typesVisible, DateTime from, DateTime to) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> employeesList = new List<EmployeeTO>();
            string select;

            try {
                if (oUnits.Length > 0) {
                    if (typesVisible == null || typesVisible.Count > 0) {
                        select = "SELECT e.* FROM employees e, employees_asco4 ea WHERE e.organizational_unit_id IN (" + oUnits + ") "
                            + "AND e.employee_id = ea.employee_id AND (ea.datetime_value_2 < '" + to.AddDays(1).Date.ToString(dateTimeformat) + "') "
                            + "AND (ea.datetime_value_3 IS NULL OR ea.datetime_value_3 > '" + from.Date.ToString(dateTimeformat) + "') ";

                        if (typesVisible != null) {
                            select += "AND e.employee_type_id IN (";
                            foreach (int id in typesVisible) {
                                select += id.ToString().Trim() + ",";
                            }

                            select = select.Substring(0, select.Length - 1);
                            select += ") ";
                        }

                        if (emplID != -1)
                            select += "AND e.employee_id <> '" + emplID.ToString().Trim() + "' ";

                        select += "ORDER BY e.last_name, e.first_name";

                        SqlCommand cmd = new SqlCommand(select, conn);
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                        sqlDataAdapter.Fill(dataSet, "Employee");
                        DataTable table = dataSet.Tables["Employee"];

                        if (table.Rows.Count > 0) {
                            foreach (DataRow row in table.Rows) {
                                employee = new EmployeeTO();
                                employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                                if (row["first_name"] != DBNull.Value) {
                                    employee.FirstName = row["first_name"].ToString().Trim();
                                }
                                if (row["last_name"] != DBNull.Value) {
                                    employee.LastName = row["last_name"].ToString().Trim();
                                }
                                if (row["working_unit_id"] != DBNull.Value) {
                                    employee.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                                }
                                if (row["status"] != DBNull.Value) {
                                    employee.Status = row["status"].ToString().Trim();
                                }
                                if (row["password"] != DBNull.Value) {
                                    employee.Password = row["password"].ToString().Trim();
                                }
                                if (row["address_id"] != DBNull.Value) {
                                    employee.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                                }
                                if (row["picture"] != DBNull.Value) {
                                    employee.Picture = row["picture"].ToString().Trim();
                                }
                                if (row["employee_group_id"] != DBNull.Value) {
                                    employee.WorkingGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());
                                }
                                if (row["type"] != DBNull.Value) {
                                    employee.Type = row["type"].ToString().Trim();
                                }
                                if (row["organizational_unit_id"] != DBNull.Value) {
                                    employee.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
                                }
                                if (row["employee_type_id"] != DBNull.Value) {
                                    employee.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                                }
                                employeesList.Add(employee);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                throw ex;
            }

            return employeesList;
        }

        public List<EmployeeTO> getEmployeesByWULoans(string wUnits, int emplID, List<int> typesVisible, DateTime from, DateTime to) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> employeesList = new List<EmployeeTO>();
            string select;

            //DateTime from = DateTime.Now.Date;
            //DateTime to = DateTime.Now.Date;

            try {
                if (wUnits.Length > 0) {
                    if (typesVisible == null || typesVisible.Count > 0) {
                        select = "SELECT e.* FROM employees e, employees_asco4 ea WHERE (((e.working_unit_id IN (" + wUnits.Trim() + ") AND e.employee_id NOT IN "
                            + "(SELECT employee_id FROM employee_loans WHERE date_start < '" + to.AddDays(1).Date.ToString(dateTimeformat) + "' AND date_end >= '" + from.ToString(dateTimeformat) + "')) "
                            + "OR e.employee_id IN (SELECT employee_id FROM employee_loans WHERE working_unit_id IN (" + wUnits.Trim() + ") AND date_start < '" + to.Date.AddDays(1).Date.ToString(dateTimeformat)
                            + "' AND date_end >= '" + from.Date.ToString(dateTimeformat) + "'))) "
                            + "AND e.employee_id = ea.employee_id AND (ea.datetime_value_2 < '" + to.AddDays(1).Date.ToString(dateTimeformat) + "') "
                            + "AND (ea.datetime_value_3 IS NULL OR ea.datetime_value_3 > '" + from.Date.ToString(dateTimeformat) + "') ";

                        if (typesVisible != null) {
                            select += "AND e.employee_type_id IN (";
                            foreach (int id in typesVisible) {
                                select += id.ToString().Trim() + ",";
                            }

                            select = select.Substring(0, select.Length - 1);
                            select += ") ";
                        }

                        if (emplID != -1)
                            select += "AND e.employee_id <> '" + emplID.ToString().Trim() + "' ";

                        select += "ORDER BY e.last_name, e.first_name";
                        SqlCommand cmd = new SqlCommand(select, conn);
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                        sqlDataAdapter.Fill(dataSet, "Employee");
                        DataTable table = dataSet.Tables["Employee"];

                        if (table.Rows.Count > 0) {
                            foreach (DataRow row in table.Rows) {
                                employee = new EmployeeTO();
                                employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                                if (row["first_name"] != DBNull.Value) {
                                    employee.FirstName = row["first_name"].ToString().Trim();
                                }
                                if (row["last_name"] != DBNull.Value) {
                                    employee.LastName = row["last_name"].ToString().Trim();
                                }
                                if (row["working_unit_id"] != DBNull.Value) {
                                    employee.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                                }
                                if (row["status"] != DBNull.Value) {
                                    employee.Status = row["status"].ToString().Trim();
                                }
                                if (row["password"] != DBNull.Value) {
                                    employee.Password = row["password"].ToString().Trim();
                                }
                                if (row["address_id"] != DBNull.Value) {
                                    employee.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                                }
                                if (row["picture"] != DBNull.Value) {
                                    employee.Picture = row["picture"].ToString().Trim();
                                }
                                if (row["employee_group_id"] != DBNull.Value) {
                                    employee.WorkingGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());
                                }
                                if (row["type"] != DBNull.Value) {
                                    employee.Type = row["type"].ToString().Trim();
                                }
                                if (row["organizational_unit_id"] != DBNull.Value) {
                                    employee.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
                                }
                                if (row["employee_type_id"] != DBNull.Value) {
                                    employee.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                                }
                                employeesList.Add(employee);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                throw ex;
            }

            return employeesList;
        }

        public List<EmployeeTO> getEmployeesNotInWU(string wUnits) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> employeesList = new List<EmployeeTO>();
            string select;

            try {
                if (wUnits.Length > 0) {
                    select = "SELECT empl.*, wu.name wu_name FROM employees empl, working_units wu "
                        + "WHERE empl.working_unit_id = wu.working_unit_id AND empl.working_unit_id NOT IN (" + wUnits + ")";

                    SqlCommand cmd = new SqlCommand(select + " ORDER BY empl.last_name, empl.first_name", conn);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "Employee");
                    DataTable table = dataSet.Tables["Employee"];

                    if (table.Rows.Count > 0) {
                        foreach (DataRow row in table.Rows) {
                            employee = new EmployeeTO();
                            employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                            if (row["first_name"] != DBNull.Value) {
                                employee.FirstName = row["first_name"].ToString().Trim();
                            }
                            if (row["last_name"] != DBNull.Value) {
                                employee.LastName = row["last_name"].ToString().Trim();
                            }
                            if (row["working_unit_id"] != DBNull.Value) {
                                employee.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                            }
                            if (row["status"] != DBNull.Value) {
                                employee.Status = row["status"].ToString().Trim();
                            }
                            if (row["password"] != DBNull.Value) {
                                employee.Password = row["password"].ToString().Trim();
                            }
                            if (row["address_id"] != DBNull.Value) {
                                employee.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                            }
                            if (row["picture"] != DBNull.Value) {
                                employee.Picture = row["picture"].ToString().Trim();
                            }
                            if (row["employee_group_id"] != DBNull.Value) {
                                employee.WorkingGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());
                            }
                            if (row["type"] != DBNull.Value) {
                                employee.Type = row["type"].ToString().Trim();
                            }
                            if (row["wu_name"] != DBNull.Value) {
                                employee.WorkingUnitName = row["wu_name"].ToString().Trim();
                            }
                            if (row["organizational_unit_id"] != DBNull.Value) {
                                employee.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
                            }
                            if (row["employee_type_id"] != DBNull.Value) {
                                employee.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                            }
                            employeesList.Add(employee);
                        }
                    }
                }
            }
            catch (Exception ex) {
                throw ex;
            }

            return employeesList;
        }

        public List<EmployeeTO> getEmployeesByWUWithStatuses(string wUnits, List<string> statuses) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> employeesList = new List<EmployeeTO>();
            string select;

            try {
                if (wUnits.Length > 0) {
                    select = "SELECT empl.*, wu.name wu_name FROM employees empl, working_units wu "
                        + "WHERE empl.working_unit_id = wu.working_unit_id AND empl.working_unit_id IN (" + wUnits + ")";

                    if (statuses.Count > 0) {
                        select += " AND (";
                        foreach (string status in statuses) {
                            select += (" UPPER(empl.status) = N'" + status.ToUpper() + "' OR ");
                        }
                        select = select.Substring(0, select.Length - 3);
                        select += ")";
                    }

                    SqlCommand cmd = new SqlCommand(select + " ORDER BY empl.last_name, empl.first_name", conn);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "Employee");
                    DataTable table = dataSet.Tables["Employee"];

                    if (table.Rows.Count > 0) {
                        foreach (DataRow row in table.Rows) {
                            employee = new EmployeeTO();
                            employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                            if (row["first_name"] != DBNull.Value) {
                                employee.FirstName = row["first_name"].ToString().Trim();
                            }
                            if (row["last_name"] != DBNull.Value) {
                                employee.LastName = row["last_name"].ToString().Trim();
                            }
                            if (row["working_unit_id"] != DBNull.Value) {
                                employee.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                            }
                            if (row["status"] != DBNull.Value) {
                                employee.Status = row["status"].ToString().Trim();
                            }
                            if (row["password"] != DBNull.Value) {
                                employee.Password = row["password"].ToString().Trim();
                            }
                            if (row["address_id"] != DBNull.Value) {
                                employee.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                            }
                            if (row["picture"] != DBNull.Value) {
                                employee.Picture = row["picture"].ToString().Trim();
                            }
                            if (row["employee_group_id"] != DBNull.Value) {
                                employee.WorkingGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());
                            }
                            if (row["type"] != DBNull.Value) {
                                employee.Type = row["type"].ToString().Trim();
                            }
                            if (row["wu_name"] != DBNull.Value) {
                                employee.WorkingUnitName = row["wu_name"].ToString().Trim();
                            }
                            if (row["organizational_unit_id"] != DBNull.Value) {
                                employee.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
                            }
                            if (row["employee_type_id"] != DBNull.Value) {
                                employee.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                            }
                            employeesList.Add(employee);
                        }
                    }
                }
            }
            catch (Exception ex) {
                throw ex;
            }

            return employeesList;
        }

        public List<EmployeeTO> getEmployeesByWUGetAccessGroup(string wUnits) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> employeesList = new List<EmployeeTO>();
            string select;

            try {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT employee_id, first_name, last_name, access_group_id FROM employees ");
                sb.Append(" WHERE (UPPER(status) = N'ACTIVE' OR UPPER(status) = N'BLOCKED') ");

                if (!wUnits.Trim().Equals("")) {
                    sb.Append(" AND ");
                    sb.Append("employees.working_unit_id IN (" + wUnits + ")"); //int.Parse(wUnits)

                    select = sb.ToString();
                }
                else {
                    select = sb.ToString();
                }

                select = select + " ORDER BY employees.last_name, employees.first_name";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    foreach (DataRow row in table.Rows) {
                        employee = new EmployeeTO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["first_name"] != DBNull.Value) {
                            employee.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value) {
                            employee.LastName = row["last_name"].ToString().Trim();
                        }
                        if (!row["access_group_id"].Equals(DBNull.Value)) {
                            employee.AccessGroupID = (int)row["access_group_id"]; //Int32.Parse(row["access_group_id"].ToString().Trim());
                        }
                        employeesList.Add(employee);
                    }
                }
            }
            catch (Exception ex) {
                throw ex;
            }

            return employeesList;
        }

        public List<EmployeeTO> getEmployeesByAccessGroup(string accessGroupID) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> employeesList = new List<EmployeeTO>();
            string select;

            try {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT employee_id, first_name, last_name, employees.working_unit_id, working_units.name FROM employees, working_units ");
                sb.Append("WHERE employees.working_unit_id = working_units.working_unit_id");
                sb.Append(" AND (UPPER(employees.status) = N'ACTIVE' OR UPPER(employees.status) = N'BLOCKED') ");

                if (!accessGroupID.Trim().Equals("")) {
                    sb.Append(" AND ");
                    sb.Append("employees.access_group_id = " + accessGroupID); //int.Parse(accessGroupID)

                    select = sb.ToString();
                }
                else {
                    select = sb.ToString();
                }

                select = select + " ORDER BY employees.last_name, employees.first_name";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    foreach (DataRow row in table.Rows) {
                        employee = new EmployeeTO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["first_name"] != DBNull.Value) {
                            employee.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value) {
                            employee.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["name"] != DBNull.Value) {
                            employee.WorkingUnitName = row["name"].ToString().Trim();
                        }
                        employeesList.Add(employee);
                    }
                }
            }
            catch (Exception ex) {
                throw ex;
            }

            return employeesList;
        }

        public List<EmployeeTO> getEmployeesNotInAccessGroup(string accessGroupID) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> employeesList = new List<EmployeeTO>();
            string select;

            try {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT employee_id, first_name, last_name, employees.working_unit_id, working_units.name FROM employees, working_units ");
                sb.Append("WHERE employees.working_unit_id = working_units.working_unit_id");
                sb.Append(" AND (UPPER(employees.status) = N'ACTIVE' OR UPPER(employees.status) = N'BLOCKED') ");

                if (!accessGroupID.Trim().Equals("")) {
                    sb.Append(" AND ");
                    sb.Append("employees.access_group_id <> " + accessGroupID); //int.Parse(accessGroupID)

                    select = sb.ToString();
                }
                else {
                    select = sb.ToString();
                }

                select = select + " ORDER BY employees.last_name, employees.first_name";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    foreach (DataRow row in table.Rows) {
                        employee = new EmployeeTO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["first_name"] != DBNull.Value) {
                            employee.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value) {
                            employee.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["name"] != DBNull.Value) {
                            employee.WorkingUnitName = row["name"].ToString().Trim();
                        }
                        employeesList.Add(employee);
                    }
                }
            }
            catch (Exception ex) {
                throw ex;
            }

            return employeesList;
        }

        public List<EmployeeTO> getEmployeesVisitors(string wUnits, List<string> statuses, string type) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> employeesList = new List<EmployeeTO>();
            string select = "";

            try {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employees");
                if (!wUnits.Trim().Equals("")) {
                    sb.Append(" WHERE");

                    sb.Append(" working_unit_id IN (" + wUnits + ")");

                    if (statuses.Count > 0) {
                        StringBuilder sb1 = new StringBuilder();
                        sb1.Append(" AND (");
                        foreach (string status in statuses) {
                            sb1.Append(" UPPER(status) = N'" + status.ToUpper() + "' OR ");
                        }

                        sb.Append(sb1.ToString(0, sb1.ToString().Length - 3) + ")");
                    }

                    if (type.Equals("AVAILABLE")) {
                        sb.Append(" AND NOT EXISTS");
                        sb.Append(" (");
                        sb.Append(" SELECT employee_id FROM visits");
                        sb.Append(" WHERE employee_id = employees.employee_id");
                        sb.Append(" AND date_end is NULL");
                        sb.Append(")");
                    }
                    else if (type.Equals("IN_USE")) {
                        sb.Append(" AND EXISTS");
                        sb.Append(" (");
                        sb.Append(" SELECT employee_id FROM visits");
                        sb.Append(" WHERE employee_id = employees.employee_id");
                        sb.Append(" AND date_end is NULL");
                        sb.Append(")");
                    }

                    select = sb.ToString();
                }
                else {
                    select = sb.ToString();
                }

                select += " ORDER BY last_name, first_name ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    foreach (DataRow row in table.Rows) {
                        employee = new EmployeeTO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["first_name"] != DBNull.Value) {
                            employee.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value) {
                            employee.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["working_unit_id"] != DBNull.Value) {
                            employee.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["status"] != DBNull.Value) {
                            employee.Status = row["status"].ToString().Trim();
                        }
                        if (row["password"] != DBNull.Value) {
                            employee.Password = row["password"].ToString().Trim();
                        }
                        if (row["address_id"] != DBNull.Value) {
                            employee.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        if (row["picture"] != DBNull.Value) {
                            employee.Picture = row["picture"].ToString().Trim();
                        }
                        if (row["employee_group_id"] != DBNull.Value) {
                            employee.WorkingGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());
                        }
                        if (row["type"] != DBNull.Value) {
                            employee.Type = row["type"].ToString().Trim();
                        }
                        if (row["organizational_unit_id"] != DBNull.Value) {
                            employee.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
                        }
                        if (row["employee_type_id"] != DBNull.Value) {
                            employee.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                        }
                        employeesList.Add(employee);
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Exception: " + ex.Message);
            }

            return employeesList;
        }

        public List<EmployeeTO> getEmployeesByTags(string tags) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> employeesList = new List<EmployeeTO>();
            string select = "";

            try {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employees");
                if (!tags.Trim().Equals("")) {
                    sb.Append(" WHERE");

                    sb.Append(" employee_id IN ");
                    sb.Append(" (");
                    sb.Append(" SELECT owner_id FROM tags");
                    sb.Append(" WHERE tag_id IN ( " + tags + ")");
                    sb.Append(" AND status = '" + Constants.statusActive + "'");
                    sb.Append(" )");

                    select = sb.ToString();
                }
                else {
                    select = sb.ToString();
                }

                select += " ORDER BY last_name, first_name ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    foreach (DataRow row in table.Rows) {
                        employee = new EmployeeTO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["first_name"] != DBNull.Value) {
                            employee.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value) {
                            employee.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["working_unit_id"] != DBNull.Value) {
                            employee.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["status"] != DBNull.Value) {
                            employee.Status = row["status"].ToString().Trim();
                        }
                        if (row["password"] != DBNull.Value) {
                            employee.Password = row["password"].ToString().Trim();
                        }
                        if (row["address_id"] != DBNull.Value) {
                            employee.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        if (row["picture"] != DBNull.Value) {
                            employee.Picture = row["picture"].ToString().Trim();
                        }
                        if (row["employee_group_id"] != DBNull.Value) {
                            employee.WorkingGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());
                        }
                        if (row["type"] != DBNull.Value) {
                            employee.Type = row["type"].ToString().Trim();
                        }
                        if (row["organizational_unit_id"] != DBNull.Value) {
                            employee.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
                        }
                        if (row["employee_type_id"] != DBNull.Value) {
                            employee.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                        }
                        employeesList.Add(employee);
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Exception: " + ex.Message);
            }

            return employeesList;
        }

        public EmployeeTO getEmployeesByTag(string tagID) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();

            string select = "";

            try {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employees");
                if (!tagID.Trim().Equals("")) {
                    sb.Append(" WHERE");

                    sb.Append(" employee_id IN ");
                    sb.Append(" (");
                    sb.Append(" SELECT owner_id FROM tags");
                    sb.Append(" WHERE tag_id = '" + tagID + "'");
                    sb.Append(" )");

                    select = sb.ToString();
                }
                else {
                    select = sb.ToString();
                }

                select += " ORDER BY last_name, first_name ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count == 1) {
                    DataRow row = table.Rows[0];
                    employee = new EmployeeTO();
                    employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                    if (row["first_name"] != DBNull.Value) {
                        employee.FirstName = row["first_name"].ToString().Trim();
                    }
                    if (row["last_name"] != DBNull.Value) {
                        employee.LastName = row["last_name"].ToString().Trim();
                    }
                    if (row["working_unit_id"] != DBNull.Value) {
                        employee.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                    }
                    if (row["status"] != DBNull.Value) {
                        employee.Status = row["status"].ToString().Trim();
                    }
                    if (row["password"] != DBNull.Value) {
                        employee.Password = row["password"].ToString().Trim();
                    }
                    if (row["address_id"] != DBNull.Value) {
                        employee.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                    }
                    if (row["picture"] != DBNull.Value) {
                        employee.Picture = row["picture"].ToString().Trim();
                    }
                    if (row["employee_group_id"] != DBNull.Value) {
                        employee.WorkingGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());
                    }
                    if (row["type"] != DBNull.Value) {
                        employee.Type = row["type"].ToString().Trim();
                    }
                    if (row["organizational_unit_id"] != DBNull.Value) {
                        employee.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
                    }
                    if (row["employee_type_id"] != DBNull.Value) {
                        employee.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Exception: " + ex.Message);
            }

            return employee;
        }


        public Dictionary<uint, EmployeeTO> getEmployeesByTagsDictionary(string tags) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            Dictionary<uint, EmployeeTO> employeesList = new Dictionary<uint, EmployeeTO>();
            string select = "";

            try {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT e.*, t.tag_id FROM employees e,tags t");
                if (!tags.Trim().Equals("")) {
                    sb.Append(" WHERE");

                    sb.Append(" t.tag_id IN ");
                    sb.Append(" (" + tags + ")");
                    sb.Append(" AND e.employee_id = t.owner_id AND t.status = 'ACTIVE' ");

                    select = sb.ToString();
                }
                else {
                    select = sb.ToString();
                }

                select += " ORDER BY e.last_name, e.first_name ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    foreach (DataRow row in table.Rows) {
                        uint tag_id = 0;
                        employee = new EmployeeTO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["first_name"] != DBNull.Value) {
                            employee.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value) {
                            employee.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["working_unit_id"] != DBNull.Value) {
                            employee.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["status"] != DBNull.Value) {
                            employee.Status = row["status"].ToString().Trim();
                        }
                        if (row["password"] != DBNull.Value) {
                            employee.Password = row["password"].ToString().Trim();
                        }
                        if (row["address_id"] != DBNull.Value) {
                            employee.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        if (row["picture"] != DBNull.Value) {
                            employee.Picture = row["picture"].ToString().Trim();
                        }
                        if (row["employee_group_id"] != DBNull.Value) {
                            employee.WorkingGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());
                        }
                        if (row["type"] != DBNull.Value) {
                            employee.Type = row["type"].ToString().Trim();
                        }
                        if (row["organizational_unit_id"] != DBNull.Value) {
                            employee.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
                        }
                        if (row["employee_type_id"] != DBNull.Value) {
                            employee.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                        }
                        if (row["tag_id"] != DBNull.Value) {
                            tag_id = uint.Parse(row["tag_id"].ToString().Trim());
                        }
                        if (!employeesList.ContainsKey(tag_id))
                            employeesList.Add(tag_id, employee);
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Exception: " + ex.Message);
            }

            return employeesList;
        }

        public List<EmployeeTO> getEmployeesByBlockedTags(string tags) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> employeesList = new List<EmployeeTO>();
            string select = "";

            try {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employees");
                if (!tags.Trim().Equals("")) {
                    sb.Append(" WHERE");
                    sb.Append(" employee_id IN ");
                    sb.Append(" (");
                    sb.Append(" SELECT owner_id FROM tags");
                    sb.Append(" WHERE tag_id IN ( " + tags + ")");
                    sb.Append(" AND status = '" + Constants.statusBlocked + "'");
                    sb.Append(" )");

                    select = sb.ToString();
                }
                else {
                    select = sb.ToString();
                }

                select += " ORDER BY last_name, first_name ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    foreach (DataRow row in table.Rows) {
                        employee = new EmployeeTO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["first_name"] != DBNull.Value) {
                            employee.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value) {
                            employee.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["working_unit_id"] != DBNull.Value) {
                            employee.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["status"] != DBNull.Value) {
                            employee.Status = row["status"].ToString().Trim();
                        }
                        if (row["password"] != DBNull.Value) {
                            employee.Password = row["password"].ToString().Trim();
                        }
                        if (row["address_id"] != DBNull.Value) {
                            employee.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        if (row["picture"] != DBNull.Value) {
                            employee.Picture = row["picture"].ToString().Trim();
                        }
                        if (row["employee_group_id"] != DBNull.Value) {
                            employee.WorkingGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());
                        }
                        if (row["type"] != DBNull.Value) {
                            employee.Type = row["type"].ToString().Trim();
                        }
                        if (row["organizational_unit_id"] != DBNull.Value) {
                            employee.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
                        }
                        if (row["employee_type_id"] != DBNull.Value) {
                            employee.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                        }
                        employeesList.Add(employee);
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Exception: " + ex.Message);
            }

            return employeesList;
        }

        public bool beginTransaction() {
            bool isStarted = false;

            try {
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
                isStarted = true;
            }
            catch (Exception ex) {
                isStarted = false;
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void commitTransaction() {
            this.SqlTrans.Commit();
            this.SqlTrans = null;
        }

        public void rollbackTransaction() {
            this.SqlTrans.Rollback();
            this.SqlTrans = null;
        }

        public IDbTransaction getTransaction() {
            return _sqlTrans;
        }

        public void setTransaction(IDbTransaction trans) {
            _sqlTrans = (SqlTransaction)trans;
        }

        public bool updateAccessGroup(string EmployeeID, string AccessGroupID, bool doCommit) {
            bool isUpdated = false;
            SqlTransaction sqlTrans = null;

            if (doCommit) {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");
            }
            else {
                sqlTrans = this.SqlTrans;
            }

            try {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE employees SET ");
                sbUpdate.Append("access_group_id = " + AccessGroupID.Trim() + ", ");
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE employee_id = '" + EmployeeID.Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0) {
                    isUpdated = true;
                }

                if (doCommit) {
                    sqlTrans.Commit();
                }

            }
            catch (Exception ex) {
                if (doCommit)
                    sqlTrans.Rollback("UPDATE");
                else
                    sqlTrans.Rollback();

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool updateWU(string EmployeeID, string WorkingUnitID, bool doCommit) {
            bool isUpdated = false;
            SqlTransaction sqlTrans = null;

            if (doCommit) {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");
            }
            else {
                sqlTrans = this.SqlTrans;
            }

            try {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE employees SET ");
                sbUpdate.Append("working_unit_id = " + WorkingUnitID.Trim() + ", ");
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE employee_id = '" + EmployeeID.Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0) {
                    isUpdated = true;
                }

                if (doCommit) {
                    sqlTrans.Commit();
                }

            }
            catch (Exception ex) {
                if (doCommit) {
                    sqlTrans.Rollback("UPDATE");
                }

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool updateOU(string EmployeeID, string ouID, bool doCommit) {
            bool isUpdated = false;
            SqlTransaction sqlTrans = null;

            if (doCommit) {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");
            }
            else {
                sqlTrans = this.SqlTrans;
            }

            try {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE employees SET ");
                sbUpdate.Append("organizational_unit_id = " + ouID.Trim() + ", ");
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE employee_id = '" + EmployeeID.Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res >= 0) {
                    isUpdated = true;
                }

                if (doCommit) {
                    sqlTrans.Commit();
                }

            }
            catch (Exception ex) {
                if (doCommit) {
                    sqlTrans.Rollback("UPDATE");
                }

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool updatePassword(string employeeID, string password) {
            bool isUpdated = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

            try {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE employees SET ");
                sbUpdate.Append("password = N'" + password.Trim() + "', ");
                sbUpdate.Append("modified_by = N'EMPLOYEE: " + employeeID.Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE employee_id = '" + employeeID.Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0) {
                    isUpdated = true;
                }

                sqlTrans.Commit();
            }
            catch (Exception ex) {
                sqlTrans.Rollback("UPDATE");

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public TagTO findActive(int ownerID) {
            DataSet dataSet = new DataSet();
            TagTO tag = new TagTO();
            try {
                string select = "SELECT * FROM tags WHERE owner_id = '" + ownerID.ToString().Trim() + "' "
                    + "AND ( UPPER(status) = N'" + Constants.statusActive.ToUpper()
                    + "' OR UPPER(status) = N'" + Constants.statusBlocked.ToUpper() + "')";
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Tag");
                DataTable table = dataSet.Tables["Tag"];

                if (table.Rows.Count == 1) {
                    tag = new TagTO();
                    tag.RecordID = Int32.Parse(table.Rows[0]["record_id"].ToString().Trim());
                    if (table.Rows[0]["tag_id"] != DBNull.Value) {
                        tag.TagID = UInt32.Parse(table.Rows[0]["tag_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["owner_id"] != DBNull.Value) {
                        tag.OwnerID = Int32.Parse(table.Rows[0]["owner_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["status"] != DBNull.Value) {
                        tag.Status = table.Rows[0]["status"].ToString().Trim();
                    }
                    if (table.Rows[0]["description"] != DBNull.Value) {
                        tag.Description = table.Rows[0]["description"].ToString().Trim();
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Exception: " + ex.Message);
            }

            return tag;
        }

        public EmployeeTO findEmplMealType(uint tagID) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();

            try {
                string select = "SELECT e.*, mt.name FROM employees e, meals_type_empl mt, tags t, employee_x_meal_type_employees me "
                    + "WHERE e.employee_id = t.owner_id AND t.tag_id = '" + tagID.ToString().Trim() + "' AND "
                    + "e.employee_id = me.employee_id AND t.status = 'ACTIVE' AND mt.meals_type_empl_id = me.meals_type_empl_id";
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count == 1) {
                    employee = new EmployeeTO();
                    employee.EmployeeID = Int32.Parse(table.Rows[0]["employee_id"].ToString().Trim());
                    if (table.Rows[0]["first_name"] != DBNull.Value) {
                        employee.FirstName = table.Rows[0]["first_name"].ToString().Trim();
                    }
                    if (table.Rows[0]["last_name"] != DBNull.Value) {
                        employee.LastName = table.Rows[0]["last_name"].ToString().Trim();
                    }
                    if (table.Rows[0]["working_unit_id"] != DBNull.Value) {
                        employee.WorkingUnitID = Int32.Parse(table.Rows[0]["working_unit_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["status"] != DBNull.Value) {
                        employee.Status = table.Rows[0]["status"].ToString().Trim();
                    }
                    if (table.Rows[0]["password"] != DBNull.Value) {
                        employee.Password = table.Rows[0]["password"].ToString().Trim();
                    }
                    if (table.Rows[0]["address_id"] != DBNull.Value) {
                        employee.AddressID = Int32.Parse(table.Rows[0]["address_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["picture"] != DBNull.Value) {
                        employee.Picture = table.Rows[0]["picture"].ToString().Trim();
                    }
                    if (table.Rows[0]["employee_group_id"] != DBNull.Value) {
                        employee.WorkingGroupID = Int32.Parse(table.Rows[0]["employee_group_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["type"] != DBNull.Value) {
                        employee.Type = table.Rows[0]["type"].ToString().Trim();
                    }

                    if (table.Rows[0]["access_group_id"] != DBNull.Value) {
                        employee.AccessGroupID = Int32.Parse(table.Rows[0]["access_group_id"].ToString().Trim());
                    }

                    if (table.Rows[0]["name"] != DBNull.Value) {
                        employee.MealTypeName = table.Rows[0]["name"].ToString().Trim();
                    }
                    if (table.Rows[0]["organizational_unit_id"] != DBNull.Value) {
                        employee.OrgUnitID = Int32.Parse(table.Rows[0]["organizational_unit_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["employee_type_id"] != DBNull.Value) {
                        employee.EmployeeTypeID = Int32.Parse(table.Rows[0]["employee_type_id"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex) {

                throw new Exception("Exception: " + ex.Message);
            }

            return employee;
        }

        public List<EmployeeTO> getEmplNumByWUnits() {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> employeesList = new List<EmployeeTO>();

            try {
                string select = "SELECT working_unit_id, count(*) num FROM employees WHERE status IN ('ACTIVE', 'BLOCKED') GROUP BY working_unit_id";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    foreach (DataRow row in table.Rows) {
                        employee = new EmployeeTO();
                        if (row["working_unit_id"] != DBNull.Value) {
                            employee.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        // put num in addres_id property
                        if (row["num"] != DBNull.Value) {
                            employee.AddressID = Int32.Parse(row["num"].ToString().Trim());
                        }

                        employeesList.Add(employee);
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Exception: " + ex.Message);
            }

            return employeesList;
        }

        public void serialize(List<EmployeeTO> employeesTO) {
            //string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLEmployeesFile"];
            string filename = Constants.XMLDataSourceDir + Constants.XMLEmployeesFile;

            try {
                Stream stream = File.Open(filename, FileMode.Create);
                //bool isOpened = true;
                EmployeeTO[] employeeArray = (EmployeeTO[])employeesTO.ToArray();

                XmlSerializer bformatter = new XmlSerializer(typeof(EmployeeTO[]));
                bformatter.Serialize(stream, employeeArray);
                stream.Close();
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// Serialize all of the employees found in database
        /// </summary>
        public void serialize() {
            try {
                // TODO: Not implemented yet
                //string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLEmployeesFile"];
                string filename = Constants.XMLDataSourceDir + Constants.XMLEmployeesFile;
                Stream stream = File.Open(filename, FileMode.Create);
                List<EmployeeTO> employeeTOList = this.getEmployees(new EmployeeTO());

                EmployeeTO[] employeeArray = (EmployeeTO[])employeeTOList.ToArray();

                XmlSerializer bformatter = new XmlSerializer(typeof(EmployeeTO[]));
                bformatter.Serialize(stream, employeeArray);
                stream.Close();
            }
            catch (Exception ex) {
                throw ex;
            }
        }
        public List<EmployeeTO> getAllEmployees() {
            DataSet dataSet = new DataSet();
            List<EmployeeTO> list = new List<EmployeeTO>();
            try {
                string select = "SELECT e.* FROM employees e ";
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    foreach (DataRow row in table.Rows) {
                        EmployeeTO employee = new EmployeeTO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["first_name"] != DBNull.Value) {
                            employee.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value) {
                            employee.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["address_id"] != DBNull.Value) {
                            employee.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        employee.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        list.Add(employee);
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Exception: " + ex.Message);
            }

            return list;
        }

        public List<int> getEmployeesIDs() {
            DataSet dataSet = new DataSet();
            List<int> list = new List<int>();

            try {
                string select = "SELECT DISTINCT employee_id FROM employees WHERE status <> '" + Constants.statusRetired.Trim() + "'";
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    foreach (DataRow row in table.Rows) {
                        list.Add(Int32.Parse(row["employee_id"].ToString().Trim()));
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Exception: " + ex.Message);
            }

            return list;
        }

        public string getActiveIDsForScheduling() {
            DataSet dataSet = new DataSet();
            string ids = "";

            try {
                string select = "SELECT DISTINCT e.employee_id FROM employees e, employees_asco4 a, rules r WHERE e.employee_id = a.employee_id AND e.employee_type_id = r.employee_type_id "
                    + "AND a.integer_value_4 = r.working_unit_id AND r.rule_type = '" + Constants.RuleMCVisitsSchedulingBorderDay
                    + "' AND UPPER(e.status) <> '" + Constants.statusRetired.Trim().ToUpper() + "' AND r.rule_value <= " + DateTime.Now.Day;
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    foreach (DataRow row in table.Rows) {
                        ids += row["employee_id"].ToString().Trim() + ",";
                    }

                    if (ids.Length > 0)
                        ids = ids.Substring(0, ids.Length - 1);
                }
            }
            catch (Exception ex) {
                throw new Exception("Exception: " + ex.Message);
            }

            return ids;
        }

        public string getAddressLine3(int employeeID) {
            DataSet dataSet = new DataSet();
            List<EmployeeTO> list = new List<EmployeeTO>();
            string address = "";
            try {
                string select = "SELECT e.*, a.address_line_3 FROM employees e, addresses a WHERE e.employee_id=" + employeeID.ToString() + " AND e.address_id=a.address_id";
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];


                if (table.Rows.Count >= 1) {
                    foreach (DataRow row in table.Rows) {
                        if (row["address_line_3"] != DBNull.Value) {
                            address = row["address_line_3"].ToString().Trim();
                        }
                    }
                }

            }
            catch (Exception ex) {
                throw new Exception("Exception: " + ex.Message);
            }

            return address;
        }

        public List<int> SyncDataWithNav() {
            DataSet dataSet = new DataSet();
            List<int> list = new List<int>();
            int s = 0;
            try {

                // 1.  create a command object identifying
                //     the stored procedure
                SqlCommand cmd = new SqlCommand("test", conn);

                // 2. set the command object so it knows
                //    to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;


                // execute the command
                s = Convert.ToInt32(cmd.ExecuteScalar());
                list.Add(s);
            }
            catch (Exception ex) {
                throw new Exception("Exception: " + ex.Message);
            }

            return list;
        }

        public List<EmployeeTO> listaRadnikaZaWU(int wuID)
        {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> emplList = new List<EmployeeTO>();
            string select = "SELECT e.* FROM actamgr.employees e, actamgr.working_units wu WHERE e.working_unit_id=wu.working_unit_id and e.status='ACTIVE' and e.working_unit_id="+wuID;
            try
            {
                SqlCommand cmd = new SqlCommand(select, conn);
                cmd.CommandTimeout = 1800;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                sqlDataAdapter.Fill(dataSet, "Employees");
                DataTable table = dataSet.Tables["Employees"];
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        employee = new EmployeeTO();
                        employee.EmployeeID = int.Parse(row["employee_id"].ToString().Trim());
                        if (row["first_name"] != DBNull.Value)
                        {
                            employee.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            employee.LastName = row["first_name"].ToString().Trim();
                        }
                        if (row["working_unit_id"] != DBNull.Value)
                        {
                            employee.WorkingUnitID = int.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["organizational_unit_id"] != DBNull.Value)
                        {
                            employee.OrgUnitID = int.Parse(row["organizational_unit_id"].ToString().Trim());
                        }
                        if (row["status"] != DBNull.Value)
                        {
                            employee.Status = row["status"].ToString();
                        }
                        if (row["password"] != DBNull.Value)
                        {
                            employee.Password = row["password"].ToString();
                        }
                        if (row["address_id"] != DBNull.Value)
                        {
                            employee.AddressID = int.Parse(row["address_id"].ToString());
                        }
                        if (row["employee_group_id"] != DBNull.Value)
                        {
                            employee.EmployeeTypeID = int.Parse(row["employee_group_id"].ToString());
                        }
                        if (row["access_group_id"] != DBNull.Value)
                        {
                            employee.AccessGroupID = int.Parse(row["access_group_id"].ToString());
                        }
                        if (row["type"] != DBNull.Value)
                        {
                            employee.Type = row["type"].ToString();
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            employee.CreatedBy = row["created_by"].ToString();
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            employee.ModifiedBy = row["modified_by"].ToString();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            employee.CreatedTime = DateTime.Parse(row["created_time"].ToString());
                        }
                        emplList.Add(employee);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in MSSQLEmployeeDAO.listaRadnikaZaWU() : " + ex.Message);
            }
            return emplList;
        }

        public List<EmployeeTO> listaRadnikaZaWU(string name)
        {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> emplList = new List<EmployeeTO>();
            string select = "SELECT e.* FROM actamgr.employees e, actamgr.working_units wu, actamgr.employees_asco4 ea WHERE e.working_unit_id=wu.working_unit_id and e.employee_id=ea.employee_id and ea.datetime_value_3 is null and e.status='ACTIVE' and e.working_unit_id in (select working_unit_id from actamgr.working_units where name in (" + name + "))";
            try
            {
                SqlCommand cmd = new SqlCommand(select, conn);
                cmd.CommandTimeout = 1800;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                sqlDataAdapter.Fill(dataSet, "Employees");
                DataTable table = dataSet.Tables["Employees"];
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        employee = new EmployeeTO();
                        employee.EmployeeID = int.Parse(row["employee_id"].ToString().Trim());
                        if (row["first_name"] != DBNull.Value)
                        {
                            employee.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            employee.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["working_unit_id"] != DBNull.Value)
                        {
                            employee.WorkingUnitID = int.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["organizational_unit_id"] != DBNull.Value)
                        {
                            employee.OrgUnitID = int.Parse(row["organizational_unit_id"].ToString().Trim());
                        }
                        if (row["status"] != DBNull.Value)
                        {
                            employee.Status = row["status"].ToString();
                        }
                        if (row["password"] != DBNull.Value)
                        {
                            employee.Password = row["password"].ToString();
                        }
                        if (row["address_id"] != DBNull.Value)
                        {
                            employee.AddressID = int.Parse(row["address_id"].ToString());
                        }
                        if (row["employee_group_id"] != DBNull.Value)
                        {
                            employee.EmployeeTypeID = int.Parse(row["employee_group_id"].ToString());
                        }
                        if (row["access_group_id"] != DBNull.Value)
                        {
                            employee.AccessGroupID = int.Parse(row["access_group_id"].ToString());
                        }
                        if (row["type"] != DBNull.Value)
                        {
                            employee.Type = row["type"].ToString();
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            employee.CreatedBy = row["created_by"].ToString();
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            employee.ModifiedBy = row["modified_by"].ToString();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            employee.CreatedTime = DateTime.Parse(row["created_time"].ToString());
                        }
                        emplList.Add(employee);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in MSSQLEmployeeDAO.listaRadnikaZaWU() : " + ex.Message);
            }
            return emplList;
        }

        public List<EmployeeTO> ProlasciZaForKast(DateTime dateTime) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> employeesList = new List<EmployeeTO>();
            string select = "";

            try {
                select += "DECLARE @tmpTable TABLE (employee_id int,eName nvarchar(300),eType nvarchar(300),wName nvarchar(300),oName nvarchar(300),pass_type int,Vreme int)"+Environment.NewLine;

                select += "INSERT INTO @tmpTable select e.employee_id, trim(first_name) + ' ' + trim(last_name) as eName, et.employee_type_name as eType,w.name as wName, o.name as oName, pass_type_id as pass_type, CASE WHEN CAST (end_time as time)='23:59' then SUM (datediff(minute,start_time,dateadd(minute,1,end_time))) else sum(datediff(minute,start_time,end_time)) end as minutes ";
                select += "from actamgr.employees e, actamgr.io_pairs_processed ipp, actamgr.organizational_units o, actamgr.working_units w, actamgr.employee_types et, actamgr.employees_asco4 ea ";
                select += "where e.employee_id=ipp.employee_id and o.organizational_unit_id=e.organizational_unit_id and w.working_unit_id=e.working_unit_id and e.employee_type_id=et.employee_type_id and ea.employee_id=e.employee_id and io_pair_date>='" + dateTime.ToString("yyyy-MM-ddT00:00:00") + "' and io_pair_date<='" + dateTime.AddMonths(1).ToString("yyyy-MM-ddT00:00:00") + "' and start_time>='" + dateTime.AddHours(4).ToString("yyyy-MM-ddTHH:mm:ss") + "' and end_time<='" + dateTime.AddMonths(1).AddHours(8).ToString("yyyy-MM-ddTHH:mm:ss") + "' and (ea.datetime_value_3 is null or ea.datetime_value_3 between '" + dateTime.ToString("yyyy-MM-ddTHH:mm:ss") + "' and '" + dateTime.AddMonths(1).ToString("yyyy-MM-ddTHH:mm:ss") + "') group by e.employee_id, first_name, last_name, et.employee_type_name, o.name,w.name, pass_type_id,end_time order by e.employee_id,pass_type_id" + Environment.NewLine;

                select += "select employee_id,eName,eType,wName,oName,pass_type,sum(Vreme) as minutes from @tmpTable group by employee_id,eName,eType,wName,oName,pass_type";

                SqlCommand cmd = new SqlCommand(select, conn);
                cmd.CommandTimeout = 1800;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    foreach (DataRow row in table.Rows) {
                        employee = new EmployeeTO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["eName"] != DBNull.Value) {
                            employee.FirstName = row["eName"].ToString().Split(' ')[0];
                            employee.LastName = row["eName"].ToString().Split(' ')[1];
                        }
                        //Radna
                        if (row["wName"] != DBNull.Value) {
                            employee.WorkingUnitName = row["wName"].ToString().Trim();
                        }
                        //Organizaciona
                        if (row["oName"] != DBNull.Value) {
                            employee.OrgUnitName = row["oName"].ToString().Trim();
                        }
                        //Divizija
                        if (row["eType"] != DBNull.Value)
                        {
                            employee.CreatedBy = row["eType"].ToString().Trim();
                        }
                        //Tip prolaska
                        if (row["pass_type"] != DBNull.Value) {
                            employee.WorkingUnitID = Int32.Parse(row["pass_type"].ToString().Trim());
                        }
                        //Suma trajanja prolaska
                        if (row["minutes"] != DBNull.Value) {
                            employee.OrgUnitID = Int32.Parse(row["minutes"].ToString().Trim());
                        }
                        employeesList.Add(employee);
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Exception in MSSQLEmployeeDAO.ProlasciZaForKast() : " + ex.Message);
            }

            return employeesList;
        }

        public List<EmployeeTO> ForKastProlasci(DateTime dateTime) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> employeesList = new List<EmployeeTO>();
            string select = "";

            try {
                //DEKLARACIJA PROMENLJIVIH I TEMP TABELE
                select += " DECLARE @d date ";
                //BILO JE " set @d= CAST(left(GETDATE()), 11) as datetime) ", DODAO SAM JEDAN DAN NA DANASNJI JER JE UBACIVAO ZA DANASNJI DAN DVA PUTA 8H
                select += " set @d= CAST(left(dateadd(day,1,GETDATE()), 11) as datetime) ";
                select += " DECLARE @tmpTable TABLE ( employee_id           int,	pass_type	int,	Vreme	int )   ";
                //PETLJA
                select += " WHILE @d<='" + dateTime.ToString(dateTimeformat) + "' ";
                select += " BEGIN ";
                select += " INSERT INTO @tmpTable ";
                //SELECT
                select += " select  e.employee_id  ";
                select += " ,CASE  ";
                select += " WHEN (CAST(end_time as time)='23:59' or CAST(start_time as time)='00:00') ";
                select += " 	THEN 6 ";
                select += " 	ELSE 1 ";
                select += " 	END AS pass_type ";
                select += "  ,CASE ";
                select += "   WHEN CAST(end_time as time)='23:59' ";
                select += " 	THEN	SUM(DATEDIFF(MINUTE,[start_time],DATEADD(MINUTE,1,[end_time])))  ";
                select += " 	ELSE	SUM(DATEDIFF(MINUTE,[start_time],[end_time]))  ";
                select += " 	END AS Vreme  ";
                //FROM
                select += " from employees_time_schedule e ,[time_schema_dtl] t , employees em";
                //USLOVI
                select += " where  e.employee_id = em.employee_id";
                // select += " AND	 e.date=(Select DISTINCT MAX(date) from employees_time_schedule where employee_id=e.employee_id ) ";
                //PROVERITI, JER JE VUKAO SATNICE ZA DAN KADA NIJE BILA DODELJENA TA SATNICA, DODAO SAM [date]<=@d DA BI PROVERIO TRENUTNI @d SA DATUMOM KAD JE KRENULA PRIMENA SATNICE
                select += " AND  e.date=(Select  top 1 [date] from employees_time_schedule where employee_id=e.employee_id and [date] < '" + dateTime.ToString(dateTimeformat) + "' and [date]<=@d order by [date] desc ) ";

                select += " and t.time_schema_id= (Select time_schema_id from employees_time_schedule where employee_id=e.employee_id and date=e.date) ";
                select += " and	 e.employee_id not in(select employee_id from io_pairs_processed where io_pair_date=@d and CAST(t.end_time as time)=CAST(end_time as time)  ";
                select += " and CAST(t.start_time as time)=CAST(start_time as time) and (pass_type_id !=-100 or end_time<=GETDATE() ))  ";
                select += " and t.cycle_day=(DATEDIFF(DAY,(";
                //select += " (Select DISTINCT MAX(date) from employees_time_schedule where employee_id=e.employee_id and time_schema_id=t.time_schema_id)";
                select += " (Select  top 1 [date] from employees_time_schedule where employee_id=e.employee_id and [date] < '" + dateTime.ToString(dateTimeformat) + "' order by [date] desc ) ";

                select += " ),@d) ";
                select += "  +  e.start_cycle_day) % 	((select DISTINCT MAX(cycle_day) FROM [time_schema_dtl] where time_schema_id=t.time_schema_id)+1) ";
                select += " and CAST(start_time as time)<= CASE ";
                select += " WHEN  @d='" + dateTime.ToString(dateTimeformat) + "' ";
                select += " THEN '00:00' ";
                select += " ELSE '23:00' ";
                select += " END ";
                //select += " and (e.employee_id<8000 or e.employee_id>=10000) ";
                select += " and DATEDIFF(MINUTE,[start_time],[end_time])>0 ";
                select += " and em.status = 'ACTIVE' ";
                //GRUPISANJE
                select += " GROUP BY e.employee_id ,t.[start_time],t.[end_time]  ";
                select += " SET @d=DATEADD(DAY,1,@d) ";
                select += " END ";
                //TEMP TABELA
                select += " select employee_id, pass_type, SUM(Vreme)AS minutes ";
                select += " from @tmpTable ";
                select += " group by employee_id, pass_type ";
                select += " order by employee_id, pass_type ";

                SqlCommand cmd = new SqlCommand(select, conn);
                cmd.CommandTimeout = 300;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    foreach (DataRow row in table.Rows) {
                        employee = new EmployeeTO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["pass_type"] != DBNull.Value) {
                            employee.WorkingUnitID = Int32.Parse(row["pass_type"].ToString().Trim());
                        }
                        if (row["minutes"] != DBNull.Value) {
                            employee.OrgUnitID = Int32.Parse(row["minutes"].ToString().Trim());
                        }
                        employeesList.Add(employee);
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Exception in MSSQLEmployeeDAO.ForKastProlasci() : " + ex.Message);
            }

            return employeesList;
        }

        //  10.06.2019. BOJAN za izvestaj za godisnje odmore zaposlenih
        public List<EmployeeTO> getNumberOfDaysVacationPerEmployees(DateTime date) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> employeesList = new List<EmployeeTO>();
            string select;

            try {
                select = " DECLARE @prolasci TABLE (empl_id int not null,Ime varchar(100),[Organizaciona jedinica] varchar (100),[Radna jedinica] varchar(100), [Divizija] varchar(100), [Godisnji 2019] int ,[Godisnji 2018] int,[Godisnji iskorisceno] int) ";
                select += " INSERT INTO @prolasci SELECT empl.employee_id ,empl.first_name+' '+empl.last_name,org.name,work.name, ";
                select += " CASE ";
                select += " WHEN (empl.organizational_unit_id IN (SELECT organizational_unit_id FROM organizational_units WHERE parent_organizational_unit_id in (SELECT organizational_unit_id FROM organizational_units WHERE parent_organizational_unit_id IN (SELECT organizational_unit_id FROM organizational_units WHERE parent_organizational_unit_id IN (SELECT organizational_unit_id FROM organizational_units WHERE organizational_unit_id IN (11,28)) OR organizational_unit_id IN (11,28)) OR organizational_unit_id IN (11,28)) OR organizational_unit_id in (11,28)) OR empl.organizational_unit_id IN (11,28)) THEN 'FUEL' ";
                select += " WHEN (empl.organizational_unit_id IN (SELECT organizational_unit_id FROM organizational_units WHERE parent_organizational_unit_id IN (SELECT organizational_unit_id FROM organizational_units WHERE parent_organizational_unit_id IN (SELECT organizational_unit_id FROM organizational_units WHERE parent_organizational_unit_id IN (SELECT organizational_unit_id FROM organizational_units WHERE organizational_unit_id IN (10,48)) OR organizational_unit_id IN (10,48)) OR organizational_unit_id in (10,48)) OR organizational_unit_id in (10,48)) OR empl.organizational_unit_id IN (10,48)) THEN 'WATER' "; //10 je WATER
                select += " ELSE NULL ";
                select += " END ";
                select += " , 0, 0, 0 FROM employees empl JOIN organizational_units org ON empl.organizational_unit_id = org.organizational_unit_id JOIN working_units work ON empl.working_unit_id = work.working_unit_id WHERE empl.status = 'ACTIVE' ";
                select += " UPDATE @prolasci SET [Godisnji 2019] = (SELECT co.value FROM employee_counter_values co WHERE co.employee_counter_type_id = 1 AND co.employee_id = empl_id) ";
                select += " UPDATE @prolasci SET [Godisnji 2018] = (SELECT co.value FROM employee_counter_values co WHERE co.employee_counter_type_id = 2 AND co.employee_id = empl_id) ";
                select += " UPDATE @prolasci SET [Godisnji iskorisceno] = ";
                select += " CASE ";
                select += " WHEN empl_id IN (SELECT co.employee_id FROM employee_counter_values co JOIN io_pairs_processed iop on co.employee_id = iop.employee_id WHERE co.employee_counter_type_id = 3 AND iop.io_pair_date > '" + date.ToString() + "' AND iop.pass_type_id = 8 AND CAST(start_time as time) != '22:00:00.000' GROUP BY co.employee_id,co.value) ";
                select += " THEN (SELECT co.value - COUNT (iop.pass_type_id) AS broj FROM employee_counter_values co JOIN io_pairs_processed iop ON co.employee_id = iop.employee_id WHERE co.employee_counter_type_id = 3 AND iop.io_pair_date > '" + date.ToString() + "' AND iop.pass_type_id = 8 AND CAST(start_time AS time) != '22:00:00.000' AND empl_id = co.employee_id GROUP BY co.employee_id, co.value) ";
                select += " ELSE (SELECT co.value FROM employee_counter_values co WHERE co.employee_counter_type_id = 3 AND co.employee_id = empl_id) ";
                select += " END ";
                select += " SELECT * FROM @prolasci ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    foreach (DataRow row in table.Rows) {
                        employee = new EmployeeTO();
                        employee.EmployeeID = Int32.Parse(row["empl_id"].ToString().Trim());
                        if (row["Ime"] != DBNull.Value) {
                            employee.FirstName = row["Ime"].ToString().Trim();
                        }
                        if (row["Organizaciona jedinica"] != DBNull.Value) {
                            employee.OrgUnitName = row["Organizaciona jedinica"].ToString().Trim();
                        }
                        if (row["Radna jedinica"] != DBNull.Value) {
                            employee.WorkingUnitName = row["Radna jedinica"].ToString().Trim();
                        }
                        if (row["Divizija"] != DBNull.Value) {
                            employee.Division = row["Divizija"].ToString().Trim();
                        }
                        if (row["Godisnji 2019"] != DBNull.Value) {
                            employee.VacationThisYear = Int32.Parse(row["Godisnji 2019"].ToString().Trim());
                        }
                        if (row["Godisnji 2018"] != DBNull.Value) {
                            employee.VacationLastYear = Int32.Parse(row["Godisnji 2018"].ToString().Trim());
                        }
                        if (row["Godisnji iskorisceno"] != DBNull.Value) {
                            employee.VacationUsed = Int32.Parse(row["Godisnji iskorisceno"].ToString().Trim());
                        }

                        employeesList.Add(employee);
                    }
                }
            }
            catch (Exception ex) {
                throw ex;
            }

            return employeesList;
        }

        public int BrZaposlenih(DateTime mesec) {
            int i = 0;
            string select = "";
            try {
                select += " SELECT Count(e.employee_id) FROM actamgr.employees e, actamgr.employees_asco4 a ";
                select += "Where e.employee_id=a.employee_id and (a.datetime_value_3 is null or a.datetime_value_3 between '"+mesec.ToString("yyyy-MM-dd HH:mm:ss")+"' and '"+mesec.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss")+"')";
                //select += " and (e.status='ACTIVE' OR a.datetime_value_3 is null or a.datetime_value_3 between '"+mesec+"' and '"+mesec.AddMonths(1).AddDays(-1)+"')";
                SqlCommand cmd = new SqlCommand(select, conn);
                cmd.CommandTimeout = 300;
                i = Int32.Parse(cmd.ExecuteScalar().ToString());
            }
            catch (Exception ex) {
                throw ex;
            }
            return i;

        }

        public int BrZaposlenih(DateTime fromTime, DateTime toTime)
        {
            int i = 0;
            string select = "";
            try
            {
                select += " SELECT Count(e.employee_id) FROM actamgr.employees e, actamgr.employees_asco4 a ";
                select += "Where e.employee_id=a.employee_id and (a.datetime_value_3 is null or a.datetime_value_3 between '" + fromTime.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + toTime.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                //select += " and (e.status='ACTIVE' OR a.datetime_value_3 is null or a.datetime_value_3 between '"+mesec+"' and '"+mesec.AddMonths(1).AddDays(-1)+"')";
                SqlCommand cmd = new SqlCommand(select, conn);
                cmd.CommandTimeout = 300;
                i = Int32.Parse(cmd.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return i;

        }

        public List<int> getEmplIDsForBankHours(DateTime mesec)
        {
            List<int> list = new List<int>();

            try
            {
                string select = "SELECT e.employee_id FROM actamgr.employees e";
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet, "EmplIDs");
                DataTable table = dataSet.Tables["EmplIDs"];
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        list.Add(int.Parse(row[0].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return list;
        }

        // BOJAN 18.06.2019.
        public List<EmployeeTO> getEmployeesByOUandWU(int orgUnitID, int workingUnitID) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> employeesList = new List<EmployeeTO>();
            string select;

            try {

                select = "SELECT e.* FROM employees e, employees_asco4 ea WHERE e.organizational_unit_id IN (" + orgUnitID + ") AND e.working_unit_id IN (" + workingUnitID + ") "
                    + "AND e.employee_id = ea.employee_id ";

                select += "ORDER BY e.last_name, e.first_name";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0) {
                    foreach (DataRow row in table.Rows) {
                        employee = new EmployeeTO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["first_name"] != DBNull.Value) {
                            employee.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value) {
                            employee.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["working_unit_id"] != DBNull.Value) {
                            employee.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["status"] != DBNull.Value) {
                            employee.Status = row["status"].ToString().Trim();
                        }
                        if (row["password"] != DBNull.Value) {
                            employee.Password = row["password"].ToString().Trim();
                        }
                        if (row["address_id"] != DBNull.Value) {
                            employee.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        if (row["picture"] != DBNull.Value) {
                            employee.Picture = row["picture"].ToString().Trim();
                        }
                        if (row["employee_group_id"] != DBNull.Value) {
                            employee.WorkingGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());
                        }
                        if (row["type"] != DBNull.Value) {
                            employee.Type = row["type"].ToString().Trim();
                        }
                        if (row["organizational_unit_id"] != DBNull.Value) {
                            employee.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
                        }
                        if (row["employee_type_id"] != DBNull.Value) {
                            employee.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                        }
                        employeesList.Add(employee);
                    }
                }

            }
            catch (Exception ex) {
                throw ex;
            }

            return employeesList;
        }


        public List<EmployeeTO> ProlasciZaForKast(DateTime fromTime, DateTime toTime)
        {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> employeesList = new List<EmployeeTO>();
            string select = "";

            try
            {
                DateTime time1 = fromTime.Date;
                DateTime time2 = toTime.Date.AddDays(1);
                select += "DECLARE @tmpTable TABLE (employee_id int,eName nvarchar(300),eType nvarchar(300),wName nvarchar(300),oName nvarchar(300),pass_type int,Vreme int)" + Environment.NewLine;

                select += "INSERT INTO @tmpTable select e.employee_id, trim(first_name) + ' ' + trim(last_name) as eName, et.employee_type_name as eType,w.name as wName, o.name as oName, pass_type_id as pass_type, CASE WHEN CAST (end_time as time)='23:59' then SUM (datediff(minute,start_time,dateadd(minute,1,end_time))) else sum(datediff(minute,start_time,end_time)) end as minutes ";
                select += "from actamgr.employees e, actamgr.io_pairs_processed ipp, actamgr.organizational_units o, actamgr.working_units w, actamgr.employee_types et, actamgr.employees_asco4 ea ";
                select += "where e.employee_id=ipp.employee_id and o.organizational_unit_id=e.organizational_unit_id and w.working_unit_id=e.working_unit_id and e.employee_type_id=et.employee_type_id and ea.employee_id=e.employee_id and io_pair_date>='" + time1.ToString("yyyy-MM-ddTHH:mm:ss") + "' and io_pair_date<='" + time2.ToString("yyyy-MM-ddTHH:mm:ss") + "' and start_time>='" + time1.AddHours(6).ToString("yyyy-MM-ddTHH:mm:ss") + "' and end_time<='" + time2.AddHours(6).ToString("yyyy-MM-ddTHH:mm:ss") + "' and (ea.datetime_value_3 is null or ea.datetime_value_3 between '" + time1.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + time2.ToString("yyyy-MM-dd HH:mm:ss") + "') group by e.employee_id, first_name, last_name, et.employee_type_name, o.name,w.name, pass_type_id,end_time order by e.employee_id,pass_type_id" + Environment.NewLine;

                select += "select employee_id,eName,eType,wName,oName,pass_type,sum(Vreme) as minutes from @tmpTable group by employee_id,eName,eType,wName,oName,pass_type";

                SqlCommand cmd = new SqlCommand(select, conn);
                cmd.CommandTimeout = 1800;//bilo je 300 25.07.2019
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        employee = new EmployeeTO();
                        employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["eName"] != DBNull.Value)
                        {
                            employee.FirstName = row["eName"].ToString().Split(' ')[0];
                            employee.LastName = row["eName"].ToString().Split(' ')[1];
                        }
                        //Radna
                        if (row["wName"] != DBNull.Value)
                        {
                            employee.WorkingUnitName = row["wName"].ToString().Trim();
                        }
                        //Organizaciona
                        if (row["oName"] != DBNull.Value)
                        {
                            employee.OrgUnitName = row["oName"].ToString().Trim();
                        }
                        //Divizija
                        if (row["eType"] != DBNull.Value)
                        {
                            employee.CreatedBy = row["eType"].ToString().Trim();
                        }
                        //Tip prolaska
                        if (row["pass_type"] != DBNull.Value)
                        {
                            employee.WorkingUnitID = Int32.Parse(row["pass_type"].ToString().Trim());
                        }
                        //Suma trajanja prolaska
                        if (row["minutes"] != DBNull.Value)
                        {
                            employee.OrgUnitID = Int32.Parse(row["minutes"].ToString().Trim());
                        }
                        employeesList.Add(employee);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in MSSQLEmployeeDAO.ProlasciZaForKast() : " + ex.Message);
            }

            return employeesList;
        }


        #region EmployeeDAO Members


        public List<EmployeeTO> getEmployeesByOU(string oUnits) {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            List<EmployeeTO> employeesList = new List<EmployeeTO>();
            string select;

            try {
                if (oUnits.Length > 0) {
                    select = "SELECT e.* FROM employees e, employees_asco4 ea WHERE e.organizational_unit_id IN (" + oUnits + ") "
                        + "AND e.employee_id = ea.employee_id ";

                    select += "ORDER BY e.last_name, e.first_name";

                    SqlCommand cmd = new SqlCommand(select, conn);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "Employee");
                    DataTable table = dataSet.Tables["Employee"];

                    if (table.Rows.Count > 0) {
                        foreach (DataRow row in table.Rows) {
                            employee = new EmployeeTO();
                            employee.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                            if (row["first_name"] != DBNull.Value) {
                                employee.FirstName = row["first_name"].ToString().Trim();
                            }
                            if (row["last_name"] != DBNull.Value) {
                                employee.LastName = row["last_name"].ToString().Trim();
                            }
                            if (row["working_unit_id"] != DBNull.Value) {
                                employee.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                            }
                            if (row["status"] != DBNull.Value) {
                                employee.Status = row["status"].ToString().Trim();
                            }
                            if (row["password"] != DBNull.Value) {
                                employee.Password = row["password"].ToString().Trim();
                            }
                            if (row["address_id"] != DBNull.Value) {
                                employee.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                            }
                            if (row["picture"] != DBNull.Value) {
                                employee.Picture = row["picture"].ToString().Trim();
                            }
                            if (row["employee_group_id"] != DBNull.Value) {
                                employee.WorkingGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());
                            }
                            if (row["type"] != DBNull.Value) {
                                employee.Type = row["type"].ToString().Trim();
                            }
                            if (row["organizational_unit_id"] != DBNull.Value) {
                                employee.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
                            }
                            if (row["employee_type_id"] != DBNull.Value) {
                                employee.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                            }
                            employeesList.Add(employee);
                        }
                    }
                }
            }
            catch (Exception ex) {
                throw ex;
            }

            return employeesList;
        }
        #endregion
    }
}
