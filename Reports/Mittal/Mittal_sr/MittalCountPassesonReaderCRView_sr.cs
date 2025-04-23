using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Common;
using Util;
using System.Globalization;
using System.Resources;

namespace Reports.Mittal.Mittal_sr
{
    public partial class MittalCountPassesonReaderCRView_sr : Form
    {
       private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		DebugLog debug;
		CultureInfo culture;
		ResourceManager rm;

        public MittalCountPassesonReaderCRView_sr(DataSet Data, string selLocation, string selDirection)
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
			this.Text = rm.GetString("employeeAnaliticReports", culture);

			Reports.Mittal.Mittal_sr.MittalCountPassesOnReaderCR_rs  ecr =
                new Reports.Mittal.Mittal_sr.MittalCountPassesOnReaderCR_rs();

            ecr.DataDefinition.FormulaFields["selLocation"].Text = "\"" + selLocation + "\"";
            ecr.DataDefinition.FormulaFields["selDirection"].Text = "\"" + selDirection + "\"";
			ecr.SetDataSource(Data);
			this.crystalReportViewer1.ReportSource = ecr;
		}

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {
            this.crystalReportViewer1.DisplayGroupTree = false;
        }
    }
}