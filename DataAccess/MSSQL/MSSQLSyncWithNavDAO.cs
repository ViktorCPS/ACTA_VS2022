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
using System.Globalization;
using Util;

namespace DataAccess
{
    /// <summary>
    /// Funkcije za povezivanje 
    /// </summary>
    public class MSSQLSyncWithNavDAO : SyncWithNavDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLSyncWithNavDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
            DAOController.GetInstance();
        }

        public MSSQLSyncWithNavDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
            DAOController.GetInstance();
        }

        public void SetDBConnection(Object dbConnection)
        {
            conn = dbConnection as SqlConnection;
        }

        public int PozoviStornuProceduru(string stornaProcedura)
        {
            DataSet dataSet = new DataSet();
            int i = 0;
            try
            {
                // 1.  create a command object identifying
                //     the stored procedure
                SqlCommand cmd = new SqlCommand(stornaProcedura, conn);
                
                cmd.CommandTimeout = 300;

                cmd.Parameters.Add("@uRedova", SqlDbType.Int).Direction = ParameterDirection.Output;
                // 2. set the command object so it knows
                //    to execute a stored procedure-
                cmd.CommandType = CommandType.StoredProcedure;

                // execute the command               
                
                //i= Convert.ToInt32(cmd.ExecuteScalar());
                cmd.ExecuteScalar();
                i = Convert.ToInt32(cmd.Parameters["@uRedova"].Value);
                //cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }
            return i;
        }
    }
}