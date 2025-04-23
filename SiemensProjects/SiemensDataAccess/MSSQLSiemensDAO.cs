using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Util;
using Common;
using System.IO;
using System.Data;
using TransferObjects;
using System.Collections;
using System.Xml.Serialization;
using System.Xml;

namespace SiemensDataAccess
{
    public class MSSQLSiemensDAO:SiemensDAO
    {
        private static string ConnectionString = "";
        // thread-safe locker
        private static object locker = new object();
        public static SqlConnection connection;
        public static SqlConnection connectionBreza;
        private SqlConnection tsConnection = null;

        public MSSQLSiemensDAO()
        {
            lock (locker)
            {
                // Decrypt user name and password
                //Crypt cr = new Crypt();

                // Decrypt connection string
                if (File.Exists(Constants.SiPassConnPath))
                {
                    StreamReader reader = new StreamReader(Constants.SiPassConnPath);
                    ConnectionString = reader.ReadLine();
                    reader.Close();
                }
             
               // ConnectionString = ConfigurationManager.AppSettings["connectionString"];

               // byte[] buffer = Convert.FromBase64String(ConnectionString);
                //ConnectionString = Util.Misc.decrypt(buffer);

                // ConnectionString contains data provader and it should be ejected from connection string
                int startIndex = -1;

                startIndex = ConnectionString.ToLower().IndexOf("server=");

                if (startIndex >= 0)
                {
                    int connEnd = ConnectionString.ToLower().IndexOf("table");
                    if (connEnd > 0)
                    ConnectionString = ConnectionString.Substring(startIndex,connEnd-startIndex);
                }
              
                //ConnectionString = cr.DecryptConnectionString(ConnectionString);
            }
        }

        public static SqlConnection getConnection()
        {
            lock (locker)
            {
                try
                {
                    if (connection == null)
                    {
                        // Set connection for the first time
                        connection = new SqlConnection(ConnectionString);
                        connection.Open();
                        return connection;
                    }
                    else
                    {
                        // TODO: Check if connection is closed or opened
                        // Connection already established
                        return connection;
                    }
                }
                catch (InvalidOperationException ioex)
                {
                    connection = null;
                    throw ioex;
                }
                catch (SqlException sqlex)
                {
                    connection = null;
                    throw sqlex;
                }
                catch (Exception ex)
                {
                    connection = null;
                    throw ex;
                }
            }
        }

        public override void CloseConnection()
        {
            lock (locker)
            {
                try
                {
                    if (connection != null) connection.Close();
                }
                catch { }
                finally
                {
                    connection = null;
                }
            }
        }

