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
    public class MSSQLEmployeePYTransportDataDAO : EmployeePYTransportDataDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLEmployeePYTransportDataDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MSSQLEmployeePYTransportDataDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(EmployeePYTransportDataTO dataTO, bool doCommit)
        {
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employee_py_transport_data ");
                sbInsert.Append("(employee_id, date, transport_type_id, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("'" + dataTO.EmployeeID.ToString().Trim() + "', ");
                sbInsert.Append("'" + dataTO.Date.ToString(dateTimeformat) + "', ");
                sbInsert.Append("'" + dataTO.TransportTypeID.ToString().Trim() + "', ");
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

        public bool delete(string emplIDs, DateTime from, DateTime to, bool doCommit)
        {
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            int rowsAffected = 0;

            try
            {
                StringBuilder sb = new StringBuilder();

                if (emplIDs.Trim().Length <= 0)
                    return false;

                sb.Append("DELETE FROM employee_py_transport_data WHERE employee_id IN (" + emplIDs.Trim() + ")");
                
                if (from != new DateTime())
                    sb.Append(" AND date >= '" + from.Date.ToString(dateTimeformat) + "'");

                if (to != new DateTime())
                    sb.Append(" AND date < '" + to.Date.AddDays(1).ToString(dateTimeformat) + "'");

                SqlCommand cmd = new SqlCommand(sb.ToString(), conn, SqlTrans);

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

            return rowsAffected >= 0;
        }

        public Dictionary<int, Dictionary<DateTime, EmployeePYTransportDataTO>> getEmplTransportData(string emplIDs, DateTime month)
        {
            DataSet dataSet = new DataSet();
            EmployeePYTransportDataTO dataTO = new EmployeePYTransportDataTO();
            Dictionary<int, Dictionary<DateTime, EmployeePYTransportDataTO>> dataDict = new Dictionary<int, Dictionary<DateTime, EmployeePYTransportDataTO>>();
            string select = "";

            try
            {
                select = "SELECT * FROM employee_py_transport_data";

                if (emplIDs.Trim() != "" || month != new DateTime())
                {
                    select += " WHERE";

                    if (emplIDs.Trim() != "")
                        select += " employee_id IN (" + emplIDs.Trim() + ") AND";

                    if (month != new DateTime())
                    {
                        DateTime firstDay = new DateTime(month.Year, month.Month, 1);
                        DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);

                        select += " date >= '" + firstDay.Date.ToString(dateTimeformat) + "' AND date < '" + lastDay.Date.AddDays(1).ToString(dateTimeformat) + "' AND";
                    }

                    select = select.Substring(0, select.Length - 3);
                }
                
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Data");
                DataTable table = dataSet.Tables["Data"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        dataTO = new EmployeePYTransportDataTO();

                        if (!row["employee_id"].Equals(DBNull.Value))
                        {
                            dataTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        
                        if (!row["date"].Equals(DBNull.Value))
                        {
                            dataTO.Date = DateTime.Parse(row["date"].ToString().Trim());
                        }
                        if (!row["transport_type_id"].Equals(DBNull.Value))
                        {
                            dataTO.TransportTypeID = Int32.Parse(row["transport_type_id"].ToString().Trim());
                        }

                        if (!dataDict.ContainsKey(dataTO.EmployeeID))
                            dataDict.Add(dataTO.EmployeeID, new Dictionary<DateTime, EmployeePYTransportDataTO>());

                        if (!dataDict[dataTO.EmployeeID].ContainsKey(dataTO.Date.Date))
                            dataDict[dataTO.EmployeeID].Add(dataTO.Date.Date, dataTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dataDict;
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
    }
}
