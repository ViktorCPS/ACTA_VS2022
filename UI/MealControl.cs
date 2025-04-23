using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Util;
using System.Resources;
using Common;
using System.Globalization;

namespace UI
{
    public partial class MealControl : UserControl
    {
        DebugLog log;      
        private bool enable = true;
        MealType mealType = new MealType();

        public MealType MealType
        {
            get { return mealType; }
            set { mealType = value; }
        }

        public bool Enable
        {
            get { return enable; }
            set 
            { 
                enable = value;
                btnMeal.Enabled = enable;
            }
        }

        private Color controlColor = Color.White;
        public int orderNumber = 0;

        public Color ControlColor
        {
            get { return controlColor; }
            set 
            {
                controlColor = value;
                btnMeal.BackColor = value;
            }
        }

        private MealsWorkingUnitSchedules parentControl;

        public MealControl()
        {
            InitializeComponent();
            // Init Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

        }
        public MealControl(MealType type, MealsWorkingUnitSchedules control, int ordNum)
        {
            InitializeComponent();
            mealType = type;
            parentControl = control;
            btnMeal.Text = mealType.Name;
            orderNumber = ordNum;
            // Init Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

        }

        private void btnMeal_Click(object sender, EventArgs e)
        {
            try
            {
                parentControl.mealsSelectionChange(mealType, orderNumber);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealControl.btnMeal_Click(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}
