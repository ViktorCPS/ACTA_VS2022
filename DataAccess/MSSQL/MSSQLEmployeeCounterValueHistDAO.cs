using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;

using TransferObjects;
using Util;

namespace DataAccess
{
    public class MSSQLEmployeeCounterValueHistDAO : EmployeeCounterValueHistDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLEmployeeCounterValueHistDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MSSQLEmployeeCounterValueHistDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
             DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public int insert(EmployeeCounterValueHistTO emplCValueTO, bool doCommit)
        {
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employee_counter_values_hist ");
                sbInsert.Append("(employee_counter_type_id, employee_id, value, measure_unit, created_by, created_time, modified_by, modified_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("'" + emplCValueTO.EmplCounterTypeID.ToString().Trim() + "', ");
                sbInsert.Append("'" + emplCValueTO.EmplID.ToString().Trim() + "', ");
                sbInsert.Append("'" + emplCValueTO.Value.ToString().Trim() + "', ");
                sbInsert.Append("N'" + emplCValueTO.MeasureUnit.Trim() + "', ");
                sbInsert.Append("N'" + emplCValueTO.CreatedBy.Trim() + "', ");
                if (!emplCValueTO.CreatedTime.Equals(new DateTime()))
                    sbInsert.Append("'" + emplCValueTO.CreatedTime.ToString(dateTimeformat) + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (!emplCValueTO.ModifiedBy.Trim().Equals(""))
                    sbInsert.Append("N'" + emplCValueTO.ModifiedBy.Trim() + "', ");
                else
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbInsert.Append("GETDATE()) ");
                                
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

        public int insert(int emplID, string modifiedBy, bool doCommit)
        {
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employee_counter_values_hist ");
                sbInsert.Append("(employee_counter_type_id, employee_id, value, measure_unit, created_by, created_time, modified_by, modified_time) ");
                sbInsert.Append("SELECT employee_counter_type_id, employee_id, value, measure_unit, created_by, created_time, '" + modifiedBy.Trim() + "', GETDATE()");
                sbInsert.Append("FROM employee_counter_values WHERE employee_id = '" + emplID.ToString().Trim() + "'");                

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
                
        public List<EmployeeCounterValueHistTO> getEmplCounterValuesHist(EmployeeCounterValueHistTO emplCValueTO)
        {
            DataSet dataSet = new DataSet();
            EmployeeCounterValueHistTO valueTO = new EmployeeCounterValueHistTO();
            List<EmployeeCounterValueHistTO> valuesList = new List<EmployeeCounterValueHistTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employee_counter_values_hist ");
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

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Values");
                DataTable table = dataSet.Tables["Values"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        valueTO = new EmployeeCounterValueHistTO();

                        valueTO.RecID = uint.Parse(row["rec_id"].ToString().Trim());
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

        public int getFirstModifiedValue(int emplID, int counterType)
        {
            DataSet dataSet = new DataSet();            
            int value = -1;
            string select = "";

            try
            {
                select = "SELECT TOP 1 * FROM employee_counter_values_hist WHERE employee_id = '" + emplID.ToString().Trim() + "' AND employee_counter_type_id = '" 
                    + counterType.ToString().Trim() + "' AND modified_by <> '" + Constants.syncUser.Trim() + "' ORDER BY modified_time";                

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Values");
                DataTable table = dataSet.Tables["Values"];

                if (table.Rows.Count == 1)
                {
                    if (!table.Rows[0]["value"].Equals(DBNull.Value))
                    {
                        value = Int32.Parse(table.Rows[0]["value"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return value;
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
