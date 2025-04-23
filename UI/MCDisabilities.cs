using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TransferObjects;
using Common;
using Util;
using System.Resources;
using System.Globalization;
using System.Collections;


namespace UI
{
    public partial class MCDisabilities : UserControl
    {
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;
        private List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();
        private string wuString = "";
        private List<int> wuList = new List<int>();
        private int startIndex;
        List<MedicalCheckDisabilityTO> disabilities = new List<MedicalCheckDisabilityTO>();

        private ListViewItemComparer _comp;
        const int Company = 0;
        const int Code = 1;
        const int Desc = 2;

        List<ApplRoleTO> currentRoles;
        Hashtable menuItemsPermissions;

        string menuItemID;
        string menuItemUsedID;

        bool addUsedPermission = false;
        bool updateUsedPermission = false;
        bool deleteUsedPermission = false;

        public MCDisabilities()
        {
            InitializeComponent();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
            logInUser = NotificationController.GetLogInUser();
            rm = new ResourceManager("UI.Resource", typeof(MCRisks).Assembly);
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

            if (logInUser != null)
            {
                wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.EmployeesPurpose);

            }

            foreach (WorkingUnitTO wUnit in wUnits)
            {
                wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                wuList.Add(wUnit.WorkingUnitID);
            }

            if (wuString.Length > 0)
                wuString = wuString.Substring(0, wuString.Length - 1);
            btnPrev.Visible = false;
            btnNext.Visible = false;
        }
        #region Inner Class for sorting items in View List

        /*
		 *  Class used for sorting items in the List View 
		 */
        private class ListViewItemComparer : IComparer
        {
            private ListView _listView;
            private CultureInfo culture;

            public ListViewItemComparer(ListView lv)
            {
                _listView = lv;
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            }
            public ListView ListView
            {
                get { return _listView; }
            }

            private int _sortColumn = 0;

            public int SortColumn
            {
                get { return _sortColumn; }
                set { _sortColumn = value; }
            }

            public int Compare(object a, object b)
            {
                ListViewItem item1 = (ListViewItem)a;
                ListViewItem item2 = (ListViewItem)b;

                if (ListView.Sorting == SortOrder.Descending)
                {
                    ListViewItem temp = item1;
                    item1 = item2;
                    item2 = temp;
                }
                // Handle non Detail Cases
                return CompareItems(item1, item2);
            }

