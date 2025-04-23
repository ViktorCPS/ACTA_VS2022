using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using TransferObjects;

namespace DataAccess
{
    public  class MSSQLWorkingUnitXOrganizationalUnitDAO:WorkingUnitXOrganizationalUnitDAO
    {
        SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;

		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

		public MSSQLWorkingUnitXOrganizationalUnitDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
		}

        public MSSQLWorkingUnitXOrganizationalUnitDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
        }

		public int insert(WorkingUnitXOrganizationalUnitTO wuXouTO)
		{
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
			int rowsAffected = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO working_units_x_organizational_units ");
				sbInsert.Append("(organizational_units_id, working_unit_id, purpose) ");
				sbInsert.Append("VALUES (");

                if (wuXouTO.OrgUnitID != -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
                    sbInsert.Append("'" + wuXouTO.OrgUnitID.ToString().Trim() + "', ");
				}
                if (wuXouTO.WorkingUnitID == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
                    sbInsert.Append("'" + wuXouTO.WorkingUnitID.ToString().Trim() + "', ");
				}
                if (wuXouTO.Purpose.Trim().Equals(""))
				{
					sbInsert.Append("null");
				}
				else
				{
					sbInsert.Append("N'" + wuXouTO.Purpose.Trim() + "')");
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

        public int insert(WorkingUnitXOrganizationalUnitTO wuXouTO, bool doCommit)
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
                sbInsert.Append("INSERT INTO working_units_x_organizational_units ");
                sbInsert.Append("(organizational_unit_id, working_unit_id, purpose) ");
                sbInsert.Append("VALUES (");

                if (wuXouTO.OrgUnitID == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + wuXouTO.OrgUnitID.ToString().Trim() + "', ");
                }
                if (wuXouTO.WorkingUnitID == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + wuXouTO.WorkingUnitID.ToString().Trim() + "', ");
                }
                if (wuXouTO.Purpose.Trim().Equals(""))
                {
                    sbInsert.Append("NULL)");
                }
                else
                {
                    sbInsert.Append("N'" + wuXouTO.Purpose.Trim() + "')");
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

        public bool delete(int orgUnitID, int wuID)
		{
			bool isDeleted = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");
			
			try
			{
				StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM working_units_x_organizational_units WHERE organizational_unit_id = '" + orgUnitID.ToString().Trim() + "'");
				sbDelete.Append(" AND working_unit_id = '" + wuID.ToString().Trim() + "'");
				
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

        public bool delete(int orgUnitID, int wuID,  bool doCommit)
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
                sbDelete.Append("DELETE FROM working_units_x_organizational_units WHERE organizational_unit_id = '" + orgUnitID.ToString().Trim() + "'");
                sbDelete.Append(" AND working_unit_id = '" + wuID.ToString().Trim() + "'");
				
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



        public List<WorkingUnitXOrganizationalUnitTO> getWUXOU(WorkingUnitXOrganizationalUnitTO auXwuTO)
		{
			DataSet dataSet = new DataSet();
            WorkingUnitXOrganizationalUnitTO applUsersXWUTO = new WorkingUnitXOrganizationalUnitTO();
            List<WorkingUnitXOrganizationalUnitTO> applUsersXWUList = new List<WorkingUnitXOrganizationalUnitTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM working_units_x_organizational_units ");

                if ((auXwuTO.OrgUnitID != -1) || (auXwuTO.WorkingUnitID != -1) ||
                    (!auXwuTO.Purpose.Trim().Equals("")))
				{
					sb.Append(" WHERE ");

                    if (auXwuTO.OrgUnitID != -1)
					{
                        sb.Append(" organizational_unit_id = " + auXwuTO.OrgUnitID.ToString().Trim() + " AND");
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

                select = select + "ORDER BY purpose ";

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ApplUsersXWU");
				DataTable table = dataSet.Tables["ApplUsersXWU"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
                        applUsersXWUTO = new WorkingUnitXOrganizationalUnitTO();
							
						if (!row["organizational_unit_id"].Equals(DBNull.Value))
						{
                            applUsersXWUTO.OrgUnitID = Int32.Parse(row["organizational_unit_id"].ToString().Trim());
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
    }
}
