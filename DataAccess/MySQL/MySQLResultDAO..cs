using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Globalization;
using MySql.Data.MySqlClient;

using TransferObjects;
using Util;

namespace DataAccess
{
    public class MySQLResultDAO : ResultDAO
    {
        MySqlConnection conn = null;
		MySqlTransaction _sqlTrans = null;
		protected string dateTimeformat = "";

		public MySQLResultDAO()
		{
			conn = MySQLDAOFactory.getConnection();
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

        public MySQLResultDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

		public MySqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

        public DataTable getResultTable(string fields, string tables, string filter, string sortCol, string sortDir, int firstRow, int lastRow)
        {
            DataSet dataSet = new DataSet();            

            try
            {
                // number of rows feched
                int rowsNum = lastRow - firstRow + 1;
                // number of rows skipped
                int offset = firstRow - 1;
                string fields2 = fields.Replace('|', ',');
                string select = "SELECT " + fields2.Trim() + " FROM " + tables.Trim();
                if (!filter.Trim().Equals(""))
                    select += " WHERE " + filter.Trim();
                select += " ORDER BY " + sortCol.Trim() + " " + sortDir.Trim()
                    + " LIMIT " + rowsNum.ToString().Trim() + " OFFSET " + offset.ToString().Trim();

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ResultTable");

                DataTable table = dataSet.Tables["ResultTable"];

                return table;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getResultTable(string fields, string tables, string filter, string sortCol, string sortDir)
        {
            DataSet dataSet = new DataSet();

            try
            {
                string[] fields2 = fields.Split('|');
                string select = "SELECT ";
                foreach (string fieldName in fields2)
                {
                    if (fieldName == "cycle_day")
                    {
                        select += "cycle_day+1 AS cycle_day, ";
                    }
                    else
                    {
                        select += fieldName + ", ";
                    }
                }
                select = select.Substring(0, select.Length - 2);
                select += " FROM " + tables.Trim();
                if (!filter.Trim().Equals(""))
                    select += " WHERE " + filter.Trim();
                select += " ORDER BY " + sortCol.Trim() + " " + sortDir.Trim();

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ResultTable");

                DataTable table = dataSet.Tables["ResultTable"];

                return table;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int getResultCount(string tables, string filter)
        {
            DataSet dataSet = new DataSet();
            int rowCount = -1;

            try
            {
                string select = "SELECT COUNT(*) AS rowCount FROM " + tables.Trim();

                if (!filter.Trim().Equals(""))
                    select += " WHERE " + filter.Trim();

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ResultTable");

                DataTable table = dataSet.Tables["ResultTable"];

                if (table.Rows.Count > 0)

                    if (table.Rows[0]["rowCount"] != DBNull.Value)
                        int.TryParse(table.Rows[0]["rowCount"].ToString(), out rowCount);

                return rowCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
