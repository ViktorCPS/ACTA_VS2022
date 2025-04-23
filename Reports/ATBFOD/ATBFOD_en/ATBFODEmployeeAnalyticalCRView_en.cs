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

namespace Reports.ATBFOD.ATBFOD_en
{
	/// <summary>
	/// Summary description for EmployeeAnalyticalCRView.
	/// </summary>
	public class ATBFODEmployeeAnalyticalCRView_en : System.Windows.Forms.Form
	{
		private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		DebugLog debug;
		CultureInfo culture;
		ResourceManager rm;

		public ATBFODEmployeeAnalyticalCRView_en(DataSet Data, DateTime date, string wuName)
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

            ATBFOD_sr.ATBFODEmployeeAnaliticalCR_sr ecr = new ATBFOD_sr.ATBFODEmployeeAnaliticalCR_sr();
            ecr.DataDefinition.FormulaFields["date"].Text = "\"" + date.ToString("dd.MM.yyyy") + "\"";
            ecr.DataDefinition.FormulaFields["selWorkingUnit"].Text = "\"" + wuName + "\"";
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ATBFODEmployeeAnalyticalCRView_en));
            this.crystalReportViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // crystalReportViewer
            // 
            this.crystalReportViewer.ActiveViewIndex = -1;
            this.crystalReportViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer.DisplayGroupTree = false;
            this.crystalReportViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer.Name = "crystalReportViewer";
            this.crystalReportViewer.SelectionFormula = "";
            this.crystalReportViewer.Size = new System.Drawing.Size(840, 621);
            this.crystalReportViewer.TabIndex = 0;
            this.crystalReportViewer.ViewTimeSelectionFormula = "";
            // 
            // ATBFODEmployeeAnalyticalCRView_en
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(840, 621);
            this.Controls.Add(this.crystalReportViewer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ATBFODEmployeeAnalyticalCRView_en";
            this.Text = "Employee Analytical";
            this.ResumeLayout(false);

		}
		#endregion
	}
}
