using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Data;
using TransferObjects;
using System.Collections;

namespace DataAccess
{
    public class MySQLUnipromButtonDAO:UnipromButtonDAO
    {
         MySqlConnection conn = null;
		MySqlTransaction _sqlTrans = null;
		protected string dateTimeformat = "";

		public MySQLUnipromButtonDAO()
		{
			conn = MySQLDAOFactory.getConnection();	
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MySQLUnipromButtonDAO(MySqlConnection mySqlConnection)
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
            _sqlTrans = (MySqlTransaction)trans;
        }

        public int update(int readerID, int antennaOutput, int status,bool doCommit)
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
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE uniprom_buttons SET");
                sbUpdate.Append(" status = '"+status+"', modified_time = NOW() ");
                sbUpdate.Append("WHERE reader_id = " + readerID + " AND antenna_output = " + antennaOutput);           
                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, sqlTrans);
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
                        select += "reader_id = " + readerId;
                    if (antenna != -1)
                        select += " AND antenna_output = " + antenna;
                }
                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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
