using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Globalization;

using TransferObjects;
using Util;

namespace DataAccess
{
	/// <summary>
	/// Summary description for MySQLHolidayDAO.
	/// </summary>
	public class MySQLHolidayDAO : HolidayDAO
	{
		MySqlConnection conn = null;
        protected string dateTimeformat = "";

		public MySQLHolidayDAO()
		{
			conn = MySQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MySQLHolidayDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
		public int insert(string description, DateTime holidayDate)
		{
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
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

				sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");

				MySqlCommand cmd = new MySqlCommand( sbInsert.ToString(), conn, sqlTrans );
				rowsAffected = cmd.ExecuteNonQuery();
				sqlTrans.Commit();
				
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback();
				throw ex;
			}

			return rowsAffected;
		}

		public bool delete(DateTime holidayDate)
		{
			bool isDeleted = false;
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

			try
			{
				StringBuilder sbDelete = new StringBuilder();
				sbDelete.Append("DELETE FROM holidays WHERE holiday_date = '" + holidayDate.ToString("yyy-MM-dd").Trim() + "'");
				
				MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, sqlTrans);
				int res = cmd.ExecuteNonQuery();
				isDeleted = true;
				sqlTrans.Commit();
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback();
				
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
				MySqlCommand cmd = new MySqlCommand("SELECT * FROM holidays WHERE holiday_date = '" + holidayDate.ToString("yyy-MM-dd").Trim() + "'", conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Holidays");
				DataTable table = dataSet.Tables["Holidays"];

				if (table.Rows.Count == 1)
				{
					holidayTO = new HolidayTO();
					holidayTO.HolidayDate = DateTime.Parse( table.Rows[0]["holiday_date"].ToString());

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
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

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
				sbUpdate.Append("modified_time = NOW() ");
				sbUpdate.Append("WHERE holiday_date = '" + oldHolidayDate.ToString("yyy-MM-dd").Trim() + "'");
				
				MySqlCommand cmd = new MySqlCommand( sbUpdate.ToString(), conn, sqlTrans );
				int res = cmd.ExecuteNonQuery();
				if(res > 0)
				{
					isUpdated = true;	
				}
				sqlTrans.Commit();
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback();
				
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
                        sb.Append(" holiday_date >= STR_TO_DATE('" + fromDate.ToString("MM/dd/yyy").Trim() + "','%m/%d/%Y') AND");
					}
					if (!toDate.Equals(new DateTime()))
					{
                        sb.Append(" holiday_date <= STR_TO_DATE('" + toDate.ToString("MM/dd/yyy").Trim() + "','%m/%d/%Y') AND");
					}
					
					select = sb.ToString(0, sb.ToString().Length - 3);
				}
				else
				{
					select = sb.ToString();
				}
				
				select = select + "ORDER BY holiday_date ";

				MySqlCommand cmd = new MySqlCommand(select, conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Holidays");
				DataTable table = dataSet.Tables["Holidays"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						holidayTO = new HolidayTO();
							
						holidayTO.HolidayDate = DateTime.Parse( row["holiday_date"].ToString());

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
