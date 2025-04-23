using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Resources;
using System.Data;
using System.Configuration;

using Util;
using Common;
using TransferObjects;

namespace ACTAWebUI
{
    public partial class WCDetailsPage : System.Web.UI.Page
    {
        const string pageName = "WCDetailsPage";

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
            EmployeeTO Empl = new EmployeeTO();
            try
            {

                if (!IsPostBack)
                {
                    Empl = getEmployee();
                    btnFromDate.Attributes.Add("onclick", "return calendarPicker('tbFromDate', 'false');");
                    btnToDate.Attributes.Add("onclick", "return calendarPicker('tbToDate', 'false');");
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

                    //tbFromDate.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());
                    //tbToDate.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());
                    //tbFromTime.Text = Constants.dayStartTime.Trim();
                    //tbToTime.Text = Constants.dayEndTime.Trim();

                    setLanguage();

                    InitializeData(Empl);
                    getTypes(Empl);
                    InitializeSQLParameters();

                    if (Request.QueryString["reloadState"] != null && Request.QueryString["reloadState"].Trim().ToUpper().Equals(Constants.falseValue.Trim().ToUpper()))
                    {
                        ClearSessionValues();
                        
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

                    }
                    else // reload selected filter state
                    {
                        LoadState();
                    }

                    //resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?showSelection=false";

                    btnShow_Click(this, new EventArgs());
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCDetailsPage.btnShow_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCDetailsPage.aspx?emplID=" + Empl.EmployeeID, false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnPrevDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCDetailsPage).Assembly);

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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCDetailsPage.btnPrevDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCDetailsPage.aspx&Header=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnNextDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCDetailsPage).Assembly);

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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCDetailsPage.btnNextDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCDetailsPage.aspx&Header=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCDetailsPage).Assembly);

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
                lblPeriod.Text = rm.GetString("lblMonth", culture);
                btnReport.Text = rm.GetString("btnReport", culture);
                btnShow.Text = rm.GetString("btnShow", culture);
                lblPeriod.Text = rm.GetString("lblPeriod", culture);
                lblPassTypes.Text = rm.GetString("lblPassType", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblPayroll.Text = rm.GetString("lblPayrollID", culture);

                cbMontlyTotals.Text = rm.GetString("lblMontlyTotals", culture);

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

        protected void InitializeData(EmployeeTO Empl)
        {

            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCDetailsPage).Assembly);

                if (Empl.EmployeeID != -1)
                {
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

        protected void btnReport_Click(Object sender, EventArgs e)
        {
            EmployeeTO Empl = new EmployeeTO();
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCDetailsPage).Assembly);
                Empl = getEmployee();

                lblError.Text = "";
                int rowCount = 0;
                DataTable ioPairs = null;
                if (cbMontlyTotals.Checked)
                {
                    Session["TLDetailedDataPage.checked"] = 1;
                }
                else
                {
                    Session["TLDetailedDataPage.checked"] = 0;
                }

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
                        ioPairs = new Result(Session[Constants.sessionConnection]).SearchResultTable(Session[Constants.sessionFields].ToString().Trim(), Session[Constants.sessionTables].ToString().Trim(), Session[Constants.sessionFilter].ToString().Trim(),
                        Session[Constants.sessionSortCol].ToString().Trim(), Session[Constants.sessionSortDir].ToString().Trim(), 1, rowCount);

                        // Table Definition for  Reports
                        DataSet dataSetCR = new DataSet();
                        DataTable tableCR = new DataTable("io_pairs");
                        DataTable tableI = new DataTable("images");


                        tableCR.Columns.Add("first_name", typeof(System.String));
                        tableCR.Columns.Add("last_name", typeof(System.String));
                        tableCR.Columns.Add("date", typeof(System.String));
                        tableCR.Columns.Add("start_time", typeof(System.TimeSpan));
                        tableCR.Columns.Add("end_time", typeof(System.TimeSpan));
                        tableCR.Columns.Add("total", typeof(System.Double));
                        tableCR.Columns.Add("pass_type", typeof(System.String));
                        tableCR.Columns.Add("description", typeof(System.String));
                        tableCR.Columns.Add("imageID", typeof(byte));
                        tableCR.Columns.Add("month", typeof(System.DateTime));
                        tableCR.Columns.Add("changedMonth", typeof(System.String));

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

                        int monthIndex = 1;
                        List<string> listMonth = new List<string>();

                        // populate dataset
                        foreach (DataRow ioPair in ioPairs.Rows)
                        {

                            DataRow row = tableCR.NewRow();

                            if (ioPair["date"] != DBNull.Value)
                                row["date"] = ioPair["date"].ToString().Trim() + ".";
                            if (ioPair["first_name"] != DBNull.Value)
                                row["first_name"] = ioPair["first_name"].ToString().Trim();
                            if (ioPair["start_time"] != DBNull.Value)
                                row["start_time"] = ioPair["start_time"].ToString().Trim();
                            if (ioPair["end_time"] != DBNull.Value)
                                row["end_time"] = ioPair["end_time"].ToString().Trim();
                            if (ioPair["total"] != DBNull.Value)
                            {
                                string hours = ioPair["total"].ToString().Remove(ioPair["total"].ToString().IndexOf(':'));
                                string minutes = ioPair["total"].ToString().Substring(ioPair["total"].ToString().IndexOf(':') + 1);
                                minutes = minutes.Remove(minutes.IndexOf(':'));

                                decimal minute = (decimal)(int.Parse(minutes)) / (decimal)60;
                                minutes = minute.ToString();
                                if (minutes.Contains('.'))
                                    minutes = minutes.Substring(minutes.IndexOf('.') + 1);
                                if (minutes.Length > 2)
                                    minutes = minutes.Remove(2);
                                if (hours[0] == '0')
                                {
                                    hours = hours.Substring(1);
                                }
                                if (minutes == "00")
                                {
                                    row["total"] = hours;
                                }
                                else if (minutes.EndsWith("0"))
                                {
                                    minutes = minutes.Remove(minutes.LastIndexOf('0'));
                                    row["total"] = hours + "." + minutes;
                                }
                                else
                                {
                                    row["total"] = hours + "." + minutes;
                                }
                            }
                            if (ioPair["pass_type"] != DBNull.Value)
                                row["pass_type"] = ioPair["pass_type"].ToString().Trim();
                            if (ioPair["description"] != DBNull.Value)
                                row["description"] = ioPair["description"].ToString().Trim();
                            DateTime dtime = CommonWeb.Misc.createDate(row["date"].ToString());
                            row["month"] = new DateTime(dtime.Year, dtime.Month, 1);

                            if (!listMonth.Contains(dtime.Month + "." + dtime.Year))
                                listMonth.Add(dtime.Month + "." + dtime.Year);

                            if (ioPairs.Rows.Count - 1 > monthIndex - 1)
                            {
                                DateTime date = CommonWeb.Misc.createDate(ioPairs.Rows[monthIndex]["date"].ToString() + ".");

                                if (dtime.Month != date.Month)
                                {
                                    row["changedMonth"] = "1";
                                }
                                else
                                {
                                    row["changedMonth"] = "0";
                                }
                                if (ioPairs.Rows[0] == ioPair)
                                {
                                    //    previous = dtime.Month;
                                    if (ioPairs.Rows.Count == 1)
                                        row["changedMonth"] = "1";
                                    else
                                        row["changedMonth"] = "0";
                                }
                                monthIndex++;
                            }
                            else if (ioPairs.Rows.Count == monthIndex)
                            {
                                row["changedMonth"] = "1";
                            }

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

                        string pass_type = "*";
                        string start_time = "";
                        string end_time = "";
                        string empolyee = "*";

                        //gate parameter for report
                        if (lbPassTypes.SelectedIndex > 0)
                        {
                            for (int intGates = 0; intGates < lbPassTypes.Items.Count; intGates++)
                            {
                                if (lbPassTypes.Items[intGates].Selected)
                                {
                                    pass_type = pass_type + ", " + lbPassTypes.Items[intGates].ToString();
                                }
                            }
                            pass_type = pass_type.Substring(pass_type.IndexOf(',') + 1);
                        }
                        if (pass_type == "")
                        {
                            for (int intGates = 0; intGates < lbPassTypes.Items.Count; intGates++)
                            {
                                pass_type = pass_type + ", " + lbPassTypes.Items[intGates].ToString();
                            }
                            pass_type = pass_type.Substring(pass_type.IndexOf(',') + 1);
                        }

                        //employee parameter for report
                        empolyee = Empl.FirstAndLastName;

                        //fromDate parameter for report
                        if (tbFromDate.Text != "")
                        {

                            start_time = tbFromDate.Text;
                        }
                        //toDate parameter for report
                        if (tbToDate.Text != "")
                        {

                            end_time = tbToDate.Text;
                        }
                        DataTable dataTableMontly = new DataTable("io_pairs_monthly_pt");
                        dataTableMontly.Columns.Add("total", typeof(System.Double));
                        dataTableMontly.Columns.Add("month", typeof(System.String));
                        dataTableMontly.Columns.Add("pass_type", typeof(System.String));
                        dataSetCR.Tables.Add(dataTableMontly);
                        foreach (string monthInt in listMonth)
                        {
                            CalcMontlyTotals(ioPairs, monthInt, dataTableMontly);
                        }
                        //send to session parameters for report
                        Session["TLDetailedDataPage.pass_type"] = pass_type;
                        Session["TLDetailedDataPage.employee"] = empolyee;
                        Session["TLDetailedDataPage.start_time"] = start_time;
                        Session["TLDetailedDataPage.end_time"] = end_time;
                        Session["TLDetailedDataPage.io_pairs"] = dataSetCR;

                        Session[Constants.sessionReportName] = rm.GetString("lblPassesReport", culture);

                        //check language and redirect to apropriate report
                        string reportURL = "";
                        if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                            reportURL = "/ACTAWeb/ReportsWeb/sr/TLDetailedDataReport_sr.aspx";

                        else reportURL = "/ACTAWeb/ReportsWeb/en/TLDetailedDataReport_en.aspx";
                        Response.Redirect("/ACTAWeb/ReportsWeb/ReportPage.aspx?Back=/ACTAWeb/ACTAWebUI/WCDetailsPage.aspx&Report=" + reportURL.Trim() + "&emplID=" + getEmployee().EmployeeID, false);
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCDetailsPage.btnReport_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCDetailsPage.aspx?emplID=" + Empl.EmployeeID, false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void CalcMontlyTotals(DataTable ioPairs, string month, DataTable tableMonthly)
        {
            try
            {
                Dictionary<string, TimeSpan> typesCounter = new Dictionary<string, TimeSpan>();
                Dictionary<int, PassTypeTO> passTypes = new Dictionary<int, PassTypeTO>();
                foreach (PassTypeTO passT in getTypes(getEmployee()))
                {
                    passTypes.Add(passT.PassTypeID, passT);
                }

                foreach (DataRow ioPair in ioPairs.Rows)
                {
                    DateTime dtime = CommonWeb.Misc.createDate(ioPair[1].ToString() + ".");
                    //row["month"] = new DateTime(dtime.Year, dtime.Month, 1);
                    if (dtime.Month + "." + dtime.Year == month)
                    {
                        DateTime start = DateTime.Parse(ioPair["start_time"].ToString());
                        DateTime end = DateTime.Parse(ioPair["end_time"].ToString());
                        string desc = ioPair[4].ToString();
                        if (!start.Equals(new DateTime()) && !end.Equals(new DateTime()))
                        {
                            foreach (KeyValuePair<int, PassTypeTO> pair in passTypes)
                            {
                                if (desc == pair.Value.DescriptionAndID || desc == pair.Value.DescriptionAltAndID)
                                {
                                    TimeSpan duration = end.TimeOfDay.Subtract(start.TimeOfDay);
                                    if (end.TimeOfDay.Hours == 23 && end.TimeOfDay.Minutes == 59)
                                        duration = duration.Add(new TimeSpan(0, 1, 0)); // add one minute if it is last interval from third shift

                                    if (typesCounter.ContainsKey(desc))
                                    {
                                        typesCounter[desc] = typesCounter[desc].Add(duration);
                                        break;
                                    }
                                    else
                                    {
                                        typesCounter.Add(desc, duration);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (string ptID in typesCounter.Keys)
                {
                    DataRow row = tableMonthly.NewRow();
                    row["month"] = month;
                    row["pass_type"] = ptID;
                    string hours = ((int)typesCounter[ptID].TotalHours).ToString().Trim();
                    string minutes = ((((int)typesCounter[ptID].TotalMinutes - (int)typesCounter[ptID].TotalHours * 60) * 100) / 60).ToString();
                    if (minutes == "0")
                    {
                        row["total"] = hours;
                    }
                    else if (minutes.EndsWith("0"))
                    {
                        minutes = minutes.Remove(minutes.LastIndexOf('0'));
                        row["total"] = hours + "." + minutes;
                    }
                    else
                    {
                        row["total"] = hours + "." + minutes;
                    }
                    tableMonthly.Rows.Add(row);
                    tableMonthly.AcceptChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
                
        private List<PassTypeTO> getTypes(EmployeeTO Empl)
        {
            try
            {
                WorkingUnitTO wu = new WorkingUnit(Session[Constants.sessionConnection]).FindWU(Empl.WorkingUnitID);

                Dictionary<int, WorkingUnitTO> WUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                int company = Common.Misc.getRootWorkingUnit(wu.WorkingUnitID, WUnits);
                bool isAltLang = !CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper());

                List<PassTypeTO> passTypeList = new PassType(Session[Constants.sessionConnection]).SearchForCompany(company, isAltLang);
                lbPassTypes.DataSource = passTypeList;

                if (!isAltLang)
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

                return passTypeList;
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

        private void InitializeSQLParameters()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCDetailsPage).Assembly);

                Session[Constants.sessionHeader] = rm.GetString("hdrEmployee", culture) + "," + rm.GetString("hdrDate", culture) + "," + rm.GetString("hdrStartTime", culture) + "," + rm.GetString("hdrEndTime", culture) + "," + rm.GetString("hdrPassType", culture) + "," + rm.GetString("hdrTotal", culture) + ","
                     + rm.GetString("hdrDesc", culture);
                string fields = "e.first_name +' '+ e.last_name AS first_name| convert(varchar,io_pair_date,104) AS date| convert(time,io.start_time) AS start_time| convert(time,io.end_time) AS end_time|";
                if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                {
                    fields = fields + "'('+convert(Varchar,pt.payment_code)+') '+ pt.description ";
                }
                else
                {
                    fields = fields + "'('+convert(Varchar,pt.payment_code)+') '+pt.description_alternative ";
                }
                string fields2 = "AS pass_type | CONVERT(VARCHAR,end_time-start_time,108) AS total| io.description AS description";
                Session[Constants.sessionFields] = fields + fields2;
                Dictionary<int, int> formating = new Dictionary<int, int>();
                formating.Add(1, (int)Constants.FormatTypes.DateTimeFormat);
                formating.Add(2, (int)Constants.FormatTypes.DateTimeFormat);
                formating.Add(3, (int)Constants.FormatTypes.DateTimeFormat);
                Session[Constants.sessionFieldsFormating] = formating;
                Session[Constants.sessionFieldsFormatedValues] = null;
                Session[Constants.sessionTables] = "io_pairs_processed io, employees e, pass_types pt";
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

                Session[Constants.sessionItemsColors] = null;                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnShow_Click(Object sender, EventArgs e)
        {
            EmployeeTO Empl = new EmployeeTO();
            try
            {
                ClearSessionValues();
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCDetailsPage).Assembly);
                Empl = getEmployee();

                CultureInfo ci = CultureInfo.InvariantCulture;
                string filter = "";
                lblError.Text = "";

                //check inserted date
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

                //date validation
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

                //create filter
                if (Session[Constants.sessionDataProvider].ToString().Trim().ToLower().Equals(Constants.dpSqlServer.Trim().ToLower()))
                    filter += "io.start_time >= '" + from.ToString(CommonWeb.Misc.getDateTimeFormatUniversal(), ci) + "' AND ";
                else if (Session[Constants.sessionDataProvider].ToString().Trim().ToLower().Equals(Constants.dpMySql.Trim().ToLower()))
                    filter += "DATE_FORMAT(io.start_time,'%Y-%m-%dT%h:%m:%s') >= '" + from.ToString(CommonWeb.Misc.getDateTimeFormatUniversal(), ci).Trim() + "' AND ";

                if (Session[Constants.sessionDataProvider].ToString().Trim().ToLower().Equals(Constants.dpSqlServer.Trim().ToLower()))
                    filter += "io.end_time <= '" + to.ToString(CommonWeb.Misc.getDateTimeFormatUniversal(), ci) + "' AND ";
                else if (Session[Constants.sessionDataProvider].ToString().Trim().ToLower().Equals(Constants.dpMySql.Trim().ToLower()))
                    filter += "DATE_FORMAT(io.end_time,'%Y-%m-%dT%h:%m:%s') <= '" + to.ToString(CommonWeb.Misc.getDateTimeFormatUniversal(), ci).Trim() + "' AND ";

                //filter for gates
                int[] selGateIndexes = lbPassTypes.GetSelectedIndices();
                if (selGateIndexes.Length > 0)
                {
                    string pass_types = "";
                    foreach (int index in selGateIndexes)
                    {
                        pass_types += lbPassTypes.Items[index].Value.Trim() + ",";
                    }

                    if (pass_types.Length > 0)
                        pass_types = pass_types.Substring(0, pass_types.Length - 1);

                    filter += "io.pass_type_id IN (" + pass_types.Trim() + ") AND ";
                }
                //filter for empolyees
                string emplIDs = Empl.EmployeeID.ToString();


                filter += "io.employee_id =" + emplIDs + " AND ";

                filter += "io.employee_id=e.employee_id AND io.pass_type_id=pt.pass_type_id";

                Session[Constants.sessionFilter] = filter;
                Session[Constants.sessionSortCol] = "io.io_pair_date";
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCDetailsPage.btnShow_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCDetailsPage.aspx?emplID=" + Empl.EmployeeID, false);
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

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "WCDetailsPage.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "WCDetailsPage.", filterState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
