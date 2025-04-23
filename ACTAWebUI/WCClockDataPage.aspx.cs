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

using Common;
using TransferObjects;
using Util;

namespace ACTAWebUI
{
    public partial class WCClockDataPage : System.Web.UI.Page
    {
        const string pageName = "WCClockDataPage";

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
                    btnFromDate.Attributes.Add("onclick", "return calendarPicker('tbFromDate', 'false');");
                    btnToDate.Attributes.Add("onclick", "return calendarPicker('tbToDate', 'false');");
                    chbGate.Attributes.Add("onclick", "return selectListItems('chbGate', 'lbGates');");
                    btnShow.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnReport.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");

                    btnFromDate.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnFromDate.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnToDate.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnToDate.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnPrevDayPeriod.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnPrevDayPeriod.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnNextDayPeriod.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnNextDayPeriod.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");

                    chbGate.Visible = false;

                    setLanguage();

                    populateGates();

                    if (Session[Constants.sessionFromDate] != null && Session[Constants.sessionFromDate] is DateTime)
                        tbFromDate.Text = ((DateTime)Session[Constants.sessionFromDate]).ToString(Constants.dateFormat.Trim());
                    else
                        tbFromDate.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());

                    if (Session[Constants.sessionToDate] != null && Session[Constants.sessionToDate] is DateTime)
                        tbToDate.Text = ((DateTime)Session[Constants.sessionToDate]).ToString(Constants.dateFormat.Trim());
                    else
                        tbToDate.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());

                    tbFromTime.Text = Constants.dayStartTime.Trim();
                    tbToTime.Text = Constants.dayEndTime.Trim();

                    lblError.Text = "";

                    InitializeData();

                    // put sql parameters for result page in session
                    InitializeSQLParameters();

                    if (Request.QueryString["reloadState"] != null && Request.QueryString["reloadState"].Trim().ToUpper().Equals(Constants.falseValue.Trim().ToUpper()))
                        ClearSessionValues();
                    else // reload selected filter state                        
                        LoadState();

                    //resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?showSelection=false";
                    btnShow_Click(this, new EventArgs());
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCClockDataPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCClockDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnPrevDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCClockDataPage).Assembly);

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

                //btnShow_Click(this, new EventArgs());

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCClockDataPage.btnPrevDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCClockDataPage.aspx&Header=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnNextDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCClockDataPage).Assembly);

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

                //btnShow_Click(this, new EventArgs());

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCClockDataPage.btnNextDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCClockDataPage.aspx&Header=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCClockDataPage).Assembly);

                lblEmplData.Text = rm.GetString("lblEmplData", culture);
                lblFirstName.Text = rm.GetString("lblFirstName", culture);
                lblLastName.Text = rm.GetString("lblLastName", culture);
                lblStringone.Text = rm.GetString("lblEmployeeType", culture);
                lblPlant.Text = rm.GetString("lblPlant", culture);
                lblCostCentar.Text = rm.GetString("lblCostCentar", culture);
                lblWorkgroup.Text = rm.GetString("lblWorkgroup", culture);
                lblUTE.Text = rm.GetString("lblUTE", culture);
                lblBranch.Text = rm.GetString("lblBranch", culture);
                lblOUnit.Text = rm.GetString("lblOUnit", culture);
                lblWUnit.Text = rm.GetString("lblWUnit", culture);
                lblPeriod.Text = rm.GetString("lblPeriod", culture);
                lblGate.Text = rm.GetString("lblGate", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);

                btnShow.Text = rm.GetString("btnShow", culture);
                btnReport.Text = rm.GetString("btnReport", culture);

                chbGate.Text = rm.GetString("lblSelectAll", culture);
                lblPayroll.Text = rm.GetString("lblPayrollID", culture);
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
        private void InitializeData()
        {
            try
            {
                EmployeeTO Empl = getEmployee();

                if (Empl.EmployeeID != -1)
                {
                    CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                    ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCClockDataPage).Assembly);

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
                        separatorLbl.Width = new Unit(105);
                        separatorLbl.Height = new Unit(5);
                        separatorLbl.CssClass = "contentLblLeft";
                        ascoCtrlHolder.Controls.Add(separatorLbl);

                        // make asco name label
                        Label ascoNameLbl = new Label();
                        ascoNameLbl.ID = "ascoNameLbl" + counter.ToString().Trim();
                        ascoNameLbl.Width = new Unit(105);
                        ascoNameLbl.Text = ascoMetadata[col].Trim() + ":";
                        ascoNameLbl.CssClass = "contentLblLeft";
                        ascoCtrlHolder.Controls.Add(ascoNameLbl);

                        // make pass type counter label
                        TextBox ascoTb = new TextBox();
                        ascoTb.ID = "ascoLbl" + counter.ToString().Trim();
                        ascoTb.Width = new Unit(105);
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

        private void populateGates()
        {
            try
            {
                List<GateTO> gateList = new Gate(Session[Constants.sessionConnection]).Search();

                lbGates.DataSource = gateList;
                lbGates.DataTextField = "Name";
                lbGates.DataValueField = "GateID";
                lbGates.DataBind();
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

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "WCClockDataPage.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "WCClockDataPage.", filterState);
                }
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
                // every session value is set to default value of corresponding type                
                if (Session[Constants.sessionFilter] != null)
                    Session[Constants.sessionFilter] = "";
                if (Session[Constants.sessionSortCol] != null)
                    Session[Constants.sessionSortCol] = "";
                if (Session[Constants.sessionSortDir] != null)
                    Session[Constants.sessionSortDir] = "";
                if (Session[Constants.sessionResultCurrentPage] != null)
                    Session[Constants.sessionResultCurrentPage] = null;
                Session[Constants.sessionItemsColors] = null;                
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCClockDataPage).Assembly);

                Session[Constants.sessionHeader] = "ID," + rm.GetString("hdrEventTime", culture) + "," + rm.GetString("hdrDirection", culture) + "," + rm.GetString("hdrLocation", culture)
                    + "," + rm.GetString("hdrGate", culture) + "," + rm.GetString("hdrProcessed", culture);
                Session[Constants.sessionFields] = "ps.pass_id AS id, ps.event_time AS event_time, ps.direction AS direction, loc.name AS loc_name, g.name AS g_name, ps.pair_gen_used AS processed";
                Dictionary<int, int> formating = new Dictionary<int, int>();
                formating.Add(1, (int)Constants.FormatTypes.DateTimeFormat);
                Session[Constants.sessionFieldsFormating] = formating;
                Dictionary<int, Dictionary<string, string>> values = new Dictionary<int, Dictionary<string, string>>();
                Dictionary<string, string> formatValues = new Dictionary<string, string>();
                formatValues.Add("0", rm.GetString("no", culture));
                formatValues.Add("1", rm.GetString("yes", culture));
                values.Add(5, formatValues);
                Session[Constants.sessionFieldsFormatedValues] = values;
                Session[Constants.sessionTables] = "passes ps, locations loc, gates g";
                Session[Constants.sessionKey] = "id";
                Session[Constants.sessionDataTableList] = null;
                Session[Constants.sessionDataTableColumns] = null;
                Session[Constants.sessionColTypes] = null;
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCClockDataPage).Assembly);

                ClearSessionValues();
                CultureInfo ci = CultureInfo.InvariantCulture;

                lblError.Text = "";

                if (tbFromDate.Text.Trim().Equals("") || tbToDate.Text.Trim().Equals("") || tbFromTime.Text.Trim().Equals("") || tbToTime.Text.Trim().Equals(""))
                {
                    lblError.Text = rm.GetString("noDateInterval", culture);

                    if (tbFromDate.Text.Trim().Equals(""))
                        tbFromDate.Focus();
                    else if (tbFromTime.Text.Trim().Equals(""))
                        tbFromTime.Focus();
                    else if (tbToDate.Text.Trim().Equals(""))
                        tbToDate.Focus();
                    else if (tbToTime.Text.Trim().Equals(""))
                        tbToTime.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                DateTime fromDate = CommonWeb.Misc.createDate(tbFromDate.Text.Trim());
                DateTime toDate = CommonWeb.Misc.createDate(tbToDate.Text.Trim());
                DateTime fromTime = CommonWeb.Misc.createTime(tbFromTime.Text.Trim());
                DateTime toTime = CommonWeb.Misc.createTime(tbToTime.Text.Trim());
                DateTime from = new DateTime();
                DateTime to = new DateTime();

                string filter = "ps.employee_id = '" + getEmployee().EmployeeID.ToString().Trim() + "' AND ";

                if (fromDate.Equals(new DateTime()) || fromTime.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidFromDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (toDate.Equals(new DateTime()) || toTime.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidToDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                from = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, fromTime.Hour, fromTime.Minute, 0);
                to = new DateTime(toDate.Year, toDate.Month, toDate.Day, toTime.Hour, toTime.Minute, 0);

                if (from > to)
                {
                    lblError.Text = rm.GetString("invalidFromToDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                Session[Constants.sessionFromDate] = fromDate;
                Session[Constants.sessionToDate] = toDate;

                if (Session[Constants.sessionDataProvider].ToString().Trim().ToLower().Equals(Constants.dpSqlServer.Trim().ToLower()))
                    filter += "ps.event_time >= '" + from.ToString(CommonWeb.Misc.getDateTimeFormatUniversal(), ci) + "' AND ";
                else if (Session[Constants.sessionDataProvider].ToString().Trim().ToLower().Equals(Constants.dpMySql.Trim().ToLower()))
                    filter += "DATE_FORMAT(ps.event_time,'%Y-%m-%dT%h:%m:%s') >= '" + from.ToString(CommonWeb.Misc.getDateTimeFormatUniversal(), ci).Trim() + "' AND ";

                if (Session[Constants.sessionDataProvider].ToString().Trim().ToLower().Equals(Constants.dpSqlServer.Trim().ToLower()))
                    filter += "ps.event_time <= '" + to.ToString(CommonWeb.Misc.getDateTimeFormatUniversal(), ci) + "' AND ";
                else if (Session[Constants.sessionDataProvider].ToString().Trim().ToLower().Equals(Constants.dpMySql.Trim().ToLower()))
                    filter += "DATE_FORMAT(ps.event_time,'%Y-%m-%dT%h:%m:%s') <= '" + to.ToString(CommonWeb.Misc.getDateTimeFormatUniversal(), ci).Trim() + "' AND ";

                int[] selGateIndexes = lbGates.GetSelectedIndices();
                if (selGateIndexes.Length > 0)
                {
                    string gates = "";
                    foreach (int index in selGateIndexes)
                    {
                        gates += lbGates.Items[index].Value.Trim() + ",";
                    }

                    if (gates.Length > 0)
                        gates = gates.Substring(0, gates.Length - 1);

                    filter += "ps.gate_id IN (" + gates.Trim() + ") AND ";
                }

                filter += "loc.location_id = ps.location_id AND ps.gate_id = g.gate_id";

                // put sql statement required data in session
                Session[Constants.sessionFilter] = filter;
                Session[Constants.sessionSortCol] = "ps.event_time";
                Session[Constants.sessionSortDir] = Constants.sortASC;

                // save selected filter state
                SaveState();

                resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?showSelection=false";

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCClockDataPage.btnShow_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCClockDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnReport_Click(Object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCClockDataPage).Assembly);

                lblError.Text = "";
                int rowCount = 0;
                DataTable passes = null;

                if (Session[Constants.sessionFields] != null && !Session[Constants.sessionFields].ToString().Trim().Equals("")
                    && Session[Constants.sessionTables] != null && !Session[Constants.sessionTables].ToString().Trim().Equals("")
                    && Session[Constants.sessionFilter] != null && !Session[Constants.sessionFilter].ToString().Trim().Equals("")
                    && Session[Constants.sessionSortCol] != null && !Session[Constants.sessionSortCol].ToString().Trim().Equals("")
                    && Session[Constants.sessionSortDir] != null && !Session[Constants.sessionSortDir].ToString().Trim().Equals(""))
                {
                    Result result = new Result(Session[Constants.sessionConnection]);
                    rowCount = result.SearchResultCount(Session[Constants.sessionTables].ToString().Trim(), Session[Constants.sessionFilter].ToString().Trim());

                    if (rowCount > 0)
                    {
                        // get all passes for search criteria for report
                        passes = new Result(Session[Constants.sessionConnection]).SearchResultTable(Session[Constants.sessionFields].ToString().Trim(), Session[Constants.sessionTables].ToString().Trim(),
                            Session[Constants.sessionFilter].ToString().Trim(), Session[Constants.sessionSortCol].ToString().Trim(), Session[Constants.sessionSortDir].ToString().Trim(), 1, rowCount);

                        // Table Definition for Crystal Reports
                        DataSet dataSetCR = new DataSet();
                        DataTable tableCR = new DataTable("employee_passes");
                        DataTable tableI = new DataTable("images");

                        tableCR.Columns.Add("event_time", typeof(System.DateTime));
                        tableCR.Columns.Add("pass_id", typeof(int));
                        tableCR.Columns.Add("imageID", typeof(byte));
                        tableCR.Columns.Add("direction", typeof(System.String));
                        tableCR.Columns.Add("location", typeof(System.String));
                        tableCR.Columns.Add("gate", typeof(System.String));
                        tableCR.Columns.Add("processed", typeof(System.String));

                        tableI.Columns.Add("imageID", typeof(byte));
                        tableI.Columns.Add("image", typeof(System.Byte[]));

                        //add logo image just once
                        DataRow rowI = tableI.NewRow();
                        rowI["image"] = Constants.LogoForReport;
                        rowI["imageID"] = 1;
                        tableI.Rows.Add(rowI);
                        tableI.AcceptChanges();

                        dataSetCR.Tables.Add(tableCR);
                        dataSetCR.Tables.Add(tableI);

                        foreach (DataRow pass in passes.Rows)
                        {
                            DataRow row = tableCR.NewRow();

                            if (pass["id"] != DBNull.Value)
                                row["pass_id"] = int.Parse(pass["id"].ToString().Trim());
                            if (pass["direction"] != DBNull.Value)
                                row["direction"] = pass["direction"].ToString().Trim();
                            if (pass["event_time"] != DBNull.Value)
                                row["event_time"] = pass["event_time"].ToString().Trim();
                            if (pass["loc_name"] != DBNull.Value)
                                row["location"] = pass["loc_name"].ToString().Trim();
                            if (pass["g_name"] != DBNull.Value)
                                row["gate"] = pass["g_name"].ToString().Trim();

                            if (pass["processed"] != DBNull.Value)
                            {
                                if (pass["processed"].ToString().Trim().Equals(((int)Constants.PairGenUsed.Used).ToString()))
                                {
                                    row["processed"] = rm.GetString("yes", culture);
                                }
                                else
                                {
                                    row["processed"] = rm.GetString("no", culture);
                                }
                            }
                            else
                                row["processed"] = "";

                            row["imageID"] = 1;

                            tableCR.Rows.Add(row);
                            tableCR.AcceptChanges();
                        }

                        if (tableCR.Rows.Count == 0)
                        {
                            lblError.Text = rm.GetString("noReportData", culture);
                            writeLog(DateTime.Now, false);
                            return;
                        }

                        string gate = "*";
                        string start_time = "*";
                        string end_time = "*";

                        if (lbGates.SelectedIndex > 0)
                        {
                            for (int intGates = 0; intGates < lbGates.Items.Count; intGates++)
                            {
                                if (lbGates.Items[intGates].Selected)
                                {
                                    gate = gate + ", " + lbGates.Items[intGates].ToString();
                                }
                            }
                            gate = gate.Substring(gate.IndexOf(',') + 1);
                        }
                        if (gate == "")
                        {
                            for (int intGates = 0; intGates < lbGates.Items.Count; intGates++)
                            {
                                gate = gate + ", " + lbGates.Items[intGates].ToString();
                            }
                            gate = gate.Substring(gate.IndexOf(',') + 1);
                        }
                        // gate = lbGates.SelectedItem.Text;
                        if (tbFromDate.Text != "")
                            start_time = tbFromDate.Text;
                        if (tbToDate.Text != "")
                            end_time = tbToDate.Text;

                        Session["WCClockDataPage.gate"] = gate;
                        Session["WCClockDataPage.start_time"] = start_time;
                        Session["WCClockDataPage.end_time"] = end_time;
                        Session["WCClockDataPage.passesDS"] = dataSetCR;

                        Session[Constants.sessionReportName] = rm.GetString("lblPassesReport", culture);
                        string reportURL = "";
                        if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                            reportURL = "/ACTAWeb/ReportsWeb/sr/WCClockDataReport_sr.aspx";
                        else
                            reportURL = "/ACTAWeb/ReportsWeb/en/WCClockDataReport_en.aspx";
                        Response.Redirect("/ACTAWeb/ReportsWeb/ReportPage.aspx?Back=/ACTAWeb/ACTAWebUI/WCClockDataPage.aspx&Report=" + reportURL.Trim() + "&emplID=" + getEmployee().EmployeeID, false);
                    }
                    else
                    {
                        lblError.Text = rm.GetString("noReportData", culture);
                    }
                }
                else
                {
                    lblError.Text = rm.GetString("noReportData", culture);
                }

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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCClockDataPage.btnReport_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCClockDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private EmployeeTO getEmployee()
        {
            try
            {
                EmployeeTO emplTO = new EmployeeTO();
                int emplID = -1;

                if (Request.QueryString["emplID"] != null)
                {
                    if (!int.TryParse(Request.QueryString["emplID"], out emplID))
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
    }
}
