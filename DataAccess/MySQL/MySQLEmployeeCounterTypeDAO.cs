using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Data;

using TransferObjects;
using Util;

namespace DataAccess
{
    public class MySQLEmployeeCounterTypeDAO : EmployeeCounterTypeDAO
    {
        MySqlConnection conn = null;
        MySqlTransaction _sqlTrans = null;

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MySQLEmployeeCounterTypeDAO()
		{
			conn = MySQLDAOFactory.getConnection();
		}
        public MySQLEmployeeCounterTypeDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
        }
        public int insert(EmployeeCounterTypeTO emplCTypeTO)
        {
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employee_counter_types ");
                sbInsert.Append("(employee_counter_type_id, name, name_alternative, description, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("'" + emplCTypeTO.EmplCounterTypeID.ToString().Trim() + "', ");
                sbInsert.Append("N'" + emplCTypeTO.Name.Trim() + "', ");
                sbInsert.Append("N'" + emplCTypeTO.NameAlt.Trim() + "', ");

                if (emplCTypeTO.Desc.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + emplCTypeTO.Desc.Trim() + "', ");
                }
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");
                                
                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);

                rowsAffected = cmd.ExecuteNonQuery();

                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw ex;
            }

            return rowsAffected;
        }

        public bool delete(int typeID)
        {
            bool isDeleted = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM employee_counter_types WHERE employee_counter_type_id = '" + typeID.ToString().Trim() + "'");

                MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                isDeleted = true;
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw ex;                
            }

            return isDeleted;
        }

        public bool update(EmployeeCounterTypeTO emplCTypeTO)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE employee_counter_types SET ");
                if (!emplCTypeTO.Name.Trim().Equals(""))
                {
                    sbUpdate.Append("name = N'" + emplCTypeTO.Name.Trim() + "', ");
                }
                if (!emplCTypeTO.NameAlt.Trim().Equals(""))
                {
                    sbUpdate.Append("name_alternative = N'" + emplCTypeTO.NameAlt.Trim() + "', ");
                }
                if (!emplCTypeTO.Desc.Trim().Equals(""))
                {
                    sbUpdate.Append("description = N'" + emplCTypeTO.Desc.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("description = null, ");
                }
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = NOW() ");
                sbUpdate.Append("WHERE employee_counter_type_id = '" + emplCTypeTO.EmplCounterTypeID.ToString().Trim() + "'");

                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw ex;
            }

            return isUpdated;
        }

        public EmployeeCounterTypeTO find(int typeID)
        {
            DataSet dataSet = new DataSet();
            EmployeeCounterTypeTO typeTO = new EmployeeCounterTypeTO();

            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM employee_counter_types WHERE employee_counter_type_id = '" + typeID.ToString().Trim() + "'", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Types");
                DataTable table = dataSet.Tables["Types"];

                if (table.Rows.Count == 1)
                {
                    typeTO = new EmployeeCounterTypeTO();
                    typeTO.EmplCounterTypeID = Int32.Parse(table.Rows[0]["employee_counter_type_id"].ToString().Trim());

                    if (!table.Rows[0]["name"].Equals(DBNull.Value))
                    {
                        typeTO.Name = table.Rows[0]["name"].ToString().Trim();
                    }
                    if (!table.Rows[0]["name_alternative"].Equals(DBNull.Value))
                    {
                        typeTO.NameAlt = table.Rows[0]["name_alternative"].ToString().Trim();
                    }
                    if (!table.Rows[0]["description"].Equals(DBNull.Value))
                    {
                        typeTO.Desc = table.Rows[0]["description"].ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return typeTO;
        }

        public List<EmployeeCounterTypeTO> getEmplCounterTypes(EmployeeCounterTypeTO emplCTypeTO)
        {
            DataSet dataSet = new DataSet();
            EmployeeCounterTypeTO typeTO = new EmployeeCounterTypeTO();
            List<EmployeeCounterTypeTO> typesList = new List<EmployeeCounterTypeTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employee_counter_types ");
                if ((emplCTypeTO.EmplCounterTypeID != -1) || (!emplCTypeTO.Name.Trim().Equals("")) || (!emplCTypeTO.NameAlt.Trim().Equals("")) || (!emplCTypeTO.Desc.Trim().Equals("")))
                {
                    sb.Append(" WHERE");
                    if (emplCTypeTO.EmplCounterTypeID != -1)
                    {
                        sb.Append(" employee_counter_type_id = '" + emplCTypeTO.EmplCounterTypeID.ToString().Trim() + "' AND");
                    }
                    if (!emplCTypeTO.Name.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(name) = N'" + emplCTypeTO.Name.ToUpper().Trim() + "' AND");
                    }
                    if (!emplCTypeTO.NameAlt.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(name_alternative) = N'" + emplCTypeTO.NameAlt.ToUpper().Trim() + "' AND");
                    }
                    if (!emplCTypeTO.Desc.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(description) = N'" + emplCTypeTO.Desc.ToUpper().Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY name ";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Types");
                DataTable table = dataSet.Tables["Types"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        typeTO = new EmployeeCounterTypeTO();

                        typeTO.EmplCounterTypeID = Int32.Parse(row["employee_counter_type_id"].ToString().Trim());

                        if (!row["name"].Equals(DBNull.Value))
                        {
                            typeTO.Name = row["name"].ToString().Trim();
                        }
                        if (!row["name_alternative"].Equals(DBNull.Value))
                        {
                            typeTO.NameAlt = row["name_alternative"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            typeTO.Desc = row["description"].ToString().Trim();
                        }

                        typesList.Add(typeTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return typesList;
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
            _sqlTrans = (MySqlTransaction)trans;
        }        
    }
}
