using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using TransferObjects;
using System.Globalization;

namespace DataAccess
{
    class MySQLEmployeeResponsibilityDAO:EmployeeResponsibilityDAO
    {
         MySqlConnection conn = null;
        MySqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

		public MySQLEmployeeResponsibilityDAO()
		{
            conn = MySQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

        public MySQLEmployeeResponsibilityDAO(MySqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public bool delete(EmployeeResponsibilityTO emplRes, bool doCommit)
        {
            bool isDeleted = false;
            MySqlTransaction sqlTrans = null;
            if (doCommit)
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            else
                sqlTrans = SqlTrans;

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM employee_responsibility WHERE unit_id = '" + emplRes.UnitID.ToString().Trim() + "' ");
                sbDelete.Append("AND employee_id = '" + emplRes.EmployeeID.ToString().Trim() + "' ");
                sbDelete.Append("AND type = '" + emplRes.Type.ToString().Trim() + "' ");
                MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                isDeleted = true;
                if (doCommit)
                    sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    sqlTrans.Rollback();
                throw ex;
            }

            return isDeleted;
        }

        public bool insert(EmployeeResponsibilityTO emplRes, bool doCommit)
        {
            if (doCommit)
            {
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead );
            }
            else
            {
                SqlTrans = this.SqlTrans;
            }
            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employee_responsibility ");
                sbInsert.Append("(employee_id, unit_id, type, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append(" " + emplRes.EmployeeID + ",");
                sbInsert.Append(" " + emplRes.UnitID + ",");
               
                if (emplRes.Type != "")
                {
                    sbInsert.Append(" N'" + emplRes.Type + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");

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

            return rowsAffected>0;
        }

        public Dictionary<int, List<int>> getUnitsResponsibilitiesByEmployee(int id, string type)
        {
            DataSet dataSet = new DataSet();
            EmployeeResponsibilityTO respTO = new EmployeeResponsibilityTO();
            Dictionary<int, List<int>> responsibilityDict = new Dictionary<int, List<int>>();
            string select = "";

            try
            {
                select = "SELECT * FROM employee_responsibility WHERE employee_id IN (SELECT employee_id FROM employee_responsibility";

                if (id != -1 || !type.Trim().Equals(""))
                {
                    select += " WHERE";

                    if (id != -1)
                        select += " unit_id = '" + id.ToString().Trim() + "' AND";

                    if (!type.Trim().Equals(""))
                        select += " UPPER(type) = N'" + type.ToUpper().Trim() + "' AND";

                    select = select.Substring(0, select.Length - 3);
                }

                select += ")";

                if (!type.Trim().Equals(""))
                    select += " AND UPPER(type) = N'" + type.ToUpper().Trim() + "'";

                select += " ORDER BY employee_id";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeesResponsibility");
                DataTable table = dataSet.Tables["EmployeesResponsibility"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        respTO = new EmployeeResponsibilityTO();
                        respTO.RecID = Int32.Parse(row["rec_id"].ToString().Trim());
                        respTO.UnitID = Int32.Parse(row["unit_id"].ToString().Trim());

                        if (!row["employee_id"].Equals(DBNull.Value))
                        {
                            respTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (!row["type"].Equals(DBNull.Value))
                        {
                            respTO.Type = row["type"].ToString().Trim();
                        }

                        if (!responsibilityDict.ContainsKey(respTO.EmployeeID))
                            responsibilityDict.Add(respTO.EmployeeID, new List<int>());

                        responsibilityDict[respTO.EmployeeID].Add(respTO.UnitID);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return responsibilityDict;
        }

        public List<EmployeeResponsibilityTO> getEmplResponsibility(EmployeeResponsibilityTO emplResTO)
        {
            DataSet dataSet = new DataSet();
            EmployeeResponsibilityTO orgUnitTO = new EmployeeResponsibilityTO();
            List<EmployeeResponsibilityTO> ouList = new List<EmployeeResponsibilityTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employee_responsibility ");
                if ((emplResTO.UnitID != -1) || (emplResTO.EmployeeID != -1) || (!emplResTO.Type.Trim().Equals("")))
                {
                    sb.Append(" WHERE");
                    if (emplResTO.UnitID != -1)
                    {
                        sb.Append(" unit_id = '" + emplResTO.UnitID.ToString().Trim() + "' AND");
                    }
                    if (emplResTO.EmployeeID != -1)
                    {
                        sb.Append(" employee_id = '" + emplResTO.EmployeeID.ToString().Trim() + "' AND");
                    }
                    if (!emplResTO.Type.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(type) = N'" + emplResTO.Type.ToUpper().Trim() + "' AND");
                    }
                    

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeesResponsibility");
                DataTable table = dataSet.Tables["EmployeesResponsibility"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        orgUnitTO = new EmployeeResponsibilityTO();
                        orgUnitTO.RecID = Int32.Parse(row["rec_id"].ToString().Trim());
                        orgUnitTO.UnitID = Int32.Parse(row["unit_id"].ToString().Trim());

                        if (!row["employee_id"].Equals(DBNull.Value))
                        {
                            orgUnitTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (!row["type"].Equals(DBNull.Value))
                        {
                            orgUnitTO.Type = row["type"].ToString().Trim();
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

        public List<EmployeeResponsibilityTO> getEmplResponsibility(string ids, string type)
        {
            DataSet dataSet = new DataSet();
            EmployeeResponsibilityTO resTO = new EmployeeResponsibilityTO();
            List<EmployeeResponsibilityTO> resList = new List<EmployeeResponsibilityTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employee_responsibility ");
                if (!ids.Trim().Equals("") || !type.Trim().Equals(""))
                {
                    sb.Append(" WHERE");
                    if (!ids.Trim().Equals(""))
                    {
                        sb.Append(" unit_id IN (" + ids.ToString().Trim() + ") AND");
                    }
                    if (!type.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(type) = N'" + type.ToUpper().Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeesResponsibility");
                DataTable table = dataSet.Tables["EmployeesResponsibility"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        resTO = new EmployeeResponsibilityTO();
                        resTO.RecID = Int32.Parse(row["rec_id"].ToString().Trim());
                        resTO.UnitID = Int32.Parse(row["unit_id"].ToString().Trim());
                        if (!row["employee_id"].Equals(DBNull.Value))
                        {
                            resTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (!row["type"].Equals(DBNull.Value))
                        {
                            resTO.Type = row["type"].ToString().Trim();
                        }

                        resList.Add(resTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resList;
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
