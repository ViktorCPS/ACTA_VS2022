using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;
using Util;
using Common;

namespace Reports.Niksic.Niksic_sr
{
    public partial class MonthlyReportCRView : Form
    {
        DebugLog debug;
        CultureInfo culture;
        ResourceManager rm;

        public MonthlyReportCRView(DataSet data, string wuName, string emplName, int emplID, DateTime month)
        {
            InitializeComponent();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            debug = new DebugLog(logFilePath);

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(MonthlyReportCRView).Assembly);
            this.Text = rm.GetString("monthlyReport", culture);

            int lastMonthDay = new DateTime(month.AddMonths(1).Year, month.AddMonths(1).Month, 1).AddDays(-1).Day;
            
            MonthlyReportCR_sr cr = new MonthlyReportCR_sr();

            cr.DataDefinition.FormulaFields["selWUName"].Text = "\"" + wuName.Trim() + "\"";
            cr.DataDefinition.FormulaFields["selEmplName"].Text = "\"" + emplName.Trim() + "\"";
            cr.DataDefinition.FormulaFields["selEmplID"].Text = emplID < 0 ? "\"\"" : "\"" + emplID.ToString().Trim() + "\""; ;
            cr.DataDefinition.FormulaFields["29"].Text = lastMonthDay < 29 ? "\"\"" : "\"29\"";
            cr.DataDefinition.FormulaFields["30"].Text = lastMonthDay < 30 ? "\"\"" : "\"30\"";
            cr.DataDefinition.FormulaFields["31"].Text = lastMonthDay < 31 ? "\"\"" : "\"31\"";

            cr.SetDataSource(data);
                       
            this.crystalReportViewer.ReportSource = cr;
        }
    }
}