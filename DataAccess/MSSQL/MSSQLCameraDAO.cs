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
    public class MSSQLCameraDAO : CameraDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLCameraDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
		}
        public MSSQLCameraDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
        }
        public int insert(int cameraID, string connAddress, string description, 
            string type, bool doCommit)
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
                sbInsert.Append("INSERT INTO cameras ");
                sbInsert.Append("(camera_id, conn_address, description, type, created_by, created_time) ");
				sbInsert.Append("VALUES (");

                sbInsert.Append("" + cameraID + ", ");
                sbInsert.Append("N'" + connAddress.Trim() + "', ");
                sbInsert.Append("N'" + description.Trim() + "', ");
                sbInsert.Append("N'" + type.Trim() + "', ");

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

        public bool update(int cameraID, string connAddress, string description,
            string type, bool doCommit)
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
                sbUpdate.Append("UPDATE cameras SET ");

                if (!connAddress.Trim().Equals(""))
                {
                    sbUpdate.Append("conn_address = N'" + connAddress.Trim() + "', ");
                }
                if (!description.Trim().Equals(""))
                {
                    sbUpdate.Append("description = N'" + description.Trim() + "', ");
                }
                if (!type.Trim().Equals(""))
                {
                    sbUpdate.Append("type = N'" + type.Trim() + "', ");
                }
                
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = GETDATE() ");

                if (cameraID != -1)
                {
                    sbUpdate.Append(" WHERE ");
                    sbUpdate.Append(" camera_id = " + cameraID);
                }

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
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

        public bool delete(int cameraID, bool doCommit)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = null;
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
                sbDelete.Append("DELETE FROM cameras WHERE camera_id = " + cameraID);

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

        public ArrayList getCameras(int cameraID, string connAddress, string description,
            string type)
		{
			DataSet dataSet = new DataSet();
            CameraTO cameraTO = new CameraTO();
            ArrayList cameraList = new ArrayList();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
                sb.Append("SELECT camera_id, conn_address, description, type FROM cameras ");

                if ((cameraID != -1) || (!connAddress.Equals("")) ||
                    (!description.Equals("")) || (!type.Equals("")))
				{
					sb.Append(" WHERE ");

                    if (cameraID != -1)
                    {
                        sb.Append(" camera_id = " + cameraID + " AND");
                    }
                    if (!connAddress.Equals(""))
                    {
                        sb.Append(" conn_address = N'" + connAddress + "' AND");
                    }
                    if (!description.Equals(""))
                    {
                        sb.Append(" description = N'" + description + "' AND");
                    }
                    if (!type.Equals(""))
                    {
                        sb.Append(" type = N'" + type + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
				}
				else
				{
					select = sb.ToString();
				}

                select = select + " ORDER BY camera_id";

				SqlCommand cmd = new SqlCommand(select, conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Cameras");
                DataTable table = dataSet.Tables["Cameras"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
                        cameraTO = new CameraTO();

                        if (!row["camera_id"].Equals(DBNull.Value))
                        {
                            cameraTO.CameraID = Int32.Parse(row["camera_id"].ToString().Trim());							
                        }
                        if (!row["conn_address"].Equals(DBNull.Value))
                        {
                            cameraTO.ConnAddress = row["conn_address"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            cameraTO.Description = row["description"].ToString().Trim();
                        }
                        if (!row["type"].Equals(DBNull.Value))
                        {
                            cameraTO.Type= row["type"].ToString().Trim();
                        }

                        cameraList.Add(cameraTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

            return cameraList;
		}

        public int getCameraNextID()
        {
            DataSet dataSet = new DataSet();
            int cameraID = 0;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT MAX(camera_id) AS CameraID FROM cameras ");

                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "CameraID");
                DataTable table = dataSet.Tables["CameraID"];

                if (table.Rows.Count > 0)
                {
                    if (!table.Rows[0]["CameraID"].Equals(DBNull.Value))
                    {
                        cameraID = Int32.Parse(table.Rows[0]["CameraID"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return (cameraID + 1);
        }

        public ArrayList getCamerasOnGate(int gateID)
        {
            DataSet dataSet = new DataSet();
            CameraTO cameraTO = new CameraTO();
            ArrayList cameraList = new ArrayList();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT cameras.camera_id, cameras.conn_address, cameras.description, gates.name ");
                sb.Append(" FROM gates, readers, cameras_x_readers, cameras ");
                sb.Append(" WHERE ");

                if (gateID != -1)
                {
                    sb.Append(" gates.gate_id = " + gateID + " AND");
                }

                sb.Append(" (readers.ant_0_gate_id = gates.gate_id OR readers.ant_1_gate_id = gates.gate_id ) AND");
                sb.Append(" cameras_x_readers.reader_id = readers.reader_id AND");
                sb.Append(" cameras.camera_id = cameras_x_readers.camera_id");
                sb.Append(" ORDER BY cameras.camera_id");

                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Cameras");
                DataTable table = dataSet.Tables["Cameras"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        cameraTO = new CameraTO();

                        if (!row["camera_id"].Equals(DBNull.Value))
                        {
                            cameraTO.CameraID = Int32.Parse(row["camera_id"].ToString().Trim());
                        }
                        if (!row["conn_address"].Equals(DBNull.Value))
                        {
                            cameraTO.ConnAddress = row["conn_address"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            cameraTO.Description = row["description"].ToString().Trim();
                        }
                        //put gates name in this field
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            cameraTO.Type = row["name"].ToString().Trim();
                        }

                        cameraList.Add(cameraTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return cameraList;
        }

        public ArrayList getCamerasForMap(int mapID)
        {
            DataSet dataSet = new DataSet();
            CameraTO cameraTO = new CameraTO();
            ArrayList cameraList = new ArrayList();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM cameras");
                sb.Append(" WHERE ");
                sb.Append("camera_id NOT IN (");
                sb.Append("SELECT object_id FROM maps_object_hdr WHERE type = 'CAMERA'");
                if (mapID != -1)
                {
                    sb.Append("AND map_id = " + mapID + " )");
                }
                else
                {
                    sb.Append(")");
                } 
                sb.Append(" ORDER BY cameras.camera_id");

                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Cameras");
                DataTable table = dataSet.Tables["Cameras"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        cameraTO = new CameraTO();

                        if (!row["camera_id"].Equals(DBNull.Value))
                        {
                            cameraTO.CameraID = Int32.Parse(row["camera_id"].ToString().Trim());
                        }
                        if (!row["conn_address"].Equals(DBNull.Value))
                        {
                            cameraTO.ConnAddress = row["conn_address"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            cameraTO.Description = row["description"].ToString().Trim();
                        }
                       

                        cameraList.Add(cameraTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return cameraList;
        }

        public ArrayList getCamerasForReaders(string  readerID, string direction)
        {
            DataSet dataSet = new DataSet();
            CameraTO cameraTO = new CameraTO();
            ArrayList cameraList = new ArrayList();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT cameras.camera_id, cameras.conn_address, cameras.description ");
                sb.Append(" FROM  cameras_x_readers, cameras ");
                sb.Append(" WHERE ");

                if (!readerID.Equals(""))
                {
                    sb.Append(" cameras_x_readers.reader_id IN ( " + readerID + " ) AND");
                }
                if (!direction.Equals(""))
                {
                    if (!direction.Contains("%"))
                        sb.Append(" cameras_x_readers.direction_covered = '" + direction + "' AND");
                    else
                        sb.Append(" cameras_x_readers.direction_covered LIKE '" + direction + "' AND");
                }
                sb.Append(" cameras.camera_id = cameras_x_readers.camera_id");
                sb.Append(" ORDER BY cameras.camera_id");

                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Cameras");
                DataTable table = dataSet.Tables["Cameras"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        cameraTO = new CameraTO();

                        if (!row["camera_id"].Equals(DBNull.Value))
                        {
                            cameraTO.CameraID = Int32.Parse(row["camera_id"].ToString().Trim());
                        }
                        if (!row["conn_address"].Equals(DBNull.Value))
                        {
                            cameraTO.ConnAddress = row["conn_address"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            cameraTO.Description = row["description"].ToString().Trim();
                        }
                       
                        cameraList.Add(cameraTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return cameraList;
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
        public void serialize(ArrayList cameraTOList)
		{
			try
			{
                string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLCameraFile"];
				Stream stream = File.Open(filename, FileMode.Create);

                CameraTO[] cameraTOArray = (CameraTO[])cameraTOList.ToArray(typeof(CameraTO));

                XmlSerializer bformatter = new XmlSerializer(typeof(CameraTO[]));
                bformatter.Serialize(stream, cameraTOArray);
				stream.Close();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
    }
}
