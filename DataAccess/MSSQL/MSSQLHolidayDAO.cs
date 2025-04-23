using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using TransferObjects;
using Util;

namespace DataAccess
{
	/// <summary>
	/// Summary description for MSSQLHolidayDAO.
	/// </summary>
	public class MSSQLHolidayDAO : HolidayDAO
	{
		SqlConnection conn = null;
        protected string dateTimeformat = "";

		public MSSQLHolidayDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MSSQLHolidayDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

		public int insert(string description, DateTime holidayDate)
		{
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
			int rowsAffected = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO holidays ");
				sbInsert.Append("(description, holiday_date, created_by, created_time) ");
				sbInsert.Append("VALUES (");

				sbInsert.Append("N'" + description.Trim() + "', ");

				if(!holidayDate.Equals(new DateTime(0)))
				{
					sbInsert.Append("'" + holidayDate.ToString("yyy-MM-dd") + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}

				sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

				SqlCommand cmd = new SqlCommand( sbInsert.ToString(), conn, sqlTrans );
				rowsAffected = cmd.ExecuteNonQuery();
				sqlTrans.Commit();
				
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback("INSERT");
				throw ex;
			}

			return rowsAffected;
		}

		public bool delete(DateTime holidayDate)
		{
			bool isDeleted = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

			try
			{
				StringBuilder sbDelete = new StringBuilder();
				sbDelete.Append("DELETE FROM holidays WHERE holiday_date = '" + holidayDate.ToString("yyy-MM-dd").Trim() + "'");
				
				SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
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

		public HolidayTO find(DateTime holidayDate)
		{
			DataSet dataSet = new DataSet();
			HolidayTO holidayTO = new HolidayTO();
			try
			{
				SqlCommand cmd = new SqlCommand("SELECT * FROM holidays WHERE holiday_date = '" + holidayDate.ToString("yyy-MM-dd").Trim() + "'", conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Holidays");
				DataTable table = dataSet.Tables["Holidays"];

				if (table.Rows.Count == 1)
				{
					holidayTO = new HolidayTO();
					holidayTO.HolidayDate = (DateTime) table.Rows[0]["holiday_date"];

					if (!table.Rows[0]["description"].Equals(DBNull.Value))
					{
						holidayTO.Description = table.Rows[0]["description"].ToString().Trim();
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return holidayTO;
		}

		public bool update(string description, DateTime oldHolidayDate, DateTime newHolidayDate)
		{
			bool isUpdated = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

			try
			{
				StringBuilder sbUpdate = new StringBuilder();
				sbUpdate.Append("UPDATE holidays SET ");
				
				if (!description.Trim().Equals(""))
				{
					sbUpdate.Append("description = N'" + description.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("description = null, ");
				}
				if (!newHolidayDate.Equals(new DateTime(0)))
				{
					sbUpdate.Append("holiday_date = '" + newHolidayDate.ToString("yyy-MM-dd").Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("holiday_date = null, ");
				}
				
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = GETDATE() ");
				sbUpdate.Append("WHERE holiday_date = '" + oldHolidayDate.ToString("yyy-MM-dd").Trim() + "'");
				
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

		// This method should be called with fromDate and toDate converted to string in format "MM/dd/yyy"!!!!!
		public List<HolidayTO> getHolidays(HolidayTO hTO, DateTime fromDate, DateTime toDate)
		{
			DataSet dataSet = new DataSet();
			HolidayTO holidayTO = new HolidayTO();
			List<HolidayTO> holidaysList = new List<HolidayTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT * FROM holidays ");

				if((!hTO.Description.Trim().Equals("")) || (!fromDate.Equals(new DateTime())) || (!toDate.Equals(new DateTime())))
				{
					sb.Append(" WHERE ");
					
					if (!hTO.Description.Trim().Equals(""))
					{
						sb.Append(" UPPER(description) LIKE N'%" + hTO.Description.ToUpper().Trim() + "%' AND");
					}
					if (!fromDate.Equals(new DateTime()))
					{
                        sb.Append(" CONVERT(DATETIME, holiday_date, 101) >= '" + fromDate.ToString("MM/dd/yyy").Trim() + "' AND");
					}
					if (!toDate.Equals(new DateTime()))
					{
                        sb.Append(" CONVERT(DATETIME, holiday_date, 101) <= '" + toDate.ToString("MM/dd/yyy").Trim() + "' AND");
					}
					
					select = sb.ToString(0, sb.ToString().Length - 3);
				}
				else
				{
					select = sb.ToString();
				}
				
				select = select + "ORDER BY holiday_date ";

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Holidays");
				DataTable table = dataSet.Tables["Holidays"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						holidayTO = new HolidayTO();
							
						holidayTO.HolidayDate = (DateTime) row["holiday_date"];

						if (!row["description"].Equals(DBNull.Value))
						{
							holidayTO.Description = row["description"].ToString().Trim();
						}
						holidaysList.Add(holidayTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return holidaysList;
		}

		// TODO!!!
		public void serialize(List<HolidayTO> HolidayTOList)
		{
			try
			{
				string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLHolidayFile"];
				Stream stream = File.Open(filename, FileMode.Create);

				HolidayTO[] holidayTOArray = (HolidayTO[]) HolidayTOList.ToArray();

				XmlSerializer bformatter = new XmlSerializer(typeof(HolidayTO[]));
				bformatter.Serialize(stream, holidayTOArray);
				stream.Close();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

	}
}
