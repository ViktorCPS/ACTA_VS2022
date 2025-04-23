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

namespace Reports.Reports_fi
{
    public partial class EmployeeAbsencesCRView_fi : Form
    {
      private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer;
	
		DebugLog debug;
		CultureInfo culture;
		ResourceManager rm;

        public EmployeeAbsencesCRView_fi(DataSet Data, string selWorkingUnit, string selEmployee, string selPassType, string lblFootNote, DateTime from, DateTime to, string selUser,string totals)
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
			this.Text = rm.GetString("employeeAbsenceReports", culture);

			Reports_fi.EmployeeAbsencesCR_fi  cr = new Reports_fi.EmployeeAbsencesCR_fi();
            cr.DataDefinition.FormulaFields["selWorkingUnit"].Text = "\"" + selWorkingUnit + "\"";
            cr.DataDefinition.FormulaFields["selUser"].Text = "\"" + selUser + "\"";
            cr.DataDefinition.FormulaFields["totals"].Text = "\"" + totals + "\"";
            cr.DataDefinition.FormulaFields["selEmployee"].Text = "\"" + selEmployee + "\"";
            cr.DataDefinition.FormulaFields["selPassType"].Text = "\"" + selPassType + "\"";
            cr.DataDefinition.FormulaFields["lblFootnote"].Text = "\"" + lblFootNote + "\"";
            cr.DataDefinition.FormulaFields["from"].Text = "\"" + from.ToString("dd.MM.yyyy") + "\"";
			cr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";
			cr.SetDataSource(Data);
			this.crystalReportViewer.ReportSource = cr;
		}

    }
}