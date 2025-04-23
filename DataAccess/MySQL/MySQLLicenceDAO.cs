using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Globalization;

using TransferObjects;
using Util;

namespace DataAccess
{
    class MySQLLicenceDAO : LicenceDAO
    {
        MySqlConnection conn = null;
        MySqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public MySQLLicenceDAO()
        {
            conn = MySQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public MySQLLicenceDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public int insert(string licenceKey, bool doCommit)
        {
            MySqlTransaction sqlTrans;
            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
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

                sbInsert.Append(" N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");

                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit)
                {
                    sqlTrans.Commit();
                }
            }
            catch (MySqlException sqlEx)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback();
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
                sqlTrans.Rollback();
                throw ex;
            }

            return rowsAffected;
        }

        public bool delete(string licenceKey)
        {
            bool isDeleted = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM licence WHERE licence_key = '" + licenceKey + "'" );

                MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, sqlTrans);
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
                    sqlTrans.Rollback();
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public bool update(int recID, string licenceKey, bool doCommit)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans;

            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
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
                sbUpdate.Append("modified_time = NOW() ");
                sbUpdate.Append("WHERE rec_id = '" + recID.ToString().Trim() + "'");

                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, sqlTrans);
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
            catch (MySqlException sqlEx)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback();
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
                sqlTrans.Rollback();
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

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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
            _sqlTrans = (MySqlTransaction)trans;
        }
    }
}
