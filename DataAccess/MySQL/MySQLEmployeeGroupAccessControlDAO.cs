using System;
using System.Collections;
using System.Configuration;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using TransferObjects;
using Util;

namespace DataAccess
{
	/// <summary>
	/// Summary description for MySQLEmployeeGroupAccessControlDAO.
	/// </summary>
	public class MySQLEmployeeGroupAccessControlDAO : EmployeeGroupAccessControlDAO
	{
		MySqlConnection conn = null;

		public MySQLEmployeeGroupAccessControlDAO()
		{
			conn = MySQLDAOFactory.getConnection();
		}
        public MySQLEmployeeGroupAccessControlDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
        }

		public int insert(string name, string description)
		{
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
			int rowsAffected = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO employee_groups_access_control ");
				sbInsert.Append("(name, description, created_by, created_time) ");
				sbInsert.Append("VALUES (");

				sbInsert.Append("N'" + name.Trim() + "', ");

				sbInsert.Append("N'" + description.Trim() + "', ");

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

		public bool update(string accessGroupId, string name, string description)
		{
			bool isUpdated = false;
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

			try
			{
				StringBuilder sbUpdate = new StringBuilder();
				sbUpdate.Append("UPDATE employee_groups_access_control SET ");

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
				sbUpdate.Append("modified_time = NOW() ");
				sbUpdate.Append("WHERE access_group_id = " + Int32.Parse(accessGroupId.Trim()));
				
				MySqlCommand cmd = new MySqlCommand( sbUpdate.ToString(), conn, sqlTrans );
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

		public bool delete(string accessGroupId)
		{
			bool isDeleted = false;
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

			try
			{
				StringBuilder sbDelete = new StringBuilder();
				sbDelete.Append("DELETE FROM employee_groups_access_control WHERE access_group_id = " + Int32.Parse(accessGroupId.Trim()));
				
				MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, sqlTrans);
				int res = cmd.ExecuteNonQuery();
				if (res != 0)
				{
					isDeleted = true;
				}
				sqlTrans.Commit();
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback();
				
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public EmployeeGroupAccessControlTO find(string accessGroupId)
		{
			DataSet dataSet = new DataSet();
			EmployeeGroupAccessControlTO employeeGroupAccessControlTO = new EmployeeGroupAccessControlTO();
			try
			{
				MySqlCommand cmd = new MySqlCommand("SELECT access_group_id, name, description FROM employee_groups_access_control WHERE access_group_id = " + Int32.Parse(accessGroupId.Trim()), conn );				
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "EmployeeGroupAccessControl");
				DataTable table = dataSet.Tables["EmployeeGroupAccessControl"];

				if (table.Rows.Count == 1)
				{
					employeeGroupAccessControlTO = new EmployeeGroupAccessControlTO();

					if (table.Rows[0]["access_group_id"] != DBNull.Value)
					{
						employeeGroupAccessControlTO.AccessGroupId = Int32.Parse(table.Rows[0]["access_group_id"].ToString().Trim());
					}					

					if (!table.Rows[0]["name"].Equals(DBNull.Value))
					{
						employeeGroupAccessControlTO.Name = table.Rows[0]["name"].ToString().Trim();
					}

					if (!table.Rows[0]["description"].Equals(DBNull.Value))
					{
						employeeGroupAccessControlTO.Description = table.Rows[0]["description"].ToString().Trim();
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return employeeGroupAccessControlTO;
		}

		
		public ArrayList getEmployeeGroupAccessControl(string name)
		{
			DataSet dataSet = new DataSet();
			EmployeeGroupAccessControlTO employeeGroupAccessControlTO = new EmployeeGroupAccessControlTO();
			ArrayList employeeGroupAccessControlList = new ArrayList();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT access_group_id, name, description FROM employee_groups_access_control ");

				if (!name.Trim().Equals(""))
				{
					sb.Append(" WHERE ");					
					sb.Append(" UPPER(name) = N'" + name.ToUpper().Trim() + "'");

					select = sb.ToString();
				}
				else
				{
					select = sb.ToString();
				}

				select = select + " ORDER BY name ";

				MySqlCommand cmd = new MySqlCommand(select, conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "AccessGroups");
				DataTable table = dataSet.Tables["AccessGroups"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						employeeGroupAccessControlTO = new EmployeeGroupAccessControlTO();

						if (!row["access_group_id"].Equals(DBNull.Value))
						{
							employeeGroupAccessControlTO.AccessGroupId = (int)row["access_group_id"]; //Int32.Parse(row["access_group_id"].ToString().Trim());
						}
						if (!row["name"].Equals(DBNull.Value))
						{
							employeeGroupAccessControlTO.Name = row["name"].ToString().Trim();
						}
						if (!row["description"].Equals(DBNull.Value))
						{
							employeeGroupAccessControlTO.Description = row["description"].ToString().Trim();
						}
						employeeGroupAccessControlList.Add(employeeGroupAccessControlTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return employeeGroupAccessControlList;
		}

		// TODO!!!
		public void serialize(ArrayList employeeGroupAccessControlTOList)
		{
			try
			{
				// TODO: Not implemented yet
				string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLEmployeeGroupAccessControlFile"];
				Stream stream = File.Open(filename, FileMode.Create);

				EmployeeGroupAccessControlTO[] employeeGroupAccessControlTOArray = (EmployeeGroupAccessControlTO[]) employeeGroupAccessControlTOList.ToArray(typeof(EmployeeGroupAccessControlTO));

				XmlSerializer bformatter = new XmlSerializer(typeof(EmployeeGroupAccessControlTO[]));
				bformatter.Serialize(stream, employeeGroupAccessControlTOArray);
				stream.Close();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
	}
}
