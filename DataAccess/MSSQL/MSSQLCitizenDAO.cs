using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// Summary description for MSSQLCitizenDAO.
	/// </summary>
	public class MSSQLCitizenDAO : CitizenDAO
	{
		SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;
		protected string dateTimeformat = "";

		public MSSQLCitizenDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MSSQLCitizenDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
              DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
		public SqlTransaction SqlTrans
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

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

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
