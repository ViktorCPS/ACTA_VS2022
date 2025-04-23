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
using System.Net;
using System.IO;

namespace ACTAWebUI
{
    public partial class MCSchedulingPage : System.Web.UI.Page
    {
        const string pageName = "MCSchedulingPage";
        const string pointsChanged = "MCSchedulingPointsChanged";

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
                    ClearSessionValues();

                    btnFromDate.Attributes.Add("onclick", "return calendarPicker('tbFromDate', 'true');");
                    btnToDate.Attributes.Add("onclick", "return calendarPicker('tbToDate', 'true');");
                    btnSchDate.Attributes.Add("onclick", "return calendarPicker('tbSchDate', 'true');");
                    btnWUTree.Attributes.Add("onclick", "return wUnitsPicker('btnWUTree', 'false');");
                    btnOrgTree.Attributes.Add("onclick", "return oUnitsPicker('btnOrgTree', 'false');");
                    tbEmployee.Attributes.Add("onKeyUp", "return selectItem('tbEmployee', 'lboxEmployees');");
                    btnShow.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnCoupons.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnPersonalData.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnSplitMerge.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnSave.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnSchedule.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");

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
                    btnSchDate.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnSchDate.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");

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

                    InitializeSQLParameters();

                    populateStatus();
                    populatePoints();

                    tbGroup.Text = "1";
                    rbDeselectAll.Checked = true;

                    if (Request.QueryString["reloadState"] != null && Request.QueryString["reloadState"].Trim().ToUpper().Equals(Constants.falseValue.Trim().ToUpper()))
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

