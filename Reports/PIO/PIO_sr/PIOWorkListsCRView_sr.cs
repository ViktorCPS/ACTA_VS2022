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
using TransferObjects;

namespace Reports.PIO.PIO_sr
{
    public partial class PIOWorkListsCRView_sr : Form
    {
        CultureInfo culture;
        ResourceManager rm;

        public PIOWorkListsCRView_sr(DataSet data, DateTime from, DateTime to, string indicator)
        {
            InitializeComponent();

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(PIOWorkListsCRView_sr).Assembly);
            this.Text = rm.GetString("pioWorkListsReports", culture);

            Reports.PIO.PIO_sr.PIOWorkListsCR_sr cr = new Reports.PIO.PIO_sr.PIOWorkListsCR_sr();
            cr.DataDefinition.FormulaFields["from"].Text = "\"" + from.ToString("dd.MM.yyyy") + "\"";
            cr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";

            ApplUserTO applUser = NotificationController.GetLogInUser();
            cr.DataDefinition.FormulaFields["user_name"].Text = "\"" + applUser.Name + "\"";
            cr.DataDefinition.FormulaFields["user_id"].Text = "\"" + applUser.UserID + "\"";

            cr.DataDefinition.FormulaFields["indicator"].Text = "\"" + indicator + "\"";

            cr.SetDataSource(data);
            this.crystalReportViewer1.ReportSource = cr;
        }
    }
}