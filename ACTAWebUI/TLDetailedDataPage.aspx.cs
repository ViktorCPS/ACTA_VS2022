using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Resources;
using System.Globalization;
using System.Drawing;
using System.Data;

using Common;
using TransferObjects;
using Util;
using ReportsWeb;
using System.Configuration;
using System.Net;
using System.IO;
using System.Text;


namespace ACTAWebUI
{
    public partial class TLDetailedDataPage : System.Web.UI.Page
    {
        const string pageName = "TLDetailedDataPage";

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




        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // if user do not want to reload selected filter and get results for that filter, parameter reloadState should be passes through url and set to false
                if (!IsPostBack)
                {
                    btnFromDate.Attributes.Add("onclick", "return calendarPicker('tbFromDate', 'true');");
                    btnToDate.Attributes.Add("onclick", "return calendarPicker('tbToDate', 'true');");
                    cbSelectAllPassTypes.Attributes.Add("onclick", "return selectListItems('cbSelectAllPassTypes', 'lbPassTypes');");
                    cbSelectAllEmpolyees.Attributes.Add("onclick", "return selectListItems('cbSelectAllEmpolyees', 'lboxEmployees');");
                    tbEmployee.Attributes.Add("onKeyUp", "return selectItem('tbEmployee', 'lboxEmployees');");
                    btnShow.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnReport.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnReportNew.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    //hidden.ValueChanged += new EventHandler(hidden_ValueChanged);
                    btnFromDate.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnFromDate.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnToDate.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnToDate.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnPrevDayPeriod.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnPrevDayPeriod.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnNextDayPeriod.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnNextDayPeriod.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");

                    cbSelectAllEmpolyees.Visible = cbSelectAllPassTypes.Visible = false;

                    if (Session[Constants.sessionFromDate] != null && Session[Constants.sessionFromDate] is DateTime)
                        tbFromDate.Text = ((DateTime)Session[Constants.sessionFromDate]).ToString(Constants.dateFormat.Trim());
                    else
                        tbFromDate.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());

                    if (Session[Constants.sessionToDate] != null && Session[Constants.sessionToDate] is DateTime)
                        tbToDate.Text = ((DateTime)Session[Constants.sessionToDate]).ToString(Constants.dateFormat.Trim());
                    else
                        tbToDate.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());

                    tbFromTime.Text = Constants.dayStartTime.Trim();
                    tbToTime.Text = Constants.dayEndTime.Trim();

                    int defaultWUID = -1;
                    int defaultOUID = -1;
                    if (Session[Constants.sessionLogInEmployee] != null && Session[Constants.sessionLogInEmployee] is EmployeeTO)
                    {
                        EmployeeAsco4 emplAsco = new EmployeeAsco4(Session[Constants.sessionConnection]);
                        emplAsco.EmplAsco4TO.EmployeeID = ((EmployeeTO)Session[Constants.sessionLogInEmployee]).EmployeeID;

                        List<EmployeeAsco4TO> emplAscoList = emplAsco.Search();

                        if (emplAscoList.Count == 1)
                        {
                            defaultWUID = emplAscoList[0].IntegerValue2;
                            defaultOUID = emplAscoList[0].IntegerValue3;
                        }
                    }

                    populateWU(defaultWUID);
                    populateOU(defaultOUID);

                    if (Session[Constants.sessionWU] == null && Session[Constants.sessionOU] == null)
                    {
                        if (defaultWUID == -1 && defaultOUID != -1)
                            Session[Constants.sessionOU] = defaultOUID;
                        else
                            Session[Constants.sessionWU] = defaultWUID;
                    }

                    if (Session[Constants.sessionOU] != null && Session[Constants.sessionOU] is int)
                    {
                        foreach (ListItem oItem in lBoxOU.Items)
                        {
                            oItem.Selected = oItem.Value.Trim().Equals(((int)Session[Constants.sessionOU]).ToString().Trim());
                        }

                        Menu1.Items[1].Selected = true;
                        MultiView1.SetActiveView(MultiView1.Views[1]);
                    }
                    else if (Session[Constants.sessionWU] != null && Session[Constants.sessionWU] is int)
                    {
                        foreach (ListItem wItem in lBoxWU.Items)
                        {
                            wItem.Selected = wItem.Value.Trim().Equals(((int)Session[Constants.sessionWU]).ToString().Trim());
                        }

                        Menu1.Items[0].Selected = true;
                        MultiView1.SetActiveView(MultiView1.Views[0]);
                    }

                    for (int i = 0; i < Menu1.Items.Count; i++)
                    {
                        CommonWeb.Misc.setMenuImage(Menu1, i, Menu1.Items[i].Selected);
                        CommonWeb.Misc.setMenuSeparator(Menu1, i, Menu1.Items[i].Selected);
                    }

                    setLanguage();

                    rbPassType.Checked = true;
                    rbLocation.Checked = false;
                    
                    // check pass types/locations switch visibility
                    Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                    rule.RuleTO.EmployeeTypeID = (int)Constants.EmployeeTypesFIAT.BC;
                    rule.RuleTO.WorkingUnitID = Constants.defaultWorkingUnitID;
                    rule.RuleTO.RuleType = Constants.RulePairsByLocation;
                    List<RuleTO> locRules = rule.Search();

                    if (locRules.Count > 0 && locRules[0].RuleValue == Constants.yesInt)
                        rbPassType.Visible = rbLocation.Visible = true;
                    else
                        rbPassType.Visible = rbLocation.Visible = false;                    
                    
                    populatePassTypes(Menu1.SelectedItem.Value.Equals("0"));

                    if (Request.QueryString["reloadState"] != null && Request.QueryString["reloadState"].Trim().ToUpper().Equals(Constants.falseValue.Trim().ToUpper()))
                    {
                        ClearSessionValues();

                        populateEmployees(Menu1.SelectedItem.Value.Equals("0"));
                        
                        if (Session[Constants.sessionSelectedEmplIDs] != null && Session[Constants.sessionSelectedEmplIDs] is List<string>)
                        {
                            foreach (ListItem item in lboxEmployees.Items)
                            {
                                if (((List<string>)Session[Constants.sessionSelectedEmplIDs]).Contains(item.Value.Trim()))
                                    item.Selected = true;
                            }
                        }
                    }
                    else
                    {
                        // reload selected filter state                        
                        LoadState();
                        populateEmployees(Menu1.SelectedItem.Value.Equals("0"));
                        populatePassTypes(Menu1.SelectedItem.Value.Equals("0"));                        
                        LoadState();

                        //resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?showSelection=false";
                    }

                    btnShow_Click(this, new EventArgs());

