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
using System.Globalization;

namespace SiemensDataAccess
{
   public class MSSQLBrezaDAO:BrezaDAO
    {
        private static string ConnectionString = "";
        // thread-safe locker
        private static object locker = new object();
        public static SqlConnection connection;
        private SqlConnection tsConnection = null;
        SqlTransaction _sqlTrans = null;

        protected string dateTimeformat = "";
        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public override void SetDBConnection(Object dbConnection)
        {
            connection = dbConnection as SqlConnection;
        }

        public MSSQLBrezaDAO()
        {
            lock (locker)
            {
                // Decrypt user name and password
                //Crypt cr = new Crypt();

                // Decrypt connection string
                if (File.Exists(Constants.SiPassConnPathBreza))
                {
                    StreamReader reader = new StreamReader(Constants.SiPassConnPathBreza);
                    ConnectionString = reader.ReadLine();
                    reader.Close();
                }

                DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;                
                dateTimeformat = dateTimeFormat.SortableDateTimePattern;
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

        public override uint insert(SiemensLogTO log, bool doCommit)
        {
            uint PK = 0;
            DataSet dataSet = new DataSet();
            SqlTransaction sqltrans = null;

            if (doCommit)
                sqltrans = connection.BeginTransaction(IsolationLevel.RepeatableRead);
            else
                sqltrans = this.SqlTrans;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO " + DataBase + "." + SiPassUser + "." + AuditTrailTableName);
                sbInsert.Append("(ID, RegTime, RegLoc, Direction, TypeID, Name, LastName, Col1, ReadStatus) ");
                sbInsert.Append("VALUES (");                
                sbInsert.Append("N'" + getValidStringField(log.Id.Trim().PadLeft(5,'0'), 20) + "', ");
                if (log.RegTime != new DateTime())
                    sbInsert.Append("'" + log.RegTime.ToString(dateTimeformat) + "', ");
                else
                    sbInsert.Append("GETDATE(), ");
                sbInsert.Append(log.RegLoc.ToString() + ", ");
                sbInsert.Append("N'" + getValidStringField(log.Direction.Trim(), 1) + "', ");
                sbInsert.Append("N'" + getValidStringField(log.TypeID, 1) + "', ");
                sbInsert.Append("N'" + getValidStringField(log.Name.Trim(), 30) + "', ");
                sbInsert.Append("N'" + getValidStringField(log.LastName.Trim(), 30) + "', ");
                if (log.Col1.Trim() != "")
                    sbInsert.Append("N'" + getValidStringField(log.Col1.Trim(), 20) + "', ");
                else
                    sbInsert.Append("NULL, ");
                sbInsert.Append("0)");
                
                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), connection, sqltrans);
                if (cmd.ExecuteNonQuery() == 1)
                {
                    SqlCommand identityCmd = new SqlCommand("SELECT IDENT_CURRENT('IT_Reg') AS PK", connection, sqltrans);
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(identityCmd);
                    sqlAdapter.Fill(dataSet, "Reg");
                    DataTable dataTable = dataSet.Tables["Reg"];
                                        
                    if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["PK"] != DBNull.Value)
                    {
                        if (!uint.TryParse(dataTable.Rows[0]["PK"].ToString().Trim(), out PK))
                            PK = 0;
                    }
                }

                if (doCommit)                
                    sqltrans.Commit();                
            }
            catch (Exception ex)
            {
                if (doCommit)                
                    sqltrans.Rollback();
                
                throw ex;
            }

