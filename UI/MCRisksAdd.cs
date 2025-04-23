using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TransferObjects;
using Common;
using Util;
using System.Resources;
using System.Globalization;

namespace UI
{
    public partial class MCRisksAdd : Form
    {
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;
        RiskTO currentrisk = new RiskTO();
        
        public MCRisksAdd()
        {
            try
            {
                InitializeComponent();
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);
                logInUser = NotificationController.GetLogInUser();
                rm = new ResourceManager("UI.Resource", typeof(MCRisksAdd).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

                populateCompany();
                populateRotation();
            }
            catch (Exception ex)
            {
                
                log.writeLog(DateTime.Now + " MCRisksAdd.MCRisksAdd(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        string wuString = "";
        public MCRisksAdd(string wuString)
        {
            try
            {
                InitializeComponent();
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);
                logInUser = NotificationController.GetLogInUser();
                rm = new ResourceManager("UI.Resource", typeof(MCRisksAdd).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                this.wuString = wuString;
                setLanguage();
                btnInsert.Visible = true;
                btnUpdate.Visible = false;
                this.Text = rm.GetString("riskAdd", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MCRisksAdd.MCRisksAdd(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

        }
        public MCRisksAdd(string wuString, RiskTO risk)
        {
            try
            {
                InitializeComponent();
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);
                logInUser = NotificationController.GetLogInUser();
                rm = new ResourceManager("UI.Resource", typeof(MCRisksAdd).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                this.wuString = wuString;
                setLanguage();
                currentrisk = risk;
                txtCode.Text = risk.RiskCode;
                txtDescEn.Text = risk.DescEN;
                txtDescSr.Text = risk.DescSR;
                cbCompany.SelectedValue = risk.WorkingUnitID;
                cbDefRot.SelectedItem= risk.DefaultRotation.ToString();
                btnInsert.Visible = false;
                btnUpdate.Visible = true;
                this.Text = rm.GetString("riskUpdate", culture);
                cbCompany.Enabled = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MCRisksAdd.MCRisksAdd(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

        }
        private void setLanguage()
        {
           try
            {
                
                lblCompany.Text = rm.GetString("lblCompany", culture);
                lblDefaultRot.Text = rm.GetString("lblDefRotation", culture);
                lblDescEn.Text = rm.GetString("lblDescEn", culture);
                lblDescSr.Text = rm.GetString("lblDescSr", culture);
                lblCode.Text = rm.GetString("lblRiskCode", culture);
                btnInsert.Text = rm.GetString("btnSave", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnClose.Text = rm.GetString("btnCancel", culture);

                // groupbox text
                this.gbInserRisk.Text = rm.GetString("", culture);
                populateCompany();
                populateRotation();
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " MCRisksAdd.populateCompany(): " + ex.Message + "\n");
                throw ex;
            }
        }
        private void populateRotation()
        {
            try
            {

                List<string> rotation = new List<string>();
                rotation.Add("*");
                for (int i = 0; i < 25; i++)
                {

                    rotation.Add(i.ToString());
                }
                cbDefRot.DataSource = rotation;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MCRisksAdd.populateRotation(): " + ex.Message + "\n");
                 throw ex;
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                RiskTO riskTo = new RiskTO();

                if (cbCompany.SelectedIndex > 0)
                    riskTo.WorkingUnitID = (int)cbCompany.SelectedValue;
                else MessageBox.Show(rm.GetString("mustSelectCompany", culture));

                if (cbDefRot.SelectedIndex > 0)
                    riskTo.DefaultRotation = int.Parse((string)cbDefRot.SelectedValue);
                else MessageBox.Show(rm.GetString("mustSelectRotation", culture));

                riskTo.DescEN = txtDescEn.Text;
                riskTo.DescSR = txtDescSr.Text;
                riskTo.RiskCode = txtCode.Text;

                Risk risk = new Risk();
                risk.RiskTO = riskTo;
                risk.Save(false);
                MessageBox.Show(rm.GetString("succInsertRisk", culture));

            }
            catch (Exception ex)
            {
                MessageBox.Show(rm.GetString("errorInsertRisk", culture) + ": " + ex.Message);
                log.writeLog(DateTime.Now + " MCRisksAdd.btnInsert_Click(): " + ex.Message + "\n");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                 
                if (cbCompany.SelectedIndex > 0)
                    currentrisk.WorkingUnitID = (int)cbCompany.SelectedValue;
                else MessageBox.Show(rm.GetString("mustSelectCompany", culture));

                if (cbDefRot.SelectedIndex > 0)
                    currentrisk.DefaultRotation = int.Parse((string)cbDefRot.SelectedValue);
                else MessageBox.Show(rm.GetString("mustSelectRotation", culture));

                currentrisk.DescEN = txtDescEn.Text;
                currentrisk.DescSR = txtDescSr.Text;
                currentrisk.RiskCode = txtCode.Text;
              
                Risk risk = new Risk();
                risk.RiskTO = currentrisk;
              bool upd=  risk.Update(false);
              if (upd)
                  MessageBox.Show(rm.GetString("succUpdateRisk", culture));
            }
            catch (Exception ex)
            {

                MessageBox.Show(rm.GetString("errorUpdateRisk", culture) + ": " + ex.Message);
                log.writeLog(DateTime.Now + " MCRisksAdd.btnUpdate_Click(): " + ex.Message + "\n");
            }
        }
    }
}