        public override void CloseConnection(Object dbConnection)
        {
            try
            {
                if (dbConnection != null)
                {
                    SqlConnection conn = dbConnection as SqlConnection;
                    if (conn != null)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            catch { }
        }

        public override Object MakeNewDBConnection()
        {
            lock (locker)
            {
                SqlConnection sqlConnection = null;
                try
                {
                    sqlConnection = new SqlConnection(ConnectionString);
                    sqlConnection.Open();
                    return sqlConnection;
                }
                catch (InvalidOperationException ioex)
                {
                    throw ioex;
                }
                catch (SqlException sqlex)
                {
                    throw sqlex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public override void SetDBConnection(Object dbConnection)
        {
            connection = dbConnection as SqlConnection;
        }
     
        public override bool TestDataSourceConnection()
        {
            bool isConnected = false;

            try
            {
                SqlConnection testConn = getConnection();
                if (threadSafe)
                {
                    tsConnection = MakeNewDBConnection() as SqlConnection;
                }
                if (!threadSafe)
                {
                    isConnected = (testConn.State == ConnectionState.Open);
                }
                else
                {
                    isConnected = (((tsConnection != null) && (tsConnection.State == ConnectionState.Open)) &&
                                   (testConn.State == ConnectionState.Open));
                }
            }
            catch
            {
                return isConnected;
            }

            return isConnected;
        }

        public override Dictionary<int, SiemensEmployeeTO> getAllEmployees()
        {
            DataSet dataSet = new DataSet();
            SiemensEmployeeTO emplTO = new SiemensEmployeeTO();
            Dictionary<int, SiemensEmployeeTO> emplDict = new Dictionary<int, SiemensEmployeeTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from " + DataBase + "." + SiPassUser + ".employee");

                select = sb.ToString();

                SqlCommand cmd = new SqlCommand(select, connection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        emplTO = new SiemensEmployeeTO();
                        if (row["emp_id"] != DBNull.Value)
                            emplTO.EmplID = int.Parse(row["emp_id"].ToString().Trim());
                        if (row["emp_no"] != DBNull.Value)
                            emplTO.ID = row["emp_no"].ToString().Trim();
                        if (row["first_name"] != DBNull.Value)
                            emplTO.FirstName = row["first_name"].ToString().Trim();
                        if (row["last_name"] != DBNull.Value)
                            emplTO.LastName = row["last_name"].ToString().Trim();
                        if (row["visitor"] != DBNull.Value)
                            emplTO.Visitor = int.Parse(row["visitor"].ToString().Trim()) == Constants.SiemensVisitorInt;
                        if (row["wkg_id"] != DBNull.Value)
                            emplTO.DepartmentID = int.Parse(row["wkg_id"].ToString().Trim());
                        if (row["card_no"] != DBNull.Value)
                            emplTO.CardNumber = row["card_no"].ToString().Trim();
                        
                        if (!emplDict.ContainsKey(emplTO.EmplID))
                            emplDict.Add(emplTO.EmplID, emplTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return emplDict;
        }

        public override Dictionary<int, SiemensEmployeeTO> getEmployeesNonVisitors()
        {
            DataSet dataSet = new DataSet();
            SiemensEmployeeTO employee = new SiemensEmployeeTO();
            Dictionary<int, SiemensEmployeeTO> emplDict = new Dictionary<int, SiemensEmployeeTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM " + DataBase + "." + SiPassUser + ".employee WHERE visitor = 0");

                select = sb.ToString();
                select += " ORDER BY last_name, first_name ";

                SqlCommand cmd = new SqlCommand(select, connection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        bool validEmployee = true;

                        employee = new SiemensEmployeeTO();
                        if (row["emp_id"] != DBNull.Value)
                        {
                            employee.EmplID = int.Parse(row["emp_id"].ToString().Trim());
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            employee.FirstName = getValidString(row["first_name"].ToString().Trim());
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            employee.LastName = getValidString(row["last_name"].ToString().Trim());
                        }
                        if (row["emp_no"] != DBNull.Value)
                        {
                            employee.ID = getValidString(row["emp_no"].ToString().Trim());
                        }
                        if (row["card_no"] != DBNull.Value)
                        {
                            string str = row["card_no"].ToString().Trim();
                            uint i = 0;
                            validEmployee = uint.TryParse(str, out i);
                        }
                        if (row["cardholder_id"] != DBNull.Value)
                        {
                            employee.CardHolderID = int.Parse(row["cardholder_id"].ToString().Trim());
                        }
                        employee.Visitor = false;
                        if (validEmployee && !emplDict.ContainsKey(employee.EmplID))
                            emplDict.Add(employee.EmplID, employee);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return emplDict;
        }

        public override List<WorkingUnitTO> getAllWorkingUnits()
        {
            DataSet dataSet = new DataSet();
            WorkingUnitTO workUnit = new WorkingUnitTO();
            List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from " + DataBase + "." + SiPassUser + ".work_group");

                select = sb.ToString();
                select += " ORDER BY wkg_id ";

                SqlCommand cmd = new SqlCommand(select, connection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "WU");
                DataTable table = dataSet.Tables["WU"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        
                        workUnit = new WorkingUnitTO();
                        workUnit.WorkingUnitID = Int32.Parse(row["wkg_id"].ToString().Trim());
                        if (row["name"] != DBNull.Value)
                        {
                            workUnit.Name= getValidString(row["name"].ToString().Trim());
                        }                        
                            wuList.Add(workUnit);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return wuList;
        }

        public override bool getCardStatus(string emplNum)
        {
            DataSet dataSet = new DataSet();
            bool active = false;

            try
            {
                string select = "SELECT c.active FROM " + DataBase + "." + SiPassUser + ".card_physical c, " + DataBase + "." + SiPassUser + ".employee e WHERE c.cardholder_id = e.cardholder_id AND e.emp_no = '" 
                    + emplNum.Trim() + "' ORDER BY card_physical_id";
                
                SqlCommand cmd = new SqlCommand(select, connection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Card");
                DataTable table = dataSet.Tables["Card"];

                if (table.Rows.Count > 0)
                {
                    if (table.Rows[table.Rows.Count - 1]["active"] != DBNull.Value)
                    {
                        active = (table.Rows[table.Rows.Count - 1]["active"].ToString().Trim().ToUpper() == Constants.trueValue.Trim().ToUpper());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return active;
        }

        public override bool getCardStatus(int emplID)
        {
            DataSet dataSet = new DataSet();
            bool active = false;

            try
            {
                string select = "SELECT c.active FROM " + DataBase + "." + SiPassUser + ".card_physical c, " + DataBase + "." + SiPassUser + ".employee e WHERE c.cardholder_id = e.cardholder_id AND e.emp_id = '"
                    + emplID.ToString().Trim() + "' ORDER BY card_physical_id";

                SqlCommand cmd = new SqlCommand(select, connection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Card");
                DataTable table = dataSet.Tables["Card"];

                if (table.Rows.Count > 0)
                {
                    if (table.Rows[table.Rows.Count - 1]["active"] != DBNull.Value)
                    {
                        active = (table.Rows[table.Rows.Count - 1]["active"].ToString().Trim().ToUpper() == Constants.trueValue.Trim().ToUpper());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return active;
        }

        public override Dictionary<int, bool> getCardStatuses()
        {
            DataSet dataSet = new DataSet();
            Dictionary <int, bool> cardStatuses = new Dictionary<int, bool>();

            try
            {
                string select = "SELECT c.active, e.emp_id FROM asco4.asco.card_physical c, asco4.asco.employee e WHERE c.cardholder_id = e.cardholder_id ORDER BY e.emp_id, c.card_physical_id";

                SqlCommand cmd = new SqlCommand(select, connection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Card");
                DataTable table = dataSet.Tables["Card"];

                foreach (DataRow row in table.Rows)
                {
                    if (row["emp_id"] != DBNull.Value)
                    {
                        if (!cardStatuses.ContainsKey(int.Parse(row["emp_id"].ToString().Trim())))
                            cardStatuses.Add(int.Parse(row["emp_id"].ToString().Trim()), false);

                        if (row["active"] != DBNull.Value)
                        {
                            cardStatuses[int.Parse(row["emp_id"].ToString().Trim())] = (row["active"].ToString().Trim().ToUpper() == Constants.trueValue.Trim().ToUpper());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return cardStatuses;
        }

        private string getValidString(string siemensString)
        {
            string str = siemensString;
            try
            {
                if (str.Length > 50)
                    str = str.Substring(0, 50);
               str = str.Replace("'", "");
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return str;
        }

        public override List<SiemensLogTO> getNewLogs(string pointNames, int diffLog, Dictionary<int, PointTO> points, Dictionary<int, PointTO> truckPointDict)
        {
            DataSet dataSet = new DataSet();
            SiemensLogTO log = new SiemensLogTO();
            List<SiemensLogTO> logsList = new List<SiemensLogTO>();
            string select = "";

            try
            {
                if (pointNames.Trim() == "" || Constants.SiemensAuditTrailValidStates.Trim() == "")
                    return logsList;

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT AuditTr.*, empl.emp_no, empl.visitor FROM " + DataBase + "." + SiPassUser + "." + AuditTrailTableName + " AuditTr, " + DataBase + "." + SiPassUser + ".employee empl");
                sb.Append(" WHERE");                
                //sb.Append(" AuditTr.message <> '" + Constants.SiemensRegistrationSkipMessage.Trim() + "' AND");
                sb.Append(" AuditTr.emp_id = empl.emp_id AND");
                sb.Append(" AuditTr.pt_id IN (" + pointNames + ") AND");
                sb.Append(" AuditTr.type = " + Constants.SiemensType.ToString() + " AND");
                if(!Constants.SiemensStatesAntiPassBack.Equals(""))
                    sb.Append(" AuditTr.state_id NOT IN (" + Constants.SiemensStatesAntiPassBack.Trim() + ") AND");
                sb.Append(" AuditTr.state_id IN (" + Constants.SiemensAuditTrailValidStates.Trim() + ") AND");
                sb.Append(" AuditTr.at_id > " + diffLog.ToString());                

                select = sb.ToString();
                select += " ORDER BY AuditTr.at_id";
                //select += " ORDER BY AuditTr.pt_id, AuditTr.date_occurred, AuditTr.time_occurred ";
               
                SqlCommand cmd = new SqlCommand(select, connection);
                
                cmd.CommandTimeout = 1500;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Log");
                DataTable table = dataSet.Tables["Log"];                
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        log = new SiemensLogTO();
                        log.LogID = int.Parse(row["at_id"].ToString().Trim());
                        if (row["emp_no"] != DBNull.Value)
                            log.Id = row["emp_no"].ToString().Trim();
                        if (row["date_occurred"] != DBNull.Value && row["time_occurred"] != DBNull.Value)
                        {
                            string date = row["date_occurred"].ToString().Trim();
                            string time = row["time_occurred"].ToString().Trim();
                            int year = int.Parse(date.Substring(0, 4));
                            int month = int.Parse(date.Substring(4, 2));
                            int day = int.Parse(date.Substring(6));
                            int hour = int.Parse(time.Substring(0, 2));
                            int minute = int.Parse(time.Substring(2, 2));
                            int sec = int.Parse(time.Substring(4));
                            log.RegTime = new DateTime(year, month, day, hour, minute, sec);
                        }
                        PointTO point = new PointTO();
                        //if (row["point_name"] != DBNull.Value && points.ContainsKey(row["point_name"].ToString().Trim()))
                        //    point = points[row["point_name"].ToString().Trim()];
                        if (row["pt_id"] != DBNull.Value && points.ContainsKey(int.Parse(row["pt_id"].ToString().Trim())))
                            point = points[int.Parse(row["pt_id"].ToString().Trim())];
                        if (truckPointDict.ContainsKey(point.PointID))
                        {
                            log.RegLoc = point.PointID;
                            log.TruckCandidate = true;
                        }
                        else
                        {
                            log.RegLoc = point.Gate;
                            log.TruckCandidate = false;
                        }
                        log.PointID = point.PointID;
                        if (point.Direction == Constants.SiemensDirectionIn)
                            log.Direction = Constants.SiemensRegDirectionIn;
                        else
                            log.Direction = Constants.SiemensRegDirectionOut;                        
                        if (row["visitor"] != DBNull.Value && int.Parse(row["visitor"].ToString().Trim()) == Constants.SiemensVisitorInt)
                            log.TypeID = Constants.SiemensVisitorType;
                        else
                            log.TypeID = Constants.SiemensEmployeeType;
                        if (row["first_name"] != DBNull.Value)
                            log.Name = row["first_name"].ToString().Trim();
                        if (row["last_name"] != DBNull.Value)
                            log.LastName = row["last_name"].ToString().Trim();
                        log.ReadStatus = Constants.SiemensDefaultReadStatus;
                        if (row["message"] != DBNull.Value)
                            log.Message = row["message"].ToString().Trim();
                        if (row["emp_id"] != DBNull.Value)
                            log.EmplID = int.Parse(row["emp_id"].ToString().Trim());

                        logsList.Add(log);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return logsList;
        }

        public override List<LogTO> getDiferenceLogs(string pointNames, ArrayList points)
        {
            DataSet dataSet = new DataSet();
            LogTO log = new LogTO();
            List<LogTO> logsList = new List<LogTO>();
            ArrayList tables = new ArrayList();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT name ");
                sb.Append("FROM sysobjects ");
                sb.Append("WHERE ");
                sb.Append("name LIKE '%AuditTrail%' ");
                sb.Append("AND ");
                sb.Append("name <> 'AuditTrail' ");
                sb.Append("AND ");
                sb.Append("sysstat & 0xf = 3");

                SqlCommand cmd = new SqlCommand(sb.ToString(), connection);
                cmd.CommandTimeout = 1500;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Table");
                DataTable table = dataSet.Tables["Table"];
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (row["name"] != DBNull.Value)
                        {
                            tables.Add(row["name"].ToString());
                        }
                    }
                }

                foreach (string tableName in tables)
                {
                    StringBuilder sb1 = new StringBuilder();
                    sb1.Append("SELECT card_no,point_name,date_occurred,time_occurred,pt_id ");
                    sb1.Append("FROM "+DataBase + "."+SiPassUser+".["+tableName+ "] AS tbl ");
                    sb1.Append("WHERE ");
                    sb1.Append("tbl.date_occurred >= '"+DateTime.Now.AddDays(-1).ToString("yyyyMMdd")+"' ");
                    sb1.Append(" AND tbl.point_name IN ( " + pointNames + " )AND");
                    sb1.Append(" tbl.type = " + Constants.SiemensType.ToString() + " AND");
                    if (!Constants.SiemensStatesAntiPassBack.Equals(""))
                        sb1.Append(" tbl.state_id NOT IN( " + Constants.SiemensStatesAntiPassBack + " ) AND");
                    sb1.Append(" NOT EXISTS ( " );
                                 sb1.Append("SELECT att.auto_id ");
                                 sb1.Append("FROM "+DataBase + "."+SiPassUser+"." + AuditTrailTableName +" AS att ");
                                 sb1.Append("WHERE ");
                                 sb1.Append("tbl.card_no = att.card_no ");
                                 sb1.Append("AND tbl.point_name = att.point_name ");
                                 sb1.Append("AND tbl.date_occurred = att.date_occurred ");
                                 sb1.Append("AND tbl.time_occurred = att.time_occurred ");
                                 sb1.Append("AND tbl.pt_id = att.pt_id ");
                                 sb1.Append("AND tbl.date_occurred >= '" + DateTime.Now.AddDays(-1).ToString("yyyyMMdd")+"' ");
                                 sb1.Append("AND tbl.type = " + Constants.SiemensType.ToString()+" ");
                                 if (!Constants.SiemensStatesAntiPassBack.Equals(""))
                                     sb1.Append(" AND tbl.state_id NOT IN( " + Constants.SiemensStatesAntiPassBack + " )");
                                 sb1.Append(" AND tbl.point_name IN ( " + pointNames + " ) ");
                    sb1.Append(" )");

                    SqlCommand cmd1 = new SqlCommand(sb1.ToString(), connection);
                    cmd1.CommandTimeout = 1500;

                    SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter(cmd1);

                    DataSet dataSet1 = new DataSet();
                    sqlDataAdapter1.Fill(dataSet1, "Logs");
                    DataTable table1 = dataSet1.Tables["Logs"];
                    PointTO point = new PointTO();
                    if (table1.Rows.Count > 0)
                    {
                        foreach (DataRow row in table1.Rows)
                        {
                            string card = row["card_no"].ToString();
                            if (card.Trim().Equals(""))
                                continue;
                            if (!row["point_name"].Equals(point.PointName))
                            {
                                foreach (PointTO p in points)
                                {
                                    if (p.PointName.Equals(row["point_name"].ToString()))
                                    {
                                        point = p;
                                        break;
                                    }
                                }
                            }
                            log = new LogTO();
                            if (row["card_no"] != DBNull.Value)
                            {
                                uint ui = 0;
                                bool isUint = uint.TryParse(row["card_no"].ToString().Trim(),out ui);
                                if (isUint)
                                    log.TagID = ui;
                                else
                                    continue;

                            }
                            if (row["date_occurred"] != DBNull.Value && row["time_occurred"] != DBNull.Value)
                            {
                                string date = row["date_occurred"].ToString().Trim();
                                string time = row["time_occurred"].ToString().Trim();
                                int year = int.Parse(date.Substring(0, 4));
                                int month = int.Parse(date.Substring(4, 2));
                                int day = int.Parse(date.Substring(6));
                                int hour = int.Parse(time.Substring(0, 2));
                                int minute = int.Parse(time.Substring(2, 2));
                                int sec = int.Parse(time.Substring(4));
                                log.EventTime = new DateTime(year, month, day, hour, minute, sec);
                            }
                            //log.LogID = int.Parse(row["auto_id"].ToString().Trim());  RZZO
                            //log.LogID = int.Parse(row["auto_id"].ToString().Trim());
                            if (row["pt_id"] != DBNull.Value)
                                log.ReaderID = int.Parse(row["pt_id"].ToString().Trim());
                            log.Antenna = Constants.SiemensDirectionIn;
                            log.EventHappened = Constants.SiemensEventHappened;
                            log.ActionCommited = Constants.SiemensActionCommited;
                            log.PassGenUsed = Constants.SiemensPassGanUsed;
                            if (point.Direction == Constants.SiemensDirectionIn)
                                log.Direction = Constants.DirectionIn;
                            else
                                log.Direction = Constants.DirectionOut;

                            logsList.Add(log);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return logsList;
        }

        public override ArrayList getNewPoints()
        {
            DataSet dataSet = new DataSet();
            PointTO point = new PointTO();
            ArrayList pointsList = new ArrayList();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT name, pt_id from " + DataBase + "." + SiPassUser + ".point");

                sb.Append(" WHERE ");
                sb.Append(" type = " + Constants.SiemensType.ToString() + " ");
                //if(!pointNames.Equals(""))
                //   sb.Append(" AND name NOT IN ( " + pointNames + " ) ");

                select = sb.ToString();
                select += " ORDER BY name ";

                SqlCommand cmd = new SqlCommand(select, connection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Points");
                DataTable table = dataSet.Tables["Points"];
                
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        point = new PointTO();
                        point.PointName = row["name"].ToString();
                        point.PointID = int.Parse(row["pt_id"].ToString().Trim());
                        pointsList.Add(point);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pointsList;
        }

        public override ArrayList deserializeMapping(string filePath)
        {
            ArrayList logList = new ArrayList();

            try
            {
                if (File.Exists(filePath))
                {
                    Stream stream = File.OpenRead(filePath);
                    XmlSerializer bformatter = new XmlSerializer(typeof(PointTO[]));
                    PointTO[] deserialized;

                    try
                    {
                        deserialized = (PointTO[])bformatter.Deserialize(stream);
                        logList = ArrayList.Adapter(deserialized);
                    }
                     catch (Exception ex)
                    {
                        stream.Close();
                        throw new DataProcessingException("File: " + filePath + " " + ex.Message + "\n", 3);
                    }

                    stream.Close();
                }
            }
            catch (IOException ioEx)
            {
                throw new DataProcessingException(ioEx + " File: " + filePath, 2);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return logList;
        }

        public override bool serialize(System.Collections.ArrayList PointTOList, string filePath)
        {
            bool isSerialized = false;

            try
            {
                Stream stream = File.Open(filePath, FileMode.Create);
                PointTO[] pointTOArray = (PointTO[])PointTOList.ToArray(typeof(PointTO));

                XmlSerializer bformatter = new XmlSerializer(typeof(PointTO[]));
                bformatter.Serialize(stream, pointTOArray);
                stream.Close();
                isSerialized = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isSerialized;
        }
        
        public override int getDiffLogID(string filePath)
        {
            int i = -1;

            try
            {
                // Create an isntance of XmlTextReader and call Read method to read the file

                XmlTextReader textReader = new XmlTextReader(filePath);

                if (textReader.Read())
                {
                    textReader.MoveToFirstAttribute();
                    textReader.Read();
                    i = int.Parse(textReader.Value.ToString());
                }

                textReader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("MSSQLSiemensDAO.getDiffLog():"+ ex.Message);
            }
            return i;
        }

        public override bool setDiffLogID(int logID, string filePath)
        {
            //int i = -1;
            bool saved = false;
            try
            {
                int value = logID;
                Stream stream = File.Open(filePath, FileMode.Create);
               
                XmlTextWriter textWriter = new XmlTextWriter(stream, Encoding.BigEndianUnicode);
                textWriter.WriteElementString("value", logID.ToString());
                                
                textWriter.Close();
                saved = true;

                stream.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("MSSQLSiemensDAO.setDiffLogID():" + ex.Message);
            }
            return saved;
        }

    }
}
