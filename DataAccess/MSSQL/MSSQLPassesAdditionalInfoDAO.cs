using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using TransferObjects;
using Util;


namespace DataAccess.MSSQL
{
    public class MSSQLPassesAdditionalInfoDAO : PassesAdditionalInfoDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLPassesAdditionalInfoDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
		}

        public MSSQLPassesAdditionalInfoDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
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

        public void setTransaction(IDbTransaction trans)
        {
            _sqlTrans = (SqlTransaction)trans;
        }

        public IDbTransaction getTransaction()
        {
            return _sqlTrans;
        }

        public PassesAdditionalInfoTO find(uint passID)
        {
            DataSet dataSet = new DataSet();
            PassesAdditionalInfoTO pass = new PassesAdditionalInfoTO();
            try
            {
                string select = "SELECT * FROM passes_additional_info "
                    + "WHERE pass_id = '" + passID + "'";

                SqlCommand cmd = new SqlCommand(select, conn);
                cmd.CommandTimeout = Constants.commandTimeout;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Pass");
                DataTable table = dataSet.Tables["Pass"];

                if (table.Rows.Count == 1)
                {
                    pass = new PassesAdditionalInfoTO();
                    pass.PassID = UInt32.Parse(table.Rows[0]["pass_id"].ToString().Trim());


                    if (table.Rows[0]["gps_data"] != DBNull.Value)
                    {
                        pass.GpsData = table.Rows[0]["gps_data"].ToString().Trim();
                    }
                    if (table.Rows[0]["cardholder_name"] != DBNull.Value)
                    {
                        pass.CardholderName = table.Rows[0]["cardholder_name"].ToString().Trim();
                    }
                    if (table.Rows[0]["cardholder_id"] != DBNull.Value)
                    {
                        pass.CardholderID = Int32.Parse(table.Rows[0]["cardholder_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["created_by"] != DBNull.Value)
                    {
                        pass.CreatedBy = table.Rows[0]["created_by"].ToString().Trim();
                    }
                    if (table.Rows[0]["created_time"] != DBNull.Value)
                    {
                        pass.CreatedTime = (DateTime)table.Rows[0]["created_time"];
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pass;
        }

        public int insert(PassesAdditionalInfoTO passesAdditionalInfoTO, bool doCommit)
        {
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO passes_additional_info ");
                sbInsert.Append("(pass_id, gps_data, cardholder_name, cardholder_id, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                if (passesAdditionalInfoTO.PassID != 0)
                {
                    sbInsert.Append("'" + passesAdditionalInfoTO.PassID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (!passesAdditionalInfoTO.GpsData.Trim().Equals(""))
                {
                    sbInsert.Append("N'" + passesAdditionalInfoTO.GpsData.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (!passesAdditionalInfoTO.CardholderName.Equals(null))
                {
                    sbInsert.Append("'" + passesAdditionalInfoTO.CardholderName.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (passesAdditionalInfoTO.CardholderID != -1)
                {
                    sbInsert.Append("'" + passesAdditionalInfoTO.CardholderID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append(" N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);

                rowsAffected = cmd.ExecuteNonQuery();
                sqlTrans.Commit();
            }
            catch (SqlException sqlEx)
            {
                sqlTrans.Rollback("INSERT");
                if (sqlEx.Number == 2627)
                {
                    throw new Exception(sqlEx.Number.ToString());
                }
                else
                {
                    throw new Exception(sqlEx.Message);
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("INSERT");
                throw ex;
            }

            return rowsAffected;
        }

        public bool update(PassesAdditionalInfoTO passesAdditionalInfoTO, bool doCommit)
        {
            bool isUpdated = false;
            SqlTransaction sqlTrans = null;

            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE passes_additional_info SET ");
                sbUpdate.Append("gps_data = '" + passesAdditionalInfoTO.GpsData + "', ");
                sbUpdate.Append("cardholder_name = '" + passesAdditionalInfoTO.CardholderName + "', ");
                sbUpdate.Append("cardholder_id = '" + passesAdditionalInfoTO.CardholderID + "', ");
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE pass_id = '" + passesAdditionalInfoTO.PassID + "' ");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res >= 0)
                {
                    isUpdated = true;
                }

                if (doCommit)
                {
                    sqlTrans.Commit();
                }
            }
            catch (SqlException sqlEx)
            {
                isUpdated = false;
                if (doCommit)
                {
                    sqlTrans.Rollback("UPDATE");
                }
                else
                {
                    sqlTrans.Rollback();
                }
                if (sqlEx.Number == 2627)
                {
                    DataProcessingException procEx = new DataProcessingException(sqlEx.Message, 14);
                    throw procEx;
                }
                else
                {
                    DataProcessingException procEx = new DataProcessingException(sqlEx.Message, 11);
                    throw procEx;
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback("UPDATE");
                }
                else
                {
                    sqlTrans.Rollback();
                }
                DataProcessingException procEx = new DataProcessingException(ex.Message, 11);
                throw procEx;
            }

            return isUpdated;
        }

        public List<PassesAdditionalInfoTO> getPasses(PassesAdditionalInfoTO passesAdditionalInfoTO)
        {
            DataSet dataSet = new DataSet();
            PassesAdditionalInfoTO pass = new PassesAdditionalInfoTO();
            List<PassesAdditionalInfoTO> passesList = new List<PassesAdditionalInfoTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM passes_additional_info ");

                if ((passesAdditionalInfoTO.PassID != 0) || (!passesAdditionalInfoTO.GpsData.Equals(null)) || (!passesAdditionalInfoTO.CardholderName.Equals(null)) ||
                    (passesAdditionalInfoTO.CardholderID!= -1))
                {
                    sb.Append(" WHERE ");

                    if (passesAdditionalInfoTO.PassID != 0)
                    {
                        //sb.Append(" UPPER(pass_id) LIKE '" + passID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" pass_id = '" + passesAdditionalInfoTO.PassID.ToString().Trim() + "' AND");
                    }

                    if (!passesAdditionalInfoTO.GpsData.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(gps_data) LIKE N'%" + passesAdditionalInfoTO.GpsData.Trim().ToUpper() + "%' AND");
                    }

                    if (!passesAdditionalInfoTO.CardholderName.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(cardholder_name) LIKE N'%" + passesAdditionalInfoTO.CardholderName.Trim().ToUpper() + "%' AND");
                    }

                    if (passesAdditionalInfoTO.CardholderID != -1)
                    {
                        //sb.Append(" UPPER(employee_id) LIKE '" + employeeID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" cardholderID = '" + passesAdditionalInfoTO.CardholderID.ToString().Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                SqlCommand cmd = new SqlCommand(select, conn);
                cmd.CommandTimeout = Constants.commandTimeout;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Pass");
                DataTable table = dataSet.Tables["Pass"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pass = new PassesAdditionalInfoTO();
                        pass.PassID = UInt32.Parse(row["pass_id"].ToString().Trim());

                        if (row["pass_id"] != DBNull.Value)
                        {
                            pass.PassID = UInt32.Parse(row["pass_id"].ToString().Trim());
                        }
                        if (row["gps_data"] != DBNull.Value)
                        {
                            pass.GpsData = row["gps_data"].ToString().Trim();
                        }
                        if (row["cardholder_name"] != DBNull.Value)
                        {
                            pass.CardholderName = row["cardholder_name"].ToString().Trim();
                        }
                        if (row["cardholder_id"] != DBNull.Value)
                        {
                            pass.CardholderID = Int32.Parse(row["cardholder_id"].ToString().Trim());
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            pass.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pass.CreatedTime = (DateTime)row["created_time"];
                        }

                        passesList.Add(pass);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return passesList;
        }

        public bool delete(uint passID, bool doCommit)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = null;
            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM passes_additional_info WHERE pass_id = " + passID);

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    isDeleted = true;
                }

                if (doCommit)
                {
                    sqlTrans.Commit();
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback("INSERT");
                }
                else
                {
                    sqlTrans.Rollback();
                }
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }
    }
}
