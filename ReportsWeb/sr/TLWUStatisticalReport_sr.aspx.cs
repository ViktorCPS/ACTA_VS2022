using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Configuration;
using Util;
using TransferObjects;

namespace ReportsWeb.sr
{
    public partial class TLWUStatisticalReport_sr : System.Web.UI.Page
    {
        string start_time = "";
        string end_time = "";
        string wu = "";

        const string pageName = "TLWUStatisticalReport_sr";

        private DateTime LoadTime
        {
            get
            {
                DateTime loadDate = new DateTime();
                if (ViewState["loadDate"] != null && ViewState["loadDate"] is DateTime)
                {
                    loadDate = (DateTime)ViewState["loadDate"];
                }

                return loadDate;
            }
            set
            {
                if (value.Equals(new DateTime()))
                    ViewState["loadDate"] = null;
                else
                    ViewState["loadDate"] = value;
            }
        }

        private string Message
        {
            get
            {
                string message = "";
                if (ViewState["message"] != null)
                    message = ViewState["message"].ToString().Trim();

                return message;
            }
            set
            {
                if (value.Trim().Equals(""))
                    ViewState["message"] = null;
                else
                    ViewState["message"] = value;
            }
        }

        private DateTime StartLoadTime
        {
            get
            {
                DateTime startLoadDate = new DateTime();
                if (ViewState["startLoadDate"] != null && ViewState["startLoadDate"] is DateTime)
                {
                    startLoadDate = (DateTime)ViewState["startLoadDate"];
                }

                return startLoadDate;
            }
            set
            {
                if (value.Equals(new DateTime()))
                    ViewState["startLoadDate"] = null;
                else
                    ViewState["startLoadDate"] = value;
            }
        }

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            try
            {
                StartLoadTime = DateTime.Now;
                LoadTime = new DateTime();
                Message = "";
            }
            catch { }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            try
            {
                writeLog(DateTime.Now, true);
            }
            catch { }
        }

