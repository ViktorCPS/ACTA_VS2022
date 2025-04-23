using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Resources;
using System.Globalization;

using Common;
using TransferObjects;
using Util;

namespace UI
{
    public partial class Rules : Form
    {
        ApplUserTO logInUser;

        private CultureInfo culture;
        ResourceManager rm;
        DebugLog log;

        private ListViewItemComparer _comp;

        private const int CompanyNameIndex = 0;
        private const int EmployeeTypeIndex = 1;
        private const int RuleTypeIndex = 2;        
        private const int RuleValueIndex = 3;
        private const int RuleDate1Index = 4;
        private const int RuleDate2Index = 5;

        private List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();
        List<int> wuList = new List<int>();
        private string wuString = "";
        Dictionary<int, WorkingUnitTO> wuDict = new Dictionary<int, WorkingUnitTO>();

        Dictionary<int, List<EmployeeTypeTO>> companyTypes = new Dictionary<int, List<EmployeeTypeTO>>();
        Dictionary<int, Dictionary<int, string>> companyTypesDict = new Dictionary<int, Dictionary<int, string>>();
        
        List<ApplRoleTO> currentRoles;
        Hashtable menuItemsPermissions;
        string menuItemID;
        bool addPermission = false;
        bool updatePermission = false;
        bool deletePermission = false;

        public Rules()
        {
            try
            {
                InitializeComponent();
                this.CenterToScreen();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                logInUser = NotificationController.GetLogInUser();

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(Rules).Assembly);
                setLanguage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region Inner Class for sorting items in View List

        /*
		 *  Class used for sorting items in the List View 
		*/
        private class ListViewItemComparer : IComparer
        {
            private ListView _listView;

            public ListViewItemComparer(ListView lv)
            {
                _listView = lv;
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
                    case Rules.CompanyNameIndex:
                    case Rules.EmployeeTypeIndex:
                    case Rules.RuleTypeIndex:
                        return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                    case Rules.RuleValueIndex:
                        {
                            int val1 = -1;
                            int val2 = -1;
                            int.TryParse(sub1.Text, out val1);
                            int.TryParse(sub2.Text, out val2);
                            return val1.CompareTo(val2);
                        }
                    case Rules.RuleDate1Index:
                    case Rules.RuleDate2Index:
                        {
                            DateTime date1 = new DateTime();
                            DateTime date2 = new DateTime();
                            DateTime.TryParseExact(sub1.Text, Constants.dateFormat + " " + Constants.timeFormat, null, DateTimeStyles.None, out date1);
                            DateTime.TryParseExact(sub2.Text, Constants.dateFormat + " " + Constants.timeFormat, null, DateTimeStyles.None, out date2);
                            return date1.CompareTo(date2);
                        }
                    default:
                        return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                }
                
            }
        }

        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Rules_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Initialize comparer objects
                _comp = new ListViewItemComparer(lvRules);
                lvRules.ListViewItemSorter = _comp;
                lvRules.Sorting = SortOrder.Ascending;

                wuDict = new WorkingUnit().getWUDictionary();
                
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

                // get company types
                List<EmployeeTypeTO> typesList = new EmployeeType().Search();

                foreach (EmployeeTypeTO type in typesList)
                {
                    if (!companyTypes.ContainsKey(type.WorkingUnitID))
                        companyTypes.Add(type.WorkingUnitID, new List<EmployeeTypeTO>());

                    companyTypes[type.WorkingUnitID].Add(type);
                }

                companyTypesDict = new EmployeeType().SearchDictionary();
                                
                populateCompanies();
                populateRuleTypes();
                                
                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                menuItemID = NotificationController.GetCurrentMenuItemID();

