using System;
using System.Drawing;
using System.Data;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Resources;
using System.Windows.Forms;

using Util;
using Common;

namespace Reports.Reports_sr
{
	/// <summary>
	/// Summary description for EmployeeCRView.
	/// </summary>
	public class EmployeesCRView : System.Windows.Forms.Form
	{
		private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		DebugLog debug;
		CultureInfo culture;
		ResourceManager rm;

		public EmployeesCRView(DataSet Data, DateTime form, DateTime to)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);

			// Language tool
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("Reports.ReportResource", typeof(LocationsReports).Assembly);
			this.Text = rm.GetString("employeeReports", culture);

			Reports_sr.EmployeesCR_sr  ecr = new Reports_sr.EmployeesCR_sr();
			ecr.DataDefinition.FormulaFields["from"].Text = "\"" + form.ToString("dd.MM.yyyy") + "\"";
			ecr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";
			ecr.SetDataSource(Data);
			this.crystalReportViewer.ReportSource = ecr;
		}
        //  22.05.2019. BOJAN
        public EmployeesCRView(DataSet Data, DateTime date, string workingUnit, string orgUnit) {
            //
            // Required for Windows Form Designer support
            //
                InitializeComponent();

            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            debug = new DebugLog(logFilePath);

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(LocationsReports).Assembly);
            this.Text = rm.GetString("employeeReports", culture);

            DateTime selected = date;
            DateTime from = new DateTime(selected.Year, selected.Month, 1, 0, 0, 0);
            DateTime to = new DateTime();

            if (selected.Month == DateTime.Now.Month) {
                to = DateTime.Now.Date;
            }
            else {
                to = from.AddMonths(1).AddDays(-1);
            }


            Reports_sr.OpenPairsByEmployeesCR_sr ecr = new Reports_sr.OpenPairsByEmployeesCR_sr();
            ecr.DataDefinition.FormulaFields["from"].Text = "\"" + from.ToString("dd.MM.yyyy") + "\"";
            ecr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";
            ecr.DataDefinition.FormulaFields["working_unit"].Text = "\"" + workingUnit + "\"";
            ecr.DataDefinition.FormulaFields["org_unit"].Text = "\"" + orgUnit + "\"";

            ecr.SetDataSource(Data);
            this.crystalReportViewer.ReportSource = ecr;
        }

        //  24.05.2019. BOJAN
        public EmployeesCRView(DataSet Data, DateTime form, DateTime to, bool chbOpenPair) {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            debug = new DebugLog(logFilePath);

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(LocationsReports).Assembly);
            this.Text = rm.GetString("employeeReports", culture);

            Reports_sr.EmployeesForManualCreatedCR_2_sr ecr = new Reports_sr.EmployeesForManualCreatedCR_2_sr();
            ecr.DataDefinition.FormulaFields["from"].Text = "\"" + form.ToString("dd.MM.yyyy") + "\"";
            ecr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";
            ecr.SetDataSource(Data);
            this.crystalReportViewer.ReportSource = ecr;
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmployeesCRView));
            this.crystalReportViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // crystalReportViewer
            // 
            this.crystalReportViewer.ActiveViewIndex = -1;
            this.crystalReportViewer.AutoScroll = true;
            this.crystalReportViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer.Name = "crystalReportViewer";
            this.crystalReportViewer.SelectionFormula = "";
            this.crystalReportViewer.Size = new System.Drawing.Size(784, 585);
            this.crystalReportViewer.TabIndex = 0;
            this.crystalReportViewer.ViewTimeSelectionFormula = "";
            // 
            // EmployeesCRView
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(784, 585);
            this.Controls.Add(this.crystalReportViewer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EmployeesCRView";
            this.Text = "Employee Report";
            this.ResumeLayout(false);

		}
		#endregion
	}
}
