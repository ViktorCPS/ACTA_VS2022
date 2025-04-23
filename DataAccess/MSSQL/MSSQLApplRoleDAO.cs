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
	/// Summary description for MSSQLApplRoleDAO.
	/// </summary>
	public class MSSQLApplRoleDAO : ApplRoleDAO
	{
		SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;

		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

		public MSSQLApplRoleDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
		}
        public MSSQLApplRoleDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
        }
		public int insert(int applRoleID, string name, string description)
		{
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
			int rowsAffected = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO appl_roles ");
				sbInsert.Append("(appl_role_id, name, description, created_by, created_time) ");
				sbInsert.Append("VALUES (");

				sbInsert.Append("'" + applRoleID.ToString().Trim() + "'");
				sbInsert.Append("N'" + name.Trim() + "', ");

				if (description.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + description.Trim() + "', ");
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

		public bool delete(int applRoleID)
		{
			bool isDeleted = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

			try
			{
				StringBuilder sbDelete = new StringBuilder();
				sbDelete.Append("DELETE FROM appl_roles WHERE appl_role_id = '" + applRoleID.ToString().Trim() + "'");
				
				SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
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

		public ApplRoleTO find(int applRoleID)
		{
			DataSet dataSet = new DataSet();
			ApplRoleTO applRoleTO = new ApplRoleTO();
			try
			{
				SqlCommand cmd = new SqlCommand( "SELECT * FROM appl_roles WHERE appl_role_id = '" + applRoleID.ToString().Trim() + "'", conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ApplRoles");
				DataTable table = dataSet.Tables["ApplRoles"];

				if (table.Rows.Count == 1)
				{
					applRoleTO = new ApplRoleTO();
					applRoleTO.ApplRoleID = Int32.Parse(table.Rows[0]["appl_role_id"].ToString().Trim());

					applRoleTO.Name = table.Rows[0]["name"].ToString().Trim();
					
					if (!table.Rows[0]["description"].Equals(DBNull.Value))
					{
						applRoleTO.Description = table.Rows[0]["description"].ToString().Trim();
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return applRoleTO;
		}

		public int findEmptyRole()
		{
			DataSet dataSet = new DataSet();
			int applRoleID = 0;
			try
			{
				SqlCommand cmd = new SqlCommand( "SELECT MIN(appl_role_id) AS appl_role_id FROM appl_roles WHERE name LIKE 'EMPTY ROLE%'", conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ApplRoles");
				DataTable table = dataSet.Tables["ApplRoles"];

				if (table.Rows.Count == 1 && !table.Rows[0]["appl_role_id"].Equals(DBNull.Value))
				{					
					applRoleID = Int32.Parse(table.Rows[0]["appl_role_id"].ToString().Trim());
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return applRoleID;
		}

		public bool update(int applRoleID, string name, string description)
		{
			bool isUpdated = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

			try
			{
				StringBuilder sbUpdate = new StringBuilder();
				sbUpdate.Append("UPDATE appl_roles SET ");
				if (!name.Trim().Equals(""))
				{
					sbUpdate.Append("name = N'" + name.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("name = null, ");
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
				sbUpdate.Append("WHERE appl_role_id = '" + applRoleID.ToString().Trim() + "'");
				
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
				sqlTrans.Rollback("UPDATE");
				
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public bool updateOnEmptyRole(int applRoleID, DAOFactory factory)
		{
			MSSQLApplMenuItemDAO menuItemDAO = (MSSQLApplMenuItemDAO) factory.getApplMenuItemDAO(conn);
			
			bool isUpdated = false;
			this.SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
			menuItemDAO.SqlTrans = this.SqlTrans;

			try
			{
				StringBuilder sbUpdate = new StringBuilder();
				sbUpdate.Append("UPDATE appl_roles SET ");
				sbUpdate.Append("name = 'EMPTY ROLE " + applRoleID.ToString().Trim() + "', ");
				sbUpdate.Append("description = 'MEMBER HAS NO  ACCESS AND NO CONTROL ON ANY OBJECT AND ANY OPERATION', ");
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = GETDATE() ");
				sbUpdate.Append("WHERE appl_role_id = '" + applRoleID.ToString().Trim() + "'");
				
				SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, this.SqlTrans);
				int res = cmd.ExecuteNonQuery();

				if(res > 0)
				{
					isUpdated = true;	
				}

				isUpdated = isUpdated && menuItemDAO.updateEmptyRole(applRoleID, false);

				this.SqlTrans.Commit();
			}
			catch(Exception ex)
			{
				this.SqlTrans.Rollback();
				
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public List<ApplRoleTO> getApplRoles(ApplRoleTO roleTO)
		{
			DataSet dataSet = new DataSet();
			ApplRoleTO applRoleTO = new ApplRoleTO();
			List<ApplRoleTO> applRolesList = new List<ApplRoleTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT * FROM appl_roles ");

				if((roleTO.ApplRoleID != -1) || (!roleTO.Name.Trim().Equals("")) || 
					(!roleTO.Description.Trim().Equals("")))
				{
					sb.Append(" WHERE ");
					
					if (roleTO.ApplRoleID != -1)
					{
						sb.Append(" appl_role_id = '" + roleTO.ApplRoleID.ToString().Trim() + "' AND");
					}
					if (!roleTO.Name.Trim().Equals(""))
					{
						sb.Append(" UPPER(name) LIKE N'%" + roleTO.Name.ToUpper().Trim() + "%' AND");
					}
					if (!roleTO.Description.Trim().Equals(""))
					{
						sb.Append(" UPPER(description) LIKE N'%" + roleTO.Description.ToUpper().Trim() + "%' AND");
					}

					select = sb.ToString(0, sb.ToString().Length - 3);
				}
				else
				{
					select = sb.ToString();
				}
				
				select = select + "ORDER BY appl_role_id ";

				SqlCommand cmd = new SqlCommand(select, conn);
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ApplRoles");
				DataTable table = dataSet.Tables["ApplRoles"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						applRoleTO = new ApplRoleTO();
							
						applRoleTO.ApplRoleID = Int32.Parse(row["appl_role_id"].ToString().Trim());
						if (!row["name"].Equals(DBNull.Value))
						{
							applRoleTO.Name = row["name"].ToString().Trim();
						}
						if (!row["description"].Equals(DBNull.Value))
						{
							applRoleTO.Description = row["description"].ToString().Trim();
						}

						applRolesList.Add(applRoleTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return applRolesList;
		}

		public List<ApplRoleTO> getUserCreatedRoles()
		{
			DataSet dataSet = new DataSet();
			ApplRoleTO applRoleTO = new ApplRoleTO();
			List<ApplRoleTO> applRolesList = new List<ApplRoleTO>();
			string select = "";

			try
			{
				select = "SELECT * FROM appl_roles WHERE UPPER(name) NOT LIKE 'EMPTY ROLE%' ORDER BY appl_role_id ";

				SqlCommand cmd = new SqlCommand(select, conn);
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ApplRoles");
				DataTable table = dataSet.Tables["ApplRoles"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						applRoleTO = new ApplRoleTO();
							
						applRoleTO.ApplRoleID = Int32.Parse(row["appl_role_id"].ToString().Trim());
						if (!row["name"].Equals(DBNull.Value))
						{
							applRoleTO.Name = row["name"].ToString().Trim();
						}
						if (!row["description"].Equals(DBNull.Value))
						{
							applRoleTO.Description = row["description"].ToString().Trim();
						}

						applRolesList.Add(applRoleTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return applRolesList;
		}

		// TODO!!!
		public void serialize(List<ApplRoleTO> ApplRolesTOList)
		{
			try
			{
				string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLApplRoleFile"];
				Stream stream = File.Open(filename, FileMode.Create);

				ApplRoleTO[] applRoleTOArray = (ApplRoleTO[]) ApplRolesTOList.ToArray();

				XmlSerializer bformatter = new XmlSerializer(typeof(ApplRoleTO[]));
				bformatter.Serialize(stream, applRoleTOArray);
				stream.Close();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
	}
}