            public int CompareItems(ListViewItem item1, ListViewItem item2)
            {
                // Subitem instances
                ListViewItem.ListViewSubItem sub1 = item1.SubItems[SortColumn];
                ListViewItem.ListViewSubItem sub2 = item2.SubItems[SortColumn];

                // Return value based on sort column	
                switch (SortColumn)
                {
                    case MCDisabilities.Company: 
                    case MCDisabilities.Desc:
                    case MCDisabilities.Code:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }

                    default:
                        return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                }
            }
        }

        #endregion
        private void populateList(List<MedicalCheckDisabilityTO> disabilities, int startIndex)
        {
            try
            {
                if (disabilities.Count > Constants.recordsPerPage)
                {
                    btnPrev.Visible = true;
                    btnNext.Visible = true;
                }
                else
                {
                    btnPrev.Visible = false;
                    btnNext.Visible = false;
                }

                lvDisabilities.BeginUpdate();
                lvDisabilities.Items.Clear();

                if (disabilities.Count > 0)
                {
                    if ((startIndex >= 0) && (startIndex < disabilities.Count))
                    {
                        if (startIndex == 0)
                        {
                            btnPrev.Enabled = false;
                        }
                        else
                        {
                            btnPrev.Enabled = true;
                        }

                        int lastIndex = startIndex + Constants.recordsPerPage;
                        if (lastIndex >= disabilities.Count)
                        {
                            btnNext.Enabled = false;
                            lastIndex = disabilities.Count;
                        }
                        else
                        {
                            btnNext.Enabled = true;
                        }

                        for (int i = startIndex; i < lastIndex; i++)
                        {
                            
                                MedicalCheckDisabilityTO disability = disabilities[i];
                                ListViewItem item = new ListViewItem();


                                WorkingUnitTO company = new WorkingUnitTO();
                                WorkingUnit wu = new WorkingUnit();
                                company = new WorkingUnit().FindWU(disability.WorkingUnitID);

                                item.Text = company.Description;
                                item.SubItems.Add(disability.DisabilityCode);

                                if (logInUser.LangCode.Equals(Constants.Lang_en))
                                    item.SubItems.Add(disability.DescEN);
                                else item.SubItems.Add(disability.DescSR);

                                item.Tag = disability;
                                lvDisabilities.Items.Add(item);
                          
                        }
                    }
                }

                lvDisabilities.EndUpdate();
                lvDisabilities.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MCDisabilities.populateList(): " + ex.Message + "\n");
                throw ex;

            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                startIndex -= Constants.recordsPerPage;
                if (startIndex < 0)
                {
                    startIndex = 0;
                }
                populateList(disabilities, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MCDisabilities.btnPrev_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                startIndex += Constants.recordsPerPage;
                populateList(disabilities, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MCDisabilities.btnNext_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            new MCDisabilitiesAdd(wuString).ShowDialog();
            btnSearch_Click(this, new EventArgs());
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lvDisabilities.SelectedItems.Count == 1)
            {
                new MCDisabilitiesAdd(wuString, (MedicalCheckDisabilityTO)lvDisabilities.SelectedItems[0].Tag).ShowDialog();
                btnSearch_Click(this, new EventArgs());
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvDisabilities.SelectedItems.Count == 1)
                {
                    MedicalCheckDisability risk = new MedicalCheckDisability();
                    risk.DisabilityTO = (MedicalCheckDisabilityTO)lvDisabilities.SelectedItems[0].Tag;
                    risk.Delete(risk.DisabilityTO.DisabilityID, false);
                    btnSearch_Click(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MCDisabilties.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                int company = -1;

                if (cbCompany.SelectedIndex > 0)
                    company = (int)cbCompany.SelectedValue;


                string code = txtCode.Text.Trim();
                string desc = txtDesc.Text.Trim();

                MedicalCheckDisabilityTO riskTO = new MedicalCheckDisabilityTO();
                riskTO.WorkingUnitID = company;
                riskTO.DisabilityCode = code;

                if (logInUser.LangCode.Equals(Constants.Lang_en))
                    riskTO.DescEN = desc;
                else riskTO.DescSR = desc;

                MedicalCheckDisability risk = new MedicalCheckDisability();
                risk.DisabilityTO = riskTO;
                disabilities = risk.SearchMedicalCheckDisabilities();
                if (disabilities.Count > 0)
                    populateList(disabilities, startIndex);
                else
                {
                    populateList(new List<MedicalCheckDisabilityTO>(), startIndex);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.writeLog("Error in MCDisabilties.btnSearch_Click: " + ex.Message + " " + DateTime.Now.ToString(Constants.dateFormat + " " + Constants.timeFormat));
            }
        }
        private void setLanguage()
        {

            try
            {
                lvDisabilities.BeginUpdate();
                lvDisabilities.Columns.Add(rm.GetString("hdrCompany", culture), (lvDisabilities.Width - 5) / 3 - 4, HorizontalAlignment.Right);
                lvDisabilities.Columns.Add(rm.GetString("hdrRiskCode", culture), (lvDisabilities.Width - 5) / 3 - 10, HorizontalAlignment.Left);
                lvDisabilities.Columns.Add(rm.GetString("hdrDesc", culture), (lvDisabilities.Width - 5) / 3 - 10, HorizontalAlignment.Left);

                lvDisabilities.EndUpdate();

                // groupbox text
                this.gbSearchRisk.Text = rm.GetString("gbSearch", culture);

                lblCompany.Text = rm.GetString("lblCompany", culture);

                lblDesc.Text = rm.GetString("lblDesc", culture);
                lblCode.Text = rm.GetString("lblDisabilityCode", culture);
                btnSearch.Text = rm.GetString("btnSearch", culture);
                btnAdd.Text = rm.GetString("btnAdd", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnDelete.Text = rm.GetString("btnDelete", culture);

                populateCompany();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " MCDisabilties.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }

        }
        private void populateCompany()
        {
            try
            {
                List<WorkingUnitTO> companyAllList = new WorkingUnit().getRootWorkingUnitsList(wuString);
                List<WorkingUnitTO> companyList = new List<WorkingUnitTO>();
                WorkingUnitTO wuDef = new WorkingUnitTO();
                wuDef.Name = "*";
                wuDef.WorkingUnitID = -1;
                companyList.Add(wuDef);
                Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rules = new Common.Rule().SearchTypeAllRules(Constants.RuleRestaurant);

                foreach (WorkingUnitTO wu in companyAllList)
                {
                    if (!rules.ContainsKey(wu.WorkingUnitID))
                        continue;

                    bool useRestaurant = false;
                    foreach (int type in rules[wu.WorkingUnitID].Keys)
                    {
                        foreach (string ruleType in rules[wu.WorkingUnitID][type].Keys)
                        {
                            if (rules[wu.WorkingUnitID][type][ruleType].RuleValue == Constants.yesInt)
                            {
                                useRestaurant = true;
                                break;
                            }
                        }

                        if (useRestaurant)
                            break;
                    }

                    if (useRestaurant)
                        companyList.Add(wu);
                }

                if (companyList.Count > 0)
                {
                    cbCompany.DataSource = companyList;
                    cbCompany.DisplayMember = "Name";
                    cbCompany.ValueMember = "WorkingUnitID";

                    //btnValidate.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " MCDisabilties.populateCompany(): " + ex.Message + "\n");
                throw ex;
            }
        }
        private void lvVRisks_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                SortOrder prevOrder = lvDisabilities.Sorting;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvDisabilities.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvDisabilities.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvDisabilities.Sorting = SortOrder.Ascending;
                }

                lvDisabilities.Sort();
                startIndex = 0;
                populateList(disabilities, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MCDisabilties.lvVRisks_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void MCDisabilities_Load(object sender, EventArgs e)
        {
            try
            {
                setLanguage();
                _comp = new ListViewItemComparer(lvDisabilities);
                lvDisabilities.ListViewItemSorter = _comp;
                _comp.SortColumn = Company;
                lvDisabilities.Sorting = SortOrder.Ascending;


                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();

                menuItemID = NotificationController.GetCurrentMenuItemID();
                int index = menuItemID.LastIndexOf('_');

                menuItemUsedID = menuItemID + "_" + rm.GetString("tpDisabilities", culture);
                setVisibility();

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MCDisabilties.MCDisabilities_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        private void setVisibility()
        {
            try
            {
                int permissionUsed;

                foreach (ApplRoleTO role in currentRoles)
                {
                    permissionUsed = (((int[])menuItemsPermissions[menuItemUsedID])[role.ApplRoleID]);

                    addUsedPermission = addUsedPermission || (((permissionUsed / 4) % 2) == 0 ? false : true);
                    updateUsedPermission = updateUsedPermission || (((permissionUsed / 2) % 2) == 0 ? false : true);
                    deleteUsedPermission = deleteUsedPermission || ((permissionUsed % 2) == 0 ? false : true);

                }

                btnAdd.Enabled = addUsedPermission;
                btnUpdate.Enabled = updateUsedPermission;
                btnDelete.Enabled = deleteUsedPermission;
             
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MCDisabilties.setVisibility(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}
