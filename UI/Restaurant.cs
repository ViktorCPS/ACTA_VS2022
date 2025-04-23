using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;

using System.Data;
using TransferObjects;
using Common;
using Util;
using Reports;

using System.Resources;
using System.Globalization;

namespace UI
{
    public partial class Restaurant : Form
    {
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;

        public Restaurant()
        {
            InitializeComponent();
            this.CenterToScreen();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            logInUser = NotificationController.GetLogInUser();

            rm = new ResourceManager("UI.Resource", typeof(MealsAssigned).Assembly);
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            setLanguage();
        }
        private void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("restaurant", culture);

                this.tpMealPoints.Text = rm.GetString("tpMealPoints", culture);
                this.tpMealsAssigned.Text = rm.GetString("MealsAssignedTitle", culture);
                this.tpMealType.Text = rm.GetString("tpMealTypes", culture);
                this.tpMealTypeEmpl.Text = rm.GetString("tpMealTypeEmpl", culture);
                this.tpMealsUsed.Text = rm.GetString("MealsUsedForm", culture);

                this.btnClose.Text = rm.GetString("btnClose", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Restaurant.setLanguage((): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Restaurant.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void Restaurant_KeyUp(object sender, KeyEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (e.KeyCode.Equals(Keys.F1))
                {
                    Util.Misc.helpManualHtml(this.Name);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Restauran.Restauran_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void Restaurant_Load(object sender, EventArgs e)
        {

        }
    }
}