using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using TransferObjects;
using Util;

namespace DataAccess
{
    /// <summary>
    /// MSSQLWorkingUnitsDAO implements MSSQL WorkingUnits Data Access Object
    /// </summary>
    public class MSSQLWorkingUnitsDAO : WorkingUnitsDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        /// <summary>
        /// Constructor, get database 
        /// connection from it's factory class
        /// </summary>
        public MSSQLWorkingUnitsDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
        }

        public MSSQLWorkingUnitsDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
        }

        /// <summary>
        /// Insert WorkingUnitTO to working_units table
        /// </summary>
        /// <param name="workingUnitID">primary key</param>
        /// <param name="parentWUID">wokring unit id of the unit where this working unit belongs to</param>
        /// <param name="description">working unit description</param>
        /// <param name="name">working unit name</param>
        /// <returns>1 if succedeed</returns>
        public int insert(WorkingUnitTO wuTO)
        {
            int rowsAffected = 0;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO working_units ");
                sbInsert.Append("(working_unit_id, parent_working_unit_id, description, name, status, address_id, created_by, created_time) ");
                sbInsert.Append("VALUES ('" + wuTO.WorkingUnitID.ToString().Trim() + "', ");
                if(wuTO.ParentWorkingUID == -1)
                    sbInsert.Append("NULL, ");
                else
                    sbInsert.Append("'" + wuTO.ParentWorkingUID.ToString().Trim() + "', ");
                sbInsert.Append("N'"+ wuTO.Description.Trim() + "', N'" + wuTO.Name.Trim() + "', N'" + wuTO.Status.Trim() + "', ");
                if(wuTO.AddressID == -1)
                    sbInsert.Append("NULL, ");
                else
                    sbInsert.Append("'" + wuTO.AddressID.ToString().Trim() + "', ");
                sbInsert.Append(" N'"+ DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("INSERT");
                throw ex;
            }
            return rowsAffected;
        }

        /// <summary>
        /// Insert WorkingUnitTO to working_units table
        /// </summary>
        /// <param name="workingUnitID">primary key</param>
        /// <param name="parentWUID">wokring unit id of the unit where this working unit belongs to</param>
        /// <param name="description">working unit description</param>
        /// <param name="name">working unit name</param>
        /// <returns>1 if succedeed</returns>
        public int insert(WorkingUnitTO wuTO, bool doCommit)
        {
            int rowsAffected = 0;
            SqlTransaction sqlTrans = null;
            if (doCommit)
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            else
                sqlTrans = SqlTrans;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO working_units ");
                sbInsert.Append("(working_unit_id, parent_working_unit_id, description, name, status,code, address_id, created_by, created_time) ");
                sbInsert.Append("VALUES ('" + wuTO.WorkingUnitID.ToString().Trim() + "', ");
                if (wuTO.ParentWorkingUID == -1)
                    sbInsert.Append("NULL, ");
                else
                    sbInsert.Append("'" + wuTO.ParentWorkingUID.ToString().Trim() + "', ");
                sbInsert.Append("N'" + wuTO.Description.Trim() + "', N'" + wuTO.Name.Trim() + "', N'" + wuTO.Status.Trim() + "', N'" + wuTO.Code.Trim() + "', ");
                if (wuTO.AddressID == -1)
                    sbInsert.Append("NULL, ");
                else
                    sbInsert.Append("'" + wuTO.AddressID.ToString().Trim() + "', ");
                sbInsert.Append(" N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();
                if (doCommit)
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                sqlTrans.Rollback("INSERT");
                throw ex;
            }
            return rowsAffected;
        }

        /// <summary>
        /// Delete working unit with given primary key
        /// from working_units table
        /// </summary>
        /// <param name="workingUnitsID">primary key</param>
        /// <returns>true if succedeed, false otherwise</returns>
        public bool delete(int workingUnitsID)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM working_units WHERE working_unit_id = " + workingUnitsID);

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                isDeleted = true;

                sqlTrans.Commit();
            }
            catch (Exception ex)
            {

                sqlTrans.Rollback("DELETE");
                throw new Exception(ex.Message);

            }

            return isDeleted;
        }

        /// <summary>
        /// Update working unit data in working_units table
        /// </summary>
        /// <param name="workingUnitID">primary key</param>
        /// <param name="parentWUID">wokring unit id of the unit where this working unit belongs to</param>
        /// <param name="description">working unit description</param>
        /// <param name="name">working unit name</param>
        public bool update(WorkingUnitTO wuTO)
        {

            bool isUpdated = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE working_units SET ");
                sbUpdate.Append("parent_working_unit_id = '" + wuTO.ParentWorkingUID.ToString().Trim() + "', ");
                sbUpdate.Append("description = N'" + wuTO.Description.Trim() + "', ");
                sbUpdate.Append("name = N'" + wuTO.Name.Trim() + "', ");
                sbUpdate.Append("status = N'" + wuTO.Status.Trim() + "', ");
                if (wuTO.AddressID != -1)
                    sbUpdate.Append("address_id = '" + wuTO.AddressID.ToString().Trim() + "', ");
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE working_unit_id = '" + wuTO.WorkingUnitID + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {

                sqlTrans.Rollback("UPDATE");
                throw new Exception(ex.Message);

            }

            return isUpdated;
        }

        /// <summary>
        /// Update working unit data in working_units table
        /// </summary>
        /// <param name="workingUnitID">primary key</param>
        /// <param name="parentWUID">wokring unit id of the unit where this working unit belongs to</param>
        /// <param name="description">working unit description</param>
        /// <param name="name">working unit name</param>
        /// <param name="doCommit">Commit transaction</param>
        public bool update(WorkingUnitTO wuTO, bool doCommit)
        {
            bool isUpdated = false;
            SqlTransaction sqlTrans = null;

            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE working_units SET ");
                sbUpdate.Append("parent_working_unit_id = '" + wuTO.ParentWorkingUID.ToString().Trim() + "', ");
                sbUpdate.Append("description = N'" + wuTO.Description.Trim() + "', ");
                sbUpdate.Append("name = N'" + wuTO.Name.Trim() + "', ");
                sbUpdate.Append("status = N'" + wuTO.Status.Trim() + "', ");
                if (wuTO.AddressID != -1)
                    sbUpdate.Append("address_id = '" + wuTO.AddressID.ToString().Trim() + "', ");
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE working_unit_id = '" + wuTO.WorkingUnitID + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }
                if (doCommit)
                {
                    sqlTrans.Commit();
                }
            }
            catch (Exception ex)
            {

                sqlTrans.Rollback();
                throw new Exception(ex.Message);

            }

            return isUpdated;
        }

        /// <summary>
        /// Find and return Working Unit object 
        /// with given key in working_units table
        /// </summary>
        /// <param name="workingUnitID">primary key</param>
        /// <returns>WorkingUnitTO object if succedeed</returns>
        public WorkingUnitTO find(int workingUnitID)
        {
            DataSet dataSet = new DataSet();
            WorkingUnitTO wUnit = new WorkingUnitTO();

            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM working_units WHERE working_unit_id = " + workingUnitID + " ", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "WorkingUnit");
                DataTable table = dataSet.Tables["WorkingUnit"];

                if (table.Rows.Count == 1)
                {
                    wUnit.WorkingUnitID = Int32.Parse(table.Rows[0]["working_unit_id"].ToString().Trim());

                    if (!table.Rows[0]["parent_working_unit_id"].Equals(DBNull.Value))
                    {
                        wUnit.ParentWorkingUID = Int32.Parse(table.Rows[0]["parent_working_unit_id"].ToString().Trim());
                    }
                    if (!table.Rows[0]["description"].Equals(DBNull.Value))
                    {
                        wUnit.Description = table.Rows[0]["description"].ToString().Trim();
                    }
                    if (!table.Rows[0]["name"].Equals(DBNull.Value))
                    {
                        wUnit.Name = table.Rows[0]["name"].ToString().Trim();
                    }
                    if (!table.Rows[0]["status"].Equals(DBNull.Value))
                    {
                        wUnit.Status = table.Rows[0]["status"].ToString().Trim();
                    }
                    if (!table.Rows[0]["address_id"].Equals(DBNull.Value))
                    {
                        wUnit.AddressID = Int32.Parse(table.Rows[0]["address_id"].ToString().Trim());
                    }
                    if (!table.Rows[0]["code"].Equals(DBNull.Value))
                    {
                        wUnit.Code = table.Rows[0]["code"].ToString().Trim();
                    }
                }
                wUnit.EmplNumber = getNumberOfEmployees(workingUnitID);
                wUnit.ChildWUNumber = getNumberOfChild(workingUnitID);
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return wUnit;
        }

        private int getNumberOfEmployees(int workingUnitID)
        {
            DataSet dataSet = new DataSet();
            int numOfEmployees = 0;
            try
            {
                //SqlCommand cmd = new SqlCommand("SELECT COUNT(*)employees_number FROM employees WHERE working_unit_id = " + workingUnitID + " ", conn);
                
                string select  = "SELECT COUNT(*) employees_number FROM employees WHERE working_unit_id = '" + workingUnitID + "' ";
                SqlCommand cmd;
                if (SqlTrans != null)
                {
                    cmd = new SqlCommand(select, conn, SqlTrans);
                }
                else
                {
                    cmd = new SqlCommand(select, conn);
                }
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count == 1)
                {
                    numOfEmployees = Int32.Parse(table.Rows[0]["employees_number"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return numOfEmployees;
        }

        private int getNumberOfEmployees(int workingUnitID, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            int numOfEmployees = 0;
            try
            {
                //SqlCommand cmd = new SqlCommand("SELECT COUNT(*)employees_number FROM employees WHERE working_unit_id = " + workingUnitID + " ", conn);

                string select = "SELECT COUNT(*) employees_number FROM employees WHERE working_unit_id = '" + workingUnitID + "' ";
                SqlCommand cmd;
               
                cmd = new SqlCommand(select, conn, (SqlTransaction)trans);
               
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count == 1)
                {
                    numOfEmployees = Int32.Parse(table.Rows[0]["employees_number"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return numOfEmployees;
        }

        private int getNumberOfChild(int workingUnitID)
        {
            DataSet dataSet = new DataSet();
            int numOfChild = 0;
            try
            {
                //SqlCommand cmd = new SqlCommand("SELECT COUNT(*)child_number FROM working_units WHERE parent_working_unit_id = " + workingUnitID + " ", conn);
                string select = "SELECT COUNT(*) child_number FROM working_units WHERE parent_working_unit_id = " + workingUnitID + " ";
                SqlCommand cmd;
                if (SqlTrans != null)
                {
                    cmd = new SqlCommand(select, conn, SqlTrans);
                }
                else
                {
                    cmd = new SqlCommand(select, conn);
                }
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Child");
                DataTable table = dataSet.Tables["Child"];

                if (table.Rows.Count == 1)
                {
                    numOfChild = Int32.Parse(table.Rows[0]["child_number"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return numOfChild;
        }

        private int getNumberOfChild(int workingUnitID, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            int numOfChild = 0;
            try
            {
                //SqlCommand cmd = new SqlCommand("SELECT COUNT(*)child_number FROM working_units WHERE parent_working_unit_id = " + workingUnitID + " ", conn);
                string select = "SELECT COUNT(*) child_number FROM working_units WHERE parent_working_unit_id = " + workingUnitID + " ";
                SqlCommand cmd;
                
                cmd = new SqlCommand(select, conn, (SqlTransaction)trans);
                
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Child");
                DataTable table = dataSet.Tables["Child"];

                if (table.Rows.Count == 1)
                {
                    numOfChild = Int32.Parse(table.Rows[0]["child_number"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return numOfChild;
        }
        
        public DataSet getWorkingUnits(string workigUnitID)
        {
            DataSet dataSet = new DataSet();
            string select = "";
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT * FROM working_units WHERE name IS NOT null AND status = 'ACTIVE' ");
                if (!workigUnitID.Equals(""))
                {
                    sb.Append("AND working_unit_id IN (" + workigUnitID + ")");
                }
                select = sb.ToString();

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Working units");
                DataTable table = dataSet.Tables["Working units"];
            }
            catch (Exception ex)
            {

                throw new Exception("Exception: " + ex.Message);
            }

            return dataSet;
        }

        public List<WorkingUnitTO> getWorkingUnitsForOU(string orgUnitID) // Nenad 03. XI 2017.
        {
            DataSet dataSet = new DataSet();
            string select = "";
            List<WorkingUnitTO> list = new List<WorkingUnitTO>();
            WorkingUnitTO wUnit = new WorkingUnitTO();
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT distinct " +
                  "wu.[working_unit_id] " +
                  ",wu.[parent_working_unit_id] " +
                  ",wu.[description] " +
                  ",wu.[name] " +
                  ",wu.[address_id] " +
                  ",wu.[status] " +
                  ",wu.[created_by] " +
                  ",wu.[created_time] " +
                  ",wu.[modified_by] " +
                  ",wu.[modified_time] " +
                  ",wu.[code] " +
                  "FROM [working_units] wu, [employees] empl, organizational_units ou where " +
                  "empl.working_unit_id = wu.working_unit_id and wu.name IS NOT null AND wu.status = 'ACTIVE' ");
                if (!orgUnitID.Equals(""))
                {
                    sb.Append("AND empl.organizational_unit_id IN (" + orgUnitID + ")");
                }
                select = sb.ToString();

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Working units");
                DataTable table = dataSet.Tables["Working units"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        wUnit = new WorkingUnitTO();
                        wUnit.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());

                        if (!row["parent_working_unit_id"].Equals(DBNull.Value))
                        {
                            wUnit.ParentWorkingUID = Int32.Parse(row["parent_working_unit_id"].ToString().Trim());
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            wUnit.Description = row["description"].ToString().Trim();
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            wUnit.Name = row["name"].ToString().Trim();
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            wUnit.Status = row["status"].ToString().Trim();
                        }
                        if (!row["address_id"].Equals(DBNull.Value))
                        {
                            wUnit.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        if (!row["code"].Equals(DBNull.Value))
                        {
                            wUnit.Code = row["code"].ToString().Trim();
                        }

                        // Add Working Unit object
                        list.Add(wUnit);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Exception: " + ex.Message);
            }

            return list;
        }
        public DataSet getRootWorkingUnits(string workigUnitID)
        {
            DataSet dataSet = new DataSet();
            string select = "";
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT * FROM working_units WHERE working_unit_id = parent_working_unit_id AND status = 'ACTIVE' ");

                if (!workigUnitID.Equals(""))
                {
                    sb.Append("AND working_unit_id IN (" + workigUnitID.Trim() + ")");
                }

                select = sb.ToString();

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Working units");
                DataTable table = dataSet.Tables["Working units"];
            }
            catch (Exception ex)
            {

                throw new Exception("Exception: " + ex.Message);
            }

            return dataSet;
        }

        public List<WorkingUnitTO> getRootWorkingUnitsList(string workigUnitID)
        {
            DataSet dataSet = new DataSet();
            string select = "";
            List<WorkingUnitTO> list = new List<WorkingUnitTO>();
            WorkingUnitTO wUnit = new WorkingUnitTO();
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT * FROM working_units WHERE working_unit_id = parent_working_unit_id AND status = 'ACTIVE' ");

                if (!workigUnitID.Equals(""))
                {
                    sb.Append("AND working_unit_id IN (" + workigUnitID.Trim() + ")");
                }

                select = sb.ToString();

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                sqlDataAdapter.Fill(dataSet, "WorkingUnits");
                DataTable table = dataSet.Tables["WorkingUnits"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        wUnit = new WorkingUnitTO();
                        wUnit.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());

                        if (!row["parent_working_unit_id"].Equals(DBNull.Value))
                        {
                            wUnit.ParentWorkingUID = Int32.Parse(row["parent_working_unit_id"].ToString().Trim());
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            wUnit.Description = row["description"].ToString().Trim();
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            wUnit.Name = row["name"].ToString().Trim();
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            wUnit.Status = row["status"].ToString().Trim();
                        }
                        if (!row["address_id"].Equals(DBNull.Value))
                        {
                            wUnit.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        if (!row["code"].Equals(DBNull.Value))
                        {
                            wUnit.Code = row["code"].ToString().Trim();
                        }
                        //wUnit.EmplNumber = getNumberOfEmployees(wUnit.WorkingUnitID);
                        //wUnit.ChildWUNumber = getNumberOfChild(wUnit.WorkingUnitID);

                        // Add Working Unit object
                        list.Add(wUnit);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Exception: " + ex.Message);
            }

            return list;
        }
        //public List<WorkingUnitTO> getWUnitByName(string wUnitS) 
        //{
        //    DataSet dataSet = new DataSet();
        //    WorkingUnitTO wUnit = new WorkingUnitTO();
        //    List<WorkingUnitTO> wUnitList = new List<WorkingUnitTO>();
        //    string select = "";
        //    try
        //    {
        //        if (!wUnitS.Trim().Equals(""))
        //        {
        //            StringBuilder sb = new StringBuilder();

        //            sb.Append("SELECT * FROM working_units WHERE status = 'ACTIVE' ");
        //            if (!wUnit.Equals(""))
        //            {
        //                sb.Append("AND name IN (" + wUnitS.Trim() + ")");
        //            }
        //            select = sb.ToString();
        //            SqlCommand cmd = new SqlCommand(select, conn);
        //            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

        //            sqlDataAdapter.Fill(dataSet, "WorkingUnits");
        //            DataTable table = dataSet.Tables["WorkingUnits"];

        //            if (table.Rows.Count > 0)
        //            {
        //                foreach (DataRow row in table.Rows)
        //                {
        //                    wUnit = new WorkingUnitTO();
        //                    wUnit.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());

        //                    if (!row["parent_working_unit_id"].Equals(DBNull.Value))
        //                    {
        //                        wUnit.ParentWorkingUID = Int32.Parse(row["parent_working_unit_id"].ToString().Trim());
        //                    }
        //                    if (!row["description"].Equals(DBNull.Value))
        //                    {
        //                        wUnit.Description = row["description"].ToString().Trim();
        //                    }
        //                    if (!row["name"].Equals(DBNull.Value))
        //                    {
        //                        wUnit.Name = row["name"].ToString().Trim();
        //                    }
        //                    if (!row["status"].Equals(DBNull.Value))
        //                    {
        //                        wUnit.Status = row["status"].ToString().Trim();
        //                    }
        //                    if (!row["address_id"].Equals(DBNull.Value))
        //                    {
        //                        wUnit.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
        //                    }
        //                    if (!row["code"].Equals(DBNull.Value))
        //                    {
        //                        wUnit.Code = row["code"].ToString().Trim();
        //                    }
        //                    //wUnit.EmplNumber = getNumberOfEmployees(wUnit.WorkingUnitID);
        //                    //wUnit.ChildWUNumber = getNumberOfChild(wUnit.WorkingUnitID);

        //                    // Add Working Unit object
        //                    wUnitList.Add(wUnit);
        //                }
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
                
        //    }
        //    return wUnitList;
        //}

        public List<WorkingUnitTO> getWUnits(string wUnits)
        {
            DataSet dataSet = new DataSet();
            WorkingUnitTO wUnit = new WorkingUnitTO();
            List<WorkingUnitTO> wUnitList = new List<WorkingUnitTO>();
            string select = "";

            try
            {
                if (!wUnits.Trim().Equals(""))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Append("SELECT * FROM working_units WHERE status = 'ACTIVE' ");
                    if (!wUnits.Equals(""))
                    {
                        sb.Append("AND working_unit_id IN (" + wUnits.Trim() + ")");
                    }
                    select = sb.ToString();

                    SqlCommand cmd = new SqlCommand(select, conn);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "WorkingUnits");
                    DataTable table = dataSet.Tables["WorkingUnits"];

                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            wUnit = new WorkingUnitTO();
                            wUnit.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());

                            if (!row["parent_working_unit_id"].Equals(DBNull.Value))
                            {
                                wUnit.ParentWorkingUID = Int32.Parse(row["parent_working_unit_id"].ToString().Trim());
                            }
                            if (!row["description"].Equals(DBNull.Value))
                            {
                                wUnit.Description = row["description"].ToString().Trim();
                            }
                            if (!row["name"].Equals(DBNull.Value))
                            {
                                wUnit.Name = row["name"].ToString().Trim();
                            }
                            if (!row["status"].Equals(DBNull.Value))
                            {
                                wUnit.Status = row["status"].ToString().Trim();
                            }
                            if (!row["address_id"].Equals(DBNull.Value))
                            {
                                wUnit.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                            }
                            if (!row["code"].Equals(DBNull.Value))
                            {
                                wUnit.Code = row["code"].ToString().Trim();
                            }
                            //wUnit.EmplNumber = getNumberOfEmployees(wUnit.WorkingUnitID);
                            //wUnit.ChildWUNumber = getNumberOfChild(wUnit.WorkingUnitID);

                            // Add Working Unit object
                            wUnitList.Add(wUnit);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Exception: " + ex.Message);
            }

            return wUnitList;
        }

        /// <summary>
        /// Find and return Working Unit object 
        /// with given key in working_units table
        /// </summary>
        /// <param name="workingUnitID">primary key</param>
        /// <returns>WorkingUnitTO object if succedeed</returns>
        public WorkingUnitTO find(int workingUnitID, bool useTrans)
        {
            DataSet dataSet = new DataSet();
            WorkingUnitTO wUnit = new WorkingUnitTO();

            try
            {
                //SqlCommand cmd = new SqlCommand( "SELECT * FROM working_units WHERE working_unit_id = " + workingUnitID + " ", conn );
                SqlCommand cmd = null;
                if (useTrans)
                    cmd = new SqlCommand("SELECT * FROM working_units WHERE working_unit_id = " + workingUnitID + " ", conn, SqlTrans);
                else
                    cmd = new SqlCommand("SELECT * FROM working_units WHERE working_unit_id = " + workingUnitID + " ", conn);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "WorkingUnit");
                DataTable table = dataSet.Tables["WorkingUnit"];

                if (table.Rows.Count == 1)
                {
                    wUnit.WorkingUnitID = Int32.Parse(table.Rows[0]["working_unit_id"].ToString().Trim());

                    if (!table.Rows[0]["parent_working_unit_id"].Equals(DBNull.Value))
                    {
                        wUnit.ParentWorkingUID = Int32.Parse(table.Rows[0]["parent_working_unit_id"].ToString().Trim());
                    }
                    if (!table.Rows[0]["description"].Equals(DBNull.Value))
                    {
                        wUnit.Description = table.Rows[0]["description"].ToString().Trim();
                    }
                    if (!table.Rows[0]["name"].Equals(DBNull.Value))
                    {
                        wUnit.Name = table.Rows[0]["name"].ToString().Trim();
                    }
                    if (!table.Rows[0]["status"].Equals(DBNull.Value))
                    {
                        wUnit.Status = table.Rows[0]["status"].ToString().Trim();
                    }
                    if (!table.Rows[0]["address_id"].Equals(DBNull.Value))
                    {
                        wUnit.AddressID = Int32.Parse(table.Rows[0]["address_id"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Exception: " + ex.Message);
            }

            return wUnit;

        }

        public int findMAXWUID()
        {
            DataSet dataSet = new DataSet();
            int wuID = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT MAX(working_unit_id) AS wu_id FROM working_units", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "WUnits");
                DataTable table = dataSet.Tables["WUnits"];

                if (table.Rows.Count == 1 && !table.Rows[0]["wu_id"].Equals(DBNull.Value))
                {
                    wuID = Int32.Parse(table.Rows[0]["wu_id"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return wuID;
        }

        public int findMINWUID()
        {
            DataSet dataSet = new DataSet();
            int wuID = -1;
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT MIN(working_unit_id) AS wu_id FROM working_units", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "WUnits");
                DataTable table = dataSet.Tables["WUnits"];

                if (table.Rows.Count == 1 && !table.Rows[0]["wu_id"].Equals(DBNull.Value))
                {
                    wuID = Int32.Parse(table.Rows[0]["wu_id"].ToString().Trim());
                }                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return wuID;
        }

        public Dictionary<int, WorkingUnitTO> getWUDictionary()
        {

            DataSet dataSet = new DataSet();
            WorkingUnitTO wUnit = new WorkingUnitTO();
            Dictionary<int, WorkingUnitTO> wUnitDict = new Dictionary<int, WorkingUnitTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM working_units ");

                select += sb.ToString()+ " ORDER BY name";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "WorkingUnits");
                DataTable table = dataSet.Tables["WorkingUnits"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        wUnit = new WorkingUnitTO();
                        wUnit.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());

                        if (!row["parent_working_unit_id"].Equals(DBNull.Value))
                        {
                            wUnit.ParentWorkingUID = Int32.Parse(row["parent_working_unit_id"].ToString().Trim());
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            wUnit.Description = row["description"].ToString().Trim();
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            wUnit.Name = row["name"].ToString().Trim();
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            wUnit.Status = row["status"].ToString().Trim();
                        }
                        if (!row["address_id"].Equals(DBNull.Value))
                        {
                            wUnit.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        if (!row["code"].Equals(DBNull.Value))
                        {
                            wUnit.Code = row["code"].ToString().Trim();
                        }
                        wUnit.EmplNumber = getNumberOfEmployees(wUnit.WorkingUnitID);
                        wUnit.ChildWUNumber = getNumberOfChild(wUnit.WorkingUnitID);

                        if (!wUnitDict.ContainsKey(wUnit.WorkingUnitID))
                            wUnitDict.Add(wUnit.WorkingUnitID, new WorkingUnitTO());

                        wUnitDict[wUnit.WorkingUnitID] = wUnit;

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }
            return wUnitDict;
        }
        public Dictionary<int, WorkingUnitTO> getWUDictionary(IDbTransaction trans)
        {

            DataSet dataSet = new DataSet();
            WorkingUnitTO wUnit = new WorkingUnitTO();
            Dictionary<int, WorkingUnitTO> wUnitDict = new Dictionary<int, WorkingUnitTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM working_units ");

                select += sb.ToString() + " ORDER BY name";

                SqlCommand cmd = new SqlCommand(select, conn,(SqlTransaction)trans);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "WorkingUnits");
                DataTable table = dataSet.Tables["WorkingUnits"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        wUnit = new WorkingUnitTO();
                        wUnit.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());

                        if (!row["parent_working_unit_id"].Equals(DBNull.Value))
                        {
                            wUnit.ParentWorkingUID = Int32.Parse(row["parent_working_unit_id"].ToString().Trim());
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            wUnit.Description = row["description"].ToString().Trim();
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            wUnit.Name = row["name"].ToString().Trim();
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            wUnit.Status = row["status"].ToString().Trim();
                        }
                        if (!row["address_id"].Equals(DBNull.Value))
                        {
                            wUnit.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        wUnit.EmplNumber = getNumberOfEmployees(wUnit.WorkingUnitID,trans);
                        wUnit.ChildWUNumber = getNumberOfChild(wUnit.WorkingUnitID,trans);

                        if (!wUnitDict.ContainsKey(wUnit.WorkingUnitID))
                            wUnitDict.Add(wUnit.WorkingUnitID, new WorkingUnitTO());

                        wUnitDict[wUnit.WorkingUnitID] = wUnit;

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }
            return wUnitDict;
        }
        /// <summary>
        /// Create list of the WorkingUnitTO objects that achieve given conditions
        /// </summary>
        /// <param name="workingUnitID">primary key</param>
        /// <param name="parentWUID">wokring unit id of the unit where this one belongs to</param>
        /// <param name="description">working unit description</param>
        /// <param name="name">working unit name</param>
        /// <returns>WorkingUnitTO list</returns>
        public List<WorkingUnitTO> getWorkingUnits(WorkingUnitTO wuTO)
        {
            DataSet dataSet = new DataSet();
            WorkingUnitTO wUnit = new WorkingUnitTO();
            List<WorkingUnitTO> wUnitList = new List<WorkingUnitTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM working_units ");

                if ((wuTO.WorkingUnitID != -1) || (wuTO.ParentWorkingUID != -1) ||
                    (!wuTO.Description.Trim().Equals("")) || (!wuTO.Name.Trim().Equals("")) || (!wuTO.Status.Trim().Equals("")))
                {
                    sb.Append(" WHERE ");

                    if (wuTO.WorkingUnitID != -1)
                    {
                        sb.Append(" working_unit_id = '" + wuTO.WorkingUnitID.ToString().Trim() + "' AND");
                    }
                    if (wuTO.ParentWorkingUID != -1)
                    {
                        sb.Append(" parent_working_unit_id = '" + wuTO.ParentWorkingUID.ToString().Trim() + "' AND");
                    }
                    if (!wuTO.Description.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(description) LIKE N'%" + wuTO.Description.ToUpper().Trim() + "%' AND");
                    }
                    if (!wuTO.Name.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(name) LIKE N'%" + wuTO.Name.ToUpper().Trim() + "%' AND");
                    }
                    if (!wuTO.Status.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(status) LIKE N'%" + wuTO.Status.ToUpper().Trim() + "%' AND");
                    }
                    
                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY name ";

                SqlCommand cmd;
                if (SqlTrans != null)
                {
                    cmd = new SqlCommand(select, conn, SqlTrans);
                }
                else
                {
                    cmd = new SqlCommand(select, conn);
                }

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "WorkingUnits");
                DataTable table = dataSet.Tables["WorkingUnits"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        wUnit = new WorkingUnitTO();

                        wUnit.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());

                        if (!row["parent_working_unit_id"].Equals(DBNull.Value))
                        {
                            wUnit.ParentWorkingUID = Int32.Parse(row["parent_working_unit_id"].ToString().Trim());
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            wUnit.Description = row["description"].ToString().Trim();
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            wUnit.Name = row["name"].ToString().Trim();
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            wUnit.Status = row["status"].ToString().Trim();
                        }
                        if (!row["address_id"].Equals(DBNull.Value))
                        {
                            wUnit.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        if (!row["working_unit_id"].Equals(DBNull.Value))
                        {
                            wUnit.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (!row["code"].Equals(DBNull.Value))
                        {
                            wUnit.Code = row["code"].ToString().Trim();
                        }
                        wUnit.EmplNumber = getNumberOfEmployees(wUnit.WorkingUnitID);
                        wUnit.ChildWUNumber = getNumberOfChild(wUnit.WorkingUnitID);

                        wUnitList.Add(wUnit);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Exception: " + ex.Message);

            }
            return wUnitList;
        }
        /// <summary>
        /// Create list of the WorkingUnitTO objects that achieve given conditions
        /// </summary>
        /// <param name="workingUnitID">primary key</param>
        /// <param name="parentWUID">wokring unit id of the unit where this one belongs to</param>
        /// <param name="description">working unit description</param>
        /// <param name="name">working unit name</param>
        /// <returns>WorkingUnitTO list</returns>
        public List<WorkingUnitTO> getWorkingUnitsExact(WorkingUnitTO wuTO)
        {
            DataSet dataSet = new DataSet();
            WorkingUnitTO wUnit = new WorkingUnitTO();
            List<WorkingUnitTO> wUnitList = new List<WorkingUnitTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM working_units ");

                if ((wuTO.WorkingUnitID != -1) || (wuTO.ParentWorkingUID != -1) ||
                    (!wuTO.Description.Trim().Equals("")) || (!wuTO.Name.Trim().Equals("")) || (!wuTO.Status.Trim().Equals("")))
                {
                    sb.Append(" WHERE ");

                    if (wuTO.WorkingUnitID != -1)
                    {
                        sb.Append(" working_unit_id = '" + wuTO.WorkingUnitID.ToString().Trim() + "' AND");
                    }
                    if (wuTO.ParentWorkingUID != -1)
                    {
                        sb.Append(" parent_working_unit_id = '" + wuTO.ParentWorkingUID.ToString().Trim() + "' AND");
                    }
                    if (!wuTO.Description.Trim().Equals(""))
                    {
                        sb.Append(" description = N'" + wuTO.Description.ToUpper().Trim() + "' AND");
                    }
                    if (!wuTO.Name.Trim().Equals(""))
                    {
                        sb.Append(" name = N'" + wuTO.Name.ToUpper().Trim() + "' AND");
                    }
                    if (!wuTO.Status.Trim().Equals(""))
                    {
                        sb.Append(" status = N'" + wuTO.Status.ToUpper().Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY name ";

                SqlCommand cmd;
                if (SqlTrans != null)
                {
                    cmd = new SqlCommand(select, conn, SqlTrans);
                }
                else
                {
                    cmd = new SqlCommand(select, conn);
                }

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "WorkingUnits");
                DataTable table = dataSet.Tables["WorkingUnits"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        wUnit = new WorkingUnitTO();

                        wUnit.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());

                        if (!row["parent_working_unit_id"].Equals(DBNull.Value))
                        {
                            wUnit.ParentWorkingUID = Int32.Parse(row["parent_working_unit_id"].ToString().Trim());
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            wUnit.Description = row["description"].ToString().Trim();
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            wUnit.Name = row["name"].ToString().Trim();
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            wUnit.Status = row["status"].ToString().Trim();
                        }
                        if (!row["address_id"].Equals(DBNull.Value))
                        {
                            wUnit.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        if (!row["working_unit_id"].Equals(DBNull.Value))
                        {
                            wUnit.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (!row["code"].Equals(DBNull.Value))
                        {
                            wUnit.Code = row["code"].ToString().Trim();
                        }
                        wUnit.EmplNumber = getNumberOfEmployees(wUnit.WorkingUnitID);
                        wUnit.ChildWUNumber = getNumberOfChild(wUnit.WorkingUnitID);

                        wUnitList.Add(wUnit);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Exception: " + ex.Message);

            }
            return wUnitList;
        }

        public List<WorkingUnitTO> getRootWU(string wuIDs)
        {
            DataSet dataSet = new DataSet();
            WorkingUnitTO wUnit = new WorkingUnitTO();
            List<WorkingUnitTO> wUnitList = new List<WorkingUnitTO>();

            try
            {
                string select = "SELECT * FROM working_units WHERE working_unit_id = parent_working_unit_id AND status = 'ACTIVE' ";

                if (wuIDs.Length > 0)
                    select += "AND working_unit_id IN (" + wuIDs.Trim() + ") ";

                select += "ORDER BY name";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "WorkingUnits");
                DataTable table = dataSet.Tables["WorkingUnits"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        wUnit = new WorkingUnitTO();
                        wUnit.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());

                        if (!row["parent_working_unit_id"].Equals(DBNull.Value))
                        {
                            wUnit.ParentWorkingUID = Int32.Parse(row["parent_working_unit_id"].ToString().Trim());
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            wUnit.Description = row["description"].ToString().Trim();
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            wUnit.Name = row["name"].ToString().Trim();
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            wUnit.Status = row["status"].ToString().Trim();
                        }
                        if (!row["address_id"].Equals(DBNull.Value))
                        {
                            wUnit.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        wUnit.EmplNumber = getNumberOfEmployees(wUnit.WorkingUnitID);
                        wUnit.ChildWUNumber = getNumberOfChild(wUnit.WorkingUnitID);

                        // Add Working Unit object
                        wUnitList.Add(wUnit);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return wUnitList;
        }

        public List<WorkingUnitTO> getChildWU(string parentID)
        {
            DataSet dataSet = new DataSet();
            WorkingUnitTO wUnit = new WorkingUnitTO();
            List<WorkingUnitTO> wUnitList = new List<WorkingUnitTO>();

            try
            {
                string select = "SELECT * FROM working_units WHERE parent_working_unit_id = '" + parentID.Trim() +
                    "' AND working_unit_id <> parent_working_unit_id AND status = 'ACTIVE' ORDER BY name";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "WorkingUnits");
                DataTable table = dataSet.Tables["WorkingUnits"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        wUnit = new WorkingUnitTO();
                        wUnit.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());

                        if (!row["parent_working_unit_id"].Equals(DBNull.Value))
                        {
                            wUnit.ParentWorkingUID = Int32.Parse(row["parent_working_unit_id"].ToString().Trim());
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            wUnit.Description = row["description"].ToString().Trim();
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            wUnit.Name = row["name"].ToString().Trim();
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            wUnit.Status = row["status"].ToString().Trim();
                        }
                        if (!row["address_id"].Equals(DBNull.Value))
                        {
                            wUnit.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
                        }
                        wUnit.EmplNumber = getNumberOfEmployees(wUnit.WorkingUnitID);
                        wUnit.ChildWUNumber = getNumberOfChild(wUnit.WorkingUnitID);

                        // Add Working Unit object
                        wUnitList.Add(wUnit);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return wUnitList;
        }

        public bool beginTransaction()
        {
            bool isStarted = false;

            try
            {
                this.SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
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

        public void serialize(List<WorkingUnitTO> WorkingUnitTO)
        {
            try
            {
                //string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLWUsFile"];
                string filename = Constants.XMLDataSourceDir + Constants.XMLWUsFile;
                Stream stream = File.Open(filename, FileMode.Create);

                WorkingUnitTO[] workingUnitArray = (WorkingUnitTO[])WorkingUnitTO.ToArray();

                XmlSerializer bformatter = new XmlSerializer(typeof(WorkingUnitTO[]));
                bformatter.Serialize(stream, workingUnitArray);
                stream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<WorkingUnitTO> getWUByName(string name)
        {
            DataSet dataSet = new DataSet();
            string select = "select * from actamgr.working_units where name='" + name + "'";
            SqlCommand cmd = new SqlCommand(select, conn);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

            sqlDataAdapter.Fill(dataSet, "WorkingUnits");
            DataTable table = dataSet.Tables["WorkingUnits"];
            List<WorkingUnitTO> wUnitList = new List<WorkingUnitTO>();
            WorkingUnitTO wu = new WorkingUnitTO();
            foreach (DataRow item in table.Rows)
            {
                
                wu.WorkingUnitID = int.Parse(item["working_unit_id"].ToString());
                wu.Name = item["name"].ToString();
                wu.ParentWorkingUID = int.Parse(item["parent_working_unit_id"].ToString());
                wu.Description = item["description"].ToString();
                wu.Status = item["status"].ToString();
                wu.Code = item["code"].ToString();
                wu.EmplNumber = getNumberOfEmployees(wu.WorkingUnitID);
                wu.ChildWUNumber = getNumberOfChild(wu.WorkingUnitID);
                wUnitList.Add(wu);
            }
            return wUnitList;
        }
        public List<WorkingUnitTO> getAllWU()
        {
            DataSet dataSet = new DataSet();
            string select = "SELECT * FROM actamgr.working_units WHERE status='ACTIVE'";
            SqlCommand cmd = new SqlCommand(select, conn);
            SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);
            sqlDA.Fill(dataSet, "WU");
            DataTable table = dataSet.Tables["WU"];
            List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
            WorkingUnitTO wu = new WorkingUnitTO();
            foreach (DataRow item in table.Rows)
            {
                wu = new WorkingUnitTO();
                wu.WorkingUnitID=int.Parse(item["working_unit_id"].ToString());
                wu.Name = item["name"].ToString();
                wu.ParentWorkingUID = int.Parse(item["parent_working_unit_id"].ToString());
                wu.Description = item["description"].ToString();
                wuList.Add(wu);
            }
            return wuList;
        }
    }
}
