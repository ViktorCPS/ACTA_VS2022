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
    public partial class BufferReport : System.Web.UI.Page
    {
        const string pageName = "BufferReport";

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
                    tbEmployee.Attributes.Add("onKeyUp", "return selectItem('tbEmployee', 'lboxEmployees');");
                    btnShow.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnReport.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    cbSelectAllEmpolyees.Attributes.Add("onclick", "return selectListItems('cbSelectAllEmpolyees', 'lboxEmployees');");

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

                    //btnReport.Visible = false;

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

                    InitializeSQLParameters(true);

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
                            populateEmplTypes();
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
                            populateEmplTypes();
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in BufferReport.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnPrevDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(BufferReport).Assembly);

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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in BufferReport.btnPrevDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/BufferReport.aspx&Header=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnNextDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(BufferReport).Assembly);

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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in BufferReport.btnNextDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/BufferReport.aspx&Header=" + Constants.trueValue.Trim(), false);
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in BufferReport.cbEmplType_SelectedIndexChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/BufferReport.aspx&Header=" + Constants.falseValue.Trim(), false);
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in BufferReport.Date_Changed(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/BufferReport.aspx&Header=" + Constants.falseValue.Trim(), false);
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(BufferReport).Assembly);

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

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(BufferReport).Assembly);

                lblEmplType.Text = rm.GetString("lblEmployeeType", culture);
                lblEmployee.Text = rm.GetString("lblEmployeeSelection", culture);
                lblPeriod.Text = rm.GetString("lblPeriod", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblLegend.Text = "*" + rm.GetString("BH", culture) + " " + rm.GetString("ALP", culture) + " " + rm.GetString("ALC", culture) + " " + rm.GetString("AL", culture)
                    + " " + rm.GetString("SW", culture) + " " + rm.GetString("PL", culture) + " " + rm.GetString("ON", culture);

                cbSelectAllEmpolyees.Text = rm.GetString("lblSelectAll", culture);
                cbONRecalculated.Text = rm.GetString("cbONRecalculated", culture);
                cbONCurrentMonth.Text = rm.GetString("cbONCurrentMonth", culture);

                btnShow.Text = rm.GetString("btnShow", culture);
                btnReport.Text = rm.GetString("btnReport", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeSQLParameters(bool emplDataShow)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(BufferReport).Assembly);

                Session[Constants.sessionHeader] = "";
                Session[Constants.sessionFields] = "";
                if (emplDataShow)
                {
                    Session[Constants.sessionHeader] += rm.GetString("hdrEmployeeID", culture) + "," + rm.GetString("hdrStringone", culture) + "," + rm.GetString("hdrEmployee", culture)
                        + "," + rm.GetString("hdrCostCentre", culture) + "," + rm.GetString("hdrDesc", culture) + "," + rm.GetString("hdrEmplType", culture) + ","
                        + rm.GetString("hdrBranch", culture) + ",";
                    Session[Constants.sessionFields] += "emplID, stringone, empl, cc, ccDesc, type, branch, ";
                }
                Session[Constants.sessionHeader] += rm.GetString("hdrBeforeBH", culture) + "," + rm.GetString("hdrPeriodBH", culture) + "," + rm.GetString("hdrAfterBH", culture)
                    + "," + rm.GetString("hdrPrevALBeforeLeft", culture) + "," + rm.GetString("hdrCurrALBeforeLeft", culture) + "," + rm.GetString("hdrALBeforeLeft", culture)
                    + "," + rm.GetString("hdrPrevALPeriodUsed", culture) + "," + rm.GetString("hdrCurrALPeriodUsed", culture) + "," + rm.GetString("hdrALPeriodUsed", culture)
                    + "," + rm.GetString("hdrPrevALAfterLeft", culture) + "," + rm.GetString("hdrCurrALAfterLeft", culture) + "," + rm.GetString("hdrALAfterLeft", culture)
                    + "," + rm.GetString("hdrBeforeSW", culture) + "," + rm.GetString("hdrPeriodSW", culture) + "," + rm.GetString("hdrAfterSW", culture)
                    + "," + rm.GetString("hdrBeforePL", culture) + "," + rm.GetString("hdrPeriodPL", culture) + "," + rm.GetString("hdrAfterPL", culture)
                    + "," + rm.GetString("hdrBeforeOvertimeKeep", culture) + "," + rm.GetString("hdrPeriodOvertimeKeep", culture) + "," + rm.GetString("hdrAfterOvertimeKeep", culture);
                Session[Constants.sessionFields] += "bhBefore, bhPeriod, bhAfter, alPrevLeftBefore, alCurrLeftBefore, alLeftBefore, "
                    + "alPrevUsedPeriod, alCurrUsedPeriod, alUsedPeriod, alPrevLeftAfter, alCurrLeftAfter, alLeftAfter, swBefore, swPeriod, swAfter, plBefore, plUsed, plAfter, "
                    + "oKeepBefore, oKeepPeriod, oKeepAfter";
                Dictionary<int, int> formating = new Dictionary<int, int>();
                if (!emplDataShow)
                {
                    formating.Add(0, (int)Constants.FormatTypes.DoubleFormat);
                    formating.Add(1, (int)Constants.FormatTypes.DoubleFormat);
                    formating.Add(2, (int)Constants.FormatTypes.DoubleFormat);
                    formating.Add(12, (int)Constants.FormatTypes.DoubleFormat);
                    formating.Add(13, (int)Constants.FormatTypes.DoubleFormat);
                    formating.Add(14, (int)Constants.FormatTypes.DoubleFormat);
                    formating.Add(18, (int)Constants.FormatTypes.DoubleFormat);
                    formating.Add(19, (int)Constants.FormatTypes.DoubleFormat);
                    formating.Add(20, (int)Constants.FormatTypes.DoubleFormat);
                }
                else
                {
                    formating.Add(7, (int)Constants.FormatTypes.DoubleFormat);
                    formating.Add(8, (int)Constants.FormatTypes.DoubleFormat);
                    formating.Add(9, (int)Constants.FormatTypes.DoubleFormat);
                    formating.Add(19, (int)Constants.FormatTypes.DoubleFormat);
                    formating.Add(20, (int)Constants.FormatTypes.DoubleFormat);
                    formating.Add(21, (int)Constants.FormatTypes.DoubleFormat);
                    formating.Add(25, (int)Constants.FormatTypes.DoubleFormat);
                    formating.Add(26, (int)Constants.FormatTypes.DoubleFormat);
                    formating.Add(27, (int)Constants.FormatTypes.DoubleFormat);
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in BufferReport.Menu1_MenuItemClick(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/BufferReport.aspx&Header=" + Constants.falseValue.Trim(), false);
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(BufferReport).Assembly);

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

                DateTime fromMonth = new DateTime(fromDate.Year, fromDate.Month, 1);
                DateTime toMonth = new DateTime(toDate.Year, toDate.Month, 1);

                Session[Constants.sessionFromDate] = fromDate;
                Session[Constants.sessionToDate] = toDate;

                Dictionary<int, WorkingUnitTO> WUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();

                // get all employee types, key is employee_type, value name for that company
                Dictionary<int, Dictionary<int, string>> emplTypes = new EmployeeType(Session[Constants.sessionConnection]).SearchDictionary();

                // get selected employees and additional data for branch and stringone
                string emplIDs = "";
                bool emplSelected = false;
                if (lboxEmployees.GetSelectedIndices().Length > 0)
                {
                    emplSelected = true;
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

                InitializeSQLParameters(emplSelected);

                List<EmployeeTO> emplList = new Employee(Session[Constants.sessionConnection]).Search(emplIDs);

                Dictionary<int, EmployeeTO> employees = new Dictionary<int, EmployeeTO>();
                foreach (EmployeeTO empl in emplList)
                {
                    if (!employees.ContainsKey(empl.EmployeeID))
                        employees.Add(empl.EmployeeID, empl);
                    else
                        employees[empl.EmployeeID] = empl;
                }

                // stringone, costcenter, ute, workgruop, branch
                List<EmployeeAsco4TO> emplAscoList = new EmployeeAsco4(Session[Constants.sessionConnection]).Search(emplIDs);
                Dictionary<int, string> stringoneList = new Dictionary<int, string>();
                Dictionary<int, string> branchList = new Dictionary<int, string>();
                Dictionary<int, string> uteList = new Dictionary<int, string>();
                Dictionary<int, string> workgroupList = new Dictionary<int, string>();
                Dictionary<int, string> costcenterList = new Dictionary<int, string>();
                Dictionary<int, string> costcenterDescList = new Dictionary<int, string>();
                Dictionary<int, string> emplTypeList = new Dictionary<int, string>();

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

                    if (!stringoneList.ContainsKey(asco.EmployeeID))
                        stringoneList.Add(asco.EmployeeID, asco.NVarcharValue2.Trim());
                    else
                        stringoneList[asco.EmployeeID] = asco.NVarcharValue2.Trim();

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

                    string cost = tempWU.Code.Trim();
                    string costDescription = tempWU.Description.Trim();

                    if (WUnits.ContainsKey(tempWU.ParentWorkingUID))
                        tempWU = WUnits[tempWU.ParentWorkingUID];

                    string plant = tempWU.Code.Trim();
                    if (!costcenterList.ContainsKey(asco.EmployeeID))
                        costcenterList.Add(asco.EmployeeID, plant + cost);
                    else
                        costcenterList[asco.EmployeeID] = plant + cost;

                    if (!costcenterDescList.ContainsKey(asco.EmployeeID))
                        costcenterDescList.Add(asco.EmployeeID, costDescription);
                    else
                        costcenterDescList[asco.EmployeeID] = costDescription;
                }

                // get dictionary of all rules, key is company and value are rules by employee type id
                Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> emplRules = new Common.Rule(Session[Constants.sessionConnection]).SearchWUEmplTypeDictionary();

                // get all current counter values
                Dictionary<int, Dictionary<int, int>> emplCounters = new EmployeeCounterValue(Session[Constants.sessionConnection]).Search(emplIDs);

                // get paid buffer's values for selected employees
                Dictionary<int, Dictionary<DateTime, Dictionary<int, BufferMonthlyBalancePaidTO>>> buffersPaidDict = new BufferMonthlyBalancePaid(Session[Constants.sessionConnection]).SearchEmployeeBalancesPaid(emplIDs, fromDate.Date, new DateTime());

                // get all pairs after week beginning of from date (before 10h shift annual leave days)
                DateTime startPairDate = fromDate.Date;
                DateTime notCalculatedONDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime currMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                if (!cbONRecalculated.Checked)
                    notCalculatedONDate = notCalculatedONDate.AddMonths(-1);

                if (notCalculatedONDate.Date < startPairDate.Date)
                    startPairDate = notCalculatedONDate.Date;

                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplPairs = new IOPairProcessed(Session[Constants.sessionConnection]).getPairsForInterval(Common.Misc.getWeekBeggining(startPairDate.Date), new DateTime(), emplIDs);
                
                // get pass types
                Dictionary<int, PassTypeTO> passTypes = new PassType(Session[Constants.sessionConnection]).SearchDictionary();

                // bank hours dicitionaries
                Dictionary<int, double> emplBHBeforePeriodLeft = new Dictionary<int, double>();
                Dictionary<int, double> emplBHPeriodUsed = new Dictionary<int, double>();
                Dictionary<int, double> emplBHAfterPeriodLeft = new Dictionary<int, double>();
                double totalEmplBHBeforePeriodLeft = 0;
                double totalEmplBHPeriodUsed = 0;
                double totalEmplBHAfterPeriodLeft = 0;

                // annual leaves dicitionaries previous year
                Dictionary<int, int> emplALPrevBeforePeriodLeft = new Dictionary<int, int>();
                Dictionary<int, int> emplALPrevPeriodUsed = new Dictionary<int, int>();
                Dictionary<int, int> emplALPrevAfterPeriodLeft = new Dictionary<int, int>();
                int totalEmplALPrevBeforePeriodLeft = 0;
                int totalEmplALPrevPeriodUsed = 0;
                int totalEmplALPrevAfterPeriodLeft = 0;

                // annual leaves dicitionaries current year
                Dictionary<int, int> emplALCurrBeforePeriodLeft = new Dictionary<int, int>();
                Dictionary<int, int> emplALCurrPeriodUsed = new Dictionary<int, int>();
                Dictionary<int, int> emplALCurrAfterPeriodLeft = new Dictionary<int, int>();
                int totalEmplALCurrBeforePeriodLeft = 0;
                int totalEmplALCurrPeriodUsed = 0;
                int totalEmplALCurrAfterPeriodLeft = 0;

                // annual leaves dicitionaries
                Dictionary<int, int> emplALBeforePeriodLeft = new Dictionary<int, int>();
                Dictionary<int, int> emplALPeriodUsed = new Dictionary<int, int>();
                Dictionary<int, int> emplALAfterPeriodLeft = new Dictionary<int, int>();
                int totalEmplALBeforePeriodLeft = 0;
                int totalEmplALPeriodUsed = 0;
                int totalEmplALAfterPeriodLeft = 0;

                // stop working dicitionaries
                Dictionary<int, double> emplSWBeforePeriodLeft = new Dictionary<int, double>();
                Dictionary<int, double> emplSWPeriodUsed = new Dictionary<int, double>();
                Dictionary<int, double> emplSWAfterPeriodLeft = new Dictionary<int, double>();
                double totalEmplSWBeforePeriodLeft = 0;
                double totalEmplSWPeriodUsed = 0;
                double totalEmplSWAfterPeriodLeft = 0;

                // paid leaves dicitionaries
                Dictionary<int, int> emplPLBeforePeriodLeft = new Dictionary<int, int>();
                Dictionary<int, int> emplPLPeriodUsed = new Dictionary<int, int>();
                Dictionary<int, int> emplPLAfterPeriodLeft = new Dictionary<int, int>();
                int totalEmplPLBeforePeriodLeft = 0;
                int totalEmplPLPeriodUsed = 0;
                int totalEmplPLAfterPeriodLeft = 0;

                // overtime not justified dicitionaries
                Dictionary<int, double> emplONBeforePeriodLeft = new Dictionary<int, double>();
                Dictionary<int, double> emplONPeriodUsed = new Dictionary<int, double>();
                Dictionary<int, double> emplONAfterPeriodLeft = new Dictionary<int, double>();
                double totalEmplONBeforePeriodLeft = 0;
                double totalEmplONPeriodUsed = 0;
                double totalEmplONAfterPeriodLeft = 0;

                foreach (int emplID in employees.Keys)
                {
                    // counter values on from date
                    int alCurrCounterValue = 0;
                    int alPrevCounterValue = 0;
                    int alUsedCounterValue = 0;
                    int alUsedCounterValueFromPaid = 0;
                    int alUsedCounterValueToPaid = 0;
                    int plCounterValue = 0;
                    int bhCounterValue = 0;
                    int bhCounterValueFromPaid = 0;
                    int bhCounterValueToPaid = 0;
                    int swCounterValue = 0;
                    int swCounterValueFromPaid = 0;
                    int swCounterValueToPaid = 0;
                    int onCounterValue = 0;
                    
                    // get current counter values
                    if (emplCounters.ContainsKey(emplID))
                    {
                        if (emplCounters[emplID].ContainsKey((int)Constants.EmplCounterTypes.AnnualLeaveCounter))                        
                            alCurrCounterValue = emplCounters[emplID][(int)Constants.EmplCounterTypes.AnnualLeaveCounter];
                        
                        if (emplCounters[emplID].ContainsKey((int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter))                        
                            alPrevCounterValue = emplCounters[emplID][(int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter];

                        if (emplCounters[emplID].ContainsKey((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter))                        
                            alUsedCounterValue = emplCounters[emplID][(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter];
                            
                        if (emplCounters[emplID].ContainsKey((int)Constants.EmplCounterTypes.PaidLeaveCounter))                        
                            plCounterValue = emplCounters[emplID][(int)Constants.EmplCounterTypes.PaidLeaveCounter];

                        if (emplCounters[emplID].ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter))
                            bhCounterValue = emplCounters[emplID][(int)Constants.EmplCounterTypes.BankHoursCounter];
                            
                        if (emplCounters[emplID].ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter))                        
                            swCounterValue = emplCounters[emplID][(int)Constants.EmplCounterTypes.StopWorkingCounter];
                            
                        if (emplCounters[emplID].ContainsKey((int)Constants.EmplCounterTypes.NotJustifiedOvertime))                        
                            onCounterValue = emplCounters[emplID][(int)Constants.EmplCounterTypes.NotJustifiedOvertime];                            
                    }

                    if (buffersPaidDict.ContainsKey(emplID))
                    {
                        foreach (DateTime month in buffersPaidDict[emplID].Keys)
                        {
                            if (month.Date >= fromMonth.Date)
                            {
                                // add all paid bank hours for from month and after
                                if (buffersPaidDict[emplID][month].ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter))
                                    bhCounterValueFromPaid += buffersPaidDict[emplID][month][(int)Constants.EmplCounterTypes.BankHoursCounter].ValuePaid;

                                // add all paid days of holliday left of previous year for from month and after
                                if (buffersPaidDict[emplID][month].ContainsKey((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter))
                                    alUsedCounterValueFromPaid += buffersPaidDict[emplID][month][(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter].ValuePaid;

                                // add all paid stop working hours for from month and after
                                if (buffersPaidDict[emplID][month].ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter))
                                    swCounterValueFromPaid += buffersPaidDict[emplID][month][(int)Constants.EmplCounterTypes.StopWorkingCounter].ValuePaid;
                            }

                            if (month.Date >= toMonth.Date)
                            {
                                // add all paid bank hours for from month and after
                                if (buffersPaidDict[emplID][month].ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter))
                                    bhCounterValueToPaid += buffersPaidDict[emplID][month][(int)Constants.EmplCounterTypes.BankHoursCounter].ValuePaid;

                                // add all paid days of holliday left of previous year for from month and after
                                if (buffersPaidDict[emplID][month].ContainsKey((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter))
                                    alUsedCounterValueToPaid += buffersPaidDict[emplID][month][(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter].ValuePaid;

                                // add all paid stop working hours for from month and after
                                if (buffersPaidDict[emplID][month].ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter))
                                    swCounterValueToPaid += buffersPaidDict[emplID][month][(int)Constants.EmplCounterTypes.StopWorkingCounter].ValuePaid;
                            }
                        }
                    }

                    int bhUsedPeriod = 0;
                    int bhUsedAfter = 0;
                    int alDaysPeriod = 0;
                    int alDaysAfter = 0;
                    int swUsedPeriod = 0;
                    int swUsedAfter = 0;
                    int plUsedPeriod = 0;
                    int plUsedAfter = 0;
                    int onUsedPeriod = 0;
                    int onUsedAfter = 0;

                    // get employee company
                    int emplCompany = -1;
                    emplCompany = Common.Misc.getRootWorkingUnit(employees[emplID].WorkingUnitID, WUnits);

                    // get employee type name
                    if (!emplTypeList.ContainsKey(emplID))
                        emplTypeList.Add(emplID, "");

                    if (emplTypes.ContainsKey(emplCompany) && emplTypes[emplCompany].ContainsKey(employees[emplID].EmployeeTypeID))
                        emplTypeList[emplID] = emplTypes[emplCompany][employees[emplID].EmployeeTypeID];

                    // get bank hour, bank hour use, annual leave, stop working, stop working done, paid leave type and bank hours rounding
                    int bhType = -1;
                    int bhUsedType = -1;
                    int alType = -1;
                    int calType = -1;
                    int swType = -1;
                    int swDoneType = -1;
                    int bhRounding = 1;
                    int onType = Constants.overtimeUnjustified;
                    int onUsedType = -1;
                    int onRounding = 1;

                    if (emplRules.ContainsKey(emplCompany) && emplRules[emplCompany].ContainsKey(employees[emplID].EmployeeTypeID))
                    {
                        if (emplRules[emplCompany][employees[emplID].EmployeeTypeID].ContainsKey(Constants.RuleCompanyBankHour))
                            bhType = emplRules[emplCompany][employees[emplID].EmployeeTypeID][Constants.RuleCompanyBankHour].RuleValue;

                        if (emplRules[emplCompany][employees[emplID].EmployeeTypeID].ContainsKey(Constants.RuleCompanyBankHourUsed))
                            bhUsedType = emplRules[emplCompany][employees[emplID].EmployeeTypeID][Constants.RuleCompanyBankHourUsed].RuleValue;

                        if (emplRules[emplCompany][employees[emplID].EmployeeTypeID].ContainsKey(Constants.RuleCompanyAnnualLeave))
                            alType = emplRules[emplCompany][employees[emplID].EmployeeTypeID][Constants.RuleCompanyAnnualLeave].RuleValue;

                        if (emplRules[emplCompany][employees[emplID].EmployeeTypeID].ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave))
                            calType = emplRules[emplCompany][employees[emplID].EmployeeTypeID][Constants.RuleCompanyCollectiveAnnualLeave].RuleValue;

                        if (emplRules[emplCompany][employees[emplID].EmployeeTypeID].ContainsKey(Constants.RuleCompanyStopWorking))
                            swType = emplRules[emplCompany][employees[emplID].EmployeeTypeID][Constants.RuleCompanyStopWorking].RuleValue;

                        if (emplRules[emplCompany][employees[emplID].EmployeeTypeID].ContainsKey(Constants.RuleCompanyStopWorkingDone))
                            swDoneType = emplRules[emplCompany][employees[emplID].EmployeeTypeID][Constants.RuleCompanyStopWorkingDone].RuleValue;

                        if (emplRules[emplCompany][employees[emplID].EmployeeTypeID].ContainsKey(Constants.RuleBankHoursUsedRounding))
                            bhRounding = emplRules[emplCompany][employees[emplID].EmployeeTypeID][Constants.RuleBankHoursUsedRounding].RuleValue;

                        if (emplRules[emplCompany][employees[emplID].EmployeeTypeID].ContainsKey(Constants.RuleCompanyInitialOvertime))
                            onType = emplRules[emplCompany][employees[emplID].EmployeeTypeID][Constants.RuleCompanyInitialOvertime].RuleValue;

                        if (emplRules[emplCompany][employees[emplID].EmployeeTypeID].ContainsKey(Constants.RuleCompanyInitialOvertimeUsed))
                            onUsedType = emplRules[emplCompany][employees[emplID].EmployeeTypeID][Constants.RuleCompanyInitialOvertimeUsed].RuleValue;

                        if (emplRules[emplCompany][employees[emplID].EmployeeTypeID].ContainsKey(Constants.RuleInitialOvertimeUsedRounding))
                            onRounding = emplRules[emplCompany][employees[emplID].EmployeeTypeID][Constants.RuleInitialOvertimeUsedRounding].RuleValue;
                    }

                    // add key in all dictionaries
                    if (!emplBHBeforePeriodLeft.ContainsKey(emplID))
                        emplBHBeforePeriodLeft.Add(emplID, 0);

                    if (!emplBHPeriodUsed.ContainsKey(emplID))
                        emplBHPeriodUsed.Add(emplID, 0);

                    if (!emplBHAfterPeriodLeft.ContainsKey(emplID))
                        emplBHAfterPeriodLeft.Add(emplID, 0);

                    if (!emplALPrevBeforePeriodLeft.ContainsKey(emplID))
                        emplALPrevBeforePeriodLeft.Add(emplID, 0);

                    if (!emplALPrevPeriodUsed.ContainsKey(emplID))
                        emplALPrevPeriodUsed.Add(emplID, 0);

                    if (!emplALPrevAfterPeriodLeft.ContainsKey(emplID))
                        emplALPrevAfterPeriodLeft.Add(emplID, 0);

                    if (!emplALCurrBeforePeriodLeft.ContainsKey(emplID))
                        emplALCurrBeforePeriodLeft.Add(emplID, 0);

                    if (!emplALCurrPeriodUsed.ContainsKey(emplID))
                        emplALCurrPeriodUsed.Add(emplID, 0);

                    if (!emplALCurrAfterPeriodLeft.ContainsKey(emplID))
                        emplALCurrAfterPeriodLeft.Add(emplID, 0);

                    if (!emplALBeforePeriodLeft.ContainsKey(emplID))
                        emplALBeforePeriodLeft.Add(emplID, 0);

                    if (!emplALPeriodUsed.ContainsKey(emplID))
                        emplALPeriodUsed.Add(emplID, 0);

                    if (!emplALAfterPeriodLeft.ContainsKey(emplID))
                        emplALAfterPeriodLeft.Add(emplID, 0);

                    if (!emplSWBeforePeriodLeft.ContainsKey(emplID))
                        emplSWBeforePeriodLeft.Add(emplID, 0);

                    if (!emplSWPeriodUsed.ContainsKey(emplID))
                        emplSWPeriodUsed.Add(emplID, 0);

                    if (!emplSWAfterPeriodLeft.ContainsKey(emplID))
                        emplSWAfterPeriodLeft.Add(emplID, 0);

                    if (!emplPLBeforePeriodLeft.ContainsKey(emplID))
                        emplPLBeforePeriodLeft.Add(emplID, 0);

                    if (!emplPLPeriodUsed.ContainsKey(emplID))
                        emplPLPeriodUsed.Add(emplID, 0);

                    if (!emplPLAfterPeriodLeft.ContainsKey(emplID))
                        emplPLAfterPeriodLeft.Add(emplID, 0);

                    if (!emplONBeforePeriodLeft.ContainsKey(emplID))
                        emplONBeforePeriodLeft.Add(emplID, 0);

                    if (!emplONPeriodUsed.ContainsKey(emplID))
                        emplONPeriodUsed.Add(emplID, 0);

                    if (!emplONAfterPeriodLeft.ContainsKey(emplID))
                        emplONAfterPeriodLeft.Add(emplID, 0);

                    if (emplPairs.ContainsKey(emplID))
                    {
                        int alWeekDays = 0;
                        DateTime prevDateWeekBeginning = new DateTime();
                        foreach (DateTime date in emplPairs[emplID].Keys)
                        {
                            DateTime weekBeginning = Common.Misc.getWeekBeggining(date.Date);
                            if (!weekBeginning.Date.Equals(prevDateWeekBeginning.Date))
                            {
                                prevDateWeekBeginning = weekBeginning.Date;
                                alWeekDays = 0;
                            }

                            List<IOPairProcessedTO> dayPairs = emplPairs[emplID][date];

                            bool previousDayPair = false;
                            for (int i = 0; i < dayPairs.Count; i++)
                            {
                                if (dayPairs[i].StartTime.Hour == 0 && dayPairs[i].StartTime.Minute == 0)
                                    previousDayPair = true;
                                else if (i > 0)
                                    previousDayPair = previousDayPair && dayPairs[i].StartTime.Equals(dayPairs[i - 1].EndTime);

                                DateTime calcDate = date.Date;

                                if (previousDayPair)
                                    calcDate = date.AddDays(-1).Date;

                                // calculate pair duration
                                int pairDuration = (int)dayPairs[i].EndTime.Subtract(dayPairs[i].StartTime).TotalMinutes;
                                if (dayPairs[i].EndTime.Hour == 23 && dayPairs[i].EndTime.Minute == 59)
                                    pairDuration++;

                                // calculate bank hours
                                if (dayPairs[i].PassTypeID == bhType)
                                {
                                    if (calcDate.Date >= fromDate.Date)
                                    {
                                        if (calcDate.Date <= toDate.Date)
                                            bhUsedPeriod += pairDuration;
                                        else
                                            bhUsedAfter += pairDuration;
                                    }
                                }
                                else if (dayPairs[i].PassTypeID == bhUsedType)
                                {
                                    if (calcDate.Date >= fromDate.Date)
                                    {
                                        if (pairDuration % bhRounding != 0)
                                            pairDuration += bhRounding - (pairDuration % bhRounding);

                                        if (calcDate.Date <= toDate.Date)
                                            bhUsedPeriod -= pairDuration;
                                        else
                                            bhUsedAfter -= pairDuration;
                                    }
                                }
                                // calculate anual leave days
                                else if (dayPairs[i].PassTypeID == alType || dayPairs[i].PassTypeID == calType)
                                {
                                    if (!(dayPairs[i].StartTime.Hour == 0 && dayPairs[i].StartTime.Minute == 0))
                                    {
                                        alWeekDays++;
                                        if (calcDate.Date >= fromDate.Date)
                                        {
                                            int alValue = 1;

                                            // if pair is from night shift, find sum of its and tomorrow pair duration
                                            if (dayPairs[i].EndTime.Hour == 23 && dayPairs[i].EndTime.Minute == 59)
                                            {
                                                if (emplPairs[emplID].ContainsKey(dayPairs[i].IOPairDate.AddDays(1).Date))
                                                {
                                                    foreach (IOPairProcessedTO tomorrowPair in emplPairs[emplID][dayPairs[i].IOPairDate.AddDays(1).Date])
                                                    {
                                                        if ((tomorrowPair.PassTypeID == alType || tomorrowPair.PassTypeID == calType) 
                                                            && tomorrowPair.StartTime.Hour == 0 && tomorrowPair.StartTime.Minute == 0)
                                                        {
                                                            pairDuration += (int)tomorrowPair.EndTime.Subtract(tomorrowPair.StartTime).TotalMinutes;

                                                            if (tomorrowPair.EndTime.Hour == 23 && tomorrowPair.EndTime.Minute == 59)
                                                                pairDuration++;

                                                            break;
                                                        }
                                                    }
                                                }
                                            }

                                            // if duration is greater then standard 8h pair and four days are already used, value of increment annual leave counter is 2
                                            if (pairDuration == Constants.dayDuration10hShift * 60 && alWeekDays == Constants.fullWeek10hShift)
                                                alValue++;

                                            if (calcDate.Date <= toDate.Date)
                                                alDaysPeriod += alValue;
                                            else
                                                alDaysAfter += alValue;
                                        }
                                    }
                                }
                                // calculate stop working
                                else if (dayPairs[i].PassTypeID == swType)
                                {
                                    if (calcDate.Date >= fromDate.Date)
                                    {
                                        if (calcDate.Date <= toDate.Date)
                                            swUsedPeriod += pairDuration;
                                        else
                                            swUsedAfter += pairDuration;
                                    }
                                }
                                else if (dayPairs[i].PassTypeID == swDoneType)
                                {
                                    if (calcDate.Date >= fromDate.Date)
                                    {
                                        if (calcDate.Date <= toDate.Date)
                                            swUsedPeriod -= pairDuration;
                                        else
                                            swUsedAfter -= pairDuration;
                                    }
                                }
                                // calculate paid leaves
                                else if (passTypes.ContainsKey(dayPairs[i].PassTypeID) && passTypes[dayPairs[i].PassTypeID].LimitCompositeID != -1)
                                {
                                    if (calcDate.Date >= fromDate.Date && !(dayPairs[i].StartTime.Hour == 0 && dayPairs[i].StartTime.Minute == 0))
                                    {
                                        if (calcDate.Date <= toDate.Date)
                                            plUsedPeriod++;
                                        else
                                            plUsedAfter++;
                                    }
                                }
                                // calculate ovetime not justified                                
                                else if (dayPairs[i].PassTypeID == onType)
                                {
                                    if (calcDate.Date < currMonth.Date || cbONCurrentMonth.Checked)
                                    {
                                        if (calcDate.Date >= notCalculatedONDate.Date)
                                        {
                                            // increase counter with not calculated values
                                            onCounterValue += pairDuration;
                                        }
                                        if (calcDate.Date >= fromDate.Date)
                                        {
                                            if (calcDate.Date <= toDate.Date)
                                                onUsedPeriod += pairDuration;
                                            else
                                                onUsedAfter += pairDuration;
                                        }
                                    }
                                }
                                else if (dayPairs[i].PassTypeID == onUsedType)
                                {                                    
                                    if (calcDate.Date >= fromDate.Date)
                                    {
                                        if (pairDuration % onRounding != 0)
                                            pairDuration += onRounding - (pairDuration % onRounding);

                                        if (calcDate.Date <= toDate.Date)
                                            onUsedPeriod -= pairDuration;
                                        else
                                            onUsedAfter -= pairDuration;
                                    }
                                }
                            }
                        }
                    }

                    // add hours paid from from month and after
                    emplBHBeforePeriodLeft[emplID] = bhCounterValue - bhUsedPeriod - bhUsedAfter + bhCounterValueFromPaid;
                    emplBHPeriodUsed[emplID] = bhUsedPeriod;
                    // add hours paid from to month and after
                    emplBHAfterPeriodLeft[emplID] = bhCounterValue - bhUsedAfter + bhCounterValueToPaid;                    

                    emplALPrevBeforePeriodLeft[emplID] = getPreviousLeft(alCurrCounterValue, alPrevCounterValue, (alUsedCounterValue - alUsedCounterValueFromPaid) - alDaysPeriod - alDaysAfter);
                    emplALPrevAfterPeriodLeft[emplID] = getPreviousLeft(alCurrCounterValue, alPrevCounterValue, (alUsedCounterValue - alUsedCounterValueToPaid) - alDaysAfter);

                    emplALCurrBeforePeriodLeft[emplID] = getCurrentLeft(alCurrCounterValue, alPrevCounterValue, (alUsedCounterValue - alUsedCounterValueFromPaid) - alDaysPeriod - alDaysAfter);
                    emplALCurrAfterPeriodLeft[emplID] = getCurrentLeft(alCurrCounterValue, alPrevCounterValue, (alUsedCounterValue - alUsedCounterValueToPaid) - alDaysAfter);

                    emplALPrevPeriodUsed[emplID] = getPreviousUsed(emplALCurrBeforePeriodLeft[emplID], emplALPrevBeforePeriodLeft[emplID], alDaysPeriod);
                    emplALCurrPeriodUsed[emplID] = getCurrentUsed(emplALCurrBeforePeriodLeft[emplID], emplALPrevBeforePeriodLeft[emplID], alDaysPeriod);

                    emplALBeforePeriodLeft[emplID] = emplALPrevBeforePeriodLeft[emplID] + emplALCurrBeforePeriodLeft[emplID];
                    emplALPeriodUsed[emplID] = emplALPrevPeriodUsed[emplID] + emplALCurrPeriodUsed[emplID];
                    emplALAfterPeriodLeft[emplID] = emplALPrevAfterPeriodLeft[emplID] + emplALCurrAfterPeriodLeft[emplID];

                    // add hours paid from from month and after
                    emplSWBeforePeriodLeft[emplID] = swCounterValue - swUsedPeriod - swUsedAfter + swCounterValueFromPaid;
                    emplSWPeriodUsed[emplID] = swUsedPeriod;
                    // add hours paid from to month and after
                    emplSWAfterPeriodLeft[emplID] = swCounterValue - swUsedAfter + swCounterValueToPaid;

                    emplPLBeforePeriodLeft[emplID] = plCounterValue - plUsedPeriod - plUsedAfter;
                    emplPLPeriodUsed[emplID] = plUsedPeriod;
                    emplPLAfterPeriodLeft[emplID] = plCounterValue - plUsedAfter;
                                        
                    emplONBeforePeriodLeft[emplID] = onCounterValue - onUsedPeriod - onUsedAfter;
                    emplONPeriodUsed[emplID] = onUsedPeriod;
                    emplONAfterPeriodLeft[emplID] = onCounterValue - onUsedAfter;

                    totalEmplBHBeforePeriodLeft += emplBHBeforePeriodLeft[emplID];
                    totalEmplBHPeriodUsed += emplBHPeriodUsed[emplID];
                    totalEmplBHAfterPeriodLeft += emplBHAfterPeriodLeft[emplID];
                    totalEmplALPrevBeforePeriodLeft += emplALPrevBeforePeriodLeft[emplID];
                    totalEmplALCurrBeforePeriodLeft += emplALCurrBeforePeriodLeft[emplID];
                    totalEmplALBeforePeriodLeft += emplALBeforePeriodLeft[emplID];
                    totalEmplALPrevPeriodUsed += emplALPrevPeriodUsed[emplID];
                    totalEmplALCurrPeriodUsed += emplALCurrPeriodUsed[emplID];
                    totalEmplALPeriodUsed += emplALPeriodUsed[emplID];
                    totalEmplALPrevAfterPeriodLeft += emplALPrevAfterPeriodLeft[emplID];
                    totalEmplALCurrAfterPeriodLeft += emplALCurrAfterPeriodLeft[emplID];
                    totalEmplALAfterPeriodLeft += emplALAfterPeriodLeft[emplID];
                    totalEmplSWBeforePeriodLeft += emplSWBeforePeriodLeft[emplID];
                    totalEmplSWPeriodUsed += emplSWPeriodUsed[emplID];
                    totalEmplSWAfterPeriodLeft += emplSWAfterPeriodLeft[emplID];
                    totalEmplPLBeforePeriodLeft += emplPLBeforePeriodLeft[emplID];
                    totalEmplPLPeriodUsed += emplPLPeriodUsed[emplID];
                    totalEmplPLAfterPeriodLeft += emplPLAfterPeriodLeft[emplID];
                    totalEmplONBeforePeriodLeft += emplONBeforePeriodLeft[emplID];
                    totalEmplONPeriodUsed += emplONPeriodUsed[emplID];
                    totalEmplONAfterPeriodLeft += emplONAfterPeriodLeft[emplID];
                }

                if (emplSelected)
                    createResultTable(employees, stringoneList, costcenterList, costcenterDescList, branchList, emplTypeList, emplBHBeforePeriodLeft, emplBHPeriodUsed, emplBHAfterPeriodLeft,
                        emplALPrevBeforePeriodLeft, emplALPrevPeriodUsed, emplALPrevAfterPeriodLeft, emplALCurrBeforePeriodLeft, emplALCurrPeriodUsed, emplALCurrAfterPeriodLeft,
                        emplALBeforePeriodLeft, emplALPeriodUsed, emplALAfterPeriodLeft, emplSWBeforePeriodLeft, emplSWPeriodUsed, emplSWAfterPeriodLeft, emplPLBeforePeriodLeft,
                        emplPLPeriodUsed, emplPLAfterPeriodLeft, emplONBeforePeriodLeft, emplONPeriodUsed, emplONAfterPeriodLeft);
                else
                    createResultTable(totalEmplBHBeforePeriodLeft, totalEmplBHPeriodUsed, totalEmplBHAfterPeriodLeft, totalEmplALPrevBeforePeriodLeft,
                        totalEmplALPrevPeriodUsed, totalEmplALPrevAfterPeriodLeft, totalEmplALCurrBeforePeriodLeft, totalEmplALCurrPeriodUsed, totalEmplALCurrAfterPeriodLeft,
                        totalEmplALBeforePeriodLeft, totalEmplALPeriodUsed, totalEmplALAfterPeriodLeft, totalEmplSWBeforePeriodLeft, totalEmplSWPeriodUsed, totalEmplSWAfterPeriodLeft,
                        totalEmplPLBeforePeriodLeft, totalEmplPLPeriodUsed, totalEmplPLAfterPeriodLeft, totalEmplONBeforePeriodLeft, totalEmplONPeriodUsed, totalEmplONAfterPeriodLeft);

                resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?showSelection=false";

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in BufferReport.btnShow_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/BufferReport.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnReport_Click(Object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(BufferReport).Assembly);

                string parameterReportType = "";
                if (Session[Constants.sessionDataTableColumns] == null && Session[Constants.sessionDataTableList] == null)
                {
                    lblError.Text = rm.GetString("noReportData", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }
                else
                {
                    List<List<object>> resultTable = new List<List<object>>();
                    List<DataColumn> resultColumns = new List<DataColumn>();

                    if (Session[Constants.sessionDataTableList] != null && Session[Constants.sessionDataTableList] is List<List<object>>)
                        resultTable = (List<List<object>>)Session[Constants.sessionDataTableList];

                    if (Session[Constants.sessionDataTableColumns] != null && Session[Constants.sessionDataTableColumns] is List<DataColumn>)
                        resultColumns = (List<DataColumn>)Session[Constants.sessionDataTableColumns];

                    DataTable tableForReport = new DataTable();
                    if (resultTable.Count > 0)
                    {
                        foreach (DataColumn column in resultColumns)
                        {
                            tableForReport.Columns.Add(column.ColumnName, column.DataType);

                        }
                        if (resultColumns.Count == 21)
                        {
                            tableForReport.Columns.Add("emplID", typeof(int));
                            tableForReport.Columns.Add("stringone", typeof(string));
                            tableForReport.Columns.Add("empl", typeof(string));
                            tableForReport.Columns.Add("cc", typeof(string));
                            tableForReport.Columns.Add("ccDesc", typeof(string));
                            tableForReport.Columns.Add("type", typeof(string));
                            tableForReport.Columns.Add("branch", typeof(string));
                            parameterReportType = "1";
                        }
                        else
                            parameterReportType = "2";

                        foreach (List<object> listRow in resultTable)
                        {
                            DataRow rowReport = tableForReport.NewRow();
                            for (int i = 0; i <= listRow.Count - 1; i++)
                            {
                                rowReport[i] = listRow[i];
                            }
                            tableForReport.Rows.Add(rowReport);
                        }

                        Session["BufferReport.datatable"] = tableForReport;
                        Session["BufferReport.reportType"] = parameterReportType;
                        DateTime fromDate = (DateTime)Session[Constants.sessionFromDate];
                        DateTime toDate = (DateTime)Session[Constants.sessionToDate];
                        Session["BufferReport.fromDate"] = fromDate.ToString("dd.MM.yyyy");
                        Session["BufferReport.toDate"] = toDate.ToString("dd.MM.yyyy");

                        if(Menu1.Items[0].Selected)
                            Session["BufferReport.ute"] = tbUte.Text;
                        else
                            Session["BufferReport.ute"] = tbOrgUte.Text;

                        Session["BufferReport.legend"] = lblLegend.Text;
                        SaveState();

                        string reportURL = "";
                        if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                            reportURL = "/ACTAWeb/ReportsWeb/sr/BufferReport_sr.aspx";
                        else reportURL = "/ACTAWeb/ReportsWeb/en/BufferReport_en.aspx";
                        Response.Redirect("/ACTAWeb/ReportsWeb/ReportPage.aspx?Back=/ACTAWeb/ACTAWebUI/BufferReport.aspx&Report=" + reportURL.Trim(), false);
                        //save data for returning on same page
                        //Session[Constants.sessionSamePage] = true;
                    }
                    else
                    {
                        lblError.Text = rm.GetString("noReportData", culture);
                        writeLog(DateTime.Now, false);
                        return;
                    }
                }
                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in BufferReport.btnReport_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/BufferReport.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void createResultTable(Dictionary<int, EmployeeTO> employees, Dictionary<int, string> stringoneList, Dictionary<int, string> costcenterList, Dictionary<int, string> ccDescList,
            Dictionary<int, string> branchList, Dictionary<int, string> emplTypes, Dictionary<int, double> emplBHBeforePeriodLeft, Dictionary<int, double> emplBHPeriodUsed,
            Dictionary<int, double> emplBHAfterPeriodLeft, Dictionary<int, int> emplALPrevBeforePeriodLeft, Dictionary<int, int> emplALPrevPeriodUsed, Dictionary<int, int> emplALPrevAfterPeriodLeft,
            Dictionary<int, int> emplALCurrBeforePeriodLeft, Dictionary<int, int> emplALCurrPeriodUsed, Dictionary<int, int> emplALCurrAfterPeriodLeft, Dictionary<int, int> emplALBeforePeriodLeft,
            Dictionary<int, int> emplALPeriodUsed, Dictionary<int, int> emplALAfterPeriodLeft, Dictionary<int, double> emplSWBeforePeriodLeft, Dictionary<int, double> emplSWPeriodUsed,
            Dictionary<int, double> emplSWAfterPeriodLeft, Dictionary<int, int> emplPLBeforePeriodLeft, Dictionary<int, int> emplPLPeriodUsed, Dictionary<int, int> emplPLAfterPeriodLeft,
            Dictionary<int, double> emplONBeforePeriodLeft, Dictionary<int, double> emplONPeriodUsed, Dictionary<int, double> emplONAfterPeriodLeft)
        {
            try
            {
                List<List<object>> resultTable = new List<List<object>>();

                // create list of columns for list preview of pairs for inserting
                List<DataColumn> resultColumns = new List<DataColumn>();
                resultColumns.Add(new DataColumn("emplID", typeof(int)));
                resultColumns.Add(new DataColumn("stringone", typeof(string)));
                resultColumns.Add(new DataColumn("empl", typeof(string)));
                resultColumns.Add(new DataColumn("cc", typeof(string)));
                resultColumns.Add(new DataColumn("ccDesc", typeof(string)));
                resultColumns.Add(new DataColumn("type", typeof(string)));
                resultColumns.Add(new DataColumn("branch", typeof(string)));
                resultColumns.Add(new DataColumn("bhBefore", typeof(double)));
                resultColumns.Add(new DataColumn("bhPeriod", typeof(double)));
                resultColumns.Add(new DataColumn("bhAfter", typeof(double)));
                resultColumns.Add(new DataColumn("alPrevLeftBefore", typeof(int)));
                resultColumns.Add(new DataColumn("alCurrLeftBefore", typeof(int)));
                resultColumns.Add(new DataColumn("alLeftBefore", typeof(int)));
                resultColumns.Add(new DataColumn("alPrevUsedPeriod", typeof(int)));
                resultColumns.Add(new DataColumn("alCurrUsedPeriod", typeof(int)));
                resultColumns.Add(new DataColumn("alUsedPeriod", typeof(int)));
                resultColumns.Add(new DataColumn("alPrevLeftAfter", typeof(int)));
                resultColumns.Add(new DataColumn("alCurrLeftAfter", typeof(int)));
                resultColumns.Add(new DataColumn("alLeftAfter", typeof(int)));
                resultColumns.Add(new DataColumn("swBefore", typeof(double)));
                resultColumns.Add(new DataColumn("swPeriod", typeof(double)));
                resultColumns.Add(new DataColumn("swAfter", typeof(double)));
                resultColumns.Add(new DataColumn("plBefore", typeof(int)));
                resultColumns.Add(new DataColumn("plUsed", typeof(int)));
                resultColumns.Add(new DataColumn("plAfter", typeof(int)));
                resultColumns.Add(new DataColumn("oKeepBefore", typeof(double)));
                resultColumns.Add(new DataColumn("oKeepPeriod", typeof(double)));
                resultColumns.Add(new DataColumn("oKeepAfter", typeof(double)));

                foreach (int emplID in employees.Keys)
                {
                    // create result row
                    List<object> resultRow = new List<object>();

                    resultRow.Add(emplID);
                    if (stringoneList.ContainsKey(emplID))
                        resultRow.Add(stringoneList[emplID].Trim());
                    else
                        resultRow.Add("");
                    resultRow.Add(employees[emplID].FirstAndLastName.Trim());
                    if (costcenterList.ContainsKey(emplID))
                        resultRow.Add(costcenterList[emplID].Trim());
                    else
                        resultRow.Add("");
                    if (ccDescList.ContainsKey(emplID))
                        resultRow.Add(ccDescList[emplID].Trim());
                    else
                        resultRow.Add("");
                    if (emplTypes.ContainsKey(emplID))
                        resultRow.Add(emplTypes[emplID].Trim());
                    else
                        resultRow.Add("");
                    if (branchList.ContainsKey(emplID))
                        resultRow.Add(branchList[emplID].Trim());
                    else
                        resultRow.Add("");
                    if (emplBHBeforePeriodLeft.ContainsKey(emplID))
                        resultRow.Add(emplBHBeforePeriodLeft[emplID] / 60);
                    else
                        resultRow.Add(0);
                    if (emplBHPeriodUsed.ContainsKey(emplID))
                        resultRow.Add(emplBHPeriodUsed[emplID] / 60);
                    else
                        resultRow.Add(0);
                    if (emplBHAfterPeriodLeft.ContainsKey(emplID))
                        resultRow.Add(emplBHAfterPeriodLeft[emplID] / 60);
                    else
                        resultRow.Add(0);
                    if (emplALPrevBeforePeriodLeft.ContainsKey(emplID))
                        resultRow.Add(emplALPrevBeforePeriodLeft[emplID]);
                    else
                        resultRow.Add(0);
                    if (emplALCurrBeforePeriodLeft.ContainsKey(emplID))
                        resultRow.Add(emplALCurrBeforePeriodLeft[emplID]);
                    else
                        resultRow.Add(0);
                    if (emplALBeforePeriodLeft.ContainsKey(emplID))
                        resultRow.Add(emplALBeforePeriodLeft[emplID]);
                    else
                        resultRow.Add(0);
                    if (emplALPrevPeriodUsed.ContainsKey(emplID))
                        resultRow.Add(emplALPrevPeriodUsed[emplID]);
                    else
                        resultRow.Add(0);
                    if (emplALCurrPeriodUsed.ContainsKey(emplID))
                        resultRow.Add(emplALCurrPeriodUsed[emplID]);
                    else
                        resultRow.Add(0);
                    if (emplALPeriodUsed.ContainsKey(emplID))
                        resultRow.Add(emplALPeriodUsed[emplID]);
                    else
                        resultRow.Add(0);
                    if (emplALPrevAfterPeriodLeft.ContainsKey(emplID))
                        resultRow.Add(emplALPrevAfterPeriodLeft[emplID]);
                    else
                        resultRow.Add(0);
                    if (emplALCurrAfterPeriodLeft.ContainsKey(emplID))
                        resultRow.Add(emplALCurrAfterPeriodLeft[emplID]);
                    else
                        resultRow.Add(0);
                    if (emplALAfterPeriodLeft.ContainsKey(emplID))
                        resultRow.Add(emplALAfterPeriodLeft[emplID]);
                    else
                        resultRow.Add(0);
                    if (emplSWBeforePeriodLeft.ContainsKey(emplID))
                        resultRow.Add(emplSWBeforePeriodLeft[emplID] / 60);
                    else
                        resultRow.Add(0);
                    if (emplSWPeriodUsed.ContainsKey(emplID))
                        resultRow.Add(emplSWPeriodUsed[emplID] / 60);
                    else
                        resultRow.Add(0);
                    if (emplSWAfterPeriodLeft.ContainsKey(emplID))
                        resultRow.Add(emplSWAfterPeriodLeft[emplID] / 60);
                    else
                        resultRow.Add(0);
                    if (emplPLBeforePeriodLeft.ContainsKey(emplID))
                        resultRow.Add(emplPLBeforePeriodLeft[emplID]);
                    else
                        resultRow.Add(0);
                    if (emplPLPeriodUsed.ContainsKey(emplID))
                        resultRow.Add(emplPLPeriodUsed[emplID]);
                    else
                        resultRow.Add(0);
                    if (emplPLAfterPeriodLeft.ContainsKey(emplID))
                        resultRow.Add(emplPLAfterPeriodLeft[emplID]);
                    else
                        resultRow.Add(0);
                    if (emplONBeforePeriodLeft.ContainsKey(emplID))
                        resultRow.Add(emplONBeforePeriodLeft[emplID] / 60);
                    else
                        resultRow.Add(0);
                    if (emplONPeriodUsed.ContainsKey(emplID))
                        resultRow.Add(emplONPeriodUsed[emplID] / 60);
                    else
                        resultRow.Add(0);
                    if (emplONAfterPeriodLeft.ContainsKey(emplID))
                        resultRow.Add(emplONAfterPeriodLeft[emplID] / 60);
                    else
                        resultRow.Add(0);

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

        private void createResultTable(double totalEmplBHBeforePeriodLeft, double totalEmplBHPeriodUsed, double totalEmplBHAfterPeriodLeft, int totalEmplALPrevBeforePeriodLeft,
            int totalEmplALPrevPeriodUsed, int totalEmplALPrevAfterPeriodLeft, int totalEmplALCurrBeforePeriodLeft, int totalEmplALCurrPeriodUsed, int totalEmplALCurrAfterPeriodLeft,
            int totalEmplALBeforePeriodLeft, int totalEmplALPeriodUsed, int totalEmplALAfterPeriodLeft, double totalEmplSWBeforePeriodLeft, double totalEmplSWPeriodUsed,
            double totalEmplSWAfterPeriodLeft, int totalEmplPLBeforePeriodLeft, int totalEmplPLPeriodUsed, int totalEmplPLAfterPeriodLeft, double totalEmplONBeforePeriodLeft, 
            double totalEmplONPeriodUsed, double totalEmplONAfterPeriodLeft)
        {
            try
            {
                List<List<object>> resultTable = new List<List<object>>();

                // create list of columns for list preview of pairs for inserting
                List<DataColumn> resultColumns = new List<DataColumn>();
                resultColumns.Add(new DataColumn("bhBefore", typeof(double)));
                resultColumns.Add(new DataColumn("bhPeriod", typeof(double)));
                resultColumns.Add(new DataColumn("bhAfter", typeof(double)));
                resultColumns.Add(new DataColumn("alPrevLeftBefore", typeof(int)));
                resultColumns.Add(new DataColumn("alCurrLeftBefore", typeof(int)));
                resultColumns.Add(new DataColumn("alLeftBefore", typeof(int)));
                resultColumns.Add(new DataColumn("alPrevUsedPeriod", typeof(int)));
                resultColumns.Add(new DataColumn("alCurrUsedPeriod", typeof(int)));
                resultColumns.Add(new DataColumn("alUsedPeriod", typeof(int)));
                resultColumns.Add(new DataColumn("alPrevLeftAfter", typeof(int)));
                resultColumns.Add(new DataColumn("alCurrLeftAfter", typeof(int)));
                resultColumns.Add(new DataColumn("alLeftAfter", typeof(int)));
                resultColumns.Add(new DataColumn("swBefore", typeof(double)));
                resultColumns.Add(new DataColumn("swPeriod", typeof(double)));
                resultColumns.Add(new DataColumn("swAfter", typeof(double)));
                resultColumns.Add(new DataColumn("plBefore", typeof(int)));
                resultColumns.Add(new DataColumn("plUsed", typeof(int)));
                resultColumns.Add(new DataColumn("plAfter", typeof(int)));
                resultColumns.Add(new DataColumn("oKeepBefore", typeof(double)));
                resultColumns.Add(new DataColumn("oKeepPeriod", typeof(double)));
                resultColumns.Add(new DataColumn("oKeepAfter", typeof(double)));

                // create result row
                List<object> resultRow = new List<object>();
                resultRow.Add(totalEmplBHBeforePeriodLeft / 60);
                resultRow.Add(totalEmplBHPeriodUsed / 60);
                resultRow.Add(totalEmplBHAfterPeriodLeft / 60);
                resultRow.Add(totalEmplALPrevBeforePeriodLeft);
                resultRow.Add(totalEmplALCurrBeforePeriodLeft);
                resultRow.Add(totalEmplALBeforePeriodLeft);
                resultRow.Add(totalEmplALPrevPeriodUsed);
                resultRow.Add(totalEmplALCurrPeriodUsed);
                resultRow.Add(totalEmplALPeriodUsed);
                resultRow.Add(totalEmplALPrevAfterPeriodLeft);
                resultRow.Add(totalEmplALCurrAfterPeriodLeft);
                resultRow.Add(totalEmplALAfterPeriodLeft);
                resultRow.Add(totalEmplSWBeforePeriodLeft / 60);
                resultRow.Add(totalEmplSWPeriodUsed / 60);
                resultRow.Add(totalEmplSWAfterPeriodLeft / 60);
                resultRow.Add(totalEmplPLBeforePeriodLeft);
                resultRow.Add(totalEmplPLPeriodUsed);
                resultRow.Add(totalEmplPLAfterPeriodLeft);
                resultRow.Add(totalEmplONBeforePeriodLeft / 60);
                resultRow.Add(totalEmplONPeriodUsed / 60);
                resultRow.Add(totalEmplONAfterPeriodLeft / 60);

                resultTable.Add(resultRow);

                Session[Constants.sessionDataTableColumns] = resultColumns;
                Session[Constants.sessionDataTableList] = resultTable;
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

        private void SaveState()
        {
            try
            {
                Dictionary<string, string> filterState = new Dictionary<string, string>();

                if (Session[Constants.sessionFilterState] != null && Session[Constants.sessionFilterState] is Dictionary<string, string>)
                    filterState = (Dictionary<string, string>)Session[Constants.sessionFilterState];

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "BufferReport.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "BufferReport.", filterState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
