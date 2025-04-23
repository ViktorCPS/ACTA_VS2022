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
    public class MSSQLOrganizationalUnitDAO : OrganizationalUnitDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLOrganizationalUnitDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public MSSQLOrganizationalUnitDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(OrganizationalUnitTO ouTO, bool doCommit)
        {
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO organizational_units ");
                sbInsert.Append("(organizational_unit_id, parent_organizational_unit_id, description, name, address_id, status, code, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("'" + ouTO.OrgUnitID.ToString().Trim() + "', ");
                sbInsert.Append("'" + ouTO.ParentOrgUnitID.ToString().Trim() + "', ");
                if (ouTO.Desc.Trim().Equals(""))
                    sbInsert.Append("null, ");
                else
                    sbInsert.Append("N'" + ouTO.Desc.Trim() + "', ");
                if (ouTO.Name.Trim().Equals(""))
                    sbInsert.Append("null, ");
                else
                    sbInsert.Append("N'" + ouTO.Name.Trim() + "', ");
                if (ouTO.AddressID == -1)
                    sbInsert.Append("null, ");
                else
                    sbInsert.Append("'" + ouTO.AddressID.ToString().Trim() + "', ");
                if (ouTO.Status.Trim().Equals(""))
                    sbInsert.Append("null, ");
                else
                    sbInsert.Append("N'" + ouTO.Status.Trim() + "', ");
                sbInsert.Append("N'" + ouTO.Code.Trim() + "', ");
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

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

        public bool delete(int ouID)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM organizational_units WHERE organizational_unit_id = '" + ouID.ToString().Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                isDeleted = true;
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("DELETE");
                throw ex;
            }

            return isDeleted;
        }

        public bool delete(int ouID, bool doCommit)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = null;
            if (doCommit)
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");
            else
                sqlTrans = SqlTrans;

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM organizational_units WHERE organizational_unit_id = '" + ouID.ToString().Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                isDeleted = true;
                if (doCommit)
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                sqlTrans.Rollback("DELETE");
                throw ex;
            }

            return isDeleted;
        }


        public int findMAXOUID()
        {
            DataSet dataSet = new DataSet();
            int ouID = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT MAX(organizational_unit_id) AS ou_id FROM organizational_units", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "WUnits");
                DataTable table = dataSet.Tables["WUnits"];

                if (table.Rows.Count == 1 && !table.Rows[0]["ou_id"].Equals(DBNull.Value))
                {
                    ouID = Int32.Parse(table.Rows[0]["ou_id"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return ouID;
        }

        public bool update(OrganizationalUnitTO ouTO, bool doCommit)
        {
            bool isUpdated = false;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE organizational_units SET ");
                if (ouTO.ParentOrgUnitID != -1)
                    sbUpdate.Append("parent_organizational_unit_id = '" + ouTO.ParentOrgUnitID.ToString().Trim() + "', ");
                if (!ouTO.Name.Trim().Equals(""))
                    sbUpdate.Append("name = N'" + ouTO.Name.Trim() + "', ");
                else
                    sbUpdate.Append("name = null, ");
                if (!ouTO.Desc.Trim().Equals(""))
                    sbUpdate.Append("description = N'" + ouTO.Desc.Trim() + "', ");
                else
                    sbUpdate.Append("description = null, ");
                if (ouTO.AddressID != -1)
                    sbUpdate.Append("address_id = '" + ouTO.AddressID.ToString().Trim() + "', ");
                else
                    sbUpdate.Append("address_id = null, ");
                if (!ouTO.Status.Trim().Equals(""))
                    sbUpdate.Append("status = N'" + ouTO.Status.Trim() + "', ");
                else
                    sbUpdate.Append("status = null, ");
                if (!ouTO.Code.Trim().Equals(""))
                    sbUpdate.Append("code = N'" + ouTO.Code.Trim() + "', ");
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE organizational_unit_id = '" + ouTO.OrgUnitID.ToString().Trim() + "'");

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
                throw ex;
            }

            return isUpdated;
        }

        public OrganizationalUnitTO find(int ouID)
        {
            DataSet dataSet = new DataSet();
            OrganizationalUnitTO ouTO = new OrganizationalUnitTO();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM organizational_units WHERE organizational_unit_id = '" + ouID.ToString().Trim() + "'", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "OUnits");
                DataTable table = dataSet.Tables["OUnits"];

                if (table.Rows.Count == 1)
                {
                    ouTO = new OrganizationalUnitTO();
                    ouTO.OrgUnitID = Int32.Parse(table.Rows[0]["organizational_unit_id"].ToString().Trim());

                    if (!table.Rows[0]["parent_organizational_unit_id"].Equals(DBNull.Value))
                    {
                        ouTO.ParentOrgUnitID = Int32.Parse(table.Rows[0]["parent_organizational_unit_id"].ToString().Trim());
                    }
                    if (!table.Rows[0]["name"].Equals(DBNull.Value))
                    {
                        ouTO.Name = table.Rows[0]["name"].ToString().Trim();
                    }
                    if (!table.Rows[0]["description"].Equals(DBNull.Value))
                    {
                        ouTO.Desc = table.Rows[0]["description"].ToString().Trim();
                    }
                    if (!table.Rows[0]["address_id"].Equals(DBNull.Value))
                    {
                        ouTO.AddressID = Int32.Parse(table.Rows[0]["address_id"].ToString().Trim());
                    }
                    if (!table.Rows[0]["status"].Equals(DBNull.Value))
                    {
                        ouTO.Status = table.Rows[0]["status"].ToString().Trim();
                    }
                    if (!table.Rows[0]["code"].Equals(DBNull.Value))
                    {
                        ouTO.Code = table.Rows[0]["code"].ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ouTO;
        }
        public List<OrganizationalUnitTO> getChildOU(string parentID)
        {
            DataSet dataSet = new DataSet();
            OrganizationalUnitTO oUnit = new OrganizationalUnitTO();
            List<OrganizationalUnitTO> oUnitList = new List<OrganizationalUnitTO>();

            try
            {
                string select = "SELECT * FROM organizational_units WHERE parent_organizational_unit_id = '" + parentID.Trim() +
                    "' AND organizational_unit_id <> parent_organizational_unit_id AND status = 'ACTIVE' ORDER BY name";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "OUnits");
                DataTable table = dataSet.Tables["OUnits"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        oUnit = new OrganizationalUnitTO();
                        oUnit.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());

                        if (!row["parent_organizational_unit_id"].Equals(DBNull.Value))
                        {
                            oUnit.ParentOrgUnitID = Int32.Parse(row["parent_organizational_unit_id"].ToString().Trim());
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            oUnit.Desc = row["description"].ToString().Trim();
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            oUnit.Name = row["name"].ToString().Trim();
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            oUnit.Status = row["status"].ToString().Trim();
                        }
                        if (!row["address_id"].Equals(DBNull.Value))
                        {
                            oUnit.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        } if (!row["code"].Equals(DBNull.Value))
                        {
                            oUnit.Code = row["code"].ToString().Trim();
                        }

                        // Add org Unit object
                        oUnitList.Add(oUnit);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return oUnitList;
        }

        public List<OrganizationalUnitTO> getOrgUnits(OrganizationalUnitTO ouTO)
        {
            DataSet dataSet = new DataSet();
            OrganizationalUnitTO orgUnitTO = new OrganizationalUnitTO();
            List<OrganizationalUnitTO> ouList = new List<OrganizationalUnitTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM organizational_units ");
                if ((ouTO.OrgUnitID != -1) || (ouTO.ParentOrgUnitID != -1) || (!ouTO.Name.Trim().Equals("")) || (!ouTO.Desc.Trim().Equals("")) || (ouTO.AddressID != -1)
                    || (!ouTO.Status.Trim().Equals("")) || (!ouTO.Code.Trim().Equals("")))
                {
                    sb.Append(" WHERE");
                    if (ouTO.OrgUnitID != -1)
                    {
                        sb.Append(" organizational_unit_id = '" + ouTO.OrgUnitID.ToString().Trim() + "' AND");
                    }
                    if (ouTO.ParentOrgUnitID != -1)
                    {
                        sb.Append(" parent_organizational_unit_id = '" + ouTO.ParentOrgUnitID.ToString().Trim() + "' AND");
                    }
                    if (!ouTO.Name.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(name) = N'" + ouTO.Name.ToUpper().Trim() + "' AND");
                    }
                    if (!ouTO.Desc.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(description) = N'" + ouTO.Desc.ToUpper().Trim() + "' AND");
                    }
                    if (ouTO.AddressID != -1)
                    {
                        sb.Append(" address_id = '" + ouTO.AddressID.ToString().Trim() + "' AND");
                    }
                    if (!ouTO.Status.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(status) = N'" + ouTO.Status.ToUpper().Trim() + "' AND");
                    }
                    if (!ouTO.Code.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(code) = N'" + ouTO.Code.ToUpper().Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY name ";

                SqlCommand cmd;

                if (this.SqlTrans != null)
                    cmd = new SqlCommand(select, conn, SqlTrans);
                else
                    cmd = new SqlCommand(select, conn);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "OUnits");
                DataTable table = dataSet.Tables["OUnits"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        orgUnitTO = new OrganizationalUnitTO();

                        orgUnitTO.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());

                        if (!row["parent_organizational_unit_id"].Equals(DBNull.Value))
                        {
                            orgUnitTO.ParentOrgUnitID = Int32.Parse(row["parent_organizational_unit_id"].ToString().Trim());
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            orgUnitTO.Name = row["name"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            orgUnitTO.Desc = row["description"].ToString().Trim();
                        }
                        if (!row["address_id"].Equals(DBNull.Value))
                        {
                            orgUnitTO.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            orgUnitTO.Status = row["status"].ToString().Trim();
                        }
                        if (!row["code"].Equals(DBNull.Value))
                        {
                            orgUnitTO.Code = row["code"].ToString().Trim();
                        }

                        ouList.Add(orgUnitTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ouList;
        }

        public Dictionary<int, OrganizationalUnitTO> getOrgUnitsDictionary()
        {
            DataSet dataSet = new DataSet();
            OrganizationalUnitTO orgUnitTO = new OrganizationalUnitTO();
            Dictionary<int, OrganizationalUnitTO> ouList = new Dictionary<int,OrganizationalUnitTO>();
            string select = "";

            try
            {
                select = "SELECT * FROM organizational_units ORDER BY name ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "OUnits");
                DataTable table = dataSet.Tables["OUnits"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        orgUnitTO = new OrganizationalUnitTO();

                        orgUnitTO.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());

                        if (!row["parent_organizational_unit_id"].Equals(DBNull.Value))
                        {
                            orgUnitTO.ParentOrgUnitID = Int32.Parse(row["parent_organizational_unit_id"].ToString().Trim());
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            orgUnitTO.Name = row["name"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            orgUnitTO.Desc = row["description"].ToString().Trim();
                        }
                        if (!row["address_id"].Equals(DBNull.Value))
                        {
                            orgUnitTO.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            orgUnitTO.Status = row["status"].ToString().Trim();
                        }
                        if (!row["code"].Equals(DBNull.Value))
                        {
                            orgUnitTO.Code = row["code"].ToString().Trim();
                        }

                        if (!ouList.ContainsKey(orgUnitTO.OrgUnitID))
                            ouList.Add(orgUnitTO.OrgUnitID, orgUnitTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ouList;
        }

        public DataSet getOrgUnitsDS(string oUnits)
        {
            DataSet dataSet = new DataSet();
            string select = "";
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT * FROM organizational_units WHERE name IS NOT null AND status = 'ACTIVE' ");
                if (!oUnits.Equals(""))
                {
                    sb.Append("AND organizational_unit_id IN (" + oUnits + ")");
                }
                select = sb.ToString();

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Organization units");
                DataTable table = dataSet.Tables["Organization units"];
            }
            catch (Exception ex)
            {

                throw new Exception("Exception: " + ex.Message);
            }

            return dataSet;
        }

        public List<OrganizationalUnitTO> getOrgUnits(string oUnits)
        {
            DataSet dataSet = new DataSet();
            OrganizationalUnitTO orgUnitTO = new OrganizationalUnitTO();
            List<OrganizationalUnitTO> ouList = new List<OrganizationalUnitTO>();
            string select = "";

            try
            {
                if (!oUnits.Trim().Equals(""))
                {
                    select = "SELECT * FROM organizational_units WHERE organizational_unit_id IN (" + oUnits.Trim() + ") ORDER BY name";

                    SqlCommand cmd = new SqlCommand(select, conn);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "OUnits");
                    DataTable table = dataSet.Tables["OUnits"];

                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            orgUnitTO = new OrganizationalUnitTO();

                            orgUnitTO.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());

                            if (!row["parent_organizational_unit_id"].Equals(DBNull.Value))
                            {
                                orgUnitTO.ParentOrgUnitID = Int32.Parse(row["parent_organizational_unit_id"].ToString().Trim());
                            }
                            if (!row["name"].Equals(DBNull.Value))
                            {
                                orgUnitTO.Name = row["name"].ToString().Trim();
                            }
                            if (!row["description"].Equals(DBNull.Value))
                            {
                                orgUnitTO.Desc = row["description"].ToString().Trim();
                            }
                            if (!row["address_id"].Equals(DBNull.Value))
                            {
                                orgUnitTO.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                            }
                            if (!row["status"].Equals(DBNull.Value))
                            {
                                orgUnitTO.Status = row["status"].ToString().Trim();
                            }
                            if (!row["code"].Equals(DBNull.Value))
                            {
                                orgUnitTO.Code = row["code"].ToString().Trim();
                            }

                            ouList.Add(orgUnitTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ouList;
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
                throw ex;
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
        public List<OrganizationalUnitTO> getOUByName(string name)
        {
            DataSet dataSet = new DataSet();
            string select = "select * from actamgr.organizational_units where name='" + name + "'";
            SqlCommand cmd = new SqlCommand(select, conn);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

            sqlDataAdapter.Fill(dataSet, "WorkingUnits");
            DataTable table = dataSet.Tables["WorkingUnits"];
            List<OrganizationalUnitTO> oUnitList = new List<OrganizationalUnitTO>();
            OrganizationalUnitTO ou = new OrganizationalUnitTO();
            foreach (DataRow item in table.Rows)
            {

                ou.OrgUnitID = int.Parse(item["organizational_unit_id"].ToString());
                ou.Name = item["name"].ToString();
                ou.ParentOrgUnitID = int.Parse(item["parent_organizational_unit_id"].ToString());
                ou.Desc = item["description"].ToString();
                ou.Status = item["status"].ToString();
                ou.Code = item["code"].ToString();
                //ou. = getNumberOfEmployees(ou.WorkingUnitID);
                //ou.ChildWUNumber = getNumberOfChild(ou.WorkingUnitID);
                oUnitList.Add(ou);
            }
            return oUnitList;
        }
    }
}
