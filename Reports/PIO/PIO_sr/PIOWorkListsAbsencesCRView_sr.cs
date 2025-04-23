using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Resources;
using System.Globalization;
using Common;

namespace Reports.PIO.PIO_sr
{
    public partial class PIOWorkListsAbsencesCRView_sr : Form
    {
        CultureInfo culture;
        ResourceManager rm;

        public PIOWorkListsAbsencesCRView_sr(DataSet data, DateTime from, DateTime to)
        {
            InitializeComponent();

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(PIOWorkListsAbsencesCRView_sr).Assembly);
            this.Text = rm.GetString("pioWorkListsAbsencesReports", culture);

            Reports.PIO.PIO_sr.PIOWorkListsAbsencesCR_sr cr = new Reports.PIO.PIO_sr.PIOWorkListsAbsencesCR_sr();
            cr.DataDefinition.FormulaFields["from"].Text = "\"" + from.ToString("dd.MM.yyyy") + "\"";
            cr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";

            cr.SetDataSource(data);
            this.crystalReportViewer1.ReportSource = cr;
        }
    }
}