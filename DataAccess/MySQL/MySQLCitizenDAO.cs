using System;
using System.Collections;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Text;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// Summary description for MySQLCitizenDAO.
	/// </summary>
	public class MySQLCitizenDAO : CitizenDAO
	{
		MySqlConnection conn = null;
		MySqlTransaction _sqlTrans = null;
		protected string dateTimeformat = "";

		public MySQLCitizenDAO()
		{
			conn = MySQLDAOFactory.getConnection();
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MySQLCitizenDAO(MySqlConnection mySqlConnection)
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

		public ArrayList getCitizens(string jmbg, string firstName, string lastName)
		{
			DataSet dataSet = new DataSet();
			CitizenTO citizen = new CitizenTO();
			ArrayList citizenList = new ArrayList();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT * FROM citizens ");

				if(	(!jmbg.Trim().Equals("")) || (!firstName.Trim().Equals("")) || (!lastName.Trim().Equals("")))
				{
					sb.Append(" WHERE ");
					if (!jmbg.Trim().Equals(""))
					{
						sb.Append(" UPPER(jmbg) LIKE N'" + jmbg.Trim().ToUpper() + "' AND");
					}					
					if (!firstName.Trim().Equals(""))
					{
						sb.Append(" UPPER(first_name) LIKE N'" + firstName.Trim().ToUpper() + "' AND");
					}
					if (!lastName.Trim().Equals(""))
					{
						sb.Append(" UPPER(last_name) LIKE N'" + lastName.Trim().ToUpper() + "' AND");
					}
					select = sb.ToString(0, sb.ToString().Length - 3);
				}
				else
				{
					select = sb.ToString();
				}

				MySqlCommand cmd = new MySqlCommand(select, conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Visits");
				DataTable table = dataSet.Tables["Visits"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						citizen = new CitizenTO();
						citizen.JMBG = row["jmbg"].ToString().Trim();
						citizen.FirstName = row["first_name"].ToString().Trim();
						citizen.LastName = row["last_name"].ToString().Trim();
						
						citizenList.Add(citizen);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception("Exception: " + ex.Message);
			}

			return citizenList;
		}
}
}
