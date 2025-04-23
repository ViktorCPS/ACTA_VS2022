
using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Globalization;

using TransferObjects;
using Util;

namespace DataAccess
{
    public class MySQLEmployeeHistDAO:EmployeeHistDAO
    {
        MySqlConnection conn = null;
		MySqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

		public MySqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}
		
		public MySQLEmployeeHistDAO()
		{
			conn = MySQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
			DAOController.GetInstance();
		}

        public MySQLEmployeeHistDAO(MySqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
            DAOController.GetInstance();
        }

        public void SetDBConnection(Object dbConnection)
        {
            conn = dbConnection as MySqlConnection;
        }

        public int insert(string EmployeeID, string FirstName, string LastName, string WorkingUnitID,
             string Status, string Password, string AddressID, string Picture, string WorkingGroupID,
             string type, string accessGroupID, string EmployeeTypeID,string orgUnitID, string createdBy, DateTime createdTime,DateTime validTO, bool doCommit)
        {
            MySqlTransaction sqlTrans = null;

            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }
            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employees_hist ");
                sbInsert.Append("(employee_id, first_name, last_name, working_unit_id, status, password, address_id, picture, employee_group_id, type, access_group_id,employee_type_id,organizational_unit_id, created_by, created_time,valid_to, modified_by, modified_time) ");
                sbInsert.Append("VALUES (");

