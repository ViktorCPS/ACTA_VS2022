using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;


namespace ACTADBSetup
{
    public partial class SetupWizardMainForm : Form
    {
        private string dataFilePath = string.Empty;

        public SetupWizardMainForm()
        {
            InitializeComponent();
        }

        private void rbDatabaseType_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMSSQLServer.Checked)
            {
                if (txtHostName.Text == "localhost") txtHostName.Text = "(local)";
                lblHostName.Text = "Database server (default or named instance)";
                lblDBAPassword.Text = "Database sa password";
                lblHostPort.Enabled = txtHostPort.Enabled = false;
                txtHostPort.Text = "";
                btnAdvanced.Enabled = true;
            }
            else if (rbMySQL.Checked)
            {
                if (txtHostName.Text == "(local)") txtHostName.Text = "localhost";
                lblHostName.Text = "Database server (host name or IP address)";
                lblDBAPassword.Text = "Database root password";
                lblHostPort.Enabled = txtHostPort.Enabled = true;
                if (txtHostPort.Text == "") txtHostPort.Text = "3306";
                btnAdvanced.Enabled = false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            try
            {
                string logFilePath = string.Empty;

                string arguments = string.Empty;

                string overwriteExistingDatabase = cbOverwriteExistingDatabase.Checked ? "OVERWRITE" : "NOOVERWRITE";

                System.Diagnostics.Process process = null;

                if (rbMSSQLServer.Checked)
                {
                    if (dataFilePath == String.Empty)
                    {
                        try
                        {
                            dataFilePath = GetSQLServerDataFilePath();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ACTA Database Setup exception: " + ex.Message);
                            return;
                        }
                    }
                    logFilePath = @".\MSSQL\Log\outputlog.txt";
                    dataFilePath = "\"" + dataFilePath.Replace(@"\\", @"\") + "\"";
                    arguments = txtHostName.Text + " " + txtDBAPassword.Text + " " + dataFilePath + " " + overwriteExistingDatabase + " actamgr2005";
                    btnInstall.Enabled = false;
                    process = System.Diagnostics.Process.Start("installmssql.bat",arguments);
                }
                else
                {
                    try
                    {
                        CheckMySQLConnection();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ACTA Database Setup exception: " + ex.Message);
                        return;
                    }
                    logFilePath = @".\MYSQL\Log\outputlog.txt";
                    arguments = txtHostName.Text + " " + txtHostPort.Text + " " + txtDBAPassword.Text + " " + overwriteExistingDatabase + " password";
                    btnInstall.Enabled = false;
                    process = System.Diagnostics.Process.Start("installmysql.bat", arguments);
                }
                int seconds = 0; while ((!process.HasExited) && (seconds < 300)) { Thread.Sleep(1000); seconds++; }
                this.Focus();
                if (MessageBox.Show("Database setup completed! Do you want to see the installation log?","Info",MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("notepad.exe", logFilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ACTA Database Setup exception: " + ex.Message);
                return;
            }
        }

        private void btnAdvanced_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                folderBrowserDialog.Description = "Choose data file path for your ACTA system database schemas if you don't want to use the default data path.";
                folderBrowserDialog.SelectedPath = GetSQLServerDataFilePath();
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    dataFilePath = folderBrowserDialog.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ACTA Database Setup exception: " + ex.Message);
                return;
            }
        }

        private string GetSQLServerDataFilePath()
        {
            try
            {
                string dfp = string.Empty;
                string connectionString = "server=" + txtHostName.Text + ";database=master;uid=sa;pwd=" + txtDBAPassword.Text;

                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand cmd = new SqlCommand("Select Reverse(substring(ltrim(reverse([FileName])),charindex('\',Ltrim(reverse([Filename]))),datalength(Ltrim(reverse([Filename]))))) AS DataFilePath From sysfiles Where groupid = 1", connection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet, "DataFilePathTable");
                DataTable table = dataSet.Tables["DataFilePathTable"];
                connection.Close();
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (!row["DataFilePath"].Equals(DBNull.Value))
                        {
                            dfp = (string)row["DataFilePath"];
                            int lastSeparatorPosition = dfp.LastIndexOf(@"\");
                            dfp = dfp.Substring(0, lastSeparatorPosition);
                        }
                    }
                }
                return dfp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CheckMySQLConnection()
        {
            try
            {
                string connectionString = "server=" + txtHostName.Text + ";user id=root;password=" + txtDBAPassword.Text + ";"; 
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (rbMSSQLServer.Checked)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    GetSQLServerDataFilePath();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ACTA Database Setup exception: " + ex.Message);
                    return;
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
            else
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    CheckMySQLConnection();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ACTA Database Setup exception: " + ex.Message);
                    return;
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }

            MessageBox.Show("Database connection OK!");
        }
    }
}