                    if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO)
                    {
                        if (((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.MedicalCheckSupervisor
                            || ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.MedicalCheckWCDR
                            || ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.MedicalCheckLE)
                        {
                            pointTable.Visible = schedulingTable.Visible = false;
                            btnSave.Visible = btnSplitMerge.Visible = false;
                        }

                        if (((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.MedicalCheckLE)
                            btnAdd.Visible = false;
                    }

                    resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?height=340";
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

                setSelection();

                Session[Constants.sessionSelectedWUID] = null;
                Session[Constants.sessionSelectedOUID] = null;

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCSchedulingPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCSchedulingPage.aspx&Header=" + Constants.falseValue.Trim(), false);
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
                Session[pointsChanged] = null;
            }
            catch (Exception ex)
            {
                throw ex;
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

        private void populateEmployees(int ID, bool isWU)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCSchedulingPage).Assembly);

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

        private void populatePoints()
        {
            try
            {
                List<MedicalCheckPointTO> pointList = pointList = new MedicalCheckPoint(Session[Constants.sessionConnection]).SearchMedicalCheckPoints();

                cbPoint.DataSource = pointList;
                cbPoint.DataTextField = "Desc";
                cbPoint.DataValueField = "PointID";
                cbPoint.DataBind();
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCSchedulingPage).Assembly);

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

        protected void btnPrevDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCSchedulingPage).Assembly);

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

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCSchedulingPage.btnPrevDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCSchedulingPage.aspx&Header=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnNextDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCSchedulingPage).Assembly);

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

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCSchedulingPage.btnNextDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCSchedulingPage.aspx&Header=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCSchedulingPage).Assembly);

                lblEmployee.Text = rm.GetString("lblEmployeeSelection", culture);
                lblPeriod.Text = rm.GetString("lblPeriod", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblStatus.Text = rm.GetString("lblStatus", culture);
                lblGroup.Text = rm.GetString("lblGroup", culture);
                lblStart.Text = rm.GetString("lblStart", culture);
                lblStep.Text = rm.GetString("lblStep", culture);
                lblAmbulance.Text = rm.GetString("lblAmbulance", culture);
                lblDate.Text = rm.GetString("lblDate", culture);

                chbDeleted.Text = rm.GetString("chbDeleted", culture);

                btnShow.Text = rm.GetString("btnShow", culture);
                btnCoupons.Text = rm.GetString("btnCoupons", culture);
                btnPersonalData.Text = rm.GetString("btnPersonalData", culture);
                btnSplitMerge.Text = rm.GetString("btnSplitMerge", culture);
                btnSave.Text = rm.GetString("btnSave", culture);
                btnSchedule.Text = rm.GetString("btnSchedule", culture);
                btnAdd.Text = rm.GetString("btnAddToSchedule", culture);
                btnApply.Text = rm.GetString("btnApply", culture);
                btnGrpApply.Text = rm.GetString("btnApply", culture);

                rbSelectAll.Text = rm.GetString("rbSelectAll", culture);
                rbDeselectAll.Text = rm.GetString("rbDeselectAll", culture);
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCSchedulingPage.Menu1_MenuItemClick(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCSchedulingPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCSchedulingPage.Date_Changed(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCSchedulingPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void InitializeSQLParameters()
        {
            try
            {
                // if columns are changed, change column indexes in constants for this page!!!
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCSchedulingPage).Assembly);

                //UKOLIKO SE PROMENE KOLONE POTREBNO JE PROMENITI I KOD GENERISANJA KUPONA, METODA getTextForPDF(...)
                Session[Constants.sessionHeader] = rm.GetString("hdrVisitID", culture) + "," + rm.GetString("hdrCompany", culture)
                    + "," + rm.GetString("hdrCostCentre", culture) + "," + rm.GetString("hdrCostCenterName", culture) + "," + rm.GetString("hdrWorkgroup", culture)
                    + "," + rm.GetString("hdrUte", culture) + "," + rm.GetString("hdrBranch", culture)
                    + "," + rm.GetString("hdrEmplType", culture) + "," + rm.GetString("hdrID", culture) + "," + rm.GetString("hdrEmployee", culture)
                    + "," + rm.GetString("hdrShift", culture) + "," + rm.GetString("hdrInterval", culture) + "," + rm.GetString("hdrEmplRisk", culture)
                    + "," + rm.GetString("hdrAmbulance", culture) + "," + rm.GetString("hdrTermin", culture) + "," + rm.GetString("hdrDay", culture)
                    + "," + rm.GetString("hdrMonth", culture) + "," + rm.GetString("hdrYear", culture) + "," + rm.GetString("hdrStatus", culture)
                    + "," + rm.GetString("hdrHist", culture) + "," + rm.GetString("hdrTS", culture);
                Session[Constants.sessionFields] = "visitID, company, costcenter, ccDesc, workgroup, ute, branch, type, employeeID, employee, shift, interval, risk, ambulance, termin, d, m, y, status, history, tooltip";
                string colTypes = ((int)Constants.ColumnTypes.TEXT).ToString().Trim() + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim()
                    + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim() + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim()
                    + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim() + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim()
                    + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim() + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim()
                    + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim() + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim()
                    + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim() + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim()
                    + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim() + "," + ((int)Constants.ColumnTypes.CHANGE_TEXT).ToString().Trim()
                    + "," + ((int)Constants.ColumnTypes.EDIT_TEXT).ToString().Trim() + "," + ((int)Constants.ColumnTypes.EDIT_TEXT).ToString().Trim()
                    + "," + ((int)Constants.ColumnTypes.EDIT_TEXT).ToString().Trim() + "," + ((int)Constants.ColumnTypes.EDIT_TEXT).ToString().Trim()
                    + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim() + "," + ((int)Constants.ColumnTypes.VISIT_HIST).ToString().Trim()
                    + "," + ((int)Constants.ColumnTypes.TOOLTIP).ToString().Trim();

                if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO)
                {
                    if (((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.MedicalCheckSupervisor
                        || ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.MedicalCheckWCDR
                        || ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.MedicalCheckLE)
                    {
                        colTypes = ((int)Constants.ColumnTypes.TEXT).ToString().Trim() + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim()
                            + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim() + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim()
                            + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim() + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim()
                            + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim() + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim()
                            + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim() + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim()
                            + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim() + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim()
                            + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim() + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim()
                            + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim() + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim()
                            + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim() + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim()
                            + "," + ((int)Constants.ColumnTypes.TEXT).ToString().Trim() + "," + ((int)Constants.ColumnTypes.VISIT_HIST).ToString().Trim()
                            + "," + ((int)Constants.ColumnTypes.TOOLTIP).ToString().Trim();
                    }
                }
                Session[Constants.sessionColTypes] = colTypes;
                Dictionary<int, int> formating = new Dictionary<int, int>();
                formating.Add(14, (int)Constants.FormatTypes.TimeFormat);
                Session[Constants.sessionFieldsFormating] = formating;
                Session[Constants.sessionFieldsFormatedValues] = null;
                Session[Constants.sessionTables] = null;
                Session[Constants.sessionDataTableList] = null;
                Session[Constants.sessionDataTableColumns] = null;
                Session[Constants.sessionKey] = "visitID";
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCSchedulingPage).Assembly);

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

                Dictionary<int, EmployeeTO> employees = new Employee(Session[Constants.sessionConnection]).SearchDictionary(emplIDs);

                // stringone, company
                Dictionary<int, EmployeeAsco4TO> emplAscoList = new EmployeeAsco4(Session[Constants.sessionConnection]).SearchDictionary(emplIDs);
                Dictionary<int, string> stringoneList = new Dictionary<int, string>();
                Dictionary<int, string> compList = new Dictionary<int, string>();
                Dictionary<int, string> emplTypeList = new Dictionary<int, string>();

                // get all time schemas
                Dictionary<int, WorkTimeSchemaTO> schemas = new TimeSchema(Session[Constants.sessionConnection]).getDictionary();

                // get schedules for months selected
                DateTime firstFromMonthDay = new DateTime(fromDate.Year, fromDate.Month, 1).Date;
                DateTime lastToMonthDay = new DateTime(toDate.Year, toDate.Month, 1).AddMonths(1).AddDays(-1).Date;
                EmployeesTimeSchedule schedule = new EmployeesTimeSchedule(Session[Constants.sessionConnection]);
                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = schedule.SearchEmployeesSchedulesExactDate(emplIDs, firstFromMonthDay.Date, lastToMonthDay.Date, null);

                // get schedules for next month
                DateTime firstNextMonthDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).Date;
                DateTime lastNextMonthDay = firstNextMonthDay.AddMonths(1).AddDays(-1).Date;
                Dictionary<int, List<EmployeeTimeScheduleTO>> emplNextMonthSchedules = schedule.SearchEmployeesSchedulesExactDate(emplIDs, firstNextMonthDay.Date, lastNextMonthDay.Date, null);

                Dictionary<int, string> emplNextMonthSchedulesTooltips = new Dictionary<int, string>();
                Dictionary<int, Dictionary<DateTime, string>> emplMonthSchedulesTooltips = new Dictionary<int, Dictionary<DateTime, string>>();

                // calculate schedules tooltips for selected months
                foreach (int id in emplSchedules.Keys)
                {
                    if (!emplMonthSchedulesTooltips.ContainsKey(id))
                        emplMonthSchedulesTooltips.Add(id, new Dictionary<DateTime, string>());

                    DateTime currDate = firstFromMonthDay;
                    while (currDate.Date <= lastToMonthDay)
                    {
                        // get month of calculated day
                        DateTime currMonth = new DateTime(currDate.Year, currDate.Month, 1).Date;

                        if (!emplMonthSchedulesTooltips[id].ContainsKey(currMonth))
                            emplMonthSchedulesTooltips[id].Add(currMonth, "");

                        if (emplMonthSchedulesTooltips[id][currMonth].Length > 0)
                            emplMonthSchedulesTooltips[id][currMonth] += "<br/>";
                        emplMonthSchedulesTooltips[id][currMonth] += currDate.Date.ToString(Constants.dateFormat) + " ";
                        List<WorkTimeIntervalTO> dayIntervals = Common.Misc.getTimeSchemaInterval(currDate.Date, emplSchedules[id], schemas);

                        string dayIntervalString = "";
                        bool intervalFound = false;
                        foreach (WorkTimeIntervalTO interval in dayIntervals)
                        {
                            if (interval.StartTime.TimeOfDay != new TimeSpan(0, 0, 0) || interval.EndTime.TimeOfDay != new TimeSpan(0, 0, 0))
                                intervalFound = true;

                            dayIntervalString += interval.StartTime.ToString(Constants.timeFormat) + "-" + interval.EndTime.ToString(Constants.timeFormat) + " ";
                        }

                        if (intervalFound)
                            emplMonthSchedulesTooltips[id][currMonth] += "<font color=green>" + dayIntervalString + "</font>";
                        else
                            emplMonthSchedulesTooltips[id][currMonth] += "<font color=red>" + dayIntervalString + "</font>";

                        currDate = currDate.AddDays(1);
                    }
                }

                // calculate schedules tooltips for next month
                foreach (int id in emplNextMonthSchedules.Keys)
                {
                    string intervalsString = "";
                    DateTime currDate = firstNextMonthDay;
                    while (currDate.Date <= lastNextMonthDay)
                    {
                        if (intervalsString.Length > 0)
                            intervalsString += "<br/>";
                        intervalsString += currDate.Date.ToString(Constants.dateFormat) + " ";
                        List<WorkTimeIntervalTO> dayIntervals = Common.Misc.getTimeSchemaInterval(currDate.Date, emplNextMonthSchedules[id], schemas);

                        string dayIntervalString = "";
                        bool intervalFound = false;
                        foreach (WorkTimeIntervalTO interval in dayIntervals)
                        {
                            if (interval.StartTime.TimeOfDay != new TimeSpan(0, 0, 0) || interval.EndTime.TimeOfDay != new TimeSpan(0, 0, 0))
                                intervalFound = true;

                            dayIntervalString += interval.StartTime.ToString(Constants.timeFormat) + "-" + interval.EndTime.ToString(Constants.timeFormat) + " ";
                        }

                        if (intervalFound)
                            intervalsString += "<font color=green>" + dayIntervalString + "</font>";
                        else
                            intervalsString += "<font color=red>" + dayIntervalString + "</font>";

                        currDate = currDate.AddDays(1);
                    }

                    if (!emplNextMonthSchedulesTooltips.ContainsKey(id))
                        emplNextMonthSchedulesTooltips.Add(id, intervalsString.Trim());
                }

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

                Dictionary<uint, MedicalCheckVisitHdrTO> visitDict = new MedicalCheckVisitHdr(Session[Constants.sessionConnection]).SearchMedicalCheckVisits(emplIDs, status, "", "", "", fromDate.Date, toDate.Date);

                // get visit ids that has history
                string visitIDs = "";
                foreach (uint visitID in visitDict.Keys)
                {
                    visitIDs += visitID.ToString().Trim() + ",";
                }

                if (visitIDs.Length > 0)
                    visitIDs = visitIDs.Substring(0, visitIDs.Length - 1);

                List<uint> histIDs = new MedicalCheckVisitHdrHist(Session[Constants.sessionConnection]).SearchMedicalCheckVisitHeadersHistory(visitIDs);

                List<List<object>> resultTable = new List<List<object>>();

                // create list of columns for list preview of pairs for inserting
                List<DataColumn> resultColumns = new List<DataColumn>();
                resultColumns.Add(new DataColumn("visitID", typeof(uint)));
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
                resultColumns.Add(new DataColumn("history", typeof(bool)));
                resultColumns.Add(new DataColumn("tooltip", typeof(string)));

                Dictionary<string, Color> itemColors = new Dictionary<string, Color>();
                Dictionary<string, string> itemTooltips = new Dictionary<string, string>();
                foreach (uint visitID in visitDict.Keys)
                {
                    // create result row
                    List<object> resultRow = new List<object>();

                    resultRow.Add(visitID);
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
                    bool intervalFound = false;
                    foreach (WorkTimeIntervalTO interval in intervals)
                    {
                        intervalsString += interval.StartTime.ToString(Constants.timeFormat) + "-" + interval.EndTime.ToString(Constants.timeFormat) + Environment.NewLine;

                        if (interval.StartTime.TimeOfDay <= visitDict[visitID].ScheduleDate.TimeOfDay && interval.EndTime.TimeOfDay > visitDict[visitID].ScheduleDate.TimeOfDay)
                            intervalFound = true;
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
                    resultRow.Add(histIDs.Contains(visitID));

                    // set tooltips
                    if (visitDict[visitID].ScheduleDate.Equals(new DateTime()) || visitDict[visitID].ScheduleDate.Equals(Constants.dateTimeNullValue()))
                    {
                        if (emplNextMonthSchedulesTooltips.ContainsKey(visitDict[visitID].EmployeeID))
                            resultRow.Add(emplNextMonthSchedulesTooltips[visitDict[visitID].EmployeeID].Trim());
                        else
                            resultRow.Add("");
                    }
                    else
                    {
                        // get scheduled date month
                        DateTime schMonth = new DateTime(visitDict[visitID].ScheduleDate.Year, visitDict[visitID].ScheduleDate.Month, 1);
                        // get scheduled date week
                        DateTime schWeekBegining = Common.Misc.getWeekBeggining(visitDict[visitID].ScheduleDate);
                        DateTime schNextWeekBegining = schWeekBegining.AddDays(7);

                        if (emplMonthSchedulesTooltips.ContainsKey(visitDict[visitID].EmployeeID) && emplMonthSchedulesTooltips[visitDict[visitID].EmployeeID].ContainsKey(schMonth.Date))
                        {
                            // set scheduled week to bold
                            int weekBeginingIndex = emplMonthSchedulesTooltips[visitDict[visitID].EmployeeID][schMonth.Date].IndexOf(schWeekBegining.ToString(Constants.dateFormat));
                            int weekEndIndex = emplMonthSchedulesTooltips[visitDict[visitID].EmployeeID][schMonth.Date].IndexOf(schNextWeekBegining.ToString(Constants.dateFormat));
                            string beforeSchWeek = "";
                            string schWeek = "";
                            string afterSchWeek = "";

                            if (weekBeginingIndex >= 0)
                                beforeSchWeek = emplMonthSchedulesTooltips[visitDict[visitID].EmployeeID][schMonth.Date].Substring(0, weekBeginingIndex);
                            else
                                weekBeginingIndex = 0;

                            if (weekEndIndex >= 0)
                                afterSchWeek = emplMonthSchedulesTooltips[visitDict[visitID].EmployeeID][schMonth.Date].Substring(weekEndIndex);
                            else
                                weekEndIndex = emplMonthSchedulesTooltips[visitDict[visitID].EmployeeID][schMonth.Date].Length;
                            schWeek = emplMonthSchedulesTooltips[visitDict[visitID].EmployeeID][schMonth.Date].Substring(weekBeginingIndex, weekEndIndex - weekBeginingIndex);

                            resultRow.Add(beforeSchWeek + "<b>" + schWeek + "</b>" + afterSchWeek);
                        }
                        else
                            resultRow.Add("");
                    }

                    resultTable.Add(resultRow);

                    if (!visitDict[visitID].ScheduleDate.Equals(new DateTime()) && !visitDict[visitID].ScheduleDate.Equals(Constants.dateTimeNullValue())
                        && !intervalFound && !itemColors.ContainsKey(visitID.ToString().Trim()))
                        itemColors.Add(visitID.ToString().Trim(), Color.Pink);
                }

                Session[Constants.sessionDataTableColumns] = resultColumns;
                Session[Constants.sessionDataTableList] = resultTable;
                Session[Constants.sessionItemsColors] = itemColors;

                SelBox.Value = "";
                ChangedBox.Value = "";

                resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?height=340";

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCSchedulingPage.btnShow_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCSchedulingPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnSave_Click(Object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCSchedulingPage).Assembly);

                lblError.Text = "";

                // get all changes
                Dictionary<string, Dictionary<int, string>> chgKeys = getChanges();

                if (chgKeys.Count <= 0)
                {
                    lblError.Text = rm.GetString("noChangesToSave", culture);
                }

                // get all changed visits
                List<string> chgVisits = new List<string>();
                string visitIDs = "";
                foreach (string id in chgKeys.Keys)
                {
                    visitIDs += id + ",";
                    chgVisits.Add(id);
                }

                Dictionary<string, int> pointsChangedDict = new Dictionary<string, int>();

                if (Session[pointsChanged] != null && Session[pointsChanged] is Dictionary<string, int>)
                    pointsChangedDict = (Dictionary<string, int>)Session[pointsChanged];

                foreach (string id in pointsChangedDict.Keys)
                {
                    if (!chgVisits.Contains(id))
                    {
                        visitIDs += id + ",";
                        chgVisits.Add(id);
                    }
                }

                if (visitIDs.Length > 0)
                    visitIDs = visitIDs.Substring(0, visitIDs.Length - 1);

                MedicalCheckVisitHdr hdr = new MedicalCheckVisitHdr(Session[Constants.sessionConnection]);
                List<MedicalCheckVisitHdrTO> hdrList = hdr.SearchMedicalCheckVisits(visitIDs);
                if (hdrList.Count <= 0)
                {
                    lblError.Text = rm.GetString("noVisitsFound", culture);
                    setSelection();
                    return;
                }

                MedicalCheckVisitHdrHist hdrHist = new MedicalCheckVisitHdrHist(Session[Constants.sessionConnection]);
                List<MedicalCheckVisitHdrHistTO> hdrHistList = new List<MedicalCheckVisitHdrHistTO>();

                string user = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);

                // put all visits to history
                foreach (MedicalCheckVisitHdrTO hdrTO in hdrList)
                {
                    // done and deleted visits can not be changed, so do not put them to history
                    if (hdrTO.Status.Trim().ToUpper().Equals(Constants.MedicalCheckVisitStatus.DONE.ToString().Trim().ToUpper())
                        || hdrTO.Status.Trim().ToUpper().Equals(Constants.MedicalCheckVisitStatus.DELETED.ToString().Trim().ToUpper()))
                        continue;

                    MedicalCheckVisitHdrHistTO hdrHistTO = new MedicalCheckVisitHdrHistTO(hdrTO);
                    hdrHistTO.ModifiedBy = user;
                    hdrHistTO.ModifiedTime = new DateTime();

                    foreach (MedicalCheckVisitDtlHistTO dtlTO in hdrHistTO.VisitDetails)
                    {
                        dtlTO.ModifiedBy = user;
                        dtlTO.ModifiedTime = new DateTime();
                    }

                    hdrHistList.Add(hdrHistTO);
                }

                string invalidVisits = "";

                foreach (MedicalCheckVisitHdrTO hdrTO in hdrList)
                {
                    bool visitRescheduled = false;
                    if (pointsChangedDict.ContainsKey(hdrTO.VisitID.ToString()))
                    {
                        if (hdrTO.Status.Trim().ToUpper().Equals(Constants.MedicalCheckVisitStatus.DONE.ToString().Trim().ToUpper())
                            && hdrTO.PointID != pointsChangedDict[hdrTO.VisitID.ToString()])
                        {
                            invalidVisits += rm.GetString("doneVisitChange", culture) + " " + hdrTO.VisitID.ToString() + "." + Environment.NewLine;
                            continue;
                        }

                        if (hdrTO.Status.Trim().ToUpper().Equals(Constants.MedicalCheckVisitStatus.DELETED.ToString().Trim().ToUpper())
                            && hdrTO.PointID != pointsChangedDict[hdrTO.VisitID.ToString()])
                        {
                            invalidVisits += rm.GetString("deletedVisitChange", culture) + " " + hdrTO.VisitID.ToString() + "." + Environment.NewLine;
                            continue;
                        }

                        if (hdrTO.Status.Trim().ToUpper().Equals(Constants.MedicalCheckVisitStatus.RND.ToString().Trim().ToUpper()) &&
                            hdrTO.PointID != pointsChangedDict[hdrTO.VisitID.ToString()])
                            visitRescheduled = true;

                        hdrTO.PointID = pointsChangedDict[hdrTO.VisitID.ToString()];
                    }

                    if (chgKeys.ContainsKey(hdrTO.VisitID.ToString()))
                    {
                        DateTime schTime = hdrTO.ScheduleDate;
                        int d = hdrTO.ScheduleDate.Day;
                        int m = hdrTO.ScheduleDate.Month;
                        int y = hdrTO.ScheduleDate.Year;

                        if (chgKeys[hdrTO.VisitID.ToString()].ContainsKey(Constants.MCSchedulingTerminColIndex))
                            schTime = CommonWeb.Misc.createTime(chgKeys[hdrTO.VisitID.ToString()][Constants.MCSchedulingTerminColIndex].Trim());

                        if (chgKeys[hdrTO.VisitID.ToString()].ContainsKey(Constants.MCSchedulingDColIndex))
                        {
                            if (!int.TryParse(chgKeys[hdrTO.VisitID.ToString()][Constants.MCSchedulingDColIndex].Trim(), out d))
                                d = -1;
                        }

                        if (chgKeys[hdrTO.VisitID.ToString()].ContainsKey(Constants.MCSchedulingMColIndex))
                        {
                            if (!int.TryParse(chgKeys[hdrTO.VisitID.ToString()][Constants.MCSchedulingMColIndex].Trim(), out m))
                                m = -1;
                        }

                        if (chgKeys[hdrTO.VisitID.ToString()].ContainsKey(Constants.MCSchedulingYColIndex))
                        {
                            if (!int.TryParse(chgKeys[hdrTO.VisitID.ToString()][Constants.MCSchedulingYColIndex].Trim(), out y))
                                y = -1;
                        }

                        if (schTime.Equals(new DateTime()) || d == -1 || m == -1 || y == -1)
                        {
                            invalidVisits += rm.GetString("invalidScheduledDate", culture) + " " + hdrTO.VisitID.ToString() + "." + Environment.NewLine;
                        }
                        else
                        {
                            try
                            {
                                DateTime newSchDate = new DateTime(y, m, d, schTime.Hour, schTime.Minute, 0);
                                if (hdrTO.Status.Trim().ToUpper().Equals(Constants.MedicalCheckVisitStatus.DONE.ToString().Trim().ToUpper())
                                    && hdrTO.ScheduleDate != newSchDate)
                                {
                                    invalidVisits += rm.GetString("doneVisitChange", culture) + " " + hdrTO.VisitID.ToString() + "." + Environment.NewLine;
                                    continue;
                                }

                                if (hdrTO.Status.Trim().ToUpper().Equals(Constants.MedicalCheckVisitStatus.DELETED.ToString().Trim().ToUpper())
                                    && hdrTO.ScheduleDate != newSchDate)
                                {
                                    invalidVisits += rm.GetString("deletedVisitChange", culture) + " " + hdrTO.VisitID.ToString() + "." + Environment.NewLine;
                                    continue;
                                }

                                if (hdrTO.Status.Trim().ToUpper().Equals(Constants.MedicalCheckVisitStatus.RND.ToString().Trim().ToUpper()) &&
                                    hdrTO.ScheduleDate != newSchDate)
                                    visitRescheduled = true;

                                hdrTO.ScheduleDate = newSchDate;
                                hdrTO.ModifiedBy = user;
                                hdrTO.ModifiedTime = new DateTime();

                                if ((hdrTO.Status.Trim().ToUpper().Equals(Constants.MedicalCheckVisitStatus.WR.ToString().Trim().ToUpper())
                                    || hdrTO.Status.Trim().ToUpper().Equals(Constants.MedicalCheckVisitStatus.DEMANDED.ToString().Trim().ToUpper()))
                                    && !hdrTO.ScheduleDate.Equals(new DateTime()) && !hdrTO.ScheduleDate.Equals(Constants.dateTimeNullValue()))
                                    hdrTO.Status = Constants.MedicalCheckVisitStatus.RND.ToString();

                                if (visitRescheduled)
                                    hdrTO.FlagChange = Constants.yesInt;
                            }
                            catch
                            {
                                // add to invalid visits
                                invalidVisits += rm.GetString("invalidScheduledDate", culture) + " " + hdrTO.VisitID.ToString() + "." + Environment.NewLine;
                            }
                        }
                    }
                }

                // pop up with invalid visit
                if (!invalidVisits.Trim().Equals(""))
                {
                    lblError.Text = rm.GetString("changesNotSaved", culture);
                    Session[Constants.sessionInfoMessage] = invalidVisits.Trim();
                    string wOptions = "dialogWidth:800px; dialogHeight:300px; edge:Raised; center:Yes; resizable:Yes; status:Yes;";
                    ClientScript.RegisterStartupScript(GetType(), "infoPopup", "window.open('/ACTAWeb/CommonWeb/MessagesPages/Info.aspx?isError=true', window, '" + wOptions.Trim() + "');", true);
                }
                else
                {
                    // save history and update visits
                    if (hdr.BeginTransaction())
                    {
                        try
                        {
                            bool saved = true;
                            hdrHist.SetTransaction(hdr.GetTransaction());

                            foreach (MedicalCheckVisitHdrHistTO histTO in hdrHistList)
                            {
                                hdrHist.VisitHdrHistTO = histTO;
                                saved = saved && hdrHist.Save(false);

                                if (!saved)
                                    break;
                            }

                            if (saved)
                            {
                                foreach (MedicalCheckVisitHdrTO hdrTO in hdrList)
                                {
                                    hdr.VisitHdrTO = hdrTO;
                                    saved = saved && hdr.Update(false);

                                    if (!saved)
                                        break;
                                }
                            }

                            if (saved)
                            {
                                hdr.CommitTransaction();

                                btnShow_Click(this, new EventArgs());

                                lblError.Text = rm.GetString("changesSaved", culture);
                            }
                            else
                            {
                                if (hdr.GetTransaction() != null)
                                    hdr.RollbackTransaction();

                                lblError.Text = rm.GetString("changesNotSaved", culture);
                            }
                        }
                        catch
                        {
                            if (hdr.GetTransaction() != null)
                                hdr.RollbackTransaction();

                            lblError.Text = rm.GetString("changesNotSaved", culture);
                        }
                    }
                    else
                        lblError.Text = rm.GetString("changesNotSaved", culture);
                }

                setSelection();

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCSchedulingPage.btnShow_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCSchedulingPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnSplitMerge_Click(Object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCSchedulingPage).Assembly);

                lblError.Text = "";

                List<string> selKeys = getSelection();
                if (selKeys.Count <= 0)
                {
                    lblError.Text = rm.GetString("noSelectedRecord", culture);
                    setSelection();
                    return;
                }

                string visitIDs = "";
                foreach (string id in selKeys)
                {
                    visitIDs += id.Trim() + ",";
                }

                if (visitIDs.Length > 0)
                    visitIDs = visitIDs.Substring(0, visitIDs.Length - 1);

                MedicalCheckVisitHdr hdr = new MedicalCheckVisitHdr(Session[Constants.sessionConnection]);
                List<MedicalCheckVisitHdrTO> hdrList = hdr.SearchMedicalCheckVisits(visitIDs);
                if (hdrList.Count <= 0)
                {
                    lblError.Text = rm.GetString("noVisitsFound", culture);
                    setSelection();
                    return;
                }

                // check visits status, only WR and DEMANDED visits can be split and merged
                bool statusChanged = false;
                foreach (MedicalCheckVisitHdrTO hdrTO in hdrList)
                {
                    if (!hdrTO.Status.Trim().ToUpper().Equals(Constants.MedicalCheckVisitStatus.WR.ToString().Trim().ToUpper())
                        && !hdrTO.Status.Trim().ToUpper().Equals(Constants.MedicalCheckVisitStatus.DEMANDED.ToString().Trim().ToUpper()))
                    {
                        statusChanged = true;
                        break;
                    }
                }

                if (statusChanged)
                {
                    lblError.Text = rm.GetString("statusChahgedNoSplitMergeAllowed", culture);
                    setSelection();
                    return;
                }

                string user = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);

                MedicalCheckVisitHdrHist hdrHist = new MedicalCheckVisitHdrHist(Session[Constants.sessionConnection]);
                List<MedicalCheckVisitHdrHistTO> hdrHistList = new List<MedicalCheckVisitHdrHistTO>();
                List<MedicalCheckVisitHdrTO> hdrNewList = new List<MedicalCheckVisitHdrTO>();

                // put all visits to history
                foreach (MedicalCheckVisitHdrTO hdrTO in hdrList)
                {
                    MedicalCheckVisitHdrHistTO hdrHistTO = new MedicalCheckVisitHdrHistTO(hdrTO);
                    hdrHistTO.ModifiedBy = user;
                    hdrHistTO.ModifiedTime = new DateTime();

                    foreach (MedicalCheckVisitDtlHistTO dtlTO in hdrHistTO.VisitDetails)
                    {
                        dtlTO.ModifiedBy = user;
                        dtlTO.ModifiedTime = new DateTime();
                    }

                    hdrHistList.Add(hdrHistTO);
                }

                // if one record is selected, split it to as much visits as there are details, one detail - one visit
                string error = "";
                if (hdrList.Count == 1)
                {
                    if (hdrList[0].VisitDetails.Count <= 1)
                    {
                        lblError.Text = rm.GetString("noSplitDetails", culture);
                        setSelection();
                        return;
                    }

                    foreach (MedicalCheckVisitDtlTO dtlTO in hdrList[0].VisitDetails)
                    {
                        // create new header
                        MedicalCheckVisitHdrTO newHdr = new MedicalCheckVisitHdrTO(hdrList[0]);
                        newHdr.CreatedBy = user;
                        newHdr.CreatedTime = new DateTime();
                        newHdr.ModifiedBy = "";
                        newHdr.ModifiedTime = new DateTime();
                        newHdr.VisitDetails = new List<MedicalCheckVisitDtlTO>();
                        MedicalCheckVisitDtlTO newDtl = new MedicalCheckVisitDtlTO(dtlTO);
                        newDtl.CreatedBy = user;
                        newDtl.CreatedTime = new DateTime();
                        newDtl.ModifiedBy = "";
                        newDtl.ModifiedTime = new DateTime();
                        newHdr.VisitDetails.Add(newDtl);
                        hdrNewList.Add(newHdr);
                    }

                    // change email flag to deleted visit to sent mail, becouse mails are not sent for splited/merged visits so there is nothing to cancel                    
                    hdrList[0].FlagEmail = Constants.yesInt;
                }
                // if more records are selected, merge them in one visit
                else
                {
                    int emplID = hdrList[0].EmployeeID;
                    DateTime schDate = Constants.dateTimeNullValue();
                    string status = "";
                    int point = Constants.defaultMedicalCheckPointId;

                    MedicalCheckVisitHdrTO newHdr = new MedicalCheckVisitHdrTO(hdrList[0]);
                    newHdr.EmployeeID = emplID;
                    newHdr.CreatedBy = user;
                    newHdr.CreatedTime = new DateTime();
                    newHdr.ModifiedBy = "";
                    newHdr.ModifiedTime = new DateTime();
                    newHdr.VisitDetails = new List<MedicalCheckVisitDtlTO>();

                    bool doneVisits = true;
                    foreach (MedicalCheckVisitHdrTO hdrTO in hdrList)
                    {
                        if (hdrTO.EmployeeID != emplID)
                        {
                            error = "differentEmployeeMerge";
                            break;
                        }

                        if (hdrTO.PointID != Constants.defaultMedicalCheckPointId)
                            point = hdrTO.PointID;

                        if (!hdrTO.ScheduleDate.Equals(Constants.dateTimeNullValue()))
                            schDate = hdrTO.ScheduleDate;

                        foreach (MedicalCheckVisitDtlTO dtlTO in hdrTO.VisitDetails)
                        {
                            if (dtlTO.DatePerformed.Equals(new DateTime()))
                                doneVisits = false;

                            MedicalCheckVisitDtlTO newDtl = new MedicalCheckVisitDtlTO(dtlTO);
                            newDtl.CreatedBy = user;
                            newDtl.CreatedTime = new DateTime();
                            newDtl.ModifiedBy = "";
                            newDtl.ModifiedTime = new DateTime();
                            newHdr.VisitDetails.Add(newDtl);
                        }

                        hdrTO.FlagEmail = Constants.yesInt;
                    }

                    if (!error.Trim().Equals(""))
                    {
                        lblError.Text = rm.GetString(error, culture);
                        setSelection();
                        return;
                    }

                    newHdr.ScheduleDate = schDate;
                    newHdr.PointID = point;

                    if (doneVisits)
                        status = Constants.MedicalCheckVisitStatus.DONE.ToString();
                    else if (!schDate.Equals(Constants.dateTimeNullValue()) && !schDate.Equals(new DateTime()))
                        status = Constants.MedicalCheckVisitStatus.RND.ToString();
                    else
                        status = Constants.MedicalCheckVisitStatus.WR.ToString();
                    newHdr.Status = status;
                    newHdr.FlagEmail = Constants.noInt;
                    newHdr.FlagEmailCratedTime = new DateTime();
                    hdrNewList.Add(newHdr);
                }

                // move records to history, update old visits to deleted and save new one
                if (hdr.BeginTransaction())
                {
                    try
                    {
                        bool saved = true;
                        hdrHist.SetTransaction(hdr.GetTransaction());

                        foreach (MedicalCheckVisitHdrHistTO histTO in hdrHistList)
                        {
                            hdrHist.VisitHdrHistTO = histTO;
                            saved = saved && hdrHist.Save(false);

                            if (!saved)
                                break;
                        }

                        if (saved)
                        {
                            foreach (MedicalCheckVisitHdrTO hdrTO in hdrList)
                            {
                                hdrTO.Status = Constants.MedicalCheckVisitStatus.DELETED.ToString();
                                hdr.VisitHdrTO = hdrTO;
                                saved = saved && hdr.Update(false);

                                if (!saved)
                                    break;
                            }
                        }

                        if (saved)
                        {
                            foreach (MedicalCheckVisitHdrTO hdrTO in hdrNewList)
                            {
                                hdr.VisitHdrTO = hdrTO;
                                saved = saved && hdr.Save(false);

                                if (!saved)
                                    break;
                            }
                        }

                        if (saved)
                        {
                            hdr.CommitTransaction();

                            btnShow_Click(this, new EventArgs());

                            lblError.Text = rm.GetString("recordUpdated", culture);
                        }
                        else
                        {
                            if (hdr.GetTransaction() != null)
                                hdr.RollbackTransaction();

                            lblError.Text = rm.GetString("recordNotUpdated", culture);
                            setSelection();
                        }
                    }
                    catch
                    {
                        if (hdr.GetTransaction() != null)
                            hdr.RollbackTransaction();

                        lblError.Text = rm.GetString("recordNotUpdated", culture);
                        setSelection();
                    }
                }
                else
                {
                    lblError.Text = rm.GetString("recordNotUpdated", culture);
                    setSelection();
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCSchedulingPage.btnSplitMerge_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCSchedulingPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnPersonalData_Click(Object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCSchedulingPage).Assembly);

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

                    Response.Redirect("/ACTAWeb/ACTAWebUI/MCEmployeeDataPage.aspx?reloadState=false&emplID=" + emplID.Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCSchedulingPage.aspx", false);
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCSchedulingPage.btnPersonalData_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCSchedulingPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnCoupons_Click(Object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";

                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCSchedulingPage).Assembly);


                if (Session[Constants.sessionDataTableList] != null)
                {
                    List<List<object>> resultTable = new List<List<object>>();
                    resultTable = (List<List<object>>)Session[Constants.sessionDataTableList];

                    ApplUserTO user = (ApplUserTO)Session[Constants.sessionLogInUser];
                    string filePath = Constants.logFilePath + "Temp\\" + user.UserID + "_Coupon" + DateTime.Now.ToString("dd-MM-yyyy HHmmss") + "\\";
                    string name = "Coupon.pdf";
                    filePath += name;
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

                    string stringresult = "";
                    List<string> selected = getSelection();
                    Dictionary<string, Dictionary<int, string>> changes = getChanges();
                    string error = "";
                    int numOfRows = 0;
                    foreach (List<object> resultRow in resultTable)
                    {
                        string ID = resultRow[0].ToString().Trim();
                        string point = resultRow[Constants.MCSchedulingPointColIndex].ToString();
                        DateTime termin = (DateTime)resultRow[Constants.MCSchedulingTerminColIndex];
                        string day = resultRow[Constants.MCSchedulingDColIndex].ToString();
                        string month = resultRow[Constants.MCSchedulingMColIndex].ToString();
                        string year = resultRow[Constants.MCSchedulingYColIndex].ToString();
                        string status = resultRow[18].ToString();
                        if (selected.Contains(resultRow[0].ToString().Trim()))
                        {
                            if (status.Equals(rm.GetString("RND", culture)) || status.Equals(rm.GetString("DONE", culture)))
                            {

                                if (changes.ContainsKey(ID))
                                {
                                    DateTime schTime = new DateTime();
                                    if (changes[ID].ContainsKey(Constants.MCSchedulingTerminColIndex))
                                        schTime = CommonWeb.Misc.createTime(changes[resultRow[0].ToString()][Constants.MCSchedulingTerminColIndex].Trim());

                                    Dictionary<int, string> dictionaryChanges = changes[resultRow[0].ToString().Trim()];

                                    if ((dictionaryChanges.ContainsKey(Constants.MCSchedulingPointColIndex) && !dictionaryChanges[Constants.MCSchedulingPointColIndex].Equals(point)))
                                    {
                                        error += rm.GetString("errorCouponAmbulanceChanged", culture) + "ID - " + ID + ". ";

                                    }
                                    else if (schTime != new DateTime() && schTime.TimeOfDay != termin.TimeOfDay)
                                    {
                                        error += rm.GetString("errorCouponTimeChanged", culture) + "ID - " + ID + ". ";

                                    }
                                    else if (dictionaryChanges.ContainsKey(Constants.MCSchedulingDColIndex) && !dictionaryChanges[Constants.MCSchedulingDColIndex].Equals(day))
                                    {
                                        error += rm.GetString("errorCouponDayChanged", culture) + "ID - " + ID + ". ";

                                    }
                                    else if (dictionaryChanges.ContainsKey(Constants.MCSchedulingMColIndex) && !dictionaryChanges[Constants.MCSchedulingMColIndex].Equals(month))
                                    {
                                        error += rm.GetString("errorCouponMonthChanged", culture) + "ID - " + ID + ". ";

                                    }
                                    else if (dictionaryChanges.ContainsKey(Constants.MCSchedulingYColIndex) && !dictionaryChanges[Constants.MCSchedulingYColIndex].Equals(year))
                                    {
                                        error += rm.GetString("errorCouponYearChanged", culture) + "ID - " + ID + ". ";
                                    }
                                    else
                                    {
                                        stringresult += getTextForPDF(resultRow, point, day, month, year, termin, rm, culture);
                                        numOfRows++;
                                    }
                                }
                                else
                                {
                                    stringresult += getTextForPDF(resultRow, point, day, month, year, termin, rm, culture);
                                    numOfRows++;
                                }
                            }
                        }
                    }
                    if (!error.Trim().Equals(""))
                    {
                        error += rm.GetString("errorSaveBeforeGenerating", culture);
                        Session[Constants.sessionInfoMessage] = error.Trim();
                        string wOptions = "dialogWidth:800px; dialogHeight:300px; edge:Raised; center:Yes; resizable:Yes; status:Yes;";
                        ClientScript.RegisterStartupScript(GetType(), "infoPopup", "window.open('/ACTAWeb/CommonWeb/MessagesPages/Info.aspx?isError=true', window, '" + wOptions.Trim() + "');", true);
                        return;
                    }

                    if (stringresult.Trim().Equals(""))
                    {
                        lblError.Text = rm.GetString("noReportData", culture);
                        return;
                    }
                    iTextSharp.text.Document pdfDocCreatePDF = new iTextSharp.text.Document();

                    //Because of UNICODE char.
                    string sylfaenpath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\sylfaen.ttf";
                    iTextSharp.text.pdf.BaseFont sylfaen = iTextSharp.text.pdf.BaseFont.CreateFont(sylfaenpath, iTextSharp.text.pdf.BaseFont.IDENTITY_H, iTextSharp.text.pdf.BaseFont.EMBEDDED);
                    iTextSharp.text.Font head = new iTextSharp.text.Font(sylfaen, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLUE);
                    iTextSharp.text.Font normal = new iTextSharp.text.Font(sylfaen, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                    iTextSharp.text.Font underline = new iTextSharp.text.Font(sylfaen, 10f, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK);

                    if (numOfRows < 10)
                    {
                        iTextSharp.text.pdf.PdfWriter writerPdf = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDocCreatePDF, new FileStream(filePath, FileMode.Create));
                        iTextSharp.text.Paragraph para = new iTextSharp.text.Paragraph(10, stringresult, normal);

                        pdfDocCreatePDF.Open();
                        pdfDocCreatePDF.Add(para);
                        pdfDocCreatePDF.Close();

                        ReadPdfFile(filePath);
                    }
                    else
                    {

                        iTextSharp.text.pdf.PdfWriter writerPdf = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDocCreatePDF, new FileStream(filePath, FileMode.Create));
                        iTextSharp.text.Paragraph para = new iTextSharp.text.Paragraph(10, stringresult, normal);

                        pdfDocCreatePDF.Open();
                        pdfDocCreatePDF.Add(para);
                        pdfDocCreatePDF.Close();

                        ReadPdfFileAttach(name);

                    }


                }
                else
                {
                    lblError.Text = rm.GetString("noReportData", culture);
                    return;
                }
                setSelection();

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCSchedulingPage.btnCoupons_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCSchedulingPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
        private void ReadPdfFile(string path)
        {

            WebClient client = new WebClient();
            Byte[] buffer = client.DownloadData(path);

            if (buffer != null)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", buffer.Length.ToString());
                Response.TransmitFile(path);
                Response.End();

            }

        }
        private string getTextForPDF(List<object> resultRow, string point, string day, string month, string year, DateTime termin, ResourceManager rm, CultureInfo culture)
        {
            string stringresult = "";
            try
            {

                stringresult += rm.GetString("couponAppointment", culture) + "\t\n\t\n";
                stringresult += rm.GetString("hdrCompany", culture) + ":        " + resultRow[1].ToString() + " \t\n";
                stringresult += rm.GetString("couponPrevention", culture) + "\t\n\t\n";
                stringresult += "_________________________________________________________________________________________________\t\n\t\n";
                if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                {
                    stringresult += String.Format("{0,-21}  {1,-21}", rm.GetString("hdrVisitID", culture) + ":", resultRow[0].ToString()) + " \t\n";
                    stringresult += String.Format("{0,-21}  {1,-21}", rm.GetString("hdrStringone", culture) + ":", resultRow[2].ToString() + "." + resultRow[4] + "." + resultRow[5] + "." + resultRow[6]) + " \t\n";
                    stringresult += String.Format("{0,-25}  {1,-25}", rm.GetString("hdrEmplType", culture) + ":", resultRow[7].ToString()) + " \t\n";
                    stringresult += String.Format("{0,-26}  {1,-26}", rm.GetString("hdrID", culture) + ":", resultRow[8].ToString()) + " \t\n";
                    stringresult += String.Format("{0,-21}  {1,-21}", rm.GetString("hdrEmployee", culture) + ":", resultRow[9].ToString()) + " \t\n";
                    stringresult += String.Format("{0,-24}  {1,-24}", rm.GetString("hdrEmplRisk", culture) + ":", resultRow[12].ToString().Replace("\r\n", ",")) + " \t\n";
                    stringresult += String.Format("{0,-22}  {1,-22}", rm.GetString("hdrShift", culture) + ":", resultRow[10].ToString()) + " \t\n";
                    stringresult += String.Format("{0,-23}  {1,-23}", rm.GetString("hdrInterval", culture) + ":", resultRow[11].ToString()) + " \t\n";
                    stringresult += String.Format("{0,-18}  {1,-18}", rm.GetString("visitMonth", culture) + ":", year + month) + " \t\n\t\n\t\n\t\n";
                }
                else
                {
                    stringresult += String.Format("{0,-23}  {1,-23}", rm.GetString("hdrVisitID", culture) + ":", resultRow[0].ToString()) + " \t\n";
                    stringresult += String.Format("{0,-21}  {1,-21}", rm.GetString("hdrStringone", culture) + ":", resultRow[2].ToString() + "." + resultRow[4] + "." + resultRow[5] + "." + resultRow[6]) + " \t\n";
                    stringresult += String.Format("{0,-24}  {1,-24}", rm.GetString("hdrEmplType", culture) + ":", resultRow[7].ToString()) + " \t\n";
                    stringresult += String.Format("{0,-26}  {1,-26}", rm.GetString("hdrID", culture) + ":", resultRow[8].ToString()) + " \t\n";
                    stringresult += String.Format("{0,-20}  {1,-20}", rm.GetString("hdrEmployee", culture) + ":", resultRow[9].ToString()) + " \t\n";
                    stringresult += String.Format("{0,-25}  {1,-25}", rm.GetString("hdrEmplRisk", culture) + ":", resultRow[12].ToString().Replace("\r\n", ",")) + " \t\n";
                    stringresult += String.Format("{0,-26}  {1,-26}", rm.GetString("hdrShift", culture) + ":", resultRow[10].ToString()) + " \t\n";
                    stringresult += String.Format("{0,-23}  {1,-23}", rm.GetString("hdrInterval", culture) + ":", resultRow[11].ToString()) + " \t\n";
                    stringresult += String.Format("{0,-19}  {1,-19}", rm.GetString("visitMonth", culture) + ":", year + month) + " \t\n\t\n\t\n\t\n";
                }
                stringresult += rm.GetString("visitAppointmentDate", culture) + day + "." + month + "." + year + rm.GetString("visitAppointmentTime", culture) + termin.ToString("HH:mm") + rm.GetString("visitAppointmentAmbulance", culture) + point + " \t\n\t\n\t\n";
                stringresult += "_________________________________________________________________________________________________\t\n\t\n";

                stringresult += "_________________________________________________________________________________________________\t\n\t\n\t\n\t\n\t\n";

                stringresult += rm.GetString("couponResponsible", culture) + "\t\n\t\n";

                stringresult += "                     ________________ __________ _______________\t\n\t\n\t\n\t\n\t\n";

                stringresult += "__________________________________________________________________________________________________\t\n\t\n\t\n\t\n\t\n\t\n";
                stringresult += rm.GetString("couponEmployeeBeen", culture) + "\t\n\t\n\t\n\t\n\n";
                stringresult += rm.GetString("couponSuccVisit", culture) + "\t\n\t\n\t\n\t\n";


                for (int i = 0; i < 14; i++)
                {
                    stringresult += "\t\n";

                }
                stringresult += rm.GetString("couponDoctor", culture) + " ________________________________________________________________________\t\n";

                for (int i = 0; i < 10; i++)
                {
                    stringresult += "\t\n";

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return stringresult;
        }



        private void ReadPdfFileAttach(string name)
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment; filename=" + name);
            Response.Flush();
            Response.End();
        }

        protected void btnAdd_Click(Object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCSchedulingPage).Assembly);

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

                    Response.Redirect("/ACTAWeb/ACTAWebUI/MCEmployeeDataPage.aspx?reloadState=false&emplID=" + emplID.Trim() + "&selPage=OnDemandAppointments&Back=/ACTAWeb/ACTAWebUI/MCSchedulingPage.aspx", false);
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCSchedulingPage.btnAdd_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCSchedulingPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnSchedule_Click(Object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCSchedulingPage).Assembly);

                lblError.Text = "";

                if (lboxEmployees.GetSelectedIndices().Length <= 0)
                    lblError.Text = rm.GetString("noSelectedEmployee", culture);
                else
                {
                    string emplID = lboxEmployees.Items[lboxEmployees.GetSelectedIndices()[0]].Value.Trim();

                    string wOptions = "dialogWidth:1010px; dialogHeight:600px; edge:Raised; center:Yes; resizable:Yes; status:Yes;";
                    ClientScript.RegisterStartupScript(GetType(), "schedulePopup", "window.open('/ACTAWeb/ACTAWebUI/EmployeeSchedulesPage.aspx?emplID=" + emplID.Trim() + "', window, '" + wOptions.Trim() + "');", true);
                }

                setSelection();

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCSchedulingPage.btnSchedule_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCSchedulingPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnApply_Click(Object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCSchedulingPage).Assembly);

                lblError.Text = "";

                int selPoint = -1;
                if (cbPoint.Items.Count > 0 && cbPoint.SelectedItem != null)
                {
                    if (!int.TryParse(cbPoint.SelectedItem.Value.Trim(), out selPoint))
                        selPoint = -1;
                }

                if (selPoint == -1)
                {
                    lblError.Text = rm.GetString("noSelectedPoint", culture);
                    setSelection();
                    return;
                }

                List<string> selKeys = getSelection();
                if (selKeys.Count <= 0)
                {
                    lblError.Text = rm.GetString("noSelectedRecord", culture);
                    setSelection();
                    return;
                }

                Dictionary<string, Dictionary<int, string>> chgKeys = getChanges();

                Dictionary<string, int> pointsChangesDict = new Dictionary<string, int>();

                if (Session[pointsChanged] != null && Session[pointsChanged] is Dictionary<string, int>)
                    pointsChangesDict = (Dictionary<string, int>)Session[pointsChanged];

                foreach (string selKey in selKeys)
                {
                    if (!chgKeys.ContainsKey(selKey))
                        chgKeys.Add(selKey, new Dictionary<int, string>());

                    if (!chgKeys[selKey].ContainsKey(Constants.MCSchedulingPointColIndex))
                        chgKeys[selKey].Add(Constants.MCSchedulingPointColIndex, cbPoint.SelectedItem.Text.Trim());
                    else
                        chgKeys[selKey][Constants.MCSchedulingPointColIndex] = cbPoint.SelectedItem.Text.Trim();

                    if (pointsChangesDict.ContainsKey(selKey))
                        pointsChangesDict[selKey] = selPoint;
                    else
                        pointsChangesDict.Add(selKey, selPoint);
                }

                Session[pointsChanged] = pointsChangesDict;

                setChanges(chgKeys);

                setSelection();

                resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?height=340";

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCSchedulingPage.btnApply_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCSchedulingPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnGrpApply_Click(Object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCSchedulingPage).Assembly);

                lblError.Text = "";

                List<string> selKeys = getSelection();
                if (selKeys.Count <= 0)
                {
                    lblError.Text = rm.GetString("noSelectedRecord", culture);
                    setSelection();
                    return;
                }

                // check input parameters validity
                // group must be positive number
                int group = -1;
                if (!int.TryParse(tbGroup.Text.Trim(), out group))
                    group = -1;

                if (group <= 0)
                {
                    lblError.Text = rm.GetString("invalidSchedulingGroup", culture);
                    setSelection();
                    return;
                }

                DateTime terminDate = CommonWeb.Misc.createDate(tbSchDate.Text.Trim());
                if (terminDate.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidSchedulingTermin", culture);
                    setSelection();
                    return;
                }

                DateTime startTime = CommonWeb.Misc.createTime(tbStart.Text.Trim());
                if (startTime.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidSchedulingStartTime", culture);
                    setSelection();
                    return;
                }

                int step = -1;
                if (!int.TryParse(tbStep.Text.Trim(), out step))
                    step = -1;

                if (step < 0)
                {
                    lblError.Text = rm.GetString("invalidSchedulingStep", culture);
                    setSelection();
                    return;
                }

                Dictionary<string, Dictionary<int, string>> chgKeys = getChanges();

                int count = 0;
                DateTime schDate = new DateTime(terminDate.Year, terminDate.Month, terminDate.Day, startTime.Hour, startTime.Minute, 0);

                if (Session[Constants.sessionDataTableList] != null && Session[Constants.sessionDataTableList] is List<List<object>>)
                {
                    List<List<object>> table = (List<List<object>>)Session[Constants.sessionDataTableList];

                    foreach (List<object> row in table)
                    {
                        if (row.Count < 1 || !selKeys.Contains(row[0].ToString()))
                            continue;

                        if (!chgKeys.ContainsKey(row[0].ToString()))
                            chgKeys.Add(row[0].ToString(), new Dictionary<int, string>());

                        if (!chgKeys[row[0].ToString()].ContainsKey(Constants.MCSchedulingTerminColIndex))
                            chgKeys[row[0].ToString()].Add(Constants.MCSchedulingTerminColIndex, schDate.ToString(Constants.timeFormat));
                        else
                            chgKeys[row[0].ToString()][Constants.MCSchedulingTerminColIndex] = schDate.ToString(Constants.timeFormat);

                        if (!chgKeys[row[0].ToString()].ContainsKey(Constants.MCSchedulingDColIndex))
                            chgKeys[row[0].ToString()].Add(Constants.MCSchedulingDColIndex, schDate.Day.ToString());
                        else
                            chgKeys[row[0].ToString()][Constants.MCSchedulingDColIndex] = schDate.Day.ToString();

                        if (!chgKeys[row[0].ToString()].ContainsKey(Constants.MCSchedulingMColIndex))
                            chgKeys[row[0].ToString()].Add(Constants.MCSchedulingMColIndex, schDate.Month.ToString());
                        else
                            chgKeys[row[0].ToString()][Constants.MCSchedulingMColIndex] = schDate.Month.ToString();

                        if (!chgKeys[row[0].ToString()].ContainsKey(Constants.MCSchedulingYColIndex))
                            chgKeys[row[0].ToString()].Add(Constants.MCSchedulingYColIndex, schDate.Year.ToString());
                        else
                            chgKeys[row[0].ToString()][Constants.MCSchedulingYColIndex] = schDate.Year.ToString();

                        count++;

                        if (count == group)
                        {
                            count = 0;
                            schDate = schDate.AddMinutes(step);
                        }
                    }
                }

                setChanges(chgKeys);

                setSelection();

                resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?height=340";

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCSchedulingPage.btnGrpApply_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCSchedulingPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void rbSelectAll_CheckedChanged(Object sender, EventArgs e)
        {
            try
            {
                rbDeselectAll.Checked = !rbSelectAll.Checked;

                if (rbSelectAll.Checked)
                {
                    SelBox.Value = "";

                    if (Session[Constants.sessionDataTableList] != null && Session[Constants.sessionDataTableList] is List<List<object>>)
                    {
                        foreach (List<object> row in (List<List<object>>)Session[Constants.sessionDataTableList])
                        {
                            if (row.Count > 0)
                                SelBox.Value += row[0].ToString() + Constants.delimiter;
                        }

                        setSelection();
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCSchedulingPage.rbSelectAll_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCSchedulingPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void rbDeselectAll_CheckedChanged(Object sender, EventArgs e)
        {
            try
            {
                rbSelectAll.Checked = !rbDeselectAll.Checked;

                if (rbDeselectAll.Checked)
                {
                    SelBox.Value = "";
                    setSelection();
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCSchedulingPage.rbDeselectAll_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCSchedulingPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void chbDeleted_OnCheckChanged(object sender, EventArgs e)
        {
            try
            {
                populateStatus();

                setSelection();

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCSchedulingPage.chbDeleted_OnCheckChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCSchedulingPage.aspx&Header=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setSelection()
        {
            try
            {
                // put selection and changes in session                
                Session[Constants.sessionSelectedKeys] = getSelection();
                Session[Constants.sessionChangedKeys] = getChanges();
                Session[Constants.sessionSamePage] = true;
            }
            catch (Exception ex)
            {
                throw ex;
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

        private Dictionary<string, Dictionary<int, string>> getChanges()
        {
            try
            {
                Dictionary<string, Dictionary<int, string>> changedKeys = new Dictionary<string, Dictionary<int, string>>();

                string[] changedRows = ChangedBox.Value.Trim().Split(Constants.rowDelimiter);

                foreach (string row in changedRows)
                {
                    string[] changedValues = row.Trim().Split(Constants.delimiter);

                    if (changedValues.Length == 3)
                    {
                        if (!changedKeys.ContainsKey(changedValues[0]))
                            changedKeys.Add(changedValues[0], new Dictionary<int, string>());

                        int colIndex = -1;
                        if (!int.TryParse(changedValues[1], out colIndex))
                            colIndex = -1;

                        if (colIndex >= 0)
                        {
                            if (!changedKeys[changedValues[0]].ContainsKey(colIndex))
                                changedKeys[changedValues[0]].Add(colIndex, "");

                            changedKeys[changedValues[0]][colIndex] = changedValues[2];
                        }
                    }
                }

                return changedKeys;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void setChanges(Dictionary<string, Dictionary<int, string>> chgKeys)
        {
            try
            {
                string chgValue = "";

                foreach (string row in chgKeys.Keys)
                {
                    foreach (int col in chgKeys[row].Keys)
                    {
                        chgValue += row.Trim() + Constants.delimiter + col.ToString().Trim() + Constants.delimiter + chgKeys[row][col].Trim() + Constants.rowDelimiter;
                    }
                }

                ChangedBox.Value = chgValue.Trim();

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

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "MCSchedulingPage.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "MCSchedulingPage.", filterState);
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

