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
    public partial class MCVisitsSearchPage : System.Web.UI.Page
    {
        const string pageName = "MCVisitsSearchPage";

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
                    btnFromDate.Attributes.Add("onclick", "return calendarPicker('tbFromDate', 'true');");
                    btnToDate.Attributes.Add("onclick", "return calendarPicker('tbToDate', 'true');");
                    btnWUTree.Attributes.Add("onclick", "return wUnitsPicker('btnWUTree', 'false');");
                    btnOrgTree.Attributes.Add("onclick", "return oUnitsPicker('btnOrgTree', 'false');");
                    tbEmployee.Attributes.Add("onKeyUp", "return selectItem('tbEmployee', 'lboxEmployees');");
                    btnShow.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnReport.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnPersonalData.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");

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

                    InitializeSQLParametersVisits();

                    populateStatus();
                    populatePoints();

                    rbVisits.Checked = true;
                    rbVisits_CheckedChanged(this, new EventArgs());

                    rbRisks.Checked = true;
                    rbRisks_CheckedChanged(this, new EventArgs());

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

                        rbVisits_CheckedChanged(this, new EventArgs());
                        rbRisks_CheckedChanged(this, new EventArgs());
                        rbVaccines_CheckedChanged(this, new EventArgs());
                        rbDisabilities_CheckedChanged(this, new EventArgs());

                        // do again load state to select previously selected employees in employees list                        
                        LoadState();
                    }

                    resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?showSelection=false&height=360";
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCVisitsSearchPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnPrevDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCVisitsSearchPage).Assembly);

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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCVisitsSearchPage.btnPrevDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCVisitsSearchPage.aspx&Header=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnNextDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCVisitsSearchPage).Assembly);

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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCVisitsSearchPage.btnNextDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCVisitsSearchPage.aspx&Header=" + Constants.trueValue.Trim(), false);
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCVisitsSearchPage.Date_Changed(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCVisitsSearchPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void populateEmployees(int ID, bool isWU)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCVisitsSearchPage).Assembly);

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

                //For report purpose
                Session["Type"] = null;
                Session["RbVisits"] = null;
                Session["Status"] = null;
                Session["Ambulance"] = null;

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

        private void populateStatus()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCVisitsSearchPage).Assembly);

                List<VisitStatus> statuses = new List<VisitStatus>();
                VisitStatus wrStatus = new VisitStatus(rm.GetString(Constants.MedicalCheckVisitStatus.WR.ToString(), culture), Constants.MedicalCheckVisitStatus.WR.ToString());
                VisitStatus rndStatus = new VisitStatus(rm.GetString(Constants.MedicalCheckVisitStatus.RND.ToString(), culture), Constants.MedicalCheckVisitStatus.RND.ToString());
                VisitStatus doneStatus = new VisitStatus(rm.GetString(Constants.MedicalCheckVisitStatus.DONE.ToString(), culture), Constants.MedicalCheckVisitStatus.DONE.ToString());
                VisitStatus demandedStatus = new VisitStatus(rm.GetString(Constants.MedicalCheckVisitStatus.DEMANDED.ToString(), culture), Constants.MedicalCheckVisitStatus.DEMANDED.ToString());

                statuses.Add(wrStatus);
                statuses.Add(rndStatus);
                statuses.Add(doneStatus);
                statuses.Add(demandedStatus);

                if (chbDeleted.Checked)
                {
                    VisitStatus deletedStatus = new VisitStatus(rm.GetString(Constants.MedicalCheckVisitStatus.DELETED.ToString(), culture), Constants.MedicalCheckVisitStatus.DELETED.ToString());
                    statuses.Add(deletedStatus);
                }

                lbStatus.DataSource = statuses;
                lbStatus.DataTextField = "Desc";
                lbStatus.DataValueField = "Status";
                lbStatus.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populatePoints()
        {
            try
            {
                List<MedicalCheckPointTO> points = new MedicalCheckPoint(Session[Constants.sessionConnection]).SearchMedicalCheckPoints();

                lbPoint.DataSource = points;
                lbPoint.DataTextField = "Desc";
                lbPoint.DataValueField = "PointID";
                lbPoint.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void lbAddData_PreRender(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem item in lbAddData.Items)
                {
                    item.Attributes.Add("title", item.Text);
                }
            }
            catch { }
        }

        private void populateAddData()
        {
            try
            {
                int company = -1;
                int ID = -1;
                bool isWU = true;
                if (rbRisks.Checked || rbDisabilities.Checked)
                {
                    if (Menu1.SelectedItem.Value.Equals("0"))
                    {
                        if (tbUte.Attributes["id"] != null)
                            if (!int.TryParse(tbUte.Attributes["id"].Trim(), out ID))
                                ID = -1;
                        isWU = true;
                    }
                    else
                    {
                        if (tbOrgUte.Attributes["id"] != null)
                            if (!int.TryParse(tbOrgUte.Attributes["id"].Trim(), out ID))
                                ID = -1;
                        isWU = false;
                    }

                    Dictionary<int, WorkingUnitTO> wuDict = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();

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
                }

                bool isAltLang = !CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper());

                if (rbRisks.Checked)
                {
                    Risk risk = new Risk(Session[Constants.sessionConnection]);
                    risk.RiskTO.WorkingUnitID = company;
                    List<RiskTO> risks = risk.SearchRisks();

                    lbAddData.DataSource = risks;
                    if (!isAltLang)                    
                        lbAddData.DataTextField = "RiskCodeDescSR";
                    else
                        lbAddData.DataTextField = "RiskCodeDescEN";
                    lbAddData.DataValueField = "RiskID";
                    
                    lbAddData.DataBind();
                }
                else if (rbVaccines.Checked)
                {
                    List<VaccineTO> vaccines = new Vaccine(Session[Constants.sessionConnection]).SearchVaccines();

                    lbAddData.DataSource = vaccines;
                    if (isAltLang)
                        lbAddData.DataTextField = "VaccineTypeDescSR";
                    else
                        lbAddData.DataTextField = "VaccineTypeDescEN";
                    lbAddData.DataValueField = "VaccineID";
                    lbAddData.DataBind();
                }
                else if (rbDisabilities.Checked)
                {
                    MedicalCheckDisability disability = new MedicalCheckDisability(Session[Constants.sessionConnection]);
                    disability.DisabilityTO.WorkingUnitID = company;
                    List<MedicalCheckDisabilityTO> disabilities = disability.SearchMedicalCheckDisabilities();

                    lbAddData.DataSource = disabilities;
                    if (!isAltLang)
                        lbAddData.DataTextField = "DisabilityCodeDescSR";
                    else
                        lbAddData.DataTextField = "DisabilityCodeDescEN";
                    lbAddData.DataValueField = "DisabilityID";
                    lbAddData.DataBind();
                }

                foreach (ListItem item in lbAddData.Items)
                {
                    item.Attributes.Add("title", item.Text);
                }
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCVisitsSearchPage).Assembly);

                lblEmployee.Text = rm.GetString("lblEmployeeSelection", culture);
                lblPeriod.Text = rm.GetString("lblPeriod", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblStatus.Text = rm.GetString("lblStatus", culture);
                lblPoint.Text = rm.GetString("lblAmbulance", culture);

                chbDeleted.Text = rm.GetString("chbDeleted", culture);

                btnShow.Text = rm.GetString("btnShow", culture);
                btnReport.Text = rm.GetString("btnReport", culture);
                btnPersonalData.Text = rm.GetString("btnPersonalData", culture);

                rbVisits.Text = rm.GetString("rbVisits", culture);
                rbEmployeeData.Text = rm.GetString("rbEmployeeData", culture);
                rbRisks.Text = rm.GetString("rbRisks", culture);
                rbVaccines.Text = rm.GetString("rbVaccines", culture);
                rbDisabilities.Text = rm.GetString("rbDisabilities", culture);
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

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCVisitsSearchPage.Menu1_MenuItemClick(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCVisitsSearchPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void InitializeSQLParametersVisits()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCVisitsSearchPage).Assembly);

                Session[Constants.sessionHeader] = rm.GetString("hdrCompany", culture) + "," + rm.GetString("hdrCostCentre", culture) 
                    + "," + rm.GetString("hdrCostCenterName", culture) + "," + rm.GetString("hdrWorkgroup", culture)
                    + "," + rm.GetString("hdrUte", culture) + "," + rm.GetString("hdrBranch", culture)
                    + "," + rm.GetString("hdrEmplType", culture) + "," + rm.GetString("hdrID", culture) + "," + rm.GetString("hdrEmployee", culture)
                    + "," + rm.GetString("hdrShift", culture) + "," + rm.GetString("hdrInterval", culture) + "," + rm.GetString("hdrEmplRisk", culture)
                    + "," + rm.GetString("hdrAmbulance", culture) + "," + rm.GetString("hdrTermin", culture) + "," + rm.GetString("hdrDay", culture)
                    + "," + rm.GetString("hdrMonth", culture) + "," + rm.GetString("hdrYear", culture) + "," + rm.GetString("hdrStatus", culture);
                Session[Constants.sessionFields] = "company, costcenter, ccDesc, workgroup, ute, branch, type, employeeID, employee, shift, interval, risk, ambulance, termin, d, m, y, status";
                Session[Constants.sessionColTypes] = null;
                Dictionary<int, int> formating = new Dictionary<int, int>();
                formating.Add(13, (int)Constants.FormatTypes.TimeFormat);
                Session[Constants.sessionFieldsFormating] = formating;
                Session[Constants.sessionFieldsFormatedValues] = null;
                Session[Constants.sessionTables] = null;
                Session[Constants.sessionDataTableList] = null;
                Session[Constants.sessionDataTableColumns] = null;
                Session[Constants.sessionKey] = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeSQLParametersEmplData(string type)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCVisitsSearchPage).Assembly);

                string header = rm.GetString("hdrCompany", culture) + "," + rm.GetString("hdrCostCentre", culture) 
                    + "," + rm.GetString("hdrCostCenterName", culture) + "," + rm.GetString("hdrWorkgroup", culture)
                    + "," + rm.GetString("hdrUte", culture) + "," + rm.GetString("hdrBranch", culture)
                    + "," + rm.GetString("hdrEmplType", culture) + "," + rm.GetString("hdrID", culture) + "," + rm.GetString("hdrEmployee", culture);
                string fileds = "company, costcenter, ccDesc, workgroup, ute, branch, type, employeeID, employee";
                Dictionary<int, int> formating = new Dictionary<int, int>();
                Dictionary<int, Dictionary<string, string>> values = new Dictionary<int, Dictionary<string, string>>();
                if (type == Constants.VisitType.R.ToString())
                {
                    header += "," + rm.GetString("hdrEmplRisk", culture) + "," + rm.GetString("hdrStartTime", culture) + "," + rm.GetString("hdrEndTime", culture);
                    fileds += ", risk, dateStart, dateEnd";
                    formating.Add(10, (int)Constants.FormatTypes.DateFormat);
                    formating.Add(11, (int)Constants.FormatTypes.DateFormat);
                }
                else if (type == Constants.VisitType.V.ToString())
                {
                    header += "," + rm.GetString("hdrVaccine", culture) + "," + rm.GetString("hdrDate", culture);
                    fileds += ", vaccine, date";
                    formating.Add(10, (int)Constants.FormatTypes.DateFormat);
                }
                else if (type == Constants.VisitType.D.ToString())
                {
                    header += "," + rm.GetString("hdrDisability", culture) + "," + rm.GetString("hdrDisabilityType", culture) + "," + rm.GetString("hdrStartTime", culture) + "," + rm.GetString("hdrEndTime", culture);
                    fileds += ", disability, disabilityType, dateStart, dateEnd";
                    formating.Add(11, (int)Constants.FormatTypes.DateFormat);
                    formating.Add(12, (int)Constants.FormatTypes.DateFormat);
                    
                    Dictionary<string, string> formatValues = new Dictionary<string, string>();
                    formatValues.Add(Constants.MedicalCheckDisabilityType.PERMANENT.ToString(), rm.GetString(Constants.MedicalCheckDisabilityType.PERMANENT.ToString(), culture));
                    formatValues.Add(Constants.MedicalCheckDisabilityType.TEMPORARY.ToString(), rm.GetString(Constants.MedicalCheckDisabilityType.TEMPORARY.ToString(), culture));
                    values.Add(10, formatValues);
                }

                Session[Constants.sessionHeader] = header;
                Session[Constants.sessionFields] = fileds;
                Session[Constants.sessionColTypes] = null;
                Session[Constants.sessionFieldsFormating] = formating;
                if (values.Count > 0)
                    Session[Constants.sessionFieldsFormatedValues] = values;
                else
                    Session[Constants.sessionFieldsFormatedValues] = null;
                Session[Constants.sessionTables] = null;
                Session[Constants.sessionDataTableList] = null;
                Session[Constants.sessionDataTableColumns] = null;
                Session[Constants.sessionKey] = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void chbDeleted_OnCheckChanged(object sender, EventArgs e)
        {
            try
            {
                populateStatus();

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCVisitsSearchPage.chbDeleted_OnCheckChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCVisitsSearchPage.aspx&Header=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnShow_Click(Object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCVisitsSearchPage).Assembly);

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

                // get all working units
                Dictionary<int, WorkingUnitTO> WUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();

                // get all medical check points
                Dictionary<int, MedicalCheckPointTO> mcPointDict = new MedicalCheckPoint(Session[Constants.sessionConnection]).SearchMedicalCheckPointsDictionary();

                // get all risks
                Dictionary<int, RiskTO> riskDict = new Risk(Session[Constants.sessionConnection]).SearchRisksDictionary();

                // get all vaccines
                Dictionary<int, VaccineTO> vaccineDict = new Vaccine(Session[Constants.sessionConnection]).SearchVaccinesDictionary();

                // get all employee types, key is employee_type, value name for that company
                Dictionary<int, Dictionary<int, string>> emplTypes = new EmployeeType(Session[Constants.sessionConnection]).SearchDictionary();

                // get selected employees and additional data for branch and stringone
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

                Dictionary<int, EmployeeTO> employees = new Employee(Session[Constants.sessionConnection]).SearchDictionary(emplIDs);

                // stringone, company
                Dictionary<int, EmployeeAsco4TO> emplAscoList = new EmployeeAsco4(Session[Constants.sessionConnection]).SearchDictionary(emplIDs);
                Dictionary<int, string> stringoneList = new Dictionary<int, string>();
                Dictionary<int, string> compList = new Dictionary<int, string>();
                Dictionary<int, string> emplTypeList = new Dictionary<int, string>();

                //costcenter, ute, workgruop, branch                
                Dictionary<int, string> branchList = new Dictionary<int, string>();
                Dictionary<int, string> uteList = new Dictionary<int, string>();
                Dictionary<int, string> workgroupList = new Dictionary<int, string>();
                Dictionary<int, string> costcenterList = new Dictionary<int, string>();
                Dictionary<int, string> costcenterListDesc = new Dictionary<int, string>();

                foreach (int emplID in employees.Keys)
                {
                    EmployeeAsco4TO asco = new EmployeeAsco4TO();

                    if (emplAscoList.ContainsKey(emplID))
                        asco = emplAscoList[emplID];

                    if (!stringoneList.ContainsKey(asco.EmployeeID))
                        stringoneList.Add(asco.EmployeeID, asco.NVarcharValue2.Trim());
                    else
                        stringoneList[asco.EmployeeID] = asco.NVarcharValue2.Trim();

                    if (!compList.ContainsKey(asco.EmployeeID))
                        compList.Add(asco.EmployeeID, "");

                    if (WUnits.ContainsKey(asco.IntegerValue4))
                        compList[asco.EmployeeID] = WUnits[asco.IntegerValue4].Description.Trim();

                    // get employee company
                    int emplCompany = asco.IntegerValue4;

                    // get employee type name
                    if (!emplTypeList.ContainsKey(emplID))
                        emplTypeList.Add(emplID, "");

                    if (emplTypes.ContainsKey(emplCompany) && emplTypes[emplCompany].ContainsKey(employees[emplID].EmployeeTypeID))
                        emplTypeList[emplID] = emplTypes[emplCompany][employees[emplID].EmployeeTypeID];

                    EmployeeTO empl = employees[emplID];

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

                string type = "";
                if (rbRisks.Checked)
                    type = Constants.VisitType.R.ToString();
                else if (rbVaccines.Checked)
                    type = Constants.VisitType.V.ToString();
                else if (rbDisabilities.Checked)
                    type = Constants.VisitType.D.ToString();

                string data = "";
                if (lbAddData.GetSelectedIndices().Length > 0)
                {
                    foreach (int index in lbAddData.GetSelectedIndices())
                    {
                        if (index >= 0 && index < lbAddData.Items.Count)
                        {
                            data += lbAddData.Items[index].Value.Trim() + ",";
                        }
                    }

                    if (data.Length > 0)
                        data = data.Substring(0, data.Length - 1);
                }

                if (rbVisits.Checked)
                    showVisits(emplIDs, employees, compList, stringoneList, emplTypeList, branchList, uteList, workgroupList, costcenterList, costcenterListDesc, type, data, riskDict, vaccineDict, mcPointDict, fromDate, toDate, culture, rm);
                else
                    showEmplData(emplIDs, employees, compList, stringoneList, emplTypeList, branchList, uteList, workgroupList, costcenterList, costcenterListDesc, type, data, riskDict, vaccineDict, fromDate, toDate);

                //For report purpose
                Session["Type"] = type;
                Session["RbVisits"] = rbVisits.Checked;
                string status = "*";

                if (lbStatus.GetSelectedIndices().Length > 0)
                {
                    if (lbStatus.GetSelectedIndices().Length <= 3)
                    {


                        foreach (int statusIndex in lbStatus.GetSelectedIndices())
                        {
                            if (statusIndex >= 0 && statusIndex < lbStatus.Items.Count)
                            {
                                status += "'" + lbStatus.Items[statusIndex].Text.Trim() + "',";
                            }
                        }

                        if (status.Length > 0)
                            status = status.Substring(0, status.Length - 1);
                    }
                    else
                        status = "*";
                }
                Session["Status"] = status;
                string point = "*";

                if (lbPoint.GetSelectedIndices().Length > 0)
                {
                    if (lbPoint.GetSelectedIndices().Length <= 3)
                    {
                        foreach (int pointIndex in lbPoint.GetSelectedIndices())
                        {
                            if (pointIndex >= 0 && pointIndex < lbPoint.Items.Count)
                            {
                                point += lbPoint.Items[pointIndex].Text.Trim() + ",";
                            }
                        }

                        if (point.Length > 0)
                            point = point.Substring(0, point.Length - 1);
                    }
                    else point = "*";
                }
                Session["Ambulance"] = point;

                resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?showSelection=false&height=360";

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCVisitsSearchPage.btnShow_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCVisitsSearchPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
        
        private void showVisits(string emplIDs, Dictionary<int, EmployeeTO> employees, Dictionary<int, string> compList, Dictionary<int, string> stringoneList,
            Dictionary<int, string> emplTypeList, Dictionary<int, string> branchList, Dictionary<int, string> uteList, Dictionary<int, string> workgroupList,
            Dictionary<int, string> costcenterList, Dictionary<int, string> costcenterListDesc,
            string type, string data, Dictionary<int, RiskTO> riskDict, Dictionary<int, VaccineTO> vaccineDict,
            Dictionary<int, MedicalCheckPointTO> mcPointDict, DateTime fromDate, DateTime toDate, CultureInfo culture, ResourceManager rm)
        {
            try
            {
                InitializeSQLParametersVisits();

                // get all time schemas
                Dictionary<int, WorkTimeSchemaTO> schemas = new TimeSchema(Session[Constants.sessionConnection]).getDictionary();

                // get schedules
                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule(Session[Constants.sessionConnection]).SearchEmployeesSchedulesExactDate(emplIDs, fromDate.Date, toDate.Date, null);

                // get selected statuses
                string status = "";
                if (lbStatus.GetSelectedIndices().Length > 0)
                {
                    foreach (int statusIndex in lbStatus.GetSelectedIndices())
                    {
                        if (statusIndex >= 0 && statusIndex < lbStatus.Items.Count)
                        {
                            status += "'" + lbStatus.Items[statusIndex].Value.Trim() + "',";
                        }
                    }
                }
                else
                {
                    foreach (ListItem item in lbStatus.Items)
                    {
                        status += "'" + item.Value.Trim() + "',";
                    }
                }

                if (status.Length > 0)
                    status = status.Substring(0, status.Length - 1);

                // get selected points
                string point = "";
                if (lbPoint.GetSelectedIndices().Length > 0)
                {
                    foreach (int pointIndex in lbPoint.GetSelectedIndices())
                    {
                        if (pointIndex >= 0 && pointIndex < lbPoint.Items.Count)
                        {
                            point += lbPoint.Items[pointIndex].Value.Trim() + ",";
                        }
                    }

                    if (point.Length > 0)
                        point = point.Substring(0, point.Length - 1);
                }

                Dictionary<uint, MedicalCheckVisitHdrTO> visitDict = new MedicalCheckVisitHdr(Session[Constants.sessionConnection]).SearchMedicalCheckVisits(emplIDs, status, point, data, type, fromDate.Date, toDate.Date);

                List<List<object>> resultTable = new List<List<object>>();

                // create list of columns for list preview of pairs for inserting
                List<DataColumn> resultColumns = new List<DataColumn>();
                resultColumns.Add(new DataColumn("company", typeof(string)));
                resultColumns.Add(new DataColumn("costcenter", typeof(string)));
                resultColumns.Add(new DataColumn("ccDesc", typeof(string)));
                resultColumns.Add(new DataColumn("workgroup", typeof(string)));
                resultColumns.Add(new DataColumn("ute", typeof(string)));
                resultColumns.Add(new DataColumn("branch", typeof(string)));
                resultColumns.Add(new DataColumn("type", typeof(string)));
                resultColumns.Add(new DataColumn("employeeID", typeof(int)));
                resultColumns.Add(new DataColumn("employee", typeof(string)));
                resultColumns.Add(new DataColumn("shift", typeof(int)));
                resultColumns.Add(new DataColumn("interval", typeof(string)));
                resultColumns.Add(new DataColumn("risk", typeof(string)));
                resultColumns.Add(new DataColumn("ambulance", typeof(string)));
                resultColumns.Add(new DataColumn("termin", typeof(DateTime)));
                resultColumns.Add(new DataColumn("d", typeof(int)));
                resultColumns.Add(new DataColumn("m", typeof(int)));
                resultColumns.Add(new DataColumn("y", typeof(int)));
                resultColumns.Add(new DataColumn("status", typeof(string)));

                foreach (uint visitID in visitDict.Keys)
                {
                    // create result row
                    List<object> resultRow = new List<object>();

                    if (compList.ContainsKey(visitDict[visitID].EmployeeID))
                        resultRow.Add(compList[visitDict[visitID].EmployeeID].Trim());
                    else
                        resultRow.Add("");
                    if (costcenterList.ContainsKey(visitDict[visitID].EmployeeID))
                        resultRow.Add(costcenterList[visitDict[visitID].EmployeeID].Trim());
                    else
                        resultRow.Add("");
                    if (costcenterListDesc.ContainsKey(visitDict[visitID].EmployeeID))
                        resultRow.Add(costcenterListDesc[visitDict[visitID].EmployeeID].Trim());
                    else
                        resultRow.Add("");
                    if (workgroupList.ContainsKey(visitDict[visitID].EmployeeID))
                        resultRow.Add(workgroupList[visitDict[visitID].EmployeeID].Trim());
                    else
                        resultRow.Add("");
                    if (uteList.ContainsKey(visitDict[visitID].EmployeeID))
                        resultRow.Add(uteList[visitDict[visitID].EmployeeID].Trim());
                    else
                        resultRow.Add("");
                    if (branchList.ContainsKey(visitDict[visitID].EmployeeID))
                        resultRow.Add(branchList[visitDict[visitID].EmployeeID].Trim());
                    else
                        resultRow.Add("");
                    if (emplTypeList.ContainsKey(visitDict[visitID].EmployeeID))
                        resultRow.Add(emplTypeList[visitDict[visitID].EmployeeID].Trim());
                    else
                        resultRow.Add("");
                    resultRow.Add(visitDict[visitID].EmployeeID);
                    if (employees.ContainsKey(visitDict[visitID].EmployeeID))
                        resultRow.Add(employees[visitDict[visitID].EmployeeID].FirstAndLastName.Trim());
                    else
                        resultRow.Add("");
                    List<EmployeeTimeScheduleTO> schList = new List<EmployeeTimeScheduleTO>();
                    if (emplSchedules.ContainsKey(visitDict[visitID].EmployeeID))
                        schList = emplSchedules[visitDict[visitID].EmployeeID];
                    int schID = Common.Misc.getTimeSchema(visitDict[visitID].ScheduleDate.Date, schList, schemas).TimeSchemaID;
                    if (schID != -1)
                        resultRow.Add(schID);
                    else
                        resultRow.Add(0);
                    List<WorkTimeIntervalTO> intervals = Common.Misc.getTimeSchemaInterval(visitDict[visitID].ScheduleDate.Date, schList, schemas);
                    string intervalsString = "";
                    foreach (WorkTimeIntervalTO interval in intervals)
                    {
                        intervalsString += interval.StartTime.ToString(Constants.timeFormat) + "-" + interval.EndTime.ToString(Constants.timeFormat) + Environment.NewLine;
                    }
                    if (intervalsString.Length > 0)
                        intervalsString = intervalsString.Substring(0, intervalsString.Length - 2);
                    resultRow.Add(intervalsString);
                    string risk = "";
                    foreach (MedicalCheckVisitDtlTO dtlTO in visitDict[visitID].VisitDetails)
                    {
                        if (dtlTO.Type.Trim().ToUpper().Equals(Constants.VisitType.R.ToString().Trim().ToUpper()))
                        {
                            if (riskDict.ContainsKey(dtlTO.CheckID))
                                risk += riskDict[dtlTO.CheckID].RiskCode.Trim() + Environment.NewLine;
                        }
                        else if (dtlTO.Type.Trim().ToUpper().Equals(Constants.VisitType.V.ToString().Trim().ToUpper()))
                        {
                            if (vaccineDict.ContainsKey(dtlTO.CheckID))
                                risk += vaccineDict[dtlTO.CheckID].VaccineType.Trim() + Environment.NewLine;
                        }
                    }
                    if (risk.Length > 0)
                        risk = risk.Substring(0, risk.Length - 2);
                    resultRow.Add(risk);
                    if (mcPointDict.ContainsKey(visitDict[visitID].PointID))
                        resultRow.Add(mcPointDict[visitDict[visitID].PointID].Desc.Trim());
                    else
                        resultRow.Add("");
                    resultRow.Add(visitDict[visitID].ScheduleDate);
                    resultRow.Add(visitDict[visitID].ScheduleDate.Day);
                    resultRow.Add(visitDict[visitID].ScheduleDate.Month);
                    resultRow.Add(visitDict[visitID].ScheduleDate.Year);
                    resultRow.Add(rm.GetString(visitDict[visitID].Status, culture));

                    resultTable.Add(resultRow);
                }

                Session[Constants.sessionDataTableColumns] = resultColumns;
                Session[Constants.sessionDataTableList] = resultTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void showEmplData(string emplIDs, Dictionary<int, EmployeeTO> employees, Dictionary<int, string> compList, Dictionary<int, string> stringoneList,
            Dictionary<int, string> emplTypeList, Dictionary<int, string> branchList, Dictionary<int, string> uteList, Dictionary<int, string> workgroupList,
            Dictionary<int, string> costcenterList, Dictionary<int, string> costcenterListDesc,
            string type, string data, Dictionary<int, RiskTO> riskDict, Dictionary<int, VaccineTO> vaccineDict, DateTime fromDate, DateTime toDate)
        {
            try
            {
                InitializeSQLParametersEmplData(type);

                List<List<object>> resultTable = new List<List<object>>();

                // create list of columns for list preview of pairs for inserting
                List<DataColumn> resultColumns = new List<DataColumn>();
                resultColumns.Add(new DataColumn("company", typeof(string)));
                resultColumns.Add(new DataColumn("costcenter", typeof(string)));
                resultColumns.Add(new DataColumn("ccDesc", typeof(string)));
                resultColumns.Add(new DataColumn("workgroup", typeof(string)));
                resultColumns.Add(new DataColumn("ute", typeof(string)));
                resultColumns.Add(new DataColumn("branch", typeof(string)));
                resultColumns.Add(new DataColumn("type", typeof(string)));
                resultColumns.Add(new DataColumn("employeeID", typeof(int)));
                resultColumns.Add(new DataColumn("employee", typeof(string)));

                if (type == Constants.VisitType.R.ToString())
                {
                    resultColumns.Add(new DataColumn("risk", typeof(string)));
                    resultColumns.Add(new DataColumn("dateStart", typeof(DateTime)));
                    resultColumns.Add(new DataColumn("dateEnd", typeof(DateTime)));

                    Dictionary<uint, EmployeeXRiskTO> emplRiskDict = new EmployeeXRisk(Session[Constants.sessionConnection]).SearchEmployeeXRisks(emplIDs, data, fromDate, toDate);

                    foreach (uint recID in emplRiskDict.Keys)
                    {
                        // create result row
                        List<object> resultRow = new List<object>();

                        if (compList.ContainsKey(emplRiskDict[recID].EmployeeID))
                            resultRow.Add(compList[emplRiskDict[recID].EmployeeID].Trim());
                        else
                            resultRow.Add("");
                        if (costcenterList.ContainsKey(emplRiskDict[recID].EmployeeID))
                            resultRow.Add(costcenterList[emplRiskDict[recID].EmployeeID].Trim());
                        else
                            resultRow.Add("");
                        if (costcenterListDesc.ContainsKey(emplRiskDict[recID].EmployeeID))
                            resultRow.Add(costcenterListDesc[emplRiskDict[recID].EmployeeID].Trim());
                        else
                            resultRow.Add("");
                        if (workgroupList.ContainsKey(emplRiskDict[recID].EmployeeID))
                            resultRow.Add(workgroupList[emplRiskDict[recID].EmployeeID].Trim());
                        else
                            resultRow.Add("");
                        if (uteList.ContainsKey(emplRiskDict[recID].EmployeeID))
                            resultRow.Add(uteList[emplRiskDict[recID].EmployeeID].Trim());
                        else
                            resultRow.Add("");
                        if (branchList.ContainsKey(emplRiskDict[recID].EmployeeID))
                            resultRow.Add(branchList[emplRiskDict[recID].EmployeeID].Trim());
                        else
                            resultRow.Add("");
                        if (emplTypeList.ContainsKey(emplRiskDict[recID].EmployeeID))
                            resultRow.Add(emplTypeList[emplRiskDict[recID].EmployeeID].Trim());
                        else
                            resultRow.Add("");
                        resultRow.Add(emplRiskDict[recID].EmployeeID);
                        if (employees.ContainsKey(emplRiskDict[recID].EmployeeID))
                            resultRow.Add(employees[emplRiskDict[recID].EmployeeID].FirstAndLastName.Trim());
                        else
                            resultRow.Add("");
                        if (riskDict.ContainsKey(emplRiskDict[recID].RiskID))
                            resultRow.Add(riskDict[emplRiskDict[recID].RiskID].RiskCode.Trim());
                        else
                            resultRow.Add("");
                        resultRow.Add(emplRiskDict[recID].DateStart.Date);
                        resultRow.Add(emplRiskDict[recID].DateEnd.Date);

                        resultTable.Add(resultRow);
                    }
                }
                else if (type == Constants.VisitType.V.ToString())
                {
                    resultColumns.Add(new DataColumn("vaccine", typeof(string)));
                    resultColumns.Add(new DataColumn("date", typeof(DateTime)));

                    Dictionary<uint, EmployeeXVaccineTO> emplVaccineDict = new EmployeeXVaccine(Session[Constants.sessionConnection]).SearchEmployeeXVaccines(emplIDs, data, fromDate, toDate);

                    foreach (uint recID in emplVaccineDict.Keys)
                    {
                        // create result row
                        List<object> resultRow = new List<object>();

                        if (compList.ContainsKey(emplVaccineDict[recID].EmployeeID))
                            resultRow.Add(compList[emplVaccineDict[recID].EmployeeID].Trim());
                        else
                            resultRow.Add("");
                        if (costcenterList.ContainsKey(emplVaccineDict[recID].EmployeeID))
                            resultRow.Add(costcenterList[emplVaccineDict[recID].EmployeeID].Trim());
                        else
                            resultRow.Add("");
                        if (costcenterListDesc.ContainsKey(emplVaccineDict[recID].EmployeeID))
                            resultRow.Add(costcenterListDesc[emplVaccineDict[recID].EmployeeID].Trim());
                        else
                            resultRow.Add("");
                        if (workgroupList.ContainsKey(emplVaccineDict[recID].EmployeeID))
                            resultRow.Add(workgroupList[emplVaccineDict[recID].EmployeeID].Trim());
                        else
                            resultRow.Add("");
                        if (uteList.ContainsKey(emplVaccineDict[recID].EmployeeID))
                            resultRow.Add(uteList[emplVaccineDict[recID].EmployeeID].Trim());
                        else
                            resultRow.Add("");
                        if (branchList.ContainsKey(emplVaccineDict[recID].EmployeeID))
                            resultRow.Add(branchList[emplVaccineDict[recID].EmployeeID].Trim());
                        else
                            resultRow.Add("");
                        if (emplTypeList.ContainsKey(emplVaccineDict[recID].EmployeeID))
                            resultRow.Add(emplTypeList[emplVaccineDict[recID].EmployeeID].Trim());
                        else
                            resultRow.Add("");
                        resultRow.Add(emplVaccineDict[recID].EmployeeID);
                        if (employees.ContainsKey(emplVaccineDict[recID].EmployeeID))
                            resultRow.Add(employees[emplVaccineDict[recID].EmployeeID].FirstAndLastName.Trim());
                        else
                            resultRow.Add("");
                        if (vaccineDict.ContainsKey(emplVaccineDict[recID].VaccineID))
                            resultRow.Add(vaccineDict[emplVaccineDict[recID].VaccineID].VaccineType.Trim());
                        else
                            resultRow.Add("");
                        resultRow.Add(emplVaccineDict[recID].DatePerformed.Date);

                        resultTable.Add(resultRow);
                    }
                }
                else if (type == Constants.VisitType.D.ToString())
                {
                    resultColumns.Add(new DataColumn("disability", typeof(string)));
                    resultColumns.Add(new DataColumn("disabilityType", typeof(string)));
                    resultColumns.Add(new DataColumn("dateStart", typeof(DateTime)));
                    resultColumns.Add(new DataColumn("dateEnd", typeof(DateTime)));

                    Dictionary<int, MedicalCheckDisabilityTO> disabilityDict = new MedicalCheckDisability(Session[Constants.sessionConnection]).SearchMedicalCheckDisabilitiesDictionary();

                    Dictionary<uint, EmployeeXMedicalCheckDisabilityTO> emplDisabilitiesDict = new EmployeeXMedicalCheckDisability(Session[Constants.sessionConnection]).SearchEmployeeXMedicalCheckDisabilities(emplIDs, data, fromDate, toDate);

                    foreach (uint recID in emplDisabilitiesDict.Keys)
                    {
                        // create result row
                        List<object> resultRow = new List<object>();

                        if (compList.ContainsKey(emplDisabilitiesDict[recID].EmployeeID))
                            resultRow.Add(compList[emplDisabilitiesDict[recID].EmployeeID].Trim());
                        else
                            resultRow.Add("");
                        if (costcenterList.ContainsKey(emplDisabilitiesDict[recID].EmployeeID))
                            resultRow.Add(costcenterList[emplDisabilitiesDict[recID].EmployeeID].Trim());
                        else
                            resultRow.Add("");
                        if (costcenterListDesc.ContainsKey(emplDisabilitiesDict[recID].EmployeeID))
                            resultRow.Add(costcenterListDesc[emplDisabilitiesDict[recID].EmployeeID].Trim());
                        else
                            resultRow.Add("");
                        if (workgroupList.ContainsKey(emplDisabilitiesDict[recID].EmployeeID))
                            resultRow.Add(workgroupList[emplDisabilitiesDict[recID].EmployeeID].Trim());
                        else
                            resultRow.Add("");
                        if (uteList.ContainsKey(emplDisabilitiesDict[recID].EmployeeID))
                            resultRow.Add(uteList[emplDisabilitiesDict[recID].EmployeeID].Trim());
                        else
                            resultRow.Add("");
                        if (branchList.ContainsKey(emplDisabilitiesDict[recID].EmployeeID))
                            resultRow.Add(branchList[emplDisabilitiesDict[recID].EmployeeID].Trim());
                        else
                            resultRow.Add("");
                        if (emplTypeList.ContainsKey(emplDisabilitiesDict[recID].EmployeeID))
                            resultRow.Add(emplTypeList[emplDisabilitiesDict[recID].EmployeeID].Trim());
                        else
                            resultRow.Add("");
                        resultRow.Add(emplDisabilitiesDict[recID].EmployeeID);
                        if (employees.ContainsKey(emplDisabilitiesDict[recID].EmployeeID))
                            resultRow.Add(employees[emplDisabilitiesDict[recID].EmployeeID].FirstAndLastName.Trim());
                        else
                            resultRow.Add("");
                        if (disabilityDict.ContainsKey(emplDisabilitiesDict[recID].DisabilityID))
                            resultRow.Add(disabilityDict[emplDisabilitiesDict[recID].DisabilityID].DisabilityCode.Trim());
                        else
                            resultRow.Add("");
                        resultRow.Add(emplDisabilitiesDict[recID].Type);
                        resultRow.Add(emplDisabilitiesDict[recID].DateStart.Date);
                        resultRow.Add(emplDisabilitiesDict[recID].DateEnd.Date);

                        resultTable.Add(resultRow);
                    }
                }

                Session[Constants.sessionDataTableColumns] = resultColumns;
                Session[Constants.sessionDataTableList] = resultTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnReport_Click(Object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                SaveState();
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCVisitsSearchPage).Assembly);

                List<List<object>> resultTable = new List<List<object>>();
                List<DataColumn> resultColumns = new List<DataColumn>();

                if (Session[Constants.sessionDataTableList] != null && Session[Constants.sessionDataTableList] is List<List<object>>)
                    resultTable = (List<List<object>>)Session[Constants.sessionDataTableList];

                if (Session[Constants.sessionDataTableColumns] != null && Session[Constants.sessionDataTableColumns] is List<DataColumn>)
                    resultColumns = (List<DataColumn>)Session[Constants.sessionDataTableColumns];

                if (resultTable.Count > 0)
                {
                    string type = "";
                    if (Session["Type"] != null)
                        type = Session["Type"].ToString();
                    bool rbVisit = false;
                    if (Session["RbVisits"] != null)
                    {
                        rbVisit = bool.Parse(Session["RbVisits"].ToString());
                    }
                    DataSet dataSet = new DataSet();
                    int typeRep = -1;
                    if (rbVisit)
                    {
                        typeRep = 1;
                        DataTable tableCR = new DataTable("visits");

                        tableCR.Columns.Add("company", typeof(System.String));
                        tableCR.Columns.Add("costcenter", typeof(string));
                        tableCR.Columns.Add("ccDesc", typeof(string));
                        tableCR.Columns.Add("workgroup", typeof(string));
                        tableCR.Columns.Add("ute", typeof(string));
                        tableCR.Columns.Add("branch", typeof(string));
                        //tableCR.Columns.Add("stringone", typeof(System.String));
                        tableCR.Columns.Add("type", typeof(System.String));
                        tableCR.Columns.Add("employeeID", typeof(System.String));
                        tableCR.Columns.Add("employee", typeof(System.String));
                        tableCR.Columns.Add("shift", typeof(System.String));
                        tableCR.Columns.Add("interval", typeof(System.String));
                        tableCR.Columns.Add("risk", typeof(System.String));
                        tableCR.Columns.Add("ambulance", typeof(System.String));
                        tableCR.Columns.Add("termin", typeof(System.String));
                        tableCR.Columns.Add("d", typeof(System.String));
                        tableCR.Columns.Add("m", typeof(System.String));
                        tableCR.Columns.Add("y", typeof(System.String));
                        tableCR.Columns.Add("status", typeof(System.String));

                        dataSet.Tables.Add(tableCR);
                        foreach (List<object> listObj in resultTable)
                        {
                            DataRow row = tableCR.NewRow();

                            if (resultColumns.Count == listObj.Count)
                            {
                                for (int i = 0; i < listObj.Count; i++)
                                {
                                    if (listObj[i] != null)
                                    {
                                        if (listObj[i] is DateTime)
                                        {
                                            if ((DateTime)listObj[i] != new DateTime())
                                                row[resultColumns[i].ColumnName] = ((DateTime)listObj[i]).ToString("HH:mm").Trim();
                                            else
                                                row[resultColumns[i].ColumnName] = "N/A";
                                        }
                                        else
                                            row[resultColumns[i].ColumnName] = listObj[i].ToString().Trim();
                                    }
                                }


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

                    }
                    else
                    {

                        DataTable tableCR = new DataTable();
                        if (type == Constants.VisitType.R.ToString())
                        {
                            typeRep = 2;
                            tableCR = new DataTable("Risks");

                            tableCR.Columns.Add("company", typeof(System.String));
                            tableCR.Columns.Add("costcenter", typeof(string));
                            tableCR.Columns.Add("ccDesc", typeof(string));
                            tableCR.Columns.Add("workgroup", typeof(string));
                            tableCR.Columns.Add("ute", typeof(string));
                            tableCR.Columns.Add("branch", typeof(string));
                            //tableCR.Columns.Add("stringone", typeof(System.String));
                            tableCR.Columns.Add("type", typeof(System.String));
                            tableCR.Columns.Add("employeeID", typeof(System.String));
                            tableCR.Columns.Add("employee", typeof(System.String));

                            tableCR.Columns.Add("risk", typeof(System.String));
                            tableCR.Columns.Add("dateStart", typeof(System.String));
                            tableCR.Columns.Add("dateEnd", typeof(System.String));
                            dataSet.Tables.Add(tableCR);

                        }
                        else if (type == Constants.VisitType.V.ToString())
                        {
                            typeRep = 3;
                            tableCR = new DataTable("Vaccines");

                            tableCR.Columns.Add("company", typeof(System.String));
                            tableCR.Columns.Add("costcenter", typeof(string));
                            tableCR.Columns.Add("ccDesc", typeof(string));
                            tableCR.Columns.Add("workgroup", typeof(string));
                            tableCR.Columns.Add("ute", typeof(string));
                            tableCR.Columns.Add("branch", typeof(string));
                            //tableCR.Columns.Add("stringone", typeof(System.String));
                            tableCR.Columns.Add("type", typeof(System.String));
                            tableCR.Columns.Add("employeeID", typeof(System.String));
                            tableCR.Columns.Add("employee", typeof(System.String));

                            tableCR.Columns.Add("vaccine", typeof(System.String));
                            tableCR.Columns.Add("date", typeof(System.String));
                            dataSet.Tables.Add(tableCR);


                        }
                        else if (type == Constants.VisitType.D.ToString())
                        {
                            typeRep = 4;
                            tableCR = new DataTable("Disabilities");

                            tableCR.Columns.Add("company", typeof(System.String));
                            tableCR.Columns.Add("costcenter", typeof(string));
                            tableCR.Columns.Add("ccDesc", typeof(string));
                            tableCR.Columns.Add("workgroup", typeof(string));
                            tableCR.Columns.Add("ute", typeof(string));
                            tableCR.Columns.Add("branch", typeof(string));
                            //tableCR.Columns.Add("stringone", typeof(System.String));
                            tableCR.Columns.Add("type", typeof(System.String));
                            tableCR.Columns.Add("employeeID", typeof(System.String));
                            tableCR.Columns.Add("employee", typeof(System.String));

                            tableCR.Columns.Add("disability", typeof(System.String));
                            tableCR.Columns.Add("disabilityType", typeof(System.String));
                            tableCR.Columns.Add("dateStart", typeof(System.String));
                            tableCR.Columns.Add("dateEnd", typeof(System.String));
                            dataSet.Tables.Add(tableCR);

                        }

                        foreach (List<object> listObj in resultTable)
                        {
                            DataRow row = tableCR.NewRow();

                            if (resultColumns.Count == listObj.Count)
                            {
                                for (int i = 0; i < listObj.Count; i++)
                                {
                                    if (listObj[i] != null)
                                    {
                                        if (listObj[i] is DateTime)
                                        {
                                            if ((DateTime)listObj[i] != new DateTime())
                                                row[resultColumns[i].ColumnName] = ((DateTime)listObj[i]).ToString("dd.MM.yyyy").Trim();
                                            else
                                                row[resultColumns[i].ColumnName] = "N/A";
                                        }
                                        else
                                            row[resultColumns[i].ColumnName] = listObj[i].ToString().Trim();
                                    }
                                }


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

                    }
                    string status = "";
                    string point = "";
                    if (Session["Status"] != null)
                        status = Session["Status"].ToString();
                    if (Session["Ambulance"] != null)
                        point = Session["Ambulance"].ToString();


                    Session["MCVisitsSearchReportPage.status"] = status;
                    Session["MCVisitsSearchReportPage.ambulance"] = point;
                    Session["MCVisitsSearchReportPage.fromDate"] = ((DateTime)Session[Constants.sessionFromDate]).ToString("dd.MM.yyyy");
                    Session["MCVisitsSearchReportPage.toDate"] = ((DateTime)Session[Constants.sessionToDate]).ToString("dd.MM.yyyy");
                    Session["MCVisitsSearchReportPage.dataSet"] = dataSet;
                    Session["MCVisitsSearchReportPage.type"] = typeRep;
                }
                else
                {

                    lblError.Text = rm.GetString("noReportData", culture);
                    return;
                }

                string reportURL = "";
                if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                    reportURL = "/ACTAWeb/ReportsWeb/en/MCVisitsReport_en.aspx";
                else
                    reportURL = "/ACTAWeb/ReportsWeb/en/MCVisitsReport_en.aspx";
                Response.Redirect("/ACTAWeb/ReportsWeb/ReportPage.aspx?Back=/ACTAWeb/ACTAWebUI/MCVisitsSearchPage.aspx&Report=" + reportURL.Trim(), false);


            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCVisitsSearchPage.btnReport_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCVisitsSearchPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void rbVisits_CheckedChanged(Object sender, EventArgs e)
        {
            try
            {
                rbEmployeeData.Checked = !rbVisits.Checked;
                setVisibility();
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCVisitsSearchPage.rbVisits_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCVisitsSearchPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void rbEmployeeData_CheckedChanged(Object sender, EventArgs e)
        {
            try
            {
                rbVisits.Checked = !rbEmployeeData.Checked;
                setVisibility();
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCVisitsSearchPage.rbEmployeeData_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCVisitsSearchPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setVisibility()
        {
            try
            {
                // deselect all statuses and points
                foreach (int statusIndex in lbStatus.GetSelectedIndices())
                {
                    if (statusIndex >= 0 && statusIndex < lbStatus.Items.Count)
                    {
                        lbStatus.Items[statusIndex].Selected = false;
                    }
                }

                foreach (int pointIndex in lbPoint.GetSelectedIndices())
                {
                    if (pointIndex >= 0 && pointIndex < lbPoint.Items.Count)
                    {
                        lbPoint.Items[pointIndex].Selected = false;
                    }
                }

                tableStatus.Visible = tablePoint.Visible = rbVisits.Checked;
                rbDisabilities.Visible = btnPersonalData.Visible = rbEmployeeData.Checked;

                if (!rbDisabilities.Visible && rbDisabilities.Checked)
                {
                    rbRisks.Checked = true;
                    rbRisks_CheckedChanged(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void rbRisks_CheckedChanged(Object sender, EventArgs e)
        {
            try
            {
                if (rbRisks.Checked)
                {
                    rbVaccines.Checked = rbDisabilities.Checked = !rbRisks.Checked;
                    populateAddData();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCVisitsSearchPage.rbRisks_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCVisitsSearchPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void rbVaccines_CheckedChanged(Object sender, EventArgs e)
        {
            try
            {
                if (rbVaccines.Checked)
                {
                    rbRisks.Checked = rbDisabilities.Checked = !rbVaccines.Checked;
                    populateAddData();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCVisitsSearchPage.rbVaccines_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCVisitsSearchPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void rbDisabilities_CheckedChanged(Object sender, EventArgs e)
        {
            try
            {
                if (rbDisabilities.Checked)
                {
                    rbVaccines.Checked = rbRisks.Checked = !rbDisabilities.Checked;
                    populateAddData();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCVisitsSearchPage.rbDisabilities_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCVisitsSearchPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnPersonalData_Click(Object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLTMDataPage).Assembly);

                lblError.Text = "";
                if (lboxEmployees.GetSelectedIndices().Length <= 0)
                    lblError.Text = rm.GetString("noSelectedEmployee", culture);
                else
                {
                    SaveState();

                    // get first selected employee
                    string emplID = "";
                    int emplIndex = lboxEmployees.GetSelectedIndices()[0];
                    if (emplIndex >= 0 && emplIndex < lboxEmployees.Items.Count)
                        emplID = lboxEmployees.Items[emplIndex].Value.Trim();

                    Response.Redirect("/ACTAWeb/ACTAWebUI/MCEmployeeDataPage.aspx?reloadState=false&emplID=" + emplID.Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCVisitsSearchPage.aspx", false);
                }

                // do not write load time to a file, becouse user is redirected to another page
                Message = "";
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCVisitsSearchPage.btnPersonalData_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCVisitsSearchPage.aspx", false);
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

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "MCVisitsSearchPage.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "MCVisitsSearchPage.", filterState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private class VisitStatus
        {
            private string _desc = "";
            private string _status = "";

            public string Desc
            {
                get { return _desc; }
                set { _desc = value; }
            }

            public string Status
            {
                get { return _status; }
                set { _status = value; }
            }

            public VisitStatus(string desc, string status)
            {
                this.Desc = desc;
                this.Status = status;
            }
        }
    }
}
