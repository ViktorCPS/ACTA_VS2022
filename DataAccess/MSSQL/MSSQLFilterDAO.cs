using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using TransferObjects;
using Util;
using System.Runtime.Serialization.Formatters.Binary;

namespace DataAccess
{
    public class MSSQLFilterDAO :FilterDAO
    {
        SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;

		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}
		
		public MSSQLFilterDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
			DAOController.GetInstance();
		}

        public MSSQLFilterDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
        }

        public void SetDBConnection(Object dbConnection)
        {
            conn = dbConnection as SqlConnection;
        }
                
        public FilterTO find(int filterID)
        {
            DataSet dataSet = new DataSet();
            FilterTO filter = new FilterTO();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM appl_users_filters WHERE filter_id = " + filterID , conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Filter");
                DataTable table = dataSet.Tables["Filter"];

                if (table.Rows.Count == 1)
                {
                    filter = new FilterTO();
                    filter.FilterID = Int32.Parse(table.Rows[0]["filter_id"].ToString().Trim());
                    if (table.Rows[0]["appl_menu_item_id"] != DBNull.Value)
                    {
                        filter.ApplMenuItemID = table.Rows[0]["appl_menu_item_id"].ToString().Trim();
                    }
                    if (table.Rows[0]["tab_id"] != DBNull.Value)
                    {
                        filter.TabID = table.Rows[0]["tab_id"].ToString().Trim();
                    }
                    if (table.Rows[0]["user_id"] != DBNull.Value)
                    {
                        filter.UserID = table.Rows[0]["user_id"].ToString().Trim();
                    }
                    if (table.Rows[0]["name"] != DBNull.Value)
                    {
                        filter.FilterName = table.Rows[0]["name"].ToString().Trim();
                    }
                    if (table.Rows[0]["description"] != DBNull.Value)
                    {
                        filter.Description= table.Rows[0]["description"].ToString().Trim();
                    }
                    if (table.Rows[0]["filter_default"] != DBNull.Value)
                    {
                        filter.Default = Int32.Parse(table.Rows[0]["filter_default"].ToString().Trim());
                    }
                    if (table.Rows[0]["filter_xml"] != DBNull.Value)
                    {
                        filter.XmlDocument = table.Rows[0]["filter_xml"].ToString().Trim();
                    }
                    if (table.Rows[0]["created_time"] != DBNull.Value)
                    {
                        filter.CreatedTime = DateTime.Parse(table.Rows[0]["created_time"].ToString().Trim());
                    }
                    
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return filter;
        }
        public ArrayList search(string menuItem, string user, string tabID)
        {
            DataSet dataSet = new DataSet();
            FilterTO filter = new FilterTO();
            ArrayList filters = new ArrayList();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM appl_users_filters WHERE appl_menu_item_id = N'" +menuItem+"' ");
                sb.Append("AND user_id = N'"+user+"'");
                if(!tabID.Equals(""))
                {
                    sb.Append(" AND tab_id = N'"+tabID+"'");
                }
                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Filter");
                DataTable table = dataSet.Tables["Filter"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        filter = new FilterTO();
                        filter.FilterID = Int32.Parse(row["filter_id"].ToString().Trim());
                        if (row["appl_menu_item_id"] != DBNull.Value)
                        {
                            filter.ApplMenuItemID = row["appl_menu_item_id"].ToString().Trim();
                        }
                        if (row["tab_id"] != DBNull.Value)
                        {
                            filter.TabID = row["tab_id"].ToString().Trim();
                        }
                        if (row["user_id"] != DBNull.Value)
                        {
                            filter.UserID = row["user_id"].ToString().Trim();
                        }
                        if (row["name"] != DBNull.Value)
                        {
                            filter.FilterName = row["name"].ToString().Trim();
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            filter.Description = row["description"].ToString().Trim();
                        }
                        if (row["filter_default"] != DBNull.Value)
                        {
                            filter.Default = Int32.Parse(row["filter_default"].ToString().Trim());
                        }
                        if (row["filter_xml"] != DBNull.Value)
                        {
                            filter.XmlDocument = row["filter_xml"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            filter.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        filters.Add(filter);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return filters;
        }

        public string serialize(ArrayList passesTOList)
        {
            string isSerialized = "";

            try
            {
                ControlFilterTO[] passesTOArray = (ControlFilterTO[])passesTOList.ToArray(typeof(ControlFilterTO));
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream mem = new MemoryStream();
                bf.Serialize(mem, passesTOArray);
                isSerialized = Convert.ToBase64String(mem.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSerialized;
        }
       
        public ArrayList deserialize(string xmlDoc)
        {
            ArrayList passesListTO = new ArrayList();
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream mem = new MemoryStream(Convert.FromBase64String(xmlDoc));
                ControlFilterTO[] deserialized = (ControlFilterTO[])bf.Deserialize(mem);
                passesListTO = ArrayList.Adapter(deserialized);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return passesListTO;
        }

        public bool setNoDefaults(string menuItem, string user, string tabID, bool doCommit)
        {
            SqlTransaction sqlTrans = null;
            bool isUpdated = false;

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
                sbUpdate.Append("UPDATE appl_users_filters SET ");
                sbUpdate.Append("filter_default = " + Constants.filterNotDefault + ", ");
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE user_id = N'" + user+"' ");
                sbUpdate.Append("AND appl_menu_item_id = N'" + menuItem + "' ");
                if(!tabID.Equals(""))
                sbUpdate.Append("AND tab_id = N'" + tabID + "' ");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                if (doCommit)
                {
                    sqlTrans.Commit();
                }

            }
            catch (Exception ex)
            {
                if (doCommit)
                    sqlTrans.Rollback("UPDATE");
                else
                    sqlTrans.Rollback();

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool save(FilterTO filterTO, bool doCommit)
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
            bool saved = false;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO appl_users_filters ");
                sbInsert.Append("(user_id, appl_menu_item_id, tab_id, name, description, filter_default, filter_xml, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                if (filterTO.UserID != "")
                {
                    sbInsert.Append("N'"+filterTO.UserID.ToString() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (filterTO.ApplMenuItemID != "")
                {
                    sbInsert.Append("N'"+filterTO.ApplMenuItemID.ToString() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (filterTO.TabID != "")
                {
                    sbInsert.Append("N'"+filterTO.TabID.ToString() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (filterTO.FilterName != "")
                {
                    sbInsert.Append("N'"+filterTO.FilterName.ToString() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (filterTO.Description != "")
                {
                    sbInsert.Append("N'"+filterTO.Description.ToString() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (filterTO.Default != -1)
                {
                    sbInsert.Append(filterTO.Default.ToString() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }                

                if (!filterTO.XmlDocument.Equals(""))
                {
                    sbInsert.Append("N'" + filterTO.XmlDocument + "' , ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }              

                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                    saved = true;
                if(doCommit)
                sqlTrans.Commit();

            }
            catch (Exception ex)
            {
                if(doCommit)
                sqlTrans.Rollback("INSERT");
                throw ex;
               
            }
            return saved;
        }

        public bool update(FilterTO filterTO,int filterID, bool doCommit)
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
            int rowsAffected = 0;
            bool updated = false;

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE appl_users_filters ");
                sbUpdate.Append("SET ");

                if (filterTO.UserID != "")
                {
                    sbUpdate.Append("user_id = N'" + filterTO.UserID.ToString() + "', ");
                }
                else
                {
                    sbUpdate.Append("user_id = NULL, ");
                }
                if (filterTO.ApplMenuItemID != "")
                {
                    sbUpdate.Append("appl_menu_item_id = N'" + filterTO.ApplMenuItemID.ToString() + "', ");
                }
                else
                {
                    sbUpdate.Append("appl_menu_item_id = NULL, ");
                }
                if (filterTO.TabID != "")
                {
                    sbUpdate.Append("tab_id = N'" + filterTO.TabID.ToString() + "', ");
                }
                else
                {
                    sbUpdate.Append("tab_id = NULL, ");
                }
                if (filterTO.FilterName != "")
                {
                    sbUpdate.Append("name = N'" + filterTO.FilterName.ToString() + "', ");
                }
                else
                {
                    sbUpdate.Append("name = NULL, ");
                }
                if (filterTO.Description != "")
                {
                    sbUpdate.Append("description = N'" + filterTO.Description.ToString() + "', ");
                }
                else
                {
                    sbUpdate.Append("description = NULL, ");
                }
                if (filterTO.Default != -1)
                {
                    sbUpdate.Append("filter_default = "+filterTO.Default.ToString() + ", ");
                }
                else
                {
                    sbUpdate.Append("filter_default = NULL, ");
                }

                if (!filterTO.XmlDocument.Equals(""))
                {
                    sbUpdate.Append("filter_xml = N'" + filterTO.XmlDocument + "' , ");
                }
                else
                {
                    sbUpdate.Append("filter_xml = NULL, ");
                }
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE filter_id = '" + filterID.ToString().Trim() + "'"); 
               
                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                    updated = true;
                if (doCommit)
                    sqlTrans.Commit();

            }
            catch (Exception ex)
            {
                if (doCommit)
                    sqlTrans.Rollback("UPDATE");
                throw ex;

            }
            return updated;
        }


        public ArrayList getDefaults(string menuItem, string user, string tabID)
        {
            DataSet dataSet = new DataSet();
            FilterTO filter = new FilterTO();
            ArrayList filters = new ArrayList();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM appl_users_filters WHERE appl_menu_item_id = N'" + menuItem + "' ");
                sb.Append("AND user_id = N'" + user + "'");
                if (!tabID.Equals(""))
                {
                    sb.Append(" AND tab_id = N'" + tabID + "'");
                }
                sb.Append("AND filter_default = " + Constants.filterDefault);
                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Filter");
                DataTable table = dataSet.Tables["Filter"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        filter = new FilterTO();
                        filter.FilterID = Int32.Parse(row["filter_id"].ToString().Trim());
                        if (row["appl_menu_item_id"] != DBNull.Value)
                        {
                            filter.ApplMenuItemID = row["appl_menu_item_id"].ToString().Trim();
                        }
                        if (row["tab_id"] != DBNull.Value)
                        {
                            filter.TabID = row["tab_id"].ToString().Trim();
                        }
                        if (row["user_id"] != DBNull.Value)
                        {
                            filter.UserID = row["user_id"].ToString().Trim();
                        }
                        if (row["name"] != DBNull.Value)
                        {
                            filter.FilterName = row["name"].ToString().Trim();
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            filter.Description = row["description"].ToString().Trim();
                        }
                        if (row["filter_default"] != DBNull.Value)
                        {
                            filter.Default = Int32.Parse(row["filter_default"].ToString().Trim());
                        }
                        if (row["filter_xml"] != DBNull.Value)
                        {
                            filter.XmlDocument = row["filter_xml"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            filter.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        filters.Add(filter);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return filters;
        }

        public bool delete(int filterID)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM appl_users_filters WHERE filter_id = '" + filterID.ToString() + "'");

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    isDeleted = true;
                }
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
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

    }
}
