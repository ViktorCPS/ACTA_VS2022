using System;
using System.Collections;
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
using Util;
using System.Globalization;
using System.Resources;
using System.Collections.Generic;
using TransferObjects;
using Common;
using System.Text;

namespace ACTAWebUI
{
    public partial class TimeSchemaAssignSingleEmployeePage : System.Web.UI.Page
    {
        const string vsEmplSchemaAssignSelFrom = "vsEmplSchemaAssignSelFrom";
        const string vsEmplSchemaAssignSelTo = "vsEmplSchemaAssignSelTo";
        const string vsEmplLockedDays = "vsEmplLockedDays";
        const string sessionCycleDayList = "TimeSchemaAssignSingleEmployeePage.CycleDayList";
        const string sessionChangedDaysList = "TimeSchemaAssignSingleEmployeePage.ChangedDaysList";

        
        private DateTime SelectedFrom
        {
            get
            {
                DateTime selFrom = new DateTime();
                if (ViewState[vsEmplSchemaAssignSelFrom] != null && ViewState[vsEmplSchemaAssignSelFrom] is DateTime)
                {
                    selFrom = (DateTime)ViewState[vsEmplSchemaAssignSelFrom];
                }

                return selFrom;
            }
            set
            {
                ViewState[vsEmplSchemaAssignSelFrom] = value;
            }
        }

        private DateTime SelectedTo
        {
            get
            {
                DateTime selTo = new DateTime();
                if (ViewState[vsEmplSchemaAssignSelTo] != null && ViewState[vsEmplSchemaAssignSelTo] is DateTime)
                {
                    selTo = (DateTime)ViewState[vsEmplSchemaAssignSelTo];
                }

                return selTo;
            }
            set
            {
                ViewState[vsEmplSchemaAssignSelTo] = value;
            }
        }

