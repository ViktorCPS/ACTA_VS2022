using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.Configuration;

using Util;
using TransferObjects;


namespace ReportsWeb.sr
{
    public partial class HRSSCOutstandingDataReport_sr : System.Web.UI.Page
    {
        const string pageName = "HRSSCOutstandingDataReport_sr";

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
                    DataSet ds = (DataSet)Session["HRSSCOutstandingDataPage.dataSet"];
                    string standard = "1";
                    string group = "0";
                    string schema = "0";
                    string effective = "0";
                    string employee = Session["HRSSCOutstandingDataPage.empl"].ToString();
                    string anomalycategory = Session["HRSSCOutstandingDataPage.anomalycategory"].ToString();
                    string fromDate = Session["HRSSCOutstandingDataPage.fromDate"].ToString();
                    string toDate = Session["HRSSCOutstandingDataPage.toDate"].ToString();

                    standard = Session["HRSSCOutstandingDataPage.standard"].ToString();
                   if(Session["HRSSCOutstandingDataPage.group_hours"] != null)
                       group = Session["HRSSCOutstandingDataPage.group_hours"].ToString();
                   if (Session["HRSSCOutstandingDataPage.schema_hours"] != null)
                        schema = Session["HRSSCOutstandingDataPage.schema_hours"].ToString();
                   if (Session["HRSSCOutstandingDataPage.effective_hours"] != null)
                        effective = Session["HRSSCOutstandingDataPage.effective_hours"].ToString();

                    ReportParameter param1 = new ReportParameter("empl", employee);
                    ReportParameter param2 = new ReportParameter("anomalycategory", anomalycategory);
                    ReportParameter param3 = new ReportParameter("fromDate", fromDate);
                    ReportParameter param4 = new ReportParameter("toDate", toDate);
                    ReportParameter param5 = new ReportParameter("standard", standard);
                    ReportParameter param6 = new ReportParameter("group_hours", group);
                    ReportParameter param7 = new ReportParameter("schema_hours", schema);
                    ReportParameter param8 = new ReportParameter("effective_hours", effective);

                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { param1, param2, param3, param4,param5,param6,param7,param8});
                    ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0], ds.Tables[0]));

                    writeLog(DateTime.Now, false);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in HRSSCOutstandingDataReport_sr.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
    }
}
