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
using System.Collections;

namespace UI
{
    public partial class MedicalCheck : Form
    {
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;

        List<ApplRoleTO> currentRoles;
        Hashtable menuItemsPermissions;
        string menuItemID;
        string menuItemRiskID;
        string menuItemRepID;
        string menuItemVaccineID;
        string menuItemDisabilityID;
        string menuItemRiskPositionID;

        bool readRiskPermission = false;
        bool readVaccinePermission = false;  
        bool readDisabilityPermission = false;
        bool readRiskPosPermission = false;

        public MedicalCheck()
        {
            try
            {
                InitializeComponent();
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);
                logInUser = NotificationController.GetLogInUser();
                rm = new ResourceManager("UI.Resource", typeof(MCRisks).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                setLanguage();
                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();

                menuItemID = NotificationController.GetCurrentMenuItemID();
                int index = menuItemID.LastIndexOf('_');

                menuItemRepID = rm.GetString("menuLibraries", culture) + "_"
                    + rm.GetString("medCheck", culture) + "_";

                menuItemRiskID = menuItemRepID + rm.GetString("tpRisk", culture);
                menuItemVaccineID = menuItemRepID + rm.GetString("tpVaccines", culture);
                menuItemDisabilityID = menuItemRepID + rm.GetString("tpDisabilities", culture);
                menuItemRiskPositionID = menuItemRepID + rm.GetString("tpRiskPostion", culture);
                setVisibility();
            }
            catch (Exception ex)
            {
                
                 log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " MedicalCheck.setLanguage(): " + ex.Message + "\n");
                 MessageBox.Show(ex.Message);
            }
        }

        private void setLanguage() {
            try
            {
                this.Text = rm.GetString("medCheck", culture);
                tpDisabilities.Text = rm.GetString("tpDisabilities", culture);
                tpRisk.Text = rm.GetString("tpRisk", culture);
                tpRiskPostion.Text = rm.GetString("tpRiskPostion", culture);
                tpVaccines.Text = rm.GetString("tpVaccines", culture);
                btnClose.Text = rm.GetString("btnClose", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " MedicalCheck.setLanguage(): " + ex.Message + "\n"); 
                throw ex;
            }
        }

        private void setVisibility()
        {
            try
            {
                int permission;
                int permissionRisk;
                int permissionRP;

                int permissionVaccine;
                int permissionDisability;
              
                foreach (ApplRoleTO role in currentRoles)
                {
                    permission = (((int[])menuItemsPermissions[menuItemID])[role.ApplRoleID]);
                    permissionRisk = (((int[])menuItemsPermissions[menuItemRiskID])[role.ApplRoleID]);
                    permissionRP = (((int[])menuItemsPermissions[menuItemRiskPositionID])[role.ApplRoleID]);
                    permissionVaccine = (((int[])menuItemsPermissions[menuItemVaccineID])[role.ApplRoleID]);
                    permissionDisability = (((int[])menuItemsPermissions[menuItemDisabilityID])[role.ApplRoleID]);


                    readRiskPosPermission = readRiskPosPermission || (((permissionRP / 8) % 2) == 0 ? false : true);
                       readRiskPermission = readRiskPermission || (((permissionRisk / 8) % 2) == 0 ? false : true);
                   
                    readVaccinePermission = readVaccinePermission || (((permissionVaccine / 8) % 2) == 0 ? false : true);
               
                    readDisabilityPermission = readDisabilityPermission || (((permissionDisability / 8) % 2) == 0 ? false : true);
                
                }

                if (!readRiskPermission)
                    tcMC.TabPages.Remove(tpRisk);
                if (!readVaccinePermission)
                    tcMC.TabPages.Remove(tpVaccines);
                if (!readDisabilityPermission)
                    tcMC.TabPages.Remove(tpDisabilities);
                if (!readRiskPosPermission)
                    tcMC.TabPages.Remove(tpRiskPostion);
               
                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Securityroutes.setVisibility(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