        const string pageName = "TimeSchemaAssignSingleEmployeePage";

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
            tbFrom.Text=fromDate.ToString("dd.MM.yyyy.");
            calendarFrom.Visible = false;
            btnFrom.Visible = true;
        }
        protected void DataToChange(object sender, EventArgs e)
        {
            toDate = calendarTo.SelectedDate;
            tbTo.Text = toDate.ToString("dd.MM.yyyy.");
            calendarTo.Visible = false;
            btnTo.Visible = true;
        }




        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                
                //CheckBox1.Checked = false;
                // if user do not want to reload selected filter and get results for that filter, parameter reloadState should be passes through url and set to false
                if (!IsPostBack)
                {
                    btnFromDate.Attributes.Add("onclick", "return calendarPicker('tbFrom', 'false');");
                    btnToDate.Attributes.Add("onclick", "return calendarPicker('tbTo', 'false');");
                    
                    btnFromDate.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnFromDate.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnToDate.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnToDate.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");

                    ClearSessionValues();
                    InitializeSQLParameters();

                    setLanguage();

                    EmployeeTO Empl = getEmployee();
                    InitializeData(Empl);

                    populateMonthsAndYear();
                    
                    populateTimeSchemas();
                    
                    SetTimeShema(Empl.EmployeeID, CommonWeb.Misc.createDate("01." + (cbMonths.SelectedIndex + 1) + "." + tbYear.Text));
                    
                    resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/SmallResultPage.aspx?showSelection=false";

                    if (!(Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                        && (((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.WCManager
                        || ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRLegalEntity)))
                    {
                        btnAssign.Visible = btnSave.Visible = selectionTable.Visible = true;

                        if (!checkCutOffDate(CommonWeb.Misc.createDate("01." + (cbMonths.SelectedIndex + 1) + "." + tbYear.Text)))
                        {
                            btnAssign.Enabled = false;
                            btnSave.Enabled = false;
                            writeError("cutOffDayPessed");
                        }
                    }
                    else
                    {
                        btnAssign.Visible = btnSave.Visible = selectionTable.Visible = false;
                    }

                    chbCheckLaborLaw.Visible = Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                        && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRSSC;
                }
                
                errorLabel.Text = "";
                DateTime month = CommonWeb.Misc.createDate("01." + (cbMonths.SelectedIndex + 1) + "." + tbYear.Text);                
                setCalendar(month, SelectedFrom, SelectedTo);

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TimeSchemaAssignSingleEmployeePage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TimeSchemaAssignSingleEmployeePage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private bool checkCutOffDate(DateTime dateTime)
        {
            try
            {
                if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO)
                {
                    if ((((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID != (int)Constants.Categories.WCManager
                            && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID != (int)Constants.Categories.HRLegalEntity))
                    {                        
                        if (dateTime.Date < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1))
                            return false;
                        else if (dateTime.Date < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1))// NATALIJA28022018 DOZVOLA ZA MENJANJE JANUARA
                        {
                            // get dictionary of all rules, key is company and value are rules by employee type id
                            Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> emplRules = new Common.Rule(Session[Constants.sessionConnection]).SearchWUEmplTypeDictionary();
                            Dictionary<int, WorkingUnitTO> wUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();

                            int cutOffDate = -1;

                            if (Session[Constants.sessionLogInEmployee] != null && Session[Constants.sessionLogInEmployee] is EmployeeTO)
                            {
                                EmployeeTO empl = (EmployeeTO)Session[Constants.sessionLogInEmployee];
                                int emplCompany = Common.Misc.getRootWorkingUnit(empl.WorkingUnitID, wUnits);

                                if (emplRules.ContainsKey(emplCompany) && emplRules[emplCompany].ContainsKey(empl.EmployeeTypeID))
                                {
                                    if (Session[Constants.sessionLogInUser] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO)
                                    {
                                        string ruleType = "";
                                        if (((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRSSC)
                                            ruleType = Constants.RuleHRSSCCutOffDate;
                                        else
                                            ruleType = Constants.RuleCutOffDate;

                                        if (emplRules[emplCompany][empl.EmployeeTypeID].ContainsKey(ruleType))
                                            cutOffDate = emplRules[emplCompany][empl.EmployeeTypeID][ruleType].RuleValue;
                                    }
                                }
                            }

                            if (cutOffDate >= 0 && Common.Misc.countWorkingDays(DateTime.Now.Date, Session[Constants.sessionConnection]) <= cutOffDate)
                                return true;
                            else
                                return false;
                        }
                        else
                            return true;
                    }
                    else
                        return false;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<string> getSelectionValues(HtmlInputHidden selBox)
        {
            try
            {
                List<string> selKeys = new List<string>();

                string[] selectedKeys = selBox.Value.Trim().Split(Constants.delimiter);

                foreach (string key in selectedKeys)
                {
                    if (!key.Trim().Equals("") && !selKeys.Contains(key))
                        selKeys.Add(key);                    
                }

                return selKeys;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        private void populateMonthsAndYear()
        {
            try
            {
                foreach (KeyValuePair<int, string> pair in getMonths())
                {
                    cbMonths.Items.Add(pair.Value);
                }

                DateTime current = DateTime.Now;
                cbMonths.SelectedIndex = current.Month - 1;
                tbYear.Text = current.Year + ".";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Dictionary<int, string> getMonths()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TimeSchemaAssignSingleEmployeePage).Assembly);

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

        private EmployeeTO getEmployee()
        {
            try
            {
                EmployeeTO emplTO = new EmployeeTO();
                int emplID = -1;

                if (Request.QueryString["emplIDs"] != null)
                {
                    if (!int.TryParse(Request.QueryString["emplIDs"], out emplID))
                        emplID = -1;
                }

                if (emplID != -1)
                {
                    emplTO = new Employee(Session[Constants.sessionConnection]).Find(emplID.ToString().Trim());
                }

                return emplTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private EmployeeAsco4TO getEmployeeAddData()
        {
            try
            {
                EmployeeAsco4TO emplAddTO = new EmployeeAsco4TO();
                int emplID = -1;

                if (Request.QueryString["emplIDs"] != null)
                {
                    if (!int.TryParse(Request.QueryString["emplIDs"], out emplID))
                        emplID = -1;
                }

                if (emplID != -1)
                {
                    List<EmployeeAsco4TO> addList = new EmployeeAsco4(Session[Constants.sessionConnection]).Search(emplID.ToString().Trim());

                    if (addList.Count > 0)
                        emplAddTO = addList[0];
                }

                return emplAddTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeData(EmployeeTO Empl)
        {
            try
            {
                // get employee data
                if (Empl.EmployeeID != -1)
                {
                    CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                    ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TimeSchemaAssignSingleEmployeePage).Assembly);

                    tbFirstName.Text = Empl.FirstName.Trim();
                    tbLastName.Text = Empl.LastName.Trim();
                    tbPayroll.Text = Empl.EmployeeID.ToString().Trim();
                    // get additional employee data
                    EmployeeAsco4 asco = new EmployeeAsco4(Session[Constants.sessionConnection]);
                    asco.EmplAsco4TO.EmployeeID = Empl.EmployeeID;
                    List<EmployeeAsco4TO> ascoList = asco.Search();

                    EmployeeAsco4TO ascoTO = new EmployeeAsco4TO();
                    if (ascoList.Count > 0)
                        ascoTO = ascoList[0];

                    tbStringone.Text = emplTypes(Empl);

                    // tbStringone.Text = ascoList[0].NVarcharValue2.Trim();
                    tbBranch.Text = ascoTO.NVarcharValue6.Trim();

                    // get employee working unit - UTE
                    WorkingUnit wu = new WorkingUnit(Session[Constants.sessionConnection]);
                    WorkingUnitTO tempWU = wu.FindWU(Empl.WorkingUnitID);
                    tbUTE.Text = tempWU.Code.Trim();
                    tbWUnit.Text = tempWU.Name.Trim();
                    tbWUnit.ToolTip = tempWU.Description.Trim() + "(" + tempWU.Code.Trim() + ")";

                    // get workshop (parent of UTE)
                    wu.WUTO = tempWU;
                    tempWU = wu.getParentWorkingUnit();
                    tbWorkgroup.Text = tempWU.Code.Trim();

                    // get cost centar
                    wu.WUTO = tempWU;
                    tempWU = wu.getParentWorkingUnit();
                    tbCostCentar.Text = tempWU.Code.Trim();

                    // get plant
                    wu.WUTO = tempWU;
                    tempWU = wu.getParentWorkingUnit();
                    tbPlant.Text = tempWU.Code.Trim();

                    // get organizational unit
                    OrganizationalUnitTO ou = new OrganizationalUnit(Session[Constants.sessionConnection]).Find(Empl.OrgUnitID);
                    tbOUnit.Text = ou.Code.Trim();
                    tbOUnit.ToolTip = ou.Name.Trim();

                    // get FS responsible persons
                    List<EmployeeTO> emplWUResList = new Employee(Session[Constants.sessionConnection]).SearchEmployeesWUResponsible(Empl.WorkingUnitID.ToString().Trim(), null, DateTime.Now.Date, DateTime.Now.Date);
                    string responsibility = rm.GetString("FSResPerson", culture) + Environment.NewLine;
                    foreach (EmployeeTO empl in emplWUResList)
                    {
                        responsibility += empl.FirstAndLastName.Trim() + Environment.NewLine;
                    }

                    // get OU responsible persons
                    List<EmployeeTO> emplOUResList = new Employee(Session[Constants.sessionConnection]).SearchEmployeesOUResponsible(Empl.OrgUnitID.ToString().Trim(), null, DateTime.Now.Date, DateTime.Now.Date);
                    responsibility += rm.GetString("OUResPerson", culture) + Environment.NewLine;
                    foreach (EmployeeTO empl in emplOUResList)
                    {
                        responsibility += empl.FirstAndLastName.Trim() + Environment.NewLine;
                    }

                    tbStringone.ToolTip = responsibility.Trim();

                    string costumer = Common.Misc.getCustomer(Session[Constants.sessionConnection]);
                    int cost = 0;
                    bool costum = int.TryParse(costumer, out cost);
                    bool isFiat = (cost == (int)Constants.Customers.FIAT);

                    rowBranch.Visible = rowBranchtlbl.Visible = rowPlant.Visible = rowPlantlbl.Visible = rowCC.Visible = rowCClbl.Visible = rowWG.Visible = rowWGlbl.Visible = rowUTE.Visible = rowUTElbl.Visible = isFiat;
                    rowWU.Visible = rowWUlbl.Visible = !isFiat;

                    // create dinamically asco data
                    Dictionary<string, string> ascoMetadata = new EmployeeAsco4Metadata(Session[Constants.sessionConnection]).GetMetadataWebValues(CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]));

                    int counter = 0;
                    foreach (string col in ascoMetadata.Keys)
                    {
                        if (ascoMetadata[col].Trim() == "")
                            continue;

                        // make row separator label
                        Label separatorLbl = new Label();
                        separatorLbl.ID = "separatorLbl" + counter.ToString().Trim();
                        separatorLbl.Width = new Unit(280);
                        separatorLbl.Height = new Unit(5);
                        separatorLbl.CssClass = "contentLblLeft";
                        ascoCtrlHolder.Controls.Add(separatorLbl);

                        // make asco name label
                        Label ascoNameLbl = new Label();
                        ascoNameLbl.ID = "ascoNameLbl" + counter.ToString().Trim();
                        ascoNameLbl.Width = new Unit(280);
                        ascoNameLbl.Text = ascoMetadata[col].Trim() + ":";
                        ascoNameLbl.CssClass = "contentLblLeft";
                        ascoCtrlHolder.Controls.Add(ascoNameLbl);

                        // make pass type counter label
                        TextBox ascoTb = new TextBox();
                        ascoTb.ID = "ascoLbl" + counter.ToString().Trim();
                        ascoTb.Width = new Unit(280);
                        ascoTb.Text = Common.Misc.getAscoValue(ascoTO, col);
                        ascoTb.ReadOnly = true;
                        ascoTb.CssClass = "contentTbDisabled";
                        ascoCtrlHolder.Controls.Add(ascoTb);

                        counter++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string emplTypes(EmployeeTO employee)
        {
            try
            {
                string empl = "";
                //// get selected company
                int company = -1;

                int wuID = employee.WorkingUnitID;
                Dictionary<int, WorkingUnitTO> WUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                company = Common.Misc.getRootWorkingUnit(wuID, WUnits);
                if (company == -1)
                {
                    WorkingUnitXOrganizationalUnit wuXou = new WorkingUnitXOrganizationalUnit(Session[Constants.sessionConnection]);
                    wuXou.WUXouTO.OrgUnitID = employee.OrgUnitID;
                    List<WorkingUnitXOrganizationalUnitTO> list = wuXou.Search();
                    if (list.Count > 0)
                    {
                        WorkingUnitXOrganizationalUnitTO wuXouTO = list[0];
                        company = Common.Misc.getRootWorkingUnit(wuXouTO.WorkingUnitID, WUnits);
                    }
                    else
                        company = -1;
                }
                List<EmployeeTypeTO> listEmplTypes = new List<EmployeeTypeTO>();
                listEmplTypes = new EmployeeType(Session[Constants.sessionConnection]).Search();
                foreach (EmployeeTypeTO emplType in listEmplTypes)
                {
                    if (emplType.EmployeeTypeID == employee.EmployeeTypeID && emplType.WorkingUnitID == company)
                    {
                        empl = emplType.EmployeeTypeName;
                        break;
                    }
                }

                return empl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populateTimeSchemas()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TimeSchemaAssignSingleEmployeePage).Assembly);
                
                //get time schemas not retired and for exect company
                EmployeeTO selEmpl = getEmployee();
                Dictionary<int, WorkingUnitTO> wUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                int company = Common.Misc.getRootWorkingUnit(selEmpl.WorkingUnitID, wUnits);
                TimeSchema timeSchema = new TimeSchema(Session[Constants.sessionConnection]);
                timeSchema.TimeSchemaTO.WorkingUnitID = company;

                List<WorkTimeSchemaTO> tsArray = timeSchema.Search();
                List<WorkTimeSchemaTO> tsArrayNew = new List<WorkTimeSchemaTO>();
                foreach (WorkTimeSchemaTO wts in tsArray)
                {
                    if (wts.Status != Constants.statusRetired)
                    {
                        wts.Name = wts.TimeSchemaID.ToString() + "-" + wts.Name;
                        tsArrayNew.Add(wts);
                    }
                }

                WorkTimeSchemaTO ts = new WorkTimeSchemaTO();
                ts.Name = rm.GetString("all", culture);
                tsArrayNew.Insert(0, ts);

                cbTimeSchema.DataSource = tsArrayNew;
                cbTimeSchema.DataTextField = "Name";
                cbTimeSchema.DataValueField = "TimeSchemaID";

                cbTimeSchema.DataBind();
                cbTimeSchema.SelectedIndex = 0;
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TimeSchemaAssignSingleEmployeePage).Assembly);
                                
                lblWUnit.Text = rm.GetString("lblWUnit", culture);                
                lblPayroll.Text = rm.GetString("lblPayrollID", culture);
                lblEmplData.Text = rm.GetString("lblEmplData", culture);
                lblFirstName.Text = rm.GetString("lblFirstName", culture);
                lblLastName.Text = rm.GetString("lblLastName", culture);
                lblStringone.Text = rm.GetString("lblStringone", culture);
                lblPlant.Text = rm.GetString("lblPlant", culture);
                lblCostCentar.Text = rm.GetString("lblCostCentar", culture);
                lblWorkgroup.Text = rm.GetString("lblWorkgroup", culture);
                lblUTE.Text = rm.GetString("lblUTE", culture);
                lblBranch.Text = rm.GetString("lblBranch", culture);
                lblOUnit.Text = rm.GetString("lblOUnit", culture);
                lblTimeSchema.Text = rm.GetString("lblTimeSchema", culture);
                lblPeriod.Text = rm.GetString("lblPeriod", culture);
                lblSelection.Text = rm.GetString("lblSelection", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblWorkingDay.Text = rm.GetString("lblWorkingDay", culture);
                lblWeekend.Text = rm.GetString("lblWeekend", culture);
                lblNationalHoliday.Text = rm.GetString("lblNationalHoliday", culture);
                lblPersonalHoliday.Text = rm.GetString("lblPersonalHoliday", culture);
                Mon.Text = rm.GetString("hdrMon", culture);
                Tue.Text = rm.GetString("hdrTue", culture);
                Wed.Text = rm.GetString("hdrWed", culture);
                Thu.Text = rm.GetString("hdrThu", culture);
                Fri.Text = rm.GetString("hdrFri", culture);
                Sat.Text = rm.GetString("hdrSat", culture);
                Sun.Text = rm.GetString("hdrSun", culture);

                btnAssign.Text = rm.GetString("btnAssign", culture);
                btnBack.Text = rm.GetString("btnBack", culture);
                btnSave.Text = rm.GetString("btnSave", culture);
                btnSelect.Text = rm.GetString("btnSelect", culture);
                btnClear.Text = rm.GetString("btnClear", culture);//natalija 12012018
                CheckBox1.Text = rm.GetString("CheckBox1", culture);//natalija 22112017
                chbCheckLaborLaw.Text = rm.GetString("chbCheckLaborLaw", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetTimeShema(int employeeID, DateTime month)
        {
            try
            {
                Dictionary<DateTime, cycleDay> dict = new Dictionary<DateTime, cycleDay>();
                if (Session[sessionCycleDayList] != null && Session[sessionCycleDayList] is Dictionary<DateTime, cycleDay>)
                {
                    dict = (Dictionary<DateTime, cycleDay>)Session[sessionCycleDayList];
                }

                DateTime startDate = new DateTime(month.Year, month.Month, 1);                
                DateTime endDate = Common.Misc.getWeekEnding(startDate.AddMonths(1).AddDays(-1));
                                
                EmployeeAsco4 asco = new EmployeeAsco4(Session[Constants.sessionConnection]);
                asco.EmplAsco4TO.EmployeeID = employeeID;
                List<EmployeeAsco4TO> ascoList = asco.Search();
                if (ascoList.Count > 0)
                    asco.EmplAsco4TO = ascoList[0];

                List<DateTime> nationalHolidaysDays = new List<DateTime>();
                List<DateTime> nationalHolidaysSundays = new List<DateTime>();
                List<DateTime> nationalHolidaysSundaysDays = new List<DateTime>();
                List<DateTime> personalHolidayDays = new List<DateTime>();
                Dictionary<string, List<DateTime>> personalHolidayDaysAll = new Dictionary<string, List<DateTime>>();
                List<HolidaysExtendedTO> nationalTransferableHolidays = new List<HolidaysExtendedTO>();

                Common.Misc.getHolidays(startDate.Date, endDate.Date, personalHolidayDaysAll, nationalHolidaysDays, nationalHolidaysSundays, Session[Constants.sessionConnection], nationalTransferableHolidays);
                nationalHolidaysDays.AddRange(nationalHolidaysSundays);
                if (personalHolidayDaysAll.ContainsKey(asco.EmplAsco4TO.NVarcharValue1))
                    personalHolidayDays = personalHolidayDaysAll[asco.EmplAsco4TO.NVarcharValue1];
                else if (asco.EmplAsco4TO.NVarcharValue1 == Constants.holidayTypeIV)
                {
                    DateTime personalHolidayDate = new DateTime();
                    if (asco.EmplAsco4TO.DatetimeValue1 != new DateTime() && asco.EmplAsco4TO.DatetimeValue1.Date != Constants.dateTimeNullValue().Date)
                    {
                        if (asco.EmplAsco4TO.DatetimeValue1.Date.Month == startDate.Month)
                            personalHolidayDate = new DateTime(startDate.Year, startDate.Month, asco.EmplAsco4TO.DatetimeValue1.Date.Day);
                        else if (asco.EmplAsco4TO.DatetimeValue1.Date.Month == endDate.Month)
                            personalHolidayDate = new DateTime(endDate.Year, endDate.Month, asco.EmplAsco4TO.DatetimeValue1.Date.Day);
                    }

                    if (personalHolidayDate.Date >= startDate.Date && personalHolidayDate.Date <= endDate.Date)
                        personalHolidayDays.Add(personalHolidayDate.Date);
                }

                List<EmployeeTimeScheduleTO> timeSchedule = new List<EmployeeTimeScheduleTO>();
                Dictionary<int, List<EmployeeTimeScheduleTO>> emplTimeSchedule = new EmployeesTimeSchedule(Session[Constants.sessionConnection]).SearchEmployeesSchedulesExactDate(employeeID.ToString(), 
                    startDate.Date, endDate.Date, null);

                if (emplTimeSchedule.ContainsKey(employeeID))
                    timeSchedule = emplTimeSchedule[employeeID];

                Dictionary<int, WorkTimeSchemaTO> schemas = new TimeSchema(Session[Constants.sessionConnection]).getDictionary();

                if (timeSchedule.Count > 0)
                {
                    for (int scheduleIndex = 0; scheduleIndex < timeSchedule.Count; scheduleIndex++)
                    {
                        int cycleDuration = 0;
                        int day = timeSchedule[scheduleIndex].StartCycleDay + 1;
                        int schemaID = timeSchedule[scheduleIndex].TimeSchemaID;
                        
                        WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                        if (schemas.ContainsKey(schemaID))
                        {
                            schema = schemas[schemaID];
                            cycleDuration = schema.CycleDuration;
                        }

                        /* 2008-03-14
                         * From now one, take the last existing time schedule, don't expect that every month has 
                         * time schedule*/
                        DateTime i = timeSchedule[scheduleIndex].Date;                        
                        if ((timeSchedule[scheduleIndex].Date.Year < month.Year) || ((timeSchedule[scheduleIndex].Date.Year == month.Year) && (timeSchedule[scheduleIndex].Date.Month < month.Month)))                            
                        {
                            i = startDate.Date;

                            TimeSpan ts = new TimeSpan((new DateTime(month.Year, month.Month, 1)).Date.Ticks - timeSchedule[scheduleIndex].Date.Date.Ticks);
                            day = ((timeSchedule[scheduleIndex].StartCycleDay + (int)ts.TotalDays) % cycleDuration) + 1;
                        }

                        for (; i < ((scheduleIndex + 1 < timeSchedule.Count) ? timeSchedule[scheduleIndex + 1].Date.Date : endDate.AddDays(1).Date); i = i.AddDays(1))
                        {
                            if (!dict.ContainsKey(i.Date))
                                dict.Add(i.Date, new cycleDay());

                            dict[i.Date].Schema = schema;
                            dict[i.Date].Day = day;
                            dict[i.Date].StartDay = day - 1;
                            dict[i.Date].CycleDuration = cycleDuration;
                            if (nationalHolidaysDays.Contains(i.Date))
                                dict[i.Date].Color = Constants.calendarNationalHolidayDayColor;
                            if (schema.Type.Trim() != Constants.schemaTypeIndustrial && nationalHolidaysSundaysDays.Contains(i.Date))
                                dict[i.Date].Color = Constants.calendarNationalHolidayDayColor;
                            if (personalHolidayDays.Contains(i.Date))
                                dict[i.Date].Color = Constants.calendarPersonalHolidayDayColor;
                            if (schema.Days.ContainsKey(day - 1) && schema.Days[day - 1].Keys.Count > 0 && schema.Days[day - 1][0].Description.Length > 0)
                            {
                                dict[i.Date].Description = schema.Days[day - 1][0].Description;
                            }
                            else
                            {
                                dict[i.Date].Description = "";
                            }

                            day = (day == cycleDuration ? 1 : day + 1);
                        }
                    }
                }

                Session[sessionCycleDayList] = dict;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }      

        /// <summary>
        /// Draw calendar for the selected month and year.
        /// </summary>
        /// <param name="month"></param>
        private void setCalendar(DateTime month, DateTime selDayFrom, DateTime selDayTo)
        {
            try
            {
                int emplID = -1;

                Dictionary<DateTime, cycleDay> timeSchemaDict = new Dictionary<DateTime, cycleDay>();
                if (Session[sessionCycleDayList] != null && Session[sessionCycleDayList] is Dictionary<DateTime, cycleDay>)
                {
                    timeSchemaDict = (Dictionary<DateTime, cycleDay>)Session[sessionCycleDayList];
                }

                if (Request.QueryString["emplIDs"] != null)
                {
                    if (!int.TryParse(Request.QueryString["emplIDs"], out emplID))
                        emplID = -1;
                }

                int currentMonth = month.Month;
                ctrlHolder.Controls.Clear();

                DateTime startDate = new DateTime(month.Year, month.Month, 1);
                DateTime endDate = Common.Misc.getWeekEnding(startDate.AddMonths(1).AddDays(-1));

                addHrisontalSpace();
                addSpace();

                int i = 1;
                
                int dayOfWeek = (int)startDate.DayOfWeek;
                if (dayOfWeek == 0)
                    dayOfWeek = 7;
                
                while (i < dayOfWeek)
                {
                    DayOfCalendarUC calDay = new DayOfCalendarUC();
                    calDay.ID = "dayHr" + i.ToString().Trim();
                    calDay.Transprent = true;
                    ctrlHolder.Controls.Add(calDay);
                    addSpace();
                    i++;
                }

                List<DayOfCalendarUC> controls = new List<DayOfCalendarUC>();
                int ctrlCounter = 0;

                DateTime date = startDate.Date;
                while (date.Date <= endDate.Date)
                {
                    DayOfCalendarUC dayOfCalendar = new DayOfCalendarUC();
                    dayOfCalendar.ID = "day" + ctrlCounter.ToString().Trim();
                    dayOfCalendar.Date = date;
                    dayOfCalendar.Transprent = false;
                    if (timeSchemaDict.ContainsKey(date.Date))
                        dayOfCalendar.Description = timeSchemaDict[date.Date].Description;
                    if (dayOfCalendar.Date.DayOfWeek == DayOfWeek.Sunday || dayOfCalendar.Date.DayOfWeek == DayOfWeek.Saturday)
                    {
                        dayOfCalendar.Color = Constants.calendarWeekEndDayColor;
                    }
                    else
                    {
                        dayOfCalendar.Color = Constants.calendarDayColor;
                    }
                    if (date.Date >= selDayFrom.Date && date.Date <= selDayTo.Date)
                        dayOfCalendar.Selected = true;
                    if (timeSchemaDict.ContainsKey(date.Date))
                    {
                        dayOfCalendar.SchemaID = timeSchemaDict[date.Date].Schema.TimeSchemaID;
                        dayOfCalendar.CycleDay = timeSchemaDict[date.Date].Day;
                        if (timeSchemaDict[date.Date].Color.Length > 0)
                            dayOfCalendar.Color = timeSchemaDict[date.Date].Color;
                    }

                    controls.Add(dayOfCalendar);
                    
                    date = date.AddDays(1);
                    ctrlCounter++;
                }

                foreach (DayOfCalendarUC dayOfCalendar in controls)
                {
                    if (dayOfCalendar.Date.DayOfWeek == DayOfWeek.Monday && dayOfCalendar.Date.Day > 1)
                    {
                        addSpace();
                    }
                    ctrlHolder.Controls.Add(dayOfCalendar);
                    addSpace();
                    if (dayOfCalendar.Date.DayOfWeek == DayOfWeek.Sunday)
                    {
                        addHrisontalSpace();
                    }
                }                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void addHrisontalSpace()
        {
            try
            {
                Label space = new Label();
                space.Width = 790;
                space.Height = 10;
                ctrlHolder.Controls.Add(space);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void addSpace()
        {
            try
            {
                Label space = new Label();
                space.Width = 10;
                space.Height = 50;
                ctrlHolder.Controls.Add(space);
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TimeSchemaAssignSingleEmployeePage).Assembly);

                Session[Constants.sessionHeader] = rm.GetString("hdrCycleDay", culture) + "," + rm.GetString("hdrDescription", culture) + "," + rm.GetString("hdrInterval", culture) + "," + rm.GetString("hdrStartTime", culture) + "," + rm.GetString("hdrEndTime", culture);
                Session[Constants.sessionFields] = "cycle_day,description, interval_ord_num AS interval, start_time, end_time";
                Dictionary<int, int> formating = new Dictionary<int, int>();
                formating.Add(3, (int)Constants.FormatTypes.TimeFormat);
                formating.Add(4, (int)Constants.FormatTypes.TimeFormat);
                Session[Constants.sessionFieldsFormating] = formating;
                Session[Constants.sessionFieldsFormatedValues] = null;
                Session[Constants.sessionTables] = "time_schema_dtl";
                Session[Constants.sessionKey] = "cycle_day";
                Session[Constants.sessionColTypes] = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void dateSelection_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime dt = CommonWeb.Misc.createDate("01." + (cbMonths.SelectedIndex + 1) + "." + tbYear.Text);

                Session[sessionCycleDayList] = null;
                Session[sessionChangedDaysList] = null;

                int emplID = -1;

                if (Request.QueryString["emplIDs"] != null)
                {
                    if (!int.TryParse(Request.QueryString["emplIDs"], out emplID))
                        emplID = -1;
                }

                SelectedTo = SelectedFrom = new DateTime();
                tbTo.Text = tbFrom.Text = "";

                if (dt == new DateTime())
                {
                    writeError("mothUnregular");
                }
                else
                {
                    SetTimeShema(emplID, dt);                    
                    setCalendar(dt, SelectedFrom, SelectedTo);
                }

                if (!(Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                        && (((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.WCManager
                        || ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRLegalEntity)))
                {
                    if (!checkCutOffDate(CommonWeb.Misc.createDate("01." + (cbMonths.SelectedIndex + 1) + "." + tbYear.Text)))
                    {
                        btnAssign.Enabled = false;
                        btnSave.Enabled = false;
                        writeError("cutOffDayPessed");
                    }
                    else
                        btnAssign.Enabled = btnSave.Enabled = true;
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TimeSchemaAssignSingleEmployeePage.dateSelection_TextChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TimeSchemaAssignSingleEmployeePage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void getSelDays(DateTime dateTime)
        {
            try
            {
                if (tbFrom.Text == "" || tbTo.Text == "")
                {
                    SelectedTo = SelectedFrom = new DateTime();
                }
                else
                {
                    DateTime startDate = new DateTime(dateTime.Year, dateTime.Month, 1);
                    DateTime endDate = Common.Misc.getWeekEnding(startDate.AddMonths(1).AddDays(-1));

                    DateTime selFrom = CommonWeb.Misc.createDate(tbFrom.Text.Trim());
                    DateTime selTo = CommonWeb.Misc.createDate(tbTo.Text.Trim());

                    if ((!tbFrom.Text.Trim().Equals("") && selFrom.Equals(new DateTime())) || (!tbTo.Text.Trim().Equals("") && selTo.Equals(new DateTime())))
                    {
                        SelectedFrom = SelectedTo = new DateTime();
                        writeError("invalidDateFormat");
                        return;
                    }
                    else
                    {
                        if (selFrom > selTo)
                        {
                            SelectedFrom = SelectedTo = new DateTime();
                            writeError("selectionToLessThanFrom");
                            return;
                        }
                        
                        if (selFrom.Date < startDate.Date || selTo.Date > endDate.Date)
                        {
                            SelectedFrom = SelectedTo = new DateTime();
                            writeError("selectionInsideStartEnd");
                            return;
                        }

                        List<DateTime> lockedDays = getLockedDays(selFrom, selTo);

                        if (lockedDays.Count > 0)
                        {
                            DateTime currDate = selFrom.Date;
                            string locked = "";
                            while (currDate.Date <= selTo.Date)
                            {
                                if (lockedDays.Contains(currDate.Date))
                                    locked += currDate.ToString(Constants.dateFormat);

                                currDate = currDate.AddDays(1);
                            }

                            if (!locked.Trim().Equals(""))
                            {
                                SelectedFrom = SelectedTo = new DateTime();
                                writeError("selectionLockedDays", " " + locked);
                                return;
                            }
                        }

                        // get employee hiring and termination dates
                        EmployeeAsco4TO emplAddData = getEmployeeAddData();
                        DateTime emplHiringDate = emplAddData.DatetimeValue2;
                        DateTime emplTerminationDate = emplAddData.DatetimeValue3;

                        if (!emplHiringDate.Equals(new DateTime()) && emplHiringDate.Date <= selFrom.Date && (emplTerminationDate.Equals(new DateTime()) || emplTerminationDate > selTo.Date))
                        {
                            SelectedFrom = selFrom;
                            SelectedTo = selTo;
                        }
                        else
                        {
                            SelectedFrom = SelectedTo = new DateTime();
                            writeError("selectionNonActiveEmployeesDays");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                btnAssign.Enabled = true;
                btnSave.Enabled = true;
                
                writeError("");

                DateTime month = CommonWeb.Misc.createDate("01." + (cbMonths.SelectedIndex + 1) + "." + tbYear.Text);
                
                getSelDays(month);
                setCalendar(month, SelectedFrom, SelectedTo);

                if (!(Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                        && (((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.WCManager
                        || ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRLegalEntity)))
                {
                    if (!checkCutOffDate(CommonWeb.Misc.createDate("01." + (cbMonths.SelectedIndex + 1) + "." + tbYear.Text)))
                    {
                        btnAssign.Enabled = false;
                        btnSave.Enabled = false;
                        writeError("cutOffDayPessed");
                    }
                    else
                        btnAssign.Enabled = btnSave.Enabled = true;
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TimeSchemaAssignSingleEmployeePage.btnSelect_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TimeSchemaAssignSingleEmployeePage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
        
        private void writeError(string message)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TimeSchemaAssignSingleEmployeePage).Assembly);

                errorLabel.Text = rm.GetString(message, culture);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void writeError(string message, string addMessage)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TimeSchemaAssignSingleEmployeePage).Assembly);

                errorLabel.Text = rm.GetString(message, culture) + addMessage;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "Redirect", "window.history.back();", true);
                Session[sessionCycleDayList] = null;
                Session[sessionChangedDaysList] = new List<DateTime>();

                if (Request.QueryString["Back"] != null && !Request.QueryString["Back"].Trim().Equals(""))
                    Response.Redirect(Request.QueryString["Back"].Trim() + "?reloadState=false", false);

                Message = "";
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TimeSchemaAssignSingleEmployeePage.btnBack_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TimeSchemaAssignSingleEmployeePage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
            ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCSchedulingPage).Assembly);

            lblError.Text = rm.GetString("allCleared", culture);

            
            EmployeeTO Empl = getEmployee();
            string emplID = Empl.EmployeeID.ToString();

            Response.Redirect("/ACTAWeb/ACTAWebUI/TimeSchemaAssignSingleEmployeePage.aspx?emplIDs=" + emplID.Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLTMDataPage.aspx", false);
                    
            
        }

        protected void btnAssign_Click(object sender, EventArgs e)
        {
            try
            {
                List<DateTime> SelectedDates = new List<DateTime>();

                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TimeSchemaAssignSingleEmployeePage).Assembly);
                DateTime dateTime = CommonWeb.Misc.createDate("01." + (cbMonths.SelectedIndex + 1) + "." + tbYear.Text);
                List<string> selKeys = getSelectionValues(SelBox);
                
                int day = -1;
                if (cbTimeSchema.SelectedIndex <= 0)
                {
                    errorLabel.Text = rm.GetString("selectSchema", culture);                    
                }
                else if (selKeys.Count <= 0 || !int.TryParse(selKeys[0], out day))
                {
                    errorLabel.Text = rm.GetString("selectOneDay", culture);
                }
                else
                {
                    if (SelectedTo.Equals(new DateTime()) || SelectedFrom.Equals(new DateTime()))
                    {
                        errorLabel.Text = rm.GetString("selectOneDayCalendar", culture);
                    }
                    else
                    {
                        Dictionary<DateTime, cycleDay> timeSchemaDict = new Dictionary<DateTime, cycleDay>();
                        if (Session[sessionCycleDayList] != null && Session[sessionCycleDayList] is Dictionary<DateTime, cycleDay>)
                        {
                            timeSchemaDict = (Dictionary<DateTime, cycleDay>)Session[sessionCycleDayList];
                        }
                        
                        int newTimeSchema = int.Parse(cbTimeSchema.SelectedValue);
                        List<WorkTimeSchemaTO> schemas = new TimeSchema(Session[Constants.sessionConnection]).Search(newTimeSchema.ToString().Trim());
                        WorkTimeSchemaTO newSchema = new WorkTimeSchemaTO();
                        if (schemas.Count > 0)
                            newSchema = schemas[0];
                        DateTime dt = SelectedTo.Date;
                        if (day >= 0 && newSchema.TimeSchemaID >= 0)
                        {
                            //natalija 11012018
                            if (CheckBox1.Checked)
                            {
                                DateTime month = CommonWeb.Misc.createDate("01." + (cbMonths.SelectedIndex + 1) + "." + tbYear.Text);
                                DateTime startD = new DateTime(month.Year, month.Month, 1);
                                DateTime endD = Common.Misc.getWeekEnding(startD.AddMonths(1).AddDays(-1));
                                SelectedTo = endD;
                                tbTo.Text= endD.ToString("dd.MM.yyyy.");
                                btnSelect_Click(sender, e); 
                                lblMessage.Text = rm.GetString("setShiftFromDateToContinue", culture) + SelectedFrom.ToString("dd.MM.yyyy");
                                                  
                            }
                            //n
                            for (DateTime i = SelectedFrom.Date; i <= SelectedTo.Date; i = i.AddDays(1))
                            {
                                if (!timeSchemaDict.ContainsKey(i.Date))
                                    timeSchemaDict.Add(i.Date, new cycleDay());

                                //NATALIJA
                                int prevSchemaID = timeSchemaDict[i.Date].Schema.TimeSchemaID;
                                int expectedStartDay = timeSchemaDict[i.Date].StartDay -1;
                                //N

                                timeSchemaDict[i.Date].Schema = newSchema;
                                timeSchemaDict[i.Date].Day = day;
                                timeSchemaDict[i.Date].StartDay = day - 1;
                                timeSchemaDict[i.Date].CycleDuration = newSchema.CycleDuration;
                                if (newSchema.Days.ContainsKey(day - 1) && newSchema.Days[day - 1].Keys.Count > 0 && newSchema.Days[day - 1][0].Description.Length > 0)
                                {
                                    timeSchemaDict[i.Date].Description = newSchema.Days[day - 1][0].Description;
                                }

                                day = (day == newSchema.CycleDuration ? 1 : day + 1);

                                

                            }
                        }

                        Session[sessionCycleDayList] = timeSchemaDict;

                        for (DateTime dateSel = SelectedFrom.Date; dateSel <= SelectedTo.Date; dateSel = dateSel.AddDays(1))
                        {
                            
                            if(Session[sessionChangedDaysList] != null && Session[sessionChangedDaysList] is List<DateTime>)
                               SelectedDates = ((List<DateTime>)Session[sessionChangedDaysList]);

                            if (!SelectedDates.Contains(dateSel))
                                SelectedDates.Add(dateSel);

                            Session[sessionChangedDaysList] = SelectedDates;
                        }

                        setCalendar(dateTime, SelectedFrom, SelectedTo);
                    }

                   /* if (CheckBox1.Checked)
                    {
                        lblMessage.Text += rm.GetString("setShiftFromDateToContinue", culture) + SelectedFrom.ToString("dd.MM.yyyy");
                    }
                    else
                    {
                        lblMessage.Text += rm.GetString("setShiftFromDate", culture) + SelectedFrom.ToString("dd.MM.yyyy") + " " + rm.GetString("setShiftToDate", culture) + SelectedTo.ToString("dd.MM.yyyy");
                    }*/
                }

                SelBox.Value = "";

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TimeSchemaAssignSingleEmployeePage.btnAssign_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TimeSchemaAssignSingleEmployeePage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // if system is closed, sign out and redirect to login page
                if (Common.Misc.getClosingEvent(DateTime.Now, Session[Constants.sessionConnection]).EventID != -1)
                    CommonWeb.Misc.LogOut(Session[Constants.sessionLogInUserLog], Session[Constants.sessionLogInUser], Session[Constants.sessionConnection], Session, Response);

                Dictionary<DateTime, cycleDay> dict = new Dictionary<DateTime, cycleDay>();
                if (Session[sessionCycleDayList] != null && Session[sessionCycleDayList] is Dictionary<DateTime, cycleDay>)
                {
                    dict = (Dictionary<DateTime, cycleDay>)Session[sessionCycleDayList];
                }
                
                int employeeID = -1;

                if (Request.QueryString["emplIDs"] != null)
                {
                    if (!int.TryParse(Request.QueryString["emplIDs"], out employeeID))
                        employeeID = -1;
                }

                DateTime startDate = CommonWeb.Misc.createDate("01." + (cbMonths.SelectedIndex + 1) + "." + tbYear.Text);                
                DateTime endDate = Common.Misc.getWeekEnding(startDate.AddMonths(1).AddDays(-1));

                // every day must have schema assigned
                bool assignStatus = true;                
                DateTime minDate = new DateTime();
                DateTime maxDate = new DateTime();
                foreach (DateTime day in dict.Keys)
                {
                    if (dict[day].Schema.TimeSchemaID == -1)
                    {
                        assignStatus = false;
                        writeError("assignEachDay");
                        break;
                    }
                    else
                    {
                        if (minDate.Equals(new DateTime()))
                            minDate = day.Date;
                        else if (day.Date < minDate.Date)
                            minDate = day.Date;
                        if (day.Date > maxDate.Date)
                            maxDate = day.Date;
                    }
                }
                
                List<DateTime> datesList = new List<DateTime>();
                if (Session[sessionChangedDaysList] != null && Session[sessionChangedDaysList] is List<DateTime>)
                {
                    datesList = (List<DateTime>)Session[sessionChangedDaysList];
                }
             
                if (datesList.Count <= 0)
                {
                    assignStatus = false;
                    writeError("noChangesDetected");
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (assignStatus)
                {
                    DateTime startReprocessingDate = startDate.Date;

                    // if first day of month is changing and first day is leaving third shift from previous month, reprocess from second day
                    if (datesList.Contains(startDate.Date) && !checkCutOffDate(startDate.AddDays(-1).Date) && dict.ContainsKey(startDate.Date) && dict[startDate.Date].Schema.Days.ContainsKey(dict[startDate.Date].StartDay))
                    {
                        foreach (int num in dict[startDate.Date].Schema.Days[dict[startDate.Date].StartDay].Keys)
                        {
                            if (dict[startDate.Date].Schema.Days[dict[startDate.Date].StartDay][num].StartTime.TimeOfDay == new TimeSpan(0, 0, 0)
                                && dict[startDate.Date].Schema.Days[dict[startDate.Date].StartDay][num].EndTime.TimeOfDay != new TimeSpan(0, 0, 0))
                            {
                                startReprocessingDate = startReprocessingDate.AddDays(1).Date;
                                datesList.Remove(startDate.Date);
                                break;
                            }
                        }
                    }

                    Dictionary<int, EmployeeTO> emplDict = new Employee(Session[Constants.sessionConnection]).SearchDictionary(employeeID.ToString().Trim());
                    Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4(Session[Constants.sessionConnection]).SearchDictionary(employeeID.ToString().Trim());
                    Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDict = new Common.Rule(Session[Constants.sessionConnection]).SearchWUEmplTypeDictionary();

                    EmployeesTimeSchedule emplTimeSchedule = new EmployeesTimeSchedule(Session[Constants.sessionConnection]);

                    bool transBegin = emplTimeSchedule.BeginTransaction();
                    bool isSaved = true;

                    if (transBegin)
                    {
                        try
                        {
                            #region Reprocess Dates

                            IDbTransaction trans = emplTimeSchedule.GetTransaction();

                            //list of datetime for each employee
                            Dictionary<int, List<DateTime>> emplDateWholeDayList = new Dictionary<int, List<DateTime>>();
                            emplDateWholeDayList.Add(employeeID, datesList);
                            if (datesList.Count > 0)
                            {
                                isSaved = isSaved && Common.Misc.ReprocessPairsAndRecalculateCounters(employeeID.ToString(), startReprocessingDate.Date.Date, endDate.Date, trans, emplDateWholeDayList, Session[Constants.sessionConnection], CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]));
                            }

                            #endregion

                            EmployeeTO employeeTO = (new Employee(Session[Constants.sessionConnection])).Find(employeeID.ToString(), emplTimeSchedule.GetTransaction());
                            bool statuscb = CheckBox1.Checked;

                            /*for (DateTime i = SelectedFrom.Date; i <= SelectedTo.Date; i = i.AddDays(1))
                            {
                                selectedDays.Add(i);
                            }*/

                            //NATALIJA 22112017
                            List<DateTime> selectedDays= new List<DateTime>();

                            if (isSaved)
                            {
                                List<EmployeeTimeScheduleTO> oldEmployeeTimeSchedule = emplTimeSchedule.SearchEmployeesSchedules(employeeID.ToString(), startDate.Date, endDate.Date, emplTimeSchedule.GetTransaction());
                                
                                // delete time schedule for selected month
                                isSaved = isSaved && emplTimeSchedule.DeleteFromToSchedule(employeeID, startDate.Date, endDate.Date, CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]), false, statuscb);
                               
                                //30.11.2017 Miodrag / Sredio proveru prvog dana u mesecu. 
                                WorkTimeSchemaTO actualTimeSchema = null;
                                List<WorkTimeSchemaTO> timeSchemaZ = new List<WorkTimeSchemaTO>();
                                TimeSchema s = new TimeSchema();
                                timeSchemaZ = s.Search();
                                foreach (WorkTimeSchemaTO currentTimeSchema in timeSchemaZ)
                                {
                                    if (currentTimeSchema.TimeSchemaID == oldEmployeeTimeSchedule[0].TimeSchemaID)
                                    {
                                        actualTimeSchema = currentTimeSchema;
                                        break;
                                    }
                                }
                                TimeSpan ts = new TimeSpan(startDate.Date.Ticks - oldEmployeeTimeSchedule[0].Date.Date.Ticks);
                                int dayNum = (oldEmployeeTimeSchedule[0].StartCycleDay + (int)ts.TotalDays) % actualTimeSchema.CycleDuration;

                                //19.09.2017 Miodrag / Postojala je mogucnost da je dayNum negativan.
                                if (++dayNum < 0) //dayNum se povecava za jedan jer se poredi sa cycle dayom koji za razliku od njega krece od 1.
                                {
                                    dayNum = actualTimeSchema.CycleDuration + dayNum;
                                }
                                //mm
                                int prevSchemaID = oldEmployeeTimeSchedule[0].TimeSchemaID;
                                int expectedStartDay = dayNum; // oldEmployeeTimeSchedule[0].StartCycleDay;

                                if (isSaved)
                                {
                                    EmployeeTimeScheduleTO timeSceduleTO = new EmployeeTimeScheduleTO();
                                    List<EmployeeTimeScheduleTO> emplSchedulesList = new List<EmployeeTimeScheduleTO>();

                                    foreach (DateTime date in dict.Keys)
                                    {
                                        
                                       // if (selectedDays.Contains(date))
                                       // {
                                            cycleDay day = dict[date];
                                            // if schema is different, or if schema is the same the same but cycle day is not the expected one
                                            // save new record
                                            if ((day.Schema.TimeSchemaID != prevSchemaID) ||
                                                ((day.Schema.TimeSchemaID == prevSchemaID) && (day.Day != expectedStartDay)))
                                            {
                                                isSaved = (emplTimeSchedule.Save(employeeID, date.Date, day.Schema.TimeSchemaID, day.StartDay, CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]), false) > 0 ? true : false) && isSaved;

                                                timeSceduleTO = new EmployeeTimeScheduleTO(employeeID, date, dict[date].Schema.TimeSchemaID, dict[date].StartDay);
                                                emplSchedulesList.Add(timeSceduleTO);

                                                if (!isSaved)
                                                    break;

                                                prevSchemaID = day.Schema.TimeSchemaID;
                                            }

                                            expectedStartDay = (day.StartDay == day.CycleDuration - 1 ? 0 : day.StartDay + 1) + 1;
                                        //}
                                    }
                                        //NATALIJA 22112017
                                    //if (CheckBox1.Checked) //promena smene za stalno
                                    if(lblMessage.Text!="")
                                    {
                                        if (isSaved)
                                        {
                                            EmployeeGroupsTimeScheduleTO currentTS = new EmployeeGroupsTimeScheduleTO();
                                            currentTS = new EmployeeGroupsTimeSchedule(Session[Constants.sessionConnection]).Find(emplSchedulesList[emplSchedulesList.Count-1].TimeSchemaID, -1, emplTimeSchedule.GetTransaction());
                                            WorkingGroupTO workingGroup = new WorkingGroupTO();
                                            workingGroup = new WorkingGroup(Session[Constants.sessionConnection]).Find(currentTS.EmployeeGroupID, emplTimeSchedule.GetTransaction());
                                            Employee employee = new Employee(Session[Constants.sessionConnection]);
                                            employeeTO.ModifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                                            isSaved = isSaved && employee.Update(employeeTO, workingGroup.EmployeeGroupID, emplTimeSchedule.GetTransaction());
                                        }
                                        //natalija 11012018
                                        /*DateTime month = CommonWeb.Misc.createDate("01." + (cbMonths.SelectedIndex + 1) + "." + tbYear.Text);
                
                                        DateTime startD = new DateTime(month.Year, month.Month, 1);
                                         DateTime endD = Common.Misc.getWeekEnding(startD.AddMonths(1).AddDays(-1));
                                        if (SelectedTo < endD)
                                        {
                                            writeError("Promena smene za stalno. Oznaci datume do kraja!");
                                            break;
                                        }*/
                                    }
                                    
                                    


                                        if (isSaved)
                                        {
                                            //  procesirani parovi koji jesu celodnevna odsustva
                                            IOPairProcessed pairProcessed = new IOPairProcessed(Session[Constants.sessionConnection]);
                                            Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> manualCreated = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();


                                            List<WorkTimeSchemaTO> timeSchemas = new List<WorkTimeSchemaTO>();

                                            WorkTimeSchemaTO wtsTO = new WorkTimeSchemaTO(timeSceduleTO.TimeSchemaID);

                                            timeSchemas = new TimeSchema(Session[Constants.sessionConnection]).SearchTimeSchedule(wtsTO, emplTimeSchedule.GetTransaction());


                                            manualCreated = pairProcessed.getManualCreatedPairsWholeDayAbsence(emplDateWholeDayList, emplTimeSchedule.GetTransaction());

                                            Dictionary<DateTime, List<IOPairProcessedTO>> lista = new Dictionary<DateTime, List<IOPairProcessedTO>>();
                                            List<IOPairProcessedTO> listaProcessed = new List<IOPairProcessedTO>();


                                            foreach (int emplID in manualCreated.Keys)
                                            {
                                                lista = manualCreated[emplID];

                                                foreach (DateTime dt in lista.Keys)
                                                {
                                                    listaProcessed = lista[dt];
                                                    foreach (IOPairProcessedTO processed in listaProcessed)
                                                    {
                                                        bool is2DayShift = false;
                                                        bool is2DaysShiftPrevious = false;
                                                        WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                                                        Dictionary<int, WorkTimeIntervalTO> workTimeInterval = Common.Misc.getDayTimeSchemaIntervals(emplSchedulesList, dt, ref is2DayShift, ref is2DaysShiftPrevious, ref firstIntervalNextDay, timeSchemas);
                                                        Dictionary<int, WorkTimeIntervalTO> workTimeIntervalNextDay = Common.Misc.getDayTimeSchemaIntervals(emplSchedulesList, dt.AddDays(1), ref is2DayShift, ref is2DaysShiftPrevious, ref firstIntervalNextDay, timeSchemas);

                                                        isSaved = isSaved && pairProcessed.UpdateManualCreatedProcessedPairs(processed, workTimeInterval, workTimeIntervalNextDay, is2DayShift, emplTimeSchedule.GetTransaction());
                                                    }
                                                }
                                            }

                                        }
                                        if (isSaved)
                                        {
                                            IOPair ioPair = new IOPair(Session[Constants.sessionConnection]);
                                            if ((startDate.Year < DateTime.Now.Year) ||
                                                ((startDate.Year == DateTime.Now.Year) && (startDate.Month <= DateTime.Now.Month)))
                                            {

                                                if (endDate.Date > DateTime.Now.Date)
                                                    endDate = DateTime.Now.Date;

                                                ioPair.recalculatePause(employeeID.ToString(), startDate.Date, endDate.Date, emplTimeSchedule.GetTransaction(), Session[Constants.sessionConnection]);
                                            }
                                        }                                                                               
                                    
                            
                            /*
                            if (isSaved)
                            {
                                // delete time schedule for selected month
                                isSaved = isSaved && emplTimeSchedule.DeleteFromToSchedule(employeeID, startDate.Date, endDate.Date, CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]), false);

                                int prevSchemaID = -1;
                                int expectedStartDay = -1;

                                if (isSaved)
                                {
                                    foreach (DateTime date in dict.Keys)
                                    {
                                        cycleDay day = dict[date];
                                        // if schema is different, or if schema is the same the same but cycle day is not the expected one
                                        // save new record
                                        if ((day.Schema.TimeSchemaID != prevSchemaID) ||
                                            ((day.Schema.TimeSchemaID == prevSchemaID) && (day.Day != expectedStartDay)))
                                        {
                                            isSaved = (emplTimeSchedule.Save(employeeID, date.Date, day.Schema.TimeSchemaID, day.StartDay, CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]), false) > 0 ? true : false) && isSaved;

                                            if (!isSaved)
                                                break;

                                            prevSchemaID = day.Schema.TimeSchemaID;
                                        }

                                        expectedStartDay = (day.StartDay == day.CycleDuration - 1 ? 0 : day.StartDay + 1) + 1;
                                    }

                                    if (isSaved)
                                    {
                                        // assign employee's working group time schedule for next month
                                        DateTime firstNextDate = endDate.AddDays(1);
                                        List<EmployeeTimeScheduleTO> nextSchedule = emplTimeSchedule.SearchEmployeesNextSchedule(employeeID.ToString(), endDate.Date, emplTimeSchedule.GetTransaction());

                                        bool insertGroupSchedule = false;
                                        if (nextSchedule.Count > 0)
                                        {
                                            DateTime nextScheduleDate = nextSchedule[0].Date.Date;
                                            if (nextScheduleDate.Date > firstNextDate.Date)
                                                insertGroupSchedule = true;
                                        }
                                        else
                                        {
                                            insertGroupSchedule = true;
                                        }

                                        if (insertGroupSchedule)
                                        {
                                            if (employeeTO.WorkingGroupID != -1)
                                            {
                                                List<EmployeeGroupsTimeScheduleTO> timeSchedule = new EmployeeGroupsTimeSchedule(Session[Constants.sessionConnection]).SearchMonthSchedule(employeeTO.WorkingGroupID, firstNextDate, emplTimeSchedule.GetTransaction());

                                                int timeScheduleIndex = -1;
                                                for (int scheduleIndex = 0; scheduleIndex < timeSchedule.Count; scheduleIndex++)
                                                {
                                                    if (firstNextDate.Date >= timeSchedule[scheduleIndex].Date)
                                                    {
                                                        timeScheduleIndex = scheduleIndex;
                                                    }
                                                }

                                                if (timeScheduleIndex >= 0)
                                                {
                                                    EmployeeGroupsTimeScheduleTO egts = timeSchedule[timeScheduleIndex];
                                                    int startDay = egts.StartCycleDay;
                                                    int schemaID = egts.TimeSchemaID;

                                                    if (employeeTO.Status.ToUpper().Trim().Equals(Constants.statusRetired.ToUpper().Trim()))
                                                        schemaID = Constants.defaultSchemaID;

                                                    TimeSchema sch = new TimeSchema(Session[Constants.sessionConnection]);
                                                    sch.TimeSchemaTO.TimeSchemaID = schemaID;
                                                    List<WorkTimeSchemaTO> timeSchemas = sch.Search(emplTimeSchedule.GetTransaction());
                                                    WorkTimeSchemaTO actualTimeSchema = null;

                                                    foreach (WorkTimeSchemaTO currentTimeSchema in timeSchemas)
                                                    {
                                                        if (currentTimeSchema.TimeSchemaID == schemaID)
                                                        {
                                                            actualTimeSchema = currentTimeSchema;
                                                            break;
                                                        }
                                                    }
                                                    if (actualTimeSchema != null)
                                                    {
                                                        int cycleDuration = actualTimeSchema.CycleDuration;

                                                        TimeSpan ts = new TimeSpan(firstNextDate.Date.Ticks - egts.Date.Date.Ticks);
                                                        int dayNum = (startDay + (int)ts.TotalDays) % cycleDuration;

                                                        int insert = emplTimeSchedule.Save(employeeTO.EmployeeID, firstNextDate.Date,
                                                            schemaID, dayNum, CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]), false);

                                                        isSaved = (insert > 0 ? true : false) && isSaved;

                                                        // 04.07.2013. Sanja if group schedule is entered, reprocess days form first next day, until first next schedule (if there is one)
                                                        List<EmployeeTimeScheduleTO> nextMonthSchedule = emplTimeSchedule.SearchEmployeesNextSchedule(employeeID.ToString(), firstNextDate.Date, emplTimeSchedule.GetTransaction());
                                                        if (nextMonthSchedule.Count > 0)
                                                        {
                                                            List<DateTime> additionalDateList = new List<DateTime>();
                                                            for (DateTime currDay = firstNextDate.Date; currDay.Date < nextMonthSchedule[0].Date.Date; currDay = currDay.Date.AddDays(1))
                                                            {
                                                                additionalDateList.Add(currDay.Date);
                                                            }

                                                            if (additionalDateList.Count > 0)
                                                            {
                                                                emplDateWholeDayList = new Dictionary<int, List<DateTime>>();
                                                                emplDateWholeDayList.Add(employeeID, additionalDateList);

                                                                isSaved = isSaved && Common.Misc.ReprocessPairsAndRecalculateCounters(employeeID.ToString(), firstNextDate.Date.Date, nextMonthSchedule[0].Date.Date.AddDays(-1), trans, emplDateWholeDayList, Session[Constants.sessionConnection], CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]));                                                                
                                                            }
                                                        }
                                                    }
                                                } //if (timeScheduleIndex >= 0)
                                            } //if (employeeTO.WorkingGroupID != -1)
                                        } //if (insertGroupSchedule)

                                        if (isSaved)
                                        {
                                            IOPair ioPair = new IOPair(Session[Constants.sessionConnection]);
                                            if ((startDate.Year < DateTime.Now.Year) ||
                                                ((startDate.Year == DateTime.Now.Year) && (startDate.Month <= DateTime.Now.Month)))
                                            {
                                               // * 2008-03-14
                                               //  * From now one, take the last existing time schedule, don't expect that every month has 
                                               //  * time schedule*
                                                //DateTime toDate = (new DateTime(month.AddMonths(1).Year, month.AddMonths(1).Month, 1)).AddDays(-1);
                                                //ArrayList nextSchedule = emplTimeSchedule.SearchEmployeesNextSchedule(tbEmployeeID.Text, toDate);

                                            //    if (nextSchedule.Count > 0)
                                            //    {
                                             //       toDate = ((EmployeesTimeSchedule)nextSchedule[0]).Date.AddDays(-1).Date;
                                             //       if (toDate.Date > DateTime.Now.Date)
                                              //          toDate = DateTime.Now.Date;
                                              //  }
                                              //  else
                                              //  {
                                              //      toDate = DateTime.Now.Date;
                                               // }
                                                if (endDate.Date > DateTime.Now.Date)
                                                    endDate = DateTime.Now.Date;

                                                //ioPair.recalculatePause(tbEmployeeID.Text, new DateTime(month.Year, month.Month, 1), (new DateTime(month.AddMonths(1).Year, month.AddMonths(1).Month, 1)).AddDays(-1));
                                                ioPair.recalculatePause(employeeID.ToString(), startDate.Date, endDate.Date, emplTimeSchedule.GetTransaction(), Session[Constants.sessionConnection]);
                                            }
                                        }
                                    }*/
                                }
                            }

                            if (isSaved)
                            {
                                // validate new employee schedule
                                bool validFundHrs = true;
                                //DateTime invalidDate = Common.Misc.isValidTimeSchedule(employeeTO.EmployeeID.ToString().Trim(), minDate.Date, maxDate.Date, emplTimeSchedule.GetTransaction(), 
                                //    Session[Constants.sessionConnection], CommonWeb.Misc.checkLimit(Session[Constants.sessionLoginCategory]), ref validFundHrs);
                                DateTime invalidDate = Common.Misc.isValidTimeSchedule(emplDict, ascoDict, rulesDict, employeeTO.EmployeeID.ToString().Trim(), 
                                    minDate.Date, maxDate.Date, emplTimeSchedule.GetTransaction(), Session[Constants.sessionConnection], false, ref validFundHrs, chbCheckLaborLaw.Checked);
                                
                                //NATALIJA 11012018
                                /*if (CheckBox1.Checked)
                                {
                                    DateTime month = CommonWeb.Misc.createDate("01." + (cbMonths.SelectedIndex + 1) + "." + tbYear.Text);
                                    DateTime startD = new DateTime(month.Year, month.Month, 1);
                                    DateTime endD = Common.Misc.getWeekEnding(startD.AddMonths(1).AddDays(-1));
                                     if (SelectedTo < endD)
                                     {
                                         emplTimeSchedule.RollbackTransaction();
                                         writeError("Promena smene za stalno. Oznaci datume do kraja!");
                                     }
                                     else if(SelectedTo == endD)
                                     {*/
                                    if (invalidDate.Equals(new DateTime()))
                                    {
                                        emplTimeSchedule.CommitTransaction();
                                        writeError("emplScheduleSaved");

                                    }
                                    else
                                    {
                                        emplTimeSchedule.RollbackTransaction();
                                        if (validFundHrs)
                                        {
                                            writeError("notValidScheduleAssigned", " " + invalidDate.Date.AddDays(-1).ToString(Constants.dateFormat) + "/" + invalidDate.Date.ToString(Constants.dateFormat));
                                            lblMessage.Text = "";//N
                                        }
                                        else
                                        {
                                            writeError("notValidFundHrs", " " + invalidDate.Date.ToString(Constants.dateFormat) + "-" + invalidDate.AddDays(6).Date.ToString(Constants.dateFormat));
                                            lblMessage.Text = "";//N
                                        }/*  }
                                      }
                                  }
                                  else
                                  {
                                      //n
                                      if (invalidDate.Equals(new DateTime()))
                                      {
                                          emplTimeSchedule.CommitTransaction();
                                          writeError("emplScheduleSaved");
                                      }
                                      else
                                      {
                                          emplTimeSchedule.RollbackTransaction();
                                          if (validFundHrs)
                                              writeError("notValidScheduleAssigned", " " + invalidDate.Date.AddDays(-1).ToString(Constants.dateFormat) + "/" + invalidDate.Date.ToString(Constants.dateFormat));
                                          else
                                              writeError("notValidFundHrs", " " + invalidDate.Date.ToString(Constants.dateFormat) + "-" + invalidDate.AddDays(6).Date.ToString(Constants.dateFormat));
                                      }*/
                                    }
                                
                            }
                            else
                            {
                                emplTimeSchedule.RollbackTransaction();
                                writeError("emplScheduleNotSaved");
                            }
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                if (emplTimeSchedule.GetTransaction() != null)
                                    emplTimeSchedule.RollbackTransaction();

                                Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TimeSchemaAssignSingleEmployeePage.btnSave_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TimeSchemaAssignSingleEmployeePage.aspx&Header=" + Constants.falseValue.Trim(), false);
                            }
                            catch (System.Threading.ThreadAbortException) { }
                        }
                    }
                }
        

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TimeSchemaAssignSingleEmployeePage.btnSave_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TimeSchemaAssignSingleEmployeePage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void cbTimeSchema_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Session[Constants.sessionFilter] = null;
                if (cbTimeSchema.SelectedIndex > 0)
                    Session[Constants.sessionFilter] = "time_schema_id = " + cbTimeSchema.SelectedValue;
                Session[Constants.sessionSortCol] = "cycle_day";
                Session[Constants.sessionSortDir] = Constants.sortASC;

                resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/SmallResultPage.aspx?showSelection=false";

                SelBox.Value = "";

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TimeSchemaAssignSingleEmployeePage.cbTimeSchema_SelectedIndexChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TimeSchemaAssignSingleEmployeePage.aspx", false);
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

                Session[sessionChangedDaysList] = null;
                Session[sessionCycleDayList] = null;

                Session[Constants.sessionItemsColors] = null;                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<DateTime> getLockedDays(DateTime startDate, DateTime endDate)
        {
            try
            {
                List<DateTime> lockedDays = new List<DateTime>();

                if (!(Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO &&
                    ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRSSC))
                {
                    int emplID = -1;
                    if (Request.QueryString["emplIDs"] != null)
                    {
                        if (int.TryParse(Request.QueryString["emplIDs"], out emplID))
                        {
                            Dictionary<int, List<DateTime>> lockedDaysDict = new EmployeeLockedDay(Session[Constants.sessionConnection]).SearchLockedDays(emplID.ToString().Trim(), "", startDate.Date, endDate.Date);

                            if (lockedDaysDict.ContainsKey(emplID))
                                lockedDays = lockedDaysDict[emplID];
                        }
                    }
                }

                return lockedDays;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private class cycleDay
        {
            private WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
            private int day = -1;
            private string color = "";
            private int startDay = -1;
            private int cycleDuration = -1;
            private string description = "";

            public string Description
            {
                get { return description; }
                set { description = value; }
            }

            public int CycleDuration
            {
                get { return cycleDuration; }
                set { cycleDuration = value; }
            }

            public int StartDay
            {
                get { return startDay; }
                set { startDay = value; }
            }

            public string Color
            {
                get { return color; }
                set { color = value; }
            }

            public int Day
            {
                get { return day; }
                set { day = value; }
            }

            public WorkTimeSchemaTO Schema
            {
                get { return schema; }
                set { schema = value; }
            }
        }
    }
}
