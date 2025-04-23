using System;
using System.Collections;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Globalization;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using TransferObjects;
using Util;

namespace DataAccess
{
    class MySQLMapDAO:MapDAO
    {
        MySqlConnection conn = null;
        MySqlTransaction _sqlTrans = null;

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
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
                sb.Append(" map_id = " + parentMapID );                  
                 
                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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

        public bool update(int mapID, int parentID, string name, string description, byte[] content)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
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
                if (content!=null)
                {
                    sbUpdate.Append("content = ?Content, ");
                }
               
               
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().ToString().Trim() + "', modified_time = " + " NOW() ");
                sbUpdate.Append("WHERE map_id = '" + mapID.ToString().Trim() + "'");
                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                cmd.Parameters.Add("?Content", MySqlDbType.MediumBlob, content.Length).Value = content;
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                sqlTrans.Commit();

            }
            catch (MySqlException sqlex)
            {
                sqlTrans.Rollback();

                if (sqlex.Number == 1062)
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
            MySqlTransaction trans = null;
            if (doCommit)
            {
                trans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            }
            else
            {
                trans = this.SqlTrans;
            }

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM maps WHERE map_id = '" + MapID.ToString().Trim() + "'");

                MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, trans);
                int res = cmd.ExecuteNonQuery();
                if (res <= 0)
                {
                    isDeleted = false;
                }
                else
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
                        trans.Rollback();
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
                        sb.Append(" map_id = " + mapID + " AND");
                    }
                    if (parentMapID != -1)
                    {
                        sb.Append(" parent_map_id = " + parentMapID + " AND");
                    }
                    if (!name.Equals(""))
                    {
                        sb.Append(" name = N'" + name.ToString().Trim() + "' AND");
                    }
                    if (!description.Equals(""))
                    {
                        sb.Append(" description = N'" + description.ToString().Trim() + "' AND");
                    }
                   

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY name ";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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
            MySqlTransaction sqltrans = null;
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
               
                sbInsert.Append("?Content, ");
                
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");
                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqltrans);
                cmd.Parameters.Add("?Content", MySqlDbType.MediumBlob, content.Length).Value = content;
                int rowsAffected = cmd.ExecuteNonQuery();
                sqltrans.Commit();

                inserted = true;
            }
            catch (MySqlException ex)
            {             
                sqltrans.Rollback();
                if (ex.Number.Equals(1062))
                {
                    throw new Exception("Map with the same Map ID already exist.");
                }
                else
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
                MySqlCommand cmd = new MySqlCommand("SELECT MAX(map_id) AS map_id FROM maps", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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

        public MySQLMapDAO()
		{
			conn = MySQLDAOFactory.getConnection();
		}
        public MySQLMapDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
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
