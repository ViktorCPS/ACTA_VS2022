using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using TransferObjects;
using Util;
using System.Collections.Generic;

namespace DataAccess
{
	/// <summary>
	/// Summary description for MSSQLVisitDAO.
	/// </summary>
	public class MSSQLVisitDAO :VisitDAO
	{
		SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;
		protected string dateTimeformat = "";

		public MSSQLVisitDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MSSQLVisitDAO(SqlConnection sqlConnection)
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

		public int insert(int visitID, int employeeID, string firstName, string lastName, string visitorJMBG, string visitorID, DateTime dateStart, DateTime dateEnd, int visitedPerson, int visitedWorkingUnit, string visitDescr, int locationID, string remarks)
		{
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
			int rowsAffected = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO visits ");
				sbInsert.Append("(visit_id, employee_id, first_name, last_name, visitor_jmbg, visitor_id, date_start, date_end, visited_person, visited_working_unit, " +
					             "visit_descr, location_id, remarks, created_by, created_time) ");
				sbInsert.Append("VALUES (");

				if (visitID != -1)
				{
					sbInsert.Append("'" + visitID.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (employeeID != -1)
				{
					sbInsert.Append("'" + employeeID.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (!firstName.Trim().Equals(""))
				{
					sbInsert.Append("N'" + firstName.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (!lastName.Trim().Equals(""))
				{
					sbInsert.Append("N'" + lastName.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (!visitorJMBG.Trim().Equals(""))
				{
					sbInsert.Append("N'" + visitorJMBG.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (!visitorID.Trim().Equals(""))
				{
					sbInsert.Append("N'" + visitorID.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (!dateStart.Equals(new DateTime()))
				{
					sbInsert.Append("'" + dateStart.ToString(dateTimeformat).Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (!dateEnd.Equals(new DateTime()))
				{
					sbInsert.Append("'" + dateEnd.ToString(dateTimeformat).Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (visitedPerson != -1)
				{
					sbInsert.Append("'" + visitedPerson.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if ( visitedWorkingUnit != -1)
				{
					sbInsert.Append("'" + visitedWorkingUnit.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (!visitDescr.Trim().Equals(""))
				{
					sbInsert.Append("N'" + visitDescr.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if ( locationID != -1)
				{
					sbInsert.Append("'" + locationID.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (!remarks.Trim().Equals(""))
				{
					sbInsert.Append("N'" + remarks.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				
				sbInsert.Append(" N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");
					
				SqlCommand cmd = new SqlCommand( sbInsert.ToString(), conn, sqlTrans );
				rowsAffected = cmd.ExecuteNonQuery();
				sqlTrans.Commit();
				
			}
			catch(SqlException sqlEx)
			{
				sqlTrans.Rollback("INSERT");
				if (sqlEx.Number == 2627)
				{
					throw new Exception(sqlEx.Number.ToString());
				}
				else
				{
					throw new Exception(sqlEx.Message);
				}
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback("INSERT");
				throw ex;
			}

			return rowsAffected;
		}

		public int insert(VisitTO visitTO)
		{
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
			int visitID = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("SET NOCOUNT ON ");
				sbInsert.Append("INSERT INTO visits ");
				sbInsert.Append("(employee_id, first_name, last_name, visitor_jmbg, visitor_id, date_start, date_end, visited_person, visited_working_unit, " +
					"visit_descr, location_id, remarks, created_by, created_time) ");
				sbInsert.Append("VALUES (");
				
				if (visitTO.EmployeeID != -1)
				{
					sbInsert.Append("'" + visitTO.EmployeeID.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (!visitTO.FirstName.Trim().Equals(""))
				{
					sbInsert.Append("N'" + visitTO.FirstName.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (!visitTO.LastName.Trim().Equals(""))
				{
					sbInsert.Append("N'" + visitTO.LastName.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (!visitTO.VisitorJMBG.Trim().Equals(""))
				{
					sbInsert.Append("N'" + visitTO.VisitorJMBG.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (!visitTO.VisitorID.Trim().Equals(""))
				{
					sbInsert.Append("N'" + visitTO.VisitorID.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (!visitTO.DateStart.Equals(new DateTime()))
				{
					sbInsert.Append("'" + visitTO.DateStart.ToString(dateTimeformat).Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (!visitTO.DateEnd.Equals(new DateTime()))
				{
					sbInsert.Append("'" + visitTO.DateEnd.ToString(dateTimeformat).Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (visitTO.VisitedPerson != -1)
				{
					sbInsert.Append("'" + visitTO.VisitedPerson.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if ( visitTO.VisitedWorkingUnit != -1)
				{
					sbInsert.Append("'" + visitTO.VisitedWorkingUnit.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (!visitTO.VisitDescr.Trim().Equals(""))
				{
					sbInsert.Append("N'" + visitTO.VisitDescr.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if ( visitTO.LocationID != -1)
				{
					sbInsert.Append("'" + visitTO.LocationID.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (!visitTO.Remarks.Trim().Equals(""))
				{
					sbInsert.Append("N'" + visitTO.Remarks.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				
				sbInsert.Append(" N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");
                sbInsert.Append("SELECT @@Identity AS visit_id ");
                sbInsert.Append("SET NOCOUNT OFF ");

                DataSet dataSet = new DataSet();
                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                sqlDataAdapter.Fill(dataSet, "Visits");
                DataTable table = dataSet.Tables["Visits"];
                visitID = Int32.Parse(table.Rows[0]["visit_id"].ToString());
				
                sqlTrans.Commit();
			}
			catch(SqlException sqlEx)
			{
				sqlTrans.Rollback("INSERT");
				if (sqlEx.Number == 2627)
				{
					throw new Exception(sqlEx.Number.ToString());
				}
				else
				{
					throw new Exception(sqlEx.Message);
				}
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback("INSERT");
				throw ex;
			}
			
			return visitID;
		}

        public List<string> getDistinctVisitType()
        {
            List<string> visitType = new List<string>();
            DataSet dataSet = new DataSet();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT visit_descr from visits ORDER BY visit_descr");
                select = sb.ToString().Trim();
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visit");
                DataTable table = dataSet.Tables["Visit"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {

                        if (row["visit_descr"] != DBNull.Value)
                        {
                            visitType.Add(row["visit_descr"].ToString().Trim());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }
            return visitType;
        }

        public int insert(VisitTO visitTO, bool doCommit)
        {
            SqlTransaction sqlTrans = null;
            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }
            int visitID = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("SET NOCOUNT ON ");
                sbInsert.Append("INSERT INTO visits ");
                sbInsert.Append("(employee_id, first_name, last_name, visitor_jmbg, visitor_id, date_start, date_end, visited_person, visited_working_unit, " +
                    "visit_descr, location_id, remarks, company, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                if (visitTO.EmployeeID != -1)
                {
                    sbInsert.Append("'" + visitTO.EmployeeID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (!visitTO.FirstName.Trim().Equals(""))
                {
                    sbInsert.Append("N'" + visitTO.FirstName.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (!visitTO.LastName.Trim().Equals(""))
                {
                    sbInsert.Append("N'" + visitTO.LastName.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (!visitTO.VisitorJMBG.Trim().Equals(""))
                {
                    sbInsert.Append("N'" + visitTO.VisitorJMBG.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (!visitTO.VisitorID.Trim().Equals(""))
                {
                    sbInsert.Append("N'" + visitTO.VisitorID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (!visitTO.DateStart.Equals(new DateTime()))
                {
                    sbInsert.Append("'" + visitTO.DateStart.ToString(dateTimeformat).Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (!visitTO.DateEnd.Equals(new DateTime()))
                {
                    sbInsert.Append("'" + visitTO.DateEnd.ToString(dateTimeformat).Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (visitTO.VisitedPerson != -1)
                {
                    sbInsert.Append("'" + visitTO.VisitedPerson.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (visitTO.VisitedWorkingUnit != -1)
                {
                    sbInsert.Append("'" + visitTO.VisitedWorkingUnit.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (!visitTO.VisitDescr.Trim().Equals(""))
                {
                    sbInsert.Append("N'" + visitTO.VisitDescr.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (visitTO.LocationID != -1)
                {
                    sbInsert.Append("'" + visitTO.LocationID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (!visitTO.Remarks.Trim().Equals(""))
                {
                    sbInsert.Append("N'" + visitTO.Remarks.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (!visitTO.Company.Trim().Equals(""))
                {
                    sbInsert.Append("N'" + visitTO.Company.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append(" N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");
                sbInsert.Append("SELECT @@Identity AS visit_id ");
                sbInsert.Append("SET NOCOUNT OFF ");

                DataSet dataSet = new DataSet();
                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                sqlDataAdapter.Fill(dataSet, "Visits");
                DataTable table = dataSet.Tables["Visits"];
                visitID = Int32.Parse(table.Rows[0]["visit_id"].ToString());
                if (doCommit)
                {
                    sqlTrans.Commit();
                }
            }
            catch (SqlException sqlEx)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback("INSERT");
                }
                if (sqlEx.Number == 2627)
                {
                    throw new Exception(sqlEx.Number.ToString());
                }
                else
                {
                    throw new Exception(sqlEx.Message);
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback("INSERT");
                }
                throw ex;
            }

            return visitID;
        }

		public bool delete(string visitID)
		{
			bool isDeleted = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

			try
			{
				StringBuilder sbDelete = new StringBuilder();
				sbDelete.Append("DELETE FROM visits WHERE visit_id = " + Int32.Parse(visitID.Trim()));
				
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

		public VisitTO find(string employeeID)
		{
			DataSet dataSet = new DataSet();
			VisitTO visit = new VisitTO();
			
			try
			{
				string select = "SELECT * "
					+ "FROM visits "
					+ "WHERE employee_id = '" + employeeID.Trim() + "' AND date_end is null";					

				SqlCommand cmd = new SqlCommand(select, conn);
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Visit");
				DataTable table = dataSet.Tables["Visit"];

				if (table.Rows.Count == 1)
				{
					visit = new VisitTO();
					visit.VisitID = Int32.Parse(table.Rows[0]["visit_id"].ToString().Trim());
					
					if (table.Rows[0]["employee_id"] != DBNull.Value)
					{
						visit.EmployeeID = Int32.Parse(table.Rows[0]["employee_id"].ToString().Trim());
					}
					if (table.Rows[0]["first_name"] != DBNull.Value)
					{
						visit.FirstName = table.Rows[0]["first_name"].ToString().Trim();
					}
					if (table.Rows[0]["last_name"] != DBNull.Value)
					{
						visit.LastName = table.Rows[0]["last_name"].ToString().Trim();
					}
					if (table.Rows[0]["visitor_jmbg"] != DBNull.Value)
					{
						visit.VisitorJMBG = table.Rows[0]["visitor_jmbg"].ToString().Trim();
					}
					if (table.Rows[0]["visitor_id"] != DBNull.Value)
					{
						visit.VisitorID = table.Rows[0]["visitor_id"].ToString().Trim();
					}
					if (table.Rows[0]["date_start"] != DBNull.Value)
					{
						visit.DateStart = (DateTime) table.Rows[0]["date_start"];
					}
					if (table.Rows[0]["date_end"] != DBNull.Value)
					{
						visit.DateEnd = (DateTime) table.Rows[0]["date_end"];
					}
					if (table.Rows[0]["visited_person"] != DBNull.Value)
					{
						visit.VisitedPerson = Int32.Parse(table.Rows[0]["visited_person"].ToString().Trim());
					}
					if (table.Rows[0]["visited_working_unit"] != DBNull.Value)
					{
						visit.VisitedWorkingUnit = Int32.Parse(table.Rows[0]["visited_working_unit"].ToString().Trim());
					}
					if (table.Rows[0]["visit_descr"] != DBNull.Value)
					{
						visit.VisitDescr = table.Rows[0]["visit_descr"].ToString().Trim();
					}					
					if (table.Rows[0]["location_id"] != DBNull.Value)
					{
						visit.LocationID = Int32.Parse(table.Rows[0]["location_id"].ToString().Trim());
					}
					if (table.Rows[0]["remarks"] != DBNull.Value)
					{
						visit.Remarks = table.Rows[0]["remarks"].ToString().Trim();
					}					
				}
			}
			catch(Exception ex)
			{
				throw new Exception("Exception: " + ex.Message);
			}
            
			return visit;
		}

        public VisitTO findMAXVisitPIN(string PIN)
        {
            DataSet dataSet = new DataSet();
            VisitTO visit = new VisitTO();

            try
            {
                string select = "SELECT * FROM visits WHERE visitor_jmbg LIKE '%" + PIN.Trim() + "%' AND "
                        + "visit_id = (SELECT MAX(visit_id) FROM visits WHERE visitor_jmbg LIKE '%" + PIN.Trim() + "%')";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visit");
                DataTable table = dataSet.Tables["Visit"];

                if (table.Rows.Count == 1)
                {
                    visit = new VisitTO();
                    visit.VisitID = Int32.Parse(table.Rows[0]["visit_id"].ToString().Trim());

                    if (table.Rows[0]["employee_id"] != DBNull.Value)
                    {
                        visit.EmployeeID = Int32.Parse(table.Rows[0]["employee_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["first_name"] != DBNull.Value)
                    {
                        visit.FirstName = table.Rows[0]["first_name"].ToString().Trim();
                    }
                    if (table.Rows[0]["last_name"] != DBNull.Value)
                    {
                        visit.LastName = table.Rows[0]["last_name"].ToString().Trim();
                    }
                    if (table.Rows[0]["visitor_jmbg"] != DBNull.Value)
                    {
                        visit.VisitorJMBG = table.Rows[0]["visitor_jmbg"].ToString().Trim();
                    }
                    if (table.Rows[0]["visitor_id"] != DBNull.Value)
                    {
                        visit.VisitorID = table.Rows[0]["visitor_id"].ToString().Trim();
                    }
                    if (table.Rows[0]["date_start"] != DBNull.Value)
                    {
                        visit.DateStart = DateTime.Parse(table.Rows[0]["date_start"].ToString().Trim());
                    }
                    if (table.Rows[0]["date_end"] != DBNull.Value)
                    {
                        visit.DateEnd = DateTime.Parse(table.Rows[0]["date_end"].ToString().Trim());
                    }
                    if (table.Rows[0]["visited_person"] != DBNull.Value)
                    {
                        visit.VisitedPerson = Int32.Parse(table.Rows[0]["visited_person"].ToString().Trim());
                    }
                    if (table.Rows[0]["visited_working_unit"] != DBNull.Value)
                    {
                        visit.VisitedWorkingUnit = Int32.Parse(table.Rows[0]["visited_working_unit"].ToString().Trim());
                    }
                    if (table.Rows[0]["visit_descr"] != DBNull.Value)
                    {
                        visit.VisitDescr = table.Rows[0]["visit_descr"].ToString().Trim();
                    }
                    if (table.Rows[0]["location_id"] != DBNull.Value)
                    {
                        visit.LocationID = Int32.Parse(table.Rows[0]["location_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["remarks"] != DBNull.Value)
                    {
                        visit.Remarks = table.Rows[0]["remarks"].ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return visit;
        }

        public VisitTO findMAXVisitIdCard(string idCard)
        {
            DataSet dataSet = new DataSet();
            VisitTO visit = new VisitTO();

            try
            {
                string select = "SELECT * FROM visits WHERE visitor_id LIKE '%" + idCard.Trim() + "%' AND "
                        + "visit_id = (SELECT MAX(visit_id) FROM visits WHERE visitor_id LIKE '%" + idCard.Trim() + "%')";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visit");
                DataTable table = dataSet.Tables["Visit"];

                if (table.Rows.Count == 1)
                {
                    visit = new VisitTO();
                    visit.VisitID = Int32.Parse(table.Rows[0]["visit_id"].ToString().Trim());

                    if (table.Rows[0]["employee_id"] != DBNull.Value)
                    {
                        visit.EmployeeID = Int32.Parse(table.Rows[0]["employee_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["first_name"] != DBNull.Value)
                    {
                        visit.FirstName = table.Rows[0]["first_name"].ToString().Trim();
                    }
                    if (table.Rows[0]["last_name"] != DBNull.Value)
                    {
                        visit.LastName = table.Rows[0]["last_name"].ToString().Trim();
                    }
                    if (table.Rows[0]["visitor_jmbg"] != DBNull.Value)
                    {
                        visit.VisitorJMBG = table.Rows[0]["visitor_jmbg"].ToString().Trim();
                    }
                    if (table.Rows[0]["visitor_id"] != DBNull.Value)
                    {
                        visit.VisitorID = table.Rows[0]["visitor_id"].ToString().Trim();
                    }
                    if (table.Rows[0]["date_start"] != DBNull.Value)
                    {
                        visit.DateStart = DateTime.Parse(table.Rows[0]["date_start"].ToString().Trim());
                    }
                    if (table.Rows[0]["date_end"] != DBNull.Value)
                    {
                        visit.DateEnd = DateTime.Parse(table.Rows[0]["date_end"].ToString().Trim());
                    }
                    if (table.Rows[0]["visited_person"] != DBNull.Value)
                    {
                        visit.VisitedPerson = Int32.Parse(table.Rows[0]["visited_person"].ToString().Trim());
                    }
                    if (table.Rows[0]["visited_working_unit"] != DBNull.Value)
                    {
                        visit.VisitedWorkingUnit = Int32.Parse(table.Rows[0]["visited_working_unit"].ToString().Trim());
                    }
                    if (table.Rows[0]["visit_descr"] != DBNull.Value)
                    {
                        visit.VisitDescr = table.Rows[0]["visit_descr"].ToString().Trim();
                    }
                    if (table.Rows[0]["location_id"] != DBNull.Value)
                    {
                        visit.LocationID = Int32.Parse(table.Rows[0]["location_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["remarks"] != DBNull.Value)
                    {
                        visit.Remarks = table.Rows[0]["remarks"].ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return visit;
        }
        public bool update(string firstName, string lastName, string jmbg)
        {
            bool isUpdated = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE visits SET ");
                
                if (!firstName.Trim().Equals(""))
                {
                    sbUpdate.Append("first_name = N'" + firstName.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("first_name = NULL, ");
                }
                if (!lastName.Trim().Equals(""))
                {
                    sbUpdate.Append("last_name = N'" + lastName.Trim() + "' ");
                }
                else
                {
                    sbUpdate.Append("last_name = NULL ");
                }
                
                sbUpdate.Append("WHERE visitor_jmbg = '" + jmbg.ToString().Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                sqlTrans.Commit();

            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("UPDATE");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

		public bool update(int visitID, int employeeID, string firstName, string lastName, string visitorJMBG, string visitorID, DateTime dateStart, DateTime dateEnd, int visitedPerson, int visitedWorkingUnit, string visitDescr, int locationID, string remarks)
		{
			bool isUpdated = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

			try
			{
				StringBuilder sbUpdate = new StringBuilder();
				sbUpdate.Append("UPDATE visits SET ");

				if (visitID != -1)
				{
					sbUpdate.Append("visit_id = " + visitID.ToString().Trim() + ", ");
				}
				else
				{
					sbUpdate.Append("visit_id = NULL, ");
				}
				if (employeeID != -1)
				{
					sbUpdate.Append("employee_id = " + employeeID.ToString().Trim() + ", ");
				}
				else
				{
					sbUpdate.Append("employee_id = NULL, ");
				}
				if (!firstName.Trim().Equals(""))
				{
					sbUpdate.Append("first_name = N'" +  firstName.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("first_name = NULL, ");
				}
				if (!lastName.Trim().Equals(""))
				{
					sbUpdate.Append("last_name = N'" +  lastName.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("last_name = NULL, ");
				}
				if (!visitorJMBG.Trim().Equals(""))
				{
					sbUpdate.Append("visitor_jmbg = N'" +  visitorJMBG.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("visitor_jmbg = NULL, ");
				}
				if (!visitorID.Trim().Equals(""))
				{
					sbUpdate.Append("visitor_id = N'" +  visitorID.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("visitor_id = NULL, ");
				}
				if (!dateStart.Equals(new DateTime()))
				{
					sbUpdate.Append("date_start = '" +  dateStart.ToString(dateTimeformat) + "', ");
				}
				else
				{
					sbUpdate.Append("date_start = NULL, ");
				}
				if (!dateEnd.Equals(new DateTime()))
				{
					sbUpdate.Append("date_end = '" +  dateEnd.ToString(dateTimeformat) + "', ");
				}
				else
				{
					sbUpdate.Append("date_end = NULL, ");
				}
				if (visitedPerson != -1)
				{
					sbUpdate.Append("visited_person = " + visitedPerson.ToString().Trim() + ", ");
				}
				else
				{
					sbUpdate.Append("visited_person = NULL, ");
				}
				if (visitedWorkingUnit != -1)
				{
					sbUpdate.Append("visited_working_unit = " + visitedWorkingUnit.ToString().Trim() + ", ");
				}
				else
				{
					sbUpdate.Append("visited_working_unit = NULL, ");
				}
				if (!visitDescr.Trim().Equals(""))
				{
					sbUpdate.Append("visit_descr = N'" +  visitDescr.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("visit_descr = NULL, ");
				}				
				if ( locationID != -1)
				{
					sbUpdate.Append("location_id = " +  locationID.ToString().Trim() + ", ");
				}
				else
				{
					sbUpdate.Append("location_id = NULL, ");
				}
				if (!remarks.Trim().Equals(""))
				{
					sbUpdate.Append("remarks = N'" +  remarks.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("remarks = NULL, ");
				}

				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = GETDATE() ");
				sbUpdate.Append("WHERE visit_id = '" + visitID.ToString().Trim() + "'");

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

		public bool update(VisitTO visitTO)
		{
			bool isUpdated = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

			try
			{
				StringBuilder sbUpdate = new StringBuilder();
				sbUpdate.Append("UPDATE visits SET ");

				if (!visitTO.DateEnd.Equals(new DateTime()))
				{
					sbUpdate.Append("date_end = '" +  visitTO.DateEnd.ToString(dateTimeformat) + "', ");
				}
				else
				{
					sbUpdate.Append("date_end = NULL, ");
				}

				if (!visitTO.Remarks.Trim().Equals(""))
				{
					sbUpdate.Append("remarks = N'" +  visitTO.Remarks.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("remarks = NULL, ");
				}
                
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = GETDATE() ");
				sbUpdate.Append("WHERE visit_id = " + visitTO.VisitID.ToString().Trim() );

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

        public bool updateVisit(VisitTO visitTO)
        {
            bool isUpdated = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE visits SET ");

                if (!visitTO.DateEnd.Equals(new DateTime()))
                {
                    sbUpdate.Append("date_end = '" + visitTO.DateEnd.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbUpdate.Append("date_end = NULL, ");
                }

                if (!visitTO.Remarks.Trim().Equals(""))
                {
                    sbUpdate.Append("remarks = N'" + visitTO.Remarks.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("remarks = NULL, ");
                }

                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE visit_id = " + visitTO.VisitID.ToString().Trim() );

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                sqlTrans.Commit();

            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("UPDATE");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }
        public ArrayList getVisitsUNIPROM(int visitID, int employeeID, string firstName,
            string lastName, string visitorJMBG, string visitorID, DateTime dateStart,
            DateTime dateEnd, int visitedPerson, int visitedWorkingUnit, string visitDescr,
            int locationID, string remarks)
        {
            DataSet dataSet = new DataSet();
            VisitTO visit = new VisitTO();
            ArrayList visitList = new ArrayList();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT v.*,e.first_name as first,e.last_name as last FROM visits v, employees e ");

                sb.Append(" WHERE v.employee_id = e.employee_id");

                if ((!visitID.Equals(-1)) ||
                    (!employeeID.Equals(-1)) || (!firstName.Trim().Equals("")) || (!lastName.Trim().Equals("")) ||
                    (!visitorJMBG.Trim().Equals("")) || (!visitorID.Trim().Equals("")) ||
                    (!dateStart.Equals(new DateTime())) || (!dateEnd.Equals(new DateTime())) ||
                    (!visitedPerson.Equals(-1)) || (!visitedWorkingUnit.Equals(-1)) ||
                    (!visitDescr.Trim().Equals("")) || (!locationID.Equals(-1)) ||
                    (!remarks.Trim().Equals("")))
                {
                    sb.Append(" AND ");

                    if (!visitID.Equals(-1))
                    {
                        sb.Append(" UPPER(v.visit_id) LIKE '" + visitID.ToString().Trim().ToUpper() + "' AND");
                    }
                    if (!employeeID.Equals(-1))
                    {
                        sb.Append(" UPPER(v.employee_id) LIKE '" + employeeID.ToString().Trim().ToUpper() + "' AND");
                    }
                    if (!firstName.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(v.first_name) LIKE N'" + firstName.Trim().ToUpper() + "' AND");
                    }
                    if (!lastName.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(v.last_name) LIKE N'" + lastName.Trim().ToUpper() + "' AND");
                    }
                    if (!visitorJMBG.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(v.visitor_jmbg) LIKE N'" + visitorJMBG.Trim().ToUpper() + "' AND");
                    }
                    if (!visitorID.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(v.visitor_id) LIKE N'" + visitorID.Trim().ToUpper() + "' AND");
                    }
                    if (!dateStart.Equals(new DateTime()))
                    {
                        sb.Append(" UPPER(v.date_start) LIKE '" + dateStart.ToString(dateTimeformat).Trim().ToUpper() + "' AND");
                    }
                    if (!dateEnd.Equals(new DateTime()))
                    {
                        sb.Append(" UPPER(v.date_end) LIKE '" + dateEnd.ToString(dateTimeformat).Trim().ToUpper() + "' AND");
                    }
                    if (!visitedPerson.Equals(-1))
                    {
                        sb.Append(" UPPER(v.visited_person) LIKE '" + visitedPerson.ToString().Trim().ToUpper() + "' AND");
                    }
                    if (!visitedWorkingUnit.Equals(-1))
                    {
                        sb.Append(" UPPER(v.visited_working_unit) LIKE '" + visitedWorkingUnit.ToString().Trim().ToUpper() + "' AND");
                    }
                    if (!visitDescr.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(v.visit_descr) LIKE N'" + visitDescr.Trim().ToUpper() + "' AND");
                    }
                    if (!locationID.Equals(-1))
                    {
                        sb.Append(" UPPER(v.location_id) LIKE '" + locationID.ToString().Trim().ToUpper() + "' AND");
                    }
                    if (!remarks.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(v.remarks) LIKE N'" + remarks.ToString().Trim().ToUpper() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }
                select = select + "ORDER BY v.date_start";
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visits");
                DataTable table = dataSet.Tables["Visits"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        visit = new VisitTO();
                        visit.VisitID = Int32.Parse(row["visit_id"].ToString().Trim());

                        if (row["employee_id"] != DBNull.Value)
                        {
                            visit.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["first"] != DBNull.Value)
                        {
                            visit.EmployeeFirstName = row["first"].ToString().Trim();
                        }
                        if (row["last"] != DBNull.Value)
                        {
                            visit.EmployeeLastName = row["last"].ToString().Trim();
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            visit.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            visit.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["visitor_id"] != DBNull.Value)
                        {
                            visit.VisitorID = row["visitor_id"].ToString().Trim();
                        }
                        if (row["visitor_jmbg"] != DBNull.Value)
                        {
                            visit.VisitorJMBG = row["visitor_jmbg"].ToString().Trim();
                        }
                        if (row["date_start"] != DBNull.Value)
                        {
                            visit.DateStart = (DateTime)row["date_start"];
                        }
                        if (row["date_end"] != DBNull.Value)
                        {
                            visit.DateEnd = (DateTime)row["date_end"];
                        }
                        if (row["visited_person"] != DBNull.Value)
                        {
                            visit.VisitedPerson = Int32.Parse(row["visited_person"].ToString().Trim());
                        }
                        if (row["visited_working_unit"] != DBNull.Value)
                        {
                            visit.VisitedWorkingUnit = Int32.Parse(row["visited_working_unit"].ToString().Trim());
                        }
                        if (row["visit_descr"] != DBNull.Value)
                        {
                            visit.VisitDescr = row["visit_descr"].ToString().Trim();
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            visit.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["remarks"] != DBNull.Value)
                        {
                            visit.Remarks = row["remarks"].ToString().Trim();
                        }

                        visitList.Add(visit);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return visitList;
        }
        public ArrayList getVisitsCompleted()
        { 
            DataSet dataSet = new DataSet();
			VisitTO visit = new VisitTO();
			ArrayList visitList = new ArrayList();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT * FROM visits WHERE date_end IS NOT NULL ");
                select = sb.ToString();
                select = select + "ORDER BY date_start";
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visits");
                DataTable table = dataSet.Tables["Visits"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        visit = new VisitTO();
                        visit.VisitID = Int32.Parse(row["visit_id"].ToString().Trim());

                        if (row["employee_id"] != DBNull.Value)
                        {
                            visit.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            visit.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            visit.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["visitor_id"] != DBNull.Value)
                        {
                            visit.VisitorID = row["visitor_id"].ToString().Trim();
                        }
                        if (row["visitor_jmbg"] != DBNull.Value)
                        {
                            visit.VisitorJMBG = row["visitor_jmbg"].ToString().Trim();
                        }
                        if (row["date_start"] != DBNull.Value)
                        {
                            visit.DateStart = (DateTime)row["date_start"];
                        }
                        if (row["date_end"] != DBNull.Value)
                        {
                            visit.DateEnd = (DateTime)row["date_end"];
                        }
                        if (row["visited_person"] != DBNull.Value)
                        {
                            visit.VisitedPerson = Int32.Parse(row["visited_person"].ToString().Trim());
                        }
                        if (row["visited_working_unit"] != DBNull.Value)
                        {
                            visit.VisitedWorkingUnit = Int32.Parse(row["visited_working_unit"].ToString().Trim());
                        }
                        if (row["visit_descr"] != DBNull.Value)
                        {
                            visit.VisitDescr = row["visit_descr"].ToString().Trim();
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            visit.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["remarks"] != DBNull.Value)
                        {
                            visit.Remarks = row["remarks"].ToString().Trim();
                        }
                        if (row["company"] != DBNull.Value)
                        {
                            visit.Company = row["company"].ToString().Trim();
                        }

                        visitList.Add(visit);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return visitList;
        }


        public DataSet getVisitsCompletedDS()
        {
            DataSet dataSet = new DataSet();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT v.visit_id ,v.visitor_id, v.first_name , v.last_name,v.visited_person , v.employee_id, v.visitor_jmbg , e.first_name visit_1, e.last_name visit_2, ");
                sb.Append("a.nvarchar_value_2 document_name, v.company , v.visit_descr description, a.datetime_value_1 privacy_statement, v.date_start ,v.date_end, ");
                sb.Append("a.nvarchar_value_8 local,a.nvarchar_value_1 visitor, a.datetime_value_2 as ban, a.integer_value_1 ban_active, ");
                sb.Append("a.nvarchar_value_3 place, a.nvarchar_value_4 state, UPPER(v.first_name) UPPER_first_name, UPPER(v.last_name) UPPER_last_name, ");
                sb.Append(" UPPER(a.nvarchar_value_3) UPPER_place, UPPER(a.nvarchar_value_4) UPPER_state, UPPER(v.company) UPPER_company, UPPER(a.nvarchar_value_1) UPPER_visitor, ");
                sb.Append("UPPER(a.nvarchar_value_2) UPPER_document_name,UPPER(v.visitor_id) UPPER_visitor_id, UPPER(v.visitor_jmbg) UPPER_visitor_jmbg, v.visited_working_unit visited_wu ");
                sb.Append (" FROM visits v, visits_asco4 a, employees e WHERE date_end IS NOT NULL ");
                sb.Append(" AND v.visit_id = a.visit_id");
                sb.Append(" AND v.employee_id = e.employee_id");
                select = sb.ToString();
                select = select + " ORDER BY date_start";
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visits");
                
                
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return dataSet;
        }



        public ArrayList getVisitsNotCompleted(bool notCompleted)
        {
            DataSet dataSet = new DataSet();
            VisitTO visit = new VisitTO();
            ArrayList visitList = new ArrayList();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM visits");
                if(notCompleted)
                sb.Append(" WHERE date_end IS NULL ");
                select = sb.ToString();
                select = select + "ORDER BY date_start";
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visits");
                DataTable table = dataSet.Tables["Visits"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        visit = new VisitTO();
                        visit.VisitID = Int32.Parse(row["visit_id"].ToString().Trim());

                        if (row["employee_id"] != DBNull.Value)
                        {
                            visit.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            visit.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            visit.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["visitor_id"] != DBNull.Value)
                        {
                            visit.VisitorID = row["visitor_id"].ToString().Trim();
                        }
                        if (row["visitor_jmbg"] != DBNull.Value)
                        {
                            visit.VisitorJMBG = row["visitor_jmbg"].ToString().Trim();
                        }
                        if (row["date_start"] != DBNull.Value)
                        {
                            visit.DateStart = (DateTime)row["date_start"];
                        }
                        if (row["date_end"] != DBNull.Value)
                        {
                            visit.DateEnd = (DateTime)row["date_end"];
                        }
                        if (row["visited_person"] != DBNull.Value)
                        {
                            visit.VisitedPerson = Int32.Parse(row["visited_person"].ToString().Trim());
                        }
                        if (row["visited_working_unit"] != DBNull.Value)
                        {
                            visit.VisitedWorkingUnit = Int32.Parse(row["visited_working_unit"].ToString().Trim());
                        }
                        if (row["visit_descr"] != DBNull.Value)
                        {
                            visit.VisitDescr = row["visit_descr"].ToString().Trim();
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            visit.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["remarks"] != DBNull.Value)
                        {
                            visit.Remarks = row["remarks"].ToString().Trim();
                        }
                        if (row["company"] != DBNull.Value)
                        {
                            visit.Company = row["company"].ToString().Trim();
                        }

                        visitList.Add(visit);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return visitList;
        }
		public ArrayList getVisits(int visitID, int employeeID, string firstName, 
			string lastName, string visitorJMBG, string visitorID, DateTime dateStart, 
			DateTime dateEnd, int visitedPerson, int visitedWorkingUnit, string visitDescr, 
			int locationID, string remarks)
		{
			DataSet dataSet = new DataSet();
			VisitTO visit = new VisitTO();
			ArrayList visitList = new ArrayList();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT * FROM visits ");

				if((!visitID.Equals(-1)) || 
					(!employeeID.Equals(-1)) || (!firstName.Trim().Equals("")) || (!lastName.Trim().Equals("")) ||
					(!visitorJMBG.Trim().Equals("")) || (!visitorID.Trim().Equals("")) ||
					(!dateStart.Equals(new DateTime())) || (!dateEnd.Equals(new DateTime())) || 
					(!visitedPerson.Equals(-1)) || (!visitedWorkingUnit.Equals(-1)) || 
					(!visitDescr.Trim().Equals("")) || (!locationID.Equals(-1)) || 
					(!remarks.Trim().Equals("")))
				{
					sb.Append(" WHERE ");
					
					if (!visitID.Equals(-1))
					{
						sb.Append(" UPPER(visit_id) LIKE '" + visitID.ToString().Trim().ToUpper() + "' AND");
					}					
					if (!employeeID.Equals(-1))
					{
						sb.Append(" UPPER(employee_id) LIKE '" + employeeID.ToString().Trim().ToUpper() + "' AND");
					}
					if (!firstName.Trim().Equals(""))
					{
						sb.Append(" UPPER(first_name) LIKE N'" + firstName.Trim().ToUpper() + "' AND");
					}
					if (!lastName.Trim().Equals(""))
					{
						sb.Append(" UPPER(last_name) LIKE N'" + lastName.Trim().ToUpper() + "' AND");
					}
					if (!visitorJMBG.Trim().Equals(""))
					{
						sb.Append(" UPPER(visitor_jmbg) LIKE N'" + visitorJMBG.Trim().ToUpper() + "' AND");
					}					
					if (!visitorID.Trim().Equals(""))
					{
						sb.Append(" UPPER(visitor_id) LIKE N'" + visitorID.Trim().ToUpper() + "' AND");
					}					
					if (!dateStart.Equals(new DateTime()))
					{
						sb.Append(" UPPER(date_start) LIKE '" + dateStart.ToString(dateTimeformat).Trim().ToUpper() + "' AND");
					}
					if (!dateEnd.Equals(new DateTime()))
					{
						sb.Append(" UPPER(date_end) LIKE '" + dateEnd.ToString(dateTimeformat).Trim().ToUpper() + "' AND");
					}
					if (!visitedPerson.Equals(-1))
					{
						sb.Append(" UPPER(visited_person) LIKE '" + visitedPerson.ToString().Trim().ToUpper() + "' AND");
					}
					if (!visitedWorkingUnit.Equals(-1))
					{
						sb.Append(" UPPER(visited_working_unit) LIKE '" + visitedWorkingUnit.ToString().Trim().ToUpper() + "' AND");
					}
					if (!visitDescr.Trim().Equals(""))
					{
						sb.Append(" UPPER(visit_descr) LIKE N'" + visitDescr.Trim().ToUpper() + "' AND");
					}					
					if (!locationID.Equals(-1))
					{
						sb.Append(" UPPER(location_id) LIKE '" + locationID.ToString().Trim().ToUpper() + "' AND");
					}
					if (!remarks.Trim().Equals(""))
					{
						sb.Append(" UPPER(remarks) LIKE N'" + remarks.ToString().Trim().ToUpper() + "' AND");
					}
					
					select = sb.ToString(0, sb.ToString().Length - 3);
				}
				else
				{
					select = sb.ToString();
				}
				select = select + "ORDER BY date_start";
				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Visits");
				DataTable table = dataSet.Tables["Visits"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						visit = new VisitTO();
						visit.VisitID = Int32.Parse(row["visit_id"].ToString().Trim());
						
						if (row["employee_id"] != DBNull.Value)
						{
							visit.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
						}
						if (row["first_name"] != DBNull.Value)
						{
							visit.FirstName = row["first_name"].ToString().Trim();
						}
						if (row["last_name"] != DBNull.Value)
						{
							visit.LastName = row["last_name"].ToString().Trim();
						}
						if (row["visitor_id"] != DBNull.Value)
						{
							visit.VisitorID = row["visitor_id"].ToString().Trim();
						}
						if (row["visitor_jmbg"] != DBNull.Value)
						{
							visit.VisitorJMBG = row["visitor_jmbg"].ToString().Trim();
						}
						if (row["date_start"] != DBNull.Value)
						{
							visit.DateStart = (DateTime) row["date_start"];
						}
						if (row["date_end"] != DBNull.Value)
						{
							visit.DateEnd = (DateTime) row["date_end"];
						}
						if (row["visited_person"] != DBNull.Value)
						{
							visit.VisitedPerson = Int32.Parse(row["visited_person"].ToString().Trim());
						}
						if (row["visited_working_unit"] != DBNull.Value)
						{
							visit.VisitedWorkingUnit = Int32.Parse(row["visited_working_unit"].ToString().Trim());
						}
						if (row["visit_descr"] != DBNull.Value)
						{
							visit.VisitDescr = row["visit_descr"].ToString().Trim();
						}
						if (row["location_id"] != DBNull.Value)
						{
							visit.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
						}
						if (row["remarks"] != DBNull.Value)
						{
							visit.Remarks = row["remarks"].ToString().Trim();
						}
                        if (row["company"] != DBNull.Value)
                        {
                            visit.Company = row["company"].ToString().Trim();
                        }

						visitList.Add(visit);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception("Exception: " + ex.Message);
			}

			return visitList;
		}

        public ArrayList getVisitsDateInterval(string employeeID, string visitedWorkingUnit,
            string visitedPerson, string visitorIdent, DateTime dateFrom, DateTime dateTo, 
            string visitDescr, string wUnits)
        {
            DataSet dataSet = new DataSet();
            VisitTO visit = new VisitTO();
            ArrayList visitList = new ArrayList();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" SELECT visits.*, e1.first_name AS EmplFirstName, e1.last_name AS EmplLastName, wu.name AS WUName");
                sb.Append(" FROM visits, employees AS e1, working_units AS wu");
                sb.Append(" WHERE visits.employee_id = e1.employee_id");
                sb.Append(" AND visits.visited_working_unit = wu.working_unit_id");

                if ((!employeeID.Trim().Equals("")) || (!visitedWorkingUnit.Equals("")) ||
                    (!visitedPerson.Equals("")) || (!visitorIdent.Trim().Equals("")) ||
                    ((!dateFrom.Equals(new DateTime())) && (!dateTo.Equals(new DateTime()))) ||
                    (!visitDescr.Trim().Equals("")) || (!wUnits.Trim().Equals("")))
                {
                    if (!wUnits.Trim().Equals(""))
                    {
                        sb.Append(" AND wu.working_unit_id IN (" + wUnits + ")");
                    }

                    if (!employeeID.Equals(""))
                    {
                        sb.Append(" AND visits.employee_id = " + employeeID);
                    }
                    if (!visitedWorkingUnit.Equals(""))
                    {
                        sb.Append(" AND visits.visited_working_unit = " + visitedWorkingUnit);
                    }
                    if (!visitedPerson.Equals(""))
                    {
                        sb.Append(" AND visits.visited_person = " + visitedPerson);
                    }
                    if (!visitorIdent.Trim().Equals(""))
                    {
                        sb.Append(" AND (visitor_jmbg = '" + visitorIdent.Trim() + "'");
                        sb.Append(" OR visitor_id = '" + visitorIdent.Trim() + "')");
                    }
                    if (!visitDescr.Trim().Equals(""))
                    {
                        sb.Append(" AND visit_descr = '" + visitDescr.Trim() + "'");
                    }

                    if ((!dateFrom.Equals(new DateTime())) && (!dateTo.Equals(new DateTime())))
                    {
                        sb.Append(" AND (");

                        sb.Append(" (");
                        sb.Append(" convert(datetime, convert(varchar(10), date_start, 111), 111) < convert(datetime,'" + dateFrom.ToString("yyyy-MM-dd") + "', 111)");
                        sb.Append(" AND (convert(datetime, convert(varchar(10), date_end, 111), 111) >= convert(datetime,'" + dateFrom.ToString("yyyy-MM-dd") + "', 111)");
                        sb.Append(" OR date_end IS NULL)");
                        sb.Append(" )");

                        sb.Append(" OR (");
                        sb.Append(" convert(datetime, convert(varchar(10), date_start, 111), 111) >= convert(datetime,'" + dateFrom.ToString("yyyy-MM-dd") + "', 111)");
                        sb.Append(" AND convert(datetime, convert(varchar(10), date_start, 111), 111) <= convert(datetime,'" + dateTo.ToString("yyyy-MM-dd") + "', 111)");
                        sb.Append(" )");

                        sb.Append(")");
                    } 	
                }
                select = sb.ToString() + " ORDER BY date_start";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visits");
                DataTable table = dataSet.Tables["Visits"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        visit = new VisitTO();
                        visit.VisitID = Int32.Parse(row["visit_id"].ToString().Trim());

                        if (row["employee_id"] != DBNull.Value)
                        {
                            visit.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            visit.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            visit.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["visitor_id"] != DBNull.Value)
                        {
                            visit.VisitorID = row["visitor_id"].ToString().Trim();
                        }
                        if (row["visitor_jmbg"] != DBNull.Value)
                        {
                            visit.VisitorJMBG = row["visitor_jmbg"].ToString().Trim();
                        }
                        if (row["date_start"] != DBNull.Value)
                        {
                            visit.DateStart = (DateTime)row["date_start"];
                        }
                        if (row["date_end"] != DBNull.Value)
                        {
                            visit.DateEnd = (DateTime)row["date_end"];
                        }
                        if (row["visited_person"] != DBNull.Value)
                        {
                            visit.VisitedPerson = Int32.Parse(row["visited_person"].ToString().Trim());
                        }
                        if (row["visited_working_unit"] != DBNull.Value)
                        {
                            visit.VisitedWorkingUnit = Int32.Parse(row["visited_working_unit"].ToString().Trim());
                        }
                        if (row["visit_descr"] != DBNull.Value)
                        {
                            visit.VisitDescr = row["visit_descr"].ToString().Trim();
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            visit.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["remarks"] != DBNull.Value)
                        {
                            visit.Remarks = row["remarks"].ToString().Trim();
                        }
                        if (row["company"] != DBNull.Value)
                        {
                            visit.Company= row["company"].ToString().Trim();
                        }
                        if (row["EmplFirstName"] != DBNull.Value)
                        {
                            visit.EmployeeFirstName = row["EmplFirstName"].ToString().Trim();
                        }
                        if (row["EmplLastName"] != DBNull.Value)
                        {
                            visit.EmployeeLastName = row["EmplLastName"].ToString().Trim();
                        }
                        if (row["WUName"] != DBNull.Value)
                        {
                            visit.WUName = row["WUName"].ToString().Trim();
                        }

                        visitList.Add(visit);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return visitList;
        }

        public ArrayList getVisitsDateInterval(string employeeID, string visitedWorkingUnit, string visitedPerson, string visitorIdent,
             DateTime dateFrom, DateTime dateTo, string visitDescr, string wUnits, string state, string company, string visitor, string privacy,
             string visitAsco4, Dictionary<int, List<VisitAsco4TO>> ascoTO)
        {
            DataSet dataSet = new DataSet();
            VisitTO visit = new VisitTO();
            ArrayList visitList = new ArrayList();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" SELECT visits.*, e1.first_name AS EmplFirstName, e1.last_name AS EmplLastName, wu.name AS WUName, ");
                sb.Append("asco4.nvarchar_value_2 as document_name,asco4.nvarchar_value_4 AS state,asco4.nvarchar_value_3 AS place,asco4.datetime_value_2 AS DatetimeValue2,asco4.integer_value_1 AS ban, asco4.datetime_value_1 AS privacy_statement, asco4.nvarchar_value_8 AS local,asco4.nvarchar_value_5 AS num");
                sb.Append(" FROM visits, employees AS e1, working_units AS wu, visits_asco4 AS asco4");
                sb.Append(" WHERE visits.employee_id = e1.employee_id");
                sb.Append(" AND visits.visited_working_unit = wu.working_unit_id");
                sb.Append(" AND visits.visit_id = asco4.visit_id");

                if ((!employeeID.Trim().Equals("")) || (!visitedWorkingUnit.Equals("")) ||
                    (!visitedPerson.Equals("")) || (!visitorIdent.Trim().Equals("")) ||
                    ((!dateFrom.Equals(new DateTime())) && (!dateTo.Equals(new DateTime()))) ||
                    (!visitDescr.Trim().Equals("")) || (!wUnits.Trim().Equals(""))
                    || (!state.Equals("")) || (!company.Equals("")) || (!visitor.Equals("")) ||
                    (!privacy.Equals(""))||(!visitAsco4.Equals("")))
                {
                    if (!wUnits.Trim().Equals(""))
                    {
                        sb.Append(" AND wu.working_unit_id IN (" + wUnits + ")");
                    }

                    if (!employeeID.Equals(""))
                    {
                        sb.Append(" AND visits.employee_id = " + employeeID);
                    }
                    if (!visitedWorkingUnit.Equals(""))
                    {
                        sb.Append(" AND visits.visited_working_unit = " + visitedWorkingUnit);
                    }
                    if (!visitedPerson.Equals(""))
                    {
                        sb.Append(" AND visits.visited_person = " + visitedPerson);
                    }
                    if (!visitorIdent.Trim().Equals(""))
                    {
                        sb.Append(" AND (UPPER(visitor_jmbg) = N'" + visitorIdent.Trim().ToUpper() + "'");
                        sb.Append(" OR UPPER(visitor_id) = N'" + visitorIdent.Trim().ToUpper() + "')");
                    }
                    if (!visitDescr.Trim().Equals(""))
                    {
                        sb.Append(" AND UPPER(visits.visit_descr) = '" + visitDescr.Trim().ToUpper() + "'");
                    }
                    if (!visitAsco4.Trim().Equals(""))
                    {
                        sb.Append(" AND UPPER(asco4.nvarchar_value_1) = N'" + visitAsco4.Trim().ToUpper() + "'");
                    }
                    if (!company.Trim().Equals(""))
                    {
                        sb.Append(" AND UPPER(visits.company) = N'" + company.Trim().ToUpper() + "'");
                    }
                    if (!state.Trim().Equals(""))
                    {
                        sb.Append(" AND UPPER(asco4.nvarchar_value_4) = N'" + state.Trim().ToUpper() + "'");
                    }
                    if (!visitor.Trim().Equals(""))
                    {
                        string[] v = new string[2];
                        if (visitor.Contains(" "))
                        {
                            v[0] = visitor.Substring(0, visitor.IndexOf(' ')).Trim();
                            v[1] = visitor.Substring(visitor.IndexOf(' ') + 1).Trim();
                        }
                        else
                        {
                            v[0] = visitor;
                            v[1] = "";
                        }
                        sb.Append(" AND UPPER(visits.first_name) = N'" + v[0].Trim().ToUpper() + "'");
                        if(!v[1].Equals(""))
                            sb.Append(" AND UPPER(visits.last_name) = N'" + v[1].Trim().ToUpper() + "'");
                    }

                    if ((!dateFrom.Equals(new DateTime())) && (!dateTo.Equals(new DateTime())))
                    {
                        sb.Append(" AND (");

                        sb.Append(" (");
                        sb.Append("  date_start < CONVERT(datetime,'" + dateFrom.ToString("yyyy-MM-dd HH:mm:ss") + "', 111) AND ");
                        sb.Append(" date_end >= CONVERT(datetime,'" + dateFrom.ToString("yyyy-MM-dd HH:mm:ss") + "', 111) ");
                        sb.Append(" )");

                        sb.Append(" OR (");
                        sb.Append("  date_start >= CONVERT(datetime,'" + dateFrom.ToString("yyyy-MM-dd HH:mm:ss") + "', 111) AND ");
                        sb.Append(" date_start <= CONVERT(datetime,'" + dateTo.ToString("yyyy-MM-dd HH:mm:ss") + "', 111) ");
                        sb.Append(" )");

                        sb.Append(")");
                    }
                    if (!privacy.Equals(""))
                    {
                        if (privacy.Equals("YES"))
                        {
                            sb.Append(" AND asco4.datetime_value_1 IS NOT NULL ");
                        }
                        if (privacy.Equals("NO"))
                        {
                            sb.Append(" AND asco4.datetime_value_1 IS NULL ");
                        }
                    }
                }
                select = sb.ToString() + " ORDER BY date_start";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visits");
                DataTable table = dataSet.Tables["Visits"];
                VisitAsco4TO asco4TO = new VisitAsco4TO();
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        visit = new VisitTO();
                        asco4TO = new VisitAsco4TO();
                        visit.VisitID = Int32.Parse(row["visit_id"].ToString().Trim());

                        if (row["employee_id"] != DBNull.Value)
                        {
                            visit.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            visit.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            visit.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["visitor_id"] != DBNull.Value)
                        {
                            visit.VisitorID = row["visitor_id"].ToString().Trim();
                        }
                        if (row["visitor_jmbg"] != DBNull.Value)
                        {
                            visit.VisitorJMBG = row["visitor_jmbg"].ToString().Trim();
                        }
                        if (row["date_start"] != DBNull.Value)
                        {
                            visit.DateStart = (DateTime)row["date_start"];
                        }
                        if (row["date_end"] != DBNull.Value)
                        {
                            visit.DateEnd = (DateTime)row["date_end"];
                        }
                        if (row["visited_person"] != DBNull.Value)
                        {
                            visit.VisitedPerson = Int32.Parse(row["visited_person"].ToString().Trim());
                        }
                        if (row["visited_working_unit"] != DBNull.Value)
                        {
                            visit.VisitedWorkingUnit = Int32.Parse(row["visited_working_unit"].ToString().Trim());
                        }
                        if (row["visit_descr"] != DBNull.Value)
                        {
                            visit.VisitDescr = row["visit_descr"].ToString().Trim();
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            visit.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["remarks"] != DBNull.Value)
                        {
                            visit.Remarks = row["remarks"].ToString().Trim();
                        }
                        if (row["company"] != DBNull.Value)
                        {
                            visit.Company = row["company"].ToString().Trim();
                        }
                        if (row["EmplFirstName"] != DBNull.Value)
                        {
                            visit.EmployeeFirstName = row["EmplFirstName"].ToString().Trim();
                        }
                        if (row["EmplLastName"] != DBNull.Value)
                        {
                            visit.EmployeeLastName = row["EmplLastName"].ToString().Trim();
                        }                       
                        if (row["WUName"] != DBNull.Value)
                        {
                            visit.WUName = row["WUName"].ToString().Trim();
                        }
                        if (!ascoTO.ContainsKey(visit.VisitID))
                            ascoTO.Add(visit.VisitID, new List<VisitAsco4TO>());
                        if (row["local"] != DBNull.Value)
                        {
                            asco4TO.NVarcharValue8 = row["local"].ToString().Trim();
                        }
                        if (row["document_name"] != DBNull.Value)
                        {
                            asco4TO.NVarcharValue2 = row["document_name"].ToString().Trim();
                        }
                        if (row["place"] != DBNull.Value)
                        {
                            asco4TO.NVarcharValue3 = row["place"].ToString().Trim();
                        }
                        if (row["state"] != DBNull.Value)
                        {
                            asco4TO.NVarcharValue4 = row["state"].ToString().Trim();
                        }
                        if (row["privacy_statement"] != DBNull.Value)
                        {
                            asco4TO.DatetimeValue1 = DateTime.Parse(row["privacy_statement"].ToString().Trim());
                        }
                        if (row["DatetimeValue2"] != DBNull.Value)
                        {
                            asco4TO.DatetimeValue2 = DateTime.Parse(row["DatetimeValue2"].ToString().Trim());
                        }
                        if (row["ban"] != DBNull.Value)
                        {
                            asco4TO.IntegerValue1 = int.Parse(row["ban"].ToString().Trim());
                        }
                        if (row["num"] != DBNull.Value)
                        {
                            asco4TO.NVarcharValue5 = row["num"].ToString().Trim();
                        }
                        ascoTO[visit.VisitID].Add(asco4TO);
                        visitList.Add(visit);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return visitList;
        }

        public ArrayList getVisitsDateIntervalDistinct(string employeeID, string visitedWorkingUnit, string visitorID, string visitorIdent,
           DateTime dateFrom, DateTime dateTo, string visitDescr, string wUnits, string state, string company, string visitor, string privacy,
           string visitAsco4, Dictionary<int, List<VisitAsco4TO>> ascoTO)
        {
            DataSet dataSet = new DataSet();
            VisitTO visit = new VisitTO();
            ArrayList visitList = new ArrayList();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" SELECT DISTINCT visits.first_name,visits.last_name,visits.visitor_id,visits.visitor_jmbg,visits.company,asco4.integer_value_1 AS ban,asco4.datetime_value_2 AS ban_date ");
                sb.Append(" FROM visits, employees AS e1, working_units AS wu, visits_asco4 AS asco4");
                sb.Append(" WHERE visits.employee_id = e1.employee_id");
                sb.Append(" AND visits.visited_working_unit = wu.working_unit_id");
                sb.Append(" AND visits.visit_id = asco4.visit_id");

                if ((!employeeID.Trim().Equals("")) || (!visitedWorkingUnit.Equals("")) ||
                    (!visitorID.Equals("")) || (!visitorIdent.Trim().Equals("")) ||
                    ((!dateFrom.Equals(new DateTime())) && (!dateTo.Equals(new DateTime()))) ||
                    (!visitDescr.Trim().Equals("")) || (!wUnits.Trim().Equals(""))
                    || (!state.Equals("")) || (!company.Equals("")) || (!visitor.Equals("")) ||
                    (!privacy.Equals("")) || (!visitAsco4.Equals("")))
                {
                    if (!wUnits.Trim().Equals(""))
                    {
                        sb.Append(" AND wu.working_unit_id IN (" + wUnits + ")");
                    }

                    if (!employeeID.Equals(""))
                    {
                        sb.Append(" AND visits.employee_id = " + employeeID);
                    }
                    if (!visitedWorkingUnit.Equals(""))
                    {
                        sb.Append(" AND visits.visited_working_unit = " + visitedWorkingUnit);
                    }
                    if (!visitorID.Equals(""))
                    {
                        sb.Append(" AND UPPER(visits.visitor_id) = N'" + visitorID.Trim().ToUpper() + "'");
                    }
                    if (!visitorIdent.Trim().Equals(""))
                    {
                        sb.Append(" AND UPPER(visitor_jmbg) = N'" + visitorIdent.Trim().ToUpper() + "'");
                        //sb.Append(" OR UPPER(visitor_id) = N'" + visitorIdent.Trim().ToUpper() + "')");
                    }
                    if (!visitDescr.Trim().Equals(""))
                    {
                        sb.Append(" AND UPPER(visits.visit_descr) = '" + visitDescr.Trim().ToUpper() + "'");
                    }
                    if (!visitAsco4.Trim().Equals(""))
                    {
                        sb.Append(" AND UPPER(asco4.nvarchar_value_1) = N'" + visitAsco4.Trim().ToUpper() + "'");
                    }
                    if (!company.Trim().Equals(""))
                    {
                        sb.Append(" AND UPPER(visits.company) = N'" + company.Trim().ToUpper() + "'");
                    }
                    if (!state.Trim().Equals(""))
                    {
                        sb.Append(" AND UPPER(asco4.nvarchar_value_4) = N'" + state.Trim().ToUpper() + "'");
                    }
                    if (!visitor.Trim().Equals(""))
                    {
                        string[] v = new string[2];
                        if (visitor.Contains(" "))
                        {
                            v[0] = visitor.Substring(0, visitor.IndexOf(' ')).Trim();
                            v[1] = visitor.Substring(visitor.IndexOf(' ') + 1).Trim();
                        }
                        else
                        {
                            v[0] = visitor;
                            v[1] = "";
                        }
                        sb.Append(" AND UPPER(visits.first_name) = N'" + v[0].Trim().ToUpper() + "'");
                        if (!v[1].Equals(""))
                            sb.Append(" AND UPPER(visits.last_name) = N'" + v[1].Trim().ToUpper() + "'");
                    }

                    if ((!dateFrom.Equals(new DateTime())) && (!dateTo.Equals(new DateTime())))
                    {
                        sb.Append(" AND (");

                        sb.Append(" (");
                        sb.Append("  date_start < CONVERT(datetime,'" + dateFrom.ToString("yyyy-MM-dd HH:mm:ss") + "', 111) AND ");
                        sb.Append(" date_end >= CONVERT(datetime,'" + dateFrom.ToString("yyyy-MM-dd HH:mm:ss") + "', 111) ");
                        sb.Append(" )");

                        sb.Append(" OR (");
                        sb.Append("  date_start >= CONVERT(datetime,'" + dateFrom.ToString("yyyy-MM-dd HH:mm:ss") + "', 111) AND ");
                        sb.Append(" date_start <= CONVERT(datetime,'" + dateTo.ToString("yyyy-MM-dd HH:mm:ss") + "', 111) ");
                        sb.Append(" )");

                        sb.Append(")");
                    }
                    if (!privacy.Equals(""))
                    {
                        if (privacy.Equals("YES"))
                        {
                            sb.Append(" AND asco4.datetime_value_1 IS NOT NULL ");
                        }
                        if (privacy.Equals("NO"))
                        {
                            sb.Append(" AND asco4.datetime_value_1 IS NULL ");
                        }
                    }
                }
                select = sb.ToString(); //+ " ORDER BY date_start";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visits");
                DataTable table = dataSet.Tables["Visits"];
                VisitAsco4TO asco4TO = new VisitAsco4TO();
                if (table.Rows.Count > 0)
                {
                    int i = 0;
                    foreach (DataRow row in table.Rows)
                    {
                        i++;
                        visit = new VisitTO();
                        asco4TO = new VisitAsco4TO();
                        visit.VisitID = i;
                        if (row["first_name"] != DBNull.Value)
                        {
                            visit.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            visit.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["visitor_id"] != DBNull.Value)
                        {
                            visit.VisitorID = row["visitor_id"].ToString().Trim();
                        }
                        if (row["visitor_jmbg"] != DBNull.Value)
                        {
                            visit.VisitorJMBG = row["visitor_jmbg"].ToString().Trim();
                        }                        
                        if (row["company"] != DBNull.Value)
                        {
                            visit.Company = row["company"].ToString().Trim();
                        } 
                        if (row["ban"] != DBNull.Value)
                        {
                            asco4TO.IntegerValue1 = int.Parse(row["ban"].ToString().Trim());
                        }
                        if (row["ban_date"] != DBNull.Value)
                        {
                            asco4TO.DatetimeValue2 = DateTime.Parse(row["ban_date"].ToString().Trim());
                        }
                        ascoTO.Add(visit.VisitID, new List<VisitAsco4TO>());
                        ascoTO[visit.VisitID].Add(asco4TO);
                        visitList.Add(visit);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return visitList;
        }

        public int getVisitsDateIntervalCount(string employeeID, string visitedWorkingUnit,
            string visitedPerson, string visitorIdent, DateTime dateFrom, DateTime dateTo,
            string visitDescr, string wUnits)
        {
            DataSet dataSet = new DataSet();
            VisitTO visit = new VisitTO();
            ArrayList visitList = new ArrayList();
            string select = "";

            int count = 0;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" SELECT COUNT(*) AS count");
                sb.Append(" FROM visits, employees AS e1, working_units AS wu");
                sb.Append(" WHERE visits.employee_id = e1.employee_id");
                sb.Append(" AND visits.visited_working_unit = wu.working_unit_id");

                if ((!employeeID.Trim().Equals("")) || (!visitedWorkingUnit.Equals("")) ||
                    (!visitedPerson.Equals("")) || (!visitorIdent.Trim().Equals("")) ||
                    ((!dateFrom.Equals(new DateTime())) && (!dateTo.Equals(new DateTime()))) ||
                    (!visitDescr.Trim().Equals("")) || (!wUnits.Trim().Equals("")))
                {
                    if (!wUnits.Trim().Equals(""))
                    {
                        sb.Append(" AND wu.working_unit_id IN (" + wUnits + ")");
                    }

                    if (!employeeID.Equals(""))
                    {
                        sb.Append(" AND visits.employee_id = " + employeeID);
                    }
                    if (!visitedWorkingUnit.Equals(""))
                    {
                        sb.Append(" AND visits.visited_working_unit = " + visitedWorkingUnit);
                    }
                    if (!visitedPerson.Equals(""))
                    {
                        sb.Append(" AND visits.visited_person = " + visitedPerson);
                    }
                    if (!visitorIdent.Trim().Equals(""))
                    {
                        sb.Append(" AND (visitor_jmbg = '" + visitorIdent.Trim() + "'");
                        sb.Append(" OR visitor_id = '" + visitorIdent.Trim() + "')");
                    }
                    if (!visitDescr.Trim().Equals(""))
                    {
                        sb.Append(" AND visit_descr = '" + visitDescr.Trim() + "'");
                    }

                    if ((!dateFrom.Equals(new DateTime())) && (!dateTo.Equals(new DateTime())))
                    {
                        sb.Append(" AND (");

                        sb.Append(" (");
                        sb.Append(" convert(datetime, convert(varchar(10), date_start, 111), 111) < convert(datetime,'" + dateFrom.ToString("yyyy-MM-dd") + "', 111)");
                        sb.Append(" AND (convert(datetime, convert(varchar(10), date_end, 111), 111) >= convert(datetime,'" + dateFrom.ToString("yyyy-MM-dd") + "', 111)");
                        sb.Append(" OR date_end IS NULL)");
                        sb.Append(" )");

                        sb.Append(" OR (");
                        sb.Append(" convert(datetime, convert(varchar(10), date_start, 111), 111) >= convert(datetime,'" + dateFrom.ToString("yyyy-MM-dd") + "', 111)");
                        sb.Append(" AND convert(datetime, convert(varchar(10), date_start, 111), 111) <= convert(datetime,'" + dateTo.ToString("yyyy-MM-dd") + "', 111)");
                        sb.Append(" )");

                        sb.Append(")");
                    }
                }
                select = sb.ToString();

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visits");
                DataTable table = dataSet.Tables["Visits"];

                if (table.Rows.Count > 0)
                {
                    count = Int32.Parse(table.Rows[0]["count"].ToString().Trim());    
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return count;
        }

        public ArrayList getCurrentVisits(string wUnits)
		{
			DataSet dataSet = new DataSet();
			VisitTO visit = new VisitTO();
			ArrayList visitList = new ArrayList();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
                sb.Append("SELECT visits.*, employees.first_name AS emplFirstName, employees.last_name AS emplLastName");
                sb.Append(" FROM visits, employees");
                sb.Append(" WHERE visits.date_end is null");
                sb.Append(" AND employees.employee_id = visits.employee_id");
                if (!wUnits.Trim().Equals(""))
                    sb.Append(" AND employees.working_unit_id IN (" + wUnits + ")");
                sb.Append(" ORDER BY date_start");

				select = sb.ToString();

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "CurrentVisits");
				DataTable table = dataSet.Tables["CurrentVisits"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						visit = new VisitTO();
						visit.VisitID = Int32.Parse(row["visit_id"].ToString().Trim());
						
						if (row["employee_id"] != DBNull.Value)
						{
							visit.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
						}
						if (row["first_name"] != DBNull.Value)
						{
							visit.FirstName = row["first_name"].ToString().Trim();
						}
						if (row["last_name"] != DBNull.Value)
						{
							visit.LastName = row["last_name"].ToString().Trim();
						}
						if (row["visitor_id"] != DBNull.Value)
						{
							visit.VisitorID = row["visitor_id"].ToString().Trim();
						}
						if (row["visitor_jmbg"] != DBNull.Value)
						{
							visit.VisitorJMBG = row["visitor_jmbg"].ToString().Trim();
						}
						if (row["date_start"] != DBNull.Value)
						{
							visit.DateStart = (DateTime) row["date_start"];
						}
						if (row["date_end"] != DBNull.Value)
						{
							visit.DateEnd = (DateTime) row["date_end"];
						}
						if (row["visited_person"] != DBNull.Value)
						{
							visit.VisitedPerson = Int32.Parse(row["visited_person"].ToString().Trim());
						}
						if (row["visited_working_unit"] != DBNull.Value)
						{
							visit.VisitedWorkingUnit = Int32.Parse(row["visited_working_unit"].ToString().Trim());
						}
						if (row["visit_descr"] != DBNull.Value)
						{
							visit.VisitDescr = row["visit_descr"].ToString().Trim();
						}
						if (row["location_id"] != DBNull.Value)
						{
							visit.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
						}
						if (row["remarks"] != DBNull.Value)
						{
							visit.Remarks = row["remarks"].ToString().Trim();
						}
                        if (row["company"] != DBNull.Value)
                        {
                            visit.Company = row["company"].ToString().Trim();
                        }
                        if (row["emplFirstName"] != DBNull.Value)
                        {
                            visit.EmployeeFirstName= row["emplFirstName"].ToString().Trim();
                        }
                        if (row["emplLastName"] != DBNull.Value)
                        {
                            visit.EmployeeLastName = row["emplLastName"].ToString().Trim();
                        }

						visitList.Add(visit);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception("Exception: " + ex.Message);
			}

			return visitList;
		}

        public ArrayList getCurrentVisitsDetail(string wUnits)
        {
            DataSet dataSet = new DataSet();
            VisitTO visit = new VisitTO();
            ArrayList visitList = new ArrayList();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT * FROM");
                sb.Append(" (");
                sb.Append(" (");
                sb.Append(" SELECT visits.*, e1.first_name AS EmplFirstName, e1.last_name AS EmplLastName, e2.first_name AS VisitedFirstName,");
                sb.Append(" e2.last_name AS VisitedLastName, wu.name AS WUName");
                sb.Append(" FROM visits, employees AS e1, employees AS e2, working_units as wu");
                sb.Append(" WHERE visits.date_end IS NULL");
                sb.Append(" AND visits.visited_person = e2.employee_id");
                sb.Append(" AND visits.employee_id = e1.employee_id");
                sb.Append(" AND visits.visited_working_unit = wu.working_unit_id");
                if (!wUnits.Trim().Equals(""))
                    sb.Append(" AND wu.working_unit_id IN (" + wUnits + ")");
                sb.Append(" )");
                sb.Append(" UNION");
                sb.Append(" (");
                sb.Append(" SELECT visits.*, e1.first_name AS EmplFirstName, e1.last_name AS EmplLastName, '' AS VisitedFirstName,");
                sb.Append(" '' AS VisitedLastName, wu.name AS WUName");
                sb.Append(" FROM visits, employees AS e1, employees AS e2, working_units as wu");
                sb.Append(" WHERE visits.date_end IS NULL");
                sb.Append(" AND visits.visited_person IS NULL");
                sb.Append(" AND visits.employee_id = e1.employee_id");
                sb.Append(" AND visits.visited_working_unit = wu.working_unit_id");
                if (!wUnits.Trim().Equals(""))
                    sb.Append(" AND wu.working_unit_id IN (" + wUnits + ")");
                sb.Append(" )");
                sb.Append(" ) AS temp");
                sb.Append(" ORDER BY date_start");

                select = sb.ToString();

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "CurrentVisits");
                DataTable table = dataSet.Tables["CurrentVisits"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        visit = new VisitTO();
                        visit.VisitID = Int32.Parse(row["visit_id"].ToString().Trim());

                        if (row["employee_id"] != DBNull.Value)
                        {
                            visit.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            visit.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            visit.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["visitor_id"] != DBNull.Value)
                        {
                            visit.VisitorID = row["visitor_id"].ToString().Trim();
                        }
                        if (row["visitor_jmbg"] != DBNull.Value)
                        {
                            visit.VisitorJMBG = row["visitor_jmbg"].ToString().Trim();
                        }
                        if (row["date_start"] != DBNull.Value)
                        {
                            visit.DateStart = (DateTime)row["date_start"];
                        }
                        if (row["date_end"] != DBNull.Value)
                        {
                            visit.DateEnd = (DateTime)row["date_end"];
                        }
                        if (row["visited_person"] != DBNull.Value)
                        {
                            visit.VisitedPerson = Int32.Parse(row["visited_person"].ToString().Trim());
                        }
                        if (row["visited_working_unit"] != DBNull.Value)
                        {
                            visit.VisitedWorkingUnit = Int32.Parse(row["visited_working_unit"].ToString().Trim());
                        }
                        if (row["visit_descr"] != DBNull.Value)
                        {
                            visit.VisitDescr = row["visit_descr"].ToString().Trim();
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            visit.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["remarks"] != DBNull.Value)
                        {
                            visit.Remarks = row["remarks"].ToString().Trim();
                        }
                        if (row["company"] != DBNull.Value)
                        {
                            visit.Company = row["company"].ToString().Trim();
                        }
                        if (row["EmplFirstName"] != DBNull.Value)
                        {
                            visit.EmployeeFirstName = row["EmplFirstName"].ToString().Trim();
                        }
                        if (row["EmplLastName"] != DBNull.Value)
                        {
                            visit.EmployeeLastName = row["EmplLastName"].ToString().Trim();
                        }
                        if (row["VisitedFirstName"] != DBNull.Value)
                        {
                            visit.VisitedFirstName = row["VisitedFirstName"].ToString().Trim();
                        }
                        if (row["VisitedLastName"] != DBNull.Value)
                        {
                            visit.VisitedLastName = row["VisitedLastName"].ToString().Trim();
                        }
                        if (row["WUName"] != DBNull.Value)
                        {
                            visit.WUName = row["WUName"].ToString().Trim();
                        }

                        visitList.Add(visit);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return visitList;
        }

        public ArrayList getAllVisits(string wUnits)
        {
            DataSet dataSet = new DataSet();
            VisitTO visit = new VisitTO();
            ArrayList visitList = new ArrayList();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT distinct visitor_jmbg, visitor_id, last_name, first_name FROM visits");
                if (!wUnits.Trim().Equals(""))
                    sb.Append(" WHERE visited_working_unit IN (" + wUnits + ")");
                sb.Append(" ORDER BY last_name, first_name");

                select = sb.ToString();
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visits");
                DataTable table = dataSet.Tables["Visits"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        visit = new VisitTO();
                        if (row["first_name"] != DBNull.Value)
                        {
                            visit.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            visit.LastName = row["last_name"].ToString().Trim();
                        }

                        if (row["visitor_id"] != DBNull.Value)
                        {
                            visit.VisitorID = row["visitor_id"].ToString().Trim();
                        }
                        else if (row["visitor_jmbg"] != DBNull.Value)
                        {
                            visit.VisitorID = row["visitor_jmbg"].ToString().Trim();
                        }

                        visitList.Add(visit);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return visitList;
        }

        public List<string> getDistinctName()
        {
            List<string> names = new List<string>();
            DataSet dataSet = new DataSet();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT first_name,last_name from visits ORDER BY first_name, last_name");
                select = sb.ToString().Trim();
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visit");
                DataTable table = dataSet.Tables["Visit"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        string str = "";
                        if (row["first_name"] != DBNull.Value)
                        {
                            str += row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            str += " "+row["last_name"].ToString().Trim();
                        }
                        names.Add(str);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }
            return names;
        }

        public List<string> getDistinctCompany()
        {
            List<string> companies = new List<string>();
            DataSet dataSet = new DataSet();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT company from visits ORDER BY company");
                select = sb.ToString().Trim();
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visit");
                DataTable table = dataSet.Tables["Visit"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        string str = "";
                        if (row["company"] != DBNull.Value)
                        {
                            str += row["company"].ToString().Trim();
                        }
                      
                        companies.Add(str);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }
            return companies;
        }


		public bool serialize(ArrayList visitTOList, string filePath)
		{
			bool isSerialized = false;

			try
			{
				Stream stream = File.Open(filePath, FileMode.Create);

				VisitTO[] visitTOArray = (VisitTO[]) visitTOList.ToArray(typeof(VisitTO));

				XmlSerializer bformatter = new XmlSerializer(typeof(VisitTO[]));
				bformatter.Serialize(stream, visitTOArray);
				stream.Close();

				isSerialized = true;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			return isSerialized;
		}

		public bool serialize(ArrayList visitTOList)
		{
			bool isSerialized = false;

			try
			{
				//string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLVisitsFile"];
                string filename = Constants.XMLDataSourceDir + Constants.XMLVisitsFile;
				serialize(visitTOList, filename);

				isSerialized = true;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			return isSerialized;
		}

		public ArrayList deserialize(string filePath)
		{
			ArrayList visitListTO = new ArrayList();
			try
			{
				if (File.Exists(filePath))
				{
					Stream stream = File.Open(filePath, FileMode.Open);

					XmlSerializer bformatter = new XmlSerializer(typeof(VisitTO[]));
					VisitTO[] deserialized = (VisitTO[]) bformatter.Deserialize(stream);
					visitListTO = ArrayList.Adapter(deserialized);
					stream.Close();
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}

			return visitListTO;
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

		
	}
}
