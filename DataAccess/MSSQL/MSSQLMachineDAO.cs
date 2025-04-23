using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;
using TransferObjects;
using System.Data;

namespace DataAccess.MSSQL
{
    public class MSSQLMachineDAO : MachineDAO
    {

        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLMachineDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public MSSQLMachineDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        #region MachineDAO Members

        public List<MachineTO> getMachines(MachineTO machineTO)
        {
            DataSet dataSet = new DataSet();
            List<MachineTO> machinesList = new List<MachineTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM machines ");
                sb.Append(" WHERE machine_id >= 0");
                if ((machineTO.MachineID != -1) || (!machineTO.Name.Trim().Equals("")) ||
                    (!machineTO.Description.Trim().Equals("")))
                {
                    sb.Append(" AND");
                    if (machineTO.MachineID != -1)
                    {
                        sb.Append(" machine_id = '" + machineTO.MachineID.ToString().Trim() + "' AND");
                    }
                    if (!machineTO.Name.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(name) LIKE N'%" + machineTO.Name.ToUpper().Trim() + "%' AND");
                    }
                    if (!machineTO.Description.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(description) LIKE N'%" + machineTO.Description.ToUpper().Trim() + "%' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY machine_id ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Machines");
                DataTable table = dataSet.Tables["Machines"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        MachineTO mTO = new MachineTO();

                        mTO.MachineID = Int32.Parse(row["machine_id"].ToString().Trim());
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            mTO.Name = row["name"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            mTO.Description = row["description"].ToString().Trim();
                        }

                        machinesList.Add(mTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return machinesList;
        }

        public int insert(string name, string description)
        {
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            int machineID = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("SET NOCOUNT ON ");
                sbInsert.Append("INSERT INTO machines ");
                sbInsert.Append("(name, description, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                if (name.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + name.Trim() + "', ");
                }
                if (description.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + description.Trim() + "', ");
                }

                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");
                sbInsert.Append("SELECT @@Identity AS machine_id ");
                sbInsert.Append("SET NOCOUNT OFF ");

                DataSet dataSet = new DataSet();
                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Machines");

                DataTable table = dataSet.Tables["Machines"];

                machineID = Int32.Parse(table.Rows[0]["machine_id"].ToString());

                sqlTrans.Commit();

            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("INSERT");

                throw ex;
            }
            return machineID;
        }

        public bool update(int machineID, string name, string description)
        {
            bool isUpdated = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE machines SET ");
                if (!name.Trim().Equals(""))
                {
                    sbUpdate.Append("name = N'" + name.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("name = null, ");
                }
                if (!description.Trim().Equals(""))
                {
                    sbUpdate.Append("description = N'" + description.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("description = null, ");
                }

                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE machine_id = '" + machineID.ToString().Trim() + "'");

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

        public bool delete(int machineID)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM machines WHERE machine_id = '" + machineID.ToString().Trim() + "'");

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

        public MachineTO find(int machineID)
        {
            DataSet dataSet = new DataSet();
            MachineTO machineTO = new MachineTO();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM machines WHERE machine_id = '" + machineID.ToString().Trim() + "'", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Machines");
                DataTable table = dataSet.Tables["Machines"];

                if (table.Rows.Count == 1)
                {
                    machineTO = new MachineTO();
                    machineTO.MachineID = Int32.Parse(table.Rows[0]["machine_id"].ToString().Trim());

                    if (!table.Rows[0]["name"].Equals(DBNull.Value))
                    {
                        machineTO.Name = table.Rows[0]["name"].ToString().Trim();
                    }
                    if (!table.Rows[0]["description"].Equals(DBNull.Value))
                    {
                        machineTO.Description = table.Rows[0]["description"].ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return machineTO;
        }

        public List<EmployeeXMachineTO> findEmployeesForMachine(int machineID)
        {
            DataSet dataSet = new DataSet();
            List<EmployeeXMachineTO> employees = new List<EmployeeXMachineTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employee_x_machines exm INNER JOIN employees e ON exm.employee_id = e.employee_id ");
                sb.Append(" WHERE machine_id = " + machineID + " ");
                sb.Append(" AND exm.status = 'ACTIVE' ");

                select = sb.ToString();
                select = select + "ORDER BY exm.employee_id ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Machines");
                DataTable table = dataSet.Tables["Machines"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        EmployeeXMachineTO exmTO = new EmployeeXMachineTO();
                        exmTO.MachineTO = new MachineTO();
                        exmTO.EmployeeTO = new EmployeeTO();
                        exmTO.MachineTO.MachineID = Int32.Parse(row["machine_id"].ToString().Trim());
                        exmTO.EmployeeTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (!row["first_name"].Equals(DBNull.Value))
                        {
                            exmTO.EmployeeTO.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (!row["last_name"].Equals(DBNull.Value))
                        {
                            exmTO.EmployeeTO.LastName = row["last_name"].ToString().Trim();
                        }
                        if (!row["date"].Equals(DBNull.Value))
                        {
                            exmTO.Date = Convert.ToDateTime(row["date"].ToString().Trim());
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            exmTO.Status = row["status"].ToString().Trim();
                        }

                        employees.Add(exmTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return employees;
        }

        public int insertEmployeesForMachine(List<EmployeeXMachineTO> employeesForMachine)
        {
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            //int machineID = 0;

            try
            {
                foreach (EmployeeXMachineTO exmTO in employeesForMachine)
                {
                    StringBuilder sbInsert = new StringBuilder();
                    //sbInsert.Append("SET NOCOUNT ON ");
                    sbInsert.Append("INSERT INTO employee_x_machines ");
                    sbInsert.Append("(employee_id, machine_id, date, status, created_by, created_time) ");
                    sbInsert.Append("VALUES (");

                    if (exmTO.EmployeeTO.EmployeeID != -1)
                    {
                        sbInsert.Append(exmTO.EmployeeTO.EmployeeID + ", ");
                    }
                    if (exmTO.MachineTO.MachineID != -1)
                    {
                        sbInsert.Append(exmTO.MachineTO.MachineID + ", ");
                    }

                    sbInsert.Append("GETDATE(), ");
                    sbInsert.Append("'" + exmTO.Status + "', ");
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");
                    sbInsert.Append("SELECT @@Identity AS machine_id ");
                    sbInsert.Append("SET NOCOUNT OFF ");

                    DataSet dataSet = new DataSet();
                    SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "EmployeeXMachines");

                    DataTable table = dataSet.Tables["EmployeeXMachines"];

                    //machineID = Int32.Parse(table.Rows[0]["machine_id"].ToString()); 
                }

                sqlTrans.Commit();

            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("INSERT");

                throw ex;
            }
            return 1;
        }

        public bool deleteFromEmployeeXMachine(List<EmployeeXMachineTO> listEXmTO)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try
            {
                foreach (EmployeeXMachineTO eXmTO in listEXmTO)
                {
                    StringBuilder sbDelete = new StringBuilder();
                    sbDelete.Append("UPDATE employee_x_machines SET status = 'FINISHED', modified_by = '" + DAOController.GetLogInUser().Trim() + "', modified_time = getdate() WHERE machine_id = '" + eXmTO.MachineTO.MachineID.ToString().Trim() + "' AND employee_id = '" + eXmTO.EmployeeTO.EmployeeID + "'");

                    SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
                    int res = cmd.ExecuteNonQuery();
                }
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

        #endregion
    }
}
