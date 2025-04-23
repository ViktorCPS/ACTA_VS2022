using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;

using Util;
using Common;

namespace Reports.DSF.DSF_en
{
    public partial class PresenceReportCRView_en : Form
    {
        DebugLog debug;
        CultureInfo culture;
        ResourceManager rm;

        public PresenceReportCRView_en(DataSet data, DateTime from, DateTime to)
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                debug = new DebugLog(logFilePath);

                // Language tool
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("Reports.ReportResource", typeof(PresenceReportCRView_en).Assembly);
                this.Text = rm.GetString("PresenceReport", culture);

                PresenceReportSummaryCR_en ecr = new PresenceReportSummaryCR_en();
                ecr.DataDefinition.FormulaFields["from"].Text = "\"" + from.ToString("dd.MM.yyyy") + "\"";
                ecr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";
                ecr.SetDataSource(data);
                this.crystalReportViewer.ReportSource = ecr;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
