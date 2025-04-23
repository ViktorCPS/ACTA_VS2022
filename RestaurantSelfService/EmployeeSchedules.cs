using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Common;
using System.Globalization;
using System.Resources;
using Util;
using TransferObjects;

namespace RestaurantSelfService
{
    public partial class EmployeeSchedules : Form
    {
        private CultureInfo culture;
        ResourceManager rm;
        DebugLog log;

        public EmployeeSchedules()
        {
            InitializeComponent();
            // Init Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(UI.Employees).Assembly);

        }
        public EmployeeSchedules(EmployeeTO empl)
        {
            InitializeComponent();
            // Init Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            mealsWorkingUnitSchedules1.CurrentEmployee = empl;

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(UI.Employees).Assembly);
            this.Text = rm.GetString("selfServMeal", culture);
            this.btnClose.Text = rm.GetString("btnClose", culture);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}