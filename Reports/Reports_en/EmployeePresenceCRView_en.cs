using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Resources;
using System.Data;
using System.Windows.Forms;

using Util;
using Common;

namespace Reports.Reports_en
{
	/// <summary>
	/// Summary description for EmployeePresenceCRView_en.
	/// </summary>
	public class EmployeePresenceCRView_en : System.Windows.Forms.Form
	{
		private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		DebugLog debug;
		CultureInfo culture;
		ResourceManager rm;


		public EmployeePresenceCRView_en(DataSet Data, DateTime from)
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
			this.Text = rm.GetString("employeePresenceReport", culture);

			EmployeePresenceCR_en  ecr = new EmployeePresenceCR_en();
			ecr.DataDefinition.FormulaFields["month"].Text = "\"" + from.ToString("MM.yyyy") + "\"";

            int lastMonthDay = new DateTime(from.AddMonths(1).Year, from.AddMonths(1).Month, 1).AddDays(-1).Day;
            ecr.DataDefinition.FormulaFields["29"].Text = lastMonthDay < 29 ? "\"\"" : "\"29\"";
            ecr.DataDefinition.FormulaFields["30"].Text = lastMonthDay < 30 ? "\"\"" : "\"30\"";
            ecr.DataDefinition.FormulaFields["31"].Text = lastMonthDay < 31 ? "\"\"" : "\"31\"";

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmployeePresenceCRView_en));
            this.crystalReportViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // crystalReportViewer
            // 
            this.crystalReportViewer.ActiveViewIndex = -1;
            this.crystalReportViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer.Name = "crystalReportViewer";
            this.crystalReportViewer.SelectionFormula = "";
            this.crystalReportViewer.Size = new System.Drawing.Size(840, 621);
            this.crystalReportViewer.TabIndex = 0;
            this.crystalReportViewer.ViewTimeSelectionFormula = "";
            // 
            // EmployeePresenceCRView_en
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(840, 621);
            this.Controls.Add(this.crystalReportViewer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EmployeePresenceCRView_en";
            this.Text = "Employee Presence";
            this.ResumeLayout(false);

		}
		#endregion
	}
}
