using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using Util;
using Common;
using TransferObjects;
using System.Collections;
using UI;

namespace ACTASelftService
{
    public partial class Choice : Form
    {
        private CultureInfo culture;
        ResourceManager rm;
        DebugLog log;

        EmployeeTO empl;

        public Choice(EmployeeTO e)
        {
            InitializeComponent();

            this.CenterToScreen();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
            empl = e;
        }

        private void btbCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Choice_Load(object sender, EventArgs e)
        {
            try
            {
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(Employees).Assembly);

                setLanguage();

                textBox1.Text = empl.FirstName + " " + empl.LastName;

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SelfSerfMain.CheckInForm_Load(): " + ex.Message + "\n");
            }
        }

        private void setLanguage()
        {
            try
            {
                //set form text
                this.Text = rm.GetString("choice", culture);

                //buttens text
                btnEnterAbsence.Text = rm.GetString("btnEnterAbsence", culture);
                btnForgotenCard.Text = rm.GetString("btnForgotenCard", culture);
                btbClose.Text = rm.GetString("btnClose", culture);

                //label's text
                lblName.Text = rm.GetString("firstAndLastName", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Choice.setLanguage(): " + ex.Message + "\n");
            }
        }

        private void btnEnterAbsence_Click(object sender, EventArgs e)
        {
            try
            {
                EmployeeAbsencesAdd abs = new EmployeeAbsencesAdd(empl);
                abs.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Choice.btnEnterAbsence_Click(): " + ex.Message + "\n");
            }
        }

        private void btnForgotenCard_Click(object sender, EventArgs e)
        {
            try
            {
                PassesAdd pass = new PassesAdd(empl);
                pass.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Choice.btnForgotenCard_Click(): " + ex.Message + "\n");
            }
        }
    }
}