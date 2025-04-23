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
	/// Summary description for MSSQLWorkingGroupDAO.
	/// </summary>
	public class MSSQLWorkingGroupDAO : WorkingGroupDAO
	{
		SqlConnection conn = null;
        protected string dateTimeformat = "";
        SqlTransaction _sqlTrans = null;

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

		public MSSQLWorkingGroupDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

        public MSSQLWorkingGroupDAO(SqlConnection sqlConnection)
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

        public int insert(string groupName, string description, DateTime date, int timeSchemaID, int startCycleDay)
		{
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
			int employeeGroupID = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("SET NOCOUNT ON ");
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

				sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");
				sbInsert.Append("SELECT @@Identity AS employee_group_id ");
				sbInsert.Append("SET NOCOUNT OFF ");

				DataSet dataSet = new DataSet();
				SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans );

				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

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

                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");
                cmd.CommandText = sbInsert.ToString();

                int rowsAffected = cmd.ExecuteNonQuery();

				sqlTrans.Commit();
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback("INSERT");
				throw ex;
			}

			return employeeGroupID;
		}

		public int insert(WorkingGroupTO wrkGroupTO)
		{
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
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

		public bool delete(int employeeGroupID)
		{
			bool isDeleted = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

			try
			{
				StringBuilder sbDelete = new StringBuilder();
				sbDelete.Append("DELETE FROM employee_groups WHERE employee_group_id = '" + employeeGroupID.ToString().Trim() + "'");
				
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

        public bool delete(int employeeGroupID, bool doComit)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = null;
            if(doComit)
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");
            else
            sqlTrans = SqlTrans;

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM employee_groups WHERE employee_group_id = '" + employeeGroupID.ToString().Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                isDeleted = true;
                if (doComit)
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doComit)
                sqlTrans.Rollback("DELETE");

                throw new Exception(ex.Message);
            }

            return isDeleted;
        }


        // same as method find(int employeeGroupID, IdbTransaction trans)
        // command is made without transaction
		public WorkingGroupTO find(int employeeGroupID)
		{
			DataSet dataSet = new DataSet();
			WorkingGroupTO wrkGroupTO = new WorkingGroupTO();
			try
			{
				SqlCommand cmd = new SqlCommand( "SELECT * FROM employee_groups WHERE employee_group_id = '" + employeeGroupID.ToString().Trim() + "'", conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

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

        // same as method find(int employeeGroupID)
        // command is made with transaction
        public WorkingGroupTO find(int employeeGroupID, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            WorkingGroupTO wrkGroupTO = new WorkingGroupTO();
            try
            {
                SqlCommand cmd = null;
                if (trans != null)
                {
                    cmd = new SqlCommand("SELECT * FROM employee_groups WHERE employee_group_id = '" + employeeGroupID.ToString().Trim() + "'", conn, (SqlTransaction) trans);
                }
                else
                {
                    cmd = new SqlCommand("SELECT * FROM employee_groups WHERE employee_group_id = '" + employeeGroupID.ToString().Trim() + "'", conn);
                }

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

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
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

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
				sbUpdate.Append("modified_time = GETDATE() ");
				sbUpdate.Append("WHERE employee_group_id = '" + employeeGroupID.ToString().Trim() + "'");
				
				SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
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

            SqlTransaction sqlTrans = null;
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
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE employee_group_id = '" + employeeGroupID.ToString().Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
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

				SqlCommand cmd = new SqlCommand(select, conn);
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

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
				
				select = select + "ORDER BY employee_group_id ";

				SqlCommand cmd = new SqlCommand(select, conn);
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

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
	}
}
