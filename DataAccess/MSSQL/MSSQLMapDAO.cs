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
    class MSSQLMapDAO:MapDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLMapDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
		}
        public MSSQLMapDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
        }
        public bool update(int mapID, int parentID, string name, string description, byte[] content)
        {
            bool isUpdated = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE maps SET ");
                if (!name.Equals(""))
                {
                    sbUpdate.Append("name = N'" + name + "', ");
                }
                else
                {
                    sbUpdate.Append("name = NULL, ");
                }
                if (!description.Equals(""))
                {
                    sbUpdate.Append("description = N'" + description + "', ");
                }
                else
                {
                    sbUpdate.Append("description = NULL, ");
                }
                if (content != null)
                {
                    sbUpdate.Append("content = @Content,");
                }


                sbUpdate.Append(" modified_by = N'" + DAOController.GetLogInUser().ToString().Trim() + "', modified_time = " + " GETDATE() ");
                sbUpdate.Append("WHERE map_id = " + mapID + "");
                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                cmd.Parameters.Add("@Content", SqlDbType.Image, content.Length).Value = content;
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                sqlTrans.Commit();

            }
            catch (SqlException sqlex)
            {
                sqlTrans.Rollback();

                if (sqlex.Number == 2627)
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message, 12);
                    throw procEx;
                }
                else
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message, 11);
                    throw procEx;
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }


        public bool delete(int MapID, bool doCommit)
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
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM maps WHERE map_id = '" + MapID.ToString().Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, trans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
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

                throw new Exception("Exception: " + ex.Message);
            }

            return isDeleted;
        }

        public MapTO find(int parentMapID)
        {
            DataSet dataSet = new DataSet();
            MapTO mapTO = new MapTO();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM maps ");
                sb.Append(" WHERE ");
                sb.Append(" map_id = " + parentMapID);

                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Maps");
                DataTable table = dataSet.Tables["Maps"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        mapTO = new MapTO();

                        mapTO.MapID = Int32.Parse(row["map_id"].ToString().Trim());
                        mapTO.ParentMapID = Int32.Parse(row["parent_map_id"].ToString().Trim());
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            mapTO.Name = row["name"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            mapTO.Description = row["description"].ToString().Trim();
                        }
                        if (!row["content"].Equals(DBNull.Value))
                        {
                            mapTO.Content = (byte[])row["content"];
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return mapTO;
        }


        public ArrayList getMaps(int mapID, int parentMapID, string name, string description)
        {
            DataSet dataSet = new DataSet();
            MapTO mapTO = new MapTO();
            ArrayList mapsList = new ArrayList();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM maps ");

                if ((mapID != -1) || (parentMapID != -1) || (!name.Trim().Equals("")) || (!description.Trim().Equals("")))
                {
                    sb.Append(" WHERE ");

                    if (mapID != -1)
                    {
                        sb.Append(" map_id = " + mapID + " AND ");
                    }
                    if (parentMapID != -1)
                    {
                        sb.Append(" parent_map_id = " + parentMapID + " AND ");
                    }
                    if (!name.Equals(""))
                    {
                        sb.Append("name = N'" + name.ToString().Trim() + "' AND ");
                    }
                    if (!description.Equals(""))
                    {
                        sb.Append("description = N'" + description.ToString().Trim() + "' AND ");
                    }


                    select = sb.ToString(0, sb.ToString().Length - 4);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY name ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Maps");
                DataTable table = dataSet.Tables["Maps"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        mapTO = new MapTO();

                        mapTO.MapID = Int32.Parse(row["map_id"].ToString().Trim());
                        mapTO.ParentMapID = Int32.Parse(row["parent_map_id"].ToString().Trim());
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            mapTO.Name = row["name"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            mapTO.Description = row["description"].ToString().Trim();
                        }
                        if (!row["content"].Equals(DBNull.Value))
                        {
                            mapTO.Content = (byte[])row["content"];
                        }

                        mapsList.Add(mapTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return mapsList;
        }

        public bool insert(int mapID, string name, string description, byte[] content)
        {
            bool inserted = false;
            SqlTransaction sqltrans = null;
            sqltrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {

                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO maps ");
                sbInsert.Append("(map_id,parent_map_id, name, description, content, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                if (mapID != -1)
                {
                    sbInsert.Append("'" + mapID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (mapID != -1)
                {
                    sbInsert.Append("'" + mapID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (name.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + name.Trim() + "', ");
                }
                if (description.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + description.Trim() + "', ");
                }

                sbInsert.Append("@Content, ");

                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");
                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqltrans);
                cmd.Parameters.Add("@Content", SqlDbType.Image, content.Length).Value = content;
                int rowsAffected = cmd.ExecuteNonQuery();
                sqltrans.Commit();
                if (rowsAffected > 0)
                {
                    inserted = true;
                }
            }
            catch (Exception ex)
            {
                sqltrans.Rollback();
                throw ex;
            }
            return inserted;
        }

        public int findMAXMapID()
        {
            DataSet dataSet = new DataSet();
            int mapID = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT MAX(map_id) AS map_id FROM maps", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Maps");
                DataTable table = dataSet.Tables["Maps"];

                if (table.Rows.Count == 1 && !table.Rows[0]["map_id"].Equals(DBNull.Value))
                {
                    mapID = Int32.Parse(table.Rows[0]["map_id"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return mapID;
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
