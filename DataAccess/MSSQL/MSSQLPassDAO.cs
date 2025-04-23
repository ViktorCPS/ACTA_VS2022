using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using Util;
using TransferObjects;
using System.Collections.Generic;

namespace DataAccess
{
	/// <summary>
	/// Summary description for MSSQLPassDAO.
	/// </summary>
	public class MSSQLPassDAO : PassDAO
	{
		SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;
		protected string dateTimeformat = "";

		public MSSQLPassDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

        public MSSQLPassDAO(SqlConnection SqlConnection)
        {
            conn = SqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        
		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

		public int insert(int employeeID, string direction, DateTime eventTime, int passTypeID, int pairGenUsed,
			int locationID, int manualCreated, int isWrkHrsCount)
		{
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
			int rowsAffected = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO passes ");
				sbInsert.Append("(employee_id, direction, event_time, pass_type_id, pair_gen_used, location_id, manual_created, is_wrk_hrs, created_by, created_time) ");
				sbInsert.Append("VALUES (");

				if (employeeID != -1)
				{
					sbInsert.Append("'" + employeeID.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}

				if (!direction.Trim().Equals(""))
				{
					sbInsert.Append("N'" + direction.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}

				if (!eventTime.Equals(new DateTime()))
				{
					sbInsert.Append("'" + eventTime.ToString(dateTimeformat).Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (passTypeID != -1)
				{
					sbInsert.Append("'" + passTypeID.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if ( pairGenUsed != -1)
				{
					sbInsert.Append("'" + pairGenUsed.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if ( locationID != -1)
				{
					sbInsert.Append("'" + locationID.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}

				if ( manualCreated != -1)
				{
					sbInsert.Append("'" + manualCreated.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}

				if ( isWrkHrsCount != -1)
				{
					sbInsert.Append("'" + isWrkHrsCount.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}

				sbInsert.Append(" N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");
					
				SqlCommand cmd = new SqlCommand( sbInsert.ToString(), conn, sqlTrans );

				rowsAffected = cmd.ExecuteNonQuery();
				sqlTrans.Commit();				
			}
			catch(SqlException sqlEx)
			{
				sqlTrans.Rollback("INSERT");
				if (sqlEx.Number == 2627)
				{
					throw new Exception(sqlEx.Number.ToString());
				}
				else
				{
					throw new Exception(sqlEx.Message);
				}
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback("INSERT");
				throw ex;
			}

			return rowsAffected;
		}
		
		public int insert(PassTO passTO)
		{
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
			int rowsAffected = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO passes ");
				sbInsert.Append("(employee_id, direction, event_time, pass_type_id, pair_gen_used, location_id, manual_created, is_wrk_hrs, gate_id, created_by, created_time) ");
				sbInsert.Append("VALUES ('");

				if (passTO.EmployeeID != -1)
				{
					sbInsert.Append(passTO.EmployeeID.ToString().Trim() + ", ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}

				if (!passTO.Direction.Trim().Equals(""))
				{
					sbInsert.Append("N'" + passTO.Direction.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}

				if (!passTO.EventTime.Equals(new DateTime()))
				{
					sbInsert.Append(passTO.EventTime.ToString(dateTimeformat).Trim() + ", ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (passTO.PassTypeID != -1)
				{
					sbInsert.Append(passTO.PassTypeID.ToString().Trim() + ", ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if ( passTO.PairGenUsed != -1)
				{
					sbInsert.Append(passTO.PairGenUsed.ToString().Trim() + ", ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if ( passTO.LocationID != -1)
				{
					sbInsert.Append(passTO.LocationID.ToString().Trim() + ", ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}

				if ( passTO.ManualyCreated != -1)
				{
					sbInsert.Append(passTO.ManualyCreated.ToString().Trim() + ", ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}

				if ( passTO.IsWrkHrsCount != -1)
				{
					sbInsert.Append(passTO.IsWrkHrsCount.ToString().Trim() + ", ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
                sbInsert.Append("'" + passTO.GateID.ToString().Trim() + "', ");
				sbInsert.Append(" N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");
				
				SqlCommand cmd = new SqlCommand( sbInsert.ToString(), conn, sqlTrans );
				rowsAffected = cmd.ExecuteNonQuery();
				sqlTrans.Commit();				
			}
			catch(SqlException sqlEx)
			{
				sqlTrans.Rollback("INSERT");
				if (sqlEx.Number == 2627)
				{
					throw new Exception(sqlEx.Number.ToString());
				}
				else
				{
					throw new Exception(sqlEx.Message);
				}
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback("INSERT");
				throw ex;
			}

			return rowsAffected;
		}

		/// <summary>
		/// Insert Pass record
		/// </summary>
		/// <param name="logTo"></param>
		/// <param name="doCommit">true - do commit, false  transaction is started elsewhere, 
		/// don't do commit here</param>
		/// <returns>true if succedeed, false otherwise</returns>
		public int insert(PassTO passTO, bool doCommit)
		{
			int rowsAffected = 0;
			SqlTransaction sqlTrans = null;
			if(doCommit)
			{
				sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
			}
			else
			{
				sqlTrans = this.SqlTrans;
			}
			
			SqlCommand cmd = null;
			StringBuilder sbInsert = new StringBuilder();

			try
			{				
				sbInsert.Append("INSERT INTO passes ");
				sbInsert.Append("(employee_id, direction, event_time, pass_type_id, pair_gen_used, location_id, manual_created, is_wrk_hrs, gate_id, created_by, created_time) ");
				sbInsert.Append("VALUES (");

				if (passTO.EmployeeID != -1)
				{
					sbInsert.Append("'" + passTO.EmployeeID.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}

				if (!passTO.Direction.Trim().Equals(""))
				{
					sbInsert.Append("N'" + passTO.Direction.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}

				if (!passTO.EventTime.Equals(new DateTime()))
				{
					sbInsert.Append("'" + passTO.EventTime.ToString(dateTimeformat).Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (passTO.PassTypeID != -1)
				{
					sbInsert.Append("'" + passTO.PassTypeID.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if ( passTO.PairGenUsed != -1)
				{
					sbInsert.Append("'" + passTO.PairGenUsed.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if ( passTO.LocationID != -1)
				{
					sbInsert.Append("'" + passTO.LocationID.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}

				if ( passTO.ManualyCreated != -1)
				{
					sbInsert.Append("'" + passTO.ManualyCreated.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}

				if ( passTO.IsWrkHrsCount != -1)
				{
					sbInsert.Append("'" + passTO.IsWrkHrsCount.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}

                sbInsert.Append("'" + passTO.GateID.ToString().Trim() + "', ");

				sbInsert.Append(" N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");
				
				cmd = new SqlCommand( sbInsert.ToString(), conn, sqlTrans);
				rowsAffected = cmd.ExecuteNonQuery();
				if (doCommit)
				{
					sqlTrans.Commit();
				}				
			}
			catch(SqlException sqlEx)
			{
				if(doCommit)
				{
					sqlTrans.Rollback("INSERT");
				}
				else
				{
					sqlTrans.Rollback();
				}
				if (sqlEx.Number == 2627)
				{
					DataProcessingException pdEx = new DataProcessingException(sqlEx.Message + passTO.ToString(), 8);
					throw pdEx;
				}
				else
				{
					throw new Exception(sqlEx.Message);
				}
			}
			catch(Exception ex)
			{
				if (doCommit)
				{
					sqlTrans.Rollback("INSERT");
				}
				else
				{
					sqlTrans.Rollback();
				}
				DataProcessingException pdEx = new DataProcessingException(ex.Message + passTO.ToString(), 9);
				throw pdEx;
			}

			return rowsAffected;
		}

        /// <summary>
        /// Insert Pass record
        /// </summary>
        /// <param name="logTo"></param>
        /// <param name="doCommit">true - do commit, false  transaction is started elsewhere, 
        /// don't do commit here</param>
        /// <returns>PassID</returns>
        public int insertGetID(PassTO passTO, bool doCommit)
        {
            //Tamara 6.2.2020. zakomentarisala da bi se pojavili
            //Tamara 22.11.2018. da ne bih imala u tabeli io_pairs prolaske koji se odnose na kontrolu pristupa,jer se u suprotnom moze desiti da se ne spajaju dobro parovi 
            //if (passTO.IsWrkHrsCount !=(int) Constants.IsWrkCount.IsCounter)
            //    passTO.PairGenUsed = 1;
            
            int passID = -1;
            SqlTransaction sqlTrans = null;
            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }

            SqlCommand cmd = null;
            StringBuilder sbInsert = new StringBuilder();

            try
            {
                sbInsert.Append("INSERT INTO passes ");
                sbInsert.Append("(employee_id, direction, event_time, pass_type_id, pair_gen_used, location_id, manual_created, is_wrk_hrs, gate_id, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                if (passTO.EmployeeID != -1)
                {
                    sbInsert.Append("'" + passTO.EmployeeID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (!passTO.Direction.Trim().Equals(""))
                {
                    sbInsert.Append("N'" + passTO.Direction.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (!passTO.EventTime.Equals(new DateTime()))
                {
                    sbInsert.Append("'" + passTO.EventTime.ToString(dateTimeformat).Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (passTO.PassTypeID != -1)
                {
                    sbInsert.Append("'" + passTO.PassTypeID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (passTO.PairGenUsed != -1)
                {
                    sbInsert.Append("'" + passTO.PairGenUsed.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (passTO.LocationID != -1)
                {
                    sbInsert.Append("'" + passTO.LocationID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (passTO.ManualyCreated != -1)
                {
                    sbInsert.Append("'" + passTO.ManualyCreated.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (passTO.IsWrkHrsCount != -1)
                {
                    sbInsert.Append("'" + passTO.IsWrkHrsCount.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append("'" + passTO.GateID.ToString().Trim() + "', ");

                sbInsert.Append(" N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");
                //sbInsert.Append("SELECT @@Identity AS pass_id ");                
                //sbInsert.Append("SET NOCOUNT OFF ");
                sbInsert.Append("SELECT IDENT_CURRENT('passes') AS pass_id");

                DataSet dataSet = new DataSet();
                cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Passes");

                DataTable table = dataSet.Tables["Passes"];

                if (table.Rows[0]["pass_id"] != DBNull.Value)
                    passID = Int32.Parse(table.Rows[0]["pass_id"].ToString());

                if (doCommit)
                {
                    sqlTrans.Commit();
                }
            }
            catch (SqlException sqlEx)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback("INSERT");
                }
                else
                {
                    sqlTrans.Rollback();
                }
                if (sqlEx.Number == 2627)
                {
                    DataProcessingException pdEx = new DataProcessingException(sqlEx.Message + passTO.ToString(), 8);
                    throw pdEx;
                }
                else
                {
                    throw new Exception(sqlEx.Message);
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback("INSERT");
                }
                else
                {
                    sqlTrans.Rollback();
                }
                DataProcessingException pdEx = new DataProcessingException(ex.Message + passTO.ToString(), 9);
                throw pdEx;
            }

            return passID;
        }

		public int insert(PassTO passTO, string createdBy, bool doCommit)
		{
			int rowsAffected = 0;
			SqlTransaction sqlTrans = null;
			if(doCommit)
			{
				sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
			}
			else
			{
				sqlTrans = this.SqlTrans;
			}
			
			SqlCommand cmd = null;
			StringBuilder sbInsert = new StringBuilder();

			try
			{
				sbInsert.Append("INSERT INTO passes ");
				sbInsert.Append("(employee_id, direction, event_time, pass_type_id, pair_gen_used, location_id, manual_created, is_wrk_hrs, gate_id, created_by, created_time) ");
				sbInsert.Append("VALUES (");

				if (passTO.EmployeeID != -1)
				{
					sbInsert.Append("'" + passTO.EmployeeID.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}

				if (!passTO.Direction.Trim().Equals(""))
				{
					sbInsert.Append("N'" + passTO.Direction.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}

				if (!passTO.EventTime.Equals(new DateTime()))
				{
					sbInsert.Append("'" + passTO.EventTime.ToString(dateTimeformat).Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (passTO.PassTypeID != -1)
				{
					sbInsert.Append("'" + passTO.PassTypeID.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if ( passTO.PairGenUsed != -1)
				{
					sbInsert.Append("'" + passTO.PairGenUsed.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if ( passTO.LocationID != -1)
				{
					sbInsert.Append("'" + passTO.LocationID.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}

				if ( passTO.ManualyCreated != -1)
				{
					sbInsert.Append("'" + passTO.ManualyCreated.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}

				if ( passTO.IsWrkHrsCount != -1)
				{
					sbInsert.Append("'" + passTO.IsWrkHrsCount.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
                sbInsert.Append("'" + passTO.GateID.ToString().Trim() + "', ");
				if (!createdBy.Equals(""))
				{
					sbInsert.Append(" N'" + createdBy.Trim() + "', ");
				}
				else
				{
					sbInsert.Append(" N'" + DAOController.GetLogInUser().Trim() + "', ");
				}
				
				sbInsert.Append("GETDATE())");
				
				cmd = new SqlCommand( sbInsert.ToString(), conn, sqlTrans);
				rowsAffected = cmd.ExecuteNonQuery();
				if (doCommit)
				{
					sqlTrans.Commit();
				}				
			}
			catch(SqlException sqlEx)
			{
				if(doCommit)
				{
					sqlTrans.Rollback("INSERT");
				}
				else
				{
					sqlTrans.Rollback();
				}
				if (sqlEx.Number == 2627)
				{
					DataProcessingException pdEx = new DataProcessingException(sqlEx.Message + passTO.ToString(), 8);
					throw pdEx;
				}
				else
				{
					throw new Exception(sqlEx.Message);
				}
			}
			catch(Exception ex)
			{
				if (doCommit)
				{
					sqlTrans.Rollback("INSERT");
				}
				else
				{
					sqlTrans.Rollback();
				}
				DataProcessingException pdEx = new DataProcessingException(ex.Message + passTO.ToString(), 9);
				throw pdEx;
			}

			return rowsAffected;
		}

		public int insert(List<PassTO> passTOList, ExitPermissionTO perm, string createdBy, DAOFactory factory)
		{			
			int rowsAffected = 0;
			bool updated = false;
			this.SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
			MSSQLExitPermissionDAO exitPermDAO = (MSSQLExitPermissionDAO) factory.getExitPermissionsDAO(null);
			exitPermDAO.SqlTrans = this.SqlTrans;

			try
			{
				foreach(PassTO passTO in passTOList)
				{
					rowsAffected += this.insert(passTO, createdBy, false);
				}
				
				if(rowsAffected == passTOList.Count)
				{
					if (perm.PermissionID >= 0)
					{
						updated = exitPermDAO.update(perm.PermissionID, perm.EmployeeID,
							perm.PassTypeID, perm.StartTime, perm.Offset, perm.Used, 
							perm.Description, false);
					}
				}

				if (updated)
				{
					commitTransaction();
				}
				else
				{
					rollbackTransaction();
				}
			}
			catch(Exception ex)
			{
				rollbackTransaction();
				throw ex;
			}

			return rowsAffected;
		}

		public int insertPassesPermission(List<PassTO> passTOList, ExitPermissionTO perm, string createdBy, DAOFactory factory)
		{			
			int rowsAffected = 0;
			int inserted = 0;
			this.SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
			MSSQLExitPermissionDAO exitPermDAO = (MSSQLExitPermissionDAO) factory.getExitPermissionsDAO(conn);
			exitPermDAO.SqlTrans = this.SqlTrans;

			try
			{
				foreach(PassTO passTO in passTOList)
				{
					rowsAffected += this.insert(passTO, createdBy, false);
				}
				
				if(rowsAffected == passTOList.Count)
				{
					inserted = exitPermDAO.insert(perm.EmployeeID,
						perm.PassTypeID, perm.StartTime, perm.Offset, perm.Used, 
						perm.Description, perm.VerifiedBy, false);
				}

				if (inserted > 0)
				{
					commitTransaction();
				}
				else
				{
					rollbackTransaction();
				}
			}
			catch(Exception ex)
			{
				rollbackTransaction();
				throw ex;
			}

			return rowsAffected;
		}

		public bool delete(string passID)
		{
			bool isDeleted = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

			try
			{
                StringBuilder sbSelect = new StringBuilder();
                sbSelect.Append("SELECT employee_id, convert(varchar(10), event_time, 111) currentDate ");
                sbSelect.Append("FROM passes ");
                sbSelect.Append("WHERE ");
                sbSelect.Append("pass_id = " + Int32.Parse(passID.Trim()));

                SqlCommand cmdS = new SqlCommand(sbSelect.ToString(), conn, sqlTrans);
                SqlDataAdapter sqlDA = new SqlDataAdapter(cmdS);
                DataSet dataSetI = new DataSet();
                int employeeID = 0;
                string currentDate = "";

                sqlDA.Fill(dataSetI, "PasesInfo");
                DataTable tableI = dataSetI.Tables["PasesInfo"];

                if (tableI.Rows.Count > 0)
                {
                    employeeID = Int32.Parse(tableI.Rows[0]["employee_id"].ToString().Trim());
                    currentDate = tableI.Rows[0]["currentDate"].ToString().Trim();
                }

				StringBuilder sbDelete = new StringBuilder();
				sbDelete.Append("DELETE FROM passes WHERE pass_id = " + Int32.Parse(passID.Trim()));
				
				SqlCommand cmd = new SqlCommand( sbDelete.ToString(), conn, sqlTrans);
				int res = cmd.ExecuteNonQuery();
				if (res != 0)
				{
					isDeleted = true;
				}

                if (isDeleted)
                {
                    sbSelect.Length = 0;
                    sbSelect.Append("SELECT COUNT(*) count ");
                    sbSelect.Append("FROM passes ");
                    sbSelect.Append("WHERE ");
                    sbSelect.Append("employee_id = " + employeeID);
                    sbSelect.Append(" AND convert(datetime, convert(varchar(10), event_time, 111), 111) = convert(datetime,'" + currentDate + "', 111)");

                    cmdS.CommandText = sbSelect.ToString();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmdS);
                    DataSet dataSet = new DataSet();
                    int count = 0;

                    sqlDataAdapter.Fill(dataSet, "PasesCount");
                    DataTable table = dataSet.Tables["PasesCount"];

                    if (table.Rows.Count > 0)
                    {
                        count = Int32.Parse(table.Rows[0]["count"].ToString().Trim());
                    }

                    if (count > 0)
                    {
                        StringBuilder sbUpdate = new StringBuilder();
                        sbUpdate.Append("UPDATE passes SET ");
                        sbUpdate.Append("pair_gen_used = 0, ");
                        sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                        sbUpdate.Append("modified_time = GETDATE() ");
                        sbUpdate.Append("WHERE ");
                        sbUpdate.Append("employee_id = " + employeeID);
                        sbUpdate.Append(" AND convert(datetime, convert(varchar(10), event_time, 111), 111) = convert(datetime,'" + currentDate + "', 111)");

                        SqlCommand cmdU = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                        int resU = cmdU.ExecuteNonQuery();
                        if (resU <= 0)
                        {
                            isDeleted = false;
                        }  
                    } //if (count > 0)
                }

                if (isDeleted)
                {
                    sqlTrans.Commit();
                }
                else
                {
                    sqlTrans.Rollback("DELETE");
                }
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback("DELETE");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

        public bool delete(string passID, bool doCommit)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = null;
            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }

            try
            {
                StringBuilder sbSelect = new StringBuilder();
                sbSelect.Append("SELECT employee_id, convert(varchar(10), event_time, 111) currentDate ");
                sbSelect.Append("FROM passes ");
                sbSelect.Append("WHERE ");
                sbSelect.Append("pass_id = " + Int32.Parse(passID.Trim()));

                SqlCommand cmdS = new SqlCommand(sbSelect.ToString(), conn, sqlTrans);
                SqlDataAdapter sqlDA = new SqlDataAdapter(cmdS);
                DataSet dataSetI = new DataSet();
                int employeeID = 0;
                string currentDate = "";

                sqlDA.Fill(dataSetI, "PasesInfo");
                DataTable tableI = dataSetI.Tables["PasesInfo"];

                if (tableI.Rows.Count > 0)
                {
                    employeeID = Int32.Parse(tableI.Rows[0]["employee_id"].ToString().Trim());
                    currentDate = tableI.Rows[0]["currentDate"].ToString().Trim();
                }

                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM passes WHERE pass_id = " + Int32.Parse(passID.Trim()));

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    isDeleted = true;
                }

                if (isDeleted)
                {
                    sbSelect.Length = 0;
                    sbSelect.Append("SELECT COUNT(*) count ");
                    sbSelect.Append("FROM passes ");
                    sbSelect.Append("WHERE ");
                    sbSelect.Append("employee_id = " + employeeID);
                    sbSelect.Append(" AND convert(datetime, convert(varchar(10), event_time, 111), 111) = convert(datetime,'" + currentDate + "', 111)");

                    cmdS.CommandText = sbSelect.ToString();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmdS);
                    DataSet dataSet = new DataSet();
                    int count = 0;

                    sqlDataAdapter.Fill(dataSet, "PasesCount");
                    DataTable table = dataSet.Tables["PasesCount"];

                    if (table.Rows.Count > 0)
                    {
                        count = Int32.Parse(table.Rows[0]["count"].ToString().Trim());
                    }

                    if (count > 0)
                    {
                        StringBuilder sbUpdate = new StringBuilder();
                        sbUpdate.Append("UPDATE passes SET ");
                        sbUpdate.Append("pair_gen_used = 0, ");
                        sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                        sbUpdate.Append("modified_time = GETDATE() ");
                        sbUpdate.Append("WHERE ");
                        sbUpdate.Append("employee_id = " + employeeID);
                        sbUpdate.Append(" AND convert(datetime, convert(varchar(10), event_time, 111), 111) = convert(datetime,'" + currentDate + "', 111)");

                        SqlCommand cmdU = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                        int resU = cmdU.ExecuteNonQuery();
                        if (resU <= 0)
                        {
                            isDeleted = false;
                        }
                    } //if (count > 0)
                }

                if (doCommit)
                {
                    if (isDeleted)
                    {
                        sqlTrans.Commit();
                    }
                    else
                    {
                        sqlTrans.Rollback("DELETE");
                    }
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback("DELETE");
                }
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        //public bool delete(string passID, bool doCommit)
        //{
        //    bool isDeleted = false;
        //    SqlTransaction sqlTrans = null;
        //    if(doCommit)
        //    {
        //        sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
        //    }
        //    else
        //    {
        //        sqlTrans = this.SqlTrans;
        //    }

        //    try
        //    {
        //        StringBuilder sbDelete = new StringBuilder();
        //        sbDelete.Append("DELETE FROM passes WHERE pass_id = " + Int32.Parse(passID.Trim()));
				
        //        SqlCommand cmd = new SqlCommand( sbDelete.ToString(), conn, sqlTrans);
        //        int res = cmd.ExecuteNonQuery();
        //        if (res != 0)
        //        {
        //            isDeleted = true;
        //        }

        //        if (doCommit)
        //        {
        //            sqlTrans.Commit();
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        if (doCommit)
        //        {
        //            sqlTrans.Rollback("INSERT");
        //        }
        //        else
        //        {
        //            sqlTrans.Rollback();
        //        }
        //        throw new Exception(ex.Message);
        //    }

        //    return isDeleted;
        //}

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

		public PassTO find(string passID)
		{
			DataSet dataSet = new DataSet();
			PassTO pass = new PassTO();
			try
			{
                string select = "SELECT ps.*, empl.last_name, empl.first_name, pt.description, loc.name, wu.name wu_name "
					+ "FROM passes ps, employees empl, pass_types pt, locations loc,working_units wu "
					+ "WHERE ps.pass_id = '" + passID.Trim() + "' AND "
					+ "empl.employee_id = ps.employee_id AND pt.pass_type_id = ps.pass_type_id AND "
					+ "loc.location_id = ps.location_id AND empl.working_unit_id = wu.working_unit_id";

				SqlCommand cmd = new SqlCommand(select, conn);
                cmd.CommandTimeout = Constants.commandTimeout;

				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Pass");
				DataTable table = dataSet.Tables["Pass"];

				if (table.Rows.Count == 1)
				{
					pass = new PassTO();
					pass.PassID = Int32.Parse(table.Rows[0]["pass_id"].ToString().Trim());
					
					if (table.Rows[0]["employee_id"] != DBNull.Value)
					{
						pass.EmployeeID = Int32.Parse(table.Rows[0]["employee_id"].ToString().Trim());
					}
					if (table.Rows[0]["direction"] != DBNull.Value)
					{
						pass.Direction = table.Rows[0]["direction"].ToString().Trim();
					}
					if (table.Rows[0]["event_time"] != DBNull.Value)
					{
						pass.EventTime = (DateTime) table.Rows[0]["event_time"];
					}
					if (table.Rows[0]["pass_type_id"] != DBNull.Value)
					{
						pass.PassTypeID = Int32.Parse(table.Rows[0]["pass_type_id"].ToString().Trim());
					}
					if (table.Rows[0]["pair_gen_used"] != DBNull.Value)
					{
						pass.PairGenUsed = Int32.Parse(table.Rows[0]["pair_gen_used"].ToString().Trim());
					}
					if (table.Rows[0]["location_id"] != DBNull.Value)
					{
						pass.LocationID = Int32.Parse(table.Rows[0]["location_id"].ToString().Trim());
					}
					if (table.Rows[0]["manual_created"] != DBNull.Value)
					{
						pass.ManualyCreated = Int32.Parse(table.Rows[0]["manual_created"].ToString().Trim());
					}
					if (table.Rows[0]["is_wrk_hrs"] != DBNull.Value)
					{
						pass.IsWrkHrsCount = Int32.Parse(table.Rows[0]["is_wrk_hrs"].ToString().Trim());
					}
                    if (table.Rows[0]["created_by"] != DBNull.Value)
                    {
                        pass.CreatedBy = table.Rows[0]["created_by"].ToString().Trim();
                    }
                    if (table.Rows[0]["created_time"] != DBNull.Value)
                    {
                        pass.CreatedTime = (DateTime)table.Rows[0]["created_time"];
                    }
					if (table.Rows[0]["last_name"] != DBNull.Value)
					{
						pass.EmployeeName = table.Rows[0]["last_name"].ToString().Trim();
					}
					if (table.Rows[0]["first_name"] != DBNull.Value)
					{
						pass.EmployeeName += " " + table.Rows[0]["first_name"].ToString().Trim();
					}
					if (table.Rows[0]["description"] != DBNull.Value)
					{
						pass.PassType = table.Rows[0]["description"].ToString().Trim();
					}
					if (table.Rows[0]["name"] != DBNull.Value)
					{
						pass.LocationName = table.Rows[0]["name"].ToString().Trim();
					}
                    if (table.Rows[0]["wu_name"] != DBNull.Value)
					{
						pass.WUName = table.Rows[0]["wu_name"].ToString().Trim();
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception("Exception: " + ex.Message);
			}

			return pass;
		}

		public bool update(int passID, int employeeID, string direction, DateTime eventTime, int passTypeID, int pairGenUsed,
			int locationID, int manualCreated, int isWrkHrsCount)
		{
			bool isUpdated = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

			try
			{
				StringBuilder sbUpdate = new StringBuilder();
				sbUpdate.Append("UPDATE passes SET ");

				if (employeeID != -1)
				{
					sbUpdate.Append("employee_id = " + employeeID.ToString().Trim() + ", ");
				}
				else
				{
					sbUpdate.Append("employee_id = NULL, ");
				}

				if (!direction.Trim().Equals(""))
				{
					sbUpdate.Append("direction = N'" +  direction.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("direction = NULL, ");
				}

				if (!eventTime.Equals(new DateTime()))
				{
					sbUpdate.Append("event_time = '" +  eventTime.ToString(dateTimeformat) + "', ");
				}
				else
				{
					sbUpdate.Append("event_time = NULL, ");
				}
				if (passTypeID != -1)
				{
					sbUpdate.Append("pass_type_id = " +  passTypeID.ToString().Trim() + ", ");
				}
				else
				{
					sbUpdate.Append("pass_type_id = NULL, ");
				}
				if ( pairGenUsed != -1)
				{
					sbUpdate.Append("pair_gen_used = " + pairGenUsed.ToString().Trim() + ", ");
				}
				else
				{
					sbUpdate.Append("pair_gen_used = NULL, ");
				}
				if ( locationID != -1)
				{
					sbUpdate.Append("location_id = " +  locationID.ToString().Trim() + ", ");
				}
				else
				{
					sbUpdate.Append("location_id = NULL, ");
				}

				if ( manualCreated != -1)
				{
					sbUpdate.Append("manual_created = " +  manualCreated.ToString().Trim() + ", ");
				}
				else
				{
					sbUpdate.Append("manual_created = NULL, ");
				}

				if ( isWrkHrsCount != -1)
				{
					sbUpdate.Append("is_wrk_hrs = " +  isWrkHrsCount.ToString().Trim() + ", ");
				}
				else
				{
					sbUpdate.Append("is_wrk_hrs = NULL, ");
				}

				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = GETDATE() ");
				sbUpdate.Append("WHERE pass_id = '" + passID.ToString().Trim() + "'");

				SqlCommand cmd = new SqlCommand( sbUpdate.ToString(), conn, sqlTrans );
				int res = cmd.ExecuteNonQuery();
				if(res > 0)
				{
					isUpdated = true;
				}

				sqlTrans.Commit();				
			}
			catch(SqlException sqlEx)
			{
				sqlTrans.Rollback("UPDATE");
				if (sqlEx.Number == 2627)
				{
					throw new Exception(sqlEx.Number.ToString());
				}
				else
				{
					throw new Exception(sqlEx.Message);
				}
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback("UPDATE");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}
        //Tamara 22.11.2018. da ostavi 1 za pair_gen_used kod onih koji su kontrola pristupa da ne bi dalje obradjivao
		public bool update(int employeeID, string date, bool doCommit)
		{
			bool isUpdated = false;
			SqlTransaction sqlTrans = null;

			if (doCommit)
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
				sbUpdate.Append("UPDATE passes SET ");
				sbUpdate.Append("pair_gen_used = '" + ((int) Constants.PairGenUsed.Unused) + "', ");

				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = GETDATE() ");
				sbUpdate.Append("WHERE employee_id = '" + employeeID.ToString().Trim() + "' ");
				sbUpdate.Append("AND convert(varchar(20), event_time, 101) = '" + date.Trim() + "' AND is_wrk_hrs=1");

				SqlCommand cmd = new SqlCommand( sbUpdate.ToString(), conn, sqlTrans);
				int res = cmd.ExecuteNonQuery();
				if(res >= 0)
				{
					isUpdated = true;
				}

				if (doCommit)
				{
					sqlTrans.Commit();
				}				
			}
			catch(SqlException sqlEx)
			{
				isUpdated = false;
				if (doCommit)
				{
					sqlTrans.Rollback("UPDATE");
				}
				else
				{
					sqlTrans.Rollback();
				}
				if(sqlEx.Number == 2627)
				{
					DataProcessingException procEx = new DataProcessingException(sqlEx.Message, 14);
					throw procEx;
				}
				else
				{
					DataProcessingException procEx = new DataProcessingException(sqlEx.Message, 11);
					throw procEx;
				}
			}
			catch(Exception ex)
			{
				if (doCommit)
				{
					sqlTrans.Rollback("UPDATE");
				}
				else
				{
					sqlTrans.Rollback();
				}
				DataProcessingException procEx = new DataProcessingException(ex.Message, 11);
				throw procEx;
			}

			return isUpdated;
		}

        public void UnlockPasses(DateTime startDate, DateTime  endDate, bool doCommit)
        {            
            SqlTransaction sqlTrans = null;

            if (doCommit)
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
                sbUpdate.Append("UPDATE passes SET ");
                sbUpdate.Append("pair_gen_used = '" + ((int)Constants.PairGenUsed.Unused) + "', ");

                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE  ");
                sbUpdate.Append("convert(varchar(20), event_time, 101) >= '" + startDate.ToString("yyyy-MM-dd").Trim() + "'");
                sbUpdate.Append("AND convert(varchar(20), event_time, 101) <= '" + endDate.ToString("yyyy-MM-dd").Trim() + "'");
                sbUpdate.Append("AND pair_gen_used = '" + ((int)Constants.PairGenUsed.Obsolete) + "' ");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                
                if (doCommit)
                {
                    sqlTrans.Commit();
                }
            }
            catch (SqlException sqlEx)
            {                
                if (doCommit)
                {
                    sqlTrans.Rollback("UPDATE");
                }
                else
                {
                    sqlTrans.Rollback();
                }
                if (sqlEx.Number == 2627)
                {
                    DataProcessingException procEx = new DataProcessingException(sqlEx.Message, 14);
                    throw procEx;
                }
                else
                {
                    DataProcessingException procEx = new DataProcessingException(sqlEx.Message, 11);
                    throw procEx;
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback("UPDATE");
                }
                else
                {
                    sqlTrans.Rollback();
                }
                DataProcessingException procEx = new DataProcessingException(ex.Message, 11);
                throw procEx;
            }            
        }

		/// <summary>
		/// Update passes table
		/// </summary>
		/// <param name="passTO">pass data</param>
		/// <param name="doCommit">if value is true - use new transaction, false - 
		/// transaction started elsewhere</param>
		/// <returns></returns>
		public bool update(PassTO passTO, bool doCommit)
		{
			bool isUpdated = false;
			SqlTransaction sqlTrans = null;

			if (doCommit)
			{
				sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");
			}
			else
			{
				sqlTrans = this.SqlTrans;
			}

			try
			{
                StringBuilder sbSelect = new StringBuilder();
                sbSelect.Append("SELECT employee_id, convert(varchar(10), event_time, 111) currentDate ");
                sbSelect.Append("FROM passes ");
                sbSelect.Append("WHERE ");
                sbSelect.Append("pass_id = " + Int32.Parse(passTO.PassID.ToString().Trim()));

                SqlCommand cmdS = new SqlCommand(sbSelect.ToString(), conn, sqlTrans);
                SqlDataAdapter sqlDA = new SqlDataAdapter(cmdS);
                DataSet dataSetI = new DataSet();
                int employeeID = 0;
                string currentDate = "";

                sqlDA.Fill(dataSetI, "PasesInfo");
                DataTable tableI = dataSetI.Tables["PasesInfo"];

                if (tableI.Rows.Count > 0)
                {
                    employeeID = Int32.Parse(tableI.Rows[0]["employee_id"].ToString().Trim());
                    currentDate = tableI.Rows[0]["currentDate"].ToString().Trim();
                }

                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE passes SET ");

                if (passTO.EmployeeID != -1)
                {
                    sbUpdate.Append("employee_id = " + passTO.EmployeeID.ToString().Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("employee_id = NULL, ");
                }

                if (!passTO.Direction.Trim().Equals(""))
                {
                    sbUpdate.Append("direction = N'" + passTO.Direction.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("direction = NULL, ");
                }

                if (!passTO.EventTime.Equals(new DateTime()))
                {
                    sbUpdate.Append("event_time = '" + passTO.EventTime.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbUpdate.Append("event_time = NULL, ");
                }
                if (passTO.PassTypeID != -1)
                {
                    sbUpdate.Append("pass_type_id = " + passTO.PassTypeID.ToString().Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("pass_type_id = NULL, ");
                }
                if (passTO.PairGenUsed != -1)
                {
                    sbUpdate.Append("pair_gen_used = " + passTO.PairGenUsed.ToString().Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("pair_gen_used = NULL, ");
                }
                if (passTO.LocationID != -1)
                {
                    sbUpdate.Append("location_id = " + passTO.LocationID.ToString().Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("location_id = NULL, ");
                }

                if (passTO.ManualyCreated != -1)
                {
                    sbUpdate.Append("manual_created = " + passTO.ManualyCreated.ToString().Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("manual_created = NULL, ");
                }

                if (passTO.IsWrkHrsCount != -1)
                {
                    sbUpdate.Append("is_wrk_hrs = " + passTO.IsWrkHrsCount.ToString().Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("is_wrk_hrs = NULL, ");
                }

                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE pass_id = '" + passTO.PassID.ToString().Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                if (isUpdated && !passTO.EventTime.Date.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture).Equals(currentDate))
                {
                    sbSelect.Length = 0;
                    sbSelect.Append("SELECT COUNT(*) count ");
                    sbSelect.Append("FROM passes ");
                    sbSelect.Append("WHERE ");
                    sbSelect.Append("employee_id = " + employeeID);
                    sbSelect.Append(" AND convert(datetime, convert(varchar(10), event_time, 111), 111) = convert(datetime,'" + currentDate + "', 111)");

                    cmdS.CommandText = sbSelect.ToString();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmdS);
                    DataSet dataSet = new DataSet();
                    int count = 0;

                    sqlDataAdapter.Fill(dataSet, "PasesCount");
                    DataTable table = dataSet.Tables["PasesCount"];

                    if (table.Rows.Count > 0)
                    {
                        count = Int32.Parse(table.Rows[0]["count"].ToString().Trim());
                    }

                    if (count > 0)
                    {
                        StringBuilder sbUpdate1 = new StringBuilder();
                        sbUpdate1.Append("UPDATE passes SET ");
                        sbUpdate1.Append("pair_gen_used = 0, ");
                        sbUpdate1.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                        sbUpdate1.Append("modified_time = GETDATE() ");
                        sbUpdate1.Append("WHERE ");
                        sbUpdate1.Append("employee_id = " + employeeID);
                        sbUpdate1.Append(" AND convert(datetime, convert(varchar(10), event_time, 111), 111) = convert(datetime,'" + currentDate + "', 111)");

                        SqlCommand cmdU = new SqlCommand(sbUpdate1.ToString(), conn, sqlTrans);
                        int resU = cmdU.ExecuteNonQuery();
                        if (resU <= 0)
                        {
                            isUpdated = false;
                        }
                    } //if (count > 0)
                }			

				if (doCommit)
				{
                    if (isUpdated)
                    {
                        sqlTrans.Commit();
                    }
                    else
                    {
                        sqlTrans.Rollback();
                    }
				}				
			}
			catch(SqlException sqlEx)
			{
				if (doCommit)
				{
					sqlTrans.Rollback("UPDATE");
				}
				else
				{
					sqlTrans.Rollback();
				}				
				if(sqlEx.Number == 2627)
				{
					DataProcessingException procEx = new DataProcessingException(sqlEx.Message + passTO.ToString(), 14);
					throw procEx;
				}
				else
				{
					DataProcessingException procEx = new DataProcessingException(sqlEx.Message + passTO.ToString(), 11);
					throw procEx;
				}
			}
			catch(Exception ex)
			{
				if (doCommit)
				{
					sqlTrans.Rollback("UPDATE");
				}
				else
				{
					sqlTrans.Rollback();
				}
				DataProcessingException procEx = new DataProcessingException(ex.Message, 11);
				throw procEx;
			}

			return isUpdated;
		}

		public List<PassTO> getPasses(PassTO passTO)
		{
			DataSet dataSet = new DataSet();
			PassTO pass = new PassTO();
			List<PassTO> passesList = new List<PassTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT * FROM passes ");

                if ((passTO.PassID != -1) || (passTO.EmployeeID != -1) || (!passTO.Direction.Trim().Equals("")) ||
                    (!passTO.EventTime.Equals(new DateTime())) || (passTO.PassTypeID != -1) ||
                    (passTO.PairGenUsed != -1) || (passTO.LocationID != -1) || (passTO.ManualyCreated != -1) ||
                    (passTO.IsWrkHrsCount != -1))
				{
					sb.Append(" WHERE ");

                    if (passTO.PassID != -1)
					{
						//sb.Append(" UPPER(pass_id) LIKE '" + passID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" pass_id = '" + passTO.PassID.ToString().Trim() + "' AND");
					}
                    if (passTO.EmployeeID != -1)
					{
						//sb.Append(" UPPER(employee_id) LIKE '" + employeeID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" employee_id = '" + passTO.EmployeeID.ToString().Trim() + "' AND");
					}
                    if (!passTO.Direction.Trim().Equals(""))
					{
                        sb.Append(" UPPER(direction) LIKE N'%" + passTO.Direction.Trim().ToUpper() + "%' AND");
					}
                    if (!passTO.EventTime.Equals(new DateTime()))
					{
                        sb.Append(" event_time = '" + passTO.EventTime.ToString(dateTimeformat).Trim() + "' AND");
					}
                    if (passTO.PassTypeID != -1)
					{
						//sb.Append(" UPPER(pass_type_id) LIKE '" + passTypeID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" pass_type_id = '" + passTO.PassTypeID.ToString().Trim() + "' AND");
					}
                    if (passTO.PairGenUsed != -1)
					{
						//sb.Append(" UPPER(pair_gen_used) LIKE '" + pairGenUsed.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" pair_gen_used = '" + passTO.PairGenUsed.ToString().Trim() + "' AND");
					}
                    if (passTO.LocationID != -1)
					{
						//sb.Append(" UPPER(location_id) LIKE '" + locationID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" location_id = '" + passTO.LocationID.ToString().Trim() + "' AND");
					}
                    if (passTO.ManualyCreated != -1)
					{
						//sb.Append(" UPPER(manual_created) LIKE '" + manualCreated.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" manual_created = '" + passTO.ManualyCreated.ToString().Trim() + "' AND");
					}
                    if (passTO.IsWrkHrsCount != -1)
					{
						//sb.Append(" UPPER(is_wrk_hrs) LIKE '" + isWrkHrsCount.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" is_wrk_hrs = '" + passTO.IsWrkHrsCount.ToString().Trim() + "' AND");
					}
					
					select = sb.ToString(0, sb.ToString().Length - 3);
				}
				else
				{
					select = sb.ToString();
				}

				SqlCommand cmd = new SqlCommand(select, conn );
                cmd.CommandTimeout = Constants.commandTimeout;

				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Pass");
				DataTable table = dataSet.Tables["Pass"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						pass = new PassTO();
						pass.PassID = Int32.Parse(row["pass_id"].ToString().Trim());
						
						if (row["employee_id"] != DBNull.Value)
						{
							pass.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
						}
						if (row["direction"] != DBNull.Value)
						{
							pass.Direction = row["direction"].ToString().Trim();
						}
						if (row["event_time"] != DBNull.Value)
						{
							pass.EventTime = (DateTime) row["event_time"];
						}
						if (row["pass_type_id"] != DBNull.Value)
						{
							pass.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
						}
						if (row["pair_gen_used"] != DBNull.Value)
						{
							pass.PairGenUsed = Int32.Parse(row["pair_gen_used"].ToString().Trim());
						}
						if (row["location_id"] != DBNull.Value)
						{
							pass.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
						}
						if (row["manual_created"] != DBNull.Value)
						{
							pass.ManualyCreated = Int32.Parse(row["manual_created"].ToString().Trim());
						}
						if (row["is_wrk_hrs"] != DBNull.Value)
						{
							pass.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs"].ToString().Trim());
						}
                        if (row["created_by"] != DBNull.Value)
                        {
                            pass.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pass.CreatedTime = (DateTime)row["created_time"];
                        }

						passesList.Add(pass);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception("Exception: " + ex.Message);
			}

			return passesList;
		}

        public List<PassTO> getPassForPerm(int employeeID, string direction, DateTime eventTime, int isWrkHrsCount)
        {
            DataSet dataSet = new DataSet();
            PassTO pass = new PassTO();
            List<PassTO> passesList = new List<PassTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM passes ");

                if ((employeeID != -1) || (!direction.Trim().Equals("")) ||
                    (!eventTime.Equals(new DateTime(0))) ||
                    (isWrkHrsCount != -1))
                {
                    sb.Append(" WHERE ");

                    if (employeeID != -1)
                    {
                        //sb.Append(" UPPER(employee_id) LIKE '" + employeeID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" employee_id = '" + employeeID.ToString().Trim().ToUpper() + "' AND");
                    }
                    if (!direction.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(direction) = '" + direction.Trim().ToUpper() + "' AND");
                    }
                    if (!eventTime.Equals(new DateTime(0)))
                    {
                        sb.Append(" event_time = '" + eventTime.ToString(dateTimeformat).Trim() + "' AND");
                    }
                    if (isWrkHrsCount != -1)
                    {
                        //sb.Append(" UPPER(is_wrk_hrs) LIKE '" + isWrkHrsCount.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" is_wrk_hrs = '" + isWrkHrsCount.ToString().Trim().ToUpper() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                SqlCommand cmd = new SqlCommand(select, conn);
                cmd.CommandTimeout = Constants.commandTimeout;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Pass");
                DataTable table = dataSet.Tables["Pass"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pass = new PassTO();
                        pass.PassID = Int32.Parse(row["pass_id"].ToString().Trim());

                        if (row["employee_id"] != DBNull.Value)
                        {
                            pass.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["direction"] != DBNull.Value)
                        {
                            pass.Direction = row["direction"].ToString().Trim();
                        }
                        if (row["event_time"] != DBNull.Value)
                        {
                            pass.EventTime = (DateTime)row["event_time"];
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pass.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["pair_gen_used"] != DBNull.Value)
                        {
                            pass.PairGenUsed = Int32.Parse(row["pair_gen_used"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pass.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pass.ManualyCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs"] != DBNull.Value)
                        {
                            pass.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs"].ToString().Trim());
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            pass.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pass.CreatedTime = (DateTime)row["created_time"];
                        }

                        passesList.Add(pass);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return passesList;
        }

		public List<PassTO> getPassesInterval(PassTO passTO, DateTime fromTime, DateTime toTime, string wUnits, DateTime advFromTime, DateTime advToTime)
		{
			DataSet dataSet = new DataSet();
			PassTO pass = new PassTO();
			List<PassTO> passesList = new List<PassTO>();
			string select = "";

			try
			{
                TimeSpan fromSpan = new TimeSpan(0, 0, 0);
                TimeSpan toSpan = new TimeSpan(23, 59, 0);

				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT ps.*, empl.last_name, empl.first_name, pt.description, loc.name, wu.name as wuName ");
                sb.Append("FROM passes ps, employees empl, pass_types pt, locations loc, working_units wu ");
				sb.Append("WHERE ");

                if ((passTO.EmployeeID != -1) || (!passTO.Direction.Trim().Equals("")) ||
					(!fromTime.Equals(new DateTime())) || (!toTime.Equals(new DateTime())) ||
                    (passTO.PassTypeID != -1) || (passTO.LocationID != -1) || (passTO.GateID != -1)
                    || (!advFromTime.TimeOfDay.Equals(fromSpan) || !advToTime.TimeOfDay.Equals(toSpan)))
				{
                    if (passTO.EmployeeID != -1)
					{
						//sb.Append(" UPPER(ps.employee_id) LIKE '" + employeeID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" ps.employee_id = '" + passTO.EmployeeID.ToString().Trim() + "' AND");
					}
                    if (!passTO.Direction.Trim().Equals(""))
					{
                        sb.Append(" RTRIM(LTRIM(UPPER(ps.direction))) LIKE N'%" + passTO.Direction.Trim().ToUpper() + "%' AND");
					}
					if (!fromTime.Equals(new DateTime()) && !toTime.Equals(new DateTime()))
					{
						//sb.Append(" CONVERT(datetime,ps.event_time,101) >= '" + fromTime.ToString(dateTimeformat).Trim() + "' AND CONVERT(datetime,ps.event_time,101) <= '" + toTime.AddDays(1).ToString(dateTimeformat).Trim() + "' AND");
                        sb.Append(" ps.event_time >= CONVERT(datetime,'" + fromTime.ToString("yyyy-MM-dd") + "', 111) AND ");
                        sb.Append(" ps.event_time < CONVERT(datetime,'" + toTime.AddDays(1).ToString("yyyy-MM-dd") + "', 111) AND ");
					}
                    if (passTO.PassTypeID != -1)
					{
						//sb.Append(" UPPER(ps.pass_type_id) LIKE '" + passTypeID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" ps.pass_type_id = '" + passTO.PassTypeID.ToString().Trim() + "' AND");
					}
                    if (passTO.LocationID != -1)
					{
						//sb.Append(" UPPER(ps.location_id) LIKE '" + locationID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" ps.location_id = '" + passTO.LocationID.ToString().Trim().ToUpper() + "' AND");
					}
                    if (passTO.GateID != -1)
                    {
                        //sb.Append(" UPPER(ps.location_id) LIKE '" + locationID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" ps.gate_id = '" + passTO.GateID.ToString().Trim().ToUpper() + "' AND");
                    }

                    if (!advFromTime.TimeOfDay.Equals(fromSpan) || !advToTime.TimeOfDay.Equals(toSpan))
                    {
                        if (!advFromTime.TimeOfDay.Equals(fromSpan))
                            sb.Append(" CONVERT(datetime, CONVERT(varchar(10), ps.event_time, 108), 108) >= CONVERT(datetime,'" + advFromTime.ToString("HH:mm:ss") + "', 108) AND ");
                        if (!advToTime.TimeOfDay.Equals(toSpan))
                            sb.Append(" CONVERT(datetime, CONVERT(varchar(10), ps.event_time, 108), 108) < CONVERT(datetime,'" + advToTime.AddMinutes(1).ToString("HH:mm:ss") + "', 108) AND ");
                    }
					
					//select = sb.ToString(0, sb.ToString().Length - 3);
				}
				
				select = sb.ToString();
				
				select += " empl.employee_id = ps.employee_id AND pt.pass_type_id = ps.pass_type_id AND "
					+ "loc.location_id = ps.location_id";
				if (!wUnits.Equals(""))
				{
					select += " AND empl.working_unit_id IN (" + wUnits + ")";
				}
                select += " AND empl.working_unit_id = wu.working_unit_id";
                select += " ORDER BY ps.employee_id, ps.event_time";

				SqlCommand cmd = new SqlCommand(select, conn);
                cmd.CommandTimeout = Constants.commandTimeout;

				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Pass");
				DataTable table = dataSet.Tables["Pass"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						pass = new PassTO();
						pass.PassID = Int32.Parse(row["pass_id"].ToString().Trim());
						
						if (row["employee_id"] != DBNull.Value)
						{
							pass.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
						}
						if (row["direction"] != DBNull.Value)
						{
							pass.Direction = row["direction"].ToString().Trim();
						}
						if (row["event_time"] != DBNull.Value)
						{
							pass.EventTime = (DateTime) row["event_time"];
						}
						if (row["pass_type_id"] != DBNull.Value)
						{
							pass.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
						}
						if (row["pair_gen_used"] != DBNull.Value)
						{
							pass.PairGenUsed = Int32.Parse(row["pair_gen_used"].ToString().Trim());
						}
						if (row["location_id"] != DBNull.Value)
						{
							pass.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
						}
						if (row["manual_created"] != DBNull.Value)
						{
							pass.ManualyCreated = Int32.Parse(row["manual_created"].ToString().Trim());
						}
						if (row["is_wrk_hrs"] != DBNull.Value)
						{
							pass.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs"].ToString().Trim());
						}
                        if (row["created_by"] != DBNull.Value)
                        {
                            pass.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pass.CreatedTime = (DateTime)row["created_time"];
                        }
						if (row["last_name"] != DBNull.Value)
						{
							pass.EmployeeName = row["last_name"].ToString().Trim();
						}
						if (row["first_name"] != DBNull.Value)
						{
							pass.EmployeeName += " " + row["first_name"].ToString().Trim();
						}
						if (row["description"] != DBNull.Value)
						{
							pass.PassType = row["description"].ToString().Trim();
						}
						if (row["name"] != DBNull.Value)
						{
							pass.LocationName = row["name"].ToString().Trim();
						}
                        if (row["gate_id"] != DBNull.Value)
                        {
                            pass.GateID = Int32.Parse(row["gate_id"].ToString().Trim());
                        }
                        if (row["gate_id"] != DBNull.Value)
                        {
                            pass.GateID = Int32.Parse(row["gate_id"].ToString().Trim());
                        }
                        if (row["wuName"] != DBNull.Value)
                        {
                            pass.WUName = row["wuName"].ToString().Trim();
                        }
						passesList.Add(pass);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception("Exception: " + ex.Message);
			}

			return passesList;
		}

        public Dictionary<int, Dictionary<DateTime, List<PassTO>>> getPassesInterval(DateTime month, string emplIDs)
        {
            DataSet dataSet = new DataSet();
            PassTO pass = new PassTO();
            Dictionary<int, Dictionary<DateTime, List<PassTO>>> passes = new Dictionary<int, Dictionary<DateTime, List<PassTO>>>();
            string select = "";
            CultureInfo ci = CultureInfo.InvariantCulture;
            try
            {
                DateTime fromDate = new DateTime(month.Year, month.Month, 1, 0, 0, 0); // first day of month
                DateTime toDate = fromDate.AddMonths(1); // first day of next month

                select = "SELECT * FROM passes WHERE event_time >= '" + fromDate.ToString(dateTimeformat) + "' AND event_time < '" + toDate.ToString(dateTimeformat) + "'";

                if (emplIDs.Length > 0)
                    select += " AND employee_id IN (" + emplIDs.Trim() + ")";

                select += " ORDER BY employee_id, event_time";

                SqlCommand cmd = new SqlCommand(select, conn);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Pass");

                DataTable table = dataSet.Tables["Pass"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pass = new PassTO();
                        pass.PassID = Int32.Parse(row["pass_id"].ToString().Trim());

                        if (row["employee_id"] != DBNull.Value)
                        {
                            pass.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["direction"] != DBNull.Value)
                        {
                            pass.Direction = row["direction"].ToString().Trim();
                        }
                        if (row["event_time"] != DBNull.Value)
                        {
                            pass.EventTime = DateTime.Parse(row["event_time"].ToString());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pass.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["pair_gen_used"] != DBNull.Value)
                        {
                            pass.PairGenUsed = Int32.Parse(row["pair_gen_used"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pass.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pass.ManualyCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs"] != DBNull.Value)
                        {
                            pass.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs"].ToString().Trim());
                        }
                        if (row["gate_id"] != DBNull.Value)
                        {
                            pass.GateID = Int32.Parse(row["gate_id"].ToString().Trim());
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            pass.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pass.CreatedTime = DateTime.Parse(row["created_time"].ToString());
                        }                        
                        
                        if (!passes.ContainsKey(pass.EmployeeID))
                            passes.Add(pass.EmployeeID, new Dictionary<DateTime, List<PassTO>>());

                        if (!passes[pass.EmployeeID].ContainsKey(pass.EventTime.Date))
                            passes[pass.EmployeeID].Add(pass.EventTime.Date, new List<PassTO>());

                        passes[pass.EmployeeID][pass.EventTime.Date].Add(pass);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return passes;
        }

        public List<PassTO> getPassesIntervalForZINReport(DateTime fromTime, DateTime toTime, string employees)
        {
            DataSet dataSet = new DataSet();
            PassTO pass = new PassTO();
            List<PassTO> passesList = new List<PassTO>();
            string select = "";

            try
            {
                TimeSpan fromSpan = new TimeSpan(0, 0, 0);
                TimeSpan toSpan = new TimeSpan(23, 59, 0);

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ps.*, empl.last_name, empl.first_name, pt.description, loc.name, wu.name as wuName ");
                sb.Append("FROM passes ps, employees empl, pass_types pt, locations loc, working_units wu ");
                sb.Append("WHERE ");

                if ((!employees.Equals("")) || 
                    (!fromTime.Equals(new DateTime())) || (!toTime.Equals(new DateTime())))
                {
                    if (!employees.Equals(""))
                    {
                        //sb.Append(" UPPER(ps.employee_id) LIKE '" + employeeID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" ps.employee_id IN (" + employees + " ) AND");
                    }
                   
                    if (!fromTime.Equals(new DateTime(0)) && !toTime.Equals(new DateTime(0)))
                    {
                        //sb.Append(" CONVERT(datetime,ps.event_time,101) >= '" + fromTime.ToString(dateTimeformat).Trim() + "' AND CONVERT(datetime,ps.event_time,101) <= '" + toTime.AddDays(1).ToString(dateTimeformat).Trim() + "' AND");
                        sb.Append(" ps.event_time >= CONVERT(datetime,'" + fromTime.ToString("yyyy-MM-dd") + "', 111) AND ");
                        sb.Append(" ps.event_time < CONVERT(datetime,'" + toTime.AddDays(1).ToString("yyyy-MM-dd") + "', 111) AND ");
                    }                    
                }

                select = sb.ToString();

                select += " empl.employee_id = ps.employee_id AND pt.pass_type_id = ps.pass_type_id AND "
                    + "loc.location_id = ps.location_id";                
                select += " AND empl.working_unit_id = wu.working_unit_id";
                select += " ORDER BY ps.employee_id, ps.event_time";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Pass");
                DataTable table = dataSet.Tables["Pass"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pass = new PassTO();
                        pass.PassID = Int32.Parse(row["pass_id"].ToString().Trim());

                        if (row["employee_id"] != DBNull.Value)
                        {
                            pass.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["direction"] != DBNull.Value)
                        {
                            pass.Direction = row["direction"].ToString().Trim();
                        }
                        if (row["event_time"] != DBNull.Value)
                        {
                            pass.EventTime = (DateTime)row["event_time"];
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pass.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["pair_gen_used"] != DBNull.Value)
                        {
                            pass.PairGenUsed = Int32.Parse(row["pair_gen_used"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pass.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pass.ManualyCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs"] != DBNull.Value)
                        {
                            pass.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs"].ToString().Trim());
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            pass.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pass.CreatedTime = (DateTime)row["created_time"];
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            pass.EmployeeName = row["last_name"].ToString().Trim();
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            pass.EmployeeName += " " + row["first_name"].ToString().Trim();
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            pass.PassType = row["description"].ToString().Trim();
                        }
                        if (row["name"] != DBNull.Value)
                        {
                            pass.LocationName = row["name"].ToString().Trim();
                        }
                        if (row["gate_id"] != DBNull.Value)
                        {
                            pass.GateID = Int32.Parse(row["gate_id"].ToString().Trim());
                        }
                        if (row["gate_id"] != DBNull.Value)
                        {
                            pass.GateID = Int32.Parse(row["gate_id"].ToString().Trim());
                        }
                        if (row["wuName"] != DBNull.Value)
                        {
                            pass.WUName = row["wuName"].ToString().Trim();
                        }
                        passesList.Add(pass);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return passesList;
        }


		public int getPassesIntervalCount(PassTO passTO, DateTime fromTime, DateTime toTime, string wUnits, DateTime advFromTime, DateTime advToTime)
		{
			DataSet dataSet = new DataSet();
			
			int count = 0;
			string select = "";

			try
			{
                TimeSpan fromSpan = new TimeSpan(0, 0, 0);
                TimeSpan toSpan = new TimeSpan(23, 59, 0);

				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT count(ps.pass_id) count_pass ");
				sb.Append("FROM passes ps, employees empl ");
				sb.Append("WHERE ");

                if ((passTO.EmployeeID != -1) || (!passTO.Direction.Trim().Equals("")) ||
					(!fromTime.Equals(new DateTime())) || (!toTime.Equals(new DateTime())) ||
                    (passTO.PassTypeID != -1) || (passTO.LocationID != -1) || (passTO.GateID != -1)
                    || (!advFromTime.TimeOfDay.Equals(fromSpan) || !advToTime.TimeOfDay.Equals(toSpan)))
				{
                    if (passTO.EmployeeID != -1)
					{
						//sb.Append("UPPER(ps.employee_id) LIKE '" + employeeID.ToString().Trim().ToUpper() + "' AND ");
                        sb.Append("ps.employee_id = '" + passTO.EmployeeID.ToString().Trim() + "' AND " );
					}
                    if (!passTO.Direction.Trim().Equals(""))
					{
                        sb.Append("RTRIM(LTRIM(UPPER(ps.direction))) LIKE N'%" + passTO.Direction.Trim().ToUpper() + "%' AND ");
					}
					if (!fromTime.Equals(new DateTime()) && !toTime.Equals(new DateTime()))
					{
						//sb.Append("CONVERT(datetime, ps.event_time, 101) >= '" + fromTime.ToString(dateTimeformat).Trim() + "' AND CONVERT(datetime,ps.event_time,101) <= '" + toTime.AddDays(1).ToString(dateTimeformat).Trim() + "' AND ");
                        sb.Append("ps.event_time >= convert(datetime,'" + fromTime.ToString("yyyy-MM-dd") + "', 111) AND ");
                        sb.Append("ps.event_time < convert(datetime,'" + toTime.AddDays(1).ToString("yyyy-MM-dd") + "', 111) AND ");
					}
                    if (passTO.PassTypeID != -1)
					{
						//sb.Append("UPPER(ps.pass_type_id) LIKE '" + passTypeID.ToString().Trim().ToUpper() + "' AND ");
                        sb.Append("ps.pass_type_id = '" + passTO.PassTypeID.ToString().Trim() + "' AND ");
					}
                    if (passTO.LocationID != -1)
					{
						//sb.Append("UPPER(ps.location_id) LIKE '" + locationID.ToString().Trim().ToUpper() + "' AND ");
                        sb.Append("ps.location_id = '" + passTO.LocationID.ToString().Trim() + "' AND ");
					}
                    if (passTO.GateID != -1)
                    {
                        //sb.Append("UPPER(ps.location_id) LIKE '" + locationID.ToString().Trim().ToUpper() + "' AND ");
                        sb.Append("ps.gate_id = '" + passTO.GateID.ToString().Trim() + "' AND ");
                    }

                    if (!advFromTime.TimeOfDay.Equals(fromSpan) || !advToTime.TimeOfDay.Equals(toSpan))
                    {
                        if (!advFromTime.TimeOfDay.Equals(fromSpan))
                            sb.Append(" CONVERT(datetime, CONVERT(varchar(10), ps.event_time, 108), 108) >= CONVERT(datetime,'" + advFromTime.ToString("HH:mm:ss") + "', 108) AND ");
                        if (!advToTime.TimeOfDay.Equals(toSpan))
                            sb.Append(" CONVERT(datetime, CONVERT(varchar(10), ps.event_time, 108), 108) < CONVERT(datetime,'" + advToTime.AddMinutes(1).ToString("HH:mm:ss") + "', 108) AND ");
                    }
				}
				
				select = sb.ToString();

                select += "ps.employee_id = empl.employee_id";
                if(wUnits.Length>0)
                select += " AND empl.working_unit_id IN (" + wUnits + ")";

				SqlCommand cmd = new SqlCommand(select, conn);
                cmd.CommandTimeout = Constants.commandTimeout;

				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Pass");
				DataTable table = dataSet.Tables["Pass"];

				if (table.Rows.Count > 0)
				{
					count = Int32.Parse(table.Rows[0]["count_pass"].ToString().Trim());
				}
			}
			catch(Exception ex)
			{
				throw new Exception("Exception: " + ex.Message);
			}

			return count;
		}

        public int getPassesIntervalCountForZINReport(DateTime fromTime, DateTime toTime, string employees)
        {
            DataSet dataSet = new DataSet();

            int count = 0;
            string select = "";

            try
            {
                TimeSpan fromSpan = new TimeSpan(0, 0, 0);
                TimeSpan toSpan = new TimeSpan(23, 59, 0);

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT count(ps.pass_id) count_pass ");
                sb.Append("FROM passes ps, employees empl ");
                sb.Append("WHERE ");

                if ((!employees.Equals("")) || (!fromTime.Equals(new DateTime(0)))
                    || (!toTime.Equals(new DateTime(0))))
                {                    

                    if (!employees.Equals(""))
                    {
                        //sb.Append("UPPER(ps.employee_id) LIKE '" + employeeID.ToString().Trim().ToUpper() + "' AND ");
                        sb.Append("ps.employee_id IN (" + employees + " ) AND ");
                    }
                    
                    if (!fromTime.Equals(new DateTime(0)) && !toTime.Equals(new DateTime(0)))
                    {
                        //sb.Append("CONVERT(datetime, ps.event_time, 101) >= '" + fromTime.ToString(dateTimeformat).Trim() + "' AND CONVERT(datetime,ps.event_time,101) <= '" + toTime.AddDays(1).ToString(dateTimeformat).Trim() + "' AND ");
                        sb.Append("ps.event_time >= convert(datetime,'" + fromTime.ToString("yyyy-MM-dd") + "', 111) AND ");
                        sb.Append("ps.event_time < convert(datetime,'" + toTime.AddDays(1).ToString("yyyy-MM-dd") + "', 111) AND ");
                    }                    
                }

                select = sb.ToString();

                select += "ps.employee_id = empl.employee_id ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Pass");
                DataTable table = dataSet.Tables["Pass"];

                if (table.Rows.Count > 0)
                {
                    count = Int32.Parse(table.Rows[0]["count_pass"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return count;
        }

        public List<PassTO> getPassesForSnapshots(string passID)
        {
            DataSet dataSet = new DataSet();
            PassTO pass = new PassTO();
            List<PassTO> passesList = new List<PassTO>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ps.pass_id, ps.employee_id, ps.direction, ps.event_time, ps.is_wrk_hrs, ps.created_by, ps.created_time, empl.last_name, empl.first_name, pt.description, loc.name, wu.name AS WUName ");
                sb.Append("FROM passes ps, employees empl, pass_types pt, locations loc, working_units wu ");
                sb.Append("WHERE ");

                if (!passID.Equals(""))
                {
                    sb.Append("ps.pass_id IN (" + passID.ToString().Trim() + ") AND");
                }

                sb.Append(" empl.employee_id = ps.employee_id AND pt.pass_type_id = ps.pass_type_id AND");
                sb.Append(" loc.location_id = ps.location_id AND wu.working_unit_id = empl.working_unit_id");
                sb.Append(" ORDER BY ps.event_time");

                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                cmd.CommandTimeout = Constants.commandTimeout;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Pass");
                DataTable table = dataSet.Tables["Pass"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pass = new PassTO();
                        pass.PassID = Int32.Parse(row["pass_id"].ToString().Trim());

                        if (row["employee_id"] != DBNull.Value)
                        {
                            pass.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["direction"] != DBNull.Value)
                        {
                            pass.Direction = row["direction"].ToString().Trim();
                        }
                        if (row["event_time"] != DBNull.Value)
                        {
                            pass.EventTime = (DateTime)row["event_time"];
                        }
                        if (row["is_wrk_hrs"] != DBNull.Value)
                        {
                            pass.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs"].ToString().Trim());
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            pass.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pass.CreatedTime = (DateTime)row["created_time"];
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            pass.EmployeeName = row["last_name"].ToString().Trim();
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            pass.EmployeeName += " " + row["first_name"].ToString().Trim();
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            pass.PassType = row["description"].ToString().Trim();
                        }
                        if (row["name"] != DBNull.Value)
                        {
                            pass.LocationName = row["name"].ToString().Trim();
                        }
                        if (row["WUName"] != DBNull.Value)
                        {
                            pass.WUName = row["WUName"].ToString().Trim();
                        }

                        passesList.Add(pass);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return passesList;
        }

		/// <summary>
		/// Return list of Pass Transfere Object that fill given 
		/// criteria
		/// </summary>
		/// <param name="passID">pass ID value</param>
		/// <param name="employeeID">empolyee ID </param>
		/// <param name="direction"></param>
		/// <param name="eventTime"></param>
		/// <param name="passTypeID"></param>
		/// <param name="pairGenUsed"></param>
		/// <param name="locationID"></param>
		/// <param name="manualCreated"></param>
		/// <param name="isWrkHrsCount"></param>
		/// <returns>Pass Transvere Objects list</returns>
		public List<PassTO> getPassesList(PassTO passTO)
		{
			DataSet dataSet = new DataSet();
			PassTO pass = new PassTO();
			List<PassTO> passesList = new List<PassTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();

				sb.Append("SELECT ps.*, empl.last_name, empl.first_name, pt.description, loc.name ");
				sb.Append("FROM passes ps, employees empl, pass_types pt, locations loc ");
				sb.Append("WHERE ");

                if ((passTO.PassID != -1) ||
                    (passTO.EmployeeID != -1) ||
                    (!passTO.Direction.Trim().Equals("")) ||
                    (!passTO.EventTime.Equals(new DateTime())) ||
                    (passTO.PassTypeID != -1) ||
                    (passTO.LocationID != -1) ||
                    (passTO.ManualyCreated != -1) ||
                    (passTO.IsWrkHrsCount != -1) || 
					(passTO.PairGenUsed != -1))
				{
                    if (passTO.PassID != -1)
					{
						//sb.Append(" UPPER(ps.pass_id) LIKE '" + passID.Trim().ToUpper() + "' AND");
                        sb.Append(" ps.pass_id = '" + passTO.PassID.ToString().Trim() + "' AND");
					}
                    if (passTO.EmployeeID != -1)
					{
						//sb.Append(" UPPER(ps.employee_id) LIKE '" + employeeID.Trim().ToUpper() + "' AND");
                        sb.Append(" ps.employee_id = '" + passTO.EmployeeID.ToString().Trim() + "' AND");
					}
                    if (!passTO.Direction.Trim().Equals(""))
					{
                        sb.Append(" UPPER(ps.direction) LIKE N'%" + passTO.Direction.Trim().ToUpper() + "%' AND");
					}
                    if (!passTO.EventTime.Equals(new DateTime()))
					{
                        sb.Append(" UPPER(CONVERT(CHAR(24), ps.event_time, 113)) LIKE '" + passTO.EventTime.ToString(dateTimeformat) + "' AND");
					}
                    if (passTO.PassTypeID != -1)
					{
						//sb.Append(" UPPER(ps.pass_type_id) LIKE '" + passTypeID.Trim().ToUpper() + "' AND");
                        sb.Append(" ps.pass_type_id = '" + passTO.PassTypeID.ToString().Trim() + "' AND");
					}
                    if (passTO.PairGenUsed.ToString().Trim().Equals(""))
					{
						//sb.Append(" UPPER(ps.pair_gen_used) LIKE '" + pairGenUsed.Trim().ToUpper() + "' AND");
                        sb.Append(" ps.pair_gen_used = '" + passTO.PairGenUsed.ToString().Trim() + "' AND");
					}
					if (passTO.LocationID != -1)
					{
						//sb.Append(" UPPER(ps.location_id) LIKE '" + locationID.Trim().ToUpper() + "' AND");
                        sb.Append(" ps.location_id = '" + passTO.LocationID.ToString().Trim() + "' AND");
					}
                    if (passTO.ManualyCreated != -1)
					{
						//sb.Append(" UPPER(ps.manual_created) LIKE '" + manualCreated.Trim().ToUpper() + "' AND");
                        sb.Append(" ps.manual_created = '" + passTO.ManualyCreated.ToString().Trim() + "' AND");
					}
                    if (passTO.IsWrkHrsCount != -1)
					{
						//sb.Append(" UPPER(ps.is_wrk_hrs) LIKE '" + isWrkHrsCount.Trim().ToUpper() + "' AND");
                        sb.Append(" ps.is_wrk_hrs = '" + passTO.IsWrkHrsCount.ToString().Trim().ToUpper() + "' AND");
					}
					
					//select = sb.ToString(0, sb.ToString().Length - 3);
				}
				
				select = sb.ToString();
				
				select += " empl.employee_id = ps.employee_id AND pt.pass_type_id = ps.pass_type_id AND "
					+ "loc.location_id = ps.location_id";

				SqlCommand cmd = new SqlCommand(select, conn );
                cmd.CommandTimeout = Constants.commandTimeout;

				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Pass");
				DataTable table = dataSet.Tables["Pass"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						pass = new PassTO();
						pass.PassID = Int32.Parse(row["pass_id"].ToString().Trim());
						
						if (row["employee_id"] != DBNull.Value)
						{
							pass.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
						}
						if (row["direction"] != DBNull.Value)
						{
							pass.Direction = row["direction"].ToString().Trim();
						}
						if (row["event_time"] != DBNull.Value)
						{
							pass.EventTime = (DateTime) row["event_time"];
						}
						if (row["pass_type_id"] != DBNull.Value)
						{
							pass.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
						}
						if (row["pair_gen_used"] != DBNull.Value)
						{
							pass.PairGenUsed = Int32.Parse(row["pair_gen_used"].ToString().Trim());
						}
						if (row["location_id"] != DBNull.Value)
						{
							pass.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
						}
						if (row["manual_created"] != DBNull.Value)
						{
							pass.ManualyCreated = Int32.Parse(row["manual_created"].ToString().Trim());
						}
						if (row["is_wrk_hrs"] != DBNull.Value)
						{
							pass.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs"].ToString().Trim());
						}
                        if (row["created_by"] != DBNull.Value)
                        {
                            pass.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pass.CreatedTime = (DateTime)row["created_time"];
                        }
						if (row["last_name"] != DBNull.Value)
						{
							pass.EmployeeName = row["last_name"].ToString().Trim();
						}
						if (row["first_name"] != DBNull.Value)
						{
							pass.EmployeeName += " " + row["first_name"].ToString().Trim();
						}
						if (row["description"] != DBNull.Value)
						{
							pass.PassType = row["description"].ToString().Trim();
						}
						if (row["name"] != DBNull.Value)
						{
							pass.LocationName = row["name"].ToString().Trim();
						}

						passesList.Add(pass);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception("Exception: " + ex.Message);
			}

			return passesList;
		}

		/// <summary>
		/// Processing logs and prepere it for populating passes
		/// </summary>
		/// <param name="passTOList"></param>
		/// <param name="logTOList"></param>
		/// <param name="factory"></param>
        public void logToPass(List<PassTO> passTOList, List<LogTO> logTOList, DAOFactory factory)
		{
			DataSet dataSet = new DataSet();
			
			try
			{
				SqlCommand cmdLog = new SqlCommand( "SELECT * FROM log", conn);
				SqlDataAdapter sqlDataAdapterLog = new SqlDataAdapter(cmdLog);

				SqlCommandBuilder builderLog = new SqlCommandBuilder(sqlDataAdapterLog);

				sqlDataAdapterLog.MissingSchemaAction = MissingSchemaAction.AddWithKey;
				sqlDataAdapterLog.Fill(dataSet, "log");

				SqlCommand cmdPass = new SqlCommand( "SELECT * FROM passes", conn);
				SqlDataAdapter sqlDataAdapterPass = new SqlDataAdapter(cmdPass);

				SqlCommandBuilder bulderPass = new SqlCommandBuilder(sqlDataAdapterPass);

				sqlDataAdapterPass.MissingSchemaAction = MissingSchemaAction.AddWithKey;
				sqlDataAdapterPass.Fill(dataSet, "passes");

				DataRow row = null;

				// Update Log table
				System.Console.WriteLine("Start to execute fill Log DataSet ...: " + DateTime.Now);
				System.Console.WriteLine("---------------------");
				foreach (LogTO logTOMember in logTOList)
				{
					row = dataSet.Tables["log"].Rows.Find(logTOMember.LogID);

					row["pass_gen_used"] = logTOMember.PassGenUsed;
					row["modified_by"] = DAOController.GetLogInUser().Trim();
					row["modified_time"] = DateTime.Now;
				}
				System.Console.WriteLine("End execute fill Log DataSet ...: " + DateTime.Now);
				System.Console.WriteLine("---------------------");

				System.Console.WriteLine("Start to execute fill Pass DataSet ...: " + DateTime.Now);
				System.Console.WriteLine("---------------------");
				
				// Insert to Passes table
				foreach(PassTO passTOMember in passTOList)
				{
					row = dataSet.Tables["passes"].NewRow();

					row["employee_id"] = passTOMember.EmployeeID;
					row["direction"] = passTOMember.Direction;
					row["event_time"] = passTOMember.EventTime;
					row["pass_type_id"] = passTOMember.PassTypeID;
					row["is_wrk_hrs"] = passTOMember.IsWrkHrsCount;
					row["location_id"] = passTOMember.LocationID;
					row["pair_gen_used"] = passTOMember.PairGenUsed;
					row["manual_created"] = passTOMember.ManualyCreated;
					row["created_by"] = DAOController.GetLogInUser().Trim();
					row["created_time"] = DateTime.Now;

					dataSet.Tables["passes"].Rows.Add(row);
				}
				System.Console.WriteLine("End execute fill Pass DataSet ...: " + DateTime.Now);
				System.Console.WriteLine("---------------------");

				System.Console.WriteLine("Start to execute insert Passes ...: " + DateTime.Now);
				System.Console.WriteLine("---------------------");
				sqlDataAdapterPass.Update(dataSet, "passes");
				System.Console.WriteLine("End execute insert Passes ...: " + DateTime.Now);
				System.Console.WriteLine("---------------------");

				System.Console.WriteLine("Start to execute update Log ...: " + DateTime.Now);
				System.Console.WriteLine("---------------------");
				sqlDataAdapterLog.Update(dataSet, "log");
				System.Console.WriteLine("End execute update Log ...: " + DateTime.Now);
				System.Console.WriteLine("---------------------");
			}
			catch(Exception ex)
			{
				System.Console.WriteLine("Exception in TestDB class: " + ex.Message);
				throw new Exception (ex.Message);
			}
		}

		/// <summary>
		/// Updates Log and Passes data table in the same transaction
		/// </summary>
		/// <param name="passTOList"></param>
		/// <param name="logTOList"></param>
		/// <param name="factory"></param>
		/// <returns></returns>
        public bool log2pass(List<PassTO> passTOList, List<LogTO> logTOList, ExitPermissionTO exitPerm, DAOFactory factory)
		{
			MSSQLLogDAO logDAO = (MSSQLLogDAO) factory.getLogDAO(conn);
			MSSQLExitPermissionDAO exitPermDAO = (MSSQLExitPermissionDAO) factory.getExitPermissionsDAO(conn);
			
			bool isUpdated = true;
			this.SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
			logDAO.SqlTrans = this.SqlTrans;
			exitPermDAO.SqlTrans = this.SqlTrans;

			try
			{
				if (exitPerm.PermissionID >= 0)
				{
					isUpdated = isUpdated && exitPermDAO.update(exitPerm.PermissionID, exitPerm.EmployeeID,
						exitPerm.PassTypeID, exitPerm.StartTime, exitPerm.Offset, exitPerm.Used, 
						exitPerm.Description, false);
				}

				if (isUpdated)
				{
					foreach(PassTO passTO in passTOList)
					{
						//isUpdated = isUpdated && (this.insert(passTO, false) > 0);
                        int passID = this.insertGetID(passTO, false);
                        isUpdated = isUpdated && (passID >= 0);

                        // get additional info if exist
                        if (isUpdated && logTOList.Count > 0 && logTOList[0].AddTO.LogID != 0)
                        {
                            StringBuilder sbInsert = new StringBuilder();
                            sbInsert.Append("INSERT INTO passes_additional_info ");
                            sbInsert.Append("(pass_id, gps_data, cardholder_name, cardholder_id, created_by, created_time) ");
                            sbInsert.Append("VALUES (");
                            sbInsert.Append("'" + passID.ToString().Trim() + "', ");
                            sbInsert.Append("N'" + logTOList[0].AddTO.GpsData.ToString().Trim() + "', ");
                            sbInsert.Append("N'" + logTOList[0].AddTO.CardholderName.Trim() + "', ");
                            sbInsert.Append("'" + logTOList[0].AddTO.CardholderID.ToString().Trim() + "', ");
                            sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                            sbInsert.Append("GETDATE()) ");
                            SqlCommand addCmd = new SqlCommand(sbInsert.ToString(), conn, SqlTrans);

                            isUpdated = isUpdated && (addCmd.ExecuteNonQuery() > 0);

                            if (!isUpdated)
                                break;
                        }
					}
				}

				if (isUpdated)
				{
					foreach(LogTO logTO in logTOList)
					{
						isUpdated = isUpdated && logDAO.updateAsUsed(logTO, false);
					}
				}						
				
				if(isUpdated)
				{
					commitTransaction();
				}
				else
				{
					rollbackTransaction();
				}				
			}
			catch(Exception ex)
			{
				rollbackTransaction();
				throw ex;
			}

			return isUpdated;
		}

		/// <summary>
		/// Returns list of all employees and distinct days they were worked
		/// </summary>
		/// <returns>Employees</returns>
		public ArrayList getEmpoloyeesByDate()
		{
			ArrayList employeesDate = new ArrayList();

			// list will contain two elements only:
			// first - employee_id and second is date 
			ArrayList member = new ArrayList();
			StringBuilder sb = new StringBuilder();
			DataSet dataSet = new DataSet();

			try
			{
				// CHANGED: Take only unused records
				sb.Append("SELECT DISTINCT employee_id, CONVERT (varchar (20), event_time, 101) AS workday "
					+ "FROM passes WHERE pair_gen_used = '" + ((int) Constants.PairGenUsed.Unused) + "' ORDER BY employee_id, workday");
				SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                cmd.CommandTimeout = Constants.commandTimeout;

				SqlDataAdapter addapter = new SqlDataAdapter(cmd);
				addapter.Fill(dataSet, "empl");

				DataTable table = dataSet.Tables["empl"];

				foreach(DataRow row in table.Rows)
				{
					member =  new ArrayList();
					member.Insert(0,Int32.Parse(row["employee_id"].ToString()));
					//member.Add(DateTime.Parse(row["workday"].ToString(), new CultureInfo("en-US"), DateTimeStyles.NoCurrentDateDefault));
					// Format of workday is mm/dd/yyy!!!!!
					member.Insert(1, row["workday"].ToString());

					employeesDate.Add(member);
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return employeesDate;
		}

        public List<PassTO> getDiferencePasses(DateTime lastReadingTime)
        {
            List<PassTO> passesList = new List<PassTO>();
            PassTO pass = new PassTO();
            DataSet dataset = new DataSet();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * ");
                sb.Append("FROM ( ");

                sb.Append("SELECT ");
                sb.Append("e.employee_id AS ID, ");
                sb.Append("e.first_name AS Ime, ");
                sb.Append("e.last_name AS Prezime, ");
                sb.Append("a.address_line_3 Mail, ");
                sb.Append("'REGISTROVAN' AS Status, ");
                sb.Append("p.event_time AS Vreme, ");
                sb.Append("p.direction AS Smer, ");
                sb.Append("pt.description AS Tip, ");
                sb.Append("l.name as Lokacija, ");
                sb.Append("g.name as Kapija ");
                sb.Append("FROM ");
                sb.Append("employees AS e, ");
                sb.Append("passes AS p, ");
                sb.Append("addresses AS a, ");
                sb.Append("locations AS l, ");
                sb.Append("gates AS g, ");
                sb.Append("pass_types as pt ");
                sb.Append("WHERE ");
                sb.Append("e.employee_id = p.employee_id ");
                sb.Append("AND ");
                sb.Append("e.address_id = a.address_id ");
                sb.Append("AND ");
                sb.Append("a.address_line_3 IS NOT NULL ");
                sb.Append("AND ");
                sb.Append("p.location_id = l.location_id ");
                sb.Append("AND ");
                sb.Append("p.gate_id = g.gate_id ");
                sb.Append("AND ");
                sb.Append("p.pass_type_id = pt.pass_type_id ");
                sb.Append("AND ");
                sb.Append("p.pair_gen_used = 1 ");
                sb.Append("AND ");
                sb.Append("p.created_time > CONVERT(datetime,'" + lastReadingTime.ToString("yyyy-MM-dd HH:mm:ss") + "', 111) ");

                sb.Append("UNION ");

                sb.Append("SELECT ");
                sb.Append("e.employee_id AS ID, ");
                sb.Append("e.first_name AS Ime, ");
                sb.Append("e.last_name AS Prezime, ");
                sb.Append("a.address_line_3 AS Mail, ");
                sb.Append("'IZMENJEN' AS Status, ");
                sb.Append("p.event_time AS Vreme, ");
                sb.Append("p.direction AS Smer, ");
                sb.Append("pt.description AS Tip, ");
                sb.Append("l.name as Lokacija, ");
                sb.Append("g.name as Kapija ");
                sb.Append("FROM ");
                sb.Append("employees AS e, ");
                sb.Append("passes AS p, ");
                sb.Append("addresses AS a, ");
                sb.Append("locations AS l, ");
                sb.Append("gates AS g, ");
                sb.Append("pass_types as pt ");
                sb.Append("WHERE ");
                sb.Append("e.employee_id = p.employee_id ");
                sb.Append("AND ");
                sb.Append("e.address_id = a.address_id ");
                sb.Append("AND ");
                sb.Append("a.address_line_3 IS NOT NULL ");
                sb.Append("AND ");
                sb.Append("p.location_id = l.location_id ");
                sb.Append("AND ");
                sb.Append("p.gate_id = g.gate_id ");
                sb.Append("AND ");
                sb.Append("p.pass_type_id = pt.pass_type_id ");
                sb.Append("AND ");
                sb.Append("p.pair_gen_used = 1 ");
                sb.Append("AND ");
                sb.Append("p.manual_created = 1 ");
                sb.Append("AND ");
                sb.Append("p.modified_time > CONVERT(datetime,'" + lastReadingTime.ToString("yyyy-MM-dd HH:mm:ss") + "', 111) ");
                sb.Append(") as gry ");
                sb.Append("ORDER BY ID, Vreme ");
                // CHANGED: Take only unused passes
                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                cmd.CommandTimeout = Constants.commandTimeout;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataset, "Pass");

                DataTable table = dataset.Tables["Pass"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pass = new PassTO();
                                             
                        if (row["ID"] != DBNull.Value)
                        {
                            pass.EmployeeID = Int32.Parse(row["ID"].ToString().Trim());
                        }
                        if (row["Ime"] != DBNull.Value)
                        {
                            pass.EmployeeName = row["Ime"].ToString().Trim();
                        }
                        if (row["Prezime"] != DBNull.Value)
                        {
                            pass.EmployeeName += " " + row["Prezime"].ToString().Trim();
                        }
                        if (row["Smer"] != DBNull.Value)
                        {
                            pass.Direction = row["Smer"].ToString().Trim();
                        }
                        if (row["Vreme"] != DBNull.Value)
                        {
                            pass.EventTime = (DateTime)row["Vreme"];
                        }
                        if (row["Lokacija"] != DBNull.Value)
                        {
                            pass.LocationName = row["Lokacija"].ToString().Trim();
                        }
                        if (row["Kapija"] != DBNull.Value)
                        {
                            pass.GateName = row["Kapija"].ToString().Trim();
                        }
                        if (row["Tip"] != DBNull.Value)
                        {
                            pass.Description= row["Tip"].ToString().Trim();
                        }
                        if (row["Mail"] != DBNull.Value)
                        {
                            pass.Email = row["Mail"].ToString().Trim();
                        }

                        if (row["Status"] != DBNull.Value)
                        {
                            pass.Status = row["Status"].ToString().Trim();
                        }
                        
                        passesList.Add(pass);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return passesList;
        }

		public List<PassTO> getPassesForEmployee(int empoyeeID, string date)
		{
			List<PassTO> passesList = new List<PassTO>();
			PassTO pass = new PassTO();
			DataSet dataset = new DataSet();

			try
			{
				// CHANGED: Take only unused passes
				SqlCommand cmd = new SqlCommand("select * from passes where employee_id = " + empoyeeID + " and " +
												" convert (varchar (20), event_time, 101) = '" + date + "' and pair_gen_used = '" + ((int) Constants.PairGenUsed.Unused) + "' order by event_time, pass_id ", conn);
                cmd.CommandTimeout = Constants.commandTimeout;

				SqlDataAdapter adapter = new SqlDataAdapter(cmd);
				adapter.Fill(dataset, "Pass");
				
				DataTable table = dataset.Tables["Pass"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						pass = new PassTO();
						pass.PassID = Int32.Parse(row["pass_id"].ToString().Trim());
						/*if (row["reader_id"] != DBNull.Value)
						{
							pass.ReaderID = Int32.Parse(row["reader_id"].ToString().Trim());
						}*/
						if (row["employee_id"] != DBNull.Value)
						{
							pass.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
						}
						if (row["direction"] != DBNull.Value)
						{
							pass.Direction = row["direction"].ToString().Trim();
						}
						if (row["event_time"] != DBNull.Value)
						{
							pass.EventTime = (DateTime) row["event_time"];
						}
						if (row["pass_type_id"] != DBNull.Value)
						{
							pass.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
						}
                        //Tamara 30.1.2020.
                        if (row["gate_id"] != DBNull.Value)
                        {
                            pass.GateID = Int32.Parse(row["gate_id"].ToString().Trim());
                        }
						if (row["pair_gen_used"] != DBNull.Value)
						{
							pass.PairGenUsed = Int32.Parse(row["pair_gen_used"].ToString().Trim());
						}
						if (row["location_id"] != DBNull.Value)
						{
							pass.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
						}
						if (row["manual_created"] != DBNull.Value)
						{
							pass.ManualyCreated = Int32.Parse(row["manual_created"].ToString().Trim());
						}
                        if (row["created_by"] != DBNull.Value)
                        {
                            pass.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pass.CreatedTime = (DateTime)row["created_time"];
                        }
						if (row["is_wrk_hrs"] != DBNull.Value)
						{
							pass.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs"].ToString().Trim());
						}

						passesList.Add(pass);
					}
				}				
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return passesList;
		}


        public List<PassTO> getPassesForEmployeeSched(int employeeID)
        {
            List<PassTO> passesList = new List<PassTO>();
            PassTO pass = new PassTO();
            DataSet dataset = new DataSet();

            try
            {
                // CHANGED: Take only unused passes
                SqlCommand cmd = new SqlCommand("select * from passes where employee_id = " + employeeID + " order by event_time, pass_id ", conn);
                cmd.CommandTimeout = Constants.commandTimeout;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataset, "Pass");

                DataTable table = dataset.Tables["Pass"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pass = new PassTO();
                        pass.PassID = Int32.Parse(row["pass_id"].ToString().Trim());
                        /*if (row["reader_id"] != DBNull.Value)
                        {
                            pass.ReaderID = Int32.Parse(row["reader_id"].ToString().Trim());
                        }*/
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pass.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["direction"] != DBNull.Value)
                        {
                            pass.Direction = row["direction"].ToString().Trim();
                        }
                        if (row["event_time"] != DBNull.Value)
                        {
                            pass.EventTime = (DateTime)row["event_time"];
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pass.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["pair_gen_used"] != DBNull.Value)
                        {
                            pass.PairGenUsed = Int32.Parse(row["pair_gen_used"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pass.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pass.ManualyCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            pass.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pass.CreatedTime = (DateTime)row["created_time"];
                        }
                        if (row["is_wrk_hrs"] != DBNull.Value)
                        {
                            pass.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs"].ToString().Trim());
                        }

                        passesList.Add(pass);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return passesList;
        }





        public Dictionary<int, Dictionary<DateTime, List<PassTO>>> getPassesForEmployeesPeriod(string employeesID, DateTime from, DateTime to)
        {
            Dictionary<int, Dictionary<DateTime, List<PassTO>>> emplPasses = new Dictionary<int, Dictionary<DateTime, List<PassTO>>>();
            PassTO pass = new PassTO();
            DataSet dataSet = new DataSet();

            try
            {
                CultureInfo ci = CultureInfo.InvariantCulture;

                string select = "SELECT * FROM passes WHERE event_time >= '" + from.Date.ToString(dateTimeformat) + "' AND event_time < '" + to.AddDays(1).ToString(dateTimeformat) + "' ";

                if (employeesID.Length > 0)
                    select += "AND employee_id IN (" + employeesID + ") ";

                select += "ORDER BY employee_id, event_time";
                                
                SqlCommand cmd = new SqlCommand(select.Trim(), conn);
                cmd.CommandTimeout = Constants.commandTimeout;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Passes");
                DataTable table = dataSet.Tables["Passes"];

                if (table.Rows.Count > 0)
                {                    
                    foreach (DataRow row in table.Rows)
                    {
                        pass = new PassTO();
                        pass.PassID = Int32.Parse(row["pass_id"].ToString().Trim());
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pass.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["direction"] != DBNull.Value)
                        {
                            pass.Direction = row["direction"].ToString().Trim();
                        }
                        if (row["event_time"] != DBNull.Value)
                        {
                            pass.EventTime = DateTime.Parse(row["event_time"].ToString());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pass.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["pair_gen_used"] != DBNull.Value)
                        {
                            pass.PairGenUsed = Int32.Parse(row["pair_gen_used"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pass.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pass.ManualyCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs"] != DBNull.Value)
                        {
                            pass.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs"].ToString().Trim());
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            pass.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pass.CreatedTime = DateTime.Parse(row["created_time"].ToString());
                        }
                        
                        if (!emplPasses.ContainsKey(pass.EmployeeID))                        
                            emplPasses.Add(pass.EmployeeID, new Dictionary<DateTime, List<PassTO>>());                        

                        if (!emplPasses[pass.EmployeeID].ContainsKey(pass.EventTime.Date))                        
                            emplPasses[pass.EmployeeID].Add(pass.EventTime.Date, new List<PassTO>());

                        emplPasses[pass.EmployeeID][pass.EventTime.Date].Add(pass);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return emplPasses;
        }
        
        public List<PassTO> getPassesForEmployeeAll(int empoyeeID, DateTime date)
        {
            List<PassTO> passesList = new List<PassTO>();
            PassTO pass = new PassTO();
            DataSet dataset = new DataSet();

            try
            {
                // CHANGED: Take only unused passes
                SqlCommand cmd = new SqlCommand("select * from passes where employee_id = " + empoyeeID + " and " +
                                                " convert (varchar (20), event_time, 101) = '" + date.ToString("yyyy-MM-dd") + "' AND UPPER(direction) = 'IN' order by event_time, pass_id ", conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = Constants.commandTimeout;

                adapter.Fill(dataset, "Pass");

                DataTable table = dataset.Tables["Pass"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pass = new PassTO();
                        pass.PassID = Int32.Parse(row["pass_id"].ToString().Trim());
                        /*if (row["reader_id"] != DBNull.Value)
                        {
                            pass.ReaderID = Int32.Parse(row["reader_id"].ToString().Trim());
                        }*/
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pass.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["direction"] != DBNull.Value)
                        {
                            pass.Direction = row["direction"].ToString().Trim();
                        }
                        if (row["event_time"] != DBNull.Value)
                        {
                            pass.EventTime = (DateTime)row["event_time"];
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pass.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["pair_gen_used"] != DBNull.Value)
                        {
                            pass.PairGenUsed = Int32.Parse(row["pair_gen_used"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pass.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pass.ManualyCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }

                        if (row["created_by"] != DBNull.Value)
                        {
                            pass.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pass.CreatedTime = (DateTime)row["created_time"];
                        }
                        if (row["is_wrk_hrs"] != DBNull.Value)
                        {
                            pass.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs"].ToString().Trim());
                        }

                        passesList.Add(pass);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return passesList;
        }


		public List<PassTO> getPassesForExitPerm(int employeeID, DateTime eventTime)
		{
			List<PassTO> passesList = new List<PassTO>();
			PassTO pass = new PassTO();
			DataSet dataSet = new DataSet();
			
			try
			{
				CultureInfo ci = CultureInfo.InvariantCulture;

				StringBuilder sbSelect = new StringBuilder();
				sbSelect.Append("SELECT ps.*, empl.last_name, empl.first_name, pt.description, loc.name ");
				sbSelect.Append("FROM passes ps, employees empl, pass_types pt, locations loc ");
				sbSelect.Append("WHERE ps.employee_id = '" + employeeID.ToString() + "' AND ");
				sbSelect.Append("CONVERT(VARCHAR(20), ps.event_time, 101) = '" + eventTime.ToString("MM/dd/yyy", ci) + "' AND ");
				sbSelect.Append("UPPER(ps.direction) = N'OUT' AND");
                sbSelect.Append(" ps.is_wrk_hrs = 1 AND");
                sbSelect.Append(" ps.pass_type_id = 0 AND");

				sbSelect.Append(" empl.employee_id = ps.employee_id AND pt.pass_type_id = ps.pass_type_id AND ");
				sbSelect.Append("loc.location_id = ps.location_id");
				
				SqlCommand cmd = new SqlCommand(sbSelect.ToString(), conn );
                cmd.CommandTimeout = Constants.commandTimeout;

				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Passes");
				DataTable table = dataSet.Tables["Passes"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						pass = new PassTO();
						pass.PassID = Int32.Parse(row["pass_id"].ToString().Trim());
						if (row["employee_id"] != DBNull.Value)
						{
							pass.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
						}
						if (row["direction"] != DBNull.Value)
						{
							pass.Direction = row["direction"].ToString().Trim();
						}
						if (row["event_time"] != DBNull.Value)
						{
							pass.EventTime = (DateTime) row["event_time"];
						}
						if (row["pass_type_id"] != DBNull.Value)
						{
							pass.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
						}
						if (row["pair_gen_used"] != DBNull.Value)
						{
							pass.PairGenUsed = Int32.Parse(row["pair_gen_used"].ToString().Trim());
						}
						if (row["location_id"] != DBNull.Value)
						{
							pass.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
						}
						if (row["manual_created"] != DBNull.Value)
						{
							pass.ManualyCreated = Int32.Parse(row["manual_created"].ToString().Trim());
						}
                        if (row["created_by"] != DBNull.Value)
                        {
                            pass.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pass.CreatedTime = (DateTime)row["created_time"];
                        }
						if (row["is_wrk_hrs"] != DBNull.Value)
						{
							pass.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs"].ToString().Trim());
						}
						if (row["last_name"] != DBNull.Value)
						{
							pass.EmployeeName = row["last_name"].ToString().Trim();
						}
						if (row["first_name"] != DBNull.Value)
						{
							pass.EmployeeName += " " + row["first_name"].ToString().Trim();
						}
						if (row["description"] != DBNull.Value)
						{
							pass.PassType = row["description"].ToString().Trim();
						}
						if (row["name"] != DBNull.Value)
						{
							pass.LocationName = row["name"].ToString().Trim();
						}

						passesList.Add(pass);
					}
				}				
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return passesList;
		}

		public List<PassTO> getPassesForExitPerm(int employeeID, DateTime startTime, int offset)
		{
			List<PassTO> passesList = new List<PassTO>();
			PassTO pass = new PassTO();
			DataSet dataSet = new DataSet();
			
			try
			{
				StringBuilder sbSelect = new StringBuilder();
				sbSelect.Append("SELECT * ");
				sbSelect.Append("FROM passes ");
				sbSelect.Append("WHERE employee_id = '" + employeeID.ToString() + "' AND ");
				sbSelect.Append("CONVERT(VARCHAR(20), event_time, 120) >= '" + startTime.ToString("yyy-MM-dd HH:mm:ss") + "' AND ");
				sbSelect.Append("CONVERT(VARCHAR(20), event_time, 120) <= '" + startTime.AddMinutes(offset).ToString("yyy-MM-dd HH:mm:ss") + "' AND ");
                sbSelect.Append(" ps.is_wrk_hrs = 1 AND ");
                sbSelect.Append(" ps.pass_type_id = 0 AND ");
				sbSelect.Append("UPPER(direction) = N'OUT' ORDER BY event_time");
				
				SqlCommand cmd = new SqlCommand(sbSelect.ToString(), conn );
                cmd.CommandTimeout = Constants.commandTimeout;

				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Passes");
				DataTable table = dataSet.Tables["Passes"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						pass = new PassTO();
						pass.PassID = Int32.Parse(row["pass_id"].ToString().Trim());
						if (row["employee_id"] != DBNull.Value)
						{
							pass.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
						}
						if (row["direction"] != DBNull.Value)
						{
							pass.Direction = row["direction"].ToString().Trim();
						}
						if (row["event_time"] != DBNull.Value)
						{
							pass.EventTime = (DateTime) row["event_time"];
						}
						if (row["pass_type_id"] != DBNull.Value)
						{
							pass.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
						}
						if (row["pair_gen_used"] != DBNull.Value)
						{
							pass.PairGenUsed = Int32.Parse(row["pair_gen_used"].ToString().Trim());
						}
						if (row["location_id"] != DBNull.Value)
						{
							pass.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
						}
						if (row["manual_created"] != DBNull.Value)
						{
							pass.ManualyCreated = Int32.Parse(row["manual_created"].ToString().Trim());
						}
                        if (row["created_by"] != DBNull.Value)
                        {
                            pass.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pass.CreatedTime = (DateTime)row["created_time"];
                        }
						if (row["is_wrk_hrs"] != DBNull.Value)
						{
							pass.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs"].ToString().Trim());
						}
						
						passesList.Add(pass);
					}
				}				
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return passesList;
		}

		public List<PassTO> getPermPassesForPair(int employeeID, DateTime startTime, DateTime endTime)
		{
			List<PassTO> passesList = new List<PassTO>();
			PassTO pass = new PassTO();
			DataSet dataSet = new DataSet();
			
			try
			{
				StringBuilder sbSelect = new StringBuilder();
				sbSelect.Append("SELECT * FROM passes WHERE ");
				sbSelect.Append("employee_id = '" + employeeID.ToString() + "' AND is_wrk_hrs = '" + (int) Constants.IsWrkCount.IsCounter + "' ");
				sbSelect.Append("AND UPPER(created_by) = N'" + Constants.PermPassUser.ToUpper() + "' ");
				sbSelect.Append("AND ((CONVERT(VARCHAR(20), event_time, 120) = '" + startTime.ToString("yyy-MM-dd HH:mm:ss") + "' ");
				sbSelect.Append("AND UPPER(direction) = N'" + Constants.DirectionIn.ToUpper() + "') ");
				sbSelect.Append("OR (CONVERT(VARCHAR(20), event_time, 120) = '" + endTime.ToString("yyy-MM-dd HH:mm:ss") + "' ");
				sbSelect.Append("AND UPPER(direction) = N'" + Constants.DirectionOut.ToUpper() + "'))");
				
				SqlCommand cmd = new SqlCommand(sbSelect.ToString(), conn );
                cmd.CommandTimeout = Constants.commandTimeout;

				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Passes");
				DataTable table = dataSet.Tables["Passes"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						pass = new PassTO();
						pass.PassID = Int32.Parse(row["pass_id"].ToString().Trim());
						if (row["employee_id"] != DBNull.Value)
						{
							pass.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
						}
						if (row["direction"] != DBNull.Value)
						{
							pass.Direction = row["direction"].ToString().Trim();
						}
						if (row["event_time"] != DBNull.Value)
						{
							pass.EventTime = (DateTime) row["event_time"];
						}
						if (row["pass_type_id"] != DBNull.Value)
						{
							pass.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
						}
						if (row["pair_gen_used"] != DBNull.Value)
						{
							pass.PairGenUsed = Int32.Parse(row["pair_gen_used"].ToString().Trim());
						}
						if (row["location_id"] != DBNull.Value)
						{
							pass.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
						}
						if (row["manual_created"] != DBNull.Value)
						{
							pass.ManualyCreated = Int32.Parse(row["manual_created"].ToString().Trim());
						}
                        if (row["created_by"] != DBNull.Value)
                        {
                            pass.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pass.CreatedTime = (DateTime)row["created_time"];
                        }
						if (row["is_wrk_hrs"] != DBNull.Value)
						{
							pass.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs"].ToString().Trim());
						}
						
						passesList.Add(pass);
					}
				}				
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return passesList;
		}

		public PassTO getPassBeforePermissionPass(int emplID, DateTime eventTime)
		{
			PassTO pass = new PassTO();
			DataSet dataSet = new DataSet();
			
			try
			{
				StringBuilder sbSelect = new StringBuilder();
				sbSelect.Append("SELECT TOP 1 * FROM passes WHERE employee_id = '" + emplID.ToString().Trim() + "' ");
				sbSelect.Append("AND is_wrk_hrs = '" + (int) Constants.IsWrkCount.IsCounter + "' ");
				sbSelect.Append("AND convert(VARCHAR(20), event_time, 101) = '" + eventTime.ToString("MM/dd/yyy") + "' ");
				sbSelect.Append("AND CONVERT(VARCHAR(20), event_time, 108) < '" + eventTime.ToString("HH:mm:ss") + "' ");
				sbSelect.Append("ORDER BY event_time DESC");

				SqlCommand cmd = new SqlCommand(sbSelect.ToString(), conn );
                cmd.CommandTimeout = Constants.commandTimeout;

				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Passes");
				DataTable table = dataSet.Tables["Passes"];

				if (table.Rows.Count > 0)
				{
					pass.PassID = Int32.Parse(table.Rows[0]["pass_id"].ToString().Trim());
					if (table.Rows[0]["employee_id"] != DBNull.Value)
					{
						pass.EmployeeID = Int32.Parse(table.Rows[0]["employee_id"].ToString().Trim());
					}
					if (table.Rows[0]["direction"] != DBNull.Value)
					{
						pass.Direction = table.Rows[0]["direction"].ToString().Trim();
					}
					if (table.Rows[0]["event_time"] != DBNull.Value)
					{
						pass.EventTime = (DateTime) table.Rows[0]["event_time"];
					}
					if (table.Rows[0]["pass_type_id"] != DBNull.Value)
					{
						pass.PassTypeID = Int32.Parse(table.Rows[0]["pass_type_id"].ToString().Trim());
					}
					if (table.Rows[0]["pair_gen_used"] != DBNull.Value)
					{
						pass.PairGenUsed = Int32.Parse(table.Rows[0]["pair_gen_used"].ToString().Trim());
					}
					if (table.Rows[0]["location_id"] != DBNull.Value)
					{
						pass.LocationID = Int32.Parse(table.Rows[0]["location_id"].ToString().Trim());
					}
					if (table.Rows[0]["manual_created"] != DBNull.Value)
					{
						pass.ManualyCreated = Int32.Parse(table.Rows[0]["manual_created"].ToString().Trim());
					}
                    if (table.Rows[0]["created_by"] != DBNull.Value)
                    {
                        pass.CreatedBy = table.Rows[0]["created_by"].ToString().Trim();
                    }
                    if (table.Rows[0]["created_time"] != DBNull.Value)
                    {
                        pass.CreatedTime = (DateTime)table.Rows[0]["created_time"];
                    }
					if (table.Rows[0]["is_wrk_hrs"] != DBNull.Value)
					{
						pass.IsWrkHrsCount = Int32.Parse(table.Rows[0]["is_wrk_hrs"].ToString().Trim());
					}	
				}			
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return pass;
		}

        public List<PassTO> getCurrentPasses(PassTO passTO, DateTime fromTime, DateTime toTime, string wUnits, string modifiedBy)//Current passes values for history of change
        {
            List<PassTO> passesList = new List<PassTO>();
            PassTO pass = new PassTO();
            DataSet dataSet = new DataSet();

            try
            {
                CultureInfo ci = CultureInfo.InvariantCulture;

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ps.*, empl.last_name, empl.first_name, pt.description, loc.name ");
                sb.Append("FROM passes ps, employees empl, pass_types pt, locations loc ");
                sb.Append("WHERE ps.pass_id IN ( SELECT ph.pass_id FROM passes_hist ph WHERE ");

                if ((passTO.EmployeeID != -1) || (!passTO.Direction.Trim().Equals("")) ||
                    (!fromTime.Equals(new DateTime())) || (!toTime.Equals(new DateTime())) ||
                    (passTO.PassTypeID != -1) || (passTO.LocationID != -1)
                    || (!modifiedBy.Equals("")))
                {
                    if (passTO.EmployeeID != -1)
                    {
                        //sb.Append(" UPPER(ps.employee_id) LIKE '" + employeeID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" ph.employee_id = '" + passTO.EmployeeID.ToString().Trim() + "' AND");
                    }
                    if (!passTO.Direction.Trim().Equals(""))
                    {
                        sb.Append(" RTRIM(LTRIM(UPPER(ph.direction))) LIKE N'%" + passTO.Direction.Trim().ToUpper() + "%' AND");
                    }
                    if (!fromTime.Equals(new DateTime()) && !toTime.Equals(new DateTime()))
                    {
                        //sb.Append(" DATE_FORMAT(ps.event_time,'%Y-%m-%dT%h:%m:%s') >='" + fromTime.ToString(dateTimeformat,ci).Trim() + "' AND DATE_FORMAT(ps.event_time,'%Y-%m-%dT%h:%m:%s') <= '" + toTime.AddDays(1).ToString(dateTimeformat,ci).Trim() + "' AND");
                        sb.Append(" ph.event_time >= CONVERT(datetime,'" + fromTime.ToString("yyyy-MM-dd") + "', 111) AND ");
                        sb.Append(" ph.event_time < CONVERT(datetime,'" + toTime.AddDays(1).ToString("yyyy-MM-dd") + "', 111) ");
                    }
                    if (passTO.PassTypeID != -1)
                    {
                        //sb.Append(" UPPER(ps.pass_type_id) LIKE '" + passTypeID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" AND ph.pass_type_id = '" + passTO.PassTypeID.ToString().Trim() + "'");
                    }
                    if (passTO.LocationID != -1)
                    {
                        //sb.Append(" UPPER(ps.location_id) LIKE '" + locationID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" AND ph.location_id = '" + passTO.LocationID.ToString().Trim() + "'");
                    }

                    if (!modifiedBy.Equals("") && !modifiedBy.Equals("*"))
                    {
                        sb.Append(" AND ph.modified_by = N'" + modifiedBy.Trim() + "'");
                    }

                    //select = sb.ToString(0, sb.ToString().Length - 3);
                }

                sb.Append(" GROUP BY ph.pass_id) AND");
                sb.Append(" empl.employee_id = ps.employee_id AND pt.pass_type_id = ps.pass_type_id AND ");
                sb.Append("loc.location_id = ps.location_id ");                

                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                cmd.CommandTimeout = Constants.commandTimeout;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Passes");
                DataTable table = dataSet.Tables["Passes"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pass = new PassTO();
                        pass.PassID = Int32.Parse(row["pass_id"].ToString().Trim());
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pass.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["direction"] != DBNull.Value)
                        {
                            pass.Direction = row["direction"].ToString().Trim();
                        }
                        if (row["event_time"] != DBNull.Value)
                        {
                            pass.EventTime = DateTime.Parse(row["event_time"].ToString());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pass.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["pair_gen_used"] != DBNull.Value)
                        {
                            pass.PairGenUsed = Int32.Parse(row["pair_gen_used"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pass.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pass.ManualyCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs"] != DBNull.Value)
                        {
                            pass.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs"].ToString().Trim());
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            pass.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pass.CreatedTime = DateTime.Parse(row["created_time"].ToString());
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            pass.EmployeeName = row["last_name"].ToString().Trim();
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            pass.EmployeeName += " " + row["first_name"].ToString().Trim();
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            pass.PassType = row["description"].ToString().Trim();
                        }
                        if (row["name"] != DBNull.Value)
                        {
                            pass.LocationName = row["name"].ToString().Trim();
                        }

                        passesList.Add(pass);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return passesList;
        }

		public bool serialize(List<PassTO> passesTOList)
		{
			bool isSerialized = false;

			try
			{
				//string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLPassesFile"];
                string filename = Constants.XMLDataSourceDir + Constants.XMLPassesFile;
				serialize(passesTOList, filename);

				isSerialized = true;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			return isSerialized;
		}
		
		private bool serialize(List<PassTO> passesTOList, string filePath)
		{
			bool isSerialized = false;

			try
			{
				Stream stream = File.Open(filePath, FileMode.Create);

				PassTO[] passesTOArray = (PassTO[]) passesTOList.ToArray();

				XmlSerializer bformatter = new XmlSerializer(typeof(PassTO[]));
				bformatter.Serialize(stream, passesTOArray);
				stream.Close();

				isSerialized = true;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			return isSerialized;
		}

		public List<PassTO> deserialize(string filePath)
		{
			List<PassTO> passesTO = new List<PassTO>();
			try
			{
				if (File.Exists(filePath))
				{
					Stream stream = File.Open(filePath, FileMode.Open);

					XmlSerializer bformatter = new XmlSerializer(typeof(PassTO[]));
					PassTO[] deserialized = (PassTO[]) bformatter.Deserialize(stream);
					ArrayList passesListTO = ArrayList.Adapter(deserialized);

                    foreach (PassTO passTO in passesListTO)
                    {
                        passesTO.Add(passTO);
                    }

					stream.Close();
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}

			return passesTO;
		}
        //TAMARA 30.1.2020.
        public bool compareGates(int gateID, int organUnitID)
        {
            DataSet dataSet = new DataSet();
            int foundGate = -1;
            bool result = false;
            try
            {
                SqlCommand cmd = new SqlCommand("select gate_id from organizational_unit_x_gate where organizational_unit_id= '" + organUnitID + "'", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Gate");
                DataTable table = dataSet.Tables["Gate"];

                if (table.Rows.Count >0)
                {

                    foreach (DataRow row in table.Rows)
                    {
                        if (row["gate_id"] != DBNull.Value)
                        {
                            foundGate = Int32.Parse(row["gate_id"].ToString().Trim());
                            if (foundGate == gateID)
                            {
                                result = true;
                                break;
                            }
                        }
                    }
                }
                
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public int getOrganUnitID(ulong tagID)
        {
            DataSet dataSet = new DataSet();
            EmployeeTO employee = new EmployeeTO();
            try
            {
                SqlCommand cmd = new SqlCommand(" select e.* from employees e join tags t on e.employee_id=t.owner_id where t.tag_id= '" + tagID + "' and t.status='ACTIVE'", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Empl");
                DataTable table = dataSet.Tables["Empl"];

                if (table.Rows.Count == 1)
                {
                    employee = new EmployeeTO();
                    employee.EmployeeID = Int32.Parse(table.Rows[0]["employee_id"].ToString().Trim());
                    if (table.Rows[0]["first_name"] != DBNull.Value)
                    {
                        employee.FirstName = table.Rows[0]["first_name"].ToString().Trim();
                    }
                    if (table.Rows[0]["last_name"] != DBNull.Value)
                    {
                        employee.LastName = table.Rows[0]["last_name"].ToString().Trim();
                    }
                    if (table.Rows[0]["working_unit_id"] != DBNull.Value)
                    {
                        employee.WorkingUnitID = Int32.Parse(table.Rows[0]["working_unit_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["status"] != DBNull.Value)
                    {
                        employee.Status = table.Rows[0]["status"].ToString().Trim();
                    }
                    if (table.Rows[0]["password"] != DBNull.Value)
                    {
                        employee.Password = table.Rows[0]["password"].ToString().Trim();
                    }
                    if (table.Rows[0]["address_id"] != DBNull.Value)
                    {
                        employee.AddressID = Int32.Parse(table.Rows[0]["address_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["picture"] != DBNull.Value)
                    {
                        employee.Picture = table.Rows[0]["picture"].ToString().Trim();
                    }
                    if (table.Rows[0]["employee_group_id"] != DBNull.Value)
                    {
                        employee.WorkingGroupID = Int32.Parse(table.Rows[0]["employee_group_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["type"] != DBNull.Value)
                    {
                        employee.Type = table.Rows[0]["type"].ToString().Trim();
                    }
                    if (table.Rows[0]["access_group_id"] != DBNull.Value)
                    {
                        employee.AccessGroupID = Int32.Parse(table.Rows[0]["access_group_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["organizational_unit_id"] != DBNull.Value)
                    {
                        employee.OrgUnitID = Int32.Parse(table.Rows[0]["organizational_unit_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["employee_type_id"] != DBNull.Value)
                    {
                        employee.EmployeeTypeID = Int32.Parse(table.Rows[0]["employee_type_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["created_by"] != DBNull.Value)
                    {
                        employee.CreatedBy = table.Rows[0]["created_by"].ToString().Trim();
                    }
                    if (table.Rows[0]["created_time"] != DBNull.Value)
                    {
                        employee.CreatedTime = DateTime.Parse(table.Rows[0]["created_time"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return employee.OrgUnitID;
        }

        public int getGate(int readerID)
        {
            DataSet dataSet = new DataSet();
            int gateID = -1;
            //List<GateTO> gatesList = new List<GateTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ant_0_gate_id FROM readers ");

                sb.Append(" WHERE reader_id= " + readerID);

                select = sb.ToString();


                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Gate");
                DataTable table = dataSet.Tables["Gate"];

                if (table.Rows.Count == 1)
                {
                    if (table.Rows[0]["ant_0_gate_id"] != DBNull.Value)
                    {
                        gateID = int.Parse(table.Rows[0]["ant_0_gate_id"].ToString().Trim());
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return gateID;
        }

        public List<PassTO> getListOfIN_OUTforDay(DateTime date, int emplID, string direction)
        {
            List<PassTO> listaProlazaka = new List<PassTO>();
            //string select = "SELECT pass_id,direction,event_time FROM actamgr.passes WHERE employee_id="+emplID+" and direction='"+direction+"' and convert(date,event_time)='"+date.ToString("yyyy-MM-dd")+"' ORDER BY event_time";
            string select = "";
            if(direction=="IN")
                select = "SELECT pass_id,direction,event_time,is_wrk_hrs FROM actamgr.passes WHERE employee_id=" + emplID + " and direction='" + direction + "' and convert(date,event_time)='" + date.ToString("yyyy-MM-dd") + "' ORDER BY event_time";
            else
                select = "SELECT pass_id,direction,event_time,is_wrk_hrs FROM actamgr.passes WHERE employee_id=" + emplID + " and direction='" + direction + "' and convert(date,event_time)='" + date.ToString("yyyy-MM-dd") + "' ORDER BY event_time desc";
            
            try
            {
                DataSet dataSet = new DataSet();
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dataSet, "PassTable");
                DataTable table = dataSet.Tables["PassTable"];
                PassTO pass = new PassTO();
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pass = new PassTO();
                        pass.PassID = int.Parse(row[0].ToString());
                        pass.Direction = row[1].ToString();
                        pass.EventTime = DateTime.Parse(row[2].ToString());
                        pass.IsWrkHrsCount = int.Parse(row[3].ToString());
                        listaProlazaka.Add(pass);
                    }
                }
                string update = "UPDATE actamgr.passes SET [pair_gen_used]=0 WHERE employee_id="+emplID+" and direction='"+direction+"' and convert(date,event_time)='"+date.ToString("yyyy-MM-dd")+"'";
                SqlCommand cmdUpd = new SqlCommand(update, conn);
                cmdUpd.CommandTimeout = 1200;
                cmdUpd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listaProlazaka;
        }

        public void updateListPassesToWrkHrs0(string passIDs)
        {
            try
            {
                string update = "UPDATE actamgr.passes SET is_wrk_hrs=0,modified_by='"+DAOController.GetLogInUser().Trim()+"',modified_time=GETDATE() WHERE pass_id in (" + passIDs + ")";
                SqlCommand cmdUpd = new SqlCommand(update, conn);
                cmdUpd.CommandTimeout = 1200;
                cmdUpd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<PassTO> getPassesForDayForEmployee(int emplID, DateTime day)
        {
            List<PassTO> passesList = new List<PassTO>();
            PassTO pass = new PassTO();
            DataSet dataSet = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * From actamgr.passes WHERE employee_id=" + emplID + " and event_time between '" + day.Date.ToString("yyyy-MM-dd") + "' and '" + day.Date.AddDays(1).ToString("yyyy-MM-dd") + "' and is_wrk_hrs=1 and direction='IN'", conn);
                cmd.CommandTimeout = Constants.commandTimeout;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataSet, "Passes");
                DataTable table = dataSet.Tables["Passes"];
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pass = new PassTO();
                        pass.PassID = Int32.Parse(row["pass_id"].ToString().Trim());
                        /*if (row["reader_id"] != DBNull.Value)
                        {
                            pass.ReaderID = Int32.Parse(row["reader_id"].ToString().Trim());
                        }*/
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pass.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["direction"] != DBNull.Value)
                        {
                            pass.Direction = row["direction"].ToString().Trim();
                        }
                        if (row["event_time"] != DBNull.Value)
                        {
                            pass.EventTime = (DateTime)row["event_time"];
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pass.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["pair_gen_used"] != DBNull.Value)
                        {
                            pass.PairGenUsed = Int32.Parse(row["pair_gen_used"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pass.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pass.ManualyCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            pass.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pass.CreatedTime = (DateTime)row["created_time"];
                        }
                        if (row["is_wrk_hrs"] != DBNull.Value)
                        {
                            pass.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs"].ToString().Trim());
                        }

                        passesList.Add(pass);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return passesList;
        }
        public void updatePassesOnUnprocessed(int emplID, DateTime date, bool doCommit)
        {
            SqlTransaction sqlTrans = null;
            if (doCommit)
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");
            else
                sqlTrans = this.SqlTrans;
            try
            {
                string update = "UPDATE actamgr.passes SET pair_gen_used=0 WHERE employee_id=" + emplID.ToString() + " and event_time>='" + date.Date.ToString("yyyy-MM-dd 00:00:00") + "' and event_time<'" + date.Date.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "' and is_wrk_hrs=1";
                SqlCommand cmd = new SqlCommand(update, conn, sqlTrans);
                cmd.CommandTimeout = Constants.commandTimeout;
                cmd.ExecuteNonQuery();
                if (doCommit)
                    sqlTrans.Commit();
            }
            catch (SqlException sqlEx)
            {
                //isUpdated = false;
                if (doCommit)
                {
                    sqlTrans.Rollback("UPDATE");
                }
                else
                {
                    sqlTrans.Rollback();
                }
                if (sqlEx.Number == 2627)
                {
                    DataProcessingException procEx = new DataProcessingException(sqlEx.Message, 14);
                    throw procEx;
                }
                else
                {
                    DataProcessingException procEx = new DataProcessingException(sqlEx.Message, 11);
                    throw procEx;
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback("UPDATE");
                }
                else
                {
                    sqlTrans.Rollback();
                }
                DataProcessingException procEx = new DataProcessingException(ex.Message, 11);
                throw procEx;
            }
        }


	}
}
	