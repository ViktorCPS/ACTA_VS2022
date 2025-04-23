using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

using TransferObjects;
using Util;

namespace DataAccess
{
    class MSSQLLicenceDAO : LicenceDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public MSSQLLicenceDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public MSSQLLicenceDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public int insert(string licenceKey, bool doCommit)
        {
            SqlTransaction sqlTrans;

            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO licence ");
                sbInsert.Append("(licence_key, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                                
                if (!licenceKey.Trim().Equals(""))
                {
                    sbInsert.Append("N'" + licenceKey + "', ");
                }
                else
                {
                    sbInsert.Append("'" + Constants.licenceKeyValue + "', ");
                }

                sbInsert.Append(" N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit)
                {
                    sqlTrans.Commit();
                }

            }
            catch (SqlException sqlEx)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback("INSERT");
                }

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

        public bool delete(string licenceKey)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM licence WHERE licence_key = '" + licenceKey + "'" );

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    isDeleted = true;
                }
                
                if (isDeleted)
                {
                    sqlTrans.Commit();
                }
                else
                {
                    sqlTrans.Rollback("DELETE");
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("DELETE");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public bool update(int recID, string licenceKey, bool doCommit)
        {
            bool isUpdated = false;
            SqlTransaction sqlTrans;

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
                sbUpdate.Append("UPDATE licence SET ");
                               
                if (!licenceKey.Trim().Equals(""))
                {
                    sbUpdate.Append("licence_key = N'" + licenceKey + "', ");
                }
                else
                {
                    sbUpdate.Append("licence_key = '" + Constants.licenceKeyValue + "', ");
                }
             
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE rec_id = '" + recID.ToString().Trim() + "'");

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
            catch (SqlException sqlEx)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback("UPDATE");
                }

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
                sqlTrans.Rollback("UPDATE");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public LicenceTO findMAX()
        {
            DataSet dataSet = new DataSet();
            LicenceTO licence = new LicenceTO();
            try
            {
                string select = "SELECT * FROM licence WHERE rec_id = (SELECT MAX(rec_id) FROM licence)";

                SqlCommand cmd;
                if (this.SqlTrans != null)
                {
                    cmd = new SqlCommand(select, conn, this.SqlTrans);
                }
                else
                {
                    cmd = new SqlCommand(select, conn);
                }

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Licence");
                DataTable table = dataSet.Tables["Licence"];

                if (table.Rows.Count == 1)
                {
                    licence = new LicenceTO();
                    licence.RecID = Int32.Parse(table.Rows[0]["rec_id"].ToString().Trim());

                    if (table.Rows[0]["licence_key"] != DBNull.Value)
                    {
                        licence.LicenceKey = table.Rows[0]["licence_key"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return licence;
        }

        public List<LicenceTO> getLicences(LicenceTO licTO)
        {
            DataSet dataSet = new DataSet();
            LicenceTO licence = new LicenceTO();
            List<LicenceTO> licencesList = new List<LicenceTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM licence");

                if ((licTO.RecID != -1) || (!licTO.LicenceKey.Trim().Equals("")))
                {
                    sb.Append(" WHERE ");

                    if (licTO.RecID != -1)
                    {
                        sb.Append(" rec_id = '" + licTO.RecID.ToString().Trim().ToUpper() + "' AND");
                    }

                    if (!licTO.LicenceKey.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(licence_key) LIKE N'" + licTO.LicenceKey.ToUpper() + "' AND");
                    }
                    
                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Licence");
                DataTable table = dataSet.Tables["Licence"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        licence = new LicenceTO();
                        licence.RecID = Int32.Parse(row["rec_id"].ToString().Trim());

                        if (row["licence_key"] != DBNull.Value)
                        {
                            licence.LicenceKey = row["licence_key"].ToString();
                        }

                        licencesList.Add(licence);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return licencesList;
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
