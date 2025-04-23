using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Data;
using System.Collections;
using TransferObjects;

namespace DataAccess
{
    public  class MySQLLockDAO:LockDAO
    {
        MySqlConnection conn = null;
		MySqlTransaction _sqlTrans = null;
		protected string dateTimeformat = "";

		public MySQLLockDAO()
		{
			conn = MySQLDAOFactory.getConnection();	
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MySQLLockDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

		public MySqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}
        public bool beginTransaction()
        {
            bool isStarted = false;

            try
            {
                this.SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
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

        public int insert(DateTime lockDate, string type, string comment, bool doCommit)
        {
            MySqlTransaction sqlTrans = null;

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
                sbInsert.Append("INSERT INTO locking ");
                sbInsert.Append("(lock_date, type, comment, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                if (!lockDate.Equals(new DateTime()))
                {
                    sbInsert.Append("'" + lockDate.Date.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append("N'" + type.Trim() + "', ");

                if (!comment.Equals(""))
                {
                    sbInsert.Append("'"+comment + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");

                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit)
                {
                    sqlTrans.Commit();
                }
                rowsAffected = 1;
            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback();
                }
                throw ex;
            }

            return rowsAffected;
        }

        public ArrayList search()
        {
            DataSet dataSet = new DataSet();
            ArrayList list = new ArrayList();
            try
            {
                string select = "SELECT l.* FROM locking l ";
                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Lock");
                DataTable table = dataSet.Tables["Lock"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        LockTO locking = new LockTO();
                        locking.LockID = Int32.Parse(row["lock_id"].ToString().Trim());
                        if (row["comment"] != DBNull.Value)
                        {
                            locking.Comment = row["comment"].ToString().Trim();
                        }
                        if (row["type"] != DBNull.Value)
                        {
                            locking.Type = row["type"].ToString().Trim();
                        }
                        if (row["lock_date"] != DBNull.Value)
                        {
                            locking.LockDate = DateTime.Parse(row["lock_date"].ToString().Trim());
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            locking.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            locking.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        list.Add(locking);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return list;
        }

    }
}
