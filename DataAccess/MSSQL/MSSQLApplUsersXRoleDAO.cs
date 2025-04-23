using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using TransferObjects;
using Util;

namespace DataAccess
{
	/// <summary>
	/// Summary description for MSSQLApplUsersXRoleDAO.
	/// </summary>
	public class MSSQLApplUsersXRoleDAO : ApplUsersXRoleDAO
	{
		SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;

		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

		public MSSQLApplUsersXRoleDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
		}
        public MSSQLApplUsersXRoleDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
        }


		public int insert(string userID, int roleID)
		{
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
			int rowsAffected = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO appl_users_x_appl_roles ");
				sbInsert.Append("(user_id, appl_role_id, created_by, created_time) ");
				sbInsert.Append("VALUES (");
				
				if (userID.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + userID.Trim() + "', ");
				}
				if (roleID == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + roleID.ToString().Trim() + "', ");
				}
				sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE())");

				SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans );
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

		public int insert(string userID, int roleID, bool doCommit)
		{
			SqlTransaction sqlTrans = null;
			if(doCommit)
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
				sbInsert.Append("INSERT INTO appl_users_x_appl_roles ");
				sbInsert.Append("(user_id, appl_role_id, created_by, created_time) ");
				sbInsert.Append("VALUES (");
				
				if (userID.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + userID.Trim() + "', ");
				}
				if (roleID == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + roleID.ToString().Trim() + "', ");
				}
			
				sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE())");

				SqlCommand cmd = new SqlCommand( sbInsert.ToString(), conn, sqlTrans );
				rowsAffected = cmd.ExecuteNonQuery();
				
				if (doCommit)
				{
					sqlTrans.Commit();
				}
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback();
			
				throw ex;
			}

			return rowsAffected;
		}

		public bool delete(string userID, int roleID)
		{
			bool isDeleted = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");
			
			try
			{
				StringBuilder sbDelete = new StringBuilder();
				sbDelete.Append("DELETE FROM appl_users_x_appl_roles WHERE user_id = N'" + userID.ToString().Trim() + "'");
				sbDelete.Append(" AND appl_role_id = '" + roleID.ToString().Trim() + "'");
				
				SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
				int res = cmd.ExecuteNonQuery();
				isDeleted = true;
				
				sqlTrans.Commit();
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback("INSERT");
								
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public bool delete(string userID, int roleID, bool doCommit)
		{
			bool isDeleted = false;
			SqlTransaction sqlTrans = null;

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
				StringBuilder sbDelete = new StringBuilder();
				sbDelete.Append("DELETE FROM appl_users_x_appl_roles WHERE user_id = N'" + userID.ToString().Trim() + "'");
				sbDelete.Append(" AND appl_role_id = '" + roleID.ToString().Trim() + "'");
				
				SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
				int res = cmd.ExecuteNonQuery();
				isDeleted = true;
				
				if (doCommit)
				{
					sqlTrans.Commit();
				}
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback();
				
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public ApplUsersXRoleTO find(string userID, int roleID)
		{
			DataSet dataSet = new DataSet();
			ApplUsersXRoleTO applUsersXRoleTO = new ApplUsersXRoleTO();
			try
			{
				StringBuilder sbFind = new StringBuilder();
				sbFind.Append("SELECT * FROM appl_users_x_applg_roles WHERE");
				sbFind.Append(" user_id = N'" + userID.Trim() + "'");
				sbFind.Append(" AND appl_role_id = '" + roleID.ToString().Trim() + "'");
				
				SqlCommand cmd = new SqlCommand(sbFind.ToString(), conn);
				
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ApplUsersXRoles");
				DataTable table = dataSet.Tables["ApplUsersXRoles"];

				if (table.Rows.Count == 1)
				{
					applUsersXRoleTO = new ApplUsersXRoleTO();
					if (!table.Rows[0]["user_id"].Equals(DBNull.Value))
					{
						applUsersXRoleTO.UserID = table.Rows[0]["user_id"].ToString().Trim();
					}
					if (!table.Rows[0]["appl_role_id"].Equals(DBNull.Value))
					{
						applUsersXRoleTO.RoleID = Int32.Parse(table.Rows[0]["appl_role_id"].ToString().Trim());
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return applUsersXRoleTO;
		}

		public bool update(string userID, int roleID)
		{
			bool isUpdated = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

			try
			{
				StringBuilder sbUpdate = new StringBuilder();
				sbUpdate.Append("UPDATE appl_users_x_appl_roles SET ");
				if (!userID.Trim().Equals(""))
				{
					sbUpdate.Append("user_id = N'" + userID.Trim() + "', ");
				}
				if (roleID != -1)
				{
					sbUpdate.Append("appl_role_id = N'" + roleID.ToString().Trim() + "', ");
				}
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = GETDATE() ");
				
				sbUpdate.Append("WHERE user_id = N'" + userID.ToString().Trim() + "'");
				sbUpdate.Append(" AND appl_role_id = '" + roleID.ToString().Trim() + "'");
                				
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

		public List<ApplUsersXRoleTO> getApplUsersXRoles(ApplUsersXRoleTO auXrTO)
		{
			DataSet dataSet = new DataSet();
			ApplUsersXRoleTO applUsersXRoleTO = new ApplUsersXRoleTO();
            List<ApplUsersXRoleTO> applUsersXRoleList = new List<ApplUsersXRoleTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT * FROM appl_users_x_appl_roles ");

                if ((!auXrTO.UserID.Trim().Equals("")) || (auXrTO.RoleID != -1))
				{
					sb.Append(" WHERE ");

                    if (!auXrTO.UserID.Trim().Equals(""))
					{
                        sb.Append(" UPPER(user_id) LIKE N'%" + auXrTO.UserID.ToUpper().Trim() + "%' AND");
					}
                    if (auXrTO.RoleID != -1)
					{
                        sb.Append(" appl_role_id = '" + auXrTO.RoleID.ToString().Trim() + "' AND");
					}

					select = sb.ToString(0, sb.ToString().Length - 3);
				}
				else
				{
					select = sb.ToString();
				}
				
				select = select + "ORDER BY user_id, appl_role_id";

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ApplUsersXRole");
				DataTable table = dataSet.Tables["ApplUsersXRole"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						applUsersXRoleTO = new ApplUsersXRoleTO();
							
						if (!row["user_id"].Equals(DBNull.Value))
						{
							applUsersXRoleTO.UserID = row["user_id"].ToString().Trim();
						}
						if (!row["appl_role_id"].Equals(DBNull.Value))
						{
							applUsersXRoleTO.RoleID = Int32.Parse(row["appl_role_id"].ToString().Trim());
						}
						applUsersXRoleList.Add(applUsersXRoleTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return applUsersXRoleList;
		}

		public List<ApplRoleTO> findRolesForUserID(string userID, DAOFactory factory)
		{
			MSSQLApplRoleDAO roleDAO = (MSSQLApplRoleDAO) factory.getApplRoleDAO(conn);

			DataSet dataSet = new DataSet();

			List<ApplRoleTO> rolesForUserTOList = new List<ApplRoleTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				select = "SELECT uxr.appl_role_id FROM appl_users_x_appl_roles uxr, appl_roles ar "
					+ "WHERE UPPER(uxr.user_id) = N'" + userID.ToUpper().Trim() + "' "
					+ "AND uxr.appl_role_id = ar.appl_role_id ORDER BY ar.name";

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ApplUsersXRoles");
				DataTable table = dataSet.Tables["ApplUsersXRoles"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						ApplRoleTO roleTO = new ApplRoleTO();
							
						if (!row["appl_role_id"].Equals(DBNull.Value))
						{
							roleTO = roleDAO.find(Int32.Parse(row["appl_role_id"].ToString().Trim()));
						}

						if (roleTO.ApplRoleID != -1)
						{						
							rolesForUserTOList.Add(roleTO);
						}
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return rolesForUserTOList;
		}

		public List<ApplUserTO> findUsersForRoleID(int roleID)
		{
            DataSet dataSet = new DataSet();
            ApplUserTO applUserTO = new ApplUserTO();
            List<ApplUserTO> usersForRoleTOList = new List<ApplUserTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                select = "SELECT au.* FROM appl_users au, appl_users_x_appl_roles aur WHERE au.user_id = aur.user_id AND aur.appl_role_id = '" + roleID.ToString().Trim() + "'";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ApplUsers");
                DataTable table = dataSet.Tables["ApplUsers"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        applUserTO = new ApplUserTO();

                        if (!row["user_id"].Equals(DBNull.Value))
                        {
                            applUserTO.UserID = row["user_id"].ToString().Trim();
                        }
                        if (!row["password"].Equals(DBNull.Value))
                        {
                            applUserTO.Password = row["password"].ToString().Trim();
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            applUserTO.Name = row["name"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            applUserTO.Description = row["description"].ToString().Trim();
                        }
                        if (!row["privilege_lvl"].Equals(DBNull.Value))
                        {
                            applUserTO.PrivilegeLvl = Int32.Parse(row["privilege_lvl"].ToString().Trim());
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            applUserTO.Status = row["status"].ToString().Trim();
                        }
                        if (!row["num_of_tries"].Equals(DBNull.Value))
                        {
                            applUserTO.NumOfTries = Int32.Parse(row["num_of_tries"].ToString().Trim());
                        }
                        if (!row["lang_code"].Equals(DBNull.Value))
                        {
                            applUserTO.LangCode = row["lang_code"].ToString().Trim();
                        }

                        usersForRoleTOList.Add(applUserTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return usersForRoleTOList;
		}

		public bool beginTransaction()
		{
			bool isStarted = false;

			try
			{
				this.SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
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

		// TODO!!!
		public void serialize(List<ApplUsersXRoleTO> applUsersXRoleTOList)
		{
			try
			{
				string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLApplUsersXRoleFile"];
				Stream stream = File.Open(filename, FileMode.Create);

				ApplUsersXRoleTO[] applUsersXRoleTOArray = (ApplUsersXRoleTO[]) applUsersXRoleTOList.ToArray();

				XmlSerializer bformatter = new XmlSerializer(typeof(ApplUsersXRoleTO[]));
				bformatter.Serialize(stream, applUsersXRoleTOArray);
				stream.Close();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
	}
}
