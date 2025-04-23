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
    public partial class WCManagerVerificationPage : System.Web.UI.Page
    {
        const string pageName = "WCManagerVerificationPage";

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
                    btnFromDate.Attributes.Add("onclick", "return calendarPicker('tbFromDate', 'true');");
                    btnToDate.Attributes.Add("onclick", "return calendarPicker('tbToDate', 'true');");
                    btnWUTree.Attributes.Add("onclick", "return wUnitsPicker('btnWUTree', 'false');");
                    btnOrgTree.Attributes.Add("onclick", "return oUnitsPicker('btnOrgTree', 'false');");
                    cbSelectPassTypes.Attributes.Add("onclick", "return selectListItems('cbSelectPassTypes', 'lbPassTypes');");
                    cbSelectAllEmpolyees.Attributes.Add("onclick", "return selectListItems('cbSelectAllEmpolyees', 'lboxEmployees');");
                    tbEmployee.Attributes.Add("onKeyUp", "return selectItem('tbEmployee', 'lboxEmployees');");
                    btnShow.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnVerifyAll.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
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

                    cbSelectAllEmpolyees.Visible = cbSelectPassTypes.Visible = false;

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

                    rbVerification.Checked = true;
                    rbVerification_CheckedChanged(this, new EventArgs());
                    
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
                    else if (Request["__EVENTARGUMENT"] != null && Request["__EVENTARGUMENT"].StartsWith(Constants.undoVerificationClientScriptArg))
                    {
                        lblError.Text = "";

                        string[] data = Request["__EVENTARGUMENT"].Substring(Constants.undoVerificationClientScriptArg.Length).Split('|');
                        
                        int emplID = -1;
                        DateTime date = new DateTime();
                        DateTime modifiedTime = new DateTime();
                        string modifiedBy = "";

                        // 0 - employee id
                        // 1 - date of pairs
                        // 2 - modified time from hist day set                        
                        // 3 - modified by from hist day set
                        if (data.Length == 4)
                        {
                            if (!int.TryParse(data[0], out emplID))
                                emplID = -1;

                            date = CommonWeb.Misc.createDate(data[1]);

                            modifiedTime = DateTime.Parse(data[2]);
                            
                            modifiedBy = data[3];
                        }

                        if (emplID != -1 && !date.Equals(new DateTime()) && !modifiedTime.Equals(new DateTime()) && !modifiedBy.Trim().Equals(""))
                        {
                            string error = Undoverify(emplID, date, modifiedTime, modifiedBy);

                            if (!error.Trim().Equals(""))
                                lblError.Text = error.Trim();
                        }
                        
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCManagerVerificationPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCManagerVerificationPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnPrevDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCManagerVerificationPage).Assembly);

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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCManagerVerificationPage.btnPrevDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCManagerVerificationPage.aspx&Header=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnNextDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCManagerVerificationPage).Assembly);

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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCManagerVerificationPage.btnNextDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCManagerVerificationPage.aspx&Header=" + Constants.trueValue.Trim(), false);
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCManagerVerificationPage.Date_Changed(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCManagerVerificationPage.aspx&Header=" + Constants.falseValue.Trim(), false);
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCManagerVerificationPage).Assembly);

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
                List<int> resPersonTypesVisible = new List<int>();

                if (cbEmplType.SelectedIndex > 0)
                {
                    int type = -1;
                    if (int.TryParse(cbEmplType.SelectedValue.Trim(), out type) && type != -1)
                    {
                        typesVisible.Add(type);

                        if (type != (int)Constants.EmployeeTypesFIAT.Expat && type != (int)Constants.EmployeeTypesFIAT.TaskForce && type != (int)Constants.EmployeeTypesFIAT.Agency)
                            resPersonTypesVisible.Add(type);
                    }
                }
                else
                {
                    foreach (ListItem item in cbEmplType.Items)
                    {
                        int type = -1;
                        if (int.TryParse(item.Value.Trim(), out type) && type != -1)
                        {
                            typesVisible.Add(type);

                            if (type != (int)Constants.EmployeeTypesFIAT.Expat && type != (int)Constants.EmployeeTypesFIAT.TaskForce && type != (int)Constants.EmployeeTypesFIAT.Agency)
                                resPersonTypesVisible.Add(type);
                        }
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

        private string Undoverify(int emplID, DateTime date, DateTime modifiedTime, string modifiedBy)
        {
            try
            {
                // if system is closed, sign out and redirect to login page
                if (Common.Misc.getClosingEvent(DateTime.Now, Session[Constants.sessionConnection]).EventID != -1)
                    CommonWeb.Misc.LogOut(Session[Constants.sessionLogInUserLog], Session[Constants.sessionLogInUser], Session[Constants.sessionConnection], Session, Response);

                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCManagerVerificationPage).Assembly);
                                
                List<DateTime> dateList = new List<DateTime>();
                dateList.Add(date.Date);
                List<IOPairProcessedTO> verifiedPairs = new IOPairProcessed(Session[Constants.sessionConnection]).SearchAllPairsForEmpl(emplID.ToString(), dateList, "");

                Dictionary<DateTime, List<IOPairsProcessedHistTO>> histSets = new IOPairsProcessedHist(Session[Constants.sessionConnection]).SearchIOPairsSet(emplID, date);
                List<IOPairsProcessedHistTO> unverifiedPairsHist = new List<IOPairsProcessedHistTO>();
                if (histSets.ContainsKey(modifiedTime) && CommonWeb.Misc.isUserChangedDay(histSets[modifiedTime], CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser])))
                    unverifiedPairsHist = histSets[modifiedTime];
                
                // get asco data
                Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4(Session[Constants.sessionConnection]).SearchDictionary(emplID.ToString().Trim());
                EmployeeAsco4TO emplAsco = new EmployeeAsco4TO();
                if (ascoDict.ContainsKey(emplID))
                    emplAsco = ascoDict[emplID];

                // get counters
                Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> oldValues = new EmployeeCounterValue(Session[Constants.sessionConnection]).SearchValues(emplID.ToString());
                Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> newValues = new EmployeeCounterValue(Session[Constants.sessionConnection]).SearchValues(emplID.ToString());

                Dictionary<int, EmployeeCounterValueTO> emplCounters = new Dictionary<int, EmployeeCounterValueTO>();
                if (newValues.ContainsKey(emplID))
                    emplCounters = newValues[emplID];

                Dictionary<int, EmployeeCounterValueTO> emplOldCounters = new Dictionary<int, EmployeeCounterValueTO>();
                if (oldValues.ContainsKey(emplID))
                    emplOldCounters = oldValues[emplID];

                // get rules
                EmployeeTO empl = new Employee(Session[Constants.sessionConnection]).Find(emplID.ToString());
                Dictionary<int, WorkingUnitTO> WUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                int company = Common.Misc.getRootWorkingUnit(empl.WorkingUnitID, WUnits);

                Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                rule.RuleTO.EmployeeTypeID = empl.EmployeeTypeID;
                rule.RuleTO.WorkingUnitID = company;
                List<RuleTO> emplRules = rule.Search();

                Dictionary<string, RuleTO> rules = new Dictionary<string, RuleTO>();
                foreach (RuleTO ruleTO in emplRules)
                {
                    if (!rules.ContainsKey(ruleTO.RuleType))
                        rules.Add(ruleTO.RuleType, ruleTO);
                    else
                        rules[ruleTO.RuleType] = ruleTO;
                }

                // get pass types
                Dictionary<int, PassTypeTO> passTypesAllDic = getTypes();

                // get time schema
                List<EmployeeTimeScheduleTO> timeScheduleList = new List<EmployeeTimeScheduleTO>();
                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule(Session[Constants.sessionConnection]).SearchEmployeesSchedulesExactDate(empl.EmployeeID.ToString().Trim(), date.Date, date.Date, null);
                if (emplSchedules.ContainsKey(empl.EmployeeID))
                    timeScheduleList = emplSchedules[empl.EmployeeID];

                string schemaID = "";
                foreach (EmployeeTimeScheduleTO employeeTimeSchedule in timeScheduleList)
                {
                    schemaID += employeeTimeSchedule.TimeSchemaID.ToString() + ", ";
                }
                if (!schemaID.Equals(""))
                {
                    schemaID = schemaID.Substring(0, schemaID.Length - 2);
                }
                                
                Dictionary<int, WorkTimeSchemaTO> schemasDic = new TimeSchema(Session[Constants.sessionConnection]).getDictionary();
                
                WorkTimeSchemaTO schema = Common.Misc.getTimeSchema(date, timeScheduleList, schemasDic);
                List<WorkTimeIntervalTO> dayIntervals = Common.Misc.getTimeSchemaInterval(date, timeScheduleList, schemasDic);

                // get pass types limits
                List<PassTypeLimitTO> ptLimitsList = new PassTypeLimit(Session[Constants.sessionConnection]).Search();
                Dictionary<int, PassTypeLimitTO> passTypeLimits = new Dictionary<int, PassTypeLimitTO>();

                foreach (PassTypeLimitTO limit in ptLimitsList)
                {
                    if (!passTypeLimits.ContainsKey(limit.PtLimitID))
                        passTypeLimits.Add(limit.PtLimitID, limit);
                    else
                        passTypeLimits[limit.PtLimitID] = limit;
                }
                                
                Dictionary<DateTime, WorkTimeSchemaTO> daySchemas = new Dictionary<DateTime, WorkTimeSchemaTO>();
                daySchemas.Add(date, schema);

                Dictionary<DateTime, List<WorkTimeIntervalTO>> dayIntervalsList = new Dictionary<DateTime, List<WorkTimeIntervalTO>>();
                dayIntervalsList.Add(date, dayIntervals);
                
                // get annual leaves from from and to week
                List<IOPairProcessedTO> weekAnnualLeave = new IOPairProcessed(Session[Constants.sessionConnection]).SearchWeekPairs(empl.EmployeeID, date.Date, false, Common.Misc.getAnnualLeaveTypesString(rules), null);

                List<IOPairProcessedTO> unverifiedPairs = new List<IOPairProcessedTO>();

                foreach (IOPairsProcessedHistTO unverifiedPair in unverifiedPairsHist)
                {
                    unverifiedPairs.Add(new IOPairProcessedTO(unverifiedPair));                    
                }

                string error = Common.Misc.validatePairsPassType(emplID, emplAsco, date.Date, date.Date, unverifiedPairs, verifiedPairs, verifiedPairs, ref emplCounters, rules, passTypesAllDic,
                    passTypeLimits, schemasDic, daySchemas, dayIntervalsList, weekAnnualLeave, weekAnnualLeave, null, new List<IOPairProcessedTO>(), new List<DateTime>(), null,
                    Session[Constants.sessionConnection], CommonWeb.Misc.checkLimit(Session[Constants.sessionLoginCategory]), false, true, false);

                if (!error.Trim().Equals(""))
                    return rm.GetString(error.Trim(), culture);
                                
                IOPairProcessed pair = new IOPairProcessed(Session[Constants.sessionConnection]);
                IOPairsProcessedHist pairHist = new IOPairsProcessedHist(Session[Constants.sessionConnection]);
                EmployeeCounterValue counter = new EmployeeCounterValue(Session[Constants.sessionConnection]);
                EmployeeCounterValueHist counterHist = new EmployeeCounterValueHist(Session[Constants.sessionConnection]);

                bool saved = true;
                DateTime modTime = DateTime.Now;
                if (pair.BeginTransaction())
                {
                    try
                    {
                        foreach (IOPairProcessedTO verifiedPair in verifiedPairs)
                        {
                            // move pair to hist
                            pairHist.SetTransaction(pair.GetTransaction());
                            pairHist.IOPairProcessedHistTO = new IOPairsProcessedHistTO(verifiedPair);
                            pairHist.IOPairProcessedHistTO.Alert = Constants.alertStatus.ToString();
                            pairHist.IOPairProcessedHistTO.ModifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                            pairHist.IOPairProcessedHistTO.ModifiedTime = modTime;
                            saved = saved && (pairHist.Save(false) >= 0);

                            if (!saved)
                                break;

                            // delete pair
                            saved = saved && pair.Delete(verifiedPair.RecID.ToString().Trim(), false);

                            if (!saved)
                                break;
                        }

                        foreach (IOPairsProcessedHistTO unverifiedPair in unverifiedPairsHist)
                        {
                            // save pair to table                            
                            pair.IOPairProcessedTO = new IOPairProcessedTO(unverifiedPair);
                            pair.IOPairProcessedTO.ManualCreated = (int)Constants.recordCreated.Manualy;
                            saved = saved && (pair.Save(false) >= 0);

                            if (!saved)
                                break;
                        }

                        // update counters
                        if (saved)
                        {
                            // update counters from session, updated counters insert to hist table
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
                                    //counter.ValueTO.EmplID = emplID;
                                    //counter.ValueTO.Value = emplCounters[type];
                                    counter.ValueTO.ModifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);

                                    saved = saved && counter.Update(false);

                                    if (!saved)
                                        break;
                                }
                            }
                        }

                        if (saved)
                            pair.CommitTransaction();
                        else
                        {
                            pair.RollbackTransaction();
                            return rm.GetString("undoVerifiedFailed", culture);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (pair.GetTransaction() != null)
                            pair.RollbackTransaction();

                        throw ex;
                    }
                }

                return "";
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCManagerVerificationPage.Menu1_MenuItemClick(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCManagerVerificationPage.aspx&Header=" + Constants.falseValue.Trim(), false);
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCManagerVerificationPage.cbEmplType_SelectedIndexChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCManagerVerificationPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void rbVerification_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                rbUndoVerification.Checked = !rbVerification.Checked;
                btnVerifyAll.Visible = rbVerification.Checked;
                lblHelp.Visible = !rbVerification.Checked;

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCManagerVerificationPage.rbVerification_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCManagerVerificationPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void rbUndoVerification_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                rbVerification.Checked = !rbUndoVerification.Checked;
                btnVerifyAll.Visible = !rbUndoVerification.Checked;
                lblHelp.Visible = rbUndoVerification.Checked;

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCManagerVerificationPage.rbUndoVerification_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCManagerVerificationPage.aspx&Header=" + Constants.falseValue.Trim(), false);
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
                        if (pt.PassTypeID != Constants.absence)
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCManagerVerificationPage).Assembly);

                lblEmployee.Text = rm.GetString("lblEmployeeSelection", culture);
                lblPeriod.Text = rm.GetString("lblPeriod", culture);
                lblPassTypes.Text = rm.GetString("lblPassTypes", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblHelp.Text = rm.GetString("lblUndoVerificationHelp", culture);
                lblEmplType.Text = rm.GetString("lblEmployeeType", culture);
                lblPassTypes.Text = rm.GetString("lblPassType", culture);
                
                cbSelectAllEmpolyees.Text = rm.GetString("lblSelectAll", culture);
                cbSelectPassTypes.Text = rm.GetString("lblSelectAll", culture);

                rbVerification.Text = rm.GetString("rbVerification", culture);
                rbUndoVerification.Text = rm.GetString("rbUndoVerification", culture);
                rbHierarchically.Text = rm.GetString("rbHierarchically", culture);
                rbSelected.Text = rm.GetString("rbSelected", culture);
                rbResponsiblePerson.Text = rm.GetString("rbResponsiblePerson", culture);

                btnShow.Text = rm.GetString("btnShow", culture);
                btnVerifyAll.Text = rm.GetString("btnVerifyAll", culture);                
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
                selectedPairs.Value = "";

                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCManagerVerificationPage).Assembly);

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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCManagerVerificationPage.btnShow_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCManagerVerificationPage.aspx", false);
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

                if (!fromDate.Equals(new DateTime()) && !toDate.Equals(new DateTime()) && lboxEmployees.Items.Count > 0)
                {
                    List<DateTime> dateList = new List<DateTime>();
                    DateTime day = fromDate.Date;
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
                        
                        List<IOPairProcessedTO> IOPairList = FindIOPairsForEmployee(emplIDs, dateList, ptIDs);

                        DrawGraphControl(emplIDs, dateList, IOPairList, PassTypes, WUnits, Empl, emplList, rbVerification.Checked, emplAdditionalData);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<IOPairProcessedTO> FindIOPairsForEmployee(string emplIDs, List<DateTime> dateList, string ptIDs)
        {
            try
            {
                List<IOPairProcessedTO> pairs = new IOPairProcessed(Session[Constants.sessionConnection]).SearchAllPairsForEmpl(emplIDs, dateList, ptIDs);
                
                // pairs that do not belong to selected interval will be skiped during the drawing

                //if (dateList.Count > 0)
                //{
                //    IEnumerator<IOPairProcessedTO> pairEnumerator = pairs.GetEnumerator();
                //    while (pairEnumerator.MoveNext())
                //    {
                //        // remove second night shift interval from first day, it belongs to previous day **********night shift
                //        if (pairEnumerator.Current.IOPairDate.Date.Equals(dateList[0].Date) && pairEnumerator.Current.StartTime.Hour == 0 && pairEnumerator.Current.StartTime.Minute == 0)
                //        {
                //            pairs.Remove(pairEnumerator.Current);
                //            pairEnumerator = pairs.GetEnumerator();
                //        }
                //        // remove all but second shift interval from day after last day, it belongs to last day
                //        else if (pairEnumerator.Current.IOPairDate.Date.Equals(dateList[dateList.Count - 1].Date)
                //            && !(pairEnumerator.Current.StartTime.Hour == 0 && pairEnumerator.Current.StartTime.Minute == 0))
                //        {
                //            pairs.Remove(pairEnumerator.Current);
                //            pairEnumerator = pairs.GetEnumerator();
                //        }
                //    }
                //}

                return pairs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Dictionary<int, Dictionary<DateTime, Dictionary<string, List<IOPairsProcessedHistTO>>>> FindIOPairsHistForEmployee(string emplIDs, List<DateTime> dateList)
        {
            try
            {
                Dictionary<int, Dictionary<DateTime, Dictionary<string, List<IOPairsProcessedHistTO>>>> pairs = new IOPairsProcessedHist(Session[Constants.sessionConnection]).SearchAllPairsForEmpl(emplIDs, dateList);                

                return pairs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DrawGraphControl(string emplIDs, List<DateTime> dateList, List<IOPairProcessedTO> IOPairList, Dictionary<int, PassTypeTO> PassTypes, Dictionary<int, WorkingUnitTO> WUnits,
            EmployeeTO Empl, List<EmployeeTO> emplList, bool forVerification, Dictionary<int, EmployeeAsco4TO> emplAddData)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCManagerVerificationPage).Assembly);

                //get hrssc cuttoff date - Empl is logged in employee
                int hrsscCutoffDate = -1;
                if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                    && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRSSC && Empl.EmployeeID != -1)
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
                            hrsscCutoffDate = rules[0].RuleValue;
                        }
                    }
                }

                //get wcdr cuttoff date - Empl is logged in employee
                int wcdrCutoffDate = -1;
                if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                    && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.WCManager && Empl.EmployeeID != -1)
                {
                    int company = Common.Misc.getRootWorkingUnit(Empl.WorkingUnitID, WUnits);

                    if (company != -1)
                    {
                        Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                        rule.RuleTO.WorkingUnitID = company;
                        rule.RuleTO.EmployeeTypeID = Empl.EmployeeTypeID;
                        rule.RuleTO.RuleType = Constants.RuleWCDRCutOffDate;

                        List<RuleTO> rules = rule.Search();

                        if (rules.Count == 1)
                        {
                            wcdrCutoffDate = rules[0].RuleValue;
                        }
                    }
                }

                // get all cut off dates
                Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> cuttOffDays = new Common.Rule(Session[Constants.sessionConnection]).SearchTypeAllRules(Constants.RuleCutOffDate);
                
                // create dictionary with pairs for specific employee for specific day - key is employeeID, value is dictionary of day and pairs for that day
                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplDayPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplDayPairsToVerify = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplDayVerifiedPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                Dictionary<int, Dictionary<DateTime, List<IOPairsProcessedHistTO>>> emplDayUnverifiedPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairsProcessedHistTO>>>();

                // employee, modified time, modified by - day sets by employee, modified time and modified by
                Dictionary<int, Dictionary<DateTime, Dictionary<string, List<IOPairsProcessedHistTO>>>> histPairs = new Dictionary<int,Dictionary<DateTime,Dictionary<string,List<IOPairsProcessedHistTO>>>>();
                if (!forVerification)
                {
                    histPairs = FindIOPairsHistForEmployee(emplIDs, dateList);
                }

                // dispose and clear existing controls
                foreach (Control ctrl in ctrlHolder.Controls)
                {
                    ctrl.Dispose();
                }

                ctrlHolder.Controls.Clear();

                // draw pair bars for each selected employee and each day
                int currentIndex = 0;

                if (dateList.Count > 0)
                {
                    DateTime firstDay = dateList[0];
                    DateTime lastDay = dateList[dateList.Count - 1];

                    Color backColor = Color.White;
                    bool isAltCtrl = false;

                    // get schedules for all employees
                    Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule(Session[Constants.sessionConnection]).SearchEmployeesSchedulesExactDate(emplIDs, firstDay.Date, lastDay.Date, null);

                    // get all time schemas
                    Dictionary<int, WorkTimeSchemaTO> schemas = new TimeSchema(Session[Constants.sessionConnection]).getDictionary();

                    // get all pairs for first and last day
                    List<DateTime> borderDays = new List<DateTime>();
                    borderDays.Add(firstDay.Date);
                    borderDays.Add(lastDay.Date);
                    List<IOPairProcessedTO> emplBorderDays = new IOPairProcessed(Session[Constants.sessionConnection]).SearchAllPairsForEmpl(emplIDs, borderDays, "");
                    Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplBorderDaysPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                    foreach (IOPairProcessedTO pair in emplBorderDays)
                    {
                        if (!emplBorderDaysPairs.ContainsKey(pair.EmployeeID))
                            emplBorderDaysPairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());

                        if (!emplBorderDaysPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                            emplBorderDaysPairs[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<IOPairProcessedTO>());

                        emplBorderDaysPairs[pair.EmployeeID][pair.IOPairDate.Date].Add(pair);
                    }

                    foreach (IOPairProcessedTO pair in IOPairList)
                    {
                        if (pair.IOPairDate.Date.Equals(firstDay.Date) || pair.IOPairDate.Date.Equals(lastDay.Date))
                        {
                            List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                            if (emplBorderDaysPairs.ContainsKey(pair.EmployeeID) && emplBorderDaysPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                                dayPairs = emplBorderDaysPairs[pair.EmployeeID][pair.IOPairDate.Date];

                            List<EmployeeTimeScheduleTO> schList = new List<EmployeeTimeScheduleTO>();
                            if (emplSchedules.ContainsKey(pair.EmployeeID))
                                schList = emplSchedules[pair.EmployeeID];

                            bool prevDayPair = Common.Misc.isPreviousDayPair(pair, PassTypes, dayPairs, Common.Misc.getTimeSchema(pair.IOPairDate.Date, schList, schemas), Common.Misc.getTimeSchemaInterval(pair.IOPairDate.Date, schList, schemas));

                            if ((pair.IOPairDate.Date.Equals(firstDay.Date) && prevDayPair) || (pair.IOPairDate.Date.Equals(lastDay.Date) && !prevDayPair))
                                continue;
                        }

                        if (!emplDayPairs.ContainsKey(pair.EmployeeID))
                            emplDayPairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());

                        if (!emplDayPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                            emplDayPairs[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<IOPairProcessedTO>());

                        emplDayPairs[pair.EmployeeID][pair.IOPairDate.Date].Add(pair);

                        if (pair.VerificationFlag == (int)Constants.Verification.NotVerified)
                        {
                            if (!emplDayPairsToVerify.ContainsKey(pair.EmployeeID))
                                emplDayPairsToVerify.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());

                            if (!emplDayPairsToVerify[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                                emplDayPairsToVerify[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<IOPairProcessedTO>());

                            emplDayPairsToVerify[pair.EmployeeID][pair.IOPairDate.Date].Add(pair);
                        }
                        else if (!forVerification && ((!pair.VerifiedBy.Trim().Equals("") && !pair.VerifiedTime.Equals(new DateTime())) || pair.PassTypeID == Constants.absence))
                        {
                            if (!pair.VerifiedBy.Trim().Equals("") && !pair.VerifiedTime.Equals(new DateTime()))
                            {
                                if (!emplDayVerifiedPairs.ContainsKey(pair.EmployeeID))
                                    emplDayVerifiedPairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());

                                if (!emplDayVerifiedPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                                    emplDayVerifiedPairs[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<IOPairProcessedTO>());

                                emplDayVerifiedPairs[pair.EmployeeID][pair.IOPairDate.Date].Add(pair);

                                if (!emplDayUnverifiedPairs.ContainsKey(pair.EmployeeID))
                                    emplDayUnverifiedPairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairsProcessedHistTO>>());

                                if (!emplDayUnverifiedPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                                {
                                    if (histPairs.ContainsKey(pair.EmployeeID) && histPairs[pair.EmployeeID].ContainsKey(pair.CreatedTime)
                                        && histPairs[pair.EmployeeID][pair.CreatedTime].ContainsKey(pair.CreatedBy))
                                        emplDayUnverifiedPairs[pair.EmployeeID].Add(pair.IOPairDate.Date, histPairs[pair.EmployeeID][pair.CreatedTime][pair.CreatedBy]);
                                }
                            }

                            if (pair.PassTypeID == Constants.absence)
                            {
                                // check if there is corresponding unverified day set in hist table
                                if (histPairs.ContainsKey(pair.EmployeeID) && histPairs[pair.EmployeeID].ContainsKey(pair.CreatedTime)
                                    && histPairs[pair.EmployeeID][pair.CreatedTime].ContainsKey(pair.CreatedBy))
                                {
                                    List<IOPairsProcessedHistTO> histVerifiedPairs = histPairs[pair.EmployeeID][pair.CreatedTime][pair.CreatedBy];
                                    bool unverifiedSet = false;
                                    foreach (IOPairsProcessedHistTO histPair in histVerifiedPairs)
                                    {
                                        if (histPair.VerificationFlag == (int)Constants.Verification.NotVerified && pair.IOPairDate.Date.Equals(histPair.IOPairDate.Date)
                                            && pair.StartTime.Equals(histPair.StartTime) && pair.EndTime.Equals(histPair.EndTime))
                                        {
                                            unverifiedSet = true;
                                            break;
                                        }
                                    }

                                    if (unverifiedSet)
                                    {
                                        if (!emplDayVerifiedPairs.ContainsKey(pair.EmployeeID))
                                            emplDayVerifiedPairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());

                                        if (!emplDayVerifiedPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                                            emplDayVerifiedPairs[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<IOPairProcessedTO>());

                                        emplDayVerifiedPairs[pair.EmployeeID][pair.IOPairDate.Date].Add(pair);

                                        if (!emplDayUnverifiedPairs.ContainsKey(pair.EmployeeID))
                                            emplDayUnverifiedPairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairsProcessedHistTO>>());

                                        if (!emplDayUnverifiedPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                                        {
                                            emplDayUnverifiedPairs[pair.EmployeeID].Add(pair.IOPairDate.Date, histVerifiedPairs);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    foreach (EmployeeTO employee in emplList)
                    {
                        int emplCompany = Common.Misc.getRootWorkingUnit(employee.WorkingUnitID, WUnits);

                        if (employee.EmployeeID != -1)
                        {
                            // get employee hiring and termination dates
                            DateTime emplHiringDate = new DateTime();
                            DateTime emplTerminationDate = new DateTime();

                            if (emplAddData.ContainsKey(employee.EmployeeID))
                            {
                                emplHiringDate = emplAddData[employee.EmployeeID].DatetimeValue2;

                                if (employee.Status.Trim().ToUpper().Equals(Constants.statusRetired.Trim().ToUpper()))
                                    emplTerminationDate = emplAddData[employee.EmployeeID].DatetimeValue3;
                            }

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

                            List<EmployeeTimeScheduleTO> timeScheduleList = new List<EmployeeTimeScheduleTO>();
                            if (emplSchedules.ContainsKey(employee.EmployeeID))
                                timeScheduleList = emplSchedules[employee.EmployeeID];

                            DateTime currDay = new DateTime(firstDay.Year, firstDay.Month, firstDay.Day, 0, 0, 0);

                            bool headerAdded = false;
                            
                            // get employee cut off date                            
                            int cutOffDate = -1;
                            if (emplCompany != -1 && cuttOffDays.ContainsKey(emplCompany) && cuttOffDays[emplCompany].ContainsKey(employee.EmployeeTypeID)
                                && cuttOffDays[emplCompany][employee.EmployeeTypeID].ContainsKey(Constants.RuleCutOffDate))
                                cutOffDate = cuttOffDays[emplCompany][employee.EmployeeTypeID][Constants.RuleCutOffDate].RuleValue;

                            while (currDay.Date <= lastDay.Date)
                            {
                                List<IOPairProcessedTO> ioPairsForDay = new List<IOPairProcessedTO>();
                                List<IOPairProcessedTO> ioPairsForDayToVerify = new List<IOPairProcessedTO>();
                                List<IOPairProcessedTO> ioPairsVerifiedDay = new List<IOPairProcessedTO>();
                                List<IOPairsProcessedHistTO> ioPairsUnverifiedDay = new List<IOPairsProcessedHistTO>();

                                if (emplDayPairs.ContainsKey(employee.EmployeeID) && emplDayPairs[employee.EmployeeID].ContainsKey(currDay.Date))
                                    ioPairsForDay = emplDayPairs[employee.EmployeeID][currDay.Date];

                                if (emplDayPairsToVerify.ContainsKey(employee.EmployeeID) && emplDayPairsToVerify[employee.EmployeeID].ContainsKey(currDay.Date))
                                    ioPairsForDayToVerify = emplDayPairsToVerify[employee.EmployeeID][currDay.Date];

                                if (emplDayVerifiedPairs.ContainsKey(employee.EmployeeID) && emplDayVerifiedPairs[employee.EmployeeID].ContainsKey(currDay.Date))
                                    ioPairsVerifiedDay = emplDayVerifiedPairs[employee.EmployeeID][currDay.Date];

                                if (emplDayUnverifiedPairs.ContainsKey(employee.EmployeeID) && emplDayUnverifiedPairs[employee.EmployeeID].ContainsKey(currDay.Date))
                                    ioPairsUnverifiedDay = emplDayUnverifiedPairs[employee.EmployeeID][currDay.Date];

                                if (forVerification && ioPairsForDayToVerify.Count <= 0)
                                {
                                    currDay = currDay.AddDays(1);
                                    continue;
                                }

                                if (!forVerification && ioPairsVerifiedDay.Count <= 0)
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

                                List<IOPairProcessedTO> showPairs = new List<IOPairProcessedTO>();
                                if (forVerification)
                                    showPairs = ioPairsForDayToVerify;
                                else
                                    showPairs = ioPairsVerifiedDay;

                                foreach (IOPairProcessedTO iopair in showPairs)
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
                                emplDay.DayPairList = showPairs;
                                if (!forVerification)
                                    emplDay.DayUnverifiedPairList = ioPairsUnverifiedDay;
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
                                emplDay.EmplTimeSchema = schema;
                                emplDay.PostBackCtrlID = btnShow.ID;

                                if (!emplHiringDate.Equals(new DateTime()) && emplHiringDate.Date <= currDay.Date && (emplTerminationDate.Equals(new DateTime()) || emplTerminationDate >= currDay.Date))
                                {
                                    // get number of working days
                                    int workingDaysNum = Common.Misc.countWorkingDays(DateTime.Now.Date, Session[Constants.sessionConnection]);

                                    if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                                        && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRSSC)
                                    {
                                        if (currDay.Date >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0))
                                        {
                                            if (forVerification)
                                                emplDay.AllowVerify = true;
                                            else if (CommonWeb.Misc.isUserChangedDay(ioPairsUnverifiedDay, CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser])))
                                                emplDay.AllowUndoVerify = true;
                                        }
                                        else if (hrsscCutoffDate != -1 && workingDaysNum <= hrsscCutoffDate && currDay.Date >= new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1, 0, 0, 0))
                                        {
                                            if (forVerification)
                                                emplDay.AllowVerify = true;
                                            else if (CommonWeb.Misc.isUserChangedDay(ioPairsUnverifiedDay, CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser])))
                                                emplDay.AllowUndoVerify = true;
                                        }
                                    }
                                    else if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                                        && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.WCManager)
                                    {
                                        if (currDay.Date >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0))
                                        {
                                            if (!CommonWeb.Misc.isVerifiedConfirmedDay(ioPairsForDay, CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser])))
                                            {
                                                if (forVerification)
                                                    emplDay.AllowVerify = true;
                                                else if (CommonWeb.Misc.isUserChangedDay(ioPairsUnverifiedDay, CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser])))
                                                    emplDay.AllowUndoVerify = true;
                                            }
                                        }
                                        else if (wcdrCutoffDate != -1 && workingDaysNum <= wcdrCutoffDate && currDay.Date >= new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1, 0, 0, 0)
                                            && !CommonWeb.Misc.isVerifiedConfirmedDay(ioPairsForDay, CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser])))
                                            if (forVerification)
                                                emplDay.AllowVerify = true;
                                            else if (CommonWeb.Misc.isUserChangedDay(ioPairsUnverifiedDay, CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser])))
                                                emplDay.AllowUndoVerify = true;
                                    }

                                    if (emplDay.AllowVerify)
                                    {
                                        foreach (IOPairProcessedTO pair in ioPairsForDayToVerify)
                                        {
                                            // check if it belongs to previous day
                                            if (Session[Constants.sessionLoginCategoryPassTypes] != null && Session[Constants.sessionLoginCategoryPassTypes] is List<int>
                                                && ((List<int>)Session[Constants.sessionLoginCategoryPassTypes]).Contains(pair.PassTypeID))
                                                selectedPairs.Value += pair.RecID.ToString().Trim() + ",";
                                        }
                                    }
                                }
                                else
                                    emplDay.AllowChange = emplDay.AllowConfirm = emplDay.AllowUndoVerify = emplDay.AllowVerify = false;

                                ctrlHolder.Controls.Add(emplDay);

                                currDay = currDay.AddDays(1);
                                currentIndex++;
                            }
                        }
                    }

                    if (selectedPairs.Value.Trim().Length > 0)
                        selectedPairs.Value = selectedPairs.Value.Substring(0, selectedPairs.Value.Length - 1);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnVerifyAll_Click(Object sender, EventArgs e)
        {
            try
            {
                // if system is closed, sign out and redirect to login page
                if (Common.Misc.getClosingEvent(DateTime.Now, Session[Constants.sessionConnection]).EventID != -1)
                    CommonWeb.Misc.LogOut(Session[Constants.sessionLogInUserLog], Session[Constants.sessionLogInUser], Session[Constants.sessionConnection], Session, Response);

                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCManagerVerificationPage).Assembly);

                lblError.Text = "";
                
                if (selectedPairs.Value.Trim().Length <= 0)
                    lblError.Text = rm.GetString("noPairsToVerify", culture);
                else
                {
                    // get pairs by day to be moved to hist table
                    Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> pairsToVerifyEmplDaySet = new IOPairProcessed(Session[Constants.sessionConnection]).SearchPairsToVerifyEmplDaySet(selectedPairs.Value);

                    // create new pairs by day to be inserted
                    Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> verifiedPairsDaySet = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();

                    DateTime modifiedTime = DateTime.Now;
                    string histRecIDs = "";
                    foreach (int emplID in pairsToVerifyEmplDaySet.Keys)
                    {
                        if (!verifiedPairsDaySet.ContainsKey(emplID))
                            verifiedPairsDaySet.Add(emplID, new Dictionary<DateTime, List<IOPairProcessedTO>>());

                        foreach (DateTime date in pairsToVerifyEmplDaySet[emplID].Keys)
                        {
                            if (!verifiedPairsDaySet[emplID].ContainsKey(date))
                                verifiedPairsDaySet[emplID].Add(date, new List<IOPairProcessedTO>());

                            foreach (IOPairProcessedTO pairTO in pairsToVerifyEmplDaySet[emplID][date])
                            {
                                histRecIDs += pairTO.RecID.ToString().Trim() + ",";

                                IOPairProcessedTO newPair = new IOPairProcessedTO(pairTO);
                                newPair.CreatedTime = modifiedTime;

                                if (selectedPairs.Value.Contains(newPair.RecID.ToString()))
                                {
                                    newPair.VerificationFlag = (int)Constants.Verification.Verified;
                                    newPair.VerifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                                    newPair.VerifiedTime = modifiedTime;
                                }

                                verifiedPairsDaySet[emplID][date].Add(newPair);
                            }
                        }
                    }

                    if (histRecIDs.Length > 0)
                        histRecIDs = histRecIDs.Substring(0, histRecIDs.Length - 1);

                    IOPairProcessed pair = new IOPairProcessed(Session[Constants.sessionConnection]);
                    IOPairsProcessedHist pairHist = new IOPairsProcessedHist(Session[Constants.sessionConnection]);

                    if (pair.BeginTransaction())
                    {
                        string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                        DebugLog log = new DebugLog(logFilePath);

                        try
                        {
                            // save pairs from session, move all day pairs to hist table with alert value of 1, insert new pairs that last more then 0min
                            bool saved = true;
                            if (histRecIDs.Length > 0)
                            {
                                // move old pairs to hist table with alert value of 1
                                pairHist.SetTransaction(pair.GetTransaction());
                                saved = saved && (pairHist.Save(histRecIDs, CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]), modifiedTime, Constants.alertStatus, false) >= 0);
                                                                
                                if (saved)
                                    saved = saved && pair.Delete(histRecIDs, false);

                                if (saved)
                                {
                                    foreach (int emplID in verifiedPairsDaySet.Keys)
                                    {
                                        foreach (DateTime date in verifiedPairsDaySet[emplID].Keys)
                                        {
                                            foreach (IOPairProcessedTO pairTO in verifiedPairsDaySet[emplID][date])
                                            {
                                                if ((int)pairTO.EndTime.Subtract(pairTO.StartTime).TotalMinutes > 0)
                                                {
                                                    pair.IOPairProcessedTO = pairTO;
                                                    pair.IOPairProcessedTO.CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                                                    pair.IOPairProcessedTO.CreatedTime = modifiedTime;
                                                    saved = saved && (pair.Save(false) >= 0);

                                                    if (!saved)
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (saved)
                                {
                                    pair.CommitTransaction();

                                    lblError.Text = rm.GetString("pairsVerified", culture);
                                    selectedPairs.Value = "";
                                    btnShow_Click(this, new EventArgs());
                                }
                                else
                                {
                                    pair.RollbackTransaction();
                                    lblError.Text = rm.GetString("pairsNotVerified", culture);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (pair.GetTransaction() != null)
                                pair.RollbackTransaction();

                            log.writeLog(DateTime.Now + " " + ex.Message.Trim() + "\n");
                            lblError.Text = rm.GetString("pairsNotVerified", culture);
                        }
                    }
                    else
                        lblError.Text = rm.GetString("pairsNotVerified", culture);                    
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCManagerVerificationPage.btnVerifyAll_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCManagerVerificationPage.aspx", false);
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

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "WCManagerVerificationPage.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "WCManagerVerificationPage.", filterState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
