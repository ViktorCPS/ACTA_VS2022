using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Text;
using System.Configuration;
using System.Globalization;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// Summary description for MySQLWorkingGroupDAO.
	/// </summary>
	public class MySQLWorkingGroupDAO : WorkingGroupDAO
	{
		MySqlConnection conn = null;
        protected string dateTimeformat = "";
        MySqlTransaction _sqlTrans = null;

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }
		
		public MySQLWorkingGroupDAO()
		{
			conn = MySQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MySQLWorkingGroupDAO(MySqlConnection mySqlConnection)
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

        public int insert(string groupName, string description, DateTime date, int timeSchemaID, int startCycleDay)
		{
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
			int employeeGroupID = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO employee_groups ");
				sbInsert.Append("(group_name, description, created_by, created_time) ");
				sbInsert.Append("VALUES (");
				
				if (groupName.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + groupName.Trim() + "', ");
				}
				if (description.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + description.Trim() + "', ");
				}

				sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()); ");
				sbInsert.Append("SELECT LAST_INSERT_ID() AS employee_group_id ");

				DataSet dataSet = new DataSet();
				MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans );

				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "WorkingGroup");

				DataTable table = dataSet.Tables["WorkingGroup"];

				employeeGroupID = Int32.Parse(table.Rows[0]["employee_group_id"].ToString());

                sbInsert.Length = 0;
                sbInsert.Append("INSERT INTO employee_groups_time_schedule ");
                sbInsert.Append("(employee_group_id, date, time_schema_id, start_cycle_day, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("" + employeeGroupID.ToString() + ", ");

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
                    sbInsert.Append("" + timeSchemaID.ToString() + ", ");
                }
                if (startCycleDay == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("" + startCycleDay.ToString() + ", ");
                }

                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");
                cmd.CommandText = sbInsert.ToString();

                int rowsAffected = cmd.ExecuteNonQuery();

				sqlTrans.Commit();
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback();
				throw ex;
			}

			return employeeGroupID;
		}

		public int insert(WorkingGroupTO wrkGroupTO)
		{
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
			int rowsAffected = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO employee_groups ");
				sbInsert.Append("(group_name, description, created_by, created_time) ");
				sbInsert.Append("VALUES (");
				
				if (wrkGroupTO.GroupName.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + wrkGroupTO.GroupName.Trim() + "', ");
				}
				if (wrkGroupTO.Description.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + wrkGroupTO.Description.Trim() + "', ");
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

		public bool delete(int employeeGroupID)
		{
			bool isDeleted = false;
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

			try
			{
				StringBuilder sbDelete = new StringBuilder();
				sbDelete.Append("DELETE FROM employee_groups WHERE employee_group_id = '" + employeeGroupID.ToString().Trim() + "'");
				
				MySqlCommand cmd = new MySqlCommand( sbDelete.ToString(), conn, sqlTrans);
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

        public bool delete(int employeeGroupID, bool doCommit)
        {
            bool isDeleted = false;
            MySqlTransaction sqlTrans = null;
            if (doCommit)
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            else
                sqlTrans = SqlTrans;

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM employee_groups WHERE employee_group_id = '" + employeeGroupID.ToString().Trim() + "'");

                MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                isDeleted = true;
                if (doCommit)
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                sqlTrans.Rollback();

                throw new Exception(ex.Message);
            }

            return isDeleted;
        }


        // Same as method find(int employeeGroupID, IdbTransaction trans)
        // command is made without transaction
		public WorkingGroupTO find(int employeeGroupID)
		{
			DataSet dataSet = new DataSet();
			WorkingGroupTO wrkGroupTO = new WorkingGroupTO();
			try
			{
				MySqlCommand cmd = new MySqlCommand( "SELECT * FROM employee_groups WHERE employee_group_id = '" + employeeGroupID.ToString().Trim() + "'", conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "EmployeeGroups");
				DataTable table = dataSet.Tables["EmployeeGroups"];

				if (table.Rows.Count == 1)
				{
					wrkGroupTO = new WorkingGroupTO();
					wrkGroupTO.EmployeeGroupID = Int32.Parse(table.Rows[0]["employee_group_id"].ToString().Trim());

					if (!table.Rows[0]["group_name"].Equals(DBNull.Value))
					{
						wrkGroupTO.GroupName = table.Rows[0]["group_name"].ToString().Trim();
					}
					if (!table.Rows[0]["description"].Equals(DBNull.Value))
					{
						wrkGroupTO.Description = table.Rows[0]["description"].ToString().Trim();
					}
				}
			}
			catch(Exception ex)
			{				
				throw new Exception(ex.Message);
			}

			return wrkGroupTO;
		}

        // Same as method find(int employeeGroupID)
        // command is made with transaction
        public WorkingGroupTO find(int employeeGroupID, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            WorkingGroupTO wrkGroupTO = new WorkingGroupTO();
            try
            {
                MySqlCommand cmd = null;

                if (trans != null)
                {
                    cmd = new MySqlCommand("SELECT * FROM employee_groups WHERE employee_group_id = '" + employeeGroupID.ToString().Trim() + "'", conn, (MySqlTransaction)trans);
                }
                else
                {
                    cmd = new MySqlCommand("SELECT * FROM employee_groups WHERE employee_group_id = '" + employeeGroupID.ToString().Trim() + "'", conn);
                }

                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeGroups");
                DataTable table = dataSet.Tables["EmployeeGroups"];

                if (table.Rows.Count == 1)
                {
                    wrkGroupTO = new WorkingGroupTO();
                    wrkGroupTO.EmployeeGroupID = Int32.Parse(table.Rows[0]["employee_group_id"].ToString().Trim());

                    if (!table.Rows[0]["group_name"].Equals(DBNull.Value))
                    {
                        wrkGroupTO.GroupName = table.Rows[0]["group_name"].ToString().Trim();
                    }
                    if (!table.Rows[0]["description"].Equals(DBNull.Value))
                    {
                        wrkGroupTO.Description = table.Rows[0]["description"].ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return wrkGroupTO;
        }

		public bool update(int employeeGroupID, string groupName, string description)
		{
			bool isUpdated = false;
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

			try
			{
				StringBuilder sbUpdate = new StringBuilder();
				sbUpdate.Append("UPDATE employee_groups SET ");
				if (!groupName.Trim().Equals(""))
				{
					sbUpdate.Append("group_name = N'" + groupName.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("group_name = null, ");
				}
				if (!description.Trim().Equals(""))
				{
					sbUpdate.Append("description = N'" + description.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("description = null, ");
				}
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = NOW() ");
				sbUpdate.Append("WHERE employee_group_id = '" + employeeGroupID.ToString().Trim() + "'");
				
				MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, sqlTrans);
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
                throw ex;
			}

			return isUpdated;
		}

        public bool update(int employeeGroupID, string groupName, string description, bool doCommit)
        {
            bool isUpdated = false;
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
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE employee_groups SET ");
                if (!groupName.Trim().Equals(""))
                {
                    sbUpdate.Append("group_name = N'" + groupName.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("group_name = null, ");
                }
                if (!description.Trim().Equals(""))
                {
                    sbUpdate.Append("description = N'" + description.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("description = null, ");
                }
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = NOW() ");
                sbUpdate.Append("WHERE employee_group_id = '" + employeeGroupID.ToString().Trim() + "'");

                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

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
                throw ex;
            }

            return isUpdated;
        }

		public List<WorkingGroupTO> getWorkingGroups(WorkingGroupTO wgTO)
		{
			DataSet dataSet = new DataSet();
			WorkingGroupTO wrkGroupTO = new WorkingGroupTO();
			List<WorkingGroupTO> wrkGroupsList = new List<WorkingGroupTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT * FROM employee_groups ");

				if((wgTO.EmployeeGroupID != -1) || (!wgTO.GroupName.Trim().Equals("")) ||
					(!wgTO.Description.Trim().Equals("")))
				{
					sb.Append(" WHERE ");
					
					if (wgTO.EmployeeGroupID != -1)
					{
						sb.Append(" employee_group_id = '" + wgTO.EmployeeGroupID.ToString().Trim() + "' AND");
					}
					if (!wgTO.GroupName.Trim().Equals(""))
					{
						sb.Append(" UPPER(group_name) LIKE N'%" + wgTO.GroupName.ToUpper().Trim() + "%' AND");
					}
					if (!wgTO.Description.Trim().Equals(""))
					{
						sb.Append(" UPPER(description) LIKE N'%" + wgTO.Description.ToUpper().Trim() + "%' AND");
					}
					
					select = sb.ToString(0, sb.ToString().Length - 3);
				}
				else
				{
					select = sb.ToString();
				}
				
				select = select + "ORDER BY group_name ";

				MySqlCommand cmd = new MySqlCommand(select, conn);
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "EmployeeGroups");
				DataTable table = dataSet.Tables["EmployeeGroups"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						wrkGroupTO = new WorkingGroupTO();
						wrkGroupTO.EmployeeGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());
						if (!row["group_name"].Equals(DBNull.Value))
						{
							wrkGroupTO.GroupName = row["group_name"].ToString().Trim();
						}
						if (!row["description"].Equals(DBNull.Value))
						{
							wrkGroupTO.Description = row["description"].ToString().Trim();
						}

						wrkGroupsList.Add(wrkGroupTO);
					}
				}
			}
			catch(Exception ex)
			{
				
				throw new Exception("Exception: " + ex.Message);
			}

			return wrkGroupsList;
		}

        public List<WorkingGroupTO> getWorkingGroupsIDSort(WorkingGroupTO wgTO)
        {
            DataSet dataSet = new DataSet();
            WorkingGroupTO wrkGroupTO = new WorkingGroupTO();
            List<WorkingGroupTO> wrkGroupsList = new List<WorkingGroupTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employee_groups ");

                if ((wgTO.EmployeeGroupID != -1) || (!wgTO.GroupName.Trim().Equals("")) ||
                    (!wgTO.Description.Trim().Equals("")))
                {
                    sb.Append(" WHERE ");

                    if (wgTO.EmployeeGroupID != -1)
                    {
                        sb.Append(" employee_group_id = '" + wgTO.EmployeeGroupID.ToString().Trim() + "' AND");
                    }
                    if (!wgTO.GroupName.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(group_name) LIKE N'%" + wgTO.GroupName.ToUpper().Trim() + "%' AND");
                    }
                    if (!wgTO.Description.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(description) LIKE N'%" + wgTO.Description.ToUpper().Trim() + "%' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY employee_group_id ";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeGroups");
                DataTable table = dataSet.Tables["EmployeeGroups"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        wrkGroupTO = new WorkingGroupTO();
                        wrkGroupTO.EmployeeGroupID = Int32.Parse(row["employee_group_id"].ToString().Trim());
                        if (!row["group_name"].Equals(DBNull.Value))
                        {
                            wrkGroupTO.GroupName = row["group_name"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            wrkGroupTO.Description = row["description"].ToString().Trim();
                        }

                        wrkGroupsList.Add(wrkGroupTO);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Exception: " + ex.Message);
            }

            return wrkGroupsList;
        }
	}
}
