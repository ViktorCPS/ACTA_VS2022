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
	/// Summary description for MSSQLAccessGroupXGateDAO.
	/// </summary>
	public class MSSQLAccessGroupXGateDAO : AccessGroupXGateDAO
	{
		SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;

		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

		public MSSQLAccessGroupXGateDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
		}
        public MSSQLAccessGroupXGateDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
        }
		public int insert(string accessGroupID, string gateID, string gateTimeAccessProfile, string readerAccessGroupOrdNum, bool doCommit)
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
			
			int rowsAffected = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO employee_groups_access_control_x_gates ");
				sbInsert.Append("(access_group_id, gate_id, gate_timeacess_profile, reader_access_group_ord_num, created_by, created_time) ");
				sbInsert.Append("VALUES (");

				sbInsert.Append(Int32.Parse(accessGroupID.Trim()) + ", ");
				sbInsert.Append(Int32.Parse(gateID.Trim()) + ", ");
				sbInsert.Append(Int32.Parse(gateTimeAccessProfile.Trim()) + ", ");
				sbInsert.Append(Int32.Parse(readerAccessGroupOrdNum.Trim()) + ", ");

				sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

				SqlCommand cmd = new SqlCommand( sbInsert.ToString(), conn, sqlTrans );
				rowsAffected = cmd.ExecuteNonQuery();

				if (doCommit)
				{
					sqlTrans.Commit();
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

			return rowsAffected;
		}		

		public bool update(string accessGroupID, string gateID, string gateTimeAccessProfile, string readerAccessGroupOrdNum, bool doCommit)
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
				sbUpdate.Append("UPDATE employee_groups_access_control_x_gates SET ");
				
				sbUpdate.Append("gate_timeacess_profile = " + Int32.Parse(gateTimeAccessProfile.Trim()) + ", ");
				sbUpdate.Append("reader_access_group_ord_num = " + Int32.Parse(readerAccessGroupOrdNum.Trim()) + ", ");
				
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = GETDATE() ");
				sbUpdate.Append("WHERE access_group_id = " + Int32.Parse(accessGroupID.Trim()));
				sbUpdate.Append(" AND gate_id = " + Int32.Parse(gateID.Trim()));
				
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

		public bool delete(string accessGroupID, string gateID, bool doCommit)
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
				sbDelete.Append("DELETE FROM employee_groups_access_control_x_gates ");
				sbDelete.Append("WHERE access_group_id = " + Int32.Parse(accessGroupID.Trim()));
				sbDelete.Append(" AND gate_id = " + Int32.Parse(gateID.Trim()));
				
				SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
				int res = cmd.ExecuteNonQuery();
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

		public bool deleteGates(string gateID, bool doCommit)
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
				sbDelete.Append("DELETE FROM employee_groups_access_control_x_gates ");
				sbDelete.Append("WHERE gate_id = " + Int32.Parse(gateID.Trim()));
				
				SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
				int res = cmd.ExecuteNonQuery();
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

		public AccessGroupXGateTO find(string accessGroupID, string gateID)
		{
			DataSet dataSet = new DataSet();
			AccessGroupXGateTO accessGroupXGateTO = new AccessGroupXGateTO();
			try
			{
				SqlCommand cmd = new SqlCommand("SELECT access_group_id, gate_id, gate_timeacess_profile, reader_access_group_ord_num FROM employee_groups_access_control_x_gates "
					+ "WHERE access_group_id = " + Int32.Parse(accessGroupID.Trim())
				    + " AND gate_id = " + Int32.Parse(gateID.Trim()), conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "AccessGroupXGate");
				DataTable table = dataSet.Tables["AccessGroupXGate"];

				if (table.Rows.Count == 1)
				{
					accessGroupXGateTO = new AccessGroupXGateTO();

					if (table.Rows[0]["access_group_id"] != DBNull.Value)
					{
						accessGroupXGateTO.AccessGroupID = Int32.Parse(table.Rows[0]["access_group_id"].ToString().Trim());
					}					
					if (!table.Rows[0]["gate_id"].Equals(DBNull.Value))
					{
						accessGroupXGateTO.GateID = Int32.Parse(table.Rows[0]["gate_id"].ToString().Trim());
					}
					if (!table.Rows[0]["gate_timeacess_profile"].Equals(DBNull.Value))
					{
						accessGroupXGateTO.GateTimeAccessProfile = Int32.Parse(table.Rows[0]["gate_timeacess_profile"].ToString().Trim());
					}
					if (!table.Rows[0]["reader_access_group_ord_num"].Equals(DBNull.Value))
					{
						accessGroupXGateTO.ReaderAccessGroupOrdNum = Int32.Parse(table.Rows[0]["reader_access_group_ord_num"].ToString().Trim());
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return accessGroupXGateTO;
		}

		
		public ArrayList getAccessGroupXGate(string accessGroupID, string gateID)
		{
			DataSet dataSet = new DataSet();
			AccessGroupXGateTO accessGroupXGateTO = new AccessGroupXGateTO();
			ArrayList accessGroupXGateList = new ArrayList();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT access_group_id, gate_id, gate_timeacess_profile, reader_access_group_ord_num FROM employee_groups_access_control_x_gates ");

				if (!accessGroupID.Trim().Equals("") || !gateID.Trim().Equals(""))
				{
					sb.Append(" WHERE ");

					if (!accessGroupID.Trim().Equals(""))
					{						
						sb.Append(" access_group_id = " + Int32.Parse(accessGroupID.Trim()) + " AND");
					}
					if (!gateID.Trim().Equals(""))
					{
						sb.Append(" gate_id = " + Int32.Parse(gateID.Trim()) + " AND");
					}

					select = sb.ToString(0, sb.ToString().Length - 3);										
				}
				else
				{
					select = sb.ToString();
				}

				//select = select + " ORDER BY name ";

				//SqlCommand cmd = new SqlCommand(select, conn );
                SqlCommand cmd;
                if (this.SqlTrans == null)
                    cmd = new SqlCommand(select, conn);
                else
                    cmd = new SqlCommand(select, conn, this.SqlTrans);
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "AccessGroupXGate");
				DataTable table = dataSet.Tables["AccessGroupXGate"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						accessGroupXGateTO = new AccessGroupXGateTO();

						if (!row["access_group_id"].Equals(DBNull.Value))
						{
							accessGroupXGateTO.AccessGroupID = (int)row["access_group_id"]; //Int32.Parse(row["access_group_id"].ToString().Trim());
						}
						if (!row["gate_id"].Equals(DBNull.Value))
						{
							accessGroupXGateTO.GateID = (int)row["gate_id"]; //Int32.Parse(row["gate_id"].ToString().Trim());
						}
						if (!row["gate_timeacess_profile"].Equals(DBNull.Value))
						{
							accessGroupXGateTO.GateTimeAccessProfile = (int)row["gate_timeacess_profile"]; //Int32.Parse(row["gate_timeacess_profile"].ToString().Trim());
						}
						if (!row["reader_access_group_ord_num"].Equals(DBNull.Value))
						{
							accessGroupXGateTO.ReaderAccessGroupOrdNum = (int)row["reader_access_group_ord_num"]; //Int32.Parse(row["reader_access_group_ord_num"].ToString().Trim());
						}
						accessGroupXGateList.Add(accessGroupXGateTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return accessGroupXGateList;
		}

		// TODO!!!
		public void serialize(ArrayList accessGroupXGateTOList)
		{
			try
			{
				string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLAccessGroupXGateFile"];
				Stream stream = File.Open(filename, FileMode.Create);

				AccessGroupXGateTO[] accessGroupXGateTOArray = (AccessGroupXGateTO[]) accessGroupXGateTOList.ToArray(typeof(AccessGroupXGateTO));

				XmlSerializer bformatter = new XmlSerializer(typeof(AccessGroupXGateTO[]));
				bformatter.Serialize(stream, accessGroupXGateTOArray);
				stream.Close();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
	}
}
