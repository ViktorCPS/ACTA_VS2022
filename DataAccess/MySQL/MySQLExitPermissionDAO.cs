using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
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
	/// Summary description for MySQLExitPermissionDAO.
	/// </summary>
	public class MySQLExitPermissionDAO : ExitPermissionDAO
	{
		MySqlConnection conn = null;
		MySqlTransaction _sqlTrans = null;
		protected string dateTimeformat = "";
		public MySqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

		public MySQLExitPermissionDAO()
		{
			conn = MySQLDAOFactory.getConnection();
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MySQLExitPermissionDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public int insert(ExitPermissionTO permTO)
        {
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            int permissionID = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO exit_permissions ");
                sbInsert.Append("(employee_id, pass_type_id, start_time, offset, used, description, verified_by, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                if (permTO.EmployeeID == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + permTO.EmployeeID.ToString().Trim() + "', ");
                }
                if (permTO.PassTypeID == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + permTO.PassTypeID.ToString().Trim() + "', ");
                }
                if (permTO.StartTime.Equals(new DateTime(0)))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + permTO.StartTime.ToString("yyy-MM-dd HH:mm").Trim() + "', ");
                }
                if (permTO.Offset == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + permTO.Offset.ToString().Trim() + "', ");
                }
                if (permTO.Used == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + permTO.Used.ToString().Trim() + "', ");
                }
                if (permTO.Description.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + permTO.Description.Trim() + "', ");
                }
                if (permTO.VerifiedBy.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + permTO.VerifiedBy.Trim() + "', ");
                }
                if (permTO.IssuedBy.Trim().Equals(""))
                {
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("N'" + permTO.IssuedBy.Trim() + "', ");
                }

                sbInsert.Append("NOW());");
                sbInsert.Append("SELECT LAST_INSERT_ID() AS permission_id ");
                DataSet dataSet = new DataSet();
                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);

                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ExitPermissions");

                DataTable table = dataSet.Tables["ExitPermissions"];

                permissionID = Int32.Parse(table.Rows[0]["permission_id"].ToString());

                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();

                throw ex;
            }

            return permissionID;
        }

		public int insert(int employeeID, int passTypeID, DateTime startTime, int offset, int used, string description, string verifiedBy)
		{
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
			int permissionID = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				
				sbInsert.Append("INSERT INTO exit_permissions ");
				sbInsert.Append("(employee_id, pass_type_id, start_time, offset, used, description, verified_by, created_by, created_time) ");
				sbInsert.Append("VALUES (");
				
				if (employeeID == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("'" + employeeID.ToString().Trim() + "', ");
				}
				if (passTypeID == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("'" + passTypeID.ToString().Trim() + "', ");
				}
				if (startTime.Equals(new DateTime(0)))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("'" + startTime.ToString("yyy-MM-dd HH:mm").Trim() + "', ");
				}
				if (offset == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("'" + offset.ToString().Trim() + "', ");
				}
				if (used == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("'" + used.ToString().Trim() + "', ");
				}
				if (description.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + description.Trim() + "', ");
				}
				if (verifiedBy.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + verifiedBy.Trim() + "', ");
				}
				sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbInsert.Append("NOW());");
				sbInsert.Append("SELECT LAST_INSERT_ID() AS permission_id ");
				

				DataSet dataSet = new DataSet();
				MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans );

				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ExitPermissions");

				DataTable table = dataSet.Tables["ExitPermissions"];

				permissionID = Int32.Parse(table.Rows[0]["permission_id"].ToString());

				sqlTrans.Commit();
				
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback();
				
				throw ex;
			}

			return permissionID;
		}

		public int insert(int employeeID, int passTypeID, DateTime startTime, int offset, int used, 
			string description, string verifiedBy, bool doCommit)
		{
			MySqlTransaction sqlTrans = null;
			if(doCommit)
			{
				sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
			}
			else
			{
				sqlTrans = this.SqlTrans;
			}

			int permissionID = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO exit_permissions ");
				sbInsert.Append("(employee_id, pass_type_id, start_time, offset, used, description, verified_by, created_by, created_time) ");
				sbInsert.Append("VALUES (");
				
				if (employeeID == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("'" + employeeID.ToString().Trim() + "', ");
				}
				if (passTypeID == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("'" + passTypeID.ToString().Trim() + "', ");
				}
				if (startTime.Equals(new DateTime(0)))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("'" + startTime.ToString("yyy-MM-dd HH:mm").Trim() + "', ");
				}
				if (offset == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("'" + offset.ToString().Trim() + "', ");
				}
				if (used == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("'" + used.ToString().Trim() + "', ");
				}
				if (description.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + description.Trim() + "', ");
				}
				if (verifiedBy.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + verifiedBy.Trim() + "', ");
				}
				sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbInsert.Append("NOW());");
				sbInsert.Append("SELECT LAST_INSERT_ID() AS permission_id ");
				

				DataSet dataSet = new DataSet();
				MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans );

				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ExitPermissions");

				DataTable table = dataSet.Tables["ExitPermissions"];

				permissionID = Int32.Parse(table.Rows[0]["permission_id"].ToString());

				if(doCommit)
				{
					sqlTrans.Commit();
				}
				
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback();
				
				throw ex;
			}

			return permissionID;
		}

		public bool insertRetroactive(int employeeID, int passTypeID, DateTime startTime, int offset, int used, string description, PassTO passTO, DAOFactory factory, string verifiedBy)
		{
			bool savedRetroactive = false;

			int permissionID = 0;

			MySQLPassDAO passDAO = (MySQLPassDAO) factory.getPassDAO(null);

			this.SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
			passDAO.SqlTrans = this.SqlTrans;
			
			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO exit_permissions ");
				sbInsert.Append("(employee_id, pass_type_id, start_time, offset, used, description, verified_by, created_by, created_time) ");
				sbInsert.Append("VALUES (");
				
				if (employeeID == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("'" + employeeID.ToString().Trim() + "', ");
				}
				if (passTypeID == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("'" + passTypeID.ToString().Trim() + "', ");
				}
				if (startTime.Equals(new DateTime(0)))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("'" + startTime.ToString("yyy-MM-dd HH:mm").Trim() + "', ");
				}
				if (offset == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("'" + offset.ToString().Trim() + "', ");
				}
				if (used == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("'" + used.ToString().Trim() + "', ");
				}
				if (description.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + description.Trim() + "', ");
				}
				if (verifiedBy.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + verifiedBy.Trim() + "', ");
				}
				sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbInsert.Append("NOW());");
				sbInsert.Append("SELECT LAST_INSERT_ID() AS permission_id ");
				

				DataSet dataSet = new DataSet();
				MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, this.SqlTrans );

				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ExitPermissions");

				DataTable table = dataSet.Tables["ExitPermissions"];

				permissionID = Int32.Parse(table.Rows[0]["permission_id"].ToString());

				bool updated = passDAO.update(passTO, false);

				if (permissionID > 0 && updated)
				{
					savedRetroactive = true;
					this.SqlTrans.Commit();
				}
				else
				{
					this.SqlTrans.Rollback();
				}
			}
			catch(Exception ex)
			{
				this.SqlTrans.Rollback();
				
				throw ex;
			}

			return savedRetroactive;
		}

		public bool delete(int permissionID)
		{
			bool isDeleted = false;
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

			try
			{
				StringBuilder sbDelete = new StringBuilder();
				sbDelete.Append("DELETE FROM exit_permissions WHERE permission_id = '" + permissionID.ToString().Trim() + "'");
				
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

		public ExitPermissionTO find(int permissionID)
		{
			DataSet dataSet = new DataSet();
			ExitPermissionTO permissionTO = new ExitPermissionTO();
			try
			{
				MySqlCommand cmd = new MySqlCommand( "SELECT * FROM exit_permissions WHERE permission_id = '" + permissionID.ToString().Trim() + "'", conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ExitPermissions");
				DataTable table = dataSet.Tables["ExitPermissions"];

				if (table.Rows.Count == 1)
				{
					permissionTO = new ExitPermissionTO();
					permissionTO.PermissionID = Int32.Parse(table.Rows[0]["permission_id"].ToString().Trim());

					if (!table.Rows[0]["employee_id"].Equals(DBNull.Value))
					{
						permissionTO.EmployeeID = Int32.Parse(table.Rows[0]["employee_id"].ToString().Trim());
					}
					if (!table.Rows[0]["pass_type_id"].Equals(DBNull.Value))
					{
						permissionTO.PassTypeID = Int32.Parse(table.Rows[0]["pass_type_id"].ToString().Trim());
					}
					if (!table.Rows[0]["start_time"].Equals(DBNull.Value))
					{
						permissionTO.StartTime = DateTime.Parse( table.Rows[0]["start_time"].ToString());
					}
					if (!table.Rows[0]["offset"].Equals(DBNull.Value))
					{
						permissionTO.Offset = Int32.Parse(table.Rows[0]["offset"].ToString().Trim());
					}
					if (!table.Rows[0]["used"].Equals(DBNull.Value))
					{
						permissionTO.Used = Int32.Parse(table.Rows[0]["used"].ToString().Trim());
					}
					if (!table.Rows[0]["description"].Equals(DBNull.Value))
					{
						permissionTO.Description = table.Rows[0]["description"].ToString().Trim();
					}
					if (!table.Rows[0]["created_by"].Equals(DBNull.Value))
					{
						permissionTO.IssuedBy = table.Rows[0]["created_by"].ToString().Trim();
					}
                    if (!table.Rows[0]["verified_by"].Equals(DBNull.Value))
                    {
                        permissionTO.VerifiedBy = table.Rows[0]["verified_by"].ToString().Trim();
                    }
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return permissionTO;
		}

        public bool update(ExitPermissionTO permTO)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE exit_permissions SET ");
                if (permTO.EmployeeID != -1)
                {
                    sbUpdate.Append("employee_id = '" + permTO.EmployeeID.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("employee_id = null, ");
                }
                if (permTO.PassTypeID != -1)
                {
                    sbUpdate.Append("pass_type_id = '" + permTO.PassTypeID.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("employee_id = null, ");
                }
                if (!permTO.StartTime.Equals(new DateTime(0)))
                {
                    sbUpdate.Append("start_time = '" + permTO.StartTime.ToString("yyy-MM-dd HH:mm").Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("start_time = null, ");
                }
                if (permTO.Offset != -1)
                {
                    sbUpdate.Append("offset = '" + permTO.Offset.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("offset = null, ");
                }
                if (permTO.Used != -1)
                {
                    sbUpdate.Append("used = '" + permTO.Used.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("used = null, ");
                }
                if (!permTO.Description.Trim().Equals(""))
                {
                    sbUpdate.Append("description = N'" + permTO.Description.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("description = null, ");
                }
                if (!permTO.VerifiedBy.Trim().Equals(""))
                {
                    sbUpdate.Append("verified_by = N'" + permTO.VerifiedBy.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("verified_by = null, ");
                }
                if (permTO.IssuedBy.Trim().Equals(""))
                {
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("modified_by = N'" + permTO.IssuedBy.Trim() + "', ");
                }
                sbUpdate.Append("modified_time = NOW() ");
                sbUpdate.Append("WHERE permission_id = '" + permTO.PermissionID.ToString().Trim() + "'");

                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

		public bool update(int permissionID, int employeeID, int passTypeID, DateTime startTime, int offset, int used, string description, string verifiedBy)
		{
			bool isUpdated = false;
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

			try
			{
				StringBuilder sbUpdate = new StringBuilder();
				sbUpdate.Append("UPDATE exit_permissions SET ");
				if (employeeID != -1)
				{
					sbUpdate.Append("employee_id = '" + employeeID.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("employee_id = null, ");
				}
				if (passTypeID != -1)
				{
					sbUpdate.Append("pass_type_id = '" + passTypeID.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("employee_id = null, ");
				}
				if (!startTime.Equals(new DateTime(0)))
				{
					sbUpdate.Append("start_time = '" + startTime.ToString("yyy-MM-dd HH:mm").Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("start_time = null, ");
				}
				if (offset != -1)
				{
					sbUpdate.Append("offset = '" + offset.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("offset = null, ");
				}
				if (used != -1)
				{
					sbUpdate.Append("used = '" + used.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("used = null, ");
				}
				if (!description.Trim().Equals(""))
				{
					sbUpdate.Append("description = N'" + description.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("description = null, ");
				}
				if (!verifiedBy.Trim().Equals(""))
				{
					sbUpdate.Append("verified_by = N'" + verifiedBy.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("verified_by = null, ");
				}
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = NOW() ");
				sbUpdate.Append("WHERE permission_id = '" + permissionID.ToString().Trim() + "'");
				
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
				
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public bool update(int permissionID, int employeeID, int passTypeID, DateTime startTime, int offset, int used, string description, bool doCommit)
		{
			bool isUpdated = false;
			MySqlTransaction sqlTrans = null;
			if(doCommit)
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
				sbUpdate.Append("UPDATE exit_permissions SET ");
				if (employeeID != -1)
				{
					sbUpdate.Append("employee_id = '" + employeeID.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("employee_id = null, ");
				}
				if (passTypeID != -1)
				{
					sbUpdate.Append("pass_type_id = '" + passTypeID.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("employee_id = null, ");
				}
				if (!startTime.Equals(new DateTime(0)))
				{
					sbUpdate.Append("start_time = '" + startTime.ToString("yyy-MM-dd HH:mm").Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("start_time = null, ");
				}
				if (offset != -1)
				{
					sbUpdate.Append("offset = '" + offset.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("offset = null, ");
				}
				if (used != -1)
				{
					sbUpdate.Append("used = '" + used.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("used = null, ");
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
				sbUpdate.Append("WHERE permission_id = '" + permissionID.ToString().Trim() + "'");
				
				MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, sqlTrans);
				int res = cmd.ExecuteNonQuery();
				if(res > 0)
				{
					isUpdated = true;	
				}
				
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

			return isUpdated;
		}

		public bool updateRetroactive(int permissionID, int employeeID, int passTypeID, DateTime startTime, int offset, int used, string description, PassTO passTO, DAOFactory factory, string verifiedBy)
		{
			bool isUpdatedRetroactive = false;
			bool isUpdated = false;
			
			MySQLPassDAO passDAO = (MySQLPassDAO) factory.getPassDAO(conn);

			this.SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
			passDAO.SqlTrans = this.SqlTrans;
			

			try
			{
				StringBuilder sbUpdate = new StringBuilder();
				sbUpdate.Append("UPDATE exit_permissions SET ");
				if (employeeID != -1)
				{
					sbUpdate.Append("employee_id = '" + employeeID.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("employee_id = null, ");
				}
				if (passTypeID != -1)
				{
					sbUpdate.Append("pass_type_id = '" + passTypeID.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("employee_id = null, ");
				}
				if (!startTime.Equals(new DateTime(0)))
				{
					sbUpdate.Append("start_time = '" + startTime.ToString("yyy-MM-dd HH:mm").Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("start_time = null, ");
				}
				if (offset != -1)
				{
					sbUpdate.Append("offset = '" + offset.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("offset = null, ");
				}
				if (used != -1)
				{
					sbUpdate.Append("used = '" + used.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("used = null, ");
				}
				if (!description.Trim().Equals(""))
				{
					sbUpdate.Append("description = N'" + description.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("description = null, ");
				}
				if (!verifiedBy.Trim().Equals(""))
				{
					sbUpdate.Append("verified_by = N'" + verifiedBy.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("verified_by = null, ");
				}
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = NOW() ");
				sbUpdate.Append("WHERE permission_id = '" + permissionID.ToString().Trim() + "'");
				
				MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, this.SqlTrans);
				int res = cmd.ExecuteNonQuery();
				if(res > 0)
				{
					isUpdated = true;	
				}
				
				bool updated = passDAO.update(passTO, false);

				if (isUpdated && updated)
				{
					isUpdatedRetroactive = true;
					this.SqlTrans.Commit();
				}
				else
				{
					this.SqlTrans.Rollback();
				}
			}
			catch(Exception ex)
			{
				this.SqlTrans.Rollback();
				
				throw new Exception(ex.Message);
			}

			return isUpdatedRetroactive;
		}

        public List<ExitPermissionTO> getExitPermissions(ExitPermissionTO permTO, DateTime fromTime, DateTime toTime, string wUnits)
		{
			DataSet dataSet = new DataSet();
			ExitPermissionTO exitPermissionTO = new ExitPermissionTO();
            List<ExitPermissionTO> exitPermissionsList = new List<ExitPermissionTO>();
			CultureInfo ci = CultureInfo.InvariantCulture;
			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT ep.*, empl.first_name empl_first_name, empl.last_name empl_last_name, pt.description pt_description, auser.name user_name ");
				sb.Append("FROM exit_permissions ep, employees empl, pass_types pt, appl_users auser");
				sb.Append(" WHERE ");

                if ((permTO.PermissionID != -1) || (permTO.EmployeeID != -1) ||
                    (permTO.PassTypeID != -1) || (!fromTime.Equals(new DateTime(0))) ||
                    (!toTime.Equals(new DateTime(0))) || (permTO.Offset != -1) ||
                    (permTO.Used != -1) || (!permTO.Description.Trim().Equals("")) ||
                    (!permTO.IssuedBy.Trim().Equals("")) || (!permTO.IssuedTime.Equals(new DateTime())))
				{
					if (permTO.PermissionID != -1)
					{
						//sb.Append(" UPPER(ep.permission_id) LIKE '" + permissionID.ToUpper().Trim() + "' AND");
                        sb.Append(" ep.permission_id = '" + permTO.PermissionID.ToString().Trim() + "' AND");
					}
					if (permTO.EmployeeID != -1)
					{
						//sb.Append(" UPPER(ep.employee_id) LIKE '" + employeeID.ToUpper().Trim() + "' AND");
                        sb.Append(" ep.employee_id = '" + permTO.EmployeeID.ToString().Trim() + "' AND");
					}
					if (permTO.PassTypeID != -1)
					{
						//sb.Append(" UPPER(ep.pass_type_id) LIKE '" + passTypeID.ToUpper().Trim() + "' AND");
                        sb.Append(" ep.pass_type_id = '" + permTO.PassTypeID.ToString().Trim() + "' AND");
					}
                    if (!fromTime.Equals(new DateTime(0)))
					{
                        sb.Append(" UPPER(DATE_FORMAT(ep.start_time,'%Y-%m-%dT%h:%m:%s')) >= '" + fromTime.ToString(dateTimeformat, ci).Trim() + "' AND");
					}
                    if (!toTime.Equals(new DateTime(0)))
					{
                        sb.Append(" UPPER(DATE_FORMAT(ep.start_time,'%Y-%m-%dT%h:%m:%s')) <= '" + toTime.ToString(dateTimeformat, ci).Trim() + "' AND");
					}
					if (permTO.Offset != -1)
					{
						//sb.Append(" UPPER(ep.offset) LIKE '" + offset.ToUpper().Trim() + "' AND");
                        sb.Append(" ep.offset = '" + permTO.Offset.ToString().Trim() + "' AND");
					}
					if (permTO.Used != -1)
					{
						//sb.Append(" UPPER(ep.used) LIKE '" + used.ToUpper().Trim() + "' AND");
                        sb.Append(" ep.used = '" + permTO.Used.ToString().Trim() + "' AND");
					}
					if (!permTO.Description.Trim().Equals(""))
					{
						sb.Append(" UPPER(ep.description) LIKE N'" + permTO.Description.ToUpper().Trim() + "' AND");
					}
					if (!permTO.IssuedBy.Trim().Equals(""))
					{
						sb.Append(" UPPER(ep.created_by) LIKE N'" + permTO.IssuedBy.ToUpper().Trim() + "' AND");
					}
					if (!permTO.IssuedTime.Equals(new DateTime()))
					{
						sb.Append(" UPPER(DATE_FORMAT(ep.created_time,GET_FORMAT(DATETIME,'ISO'))) = '" + permTO.IssuedTime.ToString(dateTimeformat, ci) + "' AND");
					}
				}

				sb.Append(" ep.employee_id = empl.employee_id AND ep.pass_type_id = pt.pass_type_id");
				sb.Append(" AND ep.created_by = auser.user_id");
				if (!wUnits.Equals(""))
				{
					sb.Append(" AND empl.working_unit_id IN (" + wUnits + ")");
				}

				MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ExitPermissions");
				DataTable table = dataSet.Tables["ExitPermissions"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						exitPermissionTO = new ExitPermissionTO();
						exitPermissionTO.PermissionID = Int32.Parse(row["permission_id"].ToString().Trim());

						if (!row["employee_id"].Equals(DBNull.Value))
						{
							exitPermissionTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
						}
						if (!row["pass_type_id"].Equals(DBNull.Value))
						{
							exitPermissionTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
						}
						if (!row["start_time"].Equals(DBNull.Value))
						{
							exitPermissionTO.StartTime = DateTime.Parse( row["start_time"].ToString());
						}
						if (!row["offset"].Equals(DBNull.Value))
						{
							exitPermissionTO.Offset = Int32.Parse(row["offset"].ToString().Trim());
						}
						if (!row["used"].Equals(DBNull.Value))
						{
							exitPermissionTO.Used = Int32.Parse(row["used"].ToString().Trim());
						}
						if (!row["description"].Equals(DBNull.Value))
						{
							exitPermissionTO.Description = row["description"].ToString().Trim();
						}
						if (!row["created_by"].Equals(DBNull.Value))
						{
							exitPermissionTO.IssuedBy = row["created_by"].ToString().Trim();
						}
						if (!row["created_time"].Equals(DBNull.Value))
						{
							exitPermissionTO.IssuedTime = DateTime.Parse( row["created_time"].ToString());
						}
						if (!row["empl_last_name"].Equals(DBNull.Value))
						{
							exitPermissionTO.EmployeeName = row["empl_last_name"].ToString().Trim();
						}
						if (!row["empl_first_name"].Equals(DBNull.Value))
						{
							exitPermissionTO.EmployeeName += " " + row["empl_first_name"].ToString().Trim();
						}
						if (!row["pt_description"].Equals(DBNull.Value))
						{
							exitPermissionTO.PassTypeDesc = row["pt_description"].ToString().Trim();
						}
						if (!row["user_name"].Equals(DBNull.Value))
						{
							exitPermissionTO.UserName = row["user_name"].ToString().Trim();
						}
						exitPermissionsList.Add(exitPermissionTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return exitPermissionsList;
		}

        public List<ExitPermissionTO> getExitPermissionsVerifiedBy(ExitPermissionTO permTO, DateTime fromTime, DateTime toTime, string wUnits)
		{
			DataSet dataSet = new DataSet();
			ExitPermissionTO exitPermissionTO = new ExitPermissionTO();
            List<ExitPermissionTO> exitPermissionsList = new List<ExitPermissionTO>();
			CultureInfo ci = CultureInfo.InvariantCulture;
			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT ep.*, empl.first_name empl_first_name, empl.last_name empl_last_name, pt.description pt_description, auser.name user_name, wu.name wu_name ");
				sb.Append("FROM exit_permissions ep, employees empl, pass_types pt, appl_users auser, working_units wu");
				sb.Append(" WHERE ");

                if ((permTO.PermissionID != -1) || (permTO.EmployeeID != -1) ||
                    (permTO.PassTypeID != -1) || (!fromTime.Equals(new DateTime(0))) ||
                    (!toTime.Equals(new DateTime(0))) || (permTO.Offset != -1) ||
                    (permTO.Used != -1) || (!permTO.Description.Trim().Equals("")) ||
                    (!permTO.IssuedBy.Trim().Equals("")) || (!permTO.IssuedTime.Equals(new DateTime())))
				{
					if (permTO.PermissionID != -1)
					{
						//sb.Append(" UPPER(ep.permission_id) LIKE '" + permissionID.ToUpper().Trim() + "' AND");
                        sb.Append(" ep.permission_id = '" + permTO.PermissionID.ToString().Trim() + "' AND");
					}
					if (permTO.EmployeeID != -1)
					{
						//sb.Append(" UPPER(ep.employee_id) LIKE '" + employeeID.ToUpper().Trim() + "' AND");
                        sb.Append(" ep.employee_id = '" + permTO.EmployeeID.ToString().Trim() + "' AND");
					}
					if (permTO.PassTypeID != -1)
					{
						//sb.Append(" UPPER(ep.pass_type_id) LIKE '" + passTypeID.ToUpper().Trim() + "' AND");
                        sb.Append(" ep.pass_type_id = '" + permTO.PassTypeID.ToString().Trim() + "' AND");
					}
                    if (!fromTime.Equals(new DateTime(0)))
					{
                        sb.Append(" UPPER(DATE_FORMAT(ep.start_time,'%Y-%m-%dT%h:%m:%s')) >= '" + fromTime.ToString(dateTimeformat, ci).Trim() + "' AND");
					}
                    if (!toTime.Equals(new DateTime(0)))
					{
						//sb.Append(" UPPER(DATE_FORMAT(ep.start_time,'%Y-%m-%dT%h:%m:%s')) <= '" + Convert.ToDateTime(toTime.Trim(),ci).AddDays(1).ToString(dateTimeformat) + "' AND");
						//now, one day is addedd on UI, ExitPermissions.cs
                        sb.Append(" UPPER(DATE_FORMAT(ep.start_time,'%Y-%m-%dT%h:%m:%s')) <= '" + toTime.ToString(dateTimeformat, ci).Trim() + "' AND");
					}
					if (permTO.Offset != -1)
					{
						//sb.Append(" UPPER(ep.offset) LIKE '" + offset.ToUpper().Trim() + "' AND");
                        sb.Append(" ep.offset = '" + permTO.Offset.ToString().Trim() + "' AND");
					}
					if (permTO.Used != -1)
					{
						//sb.Append(" UPPER(ep.used) LIKE '" + used.ToUpper().Trim() + "' AND");
                        sb.Append(" ep.used = '" + permTO.Used.ToString().Trim() + "' AND");
					}
					if (!permTO.Description.Trim().Equals(""))
					{
						sb.Append(" UPPER(ep.description) LIKE N'" + permTO.Description.ToUpper().Trim() + "' AND");
					}
					if (!permTO.IssuedBy.Trim().Equals(""))
					{
						sb.Append(" UPPER(ep.created_by) LIKE N'" + permTO.IssuedBy.ToUpper().Trim() + "' AND");
					}
					if (!permTO.IssuedTime.Equals(new DateTime()))
					{
						sb.Append(" UPPER(DATE_FORMAT(ep.created_time,GET_FORMAT(DATETIME,'ISO'))) = '" + permTO.IssuedTime.ToString(dateTimeformat, ci) + "' AND");
					}
				}

				sb.Append(" ep.employee_id = empl.employee_id AND ep.pass_type_id = pt.pass_type_id");
				sb.Append(" AND ep.created_by = auser.user_id");
				sb.Append(" AND empl.working_unit_id = wu.working_unit_id");
				if (!wUnits.Equals(""))
				{
					sb.Append(" AND empl.working_unit_id IN (" + wUnits + ")");
				}

				MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ExitPermissions");
				DataTable table = dataSet.Tables["ExitPermissions"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						exitPermissionTO = new ExitPermissionTO();
						exitPermissionTO.PermissionID = Int32.Parse(row["permission_id"].ToString().Trim());

						if (!row["employee_id"].Equals(DBNull.Value))
						{
							exitPermissionTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
						}
						if (!row["pass_type_id"].Equals(DBNull.Value))
						{
							exitPermissionTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
						}
						if (!row["start_time"].Equals(DBNull.Value))
						{
							exitPermissionTO.StartTime = DateTime.Parse( row["start_time"].ToString());
						}
						if (!row["offset"].Equals(DBNull.Value))
						{
							exitPermissionTO.Offset = Int32.Parse(row["offset"].ToString().Trim());
						}
						if (!row["used"].Equals(DBNull.Value))
						{
							exitPermissionTO.Used = Int32.Parse(row["used"].ToString().Trim());
						}
						if (!row["description"].Equals(DBNull.Value))
						{
							exitPermissionTO.Description = row["description"].ToString().Trim();
						}
						if (!row["created_by"].Equals(DBNull.Value))
						{
							exitPermissionTO.IssuedBy = row["created_by"].ToString().Trim();
						}
						if (!row["created_time"].Equals(DBNull.Value))
						{
							exitPermissionTO.IssuedTime = DateTime.Parse( row["created_time"].ToString());
						}
						if (!row["empl_last_name"].Equals(DBNull.Value))
						{
							exitPermissionTO.EmployeeName = row["empl_last_name"].ToString().Trim();
						}
						if (!row["empl_first_name"].Equals(DBNull.Value))
						{
							exitPermissionTO.EmployeeName += " " + row["empl_first_name"].ToString().Trim();
						}
						if (!row["pt_description"].Equals(DBNull.Value))
						{
							exitPermissionTO.PassTypeDesc = row["pt_description"].ToString().Trim();
						}
						if (!row["user_name"].Equals(DBNull.Value))
						{
							exitPermissionTO.UserName = row["user_name"].ToString().Trim();
						}
						if (!row["verified_by"].Equals(DBNull.Value))
						{
							exitPermissionTO.VerifiedBy = row["verified_by"].ToString().Trim();
						}
						if (row["wu_name"] != DBNull.Value)
						{
							exitPermissionTO.WorkingUnitName = row["wu_name"].ToString().Trim();
						}

						exitPermissionsList.Add(exitPermissionTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return exitPermissionsList;
		}

        public List<ExitPermissionTO> getValidExitPermissions(string employeeID)
		{
			DataSet dataSet = new DataSet();
			ExitPermissionTO exitPermissionTO = new ExitPermissionTO();
            List<ExitPermissionTO> exitPermissionsList = new List<ExitPermissionTO>();

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT ep.*, empl.first_name empl_first_name, empl.last_name empl_last_name, pt.description pt_description, auser.name user_name ");
				sb.Append("FROM exit_permissions ep, employees empl, pass_types pt, appl_users auser");
				sb.Append(" WHERE ");
				//sb.Append(" UPPER(ep.employee_id) LIKE '" + employeeID.ToUpper().Trim() + "' AND");
                sb.Append(" ep.employee_id = '" + employeeID.ToUpper().Trim() + "' AND");
				sb.Append(" NOW() >= start_time AND NOW() <= DATE_ADD(start_time,interval offset minute) AND");
				sb.Append(" ep.employee_id = empl.employee_id AND ep.pass_type_id = pt.pass_type_id AND ep.created_by = auser.user_id");
				MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ExitPermissions");
				DataTable table = dataSet.Tables["ExitPermissions"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						exitPermissionTO = new ExitPermissionTO();
						exitPermissionTO.PermissionID = Int32.Parse(row["permission_id"].ToString().Trim());

						if (!row["employee_id"].Equals(DBNull.Value))
						{
							exitPermissionTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
						}
						if (!row["pass_type_id"].Equals(DBNull.Value))
						{
							exitPermissionTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
						}
						if (!row["start_time"].Equals(DBNull.Value))
						{
							exitPermissionTO.StartTime = DateTime.Parse( row["start_time"].ToString());
						}
						if (!row["offset"].Equals(DBNull.Value))
						{
							exitPermissionTO.Offset = Int32.Parse(row["offset"].ToString().Trim());
						}
						if (!row["used"].Equals(DBNull.Value))
						{
							exitPermissionTO.Used = Int32.Parse(row["used"].ToString().Trim());
						}
						if (!row["description"].Equals(DBNull.Value))
						{
							exitPermissionTO.Description = row["description"].ToString().Trim();
						}
						if (!row["created_by"].Equals(DBNull.Value))
						{
							exitPermissionTO.IssuedBy = row["created_by"].ToString().Trim();
						}
						if (!row["created_time"].Equals(DBNull.Value))
						{
							exitPermissionTO.IssuedTime = DateTime.Parse( row["created_time"].ToString());
						}
						if (!row["empl_last_name"].Equals(DBNull.Value))
						{
							exitPermissionTO.EmployeeName = row["empl_last_name"].ToString().Trim();
						}
						if (!row["empl_first_name"].Equals(DBNull.Value))
						{
							exitPermissionTO.EmployeeName += " " + row["empl_first_name"].ToString().Trim();
						}
						if (!row["pt_description"].Equals(DBNull.Value))
						{
							exitPermissionTO.PassTypeDesc = row["pt_description"].ToString().Trim();
						}
						if (!row["user_name"].Equals(DBNull.Value))
						{
							exitPermissionTO.UserName = row["user_name"].ToString().Trim();
						}
						exitPermissionsList.Add(exitPermissionTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return exitPermissionsList;
		}

		// TODO!!!
        public void serialize(List<ExitPermissionTO> ExitPermissionsTOList)
		{
			try
			{
				string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLExitPermissiionFile"];
				Stream stream = File.Open(filename, FileMode.Create);

				ExitPermissionTO[] exitPermissionTOArray = (ExitPermissionTO[]) ExitPermissionsTOList.ToArray();

				XmlSerializer bformatter = new XmlSerializer(typeof(ExitPermissionTO[]));
				bformatter.Serialize(stream, exitPermissionTOArray);
				stream.Close();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
	}
}
