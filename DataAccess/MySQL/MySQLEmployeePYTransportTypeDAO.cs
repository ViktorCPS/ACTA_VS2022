using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Data;

using TransferObjects;
using Util;

namespace DataAccess
{
    public class MySQLEmployeePYTransportTypeDAO : EmployeePYTransportTypeDAO
    {
        MySqlConnection conn = null;
        MySqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MySQLEmployeePYTransportTypeDAO()
		{
            conn = MySQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

        public MySQLEmployeePYTransportTypeDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public Dictionary<int, EmployeePYTransportTypeTO> getEmplTransportTypes()
        {
            DataSet dataSet = new DataSet();
            EmployeePYTransportTypeTO typeTO = new EmployeePYTransportTypeTO();
            Dictionary<int, EmployeePYTransportTypeTO> typeDict = new Dictionary<int, EmployeePYTransportTypeTO>();
            string select = "";

            try
            {
                select = "SELECT * FROM employee_py_transport_types";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Types");
                DataTable table = dataSet.Tables["Types"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        typeTO = new EmployeePYTransportTypeTO();

                        typeTO.TransportTypeID = Int32.Parse(row["transport_type_id"].ToString().Trim());
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            typeTO.Name = row["name"].ToString().Trim();
                        }
                        if (!row["price_daily"].Equals(DBNull.Value))
                        {
                            typeTO.PriceDaily = decimal.Parse(row["price_daily"].ToString().Trim());
                        }

                        if (!typeDict.ContainsKey(typeTO.TransportTypeID))
                            typeDict.Add(typeTO.TransportTypeID, typeTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return typeDict;
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
                throw ex;
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
