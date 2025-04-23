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
	/// Summary description for MySQLExtraHourDAO.
	/// </summary>
	public class MySQLExtraHourDAO  : ExtraHourDAO
	{
		MySqlConnection conn = null;
        MySqlTransaction _sqlTrans = null;

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }
		public MySQLExtraHourDAO()
		{
			conn = MySQLDAOFactory.getConnection();
		}
        public MySQLExtraHourDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
        }
		public int insert(int employeeID, DateTime dateEarned, int extraTimeAmt)
		{
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
			int rowsAffected = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO extra_hours ");
				sbInsert.Append("(employee_id, date_earned, extra_time_amt, created_by, created_time) ");
				sbInsert.Append("VALUES (");

				sbInsert.Append("" + employeeID + ", ");

				sbInsert.Append("'" + dateEarned.ToString("yyy-MM-dd") + "', ");				

				sbInsert.Append("" + extraTimeAmt + ", ");

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

        public int insert(int employeeID, DateTime dateEarned, int extraTimeAmt, bool doCommit)
        {
            MySqlTransaction sqlTrans = null;
            if (doCommit)
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
                sbInsert.Append("INSERT INTO extra_hours ");
                sbInsert.Append("(employee_id, date_earned, extra_time_amt, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                sbInsert.Append("" + employeeID + ", ");

                sbInsert.Append("'" + dateEarned.ToString("yyy-MM-dd") + "', ");

                sbInsert.Append("" + extraTimeAmt + ", ");

                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");

                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();
                if (doCommit)
                {
                    sqlTrans.Commit();
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw ex;
            }

            return rowsAffected;
        }		

		public bool update(int employeeID, DateTime dateEarned, int extraTimeAmt)
		{
			bool isUpdated = false;
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

			try
			{
				StringBuilder sbUpdate = new StringBuilder();
				sbUpdate.Append("UPDATE extra_hours SET ");

				if (extraTimeAmt != -1)
				{
					sbUpdate.Append("extra_time_amt = " + extraTimeAmt + ", ");
				}
				else
				{
					//not null
				}
				
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = NOW() ");
				sbUpdate.Append("WHERE employee_id = " + employeeID + " AND date_earned = '" + dateEarned.ToString("yyy-MM-dd").Trim() + "'");				
				
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

		public bool delete(int employeeID, DateTime dateEarned)
		{
			bool isDeleted = false;
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

			try
			{
				StringBuilder sbDelete = new StringBuilder();
				sbDelete.Append("DELETE FROM extra_hours WHERE employee_id = " 
					+ employeeID + " AND date_earned = '" + dateEarned.ToString("yyy-MM-dd").Trim() + "'");
				
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

		public ExtraHourTO find(int employeeID, DateTime dateEarned)
		{
			DataSet dataSet = new DataSet();
			ExtraHourTO extraHourTO = new ExtraHourTO();
			try
			{
				MySqlCommand cmd = new MySqlCommand("SELECT employee_id, date_earned, extra_time_amt FROM extra_hours WHERE employee_id = " 
					+ employeeID + " AND date_earned = '" + dateEarned.ToString("yyy-MM-dd").Trim() + "'", conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ExtraHour");
				DataTable table = dataSet.Tables["ExtraHour"];

				if (table.Rows.Count == 1)
				{
					extraHourTO = new ExtraHourTO();

					if (table.Rows[0]["employee_id"] != DBNull.Value)
					{
						extraHourTO.EmployeeID = Int32.Parse(table.Rows[0]["employee_id"].ToString().Trim());
					}					

					if (!table.Rows[0]["date_earned"].Equals(DBNull.Value))
					{
						extraHourTO.DateEarned = DateTime.Parse(table.Rows[0]["date_earned"].ToString());
					}

					if (!table.Rows[0]["extra_time_amt"].Equals(DBNull.Value))
					{
						extraHourTO.ExtraTimeAmt = Int32.Parse(table.Rows[0]["extra_time_amt"].ToString().Trim());						
						extraHourTO.CalculatedTimeAmt = Util.Misc.transformMinToStringTime(extraHourTO.ExtraTimeAmt);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return extraHourTO;
		}
		
		public List<ExtraHourTO> getExtraHour(int employeeID, DateTime earnedFrom, DateTime earnedTo)
		{
			DataSet dataSet = new DataSet();
			ExtraHourTO extraHourTO = new ExtraHourTO();
            List<ExtraHourTO> extraHourList = new List<ExtraHourTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT employee_id, date_earned, extra_time_amt FROM extra_hours ");

				if (employeeID != -1)
				{
					sb.Append(" WHERE ");					
					sb.Append(" employee_id = " + employeeID);

					if (!earnedFrom.Equals(new DateTime()))
					{
						sb.Append(" AND date_earned >= '" + earnedFrom.ToString("yyyy-MM-dd") + "'");
					}
					if (!earnedTo.Equals(new DateTime()))
					{
						sb.Append(" AND date_earned <= '" + earnedTo.ToString("yyyy-MM-dd") + "'");
					}

					select = sb.ToString();
				}
				else
				{
					select = sb.ToString();
				}

				select = select + " ORDER BY date_earned ";

				MySqlCommand cmd = new MySqlCommand(select, conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ExtraHour");
				DataTable table = dataSet.Tables["ExtraHour"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						extraHourTO = new ExtraHourTO();

						if (!row["employee_id"].Equals(DBNull.Value))
						{
							extraHourTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());							
						}					
						if (!row["date_earned"].Equals(DBNull.Value))
						{
							extraHourTO.DateEarned = DateTime.Parse(row["date_earned"].ToString());
						}
						if (!row["extra_time_amt"].Equals(DBNull.Value))
						{
							extraHourTO.ExtraTimeAmt = Int32.Parse(row["extra_time_amt"].ToString().Trim());							
							extraHourTO.CalculatedTimeAmt = Util.Misc.transformMinToStringTime(extraHourTO.ExtraTimeAmt);
						}
						extraHourList.Add(extraHourTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return extraHourList;
		}

		//not in use any more. This was used for calculating total available hours for employee, 
		//but now, that is calculated by adding rows from listview that contains 
		//available hours for each earned date
        public List<ExtraHourTO> getEmployeeSum(int employeeID)
		{
			DataSet dataSet = new DataSet();
			ExtraHourTO extraHourTO = new ExtraHourTO();
            List<ExtraHourTO> extraHourList = new List<ExtraHourTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				//connector to MySql has some bug when SUM is used, returns byte array
				//sb.Append("SELECT SUM(extra_time_amt) EmployeeSum FROM extra_hours ");
				sb.Append("SELECT extra_time_amt FROM extra_hours ");

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

				sqlDataAdapter.Fill(dataSet, "ExtraHour");
				DataTable table = dataSet.Tables["ExtraHour"];
				
				if (table.Rows.Count > 0)
				{
					int count = 0;
					foreach(DataRow row in table.Rows)
					{
						if (!row["extra_time_amt"].Equals(DBNull.Value))
						{
							count += Int32.Parse(row["extra_time_amt"].ToString().Trim());
						}						
					}
					extraHourTO = new ExtraHourTO();
					extraHourTO.EmployeeSum = count;

					extraHourList.Add(extraHourTO);				
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return extraHourList;
		}

        public List<ExtraHourTO> getEmployeeAvailableDates(int employeeID)
		{
			DataSet dataSet = new DataSet();
			ExtraHourTO extraHourTO = new ExtraHourTO();
            List<ExtraHourTO> extraHourList = new List<ExtraHourTO>();
			string select = "";

			try
			{
				if (employeeID != -1)
				{
					StringBuilder sbExists = new StringBuilder();
					sbExists.Append(" EXISTS (SELECT employee_id FROM extra_hours_used AS eu");
					sbExists.Append(" WHERE extra_hours.employee_id = eu.employee_id");
					sbExists.Append(" AND extra_hours.date_earned = eu.date_earned)");

					StringBuilder sbSUM = new StringBuilder();
					sbSUM.Append(" (SELECT SUM(extra_hours_used.extra_time_amt_used) EmployeeUsedSum");
					sbSUM.Append(" FROM extra_hours_used");
					sbSUM.Append(" WHERE extra_hours.employee_id = extra_hours_used.employee_id");
					sbSUM.Append(" AND extra_hours.date_earned = extra_hours_used.date_earned)");

					StringBuilder sbFrom = new StringBuilder();
					sbFrom.Append(" FROM extra_hours WHERE employee_id = " + employeeID);

					StringBuilder sb = new StringBuilder();
					sb.Append("SELECT extra_hours.date_earned, extra_hours.extra_time_amt");
					sb.Append(sbFrom.ToString());
					sb.Append(" AND NOT");
					sb.Append(sbExists.ToString());
					sb.Append(" UNION");
					sb.Append(" SELECT extra_hours.date_earned, (extra_hours.extra_time_amt -");
					sb.Append(sbSUM.ToString() + ")");
					sb.Append(sbFrom.ToString());
					sb.Append(" AND");
					sb.Append(sbExists.ToString());
					sb.Append(" AND");
					sb.Append(sbSUM.ToString());
					sb.Append(" < extra_hours.extra_time_amt");

					select = sb.ToString();

					MySqlCommand cmd = new MySqlCommand(select, conn );
					MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

					sqlDataAdapter.Fill(dataSet, "ExtraHour");
					DataTable table = dataSet.Tables["ExtraHour"];

					if (table.Rows.Count > 0)
					{
						foreach(DataRow row in table.Rows)
						{
							extraHourTO = new ExtraHourTO();
					
							if (!row["date_earned"].Equals(DBNull.Value))
							{
								extraHourTO.DateEarned = DateTime.Parse(row["date_earned"].ToString());
							}
							if (!row["extra_time_amt"].Equals(DBNull.Value))
							{
								extraHourTO.ExtraTimeAmt = Int32.Parse(row["extra_time_amt"].ToString().Trim());							
								extraHourTO.CalculatedTimeAmt = Util.Misc.transformMinToStringTime(extraHourTO.ExtraTimeAmt);
							}
							extraHourList.Add(extraHourTO);
						}
					}
				} //if (employeeID != -1)
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return extraHourList;
		}

		//not in use any more.
		public int extraHoursCount()
		{
			DataSet dataSet = new DataSet();
			int count = 0;

			try
			{		
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT COUNT(*) count FROM extra_hours");
			
				MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ExtraHour");
				DataTable table = dataSet.Tables["ExtraHour"];

				if (table.Rows.Count > 0)
				{
					count = Int32.Parse(table.Rows[0]["count"].ToString().Trim());
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return count;
		}

		// TODO!!!
        public void serialize(List<ExtraHourTO> extraHourTOList)
		{
			try
			{
				string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLExtraHourFile"];
				Stream stream = File.Open(filename, FileMode.Create);

				ExtraHourTO[] extraHourTOArray = (ExtraHourTO[]) extraHourTOList.ToArray();

				XmlSerializer bformatter = new XmlSerializer(typeof(ExtraHourTO[]));
				bformatter.Serialize(stream, extraHourTOArray);
				stream.Close();
			}
			catch(Exception ex)
			{
				throw ex;
			}
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

	}
}
