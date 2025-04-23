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
	/// Summary description for MSSQLApplUsersXWUDAO.
	/// </summary>
	public class MSSQLApplUsersXWUDAO : ApplUsersXWUDAO
	{
		SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;

		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

		public MSSQLApplUsersXWUDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
		}

        public MSSQLApplUsersXWUDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
        }

		public int insert(string userID, int wuID, string purpose)
		{
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
			int rowsAffected = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO appl_users_x_working_units ");
				sbInsert.Append("(user_id, working_unit_id, purpose) ");
				sbInsert.Append("VALUES (");
				
				if (userID.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + userID.Trim() + "', ");
				}
				if (wuID == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + wuID.ToString().Trim() + "', ");
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

		public int insert(string userID, int wuID, string purpose, bool doCommit)
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
				sbInsert.Append("INSERT INTO appl_users_x_working_units ");
				sbInsert.Append("(user_id, working_unit_id, purpose) ");
				sbInsert.Append("VALUES (");
				
				if (userID.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + userID.Trim() + "', ");
				}
				if (wuID == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + wuID.ToString().Trim() + "', ");
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
				sbDelete.Append("DELETE FROM appl_users_x_working_units WHERE user_id = N'" + userID.ToString().Trim() + "'");
				sbDelete.Append(" AND working_unit_id = '" + wuID.ToString().Trim() + "'");
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
				sbDelete.Append("DELETE FROM appl_users_x_working_units WHERE user_id = N'" + userID.ToString().Trim() + "'");
                if (wuID != -1 || purpose != "")
                {
                    if (wuID != -1)
                    sbDelete.Append(" AND working_unit_id = '" + wuID.ToString().Trim() + "'");
                    if (purpose != "")
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

		public bool delete(int wuID, bool doCommit)
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
				sbDelete.Append("DELETE FROM appl_users_x_working_units WHERE working_unit_id = '" + wuID.ToString().Trim() + "'");
				
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

		public ApplUsersXWUTO find(string userID, int wuID, string purpose)
		{
			DataSet dataSet = new DataSet();
			ApplUsersXWUTO applUsersXWUTO = new ApplUsersXWUTO();
			try
			{
				StringBuilder sbFind = new StringBuilder();
				sbFind.Append("SELECT * FROM appl_users_x_working_units WHERE");
				sbFind.Append(" user_id = N'" + userID.Trim() + "'");
				sbFind.Append(" AND woking_unit_id = '" + wuID.ToString().Trim() + "'");
				sbFind.Append(" AND purpose = N'" + purpose.Trim() + "'");
				SqlCommand cmd = new SqlCommand(sbFind.ToString(), conn);
				
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ApplUsersXWU");
				DataTable table = dataSet.Tables["ApplUsersXWU"];

				if (table.Rows.Count == 1)
				{
					applUsersXWUTO = new ApplUsersXWUTO();
					if (!table.Rows[0]["user_id"].Equals(DBNull.Value))
					{
						applUsersXWUTO.UserID = table.Rows[0]["user_id"].ToString().Trim();
					}
					if (!table.Rows[0]["working_unit_id"].Equals(DBNull.Value))
					{
						applUsersXWUTO.WorkingUnitID = Int32.Parse(table.Rows[0]["working_unit_id"].ToString().Trim());
					}
					if (!table.Rows[0]["purpose"].Equals(DBNull.Value))
					{
						applUsersXWUTO.Purpose = table.Rows[0]["purpose"].ToString().Trim();
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return applUsersXWUTO;
		}

		public bool update(string userID, int wuID, string purpose)
		{
			bool isUpdated = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

			try
			{
				StringBuilder sbUpdate = new StringBuilder();
				sbUpdate.Append("UPDATE appl_users_x_working_units SET ");
				if (!userID.Trim().Equals(""))
				{
					sbUpdate.Append("user_id = N'" + userID.Trim() + "', ");
				}
				if (wuID != -1)
				{
					sbUpdate.Append("working_unit_id = N'" + wuID.ToString().Trim() + "', ");
				}
				if (!purpose.Trim().Equals(""))
				{
					sbUpdate.Append("purpose = N'" + purpose.Trim() + "', ");
				}
				
				sbUpdate.Append("WHERE user_id = N'" + userID.ToString().Trim() + "'");
				sbUpdate.Append(" AND working_unit_id = '" + wuID.ToString().Trim() + "'");
				sbUpdate.Append(" AND purpose = N'" + purpose.ToString().Trim() + "'");
				
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

		public List<ApplUsersXWUTO> getApplUsersXWU(ApplUsersXWUTO auXwuTO)
		{
			DataSet dataSet = new DataSet();
			ApplUsersXWUTO applUsersXWUTO = new ApplUsersXWUTO();
			List<ApplUsersXWUTO> applUsersXWUList = new List<ApplUsersXWUTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT * FROM appl_users_x_working_units ");

                if ((!auXwuTO.UserID.Trim().Equals("")) || (auXwuTO.WorkingUnitID != -1) ||
                    (!auXwuTO.Purpose.Trim().Equals("")))
				{
					sb.Append(" WHERE ");

                    if (!auXwuTO.UserID.Trim().Equals(""))
					{
                        sb.Append(" UPPER(user_id) LIKE N'%" + auXwuTO.UserID.ToUpper().Trim() + "%' AND");
					}
                    if (auXwuTO.WorkingUnitID != -1)
					{
                        sb.Append(" working_unit_id = '" + auXwuTO.WorkingUnitID.ToString().Trim() + "' AND");
					}
                    if (!auXwuTO.Purpose.Trim().Equals(""))
					{
                        sb.Append(" UPPER(purpose) LIKE N'%" + auXwuTO.Purpose.ToUpper().Trim() + "%' AND");
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
						applUsersXWUTO = new ApplUsersXWUTO();
							
						if (!row["user_id"].Equals(DBNull.Value))
						{
							applUsersXWUTO.UserID = row["user_id"].ToString().Trim();
						}
						if (!row["working_unit_id"].Equals(DBNull.Value))
						{
							applUsersXWUTO.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
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

        public List<WorkingUnitTO> findWUForUserID(string userID, string purpose, DAOFactory factory)
		{
			MSSQLWorkingUnitsDAO wuDAO = (MSSQLWorkingUnitsDAO) factory.getWorkingUnitsDAO(null);

			DataSet dataSet = new DataSet();

            List<WorkingUnitTO> wuForUserTOList = new List<WorkingUnitTO>();

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT uxwu.working_unit_id FROM appl_users_x_working_units uxwu, working_units wu ");
				sb.Append("WHERE UPPER(uxwu.user_id) = N'" + userID.ToUpper().Trim() + "' ");
				if (!purpose.Equals(""))
				{
					sb.Append("AND UPPER(uxwu.purpose) = N'" + purpose.ToUpper().Trim() + "'");
				}

				sb.Append(" AND uxwu.working_unit_id = wu.working_unit_id");
				SqlCommand cmd = new SqlCommand(sb.ToString() + " ORDER BY wu.name", conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ApplUsersXWU");
				DataTable table = dataSet.Tables["ApplUsersXWU"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						WorkingUnitTO wuTO = new WorkingUnitTO();
							
						if (!row["working_unit_id"].Equals(DBNull.Value))
						{
							wuTO = wuDAO.find(Int32.Parse(row["working_unit_id"].ToString().Trim()));
						}

						if (wuTO.WorkingUnitID != -1)
						{						
							wuForUserTOList.Add(wuTO);
						}
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return wuForUserTOList;
		}

        public List<WorkingUnitTO> findWUForUser(string userID, string purpose, DAOFactory factory, IDbTransaction trans)
        {
            MSSQLWorkingUnitsDAO wuDAO = (MSSQLWorkingUnitsDAO)factory.getWorkingUnitsDAO(conn);

            if (trans != null)
                wuDAO.SqlTrans = (SqlTransaction)trans;

            DataSet dataSet = new DataSet();

            List<WorkingUnitTO> wuForUserTOList = new List<WorkingUnitTO>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT uxwu.working_unit_id FROM appl_users_x_working_units uxwu, working_units wu ");
                sb.Append("WHERE UPPER(uxwu.user_id) = N'" + userID.ToUpper().Trim() + "' ");
                if (!purpose.Equals(""))
                {
                    sb.Append("AND UPPER(uxwu.purpose) = N'" + purpose.ToUpper().Trim() + "'");
                }

                sb.Append(" AND uxwu.working_unit_id = wu.working_unit_id");
                SqlCommand cmd;

                if (trans != null)
                    cmd = new SqlCommand(sb.ToString() + " ORDER BY wu.name", conn, (SqlTransaction)trans);
                else
                    cmd = new SqlCommand(sb.ToString() + " ORDER BY wu.name", conn);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ApplUsersXWU");
                DataTable table = dataSet.Tables["ApplUsersXWU"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        WorkingUnitTO wuTO = new WorkingUnitTO();

                        if (!row["working_unit_id"].Equals(DBNull.Value))
                        {
                            bool useTrans = trans != null;
                            wuTO = wuDAO.find(Int32.Parse(row["working_unit_id"].ToString().Trim()), useTrans);
                        }

                        if (wuTO.WorkingUnitID != -1)
                        {
                            wuForUserTOList.Add(wuTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return wuForUserTOList;
        }

        public Dictionary<int,WorkingUnitTO> findWUForUserIDDictionary(string userID, string purpose, DAOFactory factory)
        {
            MSSQLWorkingUnitsDAO wuDAO = (MSSQLWorkingUnitsDAO)factory.getWorkingUnitsDAO(null);

            DataSet dataSet = new DataSet();

            Dictionary<int, WorkingUnitTO> wuForUserTOList = new Dictionary<int, WorkingUnitTO>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT uxwu.working_unit_id FROM appl_users_x_working_units uxwu, working_units wu ");
                sb.Append("WHERE UPPER(uxwu.user_id) = N'" + userID.ToUpper().Trim() + "' ");
                if (!purpose.Equals(""))
                {
                    sb.Append("AND UPPER(uxwu.purpose) = N'" + purpose.ToUpper().Trim() + "'");
                }

                sb.Append(" AND uxwu.working_unit_id = wu.working_unit_id");
                SqlCommand cmd = new SqlCommand(sb.ToString() + " ORDER BY wu.name", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ApplUsersXWU");
                DataTable table = dataSet.Tables["ApplUsersXWU"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        WorkingUnitTO wuTO = new WorkingUnitTO();

                        if (!row["working_unit_id"].Equals(DBNull.Value))
                        {
                            wuTO = wuDAO.find(Int32.Parse(row["working_unit_id"].ToString().Trim()));
                        }

                        if (wuTO.WorkingUnitID != -1)
                        {
                            wuForUserTOList.Add(wuTO.WorkingUnitID, wuTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return wuForUserTOList;
        }

		public List<ApplUserTO> findUsersForWUID(int wuID, string purpose, List<string> statuses)
		{
            DataSet dataSet = new DataSet();
            List<ApplUserTO> usersForWUTOList = new List<ApplUserTO>();
            ApplUserTO applUserTO = new ApplUserTO();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT au.* FROM appl_users au, appl_users_x_working_units auwu ");
                sb.Append("WHERE auwu.working_unit_id = '" + wuID.ToString().Trim() + "' AND au.user_id = auwu.user_id");
                if (!purpose.Equals(""))
                {
                    sb.Append(" AND UPPER(auwu.purpose) = N'" + purpose.ToUpper().Trim() + "'");
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

                        usersForWUTOList.Add(applUserTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return usersForWUTOList;
		}

		public List<ApplUsersXWUTO> findGrantedParentWUForWU(ApplUsersXWUTO auXwuTO)
		{
			DataSet dataSet = new DataSet();
			ApplUsersXWUTO applUsersXWUTO = new ApplUsersXWUTO();
			List<ApplUsersXWUTO> applUsersXWUList = new List<ApplUsersXWUTO>();

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT uxwu.* FROM appl_users_x_working_units uxwu, working_units wu ");
                sb.Append("WHERE UPPER(uxwu.user_id) = N'" + auXwuTO.UserID.Trim().ToUpper() + "' AND UPPER(uxwu.purpose) = N'" + auXwuTO.Purpose.Trim().ToUpper() + "' AND ");
                sb.Append("wu.working_unit_id = '" + auXwuTO.WorkingUnitID.ToString().Trim() + "' AND ");
				sb.Append("wu.working_unit_id <> wu.parent_working_unit_id AND ");
				sb.Append("uxwu.working_unit_id = wu.parent_working_unit_id");			

				SqlCommand cmd = new SqlCommand(sb.ToString(), conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ApplUsersXWU");
				DataTable table = dataSet.Tables["ApplUsersXWU"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						applUsersXWUTO = new ApplUsersXWUTO();
							
						if (!row["user_id"].Equals(DBNull.Value))
						{
							applUsersXWUTO.UserID = row["user_id"].ToString().Trim();
						}
						if (!row["working_unit_id"].Equals(DBNull.Value))
						{
							applUsersXWUTO.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
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

		// TODO!!!
		public void serialize(List<ApplUsersXWUTO> applUsersXWUTOList)
		{
			try
			{
				string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLApplUsersXWUFile"];
				Stream stream = File.Open(filename, FileMode.Create);

				ApplUsersXWUTO[] applUsersXWUTOArray = (ApplUsersXWUTO[]) applUsersXWUTOList.ToArray();

				XmlSerializer bformatter = new XmlSerializer(typeof(ApplUsersXWUTO[]));
				bformatter.Serialize(stream, applUsersXWUTOArray);
				stream.Close();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
	}
}