            return PK;
        }

        public override int insertTruckLog(SiemensTruckLogTO log, bool doCommit)
        {
            int id = 0;
            DataSet dataSet = new DataSet();
            SqlTransaction sqltrans = null;
            if (doCommit)
                sqltrans = connection.BeginTransaction(IsolationLevel.RepeatableRead);
            else
                sqltrans = this.SqlTrans;

            try
            {
                StringBuilder sbInsert = new StringBuilder();                

                sbInsert.Append("INSERT INTO " + DataBase + "." + SiPassUser + ".IT_kamioni");
                sbInsert.Append("(kamion_registracija, vozac_ime, vozac_prezime, kupac_naziv, spediter_naziv, porudzbenica, vreme_registracije, tip_registracije, lokacija_registracije, read_status) ");
                sbInsert.Append("VALUES (");
                //sbInsert.Append("'" + log.ID.ToString().Trim() + "', ");
                if (log.TruckRegistration.Trim() != "")
                    sbInsert.Append("N'" + getValidStringField(log.TruckRegistration.Trim(), 30) + "', ");
                else
                    sbInsert.Append("NULL, ");                
                if (log.DriverFirstName.Trim() != "")
                    sbInsert.Append("N'" + getValidStringField(log.DriverFirstName.Trim(), 30) + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (log.DriverLastName.Trim() != "")
                    sbInsert.Append("N'" + getValidStringField(log.DriverLastName.Trim(), 30) + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (log.BuyerName.Trim() != "")
                    sbInsert.Append("N'" + getValidStringField(log.BuyerName.Trim(), 50) + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (log.ForwarderName.Trim() != "")
                    sbInsert.Append("N'" + getValidStringField(log.ForwarderName.Trim(), 50) + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (log.OrderForm.Trim() != "")
                    sbInsert.Append("N'" + getValidStringField(log.OrderForm.Trim(), 50) + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (log.RegistrationTime != new DateTime())
                    sbInsert.Append("N'" + log.RegistrationTime.ToString(dateTimeformat) + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (log.RegistrationType.Trim() != "")
                    sbInsert.Append("'" + getValidStringField(log.RegistrationType.Trim(), 20) + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (log.RegistrationLocation.Trim() != "")
                    sbInsert.Append("'" + getValidStringField(log.RegistrationLocation.Trim(), 20) + "', ");
                else
                    sbInsert.Append("NULL, ");
                sbInsert.Append("'" + Constants.SiemensDefaultReadStatus.ToString().Trim() + "') ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), connection, sqltrans);
                if (cmd.ExecuteNonQuery() == 1)
                {
                    SqlCommand identityCmd = new SqlCommand("SELECT IDENT_CURRENT('IT_kamioni') AS id", connection, sqltrans);
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(identityCmd);
                    sqlAdapter.Fill(dataSet, "Kamioni");
                    DataTable dataTable = dataSet.Tables["Kamioni"];

                    if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["id"] != DBNull.Value)
                    {
                        if (!int.TryParse(dataTable.Rows[0]["id"].ToString().Trim(), out id))
                            id = 0;
                    }
                }

                if (doCommit)
                    sqltrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                   sqltrans.Rollback();
                
                throw ex;
            }

            return id;
        }

        public override bool insertEmployeeImage(SiemensEmployeeImageTO imgTO, bool doCommit)
        {
            int rowsAffected = 0;
            SqlTransaction sqltrans = null;
            if (doCommit)
                sqltrans = connection.BeginTransaction(IsolationLevel.RepeatableRead);
            else
                sqltrans = this.SqlTrans;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO " + DataBase + "." + SiPassUser + ".IT_HRImage");
                sbInsert.Append("(ID, Image, ReadStatus, TypeOfCh) ");
                sbInsert.Append("VALUES (");
                if (imgTO.ID.Trim() != "")
                    sbInsert.Append("N'" + getValidStringField(imgTO.ID.Trim(), 6) + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (imgTO.Image != null)
                    sbInsert.Append("@Content, ");
                else
                    sbInsert.Append("NULL, ");
                if (imgTO.ReadStatus != -1)
                    sbInsert.Append("'" + imgTO.ReadStatus.ToString().Trim() + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (imgTO.TypeOfCh.Trim() != "")
                    sbInsert.Append("N'" + getValidStringField(imgTO.TypeOfCh.Trim(), 20) + "')");
                else
                    sbInsert.Append("NULL)");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), connection, sqltrans);
                cmd.Parameters.Add("@Content", SqlDbType.Image, imgTO.Image.Length).Value = imgTO.Image;

                rowsAffected = cmd.ExecuteNonQuery();
                if (doCommit)
                    sqltrans.Commit();

                rowsAffected = 1;
            }
            catch (Exception ex)
            {
                if (doCommit)
                    sqltrans.Rollback();
                
                throw ex;
            }

            return rowsAffected > 0;
        }

        public override bool insertEmp(SiemensEmpTO empTO, bool doCommit)
        {
            int rowsAffected = 0;
            SqlTransaction sqltrans = null;
            if (doCommit)
                sqltrans = connection.BeginTransaction(IsolationLevel.RepeatableRead);
            else
                sqltrans = this.SqlTrans;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO " + DataBase + "." + SiPassUser + ".IT_Emp");
                sbInsert.Append("(emp_id, emp_idc, emp_rs) ");
                sbInsert.Append("VALUES (");
                if (empTO.ID.Trim() != "")
                    sbInsert.Append("N'" + getValidStringField(empTO.ID.Trim(), 6) + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (empTO.IDC.Trim() != "")
                    sbInsert.Append("N'" + getValidStringField(empTO.IDC.Trim(), 10) + "', ");
                else
                    sbInsert.Append("NULL, ");                
                if (empTO.RS != -1)
                    sbInsert.Append("'" + empTO.RS.ToString().Trim() + "') ");
                else
                    sbInsert.Append("NULL) ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), connection, sqltrans);
                rowsAffected = cmd.ExecuteNonQuery();
                if (doCommit)
                    sqltrans.Commit();

                rowsAffected = 1;
            }
            catch (Exception ex)
            {
                if (doCommit)
                    sqltrans.Rollback();

                throw ex;
            }

            return rowsAffected > 0;
        }

        public override Dictionary<string, SiemensEmployeeTO> getEmployees()
        {
            DataSet dataSet = new DataSet();
            SiemensEmployeeTO empl = new SiemensEmployeeTO();
            Dictionary<string, SiemensEmployeeTO> emplDict = new Dictionary<string, SiemensEmployeeTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM " + DataBase + "." + SiPassUser + ".IT_HRData");
                
                select = sb.ToString();
                select += " ORDER BY LastName, FirstName ";

                SqlCommand cmd = new SqlCommand(select, connection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        empl = new SiemensEmployeeTO();
                        if (row["PK"] != DBNull.Value)
                        {
                            empl.PK = uint.Parse(row["PK"].ToString().Trim());
                        }                        
                        if (row["ID"] != DBNull.Value)
                        {
                            empl.ID = row["ID"].ToString().Trim();
                        }                        
                        if (row["FirstName"] != DBNull.Value)
                        {
                            empl.FirstName = row["FirstName"].ToString().Trim();
                        }
                        if (row["LastName"] != DBNull.Value)
                        {
                            empl.LastName = row["LastName"].ToString().Trim();
                        }
                        if (row["IdDept"] != DBNull.Value)
                        {
                            empl.DepartmentID = int.Parse(row["IdDept"].ToString().Trim());
                        }
                        if (row["IdDeptOld"] != DBNull.Value)
                        {
                            empl.DepartmentIDOld = row["IdDeptOld"].ToString().Trim();
                        }
                        if (row["Status"] != DBNull.Value)
                        {
                            empl.Status = row["Status"].ToString().Trim();
                        }
                        if (row["IDOld"] != DBNull.Value)
                        {
                            empl.IDOld = row["IDOld"].ToString().Trim();
                        }
                        if (row["ReadStatus"] != DBNull.Value)
                        {
                            empl.ReadStatus = (int)byte.Parse(row["ReadStatus"].ToString());
                        }
                        if (row["TypeOfCh"] != DBNull.Value)
                        {
                            empl.TypeOfCh = row["TypeOfCh"].ToString().Trim();
                        }
                        if (row["TimeSc"] != DBNull.Value)
                        {
                            empl.TimeSc = DateTime.Parse(row["TimeSc"].ToString());
                        }
                        if (row["jmbg"] != DBNull.Value)
                        {
                            empl.JMBG = row["jmbg"].ToString();
                        }

                        if (empl.ID.Trim() != "" && !emplDict.ContainsKey(empl.ID))
                            emplDict.Add(empl.ID, empl);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return emplDict;
        }

        public override List<SiemensEmployeeTO> getEmployeesUnprocessed()
        {
            DataSet dataSet = new DataSet();
            SiemensEmployeeTO empl = new SiemensEmployeeTO();
            List<SiemensEmployeeTO> emplList = new List<SiemensEmployeeTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM " + DataBase + "." + SiPassUser + ".IT_HRData");
                sb.Append(" WHERE ReadStatus = '" + Constants.noInt.ToString().Trim() + "'");

                select = sb.ToString();
                select += " ORDER BY PK ";

                SqlCommand cmd = new SqlCommand(select, connection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Employee");
                DataTable table = dataSet.Tables["Employee"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        empl = new SiemensEmployeeTO();
                        if (row["PK"] != DBNull.Value)
                        {
                            empl.PK = uint.Parse(row["PK"].ToString().Trim());
                        }
                        if (row["ID"] != DBNull.Value)
                        {
                            empl.ID = row["ID"].ToString().Trim();
                        }
                        if (row["FirstName"] != DBNull.Value)
                        {
                            empl.FirstName = row["FirstName"].ToString().Trim();
                        }
                        if (row["LastName"] != DBNull.Value)
                        {
                            empl.LastName = row["LastName"].ToString().Trim();
                        }
                        if (row["IdDept"] != DBNull.Value)
                        {
                            empl.DepartmentID = int.Parse(row["IdDept"].ToString().Trim());
                        }
                        if (row["IdDeptOld"] != DBNull.Value)
                        {
                            empl.DepartmentIDOld = row["IdDeptOld"].ToString().Trim();
                        }
                        if (row["Status"] != DBNull.Value)
                        {
                            empl.Status = row["Status"].ToString().Trim();
                        }
                        if (row["IDOld"] != DBNull.Value)
                        {
                            empl.IDOld = row["IDOld"].ToString().Trim();
                        }
                        if (row["ReadStatus"] != DBNull.Value)
                        {
                            empl.ReadStatus = (int)byte.Parse(row["ReadStatus"].ToString());
                        }
                        if (row["TypeOfCh"] != DBNull.Value)
                        {
                            empl.TypeOfCh = row["TypeOfCh"].ToString().Trim();
                        }
                        if (row["TimeSc"] != DBNull.Value)
                        {
                            empl.TimeSc = DateTime.Parse(row["TimeSc"].ToString());
                        }
                        //if (row["jmbg"] != DBNull.Value)
                        //{
                        //    empl.JMBG = row["jmbg"].ToString();
                        //}

                        emplList.Add(empl);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return emplList;
        }

        public override int getParentDepartment(int id)
        {
            DataSet dataSet = new DataSet();
            int parentID = -1;
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM " + DataBase + "." + SiPassUser + ".IT_DPData WHERE Odid = '" + id.ToString().Trim() + "' AND IDDeptParent IS NOT NULL AND (ReadStatus = '" 
                    + Constants.yesInt.ToString().Trim() + "' OR LevelDept = '" + Constants.SiemensDepartmentLevel.ToString().Trim() + "') ORDER BY PK DESC");

                select = sb.ToString();

                SqlCommand cmd = new SqlCommand(select, connection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Department");
                DataTable table = dataSet.Tables["Department"];

                if (table.Rows.Count > 0)
                {
                    if (table.Rows[0]["IdDeptParent"] != DBNull.Value)
                    {
                        parentID = int.Parse(table.Rows[0]["IdDeptParent"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return parentID;
        }

        public override int getLevelDepartment(int id)
        {
            DataSet dataSet = new DataSet();
            int level = -1;
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM " + DataBase + "." + SiPassUser + ".IT_DPData WHERE Odid = '" + id.ToString().Trim() + "' AND LevelDept IS NOT NULL AND (ReadStatus = '"
                    + Constants.yesInt.ToString().Trim() + "' OR LevelDept = '" + Constants.SiemensDepartmentLevel.ToString().Trim() + "') ORDER BY PK DESC");

                select = sb.ToString();

                SqlCommand cmd = new SqlCommand(select, connection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Department");
                DataTable table = dataSet.Tables["Department"];

                if (table.Rows.Count > 0)
                {
                    if (table.Rows[0]["LevelDept"] != DBNull.Value)
                    {
                        level = int.Parse(table.Rows[0]["LevelDept"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return level;
        }

        public override string getNameDepartment(int id)
        {
            DataSet dataSet = new DataSet();
            string name = "";
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM " + DataBase + "." + SiPassUser + ".IT_DPData WHERE Odid = '" + id.ToString().Trim() + "' AND NameDept IS NOT NULL AND (ReadStatus = '"
                    + Constants.yesInt.ToString().Trim() + "' OR LevelDept = '" + Constants.SiemensDepartmentLevel.ToString().Trim() + "') ORDER BY PK DESC");

                select = sb.ToString();

                SqlCommand cmd = new SqlCommand(select, connection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Department");
                DataTable table = dataSet.Tables["Department"];

                if (table.Rows.Count > 0)
                {
                    if (table.Rows[0]["NameDept"] != DBNull.Value)
                    {
                        name = table.Rows[0]["NameDept"].ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return name;
        }

        public override List<SiemensDepartmentTO> getDepartmentsUnprocessed()
        {
            DataSet dataSet = new DataSet();
            SiemensDepartmentTO deptTO = new SiemensDepartmentTO();
            List<SiemensDepartmentTO> deptList = new List<SiemensDepartmentTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM " + DataBase + "." + SiPassUser + ".IT_DPData WHERE ReadStatus = '" + Constants.noInt.ToString().Trim() + "' AND LevelDept < '" + Constants.SiemensDepartmentLevel.ToString().Trim() + "'");

                select = sb.ToString();
                select += " ORDER BY PK";

                SqlCommand cmd = new SqlCommand(select, connection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Department");
                DataTable table = dataSet.Tables["Department"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        deptTO = new SiemensDepartmentTO();
                        if (row["PK"] != DBNull.Value)
                        {
                            deptTO.PK = uint.Parse(row["PK"].ToString().Trim());
                        }
                        if (row["IDDept"] != DBNull.Value)
                        {
                            deptTO.ID = row["IDDept"].ToString().Trim();
                        }
                        if (row["NameDept"] != DBNull.Value)
                        {
                            deptTO.Name = row["NameDept"].ToString().Trim();

                            if (deptTO.Name.Length > Constants.SiemensWorkgroupMaxNameLenght)
                                deptTO.Name = deptTO.Name.Substring(0, Constants.SiemensWorkgroupMaxNameLenght);
                        }
                        if (row["LevelDept"] != DBNull.Value)
                        {
                            deptTO.Level = int.Parse(row["LevelDept"].ToString());
                        }
                        if (row["NameDeptE"] != DBNull.Value)
                        {
                            deptTO.NameE = row["NameDeptE"].ToString().Trim();
                        }
                        if (row["IDDeptParentOld"] != DBNull.Value)
                        {
                            deptTO.ParentIDOld = row["IDDeptParentOld"].ToString().Trim();
                        }
                        if (row["Odid"] != DBNull.Value)
                        {
                            deptTO.Odid = int.Parse(row["Odid"].ToString());
                        }
                        if (row["IDDeptParent"] != DBNull.Value)
                        {
                            deptTO.ParentID = int.Parse(row["IDDeptParent"].ToString().Trim());
                        }
                        if (row["ReadStatus"] != DBNull.Value)
                        {
                            deptTO.ReadStatus = (int)byte.Parse(row["ReadStatus"].ToString());
                        }
                        if (row["TypeCh"] != DBNull.Value)
                        {
                            deptTO.TypeOfCh = row["TypeCh"].ToString().Trim();
                        }
                        if (row["TimeSc"] != DBNull.Value)
                        {
                            deptTO.TimeSc = DateTime.Parse(row["TimeSc"].ToString());
                        }

                        deptList.Add(deptTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return deptList;
        }

        public override List<SiemensLogTO> getPasses(string id, string direction, string created, string remark, DateTime from, DateTime to)
        {
            DataSet dataSet = new DataSet();
            SiemensLogTO pass = new SiemensLogTO();
            List<SiemensLogTO> passList = new List<SiemensLogTO>();
            string select = "";
            
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM " + DataBase + "." + SiPassUser + "." + AuditTrailTableName);

                if (id.Trim() != "" || direction.Trim() != "" || created.Trim() != "" || remark.Trim() != "" || from != new DateTime() || to != new DateTime())
                {
                    sb.Append(" WHERE ");

                    if (id.Trim() != "")
                        sb.Append("ID = N'" + id.Trim() + "' AND ");

                    if (direction.Trim() != "")
                        sb.Append("Direction = '" + direction.Trim() + "' AND ");

                    if (created.Trim() != "")
                        sb.Append("RegLoc " + created.Trim() + " AND ");

                    if (remark.Trim() != "")
                        sb.Append("UPPER(Col1) LIKE N'%" + remark.Trim() + "%' AND ");

                    if (from != new DateTime())
                        sb.Append("RegTime >= '" + from.Date.ToString(dateTimeformat) + "' AND ");

                    if (to != new DateTime())
                        sb.Append("RegTime < '" + to.Date.AddDays(1).ToString(dateTimeformat) + "' AND ");

                    select = sb.ToString();

                    select = select.Substring(0, select.Length - 4);
                }
                else
                    select = sb.ToString();

                select += " ORDER BY RegTime";

                SqlCommand cmd = new SqlCommand(select, connection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Passes");
                DataTable table = dataSet.Tables["Passes"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pass = new SiemensLogTO();
                        if (row["PK"] != DBNull.Value)
                        {
                            pass.PK = uint.Parse(row["PK"].ToString().Trim());
                        }
                        if (row["ID"] != DBNull.Value)
                        {
                            pass.Id = row["ID"].ToString().Trim();
                        }
                        if (row["Name"] != DBNull.Value)
                        {
                            pass.Name = row["Name"].ToString().Trim();
                        }
                        if (row["LastName"] != DBNull.Value)
                        {
                            pass.LastName = row["LastName"].ToString().Trim();
                        }
                        if (row["RegTime"] != DBNull.Value)
                        {
                            pass.RegTime = DateTime.Parse(row["RegTime"].ToString());
                        }
                        if (row["Direction"] != DBNull.Value)
                        {
                            pass.Direction = row["Direction"].ToString().Trim();
                        }
                        if (row["RegLoc"] != DBNull.Value)
                        {
                            pass.RegLoc = int.Parse(row["RegLoc"].ToString());
                        }
                        if (row["TypeID"] != DBNull.Value)
                        {
                            pass.TypeID = row["TypeID"].ToString().Trim();
                        }
                        if (row["Company"] != DBNull.Value)
                        {
                            pass.Company = row["Company"].ToString().Trim();
                        }
                        //if (row["ReadStatus"] != DBNull.Value)
                        //{
                        //    pass.ReadStatus = (int)byte.Parse(row["ReadStatus"].ToString());
                        //}
                        if (row["Col1"] != DBNull.Value)
                        {
                            pass.Col1 = row["Col1"].ToString().Trim();
                        }

                        passList.Add(pass);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return passList;
        }

        public override List<SiemensTruckLogTO> getTruckPasses(string truckReg, string buyer, string forwarder, string order, string driverLastName, string driverFirstName, string direction, string created, DateTime from, DateTime to)
        {
            DataSet dataSet = new DataSet();
            SiemensTruckLogTO pass = new SiemensTruckLogTO();
            List<SiemensTruckLogTO> passList = new List<SiemensTruckLogTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM " + DataBase + "." + SiPassUser + ".IT_kamioni");

                if (truckReg.Trim() != "" || buyer.Trim() != "" || forwarder.Trim() != "" || order.Trim() != "" || driverLastName.Trim() != "" || driverFirstName.Trim() != "" 
                    || direction.Trim() != "" || created.Trim() != "" || from != new DateTime() || to != new DateTime())
                {
                    sb.Append(" WHERE ");

                    if (truckReg.Trim() != "")
                        sb.Append("kamion_registracija = N'" + truckReg.Trim() + "' AND ");

                    if (buyer.Trim() != "")
                        sb.Append("kupac_naziv = N'" + buyer.Trim() + "' AND ");

                    if (forwarder.Trim() != "")
                        sb.Append("spediter_naziv = N'" + forwarder.Trim() + "' AND ");

                    if (order.Trim() != "")
                        sb.Append("porudzbenica = N'" + order.Trim() + "' AND ");

                    if (driverLastName.Trim() != "")
                        sb.Append("vozac_prezime = N'" + driverLastName.Trim() + "' AND ");

                    if (driverFirstName.Trim() != "")
                        sb.Append("vozac_ime = N'" + driverFirstName.Trim() + "' AND ");

                    if (direction.Trim() != "")
                        sb.Append("tip_registracije = '" + direction.Trim() + "' AND ");

                    if (created.Trim() != "")
                        sb.Append("lokacija_registracije " + created.Trim() + " AND ");

                    if (from != new DateTime())
                        sb.Append("vreme_registracije >= '" + from.Date.ToString(dateTimeformat) + "' AND ");

                    if (to != new DateTime())
                        sb.Append("vreme_registracije < '" + to.Date.AddDays(1).ToString(dateTimeformat) + "' AND ");

                    select = sb.ToString();

                    select = select.Substring(0, select.Length - 4);
                }
                else
                    select = sb.ToString();
                
                select += " ORDER BY vreme_registracije";

                SqlCommand cmd = new SqlCommand(select, connection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Passes");
                DataTable table = dataSet.Tables["Passes"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pass = new SiemensTruckLogTO();
                        if (row["id"] != DBNull.Value)
                        {
                            pass.ID = int.Parse(row["id"].ToString().Trim());
                        }
                        if (row["kamion_registracija"] != DBNull.Value)
                        {
                            pass.TruckRegistration = row["kamion_registracija"].ToString().Trim();
                        }
                        if (row["vozac_ime"] != DBNull.Value)
                        {
                            pass.DriverFirstName = row["vozac_ime"].ToString().Trim();
                        }
                        if (row["vozac_prezime"] != DBNull.Value)
                        {
                            pass.DriverLastName = row["vozac_prezime"].ToString().Trim();
                        }
                        if (row["kupac_naziv"] != DBNull.Value)
                        {
                            pass.BuyerName = row["kupac_naziv"].ToString().Trim();
                        }
                        if (row["spediter_naziv"] != DBNull.Value)
                        {
                            pass.ForwarderName = row["spediter_naziv"].ToString();
                        }
                        if (row["porudzbenica"] != DBNull.Value)
                        {
                            pass.OrderForm = row["porudzbenica"].ToString().Trim();
                        }
                        if (row["vreme_registracije"] != DBNull.Value)
                        {
                            pass.RegistrationTime = DateTime.Parse(row["vreme_registracije"].ToString().Trim());
                        }                        
                        if (row["tip_registracije"] != DBNull.Value)
                        {
                            pass.RegistrationType = row["tip_registracije"].ToString().Trim();
                        }
                        if (row["lokacija_registracije"] != DBNull.Value)
                        {
                            pass.RegistrationLocation = row["lokacija_registracije"].ToString().Trim();
                        }

                        passList.Add(pass);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return passList;
        }

        public override List<string> getEmplImages()
        {
            DataSet dataSet = new DataSet();
            List<string> emplNums = new List<string>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT ID FROM " + DataBase + "." + SiPassUser + ".IT_HRImage");

                select = sb.ToString();

                SqlCommand cmd = new SqlCommand(select, connection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Images");
                DataTable table = dataSet.Tables["Images"];

                foreach (DataRow row in table.Rows)
                {
                    if (row["ID"] != DBNull.Value)
                        emplNums.Add(row["ID"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return emplNums;
        }

        public override Dictionary<string, string> getEmpls()
        {
            DataSet dataSet = new DataSet();
            SiemensEmpTO empTO = new SiemensEmpTO();
            Dictionary<string, string> empDict = new Dictionary<string, string>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM " + DataBase + "." + SiPassUser + ".IT_Emp ORDER BY emp_id, PK DESC");

                select = sb.ToString();
                
                SqlCommand cmd = new SqlCommand(select, connection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Emp");
                DataTable table = dataSet.Tables["Emp"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        empTO = new SiemensEmpTO();
                        if (row["emp_id"] != DBNull.Value)
                        {
                            empTO.ID = row["emp_id"].ToString().Trim();
                        }
                        if (row["emp_idc"] != DBNull.Value)
                        {
                            empTO.IDC = row["emp_idc"].ToString().Trim();
                        }

                        if (empTO.ID.Trim() != "" && empTO.IDC.Trim() != "")
                        {
                            if (!empDict.ContainsKey(empTO.ID.Trim()))
                                empDict.Add(empTO.ID.Trim(), empTO.IDC.Trim());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return empDict;
        }

        public override DateTime getMaxDateTruckPassTransfered()
        {
            DataSet dataSet = new DataSet();
            DateTime maxDate = new DateTime();

            try
            {
                string select = "SELECT MAX(vreme_registracije) AS max_date FROM " + DataBase + "." + SiPassUser + ".IT_kamioni WHERE lokacija_registracije <> '" + Constants.SiemensManualCreatedLoc.ToString().Trim() + "'";

                SqlCommand cmd = new SqlCommand(select, connection);

                cmd.CommandTimeout = 1500;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Log");
                DataTable table = dataSet.Tables["Log"];
                if (table.Rows.Count > 0 && table.Rows[0]["max_date"] != DBNull.Value)
                    maxDate = DateTime.Parse(table.Rows[0]["max_date"].ToString().Trim()).Date;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return maxDate;
        }

        public override DateTime getMaxDatePassTransfered()
        {
            DataSet dataSet = new DataSet();
            DateTime maxDate = new DateTime();

            try
            {
                string select = "SELECT MAX(RegTime) AS max_date FROM " + DataBase + "." + SiPassUser + ".IT_Reg WHERE RegLoc <> '" + Constants.SiemensManualCreatedLoc.ToString().Trim() + "'";

                SqlCommand cmd = new SqlCommand(select, connection);

                cmd.CommandTimeout = 1500;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Log");
                DataTable table = dataSet.Tables["Log"];
                if (table.Rows.Count > 0 && table.Rows[0]["max_date"] != DBNull.Value)
                    maxDate = DateTime.Parse(table.Rows[0]["max_date"].ToString().Trim()).Date;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return maxDate;
        }

        public override bool updateEmployeeToProcessed(uint pk, bool doCommit)
        {
            bool isUpdated = false;
            
            SqlTransaction sqlTrans = null;
            if (doCommit)
                sqlTrans = connection.BeginTransaction(IsolationLevel.RepeatableRead);
            else
                sqlTrans = this.SqlTrans;

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE " + DataBase + "." + SiPassUser + ".IT_HRData SET ReadStatus = '" + Constants.yesInt.ToString().Trim() + "' ");
                sbUpdate.Append("WHERE PK = '" + pk.ToString().Trim() + "'" );
                
                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), connection, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)                
                    isUpdated = true;

                if (doCommit)
                    sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    sqlTrans.Rollback();

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public override bool updateDepartmentToProcessed(uint pk, bool doCommit)
        {
            bool isUpdated = false;

            SqlTransaction sqlTrans = null;
            if (doCommit)
                sqlTrans = connection.BeginTransaction(IsolationLevel.RepeatableRead);
            else
                sqlTrans = this.SqlTrans;

            try
            {
                StringBuilder sbUpdate = new StringBuilder();                
                sbUpdate.Append("UPDATE " + DataBase + "." + SiPassUser + ".IT_DPData SET ReadStatus = '" + Constants.yesInt.ToString().Trim() + "' ");
                sbUpdate.Append("WHERE PK = '" + pk.ToString().Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), connection, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                    isUpdated = true;

                if (doCommit)
                    sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    sqlTrans.Rollback();

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public string getValidStringField(string field, int length)
        {
            try
            {
                if (field.Trim().Length <= length)
                    return field.Trim();
                else
                    return field.Substring(0, length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override ArrayList deserializeDepartments(string filePath)
        {
            ArrayList deptList = new ArrayList();

            try
            {
                if (File.Exists(filePath))
                {
                    Stream stream = File.OpenRead(filePath);
                    XmlSerializer bformatter = new XmlSerializer(typeof(SiemensDepartmentTO[]));
                    SiemensDepartmentTO[] deserialized;

                    try
                    {
                        deserialized = (SiemensDepartmentTO[])bformatter.Deserialize(stream);
                        deptList = ArrayList.Adapter(deserialized);
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

            return deptList;
        }

        public override bool serializeDepartments(System.Collections.ArrayList DeptTOList, string filePath)
        {
            bool isSerialized = false;

            try
            {
                Stream stream = File.Open(filePath, FileMode.Create);
                SiemensDepartmentTO[] deptTOArray = (SiemensDepartmentTO[])DeptTOList.ToArray(typeof(SiemensDepartmentTO));

                XmlSerializer bformatter = new XmlSerializer(typeof(SiemensDepartmentTO[]));
                bformatter.Serialize(stream, deptTOArray);
                stream.Close();
                isSerialized = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isSerialized;
        }

        public override bool beginTransaction()
        {
            bool isStarted = false;

            try
            {
                SqlTrans = connection.BeginTransaction(IsolationLevel.RepeatableRead);
                isStarted = true;
            }
            catch (Exception ex)
            {
                isStarted = false;
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public override void commitTransaction()
        {
            this.SqlTrans.Commit();
            this.SqlTrans = null;
        }

        public override void rollbackTransaction()
        {
            if (this.SqlTrans != null)
                this.SqlTrans.Rollback();
            this.SqlTrans = null;
        }

        public override IDbTransaction getTransaction()
        {
            return _sqlTrans;
        }

        public override void setTransaction(IDbTransaction trans)
        {
            _sqlTrans = (SqlTransaction)trans;
        }
    }
}
