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
using Util;
using TransferObjects;
using System.IO;


namespace Reports.FIAT
{
    public partial class Decisions : Form
    {
        ResourceManager rm;
        private CultureInfo culture;
        DebugLog log;
        private string wuString = "";

        private List<WorkingUnitTO> wUnits;

        private Dictionary<int, EmployeeAsco4TO> ascoDict = new Dictionary<int, EmployeeAsco4TO>();

        Dictionary<int, PassTypeTO> typesDict = new Dictionary<int, PassTypeTO>();

        ApplUserTO logInUser;

        Dictionary<int, WorkingUnitTO> wuDict = new Dictionary<int, WorkingUnitTO>();

        public Decisions()
        {
            InitializeComponent();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(Decisions).Assembly);
            setLanguage();

        }
        private void setLanguage()
        {
            try
            {
                lblReportType.Text = rm.GetString("lblReportType", culture);
                gbSelection.Text = rm.GetString("gbSearchCriteria", culture);
                lblCompany.Text = rm.GetString("lblCompany", culture);
                lblFrom.Text = rm.GetString("lblMonth", culture);
                btnReport.Text = rm.GetString("btnReport", culture);
                btnClose.Text = rm.GetString("btnClose", culture);
                this.Text = rm.GetString("hdrDecisions", culture);
            }
            catch (Exception ex)
            {
                log.writeLog("Exception in Decisions.setLanguage(). " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ": " + ex.Message);
                MessageBox.Show(ex.Message);
            }
        }
        private void populateCbType()
        {
            try
            {
                cbType.Items.Add(rm.GetString("RedistributionOfWorkingHours", culture));

            }
            catch (Exception ex)
            {
                log.writeLog("Exception in Decisions.setLanguage(). " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ": " + ex.Message);
                throw ex;
            }
        }

        private void Decisions_Load(object sender, EventArgs e)
        {
            try
            {
                populateCbType();
                cbType.SelectedIndex = 0;

                wuDict = new WorkingUnit().getWUDictionary();

                wUnits = new List<WorkingUnitTO>();

                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.EmployeesPurpose);
                }

                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }
                typesDict = new PassType().SearchDictionary();

                populateCompanyList();


            }
            catch (Exception ex)
            {

                log.writeLog(DateTime.Now + " Decisions.Decisions_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateCompanyList()
        {
            try
            {
                List<WorkingUnitTO> wuArray = new WorkingUnit().getRootWorkingUnitsList(wuString);

                cbCompany.DataSource = wuArray;
                cbCompany.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Decisions.populateCompanyList(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
               
                    string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Decisions" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf";
                    DateTime month = dtMonth.Value;
                    DateTime fromDate = new DateTime(month.Year, month.Month, 1);
                    DateTime toDate = new DateTime(month.Year, month.Month, 1);
                    toDate = toDate.AddMonths(1);
                    toDate = toDate.AddDays(-1);
                    string message = Common.Misc.generatePDFDecisions(((WorkingUnitTO)cbCompany.SelectedItem).WorkingUnitID, filePath, fromDate, toDate);
                    if (message.Equals(""))
                    {

                        MessageBox.Show(rm.GetString("lblReportGenerated", culture));
                    }
                    else
                    {
                        if (message.Equals("Res.person"))
                            MessageBox.Show(rm.GetString("noResPerson", culture));
                        else
                            MessageBox.Show(rm.GetString("noReportData", culture));

                    }
            }
            catch (Exception ex)
            {

                log.writeLog(DateTime.Now + " Decisions.btnReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}
