using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Globalization;

namespace DataAccess
{
    public class MSSQLUnipromButtonDAO:UnipromButtonDAO
    {
        SqlConnection conn = null;
	    SqlTransaction _sqlTrans = null;
		protected string dateTimeformat = "";

		public MSSQLUnipromButtonDAO()
		{
			conn = MSSQLDAOFactory.getConnection();	
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MSSQLUnipromButtonDAO(SqlConnection SqlConnection)
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

        public int update(int readerID, int antennaOutput, int status,bool doCommit)
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
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE uniprom_buttons SET ");
                sbUpdate.Append("status = "+status+", modified_time = GETDATE() ");
                sbUpdate.Append("WHERE reader_id = " + readerID + " AND antenna_output = "+antennaOutput);           
                
                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
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

        public ArrayList search(int readerId, int antenna)
        {
            DataSet dataSet = new DataSet();
            ArrayList list = new ArrayList();
            try
            {
                string select = "SELECT * FROM uniprom_buttons";
                if (readerId != -1 || antenna != -1)
                {
                    select += " WHERE ";
                    if (readerId != -1)
                    select += "reader_id = " + readerId+" AND";
                if (antenna != -1)
                    select += " antenna_output = " + antenna+" AND";
                select = select.Substring(0, select.Length - 3);
                }
                 
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "UnipromButtons");
                DataTable table = dataSet.Tables["UnipromButtons"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        UnipromButtonTO ub = new UnipromButtonTO();
                        ub.ReaderID = Int32.Parse(row["reader_id"].ToString().Trim());
                        if (row["antenna_output"] != DBNull.Value)
                        {
                            ub.AntennaOutput = int.Parse(row["antenna_output"].ToString().Trim());
                        }
                        if (row["status"] != DBNull.Value)
                        {
                            ub.Status = int.Parse(row["status"].ToString().Trim());
                        }                        
                        if (row["modified_time"] != DBNull.Value)
                        {
                            ub.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }
                        list.Add(ub);
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
