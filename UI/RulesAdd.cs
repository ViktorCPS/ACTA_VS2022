using System;
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
    public partial class RulesAdd : Form
    {
        ApplUserTO logInUser;

        private CultureInfo culture;
        ResourceManager rm;
        DebugLog log;

        RuleTO currentRule = new RuleTO();

        public RulesAdd(RuleTO ruleTO, string company, string emplType)
        {
            try
            {
                InitializeComponent();
                this.CenterToScreen();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                logInUser = NotificationController.GetLogInUser();

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(RulesAdd).Assembly);
                setLanguage();
                
                if (ruleTO.RuleDateTime1.Equals(new DateTime()))
                    ruleTO.RuleDateTime1 = Constants.dateTimeNullValue();
                if (ruleTO.RuleDateTime2.Equals(new DateTime()))
                    ruleTO.RuleDateTime2 = Constants.dateTimeNullValue();

                tbCompany.Text = company;
                tbEmplType.Text = emplType;
                tbType.Text = ruleTO.RuleType;
                tbDesc.Text = ruleTO.RuleDescription;
                numValue.Value = ruleTO.RuleValue;
                dtpDate1.Value = ruleTO.RuleDateTime1;
                dtpDate2.Value = ruleTO.RuleDateTime2;

                currentRule = ruleTO;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("RulesUpdate", culture);

                // button's text
                btnCancel.Text = rm.GetString("btnCancel", culture);                
                btnUpdate.Text = rm.GetString("btnUpdate", culture);

                // group box text
                gbRule.Text = rm.GetString("gbRule", culture);

                // label's text                
                lblEmplType.Text = rm.GetString("lblEmplType", culture);
                lblCompany.Text = rm.GetString("lblCompany", culture);
                lblType.Text = rm.GetString("lblType", culture);
                lblDesc.Text = rm.GetString("lblDesc", culture);
                lblValue.Text = rm.GetString("lblValue", culture);
                lblDate1.Text = rm.GetString("lblDate1", culture);
                lblDate2.Text = rm.GetString("lblDate2", culture);
                lblInfo.Text = rm.GetString("lblRuleInfo", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RulesAdd.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if ((int)numValue.Value == -1)
                {
                    MessageBox.Show(rm.GetString("undefinedValue", culture));
                    return;
                }

                currentRule.RuleValue = (int)numValue.Value;
                currentRule.RuleDateTime1 = dtpDate1.Value;
                currentRule.RuleDateTime2 = dtpDate2.Value;

                Common.Rule rule = new Common.Rule();
                rule.RuleTO = currentRule;

                if (rule.Update(true))
                {
                    MessageBox.Show(rm.GetString("ruleUpdated", culture));
                        this.Close();
                }
                else
                    MessageBox.Show(rm.GetString("ruleNotUpdated", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RulesAdd.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
    }
}
