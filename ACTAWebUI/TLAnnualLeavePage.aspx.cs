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
using CommonWeb.Pickers;
using Common;
using TransferObjects;
using Util;
using ReportsWeb;

namespace ACTAWebUI
{
    public partial class TLAnnualLeavePage : System.Web.UI.Page
    {
        const string sessionPairs = "TLAnnualLeavePage.PairsListByEmployee";
        const string sessionOldPairs = "TLAnnualLeavePage.OldPairsListByEmployee";
        const string sessionCounters = "TLAnnualLeavePage.CounterListByEmployee";

        const string pageName = "TLAnnualLeavePage";

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
                Dictionary<int, WorkingUnitTO> wUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                // if user do not want to reload selected filter and get results for that filter, parameter reloadState should be passes through url and set to false
                if (!IsPostBack)
                {
                    ClearPageSessionValues();

                    btnFromDate.Attributes.Add("onclick", "return calendarPicker('tbFromDate', 'true');");
                    btnToDate.Attributes.Add("onclick", "return calendarPicker('tbToDate', 'true');");
                    btnWUTree.Attributes.Add("onclick", "return wUnitsPicker('btnWUTree', 'false');");
                    btnOrgTree.Attributes.Add("onclick", "return oUnitsPicker('btnOrgTree', 'false');");
                    cbSelectAllEmpolyees.Attributes.Add("onclick", "return selectListItems('cbSelectAllEmpolyees', 'lboxEmployees');");
                    tbEmployee.Attributes.Add("onKeyUp", "return selectItem('tbEmployee', 'lboxEmployees');");
                    rbYes.Attributes.Add("onclick", "return check('rbYes', 'rbNo');");
                    rbNo.Attributes.Add("onclick", "return check('rbNo', 'rbYes');");
                    btnShow.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnSave.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnPrint.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");

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

                    cbSelectAllEmpolyees.Visible = false;

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

                    populateEmplTypes();

                    Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                    RuleTO ruleTo = new RuleTO();

                    ruleTo.RuleType = Constants.RulePrintAnnualLeaveReport;
                    rule.RuleTO = ruleTo;
                    List<RuleTO> rules = rule.Search();
                    if (rules.Count > 0 && rules[0].RuleValue == 1)
                    {
                        btnPrint.Visible = true;
                        chbReport.Visible = true;
                    }
                    else
                    {
                      //  btnPrint.Visible = false;
                       // chbReport.Checked = false;
                       // chbReport.Visible = false;
                    }

                    InitializeSQLParameters();

                    rbNo.Checked = true;
                    rbNo.Visible = rbYes.Visible = lblOverwritePairs.Visible = false;

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
                            populateEmplTypes();
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
                            populateEmplTypes();
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLAnnualLeavePage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnPrevDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLAnnualLeavePage).Assembly);

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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLAnnualLeavePage.btnPrevDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLAnnualLeavePage.aspx&Header=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnNextDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLAnnualLeavePage).Assembly);

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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLAnnualLeavePage.btnNextDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLAnnualLeavePage.aspx&Header=" + Constants.trueValue.Trim(), false);
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLAnnualLeavePage.Date_Changed(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLAnnualLeavePage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void cbEmplType_SelectedIndexChanged(object sender, EventArgs e)
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLAnnualLeavePage.cbEmplType_SelectedIndexChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLAnnualLeavePage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void populateEmplTypes()
        {
            try
            {
                Dictionary<int, List<int>> companyVisibleTypes = new Dictionary<int, List<int>>();
                List<int> typesVisible = new List<int>();

                if (Session[Constants.sessionVisibleEmplTypes] != null && Session[Constants.sessionVisibleEmplTypes] is Dictionary<int, List<int>>)
                    companyVisibleTypes = (Dictionary<int, List<int>>)Session[Constants.sessionVisibleEmplTypes];

                // get company
                Dictionary<int, WorkingUnitTO> wuDict = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                int company = -1;
                int ID = -1;
                if (Menu1.SelectedItem.Value.Equals("0"))
                {
                    if (tbUte.Attributes["id"] != null)
                        if (!int.TryParse(tbUte.Attributes["id"].Trim(), out ID))
                            ID = -1;

                    company = Common.Misc.getRootWorkingUnit(ID, wuDict);
                }
                else
                {
                    if (tbOrgUte.Attributes["id"] != null)
                        if (!int.TryParse(tbOrgUte.Attributes["id"].Trim(), out ID))
                            ID = -1;

                    if (ID != -1)
                    {
                        WorkingUnitXOrganizationalUnit wuXou = new WorkingUnitXOrganizationalUnit(Session[Constants.sessionConnection]);
                        wuXou.WUXouTO.OrgUnitID = ID;
                        List<WorkingUnitXOrganizationalUnitTO> list = wuXou.Search();
                        if (list.Count > 0)
                            company = Common.Misc.getRootWorkingUnit(list[0].WorkingUnitID, wuDict);
                    }
                }

                if (companyVisibleTypes.ContainsKey(company))
                    typesVisible = companyVisibleTypes[company];

                List<EmployeeTypeTO> emplTypes = new List<EmployeeTypeTO>();
                EmployeeTypeTO all = new EmployeeTypeTO();
                all.WorkingUnitID = company;
                all.EmployeeTypeName = "*";
                emplTypes.Add(all);

                if (typesVisible.Count > 0)
                {
                    List<EmployeeTypeTO> types = new EmployeeType(Session[Constants.sessionConnection]).Search();

                    foreach (EmployeeTypeTO type in types)
                    {
                        if (type.WorkingUnitID == company && typesVisible.Contains(type.EmployeeTypeID))
                            emplTypes.Add(type);
                    }
                }

                cbEmplType.DataSource = emplTypes;
                cbEmplType.DataTextField = "EmployeeTypeName";
                cbEmplType.DataValueField = "EmployeeTypeID";
                cbEmplType.DataBind();

                rowEmplType.Visible = rowEmplTypeLbl.Visible = typesVisible.Count > 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populateEmployees(int ID, bool isWU)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLAnnualLeavePage).Assembly);

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

                if (from.Date < new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1, 0, 0, 0))
                {
                    lblError.Text = rm.GetString("changingLessPreviousMonth", culture);
                    return;
                }

                List<EmployeeTO> empolyeeList = new List<EmployeeTO>();

                //int onlyEmplTypeID = -1;
                //int exceptEmplTypeID = -1;
                int emplID = -1;

                //Dictionary<int, List<int>> companyVisibleTypes = new Dictionary<int, List<int>>();
                List<int> typesVisible = new List<int>();

                if (cbEmplType.SelectedIndex > 0)
                {
                    int type = -1;
                    if (int.TryParse(cbEmplType.SelectedValue.Trim(), out type) && type != -1)
                        typesVisible.Add(type);
                }
                else
                {
                    foreach (ListItem item in cbEmplType.Items)
                    {
                        int type = -1;
                        if (int.TryParse(item.Value.Trim(), out type) && type != -1)
                            typesVisible.Add(type);
                    }
                }

                //if (Session[Constants.sessionVisibleEmplTypes] != null && Session[Constants.sessionVisibleEmplTypes] is Dictionary<int, List<int>>)
                //    companyVisibleTypes = (Dictionary<int, List<int>>)Session[Constants.sessionVisibleEmplTypes];

                //// get company
                //Dictionary<int, WorkingUnitTO> wuDict = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                //int company = -1;
                //if (isWU)
                //    company = Common.Misc.getRootWorkingUnit(ID, wuDict);
                //else
                //{
                //    WorkingUnitXOrganizationalUnit wuXou = new WorkingUnitXOrganizationalUnit(Session[Constants.sessionConnection]);
                //    wuXou.WUXouTO.OrgUnitID = ID;
                //    List<WorkingUnitXOrganizationalUnitTO> list = wuXou.Search();
                //    if (list.Count > 0)
                //        company = Common.Misc.getRootWorkingUnit(list[0].WorkingUnitID, wuDict);
                //}

                //if (companyVisibleTypes.ContainsKey(company))
                //    typesVisible = companyVisibleTypes[company];

                if (Session[Constants.sessionLogInEmployee] != null && Session[Constants.sessionLogInEmployee] is EmployeeTO)
                    emplID = ((EmployeeTO)Session[Constants.sessionLogInEmployee]).EmployeeID;

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

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLAnnualLeavePage).Assembly);

                lblEmployee.Text = rm.GetString("lblEmployeeSelection", culture);
                lblPeriod.Text = rm.GetString("lblPeriod", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblOverwritePairs.Text = rm.GetString("lblOverwritePairs", culture);
                lblEmplType.Text = rm.GetString("lblEmployeeType", culture);

                rbYes.Text = rm.GetString("yes", culture);
                rbNo.Text = rm.GetString("no", culture);

                cbSelectAllEmpolyees.Text = rm.GetString("lblSelectAll", culture);

                btnShow.Text = rm.GetString("btnShow", culture);
                btnSave.Text = rm.GetString("btnSave", culture);
                chbReport.Text = rm.GetString("chbPrintAnnualReport", culture);
                btnPrint.Text = rm.GetString("btnPrintAnnualReport", culture);



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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLAnnualLeavePage).Assembly);

                Session[Constants.sessionHeader] = rm.GetString("hdrEmployeeID", culture) + "," + rm.GetString("hdrEmployee", culture) + "," + rm.GetString("hdrPassType", culture) + "," + rm.GetString("hdrDate", culture) + ","
                    + rm.GetString("hdrStartTime", culture) + "," + rm.GetString("hdrEndTime", culture) + "," + rm.GetString("hdrHours", culture);
                Session[Constants.sessionFields] = "emplID, empl, pt, pairdate, pairstart, pairend, hours";
                Dictionary<int, int> formating = new Dictionary<int, int>();
                formating.Add(3, (int)Constants.FormatTypes.DateFormat);
                formating.Add(4, (int)Constants.FormatTypes.TimeFormat);
                formating.Add(5, (int)Constants.FormatTypes.TimeFormat);
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
                Dictionary<int, WorkingUnitTO> wUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                populateEmplTypes();
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLAnnualLeavePage.Menu1_MenuItemClick(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLAnnualLeavePage.aspx&Header=" + Constants.falseValue.Trim(), false);
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

                Session[Constants.sessionResultCurrentPage] = null;
                Session[Constants.sessionDataTableList] = null;
                Session[Constants.sessionDataTableColumns] = null;
                Session[Constants.sessionItemsColors] = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ClearPageSessionValues()
        {
            try
            {
                Session[sessionPairs] = null;
                Session[sessionOldPairs] = null;
                Session[sessionCounters] = null;

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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLAnnualLeavePage).Assembly);

                ClearSessionValues();
                ClearPageSessionValues();

                lblError.Text = "";

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

                if (fromDate.Date < new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1, 0, 0, 0))
                {
                    lblError.Text = rm.GetString("changingLessPreviousMonth", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                Session[Constants.sessionFromDate] = fromDate;
                Session[Constants.sessionToDate] = toDate;

                Dictionary<int, WorkingUnitTO> wUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();

                // get number of working days
                int workingDaysNum = Common.Misc.countWorkingDays(DateTime.Now.Date, Session[Constants.sessionConnection]);

                //get hrssc cuttoff date
                int hrsscCutoffDate = -1;
                if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                    && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRSSC)
                {
                    if (Session[Constants.sessionLogInEmployee] != null && Session[Constants.sessionLogInEmployee] is EmployeeTO)
                    {
                        int company = Common.Misc.getRootWorkingUnit(((EmployeeTO)Session[Constants.sessionLogInEmployee]).WorkingUnitID, wUnits);

                        if (company != -1)
                        {
                            Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                            rule.RuleTO.WorkingUnitID = company;
                            rule.RuleTO.EmployeeTypeID = ((EmployeeTO)Session[Constants.sessionLogInEmployee]).EmployeeTypeID;
                            rule.RuleTO.RuleType = Constants.RuleHRSSCCutOffDate;

                            List<RuleTO> rules = rule.Search();

                            if (rules.Count == 1)
                            {
                                hrsscCutoffDate = rules[0].RuleValue;
                            }
                        }
                    }

                    if (((hrsscCutoffDate != -1 && workingDaysNum > hrsscCutoffDate) || hrsscCutoffDate == -1)
                        && fromDate.Date < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0))
                    {
                        lblError.Text = rm.GetString("changingPreviousMonthCuttOffDatePassed", culture);
                        writeLog(DateTime.Now, false);
                        return;
                    }
                }

                // create list of columns for list privew of pairs for inserting
                List<DataColumn> resultColumns = new List<DataColumn>();
                resultColumns.Add(new DataColumn("emplID", typeof(int)));
                resultColumns.Add(new DataColumn("empl", typeof(string)));
                resultColumns.Add(new DataColumn("pt", typeof(string)));
                resultColumns.Add(new DataColumn("pairdate", typeof(DateTime)));
                resultColumns.Add(new DataColumn("pairstart", typeof(DateTime)));
                resultColumns.Add(new DataColumn("pairend", typeof(DateTime)));
                resultColumns.Add(new DataColumn("hours", typeof(string)));

                // get all selected employees for inserting pairs                
                // if overwritting pairs is selected, all existing pairs for selected days will be moved to hist table so get all selected employees
                // if overwritting pairs is not selected, remove all employees that has at least one pair different than unjustified for one of selected days
                // get all pairs for selected employees for selected days - get one day more for third shifts
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
                }
                else
                {
                    foreach (ListItem item in lboxEmployees.Items)
                    {
                        emplIDs += item.Value.Trim() + ",";
                    }
                }

                if (emplIDs.Length > 0)
                    emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                List<EmployeeTO> emplList = new Employee(Session[Constants.sessionConnection]).Search(emplIDs);

                // get asco data
                Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4(Session[Constants.sessionConnection]).SearchDictionary(emplIDs);

                List<DateTime> datesList = new List<DateTime>();
                DateTime currDate = fromDate.Date;
                while (currDate <= toDate.AddDays(1).Date)
                {
                    datesList.Add(currDate);
                    currDate = currDate.AddDays(1);
                }

                // get dictionary of all rules, key is company and value are rules by employee type id
                Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> emplRules = new Common.Rule(Session[Constants.sessionConnection]).SearchWUEmplTypeDictionary();

                // dictioanry of employees
                Dictionary<int, EmployeeTO> employees = new Dictionary<int, EmployeeTO>();

                foreach (EmployeeTO empl in emplList)
                {
                    if (!employees.ContainsKey(empl.EmployeeID))
                        employees.Add(empl.EmployeeID, empl);
                }
                Session["TLAnnualLeavePage.employeeDict"] = employees;
                Session["TLAnnualLeavePage.employeeAsco4Dict"] = ascoDict;
                if (rbNo.Checked)
                {
                    List<IOPairProcessedTO> emplPairsList = new IOPairProcessed(Session[Constants.sessionConnection]).SearchAllPairsForEmpl(emplIDs, datesList, "");

                    List<int> emplPairIDs = new List<int>();

                    foreach (IOPairProcessedTO pair in emplPairsList)
                    {
                        int holidayPT = -1;
                        int personalHolidayPT = -1;
                        int emplCompany = -1;
                        if (employees.ContainsKey(pair.EmployeeID))
                        {
                            emplCompany = Common.Misc.getRootWorkingUnit(employees[pair.EmployeeID].WorkingUnitID, wUnits);

                            if (emplRules.ContainsKey(emplCompany) && emplRules[emplCompany].ContainsKey(employees[pair.EmployeeID].EmployeeTypeID)
                                && emplRules[emplCompany][employees[pair.EmployeeID].EmployeeTypeID].ContainsKey(Constants.RuleHolidayPassType))
                                holidayPT = emplRules[emplCompany][employees[pair.EmployeeID].EmployeeTypeID][Constants.RuleHolidayPassType].RuleValue;

                            if (emplRules.ContainsKey(emplCompany) && emplRules[emplCompany].ContainsKey(employees[pair.EmployeeID].EmployeeTypeID)
                                && emplRules[emplCompany][employees[pair.EmployeeID].EmployeeTypeID].ContainsKey(Constants.RulePersonalHolidayPassType))
                                personalHolidayPT = emplRules[emplCompany][employees[pair.EmployeeID].EmployeeTypeID][Constants.RulePersonalHolidayPassType].RuleValue;
                        }

                        //**********night shift
                        if (pair.PassTypeID != Constants.absence && pair.PassTypeID != holidayPT && pair.PassTypeID != personalHolidayPT
                            && !emplPairIDs.Contains(pair.EmployeeID) && ((pair.IOPairDate.Date > fromDate.Date && pair.IOPairDate.Date <= toDate.Date)
                            || (pair.IOPairDate.Date == fromDate.Date && !(pair.StartTime.Hour == 0 && pair.StartTime.Minute == 0))
                            || (pair.IOPairDate.Date > toDate.Date && pair.StartTime.Hour == 0 && pair.StartTime.Minute == 0)))
                            emplPairIDs.Add(pair.EmployeeID);
                    }

                    // remove from employee list employees that has at least one pair in selected period
                    IEnumerator<EmployeeTO> emplEnumerator = emplList.GetEnumerator();
                    while (emplEnumerator.MoveNext())
                    {
                        if (emplPairIDs.Contains(emplEnumerator.Current.EmployeeID))
                        {
                            emplList.Remove(emplEnumerator.Current);
                            emplEnumerator = emplList.GetEnumerator();
                        }
                    }

                    // get new emplIDs string of employees allowed to insert pairs
                    emplIDs = "";
                    foreach (EmployeeTO empl in emplList)
                    {
                        emplIDs += empl.EmployeeID.ToString().Trim() + ",";
                    }

                    if (emplIDs.Length > 0)
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                }

                if (emplList.Count <= 0)
                {
                    lblError.Text = rm.GetString("noAnnualLeaveAllowedEmployees", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                // get time schedules and time schemas for selected employees for selected days, add one day for third shifts                
                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule(Session[Constants.sessionConnection]).SearchEmployeesSchedulesExactDate(emplIDs, fromDate.Date, toDate.AddDays(1).Date, null);

                Dictionary<int, WorkTimeSchemaTO> schemas = new TimeSchema(Session[Constants.sessionConnection]).getDictionary();

                // get employee holidays
                Dictionary<int, List<DateTime>> personalHolidays = new Dictionary<int, List<DateTime>>();
                Dictionary<int, List<DateTime>> emplHolidays = employeeHolidays(ref personalHolidays, fromDate.Date, toDate.Date, emplIDs, emplSchedules, schemas, employees, emplRules);

                // get old pairs for employees for selected days, and for next day becouse of third shifts
                List<IOPairProcessedTO> oldPairs = new IOPairProcessed(Session[Constants.sessionConnection]).SearchAllPairsForEmpl(emplIDs, datesList, "");

                // create dictionary of old pairs list for each employee by date
                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplOldPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                foreach (IOPairProcessedTO oldPair in oldPairs)
                {
                    if (!emplOldPairs.ContainsKey(oldPair.EmployeeID))
                        emplOldPairs.Add(oldPair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());

                    if (!emplOldPairs[oldPair.EmployeeID].ContainsKey(oldPair.IOPairDate.Date))
                        emplOldPairs[oldPair.EmployeeID].Add(oldPair.IOPairDate.Date, new List<IOPairProcessedTO>());

                    emplOldPairs[oldPair.EmployeeID][oldPair.IOPairDate.Date].Add(oldPair);
                }

                // get old counters
                Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplOldCounters = new EmployeeCounterValue(Session[Constants.sessionConnection]).SearchValues(emplIDs);

                // get all pass types
                Dictionary<int, PassTypeTO> passTypesAll = new PassType(Session[Constants.sessionConnection]).SearchDictionary();

                // get pass types limits
                List<PassTypeLimitTO> ptLimitsList = new PassTypeLimit(Session[Constants.sessionConnection]).Search();
                Dictionary<int, PassTypeLimitTO> ptLimits = new Dictionary<int, PassTypeLimitTO>();

                foreach (PassTypeLimitTO limit in ptLimitsList)
                {
                    if (!ptLimits.ContainsKey(limit.PtLimitID))
                        ptLimits.Add(limit.PtLimitID, limit);
                    else
                        ptLimits[limit.PtLimitID] = limit;
                }

                if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                    && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.TL)
                {
                    bool cutOffDateRule = false;

                    foreach (EmployeeTO empl in emplList)
                    {
                        int cutOffDate = -1;
                        int emplCompany = Common.Misc.getRootWorkingUnit(empl.WorkingUnitID, wUnits);

                        if (emplRules.ContainsKey(emplCompany) && emplRules[emplCompany].ContainsKey(empl.EmployeeTypeID) && emplRules[emplCompany][empl.EmployeeTypeID].ContainsKey(Constants.RuleCutOffDate))
                            cutOffDate = emplRules[emplCompany][empl.EmployeeTypeID][Constants.RuleCutOffDate].RuleValue;

                        if (((cutOffDate != -1 && workingDaysNum > cutOffDate) || cutOffDate == -1)
                            && fromDate.Date < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0))
                        {
                            cutOffDateRule = true;
                            lblError.Text = rm.GetString("changingPreviousMonthCuttOffDatePassed", culture);
                            break;
                        }
                    }

                    if (cutOffDateRule)
                    {
                        writeLog(DateTime.Now, false);
                        return;
                    }
                }

                List<List<object>> resultTable = new List<List<object>>();

                Dictionary<int, List<IOPairProcessedTO>> emplPairs = new Dictionary<int, List<IOPairProcessedTO>>();
                Dictionary<int, List<IOPairProcessedTO>> emplValidPairs = new Dictionary<int, List<IOPairProcessedTO>>();

                Dictionary<int, string> emplValidationErrors = new Dictionary<int, string>();

                string error = "";

                // go through one day more if employee worked third shift for last selected day                
                foreach (EmployeeTO empl in emplList)
                {
                    if (!emplPairs.ContainsKey(empl.EmployeeID))
                        emplPairs.Add(empl.EmployeeID, new List<IOPairProcessedTO>());

                    // get rules for employeeID and its company
                    Dictionary<string, RuleTO> rules = new Dictionary<string, RuleTO>();

                    int company = -1;
                    int emplTypeID = -1;
                    if (employees.ContainsKey(empl.EmployeeID))
                    {
                        company = Common.Misc.getRootWorkingUnit(employees[empl.EmployeeID].WorkingUnitID, wUnits);
                        emplTypeID = employees[empl.EmployeeID].EmployeeTypeID;
                    }
                    if (company != -1 && emplTypeID != -1 && emplRules.ContainsKey(company) && emplRules[company].ContainsKey(emplTypeID))
                        rules = emplRules[company][emplTypeID];

                    // get company annualLeave
                    PassTypeTO ptAL = new PassTypeTO();
                    if (rules.ContainsKey(Constants.RuleCompanyAnnualLeave) && passTypesAll.ContainsKey(rules[Constants.RuleCompanyAnnualLeave].RuleValue))
                    {
                        ptAL = passTypesAll[rules[Constants.RuleCompanyAnnualLeave].RuleValue];
                    }
                    else
                    {
                        lblError.Text = rm.GetString("noCompanyAnnualLeave", culture);
                        writeLog(DateTime.Now, false);
                        return;
                    }

                    PassTypeTO ptCAL = new PassTypeTO();
                    if (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && passTypesAll.ContainsKey(rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue))
                    {
                        ptCAL = passTypesAll[rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue];
                    }

                    // old pairs which will be moved to hist table
                    List<IOPairProcessedTO> oldPairsList = new List<IOPairProcessedTO>();

                    DateTime date = fromDate.Date;
                    Dictionary<DateTime, WorkTimeSchemaTO> daySchemas = new Dictionary<DateTime, WorkTimeSchemaTO>();
                    Dictionary<DateTime, List<WorkTimeIntervalTO>> dayIntervalsList = new Dictionary<DateTime, List<WorkTimeIntervalTO>>();
                    while (date <= toDate.AddDays(1).Date)
                    {
                        DateTime dayBegining = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                        DateTime dayEnd = new DateTime(date.Year, date.Month, date.Day, 23, 59, 0);

                        // get intervals for employee/date
                        List<WorkTimeIntervalTO> timeSchemaIntervalList = new List<WorkTimeIntervalTO>();
                        WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                        if (emplSchedules.ContainsKey(empl.EmployeeID))
                        {
                            timeSchemaIntervalList = Common.Misc.getTimeSchemaInterval(date.Date, emplSchedules[empl.EmployeeID], schemas);
                            if (timeSchemaIntervalList.Count > 0 && schemas.ContainsKey(timeSchemaIntervalList[0].TimeSchemaID))
                                schema = schemas[timeSchemaIntervalList[0].TimeSchemaID];
                        }

                        if (schema.Type == Constants.schemaTypeNightFlexi)
                        {
                            if (!emplValidationErrors.ContainsKey(empl.EmployeeID))
                                emplValidationErrors.Add(empl.EmployeeID, rm.GetString("nightFlexyChangesNotAllowed", culture));
                            break;
                        }

                        if (!dayIntervalsList.ContainsKey(date.Date))
                            dayIntervalsList.Add(date.Date, timeSchemaIntervalList);
                        else
                            dayIntervalsList[date.Date] = timeSchemaIntervalList;

                        if (!daySchemas.ContainsKey(date.Date))
                            daySchemas.Add(date.Date, schema);
                        else
                            daySchemas[date.Date] = schema;

                        // validate pairs and insert pairs if valid for all intervals
                        foreach (WorkTimeIntervalTO interval in timeSchemaIntervalList)
                        {
                            // do not insert pairs for interval of 0 minutes
                            if (interval.StartTime.TimeOfDay.Equals(interval.EndTime.TimeOfDay))
                                continue;

                            // first day - do not insert second night shift interval, it belongs to previous day
                            if (date.Date.Equals(fromDate.Date) && Common.Misc.isThirdShiftEndInterval(interval))
                            {
                                // remove old pairs from this interval
                                removeIntervalPairs(ref emplOldPairs, empl.EmployeeID, date.Date, interval, schema);
                                continue;
                            }

                            // last day - insert second night shift interval from next day
                            if (date.Date.Equals(toDate.AddDays(1).Date) && !Common.Misc.isThirdShiftEndInterval(interval))
                            {
                                // remove old pairs from this interval
                                removeIntervalPairs(ref emplOldPairs, empl.EmployeeID, date.Date, interval, schema);
                                continue;
                            }

                            // do not insert if day is employees holiday
                            if (!schema.Type.Trim().ToUpper().Equals(Constants.schemaTypeIndustrial.Trim().ToUpper()))
                            {
                                if (emplHolidays.ContainsKey(empl.EmployeeID) && emplHolidays[empl.EmployeeID].Contains(date.AddDays(-1).Date) && Common.Misc.isThirdShiftEndInterval(interval))
                                {
                                    removeIntervalPairs(ref emplOldPairs, empl.EmployeeID, date.Date, interval, schema);
                                    continue;
                                }

                                if (emplHolidays.ContainsKey(empl.EmployeeID) && emplHolidays[empl.EmployeeID].Contains(date.Date) && !Common.Misc.isThirdShiftEndInterval(interval))
                                {
                                    removeIntervalPairs(ref emplOldPairs, empl.EmployeeID, date.Date, interval, schema);
                                    continue;
                                }
                            }
                            else
                            {
                                if (personalHolidays.ContainsKey(empl.EmployeeID) && personalHolidays[empl.EmployeeID].Contains(date.AddDays(-1).Date) && Common.Misc.isThirdShiftEndInterval(interval))
                                {
                                    removeIntervalPairs(ref emplOldPairs, empl.EmployeeID, date.Date, interval, schema);
                                    continue;
                                }

                                if (personalHolidays.ContainsKey(empl.EmployeeID) && personalHolidays[empl.EmployeeID].Contains(date.Date) && !Common.Misc.isThirdShiftEndInterval(interval))
                                {
                                    removeIntervalPairs(ref emplOldPairs, empl.EmployeeID, date.Date, interval, schema);
                                    continue;
                                }
                            }

                            DateTime start = CommonWeb.Misc.getIntervalStart(interval, new List<IOPairProcessedTO>(), schema, new DateTime(), new Dictionary<int, PassTypeTO>());
                            DateTime end = CommonWeb.Misc.getIntervalEnd(interval, new List<IOPairProcessedTO>(), schema, new DateTime(), new Dictionary<int, PassTypeTO>());

                            start = new DateTime(date.Year, date.Month, date.Day, start.Hour, start.Minute, 0);
                            end = new DateTime(date.Year, date.Month, date.Day, end.Hour, end.Minute, 0);

                            IOPairProcessedTO pair = createPair(empl.EmployeeID, ptAL, date.Date, start, end);

                            emplPairs[empl.EmployeeID].Add(pair);
                        }

                        if (emplOldPairs.ContainsKey(empl.EmployeeID) && emplOldPairs[empl.EmployeeID].ContainsKey(date.Date))
                            oldPairsList.AddRange(emplOldPairs[empl.EmployeeID][date.Date]);

                        date = date.AddDays(1);
                    }

                    if (emplValidationErrors.ContainsKey(empl.EmployeeID))
                        continue;

                    // old counters
                    Dictionary<int, EmployeeCounterValueTO> oldCounters = new Dictionary<int, EmployeeCounterValueTO>();
                    if (emplOldCounters.ContainsKey(empl.EmployeeID))
                        oldCounters = emplOldCounters[empl.EmployeeID];

                    if (emplPairs[empl.EmployeeID].Count > 0)
                    {
                        // get annual leaves from from and to week
                        List<IOPairProcessedTO> fromWeekAnnualPairs = new List<IOPairProcessedTO>();
                        List<IOPairProcessedTO> toWeekAnnualPairs = new List<IOPairProcessedTO>();

                        IOPairProcessed annualPair = new IOPairProcessed(Session[Constants.sessionConnection]);

                        string ptIDs = ptAL.PassTypeID.ToString().Trim();
                        if (ptCAL.PassTypeID != -1)
                            ptIDs += "," + ptCAL.PassTypeID.ToString().Trim();

                        fromWeekAnnualPairs = annualPair.SearchWeekPairs(empl.EmployeeID, fromDate.Date, false, ptIDs, null);
                        toWeekAnnualPairs = annualPair.SearchWeekPairs(empl.EmployeeID, toDate.Date, false, ptIDs, null);

                        // remove pairs that are in selected period
                        IEnumerator<IOPairProcessedTO> pairEnumerator = fromWeekAnnualPairs.GetEnumerator();
                        while (pairEnumerator.MoveNext())
                        {
                            if (pairEnumerator.Current.IOPairDate.Date >= fromDate.Date)
                            {
                                fromWeekAnnualPairs.Remove(pairEnumerator.Current);
                                pairEnumerator = fromWeekAnnualPairs.GetEnumerator();
                            }
                        }

                        pairEnumerator = toWeekAnnualPairs.GetEnumerator();
                        while (pairEnumerator.MoveNext())
                        {
                            if (pairEnumerator.Current.IOPairDate.Date <= toDate.Date)
                            {
                                toWeekAnnualPairs.Remove(pairEnumerator.Current);
                                pairEnumerator = toWeekAnnualPairs.GetEnumerator();
                            }
                        }

                        EmployeeAsco4TO emplAsco = new EmployeeAsco4TO();
                        if (ascoDict.ContainsKey(empl.EmployeeID))
                            emplAsco = ascoDict[empl.EmployeeID];

                        Dictionary<int, int> paidLeavesElementaryPairsDict = new Dictionary<int, int>();
                        error = Common.Misc.validatePairsPassType(empl.EmployeeID, emplAsco, fromDate.Date, toDate.Date, emplPairs[empl.EmployeeID], oldPairsList, new List<IOPairProcessedTO>(), ref oldCounters, rules,
                                passTypesAll, ptLimits, schemas, daySchemas, dayIntervalsList, fromWeekAnnualPairs, toWeekAnnualPairs, paidLeavesElementaryPairsDict,
                                new List<IOPairProcessedTO>(), new List<DateTime>(), null, Session[Constants.sessionConnection], CommonWeb.Misc.checkLimit(Session[Constants.sessionLoginCategory]), false, true, false);
                        //error = validatePairsPassType(fromDate.Date, toDate.Date, emplPairs[empl.EmployeeID], oldPairsList, ref oldCounters, rules, passTypesAll, ptLimits, daySchemas,
                        //    fromWeekAnnualPairs, toWeekAnnualPairs);

                        if (error.Trim().Equals(""))
                        {
                            if (emplOldCounters.ContainsKey(empl.EmployeeID))
                                emplOldCounters[empl.EmployeeID] = oldCounters;

                            if (!emplValidPairs.ContainsKey(empl.EmployeeID))
                                emplValidPairs.Add(empl.EmployeeID, emplPairs[empl.EmployeeID]);
                        }
                        else if (!emplValidationErrors.ContainsKey(empl.EmployeeID))
                            emplValidationErrors.Add(empl.EmployeeID, rm.GetString(error.Trim(), culture));
                    }
                }

                foreach (int emplID in emplValidPairs.Keys)
                {
                    foreach (IOPairProcessedTO pair in emplValidPairs[emplID])
                    {
                        EmployeeTO empl = new EmployeeTO();
                        if (employees.ContainsKey(emplID))
                            empl = employees[emplID];

                        resultTable.Add(createResultRow(empl, pair));
                    }
                }

                Session[sessionPairs] = emplValidPairs;
                Session[sessionOldPairs] = emplOldPairs;
                Session[sessionCounters] = emplOldCounters;
                Session[Constants.sessionDataTableList] = resultTable;
                Session[Constants.sessionDataTableColumns] = resultColumns;
                if (emplValidPairs.Count > 0)
                {
                    btnPrint.Enabled = false;
                    btnPrint.CssClass = "contentBtnDisabled";
                }
                string errorMessage = "";
                if (emplValidationErrors.Count == 1)
                {
                    int emplID = emplValidationErrors.Keys.ElementAt<int>(0);
                    lblError.Text = emplID.ToString().Trim();
                    if (employees.ContainsKey(emplID))
                        lblError.Text += " " + employees[emplID].FirstAndLastName.Trim();
                    lblError.Text += ": " + emplValidationErrors[emplID].Trim();
                }
                else
                {
                    foreach (int emplID in emplValidationErrors.Keys)
                    {
                        errorMessage += emplID.ToString().Trim();
                        if (employees.ContainsKey(emplID))
                            errorMessage += " " + employees[emplID].FirstAndLastName.Trim();
                        errorMessage += ": " + emplValidationErrors[emplID].Trim() + " ";
                    }
                }

                // save selected filter state
                SaveState();

                resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?showSelection=false";

                // show error page pop up
                if (!errorMessage.Trim().Equals(""))
                {
                    Session[Constants.sessionInfoMessage] = errorMessage.Trim();
                    string wOptions = "dialogWidth:800px; dialogHeight:300px; edge:Raised; center:Yes; resizable:Yes; status:Yes;";
                    ClientScript.RegisterStartupScript(GetType(), "infoPopup", "window.open('/ACTAWeb/CommonWeb/MessagesPages/Info.aspx?isError=true', window, '" + wOptions.Trim() + "');", true);

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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLAnnualLeavePage.btnShow_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLAnnualLeavePage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnPrint_Click(Object sender, EventArgs e)
        {
            try
            {
                Session["TLAnnualLeavePage.ds"] = null;
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLAnnualLeavePage).Assembly);

                ClearSessionValues();
                ClearPageSessionValues();
                bool stalni_privremeni = true;

                lblError.Text = "";

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

                Session[Constants.sessionFromDate] = fromDate;
                Session[Constants.sessionToDate] = toDate;
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
                }
                else
                {
                    foreach (ListItem item in lboxEmployees.Items)
                    {
                        emplIDs += item.Value.Trim() + ",";
                    }
                }

                if (emplIDs.Length > 0)
                    emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                List<EmployeeTO> emplList = new Employee(Session[Constants.sessionConnection]).Search(emplIDs);
                Dictionary<int, WorkingUnitTO> wUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();

                // get asco data
                Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4(Session[Constants.sessionConnection]).SearchDictionary(emplIDs);
                WorkingUnit wu = new WorkingUnit();
                WorkingUnitTO rootWu = new WorkingUnitTO();
                
                List<DateTime> datesList = new List<DateTime>();
                DateTime currDate = fromDate.Date;
                while (currDate < toDate.AddDays(1).Date)
                {
                    datesList.Add(currDate);
                    currDate = currDate.AddDays(1);
                }
                // dictioanry of employees
                Dictionary<int, EmployeeTO> employees = new Dictionary<int, EmployeeTO>();

                foreach (EmployeeTO empl in emplList)
                {
                    if (!employees.ContainsKey(empl.EmployeeID))
                        employees.Add(empl.EmployeeID, empl);
                }
                List<IOPairProcessedTO> emplPairsList = new IOPairProcessed(Session[Constants.sessionConnection]).SearchAllPairsForEmpl(emplIDs, datesList, "");
                Dictionary<int, List<IOPairProcessedTO>> emplValidPairs = new Dictionary<int, List<IOPairProcessedTO>>();
                Dictionary<int, int> emplAfterAL = new Dictionary<int, int>();
                Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> emplRules = new Common.Rule(Session[Constants.sessionConnection]).SearchWUEmplTypeDictionary();
                // get all pass types
                Dictionary<int, PassTypeTO> passTypesAll = new PassType(Session[Constants.sessionConnection]).SearchDictionary();
                Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplOldCompareCounters = new EmployeeCounterValue(Session[Constants.sessionConnection]).SearchValues(emplIDs);
                if (emplPairsList.Count <= 0)
                {
                    lblError.Text = rm.GetString("noReportData", culture);
                    return;
                }
             
               
                foreach (IOPairProcessedTO pair in emplPairsList)
                {

                    // get rules for employeeID and its company
                    Dictionary<string, RuleTO> rules = new Dictionary<string, RuleTO>();

                    int company = -1;
                    int emplTypeID = -1;
                    if (employees.ContainsKey(pair.EmployeeID))
                    {
                        company = Common.Misc.getRootWorkingUnit(employees[pair.EmployeeID].WorkingUnitID, wUnits);
                        emplTypeID = employees[pair.EmployeeID].EmployeeTypeID;
                    }
                    if (company != -1 && emplTypeID != -1 && emplRules.ContainsKey(company) && emplRules[company].ContainsKey(emplTypeID))
                        rules = emplRules[company][emplTypeID];
                    
                    int emplID = pair.EmployeeID;

                   
                    // get company annualLeave
                    PassTypeTO ptAL = new PassTypeTO();
                    if (rules.ContainsKey(Constants.RuleCompanyAnnualLeave) && passTypesAll.ContainsKey(rules[Constants.RuleCompanyAnnualLeave].RuleValue))
                    {
                        ptAL = passTypesAll[rules[Constants.RuleCompanyAnnualLeave].RuleValue];
                    }
                    else
                    {
                        lblError.Text = rm.GetString("noCompanyAnnualLeave", culture);
                        writeLog(DateTime.Now, false);
                        return;
                    }

                    PassTypeTO ptCAL = new PassTypeTO();
                    if (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && passTypesAll.ContainsKey(rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue))
                    {
                        ptCAL = passTypesAll[rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue];
                    }
                    if (pair.PassTypeID == ptAL.PassTypeID || pair.PassTypeID == ptCAL.PassTypeID)
                    {

                        if (!emplValidPairs.ContainsKey(pair.EmployeeID))
                            emplValidPairs.Add(pair.EmployeeID, new List<IOPairProcessedTO>());

                        emplValidPairs[pair.EmployeeID].Add(pair);
                    }
                    if (!emplAfterAL.ContainsKey(pair.EmployeeID))
                        emplAfterAL.Add(pair.EmployeeID, 0);
                    
                int alAfterAl = new IOPairProcessed(Session[Constants.sessionConnection]).SearchCollectiveAnnualLeaves(pair.EmployeeID, ptAL.PassTypeID, toDate.AddDays(1), new List<DateTime>());
                int alAfterCal = new IOPairProcessed(Session[Constants.sessionConnection]).SearchCollectiveAnnualLeaves(pair.EmployeeID, ptCAL.PassTypeID, toDate.AddDays(1), new List<DateTime>());

                emplAfterAL[pair.EmployeeID] = alAfterAl + alAfterCal;
                }
                 
                if (emplValidPairs.Count <= 0)
                {
                    lblError.Text = rm.GetString("noReportData", culture);
                    return;
                }
                DataSet ds = new DataSet();
                DataTable table = new DataTable();
                table.Columns.Add("employee", typeof(string));
               
                table.Columns.Add("working_unit", typeof(string));
                table.Columns.Add("inital_an", typeof(string));
                table.Columns.Add("length", typeof(string));
                table.Columns.Add("start", typeof(string));
                table.Columns.Add("end", typeof(string));
                table.Columns.Add("from_previous", typeof(string));
                table.Columns.Add("from_current", typeof(string));
                table.Columns.Add("total_left", typeof(string));
                table.Columns.Add("previous_left", typeof(string));
                table.Columns.Add("current_left", typeof(string));
                table.Columns.Add("hiring_date", typeof(string));
                table.Columns.Add("contract", typeof(string));
                table.Columns.Add("printDate", typeof(string));
                table.Columns.Add("position", typeof(string));
                table.Columns.Add("JMBG", typeof(string));
                ds.Tables.Add(table);


                RuleTO ruleTo = new RuleTO();
                ruleTo.RuleType = Constants.RulePrintPaidLeaveReportOffset;
                Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                rule.RuleTO = ruleTo;
                List<RuleTO> rulesList = rule.Search();

                foreach (KeyValuePair<int, List<IOPairProcessedTO>> IoPair in emplValidPairs)
                {
                    int annual_leave_count = 0;
                    foreach (IOPairProcessedTO pairProc in IoPair.Value)
                    {
                        if (!pairProc.StartTime.ToString("HH:mm").Equals("00:00") || (int)pairProc.EndTime.Subtract(pairProc.StartTime).TotalHours == Constants.dayDurationStandardShift)
                            annual_leave_count++;
                    }
                  
                    if (annual_leave_count > 0)
                    {
                        DataRow row = table.NewRow();
                        if (employees.ContainsKey(IoPair.Key))
                        {
                            row[0] = employees[IoPair.Key].FirstAndLastName;
                            if (wUnits.ContainsKey(employees[IoPair.Key].WorkingUnitID))
                                row[1] = wUnits[employees[IoPair.Key].WorkingUnitID].Description;
                        }
                        
                        if (emplOldCompareCounters.ContainsKey(IoPair.Key))
                        {
                            if (emplOldCompareCounters[IoPair.Key].ContainsKey((int)Constants.EmplCounterTypes.AnnualLeaveCounter))
                                row[2] = emplOldCompareCounters[IoPair.Key][(int)Constants.EmplCounterTypes.AnnualLeaveCounter].Value;
                            int previous_year = 0;
                            int current_year = 0;
                            int used = 0;
                            if (emplOldCompareCounters[IoPair.Key].ContainsKey((int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter))
                                previous_year = emplOldCompareCounters[IoPair.Key][(int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter].Value;

                            if (emplOldCompareCounters[IoPair.Key].ContainsKey((int)Constants.EmplCounterTypes.AnnualLeaveCounter))
                                current_year = emplOldCompareCounters[IoPair.Key][(int)Constants.EmplCounterTypes.AnnualLeaveCounter].Value;

                            if (emplOldCompareCounters[IoPair.Key].ContainsKey((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter))
                                used = emplOldCompareCounters[IoPair.Key][(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter].Value;

                            if (emplAfterAL.ContainsKey(IoPair.Key))
                                used -= emplAfterAL[IoPair.Key];

                            if (emplOldCompareCounters[IoPair.Key].ContainsKey((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter))
                            {
                                //  used += IoPair.Value.Count;
                                int from_current = 0;
                                int from_previous = 0;
                                int previous_left = 0;
                                int current_left = 0;

                                int iopairused = previous_year - used;

                                if (iopairused <= 0)
                                {
                                    from_previous = iopairused + annual_leave_count;
                                    if (from_previous < 0)
                                    {
                                        from_previous = 0;
                                        from_current = annual_leave_count;
                                    }
                                    else
                                      from_current = Math.Abs(iopairused);
                                   
                                    previous_left = 0;
                                    current_left = current_year + iopairused;
                                }
                                else
                                {
                                    current_left = current_year;
                                    previous_left = iopairused;
                                    int count = iopairused - annual_leave_count;
                                    //if (count < 0)
                                    //{
                                    //    if (Math.Abs(count) > annual_leave_count)
                                    //    {
                                    //        from_previous = 0;
                                    //        from_current = annual_leave_count;
                                    //    }
                                    //    else
                                    //    {
                                    from_previous = annual_leave_count;
                                    from_current = annual_leave_count - from_previous;
                                    //    }

                                    //}
                                    //else
                                    //{
                                        //from_previous = annual_leave_count;
                                        //from_current = 0;

                                    //}
                                }
                                row[6] = from_previous;
                                row[7] = from_current;

                                row[8] = previous_left + current_left;
                                row[9] = previous_left;
                                row[10] = current_left;

                            }
                        }
                        row[3] = annual_leave_count;
                        row[4] = IoPair.Value[0].IOPairDate.ToString("dd.MM.yyyy");
                        row[5] = IoPair.Value[IoPair.Value.Count - 1].IOPairDate.ToString("dd.MM.yyyy");


                        if (ascoDict.ContainsKey(IoPair.Key))
                        {
                            row[11] = ascoDict[IoPair.Key].DatetimeValue5.ToString("dd.MM.yyyy");
                            if (!ascoDict[IoPair.Key].NVarcharValue10.Equals("")) row[12] = ascoDict[IoPair.Key].NVarcharValue10;
                            else row[12] = "      ";
                            if (!ascoDict[IoPair.Key].NVarcharValue4.Equals(""))
                            {
                                row["JMBG"] = ascoDict[IoPair.Key].NVarcharValue4;
                            }
                            else
                            {
                                row["JMBG"] = "NO DATA";
                            }
                            if (!ascoDict[IoPair.Key].NVarcharValue6.Equals(""))
                            {
                                if (ascoDict[IoPair.Key].NVarcharValue6.ToLower().Contains("stalni"))
                                    stalni_privremeni = true;
                                else if (ascoDict[IoPair.Key].NVarcharValue6.ToLower().Contains("privremeni"))
                                    stalni_privremeni = false;
                            }
                            rootWu = wu.FindWU(ascoDict[IoPair.Key].IntegerValue4);
                        }
                        DateTime offsetDate = new DateTime();
                        if (rulesList.Count > 0)
                        {

                            if (DateTime.Now.Date <= IoPair.Value[0].IOPairDate.Date)
                            {
                                if (IoPair.Value[0].IOPairDate.AddDays(-rulesList[0].RuleValue).Date < DateTime.Now.Date)
                                    offsetDate = DateTime.Now;
                                else
                                    offsetDate = IoPair.Value[0].IOPairDate.AddDays(-rulesList[0].RuleValue);
                            }
                            else
                            {

                                offsetDate = IoPair.Value[0].IOPairDate.AddDays(-rulesList[0].RuleValue);
                            }
                        }
                        else
                        {
                            if (DateTime.Now < IoPair.Value[0].IOPairDate)
                                offsetDate = DateTime.Now;
                            else offsetDate = IoPair.Value[0].IOPairDate;
                        }
                        row[13] = offsetDate.ToString("dd.MM.yyyy");

                        if (ascoDict.ContainsKey(IoPair.Key))
                        {
                            int positionID = ascoDict[IoPair.Key].IntegerValue6;
                            if (positionID != -1)
                            {
                                EmployeePosition posit = new EmployeePosition();
                                posit.EmplPositionTO.PositionID = positionID;
                                List<EmployeePositionTO> position = posit.SearchEmployeePositions();
                                row[14] = position[0].PositionCodeTitleSR;
                            }
                            else
                            {
                                row[14] = "/";
                            }
                        }

                        table.Rows.Add(row);
                    }
                }
                if (table.Rows.Count > 0)
                {

                    string costumer = Common.Misc.getCustomer(Session[Constants.sessionConnection]);
                    int cost = 0;
                    bool costum = int.TryParse(costumer, out cost);
                    bool isHyatt = (cost == (int)Constants.Customers.Hyatt);
                    bool isPMC = (cost == (int)Constants.Customers.PMC);
                    bool isGrundfos = (cost == (int)Constants.Customers.Grundfos);

                    Session["TLAnnualLeavePage.ds"] = ds;

                    string wOptions = "dialogWidth:800px; dialogHeight:600px; edge:Raised; center:Yes; resizable:Yes; status:Yes;";

                    if (isPMC)
                        ClientScript.RegisterStartupScript(GetType(), "infoPopup", "window.open('/ACTAWeb/ReportsWeb/TLAnnualLeaveReport.aspx', window, '" + wOptions.Trim() + "');", true);
                    else if (isGrundfos)
                        ClientScript.RegisterStartupScript(GetType(), "infoPopup", "window.open('/ACTAWeb/ReportsWeb/TLAnnualLeaveReportGrundfos.aspx', window, '" + wOptions.Trim() + "');", true);
                    else if (isHyatt)
                        ClientScript.RegisterStartupScript(GetType(), "infoPopup", "window.open('/ACTAWeb/ReportsWeb/TLAnnualLeaveReportHyatt.aspx', window, '" + wOptions.Trim() + "');", true);
                    else
                    {
                        if(stalni_privremeni && rootWu.Name.ToLower().Contains("hutchinson"))
                            ClientScript.RegisterStartupScript(GetType(), "infoPopup", "window.open('/ACTAWeb/ReportsWeb/TLAnnualLeaveReportHutchinsonStalni.aspx', window, '" + wOptions.Trim() + "');", true);
                        else if (!stalni_privremeni && rootWu.Name.ToLower().Contains("hutchinson"))
                            ClientScript.RegisterStartupScript(GetType(), "infoPopup", "window.open('/ACTAWeb/ReportsWeb/TLAnnualLeaveReportHutchinsonPrivremeni.aspx', window, '" + wOptions.Trim() + "');", true);
                        else
                        {
                            ClientScript.RegisterStartupScript(GetType(), "infoPopup", "window.open('/ACTAWeb/ReportsWeb/TLAnnualLeaveReport.aspx', window, '" + wOptions.Trim() + "');", true);
                        }
                    }//ScriptManager.RegisterStartupScript(this.Page, GetType(), "infoPopup", "window.open('/ACTAWeb/ReportsWeb/TLAnnualLeaveReport.aspx', window, '" + wOptions.Trim() + "');", true);
                }
                else
                {
                    lblError.Text = rm.GetString("noReportData", culture);
                }
            }
            catch (Exception ex)
            {

                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLAnnualLeavePage.btnSave_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLAnnualLeavePage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private int getPreviousUsed(int currLeft, int prevLeft, int used)
        {
            try
            {
                if (prevLeft > used)
                    return used;
                else
                    return prevLeft;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int getCurrentUsed(int currLeft, int prevLeft, int used)
        {
            try
            {
                if (prevLeft > used)
                    return 0;
                else
                    return used - prevLeft;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int getCurrentLeft(int curr, int prev, int used)
        {
            try
            {
                if (prev > used)
                    return curr;
                else
                    return curr + prev - used;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int getPreviousLeft(int curr, int prev, int used)
        {
            try
            {
                if (prev > used)
                    return prev - used;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnSave_Click(Object sender, EventArgs e)
        {
            try
            {
                // if system is closed, sign out and redirect to login page
                if (Common.Misc.getClosingEvent(DateTime.Now, Session[Constants.sessionConnection]).EventID != -1)
                    CommonWeb.Misc.LogOut(Session[Constants.sessionLogInUserLog], Session[Constants.sessionLogInUser], Session[Constants.sessionConnection], Session, Response);

                lblError.Text = "";
                Session["TLAnnualLeavePage.ds"] = null;
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLAnnualLeavePage).Assembly);

                if (Session[sessionPairs] != null && Session[sessionPairs] is Dictionary<int, List<IOPairProcessedTO>>)
                {
                    btnPrint.Enabled = true;
                    btnPrint.CssClass = "contentBtn";
                    // get old counters
                    string emplIDs = "";
                    foreach (int emplID in ((Dictionary<int, List<IOPairProcessedTO>>)Session[sessionPairs]).Keys)
                    {
                        emplIDs += emplID.ToString().Trim() + ",";
                    }

                    if (emplIDs.Length > 0)
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                    Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplOldCompareCounters = new EmployeeCounterValue(Session[Constants.sessionConnection]).SearchValues(emplIDs);

                    IOPairProcessed pair = new IOPairProcessed(Session[Constants.sessionConnection]);

                    if (pair.BeginTransaction())
                    {
                        try
                        {
                            EmployeeCounterValue counter = new EmployeeCounterValue(Session[Constants.sessionConnection]);
                            IOPairsProcessedHist pairHist = new IOPairsProcessedHist(Session[Constants.sessionConnection]);
                            EmployeeCounterValueHist counterHist = new EmployeeCounterValueHist(Session[Constants.sessionConnection]);
                            bool saved = true;
                            foreach (int emplID in ((Dictionary<int, List<IOPairProcessedTO>>)Session[sessionPairs]).Keys)
                            {
                                // get old pairs for this employee
                                List<IOPairProcessedTO> oldPairs = new List<IOPairProcessedTO>();
                                if (Session[sessionOldPairs] != null && Session[sessionOldPairs] is Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>
                                    && ((Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>)Session[sessionOldPairs]).ContainsKey(emplID))
                                {
                                    foreach (DateTime date in ((Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>)Session[sessionOldPairs])[emplID].Keys)
                                    {
                                        oldPairs.AddRange(((Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>)Session[sessionOldPairs])[emplID][date]);
                                    }
                                }

                                // get counters for this employee
                                Dictionary<int, EmployeeCounterValueTO> counters = new Dictionary<int, EmployeeCounterValueTO>();
                                if (Session[sessionCounters] != null && Session[sessionCounters] is Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>
                                    && ((Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>)Session[sessionCounters]).ContainsKey(emplID))
                                    counters = ((Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>)Session[sessionCounters])[emplID];

                                // get compare counters
                                Dictionary<int, EmployeeCounterValueTO> oldCounters = new Dictionary<int, EmployeeCounterValueTO>();
                                if (emplOldCompareCounters.ContainsKey(emplID))
                                    oldCounters = emplOldCompareCounters[emplID];

                                // get new pairs for this employee
                                List<IOPairProcessedTO> newPairs = ((Dictionary<int, List<IOPairProcessedTO>>)Session[sessionPairs])[emplID];

                                // move old pairs to hist table
                                string recIDs = "";
                                if (oldPairs.Count > 0)
                                {
                                    foreach (IOPairProcessedTO oldPair in oldPairs)
                                    {
                                        recIDs += oldPair.RecID.ToString().Trim() + ",";
                                    }

                                    if (recIDs.Length > 0)
                                        recIDs = recIDs.Substring(0, recIDs.Length - 1);
                                }

                                if (recIDs.Length > 0)
                                {
                                    pairHist.SetTransaction(pair.GetTransaction());
                                    saved = saved && (pairHist.Save(recIDs, CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]), DateTime.Now, Constants.alertStatus, false) >= 0);

                                    if (saved)
                                        saved = saved && pair.Delete(recIDs, false);
                                }

                                if (saved)
                                {
                                    foreach (IOPairProcessedTO pairTO in newPairs)
                                    {
                                        if ((int)pairTO.EndTime.Subtract(pairTO.StartTime).TotalMinutes > 0)
                                        {
                                            pair.IOPairProcessedTO = pairTO;
                                            saved = saved && (pair.Save(false) >= 0);

                                            if (!saved)
                                                break;
                                        }
                                    }
                                }

                                if (saved)
                                {
                                    // update counters from session, updated counters insert to hist table
                                    counterHist.SetTransaction(pair.GetTransaction());
                                    counter.SetTransaction(pair.GetTransaction());
                                    // update counters and move old counter values to hist table if updated
                                    foreach (int type in counters.Keys)
                                    {
                                        if (oldCounters.ContainsKey(type) && oldCounters[type].Value != counters[type].Value)
                                        {
                                            // move to hist table
                                            counterHist.ValueTO = new EmployeeCounterValueHistTO(oldCounters[type]);
                                            counterHist.ValueTO.ModifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                                            saved = saved && (counterHist.Save(false) >= 0);

                                            if (!saved)
                                                break;

                                            counter.ValueTO = new EmployeeCounterValueTO(counters[type]);
                                            //counter.ValueTO.EmplCounterTypeID = type;
                                            //counter.ValueTO.EmplID = emplID;
                                            //counter.ValueTO.Value = counters[type];
                                            counter.ValueTO.ModifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);

                                            saved = saved && counter.Update(false);

                                            if (!saved)
                                                break;
                                        }
                                    }
                                }
                            }

                            if (saved)
                            {
                                pair.CommitTransaction();

                                lblError.Text = rm.GetString("pairsSaved", culture);
                                if (chbReport.Checked)
                                {
                                    Dictionary<int, WorkingUnitTO> wuDict = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                                    Dictionary<int, List<IOPairProcessedTO>> ioPairs = (Dictionary<int, List<IOPairProcessedTO>>)Session[sessionPairs];
                                    Dictionary<int, EmployeeTO> employees = (Dictionary<int, EmployeeTO>)Session["TLAnnualLeavePage.employeeDict"];
                                    Dictionary<int, EmployeeAsco4TO> ascoDict = (Dictionary<int, EmployeeAsco4TO>)Session["TLAnnualLeavePage.employeeAsco4Dict"];
                                    Dictionary<int, int> emplAfterAL = new Dictionary<int, int>();

                                    DataSet ds = new DataSet();
                                    DataTable table = new DataTable();
                                    table.Columns.Add("employee", typeof(string));
                                    table.Columns.Add("working_unit", typeof(string));
                                    table.Columns.Add("inital_an", typeof(string));
                                    table.Columns.Add("length", typeof(string));
                                    table.Columns.Add("start", typeof(string));
                                    table.Columns.Add("end", typeof(string));
                                    table.Columns.Add("from_previous", typeof(string));
                                    table.Columns.Add("from_current", typeof(string));
                                    table.Columns.Add("total_left", typeof(string));
                                    table.Columns.Add("previous_left", typeof(string));
                                    table.Columns.Add("current_left", typeof(string));
                                    table.Columns.Add("hiring_date", typeof(string));
                                    table.Columns.Add("contract", typeof(string));
                                    table.Columns.Add("printDate", typeof(string));
                                    table.Columns.Add("position", typeof(string)); 
                                    ds.Tables.Add(table);

                                    RuleTO ruleTo = new RuleTO();
                                    ruleTo.RuleType = Constants.RulePrintPaidLeaveReportOffset;
                                    Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                                    rule.RuleTO = ruleTo;
                                    List<RuleTO> rulesList = rule.Search();

                                    foreach (KeyValuePair<int, List<IOPairProcessedTO>> IoPair in ioPairs)
                                    {
                                        int annual_leave_count = 0;
                                        foreach (IOPairProcessedTO pairProc in IoPair.Value)
                                        {
                                            if (!pairProc.StartTime.ToString("HH:mm").Equals("00:00"))
                                                annual_leave_count++;
                                        }
                                       
                                        if (annual_leave_count > 0)
                                        {
                                            DataRow row = table.NewRow();
                                            if (employees.ContainsKey(IoPair.Key))
                                            {
                                                row[0] = employees[IoPair.Key].FirstAndLastName;
                                                if (wuDict.ContainsKey(employees[IoPair.Key].WorkingUnitID))
                                                    row[1] = wuDict[employees[IoPair.Key].WorkingUnitID].Description;
                                            }

                                            if (emplOldCompareCounters.ContainsKey(IoPair.Key))
                                            {
                                                if (emplOldCompareCounters[IoPair.Key].ContainsKey((int)Constants.EmplCounterTypes.AnnualLeaveCounter))
                                                    row[2] = emplOldCompareCounters[IoPair.Key][(int)Constants.EmplCounterTypes.AnnualLeaveCounter].Value;

                                                if (emplOldCompareCounters[IoPair.Key].ContainsKey((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter))
                                                {
                                                    int previous_year = 0;
                                                    int current_year = 0;
                                                    int used = 0;
                                                    if (emplOldCompareCounters[IoPair.Key].ContainsKey((int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter))
                                                        previous_year = emplOldCompareCounters[IoPair.Key][(int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter].Value;

                                                    if (emplOldCompareCounters[IoPair.Key].ContainsKey((int)Constants.EmplCounterTypes.AnnualLeaveCounter))
                                                        current_year = emplOldCompareCounters[IoPair.Key][(int)Constants.EmplCounterTypes.AnnualLeaveCounter].Value;

                                                    if (emplOldCompareCounters[IoPair.Key].ContainsKey((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter))
                                                        used = emplOldCompareCounters[IoPair.Key][(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter].Value;


                                                    used += annual_leave_count;
                                                    int from_current = 0;
                                                    int from_previous = 0;
                                                    int current_left = 0;
                                                    int prev_left = previous_year - used;
                                                  
                                                    int previous_left = 0;

                                                    int iopairused = previous_year - used;
                                                    if (iopairused < 0)
                                                    {
                                                        from_previous = iopairused + annual_leave_count;
                                                        if (from_previous < 0)
                                                        {
                                                            from_previous = 0;
                                                            from_current = annual_leave_count;
                                                        }
                                                        else
                                                            from_current = Math.Abs(iopairused);

                                                        previous_left = 0;
                                                        current_left = current_year + iopairused;
                                                    }
                                                    else
                                                    {
                                                        current_left = current_year;
                                                        previous_left = iopairused;
                                                        int count = iopairused - annual_leave_count;
                                                        //if (count < 0)
                                                        //{
                                                        //    if (Math.Abs(count) > annual_leave_count)
                                                        //    {
                                                        //        from_previous = 0;
                                                        //        from_current = annual_leave_count;
                                                        //    }
                                                        //    else
                                                        //    {
                                                        from_previous = annual_leave_count;
                                                        from_current = annual_leave_count - from_previous;
                                                        //    }

                                                        //}
                                                        //else
                                                        //{
                                                        //from_previous = annual_leave_count;
                                                        //from_current = 0;

                                                        //}
                                                    }
                                                    row[6] = from_previous;
                                                    row[7] = from_current;

                                                    row[8] = previous_left + current_left;
                                                    row[9] = previous_left;
                                                    row[10] = current_left;

                                                }
                                            }
                                            row[3] = annual_leave_count;
                                            row[4] = IoPair.Value[0].IOPairDate.ToString("dd.MM.yyyy.");
                                            row[5] = IoPair.Value[IoPair.Value.Count - 1].IOPairDate.ToString("dd.MM.yyyy.");


                                            if (ascoDict.ContainsKey(IoPair.Key))
                                            {
                                                row[11] = ascoDict[IoPair.Key].DatetimeValue5.ToString("dd.MM.yyyy.");
                                                if (!ascoDict[IoPair.Key].NVarcharValue10.Equals("")) row[12] = ascoDict[IoPair.Key].NVarcharValue10;
                                                else row[12] = "      ";
                                            }


                                            DateTime offsetDate = new DateTime();
                                            if (rulesList.Count > 0)
                                            {

                                                if (DateTime.Now.Date <= IoPair.Value[0].IOPairDate.Date)
                                                {
                                                    if (IoPair.Value[0].IOPairDate.AddDays(-rulesList[0].RuleValue).Date < DateTime.Now.Date)
                                                        offsetDate = DateTime.Now;
                                                    else
                                                        offsetDate = IoPair.Value[0].IOPairDate.AddDays(-rulesList[0].RuleValue);
                                                }
                                                else
                                                {

                                                    offsetDate = IoPair.Value[0].IOPairDate.AddDays(-rulesList[0].RuleValue);
                                                }
                                            }
                                            else
                                            {
                                                if (DateTime.Now < IoPair.Value[0].IOPairDate)
                                                    offsetDate = DateTime.Now;
                                                else offsetDate = IoPair.Value[0].IOPairDate;
                                            }
                                            row[13] = offsetDate.ToString("dd.MM.yyyy");
                                           
                                            if (ascoDict.ContainsKey(IoPair.Key))
                                            {
                                                int positionID = ascoDict[IoPair.Key].IntegerValue6;
                                                if (positionID != -1)
                                                {
                                                    EmployeePosition posit = new EmployeePosition();
                                                    posit.EmplPositionTO.PositionID = positionID;
                                                    List<EmployeePositionTO> position = posit.SearchEmployeePositions();
                                                    row[14] = position[0].PositionCodeTitleSR;
                                                }
                                                else
                                                {
                                                    row[14] = "/";
                                                }
                                            }

                                            table.Rows.Add(row);

                                        }

                                    }
                                    ClearPageSessionValues();
                                    ClearSessionValues();
                                    if (table.Rows.Count > 0)
                                    {
                                        string costumer = Common.Misc.getCustomer(Session[Constants.sessionConnection]);
                                        int cost = 0;
                                        bool costum = int.TryParse(costumer, out cost);
                                        bool isHyatt = (cost == (int)Constants.Customers.Hyatt);
                                        bool isPMC = (cost == (int)Constants.Customers.PMC);
                                        bool isGrundfos = (cost == (int)Constants.Customers.Grundfos);

                                        Session["TLAnnualLeavePage.ds"] = ds;

                                        string wOptions = "dialogWidth:800px; dialogHeight:600px; edge:Raised; center:Yes; resizable:Yes; status:Yes;";
                                        if (isPMC)
                                            ClientScript.RegisterStartupScript(GetType(), "infoPopup", "window.open('/ACTAWeb/ReportsWeb/TLAnnualLeaveReport.aspx', window, '" + wOptions.Trim() + "');", true);
                                        else if (isGrundfos)
                                            ClientScript.RegisterStartupScript(GetType(), "infoPopup", "window.open('/ACTAWeb/ReportsWeb/TLAnnualLeaveReportGrundfos.aspx', window, '" + wOptions.Trim() + "');", true);
                                        else if (isHyatt)
                                            ClientScript.RegisterStartupScript(GetType(), "infoPopup", "window.open('/ACTAWeb/ReportsWeb/TLAnnualLeaveReportHyatt.aspx', window, '" + wOptions.Trim() + "');", true);
                                        else
                                            ClientScript.RegisterStartupScript(GetType(), "infoPopup", "window.open('/ACTAWeb/ReportsWeb/TLAnnualLeaveReportHutchinsonStalni.aspx', window, '" + wOptions.Trim() + "');", true);

                                    }
                                }
                            }
                            else
                            {
                                if (pair.GetTransaction() != null)
                                    pair.RollbackTransaction();
                                lblError.Text = rm.GetString("pairsSavingFaild", culture);
                            }
                        }
                        catch
                        {
                            if (pair.GetTransaction() != null)
                                pair.RollbackTransaction();
                            lblError.Text = rm.GetString("pairsSavingFaild", culture);
                        }
                    }
                    else
                        lblError.Text = rm.GetString("pairsSavingFaild", culture);
                }
                else
                {
                    lblError.Text = rm.GetString("noPairsForSaving", culture);
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLAnnualLeavePage.btnSave_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLAnnualLeavePage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private List<object> createResultRow(EmployeeTO empl, IOPairProcessedTO pair)
        {
            try
            {
                // create result row
                List<object> resultRow = new List<object>();
                int totalMinutes = 0;
                if (pair.EndTime.Hour == 23 && pair.EndTime.Minute == 59)
                    totalMinutes = (int)pair.EndTime.AddMinutes(1).Subtract(pair.StartTime).TotalMinutes;
                else
                    totalMinutes = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;
                int hours = totalMinutes / 60;
                int minutes = totalMinutes % 60;
                resultRow.Add(empl.EmployeeID);
                resultRow.Add(empl.FirstAndLastName.Trim());
                resultRow.Add(pair.PTDesc.Trim());
                resultRow.Add(pair.IOPairDate.Date);
                resultRow.Add(pair.StartTime);
                resultRow.Add(pair.EndTime);

                resultRow.Add(hours.ToString().Trim() + "h" + minutes.ToString().Trim() + "min");

                return resultRow;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string validatePair(IOPairProcessedTO pair, int minimalPresence, Dictionary<string, RuleTO> rules)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLAnnualLeavePage).Assembly);

                if (pair.StartTime >= pair.EndTime)
                    return rm.GetString("pairInvalidDuration", culture);

                if (rules.ContainsKey(Constants.RuleCompanyRegularWork) && pair.PassTypeID == rules[Constants.RuleCompanyRegularWork].RuleValue)
                {
                    // validate duration due to minimal presence
                    int pairDuration = ((int)pair.EndTime.TimeOfDay.TotalMinutes - (int)pair.StartTime.TimeOfDay.TotalMinutes);

                    // if it is last pair from first night shift interval, add one minute
                    if (pair.EndTime.Hour == 23 && pair.EndTime.Minute == 59)
                        pairDuration++;

                    if (pairDuration < minimalPresence)
                        return rm.GetString("pairLessThenMinimum", culture);
                }

                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void rbYes_CheckedChanged(Object sender, EventArgs e)
        {
            try
            {
                rbNo.Checked = !rbYes.Checked;
                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLAnnualLeavePage.rbYes_CheckedChange(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLAnnualLeavePage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void rbNo_CheckedChanged(Object sender, EventArgs e)
        {
            try
            {
                rbYes.Checked = !rbNo.Checked;
                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLAnnualLeavePage.rbNo_CheckedChange(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLAnnualLeavePage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private bool pairInInterval(IOPairProcessedTO pair, WorkTimeIntervalTO interval, WorkTimeSchemaTO schema)
        {
            try
            {
                if (schema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()))
                {
                    if (pair.StartTime.TimeOfDay >= interval.EarliestArrived.TimeOfDay && pair.EndTime.TimeOfDay <= interval.LatestLeft.TimeOfDay
                        && pair.EndTime.Subtract(pair.StartTime).TotalMinutes <= interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes)
                        return true;
                    else
                        return false;
                }
                else if (pair.StartTime.TimeOfDay >= interval.StartTime.TimeOfDay && pair.EndTime.TimeOfDay <= interval.EndTime.TimeOfDay)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void removeIntervalPairs(ref Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplOldPairs, int emplID, DateTime date, WorkTimeIntervalTO interval, WorkTimeSchemaTO schema)
        {
            try
            {
                if (emplOldPairs.ContainsKey(emplID) && emplOldPairs[emplID].ContainsKey(date.Date))
                {
                    IEnumerator<IOPairProcessedTO> pairEnumerator = emplOldPairs[emplID][date.Date].GetEnumerator();

                    while (pairEnumerator.MoveNext())
                    {
                        if (pairInInterval(pairEnumerator.Current, interval, schema))
                        {
                            emplOldPairs[emplID][date.Date].Remove(pairEnumerator.Current);
                            pairEnumerator = emplOldPairs[emplID][date.Date].GetEnumerator();
                        }
                    }
                }
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

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "TLAnnualLeavePage.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "TLAnnualLeavePage.", filterState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IOPairProcessedTO createPair(int emplID, PassTypeTO pt, DateTime date, DateTime start, DateTime end)
        {
            try
            {
                IOPairProcessedTO pair = new IOPairProcessedTO();
                pair.EmployeeID = emplID;
                pair.IOPairDate = date.Date;
                pair.StartTime = start;
                pair.EndTime = end;
                pair.IsWrkHrsCounter = (int)Constants.IsWrkCount.IsCounter;
                pair.LocationID = Constants.locationOut;
                pair.Alert = Constants.alertStatusNoAlert.ToString();
                pair.ManualCreated = (int)Constants.recordCreated.Manualy;
                pair.CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                pair.PassTypeID = pt.PassTypeID;
                pair.ConfirmationFlag = pt.ConfirmFlag;
                // TL and HRSSC automatically verify pairs for other employees - unjustified is verified by default, other not verified
                if (CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], emplID))
                {
                    if (pt.VerificationFlag == (int)Constants.Verification.NotVerified)
                    {
                        pair.VerificationFlag = (int)Constants.Verification.Verified;
                        pair.VerifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                        pair.VerifiedTime = DateTime.Now;
                    }
                    else
                        pair.VerificationFlag = (int)Constants.Verification.Verified;
                }
                else
                    pair.VerificationFlag = CommonWeb.Misc.verificationFlag(pt, false, false);

                if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                    pair.PTDesc = pt.Description.Trim();
                else
                    pair.PTDesc = pt.DescAlt.Trim();

                pair.IOPairID = Constants.unjustifiedIOPairID;

                return pair;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Dictionary<int, List<DateTime>> employeeHolidays(ref Dictionary<int, List<DateTime>> personalHolidays, DateTime startTime, DateTime endTime, string emplIDs, Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules,
            Dictionary<int, WorkTimeSchemaTO> schemas, Dictionary<int, EmployeeTO> employees, Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDict)
        {
            try
            {
                Dictionary<int, List<DateTime>> emplHolidays = new Dictionary<int, List<DateTime>>();

                string holidayType = "";

                // check if there are personal holidays for selected perios and selected employees, no transfering holidays for personal holidays                
                List<EmployeeAsco4TO> ascoList = new EmployeeAsco4(Session[Constants.sessionConnection]).Search(emplIDs);
                List<DateTime> nationalHolidaysDays = new List<DateTime>();
                List<DateTime> nationalHolidaysSundays = new List<DateTime>();
                Dictionary<string, List<DateTime>> personalHolidayDays = new Dictionary<string, List<DateTime>>();
                List<HolidaysExtendedTO> nationalTransferableHolidays = new List<HolidaysExtendedTO>();

                Common.Misc.getHolidays(startTime, endTime, personalHolidayDays, nationalHolidaysDays, nationalHolidaysSundays, Session[Constants.sessionConnection], nationalTransferableHolidays);

                // get personal holidays in selected period
                foreach (EmployeeAsco4TO asco in ascoList)
                {
                    // expat out does not have holidays
                    if (employees.ContainsKey(asco.EmployeeID) && Common.Misc.isExpatOut(rulesDict, employees[asco.EmployeeID]))
                        continue;

                    holidayType = asco.NVarcharValue1.Trim();

                    if (!holidayType.Trim().Equals(""))
                    {
                        // if category is IV, find holiday date
                        if (holidayType.Trim().Equals(Constants.holidayTypeIV.Trim()))
                        {
                            if (!emplHolidays.ContainsKey(asco.EmployeeID))
                                emplHolidays.Add(asco.EmployeeID, new List<DateTime>());

                            if (!personalHolidays.ContainsKey(asco.EmployeeID))
                                personalHolidays.Add(asco.EmployeeID, new List<DateTime>());

                            DateTime personalHolidayStartDate = new DateTime(startTime.Year, asco.DatetimeValue1.Month, asco.DatetimeValue1.Day, 0, 0, 0);
                            DateTime personalHolidayEndDate = new DateTime(endTime.Year, asco.DatetimeValue1.Month, asco.DatetimeValue1.Day, 0, 0, 0);
                            if (startTime.Date <= personalHolidayStartDate.Date && endTime.Date >= personalHolidayStartDate.Date)
                            {
                                emplHolidays[asco.EmployeeID].Add(personalHolidayStartDate.Date);
                                personalHolidays[asco.EmployeeID].Add(personalHolidayStartDate.Date);
                            }
                            else if (startTime.Date <= personalHolidayEndDate.Date && endTime.Date >= personalHolidayEndDate.Date)
                            {
                                emplHolidays[asco.EmployeeID].Add(personalHolidayEndDate);
                                personalHolidays[asco.EmployeeID].Add(personalHolidayEndDate);
                            }
                        }
                        else
                        {
                            // if category is I, II or III, check if pair date is personal holiday
                            if (personalHolidayDays.ContainsKey(holidayType.Trim()))
                            {
                                if (!emplHolidays.ContainsKey(asco.EmployeeID))
                                    emplHolidays.Add(asco.EmployeeID, new List<DateTime>());

                                if (!personalHolidays.ContainsKey(asco.EmployeeID))
                                    personalHolidays.Add(asco.EmployeeID, new List<DateTime>());

                                emplHolidays[asco.EmployeeID].AddRange(personalHolidayDays[holidayType.Trim()]);
                                personalHolidays[asco.EmployeeID].AddRange(personalHolidayDays[holidayType.Trim()]);
                            }
                        }
                    }
                }

                // add national holidays to all selected employees, industrial time schemas do not transfer holidays
                string[] IDs = emplIDs.Split(',');
                foreach (string emplID in IDs)
                {
                    int id = -1;
                    if (int.TryParse(emplID.Trim(), out id) && id != -1)
                    {
                        // expat out does not have holidays
                        if (employees.ContainsKey(id) && Common.Misc.isExpatOut(rulesDict, employees[id]))
                            continue;

                        foreach (DateTime natHoliday in nationalHolidaysDays)
                        {
                            if (!emplHolidays.ContainsKey(id))
                                emplHolidays.Add(id, new List<DateTime>());

                            emplHolidays[id].Add(natHoliday.Date);
                        }

                        // add transfered holidays to employees who does not have industrial schema for that day
                        foreach (DateTime natHolidaySunday in nationalHolidaysSundays)
                        {
                            bool isIndustrial = false;

                            if (emplSchedules.ContainsKey(id) && Common.Misc.getTimeSchema(natHolidaySunday.Date, emplSchedules[id], schemas).Type.Trim().ToUpper().Equals(Constants.schemaTypeIndustrial.Trim().ToUpper()))
                                isIndustrial = true;

                            if (!isIndustrial)
                            {
                                if (!emplHolidays.ContainsKey(id))
                                    emplHolidays.Add(id, new List<DateTime>());

                                emplHolidays[id].Add(natHolidaySunday.Date);
                            }
                        }
                    }
                }

                return emplHolidays;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
