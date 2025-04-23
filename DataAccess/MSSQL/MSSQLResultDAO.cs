using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

using Util;
using TransferObjects;

namespace DataAccess
{
    public class MSSQLResultDAO : ResultDAO
    {
        SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;
		protected string dateTimeformat = "";

		public MSSQLResultDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

        public MSSQLResultDAO(SqlConnection SqlConnection)
        {
            conn = SqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public DataTable getResultTable(string fields, string tables, string filter, string sortCol, string sortDir, int firstRow, int lastRow)
        {
            DataSet dataSet = new DataSet();
            
            try
            {
                string aliases = "";
                string[] aliasNames;
                if (fields.Contains("|"))
                {
                    aliasNames = fields.Split('|');                  
                }
                else
                {
                    aliasNames = fields.Split(',');                   
                }
                foreach (string alias in aliasNames)
                {
                    if (alias.IndexOf("AS") > 0)
                        aliases += alias.Substring(alias.IndexOf("AS") + 3) + ",";
                    else
                        aliases += alias + ",";
                }
                string fields2 = fields.Replace('|', ',');

                if (aliases.Trim().Length > 0)
                    aliases = aliases.Substring(0, aliases.Length - 1);

                string tmpTable = "SELECT ROW_NUMBER() OVER (ORDER BY " + sortCol.Trim() + " " + sortDir.Trim() + ") AS row, " + fields2.Trim() + " FROM " + tables.Trim();
                if (!filter.Trim().Equals(""))    
                    tmpTable += " WHERE " + filter.Trim();
                string select = "SELECT " + aliases.Trim() + " FROM ( " + tmpTable + " ) AS tmp WHERE row >= '" + firstRow.ToString().Trim() + "' AND row <= '" + lastRow.ToString().Trim() + "'";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

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

                string[] fields2 = fields.Split(',');
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
                if (!sortCol.Trim().Equals(""))
                select += " ORDER BY " + sortCol.Trim() + " " + sortDir.Trim();

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

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
                string select = "SELECT COUNT(*) rows FROM " + tables.Trim();

                if (!filter.Trim().Equals(""))
                    select += " WHERE " + filter.Trim();

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ResultTable");

                DataTable table = dataSet.Tables["ResultTable"];

                if (table.Rows.Count > 0)

                    if (table.Rows[0]["rows"] != DBNull.Value)
                        int.TryParse(table.Rows[0]["rows"].ToString(), out rowCount);

                return rowCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
