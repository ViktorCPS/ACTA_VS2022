using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using System.Globalization;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// Summary description for MSSQLEmployeeTimeSchedule.
	/// </summary>
	public class MSSQLEmployeeTimeSchedule : EmployeeTimeScheduleDAO
	{

		SqlConnection conn = null;
		protected string dateTimeformat = "";
        SqlTransaction _sqlTrans = null;

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }
		
		public MSSQLEmployeeTimeSchedule()
		{
			conn = MSSQLDAOFactory.getConnection();	
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

        public MSSQLEmployeeTimeSchedule(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
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

		public int insert(int employeeID, DateTime date, int timeSchemaID, int startCycleDay)
		{
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
			int rowsAffected = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO employees_time_schedule ");
				sbInsert.Append("(employee_id, date, time_schema_id, start_cycle_day, created_by, created_time) ");
				sbInsert.Append("VALUES (");
				
				if (employeeID == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + employeeID.ToString().Trim() + "', ");
				}
				if (!date.Equals(new DateTime()))
				{
					sbInsert.Append("'" + date.ToString(dateTimeformat).Trim() + "', ");
				}
				else
				{
					sbInsert.Append("null, ");
				}
				if (timeSchemaID == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + timeSchemaID.ToString().Trim() + "', ");
				}
				if (startCycleDay == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + startCycleDay.ToString().Trim() + "', ");
				}

				sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

				SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
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

        public int insert(int employeeID, DateTime date, int timeSchemaID, int startCycleDay, string user, bool doCommit)
        {
            //SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            SqlTransaction sqlTrans = null;

            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employees_time_schedule ");
                sbInsert.Append("(employee_id, date, time_schema_id, start_cycle_day, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                if (employeeID == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + employeeID.ToString().Trim() + "', ");
                }
                if (!date.Equals(new DateTime()))
                {
                    sbInsert.Append("'" + date.ToString(dateTimeformat).Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("null, ");
                }
                if (timeSchemaID == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + timeSchemaID.ToString().Trim() + "', ");
                }
                if (startCycleDay == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + startCycleDay.ToString().Trim() + "', ");
                }

                if (user.Trim() != "")
                {
                    sbInsert.Append("'" + user.Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                }

                sbInsert.Append("GETDATE()) ");  

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();

                if(doCommit)
                {
                    sqlTrans.Commit();
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback();
                }

                throw ex;
            }

            return rowsAffected;
        }

		public int insert(EmployeeTimeScheduleTO emplTimeScheduleTO)
		{
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
			int rowsAffected = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO employees_time_schedule ");
				sbInsert.Append("(employee_id, date, time_schema_id, start_cycle_day, created_by, created_time) ");
				sbInsert.Append("VALUES (");
				
				if (emplTimeScheduleTO.EmployeeID == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + emplTimeScheduleTO.EmployeeID.ToString().Trim() + "', ");
				}
				if (!emplTimeScheduleTO.Date.Equals(new DateTime()))
				{
					sbInsert.Append("'" + emplTimeScheduleTO.Date.ToString(dateTimeformat).Trim() + "', ");
				}
				else
				{
					sbInsert.Append("null, ");
				}
				if (emplTimeScheduleTO.TimeSchemaID == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + emplTimeScheduleTO.TimeSchemaID.ToString().Trim() + "', ");
				}
				if (emplTimeScheduleTO.StartCycleDay == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + emplTimeScheduleTO.StartCycleDay.ToString().Trim() + "', ");
				}

				sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

				SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
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

        public bool deleteFromToSchedule(int employeeID, DateTime fromDate, DateTime toDate, string modifiedBy, bool doCommit)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = null;

            if (doCommit)            
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");            
            else            
                sqlTrans = this.SqlTrans;            

            try
            {
                StringBuilder sbCondition = new StringBuilder();
                sbCondition.Append(" WHERE employee_id = " + employeeID.ToString().Trim() + " ");
                if (!fromDate.Equals(new DateTime()))
                    sbCondition.Append("AND date >= '" + fromDate.ToString(dateTimeformat).Trim() + "' ");
                if (toDate != new DateTime(0))
                    sbCondition.Append("AND date < '" + toDate.AddDays(1).ToString(dateTimeformat).Trim() + "'");

                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employees_time_schedule_hist (employee_id, date, time_schema_id, start_cycle_day, created_by, created_time, modified_by, modified_time) ");
                sbInsert.Append("SELECT employee_id, date, time_schema_id, start_cycle_day, created_by, created_time, ");
                if (modifiedBy.Trim() != "")
                    sbInsert.Append("N'" + modifiedBy.Trim() + "', ");
                else
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbInsert.Append("GETDATE() FROM employees_time_schedule");
                sbInsert.Append(sbCondition.ToString());

                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM employees_time_schedule ");
                sbDelete.Append(sbCondition.ToString());

                SqlCommand cmdHist = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                cmdHist.ExecuteNonQuery();
                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                isDeleted = true;

                if (doCommit)
                {
                    sqlTrans.Commit();
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback("DELETE");
                }
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        //natalija07112017 dodat statucb
        public bool deleteFromToSchedule(int employeeID, DateTime fromDate, DateTime toDate, string modifiedBy, bool doCommit, bool statuscb)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = null;

            if (doCommit)
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");
            else
                sqlTrans = this.SqlTrans;

            try
            {
                StringBuilder sbCondition = new StringBuilder();
                sbCondition.Append(" WHERE employee_id = " + employeeID.ToString().Trim() + " ");
                if (!fromDate.Equals(new DateTime()))
                    sbCondition.Append("AND date >= '" + fromDate.ToString(dateTimeformat).Trim() + "' ");
                if (statuscb == false)//natalija25102017
                {
                    if (toDate != new DateTime(0))
                        sbCondition.Append("AND date < '" + toDate.AddDays(1).ToString(dateTimeformat).Trim() + "'");
                }

                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employees_time_schedule_hist (employee_id, date, time_schema_id, start_cycle_day, created_by, created_time, modified_by, modified_time) ");
                sbInsert.Append("SELECT employee_id, date, time_schema_id, start_cycle_day, created_by, created_time, ");
                if (modifiedBy.Trim() != "")
                    sbInsert.Append("N'" + modifiedBy.Trim() + "', ");
                else
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbInsert.Append("GETDATE() FROM employees_time_schedule");
                sbInsert.Append(sbCondition.ToString());

                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM employees_time_schedule ");
                sbDelete.Append(sbCondition.ToString());

                SqlCommand cmdHist = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                cmdHist.ExecuteNonQuery();
                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                isDeleted = true;

                if (doCommit)
                {
                    sqlTrans.Commit();
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback("DELETE");
                }
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

		public bool update(int employeeID, DateTime date, int timeSchemaID, int startCycleDay)
		{
            bool isUpdated = false;
            SqlTransaction sqlTrans = null;
            //if (doCommit)
            //{
            //    sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");
            //}
            //else
            //{
                sqlTrans = this.SqlTrans;
           // }

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE employees_time_schedule SET ");

                if (employeeID != -1)
                {
                    sbUpdate.Append("employee_id = " + employeeID + ", ");
                }
                else
                {
                    sbUpdate.Append("employee_id = NULL, ");
                }

                if (!date.Equals(new DateTime()))
                {
                    sbUpdate.Append("date = '" + date.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbUpdate.Append("date = NULL, ");
                }

                if (timeSchemaID != -1)
                {
                    sbUpdate.Append("time_schema_id = " + timeSchemaID + ", ");
                }
                else
                {
                    sbUpdate.Append("time_schema_id = NULL, ");
                }
                if (startCycleDay != -1)
                {
                    sbUpdate.Append("start_cycle_day = " + startCycleDay + ",");
                }
                else
                {
                    sbUpdate.Append("start_cycle_day = NULL, ");
                }


                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE employee_id = '" + employeeID + "'");
                
                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

              
               //     sqlTrans.Commit();
               
            }
            catch (Exception ex)
            {
                
                    sqlTrans.Rollback("UPDATE");
               

                throw new Exception(ex.Message);
            }

            return isUpdated;
		}

        //natalija16112017
		public EmployeeTimeScheduleTO find(int employeeID, DateTime date)
		{
			DataSet dataSet = new DataSet();
			EmployeeTimeScheduleTO emplTimeScheduleTO = new EmployeeTimeScheduleTO();
			try
            {
                //SqlCommand cmd = new SqlCommand("SELECT * FROM employees_time_schedule WHERE employee_id = '" + employeeID.ToString().Trim() + "' AND date = '" + date.ToString(dateTimeformat).Trim() + "' ", conn);
                SqlCommand cmd = new SqlCommand();
                
                if (!date.Equals(DateTime.MinValue))
                {
                    cmd = new SqlCommand("SELECT * FROM employees_time_schedule WHERE employee_id = '" + employeeID.ToString().Trim() + "' AND date = '" + date.ToString(dateTimeformat).Trim() + "' ", conn);
                }
                else
                {
                    StringBuilder sbUpdate = new StringBuilder();
                    sbUpdate.Append("SELECT employee_id,date,time_schema_id,start_cycle_day,created_by,created_time,modified_by,modified_time ");
                    sbUpdate.Append("FROM employees_time_schedule WHERE employee_id = '" + employeeID.ToString().Trim() + "' ");
                    sbUpdate.Append("ORDER BY date desc ");
                    cmd = new SqlCommand(sbUpdate.ToString(), conn);

                }

				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "EmployeesTimeSchedule");
				DataTable table = dataSet.Tables["EmployeesTimeSchedule"];

				if (table.Rows.Count == 1)
				{
					emplTimeScheduleTO = new EmployeeTimeScheduleTO();
					emplTimeScheduleTO.EmployeeID = Int32.Parse(table.Rows[0]["employee_id"].ToString().Trim());

					if (!table.Rows[0]["date"].Equals(DBNull.Value))
					{
						emplTimeScheduleTO.Date = (DateTime) table.Rows[0]["date"];
					}
					if (!table.Rows[0]["time_schema_id"].Equals(DBNull.Value))
					{
						emplTimeScheduleTO.TimeSchemaID = Int32.Parse(table.Rows[0]["time_schema_id"].ToString().Trim());
					}
					if (!table.Rows[0]["start_cycle_day"].Equals(DBNull.Value))
					{
						emplTimeScheduleTO.StartCycleDay = Int32.Parse(table.Rows[0]["start_cycle_day"].ToString().Trim());
					}
				}
			}
			catch(Exception ex)
			{				
				throw new Exception(ex.Message);
			}

			return emplTimeScheduleTO;		
		}

        public List<EmployeeTimeScheduleTO> findEmplSch(int emplID)
        {
            DataSet dataSet = new DataSet();
            EmployeeTimeScheduleTO emplTO = new EmployeeTimeScheduleTO();
            List<EmployeeTimeScheduleTO> lista = new List<EmployeeTimeScheduleTO>();
            try
            {
                string select = "SELECT * FROM actamgr.employees_time_schedule WHERE employee_id="+emplID+" ORDER BY date";
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);
                sqlDA.Fill(dataSet, "Table");
                DataTable table = dataSet.Tables["Table"];
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        emplTO = new EmployeeTimeScheduleTO();
                        emplTO.EmployeeID = emplID;
                        emplTO.Date = DateTime.Parse(row["date"].ToString());
                        emplTO.StartCycleDay = int.Parse(row["start_cycle_day"].ToString());
                        emplTO.TimeSchemaID = int.Parse(row["time_schema_id"].ToString());
                        lista.Add(emplTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return lista;
        }

        public List<EmployeeTimeScheduleTO> getEmployeeTimeSchedules(EmployeeTimeScheduleTO tsTO)
		{
			DataSet dataSet = new DataSet();
			EmployeeTimeScheduleTO emplTimeScheduleTO = new EmployeeTimeScheduleTO();
            List<EmployeeTimeScheduleTO> emplTimeScheduleList = new List<EmployeeTimeScheduleTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT * FROM employees_time_schedule ");

                if ((tsTO.EmployeeID != -1) || (!tsTO.Date.Equals(new DateTime())) ||
                    (tsTO.TimeSchemaID != -1) || (tsTO.StartCycleDay != -1))
				{
					sb.Append(" WHERE ");

                    if (tsTO.EmployeeID != -1)
					{
						//sb.Append(" UPPER(employee_id) LIKE N'" + employeeID.ToUpper().Trim() + "' AND");
                        sb.Append(" employee_id = '" + tsTO.EmployeeID.ToString().Trim() + "' AND");
					}
                    if (!tsTO.Date.Equals(new DateTime()))
					{
                        sb.Append(" date = '" + tsTO.Date.ToString(dateTimeformat).Trim() + "' AND");
					}
                    if (tsTO.TimeSchemaID != -1)
					{
						//sb.Append(" UPPER(time_schema_id) LIKE N'" + timeSchemaID.ToUpper().Trim() + "' AND");
                        sb.Append(" time_schema_id = '" + tsTO.TimeSchemaID.ToString().Trim() + "' AND");
					}
                    if (tsTO.StartCycleDay != -1)
					{
						//sb.Append(" UPPER(start_cycle_day) LIKE N'" + startCycleDay.ToUpper().Trim() + "' AND");
                        sb.Append(" start_cycle_day = '" + tsTO.StartCycleDay.ToString().Trim() + "' AND");
					}
					
					select = sb.ToString(0, sb.ToString().Length - 3);
				}
				else
				{
					select = sb.ToString();
				}
				
				select = select + "ORDER BY employee_id, date ";

				SqlCommand cmd = new SqlCommand(select, conn);
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "EmployeeTimeSchedule");
				DataTable table = dataSet.Tables["EmployeeTimeSchedule"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						emplTimeScheduleTO = new EmployeeTimeScheduleTO();
						emplTimeScheduleTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());

						if (!row["date"].Equals(DBNull.Value))
						{
							emplTimeScheduleTO.Date = (DateTime) row["date"];
						}
						if (!row.Equals(DBNull.Value))
						{
							emplTimeScheduleTO.TimeSchemaID = Int32.Parse(row["time_schema_id"].ToString().Trim());
						}
						if (!row["start_cycle_day"].Equals(DBNull.Value))
						{
							emplTimeScheduleTO.StartCycleDay = Int32.Parse(row["start_cycle_day"].ToString().Trim());
						}

						emplTimeScheduleList.Add(emplTimeScheduleTO);
					}
				}
			}
			catch(Exception ex)
			{
				
				throw new Exception("Exception: " + ex.Message);
			}

			return emplTimeScheduleList;
		}

        public List<EmployeeTimeScheduleTO> getEmployeeMonthSchedules(int employeeID, DateTime date)
		{
			DataSet dataSet = new DataSet();
			EmployeeTimeScheduleTO emplTimeScheduleTO = new EmployeeTimeScheduleTO();
            List<EmployeeTimeScheduleTO> emplTimeScheduleList = new List<EmployeeTimeScheduleTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT * FROM employees_time_schedule ts");

				if(employeeID != -1 || !date.Equals(new DateTime(0)))
				{
					sb.Append(" WHERE");
					
					if (employeeID != -1)
					{
						//sb.Append(" UPPER(employee_id) LIKE N'" + employeeID.ToString().ToUpper().Trim() + "' AND");
                        sb.Append(" employee_id = '" + employeeID.ToString() + "' AND");
					}
					if (!date.Equals(new DateTime(0)))
					{
						DateTime start = new DateTime(date.Year, date.Month, 1);
						DateTime end = new DateTime(date.AddMonths(1).Year, date.AddMonths(1).Month, 1);

                        /* 2008-03-14
                         * From now one, take the last existing time schedule, don't expect that every month has 
                         * time schedule*/

                        /*sb.Append(" date >= '" + start.ToString(dateTimeformat).Trim() + "' AND");
                        sb.Append(" date < '" + end.ToString(dateTimeformat).Trim() + "' AND");*/

                        sb.Append(" (");

                        sb.Append(" (");
                        sb.Append(" date >= '" + start.ToString(dateTimeformat).Trim() + "' AND");
                        sb.Append(" date < '" + end.ToString(dateTimeformat).Trim() + "'");
                        sb.Append(" )");

                        sb.Append(" OR (");
                        sb.Append(" date < '" + start.ToString(dateTimeformat).Trim() + "' ");
                        sb.Append(" AND NOT EXISTS");
                        sb.Append(" (SELECT etsch.employee_id");
                        sb.Append(" FROM employees_time_schedule etsch");
                        sb.Append(" WHERE ts.employee_id = etsch.employee_id");
                        sb.Append(" AND etsch.date < '" + start.ToString(dateTimeformat).Trim() + "'");
                        sb.Append(" AND etsch.date > ts.date)");
                        sb.Append(" )");

                        sb.Append(" )");

                        select = sb.ToString();
					}
                    else if (employeeID != -1)
                        select = sb.ToString(0, sb.ToString().Length - 3);

                    //select = sb.ToString(0, sb.ToString().Length - 3);
				}
				else
				{
					select = sb.ToString();
				}
				
				select = select + " ORDER BY employee_id, date ";

				SqlCommand cmd = new SqlCommand(select, conn);
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "EmployeeTimeSchedule");
				DataTable table = dataSet.Tables["EmployeeTimeSchedule"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						emplTimeScheduleTO = new EmployeeTimeScheduleTO();
						emplTimeScheduleTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());

						if (!row["date"].Equals(DBNull.Value))
						{
							emplTimeScheduleTO.Date = (DateTime) row["date"];
						}
						if (!row.Equals(DBNull.Value))
						{
							emplTimeScheduleTO.TimeSchemaID = Int32.Parse(row["time_schema_id"].ToString().Trim());
						}
						if (!row["start_cycle_day"].Equals(DBNull.Value))
						{
							emplTimeScheduleTO.StartCycleDay = Int32.Parse(row["start_cycle_day"].ToString().Trim());
						}

						emplTimeScheduleList.Add(emplTimeScheduleTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return emplTimeScheduleList;
		}

        // Same as method getEmployeesSchedules(string employees, DateTime fromDate, DateTime toDate, IDBTransaction trans)
        // command is made without transaction
        public Dictionary<int,List<EmployeeTimeScheduleTO>> getEmployeesSchedulesDS(string employees, DateTime fromDate, DateTime toDate)
        {
            DataSet dataSet = new DataSet();
            EmployeeTimeScheduleTO emplTimeScheduleTO = new EmployeeTimeScheduleTO();
            Dictionary<int, List<EmployeeTimeScheduleTO>> emplTimeScheduleList = new Dictionary<int, List<EmployeeTimeScheduleTO>>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employees_time_schedule ts");

                if (employees != "" || !fromDate.Equals(new DateTime(0))
                    || !toDate.Equals(new DateTime(0)))
                {
                    sb.Append(" WHERE");

                    if (employees != "")
                    {
                        sb.Append(" employee_id IN (" + employees + ") AND");
                    }

                    /* 2008-03-14
                     * From now one, take the last existing time schedule, don't expect that every month has 
                     * time schedule*/

                    /*if (!fromDate.Equals(new DateTime(0)))
                    {
                        DateTime start = new DateTime(fromDate.Year, fromDate.Month, 1);
                        sb.Append(" date >= '" + start.ToString(dateTimeformat).Trim() + "' AND");
                    }
                    if (!toDate.Equals(new DateTime(0)))
                    {
                        DateTime end = new DateTime(toDate.AddMonths(1).Year, toDate.AddMonths(1).Month, 1);
                        sb.Append(" date < '" + end.ToString(dateTimeformat).Trim() + "' AND");
                    }
					
                    select = sb.ToString(0, sb.ToString().Length - 3);*/

                    if ((!fromDate.Equals(new DateTime(0))) || (!toDate.Equals(new DateTime(0))))
                    {
                        DateTime start = new DateTime(fromDate.Year, fromDate.Month, 1);
                        DateTime end = new DateTime(toDate.AddMonths(1).Year, toDate.AddMonths(1).Month, 1);

                        sb.Append(" (");

                        sb.Append(" (");
                        bool from = false;
                        if (!fromDate.Equals(new DateTime(0)))
                        {
                            sb.Append(" date >= '" + start.ToString(dateTimeformat).Trim() + "'");
                            from = true;
                        }
                        if (!toDate.Equals(new DateTime(0)))
                        {
                            if (from)
                                sb.Append(" AND date < '" + end.ToString(dateTimeformat).Trim() + "' ");
                            else
                                sb.Append(" date < '" + end.ToString(dateTimeformat).Trim() + "' ");
                        }
                        sb.Append(" )");

                        if (from)
                        {
                            sb.Append(" OR (");
                            sb.Append(" date < '" + start.ToString(dateTimeformat).Trim() + "' ");

                            sb.Append(" AND NOT EXISTS");
                            sb.Append(" (SELECT etsch.employee_id");
                            sb.Append(" FROM employees_time_schedule etsch");
                            sb.Append(" WHERE ts.employee_id = etsch.employee_id");
                            sb.Append(" AND etsch.date < '" + start.ToString(dateTimeformat).Trim() + "'");
                            sb.Append(" AND etsch.date > ts.date)");
                            sb.Append(" )");
                        }

                        sb.Append(" )");

                        select = sb.ToString();
                    }
                    else if (employees != "")
                        select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + " ORDER BY employee_id, date ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeTimeSchedule");
                DataTable table = dataSet.Tables["EmployeeTimeSchedule"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        emplTimeScheduleTO = new EmployeeTimeScheduleTO();
                        emplTimeScheduleTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());

                        if (!row["date"].Equals(DBNull.Value))
                        {
                            emplTimeScheduleTO.Date = (DateTime)row["date"];
                        }
                        if (!row.Equals(DBNull.Value))
                        {
                            emplTimeScheduleTO.TimeSchemaID = Int32.Parse(row["time_schema_id"].ToString().Trim());
                        }
                        if (!row["start_cycle_day"].Equals(DBNull.Value))
                        {
                            emplTimeScheduleTO.StartCycleDay = Int32.Parse(row["start_cycle_day"].ToString().Trim());
                        }
                        if (!emplTimeScheduleList.ContainsKey(emplTimeScheduleTO.EmployeeID))
                            emplTimeScheduleList.Add(emplTimeScheduleTO.EmployeeID, new List<EmployeeTimeScheduleTO>());
                        emplTimeScheduleList[emplTimeScheduleTO.EmployeeID].Add(emplTimeScheduleTO);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return emplTimeScheduleList;
        }

        // Same as method getEmployeesSchedules(string employees, DateTime fromDate, DateTime toDate, IDBTransaction trans)
        // command is made without transaction
        public Dictionary<int, List<EmployeeTimeScheduleTO>> getEmployeesSchedulesDS(string employees, DateTime fromDate, DateTime toDate, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            EmployeeTimeScheduleTO emplTimeScheduleTO = new EmployeeTimeScheduleTO();
            Dictionary<int, List<EmployeeTimeScheduleTO>> emplTimeScheduleList = new Dictionary<int, List<EmployeeTimeScheduleTO>>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employees_time_schedule ts");

                if (employees != "" || !fromDate.Equals(new DateTime(0))
                    || !toDate.Equals(new DateTime(0)))
                {
                    sb.Append(" WHERE");

                    if (employees != "")
                    {
                        sb.Append(" employee_id IN (" + employees + ") AND");
                    }

                    /* 2008-03-14
                     * From now one, take the last existing time schedule, don't expect that every month has 
                     * time schedule*/

                    /*if (!fromDate.Equals(new DateTime(0)))
                    {
                        DateTime start = new DateTime(fromDate.Year, fromDate.Month, 1);
                        sb.Append(" date >= '" + start.ToString(dateTimeformat).Trim() + "' AND");
                    }
                    if (!toDate.Equals(new DateTime(0)))
                    {
                        DateTime end = new DateTime(toDate.AddMonths(1).Year, toDate.AddMonths(1).Month, 1);
                        sb.Append(" date < '" + end.ToString(dateTimeformat).Trim() + "' AND");
                    }
					
                    select = sb.ToString(0, sb.ToString().Length - 3);*/

                    if ((!fromDate.Equals(new DateTime(0))) || (!toDate.Equals(new DateTime(0))))
                    {
                        DateTime start = new DateTime(fromDate.Year, fromDate.Month, 1);
                        DateTime end = new DateTime(toDate.AddMonths(1).Year, toDate.AddMonths(1).Month, 1);

                        sb.Append(" (");

                        sb.Append(" (");
                        bool from = false;
                        if (!fromDate.Equals(new DateTime(0)))
                        {
                            sb.Append(" date >= '" + start.ToString(dateTimeformat).Trim() + "'");
                            from = true;
                        }
                        if (!toDate.Equals(new DateTime(0)))
                        {
                            if (from)
                                sb.Append(" AND date < '" + end.ToString(dateTimeformat).Trim() + "' ");
                            else
                                sb.Append(" date < '" + end.ToString(dateTimeformat).Trim() + "' ");
                        }
                        sb.Append(" )");

                        if (from)
                        {
                            sb.Append(" OR (");
                            sb.Append(" date < '" + start.ToString(dateTimeformat).Trim() + "' ");

                            sb.Append(" AND NOT EXISTS");
                            sb.Append(" (SELECT etsch.employee_id");
                            sb.Append(" FROM employees_time_schedule etsch");
                            sb.Append(" WHERE ts.employee_id = etsch.employee_id");
                            sb.Append(" AND etsch.date < '" + start.ToString(dateTimeformat).Trim() + "'");
                            sb.Append(" AND etsch.date > ts.date)");
                            sb.Append(" )");
                        }

                        sb.Append(" )");

                        select = sb.ToString();
                    }
                    else if (employees != "")
                        select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + " ORDER BY employee_id, date ";

                SqlCommand cmd = new SqlCommand(select, conn,(SqlTransaction)trans);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeTimeSchedule");
                DataTable table = dataSet.Tables["EmployeeTimeSchedule"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        emplTimeScheduleTO = new EmployeeTimeScheduleTO();
                        emplTimeScheduleTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());

                        if (!row["date"].Equals(DBNull.Value))
                        {
                            emplTimeScheduleTO.Date = (DateTime)row["date"];
                        }
                        if (!row.Equals(DBNull.Value))
                        {
                            emplTimeScheduleTO.TimeSchemaID = Int32.Parse(row["time_schema_id"].ToString().Trim());
                        }
                        if (!row["start_cycle_day"].Equals(DBNull.Value))
                        {
                            emplTimeScheduleTO.StartCycleDay = Int32.Parse(row["start_cycle_day"].ToString().Trim());
                        }
                        if (!emplTimeScheduleList.ContainsKey(emplTimeScheduleTO.EmployeeID))
                            emplTimeScheduleList.Add(emplTimeScheduleTO.EmployeeID, new List<EmployeeTimeScheduleTO>());
                        emplTimeScheduleList[emplTimeScheduleTO.EmployeeID].Add(emplTimeScheduleTO);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return emplTimeScheduleList;
        }
                
        public Dictionary<int, List<EmployeeTimeScheduleTO>> getEmployeesSchedulesExactDate(string employees, DateTime fromDate, DateTime toDate, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            EmployeeTimeScheduleTO emplTimeScheduleTO = new EmployeeTimeScheduleTO();
            Dictionary<int, List<EmployeeTimeScheduleTO>> emplTimeScheduleList = new Dictionary<int, List<EmployeeTimeScheduleTO>>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employees_time_schedule ts");

                if (employees != "" || !fromDate.Equals(new DateTime(0))
                    || !toDate.Equals(new DateTime(0)))
                {
                    sb.Append(" WHERE");

                    if (employees != "")
                    {
                        sb.Append(" employee_id IN (" + employees + ") AND");
                    }

                    /* 2008-03-14
                     * From now one, take the last existing time schedule, don't expect that every month has 
                     * time schedule*/

                    /*if (!fromDate.Equals(new DateTime(0)))
                    {
                        DateTime start = new DateTime(fromDate.Year, fromDate.Month, 1);
                        sb.Append(" date >= '" + start.ToString(dateTimeformat).Trim() + "' AND");
                    }
                    if (!toDate.Equals(new DateTime(0)))
                    {
                        DateTime end = new DateTime(toDate.AddMonths(1).Year, toDate.AddMonths(1).Month, 1);
                        sb.Append(" date < '" + end.ToString(dateTimeformat).Trim() + "' AND");
                    }
					
                    select = sb.ToString(0, sb.ToString().Length - 3);*/

                    if ((!fromDate.Equals(new DateTime(0))) || (!toDate.Equals(new DateTime(0))))
                    {
                        DateTime start = fromDate.Date;
                        DateTime end = toDate.Date.AddDays(1);

                        sb.Append(" (");

                        sb.Append(" (");
                        bool from = false;
                        if (!fromDate.Equals(new DateTime(0)))
                        {
                            sb.Append(" date >= '" + start.ToString(dateTimeformat).Trim() + "'");
                            from = true;
                        }
                        if (!toDate.Equals(new DateTime(0)))
                        {
                            if (from)
                                sb.Append(" AND date < '" + end.ToString(dateTimeformat).Trim() + "' ");
                            else
                                sb.Append(" date < '" + end.ToString(dateTimeformat).Trim() + "' ");
                        }
                        sb.Append(" )");

                        if (from)
                        {
                            sb.Append(" OR (");
                            sb.Append(" date < '" + start.ToString(dateTimeformat).Trim() + "' ");

                            sb.Append(" AND NOT EXISTS");
                            sb.Append(" (SELECT etsch.employee_id");
                            sb.Append(" FROM employees_time_schedule etsch");
                            sb.Append(" WHERE ts.employee_id = etsch.employee_id");
                            sb.Append(" AND etsch.date < '" + start.ToString(dateTimeformat).Trim() + "'");
                            sb.Append(" AND etsch.date > ts.date)");
                            sb.Append(" )");
                        }

                        sb.Append(" )");

                        select = sb.ToString();
                    }
                    else if (employees != "")
                        select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + " ORDER BY employee_id, date ";

                SqlCommand cmd;

                if (trans != null)
                    cmd = new SqlCommand(select, conn, (SqlTransaction)trans);
                else
                    cmd = new SqlCommand(select, conn);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeTimeSchedule");
                DataTable table = dataSet.Tables["EmployeeTimeSchedule"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        emplTimeScheduleTO = new EmployeeTimeScheduleTO();
                        emplTimeScheduleTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());

                        if (!row["date"].Equals(DBNull.Value))
                        {
                            emplTimeScheduleTO.Date = (DateTime)row["date"];
                        }
                        if (!row.Equals(DBNull.Value))
                        {
                            emplTimeScheduleTO.TimeSchemaID = Int32.Parse(row["time_schema_id"].ToString().Trim());
                        }
                        if (!row["start_cycle_day"].Equals(DBNull.Value))
                        {
                            emplTimeScheduleTO.StartCycleDay = Int32.Parse(row["start_cycle_day"].ToString().Trim());
                        }
                        if (!emplTimeScheduleList.ContainsKey(emplTimeScheduleTO.EmployeeID))
                            emplTimeScheduleList.Add(emplTimeScheduleTO.EmployeeID, new List<EmployeeTimeScheduleTO>());
                        emplTimeScheduleList[emplTimeScheduleTO.EmployeeID].Add(emplTimeScheduleTO);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return emplTimeScheduleList;
        }

        // Same as method getEmployeesSchedules(string employees, DateTime fromDate, DateTime toDate, IDBTransaction trans)
        // command is made without transaction
        public List<EmployeeTimeScheduleTO> getEmployeesSchedules(string employees, DateTime fromDate, DateTime toDate)
		{
			DataSet dataSet = new DataSet();
			EmployeeTimeScheduleTO emplTimeScheduleTO = new EmployeeTimeScheduleTO();
            List<EmployeeTimeScheduleTO> emplTimeScheduleList = new List<EmployeeTimeScheduleTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT * FROM employees_time_schedule ts");

				if(employees != "" || !fromDate.Equals(new DateTime(0)) 
					|| !toDate.Equals(new DateTime(0)))
				{
					sb.Append(" WHERE");
					
					if (employees != "")
					{
						sb.Append(" employee_id IN (" + employees + ") AND");
					}

                    /* 2008-03-14
                     * From now one, take the last existing time schedule, don't expect that every month has 
                     * time schedule*/

					/*if (!fromDate.Equals(new DateTime(0)))
					{
						DateTime start = new DateTime(fromDate.Year, fromDate.Month, 1);
						sb.Append(" date >= '" + start.ToString(dateTimeformat).Trim() + "' AND");
					}
					if (!toDate.Equals(new DateTime(0)))
					{
						DateTime end = new DateTime(toDate.AddMonths(1).Year, toDate.AddMonths(1).Month, 1);
						sb.Append(" date < '" + end.ToString(dateTimeformat).Trim() + "' AND");
					}
					
					select = sb.ToString(0, sb.ToString().Length - 3);*/

                    if ((!fromDate.Equals(new DateTime(0))) || (!toDate.Equals(new DateTime(0))))
                    {
                        DateTime start = new DateTime(fromDate.Year, fromDate.Month, 1);
                        DateTime end = new DateTime(toDate.AddMonths(1).Year, toDate.AddMonths(1).Month, 1);

                        sb.Append(" (");

                        sb.Append(" (");
                        bool from = false;
                        if (!fromDate.Equals(new DateTime(0)))
                        {
                            sb.Append(" date >= '" + start.ToString(dateTimeformat).Trim() + "'");
                            from = true;
                        }
                        if (!toDate.Equals(new DateTime(0)))
                        {
                            if (from)
                                sb.Append(" AND date < '" + end.ToString(dateTimeformat).Trim() + "' ");
                            else
                                sb.Append(" date < '" + end.ToString(dateTimeformat).Trim() + "' ");
                        }
                        sb.Append(" )");

                        if (from)
                        {
                            sb.Append(" OR (");
                            sb.Append(" date < '" + start.ToString(dateTimeformat).Trim() + "' ");

                            sb.Append(" AND NOT EXISTS"); 
                            sb.Append(" (SELECT etsch.employee_id"); 
                            sb.Append(" FROM employees_time_schedule etsch");
                            sb.Append(" WHERE ts.employee_id = etsch.employee_id");
                            sb.Append(" AND etsch.date < '" + start.ToString(dateTimeformat).Trim() + "'");
                            sb.Append(" AND etsch.date > ts.date)");
                            sb.Append(" )");
                        }

                        sb.Append(" )");

                        select = sb.ToString();
                    }
                    else if (employees != "")
                        select = sb.ToString(0, sb.ToString().Length - 3);
				}
				else
				{
					select = sb.ToString();
				}
				
				select = select + " ORDER BY employee_id, date ";

				SqlCommand cmd = new SqlCommand(select, conn);
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "EmployeeTimeSchedule");
				DataTable table = dataSet.Tables["EmployeeTimeSchedule"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						emplTimeScheduleTO = new EmployeeTimeScheduleTO();
						emplTimeScheduleTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());

						if (!row["date"].Equals(DBNull.Value))
						{
							emplTimeScheduleTO.Date = (DateTime) row["date"];
						}
						if (!row.Equals(DBNull.Value))
						{
							emplTimeScheduleTO.TimeSchemaID = Int32.Parse(row["time_schema_id"].ToString().Trim());
						}
						if (!row["start_cycle_day"].Equals(DBNull.Value))
						{
							emplTimeScheduleTO.StartCycleDay = Int32.Parse(row["start_cycle_day"].ToString().Trim());
						}

						emplTimeScheduleList.Add(emplTimeScheduleTO);
					}
				}
			}
			catch(Exception ex)
			{
				
				throw new Exception(ex.Message);
			}

			return emplTimeScheduleList;
		}

        // Same as method getEmployeesSchedules(string employees, DateTime fromDate, DateTime toDate)
        // command is made with transaction
        public List<EmployeeTimeScheduleTO> getEmployeesSchedules(string employees, DateTime fromDate, DateTime toDate, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            EmployeeTimeScheduleTO emplTimeScheduleTO = new EmployeeTimeScheduleTO();
            List<EmployeeTimeScheduleTO> emplTimeScheduleList = new List<EmployeeTimeScheduleTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employees_time_schedule ts");

                if (employees != "" || !fromDate.Equals(new DateTime(0))
                    || !toDate.Equals(new DateTime(0)))
                {
                    sb.Append(" WHERE");

                    if (employees != "")
                    {
                        sb.Append(" employee_id IN (" + employees + ") AND");
                    }

                    /* 2008-03-14
                     * From now one, take the last existing time schedule, don't expect that every month has 
                     * time schedule*/

                    /*if (!fromDate.Equals(new DateTime(0)))
                    {
                        DateTime start = new DateTime(fromDate.Year, fromDate.Month, 1);
                        sb.Append(" date >= '" + start.ToString(dateTimeformat).Trim() + "' AND");
                    }
                    if (!toDate.Equals(new DateTime(0)))
                    {
                        DateTime end = new DateTime(toDate.AddMonths(1).Year, toDate.AddMonths(1).Month, 1);
                        sb.Append(" date < '" + end.ToString(dateTimeformat).Trim() + "' AND");
                    }
					
                    select = sb.ToString(0, sb.ToString().Length - 3);*/

                    if ((!fromDate.Equals(new DateTime(0))) || (!toDate.Equals(new DateTime(0))))
                    {
                        DateTime start = new DateTime(fromDate.Year, fromDate.Month, 1);
                        DateTime end = new DateTime(toDate.AddMonths(1).Year, toDate.AddMonths(1).Month, 1);

                        sb.Append(" (");

                        sb.Append(" (");
                        bool from = false;
                        if (!fromDate.Equals(new DateTime(0)))
                        {
                            sb.Append(" date >= '" + start.ToString(dateTimeformat).Trim() + "'");
                            from = true;
                        }
                        if (!toDate.Equals(new DateTime(0)))
                        {
                            if (from)
                                sb.Append(" AND date < '" + end.ToString(dateTimeformat).Trim() + "' ");
                            else
                                sb.Append(" date < '" + end.ToString(dateTimeformat).Trim() + "' ");
                        }
                        sb.Append(" )");

                        if (from)
                        {
                            sb.Append(" OR (");
                            sb.Append(" date < '" + start.ToString(dateTimeformat).Trim() + "' ");

                            sb.Append(" AND NOT EXISTS");
                            sb.Append(" (SELECT etsch.employee_id");
                            sb.Append(" FROM employees_time_schedule etsch");
                            sb.Append(" WHERE ts.employee_id = etsch.employee_id");
                            sb.Append(" AND etsch.date < '" + start.ToString(dateTimeformat).Trim() + "'");
                            sb.Append(" AND etsch.date > ts.date)");
                            sb.Append(" )");
                        }

                        sb.Append(" )");

                        select = sb.ToString();
                    }
                    else if (employees != "")
                        select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + " ORDER BY employee_id, date ";

                SqlCommand cmd = new SqlCommand(select, conn, (SqlTransaction) trans);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeTimeSchedule");
                DataTable table = dataSet.Tables["EmployeeTimeSchedule"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        emplTimeScheduleTO = new EmployeeTimeScheduleTO();
                        emplTimeScheduleTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());

                        if (!row["date"].Equals(DBNull.Value))
                        {
                            emplTimeScheduleTO.Date = (DateTime)row["date"];
                        }
                        if (!row.Equals(DBNull.Value))
                        {
                            emplTimeScheduleTO.TimeSchemaID = Int32.Parse(row["time_schema_id"].ToString().Trim());
                        }
                        if (!row["start_cycle_day"].Equals(DBNull.Value))
                        {
                            emplTimeScheduleTO.StartCycleDay = Int32.Parse(row["start_cycle_day"].ToString().Trim());
                        }

                        emplTimeScheduleList.Add(emplTimeScheduleTO);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return emplTimeScheduleList;
        }

        public List<EmployeeTimeScheduleTO> getEmployeeMonthTimeSchedules(DateTime month)
		{
			DataSet dataSet = new DataSet();
			EmployeeTimeScheduleTO emplTimeScheduleTO = new EmployeeTimeScheduleTO();
            List<EmployeeTimeScheduleTO> emplTimeScheduleList = new List<EmployeeTimeScheduleTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT * FROM employees_time_schedule");

				if(!month.Equals(new DateTime(0)))
				{
					sb.Append(" WHERE");
					
					//sb.Append(" CONVERT(datetime, date, 101) >= '" + new DateTime(month.Year, month.Month, 1).ToString("MM/dd/yyy") + "' AND");
                    sb.Append(" convert(datetime, convert(varchar(10), date, 111), 111) >= convert(datetime,'" + new DateTime(month.Year, month.Month, 1).ToString("yyyy-MM-dd") + "', 111) AND");
					//sb.Append(" CONVERT(datetime, date, 101) < '" + new DateTime(month.AddMonths(1).Year, month.AddMonths(1).Month, 1).ToString("MM/dd/yyy") + "'");
                    sb.Append(" convert(datetime, convert(varchar(10), date, 111), 111) < convert(datetime,'" + new DateTime(month.AddMonths(1).Year, month.AddMonths(1).Month, 1).ToString("yyyy-MM-dd") + "', 111)");  
				}

				select = sb.ToString();
								
				select = select + " ORDER BY employee_id, date DESC";

				SqlCommand cmd = new SqlCommand(select, conn);
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "EmployeeTimeSchedule");
				DataTable table = dataSet.Tables["EmployeeTimeSchedule"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						emplTimeScheduleTO = new EmployeeTimeScheduleTO();
						emplTimeScheduleTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());

						if (!row["date"].Equals(DBNull.Value))
						{
							emplTimeScheduleTO.Date = (DateTime) row["date"];
						}
						if (!row.Equals(DBNull.Value))
						{
							emplTimeScheduleTO.TimeSchemaID = Int32.Parse(row["time_schema_id"].ToString().Trim());
						}
						if (!row["start_cycle_day"].Equals(DBNull.Value))
						{
							emplTimeScheduleTO.StartCycleDay = Int32.Parse(row["start_cycle_day"].ToString().Trim());
						}

						emplTimeScheduleList.Add(emplTimeScheduleTO);
					}
				}
			}
			catch(Exception ex)
			{
				
				throw new Exception("Exception: " + ex.Message);
			}

			return emplTimeScheduleList;
		}

		public DataSet getTimeSchedules()
		{
			DataSet dataSet = new DataSet();

			try
			{
				string select = "SELECT * FROM employees_time_schedule ORDER BY employee_id, date";
				SqlCommand cmd = new SqlCommand(select, conn);
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
				sqlDataAdapter.Fill(dataSet, "TimeSchedules");
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return dataSet;
		}

        public List<EmployeeTimeScheduleTO> getEmployeeMonthSchedulesFromDataSet(int employeeID, DateTime date, DataSet dsTimeSchedules)
		{
			EmployeeTimeScheduleTO emplTimeScheduleTO = new EmployeeTimeScheduleTO();
            List<EmployeeTimeScheduleTO> emplTimeScheduleList = new List<EmployeeTimeScheduleTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				if(employeeID != -1 || !date.Equals(new DateTime(0)))
				{
					if (employeeID != -1)
					{
						sb.Append("employee_id = " + employeeID.ToString().Trim() + " AND");
					}
					if (!date.Equals(new DateTime(0)))
					{
						DateTime start = new DateTime(date.Year, date.Month, 1);
						DateTime end = new DateTime(date.AddMonths(1).Year, date.AddMonths(1).Month, 1);
						sb.Append(" date >= '" + start.ToString(dateTimeformat).Trim() + "' AND");
						sb.Append(" date < '" + end.ToString(dateTimeformat).Trim() + "' AND");
					}
					
					select = sb.ToString(0, sb.ToString().Length - 3);
				}
				else
				{
					select = sb.ToString();
				}

				DataTable dtTimeSchedules = dsTimeSchedules.Tables["TimeSchedules"];

				DataRow [] rows = dtTimeSchedules.Select(select);

				if (rows.Length > 0)
				{
					foreach(DataRow row in rows)
					{
						emplTimeScheduleTO = new EmployeeTimeScheduleTO();
						emplTimeScheduleTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());

						if (!row["date"].Equals(DBNull.Value))
						{
							emplTimeScheduleTO.Date = (DateTime) row["date"];
						}
						if (!row.Equals(DBNull.Value))
						{
							emplTimeScheduleTO.TimeSchemaID = Int32.Parse(row["time_schema_id"].ToString().Trim());
						}
						if (!row["start_cycle_day"].Equals(DBNull.Value))
						{
							emplTimeScheduleTO.StartCycleDay = Int32.Parse(row["start_cycle_day"].ToString().Trim());
						}

						emplTimeScheduleList.Add(emplTimeScheduleTO);
					}
				}
			}
			catch(Exception ex)
			{
				
				throw new Exception(ex.Message);
			}

			return emplTimeScheduleList;
		}

        public List<EmployeeTimeScheduleTO> getEmployeesNextSchedule(string employees, DateTime date, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            EmployeeTimeScheduleTO emplTimeScheduleTO = new EmployeeTimeScheduleTO();
            List<EmployeeTimeScheduleTO> emplTimeScheduleList = new List<EmployeeTimeScheduleTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employees_time_schedule ts");

                if (employees != "" || !date.Equals(new DateTime(0)))
                {
                    sb.Append(" WHERE");

                    if (employees != "")
                    {
                        sb.Append(" employee_id IN (" + employees + ") AND");
                    }
                    if (!date.Equals(new DateTime(0)))
                    {
                        sb.Append(" (");
                        sb.Append(" date > '" + date.ToString(dateTimeformat).Trim() + "' ");
                        sb.Append(" AND NOT EXISTS");
                        sb.Append(" (SELECT etsch.employee_id");
                        sb.Append(" FROM employees_time_schedule etsch");
                        sb.Append(" WHERE ts.employee_id = etsch.employee_id");
                        sb.Append(" AND etsch.date > '" + date.ToString(dateTimeformat).Trim() + "'");
                        sb.Append(" AND etsch.date < ts.date)");
                        sb.Append(" )");

                        select = sb.ToString();
                    }
                    else if (employees != "")
                        select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + " ORDER BY employee_id, date ";
                
                SqlCommand cmd;
                
                if (trans != null)
                {
                    cmd = new SqlCommand(select, conn, (SqlTransaction) trans);
                }
                else
                {
                    cmd = new SqlCommand(select, conn);
                }
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeTimeSchedule");
                DataTable table = dataSet.Tables["EmployeeTimeSchedule"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        emplTimeScheduleTO = new EmployeeTimeScheduleTO();
                        emplTimeScheduleTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());

                        if (!row["date"].Equals(DBNull.Value))
                        {
                            emplTimeScheduleTO.Date = (DateTime)row["date"];
                        }
                        if (!row.Equals(DBNull.Value))
                        {
                            emplTimeScheduleTO.TimeSchemaID = Int32.Parse(row["time_schema_id"].ToString().Trim());
                        }
                        if (!row["start_cycle_day"].Equals(DBNull.Value))
                        {
                            emplTimeScheduleTO.StartCycleDay = Int32.Parse(row["start_cycle_day"].ToString().Trim());
                        }

                        emplTimeScheduleList.Add(emplTimeScheduleTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return emplTimeScheduleList;
        }
        public bool update(int employeeID, DateTime date, int timeSchemaID, int startCycleDay, bool naDan)
        {
            bool isUpdated = false;
            SqlTransaction sqlTrans = null;
            //if (doCommit)
            //{
            //    sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");
            //}
            //else
            //{
            sqlTrans = this.SqlTrans;
            // }

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE employees_time_schedule SET ");

                if (employeeID != -1)
                {
                    sbUpdate.Append("employee_id = " + employeeID + ", ");
                }
                else
                {
                    sbUpdate.Append("employee_id = NULL, ");
                }

                if (!date.Equals(new DateTime()))
                {
                    sbUpdate.Append("date = '" + date.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbUpdate.Append("date = NULL, ");
                }

                if (timeSchemaID != -1)
                {
                    sbUpdate.Append("time_schema_id = " + timeSchemaID + ", ");
                }
                else
                {
                    sbUpdate.Append("time_schema_id = NULL, ");
                }
                if (startCycleDay != -1)
                {
                    sbUpdate.Append("start_cycle_day = " + startCycleDay + ",");
                }
                else
                {
                    sbUpdate.Append("start_cycle_day = NULL, ");
                }


                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE employee_id = '" + employeeID + "' and date='" + date.ToString("yyyy-MM-dd") + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }


                //     sqlTrans.Commit();

            }
            catch (Exception ex)
            {

                sqlTrans.Rollback("UPDATE");


                throw new Exception(ex.Message);
            }

            return isUpdated;
        }
	}
}
