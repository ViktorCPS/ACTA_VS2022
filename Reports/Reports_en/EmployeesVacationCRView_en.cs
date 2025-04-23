using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Util;
using System.Globalization;
using System.Resources;
using Common;

namespace Reports.Reports_en {
    public partial class EmployeesVacationCRView_en : Form {

        DebugLog debug;
        CultureInfo culture;
        ResourceManager rm;

        public EmployeesVacationCRView_en(DataSet Data, DateTime date) {
            InitializeComponent();

            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            debug = new DebugLog(logFilePath);

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(EmployeesReports).Assembly);
            this.Text = rm.GetString("ReportsForVacation", culture);

            Reports_en.EmployeesVacationCR_en ecr = new Reports_en.EmployeesVacationCR_en();
            ecr.DataDefinition.FormulaFields["date"].Text = "\"" + date.ToString("dd.MM.yyyy") + "\"";

            ecr.SetDataSource(Data);
            this.crystalReportViewer1.ReportSource = ecr;
        }
    }
}
