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
    public class MySQLEmployeeCounterValueDAO : EmployeeCounterValueDAO
    {
        MySqlConnection conn = null;
        MySqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MySQLEmployeeCounterValueDAO()
		{
            conn = MySQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MySQLEmployeeCounterValueDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public int insert(EmployeeCounterValueTO emplCValueTO)
        {
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employee_counter_values ");
                sbInsert.Append("(employee_counter_type_id, employee_id, value, measure_unit, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("'" + emplCValueTO.EmplCounterTypeID.ToString().Trim() + "', ");
                sbInsert.Append("'" + emplCValueTO.EmplID.ToString().Trim() + "', ");
                sbInsert.Append("'" + emplCValueTO.Value.ToString().Trim() + "', ");
                sbInsert.Append("N'" + emplCValueTO.MeasureUnit.Trim() + "', ");
                if (!emplCValueTO.CreatedBy.Trim().Equals(""))
                    sbInsert.Append("N'" + emplCValueTO.CreatedBy.Trim() + "', ");
                else
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbInsert.Append("NOW()) ");
                                
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

        public int insert(EmployeeCounterValueTO emplCValueTO, bool doCommit)
        {
            MySqlTransaction sqlTrans = null;
            if (doCommit)
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            else
                sqlTrans = SqlTrans;
            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employee_counter_values ");
                sbInsert.Append("(employee_counter_type_id, employee_id, value, measure_unit, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("'" + emplCValueTO.EmplCounterTypeID.ToString().Trim() + "', ");
                sbInsert.Append("'" + emplCValueTO.EmplID.ToString().Trim() + "', ");
                sbInsert.Append("'" + emplCValueTO.Value.ToString().Trim() + "', ");
                sbInsert.Append("N'" + emplCValueTO.MeasureUnit.Trim() + "', ");
                if (!emplCValueTO.CreatedBy.Trim().Equals(""))
                    sbInsert.Append("N'" + emplCValueTO.CreatedBy.Trim() + "', ");
                else
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbInsert.Append("NOW()) ");

                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);

                rowsAffected = cmd.ExecuteNonQuery();
                if (doCommit)
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                sqlTrans.Rollback();
                throw ex;
            }

            return rowsAffected;
        }

        public bool delete(int typeID, int emplID)
        {
            bool isDeleted = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM employee_counter_values WHERE employee_counter_type_id = '" + typeID.ToString().Trim() + "' AND employee_id = '" + emplID.ToString().Trim() + "'");

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

        public bool update(EmployeeCounterValueTO emplCValueTO, bool doCommit)
        {
            bool isUpdated = false;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE employee_counter_values SET ");
                if (emplCValueTO.Value != -1)
                {
                    sbUpdate.Append("value = '" + emplCValueTO.Value.ToString().Trim() + "', ");
                }
                if (!emplCValueTO.MeasureUnit.Trim().Equals(""))
                {
                    sbUpdate.Append("measure_unit = N'" + emplCValueTO.MeasureUnit.Trim() + "', ");
                }
                if (!emplCValueTO.ModifiedBy.Trim().Equals(""))
                    sbUpdate.Append("modified_by = N'" + emplCValueTO.ModifiedBy.Trim() + "', ");
                else
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = NOW() ");
                sbUpdate.Append("WHERE employee_counter_type_id = '" + emplCValueTO.EmplCounterTypeID.ToString().Trim() + "' AND employee_id = '" + emplCValueTO.EmplID.ToString().Trim() + "'");

                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, SqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                if (doCommit)
                    SqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    SqlTrans.Rollback();
                throw ex;
            }

            return isUpdated;
        }

        public EmployeeCounterValueTO find(int typeID, int emplID)
        {
            DataSet dataSet = new DataSet();
            EmployeeCounterValueTO valueTO = new EmployeeCounterValueTO();

            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM employee_counter_values WHERE employee_counter_type_id = '" + typeID.ToString().Trim() + "' AND employee_id = '" + emplID.ToString().Trim() + "'", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Values");
                DataTable table = dataSet.Tables["Values"];

                if (table.Rows.Count == 1)
                {
                    valueTO = new EmployeeCounterValueTO();
                    valueTO.EmplCounterTypeID = Int32.Parse(table.Rows[0]["employee_counter_type_id"].ToString().Trim());

                    if (!table.Rows[0]["employee_id"].Equals(DBNull.Value))
                    {
                        valueTO.EmplID = Int32.Parse(table.Rows[0]["employee_id"].ToString().Trim());
                    }
                    if (!table.Rows[0]["value"].Equals(DBNull.Value))
                    {
                        valueTO.Value = Int32.Parse(table.Rows[0]["value"].ToString().Trim());
                    }
                    if (!table.Rows[0]["measure_unit"].Equals(DBNull.Value))
                    {
                        valueTO.MeasureUnit = table.Rows[0]["measure_unit"].ToString().Trim();
                    }
                    if (!table.Rows[0]["created_by"].Equals(DBNull.Value))
                    {
                        valueTO.CreatedBy = table.Rows[0]["created_by"].ToString().Trim();
                    }
                    if (!table.Rows[0]["created_time"].Equals(DBNull.Value))
                    {
                        valueTO.CreatedTime = DateTime.Parse(table.Rows[0]["created_time"].ToString().Trim());
                    }
                    if (!table.Rows[0]["modified_by"].Equals(DBNull.Value))
                    {
                        valueTO.ModifiedBy = table.Rows[0]["modified_by"].ToString().Trim();
                    }
                    if (!table.Rows[0]["modified_time"].Equals(DBNull.Value))
                    {
                        valueTO.ModifiedTime = DateTime.Parse(table.Rows[0]["modified_time"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return valueTO;
        }
        public List<EmployeeCounterValueTO> getModifiedValues(DateTime fromDate)
        {
            DataSet dataSet = new DataSet();
            EmployeeCounterValueTO valueTO = new EmployeeCounterValueTO();
            List<EmployeeCounterValueTO> valuesList = new List<EmployeeCounterValueTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employee_counter_values ");
                if (fromDate != new DateTime())
                {
                    sb.Append(" WHERE");
                    sb.Append(" modified_time >= '" + fromDate.Date.ToString(dateTimeformat) + "' AND");
                   

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY employee_counter_type_id, employee_id ";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Values");
                DataTable table = dataSet.Tables["Values"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        valueTO = new EmployeeCounterValueTO();

                        valueTO.EmplCounterTypeID = Int32.Parse(row["employee_counter_type_id"].ToString().Trim());

                        if (!row["employee_id"].Equals(DBNull.Value))
                        {
                            valueTO.EmplID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (!row["value"].Equals(DBNull.Value))
                        {
                            valueTO.Value = Int32.Parse(row["value"].ToString().Trim());
                        }
                        if (!row["measure_unit"].Equals(DBNull.Value))
                        {
                            valueTO.MeasureUnit = row["measure_unit"].ToString().Trim();
                        }
                        if (!row["created_by"].Equals(DBNull.Value))
                        {
                            valueTO.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (!row["created_time"].Equals(DBNull.Value))
                        {
                            valueTO.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (!row["modified_by"].Equals(DBNull.Value))
                        {
                            valueTO.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (!row["modified_time"].Equals(DBNull.Value))
                        {
                            valueTO.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }

                        valuesList.Add(valueTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return valuesList;
        }
        public List<EmployeeCounterValueTO> getEmplCounterValues(EmployeeCounterValueTO emplCValueTO)
        {
            DataSet dataSet = new DataSet();
            EmployeeCounterValueTO valueTO = new EmployeeCounterValueTO();
            List<EmployeeCounterValueTO> valuesList = new List<EmployeeCounterValueTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employee_counter_values ");
                if ((emplCValueTO.EmplCounterTypeID != -1) || (emplCValueTO.EmplID != -1) || (emplCValueTO.Value != -1) || (!emplCValueTO.MeasureUnit.Trim().Equals("")))
                {
                    sb.Append(" WHERE");
                    if (emplCValueTO.EmplCounterTypeID != -1)
                    {
                        sb.Append(" employee_counter_type_id = '" + emplCValueTO.EmplCounterTypeID.ToString().Trim() + "' AND");
                    }
                    if (emplCValueTO.EmplID != -1)
                    {
                        sb.Append(" employee_id = '" + emplCValueTO.EmplID.ToString().Trim() + "' AND");
                    }
                    if (emplCValueTO.Value != -1)
                    {
                        sb.Append(" value = '" + emplCValueTO.Value.ToString().Trim() + "' AND");
                    }
                    if (!emplCValueTO.MeasureUnit.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(measure_unit) = N'" + emplCValueTO.MeasureUnit.ToUpper().Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY employee_counter_type_id, employee_id ";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Values");
                DataTable table = dataSet.Tables["Values"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        valueTO = new EmployeeCounterValueTO();

                        valueTO.EmplCounterTypeID = Int32.Parse(row["employee_counter_type_id"].ToString().Trim());

                        if (!row["employee_id"].Equals(DBNull.Value))
                        {
                            valueTO.EmplID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (!row["value"].Equals(DBNull.Value))
                        {
                            valueTO.Value = Int32.Parse(row["value"].ToString().Trim());
                        }
                        if (!row["measure_unit"].Equals(DBNull.Value))
                        {
                            valueTO.MeasureUnit = row["measure_unit"].ToString().Trim();
                        }
                        if (!row["created_by"].Equals(DBNull.Value))
                        {
                            valueTO.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (!row["created_time"].Equals(DBNull.Value))
                        {
                            valueTO.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (!row["modified_by"].Equals(DBNull.Value))
                        {
                            valueTO.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (!row["modified_time"].Equals(DBNull.Value))
                        {
                            valueTO.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }

                        valuesList.Add(valueTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return valuesList;
        }

        public List<EmployeeCounterValueTO> getEmplCounterValuesNegative(string emplIDs)
        {
            DataSet dataSet = new DataSet();
            EmployeeCounterValueTO valueTO = new EmployeeCounterValueTO();
            List<EmployeeCounterValueTO> valuesList = new List<EmployeeCounterValueTO>();
            string select = "";

            try
            {
                if (emplIDs.Trim().Length <= 0)
                    return valuesList;

                select = "SELECT * FROM employee_counter_values WHERE employee_id IN (" + emplIDs + ") AND value < 0 ORDER BY employee_counter_type_id, employee_id ";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Values");
                DataTable table = dataSet.Tables["Values"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        valueTO = new EmployeeCounterValueTO();

                        valueTO.EmplCounterTypeID = Int32.Parse(row["employee_counter_type_id"].ToString().Trim());

                        if (!row["employee_id"].Equals(DBNull.Value))
                        {
                            valueTO.EmplID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (!row["value"].Equals(DBNull.Value))
                        {
                            valueTO.Value = Int32.Parse(row["value"].ToString().Trim());
                        }
                        if (!row["measure_unit"].Equals(DBNull.Value))
                        {
                            valueTO.MeasureUnit = row["measure_unit"].ToString().Trim();
                        }
                        if (!row["created_by"].Equals(DBNull.Value))
                        {
                            valueTO.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (!row["created_time"].Equals(DBNull.Value))
                        {
                            valueTO.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (!row["modified_by"].Equals(DBNull.Value))
                        {
                            valueTO.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (!row["modified_time"].Equals(DBNull.Value))
                        {
                            valueTO.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }

                        valuesList.Add(valueTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return valuesList;
        }

        public Dictionary<int, Dictionary<int, int>> getEmplCounterValues(string emplIDs)
        {
            DataSet dataSet = new DataSet();
            EmployeeCounterValueTO valueTO = new EmployeeCounterValueTO();
            Dictionary<int, Dictionary<int, int>> emplValues = new Dictionary<int, Dictionary<int, int>>();
            string select = "";

            try
            {
                if (emplIDs.Length > 0)
                {
                    select = "SELECT * FROM employee_counter_values WHERE employee_id IN (" + emplIDs.Trim() + ") ORDER BY employee_id, employee_counter_type_id";

                    MySqlCommand cmd = new MySqlCommand(select, conn);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "Values");
                    DataTable table = dataSet.Tables["Values"];

                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            valueTO = new EmployeeCounterValueTO();

                            valueTO.EmplCounterTypeID = Int32.Parse(row["employee_counter_type_id"].ToString().Trim());

                            if (!row["employee_id"].Equals(DBNull.Value))
                            {
                                valueTO.EmplID = Int32.Parse(row["employee_id"].ToString().Trim());
                            }
                            if (!row["value"].Equals(DBNull.Value))
                            {
                                valueTO.Value = Int32.Parse(row["value"].ToString().Trim());
                            }
                            if (!row["measure_unit"].Equals(DBNull.Value))
                            {
                                valueTO.MeasureUnit = row["measure_unit"].ToString().Trim();
                            }
                            if (!row["created_by"].Equals(DBNull.Value))
                            {
                                valueTO.CreatedBy = row["created_by"].ToString().Trim();
                            }
                            if (!row["created_time"].Equals(DBNull.Value))
                            {
                                valueTO.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                            }
                            if (!row["modified_by"].Equals(DBNull.Value))
                            {
                                valueTO.ModifiedBy = row["modified_by"].ToString().Trim();
                            }
                            if (!row["modified_time"].Equals(DBNull.Value))
                            {
                                valueTO.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                            }

                            if (!emplValues.ContainsKey(valueTO.EmplID))
                                emplValues.Add(valueTO.EmplID, new Dictionary<int, int>());

                            if (!emplValues[valueTO.EmplID].ContainsKey(valueTO.EmplCounterTypeID))
                                emplValues[valueTO.EmplID].Add(valueTO.EmplCounterTypeID, valueTO.Value);
                            else
                                emplValues[valueTO.EmplID][valueTO.EmplCounterTypeID] = valueTO.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return emplValues;
        }

        public Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> getEmplCounterValuesTO(string emplIDs)
        {
            DataSet dataSet = new DataSet();
            EmployeeCounterValueTO valueTO = new EmployeeCounterValueTO();
            Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplValues = new Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>();
            string select = "";

            try
            {
                if (emplIDs.Length > 0)
                {
                    select = "SELECT * FROM employee_counter_values WHERE employee_id IN (" + emplIDs.Trim() + ") ORDER BY employee_id, employee_counter_type_id";

                    MySqlCommand cmd = new MySqlCommand(select, conn);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "Values");
                    DataTable table = dataSet.Tables["Values"];

                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            valueTO = new EmployeeCounterValueTO();

                            valueTO.EmplCounterTypeID = Int32.Parse(row["employee_counter_type_id"].ToString().Trim());

                            if (!row["employee_id"].Equals(DBNull.Value))
                            {
                                valueTO.EmplID = Int32.Parse(row["employee_id"].ToString().Trim());
                            }
                            if (!row["value"].Equals(DBNull.Value))
                            {
                                valueTO.Value = Int32.Parse(row["value"].ToString().Trim());
                            }
                            if (!row["measure_unit"].Equals(DBNull.Value))
                            {
                                valueTO.MeasureUnit = row["measure_unit"].ToString().Trim();
                            }
                            if (!row["created_by"].Equals(DBNull.Value))
                            {
                                valueTO.CreatedBy = row["created_by"].ToString().Trim();
                            }
                            if (!row["created_time"].Equals(DBNull.Value))
                            {
                                valueTO.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                            }
                            if (!row["modified_by"].Equals(DBNull.Value))
                            {
                                valueTO.ModifiedBy = row["modified_by"].ToString().Trim();
                            }
                            if (!row["modified_time"].Equals(DBNull.Value))
                            {
                                valueTO.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                            }

                            if (!emplValues.ContainsKey(valueTO.EmplID))
                                emplValues.Add(valueTO.EmplID, new Dictionary<int, EmployeeCounterValueTO>());

                            if (!emplValues[valueTO.EmplID].ContainsKey(valueTO.EmplCounterTypeID))
                                emplValues[valueTO.EmplID].Add(valueTO.EmplCounterTypeID, valueTO);
                            else
                                emplValues[valueTO.EmplID][valueTO.EmplCounterTypeID] = valueTO;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return emplValues;
        }

        public Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> getEmplCounterValuesTO(string emplIDs, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            EmployeeCounterValueTO valueTO = new EmployeeCounterValueTO();
            Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplValues = new Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>();
            string select = "";

            try
            {
                if (emplIDs.Length > 0)
                {
                    select = "SELECT * FROM employee_counter_values WHERE employee_id IN (" + emplIDs.Trim() + ") ORDER BY employee_id, employee_counter_type_id";

                    MySqlCommand cmd = new MySqlCommand(select, conn, (MySqlTransaction)trans);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "Values");
                    DataTable table = dataSet.Tables["Values"];

                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            valueTO = new EmployeeCounterValueTO();

                            valueTO.EmplCounterTypeID = Int32.Parse(row["employee_counter_type_id"].ToString().Trim());

                            if (!row["employee_id"].Equals(DBNull.Value))
                            {
                                valueTO.EmplID = Int32.Parse(row["employee_id"].ToString().Trim());
                            }
                            if (!row["value"].Equals(DBNull.Value))
                            {
                                valueTO.Value = Int32.Parse(row["value"].ToString().Trim());
                            }
                            if (!row["measure_unit"].Equals(DBNull.Value))
                            {
                                valueTO.MeasureUnit = row["measure_unit"].ToString().Trim();
                            }
                            if (!row["created_by"].Equals(DBNull.Value))
                            {
                                valueTO.CreatedBy = row["created_by"].ToString().Trim();
                            }
                            if (!row["created_time"].Equals(DBNull.Value))
                            {
                                valueTO.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                            }
                            if (!row["modified_by"].Equals(DBNull.Value))
                            {
                                valueTO.ModifiedBy = row["modified_by"].ToString().Trim();
                            }
                            if (!row["modified_time"].Equals(DBNull.Value))
                            {
                                valueTO.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                            }

                            if (!emplValues.ContainsKey(valueTO.EmplID))
                                emplValues.Add(valueTO.EmplID, new Dictionary<int, EmployeeCounterValueTO>());

                            if (!emplValues[valueTO.EmplID].ContainsKey(valueTO.EmplCounterTypeID))
                                emplValues[valueTO.EmplID].Add(valueTO.EmplCounterTypeID, valueTO);
                            else
                                emplValues[valueTO.EmplID][valueTO.EmplCounterTypeID] = valueTO;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return emplValues;
        }

        public Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> getEmplCounterValuesAll()
        {
            DataSet dataSet = new DataSet();
            EmployeeCounterValueTO valueTO = new EmployeeCounterValueTO();
            Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplValues = new Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>();
            string select = "";

            try
            {
                select = "SELECT * FROM employee_counter_values ORDER BY employee_id, employee_counter_type_id";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Values");
                DataTable table = dataSet.Tables["Values"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        valueTO = new EmployeeCounterValueTO();

                        valueTO.EmplCounterTypeID = Int32.Parse(row["employee_counter_type_id"].ToString().Trim());

                        if (!row["employee_id"].Equals(DBNull.Value))
                        {
                            valueTO.EmplID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (!row["value"].Equals(DBNull.Value))
                        {
                            valueTO.Value = Int32.Parse(row["value"].ToString().Trim());
                        }
                        if (!row["measure_unit"].Equals(DBNull.Value))
                        {
                            valueTO.MeasureUnit = row["measure_unit"].ToString().Trim();
                        }
                        if (!row["created_by"].Equals(DBNull.Value))
                        {
                            valueTO.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (!row["created_time"].Equals(DBNull.Value))
                        {
                            valueTO.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (!row["modified_by"].Equals(DBNull.Value))
                        {
                            valueTO.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (!row["modified_time"].Equals(DBNull.Value))
                        {
                            valueTO.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }

                        if (!emplValues.ContainsKey(valueTO.EmplID))
                            emplValues.Add(valueTO.EmplID, new Dictionary<int, EmployeeCounterValueTO>());

                        if (!emplValues[valueTO.EmplID].ContainsKey(valueTO.EmplCounterTypeID))
                            emplValues[valueTO.EmplID].Add(valueTO.EmplCounterTypeID, valueTO);
                        else
                            emplValues[valueTO.EmplID][valueTO.EmplCounterTypeID] = valueTO;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return emplValues;
        }

        public Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> getEmplCounterValuesOrderedByName(string emplIDs)
        {
            DataSet dataSet = new DataSet();
            EmployeeCounterValueTO valueTO = new EmployeeCounterValueTO();
            Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplValues = new Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>();
            string select = "";

            try
            {
                if (emplIDs.Length > 0)
                {
                    select = "SELECT e.last_name, e.first_name, ec.* FROM employees e, employee_counter_values ec WHERE ec.employee_id IN ("
                        + emplIDs.Trim() + ") AND e.employee_id = ec.employee_id ORDER BY e.last_name, e.first_name, ec.employee_counter_type_id";

                    MySqlCommand cmd = new MySqlCommand(select, conn);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "Values");
                    DataTable table = dataSet.Tables["Values"];

                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            valueTO = new EmployeeCounterValueTO();

                            valueTO.EmplCounterTypeID = Int32.Parse(row["employee_counter_type_id"].ToString().Trim());

                            if (!row["employee_id"].Equals(DBNull.Value))
                            {
                                valueTO.EmplID = Int32.Parse(row["employee_id"].ToString().Trim());
                            }
                            if (!row["value"].Equals(DBNull.Value))
                            {
                                valueTO.Value = Int32.Parse(row["value"].ToString().Trim());
                            }
                            if (!row["measure_unit"].Equals(DBNull.Value))
                            {
                                valueTO.MeasureUnit = row["measure_unit"].ToString().Trim();
                            }
                            if (!row["created_by"].Equals(DBNull.Value))
                            {
                                valueTO.CreatedBy = row["created_by"].ToString().Trim();
                            }
                            if (!row["created_time"].Equals(DBNull.Value))
                            {
                                valueTO.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                            }
                            if (!row["modified_by"].Equals(DBNull.Value))
                            {
                                valueTO.ModifiedBy = row["modified_by"].ToString().Trim();
                            }
                            if (!row["modified_time"].Equals(DBNull.Value))
                            {
                                valueTO.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                            }

                            if (!emplValues.ContainsKey(valueTO.EmplID))
                                emplValues.Add(valueTO.EmplID, new Dictionary<int, EmployeeCounterValueTO>());

                            if (!emplValues[valueTO.EmplID].ContainsKey(valueTO.EmplCounterTypeID))
                                emplValues[valueTO.EmplID].Add(valueTO.EmplCounterTypeID, valueTO);
                            else
                                emplValues[valueTO.EmplID][valueTO.EmplCounterTypeID] = valueTO;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return emplValues;
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
