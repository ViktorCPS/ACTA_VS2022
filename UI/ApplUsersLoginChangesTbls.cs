using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Resources;
using System.Globalization;

using TransferObjects;
using Util;
using Common;

namespace UI
{
    public partial class ApplUsersLoginChangesTbls : Form
    {
        private CultureInfo culture;
        private ResourceManager rm;

        ApplUserTO logInUser;

        DebugLog log;

        List<ApplUsersLoginChangesTblTO> currentTableList;
        ApplUsersLoginChangesTblTO currentTable;

        List<string> listToSaveToDB;

        public bool reloadOnReturn;

        public ApplUsersLoginChangesTbls()
        {
            InitializeComponent();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(SystemMessages).Assembly);

            logInUser = NotificationController.GetLogInUser();

            this.CenterToScreen();

            currentTableList = new List<ApplUsersLoginChangesTblTO>();
            currentTable = new ApplUsersLoginChangesTblTO();

            setLanguage();

            populateLVChangesTable();
            populateLVAllTables();
        }

        private void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("ApplUsersLoginChangesTbl", culture);

                // group box text
                gbULChangesTbl.Text = rm.GetString("gbApplUsersLoginChangesTbl", culture);

                // button's text
                btnSave.Text = rm.GetString("btnSave", culture);
                btnClose.Text = rm.GetString("btnClose", culture);

                lvULChangesTbl.BeginUpdate();
                lvULChangesTbl.Columns.Add(rm.GetString("TableName", culture), 295,HorizontalAlignment.Left);
                lvULChangesTbl.EndUpdate();

                lvAllTables.BeginUpdate();
                lvAllTables.Columns.Add(rm.GetString("TableName", culture),280, HorizontalAlignment.Left);
                lvAllTables.EndUpdate();

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersLoginChangesTbls.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        public void populateLVChangesTable()
        {
            try
            {
                lvULChangesTbl.BeginUpdate();
                lvULChangesTbl.Items.Clear();

                List<string> loginChangesTables = new ApplUsersLoginChangesTbl().SearchTableNames();

                if (loginChangesTables.Count > 0)
                {
                    listToSaveToDB = loginChangesTables;

                    foreach (string table in loginChangesTables)
                    {
                        ListViewItem item = new ListViewItem();
                        item = lvULChangesTbl.Items.Add(table.Trim());
                        item.Tag = table;
                    }

                }
                lvULChangesTbl.EndUpdate();
                lvULChangesTbl.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersLoginChangesTbl.populateLVChangesTable(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        public void populateLVAllTables()
        {
            try
            {
                lvAllTables.BeginUpdate();
                lvAllTables.Items.Clear();

                List<string> allTables = new ApplUsersLoginChangesTbl().SearchAllTableNames();

                if (allTables.Count > 0)
                {
                    foreach (string table in allTables)
                    {
                        if (!listToSaveToDB.Contains(table))
                        {
                            ListViewItem item = new ListViewItem();
                            item = lvAllTables.Items.Add(table.Trim());
                            item.Tag = table;
                        }
                    }
                }
                lvULChangesTbl.EndUpdate();
                lvULChangesTbl.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersLoginChangesTbl.populateLVChangesTable(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (listToSaveToDB.Count > 0)
                {
                    if (MessageBox.Show(rm.GetString("ULChangeTblSaveChanges", culture),"", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {

                        bool isDeleted = new ApplUsersLoginChangesTbl().Delete();
                        if (isDeleted)
                        {
                            int inserted = 0;
                            foreach (string table in listToSaveToDB)
                            {
                                inserted += new ApplUsersLoginChangesTbl().Save(table);
                            }
                            if (inserted > 0)
                            {
                                if (inserted == listToSaveToDB.Count)
                                {
                                    MessageBox.Show(rm.GetString("ULChangesTableSuccess", culture));
                                }
                                else
                                {
                                    MessageBox.Show(rm.GetString("ULChangesTableNotAllSuccess", culture));
                                }
                            }
                            else
                            {
                                MessageBox.Show(rm.GetString("ULChangesTableNotSuccess", culture));
                            }
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("ULChangesTableNotDeleted", culture));
                        }
                    }
                 }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersLoginChangesTbl.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersLoginChangesTbls.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (this.lvAllTables.SelectedItems.Count < 1)
                {
                    MessageBox.Show(rm.GetString("selOneTable", culture));
                }
                else
                {
                    List<ListViewItem> itemsToTransfer = new List<ListViewItem>();
                    
                    foreach (ListViewItem transferItem in lvAllTables.SelectedItems)
                    {
                        itemsToTransfer.Add(transferItem);
                    }

                    foreach (ListViewItem item in itemsToTransfer)
                    {
                        lvAllTables.Items.Remove(item);
                        lvULChangesTbl.Items.Add(item);
                        listToSaveToDB.Add(item.Text.ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersCategory.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (this.lvULChangesTbl.SelectedItems.Count < 1)
                {
                    MessageBox.Show(rm.GetString("selOneTable", culture));
                }
                else
                {
                    List<ListViewItem> itemsToTransfer = new List<ListViewItem>();

                    foreach (ListViewItem transferItem in lvULChangesTbl.SelectedItems)
                    {
                        itemsToTransfer.Add(transferItem);
                    }

                    foreach (ListViewItem item in itemsToTransfer)
                    {
                        lvULChangesTbl.Items.Remove(item);
                        lvAllTables.Items.Add(item);
                        listToSaveToDB.Remove(item.Text.ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersCategory.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

    }
}
