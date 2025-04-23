using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

using TransferObjects;
using Util;
using System.Globalization;

namespace DataAccess
{
    class MSSQLMapsObjectDAO:MapsObjectDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLMapsObjectDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
		}
        public MSSQLMapsObjectDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
        }
        public int insert(MapsObjectHdrTO mapsObjectHdrTO)
        {
            int rowsAffected = 0;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            DataSet dataSet = new DataSet();

            try
            {
                StringBuilder sbInsert = new StringBuilder();

                // Insert into header table
                sbInsert.Append("SET NOCOUNT ON ");
                sbInsert.Append("INSERT INTO maps_object_hdr (object_id, type, map_id, created_by, created_time) ");
                sbInsert.Append("VALUES (" + mapsObjectHdrTO.ObjectID + ", N'" + mapsObjectHdrTO.Type.Trim() + "', " + mapsObjectHdrTO.MapID +
                    ", N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");
                sbInsert.Append("SET NOCOUNT OFF ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                cmd.ExecuteNonQuery();
                
                if ( (mapsObjectHdrTO.Points.Count > 0))
                {
                    sbInsert.Length = 0;
                    MapsObjectDtlTO detailTO = new MapsObjectDtlTO();

                    foreach (int key in mapsObjectHdrTO.Points.Keys)
                    {
                        detailTO = (MapsObjectDtlTO)mapsObjectHdrTO.Points[key];
                        cmd.CommandText = prepareDetailInsert(detailTO);
                        rowsAffected += cmd.ExecuteNonQuery();
                    }

                    sqlTrans.Commit();
                }
                else
                {
                    sqlTrans.Rollback();
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw ex;
            }
            return rowsAffected;
        }
        private string prepareDetailInsert(MapsObjectDtlTO detail)
        {
            StringBuilder sbInsert = new StringBuilder();
         
            // Insert statement
            sbInsert.Append("INSERT INTO maps_object_dtl (");
            sbInsert.Append("object_id, type, point_ord, x_position,y_position, ");
            sbInsert.Append("created_by, created_time) ");
            sbInsert.Append("VALUES (" + detail.ObjectID + ", '" + detail.Type + "', " + detail.PointOrder + ", ");
            sbInsert.Append(detail.X.ToString(CultureInfo.InvariantCulture) + ", " + detail.Y.ToString(CultureInfo.InvariantCulture) + ", ");
            sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE())");

            return sbInsert.ToString();
        }

        public bool delete(int removeObjectID, object removeObjectTtpe)
        {
            bool isDeleted = false;
           SqlTransaction trans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                string select = "";

                // Delete form security_routes_dtl
                select = "DELETE FROM maps_object_dtl WHERE object_id = " + removeObjectID + " AND type = '" + removeObjectTtpe + "' ";
                SqlCommand cmd = new SqlCommand(select, conn, trans);
                int affectedRows = cmd.ExecuteNonQuery();

                //Delete from security_routes_hdr
                select = "DELETE FROM maps_object_hdr WHERE object_id = " + removeObjectID + " AND type = '" + removeObjectTtpe + "' ";
                cmd.CommandText = select;

                affectedRows += cmd.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    isDeleted = true;
                    trans.Commit();
                }
                else
                {
                    trans.Rollback();
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }

            return isDeleted;
        }

        public bool deleteOnMap(int mapID, bool doCommit)
        {
            bool isDeleted = false;
            SqlTransaction trans = null;
            if (doCommit)
            {
                trans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            }
            else
            {
                trans = this.SqlTrans;
            }
            try
            {
                string select = "";

                // Delete form security_routes_dtl
                select = "DELETE FROM maps_object_dtl WHERE object_id IN ( SELECT object_id FROM maps_object_hdr WHERE map_id = " + mapID + ")";
                select += " AND type IN ( SELECT type FROM maps_object_hdr WHERE map_id = " + mapID + ")";
                SqlCommand cmd = new SqlCommand(select, conn, trans);
                int affectedRows = cmd.ExecuteNonQuery();

                //Delete from security_routes_hdr
                select = "DELETE FROM maps_object_hdr WHERE map_id = " + mapID;
                cmd.CommandText = select;

                affectedRows += cmd.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    isDeleted = true;                    
                }
                if (doCommit)
                {
                    if (isDeleted)
                    {
                        trans.Commit();
                    }
                    else
                    {
                        trans.Rollback("DELETE");
                    }
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }

            return isDeleted;
        }

        public ArrayList getObjects(int mapID)
        {
            DataSet dataSet = new DataSet();
            MapsObjectHdrTO objectTO = new MapsObjectHdrTO();
            ArrayList objectsList = new ArrayList();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM maps_object_hdr ");

                if (mapID != -1)
                {
                    sb.Append("WHERE ");

                    sb.Append("map_id = " + mapID);

                }
                select = sb.ToString();
               
                select = select + "ORDER BY type";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Objects");
                DataTable table = dataSet.Tables["Objects"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        objectTO = new MapsObjectHdrTO();

                        objectTO.ObjectID = Int32.Parse(row["object_id"].ToString().Trim());
                        objectTO.Type = row["type"].ToString().Trim();
                        
                        if (!row["map_id"].Equals(DBNull.Value))
                        {
                            objectTO.MapID = int.Parse(row["map_id"].ToString().Trim());
                        }
                        
                        objectsList.Add(objectTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return objectsList;
        }

        public int getObjectsCount(int mapID)
        {
            DataSet dataSet = new DataSet();
            MapsObjectHdrTO objectTO = new MapsObjectHdrTO();
            int count = 0;
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM maps_object_hdr ");

                if (mapID != -1)
                {
                    sb.Append("WHERE ");

                    sb.Append("map_id = " + mapID);

                }
                select = sb.ToString();

                select = select + " ORDER BY type";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Objects");
                DataTable table = dataSet.Tables["Objects"];

                if (table.Rows.Count > 0)
                {
                    count = table.Rows.Count;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return count;
        }

        public ArrayList getPointsDetails(int objectID, string type)
        {
            DataSet dataSet = new DataSet();
            MapsObjectDtlTO objectTO = new MapsObjectDtlTO();
            ArrayList objectsList = new ArrayList();
            string select = "";

            try
            {
                select = "SELECT * FROM maps_object_dtl ";

                select += "WHERE object_id = " + objectID;


                if (!type.Equals(""))
                {
                    select += "AND type = '" + type + "' ";
                }
                select += "ORDER BY object_id,type";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Routes");
                DataTable table = dataSet.Tables["Routes"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        objectTO = new MapsObjectDtlTO();

                        objectTO.ObjectID = Int32.Parse(row["object_id"].ToString().Trim());
                        objectTO.Type = row["type"].ToString().Trim();

                        if (!row["point_ord"].Equals(DBNull.Value))
                        {
                            objectTO.PointOrder = Int32.Parse(row["point_ord"].ToString().Trim());
                        }
                        if (!row["x_position"].Equals(DBNull.Value))
                        {
                            objectTO.X = double.Parse(row["x_position"].ToString().Trim());
                        }
                        if (!row["y_position"].Equals(DBNull.Value))
                        {
                            objectTO.Y = double.Parse(row["y_position"].ToString().Trim());
                        }

                        objectsList.Add(objectTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return objectsList;
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

    }
}
