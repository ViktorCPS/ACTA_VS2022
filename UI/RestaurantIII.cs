using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Util;
using Common;
using System.Resources;
using System.Globalization;
using TransferObjects;
using System.Collections;

namespace UI
{
    public partial class RestaurantIII : Form
    {
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;

        List<ApplRoleTO> currentRoles;
        Hashtable menuItemsPermissions;

        string menuItemID;
        string menuItemMealTypeID;
        string menuItemRestaurantID;
        string menuItemLinesID;
        string menuItemUsedID;
        string menuItemReportID;

        bool readMealTypePermission = false;
        bool readRestaurantPermission = false;
        bool readLinesPermission = false;
        bool readUsedPermission = false;
        bool readReportPermission = false;

        public RestaurantIII()
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

                this.tpMealPoints.Text = rm.GetString("tpLines", culture);
                this.tpMealType.Text = rm.GetString("tpMealTypes", culture);
                this.tpMealRestaurant.Text = rm.GetString("tpRestaurants", culture);
                this.tpMealsUsed.Text = rm.GetString("tpMealsUsed", culture);
                this.tpReports.Text = rm.GetString("tpMealUsedReport", culture);

                this.btnClose.Text = rm.GetString("btnClose", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RestaurantIII.setLanguage((): " + ex.Message + "\n");
                throw ex;
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
                log.writeLog(DateTime.Now + " RestaurantIII.btnClose_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " RestaurantIII.Restauran_KeyUp(): " + ex.Message + "\n");
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
                int permissionMealType;
                int permissionRestaurants;
                int permissionLines;
                int permissionUsed;
                int permissionReport;

                foreach (ApplRoleTO role in currentRoles)
                {                   
                    permissionMealType = (((int[])menuItemsPermissions[menuItemMealTypeID])[role.ApplRoleID]);
                    permissionRestaurants = (((int[])menuItemsPermissions[menuItemRestaurantID])[role.ApplRoleID]);
                    permissionUsed = (((int[])menuItemsPermissions[menuItemUsedID])[role.ApplRoleID]);
                    permissionLines = (((int[])menuItemsPermissions[menuItemLinesID])[role.ApplRoleID]);
                    permissionReport = (((int[])menuItemsPermissions[menuItemReportID])[role.ApplRoleID]);

                    readLinesPermission = readLinesPermission || (((permissionLines / 8) % 2) == 0 ? false : true);
                    readMealTypePermission = readMealTypePermission || (((permissionMealType / 8) % 2) == 0 ? false : true);
                    readRestaurantPermission = readRestaurantPermission || (((permissionRestaurants / 8) % 2) == 0 ? false : true);
                    readUsedPermission = readUsedPermission || (((permissionUsed / 8) % 2) == 0 ? false : true);
                    readReportPermission = readReportPermission || (((permissionReport / 8) % 2) == 0 ? false : true);
                }

                if (!readMealTypePermission)
                    tabControl.TabPages.Remove(tpMealType);
                if (!readLinesPermission)
                    tabControl.TabPages.Remove(tpMealPoints);
                if (!readRestaurantPermission)
                    tabControl.TabPages.Remove(tpMealRestaurant);
                if (!readUsedPermission)
                    tabControl.TabPages.Remove(tpMealsUsed);
                if (!readReportPermission)
                    tabControl.TabPages.Remove(tpReports);               
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RestaurantIII.setVisibility(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void Restaurant_Load(object sender, EventArgs e)
        {
            try
            {
                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();

                menuItemID = NotificationController.GetCurrentMenuItemID();
                int index = menuItemID.LastIndexOf('_');

                menuItemMealTypeID = menuItemID + "_" + rm.GetString("tpMealTypes", culture);
                menuItemRestaurantID = menuItemID + "_" + rm.GetString("tpRestaurants", culture);
                menuItemLinesID = menuItemID + "_" + rm.GetString("tpLines", culture);
                menuItemUsedID = menuItemID + "_" + rm.GetString("tpMealsUsed", culture);
                menuItemReportID = menuItemID + "_" + rm.GetString("tpMealUsedReport", culture);

                setVisibility();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RestaurantIII.Restaurant_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }     
    }
}
