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

namespace Reports.Mittal.Mittal_sr
{
	/// <summary>
	/// Summary description for MittalWUSummaryCRView_sr.
	/// </summary>
	public class MittalWUSummaryCRView_sr : System.Windows.Forms.Form
	{
		private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		DebugLog debug;
		CultureInfo culture;
		ResourceManager rm;

		public MittalWUSummaryCRView_sr(DataSet data, DateTime from, DateTime to)
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
			this.Text = rm.GetString("employeeReports", culture);

			Reports.Mittal.Mittal_sr.MittalWorkingUnitSummaryCR_sr ecr = new Reports.Mittal.Mittal_sr.MittalWorkingUnitSummaryCR_sr();
			ecr.DataDefinition.FormulaFields["from"].Text = "\"" + from.ToString("dd.MM.yyyy") + "\"";
			ecr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";
			ecr.SetDataSource(data);
			this.crystalReportViewer1.ReportSource = ecr;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MittalWUSummaryCRView_sr));
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer1.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.SelectionFormula = "";
            this.crystalReportViewer1.Size = new System.Drawing.Size(784, 585);
            this.crystalReportViewer1.TabIndex = 0;
            this.crystalReportViewer1.ViewTimeSelectionFormula = "";
            // 
            // MittalWUSummaryCRView_sr
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(784, 585);
            this.Controls.Add(this.crystalReportViewer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MittalWUSummaryCRView_sr";
            this.Text = "Mittal working unit summary report";
            this.ResumeLayout(false);

		}
		#endregion
	}
}
