using System;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Globalization;
using System.Resources;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Common;
using Util;

namespace Reports.Millennium.Millennium_sr
{
	/// <summary>
	/// Summary description for EmployeePresenceTypeCRView_sr.
	/// </summary>
	public class MillenniumEmployeePresenceTypeCRView_sr : System.Windows.Forms.Form
	{
		private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		DebugLog debug;
		CultureInfo culture;
		ResourceManager rm;

		public MillenniumEmployeePresenceTypeCRView_sr(DataSet Data, DateTime from, DateTime to)
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
			this.Text = rm.GetString("employeePresenceTypeReport", culture);

            Millennium_sr.MillenniumEmployeePresenceTypeCR_sr ecr = new Millennium_sr.MillenniumEmployeePresenceTypeCR_sr();
			ecr.DataDefinition.FormulaFields["from"].Text = "\"" + from.ToString("dd.MM.yyyy") + "\"";
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MillenniumEmployeePresenceTypeCRView_sr));
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
            this.crystalReportViewer.Size = new System.Drawing.Size(784, 585);
            this.crystalReportViewer.TabIndex = 0;
            this.crystalReportViewer.ViewTimeSelectionFormula = "";
            // 
            // EmployeePresenceTypeCRView_sr
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(784, 585);
            this.Controls.Add(this.crystalReportViewer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EmployeePresenceTypeCRView_sr";
            this.Text = "Izvestaj o prisutnosti po tipovima prolaska";
            this.ResumeLayout(false);

		}
		#endregion
	}
}