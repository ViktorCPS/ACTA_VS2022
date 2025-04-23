using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using TransferObjects;

namespace DataAccess
{
   public class MSSQLApplUserXOrgUnitDAO:ApplUserXOrgUnitDAO
    {
       SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;

		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

		public MSSQLApplUserXOrgUnitDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
		}

      
        public MSSQLApplUserXOrgUnitDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
        }

		public int insert(string userID, int ouID, string purpose)
		{
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
			int rowsAffected = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO appl_users_x_organizational_units ");
                sbInsert.Append("(user_id, organizational_unit_id, purpose) ");
				sbInsert.Append("VALUES (");
				
				if (userID.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + userID.Trim() + "', ");
				}
				if (ouID == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + ouID.ToString().Trim() + "', ");
				}
				if (purpose.Trim().Equals(""))
				{
					sbInsert.Append("null");
				}
				else
				{
					sbInsert.Append("N'" + purpose.Trim() + "')");
				}

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

		public int insert(string userID, int ouID, string purpose, bool doCommit)
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
                sbInsert.Append("INSERT INTO appl_users_x_organizational_units ");
                sbInsert.Append("(user_id, organizational_unit_id, purpose) ");
				sbInsert.Append("VALUES (");
				
				if (userID.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + userID.Trim() + "', ");
				}
				if (ouID == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + ouID.ToString().Trim() + "', ");
				}
				if (purpose.Trim().Equals(""))
				{
					sbInsert.Append("null)");
				}
				else
				{
					sbInsert.Append("N'" + purpose.Trim() + "')");
				}

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
                    sqlTrans.Rollback();
                }

				throw ex;
			}

			return rowsAffected;
		}

		public bool delete(string userID, int wuID, string purpose)
		{
			bool isDeleted = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");
			
			try
			{
				StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM appl_users_x_organizational_units WHERE user_id = N'" + userID.ToString().Trim() + "'");               
				sbDelete.Append(" AND organizational_unit_id = '" + wuID.ToString().Trim() + "'");
				sbDelete.Append(" AND purpose = N'" + purpose.Trim() + "'");
				
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

		public bool delete(string userID, int wuID, string purpose, bool doCommit)
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
				sbDelete.Append("DELETE FROM appl_users_x_organizational_units WHERE user_id = N'" + userID.ToString().Trim() + "'");
                if (wuID != -1 || purpose != "")
                {
                    sbDelete.Append(" AND organizational_unit_id = '" + wuID.ToString().Trim() + "'");
                    sbDelete.Append(" AND purpose = N'" + purpose.Trim() + "'");
                }
				
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
                if (doCommit)
                {
                    sqlTrans.Rollback();
                }

				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

        public bool delete( int ouID, bool doCommit)
        {
            bool isDeleted = false;
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
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM appl_users_x_organizational_units WHERE organizational_unit_id = '" + ouID.ToString().Trim() + "'");
                
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
                    sqlTrans.Rollback();
                }

                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public List<ApplUserXOrgUnitTO> getApplUsersXOU(ApplUserXOrgUnitTO auXouTO)
		{
			DataSet dataSet = new DataSet();
            ApplUserXOrgUnitTO applUsersXWUTO = new ApplUserXOrgUnitTO();
            List<ApplUserXOrgUnitTO> applUsersXWUList = new List<ApplUserXOrgUnitTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT * FROM appl_users_x_organizational_units ");

                if ((!auXouTO.UserID.Trim().Equals("")) || (auXouTO.OrgUnitID!= -1) ||
                    (!auXouTO.Purpose.Trim().Equals("")))
				{
					sb.Append(" WHERE ");

                    if (!auXouTO.UserID.Trim().Equals(""))
					{
                        sb.Append(" UPPER(user_id) LIKE N'%" + auXouTO.UserID.ToUpper().Trim() + "%' AND");
					}
                    if (auXouTO.OrgUnitID != -1)
					{
                        sb.Append(" organizational_unit_id = '" + auXouTO.OrgUnitID.ToString().Trim() + "' AND");
					}
                    if (!auXouTO.Purpose.Trim().Equals(""))
					{
                        sb.Append(" UPPER(purpose) LIKE N'%" + auXouTO.Purpose.ToUpper().Trim() + "%' AND");
					}

					select = sb.ToString(0, sb.ToString().Length - 3);
				}
				else
				{
					select = sb.ToString();
				}
				
				select = select + "ORDER BY user_id ";

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ApplUsersXWU");
				DataTable table = dataSet.Tables["ApplUsersXWU"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
                        applUsersXWUTO = new ApplUserXOrgUnitTO();
							
						if (!row["user_id"].Equals(DBNull.Value))
						{
							applUsersXWUTO.UserID = row["user_id"].ToString().Trim();
						}
						if (!row["organizational_unit_id"].Equals(DBNull.Value))
						{
							applUsersXWUTO.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
						}
						if (!row["purpose"].Equals(DBNull.Value))
						{
							applUsersXWUTO.Purpose = row["purpose"].ToString().Trim();
						}
						applUsersXWUList.Add(applUsersXWUTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return applUsersXWUList;
		}

        public List<ApplUserXOrgUnitTO> getApplUsersXOU(string ids)
        {
            DataSet dataSet = new DataSet();
            ApplUserXOrgUnitTO applUsersXWUTO = new ApplUserXOrgUnitTO();
            List<ApplUserXOrgUnitTO> applUsersXWUList = new List<ApplUserXOrgUnitTO>();
            string select = "";

            try
            {
                if (ids.Trim().Length <= 0)
                    return applUsersXWUList;

                select = "SELECT * FROM appl_users_x_organizational_units WHERE organizational_unit_id IN (" + ids.Trim() + ") ORDER BY user_id";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ApplUsersXWU");
                DataTable table = dataSet.Tables["ApplUsersXWU"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        applUsersXWUTO = new ApplUserXOrgUnitTO();

                        if (!row["user_id"].Equals(DBNull.Value))
                        {
                            applUsersXWUTO.UserID = row["user_id"].ToString().Trim();
                        }
                        if (!row["organizational_unit_id"].Equals(DBNull.Value))
                        {
                            applUsersXWUTO.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
                        }
                        if (!row["purpose"].Equals(DBNull.Value))
                        {
                            applUsersXWUTO.Purpose = row["purpose"].ToString().Trim();
                        }
                        applUsersXWUList.Add(applUsersXWUTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return applUsersXWUList;
        }

        public Dictionary<int, OrganizationalUnitTO> findOUForUserIDDictionary(string userID, string purpose, DAOFactory factory)
        {
            MSSQLOrganizationalUnitDAO ouDAO = (MSSQLOrganizationalUnitDAO)factory.getOrganizationalUnitDAO(null);

            DataSet dataSet = new DataSet();

            Dictionary<int, OrganizationalUnitTO> ouForUserTOList = new Dictionary<int, OrganizationalUnitTO>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT organizational_unit_id FROM appl_users_x_organizational_units ");
                sb.Append("WHERE UPPER(user_id) = N'" + userID.ToUpper().Trim() + "' ");
                if (!purpose.Equals(""))
                {
                    sb.Append("AND UPPER(purpose) = N'" + purpose.ToUpper().Trim() + "'");
                }
                
                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ApplUsersXOU");
                DataTable table = dataSet.Tables["ApplUsersXOU"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        OrganizationalUnitTO ouTO = new OrganizationalUnitTO();

                        if (!row["organizational_unit_id"].Equals(DBNull.Value))
                        {
                            ouTO = ouDAO.find(Int32.Parse(row["organizational_unit_id"].ToString().Trim()));
                        }

                        if (ouTO.OrgUnitID != -1)
                        {
                            ouForUserTOList.Add(ouTO.OrgUnitID, ouTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return ouForUserTOList;
        }

        public List<ApplUserTO> findUsersForOUID(int ouID, string purpose, List<string> statuses)
        {
            DataSet dataSet = new DataSet();
            List<ApplUserTO> usersForOUTOList = new List<ApplUserTO>();
            ApplUserTO applUserTO = new ApplUserTO();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT au.* FROM appl_users au, appl_users_x_organizational_units auou ");
                sb.Append("WHERE auou.organizational_unit_id = '" + ouID.ToString().Trim() + "' AND au.user_id = auou.user_id");
                if (!purpose.Equals(""))
                {
                    sb.Append(" AND UPPER(auou.purpose) = N'" + purpose.ToUpper().Trim() + "'");
                }
                if (statuses.Count > 0)
                {
                    sb.Append(" AND au.status IN (");

                    string stString = "";
                    foreach (string status in statuses)
                    {
                        stString += "'" + status.Trim() + "',";
                    }

                    if (stString.Length > 0)
                        stString = stString.Substring(0, stString.Length - 1);

                    sb.Append(stString + ")");
                }

                SqlCommand cmd = new SqlCommand(sb.ToString() + " ORDER BY au.user_id", conn);
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

                        usersForOUTOList.Add(applUserTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return usersForOUTOList;
        }

        public List<ApplUserXOrgUnitTO> findGrantedParentOUForOU(ApplUserXOrgUnitTO auXouTO)
        {
            DataSet dataSet = new DataSet();
            ApplUserXOrgUnitTO applUsersXOUTO = new ApplUserXOrgUnitTO();
            List<ApplUserXOrgUnitTO> applUsersXOUList = new List<ApplUserXOrgUnitTO>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT uxou.* FROM appl_users_x_organizational_units uxou, organizational_units ou ");
                sb.Append("WHERE UPPER(uxou.user_id) = N'" + auXouTO.UserID.Trim().ToUpper() + "' AND UPPER(uxou.purpose) = N'" + auXouTO.Purpose.Trim().ToUpper() + "' AND ");
                sb.Append("ou.organizational_unit_id = '" + auXouTO.OrgUnitID.ToString().Trim() + "' AND ");
                sb.Append("ou.organizational_unit_id <> ou.parent_organizational_unit_id AND ");
                sb.Append("uxou.organizational_unit_id = ou.parent_organizational_unit_id");

                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ApplUsersXOU");
                DataTable table = dataSet.Tables["ApplUsersXOU"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        applUsersXOUTO = new ApplUserXOrgUnitTO();

                        if (!row["user_id"].Equals(DBNull.Value))
                        {
                            applUsersXOUTO.UserID = row["user_id"].ToString().Trim();
                        }
                        if (!row["organizational_unit_id"].Equals(DBNull.Value))
                        {
                            applUsersXOUTO.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
                        }
                        if (!row["purpose"].Equals(DBNull.Value))
                        {
                            applUsersXOUTO.Purpose = row["purpose"].ToString().Trim();
                        }
                        applUsersXOUList.Add(applUsersXOUTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return applUsersXOUList;
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
