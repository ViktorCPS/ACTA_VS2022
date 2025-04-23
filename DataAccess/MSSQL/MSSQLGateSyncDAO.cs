using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using TransferObjects;
using Util;

namespace DataAccess
{
    /// <summary>
    /// Summary description for MSSQLGateSyncDAO.
    /// </summary>
    public class MSSQLGateSyncDAO : GateSyncDAO
    {
        SqlConnection conn = null;
        string database = "";

        public MSSQLGateSyncDAO()
        {
            database = Constants.GetDatabaseString + "_files.actamgr.";
        }
        public MSSQLGateSyncDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            database = Constants.GetDatabaseString + "_files.actamgr.";
        }
        public void SetDBConnection(Object dbConnection)
        {
            conn = dbConnection as SqlConnection;
        }

        public bool Insert(string gateID, string readerControlRequest, string monitoring)
        {
            bool isInserted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO " + database + "gates_sync ");
                sbInsert.Append("(gate_id, reader_control_request, monitoring, created_by, created_time, modified_time) ");
                sbInsert.Append("VALUES ('");

                sbInsert.Append(gateID + "', '");
                sbInsert.Append(readerControlRequest + "', '");
                sbInsert.Append(monitoring + "', ");
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE(), GETDATE()) ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);

                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isInserted = true;
                }
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw ex;
            }

            return isInserted;
        }

        public bool Update(string gateID, string readerControlRequest, string monitoring)
        {
            bool isUpdated = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE " + database + "gates_sync SET ");

                if (!readerControlRequest.Equals(""))
                {
                    sbUpdate.Append("reader_control_request = '" + readerControlRequest + "', ");
                }

                if (!monitoring.Equals(""))
                {
                    sbUpdate.Append("monitoring = '" + monitoring + "', ");
                }

                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");

                if (gateID != "-1")
                {
                    sbUpdate.Append(" WHERE ");
                    sbUpdate.Append(" gate_id = " + gateID);
                }

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);

                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool Delete(string gateID)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM " + database + "gates_sync ");

                if (gateID != "-1")
                {
                    sbDelete.Append(" WHERE ");
                    sbDelete.Append(" gate_id = " + gateID);
                }

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);

                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isDeleted = true;
                }
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public ArrayList Search(string gateID)
        {
            DataSet dataSet = new DataSet();
            GateSyncTO gateSyncTO = new GateSyncTO();
            ArrayList gateSyncList = new ArrayList();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM " + database + "gates_sync ");

                if (gateID != "-1")
                {
                    sb.Append(" WHERE ");
                    sb.Append(" gate_id = " + gateID);
                }

                select = sb.ToString() + " ORDER BY gate_id";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "GatesSync");
                DataTable table = dataSet.Tables["GatesSync"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        gateSyncTO = new GateSyncTO();

                        if (!row["gate_id"].Equals(DBNull.Value))
                        {
                            gateSyncTO.GateID = Int32.Parse(row["gate_id"].ToString().Trim());
                        }
                        if (!row["reader_control_request"].Equals(DBNull.Value))
                        {
                            gateSyncTO.ReaderControlRequest = (string)row["reader_control_request"];
                        }
                        if (!row["monitoring"].Equals(DBNull.Value))
                        {
                            gateSyncTO.Monitoring = (string)row["monitoring"];
                        }

                        gateSyncList.Add(gateSyncTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return gateSyncList;
        }

        public int Count(string gateID)
        {
            DataSet dataSet = new DataSet();
            int count = -1;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT COUNT(*) AS count FROM " + database + "gates_sync");

                if (gateID != "-1")
                {
                    sb.Append(" WHERE");
                    sb.Append(" gate_id = " + gateID);
                }

                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "GatesSync");
                DataTable table = dataSet.Tables["GatesSync"];

                if (table.Rows.Count >= 0)
                {
                    count = Int32.Parse(table.Rows[0]["count"].ToString().Trim());
                }
            }
            catch
            {
                count = -1;
            }

            return count;
        }
    }
}
