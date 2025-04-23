using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

using TransferObjects;

namespace DataAccess
{
    public class MSSQLEmployeeLockedDayDAO : EmployeeLockedDayDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }
        public MSSQLEmployeeLockedDayDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MSSQLEmployeeLockedDayDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(EmployeeLockedDayTO lockedTO, bool doCommit)
        {
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employee_locked_days ");
                sbInsert.Append("(employee_id, date, type, created_by, created_time) ");
                sbInsert.Append("VALUES (");                
                sbInsert.Append(lockedTO.EmployeeID.ToString().Trim() + ", ");
                sbInsert.Append("'" + lockedTO.LockedDate.ToString(dateTimeformat) + "', ");
                if (!lockedTO.Type.Trim().Equals(""))
                    sbInsert.Append("N'" + lockedTO.Type.ToString().Trim() + "', ");                
                else                
                    sbInsert.Append("NULL, ");
                if (!lockedTO.CreatedBy.Trim().Equals(""))
                    sbInsert.Append("N'" + lockedTO.CreatedBy.Trim() + "', ");                
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

        public bool delete(EmployeeLockedDayTO lockedDayTO, bool doCommit)
        {
            bool isDeleted = false;
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                if (lockedDayTO.EmployeeID != -1 || !lockedDayTO.Type.Trim().Equals("") || !lockedDayTO.LockedDate.Equals(new DateTime()))
                {
                    sbDelete.Append("DELETE FROM employee_locked_days WHERE");

                    if (lockedDayTO.EmployeeID != -1)
                    {
                        sbDelete.Append(" employee_id = '" + lockedDayTO.EmployeeID.ToString().Trim() + "' AND");
                    }
                    if (!lockedDayTO.Type.Trim().Equals(""))
                    {
                        sbDelete.Append(" UPPER(type) = N'" + lockedDayTO.Type.Trim().ToUpper() + "' AND");
                    }
                    if (!lockedDayTO.LockedDate.Equals(new DateTime()))
                    {
                        sbDelete.Append(" date = '" + lockedDayTO.LockedDate.Date.ToString(dateTimeformat).Trim() + "' AND");
                    }

                    SqlCommand cmd = new SqlCommand(sbDelete.ToString(0, sbDelete.ToString().Length - 3), conn, SqlTrans);
                    int res = cmd.ExecuteNonQuery();
                    if (res >= 0)
                    {
                        isDeleted = true;
                    }
                }

                if (doCommit)
                    commitTransaction();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    rollbackTransaction();

                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public Dictionary<int, List<DateTime>> getLockedDays(string emplIDs, string type, DateTime fromDate, DateTime toDate)
        {
            DataSet dataSet = new DataSet();
            EmployeeLockedDayTO lockTO = new EmployeeLockedDayTO();
            Dictionary<int, List<DateTime>> emplLockedList = new Dictionary<int, List<DateTime>>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employee_locked_days");

                if (!emplIDs.Trim().Equals("") || !type.Trim().Equals("") || !fromDate.Equals(new DateTime()) || !toDate.Equals(new DateTime()))
                {
                    sb.Append(" WHERE");

                    if (!emplIDs.Trim().Equals(""))
                    {
                        sb.Append(" employee_id IN (" + emplIDs.Trim() + ") AND");
                    }
                    if (!type.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(type) = N'" + type.Trim().ToUpper() + "' AND");
                    }                    
                    if (fromDate != new DateTime())
                    {
                        sb.Append(" date >= '" + fromDate.Date.ToString(dateTimeformat).Trim() + "' AND");
                    }
                    if (toDate != new DateTime())
                    {
                        sb.Append(" date < '" + toDate.Date.AddDays(1).ToString(dateTimeformat).Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                SqlCommand cmd = new SqlCommand(select + " ORDER BY employee_id, type, date", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeLockedDays");
                DataTable table = dataSet.Tables["EmployeeLockedDays"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        lockTO = new EmployeeLockedDayTO();
                        if (row["rec_id"] != DBNull.Value)
                        {
                            lockTO.RecID = UInt32.Parse(row["rec_id"].ToString().Trim());
                        }                        
                        if (row["employee_id"] != DBNull.Value)
                        {
                            lockTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["date"] != DBNull.Value)
                        {
                            lockTO.LockedDate = DateTime.Parse(row["date"].ToString().Trim());
                        }
                        if (row["type"] != DBNull.Value)
                        {
                            lockTO.Type = row["type"].ToString().Trim();
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            lockTO.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            lockTO.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            lockTO.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            lockTO.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }

                        if (!emplLockedList.ContainsKey(lockTO.EmployeeID))
                            emplLockedList.Add(lockTO.EmployeeID, new List<DateTime>());
                        if (!emplLockedList[lockTO.EmployeeID].Contains(lockTO.LockedDate.Date))
                            emplLockedList[lockTO.EmployeeID].Add(lockTO.LockedDate.Date);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return emplLockedList;
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
