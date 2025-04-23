using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Util;
using System.Resources;
using TransferObjects;
using System.Globalization;
using Common;

namespace UI
{
    public partial class MCDisabilitiesAdd : Form
    {
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;
        MedicalCheckDisabilityTO currentDis = new MedicalCheckDisabilityTO();
        string wuString = "";

        public MCDisabilitiesAdd()
        {
            InitializeComponent();
        }

        public MCDisabilitiesAdd(string wuString)
        {
            try
            {
                InitializeComponent();
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);
                logInUser = NotificationController.GetLogInUser();
                rm = new ResourceManager("UI.Resource", typeof(MCDisabilitiesAdd).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                this.wuString = wuString;
                setLanguage();
                btnInsert.Visible = true;
                btnUpdate.Visible = false;
                this.Text = rm.GetString("disabilityAdd", culture);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                log.writeLog("Error in MCRisksAdd.MCRisksAdd: " + ex.Message);
            }

        }
        public MCDisabilitiesAdd(string wuString, MedicalCheckDisabilityTO risk)
        {
            try
            {
                InitializeComponent();
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);
                logInUser = NotificationController.GetLogInUser();
                rm = new ResourceManager("UI.Resource", typeof(MCDisabilitiesAdd).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                this.wuString = wuString;
                setLanguage();
                currentDis = risk;
                txtCode.Text = risk.DisabilityCode;
                txtDescEn.Text = risk.DescEN;
                txtDescSr.Text = risk.DescSR;
                cbCompany.SelectedValue = risk.WorkingUnitID;
                btnInsert.Visible = false;
                btnUpdate.Visible = true;
                this.Text = rm.GetString("disabilityUpdate", culture);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.writeLog("Error in MCRisksAdd.MCRisksAdd: " + ex.Message);
            }

        }

        private void setLanguage()
        {
            try
            {
              
                lblCompany.Text = rm.GetString("lblCompany", culture);
                lblDescEn.Text = rm.GetString("lblDescEn", culture);
                lblDescSr.Text = rm.GetString("lblDescSr", culture);
                lblCode.Text = rm.GetString("lblDisabilityCode", culture);
                lblStatus.Text = rm.GetString("lblStatus", culture);

                btnInsert.Text = rm.GetString("btnSave", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnClose.Text = rm.GetString("btnCancel", culture);

                // groupbox text
                this.gbInserRisk.Text = rm.GetString("", culture);

                populateCompany();
                populateStatus();

            }
            catch (Exception ex)
            {

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
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " MCDisabilitiesAdd.populateCompany(): " + ex.Message + "\n");
                throw ex;
            }
        }
        private void populateStatus()
        {

            List<string> status = new List<string>();
            status.Add(Constants.statusActive);
            status.Add(Constants.statusRetired);
            cbStatus.DataSource = status;

        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                MedicalCheckDisabilityTO disTo = new MedicalCheckDisabilityTO();

                if (cbCompany.SelectedIndex > 0)
                    disTo.WorkingUnitID = (int)cbCompany.SelectedValue;
                else { MessageBox.Show(rm.GetString("mustSelectCompany", culture)); return; }
                if (cbStatus.SelectedIndex >= 0)
                    disTo.Status = (string)cbStatus.SelectedValue;
                else { MessageBox.Show(rm.GetString("mustSelectCompany", culture)); return; }
                disTo.DescEN = txtDescEn.Text;
                disTo.DescSR = txtDescSr.Text;
                disTo.DisabilityCode = txtCode.Text;

                MedicalCheckDisability risk = new MedicalCheckDisability();
                risk.DisabilityTO = disTo;
                risk.Save(false);
                MessageBox.Show(rm.GetString("disSuccInsert", culture));

            }
            catch (Exception ex)
            {
                MessageBox.Show(rm.GetString("disNotSuccInsert", culture) + ": " + ex.Message);
                log.writeLog("Error in MCDisabilitiesAdd.btnInsert_Click: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {

                if (cbCompany.SelectedIndex > 0)
                    currentDis.WorkingUnitID = (int)cbCompany.SelectedValue;
                else { MessageBox.Show(rm.GetString("mustSelectCompany", culture)); return; }
                if (cbStatus.SelectedIndex >= 0)
                    currentDis.Status = (string)cbStatus.SelectedValue;
                else { MessageBox.Show(rm.GetString("mustSelectCompany", culture)); return; }

                currentDis.DescEN = txtDescEn.Text;
                currentDis.DescSR = txtDescSr.Text;
                currentDis.DisabilityCode = txtCode.Text;

                MedicalCheckDisability disability = new MedicalCheckDisability();
                disability.DisabilityTO = currentDis;
                bool upd = disability.Update(false);
                if (upd)
                    MessageBox.Show(rm.GetString("disSuccUpdate", culture));
            }
            catch (Exception ex)
            {

                MessageBox.Show(rm.GetString("disNotSuccUpdate", culture) + ": " + ex.Message);
                log.writeLog("Error in MCDisabilitiesAdd.btnUpdate_Click: " + ex.Message);
            }
        }
    }
}
