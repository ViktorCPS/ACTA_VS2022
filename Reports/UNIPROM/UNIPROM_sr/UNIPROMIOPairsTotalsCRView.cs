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

namespace Reports.UNIPROM.UNIPROM_sr
{
    public partial class UNIPROMIOPairsTotalsCRView : Form
    {
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		DebugLog debug;
		CultureInfo culture;
		ResourceManager rm;

        public UNIPROMIOPairsTotalsCRView(DataSet Data, string fromDate, string fromTime, string toDate, string toTime, string selDriver, string selVechicle, string selDirection, string TotalIn,string TotalOut)
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
			this.Text = rm.GetString("UNIPROMIOPairsReport", culture);

			UNIPROM_sr.IOPairsTotalsCR_sr ecr = new UNIPROM_sr.IOPairsTotalsCR_sr();
			ecr.DataDefinition.FormulaFields["selFromDate"].Text = "\"" + fromDate + "\"";
			ecr.DataDefinition.FormulaFields["selToDate"].Text = "\"" + toDate+ "\"";
            ecr.DataDefinition.FormulaFields["selFromTime"].Text = "\"" + fromTime + "\"";
			ecr.DataDefinition.FormulaFields["selToTime"].Text = "\"" + toTime + "\"";
            ecr.DataDefinition.FormulaFields["selDirection"].Text = "\"" + selDirection + "\"";
            ecr.DataDefinition.FormulaFields["selVechicle"].Text = "\"" + selVechicle + "\"";
            ecr.DataDefinition.FormulaFields["selDriver"].Text = "\"" + selDriver + "\"";
            ecr.DataDefinition.FormulaFields["totalIn"].Text = "\"" + TotalIn + "\"";
            ecr.DataDefinition.FormulaFields["totalOut"].Text = "\"" + TotalOut + "\"";
			ecr.SetDataSource(Data);
            crystalReportViewer.DisplayGroupTree = false;
			this.crystalReportViewer.ReportSource = ecr;
		}

		
    }
}