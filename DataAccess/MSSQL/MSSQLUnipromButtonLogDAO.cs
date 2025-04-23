using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Collections;

namespace DataAccess
{
    public class MSSQLUnipromButtonLogDAO:UnipromButtonLogDAO
    {
         SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;
		protected string dateTimeformat = "";

		public MSSQLUnipromButtonLogDAO()
		{
			conn = MSSQLDAOFactory.getConnection();	
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

       public MSSQLUnipromButtonLogDAO(SqlConnection SqlConnection)
        {
            conn = SqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

		public SqlTransaction SqlTrans
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
            if (this.SqlTrans != null)
            {
                this.SqlTrans.Commit();
                this.SqlTrans = null;
            }
        }

        public void rollbackTransaction()
        {
            if (this.SqlTrans != null)
            {
                this.SqlTrans.Rollback();
                this.SqlTrans = null;
            }
        }

        public IDbTransaction getTransaction()
        {
            return _sqlTrans;
        }

        public void setTransaction(IDbTransaction trans)
        {
            _sqlTrans = (SqlTransaction)trans;
        }

        public int insert(int readerID, int antennaOutput, int status,bool doCommit)
        {
            SqlTransaction sqlTrans = null;

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
                sbInsert.Append("INSERT INTO uniprom_buttons_log ");
                sbInsert.Append("(reader_id,antenna_output,status, created_by, created_time,modified_by,modified_time) ");
                sbInsert.Append("VALUES (");

                sbInsert.Append("'" + readerID + "', ");
                sbInsert.Append("'" + antennaOutput + "', ");
                sbInsert.Append("'"+status + "', ");

                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE(), ");
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
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

        public ArrayList search(int readerId)
        {
            DataSet dataSet = new DataSet();
            ArrayList list = new ArrayList();
            try
            {
                string select = "SELECT * FROM uniprom_buttons_log WHERE reader_id = "+readerId;
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "UnipromButtons");
                DataTable table = dataSet.Tables["UnipromButtons"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        UnipromButtonLogTO ubl = new UnipromButtonLogTO();
                        ubl.LogID = Int32.Parse(row["log_id"].ToString().Trim());
                        ubl.ReaderID = Int32.Parse(row["reader_id"].ToString().Trim());
                        if (row["antenna_output"] != DBNull.Value)
                        {
                            ubl.AntennaOutput = int.Parse(row["antenna_output"].ToString().Trim());
                        }
                        if (row["status"] != DBNull.Value)
                        {
                            ubl.Status = int.Parse(row["status"].ToString().Trim());
                        }                        
                        if (row["created_by"] != DBNull.Value)
                        {
                            ubl.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            ubl.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        list.Add(ubl);
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
