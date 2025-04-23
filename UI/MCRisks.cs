using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Util;
using System.Resources;
using TransferObjects;
using System.Globalization;
using Common;
using System.Collections;

namespace UI
{
    public partial class MCRisks : UserControl
    {
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;
        private List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();
        private string wuString = "";
        private List<int> wuList = new List<int>();
        private int startIndex;
        List<RiskTO> risks = new List<RiskTO>();

        private ListViewItemComparer _comp;
        const int Company = 0;
        const int Code = 1;
        const int Desc = 2;
        const int Rotation = 3;
        List<ApplRoleTO> currentRoles;
        Hashtable menuItemsPermissions;

        string menuItemID;
        string menuItemUsedID;

        bool addUsedPermission = false;
        bool updateUsedPermission = false;
        bool deleteUsedPermission = false;

        public MCRisks()
        {
            try
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
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                log.writeLog("Error in MCRisks.MCRisks: " + ex.Message + " " + DateTime.Now.ToString(Constants.dateFormat + " " + Constants.timeFormat));
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " MCRisks.populateCompany(): " + ex.Message + "\n");
                throw ex;
            }
        }
        private void populateRotation()
        {

            List<string> rotation = new List<string>();
            rotation.Add("*");
            for (int i = 0; i < 25; i++)
            {

                rotation.Add(i.ToString());
            }
            cmbDefRot.DataSource = rotation;
        }

        private void MCRisks_Load(object sender, EventArgs e)
        {
            try
            {
                setLanguage();
                _comp = new ListViewItemComparer(lvRisks);
                lvRisks.ListViewItemSorter = _comp;
                _comp.SortColumn = Company;
                lvRisks.Sorting = SortOrder.Ascending;

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();

                menuItemID = NotificationController.GetCurrentMenuItemID();
                int index = menuItemID.LastIndexOf('_');

                menuItemUsedID = menuItemID + "_" + rm.GetString("tpRisk", culture);
                setVisibility();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                log.writeLog("Error in MCRisks.MCRisks_Load: " + ex.Message + " " + DateTime.Now.ToString(Constants.dateFormat + " " + Constants.timeFormat));
            }
        }
        private void setLanguage()
        {

            lvRisks.BeginUpdate();
            lvRisks.Columns.Add(rm.GetString("hdrCompany", culture), (lvRisks.Width - 5) / 4 - 4, HorizontalAlignment.Right);
            lvRisks.Columns.Add(rm.GetString("hdrRiskCode", culture), (lvRisks.Width - 5) / 4 - 20, HorizontalAlignment.Left);
            lvRisks.Columns.Add(rm.GetString("hdrDesc", culture), (lvRisks.Width - 5) / 4 - 20, HorizontalAlignment.Left);
            lvRisks.Columns.Add(rm.GetString("hdrDefRotation", culture), (lvRisks.Width - 5) / 4 + 28, HorizontalAlignment.Left);
            lvRisks.EndUpdate();

            lblCompany.Text = rm.GetString("lblCompany", culture);
            lblDefaultRot.Text = rm.GetString("lblDefRotation", culture);
            lblDesc.Text = rm.GetString("lblDesc", culture);
            lblCode.Text = rm.GetString("lblRiskCode", culture);
            btnSearch.Text = rm.GetString("btnSearch", culture);
            btnAdd.Text = rm.GetString("btnAdd", culture);
            btnUpdate.Text = rm.GetString("btnUpdate", culture);
            btnDelete.Text = rm.GetString("btnDelete", culture);

            // groupbox text
            this.gbSearchRisk.Text = rm.GetString("gbSearch", culture);

            populateCompany();
            populateRotation();
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
                    case MCRisks.Company:
                    case MCRisks.Rotation:  
                    case MCRisks.Desc:
                    case MCRisks.Code:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }

                    default:
                        return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                }
            }
        }

        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                int company = -1;
                int rotation = -1;
                if (cbCompany.SelectedIndex > 0)
                    company = (int)cbCompany.SelectedValue;
                if (cmbDefRot.SelectedIndex > 0)
                    rotation = (int)cmbDefRot.SelectedValue;

                string code = txtCode.Text.Trim();
                string desc = txtDesc.Text.Trim();

                RiskTO riskTO = new RiskTO();
                riskTO.WorkingUnitID = company;
                riskTO.RiskCode = code;
                riskTO.DefaultRotation = rotation;
                if (logInUser.LangCode.Equals(Constants.Lang_en))
                    riskTO.DescEN = desc;
                else riskTO.DescSR = desc;

                Risk risk = new Risk();
                risk.RiskTO = riskTO;
                risks = risk.SearchRisks();
                if (risks.Count > 0)
                    populateList(risks, startIndex);
                else MessageBox.Show(rm.GetString("noRisksFound", culture));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.writeLog("Error in MCRisks.btnSearch_Click: " + ex.Message + " " + DateTime.Now.ToString(Constants.dateFormat + " " + Constants.timeFormat));
            }
        }

        private void populateList(List<RiskTO> risks, int startIndex)
        {
            try
            {
                if (risks.Count > Constants.recordsPerPage)
                {
                    btnPrev.Visible = true;
                    btnNext.Visible = true;
                }
                else
                {
                    btnPrev.Visible = false;
                    btnNext.Visible = false;
                }

                lvRisks.BeginUpdate();
                lvRisks.Items.Clear();

                if (risks.Count > 0)
                {
                    if ((startIndex >= 0) && (startIndex < risks.Count))
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
                        if (lastIndex >= risks.Count)
                        {
                            btnNext.Enabled = false;
                            lastIndex = risks.Count;
                        }
                        else
                        {
                            btnNext.Enabled = true;
                        }

                        for (int i = startIndex; i < lastIndex; i++)
                        {
                            RiskTO risk = risks[i];
                            ListViewItem item = new ListViewItem();

                            EmployeeTO empl = new EmployeeTO();

                            WorkingUnitTO company = new WorkingUnitTO();
                            WorkingUnit wu = new WorkingUnit();
                            company = new WorkingUnit().FindWU(risk.WorkingUnitID);

                            item.Text = company.Description;

                            item.SubItems.Add(risk.RiskCode);
                            if (logInUser.LangCode.Equals(Constants.Lang_en))
                                item.SubItems.Add(risk.DescEN);
                            else item.SubItems.Add(risk.DescSR);
                            item.SubItems.Add(risk.DefaultRotation.ToString());

                            item.Tag = risk;
                            lvRisks.Items.Add(item);
                        }
                    }
                }

                lvRisks.EndUpdate();
                lvRisks.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MCRisks.populateList(): " + ex.Message + "\n");
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
                populateList(risks, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MCRisks.btnPrev_Click(): " + ex.Message + "\n");
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
                populateList(risks, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MCRisks.btnNext_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            new MCRisksAdd(wuString).ShowDialog();
            btnSearch_Click(this, new EventArgs());
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lvRisks.SelectedItems.Count == 1)
            {
                new MCRisksAdd(wuString, (RiskTO)lvRisks.SelectedItems[0].Tag).ShowDialog();
                btnSearch_Click(this, new EventArgs());
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvRisks.SelectedItems.Count == 1)
                {
                    Risk risk = new Risk();
                    risk.RiskTO = (RiskTO)lvRisks.SelectedItems[0].Tag;
                    risk.Delete(risk.RiskTO.RiskID, false);
                    btnSearch_Click(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MCRisks.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        private void lvVRisks_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                SortOrder prevOrder = lvRisks.Sorting;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvRisks.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvRisks.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvRisks.Sorting = SortOrder.Ascending;
                }

                lvRisks.Sort();
                startIndex = 0;
                populateList(risks, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MCRisks.lvVRisks_ColumnClick(): " + ex.Message + "\n");
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
