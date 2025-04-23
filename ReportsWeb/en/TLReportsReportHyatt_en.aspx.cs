using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using Microsoft.Reporting.WebForms;

using TransferObjects;
using Util;


namespace ReportsWeb.en
{
    public partial class TLReportsReportHyatt_en : System.Web.UI.Page
    {
        DataSet ds = new DataSet();

        const string pageName = "TLReportsReportHyatt_en";

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
                    string fas = "";
                    ds = (DataSet)Session["TLReportsPage.dataSet"];
                    string reportType = Session["TLReportsPage.reportType"].ToString();
                    string singlePage = Session["TLReportsPage.singlePage"].ToString();
                    string date = Session["TLReportsPage.date"].ToString();
                    fas = Session["TLReportsPage.fas"].ToString();
                    Dictionary<string, string> dict = (Dictionary<string, string>)Session["TLReportsPage.dict"];
                    List<string> lista = new List<string>();
                    int num = 0;
                    foreach (KeyValuePair<string, string> pair in dict)
                    {
                        lista.Add(pair.Key);
                        num++;
                    }
                    while (num < 26)
                    {
                        lista.Add("");
                        num++;
                    }
                    Dictionary<string, string> dictName = (Dictionary<string, string>)Session["TLReportsPage.dictName"];
                    List<string> listaName = new List<string>();
                    int numName = 0;
                    foreach (KeyValuePair<string, string> pair in dictName)
                    {
                        listaName.Add(pair.Value);
                        numName++;
                    }
                    while (numName < 26)
                    {
                        listaName.Add("");
                        numName++;
                    }
                    ReportParameter param1 = new ReportParameter("reportType", reportType);
                    ReportParameter param2 = new ReportParameter("singlePage", singlePage);
                    ReportParameter param3 = new ReportParameter("date", date);
                    ReportParameter col1 = new ReportParameter("col1", lista[0]);
                    ReportParameter col2 = new ReportParameter("col2", lista[1]);
                    ReportParameter col3 = new ReportParameter("col3", lista[2]);
                    ReportParameter col4 = new ReportParameter("col4", lista[3]);
                    ReportParameter col5 = new ReportParameter("col5", lista[4]);
                    ReportParameter col6 = new ReportParameter("col6", lista[5]);
                    ReportParameter col7 = new ReportParameter("col7", lista[6]);
                    ReportParameter col8 = new ReportParameter("col8", lista[7]);
                    ReportParameter col9 = new ReportParameter("col9", lista[8]);
                    ReportParameter col10 = new ReportParameter("col10", lista[9]);
                    ReportParameter col11 = new ReportParameter("col11", lista[10]);
                    ReportParameter col12 = new ReportParameter("col12", lista[11]);
                    ReportParameter col13 = new ReportParameter("col13", lista[12]);
                    ReportParameter col14 = new ReportParameter("col14", lista[13]);
                    ReportParameter col15 = new ReportParameter("col15", lista[14]);
                    ReportParameter col16 = new ReportParameter("col16", lista[15]);
                    ReportParameter col17 = new ReportParameter("col17", lista[16]);
                    ReportParameter col18 = new ReportParameter("col18", lista[17]);
                    ReportParameter col19 = new ReportParameter("col19", lista[18]);
                    ReportParameter col20 = new ReportParameter("col20", lista[19]);
                    ReportParameter col21 = new ReportParameter("col21", lista[20]);
                    ReportParameter col22 = new ReportParameter("col22", lista[21]);
                    ReportParameter col23 = new ReportParameter("col23", lista[22]);
                    ReportParameter col24 = new ReportParameter("col24", lista[23]);
                    ReportParameter col25 = new ReportParameter("col25", lista[24]);
                    ReportParameter col26 = new ReportParameter("col26", lista[25]);
                    ReportParameter col1Name = new ReportParameter("col1Name", listaName[0]);
                    ReportParameter col2Name = new ReportParameter("col2Name", listaName[1]);
                    ReportParameter col3Name = new ReportParameter("col3Name", listaName[2]);
                    ReportParameter col4Name = new ReportParameter("col4Name", listaName[3]);
                    ReportParameter col5Name = new ReportParameter("col5Name", listaName[4]);
                    ReportParameter col6Name = new ReportParameter("col6Name", listaName[5]);
                    ReportParameter col7Name = new ReportParameter("col7Name", listaName[6]);
                    ReportParameter col8Name = new ReportParameter("col8Name", listaName[7]);
                    ReportParameter col9Name = new ReportParameter("col9Name", listaName[8]);
                    ReportParameter col10Name = new ReportParameter("col10Name", listaName[9]);
                    ReportParameter col11Name = new ReportParameter("col11Name", listaName[10]);
                    ReportParameter col12Name = new ReportParameter("col12Name", listaName[11]);
                    ReportParameter col13Name = new ReportParameter("col13Name", listaName[12]);
                    ReportParameter col14Name = new ReportParameter("col14Name", listaName[13]);
                    ReportParameter col15Name = new ReportParameter("col15Name", listaName[14]);
                    ReportParameter col16Name = new ReportParameter("col16Name", listaName[15]);
                    ReportParameter col17Name = new ReportParameter("col17Name", listaName[16]);
                    ReportParameter col18Name = new ReportParameter("col18Name", listaName[17]);
                    ReportParameter col19Name = new ReportParameter("col19Name", listaName[18]);
                    ReportParameter col20Name = new ReportParameter("col20Name", listaName[19]);
                    ReportParameter col21Name = new ReportParameter("col21Name", listaName[20]);
                    ReportParameter col22Name = new ReportParameter("col22Name", listaName[21]);
                    ReportParameter col23Name = new ReportParameter("col23Name", listaName[22]);
                    ReportParameter col24Name = new ReportParameter("col24Name", listaName[23]);
                    ReportParameter col25Name = new ReportParameter("col25Name", listaName[24]);
                    ReportParameter col26Name = new ReportParameter("col26Name", listaName[25]);
                    ReportParameter fasReport = new ReportParameter("fas", fas);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { param1, param2, param3, col1, col2, col3, col4, col5, col6, col7, col8, col9, col10, col11, col12, col13, col14, col15, col16, col17, col18, col19, col20, col21, col22, col23, col24, col25, col26, col1Name, col2Name, col3Name, col4Name, col5Name, col6Name, col7Name, col8Name, col9Name, col10Name, col11Name, col12Name, col13Name, col14Name, col15Name, col16Name, col17Name, col18Name, col19Name, col20Name, col21Name, col22Name, col23Name, col24Name, col25Name, col26Name, fasReport });
                    ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0], ds.Tables[0]));
                    ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[1], ds.Tables[2]));
                    // Set the parameters.
                    ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);

                    writeLog(DateTime.Now, false);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLReportsReport_en.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            try
            {
                ds = (DataSet)Session["TLReportsPage.dataSet"];
                if (e.DataSources != null)
                {
                    e.DataSources.Clear();
                }

                e.DataSources.Add(new ReportDataSource("IOPairsSummary_io_pair_analitical", ds.Tables[1]));

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLReportsReport_en.LocalReport_SubreportProcessing(): " + ex.Message.Replace('\n', ' ').Trim() + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

    }
}
