using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Globalization;
using System.Resources;
using System.Runtime.InteropServices;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.IO;

using ACTAWorkAnalysisReports;
using Util;
using TransferObjects;
using Common;
using System.Threading;
using System.Windows.Forms;

namespace ACTAWebUI
{
    public partial class WorkAnalyzeReports : System.Web.UI.Page
    {
        const string pageName = "WorkAnalyzeReports";

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

        DateTime fromDate = DateTime.Now;
        DateTime toDate = DateTime.Now;
        protected void ShowCalendarFrom(object sender, EventArgs e)
        {
            calendarFrom.Visible = true;
            btnFrom.Visible = false;
        }
        protected void ShowCalendarTo(object sender, EventArgs e)
        {
            calendarTo.Visible = true;
            btnTo.Visible = false;
        }
        protected void DataFromChange(object sender, EventArgs e)
        {
            fromDate = calendarFrom.SelectedDate;
            tbFromDate.Text = fromDate.ToString("dd.MM.yyyy.");
            calendarFrom.Visible = false;
            btnFrom.Visible = true;
        }
        protected void DataToChange(object sender, EventArgs e)
        {
            toDate = calendarTo.SelectedDate;
            tbToDate.Text = toDate.ToString("dd.MM.yyyy.");
            calendarTo.Visible = false;
            btnTo.Visible = true;
        }
        //protected void ShowCalendarPrev(object sender, EventArgs e)
        //{
        //    calendarPrev.Visible = true;
        //    btnPrev.Visible = false;
        //}
        //protected void ShowCalendarNext(object sender, EventArgs e)
        //{
        //    calendarNext.Visible = true;
        //    btnNext.Visible = false;
        //}
        protected void DataPrevChange(object sender, EventArgs e)
        {

        }
        protected void DataNextChange(object sender, EventArgs e)
        {

        }

        protected void DataFrom1Change(object sender, EventArgs e)
        {
            tbFromDate1.Text = calendarFrom1.SelectedDate.ToString("dd.MM.yyyy.");
            calendarFrom1.Visible = false;
            btnFrom1.Visible = true;
        }
        protected void ShowCalendarFrom1(object sender, EventArgs e)
        {
            calendarFrom1.Visible = true;
            btnFrom1.Visible = false;
        }
        protected void DateTo1Change(object sender, EventArgs e)
        {
            tbToDate1.Text = calendarTo1.SelectedDate.ToString("dd.MM.yyyy.");
            calendarTo1.Visible = false;
            btnTo1.Visible = true;
        }
        protected void ShowCalendarTo1(object sender, EventArgs e)
        {
            calendarTo1.Visible = true;
            btnTo1.Visible = false;
        }
        protected void fromDate_TreciIzvestaj_Changed(object sender, EventArgs e)
        {
            tbFromDate_TreciIzvestaj.Text = calendarFromDate_TreciIzvestaj.SelectedDate.ToString("dd.MM.yyyy.");
            calendarFromDate_TreciIzvestaj.Visible = false;
            btnFromDate_TreciIzvestaj.Visible = true;
        }
        protected void btnFrom_TreciIzvestaj_Click(object sender, EventArgs e)
        {
            btnFromDate_TreciIzvestaj.Visible = false;
            calendarFromDate_TreciIzvestaj.Visible = true;
        }
        protected void toDate_TreciIzvestaj_Changed(object sender, EventArgs e)
        {
            tbToDate_TreciIzvestaj.Text = calendarToDate_TreciIzvestaj.SelectedDate.ToString("dd.MM.yyyy.");
            calendarToDate_TreciIzvestaj.Visible = false;
            btnToDate_TreciIzvestaj.Visible = true;
        }
        protected void btnTo_TreciIzvestaj_Click(object sender, EventArgs e)
        {
            btnToDate_TreciIzvestaj.Visible = false;
            calendarToDate_TreciIzvestaj.Visible = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                //rbPrevMonth.Attributes.Add("onclick", "return check('rbPrevMonth', 'rbPrevWeek');");
                //rbPrevWeek.Attributes.Add("onclick", "return check('rbPrevWeek', 'rbPrevMonth');");
                ////rbLast2Weeks.Attributes.Add("onclick", "return check('rbLast2Weeks', 'rbPrevMonth', 'rbPrevWeek');");
                // if user do not want to reload selected filter and get results for that filter, parameter reloadState should be passes through url and set to false
                if (!IsPostBack)
                {
                    CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                    ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WorkAnalyzeReports).Assembly);
                    btnFromDate.Attributes.Add("onclick", "return calendarPicker('tbFromDate', 'false');");
                    btnToDate.Attributes.Add("onclick", "return calendarPicker('tbToDate', 'false');");
                    //date time for other report
                    btnFromDate1.Attributes.Add("onclick", "return calendarPicker('tbFromDate1', 'true');");
                    btnToDate1.Attributes.Add("onclick", "return calendarPicker('tbToDate1', 'true');");
                    btnFromDate1.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnFromDate1.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnToDate1.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnToDate1.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnPrevDayPeriod1.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnPrevDayPeriod1.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnNextDayPeriod1.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnNextDayPeriod1.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnReport1.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");

                    string mail = "";
                    string user_id = "";
                    lblError.Text = "";
                    btnReport.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    if (Session[Constants.sessionFromDate] != null && Session[Constants.sessionFromDate] is DateTime)
                        tbFromDate.Text = ((DateTime)Session[Constants.sessionFromDate]).ToString(Constants.dateFormat.Trim());
                    else
                        tbFromDate.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());