        private void writeLog(DateTime date, bool writeToFile)
        {
            try
            {
                string writeFile = ConfigurationManager.AppSettings["writeLoadTime"];

                if (writeFile != null && writeFile.Trim().ToUpper().Equals(Constants.yes.Trim().ToUpper()))
                {
                    DebugLog log = new DebugLog(Constants.logFilePath + "LoadTime.txt");

                    if (!writeToFile)
                    {
                        string message = pageName;

                        if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO)
                            message += "|" + ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).Name.Trim();

                        message += "|" + date.ToString("dd.MM.yyyy HH:mm:ss");

                        message += "|" + ((int)date.Subtract(StartLoadTime).TotalMilliseconds).ToString();

                        Message = message;
                        LoadTime = date;
                    }
                    else if (Message != null && !Message.Trim().Equals(""))
                    {
                        Message += "|" + ((int)date.Subtract(LoadTime).TotalMilliseconds).ToString();

                        log.writeLog(Message);
                        StartLoadTime = new DateTime();
                        LoadTime = new DateTime();
                        Message = "";
                    }
                }
            }
            catch { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    string data1 = "TLWUStatistical.dataTable" + 1;
                    string data2 = "TLWUStatistical.dataTable" + 2;
                    string data3 = "TLWUStatistical.dataTable" + 3;
                    string data4 = "TLWUStatistical.dataTable" + 4;
                    string numOfCharts = Session["TLWUStatistical.numOfCharts"].ToString();
                    string reportType = Session["TLWUStatistical.report"].ToString();


                    string realised1 = "";
                    string planned1 = "";

                    string realised2 = "";
                    string planned2 = "";

                    string realised3 = "";
                    string planned3 = "";

                    string realised4 = "";
                    string planned4 = "";

                    string title1 = "";
                    string title2 = "";
                    string title3 = "";
                    string title4 = "";
                    string fromDate = Session["TLWUStatistical.fromDate"].ToString();
                    string toDate = Session["TLWUStatistical.toDate"].ToString();

                    if (Session["TLWUStatistical.title" + 1] != null)
                        title1 = Session["TLWUStatistical.title" + 1].ToString();

                    if (Session["TLWUStatistical.title" + 2] != null)
                        title2 = Session["TLWUStatistical.title" + 2].ToString();

                    if (Session["TLWUStatistical.title" + 3] != null)
                        title3 = Session["TLWUStatistical.title" + 3].ToString();

                    if (Session["TLWUStatistical.title" + 4] != null)
                        title4 = Session["TLWUStatistical.title" + 4].ToString();

                    if (Session["TLWUStatistical.realised" + 1] != null)
                        realised1 = Session["TLWUStatistical.realised" + 1].ToString();
                    if (Session["TLWUStatistical.planned" + 1] != null)
                        planned1 = Session["TLWUStatistical.planned" + 1].ToString();

                    if (Session["TLWUStatistical.realised" + 2] != null)
                        realised2 = Session["TLWUStatistical.realised" + 2].ToString();
                    if (Session["TLWUStatistical.planned" + 2] != null)
                        planned2 = Session["TLWUStatistical.planned" + 2].ToString();

                    if (Session["TLWUStatistical.realised" + 3] != null)
                        realised3 = Session["TLWUStatistical.realised" + 3].ToString();
                    if (Session["TLWUStatistical.planned" + 3] != null)
                        planned3 = Session["TLWUStatistical.planned" + 3].ToString();

                    if (Session["TLWUStatistical.realised" + 4] != null)
                        realised4 = Session["TLWUStatistical.realised" + 4].ToString();
                    if (Session["TLWUStatistical.planned" + 4] != null)
                        planned4 = Session["TLWUStatistical.planned" + 4].ToString();

                    DataTable table1 = (DataTable)Session[data1];
                    DataTable table2 = (DataTable)Session[data2];
                    DataTable table3 = (DataTable)Session[data3];
                    DataTable table4 = (DataTable)Session[data4];
                    if (table2 == null)
                    {
                        table2 = new DataTable();
                        table2.Columns.Add("pass_type", typeof(System.String));
                        table2.Columns.Add("color", typeof(System.String));
                        table2.Columns.Add("hours", typeof(System.String));
                        table2.Columns.Add("perRealised", typeof(System.String));
                        table2.Columns.Add("perPlanned", typeof(System.String));
                    }
                    if (table3 == null)
                    {
                        table3 = new DataTable();
                        table3.Columns.Add("pass_type", typeof(System.String));
                        table3.Columns.Add("color", typeof(System.String));
                        table3.Columns.Add("hours", typeof(System.String));
                        table3.Columns.Add("perRealised", typeof(System.String));
                        table3.Columns.Add("perPlanned", typeof(System.String));
                    }
                    if (table4 == null)
                    {
                        table4 = new DataTable();
                        table4.Columns.Add("pass_type", typeof(System.String));
                        table4.Columns.Add("color", typeof(System.String));
                        table4.Columns.Add("hours", typeof(System.String));
                        table4.Columns.Add("perRealised", typeof(System.String));
                        table4.Columns.Add("perPlanned", typeof(System.String));
                    }
                    ReportParameter numOfChart = new ReportParameter("numOfChart", numOfCharts);
                    ReportParameter report = new ReportParameter("report", reportType);
                    ReportParameter realised1Param = new ReportParameter("realised1", realised1);
                    ReportParameter planned1Param = new ReportParameter("planned1", planned1);
                    ReportParameter realised2Param = new ReportParameter("realised2", realised2);
                    ReportParameter planned2Param = new ReportParameter("planned2", planned2);
                    ReportParameter realised3Param = new ReportParameter("realised3", realised3);
                    ReportParameter planned3Param = new ReportParameter("planned3", planned3);
                    ReportParameter realised4Param = new ReportParameter("realised4", realised4);
                    ReportParameter planned4Param = new ReportParameter("planned4", planned4);

                    ReportParameter title1Param = new ReportParameter("title1", title1);
                    ReportParameter title2Param = new ReportParameter("title2", title2);
                    ReportParameter title3Param = new ReportParameter("title3", title3);
                    ReportParameter title4Param = new ReportParameter("title4", title4);

                    ReportParameter fromDateParam = new ReportParameter("fromDate", fromDate);
                    ReportParameter toDateParam = new ReportParameter("toDate", toDate);

                    ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0], table1));
                    ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[1], table2));
                    ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[2], table3));
                    ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[3], table4));
                    // Set the parameters.
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { numOfChart, report, realised1Param, realised2Param, realised3Param, realised4Param, planned1Param, planned2Param, planned3Param, planned4Param, title1Param, title2Param, title3Param, title4Param, fromDateParam, toDateParam });
                    //ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(subreportProcessingEvent);
                    ReportViewer1.LocalReport.Refresh();

                    writeLog(DateTime.Now, false);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLWUStatisticalReport_sr.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }       
    }
}