                if (Int32.Parse(EmployeeID.Trim()) != -1)
                {
                    sbInsert.Append(EmployeeID + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append("N'" + FirstName.Trim() + "', N'" + LastName.Trim() + "', ");

                if (Int32.Parse(WorkingUnitID.Trim()) != -1)
                {
                    sbInsert.Append(WorkingUnitID + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append("N'" + Status.Trim() + "', N'" + Password.Trim() + "', ");

                if (Int32.Parse(AddressID.Trim()) != -1)
                {
                    sbInsert.Append(AddressID + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (!Picture.Trim().Equals(""))
                {
                    sbInsert.Append("N'" + Picture.Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (Int32.Parse(WorkingGroupID.Trim()) != -1)
                {
                    sbInsert.Append(WorkingGroupID + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append("N'" + type.Trim() + "', ");

                if (Int32.Parse(accessGroupID.Trim()) != -1)
                {
                    sbInsert.Append(accessGroupID + ", ");
                }
                else
                {
                    //can not be null
                    sbInsert.Append("0" + ", ");
                }
                if (Int32.Parse(EmployeeTypeID.Trim()) != -1)
                {
                    sbInsert.Append(EmployeeTypeID + ", ");
                }
                else
                {
                    //can not be null
                    sbInsert.Append("NULL, ");
                }
                if (Int32.Parse(orgUnitID.Trim()) != -1)
                {
                    sbInsert.Append(orgUnitID + ", ");
                }
                else
                {
                    //can not be null
                    sbInsert.Append("NULL, ");
                }
                sbInsert.Append("N'" + createdBy + "', ");
                sbInsert.Append("'" + createdTime.ToString(dateTimeformat) + "', ");
                sbInsert.Append("'" + validTO.ToString(dateTimeformat) + "', ");
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");

                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit)
                {
                    sqlTrans.Commit();
                }
                rowsAffected = 1;
            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback();
                }
                throw ex;
            }

            return rowsAffected;
        }

        public int insert(EmployeeHistTO emplHistTO, bool doCommit)
        {
            MySqlTransaction sqlTrans = null;

            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }
            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employees_hist ");
                sbInsert.Append("(employee_id, first_name, last_name, working_unit_id, status, password, address_id, picture, employee_group_id, type, access_group_id,employee_type_id, organizational_unit_id, created_by, created_time,valid_to, modified_by, modified_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("'" + emplHistTO.EmployeeID.ToString().Trim() + "', ");
                sbInsert.Append("N'" + emplHistTO.FirstName.Trim() + "', N'" + emplHistTO.LastName.Trim() + "', ");
                sbInsert.Append("'" + emplHistTO.WorkingUnitID + ", ");
                sbInsert.Append("N'" + emplHistTO.Status.Trim() + "', ");
                if (!emplHistTO.Password.Trim().Equals(""))
                    sbInsert.Append("N'" + emplHistTO.Password.Trim() + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (emplHistTO.AddressID != -1)
                    sbInsert.Append("'" + emplHistTO.AddressID.ToString().Trim() + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (!emplHistTO.Picture.Trim().Equals(""))
                    sbInsert.Append("N'" + emplHistTO.Picture.Trim() + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (emplHistTO.WorkingGroupID != -1)
                    sbInsert.Append("'" + emplHistTO.WorkingGroupID.ToString().Trim() + "', ");
                else
                    sbInsert.Append("NULL, ");
                sbInsert.Append("N'" + emplHistTO.Type.Trim() + "', ");
                sbInsert.Append("'" + emplHistTO.AccessGroupID.ToString().Trim() + "', ");
                sbInsert.Append("'" + emplHistTO.EmployeeTypeID.ToString().Trim() + "', ");
                sbInsert.Append("'" + emplHistTO.OrgUnitID.ToString().Trim() + "', ");
                if (!emplHistTO.CreatedBy.Equals(""))
                    sbInsert.Append("N'" + emplHistTO.CreatedBy.Trim() + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (!emplHistTO.CreatedTime.Equals(new DateTime()))
                    sbInsert.Append("'" + emplHistTO.CreatedTime.ToString(dateTimeformat) + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (!emplHistTO.ValidTo.Equals(new DateTime()))
                    sbInsert.Append("'" + emplHistTO.ValidTo.ToString(dateTimeformat) + "', ");
                else
                    sbInsert.Append("NULL, ");
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");

                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit)
                {
                    sqlTrans.Commit();
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback();
                }
                throw ex;
            }

            return rowsAffected;
        }

        // not sure that this is correct way getting valid employee for specific day
        // what if changes are made several times, I will get only the last one
        public EmployeeHistTO getDateEmployee(DateTime date, EmployeeTO emplTO)
        {
            DataSet dataSet = new DataSet();
            EmployeeHistTO employeeHist = new EmployeeHistTO();
            try
            {
                if (emplTO.EmployeeID == -1 || date.Equals(new DateTime()))
                    return employeeHist;

                string select = "SELECT * FROM employees_hist WHERE employee_id = '" + emplTO.EmployeeID.ToString().Trim()
                    + "' AND valid_to > '" + date.Date.ToString(dateTimeformat) + "'";

                // add conditions, difference between current and old value
                if (emplTO.WorkingUnitID != -1)
                    select += " AND working_unit_id <> '" + emplTO.WorkingUnitID.ToString().Trim() + "'";

                select += " ORDER BY modified_time DESC LIMIT 1 OFFSET 0";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeHist");
                DataTable table = dataSet.Tables["EmployeeHist"];

                if (table.Rows.Count == 1)
                {
                    employeeHist = new EmployeeHistTO();
                    employeeHist.EmployeeID = Int32.Parse(table.Rows[0]["employee_id"].ToString().Trim());
                    if (table.Rows[0]["first_name"] != DBNull.Value)
                    {
                        employeeHist.FirstName = table.Rows[0]["first_name"].ToString().Trim();
                    }
                    if (table.Rows[0]["last_name"] != DBNull.Value)
                    {
                        employeeHist.LastName = table.Rows[0]["last_name"].ToString().Trim();
                    }
                    if (table.Rows[0]["working_unit_id"] != DBNull.Value)
                    {
                        employeeHist.WorkingUnitID = Int32.Parse(table.Rows[0]["working_unit_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["status"] != DBNull.Value)
                    {
                        employeeHist.Status = table.Rows[0]["status"].ToString().Trim();
                    }
                    if (table.Rows[0]["password"] != DBNull.Value)
                    {
                        employeeHist.Password = table.Rows[0]["password"].ToString().Trim();
                    }
                    if (table.Rows[0]["address_id"] != DBNull.Value)
                    {
                        employeeHist.AddressID = Int32.Parse(table.Rows[0]["address_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["picture"] != DBNull.Value)
                    {
                        employeeHist.Picture = table.Rows[0]["picture"].ToString().Trim();
                    }
                    if (table.Rows[0]["employee_group_id"] != DBNull.Value)
                    {
                        employeeHist.WorkingGroupID = Int32.Parse(table.Rows[0]["employee_group_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["type"] != DBNull.Value)
                    {
                        employeeHist.Type = table.Rows[0]["type"].ToString().Trim();
                    }
                    if (table.Rows[0]["access_group_id"] != DBNull.Value)
                    {
                        employeeHist.AccessGroupID = Int32.Parse(table.Rows[0]["access_group_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["organizational_unit_id"] != DBNull.Value)
                    {
                        employeeHist.OrgUnitID = Int32.Parse(table.Rows[0]["organizational_unit_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["employee_type_id"] != DBNull.Value)
                    {
                        employeeHist.EmployeeTypeID = Int32.Parse(table.Rows[0]["employee_type_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["created_by"] != DBNull.Value)
                    {
                        employeeHist.CreatedBy = table.Rows[0]["created_by"].ToString().Trim();
                    }
                    if (table.Rows[0]["created_time"] != DBNull.Value)
                    {
                        employeeHist.CreatedTime = DateTime.Parse(table.Rows[0]["created_time"].ToString().Trim());
                    }
                    if (table.Rows[0]["modified_by"] != DBNull.Value)
                    {
                        employeeHist.ModifiedBy = table.Rows[0]["modified_by"].ToString().Trim();
                    }
                    if (table.Rows[0]["modified_time"] != DBNull.Value)
                    {
                        employeeHist.ModifiedTime = DateTime.Parse(table.Rows[0]["modified_time"].ToString().Trim());
                    }
                    if (table.Rows[0]["valid_to"] != DBNull.Value)
                    {
                        employeeHist.ValidTo = DateTime.Parse(table.Rows[0]["valid_to"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return employeeHist;
        }

        public DateTime getRetiredDate(int emplID)
        {
            DataSet dataSet = new DataSet();
            DateTime retiredDate = new DateTime();
            try
            {
                if (emplID == -1)
                    return retiredDate;

                string select = "SELECT valid_to FROM employees_hist WHERE employee_id = '" + emplID.ToString().Trim() + "' AND UPPER(status) = '" 
                    + Constants.statusActive.Trim().ToUpper() + "' ORDER BY modified_time DESC LIMIT 1 OFFSET 0";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeHist");
                DataTable table = dataSet.Tables["EmployeeHist"];

                if (table.Rows.Count == 1)
                {                    
                    if (table.Rows[0]["valid_to"] != DBNull.Value)
                    {
                        retiredDate = DateTime.Parse(table.Rows[0]["valid_to"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retiredDate;
        }

        public Dictionary<int, List<EmployeeHistTO>> getEmployeeChanges(DateTime from, DateTime to, string emplIDs)
        {
            DataSet dataSet = new DataSet();
            Dictionary<int, List<EmployeeHistTO>> dict = new Dictionary<int, List<EmployeeHistTO>>();
            EmployeeHistTO employeeHist = new EmployeeHistTO();
            try
            {
                string select = "SELECT * FROM employees_hist";

                if (from != new DateTime() || to != new DateTime() || emplIDs.Length > 0)
                {
                    select += " WHERE ";
                    if (from != new DateTime())
                        select += "valid_to >= '" + from.Date.ToString(dateTimeformat) + "' AND ";
                    if (to != new DateTime())
                        select += "valid_to < '" + to.Date.AddDays(1).ToString(dateTimeformat) + "' AND ";
                    if (emplIDs.Length > 0)
                        select += "employee_id IN (" + emplIDs.Trim() + ") AND ";

                    select = select.Substring(0, select.Length - 4);
                }

                select += " ORDER BY modified_time DESC";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeHist");
                DataTable table = dataSet.Tables["EmployeeHist"];

                foreach (DataRow row in table.Rows)
                {
                    employeeHist = new EmployeeHistTO();
                    employeeHist.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                    if (row["first_name"] != DBNull.Value)
                    {
                        employeeHist.FirstName = row["first_name"].ToString().Trim();
                    }
                    if (row["last_name"] != DBNull.Value)
                    {
                        employeeHist.LastName = row["last_name"].ToString().Trim();
                    }
                    if (row["working_unit_id"] != DBNull.Value)
                    {
                        employeeHist.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                    }
                    if (row["status"] != DBNull.Value)
                    {
                        employeeHist.Status = row["status"].ToString().Trim();
                    }
                    if (row["password"] != DBNull.Value)
                    {
                        employeeHist.Password = row["password"].ToString().Trim();
                    }
                    if (row["address_id"] != DBNull.Value)
                    {
                        employeeHist.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                    }
                    if (row["picture"] != DBNull.Value)
                    {
                        employeeHist.Picture = row["picture"].ToString().Trim();
                    }
                    if (row["employee_group_id"] != DBNull.Value)
                    {
                        employeeHist.WorkingGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());
                    }
                    if (row["type"] != DBNull.Value)
                    {
                        employeeHist.Type = row["type"].ToString().Trim();
                    }
                    if (row["access_group_id"] != DBNull.Value)
                    {
                        employeeHist.AccessGroupID = Int32.Parse(row["access_group_id"].ToString().Trim());
                    }
                    if (row["organizational_unit_id"] != DBNull.Value)
                    {
                        employeeHist.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
                    }
                    if (row["employee_type_id"] != DBNull.Value)
                    {
                        employeeHist.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                    }
                    if (row["created_by"] != DBNull.Value)
                    {
                        employeeHist.CreatedBy = row["created_by"].ToString().Trim();
                    }
                    if (row["created_time"] != DBNull.Value)
                    {
                        employeeHist.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                    }
                    if (row["modified_by"] != DBNull.Value)
                    {
                        employeeHist.ModifiedBy = row["modified_by"].ToString().Trim();
                    }
                    if (row["modified_time"] != DBNull.Value)
                    {
                        employeeHist.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                    }
                    if (row["valid_to"] != DBNull.Value)
                    {
                        employeeHist.ValidTo = DateTime.Parse(row["valid_to"].ToString().Trim());
                    }

                    if (!dict.ContainsKey(employeeHist.EmployeeID))
                        dict.Add(employeeHist.EmployeeID, new List<EmployeeHistTO>());

                    dict[employeeHist.EmployeeID].Add(employeeHist);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dict;
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
