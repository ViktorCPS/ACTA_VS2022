using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using TransferObjects;

namespace DataAccess
{
   public class MySQLPassTypesConfirmationDAO:PassTypesConfirmationDAO
    {
         MySqlConnection conn = null;
		MySqlTransaction _sqlTrans = null;

		public MySqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

		public MySQLPassTypesConfirmationDAO()
		{
			conn = MySQLDAOFactory.getConnection();
		}

        public MySQLPassTypesConfirmationDAO(MySqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
        }

		public int insert(PassTypesConfirmationTO ptConfirmTO)
		{
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
			int rowsAffected = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO pass_types_confirmation ");
                sbInsert.Append("(pass_type_id, confirmation_pass_type_id,created_by,created_time ) ");
				sbInsert.Append("VALUES (");

                if (ptConfirmTO.PassTypeID == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
                    sbInsert.Append("'" + ptConfirmTO.PassTypeID.ToString().Trim() + "', ");
				}
                if (ptConfirmTO.ConfirmationPassTypeID == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + ptConfirmTO.ConfirmationPassTypeID.ToString().Trim() + "', ");
                }

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

        public int insert(PassTypesConfirmationTO ptConfirmTO, bool doCommit)
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

			int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO pass_types_confirmation ");
                sbInsert.Append("(pass_type_id, confirmation_pass_type_id,created_by,created_time ) ");
                sbInsert.Append("VALUES (");

                if (ptConfirmTO.PassTypeID == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + ptConfirmTO.PassTypeID.ToString().Trim() + "', ");
                }
                if (ptConfirmTO.ConfirmationPassTypeID == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + ptConfirmTO.ConfirmationPassTypeID.ToString().Trim() + "', ");
                }

                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");


                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();
                if (doCommit)
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                sqlTrans.Rollback();

                throw ex;
            }

            return rowsAffected;
        }

        public bool delete(int passTypeID, int ConfirmPT)
		{
			bool isDeleted = false;
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
			
			try
			{
				StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM pass_types_confirmation WHERE pass_type_id = '" + passTypeID.ToString().Trim() + "'");
				sbDelete.Append(" AND confirmation_pass_type_id = '" + ConfirmPT.ToString().Trim() + "'");
				
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

        public bool delete(int passTypeID, int ConfirmPT, bool doCommit)
		{
			bool isDeleted = false;
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
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM pass_types_confirmation WHERE pass_type_id = '" + passTypeID.ToString().Trim() + "'");
                sbDelete.Append(" AND confirmation_pass_type_id = '" + ConfirmPT.ToString().Trim() + "'");
				
				MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, sqlTrans);
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

        public bool delete(int passTypeID, bool doCommit)
        {
            bool isDeleted = false;
            MySqlTransaction sqlTrans = null;

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
                sbDelete.Append("DELETE FROM pass_types_confirmation WHERE pass_type_id = '" + passTypeID.ToString().Trim() + "'");

                MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, sqlTrans);
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




        public List<PassTypesConfirmationTO> getPTConfirmation(PassTypesConfirmationTO ptConfirmTO)
		{
			DataSet dataSet = new DataSet();
            PassTypesConfirmationTO ptcTO = new PassTypesConfirmationTO();
            List<PassTypesConfirmationTO> ptcList = new List<PassTypesConfirmationTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM pass_types_confirmation ");

                if ((ptConfirmTO.PassTypeID != -1) || (ptConfirmTO.ConfirmationPassTypeID != -1))
				{
					sb.Append(" WHERE ");

                    if (ptConfirmTO.PassTypeID != -1)
					{
                        sb.Append(" pass_type_id = " + ptConfirmTO.PassTypeID.ToString().Trim() + " AND");
					}
                    if (ptConfirmTO.ConfirmationPassTypeID != -1)
					{
                        sb.Append(" confirmation_pass_type_id = '" + ptConfirmTO.ConfirmationPassTypeID.ToString().Trim() + "' AND");
					}
                  
					select = sb.ToString(0, sb.ToString().Length - 3);
				}
				else
				{
					select = sb.ToString();
				}

                select = select + "ORDER BY pass_type_id ";

				MySqlCommand cmd = new MySqlCommand(select, conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ApplUsersXWU");
				DataTable table = dataSet.Tables["ApplUsersXWU"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
                        ptcTO = new PassTypesConfirmationTO();
							
						if (!row["pass_type_id"].Equals(DBNull.Value))
						{
                            ptcTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
						}
						if (!row["confirmation_pass_type_id"].Equals(DBNull.Value))
						{
                            ptcTO.ConfirmationPassTypeID = Int32.Parse(row["confirmation_pass_type_id"].ToString().Trim());
						}
						ptcList.Add(ptcTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return ptcList;
		}

        public Dictionary<int, List<int>> getPTConfirmationDictionary(PassTypesConfirmationTO ptConfirmTO)
        {
            DataSet dataSet = new DataSet();
            PassTypesConfirmationTO ptcTO = new PassTypesConfirmationTO();
            Dictionary<int, List<int>> ptcDictionary = new Dictionary<int, List<int>>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM pass_types_confirmation ");

                if ((ptConfirmTO.PassTypeID != -1) || (ptConfirmTO.ConfirmationPassTypeID != -1))
                {
                    sb.Append(" WHERE ");

                    if (ptConfirmTO.PassTypeID != -1)
                    {
                        sb.Append(" pass_types_id = " + ptConfirmTO.PassTypeID.ToString().Trim() + " AND");
                    }
                    if (ptConfirmTO.ConfirmationPassTypeID != -1)
                    {
                        sb.Append(" confirmation_pass_type_id = '" + ptConfirmTO.ConfirmationPassTypeID.ToString().Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY pass_type_id ";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ApplUsersXWU");
                DataTable table = dataSet.Tables["ApplUsersXWU"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        ptcTO = new PassTypesConfirmationTO();

                        if (!row["pass_type_id"].Equals(DBNull.Value))
                        {
                            ptcTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (!row["confirmation_pass_type_id"].Equals(DBNull.Value))
                        {
                            ptcTO.ConfirmationPassTypeID = Int32.Parse(row["confirmation_pass_type_id"].ToString().Trim());
                        }

                        if (!ptcDictionary.ContainsKey(ptcTO.PassTypeID))
                            ptcDictionary.Add(ptcTO.PassTypeID, new List<int>());
                        if (!ptcDictionary[ptcTO.PassTypeID].Contains(ptcTO.ConfirmationPassTypeID))
                            ptcDictionary[ptcTO.PassTypeID].Add(ptcTO.ConfirmationPassTypeID);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return ptcDictionary;
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
			_sqlTrans = (MySqlTransaction)trans;
		}
        	
    }
}
