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


namespace Reports.Reports_sr
{
    public partial class EmplVacDetailsCRView_sr : Form
    {
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer;
	
		DebugLog debug;
		CultureInfo culture;
		ResourceManager rm;

        public EmplVacDetailsCRView_sr(DataSet Data, string selWorkingUnit, string selEmployee, string selYear, string lblIncludeLast
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

            Reports_sr.EmplVacDetailsCR_sr cr = new Reports_sr.EmplVacDetailsCR_sr();
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