                    if (Session[Constants.sessionToDate] != null && Session[Constants.sessionToDate] is DateTime)
                        tbToDate.Text = ((DateTime)Session[Constants.sessionToDate]).ToString(Constants.dateFormat.Trim());
                    else
                        tbToDate.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());

                    //date time for other report
                    if (Session[Constants.sessionFromDate] != null && Session[Constants.sessionFromDate] is DateTime)
                        tbFromDate1.Text = ((DateTime)Session[Constants.sessionFromDate]).ToString(Constants.dateFormat.Trim());
                    else
                        tbFromDate1.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());

                    if (Session[Constants.sessionToDate] != null && Session[Constants.sessionToDate] is DateTime)
                        tbToDate1.Text = ((DateTime)Session[Constants.sessionToDate]).ToString(Constants.dateFormat.Trim());
                    else
                        tbToDate1.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());
                    if (Session[Constants.sessionToDate] != null && Session[Constants.sessionToDate] is DateTime)
                        tbToDate_TreciIzvestaj.Text = ((DateTime)Session[Constants.sessionToDate]).ToString(Constants.dateFormat.Trim());
                    else
                        tbToDate_TreciIzvestaj.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());
                    if (Session[Constants.sessionFromDate] != null && Session[Constants.sessionFromDate] is DateTime)
                        tbFromDate_TreciIzvestaj.Text = ((DateTime)Session[Constants.sessionToDate]).ToString(Constants.dateFormat.Trim());
                    else
                        tbFromDate_TreciIzvestaj.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());
                    setLanguage();

                    rbDaily.Checked = true;
                    rbDaily_OnCheckedChanged(this, new EventArgs());

                    //Table8.Visible = false;

                    EmployeeTO Empl = getEmployee();
                    EmployeeAsco4 emplAsco4 = new EmployeeAsco4(Session[Constants.sessionConnection]);
                    emplAsco4.EmplAsco4TO.EmployeeID = Empl.EmployeeID;
                    List<EmployeeAsco4TO> emplAsco4List = emplAsco4.Search();

                    lblTo.Visible = btnTo.Visible = tbToDate.Visible = rbPrevWeek.Checked;

                    if (emplAsco4List.Count == 1)
                    {
                        mail = emplAsco4List[0].NVarcharValue3;
                        user_id = emplAsco4List[0].NVarcharValue5;
                    }
                    if (user_id == "")
                    {
                        ApplUserTO user = (ApplUserTO)Session[Constants.sessionLogInUser];
                        user_id = user.UserID;

                    }
                    populateCompany(user_id);



                    if (mail == "")
                    {
                        lblError.Text = rm.GetString("lblNoMail", culture);
                        btnReport.Enabled = false;
                    }
                    if (lBoxCompany.Items.Count == 0)
                        btnReport.Enabled = false;

                    int defaultWUID = -1;
                    if (Session[Constants.sessionLogInEmployee] != null && Session[Constants.sessionLogInEmployee] is EmployeeTO)
                    {
                        EmployeeAsco4 emplAsco = new EmployeeAsco4(Session[Constants.sessionConnection]);
                        emplAsco.EmplAsco4TO.EmployeeID = ((EmployeeTO)Session[Constants.sessionLogInEmployee]).EmployeeID;

                        List<EmployeeAsco4TO> emplAscoList = emplAsco.Search();

                        if (emplAscoList.Count == 1)
                        {
                            defaultWUID = emplAscoList[0].IntegerValue2;
                        }
                    }
                    populateWU(defaultWUID);
                    //if (Request.QueryString["reloadState"] != null && Request.QueryString["reloadState"].Trim().ToUpper().Equals(Constants.falseValue.Trim().ToUpper()))
                    //{

                    //}
                    //else                    
                    //    // reload selected filter state                        
                    //    LoadState();
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WorkAnalyzeReports.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WorkAnalyzeReports.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnPrevDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLLoansPage).Assembly);

                //check inserted date
                if (tbFromDate.Text.Trim().Equals("") || tbToDate.Text.Trim().Equals(""))
                {
                    lblError.Text = rm.GetString("noDateInterval", culture);

                    if (tbFromDate.Text.Trim().Equals(""))
                        tbFromDate.Focus();
                    else if (tbToDate.Text.Trim().Equals(""))
                        tbToDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                DateTime fromDate = CommonWeb.Misc.createDate(tbFromDate.Text.Trim());
                DateTime toDate = CommonWeb.Misc.createDate(tbToDate.Text.Trim());

                //date validation
                if (fromDate.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidFromDate", culture);
                    tbFromDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (toDate.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidToDate", culture);
                    tbToDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (fromDate > toDate)
                {
                    lblError.Text = rm.GetString("invalidFromToDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                tbFromDate.Text = fromDate.AddDays(-1).ToString(Constants.dateFormat);
                tbToDate.Text = toDate.AddDays(-1).ToString(Constants.dateFormat);

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WorkAnalyzeReports.btnPrevDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WorkAnalyzeReports.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
        protected void btnNextDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLLoansPage).Assembly);

                //check inserted date
                if (tbFromDate.Text.Trim().Equals("") || tbToDate.Text.Trim().Equals(""))
                {
                    lblError.Text = rm.GetString("noDateInterval", culture);

                    if (tbFromDate.Text.Trim().Equals(""))
                        tbFromDate.Focus();
                    else if (tbToDate.Text.Trim().Equals(""))
                        tbToDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                DateTime fromDate = CommonWeb.Misc.createDate(tbFromDate.Text.Trim());
                DateTime toDate = CommonWeb.Misc.createDate(tbToDate.Text.Trim());

                //date validation
                if (fromDate.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidFromDate", culture);
                    tbFromDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (toDate.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidToDate", culture);
                    tbToDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (fromDate > toDate)
                {
                    lblError.Text = rm.GetString("invalidFromToDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                tbFromDate.Text = fromDate.AddDays(1).ToString(Constants.dateFormat);
                tbToDate.Text = toDate.AddDays(1).ToString(Constants.dateFormat);

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {

                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WorkAnalyzeReports.btnPrevDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WorkAnalyzeReports.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnPrevDayPeriod1_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLLoansPage).Assembly);

                //check inserted date
                if (tbFromDate.Text.Trim().Equals("") || tbToDate.Text.Trim().Equals(""))
                {
                    lblError.Text = rm.GetString("noDateInterval", culture);

                    if (tbFromDate.Text.Trim().Equals(""))
                        tbFromDate.Focus();
                    else if (tbToDate.Text.Trim().Equals(""))
                        tbToDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                DateTime fromDate = CommonWeb.Misc.createDate(tbFromDate.Text.Trim());
                DateTime toDate = CommonWeb.Misc.createDate(tbToDate.Text.Trim());

                //date validation
                if (fromDate.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidFromDate", culture);
                    tbFromDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (toDate.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidToDate", culture);
                    tbToDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (fromDate > toDate)
                {
                    lblError.Text = rm.GetString("invalidFromToDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                tbFromDate.Text = fromDate.AddDays(-1).ToString(Constants.dateFormat);
                tbToDate.Text = toDate.AddDays(-1).ToString(Constants.dateFormat);

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WorkAnalyzeReports.btnPrevDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WorkAnalyzeReports.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
        protected void btnNextDayPeriod1_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLLoansPage).Assembly);

                //check inserted date
                if (tbFromDate.Text.Trim().Equals("") || tbToDate.Text.Trim().Equals(""))
                {
                    lblError.Text = rm.GetString("noDateInterval", culture);

                    if (tbFromDate.Text.Trim().Equals(""))
                        tbFromDate.Focus();
                    else if (tbToDate.Text.Trim().Equals(""))
                        tbToDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                DateTime fromDate = CommonWeb.Misc.createDate(tbFromDate.Text.Trim());
                DateTime toDate = CommonWeb.Misc.createDate(tbToDate.Text.Trim());

                //date validation
                if (fromDate.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidFromDate", culture);
                    tbFromDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (toDate.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidToDate", culture);
                    tbToDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (fromDate > toDate)
                {
                    lblError.Text = rm.GetString("invalidFromToDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                tbFromDate.Text = fromDate.AddDays(1).ToString(Constants.dateFormat);
                tbToDate.Text = toDate.AddDays(1).ToString(Constants.dateFormat);

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {

                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WorkAnalyzeReports.btnPrevDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WorkAnalyzeReports.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }


        private void populateCompany(string user_id)
        {

            Dictionary<int, WorkingUnitTO> listWU = new ApplUsersXWU(Session[Constants.sessionConnection]).FindWUForUserDictionary(user_id, Constants.EmployeesPurpose);

            List<WorkingUnitTO> listCompanies = new List<WorkingUnitTO>();
            foreach (int company in Enum.GetValues(typeof(Constants.FiatCompanies)))
            {
                if (listWU.ContainsKey(company))
                    listCompanies.Add(listWU[company]);
            }

            lBoxCompany.DataSource = listCompanies;
            lBoxCompany.DataTextField = "Description";
            lBoxCompany.DataValueField = "WorkingUnitID";
            lBoxCompany.DataBind();
        }

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WorkAnalyzeReports).Assembly);

                lblReportType.Text = rm.GetString("lblReportType", culture);
                lblPeriod.Text = lblPeriod1.Text = rm.GetString("lblPeriod", culture);
                lblTime.Text = rm.GetString("lblPeriod", culture);
                lblFrom.Text = lblFrom1.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = lblTo1.Text = rm.GetString("lblTo", culture);
                rbPrevMonth.Text = rm.GetString("rbPrevMonth", culture);
                rbPrevWeek.Text = rm.GetString("rbPrevWeek", culture);
                rbLast2Weeks.Text = rm.GetString("rbFromBeginOfMonth", culture);
                rbDaily.Text = rm.GetString("rbWorkAnalysis", culture);
                lblFrom.Text = rm.GetString("lblCycleDay", culture);
                btnReport.Text = btnReport1.Text = rm.GetString("btnReport", culture);
                lblCompany.Text = rm.GetString("company", culture);
                cbSelectAllWU.Text = rm.GetString("lblSelectAll", culture);
                lblReportSickName.Text = rm.GetString("lblReportSickName", culture);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        protected void rb400_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                rb500.Checked = !rb400.Checked;
                rbDaily.Checked = !rb400.Checked;
                rbPrevMonth.Checked = true;
                rbPrevWeek.Checked = rbLast2Weeks.Checked = false;
                Table8.Visible = !rb400.Checked;
                rbPrevMonth.Visible = rbPrevWeek.Visible = rbLast2Weeks.Visible = lblPeriod.Visible = rb400.Checked;
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WorkAnalyzeReports).Assembly);
                if (rb500.Checked || rb400.Checked)
                {
                    rbPrevMonth.Text = rm.GetString("rbPrevMonth", culture);
                    rbPrevWeek.Text = rm.GetString("rbPrevWeek", culture);
                    lblPeriod.Text = rm.GetString("lblPeriod", culture);
                }
                else if (rbDaily.Checked)
                {
                    rbPrevMonth.Text = rm.GetString("rbPresenceAndAbsence", culture);
                    rbPrevWeek.Text = rm.GetString("rbWageTypes", culture);
                    lblPeriod.Text = rm.GetString("hdrEmplType", culture);
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WorkAnalyzeReports.rb400_OnCheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WorkAnalyzeReports.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
        protected void rbPreviousMonth_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WorkAnalyzeReports).Assembly);
                lblError.Text = "";
                rbLast2Weeks.Checked = rbPrevWeek.Checked = !rbPrevMonth.Checked;
                lblTo.Visible = btnTo.Visible = tbToDate.Visible = !rbPrevMonth.Checked;

                lblFrom.Text = rm.GetString("lblCycleDay", culture);

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WorkAnalyzeReports.rb400_OnCheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WorkAnalyzeReports.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
        protected void rbPreviousWeek_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WorkAnalyzeReports).Assembly);
                lblError.Text = "";
                rbPrevMonth.Checked = rbLast2Weeks.Checked = !rbPrevWeek.Checked;
                //lblTo.Visible = btnToDate.Visible = tbToDate.Visible = rbPrevWeek.Checked;
                lblTo.Visible = btnTo.Visible = tbToDate.Visible = rbPrevWeek.Checked;
                lblFrom.Text = rm.GetString("lblFrom", culture);

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WorkAnalyzeReports.rb400_OnCheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WorkAnalyzeReports.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
        protected void rbPrevious2Weeks_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                rbPrevMonth.Checked = rbPrevWeek.Checked = !rbLast2Weeks.Checked;

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WorkAnalyzeReports.rb400_OnCheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WorkAnalyzeReports.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void sendMails(string mailTo, string attachments, EmployeeTO Empl)
        {
            try
            {
                if (mailTo != "")
                {
                    string host = "";
                    string emailAddress = "";
                    string userName = "";
                    string password = "";
                    int port = 25;
                    if (ConfigurationManager.AppSettings["emailAddress"] != null)
                    {
                        emailAddress = (string)ConfigurationManager.AppSettings["emailAddress"];
                    }
                    if (ConfigurationManager.AppSettings["host"] != null)
                    {
                        host = (string)ConfigurationManager.AppSettings["host"];
                    }

                    if (ConfigurationManager.AppSettings["port"] != null)
                    {
                        port = int.Parse((string)ConfigurationManager.AppSettings["port"]);
                    }

                    if (ConfigurationManager.AppSettings["userName"] != null)
                    {
                        userName = (string)ConfigurationManager.AppSettings["userName"];
                    }

                    if (ConfigurationManager.AppSettings["password"] != null)
                    {
                        password = (string)ConfigurationManager.AppSettings["password"];
                    }


                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(host, port);


                    string mailFrom = emailAddress;
                    smtp.Credentials = new NetworkCredential(userName, password);
                    CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                    ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WorkAnalyzeReports).Assembly);
                    System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                    string message = "";
                    message += "<html><body>" + rm.GetString("lblDear", culture) + " <b>" + Empl.FirstAndLastName + ", </b><br /><br />";
                    message += rm.GetString("messageWorkAnalysis", culture) + " <br /><br /></body></html>";
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Body = message;


                    mailMessage.To.Add(mailTo);
                    mailMessage.Subject = rm.GetString("lblReport", culture) + " " + DateTime.Now.ToString("dd.MM.yyyy");
                    mailMessage.From = new System.Net.Mail.MailAddress(mailFrom);
                    if (attachments.Length > 0)
                    {
                        foreach (string attachment in attachments.Split(','))
                        {
                            Attachment data = new Attachment(attachment, MediaTypeNames.Application.Octet);
                            mailMessage.Attachments.Add(data);
                        }
                    }

                    try
                    {
                        smtp.Send(mailMessage);
                        lblError.Text = rm.GetString("lblFinished", culture) + " " + rm.GetString("lblSentToMail", culture) + mailTo;
                    }
                    catch (Exception tex)
                    {

                        throw tex;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        protected void rbDaily_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                rb400.Checked = !rbDaily.Checked;
                rb500.Checked = !rbDaily.Checked;
                rbPrevMonth.Checked = true;
                rbPrevWeek.Checked = rbLast2Weeks.Checked = false;
                //rb500.Checked = false;
                Table8.Visible = rbDaily.Checked;
                rbPrevMonth.Visible = rbPrevWeek.Visible = lblPeriod.Visible = rbDaily.Checked;
                rbLast2Weeks.Visible = !rbDaily.Checked;
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WorkAnalyzeReports).Assembly);
                if (rbDaily.Checked)
                {
                    rbPrevMonth.Text = rm.GetString("rbPresenceAndAbsence", culture);
                    rbPrevWeek.Text = rm.GetString("rbWageTypes", culture);
                    lblPeriod.Text = rm.GetString("hdrEmplType", culture);
                }
                else
                {
                    rbPrevMonth.Text = rm.GetString("rbPrevMonth", culture);
                    rbPrevWeek.Text = rm.GetString("rbPrevWeek", culture);
                    lblPeriod.Text = rm.GetString("lblPeriod", culture);
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WorkAnalyzeReports.rb400_OnCheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WorkAnalyzeReports.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void rb500_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                rb400.Checked = !rb500.Checked;
                rbDaily.Checked = !rb500.Checked;
                rbPrevMonth.Checked = true;
                rbPrevWeek.Checked = rbLast2Weeks.Checked = false;
                //rbDaily.Checked = false;
                //rbPrevMonth.Visible = rbPrevWeek.Visible = rbLast2Weeks.Visible = lblPeriod.Visible = rb500.Checked; 
                Table8.Visible = !rb500.Checked;
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WorkAnalyzeReports).Assembly);
                if (rb500.Checked || rb400.Checked)
                {
                    rbPrevMonth.Text = rm.GetString("rbPrevMonth", culture);
                    rbPrevWeek.Text = rm.GetString("rbPrevWeek", culture);
                    lblPeriod.Text = rm.GetString("lblPeriod", culture);
                }
                else if (rbDaily.Checked)
                {
                    rbPrevMonth.Text = rm.GetString("rbPresenceAndAbsence", culture);
                    rbPrevWeek.Text = rm.GetString("rbWageTypes", culture);
                    lblPeriod.Text = rm.GetString("hdrEmplType", culture);
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WorkAnalyzeReports.rb500_OnCheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WorkAnalyzeReports.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private List<DateTime> returnPreviousWeek()
        {
            int numOfDaysPrevious = returnMonthDays(DateTime.Now.Month, true);
            List<DateTime> datesList = new List<DateTime>();
            DateTime day = new DateTime();
            DateTime dayOne = new DateTime();
            int num = 0;
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            {
                if (DateTime.Now.Day - 7 <= 0)
                {
                    num = 7 - DateTime.Now.Day;
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, numOfDaysPrevious - num, 0, 0, 0);
                }
                else
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 7, 0, 0, 0);


                dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                while (day < dayOne)
                {
                    datesList.Add(day);
                    day = day.AddDays(1);
                }
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
            {
                if (DateTime.Now.Day - 8 <= 0)
                {
                    num = 8 - DateTime.Now.Day;
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, numOfDaysPrevious - num, 0, 0, 0);
                }
                else
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 8, 0, 0, 0);

                if (DateTime.Now.Day - 1 <= 0)
                {
                    num = 1 - DateTime.Now.Day;
                    dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, numOfDaysPrevious - num, 0, 0, 0);
                }
                else
                    dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1, 0, 0, 0);
                while (day < dayOne)
                {
                    datesList.Add(day);
                    day = day.AddDays(1);
                }
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Wednesday)
            {
                if (DateTime.Now.Day - 9 <= 0)
                {
                    num = 9 - DateTime.Now.Day;
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, numOfDaysPrevious - num, 0, 0, 0);
                }
                else
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 9, 0, 0, 0);

                if (DateTime.Now.Day - 2 <= 0)
                {
                    num = 2 - DateTime.Now.Day;
                    dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, numOfDaysPrevious - num, 0, 0, 0);
                }
                else
                    dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 2, 0, 0, 0);
                while (day < dayOne)
                {
                    datesList.Add(day);
                    day = day.AddDays(1);
                }
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
            {
                if (DateTime.Now.Day - 10 <= 0)
                {
                    num = 10 - DateTime.Now.Day;
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, numOfDaysPrevious - num, 0, 0, 0);
                }
                else
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 10, 0, 0, 0);

                if (DateTime.Now.Day - 3 <= 0)
                {
                    num = 3 - DateTime.Now.Day;
                    dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, numOfDaysPrevious - num, 0, 0, 0);
                }
                else
                    dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 3, 0, 0, 0);
                while (day < dayOne)
                {
                    datesList.Add(day);
                    day = day.AddDays(1);
                }
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                if (DateTime.Now.Day - 11 <= 0)
                {
                    num = 11 - DateTime.Now.Day;
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, numOfDaysPrevious - num, 0, 0, 0);
                }
                else
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 11, 0, 0, 0);

                if (DateTime.Now.Day - 4 <= 0)
                {
                    num = 4 - DateTime.Now.Day;
                    dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, numOfDaysPrevious - num, 0, 0, 0);
                }
                else
                    dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 4, 0, 0, 0);
                while (day < dayOne)
                {
                    datesList.Add(day);
                    day = day.AddDays(1);
                }
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
            {
                if (DateTime.Now.Day - 12 <= 0)
                {
                    num = 12 - DateTime.Now.Day;
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, numOfDaysPrevious - num, 0, 0, 0);
                }
                else
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 12, 0, 0, 0);
                if (DateTime.Now.Day - 5 <= 0)
                {
                    num = 5 - DateTime.Now.Day;
                    dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, numOfDaysPrevious - num, 0, 0, 0);
                }
                else
                    dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 5, 0, 0, 0);
                while (day < dayOne)
                {
                    datesList.Add(day);
                    day = day.AddDays(1);
                }
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                if (DateTime.Now.Day - 13 <= 0)
                {
                    num = 13 - DateTime.Now.Day;
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, numOfDaysPrevious - num, 0, 0, 0);
                }
                else
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 13, 0, 0, 0);
                if (DateTime.Now.Day - 6 <= 0)
                {
                    num = 6 - DateTime.Now.Day;
                    dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, numOfDaysPrevious - num, 0, 0, 0);
                }
                else
                    dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 6, 0, 0, 0);
                while (day < dayOne)
                {
                    datesList.Add(day);
                    day = day.AddDays(1);
                }
            }
            return datesList;
        }


        protected void lBoxWU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int[] selWUIndex = lBoxWU.GetSelectedIndices();
                if (selWUIndex.Length > 0)
                {
                    int wuID = -1;

                    if (int.TryParse(lBoxWU.Items[selWUIndex[0]].Value.Trim(), out wuID))
                    {
                        Session[Constants.sessionWU] = wuID;
                        Session[Constants.sessionOU] = null;
                    }
                }

                Session[Constants.sessionSamePage] = true;
                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WorkAnalyzeReports.lBoxWU_SelectedIndexChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLDetailedDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void cbSelectAllWU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem item in lBoxWU.Items)
                {
                    item.Selected = cbSelectAllWU.Checked;
                }

                int[] selWUIndex = lBoxWU.GetSelectedIndices();
                if (selWUIndex.Length > 0)
                {
                    int wuID = -1;

                    if (int.TryParse(lBoxWU.Items[selWUIndex[0]].Value.Trim(), out wuID))
                    {
                        Session[Constants.sessionWU] = wuID;
                        Session[Constants.sessionOU] = null;
                    }
                }

                Session[Constants.sessionSamePage] = true;

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WorkAnalyzeReports.cbSelectAllWU_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLDetailedDataPage.aspx".Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }



        private void populateWU(int wuID)
        {
            try
            {
                if (Session[Constants.sessionLoginCategoryWUnits] != null && Session[Constants.sessionLoginCategoryWUnits] is List<int>)
                {
                    List<WorkingUnitTO> workingUnitsList = new WorkingUnit(Session[Constants.sessionConnection]).Search();
                    List<WorkingUnitTO> WUList = new List<WorkingUnitTO>();

                    foreach (WorkingUnitTO wu in workingUnitsList)
                    {
                        if (((List<int>)Session[Constants.sessionLoginCategoryWUnits]).Contains(wu.WorkingUnitID))
                            WUList.Add(wu);
                    }

                    lBoxWU.DataSource = WUList;
                    lbWU_TreciIzvestaj.DataSource = WUList;
                    lbWU_TreciIzvestaj.DataTextField = "Name";
                    lbWU_TreciIzvestaj.DataValueField = "WorkingUnitID";
                    lBoxWU.DataTextField = "Name";
                    lBoxWU.DataValueField = "WorkingUnitID";
                    lBoxWU.DataBind();
                    lbWU_TreciIzvestaj.DataBind();

                    foreach (ListItem wItem in lBoxWU.Items)
                    {
                        if (wItem.Value.Trim().Equals(wuID.ToString().Trim()))
                        {
                            wItem.Selected = true;
                            break;
                        }
                    }
                    foreach (ListItem wItem in lbWU_TreciIzvestaj.Items)
                    {
                        if (wItem.Value.Trim().Equals(wuID.ToString().Trim()))
                        {
                            wItem.Selected = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ReadAppSettings(string emailAddress, string Host, int port, string userName, string password)
        {
            if (ConfigurationManager.AppSettings["emailAddress"] != null)
            {
                emailAddress = (string)ConfigurationManager.AppSettings["emailAddress"];
            }
            if (ConfigurationManager.AppSettings["host"] != null)
            {
                Host = (string)ConfigurationManager.AppSettings["host"];
            }

            if (ConfigurationManager.AppSettings["port"] != null)
            {
                port = int.Parse((string)ConfigurationManager.AppSettings["port"]);
            }

            if (ConfigurationManager.AppSettings["userName"] != null)
            {
                userName = (string)ConfigurationManager.AppSettings["userName"];
            }

            if (ConfigurationManager.AppSettings["password"] != null)
            {
                password = (string)ConfigurationManager.AppSettings["password"];
            }
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (lBoxCompany.Items.Count == 0)
                {
                    writeLog(DateTime.Now, false);
                    return;
                }
                ApplUserTO user = (ApplUserTO)Session[Constants.sessionLogInUser];
                string tempFolder = Constants.logFilePath + "Temp";
                string[] dirs = Directory.GetDirectories(tempFolder);
                foreach (string direct in dirs)
                {
                    DateTime creation = Directory.GetCreationTime(direct);
                    TimeSpan create = creation.AddHours(1).TimeOfDay;

                    if (creation.Date < DateTime.Now.Date || create < DateTime.Now.TimeOfDay)
                        Directory.Delete(direct, true);
                }

                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WorkAnalyzeReports).Assembly);
                string mail = "";
                int[] selectedCompanies = lBoxCompany.GetSelectedIndices();
                if (selectedCompanies.Length <= 0)
                {
                    lblError.Text = rm.GetString("mustSelectCompany", culture);
                    return;
                }
                string costumer = Common.Misc.getCustomer(Session[Constants.sessionConnection]);
                int cost = 0;
                bool costum = int.TryParse(costumer, out cost);
                bool isFiat = (cost == (int)Constants.Customers.FIAT);


                if (rbDaily.Checked)
                {
                    DateTime fromDate = CommonWeb.Misc.createDate(tbFromDate.Text.Trim());
                    DateTime toDate = CommonWeb.Misc.createDate(tbToDate.Text.Trim());

                    DateTime from = new DateTime();
                    DateTime to = new DateTime();

                    //date validation
                    if (fromDate.Equals(new DateTime()))
                    {
                        lblError.Text = rm.GetString("invalidFromDate", culture);
                        writeLog(DateTime.Now, false);
                        return;
                    }

                    if (toDate.Equals(new DateTime()))
                    {
                        lblError.Text = rm.GetString("invalidToDate", culture);
                        writeLog(DateTime.Now, false);
                        return;
                    }

                    from = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 0, 0, 0);
                    to = new DateTime(toDate.Year, toDate.Month, toDate.Day, 0, 0, 0);

                    if (from > to)
                    {
                        lblError.Text = rm.GetString("invalidFromToDate", culture);
                        writeLog(DateTime.Now, false);
                        return;
                    }
                }
                EmployeeTO Empl = getEmployee();
                EmployeeAsco4 emplAsco4 = new EmployeeAsco4(Session[Constants.sessionConnection]);
                emplAsco4.EmplAsco4TO.EmployeeID = Empl.EmployeeID;
                List<EmployeeAsco4TO> emplAsco4List = emplAsco4.Search();

                if (emplAsco4List.Count == 1)
                {
                    mail = emplAsco4List[0].NVarcharValue3;
                }

                lblError.Text = "";
                int daysInMonth = returnMonthDays(DateTime.Now.Month, true);

                Dictionary<int, List<WorkingUnitTO>> dictionaryCompanies = new Dictionary<int, List<WorkingUnitTO>>();
                Dictionary<int, string> dictionaryComaniesNames = new Dictionary<int, string>();
                if (isFiat)
                {
                    foreach (int comp in Enum.GetValues(typeof(Constants.FiatCompanies)))
                    {
                        List<WorkingUnitTO> listicaFAS = new WorkingUnit(Session[Constants.sessionConnection]).SearchChildWU(comp.ToString());
                        dictionaryCompanies.Add(comp, listicaFAS);
                        string name = Enum.GetName(typeof(Constants.FiatCompanies), comp);
                        dictionaryComaniesNames.Add(comp, name);
                    }
                }
                else
                {
                    foreach (int index in selectedCompanies)
                    {
                        int comp = int.Parse(lBoxCompany.Items[index].Value);
                        List<WorkingUnitTO> lista = new WorkingUnit(Session[Constants.sessionConnection]).SearchChildWU(comp.ToString());
                        dictionaryCompanies.Add(comp, lista);
                        WorkingUnitTO wu = new WorkingUnit(Session[Constants.sessionConnection]).FindWU(comp);
                        dictionaryComaniesNames.Add(comp, wu.Name);
                    }
                }

                WorkAnalysis managerInstance = WorkAnalysis.GetInstance(false);
                try
                {

                    List<DateTime> datesList = new List<DateTime>();
                    DateTime day = DateTime.Now.Date;
                    day = day.AddMonths(-1);

                    DateTime dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                    while (day < dayOne)
                    {
                        datesList.Add(day);
                        day = day.AddDays(1);
                    }

                    string attachments = "";
                    if (rb400.Checked)
                    {
                        if (rbPrevMonth.Checked)
                        {

                            if (selectedCompanies.Length > 0)
                            {
                                foreach (int index in selectedCompanies)
                                {
                                    int company = int.Parse(lBoxCompany.Items[index].Value);

                                    List<WorkingUnitTO> listPlants = dictionaryCompanies[company];
                                    string filePath = Constants.logFilePath + "Temp\\" + user.UserID + "_" + DateTime.Now.ToString("mm.ss.ff") + "\\" + "400_";
                                    filePath += dictionaryComaniesNames[company];
                                    filePath += "_Month_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";

                                    string attach = managerInstance.generate400ReportNew(Session[Constants.sessionConnection], filePath, listPlants, datesList, company);
                                    if (attach != "")
                                        attachments += attach + ",";
                                }
                            }

                            //else
                            //{
                            //    foreach (KeyValuePair<int, List<WorkingUnitTO>> pairComany in dictionaryCompanies)
                            //    {
                            //        int company = pairComany.Key;

                            //        List<WorkingUnitTO> listPlants = dictionaryCompanies[company];
                            //        string filePath = Constants.logFilePath + "Temp\\" + user.UserID + "_" +DateTime.Now.ToString("mm.ss.ff") + "\\" + "400_" +dictionaryComaniesNames[company] + "_Month_" +
                            //              DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";

                            //        string attach = managerInstance.generate400ReportNew(Session[Constants.sessionConnection], filePath, listPlants, datesList, company);
                            //        if (attach != "")
                            //            attachments += attach + ",";
                            //    }
                            //}

                            if (attachments.Length > 0)
                            {
                                attachments = attachments.Remove(attachments.LastIndexOf(','));

                            }
                        }
                        else if (rbPrevWeek.Checked)
                        {
                            datesList = returnPreviousWeek();
                            if (selectedCompanies.Length > 0)
                            {
                                foreach (int index in selectedCompanies)
                                {
                                    int company = int.Parse(lBoxCompany.Items[index].Value);
                                    List<WorkingUnitTO> listPlants = dictionaryCompanies[company];

                                    string filePath = Constants.logFilePath + "Temp\\" + user.UserID + "_" + DateTime.Now.ToString("mm.ss.ff") + "\\" + "400_";

                                    filePath += dictionaryComaniesNames[company];

                                    filePath += "_Week_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";
                                    string attach = managerInstance.generate400ReportNew(Session[Constants.sessionConnection], filePath, listPlants, datesList, company);
                                    if (attach != "")
                                        attachments += attach + ",";

                                }
                            }
                            //else
                            //{
                            //    foreach (KeyValuePair<int, List<WorkingUnitTO>> pairComany in dictionaryCompanies)
                            //    {
                            //        int company = pairComany.Key;

                            //        List<WorkingUnitTO> listPlants = pairComany.Value;

                            //        string filePath = Constants.logFilePath + "Temp\\" + user.UserID + "_" + DateTime.Now.ToString("mm.ss.ff") + "\\" + "400_" + dictionaryComaniesNames[company] + "_Week_" +
                            //              DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";
                            //        string attach = managerInstance.generate400ReportNew(Session[Constants.sessionConnection], filePath, listPlants, datesList, company);
                            //        if (attach != "")
                            //            attachments += attach + ",";
                            //    }
                            //}

                            if (attachments.Length > 0)
                            {
                                attachments = attachments.Remove(attachments.LastIndexOf(','));
                            }
                        }
                        else if (rbLast2Weeks.Checked)
                        {

                            datesList = new List<DateTime>();
                            day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                            dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                            while (day < dayOne)
                            {
                                datesList.Add(day);
                                day = day.AddDays(1);
                            }
                            if (selectedCompanies.Length > 0)
                            {
                                foreach (int index in selectedCompanies)
                                {
                                    int company = int.Parse(lBoxCompany.Items[index].Value);

                                    List<WorkingUnitTO> listPlants = dictionaryCompanies[company];
                                    string filePath = Constants.logFilePath + "Temp\\" + user.UserID + "_" + DateTime.Now.ToString("mm.ss.ff") + "\\" + "400_";

                                    filePath += dictionaryComaniesNames[company];

                                    filePath += "_Week_agregate_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";
                                    string attach = managerInstance.generate400ReportNew(Session[Constants.sessionConnection], filePath, listPlants, datesList, company);
                                    if (attach != "")
                                        attachments += attach + ",";
                                }
                            }
                            //else
                            //{
                            //    foreach (KeyValuePair<int, List<WorkingUnitTO>> pairComany in dictionaryCompanies)
                            //    {
                            //        int company = pairComany.Key;

                            //        List<WorkingUnitTO> listPlants = pairComany.Value;
                            //        string filePath = Constants.logFilePath + "Temp\\" + user.UserID + "_" + DateTime.Now.ToString("mm.ss.ff") + "\\" + "500_" + dictionaryComaniesNames[company] + "_Week_Agregate_" +
                            //               DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";
                            //        string attach = managerInstance.generate400ReportNew(Session[Constants.sessionConnection], filePath, listPlants, datesList, company);
                            //        if (attach != "")
                            //            attachments += attach + ",";

                            //    }
                            //}

                            if (attachments.Length > 0)
                            {
                                attachments = attachments.Remove(attachments.LastIndexOf(','));
                            }
                        }
                    }
                    else if (rb500.Checked)
                    {
                        if (rbPrevMonth.Checked)
                        {
                            if (selectedCompanies.Length > 0)
                            {
                                foreach (int index in selectedCompanies)
                                {
                                    int company = int.Parse(lBoxCompany.Items[index].Value);

                                    List<WorkingUnitTO> listPlants = dictionaryCompanies[company];

                                    string filePath = Constants.logFilePath + "Temp\\" + user.UserID + "_" + DateTime.Now.ToString("mm.ss.ff") + "\\" + "500_";

                                    filePath += dictionaryComaniesNames[company];

                                    filePath += "_Month_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";
                                    string attach = managerInstance.generate500ReportNew(Session[Constants.sessionConnection], datesList, filePath, listPlants, company);
                                    if (attach != "")
                                        attachments += attach + ",";
                                }
                            }
                            //else
                            //{
                            //    foreach (KeyValuePair<int, List<WorkingUnitTO>> pairComany in dictionaryCompanies)
                            //    {
                            //        int company = pairComany.Key;

                            //        List<WorkingUnitTO> listPlants = pairComany.Value;
                            //        string filePath = Constants.logFilePath + "Temp\\" + user.UserID + "_" + DateTime.Now.ToString("mm.ss.ff") + "\\" + "500_" + dictionaryComaniesNames[company] + "_Month_" +
                            //             DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";
                            //        string attach = managerInstance.generate500ReportNew(Session[Constants.sessionConnection], datesList, filePath, listPlants, company);
                            //        if (attach != "")
                            //            attachments += attach + ",";
                            //    }
                            //}
                            if (attachments.Length > 0)
                            {
                                attachments = attachments.Remove(attachments.LastIndexOf(','));
                            }
                        }
                        else if (rbPrevWeek.Checked)
                        {
                            datesList = returnPreviousWeek();
                            if (selectedCompanies.Length > 0)
                            {
                                foreach (int index in selectedCompanies)
                                {
                                    int company = int.Parse(lBoxCompany.Items[index].Value);
                                    List<WorkingUnitTO> listPlants = dictionaryCompanies[company];
                                    string filePath = Constants.logFilePath + "Temp\\" + user.UserID + "_" + DateTime.Now.ToString("mm.ss.ff") + "\\" + "500_";

                                    filePath += dictionaryComaniesNames[company];
                                    filePath += "_Week_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";
                                    string attach = managerInstance.generate500ReportNew(Session[Constants.sessionConnection], datesList, filePath, listPlants, company);
                                    if (attach != "")
                                        attachments += attach + ",";
                                }
                            }
                            //else
                            //{
                            //    foreach (KeyValuePair<int, List<WorkingUnitTO>> pairComany in dictionaryCompanies)
                            //    {
                            //        int company = pairComany.Key;

                            //        List<WorkingUnitTO> listPlants = pairComany.Value;
                            //        string filePath = Constants.logFilePath + "Temp\\" + user.UserID + "_" + DateTime.Now.ToString("mm.ss.ff") + "\\" + "500_" + dictionaryComaniesNames[company] + "_Week_" +
                            //            DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";
                            //        string attach = managerInstance.generate500ReportNew(Session[Constants.sessionConnection], datesList, filePath, listPlants, company);
                            //        if (attach != "")
                            //            attachments += attach + ",";
                            //    }
                            //}
                            if (attachments.Length > 0)
                            {
                                attachments = attachments.Remove(attachments.LastIndexOf(','));
                            }
                        }
                        else if (rbLast2Weeks.Checked)
                        {
                            datesList = new List<DateTime>();
                            day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                            dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                            while (day < dayOne)
                            {
                                datesList.Add(day);
                                day = day.AddDays(1);
                            }
                            if (selectedCompanies.Length > 0)
                            {
                                foreach (int index in selectedCompanies)
                                {
                                    int company = int.Parse(lBoxCompany.Items[index].Value);

                                    List<WorkingUnitTO> listPlants = dictionaryCompanies[company];

                                    string filePath = Constants.logFilePath + "Temp\\" + user.UserID + "_" + DateTime.Now.ToString("mm.ss.ff") + "\\" + "500_";

                                    filePath += dictionaryComaniesNames[company];

                                    filePath += "_Week_Agregate_" +
                                         DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";
                                    string attach = managerInstance.generate500ReportNew(Session[Constants.sessionConnection], datesList, filePath, listPlants, company);
                                    if (attach != "")
                                        attachments += attach + ",";
                                }
                            }
                            //else
                            //{
                            //    foreach (KeyValuePair<int, List<WorkingUnitTO>> pairComany in dictionaryCompanies)
                            //    {
                            //        int company = pairComany.Key;

                            //        List<WorkingUnitTO> listPlants = pairComany.Value;
                            //        string filePath = Constants.logFilePath + "Temp\\" + user.UserID + "_" + DateTime.Now.ToString("mm.ss.ff") + "\\" + "500_" + dictionaryComaniesNames[company] + "_Week_Agregate_" +
                            //               DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";
                            //        string attach = managerInstance.generate500ReportNew(Session[Constants.sessionConnection], datesList, filePath, listPlants, company);
                            //        if (attach != "")
                            //            attachments += attach + ",";
                            //    }
                            //}
                            if (attachments.Length > 0)
                            {
                                attachments = attachments.Remove(attachments.LastIndexOf(','));
                            }
                        }
                    }
                    else
                    {
                        bool isAltLang = false;
                        if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                            isAltLang = false;
                        else
                            isAltLang = true;

                        if (rbPrevMonth.Checked)
                        {
                            DateTime oneDay = CommonWeb.Misc.createDate(tbFromDate.Text.Trim());
                            if (selectedCompanies.Length > 0)
                            {
                                foreach (int index in selectedCompanies)
                                {
                                    int company = int.Parse(lBoxCompany.Items[index].Value);

                                    List<WorkingUnitTO> listPlants = dictionaryCompanies[company];

                                    string filePath = Constants.logFilePath + "Temp\\" + user.UserID + "_" + DateTime.Now.ToString("mm.ss.ff") + "\\" + "P&A_";

                                    filePath += dictionaryComaniesNames[company];

                                    filePath += "_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";
                                    string attach = managerInstance.generatePAReportNew(Session[Constants.sessionConnection], filePath, listPlants, oneDay, company, isAltLang);
                                    if (attach != "")
                                        attachments += attach + ",";
                                }
                            }
                            //else
                            //{
                            //    foreach (KeyValuePair<int, List<WorkingUnitTO>> pairComany in dictionaryCompanies)
                            //    {
                            //        int company = pairComany.Key;

                            //        List<WorkingUnitTO> listPlants = pairComany.Value;
                            //        string filePath = Constants.logFilePath + "Temp\\" + user.UserID + "_" + DateTime.Now.ToString("mm.ss.ff") + "\\" + "P&A_" + dictionaryComaniesNames[company] + "_" +
                            // DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";
                            //        string attach = managerInstance.generatePAReportNew(Session[Constants.sessionConnection], filePath, listPlants, oneDay, company, isAltLang);
                            //        if (attach != "")
                            //            attachments += attach + ",";
                            //    }
                            //}
                            if (attachments.Length > 0)
                            {
                                attachments = attachments.Remove(attachments.LastIndexOf(','));
                            }
                        }
                        else if (rbPrevWeek.Checked)
                        {
                            datesList = new List<DateTime>();
                            day = CommonWeb.Misc.createDate(tbFromDate.Text);
                            dayOne = CommonWeb.Misc.createDate(tbToDate.Text);

                            while (day <= dayOne)
                            {
                                datesList.Add(day);
                                day = day.AddDays(1);
                            }

                            if (selectedCompanies.Length > 0)
                            {
                                foreach (int index in selectedCompanies)
                                {
                                    int company = int.Parse(lBoxCompany.Items[index].Value);

                                    List<WorkingUnitTO> listPlants = dictionaryCompanies[company];
                                    string filePath = Constants.logFilePath + "Temp\\" + user.UserID + "_" + DateTime.Now.ToString("mm.ss.ff") + "\\" + "Wage_Type_";

                                    filePath += dictionaryComaniesNames[company];
                                    filePath += "_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";
                                    string attach = managerInstance.generateWageTypesReportNew(Session[Constants.sessionConnection], filePath, listPlants, company, isAltLang, datesList);
                                    if (attach != "")
                                        attachments += attach + ",";
                                }
                            }
                            //else
                            //{
                            //    foreach (KeyValuePair<int, List<WorkingUnitTO>> pairComany in dictionaryCompanies)
                            //    {

                            //        int company = pairComany.Key;

                            //        List<WorkingUnitTO> listPlants = pairComany.Value;
                            //        string filePath = Constants.logFilePath + "Temp\\" + user.UserID + "_" + DateTime.Now.ToString("mm.ss.ff") + "\\" + "Wage_Type_" + dictionaryComaniesNames[company] + "_" +
                            //DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";
                            //        string attach = managerInstance.generateWageTypesReportNew(Session[Constants.sessionConnection], filePath, listPlants, company, isAltLang, datesList);
                            //        if (attach != "")
                            //            attachments += attach + ",";
                            //    }
                            //}

                            if (attachments.Length > 0)
                            {
                                attachments = attachments.Remove(attachments.LastIndexOf(','));

                            }
                        }
                    }

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    if (attachments.Length > 0)
                    {
                        sendMails(mail, attachments, Empl);
                    }

                    //ReadPdfFileAttach(attachments);
                    //lblError.Text = rm.GetString("lblFinished", culture) + " " + rm.GetString("lblSentToMail", culture) + mail;
                }
                catch (Exception ex)
                {

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WorkAnalyzeReports.btnReport_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WorkAnalyzeReports.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void ReadPdfFileAttach(string path)
        {
            string name = Path.GetFileName(path);
            WebClient client = new WebClient();
            Byte[] buffer = client.DownloadData(path);

            if (buffer != null)
            {
                Response.ContentType = "application/xlsx";
                Response.Clear();
                Response.AppendHeader("application/download", "attachment;Filename=" + name);
                Response.TransmitFile(path);
                Response.Flush();
                //Response.End();
            }
        }

        private EmployeeTO getEmployee()
        {
            try
            {
                EmployeeTO emplTO = new EmployeeTO();
                int emplID = -1;

                if (Request.QueryString["emplID"] != null)
                {
                    if (!int.TryParse(Request.QueryString["emplID"], out emplID))
                        emplID = -1;
                }

                if (emplID != -1)
                {
                    emplTO = new Employee(Session[Constants.sessionConnection]).Find(emplID.ToString().Trim());
                }

                return emplTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int returnMonthDays(int month, bool prev)
        {
            int i = 0;
            if (prev)
            {

                DateTime date = new DateTime();
                if (month - 1 == 0)
                    date = new DateTime(DateTime.Now.Year - 1, 12, 1, 0, 0, 0);
                else
                    date = new DateTime(DateTime.Now.Year, month - 1, 1, 0, 0, 0);
                DateTime now = new DateTime(DateTime.Now.Year, month, 1, 0, 0, 0);
                while (date < now)
                {
                    i++;
                    date = date.AddDays(1);
                }
            }
            else
            {
                DateTime date = new DateTime();
                if (month + 1 == 13)
                    date = new DateTime(DateTime.Now.Year + 1, 1, 1, 0, 0, 0);
                else
                    date = new DateTime(DateTime.Now.Year, month, 1, 0, 0, 0);
                DateTime now = new DateTime(DateTime.Now.Year, month + 1, 1, 0, 0, 0);
                while (date < now)
                {
                    i++;
                    date = date.AddDays(1);
                }
            }
            return i;
        }

        private List<IOPairProcessedTO> listaProlazakaZaRadnike(List<EmployeeTO> listaRadnikaIzWU, DateTime dtpFrom, DateTime dtpTo)
        {
            List<IOPairProcessedTO> listaParova = new List<IOPairProcessedTO>();
            if (dtpFrom > dtpTo)
            {
                lblError.Text = "Neispravan interval datuma!";
                return new List<IOPairProcessedTO>();
            }
            string emplIDs = "";
            foreach (EmployeeTO item in listaRadnikaIzWU)
            {
                emplIDs += item.EmployeeID + ",";
            }
            if (emplIDs.Length > 0)
            {
                emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
            }
            return listaParova = new Common.IOPairProcessed().pairsForPeriod(emplIDs, dtpFrom, dtpTo);
        }

        private int brSatiZaInterval(DateTime from, DateTime to, List<EmployeeTO> listaRadnikaZaWU)
        {
            int brSati = 0;
            Dictionary<int, WorkTimeSchemaTO> dictTimeSchema = new Common.TimeSchema().getDictionary();
            //Dictionary<int, List<TransferObjects.EmployeeTimeScheduleTO>> EmplTimeSchemas = new Common.EmployeesTimeSchedule().SearchEmployeesSchedulesDS;
            foreach (TransferObjects.EmployeeTO emplTO in listaRadnikaZaWU)
            {
                Dictionary<int, List<EmployeeTimeScheduleTO>> EmplTimeSchemas = new Common.EmployeesTimeSchedule().SearchEmployeesSchedulesDS(emplTO.EmployeeID.ToString().Trim(), from.Date, to.AddDays(1).Date);
                List<EmployeeTimeScheduleTO> emplTS = EmplTimeSchemas[emplTO.EmployeeID];
                DateTime currDate = from;
                while (currDate.Date <= to.Date)
                {
                    bool is2DayShift = false;
                    bool is2DayShiftPrevious = false;
                    WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                    WorkTimeSchemaTO workTimeSchema = null;
                    Dictionary<int, WorkTimeIntervalTO> edi = Common.Misc.getDayTimeSchemaIntervals(emplTS, currDate, ref is2DayShift,
                                ref is2DayShiftPrevious, ref firstIntervalNextDay, ref workTimeSchema, dictTimeSchema);
                    List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
                    TimeSpan intervalDuration = new TimeSpan();
                    if (edi == null)
                    {
                        currDate = currDate.AddDays(1);
                        continue;
                    }
                    intervals = Common.Misc.getEmployeeDayIntervals(is2DayShift, is2DayShiftPrevious, firstIntervalNextDay, edi);
                    foreach (WorkTimeIntervalTO interval in intervals)
                    {
                        intervalDuration += interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay;
                        if (interval.EndTime.Minute == 59)
                            intervalDuration = intervalDuration.Add(new TimeSpan(0, 1, 0));
                    }
                    brSati += intervalDuration.Hours;
                    currDate = currDate.AddDays(1);

                }
            }
            return brSati;
        }

        protected void btnTreciIzvestaj_Click(Object sender, EventArgs e)
        {
            btnTreciIzvestaj.Enabled = false;
            CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
            ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WorkAnalyzeReports).Assembly);
            Dictionary<int, WorkingUnitTO> wuDict = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
            DateTime from = CommonWeb.Misc.createDate(tbFromDate_TreciIzvestaj.Text.Trim());
            DateTime to = CommonWeb.Misc.createDate(tbToDate_TreciIzvestaj.Text.Trim());
            lblError.Text = "";
            if (from.Equals(new DateTime()))
            {
                lblError.Text = rm.GetString("invalidFromDate", culture);
                writeLog(DateTime.Now, false);
                return;
            }

            if (to.Equals(new DateTime()))
            {
                lblError.Text = rm.GetString("invalidToDate", culture);
                writeLog(DateTime.Now, false);
                return;
            }

            if (from > to)
            {
                lblError.Text = rm.GetString("invalidFromToDate", culture);
                writeLog(DateTime.Now, false);
                return;
            }
            if (lBoxWU.Items.Count == 0)
            {
                lblError.Text = rm.GetString("mustSelectWU", culture);
                writeLog(DateTime.Now, false);
                return;
            }
            string wuName = "";
            int[] selectedWU = lbWU_TreciIzvestaj.GetSelectedIndices();
            if (selectedWU.Length <= 0)
            {
                lblError.Text = rm.GetString("mustSelectWU", culture);
                return;
            }
            int rootWU = -1;
            foreach (int index in selectedWU)
            {
                wuName += "'"+lbWU_TreciIzvestaj.Items[index].Text+"'" + ",";
                int id = -1;
                if(int.TryParse(lbWU_TreciIzvestaj.Items[index].Value,out id))
                {
                    rootWU = Common.Misc.getRootWorkingUnit(id, wuDict);
                }
            }
            if (wuName.Length > 1)
            {
                wuName = wuName.Substring(0, wuName.Length - 1);
            }
            List<EmployeeTO> listaRadnikaZaWU = new Employee().listaRadnikaZaWU(wuName);
            if (listaRadnikaZaWU.Count <= 0 && selectedWU.Length==1)
            {
                lblError.Text = "Nema radnika za izabranu radnu jedinicu!";
                return;
            }
            else if (listaRadnikaZaWU.Count <= 0 && selectedWU.Length > 1)
            {
                lblError.Text="Nema radnika za izabrane radne jedinice!";
                return;
            }
            List<IOPairProcessedTO> pairs = listaProlazakaZaRadnike(listaRadnikaZaWU, from.Date, to.Date);
            if (pairs.Count < 1)
            {
                lblError.Text = "Nema prolazaka za zadate parametre";
                return;
            }
            int brojOcekivanihSati = brSatiZaInterval(from.Date, to.Date, listaRadnikaZaWU);
            Dictionary<int, TimeSpan> passType_duration = new Dictionary<int, TimeSpan>();
            foreach (IOPairProcessedTO pair in pairs)
            {
                if (pair.EndTime.Minute == 59)
                    pair.EndTime = pair.EndTime.AddMinutes(1);
                TimeSpan duration = pair.EndTime - pair.StartTime;
                if (!passType_duration.ContainsKey(pair.PassTypeID))
                    passType_duration.Add(pair.PassTypeID, new TimeSpan());
                passType_duration[pair.PassTypeID] += duration;
            }
            Dictionary<int, int> passType_numEmpl = new Dictionary<int, int>();
            Dictionary<int, List<int>> passType_empls = new Dictionary<int, List<int>>();
            foreach (IOPairProcessedTO pair in pairs)
            {
                if (!passType_empls.ContainsKey(pair.PassTypeID))
                    passType_empls.Add(pair.PassTypeID, new List<int>());
                if (!passType_empls[pair.PassTypeID].Contains(pair.EmployeeID))
                    passType_empls[pair.PassTypeID].Add(pair.EmployeeID);
                else
                    continue;
                if (!passType_numEmpl.ContainsKey(pair.PassTypeID))
                    passType_numEmpl.Add(pair.PassTypeID, 0);
                passType_numEmpl[pair.PassTypeID] += 1;
            }
            PopuniExcelSaPassTypes(passType_duration, passType_numEmpl, passType_empls, brojOcekivanihSati, listaRadnikaZaWU.Count, wuName, pairs);
            //lblError.Text = "Izvestaj je gotov i nalazi se u C:\\temp\\ folderu!";
            btnTreciIzvestaj.Enabled = true;
        }

        private void PopuniExcelSaPassTypes(Dictionary<int, TimeSpan> passType_duration, Dictionary<int, int> passType_numEmpl, Dictionary<int, List<int>> passType_empls, int brOcekivanihSati, int brRadnika, string wuName, List<TransferObjects.IOPairProcessedTO> listPairs)
        {
            TimeSpan wholeHours = new TimeSpan();
            foreach (KeyValuePair<int, TimeSpan> pst in passType_duration)
            {
                wholeHours += pst.Value;
            }
            string logFilePath = ConfigurationManager.AppSettings["treciIzvestajPath"] + "\\TreciIzvestaj_template.xlsx";
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Visible = false;
            Microsoft.Office.Interop.Excel.Workbook wb =
                excel.Workbooks.Open(logFilePath, Type.Missing, false, Type.Missing, Type.Missing, Type.Missing, true,
                Type.Missing, Type.Missing, true, false, Type.Missing, false, Type.Missing, Type.Missing);
            Microsoft.Office.Interop.Excel.Worksheet oSheets;
            oSheets = (Microsoft.Office.Interop.Excel.Worksheet)wb.Sheets[1];
            if (CheckBoxAllWU_TreciIzvestaj.Checked)
                oSheets.Cells[3, 2] = "Jedinica: sve jedinice izabrane";
            else
                oSheets.Cells[3, 2] = "Jedinica: " + wuName;
            oSheets.Cells[4, 2] = "Ukupno radnika: " + brRadnika;
            oSheets.Cells[5, 2] = "Ukupno planiranih sati: " + brOcekivanihSati + "h";
            oSheets.Cells[4, 3] = "Od: " + tbFromDate_TreciIzvestaj.Text;
            oSheets.Cells[5, 3] = "Do: " + tbToDate_TreciIzvestaj.Text;
            oSheets.Cells[4, 5] = "Ukupan broj sati";
            oSheets.Cells[5, 5] = (wholeHours.TotalHours * 1.0 / 24);
            oSheets.get_Range(oSheets.Cells[5, 5], oSheets.Cells[5, 5]).NumberFormat = "[h]:mm";
            int red = 7, kol = 2, i = 0;
            Common.PassType pt = new Common.PassType();
            Common.Employee emp = new Common.Employee();
            List<TransferObjects.PassTypeTO> listPT = new List<TransferObjects.PassTypeTO>();
            //popuni tipove prolaska koji idu za FTE nacin racunanja
            foreach (KeyValuePair<int, TimeSpan> pass_type in passType_duration)
            {
                TransferObjects.PassTypeTO ptTO = new TransferObjects.PassTypeTO();
                ptTO = pt.Find(pass_type.Key);
                //-1000,4,48,50,60,88,108
                if (pass_type.Key != -1000 && pass_type.Key != 4 && pass_type.Key != 48 && pass_type.Key != 50 && pass_type.Key != 60 && pass_type.Key != 88 && pass_type.Key != 108)
                {
                    oSheets.Cells[red + i, kol] = "(" + ptTO.PaymentCode + ") " + ptTO.Description;
                    oSheets.Cells[red + i, kol + 1] = pass_type.Value.TotalHours / brOcekivanihSati;
                    oSheets.get_Range(oSheets.Cells[red + i, kol + 1], oSheets.Cells[red + i, kol + 1]).NumberFormat = "00.00";
                    oSheets.Cells[red + i, kol + 2] = pass_type.Value.TotalHours / brOcekivanihSati;
                    oSheets.get_Range(oSheets.Cells[red + i, kol + 2], oSheets.Cells[red + i, kol + 2]).NumberFormat = "00.00%";
                    double dur = pass_type.Value.TotalHours / 24;
                    oSheets.Cells[red + i, kol + 3] = dur;
                    oSheets.get_Range(oSheets.Cells[red + i, kol + 3], oSheets.Cells[red + i, kol + 3]).NumberFormat = "[hh]:mm";
                    oSheets.get_Range("D" + (red + i), "E" + (red + i)).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    if (passType_numEmpl.ContainsKey(pass_type.Key))
                    {
                        oSheets.Cells[red + i, kol + 4] = passType_numEmpl[pass_type.Key];
                        oSheets.get_Range(oSheets.Cells[red + i, kol + 4], oSheets.Cells[red + i, kol + 4]).NumberFormat = "0";
                    }
                    i++;
                }
            }
            i--;
            Microsoft.Office.Interop.Excel.ChartObjects xlCharts = (Microsoft.Office.Interop.Excel.ChartObjects)oSheets.ChartObjects(Type.Missing);
            Microsoft.Office.Interop.Excel.ChartObject myChart = (Microsoft.Office.Interop.Excel.ChartObject)xlCharts.Add(0, 0, 550.08, 324);
            Microsoft.Office.Interop.Excel.Chart chartPage = myChart.Chart;
            chartPage.SetSourceData(oSheets.get_Range("B7", "C" + (red + i)), Type.Missing);
            chartPage.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlPie;
            i++;
            oSheets.get_Range(oSheets.Cells[red + i, kol], oSheets.Cells[red + i, kol + 4]).Merge(true);
            oSheets.Cells[red + i, kol] = "Tipovi prolaska dole ne ulaze u FTE format racunanja!";
            oSheets.get_Range(oSheets.Cells[red + i, kol], oSheets.Cells[red + i, kol]).Font.Bold = true;
            oSheets.get_Range(oSheets.Cells[red + i, kol], oSheets.Cells[red + i, kol]).RowHeight = 30;
            i++;
            //tipovi prolazaka koji ne idu u FTE -- prekovremeni radovi
            foreach (KeyValuePair<int, TimeSpan> pass_type in passType_duration)
            {
                TransferObjects.PassTypeTO ptTO = new TransferObjects.PassTypeTO();
                ptTO = pt.Find(pass_type.Key);
                //-1000,4,48,50,60,88,108
                if (pass_type.Key == -1000 || pass_type.Key == 4 || pass_type.Key == 48 || pass_type.Key == 50 || pass_type.Key == 60 || pass_type.Key == 88 || pass_type.Key == 108)
                {
                    oSheets.Cells[red + i, kol] = "(" + ptTO.PaymentCode + ") " + ptTO.Description;
                    oSheets.Cells[red + i, kol + 1] = "Neocekivani sati";
                    oSheets.Cells[red + i, kol + 2] = "Neocekivani sati";
                    double dur = pass_type.Value.TotalHours / 24;
                    oSheets.Cells[red + i, kol + 3] = dur;
                    oSheets.get_Range(oSheets.Cells[red + i, kol + 3], oSheets.Cells[red + i, kol + 3]).NumberFormat = "[hh]:mm";
                    oSheets.get_Range(oSheets.Cells[red + i, kol + 3], oSheets.Cells[red + i, kol + 3]).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    if (passType_numEmpl.ContainsKey(pass_type.Key))
                    {
                        oSheets.Cells[red + i, kol + 4] = passType_numEmpl[pass_type.Key];
                        oSheets.get_Range(oSheets.Cells[red + i, kol + 4], oSheets.Cells[red + i, kol + 4]).NumberFormat = "0";
                    }
                    i++;
                }
            }
            i--;
            Microsoft.Office.Interop.Excel.Range r = oSheets.get_Range("B3", "F" + (red + i));
            r.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            r.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;


            //postavljanje info o radnicima za tipove prolaska u dictionary
            red = 1; kol = 8; i = 0; // postavljanje parametara za celije u excel

            foreach (KeyValuePair<int, List<int>> emplFromList in passType_empls)
            {
                TransferObjects.PassTypeTO ptTO = new TransferObjects.PassTypeTO();
                TransferObjects.EmployeeTO emplTO = new TransferObjects.EmployeeTO();
                ptTO = pt.Find(emplFromList.Key);
                oSheets.get_Range(oSheets.Cells[red, kol + i], oSheets.Cells[red, kol + 2 + i]).Merge(true);
                oSheets.get_Range(oSheets.Cells[red + 1, kol + i], oSheets.Cells[red + 1, kol + 2 + i]).Merge(true);
                oSheets.Cells[red, kol + i] = "(" + ptTO.PaymentCode + ") " + ptTO.Description;
                oSheets.get_Range(oSheets.Cells[red, kol + i], oSheets.Cells[red, kol + i]).WrapText = true;
                Microsoft.Office.Interop.Excel.Range rng = oSheets.get_Range(oSheets.Cells[red, kol + i], oSheets.Cells[red, kol + 2 + i]);
                rng.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                rng.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;
                oSheets.get_Range(oSheets.Cells[red, kol + i], oSheets.Cells[red, kol + 2 + i]).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                red = 3;
                foreach (int emplID in emplFromList.Value)
                {
                    emplTO = emp.Find(emplID.ToString().Trim());
                    TimeSpan duration = new TimeSpan();
                    foreach (TransferObjects.IOPairProcessedTO pair in listPairs)
                    {
                        if (pair.PassTypeID == emplFromList.Key && emplID == pair.EmployeeID)
                        {
                            if (pair.EndTime.Minute == 59)
                                pair.EndTime = pair.EndTime.AddMinutes(1);
                            duration += (pair.EndTime - pair.StartTime);
                        }
                    }
                    oSheets.get_Range(oSheets.Cells[red, kol + i], oSheets.Cells[red + 1, kol + i + 2]).Merge(false);
                    oSheets.get_Range(oSheets.Cells[red, kol + i], oSheets.Cells[red + 1, kol + i + 2]).WrapText = true;
                    oSheets.get_Range(oSheets.Cells[red, kol + i], oSheets.Cells[red + 1, kol + i + 2]).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                    oSheets.get_Range(oSheets.Cells[red, kol + i], oSheets.Cells[red + 1, kol + i + 2]).Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
                    string hours = "", minutes = "";
                    int hoursInt = duration.Hours; int minutesInt = duration.Minutes; int daysInt = duration.Days;
                    if (daysInt > 0)
                        hoursInt += daysInt * 24;
                    if (hoursInt.ToString().Length < 2)
                    {
                        hours = "0" + hoursInt.ToString();
                    }
                    else
                        hours = hoursInt.ToString();
                    if (minutesInt.ToString().Length < 2)
                        minutes = "0" + minutesInt.ToString();
                    else
                        minutes = minutesInt.ToString();
                    oSheets.Cells[red, kol + i] = emplTO.FirstName + " " + emplTO.LastName + "(" + emplTO.EmployeeID + "), ukupno " + hours + ":" + minutes + "h";
                    red += 2;
                }
                red--;
                Microsoft.Office.Interop.Excel.Range rng1 = oSheets.get_Range(oSheets.Cells[3, kol + i], oSheets.Cells[red, kol + i + 2]);
                rng1.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                rng1.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;
                i += 4;
                red = 1;
            }
            DateTime from=Common.Misc.createDate(tbFromDate_TreciIzvestaj.Text.Trim());
            DateTime to=Common.Misc.createDate(tbFromDate_TreciIzvestaj.Text.Trim());
            string moment = DateTime.Now.ToString("yyyyMMddHHmmss");
            string pathSave=Constants.LPath+"\\Broj prisutnih period "+from.ToString("dd.MM.yy.")+"-"+to.ToString("dd.MM.yy")+"_"+moment+".xls";

            wb.SaveAs(pathSave, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal,
                                            Type.Missing, Type.Missing,
                                            false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                                            Type.Missing, Type.Missing, Type.Missing,
                                            Type.Missing, Type.Missing);
            Thread.Sleep(500);
            excel.Visible = true;
            Thread.Sleep(200);
            excel.Visible = false;
            excel.Quit();
            Marshal.ReleaseComObject(oSheets);
            Marshal.ReleaseComObject(wb);
            Marshal.ReleaseComObject(excel);

            oSheets = null;
            wb = null;
            excel = null;
            GC.GetTotalMemory(false);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.GetTotalMemory(true);
            Session["WorkAnalyzeReportsFilePath"] = pathSave;
            method();
        }


        protected void btnReport1_Click(Object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WorkAnalyzeReports).Assembly);

                lblError.Text = "";

                #region filters
                if (lBoxWU.Items.Count == 0)
                {
                    lblError.Text = rm.GetString("mustSelectWU", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                //get employees from selected WU
                string emplIDs = "";
                Dictionary<int, EmployeeTO> emplDict = new Dictionary<int, EmployeeTO>();
                Dictionary<int, WorkingUnitTO> wuDict = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();

                int[] selectedWU = lBoxWU.GetSelectedIndices();
                if (selectedWU.Length <= 0)
                {
                    lblError.Text = rm.GetString("mustSelectWU", culture);
                    return;
                }

                string units = "";
                int rootWorkingUnit = -1;
                int[] wuIndexes = lBoxWU.GetSelectedIndices();
                foreach (int index in wuIndexes)
                {
                    units += lBoxWU.Items[index].Value + ",";
                    int ID = -1;
                    if (int.TryParse(lBoxWU.Items[index].Value, out ID))
                    {
                        rootWorkingUnit = Common.Misc.getRootWorkingUnit(ID, wuDict);

                        //if (companyVisibleTypes.ContainsKey(company))
                        //    typesVisible.AddRange(companyVisibleTypes[company]);
                    }
                }

                if (units.Length > 0)
                    units = units.Substring(0, units.Length - 1);

                ////filter for wu
                //string wuSelected = "";

                //foreach (int index in selectedWU)
                //{
                //    wuSelected += lBoxWU.Items[index].Value + ",";
                //}

                //if (wuSelected.Length > 0)
                //    wuSelected = wuSelected.Substring(0, wuSelected.Length - 1);

                List<EmployeeTO> listOfEmployee = new Employee(Session[Constants.sessionConnection]).SearchByWU(Common.Misc.getWorkingUnitHierarhicly(units, (List<int>)Session[Constants.sessionLoginCategoryWUnits], Session[Constants.sessionConnection]));
                //(Session[Constants.sessionConnection]).SearchByWULoans(Common.Misc.getWorkingUnitHierarhicly(units, (List<int>)Session[Constants.sessionLoginCategoryWUnits], Session[Constants.sessionConnection])
                //new Employee(Session[Constants.sessionConnection]).SearchByWU(wuSelected);
                foreach (EmployeeTO empl in listOfEmployee)
                {
                    emplIDs += empl.EmployeeID + ",";
                    if (!emplDict.ContainsKey(empl.EmployeeID))
                        emplDict.Add(empl.EmployeeID, empl);
                }

                if (emplIDs.Length > 0)
                    emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                if (emplIDs.Length == 0)
                {
                    lblError.Text = rm.GetString("noSickLeave", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                // get asco data
                Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4(Session[Constants.sessionConnection]).SearchDictionary(emplIDs);

                DateTime from = CommonWeb.Misc.createDate(tbFromDate1.Text.Trim());
                DateTime to = CommonWeb.Misc.createDate(tbToDate1.Text.Trim());

                //date validation
                if (from.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidFromDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (to.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidToDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (from > to)
                {
                    lblError.Text = rm.GetString("invalidFromToDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }
                #endregion


                #region pairs
                // get pairs for one more day becouse of third shifts
                List<DateTime> dateList = new List<DateTime>();
                DateTime currentDate = from.Date;
                while (currentDate.Date <= to.AddDays(1).Date)
                {
                    dateList.Add(currentDate.Date);
                    currentDate = currentDate.AddDays(1);
                }

                List<IOPairProcessedTO> allPairs = new IOPairProcessed(Session[Constants.sessionConnection]).SearchAllPairsForEmpl(emplIDs, dateList, "");
                if (allPairs.Count == 0)
                {
                    lblError.Text = rm.GetString("noSickLeave", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }
                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplDayPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplBelongingDayPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                // create list of pairs foreach employee and each day
                foreach (IOPairProcessedTO pair in allPairs)
                {
                    if (!emplDayPairs.ContainsKey(pair.EmployeeID))
                        emplDayPairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());
                    if (!emplDayPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                        emplDayPairs[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<IOPairProcessedTO>());
                    emplDayPairs[pair.EmployeeID][pair.IOPairDate.Date].Add(pair);
                }
                #endregion

                #region national and personal holidays
                //get national and personal holidays
                List<DateTime> nationalHolidaysDays = new List<DateTime>();
                Dictionary<string, List<DateTime>> personalHolidayDays = new Dictionary<string, List<DateTime>>();
                List<DateTime> nationalHolidaysSundays = new List<DateTime>();
                List<HolidaysExtendedTO> nationalTransferableHolidays = new List<HolidaysExtendedTO>();
                //filling the dictionaries and lists above
                Common.Misc.getHolidays(from.Date, to.Date, personalHolidayDays, nationalHolidaysDays, nationalHolidaysSundays, null, nationalTransferableHolidays);
                #endregion

                #region schemas and intervals
                // get schemas and intervals
                Dictionary<int, Dictionary<DateTime, WorkTimeSchemaTO>> emplDaySchemas = new Dictionary<int, Dictionary<DateTime, WorkTimeSchemaTO>>();
                Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>> emplDayIntervals = new Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>>();
                // get schedules for selected employees and date interval
                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule(Session[Constants.sessionConnection]).SearchEmployeesSchedulesExactDate(emplIDs, from.Date, to.Date.AddDays(1), null);
                Dictionary<int, WorkTimeSchemaTO> schemas = new TimeSchema(Session[Constants.sessionConnection]).getDictionary();

                foreach (int emplID in emplSchedules.Keys)
                {
                    DateTime currDate = from.Date;

                    while (currDate <= to.Date.AddDays(1))
                    {
                        if (!emplDayIntervals.ContainsKey(emplID))
                            emplDayIntervals.Add(emplID, new Dictionary<DateTime, List<WorkTimeIntervalTO>>());

                        if (!emplDayIntervals[emplID].ContainsKey(currDate.Date))
                            emplDayIntervals[emplID].Add(currDate.Date, Common.Misc.getTimeSchemaInterval(currDate.Date, emplSchedules[emplID], schemas));

                        if (!emplDaySchemas.ContainsKey(emplID))
                            emplDaySchemas.Add(emplID, new Dictionary<DateTime, WorkTimeSchemaTO>());

                        WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                        if (emplDayIntervals[emplID][currDate.Date].Count > 0 && schemas.ContainsKey(emplDayIntervals[emplID][currDate.Date][0].TimeSchemaID))
                            sch = schemas[emplDayIntervals[emplID][currDate.Date][0].TimeSchemaID];

                        if (!emplDaySchemas[emplID].ContainsKey(currDate.Date))
                            emplDaySchemas[emplID].Add(currDate.Date, sch);

                        currDate = currDate.AddDays(1).Date;
                    }
                }
                #endregion

                //string costumer = Common.Misc.getCustomer(Session[Constants.sessionConnection]);
                //int cost = 0;
                //bool costum = int.TryParse(costumer, out cost);
                //bool isFiat = (cost == (int)Constants.Customers.FIAT);

                #region rules
                Dictionary<int, Dictionary<string, RuleTO>> emplRules = new Dictionary<int, Dictionary<string, RuleTO>>();
                Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rules = new Common.Rule(Session[Constants.sessionConnection]).SearchWUEmplTypeDictionary();
                // create rules for each employee
                foreach (int emplID in emplDict.Keys)
                {
                    if (!emplRules.ContainsKey(emplID))
                    {
                        int company = Common.Misc.getRootWorkingUnit(emplDict[emplID].WorkingUnitID, wuDict);

                        if (rules.ContainsKey(company) && rules[company].ContainsKey(emplDict[emplID].EmployeeTypeID))
                            emplRules.Add(emplID, rules[company][emplDict[emplID].EmployeeTypeID]);
                    }
                }
                #endregion


                #region Fill the dictionary
                Dictionary<int, List<SickLeavePeriodTO>> emplAndSickLeavePeriods = new Dictionary<int, List<SickLeavePeriodTO>>(); //emplID is the key, list of SickLeavePeriods is value
                int maxNoOfPeriod = 0;
                foreach (int emplID in emplDict.Keys)
                {
                    if (emplDayPairs.ContainsKey(emplID))
                    {
                        List<SickLeavePeriodTO> listOfSickPeriods = new List<SickLeavePeriodTO>();
                        if (!emplDayPairs.ContainsKey(emplID) || !emplDaySchemas.ContainsKey(emplID) || !emplDayIntervals.ContainsKey(emplID) || !emplRules.ContainsKey(emplID))
                            continue;

                        Dictionary<DateTime, List<IOPairProcessedTO>> employeeDayPairs = emplDayPairs[emplID];
                        Dictionary<DateTime, WorkTimeSchemaTO> daysSchema = emplDaySchemas[emplID];
                        Dictionary<DateTime, List<WorkTimeIntervalTO>> daysListInterval = emplDayIntervals[emplID];
                        Dictionary<string, RuleTO> employeeRules = emplRules[emplID];

                        int noOfPeriods = 0;
                        //TimeSpan noOfHours = new TimeSpan();
                        int previousType = -1;
                        DateTime previousDate = new DateTime();
                        bool sickLeaveStarted = false;
                        SickLeavePeriodTO sickLeave = new SickLeavePeriodTO();
                        List<WorkTimeIntervalTO> listOfWorkInterval = new List<WorkTimeIntervalTO>();
                        List<IOPairProcessedTO> listOfIOPairs = new List<IOPairProcessedTO>();

                        foreach (DateTime date in employeeDayPairs.Keys)
                        {
                            listOfWorkInterval = daysListInterval[date];
                            listOfIOPairs = employeeDayPairs[date];

                            foreach (WorkTimeIntervalTO timeInterval in listOfWorkInterval)
                            {
                                if (timeInterval.StartTime.TimeOfDay.Hours == 0 && timeInterval.StartTime.TimeOfDay.Minutes == 0 && timeInterval.StartTime.TimeOfDay.Seconds == 0 &&
                                    timeInterval.EndTime.TimeOfDay.Hours == 0 && timeInterval.EndTime.TimeOfDay.Minutes == 0 && timeInterval.EndTime.TimeOfDay.Seconds == 0)
                                    continue;
                            }

                            foreach (IOPairProcessedTO pair in listOfIOPairs)
                            {
                                int passTypeID = pair.PassTypeID; //passTypeID of pair

                                //check with the rules

                                if (!employeeRules.ContainsKey(Constants.RuleCompanySickLeaveNCF) ||
                                    !employeeRules.ContainsKey(Constants.RuleCompanySickLeave30Days) ||
                                    !employeeRules.ContainsKey(Constants.RuleCompanySickLeave30DaysContinuation) ||
                                    !employeeRules.ContainsKey(Constants.RuleCompanySickLeaveIndustrialInjury) ||
                                    !employeeRules.ContainsKey(Constants.RuleCompanySickLeaveIndustrialInjuryContinuation))
                                    continue;

                                if (passTypeID == employeeRules[Constants.RuleCompanySickLeaveNCF].RuleValue || //bolovanje
                                    passTypeID == employeeRules[Constants.RuleCompanySickLeave30Days].RuleValue || //bolovanje do 30 dana
                                    passTypeID == employeeRules[Constants.RuleCompanySickLeave30DaysContinuation].RuleValue || //bolovanje do 30 dana nastavak
                                    passTypeID == employeeRules[Constants.RuleCompanySickLeaveIndustrialInjury].RuleValue || // povreda na radu
                                    passTypeID == employeeRules[Constants.RuleCompanySickLeaveIndustrialInjuryContinuation].RuleValue)
                                {
                                    if (sickLeaveStarted == false) //if any leave has not started
                                    {
                                        bool continueCalculate = true;
                                        bool calculateThisDay = true;
                                        foreach (WorkTimeIntervalTO timeInterval in listOfWorkInterval)
                                        {
                                            if (timeInterval.StartTime.TimeOfDay.Hours == 0 && timeInterval.StartTime.TimeOfDay.Minutes == 0 && timeInterval.StartTime.TimeOfDay.Seconds == 0 &&
                                                pair.StartTime.TimeOfDay.Hours == 0 && pair.StartTime.TimeOfDay.Minutes == 0 && pair.StartTime.TimeOfDay.Seconds == 0
                                                && date.Date == from.Date) //if it is first chosen date (FROM) and it is the end of third shift from a day before, do not calculate it!
                                            {
                                                continueCalculate = false;
                                            }
                                        }

                                        if (date.Date == to.AddDays(1).Date &&
                                         pair.StartTime > new DateTime(date.Year, date.Month, date.Day, 0, 0, 0) &&
                                         pair.EndTime < new DateTime(date.Year, date.Month, date.Day, 23, 59, 0)) //this is not third shift so you do not need to calculate this day!
                                        {
                                            calculateThisDay = false;
                                        }

                                        if (calculateThisDay && continueCalculate)
                                        {
                                            sickLeave = new SickLeavePeriodTO();
                                            sickLeave.WageType = passTypeID;
                                            sickLeave.StartDate = date;
                                            sickLeave.EndDate = date;
                                            sickLeave.NoOfDays += 1;
                                            noOfPeriods++;
                                            sickLeave.PeriodNo = noOfPeriods;
                                            int duration = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;
                                            if (pair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                                duration++;
                                            sickLeave.NoOfMinutes += duration;
                                            previousType = pair.PassTypeID;
                                            previousDate = date;
                                            sickLeaveStarted = true;
                                        }
                                    }
                                    else if (previousType == passTypeID)
                                    {
                                        bool continueCalculate = true;
                                        bool isThirdShift = false;
                                        bool calculateThisDay = true;
                                        foreach (WorkTimeIntervalTO timeInterval in listOfWorkInterval)
                                        {
                                            if (timeInterval.StartTime.TimeOfDay.Hours == 0 && timeInterval.StartTime.TimeOfDay.Minutes == 0 && timeInterval.StartTime.TimeOfDay.Seconds == 0 &&
                                                pair.StartTime.TimeOfDay.Hours == 0 && pair.StartTime.TimeOfDay.Minutes == 0 && pair.StartTime.TimeOfDay.Seconds == 0)
                                            {
                                                isThirdShift = true;
                                            }
                                            if (date.Date == to.AddDays(1).Date &&
                                                timeInterval.EndTime.TimeOfDay.Hours == 23 && timeInterval.EndTime.TimeOfDay.Minutes == 59 && timeInterval.EndTime.TimeOfDay.Seconds == 0 &&
                                                pair.EndTime.TimeOfDay.Hours == 23 && pair.EndTime.TimeOfDay.Minutes == 59 && pair.EndTime.TimeOfDay.Seconds == 0)
                                            {
                                                continueCalculate = false;
                                            }
                                        }

                                        if (date.Date == to.AddDays(1).Date &&
                                         pair.StartTime > new DateTime(date.Year, date.Month, date.Day, 0, 0, 0) &&
                                         pair.EndTime < new DateTime(date.Year, date.Month, date.Day, 23, 59, 0)) //this is not third shift so you do not need to calculate this day!
                                        {
                                            calculateThisDay = false;
                                        }

                                        if (calculateThisDay && continueCalculate)
                                        {
                                            int duration = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;
                                            if (pair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                                duration++;
                                            sickLeave.NoOfMinutes += duration;
                                            previousType = pair.PassTypeID;
                                            if (isThirdShift == false)
                                            {
                                                sickLeave.EndDate = date;
                                                sickLeave.NoOfDays += 1;
                                            }
                                            isThirdShift = false;
                                            previousDate = date;
                                        }
                                    }
                                    else
                                    {
                                        if ((previousType == employeeRules[Constants.RuleCompanySickLeave30Days].RuleValue && passTypeID == employeeRules[Constants.RuleCompanySickLeave30DaysContinuation].RuleValue) ||
                                            (previousType == employeeRules[Constants.RuleCompanySickLeaveIndustrialInjury].RuleValue && passTypeID == employeeRules[Constants.RuleCompanySickLeaveIndustrialInjuryContinuation].RuleValue))
                                        {
                                            bool continueCalculate = true;
                                            bool isThirdShift = false;
                                            bool calculateThisDay = true;
                                            foreach (WorkTimeIntervalTO timeInterval in listOfWorkInterval)
                                            {
                                                if (timeInterval.StartTime.TimeOfDay.Hours == 0 && timeInterval.StartTime.TimeOfDay.Minutes == 0 && timeInterval.StartTime.TimeOfDay.Seconds == 0 &&
                                                    pair.StartTime.TimeOfDay.Hours == 0 && pair.StartTime.TimeOfDay.Minutes == 0 && pair.StartTime.TimeOfDay.Seconds == 0)
                                                {
                                                    isThirdShift = true;
                                                }
                                                if (date.Date == to.AddDays(1).Date &&
                                                timeInterval.EndTime.TimeOfDay.Hours == 23 && timeInterval.EndTime.TimeOfDay.Minutes == 59 && timeInterval.EndTime.TimeOfDay.Seconds == 0 &&
                                                pair.EndTime.TimeOfDay.Hours == 23 && pair.EndTime.TimeOfDay.Minutes == 59 && pair.EndTime.TimeOfDay.Seconds == 0)
                                                {
                                                    continueCalculate = false;
                                                }
                                                //if (date.Date == to.AddDays(1).Date && timeInterval.EndTime.TimeOfDay.Hours != 23 && timeInterval.EndTime.TimeOfDay.Minutes != 59 && timeInterval.EndTime.TimeOfDay.Seconds != 0 &&
                                                //timeInterval.StartTime.TimeOfDay.Hours != 0 && timeInterval.StartTime.TimeOfDay.Minutes != 0 && timeInterval.StartTime.TimeOfDay.Seconds != 0) //this is not third shift so you do not need to calculate this day!
                                                //{
                                                //    calculateThisDay = false;
                                                //}
                                            }

                                            if (date.Date == to.AddDays(1).Date &&
                                                pair.StartTime > new DateTime(date.Year, date.Month, date.Day, 0, 0, 0) &&
                                                pair.EndTime < new DateTime(date.Year, date.Month, date.Day, 23, 59, 0)) //this is not third shift so you do not need to calculate this day!
                                            {
                                                calculateThisDay = false;
                                            }

                                            if (calculateThisDay && continueCalculate)
                                            {
                                                int duration = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;
                                                if (pair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                                    duration++;
                                                sickLeave.NoOfMinutes += duration;
                                                previousType = pair.PassTypeID;
                                                sickLeave.WageTypeContinue = pair.PassTypeID;
                                                if (isThirdShift == false)
                                                {
                                                    sickLeave.EndDate = date;
                                                    sickLeave.NoOfDays += 1;
                                                }
                                                isThirdShift = false;
                                                previousDate = date;
                                            }
                                        }
                                        else
                                        {

                                            listOfSickPeriods.Add(sickLeave); //add SickLeave to a list, because it is over

                                            bool calculateThisDay = true;
                                            if (date.Date == to.AddDays(1).Date &&
                                                pair.StartTime > new DateTime(date.Year, date.Month, date.Day, 0, 0, 0) &&
                                                pair.EndTime < new DateTime(date.Year, date.Month, date.Day, 23, 59, 0)) //this is not third shift so you do not need to calculate this day!
                                            {
                                                calculateThisDay = false;
                                            }

                                            if (calculateThisDay)
                                            {
                                                //create new sickLeave object and add data
                                                sickLeave = new SickLeavePeriodTO();
                                                sickLeave.WageType = passTypeID;
                                                sickLeave.StartDate = date;
                                                sickLeave.EndDate = date;
                                                sickLeave.NoOfDays += 1;
                                                noOfPeriods++;
                                                sickLeave.PeriodNo = noOfPeriods;
                                                int duration = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;
                                                if (pair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                                    duration++;
                                                sickLeave.NoOfMinutes += duration;
                                                previousType = pair.PassTypeID;
                                                previousDate = date;
                                                sickLeaveStarted = true;
                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    if (sickLeaveStarted)
                                    {
                                        listOfSickPeriods.Add(sickLeave); //add SickLeave to a list, because it is over
                                        previousType = 0;
                                        previousDate = new DateTime();
                                        sickLeaveStarted = false;
                                    }
                                }
                            }
                        }

                        if (sickLeaveStarted)
                        {
                            listOfSickPeriods.Add(sickLeave); //add SickLeave to a list, because it is over
                            previousType = 0;
                            previousDate = new DateTime();
                            sickLeaveStarted = false;
                        }

                        if (!emplAndSickLeavePeriods.ContainsKey(emplID) && listOfSickPeriods.Count > 0)
                        {
                            emplAndSickLeavePeriods.Add(emplID, listOfSickPeriods);
                            if (maxNoOfPeriod < listOfSickPeriods.Count)
                                maxNoOfPeriod = listOfSickPeriods.Count;
                        }
                    }
                }
                #endregion

                if (emplAndSickLeavePeriods.Count == 0)
                {
                    lblError.Text = rm.GetString("noSickLeave", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                #region create XLS

                int rowCount = 0;

                DebugLog log = new DebugLog(Constants.logFilePath + "LogReport.txt");
                log.writeLog("Start " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:sss"));

                //DataTable ioPairs = null;
                log.writeLog(rowCount.ToString());

                if (maxNoOfPeriod == 0)
                {
                    lblError.Text = rm.GetString("noSickLeave", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                ApplUserTO user = (ApplUserTO)Session[Constants.sessionLogInUser];
                string filePath = Constants.logFilePath + "Temp\\" + user.UserID + "_SickLeavePeriods" + DateTime.Now.ToString("dd-MM-yyyy HHmmss") + "\\";
                filePath += "SickLeavePeriods" + DateTime.Now.ToString("dd-MM-yyyy HHmmss") + ".xls";
                log.writeLog(filePath);

                string Pathh = Directory.GetParent(filePath).FullName;
                if (!Directory.Exists(Pathh))
                {
                    Directory.CreateDirectory(Pathh);
                }
                if (File.Exists(filePath))
                    File.Delete(filePath);

                ////DELETE OLD FOLDERS AND FILES FROM TEMP
                string tempFolder = Constants.logFilePath + "Temp";
                string[] dirs = Directory.GetDirectories(tempFolder);
                foreach (string direct in dirs)
                {
                    DateTime creation = Directory.GetCreationTime(direct);
                    TimeSpan create = creation.AddHours(2).TimeOfDay;

                    if (creation.Date < DateTime.Now.Date || create < DateTime.Now.TimeOfDay)
                        Directory.Delete(direct, true);
                }


                StreamWriter writer = new StreamWriter(filePath, true, System.Text.Encoding.Unicode);

                int noOfColumns = 6 + (5 * maxNoOfPeriod);
                string[] columns = new string[noOfColumns];

                int periodNumber = 1;
                for (int i = 0; i < noOfColumns; i += 6)
                {
                    int columnNo = i + 6;
                    if (columnNo < noOfColumns)
                    {
                        columns[columnNo] = rm.GetString("hdrPeriod", culture) + " " + periodNumber;
                        periodNumber++;
                    }
                }

                string columnsString = "";
                foreach (string c in columns)
                {
                    columnsString += c + "\t";
                }
                writer.WriteLine(columnsString);

                string namesOfColumns = "";

                namesOfColumns = "PY ID" + "\t" +
                    "Employee" + "\t" +
                    "Stringone" + "\t" +
                    "Cost center" + "\t" +
                    "Num. of periods" + "\t" +
                    "Num of days" + "\t";

                for (int i = 0; i < maxNoOfPeriod; i++)
                {
                    namesOfColumns += "Wage types" + "\t" +
                        "Num of days" + "\t" +
                         "Num of hrs" + "\t" +
                          "Start date" + "\t" +
                           "End date" + "\t";
                }

                writer.WriteLine(namesOfColumns);

                Dictionary<int, PassTypeTO> ptDict = new PassType(Session[Constants.sessionConnection]).SearchDictionary();

                foreach (int id in emplAndSickLeavePeriods.Keys)
                {
                    EmployeeTO employee = emplDict[id];
                    string stringone = ascoDict[id].NVarcharValue2;
                    string cc = Common.Misc.getEmplCostCenter(employee, wuDict, Session[Constants.sessionConnection]).Description.Trim();
                    int noOfPeriods = emplAndSickLeavePeriods[id].Count;
                    int noOfDays = 0;
                    foreach (SickLeavePeriodTO sickLeavePeriod in emplAndSickLeavePeriods[id])
                    {
                        noOfDays += sickLeavePeriod.NoOfDays;
                    }

                    string resultString = "";

                    resultString += employee.EmployeeID + "\t";
                    resultString += employee.FirstAndLastName + "\t";
                    resultString += stringone + "\t";
                    resultString += cc + "\t";
                    resultString += noOfPeriods + "\t";
                    resultString += noOfDays + "\t";

                    foreach (SickLeavePeriodTO sickLeavePeriod in emplAndSickLeavePeriods[id])
                    {
                        int company = Common.Misc.getRootWorkingUnit(emplDict[id].WorkingUnitID, wuDict);
                        int wageType = sickLeavePeriod.WageType;
                        PassTypeTO passType = new PassTypeTO();
                        if (ptDict.ContainsKey(wageType))
                            passType = ptDict[wageType];
                        PassTypeTO passTypeContinue = new PassTypeTO();
                        int wageTypeContinue = -1;
                        if (sickLeavePeriod.WageTypeContinue != -1)
                        {
                            wageTypeContinue = sickLeavePeriod.WageTypeContinue;
                            if (ptDict.ContainsKey(wageTypeContinue))
                                passTypeContinue = ptDict[wageTypeContinue];
                            //passTypeContinue = new PassType(Session[Constants.sessionConnection]).Find(wageTypeContinue);
                        }

                        if (wageTypeContinue != -1)
                            resultString += passType.Description + " (" + passType.PaymentCode + ")" + " / " +
                                            passTypeContinue.Description + " (" + passTypeContinue.PaymentCode + ")" + "\t";
                        else
                            resultString += passType.Description + " (" + passType.PaymentCode + ")" + "\t";

                        resultString += sickLeavePeriod.NoOfDays + "\t";
                        //resultString += Math.Round(sickLeavePeriod.NoOfHours) + "\t";
                        resultString += (sickLeavePeriod.NoOfMinutes / 60.0).ToString(Constants.doubleFormat) + "\t";
                        resultString += sickLeavePeriod.StartDate.Date.ToString(Constants.dateFormat.Trim()) + "\t";
                        resultString += sickLeavePeriod.EndDate.ToString(Constants.dateFormat.Trim()) + "\t";
                    }
                    writer.WriteLine(resultString);

                }

                int numr = 0;

                log.writeLog(numr + "  " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:sss"));
                log.writeLog("Close" + "  " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:sss"));

                writer.Close();
                Response.Write("finished");

                Session["WorkAnalyzeReportsFilePath"] = filePath;

                #endregion
                method();
                lblError.Text = "";
                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WorkAnalyzeReports.btnReport1_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WorkAnalyzeReports.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
            finally
            {

            }
        }

        protected void method()
        {

            DebugLog log = new DebugLog(Constants.logFilePath + "LogReport.txt");

            if (Session["WorkAnalyzeReportsFilePath"] != null)
            {
                log.writeLog("Start detailed" + " " + DateTime.Now.ToString("HH:mm:sss"));

                string filePath = Session["WorkAnalyzeReportsFilePath"].ToString();
                Session["WorkAnalyzeReportsFilePath"] = null;

                if (!filePath.Trim().Equals(""))
                {
                    string name = Path.GetFileNameWithoutExtension(filePath);
                    WebClient client = new WebClient();
                    Byte[] buffer = client.DownloadData(filePath);

                    if (buffer != null)
                    {
                        Response.Clear();
                        Response.ContentType = "application/xls";
                        //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AppendHeader("Content-Disposition", "attachment;Filename=" + name + ".xls");
                        Response.AddHeader("Content-Length", buffer.Length.ToString());
                        Response.TransmitFile(filePath);
                        //Response.End();
                        Response.Flush();
                        log.writeLog("Flush " + " " + DateTime.Now.ToString("HH:mm:sss"));

                    }
                    Response.Clear();

                    log.writeLog("Session clear" + " " + DateTime.Now.ToString("HH:mm:sss"));
                }
            }
        }
        public void lbWU_IndexChanged(object sender, EventArgs e)
        {
        }
        public void cbSelectAllWU_CheckedChanged_TreciIzvestaj(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem item in lbWU_TreciIzvestaj.Items)
                {
                    item.Selected = CheckBoxAllWU_TreciIzvestaj.Checked;
                }

                int[] selWUIndex = lbWU_TreciIzvestaj.GetSelectedIndices();
                if (selWUIndex.Length > 0)
                {
                    int wuID = -1;

                    if (int.TryParse(lbWU_TreciIzvestaj.Items[selWUIndex[0]].Value.Trim(), out wuID))
                    {
                        Session[Constants.sessionWU] = wuID;
                        Session[Constants.sessionOU] = null;
                    }
                }

                Session[Constants.sessionSamePage] = true;

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WorkAnalyzeReports.cbSelectAllWU_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLDetailedDataPage.aspx".Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void SaveState()
        {
            try
            {
                Dictionary<string, string> filterState = new Dictionary<string, string>();

                if (Session[Constants.sessionFilterState] != null && Session[Constants.sessionFilterState] is Dictionary<string, string>)
                    filterState = (Dictionary<string, string>)Session[Constants.sessionFilterState];

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "WorkAnalyzeReports.", filterState);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadState()
        {
            try
            {
                Dictionary<string, string> filterState = new Dictionary<string, string>();

                if (Session[Constants.sessionFilterState] != null && Session[Constants.sessionFilterState] is Dictionary<string, string>)
                {
                    filterState = (Dictionary<string, string>)Session[Constants.sessionFilterState];
                    CommonWeb.Misc.LoadState(this, "WorkAnalyzeReports.", filterState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class SickLeavePeriodTO
    {
        int _periodNo = 0;

        public int PeriodNo
        {
            get { return _periodNo; }
            set { _periodNo = value; }
        }

        int _wageType = -1;

        public int WageType
        {
            get { return _wageType; }
            set { _wageType = value; }
        }

        int _wageTypeContinue = -1;

        public int WageTypeContinue
        {
            get { return _wageTypeContinue; }
            set { _wageTypeContinue = value; }
        }

        int _noOfDays = 0;

        public int NoOfDays
        {
            get { return _noOfDays; }
            set { _noOfDays = value; }
        }

        int _noOfMinutes = 0;

        public int NoOfMinutes
        {
            get { return _noOfMinutes; }
            set { _noOfMinutes = value; }
        }

        DateTime _startDate = new DateTime();

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        DateTime _endDate = new DateTime();

        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        public SickLeavePeriodTO()
        {


        }

        public SickLeavePeriodTO(int periodNo, int wageType, int wageTypeContinue, int noOfDays, int noOfMinutes, DateTime startDate, DateTime endDate)
        {
            this.PeriodNo = periodNo;
            this.WageType = wageType;
            this.WageTypeContinue = wageTypeContinue;
            this.NoOfDays = noOfDays;
            this.NoOfMinutes = noOfMinutes;
            this.StartDate = startDate;
            this.EndDate = endDate;
        }
    }
}
