using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using MySql.Data.Types;
using MySql.Data.MySqlClient;
using System.Text;
using System.Configuration;
using System.Globalization;

using TransferObjects;

namespace DataAccess
{
    /// <summary>
    /// Summary description for MySQLEmployeeGroupsTimeScheduleDAO.
    /// </summary>
    public class MySQLEmployeeGroupsTimeScheduleDAO : EmployeeGroupsTimeScheduleDAO
    {
        MySqlConnection conn = null;
		protected string dateTimeformat = "";
        MySqlTransaction _sqlTrans = null;

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MySQLEmployeeGroupsTimeScheduleDAO()
		{
			conn = MySQLDAOFactory.getConnection();	
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MySQLEmployeeGroupsTimeScheduleDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
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
            _sqlTrans = (MySqlTransaction)trans;
        }

        public int insert(int employeeGroupID, DateTime date, int timeSchemaID, int startCycleDay, bool doCommit)
		{
			MySqlTransaction sqlTrans = null;

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
                sbInsert.Append("INSERT INTO employee_groups_time_schedule ");
                sbInsert.Append("(employee_group_id, date, time_schema_id, start_cycle_day, created_by, created_time) ");
				sbInsert.Append("VALUES (");

                if (employeeGroupID == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
                    sbInsert.Append("" + employeeGroupID.ToString().Trim() + ", ");
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
					sbInsert.Append("" + timeSchemaID.ToString().Trim() + ", ");
				}
				if (startCycleDay == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("" + startCycleDay.ToString().Trim() + ", ");
				}

				sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");

				MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);
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
                    sqlTrans.Rollback();
                }
				throw ex;
			}

			return rowsAffected;		
		}

        public bool deleteMonthSchedule(int employeeGroupID, DateTime date, bool doCommit)
        {
            bool isDeleted = false;

            MySqlTransaction sqlTrans = null;
            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }

            try
            {
                DateTime start = new DateTime(date.Year, date.Month, 1);
                DateTime end = new DateTime(date.AddMonths(1).Year, date.AddMonths(1).Month, 1);
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM employee_groups_time_schedule ");
                sbDelete.Append("WHERE employee_group_id = '" + employeeGroupID.ToString().Trim() + "' ");
                sbDelete.Append("AND date >= '" + start.ToString(dateTimeformat).Trim() + "' ");
                sbDelete.Append("AND date < '" + end.ToString(dateTimeformat).Trim() + "'");
				
                MySqlCommand cmd = new MySqlCommand( sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                isDeleted = true;

                if (doCommit)
                {
                    sqlTrans.Commit();
                }
            }
            catch(Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback();
                }
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public bool deleteSchedule(int employeeGroupID, DateTime from, DateTime to, bool doCommit)
        {
            bool isDeleted = false;

            MySqlTransaction sqlTrans = null;
            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM employee_groups_time_schedule ");
                sbDelete.Append("WHERE employee_group_id = '" + employeeGroupID.ToString().Trim() + "' ");
                if (!from.Equals(new DateTime()))
                    sbDelete.Append("AND date >= '" + from.ToString(dateTimeformat).Trim() + "' ");
                if (to != new DateTime(0))
                    sbDelete.Append("AND date < '" + to.AddDays(1).ToString(dateTimeformat).Trim() + "'");

                MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, sqlTrans);
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
                    sqlTrans.Rollback();
                }
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public bool deleteSchedule(int employeeGroupID,  bool doCommit)
        {
            bool isDeleted = false;

            MySqlTransaction sqlTrans = null;
            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM employee_groups_time_schedule ");
                sbDelete.Append("WHERE employee_group_id = '" + employeeGroupID.ToString().Trim() + "' ");

                MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, sqlTrans);
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
                    sqlTrans.Rollback();
                }
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        // Same as method getEmployeeMonthSchedules(int employeeGroupID, DateTime date, IDBTransaction trans) - command is made without transaction
        public List<EmployeeGroupsTimeScheduleTO> getEmployeeMonthSchedules(int employeeGroupID, DateTime date)
        {
            DataSet dataSet = new DataSet();
            EmployeeGroupsTimeScheduleTO emplGroupsTimeScheduleTO = new EmployeeGroupsTimeScheduleTO();
            List<EmployeeGroupsTimeScheduleTO> emplGroupsTimeScheduleList = new List<EmployeeGroupsTimeScheduleTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employee_groups_time_schedule ts");

                if(employeeGroupID != -1 || !date.Equals(new DateTime(0)))
                {
                    sb.Append(" WHERE");
					
                    if (employeeGroupID != -1)
                    {
                        sb.Append(" employee_group_id = '" + employeeGroupID.ToString() + "' AND");
                    }
                    if (!date.Equals(new DateTime(0)))
                    {
                        DateTime start = new DateTime(date.Year, date.Month, 1);
                        DateTime end = new DateTime(date.AddMonths(1).Year, date.AddMonths(1).Month, 1);

                        sb.Append(" (");

                        sb.Append(" (");
                        sb.Append(" date >= '" + start.ToString(dateTimeformat).Trim() + "' AND");
                        sb.Append(" date < '" + end.ToString(dateTimeformat).Trim() + "'");
                        sb.Append(" )");

                        sb.Append(" OR (");
                        sb.Append(" date < '" + start.ToString(dateTimeformat).Trim() + "' ");
                        sb.Append(" AND NOT EXISTS");
                        sb.Append(" (SELECT etsch.employee_group_id");
                        sb.Append(" FROM employee_groups_time_schedule etsch");
                        sb.Append(" WHERE ts.employee_group_id = etsch.employee_group_id");
                        sb.Append(" AND etsch.date < '" + start.ToString(dateTimeformat).Trim() + "'");
                        sb.Append(" AND etsch.date > ts.date)");
                        sb.Append(" )");

                        sb.Append(" )");

                        select = sb.ToString();
                    }
                    else if (employeeGroupID != -1)
                        select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + " ORDER BY employee_group_id, date ";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeGroupTimeSchedule");
                DataTable table = dataSet.Tables["EmployeeGroupTimeSchedule"];

                if (table.Rows.Count > 0)
                {
                    foreach(DataRow row in table.Rows)
                    {
                        emplGroupsTimeScheduleTO = new EmployeeGroupsTimeScheduleTO();
                        emplGroupsTimeScheduleTO.EmployeeGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());

                        if (!row["date"].Equals(DBNull.Value))
                        {
                            emplGroupsTimeScheduleTO.Date = DateTime.Parse(row["date"].ToString());
                        }
                        if (!row.Equals(DBNull.Value))
                        {
                            emplGroupsTimeScheduleTO.TimeSchemaID = Int32.Parse(row["time_schema_id"].ToString().Trim());
                        }
                        if (!row["start_cycle_day"].Equals(DBNull.Value))
                        {
                            emplGroupsTimeScheduleTO.StartCycleDay = Int32.Parse(row["start_cycle_day"].ToString().Trim());
                        }

                        emplGroupsTimeScheduleList.Add(emplGroupsTimeScheduleTO);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return emplGroupsTimeScheduleList;
        }

        public List<EmployeeGroupsTimeScheduleTO> getGroupsSchedules(string groups, DateTime fromDate, DateTime toDate, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            EmployeeGroupsTimeScheduleTO groupScheduleTO = new EmployeeGroupsTimeScheduleTO();
            List<EmployeeGroupsTimeScheduleTO> groupScheduleList = new List<EmployeeGroupsTimeScheduleTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employee_groups_time_schedule ts");

                if (groups.Trim() != "" || !fromDate.Equals(new DateTime(0))
                    || !toDate.Equals(new DateTime(0)))
                {
                    sb.Append(" WHERE");

                    if (groups.Trim() != "")
                    {
                        sb.Append(" employee_group_id IN (" + groups.Trim() + ") AND");
                    }

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
                            sb.Append(" (SELECT etsch.employee_group_id");
                            sb.Append(" FROM employee_groups_time_schedule etsch");
                            sb.Append(" WHERE ts.employee_group_id = etsch.employee_group_id");
                            sb.Append(" AND etsch.date < '" + start.ToString(dateTimeformat).Trim() + "'");
                            sb.Append(" AND etsch.date > ts.date)");
                            sb.Append(" )");
                        }

                        sb.Append(" )");

                        select = sb.ToString();
                    }
                    else if (groups.Trim() != "")
                        select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + " ORDER BY employee_group_id, date ";

                MySqlCommand cmd;
                if (trans != null)
                    cmd = new MySqlCommand(select, conn, (MySqlTransaction)trans);
                else
                    cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "TimeSchedule");
                DataTable table = dataSet.Tables["TimeSchedule"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        groupScheduleTO = new EmployeeGroupsTimeScheduleTO();
                        groupScheduleTO.EmployeeGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());

                        if (!row["date"].Equals(DBNull.Value))
                        {
                            groupScheduleTO.Date = (DateTime)row["date"];
                        }
                        if (!row.Equals(DBNull.Value))
                        {
                            groupScheduleTO.TimeSchemaID = Int32.Parse(row["time_schema_id"].ToString().Trim());
                        }
                        if (!row["start_cycle_day"].Equals(DBNull.Value))
                        {
                            groupScheduleTO.StartCycleDay = Int32.Parse(row["start_cycle_day"].ToString().Trim());
                        }

                        groupScheduleList.Add(groupScheduleTO);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return groupScheduleList;
        }

        // Same as method getEmployeeMonthSchedules(int employeeGroupID, DateTime date) - command is made with transaction
        public List<EmployeeGroupsTimeScheduleTO> getEmployeeMonthSchedules(int employeeGroupID, DateTime date, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            EmployeeGroupsTimeScheduleTO emplGroupsTimeScheduleTO = new EmployeeGroupsTimeScheduleTO();
            List<EmployeeGroupsTimeScheduleTO> emplGroupsTimeScheduleList = new List<EmployeeGroupsTimeScheduleTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employee_groups_time_schedule ts");

                if (employeeGroupID != -1 || !date.Equals(new DateTime(0)))
                {
                    sb.Append(" WHERE");

                    if (employeeGroupID != -1)
                    {
                        sb.Append(" employee_group_id = '" + employeeGroupID.ToString() + "' AND");
                    }
                    if (!date.Equals(new DateTime(0)))
                    {
                        DateTime start = new DateTime(date.Year, date.Month, 1);
                        DateTime end = new DateTime(date.AddMonths(1).Year, date.AddMonths(1).Month, 1);

                        sb.Append(" (");

                        sb.Append(" (");
                        sb.Append(" date >= '" + start.ToString(dateTimeformat).Trim() + "' AND");
                        sb.Append(" date < '" + end.ToString(dateTimeformat).Trim() + "'");
                        sb.Append(" )");

                        sb.Append(" OR (");
                        sb.Append(" date < '" + start.ToString(dateTimeformat).Trim() + "' ");
                        sb.Append(" AND NOT EXISTS");
                        sb.Append(" (SELECT etsch.employee_group_id");
                        sb.Append(" FROM employee_groups_time_schedule etsch");
                        sb.Append(" WHERE ts.employee_group_id = etsch.employee_group_id");
                        sb.Append(" AND etsch.date < '" + start.ToString(dateTimeformat).Trim() + "'");
                        sb.Append(" AND etsch.date > ts.date)");
                        sb.Append(" )");

                        sb.Append(" )");

                        select = sb.ToString();
                    }
                    else if (employeeGroupID != -1)
                        select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + " ORDER BY employee_group_id, date ";

                MySqlCommand cmd = new MySqlCommand(select, conn, (MySqlTransaction) trans);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeGroupTimeSchedule");
                DataTable table = dataSet.Tables["EmployeeGroupTimeSchedule"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        emplGroupsTimeScheduleTO = new EmployeeGroupsTimeScheduleTO();
                        emplGroupsTimeScheduleTO.EmployeeGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());

                        if (!row["date"].Equals(DBNull.Value))
                        {
                            emplGroupsTimeScheduleTO.Date = DateTime.Parse(row["date"].ToString());
                        }
                        if (!row.Equals(DBNull.Value))
                        {
                            emplGroupsTimeScheduleTO.TimeSchemaID = Int32.Parse(row["time_schema_id"].ToString().Trim());
                        }
                        if (!row["start_cycle_day"].Equals(DBNull.Value))
                        {
                            emplGroupsTimeScheduleTO.StartCycleDay = Int32.Parse(row["start_cycle_day"].ToString().Trim());
                        }

                        emplGroupsTimeScheduleList.Add(emplGroupsTimeScheduleTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return emplGroupsTimeScheduleList;
        }

        public List<EmployeeGroupsTimeScheduleTO> getGroupFromSchedules(int employeeGroupID, DateTime date, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            EmployeeGroupsTimeScheduleTO emplGroupsTimeScheduleTO = new EmployeeGroupsTimeScheduleTO();
            List<EmployeeGroupsTimeScheduleTO> emplGroupsTimeScheduleList = new List<EmployeeGroupsTimeScheduleTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employee_groups_time_schedule ts");

                if (employeeGroupID != -1 || !date.Equals(new DateTime(0)))
                {
                    sb.Append(" WHERE");

                    if (employeeGroupID != -1)
                    {
                        sb.Append(" employee_group_id = '" + employeeGroupID.ToString() + "' AND");
                    }
                    if (!date.Equals(new DateTime(0)))
                    {
                        DateTime start = new DateTime(date.Year, date.Month, 1);

                        sb.Append(" (");

                        sb.Append(" (");
                        sb.Append(" date >= '" + start.ToString(dateTimeformat).Trim() + "'");
                        sb.Append(" )");

                        sb.Append(" OR (");
                        sb.Append(" date < '" + start.ToString(dateTimeformat).Trim() + "' ");
                        sb.Append(" AND NOT EXISTS");
                        sb.Append(" (SELECT etsch.employee_group_id");
                        sb.Append(" FROM employee_groups_time_schedule etsch");
                        sb.Append(" WHERE ts.employee_group_id = etsch.employee_group_id");
                        sb.Append(" AND etsch.date < '" + start.ToString(dateTimeformat).Trim() + "'");
                        sb.Append(" AND etsch.date > ts.date)");
                        sb.Append(" )");

                        sb.Append(" )");

                        select = sb.ToString();
                    }
                    else if (employeeGroupID != -1)
                        select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + " ORDER BY employee_group_id, date ";

                MySqlCommand cmd;
                if (trans != null)
                {
                    cmd = new MySqlCommand(select, conn, (MySqlTransaction) trans);
                }
                else
                {
                    cmd = new MySqlCommand(select, conn, (MySqlTransaction) trans);
                }

                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeGroupTimeSchedule");
                DataTable table = dataSet.Tables["EmployeeGroupTimeSchedule"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        emplGroupsTimeScheduleTO = new EmployeeGroupsTimeScheduleTO();
                        emplGroupsTimeScheduleTO.EmployeeGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());

                        if (!row["date"].Equals(DBNull.Value))
                        {
                            emplGroupsTimeScheduleTO.Date = DateTime.Parse(row["date"].ToString());
                        }
                        if (!row.Equals(DBNull.Value))
                        {
                            emplGroupsTimeScheduleTO.TimeSchemaID = Int32.Parse(row["time_schema_id"].ToString().Trim());
                        }
                        if (!row["start_cycle_day"].Equals(DBNull.Value))
                        {
                            emplGroupsTimeScheduleTO.StartCycleDay = Int32.Parse(row["start_cycle_day"].ToString().Trim());
                        }

                        emplGroupsTimeScheduleList.Add(emplGroupsTimeScheduleTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return emplGroupsTimeScheduleList;
        }

        public List<EmployeeGroupsTimeScheduleTO> getGroupsNextSchedule(string employeeGroups, DateTime date)
        {
            DataSet dataSet = new DataSet();
            EmployeeGroupsTimeScheduleTO emplGroupTimeScheduleTO = new EmployeeGroupsTimeScheduleTO();
            List<EmployeeGroupsTimeScheduleTO> emplGroupTimeScheduleList = new List<EmployeeGroupsTimeScheduleTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employee_groups_time_schedule ts");

                if (employeeGroups != "" || !date.Equals(new DateTime(0)))
                {
                    sb.Append(" WHERE");

                    if (employeeGroups != "")
                    {
                        sb.Append(" employee_group_id IN (" + employeeGroups + ") AND");
                    }
                    if (!date.Equals(new DateTime(0)))
                    {
                        sb.Append(" (");
                        sb.Append(" date > '" + date.ToString(dateTimeformat).Trim() + "' ");
                        sb.Append(" AND NOT EXISTS");
                        sb.Append(" (SELECT etsch.employee_group_id");
                        sb.Append(" FROM employee_groups_time_schedule etsch");
                        sb.Append(" WHERE ts.employee_group_id = etsch.employee_group_id");
                        sb.Append(" AND etsch.date > '" + date.ToString(dateTimeformat).Trim() + "'");
                        sb.Append(" AND etsch.date < ts.date)");
                        sb.Append(" )");

                        select = sb.ToString();
                    }
                    else if (employeeGroups != "")
                        select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + " ORDER BY employee_group_id, date ";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeGroupTimeSchedule");
                DataTable table = dataSet.Tables["EmployeeGroupTimeSchedule"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        emplGroupTimeScheduleTO = new EmployeeGroupsTimeScheduleTO();
                        emplGroupTimeScheduleTO.EmployeeGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());

                        if (!row["date"].Equals(DBNull.Value))
                        {
                            emplGroupTimeScheduleTO.Date = DateTime.Parse(row["date"].ToString());
                        }
                        if (!row.Equals(DBNull.Value))
                        {
                            emplGroupTimeScheduleTO.TimeSchemaID = Int32.Parse(row["time_schema_id"].ToString().Trim());
                        }
                        if (!row["start_cycle_day"].Equals(DBNull.Value))
                        {
                            emplGroupTimeScheduleTO.StartCycleDay = Int32.Parse(row["start_cycle_day"].ToString().Trim());
                        }

                        emplGroupTimeScheduleList.Add(emplGroupTimeScheduleTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return emplGroupTimeScheduleList;
        }
        //natalija07112017
        public EmployeeGroupsTimeScheduleTO find(int timeSchemaID, int employeeGroupID, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            EmployeeGroupsTimeScheduleTO employeeGroupsTimeScheduleTO = new EmployeeGroupsTimeScheduleTO();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" SELECT * ");
                sb.Append(" FROM employee_groups_time_schedule ");
                if (timeSchemaID != -1)
                {
                    sb.Append(" WHERE time_schema_id = '" + timeSchemaID.ToString().Trim() + "' ");
                }
                if (employeeGroupID != -1)
                {
                    sb.Append(" WHERE employee_group_id = '" + employeeGroupID.ToString().Trim() + "' ");
                }

                MySqlCommand cmd = null;
                if (trans != null)
                {
                    cmd = new MySqlCommand(sb.ToString(), conn, (MySqlTransaction)trans);
                }
                else
                {
                    cmd = new MySqlCommand(sb.ToString(), conn);
                }

                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeGroupTimeSchedule");
                DataTable table = dataSet.Tables["EmployeeGroupTimeSchedule"];

                if (table.Rows.Count == 1)
                {
                    employeeGroupsTimeScheduleTO = new EmployeeGroupsTimeScheduleTO();
                    employeeGroupsTimeScheduleTO.EmployeeGroupID = Int32.Parse(table.Rows[0]["employee_group_id"].ToString().Trim());

                    if (!table.Rows[0]["time_schema_id"].Equals(DBNull.Value))
                    {
                        employeeGroupsTimeScheduleTO.TimeSchemaID = Int32.Parse(table.Rows[0]["time_schema_id"].ToString().Trim());
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return employeeGroupsTimeScheduleTO;
        }
    }
}
