using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataAccess
{
    public class MySQLApplUsersLoginChangesTblDAO : ApplUsersLoginChangesTblDAO
    {
        MySqlConnection conn = null;
		
		public MySQLApplUsersLoginChangesTblDAO()
		{
			conn = MySQLDAOFactory.getConnection();			
		}

        public MySQLApplUsersLoginChangesTblDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
        }

        public List<string> getTablesNames()
        {
            DataSet dataSet = new DataSet();
            List<string> tableNames = new List<string>();

            try
            {
                string select = "SELECT * FROM appl_users_login_changes_tbl";
                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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

        public List<string> getAllTablesNames()
        {
            DataSet dataSet = new DataSet();
            List<string> tableNames = new List<string>();

            try
            {
                string select = "SELECT * FROM sys.tables";
                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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


        public int insert(List<string> listToSaveToDB)
        {
            throw new NotImplementedException();
        }

        public bool delete()
        {
            bool isDeleted = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM appl_users_login_changes_tbl");

                MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    isDeleted = true;
                }
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();

                throw new Exception(ex.Message);
            }
            return isDeleted;
        }


        List<string> ApplUsersLoginChangesTblDAO.getTablesNames()
        {
            throw new NotImplementedException();
        }

        List<string> ApplUsersLoginChangesTblDAO.getAllTablesNames()
        {
            throw new NotImplementedException();
        }

        int ApplUsersLoginChangesTblDAO.insert(List<string> listToSaveToDB)
        {
            throw new NotImplementedException();
        }

        bool ApplUsersLoginChangesTblDAO.delete()
        {
            throw new NotImplementedException();
        }

        public int insert(string table)
        {
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
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

    }
}
