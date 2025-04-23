using System;
using System.Collections;
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
	/// Summary description for MSSQLTimeAccessProfileDAO.
	/// </summary>
	public class MSSQLTimeAccessProfileDAO : TimeAccessProfileDAO
	{
		SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;

		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}
        public MSSQLTimeAccessProfileDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
        }
		public MSSQLTimeAccessProfileDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
		}

		public int insert(string name, string description, bool doCommit)
		{			
			SqlTransaction sqlTrans = null;

			if(doCommit)
			{
				sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
			}
			else
			{
				sqlTrans = this.SqlTrans;
			}

			int timeAccessProfileID = -1;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("SET NOCOUNT ON ");
				sbInsert.Append("INSERT INTO timeaccess_profiles_hdr ");
				sbInsert.Append("(name, description, created_by, created_time) ");
				sbInsert.Append("VALUES (");

				sbInsert.Append("N'" + name.Trim() + "', ");

				sbInsert.Append("N'" + description.Trim() + "', ");

				sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");
				sbInsert.Append("SELECT @@Identity AS timeaccess_profile_id, @@Error as error ");
				sbInsert.Append("SET NOCOUNT OFF ");

				SqlCommand cmd = new SqlCommand( sbInsert.ToString(), conn, sqlTrans );
				//rowsAffected = cmd.ExecuteNonQuery();
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
				DataSet dataSet = new DataSet();
				sqlDataAdapter.Fill(dataSet, "TimeaccessProfileId");
				DataTable table = dataSet.Tables["TimeaccessProfileId"];

				int error = int.Parse(((DataRow) table.Rows[0])["error"].ToString());
				if (error == 0) //OK
				{
					timeAccessProfileID = int.Parse(((DataRow) table.Rows[0])["timeaccess_profile_id"].ToString());

					if (doCommit)
					{
						sqlTrans.Commit();
					}
				}
				else
				{
					if (doCommit)
					{
						sqlTrans.Rollback("INSERT");
					}
				}								
			}
			catch(Exception ex)
			{
				if(doCommit)
					sqlTrans.Rollback("INSERT");
				else
					sqlTrans.Rollback();				
				throw ex;
			}

			return timeAccessProfileID;
		}		

		public bool update(string timeAccessProfileId, string name, string description, bool doCommit)
		{
			bool isUpdated = false;
			SqlTransaction sqlTrans = null;

			if(doCommit)
			{
				sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");
			}
			else
			{
				sqlTrans = this.SqlTrans;
			}

			try
			{
				StringBuilder sbUpdate = new StringBuilder();
				sbUpdate.Append("UPDATE timeaccess_profiles_hdr SET ");

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
				sbUpdate.Append("WHERE timeaccess_profile_id = " + Int32.Parse(timeAccessProfileId.Trim()));
				
				SqlCommand cmd = new SqlCommand( sbUpdate.ToString(), conn, sqlTrans );
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
				if(doCommit)
					sqlTrans.Rollback("UPDATE");
				else
					sqlTrans.Rollback();				
				
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public bool delete(string timeAccessProfileId, bool doCommit)
		{
			bool isDeleted = false;
			SqlTransaction sqlTrans = null;

			if(doCommit)
			{
				sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");
			}
			else
			{
				sqlTrans = this.SqlTrans;
			}

			try
			{
				StringBuilder sbDelete = new StringBuilder();
				// Delete from timeaccess_profiles_dtl
				sbDelete.Append("DELETE FROM timeaccess_profiles_dtl WHERE timeaccess_profile_id = " + Int32.Parse(timeAccessProfileId.Trim()));
				SqlCommand cmd = new SqlCommand( sbDelete.ToString(), conn, sqlTrans);
				int res = cmd.ExecuteNonQuery();
				sbDelete.Length = 0;

				// Delete from timeaccess_profiles_hdr
				sbDelete.Append("DELETE FROM timeaccess_profiles_hdr WHERE timeaccess_profile_id = " + Int32.Parse(timeAccessProfileId.Trim()));
				cmd.CommandText = sbDelete.ToString();
				//res += cmd.ExecuteNonQuery();
				res = cmd.ExecuteNonQuery();
				if (res != 0)
				{
					isDeleted = true;
				}
				if (doCommit)
				{
					sqlTrans.Commit();
				}
			}
			catch(Exception ex)
			{
				if(doCommit)
					sqlTrans.Rollback("DELETE");
				else
					sqlTrans.Rollback();				
				
				throw new Exception(ex.Message);
			}

			return isDeleted;
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

		public TimeAccessProfileTO find(string timeAccessProfileId)
		{
			DataSet dataSet = new DataSet();
			TimeAccessProfileTO timeAccessProfileTO = new TimeAccessProfileTO();
			try
			{
				SqlCommand cmd = new SqlCommand("SELECT timeaccess_profile_id, name, description FROM timeaccess_profiles_hdr WHERE timeaccess_profile_id = " + Int32.Parse(timeAccessProfileId.Trim()), conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "TimeAccessProfile");
				DataTable table = dataSet.Tables["TimeAccessProfile"];

				if (table.Rows.Count == 1)
				{
					timeAccessProfileTO = new TimeAccessProfileTO();

					if (table.Rows[0]["timeaccess_profile_id"] != DBNull.Value)
					{
						timeAccessProfileTO.TimeAccessProfileId = Int32.Parse(table.Rows[0]["timeaccess_profile_id"].ToString().Trim());
					}					

					if (!table.Rows[0]["name"].Equals(DBNull.Value))
					{
						timeAccessProfileTO.Name = table.Rows[0]["name"].ToString().Trim();
					}

					if (!table.Rows[0]["description"].Equals(DBNull.Value))
					{
						timeAccessProfileTO.Description = table.Rows[0]["description"].ToString().Trim();
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return timeAccessProfileTO;
		}
		
		public ArrayList getTimeAccessProfile(string name)
		{
			DataSet dataSet = new DataSet();
			TimeAccessProfileTO timeAccessProfileTO = new TimeAccessProfileTO();
			ArrayList timeAccessProfileList = new ArrayList();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT timeaccess_profile_id, name, description FROM timeaccess_profiles_hdr ");

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

				select = select + "ORDER BY name ";

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "TimeAccessProfile");
				DataTable table = dataSet.Tables["TimeAccessProfile"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						timeAccessProfileTO = new TimeAccessProfileTO();

						if (!row["timeaccess_profile_id"].Equals(DBNull.Value))
						{
							timeAccessProfileTO.TimeAccessProfileId = (int)row["timeaccess_profile_id"]; //Int32.Parse(row["timeaccess_profile_id"].ToString().Trim());
						}
						if (!row["name"].Equals(DBNull.Value))
						{
							timeAccessProfileTO.Name = row["name"].ToString().Trim();
						}
						if (!row["description"].Equals(DBNull.Value))
						{
							timeAccessProfileTO.Description = row["description"].ToString().Trim();
						}
						timeAccessProfileList.Add(timeAccessProfileTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return timeAccessProfileList;
		}

		// TODO!!!
		public void serialize(ArrayList timeAccessProfileTOList)
		{
			try
			{
				string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLTimeAccessProfileFile"];
				Stream stream = File.Open(filename, FileMode.Create);

				TimeAccessProfileTO[] timeAccessProfileTOArray = (TimeAccessProfileTO[]) timeAccessProfileTOList.ToArray(typeof(TimeAccessProfileTO));

				XmlSerializer bformatter = new XmlSerializer(typeof(TimeAccessProfileTO[]));
				bformatter.Serialize(stream, timeAccessProfileTOArray);
				stream.Close();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
	}
}
