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
	/// Summary description for MSSQLExtraHourUsedDAO.
	/// </summary>
	public class MSSQLExtraHourUsedDAO : ExtraHourUsedDAO
	{
		SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;

		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

		public MSSQLExtraHourUsedDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
		}
        public MSSQLExtraHourUsedDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
        }
		public int insert(int employeeID, DateTime dateEarned, DateTime dateUsed, int extraTimeAmtUsed,
			DateTime startTime, DateTime endTime, Int32 ioPairID, string type, bool doCommit)
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
				sbInsert.Append("INSERT INTO extra_hours_used ");
				sbInsert.Append("(employee_id, date_earned, date_used, extra_time_amt_used, start_time, end_time, io_pair_id, type, created_by, created_time) ");
				sbInsert.Append("VALUES (");

				sbInsert.Append("" + employeeID + ", ");

				sbInsert.Append("'" + dateEarned.ToString("yyy-MM-dd") + "', ");

				sbInsert.Append("'" + dateUsed.ToString("yyy-MM-dd") + "', ");

				sbInsert.Append("" + extraTimeAmtUsed + ", ");

                if (startTime == new DateTime())
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + startTime.ToString("yyy-MM-dd HH:mm") + "', ");
                }

                if (endTime == new DateTime())
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + endTime.ToString("yyy-MM-dd HH:mm") + "', ");
                }

				if (ioPairID == -1)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("" + ioPairID + ", ");
				}

                sbInsert.Append("'" + type + "', ");
				
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

		//not in use yet
		public bool update(int recordID, int employeeID, DateTime dateEarned, DateTime dateUsed, 
			int extraTimeAmtUsed, bool doCommit)
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
				sbUpdate.Append("UPDATE extra_hours_used SET ");

				/*if (extraTimeAmt != -1)
				{
					sbUpdate.Append("extra_time_amt = " + extraTimeAmt + ", ");
				}
				else
				{
					//not null
				}
				
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = GETDATE() ");
				sbUpdate.Append("WHERE employee_id = " + employeeID + " AND date_earned = '" + dateEarned.ToString("yyy-MM-dd").Trim() + "'");*/
				
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

        public bool delete(int recordID, bool isRegularWork)
		{
			DataSet dataSet = new DataSet();
			bool isDeleted = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT io_pair_id FROM extra_hours_used WHERE record_id = " + recordID);

				SqlCommand cmdSelect = new SqlCommand(sb.ToString(), conn, sqlTrans);
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmdSelect);

				sqlDataAdapter.Fill(dataSet, "ExtraHourUsed");
				DataTable table = dataSet.Tables["ExtraHourUsed"];

				int ioPairID = -1;
				if (table.Rows.Count == 1)
				{
					if (!table.Rows[0]["io_pair_id"].Equals(DBNull.Value))
					{
						ioPairID = Int32.Parse(table.Rows[0]["io_pair_id"].ToString().Trim());							
					}
				}

                if ((ioPairID != -1) || !isRegularWork)
				{
					StringBuilder sbDelete = new StringBuilder();

					//Delete Extra Hours Used
					sbDelete.Append("DELETE FROM extra_hours_used WHERE record_id = " + recordID);
					SqlCommand cmd = new SqlCommand( sbDelete.ToString(), conn, sqlTrans);
					int res = cmd.ExecuteNonQuery();

                    if (isRegularWork && (res != 0))
                    {
                        //Delete appropriate IO Pair
                        sbDelete.Length = 0;
                        sbDelete.Append("DELETE FROM io_pairs WHERE io_pair_id = " + ioPairID);
                        cmd.CommandText = sbDelete.ToString();
                        res = cmd.ExecuteNonQuery();
                    }

					if (res != 0)
					{
						isDeleted = true;
                        sqlTrans.Commit();
					}
                    else
                        sqlTrans.Rollback("DELETE");
				}
                else
                    sqlTrans.Rollback("DELETE");
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback("DELETE");
				
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

		//not in use yet
		/*public ExtraHourUsedTO find(int employeeID, DateTime dateEarned)
		{
			DataSet dataSet = new DataSet();
			ExtraHourUsedTO extraHourUsedTO = new ExtraHourUsedTO();
			try
			{
				SqlCommand cmd = new SqlCommand("SELECT employee_id, date_earned, extra_time_amt FROM extra_hours WHERE employee_id = " 
					+ employeeID + " AND date_earned = '" + dateEarned.ToString("yyy-MM-dd").Trim() + "'", conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ExtraHourUsed");
				DataTable table = dataSet.Tables["ExtraHourUsed"];

				if (table.Rows.Count == 1)
				{
					extraHourUsedTO = new ExtraHourUsedTO();

					if (table.Rows[0]["employee_id"] != DBNull.Value)
					{
						extraHourUsedTO.EmployeeID = Int32.Parse(table.Rows[0]["employee_id"].ToString().Trim());
					}					

					if (!table.Rows[0]["date_earned"].Equals(DBNull.Value))
					{
						extraHourUsedTO.DateEarned = (DateTime)table.Rows[0]["date_earned"];
					}

					if (!table.Rows[0]["extra_time_amt"].Equals(DBNull.Value))
					{
						extraHourUsedTO.ExtraTimeAmt = Int32.Parse(table.Rows[0]["extra_time_amt"].ToString().Trim());						
						extraHourTO.CalculatedTimeAmtUsed = Util.Misc.transformMinToStringTime(extraHourUsedTO.ExtraTimeAmtUsed);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return extraHourUsedTO;
		}*/

        public List<ExtraHourUsedTO> getExtraHourUsed(int employeeID, DateTime usedFrom, DateTime usedTo, string type)
		{
			DataSet dataSet = new DataSet();
			ExtraHourUsedTO extraHourUsedTO = new ExtraHourUsedTO();
            List<ExtraHourUsedTO> extraHourUsedList = new List<ExtraHourUsedTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
                sb.Append("SELECT record_id, employee_id, date_earned, date_used, extra_time_amt_used, start_time, end_time, io_pair_id, type, appl_users.name ");
                sb.Append("FROM extra_hours_used, appl_users ");
                sb.Append("WHERE ");
                sb.Append("extra_hours_used.created_by = appl_users.user_id ");

				if (employeeID != -1)
				{					
					sb.Append(" AND employee_id = " + employeeID);

					if (!usedFrom.Equals(new DateTime()))
					{
						sb.Append(" AND date_used >= '" + usedFrom.ToString("yyyy-MM-dd") + "'");
					}
					if (!usedTo.Equals(new DateTime()))
					{
						sb.Append(" AND date_used <= '" + usedTo.ToString("yyyy-MM-dd") + "'");
					}
                    if (type != "")
                    {
                        sb.Append(" AND type = '" + type + "'");
                    }

					select = sb.ToString();
				}
				else
				{
					select = sb.ToString();
				}

				select = select + " ORDER BY date_used ";

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ExtraHourUsed");
				DataTable table = dataSet.Tables["ExtraHourUsed"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						extraHourUsedTO = new ExtraHourUsedTO();

						if (!row["record_id"].Equals(DBNull.Value))
						{
							extraHourUsedTO.RecordID = (int)row["record_id"]; //Int32.Parse(row["employee_id"].ToString().Trim());							
						}
						if (!row["employee_id"].Equals(DBNull.Value))
						{
							extraHourUsedTO.EmployeeID = (int)row["employee_id"]; //Int32.Parse(row["employee_id"].ToString().Trim());							
						}					
						if (!row["date_earned"].Equals(DBNull.Value))
						{
							extraHourUsedTO.DateEarned = (DateTime)row["date_earned"];
						}
						if (!row["date_used"].Equals(DBNull.Value))
						{
							extraHourUsedTO.DateUsed = (DateTime)row["date_used"];
						}
						if (!row["extra_time_amt_used"].Equals(DBNull.Value))
						{
							extraHourUsedTO.ExtraTimeAmtUsed = (int)row["extra_time_amt_used"]; //Int32.Parse(row["extra_time_amt"].ToString().Trim());							
							extraHourUsedTO.CalculatedTimeAmtUsed = Util.Misc.transformMinToStringTime(extraHourUsedTO.ExtraTimeAmtUsed);
						}
						if (!row["start_time"].Equals(DBNull.Value))
						{
							extraHourUsedTO.StartTime = (DateTime)row["start_time"];
						}
						if (!row["end_time"].Equals(DBNull.Value))
						{
							extraHourUsedTO.EndTime = (DateTime)row["end_time"];
						}
						if (!row["io_pair_id"].Equals(DBNull.Value))
						{
							extraHourUsedTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());							
						}
                        if (!row["type"].Equals(DBNull.Value))
                        {
                            extraHourUsedTO.Type = row["type"].ToString().Trim();
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            extraHourUsedTO.CreatedByName = row["name"].ToString().Trim();
                        }

						extraHourUsedList.Add(extraHourUsedTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return extraHourUsedList;
		}

        public List<ExtraHourUsedTO> getExtraHourUsedByType(int emplID, string type)
        {
            DataSet dataSet = new DataSet();
            ExtraHourUsedTO extraHourUsedTO = new ExtraHourUsedTO();
            List<ExtraHourUsedTO> extraHourUsedList = new List<ExtraHourUsedTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT record_id, employee_id, date_earned, date_used, extra_time_amt_used, start_time, end_time, io_pair_id, type, appl_users.name ");
                sb.Append("FROM extra_hours_used, appl_users ");
                sb.Append("WHERE ");
                sb.Append("extra_hours_used.created_by = appl_users.user_id ");


                if (type != "")
                {
                    sb.Append(" AND type = '" + type + "'");
                    sb.Append(" AND employee_id = " + emplID);
                }

                select = sb.ToString();                

                select = select + " ORDER BY date_used ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ExtraHourUsed");
                DataTable table = dataSet.Tables["ExtraHourUsed"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        extraHourUsedTO = new ExtraHourUsedTO();

                        if (!row["record_id"].Equals(DBNull.Value))
                        {
                            extraHourUsedTO.RecordID = (int)row["record_id"]; //Int32.Parse(row["employee_id"].ToString().Trim());							
                        }
                        if (!row["employee_id"].Equals(DBNull.Value))
                        {
                            extraHourUsedTO.EmployeeID = (int)row["employee_id"]; //Int32.Parse(row["employee_id"].ToString().Trim());							
                        }
                        if (!row["date_earned"].Equals(DBNull.Value))
                        {
                            extraHourUsedTO.DateEarned = (DateTime)row["date_earned"];
                        }
                        if (!row["date_used"].Equals(DBNull.Value))
                        {
                            extraHourUsedTO.DateUsed = (DateTime)row["date_used"];
                        }
                        if (!row["extra_time_amt_used"].Equals(DBNull.Value))
                        {
                            extraHourUsedTO.ExtraTimeAmtUsed = (int)row["extra_time_amt_used"]; //Int32.Parse(row["extra_time_amt"].ToString().Trim());							
                            extraHourUsedTO.CalculatedTimeAmtUsed = Util.Misc.transformMinToStringTime(extraHourUsedTO.ExtraTimeAmtUsed);
                        }
                        if (!row["start_time"].Equals(DBNull.Value))
                        {
                            extraHourUsedTO.StartTime = (DateTime)row["start_time"];
                        }
                        if (!row["end_time"].Equals(DBNull.Value))
                        {
                            extraHourUsedTO.EndTime = (DateTime)row["end_time"];
                        }
                        if (!row["io_pair_id"].Equals(DBNull.Value))
                        {
                            extraHourUsedTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        }
                        if (!row["type"].Equals(DBNull.Value))
                        {
                            extraHourUsedTO.Type = row["type"].ToString().Trim();
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            extraHourUsedTO.CreatedByName = row["name"].ToString().Trim();
                        }

                        extraHourUsedList.Add(extraHourUsedTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return extraHourUsedList;
        }

		//not in use any more. This was used for calculating total available hours for employee, 
		//but now, that is calculated by adding rows from listview that contains 
		//available hours for each earned date
        public List<ExtraHourUsedTO> getEmployeeUsedSum(int employeeID)
		{
			DataSet dataSet = new DataSet();
			ExtraHourUsedTO extraHourUsedTO = new ExtraHourUsedTO();
            List<ExtraHourUsedTO> extraHourUsedList = new List<ExtraHourUsedTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT SUM(extra_time_amt_used) EmployeeUsedSum FROM extra_hours_used");

				if (employeeID != -1)
				{
					sb.Append(" WHERE ");					
					sb.Append(" employee_id = " + employeeID);

					select = sb.ToString();
				}
				else
				{
					select = sb.ToString();
				}

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ExtraHourUsed");
				DataTable table = dataSet.Tables["ExtraHourUsed"];

				if (table.Rows.Count == 1)
				{
					foreach(DataRow row in table.Rows)
					{
						extraHourUsedTO = new ExtraHourUsedTO();

						if (!row["EmployeeUsedSum"].Equals(DBNull.Value))
						{
							extraHourUsedTO.EmployeeUsedSum = (int)row["EmployeeUsedSum"]; //Int32.Parse(row["EmployeeUsedSum"].ToString().Trim());							
						}
						extraHourUsedList.Add(extraHourUsedTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return extraHourUsedList;
		}

        public int getEmployeeUsedSumByType(int employeeID, DateTime fromDate, DateTime toDate, string type)
        {
            DataSet dataSet = new DataSet();
            ExtraHourUsedTO extraHourUsedTO = new ExtraHourUsedTO();
            int sum = 0;
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT SUM(extra_time_amt_used) EmployeeUsedSum FROM extra_hours_used");

                if ((employeeID != -1) || ((!fromDate.Equals(new DateTime(0)))
                    && (!toDate.Equals(new DateTime(0)))) || (!type.Trim().Equals("")))
                {
                    sb.Append(" WHERE ");
                    if (employeeID != -1)
                    {
                        sb.Append(" employee_id = " + employeeID + " AND");
                    }

                    if ((!fromDate.Equals(new DateTime(0))) && (!toDate.Equals(new DateTime(0))))
                    {
                        sb.Append(" date_used >= convert(datetime,'" + fromDate.ToString("yyyy-MM-dd") + "', 111) AND ");
                        sb.Append(" date_used < convert(datetime,'" + toDate.AddDays(1).ToString("yyyy-MM-dd") + "', 111) AND");
                    }

                    if (!type.Trim().Equals(""))
                    {
                        sb.Append(" type = N'" + type.Trim().ToUpper() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ExtraHourUsed");
                DataTable table = dataSet.Tables["ExtraHourUsed"];

                if (table.Rows.Count == 1)
                {
                    if (!table.Rows[0]["EmployeeUsedSum"].Equals(DBNull.Value))
                    {
                        sum = Int32.Parse(table.Rows[0]["EmployeeUsedSum"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return sum;
        }

		//not in use any more. This was used for calculating available hours left for each
		//earned date, but now, there is getEmployeeAvailableDates() in ExtraHourDAO, that
		//calculate everything on database side
        public List<ExtraHourUsedTO> getEmployeeUsedSumDateEarned(int employeeID)
		{
			DataSet dataSet = new DataSet();
			ExtraHourUsedTO extraHourUsedTO = new ExtraHourUsedTO();
            List<ExtraHourUsedTO> extraHourUsedList = new List<ExtraHourUsedTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT date_earned, SUM(extra_time_amt_used) EmployeeUsedSum FROM extra_hours_used");

				if (employeeID != -1)
				{
					sb.Append(" WHERE ");					
					sb.Append(" employee_id = " + employeeID);

					select = sb.ToString();
				}
				else
				{
					select = sb.ToString();
				}

				select = select + " GROUP BY date_earned ";

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ExtraHourUsed");
				DataTable table = dataSet.Tables["ExtraHourUsed"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						extraHourUsedTO = new ExtraHourUsedTO();

						if (!row["date_earned"].Equals(DBNull.Value))
						{
							extraHourUsedTO.DateEarned = (DateTime)row["date_earned"];
						}
						if (!row["EmployeeUsedSum"].Equals(DBNull.Value))
						{
							extraHourUsedTO.EmployeeUsedSum = (int)row["EmployeeUsedSum"]; //Int32.Parse(row["EmployeeUsedSum"].ToString().Trim());							
						}
						extraHourUsedList.Add(extraHourUsedTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return extraHourUsedList;
		}

		public bool existOverlap(int employeeID, DateTime dateUsed, DateTime startTime, DateTime endTime)
		{
			bool existOverlap = false;

			DataSet dataSet = new DataSet();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT COUNT(*) FROM extra_hours_used ");

				if (employeeID != -1)
				{
					sb.Append(" WHERE ");					
					sb.Append(" employee_id = " + employeeID);

					if (!dateUsed.Equals(new DateTime()))
					{
						sb.Append(" AND date_used = '" + dateUsed.ToString("yyyy-MM-dd") + "'");
					}
					if ((!startTime.Equals(new DateTime())) && (!endTime.Equals(new DateTime())))
					{
						sb.Append(" AND (");

						sb.Append(" (");
						sb.Append(" CONVERT(VARCHAR(10), start_time, 108) < '" + startTime.ToString("HH:mm:ss") + "'");
						sb.Append(" AND CONVERT(VARCHAR(10), end_time, 108) > '" + startTime.ToString("HH:mm:ss") + "'");
						sb.Append(" )");

						sb.Append(" OR (");
						sb.Append(" CONVERT(VARCHAR(10), start_time, 108) = '" + startTime.ToString("HH:mm:ss") + "'");
						sb.Append(" )");

						sb.Append(" OR (");
						sb.Append(" CONVERT(VARCHAR(10), start_time, 108) > '" + startTime.ToString("HH:mm:ss") + "'");
						sb.Append(" AND CONVERT(VARCHAR(10), start_time, 108) < '" + endTime.ToString("HH:mm:ss") + "'");
						sb.Append(" )");
						
						sb.Append(")");
					}

					select = sb.ToString();
				}
				else
				{
					select = sb.ToString();
				}

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ExtraHourOverlap");
				DataTable table = dataSet.Tables["ExtraHourOverlap"];

				if (table.Rows.Count > 0)
				{
					if (Int32.Parse(table.Rows[0][0].ToString()) > 0)
						existOverlap = true;
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return existOverlap;
		}

		// TODO!!!
        public void serialize(List<ExtraHourUsedTO> extraHourUsedTOList)
		{
			try
			{
				string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLExtraHourUsedFile"];
				Stream stream = File.Open(filename, FileMode.Create);

				ExtraHourUsedTO[] extraHourUsedTOArray = (ExtraHourUsedTO[]) extraHourUsedTOList.ToArray();

				XmlSerializer bformatter = new XmlSerializer(typeof(ExtraHourUsedTO[]));
				bformatter.Serialize(stream, extraHourUsedTOArray);
				stream.Close();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
	}
}
