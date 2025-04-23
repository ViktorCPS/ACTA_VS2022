using System;
using System.Drawing;
using System.Data;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Resources;
using System.Windows.Forms;

using Common;
using Util;


namespace Reports.Reports_en
{
    public partial class EmplVacationsCRView_en : Form
    {
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer;
	
		DebugLog debug;
		CultureInfo culture;
		ResourceManager rm;

        public EmplVacationsCRView_en(DataSet Data, string selWorkingUnit, string selEmployee, string selYear, string lblIncludeLast
            , string  approvedFrom,string  approvedTo, string usedFrom, string usedTo, string leftFrom, string leftTo)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);

			// Language tool
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("Reports.ReportResource", typeof(LocationsReports).Assembly);
            this.Text = rm.GetString("employeeVacationRep", culture);

            Reports_en.EmplVacationsCR_en cr = new Reports_en.EmplVacationsCR_en();
            cr.DataDefinition.FormulaFields["selWorkingUnit"].Text = "\"" + selWorkingUnit + "\"";
            cr.DataDefinition.FormulaFields["selEmployee"].Text = "\"" + selEmployee + "\"";
            cr.DataDefinition.FormulaFields["selYear"].Text = "\"" + selYear + "\"";
            cr.DataDefinition.FormulaFields["lblIncludeLast"].Text = "\"" + lblIncludeLast + "\"";
            cr.DataDefinition.FormulaFields["selApprovedFrom"].Text = "\"" + approvedFrom.ToString() + "\"";
            cr.DataDefinition.FormulaFields["selApprovedTo"].Text = "\"" + approvedTo.ToString() + "\"";
            cr.DataDefinition.FormulaFields["selUsedFrom"].Text = "\"" + usedFrom.ToString() + "\"";
            cr.DataDefinition.FormulaFields["selUsedTo"].Text = "\"" + usedTo.ToString() + "\"";
            cr.DataDefinition.FormulaFields["selLeftFrom"].Text = "\"" + leftFrom.ToString() + "\"";
            cr.DataDefinition.FormulaFields["selLeftTo"].Text = "\"" + leftTo.ToString() + "\"";
            cr.SetDataSource(Data);
			this.crystalReportViewer.ReportSource = cr;
		}
    }
}