                    //method();
                }

                writeLog(DateTime.Now, false);

            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLDetailedDataPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLDetailedDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }


        protected void method()
        {

            DebugLog log = new DebugLog(Constants.logFilePath + "LogReport.txt");


            if (Session["DetailedDataReportFilePath"] != null)
            {
                log.writeLog("Start detailed" + " " + DateTime.Now.ToString("HH:mm:sss"));

                string filePath = Session["DetailedDataReportFilePath"].ToString();                
                Session["DetailedDataReportFilePath"] = null;

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
        protected void btnPrevDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLDetailedDataPage).Assembly);

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

                Date_Changed(this, new EventArgs());

                //btnShow_Click(this, new EventArgs());

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLDetailedDataPage.btnPrevDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLDetailedDataPage.aspx&Header=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnNextDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLDetailedDataPage).Assembly);

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

                Date_Changed(this, new EventArgs());

                //btnShow_Click(this, new EventArgs());

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLDetailedDataPage.btnNextDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLDetailedDataPage.aspx&Header=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
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

                populateEmployees(true);
                populatePassTypes(true);
                Session[Constants.sessionSamePage] = true;
                cbSelectAllEmpolyees.Checked = false;

                btnShow_Click(this, new EventArgs());

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLDetailedDataPage.lBoxWU_SelectedIndexChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLDetailedDataPage.aspx", false);
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

                populateEmployees(true);
                Session[Constants.sessionSamePage] = true;

                btnShow_Click(this, new EventArgs());

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLDetailedDataPage.cbSelectAllWU_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLDetailedDataPage.aspx".Trim(), false);
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
                    lBoxWU.DataTextField = "Name";
                    lBoxWU.DataValueField = "WorkingUnitID";
                    lBoxWU.DataBind();

                    foreach (ListItem wItem in lBoxWU.Items)
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

        protected void Date_Changed(object sender, EventArgs e)
        {
            try
            {
                populateEmployees(Menu1.SelectedItem.Value.Equals("0"));

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLDetailedDataPage.Date_Changed(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLDetailedDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void populateEmployees(bool isWU)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLDetailedDataPage).Assembly);

                lblError.Text = "";

                //check inserted date
                if (tbFromDate.Text.Trim().Equals("") || tbToDate.Text.Trim().Equals("") || tbFromTime.Text.Trim().Equals("") || tbToTime.Text.Trim().Equals(""))
                {
                    lblError.Text = rm.GetString("noDateInterval", culture);

                    if (tbFromDate.Text.Trim().Equals(""))
                        tbFromDate.Focus();
                    else if (tbFromTime.Text.Trim().Equals(""))
                        tbFromTime.Focus();
                    else if (tbToDate.Text.Trim().Equals(""))
                        tbToDate.Focus();
                    else if (tbToTime.Text.Trim().Equals(""))
                        tbToTime.Focus();
                    return;
                }

                DateTime fromDate = CommonWeb.Misc.createDate(tbFromDate.Text.Trim());
                DateTime toDate = CommonWeb.Misc.createDate(tbToDate.Text.Trim());
                DateTime fromTime = CommonWeb.Misc.createTime(tbFromTime.Text.Trim());
                DateTime toTime = CommonWeb.Misc.createTime(tbToTime.Text.Trim());
                DateTime from = new DateTime();
                DateTime to = new DateTime();

                //date validation
                if (fromDate.Equals(new DateTime()) || fromTime.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidFromDate", culture);
                    return;
                }

                if (toDate.Equals(new DateTime()) || toTime.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidToDate", culture);
                    return;
                }

                from = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, fromTime.Hour, fromTime.Minute, 0);
                to = new DateTime(toDate.Year, toDate.Month, toDate.Day, toTime.Hour, toTime.Minute, 0);

                if (from > to)
                {
                    lblError.Text = rm.GetString("invalidFromToDate", culture);
                    return;
                }

                List<EmployeeTO> empolyeeList = new List<EmployeeTO>();

                //int onlyEmplTypeID = -1;
                //int exceptEmplTypeID = -1;
                int emplID = -1;

                Dictionary<int, List<int>> companyVisibleTypes = new Dictionary<int, List<int>>();
                List<int> typesVisible = new List<int>();

                if (Session[Constants.sessionVisibleEmplTypes] != null && Session[Constants.sessionVisibleEmplTypes] is Dictionary<int, List<int>>)
                    companyVisibleTypes = (Dictionary<int, List<int>>)Session[Constants.sessionVisibleEmplTypes];

                // get companies
                string units = "";
                Dictionary<int, WorkingUnitTO> wuDict = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                int company = -1;
                if (isWU)
                {
                    int[] wuIndexes = lBoxWU.GetSelectedIndices();
                    foreach (int index in wuIndexes)
                    {
                        units += lBoxWU.Items[index].Value + ",";
                        int ID = -1;
                        if (int.TryParse(lBoxWU.Items[index].Value, out ID))
                        {
                            company = Common.Misc.getRootWorkingUnit(ID, wuDict);

                            if (companyVisibleTypes.ContainsKey(company))
                                typesVisible.AddRange(companyVisibleTypes[company]);
                        }
                    }

                    if (units.Length > 0)
                        units = units.Substring(0, units.Length - 1);
                }
                else
                {
                    int[] ouIndexes = lBoxOU.GetSelectedIndices();
                    foreach (int index in ouIndexes)
                    {
                        units += lBoxOU.Items[index].Value + ",";

                        int ID = -1;
                        if (int.TryParse(lBoxOU.Items[index].Value, out ID))
                        {
                            WorkingUnitXOrganizationalUnit wuXou = new WorkingUnitXOrganizationalUnit(Session[Constants.sessionConnection]);
                            wuXou.WUXouTO.OrgUnitID = ID;
                            List<WorkingUnitXOrganizationalUnitTO> list = wuXou.Search();
                            if (list.Count > 0)
                            {
                                company = Common.Misc.getRootWorkingUnit(list[0].WorkingUnitID, wuDict);
                                if (companyVisibleTypes.ContainsKey(company))
                                    typesVisible.AddRange(companyVisibleTypes[company]);
                            }
                        }
                    }

                    if (units.Length > 0)
                        units = units.Substring(0, units.Length - 1);
                }

                // 09.01.2012. Sanja - do not exclude login employee from reports
                //if (Session[Constants.sessionLogInEmployee] != null && Session[Constants.sessionLogInEmployee] is EmployeeTO)
                //    emplID = ((EmployeeTO)Session[Constants.sessionLogInEmployee]).EmployeeID;                

                if (isWU)
                {
                    if (!units.Trim().Equals("") && Session[Constants.sessionLoginCategoryWUnits] != null && Session[Constants.sessionLoginCategoryWUnits] is List<int>)
                    {
                        empolyeeList = new Employee(Session[Constants.sessionConnection]).SearchByWULoans(Common.Misc.getWorkingUnitHierarhicly(units, (List<int>)Session[Constants.sessionLoginCategoryWUnits], Session[Constants.sessionConnection]), emplID, typesVisible, from, to);
                    }
                }
                else
                {
                    if (!units.Trim().Equals(""))
                    {
                        if (Session[Constants.sessionLoginCategoryOUnits] != null && Session[Constants.sessionLoginCategoryOUnits] is List<int>)
                        {
                            empolyeeList = new Employee(Session[Constants.sessionConnection]).SearchByOU(Common.Misc.getOrgUnitHierarhicly(units.Trim(), (List<int>)Session[Constants.sessionLoginCategoryOUnits], Session[Constants.sessionConnection]), emplID, typesVisible, from, to);
                        }
                    }
                }

                lboxEmployees.DataSource = empolyeeList;
                lboxEmployees.DataTextField = "FirstAndLastName";
                lboxEmployees.DataValueField = "EmployeeID";
                lboxEmployees.DataBind();
                foreach (ListItem item in lboxEmployees.Items)
                {
                    item.Attributes.Add("title", item.Value);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        protected void lboxEmployees_PreRender(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem item in lboxEmployees.Items)
                {
                    item.Attributes.Add("title", item.Value);
                }
            }
            catch { }
        }

        private void populateOU(int ouID)
        {
            try
            {
                if (Session[Constants.sessionLoginCategoryOUnits] != null && Session[Constants.sessionLoginCategoryOUnits] is List<int>)
                {
                    List<OrganizationalUnitTO> oUnitsList = new OrganizationalUnit(Session[Constants.sessionConnection]).Search();
                    List<OrganizationalUnitTO> OUList = new List<OrganizationalUnitTO>();

                    foreach (OrganizationalUnitTO ou in oUnitsList)
                    {
                        if (((List<int>)Session[Constants.sessionLoginCategoryOUnits]).Contains(ou.OrgUnitID))
                            OUList.Add(ou);
                    }
                    //if (Session[Constants.sessionLogInEmployee] != null && Session[Constants.sessionLogInEmployee] is EmployeeTO)
                    //{
                    //    EmployeeTO empl = (EmployeeTO)Session[Constants.sessionLogInEmployee];
                    //    OrganizationalUnitTO org = new OrganizationalUnit(Session[Constants.sessionConnection]).Find(empl.OrgUnitID);
                    //    OUList.Add(org);
                    lBoxOU.DataSource = OUList;
                    lBoxOU.DataTextField = "Name";
                    lBoxOU.DataValueField = "OrgUnitID";
                    lBoxOU.DataBind();
                    //}

                    foreach (ListItem oItem in lBoxOU.Items)
                    {
                        if (oItem.Value.Trim().Equals(ouID.ToString().Trim()))
                        {
                            oItem.Selected = true;
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

        protected void cbSelectAllOU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem item in lBoxOU.Items)
                {
                    item.Selected = cbSelectAllOU.Checked;
                }

                int[] selOUIndex = lBoxOU.GetSelectedIndices();
                if (selOUIndex.Length > 0)
                {
                    int ouID = -1;

                    if (int.TryParse(lBoxOU.Items[selOUIndex[0]].Value.Trim(), out ouID))
                    {
                        Session[Constants.sessionOU] = ouID;
                        Session[Constants.sessionWU] = null;
                    }
                }

                populateEmployees(false);

                btnShow_Click(this, new EventArgs());

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLDetailedDataPage.cbSelectAllWU_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLDetailedDataPage.aspx".Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void lBoxOU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int[] selOUIndex = lBoxOU.GetSelectedIndices();
                if (selOUIndex.Length > 0)
                {
                    int ouID = -1;

                    if (int.TryParse(lBoxOU.Items[selOUIndex[0]].Value.Trim(), out ouID))
                    {
                        Session[Constants.sessionOU] = ouID;
                        Session[Constants.sessionWU] = null;
                    }
                }

                populateEmployees(false);
                populatePassTypes(false);
                cbSelectAllEmpolyees.Checked = false;

                btnShow_Click(this, new EventArgs());

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLDetailedDataPage.lBoxOU_SelectedIndexChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLDetailedDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void populatePassTypes(bool wu)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLDetailedDataPage).Assembly);

                InitializeSQLParameters();

                if (rbPassType.Checked)
                {
                    lblPassTypes.Text = rm.GetString("lblPassType", culture);
                    Dictionary<int, WorkingUnitTO> wUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                    bool isAltLang = !CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper());
                    List<PassTypeTO> passTypeList = new List<PassTypeTO>();
                    if (wu)
                    {
                        if (lBoxWU.SelectedIndex != -1)
                        {
                            passTypeList = new PassType(Session[Constants.sessionConnection]).SearchForCompany(Common.Misc.getRootWorkingUnit(int.Parse(lBoxWU.SelectedValue), wUnits), isAltLang);
                        }
                    }
                    else
                    {
                        if (lBoxOU.SelectedIndex != -1)
                        {
                            WorkingUnitXOrganizationalUnit wuXou = new WorkingUnitXOrganizationalUnit(Session[Constants.sessionConnection]);
                            wuXou.WUXouTO.OrgUnitID = int.Parse(lBoxOU.SelectedValue);
                            List<WorkingUnitXOrganizationalUnitTO> list = wuXou.Search();
                            if (list.Count > 0)
                            {
                                WorkingUnitXOrganizationalUnitTO wuXouTO = list[0];
                                passTypeList = new PassType(Session[Constants.sessionConnection]).SearchForCompany(Common.Misc.getRootWorkingUnit(wuXouTO.WorkingUnitID, wUnits), isAltLang);
                            }
                        }
                    }

                    lbPassTypes.DataSource = passTypeList;

                    if (!isAltLang)
                    {
                        lbPassTypes.DataTextField = "DescriptionAndID";
                    }
                    else
                    {
                        lbPassTypes.DataTextField = "DescriptionAltAndID";
                    }
                    lbPassTypes.DataValueField = "PassTypeID";
                    lbPassTypes.DataBind();
                }
                else
                {
                    lblPassTypes.Text = rm.GetString("lblLocations", culture);
                    List<LocationTO> locList = new Location(Session[Constants.sessionConnection]).Search();

                    lbPassTypes.DataSource = locList;

                    lbPassTypes.DataTextField = "Name";                    
                    lbPassTypes.DataValueField = "LocationID";
                    lbPassTypes.DataBind();
                }

                foreach (ListItem item in lbPassTypes.Items)
                {
                    item.Attributes.Add("title", item.Text);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void lbPassTypes_PreRender(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem item in lbPassTypes.Items)
                {
                    item.Attributes.Add("title", item.Text);
                }
            }
            catch { }
        }

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLDetailedDataPage).Assembly);

                lblEmployee.Text = rm.GetString("lblEmployeeSelection", culture);
                lblPeriod.Text = rm.GetString("lblPeriod", culture);
                lblPassTypes.Text = rm.GetString("lblPassType", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                
                cbSelectAllEmpolyees.Text = rm.GetString("lblSelectAll", culture);
                cbSelectAllPassTypes.Text = rm.GetString("lblSelectAll", culture);
                cbSelectAllWU.Text = rm.GetString("lblSelectAll", culture);
                cbSelectAllOU.Text = rm.GetString("lblSelectAll", culture);
                cbSelectAllWU.Text = rm.GetString("lblSelectAll", culture);
                cbMontlyTotals.Text = rm.GetString("lblMontlyTotals", culture);
                cbEmplTotals.Text = rm.GetString("cbEmplTotals", culture);

                rbPassType.Text = rm.GetString("rbPassType", culture);
                rbLocation.Text = rm.GetString("rbLocation", culture);
                
                btnShow.Text = rm.GetString("btnShow", culture);
                btnReport.Text = rm.GetString("btnReport", culture);
                btnReportNew.Text = rm.GetString("btnExport", culture);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void InitializeSQLParameters()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLDetailedDataPage).Assembly);

                ClearSessionValues();

                string header = rm.GetString("hdrEmployeeID", culture) + "," + rm.GetString("hdrEmployee", culture) + "," + rm.GetString("hdrCostCenterCode", culture) + ","
                    + rm.GetString("hdrCostCenterName", culture) + "," + rm.GetString("hdrWorkgroup", culture) + "," + rm.GetString("hdrUte", culture) + ","
                    + rm.GetString("lblBranch", culture).Remove(rm.GetString("lblBranch", culture).IndexOf(':')) + "," + rm.GetString("hdrEmplType", culture) + ","
                    + rm.GetString("hdrWeek", culture) + "," + rm.GetString("hdrYear", culture) + "," + rm.GetString("hdrMonth", culture) + "," + rm.GetString("hdrDay", culture) + ","
                    + rm.GetString("hdrStartTime", culture) + "," + rm.GetString("hdrEndTime", culture) + ",";

                if (rbPassType.Checked)
                    header += rm.GetString("hdrPassType", culture) + ",";
                else
                    header += rm.GetString("hdrLocation", culture) + ",";
                header += rm.GetString("hdrTotal", culture) + "," + rm.GetString("hdrDesc", culture) + "," + rm.GetString("hdrPairDate", culture);
                Session[Constants.sessionHeader] = header;
                //string fields = "e.employee_id AS empl_id|e.last_name +' '+ e.first_name AS first_name| wu.name AS cost_centre_code|wu.description AS cost_centre| SUBSTRING(ea.nvarchar_value_2,10,2) AS workgroup| SUBSTRING(ea.nvarchar_value_2,13,2) AS ute| ea.nvarchar_value_6 AS branch| et.employee_type_name AS empl_type|datepart(week,io.io_pair_date) AS date_week|datepart(year,io.io_pair_date) AS date_year|datepart(month,io.io_pair_date) AS date_month|datepart(day,io.io_pair_date) AS date_day| convert(time,io.start_time) AS start_time| convert(time,io.end_time) AS end_time|";

                //if (rbPassType.Checked)
                //{
                //    if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                //        fields += " '('+convert(Varchar,pt.payment_code)+') '+ pt.description AS pass_type ";
                //    else
                //        fields += " '('+convert(Varchar,pt.payment_code)+') '+ pt.description_alternative AS pass_type ";
                //}
                //else
                //    fields += " l.name AS location ";
                //fields += " | CONVERT(VARCHAR,end_time-start_time,108) AS total| io.description AS description";
                string fields = "empl_id, first_name, cost_centre_code, cost_centre, workgroup, ute, branch, empl_type, date_week, date_year, date_month, date_day, start_time, end_time, ";

                if (rbPassType.Checked)                
                        fields += "pass_type, ";
                
                else
                    fields += "location, ";
                fields += "total, description, pair_date";
                Session[Constants.sessionFields] = fields;
                Dictionary<int, int> formating = new Dictionary<int, int>();
                //formating.Add(2, (int)Constants.FormatTypes.DateTimeFormat);
                //formating.Add(3, (int)Constants.FormatTypes.DateTimeFormat);
                //formating.Add(4, (int)Constants.FormatTypes.DateTimeFormat);
                formating.Add(17, (int)Constants.FormatTypes.DateFormat);
                Session[Constants.sessionFieldsFormating] = formating;
                Session[Constants.sessionFieldsFormatedValues] = null;
                //Session[Constants.sessionTables] = "io_pairs_processed io, employees e, " + (rbPassType.Checked ? "pass_types pt" : "locations l") + ",employees_asco4 ea,employee_types et,working_units wu";
                Session[Constants.sessionDataTableList] = null;
                Session[Constants.sessionDataTableColumns] = null;
                Session[Constants.sessionKey] = null;
                Session[Constants.sessionColTypes] = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        {
            try
            {
                for (int i = 0; i < Menu1.Items.Count; i++)
                {
                    if (i == int.Parse(e.Item.Value))
                    {
                        Menu1.Items[i].Selected = true;
                        MultiView1.SetActiveView(MultiView1.Views[i]);
                    }

                    CommonWeb.Misc.setMenuImage(Menu1, i, Menu1.Items[i].Selected);
                    CommonWeb.Misc.setMenuSeparator(Menu1, i, Menu1.Items[i].Selected);
                }

                // value 0 - WU tab, value 1 - OU tab
                if (e.Item.Value.Equals("0"))
                {
                    int wuID = -1;
                    foreach (ListItem wItem in lBoxWU.Items)
                    {
                        if (wItem.Selected)
                        {
                            if (int.TryParse(wItem.Value.Trim(), out wuID))
                            {
                                Session[Constants.sessionWU] = wuID;
                                Session[Constants.sessionOU] = null;
                                break;
                            }
                        }
                    }

                    populateEmployees(true);
                }
                else
                {
                    int ouID = -1;
                    foreach (ListItem oItem in lBoxOU.Items)
                    {
                        if (oItem.Selected)
                        {
                            if (int.TryParse(oItem.Value.Trim(), out ouID))
                            {
                                Session[Constants.sessionOU] = ouID;
                                Session[Constants.sessionWU] = null;
                                break;
                            }
                        }
                    }

                    populateEmployees(false);
                }

                // save selected filter state
                SaveState();

                Session[Constants.sessionSamePage] = true;

                btnShow_Click(this, new EventArgs());

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLDetailedDataPage.Menu1_MenuItemClick(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLDetailedDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void ClearSessionValues()
        {
            try
            {
                if (Session[Constants.sessionFilter] != null)
                    Session[Constants.sessionFilter] = "";
                if (Session[Constants.sessionSortCol] != null)
                    Session[Constants.sessionSortCol] = "";
                if (Session[Constants.sessionSortDir] != null)
                    Session[Constants.sessionSortDir] = "";
                if (Session[Constants.sessionResultCurrentPage] != null)
                    Session[Constants.sessionResultCurrentPage] = null;

                Session[Constants.sessionItemsColors] = null;

                if (Session["emplIDs"] != null)
                    Session["emplIDs"] = null;
                Session["SessionReportFirstClick"] = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnReportNew_Click(Object sender, EventArgs e)
        {
            try
            {
                generate();
                method();
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLDetailedDataPage.btnReportNew_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLDetailedDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnShowOld_Click(Object sender, EventArgs e)
        {
            try
            {
                //Session["DetailedDataReportFilePath"] = null;
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLDetailedDataPage).Assembly);

                ClearSessionValues();
                CultureInfo ci = CultureInfo.InvariantCulture;
                string filter = "";
                lblError.Text = "";
                btnReport.Enabled = true;
                // check if there are some employees in list
                if (lboxEmployees.Items.Count <= 0)
                {
                    lblError.Text = rm.GetString("noEmployeesForReport", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                //check inserted date
                if (tbFromDate.Text.Trim().Equals("") || tbToDate.Text.Trim().Equals("") || tbFromTime.Text.Trim().Equals("") || tbToTime.Text.Trim().Equals(""))
                {
                    lblError.Text = rm.GetString("noDateInterval", culture);

                    if (tbFromDate.Text.Trim().Equals(""))
                        tbFromDate.Focus();
                    else if (tbFromTime.Text.Trim().Equals(""))
                        tbFromTime.Focus();
                    else if (tbToDate.Text.Trim().Equals(""))
                        tbToDate.Focus();
                    else if (tbToTime.Text.Trim().Equals(""))
                        tbToTime.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                DateTime fromDate = CommonWeb.Misc.createDate(tbFromDate.Text.Trim());
                DateTime toDate = CommonWeb.Misc.createDate(tbToDate.Text.Trim());
                DateTime fromTime = CommonWeb.Misc.createTime(tbFromTime.Text.Trim());
                DateTime toTime = CommonWeb.Misc.createTime(tbToTime.Text.Trim());
                DateTime from = new DateTime();
                DateTime to = new DateTime();

                //date validation
                if (fromDate.Equals(new DateTime()) || fromTime.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidFromDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (toDate.Equals(new DateTime()) || toTime.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidToDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                from = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, fromTime.Hour, fromTime.Minute, 0);
                to = new DateTime(toDate.Year, toDate.Month, toDate.Day, toTime.Hour, toTime.Minute, 0);

                if (from > to)
                {
                    lblError.Text = rm.GetString("invalidFromToDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                Session[Constants.sessionFromDate] = fromDate;
                Session[Constants.sessionToDate] = toDate;

                //create filter
                if (Session[Constants.sessionDataProvider].ToString().Trim().ToLower().Equals(Constants.dpSqlServer.Trim().ToLower()))
                    filter += "io.start_time >= '" + from.ToString(CommonWeb.Misc.getDateTimeFormatUniversal(), ci) + "' AND ";
                else if (Session[Constants.sessionDataProvider].ToString().Trim().ToLower().Equals(Constants.dpMySql.Trim().ToLower()))
                    filter += "DATE_FORMAT(io.start_time,'%Y-%m-%dT%h:%m:%s') >= '" + from.ToString(CommonWeb.Misc.getDateTimeFormatUniversal(), ci).Trim() + "' AND ";

                if (Session[Constants.sessionDataProvider].ToString().Trim().ToLower().Equals(Constants.dpSqlServer.Trim().ToLower()))
                    filter += "io.end_time <= '" + to.ToString(CommonWeb.Misc.getDateTimeFormatUniversal(), ci) + "' AND ";
                else if (Session[Constants.sessionDataProvider].ToString().Trim().ToLower().Equals(Constants.dpMySql.Trim().ToLower()))
                    filter += "DATE_FORMAT(io.end_time,'%Y-%m-%dT%h:%m:%s') <= '" + to.ToString(CommonWeb.Misc.getDateTimeFormatUniversal(), ci).Trim() + "' AND ";

                //filter for pass_types/locations
                int[] selPassTypeIndexes = lbPassTypes.GetSelectedIndices();
                if (selPassTypeIndexes.Length > 0)
                {
                    string pass_types = "";
                    foreach (int index in selPassTypeIndexes)
                    {
                        pass_types += lbPassTypes.Items[index].Value.Trim() + ",";
                    }

                    if (pass_types.Length > 0)
                        pass_types = pass_types.Substring(0, pass_types.Length - 1);

                    if (rbPassType.Checked)
                        filter += "io.pass_type_id IN (" + pass_types.Trim() + ") AND ";
                    else
                        filter += "io.location_id IN (" + pass_types.Trim() + ") AND ";
                }

                //filter for empolyees
                string emplIDs = "";
                if (lboxEmployees.GetSelectedIndices().Length > 0)
                {
                    List<string> idList = new List<string>();
                    foreach (int emplIndex in lboxEmployees.GetSelectedIndices())
                    {
                        if (emplIndex >= 0 && emplIndex < lboxEmployees.Items.Count)
                        {
                            idList.Add(lboxEmployees.Items[emplIndex].Value.Trim());
                            emplIDs += lboxEmployees.Items[emplIndex].Value.Trim() + ",";
                        }
                    }

                    Session[Constants.sessionSelectedEmplIDs] = idList;

                    if (emplIDs.Length > 0)
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                }
                else
                {
                    foreach (ListItem item in lboxEmployees.Items)
                    {
                        emplIDs += item.Value.Trim() + ",";
                    }

                    if (emplIDs.Length > 0)
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                }
                // get selected company
                int company = -1;
                Dictionary<int, WorkingUnitTO> WUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                if (Menu1.SelectedItem.Value.Equals("0"))
                {
                    int wuID = -1;
                    if (lBoxWU.SelectedValue != null)
                        if (!int.TryParse(lBoxWU.SelectedValue.Trim(), out wuID))
                            wuID = -1;

                    company = Common.Misc.getRootWorkingUnit(wuID, WUnits);
                }
                else
                {
                    int ouID = -1;
                    if (lBoxOU.SelectedValue != null)
                        if (!int.TryParse(lBoxOU.SelectedValue.Trim(), out ouID))
                            ouID = -1;

                    if (ouID >= 0)
                    {
                        WorkingUnitXOrganizationalUnit wuXou = new WorkingUnitXOrganizationalUnit(Session[Constants.sessionConnection]);
                        wuXou.WUXouTO.OrgUnitID = ouID;
                        List<WorkingUnitXOrganizationalUnitTO> list = wuXou.Search();
                        if (list.Count > 0)
                        {
                            WorkingUnitXOrganizationalUnitTO wuXouTO = list[0];
                            company = Common.Misc.getRootWorkingUnit(wuXouTO.WorkingUnitID, WUnits);
                        }
                        else
                            company = -1;
                    }
                    else
                        company = -1;
                }

                filter += "io.employee_id IN (" + emplIDs + ") AND  et.working_unit_id=" + company + " AND ";

                filter += "io.employee_id = e.employee_id AND e.employee_id = ea.employee_id AND e.employee_type_id = et.employee_type_id";
                if (rbPassType.Checked)
                    filter += " AND io.pass_type_id = pt.pass_type_id";
                else
                    filter += " AND io.location_id = l.location_id";
                filter += " AND wu.working_unit_id = (select working_unit_id from working_units where working_unit_id = (select parent_working_unit_id as id from working_units where working_unit_id = (select parent_working_unit_id from working_units where working_unit_id = e.working_unit_id)))";

                Session[Constants.sessionFilter] = filter;
                Session[Constants.sessionSortCol] = "io.io_pair_date";
                Session[Constants.sessionSortDir] = Constants.sortASC;

                Session["emplIDs"] = emplIDs;

                // save selected filter state
                SaveState();

                resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?showSelection=false";

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLDetailedDataPage.btnShow_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLDetailedDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnShow_Click(Object sender, EventArgs e)
        {
            try
            {
                //Session["DetailedDataReportFilePath"] = null;
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLDetailedDataPage).Assembly);

                ClearSessionValues();
                CultureInfo ci = CultureInfo.InvariantCulture;
                //string filter = "";
                lblError.Text = "";
                btnReport.Enabled = true;
                // check if there are some employees in list
                if (lboxEmployees.Items.Count <= 0)
                {
                    lblError.Text = rm.GetString("noEmployeesForReport", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                //check inserted date
                if (tbFromDate.Text.Trim().Equals("") || tbToDate.Text.Trim().Equals("") || tbFromTime.Text.Trim().Equals("") || tbToTime.Text.Trim().Equals(""))
                {
                    lblError.Text = rm.GetString("noDateInterval", culture);

                    if (tbFromDate.Text.Trim().Equals(""))
                        tbFromDate.Focus();
                    else if (tbFromTime.Text.Trim().Equals(""))
                        tbFromTime.Focus();
                    else if (tbToDate.Text.Trim().Equals(""))
                        tbToDate.Focus();
                    else if (tbToTime.Text.Trim().Equals(""))
                        tbToTime.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                DateTime fromDate = CommonWeb.Misc.createDate(tbFromDate.Text.Trim());
                DateTime toDate = CommonWeb.Misc.createDate(tbToDate.Text.Trim());
                DateTime fromTime = CommonWeb.Misc.createTime(tbFromTime.Text.Trim());
                DateTime toTime = CommonWeb.Misc.createTime(tbToTime.Text.Trim());
                DateTime from = new DateTime();
                DateTime to = new DateTime();

                //date validation
                if (fromDate.Equals(new DateTime()) || fromTime.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidFromDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (toDate.Equals(new DateTime()) || toTime.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidToDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                from = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, fromTime.Hour, fromTime.Minute, 0);
                to = new DateTime(toDate.Year, toDate.Month, toDate.Day, toTime.Hour, toTime.Minute, 0);

                if (from > to)
                {
                    lblError.Text = rm.GetString("invalidFromToDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                Session[Constants.sessionFromDate] = fromDate;
                Session[Constants.sessionToDate] = toDate;

                //filter for pass_types/locations
                List<int> ptLocList = new List<int>();
                int[] selPassTypeIndexes = lbPassTypes.GetSelectedIndices();
                if (selPassTypeIndexes.Length > 0)
                {
                    foreach (int index in selPassTypeIndexes)
                    {
                        int selID = -1;
                        if (int.TryParse(lbPassTypes.Items[index].Value.Trim(), out selID) && !ptLocList.Contains(selID))
                            ptLocList.Add(selID);
                    }
                }

                //filter for empolyees
                string emplIDs = "";
                if (lboxEmployees.GetSelectedIndices().Length > 0)
                {
                    List<string> idList = new List<string>();
                    foreach (int emplIndex in lboxEmployees.GetSelectedIndices())
                    {
                        if (emplIndex >= 0 && emplIndex < lboxEmployees.Items.Count)
                        {
                            idList.Add(lboxEmployees.Items[emplIndex].Value.Trim());
                            emplIDs += lboxEmployees.Items[emplIndex].Value.Trim() + ",";
                        }
                    }

                    Session[Constants.sessionSelectedEmplIDs] = idList;

                    if (emplIDs.Length > 0)
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                }
                else
                {
                    foreach (ListItem item in lboxEmployees.Items)
                    {
                        emplIDs += item.Value.Trim() + ",";
                    }

                    if (emplIDs.Length > 0)
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                }

                // get selected company
                int company = -1;
                Dictionary<int, WorkingUnitTO> WUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                if (Menu1.SelectedItem.Value.Equals("0"))
                {
                    int wuID = -1;
                    if (lBoxWU.SelectedValue != null)
                        if (!int.TryParse(lBoxWU.SelectedValue.Trim(), out wuID))
                            wuID = -1;

                    company = Common.Misc.getRootWorkingUnit(wuID, WUnits);
                }
                else
                {
                    int ouID = -1;
                    if (lBoxOU.SelectedValue != null)
                        if (!int.TryParse(lBoxOU.SelectedValue.Trim(), out ouID))
                            ouID = -1;

                    if (ouID >= 0)
                    {
                        WorkingUnitXOrganizationalUnit wuXou = new WorkingUnitXOrganizationalUnit(Session[Constants.sessionConnection]);
                        wuXou.WUXouTO.OrgUnitID = ouID;
                        List<WorkingUnitXOrganizationalUnitTO> list = wuXou.Search();
                        if (list.Count > 0)
                        {
                            WorkingUnitXOrganizationalUnitTO wuXouTO = list[0];
                            company = Common.Misc.getRootWorkingUnit(wuXouTO.WorkingUnitID, WUnits);
                        }
                        else
                            company = -1;
                    }
                    else
                        company = -1;
                }

                // get all employee types, key is employee_type, value name for that company
                Dictionary<int, string> emplTypes = new Dictionary<int, string>();

                if (company != -1)
                {
                    EmployeeType eType = new EmployeeType(Session[Constants.sessionConnection]);
                    eType.EmployeeTypeTO.WorkingUnitID = company;
                    List<EmployeeTypeTO> emplTypeList = eType.Search();

                    foreach (EmployeeTypeTO type in emplTypeList)
                    {
                        if (!emplTypes.ContainsKey(type.EmployeeTypeID))
                            emplTypes.Add(type.EmployeeTypeID, type.EmployeeTypeName.Trim());
                        else
                            emplTypes[type.EmployeeTypeID] = type.EmployeeTypeName.Trim();
                    }
                }

                // get selected employees
                List<EmployeeTO> emplList = new Employee(Session[Constants.sessionConnection]).Search(emplIDs);

                Dictionary<int, EmployeeTO> employees = new Dictionary<int, EmployeeTO>();                
                foreach (EmployeeTO empl in emplList)
                {
                    if (!employees.ContainsKey(empl.EmployeeID))
                        employees.Add(empl.EmployeeID, empl);
                    else
                        employees[empl.EmployeeID] = empl;
                }

                //costcenter, ute, workgruop, branch
                List<EmployeeAsco4TO> emplAscoList = new EmployeeAsco4(Session[Constants.sessionConnection]).Search(emplIDs);
                Dictionary<int, string> branchList = new Dictionary<int, string>();
                Dictionary<int, string> uteList = new Dictionary<int, string>();
                Dictionary<int, string> workgroupList = new Dictionary<int, string>();
                Dictionary<int, string> costcenterList = new Dictionary<int, string>();
                Dictionary<int, string> costcenterListDesc = new Dictionary<int, string>();

                foreach (EmployeeAsco4TO asco in emplAscoList)
                {
                    EmployeeTO empl = new EmployeeTO();

                    if (employees.ContainsKey(asco.EmployeeID))
                        empl = employees[asco.EmployeeID];
                                        
                    WorkingUnitTO tempWU = new WorkingUnitTO();
                    if (WUnits.ContainsKey(empl.WorkingUnitID))
                        tempWU = WUnits[empl.WorkingUnitID];
                    
                    if (!branchList.ContainsKey(asco.EmployeeID))
                        branchList.Add(asco.EmployeeID, asco.NVarcharValue6.Trim());
                    else
                        branchList[asco.EmployeeID] = asco.NVarcharValue6.Trim();

                    if (!uteList.ContainsKey(asco.EmployeeID))
                        uteList.Add(asco.EmployeeID, tempWU.Code.Trim());                    
                    else
                        uteList[asco.EmployeeID] = tempWU.Code.Trim();

                    if (WUnits.ContainsKey(tempWU.ParentWorkingUID))
                        tempWU = WUnits[tempWU.ParentWorkingUID];

                    if (!workgroupList.ContainsKey(asco.EmployeeID))                    
                        workgroupList.Add(asco.EmployeeID, tempWU.Code.Trim());                    
                    else                    
                        workgroupList[asco.EmployeeID] = tempWU.Code.Trim();

                    if (WUnits.ContainsKey(tempWU.ParentWorkingUID))
                        tempWU = WUnits[tempWU.ParentWorkingUID];

                    string cost = tempWU.Name.Trim();
                    string costDesc = tempWU.Description.Trim();
                    if (!costcenterList.ContainsKey(asco.EmployeeID))                        
                        costcenterList.Add(asco.EmployeeID, cost);                    
                    else
                        costcenterList[asco.EmployeeID] = cost;

                    if (!costcenterListDesc.ContainsKey(asco.EmployeeID))
                        costcenterListDesc.Add(asco.EmployeeID, costDesc);
                    else
                        costcenterListDesc[asco.EmployeeID] = costDesc;     
                }

                Dictionary<int, PassTypeTO> ptDict = new PassType(Session[Constants.sessionConnection]).SearchDictionary();
                Dictionary<int, WorkTimeSchemaTO> schemas = new TimeSchema(Session[Constants.sessionConnection]).getDictionary();
                Dictionary<int, LocationTO> locDict = new Location(Session[Constants.sessionConnection]).SearchDict();
                
                // get pairs for one more day becouse of third shifts
                List<DateTime> dateList = new List<DateTime>();
                DateTime currentDate = from.Date;
                while (currentDate.Date <= to.AddDays(1).Date)
                {
                    dateList.Add(currentDate.Date);
                    currentDate = currentDate.AddDays(1);
                }
                List<IOPairProcessedTO> allPairs = new IOPairProcessed().SearchAllPairsForEmpl(emplIDs, dateList, "");

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

                // get schemas and intervals
                Dictionary<int, Dictionary<DateTime, WorkTimeSchemaTO>> emplDaySchemas = new Dictionary<int, Dictionary<DateTime, WorkTimeSchemaTO>>();
                Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>> emplDayIntervals = new Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>>();

                // get schedules for selected employees and date interval
                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedulesExactDate(emplIDs, from.Date, to.Date.AddDays(1), null);
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

                List<List<object>> resultTable = new List<List<object>>();

                // create table for result and list of pairs                
                List<DataColumn> resultColumns = new List<DataColumn>();                
                resultColumns.Add(new DataColumn("empl_id", typeof(int)));
                resultColumns.Add(new DataColumn("first_name", typeof(string)));
                resultColumns.Add(new DataColumn("cost_centre_code", typeof(string)));
                resultColumns.Add(new DataColumn("cost_centre", typeof(string)));
                resultColumns.Add(new DataColumn("workgroup", typeof(string)));
                resultColumns.Add(new DataColumn("ute", typeof(string)));
                resultColumns.Add(new DataColumn("branch", typeof(string)));
                resultColumns.Add(new DataColumn("empl_type", typeof(string)));
                resultColumns.Add(new DataColumn("date_week", typeof(int)));
                resultColumns.Add(new DataColumn("date_year", typeof(int)));
                resultColumns.Add(new DataColumn("date_month", typeof(int)));
                resultColumns.Add(new DataColumn("date_day", typeof(int)));
                resultColumns.Add(new DataColumn("start_time", typeof(TimeSpan)));
                resultColumns.Add(new DataColumn("end_time", typeof(TimeSpan)));
                if (rbPassType.Checked)
                {
                    resultColumns.Add(new DataColumn("pass_type", typeof(string)));
                }
                else
                {
                    resultColumns.Add(new DataColumn("location", typeof(string)));
                }
                resultColumns.Add(new DataColumn("total", typeof(TimeSpan)));                
                resultColumns.Add(new DataColumn("description", typeof(string)));
                resultColumns.Add(new DataColumn("pair_date", typeof(DateTime)));
                                
                foreach (IOPairProcessedTO pair in allPairs)
                {
                    if (pair.EndTime <= from || pair.StartTime >= to)
                        continue;

                    if (ptLocList.Count > 0)
                    {
                        if ((rbPassType.Checked && !ptLocList.Contains(pair.PassTypeID)) || (rbLocation.Checked && !ptLocList.Contains(pair.LocationID)))
                            continue;
                    }

                    DateTime pairDate = pair.IOPairDate.Date;

                    List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                    if (emplDayPairs.ContainsKey(pair.EmployeeID) && emplDayPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                        dayPairs = emplDayPairs[pair.EmployeeID][pair.IOPairDate.Date];

                    WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                    if (emplDaySchemas.ContainsKey(pair.EmployeeID) && emplDaySchemas[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                        sch = emplDaySchemas[pair.EmployeeID][pair.IOPairDate.Date];

                    List<WorkTimeIntervalTO> dayIntervals = new List<WorkTimeIntervalTO>();
                    if (emplDayIntervals.ContainsKey(pair.EmployeeID) && emplDayIntervals[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                        dayIntervals = emplDayIntervals[pair.EmployeeID][pair.IOPairDate.Date];

                    bool previousDayPair = Common.Misc.isPreviousDayPair(pair, ptDict, dayPairs, sch, dayIntervals);

                    if (previousDayPair)
                        pairDate = pairDate.AddDays(-1).Date;

                    // create result row                    
                    List<object> resultRow = new List<object>();
                    // employee id
                    resultRow.Add(pair.EmployeeID);
                    // employee name
                    if (employees.ContainsKey(pair.EmployeeID))
                        resultRow.Add(employees[pair.EmployeeID].FirstAndLastName.Trim());
                    else
                        resultRow.Add("");
                    if (costcenterList.ContainsKey(pair.EmployeeID))
                        resultRow.Add(costcenterList[pair.EmployeeID].Trim());
                    else
                        resultRow.Add("");
                    if (costcenterListDesc.ContainsKey(pair.EmployeeID))
                        resultRow.Add(costcenterListDesc[pair.EmployeeID].Trim());
                    else
                        resultRow.Add("");
                    if (workgroupList.ContainsKey(pair.EmployeeID))
                        resultRow.Add(workgroupList[pair.EmployeeID].Trim());
                    else
                        resultRow.Add("");
                    if (uteList.ContainsKey(pair.EmployeeID))
                        resultRow.Add(uteList[pair.EmployeeID].Trim());
                    else
                        resultRow.Add("");
                    if (branchList.ContainsKey(pair.EmployeeID))
                        resultRow.Add(branchList[pair.EmployeeID].Trim());
                    else
                        resultRow.Add("");
                    // employee type
                    if (employees.ContainsKey(pair.EmployeeID) && emplTypes.ContainsKey(employees[pair.EmployeeID].EmployeeTypeID))
                        resultRow.Add(emplTypes[employees[pair.EmployeeID].EmployeeTypeID].Trim());
                    else
                        resultRow.Add("");
                    resultRow.Add(getDateWeekOfYear(pair.IOPairDate.Date));
                    resultRow.Add(pair.IOPairDate.Year);
                    resultRow.Add(pair.IOPairDate.Month);
                    resultRow.Add(pair.IOPairDate.Day);
                    resultRow.Add(pair.StartTime.TimeOfDay);
                    resultRow.Add(pair.EndTime.TimeOfDay);

                    if (rbPassType.Checked)
                    {
                        if (ptDict.ContainsKey(pair.PassTypeID))
                        {
                            if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                                resultRow.Add(ptDict[pair.PassTypeID].DescriptionAndID.Trim());
                            else
                                resultRow.Add(ptDict[pair.PassTypeID].DescriptionAltAndID.Trim());
                        }
                        else
                            resultRow.Add("");
                    }
                    else
                    {
                        if (locDict.ContainsKey(pair.LocationID))
                            resultRow.Add(locDict[pair.LocationID].Name.Trim());
                        else
                            resultRow.Add("");
                    }

                    TimeSpan pairDuration = pair.EndTime.TimeOfDay.Subtract(pair.StartTime.TimeOfDay);
                    if (pair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                        pairDuration = pairDuration.Add(new TimeSpan(0, 1, 0));
                    resultRow.Add(pairDuration);
                    resultRow.Add(pair.Desc.Trim());
                    resultRow.Add(pairDate.Date);

                    resultTable.Add(resultRow);
                }

                Session[Constants.sessionSortCol] = "pair_date";
                Session[Constants.sessionSortDir] = Constants.sortASC;

                Session["emplIDs"] = emplIDs;

                Session[Constants.sessionDataTableList] = resultTable;
                Session[Constants.sessionDataTableColumns] = resultColumns;

                // save selected filter state
                SaveState();

                resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?showSelection=false";

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLDetailedDataPage.btnShow_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLDetailedDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private int getDateWeekOfYear(DateTime date)
        {
            try
            {
                DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                System.Globalization.Calendar cal = dfi.Calendar;

                return cal.GetWeekOfYear(date, dfi.CalendarWeekRule, DayOfWeek.Monday);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnReportOld_Click(Object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLDetailedDataPage).Assembly);

                SaveState();

                lblError.Text = "";
                int rowCount = 0;
                DataTable ioPairs = null;
                if (cbEmplTotals.Checked)
                    Session["TLDetailedDataPage.checked"] = 2;
                else
                {
                    if (cbMontlyTotals.Checked)
                        Session["TLDetailedDataPage.checked"] = 1;
                    else
                        Session["TLDetailedDataPage.checked"] = 0;
                }

                if (Session[Constants.sessionFields] != null && !Session[Constants.sessionFields].ToString().Trim().Equals("")
                    && Session[Constants.sessionTables] != null && !Session[Constants.sessionTables].ToString().Trim().Equals("")
                    && Session[Constants.sessionFilter] != null && !Session[Constants.sessionFilter].ToString().Trim().Equals("")
                    && Session[Constants.sessionSortCol] != null && !Session[Constants.sessionSortCol].ToString().Trim().Equals("")
                    && Session[Constants.sessionSortDir] != null && !Session[Constants.sessionSortDir].ToString().Trim().Equals(""))
                {
                    Result result = new Result(Session[Constants.sessionConnection]);
                    rowCount = result.SearchResultCount(Session[Constants.sessionTables].ToString().Trim(), Session[Constants.sessionFilter].ToString().Trim());
                    if (rowCount > 30000)
                    {
                        btnReport.Enabled = false;
                        lblError.Text = rm.GetString("largeReportDisabled", culture);
                        return;
                    }
                    else if (rowCount > 10000 && (Session["SessionReportFirstClick"] == null))
                    {
                        lblError.Text = rm.GetString("largeReport", culture);
                        Session["SessionReportFirstClick"] = "2";
                        return;
                    }

                    if (rowCount > 0)
                    {
                        // get all passes for search criteria for report
                        ioPairs = new Result(Session[Constants.sessionConnection]).SearchResultTable(Session[Constants.sessionFields].ToString().Trim(), Session[Constants.sessionTables].ToString().Trim(), Session[Constants.sessionFilter].ToString().Trim(),
                        Session[Constants.sessionSortCol].ToString().Trim(), Session[Constants.sessionSortDir].ToString().Trim(), 1, rowCount);

                        // Table Definition for  Reports
                        DataSet dataSetCR = new DataSet();
                        DataTable tableCR = new DataTable("io_pairs");
                        DataTable tableI = new DataTable("images");

                        tableCR.Columns.Add("first_name", typeof(System.String));
                        tableCR.Columns.Add("last_name", typeof(System.String));
                        tableCR.Columns.Add("date_week", typeof(System.String));
                        tableCR.Columns.Add("date_year", typeof(System.String));
                        tableCR.Columns.Add("date_month", typeof(System.String));
                        tableCR.Columns.Add("date_day", typeof(System.String));
                        tableCR.Columns.Add("branch", typeof(System.String));
                        tableCR.Columns.Add("cost_centre_code", typeof(System.String));
                        tableCR.Columns.Add("cost_centre", typeof(System.String));
                        tableCR.Columns.Add("workgroup", typeof(System.String));
                        tableCR.Columns.Add("ute", typeof(System.String));
                        tableCR.Columns.Add("empl_type", typeof(System.String));
                        tableCR.Columns.Add("start_time", typeof(System.TimeSpan));
                        tableCR.Columns.Add("end_time", typeof(System.TimeSpan));
                        tableCR.Columns.Add("total", typeof(System.Double));
                        tableCR.Columns.Add("pass_type", typeof(System.String));
                        tableCR.Columns.Add("description", typeof(System.String));
                        tableCR.Columns.Add("imageID", typeof(byte));
                        tableCR.Columns.Add("month", typeof(System.DateTime));
                        tableCR.Columns.Add("changedMonth", typeof(System.String));
                        tableCR.Columns.Add("empl_id", typeof(System.String));
                        tableCR.Columns.Add("shift", typeof(System.String));
                        tableCR.Columns.Add("cycle_day", typeof(System.String));
                        tableCR.Columns.Add("intervals", typeof(System.String));
                        tableI.Columns.Add("imageID", typeof(byte));
                        tableI.Columns.Add("image", typeof(System.Byte[]));

                        //add logo image just once
                        DataRow rowI = tableI.NewRow();
                        rowI["image"] = Constants.LogoForReport;
                        rowI["imageID"] = 1;
                        tableI.Rows.Add(rowI);
                        tableI.AcceptChanges();

                        dataSetCR.Tables.Add(tableCR);
                        dataSetCR.Tables.Add(tableI);

                        int monthIndex = 1;
                        List<string> listMonth = new List<string>();
                        string emplIDs = Session["emplIDs"].ToString();
                        //foreach (DataRow row in ioPairs.Rows)
                        //{
                        //    string emplID = row["empl_id"].ToString() + ",";


                        //}
                        //if (emplIDs.Length > 0)
                        //    emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                        DateTime fromDate = (DateTime)Session[Constants.sessionFromDate];
                        DateTime toDate = (DateTime)Session[Constants.sessionToDate];
                        Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule(Session[Constants.sessionConnection]).SearchEmployeesSchedulesExactDate(emplIDs, fromDate, toDate, null);
                        Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4(Session[Constants.sessionConnection]).SearchDictionary(emplIDs);
                        Dictionary<int, WorkTimeSchemaTO> schemas = new TimeSchema(Session[Constants.sessionConnection]).getDictionary();
                        // populate dataset
                        foreach (DataRow ioPair in ioPairs.Rows)
                        {
                            int empl = -1;
                            int day = -1;
                            int month = -1;
                            int year = -1;
                            DataRow row = tableCR.NewRow();
                            if (ioPair["empl_id"] != DBNull.Value)
                            {
                                row["empl_id"] = ioPair["empl_id"].ToString().Trim() + ".";
                                empl = int.Parse(ioPair["empl_id"].ToString());
                            }
                            if (ioPair["date_week"] != DBNull.Value)
                                row["date_week"] = ioPair["date_week"].ToString().Trim() + ".";
                            if (ioPair["date_year"] != DBNull.Value)
                            {
                                row["date_year"] = ioPair["date_year"].ToString().Trim() + ".";
                                year = int.Parse(ioPair["date_year"].ToString().Trim());
                            }
                            if (ioPair["date_month"] != DBNull.Value)
                            {
                                row["date_month"] = ioPair["date_month"].ToString().Trim() + ".";
                                month = int.Parse(ioPair["date_month"].ToString().Trim());
                            }
                            if (ioPair["date_day"] != DBNull.Value)
                            {
                                row["date_day"] = ioPair["date_day"].ToString().Trim() + ".";
                                day = int.Parse(ioPair["date_day"].ToString().Trim());
                            }
                            if (ioPair["first_name"] != DBNull.Value)
                                row["first_name"] = ioPair["first_name"].ToString().Trim();
                            if (ioPair["branch"] != DBNull.Value)
                                row["branch"] = ioPair["branch"].ToString().Trim();
                            if (ioPair["cost_centre"] != DBNull.Value)
                                row["cost_centre"] = ioPair["cost_centre"].ToString().Trim();
                            if (ioPair["cost_centre_code"] != DBNull.Value)
                                row["cost_centre_code"] = ioPair["cost_centre_code"].ToString().Trim();
                            if (ioPair["empl_type"] != DBNull.Value)
                                row["empl_type"] = ioPair["empl_type"].ToString().Trim();
                            if (ioPair["start_time"] != DBNull.Value)
                                row["start_time"] = ioPair["start_time"].ToString().Trim();
                            if (ioPair["end_time"] != DBNull.Value)
                                row["end_time"] = ioPair["end_time"].ToString().Trim();
                            if (ioPair["workgroup"] != DBNull.Value)
                                row["workgroup"] = ioPair["workgroup"].ToString().Trim();
                            if (ioPair["ute"] != DBNull.Value)
                                row["ute"] = ioPair["ute"].ToString().Trim();

                            List<EmployeeTimeScheduleTO> schedules = new List<EmployeeTimeScheduleTO>();
                            DateTime ioPairDate = new DateTime();

                            if (year != -1 && month != -1 && day != -1) { ioPairDate = new DateTime(year, month, day); }
                            if (empl != -1)
                            {
                                if (emplSchedules.ContainsKey(empl))
                                    schedules = emplSchedules[empl];

                                WorkTimeSchemaTO sch = Common.Misc.getTimeSchema(ioPairDate, schedules, schemas);

                                if (sch.TimeSchemaID != -1)
                                    row["shift"] = sch.Description.Trim();
                                else
                                    row["shift"] = "N/A";

                                List<WorkTimeIntervalTO> intervals = Common.Misc.getTimeSchemaInterval(ioPairDate, schedules, schemas);

                                int cycleDay = 0;
                                string intervalsString = "";
                                foreach (WorkTimeIntervalTO interval in intervals)
                                {
                                    cycleDay = interval.DayNum + 1;

                                    intervalsString += interval.StartTime.ToString(Constants.timeFormat) + "-" + interval.EndTime.ToString(Constants.timeFormat);

                                    if (!interval.Description.Trim().Equals(""))
                                        intervalsString += "(" + interval.Description.Trim() + ")";

                                    intervalsString += "; ";
                                }

                                row["cycle_day"] = cycleDay.ToString().Trim();
                                row["intervals"] = intervalsString.Trim();
                            }

                            CultureInfo cultureInv = CultureInfo.InvariantCulture;
                            if (ioPair["total"] != DBNull.Value)
                            {
                                string hours = ioPair["total"].ToString().Remove(ioPair["total"].ToString().IndexOf(':'));
                                string minutes = ioPair["total"].ToString().Substring(ioPair["total"].ToString().IndexOf(':') + 1);
                                minutes = minutes.Remove(minutes.IndexOf(':'));

                                decimal minute = (decimal)(int.Parse(minutes)) / (decimal)60;
                                //minute = Math.Round(minute,2);

                                int hour = int.Parse(hours);

                                double num = (double)hour + (double)minute;
                                num = Math.Round(num, 2);

                                row["total"] = num;
                            }

                            if (rbPassType.Checked)
                            {
                                if (ioPair["pass_type"] != DBNull.Value)
                                    row["pass_type"] = ioPair["pass_type"].ToString().Trim();
                            }
                            else
                            {
                                if (ioPair["location"] != DBNull.Value)
                                    row["pass_type"] = ioPair["location"].ToString().Trim();
                            }
                            if (ioPair["description"] != DBNull.Value)
                                row["description"] = ioPair["description"].ToString().Trim();
                            int yearDate = int.Parse(row["date_year"].ToString().Remove(row["date_year"].ToString().LastIndexOf('.')));
                            int monthDate = int.Parse(row["date_month"].ToString().Remove(row["date_month"].ToString().LastIndexOf('.')));

                            DateTime dtime = new DateTime(yearDate, monthDate, 1, 0, 0, 0);
                            row["month"] = new DateTime(dtime.Year, dtime.Month, 1);

                            if (!listMonth.Contains(dtime.Month + "." + dtime.Year))
                                listMonth.Add(dtime.Month + "." + dtime.Year);

                            if (ioPairs.Rows.Count - 1 > monthIndex - 1)
                            {
                                string date_year = ioPairs.Rows[monthIndex]["date_year"].ToString();
                                int year1 = int.Parse(date_year);
                                int month1 = int.Parse(ioPairs.Rows[monthIndex]["date_month"].ToString());
                                int day1 = int.Parse(ioPairs.Rows[monthIndex]["date_day"].ToString());

                                DateTime date = new DateTime(year1, month1, day1, 0, 0, 0);

                                if (dtime.Month != date.Month)
                                {
                                    row["changedMonth"] = "1";
                                }
                                else
                                {
                                    row["changedMonth"] = "0";
                                }
                                if (ioPairs.Rows[0] == ioPair)
                                {
                                    //    previous = dtime.Month;
                                    if (ioPairs.Rows.Count == 1)
                                        row["changedMonth"] = "1";
                                    else
                                        row["changedMonth"] = "0";
                                }
                                monthIndex++;
                            }
                            else if (ioPairs.Rows.Count == monthIndex)
                            {
                                row["changedMonth"] = "1";
                            }

                            row["imageID"] = 1;

                            if (ioPairs.Rows[ioPairs.Rows.Count - 1] == ioPair)
                            {
                                DateTime from = CommonWeb.Misc.createDate(tbFromDate.Text);
                                DateTime to = CommonWeb.Misc.createDate(tbToDate.Text);
                                if (from.Month == to.Month)
                                    row["changedMonth"] = "1";
                            }
                            tableCR.Rows.Add(row);
                            tableCR.AcceptChanges();
                        }

                        if (tableCR.Rows.Count == 0)
                        {
                            lblError.Text = rm.GetString("noReportData", culture);
                            writeLog(DateTime.Now, false);
                            return;
                        }

                        string pass_type = "*";
                        string start_time = "";
                        string end_time = "";
                        string empolyee = "*";

                        //gate parameter for report
                        if (lbPassTypes.SelectedIndex > 0)
                        {
                            for (int intGates = 0; intGates < lbPassTypes.Items.Count; intGates++)
                            {
                                if (lbPassTypes.Items[intGates].Selected)
                                {
                                    pass_type = pass_type + ", " + lbPassTypes.Items[intGates].ToString();
                                }
                            }
                            pass_type = pass_type.Substring(pass_type.IndexOf(',') + 1);
                        }
                        if (pass_type == "")
                        {
                            for (int intGates = 0; intGates < lbPassTypes.Items.Count; intGates++)
                            {
                                pass_type = pass_type + ", " + lbPassTypes.Items[intGates].ToString();
                            }
                            pass_type = pass_type.Substring(pass_type.IndexOf(',') + 1);
                        }

                        //employee parameter for report
                        if (lboxEmployees.SelectedIndex > 0)
                        {
                            empolyee = "";
                            for (int intEmpolyees = 0; intEmpolyees < lboxEmployees.Items.Count; intEmpolyees++)
                            {
                                if (lboxEmployees.Items[intEmpolyees].Selected)
                                {
                                    empolyee = empolyee + ", " + lboxEmployees.Items[intEmpolyees].ToString();
                                }
                            }

                            empolyee = empolyee.Substring(empolyee.IndexOf(',') + 1);
                        }

                        if (empolyee == "")
                        {
                            for (int intEmpolyees = 0; intEmpolyees < lboxEmployees.Items.Count; intEmpolyees++)
                            {
                                empolyee = empolyee + ", " + lboxEmployees.Items[intEmpolyees].ToString();
                            }
                            empolyee = empolyee.Substring(empolyee.IndexOf(',') + 1);
                        }

                        //fromDate parameter for report
                        if (tbFromDate.Text != "")
                        {
                            start_time = tbFromDate.Text;
                        }
                        //toDate parameter for report
                        if (tbToDate.Text != "")
                        {
                            end_time = tbToDate.Text;
                        }

                        DataTable dataTableMonthly = new DataTable("io_pairs_monthly_pt");
                        dataTableMonthly.Columns.Add("total", typeof(System.Double));
                        dataTableMonthly.Columns.Add("month", typeof(System.String));
                        dataTableMonthly.Columns.Add("pass_type", typeof(System.String));
                        dataTableMonthly.Columns.Add("employee", typeof(System.String));
                        dataSetCR.Tables.Add(dataTableMonthly);

                        if (cbEmplTotals.Checked)
                            CalcEmplMonthlyTotalsOld(ioPairs, dataTableMonthly);                        
                        else if (cbMontlyTotals.Checked)
                        {
                            foreach (string monthInt in listMonth)
                            {
                                CalcMonthlyTotalsOld(ioPairs, monthInt, dataTableMonthly);
                            }
                        }

                        //send to session parameters for report
                        if (rbPassType.Checked)
                            Session["TLDetailedDataPage.pt_loc"] = rm.GetString("hdrPassType", culture);
                        else
                            Session["TLDetailedDataPage.pt_loc"] = rm.GetString("hdrLocation", culture);
                        Session["TLDetailedDataPage.pass_type"] = pass_type;
                        Session["TLDetailedDataPage.employee"] = empolyee;
                        Session["TLDetailedDataPage.start_time"] = start_time;
                        Session["TLDetailedDataPage.end_time"] = end_time;
                        Session["TLDetailedDataPage.io_pairs"] = dataSetCR;
                        Session[Constants.sessionReportName] = rm.GetString("lblPassesReport", culture);
                        Session["DetailedDataReportFilePath"] = null;
                        //check language and redirect to apropriate report
                        string reportURL = "";
                        if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                            reportURL = "/ACTAWeb/ReportsWeb/sr/TLDetailedDataReport_sr.aspx";
                        else
                            reportURL = "/ACTAWeb/ReportsWeb/en/TLDetailedDataReport_en.aspx";
                        Response.Redirect("/ACTAWeb/ReportsWeb/ReportPage.aspx?Back=/ACTAWeb/ACTAWebUI/TLDetailedDataPage.aspx&Report=" + reportURL.Trim(), false);
                    }
                    else
                    {
                        lblError.Text = rm.GetString("noReportData", culture);
                    }
                }
                else
                {
                    lblError.Text = rm.GetString("noReportData", culture);
                }

                //save data for returning on same page
                Session[Constants.sessionSamePage] = true;

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLDetailedDataPage.btnReport_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLDetailedDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnReport_Click(Object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLDetailedDataPage).Assembly);

                SaveState();

                lblError.Text = "";
                int rowCount = 0;
                //DataTable ioPairs = null;
                if (cbEmplTotals.Checked)
                    Session["TLDetailedDataPage.checked"] = 2;
                else
                {
                    if (cbMontlyTotals.Checked)
                        Session["TLDetailedDataPage.checked"] = 1;
                    else
                        Session["TLDetailedDataPage.checked"] = 0;
                }

                if (Session[Constants.sessionDataTableList] != null && Session[Constants.sessionDataTableList] is List<List<object>>
                    && Session[Constants.sessionDataTableColumns] != null && Session[Constants.sessionDataTableColumns] is List<DataColumn>)
                {
                    List<List<object>> resultTable = (List<List<object>>)Session[Constants.sessionDataTableList];
                    List<DataColumn> resultColumns = (List<DataColumn>)Session[Constants.sessionDataTableColumns];

                    rowCount = resultTable.Count;
                    if (rowCount > 30000)
                    {
                        btnReport.Enabled = false;
                        lblError.Text = rm.GetString("largeReportDisabled", culture);
                        return;
                    }
                    else if (rowCount > 10000 && (Session["SessionReportFirstClick"] == null))
                    {
                        lblError.Text = rm.GetString("largeReport", culture);
                        Session["SessionReportFirstClick"] = "2";
                        return;
                    }

                    if (rowCount > 0)
                    {
                        // get all passes for search criteria for report
                        //ioPairs = new Result(Session[Constants.sessionConnection]).SearchResultTable(Session[Constants.sessionFields].ToString().Trim(), Session[Constants.sessionTables].ToString().Trim(), Session[Constants.sessionFilter].ToString().Trim(),
                        //Session[Constants.sessionSortCol].ToString().Trim(), Session[Constants.sessionSortDir].ToString().Trim(), 1, rowCount);

                        // Table Definition for  Reports
                        DataSet dataSetCR = new DataSet();
                        DataTable tableCR = new DataTable("io_pairs");
                        DataTable tableI = new DataTable("images");

                        tableCR.Columns.Add("first_name", typeof(System.String));
                        tableCR.Columns.Add("last_name", typeof(System.String));
                        tableCR.Columns.Add("date_week", typeof(System.String));
                        tableCR.Columns.Add("date_year", typeof(System.String));
                        tableCR.Columns.Add("date_month", typeof(System.String));
                        tableCR.Columns.Add("date_day", typeof(System.String));
                        tableCR.Columns.Add("branch", typeof(System.String));
                        tableCR.Columns.Add("cost_centre_code", typeof(System.String));
                        tableCR.Columns.Add("cost_centre", typeof(System.String));
                        tableCR.Columns.Add("workgroup", typeof(System.String));
                        tableCR.Columns.Add("ute", typeof(System.String));
                        tableCR.Columns.Add("empl_type", typeof(System.String));
                        tableCR.Columns.Add("start_time", typeof(System.TimeSpan));
                        tableCR.Columns.Add("end_time", typeof(System.TimeSpan));
                        tableCR.Columns.Add("total", typeof(System.Double));
                        tableCR.Columns.Add("pass_type", typeof(System.String));
                        tableCR.Columns.Add("description", typeof(System.String));
                        tableCR.Columns.Add("imageID", typeof(byte));
                        tableCR.Columns.Add("month", typeof(System.DateTime));
                        tableCR.Columns.Add("changedMonth", typeof(System.String));
                        tableCR.Columns.Add("empl_id", typeof(System.String));
                        tableCR.Columns.Add("shift", typeof(System.String));
                        tableCR.Columns.Add("cycle_day", typeof(System.String));
                        tableCR.Columns.Add("intervals", typeof(System.String));
                        tableCR.Columns.Add("pair_date", typeof(System.String));
                        tableI.Columns.Add("imageID", typeof(byte));
                        tableI.Columns.Add("image", typeof(System.Byte[]));

                        //add logo image just once
                        DataRow rowI = tableI.NewRow();
                        rowI["image"] = Constants.LogoForReport;
                        rowI["imageID"] = 1;
                        tableI.Rows.Add(rowI);
                        tableI.AcceptChanges();

                        dataSetCR.Tables.Add(tableCR);
                        dataSetCR.Tables.Add(tableI);

                        int monthIndex = 1;
                        List<string> listMonth = new List<string>();
                        string emplIDs = Session["emplIDs"].ToString();
                        
                        DateTime fromDate = (DateTime)Session[Constants.sessionFromDate];
                        DateTime toDate = (DateTime)Session[Constants.sessionToDate];
                        Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule(Session[Constants.sessionConnection]).SearchEmployeesSchedulesExactDate(emplIDs, fromDate, toDate, null);
                        Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4(Session[Constants.sessionConnection]).SearchDictionary(emplIDs);
                        Dictionary<int, WorkTimeSchemaTO> schemas = new TimeSchema(Session[Constants.sessionConnection]).getDictionary();
                        
                        // populate dataset
                        int index = 0;
                        foreach (List<object> currRow in resultTable)
                        {
                            if (currRow.Count < 18)
                                continue;

                            int empl = -1;
                            int day = -1;
                            int month = -1;
                            int year = -1;
                            DataRow row = tableCR.NewRow();
                            if (currRow[0] is int)
                                empl = (int)currRow[0];
                            if (currRow[11] is int)
                                day = (int)currRow[11];
                            if (currRow[10] is int)
                                month = (int)currRow[10];
                            if (currRow[9] is int)
                                year = (int)currRow[9];

                            row["empl_id"] = currRow[0].ToString().Trim();
                            row["first_name"] = currRow[1].ToString().Trim();
                            row["cost_centre_code"] = currRow[2].ToString().Trim();
                            row["cost_centre"] = currRow[3].ToString().Trim();
                            row["workgroup"] = currRow[4].ToString().Trim();
                            row["ute"] = currRow[5].ToString().Trim();
                            row["branch"] = currRow[6].ToString().Trim();
                            row["empl_type"] = currRow[7].ToString().Trim();
                            row["date_week"] = currRow[8].ToString().Trim();
                            row["date_year"] = currRow[9].ToString().Trim();
                            row["date_month"] = currRow[10].ToString().Trim();
                            row["date_day"] = currRow[11].ToString().Trim();
                            TimeSpan startTime = new TimeSpan();
                            TimeSpan endTime = new TimeSpan();
                            if (currRow[12] is TimeSpan)
                                startTime = (TimeSpan)currRow[12];
                            if (currRow[13] is TimeSpan)
                                endTime = (TimeSpan)currRow[13];
                            row["start_time"] = startTime;
                            row["end_time"] = endTime;
                            row["pass_type"] = currRow[14].ToString().Trim();                            
                            double num = 0;
                            if (currRow[15] is TimeSpan)                            
                                num = Math.Round(((TimeSpan)currRow[15]).TotalHours, 2);
                            
                            row["total"] = num;
                            row["description"] = currRow[16].ToString().Trim();
                            DateTime pairDate = new DateTime();
                            if (currRow[17] is DateTime)
                                pairDate = (DateTime)currRow[17];
                            row["pair_date"] = pairDate.ToString(Constants.dateFormat);
                            
                            List<EmployeeTimeScheduleTO> schedules = new List<EmployeeTimeScheduleTO>();
                            DateTime ioPairDate = new DateTime();

                            if (year != -1 && month != -1 && day != -1) { ioPairDate = new DateTime(year, month, day); }
                            if (empl != -1)
                            {
                                if (emplSchedules.ContainsKey(empl))
                                    schedules = emplSchedules[empl];

                                WorkTimeSchemaTO sch = Common.Misc.getTimeSchema(ioPairDate, schedules, schemas);

                                if (sch.TimeSchemaID != -1)
                                    row["shift"] = sch.Description.Trim();
                                else
                                    row["shift"] = "N/A";

                                List<WorkTimeIntervalTO> intervals = Common.Misc.getTimeSchemaInterval(ioPairDate, schedules, schemas);

                                int cycleDay = 0;
                                string intervalsString = "";
                                foreach (WorkTimeIntervalTO interval in intervals)
                                {
                                    cycleDay = interval.DayNum + 1;

                                    intervalsString += interval.StartTime.ToString(Constants.timeFormat) + "-" + interval.EndTime.ToString(Constants.timeFormat);

                                    if (!interval.Description.Trim().Equals(""))
                                        intervalsString += "(" + interval.Description.Trim() + ")";

                                    intervalsString += "; ";
                                }

                                row["cycle_day"] = cycleDay.ToString().Trim();
                                row["intervals"] = intervalsString.Trim();
                            }
                            
                            //int yearDate = int.Parse(row["date_year"].ToString().Remove(row["date_year"].ToString().LastIndexOf('.')));
                            //int monthDate = int.Parse(row["date_month"].ToString().Remove(row["date_month"].ToString().LastIndexOf('.')));

                            DateTime dtime = new DateTime(year, month, 1, 0, 0, 0);
                            row["month"] = new DateTime(dtime.Year, dtime.Month, 1);

                            if (!listMonth.Contains(dtime.Month + "." + dtime.Year))
                                listMonth.Add(dtime.Month + "." + dtime.Year);

                            if (resultTable.Count > monthIndex && resultTable[monthIndex].Count > 11 && resultTable[monthIndex][9] is int
                                && resultTable[monthIndex][10] is int && resultTable[monthIndex][11] is int)
                            {
                                int year1 = (int)resultTable[monthIndex][9];
                                int month1 = (int)resultTable[monthIndex][10];
                                int day1 = (int)resultTable[monthIndex][11];

                                DateTime date = new DateTime(year1, month1, day1, 0, 0, 0);

                                if (dtime.Month != date.Month)
                                {
                                    row["changedMonth"] = "1";
                                }
                                else
                                {
                                    row["changedMonth"] = "0";
                                }
                                if (index == 0)
                                {
                                    //    previous = dtime.Month;
                                    if (resultTable.Count == 1)
                                        row["changedMonth"] = "1";
                                    else
                                        row["changedMonth"] = "0";
                                }

                                monthIndex++;
                            }
                            else if (resultTable.Count == monthIndex)
                            {
                                row["changedMonth"] = "1";
                            }

                            row["imageID"] = 1;

                            if (index == resultTable.Count - 1)
                            {
                                DateTime from = CommonWeb.Misc.createDate(tbFromDate.Text);
                                DateTime to = CommonWeb.Misc.createDate(tbToDate.Text);
                                if (from.Month == to.Month)
                                    row["changedMonth"] = "1";
                            }
                            tableCR.Rows.Add(row);
                            tableCR.AcceptChanges();

                            index++;
                        }

                        if (tableCR.Rows.Count == 0)
                        {
                            lblError.Text = rm.GetString("noReportData", culture);
                            writeLog(DateTime.Now, false);
                            return;
                        }

                        string pass_type = "*";
                        string start_time = "";
                        string end_time = "";
                        string empolyee = "*";

                        //gate parameter for report
                        if (lbPassTypes.SelectedIndex > 0)
                        {
                            for (int intGates = 0; intGates < lbPassTypes.Items.Count; intGates++)
                            {
                                if (lbPassTypes.Items[intGates].Selected)
                                {
                                    pass_type = pass_type + ", " + lbPassTypes.Items[intGates].ToString();
                                }
                            }
                            pass_type = pass_type.Substring(pass_type.IndexOf(',') + 1);
                        }
                        if (pass_type == "")
                        {
                            for (int intGates = 0; intGates < lbPassTypes.Items.Count; intGates++)
                            {
                                pass_type = pass_type + ", " + lbPassTypes.Items[intGates].ToString();
                            }
                            pass_type = pass_type.Substring(pass_type.IndexOf(',') + 1);
                        }

                        //employee parameter for report
                        if (lboxEmployees.SelectedIndex > 0)
                        {
                            empolyee = "";
                            for (int intEmpolyees = 0; intEmpolyees < lboxEmployees.Items.Count; intEmpolyees++)
                            {
                                if (lboxEmployees.Items[intEmpolyees].Selected)
                                {
                                    empolyee = empolyee + ", " + lboxEmployees.Items[intEmpolyees].ToString();
                                }
                            }

                            empolyee = empolyee.Substring(empolyee.IndexOf(',') + 1);
                        }

                        if (empolyee == "")
                        {
                            for (int intEmpolyees = 0; intEmpolyees < lboxEmployees.Items.Count; intEmpolyees++)
                            {
                                empolyee = empolyee + ", " + lboxEmployees.Items[intEmpolyees].ToString();
                            }
                            empolyee = empolyee.Substring(empolyee.IndexOf(',') + 1);
                        }

                        //fromDate parameter for report
                        if (tbFromDate.Text != "")
                        {
                            start_time = tbFromDate.Text;
                        }
                        //toDate parameter for report
                        if (tbToDate.Text != "")
                        {
                            end_time = tbToDate.Text;
                        }

                        DataTable dataTableMonthly = new DataTable("io_pairs_monthly_pt");
                        dataTableMonthly.Columns.Add("total", typeof(System.Double));
                        dataTableMonthly.Columns.Add("month", typeof(System.String));
                        dataTableMonthly.Columns.Add("pass_type", typeof(System.String));
                        dataTableMonthly.Columns.Add("employee", typeof(System.String));
                        dataSetCR.Tables.Add(dataTableMonthly);

                        if (cbEmplTotals.Checked)
                            CalcEmplMonthlyTotals(resultTable, dataTableMonthly);
                        else if (cbMontlyTotals.Checked)
                        {
                            foreach (string monthInt in listMonth)
                            {
                                CalcMonthlyTotals(resultTable, monthInt, dataTableMonthly);
                            }
                        }

                        //send to session parameters for report
                        if (rbPassType.Checked)
                            Session["TLDetailedDataPage.pt_loc"] = rm.GetString("hdrPassType", culture);
                        else
                            Session["TLDetailedDataPage.pt_loc"] = rm.GetString("hdrLocation", culture);
                        Session["TLDetailedDataPage.pass_type"] = pass_type;
                        Session["TLDetailedDataPage.employee"] = empolyee;
                        Session["TLDetailedDataPage.start_time"] = start_time;
                        Session["TLDetailedDataPage.end_time"] = end_time;
                        Session["TLDetailedDataPage.io_pairs"] = dataSetCR;
                        Session[Constants.sessionReportName] = rm.GetString("lblPassesReport", culture);
                        Session["DetailedDataReportFilePath"] = null;
                        //check language and redirect to apropriate report
                        string reportURL = "";
                        if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                            reportURL = "/ACTAWeb/ReportsWeb/sr/TLDetailedDataReport_sr.aspx";
                        else
                            reportURL = "/ACTAWeb/ReportsWeb/en/TLDetailedDataReport_en.aspx";
                        Response.Redirect("/ACTAWeb/ReportsWeb/ReportPage.aspx?Back=/ACTAWeb/ACTAWebUI/TLDetailedDataPage.aspx&Report=" + reportURL.Trim(), false);
                    }
                    else
                    {
                        lblError.Text = rm.GetString("noReportData", culture);
                    }
                }
                else
                {
                    lblError.Text = rm.GetString("noReportData", culture);
                }

                //save data for returning on same page
                Session[Constants.sessionSamePage] = true;

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLDetailedDataPage.btnReport_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLDetailedDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void ShowPopUpMsg(string msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("alert('");
            sb.Append(msg.Replace("\n", "\\n").Replace("\r", "").Replace("'", "\\'"));
            sb.Append("');");
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", sb.ToString(), true);
        }

        protected void btnExport_Click(Object sender, EventArgs e)
        {
            try
            {
                //method();

                //string filePath = t1.Text;
                //if (!filePath.Trim().Equals(""))
                //{
                //    string name = Path.GetFileNameWithoutExtension(filePath);
                //    WebClient client = new WebClient();
                //    Byte[] buffer = client.DownloadData(filePath);

                //    if (buffer != null)
                //    {
                //        Response.Clear();
                //        Response.ContentType = "application/xls";
                //        Response.AppendHeader("Content-Disposition", "attachment;Filename=" + name + ".xls");
                //        Response.TransmitFile(filePath);
                //        //Response.End();
                //        Response.Flush();


                //        ShowPopUpMsg("export");

                //    }
                //    Session["DetailedDataReportFilePath"] = null;
                //    t1.Text = "";
                //}
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLDetailedDataPage.btnReport_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLDetailedDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void CalcMonthlyTotalsOld(DataTable ioPairs, string month, DataTable tableMonthly)
        {
            try
            {
                Dictionary<string, TimeSpan> typesCounter = new Dictionary<string, TimeSpan>();
                Dictionary<int, PassTypeTO> passTypes = getTypes();
                Dictionary<int, LocationTO> locDict = new Location(Session[Constants.sessionConnection]).SearchDict();
                foreach (DataRow ioPair in ioPairs.Rows)
                {
                    string date_year = ioPair["date_year"].ToString();
                    int year1 = int.Parse(date_year);
                    int month1 = int.Parse(ioPair["date_month"].ToString());
                    int day1 = int.Parse(ioPair["date_day"].ToString());

                    DateTime dtime = new DateTime(year1, month1, day1, 0, 0, 0);

                    //DateTime dtime = CommonWeb.Misc.createDate(ioPair["date"].ToString() + ".");
                    //row["month"] = new DateTime(dtime.Year, dtime.Month, 1);
                    if (dtime.Month + "." + dtime.Year == month)
                    {
                        DateTime start = DateTime.Parse(ioPair["start_time"].ToString());
                        DateTime end = DateTime.Parse(ioPair["end_time"].ToString());
                        string desc = "";
                        if (rbPassType.Checked)
                            desc = ioPair["pass_type"].ToString();
                        else
                            desc = ioPair["location"].ToString();
                        if (!start.Equals(new DateTime()) && !end.Equals(new DateTime()))
                        {
                            if (rbPassType.Checked)
                            {
                                foreach (KeyValuePair<int, PassTypeTO> pair in passTypes)
                                {
                                    if (desc == pair.Value.DescriptionAndID || desc == pair.Value.DescriptionAltAndID)
                                    {
                                        TimeSpan duration = end.TimeOfDay.Subtract(start.TimeOfDay);
                                        if (end.TimeOfDay.Hours == 23 && end.TimeOfDay.Minutes == 59)
                                            duration = duration.Add(new TimeSpan(0, 1, 0)); // add one minute if it is last interval from third shift

                                        if (typesCounter.ContainsKey(desc))
                                        {
                                            typesCounter[desc] = typesCounter[desc].Add(duration);
                                            break;
                                        }
                                        else
                                        {
                                            typesCounter.Add(desc, duration);
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (KeyValuePair<int, LocationTO> loc in locDict)
                                {
                                    if (desc == loc.Value.Name)
                                    {
                                        TimeSpan duration = end.TimeOfDay.Subtract(start.TimeOfDay);
                                        if (end.TimeOfDay.Hours == 23 && end.TimeOfDay.Minutes == 59)
                                            duration = duration.Add(new TimeSpan(0, 1, 0)); // add one minute if it is last interval from third shift

                                        if (typesCounter.ContainsKey(desc))
                                        {
                                            typesCounter[desc] = typesCounter[desc].Add(duration);
                                            break;
                                        }
                                        else
                                        {
                                            typesCounter.Add(desc, duration);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (string desc in typesCounter.Keys)
                {
                    DataRow row = tableMonthly.NewRow();
                    row["month"] = month;
                    row["pass_type"] = desc;
                    double hours = typesCounter[desc].TotalHours;
                    double num = Math.Round(hours, 2);
                    row["total"] = num;
                    row["employee"] = "";
                    tableMonthly.Rows.Add(row);
                    tableMonthly.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CalcMonthlyTotals(List<List<object>> resultTable, string month, DataTable tableMonthly)
        {
            try
            {
                Dictionary<string, TimeSpan> typesCounter = new Dictionary<string, TimeSpan>();
                Dictionary<int, PassTypeTO> passTypes = getTypes();
                Dictionary<int, LocationTO> locDict = new Location(Session[Constants.sessionConnection]).SearchDict();
                foreach (List<object> currRow in resultTable)
                {
                    if (currRow.Count < 15)
                        continue;

                    // get pair date                    
                    int year1 = -1;
                    if (currRow[9] != null && currRow[9] is int)
                        year1 = (int)currRow[9];

                    int month1 = -1;
                    if (currRow[10] != null && currRow[10] is int)
                        month1 = (int)currRow[10];

                    int day1 = -1;
                    if (currRow[11] != null && currRow[11] is int)
                        day1 = (int)currRow[11];
                    
                    DateTime dtime = new DateTime(year1, month1, day1, 0, 0, 0);

                    //DateTime dtime = CommonWeb.Misc.createDate(ioPair["date"].ToString() + ".");
                    //row["month"] = new DateTime(dtime.Year, dtime.Month, 1);
                    if (dtime.Month + "." + dtime.Year == month)
                    {
                        // get pair start time
                        TimeSpan start = new TimeSpan();
                        if (currRow[12] != null && currRow[12] is TimeSpan)
                            start = (TimeSpan)currRow[12];

                        // get pair end time
                        TimeSpan end = new TimeSpan();
                        if (currRow[13] != null && currRow[13] is TimeSpan)
                            end = (TimeSpan)currRow[13];

                        string desc = currRow[14].ToString().Trim();
                        
                        //if (!start.Equals(new DateTime()) && !end.Equals(new DateTime()))
                        //{
                            if (rbPassType.Checked)
                            {
                                foreach (KeyValuePair<int, PassTypeTO> pair in passTypes)
                                {
                                    if (desc == pair.Value.DescriptionAndID || desc == pair.Value.DescriptionAltAndID)
                                    {
                                        TimeSpan duration = end.Subtract(start);
                                        if (end.Hours == 23 && end.Minutes == 59)
                                            duration = duration.Add(new TimeSpan(0, 1, 0)); // add one minute if it is last interval from third shift

                                        if (typesCounter.ContainsKey(desc))
                                        {
                                            typesCounter[desc] = typesCounter[desc].Add(duration);
                                            break;
                                        }
                                        else
                                        {
                                            typesCounter.Add(desc, duration);
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (KeyValuePair<int, LocationTO> loc in locDict)
                                {
                                    if (desc == loc.Value.Name)
                                    {
                                        TimeSpan duration = end.Subtract(start);
                                        if (end.Hours == 23 && end.Minutes == 59)
                                            duration = duration.Add(new TimeSpan(0, 1, 0)); // add one minute if it is last interval from third shift

                                        if (typesCounter.ContainsKey(desc))
                                        {
                                            typesCounter[desc] = typesCounter[desc].Add(duration);
                                            break;
                                        }
                                        else
                                        {
                                            typesCounter.Add(desc, duration);
                                            break;
                                        }
                                    }
                                }
                            }
                        //}
                    }
                }

                foreach (string desc in typesCounter.Keys)
                {
                    DataRow row = tableMonthly.NewRow();
                    row["month"] = month;
                    row["pass_type"] = desc;
                    double hours = typesCounter[desc].TotalHours;
                    double num = Math.Round(hours, 2);
                    row["total"] = num;
                    row["employee"] = "";
                    tableMonthly.Rows.Add(row);
                    tableMonthly.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CalcEmplMonthlyTotalsOld(DataTable ioPairs, DataTable tableMonthly)
        {
            try
            {
                Dictionary<int, Dictionary<DateTime, Dictionary<string, TimeSpan>>> emplTypesCounter = new Dictionary<int, Dictionary<DateTime, Dictionary<string, TimeSpan>>>();
                Dictionary<int, string> emplNames = new Dictionary<int, string>();
                foreach (DataRow ioPair in ioPairs.Rows)
                {
                    // get pair employee
                    int emplID = -1;
                    if (ioPair["empl_id"] != DBNull.Value && !int.TryParse(ioPair["empl_id"].ToString(), out emplID))
                        emplID = -1;

                    if (emplID == -1)
                        continue;

                    if (!emplNames.ContainsKey(emplID))
                        emplNames.Add(emplID, "");

                    if (emplNames[emplID] == "" && ioPair["first_name"] != DBNull.Value)
                        emplNames[emplID] = ioPair["first_name"].ToString().Trim();

                    // get pair date                    
                    int year1 = -1;
                    if (ioPair["date_year"] != DBNull.Value && !int.TryParse(ioPair["date_year"].ToString(), out year1))
                        year1 = -1;

                    int month1 = -1;
                    if (ioPair["date_month"] != DBNull.Value && !int.TryParse(ioPair["date_month"].ToString(), out month1))
                        month1 = -1;

                    int day1 = -1;
                    if (ioPair["date_day"] != DBNull.Value && !int.TryParse(ioPair["date_day"].ToString(), out day1))
                        day1 = -1;

                    DateTime dtime = new DateTime();

                    if (year1 != -1 && month1 != -1 && day1 != -1)
                        dtime = new DateTime(year1, month1, day1, 0, 0, 0);

                    if (dtime == new DateTime())
                        continue;

                    // get pair start time
                    DateTime start = new DateTime();
                    if (ioPair["start_time"] != DBNull.Value && !DateTime.TryParse(ioPair["start_time"].ToString(), out start))
                        start = new DateTime();

                    // get pair end time
                    DateTime end = new DateTime();
                    if (ioPair["end_time"] != DBNull.Value && !DateTime.TryParse(ioPair["end_time"].ToString(), out end))
                        end = new DateTime();

                    if (start == new DateTime() || end == new DateTime())
                        continue;

                    TimeSpan duration = end.TimeOfDay.Subtract(start.TimeOfDay);
                    if (end.TimeOfDay.Hours == 23 && end.TimeOfDay.Minutes == 59)
                        duration = duration.Add(new TimeSpan(0, 1, 0)); // add one minute if it is last interval from third shift

                    // get pass type/location
                    string desc = "";

                    if (rbPassType.Checked)
                    {
                        if (ioPair["pass_type"] != DBNull.Value)
                            desc = ioPair["pass_type"].ToString().Trim();
                    }
                    else
                    {
                        if (ioPair["location"] != DBNull.Value)
                            desc = ioPair["location"].ToString().Trim();
                    }

                    if (desc == "")
                        continue;

                    if (!emplTypesCounter.ContainsKey(emplID))
                        emplTypesCounter.Add(emplID, new Dictionary<DateTime, Dictionary<string, TimeSpan>>());

                    DateTime pairSumDate = new DateTime(dtime.Year, dtime.Month, 1);

                    if (!cbMontlyTotals.Checked)
                        pairSumDate = Constants.dateTimeNullValue();

                    if (!emplTypesCounter[emplID].ContainsKey(pairSumDate.Date))
                        emplTypesCounter[emplID].Add(pairSumDate.Date, new Dictionary<string, TimeSpan>());

                    if (!emplTypesCounter[emplID][pairSumDate.Date].ContainsKey(desc))
                        emplTypesCounter[emplID][pairSumDate.Date].Add(desc, new TimeSpan());

                    emplTypesCounter[emplID][pairSumDate.Date][desc] = emplTypesCounter[emplID][pairSumDate.Date][desc].Add(duration);
                }

                foreach (int emplID in emplTypesCounter.Keys)
                {
                    foreach (DateTime month in emplTypesCounter[emplID].Keys)
                    {
                        foreach (string desc in emplTypesCounter[emplID][month].Keys)
                        {
                            DataRow row = tableMonthly.NewRow();
                            if (month == Constants.dateTimeNullValue())
                                row["month"] = "";
                            else
                                row["month"] = month.Month.ToString() + "." + month.Year.ToString();
                            row["pass_type"] = desc;
                            double hours = emplTypesCounter[emplID][month][desc].TotalHours;
                            double num = Math.Round(hours, 2);
                            row["total"] = num;
                            if (emplNames.ContainsKey(emplID))
                                row["employee"] = emplID.ToString() + " " + emplNames[emplID].Trim();
                            else
                                row["employee"] = "";
                            tableMonthly.Rows.Add(row);
                            tableMonthly.AcceptChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CalcEmplMonthlyTotals(List<List<object>> resultTable, DataTable tableMonthly)
        {
            try
            {
                Dictionary<int, Dictionary<DateTime, Dictionary<string, TimeSpan>>> emplTypesCounter = new Dictionary<int, Dictionary<DateTime, Dictionary<string, TimeSpan>>>();
                Dictionary<int, string> emplNames = new Dictionary<int, string>();
                foreach (List<object> currRow in resultTable)
                {
                    if (currRow.Count < 15)
                        continue;

                    // get pair employee
                    int emplID = -1;
                    if (currRow[0] != null && currRow[0] is int)
                        emplID = (int)currRow[0];

                    if (emplID == -1)                    
                        continue;

                    if (!emplNames.ContainsKey(emplID))
                        emplNames.Add(emplID, "");

                    if (emplNames[emplID] == "" && currRow[1] != null)
                        emplNames[emplID] = currRow[1].ToString().Trim();

                    // get pair date                    
                    int year1 = -1;
                    if (currRow[9] != null && currRow[9] is int)
                        year1 = (int)currRow[9];

                    int month1 = -1;
                    if (currRow[10] != null && currRow[10] is int)
                        month1 = (int)currRow[10];

                    int day1 = -1;
                    if (currRow[11] != null && currRow[11] is int)
                        day1 = (int)currRow[11];

                    DateTime dtime = new DateTime();

                    if (year1 != -1 && month1 != -1 && day1 != -1)
                        dtime = new DateTime(year1, month1, day1, 0, 0, 0);
                    
                    if (dtime == new DateTime())
                        continue;

                    // get pair start time
                    TimeSpan start = new TimeSpan();
                    if (currRow[12] != null && currRow[12] is TimeSpan)
                        start = (TimeSpan)currRow[12];

                    // get pair end time
                    TimeSpan end = new TimeSpan();
                    if (currRow[13] != null && currRow[13] is TimeSpan)
                        end = (TimeSpan)currRow[13];

                    //if (start == new TimeSpan() || end == new TimeSpan())
                    //    continue;
                    
                    TimeSpan duration = end.Subtract(start);
                    if (end.Hours == 23 && end.Minutes == 59)
                        duration = duration.Add(new TimeSpan(0, 1, 0)); // add one minute if it is last interval from third shift

                    // get pass type/location
                    string desc = currRow[14].ToString().Trim();
                    
                    if (desc == "")
                        continue;

                    if (!emplTypesCounter.ContainsKey(emplID))
                        emplTypesCounter.Add(emplID, new Dictionary<DateTime, Dictionary<string, TimeSpan>>());

                    DateTime pairSumDate = new DateTime(dtime.Year, dtime.Month, 1);

                    if (!cbMontlyTotals.Checked)
                        pairSumDate = Constants.dateTimeNullValue();

                    if (!emplTypesCounter[emplID].ContainsKey(pairSumDate.Date))
                        emplTypesCounter[emplID].Add(pairSumDate.Date, new Dictionary<string, TimeSpan>());

                    if (!emplTypesCounter[emplID][pairSumDate.Date].ContainsKey(desc))
                        emplTypesCounter[emplID][pairSumDate.Date].Add(desc, new TimeSpan());

                    emplTypesCounter[emplID][pairSumDate.Date][desc] = emplTypesCounter[emplID][pairSumDate.Date][desc].Add(duration);
                }

                foreach (int emplID in emplTypesCounter.Keys)
                {
                    foreach (DateTime month in emplTypesCounter[emplID].Keys)
                    {
                        foreach (string desc in emplTypesCounter[emplID][month].Keys)
                        {
                            DataRow row = tableMonthly.NewRow();
                            if (month == Constants.dateTimeNullValue())
                                row["month"] = "";
                            else
                                row["month"] = month.Month.ToString() + "." + month.Year.ToString();
                            row["pass_type"] = desc;
                            double hours = emplTypesCounter[emplID][month][desc].TotalHours;
                            double num = Math.Round(hours, 2);
                            row["total"] = num;
                            if (emplNames.ContainsKey(emplID))
                                row["employee"] = emplID.ToString() + " " + emplNames[emplID].Trim();
                            else
                                row["employee"] = "";
                            tableMonthly.Rows.Add(row);
                            tableMonthly.AcceptChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void generateOld()
        {
            try
            {
                DebugLog log = new DebugLog(Constants.logFilePath + "LogReport.txt");
                log.writeLog("Start " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:sss"));

                int rowCount = 0;
                DataTable ioPairs = null;
                log.writeLog(rowCount.ToString());

                if (Session[Constants.sessionFields] != null && !Session[Constants.sessionFields].ToString().Trim().Equals("")
                    && Session[Constants.sessionTables] != null && !Session[Constants.sessionTables].ToString().Trim().Equals("")
                    && Session[Constants.sessionFilter] != null && !Session[Constants.sessionFilter].ToString().Trim().Equals("")
                    && Session[Constants.sessionSortCol] != null && !Session[Constants.sessionSortCol].ToString().Trim().Equals("")
                    && Session[Constants.sessionSortDir] != null && !Session[Constants.sessionSortDir].ToString().Trim().Equals(""))
                {
                    Result result = new Result(Session[Constants.sessionConnection]);
                    rowCount = result.SearchResultCount(Session[Constants.sessionTables].ToString().Trim(), Session[Constants.sessionFilter].ToString().Trim());
                    log.writeLog(rowCount.ToString());
                    if (rowCount > 0)
                    {
                        log.writeLog(rowCount.ToString() + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:sss"));
                        // get all passes for search criteria for report

                        ioPairs = new Result(Session[Constants.sessionConnection]).SearchResultTable(Session[Constants.sessionFields].ToString().Trim(), Session[Constants.sessionTables].ToString().Trim(), Session[Constants.sessionFilter].ToString().Trim(),
                        Session[Constants.sessionSortCol].ToString().Trim(), Session[Constants.sessionSortDir].ToString().Trim(), 1, rowCount);

                        List<string> listMonth = new List<string>();
                        string emplIDs = Session["emplIDs"].ToString();


                        DateTime fromDate = (DateTime)Session[Constants.sessionFromDate];
                        DateTime toDate1 = (DateTime)Session[Constants.sessionToDate];
                        Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule(Session[Constants.sessionConnection]).SearchEmployeesSchedulesExactDate(emplIDs, fromDate, toDate1, null);

                        Dictionary<int, WorkTimeSchemaTO> schemas = new TimeSchema(Session[Constants.sessionConnection]).getDictionary();

                        ApplUserTO user = (ApplUserTO)Session[Constants.sessionLogInUser];
                        string filePath = Constants.logFilePath + "Temp\\" + user.UserID + "_DetailedDataReport" + DateTime.Now.ToString("dd-MM-yyyy HHmmss") + "\\";
                        filePath += "DetailedDataReport.xls";
                        log.writeLog(filePath);

                        string Pathh = Directory.GetParent(filePath).FullName;
                        if (!Directory.Exists(Pathh))
                        {
                            Directory.CreateDirectory(Pathh);
                        }
                        if (File.Exists(filePath))
                            File.Delete(filePath);

                        //DELETE OLD FOLDERS AND FILES FROM TEMP
                        string tempFolder = Constants.logFilePath + "Temp";
                        string[] dirs = Directory.GetDirectories(tempFolder);
                        foreach (string direct in dirs)
                        {
                            DateTime creation = Directory.GetCreationTime(direct);
                            TimeSpan create = creation.AddHours(2).TimeOfDay;

                            if (creation.Date < DateTime.Now.Date || create < DateTime.Now.TimeOfDay)
                                Directory.Delete(direct, true);
                        }

                        CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                        ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TestReport).Assembly);

                        StreamWriter writer = new StreamWriter(filePath, true, System.Text.Encoding.Unicode);

                        string[] columns = new string[20];

                        columns[0] = rm.GetString("hdrEmployeeID", culture);
                        columns[1] = rm.GetString("hdrEmployee", culture);
                        columns[2] = rm.GetString("hdrShift", culture);
                        columns[3] = rm.GetString("hdrCycleDay", culture);
                        columns[4] = rm.GetString("hdrInterval", culture);
                        columns[5] = rm.GetString("hdrWeek", culture);
                        columns[6] = rm.GetString("hdrYear", culture);
                        columns[7] = rm.GetString("hdrMonth", culture);
                        columns[8] = rm.GetString("hdrDay", culture);
                        columns[9] = rm.GetString("hdrCostCenterCode", culture);
                        columns[10] = rm.GetString("hdrCostCenterName", culture);
                        columns[11] = rm.GetString("hdrWorkgroup", culture);
                        columns[12] = rm.GetString("hdrUte", culture);
                        columns[13] = rm.GetString("lblBranch", culture).Remove(rm.GetString("lblBranch", culture).IndexOf(':'));
                        columns[14] = rm.GetString("hdrEmplType", culture);
                        columns[15] = rm.GetString("hdrStartTime", culture);
                        columns[16] = rm.GetString("hdrEndTime", culture);
                        if (rbPassType.Checked)
                            columns[17] = rm.GetString("hdrPassType", culture);
                        else
                            columns[17] = rm.GetString("hdrLocation", culture);
                        columns[18] = rm.GetString("hdrTotal", culture);
                        columns[19] = rm.GetString("hdrDescription", culture);

                        string columnsString = "";
                        foreach (string c in columns)
                        {
                            columnsString += c + "\t";
                        }
                        writer.WriteLine(columnsString);
                        int numr = 0;
                        foreach (DataRow ioPair in ioPairs.Rows)
                        {
                            numr++;
                            string resultStr = "";
                            int empl = -1;
                            int day = -1;
                            int month = -1;

                            int year = -1;

                            if (ioPair["empl_id"] != DBNull.Value)
                            {
                                resultStr += ioPair["empl_id"].ToString().Trim() + "\t";
                                empl = int.Parse(ioPair["empl_id"].ToString());
                            }
                            else
                                resultStr += "\t";
                            if (ioPair["first_name"] != DBNull.Value)
                                resultStr += ioPair["first_name"].ToString().Trim() + "\t";
                            else
                                resultStr += "\t";

                            List<EmployeeTimeScheduleTO> schedules = new List<EmployeeTimeScheduleTO>();
                            if (ioPair["date_year"] != DBNull.Value)                            
                                year = int.Parse(ioPair["date_year"].ToString().Trim());
                            
                            if (ioPair["date_month"] != DBNull.Value)
                                month = int.Parse(ioPair["date_month"].ToString().Trim());
                            
                            if (ioPair["date_day"] != DBNull.Value)
                                day = int.Parse(ioPair["date_day"].ToString().Trim());
                            
                            DateTime ioPairDate = new DateTime();

                            if (year != -1 && month != -1 && day != -1)
                                ioPairDate = new DateTime(year, month, day);

                            if (empl != -1)
                            {
                                if (emplSchedules.ContainsKey(empl))
                                    schedules = emplSchedules[empl];

                                WorkTimeSchemaTO sch = Common.Misc.getTimeSchema(ioPairDate, schedules, schemas);

                                if (sch.TimeSchemaID != -1)
                                    resultStr += sch.Description.Trim() + "\t";
                                else
                                    resultStr += "N/A" + "\t";

                                List<WorkTimeIntervalTO> intervals = Common.Misc.getTimeSchemaInterval(ioPairDate, schedules, schemas);

                                int cycleDay = 0;
                                string intervalsString = "";
                                foreach (WorkTimeIntervalTO interval in intervals)
                                {
                                    cycleDay = interval.DayNum + 1;

                                    intervalsString += interval.StartTime.ToString(Constants.timeFormat) + "-" + interval.EndTime.ToString(Constants.timeFormat);

                                    if (!interval.Description.Trim().Equals(""))
                                        intervalsString += "(" + interval.Description.Trim() + ")";

                                    intervalsString += "; ";
                                }
                                resultStr += cycleDay.ToString().Trim() + "\t";
                                resultStr += intervalsString.Trim() + "\t";

                            }
                            if (ioPair["date_week"] != DBNull.Value)
                                resultStr += ioPair["date_week"].ToString().Trim() + "\t";
                            else
                                resultStr += "\t";

                            if (ioPair["date_year"] != DBNull.Value)
                                resultStr += ioPair["date_year"].ToString().Trim() + "\t";
                            else
                                resultStr += "\t";

                            if (ioPair["date_month"] != DBNull.Value)
                                resultStr += ioPair["date_month"].ToString().Trim() + "\t";
                            else
                                resultStr += "\t";
                            
                            if (ioPair["date_day"] != DBNull.Value)                            
                                resultStr += ioPair["date_day"].ToString().Trim() + "\t";
                            else
                                resultStr += "\t";                           

                            if (ioPair["cost_centre"] != DBNull.Value)
                                resultStr += ioPair["cost_centre"].ToString().Trim() + "\t";
                            else
                                resultStr += "\t";

                            if (ioPair["cost_centre_code"] != DBNull.Value)
                                resultStr += ioPair["cost_centre_code"].ToString().Trim() + "\t";
                            else
                                resultStr += "\t";

                            if (ioPair["workgroup"] != DBNull.Value)
                                resultStr += ioPair["workgroup"].ToString().Trim() + "\t";
                            else
                                resultStr += "\t";

                            if (ioPair["ute"] != DBNull.Value)
                                resultStr += ioPair["ute"].ToString().Trim() + "\t";
                            else
                                resultStr += "\t";

                            if (ioPair["branch"] != DBNull.Value)
                                resultStr += ioPair["branch"].ToString().Trim() + "\t";
                            else
                                resultStr += "\t";

                            if (ioPair["empl_type"] != DBNull.Value)
                                resultStr += ioPair["empl_type"].ToString().Trim() + "\t";
                            else
                                resultStr += "\t";

                            if (ioPair["start_time"] != DBNull.Value)
                                resultStr += ioPair["start_time"].ToString().Trim() + "\t";
                            else
                                resultStr += "\t";

                            if (ioPair["end_time"] != DBNull.Value)
                                resultStr += ioPair["end_time"].ToString().Trim() + "\t";
                            else
                                resultStr += "\t";

                            if (rbPassType.Checked)
                            {
                                if (ioPair["pass_type"] != DBNull.Value)
                                    resultStr += ioPair["pass_type"].ToString().Trim() + "\t";
                                else
                                    resultStr += "\t";
                            }
                            else
                            {
                                if (ioPair["location"] != DBNull.Value)
                                    resultStr += ioPair["location"].ToString().Trim() + "\t";
                                else
                                    resultStr += "\t";
                            }

                            if (ioPair["total"] != DBNull.Value)
                            {
                                string hours = ioPair["total"].ToString().Remove(ioPair["total"].ToString().IndexOf(':'));
                                string minutes = ioPair["total"].ToString().Substring(ioPair["total"].ToString().IndexOf(':') + 1);
                                minutes = minutes.Remove(minutes.IndexOf(':'));

                                decimal minute = (decimal)(int.Parse(minutes)) / (decimal)60;

                                int hour = int.Parse(hours);

                                double num = (double)hour + (double)minute;
                                num = Math.Round(num, 2);
                                resultStr += num + "\t";
                            }
                            else
                                resultStr += "\t";

                            if (ioPair["description"] != DBNull.Value)
                            {
                                string res = ioPair["description"].ToString().Trim();
                                res = res.Replace("\r\n", " ");
                                resultStr += res + "\t";
                            }
                            else
                                resultStr += "\t";

                            writer.WriteLine(resultStr);
                        }

                        log.writeLog(numr + "  " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:sss"));
                        log.writeLog("Close" + "  " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:sss"));

                        writer.Close();
                        Response.Write("finished");

                        Session["DetailedDataReportFilePath"] = filePath;
                    }
                }
            }
            catch
            { }
        }

        private void generate()
        {
            try
            {
                DebugLog log = new DebugLog(Constants.logFilePath + "LogReport.txt");
                log.writeLog("Start " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:sss"));

                int rowCount = 0;
                //DataTable ioPairs = null;
                log.writeLog(rowCount.ToString());

                if (Session[Constants.sessionDataTableList] != null && Session[Constants.sessionDataTableList] is List<List<object>>
                    && Session[Constants.sessionDataTableColumns] != null && Session[Constants.sessionDataTableColumns] is List<DataColumn>)
                {
                    List<List<object>> resultTable = (List<List<object>>)Session[Constants.sessionDataTableList];
                    List<DataColumn> resultColumns = (List<DataColumn>)Session[Constants.sessionDataTableColumns];
                    rowCount = resultTable.Count;
                    log.writeLog(rowCount.ToString());
                    if (rowCount > 0)
                    {
                        log.writeLog(rowCount.ToString() + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:sss"));
                        // get all passes for search criteria for report

                        //ioPairs = new Result(Session[Constants.sessionConnection]).SearchResultTable(Session[Constants.sessionFields].ToString().Trim(), Session[Constants.sessionTables].ToString().Trim(), Session[Constants.sessionFilter].ToString().Trim(),
                        //Session[Constants.sessionSortCol].ToString().Trim(), Session[Constants.sessionSortDir].ToString().Trim(), 1, rowCount);

                        List<string> listMonth = new List<string>();
                        string emplIDs = Session["emplIDs"].ToString();


                        DateTime fromDate = (DateTime)Session[Constants.sessionFromDate];
                        DateTime toDate1 = (DateTime)Session[Constants.sessionToDate];
                        Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule(Session[Constants.sessionConnection]).SearchEmployeesSchedulesExactDate(emplIDs, fromDate, toDate1, null);

                        Dictionary<int, WorkTimeSchemaTO> schemas = new TimeSchema(Session[Constants.sessionConnection]).getDictionary();

                        ApplUserTO user = (ApplUserTO)Session[Constants.sessionLogInUser];
                        string filePath = Constants.logFilePath + "Temp\\" + user.UserID + "_DetailedDataReport" + DateTime.Now.ToString("dd-MM-yyyy HHmmss") + "\\";
                        filePath += "DetailedDataReport.xls";
                        log.writeLog(filePath);

                        string Pathh = Directory.GetParent(filePath).FullName;
                        if (!Directory.Exists(Pathh))
                        {
                            Directory.CreateDirectory(Pathh);
                        }
                        if (File.Exists(filePath))
                            File.Delete(filePath);

                        //DELETE OLD FOLDERS AND FILES FROM TEMP
                        string tempFolder = Constants.logFilePath + "Temp";
                        string[] dirs = Directory.GetDirectories(tempFolder);
                        foreach (string direct in dirs)
                        {
                            DateTime creation = Directory.GetCreationTime(direct);
                            TimeSpan create = creation.AddHours(2).TimeOfDay;

                            if (creation.Date < DateTime.Now.Date || create < DateTime.Now.TimeOfDay)
                                Directory.Delete(direct, true);
                        }

                        CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                        ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TestReport).Assembly);

                        StreamWriter writer = new StreamWriter(filePath, true, System.Text.Encoding.Unicode);

                        string[] columns = new string[21];

                        columns[0] = rm.GetString("hdrEmployeeID", culture);
                        columns[1] = rm.GetString("hdrEmployee", culture);
                        columns[2] = rm.GetString("hdrShift", culture);
                        columns[3] = rm.GetString("hdrCycleDay", culture);
                        columns[4] = rm.GetString("hdrInterval", culture);
                        columns[5] = rm.GetString("hdrWeek", culture);
                        columns[6] = rm.GetString("hdrYear", culture);
                        columns[7] = rm.GetString("hdrMonth", culture);
                        columns[8] = rm.GetString("hdrDay", culture);
                        columns[9] = rm.GetString("hdrCostCenterCode", culture);
                        columns[10] = rm.GetString("hdrCostCenterName", culture);
                        columns[11] = rm.GetString("hdrWorkgroup", culture);
                        columns[12] = rm.GetString("hdrUte", culture);
                        columns[13] = rm.GetString("lblBranch", culture).Remove(rm.GetString("lblBranch", culture).IndexOf(':'));
                        columns[14] = rm.GetString("hdrEmplType", culture);
                        columns[15] = rm.GetString("hdrStartTime", culture);
                        columns[16] = rm.GetString("hdrEndTime", culture);
                        if (rbPassType.Checked)
                            columns[17] = rm.GetString("hdrPassType", culture);
                        else
                            columns[17] = rm.GetString("hdrLocation", culture);
                        columns[18] = rm.GetString("hdrTotal", culture);
                        columns[19] = rm.GetString("hdrDescription", culture);
                        columns[20] = rm.GetString("hdrPairDate", culture);

                        string columnsString = "";
                        foreach (string c in columns)
                        {
                            columnsString += c + "\t";
                        }
                        writer.WriteLine(columnsString);
                        int numr = 0;
                        foreach (List<object> currRow in resultTable)
                        {
                            numr++;
                            string resultStr = "";

                            if (currRow.Count < 18)
                                continue;                            

                            int empl = -1;
                            int day = -1;
                            int month = -1;
                            int year = -1;
                            if (currRow[0] is int)
                                empl = (int)currRow[0];
                            if (currRow[11] is int)
                                day = (int)currRow[11];
                            if (currRow[10] is int)
                                month = (int)currRow[10];
                            if (currRow[9] is int)
                                year = (int)currRow[9];

                            resultStr += currRow[0].ToString().Trim() + "\t";
                            resultStr += currRow[1].ToString().Trim() + "\t";

                            List<EmployeeTimeScheduleTO> schedules = new List<EmployeeTimeScheduleTO>();
                            DateTime ioPairDate = new DateTime();

                            if (year != -1 && month != -1 && day != -1) { ioPairDate = new DateTime(year, month, day); }
                            if (empl != -1)
                            {
                                if (emplSchedules.ContainsKey(empl))
                                    schedules = emplSchedules[empl];

                                WorkTimeSchemaTO sch = Common.Misc.getTimeSchema(ioPairDate, schedules, schemas);

                                if (sch.TimeSchemaID != -1)
                                    resultStr += sch.Description.Trim() + "\t";
                                else
                                    resultStr += "N/A" + "\t";

                                List<WorkTimeIntervalTO> intervals = Common.Misc.getTimeSchemaInterval(ioPairDate, schedules, schemas);

                                int cycleDay = 0;
                                string intervalsString = "";
                                foreach (WorkTimeIntervalTO interval in intervals)
                                {
                                    cycleDay = interval.DayNum + 1;

                                    intervalsString += interval.StartTime.ToString(Constants.timeFormat) + "-" + interval.EndTime.ToString(Constants.timeFormat);

                                    if (!interval.Description.Trim().Equals(""))
                                        intervalsString += "(" + interval.Description.Trim() + ")";

                                    intervalsString += "; ";
                                }

                                resultStr += cycleDay.ToString().Trim() + "\t";
                                resultStr += intervalsString.Trim() + "\t";
                            }

                            resultStr += currRow[8].ToString().Trim() + "\t";
                            resultStr += currRow[9].ToString().Trim() + "\t";
                            resultStr += currRow[10].ToString().Trim() + "\t";
                            resultStr += currRow[11].ToString().Trim() + "\t";
                            resultStr += currRow[2].ToString().Trim() + "\t";
                            resultStr += currRow[3].ToString().Trim() + "\t";
                            resultStr += currRow[4].ToString().Trim() + "\t";
                            resultStr += currRow[5].ToString().Trim() + "\t";
                            resultStr += currRow[6].ToString().Trim() + "\t";
                            resultStr += currRow[7].ToString().Trim() + "\t";                                                        
                            TimeSpan startTime = new TimeSpan();
                            TimeSpan endTime = new TimeSpan();
                            if (currRow[12] is TimeSpan)
                                startTime = (TimeSpan)currRow[12];
                            if (currRow[13] is TimeSpan)
                                endTime = (TimeSpan)currRow[13];
                            resultStr += startTime.ToString().Trim() + "\t";
                            resultStr += endTime.ToString().Trim() + "\t";
                            resultStr += currRow[14].ToString().Trim() + "\t";
                            double num = 0;
                            if (currRow[15] is TimeSpan)
                                num = Math.Round(((TimeSpan)currRow[15]).TotalHours, 2);

                            resultStr += num.ToString().Trim() + "\t";
                            resultStr += currRow[16].ToString().Trim() + "\t";
                            DateTime pairDate = new DateTime();
                            if (currRow[17] is DateTime)
                                pairDate = (DateTime)currRow[17];
                            resultStr += pairDate.ToString() + "\t";

                            writer.WriteLine(resultStr);
                        }

                        log.writeLog(numr + "  " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:sss"));
                        log.writeLog("Close" + "  " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:sss"));

                        writer.Close();
                        Response.Write("finished");

                        Session["DetailedDataReportFilePath"] = filePath;
                    }
                }
            }
            catch
            { }
        }

        private Dictionary<int, PassTypeTO> getTypes()
        {
            try
            {
                return new PassType(Session[Constants.sessionConnection]).SearchDictionary();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void rbLocation_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbLocation.Checked)
                    rbPassType.Checked = !rbLocation.Checked;

                populatePassTypes(Menu1.SelectedItem.Value.Equals("0"));

                SaveState();

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLDetailedDataPage.rbLocation_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCSelfTMDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void rbPassType_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbPassType.Checked)
                    rbLocation.Checked = !rbPassType.Checked;

                populatePassTypes(Menu1.SelectedItem.Value.Equals("0"));

                SaveState();

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLDetailedDataPage.rbPassType_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCSelfTMDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
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

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "TLDetailedDataPage.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "TLDetailedDataPage.", filterState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
