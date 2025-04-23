using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;
using TransferObjects;

namespace DataAccess
{
    public class MSSQLHolidaysExtendedDAO:HolidaysExtendedDAO
    {
        SqlConnection conn = null;
        protected string dateTimeformat = "";

        public MSSQLHolidaysExtendedDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MSSQLHolidaysExtendedDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public int insert(HolidaysExtendedTO hTOs)
		{
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
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
                    sbInsert.Append(hTOs.SundayTransferable + ", ");
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

        public bool delete(int recID)
		{
			bool isDeleted = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

			try
			{
				StringBuilder sbDelete = new StringBuilder();

                sbDelete.Append("DELETE FROM holidays_extended WHERE rec_id = '" + recID + "' ");

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


        public bool update(HolidaysExtendedTO hTOs)
		{
			bool isUpdated = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

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
                if (hTOs.SundayTransferable != -1)
                {
                    sbUpdate.Append("sunday_transferable = '" + hTOs.SundayTransferable.ToString() + "', ");
                }
                else
                {
                    sbUpdate.Append("sunday_transferable = null, ");
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
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = GETDATE() ");
				sbUpdate.Append("WHERE rec_id = '" + hTOs.RecID + "' ");

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
        public List<string> SearchDescriptions()
        {
            DataSet dataSet = new DataSet();
            List<string> holidaysDesc = new List<string>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT description FROM holidays_extended ");

                select = sb.ToString() + "ORDER BY description ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Holidays");
                DataTable table = dataSet.Tables["Holidays"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        holidaysDesc.Add(row["description"].ToString().Trim());
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
                        sb.Append("sunday_transferable = '" + hTO.SundayTransferable.ToString() + "', ");
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

                select = select + "ORDER BY date_start ";

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

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
                        if (!row["rec_id"].Equals(DBNull.Value))
                        {
                            holidayTO.RecID = int.Parse(row["rec_id"].ToString().Trim());
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
