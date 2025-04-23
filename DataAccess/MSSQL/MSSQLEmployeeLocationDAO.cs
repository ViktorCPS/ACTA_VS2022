using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using TransferObjects;

using Util;

namespace DataAccess
{
	/// <summary>
	/// Summary description for MSSQLEmployeeLocationDAO.
	/// </summary>
	public class MSSQLEmployeeLocationDAO : EmployeeLocationDAO
	{
		SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;
		protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

		public MSSQLEmployeeLocationDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MSSQLEmployeeLocationDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
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
                throw new Exception(ex.Message);
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
            _sqlTrans = (SqlTransaction)trans;
        }

        public void SetDBConnection(Object dbConnection)
        {
            conn = dbConnection as SqlConnection;
        }

        public int insert(int employeeID, int readerID, int antenna, int passTypeID, DateTime eventTime, int locationID, bool doCommit)
		{
			int rowsAffected = 0;
            SqlTransaction sqlTrans = null;
            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO employee_location ");
				sbInsert.Append("(employee_id, reader_id, antenna, pass_type_id, event_time, location_id, created_by, created_time) ");
				sbInsert.Append("VALUES (");

				if (employeeID != -1)
				{
					sbInsert.Append("'" + employeeID.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (readerID != -1)
				{
					sbInsert.Append("'" + readerID.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (antenna != -1)
				{
					sbInsert.Append("'" + antenna.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (passTypeID != -1)
				{
					sbInsert.Append("'" + passTypeID.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (!eventTime.Equals(new DateTime()))
				{
					sbInsert.Append("'" + eventTime.ToString(dateTimeformat).Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (locationID != -1)
				{
					sbInsert.Append("'" + locationID.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				
				sbInsert.Append(" N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");
					
				SqlCommand cmd = new SqlCommand( sbInsert.ToString(), conn, sqlTrans );
				rowsAffected = cmd.ExecuteNonQuery();
                if (doCommit)
                {
                    sqlTrans.Commit();
                }
				
			}
			catch(Exception ex)
			{
                if (doCommit)
                {
                    sqlTrans.Rollback("INSERT");
                }
				throw ex;
			}

			return rowsAffected;
		}

		public bool update(int employeeID, int readerID, int antenna, int passTypeID, DateTime eventTime, int locationID)
		{
			bool isUpdated = false;
            SqlTransaction sqlTrans = null;
            try
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");
            }
            catch (Exception ex)
            {
                throw new FailToStartDBTransactionException(ex.Message);
            }

			try
			{
				StringBuilder sbUpdate = new StringBuilder();
				sbUpdate.Append("UPDATE employee_location SET ");

				/*
				if (employeeID != -1)
				{
					sbUpdate.Append("employee_id = '" + employeeID.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("employee_id = NULL, ");
				}
				*/
				if (readerID != -1)
				{
					sbUpdate.Append("reader_id = '" +  readerID.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("reader_id = NULL, ");
				}
				if (antenna != -1)
				{
					sbUpdate.Append("antenna = '" +  antenna.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("antenna = NULL, ");
				}
				if (passTypeID != -1)
				{
					sbUpdate.Append("pass_type_id = '" +  passTypeID.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("pass_type_id = NULL, ");
				}				
				if (!eventTime.Equals(new DateTime()))
				{
					sbUpdate.Append("event_time = '" +  eventTime.ToString(dateTimeformat) + "', ");
				}
				else
				{
					sbUpdate.Append("event_time = NULL, ");
				}	
				if (locationID != -1)
				{
					sbUpdate.Append("location_id = '" +  locationID.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("location_id = NULL, ");
				}	
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = GETDATE() ");
				sbUpdate.Append("WHERE employee_id = '" + employeeID.ToString().Trim() + "'");

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

		public bool delete(int employeeID)
		{
			bool isDeleted = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

			try
			{
				StringBuilder sbDelete = new StringBuilder();
				sbDelete.Append("DELETE FROM employee_location WHERE employee_id = '" + employeeID.ToString() + "'");
				
				SqlCommand cmd = new SqlCommand( sbDelete.ToString(), conn, sqlTrans);
				int res = cmd.ExecuteNonQuery();
				if (res != 0)
				{
					isDeleted = true;
				}
				sqlTrans.Commit();
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback("DELETE");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public EmployeeLocationTO find(int employeeID)
		{
			DataSet dataSet = new DataSet();
			EmployeeLocationTO emplLocation = new EmployeeLocationTO();
			try
			{
				SqlCommand cmd = new SqlCommand( "SELECT * FROM employee_location WHERE employee_id = '" + employeeID.ToString().Trim() + "'", conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "EmployeeLocation");
				DataTable table = dataSet.Tables["EmployeeLocation"];

				if (table.Rows.Count == 1)
				{
					emplLocation = new EmployeeLocationTO();
					emplLocation.EmployeeID = Int32.Parse(table.Rows[0]["employee_id"].ToString().Trim());
					
					if (table.Rows[0]["reader_id"] != DBNull.Value)
					{
						emplLocation.ReaderID = Int32.Parse(table.Rows[0]["reader_id"].ToString().Trim());
					}
					if (table.Rows[0]["antenna"] != DBNull.Value)
					{
						emplLocation.Antenna = Int32.Parse(table.Rows[0]["antenna"].ToString().Trim());
					}
					if (table.Rows[0]["pass_type_id"] != DBNull.Value)
					{
						emplLocation.PassTypeID = Int32.Parse(table.Rows[0]["pass_type_id"].ToString().Trim());
					}
					if (table.Rows[0]["event_time"] != DBNull.Value)
					{
						emplLocation.EventTime = (DateTime) table.Rows[0]["event_time"];
					}
					if (table.Rows[0]["location_id"] != DBNull.Value)
					{
						emplLocation.LocationID = Int32.Parse(table.Rows[0]["location_id"].ToString().Trim());
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return emplLocation;
		}
        public List<EmployeeLocationTO> getEmployeeLocations(EmployeeLocationTO emplLocTO, string locationID, string wUnits, DateTime fromDate, DateTime toDate)
        {
            DataSet dataSet = new DataSet();
            EmployeeLocationTO emplLocation = new EmployeeLocationTO();
            List<EmployeeLocationTO> emplLocationList = new List<EmployeeLocationTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT el.*, empl.first_name empl_first_name, empl.last_name empl_last_name, ");
                sb.Append("wu.name wu_name, pt.description pt_desc, loc.name loc_name ");
                sb.Append("FROM employee_location el, employees empl, working_units wu, pass_types pt, locations loc");

                sb.Append(" WHERE el.employee_id = empl.employee_id AND");
                sb.Append(" empl.status <> 'RETIRED' AND");
                sb.Append(" empl.working_unit_id = wu.working_unit_id AND");
                sb.Append(" el.pass_type_id = pt.pass_type_id AND");
                sb.Append(" el.location_id = loc.location_id AND");
                if (!fromDate.Equals(new DateTime()) && !toDate.Equals(new DateTime()))
                    sb.Append(" el.event_time >='" + fromDate.ToString(dateTimeformat) + "' AND el.event_time<='" + toDate.ToString(dateTimeformat) + "' AND");

                if (!wUnits.Equals(""))
                {
                    sb.Append(" empl.working_unit_id IN (" + wUnits.Trim() + ") AND");
                }

                if ((emplLocTO.EmployeeID != -1) || (emplLocTO.ReaderID != -1) ||
                    (emplLocTO.Antenna != -1) || (emplLocTO.PassTypeID != -1) ||
                    (!emplLocTO.EventTime.Equals(new DateTime())) || (!locationID.Trim().Equals("")))
                {
                    if (emplLocTO.EmployeeID != -1)
                    {
                        sb.Append(" el.employee_id = '" + emplLocTO.EmployeeID.ToString().Trim() + "' AND");
                    }
                    if (emplLocTO.ReaderID != -1)
                    {
                        sb.Append(" el.reader_id = '" + emplLocTO.ReaderID.ToString().Trim() + "' AND");
                    }
                    if (emplLocTO.Antenna != -1)
                    {
                        sb.Append(" el.antenna = '" + emplLocTO.Antenna.ToString().Trim() + "' AND");
                    }
                    if (emplLocTO.PassTypeID != -1)
                    {
                        sb.Append(" el.pass_type_id = '" + emplLocTO.PassTypeID.ToString().Trim() + "' AND");
                    }
                    if (!emplLocTO.EventTime.Equals(new DateTime()))
                    {
                        sb.Append(" el.eventTime = '" + emplLocTO.EventTime.ToString(dateTimeformat).Trim() + "' AND");
                    }
                    if (!locationID.Trim().Equals(""))
                    {
                        sb.Append(" el.location_id IN (" + locationID.Trim() + ") AND");
                    }
                }

                select = sb.ToString(0, sb.ToString().Length - 3);

                select += " ORDER BY empl_last_name, empl_first_name";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeLocation");
                DataTable table = dataSet.Tables["EmployeeLocation"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        emplLocation = new EmployeeLocationTO();
                        emplLocation.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());

                        if (row["reader_id"] != DBNull.Value)
                        {
                            emplLocation.ReaderID = Int32.Parse(row["reader_id"].ToString().Trim());
                        }
                        if (row["antenna"] != DBNull.Value)
                        {
                            emplLocation.Antenna = Int32.Parse(row["antenna"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            emplLocation.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["event_time"] != DBNull.Value)
                        {
                            emplLocation.EventTime = (DateTime)row["event_time"];
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            emplLocation.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["wu_name"] != DBNull.Value)
                        {
                            emplLocation.WUName = row["wu_name"].ToString().Trim();
                        }
                        if (row["loc_name"] != DBNull.Value)
                        {
                            emplLocation.LocName = row["loc_name"].ToString().Trim();
                        }
                        if (row["pt_desc"] != DBNull.Value)
                        {
                            emplLocation.PassType = row["pt_desc"].ToString().Trim();
                        }
                        if (row["empl_last_name"] != DBNull.Value)
                        {
                            emplLocation.EmployeeName = row["empl_last_name"].ToString().Trim();
                        }
                        if (row["empl_first_name"] != DBNull.Value)
                        {
                            emplLocation.EmployeeName += " " + row["empl_first_name"].ToString().Trim();
                        }

                        emplLocationList.Add(emplLocation);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return emplLocationList;
        }

        public List<EmployeeLocationTO> getEmployeeLocations(EmployeeLocationTO emplLocTO, string locationID, string wUnits)
		{
			DataSet dataSet = new DataSet();
			EmployeeLocationTO emplLocation = new EmployeeLocationTO();
            List<EmployeeLocationTO> emplLocationList = new List<EmployeeLocationTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT el.*, empl.first_name empl_first_name, empl.last_name empl_last_name, ");
				sb.Append("wu.name wu_name, pt.description pt_desc, loc.name loc_name ");
				sb.Append("FROM employee_location el, employees empl, working_units wu, pass_types pt, locations loc");

                sb.Append(" WHERE el.employee_id = empl.employee_id AND");
                sb.Append(" empl.status <> 'RETIRED' AND");
				sb.Append(" empl.working_unit_id = wu.working_unit_id AND");
				sb.Append(" el.pass_type_id = pt.pass_type_id AND");
				sb.Append(" el.location_id = loc.location_id AND");
				if (!wUnits.Equals(""))
				{
					sb.Append(" empl.working_unit_id IN (" + wUnits.Trim() + ") AND");
				}

                if ((emplLocTO.EmployeeID != -1) || (emplLocTO.ReaderID != -1) ||
                    (emplLocTO.Antenna != -1) || (emplLocTO.PassTypeID != -1) ||
                    (!emplLocTO.EventTime.Equals(new DateTime())) || (!locationID.Trim().Equals("")))
				{
                    if (emplLocTO.EmployeeID != -1)
					{
                        sb.Append(" el.employee_id = '" + emplLocTO.EmployeeID.ToString().Trim() + "' AND");
					}
                    if (emplLocTO.ReaderID != -1)
					{
                        sb.Append(" el.reader_id = '" + emplLocTO.ReaderID.ToString().Trim() + "' AND");
					}
                    if (emplLocTO.Antenna != -1)
					{
                        sb.Append(" el.antenna = '" + emplLocTO.Antenna.ToString().Trim() + "' AND");
					}
                    if (emplLocTO.PassTypeID != -1)
					{
                        sb.Append(" el.pass_type_id = '" + emplLocTO.PassTypeID.ToString().Trim() + "' AND");
					}
                    if (!emplLocTO.EventTime.Equals(new DateTime()))
					{
                        sb.Append(" el.eventTime = '" + emplLocTO.EventTime.ToString(dateTimeformat).Trim() + "' AND");						
					}
                    if (!locationID.Trim().Equals(""))
                    {
                        sb.Append(" el.location_id IN (" + locationID.Trim() + ") AND");
                    }
				}

				select = sb.ToString(0, sb.ToString().Length - 3);

				select += " ORDER BY empl_last_name, empl_first_name";

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "EmployeeLocation");
				DataTable table = dataSet.Tables["EmployeeLocation"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						emplLocation = new EmployeeLocationTO();
						emplLocation.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
					
						if (row["reader_id"] != DBNull.Value)
						{
							emplLocation.ReaderID = Int32.Parse(row["reader_id"].ToString().Trim());
						}
						if (row["antenna"] != DBNull.Value)
						{
							emplLocation.Antenna = Int32.Parse(row["antenna"].ToString().Trim());
						}
						if (row["pass_type_id"] != DBNull.Value)
						{
							emplLocation.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
						}
						if (row["event_time"] != DBNull.Value)
						{
							emplLocation.EventTime = (DateTime) row["event_time"];
						}
						if (row["location_id"] != DBNull.Value)
						{
							emplLocation.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
						}
						if (row["wu_name"] != DBNull.Value)
						{
							emplLocation.WUName = row["wu_name"].ToString().Trim();
						}
						if (row["loc_name"] != DBNull.Value)
						{
							emplLocation.LocName = row["loc_name"].ToString().Trim();
						}
						if (row["pt_desc"] != DBNull.Value)
						{
							emplLocation.PassType = row["pt_desc"].ToString().Trim();
						}
						if (row["empl_last_name"] != DBNull.Value)
						{
							emplLocation.EmployeeName = row["empl_last_name"].ToString().Trim();
						}
						if (row["empl_first_name"] != DBNull.Value)
						{
							emplLocation.EmployeeName += " " + row["empl_first_name"].ToString().Trim();
						}

						emplLocationList.Add(emplLocation);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return emplLocationList;
		}

        public List<EmployeeLocationTO> getEmployeeLocationsAll(EmployeeLocationTO emplLocTO)
		{
			DataSet dataSet = new DataSet();
			EmployeeLocationTO emplLocation = new EmployeeLocationTO();
            List<EmployeeLocationTO> emplLocationList = new List<EmployeeLocationTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT * FROM employee_location ");

                if ((emplLocTO.EmployeeID != -1) || (emplLocTO.ReaderID != -1) ||
                    (emplLocTO.Antenna != -1) || (emplLocTO.PassTypeID != -1) ||
                    (!emplLocTO.EventTime.Equals(new DateTime())) || (emplLocTO.LocationID != -1))
				{	
					sb.Append("WHERE");
                    if (emplLocTO.EmployeeID != -1)
					{
                        sb.Append(" employee_id = '" + emplLocTO.EmployeeID.ToString().Trim() + "' AND");
					}
                    if (emplLocTO.ReaderID != -1)
					{
                        sb.Append(" reader_id = '" + emplLocTO.ReaderID.ToString().Trim() + "' AND");
					}
                    if (emplLocTO.Antenna != -1)
					{
                        sb.Append(" antenna = '" + emplLocTO.Antenna.ToString().Trim() + "' AND");
					}
                    if (emplLocTO.PassTypeID != -1)
					{
                        sb.Append(" pass_type_id = '" + emplLocTO.PassTypeID.ToString().Trim() + "' AND");
					}
                    if (!emplLocTO.EventTime.Equals(new DateTime()))
					{
                        sb.Append(" eventTime = '" + emplLocTO.EventTime.ToString(dateTimeformat).Trim() + "' AND");						
					}
                    if (emplLocTO.LocationID != -1)
					{
                        sb.Append(" location_id = '" + emplLocTO.LocationID.ToString().Trim() + "' AND");
					}

					select = sb.ToString(0, sb.ToString().Length - 3);
				}
				else
				{
					select = sb.ToString();
				}				

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "EmployeeLocation");
				DataTable table = dataSet.Tables["EmployeeLocation"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						emplLocation = new EmployeeLocationTO();
						emplLocation.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
					
						if (row["reader_id"] != DBNull.Value)
						{
							emplLocation.ReaderID = Int32.Parse(row["reader_id"].ToString().Trim());
						}
						if (row["antenna"] != DBNull.Value)
						{
							emplLocation.Antenna = Int32.Parse(row["antenna"].ToString().Trim());
						}
						if (row["pass_type_id"] != DBNull.Value)
						{
							emplLocation.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
						}
						if (row["event_time"] != DBNull.Value)
						{
							emplLocation.EventTime = (DateTime) row["event_time"];
						}
						if (row["location_id"] != DBNull.Value)
						{
							emplLocation.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
						}
						
						emplLocationList.Add(emplLocation);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return emplLocationList;
		}

        public List<EmployeeLocationTO> getEmployeeLocationsMittal(string workingUnitsOther)
		{
			DataSet dataSet = new DataSet();
			EmployeeLocationTO emplLocation = new EmployeeLocationTO();
            List<EmployeeLocationTO> emplLocationList = new List<EmployeeLocationTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT el.location_id, count(*) number ");				
				sb.Append("FROM employee_location el, employees empl");
				sb.Append(" WHERE el.employee_id = empl.employee_id AND");	
				sb.Append(" (el.location_id <> -1 AND el.location_id IS NOT NULL) AND");							
				if (!workingUnitsOther.Equals(""))
				{					
					sb.Append(" empl.working_unit_id NOT IN ( " + workingUnitsOther.Trim() + " ) AND");
				}
				select = sb.ToString(0, sb.ToString().Length - 3);

				select += " GROUP BY el.location_id";

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "EmployeeLocation");
				DataTable table = dataSet.Tables["EmployeeLocation"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						emplLocation = new EmployeeLocationTO();

						if (row["location_id"] != DBNull.Value)
						{
							emplLocation.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
						}
					
						//put count(*) in reader_id property
						if (row["number"] != DBNull.Value)
						{
							emplLocation.ReaderID = Int32.Parse(row["number"].ToString().Trim());
						}
						
						emplLocationList.Add(emplLocation);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return emplLocationList;
		}

        public List<EmployeeLocationTO> getEmployeeLocationsOther(string workingUnitsOther)
		{
			DataSet dataSet = new DataSet();
			EmployeeLocationTO emplLocation = new EmployeeLocationTO();
            List<EmployeeLocationTO> emplLocationList = new List<EmployeeLocationTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT el.location_id, count(*) number ");				
				sb.Append("FROM employee_location el, employees empl");
				sb.Append(" WHERE el.employee_id = empl.employee_id AND");	
				sb.Append(" (el.location_id <> -1 AND el.location_id IS NOT NULL) AND");
				if (!workingUnitsOther.Equals(""))
				{					
					sb.Append(" empl.working_unit_id IN ( " + workingUnitsOther.Trim() + " ) AND");
				}
				select = sb.ToString(0, sb.ToString().Length - 3);

				select += " GROUP BY el.location_id";

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "EmployeeLocation");
				DataTable table = dataSet.Tables["EmployeeLocation"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						emplLocation = new EmployeeLocationTO();

						if (row["location_id"] != DBNull.Value)
						{
							emplLocation.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
						}
					
						//put count(*) in reader_id property
						if (row["number"] != DBNull.Value)
						{
							emplLocation.ReaderID = Int32.Parse(row["number"].ToString().Trim());
						}
						
						emplLocationList.Add(emplLocation);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return emplLocationList;
		}

		public int getTotalMittalOut(string workingUnitsOther)
		{
			DataSet dataSet = new DataSet();
			EmployeeLocationTO emplLocation = new EmployeeLocationTO();
			
			string select = "";
			int count = 0;

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT count(*) number ");				
				sb.Append("FROM employee_location el, employees empl");
				sb.Append(" WHERE el.employee_id = empl.employee_id AND");	
				sb.Append(" el.location_id = -1 AND");							
				if (!workingUnitsOther.Equals(""))
				{					
					sb.Append(" empl.working_unit_id NOT IN ( " + workingUnitsOther.Trim() + " ) AND");
				}
				select = sb.ToString(0, sb.ToString().Length - 3);

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "EmployeeLocation");
				DataTable table = dataSet.Tables["EmployeeLocation"];
				
				if (table.Rows.Count == 1)
				{
					if (table.Rows[0]["number"] != DBNull.Value)
					{
						count = Int32.Parse(table.Rows[0]["number"].ToString());
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return count;
		}

		public int getTotalOtherOut(string workingUnitsOther)
		{
			DataSet dataSet = new DataSet();
			EmployeeLocationTO emplLocation = new EmployeeLocationTO();
			
			string select = "";
			int count = 0;

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT count(*) number ");				
				sb.Append("FROM employee_location el, employees empl");
				sb.Append(" WHERE el.employee_id = empl.employee_id AND");	
				sb.Append(" el.location_id = -1 AND");							
				if (!workingUnitsOther.Equals(""))
				{					
					sb.Append(" empl.working_unit_id IN ( " + workingUnitsOther.Trim() + " ) AND");
				}
				select = sb.ToString(0, sb.ToString().Length - 3);

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "EmployeeLocation");
				DataTable table = dataSet.Tables["EmployeeLocation"];
				
				if (table.Rows.Count == 1)
				{
					if (table.Rows[0]["number"] != DBNull.Value)
					{
						count = Int32.Parse(table.Rows[0]["number"].ToString());
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return count;
		}

        public List<EmployeeLocationTO> getEmployeeLocationsMittalDet(string locationID, string workingUnitsOther)
		{			
			DataSet dataSet = new DataSet();
			EmployeeLocationTO emplLocation = new EmployeeLocationTO();
            List<EmployeeLocationTO> emplLocationList = new List<EmployeeLocationTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT el.employee_id, el.event_time, empl.first_name empl_first_name, empl.last_name empl_last_name, ");
				sb.Append("wu.name wu_name, pt.description pt_desc ");				
				sb.Append("FROM employee_location el, employees empl, working_units wu, pass_types pt");
				sb.Append(" WHERE el.employee_id = empl.employee_id AND");
				sb.Append(" empl.working_unit_id = wu.working_unit_id AND");
				sb.Append(" el.pass_type_id = pt.pass_type_id AND");
								
				if (!workingUnitsOther.Equals(""))
				{					
					sb.Append(" empl.working_unit_id NOT IN ( " + workingUnitsOther.Trim() + " ) AND");
				}
				if (!locationID.ToString().Trim().Equals(""))
				{				
					sb.Append(" el.location_id = " + Int32.Parse(locationID.Trim()) + " AND");
				}

				select = sb.ToString(0, sb.ToString().Length - 3);

				select += " ORDER BY empl_last_name, empl_first_name";

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "EmployeeLocation");
				DataTable table = dataSet.Tables["EmployeeLocation"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						emplLocation = new EmployeeLocationTO();

						emplLocation.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());																																	
						if (row["event_time"] != DBNull.Value)
						{
							emplLocation.EventTime = (DateTime) row["event_time"];
						}
						if (row["wu_name"] != DBNull.Value)
						{
							emplLocation.WUName = row["wu_name"].ToString().Trim();
						}
						if (row["pt_desc"] != DBNull.Value)
						{
							emplLocation.PassType = row["pt_desc"].ToString().Trim();
						}
						if (row["empl_last_name"] != DBNull.Value)
						{
							emplLocation.EmployeeName = row["empl_last_name"].ToString().Trim();
						}						
						if (row["empl_first_name"] != DBNull.Value)
						{
							emplLocation.EmployeeName += " " + row["empl_first_name"].ToString().Trim();
						}
						
						emplLocationList.Add(emplLocation);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return emplLocationList;
		}

        public List<EmployeeLocationTO> getEmployeeLocationsOtherDet(string locationID, string workingUnitsOther)
		{
			DataSet dataSet = new DataSet();
			EmployeeLocationTO emplLocation = new EmployeeLocationTO();
            List<EmployeeLocationTO> emplLocationList = new List<EmployeeLocationTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT el.employee_id, el.event_time, empl.first_name empl_first_name, empl.last_name empl_last_name, ");
				sb.Append("wu.name wu_name, pt.description pt_desc ");				
				sb.Append("FROM employee_location el, employees empl, working_units wu, pass_types pt");
				sb.Append(" WHERE el.employee_id = empl.employee_id AND");
				sb.Append(" empl.working_unit_id = wu.working_unit_id AND");
				sb.Append(" el.pass_type_id = pt.pass_type_id AND");
								
				if (!workingUnitsOther.Equals(""))
				{					
					sb.Append(" empl.working_unit_id IN ( " + workingUnitsOther.Trim() + " ) AND");
				}
				if (!locationID.ToString().Trim().Equals(""))
				{
					sb.Append(" el.location_id = " + Int32.Parse(locationID.Trim()) + " AND");
				}

				select = sb.ToString(0, sb.ToString().Length - 3);

				select += " ORDER BY empl_last_name, empl_first_name";

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "EmployeeLocation");
				DataTable table = dataSet.Tables["EmployeeLocation"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						emplLocation = new EmployeeLocationTO();

						emplLocation.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());																																	
						if (row["event_time"] != DBNull.Value)
						{
							emplLocation.EventTime = (DateTime) row["event_time"];
						}
						if (row["wu_name"] != DBNull.Value)
						{
							emplLocation.WUName = row["wu_name"].ToString().Trim();
						}
						if (row["pt_desc"] != DBNull.Value)
						{
							emplLocation.PassType = row["pt_desc"].ToString().Trim();
						}
						if (row["empl_last_name"] != DBNull.Value)
						{
							emplLocation.EmployeeName = row["empl_last_name"].ToString().Trim();
						}						
						if (row["empl_first_name"] != DBNull.Value)
						{
							emplLocation.EmployeeName += " " + row["empl_first_name"].ToString().Trim();
						}
						
						emplLocationList.Add(emplLocation);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return emplLocationList;
		}

        public List<EmployeeLocationTO> getEmployeeLocationsIn(string wuID)
        {
            DataSet dataSet = new DataSet();
            EmployeeLocationTO emplLocation = new EmployeeLocationTO();
            List<EmployeeLocationTO> emplLocationList = new List<EmployeeLocationTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT el.location_id, count(*) number ");
                sb.Append("FROM employee_location el ");
                if (!wuID.Trim().Equals(""))
                {
                    sb.Append(", employees e ");
                }
                sb.Append("WHERE el.location_id <> -1 AND el.location_id IS NOT NULL ");
                if (!wuID.Trim().Equals(""))
                {
                    sb.Append("AND el.employee_id = e.employee_id AND e.working_unit_id IN (" + wuID.Trim() + ") ");
                }

                select = sb.ToString();
                select += "GROUP BY el.location_id";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeLocation");
                DataTable table = dataSet.Tables["EmployeeLocation"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        emplLocation = new EmployeeLocationTO();

                        if (row["location_id"] != DBNull.Value)
                        {
                            emplLocation.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }

                        //put count(*) in reader_id property
                        if (row["number"] != DBNull.Value)
                        {
                            emplLocation.ReaderID = Int32.Parse(row["number"].ToString().Trim());
                        }

                        emplLocationList.Add(emplLocation);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return emplLocationList;
        }

        public List<EmployeeLocationTO> getEmployeeLocationsInByWU(string locationID)
        {
            DataSet dataSet = new DataSet();
            EmployeeLocationTO emplLocation = new EmployeeLocationTO();
            List<EmployeeLocationTO> emplLocationList = new List<EmployeeLocationTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT e.working_unit_id wu_id, COUNT(*) number ");
                sb.Append("FROM employee_location el, employees e ");
                sb.Append("WHERE el.employee_id = e.employee_id AND ");
                sb.Append("el.location_id <> -1 AND el.location_id IS NOT NULL ");

                if (!locationID.Trim().Equals(""))
                {
                    sb.Append("AND el.location_id IN (" + locationID.Trim() + ") ");
                }

                select = sb.ToString();
                select += "GROUP BY e.working_unit_id";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeLocation");
                DataTable table = dataSet.Tables["EmployeeLocation"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        emplLocation = new EmployeeLocationTO();

                        if (row["wu_id"] != DBNull.Value)
                        {
                            emplLocation.WUID = Int32.Parse(row["wu_id"].ToString().Trim());
                        }

                        //put count(*) in reader_id property
                        if (row["number"] != DBNull.Value)
                        {
                            emplLocation.ReaderID = Int32.Parse(row["number"].ToString().Trim());
                        }

                        emplLocationList.Add(emplLocation);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return emplLocationList;
        }
	}
}
