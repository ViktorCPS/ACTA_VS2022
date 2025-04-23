using System;
using System.Collections;
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
    public partial class TLClockDataPage : System.Web.UI.Page
    {
        const string pageName = "TLClockDataPage";

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
                    btnWUTree.Attributes.Add("onclick", "return wUnitsPicker('btnWUTree', 'false');");
                    btnOrgTree.Attributes.Add("onclick", "return oUnitsPicker('btnOrgTree', 'false');");
                    cbSelectGates.Attributes.Add("onclick", "return selectListItems('cbSelectGates', 'lbGates');");
                    cbSelectAllEmpolyees.Attributes.Add("onclick", "return selectListItems('cbSelectAllEmpolyees', 'lboxEmployees');");
                    tbEmployee.Attributes.Add("onKeyUp", "return selectItem('tbEmployee', 'lboxEmployees');");
                    btnShow.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnReport.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    rbHierarchically.Attributes.Add("onclick", "return checkRB('rbHierarchically', 'rbSelected', 'rbResponsiblePerson');");
                    rbSelected.Attributes.Add("onclick", "return checkRB('rbSelected', 'rbHierarchically', 'rbResponsiblePerson');");
                    rbResponsiblePerson.Attributes.Add("onclick", "return checkRB('rbResponsiblePerson', 'rbHierarchically', 'rbSelected');");

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

                    cbSelectAllEmpolyees.Visible = cbSelectGates.Visible = false;

                    rbHierarchically.Checked = true;
                    rbSelected.Checked = rbResponsiblePerson.Checked = false;

                    if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                        && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.WCManager)
                        rbHierarchically.Visible = rbSelected.Visible = rbResponsiblePerson.Visible = true;
                    else
                        rbHierarchically.Visible = rbSelected.Visible = rbResponsiblePerson.Visible = false;

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

                    populateGates();

                    InitializeSQLParameters();

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

                        // do again load state to select previously selected employees in employees list
                        LoadState();
                    }

                    //resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?showSelection=false";                    
                    btnShow_Click(this, new EventArgs());
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
                            btnShow_Click(this, new EventArgs());
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
                            btnShow_Click(this, new EventArgs());
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLClockDataPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLClockDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnPrevDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLClockDataPage).Assembly);

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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLClockDataPage.btnPrevDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLClockDataPage.aspx&Header=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnNextDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLClockDataPage).Assembly);

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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLClockDataPage.btnNextDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLClockDataPage.aspx&Header=" + Constants.trueValue.Trim(), false);
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLClockDataPage.Date_Changed(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLClockDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void populateEmployees(int ID, bool isWU)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLClockDataPage).Assembly);

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
                List<int> resPersonTypesVisible = new List<int>();

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
                {
                    typesVisible = companyVisibleTypes[company];

                    foreach (int type in typesVisible)
                    {
                        if (type != (int)Constants.EmployeeTypesFIAT.Expat && type != (int)Constants.EmployeeTypesFIAT.TaskForce && type != (int)Constants.EmployeeTypesFIAT.Agency)
                            resPersonTypesVisible.Add(type);
                    }
                }

                // 09.01.2012. Sanja - do not exclude login employee from reports
                //if (Session[Constants.sessionLogInEmployee] != null && Session[Constants.sessionLogInEmployee] is EmployeeTO)
                //    emplID = ((EmployeeTO)Session[Constants.sessionLogInEmployee]).EmployeeID;

                if (ID != -1)
                {
                    if (isWU)
                    {
                        if (Session[Constants.sessionLoginCategoryWUnits] != null && Session[Constants.sessionLoginCategoryWUnits] is List<int>)
                        {
                            if (rbHierarchically.Checked)
                                empolyeeList = new Employee(Session[Constants.sessionConnection]).SearchByWULoans(Common.Misc.getWorkingUnitHierarhicly(ID, (List<int>)Session[Constants.sessionLoginCategoryWUnits], Session[Constants.sessionConnection]), emplID, typesVisible, from, to);
                            else if (rbSelected.Checked)
                            {
                                if (((List<int>)Session[Constants.sessionLoginCategoryWUnits]).Contains(ID))
                                    empolyeeList = new Employee(Session[Constants.sessionConnection]).SearchByWULoans(ID.ToString().Trim(), emplID, typesVisible, from, to);
                            }
                            else if (rbResponsiblePerson.Checked)
                            {
                                List<WorkingUnitTO> wuChildren = new WorkingUnit(Session[Constants.sessionConnection]).SearchChildWU(ID.ToString().Trim());

                                string wuString = "";

                                foreach (WorkingUnitTO wu in wuChildren)
                                {
                                    wuString += wu.WorkingUnitID.ToString().Trim() + ",";
                                }

                                if (wuString.Length > 0)
                                    wuString = wuString.Substring(0, wuString.Length - 1);

                                if (wuString.Length > 0)
                                    empolyeeList = new Employee(Session[Constants.sessionConnection]).SearchEmployeesWUResponsible(wuString, resPersonTypesVisible, from, to);
                            }
                        }
                    }
                    else if (!isWU)
                    {
                        if (Session[Constants.sessionLoginCategoryOUnits] != null && Session[Constants.sessionLoginCategoryOUnits] is List<int>)
                        {
                            if (rbHierarchically.Checked)
                                empolyeeList = new Employee(Session[Constants.sessionConnection]).SearchByOU(Common.Misc.getOrgUnitHierarhicly(ID.ToString(), (List<int>)Session[Constants.sessionLoginCategoryOUnits], Session[Constants.sessionConnection]), emplID, typesVisible, from, to);
                            else if (rbSelected.Checked)
                            {
                                if (((List<int>)Session[Constants.sessionLoginCategoryOUnits]).Contains(ID))
                                    empolyeeList = new Employee(Session[Constants.sessionConnection]).SearchByOU(ID.ToString().Trim(), emplID, typesVisible, from, to);
                            }
                            else if (rbResponsiblePerson.Checked)
                            {
                                List<OrganizationalUnitTO> ouChildren = new OrganizationalUnit(Session[Constants.sessionConnection]).SearchChildOU(ID.ToString().Trim());

                                string ouString = "";

                                foreach (OrganizationalUnitTO ou in ouChildren)
                                {
                                    ouString += ou.OrgUnitID.ToString().Trim() + ",";
                                }

                                if (ouString.Length > 0)
                                    ouString = ouString.Substring(0, ouString.Length - 1);

                                if (ouString.Length > 0)
                                    empolyeeList = new Employee(Session[Constants.sessionConnection]).SearchEmployeesOUResponsible(ouString, resPersonTypesVisible, from, to);
                            }
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

        private void populateGates()
        {
            try
            {
                List<GateTO> gateList = new Gate(Session[Constants.sessionConnection]).Search();

                lbGates.DataSource = gateList;
                lbGates.DataTextField = "Name";
                lbGates.DataValueField = "GateID";
                lbGates.DataBind();
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLClockDataPage).Assembly);

                lblEmployee.Text = rm.GetString("lblEmployeeSelection", culture);
                lblPeriod.Text = rm.GetString("lblPeriod", culture);
                lblGate.Text = rm.GetString("lblGate", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);

                cbSelectAllEmpolyees.Text = rm.GetString("lblSelectAll", culture);
                cbSelectGates.Text = rm.GetString("lblSelectAll", culture);

                rbHierarchically.Text = rm.GetString("rbHierarchically", culture);
                rbSelected.Text = rm.GetString("rbSelected", culture);
                rbResponsiblePerson.Text = rm.GetString("rbResponsiblePerson", culture);

                btnShow.Text = rm.GetString("btnShow", culture);
                btnReport.Text = rm.GetString("btnReport", culture);
                btnMap.Text = rm.GetString("btnMap", culture);

                chbSinglePage.Text = rm.GetString("lblSingleEmployeePerPage", culture);
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLClockDataPage).Assembly);

                Common.Rule rule = new Common.Rule();
                rule.RuleTO.EmployeeTypeID = (int)Constants.EmployeeTypesFIAT.BC;
                rule.RuleTO.WorkingUnitID = Constants.defaultWorkingUnitID;
                rule.RuleTO.RuleType = Constants.RuleWrittingDataToTag;
                List<RuleTO> rules = rule.Search();

                if (rules.Count > 0 && rules[0].RuleValue == Constants.yesInt)
                {
                    btnMap.Visible = true;
                    Session[Constants.sessionHeader] = rm.GetString("hdrEmployeeID", culture) + "," + rm.GetString("hdrEmployeeName", culture) + "," + rm.GetString("hdrEmpolyeeLastName", culture) + "," + rm.GetString("hdrCostCenterCode", culture) + "," + rm.GetString("hdrCostCenterName", culture) + "," + rm.GetString("hdrWorkgroup", culture) + "," + rm.GetString("hdrUte", culture) + ","
                     + rm.GetString("hdrBranch", culture) + "," + rm.GetString("hdrEmplType", culture) + "," + rm.GetString("hdrWeek", culture) + "," + rm.GetString("hdrYear", culture) + "," + rm.GetString("hdrMonth", culture) + "," + rm.GetString("hdrDay", culture) + "," + rm.GetString("hdrTime", culture) + "," + rm.GetString("hdrDirection", culture)
                     + "," + rm.GetString("hdrLocation", culture) + "," + rm.GetString("hdrGate", culture) + "," + rm.GetString("hdrProcessed", culture) + "," + rm.GetString("hdrPassID", culture);
                    
                    Session[Constants.sessionFields] = "e.employee_id AS id| e.first_name AS first_name| e.last_name AS last_name| wu.name AS cost_centre| wu.description AS cost_centre_desc| SUBSTRING(ea.nvarchar_value_2,10,2) AS workgroup| SUBSTRING(ea.nvarchar_value_2,13,2) AS ute| ea.nvarchar_value_6 AS branch| et.employee_type_name AS empl_type| datepart(week,ps.event_time) AS date_week| datepart(year,ps.event_time) AS date_year| datepart(month,ps.event_time) AS date_month|datepart(day,ps.event_time) AS date_day|ps.event_time AS date_time| ps.direction AS direction| loc.name AS loc_name| g.name AS g_name| ps.pair_gen_used AS processed| ps.pass_id AS pass_id";
                    Session[Constants.sessionKey] = "pass_id";
                }
                else
                {
                    btnMap.Visible = false;
                    Session[Constants.sessionHeader] = rm.GetString("hdrEmployeeID", culture) + "," + rm.GetString("hdrEmployeeName", culture) + "," + rm.GetString("hdrEmpolyeeLastName", culture) + "," + rm.GetString("hdrCostCenterCode", culture) + "," + rm.GetString("hdrCostCenterName", culture) + "," + rm.GetString("hdrWorkgroup", culture) + "," + rm.GetString("hdrUte", culture) + ","
                    + rm.GetString("hdrBranch", culture) + "," + rm.GetString("hdrEmplType", culture) + "," + rm.GetString("hdrWeek", culture) + "," + rm.GetString("hdrYear", culture) + "," + rm.GetString("hdrMonth", culture) + "," + rm.GetString("hdrDay", culture) + "," + rm.GetString("hdrTime", culture) + "," + rm.GetString("hdrDirection", culture)
                    + "," + rm.GetString("hdrLocation", culture) + "," + rm.GetString("hdrGate", culture) + "," + rm.GetString("hdrProcessed", culture);

                    Session[Constants.sessionFields] = "e.employee_id AS id| e.first_name AS first_name| e.last_name AS last_name| wu.name AS cost_centre| wu.description AS cost_centre_desc| SUBSTRING(ea.nvarchar_value_2,10,2) AS workgroup| SUBSTRING(ea.nvarchar_value_2,13,2) AS ute| ea.nvarchar_value_6 AS branch| et.employee_type_name AS empl_type| datepart(week,ps.event_time) AS date_week| datepart(year,ps.event_time) AS date_year| datepart(month,ps.event_time) AS date_month|datepart(day,ps.event_time) AS date_day|ps.event_time AS date_time| ps.direction AS direction| loc.name AS loc_name| g.name AS g_name| ps.pair_gen_used AS processed";
                    Session[Constants.sessionKey] = "id";
                }

 
                Dictionary<int, int> formating = new Dictionary<int, int>();
                formating.Add(13, (int)Constants.FormatTypes.TimeFormat);
                Session[Constants.sessionFieldsFormating] = formating;
                Dictionary<int, Dictionary<string, string>> values = new Dictionary<int, Dictionary<string, string>>();
                Dictionary<string, string> formatValues = new Dictionary<string, string>();
                formatValues.Add("0", rm.GetString("no", culture));
                formatValues.Add("1", rm.GetString("yes", culture));
                values.Add(17, formatValues);
                Session[Constants.sessionFieldsFormatedValues] = values;
                Session[Constants.sessionTables] = "passes ps, locations loc, gates g, employees e, employees_asco4 ea,employee_types et,working_units wu";
                Session[Constants.sessionDataTableList] = null;
                Session[Constants.sessionDataTableColumns] = null;
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

                btnShow_Click(this, new EventArgs());

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLClockDataPage.Menu1_MenuItemClick(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLClockDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLClockDataPage).Assembly);

                ClearSessionValues();
                CultureInfo ci = CultureInfo.InvariantCulture;
                string filter = "";
                lblError.Text = "";
                SelBox.Value = "";

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
                    filter += "ps.event_time >= '" + from.ToString(CommonWeb.Misc.getDateTimeFormatUniversal(), ci) + "' AND ";
                else if (Session[Constants.sessionDataProvider].ToString().Trim().ToLower().Equals(Constants.dpMySql.Trim().ToLower()))
                    filter += "DATE_FORMAT(ps.event_time,'%Y-%m-%dT%h:%m:%s') >= '" + from.ToString(CommonWeb.Misc.getDateTimeFormatUniversal(), ci).Trim() + "' AND ";

                if (Session[Constants.sessionDataProvider].ToString().Trim().ToLower().Equals(Constants.dpSqlServer.Trim().ToLower()))
                    filter += "ps.event_time <= '" + to.ToString(CommonWeb.Misc.getDateTimeFormatUniversal(), ci) + "' AND ";
                else if (Session[Constants.sessionDataProvider].ToString().Trim().ToLower().Equals(Constants.dpMySql.Trim().ToLower()))
                    filter += "DATE_FORMAT(ps.event_time,'%Y-%m-%dT%h:%m:%s') <= '" + to.ToString(CommonWeb.Misc.getDateTimeFormatUniversal(), ci).Trim() + "' AND ";

                //filter for gates
                int[] selGateIndexes = lbGates.GetSelectedIndices();
                if (selGateIndexes.Length > 0)
                {
                    string gates = "";
                    foreach (int index in selGateIndexes)
                    {
                        gates += lbGates.Items[index].Value.Trim() + ",";
                    }

                    if (gates.Length > 0)
                        gates = gates.Substring(0, gates.Length - 1);

                    filter += "ps.gate_id IN (" + gates.Trim() + ") AND ";
                }

                //filter for empolyees
                int[] selEmployeeIndexes = lboxEmployees.GetSelectedIndices();

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
                filter += "ps.employee_id IN (" + emplIDs + ") AND  et.working_unit_id=" + company + " AND ";

                filter += "loc.location_id = ps.location_id AND ps.gate_id = g.gate_id AND ps.employee_id=e.employee_id AND e.employee_id=ea.employee_id AND e.employee_type_id=et.employee_type_id AND wu.working_unit_id = (select working_unit_id from working_units where working_unit_id =(select parent_working_unit_id as id from working_units where working_unit_id = (select parent_working_unit_id from working_units where working_unit_id = e.working_unit_id)))";

                Session[Constants.sessionFilter] = filter;
                Session[Constants.sessionSortCol] = "ps.event_time";
                Session[Constants.sessionSortDir] = Constants.sortASC;

                // save selected filter state
                SaveState();

                Common.Rule rule = new Common.Rule();
                rule.RuleTO.EmployeeTypeID = (int)Constants.EmployeeTypesFIAT.BC;
                rule.RuleTO.WorkingUnitID = Constants.defaultWorkingUnitID;
                rule.RuleTO.RuleType = Constants.RuleWrittingDataToTag;
                List<RuleTO> rules = rule.Search();

                if (rules.Count > 0 && rules[0].RuleValue == Constants.yesInt)
                {
                    resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?showSelection=true";
                }
                else
                {
                    resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?showSelection=false";
                }

                
                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLClockDataPage.btnShow_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLClockDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnReport_Click(Object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLClockDataPage).Assembly);

                lblError.Text = "";
                int rowCount = 0;
                DataTable passes = null;

                if (Session[Constants.sessionFields] != null && !Session[Constants.sessionFields].ToString().Trim().Equals("")
                    && Session[Constants.sessionTables] != null && !Session[Constants.sessionTables].ToString().Trim().Equals("")
                    && Session[Constants.sessionFilter] != null && !Session[Constants.sessionFilter].ToString().Trim().Equals("")
                    && Session[Constants.sessionSortCol] != null && !Session[Constants.sessionSortCol].ToString().Trim().Equals("")
                    && Session[Constants.sessionSortDir] != null && !Session[Constants.sessionSortDir].ToString().Trim().Equals(""))
                {
                    Result result = new Result(Session[Constants.sessionConnection]);
                    rowCount = result.SearchResultCount(Session[Constants.sessionTables].ToString().Trim(), Session[Constants.sessionFilter].ToString().Trim());

                    if (rowCount > 0)
                    {
                        // get all passes for search criteria for report
                        passes = new Result(Session[Constants.sessionConnection]).SearchResultTable(Session[Constants.sessionFields].ToString().Trim(), Session[Constants.sessionTables].ToString().Trim(),
                            Session[Constants.sessionFilter].ToString().Trim(), Session[Constants.sessionSortCol].ToString().Trim(), Session[Constants.sessionSortDir].ToString().Trim(), 1, rowCount);

                        // Table Definition for  Reports
                        DataSet dataSetCR = new DataSet();
                        DataTable tableCR = new DataTable("employee_passes");


                        tableCR.Columns.Add("date_week", typeof(System.String));
                        tableCR.Columns.Add("date_year", typeof(System.String));
                        tableCR.Columns.Add("date_month", typeof(System.String));
                        tableCR.Columns.Add("date_day", typeof(System.String));
                        tableCR.Columns.Add("date_time", typeof(System.String));
                        tableCR.Columns.Add("first_name", typeof(System.String));
                        tableCR.Columns.Add("last_name", typeof(System.String));
                        tableCR.Columns.Add("cost_centre", typeof(System.String));
                        tableCR.Columns.Add("cost_centre_desc", typeof(System.String));
                        tableCR.Columns.Add("workgroup", typeof(System.String));
                        tableCR.Columns.Add("ute", typeof(System.String));
                        tableCR.Columns.Add("branch", typeof(System.String));
                        tableCR.Columns.Add("empl_type", typeof(System.String));
                        tableCR.Columns.Add("pass_id", typeof(int));
                        tableCR.Columns.Add("direction", typeof(System.String));
                        tableCR.Columns.Add("pass_type", typeof(System.String));
                        tableCR.Columns.Add("is_wrk_hrs", typeof(System.String));
                        tableCR.Columns.Add("location", typeof(System.String));
                        tableCR.Columns.Add("pair_gen_used", typeof(System.String));
                        tableCR.Columns.Add("manual_created", typeof(System.String));
                        tableCR.Columns.Add("gate_id", typeof(System.String));

                        dataSetCR.Tables.Add(tableCR);


                        // populate dataset
                        foreach (DataRow pass in passes.Rows)
                        {
                            DataRow row = tableCR.NewRow();

                            if (pass["id"] != DBNull.Value)
                                row["pass_id"] = pass["id"].ToString().Trim();
                            if (pass["first_name"] != DBNull.Value)
                                row["first_name"] = pass["first_name"].ToString().Trim();
                            if (pass["last_name"] != DBNull.Value)
                                row["last_name"] = pass["last_name"].ToString().Trim();
                            if (pass["cost_centre"] != DBNull.Value)
                                row["cost_centre"] = pass["cost_centre"].ToString().Trim();
                            if (pass["cost_centre_desc"] != DBNull.Value)
                                row["cost_centre_desc"] = pass["cost_centre_desc"].ToString().Trim();
                            if (pass["direction"] != DBNull.Value)
                                row["direction"] = pass["direction"].ToString().Trim();
                            if (pass["date_week"] != DBNull.Value)
                                row["date_week"] = pass["date_week"].ToString().Trim();
                            if (pass["date_year"] != DBNull.Value)
                                row["date_year"] = pass["date_year"].ToString().Trim();
                            if (pass["date_month"] != DBNull.Value)
                                row["date_month"] = pass["date_month"].ToString().Trim();
                            if (pass["date_day"] != DBNull.Value)
                                row["date_day"] = pass["date_day"].ToString().Trim();
                            if (pass["workgroup"] != DBNull.Value)
                                row["workgroup"] = pass["workgroup"].ToString().Trim();
                            if (pass["ute"] != DBNull.Value)
                                row["ute"] = pass["ute"].ToString().Trim();
                            if (pass["date_time"] != DBNull.Value)
                            {
                                DateTime passDate = new DateTime();
                                if (DateTime.TryParse(pass["date_time"].ToString().Trim(), out passDate))
                                    row["date_time"] = passDate.ToString(Constants.timeFormat);
                                else
                                    row["date_time"] = pass["date_time"].ToString().Trim();
                            }
                            if (pass["loc_name"] != DBNull.Value)
                                row["location"] = pass["loc_name"].ToString().Trim();
                            if (pass["g_name"] != DBNull.Value)
                                row["gate_id"] = pass["g_name"].ToString().Trim();
                            if (pass["branch"] != DBNull.Value)
                                row["branch"] = pass["branch"].ToString().Trim();
                            if (pass["empl_type"] != DBNull.Value)
                                row["empl_type"] = pass["empl_type"].ToString().Trim();
                            if (pass["processed"] != DBNull.Value)
                            {
                                if (pass["processed"].ToString().Trim().Equals(((int)Constants.PairGenUsed.Used).ToString()))
                                {
                                    row["pair_gen_used"] = rm.GetString("yes", culture);
                                }
                                else
                                {
                                    row["pair_gen_used"] = rm.GetString("no", culture);
                                }
                            }
                            else
                                row["pair_gen_used"] = "";

                            tableCR.Rows.Add(row);
                            tableCR.AcceptChanges();
                        }

                        if (tableCR.Rows.Count == 0)
                        {
                            lblError.Text = rm.GetString("noReportData", culture);
                            writeLog(DateTime.Now, false);
                            return;
                        }

                        string gate = "*";
                        string start_time = "*";
                        string end_time = "*";
                        string empolyee = "*";

                        //gate parameter for report
                        if (lbGates.SelectedIndex > 0)
                        {
                            for (int intGates = 0; intGates < lbGates.Items.Count; intGates++)
                            {
                                if (lbGates.Items[intGates].Selected)
                                {
                                    gate = gate + ", " + lbGates.Items[intGates].ToString();
                                }
                            }
                            gate = gate.Substring(gate.IndexOf(',') + 1);
                        }
                        if (gate == "")
                        {
                            for (int intGates = 0; intGates < lbGates.Items.Count; intGates++)
                            {
                                gate = gate + ", " + lbGates.Items[intGates].ToString();
                            }
                            gate = gate.Substring(gate.IndexOf(',') + 1);
                        }
                        //employee parameter for report
                        if (lboxEmployees.SelectedIndex > 0)
                        {
                            for (int intEmpolyees = 0; intEmpolyees < lboxEmployees.Items.Count; intEmpolyees++)
                            {
                                if (lboxEmployees.Items[intEmpolyees].Selected)
                                {
                                    empolyee = empolyee + ", " + lboxEmployees.Items[intEmpolyees].ToString(); ;

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
                            start_time = tbFromDate.Text;
                        //toDate parameter for report
                        if (tbToDate.Text != "")
                            end_time = tbToDate.Text;

                        //send to session parameters for report
                        Session["TLClockDataPage.gate"] = gate;
                        Session["TLClockDataPage.employee"] = empolyee;
                        Session["TLClockDataPage.start_time"] = start_time;
                        Session["TLClockDataPage.end_time"] = end_time;                                                
                        Session["TLClockDataPage.passesDS"] = dataSetCR;

                        string singlePage = "";
                        if (chbSinglePage.Checked)
                            singlePage = "1";
                        else
                            singlePage = "0";

                        Session["TLClockDataPage.singlePage"] = singlePage;

                        Session[Constants.sessionReportName] = rm.GetString("lblPassesReport", culture);

                        //check language and redirect to apropriate report
                        string reportURL = "";
                        if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                            reportURL = "/ACTAWeb/ReportsWeb/sr/TLClockDataReport_sr.aspx";
                        else
                            reportURL = "/ACTAWeb/ReportsWeb/en/TLClockDataReport_en.aspx";
                        Response.Redirect("/ACTAWeb/ReportsWeb/ReportPage.aspx?Back=/ACTAWeb/ACTAWebUI/TLClockDataPage.aspx&Report=" + reportURL.Trim(), false);
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLClockDataPage.btnReport_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLClockDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnMap_Click(Object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLClockDataPage).Assembly);

                lblError.Text = "";

                string passID = SelBox.Value.ToString();
                if(passID.Equals("") || passID.Equals(null))
                {
                   lblError.Text = rm.GetString("noPassID", culture);
                }
                else
                {
                    passID = passID.Substring(0,passID.Length - 1);
                    int noOfPasses = 0;
                    if (passID.Contains('|'))
                    {
                        string[] passes = passID.Split('|');
                        noOfPasses = passes.Count();
                    }
                    else
                    {
                        noOfPasses = 1;
                    }
                    if (noOfPasses>50)
                    {
                        lblError.Text = rm.GetString("noMorePassID", culture);
                    }
                    else
                    {
                        string wOptions = "dialogWidth:1010px; dialogHeight:600px; edge:Raised; center:Yes; resizable:Yes; status:Yes;";
                        ClientScript.RegisterStartupScript(GetType(), "mapPopup", "window.open('/ACTAWeb/ACTAWebUI/PassesOnMapPage.aspx?passID=" + passID.Trim() + "', window, '" + wOptions.Trim() + "');", true);
                        SelBox.Value = "";
                    }
                }

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLClockDataPage.btnMap_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLClockDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private List<string> getSelection()
        {
            try
            {
                string[] selectedList = SelBox.Value.Trim().Split(Constants.delimiter);
                List<string> selKeys = new List<string>();
                foreach (string key in selectedList)
                {
                    if (!key.Equals(""))
                        selKeys.Add(key);
                }

                return selKeys;
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

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "TLClockDataPage.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "TLClockDataPage.", filterState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
