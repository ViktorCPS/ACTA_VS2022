using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;

using TransferObjects;
using Util;

namespace DataAccess.MSSQL
{
    public class MSSQLACSEventsDAO: ACSEventsDAO
    {
        SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

        public MSSQLACSEventsDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

        public MSSQLACSEventsDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public List<ACSEventTO> findAll()
        {
            List<ACSEventTO> list = new List<ACSEventTO>();
            DataSet dataSet = new DataSet();
            ACSEventTO acsEv = new ACSEventTO();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM ACSEvents");
                select = sb.ToString();
                SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ACSEvents");
				DataTable table = dataSet.Tables["ACSEvents"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        acsEv = new ACSEventTO();

                        if (row["ID"] != DBNull.Value)
                        {
                            acsEv.Id = Convert.ToUInt32(row["ID"].ToString().Trim());
                        }
                        if (row["Event Type"] != DBNull.Value)
                        {
                            acsEv.EventType = Convert.ToInt32(row["Event Type"].ToString().Trim());
                        }
                        if (row["Name"] != DBNull.Value)
                        {
                            acsEv.Name = row["Name"].ToString().Trim();
                        }
                        if (row["Location"] != DBNull.Value)
                        {
                            acsEv.Location = row["Location"].ToString().Trim();
                        }
                        if (row["ACS Name"] != DBNull.Value)
                        {
                            acsEv.AcsName = row["ACS Name"].ToString().Trim();
                        }
                        if (row["Event Date Time"] != DBNull.Value)
                        {
                            acsEv.EventDateTime = (DateTime) row["Event Date Time"];
                        }
                        if (row["ACS ID"] != DBNull.Value)
                        {
                            acsEv.AcsID = Convert.ToInt32(row["ACS ID"].ToString().Trim());
                        }
                        if (row["Card ID"] != DBNull.Value)
                        {
                            acsEv.CardID = Convert.ToUInt32(row["Card ID"].ToString().Trim());
                        }
                        if (row["Direction"] != DBNull.Value)
                        {
                            acsEv.Direction = Convert.ToInt32(row["Direction"].ToString().Trim());
                        }
                        if (row["Employee ID"] != DBNull.Value)
                        {
                            acsEv.EmployeeID = Convert.ToInt32(row["Employee ID"].ToString().Trim());
                        }

                        list.Add(acsEv);
                    }
                }

            }
            catch(Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }
            return list;

        }
    }
}
