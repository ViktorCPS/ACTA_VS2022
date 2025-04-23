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
using System.Resources;
using System.Globalization;
using System.Drawing;

using Common;
using TransferObjects;
using Util;
using ReportsWeb;

namespace ACTAWebUI
{
    public partial class HRSSCOutstandingDataPage : System.Web.UI.Page
    {
        const string pageName = "HRSSCOutstandingDataPage";

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
        DateTime borderDate = DateTime.Now;
        protected void ShowCalendarBorder(object sender, EventArgs e)
        {
            calendarBorder.Visible = true;
            btnBorder.Visible = false;
        }
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
        protected void DataBorderChange(object sender, EventArgs e)
        {
            borderDate = calendarBorder.SelectedDate;
            tbBorderDate.Text = borderDate.ToString("dd.MM.yyyy.");
            calendarBorder.Visible = false;
            btnBorder.Visible = true;
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
                    btnBorderDate.Attributes.Add("onclick", "return calendarPicker('tbBorderDate', 'true');");
                    btnWUTree.Attributes.Add("onclick", "return wUnitsPicker('btnWUTree', 'false');");
                    btnOrgTree.Attributes.Add("onclick", "return oUnitsPicker('btnOrgTree', 'false');");
                    tbEmployee.Attributes.Add("onKeyUp", "return selectItem('tbEmployee', 'lboxEmployees');");
                    btnShow.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnReport.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");

                    btnWUTree.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnWUTree.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnOrgTree.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnOrgTree.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnFromDate.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnFromDate.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnToDate.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnToDate.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnPrevDayPeriod.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnPrevDayPeriod.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnNextDayPeriod.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnNextDayPeriod.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");

                    cbShowRetired.Checked = false;
                    tbBorderDate.Text = DateTime.Now.Date.ToString(Constants.dateFormat);
                    tbBorderDate.Enabled = false;

                    if (Session[Constants.sessionFromDate] != null && Session[Constants.sessionFromDate] is DateTime)
                        tbFromDate.Text = ((DateTime)Session[Constants.sessionFromDate]).ToString(Constants.dateFormat.Trim());
                    else
                        tbFromDate.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());

                    if (Session[Constants.sessionToDate] != null && Session[Constants.sessionToDate] is DateTime)
                        tbToDate.Text = ((DateTime)Session[Constants.sessionToDate]).ToString(Constants.dateFormat.Trim());
                    else
                        tbToDate.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());

                    // populate default working unit and organizational unit info
                    // get responsibility working unit (employee_asco4.integer_value_2)
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

                    if (defaultWUID != -1 && Session[Constants.sessionLoginCategoryWUnits] != null && Session[Constants.sessionLoginCategoryWUnits] is List<int>
                        && ((List<int>)Session[Constants.sessionLoginCategoryWUnits]).Contains(defaultWUID))
                    {
                        populateWU(defaultWUID);
                    }
                    if (defaultOUID != -1 && Session[Constants.sessionLoginCategoryOUnits] != null && Session[Constants.sessionLoginCategoryOUnits] is List<int>
                       && ((List<int>)Session[Constants.sessionLoginCategoryOUnits]).Contains(defaultOUID))
                    {
                        populateOU(defaultOUID);
                    }

                    if (Session[Constants.sessionWU] == null && Session[Constants.sessionOU] == null)
                    {
                        if (defaultWUID == -1 && defaultOUID != -1)
                        {
                            Menu1.Items[1].Selected = true;
                            MultiView1.SetActiveView(MultiView1.Views[1]);
                            Session[Constants.sessionOU] = defaultOUID;
                        }
                        else
                        {
                            Menu1.Items[0].Selected = true;
                            MultiView1.SetActiveView(MultiView1.Views[0]);
                            Session[Constants.sessionWU] = defaultWUID;
                        }
                    }
                    else
                    {
                        if (Session[Constants.sessionOU] != null && Session[Constants.sessionOU] is int)
                        {
                            populateOU((int)Session[Constants.sessionOU]);

                            Menu1.Items[1].Selected = true;
                            MultiView1.SetActiveView(MultiView1.Views[1]);
                        }
                        else if (Session[Constants.sessionWU] != null && Session[Constants.sessionWU] is int)
                        {
                            populateWU((int)Session[Constants.sessionWU]);

                            Menu1.Items[0].Selected = true;
                            MultiView1.SetActiveView(MultiView1.Views[0]);
                        }
                    }

                    for (int i = 0; i < Menu1.Items.Count; i++)
                    {
                        CommonWeb.Misc.setMenuImage(Menu1, i, Menu1.Items[i].Selected);
                        CommonWeb.Misc.setMenuSeparator(Menu1, i, Menu1.Items[i].Selected);
                    }

                    setLanguage();

                    rbStandard.Checked = true;
                    rbStandard_ChackedChanged(this, new EventArgs());

                    bool isHRLE = Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                        && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRLegalEntity;

                    rbStandard.Visible = rbHoursDiff.Visible = !isHRLE;

                    InitializeSQLParameters(new Dictionary<int, string>());

                    chbHoursDiff.Checked = true;

                    if (Request.QueryString["reloadState"] != null && Request.QueryString["reloadState"].Trim().ToUpper().Equals(Constants.falseValue.Trim().ToUpper()))
                    {
                        ClearSessionValues();

                        if (Menu1.SelectedItem.Value.Equals("0"))
                        {
                            int wuID = -1;
                            if (tbUte.Attributes["id"] != null)
                                if (!int.TryParse(tbUte.Attributes["id"].Trim(), out wuID))
                                    wuID = -1;
                            populateEmployees(wuID, true);
                        }
                        else
                        {
                            int ouID = -1;
                            if (tbOrgUte.Attributes["id"] != null)
                                if (!int.TryParse(tbOrgUte.Attributes["id"].Trim(), out ouID))
                                    ouID = -1;
                            populateEmployees(ouID, false);
                        }

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
                        if (Menu1.SelectedItem.Value.Equals("0"))
                        {
                            int wuID = -1;
                            if (tbUte.Attributes["id"] != null)
                                if (!int.TryParse(tbUte.Attributes["id"].Trim(), out wuID))
                                    wuID = -1;
                            populateEmployees(wuID, true);
                        }
                        else
                        {
                            int ouID = -1;
                            if (tbOrgUte.Attributes["id"] != null)
                                if (!int.TryParse(tbOrgUte.Attributes["id"].Trim(), out ouID))
                                    ouID = -1;
                            populateEmployees(ouID, false);
                        }
                        //  InitializeSQLParameters();
                        // do again load state to select previously selected employees in employees list
                        rbStandard_ChackedChanged(this, new EventArgs());
                        LoadState();
                        btnShow_Click(this, new EventArgs());
                    }

