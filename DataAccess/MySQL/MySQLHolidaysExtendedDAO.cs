using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using TransferObjects;
using System.Globalization;
using System.Data.SqlClient;

namespace DataAccess
{
   public class MySQLHolidaysExtendedDAO:HolidaysExtendedDAO
    {
       MySqlConnection conn = null;
        protected string dateTimeformat = "";

        public MySQLHolidaysExtendedDAO()
		{
			conn = MySQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MySQLHolidaysExtendedDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public int insert(HolidaysExtendedTO hTOs)
		{
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
			int rowsAffected = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO holidays_extended ");
                sbInsert.Append("(description, type,category,year, date_start, date_end,sunday_transferable, created_by, created_time) ");
				sbInsert.Append("VALUES (");
                if (hTOs.Description.Trim() != "")
                {
                    sbInsert.Append("N'" + hTOs.Description.Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (hTOs.Type.Trim() != "")
                {
                    sbInsert.Append("N'" + hTOs.Type.Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (hTOs.Category != "")
                {
                    sbInsert.Append("'" + hTOs.Category + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (hTOs.Year != new DateTime())
                {
                    sbInsert.Append("N'" + hTOs.Year.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (hTOs.DateStart != new DateTime())
                {
                    sbInsert.Append("N'" + hTOs.DateStart.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (hTOs.DateEnd != new DateTime())
                {
                    sbInsert.Append("N'" + hTOs.DateEnd.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (hTOs.SundayTransferable != -1)
                {
                    sbInsert.Append("N'" + hTOs.SundayTransferable.ToString() + "', ");
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

        public bool delete(int recID)
		{
			bool isDeleted = false;
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

			try
			{
				StringBuilder sbDelete = new StringBuilder();

                sbDelete.Append("DELETE FROM holidays_extended WHERE rec_id = '" + recID + "' ");

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


        public bool update(HolidaysExtendedTO hTOs)
		{
			bool isUpdated = false;
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

			try
			{
				StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE holidays_extended SET ");
				
				if (!hTOs.Description.Trim().Equals(""))
				{
                    sbUpdate.Append("description = N'" + hTOs.Description.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("description = null, ");
				}
                if (!hTOs.Type.Trim().Equals(""))
                {
                    sbUpdate.Append("type = N'" + hTOs.Type.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("type = null, ");
                }
                if (hTOs.Category != "")
                {
                    sbUpdate.Append("category = '" + hTOs.Category + "', ");
                }
                else
                {
                    sbUpdate.Append("category = null, ");
                }
                if (hTOs.Year != new DateTime())
                {
                    sbUpdate.Append("year = '" + hTOs.Year.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbUpdate.Append("year = null, ");
                }
                if (hTOs.DateStart != new DateTime())
                {
                    sbUpdate.Append("date_start = '" + hTOs.DateStart.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbUpdate.Append("date_start = null, ");
                }
                if (hTOs.DateEnd != new DateTime())
                {
                    sbUpdate.Append("date_end = '" + hTOs.DateEnd.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbUpdate.Append("date_end = null, ");
                }
                if (hTOs.SundayTransferable != -1)
                {
                    sbUpdate.Append("sunday_transferable = '" + hTOs.SundayTransferable.ToString() + "', ");
                }
                else
                {
                    sbUpdate.Append("sunday_transferable = null, ");
                }
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = NOW() ");
				sbUpdate.Append("WHERE rec_id = '" + hTOs.RecID + "' ");

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
        public List<string> SearchDescriptions()
        {
            DataSet dataSet = new DataSet();
            List<string> holidaysDesc = new List<string>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT description FROM holidays_extended ");

                select = select + "ORDER BY description ";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Holidays");
                DataTable table = dataSet.Tables["Holidays"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {


                        holidaysDesc.Add( row["description"].ToString().Trim());
                      
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return holidaysDesc;
        }
		// This method should be called with fromDate and toDate converted to string in format "MM/dd/yyy"!!!!!
        public List<HolidaysExtendedTO> getHolidays(HolidaysExtendedTO hTO, DateTime fromDate, DateTime toDate)
		{
			DataSet dataSet = new DataSet();
            HolidaysExtendedTO holidayTO = new HolidaysExtendedTO();
            List<HolidaysExtendedTO> holidaysList = new List<HolidaysExtendedTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT * FROM holidays_extended ");

                if ((!hTO.Description.Trim().Equals("")) || (!hTO.Type.Trim().Equals("")) || (hTO.Category != "") || (hTO.Year!= new DateTime())
                    || (!fromDate.Equals(new DateTime())) || (!toDate.Equals(new DateTime())))
				{
					sb.Append(" WHERE ");
					
					if (!hTO.Description.Trim().Equals(""))
					{
						sb.Append(" UPPER(description) LIKE N'%" + hTO.Description.ToUpper().Trim() + "%' AND");
					}
                    if (!hTO.Category.Trim().Equals(""))
                    {
                        sb.Append(" category = '" + hTO.Category.Trim() + "' AND");
                    }
                    if (!hTO.Type.Trim().Equals(""))
                    {
                        sb.Append(" type = '" + hTO.Type.Trim() + "' AND");
                    }
                    if (hTO.SundayTransferable != -1)
                    {
                        sb.Append(" sunday_transferable = '" + hTO.SundayTransferable.ToString().Trim() + "' AND");
                    }
                    if (!fromDate.Equals(new DateTime()))
					{
                        sb.Append(" CONVERT(DATETIME, date_end, 101) >= '" + fromDate.ToString("MM/dd/yyy").Trim() + "' AND");
					}
					if (!toDate.Equals(new DateTime()))
					{
                        sb.Append(" CONVERT(DATETIME, date_start, 101) <= '" + toDate.ToString("MM/dd/yyy").Trim() + "' AND");
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
                        holidayTO = new HolidaysExtendedTO();

                        holidayTO.DateStart = (DateTime)row["date_start"];
                        holidayTO.DateEnd = (DateTime)row["date_end"];

						if (!row["description"].Equals(DBNull.Value))
						{
							holidayTO.Description = row["description"].ToString().Trim();
						}
                        if (!row["type"].Equals(DBNull.Value))
                        {
                            holidayTO.Type = row["type"].ToString().Trim();
                        }
                        if (!row["category"].Equals(DBNull.Value))
                        {
                            holidayTO.Category = row["category"].ToString().Trim();
                        }
                        if (!row["year"].Equals(DBNull.Value))
                        {
                            holidayTO.Year = DateTime.Parse(row["year"].ToString().Trim());
                        }
                        if (!row["sunday_transferable"].Equals(DBNull.Value))
                        {
                            holidayTO.SundayTransferable = int.Parse(row["sunday_transferable"].ToString().Trim());
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
    }
}
