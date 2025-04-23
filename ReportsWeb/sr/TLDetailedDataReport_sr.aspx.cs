using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Reporting.WebForms;
using Util;
using TransferObjects;
using System.Configuration;

namespace ReportsWeb.sr
{
    public partial class TLDetailedDataReport_sr : System.Web.UI.Page
    {
        private DataTable tableSubReport = null;
        string employee = "";
        string passType = "";
        string start_time = "";
        string end_time = "";
        string pt_loc = "";

        const string pageName = "TLDetailedDataReport_sr";

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
                    string cbMontly = "";                    
                    // 0-not checked, 1-checked monthly totals, 2-checked employee totals
                    if (Session["TLDetailedDataPage.pt_loc"] != null)
                        pt_loc = Session["TLDetailedDataPage.pt_loc"].ToString();
                    if (Session["TLDetailedDataPage.pass_type"] != null)
                        passType = Session["TLDetailedDataPage.pass_type"].ToString();
                    if (Session["TLDetailedDataPage.employee"] != null)
                        employee = Session["TLDetailedDataPage.employee"].ToString();
                    if (Session["TLDetailedDataPage.start_time"] != null)
                        start_time = Session["TLDetailedDataPage.start_time"].ToString();
                    if (Session["TLDetailedDataPage.end_time"] != null)
                        end_time = Session["TLDetailedDataPage.end_time"].ToString();
                    if (Session["TLDetailedDataPage.checked"] != null)
                        cbMontly = Session["TLDetailedDataPage.checked"].ToString();
                    
                    ReportParameter fromDate = new ReportParameter("fromDate", start_time, false);
                    ReportParameter toDate = new ReportParameter("toDate", end_time, false);
                    ReportParameter selEmpolyee = new ReportParameter("employee", employee, false);
                    ReportParameter pType = new ReportParameter("pass_type", passType, false);
                    ReportParameter subReport = new ReportParameter("subReport", cbMontly, false);
                    ReportParameter ptLoc = new ReportParameter("ptLoc", pt_loc, false);

                    DataSet ds = (DataSet)Session["TLDetailedDataPage.io_pairs"];

                    ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0], ds.Tables[0]));
                    // Set the parameters.
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { fromDate, toDate, selEmpolyee, pType, subReport, ptLoc });
                    ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(subreportProcessingEvent);
                    ReportViewer1.LocalReport.Refresh();

                    writeLog(DateTime.Now, false);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLDetailedDataReport_sr.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void subreportProcessingEvent(object sender, SubreportProcessingEventArgs e)
        {
            try
            {
                DataSet ds = (DataSet)Session["TLDetailedDataPage.io_pairs"];
                if (e.DataSources != null)
                {
                    e.DataSources.Clear();
                }
                e.DataSources.Add(new ReportDataSource("IOPairsDS_io_pairs_monthly_pt", ds.Tables[2]));
                
                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLDetailedDataReport_sr.subreportProcessingEvent(): " + ex.Message.Replace('\n', ' ').Trim() + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
    }
}
