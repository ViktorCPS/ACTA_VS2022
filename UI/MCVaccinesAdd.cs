using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Util;
using TransferObjects;
using Common;
using System.Resources;
using System.Globalization;

namespace UI
{
    public partial class MCVaccinesAdd : Form
    {
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;
        VaccineTO currentVaccine = new VaccineTO();

        public MCVaccinesAdd()
        {
            InitializeComponent();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
            logInUser = NotificationController.GetLogInUser();
            rm = new ResourceManager("UI.Resource", typeof(MCRisksAdd).Assembly);
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            btnInsert.Visible = true;
            btnUpdate.Visible = false;

        }
        public MCVaccinesAdd(VaccineTO vaccine)
        {
            InitializeComponent();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
            logInUser = NotificationController.GetLogInUser();
            rm = new ResourceManager("UI.Resource", typeof(MCRisksAdd).Assembly);
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

            currentVaccine = vaccine;
            txtDescEn.Text = vaccine.DescEN;
            txtDescSr.Text = vaccine.DescSR;
            txtType.Text = vaccine.VaccineType;
            btnInsert.Visible = false;
            btnUpdate.Visible = true;
        }
        private void setLanguage()
        {
            try
            {

                lblDescEn.Text = rm.GetString("lblDescEn", culture);
                lblDescSr.Text = rm.GetString("lblDescSr", culture);
                lblType.Text = rm.GetString("lblVaccineType", culture);

                btnInsert.Text = rm.GetString("btnSave", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnClose.Text = rm.GetString("btnCancel", culture);

                if (currentVaccine.VaccineID != -1)
                    this.Text = rm.GetString("vacUpd", culture);
                else
                    this.Text = rm.GetString("vacAdd", culture);
                // groupbox text
                this.gbInserRisk.Text = rm.GetString("", culture);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void MCVaccinesAdd_Load(object sender, EventArgs e)
        {
            try
            {
                setLanguage();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MCVaccinesAdd.MCVaccinesAdd_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                VaccineTO vaccineTO = new VaccineTO();
                vaccineTO.VaccineType = txtType.Text.Trim();
                vaccineTO.DescEN = txtDescEn.Text.Trim();
                vaccineTO.DescSR = txtDescSr.Text.Trim();
                Vaccine vaccine = new Vaccine();
                vaccine.VaccineTO = vaccineTO;
                int saved = vaccine.Save(false);
                if (saved > 0) MessageBox.Show(rm.GetString("vacSuccInsert", culture));
                else MessageBox.Show(rm.GetString("vacNotSuccInsert", culture));
            }
            catch (Exception ex)
            {

                log.writeLog(DateTime.Now + " MCVaccinesAdd.btnInsert_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                
                currentVaccine.VaccineType = txtType.Text.Trim();
                currentVaccine.DescEN = txtDescEn.Text.Trim();
                currentVaccine.DescSR = txtDescSr.Text.Trim();
                Vaccine vaccine = new Vaccine();
                vaccine.VaccineTO = currentVaccine;
                bool updated = vaccine.Update(false);
                if (updated) MessageBox.Show(rm.GetString("vacSuccUpdate", culture));
                else MessageBox.Show(rm.GetString("vacNotSuccUpdate", culture));
            }
            catch (Exception ex)
            {

                log.writeLog(DateTime.Now + " MCVaccinesAdd.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
    }
}
