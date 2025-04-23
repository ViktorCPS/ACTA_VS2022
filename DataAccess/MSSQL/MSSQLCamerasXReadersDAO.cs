using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using System.Globalization;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using TransferObjects;
using Util;

namespace DataAccess
{
    public class MSSQLCamerasXReadersDAO : CamerasXReadersDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLCamerasXReadersDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
		}
        public MSSQLCamerasXReadersDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
        }
        public int insert(int cameraID, int readerID, string directionCovered, bool doCommit)
		{
            SqlTransaction sqlTrans = null;

            if (doCommit)
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
                sbInsert.Append("INSERT INTO cameras_x_readers ");
                sbInsert.Append("(camera_id, reader_id, direction_covered, created_by, created_time) ");
				sbInsert.Append("VALUES (");

                sbInsert.Append("" + cameraID + ", ");
                sbInsert.Append("" + readerID + ", ");
                sbInsert.Append("N'" + directionCovered.Trim() + "', ");

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
                if (doCommit)
                    sqlTrans.Rollback("INSERT");
                else
                    sqlTrans.Rollback();
				throw ex;
			}

			return rowsAffected;
		}

        public bool update(int cameraID, int readerID, string directionCovered, bool doCommit)
		{
			bool isUpdated = false;
            SqlTransaction sqlTrans = null;
            string select = "";

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
                sbUpdate.Append("UPDATE cameras_x_readers SET ");

                if (!directionCovered.Trim().Equals(""))
                {
                    sbUpdate.Append("direction_covered = N'" + directionCovered.Trim() + "', ");
                }

                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = GETDATE() ");

                if ((cameraID != -1) || (readerID != -1))
                {
                    sbUpdate.Append(" WHERE ");

                    if (cameraID != -1)
                    {
                        sbUpdate.Append(" camera_id = " + cameraID + " AND");
                    }
                    if (readerID != -1)
                    {
                        sbUpdate.Append(" reader_id = " + readerID + " AND");
                    }

                    select = sbUpdate.ToString(0, sbUpdate.ToString().Length - 3);
                }
                else
                {
                    select = sbUpdate.ToString();
                }

                SqlCommand cmd = new SqlCommand(select, conn, sqlTrans);
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
                if (doCommit)
                    sqlTrans.Rollback("UPDATE");
                else
                    sqlTrans.Rollback();
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

        public bool delete(int cameraID, int readerID, bool doCommit)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = null;
            string select = "";

            if (doCommit)
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
                sbDelete.Append("DELETE FROM cameras_x_readers");

                if ((cameraID != -1) || (readerID != -1))
                {
                    sbDelete.Append(" WHERE ");

                    if (cameraID != -1)
                    {
                        sbDelete.Append(" camera_id = " + cameraID + " AND");
                    }
                    if (readerID != -1)
                    {
                        sbDelete.Append(" reader_id = " + readerID + " AND");
                    }

                    select = sbDelete.ToString(0, sbDelete.ToString().Length - 3);
                }
                else
                {
                    select = sbDelete.ToString();
                }

                SqlCommand cmd = new SqlCommand(select, conn, sqlTrans);
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
            catch (Exception ex)
            {
                if (doCommit)
                    sqlTrans.Rollback("DELETE");
                else
                    sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

		/*public ExtraHourTO find(int employeeID, DateTime dateEarned)
		{
			DataSet dataSet = new DataSet();
			ExtraHourTO extraHourTO = new ExtraHourTO();
			try
			{
				SqlCommand cmd = new SqlCommand("SELECT employee_id, date_earned, extra_time_amt FROM extra_hours WHERE employee_id = " 
					+ employeeID + " AND date_earned = '" + dateEarned.ToString("yyy-MM-dd").Trim() + "'", conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

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
						extraHourTO.DateEarned = (DateTime)table.Rows[0]["date_earned"];
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
		}*/

        public ArrayList getCamerasXReaders(int cameraID, int readerID, string directionCovered)
		{
			DataSet dataSet = new DataSet();
            CamerasXReadersTO camerasXReadersTO = new CamerasXReadersTO();
            ArrayList camerasXReadersList = new ArrayList();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
                sb.Append("SELECT camera_id, reader_id, direction_covered FROM cameras_x_readers ");

                if ((cameraID != -1) || (readerID != -1) || (!directionCovered.Equals("")))
				{
					sb.Append(" WHERE ");

                    if (cameraID != -1)
                    {
                        sb.Append(" camera_id = " + cameraID + " AND");
                    }
                    if (readerID != -1)
                    {
                        sb.Append(" reader_id = " + readerID + " AND");
                    }
                    if (!directionCovered.Equals(""))
                    {
                        sb.Append(" direction_covered = N'" + directionCovered + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
				}
				else
				{
					select = sb.ToString();
				}

                select = select + " ORDER BY reader_id, camera_id";

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "CamerasXReaders");
                DataTable table = dataSet.Tables["CamerasXReaders"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
                        camerasXReadersTO = new CamerasXReadersTO();

                        if (!row["camera_id"].Equals(DBNull.Value))
                        {
                            camerasXReadersTO.CameraID = Int32.Parse(row["camera_id"].ToString().Trim());							
                        }
                        if (!row["reader_id"].Equals(DBNull.Value))
                        {
                            camerasXReadersTO.ReaderID = Int32.Parse(row["reader_id"].ToString().Trim());
                        }
                        if (!row["direction_covered"].Equals(DBNull.Value))
                        {
                            camerasXReadersTO.DirectionCovered = row["direction_covered"].ToString().Trim();
                        }

                        camerasXReadersList.Add(camerasXReadersTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

            return camerasXReadersList;
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

		// TODO!!!
        public void serialize(ArrayList camerasXReadersTOList)
		{
			try
			{
                string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLCamerasXReadersFile"];
				Stream stream = File.Open(filename, FileMode.Create);

                CamerasXReadersTO[] camerasXReadersTOArray = (CamerasXReadersTO[])camerasXReadersTOList.ToArray(typeof(CamerasXReadersTO));

                XmlSerializer bformatter = new XmlSerializer(typeof(CamerasXReadersTO[]));
                bformatter.Serialize(stream, camerasXReadersTOArray);
				stream.Close();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
    }
}