                setVisibility();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rules.Rules_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void setVisibility()
        {
            try
            {
                int permissionUsed;

                foreach (ApplRoleTO role in currentRoles)
                {
                    permissionUsed = (((int[])menuItemsPermissions[menuItemID])[role.ApplRoleID]);

                    addPermission = addPermission || (((permissionUsed / 4) % 2) == 0 ? false : true);
                    updatePermission = updatePermission || (((permissionUsed / 2) % 2) == 0 ? false : true);
                    deletePermission = deletePermission || ((permissionUsed % 2) == 0 ? false : true);

                }

                btnUpdate.Visible = updatePermission;

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rules.setVisibility(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("Rules", culture);
                                
                // button's text
                btnClose.Text = rm.GetString("btnClose", culture);
                btnSearch.Text = rm.GetString("btnSearch", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);

                // label's text                
                lblEmplType.Text = rm.GetString("lblEmplType", culture);
                lblCompany.Text = rm.GetString("lblCompany", culture);
                lblType.Text = rm.GetString("lblType", culture);
                
                // group boxes
                gbFilter.Text = rm.GetString("gbSearchFilter", culture);
                
                // list view
                lvRules.BeginUpdate();
                lvRules.Columns.Add(rm.GetString("hdrComp", culture), lvRules.Width / 6 - 4, HorizontalAlignment.Left);
                lvRules.Columns.Add(rm.GetString("hdrEmplType", culture), lvRules.Width / 6 - 4, HorizontalAlignment.Left);
                lvRules.Columns.Add(rm.GetString("hdrRuleType", culture), lvRules.Width / 6 - 4, HorizontalAlignment.Left);                
                lvRules.Columns.Add(rm.GetString("hdrRuleValue", culture), lvRules.Width / 6 - 4, HorizontalAlignment.Left);
                lvRules.Columns.Add(rm.GetString("hdrRuleDate1", culture), lvRules.Width / 6 - 4, HorizontalAlignment.Left);
                lvRules.Columns.Add(rm.GetString("hdrRuleDate2", culture), lvRules.Width / 6 - 4, HorizontalAlignment.Left);
                lvRules.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rules.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateRules(List<RuleTO> ruleList)
        {
            try
            {
                tbRuleDesc.Text = "";

                lvRules.BeginUpdate();
                lvRules.Items.Clear();

                for (int i = 0; i < ruleList.Count; i++)
                {
                    if (ruleList[i].RuleType.Equals(Constants.RuleCutOffDate) || ruleList[i].RuleType.Equals(Constants.RuleHRSSCCutOffDate) 
                        || ruleList[i].RuleType.Equals(Constants.RuleWCDRCutOffDate) || !wuList.Contains(ruleList[i].WorkingUnitID))
                        continue;

                    ListViewItem item = new ListViewItem();

                    if (wuDict.ContainsKey(ruleList[i].WorkingUnitID))
                        item.Text = wuDict[ruleList[i].WorkingUnitID].Name.Trim();
                    else
                        item.Text = "N/A";
                    if (companyTypesDict.ContainsKey(ruleList[i].WorkingUnitID) && companyTypesDict[ruleList[i].WorkingUnitID].ContainsKey(ruleList[i].EmployeeTypeID))
                        item.SubItems.Add(companyTypesDict[ruleList[i].WorkingUnitID][ruleList[i].EmployeeTypeID].Trim());
                    else
                        item.SubItems.Add("N/A");
                    item.SubItems.Add(ruleList[i].RuleType.Trim());
                    if (ruleList[i].RuleValue != -1)
                        item.SubItems.Add(ruleList[i].RuleValue.ToString().Trim());
                    else
                        item.SubItems.Add("N/A");

                    if (!ruleList[i].RuleDateTime1.Equals(new DateTime()) && !ruleList[i].RuleDateTime1.Equals(Constants.dateTimeNullValue()))
                        item.SubItems.Add(ruleList[i].RuleDateTime1.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                    else
                        item.SubItems.Add("N/A");
                    if (!ruleList[i].RuleDateTime2.Equals(new DateTime()) && !ruleList[i].RuleDateTime2.Equals(Constants.dateTimeNullValue()))
                        item.SubItems.Add(ruleList[i].RuleDateTime2.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                    else
                        item.SubItems.Add("N/A");
                    item.Tag = ruleList[i];
                    lvRules.Items.Add(item);
                }

                lvRules.EndUpdate();
                lvRules.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rules.populateRules(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateCompanies()
        {
            try
            {
                List<WorkingUnitTO> companyList = new WorkingUnit().getRootWorkingUnitsList(wuString);

                WorkingUnitTO allWU = new WorkingUnitTO();
                allWU.Name = rm.GetString("all", culture);
                companyList.Insert(0, allWU);

                cbCompany.DataSource = companyList;
                cbCompany.ValueMember = "WorkingUnitID";
                cbCompany.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rules.populateCompanies(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateRuleTypes()
        {
            try
            {
                List<string> types = new Common.Rule().SearchRuleTypes();

                List<string> typeList = new List<string>();                
                typeList.Add(rm.GetString("all", culture));

                foreach (string type in types)
                {
                    if (type.Equals(Constants.RuleCutOffDate) || type.Equals(Constants.RuleHRSSCCutOffDate) || type.Equals(Constants.RuleWCDRCutOffDate))
                        continue;

                    typeList.Add(type);
                }

                cbType.DataSource = typeList;                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rules.populateRuleTypes(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                List<EmployeeTypeTO> typeList = new List<EmployeeTypeTO>();

                if (cbCompany.SelectedValue != null && cbCompany.SelectedValue is int)
                {
                    if ((int)cbCompany.SelectedValue != -1 && companyTypes.ContainsKey((int)cbCompany.SelectedValue))
                        typeList = companyTypes[(int)cbCompany.SelectedValue];
                }

                EmployeeTypeTO allType = new EmployeeTypeTO();
                allType.EmployeeTypeName = rm.GetString("all", culture);
                typeList.Insert(0, allType);

                cbEmplType.DataSource = typeList;
                cbEmplType.ValueMember = "EmployeeTypeID";
                cbEmplType.DisplayMember = "EmployeeTypeName";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rules.cbCompany_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvRules_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                tbRuleDesc.Text = "";

                if (lvRules.SelectedItems.Count > 0)
                    tbRuleDesc.Text = ((RuleTO)lvRules.SelectedItems[0].Tag).RuleDescription.Trim();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rules.lvRules_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvRules_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvRules.Sorting;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvRules.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvRules.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvRules.Sorting = SortOrder.Ascending;
                }

                lvRules.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rules.lvRules_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                Common.Rule rule = new Common.Rule();

                if (cbCompany.SelectedIndex > 0)
                    rule.RuleTO.WorkingUnitID = (int)cbCompany.SelectedValue;
                if (cbEmplType.SelectedIndex > 0)
                    rule.RuleTO.EmployeeTypeID = (int)cbEmplType.SelectedValue;
                if (cbType.SelectedIndex > 0)
                    rule.RuleTO.RuleType = cbType.Text;

                populateRules(rule.Search());
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rules.btnSearch_Click(): " + ex.Message + "\n");
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
                if (lvRules.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noSelectedRule", culture));
                    return;
                }

                RuleTO ruleTO = (RuleTO)lvRules.SelectedItems[0].Tag;                
                string company = "";
                string emplType = "";

                if (wuDict.ContainsKey(ruleTO.WorkingUnitID))
                    company = wuDict[ruleTO.WorkingUnitID].Name.Trim();
                if (companyTypesDict.ContainsKey(ruleTO.WorkingUnitID) && companyTypesDict[ruleTO.WorkingUnitID].ContainsKey(ruleTO.EmployeeTypeID))
                    emplType = companyTypesDict[ruleTO.WorkingUnitID][ruleTO.EmployeeTypeID].Trim();

                RulesAdd addForm = new RulesAdd(ruleTO, company, emplType);
                addForm.ShowDialog();

                btnSearch.PerformClick();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rules.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}
