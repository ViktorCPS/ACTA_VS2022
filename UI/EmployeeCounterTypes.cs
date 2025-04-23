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
    public partial class EmployeeCounterTypes : System.Windows.Forms.Form
    {

        List<EmployeeCounterTypeTO> currentTypeList;
        EmployeeCounterTypeTO currentEmplCountType;

        // List View indexes		
        const int CounterTypeIndex = 0;
        const int NameIndex = 1;
        const int AltNameIndex = 2;
        const int DescriotionIndex = 3;

        private CultureInfo culture;
        private ResourceManager rm;

        ApplUserTO logInUser;
       
        DebugLog log;

        public EmployeeCounterTypes()
        {
            InitializeComponent();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(SystemMessages).Assembly);

            logInUser = NotificationController.GetLogInUser();

            this.CenterToScreen();
            setLanguage();

            currentTypeList = new List<EmployeeCounterTypeTO>();
            populateNameTypeCombo();
        }

        private void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("EmployeeCounterTypesForm", culture);

                // group box text
                gbEmployeeCounterTypes.Text = rm.GetString("gbEmployeeCounterTypes", culture);

                // button's text
                btnAdd.Text = rm.GetString("btnAdd", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnDelete.Text = rm.GetString("btnDelete", culture);
                btnSearch.Text = rm.GetString("btnSearch", culture);
                btnClose.Text = rm.GetString("btnClose", culture);

                // label's text
                lblName.Text = rm.GetString("lblName", culture);

                lvECTypes.BeginUpdate();
                lvECTypes.Columns.Add(rm.GetString("CounterTypeID", culture), 70, HorizontalAlignment.Left);
                lvECTypes.Columns.Add(rm.GetString("Name", culture), 120, HorizontalAlignment.Left);
                lvECTypes.Columns.Add(rm.GetString("AltName", culture), 120, HorizontalAlignment.Left);
                lvECTypes.Columns.Add(rm.GetString("Description", culture), 230, HorizontalAlignment.Left);
                lvECTypes.EndUpdate();

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterTypes.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        public void populateListView(List<EmployeeCounterTypeTO> emCountTypeList)
        {
            try
            {
                lvECTypes.BeginUpdate();
                lvECTypes.Items.Clear();

                if (emCountTypeList.Count > 0)
                {
                    foreach (EmployeeCounterTypeTO type in emCountTypeList)
                    {
                        ListViewItem item = new ListViewItem();
                        item = lvECTypes.Items.Add(type.EmplCounterTypeID.ToString());
                        item.SubItems.Add(type.Name.Trim());
                        item.SubItems.Add(type.NameAlt.Trim());
                        item.SubItems.Add(type.Desc.Trim());
                        item.Tag = type;
                    }
                }
                lvECTypes.EndUpdate();
                lvECTypes.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterTypes.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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
                log.writeLog(DateTime.Now + " EmployeeCounterTypes.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateNameTypeCombo()
        {
            try
            {
                List<EmployeeCounterTypeTO> emplCountTypeArray = new EmployeeCounterType().Search();
                emplCountTypeArray.Insert(0, new EmployeeCounterTypeTO(-1, rm.GetString("all", culture), rm.GetString("all", culture), rm.GetString("all", culture)));

                cbName.DataSource = emplCountTypeArray;
                cbName.DisplayMember = "Name";
                cbName.ValueMember = "EmplCounterTypeID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterTypse.populateNameTypeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
       
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

              //  string name = "";
                int id = 0;
                if (cbName.SelectedIndex > 0)
                {
                    id = Int32.Parse(cbName.SelectedValue.ToString().Trim());
                }
                List<EmployeeCounterTypeTO> emCountTypeList = new List<EmployeeCounterTypeTO>();
                EmployeeCounterType emCountType = new EmployeeCounterType();
                emCountType.TypeTO.EmplCounterTypeID = id;
                currentTypeList = emCountType.Search();
                

                if (currentTypeList.Count > 0)
                {
                    populateListView(currentTypeList);
                }
                else
                {
                   MessageBox.Show(rm.GetString("noEmployeeCounterTypesFound", culture));
                }

                currentEmplCountType = new EmployeeCounterTypeTO();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterType.btnSearch_Click(): " + ex.Message + "\n");
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
                EmployeeCounterTypesAdd addForm = new EmployeeCounterTypesAdd();
                addForm.ShowDialog(this);

                currentEmplCountType = new EmployeeCounterTypeTO();
                List<EmployeeCounterTypeTO> typeList = new EmployeeCounterType().Search();
                populateListView(typeList);
                populateNameTypeCombo();
                this.Invalidate();
    
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterTypes.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (this.lvECTypes.SelectedItems.Count != 1)
                {
                    MessageBox.Show(rm.GetString("selOneType", culture));
                }
                else
                {
                    currentEmplCountType = (EmployeeCounterTypeTO)lvECTypes.SelectedItems[0].Tag;

                    // Open Update Form
                    EmployeeCounterTypesAdd addForm = new EmployeeCounterTypesAdd(currentEmplCountType);
                    addForm.ShowDialog(this);

                   List<EmployeeCounterTypeTO> typeList = new EmployeeCounterType().Search();
                    populateListView(typeList);

                    cbName.SelectedIndex = 0;
                    currentEmplCountType = new EmployeeCounterTypeTO();
                    this.Invalidate();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterTypes.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;


                if (lvECTypes.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noSelCounterTypeDel", culture));
                }
                else
                {
                    DialogResult result = MessageBox.Show(rm.GetString("deleteCounterType", culture), "", MessageBoxButtons.YesNo);
                    bool isDeleted = true;

                    if (result == DialogResult.Yes)
                    {
                        int selected = lvECTypes.SelectedItems.Count;

                        foreach (ListViewItem item in lvECTypes.SelectedItems)
                        {
                            currentEmplCountType = (EmployeeCounterTypeTO)lvECTypes.SelectedItems[0].Tag;
                            EmployeeCounterType type = new EmployeeCounterType();
                            type.TypeTO = currentEmplCountType;
                            isDeleted = new EmployeeCounterType().Delete(currentEmplCountType.EmplCounterTypeID) && isDeleted;
                        }

                        if ((selected > 0) && isDeleted)
                        {
                            MessageBox.Show(rm.GetString("counterTypeDel", culture));
                        }
                        else
                            if (!isDeleted)
                            {
                                MessageBox.Show(rm.GetString("noCounterTypeDel", culture));
                            }

                        List<EmployeeCounterTypeTO> typeList = new EmployeeCounterType().Search();
                        populateListView(typeList);
                        populateNameTypeCombo();
                        cbName.SelectedIndex = 0;

                        currentEmplCountType = new EmployeeCounterTypeTO();
                        this.Invalidate();
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterTypes.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}
