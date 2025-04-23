using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;

using Common;
using Util;
using TransferObjects;


namespace UI
{
    public partial class RestaurantII : Form
    {
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;

        public RestaurantII()
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RestaurantII.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("restaurant", culture);
                // button's text
                btnClose.Text = rm.GetString("btnClose", culture);

                this.tpMealType.Text = rm.GetString("tpMealTypes", culture);
                this.tpSetingSchedule.Text = rm.GetString("tpSetingSchedule", culture);
                this.tpMealsSchedule.Text = rm.GetString("tpMealsSchedule", culture);
                this.tpReports.Text = rm.GetString("tpReports", culture);
                this.tpMealPoints.Text = rm.GetString("tpMealPointsII", culture);
                this.tpOrder.Text = rm.GetString("tpOrder", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypesII.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void tpMealType_Click(object sender, EventArgs e)
        {

        }

        private void RestaurantII_Load(object sender, EventArgs e)
        {

        }

        
    }
}