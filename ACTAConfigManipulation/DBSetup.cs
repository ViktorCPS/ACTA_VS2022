using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

using MySql.Data.MySqlClient;
using System.Data.SqlClient;

using Common;
using Util;
using System.Resources;
using System.Globalization;
using System.Configuration;

namespace ACTAConfigManipulation
{
    public partial class DBSetup : Form
    {
        private CultureInfo culture;
        ResourceManager rm;
        DebugLog log;
        private string from = "";

        public NotificationController Controller;

        public DBSetup(string dataprovider, string server, string port, string database, string  from)
        {
            InitializeComponent();

            this.CenterToScreen();

            // Init Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            this.from = from;
            Controller = NotificationController.GetInstance();

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("ACTAConfigManipulation.Resource", typeof(DBSetup).Assembly);
            setLanguage();

            if (!dataprovider.Equals(""))
            {
                if (dataprovider.Equals(Constants.dataProviderMySQL))
                {
                    rbMySQL.Checked = true;
                    rbSQLServer.Checked = false;
                }
                else if (dataprovider.Equals(Constants.dataProviderSQLServer))
                {
                    rbMySQL.Checked = false;
                    rbSQLServer.Checked = true;
                }
            }

            if (!server.Equals(""))
            {
                tbServer.Text = server;
            }
            else
            {
                tbServer.Text = (rbMySQL.Checked ? Constants.mysqlServerDefault : Constants.sqlServerDefault);
            }
            if (!port.Equals(""))
            {
                tbPort.Text = port;
            }
            else
            {
                tbPort.Text = Constants.mysqlPortDefault;
            }
            if (!database.Equals(""))
            {
                tbDatabase.Text = database;
            }
            else
            {
                tbDatabase.Text = Constants.dataBaseDefault;
            }

            tbServer.Focus();
        }

        /// <summary>
        /// Set proper language.
        /// </summary>
        private void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("DBSetupForm", culture);

                // group box text
                this.gbDatabase.Text = rm.GetString("gbDatabase", culture);

                // button's text
                btnTest.Text = rm.GetString("btnTest", culture);
                btnOK.Text = rm.GetString("btnOK", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);

                // label's text
                lblServer.Text = rm.GetString("lblServer", culture);
                lblPort.Text = rm.GetString("lblPort", culture);
                lblDatabase.Text = rm.GetString("lblDatabase", culture);
                lblCaseSensitive.Text = rm.GetString("lblCaseSensitive", culture);

                // raidio buttons
                rbMySQL.Text = rm.GetString("rbMySQL", culture);
                rbSQLServer.Text = rm.GetString("rbSQLServer", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " DBSetup.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.from.Equals(Constants.dbSetupInfo))
            {
                Controller.DBChanged(false);
                this.Close();
            }
            else
            {
                // currently, this form could be loaded only before loading application or from Info screen
                this.Close();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                //Application.Exit();
            }
        }

        private void rbMySQL_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMySQL.Checked)
            {
                rbSQLServer.Checked = false;
                lblPort.Visible = true;
                tbPort.Visible = true;
                tbPort.Text = Constants.mysqlPortDefault;
                tbServer.Text = Constants.mysqlServerDefault;
                tbDatabase.Text = Constants.dataBaseDefault;
                lblCaseSensitive.Visible = false;
            }
            else
            {
                rbSQLServer.Checked = true;
                lblPort.Visible = false;
                tbPort.Visible = false;
                tbServer.Text = Constants.sqlServerDefault;
                tbDatabase.Text = Constants.dataBaseDefault;
                lblCaseSensitive.Visible = true;
            }
        }

        private void rbSQLServer_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSQLServer.Checked)
            {
                rbMySQL.Checked = false;
                lblPort.Visible = false;
                tbPort.Visible = false;
                tbServer.Text = Constants.sqlServerDefault;
                tbDatabase.Text = Constants.dataBaseDefault;
                lblCaseSensitive.Visible = true;
            }
            else
            {
                rbMySQL.Checked = true;
                lblPort.Visible = true;
                tbPort.Visible = true;
                tbPort.Text = Constants.mysqlPortDefault;
                tbServer.Text = Constants.mysqlServerDefault;
                tbDatabase.Text = Constants.dataBaseDefault;
                lblCaseSensitive.Visible = false;
            }

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbServer.Text.Trim().Equals("") || tbDatabase.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("emptyServerORDatabase", culture));
                    return;
                }

                bool doChange = true;
                if (this.from.Equals(Constants.dbSetupInfo))
                {
                    DialogResult result = MessageBox.Show(rm.GetString("DBSetupInfo", culture), "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No)
                    {
                        doChange = false;
                        Controller.DBChanged(false);
                    }
                    else
                    {
                        Controller.DBChanged(true);
                    }
                }

                if (doChange)
                {
                    string connectionString = createConnectionString();

                    // encrypt a string to a byte array.
                    byte[] buffer = Util.Misc.encrypt(connectionString);

                    string connStringCrypted = Convert.ToBase64String(buffer);

                    Util.Misc.configAdd("connectionString", connStringCrypted);
              }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string createConnectionString()
        {
            string connectionString = "";
            StringBuilder connString = new StringBuilder();

            // create connection string using entered values
            if (rbMySQL.Checked)
            {
                connString.Append(Constants.dataProvider);
                connString.Append(Constants.dataProviderMySQL);
                connString.Append(Constants.server);
                if (tbPort.Text.Trim().Equals(""))
                {
                    connString.Append(tbServer.Text.Trim() + ";");
                }
                else
                {
                    connString.Append(tbServer.Text.Trim() + ";");
                    connString.Append(Constants.port);
                    connString.Append(tbPort.Text.Trim() + ";");
                }
                connString.Append(Constants.mysqlUID);
                connString.Append(Constants.mysqlPwd);
                connString.Append(" ");
                connString.Append(Constants.dataBase);
                connString.Append(tbDatabase.Text.Trim() + ";");
                connString.Append(Constants.pooling);
            }
            else
            {
                connString.Append(Constants.dataProvider);
                connString.Append(Constants.dataProviderSQLServer);
                connString.Append(Constants.server);
                connString.Append(tbServer.Text.Trim() + ";");
                connString.Append(Constants.dataBase);
                connString.Append(tbDatabase.Text.Trim() + ";");
                connString.Append(Constants.sqlServerUID);
                connString.Append(Constants.sqlServerPwd);
            }

            connectionString = connString.ToString().Trim();

            return connectionString;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbServer.Text.Trim().Equals("") || tbDatabase.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("emptyServerORDatabase", culture));
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

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
                    connectionString = connectionString.Substring(startIndex);
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
                            MySqlConnection connection;

                            connection = new MySqlConnection(connectionString);
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
            catch(Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                MessageBox.Show(rm.GetString("connFailed", culture) + " Error -> " + ex.Message);
            }
        }
    }
}