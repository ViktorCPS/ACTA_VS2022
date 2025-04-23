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

namespace Reports.Ministarstvo.Ministarstvo_sr
{
    public partial class MinistarstvoEmployeePresenceSumCRView : Form
    {
        DebugLog debug;
		CultureInfo culture;
		ResourceManager rm;

        public MinistarstvoEmployeePresenceSumCRView(DataSet Data, DateTime from, DateTime to, string workingUnit)
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
			this.Text = rm.GetString("employeePresenceSum", culture);

            Ministarstvo_sr.MinistarstvoEmployeePresenceSumCR_sr ecr = new Ministarstvo_sr.MinistarstvoEmployeePresenceSumCR_sr();
            ecr.DataDefinition.FormulaFields["from"].Text = "\"" + from.ToString("dd.MM.yyyy") + "\"";
            ecr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";
            ecr.DataDefinition.FormulaFields["WorkingUnit"].Text = "\"" + workingUnit + "\"";
			ecr.SetDataSource(Data);
			this.crystalReportViewer.ReportSource = ecr;
		}

		
		
    }
}