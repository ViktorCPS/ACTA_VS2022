using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Resources;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using System.Configuration;

using Common;
using Util;
using TransferObjects;

namespace ACTAWebUI
{
    public partial class TLReports : System.Web.UI.Page
    {
        const string pageName = "TLReportsPage";

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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // if user do not want to reload selected filter and get results for that filter, parameter reloadState should be passes through url and set to false
                if (!IsPostBack)
                {
                    cbSelectAllEmpolyees.Attributes.Add("onclick", "return selectListItems('cbSelectAllEmpolyees', 'lboxEmployees');");
                    tbEmployee.Attributes.Add("onKeyUp", "return selectItem('tbEmployee', 'lboxEmployees');");
                    btnShow.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnReport.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    cbSelectAllEmpolyees.Visible = false;

                    setLanguage();

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

                    foreach (KeyValuePair<int, string> pair in getMonths())
                    {
                        lbMonths.Items.Add(pair.Value);
                    }
                    if (Request.QueryString["reloadState"] != null && Request.QueryString["reloadState"].Trim().ToUpper().Equals(Constants.falseValue.Trim().ToUpper()))
                    {
                        ClearSessionValues();
                        DateTime current = DateTime.Now;
                        lbMonths.SelectedIndex = current.Month - 1;
                        tbYear.Text = current.Year + ".";

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
                    else // reload selected filter state
                    {
                        LoadState();

                        populateEmployees(Menu1.SelectedItem.Value.Equals("0"));

                        setLabel();
                        // do again load state to select previously selected employees in employees list
                        LoadState();
                    }
                    btnShow_Click(this, new EventArgs());
                    if (rbCounter.Checked)
                    {
                        lbMonths.Enabled = tbYear.Enabled = false;
                    }
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLReportsPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLReportsPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private Dictionary<int, PassTypeTO> getTypes(int company, bool isAltLang)
        {
            try
            {
                Dictionary<int, PassTypeTO> passTypes = new Dictionary<int, PassTypeTO>();
                passTypes = new PassType(Session[Constants.sessionConnection]).SearchForCompanyDictionary(company, isAltLang);
                return passTypes;
            }
            catch (Exception ex)
            {
                throw ex;
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
                cbSelectAllEmpolyees.Checked = false;

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLReportsPage.lBoxWU_SelectedIndexChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLReportsPage.aspx&Header=" + Constants.falseValue.Trim(), false);
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
                cbSelectAllEmpolyees.Checked = false;

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLReportsPage.lBoxOU_SelectedIndexChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLReportsPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void rb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RadioButton rb = sender as RadioButton;
                if (rb == rbSummary)
                {
                    rbAnalitical.Checked = !rbSummary.Checked;
                    rbCounter.Checked = !rbSummary.Checked;
                }
                else if (rb == rbAnalitical)
                {
                    rbSummary.Checked = !rbAnalitical.Checked;
                    rbCounter.Checked = !rbAnalitical.Checked;
                }
                else if (rb == rbCounter)
                {
                    rbSummary.Checked = !rbCounter.Checked;
                    rbAnalitical.Checked = !rbCounter.Checked;
                }

                lbMonths.Enabled = tbYear.Enabled = !rbCounter.Checked;

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLReportsPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLReportsPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLReportsPage.Date_Changed(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLReportsPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void populateEmployees(bool isWU)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLReports).Assembly);

                lblError.Text = "";

                DateTime from = CommonWeb.Misc.createDate("01." + (lbMonths.SelectedIndex + 1) + "." + tbYear.Text.Trim());

                if (from.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidDate", culture);
                    return;
                }

                DateTime to = from.AddMonths(1).AddDays(-1).Date;

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
                lblError.Text = "";
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

                    lBoxOU.DataSource = OUList;
                    lBoxOU.DataTextField = "Name";
                    lBoxOU.DataValueField = "OrgUnitID";
                    lBoxOU.DataBind();

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

        private void InitializeDataSet(DataSet dataSet, DataTable tableSummary, DataTable tableAnalitic, DataTable tableCounters, Dictionary<int, PassTypeTO> passTypes)
        {
            try
            {
                if (tableSummary.Columns.Count == 0)
                {
                    tableSummary.Columns.Add("wUnit", typeof(System.String));
                    tableSummary.Columns.Add("employee_id", typeof(System.Int32));
                    tableSummary.Columns.Add("first_name", typeof(System.String));


                    tableAnalitic.Columns.Add("day", typeof(System.Int32));
                    tableAnalitic.Columns.Add("employee_id", typeof(System.Int32));
                    tableAnalitic.Columns.Add("first_name", typeof(System.String));


                    tableCounters.Columns.Add("employee_id", typeof(System.Int32));
                    tableCounters.Columns.Add("first_name", typeof(System.String));

                    tableCounters.Columns.Add("thisYearLeave", typeof(System.Double));
                    tableCounters.Columns.Add("prevYearLeave", typeof(System.Double));
                    tableCounters.Columns.Add("annualLeaveUsed", typeof(System.Double));
                    tableCounters.Columns.Add("paidLeaveCounter", typeof(System.Double));
                    tableCounters.Columns.Add("bankHoursCounter", typeof(System.String));
                    tableCounters.Columns.Add("overtimeCounter", typeof(System.String));
                    tableCounters.Columns.Add("stopWorkingCounter", typeof(System.String));
                    tableCounters.Columns.Add("branch", typeof(System.String));
                    tableCounters.Columns.Add("empl_type", typeof(System.String));

                    List<string> lista = new List<string>();
                    int listNum = 1;

                    foreach (KeyValuePair<int, PassTypeTO> pair in passTypes)
                    {
                        PassTypeTO pt = pair.Value;

                        if (!lista.Contains(pt.PaymentCode))
                        {
                            lista.Add(pt.PaymentCode);
                            tableSummary.Columns.Add("Col" + listNum, typeof(System.Double));
                            tableAnalitic.Columns.Add("Col" + listNum, typeof(System.Double));
                            listNum++;

                        }
                    }
                    tableSummary.Columns.Add("branch", typeof(System.String));
                    tableSummary.Columns.Add("empl_type", typeof(System.String));
                    tableAnalitic.Columns.Add("branch", typeof(System.String));
                    tableAnalitic.Columns.Add("empl_type", typeof(System.String));

                    dataSet.Tables.Add(tableSummary);
                    dataSet.Tables.Add(tableAnalitic);
                    dataSet.Tables.Add(tableCounters);
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLReports).Assembly);

                lblEmployee.Text = rm.GetString("lblEmployeeSelection", culture);
                lblPeriod.Text = rm.GetString("lblMonth", culture);
                cbSelectAllEmpolyees.Text = rm.GetString("lblSelectAll", culture);
                cbSelectAllWU.Text = rm.GetString("lblSelectAll", culture);
                cbSelectAllOU.Text = rm.GetString("lblSelectAll", culture);
                btnShow.Text = rm.GetString("btnShow", culture);
                btnReport.Text = rm.GetString("btnReport", culture);
                chbSinglePage.Text = rm.GetString("lblSingleEmployeePerPage", culture);
                rbSummary.Text = rm.GetString("rbSummary", culture);
                rbAnalitical.Text = rm.GetString("rbAnalitical", culture);
                rbCounter.Text = rm.GetString("rbCounter", culture);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void setLabel()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLReports).Assembly);
                if (!rbCounter.Checked)
                {
                    DateTime current = CommonWeb.Misc.createDate("01." + (lbMonths.SelectedIndex + 1) + "." + tbYear.Text);

                    foreach (KeyValuePair<int, string> pair in getMonths())
                    {
                        if (pair.Key == current.Month)
                        {
                            lblGraphPeriod.Text = rm.GetString("lblDataForMonth", culture) + pair.Value + " " + current.Year;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
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

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLReportsPage.cbSelectAllWU_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLReportsPage.aspx".Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
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

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLReportsPage.cbSelectAllWU_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLReportsPage.aspx".Trim(), false);
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

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLReportsPage.Menu1_MenuItemClick(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLReportsPage.aspx&Header=" + Constants.falseValue.Trim(), false);
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
                Session[Constants.sessionFieldsFormating] = null;
                Session[Constants.sessionFieldsFormatedValues] = null;

                Session[Constants.sessionItemsColors] = null;
                Session[Constants.sessionCountersEmployees] = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int returnCompany()
        {
            try
            {
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
                return company;
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
                lblGraphPeriod.Text = "";
                Session[Constants.sessionCounters] = null;
                Session[Constants.sessionDataTableList] = null;
                Session[Constants.sessionDataTableColumns] = null;
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLReports).Assembly);

                SaveState();

                DataSet dataSet = new DataSet();

                DataTable tableSummary = new DataTable("io_pair_summary");
                DataTable tableAnalitic = new DataTable("io_pair_analitical");
                DataTable tableCounters = new DataTable("io_pair_counters");

                bool isAltLang = !CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper());
                int company = returnCompany();

                Dictionary<int, PassTypeTO> passtypes = getTypes(company, isAltLang);

                InitializeDataSet(dataSet, tableSummary, tableAnalitic, tableCounters, passtypes);

                string employeeIDs = "";
                foreach (ListItem item in lboxEmployees.Items)
                {
                    employeeIDs += item.Value.Trim() + ",";
                }

                if (employeeIDs.Length > 0)
                    employeeIDs = employeeIDs.Substring(0, employeeIDs.Length - 1);
                else
                {
                    lblError.Text = rm.GetString("noEmployeesForTMData", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }
                Dictionary<string, string> dict = new Dictionary<string, string>();
                Dictionary<string, string> dictName = new Dictionary<string, string>();
                DateTime current = CommonWeb.Misc.createDate("01." + (lbMonths.SelectedIndex + 1) + "." + tbYear.Text);

                if (current == new DateTime())
                {
                    lblError.Text = rm.GetString("invalidDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

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

                Dictionary<int, string> dictPayment = returnPaymentCode(passtypes);
                string filter = "";
                string fields = "";
                if (rbSummary.Checked)
                {

                    FillSummary(company, dict, employeeIDs, tableSummary, dictPayment);
                    foreach (KeyValuePair<int, string> pair in getMonths())
                    {
                        if (pair.Key == current.Month)
                        {
                            lblGraphPeriod.Text = rm.GetString("lblDataForMonth", culture) + pair.Value + " " + current.Year;
                        }
                    }

                    List<DataColumn> resultColumns = new List<DataColumn>();

                    for (int i = 0; i <= dict.Count + 2; i++)
                    {
                        resultColumns.Add(tableSummary.Columns[i]);
                        foreach (KeyValuePair<string, string> pair in dict)
                        {
                            if (pair.Value == tableSummary.Columns[i].ColumnName)
                            {
                                filter += pair.Key + ",";
                                break;
                            }
                        }
                    }
                    resultColumns.Add(tableSummary.Columns["branch"]);
                    resultColumns.Add(tableSummary.Columns["empl_type"]);
                    if (filter.Length > 0)
                        filter = filter.Remove(filter.LastIndexOf(','));

                    for (int i = 1; i <= dict.Count; i++)
                    {
                        fields += "Col" + i + ",";
                    }
                    if (fields.Length > 0)
                        fields = fields.Remove(fields.LastIndexOf(','));

                    InitializeSQLParameters(filter, fields);

                    List<List<object>> resultTable = new List<List<object>>();
                    foreach (DataRow row in tableSummary.Rows)
                    {
                        List<object> rowResult = new List<object>();
                        for (int i = 0; i <= dict.Count + 2; i++)
                        {
                            rowResult.Add(row.ItemArray[i]);
                        }
                        rowResult.Add(row.ItemArray[row.ItemArray.Length - 2]);
                        rowResult.Add(row.ItemArray[row.ItemArray.Length - 1]);
                        resultTable.Add(rowResult);

                    }
                    Session[Constants.sessionDataTableList] = resultTable;
                    Session[Constants.sessionDataTableColumns] = resultColumns;

                    resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?showSelection=false";
                }
                else if (rbCounter.Checked)
                {
                    FillCountersTable(company, dict, employeeIDs, tableCounters, passtypes);
                    lblGraphPeriod.Text = "";
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

                    SaveState();
                    Session[Constants.sessionCountersEmployees] = emplIDs;
                    resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/CountersPage.aspx";
                }
                else
                {
                    foreach (KeyValuePair<int, string> pair in getMonths())
                    {
                        if (pair.Key == current.Month)
                        {
                            lblGraphPeriod.Text = rm.GetString("lblDataForMonth", culture) + pair.Value + " " + current.Year;
                        }
                    }

                    FillAnaliticalTable(company, dict, employeeIDs, tableAnalitic, tableSummary, dictPayment);

                    List<DataColumn> resultColumns = new List<DataColumn>();
                    resultColumns.Add(new DataColumn("day", typeof(string)));
                    resultColumns.Add(new DataColumn("employee_id", typeof(string)));
                    for (int i = 2; i <= dict.Count + 2; i++)
                    {
                        resultColumns.Add(tableAnalitic.Columns[i]);
                        foreach (KeyValuePair<string, string> pair in dict)
                        {
                            if (pair.Value == tableAnalitic.Columns[i].ColumnName)
                            {
                                filter += pair.Key + ",";
                                break;
                            }
                        }
                    }
                    resultColumns.Add(tableSummary.Columns["branch"]);
                    resultColumns.Add(tableSummary.Columns["empl_type"]);
                    if (filter.Length > 0)
                        filter = filter.Remove(filter.LastIndexOf(','));

                    for (int i = 1; i <= dict.Count; i++)
                    {
                        fields += "Col" + i + ",";
                    }
                    if (fields.Length > 0)
                        fields = fields.Remove(fields.LastIndexOf(','));

                    InitializeSQLParametersAnalitycal(filter, fields);

                    List<List<object>> resultTable = new List<List<object>>();
                    for (int indexDay = 1; indexDay <= FindNumOfDaysInMonth(current); indexDay++)
                    {
                        foreach (DataRow row in tableAnalitic.Rows)
                        {
                            if ((int)row.ItemArray[0] == indexDay)
                            {
                                List<object> rowResult = new List<object>();
                                rowResult.Add(row.ItemArray[0].ToString());
                                rowResult.Add(row.ItemArray[1].ToString());
                                for (int i = 2; i <= dict.Count + 2; i++)
                                {
                                    rowResult.Add(row.ItemArray[i]);
                                }
                                rowResult.Add(row.ItemArray[row.ItemArray.Length - 2]);
                                rowResult.Add(row.ItemArray[row.ItemArray.Length - 1]);
                                resultTable.Add(rowResult);

                            }
                        }
                    }
                    Session[Constants.sessionDataTableList] = resultTable;
                    Session[Constants.sessionDataTableColumns] = resultColumns;
                    resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?showSelection=false";
                }

                int num = dictName.Count + 1;
                foreach (KeyValuePair<string, string> pair in dict)
                {
                    foreach (KeyValuePair<int, PassTypeTO> pairPass in passtypes)
                    {
                        if (pair.Key == pairPass.Value.PaymentCode)
                        {
                            if (!dictName.ContainsKey(pairPass.Value.PaymentCode))
                            {
                                string temp = "";
                                if (!isAltLang)
                                {
                                    if (pairPass.Value.Description.Length <= 15)
                                        temp = pairPass.Value.Description;
                                    else
                                        temp = pairPass.Value.Description.Remove(15);
                                    dictName.Add(pairPass.Value.PaymentCode, temp);
                                }
                                else
                                {
                                    if (pairPass.Value.DescAlt.Length <= 20)
                                        temp = pairPass.Value.DescAlt;
                                    else
                                        temp = pairPass.Value.DescAlt.Remove(20);
                                    dictName.Add(pairPass.Value.PaymentCode, temp);
                                }
                            }
                        }
                    }
                }
                Dictionary<string, string> dictNameCol = new Dictionary<string, string>();
                foreach (KeyValuePair<string, string> pair in dictName)
                {
                    dictNameCol.Add(pair.Key, pair.Value);
                    num++;

                }
                Session["TLReportsPage.dataSet"] = dataSet;
                Session["TLReportsPage.dict"] = dict;
                Session["TLReportsPage.dictName"] = dictNameCol;

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLReportsPage.btnShow_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLReportsPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void FillAnaliticalTable(int company, Dictionary<string, string> dict, string employee, DataTable tableAnalitic, DataTable tableSummary, Dictionary<int, string> passtypes)
        {
            try
            {
                tableAnalitic.Clear();
                tableSummary.Clear();

                //take month from form...
                DateTime month = CommonWeb.Misc.createDate("01." + (lbMonths.SelectedIndex + 1) + "." + tbYear.Text);
                int noOfDaysInMonth = FindNumOfDaysInMonth(month);
                string employees = "";
                List<IOPairProcessedTO> IOPairList = new List<IOPairProcessedTO>();
                if (lboxEmployees.SelectedIndex >= 0)
                {
                    for (int intEmpolyees = 0; intEmpolyees < lboxEmployees.Items.Count; intEmpolyees++)
                    {
                        if (lboxEmployees.Items[intEmpolyees].Selected)
                        {
                            employees += lboxEmployees.Items[intEmpolyees].Value + ",";
                        }

                    }
                    if (employees.Length > 0)
                        employees = employees.Remove(employees.LastIndexOf(','));
                }
                else
                    employees = employee;

                IOPairList = FindIOPairsForSelEmployee(month, employees);
                Dictionary<int, List<IOPairProcessedTO>> dictionary = fillIOEmplDictionary(IOPairList);
                foreach (KeyValuePair<int, List<IOPairProcessedTO>> pair in dictionary)
                {
                    IOPairList = pair.Value;

                    EmployeeTO emp = new Employee(Session[Constants.sessionConnection]).Find(pair.Key.ToString());
                    DataRow row = tableSummary.NewRow();
                    WorkingUnit wu = new WorkingUnit(Session[Constants.sessionConnection]);
                    WorkingUnitTO wUnit = wu.FindWU(emp.WorkingUnitID);
                    row["wUnit"] = wUnit.WorkingUnitID;
                    row["employee_id"] = emp.EmployeeID;
                    row["first_name"] = emp.FirstAndLastName;
                    EmployeeAsco4 emplAsco = new EmployeeAsco4(Session[Constants.sessionConnection]);
                    emplAsco.EmplAsco4TO.EmployeeID = emp.EmployeeID;
                    List<EmployeeAsco4TO> emplAscoList = emplAsco.Search();

                    if (emplAscoList.Count == 1)
                    {
                        row["branch"] = emplAscoList[0].NVarcharValue6;
                    }
                    row["empl_type"] = emplTypes(emp, company);

                    TotalHourCalc(row, IOPairList, dict, passtypes);
                    tableSummary.Rows.Add(row);
                    tableSummary.AcceptChanges();
                    for (int i = 1; i <= noOfDaysInMonth; i++)
                    {
                        DataRow rowAnalitical = tableAnalitic.NewRow();
                        rowAnalitical["day"] = i;
                        rowAnalitical["employee_id"] = emp.EmployeeID;
                        rowAnalitical["first_name"] = emp.FirstAndLastName;
                        rowAnalitical["branch"] = row["branch"];

                        rowAnalitical["empl_type"] = row["empl_type"];
                        AnaliticalHourCalc(rowAnalitical, i, IOPairList, dict, passtypes);

                        tableAnalitic.Rows.Add(rowAnalitical);
                        tableAnalitic.AcceptChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FillCountersTable(int company, Dictionary<string, string> dict, string employee, DataTable tableCounters, Dictionary<int, PassTypeTO> passTypes)
        {
            try
            {
                tableCounters.Clear();
                string employees = "";
                List<IOPairProcessedTO> IOPairList = new List<IOPairProcessedTO>();
                if (lboxEmployees.SelectedIndex >= 0)
                {

                    for (int intEmpolyees = 0; intEmpolyees < lboxEmployees.Items.Count; intEmpolyees++)
                    {
                        if (lboxEmployees.Items[intEmpolyees].Selected)
                        {
                            employees += lboxEmployees.Items[intEmpolyees].Value + ",";
                        }
                    }
                    if (employees.Length > 0)
                        employees = employees.Remove(employees.LastIndexOf(','));

                }
                else
                    employees = employee;

                List<EmployeeTO> empolyeeList = new Employee(Session[Constants.sessionConnection]).Search(employees);
                foreach (EmployeeTO emp in empolyeeList)
                {
                    EmployeeCounterValue emplCounterValue = new EmployeeCounterValue(Session[Constants.sessionConnection]);
                    emplCounterValue.ValueTO.EmplID = emp.EmployeeID;
                    List<EmployeeCounterValueTO> counterValues = emplCounterValue.Search();
                    if (counterValues.Count > 0)
                    {
                        DataRow rowCounters = tableCounters.NewRow();
                        rowCounters["employee_id"] = emp.EmployeeID;
                        rowCounters["first_name"] = emp.FirstAndLastName;
                        EmployeeAsco4 emplAsco4 = new EmployeeAsco4(Session[Constants.sessionConnection]);
                        emplAsco4.EmplAsco4TO.EmployeeID = emp.EmployeeID;
                        List<EmployeeAsco4TO> emplAscoList4 = emplAsco4.Search();

                        if (emplAscoList4.Count == 1)
                        {
                            rowCounters["branch"] = emplAscoList4[0].NVarcharValue6;
                        }
                        rowCounters["empl_type"] = emplTypes(emp, company);
                        FillCounters(rowCounters, counterValues);
                        tableCounters.Rows.Add(rowCounters);
                        tableCounters.AcceptChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FillSummary(int company, Dictionary<string, string> dict, string employee, DataTable tableSummary, Dictionary<int, string> passtypes)
        {
            try
            {
                DateTime month = CommonWeb.Misc.createDate("01." + (lbMonths.SelectedIndex + 1) + "." + tbYear.Text);
                int noOfDaysInMonth = FindNumOfDaysInMonth(month);
                string employees = "";
                List<IOPairProcessedTO> IOPairList = new List<IOPairProcessedTO>();
                if (lboxEmployees.SelectedIndex >= 0)
                {

                    for (int intEmpolyees = 0; intEmpolyees < lboxEmployees.Items.Count; intEmpolyees++)
                    {
                        if (lboxEmployees.Items[intEmpolyees].Selected)
                        {
                            employees += lboxEmployees.Items[intEmpolyees].Value + ",";
                        }

                    }
                    if (employees.Length > 0)
                        employees = employees.Remove(employees.LastIndexOf(','));

                }
                else
                    employees = employee;
                IOPairList = FindIOPairsForSelEmployee(month, employees);
                Dictionary<int, List<IOPairProcessedTO>> dictionary = fillIOEmplDictionary(IOPairList);
                foreach (KeyValuePair<int, List<IOPairProcessedTO>> pair in dictionary)
                {
                    IOPairList = pair.Value;
                    EmployeeTO emp = new Employee(Session[Constants.sessionConnection]).Find(pair.Key.ToString());
                    DataRow row = tableSummary.NewRow();
                    WorkingUnit wu = new WorkingUnit(Session[Constants.sessionConnection]);
                    WorkingUnitTO wUnit = wu.FindWU(emp.WorkingUnitID);
                    row["wUnit"] = wUnit.WorkingUnitID;
                    row["employee_id"] = emp.EmployeeID;
                    row["first_name"] = emp.FirstAndLastName;
                    EmployeeAsco4 emplAsco = new EmployeeAsco4(Session[Constants.sessionConnection]);
                    emplAsco.EmplAsco4TO.EmployeeID = emp.EmployeeID;
                    List<EmployeeAsco4TO> emplAscoList = emplAsco.Search();

                    if (emplAscoList.Count == 1)
                    {
                        row["branch"] = emplAscoList[0].NVarcharValue6;
                    }
                    row["empl_type"] = emplTypes(emp, company);

                    TotalHourCalc(row, IOPairList, dict, passtypes);
                    tableSummary.Rows.Add(row);
                    tableSummary.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Dictionary<int, List<IOPairProcessedTO>> fillIOEmplDictionary(List<IOPairProcessedTO> IOPairList)
        {
            Dictionary<int, List<IOPairProcessedTO>> dicionary = new Dictionary<int, List<IOPairProcessedTO>>();
            foreach (IOPairProcessedTO pair in IOPairList)
            {
                if (!dicionary.ContainsKey(pair.EmployeeID))
                {
                    List<IOPairProcessedTO> listPair = new List<IOPairProcessedTO>();
                    listPair.Add(pair);
                    dicionary.Add(pair.EmployeeID, listPair);
                }
                else
                {
                    dicionary[pair.EmployeeID].Add(pair);
                }
            }
            return dicionary;
        }

        private void InitializeSQLParameters(string filter, string fields)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(HRSSCOutstandingDataPage).Assembly);
                string header = rm.GetString("hdrWorkingUnit", culture) + "," + rm.GetString("hdrEmployeeID", culture) + "," + rm.GetString("hdrEmployee", culture) + "," + rm.GetString("hdrBranch", culture) + "," + rm.GetString("hdrEmplType", culture) + ","
                    + filter;
                Session[Constants.sessionHeader] = header;
                string fields1 = "wUnit,employee_id,first_name,branch,empl_type," + fields;
                Session[Constants.sessionFields] = fields1;

                Session[Constants.sessionFieldsFormating] = null;
                Session[Constants.sessionFieldsFormatedValues] = null;

                Session[Constants.sessionKey] = null;
                Session[Constants.sessionColTypes] = null;

                Session[Constants.sessionDataTableList] = null;
                Session[Constants.sessionDataTableColumns] = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeSQLParametersAnalitycal(string filter, string fields)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLReports).Assembly);
                Session[Constants.sessionHeader] = rm.GetString("hdrCycleDay", culture) + "," + rm.GetString("hdrEmployeeID", culture) + "," + rm.GetString("hdrEmployee", culture) + "," + rm.GetString("hdrBranch", culture) + "," + rm.GetString("hdrEmplType", culture) + "," + filter;

                Session[Constants.sessionFields] = "day,employee_id,first_name,branch,empl_type," + fields;

                Session[Constants.sessionFieldsFormating] = null;
                Session[Constants.sessionFieldsFormatedValues] = null;
                Session[Constants.sessionKey] = null;
                Session[Constants.sessionColTypes] = null;
                Session[Constants.sessionDataTableList] = null;
                Session[Constants.sessionDataTableColumns] = null;
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
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLReports).Assembly);

                lblError.Text = "";

                SaveState();
                string reportType = "0";

                if (rbSummary.Checked)
                    reportType = "0";
                else if (rbAnalitical.Checked)
                    reportType = "1";
                else if (rbCounter.Checked)
                    reportType = "2";

                string singlePage = "0";

                if (chbSinglePage.Checked)
                    singlePage = "1";
                else
                    singlePage = "0";

                DataSet dataset = (DataSet)Session["TLReportsPage.dataSet"];
                //if (rbSummary.Checked && dataset.Tables[0].Rows.Count > 2015)
                //{
                //    lblError.Text = rm.GetString("moreRecordsThanAllowed", culture);
                //    return;
                //}
                //else if (rbAnalitical.Checked && dataset.Tables[1].Rows.Count > 2015)
                //{
                //    lblError.Text = rm.GetString("moreRecordsThanAllowed", culture);
                //return;
                //}
                //else if (rbCounter.Checked && dataset.Tables[2].Rows.Count > 2015)
                //{
                //    lblError.Text = rm.GetString("moreRecordsThanAllowed", culture);
                //    return;
                //}

                if (rbCounter.Checked && Session[Constants.sessionCounters] == null)
                {
                    lblError.Text = rm.GetString("noReportData", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }
                List<EmployeeTO> empolyeeList = new List<EmployeeTO>();

                //int onlyEmplTypeID = -1;
                //int exceptEmplTypeID = -1;
                int emplID = -1;

                // 09.01.2012. Sanja - do not exclude login employee from reports
                //if (Session[Constants.sessionLogInEmployee] != null && Session[Constants.sessionLogInEmployee] is EmployeeTO)
                //    emplID = ((EmployeeTO)Session[Constants.sessionLogInEmployee]).EmployeeID;

                Dictionary<int, List<int>> companyVisibleTypes = new Dictionary<int, List<int>>();
                List<int> typesVisible = new List<int>();

                if (Session[Constants.sessionVisibleEmplTypes] != null && Session[Constants.sessionVisibleEmplTypes] is Dictionary<int, List<int>>)
                    companyVisibleTypes = (Dictionary<int, List<int>>)Session[Constants.sessionVisibleEmplTypes];

                Dictionary<int, WorkingUnitTO> wuDict = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                int company = -1;
                string wunit = "";
                int[] wuIndexes = lBoxWU.GetSelectedIndices();
                foreach (int index in wuIndexes)
                {
                    wunit += lBoxWU.Items[index].Value + ",";
                    int ID = -1;
                    if (int.TryParse(lBoxWU.Items[index].Value, out ID))
                    {
                        company = Common.Misc.getRootWorkingUnit(ID, wuDict);

                        if (companyVisibleTypes.ContainsKey(company))
                            typesVisible.AddRange(companyVisibleTypes[company]);
                    }
                }

                if (wunit.Length > 0)
                    wunit = wunit.Substring(0, wunit.Length - 1);

                if (!wunit.Trim().Equals(""))
                {
                    DateTime from = CommonWeb.Misc.createDate("01." + (lbMonths.SelectedIndex + 1) + "." + tbYear.Text.Trim());

                    if (from.Equals(new DateTime()))
                    {
                        lblError.Text = rm.GetString("invalidDate", culture);
                        writeLog(DateTime.Now, false);
                        return;
                    }

                    DateTime to = from.AddMonths(1).AddDays(-1).Date;

                    empolyeeList = new Employee(Session[Constants.sessionConnection]).SearchByWULoans(Common.Misc.getWorkingUnitHierarhicly(wunit, (List<int>)Session[Constants.sessionLoginCategoryWUnits], Session[Constants.sessionConnection]), emplID, typesVisible, from, to);
                }

                Dictionary<string, string> dict = new Dictionary<string, string>();

                if (dataset.Tables[0].Rows.Count == 0 && rbSummary.Checked)
                {
                    lblError.Text = rm.GetString("noReportData", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }
                if (dataset.Tables[1].Rows.Count == 0 && dataset.Tables[0].Rows.Count == 0 && rbAnalitical.Checked)
                {
                    lblError.Text = rm.GetString("noReportData", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }
                DateTime date = CommonWeb.Misc.createDate("01." + (lbMonths.SelectedIndex + 1) + "." + tbYear.Text);
                string monthYear = "";
                string reportURL = "";
                Dictionary<int, string> Months = getMonths();
                if (Months.ContainsKey(date.Month))
                    monthYear = Months[date.Month] + " " + date.Year.ToString().Trim() + ".";

                string costumer = Common.Misc.getCustomer(Session[Constants.sessionConnection]);
                int cost = 0;
                bool costum = int.TryParse(costumer, out cost);
                bool isFiat = (cost == (int)Constants.Customers.FIAT);
                string fas = "";

                Session["TLReportsPage.reportType"] = reportType;
                Session["TLReportsPage.date"] = monthYear;
                Session["TLReportsPage.singlePage"] = singlePage;

                if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                {
                    reportURL = "/ACTAWeb/ReportsWeb/sr/TLReportsReport_sr.aspx";
                    if (isFiat)
                        fas = " u FAS-u (tabela za kontrolu vremena)";
                    else
                        fas = " (tabela za kontrolu vremena)";
                }
                else
                {
                    reportURL = "/ACTAWeb/ReportsWeb/en/TLReportsReport_en.aspx";
                    if (isFiat)
                        fas = "in FAS (time control table)";
                    else
                        fas = "(time control table)";
                }
                Session["TLReportsPage.fas"] = fas;
                Response.Redirect("/ACTAWeb/ReportsWeb/ReportPage.aspx?Back=/ACTAWeb/ACTAWebUI/TLReportsPage.aspx&Report=" + reportURL.Trim(), false);
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLReportsPage.btnReport_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLReportsPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private Dictionary<int, string> getMonths()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLReports).Assembly);

                Dictionary<int, string> months = new Dictionary<int, string>();

                if (!months.ContainsKey(1))
                    months.Add(1, rm.GetString("Jan", culture));
                if (!months.ContainsKey(2))
                    months.Add(2, rm.GetString("Feb", culture));
                if (!months.ContainsKey(3))
                    months.Add(3, rm.GetString("Mar", culture));
                if (!months.ContainsKey(4))
                    months.Add(4, rm.GetString("Apr", culture));
                if (!months.ContainsKey(5))
                    months.Add(5, rm.GetString("May", culture));
                if (!months.ContainsKey(6))
                    months.Add(6, rm.GetString("Jun", culture));
                if (!months.ContainsKey(7))
                    months.Add(7, rm.GetString("Jul", culture));
                if (!months.ContainsKey(8))
                    months.Add(8, rm.GetString("Aug", culture));
                if (!months.ContainsKey(9))
                    months.Add(9, rm.GetString("Sep", culture));
                if (!months.ContainsKey(10))
                    months.Add(10, rm.GetString("Oct", culture));
                if (!months.ContainsKey(11))
                    months.Add(11, rm.GetString("Nov", culture));
                if (!months.ContainsKey(12))
                    months.Add(12, rm.GetString("Dec", culture));

                return months;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FillCounters(DataRow rowCounters, List<EmployeeCounterValueTO> counterValues)
        {
            try
            {
                int prevYearLeave = -1;
                int thisYearLeave = -1;
                int annualLeaveUsed = -1;
                string paidLeaveCounter = "";
                string bankHoursCounter = "";
                string overtimeCounter = "";
                string StopWorkingCounter = "";

                foreach (EmployeeCounterValueTO counterVal in counterValues)
                {
                    switch (counterVal.EmplCounterTypeID)
                    {
                        case (int)Constants.EmplCounterTypes.AnnualLeaveCounter:
                            thisYearLeave = counterVal.Value;
                            break;
                        case (int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter:
                            prevYearLeave = counterVal.Value;
                            break;
                        case (int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter:
                            annualLeaveUsed = counterVal.Value;
                            break;
                        case (int)Constants.EmplCounterTypes.PaidLeaveCounter:
                            paidLeaveCounter = counterVal.Value.ToString().Trim();
                            break;
                        case (int)Constants.EmplCounterTypes.BankHoursCounter:
                            bankHoursCounter = CommonWeb.Misc.createHoursFromMinutes(counterVal.Value);
                            break;
                        case (int)Constants.EmplCounterTypes.OvertimeCounter:
                            overtimeCounter = CommonWeb.Misc.createHoursFromMinutes(counterVal.Value);
                            break;
                        case (int)Constants.EmplCounterTypes.StopWorkingCounter:
                            StopWorkingCounter = CommonWeb.Misc.createHoursFromMinutes(counterVal.Value);
                            break;
                    }
                }

                if (prevYearLeave >= annualLeaveUsed)
                    prevYearLeave -= annualLeaveUsed;
                else
                {
                    thisYearLeave -= (annualLeaveUsed - prevYearLeave);
                    prevYearLeave = 0;
                }

                rowCounters["thisYearLeave"] = thisYearLeave;
                rowCounters["prevYearLeave"] = prevYearLeave;
                rowCounters["annualLeaveUsed"] = (thisYearLeave + prevYearLeave);
                rowCounters["paidLeaveCounter"] = double.Parse(paidLeaveCounter);
                rowCounters["bankHoursCounter"] = bankHoursCounter;
                rowCounters["overtimeCounter"] = overtimeCounter;
                rowCounters["StopWorkingCounter"] = StopWorkingCounter;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string emplTypes(EmployeeTO employee, int company)
        {
            try
            {
                string empl = "";
                List<EmployeeTypeTO> listEmplTypes = new List<EmployeeTypeTO>();
                listEmplTypes = new EmployeeType(Session[Constants.sessionConnection]).Search();
                foreach (EmployeeTypeTO emplType in listEmplTypes)
                {
                    if (emplType.EmployeeTypeID == employee.EmployeeTypeID && emplType.WorkingUnitID == company)
                        empl = emplType.EmployeeTypeName;
                }

                return empl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int FindNumOfDaysInMonth(DateTime month)
        {
            try
            {
                List<DateTime> datesList = new List<DateTime>();
                DateTime nextMonth = month.AddMonths(1);
                int noOfDaysInMonth = 0;
                while (month < nextMonth)
                {
                    datesList.Add(month);
                    month = month.AddDays(1);
                    noOfDaysInMonth++;
                }

                return noOfDaysInMonth;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<IOPairProcessedTO> FindIOPairsForSelEmployee(DateTime month, string selectedEmployee)
        {
            try
            {
                List<IOPairProcessedTO> IOPairList = new List<IOPairProcessedTO>();
                List<DateTime> datesList = new List<DateTime>();
                DateTime nextMonth = month.AddMonths(1);
                int noOfDaysInMonth = 0;
                while (month < nextMonth)
                {
                    datesList.Add(month);
                    month = month.AddDays(1);
                    noOfDaysInMonth++;
                }

                IOPairList = new IOPairProcessed(Session[Constants.sessionConnection]).SearchAllPairsForEmpl(selectedEmployee, datesList, "");
                return IOPairList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void AnaliticalHourCalc(DataRow row, int day, List<IOPairProcessedTO> IOPairList, Dictionary<string, string> dict, Dictionary<int, string> passTypes)
        {
            try
            {
                //Dictionary<int, PassTypeTO> passTypes = getTypes();
                Dictionary<string, TimeSpan> typesCounter = new Dictionary<string, TimeSpan>();

                foreach (IOPairProcessedTO iopair in IOPairList)
                {

                    if (iopair.IOPairDate.Day == day)
                    {
                        TimeSpan duration = iopair.EndTime.TimeOfDay.Subtract(iopair.StartTime.TimeOfDay);
                        if (iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59)
                            duration = duration.Add(new TimeSpan(0, 1, 0)); // add one minute if it is last interval from third shift
                        if (typesCounter.ContainsKey(passTypes[iopair.PassTypeID]))
                            typesCounter[passTypes[iopair.PassTypeID]] = typesCounter[passTypes[iopair.PassTypeID]].Add(duration);
                        else
                            typesCounter.Add(passTypes[iopair.PassTypeID], duration);
                    }

                }
                int num = dict.Count + 1;
                foreach (string ptID in typesCounter.Keys)
                {
                    string value = "Col" + num;
                    if (!dict.ContainsKey(ptID))
                    {
                        dict.Add(ptID, "Col" + num);
                        num++;
                    }

                    value = dict[ptID];
                    int hours = typesCounter[ptID].Days * 24 + typesCounter[ptID].Hours;
                    decimal minutes = (decimal)typesCounter[ptID].Minutes / 60;
                    row[value] = Math.Round((double)hours + (double)minutes, 2);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private Dictionary<int, string> returnPaymentCode(Dictionary<int, PassTypeTO> listPass)
        {
            try
            {
                Dictionary<int, string> dictionary = new Dictionary<int, string>();

                foreach (KeyValuePair<int, PassTypeTO> pass in listPass)
                {
                    if (!dictionary.ContainsKey(pass.Key))
                        dictionary.Add(pass.Key, pass.Value.PaymentCode);
                }
                return dictionary;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TotalHourCalc(DataRow row, List<IOPairProcessedTO> IOPairList, Dictionary<string, string> dict, Dictionary<int, string> passTypes)
        {
            try
            {
                Dictionary<string, TimeSpan> typesCounter = new Dictionary<string, TimeSpan>();
                //Dictionary<int, PassTypeTO> passTypes = getTypes();
                foreach (IOPairProcessedTO iopair in IOPairList)
                {
                    if (!iopair.StartTime.Date.Equals(new DateTime()) && !iopair.EndTime.Date.Equals(new DateTime()))
                    {

                        TimeSpan duration = iopair.EndTime.TimeOfDay.Subtract(iopair.StartTime.TimeOfDay);
                        if (iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59)
                            duration = duration.Add(new TimeSpan(0, 1, 0)); // add one minute if it is last interval from third shift
                        if (typesCounter.ContainsKey(passTypes[iopair.PassTypeID]))
                            typesCounter[passTypes[iopair.PassTypeID]] = typesCounter[passTypes[iopair.PassTypeID]].Add(duration);
                        else
                            typesCounter.Add(passTypes[iopair.PassTypeID], duration);
                    }
                }
                int num = dict.Count + 1;
                foreach (string ptID in typesCounter.Keys)
                {
                    string value = "Col" + num;
                    if (!dict.ContainsKey(ptID))
                    {
                        dict.Add(ptID, "Col" + num);
                        num++;
                    }

                    value = dict[ptID];
                    int hours = typesCounter[ptID].Days * 24 + typesCounter[ptID].Hours;

                    decimal minutes = (decimal)typesCounter[ptID].Minutes / 60;
                    row[value] = Math.Round((double)hours + (double)minutes, 2);

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

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "TLReportsPage.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "TLReportsPage.", filterState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
