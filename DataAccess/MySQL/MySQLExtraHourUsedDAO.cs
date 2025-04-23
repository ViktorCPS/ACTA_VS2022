using System;
using System.Collections;
using System.Collections.Generic;
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
	/// Summary description for MySQLExtraHourUsedDAO.
	/// </summary>
	public class MySQLExtraHourUsedDAO : ExtraHourUsedDAO
	{
		MySqlConnection conn = null;
		MySqlTransaction _sqlTrans = null;

		public MySqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

		public MySQLExtraHourUsedDAO()
		{
			conn = MySQLDAOFactory.getConnection();
		}
        public MySQLExtraHourUsedDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
        }
		public int insert(int employeeID, DateTime dateEarned, DateTime dateUsed, int extraTimeAmtUsed, 
			DateTime startTime, DateTime endTime, Int32 ioPairID, string type, bool doCommit)
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

				sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");

				MySqlCommand cmd = new MySqlCommand( sbInsert.ToString(), conn, sqlTrans );
				rowsAffected = cmd.ExecuteNonQuery();
				if (doCommit)
				{
					sqlTrans.Commit();
				}
			}
			catch(Exception ex)
			{
				if(doCommit)
					sqlTrans.Rollback();
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
				sbUpdate.Append("modified_time = NOW() ");
				sbUpdate.Append("WHERE employee_id = " + employeeID + " AND date_earned = '" + dateEarned.ToString("yyy-MM-dd").Trim() + "'");*/
				
				MySqlCommand cmd = new MySqlCommand( sbUpdate.ToString(), conn, sqlTrans );
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
					sqlTrans.Rollback();
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
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT io_pair_id FROM extra_hours_used WHERE record_id = " + recordID);

				MySqlCommand cmdSelect = new MySqlCommand(sb.ToString(), conn, sqlTrans);
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmdSelect);

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
					MySqlCommand cmd = new MySqlCommand( sbDelete.ToString(), conn, sqlTrans);
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
                        sqlTrans.Rollback();
				}
                else
                    sqlTrans.Rollback();
			}
			catch(Exception ex)
			{
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
			_sqlTrans = (MySqlTransaction)trans;
		}

		//not in use yet
		/*public ExtraHourUsedTO find(int employeeID, DateTime dateEarned)
		{
			DataSet dataSet = new DataSet();
			ExtraHourUsedTO extraHourUsedTO = new ExtraHourUsedTO();
			try
			{
				MySqlCommand cmd = new MySqlCommand("SELECT employee_id, date_earned, extra_time_amt FROM extra_hours WHERE employee_id = " 
					+ employeeID + " AND date_earned = '" + dateEarned.ToString("yyy-MM-dd").Trim() + "'", conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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
						extraHourUsedTO.DateEarned = DateTime.Parse(table.Rows[0]["date_earned"].ToString());
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

				MySqlCommand cmd = new MySqlCommand(select, conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ExtraHourUsed");
				DataTable table = dataSet.Tables["ExtraHourUsed"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						extraHourUsedTO = new ExtraHourUsedTO();

						if (!row["record_id"].Equals(DBNull.Value))
						{
							extraHourUsedTO.RecordID = Int32.Parse(row["record_id"].ToString().Trim());							
						}
						if (!row["employee_id"].Equals(DBNull.Value))
						{
							extraHourUsedTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());							
						}					
						if (!row["date_earned"].Equals(DBNull.Value))
						{
							extraHourUsedTO.DateEarned = DateTime.Parse(row["date_earned"].ToString());							
						}
						if (!row["date_used"].Equals(DBNull.Value))
						{
							extraHourUsedTO.DateUsed = DateTime.Parse(row["date_used"].ToString());
						}
						if (!row["extra_time_amt_used"].Equals(DBNull.Value))
						{
							extraHourUsedTO.ExtraTimeAmtUsed = Int32.Parse(row["extra_time_amt_used"].ToString().Trim());							
							extraHourUsedTO.CalculatedTimeAmtUsed = Util.Misc.transformMinToStringTime(extraHourUsedTO.ExtraTimeAmtUsed);
						}
						if (!row["start_time"].Equals(DBNull.Value))
						{
							extraHourUsedTO.StartTime = DateTime.Parse(row["start_time"].ToString());
						}
						if (!row["end_time"].Equals(DBNull.Value))
						{
							extraHourUsedTO.EndTime = DateTime.Parse(row["end_time"].ToString());
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

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ExtraHourUsed");
                DataTable table = dataSet.Tables["ExtraHourUsed"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        extraHourUsedTO = new ExtraHourUsedTO();

                        if (!row["record_id"].Equals(DBNull.Value))
                        {
                            extraHourUsedTO.RecordID = Int32.Parse(row["record_id"].ToString().Trim());
                        }
                        if (!row["employee_id"].Equals(DBNull.Value))
                        {
                            extraHourUsedTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (!row["date_earned"].Equals(DBNull.Value))
                        {
                            extraHourUsedTO.DateEarned = DateTime.Parse(row["date_earned"].ToString());
                        }
                        if (!row["date_used"].Equals(DBNull.Value))
                        {
                            extraHourUsedTO.DateUsed = DateTime.Parse(row["date_used"].ToString());
                        }
                        if (!row["extra_time_amt_used"].Equals(DBNull.Value))
                        {
                            extraHourUsedTO.ExtraTimeAmtUsed = Int32.Parse(row["extra_time_amt_used"].ToString().Trim());
                            extraHourUsedTO.CalculatedTimeAmtUsed = Util.Misc.transformMinToStringTime(extraHourUsedTO.ExtraTimeAmtUsed);
                        }
                        if (!row["start_time"].Equals(DBNull.Value))
                        {
                            extraHourUsedTO.StartTime = DateTime.Parse(row["start_time"].ToString());
                        }
                        if (!row["end_time"].Equals(DBNull.Value))
                        {
                            extraHourUsedTO.EndTime = DateTime.Parse(row["end_time"].ToString());
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
				//connector to MySql has some bug when SUM is used, returns byte array
				//sb.Append("SELECT SUM(extra_time_amt_used) EmployeeUsedSum FROM extra_hours_used");
				sb.Append("SELECT extra_time_amt_used FROM extra_hours_used");

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

				MySqlCommand cmd = new MySqlCommand(select, conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ExtraHourUsed");
				DataTable table = dataSet.Tables["ExtraHourUsed"];

				if (table.Rows.Count > 0)
				{
					int count = 0;
					foreach(DataRow row in table.Rows)
					{
						if (!row["extra_time_amt_used"].Equals(DBNull.Value))
						{
							count += Int32.Parse(row["extra_time_amt_used"].ToString().Trim());
						}
					}
					extraHourUsedTO = new ExtraHourUsedTO();
				    extraHourUsedTO.EmployeeUsedSum = count;

					extraHourUsedList.Add(extraHourUsedTO);
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
                //connector to MySql has some bug when SUM is used, returns byte array
                //sb.Append("SELECT SUM(extra_time_amt_used) EmployeeUsedSum FROM extra_hours_used");
                sb.Append("SELECT extra_time_amt_used FROM extra_hours_used");

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
                        sb.Append(" date_used >= convert('" + fromDate.ToString("yyyy-MM-dd") + "', datetime) AND ");
                        sb.Append(" date_used < convert('" + toDate.AddDays(1).ToString("yyyy-MM-dd") + "', datetime) AND");
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

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ExtraHourUsed");
                DataTable table = dataSet.Tables["ExtraHourUsed"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (!row["extra_time_amt_used"].Equals(DBNull.Value))
                        {
                            sum += Int32.Parse(row["extra_time_amt_used"].ToString().Trim());
                        }
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
				//connector to MySql has some bug when SUM is used, returns byte array
				//sb.Append("SELECT date_earned, SUM(extra_time_amt_used) EmployeeUsedSum FROM extra_hours_used");
				sb.Append("SELECT date_earned, extra_time_amt_used FROM extra_hours_used");

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

				select = select + " ORDER BY date_earned ";

				MySqlCommand cmd = new MySqlCommand(select, conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ExtraHourUsed");
				DataTable table = dataSet.Tables["ExtraHourUsed"];

				if (table.Rows.Count > 0)
				{
					int count = 0;
					DateTime previousDateEarned = new DateTime();
					foreach(DataRow row in table.Rows)
					{
						if (!row["date_earned"].Equals(DBNull.Value))
						{
							if ((previousDateEarned.Equals(new DateTime())) ||
								previousDateEarned.Equals(DateTime.Parse(row["date_earned"].ToString())))
							{
								if (!row["extra_time_amt_used"].Equals(DBNull.Value))
								{
									count += Int32.Parse(row["extra_time_amt_used"].ToString().Trim());
								}
							}
							else
							{
								extraHourUsedTO = new ExtraHourUsedTO();

								extraHourUsedTO.DateEarned = previousDateEarned;
								extraHourUsedTO.EmployeeUsedSum = count;

								extraHourUsedList.Add(extraHourUsedTO);
								count = Int32.Parse(row["extra_time_amt_used"].ToString().Trim());
							}

							previousDateEarned = DateTime.Parse(row["date_earned"].ToString());
						}												
					}
					if ((extraHourUsedList.Count == 0 && !previousDateEarned.Equals(new DateTime()))
						|| (extraHourUsedList.Count > 0 && !(((ExtraHourUsedTO)extraHourUsedList[extraHourUsedList.Count - 1]).DateEarned.Equals(previousDateEarned))))
					{
						extraHourUsedTO = new ExtraHourUsedTO();

						extraHourUsedTO.DateEarned = previousDateEarned;
						extraHourUsedTO.EmployeeUsedSum = count;

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
						sb.Append(" DATE_FORMAT(start_time,'%H:%i:%s') < '" + startTime.ToString("HH:mm:ss") + "'");
						sb.Append(" AND DATE_FORMAT(end_time,'%H:%i:%s') > '" + startTime.ToString("HH:mm:ss") + "'");
						sb.Append(" )");

						sb.Append(" OR (");
						sb.Append(" DATE_FORMAT(start_time,'%H:%i:%s') = '" + startTime.ToString("HH:mm:ss") + "'");
						sb.Append(" )");

						sb.Append(" OR (");
						sb.Append(" DATE_FORMAT(start_time,'%H:%i:%s') > '" + startTime.ToString("HH:mm:ss") + "'");
						sb.Append(" AND DATE_FORMAT(start_time,'%H:%i:%s') < '" + endTime.ToString("HH:mm:ss") + "'");
						sb.Append(" )");
						
						sb.Append(")");
					}

					select = sb.ToString();
				}
				else
				{
					select = sb.ToString();
				}

				MySqlCommand cmd = new MySqlCommand(select, conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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
