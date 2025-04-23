using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public class MSSQLSecurityRouteDAO : SecurityRouteDAO
    {
        SqlConnection conn = null;
        protected string dateTimeformat = "";

        public MSSQLSecurityRouteDAO()
		{
			conn = MSSQLDAOFactory.getConnection();	
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MSSQLSecurityRouteDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public int insert(SecurityRouteHdrTO secRouteTO)
        {
            int rowsAffected = 0;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            DataSet dataSet = new DataSet();
            string id = "";

            try
            {
                StringBuilder sbInsert = new StringBuilder();

                // Insert into header table
                sbInsert.Append("SET NOCOUNT ON ");
                sbInsert.Append("INSERT INTO security_routes_hdr (name, description, type, created_by, created_time) ");
                sbInsert.Append("VALUES (N'" + secRouteTO.Name.Trim() + "', N'" + secRouteTO.Description.Trim() +
                    "', N'" + secRouteTO.RouteType.Trim() + "', N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");
                sbInsert.Append("SELECT @@Identity AS security_route_id ");
                sbInsert.Append("SET NOCOUNT OFF ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                sqlDataAdapter.Fill(dataSet, "SecurityRouteID");
                DataTable table = dataSet.Tables["SecurityRouteID"];

                id = ((DataRow)table.Rows[0])["security_route_id"].ToString();

                if ((!id.Equals("")) && (secRouteTO.Segments.Count > 0))
                {
                    sbInsert.Length = 0;
                    SecurityRouteDtlTO detailTO = new SecurityRouteDtlTO();

                    foreach (int key in secRouteTO.Segments.Keys)
                    {
                        detailTO = (SecurityRouteDtlTO)secRouteTO.Segments[key];
                        cmd.CommandText = prepareDetailInsert(detailTO, id);
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

        private string prepareDetailInsert(SecurityRouteDtlTO detail, string security_route_id)
        {
            StringBuilder sbInsert = new StringBuilder();

            // Insert statement
            sbInsert.Append("INSERT INTO security_routes_dtl (");
            sbInsert.Append("security_route_id, segment_id, gate_id, ");
            sbInsert.Append("time_from, time_to, ");
            sbInsert.Append("created_by, created_time) ");
            sbInsert.Append("VALUES (" + security_route_id + ", " + detail.SegmentID + ", " + detail.GateID + ", ");
            sbInsert.Append("'" + detail.TimeFrom.ToString(dateTimeformat) + "', '" + detail.TimeTo.ToString(dateTimeformat) + "', ");
            sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE())");

            return sbInsert.ToString();
        }

        public bool delete(int securityRouteID)
        {
            bool isDeleted = false;
            SqlTransaction trans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                string select = "";

                // Delete form security_routes_dtl
                select = "DELETE FROM security_routes_dtl WHERE security_route_id = '" + securityRouteID + "' ";
                SqlCommand cmd = new SqlCommand(select, conn, trans);
                int affectedRows = cmd.ExecuteNonQuery();

                //Delete from security_routes_hdr
                select = "DELETE FROM security_routes_hdr WHERE security_route_id = '" + securityRouteID + "' ";
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

        public ArrayList getRoutes(string name, string desc)
        {
            DataSet dataSet = new DataSet();
            SecurityRouteHdrTO routeTO = new SecurityRouteHdrTO();
            ArrayList routesList = new ArrayList();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM security_routes_hdr ");

                if ((!name.Trim().Equals("")) || (!desc.Trim().Equals("")))
                {
                    sb.Append("WHERE ");

                    if (!name.Trim().Equals(""))
                    {
                        sb.Append("UPPER(name) LIKE N'" + name.ToUpper().Trim() + "' AND ");
                    }
                    if (!desc.Trim().Equals(""))
                    {
                        sb.Append("UPPER(description) LIKE N'" + desc.ToUpper().Trim() + "' AND ");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 4);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY name";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Routes");
                DataTable table = dataSet.Tables["Routes"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        routeTO = new SecurityRouteHdrTO();

                        routeTO.SecurityRouteID = Int32.Parse(row["security_route_id"].ToString().Trim());
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            routeTO.Name = row["name"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            routeTO.Description = row["description"].ToString().Trim();
                        }

                        routesList.Add(routeTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return routesList;
        }

        public ArrayList getRoutesDetailsTag(int securityRouteID)
        {
            DataSet dataSet = new DataSet();
            SecurityRouteDtlTO routeTO = new SecurityRouteDtlTO();
            ArrayList routesList = new ArrayList();
            string select = "";

            try
            {
                select = "SELECT sr.*, g.name FROM security_routes_dtl sr, gates g "
                    + "WHERE sr.gate_id = g.gate_id ";
                if (securityRouteID != -1)
                {
                    select += "AND sr.security_route_id = '" + securityRouteID + "' ";
                }
                select += "ORDER BY sr.time_from";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Routes");
                DataTable table = dataSet.Tables["Routes"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        routeTO = new SecurityRouteDtlTO();

                        routeTO.SecurityRouteID = Int32.Parse(row["security_route_id"].ToString().Trim());
                        if (!row["segment_id"].Equals(DBNull.Value))
                        {
                            routeTO.SegmentID = Int32.Parse(row["segment_id"].ToString().Trim());
                        }
                        if (!row["gate_id"].Equals(DBNull.Value))
                        {
                            routeTO.GateID = Int32.Parse(row["gate_id"].ToString().Trim());
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            routeTO.GateName = row["name"].ToString().Trim();
                        }
                        if (!row["time_from"].Equals(DBNull.Value))
                        {
                            routeTO.TimeFrom = DateTime.Parse(row["time_from"].ToString().Trim());
                        }
                        if (!row["time_to"].Equals(DBNull.Value))
                        {
                            routeTO.TimeTo = DateTime.Parse(row["time_to"].ToString().Trim());
                        }

                        routesList.Add(routeTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return routesList;
        }

        public ArrayList getRoutesDetailsTerminal(int securityRouteID)
        {
            DataSet dataSet = new DataSet();
            SecurityRouteDtlTO routeTO = new SecurityRouteDtlTO();
            ArrayList routesList = new ArrayList();
            string select = "";

            try
            {
                select = "SELECT sr.*, p.name FROM security_routes_dtl sr, security_routes_points p "
                    + "WHERE sr.gate_id = p.control_point_id ";
                if (securityRouteID != -1)
                {
                    select += "AND sr.security_route_id = '" + securityRouteID + "' ";
                }
                select += "ORDER BY sr.time_from";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Routes");
                DataTable table = dataSet.Tables["Routes"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        routeTO = new SecurityRouteDtlTO();

                        routeTO.SecurityRouteID = Int32.Parse(row["security_route_id"].ToString().Trim());
                        if (!row["segment_id"].Equals(DBNull.Value))
                        {
                            routeTO.SegmentID = Int32.Parse(row["segment_id"].ToString().Trim());
                        }
                        if (!row["gate_id"].Equals(DBNull.Value))
                        {
                            routeTO.GateID = Int32.Parse(row["gate_id"].ToString().Trim());
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            routeTO.GateName = row["name"].ToString().Trim();
                        }
                        if (!row["time_from"].Equals(DBNull.Value))
                        {
                            routeTO.TimeFrom = DateTime.Parse(row["time_from"].ToString().Trim());
                        }
                        if (!row["time_to"].Equals(DBNull.Value))
                        {
                            routeTO.TimeTo = DateTime.Parse(row["time_to"].ToString().Trim());
                        }

                        routesList.Add(routeTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return routesList;
        }
    }
}
