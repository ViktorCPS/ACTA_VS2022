using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Util;
using System.Resources;
using System.Globalization;
using Common;
using UI;
using System.IO;
using System.Data.SqlClient;

namespace SiemensUI
{
    public partial class BrezaDBConnSetup : Form
    { 
        DebugLog log;
        ResourceManager rm;
        private CultureInfo culture;

        private string host = "";
        private string loginName = "";
        private string password = "";
        private string database = "";
        private string table = "";
        private string userName = "";


        public BrezaDBConnSetup()
        {
            InitializeComponent();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);


            rm = new ResourceManager("UI.Resource", typeof(Employees).Assembly);
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
        }

        private void BrezaDBConnSetup_Load(object sender, EventArgs e)
        {
            try
            {
                this.CenterToScreen();
                
                getConnectionParams();
                tbDataBase.Text = database;
                tbHost.Text = host;
                tbLoginName.Text = loginName;
                tbPassword.Text = password;
                if (table.Equals(""))
                {
                    table = Constants.SiemensTableName;
                }
                if (userName.Equals(""))
                    userName = Constants.SiemensUser;

                tbTableName.Text = table;
                tbUserName.Text = userName;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SiemensDBConnSetup.SiemensDBConnSetup_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

        }

        private void getConnectionParams()
        {
            try
            {
                string connectionString = "";
                if (File.Exists(Constants.SiPassConnPathBreza) || File.Exists(Constants.SiPassConnPathBreza))
                {
                    StreamReader reader = new StreamReader(Constants.SiPassConnPathBreza);
                    connectionString = reader.ReadLine();
                    reader.Close();
                }
                else
                { 
                    MessageBox.Show(rm.GetString("noConnString",culture));
                    return;
                }
                if (connectionString == null || connectionString.Equals(""))
                {
                    throw new Exception(Constants.connStringNotFound);
                }
                //try
                //{
                //    byte[] buffer = Convert.FromBase64String(connectionString);
                //    connectionString = Util.Misc.decrypt(buffer);
                //}
                //catch
                //{
                //    connectionString = "";
                //}
               
                if (!connectionString.Equals(""))
                {
                    int startIndex = connectionString.ToLower().IndexOf("server");
                    startIndex = connectionString.ToLower().IndexOf("server",startIndex+1);
                    int endIndex = 0;
                    if (startIndex >= 0)
                    {
                        endIndex = connectionString.IndexOf(";", startIndex);

                        if (endIndex >= startIndex)
                        {
                            // take data provider value
                            // data provider part of the connection string is like "data provider=mysql;" and we need "mysql"
                            // or string is like "data provider=sqlserver;" and we need "sqlserver"
                            startIndex = connectionString.IndexOf("=", startIndex);
                            if (startIndex >= 0)
                                host = connectionString.Substring(startIndex + 1, endIndex - startIndex - 1);
                        }

                    }
                    int start = connectionString.ToLower().IndexOf("database");
                    if (start > 0)
                    {
                        int end = endIndex = connectionString.IndexOf(";", start);
                        if (end >= start)
                        {
                            start = connectionString.IndexOf("=", start);
                            database = connectionString.Substring(start + 1, end - start - 1);
                        }
                    }
                    startIndex = connectionString.ToLower().IndexOf("uid");
                    if (startIndex < 0)
                    {
                        startIndex = connectionString.ToLower().IndexOf("user id");
                    }

                    if (startIndex >= 0)
                    {
                        endIndex = connectionString.IndexOf(";", startIndex);

                        if (endIndex >= startIndex)
                        {
                            // take data provider value
                            // data provider part of the connection string is like "data provider=mysql;" and we need "mysql"
                            // or string is like "data provider=sqlserver;" and we need "sqlserver"
                            startIndex = connectionString.IndexOf("=", startIndex);
                            if (startIndex >= 0)
                                loginName = connectionString.Substring(startIndex + 1, endIndex - startIndex - 1);
                        }

                    }
                    startIndex = connectionString.ToLower().IndexOf("pwd");

                    if (startIndex >= 0)
                    {
                        startIndex = connectionString.IndexOf("=", startIndex);
                        if (startIndex >= 0)
                        {
                            endIndex = connectionString.IndexOf(";table=", startIndex);
                            if(endIndex > startIndex)

                                password = connectionString.Substring(startIndex + 1, endIndex - startIndex - 1);
                        }

                    }
                    else
                    {
                        startIndex = connectionString.ToLower().IndexOf("password");
                        if (startIndex >= 0)
                        {
                            endIndex = connectionString.IndexOf(";table=", startIndex);

                            if (endIndex >= startIndex)
                            {
                                // take data provider value
                                // data provider part of the connection string is like "data provider=mysql;" and we need "mysql"
                                // or string is like "data provider=sqlserver;" and we need "sqlserver"
                                startIndex = connectionString.IndexOf("=", startIndex);
                                if (startIndex >= 0)
                                    password = connectionString.Substring(startIndex + 1, endIndex - startIndex - 1);
                            }
                        }
                    }
                    start = connectionString.ToLower().IndexOf("table");
                    if (start > 0)
                    {
                        int end =  connectionString.IndexOf(";", start);
                        if (end >= start)
                        {
                            start = connectionString.IndexOf("=", start);
                            table = connectionString.Substring(start + 1, end - start - 1);
                        }
                    }

                    start = connectionString.ToLower().IndexOf("sipassuser");
                    if (start > 0)
                    {
                        int end =  connectionString.IndexOf(";", start);
                        if (end >= start)
                        {
                            start = connectionString.IndexOf("=", start);
                            userName = connectionString.Substring(start + 1, end - start - 1);
                        }
                    }
                    


                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SiemensDBConnSetup.getConnectionParams(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                bool valid = Validation();
                if (valid)
                {
                    string connectionString = createConnectionString();
                    Stream stream = new FileStream(Constants.SiPassConnPathBreza, FileMode.Create);
                    //if (File.Exists(Constants.SiPassConnPath))
                    //{

                        // encrypt connection string
                        if (connectionString.ToLower().StartsWith("data provider"))
                        {
                            // encrypt a string to a byte array.
                            //byte[] buffer = Util.Misc.encrypt(connectionString);

                            //string connStringCrypted = Convert.ToBase64String(buffer);

                            if (File.Exists(Constants.SiPassConnPath))
                            {
                                StreamWriter writer = new StreamWriter(stream);
                                writer.WriteLine(connectionString);
                                writer.Close();
                            }
                        }
                    //}
                    if (connectionString == null || connectionString.Equals(""))
                    {
                        throw new Exception(Constants.connStringNotFound);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SiemensDBConnSetup.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
                this.Close();
            }
        }

        private bool Validation()
        {
            bool valid = true;
            try
            {
                if (tbDataBase.Text.ToString().Trim().Length == 0)
                { 
                    MessageBox.Show(rm.GetString("noDataBase",culture));
                    return false;
                }
                if(tbHost.Text.ToString().Trim().Length == 0)
                {
                    MessageBox.Show(rm.GetString("noHostName",culture));
                    return false;
                }
                if(tbPassword.Text.ToString().Trim().Length == 0)
                {
                    MessageBox.Show(rm.GetString("noPwd",culture));
                    return false;
                }
                if(tbLoginName.Text.ToString().Trim().Length == 0)
                {
                    MessageBox.Show(rm.GetString("noLogInName",culture));
                    return false;
                }
                if (tbUserName.Text.ToString().Trim().Length == 0||tbTableName.Text.ToString().Trim().Length == 0)
                {
                    MessageBox.Show(rm.GetString("noUserOrTeble", culture));
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SiemensDBConnSetup.Validation(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return valid;
        }

        private string createConnectionString()
        {
            string connectionString = "";
            StringBuilder connString = new StringBuilder();

            // create connection string using entered values
            connString.Append(Constants.dataProvider);
            connString.Append(Constants.dataProviderSQLServer);
            connString.Append(Constants.server);
            connString.Append(tbHost.Text.Trim() + ";");
            connString.Append(Constants.dataBase);
            connString.Append(tbDataBase.Text.Trim() + ";");
            connString.Append(Constants.sqlServerSiemensUID);
            connString.Append(tbLoginName.Text.Trim() + ";");
            connString.Append(Constants.sqlServerSiemensPwd);
            connString.Append(tbPassword.Text.Trim() + ";");
            connString.Append(Constants.sqlServerSiemensTable);
            connString.Append(tbTableName.Text.Trim() + ";");
            connString.Append(Constants.sqlServerSiemensUser);
            connString.Append(tbUserName.Text.Trim() + ";");


            connectionString = connString.ToString().Trim();

            return connectionString;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
             this.Cursor = Cursors.WaitCursor;
             bool valid = Validation();
             if (valid)
             {
                 string connectionString = createConnectionString();

                 string dataProvider = "";
                 int startIndex = -1;
                 int endIndex = -1;

                 startIndex = connectionString.ToLower().IndexOf("data provider");

                 if (startIndex >= 0)
                 {
                     endIndex = connectionString.IndexOf(";", startIndex);

                     if (endIndex >= startIndex)
                     {
                         // take data provider value
                         // data provider part of the connection string is like "data provider=mysql;" and we need "mysql"
                         // or string is like "data provider=sqlserver;" and we need "sqlserver"
                         startIndex = connectionString.IndexOf("=", startIndex);
                         if (startIndex >= 0)
                             dataProvider = connectionString.Substring(startIndex + 1, endIndex - startIndex - 1);
                     }
                 }

                 startIndex = -1;

                 startIndex = connectionString.ToLower().IndexOf("server=");

                 if (startIndex >= 0)
                 {
                     endIndex = connectionString.ToLower().IndexOf(";table=");
                     if(endIndex>startIndex)
                         connectionString = connectionString.Substring(startIndex, endIndex-startIndex);
                 }

                 switch (dataProvider.ToLower())
                 {
                     case "sqlserver":
                         {
                             SqlConnection connection;

                             connection = new SqlConnection(connectionString);
                             connection.Open();
                             if (connection.State.Equals(ConnectionState.Open))
                             {
                                 this.Cursor = Cursors.Arrow;
                                 MessageBox.Show(rm.GetString("connSuccess", culture));
                             }
                             else
                             {
                                 this.Cursor = Cursors.Arrow;
                                 MessageBox.Show(rm.GetString("connFailed", culture));
                             }
                             return;
                         }
                     case "mysql":
                         {
                             //MySqlConnection connection;

                             //connection = new MySqlConnection(connectionString);
                             //connection.Open();
                             //if (connection.State.Equals(ConnectionState.Open))
                             //{
                             //    this.Cursor = Cursors.Arrow;
                             //    MessageBox.Show(rm.GetString("connSuccess", culture));
                             //}
                             //else
                             //{
                             //    this.Cursor = Cursors.Arrow;
                             //    MessageBox.Show(rm.GetString("connFailed", culture));
                             //}
                             return;
                         }
                     case "":
                         {
                             this.Cursor = Cursors.Arrow;
                             MessageBox.Show(rm.GetString("connFailed", culture));
                             return;
                         }
                     default:
                         {
                             this.Cursor = Cursors.Arrow;
                             MessageBox.Show(rm.GetString("connFailed", culture));
                             return;
                         }
                 }
             }
             else
             {
                 this.Cursor = Cursors.Arrow;
             }
            }
            catch(Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                MessageBox.Show(rm.GetString("connFailed", culture) + " Error -> " + ex.Message);
            }
        }

       

       
    }
}