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
    public partial class HRSSCConfirmationPage : System.Web.UI.Page
    {
        const string sessionConfirmationPairs = "HRSSCConfirmationPage.ConfirmationPairs";

        const string pageName = "HRSSCConfirmationPage";

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
                // clear day pairs and employee counters so bars and counters could be presented properly
                ClearSessionValues();

                Dictionary<int, WorkingUnitTO> wUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();

                // if user do not want to reload selected filter and get results for that filter, parameter reloadState should be passes through url and set to false
                if (!IsPostBack)
                {
                    Session[sessionConfirmationPairs] = null;

                    btnFromDate.Attributes.Add("onclick", "return calendarPicker('tbFromDate', 'true');");
                    btnToDate.Attributes.Add("onclick", "return calendarPicker('tbToDate', 'true');");
                    btnWUTree.Attributes.Add("onclick", "return wUnitsPicker('btnWUTree', 'false');");
                    btnOrgTree.Attributes.Add("onclick", "return oUnitsPicker('btnOrgTree', 'false');");
                    cbSelectPassTypes.Attributes.Add("onclick", "return selectListItems('cbSelectPassTypes', 'lbPassTypes');");
                    cbSelectAllEmpolyees.Attributes.Add("onclick", "return selectListItems('cbSelectAllEmpolyees', 'lboxEmployees');");
                    tbEmployee.Attributes.Add("onKeyUp", "return selectItem('tbEmployee', 'lboxEmployees');");
                    btnShow.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnConfirmAll.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
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

                    cbSelectAllEmpolyees.Visible = cbSelectPassTypes.Visible = false;

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
                    ruleTo.RuleType = Constants.RulePrintPaidLeaveReport;
                    rule.RuleTO = ruleTo;
                    List<RuleTO> rules = rule.Search();
                    if (rules.Count > 0 && rules[0].RuleValue == 1)
                    {
                        btnPrint.Visible = true;
                        chbPaidLeave60.Visible = true;
                    }
                    else
                    {
                        btnPrint.Visible = false;
                        chbPaidLeave60.Visible = false;
                    }

                    cbConfirmationPassType.Enabled = tbDescription.Enabled = btnConfirmAll.Enabled = false;

                    if (Request.QueryString["reloadState"] != null && Request.QueryString["reloadState"].Trim().ToUpper().Equals(Constants.falseValue.Trim().ToUpper()))
                    {
                        if (Menu1.SelectedItem.Value.Equals("0"))
                        {
                            int wuID = -1;
                            if (tbUte.Attributes["id"] != null)
                                if (!int.TryParse(tbUte.Attributes["id"].Trim(), out wuID))
                                    wuID = -1;
                            populateEmployees(wuID, true);
                            populatePassTypes(Common.Misc.getRootWorkingUnit(wuID, wUnits));
                        }
                        else
                        {
                            int ouID = -1;
                            if (tbOrgUte.Attributes["id"] != null)
                                if (!int.TryParse(tbOrgUte.Attributes["id"].Trim(), out ouID))
                                    ouID = -1;
                            populateEmployees(ouID, false);
                            if (ouID >= 0)
                            {
                                WorkingUnitXOrganizationalUnit wuXou = new WorkingUnitXOrganizationalUnit(Session[Constants.sessionConnection]);
                                wuXou.WUXouTO.OrgUnitID = ouID;
                                List<WorkingUnitXOrganizationalUnitTO> list = wuXou.Search();
                                if (list.Count > 0)
                                {
                                    WorkingUnitXOrganizationalUnitTO wuXouTO = list[0];
                                    populatePassTypes(Common.Misc.getRootWorkingUnit(wuXouTO.WorkingUnitID, wUnits));
                                }
                            }
                            else
                                populatePassTypes(-1);
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
                            populatePassTypes(Common.Misc.getRootWorkingUnit(wuID, wUnits));
                        }
                        else
                        {
                            int ouID = -1;
                            if (tbOrgUte.Attributes["id"] != null)
                                if (!int.TryParse(tbOrgUte.Attributes["id"].Trim(), out ouID))
                                    ouID = -1;
                            populateEmployees(ouID, false);
                            if (ouID >= 0)
                            {
                                WorkingUnitXOrganizationalUnit wuXou = new WorkingUnitXOrganizationalUnit(Session[Constants.sessionConnection]);
                                wuXou.WUXouTO.OrgUnitID = ouID;
                                List<WorkingUnitXOrganizationalUnitTO> list = wuXou.Search();
                                if (list.Count > 0)
                                {
                                    WorkingUnitXOrganizationalUnitTO wuXouTO = list[0];
                                    populatePassTypes(Common.Misc.getRootWorkingUnit(wuXouTO.WorkingUnitID, wUnits));
                                }
                            }
                            else
                                populatePassTypes(-1);
                        }

                        // do again load state to select previously selected employees in employees list
                        LoadState();
                    }
                }
                else
                {
                    if (Request["__EVENTARGUMENT"] != null && Request["__EVENTARGUMENT"].Equals(Constants.pairsSavedArg))
                    {
                        InitializeGraphData();
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
                                populatePassTypes(Common.Misc.getRootWorkingUnit(wuID, wUnits));
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
                                if (ouID >= 0)
                                {
                                    WorkingUnitXOrganizationalUnit wuXou = new WorkingUnitXOrganizationalUnit(Session[Constants.sessionConnection]);
                                    wuXou.WUXouTO.OrgUnitID = ouID;
                                    List<WorkingUnitXOrganizationalUnitTO> list = wuXou.Search();
                                    if (list.Count > 0)
                                    {
                                        WorkingUnitXOrganizationalUnitTO wuXouTO = list[0];
                                        populatePassTypes(Common.Misc.getRootWorkingUnit(wuXouTO.WorkingUnitID, wUnits));
                                    }
                                    else
                                        populatePassTypes(-1);
                                }
                                else
                                    populatePassTypes(-1);
                            }
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in HRSSCConfirmationPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/HRSSCConfirmationPage.aspx&Header=" + Constants.falseValue.Trim(), false);
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in HRSSCConfirmationPage.Date_Changed(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/HRSSCConfirmationPage.aspx&Header=" + Constants.falseValue.Trim(), false);
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(HRSSCConfirmationPage).Assembly);

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
                    tbFromDate.Focus();
                    return;
                }

                if (to.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidToDate", culture);
                    tbToDate.Focus();
                    return;
                }

                if (from > to)
                {
                    lblError.Text = rm.GetString("invalidFromToDate", culture);
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in HRSSCConfirmationPage.cbEmplType_SelectedIndexChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/HRSSCConfirmationPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnPrevDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(HRSSCConfirmationPage).Assembly);

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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in HRSSCConfirmationPage.btnPrevDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/HRSSCConfirmationPage.aspx&Header=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnNextDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(HRSSCConfirmationPage).Assembly);

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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in HRSSCConfirmationPage.btnNextDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/HRSSCConfirmationPage.aspx&Header=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
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

                Dictionary<int, WorkingUnitTO> wUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                populateEmplTypes();
                // value 0 - WU tab, value 1 - OU tab
                if (e.Item.Value.Equals("0"))
                {
                    int wuID = -1;
                    if (tbUte.Attributes["id"] != null)
                        if (!int.TryParse(tbUte.Attributes["id"].Trim(), out wuID))
                            wuID = -1;
                    populateEmployees(wuID, true);
                    populatePassTypes(Common.Misc.getRootWorkingUnit(wuID, wUnits));
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
                    if (ouID >= 0)
                    {
                        WorkingUnitXOrganizationalUnit wuXou = new WorkingUnitXOrganizationalUnit(Session[Constants.sessionConnection]);
                        wuXou.WUXouTO.OrgUnitID = ouID;
                        List<WorkingUnitXOrganizationalUnitTO> list = wuXou.Search();
                        if (list.Count > 0)
                        {
                            WorkingUnitXOrganizationalUnitTO wuXouTO = list[0];
                            populatePassTypes(Common.Misc.getRootWorkingUnit(wuXouTO.WorkingUnitID, wUnits));
                        }
                    }
                    else
                        populatePassTypes(-1);
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in HRSSCConfirmationPage.Menu1_MenuItemClick(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/HRSSCConfirmationPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void populatePassTypes(int company)
        {
            try
            {
                List<PassTypeTO> passTypeList = new List<PassTypeTO>();

                if (company != -1)
                {
                    bool isAltLang = !CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper());
                    List<PassTypeTO> companyPassTypeList = new PassType(Session[Constants.sessionConnection]).SearchForCompany(company, isAltLang);

                    foreach (PassTypeTO pt in companyPassTypeList)
                    {
                        if (pt.ConfirmFlag == (int)Constants.Confirmation.NotConfirmed)
                            passTypeList.Add(pt);
                    }
                }

                lbPassTypes.DataSource = passTypeList;

                if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                {
                    lbPassTypes.DataTextField = "DescriptionAndID";
                }
                else
                {
                    lbPassTypes.DataTextField = "DescriptionAltAndID";
                }
                lbPassTypes.DataValueField = "PassTypeID";
                lbPassTypes.DataBind();

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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(HRSSCConfirmationPage).Assembly);

                lblEmployee.Text = rm.GetString("lblEmployeeSelection", culture);
                lblPeriod.Text = rm.GetString("lblPeriod", culture);
                lblPassTypes.Text = rm.GetString("lblPassType", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblConfirmationPassTypes.Text = rm.GetString("lblConfirmationPassTypes", culture);
                lblDescription.Text = rm.GetString("lblDesc", culture);                
                lblEmplType.Text = rm.GetString("lblEmployeeType", culture);

                cbSelectAllEmpolyees.Text = rm.GetString("lblSelectAll", culture);
                cbSelectPassTypes.Text = rm.GetString("lblSelectAll", culture);
                chbPaidLeave60.Text = rm.GetString("chbPaidLeave60", culture);

                btnShow.Text = rm.GetString("btnShow", culture);
                btnConfirmAll.Text = rm.GetString("btnConfirmAll", culture);
                btnPrint.Text = rm.GetString("btnPrintPaidReport", culture);
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
                lblError.Text = "";
                Session[sessionConfirmationPairs] = null;

                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(HRSSCConfirmationPage).Assembly);

                // check if there are some employees in list
                if (lboxEmployees.Items.Count <= 0)
                {
                    lblError.Text = rm.GetString("noEmployeesForTMData", culture);
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

                Session[Constants.sessionFromDate] = fromDate;
                Session[Constants.sessionToDate] = toDate;

                if (lboxEmployees.GetSelectedIndices().Length > 0)
                {
                    List<string> idList = new List<string>();
                    foreach (int emplIndex in lboxEmployees.GetSelectedIndices())
                    {
                        if (emplIndex >= 0 && emplIndex < lboxEmployees.Items.Count)
                            idList.Add(lboxEmployees.Items[emplIndex].Value.Trim());
                    }
                    Session[Constants.sessionSelectedEmplIDs] = idList;
                }

                SaveState();

                InitializeGraphData();

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in HRSSCConfirmationPage.btnShow_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/HRSSCConfirmationPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void InitializeGraphData()
        {
            try
            {
                EmployeeTO Empl = new EmployeeTO();

                if (Session[Constants.sessionLogInEmployee] != null && Session[Constants.sessionLogInEmployee] is EmployeeTO)
                    Empl = (EmployeeTO)Session[Constants.sessionLogInEmployee];

                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLTMDataPage).Assembly);

                Dictionary<int, PassTypeTO> PassTypes = getTypes();
                Dictionary<int, WorkingUnitTO> WUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                Dictionary<int, WorkTimeSchemaTO> schemas = new TimeSchema(Session[Constants.sessionConnection]).getDictionary();

                // set legend control
                foreach (Control ctrl in legendCtrlHolder.Controls)
                {
                    ctrl.Dispose();
                }

                legendCtrlHolder.Controls.Clear();

                int company = -1;
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
                    }
                }

                LegendUC legendCtrl = new LegendUC();
                legendCtrl.ID = "legend";
                legendCtrl.Company = company;

                legendCtrlHolder.Controls.Add(legendCtrl);

                // set hour line control
                foreach (Control ctrl in hourLineCtrlHolder.Controls)
                {
                    ctrl.Dispose();
                }

                hourLineCtrlHolder.Controls.Clear();

                // find night hour rule
                RuleTO nightWorkRule = new RuleTO();

                if (Empl.EmployeeID != -1)
                {
                    int logInCompany = Common.Misc.getRootWorkingUnit(Empl.WorkingUnitID, WUnits);

                    if (logInCompany != -1)
                    {
                        Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                        rule.RuleTO.EmployeeTypeID = Empl.EmployeeTypeID;
                        rule.RuleTO.WorkingUnitID = logInCompany;
                        rule.RuleTO.RuleType = Constants.RuleNightWork;
                        List<RuleTO> rulesList = rule.Search();

                        if (rulesList.Count == 1)
                            nightWorkRule = rulesList[0];
                    }
                }

                HoursLineControlUC hourLine = new HoursLineControlUC();
                hourLine.ID = "hourLine";
                hourLine.NightWorkRule = nightWorkRule;

                hourLine.FirstLblText = rm.GetString("lblEmployee", culture);
                hourLine.SecondLblText = rm.GetString("lblTotal", culture);
                hourLineCtrlHolder.Controls.Add(hourLine);

                DateTime fromDate = CommonWeb.Misc.createDate(tbFromDate.Text.Trim());
                DateTime toDate = CommonWeb.Misc.createDate(tbToDate.Text.Trim());

                if (!fromDate.Equals(new DateTime()) && !toDate.Equals(new DateTime()) && fromDate.Date <= toDate.Date && lboxEmployees.Items.Count > 0)
                {
                    List<DateTime> dateList = new List<DateTime>();
                    DateTime day = fromDate;
                    while (day <= toDate.Date.AddDays(1))
                    {
                        dateList.Add(day.Date);
                        day = day.Date.AddDays(1);
                    }

                    if (dateList.Count > 0)
                    {
                        string emplIDs = "";
                        if (lboxEmployees.GetSelectedIndices().Length > 0)
                        {
                            foreach (int emplIndex in lboxEmployees.GetSelectedIndices())
                            {
                                if (emplIndex >= 0 && emplIndex < lboxEmployees.Items.Count)
                                {
                                    emplIDs += lboxEmployees.Items[emplIndex].Value.Trim() + ",";
                                }
                            }

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

                        string ptIDs = "";
                        if (lbPassTypes.GetSelectedIndices().Length > 0)
                        {
                            foreach (int ptIndex in lbPassTypes.GetSelectedIndices())
                            {
                                if (ptIndex >= 0 && ptIndex < lbPassTypes.Items.Count)
                                {
                                    ptIDs += lbPassTypes.Items[ptIndex].Value.Trim() + ",";
                                }
                            }

                            if (ptIDs.Length > 0)
                                ptIDs = ptIDs.Substring(0, ptIDs.Length - 1);
                        }

                        List<EmployeeTO> emplList = new Employee(Session[Constants.sessionConnection]).Search(emplIDs);
                        Dictionary<int, EmployeeAsco4TO> emplAdditionalData = new EmployeeAsco4(Session[Constants.sessionConnection]).SearchDictionary(emplIDs);

                        Dictionary<int, EmployeeTO> employees = new Dictionary<int, EmployeeTO>();

                        foreach (EmployeeTO empl in emplList)
                        {
                            if (!employees.ContainsKey(empl.EmployeeID))
                                employees.Add(empl.EmployeeID, empl);
                        }

                        List<IOPairProcessedTO> IOPairList = FindIOPairsForEmployee(emplIDs, dateList, ptIDs, schemas, PassTypes, employees);

                        DrawGraphControl(dateList, IOPairList, PassTypes, WUnits, Empl, emplList, schemas, emplAdditionalData, emplIDs);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<IOPairProcessedTO> FindIOPairsForEmployee(string emplIDs, List<DateTime> dateList, string ptIDs, Dictionary<int, WorkTimeSchemaTO> schemas,
            Dictionary<int, PassTypeTO> passTypesAll, Dictionary<int, EmployeeTO> employees)
        {
            try
            {
                List<IOPairProcessedTO> pairs = new IOPairProcessed(Session[Constants.sessionConnection]).SearchAllPairsForEmpl(emplIDs, dateList, ptIDs);

                List<IOPairProcessedTO> allPairs = new IOPairProcessed(Session[Constants.sessionConnection]).SearchAllPairsForEmpl(emplIDs, dateList, "");

                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplDayPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                Dictionary<int, Dictionary<DateTime, WorkTimeSchemaTO>> emplDaySchemas = new Dictionary<int, Dictionary<DateTime, WorkTimeSchemaTO>>();
                Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>> emplDayIntervals = new Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>>();

                // get time schedules and time schemas for selected employees for selected days, add one day for third shifts                        
                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule(Session[Constants.sessionConnection]).SearchEmployeesSchedulesExactDate(emplIDs, dateList[0].Date, dateList[dateList.Count - 1].AddDays(1).Date, null);

                // get intervals and schemas by employees and dates
                foreach (int emplID in employees.Keys)
                {
                    DateTime currDate = dateList[0].Date;
                    List<EmployeeTimeScheduleTO> emplScheduleList = new List<EmployeeTimeScheduleTO>();

                    if (emplSchedules.ContainsKey(emplID))
                        emplScheduleList = emplSchedules[emplID];

                    while (currDate <= dateList[dateList.Count - 1].AddDays(1))
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
                }

                // get day pairs for employees
                foreach (IOPairProcessedTO pair in allPairs)
                {
                    if (!emplDayPairs.ContainsKey(pair.EmployeeID))
                        emplDayPairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());

                    if (!emplDayPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                        emplDayPairs[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<IOPairProcessedTO>());

                    emplDayPairs[pair.EmployeeID][pair.IOPairDate.Date].Add(pair);
                }

                if (dateList.Count > 0)
                {
                    IEnumerator<IOPairProcessedTO> pairEnumerator = pairs.GetEnumerator();
                    while (pairEnumerator.MoveNext())
                    {
                        // skip all pairs from first day that belong to previous day and add pairs from day after last that belong to last day
                        if (pairEnumerator.Current.IOPairDate.Date.Equals(dateList[dateList.Count - 1].Date) || pairEnumerator.Current.IOPairDate.Date.Equals(dateList[0].Date))
                        {
                            List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                            if (emplDayPairs.ContainsKey(pairEnumerator.Current.EmployeeID) && emplDayPairs[pairEnumerator.Current.EmployeeID].ContainsKey(pairEnumerator.Current.IOPairDate.Date))
                                dayPairs = emplDayPairs[pairEnumerator.Current.EmployeeID][pairEnumerator.Current.IOPairDate.Date];

                            WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                            if (emplDaySchemas.ContainsKey(pairEnumerator.Current.EmployeeID) && emplDaySchemas[pairEnumerator.Current.EmployeeID].ContainsKey(pairEnumerator.Current.IOPairDate.Date))
                                sch = emplDaySchemas[pairEnumerator.Current.EmployeeID][pairEnumerator.Current.IOPairDate.Date];

                            List<WorkTimeIntervalTO> dayIntervals = new List<WorkTimeIntervalTO>();
                            if (emplDayIntervals.ContainsKey(pairEnumerator.Current.EmployeeID) && emplDayIntervals[pairEnumerator.Current.EmployeeID].ContainsKey(pairEnumerator.Current.IOPairDate.Date))
                                dayIntervals = emplDayIntervals[pairEnumerator.Current.EmployeeID][pairEnumerator.Current.IOPairDate.Date];

                            bool previousDayPair = CommonWeb.Misc.isPreviousDayPair(pairEnumerator.Current, passTypesAll, dayPairs, sch, dayIntervals);

                            if ((pairEnumerator.Current.IOPairDate.Date.Equals(dateList[0].Date) && previousDayPair)
                                || (pairEnumerator.Current.IOPairDate.Date.Equals(dateList[dateList.Count - 1].Date) && !previousDayPair))
                            {
                                pairs.Remove(pairEnumerator.Current);
                                pairEnumerator = pairs.GetEnumerator();
                            }
                        }
                    }
                }

                return pairs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DrawGraphControl(List<DateTime> dateList, List<IOPairProcessedTO> IOPairList, Dictionary<int, PassTypeTO> PassTypes, Dictionary<int, WorkingUnitTO> WUnits,
            EmployeeTO Empl, List<EmployeeTO> emplList, Dictionary<int, WorkTimeSchemaTO> schemas, Dictionary<int, EmployeeAsco4TO> emplAddData, string emplIDs)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(HRSSCConfirmationPage).Assembly);

                List<IOPairProcessedTO> pairsToConfirm = new List<IOPairProcessedTO>();

                //get HRSSC cuttoff date (this is page only for HRSSC) - Empl is logged in employee
                int cutoffDate = -1;
                if (Empl.EmployeeID != -1)
                {
                    int company = Common.Misc.getRootWorkingUnit(Empl.WorkingUnitID, WUnits);

                    if (company != -1)
                    {
                        Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                        rule.RuleTO.WorkingUnitID = company;
                        rule.RuleTO.EmployeeTypeID = Empl.EmployeeTypeID;
                        rule.RuleTO.RuleType = Constants.RuleHRSSCCutOffDate;

                        List<RuleTO> rules = rule.Search();

                        if (rules.Count == 1)
                        {
                            cutoffDate = rules[0].RuleValue;
                        }
                    }
                }

                // create dictionary with pairs for confirmation for specific employee for specific day - key is employeeID, value is dictionary of day and pairs for that day                    
                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplDayPairsToConfirm = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();

                foreach (IOPairProcessedTO pair in IOPairList)
                {
                    if (pair.ConfirmationFlag == (int)Constants.Confirmation.NotConfirmed)
                    {
                        if (!emplDayPairsToConfirm.ContainsKey(pair.EmployeeID))
                            emplDayPairsToConfirm.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());

                        if (!emplDayPairsToConfirm[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                            emplDayPairsToConfirm[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<IOPairProcessedTO>());

                        emplDayPairsToConfirm[pair.EmployeeID][pair.IOPairDate.Date].Add(pair);
                    }
                }

                // dispose and clear existing controls
                foreach (Control ctrl in ctrlHolder.Controls)
                {
                    ctrl.Dispose();
                }

                ctrlHolder.Controls.Clear();

                // draw pair bars for each selected employee and each day
                int currentIndex = 0;

                bool samePT = true;
                int ptID = -1;
                if (dateList.Count > 0)
                {
                    DateTime firstDay = dateList[0];
                    DateTime lastDay = dateList[dateList.Count - 1];

                    Color backColor = Color.White;
                    bool isAltCtrl = false;

                    Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> regularWorks = new Common.Rule(Session[Constants.sessionConnection]).SearchTypeAllRules(Constants.RuleCompanyRegularWork);

                    // get schedules for all employees
                    Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule(Session[Constants.sessionConnection]).SearchEmployeesSchedulesExactDate(emplIDs, firstDay.Date, lastDay.Date, null);

                    foreach (EmployeeTO employee in emplList)
                    {
                        if (employee.EmployeeID != -1)
                        {
                            string emplData = employee.LastName.Trim() + " " + employee.FirstName.Trim();

                            if (currentIndex % 2 != 0)
                            {
                                backColor = ColorTranslator.FromHtml(Constants.emplDayViewAltColor);
                                isAltCtrl = true;
                            }
                            else
                            {
                                backColor = Color.White;
                                isAltCtrl = false;
                            }

                            EmployeeWorkingDayViewUC emplDataCtrl = new EmployeeWorkingDayViewUC();
                            emplDataCtrl.ID = "emplDataView" + currentIndex.ToString();
                            emplDataCtrl.FirstData = emplData.Trim().ToUpper();
                            emplDataCtrl.BackColor = backColor;
                            emplDataCtrl.IsAltCtrl = isAltCtrl;
                            emplDataCtrl.IsHeader = true;
                            if (currentIndex == 0)
                                emplDataCtrl.IsFirst = true;

                            // get employee regular work
                            int emplCompany = Common.Misc.getRootWorkingUnit(employee.WorkingUnitID, WUnits);
                            int regularWorkID = -1;
                            if (emplCompany != -1 && regularWorks.ContainsKey(emplCompany) && regularWorks[emplCompany].ContainsKey(employee.EmployeeTypeID)
                                && regularWorks[emplCompany][employee.EmployeeTypeID].ContainsKey(Constants.RuleCompanyRegularWork))
                                regularWorkID = regularWorks[emplCompany][employee.EmployeeTypeID][Constants.RuleCompanyRegularWork].RuleValue;

                            List<EmployeeTimeScheduleTO> timeScheduleList = new List<EmployeeTimeScheduleTO>();
                            if (emplSchedules.ContainsKey(employee.EmployeeID))
                                timeScheduleList = emplSchedules[employee.EmployeeID];

                            // get employee hiring and termination dates
                            DateTime emplHiringDate = new DateTime();
                            DateTime emplTerminationDate = new DateTime();

                            if (emplAddData.ContainsKey(employee.EmployeeID))
                            {
                                emplHiringDate = emplAddData[employee.EmployeeID].DatetimeValue2;

                                if (employee.Status.Trim().ToUpper().Equals(Constants.statusRetired.Trim().ToUpper()))
                                    emplTerminationDate = emplAddData[employee.EmployeeID].DatetimeValue3;
                            }

                            DateTime currDay = new DateTime(firstDay.Year, firstDay.Month, firstDay.Day, 0, 0, 0);

                            bool headerAdded = false;
                            while (currDay.Date <= lastDay.Date)
                            {
                                List<IOPairProcessedTO> ioPairsForDayToConfirm = new List<IOPairProcessedTO>();

                                if (emplDayPairsToConfirm.ContainsKey(employee.EmployeeID) && emplDayPairsToConfirm[employee.EmployeeID].ContainsKey(currDay.Date))
                                    ioPairsForDayToConfirm = emplDayPairsToConfirm[employee.EmployeeID][currDay.Date];

                                if (ioPairsForDayToConfirm.Count <= 0)
                                {
                                    currDay = currDay.AddDays(1);
                                    continue;
                                }

                                if (!headerAdded)
                                {
                                    ctrlHolder.Controls.Add(emplDataCtrl);
                                    currentIndex++;
                                    headerAdded = true;
                                }

                                int hours = 0;
                                int min = 0;

                                foreach (IOPairProcessedTO iopair in ioPairsForDayToConfirm)
                                {
                                    if (!iopair.StartTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)) && !iopair.EndTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                                    {
                                        if (int.Parse(iopair.StartTime.ToString("mm")) == 0)
                                        {
                                            hours += int.Parse(iopair.EndTime.ToString("HH")) - int.Parse(iopair.StartTime.ToString("HH"));
                                        }
                                        else
                                        {
                                            hours += int.Parse(iopair.EndTime.ToString("HH")) - int.Parse(iopair.StartTime.ToString("HH")) - 1;
                                            min += 60 - int.Parse(iopair.StartTime.ToString("mm"));
                                        }

                                        min += int.Parse(iopair.EndTime.ToString("mm"));

                                        // add one minute for first interval of third shift
                                        if (iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59)
                                            min++;
                                    }

                                    while (min >= 60)
                                    {
                                        hours++;
                                        min -= 60;
                                    }
                                }

                                string timeString = ""; //text for summ time cell
                                if (hours == 0) { timeString += "00h"; }
                                else
                                {
                                    if (hours < 10) { timeString += "0"; }
                                    timeString += hours + "h";
                                }
                                if (min == 0) { timeString += "00min"; }
                                else
                                {
                                    if (min < 10) { timeString += "0"; }
                                    timeString += min + "min";
                                }

                                List<WorkTimeIntervalTO> timeSchemaIntervalList = Common.Misc.getTimeSchemaInterval(currDay.Date, timeScheduleList, schemas);

                                WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                                if (timeSchemaIntervalList.Count > 0 && schemas.ContainsKey(timeSchemaIntervalList[0].TimeSchemaID))
                                    schema = schemas[timeSchemaIntervalList[0].TimeSchemaID];

                                string day = "";
                                switch (currDay.DayOfWeek)
                                {
                                    case DayOfWeek.Monday:
                                        day = rm.GetString("mon", culture);
                                        break;
                                    case DayOfWeek.Tuesday:
                                        day = rm.GetString("tue", culture);
                                        break;
                                    case DayOfWeek.Wednesday:
                                        day = rm.GetString("wed", culture);
                                        break;
                                    case DayOfWeek.Thursday:
                                        day = rm.GetString("thu", culture);
                                        break;
                                    case DayOfWeek.Friday:
                                        day = rm.GetString("fri", culture);
                                        break;
                                    case DayOfWeek.Saturday:
                                        day = rm.GetString("sat", culture);
                                        break;
                                    case DayOfWeek.Sunday:
                                        day = rm.GetString("sun", culture);
                                        break;
                                    default:
                                        day = "";
                                        break;
                                }

                                if (currentIndex % 2 != 0)
                                {
                                    backColor = ColorTranslator.FromHtml(Constants.emplDayViewAltColor);
                                    isAltCtrl = true;
                                }
                                else
                                {
                                    backColor = Color.White;
                                    isAltCtrl = false;
                                }

                                EmployeeWorkingDayViewUC emplDay = new EmployeeWorkingDayViewUC();
                                emplDay.ID = "emplDayView" + currentIndex.ToString();
                                emplDay.DayPairList = ioPairsForDayToConfirm;
                                emplDay.DayIntervalList = timeSchemaIntervalList;
                                emplDay.PassTypes = PassTypes;
                                emplDay.WUnits = WUnits;
                                emplDay.Empl = employee;
                                emplDay.BackColor = backColor;
                                emplDay.Date = currDay;
                                emplDay.SecondData = timeString.Trim();
                                emplDay.FirstData = currDay.ToString(Constants.dateMonthFormat) + day;
                                if (currentIndex == 0)
                                    emplDay.IsFirst = true;
                                emplDay.IsAltCtrl = isAltCtrl;

                                if (!emplHiringDate.Equals(new DateTime()) && emplHiringDate.Date <= currDay.Date && (emplTerminationDate.Equals(new DateTime()) || emplTerminationDate >= currDay.Date))
                                {
                                    if (currDay.Date >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0))
                                        emplDay.AllowConfirm = true;
                                    else if (cutoffDate != -1 && Common.Misc.countWorkingDays(DateTime.Now.Date, Session[Constants.sessionConnection]) <= cutoffDate
                                        && currDay.Date >= new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1, 0, 0, 0))
                                        emplDay.AllowConfirm = true;
                                    else
                                        emplDay.AllowConfirm = false;

                                    if (emplDay.AllowConfirm)
                                    {
                                        foreach (IOPairProcessedTO pair in ioPairsForDayToConfirm)
                                        {
                                            if (Session[Constants.sessionLoginCategoryPassTypes] != null && Session[Constants.sessionLoginCategoryPassTypes] is List<int>
                                                && ((List<int>)Session[Constants.sessionLoginCategoryPassTypes]).Contains(pair.PassTypeID))
                                            {
                                                // check if it belongs to previous day
                                                pairsToConfirm.Add(pair);

                                                if (samePT)
                                                {
                                                    if (ptID == -1)
                                                        ptID = pair.PassTypeID;
                                                    else if (ptID != pair.PassTypeID)
                                                        samePT = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                    emplDay.AllowChange = emplDay.AllowConfirm = emplDay.AllowUndoVerify = emplDay.AllowVerify = false;

                                emplDay.EmplTimeSchema = schema;
                                emplDay.PostBackCtrlID = btnShow.ID;

                                ctrlHolder.Controls.Add(emplDay);

                                currDay = currDay.AddDays(1);
                                currentIndex++;
                            }
                        }
                    }

                    if (samePT && ptID != -1)
                    {
                        Session[sessionConfirmationPairs] = pairsToConfirm;

                        tbDescription.Text = "";
                        populateConfirmationPassTypes(ptID);

                        if (cbConfirmationPassType.Items.Count > 0)
                            cbConfirmationPassType.Enabled = tbDescription.Enabled = btnConfirmAll.Enabled = true;
                    }
                    else
                    {
                        Session[sessionConfirmationPairs] = null;
                        cbConfirmationPassType.Items.Clear();
                        tbDescription.Text = "";
                        cbConfirmationPassType.Enabled = tbDescription.Enabled = btnConfirmAll.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnPrint_Click(Object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(HRSSCConfirmationPage).Assembly);
                Session["TLPaidLeavePage.ds"] = null;

                ClearSessionValues();

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
                Dictionary<int, Dictionary<int, List<IOPairProcessedTO>>> emplValidPairs = new Dictionary<int, Dictionary<int, List<IOPairProcessedTO>>>();
                Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> emplRules = new Common.Rule(Session[Constants.sessionConnection]).SearchWUEmplTypeDictionary();
                // get all pass types
                Dictionary<int, PassTypeTO> passTypesAll = new PassType(Session[Constants.sessionConnection]).SearchDictionary();
                Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplOldCompareCounters = new EmployeeCounterValue(Session[Constants.sessionConnection]).SearchValues(emplIDs);

                foreach (IOPairProcessedTO pair in emplPairsList)
                {
                    int company = -1;
                    int emplTypeID = -1;
                    if (employees.ContainsKey(pair.EmployeeID))
                    {
                        company = Common.Misc.getRootWorkingUnit(employees[pair.EmployeeID].WorkingUnitID, wUnits);
                        emplTypeID = employees[pair.EmployeeID].EmployeeTypeID;
                    }

                    Dictionary<int, PassTypeTO> pass_types = new Dictionary<int, PassTypeTO>();
                    
                    // get rules for employeeID and its company
                    Dictionary<string, RuleTO> rules = new Dictionary<string, RuleTO>();
                    if (company != -1 && emplTypeID != -1 && emplRules.ContainsKey(company) && emplRules[company].ContainsKey(emplTypeID))
                        rules = emplRules[company][emplTypeID];

                    if (passTypesAll.ContainsKey(pair.PassTypeID) && passTypesAll[pair.PassTypeID].IsPass == Constants.wholeDayAbsence)
                    {
                        // get company annualLeave
                        PassTypeTO ptAL = new PassTypeTO();
                        if (rules.ContainsKey(Constants.RuleCompanyAnnualLeave) && passTypesAll.ContainsKey(rules[Constants.RuleCompanyAnnualLeave].RuleValue))
                        {
                            ptAL = passTypesAll[rules[Constants.RuleCompanyAnnualLeave].RuleValue];
                            if (!pass_types.ContainsKey(ptAL.PassTypeID))
                                pass_types.Add(ptAL.PassTypeID, ptAL);
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

                            if (!pass_types.ContainsKey(ptCAL.PassTypeID))
                                pass_types.Add(ptCAL.PassTypeID, ptCAL);
                        }

                        PassTypeTO ptSickNcf = new PassTypeTO();
                        if (rules.ContainsKey(Constants.RuleCompanySickLeaveNCF) && passTypesAll.ContainsKey(rules[Constants.RuleCompanySickLeaveNCF].RuleValue))
                        {
                            ptSickNcf = passTypesAll[rules[Constants.RuleCompanySickLeaveNCF].RuleValue];

                            if (!pass_types.ContainsKey(ptSickNcf.PassTypeID))
                                pass_types.Add(ptSickNcf.PassTypeID, ptSickNcf);

                            PassTypesConfirmation passConfirm = new PassTypesConfirmation(Session[Constants.sessionConnection]);
                            passConfirm.PTConfirmTO.PassTypeID = ptSickNcf.PassTypeID;
                            List<PassTypesConfirmationTO> listSickConfirm = passConfirm.Search();
                            foreach (PassTypesConfirmationTO confirm in listSickConfirm)
                            {
                                if (passTypesAll.ContainsKey(confirm.ConfirmationPassTypeID))
                                {
                                    if (!pass_types.ContainsKey(confirm.ConfirmationPassTypeID))
                                        pass_types.Add(confirm.ConfirmationPassTypeID, passTypesAll[confirm.ConfirmationPassTypeID]);
                                }
                            }
                        }

                        PassTypeTO ptOfficialTrip = new PassTypeTO();
                        if (rules.ContainsKey(Constants.RuleCompanyOfficialTrip) && passTypesAll.ContainsKey(rules[Constants.RuleCompanyOfficialTrip].RuleValue))
                        {
                            ptOfficialTrip = passTypesAll[rules[Constants.RuleCompanyOfficialTrip].RuleValue];

                            if (!pass_types.ContainsKey(ptOfficialTrip.PassTypeID))
                                pass_types.Add(ptOfficialTrip.PassTypeID, ptOfficialTrip);
                        }

                        if (!pass_types.ContainsKey(pair.PassTypeID))
                        {
                            if (!emplValidPairs.ContainsKey(pair.EmployeeID))
                                emplValidPairs.Add(pair.EmployeeID, new Dictionary<int, List<IOPairProcessedTO>>());

                            if (!emplValidPairs[pair.EmployeeID].ContainsKey(pair.PassTypeID))
                                emplValidPairs[pair.EmployeeID].Add(pair.PassTypeID, new List<IOPairProcessedTO>());

                            emplValidPairs[pair.EmployeeID][pair.PassTypeID].Add(pair);
                        }
                    }

                    PassTypeTO ptPersonal = new PassTypeTO();
                    if (rules.ContainsKey(Constants.RulePersonalHolidayPassType) && passTypesAll.ContainsKey(rules[Constants.RulePersonalHolidayPassType].RuleValue))
                    {
                        ptPersonal = passTypesAll[rules[Constants.RulePersonalHolidayPassType].RuleValue];
                    }

                    if (pair.PassTypeID == ptPersonal.PassTypeID)
                    {
                        if (!emplValidPairs.ContainsKey(pair.EmployeeID))
                            emplValidPairs.Add(pair.EmployeeID, new Dictionary<int, List<IOPairProcessedTO>>());

                        if (!emplValidPairs[pair.EmployeeID].ContainsKey(pair.PassTypeID))
                            emplValidPairs[pair.EmployeeID].Add(pair.PassTypeID, new List<IOPairProcessedTO>());

                        emplValidPairs[pair.EmployeeID][pair.PassTypeID].Add(pair);
                    }
                }

                DataSet ds = new DataSet();
                DataTable table = new DataTable();
                table.Columns.Add("employee", typeof(string));
                table.Columns.Add("working_unit", typeof(string));
                table.Columns.Add("length", typeof(string));
                table.Columns.Add("start", typeof(string));
                table.Columns.Add("end", typeof(string));
                table.Columns.Add("entry_date", typeof(string));
                table.Columns.Add("pass_type", typeof(string));
                table.Columns.Add("hiring_date", typeof(string));
                table.Columns.Add("contract", typeof(string));
                table.Columns.Add("reason", typeof(string));
                table.Columns.Add("description", typeof(string));
                ds.Tables.Add(table);

                DataSet dsPaid60 = new DataSet();
                DataTable tablePaid60 = new DataTable();
                tablePaid60.Columns.Add("employee", typeof(string));
                tablePaid60.Columns.Add("working_unit", typeof(string));
                tablePaid60.Columns.Add("length", typeof(string));
                tablePaid60.Columns.Add("start", typeof(string));
                tablePaid60.Columns.Add("end", typeof(string));
                tablePaid60.Columns.Add("entry_date", typeof(string));
                tablePaid60.Columns.Add("pass_type", typeof(string));
                tablePaid60.Columns.Add("hiring_date", typeof(string));
                tablePaid60.Columns.Add("contract", typeof(string));
                tablePaid60.Columns.Add("reason", typeof(string));
                tablePaid60.Columns.Add("description", typeof(string));
                dsPaid60.Tables.Add(tablePaid60);

                RuleTO ruleTo = new RuleTO();
                ruleTo.RuleType = Constants.RulePrintPaidLeaveReportOffset;
                Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                rule.RuleTO = ruleTo;
                List<RuleTO> rulesList = rule.Search();
                
                foreach (KeyValuePair<int, Dictionary<int, List<IOPairProcessedTO>>> emplPair in emplValidPairs)
                {
                    foreach (KeyValuePair<int, List<IOPairProcessedTO>> IoPair in emplValidPairs[emplPair.Key])
                    {
                        int count = 0;
                        foreach (IOPairProcessedTO pairProc in IoPair.Value)
                        {
                            if (!pairProc.StartTime.ToString("HH:mm").Equals("00:00") || (int)pairProc.EndTime.Subtract(pairProc.StartTime).TotalHours == Constants.dayDurationStandardShift)
                                count++;
                        }

                        if (count > 0)
                        {
                            DataRow row = null;
                            if (Constants.PMCPaidLeave60Types().Contains(IoPair.Value[0].PassTypeID))
                                row = tablePaid60.NewRow();
                            else
                                row = table.NewRow();

                            if (employees.ContainsKey(emplPair.Key))
                            {
                                row[0] = employees[emplPair.Key].FirstAndLastName;
                                if (wUnits.ContainsKey(employees[emplPair.Key].WorkingUnitID))
                                    row[1] = wUnits[employees[emplPair.Key].WorkingUnitID].Description;
                            }

                            row[2] = count;
                            row[3] = IoPair.Value[0].IOPairDate.ToString("dd.MM.yyyy.");
                            row[4] = IoPair.Value[IoPair.Value.Count - 1].IOPairDate.ToString("dd.MM.yyyy.");
                            
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

                            row[5] = offsetDate.ToString("dd.MM.yyyy.");

                            row[6] = passTypesAll[IoPair.Key].Description;

                            if (ascoDict.ContainsKey(emplPair.Key))
                            {
                                if (ascoDict[emplPair.Key].DatetimeValue5 != new DateTime())
                                    row[7] = ascoDict[emplPair.Key].DatetimeValue5.ToString("dd.MM.yyyy.");
                                else row[7] = "      ";

                                if (!ascoDict[emplPair.Key].NVarcharValue10.Equals(""))
                                    row[8] = ascoDict[emplPair.Key].NVarcharValue10;
                                else
                                    row[8] = "      ";
                            }

                            if (Constants.PMCPaidLeave60Types().Contains(IoPair.Value[0].PassTypeID))
                                tablePaid60.Rows.Add(row);
                            else
                                table.Rows.Add(row);
                        }
                    }
                }

                List<HolidaysExtendedTO> holidays = new HolidaysExtended(Session[Constants.sessionConnection]).Search(fromDate, toDate);
                foreach (int emplID in employees.Keys)
                {
                    if (ascoDict.ContainsKey(emplID) && ascoDict[emplID].NVarcharValue1.Equals(Constants.holidayTypeIV))
                    {
                        if (!ascoDict[emplID].DatetimeValue1.Equals(new DateTime()))
                        {
                            DateTime personalHoliday = new DateTime(DateTime.Now.Year, ascoDict[emplID].DatetimeValue1.Month, ascoDict[emplID].DatetimeValue1.Day);

                            if (personalHoliday.Date >= DateTime.Now.Date)
                            {
                                if (personalHoliday.Date >= fromDate.Date && personalHoliday.Date <= toDate.Date)
                                {
                                    DataRow row = table.NewRow();
                                    if (employees.ContainsKey(emplID))
                                    {
                                        row[0] = employees[emplID].FirstAndLastName;
                                        if (wUnits.ContainsKey(employees[emplID].WorkingUnitID))
                                            row[1] = wUnits[employees[emplID].WorkingUnitID].Description;
                                    }

                                    row[2] = 1;
                                    row[3] = personalHoliday.ToString("dd.MM.yyyy.");
                                    row[4] = personalHoliday.ToString("dd.MM.yyyy.");

                                    DateTime offsetDate = new DateTime();
                                    if (rulesList.Count > 0)
                                    {
                                        if (DateTime.Now.Date <= personalHoliday.Date)
                                        {
                                            if (personalHoliday.AddDays(-rulesList[0].RuleValue).Date < DateTime.Now.Date)
                                                offsetDate = DateTime.Now;
                                            else
                                                offsetDate = personalHoliday.AddDays(-rulesList[0].RuleValue);
                                        }
                                        else
                                        {
                                            offsetDate = personalHoliday.AddDays(-rulesList[0].RuleValue);
                                        }
                                    }
                                    else
                                    {
                                        if (DateTime.Now < personalHoliday)
                                            offsetDate = DateTime.Now;
                                        else
                                            offsetDate = personalHoliday;
                                    }

                                    row[5] = offsetDate.ToString("dd.MM.yyyy.");

                                    row[6] = "Lični verski praznik";
                                    if (ascoDict[emplID].DatetimeValue5 != new DateTime())
                                        row[7] = ascoDict[emplID].DatetimeValue5.ToString("dd.MM.yyyy.");
                                    else
                                        row[7] = "      ";
                                    if (!ascoDict[emplID].NVarcharValue10.Equals("")) row[8] = ascoDict[emplID].NVarcharValue10;
                                    else
                                        row[8] = "      ";

                                    table.Rows.Add(row);
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (HolidaysExtendedTO holiday in holidays)
                        {
                            if (holiday.Type.Equals(Constants.personalHoliday) && holiday.Category.Equals(ascoDict[emplID].NVarcharValue1))
                            {
                                DateTime personalHolidayStart = new DateTime(DateTime.Now.Year, holiday.DateStart.Month, holiday.DateStart.Day);
                                DateTime personalHolidayEnd = new DateTime(DateTime.Now.Year, holiday.DateEnd.Month, holiday.DateEnd.Day);

                                if (personalHolidayStart.Date >= fromDate.Date && personalHolidayEnd <= toDate.Date)
                                {
                                    DataRow row = table.NewRow();
                                    if (employees.ContainsKey(emplID))
                                    {
                                        row[0] = employees[emplID].FirstAndLastName;
                                        if (wUnits.ContainsKey(employees[emplID].WorkingUnitID))
                                            row[1] = wUnits[employees[emplID].WorkingUnitID].Description;
                                    }

                                    row[2] = (holiday.DateEnd - holiday.DateStart).Days;
                                    row[3] = personalHolidayStart.ToString("dd.MM.yyyy.");
                                    row[4] = personalHolidayEnd.ToString("dd.MM.yyyy.");
                                    
                                    DateTime offsetDate = new DateTime();
                                    if (rulesList.Count > 0)
                                    {
                                        if (DateTime.Now.Date <= personalHolidayStart.Date)
                                        {
                                            if (personalHolidayStart.AddDays(-rulesList[0].RuleValue).Date < DateTime.Now.Date)
                                                offsetDate = DateTime.Now;
                                            else
                                                offsetDate = personalHolidayStart.AddDays(-rulesList[0].RuleValue);
                                        }
                                        else
                                        {
                                            offsetDate = personalHolidayStart.AddDays(-rulesList[0].RuleValue);
                                        }
                                    }
                                    else
                                    {
                                        if (DateTime.Now < personalHolidayStart)
                                            offsetDate = DateTime.Now;
                                        else
                                            offsetDate = personalHolidayStart;
                                    }
                                    row[5] = offsetDate.ToString("dd.MM.yyyy.");
                                    //row[6] = rm.GetString("hdrPersonalHoliday", culture);
                                    row[6] = "Lični verski praznik";

                                    if (ascoDict[emplID].DatetimeValue5 != new DateTime())
                                        row[7] = ascoDict[emplID].DatetimeValue5.ToString("dd.MM.yyyy.");
                                    else
                                        row[7] = "      ";
                                    if (!ascoDict[emplID].NVarcharValue10.Equals(""))
                                        row[8] = ascoDict[emplID].NVarcharValue10;
                                    else
                                        row[8] = "      ";

                                    table.Rows.Add(row);
                                }
                            }
                        }
                    }
                }

                if (!chbPaidLeave60.Checked)
                {
                    if (table.Rows.Count > 0)
                    {
                        Session["TLPaidLeavePage.ds"] = ds;

                        string wOptions = "dialogWidth:800px; dialogHeight:600px; edge:Raised; center:Yes; resizable:Yes; status:Yes;";
                        ClientScript.RegisterStartupScript(GetType(), "infoPopup", "window.open('/ACTAWeb/ReportsWeb/TLPaidLeavesReportPage.aspx', window, '" + wOptions.Trim() + "');", true);
                    }
                    else
                        lblError.Text = rm.GetString("noReportData", culture);
                }
                else
                {
                    if (tablePaid60.Rows.Count > 0)
                    {
                        Session["TLPaidLeavePage.ds"] = dsPaid60;

                        string wOptions = "dialogWidth:500px; dialogHeight:350px; edge:Raised; center:Yes; resizable:Yes; status:Yes;";
                        ClientScript.RegisterStartupScript(GetType(), "infoPopup", "window.open('/ACTAWeb/ReportsWeb/TLPaidLeaves60AdditionalData.aspx', window, '" + wOptions.Trim() + "');", true);
                    }
                    else
                        lblError.Text = rm.GetString("noReportData", culture);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in HRSSCConfirmationPage.btnPrint_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/HRSSCConfirmationPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnConfirmAll_Click(Object sender, EventArgs e)
        {
            try
            {
                // if system is closed, sign out and redirect to login page
                if (Common.Misc.getClosingEvent(DateTime.Now, Session[Constants.sessionConnection]).EventID != -1)
                    CommonWeb.Misc.LogOut(Session[Constants.sessionLogInUserLog], Session[Constants.sessionLogInUser], Session[Constants.sessionConnection], Session, Response);

                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(HRSSCConfirmationPage).Assembly);

                lblError.Text = "";
                List<IOPairProcessedTO> pairsToConfirm = new List<IOPairProcessedTO>();

                if (Session[sessionConfirmationPairs] != null && Session[sessionConfirmationPairs] is List<IOPairProcessedTO>)
                    pairsToConfirm = (List<IOPairProcessedTO>)Session[sessionConfirmationPairs];

                if (pairsToConfirm.Count <= 0)
                    lblError.Text = rm.GetString("noPairsToConfirm", culture);
                else
                {
                    if (cbConfirmationPassType.SelectedIndex < 0)
                    {
                        InitializeGraphData();
                        lblError.Text = rm.GetString("noConfirmationType", culture);
                        writeLog(DateTime.Now, false);
                        return;
                    }                   

                    int ptID = -1;
                    if (!int.TryParse(cbConfirmationPassType.SelectedItem.Value.Trim(), out ptID))
                        ptID = -1;

                    if (ptID != -1)
                    {
                        // if confirming sick leave, description must be entered and it should be date
                        List<int> sickLeaveConfirmationTypes = new List<int>();

                        // get sick leave confirmation types
                        Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                        List<int> sickLeaveNCFRules = rule.SearchRulesExact(Constants.RuleCompanySickLeaveNCF);

                        foreach (int id in sickLeaveNCFRules)
                        {
                            PassTypesConfirmation pt = new PassTypesConfirmation(Session[Constants.sessionConnection]);
                            pt.PTConfirmTO.PassTypeID = id;

                            List<PassTypesConfirmationTO> ptConfirmedTypes = pt.Search();

                            foreach (PassTypesConfirmationTO ptCF in ptConfirmedTypes)
                            {
                                sickLeaveConfirmationTypes.Add(ptCF.ConfirmationPassTypeID);
                            }
                        }


                        if (sickLeaveConfirmationTypes.Contains(ptID))
                        {
                            DateTime descDate = CommonWeb.Misc.createDate(tbDescription.Text.Trim());
                            if (descDate.Equals(new DateTime()))
                            {
                                InitializeGraphData();
                                lblError.Text = rm.GetString("noConfirmationDate", culture) + " " + DateTime.Now.Date.ToString(Constants.dateFormat);
                                writeLog(DateTime.Now, false);
                                return;
                            }
                            if (descDate.Date < new DateTime(1900, 1, 1))
                            {
                                InitializeGraphData();
                                lblError.Text = rm.GetString("confirmationDateLessThanMinAllowed", culture) + " " + DateTime.Now.Date.ToString(Constants.dateFormat);
                                writeLog(DateTime.Now, false);
                                return;
                            }
                        }

                        string error = confirmPairs(pairsToConfirm, ptID, tbDescription.Text.Trim());
                        if (error.Trim().Equals(""))
                        {
                            Session[sessionConfirmationPairs] = null;
                            btnShow_Click(this, new EventArgs());
                        }
                        else
                        {
                            Session[Constants.sessionInfoMessage] = error.Trim();
                            string wOptions = "dialogWidth:800px; dialogHeight:300px; edge:Raised; center:Yes; resizable:Yes; status:Yes;";
                            ClientScript.RegisterStartupScript(GetType(), "infoPopup", "window.open('/ACTAWeb/CommonWeb/MessagesPages/Info.aspx?isError=true', window, '" + wOptions.Trim() + "');", true);
                        }
                    }
                    else
                    {
                        lblError.Text = rm.GetString("incorrectConfirmationType", culture);
                        writeLog(DateTime.Now, false);
                        return;
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in HRSSCConfirmationPage.btnConfirmAll_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/HRSSCConfirmationPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
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

        private string confirmPairs(List<IOPairProcessedTO> pairsToConfirm, int ptID, string desc)
        {
            try
            {
                string error = "";
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(HRSSCConfirmationPage).Assembly);

                // get data for validation of confirmed pair, working units, pass types, pass types limits
                Dictionary<int, WorkingUnitTO> wUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                Dictionary<int, PassTypeTO> passTypes = new PassType(Session[Constants.sessionConnection]).SearchDictionary();
                Dictionary<int, PassTypeLimitTO> limits = new PassTypeLimit(Session[Constants.sessionConnection]).SearchDictionary();

                string emplIDs = "";
                DateTime fromDate = new DateTime();
                DateTime toDate = new DateTime();
                List<DateTime> datesList = new List<DateTime>();
                foreach (IOPairProcessedTO pair in pairsToConfirm)
                {
                    emplIDs += pair.EmployeeID.ToString().Trim() + ",";

                    if (!datesList.Contains(pair.IOPairDate.Date))
                        datesList.Add(pair.IOPairDate.Date);

                    if (fromDate.Equals(new DateTime()) || fromDate.Date > pair.IOPairDate.Date)
                        fromDate = pair.IOPairDate.Date;
                    if (toDate < pair.IOPairDate.Date)
                        toDate = pair.IOPairDate.Date;
                }

                if (emplIDs.Length > 0)
                    emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                if (emplIDs.Length > 0)
                {
                    // get all employees from pairs
                    Dictionary<int, EmployeeTO> emplList = new Employee(Session[Constants.sessionConnection]).SearchDictionary(emplIDs);

                    // get asco data
                    Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4(Session[Constants.sessionConnection]).SearchDictionary(emplIDs);

                    // get all empoloyee counters
                    Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplCounterValues = new EmployeeCounterValue(Session[Constants.sessionConnection]).SearchValues(emplIDs);

                    Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplCounters = new Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>();
                    Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplOldCounters = new Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>();

                    foreach (int emplID in emplCounterValues.Keys)
                    {
                        if (!emplCounters.ContainsKey(emplID))
                            emplCounters.Add(emplID, new Dictionary<int, EmployeeCounterValueTO>());

                        if (!emplOldCounters.ContainsKey(emplID))
                            emplOldCounters.Add(emplID, new Dictionary<int, EmployeeCounterValueTO>());

                        foreach (int type in emplCounterValues[emplID].Keys)
                        {
                            if (!emplCounters[emplID].ContainsKey(type))
                                emplCounters[emplID].Add(type, new EmployeeCounterValueTO(emplCounterValues[emplID][type]));
                            else
                                emplCounters[emplID][type] = new EmployeeCounterValueTO(emplCounterValues[emplID][type]);

                            if (!emplOldCounters[emplID].ContainsKey(type))
                                emplOldCounters[emplID].Add(type, emplCounterValues[emplID][type]);
                            else
                                emplOldCounters[emplID][type] = emplCounterValues[emplID][type];
                        }
                    }

                    // get dictionary of all rules, key is company and value are rules by employee type id
                    Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> emplRules = new Common.Rule(Session[Constants.sessionConnection]).SearchWUEmplTypeDictionary();

                    // get all pairs for employess and dates from pars to confirm
                    List<IOPairProcessedTO> dayPairs = new IOPairProcessed(Session[Constants.sessionConnection]).SearchAllPairsForEmpl(emplIDs, datesList, "");

                    // create dictionary of day pars for each employee by each day
                    Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplDayPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                    foreach (IOPairProcessedTO pair in dayPairs)
                    {
                        if (!emplDayPairs.ContainsKey(pair.EmployeeID))
                            emplDayPairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());

                        if (!emplDayPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                            emplDayPairs[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<IOPairProcessedTO>());

                        emplDayPairs[pair.EmployeeID][pair.IOPairDate.Date].Add(pair);
                    }

                    // get time schedules and schemas for employees                    
                    Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule(Session[Constants.sessionConnection]).SearchEmployeesSchedulesExactDate(emplIDs, fromDate.Date, toDate.AddDays(1).Date, null);

                    Dictionary<int, WorkTimeSchemaTO> schemas = new TimeSchema(Session[Constants.sessionConnection]).getDictionary();

                    // validate and confirm each pair
                    foreach (IOPairProcessedTO pair in pairsToConfirm)
                    {
                        if (Session[Constants.sessionLoginCategoryPassTypes] != null && Session[Constants.sessionLoginCategoryPassTypes] is List<int>
                            && !((List<int>)Session[Constants.sessionLoginCategoryPassTypes]).Contains(ptID))
                        {
                            error += getEmployeeData(pair.EmployeeID, emplList).Trim() + ": " + rm.GetString("noConfirmTypePrivilage", culture) + " ";
                            continue;
                        }

                        Dictionary<int, EmployeeCounterValueTO> counters = new Dictionary<int, EmployeeCounterValueTO>();
                        Dictionary<int, EmployeeCounterValueTO> oldCounters = new Dictionary<int, EmployeeCounterValueTO>();
                        List<IOPairProcessedTO> emplDatePairs = new List<IOPairProcessedTO>();
                        Dictionary<string, RuleTO> rules = new Dictionary<string, RuleTO>();

                        // get data for employee and date from pair
                        if (emplCounters.ContainsKey(pair.EmployeeID))
                            counters = emplCounters[pair.EmployeeID];
                        if (emplOldCounters.ContainsKey(pair.EmployeeID))
                            oldCounters = emplOldCounters[pair.EmployeeID];
                        if (emplDayPairs.ContainsKey(pair.EmployeeID) && emplDayPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                            emplDatePairs = emplDayPairs[pair.EmployeeID][pair.IOPairDate.Date];

                        int company = -1;
                        int emplTypeID = -1;
                        if (emplList.ContainsKey(pair.EmployeeID))
                        {
                            company = Common.Misc.getRootWorkingUnit(emplList[pair.EmployeeID].WorkingUnitID, wUnits);
                            emplTypeID = emplList[pair.EmployeeID].EmployeeTypeID;
                        }
                        if (company != -1 && emplTypeID != -1 && emplRules.ContainsKey(company) && emplRules[company].ContainsKey(emplTypeID))
                            rules = emplRules[company][emplTypeID];

                        List<IOPairProcessedTO> oldPairs = new List<IOPairProcessedTO>();
                        oldPairs.Add(new IOPairProcessedTO(pair));
                        List<IOPairProcessedTO> newPairs = new List<IOPairProcessedTO>();
                        IOPairProcessedTO newPair = new IOPairProcessedTO(pair);
                        newPair.PassTypeID = ptID;
                        newPairs.Add(newPair);
                        Dictionary<DateTime, WorkTimeSchemaTO> daySchemas = new Dictionary<DateTime, WorkTimeSchemaTO>();
                        Dictionary<DateTime, List<WorkTimeIntervalTO>> dayIntervalsList = new Dictionary<DateTime, List<WorkTimeIntervalTO>>();
                        if (emplSchedules.ContainsKey(pair.EmployeeID))
                        {
                            daySchemas.Add(pair.IOPairDate.Date, Common.Misc.getTimeSchema(pair.IOPairDate.Date, emplSchedules[pair.EmployeeID], schemas));
                            dayIntervalsList.Add(pair.IOPairDate.Date, Common.Misc.getTimeSchemaInterval(pair.IOPairDate.Date, emplSchedules[pair.EmployeeID], schemas));
                        }
                        EmployeeAsco4TO emplAsco = new EmployeeAsco4TO();
                        if (ascoDict.ContainsKey(pair.EmployeeID))
                            emplAsco = ascoDict[pair.EmployeeID];

                        string validation = Common.Misc.validatePairsPassType(pair.EmployeeID, emplAsco, pair.IOPairDate, pair.IOPairDate, newPairs, oldPairs, emplDatePairs, ref counters, rules,
                            passTypes, limits, schemas, daySchemas, dayIntervalsList, null, null, null, new List<IOPairProcessedTO>(), new List<DateTime>(), null,
                            Session[Constants.sessionConnection], CommonWeb.Misc.checkLimit(Session[Constants.sessionLoginCategory]), false, true, false);

                        //string validation = validatePairPassType(pair, ref counters, pair.PassTypeID, ptID, emplDatePairs, rules, passTypes, limits);
                        if (validation.Trim().Equals(""))
                        {
                            // update pair and counters
                            pair.PassTypeID = ptID;
                            if (!desc.Trim().Equals(""))
                                pair.Desc = desc.Trim();
                            pair.ConfirmationFlag = (int)Constants.Confirmation.Confirmed;
                            pair.ConfirmedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                            pair.ConfirmationTime = DateTime.Now;
                            pair.ModifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);

                            string confirmation = confirmPairCounters(pair, counters, oldCounters);

                            if (!confirmation.Trim().Equals(""))
                            {
                                error += getEmployeeData(pair.EmployeeID, emplList) + ": " + confirmation.Trim() + " ";
                                continue;
                            }
                        }
                        else
                        {
                            error += getEmployeeData(pair.EmployeeID, emplList) + ": " + rm.GetString(validation.Trim(), culture) + " ";
                            continue;
                        }
                    }
                }

                return error;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string getEmployeeData(int id, Dictionary<int, EmployeeTO> emplList)
        {
            try
            {
                if (emplList.ContainsKey(id))
                    return id.ToString().Trim() + " " + emplList[id].FirstAndLastName.Trim();
                else
                    return id.ToString().Trim();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string confirmPairCounters(IOPairProcessedTO pairToConfirm, Dictionary<int, EmployeeCounterValueTO> emplCounters, Dictionary<int, EmployeeCounterValueTO> emplOldCounters)
        {
            try
            {
                IOPairProcessed pair = new IOPairProcessed(Session[Constants.sessionConnection]);
                bool saved = true;
                string error = "";

                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(HRSSCConfirmationPage).Assembly);

                if (pair.BeginTransaction())
                {
                    try
                    {
                        EmployeeCounterValue counter = new EmployeeCounterValue(Session[Constants.sessionConnection]);
                        EmployeeCounterValueHist counterHist = new EmployeeCounterValueHist(Session[Constants.sessionConnection]);
                        IOPairsProcessedHist pairHist = new IOPairsProcessedHist(Session[Constants.sessionConnection]);

                        pairHist.SetTransaction(pair.GetTransaction());
                        saved = saved && (pairHist.Save(pairToConfirm.EmployeeID, pairToConfirm.ModifiedBy, DateTime.Now, pairToConfirm.IOPairDate.Date, Constants.alertStatus, false) >= 0);

                        if (saved)
                        {
                            pair.IOPairProcessedTO = pairToConfirm;
                            saved = saved && (pair.Update(false));
                        }

                        if (saved)
                        {
                            // update counters, updated counters insert to hist table
                            counterHist.SetTransaction(pair.GetTransaction());
                            counter.SetTransaction(pair.GetTransaction());
                            // update counters and move old counter values to hist table if updated
                            foreach (int type in emplCounters.Keys)
                            {
                                if (emplOldCounters.ContainsKey(type) && emplOldCounters[type].Value != emplCounters[type].Value)
                                {
                                    // move to hist table
                                    counterHist.ValueTO = new EmployeeCounterValueHistTO(emplOldCounters[type]);
                                    counterHist.ValueTO.ModifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                                    saved = saved && (counterHist.Save(false) >= 0);

                                    if (!saved)
                                        break;

                                    counter.ValueTO = new EmployeeCounterValueTO(emplCounters[type]);
                                    //counter.ValueTO.EmplCounterTypeID = type;
                                    //counter.ValueTO.EmplID = pairToConfirm.EmployeeID;
                                    //counter.ValueTO.Value = emplCounters[type];
                                    counter.ValueTO.ModifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);

                                    saved = saved && counter.Update(false);

                                    if (!saved)
                                        break;
                                }
                            }
                        }

                        if (saved)
                        {
                            pair.CommitTransaction();
                        }
                        else
                        {
                            if (pair.GetTransaction() != null)
                                pair.RollbackTransaction();
                            error = rm.GetString("pairsNotConfirmed", culture);
                        }
                    }
                    catch
                    {
                        if (pair.GetTransaction() != null)
                            pair.RollbackTransaction();
                        error = rm.GetString("pairsNotConfirmed", culture);
                    }
                }
                else
                    error = rm.GetString("pairsNotConfirmed", culture);

                return error;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populateConfirmationPassTypes(int ptToConfirmID)
        {
            try
            {
                List<PassTypeTO> typeList = new List<PassTypeTO>();

                if (ptToConfirmID != -1)
                {
                    string ptIDs = "";
                    if (Session[Constants.sessionLoginCategoryPassTypes] != null && Session[Constants.sessionLoginCategoryPassTypes] is List<int>)
                    {
                        foreach (int ptID in ((List<int>)Session[Constants.sessionLoginCategoryPassTypes]))
                        {
                            ptIDs += ptID.ToString().Trim() + ",";
                        }

                        if (ptIDs.Length > 0)
                            ptIDs = ptIDs.Substring(0, ptIDs.Length - 1);
                    }

                    bool isAltLang = !CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper());
                    typeList = new PassType(Session[Constants.sessionConnection]).SearchConformationTypes(ptToConfirmID, ptIDs, isAltLang);
                }

                cbConfirmationPassType.DataSource = typeList;
                if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                {
                    cbConfirmationPassType.DataTextField = "DescriptionAndID";
                }
                else
                    cbConfirmationPassType.DataTextField = "DescriptionAltAndID";
                cbConfirmationPassType.DataValueField = "PassTypeID";
                cbConfirmationPassType.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ClearSessionValues()
        {
            try
            {
                // clean Session
                Session[Constants.sessionEmplCounters] = null;
                Session[Constants.sessionDayPairs] = null;
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

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "HRSSCConfirmationPage.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "HRSSCConfirmationPage.", filterState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
