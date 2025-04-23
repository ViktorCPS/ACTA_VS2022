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
    public partial class TLTMDataPage : System.Web.UI.Page
    {
        const string pageName = "TLTMDataPage";
        
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

        DateTime toDate = DateTime.Now;
        protected void ShowCalendar(object sender, EventArgs e)
        {
            calendar.Visible = true;
            btn.Visible = false;
        }

        protected void DataChange(object sender, EventArgs e)
        {
            toDate = calendar.SelectedDate;
            tbDate.Text = toDate.ToString("dd.MM.yyyy.");
            calendar.Visible = false;
            btn.Visible = true;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {                
                // clear day pairs and employee counters so bars and counters could be presented properly
                ClearSessionValues();

                // if user do not want to reload selected filter and get results for that filter, parameter reloadState should be passes through url and set to false
                if (!IsPostBack)
                {
                    btnDate.Attributes.Add("onclick", "return calendarPicker('tbDate', 'true');");
                    btnWUTree.Attributes.Add("onclick", "return wUnitsPicker('btnWUTree', 'false');");
                    btnOrgTree.Attributes.Add("onclick", "return oUnitsPicker('btnOrgTree', 'false');");
                    tbEmployee.Attributes.Add("onKeyUp", "return selectItem('tbEmployee', 'lboxEmployees');");
                    btnShow.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnAssignTimeSchema.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnNext.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnPrev.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    rbHierarchically.Attributes.Add("onclick", "return checkRB('rbHierarchically', 'rbSelected', 'rbResponsiblePerson');");
                    rbSelected.Attributes.Add("onclick", "return checkRB('rbSelected', 'rbHierarchically', 'rbResponsiblePerson');");
                    rbResponsiblePerson.Attributes.Add("onclick", "return checkRB('rbResponsiblePerson', 'rbHierarchically', 'rbSelected');");                    

                    btnWUTree.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnWUTree.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnOrgTree.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnOrgTree.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnDate.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnDate.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnPrev.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnPrev.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnNext.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnNext.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");

                    btnExportReport.Visible = false;

                    if (Session[Constants.sessionFromDate] != null && Session[Constants.sessionFromDate] is DateTime)
                        tbDate.Text = ((DateTime)Session[Constants.sessionFromDate]).ToString(Constants.dateFormat.Trim());
                    else
                        tbDate.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());
                                        
                    tbSelectedDate.Text = tbDate.Text;

                    tbOffset.Text = Constants.defaultClockINOffset.ToString().Trim();

                    rbHierarchically.Checked = true;
                    rbSelected.Checked = rbResponsiblePerson.Checked = false;

                    if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                        && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.WCManager)
                        rbHierarchically.Visible = rbSelected.Visible = rbResponsiblePerson.Visible = true;
                    else
                        rbHierarchically.Visible = rbSelected.Visible = rbResponsiblePerson.Visible = false;

                    rbPassType.Checked = true;
                    rbLocation.Checked = false;

                    if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                        && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.WCManager)
                        rbPassType.Visible = rbLocation.Visible = false;
                    else
                    {
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
                    }

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

                        if (Session[Constants.sessionSelectedEmplIDs] != null && Session[Constants.sessionSelectedEmplIDs ] is List<string>
                            && (((List<string>)Session[Constants.sessionSelectedEmplIDs]).Count > 1  || (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                            && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRSSC)))
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
                        // do not do again load for employee selection becouse after next step, cYalling btnShow_Click, user will always be navigated to WC data page for selected employee
                        // load (select employee only if HRSSC is logged in, HRSSC does not call btnShow_Click)
                        if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                            && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRSSC)
                            LoadState();
                    }

                    if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO 
                        && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID != (int)Constants.Categories.HRSSC)
                        btnShow_Click(this, new EventArgs());
                }
                else
                {
                    lblPresent.Text = lblAbsent.Text = lblTotal.Text = "0";

                    if (Request["__EVENTARGUMENT"] != null && Request["__EVENTARGUMENT"].Equals(Constants.pairsSavedArg))
                    {
                        InitializeGraphData();
                        writeLog(DateTime.Now, false);
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
                                if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                                    && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID != (int)Constants.Categories.HRSSC)
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
                                if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                                    && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID != (int)Constants.Categories.HRSSC)
                                    btnShow_Click(this, new EventArgs());
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLTMDataPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLTMDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLTMDataPage).Assembly);

                lblError.Text = "";

                DateTime from = CommonWeb.Misc.createDate(tbDate.Text.Trim());

                if (from.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidDateFormat", culture);
                    return;
                }

                DateTime to = from.Date;

                if (chbTwoDaysView.Checked)
                    to = from.AddDays(1).Date;

                List<EmployeeTO> empolyeeList = new List<EmployeeTO>();
                
                int emplID = -1;

                if (Session[Constants.sessionLogInEmployee] != null && Session[Constants.sessionLogInEmployee] is EmployeeTO)
                    emplID = ((EmployeeTO)Session[Constants.sessionLogInEmployee]).EmployeeID;

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

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLTMDataPage).Assembly);

                lblEmployee.Text = rm.GetString("lblEmployeeSelection", culture);
                lblEmplType.Text = rm.GetString("lblEmployeeType", culture);
                lblPeriod.Text = rm.GetString("lblPeriod", culture);
                lblPresentCounter.Text = rm.GetString("lblPresent", culture);
                lblAbsentCounter.Text = rm.GetString("lblAbsent", culture);
                lblTotalCounter.Text = rm.GetString("lblTotal", culture);                
                lblOffset.Text = rm.GetString("lblOffset", culture);
                lblPresent.Text = lblAbsent.Text = lblTotal.Text = "0";
                
                chbTwoDaysView.Text = rm.GetString("chbTwoDayView", culture);
                chbRegistration.Text = rm.GetString("chbRegistration", culture);
                chbNoRegistration.Text = rm.GetString("chbNoRegistration", culture);
                chbUnjustifiedAbs.Text = rm.GetString("chbUnjustifiedAbs", culture);
                
                rbHierarchically.Text = rm.GetString("rbHierarchically", culture);
                rbSelected.Text = rm.GetString("rbSelected", culture);
                rbResponsiblePerson.Text = rm.GetString("rbResponsiblePerson", culture);
                rbPassType.Text = rm.GetString("rbPassType", culture);
                rbLocation.Text = rm.GetString("rbLocation", culture);
                                
                btnShow.Text = rm.GetString("btnShow", culture);
                btnAssignTimeSchema.Text = rm.GetString("btnAssignTS", culture);
                btnExportReport.Text = rm.GetString("btnReport", culture);
            }
            catch (Exception ex)
            {
                throw ex;
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

                DateTime day = CommonWeb.Misc.createDate(tbSelectedDate.Text.Trim()).Date;

                if (!day.Equals(new DateTime()) && lboxEmployees.Items.Count > 0)
                {
                    int offset = -1;
                    // check offset if registration check is checked
                    if ((chbRegistration.Checked || chbNoRegistration.Checked) && (!int.TryParse(tbOffset.Text.Trim(), out offset) || offset < 0))
                    {
                        lblError.Text = rm.GetString("noRegularOffset", culture);
                        return;
                    }

                    lblGraphDay.Text = day.ToString(Constants.dateFormat.Trim());

                    if (chbTwoDaysView.Checked)
                        lblGraphDay.Text += "-" + day.AddDays(1).ToString(Constants.dateFormat.Trim());

                    string emplIDs = "";
                    if (lboxEmployees.GetSelectedIndices().Length > 0)
                    {
                        foreach (int emplIndex in lboxEmployees.GetSelectedIndices())
                        {
                            if (emplIndex >= 0 && emplIndex < lboxEmployees.Items.Count)
                                emplIDs += lboxEmployees.Items[emplIndex].Value.Trim() + ",";
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

                    Dictionary<int, EmployeeAsco4TO> emplAdditionalData = new EmployeeAsco4(Session[Constants.sessionConnection]).SearchDictionary(emplIDs);

                    List<EmployeeTO> emplList = new Employee(Session[Constants.sessionConnection]).Search(emplIDs);

                    List<IOPairProcessedTO> IOPairList = FindIOPairsForEmployee(day, emplIDs);

                    DrawGraphControl(day, IOPairList, PassTypes, WUnits, Empl, emplList, emplAdditionalData, emplIDs, offset);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<IOPairProcessedTO> FindIOPairsForEmployee(DateTime day, string emplIDs)
        {
            try
            {
                List<DateTime> datesList = new List<DateTime>();
                datesList.Add(day);

                if (chbTwoDaysView.Checked)
                    datesList.Add(day.AddDays(1));

                return new IOPairProcessed(Session[Constants.sessionConnection]).SearchAllPairsForEmpl(emplIDs, datesList, "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DrawGraphControl(DateTime day, List<IOPairProcessedTO> IOPairList, Dictionary<int, PassTypeTO> PassTypes, Dictionary<int, WorkingUnitTO> WUnits,
            EmployeeTO Empl, List<EmployeeTO> emplList, Dictionary<int, EmployeeAsco4TO> emplAddData, string emplIDs, int offset)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLTMDataPage).Assembly);
                                
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

                // get all regular works
                Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> regularWorks = new Common.Rule(Session[Constants.sessionConnection]).SearchTypeAllRules(Constants.RuleCompanyRegularWork);

                DateTime firstDay = day.Date;
                DateTime lastDay = day.Date;

                if (chbTwoDaysView.Checked)
                    lastDay = day.Date.AddDays(1);

                // create dictionary with pairs for specific employee for specific day - key is employeeID, value is dictionary of day and pairs for that day
                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplDayPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();                

                foreach (IOPairProcessedTO pair in IOPairList)
                {
                    if (!emplDayPairs.ContainsKey(pair.EmployeeID))
                        emplDayPairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());

                    if (!emplDayPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                        emplDayPairs[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<IOPairProcessedTO>());

                    emplDayPairs[pair.EmployeeID][pair.IOPairDate.Date].Add(pair);
                }

                // get schedules for all employees
                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule(Session[Constants.sessionConnection]).SearchEmployeesSchedulesExactDate(emplIDs, firstDay.Date, lastDay.Date, null);

                // get all time schemas
                Dictionary<int, WorkTimeSchemaTO> schemas = new TimeSchema(Session[Constants.sessionConnection]).getDictionary();

                // get all alerts
                Dictionary<int, Dictionary<DateTime, int>> alerts = new IOPairsProcessedHist(Session[Constants.sessionConnection]).SearchAlerts(emplIDs, firstDay.Date, lastDay.Date);

                // if checking registration is checked, get all passes
                Dictionary<int, Dictionary<DateTime, List<PassTO>>> emplPasses = new Dictionary<int, Dictionary<DateTime, List<PassTO>>>();
                if (chbRegistration.Checked || chbNoRegistration.Checked)
                    emplPasses = new Pass(Session[Constants.sessionConnection]).SearchPassesForEmployeesPeriod(emplIDs, firstDay.Date, lastDay.Date);

                // get locations if pairs should be shown by locations
                Dictionary<int, LocationTO> locDict = new Dictionary<int, LocationTO>();
                if (rbLocation.Checked)
                    locDict = new Location(Session[Constants.sessionConnection]).SearchDict();

                // dispose and clear existing controls
                foreach (Control ctrl in ctrlHolder.Controls)
                {
                    ctrl.Dispose();
                }

                ctrlHolder.Controls.Clear();

                // draw pair bars for each selected employee
                int currentIndex = 0;
                int present = 0;
                int absent = emplList.Count;
                int unjustified = 0;
                foreach (EmployeeTO employee in emplList)
                {
                    int emplCompany = Common.Misc.getRootWorkingUnit(employee.WorkingUnitID, WUnits);

                    if (employee.EmployeeID != -1)
                    {
                        List<EmployeeTimeScheduleTO> timeScheduleList = new List<EmployeeTimeScheduleTO>();
                        if (emplSchedules.ContainsKey(employee.EmployeeID))
                            timeScheduleList = emplSchedules[employee.EmployeeID];
                        
                        DateTime currDay = new DateTime(firstDay.Year, firstDay.Month, firstDay.Day, 0, 0, 0);

                        bool isPresent = false;
                        // get employee cut off date
                        int cutOffDate = -1;
                        if (emplCompany != -1 && cuttOffDays.ContainsKey(emplCompany) && cuttOffDays[emplCompany].ContainsKey(employee.EmployeeTypeID)
                            && cuttOffDays[emplCompany][employee.EmployeeTypeID].ContainsKey(Constants.RuleCutOffDate))
                            cutOffDate = cuttOffDays[emplCompany][employee.EmployeeTypeID][Constants.RuleCutOffDate].RuleValue;

                        // get employee regular work
                        int regularWorkID = -1;
                        if (emplCompany != -1 && regularWorks.ContainsKey(emplCompany) && regularWorks[emplCompany].ContainsKey(employee.EmployeeTypeID)
                            && regularWorks[emplCompany][employee.EmployeeTypeID].ContainsKey(Constants.RuleCompanyRegularWork))
                            regularWorkID = regularWorks[emplCompany][employee.EmployeeTypeID][Constants.RuleCompanyRegularWork].RuleValue;

                        // get employee hiring and termination dates
                        DateTime emplHiringDate = new DateTime();
                        DateTime emplTerminationDate = new DateTime();

                        if (emplAddData.ContainsKey(employee.EmployeeID))
                        {
                            emplHiringDate = emplAddData[employee.EmployeeID].DatetimeValue2;

                            if (employee.Status.Trim().ToUpper().Equals(Constants.statusRetired.Trim().ToUpper()))
                                emplTerminationDate = emplAddData[employee.EmployeeID].DatetimeValue3;
                        }

                        while (currDay.Date <= lastDay.Date)
                        {
                            List<IOPairProcessedTO> ioPairsForDay = new List<IOPairProcessedTO>();

                            if (emplDayPairs.ContainsKey(employee.EmployeeID) && emplDayPairs[employee.EmployeeID].ContainsKey(currDay.Date))
                                ioPairsForDay = emplDayPairs[employee.EmployeeID][currDay.Date];

                            int hours = 0;
                            int min = 0;
                            
                            foreach (IOPairProcessedTO iopair in ioPairsForDay)
                            {
                                // calculate presence of employee just for selected day
                                // employee is present if has at least one pair of regular work or overtime
                                if (!isPresent && currDay.Date.Equals(firstDay.Date))
                                {
                                    if ((PassTypes.ContainsKey(iopair.PassTypeID) && PassTypes[iopair.PassTypeID].IsPass == Constants.overTimeID)
                                        || (regularWorkID != -1 && (iopair.PassTypeID == regularWorkID || iopair.PassTypeID == Constants.nightShiftWork || iopair.PassTypeID == Constants.earnedHoursID || iopair.PassTypeID == Constants.workOnHoliday || iopair.PassTypeID == Constants.pause1 || iopair.PassTypeID==Constants.neopravdaniPrekovremeni || iopair.PassTypeID == Constants.usedBankHours1 || iopair.PassTypeID == Constants.bankHours1 || iopair.PassTypeID == Constants.homeWork1 || iopair.PassTypeID == Constants.workHolidayPlusNightWOrk1 || iopair.PassTypeID == Constants.nightWorkOvertime1 || iopair.PassTypeID == Constants.nightWorkOvertimeUnjustified1 || iopair.PassTypeID == Constants.overtimeUnjustified)))
                                        isPresent = true;
                                }

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

                            if (currDay.Date == firstDay.Date && (chbNoRegistration.Checked || chbRegistration.Checked))
                            {
                                int numOfPasses = 0;

                                if (emplPasses.ContainsKey(employee.EmployeeID) && emplPasses[employee.EmployeeID].ContainsKey(currDay.Date))
                                {
                                    // get interval start
                                    DateTime shiftStart = new DateTime();
                                    foreach (WorkTimeIntervalTO interval in timeSchemaIntervalList)
                                    {
                                        if (interval.StartTime.TimeOfDay != new TimeSpan(0, 0, 0))
                                        {
                                            shiftStart = new DateTime(currDay.Year, currDay.Month, currDay.Day, interval.StartTime.Hour, interval.StartTime.Minute, 0);
                                            break;
                                        }
                                    }

                                    if (!shiftStart.Equals(new DateTime()))
                                    {
                                        foreach (PassTO pass in emplPasses[employee.EmployeeID][currDay.Date])
                                        {
                                            if (pass.Direction.Trim().ToUpper() == Constants.DirectionIn.Trim().ToUpper()
                                                && pass.EventTime >= shiftStart.AddMinutes(-offset) && pass.EventTime <= shiftStart.AddMinutes(offset))
                                                numOfPasses++;
                                        }
                                    }
                                }

                                if (chbRegistration.Checked || chbNoRegistration.Checked || chbUnjustifiedAbs.Checked)
                                    isPresent = numOfPasses > 0;

                                if ((chbRegistration.Checked && numOfPasses <= 0) || (chbNoRegistration.Checked && numOfPasses > 0) || chbUnjustifiedAbs.Checked)
                                    break;                                
                            }

                            WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                            if (timeSchemaIntervalList.Count > 0 && schemas.ContainsKey(timeSchemaIntervalList[0].TimeSchemaID))
                                schema = schemas[timeSchemaIntervalList[0].TimeSchemaID];

                            string emplData = employee.LastName.Trim() + " " + employee.FirstName.Trim();

                            Color backColor = Color.White;
                            bool isAltCtrl = false;

                            if (currentIndex % 2 != 0)
                            {
                                backColor = ColorTranslator.FromHtml(Constants.emplDayViewAltColor);
                                isAltCtrl = true;
                            }

                            
                            bool wholeDayAbsencePair = true;
                            if (chbUnjustifiedAbs.Checked)
                            {
                                foreach (IOPairProcessedTO iopair in ioPairsForDay)
                                {
                                    if (iopair.PassTypeID == Constants.absence)
                                    {
                                        foreach (WorkTimeIntervalTO interval in timeSchemaIntervalList)
                                        {
                                            if (!Common.Misc.isWholeIntervalPair(iopair, interval, schema))
                                            {
                                                //postoji neopravdano odusutvo
                                                wholeDayAbsencePair = false;
                                                unjustified++;
                                            }
                                        }
                                    }
                                }

                                if (wholeDayAbsencePair)
                                {
                                    currDay = currDay.Date.AddDays(1);
                                    continue;
                                }
                            }
                            
                            EmployeeWorkingDayViewUC emplDay = new EmployeeWorkingDayViewUC();
                            emplDay.ID = "emplDayView" + currentIndex.ToString();
                            emplDay.DayPairList = ioPairsForDay;
                            emplDay.DayIntervalList = timeSchemaIntervalList;
                            emplDay.PassTypes = PassTypes;
                            emplDay.Locations = locDict;
                            emplDay.WUnits = WUnits;
                            emplDay.Empl = employee;
                            emplDay.BackColor = backColor;
                            emplDay.Date = currDay;
                            emplDay.SecondData = timeString.Trim();
                            emplDay.FirstData = emplData;
                            emplDay.ShowReallocated = true;
                            if (currentIndex == 0)
                                emplDay.IsFirst = true;
                            emplDay.IsAltCtrl = isAltCtrl;
                            emplDay.ShowAlert = alerts.ContainsKey(employee.EmployeeID) && alerts[employee.EmployeeID].ContainsKey(currDay.Date) && alerts[employee.EmployeeID][currDay.Date] > 0;
                            emplDay.EmplTimeSchema = schema;
                            emplDay.ByLocations = rbLocation.Checked;
                            emplDay.PostBackCtrlID = btnShow.ID;

                            if (!emplHiringDate.Equals(new DateTime()) && emplHiringDate.Date <= currDay.Date && (emplTerminationDate.Equals(new DateTime()) || emplTerminationDate >= currDay.Date))
                            {
                                // get number of working days
                                int workingDaysNum = Common.Misc.countWorkingDays(DateTime.Now.Date, Session[Constants.sessionConnection]);

                                // if employee worked on current day, check if data could be changed
                                if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                                && (((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRSSC))
                                   // || ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.WCManager))  // NENAD 02.03.2018 samo hrssc moze da menja parove za prethodni mesec
                                {
                                    if (currDay.Date >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0)) //MM

                                    //if (currDay.Date >= new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 1, 0, 0, 0))
                                    //if (currDay.Date >= new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1, 0, 0, 0))// tamara 3.07.2018.
                                        emplDay.AllowChange = true;
                                    else if (hrsscCutoffDate != -1 && workingDaysNum <= hrsscCutoffDate
                                            && currDay.Date >= new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1, 0, 0, 0))
                                        emplDay.AllowChange = true;
                                    else
                                        emplDay.AllowChange = false;
                                }
                                
                            else if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                                && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.WCManager)
                            {
                                if (CommonWeb.Misc.isVerifiedConfirmedDay(ioPairsForDay, CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser])))
                                    emplDay.AllowVerify = false;
                                else //if (currDay.Date >= new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1, 0, 0, 0)) // tamara 3.07.2018.
                                    if (currDay.Date >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0)) //tamara 3.07.2018.
                                    {
                                        //emplDay.AllowVerify = true;
                                        emplDay.AllowChange = true;

                                    }
                                    else if (wcdrCutoffDate != -1 && workingDaysNum <= wcdrCutoffDate
                                    && currDay.Date >= new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1, 0, 0, 0))

                                        //emplDay.AllowVerify = true;
                                        emplDay.AllowChange = true;
                                    else
                                        //emplDay.AllowVerify = false;
                                        //emplDay.AllowChange = true; tamara 3.07.2018.
                                        emplDay.AllowChange = false; //tamara 3.07.2018.
                            }
                                 
                                else
                                {
                                    if (CommonWeb.Misc.isVerifiedConfirmedDay(ioPairsForDay, CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser])))
                                        emplDay.AllowChange = false;
                                    else //if (currDay.Date >= new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1, 0, 0, 0))
                                        if (currDay.Date >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0)) //tamara 3.07.2018.
                                        emplDay.AllowChange = true;
                                    else if (cutOffDate != -1 && workingDaysNum <= cutOffDate
                                        && currDay.Date >= new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1, 0, 0, 0))
                                        emplDay.AllowChange = true;
                                    else
                                        emplDay.AllowChange = false;
                                }
                            }
                            else
                                emplDay.AllowChange = emplDay.AllowConfirm = emplDay.AllowUndoVerify = emplDay.AllowVerify = false;

                            ctrlHolder.Controls.Add(emplDay);

                            currDay = currDay.AddDays(1);
                            currentIndex++;
                        }
                        
                        if (isPresent)
                        {
                            if (!chbUnjustifiedAbs.Checked)
                            {
                                present++;
                                absent--;
                            }
                        }
                    }
                }
                if (!chbUnjustifiedAbs.Checked)
                {
                    lblPresent.Text = present.ToString().Trim();
                    lblAbsent.Text = absent.ToString().Trim();
                    lblTotal.Text = (present + absent).ToString().Trim();
                }
                else
                {
                    lblPresent.Text = "0";
                    lblAbsent.Text = unjustified.ToString().Trim();
                    lblTotal.Text = unjustified.ToString().Trim();
                }
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

                if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                    && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID != (int)Constants.Categories.HRSSC)
                    btnShow_Click(this, new EventArgs());

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLTMDataPage.Menu1_MenuItemClick(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLTMDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLTMDataPage.Date_Changed(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLTMDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLTMDataPage.cbEmplType_SelectedIndexChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLTMDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnShow_Click(Object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";

                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLTMDataPage).Assembly);

                DateTime date = CommonWeb.Misc.createDate(tbDate.Text.Trim());

                if (date.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidDateFormat", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }
                else
                {
                    tbSelectedDate.Text = tbDate.Text;

                    Session[Constants.sessionFromDate] = date;
                    Session[Constants.sessionToDate] = date;
                }

                if (lboxEmployees.Items.Count <= 0)
                {
                    lblError.Text = rm.GetString("noEmployeesForTMData", culture);
                    return;
                }

                int offset = -1;
                // check offset if registration check is checked
                if ((chbRegistration.Checked || chbNoRegistration.Checked) && (!int.TryParse(tbOffset.Text.Trim(), out offset) || offset < 0))
                {
                    lblError.Text = rm.GetString("noRegularOffset", culture);
                    return;
                }

                SaveState();

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
                
                if (lboxEmployees.GetSelectedIndices().Length == 1)
                {
                    int emplID = -1;
                    int index = lboxEmployees.GetSelectedIndices()[0];

                    if (index >= 0 && index < lboxEmployees.Items.Count && int.TryParse(lboxEmployees.Items[index].Value.Trim(), out emplID))
                    {                        
                        Response.Redirect("/ACTAWeb/ACTAWebUI/WCSelfTMDataPage.aspx?reloadState=false&emplID=" + emplID.ToString().Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLTMDataPage.aspx", false);

                        // do not write load time to a file, becouse user is redirected to another page
                        Message = "";
                    }
                }
                else
                {
                    InitializeGraphData();
                    writeLog(DateTime.Now, false);
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLTMDataPage.btnShow_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLTMDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnPrev_Click(Object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";

                // change date in tbDate, subtract one day                
                tbDate.Text = CommonWeb.Misc.createDate(tbSelectedDate.Text.Trim()).AddDays(-1).ToString(Constants.dateFormat);

                Date_Changed(this, new EventArgs());

                btnShow_Click(this, new EventArgs());
                
                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLTMDataPage.btnPrev_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLTMDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnNext_Click(Object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";

                // change date in tbDate, add one day                
                tbDate.Text = CommonWeb.Misc.createDate(tbSelectedDate.Text.Trim()).AddDays(1).ToString(Constants.dateFormat);

                Date_Changed(this, new EventArgs());
                
                btnShow_Click(this, new EventArgs());
                
                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLTMDataPage.btnNext_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLTMDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void rbPassType_CheckedChanged(Object sender, EventArgs e)
        {
            try
            {
                if (rbPassType.Checked)
                    rbLocation.Checked = !rbPassType.Checked;

                lblError.Text = "";
                
                btnShow_Click(this, new EventArgs());

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLTMDataPage.rbPassType_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLTMDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void rbLocation_CheckedChanged(Object sender, EventArgs e)
        {
            try
            {
                if (rbLocation.Checked)
                    rbPassType.Checked = !rbLocation.Checked;

                lblError.Text = "";

                btnShow_Click(this, new EventArgs());

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLTMDataPage.rbLocation_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLTMDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnAssignTS_Click(Object sender, EventArgs e)
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

                    // get string with selected employees
                    string emplIDs = "";
                    foreach (int emplIndex in lboxEmployees.GetSelectedIndices())
                    {
                        if (emplIndex >= 0 && emplIndex < lboxEmployees.Items.Count)
                            emplIDs += lboxEmployees.Items[emplIndex].Value.Trim() + ",";
                    }

                    if (emplIDs.Length > 0)
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                    if (lboxEmployees.GetSelectedIndices().Length == 1)
                        Response.Redirect("/ACTAWeb/ACTAWebUI/TimeSchemaAssignSingleEmployeePage.aspx?emplIDs=" + emplIDs.Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLTMDataPage.aspx", false);
                    else if (!(Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                        && (((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRLegalEntity
                        || ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.WCManager)))
                        Response.Redirect("/ACTAWeb/ACTAWebUI/TimeSchemaAssignMultipleEmployeesPage.aspx?emplIDs=" + emplIDs.Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLTMDataPage.aspx", false);
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLTMDataPage.btnAssignTS_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLTMDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void chbRegistration_OnCheckChanged(Object sender, EventArgs e)
        {
            try
            {
                if (chbRegistration.Checked)
                {
                    chbNoRegistration.Checked = false;
                    chbUnjustifiedAbs.Checked = false;

                }
                // get customer, set Report button visible only for PMC
                //string costumer = Common.Misc.getCustomer(Session[Constants.sessionConnection]);
                //int cost = 0;
                //bool costum = int.TryParse(costumer, out cost);
                //if (cost == (int)Constants.Customers.PMC)
                //    btnExportReport.Visible = chbNoRegistration.Checked;

                setTotalLabelsText();

                btnShow_Click(this, new EventArgs());
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLTMDataPage.chbRegistration_OnCheckChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLTMDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void chbUnjustifiedAbs_OnCheckChanged(Object sender, EventArgs e)
        {
            try
            {
                if (chbUnjustifiedAbs.Checked)
                {
                    chbNoRegistration.Checked = false;
                    chbRegistration.Checked = false;
                }

                // get customer, set Report button visible only for PMC
                //string costumer = Common.Misc.getCustomer(Session[Constants.sessionConnection]);
                //int cost = 0;
                //bool costum = int.TryParse(costumer, out cost);
                //if (cost == (int)Constants.Customers.PMC)
                //    btnExportReport.Visible = chbNoRegistration.Checked;

                setTotalLabelsText();

                btnShow_Click(this, new EventArgs());
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLTMDataPage.chbUnjustifiedAbs_OnCheckChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLTMDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }


        protected void chbNoRegistration_OnCheckChanged(Object sender, EventArgs e)
        {
            try
            {
                // get customer, set Report button visible only for PMC
                //string costumer = Common.Misc.getCustomer(Session[Constants.sessionConnection]);
                //int cost = 0;
                //bool costum = int.TryParse(costumer, out cost);
                //if (cost == (int)Constants.Customers.PMC)
                //    btnExportReport.Visible = chbNoRegistration.Checked;

                if (chbNoRegistration.Checked)
                {
                    chbRegistration.Checked = false;
                    chbUnjustifiedAbs.Checked = false;
                }

                setTotalLabelsText();

                btnShow_Click(this, new EventArgs());
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLTMDataPage.chbNoRegistration_OnCheckChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLTMDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnExportReport_Click(Object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLTMDataPage.btnExportReport_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLTMDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setTotalLabelsText()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLTMDataPage).Assembly);

                if (chbRegistration.Checked || chbNoRegistration.Checked)
                {
                    lblPresentCounter.Text = rm.GetString("chbRegistration", culture);
                    lblAbsentCounter.Text = rm.GetString("chbNoRegistration", culture);
                }
                else
                {
                    lblPresentCounter.Text = rm.GetString("lblPresent", culture);
                    lblAbsentCounter.Text = rm.GetString("lblAbsent", culture);
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

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "TLTMDataPage.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "TLTMDataPage.", filterState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
