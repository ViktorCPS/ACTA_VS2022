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
    class MSSQLVisitorDocFileDAO : VisitorDocFileDAO
    {
        SqlConnection conn = null;
        string database = "";
        SqlTransaction _sqlTrans = null;

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLVisitorDocFileDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
            database = Constants.GetDatabaseString + "_files.actamgr.";
		}
        public MSSQLVisitorDocFileDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            database = Constants.GetDatabaseString + "_files.actamgr.";
        }
        public int insert(int visitID, int docType, byte[] content, bool doCommit)
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
                sbInsert.Append("INSERT INTO " + database + "visitor_doc_files ");
                sbInsert.Append("(visit_id, doc_type, content, created_by, created_time, modified_time) ");
                sbInsert.Append("VALUES (");

                sbInsert.Append("'" + visitID + "', ");
                sbInsert.Append("'" + docType + "', ");

                sbInsert.Append("@Content, ");

                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE(), GETDATE()) ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                cmd.Parameters.Add("@Content", SqlDbType.Image, content.Length).Value = content;

                rowsAffected = cmd.ExecuteNonQuery();
                if (doCommit)
                {
                    sqlTrans.Commit();
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback("INSERT");
                }
                
                throw ex;
            }

            return rowsAffected;
        }

        public VisitorDocFileTO findVisitorDocFileByJMBG(string JMBG)
        {
            DataSet dataSet = new DataSet();
            VisitorDocFileTO visitorDocFileTO = new VisitorDocFileTO();
            string select = "";

            try
            {
                select = "SELECT vdf.* FROM " + database + "visitor_doc_files vdf "
                    + "WHERE vdf.visit_id = (SELECT MAX(v.visit_id) "
                    + "FROM visits v WHERE v.visitor_jmbg LIKE '%" + JMBG.Trim() + "%' "
                    + "AND EXISTS (SELECT vd.* FROM " + database + "visitor_doc_files vd "
                    + "WHERE vd.visit_id = v.visit_id))";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "VisitorDocFile");
                DataTable table = dataSet.Tables["VisitorDocFile"];

                if (table.Rows.Count == 1)
                {
                    visitorDocFileTO = new VisitorDocFileTO();

                    if (!table.Rows[0]["visit_id"].Equals(DBNull.Value))
                    {
                        visitorDocFileTO.VisitID = Int32.Parse(table.Rows[0]["visit_id"].ToString().Trim());
                    }
                    if (!table.Rows[0]["doc_type"].Equals(DBNull.Value))
                    {
                        visitorDocFileTO.DocType = Int32.Parse(table.Rows[0]["doc_type"].ToString().Trim());
                    }
                    if (!table.Rows[0]["content"].Equals(DBNull.Value))
                    {
                        visitorDocFileTO.Content = (byte[])table.Rows[0]["content"];
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return visitorDocFileTO;
        }

        public VisitorDocFileTO findVisitorDocFileByID(string ID)
        {
            DataSet dataSet = new DataSet();
            VisitorDocFileTO visitorDocFileTO = new VisitorDocFileTO();
            string select = "";

            try
            {
                select = "SELECT vdf.* FROM " + database + "visitor_doc_files vdf "
                    + "WHERE vdf.visit_id = (SELECT MAX(v.visit_id) "
                    + "FROM visits v WHERE v.visitor_id LIKE '%" + ID.Trim() + "%' "
                    + "AND EXISTS (SELECT vd.* FROM " + database + "visitor_doc_files vd "
                    + "WHERE vd.visit_id = v.visit_id))";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "VisitorDocFile");
                DataTable table = dataSet.Tables["VisitorDocFile"];

                if (table.Rows.Count == 1)
                {
                    visitorDocFileTO = new VisitorDocFileTO();

                    if (!table.Rows[0]["visit_id"].Equals(DBNull.Value))
                    {
                        visitorDocFileTO.VisitID = Int32.Parse(table.Rows[0]["visit_id"].ToString().Trim());
                    }
                    if (!table.Rows[0]["doc_type"].Equals(DBNull.Value))
                    {
                        visitorDocFileTO.DocType = Int32.Parse(table.Rows[0]["doc_type"].ToString().Trim());
                    }
                    if (!table.Rows[0]["content"].Equals(DBNull.Value))
                    {
                        visitorDocFileTO.Content = (byte[])table.Rows[0]["content"];
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return visitorDocFileTO;
        }

        public VisitorDocFileTO findVisitorDocFileByVisitID(string visitID)
        {
            DataSet dataSet = new DataSet();
            VisitorDocFileTO visitorDocFileTO = new VisitorDocFileTO();
            string select = "";

            try
            {
                select = "SELECT * FROM " + database + "visitor_doc_files "
                    + "WHERE visit_id = '" + visitID.Trim() + "'";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "VisitorDocFile");
                DataTable table = dataSet.Tables["VisitorDocFile"];

                if (table.Rows.Count == 1)
                {
                    visitorDocFileTO = new VisitorDocFileTO();

                    if (!table.Rows[0]["visit_id"].Equals(DBNull.Value))
                    {
                        visitorDocFileTO.VisitID = Int32.Parse(table.Rows[0]["visit_id"].ToString().Trim());
                    }
                    if (!table.Rows[0]["doc_type"].Equals(DBNull.Value))
                    {
                        visitorDocFileTO.DocType = Int32.Parse(table.Rows[0]["doc_type"].ToString().Trim());
                    }
                    if (!table.Rows[0]["content"].Equals(DBNull.Value))
                    {
                        visitorDocFileTO.Content = (byte[])table.Rows[0]["content"];
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return visitorDocFileTO;
        }

        public ArrayList getVisitorDocFilesForDates(DateTime DateFrom, DateTime DateTo)
        {
            DataSet dataSet = new DataSet();
            VisitorDocFileTO visitorDocFileTO = new VisitorDocFileTO();
            ArrayList visitorDocFileList = new ArrayList();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM " + database + "visitor_doc_files ");
                sb.Append("WHERE ");
                if (!DateFrom.Equals(new DateTime(0)))
                {
                    sb.Append("created_time >= '" + DateFrom.ToString("yyyy-MM-dd") + "' AND "); //da li 4y
                }
                if (!DateTo.Equals(new DateTime(0)))
                {
                    sb.Append("created_time <= '" + DateTo.AddDays(1).ToString("yyyy-MM-dd") + "' "); //da li 4y
                    select = sb.ToString();
                }
                else
                {
                    select = sb.ToString().Substring(0, sb.Length - 6);
                }


                select = select + " ORDER BY visit_id, doc_type, created_time";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "VisitorDocFile");
                DataTable table = dataSet.Tables["VisitorDocFile"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        visitorDocFileTO = new VisitorDocFileTO();


                        if (!row["visit_id"].Equals(DBNull.Value))
                        {
                            visitorDocFileTO.VisitID = Int32.Parse(row["visit_id"].ToString().Trim());
                        }
                        if (!row["doc_type"].Equals(DBNull.Value))
                        {
                            visitorDocFileTO.DocType = Int32.Parse(row["doc_type"].ToString().Trim());
                        }
                        if (!row["content"].Equals(DBNull.Value))
                        {
                            visitorDocFileTO.Content = (byte[])row["content"];
                        }
                        if (!row["created_time"].Equals(DBNull.Value))
                        {
                            visitorDocFileTO.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }

                        visitorDocFileList.Add(visitorDocFileTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return visitorDocFileList;
        }

        public bool deleteUntilDate(DateTime createdTime, bool doCommit)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = null;

            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }

            DataSet dataSet = new DataSet();
            VisitorDocFileTO visitorDocFileTO = new VisitorDocFileTO();
            ArrayList visitorsDocFileList = new ArrayList();
            string delete = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("DELETE FROM " + database + "visitor_doc_files ");

                if (!createdTime.Equals(new DateTime(0)))
                {
                    //sekunde
                    sb.Append("WHERE ");
                    sb.Append("created_time <= '" + createdTime.AddDays(1).ToString("yyyy-MM-dd") + "' "); //da li 4y
                }

                delete = sb.ToString();

                SqlCommand cmd = new SqlCommand(delete, conn, sqlTrans);
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
                {
                    sqlTrans.Rollback();
                }
                throw ex;
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
