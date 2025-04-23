using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DataAccess
{
    public class MSSQLApplUsersLoginChangesTblDAO : ApplUsersLoginChangesTblDAO
    {
        SqlConnection conn = null;
		protected string dateTimeformat = "";

		public MSSQLApplUsersLoginChangesTblDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
		}

        public MSSQLApplUsersLoginChangesTblDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;            
        }

        public List<string> getTablesNames()
        {
            DataSet dataSet = new DataSet();            
            List<string> tableNames = new List<string>();
            
            try
            {
                string select = "SELECT * FROM appl_users_login_changes_tbl";
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ApplUsersLog");
                DataTable table = dataSet.Tables["ApplUsersLog"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (!row["tbl"].Equals(DBNull.Value))
                        {
                            tableNames.Add(row["tbl"].ToString().Trim());
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return tableNames;
        }


        #region ApplUsersLoginChangesTblDAO Members


        public List<string> getAllTablesNames()
        {
            DataSet dataSet = new DataSet();
            List<string> tableNames = new List<string>();

            try
            {
                string select = "SELECT * FROM sys.tables";
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ApplUsersLog");
                DataTable table = dataSet.Tables["ApplUsersLog"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            tableNames.Add(row["name"].ToString().Trim());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return tableNames;
        }

        #endregion


        public int insert(List<string> listToSaveToDB)
        {
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            int rowsAffected = 0;

            try
            {
                foreach (string table in listToSaveToDB)
                {
                    StringBuilder sbInsert = new StringBuilder();

                    sbInsert.Append("INSERT INTO appl_users_login_changes_tbl ");
                    sbInsert.Append("(tbl) ");
                    sbInsert.Append("VALUES (");

                    if (table.Trim().Equals(""))
                    {
                        sbInsert.Append("null");
                    }
                    else
                    {
                        sbInsert.Append("N'" + table.Trim() + "' )");
                    }
                    
                    SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                    rowsAffected += cmd.ExecuteNonQuery();
                    sqlTrans.Commit();
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("INSERT");

                throw ex;
            }

            return rowsAffected;
        }


        public bool delete()
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM appl_users_login_changes_tbl");

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    isDeleted = true;
                }
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("DELETE");

                throw new Exception(ex.Message);
            }
            return isDeleted;
        }


        public int insert(string table)
        {
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            int rowsAffected = 0;

            try
            {
               StringBuilder sbInsert = new StringBuilder();

                sbInsert.Append("INSERT INTO appl_users_login_changes_tbl ");
                sbInsert.Append("(tbl) ");
                sbInsert.Append("VALUES (");

                if (table.Trim().Equals(""))
                {
                    sbInsert.Append("null");
                }
                else
                {
                    sbInsert.Append("N'" + table.Trim() + "' )");
                }

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("INSERT");

                throw ex;
            }

            return rowsAffected;
        }

    }
}
