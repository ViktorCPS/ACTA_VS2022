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
    public partial class RulesCutOffDate : Form
    {
        ApplUserTO logInUser;

        private CultureInfo culture;
        ResourceManager rm;
        DebugLog log;
        
        private List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();
        List<int> wuList = new List<int>();
        private string wuString = "";
        Dictionary<int, WorkingUnitTO> wuDict = new Dictionary<int, WorkingUnitTO>();
                
        List<ApplRoleTO> currentRoles;
        Hashtable menuItemsPermissions;
        string menuItemID;
        bool addPermission = false;
        bool updatePermission = false;
        bool deletePermission = false;

        public RulesCutOffDate()
        {
            try
            {
                InitializeComponent();
                this.CenterToScreen();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                logInUser = NotificationController.GetLogInUser();

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(RulesCutOffDate).Assembly);
                setLanguage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RulesCutOffDate_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                                
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
                                
                populateCompanies();
                populateRuleTypes();

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                menuItemID = NotificationController.GetCurrentMenuItemID();

                setVisibility();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RulesCutOffDate.RulesCutOffDate_Load(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " RulesCutOffDate.setVisibility(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("RulesCutOffDate", culture);

                // button's text
                btnClose.Text = rm.GetString("btnClose", culture);                
                btnUpdate.Text = rm.GetString("btnUpdateValue", culture);

                // label's text
                lblCompany.Text = rm.GetString("lblCompany", culture);
                lblType.Text = rm.GetString("lblType", culture);
                lblValue.Text = rm.GetString("lblValue", culture);
                lblInfo.Text = rm.GetString("undefinedCompanyRules", culture);

                // group boxes
                gbSearchFilter.Text = rm.GetString("gbSearchFilter", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RulesCutOffDate.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateCompanies()
        {
            try
            {
                List<WorkingUnitTO> companyList = new WorkingUnit().getRootWorkingUnitsList(wuString);
                
                cbCompany.DataSource = companyList;
                cbCompany.ValueMember = "WorkingUnitID";
                cbCompany.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RulesCutOffDate.populateCompanies(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateRuleTypes()
        {
            try
            {
                List<string> typeList = new List<string>();
                typeList.Add(Constants.RuleCutOffDate);
                typeList.Add(Constants.RuleHRSSCCutOffDate);
                typeList.Add(Constants.RuleWCDRCutOffDate);
                             
                cbType.DataSource = typeList;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RulesCutOffDate.populateRuleTypes(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                populateValue();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RulesCutOffDate.cbCompany_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                populateValue();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RulesCutOffDate.cbType_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateValue()
        {
            try
            {
                if (cbCompany.SelectedValue != null && cbCompany.SelectedValue is int)
                {
                    Common.Rule rule = new Common.Rule();
                    rule.RuleTO.WorkingUnitID = (int)cbCompany.SelectedValue;
                    rule.RuleTO.RuleType = cbType.Text;

                    List<RuleTO> ruleList = rule.Search();

                    if (ruleList.Count > 0)
                    {
                        numValue.Value = ruleList[0].RuleValue;
                        btnUpdate.Enabled = true;
                        lblInfo.Visible = false;
                    }
                    else
                    {
                        numValue.Value = 0;
                        btnUpdate.Enabled = false;
                        lblInfo.Visible = true;
                    }
                }
                else
                {
                    numValue.Value = 0;
                    btnUpdate.Enabled = false;
                    lblInfo.Text = rm.GetString("undefinedCompanyRules", culture);
                    lblInfo.Visible = true;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RulesCutOffDate.populateValue(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int company = -1;
                string type = "";
                
                if (cbCompany.SelectedValue != null && cbCompany.SelectedValue is int)
                    company = (int)cbCompany.SelectedValue;

                type = cbType.Text;

                if (company == -1 || type.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("notValidCompanyType", culture));
                    return;
                }

                if ((int)numValue.Value == -1)
                {
                    MessageBox.Show(rm.GetString("undefinedValue", culture));
                    return;
                }
                
                int value = (int)numValue.Value;

                if (new Common.Rule().Update(company, type, value, true))                
                    MessageBox.Show(rm.GetString("ruleUpdated", culture));                
                else
                    MessageBox.Show(rm.GetString("ruleNotUpdated", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RulesCutOffDate.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
    }
}