                    resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?showSelection=false";
                }
                else
                {
                    // if there is selected wuID in wu tree pop up, populate with that working unit
                    if (Session[Constants.sessionSelectedWUID] != null)
                    {
                        ClientScript.GetPostBackClientHyperlink(btnWUTree, "");

                        int wuID = -1;
                        if (!int.TryParse(Session[Constants.sessionSelectedWUID].ToString(), out wuID))
                            wuID = -1;

                        if (wuID != -1)
                        {
                            populateWU(wuID);
                            populateEmployees(wuID, true);
                        }
                    }
                    else if (Session[Constants.sessionSelectedOUID] != null)
                    {
                        ClientScript.GetPostBackClientHyperlink(btnOrgTree, "");

                        int ouID = -1;
                        if (!int.TryParse(Session[Constants.sessionSelectedOUID].ToString(), out ouID))
                            ouID = -1;

                        if (ouID != -1)
                        {
                            populateOU(ouID);
                            populateEmployees(ouID, false);
                        }
                    }
                }

                Session[Constants.sessionSelectedWUID] = null;
                Session[Constants.sessionSelectedOUID] = null;

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in HRSSCOutstandingDataPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/HRSSCOutstandingDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void InitializeSQLParameters(Dictionary<int, string> categories)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(HRSSCOutstandingDataPage).Assembly);

                Session[Constants.sessionHeader] = rm.GetString("hdrCostCentre", culture) + "," + rm.GetString("hdrCostCenterName", culture) + "," + rm.GetString("hdrWorkgroup", culture) + "," + rm.GetString("hdrUte", culture) + ","
                     + rm.GetString("hdrBranch", culture) + "," + rm.GetString("hdrEmployee", culture) + "," + rm.GetString("hdrEmployeeID", culture) + ",";

                if (rbStandard.Checked)
                    Session[Constants.sessionHeader] += rm.GetString("hdrDate", culture) + "," + rm.GetString("hdrAnomalyCategory", culture) + ",";
                else
                {
                    if (categories.ContainsKey((int)Constants.AnomalyCategories.GroupHours))
                        Session[Constants.sessionHeader] += rm.GetString("hdrGrpHrs", culture) + ",";
                    if (categories.ContainsKey((int)Constants.AnomalyCategories.ScheduleHours))
                        Session[Constants.sessionHeader] += rm.GetString("hdrSchHrs", culture) + ",";
                    if (categories.ContainsKey((int)Constants.AnomalyCategories.PairsHours))
                        Session[Constants.sessionHeader] += rm.GetString("hdrPairHrs", culture) + ",";
                }
                Session[Constants.sessionHeader] += rm.GetString("hdrEmplType", culture);
                Session[Constants.sessionFields] = "costcenter, ccDesc, workgroup,ute,branch, emplname, emplid, ";
                int doubleFieldsNum = 0;
                if (rbStandard.Checked)
                    Session[Constants.sessionFields] += "empldate, anomalycategory, ";
                else
                {                    
                    if (categories.ContainsKey((int)Constants.AnomalyCategories.GroupHours))
                    {
                        Session[Constants.sessionFields] += "groupHrs, ";
                        doubleFieldsNum++;
                    }
                    if (categories.ContainsKey((int)Constants.AnomalyCategories.ScheduleHours))
                    {
                        Session[Constants.sessionFields] += "schHrs, ";
                        doubleFieldsNum++;
                    }
                    if (categories.ContainsKey((int)Constants.AnomalyCategories.PairsHours))
                    {
                        Session[Constants.sessionFields] += "pairHrs, ";
                        doubleFieldsNum++;
                    }
                }
                Session[Constants.sessionFields] += "empltype";
                Dictionary<int, int> formating = new Dictionary<int, int>();
                if (rbStandard.Checked)                    
                    formating.Add(7, (int)Constants.FormatTypes.DateFormat);
                else
                {                    
                    for (int i = 7; i < 7 + doubleFieldsNum; i++)
                    {
                        formating.Add(i, (int)Constants.FormatTypes.DoubleFormat);
                    }                   
                }
                Session[Constants.sessionFieldsFormating] = formating;
                Session[Constants.sessionFieldsFormatedValues] = null;
                Session[Constants.sessionTables] = null;
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

        protected void btnPrevDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(HRSSCOutstandingDataPage).Assembly);

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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in HRSSCOutstandingDataPage.btnPrevDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/HRSSCOutstandingDataPage.aspx&Header=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnNextDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(HRSSCOutstandingDataPage).Assembly);

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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in HRSSCOutstandingDataPage.btnNextDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/HRSSCOutstandingDataPage.aspx&Header=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void populateWU(int wuID)
        {
            try
            {
                WorkingUnitTO wu = new WorkingUnit(Session[Constants.sessionConnection]).FindWU(wuID);
                WorkingUnit wUnit = new WorkingUnit(Session[Constants.sessionConnection]);
                wUnit.WUTO = wu;
                tbWorkshop.Text = wUnit.getParentWorkingUnit().Name.Trim();
                tbUte.Attributes.Add("id", wuID.ToString());
                tbUte.Text = wu.Name;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populateOU(int ouID)
        {
            try
            {
                OrganizationalUnitTO ou = new OrganizationalUnit(Session[Constants.sessionConnection]).Find(ouID);
                tbOrg.Text = new OrganizationalUnit(Session[Constants.sessionConnection]).Find(ou.ParentOrgUnitID).Name.Trim();
                tbOrgUte.Attributes.Add("id", ouID.ToString());
                tbOrgUte.Text = ou.Name.Trim();
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
                if (Menu1.SelectedItem.Value.Equals("0"))
                {
                    int wuID = -1;
                    if (tbUte.Attributes["id"] != null)
                        if (!int.TryParse(tbUte.Attributes["id"].Trim(), out wuID))
                            wuID = -1;
                    populateEmployees(wuID, true);
                }
                else
                {
                    int ouID = -1;
                    if (tbOrgUte.Attributes["id"] != null)
                        if (!int.TryParse(tbOrgUte.Attributes["id"].Trim(), out ouID))
                            ouID = -1;
                    populateEmployees(ouID, false);
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in HRSSCOutstandingDataPage.Date_Changed(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/HRSSCOutstandingDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void populateEmployees(int ID, bool isWU)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(HRSSCOutstandingDataPage).Assembly);

                lblError.Text = "";

                //check inserted date
                if (tbFromDate.Text.Trim().Equals("") || tbToDate.Text.Trim().Equals(""))
                {
                    lblError.Text = rm.GetString("noDateInterval", culture);

                    if (tbFromDate.Text.Trim().Equals(""))
                        tbFromDate.Focus();
                    else if (tbToDate.Text.Trim().Equals(""))
                        tbToDate.Focus();
                    return;
                }

                DateTime from = CommonWeb.Misc.createDate(tbFromDate.Text.Trim());
                DateTime to = CommonWeb.Misc.createDate(tbToDate.Text.Trim());
                DateTime border = CommonWeb.Misc.createDate(tbBorderDate.Text.Trim());

                //date validation
                if (from.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidFromDate", culture);
                    return;
                }

                if (to.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidToDate", culture);
                    return;
                }

                if (from > to)
                {
                    lblError.Text = rm.GetString("invalidFromToDate", culture);
                    return;
                }

                // check border date
                if (cbShowRetired.Checked)
                {
                    if (border.Equals(new DateTime()) || border.Date < from.Date || border.Date > to.Date)
                    {
                        lblError.Text = rm.GetString("invalidBorderDate", culture);
                        return;
                    }
                }

                List<EmployeeTO> empolyeeList = new List<EmployeeTO>();

                //int onlyEmplTypeID = -1;
                //int exceptEmplTypeID = -1;
                int emplID = -1;

                Dictionary<int, List<int>> companyVisibleTypes = new Dictionary<int, List<int>>();
                List<int> typesVisible = new List<int>();

                if (Session[Constants.sessionVisibleEmplTypes] != null && Session[Constants.sessionVisibleEmplTypes] is Dictionary<int, List<int>>)
                    companyVisibleTypes = (Dictionary<int, List<int>>)Session[Constants.sessionVisibleEmplTypes];

                // get company
                Dictionary<int, WorkingUnitTO> wuDict = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                int company = -1;
                if (isWU)
                    company = Common.Misc.getRootWorkingUnit(ID, wuDict);
                else
                {
                    WorkingUnitXOrganizationalUnit wuXou = new WorkingUnitXOrganizationalUnit(Session[Constants.sessionConnection]);
                    wuXou.WUXouTO.OrgUnitID = ID;
                    List<WorkingUnitXOrganizationalUnitTO> list = wuXou.Search();
                    if (list.Count > 0)
                        company = Common.Misc.getRootWorkingUnit(list[0].WorkingUnitID, wuDict);
                }

                if (companyVisibleTypes.ContainsKey(company))
                    typesVisible = companyVisibleTypes[company];

                // 09.01.2012. Sanja - do not exclude login employee from reports
                //if (Session[Constants.sessionLogInEmployee] != null && Session[Constants.sessionLogInEmployee] is EmployeeTO)
                //    emplID = ((EmployeeTO)Session[Constants.sessionLogInEmployee]).EmployeeID;

                if (ID != -1)
                {
                    if (isWU)
                    {
                        // get employees from selected working unit that are not currently loaned to other working unit or are currently loand to selected working unit                    
                        if (Session[Constants.sessionLoginCategoryWUnits] != null && Session[Constants.sessionLoginCategoryWUnits] is List<int>)
                        {
                            empolyeeList = new Employee(Session[Constants.sessionConnection]).SearchByWULoans(Common.Misc.getWorkingUnitHierarhicly(ID, (List<int>)Session[Constants.sessionLoginCategoryWUnits], Session[Constants.sessionConnection]), emplID, typesVisible, from, to);
                        }
                    }
                    else if (!isWU)
                    {
                        if (Session[Constants.sessionLoginCategoryOUnits] != null && Session[Constants.sessionLoginCategoryOUnits] is List<int>)
                        {
                            empolyeeList = new Employee(Session[Constants.sessionConnection]).SearchByOU(Common.Misc.getOrgUnitHierarhicly(ID.ToString(), (List<int>)Session[Constants.sessionLoginCategoryOUnits], Session[Constants.sessionConnection]), emplID, typesVisible, from, to);
                        }
                    }
                }                

                List<EmployeeTO> shownEmployees = new List<EmployeeTO>();

                if (cbShowRetired.Checked)
                {
                    string emplIDs = "";
                    foreach (EmployeeTO empl in empolyeeList)
                    {
                        emplIDs += empl.EmployeeID.ToString().Trim() + ",";
                    }

                    Dictionary<int, EmployeeAsco4TO> ascoDict = new Dictionary<int,EmployeeAsco4TO>();
                    if (emplIDs.Length > 0)
                    {
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                        ascoDict = new EmployeeAsco4(Session[Constants.sessionConnection]).SearchDictionary(emplIDs);
                    }

                    foreach (EmployeeTO empl in empolyeeList)
                    {
                        if (empl.Status.Trim().ToUpper().Equals(Constants.statusRetired.Trim().ToUpper()) && ascoDict.ContainsKey(empl.EmployeeID)
                            && ascoDict[empl.EmployeeID].DatetimeValue3.Date > from.Date
                            && ascoDict[empl.EmployeeID].DatetimeValue3.Date <= border.AddDays(1).Date)
                            {
                                shownEmployees.Add(empl);
                            }
                            else
                                continue;
                    }
                }
                else
                    shownEmployees = empolyeeList;

                lboxEmployees.DataSource = shownEmployees;
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
                    if (tbUte.Attributes["id"] != null)
                        if (!int.TryParse(tbUte.Attributes["id"].Trim(), out wuID))
                            wuID = -1;
                    populateEmployees(wuID, true);
                    Session[Constants.sessionWU] = wuID;
                    Session[Constants.sessionOU] = null;
                }
                else
                {
                    int ouID = -1;
                    if (tbOrgUte.Attributes["id"] != null)
                        if (!int.TryParse(tbOrgUte.Attributes["id"].Trim(), out ouID))
                            ouID = -1;
                    populateEmployees(ouID, false);
                    Session[Constants.sessionOU] = ouID;
                    Session[Constants.sessionWU] = null;
                }

                // save selected filter state
                SaveState();

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in HRSSCOutstandingDataPage.Menu1_MenuItemClick(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/HRSSCOutstandingDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void populateAnomalies()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(HRSSCOutstandingDataPage).Assembly);

                List<AnomalyCategory> categoryList = new List<AnomalyCategory>();

                bool isHRLE = Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                    && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRLegalEntity;

                if (rbStandard.Checked)
                {
                    AnomalyCategory cat = null;
                    if (!isHRLE)
                    {
                        cat = new AnomalyCategory(rm.GetString("notverified", culture), (int)Constants.AnomalyCategories.NotVerified);
                        categoryList.Add(cat);
                        cat = new AnomalyCategory(rm.GetString("notconfirmed", culture), (int)Constants.AnomalyCategories.NotConfirmed);
                        categoryList.Add(cat);
                        cat = new AnomalyCategory(rm.GetString("unjustified", culture), (int)Constants.AnomalyCategories.Unjustified);
                        categoryList.Add(cat);
                        cat = new AnomalyCategory(rm.GetString("overtimetojustify", culture), (int)Constants.AnomalyCategories.OvertimeToJusitify);
                        categoryList.Add(cat);
                        cat = new AnomalyCategory(rm.GetString("nightovertimetojustify", culture), (int)Constants.AnomalyCategories.NightOvertimeToJusitify);
                        categoryList.Add(cat);
                        cat = new AnomalyCategory(rm.GetString("negativeBuffers", culture), (int)Constants.AnomalyCategories.NegativeBuffers);
                        categoryList.Add(cat);
                        cat = new AnomalyCategory(rm.GetString("notRegularSickLeaveRefundation", culture), (int)Constants.AnomalyCategories.SickLeave);
                        categoryList.Add(cat);
                        cat = new AnomalyCategory(rm.GetString("notRegularMeals", culture), (int)Constants.AnomalyCategories.Canteen);
                        categoryList.Add(cat);
                    }
                    cat = new AnomalyCategory(rm.GetString("laborLawShift", culture), (int)Constants.AnomalyCategories.LaborLawShift);
                    categoryList.Add(cat);
                    cat = new AnomalyCategory(rm.GetString("laborLaw", culture), (int)Constants.AnomalyCategories.LaborLaw);
                    categoryList.Add(cat);
                }
                else
                {
                    AnomalyCategory cat = new AnomalyCategory(rm.GetString("grouphours", culture), (int)Constants.AnomalyCategories.GroupHours);
                    categoryList.Add(cat);
                    cat = new AnomalyCategory(rm.GetString("schedulehours", culture), (int)Constants.AnomalyCategories.ScheduleHours);
                    categoryList.Add(cat);
                    cat = new AnomalyCategory(rm.GetString("pairshours", culture), (int)Constants.AnomalyCategories.PairsHours);
                    categoryList.Add(cat);
                }

                lbAnomalies.DataSource = categoryList;
                lbAnomalies.DataTextField = "Name";
                lbAnomalies.DataValueField = "ID";
                lbAnomalies.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(HRSSCOutstandingDataPage).Assembly);

                lblEmployee.Text = rm.GetString("lblEmployeeSelection", culture);
                lblPeriod.Text = rm.GetString("lblPeriod", culture);
                lblAnomalyCategory.Text = rm.GetString("lblAnomalyCategory", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblBorderDate.Text = rm.GetString("lblBorderDate", culture);
                cbShowRetired.Text = rm.GetString("cbShowRetired", culture);
                chbHoursDiff.Text = rm.GetString("chbHoursDiff", culture);

                rbStandard.Text = rm.GetString("rbStandard", culture);
                rbHoursDiff.Text = rm.GetString("rbHoursDiff", culture);

                btnShow.Text = rm.GetString("btnShow", culture);
                btnReport.Text = rm.GetString("btnReport", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnShow_Click(Object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(HRSSCOutstandingDataPage).Assembly);

                ClearSessionValues();
                lblError.Text = "";
                Session["HRSSCOutstandingDataPage.group_hours"] = null;
                Session["HRSSCOutstandingDataPage.schema_hours"] = null;
                Session["HRSSCOutstandingDataPage.effective_hours"] = null;
                Session[Constants.sessionDataTableList] = null;
                Session[Constants.sessionDataTableColumns] = null;              
               
                // check if there are some employees in list
                if (lboxEmployees.Items.Count <= 0)
                {
                    lblError.Text = rm.GetString("noEmployeesForReport", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

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
                DateTime border = CommonWeb.Misc.createDate(tbBorderDate.Text.Trim());

                DateTime from = Common.Misc.getWeekBeggining(fromDate.Date).AddDays(-7);
                DateTime to = Common.Misc.getWeekEnding(toDate.Date);

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

                if (fromDate > toDate)
                {
                    lblError.Text = rm.GetString("invalidFromToDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                // check border date
                if (cbShowRetired.Checked)
                {
                    if (border.Equals(new DateTime()) || border.Date < fromDate.Date || border.Date > toDate.Date)
                    {
                        lblError.Text = rm.GetString("invalidBorderDate", culture);
                        return;
                    }
                }

                Session[Constants.sessionFromDate] = fromDate;
                Session[Constants.sessionToDate] = toDate;

                // get selected categories
                Dictionary<int, string> categories = new Dictionary<int, string>();

                if (lbAnomalies.GetSelectedIndices().Length > 0)
                {
                    foreach (int index in lbAnomalies.GetSelectedIndices())
                    {
                        if (index >= 0 && index < lbAnomalies.Items.Count)
                        {
                            int id = -1;
                            if (!int.TryParse(lbAnomalies.Items[index].Value, out id))
                                id = -1;

                            if (id != -1 && !categories.ContainsKey(id))
                                categories.Add(id, lbAnomalies.Items[index].Text);
                        }
                    }
                }
                else
                {
                    foreach (ListItem item in lbAnomalies.Items)
                    {
                        int id = -1;
                        if (!int.TryParse(item.Value, out id))
                            id = -1;

                        if (id != -1 && !categories.ContainsKey(id))
                            categories.Add(id, item.Text);
                    }
                }
                                
                int company = -1;
                Dictionary<int, WorkingUnitTO> WUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                if (Menu1.SelectedItem.Value.Equals("0"))
                {
                    int wuID = -1;
                    if (tbUte.Attributes["id"] != null)
                        if (!int.TryParse(tbUte.Attributes["id"].Trim(), out wuID))
                            wuID = -1;
                    company = Common.Misc.getRootWorkingUnit(wuID, WUnits);
                }
                else
                {
                    int ouID = -1;
                    if (tbOrgUte.Attributes["id"] != null)
                        if (!int.TryParse(tbOrgUte.Attributes["id"].Trim(), out ouID))
                            ouID = -1;
                    if (ouID >= 0)
                    {
                        WorkingUnitXOrganizationalUnit wuXou = new WorkingUnitXOrganizationalUnit(Session[Constants.sessionConnection]);
                        wuXou.WUXouTO.OrgUnitID = ouID;
                        List<WorkingUnitXOrganizationalUnitTO> list = wuXou.Search();
                        if (list.Count > 0)
                            company = Common.Misc.getRootWorkingUnit(list[0].WorkingUnitID, WUnits);
                    }
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

                // get selected employees and additional data for stringone
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

                List<EmployeeTO> emplList = new Employee(Session[Constants.sessionConnection]).Search(emplIDs);

                Dictionary<int, EmployeeTO> employees = new Dictionary<int, EmployeeTO>();
                string emplGroupsIDs = "";
                foreach (EmployeeTO empl in emplList)
                {
                    if (!employees.ContainsKey(empl.EmployeeID))
                        employees.Add(empl.EmployeeID, empl);
                    else
                        employees[empl.EmployeeID] = empl;

                    emplGroupsIDs += empl.WorkingGroupID.ToString().Trim() + ",";
                }

                if (emplGroupsIDs.Trim().Length > 0)
                    emplGroupsIDs = emplGroupsIDs.Substring(0, emplGroupsIDs.Length - 1);
                
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
                    //if (WUnits.ContainsKey(tempWU.ParentWorkingUID))
                    //    tempWU = WUnits[tempWU.ParentWorkingUID];

                    //string plant = tempWU.Code.Trim();
                    if (!costcenterList.ContainsKey(asco.EmployeeID))                        
                        costcenterList.Add(asco.EmployeeID, cost);                    
                    else
                        costcenterList[asco.EmployeeID] = cost;

                    if (!costcenterListDesc.ContainsKey(asco.EmployeeID))
                        costcenterListDesc.Add(asco.EmployeeID, costDesc);
                    else
                        costcenterListDesc[asco.EmployeeID] = costDesc;     
                }

                InitializeSQLParameters(categories);
                                
                List<List<object>> resultTable = new List<List<object>>();

                // create table for result and list of pairs                
                List<DataColumn> resultColumns = new List<DataColumn>();
                resultColumns.Add(new DataColumn("costcenter", typeof(string)));
                resultColumns.Add(new DataColumn("ccDesc", typeof(string)));
                resultColumns.Add(new DataColumn("workgroup", typeof(string)));
                resultColumns.Add(new DataColumn("ute", typeof(string)));
                resultColumns.Add(new DataColumn("branch", typeof(string)));
                resultColumns.Add(new DataColumn("emplname", typeof(string)));
                resultColumns.Add(new DataColumn("emplid", typeof(int)));
                if (rbStandard.Checked)
                {
                    Session["HRSSCOutstandingDataPage.standard"] = 1;
                    resultColumns.Add(new DataColumn("empldate", typeof(DateTime)));
                    resultColumns.Add(new DataColumn("anomalycategory", typeof(string)));
                }
                else
                {
                    Session["HRSSCOutstandingDataPage.standard"] = 2;
                    if (categories.ContainsKey((int)Constants.AnomalyCategories.GroupHours))
                    {
                        Session["HRSSCOutstandingDataPage.group_hours"] = 1;
                        resultColumns.Add(new DataColumn("groupHrs", typeof(double)));
                    }
                    if (categories.ContainsKey((int)Constants.AnomalyCategories.ScheduleHours))
                    {
                        Session["HRSSCOutstandingDataPage.schema_hours"] = 1;
                        resultColumns.Add(new DataColumn("schHrs", typeof(double)));
                    }
                    if (categories.ContainsKey((int)Constants.AnomalyCategories.PairsHours))
                    {
                        Session["HRSSCOutstandingDataPage.effective_hours"] = 1;
                        resultColumns.Add(new DataColumn("pairHrs", typeof(double)));
                    }
                }
                resultColumns.Add(new DataColumn("empltype", typeof(string)));                

                List<DateTime> datesList = new List<DateTime>();
                DateTime date = fromDate.Date;
                while (date <= toDate.AddDays(1).Date)
                {
                    datesList.Add(date);
                    date = date.AddDays(1);
                }

                List<IOPairProcessedTO> pairs = new IOPairProcessed(Session[Constants.sessionConnection]).SearchAllPairsForEmpl(emplIDs, datesList, "");

                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplDayPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                Dictionary<int, Dictionary<DateTime, WorkTimeSchemaTO>> emplDaySchemas = new Dictionary<int, Dictionary<DateTime, WorkTimeSchemaTO>>();
                Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>> emplDayIntervals = new Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>>();

                Dictionary<int, WorkTimeSchemaTO> schemas = new TimeSchema(Session[Constants.sessionConnection]).getDictionary();
                Dictionary<int, PassTypeTO> passTypesAll = new PassType(Session[Constants.sessionConnection]).SearchDictionary();
                                
                // get time schedules and time schemas for selected employees for selected days, add one day for third shifts
                // 03.07.2014. Sanja - get schedules from fromDate week beggining, until toDate weekEnding becouse of labor law check        
                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule(Session[Constants.sessionConnection]).SearchEmployeesSchedulesExactDate(emplIDs, from.Date, to.AddDays(1).Date, null);
                
                // get night work rules
                Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> emplNightWorkRules = new Common.Rule(Session[Constants.sessionConnection]).SearchTypeAllRules(Constants.RuleNightWork);

                // get initital night overtime type
                Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> emplNightOvertimeRules = new Common.Rule(Session[Constants.sessionConnection]).SearchTypeAllRules(Constants.RuleCompanyInitialNightOvertime);
                
                // get overtime rejected rules
                Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> emplOvertimeRejectedRules = new Common.Rule(Session[Constants.sessionConnection]).SearchTypeAllRules(Constants.RuleCompanyOvertimeRejected);
                
                Dictionary<int, Dictionary<string, RuleTO>> emplNightWork = new Dictionary<int, Dictionary<string, RuleTO>>();
                Dictionary<int, Dictionary<string, RuleTO>> emplNightOvertime = new Dictionary<int, Dictionary<string, RuleTO>>();
                Dictionary<int, Dictionary<string, RuleTO>> emplOvertimeRejected = new Dictionary<int, Dictionary<string, RuleTO>>();

                // get intervals and schemas by employees and dates
                foreach (int emplID in employees.Keys)
                {
                    DateTime currDate = fromDate.Date;
                    List<EmployeeTimeScheduleTO> emplScheduleList = new List<EmployeeTimeScheduleTO>();

                    if (emplSchedules.ContainsKey(emplID))
                        emplScheduleList = emplSchedules[emplID];
                                       
                    while (currDate <= toDate.AddDays(1))
                    {
                        if (!emplDayIntervals.ContainsKey(emplID))
                            emplDayIntervals.Add(emplID, new Dictionary<DateTime, List<WorkTimeIntervalTO>>());

                        if (!emplDayIntervals[emplID].ContainsKey(currDate.Date))
                            emplDayIntervals[emplID].Add(currDate.Date, Common.Misc.getTimeSchemaInterval(currDate.Date, emplScheduleList, schemas));

                        if (!emplDaySchemas.ContainsKey(emplID))
                            emplDaySchemas.Add(emplID, new Dictionary<DateTime, WorkTimeSchemaTO>());

                        WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                        if (emplDayIntervals[emplID][currDate.Date].Count > 0 && schemas.ContainsKey(emplDayIntervals[emplID][currDate.Date][0].TimeSchemaID))
                            sch = schemas[emplDayIntervals[emplID][currDate.Date][0].TimeSchemaID];

                        if (!emplDaySchemas[emplID].ContainsKey(currDate.Date))
                            emplDaySchemas[emplID].Add(currDate.Date, sch);

                        currDate = currDate.AddDays(1).Date;
                    }

                    int emplCompany = Common.Misc.getRootWorkingUnit(employees[emplID].WorkingUnitID, WUnits);

                    if (emplNightWorkRules.ContainsKey(emplCompany) && emplNightWorkRules[emplCompany].ContainsKey(employees[emplID].EmployeeTypeID))
                    {
                        if (!emplNightWork.ContainsKey(emplID))
                            emplNightWork.Add(emplID, new Dictionary<string, RuleTO>());

                        emplNightWork[emplID] = emplNightWorkRules[emplCompany][employees[emplID].EmployeeTypeID];
                    }

                    if (emplNightOvertimeRules.ContainsKey(emplCompany) && emplNightOvertimeRules[emplCompany].ContainsKey(employees[emplID].EmployeeTypeID))
                    {
                        if (!emplNightOvertime.ContainsKey(emplID))
                            emplNightOvertime.Add(emplID, new Dictionary<string, RuleTO>());

                        emplNightOvertime[emplID] = emplNightOvertimeRules[emplCompany][employees[emplID].EmployeeTypeID];
                    }

                    if (emplOvertimeRejectedRules.ContainsKey(emplCompany) && emplOvertimeRejectedRules[emplCompany].ContainsKey(employees[emplID].EmployeeTypeID))
                    {
                        if (!emplOvertimeRejected.ContainsKey(emplID))
                            emplOvertimeRejected.Add(emplID, new Dictionary<string, RuleTO>());

                        emplOvertimeRejected[emplID] = emplOvertimeRejectedRules[emplCompany][employees[emplID].EmployeeTypeID];
                    }
                }

                // get day pairs for employees
                foreach (IOPairProcessedTO pair in pairs)
                {
                    if (!emplDayPairs.ContainsKey(pair.EmployeeID))
                        emplDayPairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());

                    if (!emplDayPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                        emplDayPairs[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<IOPairProcessedTO>());

                    emplDayPairs[pair.EmployeeID][pair.IOPairDate.Date].Add(pair);
                }

                if (rbStandard.Checked)
                {
                    // get refundation types
                    List<int> refundationTypes = new Common.Rule(Session[Constants.sessionConnection]).SearchRules(Constants.RuleSickLeavesRefundationType);
                    
                    // key is employee id, value is dictionary with date and anomaly categories for that date
                    Dictionary<int, Dictionary<DateTime, List<int>>> emplDateAnomalyCategories = new Dictionary<int, Dictionary<DateTime, List<int>>>();

                    // duration of refundation types by employee, date, type
                    Dictionary<int, Dictionary<DateTime, Dictionary<int, int>>> refundationTypesDuration = new Dictionary<int,Dictionary<DateTime,Dictionary<int,int>>>();

                    foreach (IOPairProcessedTO pair in pairs)
                    {
                        DateTime pairDate = pair.IOPairDate.Date;
                        // skip all pairs from first day that belong to previous day and add pairs from day after last that belong to last day
                        if (pair.IOPairDate.Date.Equals(toDate.AddDays(1).Date) || pair.IOPairDate.Date.Equals(fromDate.Date) 
                            || categories.ContainsKey((int)Constants.AnomalyCategories.SickLeave))
                        {
                            List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                            if (emplDayPairs.ContainsKey(pair.EmployeeID) && emplDayPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                                dayPairs = emplDayPairs[pair.EmployeeID][pair.IOPairDate.Date];

                            WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                            if (emplDaySchemas.ContainsKey(pair.EmployeeID) && emplDaySchemas[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                                sch = emplDaySchemas[pair.EmployeeID][pair.IOPairDate.Date];

                            List<WorkTimeIntervalTO> dayIntervals = new List<WorkTimeIntervalTO>();
                            if (emplDayIntervals.ContainsKey(pair.EmployeeID) && emplDayIntervals[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                                dayIntervals = emplDayIntervals[pair.EmployeeID][pair.IOPairDate.Date];

                            bool previousDayPair = CommonWeb.Misc.isPreviousDayPair(pair, passTypesAll, dayPairs, sch, dayIntervals);

                            if (previousDayPair)
                                pairDate = pairDate.AddDays(-1).Date;

                            if (pair.IOPairDate.Date.Equals(fromDate.Date) && previousDayPair)
                                continue;

                            if (pair.IOPairDate.Date.Equals(toDate.AddDays(1).Date) && !previousDayPair)
                                continue;
                        }

                        int catID = -1;

                        // calculate duration if type is for refundation
                        if (refundationTypes.Contains(pair.PassTypeID))
                        {
                            int duration = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;

                            if (pair.EndTime.Hour == 23 && pair.EndTime.Minute == 59)
                                duration++;

                            if (!refundationTypesDuration.ContainsKey(pair.EmployeeID))
                                refundationTypesDuration.Add(pair.EmployeeID, new Dictionary<DateTime,Dictionary<int,int>>());
                            if (!refundationTypesDuration[pair.EmployeeID].ContainsKey(pairDate))
                                refundationTypesDuration[pair.EmployeeID].Add(pairDate, new Dictionary<int,int>());
                            if (!refundationTypesDuration[pair.EmployeeID][pairDate].ContainsKey(pair.PassTypeID))
                                refundationTypesDuration[pair.EmployeeID][pairDate].Add(pair.PassTypeID, duration);
                            else
                                refundationTypesDuration[pair.EmployeeID][pairDate][pair.PassTypeID] += duration;
                        }

                        // set anomaly category if exists
                        if (pair.PassTypeID == Constants.absence)
                        {
                            catID = (int)Constants.AnomalyCategories.Unjustified;
                            if (categories.ContainsKey(catID))
                            {
                                if (!emplDateAnomalyCategories.ContainsKey(pair.EmployeeID))
                                    emplDateAnomalyCategories.Add(pair.EmployeeID, new Dictionary<DateTime, List<int>>());

                                if (!emplDateAnomalyCategories[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                                    emplDateAnomalyCategories[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<int>());

                                if (!emplDateAnomalyCategories[pair.EmployeeID][pair.IOPairDate.Date].Contains(catID))
                                    emplDateAnomalyCategories[pair.EmployeeID][pair.IOPairDate.Date].Add(catID);
                            }
                        }

                        if (pair.PassTypeID == Constants.overtimeUnjustified)
                        {
                            catID = (int)Constants.AnomalyCategories.OvertimeToJusitify;
                            if (categories.ContainsKey(catID))
                            {
                                if (!emplDateAnomalyCategories.ContainsKey(pair.EmployeeID))
                                    emplDateAnomalyCategories.Add(pair.EmployeeID, new Dictionary<DateTime, List<int>>());

                                if (!emplDateAnomalyCategories[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                                    emplDateAnomalyCategories[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<int>());

                                if (!emplDateAnomalyCategories[pair.EmployeeID][pair.IOPairDate.Date].Contains(catID))
                                    emplDateAnomalyCategories[pair.EmployeeID][pair.IOPairDate.Date].Add(catID);
                            }
                        }

                        if (emplNightOvertime.ContainsKey(pair.EmployeeID) && emplNightOvertime[pair.EmployeeID].ContainsKey(Constants.RuleCompanyInitialNightOvertime)
                            && pair.PassTypeID == emplNightOvertime[pair.EmployeeID][Constants.RuleCompanyInitialNightOvertime].RuleValue)
                        {
                            catID = (int)Constants.AnomalyCategories.NightOvertimeToJusitify;
                            if (categories.ContainsKey(catID))
                            {
                                if (!emplDateAnomalyCategories.ContainsKey(pair.EmployeeID))
                                    emplDateAnomalyCategories.Add(pair.EmployeeID, new Dictionary<DateTime, List<int>>());

                                if (!emplDateAnomalyCategories[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                                    emplDateAnomalyCategories[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<int>());

                                if (!emplDateAnomalyCategories[pair.EmployeeID][pair.IOPairDate.Date].Contains(catID))
                                    emplDateAnomalyCategories[pair.EmployeeID][pair.IOPairDate.Date].Add(catID);
                            }
                        }

                        if (pair.VerificationFlag == (int)Constants.Verification.NotVerified)
                        {
                            catID = (int)Constants.AnomalyCategories.NotVerified;
                            if (categories.ContainsKey(catID))
                            {
                                if (!emplDateAnomalyCategories.ContainsKey(pair.EmployeeID))
                                    emplDateAnomalyCategories.Add(pair.EmployeeID, new Dictionary<DateTime, List<int>>());

                                if (!emplDateAnomalyCategories[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                                    emplDateAnomalyCategories[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<int>());

                                if (!emplDateAnomalyCategories[pair.EmployeeID][pair.IOPairDate.Date].Contains(catID))
                                    emplDateAnomalyCategories[pair.EmployeeID][pair.IOPairDate.Date].Add(catID);
                            }
                        }

                        if (pair.ConfirmationFlag == (int)Constants.Confirmation.NotConfirmed)
                        {
                            catID = (int)Constants.AnomalyCategories.NotConfirmed;
                            if (categories.ContainsKey(catID))
                            {
                                if (!emplDateAnomalyCategories.ContainsKey(pair.EmployeeID))
                                    emplDateAnomalyCategories.Add(pair.EmployeeID, new Dictionary<DateTime, List<int>>());

                                if (!emplDateAnomalyCategories[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                                    emplDateAnomalyCategories[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<int>());

                                if (!emplDateAnomalyCategories[pair.EmployeeID][pair.IOPairDate.Date].Contains(catID))
                                    emplDateAnomalyCategories[pair.EmployeeID][pair.IOPairDate.Date].Add(catID);
                            }
                        }
                    }

                    if (categories.ContainsKey((int)Constants.AnomalyCategories.NegativeBuffers))
                    {
                        // get negative counters
                        List<EmployeeCounterValueTO> negativeCounters = new EmployeeCounterValue(Session[Constants.sessionConnection]).SearchNegative(emplIDs);

                        foreach (EmployeeCounterValueTO val in negativeCounters)
                        {
                            if (!emplDateAnomalyCategories.ContainsKey(val.EmplID))
                                emplDateAnomalyCategories.Add(val.EmplID, new Dictionary<DateTime, List<int>>());

                            if (!emplDateAnomalyCategories[val.EmplID].ContainsKey(new DateTime()))
                                emplDateAnomalyCategories[val.EmplID].Add(new DateTime(), new List<int>());

                            if (!emplDateAnomalyCategories[val.EmplID][new DateTime()].Contains((int)Constants.AnomalyCategories.NegativeBuffers))
                                emplDateAnomalyCategories[val.EmplID][new DateTime()].Add((int)Constants.AnomalyCategories.NegativeBuffers);
                        }

                        // get negative annual leave counters
                        Dictionary<int, Dictionary<int, int>> emplCounters = new EmployeeCounterValue(Session[Constants.sessionConnection]).Search(emplIDs);

                        foreach (int emplID in emplCounters.Keys)
                        {
                            if (emplCounters[emplID].ContainsKey((int)Constants.EmplCounterTypes.AnnualLeaveCounter) && emplCounters[emplID].ContainsKey((int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter)
                                && emplCounters[emplID].ContainsKey((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter) && (emplCounters[emplID][(int)Constants.EmplCounterTypes.AnnualLeaveCounter]
                                + emplCounters[emplID][(int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter] - emplCounters[emplID][(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter]) < 0)
                            {
                                if (!emplDateAnomalyCategories.ContainsKey(emplID))
                                    emplDateAnomalyCategories.Add(emplID, new Dictionary<DateTime, List<int>>());

                                if (!emplDateAnomalyCategories[emplID].ContainsKey(new DateTime()))
                                    emplDateAnomalyCategories[emplID].Add(new DateTime(), new List<int>());

                                if (!emplDateAnomalyCategories[emplID][new DateTime()].Contains((int)Constants.AnomalyCategories.NegativeBuffers))
                                    emplDateAnomalyCategories[emplID][new DateTime()].Add((int)Constants.AnomalyCategories.NegativeBuffers);
                            }
                        }
                    }

                    if (categories.ContainsKey((int)Constants.AnomalyCategories.SickLeave))
                    {
                        foreach (int emplID in refundationTypesDuration.Keys)
                        {
                            foreach (DateTime day in refundationTypesDuration[emplID].Keys)
                            {
                                foreach (int passTypeID in refundationTypesDuration[emplID][day].Keys)
                                {
                                    if (refundationTypesDuration[emplID][day][passTypeID] % (Constants.dayDurationStandardShift * 60) != 0)
                                    {
                                        if (!emplDateAnomalyCategories.ContainsKey(emplID))
                                            emplDateAnomalyCategories.Add(emplID, new Dictionary<DateTime, List<int>>());

                                        if (!emplDateAnomalyCategories[emplID].ContainsKey(day))
                                            emplDateAnomalyCategories[emplID].Add(day, new List<int>());

                                        if (!emplDateAnomalyCategories[emplID][day].Contains((int)Constants.AnomalyCategories.SickLeave))
                                            emplDateAnomalyCategories[emplID][day].Add((int)Constants.AnomalyCategories.SickLeave);
                                    }
                                }
                            }
                        }
                    }

                    if (categories.ContainsKey((int)Constants.AnomalyCategories.Canteen))
                    {
                        Dictionary<int, Dictionary<DateTime, List<OnlineMealsUsedTO>>> employeeMeals = new OnlineMealsUsed(Session[Constants.sessionConnection]).SearchMealsUsedDict(emplIDs, fromDate.Date, toDate.Date.AddDays(2));

                        // calculate meals approved and not approved by BG and KG
                        foreach (int emplID in employeeMeals.Keys)
                        {
                            DateTime nightWorkEnd = new DateTime();

                            if (emplNightWork.ContainsKey(emplID) && emplNightWork[emplID].ContainsKey(Constants.RuleNightWork))
                                nightWorkEnd = emplNightWork[emplID][Constants.RuleNightWork].RuleDateTime2;

                            foreach (DateTime mealDate in employeeMeals[emplID].Keys)
                            {
                                foreach (OnlineMealsUsedTO meal in employeeMeals[emplID][mealDate])
                                {
                                    bool nightShiftFound = false;
                                    if ((meal.EventTime.Date.Equals(fromDate.Date) || meal.EventTime.Date.Equals(toDate.Date.AddDays(1)))
                                        && meal.EventTime.TimeOfDay <= nightWorkEnd.TimeOfDay)
                                    {
                                        // check if there is night shift                                        
                                        List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();

                                        if (emplDayIntervals.ContainsKey(meal.EmployeeID) && emplDayIntervals[meal.EmployeeID].ContainsKey(meal.EventTime.Date))
                                            intervals = emplDayIntervals[meal.EmployeeID][meal.EventTime.Date];

                                        foreach (WorkTimeIntervalTO interval in intervals)
                                        {
                                            if (interval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0) && interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes > 0)
                                            {
                                                nightShiftFound = true;
                                                break;
                                            }
                                        }
                                    }

                                    // first day meals until night work end belongs to previous day if employee has night shift ending so skip them
                                    if (meal.EventTime.Date.Equals(fromDate.Date) && meal.EventTime.TimeOfDay <= nightWorkEnd.TimeOfDay && nightShiftFound)
                                        continue;

                                    // day after last day meals until night work end belongs to selected interval if employee has night shift ending, and after night work do not
                                    if (meal.EventTime.Date.Equals(toDate.Date.AddDays(1)) && (meal.EventTime.TimeOfDay > nightWorkEnd.TimeOfDay
                                        || (meal.EventTime.TimeOfDay <= nightWorkEnd.TimeOfDay && !nightShiftFound)))
                                        continue;

                                    if (meal.Approved.Trim().Equals("") && meal.AutoCheck == -1)
                                    {
                                        if (!emplDateAnomalyCategories.ContainsKey(emplID))
                                            emplDateAnomalyCategories.Add(emplID, new Dictionary<DateTime, List<int>>());

                                        if (!emplDateAnomalyCategories[emplID].ContainsKey(meal.EventTime.Date))
                                            emplDateAnomalyCategories[emplID].Add(meal.EventTime.Date, new List<int>());

                                        if (!emplDateAnomalyCategories[emplID][meal.EventTime.Date].Contains((int)Constants.AnomalyCategories.Canteen))
                                            emplDateAnomalyCategories[emplID][meal.EventTime.Date].Add((int)Constants.AnomalyCategories.Canteen);
                                    }                                    
                                }
                            }                            
                        }
                    }

                    if (categories.ContainsKey((int)Constants.AnomalyCategories.LaborLaw) || categories.ContainsKey((int)Constants.AnomalyCategories.LaborLawShift))
                    {
                        // validate each day of interval, there must be at least one time free day in each seven days, and 12h between each shift
                        foreach (int emplID in employees.Keys)
                        {
                            DateTime lastFreeDay = from.Date.AddDays(-1);
                            DateTime currDay = from.Date;
                            Dictionary<DateTime, List<WorkTimeIntervalTO>> emplIntervals = new Dictionary<DateTime, List<WorkTimeIntervalTO>>();
                            List<EmployeeTimeScheduleTO> emplScheduleList = new List<EmployeeTimeScheduleTO>();

                            if (emplSchedules.ContainsKey(emplID))
                                emplScheduleList = emplSchedules[emplID];

                            while (currDay.Date <= to.Date)
                            {
                                // get intervals for current and next day
                                List<WorkTimeIntervalTO> dayIntervals = new List<WorkTimeIntervalTO>();
                                List<WorkTimeIntervalTO> nextIntervals = new List<WorkTimeIntervalTO>();

                                if (emplIntervals.ContainsKey(currDay.Date))
                                    dayIntervals = emplIntervals[currDay.Date];
                                else
                                {
                                    dayIntervals = Common.Misc.getTimeSchemaInterval(currDay.Date, emplScheduleList, schemas);
                                    emplIntervals.Add(currDay.Date, dayIntervals);
                                }

                                if (currDay.Date < to.Date)
                                {
                                    if (emplIntervals.ContainsKey(currDay.Date.AddDays(1)))
                                        nextIntervals = emplIntervals[currDay.Date.AddDays(1)];
                                    else
                                    {
                                        nextIntervals = Common.Misc.getTimeSchemaInterval(currDay.Date.AddDays(1), emplScheduleList, schemas);
                                        emplIntervals.Add(currDay.Date.AddDays(1), nextIntervals);
                                    }
                                }

                                // calculate gaps between intervals, each must be at least 12h, remember last free day and check if there is more then 7 days between last two free days
                                // if number of intervals is 2, check gap between intervals                          
                                int gap = 0;
                                if (dayIntervals.Count == 0)
                                {
                                    gap = 24;
                                    lastFreeDay = currDay.Date;
                                }
                                else
                                {
                                    int intervalDuration = 0;
                                    for (int i = 0; i < dayIntervals.Count; i++)
                                    {
                                        intervalDuration += (int)dayIntervals[i].EndTime.TimeOfDay.Subtract(dayIntervals[i].StartTime.TimeOfDay).TotalMinutes;

                                        if (dayIntervals[i].EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                            intervalDuration++;

                                        gap = Common.Misc.getNextIntervalGap(i, dayIntervals, nextIntervals);
                                        
                                        if (gap < Constants.FiatShiftMinimalGap)
                                        {
                                            if (!emplDateAnomalyCategories.ContainsKey(emplID))
                                                emplDateAnomalyCategories.Add(emplID, new Dictionary<DateTime, List<int>>());

                                            if (!emplDateAnomalyCategories[emplID].ContainsKey(currDay.Date))
                                                emplDateAnomalyCategories[emplID].Add(currDay.Date, new List<int>());

                                            if (!emplDateAnomalyCategories[emplID][currDay.Date].Contains((int)Constants.AnomalyCategories.LaborLawShift))
                                                emplDateAnomalyCategories[emplID][currDay.Date].Add((int)Constants.AnomalyCategories.LaborLawShift);
                                        }
                                    }

                                    if (intervalDuration == 0)
                                        lastFreeDay = currDay.Date;
                                }

                                // check when was last free day
                                int firstSchemaFreeDay = Constants.FiatShiftMinimalFreeDayGap;
                                if ((int)currDay.Date.Subtract(lastFreeDay.Date).TotalDays >= Constants.FiatShiftMinimalFreeDayGap)
                                {
                                    // 08.07.2013. Sanja - check if schema first free day is greater then 7 and if is so, compare with that value
                                    // this change is for industrial schemas (I-I-II-II-III.-.III.-III.-X) - 8th day is free
                                    WorkTimeSchemaTO daySch = new WorkTimeSchemaTO();
                                    if (dayIntervals.Count > 0 && schemas.ContainsKey(dayIntervals[0].TimeSchemaID))
                                        daySch = schemas[dayIntervals[0].TimeSchemaID];

                                    int freeDay = 0;
                                    foreach (int day in daySch.Days.Keys)
                                    {
                                        if (daySch.Days[day].Count == 0 ||
                                            (daySch.Days[day].Count == 1 && daySch.Days[day][0].StartTime.TimeOfDay == new TimeSpan(0, 0, 0) && daySch.Days[day][0].EndTime.TimeOfDay == new TimeSpan(0, 0, 0)))
                                        {
                                            freeDay = day;
                                            break;
                                        }
                                    }

                                    if (freeDay >= Constants.FiatShiftMinimalFreeDayGap)
                                        firstSchemaFreeDay = freeDay + 1;
                                }

                                if ((int)currDay.Date.Subtract(lastFreeDay.Date).TotalDays >= firstSchemaFreeDay)
                                {
                                    if (!emplDateAnomalyCategories.ContainsKey(emplID))
                                        emplDateAnomalyCategories.Add(emplID, new Dictionary<DateTime, List<int>>());

                                    if (!emplDateAnomalyCategories[emplID].ContainsKey(currDay.Date))
                                        emplDateAnomalyCategories[emplID].Add(currDay.Date, new List<int>());

                                    if (!emplDateAnomalyCategories[emplID][currDay.Date].Contains((int)Constants.AnomalyCategories.LaborLaw))
                                        emplDateAnomalyCategories[emplID][currDay.Date].Add((int)Constants.AnomalyCategories.LaborLaw);
                                }

                                currDay = currDay.AddDays(1).Date;
                                continue;
                            }
                        }
                    }

                    if (categories.ContainsKey((int)Constants.AnomalyCategories.LaborLawShift))
                    {
                        int minimalGap = Constants.FiatShiftMinimalGap * 60; // 12 hours, gap is in minutes
                        IOPairProcessed pairProcessed = new IOPairProcessed(Session[Constants.sessionConnection]);
                        foreach (int emplID in employees.Keys)
                        {
                            if (emplDayPairs.ContainsKey(emplID))
                            {
                                // list of noncounting types
                                List<int> nonCountingTypes = new List<int>();
                                //nonCountingTypes.Add(Constants.overtimeUnjustified);
                                if (emplNightOvertime.ContainsKey(emplID) && emplNightOvertime[emplID].ContainsKey(Constants.RuleCompanyInitialNightOvertime))
                                    nonCountingTypes.Add(emplNightOvertime[emplID][Constants.RuleCompanyInitialNightOvertime].RuleValue);
                                if (emplOvertimeRejected.ContainsKey(emplID) && emplOvertimeRejected[emplID].ContainsKey(Constants.RuleCompanyOvertimeRejected))
                                    nonCountingTypes.Add(emplOvertimeRejected[emplID][Constants.RuleCompanyOvertimeRejected].RuleValue);

                                foreach (DateTime currDay in emplDayPairs[emplID].Keys)
                                {
                                    List<IOPairProcessedTO> dayPairs = emplDayPairs[emplID][currDay];
                                    List<IOPairProcessedTO> prevDayPairs = new List<IOPairProcessedTO>();
                                    if (emplDayPairs[emplID].ContainsKey(currDay.Date.AddDays(-1)))
                                        prevDayPairs = emplDayPairs[emplID][currDay.Date.AddDays(-1)];
                                    else
                                    {
                                        List<DateTime> prevDateList = new List<DateTime>();
                                        prevDateList.Add(currDay.Date.AddDays(-1));
                                        prevDayPairs = pairProcessed.SearchAllPairsForEmpl(emplID.ToString().Trim(), prevDateList, "");
                                    }
                                    List<IOPairProcessedTO> nextDayPairs = new List<IOPairProcessedTO>();
                                    if (emplDayPairs[emplID].ContainsKey(currDay.Date.AddDays(1)))
                                        nextDayPairs = emplDayPairs[emplID][currDay.Date.AddDays(1)];
                                    else
                                    {
                                        List<DateTime> nextDateList = new List<DateTime>();
                                        nextDateList.Add(currDay.Date.AddDays(1));
                                        nextDayPairs = pairProcessed.SearchAllPairsForEmpl(emplID.ToString().Trim(), nextDateList, "");
                                    }

                                    for (int i = 0; i < dayPairs.Count; i++)
                                    {
                                        IOPairProcessedTO pairTO = dayPairs[i];

                                        // check only for countable overtime pairs (regular pairs are checked through shifts)
                                        if (passTypesAll.ContainsKey(pairTO.PassTypeID) && passTypesAll[pairTO.PassTypeID].IsPass != Constants.overtimePassType)
                                            continue;

                                        if (nonCountingTypes.Contains(pairTO.PassTypeID))
                                            continue;

                                        // get gap in currrent day after pair end
                                        DateTime checkStart = pairTO.EndTime;
                                        int gap = 0;
                                        bool regularFound = false;
                                        for (int index = 0; index < dayPairs.Count; index++)
                                        {
                                            // skip pairs before checking pair, and checking pair
                                            if (dayPairs[index].StartTime <= pairTO.StartTime)
                                                continue;

                                            // skip pairs of noncounting types
                                            if (nonCountingTypes.Contains(dayPairs[index].PassTypeID))
                                                continue;

                                            gap = (int)dayPairs[index].StartTime.Subtract(checkStart).TotalMinutes;

                                            if (passTypesAll.ContainsKey(dayPairs[index].PassTypeID) && passTypesAll[dayPairs[index].PassTypeID].IsPass != Constants.overtimePassType)
                                            {
                                                regularFound = true;
                                                break;
                                            }

                                            if (gap >= minimalGap)
                                                break;

                                            checkStart = dayPairs[index].EndTime;
                                        }

                                        // if gap is less then minimal
                                        if (gap < minimalGap)
                                        {
                                            // if regular pair is not found in current day and gap is less then minimal try checking in next day
                                            bool checkStartGap = false;
                                            if (!regularFound)
                                            {
                                                // get gap until next day midnight
                                                if (checkStart.TimeOfDay != new TimeSpan(23, 59, 0))
                                                    gap = (int)pairTO.IOPairDate.Date.AddDays(1).Subtract(checkStart).TotalMinutes;
                                                else
                                                    gap = 0;

                                                if (gap < minimalGap)
                                                {
                                                    checkStart = pairTO.IOPairDate.Date.AddDays(1);
                                                    for (int index = 0; index < nextDayPairs.Count; index++)
                                                    {
                                                        // skip pairs of noncounting types
                                                        if (nonCountingTypes.Contains(nextDayPairs[index].PassTypeID))
                                                            continue;

                                                        gap += (int)nextDayPairs[index].StartTime.Subtract(checkStart).TotalMinutes;

                                                        if (passTypesAll.ContainsKey(nextDayPairs[index].PassTypeID) && passTypesAll[nextDayPairs[index].PassTypeID].IsPass != Constants.overtimePassType)
                                                        {
                                                            regularFound = true;
                                                            break;
                                                        }

                                                        if (gap >= minimalGap)
                                                            break;

                                                        checkStart = nextDayPairs[index].EndTime;
                                                        gap = 0;
                                                    }

                                                    if (gap < minimalGap)
                                                    {
                                                        if (!regularFound)
                                                        {
                                                            // get gap until next day midnight
                                                            if (checkStart.TimeOfDay != new TimeSpan(23, 59, 0))
                                                                gap = (int)pairTO.IOPairDate.Date.AddDays(2).Subtract(checkStart).TotalMinutes;
                                                            else
                                                                gap = 0;

                                                            if (gap < minimalGap)
                                                                checkStartGap = true;
                                                        }
                                                        else
                                                            checkStartGap = true;
                                                    }
                                                }
                                            }
                                            else
                                                checkStartGap = true;

                                            if (checkStartGap)
                                            {
                                                // get gap in currrent day before pair start
                                                DateTime checkEnd = pairTO.StartTime;
                                                gap = 0;
                                                regularFound = false;
                                                for (int index = dayPairs.Count - 1; index >= 0; index--)
                                                {
                                                    // skip pairs after checking pair, and checking pair
                                                    if (dayPairs[index].StartTime >= pairTO.StartTime)
                                                        continue;

                                                    // skip pairs of noncounting types
                                                    if (nonCountingTypes.Contains(dayPairs[index].PassTypeID))
                                                        continue;

                                                    gap = (int)checkEnd.Subtract(dayPairs[index].EndTime).TotalMinutes;

                                                    if (passTypesAll.ContainsKey(dayPairs[index].PassTypeID) && passTypesAll[dayPairs[index].PassTypeID].IsPass != Constants.overtimePassType)
                                                    {
                                                        regularFound = true;
                                                        break;
                                                    }

                                                    if (gap >= minimalGap)
                                                        break;

                                                    checkEnd = dayPairs[index].StartTime;
                                                }

                                                if (gap < minimalGap)
                                                {
                                                    // if regular pair is not found in current day and gap is less then minimal try checking in previous day                                    
                                                    if (!regularFound)
                                                    {
                                                        // get gap until current day midnight                                        
                                                        gap = (int)checkEnd.Subtract(pairTO.IOPairDate.Date).TotalMinutes;

                                                        if (gap < minimalGap)
                                                        {
                                                            checkEnd = pairTO.IOPairDate.Date;
                                                            for (int index = prevDayPairs.Count - 1; index >= 0; index--)
                                                            {
                                                                // skip pairs of noncounting types
                                                                if (nonCountingTypes.Contains(prevDayPairs[index].PassTypeID))
                                                                    continue;

                                                                gap += (int)checkEnd.Subtract(prevDayPairs[index].EndTime).TotalMinutes;

                                                                if (passTypesAll.ContainsKey(prevDayPairs[index].PassTypeID) && passTypesAll[prevDayPairs[index].PassTypeID].IsPass != Constants.overtimePassType)
                                                                {
                                                                    regularFound = true;
                                                                    break;
                                                                }

                                                                if (gap >= minimalGap)
                                                                    break;

                                                                checkEnd = prevDayPairs[index].StartTime;
                                                                gap = 0;
                                                            }

                                                            if (gap < minimalGap)
                                                            {
                                                                if (!regularFound)
                                                                {
                                                                    // get gap until previous day midnight                                                   
                                                                    gap = (int)checkEnd.Subtract(pairTO.IOPairDate.Date.AddDays(-1)).TotalMinutes;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (gap < minimalGap)
                                        {
                                            if (!emplDateAnomalyCategories.ContainsKey(emplID))
                                                emplDateAnomalyCategories.Add(emplID, new Dictionary<DateTime, List<int>>());

                                            if (!emplDateAnomalyCategories[emplID].ContainsKey(currDay.Date))
                                                emplDateAnomalyCategories[emplID].Add(currDay.Date, new List<int>());

                                            if (!emplDateAnomalyCategories[emplID][currDay.Date].Contains((int)Constants.AnomalyCategories.LaborLawShift))
                                                emplDateAnomalyCategories[emplID][currDay.Date].Add((int)Constants.AnomalyCategories.LaborLawShift);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    foreach (int emplID in emplDateAnomalyCategories.Keys)
                    {
                        foreach (DateTime anomalyDate in emplDateAnomalyCategories[emplID].Keys)
                        {
                            if (anomalyDate.Date < fromDate.Date || anomalyDate.Date > toDate.Date)
                                continue;

                            foreach (int catID in emplDateAnomalyCategories[emplID][anomalyDate])
                            {
                                if (!categories.ContainsKey(catID))
                                    continue;

                                // create result row
                                // stringone
                                List<object> resultRow = new List<object>();
                                if (costcenterList.ContainsKey(emplID))
                                    resultRow.Add(costcenterList[emplID].Trim());
                                else
                                    resultRow.Add("");
                                if (costcenterListDesc.ContainsKey(emplID))
                                    resultRow.Add(costcenterListDesc[emplID].Trim());
                                else
                                    resultRow.Add("");
                                if (workgroupList.ContainsKey(emplID))
                                    resultRow.Add(workgroupList[emplID].Trim());
                                else
                                    resultRow.Add("");
                                if (uteList.ContainsKey(emplID))
                                    resultRow.Add(uteList[emplID].Trim());
                                else
                                    resultRow.Add("");
                                if (branchList.ContainsKey(emplID))
                                    resultRow.Add(branchList[emplID].Trim());
                                else
                                    resultRow.Add("");

                                // employee name
                                if (employees.ContainsKey(emplID))
                                    resultRow.Add(employees[emplID].FirstAndLastName.Trim());
                                else
                                    resultRow.Add("");

                                resultRow.Add(emplID);
                                resultRow.Add(anomalyDate);

                                // anomaly category
                                if (categories.ContainsKey(catID))
                                    resultRow.Add(categories[catID].Trim());
                                else
                                    resultRow.Add("");

                                // employee type
                                if (employees.ContainsKey(emplID) && emplTypes.ContainsKey(employees[emplID].EmployeeTypeID))
                                    resultRow.Add(emplTypes[employees[emplID].EmployeeTypeID].Trim());
                                else
                                    resultRow.Add("");

                                resultTable.Add(resultRow);
                            }
                        }
                    }
                }
                else
                {                    
                    Dictionary<int, double> groupHours = new Dictionary<int, double>();
                    Dictionary<int, double> scheduleHours = new Dictionary<int, double>();
                    Dictionary<int, double> pairHours = new Dictionary<int, double>();

                    Dictionary<int, List<EmployeeGroupsTimeScheduleTO>> groupSchedules = new Dictionary<int, List<EmployeeGroupsTimeScheduleTO>>();

                    // get group schedules by employees by date
                    List<EmployeeGroupsTimeScheduleTO> schList = new EmployeeGroupsTimeSchedule(Session[Constants.sessionConnection]).SearchGroupsSchedules(emplGroupsIDs, fromDate.Date, toDate.AddDays(1).Date, null);

                    foreach (EmployeeGroupsTimeScheduleTO groupSch in schList)
                    {
                        if (!groupSchedules.ContainsKey(groupSch.EmployeeGroupID))
                            groupSchedules.Add(groupSch.EmployeeGroupID, new List<EmployeeGroupsTimeScheduleTO>());

                        groupSchedules[groupSch.EmployeeGroupID].Add(groupSch);
                    }

                    foreach (int emplID in employees.Keys)
                    {
                        DateTime currDate = fromDate.Date;
                        List<EmployeeTimeScheduleTO> emplScheduleList = new List<EmployeeTimeScheduleTO>();

                        if (emplSchedules.ContainsKey(emplID))
                            emplScheduleList = emplSchedules[emplID];

                        if (!scheduleHours.ContainsKey(emplID))
                            scheduleHours.Add(emplID, 0);

                        List<EmployeeGroupsTimeScheduleTO> groupScheduleList = new List<EmployeeGroupsTimeScheduleTO>();

                        if (groupSchedules.ContainsKey(employees[emplID].WorkingGroupID))
                            groupScheduleList = groupSchedules[employees[emplID].WorkingGroupID];

                        if (!groupHours.ContainsKey(emplID))
                            groupHours.Add(emplID, 0);

                        while (currDate <= toDate.AddDays(1))
                        {                            
                            List<WorkTimeIntervalTO> grpIntervals = Common.Misc.getTimeSchemaIntervalGroup(currDate.Date, groupScheduleList, schemas);

                            foreach (WorkTimeIntervalTO interval in grpIntervals)
                            {                                
                                if (currDate.Date.Equals(fromDate.Date) && Common.Misc.isThirdShiftEndInterval(interval))
                                    continue;

                                if (currDate.Date.Equals(toDate.AddDays(1).Date) && !Common.Misc.isThirdShiftEndInterval(interval))
                                    continue;

                                double intervalDuration = 0;
                                if (interval.EndTime.Hour == 23 && interval.EndTime.Minute == 59)
                                    intervalDuration = interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).Add(new TimeSpan(0, 1, 0)).TotalHours;
                                else
                                    intervalDuration = interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalHours;

                                groupHours[emplID] += intervalDuration;
                            }

                            List<WorkTimeIntervalTO> intervals = Common.Misc.getTimeSchemaInterval(currDate.Date, emplScheduleList, schemas);

                            foreach (WorkTimeIntervalTO interval in intervals)
                            {
                                if (currDate.Date.Equals(fromDate.Date) && Common.Misc.isThirdShiftEndInterval(interval))
                                    continue;

                                if (currDate.Date.Equals(toDate.AddDays(1).Date) && !Common.Misc.isThirdShiftEndInterval(interval))
                                    continue;

                                double intervalDuration = 0;
                                if (interval.EndTime.Hour == 23 && interval.EndTime.Minute == 59)
                                    intervalDuration = interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).Add(new TimeSpan(0, 1, 0)).TotalHours;
                                else
                                    intervalDuration = interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalHours;

                                scheduleHours[emplID] += intervalDuration;
                            }                            

                            if (!emplDayIntervals.ContainsKey(emplID))
                                emplDayIntervals.Add(emplID, new Dictionary<DateTime, List<WorkTimeIntervalTO>>());

                            if (!emplDayIntervals[emplID].ContainsKey(currDate.Date))
                                emplDayIntervals[emplID].Add(currDate.Date, intervals);                            

                            if (!emplDaySchemas.ContainsKey(emplID))
                                emplDaySchemas.Add(emplID, new Dictionary<DateTime, WorkTimeSchemaTO>());

                            WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                            if (intervals.Count > 0 && schemas.ContainsKey(intervals[0].TimeSchemaID))
                                sch = schemas[intervals[0].TimeSchemaID];

                            if (!emplDaySchemas[emplID].ContainsKey(currDate.Date))
                                emplDaySchemas[emplID].Add(currDate.Date, sch);
                            
                            currDate = currDate.AddDays(1).Date;
                        }
                    }

                    if (categories.ContainsKey((int)Constants.AnomalyCategories.PairsHours))
                    {
                        // get dictionary of all rules, key is company and value are rules by employee type id
                        Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> emplRules = new Common.Rule(Session[Constants.sessionConnection]).SearchWUEmplTypeDictionary();

                        Dictionary<int, Dictionary<string, RuleTO>> employeeRules = new Dictionary<int, Dictionary<string, RuleTO>>();

                        foreach (IOPairProcessedTO pair in pairs)
                        {
                            int holidayPT = -1;
                            int personalHolidayPT = -1;
                            int expatOutWorkOnHoliday = -1;

                            if (!employeeRules.ContainsKey(pair.EmployeeID))
                            {
                                // get rules for employeeID and its company
                                Dictionary<string, RuleTO> rules = new Dictionary<string, RuleTO>();

                                if (employees.ContainsKey(pair.EmployeeID))
                                {
                                    int emplCompany = Common.Misc.getRootWorkingUnit(employees[pair.EmployeeID].WorkingUnitID, WUnits);
                                    int emplTypeID = employees[pair.EmployeeID].EmployeeTypeID;

                                    if (emplCompany != -1 && emplTypeID != -1 && emplRules.ContainsKey(emplCompany) && emplRules[emplCompany].ContainsKey(emplTypeID))
                                        rules = emplRules[emplCompany][emplTypeID];
                                }

                                employeeRules.Add(pair.EmployeeID, rules);
                            }

                            if (employeeRules[pair.EmployeeID].ContainsKey(Constants.RuleHolidayPassType))
                                holidayPT = employeeRules[pair.EmployeeID][Constants.RuleHolidayPassType].RuleValue;

                            if (employeeRules[pair.EmployeeID].ContainsKey(Constants.RulePersonalHolidayPassType))
                                personalHolidayPT = employeeRules[pair.EmployeeID][Constants.RulePersonalHolidayPassType].RuleValue;

                            if (employeeRules[pair.EmployeeID].ContainsKey(Constants.RuleWorkOnHolidayPassType))
                                expatOutWorkOnHoliday = employeeRules[pair.EmployeeID][Constants.RuleWorkOnHolidayPassType].RuleValue;
                            
                            if (pair.PassTypeID == Constants.absence || pair.PassTypeID == holidayPT || pair.PassTypeID == personalHolidayPT || pair.PassTypeID == expatOutWorkOnHoliday
                                || (passTypesAll.ContainsKey(pair.PassTypeID) && (passTypesAll[pair.PassTypeID].IsPass == Constants.passOnReader 
                                || passTypesAll[pair.PassTypeID].IsPass == Constants.wholeDayAbsence)))
                            {
                                if (pair.IOPairDate.Date.Equals(toDate.AddDays(1).Date) || pair.IOPairDate.Date.Equals(fromDate.Date))
                                {
                                    List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                                    if (emplDayPairs.ContainsKey(pair.EmployeeID) && emplDayPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                                        dayPairs = emplDayPairs[pair.EmployeeID][pair.IOPairDate.Date];

                                    WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                                    if (emplDaySchemas.ContainsKey(pair.EmployeeID) && emplDaySchemas[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                                        sch = emplDaySchemas[pair.EmployeeID][pair.IOPairDate.Date];

                                    List<WorkTimeIntervalTO> dayIntervals = new List<WorkTimeIntervalTO>();
                                    if (emplDayIntervals.ContainsKey(pair.EmployeeID) && emplDayIntervals[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                                        dayIntervals = emplDayIntervals[pair.EmployeeID][pair.IOPairDate.Date];

                                    bool previousDayPair = CommonWeb.Misc.isPreviousDayPair(pair, passTypesAll, dayPairs, sch, dayIntervals);
                                    
                                    if (pair.IOPairDate.Date.Equals(fromDate.Date) && previousDayPair)
                                        continue;

                                    if (pair.IOPairDate.Date.Equals(toDate.AddDays(1).Date) && !previousDayPair)
                                        continue;
                                }

                                double pairDuration = 0;
                                if (pair.EndTime.Hour == 23 && pair.EndTime.Minute == 59)
                                    pairDuration = pair.EndTime.Subtract(pair.StartTime).Add(new TimeSpan(0, 1, 0)).TotalMinutes;
                                else
                                    pairDuration = pair.EndTime.Subtract(pair.StartTime).TotalMinutes;

                                if (!pairHours.ContainsKey(pair.EmployeeID))
                                    pairHours.Add(pair.EmployeeID, 0);

                                pairHours[pair.EmployeeID] += pairDuration;
                            }
                        }
                    }

                    // create result table
                    foreach (int emplID in employees.Keys)
                    {
                        double groupHrs = 0;
                        double schHrs = 0;
                        double pairHrs = 0;

                        if (groupHours.ContainsKey(emplID))
                            groupHrs = groupHours[emplID];
                        if (scheduleHours.ContainsKey(emplID))
                            schHrs = scheduleHours[emplID];
                        if (pairHours.ContainsKey(emplID))
                            pairHrs = pairHours[emplID] / 60;

                        if (chbHoursDiff.Checked)
                        {
                            if (categories.ContainsKey((int)Constants.AnomalyCategories.GroupHours) && categories.ContainsKey((int)Constants.AnomalyCategories.ScheduleHours)
                                && categories.ContainsKey((int)Constants.AnomalyCategories.PairsHours))
                            {
                                if (groupHrs == schHrs && groupHrs == pairHrs)
                                    continue;
                            }
                            else if (categories.ContainsKey((int)Constants.AnomalyCategories.GroupHours) && categories.ContainsKey((int)Constants.AnomalyCategories.ScheduleHours))
                            {
                                if (groupHrs == schHrs)
                                    continue;
                            }
                            else if (categories.ContainsKey((int)Constants.AnomalyCategories.GroupHours) && categories.ContainsKey((int)Constants.AnomalyCategories.PairsHours))
                            {
                                if (groupHrs == pairHrs)
                                    continue;
                            }
                            else if (categories.ContainsKey((int)Constants.AnomalyCategories.ScheduleHours) && categories.ContainsKey((int)Constants.AnomalyCategories.PairsHours))
                            {
                                if (schHrs == pairHrs)
                                    continue;
                            }
                        }

                        // create result row
                        // stringone
                        List<object> resultRow = new List<object>();
                        if (costcenterList.ContainsKey(emplID))
                            resultRow.Add(costcenterList[emplID].Trim());
                        else
                            resultRow.Add("");
                        if (costcenterListDesc.ContainsKey(emplID))
                            resultRow.Add(costcenterListDesc[emplID].Trim());
                        else
                            resultRow.Add("");
                        if (workgroupList.ContainsKey(emplID))
                            resultRow.Add(workgroupList[emplID].Trim());
                        else
                            resultRow.Add("");
                        if (uteList.ContainsKey(emplID))
                            resultRow.Add(uteList[emplID].Trim());
                        else
                            resultRow.Add("");
                        if (branchList.ContainsKey(emplID))
                            resultRow.Add(branchList[emplID].Trim());
                        else
                            resultRow.Add("");

                        // employee name                        
                        resultRow.Add(employees[emplID].FirstAndLastName.Trim());
                        resultRow.Add(emplID);

                        if (categories.ContainsKey((int)Constants.AnomalyCategories.GroupHours))
                            resultRow.Add(groupHrs);                            
                            
                        if (categories.ContainsKey((int)Constants.AnomalyCategories.ScheduleHours))
                            resultRow.Add(schHrs);                            
                            
                        if (categories.ContainsKey((int)Constants.AnomalyCategories.PairsHours))
                            resultRow.Add(pairHrs);                            
                            
                        // employee type
                        if (emplTypes.ContainsKey(employees[emplID].EmployeeTypeID))
                            resultRow.Add(emplTypes[employees[emplID].EmployeeTypeID].Trim());
                        else
                            resultRow.Add("");

                        resultTable.Add(resultRow);
                    }
                }

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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in HRSSCOutstandingDataPage.btnShow_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/HRSSCOutstandingDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnReport_Click(Object sender, EventArgs e)
        {
            try
            {
                SaveState();
                lblError.Text = "";
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(HRSSCOutstandingDataPage).Assembly);

                List<List<object>> resultTable = new List<List<object>>();
                List<DataColumn> resultColumns = new List<DataColumn>();

                if (Session[Constants.sessionDataTableList] != null && Session[Constants.sessionDataTableList] is List<List<object>>)
                    resultTable = (List<List<object>>)Session[Constants.sessionDataTableList];

                if (Session[Constants.sessionDataTableColumns] != null && Session[Constants.sessionDataTableColumns] is List<DataColumn>)
                    resultColumns = (List<DataColumn>)Session[Constants.sessionDataTableColumns];

                DataSet dataSet = new DataSet();
                DataTable tableCR = new DataTable("OutstandingData");

                tableCR.Columns.Add("costcenter", typeof(System.String));
                tableCR.Columns.Add("ccDesc", typeof(System.String));
                tableCR.Columns.Add("workgroup", typeof(System.String));
                tableCR.Columns.Add("ute", typeof(System.String));
                tableCR.Columns.Add("branch", typeof(System.String));
                tableCR.Columns.Add("emplname", typeof(System.String));
                tableCR.Columns.Add("emplid", typeof(System.Int32));
                tableCR.Columns.Add("empldate", typeof(System.DateTime));
                tableCR.Columns.Add("anomalycategory", typeof(System.String));
                tableCR.Columns.Add("empltype", typeof(System.String));
                tableCR.Columns.Add("groupHrs", typeof(System.String));
                tableCR.Columns.Add("schHrs", typeof(System.String));
                tableCR.Columns.Add("pairHrs", typeof(System.String));

                dataSet.Tables.Add(tableCR);
                if (resultTable.Count > 0)
                {
                    foreach (List<object> listObj in resultTable)
                    {
                        DataRow row = tableCR.NewRow();

                        if (resultColumns.Count == listObj.Count)
                        {
                            for (int i = 0; i < listObj.Count; i++)
                            {
                                if (listObj[i] != null)
                                {
                                    if (listObj[i] is double)
                                        row[resultColumns[i].ColumnName] = ((double)listObj[i]).ToString("F2").Trim();
                                    else
                                        row[resultColumns[i].ColumnName] = listObj[i].ToString().Trim();
                                }
                            }

                            //if (listObj[0] != null)
                            //    row["costcentre"] = listObj[0].ToString().Trim();
                            //if (listObj[1] != null)
                            //    row["workgroup"] = listObj[1].ToString().Trim();
                            //if (listObj[2] != null)
                            //    row["ute"] = listObj[2].ToString().Trim();
                            //if (listObj[3] != null)
                            //    row["branch"] = listObj[3].ToString().Trim();
                            //if (listObj[4] != DBNull.Value)
                            //    row["emplname"] = listObj[4].ToString().Trim();
                            //if (listObj[5] != null)
                            //    row["emplid"] = listObj[5].ToString().Trim();
                            //if (listObj[6] != null)
                            //    row["empldate"] = listObj[6].ToString().Trim();
                            //if (listObj[7] != null)
                            //    row["anomalycategory"] = listObj[7].ToString().Trim();
                            //if (listObj[8] != null)
                            //    row["empltype"] = listObj[8].ToString().Trim();

                            tableCR.Rows.Add(row);
                            tableCR.AcceptChanges();
                        }
                    }
                    if (tableCR.Rows.Count == 0)
                    {
                        lblError.Text = rm.GetString("noReportData", culture);
                        writeLog(DateTime.Now, false);
                        return;
                    }
                    string employee = "*";
                    string fromDate = "";
                    string toDate = "";
                    string anomalyCategory = "*";
                    if (lboxEmployees.SelectedIndex > 0)
                    {
                        for (int intEmpolyees = 0; intEmpolyees < lboxEmployees.Items.Count; intEmpolyees++)
                        {
                            if (lboxEmployees.Items[intEmpolyees].Selected)
                            {
                                employee = employee + ", " + lboxEmployees.Items[intEmpolyees].ToString(); ;

                            }
                        }
                        employee = employee.Substring(employee.IndexOf(',') + 1);
                    }
                    if (employee == "")
                    {
                        for (int intEmpolyees = 0; intEmpolyees < lboxEmployees.Items.Count; intEmpolyees++)
                        {
                            employee = employee + ", " + lboxEmployees.Items[intEmpolyees].ToString();
                        }
                        employee = employee.Substring(employee.IndexOf(',') + 1);
                    }

                    if (lbAnomalies.SelectedIndex > 0)
                    {
                        for (int intAnomalies = 0; intAnomalies < lbAnomalies.Items.Count; intAnomalies++)
                        {
                            if (lbAnomalies.Items[intAnomalies].Selected)
                            {
                                anomalyCategory = anomalyCategory + ", " + lbAnomalies.Items[intAnomalies].ToString(); ;

                            }
                        }
                        anomalyCategory = anomalyCategory.Substring(anomalyCategory.IndexOf(',') + 1);
                    }
                    if (anomalyCategory == "")
                    {
                        for (int intAnomalies = 0; intAnomalies < lbAnomalies.Items.Count; intAnomalies++)
                        {
                            anomalyCategory = anomalyCategory + ", " + lbAnomalies.Items[intAnomalies].ToString();
                        }
                        anomalyCategory = anomalyCategory.Substring(anomalyCategory.IndexOf(',') + 1);
                    }
                    fromDate = CommonWeb.Misc.createDate(tbFromDate.Text).ToString("dd.MM.yyyy.");
                    toDate = CommonWeb.Misc.createDate(tbToDate.Text).ToString("dd.MM.yyyy.");

                    Session["HRSSCOutstandingDataPage.dataSet"] = dataSet;
                    Session["HRSSCOutstandingDataPage.empl"] = employee;
                    Session["HRSSCOutstandingDataPage.anomalycategory"] = anomalyCategory;
                    Session["HRSSCOutstandingDataPage.fromDate"] = fromDate;
                    Session["HRSSCOutstandingDataPage.toDate"] = toDate;
                    SaveState();
                    string reportURL = "";
                    if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                        reportURL = "/ACTAWeb/ReportsWeb/sr/HRSSCOutstandingDataReport_sr.aspx";
                    else
                        reportURL = "/ACTAWeb/ReportsWeb/en/HRSSCOutstandingDataReport_en.aspx";
                    Response.Redirect("/ACTAWeb/ReportsWeb/ReportPage.aspx?Back=/ACTAWeb/ACTAWebUI/HRSSCOutstandingDataPage.aspx&Report=" + reportURL.Trim(), false);
                    // Session[Constants.sessionSamePage] = true;
                }
                else
                    lblError.Text = rm.GetString("noReportData", culture);

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in HRSSCOutstandingDataPage.btnReport_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/HRSSCOutstandingDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void rbStandard_ChackedChanged(Object sender, EventArgs e)
        {
            try
            {
                rbHoursDiff.Checked = !rbStandard.Checked;

                chbHoursDiff.Visible = !rbStandard.Checked;

                populateAnomalies();

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in HRSSCOutstandingDataPage.rbStandard_ChackedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/HRSSCOutstandingDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void rbHoursDiff_ChackedChanged(Object sender, EventArgs e)
        {
            try
            {
                rbStandard.Checked = !rbHoursDiff.Checked;

                chbHoursDiff.Visible = rbHoursDiff.Checked;

                populateAnomalies();

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in HRSSCOutstandingDataPage.rbHoursDiff_ChackedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/HRSSCOutstandingDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void cbShowRetired_CheckedChanged(Object sender, EventArgs e)
        {
            try
            {
                tbBorderDate.Enabled = btnBorderDate.Enabled = cbShowRetired.Checked;

                Date_Changed(this, new EventArgs());
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in HRSSCOutstandingDataPage.cbShowRetired_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/HRSSCOutstandingDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }        

        private void ClearSessionValues()
        {
            try
            {
                if (Session[Constants.sessionTables] != null)
                    Session[Constants.sessionTables] = null;
                if (Session[Constants.sessionDataTableList] != null)
                    Session[Constants.sessionDataTableList] = null;
                if (Session[Constants.sessionDataTableColumns] != null)
                    Session[Constants.sessionDataTableColumns] = null;
                if (Session[Constants.sessionFilter] != null)
                    Session[Constants.sessionFilter] = "";
                if (Session[Constants.sessionSortCol] != null)
                    Session[Constants.sessionSortCol] = "";
                if (Session[Constants.sessionSortDir] != null)
                    Session[Constants.sessionSortDir] = "";
                if (Session[Constants.sessionResultCurrentPage] != null)
                    Session[Constants.sessionResultCurrentPage] = null;

                Session[Constants.sessionItemsColors] = null;                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SaveState()
        {
            try
            {
                Dictionary<string, string> filterState = new Dictionary<string, string>();

                if (Session[Constants.sessionFilterState] != null && Session[Constants.sessionFilterState] is Dictionary<string, string>)
                    filterState = (Dictionary<string, string>)Session[Constants.sessionFilterState];

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "HRSSCOutstandingDataPage.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "HRSSCOutstandingDataPage.", filterState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private class AnomalyCategory
        {
            private string name = "";
            private int id = -1;

            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            public int ID
            {
                get { return id; }
                set { id = value; }
            }

            public AnomalyCategory(string name, int id)
            {
                this.Name = name;
                this.ID = id;
            }
        }
    }
}
