using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Common;
using Util;
using TransferObjects;
using ReaderInterface;

using System.Resources;
using System.Globalization;

namespace UI
{
    public partial class ExitPermHeaderControl : UserControl
    {
        CultureInfo culture;
        ResourceManager rm;
        DebugLog log;
        public string manualyCreatedExist;

        public ExitPermHeaderControl(DateTime day, List<WorkTimeIntervalTO> intervals)
        {
            InitializeComponent();
            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(ExitPermControl).Assembly);
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
            setLanguage();
            lblDay.Text = rm.GetString("lblDay", culture);
            this.tbDay.Text = day.ToString("dd.MM.yyyy");
            foreach (WorkTimeIntervalTO interval in intervals)
            {
                this.cbSchedule.Items.Add(interval.StartTime.ToString("HH:mm") + "-" + interval.EndTime.ToString("HH:mm"));
            }
            this.cbSchedule.SelectedIndex = 0;
            manualyCreatedExist = "";
        }

        public ExitPermHeaderControl(EmployeeTO employee, List<WorkTimeIntervalTO> intervals)
        {
            InitializeComponent();
            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(ExitPermControl).Assembly);
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
            setLanguage();
            lblDay.Text = rm.GetString("lblEmployee", culture);
            this.tbDay.Text = employee.FirstName + " " + employee.LastName;
            foreach (WorkTimeIntervalTO interval in intervals)
            {
                this.cbSchedule.Items.Add(interval.StartTime.ToString("HH:mm") + "-" + interval.EndTime.ToString("HH:mm"));
            }
            this.cbSchedule.SelectedIndex = 0;
            manualyCreatedExist = "";
        }
        public void setLanguage()
        {
            try
            {
                // label's text
                lblDescription.Text = rm.GetString("lblDescription", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblSchedule.Text = rm.GetString("lblSchedule", culture);
                lblPassType.Text = rm.GetString("lblPassType", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " exitPermHeaderControl.setLanguage(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void ExitPermHeaderControl_Load(object sender, EventArgs e)
        {
            try
            {
                lblManualPairs.Text = this.manualyCreatedExist; 
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " exitPermHeaderControl.setLanguage(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

    }
}
