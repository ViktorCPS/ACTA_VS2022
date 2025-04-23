using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

using TransferObjects;


namespace DataAccess
{
	/// <summary>
	/// Summary description for MSSQLAddressDAO.
	/// </summary>
	public class MSSQLAddressDAO : AddressDAO
	{
		SqlConnection conn = null;

		public MSSQLAddressDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
		}
        public MSSQLAddressDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
        }
		public int insert(string name, string addressLine1, string addressLine2, string addressLine3,
			string cityName, string country, string state, string postalZipCode, string telNumber1,
			string telNumber2, string telNumber3, string faxNumber1, string faxNumber2, string faxNumber3)
		{
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
			int addressID = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("SET NOCOUNT ON ");
				sbInsert.Append("INSERT INTO addresses ");
				sbInsert.Append("(name, address_line_1, address_line_2, address_line_3, city_name, country, state, postal_zip_code, tel_number_1, tel_number_2, tel_number_3, fax_number_1, fax_number_2, fax_number_3, created_by, created_time) ");
				sbInsert.Append("VALUES (");
				
				if (name.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + name.Trim() + "', ");
				}
				if (addressLine1.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + addressLine1.Trim() + "', ");
				}
				if (addressLine2.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + addressLine2.Trim() + "', ");
				}
				if (addressLine3.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + addressLine3.Trim() + "', ");
				}
				if (cityName.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + cityName.Trim() + "', ");
				}
				if (country.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + country.Trim() + "', ");
				}
				if (state.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + state.Trim() + "', ");
				}
				if (postalZipCode.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + postalZipCode.Trim() + "', ");
				}
				if (telNumber1.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + telNumber1.Trim() + "', ");
				}
				if (telNumber2.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + telNumber2.Trim() + "', ");
				}
				if (telNumber3.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + telNumber3.Trim() + "', ");
				}
				if (faxNumber1.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + faxNumber1.Trim() + "', ");
				}
				if (faxNumber2.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + faxNumber2.Trim() + "', ");
				}
				if (faxNumber3.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + faxNumber3.Trim() + "', ");
				}
				sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");
				sbInsert.Append("SELECT @@Identity AS address_id ");
				sbInsert.Append("SET NOCOUNT OFF ");

				DataSet dataSet = new DataSet();
				SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans );

				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Addresses");

				DataTable table = dataSet.Tables["Addresses"];

				addressID = Int32.Parse(table.Rows[0]["address_id"].ToString());

				sqlTrans.Commit();
				
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback("INSERT");
				
				//throw new Exception(ex.Message);

				throw ex;
			}

			return addressID;
		}

		public bool delete(int addressID)
		{
			bool isDeleted = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

			try
			{
				StringBuilder sbDelete = new StringBuilder();
				sbDelete.Append("DELETE FROM addresses WHERE address_id = " + addressID.ToString().Trim());
				
				SqlCommand cmd = new SqlCommand( sbDelete.ToString(), conn, sqlTrans);
				int res = cmd.ExecuteNonQuery();
				isDeleted = true;
				sqlTrans.Commit();
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback("DELETE");
				
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public AddressTO find(int addressID)
		{
			DataSet dataSet = new DataSet();
			AddressTO address = new AddressTO();
			try
			{
				SqlCommand cmd = new SqlCommand( "SELECT * FROM addresses WHERE address_id = '" + addressID.ToString().Trim() + "'", conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Address");
				DataTable table = dataSet.Tables["Address"];

				if (table.Rows.Count == 1)
				{
					address = new AddressTO();
					address.AddressID = Int32.Parse(table.Rows[0]["address_id"].ToString().Trim());
					if (table.Rows[0]["name"] != DBNull.Value)
					{
						address.Name = table.Rows[0]["name"].ToString().Trim();
					}
					if (table.Rows[0]["address_line_1"] != DBNull.Value)
					{
						address.AddressLine1 = table.Rows[0]["address_line_1"].ToString().Trim();
					}
					if (table.Rows[0]["address_line_2"] != DBNull.Value)
					{
						address.AddressLine2 = table.Rows[0]["address_line_2"].ToString().Trim();
					}
					if (table.Rows[0]["address_line_3"] != DBNull.Value)
					{
						address.AddressLine3 = table.Rows[0]["address_line_3"].ToString().Trim();
					}
					if (table.Rows[0]["city_name"] != DBNull.Value)
					{
						address.CityName = table.Rows[0]["city_name"].ToString().Trim();
					}
					if (table.Rows[0]["country"] != DBNull.Value)
					{
						address.Country = table.Rows[0]["country"].ToString().Trim();
					}
					if (table.Rows[0]["state"] != DBNull.Value)
					{
						address.State = table.Rows[0]["state"].ToString().Trim();
					}
					if (table.Rows[0]["postal_zip_code"] != DBNull.Value)
					{
						address.PostalZipCode = table.Rows[0]["postal_zip_code"].ToString().Trim();
					}
					if (table.Rows[0]["tel_number_1"] != DBNull.Value)
					{
						address.TelNumber1 = table.Rows[0]["tel_number_1"].ToString().Trim();
					}
					if (table.Rows[0]["tel_number_2"] != DBNull.Value)
					{
						address.TelNumber2 = table.Rows[0]["tel_number_2"].ToString().Trim();
					}
					if (table.Rows[0]["tel_number_3"] != DBNull.Value)
					{
						address.TelNumber3 = table.Rows[0]["tel_number_3"].ToString().Trim();
					}
					if (table.Rows[0]["fax_number_1"] != DBNull.Value)
					{
						address.FaxNumber1 = table.Rows[0]["fax_number_1"].ToString().Trim();
					}
					if (table.Rows[0]["fax_number_2"] != DBNull.Value)
					{
						address.FaxNumber2 = table.Rows[0]["fax_number_2"].ToString().Trim();
					}
					if (table.Rows[0]["fax_number_3"] != DBNull.Value)
					{
						address.FaxNumber3 = table.Rows[0]["fax_number_3"].ToString().Trim();
					}

				}
			}
			catch(Exception ex)
			{
				throw new Exception("Exception: " + ex.Message);
			}

			return address;
		}

		public bool update(int addressId, string name, string addressLine1, string addressLine2, string addressLine3,
			string cityName, string country, string state, string postalZipCode, string telNumber1,
			string telNumber2, string telNumber3, string faxNumber1, string faxNumber2, string faxNumber3)
		{
			bool isUpdated = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

			try
			{
				StringBuilder sbUpdate = new StringBuilder();
				sbUpdate.Append("UPDATE addresses SET ");
				if (!name.Trim().Equals(""))
				{
					sbUpdate.Append("name = N'" + name.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("name = null, ");
				}
				if (!addressLine1.Trim().Equals(""))
				{
					sbUpdate.Append("address_line_1 = N'" + addressLine1.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("address_line_1 = null, ");
				}
				if (!addressLine2.Trim().Equals(""))
				{
					sbUpdate.Append("address_line_2 = N'" + addressLine2.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("address_line_2 = null, ");
				}
				if (!addressLine3.Trim().Equals(""))
				{
					sbUpdate.Append("address_line_3 = N'" + addressLine3.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("address_line_3 = null, ");
				}
				if (!cityName.Trim().Equals(""))
				{
					sbUpdate.Append("city_name = N'" + cityName.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("city_name = null, ");
				}
				if (!country.Trim().Equals(""))
				{
					sbUpdate.Append("country = N'" + country.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("country = null, ");
				}
				if (!state.Trim().Equals(""))
				{
					sbUpdate.Append("state = N'" + state.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("state = null, ");
				}
				if (!postalZipCode.Trim().Equals(""))
				{
					sbUpdate.Append("postal_zip_code = N'" + postalZipCode.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("postal_zip_code = null, ");
				}
				if (!telNumber1.Trim().Equals(""))
				{
					sbUpdate.Append("tel_number_1 = N'" + telNumber1.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("tel_number_1 = null, ");
				}
				if (!telNumber2.Trim().Equals(""))
				{
					sbUpdate.Append("tel_number_2 = N'" + telNumber2.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("tel_number_2 = null, ");
				}
				if (!telNumber3.Trim().Equals(""))
				{
					sbUpdate.Append("tel_number_3 = N'" + telNumber3.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("tel_number_3 = null, ");
				}
				if (!faxNumber1.Trim().Equals(""))
				{
					sbUpdate.Append("fax_number_1 = N'" + faxNumber1.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("fax_number_1 = null, ");
				}
				if (!faxNumber2.Trim().Equals(""))
				{
					sbUpdate.Append("fax_number_2 = N'" + faxNumber2.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("fax_number_2 = null, ");
				}
				if (!faxNumber3.Trim().Equals(""))
				{
					sbUpdate.Append("fax_number_3 = N'" + faxNumber3.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("fax_number_3 = null, ");
				}
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = GETDATE() ");
				sbUpdate.Append("WHERE address_id = '" + addressId.ToString().Trim() + "'");
				
				SqlCommand cmd = new SqlCommand( sbUpdate.ToString(), conn, sqlTrans );
				int res = cmd.ExecuteNonQuery();
				if(res > 0)
				{
					isUpdated = true;
				}

				sqlTrans.Commit();
				
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback("UPDATE");
				
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public ArrayList getAddresses(string addressId, string name, string addressLine1, string addressLine2, string addressLine3,
			string cityName, string country, string state, string postalZipCode, string telNumber1,
			string telNumber2, string telNumber3, string faxNumber1, string faxNumber2, string faxNumber3)
		{
			DataSet dataSet = new DataSet();
			AddressTO address = new AddressTO();
			ArrayList addressesList = new ArrayList();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT * FROM addresses ");

				if((!addressId.Trim().Equals("")) || (!name.Trim().Equals("")) ||
					(!addressLine1.Trim().Equals("")) || (!addressLine2.Trim().Equals("")) ||
					(!addressLine3.Trim().Equals("")) || (!cityName.Trim().Equals("")) ||
					(!country.Trim().Equals("")) || (!state.Trim().Equals("")) ||
					(!postalZipCode.Trim().Equals("")) || (!telNumber1.Trim().Equals("")) ||
					(!telNumber2.Trim().Equals("")) || (!telNumber3.Trim().Equals("")) ||
					(!faxNumber1.Trim().Equals("")) || (!faxNumber2.Trim().Equals("")) ||
					(!faxNumber3.Trim().Equals("")))
				{
					sb.Append(" WHERE ");
					
					if (!addressId.Trim().Equals(""))
					{
						sb.Append(" UPPER(address_id) LIKE '" + addressId.Trim().ToUpper() + "' AND");
					}
					if (!name.Trim().Equals(""))
					{
						sb.Append(" UPPER(name) LIKE N'" + name.Trim().ToUpper() + "' AND");
					}
					if (!addressLine1.Trim().Equals(""))
					{
						sb.Append(" UPPER(address_line_1) LIKE N'" + addressLine1.Trim().ToUpper() + "' AND");
					}
					if (!addressLine2.Trim().Equals(""))
					{
						sb.Append(" UPPER(address_line_2) LIKE N'" + addressLine2.Trim().ToUpper() + "' AND");
					}
					if (!addressLine3.Trim().Equals(""))
					{
						sb.Append(" UPPER(address_line_3) LIKE N'" + addressLine3.Trim().ToUpper() + "' AND");
					}
					if (!cityName.Trim().Equals(""))
					{
						sb.Append(" UPPER(city_name) LIKE N'" + cityName.Trim().ToUpper() + "' AND");
					}
					if (!country.Trim().Equals(""))
					{
						sb.Append(" UPPER(countru) LIKE N'" + country.Trim().ToUpper() + "' AND");
					}
					if (!state.Trim().Equals(""))
					{
						sb.Append(" UPPER(state) LIKE N'" + state.Trim().ToUpper() + "' AND");
					}
					if (!postalZipCode.Trim().Equals(""))
					{
						sb.Append(" UPPER(postal_zip_code) LIKE N'" + postalZipCode.Trim().ToUpper() + "' AND");
					}
					if (!telNumber1.Trim().Equals(""))
					{
						sb.Append(" UPPER(tel_number_1) LIKE '" + telNumber1.Trim().ToUpper() + "' AND");
					}
					if (!telNumber2.Trim().Equals(""))
					{
						sb.Append(" UPPER(tel_number_2) LIKE '" + telNumber2.Trim().ToUpper() + "' AND");
					}
					if (!telNumber3.Trim().Equals(""))
					{
						sb.Append(" UPPER(tel_number_3) LIKE '" + telNumber3.Trim().ToUpper() + "' AND");
					}
					if (!faxNumber1.Trim().Equals(""))
					{
						sb.Append(" UPPER(fax_number_1) LIKE '" + faxNumber1.Trim().ToUpper() + "' AND");
					}
					if (!faxNumber2.Trim().Equals(""))
					{
						sb.Append(" UPPER(fax_number_2) LIKE '" + faxNumber2.Trim().ToUpper() + "' AND");
					}
					if (!faxNumber3.Trim().Equals(""))
					{
						sb.Append(" UPPER(tel_number_3) LIKE '" + faxNumber3.Trim().ToUpper() + "' AND");
					}

					select = sb.ToString(0, sb.ToString().Length - 3);
				}
				else
				{
					select = sb.ToString();
				}

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Address");
				DataTable table = dataSet.Tables["Address"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						address = new AddressTO();
						address.AddressID = Int32.Parse(row["address_id"].ToString().Trim());
						if (row["name"] != DBNull.Value)
						{
							address.Name = row["name"].ToString().Trim();
						}
						if (row["address_line_1"] != DBNull.Value)
						{
							address.AddressLine1 = row["address_line_1"].ToString().Trim();
						}
						if (row["address_line_2"] != DBNull.Value)
						{
							address.AddressLine2 = row["address_line_2"].ToString().Trim();
						}
						if (row["address_line_3"] != DBNull.Value)
						{
							address.AddressLine3 = row["address_line_3"].ToString().Trim();
						}
						if (row["city_name"] != DBNull.Value)
						{
							address.CityName = row["city_name"].ToString().Trim();
						}
						if (row["country"] != DBNull.Value)
						{
							address.Country = row["country"].ToString().Trim();
						}
						if (row["state"] != DBNull.Value)
						{
							address.State = row["state"].ToString().Trim();
						}
						if (row["postal_zip_code"] != DBNull.Value)
						{
							address.PostalZipCode = row["postal_zip_code"].ToString().Trim();
						}
						if (row["tel_number_1"] != DBNull.Value)
						{
							address.TelNumber1 = row["tel_number_1"].ToString().Trim();
						}
						if (row["tel_number_2"] != DBNull.Value)
						{
							address.TelNumber2 = row["tel_number_2"].ToString().Trim();
						}
						if (row["tel_number_3"] != DBNull.Value)
						{
							address.TelNumber3 = row["tel_number_3"].ToString().Trim();
						}
						if (row["fax_number_1"] != DBNull.Value)
						{
							address.FaxNumber1 = row["fax_number_1"].ToString().Trim();
						}
						if (row["fax_number_2"] != DBNull.Value)
						{
							address.FaxNumber2 = row["fax_number_2"].ToString().Trim();
						}
						if (row["fax_number_3"] != DBNull.Value)
						{
							address.FaxNumber3 = row["fax_number_3"].ToString().Trim();
						}

						addressesList.Add(address);
					}
				}
			}
			catch(Exception ex)
			{
				
				throw new Exception("Exception: " + ex.Message);
			}

			return addressesList;
		}
	}
}
