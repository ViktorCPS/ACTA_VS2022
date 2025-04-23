using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

using TransferObjects;

namespace DataAccess
{
    public class MSSQLEmployeePhysicalDataDAO : EmployeePhysicalDataDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }
        public MSSQLEmployeePhysicalDataDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MSSQLEmployeePhysicalDataDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(EmployeePhysicalDataTO dataTO, bool doCommit)
        {
            int rowsAffected = 0;

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
                
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employee_physical_data ");
                sbInsert.Append("(employee_id, date_performed, weight, height, created_by, created_time) ");
                sbInsert.Append("VALUES (");                
                sbInsert.Append("'" + dataTO.EmployeeID.ToString().Trim() + "', ");
                sbInsert.Append("'" + dataTO.DatePerformed.ToString(dateTimeformat) + "', ");
                sbInsert.Append("'" + dataTO.Weight.ToString().Trim().Replace(',', '.') + "', ");
                sbInsert.Append("'" + dataTO.Height.ToString().Trim().Replace(',', '.') + "', ");
                if (!dataTO.CreatedBy.Trim().Equals(""))
                    sbInsert.Append("N'" + dataTO.CreatedBy.Trim() + "', ");
                else
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!dataTO.CreatedTime.Equals(new DateTime()))
                    sbInsert.Append("'" + dataTO.CreatedTime.ToString(dateTimeformat) + "') ");
                else
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

        public bool update(EmployeePhysicalDataTO dataTO, bool doCommit)
        {
            bool isUpdated = false;

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
                
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE employee_physical_data SET ");
                if (dataTO.EmployeeID != -1)
                    sbUpdate.Append("employee_id = '" + dataTO.EmployeeID.ToString().Trim() + "', ");
                if (!dataTO.DatePerformed.Equals(new DateTime()))
                    sbUpdate.Append("date_performed = '" + dataTO.DatePerformed.ToString(dateTimeformat) + "', ");
                if (dataTO.Weight != -1)
                    sbUpdate.Append("weight = '" + dataTO.Weight.ToString().Trim().Replace(',', '.') + "', ");
                if (dataTO.Height != -1)
                    sbUpdate.Append("height = '" + dataTO.Height.ToString().Trim().Replace(',', '.') + "', ");
                if (!dataTO.ModifiedBy.Trim().Equals(""))
                    sbUpdate.Append("modified_by = N'" + dataTO.ModifiedBy.Trim() + "', ");
                else
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!dataTO.ModifiedTime.Equals(new DateTime()))
                    sbUpdate.Append("modified_time = '" + dataTO.ModifiedTime.ToString(dateTimeformat) + "' ");
                else
                    sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE rec_id = '" + dataTO.RecID.ToString().Trim() + "'");

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
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool delete(string recID, bool doCommit)
        {
            bool isDeleted = false;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM employee_physical_data WHERE rec_id IN (" + recID.ToString().Trim() + ")");

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, SqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res >= 0)
                {
                    isDeleted = true;
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

        public List<EmployeePhysicalDataTO> getEmployeePhysicalData(EmployeePhysicalDataTO dataTO)
        {
            DataSet dataSet = new DataSet();
            EmployeePhysicalDataTO data = new EmployeePhysicalDataTO();
            List<EmployeePhysicalDataTO> dataList = new List<EmployeePhysicalDataTO>();
            string select = "";
            
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employee_physical_data ");
                if ((dataTO.RecID != 0) || (dataTO.EmployeeID != -1) || (!dataTO.DatePerformed.Equals(new DateTime()))
                    || (dataTO.Weight != -1) || (dataTO.Height != -1))
                {
                    sb.Append(" WHERE");

                    if (dataTO.RecID != 0)
                    {
                        sb.Append(" rec_id = '" + dataTO.RecID.ToString().Trim() + "' AND");
                    }
                    if (dataTO.EmployeeID != -1)
                    {
                        sb.Append(" employee_id = '" + dataTO.EmployeeID.ToString().Trim() + "' AND");
                    }
                    if (!dataTO.DatePerformed.Equals(new DateTime()))
                    {
                        sb.Append(" date_performed = '" + dataTO.DatePerformed.ToString(dateTimeformat) + "' AND");
                    }
                    if (dataTO.Weight != -1)
                    {
                        sb.Append(" weight = '" + dataTO.Weight.ToString().Trim().Replace(',', '.') + "' AND");
                    }
                    if (dataTO.Height != -1)
                    {
                        sb.Append(" height = '" + dataTO.Height.ToString().Trim().Replace(',', '.') + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                SqlCommand cmd = new SqlCommand(select + " ORDER BY employee_id, date_performed", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Data");
                DataTable table = dataSet.Tables["Data"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        data = new EmployeePhysicalDataTO();
                        if (row["rec_id"] != DBNull.Value)
                        {
                            data.RecID = UInt32.Parse(row["rec_id"].ToString().Trim());
                        }                        
                        if (row["employee_id"] != DBNull.Value)
                        {
                            data.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["date_performed"] != DBNull.Value)
                        {
                            data.DatePerformed = DateTime.Parse(row["date_performed"].ToString().Trim());
                        }
                        if (row["weight"] != DBNull.Value)
                        {
                            data.Weight = Decimal.Parse(row["weight"].ToString().Trim());
                        }
                        if (row["height"] != DBNull.Value)
                        {
                            data.Height = Decimal.Parse(row["height"].ToString().Trim());
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            data.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            data.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            data.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            data.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }

                        dataList.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return dataList;
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
