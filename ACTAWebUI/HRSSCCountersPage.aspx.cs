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
    public partial class HRSSCCountersPage : System.Web.UI.Page
    {
        const string pageName = "HRSSCCountersPage";

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
                    // clear counters to present database state
                    ClearSessionValues();

                    btnWUTree.Attributes.Add("onclick", "return wUnitsPicker('btnWUTree', 'false');");
                    btnOrgTree.Attributes.Add("onclick", "return oUnitsPicker('btnOrgTree', 'false');");
                    tbEmployee.Attributes.Add("onKeyUp", "return selectItem('tbEmployee', 'lboxEmployees');");
                    btnShow.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnSave.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnBH_Report.Attributes.Add("onclick", "return document.body.style.cursor='wait'");
                    btnGO_Report.Attributes.Add("onclick", "return document.body.style.cursor='wait'");

                    btnWUTree.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnWUTree.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnOrgTree.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnOrgTree.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");

                    // populate default working unit and organizational unit info
                    // get responsibility working unit (employee_asco4.integer_value_2)
                    // get responsibility organizational unit (employee_asco4.integer_value_3)
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

                    resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/CountersPage.aspx";
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in HRSSCCountersPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/HRSSCCountersPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void ClearSessionValues()
        {
            try
            {
                // clean Session
                Session[Constants.sessionCounters] = null;
                Session[Constants.sessionCountersEmployees] = null;
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
                // from is first day of previous month, to is current day
                DateTime from = new DateTime(DateTime.Now.Date.AddMonths(-1).Year, DateTime.Now.Date.AddMonths(-1).Month, 1, 0, 0, 0).Date;
                DateTime to = DateTime.Now.Date;

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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in HRSSCCountersPage.cbEmplType_SelectedIndexChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/HRSSCCountersPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(HRSSCCountersPage).Assembly);

                lblEmployee.Text = rm.GetString("lblEmployeeSelection", culture);
                lblEmplType.Text = rm.GetString("lblEmployeeType", culture);
                                
                btnShow.Text = rm.GetString("btnShow", culture);
                btnSave.Text = rm.GetString("btnSave", culture);
                btnBH_Report.Text = "Izveštaj banka sati";
                btnGO_Report.Text = "Izveštaj god. odmor";
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

                populateEmplTypes();
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in HRSSCCountersPage.Menu1_MenuItemClick(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/HRSSCCountersPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnGO_Report_Click(Object sender, EventArgs e)
        {
            lblError.Text = "";
            ClearSessionValues();
            if (lboxEmployees.Items.Count <= 0)
            {
                lblError.Text = "Nema radnika za izabrane kriterijume";
                writeLog(DateTime.Now, false);
                return;
            }
            string emplIDs = "";
            if (lboxEmployees.GetSelectedIndices().Length > 0)
            {
                List<string> idList = new List<string>();
                foreach (var emplIndex in lboxEmployees.GetSelectedIndices())
                {
                    if (emplIndex >= 0 && emplIndex < lboxEmployees.Items.Count)
                    {
                        idList.Add(lboxEmployees.Items[emplIndex].Value.Trim());
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
            {
                emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
            }
            SaveState();
            Dictionary<EmployeeTO, List<EmployeeCounterValueTO>> listEmplCounter = new Dictionary<EmployeeTO, List<EmployeeCounterValueTO>>();
            List<EmployeeTO> emplList = new Employee(Session[Constants.sessionConnection]).Search(emplIDs);
            foreach (EmployeeTO empl in emplList)
            {
                EmployeeCounterValue emplCounterValue = new EmployeeCounterValue(Session[Constants.sessionConnection]);
                emplCounterValue.ValueTO.EmplID = empl.EmployeeID;
                List<EmployeeCounterValueTO> counterValues = emplCounterValue.Search();
                if (counterValues.Count > 0)
                {
                    listEmplCounter.Add(empl, counterValues);
                }
            }
            Session["ListEmployeesCounters"] = listEmplCounter;

            string reportUrl = "/ACTAWeb/ReportsWeb/sr/SMPAnnualLeaveReport.aspx";
            Response.Redirect("/ACTAWeb/ReportsWeb/ReportPage.aspx?Back=/ACTAWeb/ACTAWebUI/HRSSCCountersPage.aspx&Report="+reportUrl.Trim(), false);
        }

        protected void btnBH_Report_Click(Object sender, EventArgs e)
        {
            lblError.Text = "";
            ClearSessionValues();
            if (lboxEmployees.Items.Count <= 0)
            {
                lblError.Text = "Nema radnika za izabrane kriterijume";
                writeLog(DateTime.Now, false);
                return;
            }
            string emplIDs = "";
            if (lboxEmployees.GetSelectedIndices().Length > 0)
            {
                List<string> idList = new List<string>();
                foreach (var emplIndex in lboxEmployees.GetSelectedIndices())
                {
                    if (emplIndex >= 0 && emplIndex < lboxEmployees.Items.Count)
                    {
                        idList.Add(lboxEmployees.Items[emplIndex].Value.Trim());
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
            {
                emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
            }
            SaveState();
            Dictionary<EmployeeTO, List<EmployeeCounterValueTO>> listEmplCounter = new Dictionary<EmployeeTO, List<EmployeeCounterValueTO>>();
            List<EmployeeTO> emplList = new Employee(Session[Constants.sessionConnection]).Search(emplIDs);
            foreach (EmployeeTO empl in emplList)
            {
                EmployeeCounterValue emplCounterValue = new EmployeeCounterValue(Session[Constants.sessionConnection]);
                emplCounterValue.ValueTO.EmplID = empl.EmployeeID;
                List<EmployeeCounterValueTO> counterValues = emplCounterValue.Search();
                if (counterValues.Count > 0)
                {
                    listEmplCounter.Add(empl, counterValues);
                }
            }
            Session["ListEmployeesCounters"] = listEmplCounter;

            string reportUrl = "/ACTAWeb/ReportsWeb/sr/SMPBankHoursReport.aspx";
            Response.Redirect("/ACTAWeb/ReportsWeb/ReportPage.aspx?Back=/ACTAWeb/ACTAWebUI/HRSSCCountersPage.aspx&Report=" + reportUrl.Trim(), false);
        }

        protected void btnShow_Click(Object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                
                ClearSessionValues();

                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(HRSSCCountersPage).Assembly);
                                

                if (lboxEmployees.Items.Count <= 0)
                {
                    lblError.Text = rm.GetString("noEmployeesForTMData", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                // get selected employees
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

                SaveState();

                Session[Constants.sessionCountersEmployees] = emplIDs;

                resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/CountersPage.aspx?readOnly=false";
                                
                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in HRSSCCountersPage.btnShow_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/HRSSCCountersPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
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

                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(HRSSCCountersPage).Assembly);

                EmployeeCounterValue counter = new EmployeeCounterValue(Session[Constants.sessionConnection]);
                EmployeeCounterValueHist counterHist = new EmployeeCounterValueHist(Session[Constants.sessionConnection]);

                bool saved = true;
                
                // update counters from session, updated counters insert to hist table
                if (Session[Constants.sessionCounters] != null && Session[Constants.sessionCounters] is Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>)
                {
                    Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplCounters = new Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>();
                    emplCounters = (Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>)Session[Constants.sessionCounters];

                    if (emplCounters.Count <= 0)
                    {
                        writeLog(DateTime.Now, false);
                        return;
                    }

                    // get old counters
                    string emplIDs = "";
                    foreach (int id in emplCounters.Keys)
                    {
                        emplIDs += id.ToString().Trim() + ",";
                    }

                    if (emplIDs.Length > 0)
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                    Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplOldCounters = new EmployeeCounterValue(Session[Constants.sessionConnection]).SearchValues(emplIDs);

                    if (counter.BeginTransaction())
                    {
                        try
                        {
                            counterHist.SetTransaction(counter.GetTransaction());

                            // update counters and move old counter values to hist table if updated                            
                            foreach (int emplID in emplCounters.Keys)
                            {
                                foreach (int type in emplCounters[emplID].Keys)
                                {
                                    if (emplOldCounters.ContainsKey(emplID) && emplOldCounters[emplID].ContainsKey(type)
                                        && emplOldCounters[emplID][type].Value != emplCounters[emplID][type].Value)
                                    {
                                        // move to hist table
                                        counterHist.ValueTO = new EmployeeCounterValueHistTO(emplOldCounters[emplID][type]);
                                        counterHist.ValueTO.ModifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                                        saved = saved && (counterHist.Save(false) >= 0);

                                        if (!saved)
                                            break;

                                        counter.ValueTO = emplCounters[emplID][type];
                                        counter.ValueTO.ModifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);

                                        saved = saved && counter.Update(false);

                                        if (!saved)
                                            break;
                                    }
                                }
                            }

                            if (saved)
                            {
                                counter.CommitTransaction();
                                ClearSessionValues();
                                lblError.Text = rm.GetString("countersSaved", culture);
                            }
                            else
                            {
                                counter.RollbackTransaction();
                                lblError.Text = rm.GetString("countersSavingFailed", culture);
                            }
                        }
                        catch
                        {
                            if (counter.GetTransaction() != null)
                                counter.RollbackTransaction();

                            lblError.Text = rm.GetString("countersSavingFailed", culture);
                        }
                    }
                    else
                    {
                        lblError.Text = rm.GetString("countersSavingFailed", culture);
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in HRSSCCountersPage.btnSave_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/HRSSCCountersPage.aspx", false);
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

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "HRSSCCountersPage.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "HRSSCCountersPage.", filterState);
                }
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
    }
}
