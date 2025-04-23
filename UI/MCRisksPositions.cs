using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Util;
using TransferObjects;
using System.Resources;
using System.Globalization;
using Common;
using System.Collections;

namespace UI
{
    public partial class MCRisksPositions : UserControl
    {
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;
        private List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();
        private string wuString = "";
        private List<int> wuList = new List<int>();

        Dictionary<int, RiskTO> risks = new Dictionary<int, RiskTO>();
        Dictionary<int, EmployeePositionTO> positions = new Dictionary<int, EmployeePositionTO>();
        List<ListViewItem> originRiskXPosition;
        List<ListViewItem> addedRiskXPosition;
        List<ListViewItem> removedRiskXPosition;

        List<ApplRoleTO> currentRoles;
        Hashtable menuItemsPermissions;

        string menuItemID;
        string menuItemUsedID;

        bool addUsedPermission = false;
        bool updateUsedPermission = false;
        bool deleteUsedPermission = false;

        public MCRisksPositions()
        {
            try
            {
                InitializeComponent();
                originRiskXPosition = new List<ListViewItem>();
                addedRiskXPosition = new List<ListViewItem>();
                removedRiskXPosition = new List<ListViewItem>();
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

                setLanguage();
            }
            catch (Exception ex)
            {

                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " MCRisksPositions.MCRisksPositions(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

        }

        private void setLanguage()
        {

            try
            {
                lvRisks.BeginUpdate();
                lvRisks.Columns.Add(rm.GetString("hdrRiskCode", culture), (lvRisks.Width - 5) / 2 - 20, HorizontalAlignment.Left);
                lvRisks.Columns.Add(rm.GetString("hdrDesc", culture), (lvRisks.Width - 5) / 2 - 20, HorizontalAlignment.Left);
                lvRisks.EndUpdate();

                lvPostions.BeginUpdate();
                lvPostions.Columns.Add(rm.GetString("hdrPositionCode", culture), (lvRisks.Width - 5) / 2 - 20, HorizontalAlignment.Left);
                lvPostions.Columns.Add(rm.GetString("hdrDesc", culture), (lvRisks.Width - 5) / 2 - 20, HorizontalAlignment.Left);
                lvPostions.EndUpdate();

                lvRisksPositions.BeginUpdate();
                lvRisksPositions.Columns.Add(rm.GetString("hdrRisk", culture), (lvRisks.Width - 5) / 2 - 20, HorizontalAlignment.Left);
                lvRisksPositions.Columns.Add(rm.GetString("hdrPosition", culture), (lvRisks.Width - 5) / 2 - 20, HorizontalAlignment.Left);
                lvRisksPositions.EndUpdate();

                lblCompany.Text = rm.GetString("lblCompany", culture);
              
                btnClose.Text = rm.GetString("btnClose", culture);
                btnSave.Text = rm.GetString("btnSave", culture);

                populateCompany();

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " MCRisksPositions.setLanguage(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " MCRisksPositions.populateCompany(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                if (cbCompany.SelectedIndex > 0)
                {
                    Risk risk = new Risk();
                    risk.RiskTO.WorkingUnitID = (int)cbCompany.SelectedValue;
                    risks = risk.SearchRisksDictionary();
                    populateList(risks, 0);

                    EmployeePosition position = new EmployeePosition();

                    position.EmplPositionTO.WorkingUnitID = (int)cbCompany.SelectedValue;
                    positions = position.SearchEmployeePositionsDictionary();
                    populateListPositions(positions);

                    EmployeePositionXRisk riskXPosition = new EmployeePositionXRisk();
                    List<EmployeePositionXRiskTO> riskXPositionList = riskXPosition.SearchEmployeePositionXRisksByWU((int)cbCompany.SelectedValue);
                    populateListRiskPosition(riskXPositionList, 0);
                }
                else
                {
                    populateList(new Dictionary<int, RiskTO>(), 0);
                    populateListPositions(new Dictionary<int, EmployeePositionTO>());
                    populateListRiskPosition(new List<EmployeePositionXRiskTO>(), 0);

                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " MCRisksPositions.cbCompany_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        private void populateListRiskPosition(List<EmployeePositionXRiskTO> risksPositions, int startIndex)
        {
            try
            {

                lvRisksPositions.BeginUpdate();
                lvRisksPositions.Items.Clear();

                originRiskXPosition = new List<ListViewItem>();
                removedRiskXPosition = new List<ListViewItem>();
                addedRiskXPosition = new List<ListViewItem>();

                if (risks.Count > 0)
                {
                    for (int i = startIndex; i < risksPositions.Count; i++)
                    {
                        EmployeePositionXRiskTO risk = risksPositions[i];
                        ListViewItem item = new ListViewItem();


                        if (logInUser.LangCode.Equals(Constants.Lang_en))
                        {
                            if (risks.ContainsKey(risk.RiskID))
                                item.Text = risks[risk.RiskID].RiskCodeDescEN;
                            if (positions.ContainsKey(risk.PositionID))
                                item.SubItems.Add(positions[risk.PositionID].PositionCodeTitleEN);
                        }
                        else
                        {
                            if (risks.ContainsKey(risk.RiskID))
                                item.Text = risks[risk.RiskID].RiskCodeDescSR;
                            if (positions.ContainsKey(risk.PositionID))
                                item.SubItems.Add(positions[risk.PositionID].PositionCodeTitleSR);
                        }


                        item.Tag = risk;
                        lvRisksPositions.Items.Add(item);
                        originRiskXPosition.Add(item);
                    }

                }

                lvRisksPositions.EndUpdate();
                lvRisksPositions.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " MCRisksPositions.populateListRiskPosition(): " + ex.Message + "\n");
                throw ex;
            }
        }
        private void populateList(Dictionary<int, RiskTO> risks, int startIndex)
        {
            try
            {

                lvRisks.BeginUpdate();
                lvRisks.Items.Clear();

                if (risks.Count > 0)
                {
                    foreach (KeyValuePair<int, RiskTO> pair in risks)
                    {
                        RiskTO risk = pair.Value;
                        ListViewItem item = new ListViewItem();


                        item.Text = risk.RiskCode;
                        if (logInUser.LangCode.Equals(Constants.Lang_en))
                            item.SubItems.Add(risk.DescEN);
                        else item.SubItems.Add(risk.DescSR);

                        item.Tag = risk;
                        lvRisks.Items.Add(item);
                    }

                }

                lvRisks.EndUpdate();
                lvRisks.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " MCRisksPositions.populateList(): " + ex.Message + "\n");
                throw ex;

            }
        }
        private void populateListPositions(Dictionary<int, EmployeePositionTO> positions)
        {
            try
            {

                lvPostions.BeginUpdate();
                lvPostions.Items.Clear();

                if (positions.Count > 0)
                {
                    foreach (KeyValuePair<int, EmployeePositionTO> pair in positions)
                    {
                        EmployeePositionTO position = pair.Value;
                        ListViewItem item = new ListViewItem();


                        item.Text = position.PositionCode;
                        if (logInUser.LangCode.Equals(Constants.Lang_en))
                            item.SubItems.Add(position.PositionCodeTitleEN);
                        else item.SubItems.Add(position.PositionCodeTitleSR);

                        item.Tag = position;
                        lvPostions.Items.Add(item);
                    }

                }

                lvPostions.EndUpdate();
                lvPostions.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " MCRisksPositions.populateListPositions(): " + ex.Message + "\n");
                throw ex;

            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {

                lvRisksPositions.BeginUpdate();
                List<EmployeePositionXRiskTO> listRiskPos = new List<EmployeePositionXRiskTO>();
                foreach (int i in lvRisks.SelectedIndices)
                {

                    RiskTO riskTO = (RiskTO)lvRisks.Items[i].Tag;


                    foreach (int ind in lvPostions.SelectedIndices)
                    {
                        ListViewItem item = new ListViewItem();

                        EmployeePositionTO positionTO = (EmployeePositionTO)lvPostions.Items[ind].Tag;
                        EmployeePositionXRiskTO emplRiskPos = new EmployeePositionXRiskTO();
                        emplRiskPos.RiskID = riskTO.RiskID;
                        emplRiskPos.PositionID = positionTO.PositionID;
                        listRiskPos.Add(emplRiskPos);
                        if (logInUser.LangCode.Equals(Constants.Lang_en))
                        {
                            item.Text = riskTO.RiskCodeDescEN;
                            item.SubItems.Add(positionTO.PositionCodeTitleEN);
                        }
                        else
                        {
                            item.Text = riskTO.RiskCodeDescSR;
                            item.SubItems.Add(positionTO.PositionCodeTitleSR);
                        }

                        item.Tag = emplRiskPos;
                        lvRisksPositions.Items.Add(item);

                        if (contain(originRiskXPosition, item) < 0 && contain(addedRiskXPosition, item) < 0)
                        {
                            addedRiskXPosition.Add(item);
                            int index = contain(removedRiskXPosition, item);
                            if (index >= 0)
                                removedRiskXPosition.RemoveAt(index);

                        }
                    }
                }
                lvRisksPositions.EndUpdate();
                lvRisksPositions.Invalidate();
            }

            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " MCRisksPositions.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ListViewItem empl in lvRisksPositions.SelectedItems)
                {


                    if (contain(originRiskXPosition, empl) >= 0 && contain(removedRiskXPosition, empl) < 0)
                    {
                        removedRiskXPosition.Add(empl);
                    }

                    int index = contain(addedRiskXPosition, empl);
                    if (index >= 0)
                        addedRiskXPosition.RemoveAt(index);
                    lvRisksPositions.Items.Remove(empl);


                }
            }

            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " MCRisksPositions.btnRemove_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

        }

        private int contain(List<ListViewItem> list, ListViewItem item)
        {
            try
            {
                int index = -1;
                bool found = false;

                foreach (ListViewItem listItem in list)
                {
                    index++;

                    if (listItem.Text.Trim().Equals(item.Text))
                    {
                        if (listItem.SubItems[1].Text.Equals(item.SubItems[1].Text))
                        {
                            found = true;
                            break;
                        }
                    }
                }

                if (!found)
                    index = -1;

                return index;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " MCRisksPositions.contain(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                EmployeePositionXRisk riskPosition = new EmployeePositionXRisk();
                bool saved = false;
                foreach (ListViewItem removedRiskXPos in removedRiskXPosition)
                {
                    saved = riskPosition.Delete(((EmployeePositionXRiskTO)removedRiskXPos.Tag).PositionID, ((EmployeePositionXRiskTO)removedRiskXPos.Tag).RiskID, false) && saved;

                    int index = contain(originRiskXPosition, removedRiskXPos);
                    if (index >= 0)
                        originRiskXPosition.RemoveAt(index);
                }

                foreach (ListViewItem addedRiskXPos in addedRiskXPosition)
                {
                    EmployeePositionXRiskTO added = (EmployeePositionXRiskTO)addedRiskXPos.Tag;
                    riskPosition.EmplPositionXRiskTO = added;
                    saved = riskPosition.Save(false) > 0 ? true : false && saved;

                    int index = contain(originRiskXPosition, addedRiskXPos);
                    if (index < 0)
                        originRiskXPosition.Add(addedRiskXPos);
                }

                removedRiskXPosition.Clear();
                addedRiskXPosition.Clear();

                if (saved)
                {
                    MessageBox.Show(rm.GetString("riskXPosSaved", culture));
                }
                else
                {
                    MessageBox.Show(rm.GetString("riskXPosNotSaved", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " MCRisksPositions.btnSave_Click(): " + ex.Message + "\n");
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
                    //updateUsedPermission = updateUsedPermission || (((permissionUsed / 2) % 2) == 0 ? false : true);
                    //deleteUsedPermission = deleteUsedPermission || ((permissionUsed % 2) == 0 ? false : true);
                }

                btnSave.Enabled = btnPrev.Enabled = btnNext.Enabled = addUsedPermission;

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MCDisabilties.setVisibility(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void MCRisksPositions_Load(object sender, EventArgs e)
        {
            try
            {
                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();

                menuItemID = NotificationController.GetCurrentMenuItemID();
                int index = menuItemID.LastIndexOf('_');

                menuItemUsedID = menuItemID + "_" + rm.GetString("tpRiskPostion", culture);
                setVisibility();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " MCRisksPositions.MCRisksPositions_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
    }
}
